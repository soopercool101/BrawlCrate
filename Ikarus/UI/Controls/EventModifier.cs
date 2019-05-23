using Ikarus;
using Ikarus.MovesetFile;
using Ikarus.ModelViewer;
using BrawlLib.SSBBTypes;

namespace System.Windows.Forms
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
            this.cboType = new System.Windows.Forms.ComboBox();
            this.lstParameters = new System.Windows.Forms.ListBox();
            this.btnChangeEvent = new System.Windows.Forms.Button();
            this.lblEventId = new System.Windows.Forms.Label();
            this.lblEventName = new System.Windows.Forms.Label();
            this.lblParamDescription = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnDone = new System.Windows.Forms.Button();
            this.lblName2 = new System.Windows.Forms.Label();
            this.lblName1 = new System.Windows.Forms.Label();
            this.valueGrid = new System.Windows.Forms.PropertyGrid();
            this.requirementPanel = new System.Windows.Forms.Panel();
            this.chkNot = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cboRequirement = new System.Windows.Forms.ComboBox();
            this.offsetPanel = new System.Windows.Forms.Panel();
            this.offsetOkay = new System.Windows.Forms.Button();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.comboBox2 = new System.Windows.Forms.ComboBox();
            this.comboBox3 = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.mainPanel = new System.Windows.Forms.Panel();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.typePanel = new System.Windows.Forms.Panel();
            this.requirementPanel.SuspendLayout();
            this.offsetPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.typePanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // cboType
            // 
            this.cboType.FormattingEnabled = true;
            this.cboType.Items.AddRange(new object[] {
            "Value",
            "Scalar",
            "Pointer",
            "Boolean",
            "Unknown",
            "Variable",
            "Requirement"});
            this.cboType.Location = new System.Drawing.Point(46, 0);
            this.cboType.Name = "cboType";
            this.cboType.Size = new System.Drawing.Size(82, 21);
            this.cboType.TabIndex = 63;
            this.cboType.SelectedIndexChanged += new System.EventHandler(this.cboType_SelectedIndexChanged);
            // 
            // lstParameters
            // 
            this.lstParameters.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstParameters.FormattingEnabled = true;
            this.lstParameters.IntegralHeight = false;
            this.lstParameters.Location = new System.Drawing.Point(0, 21);
            this.lstParameters.Name = "lstParameters";
            this.lstParameters.Size = new System.Drawing.Size(93, 92);
            this.lstParameters.TabIndex = 62;
            this.lstParameters.SelectedIndexChanged += new System.EventHandler(this.lstParameters_SelectedIndexChanged);
            // 
            // btnChangeEvent
            // 
            this.btnChangeEvent.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnChangeEvent.Location = new System.Drawing.Point(171, 2);
            this.btnChangeEvent.Name = "btnChangeEvent";
            this.btnChangeEvent.Size = new System.Drawing.Size(56, 23);
            this.btnChangeEvent.TabIndex = 61;
            this.btnChangeEvent.Text = "Change";
            this.btnChangeEvent.UseVisualStyleBackColor = true;
            this.btnChangeEvent.Click += new System.EventHandler(this.btnChangeEvent_Click);
            // 
            // lblEventId
            // 
            this.lblEventId.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblEventId.BackColor = System.Drawing.Color.WhiteSmoke;
            this.lblEventId.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblEventId.Location = new System.Drawing.Point(107, 3);
            this.lblEventId.Name = "lblEventId";
            this.lblEventId.Size = new System.Drawing.Size(66, 20);
            this.lblEventId.TabIndex = 60;
            this.lblEventId.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblEventName
            // 
            this.lblEventName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblEventName.BackColor = System.Drawing.Color.WhiteSmoke;
            this.lblEventName.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblEventName.Location = new System.Drawing.Point(2, 3);
            this.lblEventName.Name = "lblEventName";
            this.lblEventName.Size = new System.Drawing.Size(105, 20);
            this.lblEventName.TabIndex = 59;
            this.lblEventName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblParamDescription
            // 
            this.lblParamDescription.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblParamDescription.BackColor = System.Drawing.SystemColors.Control;
            this.lblParamDescription.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblParamDescription.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblParamDescription.Location = new System.Drawing.Point(2, 140);
            this.lblParamDescription.Name = "lblParamDescription";
            this.lblParamDescription.Size = new System.Drawing.Size(225, 63);
            this.lblParamDescription.TabIndex = 58;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Location = new System.Drawing.Point(169, 206);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(58, 24);
            this.btnCancel.TabIndex = 57;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnDone
            // 
            this.btnDone.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDone.Location = new System.Drawing.Point(105, 206);
            this.btnDone.Name = "btnDone";
            this.btnDone.Size = new System.Drawing.Size(58, 24);
            this.btnDone.TabIndex = 56;
            this.btnDone.Text = "Done";
            this.btnDone.UseVisualStyleBackColor = true;
            this.btnDone.Click += new System.EventHandler(this.btnDone_Click);
            // 
            // lblName2
            // 
            this.lblName2.BackColor = System.Drawing.Color.WhiteSmoke;
            this.lblName2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblName2.Location = new System.Drawing.Point(0, 0);
            this.lblName2.Name = "lblName2";
            this.lblName2.Size = new System.Drawing.Size(45, 21);
            this.lblName2.TabIndex = 55;
            this.lblName2.Text = "Type:";
            this.lblName2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblName1
            // 
            this.lblName1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.lblName1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblName1.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblName1.Location = new System.Drawing.Point(0, 0);
            this.lblName1.Name = "lblName1";
            this.lblName1.Size = new System.Drawing.Size(93, 21);
            this.lblName1.TabIndex = 54;
            this.lblName1.Text = "Parameter:";
            this.lblName1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // valueGrid
            // 
            this.valueGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.valueGrid.HelpVisible = false;
            this.valueGrid.Location = new System.Drawing.Point(0, 21);
            this.valueGrid.Name = "valueGrid";
            this.valueGrid.Size = new System.Drawing.Size(128, 92);
            this.valueGrid.TabIndex = 8;
            this.valueGrid.ToolbarVisible = false;
            // 
            // requirementPanel
            // 
            this.requirementPanel.Controls.Add(this.chkNot);
            this.requirementPanel.Controls.Add(this.label1);
            this.requirementPanel.Controls.Add(this.cboRequirement);
            this.requirementPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.requirementPanel.Location = new System.Drawing.Point(0, 21);
            this.requirementPanel.Name = "requirementPanel";
            this.requirementPanel.Size = new System.Drawing.Size(128, 92);
            this.requirementPanel.TabIndex = 64;
            // 
            // chkNot
            // 
            this.chkNot.AutoSize = true;
            this.chkNot.Location = new System.Drawing.Point(81, 3);
            this.chkNot.Name = "chkNot";
            this.chkNot.Size = new System.Drawing.Size(43, 17);
            this.chkNot.TabIndex = 65;
            this.chkNot.Text = "Not";
            this.chkNot.UseVisualStyleBackColor = true;
            this.chkNot.CheckedChanged += new System.EventHandler(this.Requirement_Handle);
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(75, 21);
            this.label1.TabIndex = 64;
            this.label1.Text = "Requirement:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cboRequirement
            // 
            this.cboRequirement.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cboRequirement.FormattingEnabled = true;
            this.cboRequirement.Location = new System.Drawing.Point(0, 22);
            this.cboRequirement.Name = "cboRequirement";
            this.cboRequirement.Size = new System.Drawing.Size(128, 21);
            this.cboRequirement.TabIndex = 0;
            this.cboRequirement.SelectedIndexChanged += new System.EventHandler(this.Requirement_Handle);
            // 
            // offsetPanel
            // 
            this.offsetPanel.Controls.Add(this.offsetOkay);
            this.offsetPanel.Controls.Add(this.comboBox1);
            this.offsetPanel.Controls.Add(this.comboBox2);
            this.offsetPanel.Controls.Add(this.comboBox3);
            this.offsetPanel.Controls.Add(this.label2);
            this.offsetPanel.Controls.Add(this.label3);
            this.offsetPanel.Controls.Add(this.label4);
            this.offsetPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.offsetPanel.Location = new System.Drawing.Point(0, 21);
            this.offsetPanel.Name = "offsetPanel";
            this.offsetPanel.Size = new System.Drawing.Size(128, 92);
            this.offsetPanel.TabIndex = 66;
            // 
            // offsetOkay
            // 
            this.offsetOkay.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.offsetOkay.Location = new System.Drawing.Point(-1, 69);
            this.offsetOkay.Name = "offsetOkay";
            this.offsetOkay.Size = new System.Drawing.Size(129, 23);
            this.offsetOkay.TabIndex = 13;
            this.offsetOkay.Text = "Okay";
            this.offsetOkay.UseVisualStyleBackColor = true;
            this.offsetOkay.Click += new System.EventHandler(this.offsetOkay_Click);
            // 
            // comboBox1
            // 
            this.comboBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "Actions",
            "SubActions",
            "SubRoutines",
            "External",
            "Null"});
            this.comboBox1.Location = new System.Drawing.Point(46, 3);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(82, 21);
            this.comboBox1.TabIndex = 7;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // comboBox2
            // 
            this.comboBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBox2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox2.FormattingEnabled = true;
            this.comboBox2.Location = new System.Drawing.Point(46, 24);
            this.comboBox2.Name = "comboBox2";
            this.comboBox2.Size = new System.Drawing.Size(82, 21);
            this.comboBox2.TabIndex = 9;
            // 
            // comboBox3
            // 
            this.comboBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBox3.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox3.FormattingEnabled = true;
            this.comboBox3.Location = new System.Drawing.Point(46, 45);
            this.comboBox3.Name = "comboBox3";
            this.comboBox3.Size = new System.Drawing.Size(82, 21);
            this.comboBox3.TabIndex = 11;
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.WhiteSmoke;
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label2.Location = new System.Drawing.Point(0, 3);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(45, 21);
            this.label2.TabIndex = 8;
            this.label2.Text = "List:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.WhiteSmoke;
            this.label3.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label3.Location = new System.Drawing.Point(0, 24);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(45, 21);
            this.label3.TabIndex = 10;
            this.label3.Text = "Action:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label4
            // 
            this.label4.BackColor = System.Drawing.Color.WhiteSmoke;
            this.label4.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label4.Location = new System.Drawing.Point(0, 45);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(45, 21);
            this.label4.TabIndex = 12;
            this.label4.Text = "Type:";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // mainPanel
            // 
            this.mainPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.mainPanel.Location = new System.Drawing.Point(2, 25);
            this.mainPanel.Name = "mainPanel";
            this.mainPanel.Size = new System.Drawing.Size(225, 113);
            this.mainPanel.TabIndex = 0;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.Location = new System.Drawing.Point(2, 25);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.lstParameters);
            this.splitContainer1.Panel1.Controls.Add(this.lblName1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.offsetPanel);
            this.splitContainer1.Panel2.Controls.Add(this.valueGrid);
            this.splitContainer1.Panel2.Controls.Add(this.requirementPanel);
            this.splitContainer1.Panel2.Controls.Add(this.typePanel);
            this.splitContainer1.Size = new System.Drawing.Size(225, 113);
            this.splitContainer1.SplitterDistance = 93;
            this.splitContainer1.TabIndex = 9;
            // 
            // typePanel
            // 
            this.typePanel.Controls.Add(this.lblName2);
            this.typePanel.Controls.Add(this.cboType);
            this.typePanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.typePanel.Location = new System.Drawing.Point(0, 0);
            this.typePanel.Name = "typePanel";
            this.typePanel.Size = new System.Drawing.Size(128, 21);
            this.typePanel.TabIndex = 64;
            // 
            // EventModifier
            // 
            this.AutoSize = true;
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.mainPanel);
            this.Controls.Add(this.btnChangeEvent);
            this.Controls.Add(this.lblEventId);
            this.Controls.Add(this.lblEventName);
            this.Controls.Add(this.lblParamDescription);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnDone);
            this.Name = "EventModifier";
            this.Size = new System.Drawing.Size(230, 233);
            this.requirementPanel.ResumeLayout(false);
            this.requirementPanel.PerformLayout();
            this.offsetPanel.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.typePanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        public EventModifier() { InitializeComponent(); frmEventList = new FormEventList(); }

        public DialogResult _status;

        private Event _origEvent;
        private Event _newEv = null;
        public Event NewEvent 
        {
            get 
            {
                //Generate a copy of the original event
                //So that changes don't affect the original event until the user finishes
                if (_newEv == null)
                {
                    _newEv = new Event()
                    {
                        EventID = _origEvent.EventID,
                        _offset = _origEvent._offset,
                        _initSize = _origEvent._initSize,
                    };

                    EventInformation info = _origEvent.Info;
                    for (int i = 0; i < _newEv.NumArguments; i++)
                    {
                        Parameter p = _origEvent[i];
                        int type = 0, value = 0;
                        if (_origEvent.Count > i)
                        {
                            type = (int)p.ParamType;
                            value = p.Data;
                        }

                        MovesetEntryNode e = _newEv.NewParam(i, value, type);
                        e._offset = p._offset;
                        e._initSize = p._initSize;

                        if (type == (int)ParamType.Offset)
                        {
                            EventOffset oldoff = p as EventOffset;
                            EventOffset newoff = _newEv[i] as EventOffset;
                            newoff._offsetInfo = oldoff._offsetInfo;
                            newoff._script = oldoff._script;
                        }
                    }

                    //Set the node to be "Clean", so that if the user makes a change and cancels, we'll know
                    _newEv.IsDirty = false;
                }
                return _newEv;
            }
        }

        public Parameter param = null;

        public void Setup(Event original)
        {
            _origEvent = original;

            //Setup requirements list.
            if (cboRequirement.Items.Count == 0)
                cboRequirement.Items.AddRange(Manager.iRequirements);

            _status = DialogResult.Cancel;
            _newEv = null;

            DisplayEvent();
        }

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

            EventInformation info = null;
            if (Manager.Events.ContainsKey(NewEvent.EventID))
                info = Manager.Events[NewEvent.EventID];

            if (info != null)
                lblEventName.Text = info._name;

            lblEventId.Text = Helpers.Hex8(NewEvent.EventID);

            foreach (Parameter n in NewEvent)
                lstParameters.Items.Add(n);
        }

        //Display the selected parameter's value, type and description.
        private void DisplayParameter(int index)
        {
            param = NewEvent[index];

            cboType.Enabled = true;
            try { cboType.SelectedIndex = (int)param.ParamType; }
            catch { cboType.SelectedIndex = -1; cboType.Text = "(" + param.ParamType + ")"; }
            DisplayInType(param);

            lblParamDescription.Text = param.Description;
        }

        //Display the parameter's value according to its type.
        public void DisplayInType(Parameter value)
        {
            if (value is EventOffset)
            {
                requirementPanel.Visible = false;
                valueGrid.Visible = false;
                offsetPanel.Visible = true;

                EventOffset offset = value as EventOffset;

                _updating = true;
                comboBox1.SelectedIndex = (int)offset._offsetInfo._list;
                if (offset._offsetInfo._type != TypeValue.None)
                    comboBox3.SelectedIndex = (int)offset._offsetInfo._type;
                if (offset._offsetInfo._index != -1)
                    comboBox2.SelectedIndex = offset._offsetInfo._index;
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

        public bool _updating = false;

        public FormEventList frmEventList;
        private void btnChangeEvent_Click(object sender, EventArgs e)
        {
            //Pass in the event Event.
            frmEventList.eventEvent = NewEvent.EventID;
            frmEventList.p = NewEvent._root;
            frmEventList.ShowDialog();

            //Retrieve and setup the new event according to the new event Event.
            if (frmEventList.status == DialogResult.OK)
            {
                _newEv = new Event() { _script = _origEvent._script };

                NewEvent.EventID = (uint)frmEventList.eventEvent;
                EventInformation info = NewEvent.Info;

                NewEvent.Reset();
            }

            DisplayEvent();
        }

        private void lstParameters_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstParameters.SelectedIndex == -1) return;
            int index = lstParameters.SelectedIndex;
            DisplayParameter(index);
        }

        private void cboType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboType.SelectedIndex == -1) return;
            if (lstParameters.SelectedIndex == -1) return;
            int index = lstParameters.SelectedIndex;

            //Change the type to the type selected and update the view window.

            param = NewEvent[index];

            if (param.ParamType != (ParamType)cboType.SelectedIndex)
            {
                int ind = param.Index;
                EventInformation info = NewEvent.Info;
                string name = ((ParamType)cboType.SelectedIndex).ToString();

                int value = 0;

                Parameter p = NewEvent[ind];
                if (p is EventValue || p is EventScalar || p is EventBool)
                    value = p.Data;

                NewEvent.RemoveAt(ind);

                ParamType t = ((ParamType)cboType.SelectedIndex);

                NewEvent.NewParam(ind, value, (int)t);
            }

            DisplayParameter(index);
        }

        private void Requirement_Handle(object sender, EventArgs e)
        {
            if (cboRequirement.SelectedIndex == -1) return;
            if (lstParameters.SelectedIndex == -1) return;
            int index = lstParameters.SelectedIndex;
            long value = cboRequirement.SelectedIndex;
            if (chkNot.Checked) value |= 0x80000000;

            (NewEvent[index]).Data = (int)value;
        }

        public event EventHandler Completed;

        private void btnCancel_Click(object sender, EventArgs e)
        {
            //if (NewEvent.IsDirty)
            //    if (MessageBox.Show("Are you sure you want to cancel editing this event? You have unsaved changes.", "Done Editing?", MessageBoxButtons.YesNo) == DialogResult.No)
            //        return;

            _status = DialogResult.Cancel;

            if (Completed != null)
                Completed(this, null);
        }

        private void btnDone_Click(object sender, EventArgs e)
        {
            //if (!NewEvent.IsDirty) //No changes were made.
            //{
            //    btnCancel_Click(sender, e);
            //    return;
            //}

            _status = DialogResult.OK;

            int index = _origEvent.Index;
            Script action = _origEvent._script as Script;
            action.RemoveAt(index);
            action.Insert(index, NewEvent);

            if (Completed != null)
                Completed(this, null);
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox3.Visible = label4.Visible = comboBox1.SelectedIndex < 2;
            comboBox2.Visible = label3.Visible = label2.Visible = comboBox1.SelectedIndex != 4;
            comboBox3.Items.Clear();
            switch (comboBox1.SelectedIndex)
            {
                case 0:
                    comboBox3.Items.Add("Entry");
                    comboBox3.Items.Add("Exit");
                    comboBox2.DataSource = ((MovesetNode)param._root).Actions;
                    break;
                case 1:
                    comboBox3.Items.Add("Main");
                    comboBox3.Items.Add("GFX");
                    comboBox3.Items.Add("SFX");
                    comboBox3.Items.Add("Other");
                    comboBox2.DataSource = ((MovesetNode)param._root).Data.SubActions;
                    break;
                case 2:
                    comboBox2.DataSource = ((MovesetNode)param._root).SubRoutines;
                    break;
                case 3:
                    comboBox2.DataSource = param._root.ReferenceList;
                    break;
                default:
                    comboBox2.DataSource = null;
                    break;
            }
        }

        private void offsetOkay_Click(object sender, EventArgs e)
        {
            EventOffset _targetNode = param as EventOffset;
            if (_targetNode._script != null)
            {
                _targetNode.Data = -1;
                _targetNode._script._actionRefs.Remove(param);
            }
            if (comboBox1.SelectedIndex >= 3)
            {
                if (comboBox1.SelectedIndex == 3 && comboBox2.SelectedIndex >= 0 && comboBox2.SelectedIndex < param._root.ReferenceList.Count)
                {
                    if (_targetNode._externalEntry != null)
                    {
                        _targetNode._externalEntry.References.Remove(_targetNode);
                        _targetNode._externalEntry = null;
                    }
                    (param._externalEntry = param._root.ReferenceList[comboBox2.SelectedIndex] as TableEntryNode).References.Add(param);
                }
            }
            else
            {
                if (param._externalEntry != null)
                {
                    param._externalEntry.References.Remove(param);
                    param._externalEntry = null;
                }
            }

            _targetNode._offsetInfo = new ScriptOffsetInfo(
                (ListValue)comboBox1.SelectedIndex,
                (TypeValue)(comboBox1.SelectedIndex >= 2 ? -1 : comboBox3.SelectedIndex),
                (comboBox1.SelectedIndex == 4 ? -1 : comboBox2.SelectedIndex));

            _targetNode._script = ((MovesetNode)param._root).GetScript(_targetNode._offsetInfo);
            if (_targetNode._script != null)
                _targetNode._script._actionRefs.Add(param);
            else
                param.Data = -1;
        }
    }
}
