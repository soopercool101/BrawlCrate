using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Globalization;
using UrielGuy.SyntaxHighlightingTextBox;
using Ikarus.MovesetFile;
using Ikarus;

namespace System.Windows.Forms
{
    public partial class NewScriptEditor : UserControl
    {
        public int _start = 0;
        public string _buffer = "";

        const int MaxIntelDisplayed = 9;
        const string Characters = "1234567890ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
        static readonly char[] Readable = Characters.ToCharArray();
        private List<string> values;

        void SetSource(List<string> source)
        {
            intelBox.DataSource = source;
            pnlIntel.Height = (intelBox.Items.Count > MaxIntelDisplayed ? MaxIntelDisplayed : intelBox.Items.Count) * intelBox.ItemHeight + 6;

            if (intelBox.Items.Count > 0)
                intelBox.SelectedIndex = 0;
        }

        #region Arrays
        public string[] _highlightWords = 
        {
            "Ref",
            "Req",
            "Enum",
            "File",
            "Script",
            "RA",
            "LA",
            "IC",
        };
        public string[] _highlightWords2 = 
        {
            "true",
            "false",
            "if",
            "else",
            "switch",
            "case",
            "default",
            "default:",
            "break",
            "loop",
        };
        public char[] _separators =
        {
            ' ',
            '\r',
            '\n',
            ',',
            '.',
            ')',
            '(',
            '!',
            ']',
            '[',
            '}',
            '{',
            ';',
            '+',
            '=',
            '\t',
        };
        #endregion

        //Milliseconds the user has to type before errors are highlighted
        const int _errorCheckTime = 2000;
        public NewScriptEditor()
        {
            InitializeComponent();

            values = Manager.Events.Values.Select(x => x._compressedName).ToList();

            SetSource(values);
            textBox.MouseWheel += textBox_MouseWheel;
            textBox.ScrollBars = RichTextBoxScrollBars.Both;

            _errorTimer = new System.Timers.Timer(_errorCheckTime) { AutoReset = false };
            _errorTimer.Elapsed += _errorTimer_Elapsed;
            _bWorker = new BackgroundWorker();
            _bWorker.WorkerReportsProgress = false;
            _bWorker.WorkerSupportsCancellation = true;
            _bWorker.DoWork += _bWorker_DoWork;

            textBox.Seperators.AddRange(_separators);

            Font tmp = new Font(textBox.Font, FontStyle.Bold);

            Color clr = Color.FromArgb(255, 0, 128, 0);
            textBox.AddHighlightDescriptor(DescriptorRecognition.StartsWith, "/*", DescriptorType.ToCloseToken, "*/", clr, null, false);
            textBox.AddHighlightDescriptor(DescriptorRecognition.StartsWith, "//", DescriptorType.ToEOL, clr, null, false);

            clr = Color.FromArgb(255, 43, 145, 175);
            foreach (string n in _highlightWords)
                textBox.AddHighlightDescriptor(DescriptorRecognition.WholeWord, n, DescriptorType.Word, clr, tmp, true);
            clr = Color.FromArgb(255, 0, 0, 255);
            foreach (string n in _highlightWords2)
                textBox.AddHighlightDescriptor(DescriptorRecognition.WholeWord, n, DescriptorType.Word, clr, tmp, true);

            clr = Color.FromArgb(255, 163, 21, 21);
            textBox.AddHighlightDescriptor(DescriptorRecognition.StartsWith, "\"", DescriptorType.ToCloseToken, "\"", clr, null, true);
            textBox.AddHighlightDescriptor(DescriptorRecognition.StartsWith, "\'", DescriptorType.ToCloseToken, "\'", clr, null, true);
            
            string regBase = "b[^ex]*(?:x.[^ex]*)*[e]";
            string sEx = regBase.Replace("e", "\\\"").Replace("b", "\\\"").Replace("x", "\\\\");
            textBox.AddHighlightDescriptor(DescriptorRecognition.RegEx, sEx, DescriptorType.Word, Color.Red, tmp, false);

            clr = Color.FromArgb(255, 128, 0, 0);
            textBox.AddHighlightDescriptor(DescriptorRecognition.RegEx, "\\b(?:[0-9]*\\.)?[0-9]+\\b", DescriptorType.Word, clr, tmp, false);
        }

        void textBox_MouseWheel(object sender, MouseEventArgs e)
        {
            //The focus will never lie on the intellisense panel,
            //so scrolling the list must be handled from the text box.
            if (pnlIntel.ClientRectangle.Contains(pnlIntel.PointToClient(Control.MousePosition)))
                intelBox.SelectedIndex = (intelBox.SelectedIndex - (e.Delta / 120)).Clamp(0, intelBox.Items.Count - 1);
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Script TargetNode
        {
            get { return _targetNode; }
            set { _targetNode = value; MakeScript(); }
        }
        private Script _targetNode;

        public void MakeScript()
        {
            if (TargetNode == null)
                return;

            _updating = true;
            textBox.Text = TargetNode.MakeScript();
            _updating = false;
        }

        private void textBox_KeyDown(object sender, KeyEventArgs e)
        {
            char c = (char)e.KeyCode;
            bool modifier = e.Alt || e.Shift || e.Control;
            if (pnlIntel.Visible)
            {
                //Handle accept
                if ((e.KeyCode == Keys.Enter || e.KeyCode == Keys.Space) && pnlIntel.Visible && intelBox.SelectedItem != null)
                {
                    ApplyIntel();
                    e.Handled = true;
                    return;
                }

                //Handle up and down
                if (e.KeyValue == 40 || e.KeyValue == 38)
                {
                    e.Handled = true;
                    intelBox.SelectedIndex = (intelBox.SelectedIndex + (e.KeyValue == 40 ? 1 : -1)).Clamp(0, intelBox.Items.Count - 1);
                    return;
                }

                //Check if the typed character is acceptable in the buffer
                if (CharOK(c, e.Shift) && !e.Alt && !e.Control)
                {
                    _buffer += c.ToString();
                    SetBuffer();
                }
                else
                {
                    //Handle left and right
                    if (e.KeyValue == 39 || e.KeyValue == 37)
                    {
                        int n = textBox.SelectionStart + (e.KeyValue == 39 ? 1 : -1);
                        if (!(_buffer != null && n >= _start && n <= _buffer.Length))
                        {
                            HideIntel();
                            return;
                        }
                    }
                    else if (e.KeyCode == Keys.Back)
                    {
                        //Remove from buffer until it is empty
                        //When the buffer is empty, hide the intel panel
                        if (_buffer.Length > 0)
                        {
                            _buffer = _buffer.Substring(0, _buffer.Length - 1);
                            SetBuffer();
                        }
                        if (String.IsNullOrEmpty(_buffer))
                            HideIntel();
                        return;
                    }
                    else
                    {
                        //Don't want to close the panel when these are pressed
                        if (modifier)
                            return;

                        HideIntel();
                        return;
                    }
                }
            }
            else
            {
                //Check and see if the intel panel should be shown
                if (CharOK(c, e.Shift) && !e.Alt && !e.Control)
                {
                    _start = textBox.SelectionStart;
                    System.Drawing.Point p = textBox.GetPositionFromCharIndex(textBox.SelectionStart);
                    p.Y += (int)textBox.Font.GetHeight() * 2 - 6;
                    pnlIntel.Location = p;

                    _buffer = c.ToString();
                    SetBuffer();

                    pnlIntel.Show();
                }
            }
        }

        bool CharOK(char c, bool shift)
        {
            return Readable.Contains(c) && !(char.IsDigit(c) && shift);
        }

        void HideIntel()
        {
            pnlIntel.Hide();
            _buffer = "";
        }

        private static readonly CompareInfo CompareInfo = CultureInfo.CurrentCulture.CompareInfo;
        void SetBuffer()
        {
            if (_buffer == null)
                return;

            if (_buffer.Length > 0 && char.IsDigit(_buffer[0]))
            {
                SetSource(values);
                intelBox.SelectedIndex = -1;
                return;
            }

            List<string> t = values.Where(x => CompareInfo.IndexOf(x, _buffer, CompareOptions.IgnoreCase) >= 0).ToList();
            if (t.Count > 0)
            {
                SetSource(t);
                List<string> t2 = t.Where(x => CompareInfo.IsPrefix(x, _buffer, CompareOptions.IgnoreCase)).ToList();
                if (t2.Count > 0)
                {
                    t.Sort();
                    t2.Sort();
                    int i = t.IndexOf(t2[0]);
                    intelBox.SelectedIndex = i;
                }
            }
            else
            {
                //No matching value
                SetSource(values);
                intelBox.SelectedIndex = -1;
            }
        }

        void ApplyIntel()
        {
            if (pnlIntel.Visible)
            {
                textBox.Select(_start, textBox.SelectionStart);
                string value = intelBox.SelectedItem.ToString();
                int index = values.IndexOf(value);
                EventInformation info = Manager.Events.Values[index];
                string add = "(";
                if (info._paramNames.Length == 0)
                    add += ");" + Environment.NewLine;
                textBox.SelectedText = value + add;
                pnlIntel.Hide();
            }
        }

        private void intelBox_DoubleClick(object sender, EventArgs e)
        {
            ApplyIntel();
        }

        private void intelBox_MouseDown(object sender, MouseEventArgs e)
        {
            textBox.Focus();
        }

        private void textBox_SelectionChanged(object sender, EventArgs e)
        {
            int position = textBox.SelectionStart;
            if (pnlIntel.Visible)
                if (!(_buffer != null && position >= _start && position <= _buffer.Length))
                    HideIntel();

            //If the user clicks on an event or parameter, display the name and description.
            if (_targetNode != null)
            {
                EventInformation info = _targetNode.FindEvent(position + textBox.GetLineFromCharIndex(position));
                if (info != null && !String.IsNullOrEmpty(info._description))
                {
                    txtDescription.Visible = true;
                    txtDescription.Text = info._description;
                }
                else
                    txtDescription.Visible = false;
            }
        }

        //index, length
        static SortedList<int, int> _errors = new SortedList<int,int>();

        System.Timers.Timer _errorTimer;
        void _errorTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            _errorTimer.Enabled = false;
            if (_errors.Count > 0)
            {
                //Highlight collected errors

            }
        }

        bool _updating;

        BackgroundWorker _bWorker;
        private void textBox_TextChanged(object sender, EventArgs e)
        {
            if (_updating)
                return;

            //if (_errorTimer.Enabled)
            //    _errorTimer.Enabled = false;
            
            //_errors.Clear();
            //_errorTimer.Start();
            //if (_bWorker.IsBusy)
            //    _bWorker.CancelAsync();
            //_bWorker.RunWorkerAsync();
        }

        void _bWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            if (e.Cancel)
                return;

            //Collect errors
            string[] str = textBox.Text.Split(textBox.Seperators.GetAsCharArray());
        }
    }
}
