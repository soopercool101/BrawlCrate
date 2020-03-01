using BrawlLib.SSBB.ResourceNodes;
using BrawlLib.SSBB.Types;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace BrawlLib.Internal.Windows.Forms.Moveset
{
    public class EventModifier : UserControl
    {
        private ComboBox cboType;
        private ListBox lstParameters;
        private Button btnChangeEvent;
        private Label lblEventId;
        private Label lblEventName;
        private Label lblParamDescription;
        private Button btnCancel;
        private Button btnDone;
        private Label lblName2;
        private PropertyGrid valueGrid;
        private Panel requirementPanel;
        private Panel mainPanel;
        private SplitContainer splitContainer1;
        private Panel typePanel;
        private CheckBox chkNot;
        private Label label1;
        private ComboBox cboRequirement;
        private Panel offsetPanel;
        private Button offsetOkay;
        private ComboBox comboBox1;
        private ComboBox comboBox2;
        private ComboBox comboBox3;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label lblName1;

        #region Designer

        private void InitializeComponent()
        {
            cboType = new ComboBox();
            lstParameters = new ListBox();
            btnChangeEvent = new Button();
            lblEventId = new Label();
            lblEventName = new Label();
            lblParamDescription = new Label();
            btnCancel = new Button();
            btnDone = new Button();
            lblName2 = new Label();
            lblName1 = new Label();
            valueGrid = new PropertyGrid();
            requirementPanel = new Panel();
            chkNot = new CheckBox();
            label1 = new Label();
            cboRequirement = new ComboBox();
            offsetPanel = new Panel();
            offsetOkay = new Button();
            comboBox1 = new ComboBox();
            comboBox2 = new ComboBox();
            comboBox3 = new ComboBox();
            label2 = new Label();
            label3 = new Label();
            label4 = new Label();
            mainPanel = new Panel();
            splitContainer1 = new SplitContainer();
            typePanel = new Panel();
            requirementPanel.SuspendLayout();
            offsetPanel.SuspendLayout();
            ((ISupportInitialize) splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            typePanel.SuspendLayout();
            SuspendLayout();
            // 
            // cboType
            // 
            cboType.FormattingEnabled = true;
            cboType.Items.AddRange(new object[]
            {
                "Value",
                "Scalar",
                "Drawing.Pointer",
                "Boolean",
                "Unknown",
                "Variable",
                "Requirement"
            });
            cboType.Location = new Point(46, 0);
            cboType.Name = "cboType";
            cboType.Size = new Size(82, 21);
            cboType.TabIndex = 63;
            cboType.SelectedIndexChanged += new EventHandler(cboType_SelectedIndexChanged);
            // 
            // lstParameters
            // 
            lstParameters.Dock = DockStyle.Fill;
            lstParameters.FormattingEnabled = true;
            lstParameters.Location = new Point(0, 21);
            lstParameters.Name = "lstParameters";
            lstParameters.Size = new Size(93, 92);
            lstParameters.TabIndex = 62;
            lstParameters.SelectedIndexChanged += new EventHandler(lstParameters_SelectedIndexChanged);
            // 
            // btnChangeEvent
            // 
            btnChangeEvent.Anchor = (AnchorStyles) (AnchorStyles.Top | AnchorStyles.Right);
            btnChangeEvent.Location = new Point(171, 2);
            btnChangeEvent.Name = "btnChangeEvent";
            btnChangeEvent.Size = new Size(56, 23);
            btnChangeEvent.TabIndex = 61;
            btnChangeEvent.Text = "Change";
            btnChangeEvent.UseVisualStyleBackColor = true;
            btnChangeEvent.Click += new EventHandler(btnChangeEvent_Click);
            // 
            // lblEventId
            // 
            lblEventId.Anchor = (AnchorStyles) (AnchorStyles.Top | AnchorStyles.Right);
            lblEventId.BackColor = Color.WhiteSmoke;
            lblEventId.BorderStyle = BorderStyle.Fixed3D;
            lblEventId.Location = new Point(107, 3);
            lblEventId.Name = "lblEventId";
            lblEventId.Size = new Size(66, 20);
            lblEventId.TabIndex = 60;
            lblEventId.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblEventName
            // 
            lblEventName.Anchor = (AnchorStyles) (AnchorStyles.Top | AnchorStyles.Left
                                                                   | AnchorStyles.Right);
            lblEventName.BackColor = Color.WhiteSmoke;
            lblEventName.BorderStyle = BorderStyle.Fixed3D;
            lblEventName.Location = new Point(2, 3);
            lblEventName.Name = "lblEventName";
            lblEventName.Size = new Size(105, 20);
            lblEventName.TabIndex = 59;
            lblEventName.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblParamDescription
            // 
            lblParamDescription.Anchor = (AnchorStyles) (AnchorStyles.Bottom | AnchorStyles.Left
                                                                             | AnchorStyles.Right);
            lblParamDescription.BackColor = SystemColors.Control;
            lblParamDescription.BorderStyle = BorderStyle.Fixed3D;
            lblParamDescription.Font = new Font("Microsoft Sans Serif", 9.75F, FontStyle.Regular,
                GraphicsUnit.Point,
                (byte) 0);
            lblParamDescription.Location = new Point(2, 140);
            lblParamDescription.Name = "lblParamDescription";
            lblParamDescription.Size = new Size(225, 63);
            lblParamDescription.TabIndex = 58;
            // 
            // btnCancel
            // 
            btnCancel.Anchor = (AnchorStyles) (AnchorStyles.Bottom | AnchorStyles.Right);
            btnCancel.Location = new Point(169, 206);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(58, 24);
            btnCancel.TabIndex = 57;
            btnCancel.Text = "* - Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Click += btnCancel_Click;
            // 
            // btnDone
            // 
            btnDone.Anchor = (AnchorStyles) (AnchorStyles.Bottom | AnchorStyles.Right);
            btnDone.Location = new Point(105, 206);
            btnDone.Name = "btnDone";
            btnDone.Size = new Size(58, 24);
            btnDone.TabIndex = 56;
            btnDone.Text = "* - Done";
            btnDone.UseVisualStyleBackColor = true;
            btnDone.Click += btnDone_Click;
            // 
            // lblName2
            // 
            lblName2.BackColor = Color.WhiteSmoke;
            lblName2.BorderStyle = BorderStyle.Fixed3D;
            lblName2.Location = new Point(0, 0);
            lblName2.Name = "lblName2";
            lblName2.Size = new Size(45, 21);
            lblName2.TabIndex = 55;
            lblName2.Text = "Type:";
            lblName2.TextAlign = ContentAlignment.MiddleRight;
            // 
            // lblName1
            // 
            lblName1.BackColor = Color.WhiteSmoke;
            lblName1.BorderStyle = BorderStyle.Fixed3D;
            lblName1.Dock = DockStyle.Top;
            lblName1.Location = new Point(0, 0);
            lblName1.Name = "lblName1";
            lblName1.Size = new Size(93, 21);
            lblName1.TabIndex = 54;
            lblName1.Text = "Parameter:";
            lblName1.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // valueGrid
            // 
            valueGrid.Dock = DockStyle.Fill;
            valueGrid.HelpVisible = false;
            valueGrid.Location = new Point(0, 21);
            valueGrid.Name = "valueGrid";
            valueGrid.Size = new Size(128, 92);
            valueGrid.TabIndex = 8;
            valueGrid.ToolbarVisible = false;
            // 
            // requirementPanel
            // 
            requirementPanel.Controls.Add(chkNot);
            requirementPanel.Controls.Add(label1);
            requirementPanel.Controls.Add(cboRequirement);
            requirementPanel.Dock = DockStyle.Fill;
            requirementPanel.Location = new Point(0, 21);
            requirementPanel.Name = "requirementPanel";
            requirementPanel.Size = new Size(128, 92);
            requirementPanel.TabIndex = 64;
            // 
            // chkNot
            // 
            chkNot.AutoSize = true;
            chkNot.Location = new Point(81, 3);
            chkNot.Name = "chkNot";
            chkNot.Size = new Size(43, 17);
            chkNot.TabIndex = 65;
            chkNot.Text = "Not";
            chkNot.UseVisualStyleBackColor = true;
            chkNot.CheckedChanged += new EventHandler(Requirement_Handle);
            // 
            // label1
            // 
            label1.BackColor = Color.WhiteSmoke;
            label1.BorderStyle = BorderStyle.Fixed3D;
            label1.Location = new Point(0, 0);
            label1.Name = "label1";
            label1.Size = new Size(75, 21);
            label1.TabIndex = 64;
            label1.Text = "Requirement:";
            label1.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // cboRequirement
            // 
            cboRequirement.Anchor = (AnchorStyles) (AnchorStyles.Top | AnchorStyles.Left
                                                                     | AnchorStyles.Right);
            cboRequirement.FormattingEnabled = true;
            cboRequirement.Location = new Point(0, 22);
            cboRequirement.Name = "cboRequirement";
            cboRequirement.Size = new Size(128, 21);
            cboRequirement.TabIndex = 0;
            cboRequirement.SelectedIndexChanged += new EventHandler(Requirement_Handle);
            // 
            // offsetPanel
            // 
            offsetPanel.Controls.Add(offsetOkay);
            offsetPanel.Controls.Add(comboBox1);
            offsetPanel.Controls.Add(comboBox2);
            offsetPanel.Controls.Add(comboBox3);
            offsetPanel.Controls.Add(label2);
            offsetPanel.Controls.Add(label3);
            offsetPanel.Controls.Add(label4);
            offsetPanel.Dock = DockStyle.Fill;
            offsetPanel.Location = new Point(0, 21);
            offsetPanel.Name = "offsetPanel";
            offsetPanel.Size = new Size(128, 92);
            offsetPanel.TabIndex = 66;
            // 
            // offsetOkay
            // 
            offsetOkay.Anchor = (AnchorStyles) (AnchorStyles.Top | AnchorStyles.Left
                                                                 | AnchorStyles.Right);
            offsetOkay.Location = new Point(-1, 69);
            offsetOkay.Name = "offsetOkay";
            offsetOkay.Size = new Size(129, 23);
            offsetOkay.TabIndex = 13;
            offsetOkay.Text = "Okay";
            offsetOkay.UseVisualStyleBackColor = true;
            offsetOkay.Click += new EventHandler(offsetOkay_Click);
            // 
            // comboBox1
            // 
            comboBox1.Anchor = (AnchorStyles) (AnchorStyles.Top | AnchorStyles.Left
                                                                | AnchorStyles.Right);
            comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox1.FormattingEnabled = true;
            comboBox1.Items.AddRange(new object[]
            {
                "Actions",
                "SubActions",
                "SubRoutines",
                "External",
                "Null"
            });
            comboBox1.Location = new Point(46, 3);
            comboBox1.Name = "comboBox1";
            comboBox1.Size = new Size(82, 21);
            comboBox1.TabIndex = 7;
            comboBox1.SelectedIndexChanged += new EventHandler(comboBox1_SelectedIndexChanged);
            // 
            // comboBox2
            // 
            comboBox2.Anchor = (AnchorStyles) (AnchorStyles.Top | AnchorStyles.Left
                                                                | AnchorStyles.Right);
            comboBox2.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox2.FormattingEnabled = true;
            comboBox2.Location = new Point(46, 24);
            comboBox2.Name = "comboBox2";
            comboBox2.Size = new Size(82, 21);
            comboBox2.TabIndex = 9;
            // 
            // comboBox3
            // 
            comboBox3.Anchor = (AnchorStyles) (AnchorStyles.Top | AnchorStyles.Left
                                                                | AnchorStyles.Right);
            comboBox3.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox3.FormattingEnabled = true;
            comboBox3.Location = new Point(46, 45);
            comboBox3.Name = "comboBox3";
            comboBox3.Size = new Size(82, 21);
            comboBox3.TabIndex = 11;
            // 
            // label2
            // 
            label2.BackColor = Color.WhiteSmoke;
            label2.BorderStyle = BorderStyle.Fixed3D;
            label2.Location = new Point(0, 3);
            label2.Name = "label2";
            label2.Size = new Size(45, 21);
            label2.TabIndex = 8;
            label2.Text = "List:";
            label2.TextAlign = ContentAlignment.MiddleRight;
            // 
            // label3
            // 
            label3.BackColor = Color.WhiteSmoke;
            label3.BorderStyle = BorderStyle.Fixed3D;
            label3.Location = new Point(0, 24);
            label3.Name = "label3";
            label3.Size = new Size(45, 21);
            label3.TabIndex = 10;
            label3.Text = "Action:";
            label3.TextAlign = ContentAlignment.MiddleRight;
            // 
            // label4
            // 
            label4.BackColor = Color.WhiteSmoke;
            label4.BorderStyle = BorderStyle.Fixed3D;
            label4.Location = new Point(0, 45);
            label4.Name = "label4";
            label4.Size = new Size(45, 21);
            label4.TabIndex = 12;
            label4.Text = "Type:";
            label4.TextAlign = ContentAlignment.MiddleRight;
            // 
            // mainPanel
            // 
            mainPanel.Anchor = (AnchorStyles) (AnchorStyles.Top | AnchorStyles.Bottom
                                                                | AnchorStyles.Left
                                                                | AnchorStyles.Right);
            mainPanel.Location = new Point(2, 25);
            mainPanel.Name = "mainPanel";
            mainPanel.Size = new Size(225, 113);
            mainPanel.TabIndex = 0;
            // 
            // splitContainer1
            // 
            splitContainer1.Anchor = (AnchorStyles) (AnchorStyles.Top | AnchorStyles.Bottom
                                                                      | AnchorStyles.Left
                                                                      | AnchorStyles.Right);
            splitContainer1.Location = new Point(2, 25);
            splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(lstParameters);
            splitContainer1.Panel1.Controls.Add(lblName1);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(offsetPanel);
            splitContainer1.Panel2.Controls.Add(valueGrid);
            splitContainer1.Panel2.Controls.Add(requirementPanel);
            splitContainer1.Panel2.Controls.Add(typePanel);
            splitContainer1.Size = new Size(225, 113);
            splitContainer1.SplitterDistance = 93;
            splitContainer1.TabIndex = 9;
            // 
            // typePanel
            // 
            typePanel.Controls.Add(lblName2);
            typePanel.Controls.Add(cboType);
            typePanel.Dock = DockStyle.Top;
            typePanel.Location = new Point(0, 0);
            typePanel.Name = "typePanel";
            typePanel.Size = new Size(128, 21);
            typePanel.TabIndex = 64;
            // 
            // EventModifier
            // 
            AutoSize = true;
            Controls.Add(splitContainer1);
            Controls.Add(mainPanel);
            Controls.Add(btnChangeEvent);
            Controls.Add(lblEventId);
            Controls.Add(lblEventName);
            Controls.Add(lblParamDescription);
            Controls.Add(btnCancel);
            Controls.Add(btnDone);
            Name = "EventModifier";
            Size = new Size(230, 233);
            requirementPanel.ResumeLayout(false);
            requirementPanel.PerformLayout();
            offsetPanel.ResumeLayout(false);
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel2.ResumeLayout(false);
            ((ISupportInitialize) splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            typePanel.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        public EventModifier()
        {
            InitializeComponent();
            frmEventList = new FormEventList();
        }

        public DialogResult status;
        public MoveDefEventNode origEvent;

        private MoveDefEventNode eventData => newEvent;
        private MoveDefEventNode newEv;

        private MoveDefEventNode newEvent
        {
            get
            {
                if (newEv == null)
                {
                    newEv = new MoveDefEventNode {_parent = origEvent.Parent};

                    newEv.EventID = origEvent._event;
                    ActionEventInfo info = origEvent.EventInfo;

                    for (int i = 0; i < newEv.numArguments; i++)
                    {
                        int type = 0, value = 0;
                        if (origEvent.Children.Count > i)
                        {
                            type = (int) (origEvent.Children[i] as MoveDefEventParameterNode)._type;
                            value = (int) (origEvent.Children[i] as MoveDefEventParameterNode)._value;
                        }

                        newEv.NewParam(i, value, type);
                        if (type == (int) ArgVarType.Offset)
                        {
                            MoveDefEventOffsetNode oldoff = origEvent.Children[i] as MoveDefEventOffsetNode;
                            MoveDefEventOffsetNode newoff = newEv.Children[i] as MoveDefEventOffsetNode;
                            newoff.list = oldoff.list;
                            newoff.index = oldoff.index;
                            newoff.type = oldoff.type;
                            newoff.action = oldoff.action;
                        }
                    }
                }

                return newEv;
            }
        }

        public MoveDefEventParameterNode param;

        //Display the event's name, offset and parameters.
        public void DisplayEvent()
        {
            lstParameters.Items.Clear();
            cboType.SelectedIndex = -1;
            cboType.Text = "";
            cboType.Enabled = false;
            lblParamDescription.Text = "No Description Available.";

            valueGrid.Visible = false;
            requirementPanel.Visible = false;
            offsetPanel.Visible = false;

            ActionEventInfo info = null;
            if (MoveDefNode.EventDictionary.ContainsKey(eventData._event))
            {
                info = MoveDefNode.EventDictionary[eventData._event];
            }

            if (info != null)
            {
                lblEventName.Text = info._name;
            }

            lblEventId.Text = Helpers.Hex8(eventData._event);

            foreach (MoveDefEventParameterNode n in eventData.Children)
            {
                if (!string.IsNullOrEmpty(n.Name))
                {
                    lstParameters.Items.Add(n.Name);
                }
            }
        }

        //Display the selected parameter's value, type and description.
        private void DisplayParameter(int index)
        {
            param = eventData.Children[index] as MoveDefEventParameterNode;

            cboType.Enabled = true;
            try
            {
                cboType.SelectedIndex = (int) param._type;
            }
            catch
            {
                cboType.SelectedIndex = -1;
                cboType.Text = "(" + param._type + ")";
            }

            DisplayInType(param);

            lblParamDescription.Text = param.Description;
        }

        //Display the parameter's value according to its type.
        public void DisplayInType(MoveDefEventParameterNode value)
        {
            if (value is MoveDefEventOffsetNode)
            {
                requirementPanel.Visible = false;
                valueGrid.Visible = false;
                offsetPanel.Visible = true;

                MoveDefEventOffsetNode offset = value as MoveDefEventOffsetNode;

                _updating = true;
                comboBox1.SelectedIndex = offset.list;
                if (offset.type != -1)
                {
                    comboBox3.SelectedIndex = offset.type;
                }

                if (offset.index != -1)
                {
                    comboBox2.SelectedIndex = offset.index;
                }

                _updating = false;
            }
            else
            {
                requirementPanel.Visible = false;
                valueGrid.Visible = true;
                offsetPanel.Visible = false;

                valueGrid.SelectedObject = value;
            }
        }

        public bool _updating;

        public FormEventList frmEventList;

        private void btnChangeEvent_Click(object sender, EventArgs e)
        {
            //Pass in the event Event.
            frmEventList.eventEvent = eventData._event;
            frmEventList.p = eventData.Root;
            frmEventList.ShowDialog();

            //Retrieve and setup the new event according to the new event Event.
            if (frmEventList.status == DialogResult.OK)
            {
                newEv = new MoveDefEventNode {_parent = origEvent.Parent};

                newEvent.EventID = (uint) frmEventList.eventEvent;
                ActionEventInfo info = newEvent.EventInfo;

                newEvent.NewChildren();
            }

            DisplayEvent();
        }

        private void lstParameters_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstParameters.SelectedIndex == -1)
            {
                return;
            }

            int index = lstParameters.SelectedIndex;
            DisplayParameter(index);
        }

        private void cboType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboType.SelectedIndex == -1)
            {
                return;
            }

            if (lstParameters.SelectedIndex == -1)
            {
                return;
            }

            int index = lstParameters.SelectedIndex;

            //Change the type to the type selected and update the view window.

            param = eventData.Children[index] as MoveDefEventParameterNode;

            if (param._type != (ArgVarType) cboType.SelectedIndex)
            {
                int ind = param.Index;
                ActionEventInfo info = eventData.EventInfo;
                string name = ((ArgVarType) cboType.SelectedIndex).ToString();

                int value = 0;

                MoveDefEventParameterNode p = newEvent.Children[ind] as MoveDefEventParameterNode;
                if (p is MoveDefEventValueNode || p is MoveDefEventScalarNode || p is MoveDefEventBoolNode)
                {
                    value = p._value;
                }

                newEvent.Children[ind].Remove();

                ArgVarType t = (ArgVarType) cboType.SelectedIndex;

                newEvent.NewParam(ind, value, (int) t);
            }

            DisplayParameter(index);
        }

        private void Requirement_Handle(object sender, EventArgs e)
        {
            if (cboRequirement.SelectedIndex == -1)
            {
                return;
            }

            if (lstParameters.SelectedIndex == -1)
            {
                return;
            }

            int index = lstParameters.SelectedIndex;
            long value = cboRequirement.SelectedIndex;
            if (chkNot.Checked)
            {
                value |= 0x80000000;
            }

            (newEvent.Children[index] as MoveDefEventParameterNode)._value = (int) value;
        }

        public event EventHandler Completed;

        private void btnCancel_Click(object sender, EventArgs e)
        {
            status = DialogResult.Cancel;

            Completed?.Invoke(this, null);
        }

        private void btnDone_Click(object sender, EventArgs e)
        {
            if (newEv == null) //No changes were made.
            {
                btnCancel_Click(sender, e);
                return;
            }

            status = DialogResult.OK;
            int index = origEvent.Index;
            MoveDefActionNode action = origEvent.Parent as MoveDefActionNode;
            origEvent.Remove();
            action.InsertChild(newEvent, true, index);

            Completed?.Invoke(this, null);
        }

        public object _oldSelectedObject;

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == 0)
            {
                comboBox3.Items.Clear();
                comboBox3.Items.Add("Entry");
                comboBox3.Items.Add("Exit");

                comboBox2.Items.Clear();
                comboBox2.Items.AddRange(param.Root._actions.Children.ToArray());
            }

            if (comboBox1.SelectedIndex == 1)
            {
                comboBox3.Items.Clear();
                comboBox3.Items.Add("Main");
                comboBox3.Items.Add("GFX");
                comboBox3.Items.Add("SFX");
                comboBox3.Items.Add("Other");

                comboBox2.Items.Clear();
                comboBox2.Items.AddRange(param.Root._subActions.Children.ToArray());
            }

            if (comboBox1.SelectedIndex >= 2)
            {
                comboBox3.Visible = label4.Visible = false;
            }
            else
            {
                comboBox3.Visible = label4.Visible = true;
            }

            if (comboBox1.SelectedIndex == 4)
            {
                comboBox2.Visible = label3.Visible = label2.Visible = false;
            }
            else
            {
                comboBox2.Visible = label3.Visible = label2.Visible = true;
            }

            if (comboBox1.SelectedIndex == 2)
            {
                comboBox2.Items.Clear();
                comboBox2.Items.AddRange(param.Root._subRoutineList.ToArray());
            }

            if (comboBox1.SelectedIndex == 3)
            {
                comboBox2.Items.Clear();
                comboBox2.Items.AddRange(param.Root._externalRefs.ToArray());
            }
        }

        private void offsetOkay_Click(object sender, EventArgs e)
        {
            MoveDefEventOffsetNode _targetNode = param as MoveDefEventOffsetNode;
            if (_targetNode.action != null)
            {
                _targetNode._value = -1;
                _targetNode.action._actionRefs.Remove(param);
            }

            if (comboBox1.SelectedIndex >= 3)
            {
                if (comboBox1.SelectedIndex == 3 && comboBox2.SelectedIndex >= 0 &&
                    comboBox2.SelectedIndex < param.Root._externalRefs.Count)
                {
                    if (_targetNode._extNode != null)
                    {
                        _targetNode._extNode._refs.Remove(_targetNode);
                        _targetNode._extNode = null;
                    }

                    (param._extNode = param.Root._externalRefs[comboBox2.SelectedIndex] as MoveDefExternalNode)._refs
                        .Add(
                            param);
                }
            }
            else
            {
                if (param._extNode != null)
                {
                    param._extNode._refs.Remove(param);
                    param._extNode = null;
                }
            }

            _targetNode.list = comboBox1.SelectedIndex;
            _targetNode.type = comboBox1.SelectedIndex >= 2 ? -1 : comboBox3.SelectedIndex;
            _targetNode.index = comboBox1.SelectedIndex == 4 ? -1 : comboBox2.SelectedIndex;
            _targetNode.action = param.Root.GetAction(_targetNode.list, _targetNode.type, _targetNode.index);
            if (_targetNode.action != null)
            {
                param._value = _targetNode.action._offset;
                _targetNode.action._actionRefs.Add(param);
            }
            else
            {
                param._value = -1;
            }

            param.SignalPropertyChange();
        }
    }
}