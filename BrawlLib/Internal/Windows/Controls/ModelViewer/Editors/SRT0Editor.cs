using BrawlLib.Internal.Windows.Controls.ModelViewer.MainWindowBase;
using BrawlLib.Internal.Windows.Forms;
using BrawlLib.Modeling;
using BrawlLib.SSBB.ResourceNodes;
using BrawlLib.Wii.Animations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace BrawlLib.Internal.Windows.Controls.ModelViewer.Editors
{
    public class SRT0Editor : UserControl
    {
        #region Designer

        private void InitializeComponent()
        {
            components = new Container();
            FrameScale = new CheckBox();
            toolStripMenuItem7 = new ToolStripMenuItem();
            btnPaste = new Button();
            toolStripMenuItem4 = new ToolStripMenuItem();
            FrameRot = new CheckBox();
            btnCopy = new Button();
            toolStripMenuItem3 = new ToolStripMenuItem();
            btnCut = new Button();
            subtract = new ToolStripMenuItem();
            toolStripMenuItem5 = new ToolStripMenuItem();
            toolStripMenuItem6 = new ToolStripMenuItem();
            toolStripMenuItem8 = new ToolStripMenuItem();
            FrameTrans = new CheckBox();
            numScaleY = new NumericInputBox();
            add = new ToolStripMenuItem();
            numRot = new NumericInputBox();
            toolStripSeparator1 = new ToolStripSeparator();
            Source = new ToolStripMenuItem();
            numTransX = new NumericInputBox();
            numTransY = new NumericInputBox();
            lblTransX = new Label();
            removeAllToolStripMenuItem = new ToolStripMenuItem();
            lblRot = new Label();
            ctxBox = new ContextMenuStrip(components);
            addCustomAmountToolStripMenuItem = new ToolStripMenuItem();
            btnDelete = new Button();
            grpTransform = new GroupBox();
            lblScaleX = new Label();
            numScaleX = new NumericInputBox();
            AllScale = new CheckBox();
            grpTransAll = new GroupBox();
            AllRot = new CheckBox();
            AllTrans = new CheckBox();
            btnClean = new Button();
            btnPasteAll = new Button();
            btnCopyAll = new Button();
            btnClear = new Button();
            btnInsert = new Button();
            ctxBox.SuspendLayout();
            grpTransform.SuspendLayout();
            grpTransAll.SuspendLayout();
            SuspendLayout();
            // 
            // FrameScale
            // 
            FrameScale.AutoSize = true;
            FrameScale.Checked = true;
            FrameScale.CheckState = CheckState.Checked;
            FrameScale.Location = new Point(249, 58);
            FrameScale.Name = "FrameScale";
            FrameScale.Size = new Size(53, 17);
            FrameScale.TabIndex = 35;
            FrameScale.Text = "Scale";
            FrameScale.UseVisualStyleBackColor = true;
            // 
            // toolStripMenuItem7
            // 
            toolStripMenuItem7.Name = "toolStripMenuItem7";
            toolStripMenuItem7.Size = new Size(100, 22);
            toolStripMenuItem7.Text = "+45";
            // 
            // btnPaste
            // 
            btnPaste.Location = new Point(101, 55);
            btnPaste.Name = "btnPaste";
            btnPaste.Size = new Size(50, 20);
            btnPaste.TabIndex = 23;
            btnPaste.Text = "Paste";
            btnPaste.UseVisualStyleBackColor = true;
            btnPaste.Click += new EventHandler(btnPaste_Click);
            // 
            // toolStripMenuItem4
            // 
            toolStripMenuItem4.Name = "toolStripMenuItem4";
            toolStripMenuItem4.Size = new Size(100, 22);
            toolStripMenuItem4.Text = "+90";
            // 
            // FrameRot
            // 
            FrameRot.AutoSize = true;
            FrameRot.Checked = true;
            FrameRot.CheckState = CheckState.Checked;
            FrameRot.Location = new Point(208, 58);
            FrameRot.Name = "FrameRot";
            FrameRot.Size = new Size(43, 17);
            FrameRot.TabIndex = 34;
            FrameRot.Text = "Rot";
            FrameRot.UseVisualStyleBackColor = true;
            // 
            // btnCopy
            // 
            btnCopy.Location = new Point(52, 55);
            btnCopy.Name = "btnCopy";
            btnCopy.Size = new Size(50, 20);
            btnCopy.TabIndex = 22;
            btnCopy.Text = "Copy";
            btnCopy.UseVisualStyleBackColor = true;
            btnCopy.Click += new EventHandler(btnCopy_Click);
            // 
            // toolStripMenuItem3
            // 
            toolStripMenuItem3.Name = "toolStripMenuItem3";
            toolStripMenuItem3.Size = new Size(100, 22);
            toolStripMenuItem3.Text = "+180";
            // 
            // btnCut
            // 
            btnCut.Location = new Point(3, 55);
            btnCut.Name = "btnCut";
            btnCut.Size = new Size(50, 20);
            btnCut.TabIndex = 21;
            btnCut.Text = "Cut";
            btnCut.UseVisualStyleBackColor = true;
            btnCut.Click += new EventHandler(btnCut_Click);
            // 
            // subtract
            // 
            subtract.DropDownItems.AddRange(new ToolStripItem[]
            {
                toolStripMenuItem5,
                toolStripMenuItem6,
                toolStripMenuItem8
            });
            subtract.Name = "subtract";
            subtract.Size = new Size(166, 22);
            subtract.Text = "Subtract From All";
            // 
            // toolStripMenuItem5
            // 
            toolStripMenuItem5.Name = "toolStripMenuItem5";
            toolStripMenuItem5.Size = new Size(97, 22);
            toolStripMenuItem5.Text = "-180";
            // 
            // toolStripMenuItem6
            // 
            toolStripMenuItem6.Name = "toolStripMenuItem6";
            toolStripMenuItem6.Size = new Size(97, 22);
            toolStripMenuItem6.Text = "-90";
            // 
            // toolStripMenuItem8
            // 
            toolStripMenuItem8.Name = "toolStripMenuItem8";
            toolStripMenuItem8.Size = new Size(97, 22);
            toolStripMenuItem8.Text = "-45";
            // 
            // FrameTrans
            // 
            FrameTrans.AutoSize = true;
            FrameTrans.Checked = true;
            FrameTrans.CheckState = CheckState.Checked;
            FrameTrans.Location = new Point(155, 58);
            FrameTrans.Name = "FrameTrans";
            FrameTrans.Size = new Size(53, 17);
            FrameTrans.TabIndex = 33;
            FrameTrans.Text = "Trans";
            FrameTrans.UseVisualStyleBackColor = true;
            // 
            // numScaleY
            // 
            numScaleY.BorderStyle = BorderStyle.FixedSingle;
            numScaleY.Integral = false;
            numScaleY.Location = new Point(154, 35);
            numScaleY.MaximumValue = 3.402823E+38F;
            numScaleY.MinimumValue = -3.402823E+38F;
            numScaleY.Name = "numScaleY";
            numScaleY.Size = new Size(82, 20);
            numScaleY.TabIndex = 18;
            numScaleY.Text = "0";
            numScaleY.ValueChanged += new EventHandler(BoxChangedCreateUndo);
            numScaleY.MouseDown += new MouseEventHandler(box_MouseDown);
            // 
            // add
            // 
            add.DropDownItems.AddRange(new ToolStripItem[]
            {
                toolStripMenuItem3,
                toolStripMenuItem4,
                toolStripMenuItem7
            });
            add.Name = "add";
            add.Size = new Size(166, 22);
            add.Text = "Add To All";
            // 
            // numRot
            // 
            numRot.BorderStyle = BorderStyle.FixedSingle;
            numRot.Integral = false;
            numRot.Location = new Point(235, 35);
            numRot.MaximumValue = 3.402823E+38F;
            numRot.MinimumValue = -3.402823E+38F;
            numRot.Name = "numRot";
            numRot.Size = new Size(82, 20);
            numRot.TabIndex = 15;
            numRot.Text = "0";
            numRot.ValueChanged += new EventHandler(BoxChangedCreateUndo);
            numRot.MouseDown += new MouseEventHandler(box_MouseDown);
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new Size(163, 6);
            // 
            // Source
            // 
            Source.Enabled = false;
            Source.Name = "Source";
            Source.Size = new Size(166, 22);
            Source.Text = "Source";
            // 
            // numTransX
            // 
            numTransX.BorderStyle = BorderStyle.FixedSingle;
            numTransX.Integral = false;
            numTransX.Location = new Point(73, 16);
            numTransX.MaximumValue = 3.402823E+38F;
            numTransX.MinimumValue = -3.402823E+38F;
            numTransX.Name = "numTransX";
            numTransX.Size = new Size(82, 20);
            numTransX.TabIndex = 3;
            numTransX.Text = "0";
            numTransX.ValueChanged += new EventHandler(BoxChangedCreateUndo);
            numTransX.MouseDown += new MouseEventHandler(box_MouseDown);
            // 
            // numTransY
            // 
            numTransY.BorderStyle = BorderStyle.FixedSingle;
            numTransY.Integral = false;
            numTransY.Location = new Point(154, 16);
            numTransY.MaximumValue = 3.402823E+38F;
            numTransY.MinimumValue = -3.402823E+38F;
            numTransY.Name = "numTransY";
            numTransY.Size = new Size(82, 20);
            numTransY.TabIndex = 13;
            numTransY.Text = "0";
            numTransY.ValueChanged += new EventHandler(BoxChangedCreateUndo);
            numTransY.MouseDown += new MouseEventHandler(box_MouseDown);
            // 
            // lblTransX
            // 
            lblTransX.BorderStyle = BorderStyle.FixedSingle;
            lblTransX.Location = new Point(4, 16);
            lblTransX.Name = "lblTransX";
            lblTransX.Size = new Size(70, 20);
            lblTransX.TabIndex = 4;
            lblTransX.Text = "Translation:";
            lblTransX.TextAlign = ContentAlignment.MiddleRight;
            // 
            // removeAllToolStripMenuItem
            // 
            removeAllToolStripMenuItem.Name = "removeAllToolStripMenuItem";
            removeAllToolStripMenuItem.Size = new Size(166, 22);
            removeAllToolStripMenuItem.Text = "Remove All";
            // 
            // lblRot
            // 
            lblRot.BorderStyle = BorderStyle.FixedSingle;
            lblRot.Location = new Point(235, 16);
            lblRot.Name = "lblRot";
            lblRot.Size = new Size(82, 20);
            lblRot.TabIndex = 7;
            lblRot.Text = "Rotation:";
            lblRot.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // ctxBox
            // 
            ctxBox.Items.AddRange(new ToolStripItem[]
            {
                Source,
                toolStripSeparator1,
                add,
                subtract,
                removeAllToolStripMenuItem,
                addCustomAmountToolStripMenuItem
            });
            ctxBox.Name = "ctxBox";
            ctxBox.Size = new Size(167, 120);
            // 
            // addCustomAmountToolStripMenuItem
            // 
            addCustomAmountToolStripMenuItem.Name = "addCustomAmountToolStripMenuItem";
            addCustomAmountToolStripMenuItem.Size = new Size(166, 22);
            addCustomAmountToolStripMenuItem.Text = "Edit All...";
            addCustomAmountToolStripMenuItem.Click += new EventHandler(addCustomAmountToolStripMenuItem_Click_1);
            // 
            // btnDelete
            // 
            btnDelete.Location = new Point(6, 16);
            btnDelete.Name = "btnDelete";
            btnDelete.Size = new Size(50, 20);
            btnDelete.TabIndex = 25;
            btnDelete.Text = "Delete";
            btnDelete.UseVisualStyleBackColor = true;
            btnDelete.Click += new EventHandler(btnDelete_Click);
            // 
            // grpTransform
            // 
            grpTransform.Controls.Add(lblScaleX);
            grpTransform.Controls.Add(numScaleX);
            grpTransform.Controls.Add(FrameScale);
            grpTransform.Controls.Add(btnPaste);
            grpTransform.Controls.Add(FrameRot);
            grpTransform.Controls.Add(btnCopy);
            grpTransform.Controls.Add(FrameTrans);
            grpTransform.Controls.Add(btnCut);
            grpTransform.Controls.Add(numScaleY);
            grpTransform.Controls.Add(numRot);
            grpTransform.Controls.Add(numTransX);
            grpTransform.Controls.Add(numTransY);
            grpTransform.Controls.Add(lblTransX);
            grpTransform.Controls.Add(lblRot);
            grpTransform.Dock = DockStyle.Left;
            grpTransform.Enabled = false;
            grpTransform.Location = new Point(0, 0);
            grpTransform.Name = "grpTransform";
            grpTransform.Size = new Size(321, 78);
            grpTransform.TabIndex = 28;
            grpTransform.TabStop = false;
            grpTransform.Text = "Transform Frame";
            // 
            // lblScaleX
            // 
            lblScaleX.BorderStyle = BorderStyle.FixedSingle;
            lblScaleX.Location = new Point(4, 35);
            lblScaleX.Name = "lblScaleX";
            lblScaleX.Size = new Size(70, 20);
            lblScaleX.TabIndex = 37;
            lblScaleX.Text = "Scale:";
            lblScaleX.TextAlign = ContentAlignment.MiddleRight;
            // 
            // numScaleX
            // 
            numScaleX.BorderStyle = BorderStyle.FixedSingle;
            numScaleX.Integral = false;
            numScaleX.Location = new Point(73, 35);
            numScaleX.MaximumValue = 3.402823E+38F;
            numScaleX.MinimumValue = -3.402823E+38F;
            numScaleX.Name = "numScaleX";
            numScaleX.Size = new Size(82, 20);
            numScaleX.TabIndex = 36;
            numScaleX.Text = "0";
            numScaleX.ValueChanged += new EventHandler(BoxChangedCreateUndo);
            numScaleX.MouseDown += new MouseEventHandler(box_MouseDown);
            // 
            // AllScale
            // 
            AllScale.AutoSize = true;
            AllScale.Checked = true;
            AllScale.CheckState = CheckState.Checked;
            AllScale.Location = new Point(108, 57);
            AllScale.Name = "AllScale";
            AllScale.Size = new Size(53, 17);
            AllScale.TabIndex = 32;
            AllScale.Text = "Scale";
            AllScale.UseVisualStyleBackColor = true;
            // 
            // grpTransAll
            // 
            grpTransAll.Controls.Add(AllScale);
            grpTransAll.Controls.Add(AllRot);
            grpTransAll.Controls.Add(AllTrans);
            grpTransAll.Controls.Add(btnClean);
            grpTransAll.Controls.Add(btnPasteAll);
            grpTransAll.Controls.Add(btnCopyAll);
            grpTransAll.Controls.Add(btnClear);
            grpTransAll.Controls.Add(btnInsert);
            grpTransAll.Controls.Add(btnDelete);
            grpTransAll.Dock = DockStyle.Fill;
            grpTransAll.Enabled = false;
            grpTransAll.Location = new Point(321, 0);
            grpTransAll.Name = "grpTransAll";
            grpTransAll.Size = new Size(162, 78);
            grpTransAll.TabIndex = 29;
            grpTransAll.TabStop = false;
            grpTransAll.Text = "Transform All";
            // 
            // AllRot
            // 
            AllRot.AutoSize = true;
            AllRot.Checked = true;
            AllRot.CheckState = CheckState.Checked;
            AllRot.Location = new Point(108, 38);
            AllRot.Name = "AllRot";
            AllRot.Size = new Size(43, 17);
            AllRot.TabIndex = 31;
            AllRot.Text = "Rot";
            AllRot.UseVisualStyleBackColor = true;
            // 
            // AllTrans
            // 
            AllTrans.AutoSize = true;
            AllTrans.Checked = true;
            AllTrans.CheckState = CheckState.Checked;
            AllTrans.Location = new Point(108, 19);
            AllTrans.Name = "AllTrans";
            AllTrans.Size = new Size(53, 17);
            AllTrans.TabIndex = 30;
            AllTrans.Text = "Trans";
            AllTrans.UseVisualStyleBackColor = true;
            // 
            // btnClean
            // 
            btnClean.Location = new Point(55, 35);
            btnClean.Name = "btnClean";
            btnClean.Size = new Size(50, 20);
            btnClean.TabIndex = 29;
            btnClean.Text = "Clean";
            btnClean.UseVisualStyleBackColor = true;
            btnClean.Click += new EventHandler(btnClean_Click);
            // 
            // btnPasteAll
            // 
            btnPasteAll.Location = new Point(6, 35);
            btnPasteAll.Name = "btnPasteAll";
            btnPasteAll.Size = new Size(50, 20);
            btnPasteAll.TabIndex = 28;
            btnPasteAll.Text = "Paste";
            btnPasteAll.UseVisualStyleBackColor = true;
            btnPasteAll.Click += new EventHandler(btnPasteAll_Click);
            // 
            // btnCopyAll
            // 
            btnCopyAll.Location = new Point(6, 54);
            btnCopyAll.Name = "btnCopyAll";
            btnCopyAll.Size = new Size(50, 20);
            btnCopyAll.TabIndex = 27;
            btnCopyAll.Text = "Copy";
            btnCopyAll.UseVisualStyleBackColor = true;
            btnCopyAll.Click += new EventHandler(btnCopyAll_Click);
            // 
            // btnClear
            // 
            btnClear.Location = new Point(55, 16);
            btnClear.Name = "btnClear";
            btnClear.Size = new Size(50, 20);
            btnClear.TabIndex = 26;
            btnClear.Text = "Clear";
            btnClear.UseVisualStyleBackColor = true;
            btnClear.Click += new EventHandler(btnClear_Click);
            // 
            // btnInsert
            // 
            btnInsert.Location = new Point(55, 54);
            btnInsert.Name = "btnInsert";
            btnInsert.Size = new Size(50, 20);
            btnInsert.TabIndex = 24;
            btnInsert.Text = "Insert";
            btnInsert.UseVisualStyleBackColor = true;
            btnInsert.Click += new EventHandler(btnInsert_Click);
            // 
            // SRT0Editor
            // 
            Controls.Add(grpTransAll);
            Controls.Add(grpTransform);
            MinimumSize = new Size(483, 78);
            Name = "SRT0Editor";
            Size = new Size(483, 78);
            ctxBox.ResumeLayout(false);
            grpTransform.ResumeLayout(false);
            grpTransform.PerformLayout();
            grpTransAll.ResumeLayout(false);
            grpTransAll.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private CheckBox FrameScale;
        private ToolStripMenuItem toolStripMenuItem7;
        private Button btnPaste;
        private ToolStripMenuItem toolStripMenuItem4;
        private CheckBox FrameRot;
        private Button btnCopy;
        private ToolStripMenuItem toolStripMenuItem3;
        private Button btnCut;
        private ToolStripMenuItem subtract;
        private ToolStripMenuItem toolStripMenuItem5;
        private ToolStripMenuItem toolStripMenuItem6;
        private ToolStripMenuItem toolStripMenuItem8;
        private CheckBox FrameTrans;
        private NumericInputBox numScaleY;
        private ToolStripMenuItem add;
        internal NumericInputBox numRot;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripMenuItem Source;
        internal NumericInputBox numTransX;
        internal NumericInputBox numTransY;
        private Label lblTransX;
        private ToolStripMenuItem removeAllToolStripMenuItem;
        private Label lblRot;
        private ContextMenuStrip ctxBox;
        private IContainer components;
        private ToolStripMenuItem addCustomAmountToolStripMenuItem;
        public Button btnDelete;
        private GroupBox grpTransform;
        private CheckBox AllScale;
        private GroupBox grpTransAll;
        private CheckBox AllRot;
        private CheckBox AllTrans;
        public Button btnClean;
        public Button btnPasteAll;
        public Button btnCopyAll;
        public Button btnClear;
        public Button btnInsert;
        private Label lblScaleX;
        private NumericInputBox numScaleX;

        public ModelEditorBase _mainWindow;

        public event EventHandler CreateUndo;

        internal NumericInputBox[] _transBoxes = new NumericInputBox[5];

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public IBoneNode TargetBone
        {
            get => _mainWindow.SelectedBone;
            set => _mainWindow.SelectedBone = value;
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public MDL0MaterialRefNode TargetTexRef
        {
            get => _mainWindow.TargetTexRef;
            set => _mainWindow.TargetTexRef = value;
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int CurrentFrame
        {
            get => _mainWindow.CurrentFrame;
            set => _mainWindow.CurrentFrame = value;
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public IModel TargetModel
        {
            get => _mainWindow.TargetModel;
            set => _mainWindow.TargetModel = value;
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public SRT0Node SelectedAnimation
        {
            get => _mainWindow.SelectedSRT0;
            set => _mainWindow.SelectedSRT0 = value;
        }

        public SRT0TextureNode TexEntry
        {
            get
            {
                MDL0MaterialRefNode mr = TargetTexRef;
                if (mr != null && SelectedAnimation != null)
                {
                    return SelectedAnimation.FindChild(mr.Parent.Name + "/Texture" + mr.Index, true) as SRT0TextureNode;
                }

                return null;
            }
        }

        public SRT0Editor()
        {
            InitializeComponent();
            _transBoxes[0] = numScaleX;
            numScaleX.Tag = 0;
            _transBoxes[1] = numScaleY;
            numScaleY.Tag = 1;
            _transBoxes[2] = numRot;
            numRot.Tag = 2;
            _transBoxes[3] = numTransX;
            numTransX.Tag = 3;
            _transBoxes[4] = numTransY;
            numTransY.Tag = 4;
        }

        public void UpdatePropDisplay()
        {
            if (!Enabled)
            {
                return;
            }

            grpTransAll.Enabled = SelectedAnimation != null;
            btnInsert.Enabled = btnDelete.Enabled = btnClear.Enabled = CurrentFrame >= 1 && SelectedAnimation != null;
            grpTransform.Enabled = TargetTexRef != null;

            for (int i = 0; i < 5; i++)
            {
                ResetBox(i);
            }

            if (_mainWindow.InterpolationEditor != null &&
                _mainWindow.InterpolationEditor.Visible &&
                _mainWindow.TargetAnimType == NW4RAnimType.SRT &&
                TargetTexRef != null &&
                SelectedAnimation != null &&
                CurrentFrame >= 1 &&
                _mainWindow.InterpolationEditor._targetNode != TexEntry)
            {
                _mainWindow.InterpolationEditor.SetTarget(TexEntry);
            }
        }

        public unsafe void ResetBox(int index)
        {
            if (TargetTexRef == null)
            {
                return;
            }

            NumericInputBox box = _transBoxes[index];
            if (SelectedAnimation != null && CurrentFrame >= 1 && TexEntry != null)
            {
                KeyframeEntry e = TexEntry.Keyframes.GetKeyframe(index, CurrentFrame - 1);
                if (e == null)
                {
                    box.Value = TexEntry.Keyframes[CurrentFrame - 1, index];
                    box.BackColor = Color.White;
                }
                else
                {
                    box.Value = e._value;
                    box.BackColor = Color.Yellow;
                }
            }
            else
            {
                TextureFrameState state = TargetTexRef._bindState;
                box.Value = ((float*) &state)[index];
                box.BackColor = Color.White;
            }
        }

        internal void BoxChangedCreateUndo(object sender, EventArgs e)
        {
            CreateUndo?.Invoke(sender, null);

            //Only update for input boxes: Methods affecting multiple values call BoxChanged on their own.
            if (sender.GetType() == typeof(NumericInputBox))
            {
                BoxChanged(sender, null);
            }
        }

        internal unsafe void BoxChanged(object sender, EventArgs e)
        {
            if (TargetTexRef == null || sender == null)
            {
                return;
            }

            NumericInputBox box = sender as NumericInputBox;
            int index = (int) box.Tag;

            SRTAnimationFrame kf;
            float* pkf = (float*) &kf;

            if (SelectedAnimation != null && CurrentFrame >= 1)
            {
                if (TexEntry == null)
                {
                    if (!float.IsNaN(box.Value))
                    {
                        SRT0TextureNode newEntry =
                            SelectedAnimation.FindOrCreateEntry(TargetTexRef.Parent.Name, TargetTexRef.Index, false);

                        //Set initial values
                        TextureFrameState state = TargetTexRef._bindState;
                        float* p = (float*) &state;
                        for (int i = 0; i < 2; i++)
                        {
                            if (p[i] != 1.0f)
                            {
                                newEntry.SetKeyframe(i, 0, p[i]);
                            }
                        }

                        for (int i = 2; i < 5; i++)
                        {
                            if (p[i] != 0.0f)
                            {
                                newEntry.SetKeyframe(i, 0, p[i]);
                            }
                        }

                        newEntry.SetKeyframe(index, CurrentFrame - 1, box.Value);
                    }
                }
                else if (float.IsNaN(box.Value))
                {
                    TexEntry.RemoveKeyframe(index, CurrentFrame - 1);
                }
                else
                {
                    TexEntry.SetKeyframe(index, CurrentFrame - 1, box.Value);
                }

                if (_mainWindow.InterpolationEditor != null &&
                    _mainWindow.InterpolationEditor.Visible &&
                    _mainWindow.TargetAnimType == NW4RAnimType.SRT &&
                    TargetTexRef != null &&
                    SelectedAnimation != null &&
                    CurrentFrame >= 1 &&
                    _mainWindow.InterpolationEditor._targetNode != TexEntry)
                {
                    _mainWindow.InterpolationEditor.SetTarget(TexEntry);
                }
            }
            else
            {
                //Change base transform
                TextureFrameState state = TargetTexRef._bindState;
                float* p = (float*) &state;
                p[index] = float.IsNaN(box.Value) ? index > 1 ? 0.0f : 1.0f : box.Value;
                state.CalcTransforms();
                TargetTexRef._bindState = state;
                TargetTexRef.SignalPropertyChange();
            }

            ResetBox(index);
            _mainWindow.KeyframePanel.UpdateKeyframe(CurrentFrame - 1);

            _mainWindow.UpdateModel();
            _mainWindow.ModelPanel.Invalidate();

            _mainWindow.KeyframePanel._updating = false;
        }

        private static readonly Dictionary<string, SRTAnimationFrame> _copyAllState =
            new Dictionary<string, SRTAnimationFrame>();

        private void btnCopyAll_Click(object sender, EventArgs e)
        {
            _copyAllState.Clear();

            if (CurrentFrame < 1)
            {
                if (TargetModel is MDL0Node)
                {
                    foreach (MDL0MaterialNode mat in ((MDL0Node) TargetModel).MaterialList)
                    {
                        foreach (MDL0MaterialRefNode mr in mat.Children)
                        {
                            _copyAllState[mr.Parent.Name + mr.Index] = (SRTAnimationFrame) mr._bindState;
                        }
                    }
                }
            }
            else
            {
                foreach (SRT0EntryNode entry in SelectedAnimation.Children)
                {
                    foreach (SRT0TextureNode tex in entry.Children)
                    {
                        _copyAllState[tex.Parent.Name + tex.TextureIndex] = tex.GetAnimFrame(CurrentFrame - 1);
                    }
                }
            }
        }

        private void btnPasteAll_Click(object sender, EventArgs e)
        {
            if (_copyAllState.Count == 0)
            {
                return;
            }

            if (CurrentFrame == 0)
            {
                if (TargetModel is MDL0Node)
                {
                    foreach (MDL0MaterialNode mat in ((MDL0Node) TargetModel).MaterialList)
                    {
                        foreach (MDL0MaterialRefNode mr in mat.Children)
                        {
                            if (_copyAllState.ContainsKey(mr.Parent.Name + mr.Index))
                            {
                                if (AllTrans.Checked)
                                {
                                    mr._bindState.Translate = _copyAllState[mr.Parent.Name + mr.Index].Translation;
                                }

                                if (AllRot.Checked)
                                {
                                    mr._bindState.Rotate = _copyAllState[mr.Parent.Name + mr.Index].Rotation;
                                }

                                if (AllScale.Checked)
                                {
                                    mr._bindState.Scale = _copyAllState[mr.Parent.Name + mr.Index].Scale;
                                }

                                mr.SignalPropertyChange();
                            }
                        }
                    }
                }
            }
            else
            {
                foreach (SRT0EntryNode entry in SelectedAnimation.Children)
                {
                    foreach (SRT0TextureNode tex in entry.Children)
                    {
                        if (_copyAllState.ContainsKey(tex.Parent.Name + tex.TextureIndex))
                        {
                            if (AllTrans.Checked)
                            {
                                tex.SetKeyframeOnlyTrans(CurrentFrame - 1,
                                    _copyAllState[tex.Parent.Name + tex.TextureIndex]);
                            }

                            if (AllRot.Checked)
                            {
                                tex.SetKeyframeOnlyRot(CurrentFrame - 1,
                                    _copyAllState[tex.Parent.Name + tex.TextureIndex]);
                            }

                            if (AllScale.Checked)
                            {
                                tex.SetKeyframeOnlyScale(CurrentFrame - 1,
                                    _copyAllState[tex.Parent.Name + tex.TextureIndex]);
                            }
                        }
                    }
                }
            }

            _mainWindow.UpdateModel();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            if (CurrentFrame < 1)
            {
                return;
            }

            foreach (SRT0EntryNode entry in SelectedAnimation.Children)
            {
                foreach (SRT0TextureNode tex in entry.Children)
                {
                    if (AllTrans.Checked)
                    {
                        tex.RemoveKeyframeOnlyTrans(CurrentFrame - 1);
                    }

                    if (AllRot.Checked)
                    {
                        tex.RemoveKeyframeOnlyRot(CurrentFrame - 1);
                    }

                    if (AllScale.Checked)
                    {
                        tex.RemoveKeyframeOnlyScale(CurrentFrame - 1);
                    }
                }
            }

            _mainWindow.UpdateModel();
        }

        private void btnClean_Click(object sender, EventArgs e)
        {
            if (!(TargetModel is MDL0Node))
            {
                return;
            }

            ResourceNode group = ((MDL0Node) TargetModel)._matGroup;
            ResourceNode mat = null;
            if (group == null)
            {
                return;
            }

            List<SRT0EntryNode> badMaterials = new List<SRT0EntryNode>();
            List<SRT0TextureNode> badTextures = new List<SRT0TextureNode>();
            foreach (SRT0EntryNode entry in SelectedAnimation.Children)
            {
                if ((mat = group.FindChild(entry._name, true)) == null)
                {
                    badMaterials.Add(entry);
                }
                else
                {
                    int count = 0;
                    foreach (SRT0TextureNode tex in entry.Children)
                    {
                        if ((mat = @group.FindChild(entry._name, true)) == null ||
                            mat.Children.Count < tex.TextureIndex)
                        {
                            badTextures.Add(tex);
                            count++;
                        }
                        else
                        {
                            tex.Keyframes.Clean();
                        }
                    }

                    if (count == entry.Children.Count)
                    {
                        badMaterials.Add(entry);
                    }
                }
            }

            int temp0 = badMaterials.Count;
            int temp1 = badTextures.Count;
            foreach (SRT0TextureNode n in badTextures)
            {
                n.Remove();
                n.Dispose();
            }

            foreach (SRT0EntryNode n in badMaterials)
            {
                n.Remove();
                n.Dispose();
            }

            MessageBox.Show(temp0 + " unused material entries and\n" + temp1 + " unused texture entries removed.");
            UpdatePropDisplay();
        }

        private void ctxBox_Opening(object sender, CancelEventArgs e)
        {
            if (SelectedAnimation == null)
            {
                e.Cancel = true;
            }
        }

        public void UpdateInterpolationEditor(NumericInputBox box)
        {
            if (_mainWindow.InterpolationEditor == null || !_mainWindow.InterpolationEditor.Visible)
            {
                return;
            }

            if (box.BackColor == Color.Yellow)
            {
                KeyframeEntry kfe = TexEntry.GetKeyframe(type, CurrentFrame - 1);
                if (kfe != null)
                {
                    _mainWindow.InterpolationEditor.SelectedKeyframe = kfe;
                }
            }
            else
            {
                _mainWindow.InterpolationEditor.SelectedKeyframe = null;
            }
        }

        public int type;

        private void box_MouseDown(object sender, MouseEventArgs e)
        {
            NumericInputBox box = sender as NumericInputBox;

            type = (int) box.Tag;

            if (_mainWindow.InterpolationEditor != null && _mainWindow.InterpolationEditor.Visible)
            {
                if (_mainWindow.InterpolationEditor.SelectedMode != type)
                {
                    _mainWindow.InterpolationEditor.SelectedMode = type;
                }

                UpdateInterpolationEditor(box);
            }

            if (e.Button == MouseButtons.Right)
            {
                if (box.Enabled)
                {
                    box.ContextMenuStrip = ctxBox;
                    Source.Text = box.Text;
                }
                else
                {
                    box.ContextMenuStrip = null;
                }
            }
        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            if (SelectedAnimation == null)
            {
                return;
            }

            KeyframeEntry kfe;
            CHR0EntryNode _target = SelectedAnimation.FindChild(TargetTexRef.Parent.Name, false) as CHR0EntryNode;
            for (int x = 0; x < _target.FrameCount; x++) //Loop thru each frame
            {
                if ((kfe = _target.GetKeyframe(type, x)) != null) //Check for a keyframe
                {
                    kfe._value += 180;
                }
            }

            ResetBox(type);
            _mainWindow.UpdateModel();
        }

        private void toolStripMenuItem4_Click(object sender, EventArgs e)
        {
            if (SelectedAnimation == null)
            {
                return;
            }

            KeyframeEntry kfe;
            SRT0TextureNode _target =
                SelectedAnimation.FindChild(TargetTexRef.Parent.Name + "/Texture" + TargetTexRef.Index, true) as
                    SRT0TextureNode;
            for (int x = 0; x < _target.FrameCount; x++) //Loop thru each frame
            {
                if ((kfe = _target.GetKeyframe(type, x)) != null) //Check for a keyframe
                {
                    kfe._value += 90;
                }
            }

            ResetBox(type);
            _mainWindow.UpdateModel();
        }

        private void toolStripMenuItem5_Click(object sender, EventArgs e)
        {
            if (SelectedAnimation == null)
            {
                return;
            }

            KeyframeEntry kfe;
            SRT0TextureNode _target =
                SelectedAnimation.FindChild(TargetTexRef.Parent.Name + "/Texture" + TargetTexRef.Index, true) as
                    SRT0TextureNode;
            for (int x = 0; x < _target.FrameCount; x++) //Loop thru each frame
            {
                if ((kfe = _target.GetKeyframe(type, x)) != null) //Check for a keyframe
                {
                    kfe._value -= 180;
                }
            }

            ResetBox(type);
            _mainWindow.UpdateModel();
        }

        private void toolStripMenuItem6_Click(object sender, EventArgs e)
        {
            if (SelectedAnimation == null)
            {
                return;
            }

            KeyframeEntry kfe;
            SRT0TextureNode _target =
                SelectedAnimation.FindChild(TargetTexRef.Parent.Name + "/Texture" + TargetTexRef.Index, true) as
                    SRT0TextureNode;
            for (int x = 0; x < _target.FrameCount; x++) //Loop thru each frame
            {
                if ((kfe = _target.GetKeyframe(type, x)) != null) //Check for a keyframe
                {
                    kfe._value -= 90;
                }
            }

            ResetBox(type);
            _mainWindow.UpdateModel();
        }

        private void toolStripMenuItem7_Click(object sender, EventArgs e)
        {
            if (SelectedAnimation == null)
            {
                return;
            }

            KeyframeEntry kfe;
            SRT0TextureNode _target =
                SelectedAnimation.FindChild(TargetTexRef.Parent.Name + "/Texture" + TargetTexRef.Index, true) as
                    SRT0TextureNode;
            for (int x = 0; x < _target.FrameCount; x++) //Loop thru each frame
            {
                if ((kfe = _target.GetKeyframe(type, x)) != null) //Check for a keyframe
                {
                    kfe._value += 45;
                }
            }

            ResetBox(type);
            _mainWindow.UpdateModel();
        }

        private void toolStripMenuItem8_Click(object sender, EventArgs e)
        {
            if (SelectedAnimation == null)
            {
                return;
            }

            KeyframeEntry kfe;
            SRT0TextureNode _target =
                SelectedAnimation.FindChild(TargetTexRef.Parent.Name + "/Texture" + TargetTexRef.Index, true) as
                    SRT0TextureNode;
            for (int x = 0; x < _target.FrameCount; x++) //Loop thru each frame
            {
                if ((kfe = _target.GetKeyframe(type, x)) != null) //Check for a keyframe
                {
                    kfe._value -= 45;
                }
            }

            ResetBox(type);
            _mainWindow.UpdateModel();
        }

        private void removeAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SRT0TextureNode _target =
                SelectedAnimation?.FindChild(TargetTexRef.Parent.Name + "/Texture" + TargetTexRef.Index, true) as
                    SRT0TextureNode;
            if (_target != null)
            {
                _target.Keyframes._keyArrays[type]._keyRoot = new KeyframeEntry(-1, type < 2 ? 1 : 0);
                _target.Keyframes._keyArrays[type]._keyCount = 0;
                _target.SignalPropertyChange();
                ResetBox(type);
                _mainWindow.UpdateModel();
            }
        }

        private void addCustomAmountToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (SelectedAnimation == null)
            {
                return;
            }

            EditAllKeyframesDialog ed = new EditAllKeyframesDialog();
            //ed.ShowDialog(null, type, SelectedAnimation.FindChild(TargetTexRef.Name, false) as IKeyframeSource);
            ResetBox(type);
            _mainWindow.UpdateModel();
        }

        private unsafe void btnCut_Click(object sender, EventArgs e)
        {
            SRTAnimationFrame frame;
            float* p = (float*) &frame;

            BoxChangedCreateUndo(this, null);

            for (int i = 0; i < 5; i++)
            {
                if (!FrameScale.Checked && i < 2)
                {
                    p[i] = 1;
                }
                else if (
                    FrameScale.Checked && i < 2 ||
                    FrameRot.Checked && i == 2 ||
                    FrameTrans.Checked && i > 2)
                {
                    p[i] = _transBoxes[i].Value;
                }

                _transBoxes[i].Value = float.NaN;
                BoxChanged(_transBoxes[i], null);
            }

            DataObject da = new DataObject();
            da.SetData("SRTAnimationFrame", frame);
            Clipboard.SetDataObject(da, true);
        }

        private unsafe void btnCopy_Click(object sender, EventArgs e)
        {
            SRTAnimationFrame frame;
            float* p = (float*) &frame;

            for (int i = 0; i < 5; i++)
            {
                if (!FrameScale.Checked && i < 2)
                {
                    p[i] = 1;
                }
                else if (
                    FrameScale.Checked && i < 2 ||
                    FrameRot.Checked && i == 2 ||
                    FrameTrans.Checked && i > 2)
                {
                    p[i] = _transBoxes[i].Value;
                }
            }

            DataObject da = new DataObject();
            da.SetData("SRTAnimationFrame", frame);
            Clipboard.SetDataObject(da, true);
        }

        private unsafe void btnPaste_Click(object sender, EventArgs e)
        {
            IDataObject da = Clipboard.GetDataObject();
            if (da.GetDataPresent("SRTAnimationFrame"))
            {
                object o = da.GetData("SRTAnimationFrame");
                if (o is SRTAnimationFrame)
                {
                    SRTAnimationFrame frame = (SRTAnimationFrame) o;

                    float* p = (float*) &frame;

                    BoxChangedCreateUndo(this, null);

                    for (int i = 0; i < 5; i++)
                    {
                        if (FrameScale.Checked && i < 2 ||
                            FrameRot.Checked && i == 2 ||
                            FrameTrans.Checked && i > 2)
                        {
                            _transBoxes[i].Value = p[i];
                        }

                        //_transBoxes[i].Value = p[i];
                        BoxChanged(_transBoxes[i], null);
                    }
                }
            }
        }

        private void btnInsert_Click(object sender, EventArgs e)
        {
            if (SelectedAnimation == null || CurrentFrame < 1)
            {
                return;
            }

            SelectedAnimation.InsertKeyframe(CurrentFrame - 1);
            //_mainWindow.SRT0StateChanged(this, null);
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (SelectedAnimation == null || CurrentFrame < 1)
            {
                return;
            }

            SelectedAnimation.DeleteKeyframe(CurrentFrame - 1);
            //_mainWindow.SRT0StateChanged(this, null);
        }

        private void btnClearFrame_Click(object sender, EventArgs e)
        {
            BoxChangedCreateUndo(this, null);

            for (int i = 0; i < 9; i++)
            {
                if (i == 2 || i == 4 || i == 5 || i == 8)
                {
                    continue;
                }

                _transBoxes[i].Value = float.NaN;
                BoxChanged(_transBoxes[i], null);
            }
        }

        private void addCustomAmountToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            if (SelectedAnimation == null || TexEntry == null)
            {
                return;
            }

            EditAllKeyframesDialog ed = new EditAllKeyframesDialog();
            //ed.ShowDialog(null, type, TexEntry);
            ResetBox(type);
            _mainWindow.UpdateModel();
        }
    }
}