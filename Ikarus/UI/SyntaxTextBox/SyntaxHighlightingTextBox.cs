using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace UrielGuy.SyntaxHighlightingTextBox
{
    /// <summary>
    /// A textbox the does syntax highlighting.
    /// </summary>
    public class SyntaxHighlightingTextBox : System.Windows.Forms.RichTextBox
    {
        public const int maxHighlightTextLength = 65535;
        public const int maxAutoHighlightTextLength = 16384;

        #region Members

        //Members exposed via properties
        private SeperaratorCollection mSeperators = new SeperaratorCollection();
        private HighLightDescriptorCollection mHighlightDescriptors = new HighLightDescriptorCollection();
        private bool mCaseSensitive = false;
        private bool mFilterAutoComplete = true;
        private bool mEnableHighlighting = true;
        private int mMaxUndoRedoSteps = 50;

        //Internal use members
        //private bool mAutoCompleteShown = false;
        private bool mParsing = false;
        private bool mIgnoreLostFocus = false;

        //private AutoCompleteForm mAutoCompleteForm = new AutoCompleteForm();

        //Undo/Redo members
        private ArrayList mUndoList = new ArrayList();
        private Stack mRedoStack = new Stack();
        private bool mIsUndo = false;
        private UndoRedoInfo mLastInfo = new UndoRedoInfo("", new Win32.POINT(), 0);

        #endregion

        #region Properties
        /// <summary>
        /// Determines if token recognition is case sensitive.
        /// </summary>
        [Category("Behavior")]
        [DefaultValue(false)]
        public bool CaseSensitive
        {
            get
            {
                return mCaseSensitive;
            }
            set
            {
                mCaseSensitive = value;
            }
        }


        /// <summary>
        /// Sets whether or not to remove items from the Autocomplete window as the user types...
        /// </summary>
        [Category("Behavior")]
        [DefaultValue(true)]
        public bool FilterAutoComplete
        {
            get
            {
                return mFilterAutoComplete;
            }
            set
            {
                mFilterAutoComplete = value;
            }
        }

        /// <summary>
        /// Set the maximum amount of Undo/Redo steps.
        /// </summary>
        [Category("Behavior")]
        [DefaultValue(50)]
        public int MaxUndoRedoSteps
        {
            get
            {
                return mMaxUndoRedoSteps;
            }
            set
            {
                mMaxUndoRedoSteps = value;
            }
        }

        /// <summary>
        /// A collection of charecters. a token is every string between two seperators.
        /// </summary>
        [Category("Behavior")]
        public SeperaratorCollection Seperators
        {
            get
            {
                return mSeperators;
            }
        }

        /// <summary>
        /// The collection of highlight descriptors.
        /// </summary>
        /// 
        public HighLightDescriptorCollection HighlightDescriptors
        {
            get
            {
                return mHighlightDescriptors;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [enable highlihting].
        /// </summary>
        /// <value><c>true</c> if [enable highlihting]; otherwise, <c>false</c>.</value>
        [DefaultValue(true)]
        public bool EnableHighlighting
        {
            get { return mEnableHighlighting; }
            set
            {
                mEnableHighlighting = value;
                if (value) Highlight();
                else UnHighlight();
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [enable auto complete form].
        /// </summary>
        /// <value>
        /// 	<c>true</c> if [enable auto complete form]; otherwise, <c>false</c>.
        /// </value>
        //[DefaultValue(true)]
        //public bool EnableAutoCompleteForm
        //{
        //    get { return mEnableAutoCompleteForm; }
        //    set { mEnableAutoCompleteForm = value; }
        //}
        #endregion

        #region Overriden methods

        /// <summary>
        /// The on text changed overrided. Here we parse the text into RTF for the highlighting.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnTextChanged(EventArgs e)
        {
            if (mParsing) return;

            if (!mIsUndo)
            {
                mRedoStack.Clear();
                mUndoList.Insert(0, mLastInfo);
                this.LimitUndo();
                mLastInfo = new UndoRedoInfo(Text, GetScrollPos(), SelectionStart);
            }

            if (mEnableHighlighting && Text.Length < maxAutoHighlightTextLength)
                Highlight();

            //if (mAutoCompleteShown)
            //{
            //    if (mFilterAutoComplete)
            //    {
            //        SetAutoCompleteItems();
            //        SetAutoCompleteSize();
            //        SetAutoCompleteLocation(false);
            //    }
            //    SetBestSelectedAutoCompleteItem();
            //}

            base.OnTextChanged(e);
        }

        public void UnHighlight()
        {
            if (Text.Length > maxHighlightTextLength) return;

            mParsing = true;

            //Store cursor and scrollbars location
            Win32.LockWindowUpdate(Handle);
            Win32.POINT scrollPos = GetScrollPos();
            int cursorLoc = SelectionStart;

            //Clear formatting
            string txt = Text;
            Clear();
            SelectionColor = ForeColor;
            SelectionFont = Font;
            Text = txt;

            //Restore cursor and scrollbars location
            SelectionStart = cursorLoc;
            SetScrollPos(scrollPos);
            Win32.LockWindowUpdate((IntPtr)0);
            Invalidate();

            mParsing = false;
        }

        StringBuilder m_FontHeader = new StringBuilder();
        RTFFontStyles m_FontStyles = new RTFFontStyles();
        public void Highlight()
        {
            if (Text.Length > maxHighlightTextLength) return;
            mParsing = true;
            m_FontHeader.Length = 0;
            m_FontStyles.Clear();
            //Store cursor and scrollbars location
            Win32.LockWindowUpdate(Handle);
            Win32.POINT scrollPos = GetScrollPos();
            int cursorLoc = SelectionStart;
            StringBuilder sbBody = new StringBuilder();
            try // nenecham to kvuli blbemu zvyraznovani spadnut, neee?
            {
                //Font table creation
                FontNameIndex fonts = new FontNameIndex();
                //Adding RTF header - FIX
                m_FontHeader.Append(@"{\rtf1\fbidis\ansi\ansicpg1255\deff0\deflang1037");

                Hashtable colors = AddColorTable(m_FontHeader);

                m_FontHeader.Append("\n{\\fonttbl");
                //sb.Append(@"{\rtf1\fbidis\ansi\ansicpg1255\deff0\deflang1037{\fonttbl{");

                //we do not need the counter, the Count-property of Font-index will work.
                //int fontCounter = 0;
                //AddFontToTable(sb, Font, ref fontCounter, fonts);
                //Add default font.
                AddFontToTable(m_FontHeader, Font, fonts);
                //this adds the defaultfont to the style definitions
                m_FontStyles.GetFontStyle(Font);
                //Tweak: Only load fonts that are used...
                //foreach (HighlightDescriptor hd in mHighlightDescriptors)
                //{
                //    if (hd.Font != null)
                //    {
                //        if (!fonts.ContainsKey(hd.Font))
                //        {
                //            AddFontToTable(sb, hd.Font, ref fontCounter, fonts);
                //        }
                //    }
                //}           

                //Do not close the header, we'll do that after all the fonts are added.
                // sbHeader.Append("}\n");

                //Parsing text               
                sbBody.Append(@"\viewkind4\uc1\pard\ltrpar").Append('\n');
                //this is why we added the default font allready
                SetDefaultSettings(sbBody, colors, fonts);

                m_SeperatorCharArray = mSeperators.GetAsCharArray();
                //Replacing "\" to "\\" for RTF...
                string sCurrentText = Text;
                string[] lines = sCurrentText.Replace("\\", "\\\\").Replace("{", "\\{").Replace("}", "\\}").Split('\n');

                //will be used to determine the text to be formatted
                StringBuilder sbSubText = new StringBuilder();
                #region RtfBodyGeneration
                #region for (int lineCounter = 0; lineCounter < lines.Length; lineCounter++)
                for (int lineCounter = 0; lineCounter < lines.Length; lineCounter++)
                {
                    if (lineCounter != 0)
                    {
                        AddNewLine(sbBody);
                    }
                    string line = lines[lineCounter];
                    string[] tokens = mCaseSensitive ? line.Split(m_SeperatorCharArray) : line.ToUpper().Split(m_SeperatorCharArray);
                    if (tokens.Length == 0)
                    {
                        AddUnicode(sbBody, line);
                        AddNewLine(sbBody);
                        continue;
                    }

                    int tokenCounter = 0;
                    #region for (int i = 0; i < line.Length; )
                    for (int i = 0; i < line.Length; )
                    {
                        char curChar = line[i];
                        if (mSeperators.Contains(curChar))
                        {
                            sbBody.Append(curChar);
                            i++;
                        }
                        else
                        {
                            if (tokenCounter >= tokens.Length) break;
                            string curToken = tokens[tokenCounter++];
                            bool bAddToken = true;

                            #region foreach (HighlightDescriptor hd in mHighlightDescriptors)
                            foreach (HighlightDescriptor hd in mHighlightDescriptors)
                            {
                                string compareStr = mCaseSensitive ? hd.Token : hd.Token.ToUpper();
                                bool match = false;

                                //Check if the highlight descriptor matches the current toker according to the DescriptoRecognision property.
                                #region switch (hd.DescriptorType)
                                switch (hd.DescriptorRecognition)
                                {
                                    case DescriptorRecognition.RegEx:
                                        continue;
                                    case DescriptorRecognition.WholeWord:
                                        if (curToken == compareStr)
                                        {
                                            match = true;
                                        }
                                        break;
                                    case DescriptorRecognition.StartsWith:
                                        if (curToken.StartsWith(compareStr))
                                        {
                                            match = true;
                                        }
                                        break;
                                    case DescriptorRecognition.Contains:
                                        if (curToken.IndexOf(compareStr) != -1)
                                        {
                                            match = true;
                                        }
                                        break;
                                }
                                if (!match)
                                {
                                    //If this token doesn't match chech the next one.
                                    continue;
                                }
                                #endregion switch (hd.DescriptorType)

                                //printing this token will be handled by the inner code, don't apply default settings...
                                bAddToken = false;

                                //Set colors to current descriptor settings.
                                //Open a "block", this we will close after adding the text to the body
                                sbBody.Append('{');
                                SetDescriptorSettings(sbBody, hd, colors, fonts);
                                //Improvement for readability instead of formatting the text in the 
                                //switch, just determine the text to be formatted, and format it after the swich.
                                //the result is just one call to SetDefaultSettings, which is encsulated in FormatText
                                //for better font support (another improvement).
                                string sSubText = "";
                                //Print text affected by this descriptor.
                                #region switch (hd.DescriptorType)
                                switch (hd.DescriptorType)
                                {
                                    case DescriptorType.Word:
                                        //Will be added to the rtf after  the switch
                                        sSubText = line.Substring(i, curToken.Length);
                                        i += curToken.Length;
                                        break;
                                    case DescriptorType.ToEOW:
                                        int dummy;
                                        sSubText = GetWordBounsOnCharIndex(line, i, out dummy, out i);
                                        break;
                                    case DescriptorType.ToEOL:
                                        //Will be added to the rtf after  the switch
                                        sSubText = line.Remove(0, i);
                                        i = line.Length;
                                        break;
                                    case DescriptorType.ToCloseToken:
                                        {
                                            //we have multiple itterations, so clear it first:
                                            sbSubText.Length = 0;
                                            //determine endtoken, add all encountered text to subtext
                                            int closeStart = i + hd.Token.Length;
                                            while ((line.IndexOf(hd.CloseToken, closeStart) == -1) && (lineCounter < lines.Length))
                                            {
                                                //Will be added to the rtf after  the switch
                                                sbSubText.Append(line.Remove(0, i));
                                                lineCounter++;
                                                if (lineCounter < lines.Length)
                                                {
                                                    AddNewLine(sbSubText);
                                                    line = lines[lineCounter];
                                                    i = closeStart = 0;
                                                }
                                                else
                                                    i = closeStart = line.Length;
                                            }
                                            int closeIndex = line.IndexOf(hd.CloseToken, closeStart);
                                            if (closeIndex != -1)
                                            {
                                                //Will be added to the rtf after  the switch
                                                sbSubText.Append(line.Substring(i, closeIndex + hd.CloseToken.Length - i));
                                                line = line.Remove(0, closeIndex + hd.CloseToken.Length);
                                                tokenCounter = 0;
                                                tokens = mCaseSensitive ? line.Split(m_SeperatorCharArray) : line.ToUpper().Split(m_SeperatorCharArray);
                                                i = 0;
                                            }
                                            sSubText = sbSubText.ToString();
                                        }
                                        break;
                                }
                                #endregion switch (hd.DescriptorType)
                                //now that sSubText has a value (what do we want to format), format it.
                                //Add the text to the RTF
                                AddUnicode(sbBody, sSubText);
                                //Close the "block" since the text we wanted to format, is now in the body.
                                sbBody.Append('}');
                                //this ends all the styles that were applied in the SetDescriptorSettings, 
                                //returning all formatting to the default
                                SetDefaultSettings(sbBody, colors, fonts);
                                break;
                            }
                            #endregion foreach (HighlightDescriptor hd in mHighlightDescriptors)
                            if (bAddToken)
                            {
                                //Print text with default settings...
                                AddUnicode(sbBody, (line.Substring(i, curToken.Length)));
                                i += curToken.Length;
                            }
                        }
                    }
                    #endregion for (int i = 0; i < line.Length; )
                }
                #endregion for (int lineCounter = 0; lineCounter < lines.Length; lineCounter++)
                #endregion
                //we might have added fonts to the header, which means the header is not closed yet. close it:
                m_FontHeader.Append("\n}\n");
                ApplyRTF(m_FontHeader, sbBody);
                PerformRegEx(sCurrentText);
            }
#if DEBUG
            catch (Exception exc)
            {
                System.Diagnostics.Debug.Fail(exc.Message, exc.StackTrace);
            }
#else
			catch { }
#endif
            //Restore cursor and scrollbars location.
            SelectionStart = cursorLoc;
            SelectionLength = 0;
            SetScrollPos(scrollPos);
            Win32.LockWindowUpdate((IntPtr)0);
            Invalidate();
            mParsing = false;
        }

        private void PerformRegEx(string sCurrentText)
        {
            #region RegEx
            foreach (HighlightDescriptor hd in mHighlightDescriptors)
            {
                if (hd.DescriptorRecognition == DescriptorRecognition.RegEx)
                {
                    Match regMatch;
                    Regex regKeywords = null;
                    //Only build the regular expresion once, try to retrieve from cache
                    if (!m_Expressions.TryGetValue(hd.Token, out regKeywords))
                    {
                        if (false)
                            regKeywords = new Regex(hd.Token, RegexOptions.IgnoreCase | RegexOptions.Compiled);
                        else
                            regKeywords = new Regex(hd.Token, RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.Multiline);
                        m_Expressions.Add(hd.Token, regKeywords);
                    }

                    //Enumerate all matches
                    for (regMatch = regKeywords.Match(sCurrentText); regMatch.Success; regMatch = regMatch.NextMatch())
                    {
                        //Create a selection, needed for the highlighting
                        SelectionStart = regMatch.Index;
                        SelectionLength = regMatch.Length;

                        //dont overwerite color if tranparent
                        if (hd.Color != Color.Transparent)
                            SelectionColor = hd.Color;
                        SelectionFont = hd.Font;
                    }
                }
            }
            #endregion RegEx
        }
        Dictionary<string, Regex> m_Expressions = new Dictionary<string, Regex>();

        /// <summary>
        /// Adds the color table.
        /// </summary>
        /// <param name="sbBody">The sb body.</param>
        /// <returns></returns>
        private Hashtable AddColorTable(StringBuilder sbBody)
        {
            //ColorTable
            sbBody.Append(@"{\colortbl ;");
            Hashtable colors = new Hashtable();
            int colorCounter = 1;
            AddColorToTable(sbBody, ForeColor, ref colorCounter, colors);
            AddColorToTable(sbBody, BackColor, ref colorCounter, colors);
            foreach (HighlightDescriptor hd in mHighlightDescriptors)
            {
                if (!colors.ContainsKey(hd.Color))
                {
                    AddColorToTable(sbBody, hd.Color, ref colorCounter, colors);
                }
            }
            sbBody.Append("}\n");
            return colors;
        }

        /// <summary>
        /// Applies the RTF.
        /// </summary>
        /// <param name="sbHeader">The sb header.</param>
        /// <param name="sbBody">The sb body.</param>
        private void ApplyRTF(StringBuilder sbHeader, StringBuilder sbBody)
        {
            //Now create the RTF from header and body.
            string sCompositeRTF = sbHeader.ToString() + sbBody.ToString();
            Rtf = sCompositeRTF;
        }




        #endregion
        #region Rtf building helper functions
        /// <summary>
        /// Sets the font to the specified font.
        /// </summary>
        private void SetFont(StringBuilder sb, Font pHighlightingFont, FontNameIndex fonts)
        {
            if (pHighlightingFont == null) return;

            int iFontIndex = fonts.Count;
            //tweak, only add fonts that are used.
            if (!fonts.TryGetValue(pHighlightingFont.Name, out iFontIndex))
            {
                iFontIndex = fonts.Count;
                AddFontToTable(m_FontHeader, pHighlightingFont, fonts);
            }
            sb.Append(@"\f").Append(iFontIndex).Append("{");
            sb.Append(this.m_FontStyles.GetFontStyle(pHighlightingFont));
        }

        /// <summary>
        /// Adds a font to the RTF's font table and to the fonts hashtable.
        /// </summary>
        /// <param name="sb">The RTF's string builder</param>
        /// <param name="font">the Font to add</param>
        /// <param name="counter">a counter, containing the amount of fonts in the table</param>
        /// <param name="fonts">an hashtable. the key is the font's name. the value is it's index in the table</param>
        //private void AddFontToTable(StringBuilder sb, Font font, ref int counter, FontIndex fonts)
        private void AddFontToTable(StringBuilder sbHeader, Font font, FontNameIndex fonts)
        {
            //fix
            //  sb.Append(@"{\f").Append(counter).Append(@"\fnil\fcharset0").Append(font.Name).Append(";}");
            //sb.Append(@"\f").Append(counter).Append(@"\fnil\fcharset0").Append(font.Name).Append(";}");
            int iFountCount = fonts.Count;
            sbHeader.Append("\r\n  ");
            sbHeader.Append(@"{\f").Append(iFountCount).Append(@"\fnil");
            sbHeader.Append(@"\fcharset0 ").Append(font.Name).Append(";}");
            fonts.Add(font.Name, iFountCount);
            //fonts.Add(font, counter++);
        }

        /// <summary>
        /// Set color and font to default control settings.
        /// </summary>
        /// <param name="sb">the string builder building the RTF</param>
        /// <param name="colors">colors hashtable</param>
        /// <param name="fonts">fonts hashtable</param>
        private void SetDefaultSettings(StringBuilder sb, Hashtable colors, FontNameIndex fonts)
        {
            SetFont(sb, Font, fonts);
            SetColor(sb, ForeColor, colors);
            SetFontSize(sb, (int)Font.Size);
            EndTags(sb);
        }

        /// <summary>
        /// Set Color and font to a highlight descriptor settings.
        /// </summary>
        /// <param name="sb">the string builder building the RTF</param>
        /// <param name="hd">the HighlightDescriptor with the font and color settings to apply.</param>
        /// <param name="colors">colors hashtable</param>
        /// <param name="fonts">fonts hashtable</param>
        private void SetDescriptorSettings(StringBuilder sb, HighlightDescriptor hd, Hashtable colors, FontNameIndex fonts)
        {
            if (hd.Color != Color.Transparent)
                SetColor(sb, hd.Color, colors);
            Font aFont = hd.Font;
            if (aFont != null)
            {
                SetFont(sb, aFont, fonts);
                SetFontSize(sb, (int)aFont.Size);
                sb.Append(m_FontStyles.GetFontStyle(aFont));
            }
            EndTags(sb);
        }
        /// <summary>
        /// Sets the color to the specified color
        /// </summary>
        private void SetColor(StringBuilder sb, Color color, Hashtable colors)
        {
            sb.Append(@"\cf").Append(colors[color]);
        }
        /// <summary>
        /// Sets the backgroung color to the specified color.
        /// </summary>
        private void SetBackColor(StringBuilder sb, Color color, Hashtable colors)
        {
            sb.Append(@"\cb").Append(colors[color]);
        }
        /// <summary>
        /// Sets the font size to the specified font size.
        /// </summary>
        private void SetFontSize(StringBuilder sb, int size)
        {
            sb.Append(@"\fs").Append(size * 2);
        }
        /// <summary>
        /// Adds a newLine mark to the RTF.
        /// </summary>
        private void AddNewLine(StringBuilder sb)
        {
            sb.Append("\\par\n");
        }

        /// <summary>
        /// Adds the unicode.
        /// </summary>
        /// <param name="sb">The sb.</param>
        /// <param name="toAdd">To add.</param>
        private void AddUnicode(StringBuilder sb, string toAdd)
        {
            foreach (char c in toAdd)
            {
                if (c < 128) sb.Append(c);
                else
                {
                    sb.Append(@"\u" + ((int)c).ToString() + "?");
                }
            }
        }

        /// <summary>
        /// Ends a RTF tags section.
        /// </summary>
        private void EndTags(StringBuilder sb)
        {
            sb.Append(' ');
        }



        /// <summary>
        /// Adds a color to the RTF's color table and to the colors hashtable.
        /// </summary>
        /// <param name="sb">The RTF's string builder</param>
        /// <param name="color">the color to add</param>
        /// <param name="counter">a counter, containing the amount of colors in the table</param>
        /// <param name="colors">an hashtable. the key is the color. the value is it's index in the table</param>
        private void AddColorToTable(StringBuilder sb, Color color, ref int counter, Hashtable colors)
        {
            sb.Append(@"\r\n\red").Append(color.R).Append(@"\green").Append(color.G).Append(@"\blue").Append(color.B).Append(";");
            colors.Add(color, counter++);
        }

        #endregion

        #region Undo/Redo Code
        public new bool CanUndo
        {
            get
            {
                return mUndoList.Count > 0;
            }
        }
        public new bool CanRedo
        {
            get
            {
                return mRedoStack.Count > 0;
            }
        }

        private void LimitUndo()
        {
            while (mUndoList.Count > mMaxUndoRedoSteps)
            {
                mUndoList.RemoveAt(mMaxUndoRedoSteps);
            }
        }

        public new void Undo()
        {
            if (!CanUndo) return;
            mIsUndo = true;
            mRedoStack.Push(new UndoRedoInfo(Text, GetScrollPos(), SelectionStart));
            UndoRedoInfo info = (UndoRedoInfo)mUndoList[0];
            mUndoList.RemoveAt(0);
            Text = info.Text;
            SelectionStart = info.CursorLocation;
            SetScrollPos(info.ScrollPos);
            mLastInfo = info;
            mIsUndo = false;
        }

        public new void Redo()
        {
            if (!CanRedo) return;
            mIsUndo = true;
            mUndoList.Insert(0, new UndoRedoInfo(Text, GetScrollPos(), SelectionStart));
            LimitUndo();
            UndoRedoInfo info = (UndoRedoInfo)mRedoStack.Pop();
            Text = info.Text;
            SelectionStart = info.CursorLocation;
            SetScrollPos(info.ScrollPos);
            mIsUndo = false;
        }

        private class UndoRedoInfo
        {
            public UndoRedoInfo(string text, Win32.POINT scrollPos, int cursorLoc)
            {
                Text = text;
                ScrollPos = scrollPos;
                CursorLocation = cursorLoc;
            }
            public readonly Win32.POINT ScrollPos;
            public readonly int CursorLocation;
            public readonly string Text;
        }
        #endregion

        #region AutoComplete functions
        /// <summary>
        /// (re-) Created in each  call to HighLight.
        /// </summary>
        char[] m_SeperatorCharArray = new char[] { };

        /// <summary>
        /// Entry point to autocomplete mechanism.
        /// Tries to complete the current word. if it fails it shows the AutoComplete form.
        /// </summary>
        private void CompleteWord()
        {
            int curTokenStartIndex;
            int curTokenEndIndex;
            string curTokenString = GetSelectedWordBounds(Text, out curTokenStartIndex, out curTokenEndIndex);

            string token = null;
            foreach (HighlightDescriptor hd in mHighlightDescriptors)
            {
                if (hd.UseForAutoComplete && hd.Token.ToUpper().StartsWith(curTokenString))
                {
                    if (token == null)
                    {
                        token = hd.Token;
                    }
                    else
                    {
                        token = null;
                        break;
                    }
                }
            }
            if (token == null)
            {
                //if (mEnableAutoCompleteForm) ShowAutoComplete();
            }
            else
            {
                ReplaceText(curTokenStartIndex, curTokenEndIndex, token);
            }
        }


        private string GetSelectedWordBounds(string sText, out int curTokenStartIndex, out int curTokenEndIndex)
        {
            //Each call to the textproperty will eventually lead to a call to user32.dll.GetWindowText.

            // int curTokenStartIndex = sText.LastIndexOfAny(aSeperators, Math.Min(SelectionStart, sText.Length - 1)) + 1;
            //If we want to find the start of the current token out cursor is on, we need to search 'Left' of the cursor:
            //that would be SelectionStart-1
            int iWordBoundary = SelectionStart - 1;
            return GetWordBounsOnCharIndex(sText, iWordBoundary, out curTokenStartIndex, out curTokenEndIndex);
        }

        private string GetWordBounsOnCharIndex(string sText, int pCharIndex, out int curTokenStartIndex, out int curTokenEndIndex)
        {
            curTokenStartIndex = sText.LastIndexOfAny(m_SeperatorCharArray, Math.Min(pCharIndex, sText.Length - 1)) + 1;

            //Now that we know where it starts, where does it end
            curTokenEndIndex = sText.IndexOfAny(m_SeperatorCharArray, curTokenStartIndex);
            //No end found, select all
            if (curTokenEndIndex == -1)
                curTokenEndIndex = sText.Length;
            //now determin the selected word or token
            return sText.Substring(curTokenStartIndex, Math.Max(curTokenEndIndex - curTokenStartIndex, 0)).ToUpper();
        }
        /// <summary>
        /// Replaces the text.
        /// </summary>
        /// <param name="curTokenStartIndex">Start index of the cur token.</param>
        /// <param name="curTokenEndIndex">End index of the cur token.</param>
        /// <param name="token">The token.</param>
        private void ReplaceText(int curTokenStartIndex, int curTokenEndIndex, string token)
        {
            SelectionStart = curTokenStartIndex;
            SelectionLength = curTokenEndIndex - curTokenStartIndex;
            SelectedText = token;
            SelectionStart = SelectionStart + SelectionLength;
            SelectionLength = 0;
        }


        /// <summary>
        /// replace the current word of the cursor with the one from the AutoComplete form and closes it.
        /// </summary>
        /// <returns>If the operation was succesful</returns>
        //private bool AcceptAutoCompleteItem()
        //{
        //    //if (mAutoCompleteForm.SelectedItem == null)
        //    {
        //        return false;
        //    }

        //    int curTokenStartIndex;
        //    int curTokenEndIndex;
        //    string curTokenString = GetSelectedWordBounds(Text, out curTokenStartIndex, out curTokenEndIndex);
        //    ReplaceText(curTokenStartIndex, curTokenEndIndex, mAutoCompleteForm.SelectedItem);
        //    HideAutoCompleteForm();
        //    return true;
        //}

        /// <summary>
        /// Finds the and sets the best matching token as the selected item in the AutoCompleteForm.
        /// </summary>
        //private void SetBestSelectedAutoCompleteItem()
        //{
        //    int curTokenStartIndex;
        //    int curTokenEndIndex;
        //    string curTokenString = GetSelectedWordBounds(Text, out curTokenStartIndex, out curTokenEndIndex);

        //    if ((mAutoCompleteForm.SelectedItem != null) &&
        //        mAutoCompleteForm.SelectedItem.ToUpper().StartsWith(curTokenString))
        //    {
        //        return;
        //    }

        //    int matchingChars = -1;
        //    string bestMatchingToken = null;

        //    foreach (string item in mAutoCompleteForm.Items)
        //    {
        //        bool isWholeItemMatching = true;
        //        for (int i = 0; i < Math.Min(item.Length, curTokenString.Length); i++)
        //        {
        //            if (char.ToUpper(item[i]) != char.ToUpper(curTokenString[i]))
        //            {
        //                isWholeItemMatching = false;
        //                if (i - 1 > matchingChars)
        //                {
        //                    matchingChars = i;
        //                    bestMatchingToken = item;
        //                    break;
        //                }
        //            }
        //        }
        //        if (isWholeItemMatching &&
        //            (Math.Min(item.Length, curTokenString.Length) > matchingChars))
        //        {
        //            matchingChars = Math.Min(item.Length, curTokenString.Length);
        //            bestMatchingToken = item;
        //        }
        //    }

        //    if (bestMatchingToken != null)
        //    {
        //        mAutoCompleteForm.SelectedIndex = mAutoCompleteForm.Items.IndexOf(bestMatchingToken);
        //    }
        //}

        /// <summary>
        /// Sets the items for the AutoComplete form.
        /// </summary>
        //private void SetAutoCompleteItems()
        //{
        //    mAutoCompleteForm.Items.Clear();
        //    string filterString = "";
        //    if (mFilterAutoComplete)
        //    {
        //        int curTokenStartIndex;
        //        int curTokenEndIndex;
        //        filterString = GetSelectedWordBounds(Text, out curTokenStartIndex, out curTokenEndIndex);
        //    }

        //    foreach (HighlightDescriptor hd in mHighlightDescriptors)
        //    {
        //        if (hd.Token.ToUpper().StartsWith(filterString) && hd.UseForAutoComplete)
        //        {
        //            mAutoCompleteForm.Items.Add(hd.Token);
        //        }
        //    }
        //    mAutoCompleteForm.UpdateView();
        //}

        /// <summary>
        /// Sets the size. the size is limited by the MaxSize property in the form itself.
        /// </summary>
        //private void SetAutoCompleteSize()
        //{
        //    mAutoCompleteForm.Height = Math.Min(
        //        Math.Max(mAutoCompleteForm.Items.Count, 1) * mAutoCompleteForm.ItemHeight + 4,
        //        mAutoCompleteForm.MaximumSize.Height);
        //}

        /// <summary>
        /// closes the AutoCompleteForm.
        /// </summary>
        //private void HideAutoCompleteForm()
        //{
        //    mAutoCompleteForm.Visible = false;
        //    mAutoCompleteShown = false;
        //}

        /// <summary>
        /// Sets the location of the AutoComplete form, maiking sure it's on the screen where the cursor is.
        /// </summary>
        /// <param name="moveHorizontly">determines wheather or not to move the form horizontly.</param>
        //private void SetAutoCompleteLocation(bool moveHorizontly)
        //{
        //    Point cursorLocation = GetPositionFromCharIndex(SelectionStart);
        //    //Screen screen = Screen.FromPoint(cursorLocation);
        //    //make it show op on the proper monitor.
        //    Screen screen = Screen.FromPoint(PointToScreen(cursorLocation)); // FIX
        //    Point optimalLocation = new Point(PointToScreen(cursorLocation).X - 15, (int)(PointToScreen(cursorLocation).Y + Font.Size * 2 + 2));
        //    Rectangle desiredPlace = new Rectangle(optimalLocation, mAutoCompleteForm.Size);
        //    desiredPlace.Width = 152;
        //    if (desiredPlace.Left < screen.Bounds.Left)
        //    {
        //        desiredPlace.X = screen.Bounds.Left;
        //    }
        //    if (desiredPlace.Right > screen.Bounds.Right)
        //    {
        //        desiredPlace.X -= (desiredPlace.Right - screen.Bounds.Right);
        //    }
        //    if (desiredPlace.Bottom > screen.Bounds.Bottom)
        //    {
        //        desiredPlace.Y = cursorLocation.Y - 2 - desiredPlace.Height;
        //    }
        //    if (!moveHorizontly)
        //    {
        //        desiredPlace.X = mAutoCompleteForm.Left;
        //    }

        //    mAutoCompleteForm.Bounds = desiredPlace;
        //}

        /// <summary>
        /// Shows the Autocomplete form.
        /// </summary>
        //public void ShowAutoComplete()
        //{
        //    SetAutoCompleteItems();
        //    SetAutoCompleteSize();
        //    SetAutoCompleteLocation(true);
        //    mIgnoreLostFocus = true;
        //    mAutoCompleteForm.Visible = true;
        //    SetBestSelectedAutoCompleteItem();
        //    mAutoCompleteShown = true;
        //    Focus();
        //    mIgnoreLostFocus = false;
        //}

        #endregion

        #region Scrollbar positions functions



        /// <summary>
        /// Sends a win32 message to get the scrollbars' position.
        /// </summary>
        /// <returns>a POINT structore containing horizontal and vertical scrollbar position.</returns>
        private Win32.POINT GetScrollPos()
        {
            Win32.POINT res = new Win32.POINT();
            Win32.SendMessage(Handle, Win32.EM_GETSCROLLPOS, 0, ref res);
            return res;
        }

        /// <summary>
        /// Sends a win32 message to set scrollbars position.
        /// </summary>
        /// <param name="point">a POINT conatining H/Vscrollbar scrollpos.</param>
        private void SetScrollPos(Win32.POINT point)
        {
            Win32.SendMessage(Handle, Win32.EM_SETSCROLLPOS, 0, ref point);
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.RichTextBox.VScroll"></see> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"></see> that contains the event data.</param>
        protected override void OnVScroll(EventArgs e)
        {
            if (mParsing) return;
            base.OnVScroll(e);
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.MouseDown"></see> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.Forms.MouseEventArgs"></see> that contains the event data.</param>
        protected override void OnMouseDown(MouseEventArgs e)
        {
            //HideAutoCompleteForm();
            base.OnMouseDown(e);


        }

        /// <summary>
        /// Taking care of Keyboard events
        /// </summary>
        /// <param name="m"></param>
        /// <remarks>
        /// Since even when overriding the OnKeyDown methoed and not calling the base function 
        /// you don't have full control of the input, I've decided to catch windows messages to handle them.
        /// </remarks>
        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case Win32.WM_PAINT:
                    {
                        //Don't draw the control while parsing to avoid flicker.
                        if (mParsing)
                        {
                            return;
                        }
                        break;
                    }
                case Win32.WM_KEYDOWN:
                    {
                        //if (mAutoCompleteShown)
                        //{
                        //    switch ((Keys)(int)m.WParam)
                        //    {
                        //        case Keys.Down:
                        //            {
                        //                if (mAutoCompleteForm.Items.Count != 0)
                        //                {
                        //                    mAutoCompleteForm.SelectedIndex = (mAutoCompleteForm.SelectedIndex + 1) % mAutoCompleteForm.Items.Count;
                        //                }
                        //                return;
                        //            }
                        //        case Keys.Up:
                        //            {
                        //                if (mAutoCompleteForm.Items.Count != 0)
                        //                {
                        //                    if (mAutoCompleteForm.SelectedIndex < 1)
                        //                    {
                        //                        mAutoCompleteForm.SelectedIndex = mAutoCompleteForm.Items.Count - 1;
                        //                    }
                        //                    else
                        //                    {
                        //                        mAutoCompleteForm.SelectedIndex--;
                        //                    }
                        //                }
                        //                return;
                        //            }
                        //        case Keys.Enter:
                        //        case Keys.Space:
                        //            {
                        //                AcceptAutoCompleteItem();
                        //                m.Result = IntPtr.Zero;
                        //                return;
                        //            }
                        //        case Keys.Escape:
                        //            {
                        //                HideAutoCompleteForm();
                        //                return;
                        //            }

                        //    }
                        //}
                        //else
                        {
                            //if (((Keys)(int)m.WParam == Keys.Space) &&
                            //    ((Win32.GetKeyState(Win32.VK_CONTROL) & Win32.KS_KEYDOWN) != 0))
                            //{
                            //    CompleteWord();
                            //}
                            //else 
                                if (((Keys)(int)m.WParam == Keys.Z) &&
                                ((Win32.GetKeyState(Win32.VK_CONTROL) & Win32.KS_KEYDOWN) != 0))
                            {
                                Undo();
                                return;
                            }
                            else if (((Keys)(int)m.WParam == Keys.Y) &&
                                ((Win32.GetKeyState(Win32.VK_CONTROL) & Win32.KS_KEYDOWN) != 0))
                            {
                                Redo();
                                return;
                            }
                        }
                        break;
                    }
                case Win32.WM_CHAR:
                    {
                        switch ((Keys)(int)m.WParam)
                        {
                            case Keys.Space:
                                if ((Win32.GetKeyState(Win32.VK_CONTROL) & Win32.KS_KEYDOWN) != 0)
                                {
                                    return;
                                }
                                break;
                        }
                    }
                    break;

            }
            base.WndProc(ref m);
        }


        /// <summary>
        /// Hides the AutoComplete form when losing focus on textbox.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnLostFocus(EventArgs e)
        {
            if (!mIgnoreLostFocus)
            {
                //HideAutoCompleteForm();
            }
            base.OnLostFocus(e);
        }

        #endregion

        /// <summary>
        /// Initializes the component.
        /// </summary>
        private void InitializeComponent()
        {
            this.SuspendLayout();
            this.ResumeLayout(false);

        }

        /// <summary>
        /// Adds a highlight-descriptor.
        /// </summary>
        /// <param name="dr">The dr.</param>
        /// <param name="token">The token.</param>
        /// <param name="descriptorType">Type of the descriptor.</param>
        /// <param name="color">The color.</param>
        /// <param name="font">The font.</param>
        /// <param name="useForAutoComplete">if set to <c>true</c> [use for auto complete].</param>
        public void AddHighlightDescriptor(DescriptorRecognition dr, string token, DescriptorType descriptorType, Color color, Font font, bool useForAutoComplete)
        {
            this.HighlightDescriptors.Add(new HighlightDescriptor(token, color, font, descriptorType, dr, useForAutoComplete));
        }

        /// <summary>
        /// Adds the highlight descriptor.
        /// </summary>
        /// <param name="dr">The dr.</param>
        /// <param name="token">The token.</param>
        /// <param name="descriptorType">Type of the descriptor.</param>
        /// <param name="closeToken">The close token.</param>
        /// <param name="color">The color.</param>
        /// <param name="font">The font.</param>
        /// <param name="useForAutoComplete">if set to <c>true</c> [use for auto complete].</param>
        public void AddHighlightDescriptor(DescriptorRecognition dr, string token, DescriptorType descriptorType, string closeToken, Color color, Font font, bool useForAutoComplete)
        {
            this.HighlightDescriptors.Add(new HighlightDescriptor(token, closeToken, descriptorType, dr, color, font, useForAutoComplete));

        }
    }
}



