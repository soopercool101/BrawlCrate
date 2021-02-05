using BrawlLib.Internal.PowerPCAssembly;
using BrawlLib.SSBB.ResourceNodes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Security.Permissions;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace BrawlLib.Internal.Windows.Controls.Hex_Editor
{
    /// <summary>
    /// Represents a hex box control.
    /// </summary>
    [ToolboxBitmap(typeof(HexBox), "HexBox.bmp")]
    public class HexBox : Control
    {
        private SectionEditor _sectionEditor;

        public SectionEditor SectionEditor
        {
            get => _sectionEditor;
            set => _sectionEditor = value;
        }

        public SolidBrush BlackBrush = new SolidBrush(Color.Black);
        public SolidBrush GrayBrush = new SolidBrush(Color.Gray);
        public SolidBrush BlueBrush = new SolidBrush(Color.Blue);
        public SolidBrush GreenBrush = new SolidBrush(Color.Green);

        public List<string> annotationDescriptions = new List<string>();
        public List<string> annotationUnderlines = new List<string>();

        #region IKeyInterpreter interface

        /// <summary>
        /// Defines a user input handler such as for mouse and keyboard input
        /// </summary>
        private interface IKeyInterpreter
        {
            /// <summary>
            /// Activates mouse events
            /// </summary>
            void Activate();

            /// <summary>
            /// Deactivate mouse events
            /// </summary>
            void Deactivate();

            /// <summary>
            /// Preprocesses WM_KEYUP window message.
            /// </summary>
            /// <param name="m">the Message object to process.</param>
            /// <returns>True, if the message was processed.</returns>
            bool PreProcessWmKeyUp(ref Message m);

            /// <summary>
            /// Preprocesses WM_CHAR window message.
            /// </summary>
            /// <param name="m">the Message object to process.</param>
            /// <returns>True, if the message was processed.</returns>
            bool PreProcessWmChar(ref Message m);

            /// <summary>
            /// Preprocesses WM_KEYDOWN window message.
            /// </summary>
            /// <param name="m">the Message object to process.</param>
            /// <returns>True, if the message was processed.</returns>
            bool PreProcessWmKeyDown(ref Message m);

            /// <summary>
            /// Gives some information about where to place the caret.
            /// </summary>
            /// <param name="byteIndex">the index of the byte</param>
            /// <returns>the position where the caret is to place.</returns>
            PointF GetCaretPointF(long byteIndex);
        }

        #endregion

        #region EmptyKeyInterpreter class

        /// <summary>
        /// Represents an empty input handler without any functionality. 
        /// If is set ByteProvider to null, then this interpreter is used.
        /// </summary>
        private class EmptyKeyInterpreter : IKeyInterpreter
        {
            private readonly HexBox _hexBox;

            public EmptyKeyInterpreter(HexBox hexBox)
            {
                _hexBox = hexBox;
            }

            #region IKeyInterpreter Members

            public void Activate()
            {
            }

            public void Deactivate()
            {
            }

            public bool PreProcessWmKeyUp(ref Message m)
            {
                return _hexBox.BasePreProcessMessage(ref m);
            }

            public bool PreProcessWmChar(ref Message m)
            {
                return _hexBox.BasePreProcessMessage(ref m);
            }

            public bool PreProcessWmKeyDown(ref Message m)
            {
                return _hexBox.BasePreProcessMessage(ref m);
            }

            public PointF GetCaretPointF(long byteIndex)
            {
                return new PointF();
            }

            #endregion
        }

        #endregion

        #region KeyInterpreter class

        /// <summary>
        /// Handles user input such as mouse and keyboard input during hex view edit
        /// </summary>
        public class KeyInterpreter : IKeyInterpreter
        {
            /// <summary>
            /// Delegate for key-down processing.
            /// </summary>
            /// <param name="m">the message object contains key data information</param>
            /// <returns>True, if the message was processed</returns>
            private delegate bool MessageDelegate(ref Message m);

            #region Fields

            /// <summary>
            /// Contains the parent HexBox control
            /// </summary>
            protected HexBox _hexBox;

            /// <summary>
            /// Contains True, if shift key is down
            /// </summary>
            protected bool _shiftDown;

            /// <summary>
            /// Contains True, if mouse is down
            /// </summary>
            private bool _mouseDown;

            /// <summary>
            /// Contains the selection start position info
            /// </summary>
            private BytePositionInfo _bpiStart;

            /// <summary>
            /// Contains the current mouse selection position info
            /// </summary>
            private BytePositionInfo _bpi;

            /// <summary>
            /// Contains all message handlers of key interpreter key down message
            /// </summary>
            private Dictionary<Keys, MessageDelegate> _messageHandlers;

            #endregion

            #region Ctors

            public KeyInterpreter(HexBox hexBox)
            {
                _hexBox = hexBox;
            }

            #endregion

            #region Activate, Deactive methods

            public virtual void Activate()
            {
                _hexBox.MouseDown += new MouseEventHandler(BeginMouseSelection);
                _hexBox.MouseMove += new MouseEventHandler(UpdateMouseSelection);
                _hexBox.MouseUp += new MouseEventHandler(EndMouseSelection);
            }

            public virtual void Deactivate()
            {
                _hexBox.MouseDown -= new MouseEventHandler(BeginMouseSelection);
                _hexBox.MouseMove -= new MouseEventHandler(UpdateMouseSelection);
                _hexBox.MouseUp -= new MouseEventHandler(EndMouseSelection);
            }

            #endregion

            #region Mouse selection methods

            private void BeginMouseSelection(object sender, MouseEventArgs e)
            {
                if (e.Button != MouseButtons.Left)
                {
                    return;
                }

                _mouseDown = true;

                if (!_shiftDown)
                {
                    _bpiStart = new BytePositionInfo(_hexBox._bytePos, _hexBox._byteCharacterPos);
                    _hexBox.ReleaseSelection();
                }
                else
                {
                    UpdateMouseSelection(this, e);
                }
            }

            private void UpdateMouseSelection(object sender, MouseEventArgs e)
            {
                if (!_mouseDown)
                {
                    return;
                }

                _bpi = GetBytePositionInfo(new Point(e.X, e.Y));
                long selEnd = _bpi.Index;
                long realselStart;
                long realselLength;

                if (selEnd < _bpiStart.Index)
                {
                    realselStart = selEnd;
                    realselLength = _bpiStart.Index - selEnd;
                }
                else if (selEnd > _bpiStart.Index)
                {
                    realselStart = _bpiStart.Index;
                    realselLength = selEnd - realselStart;
                }
                else
                {
                    realselStart = _hexBox._bytePos;
                    realselLength = 0;
                }

                if (realselStart != _hexBox._bytePos || realselLength != _hexBox._selectionLength)
                {
                    _hexBox.InternalSelect(realselStart, realselLength);
                    _hexBox.ScrollByteIntoView(_bpi.Index);
                }
            }

            private void EndMouseSelection(object sender, MouseEventArgs e)
            {
                _mouseDown = false;
            }

            #endregion

            #region PrePrcessWmKeyDown methods

            public virtual bool PreProcessWmKeyDown(ref Message m)
            {
                Keys vc = (Keys) m.WParam.ToInt32();

                Keys keyData = vc | ModifierKeys;

                // detect whether key down event should be raised
                bool hasMessageHandler = MessageHandlers.ContainsKey(keyData);
                if (hasMessageHandler && RaiseKeyDown(keyData))
                {
                    return true;
                }

                MessageDelegate messageHandler = hasMessageHandler
                    ? MessageHandlers[keyData]
                    : messageHandler = new MessageDelegate(PreProcessWmKeyDown_Default);

                return messageHandler(ref m);
            }

            protected bool PreProcessWmKeyDown_Default(ref Message m)
            {
                _hexBox.ScrollByteIntoView();
                return _hexBox.BasePreProcessMessage(ref m);
            }

            protected bool RaiseKeyDown(Keys keyData)
            {
                KeyEventArgs e = new KeyEventArgs(keyData);
                _hexBox.OnKeyDown(e);
                return e.Handled;
            }

            protected virtual bool PreProcessWmKeyDown_Left(ref Message m)
            {
                return PerformPosMoveLeft();
            }

            protected virtual bool PreProcessWmKeyDown_Up(ref Message m)
            {
                long pos = _hexBox._bytePos;
                int cp = _hexBox._byteCharacterPos;

                if (!(pos == 0 && cp == 0))
                {
                    pos = Math.Max(-1, pos - _hexBox._iHexMaxHBytes);
                    if (pos == -1)
                    {
                        return true;
                    }

                    _hexBox.SetPosition(pos);

                    if (pos < _hexBox._startByte)
                    {
                        _hexBox.PerformScrollLineUp();
                    }

                    _hexBox.UpdateCaret();
                    _hexBox.Invalidate();
                }

                _hexBox.ScrollByteIntoView();
                _hexBox.ReleaseSelection();

                return true;
            }

            protected virtual bool PreProcessWmKeyDown_Right(ref Message m)
            {
                return PerformPosMoveRight();
            }

            protected virtual bool PreProcessWmKeyDown_Down(ref Message m)
            {
                long pos = _hexBox._bytePos;
                int cp = _hexBox._byteCharacterPos;

                if (pos == _hexBox._byteProvider.Length && cp == 0)
                {
                    return true;
                }

                pos = Math.Min(_hexBox._byteProvider.Length, pos + _hexBox._iHexMaxHBytes);

                if (pos == _hexBox._byteProvider.Length)
                {
                    cp = 0;
                }

                _hexBox.SetPosition(pos, cp);

                if (pos > _hexBox._endByte - 1)
                {
                    _hexBox.PerformScrollLineDown();
                }

                _hexBox.UpdateCaret();
                _hexBox.ScrollByteIntoView();
                _hexBox.ReleaseSelection();
                _hexBox.Invalidate();

                return true;
            }

            protected virtual bool PreProcessWmKeyDown_PageUp(ref Message m)
            {
                long pos = _hexBox._bytePos;
                int cp = _hexBox._byteCharacterPos;

                if (pos == 0 && cp == 0)
                {
                    return true;
                }

                pos = Math.Max(0, pos - _hexBox._iHexMaxBytes);
                if (pos == 0)
                {
                    return true;
                }

                _hexBox.SetPosition(pos);

                if (pos < _hexBox._startByte)
                {
                    _hexBox.PerformScrollPageUp();
                }

                _hexBox.ReleaseSelection();
                _hexBox.UpdateCaret();
                _hexBox.Invalidate();
                return true;
            }

            protected virtual bool PreProcessWmKeyDown_PageDown(ref Message m)
            {
                long pos = _hexBox._bytePos;
                int cp = _hexBox._byteCharacterPos;

                if (pos == _hexBox._byteProvider.Length && cp == 0)
                {
                    return true;
                }

                pos = Math.Min(_hexBox._byteProvider.Length, pos + _hexBox._iHexMaxBytes);

                if (pos == _hexBox._byteProvider.Length)
                {
                    cp = 0;
                }

                _hexBox.SetPosition(pos, cp);

                if (pos > _hexBox._endByte - 1)
                {
                    _hexBox.PerformScrollPageDown();
                }

                _hexBox.ReleaseSelection();
                _hexBox.UpdateCaret();
                _hexBox.Invalidate();

                return true;
            }

            protected virtual bool PreProcessWmKeyDown_ShiftLeft(ref Message m)
            {
                long pos = _hexBox._bytePos;
                long sel = _hexBox._selectionLength;

                if (pos + sel < 1)
                {
                    return true;
                }

                if (pos + sel <= _bpiStart.Index)
                {
                    if (pos == 0)
                    {
                        return true;
                    }

                    pos--;
                    sel++;
                }
                else
                {
                    sel = Math.Max(0, sel - 1);
                }

                _hexBox.ScrollByteIntoView();
                _hexBox.InternalSelect(pos, sel);

                return true;
            }

            protected virtual bool PreProcessWmKeyDown_ShiftUp(ref Message m)
            {
                long pos = _hexBox._bytePos;
                long sel = _hexBox._selectionLength;

                if (pos - _hexBox._iHexMaxHBytes < 0 && pos <= _bpiStart.Index)
                {
                    return true;
                }

                if (_bpiStart.Index >= pos + sel)
                {
                    pos = pos - _hexBox._iHexMaxHBytes;
                    sel += _hexBox._iHexMaxHBytes;
                    _hexBox.InternalSelect(pos, sel);
                    _hexBox.ScrollByteIntoView();
                }
                else
                {
                    sel -= _hexBox._iHexMaxHBytes;
                    if (sel < 0)
                    {
                        pos = _bpiStart.Index + sel;
                        sel = -sel;
                        _hexBox.InternalSelect(pos, sel);
                        _hexBox.ScrollByteIntoView();
                    }
                    else
                    {
                        sel -= _hexBox._iHexMaxHBytes;
                        _hexBox.InternalSelect(pos, sel);
                        _hexBox.ScrollByteIntoView(pos + sel);
                    }
                }

                return true;
            }

            protected virtual bool PreProcessWmKeyDown_ShiftRight(ref Message m)
            {
                long pos = _hexBox._bytePos;
                long sel = _hexBox._selectionLength;

                if (pos + sel >= _hexBox._byteProvider.Length)
                {
                    return true;
                }

                if (_bpiStart.Index <= pos)
                {
                    sel++;
                    _hexBox.InternalSelect(pos, sel);
                    _hexBox.ScrollByteIntoView(pos + sel);
                }
                else
                {
                    pos++;
                    sel = Math.Max(0, sel - 1);
                    _hexBox.InternalSelect(pos, sel);
                    _hexBox.ScrollByteIntoView();
                }

                return true;
            }

            protected virtual bool PreProcessWmKeyDown_ShiftDown(ref Message m)
            {
                long pos = _hexBox._bytePos;
                long sel = _hexBox._selectionLength;

                long max = _hexBox._byteProvider.Length;

                if (pos + sel + _hexBox._iHexMaxHBytes > max)
                {
                    return true;
                }

                if (_bpiStart.Index <= pos)
                {
                    sel += _hexBox._iHexMaxHBytes;
                    _hexBox.InternalSelect(pos, sel);
                    _hexBox.ScrollByteIntoView(pos + sel);
                }
                else
                {
                    sel -= _hexBox._iHexMaxHBytes;
                    if (sel < 0)
                    {
                        pos = _bpiStart.Index;
                        sel = -sel;
                    }
                    else
                    {
                        pos += _hexBox._iHexMaxHBytes;
                        //sel -= _hexBox._iHexMaxHBytes;
                    }

                    _hexBox.InternalSelect(pos, sel);
                    _hexBox.ScrollByteIntoView();
                }

                return true;
            }

            protected virtual bool PreProcessWmKeyDown_Tab(ref Message m)
            {
                if (_hexBox._stringViewVisible && _hexBox._keyInterpreter.GetType() == typeof(KeyInterpreter))
                {
                    _hexBox.ActivateStringKeyInterpreter();
                    _hexBox.ScrollByteIntoView();
                    _hexBox.ReleaseSelection();
                    _hexBox.UpdateCaret();
                    _hexBox.Invalidate();
                    return true;
                }

                _hexBox.Parent?.SelectNextControl(_hexBox, true, true, true, true);
                return true;
            }

            protected virtual bool PreProcessWmKeyDown_ShiftTab(ref Message m)
            {
                if (_hexBox._keyInterpreter is StringKeyInterpreter)
                {
                    _shiftDown = false;
                    _hexBox.ActivateKeyInterpreter();
                    _hexBox.ScrollByteIntoView();
                    _hexBox.ReleaseSelection();
                    _hexBox.UpdateCaret();
                    _hexBox.Invalidate();
                    return true;
                }

                _hexBox.Parent?.SelectNextControl(_hexBox, false, true, true, true);
                return true;
            }

            protected virtual bool PreProcessWmKeyDown_Back(ref Message m)
            {
                if (!_hexBox._byteProvider.SupportsDeleteBytes())
                {
                    return true;
                }

                if (_hexBox.ReadOnly)
                {
                    return true;
                }

                long pos = _hexBox._bytePos;
                long sel = _hexBox._selectionLength;
                int cp = _hexBox._byteCharacterPos;

                long startDelete = cp == 0 && sel == 0 ? pos - 1 : pos;
                if (startDelete < 0 && sel < 1)
                {
                    return true;
                }

                long bytesToDelete = sel > 0 ? sel : 1;
                _hexBox._byteProvider.DeleteBytes(Math.Max(0, startDelete), bytesToDelete);
                _hexBox.UpdateScrollSize();

                if (sel == 0)
                {
                    PerformPosMoveLeftByte();
                }

                _hexBox.ReleaseSelection();
                _hexBox.Invalidate();

                return true;
            }

            protected virtual bool PreProcessWmKeyDown_Delete(ref Message m)
            {
                if (!_hexBox._byteProvider.SupportsDeleteBytes())
                {
                    return true;
                }

                if (_hexBox.ReadOnly)
                {
                    return true;
                }

                long pos = _hexBox._bytePos;
                long sel = _hexBox._selectionLength;

                if (pos >= _hexBox._byteProvider.Length)
                {
                    return true;
                }

                long bytesToDelete = sel > 0 ? sel : 1;
                _hexBox._byteProvider.DeleteBytes(pos, bytesToDelete);

                _hexBox.UpdateScrollSize();
                _hexBox.ReleaseSelection();
                _hexBox.Invalidate();

                return true;
            }

            protected virtual bool PreProcessWmKeyDown_Home(ref Message m)
            {
                long pos = _hexBox._bytePos;
                int cp = _hexBox._byteCharacterPos;

                if (pos < 1)
                {
                    return true;
                }

                pos = 0;
                cp = 0;
                _hexBox.SetPosition(pos, cp);

                _hexBox.ScrollByteIntoView();
                _hexBox.UpdateCaret();
                _hexBox.ReleaseSelection();

                return true;
            }

            protected virtual bool PreProcessWmKeyDown_End(ref Message m)
            {
                long pos = _hexBox._bytePos;
                int cp = _hexBox._byteCharacterPos;

                if (pos >= _hexBox._byteProvider.Length - 1)
                {
                    return true;
                }

                pos = _hexBox._byteProvider.Length;
                cp = 0;
                _hexBox.SetPosition(pos, cp);

                _hexBox.ScrollByteIntoView();
                _hexBox.UpdateCaret();
                _hexBox.ReleaseSelection();

                return true;
            }

            protected virtual bool PreProcessWmKeyDown_ShiftShiftKey(ref Message m)
            {
                if (_mouseDown)
                {
                    return true;
                }

                if (_shiftDown)
                {
                    return true;
                }

                _shiftDown = true;

                if (_hexBox._selectionLength > 0)
                {
                    return true;
                }

                _bpiStart = new BytePositionInfo(_hexBox._bytePos, _hexBox._byteCharacterPos);

                return true;
            }

            protected virtual bool PreProcessWmKeyDown_ControlC(ref Message m)
            {
                _hexBox.Copy();
                return true;
            }

            protected virtual bool PreProcessWmKeyDown_ControlX(ref Message m)
            {
                _hexBox.Cut();
                return true;
            }

            protected virtual bool PreProcessWmKeyDown_ControlV(ref Message m)
            {
                _hexBox.Paste();
                return true;
            }

            #endregion

            #region PreProcessWmChar methods

            public virtual bool PreProcessWmChar(ref Message m)
            {
                if (ModifierKeys == Keys.Control)
                {
                    return _hexBox.BasePreProcessMessage(ref m);
                }

                bool sw = _hexBox._byteProvider.SupportsWriteByte();
                bool si = _hexBox._byteProvider.SupportsInsertBytes();
                bool sd = _hexBox._byteProvider.SupportsDeleteBytes();

                long pos = _hexBox._bytePos;
                long sel = _hexBox._selectionLength;
                int cp = _hexBox._byteCharacterPos;

                if (
                    !sw && pos != _hexBox._byteProvider.Length ||
                    !si && pos == _hexBox._byteProvider.Length)
                {
                    return _hexBox.BasePreProcessMessage(ref m);
                }

                char c = (char) m.WParam.ToInt32();

                if (Uri.IsHexDigit(c))
                {
                    if (RaiseKeyPress(c))
                    {
                        return true;
                    }

                    if (_hexBox.ReadOnly)
                    {
                        return true;
                    }

                    bool isInsertMode = pos == _hexBox._byteProvider.Length;

                    // do insert when insertActive = true
                    if (!isInsertMode && si && _hexBox.InsertActive && cp == 0)
                    {
                        isInsertMode = true;
                    }

                    if (sd && si && sel > 0)
                    {
                        _hexBox._byteProvider.DeleteBytes(pos, sel);
                        isInsertMode = true;
                        cp = 0;
                        _hexBox.SetPosition(pos, cp);
                    }

                    _hexBox.ReleaseSelection();

                    byte currentByte;
                    if (isInsertMode)
                    {
                        currentByte = 0;
                    }
                    else
                    {
                        currentByte = _hexBox._byteProvider.ReadByte(pos);
                    }

                    string sCb = currentByte.ToString("X", System.Threading.Thread.CurrentThread.CurrentCulture);
                    if (sCb.Length == 1)
                    {
                        sCb = "0" + sCb;
                    }

                    string sNewCb = c.ToString();
                    if (cp == 0)
                    {
                        sNewCb += sCb.Substring(1, 1);
                    }
                    else
                    {
                        sNewCb = sCb.Substring(0, 1) + sNewCb;
                    }

                    byte newcb = byte.Parse(sNewCb, NumberStyles.AllowHexSpecifier,
                        System.Threading.Thread.CurrentThread.CurrentCulture);

                    if (isInsertMode)
                    {
                        _hexBox._byteProvider.InsertBytes(pos, new byte[] {newcb});
                    }
                    else
                    {
                        _hexBox._byteProvider.WriteByte(pos, newcb);
                    }

                    PerformPosMoveRight();

                    _hexBox.Invalidate();
                    return true;
                }

                return _hexBox.BasePreProcessMessage(ref m);
            }

            protected bool RaiseKeyPress(char keyChar)
            {
                KeyPressEventArgs e = new KeyPressEventArgs(keyChar);
                _hexBox.OnKeyPress(e);
                return e.Handled;
            }

            #endregion

            #region PreProcessWmKeyUp methods

            public virtual bool PreProcessWmKeyUp(ref Message m)
            {
                Keys vc = (Keys) m.WParam.ToInt32();

                Keys keyData = vc | ModifierKeys;

                switch (keyData)
                {
                    case Keys.ShiftKey:
                    case Keys.Insert:
                        if (RaiseKeyUp(keyData))
                        {
                            return true;
                        }

                        break;
                }

                switch (keyData)
                {
                    case Keys.ShiftKey:
                        _shiftDown = false;
                        return true;
                    case Keys.Insert:
                        return PreProcessWmKeyUp_Insert(ref m);
                    default:
                        return _hexBox.BasePreProcessMessage(ref m);
                }
            }

            protected virtual bool PreProcessWmKeyUp_Insert(ref Message m)
            {
                _hexBox.InsertActive = !_hexBox.InsertActive;
                return true;
            }

            protected bool RaiseKeyUp(Keys keyData)
            {
                KeyEventArgs e = new KeyEventArgs(keyData);
                _hexBox.OnKeyUp(e);
                return e.Handled;
            }

            #endregion

            #region Misc

            private Dictionary<Keys, MessageDelegate> MessageHandlers
            {
                get
                {
                    if (_messageHandlers == null)
                    {
                        _messageHandlers = new Dictionary<Keys, MessageDelegate>
                        {
                            {Keys.Left, new MessageDelegate(PreProcessWmKeyDown_Left)},         // move left
                            {Keys.Up, new MessageDelegate(PreProcessWmKeyDown_Up)},             // move up
                            {Keys.Right, new MessageDelegate(PreProcessWmKeyDown_Right)},       // move right
                            {Keys.Down, new MessageDelegate(PreProcessWmKeyDown_Down)},         // move down
                            {Keys.PageUp, new MessageDelegate(PreProcessWmKeyDown_PageUp)},     // move pageup
                            {Keys.PageDown, new MessageDelegate(PreProcessWmKeyDown_PageDown)}, // move page down
                            {
                                Keys.Left | Keys.Shift, new MessageDelegate(PreProcessWmKeyDown_ShiftLeft)
                            }, // move left with selection
                            {
                                Keys.Up | Keys.Shift, new MessageDelegate(PreProcessWmKeyDown_ShiftUp)
                            }, // move up with selection
                            {
                                Keys.Right | Keys.Shift, new MessageDelegate(PreProcessWmKeyDown_ShiftRight)
                            }, // move right with selection
                            {
                                Keys.Down | Keys.Shift, new MessageDelegate(PreProcessWmKeyDown_ShiftDown)
                            },                                                              // move down with selection
                            {Keys.Tab, new MessageDelegate(PreProcessWmKeyDown_Tab)},       // switch to string view
                            {Keys.Back, new MessageDelegate(PreProcessWmKeyDown_Back)},     // back
                            {Keys.Delete, new MessageDelegate(PreProcessWmKeyDown_Delete)}, // delete
                            {Keys.Home, new MessageDelegate(PreProcessWmKeyDown_Home)},     // move to home
                            {Keys.End, new MessageDelegate(PreProcessWmKeyDown_End)},       // move to end
                            {
                                Keys.ShiftKey | Keys.Shift, new MessageDelegate(PreProcessWmKeyDown_ShiftShiftKey)
                            },                                                                          // begin selection process
                            {Keys.C | Keys.Control, new MessageDelegate(PreProcessWmKeyDown_ControlC)}, // copy 
                            {Keys.X | Keys.Control, new MessageDelegate(PreProcessWmKeyDown_ControlX)}, // cut
                            {Keys.V | Keys.Control, new MessageDelegate(PreProcessWmKeyDown_ControlV)}  // paste
                        };
                    }

                    return _messageHandlers;
                }
            }

            protected virtual bool PerformPosMoveLeft()
            {
                long pos = _hexBox._bytePos;
                long sel = _hexBox._selectionLength;
                int cp = _hexBox._byteCharacterPos;

                if (sel != 0)
                {
                    cp = 0;
                    _hexBox.SetPosition(pos, cp);
                    _hexBox.ReleaseSelection();
                }
                else
                {
                    if (pos == 0 && cp == 0)
                    {
                        return true;
                    }

                    if (cp > 0)
                    {
                        cp--;
                    }
                    else
                    {
                        pos = Math.Max(0, pos - 1);
                        cp++;
                    }

                    _hexBox.SetPosition(pos, cp);

                    if (pos < _hexBox._startByte)
                    {
                        _hexBox.PerformScrollLineUp();
                    }

                    _hexBox.UpdateCaret();
                    _hexBox.Invalidate();
                }

                _hexBox.ScrollByteIntoView();
                return true;
            }

            protected virtual bool PerformPosMoveRight()
            {
                long pos = _hexBox._bytePos;
                int cp = _hexBox._byteCharacterPos;
                long sel = _hexBox._selectionLength;

                if (sel != 0)
                {
                    pos += sel;
                    cp = 0;
                    _hexBox.SetPosition(pos, cp);
                    _hexBox.ReleaseSelection();
                }
                else
                {
                    if (!(pos == _hexBox._byteProvider.Length && cp == 0))
                    {
                        if (cp > 0)
                        {
                            pos = Math.Min(_hexBox._byteProvider.Length, pos + 1);
                            cp = 0;
                        }
                        else
                        {
                            cp++;
                        }

                        _hexBox.SetPosition(pos, cp);

                        if (pos > _hexBox._endByte - 1)
                        {
                            _hexBox.PerformScrollLineDown();
                        }

                        _hexBox.UpdateCaret();
                        _hexBox.Invalidate();
                    }
                }

                _hexBox.ScrollByteIntoView();
                return true;
            }

            protected virtual bool PerformPosMoveLeftByte()
            {
                long pos = _hexBox._bytePos;
                int cp = _hexBox._byteCharacterPos;

                if (pos == 0)
                {
                    return true;
                }

                pos = Math.Max(0, pos - 1);
                cp = 0;

                _hexBox.SetPosition(pos, cp);

                if (pos < _hexBox._startByte)
                {
                    _hexBox.PerformScrollLineUp();
                }

                _hexBox.UpdateCaret();
                _hexBox.ScrollByteIntoView();
                _hexBox.Invalidate();

                return true;
            }

            protected virtual bool PerformPosMoveRightByte()
            {
                long pos = _hexBox._bytePos;
                int cp = _hexBox._byteCharacterPos;

                if (pos == _hexBox._byteProvider.Length)
                {
                    return true;
                }

                pos = Math.Min(_hexBox._byteProvider.Length, pos + 1);
                cp = 0;

                _hexBox.SetPosition(pos, cp);

                if (pos > _hexBox._endByte - 1)
                {
                    _hexBox.PerformScrollLineDown();
                }

                _hexBox.UpdateCaret();
                _hexBox.ScrollByteIntoView();
                _hexBox.Invalidate();

                return true;
            }

            public virtual PointF GetCaretPointF(long byteIndex)
            {
                return _hexBox.GetBytePointF(byteIndex);
            }

            protected virtual BytePositionInfo GetBytePositionInfo(Point p)
            {
                return _hexBox.GetHexBytePositionInfo(p);
            }

            #endregion
        }

        #endregion

        #region StringKeyInterpreter class

        /// <summary>
        /// Handles user input such as mouse and keyboard input during string view edit
        /// </summary>
        private class StringKeyInterpreter : KeyInterpreter
        {
            #region Ctors

            public StringKeyInterpreter(HexBox hexBox)
                : base(hexBox)
            {
                _hexBox._byteCharacterPos = 0;
            }

            #endregion

            #region PreProcessWmKeyDown methods

            public override bool PreProcessWmKeyDown(ref Message m)
            {
                Keys vc = (Keys) m.WParam.ToInt32();

                Keys keyData = vc | ModifierKeys;

                switch (keyData)
                {
                    case Keys.Tab | Keys.Shift:
                    case Keys.Tab:
                        if (RaiseKeyDown(keyData))
                        {
                            return true;
                        }

                        break;
                }

                switch (keyData)
                {
                    case Keys.Tab | Keys.Shift:
                        return PreProcessWmKeyDown_ShiftTab(ref m);
                    case Keys.Tab:
                        return PreProcessWmKeyDown_Tab(ref m);
                    default:
                        return base.PreProcessWmKeyDown(ref m);
                }
            }

            protected override bool PreProcessWmKeyDown_Left(ref Message m)
            {
                return PerformPosMoveLeftByte();
            }

            protected override bool PreProcessWmKeyDown_Right(ref Message m)
            {
                return PerformPosMoveRightByte();
            }

            #endregion

            #region PreProcessWmChar methods

            public override bool PreProcessWmChar(ref Message m)
            {
                if (ModifierKeys == Keys.Control)
                {
                    return _hexBox.BasePreProcessMessage(ref m);
                }

                bool sw = _hexBox._byteProvider.SupportsWriteByte();
                bool si = _hexBox._byteProvider.SupportsInsertBytes();
                bool sd = _hexBox._byteProvider.SupportsDeleteBytes();

                long pos = _hexBox._bytePos;
                long sel = _hexBox._selectionLength;
                int cp = _hexBox._byteCharacterPos;

                if (
                    !sw && pos != _hexBox._byteProvider.Length ||
                    !si && pos == _hexBox._byteProvider.Length)
                {
                    return _hexBox.BasePreProcessMessage(ref m);
                }

                char c = (char) m.WParam.ToInt32();

                if (RaiseKeyPress(c))
                {
                    return true;
                }

                if (_hexBox.ReadOnly)
                {
                    return true;
                }

                bool isInsertMode = pos == _hexBox._byteProvider.Length;

                // do insert when insertActive = true
                if (!isInsertMode && si && _hexBox.InsertActive)
                {
                    isInsertMode = true;
                }

                if (sd && si && sel > 0)
                {
                    _hexBox._byteProvider.DeleteBytes(pos, sel);
                    isInsertMode = true;
                    cp = 0;
                    _hexBox.SetPosition(pos, cp);
                }

                _hexBox.ReleaseSelection();

                byte b = _hexBox.ByteCharConverter.ToByte(c);
                if (isInsertMode)
                {
                    _hexBox._byteProvider.InsertBytes(pos, new byte[] {b});
                }
                else
                {
                    _hexBox._byteProvider.WriteByte(pos, b);
                }

                PerformPosMoveRightByte();
                _hexBox.Invalidate();

                return true;
            }

            #endregion

            #region Misc

            public override PointF GetCaretPointF(long byteIndex)
            {
                Point gp = _hexBox.GetGridBytePoint(byteIndex);
                return _hexBox.GetByteStringPointF(gp);
            }

            protected override BytePositionInfo GetBytePositionInfo(Point p)
            {
                return _hexBox.GetStringBytePositionInfo(p);
            }

            #endregion
        }

        #endregion

        #region Fields

        /// <summary>
        /// Contains the hole content bounds of all text
        /// </summary>
        private Rectangle _recContent;

        /// <summary>
        /// Contains the line info bounds
        /// </summary>
        private Rectangle _recLineInfo;

        /// <summary>
        /// Contains the column info header rectangle bounds
        /// </summary>
        private Rectangle _recColumnInfo;

        /// <summary>
        /// Contains the hex data bounds
        /// </summary>
        private Rectangle _recHex;

        /// <summary>
        /// Contains the string view bounds
        /// </summary>
        private Rectangle _recStringView;

        /// <summary>
        /// Contains string format information for text drawing
        /// </summary>
        private readonly StringFormat _stringFormat;

        /// <summary>
        /// Contains the width and height of a single char
        /// </summary>
        private SizeF _charSize;

        /// <summary>
        /// Contains the maximum of visible horizontal bytes
        /// </summary>
        private int _iHexMaxHBytes;

        /// <summary>
        /// Contains the maximum of visible vertical bytes
        /// </summary>
        private int _iHexMaxVBytes;

        /// <summary>
        /// Contains the maximum of visible bytes.
        /// </summary>
        private int _iHexMaxBytes;

        /// <summary>
        /// Contains the scroll bars minimum value
        /// </summary>
        private long _scrollVmin;

        /// <summary>
        /// Contains the scroll bars maximum value
        /// </summary>
        private long _scrollVmax;

        /// <summary>
        /// Contains the scroll bars current position
        /// </summary>
        private long _scrollVpos;

        /// <summary>
        /// Contains a vertical scroll
        /// </summary>
        private readonly VScrollBar _vScrollBar;

        /// <summary>
        /// Contains a timer for thumbtrack scrolling
        /// </summary>
        private readonly Timer _thumbTrackTimer = new Timer();

        /// <summary>
        /// Contains the thumbtrack scrolling position
        /// </summary>
        private long _thumbTrackPosition;

        /// <summary>
        /// Contains the thumptrack delay for scrolling in milliseconds.
        /// </summary>
        private const int THUMPTRACKDELAY = 50;

        /// <summary>
        /// Contains the Enviroment.TickCount of the last refresh
        /// </summary>
        private int _lastThumbtrack;

        /// <summary>
        /// Contains the border큦 left shift
        /// </summary>
        private int _recBorderLeft = SystemInformation.Border3DSize.Width;

        /// <summary>
        /// Contains the border큦 right shift
        /// </summary>
        private int _recBorderRight = SystemInformation.Border3DSize.Width;

        /// <summary>
        /// Contains the border큦 top shift
        /// </summary>
        private int _recBorderTop = SystemInformation.Border3DSize.Height;

        /// <summary>
        /// Contains the border bottom shift
        /// </summary>
        private int _recBorderBottom = SystemInformation.Border3DSize.Height;

        /// <summary>
        /// Contains the index of the first visible byte
        /// </summary>
        private long _startByte;

        /// <summary>
        /// Contains the index of the last visible byte
        /// </summary>
        private long _endByte;

        /// <summary>
        /// Contains the current byte position
        /// </summary>
        private long _bytePos = -1;

        /// <summary>
        /// Contains the current char position in one byte
        /// </summary>
        /// <example>
        /// "1A"
        /// "1" = char position of 0
        /// "A" = char position of 1
        /// </example>
        private int _byteCharacterPos;

        /// <summary>
        /// Contains string format information for hex values
        /// </summary>
        private string _hexStringFormat = "X";


        /// <summary>
        /// Contains the current key interpreter
        /// </summary>
        private IKeyInterpreter _keyInterpreter;

        /// <summary>
        /// Contains an empty key interpreter without functionality
        /// </summary>
        private EmptyKeyInterpreter _eki;

        /// <summary>
        /// Contains the default key interpreter
        /// </summary>
        public KeyInterpreter _ki;

        /// <summary>
        /// Contains the string key interpreter
        /// </summary>
        private StringKeyInterpreter _ski;

        /// <summary>
        /// Contains True if caret is visible
        /// </summary>
        private bool _caretVisible;

        /// <summary>
        /// Contains true, if the find (Find method) should be aborted.
        /// </summary>
        private bool _abortFind;

        /// <summary>
        /// Contains a value of the current finding position.
        /// </summary>
        private long _findingPos;

        /// <summary>
        /// Contains a state value about Insert or Write mode. When this value is true and the ByteProvider SupportsInsert is true bytes are inserted instead of overridden.
        /// </summary>
        private bool _insertActive;

        #endregion

        #region Events

        /// <summary>
        /// Occurs, when the value of InsertActive property has changed.
        /// </summary>
        [Description("Occurs, when the value of InsertActive property has changed.")]
        public event EventHandler InsertActiveChanged;

        /// <summary>
        /// Occurs, when the value of ReadOnly property has changed.
        /// </summary>
        [Description("Occurs, when the value of ReadOnly property has changed.")]
        public event EventHandler ReadOnlyChanged;

        /// <summary>
        /// Occurs, when the value of ByteProvider property has changed.
        /// </summary>
        [Description("Occurs, when the value of ByteProvider property has changed.")]
        public event EventHandler ByteProviderChanged;

        /// <summary>
        /// Occurs, when the value of SelectionStart property has changed.
        /// </summary>
        [Description("Occurs, when the value of SelectionStart property has changed.")]
        public event EventHandler SelectionStartChanged;

        /// <summary>
        /// Occurs, when the value of SelectionLength property has changed.
        /// </summary>
        [Description("Occurs, when the value of SelectionLength property has changed.")]
        public event EventHandler SelectionLengthChanged;

        /// <summary>
        /// Occurs, when the value of LineInfoVisible property has changed.
        /// </summary>
        [Description("Occurs, when the value of LineInfoVisible property has changed.")]
        public event EventHandler LineInfoVisibleChanged;

        /// <summary>
        /// Occurs, when the value of ColumnInfoVisibleChanged property has changed.
        /// </summary>
        [Description("Occurs, when the value of ColumnInfoVisibleChanged property has changed.")]
        public event EventHandler ColumnInfoVisibleChanged;

        /// <summary>
        /// Occurs, when the value of GroupSeparatorVisibleChanged property has changed.
        /// </summary>
        [Description("Occurs, when the value of GroupSeparatorVisibleChanged property has changed.")]
        public event EventHandler GroupSeparatorVisibleChanged;

        /// <summary>
        /// Occurs, when the value of StringViewVisible property has changed.
        /// </summary>
        [Description("Occurs, when the value of StringViewVisible property has changed.")]
        public event EventHandler StringViewVisibleChanged;

        /// <summary>
        /// Occurs, when the value of BorderStyle property has changed.
        /// </summary>
        [Description("Occurs, when the value of BorderStyle property has changed.")]
        public event EventHandler BorderStyleChanged;

        /// <summary>
        /// Occurs, when the value of ColumnWidth property has changed.
        /// </summary>
        [Description("Occurs, when the value of GroupSize property has changed.")]
        public event EventHandler GroupSizeChanged;

        /// <summary>
        /// Occurs, when the value of BytesPerLine property has changed.
        /// </summary>
        [Description("Occurs, when the value of BytesPerLine property has changed.")]
        public event EventHandler BytesPerLineChanged;

        /// <summary>
        /// Occurs, when the value of UseFixedBytesPerLine property has changed.
        /// </summary>
        [Description("Occurs, when the value of UseFixedBytesPerLine property has changed.")]
        public event EventHandler UseFixedBytesPerLineChanged;

        /// <summary>
        /// Occurs, when the value of VScrollBarVisible property has changed.
        /// </summary>
        [Description("Occurs, when the value of VScrollBarVisible property has changed.")]
        public event EventHandler VScrollBarVisibleChanged;

        /// <summary>
        /// Occurs, when the value of HexCasing property has changed.
        /// </summary>
        [Description("Occurs, when the value of HexCasing property has changed.")]
        public event EventHandler HexCasingChanged;

        /// <summary>
        /// Occurs, when the value of HorizontalByteCount property has changed.
        /// </summary>
        [Description("Occurs, when the value of HorizontalByteCount property has changed.")]
        public event EventHandler HorizontalByteCountChanged;

        /// <summary>
        /// Occurs, when the value of VerticalByteCount property has changed.
        /// </summary>
        [Description("Occurs, when the value of VerticalByteCount property has changed.")]
        public event EventHandler VerticalByteCountChanged;

        /// <summary>
        /// Occurs, when the value of CurrentLine property has changed.
        /// </summary>
        [Description("Occurs, when the value of CurrentLine property has changed.")]
        public event EventHandler CurrentLineChanged;

        /// <summary>
        /// Occurs, when the value of CurrentPositionInLine property has changed.
        /// </summary>
        [Description("Occurs, when the value of CurrentPositionInLine property has changed.")]
        public event EventHandler CurrentPositionInLineChanged;

        /// <summary>
        /// Occurs, when Copy method was invoked and ClipBoardData changed.
        /// </summary>
        [Description("Occurs, when Copy method was invoked and ClipBoardData changed.")]
        public event EventHandler Copied;

        /// <summary>
        /// Occurs, when CopyHex method was invoked and ClipBoardData changed.
        /// </summary>
        [Description("Occurs, when CopyHex method was invoked and ClipBoardData changed.")]
        public event EventHandler CopiedHex;

        #endregion

        #region Ctors

        /// <summary>
        /// Initializes a new instance of a HexBox class.
        /// </summary>
        public HexBox()
        {
            _vScrollBar = new VScrollBar();
            _vScrollBar.Scroll += new ScrollEventHandler(vScrollBar_Scroll);
            _vScrollBar.Cursor = Cursors.Default;

            _builtInContextMenu = new BuiltInContextMenu(this);

            BackColor = Color.White;
            Font = new Font("Courier New", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            _stringFormat = new StringFormat(StringFormat.GenericTypographic)
            {
                FormatFlags = StringFormatFlags.MeasureTrailingSpaces
            };

            ActivateEmptyKeyInterpreter();

            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.DoubleBuffer, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.ResizeRedraw, true);

            _thumbTrackTimer.Interval = 50;
            _thumbTrackTimer.Tick += new EventHandler(PerformScrollThumbTrack);
        }

        #endregion

        #region Scroll methods

        private void vScrollBar_Scroll(object sender, ScrollEventArgs e)
        {
            switch (e.Type)
            {
                case ScrollEventType.Last:
                    break;
                case ScrollEventType.EndScroll:
                    break;
                case ScrollEventType.SmallIncrement:
                    PerformScrollLineDown();
                    break;
                case ScrollEventType.SmallDecrement:
                    PerformScrollLineUp();
                    break;
                case ScrollEventType.LargeIncrement:
                    PerformScrollPageDown();
                    break;
                case ScrollEventType.LargeDecrement:
                    PerformScrollPageUp();
                    break;
                case ScrollEventType.ThumbPosition:
                    long lPos = FromScrollPos(e.NewValue);
                    PerformScrollThumpPosition(lPos);
                    break;
                case ScrollEventType.ThumbTrack:
                    // to avoid performance problems use a refresh delay implemented with a timer
                    if (_thumbTrackTimer.Enabled) // stop old timer
                    {
                        _thumbTrackTimer.Enabled = false;
                    }

                    // perform scroll immediately only if last refresh is very old
                    int currentThumbTrack = Environment.TickCount;
                    if (currentThumbTrack - _lastThumbtrack > THUMPTRACKDELAY)
                    {
                        PerformScrollThumbTrack(null, null);
                        _lastThumbtrack = currentThumbTrack;
                        break;
                    }

                    // start thumbtrack timer 
                    _thumbTrackPosition = FromScrollPos(e.NewValue);
                    _thumbTrackTimer.Enabled = true;
                    break;
                case ScrollEventType.First:
                    break;
                default:
                    break;
            }

            e.NewValue = ToScrollPos(_scrollVpos);
        }

        /// <summary>
        /// Performs the thumbtrack scrolling after an delay.
        /// </summary>
        private void PerformScrollThumbTrack(object sender, EventArgs e)
        {
            _thumbTrackTimer.Enabled = false;
            PerformScrollThumpPosition(_thumbTrackPosition);
            _lastThumbtrack = Environment.TickCount;
        }

        private void UpdateScrollSize()
        {
            // calc scroll bar info
            if (VScrollBarVisible && _byteProvider != null && _byteProvider.Length > 0 && _iHexMaxHBytes != 0)
            {
                long scrollmax =
                    (long) Math.Ceiling((_byteProvider.Length + 1) / (double) _iHexMaxHBytes - _iHexMaxVBytes);
                scrollmax = Math.Max(0, scrollmax);

                long scrollpos = _startByte / _iHexMaxHBytes;

                if (scrollmax < _scrollVmax)
                {
                    /* Data size has been decreased. */
                    if (_scrollVpos == _scrollVmax)
                    {
                        /* Scroll one line up if we at bottom. */
                        PerformScrollLineUp();
                    }
                }

                if (scrollmax == _scrollVmax && scrollpos == _scrollVpos)
                {
                    return;
                }

                _scrollVmin = 0;
                _scrollVmax = scrollmax;
                _scrollVpos = Math.Min(scrollpos, scrollmax);
                UpdateVScroll();
            }
            else if (VScrollBarVisible)
            {
                // disable scroll bar
                _scrollVmin = 0;
                _scrollVmax = 0;
                _scrollVpos = 0;
                UpdateVScroll();
            }
        }

        private void UpdateVScroll()
        {
            int max = ToScrollMax(_scrollVmax);

            if (max > 0)
            {
                _vScrollBar.Minimum = 0;
                _vScrollBar.Maximum = max;
                _vScrollBar.Value = ToScrollPos(_scrollVpos);
                _vScrollBar.Visible = true;
            }
            else
            {
                _vScrollBar.Visible = false;
            }
        }

        private int ToScrollPos(long value)
        {
            int max = 65535;

            if (_scrollVmax < max)
            {
                return (int) value;
            }

            double valperc = value / (double) _scrollVmax * 100;
            int res = (int) Math.Floor(max / (double) 100 * valperc);
            res = (int) Math.Max(_scrollVmin, res);
            res = (int) Math.Min(_scrollVmax, res);
            return res;
        }

        private long FromScrollPos(int value)
        {
            int max = 65535;
            if (_scrollVmax < max)
            {
                return value;
            }

            double valperc = value / (double) max * 100;
            long res = (int) Math.Floor(_scrollVmax / (double) 100 * valperc);
            return res;
        }

        private int ToScrollMax(long value)
        {
            long max = 65535;
            if (value > max)
            {
                return (int) max;
            }

            return (int) value;
        }

        private void PerformScrollToLine(long pos)
        {
            if (pos < _scrollVmin || pos > _scrollVmax || pos == _scrollVpos)
            {
                return;
            }

            _scrollVpos = pos;

            UpdateVScroll();
            UpdateVisibilityBytes();
            UpdateCaret();
            Invalidate();
        }

        private void PerformScrollLines(int lines)
        {
            long pos;
            if (lines > 0)
            {
                pos = Math.Min(_scrollVmax, _scrollVpos + lines);
            }
            else if (lines < 0)
            {
                pos = Math.Max(_scrollVmin, _scrollVpos + lines);
            }
            else
            {
                return;
            }

            PerformScrollToLine(pos);
        }

        private void PerformScrollLineDown()
        {
            PerformScrollLines(1);
        }

        private void PerformScrollLineUp()
        {
            PerformScrollLines(-1);
        }

        private void PerformScrollPageDown()
        {
            PerformScrollLines(_iHexMaxVBytes);
        }

        private void PerformScrollPageUp()
        {
            PerformScrollLines(-_iHexMaxVBytes);
        }

        private void PerformScrollThumpPosition(long pos)
        {
            // Bug fix: Scroll to end, do not scroll to end
            int difference = _scrollVmax > 65535 ? 10 : 9;

            if (ToScrollPos(pos) == ToScrollMax(_scrollVmax) - difference)
            {
                pos = _scrollVmax;
            }
            // End Bug fix


            PerformScrollToLine(pos);
        }

        /// <summary>
        /// Scrolls the selection start byte into view
        /// </summary>
        public void ScrollByteIntoView()
        {
            ScrollByteIntoView(_bytePos);
        }

        /// <summary>
        /// Scrolls the specific byte into view
        /// </summary>
        /// <param name="index">the index of the byte</param>
        public void ScrollByteIntoView(long index)
        {
            if (_byteProvider == null || _keyInterpreter == null)
            {
                return;
            }

            if (index < _startByte)
            {
                long line = (long) Math.Floor(index / (double) _iHexMaxHBytes);
                PerformScrollThumpPosition(line);
            }
            else if (index > _endByte)
            {
                long line = (long) Math.Floor(index / (double) _iHexMaxHBytes);
                line -= _iHexMaxVBytes - 1;
                PerformScrollThumpPosition(line);
            }
        }

        #endregion

        #region Selection methods

        private void ReleaseSelection()
        {
            if (_selectionLength == 0)
            {
                return;
            }

            _selectionLength = 0;
            OnSelectionLengthChanged(EventArgs.Empty);

            if (!_caretVisible)
            {
                CreateCaret();
            }
            else
            {
                UpdateCaret();
            }

            Invalidate();
        }

        /// <summary>
        /// Returns true if Select method could be invoked.
        /// </summary>
        public bool CanSelectAll()
        {
            if (!Enabled)
            {
                return false;
            }

            if (_byteProvider == null)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Selects all bytes.
        /// </summary>
        public void SelectAll()
        {
            if (ByteProvider == null)
            {
                return;
            }

            Select(0, ByteProvider.Length);
        }

        /// <summary>
        /// Selects the hex box.
        /// </summary>
        /// <param name="start">the start index of the selection</param>
        /// <param name="length">the length of the selection</param>
        public void Select(long start, long length)
        {
            if (ByteProvider == null)
            {
                return;
            }

            if (!Enabled)
            {
                return;
            }

            InternalSelect(start, length);
            ScrollByteIntoView();
        }

        private void InternalSelect(long start, long length)
        {
            long pos = start;
            long sel = length;
            int cp = 0;

            if (sel > 0 && _caretVisible)
            {
                DestroyCaret();
            }
            else if (sel == 0 && !_caretVisible)
            {
                CreateCaret();
            }

            SetPosition(pos, cp);
            SetSelectionLength(sel);

            UpdateCaret();
            Invalidate();
        }

        #endregion

        #region Key interpreter methods

        private void ActivateEmptyKeyInterpreter()
        {
            if (_eki == null)
            {
                _eki = new EmptyKeyInterpreter(this);
            }

            if (_eki == _keyInterpreter)
            {
                return;
            }

            _keyInterpreter?.Deactivate();

            _keyInterpreter = _eki;
            _keyInterpreter.Activate();
        }

        private void ActivateKeyInterpreter()
        {
            if (_ki == null)
            {
                _ki = new KeyInterpreter(this);
            }

            if (_ki == _keyInterpreter)
            {
                return;
            }

            _keyInterpreter?.Deactivate();

            _keyInterpreter = _ki;
            _keyInterpreter.Activate();
        }

        private void ActivateStringKeyInterpreter()
        {
            if (_ski == null)
            {
                _ski = new StringKeyInterpreter(this);
            }

            if (_ski == _keyInterpreter)
            {
                return;
            }

            _keyInterpreter?.Deactivate();

            _keyInterpreter = _ski;
            _keyInterpreter.Activate();
        }

        #endregion

        #region Caret methods

        private void CreateCaret()
        {
            if (_byteProvider == null || _keyInterpreter == null || _caretVisible || !Focused)
            {
                return;
            }

            // define the caret width depending on InsertActive mode
            int caretWidth = InsertActive ? 1 : (int) _charSize.Width;
            int caretHeight = (int) _charSize.Height;
            NativeMethods.CreateCaret(Handle, IntPtr.Zero, caretWidth, caretHeight);

            UpdateCaret();

            NativeMethods.ShowCaret(Handle);

            _caretVisible = true;
        }

        private void UpdateCaret()
        {
            if (_byteProvider == null || _keyInterpreter == null)
            {
                return;
            }

            long byteIndex = _bytePos - _startByte;
            PointF p = _keyInterpreter.GetCaretPointF(byteIndex);
            p.X += _byteCharacterPos * _charSize.Width;
            NativeMethods.SetCaretPos((int) p.X, (int) p.Y);
        }

        private void DestroyCaret()
        {
            if (!_caretVisible)
            {
                return;
            }

            NativeMethods.DestroyCaret();
            _caretVisible = false;
        }

        private bool inStringArea;

        private void SetCaretPosition(Point p)
        {
            if (_byteProvider == null || _keyInterpreter == null)
            {
                return;
            }

            long pos = _bytePos;
            int cp = _byteCharacterPos;

            if (_recHex.Contains(p))
            {
                inStringArea = false;

                BytePositionInfo bpi = GetHexBytePositionInfo(p);
                pos = bpi.Index;
                cp = bpi.CharacterPosition;

                SetPosition(pos, cp);

                ActivateKeyInterpreter();
                UpdateCaret();
                Invalidate();
            }
            else if (_recStringView.Contains(p))
            {
                inStringArea = true;

                BytePositionInfo bpi = GetStringBytePositionInfo(p);
                pos = bpi.Index;
                cp = bpi.CharacterPosition;

                SetPosition(pos, cp);

                ActivateStringKeyInterpreter();
                UpdateCaret();
                Invalidate();
            }
        }

        private BytePositionInfo GetHexBytePositionInfo(Point p)
        {
            long bytePos;
            int byteCharaterPos;

            float x = (p.X - _recHex.X) / _charSize.Width;
            float y = (p.Y - _recHex.Y) / _charSize.Height;
            int iX = (int) x;
            int iY = (int) y;

            int hPos = iX / 3 + 1;

            bytePos = Math.Min(_byteProvider.Length,
                _startByte + (_iHexMaxHBytes * (iY + 1) - _iHexMaxHBytes) + hPos - 1);
            byteCharaterPos = iX % 3;
            if (byteCharaterPos > 1)
            {
                byteCharaterPos = 1;
            }

            if (bytePos == _byteProvider.Length)
            {
                byteCharaterPos = 0;
            }

            if (bytePos < 0)
            {
                return new BytePositionInfo(0, 0);
            }

            return new BytePositionInfo(bytePos, byteCharaterPos);
        }

        private BytePositionInfo GetStringBytePositionInfo(Point p)
        {
            long bytePos;
            int byteCharacterPos;

            float x = (p.X - _recStringView.X) / _charSize.Width;
            float y = (p.Y - _recStringView.Y) / _charSize.Height;
            int iX = (int) x;
            int iY = (int) y;

            int hPos = iX + 1;

            bytePos = Math.Min(_byteProvider.Length,
                _startByte + (_iHexMaxHBytes * (iY + 1) - _iHexMaxHBytes) + hPos - 1);
            byteCharacterPos = 0;

            if (bytePos < 0)
            {
                return new BytePositionInfo(0, 0);
            }

            return new BytePositionInfo(bytePos, byteCharacterPos);
        }

        #endregion

        #region PreProcessMessage methods

        /// <summary>
        /// Preprocesses windows messages.
        /// </summary>
        /// <param name="m">the message to process.</param>
        /// <returns>true, if the message was processed</returns>
        [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
        [SecurityPermission(SecurityAction.InheritanceDemand, UnmanagedCode = true)]
        public override bool PreProcessMessage(ref Message m)
        {
            switch (m.Msg)
            {
                case NativeMethods.WM_KEYDOWN:
                    return _keyInterpreter.PreProcessWmKeyDown(ref m);
                case NativeMethods.WM_CHAR:
                    return _keyInterpreter.PreProcessWmChar(ref m);
                case NativeMethods.WM_KEYUP:
                    return _keyInterpreter.PreProcessWmKeyUp(ref m);
                default:
                    return base.PreProcessMessage(ref m);
            }
        }

        private bool BasePreProcessMessage(ref Message m)
        {
            return base.PreProcessMessage(ref m);
        }

        #endregion

        #region Find methods

        /// <summary>
        /// Searches the current ByteProvider
        /// </summary>
        /// <param name="options">contains all find options</param>
        /// <returns>the SelectionStart property value if find was successful or
        /// -1 if there is no match
        /// -2 if Find was aborted.</returns>
        public long FindNext(FindOptions options)
        {
            long startIndex = SelectionStart + SelectionLength;
            int match = 0;

            byte[] buffer1 = null;
            byte[] buffer2 = null;
            if (options.Type == FindType.Text && options.MatchCase)
            {
                if (options.FindBuffer == null || options.FindBuffer.Length == 0)
                {
                    throw new ArgumentException("FindBuffer can not be null when Type: Text and MatchCase: false");
                }

                buffer1 = options.FindBuffer;
            }
            else if (options.Type == FindType.Text && !options.MatchCase)
            {
                if (options.FindBufferLowerCase == null || options.FindBufferLowerCase.Length == 0)
                {
                    throw new ArgumentException(
                        "FindBufferLowerCase can not be null when Type is Text and MatchCase is true");
                }

                if (options.FindBufferUpperCase == null || options.FindBufferUpperCase.Length == 0)
                {
                    throw new ArgumentException(
                        "FindBufferUpperCase can not be null when Type is Text and MatchCase is true");
                }

                if (options.FindBufferLowerCase.Length != options.FindBufferUpperCase.Length)
                {
                    throw new ArgumentException(
                        "FindBufferUpperCase and FindBufferUpperCase must have the same size when Type is Text and MatchCase is true");
                }

                buffer1 = options.FindBufferLowerCase;
                buffer2 = options.FindBufferUpperCase;
            }
            else if (options.Type == FindType.Hex)
            {
                if (options.Hex == null || options.Hex.Length == 0)
                {
                    return -2;
                    //throw new ArgumentException("Hex can not be null when Type is Hex");
                }

                buffer1 = options.Hex;
            }
            else if (options.Type == FindType.Annotations)
            {
                return FindNextAnnotations(options);
            }

            int buffer1Length = buffer1.Length;

            _abortFind = false;

            for (long pos = startIndex; pos < _byteProvider.Length; pos++)
            {
                if (_abortFind)
                {
                    return -2;
                }

                if (pos % 1000 == 0) // for performance reasons: DoEvents only 1 times per 1000 loops
                {
                    Application.DoEvents();
                }

                byte compareByte = _byteProvider.ReadByte(pos);
                bool buffer1Match = compareByte == buffer1[match];
                bool hasBuffer2 = buffer2 != null;
                bool buffer2Match = hasBuffer2 ? compareByte == buffer2[match] : false;
                bool isMatch = buffer1Match || buffer2Match;
                if (!isMatch)
                {
                    pos -= match;
                    match = 0;
                    _findingPos = pos;
                    continue;
                }

                match++;

                if (match == buffer1Length)
                {
                    long bytePos = pos - buffer1Length + 1;
                    Select(bytePos, buffer1Length);
                    ScrollByteIntoView(_bytePos + _selectionLength);
                    ScrollByteIntoView(_bytePos);

                    return bytePos;
                }
            }

            return -1;
        }

        /// <summary>
        /// Searches the current ByteProvider
        /// </summary>
        /// <param name="options">contains all find options</param>
        /// <returns>the SelectionStart property value if find was successful or
        /// -1 if there is no match
        /// -2 if Find was aborted.</returns>
        public long FindPrev(FindOptions options)
        {
            long startIndex = SelectionStart - 1;
            int match = 0;

            byte[] buffer1 = null;
            byte[] buffer2 = null;
            if (options.Type == FindType.Text && options.MatchCase)
            {
                if (options.FindBuffer == null || options.FindBuffer.Length == 0)
                {
                    throw new ArgumentException("FindBuffer can not be null when Type: Text and MatchCase: false");
                }

                buffer1 = options.FindBuffer;
            }
            else if (options.Type == FindType.Text && !options.MatchCase)
            {
                if (options.FindBufferLowerCase == null || options.FindBufferLowerCase.Length == 0)
                {
                    throw new ArgumentException(
                        "FindBufferLowerCase can not be null when Type is Text and MatchCase is true");
                }

                if (options.FindBufferUpperCase == null || options.FindBufferUpperCase.Length == 0)
                {
                    throw new ArgumentException(
                        "FindBufferUpperCase can not be null when Type is Text and MatchCase is true");
                }

                if (options.FindBufferLowerCase.Length != options.FindBufferUpperCase.Length)
                {
                    throw new ArgumentException(
                        "FindBufferUpperCase and FindBufferUpperCase must have the same size when Type is Text and MatchCase is true");
                }

                buffer1 = options.FindBufferLowerCase;
                buffer2 = options.FindBufferUpperCase;
            }
            else if (options.Type == FindType.Hex)
            {
                if (options.Hex == null || options.Hex.Length == 0)
                {
                    return -2;
                    //throw new ArgumentException("Hex can not be null when Type is Hex");
                }

                buffer1 = options.Hex;
            }
            else if (options.Type == FindType.Annotations)
            {
                return FindPrevAnnotations(options);
            }

            int buffer1Length = buffer1.Length;

            _abortFind = false;

            for (long pos = startIndex; pos >= 0; pos--)
            {
                if (_abortFind)
                {
                    return -2;
                }

                if (pos % 1000 == 0) // for performance reasons: DoEvents only 1 times per 1000 loops
                {
                    Application.DoEvents();
                }

                byte compareByte = _byteProvider.ReadByte(pos);
                bool buffer1Match = compareByte == buffer1[buffer1.Length - 1 - match];
                bool hasBuffer2 = buffer2 != null;
                bool buffer2Match = hasBuffer2 ? compareByte == buffer2[buffer2.Length - 1 - match] : false;
                bool isMatch = buffer1Match || buffer2Match;
                if (!isMatch)
                {
                    pos += match;
                    match = 0;
                    _findingPos = pos;
                    continue;
                }

                match++;

                if (match == buffer1Length)
                {
                    long bytePos = pos;
                    Select(bytePos, buffer1Length);
                    ScrollByteIntoView(_bytePos + _selectionLength);
                    ScrollByteIntoView(_bytePos);

                    return bytePos;
                }
            }

            return -1;
        }

        public long FindNextAnnotations(FindOptions options)
        {
            long startIndex = SelectionStart + SelectionLength;
            _abortFind = false;

            for (long pos = startIndex.RoundUp(4); pos < _byteProvider.Length; pos += 4)
            {
                if (_abortFind)
                {
                    return -2;
                }

                if (pos % 1000 == 0) // for performance reasons: DoEvents only 1 times per 1000 loops
                {
                    Application.DoEvents();
                }

                if (annotationDescriptions.Count > pos / 4)
                {
                    if (options.MatchCase)
                    {
                        if (annotationDescriptions[(int) (pos / 4)]
                            .Contains(Encoding.Default.GetString(options.FindBuffer)))
                        {
                            int posOffset = 0;
                            int byteCount = 4;
                            if (SectionEditor != null)
                            {
                                if (annotationUnderlines[(int) (pos / 4)].StartsWith("0000") ||
                                    annotationUnderlines[(int) (pos / 4)].StartsWith("1111") ||
                                    annotationUnderlines[(int) (pos / 4)].StartsWith("011"))
                                {
                                    SectionEditor.rdo4byte.Checked = true;
                                }
                                else
                                {
                                    bool firstFound = false;
                                    int numBytes = 0;
                                    numBytes += annotationUnderlines[(int) (pos / 4)].StartsWith("1") ? 1 : 0;
                                    if (numBytes > 0 && !firstFound)
                                    {
                                        firstFound = true;
                                    }

                                    numBytes += annotationUnderlines[(int) (pos / 4)].Substring(1).StartsWith("1")
                                        ? 1
                                        : 0;
                                    if (numBytes > 0 && !firstFound)
                                    {
                                        firstFound = true;
                                        posOffset = 1;
                                    }

                                    numBytes += annotationUnderlines[(int) (pos / 4)].Substring(2).StartsWith("1")
                                        ? 1
                                        : 0;
                                    if (numBytes > 0 && !firstFound)
                                    {
                                        firstFound = true;
                                        posOffset = 2;
                                    }

                                    numBytes += annotationUnderlines[(int) (pos / 4)].Substring(3).StartsWith("1")
                                        ? 1
                                        : 0;
                                    if (numBytes > 0 && !firstFound)
                                    {
                                        firstFound = true;
                                        posOffset = 3;
                                    }

                                    byteCount -= posOffset;
                                    if (numBytes == 2 && annotationUnderlines[(int) (pos / 4)].Substring(0, 4)
                                        .Contains("11"))
                                    {
                                        SectionEditor.rdo2byte.Checked = true;
                                        byteCount = 2;
                                    }
                                    else if (numBytes == 1)
                                    {
                                        SectionEditor.rdo1byte.Checked = true;
                                        byteCount = 1;
                                    }
                                    else
                                    {
                                        SectionEditor.rdo4byte.Checked = true;
                                        posOffset = 0;
                                        byteCount = 4;
                                    }
                                }
                            }

                            Select(pos + posOffset, byteCount);
                            ScrollByteIntoView(pos + _selectionLength);
                            ScrollByteIntoView(pos);
                            return pos;
                        }
                    }
                    else if (annotationDescriptions[(int) (pos / 4)].Contains(
                        Encoding.Default.GetString(options.FindBuffer), StringComparison.CurrentCultureIgnoreCase))
                    {
                        int posOffset = 0;
                        int byteCount = 4;
                        if (SectionEditor != null)
                        {
                            if (annotationUnderlines[(int) (pos / 4)].StartsWith("0000") ||
                                annotationUnderlines[(int) (pos / 4)].StartsWith("1111") ||
                                annotationUnderlines[(int) (pos / 4)].StartsWith("011"))
                            {
                                SectionEditor.rdo4byte.Checked = true;
                            }
                            else
                            {
                                bool firstFound = false;
                                int numBytes = 0;
                                numBytes += annotationUnderlines[(int) (pos / 4)].StartsWith("1") ? 1 : 0;
                                if (numBytes > 0 && !firstFound)
                                {
                                    firstFound = true;
                                }

                                numBytes += annotationUnderlines[(int) (pos / 4)].Substring(1).StartsWith("1") ? 1 : 0;
                                if (numBytes > 0 && !firstFound)
                                {
                                    firstFound = true;
                                    posOffset = 1;
                                }

                                numBytes += annotationUnderlines[(int) (pos / 4)].Substring(2).StartsWith("1") ? 1 : 0;
                                if (numBytes > 0 && !firstFound)
                                {
                                    firstFound = true;
                                    posOffset = 2;
                                }

                                numBytes += annotationUnderlines[(int) (pos / 4)].Substring(3).StartsWith("1") ? 1 : 0;
                                if (numBytes > 0 && !firstFound)
                                {
                                    firstFound = true;
                                    posOffset = 3;
                                }

                                byteCount -= posOffset;
                                if (numBytes == 2 && annotationUnderlines[(int) (pos / 4)].Substring(0, 4)
                                    .Contains("11"))
                                {
                                    SectionEditor.rdo2byte.Checked = true;
                                    byteCount = 2;
                                }
                                else if (numBytes == 1)
                                {
                                    SectionEditor.rdo1byte.Checked = true;
                                    byteCount = 1;
                                }
                                else
                                {
                                    SectionEditor.rdo4byte.Checked = true;
                                    posOffset = 0;
                                    byteCount = 4;
                                }
                            }
                        }

                        Select(pos + posOffset, byteCount);
                        ScrollByteIntoView(pos + _selectionLength);
                        ScrollByteIntoView(pos);
                        return pos;
                    }
                }
            }

            return -1;
        }

        public long FindPrevAnnotations(FindOptions options)
        {
            long startIndex = SelectionStart;
            _abortFind = false;

            for (long pos = startIndex.RoundDown(4) - 4; pos > 0; pos -= 4)
            {
                if (_abortFind)
                {
                    return -2;
                }

                if (pos % 1000 == 0) // for performance reasons: DoEvents only 1 times per 1000 loops
                {
                    Application.DoEvents();
                }

                if (annotationDescriptions.Count > pos / 4)
                {
                    if (options.MatchCase)
                    {
                        if (annotationDescriptions[(int) (pos / 4)]
                            .Contains(Encoding.Default.GetString(options.FindBuffer)))
                        {
                            int posOffset = 0;
                            int byteCount = 4;
                            if (SectionEditor != null)
                            {
                                if (annotationUnderlines[(int) (pos / 4)].StartsWith("0000") ||
                                    annotationUnderlines[(int) (pos / 4)].StartsWith("1111") ||
                                    annotationUnderlines[(int) (pos / 4)].StartsWith("011"))
                                {
                                    SectionEditor.rdo4byte.Checked = true;
                                }
                                else
                                {
                                    bool firstFound = false;
                                    int numBytes = 0;
                                    numBytes += annotationUnderlines[(int) (pos / 4)].StartsWith("1") ? 1 : 0;
                                    if (numBytes > 0 && !firstFound)
                                    {
                                        firstFound = true;
                                    }

                                    numBytes += annotationUnderlines[(int) (pos / 4)].Substring(1).StartsWith("1")
                                        ? 1
                                        : 0;
                                    if (numBytes > 0 && !firstFound)
                                    {
                                        firstFound = true;
                                        posOffset = 1;
                                    }

                                    numBytes += annotationUnderlines[(int) (pos / 4)].Substring(2).StartsWith("1")
                                        ? 1
                                        : 0;
                                    if (numBytes > 0 && !firstFound)
                                    {
                                        firstFound = true;
                                        posOffset = 2;
                                    }

                                    numBytes += annotationUnderlines[(int) (pos / 4)].Substring(3).StartsWith("1")
                                        ? 1
                                        : 0;
                                    if (numBytes > 0 && !firstFound)
                                    {
                                        firstFound = true;
                                        posOffset = 3;
                                    }

                                    byteCount -= posOffset;
                                    if (numBytes == 2 && annotationUnderlines[(int) (pos / 4)].Substring(0, 4)
                                        .Contains("11"))
                                    {
                                        SectionEditor.rdo2byte.Checked = true;
                                        byteCount = 2;
                                    }
                                    else if (numBytes == 1)
                                    {
                                        SectionEditor.rdo1byte.Checked = true;
                                        byteCount = 1;
                                    }
                                    else
                                    {
                                        SectionEditor.rdo4byte.Checked = true;
                                        posOffset = 0;
                                        byteCount = 4;
                                    }
                                }
                            }

                            Select(pos + posOffset, byteCount);
                            ScrollByteIntoView(pos + _selectionLength);
                            ScrollByteIntoView(pos);
                            return pos;
                        }
                    }
                    else if (annotationDescriptions[(int) (pos / 4)].Contains(
                        Encoding.Default.GetString(options.FindBuffer), StringComparison.CurrentCultureIgnoreCase))
                    {
                        int posOffset = 0;
                        int byteCount = 4;
                        if (SectionEditor != null)
                        {
                            if (annotationUnderlines[(int) (pos / 4)].StartsWith("0000") ||
                                annotationUnderlines[(int) (pos / 4)].StartsWith("1111") ||
                                annotationUnderlines[(int) (pos / 4)].StartsWith("011"))
                            {
                                SectionEditor.rdo4byte.Checked = true;
                            }
                            else
                            {
                                bool firstFound = false;
                                int numBytes = 0;
                                numBytes += annotationUnderlines[(int) (pos / 4)].StartsWith("1") ? 1 : 0;
                                if (numBytes > 0 && !firstFound)
                                {
                                    firstFound = true;
                                }

                                numBytes += annotationUnderlines[(int) (pos / 4)].Substring(1).StartsWith("1") ? 1 : 0;
                                if (numBytes > 0 && !firstFound)
                                {
                                    firstFound = true;
                                    posOffset = 1;
                                }

                                numBytes += annotationUnderlines[(int) (pos / 4)].Substring(2).StartsWith("1") ? 1 : 0;
                                if (numBytes > 0 && !firstFound)
                                {
                                    firstFound = true;
                                    posOffset = 2;
                                }

                                numBytes += annotationUnderlines[(int) (pos / 4)].Substring(3).StartsWith("1") ? 1 : 0;
                                if (numBytes > 0 && !firstFound)
                                {
                                    firstFound = true;
                                    posOffset = 3;
                                }

                                byteCount -= posOffset;
                                if (numBytes == 2 && annotationUnderlines[(int) (pos / 4)].Substring(0, 4)
                                    .Contains("11"))
                                {
                                    SectionEditor.rdo2byte.Checked = true;
                                    byteCount = 2;
                                }
                                else if (numBytes == 1)
                                {
                                    SectionEditor.rdo1byte.Checked = true;
                                    byteCount = 1;
                                }
                                else
                                {
                                    SectionEditor.rdo4byte.Checked = true;
                                    posOffset = 0;
                                    byteCount = 4;
                                }
                            }
                        }

                        Select(pos + posOffset, byteCount);
                        ScrollByteIntoView(pos + _selectionLength);
                        ScrollByteIntoView(pos);
                        return pos;
                    }
                }
            }

            return -1;
        }

        /// <summary>
        /// Aborts a working Find method.
        /// </summary>
        public void AbortFind()
        {
            _abortFind = true;
        }

        /// <summary>
        /// Gets a value that indicates the current position during Find method execution.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public long CurrentFindingPosition => _findingPos;

        #endregion

        #region Copy, Cut and Paste methods

        private byte[] GetCopyData()
        {
            if (!CanCopy())
            {
                return new byte[0];
            }

            // put bytes into buffer
            byte[] buffer = new byte[_selectionLength];
            int id = -1;
            for (long i = _bytePos; i < _bytePos + _selectionLength; i++)
            {
                id++;

                buffer[id] = _byteProvider.ReadByte(i);
            }

            return buffer;
        }

        /// <summary>
        /// Copies the current selection in the hex box to the Clipboard.
        /// </summary>
        public void Copy()
        {
            if (!CanCopy())
            {
                return;
            }

            // put bytes into buffer
            byte[] buffer = GetCopyData();

            string text;
            if (inStringArea)
            {
                text = Encoding.ASCII.GetString(buffer, 0, buffer.Length);
            }
            else
            {
                string s = "";
                bool first = true;
                int i = (int)(SelectionStart % 4);

                foreach (byte b in buffer)
                {
                    if (first)
                    {
                        first = false;
                    }
                    else if (i == 4)
                    {
                        s += " ";
                        i = 0;
                    }

                    s += b.ToString("X2");
                    i++;
                }

                text = s;
            }

            Clipboard.SetText(text);
            UpdateCaret();
            ScrollByteIntoView();
            Invalidate();

            OnCopied(EventArgs.Empty);
        }

        /// <summary>
        /// Return true if Copy method could be invoked.
        /// </summary>
        public bool CanCopy()
        {
            if (_selectionLength < 1 || _byteProvider == null)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Moves the current selection in the hex box to the Clipboard.
        /// </summary>
        public void Cut()
        {
            if (!CanCut())
            {
                return;
            }

            Copy();
            Delete();
        }

        /// <summary>
        /// Deletes the current selection in the hex box.
        /// </summary>
        public void Delete()
        {
            if (!CanCut())
            {
                return;
            }

            _byteProvider.DeleteBytes(_bytePos, _selectionLength);
            _byteCharacterPos = 0;
            UpdateCaret();
            ScrollByteIntoView();
            ReleaseSelection();
            Invalidate();
            Refresh();
        }

        /// <summary>
        /// Return true if Cut method could be invoked.
        /// </summary>
        public bool CanCut()
        {
            if (ReadOnly || !Enabled)
            {
                return false;
            }

            if (_byteProvider == null)
            {
                return false;
            }

            if (_selectionLength < 1 || !_byteProvider.SupportsDeleteBytes())
            {
                return false;
            }

            return true;
        }

        private static readonly char[] hexChars = "0123456789abcdefABCDEF".ToCharArray();

        /// <summary>
        /// Replaces the current selection in the hex box with the contents of the Clipboard.
        /// </summary>
        public void Paste()
        {
            if (!CanPaste())
            {
                return;
            }

            byte[] buffer = null;
            string s = Clipboard.GetText();

            if (string.IsNullOrEmpty(s))
            {
                return;
            }

            if (inStringArea)
            {
                buffer = Encoding.ASCII.GetBytes(s);
            }
            else
            {
                List<byte> b = new List<byte>();
                string byteString = "";
                foreach (char c in s)
                {
                    if (Array.IndexOf(hexChars, c) != -1)
                    {
                        byteString += c;
                    }

                    if (byteString.Length == 2)
                    {
                        b.Add(byte.Parse(byteString, NumberStyles.HexNumber, CultureInfo.InvariantCulture));
                        byteString = "";
                    }
                }

                buffer = b.ToArray();
            }

            if (_byteProvider is DynamicFileByteProvider)
            {
                ((DynamicFileByteProvider) _byteProvider)._supportsInsDel = true;
                _byteProvider.DeleteBytes(_bytePos, buffer.Length);
                _byteProvider.InsertBytes(_bytePos, buffer);
                ((DynamicFileByteProvider) _byteProvider)._supportsInsDel = false;
            }
            else if (_byteProvider is DynamicByteProvider)
            {
                ((DynamicByteProvider) _byteProvider)._supportsInsDel = true;
                _byteProvider.DeleteBytes(_bytePos, buffer.Length);
                _byteProvider.InsertBytes(_bytePos, buffer);
                ((DynamicByteProvider) _byteProvider)._supportsInsDel = false;
            }
            else
            {
                return;
            }

            SetPosition(_bytePos + buffer.Length, 0);

            ReleaseSelection();
            ScrollByteIntoView();
            UpdateCaret();
            Invalidate();
        }

        /// <summary>
        /// Return true if Paste method could be invoked.
        /// </summary>
        public bool CanPaste()
        {
            if (ReadOnly || !Enabled)
            {
                return false;
            }

            //if (_byteProvider == null || !_byteProvider.SupportsInsertBytes())
            //    return false;

            //if (!_byteProvider.SupportsDeleteBytes() && _selectionLength > 0)
            //    return false;

            if (!string.IsNullOrEmpty(Clipboard.GetText()))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Return true if PasteHex method could be invoked.
        /// </summary>
        public bool CanPasteHex()
        {
            if (!CanPaste())
            {
                return false;
            }

            byte[] buffer = null;
            IDataObject da = Clipboard.GetDataObject();
            if (da.GetDataPresent(typeof(string)))
            {
                string hexString = (string) da.GetData(typeof(string));
                buffer = ConvertHexToBytes(hexString);
                return buffer != null;
            }

            return false;
        }

        /// <summary>
        /// Replaces the current selection in the hex box with the hex string data of the Clipboard.
        /// </summary>
        public void PasteHex(bool overwrite)
        {
            if (!CanPaste())
            {
                return;
            }

            byte[] buffer = null;
            IDataObject da = Clipboard.GetDataObject();
            if (da.GetDataPresent(typeof(string)))
            {
                string hexString = (string) da.GetData(typeof(string));
                buffer = ConvertHexToBytes(hexString);
                if (buffer == null)
                {
                    MessageBox.Show(this, "Clipboard data contains invalid hex values.", "Problem",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            else
            {
                return;
            }

            if (overwrite)
            {
                long length = Math.Max(_selectionLength, buffer.Length);

                if (length > 0)
                {
                    _byteProvider.DeleteBytes(_bytePos, length);
                }
            }
            else if (_selectionLength > 0)
            {
                _byteProvider.DeleteBytes(_bytePos, _selectionLength);
            }

            _byteProvider.InsertBytes(_bytePos, buffer);
            SetPosition(_bytePos + buffer.Length, 0);

            ReleaseSelection();
            ScrollByteIntoView();
            UpdateCaret();
            Invalidate();
        }

        /// <summary>
        /// Copies the current selection in the hex box to the Clipboard in hex format.
        /// </summary>
        public void CopyHex()
        {
            if (!CanCopy())
            {
                return;
            }

            // put bytes into buffer
            byte[] buffer = GetCopyData();

            DataObject da = new DataObject();

            // set string buffer clipbard data
            string hexString = ConvertBytesToHex(buffer);
            da.SetData(typeof(string), hexString);

            //set memorystream (BinaryData) clipboard data
            System.IO.MemoryStream ms = new System.IO.MemoryStream(buffer, 0, buffer.Length, false, true);
            da.SetData("BinaryData", ms);

            Clipboard.SetDataObject(da, true);
            UpdateCaret();
            ScrollByteIntoView();
            Invalidate();

            OnCopiedHex(EventArgs.Empty);
        }

        #endregion

        #region Paint methods

        /// <summary>
        /// Paints the background.
        /// </summary>
        /// <param name="e">A PaintEventArgs that contains the event data.</param>
        protected override void OnPaintBackground(PaintEventArgs e)
        {
            switch (_borderStyle)
            {
                case BorderStyle.Fixed3D:
                {
                    if (TextBoxRenderer.IsSupported)
                    {
                        VisualStyleElement state = VisualStyleElement.TextBox.TextEdit.Normal;
                        Color backColor = BackColor;

                        if (Enabled)
                        {
                            if (ReadOnly)
                            {
                                state = VisualStyleElement.TextBox.TextEdit.ReadOnly;
                            }
                            else if (Focused)
                            {
                                state = VisualStyleElement.TextBox.TextEdit.Focused;
                            }
                        }
                        else
                        {
                            state = VisualStyleElement.TextBox.TextEdit.Disabled;
                            backColor = BackColorDisabled;
                        }

                        VisualStyleRenderer vsr = new VisualStyleRenderer(state);
                        vsr.DrawBackground(e.Graphics, ClientRectangle);

                        Rectangle rectContent = vsr.GetBackgroundContentRectangle(e.Graphics, ClientRectangle);
                        e.Graphics.FillRectangle(new SolidBrush(backColor), rectContent);
                    }
                    else
                    {
                        // draw background
                        e.Graphics.FillRectangle(new SolidBrush(BackColor), ClientRectangle);

                        // draw default border
                        ControlPaint.DrawBorder3D(e.Graphics, ClientRectangle, Border3DStyle.Sunken);
                    }

                    break;
                }

                case BorderStyle.FixedSingle:
                {
                    // draw background
                    e.Graphics.FillRectangle(new SolidBrush(BackColor), ClientRectangle);

                    // draw fixed single border
                    ControlPaint.DrawBorder(e.Graphics, ClientRectangle, Color.Black, ButtonBorderStyle.Solid);
                    break;
                }

                default:
                {
                    // draw background
                    e.Graphics.FillRectangle(new SolidBrush(BackColor), ClientRectangle);
                    break;
                }
            }
        }


        /// <summary>
        /// Paints the hex box.
        /// </summary>
        /// <param name="e">A PaintEventArgs that contains the event data.</param>
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            if (_byteProvider == null)
            {
                return;
            }

            // draw only in the content rectangle, so exclude the border and the scrollbar.
            Region r = new Region(ClientRectangle);
            r.Exclude(_recContent);
            e.Graphics.ExcludeClip(r);

            UpdateVisibilityBytes();

            if (_lineInfoVisible)
            {
                PaintLineInfo(e.Graphics, _startByte, _endByte);
            }

            PaintHexAndStringView(e.Graphics, _startByte, _endByte);

            if (_columnInfoVisible)
            {
                PaintHeaderRow(e.Graphics);
            }

            if (_groupSeparatorVisible)
            {
                PaintColumnSeparator(e.Graphics);
            }
        }

        private void PaintLineInfo(Graphics g, long startByte, long endByte)
        {
            // Ensure endByte isn't > length of array.
            endByte = Math.Min(_byteProvider.Length - 1, endByte);

            Color lineInfoColor = InfoForeColor != Color.Empty ? InfoForeColor : ForeColor;
            Brush brush = new SolidBrush(lineInfoColor);

            int maxLine = GetGridBytePoint(endByte - startByte).Y + 1;

            for (int i = 0; i < maxLine; i++)
            {
                long firstLineByte = startByte + _iHexMaxHBytes * i + _lineInfoOffset;

                PointF bytePointF = GetBytePointF(new Point(0, i));
                string info = firstLineByte.ToString(_hexStringFormat,
                    System.Threading.Thread.CurrentThread.CurrentCulture);
                int nulls = 8 - info.Length;
                string formattedInfo;
                if (nulls > -1)
                {
                    formattedInfo = new string('0', 8 - info.Length) + info;
                }
                else
                {
                    formattedInfo = new string('~', 8);
                }

                g.DrawString(formattedInfo, Font, brush, new PointF(_recLineInfo.X, bytePointF.Y), _stringFormat);
            }
        }

        public enum OffsetDisplay
        {
            Hex,
            Decimal
        }

        private readonly OffsetDisplay _offsetDisplay = OffsetDisplay.Hex;

        private void PaintHeaderRow(Graphics g)
        {
            Brush brush = new SolidBrush(InfoForeColor);
            PointF headerPointF = new PointF(_recLineInfo.X, _recColumnInfo.Y);

            g.DrawString($"Offset({(_offsetDisplay == OffsetDisplay.Hex ? "h" : "d")})", Font, brush,
                headerPointF, _stringFormat);

            for (int col = 0; col < _iHexMaxHBytes; col++)
            {
                PaintColumnInfo(g, (byte) col, brush, col);
            }
        }

        private void PaintColumnSeparator(Graphics g)
        {
            for (int col = GroupSize; col < _iHexMaxHBytes; col += GroupSize)
            {
                Pen pen = new Pen(new SolidBrush(ColumnDividerColor), 1);
                PointF headerPointF = GetColumnInfoPointF(col);
                headerPointF.X -= _charSize.Width / 2;
                g.DrawLine(pen, headerPointF,
                    new PointF(headerPointF.X, headerPointF.Y + _recColumnInfo.Height + _recHex.Height));
                //if (StringViewVisible)
                //{
                //    PointF byteStringPointF = GetByteStringPointF(new Point(col, 0));
                //    headerPointF.X -= 2;
                //    g.DrawLine(pen, new PointF(byteStringPointF.X, byteStringPointF.Y), new PointF(byteStringPointF.X, byteStringPointF.Y + _recHex.Height));
                //}
            }
        }

        private RelCommand GetBrushes(int index, ref Brush foreColor, ref Brush backColor, bool allowSelection)
        {
            //bool specialFunc = false; //_prolog || _epilog || _unresolved

            ModuleSectionNode s = _sectionEditor._section;
            List<RelocationTarget> linked = s._manager.GetLinked(index);
            List<RelocationTarget> branched = s._manager.GetBranched(index);

            if (linked != null && linked.Count > 0)
            {
                if (branched != null && branched.Count > 0)
                {
                    foreColor = new SolidBrush(Color.FromArgb(127, 50, 200));
                }
                else
                {
                    foreColor = RelocationBrush;
                }
            }
            else
            {
                if (branched != null && branched.Count > 0)
                {
                    foreColor = BlueBrush;
                }
                else
                {
                    foreColor = BlackBrush;
                }
            }

            RelCommand command = s._manager.GetCommand(index);
            bool cmd = command != null && !command.IsHalf;

            backColor =
                _sectionEditor.SelectedRelocationIndex == index && allowSelection ? SelectedBrush :
                cmd ? CommandBrush :
                null;

            if (backColor == null && s.HasCode)
            {
                PPCOpCode code = s._manager.GetCode(index);
                bool returnBranch = code is PPCblr;
                bool branch = code is PPCBranch && !returnBranch;
                bool linkedBranch =
                    _sectionEditor.TargetBranchOffsetRelocation != null &&
                    _sectionEditor.TargetBranchOffsetRelocation._index == index;

                backColor =
                    linkedBranch ? LinkedBranchBrush :
                    returnBranch ? BlrBrush :
                    branch ? BranchOffsetBrush :
                    null;
            }

            if (backColor == null)
            {
                backColor = _sectionEditor._manager.GetColor(index);
            }

            return command;
        }

        private void PaintByte(byte b, long offset, bool isSelectedByte, bool isKeyInterpreterActive, Graphics g,
                               Brush foreBrush, Brush backBrush, Point gridPoint)
        {
            if (isSelectedByte && isKeyInterpreterActive)
            {
                PaintHexStringSelected(g, b, offset, SelectionForeBrush, SelectionBackBrush, gridPoint);
            }
            else if (backBrush != null)
            {
                PaintHexStringSelected(g, b, offset, foreBrush, backBrush, gridPoint);
            }
            else
            {
                PaintHexString(g, b, offset, foreBrush, gridPoint);
            }
        }

        private void PaintHexString(Graphics g, byte b, long offset, Brush brush, Point gridPoint)
        {
            PointF bytePointF = GetBytePointF(gridPoint);

            string sB = ConvertByteToHex(b);
            Font tempFont = Font;

            if (annotationDescriptions != null && annotationDescriptions.Count > (int) (offset / 4))
            {
                if (!annotationDescriptions[(int) (offset / 4)].StartsWith("Default: 0x") &&
                    annotationUnderlines[(int) (offset / 4)].Substring((int) (offset % 4)).StartsWith("1") &&
                    annotationDescriptions[(int) (offset / 4)].Length > 0)
                {
                    tempFont = new Font(Font, FontStyle.Bold | FontStyle.Italic | FontStyle.Underline);
                }
            }

            int winVersion = -1;
            try
            {
                if (Environment.OSVersion.ToString().StartsWith("Microsoft Windows NT "))
                {
                    int.TryParse(
                        Environment.OSVersion.ToString()
                            .Substring(Environment.OSVersion.ToString().LastIndexOf(" ") + 1,
                                Environment.OSVersion.ToString().IndexOf(".") -
                                (Environment.OSVersion.ToString().LastIndexOf(" ") + 1)), out winVersion);
                }
            }
            catch
            {
                winVersion = -1;
            }

            g.DrawString(sB.Substring(0, 1), tempFont, brush,
                new PointF(bytePointF.X,
                    bytePointF.Y +
                    (Environment.OSVersion.ToString() == "Microsoft Windows NT 6.2.9200.0" || winVersion >= 10
                        ? sB.Substring(0, 1) == "A" ||
                          tempFont.Italic && !(sB.Substring(0, 1) == "1" || sB.Substring(0, 1) == "4") ? 2 : 0
                        : 0)), _stringFormat);
            bytePointF.X += _charSize.Width;
            g.DrawString(sB.Substring(1, 1), tempFont, brush,
                new PointF(bytePointF.X,
                    bytePointF.Y +
                    (Environment.OSVersion.ToString() == "Microsoft Windows NT 6.2.9200.0" || winVersion >= 10
                        ? sB.Substring(1, 1) == "A" ||
                          tempFont.Italic && !(sB.Substring(1, 1) == "1" || sB.Substring(1, 1) == "4") ? 2 : 0
                        : 0)), _stringFormat);
        }

        private void PaintColumnInfo(Graphics g, byte b, Brush brush, int col)
        {
            PointF headerPointF = GetColumnInfoPointF(col);

            string sB = ConvertByteToHex(b);

            g.DrawString(sB.Substring(0, 1), Font, brush, headerPointF, _stringFormat);
            headerPointF.X += _charSize.Width;
            g.DrawString(sB.Substring(1, 1), Font, brush, headerPointF, _stringFormat);
        }

        private void PaintHexStringSelected(Graphics g, byte b, long offset, Brush brush, Brush brushBack,
                                            Point gridPoint)
        {
            string sB = b.ToString(_hexStringFormat, System.Threading.Thread.CurrentThread.CurrentCulture);
            if (sB.Length == 1)
            {
                sB = "0" + sB;
            }

            PointF bytePointF = GetBytePointF(gridPoint);

            bool isLastLineChar = gridPoint.X + 1 == _iHexMaxHBytes;
            bool isFirstLineChar = gridPoint.X == 0;
            float bcWidth = isLastLineChar ? _charSize.Width * 2.3f : _charSize.Width * 3;
            float t = isFirstLineChar ? 0 : 3;
            Font tempFont = Font;

            if (annotationDescriptions != null && annotationDescriptions.Count > (int) (offset / 4))
            {
                if (!annotationDescriptions[(int) (offset / 4)].StartsWith("Default: 0x") &&
                    annotationUnderlines[(int) (offset / 4)].Substring((int) (offset % 4)).StartsWith("1") &&
                    annotationDescriptions[(int) (offset / 4)].Length > 0)
                {
                    tempFont = new Font(Font, FontStyle.Bold | FontStyle.Italic | FontStyle.Underline);
                }
            }

            int winVersion = -1;
            try
            {
                if (Environment.OSVersion.ToString().StartsWith("Microsoft Windows NT "))
                {
                    int.TryParse(
                        Environment.OSVersion.ToString()
                            .Substring(Environment.OSVersion.ToString().LastIndexOf(" ") + 1,
                                Environment.OSVersion.ToString().IndexOf(".") -
                                (Environment.OSVersion.ToString().LastIndexOf(" ") + 1)), out winVersion);
                }
            }
            catch
            {
                winVersion = -1;
            }

            g.FillRectangle(brushBack, bytePointF.X - t, bytePointF.Y, bcWidth, _charSize.Height);
            g.DrawString(sB.Substring(0, 1), tempFont, brush,
                new PointF(bytePointF.X,
                    bytePointF.Y +
                    (Environment.OSVersion.ToString() == "Microsoft Windows NT 6.2.9200.0" || winVersion >= 10
                        ? sB.Substring(0, 1) == "A" ||
                          tempFont.Italic && !(sB.Substring(0, 1) == "1" || sB.Substring(0, 1) == "4") ? 2 : 0
                        : 0)), _stringFormat);
            bytePointF.X += _charSize.Width;
            g.DrawString(sB.Substring(1, 1), tempFont, brush,
                new PointF(bytePointF.X,
                    bytePointF.Y +
                    (Environment.OSVersion.ToString() == "Microsoft Windows NT 6.2.9200.0" || winVersion >= 10
                        ? sB.Substring(1, 1) == "A" ||
                          tempFont.Italic && !(sB.Substring(1, 1) == "1" || sB.Substring(1, 1) == "4") ? 2 : 0
                        : 0)), _stringFormat);
        }

        public int byteCount = 4;

        private void PaintHexAndStringView(Graphics g, long startByte, long endByte)
        {
            Brush foreBrush = GetDefaultForeColor();
            Brush backBrush = null;

            if (_selectionBackBrush == null)
            {
                _selectionBackBrush = new SolidBrush(_selectionBackColor);
            }

            if (_selectionForeBrush == null)
            {
                _selectionForeBrush = new SolidBrush(_selectionForeColor);
            }

            int counter = 0;
            long intern_endByte = Math.Min(_byteProvider.Length - 1, endByte + _iHexMaxHBytes);

            bool isKeyInterpreterActive =
                _keyInterpreter == null || _keyInterpreter.GetType() == typeof(KeyInterpreter);
            bool isStringKeyInterpreterActive =
                _keyInterpreter != null && _keyInterpreter.GetType() == typeof(StringKeyInterpreter);

            if (_sectionEditor != null)
            {
                long offset = startByte;
                int index = (int) (offset / 4);
                for (long x = startByte; x <= intern_endByte; x += 4, index++)
                {
                    uint word =
                        ((uint) _byteProvider.ReadByte(x + 0) << 24) |
                        ((uint) _byteProvider.ReadByte(x + 1) << 16) |
                        ((uint) _byteProvider.ReadByte(x + 2) << 8) |
                        ((uint) _byteProvider.ReadByte(x + 3) << 0);

                    RelCommand cmd = null;
                    if (_sectionEditor != null &&
                        (cmd = GetBrushes(index, ref foreBrush, ref backBrush, false)) != null)
                    {
                        word = cmd.Apply(word, 0);
                    }

                    bool half = cmd != null && cmd.IsHalf;
                    for (int u = 0; u < 4; u++, offset++, counter++)
                    {
                        Point gridPoint = GetGridBytePoint(counter);
                        PointF byteStringPointF = GetByteStringPointF(gridPoint);
                        /*if (byteCount >= 4)
                            GetBrushes(index, ref foreBrush, ref backBrush, true);*/

                        if (half && u > 1)
                        {
                            backBrush = CommandBrush;
                        }

                        if (byteCount != 0)
                        {
                            if (_bytePos / byteCount == offset / byteCount)
                            {
                                GetBrushes(index, ref foreBrush, ref backBrush, true);
                            }
                            else if (!(half && u > 1))
                            {
                                GetBrushes(index, ref foreBrush, ref backBrush, false);
                            }
                        }

                        byte b = _byteProvider.ReadByte(x + u);
                        if (cmd != null && _sectionEditor.displayInitialized.Checked)
                        {
                            b = (byte) ((word >> ((3 - u) * 8)) & 0xFF);
                        }

                        bool isSelectedByte = offset >= _bytePos && offset <= _bytePos + _selectionLength - 1 &&
                                              _selectionLength != 0;

                        PaintByte(b, offset, isSelectedByte, isKeyInterpreterActive, g, foreBrush, backBrush,
                            gridPoint);

                        string s = new string(ByteCharConverter.ToChar(b), 1);

                        if (_stringViewVisible)
                        {
                            if (isSelectedByte && isStringKeyInterpreterActive)
                            {
                                g.FillRectangle(_selectionBackBrush, byteStringPointF.X, byteStringPointF.Y,
                                    _charSize.Width, _charSize.Height);
                                g.DrawString(s, Font, _selectionForeBrush, byteStringPointF, _stringFormat);
                            }
                            else
                            {
                                g.DrawString(s, Font, foreBrush, byteStringPointF, _stringFormat);
                            }
                        }
                    }
                }
            }
            else
            {
                for (long x = startByte; x <= intern_endByte; x++, counter++)
                {
                    Point gridPoint = GetGridBytePoint(counter);
                    PointF byteStringPointF = GetByteStringPointF(gridPoint);
                    byte b;
                    try
                    {
                        b = _byteProvider.ReadByte(x);
                    }
                    catch
                    {
                        // In event of invalid read, stop reading
                        break;
                    }

                    bool isSelectedByte =
                        x >= _bytePos && x <= _bytePos + _selectionLength - 1 && _selectionLength != 0;

                    PaintByte(b, x, isSelectedByte, isKeyInterpreterActive, g, foreBrush, backBrush, gridPoint);

                    string s = new string(ByteCharConverter.ToChar(b), 1);

                    if (_stringViewVisible)
                    {
                        if (isSelectedByte && isStringKeyInterpreterActive && _selectionBackBrush != null &&
                            _selectionForeBrush != null)
                        {
                            g.FillRectangle(_selectionBackBrush, byteStringPointF.X, byteStringPointF.Y,
                                _charSize.Width, _charSize.Height);
                            g.DrawString(s, Font, _selectionForeBrush, byteStringPointF, _stringFormat);
                        }
                        else
                        {
                            g.DrawString(s, Font, foreBrush, byteStringPointF, _stringFormat);
                        }
                    }
                }
            }

            if (_shadowSelectionVisible)
            {
                PaintCurrentBytesSign(g);
            }
        }

        private void PaintCurrentBytesSign(Graphics g)
        {
            if (_keyInterpreter != null && _bytePos != -1 && Enabled)
            {
                if (_keyInterpreter.GetType() == typeof(KeyInterpreter))
                {
                    if (_selectionLength == 0)
                    {
                        Point gp = GetGridBytePoint(_bytePos - _startByte);
                        PointF pf = GetByteStringPointF(gp);
                        Size s = new Size((int) _charSize.Width, (int) _charSize.Height);
                        Rectangle r = new Rectangle((int) pf.X, (int) pf.Y, s.Width, s.Height);
                        if (r.IntersectsWith(_recStringView))
                        {
                            r.Intersect(_recStringView);
                            PaintCurrentByteSign(g, r);
                        }
                    }
                    else
                    {
                        int lineWidth = (int) (_recStringView.Width - _charSize.Width);

                        Point startSelGridPoint = GetGridBytePoint(_bytePos - _startByte);
                        PointF startSelPointF = GetByteStringPointF(startSelGridPoint);

                        Point endSelGridPoint = GetGridBytePoint(_bytePos - _startByte + _selectionLength - 1);
                        PointF endSelPointF = GetByteStringPointF(endSelGridPoint);

                        int multiLine = endSelGridPoint.Y - startSelGridPoint.Y;
                        if (multiLine == 0)
                        {
                            Rectangle singleLine = new Rectangle(
                                (int) startSelPointF.X,
                                (int) startSelPointF.Y,
                                (int) (endSelPointF.X - startSelPointF.X + _charSize.Width),
                                (int) _charSize.Height);
                            if (singleLine.IntersectsWith(_recStringView))
                            {
                                singleLine.Intersect(_recStringView);
                                PaintCurrentByteSign(g, singleLine);
                            }
                        }
                        else
                        {
                            Rectangle firstLine = new Rectangle(
                                (int) startSelPointF.X,
                                (int) startSelPointF.Y,
                                (int) (_recStringView.X + lineWidth - startSelPointF.X + _charSize.Width),
                                (int) _charSize.Height);
                            if (firstLine.IntersectsWith(_recStringView))
                            {
                                firstLine.Intersect(_recStringView);
                                PaintCurrentByteSign(g, firstLine);
                            }

                            if (multiLine > 1)
                            {
                                Rectangle betweenLines = new Rectangle(
                                    _recStringView.X,
                                    (int) (startSelPointF.Y + _charSize.Height),
                                    _recStringView.Width,
                                    (int) (_charSize.Height * (multiLine - 1)));
                                if (betweenLines.IntersectsWith(_recStringView))
                                {
                                    betweenLines.Intersect(_recStringView);
                                    PaintCurrentByteSign(g, betweenLines);
                                }
                            }

                            Rectangle lastLine = new Rectangle(
                                _recStringView.X,
                                (int) endSelPointF.Y,
                                (int) (endSelPointF.X - _recStringView.X + _charSize.Width),
                                (int) _charSize.Height);
                            if (lastLine.IntersectsWith(_recStringView))
                            {
                                lastLine.Intersect(_recStringView);
                                PaintCurrentByteSign(g, lastLine);
                            }
                        }
                    }
                }
                else
                {
                    if (_selectionLength == 0)
                    {
                        Point gp = GetGridBytePoint(_bytePos - _startByte);
                        PointF pf = GetBytePointF(gp);
                        Size s = new Size((int) _charSize.Width * 2, (int) _charSize.Height);
                        Rectangle r = new Rectangle((int) pf.X, (int) pf.Y, s.Width, s.Height);
                        PaintCurrentByteSign(g, r);
                    }
                    else
                    {
                        int lineWidth = (int) (_recHex.Width - _charSize.Width * 5);

                        Point startSelGridPoint = GetGridBytePoint(_bytePos - _startByte);
                        PointF startSelPointF = GetBytePointF(startSelGridPoint);

                        Point endSelGridPoint = GetGridBytePoint(_bytePos - _startByte + _selectionLength - 1);
                        PointF endSelPointF = GetBytePointF(endSelGridPoint);

                        int multiLine = endSelGridPoint.Y - startSelGridPoint.Y;
                        if (multiLine == 0)
                        {
                            Rectangle singleLine = new Rectangle(
                                (int) startSelPointF.X,
                                (int) startSelPointF.Y,
                                (int) (endSelPointF.X - startSelPointF.X + _charSize.Width * 2),
                                (int) _charSize.Height);
                            if (singleLine.IntersectsWith(_recHex))
                            {
                                singleLine.Intersect(_recHex);
                                PaintCurrentByteSign(g, singleLine);
                            }
                        }
                        else
                        {
                            Rectangle firstLine = new Rectangle(
                                (int) startSelPointF.X,
                                (int) startSelPointF.Y,
                                (int) (_recHex.X + lineWidth - startSelPointF.X + _charSize.Width * 2),
                                (int) _charSize.Height);
                            if (firstLine.IntersectsWith(_recHex))
                            {
                                firstLine.Intersect(_recHex);
                                PaintCurrentByteSign(g, firstLine);
                            }

                            if (multiLine > 1)
                            {
                                Rectangle betweenLines = new Rectangle(
                                    _recHex.X,
                                    (int) (startSelPointF.Y + _charSize.Height),
                                    (int) (lineWidth + _charSize.Width * 2),
                                    (int) (_charSize.Height * (multiLine - 1)));
                                if (betweenLines.IntersectsWith(_recHex))
                                {
                                    betweenLines.Intersect(_recHex);
                                    PaintCurrentByteSign(g, betweenLines);
                                }
                            }

                            Rectangle lastLine = new Rectangle(
                                _recHex.X,
                                (int) endSelPointF.Y,
                                (int) (endSelPointF.X - _recHex.X + _charSize.Width * 2),
                                (int) _charSize.Height);
                            if (lastLine.IntersectsWith(_recHex))
                            {
                                lastLine.Intersect(_recHex);
                                PaintCurrentByteSign(g, lastLine);
                            }
                        }
                    }
                }
            }
        }

        private void PaintCurrentByteSign(Graphics g, Rectangle rec)
        {
            // stack overflowexception on big files - workaround
            if (rec.Top < 0 || rec.Left < 0 || rec.Width <= 0 || rec.Height <= 0)
            {
                return;
            }

            Bitmap myBitmap = new Bitmap(rec.Width, rec.Height);
            Graphics bitmapGraphics = Graphics.FromImage(myBitmap);

            SolidBrush greenBrush = new SolidBrush(_shadowSelectionColor);

            bitmapGraphics.FillRectangle(greenBrush, 0,
                0, rec.Width, rec.Height);

            g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.GammaCorrected;

            g.DrawImage(myBitmap, rec.Left, rec.Top);
        }

        private SolidBrush GetDefaultForeColor()
        {
            if (Enabled)
            {
                return BlackBrush;
            }

            return GrayBrush;
        }

        private void UpdateVisibilityBytes()
        {
            if (_byteProvider == null || _byteProvider.Length == 0)
            {
                return;
            }

            _startByte = (_scrollVpos + 1) * _iHexMaxHBytes - _iHexMaxHBytes;
            _endByte = Math.Min(_byteProvider.Length - 1, _startByte + _iHexMaxBytes);
        }

        #endregion

        #region Positioning methods

        private void UpdateRectanglePositioning()
        {
            // calc char size
            SizeF charSize = CreateGraphics().MeasureString("A", Font, 100, _stringFormat);
            _charSize = new SizeF((float) Math.Ceiling(charSize.Width), (float) Math.Ceiling(charSize.Height));

            // calc content bounds
            _recContent = ClientRectangle;
            _recContent.X += _recBorderLeft;
            _recContent.Y += _recBorderTop;
            _recContent.Width -= _recBorderRight + _recBorderLeft;
            _recContent.Height -= _recBorderBottom + _recBorderTop;

            if (_vScrollBarVisible)
            {
                _recContent.Width -= _vScrollBar.Width;
                _vScrollBar.Left = _recContent.X + _recContent.Width;
                _vScrollBar.Top = _recContent.Y;
                _vScrollBar.Height = _recContent.Height;
            }

            int marginLeft = Margin.Left;

            // calc line info bounds
            if (_lineInfoVisible)
            {
                _recLineInfo = new Rectangle(_recContent.X + marginLeft,
                    _recContent.Y,
                    (int) (_charSize.Width * 10),
                    _recContent.Height);
            }
            else
            {
                _recLineInfo = Rectangle.Empty;
                _recLineInfo.X = marginLeft;
            }

            // calc line info bounds
            _recColumnInfo = new Rectangle(_recLineInfo.X + _recLineInfo.Width, _recContent.Y,
                _recContent.Width - _recLineInfo.Width, (int) charSize.Height + 4);
            if (_columnInfoVisible)
            {
                _recLineInfo.Y += (int) charSize.Height + 4;
                _recLineInfo.Height -= (int) charSize.Height + 4;
            }
            else
            {
                _recColumnInfo.Height = 0;
            }

            // calc hex bounds and grid
            _recHex = new Rectangle(_recLineInfo.X + _recLineInfo.Width,
                _recLineInfo.Y,
                _recContent.Width - _recLineInfo.Width,
                _recContent.Height - _recColumnInfo.Height);

            if (UseFixedBytesPerLine)
            {
                SetHorizontalByteCount(_bytesPerLine);
                _recHex.Width = (int) Math.Floor((double) _iHexMaxHBytes * _charSize.Width * 3 + 2 * _charSize.Width);
            }
            else
            {
                int hmax = (int) Math.Floor(_recHex.Width / (double) _charSize.Width);
                if (_stringViewVisible)
                {
                    hmax -= 2;
                    if (hmax > 1)
                    {
                        SetHorizontalByteCount((int) Math.Floor((double) hmax / 4));
                    }
                    else
                    {
                        SetHorizontalByteCount(1);
                    }
                }
                else
                {
                    if (hmax > 1)
                    {
                        SetHorizontalByteCount((int) Math.Floor((double) hmax / 3));
                    }
                    else
                    {
                        SetHorizontalByteCount(1);
                    }
                }

                _recHex.Width = (int) Math.Floor((double) _iHexMaxHBytes * _charSize.Width * 3 + 2 * _charSize.Width);
            }

            if (_stringViewVisible)
            {
                _recStringView = new Rectangle(_recHex.X + _recHex.Width,
                    _recHex.Y,
                    (int) (_charSize.Width * _iHexMaxHBytes),
                    _recHex.Height);
            }
            else
            {
                _recStringView = Rectangle.Empty;
            }

            int vmax = (int) Math.Floor(_recHex.Height / (double) _charSize.Height);
            SetVerticalByteCount(vmax);

            _iHexMaxBytes = _iHexMaxHBytes * _iHexMaxVBytes;

            UpdateScrollSize();
        }

        private PointF GetBytePointF(long byteIndex)
        {
            Point gp = GetGridBytePoint(byteIndex);

            return GetBytePointF(gp);
        }

        private PointF GetBytePointF(Point gp)
        {
            float x = 3 * _charSize.Width * gp.X + _recHex.X;
            float y = (gp.Y + 1) * _charSize.Height - _charSize.Height + _recHex.Y;

            return new PointF(x, y);
        }

        private PointF GetColumnInfoPointF(int col)
        {
            Point gp = GetGridBytePoint(col);
            float x = 3 * _charSize.Width * gp.X + _recColumnInfo.X;
            float y = _recColumnInfo.Y;

            return new PointF(x, y);
        }

        private PointF GetByteStringPointF(Point gp)
        {
            float x = _charSize.Width * gp.X + _recStringView.X;
            float y = (gp.Y + 1) * _charSize.Height - _charSize.Height + _recStringView.Y;

            return new PointF(x, y);
        }

        private Point GetGridBytePoint(long byteIndex)
        {
            int row = (int) Math.Floor(byteIndex / (double) _iHexMaxHBytes);
            int column = (int) (byteIndex + _iHexMaxHBytes - _iHexMaxHBytes * (row + 1));

            Point res = new Point(column, row);
            return res;
        }

        #endregion

        #region Overridden properties

        /// <summary>
        /// Gets or sets the background color for the control.
        /// </summary>
        [DefaultValue(typeof(Color), "White")]
        public override Color BackColor
        {
            get => base.BackColor;
            set => base.BackColor = value;
        }

        /// <summary>
        /// The font used to display text in the hexbox.
        /// </summary>
        public override Font Font
        {
            get => base.Font;
            set
            {
                if (value == null)
                {
                    return;
                }

                base.Font = value;
            }
        }

        /// <summary>
        /// Not used.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Bindable(false)]
        public override string Text
        {
            get => base.Text;
            set => base.Text = value;
        }

        /// <summary>
        /// Not used.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Bindable(false)]
        public override RightToLeft RightToLeft
        {
            get => base.RightToLeft;
            set => base.RightToLeft = value;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the background color for the disabled control.
        /// </summary>
        [Category("Appearance")]
        [DefaultValue(typeof(Color), "WhiteSmoke")]
        public Color BackColorDisabled
        {
            get => _backColorDisabled;
            set => _backColorDisabled = value;
        }

        private Color _backColorDisabled = Color.FromName("WhiteSmoke");

        /// <summary>
        /// Gets or sets if the count of bytes in one line is fix.
        /// </summary>
        /// <remarks>
        /// When set to True, BytesPerLine property determine the maximum count of bytes in one line.
        /// </remarks>
        [DefaultValue(false)]
        [Category("Hex")]
        [Description("Gets or sets if the count of bytes in one line is fix.")]
        public bool ReadOnly
        {
            get => _readOnly;
            set
            {
                if (_readOnly == value)
                {
                    return;
                }

                _readOnly = value;
                OnReadOnlyChanged(EventArgs.Empty);
                Invalidate();
            }
        }

        private bool _readOnly;

        /// <summary>
        /// Gets or sets the maximum count of bytes in one line.
        /// </summary>
        /// <remarks>
        /// UseFixedBytesPerLine property no longer has to be set to true for this to work
        /// </remarks>
        [DefaultValue(16)]
        [Category("Hex")]
        [Description("Gets or sets the maximum count of bytes in one line.")]
        public int BytesPerLine
        {
            get => _bytesPerLine;
            set
            {
                if (_bytesPerLine == value)
                {
                    return;
                }

                _bytesPerLine = value;
                OnBytesPerLineChanged(EventArgs.Empty);

                UpdateRectanglePositioning();
                Invalidate();
            }
        }

        private int _bytesPerLine = 16;

        /// <summary>
        /// Gets or sets the number of bytes in a group. Used to show the group separator line (if GroupSeparatorVisible is true)
        /// </summary>
        /// <remarks>
        /// GroupSeparatorVisible property must set to true
        /// </remarks>
        [DefaultValue(4)]
        [Category("Hex")]
        [Description("Gets or sets the byte-count between group separators (if visible).")]
        public int GroupSize
        {
            get => _groupSize;
            set
            {
                if (_groupSize == value)
                {
                    return;
                }

                _groupSize = value;
                OnGroupSizeChanged(EventArgs.Empty);

                UpdateRectanglePositioning();
                Invalidate();
            }
        }

        private int _groupSize = 4;

        /// <summary>
        /// Gets or sets if the count of bytes in one line is fix.
        /// </summary>
        /// <remarks>
        /// When set to True, BytesPerLine property determine the maximum count of bytes in one line.
        /// </remarks>
        [DefaultValue(false)]
        [Category("Hex")]
        [Description("Gets or sets if the count of bytes in one line is fix.")]
        public bool UseFixedBytesPerLine
        {
            get => _useFixedBytesPerLine;
            set
            {
                if (_useFixedBytesPerLine == value)
                {
                    return;
                }

                _useFixedBytesPerLine = value;
                OnUseFixedBytesPerLineChanged(EventArgs.Empty);

                UpdateRectanglePositioning();
                Invalidate();
            }
        }

        private bool _useFixedBytesPerLine;

        /// <summary>
        /// Gets or sets the visibility of a vertical scroll bar.
        /// </summary>
        [DefaultValue(false)]
        [Category("Hex")]
        [Description("Gets or sets the visibility of a vertical scroll bar.")]
        public bool VScrollBarVisible
        {
            get => _vScrollBarVisible;
            set
            {
                if (_vScrollBarVisible == value)
                {
                    return;
                }

                _vScrollBarVisible = value;

                if (_vScrollBarVisible)
                {
                    Controls.Add(_vScrollBar);
                }
                else
                {
                    Controls.Remove(_vScrollBar);
                }

                UpdateRectanglePositioning();
                UpdateScrollSize();

                OnVScrollBarVisibleChanged(EventArgs.Empty);
            }
        }

        private bool _vScrollBarVisible;

        /// <summary>
        /// Gets or sets the ByteProvider.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public IByteProvider ByteProvider
        {
            get => _byteProvider;
            set
            {
                if (_byteProvider == value)
                {
                    return;
                }

                if (value == null)
                {
                    ActivateEmptyKeyInterpreter();
                }
                else
                {
                    ActivateKeyInterpreter();
                }

                if (_byteProvider != null)
                {
                    _byteProvider.LengthChanged -= new EventHandler(ByteProvider_LengthChanged);
                }

                _byteProvider = value;
                if (_byteProvider != null)
                {
                    _byteProvider.LengthChanged += new EventHandler(ByteProvider_LengthChanged);
                }

                OnByteProviderChanged(EventArgs.Empty);

                if (value == null) // do not raise events if value is null
                {
                    _bytePos = -1;
                    _byteCharacterPos = 0;
                    _selectionLength = 0;

                    DestroyCaret();
                }
                else
                {
                    SetPosition(0, 0);
                    SetSelectionLength(0);

                    if (_caretVisible && Focused)
                    {
                        UpdateCaret();
                    }
                    else
                    {
                        CreateCaret();
                    }
                }

                CheckCurrentLineChanged();
                CheckCurrentPositionInLineChanged();

                _scrollVpos = 0;

                UpdateVisibilityBytes();
                UpdateRectanglePositioning();

                Invalidate();
            }
        }

        private IByteProvider _byteProvider;

        /// <summary>
        /// Gets or sets the visibility of the group separator.
        /// </summary>
        [DefaultValue(false)]
        [Category("Hex")]
        [Description("Gets or sets the visibility of a separator vertical line.")]
        public bool GroupSeparatorVisible
        {
            get => _groupSeparatorVisible;
            set
            {
                if (_groupSeparatorVisible == value)
                {
                    return;
                }

                _groupSeparatorVisible = value;
                OnGroupSeparatorVisibleChanged(EventArgs.Empty);

                UpdateRectanglePositioning();
                Invalidate();
            }
        }

        private bool _groupSeparatorVisible;

        /// <summary>
        /// Gets or sets the visibility of the column info
        /// </summary>
        [DefaultValue(false)]
        [Category("Hex")]
        [Description("Gets or sets the visibility of header row.")]
        public bool ColumnInfoVisible
        {
            get => _columnInfoVisible;
            set
            {
                if (_columnInfoVisible == value)
                {
                    return;
                }

                _columnInfoVisible = value;
                OnColumnInfoVisibleChanged(EventArgs.Empty);

                UpdateRectanglePositioning();
                Invalidate();
            }
        }

        private bool _columnInfoVisible;

        /// <summary>
        /// Gets or sets the visibility of a line info.
        /// </summary>
        [DefaultValue(false)]
        [Category("Hex")]
        [Description("Gets or sets the visibility of a line info.")]
        public bool LineInfoVisible
        {
            get => _lineInfoVisible;
            set
            {
                if (_lineInfoVisible == value)
                {
                    return;
                }

                _lineInfoVisible = value;
                OnLineInfoVisibleChanged(EventArgs.Empty);

                UpdateRectanglePositioning();
                Invalidate();
            }
        }

        private bool _lineInfoVisible;

        /// <summary>
        /// Gets or sets the offset of a line info.
        /// </summary>
        [DefaultValue((long) 0)]
        [Category("Hex")]
        [Description("Gets or sets the offset of the line info.")]
        public long LineInfoOffset
        {
            get => _lineInfoOffset;
            set
            {
                if (_lineInfoOffset == value)
                {
                    return;
                }

                _lineInfoOffset = value;

                Invalidate();
            }
        }

        private long _lineInfoOffset;

        /// <summary>
        /// Gets or sets the hex box큦 border style.
        /// </summary>
        [DefaultValue(typeof(BorderStyle), "Fixed3D")]
        [Category("Hex")]
        [Description("Gets or sets the hex box큦 border style.")]
        public BorderStyle BorderStyle
        {
            get => _borderStyle;
            set
            {
                if (_borderStyle == value)
                {
                    return;
                }

                _borderStyle = value;
                switch (_borderStyle)
                {
                    case BorderStyle.None:
                        _recBorderLeft = _recBorderTop = _recBorderRight = _recBorderBottom = 0;
                        break;
                    case BorderStyle.Fixed3D:
                        _recBorderLeft = _recBorderRight = SystemInformation.Border3DSize.Width;
                        _recBorderTop = _recBorderBottom = SystemInformation.Border3DSize.Height;
                        break;
                    case BorderStyle.FixedSingle:
                        _recBorderLeft = _recBorderTop = _recBorderRight = _recBorderBottom = 1;
                        break;
                }

                UpdateRectanglePositioning();

                OnBorderStyleChanged(EventArgs.Empty);
            }
        }

        private BorderStyle _borderStyle = BorderStyle.Fixed3D;

        /// <summary>
        /// Gets or sets the visibility of the string view.
        /// </summary>
        [DefaultValue(false)]
        [Category("Hex")]
        [Description("Gets or sets the visibility of the string view.")]
        public bool StringViewVisible
        {
            get => _stringViewVisible;
            set
            {
                if (_stringViewVisible == value)
                {
                    return;
                }

                _stringViewVisible = value;
                OnStringViewVisibleChanged(EventArgs.Empty);

                UpdateRectanglePositioning();
                Invalidate();
            }
        }

        private bool _stringViewVisible;

        /// <summary>
        /// Gets or sets whether the HexBox control displays the hex characters in upper or lower case.
        /// </summary>
        [DefaultValue(typeof(HexCasing), "Upper")]
        [Category("Hex")]
        [Description("Gets or sets whether the HexBox control displays the hex characters in upper or lower case.")]
        public HexCasing HexCasing
        {
            get
            {
                if (_hexStringFormat == "X")
                {
                    return HexCasing.Upper;
                }

                return HexCasing.Lower;
            }
            set
            {
                string format;
                if (value == HexCasing.Upper)
                {
                    format = "X";
                }
                else
                {
                    format = "x";
                }

                if (_hexStringFormat == format)
                {
                    return;
                }

                _hexStringFormat = format;
                OnHexCasingChanged(EventArgs.Empty);

                Invalidate();
            }
        }

        /// <summary>
        /// Gets and sets the starting point of the bytes selected in the hex box.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public long SelectionStart
        {
            get => _bytePos;
            set
            {
                SetPosition(value, 0);
                ScrollByteIntoView();
                UpdateCaret();
                Invalidate();
            }
        }

        /// <summary>
        /// Gets and sets the number of bytes selected in the hex box.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public long SelectionLength
        {
            get => _selectionLength;
            set
            {
                SetSelectionLength(value);
                ScrollByteIntoView();
                Invalidate();
            }
        }

        private long _selectionLength;


        /// <summary>
        /// Gets or sets the info color used for line info. When this property is null, then ForeColor property is used.
        /// </summary>
        [DefaultValue(typeof(Color), "Empty")]
        [Category("Hex")]
        [Description("Gets or sets the line info color. When this property is null, then ForeColor property is used.")]
        public Color InfoForeColor
        {
            get => _infoForeColor;
            set
            {
                _infoForeColor = value;
                Invalidate();
            }
        }

        private Color _infoForeColor = Color.Empty;

        /// <summary>
        /// Gets or sets the info color used for column info. When this property is null, then ForeColor property is used.
        /// </summary>
        [DefaultValue(typeof(Color), "Empty")]
        [Category("Hex")]
        [Description(
            "Gets or sets the column divider color. When this property is null, then ForeColor property is used.")]
        public Color ColumnDividerColor
        {
            get => _columnDividerColor;
            set
            {
                _columnDividerColor = value;
                Invalidate();
            }
        }

        private Color _columnDividerColor = Color.Empty;

        /// <summary>
        /// Gets or sets the background color for the selected bytes.
        /// </summary>
        [DefaultValue(typeof(Color), "Blue")]
        [Category("Hex")]
        [Description("Gets or sets the background color for the selected bytes.")]
        public Color SelectionBackColor
        {
            get => _selectionBackColor;
            set
            {
                _selectionBackColor = value;
                Invalidate();
            }
        }

        private Color _selectionBackColor = Color.Blue;

        [Browsable(false)]
        public Brush SelectionBackBrush => _selectionBackBrush ??
                                           (_selectionBackBrush = new SolidBrush(_selectionBackColor));

        private Brush _selectionBackBrush;

        /// <summary>
        /// Gets or sets the foreground color for the selected bytes.
        /// </summary>
        [DefaultValue(typeof(Color), "White")]
        [Category("Hex")]
        [Description("Gets or sets the foreground color for the selected bytes.")]
        public Color SelectionForeColor
        {
            get => _selectionForeColor;
            set
            {
                _selectionForeColor = value;
                Invalidate();
            }
        }

        private Color _selectionForeColor = Color.White;

        [Browsable(false)]
        public Brush SelectionForeBrush => _selectionForeBrush ??
                                           (_selectionForeBrush = new SolidBrush(_selectionForeColor));

        private Brush _selectionForeBrush;

        /// <summary>
        /// Gets or sets the color for the relocations with commands.
        /// </summary>
        [DefaultValue(typeof(Color), "Red")]
        [Category("Hex")]
        [Description("Gets or sets the color for the relocations with linked relocations.")]
        public Color RelocationColor
        {
            get => _relocationColor;
            set
            {
                _relocationColor = value;
                Invalidate();
            }
        }

        private Color _relocationColor = Color.Red;

        [Browsable(false)]
        public Brush RelocationBrush => _relocationBrush ??
                                        (_relocationBrush = new SolidBrush(_relocationColor));

        private Brush _relocationBrush;

        /// <summary>
        /// Gets or sets the color for the relocations with commands.
        /// </summary>
        [Category("Hex")]
        [Description("Gets or sets the color for the relocations with a command.")]
        public Color CommandColor
        {
            get => _commandColor;
            set
            {
                _commandColor = value;
                Invalidate();
            }
        }

        private Color _commandColor = Color.FromArgb(200, 255, 200);

        [Browsable(false)]
        public Brush CommandBrush => _commandBrush ??
                                     (_commandBrush = new SolidBrush(_commandColor));

        private Brush _commandBrush;

        /// <summary>
        /// Gets or sets the color for code branch relocations.
        /// </summary>
        [Category("Hex")]
        [Description("Gets or sets the color for code branch relocations.")]
        public Color BlrColor
        {
            get => _blrColor;
            set
            {
                _blrColor = value;
                Invalidate();
            }
        }

        private Color _blrColor = Color.FromArgb(255, 255, 100);

        [Browsable(false)]
        public Brush BlrBrush => _blrBrush ??
                                 (_blrBrush = new SolidBrush(_blrColor));

        private Brush _blrBrush;

        /// <summary>
        /// Gets or sets the color for code branch relocations.
        /// </summary>
        [Category("Hex")]
        [Description("Gets or sets the color for linked branch relocations.")]
        public Color LinkedBranchColor
        {
            get => _linkedBranchColor;
            set
            {
                _linkedBranchColor = value;
                Invalidate();
            }
        }

        private Color _linkedBranchColor = Color.Orange;

        [Browsable(false)]
        public Brush LinkedBranchBrush => _linkedBranchBrush ??
                                          (_linkedBranchBrush = new SolidBrush(_linkedBranchColor));

        private Brush _linkedBranchBrush;

        /// <summary>
        /// Gets or sets the color for relocations that are branched to.
        /// </summary>
        [Category("Hex")]
        [Description("Gets or sets the color for branched relocations.")]
        public Color BranchOffsetColor
        {
            get => _branchOffsetColor;
            set
            {
                _branchOffsetColor = value;
                Invalidate();
            }
        }

        private Color _branchOffsetColor = Color.Plum;

        [Browsable(false)]
        public Brush BranchOffsetBrush => _branchOffsetBrush ??
                                          (_branchOffsetBrush = new SolidBrush(_branchOffsetColor));

        private Brush _branchOffsetBrush;

        /// <summary>
        /// Gets or sets the color for the selected bytes.
        /// </summary>
        [Category("Hex")]
        [Description("Gets or sets the foreground color for the selected bytes.")]
        public Color SelectedColor
        {
            get => _selectedColor;
            set
            {
                _selectedColor = value;
                Invalidate();
            }
        }

        private Color _selectedColor = Color.FromArgb(200, 255, 255);

        [Browsable(false)]
        public Brush SelectedBrush => _selectedBrush ??
                                      (_selectedBrush = new SolidBrush(_selectedColor));

        private Brush _selectedBrush;

        /// <summary>
        /// Gets or sets the visibility of a shadow selection.
        /// </summary>
        [DefaultValue(true)]
        [Category("Hex")]
        [Description("Gets or sets the visibility of a shadow selection.")]
        public bool ShadowSelectionVisible
        {
            get => _shadowSelectionVisible;
            set
            {
                if (_shadowSelectionVisible == value)
                {
                    return;
                }

                _shadowSelectionVisible = value;
                Invalidate();
            }
        }

        private bool _shadowSelectionVisible = true;

        /// <summary>
        /// Gets or sets the color of the shadow selection. 
        /// </summary>
        /// <remarks>
        /// A alpha component must be given! 
        /// Default alpha = 100
        /// </remarks>
        [Category("Hex")]
        [Description("Gets or sets the color of the shadow selection.")]
        public Color ShadowSelectionColor
        {
            get => _shadowSelectionColor;
            set
            {
                _shadowSelectionColor = value;
                Invalidate();
            }
        }

        private Color _shadowSelectionColor = Color.FromArgb(100, 60, 188, 255);

        /// <summary>
        /// Gets the number bytes drawn horizontally.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int HorizontalByteCount => _iHexMaxHBytes;

        /// <summary>
        /// Gets the number bytes drawn vertically.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int VerticalByteCount => _iHexMaxVBytes;

        /// <summary>
        /// Gets the current line
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public long CurrentLine => _currentLine;

        private long _currentLine;

        /// <summary>
        /// Gets the current position in the current line
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public long CurrentPositionInLine => _currentPositionInLine;

        private int _currentPositionInLine;

        /// <summary>
        /// Gets the a value if insertion mode is active or not.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool InsertActive
        {
            get => _insertActive;
            set
            {
                if (_insertActive == value)
                {
                    return;
                }

                _insertActive = value;

                // recreate caret
                DestroyCaret();
                CreateCaret();

                // raise change event
                OnInsertActiveChanged(EventArgs.Empty);
            }
        }

        /// <summary>
        /// Gets or sets the built-in context menu.
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public BuiltInContextMenu BuiltInContextMenu => _builtInContextMenu;

        private readonly BuiltInContextMenu _builtInContextMenu;


        /// <summary>
        /// Gets or sets the converter that will translate between byte and character values.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public IByteCharConverter ByteCharConverter
        {
            get
            {
                if (_byteCharConverter == null)
                {
                    _byteCharConverter = new DefaultByteCharConverter();
                }

                return _byteCharConverter;
            }
            set
            {
                if (value != null && value != _byteCharConverter)
                {
                    _byteCharConverter = value;
                    Invalidate();
                }
            }
        }

        private IByteCharConverter _byteCharConverter;

        #endregion

        #region Misc

        /// <summary>
        /// Converts a byte array to a hex string. For example: {10,11} = "0A 0B"
        /// </summary>
        /// <param name="data">the byte array</param>
        /// <returns>the hex string</returns>
        private string ConvertBytesToHex(byte[] data)
        {
            StringBuilder sb = new StringBuilder();
            foreach (byte b in data)
            {
                string hex = ConvertByteToHex(b);
                sb.Append(hex);
                sb.Append(" ");
            }

            if (sb.Length > 0)
            {
                sb.Remove(sb.Length - 1, 1);
            }

            string result = sb.ToString();
            return result;
        }

        /// <summary>
        /// Converts the byte to a hex string. For example: "10" = "0A";
        /// </summary>
        /// <param name="b">the byte to format</param>
        /// <returns>the hex string</returns>
        private string ConvertByteToHex(byte b)
        {
            string sB = b.ToString(_hexStringFormat, System.Threading.Thread.CurrentThread.CurrentCulture);
            if (sB.Length == 1)
            {
                sB = "0" + sB;
            }

            return sB;
        }

        /// <summary>
        /// Converts the hex string to an byte array. The hex string must be separated by a space char ' '. If there is any invalid hex information in the string the result will be null.
        /// </summary>
        /// <param name="hex">the hex string separated by ' '. For example: "0A 0B 0C"</param>
        /// <returns>the byte array. null if hex is invalid or empty</returns>
        private byte[] ConvertHexToBytes(string hex)
        {
            if (string.IsNullOrEmpty(hex))
            {
                return null;
            }

            hex = hex.Trim();
            string[] hexArray = hex.Split(' ');
            byte[] byteArray = new byte[hexArray.Length];

            for (int i = 0; i < hexArray.Length; i++)
            {
                string hexValue = hexArray[i];

                bool isByte = ConvertHexToByte(hexValue, out byte b);
                if (!isByte)
                {
                    return null;
                }

                byteArray[i] = b;
            }

            return byteArray;
        }

        private bool ConvertHexToByte(string hex, out byte b)
        {
            bool isByte = byte.TryParse(hex, NumberStyles.HexNumber,
                System.Threading.Thread.CurrentThread.CurrentCulture, out b);
            return isByte;
        }

        private void SetPosition(long bytePos)
        {
            SetPosition(bytePos, _byteCharacterPos);
        }

        private void SetPosition(long bytePos, int byteCharacterPos)
        {
            if (bytePos != _bytePos || _byteCharacterPos != byteCharacterPos)
            {
                _byteCharacterPos = byteCharacterPos;
                _bytePos = bytePos;

                CheckCurrentLineChanged();
                CheckCurrentPositionInLineChanged();

                OnSelectionStartChanged(EventArgs.Empty);
            }
        }

        private void SetSelectionLength(long selectionLength)
        {
            if (selectionLength != _selectionLength)
            {
                _selectionLength = selectionLength;
                OnSelectionLengthChanged(EventArgs.Empty);
            }
        }

        private void SetHorizontalByteCount(int value)
        {
            if (_iHexMaxHBytes == value)
            {
                return;
            }

            _iHexMaxHBytes = value;
            OnHorizontalByteCountChanged(EventArgs.Empty);
        }

        private void SetVerticalByteCount(int value)
        {
            if (_iHexMaxVBytes == value)
            {
                return;
            }

            _iHexMaxVBytes = value;
            OnVerticalByteCountChanged(EventArgs.Empty);
        }

        private void CheckCurrentLineChanged()
        {
            long currentLine = (long) Math.Floor(_bytePos / (double) _iHexMaxHBytes) + 1;

            if (_byteProvider == null && _currentLine != 0)
            {
                _currentLine = 0;
                OnCurrentLineChanged(EventArgs.Empty);
            }
            else if (currentLine != _currentLine)
            {
                _currentLine = currentLine;
                OnCurrentLineChanged(EventArgs.Empty);
            }
        }

        private void CheckCurrentPositionInLineChanged()
        {
            Point gb = GetGridBytePoint(_bytePos);
            int currentPositionInLine = gb.X + _byteCharacterPos;

            if (_byteProvider == null && _currentPositionInLine != 0)
            {
                _currentPositionInLine = 0;
                OnCurrentPositionInLineChanged(EventArgs.Empty);
            }
            else if (currentPositionInLine != _currentPositionInLine)
            {
                _currentPositionInLine = currentPositionInLine;
                OnCurrentPositionInLineChanged(EventArgs.Empty);
            }
        }

        /// <summary>
        /// Raises the InsertActiveChanged event.
        /// </summary>
        /// <param name="e">An EventArgs that contains the event data.</param>
        protected virtual void OnInsertActiveChanged(EventArgs e)
        {
            InsertActiveChanged?.Invoke(this, e);
        }

        /// <summary>
        /// Raises the ReadOnlyChanged event.
        /// </summary>
        /// <param name="e">An EventArgs that contains the event data.</param>
        protected virtual void OnReadOnlyChanged(EventArgs e)
        {
            ReadOnlyChanged?.Invoke(this, e);
        }

        /// <summary>
        /// Raises the ByteProviderChanged event.
        /// </summary>
        /// <param name="e">An EventArgs that contains the event data.</param>
        protected virtual void OnByteProviderChanged(EventArgs e)
        {
            ByteProviderChanged?.Invoke(this, e);
        }

        /// <summary>
        /// Raises the SelectionStartChanged event.
        /// </summary>
        /// <param name="e">An EventArgs that contains the event data.</param>
        protected virtual void OnSelectionStartChanged(EventArgs e)
        {
            SelectionStartChanged?.Invoke(this, e);
        }

        /// <summary>
        /// Raises the SelectionLengthChanged event.
        /// </summary>
        /// <param name="e">An EventArgs that contains the event data.</param>
        protected virtual void OnSelectionLengthChanged(EventArgs e)
        {
            SelectionLengthChanged?.Invoke(this, e);
        }

        /// <summary>
        /// Raises the LineInfoVisibleChanged event.
        /// </summary>
        /// <param name="e">An EventArgs that contains the event data.</param>
        protected virtual void OnLineInfoVisibleChanged(EventArgs e)
        {
            LineInfoVisibleChanged?.Invoke(this, e);
        }

        /// <summary>
        /// Raises the OnColumnInfoVisibleChanged event.
        /// </summary>
        /// <param name="e">An EventArgs that contains the event data.</param>
        protected virtual void OnColumnInfoVisibleChanged(EventArgs e)
        {
            ColumnInfoVisibleChanged?.Invoke(this, e);
        }

        /// <summary>
        /// Raises the ColumnSeparatorVisibleChanged event.
        /// </summary>
        /// <param name="e">An EventArgs that contains the event data.</param>
        protected virtual void OnGroupSeparatorVisibleChanged(EventArgs e)
        {
            GroupSeparatorVisibleChanged?.Invoke(this, e);
        }

        /// <summary>
        /// Raises the StringViewVisibleChanged event.
        /// </summary>
        /// <param name="e">An EventArgs that contains the event data.</param>
        protected virtual void OnStringViewVisibleChanged(EventArgs e)
        {
            StringViewVisibleChanged?.Invoke(this, e);
        }

        /// <summary>
        /// Raises the BorderStyleChanged event.
        /// </summary>
        /// <param name="e">An EventArgs that contains the event data.</param>
        protected virtual void OnBorderStyleChanged(EventArgs e)
        {
            BorderStyleChanged?.Invoke(this, e);
        }

        /// <summary>
        /// Raises the UseFixedBytesPerLineChanged event.
        /// </summary>
        /// <param name="e">An EventArgs that contains the event data.</param>
        protected virtual void OnUseFixedBytesPerLineChanged(EventArgs e)
        {
            UseFixedBytesPerLineChanged?.Invoke(this, e);
        }

        /// <summary>
        /// Raises the GroupSizeChanged event.
        /// </summary>
        /// <param name="e">An EventArgs that contains the event data.</param>
        protected virtual void OnGroupSizeChanged(EventArgs e)
        {
            GroupSizeChanged?.Invoke(this, e);
        }

        /// <summary>
        /// Raises the BytesPerLineChanged event.
        /// </summary>
        /// <param name="e">An EventArgs that contains the event data.</param>
        protected virtual void OnBytesPerLineChanged(EventArgs e)
        {
            BytesPerLineChanged?.Invoke(this, e);
        }

        /// <summary>
        /// Raises the VScrollBarVisibleChanged event.
        /// </summary>
        /// <param name="e">An EventArgs that contains the event data.</param>
        protected virtual void OnVScrollBarVisibleChanged(EventArgs e)
        {
            VScrollBarVisibleChanged?.Invoke(this, e);
        }

        /// <summary>
        /// Raises the HexCasingChanged event.
        /// </summary>
        /// <param name="e">An EventArgs that contains the event data.</param>
        protected virtual void OnHexCasingChanged(EventArgs e)
        {
            HexCasingChanged?.Invoke(this, e);
        }

        /// <summary>
        /// Raises the HorizontalByteCountChanged event.
        /// </summary>
        /// <param name="e">An EventArgs that contains the event data.</param>
        protected virtual void OnHorizontalByteCountChanged(EventArgs e)
        {
            HorizontalByteCountChanged?.Invoke(this, e);
        }

        /// <summary>
        /// Raises the VerticalByteCountChanged event.
        /// </summary>
        /// <param name="e">An EventArgs that contains the event data.</param>
        protected virtual void OnVerticalByteCountChanged(EventArgs e)
        {
            VerticalByteCountChanged?.Invoke(this, e);
        }

        /// <summary>
        /// Raises the CurrentLineChanged event.
        /// </summary>
        /// <param name="e">An EventArgs that contains the event data.</param>
        protected virtual void OnCurrentLineChanged(EventArgs e)
        {
            CurrentLineChanged?.Invoke(this, e);
        }

        /// <summary>
        /// Raises the CurrentPositionInLineChanged event.
        /// </summary>
        /// <param name="e">An EventArgs that contains the event data.</param>
        protected virtual void OnCurrentPositionInLineChanged(EventArgs e)
        {
            CurrentPositionInLineChanged?.Invoke(this, e);
        }


        /// <summary>
        /// Raises the Copied event.
        /// </summary>
        /// <param name="e">An EventArgs that contains the event data.</param>
        protected virtual void OnCopied(EventArgs e)
        {
            Copied?.Invoke(this, e);
        }

        /// <summary>
        /// Raises the CopiedHex event.
        /// </summary>
        /// <param name="e">An EventArgs that contains the event data.</param>
        protected virtual void OnCopiedHex(EventArgs e)
        {
            CopiedHex?.Invoke(this, e);
        }

        /// <summary>
        /// Raises the MouseDown event.
        /// </summary>
        /// <param name="e">An EventArgs that contains the event data.</param>
        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (!Focused)
            {
                Focus();
            }

            if (e.Button == MouseButtons.Left)
            {
                SetCaretPosition(new Point(e.X, e.Y));
            }

            base.OnMouseDown(e);
        }

        /// <summary>
        /// Raises the MouseWheel event
        /// </summary>
        /// <param name="e">An EventArgs that contains the event data.</param>
        protected override void OnMouseWheel(MouseEventArgs e)
        {
            int linesToScroll = -(e.Delta * SystemInformation.MouseWheelScrollLines / 120);
            PerformScrollLines(linesToScroll);

            base.OnMouseWheel(e);
        }

        /// <summary>
        /// Raises the Resize event.
        /// </summary>
        /// <param name="e">An EventArgs that contains the event data.</param>
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            UpdateRectanglePositioning();
        }

        /// <summary>
        /// Raises the GotFocus event.
        /// </summary>
        /// <param name="e">An EventArgs that contains the event data.</param>
        protected override void OnGotFocus(EventArgs e)
        {
            base.OnGotFocus(e);

            CreateCaret();
        }

        /// <summary>
        /// Raises the LostFocus event.
        /// </summary>
        /// <param name="e">An EventArgs that contains the event data.</param>
        protected override void OnLostFocus(EventArgs e)
        {
            base.OnLostFocus(e);

            DestroyCaret();
        }

        private void ByteProvider_LengthChanged(object sender, EventArgs e)
        {
            UpdateScrollSize();
        }

        #endregion
    }
}