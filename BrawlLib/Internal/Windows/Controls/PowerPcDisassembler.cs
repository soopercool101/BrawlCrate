using BrawlLib.Internal.PowerPCAssembly;
using BrawlLib.Internal.Windows.Controls.Hex_Editor;
using BrawlLib.SSBB.ResourceNodes;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace BrawlLib.Internal.Windows.Controls
{
    public partial class PPCDisassembler : UserControl
    {
        internal RelocationManager _manager;
        public int _baseOffset, _sectionOffset;
        public List<PPCOpCode> _codes = new List<PPCOpCode>();
        public bool _updating;
        public SectionEditor _editor;

        public void SetTarget(RELMethodNode node)
        {
            if (node.ResourceFileType == ResourceType.RELExternalMethod)
            {
                _codes = null;
                _sectionOffset = 0;
                _baseOffset = 0;
                _manager = null;
            }
            else
            {
                _codes = new List<PPCOpCode>();
                for (int i = 0; i < node._codeLen / 4; i++)
                {
                    _codes.Add(node._manager.GetCode(i));
                }

                _sectionOffset = 0;
                _baseOffset = (int) node._cmd._addend;
                _manager = node._manager;
            }

            Display();
        }

        public void SetTarget(List<PPCOpCode> relocations, int sectionOffset, RelocationManager manager)
        {
            if (_codes != null)
            {
                int startIndex = _sectionOffset / 4;
                for (int i = 0; i < _codes.Count; i++)
                {
                    _manager.ClearColor(startIndex + i);
                }
            }

            _codes = relocations;
            _sectionOffset = sectionOffset;
            _manager = manager;

            if (_codes != null)
            {
                Color c = Color.FromArgb(255, 155, 200, 200);
                int startIndex = _sectionOffset / 4;
                for (int i = 0; i < _codes.Count; i++)
                {
                    _manager.SetColor(startIndex + i, c);
                }
            }

            Display();
        }

        public PPCDisassembler()
        {
            InitializeComponent();
            ppcOpCodeEditControl1.OnOpChanged += ppcOpCodeEditControl1_OnOpChanged;
        }

        private void ppcOpCodeEditControl1_OnOpChanged()
        {
            if (_editor != null && grdDisassembler.SelectedRows.Count > 0)
            {
                int index = grdDisassembler.SelectedRows[0].Index;
                PPCOpCode code = ppcOpCodeEditControl1._code;
                _editor._manager.SetCode(_sectionOffset / 4 + index, code);
                _editor.hexBox1.Invalidate();
            }
        }

        public void UpdateRow(int i)
        {
            if (_codes == null)
            {
                return;
            }

            DataGridViewRow row = grdDisassembler.Rows[i];
            PPCOpCode opcode = _codes[i];

            int index = _sectionOffset / 4 + i;

            row.Cells[0].Value = PPCFormat.Offset(_baseOffset + _sectionOffset + i * 4);
            row.Cells[1].Value = opcode.Name;
            row.Cells[2].Value = opcode.GetFormattedOperands();

            List<string> s = _manager.GetTags(index);
            row.Cells[3].Value = s == null ? "" : string.Join("; ", s);

            row.DefaultCellStyle.BackColor = _manager.GetStatusColorFromIndex(index);
        }

        private void Display()
        {
            grdDisassembler.Rows.Clear();
            if (_codes != null)
            {
                for (int i = 0; i < _codes.Count; i++)
                {
                    grdDisassembler.Rows.Add();
                    UpdateRow(i);
                }
            }
        }

        private void grdDisassembler_DoubleClick(object sender, EventArgs e)
        {
        }

        private void grdDisassembler_SelectionChanged(object sender, EventArgs e)
        {
            if (grdDisassembler.SelectedRows.Count == 0)
            {
                return;
            }

            int index = grdDisassembler.SelectedRows[0].Index;
            if (index >= 0 && index < _codes.Count)
            {
                ppcOpCodeEditControl1.SetCode(_codes[index]);
            }
        }

        private void grdDisassembler_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (_updating || _editor == null || grdDisassembler.SelectedRows.Count == 0)
            {
                return;
            }

            _updating = true;
            int index = grdDisassembler.SelectedRows[0].Index;
            long offset = (long) _sectionOffset + index * 4;
            if (_editor.Position.RoundDown(4) != offset)
            {
                _editor.Position = offset;
            }

            _updating = false;
        }

        private void splitContainer_MouseDown(object sender, MouseEventArgs e)
        {
            ((SplitContainer) sender).IsSplitterFixed = true;
        }

        private void splitContainer_MouseUp(object sender, MouseEventArgs e)
        {
            ((SplitContainer) sender).IsSplitterFixed = false;
        }

        private void splitContainer_MouseMove(object sender, MouseEventArgs e)
        {
            SplitContainer splitter = (SplitContainer) sender;
            if (splitter.IsSplitterFixed)
            {
                if (e.Button.Equals(MouseButtons.Left))
                {
                    if (splitter.Orientation.Equals(Orientation.Vertical))
                    {
                        if (e.X > 0 && e.X < splitter.Width)
                        {
                            splitter.SplitterDistance = e.X;
                            splitter.Refresh();
                        }
                    }
                    else
                    {
                        if (e.Y > 0 && e.Y < splitter.Height)
                        {
                            splitter.SplitterDistance = e.Y;
                            splitter.Refresh();
                        }
                    }
                }
                else
                {
                    splitter.IsSplitterFixed = false;
                }
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            bool handled = false;
            if (keyData == (Keys.Control | Keys.C))
            {
                StringBuilder b = new StringBuilder();
                for (int i = 0; i < grdDisassembler.SelectedRows.Count; i++)
                {
                    b.Append(grdDisassembler.Rows[i].Cells[1].Value + " ");
                    b.Append(grdDisassembler.Rows[i].Cells[2].Value + "\n");
                }

                Clipboard.SetText(b.ToString());
                handled = true;
            }

            if (!handled)
            {
                return base.ProcessCmdKey(ref msg, keyData);
            }

            return handled;
        }
    }
}