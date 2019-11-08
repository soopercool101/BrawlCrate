using BrawlLib.Internal.Windows.Controls.ModelViewer.Editors;
using BrawlLib.Modeling;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;

namespace BrawlLib.Internal.Windows.Controls.ModelViewer.MainWindowBase
{
    public partial class ModelEditorBase : UserControl
    {
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool Ctrl => ModifierKeys.HasFlag(Keys.Control);

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool Alt => ModifierKeys.HasFlag(Keys.Alt);

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool Shift => ModifierKeys.HasFlag(Keys.Shift);

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool CtrlAlt => Ctrl && Alt;

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool NotCtrlAlt => !Ctrl && !Alt;

        private Dictionary<Keys, Func<bool>> _hotKeysDown;
        private Dictionary<Keys, Func<bool>> _hotKeysUp;

        /// <summary>
        /// This handles all key input in the main control instead of through child controls.
        /// If you want a hotkey to work only when a specific control is focused,
        /// use the control's 'Focused' bool in the hotkey's function.
        /// </summary>
        protected override bool ProcessKeyPreview(ref Message m)
        {
            const int WM_KEYDOWN = 0x100;
            const int WM_KEYUP = 0x101;
            //const int WM_CHAR = 0x102;
            //const int WM_SYSCHAR = 0x106;
            const int WM_SYSKEYDOWN = 0x104;
            const int WM_SYSKEYUP = 0x105;
            //const int WM_IME_CHAR = 0x286;

            bool down = m.Msg == WM_KEYDOWN || m.Msg == WM_SYSKEYDOWN;
            bool up = m.Msg == WM_KEYUP || m.Msg == WM_SYSKEYUP;

            if (down || up)
            {
                Keys key;
                if (m.Msg == WM_SYSKEYDOWN || m.Msg == WM_SYSKEYUP)
                {
                    key = ModifierKeys;
                }
                else
                {
                    key = (Keys) m.WParam;
                    if (Ctrl)
                    {
                        key |= Keys.Control;
                    }

                    if (Alt)
                    {
                        key |= Keys.Alt;
                    }

                    if (Shift)
                    {
                        key |= Keys.Shift;
                    }
                }

                if (down && _hotKeysDown.ContainsKey(key))
                {
                    if ((bool) _hotKeysDown[key].DynamicInvoke())
                    {
                        return true;
                    }
                }
                else if (_hotKeysUp.ContainsKey(key))
                {
                    if ((bool) _hotKeysUp[key].DynamicInvoke())
                    {
                        return true;
                    }
                }
            }

            return base.ProcessKeyPreview(ref m);
        }

        public class HotKeyInfo
        {
            public Keys _baseKey;
            public bool _ctrl, _alt, _shift, _keyDown, _keyUp;
            public Func<bool> _function;

            public Keys KeyCode
            {
                get
                {
                    Keys key = _baseKey;
                    if (_ctrl)
                    {
                        key |= Keys.Control;
                    }

                    if (_shift)
                    {
                        key |= Keys.Shift;
                    }

                    if (_alt)
                    {
                        key |= Keys.Alt;
                    }

                    return key;
                }
            }

            public HotKeyInfo(Keys baseKey, bool ctrl, bool alt, bool shift, Func<bool> function, bool keydown = true,
                              bool keyup = false)
            {
                _baseKey = baseKey;
                _ctrl = ctrl;
                _alt = alt;
                _shift = shift;
                _function = function;
                _keyDown = keydown;
                _keyUp = keyup;
            }
        }

        public virtual void InitHotkeyList()
        {
            _hotkeyList = new List<HotKeyInfo>
            {
                new HotKeyInfo(Keys.G, false, false, false, HotkeyRefreshReferences),
                new HotKeyInfo(Keys.U, false, false, false, HotkeyResetCamera),
                new HotKeyInfo(Keys.B, false, false, false, HotkeyRenderBones),
                new HotKeyInfo(Keys.F, false, false, false, HotkeyRenderFloor),
                new HotKeyInfo(Keys.P, false, false, false, HotkeyRenderPolygons),
                new HotKeyInfo(Keys.M, false, false, false, HotkeyRenderMetals),
                new HotKeyInfo(Keys.C, true, false, true, HotkeyCopyWholeFrame),
                new HotKeyInfo(Keys.C, true, false, false, HotkeyCopyEntryFrame),
                new HotKeyInfo(Keys.Space, false, false, false, HotkeyPlayAnim),
                new HotKeyInfo(Keys.Back, true, false, true, HotkeyClearWholeFrame),
                new HotKeyInfo(Keys.Back, true, false, false, HotkeyClearEntryFrame),
                new HotKeyInfo(Keys.Back, false, false, true, HotkeyDeleteFrame),
                new HotKeyInfo(Keys.Escape, false, false, false, HotkeyCancelChange),
                new HotKeyInfo(Keys.PageUp, true, false, false, HotkeyLastFrame),
                new HotKeyInfo(Keys.PageUp, false, false, false, HotkeyNextFrame),
                new HotKeyInfo(Keys.PageDown, true, false, false, HotkeyFirstFrame),
                new HotKeyInfo(Keys.PageDown, false, false, false, HotkeyPrevFrame),
                new HotKeyInfo(Keys.V, true, true, true, HotkeyPasteWholeFrameKeyframesOnly),
                new HotKeyInfo(Keys.V, true, false, true, HotkeyPasteWholeFrame),
                new HotKeyInfo(Keys.V, true, true, false, HotkeyPasteEntryFrameKeyframesOnly),
                new HotKeyInfo(Keys.V, true, false, false, HotkeyPasteEntryFrame),
                new HotKeyInfo(Keys.V, false, false, false, HotkeyRenderVertices),
                new HotKeyInfo(Keys.I, true, true, false, HotkeyCaptureScreenshotTransparent),
                new HotKeyInfo(Keys.I, true, false, true, HotkeyCaptureScreenshot),
                new HotKeyInfo(Keys.Z, true, false, false, HotkeyUndo),
                new HotKeyInfo(Keys.Y, true, false, false, HotkeyRedo),

#if DEBUG
                new HotKeyInfo(Keys.NumPad0, false, false, false, HotkeyRenderDepthPressed),
                new HotKeyInfo(Keys.NumPad0, false, false, false, HotkeyRenderDepthReleased, false, true),
#endif
            };
        }

#if DEBUG
        private bool HotkeyRenderDepthPressed()
        {
            if (ModelPanel.Focused && !_renderDepth)
            {
                _renderDepth = true;
                ModelPanel.Invalidate();
                return true;
            }

            return false;
        }

        private bool HotkeyRenderDepthReleased()
        {
            if (ModelPanel.Focused && _renderDepth)
            {
                _renderDepth = false;
                ModelPanel.Invalidate();
                return true;
            }

            return false;
        }
#endif

        private bool HotkeyCaptureScreenshotTransparent()
        {
            if (ModelPanel.Focused)
            {
                SaveBitmap(ModelPanel.GetScreenshot(ModelPanel.ClientRectangle, true), ScreenCaptureFolder,
                    "." + ScreenCaptureType);
                return true;
            }

            return false;
        }

        private bool HotkeyCaptureScreenshot()
        {
            if (ModelPanel.Focused)
            {
                SaveBitmap(ModelPanel.GetScreenshot(ModelPanel.ClientRectangle, false), ScreenCaptureFolder,
                    "." + ScreenCaptureType);
                return true;
            }

            return false;
        }

        private bool HotkeyUndo()
        {
            if (ModelPanel.Focused)
            {
                Undo();
                return true;
            }

            return false;
        }

        private bool HotkeyRedo()
        {
            if (ModelPanel.Focused)
            {
                Redo();
                return true;
            }

            return false;
        }

        private bool HotkeyCopyWholeFrame()
        {
            if (ModelPanel.Focused && _currentControl is CHR0Editor)
            {
                CHR0Editor.btnCopyAll.PerformClick();
                return true;
            }

            return false;
        }

        private bool HotkeyCopyEntryFrame()
        {
            if (ModelPanel.Focused && _currentControl is CHR0Editor)
            {
                CHR0Editor.btnCopy.PerformClick();
                return true;
            }

            return false;
        }

        private bool HotkeyPasteWholeFrameKeyframesOnly()
        {
            if (ModelPanel.Focused && _currentControl is CHR0Editor)
            {
                CHR0Editor._onlyKeys = true;
                CHR0Editor.btnPasteAll.PerformClick();
                return true;
            }

            return false;
        }

        private bool HotkeyPasteWholeFrame()
        {
            if (ModelPanel.Focused && _currentControl is CHR0Editor)
            {
                CHR0Editor._onlyKeys = false;
                CHR0Editor.btnPasteAll.PerformClick();
                return true;
            }

            return false;
        }

        private bool HotkeyPasteEntryFrameKeyframesOnly()
        {
            if (ModelPanel.Focused && _currentControl is CHR0Editor)
            {
                CHR0Editor._onlyKeys = true;
                CHR0Editor.btnPaste.PerformClick();
                return true;
            }

            return false;
        }

        private bool HotkeyPasteEntryFrame()
        {
            if (ModelPanel.Focused && _currentControl is CHR0Editor)
            {
                CHR0Editor._onlyKeys = false;
                CHR0Editor.btnPaste.PerformClick();
                return true;
            }

            return false;
        }

        private bool HotkeyResetCamera()
        {
            if (Ctrl)
            {
                ModelPanel.ResetCamera();
                return true;
            }

            return false;
        }

        private bool HotkeyRefreshReferences()
        {
            if (ModelPanel.Focused)
            {
                ModelPanel.RefreshReferences();
                return true;
            }

            return false;
        }

        private bool HotkeyRenderVertices()
        {
            if (ModelPanel.Focused)
            {
                RenderVertices = !RenderVertices;
                return true;
            }

            return false;
        }

        private bool HotkeyPlayAnim()
        {
            if (ModelPanel.Focused)
            {
                TogglePlay();
                return true;
            }

            return false;
        }

        private bool HotkeyRenderFloor()
        {
            if (ModelPanel.Focused)
            {
                RenderFloor = !RenderFloor;
                return true;
            }

            return false;
        }

        private bool HotkeyRenderBones()
        {
            if (ModelPanel.Focused)
            {
                RenderBones = !RenderBones;
                return true;
            }

            return false;
        }

        private bool HotkeyRenderPolygons()
        {
            if (ModelPanel.Focused)
            {
                RenderPolygons = !RenderPolygons;
                return true;
            }

            return false;
        }

        private bool HotkeyRenderMetals()
        {
            if (ModelPanel.Focused)
            {
                RenderMetals = !RenderMetals;
                return true;
            }

            return false;
        }

        private bool HotkeyClearWholeFrame()
        {
            if (ModelPanel.Focused && _currentControl is CHR0Editor)
            {
                CHR0Editor.btnClearAll.PerformClick();
                return true;
            }

            return false;
        }

        private bool HotkeyClearEntryFrame()
        {
            if (ModelPanel.Focused && _currentControl is CHR0Editor)
            {
                CHR0Editor.ClearEntry();
                return true;
            }

            return false;
        }

        private bool HotkeyDeleteFrame()
        {
            if (ModelPanel.Focused && _currentControl is CHR0Editor)
            {
                CHR0Editor.btnDelete.PerformClick();
                return true;
            }

            return false;
        }

        private bool HotkeyCancelChange()
        {
            if (!AwaitingRedoSave)
            {
                return false;
            }

            //Undo transformations, make sure to reset keyframes
            if (VertexLoc.HasValue && _currentUndo is VertexState)
            {
                VertexState v = _currentUndo as VertexState;

                for (int i = 0; i < v._vertices.Count; i++)
                {
                    v._vertices[i].WeightedPosition = v._weightedPositions[i];
                }

                _vertexSelection.ResetActions();
                CancelChangeState();
                UpdateModel();
            }
            else if (_boneSelection.IsMoving())
            {
                if (_boneSelection._rotating)
                {
                    CHR0Editor.numRotX.Value = _boneSelection._oldAngles._x;
                    CHR0Editor.numRotY.Value = _boneSelection._oldAngles._y;
                    CHR0Editor.numRotZ.Value = _boneSelection._oldAngles._z;
                    CHR0Editor.BoxChanged(CHR0Editor.numRotX, null);
                    CHR0Editor.BoxChanged(CHR0Editor.numRotY, null);
                    CHR0Editor.BoxChanged(CHR0Editor.numRotZ, null);
                }

                if (_boneSelection._translating)
                {
                    CHR0Editor.numTransX.Value = _boneSelection._oldPosition._x;
                    CHR0Editor.numTransY.Value = _boneSelection._oldPosition._y;
                    CHR0Editor.numTransZ.Value = _boneSelection._oldPosition._z;
                    CHR0Editor.BoxChanged(CHR0Editor.numTransX, null);
                    CHR0Editor.BoxChanged(CHR0Editor.numTransY, null);
                    CHR0Editor.BoxChanged(CHR0Editor.numTransZ, null);
                }

                if (_boneSelection._scaling)
                {
                    CHR0Editor.numScaleX.Value = _boneSelection._oldScale._x;
                    CHR0Editor.numScaleY.Value = _boneSelection._oldScale._y;
                    CHR0Editor.numScaleZ.Value = _boneSelection._oldScale._z;
                    CHR0Editor.BoxChanged(CHR0Editor.numScaleX, null);
                    CHR0Editor.BoxChanged(CHR0Editor.numScaleY, null);
                    CHR0Editor.BoxChanged(CHR0Editor.numScaleZ, null);
                }

                _boneSelection.ResetActions();
                CancelChangeState();
            }

            ModelPanel.CurrentViewport.AllowSelection = true;
            return false;
        }

        private bool HotkeyLastFrame()
        {
            PlaybackPanel?.btnLast_Click(this, null);

            return true;
        }

        private bool HotkeyNextFrame()
        {
            PlaybackPanel?.btnNextFrame_Click(this, null);

            return true;
        }

        private bool HotkeyFirstFrame()
        {
            PlaybackPanel?.btnFirst_Click(this, null);

            return true;
        }

        private bool HotkeyPrevFrame()
        {
            PlaybackPanel?.btnPrevFrame_Click(this, null);

            return true;
        }
    }
}