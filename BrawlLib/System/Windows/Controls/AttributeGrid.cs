using System.ComponentModel;
using BrawlLib.SSBB.ResourceNodes;
using System.Data;

namespace System.Windows.Forms
{
    public unsafe class AttributeGrid : UserControl
    {
        #region Designer

        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dtgrdAttributes = new System.Windows.Forms.DataGridView();
            this.description = new System.Windows.Forms.RichTextBox();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.panel1 = new System.Windows.Forms.Panel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.rdoDegrees = new System.Windows.Forms.RadioButton();
            this.rdoInt = new System.Windows.Forms.RadioButton();
            this.rdoFloat = new System.Windows.Forms.RadioButton();
            ((System.ComponentModel.ISupportInitialize)(this.dtgrdAttributes)).BeginInit();
            this.panel1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // dtgrdAttributes
            // 
            this.dtgrdAttributes.AllowUserToAddRows = false;
            this.dtgrdAttributes.AllowUserToDeleteRows = false;
            this.dtgrdAttributes.AllowUserToResizeRows = false;
            this.dtgrdAttributes.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dtgrdAttributes.BackgroundColor = System.Drawing.SystemColors.ControlLightLight;
            this.dtgrdAttributes.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dtgrdAttributes.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dtgrdAttributes.ColumnHeadersVisible = false;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.Format = "N4";
            dataGridViewCellStyle1.NullValue = null;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dtgrdAttributes.DefaultCellStyle = dataGridViewCellStyle1;
            this.dtgrdAttributes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dtgrdAttributes.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnKeystroke;
            this.dtgrdAttributes.EnableHeadersVisualStyles = false;
            this.dtgrdAttributes.GridColor = System.Drawing.SystemColors.ControlLight;
            this.dtgrdAttributes.Location = new System.Drawing.Point(0, 0);
            this.dtgrdAttributes.MultiSelect = false;
            this.dtgrdAttributes.Name = "dtgrdAttributes";
            this.dtgrdAttributes.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.dtgrdAttributes.RowHeadersWidth = 8;
            this.dtgrdAttributes.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dtgrdAttributes.RowTemplate.Height = 16;
            this.dtgrdAttributes.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dtgrdAttributes.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dtgrdAttributes.Size = new System.Drawing.Size(479, 239);
            this.dtgrdAttributes.TabIndex = 5;
            this.dtgrdAttributes.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dtgrdAttributes_CellEndEdit);
            this.dtgrdAttributes.CurrentCellChanged += new System.EventHandler(this.dtgrdAttributes_CurrentCellChanged);
            // 
            // description
            // 
            this.description.BackColor = System.Drawing.SystemColors.Control;
            this.description.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.description.Cursor = System.Windows.Forms.Cursors.Default;
            this.description.Dock = System.Windows.Forms.DockStyle.Fill;
            this.description.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.description.ForeColor = System.Drawing.Color.Black;
            this.description.Location = new System.Drawing.Point(0, 0);
            this.description.Name = "description";
            this.description.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.description.Size = new System.Drawing.Size(479, 63);
            this.description.TabIndex = 6;
            this.description.Text = "No Description Available.";
            this.description.TextChanged += new System.EventHandler(this.description_TextChanged);
            // 
            // splitter1
            // 
            this.splitter1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.splitter1.Location = new System.Drawing.Point(0, 239);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(479, 3);
            this.splitter1.TabIndex = 7;
            this.splitter1.TabStop = false;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.tableLayoutPanel1);
            this.panel1.Controls.Add(this.description);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 242);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(479, 63);
            this.panel1.TabIndex = 8;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.rdoDegrees, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.rdoInt, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.rdoFloat, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Right;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(458, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(21, 63);
            this.tableLayoutPanel1.TabIndex = 9;
            // 
            // rdoDegrees
            // 
            this.rdoDegrees.Appearance = System.Windows.Forms.Appearance.Button;
            this.rdoDegrees.AutoSize = true;
            this.rdoDegrees.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rdoDegrees.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rdoDegrees.Location = new System.Drawing.Point(0, 42);
            this.rdoDegrees.Margin = new System.Windows.Forms.Padding(0);
            this.rdoDegrees.Name = "rdoDegrees";
            this.rdoDegrees.Size = new System.Drawing.Size(21, 21);
            this.rdoDegrees.TabIndex = 2;
            this.rdoDegrees.TabStop = true;
            this.rdoDegrees.Text = "D";
            this.rdoDegrees.UseVisualStyleBackColor = true;
            this.rdoDegrees.CheckedChanged += new System.EventHandler(this.radioButtonsChanged);
            // 
            // rdoInt
            // 
            this.rdoInt.Appearance = System.Windows.Forms.Appearance.Button;
            this.rdoInt.AutoSize = true;
            this.rdoInt.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rdoInt.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rdoInt.Location = new System.Drawing.Point(0, 21);
            this.rdoInt.Margin = new System.Windows.Forms.Padding(0);
            this.rdoInt.Name = "rdoInt";
            this.rdoInt.Size = new System.Drawing.Size(21, 21);
            this.rdoInt.TabIndex = 1;
            this.rdoInt.TabStop = true;
            this.rdoInt.Text = "I";
            this.rdoInt.UseVisualStyleBackColor = true;
            this.rdoInt.CheckedChanged += new System.EventHandler(this.radioButtonsChanged);
            // 
            // rdoFloat
            // 
            this.rdoFloat.Appearance = System.Windows.Forms.Appearance.Button;
            this.rdoFloat.AutoSize = true;
            this.rdoFloat.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rdoFloat.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rdoFloat.Location = new System.Drawing.Point(0, 0);
            this.rdoFloat.Margin = new System.Windows.Forms.Padding(0);
            this.rdoFloat.Name = "rdoFloat";
            this.rdoFloat.Size = new System.Drawing.Size(21, 21);
            this.rdoFloat.TabIndex = 0;
            this.rdoFloat.TabStop = true;
            this.rdoFloat.Text = "F";
            this.rdoFloat.UseVisualStyleBackColor = true;
            this.rdoFloat.CheckedChanged += new System.EventHandler(this.radioButtonsChanged);
            // 
            // AttributeGrid
            // 
            this.Controls.Add(this.dtgrdAttributes);
            this.Controls.Add(this.splitter1);
            this.Controls.Add(this.panel1);
            this.Name = "AttributeGrid";
            this.Size = new System.Drawing.Size(479, 305);
            ((System.ComponentModel.ISupportInitialize)(this.dtgrdAttributes)).EndInit();
            this.panel1.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        public AttributeInfo[] AttributeArray { get; set; }

        public AttributeGrid()
        {
            InitializeComponent();
        }

        private DataGridView dtgrdAttributes;
        public RichTextBox description;
        private Splitter splitter1;
        public bool called = false;
        private Panel panel1;
        private TableLayoutPanel tableLayoutPanel1;
        private RadioButton rdoFloat;
        private RadioButton rdoInt;
        private RadioButton rdoDegrees;

        public event EventHandler CellEdited;
        public event EventHandler DictionaryChanged;

        private IAttributeList _targetNode;
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public IAttributeList TargetNode
        {
            get { return _targetNode; }
            set { _targetNode = value; if (!called && value != null) { LoadData(); called = true; } TargetChanged(); }
        }

        public void LoadData()
        {
            attributes.Columns.Clear();
            attributes.Rows.Clear();

            //Setup Attribute Table.
            attributes.Columns.Add("Name");
            attributes.Columns.Add("Value");
            //attributes.Columns[0].ReadOnly = true;
            dtgrdAttributes.DataSource = attributes;
        }

        DataTable attributes = new DataTable();
        public unsafe void TargetChanged()
        {
            if (TargetNode == null)
                return;

            attributes.Rows.Clear();
            for (int i = 0; i < TargetNode.NumEntries; i++) {
                if (i < AttributeArray.Length)
                    attributes.Rows.Add(AttributeArray[i]._name);
                else
                    attributes.Rows.Add("0x" + (i * 4).ToString("X"));
            }

            //Add attributes to the attribute table.
            for (int i = 0; i < TargetNode.NumEntries; i++)
                RefreshRow(i);
        }

        private void RefreshRow(int i) {
            if (AttributeArray.Length <= i || AttributeArray[i]._type == 2)
                attributes.Rows[i][1] = (float)((bfloat*)TargetNode.AttributeAddress)[i] * Maths._rad2degf;
            else if (AttributeArray[i]._type == 1)
                attributes.Rows[i][1] = (int)((bint*)TargetNode.AttributeAddress)[i];
            else
                attributes.Rows[i][1] = (float)((bfloat*)TargetNode.AttributeAddress)[i];
        }

        public void SetFloat(int index, float value) { TargetNode.SetFloat(index, value); }
        public float GetFloat(int index) { return TargetNode.GetFloat(index); }
        public void SetInt(int index, int value) { TargetNode.SetInt(index, value); }
        public int GetInt(int index) { return TargetNode.GetInt(index); }

        private unsafe void dtgrdAttributes_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (dtgrdAttributes.CurrentCell == null) return;
            int index = dtgrdAttributes.CurrentCell.RowIndex;
            string value = attributes.Rows[index][1].ToString();

            string name = attributes.Rows[index][0].ToString();
            if (AttributeArray[index]._name != name)
            {
                AttributeArray[index]._name = name;
                if (DictionaryChanged != null) DictionaryChanged.Invoke(this, EventArgs.Empty);
                return;
            }

            byte* buffer = (byte*)TargetNode.AttributeAddress;

            if (AttributeArray[index]._type == 2) //degrees
            {
                float val;
                if (!float.TryParse(value, out val))
                    value = ((float)(((bfloat*)buffer)[index])).ToString();
                else
                {
                    if (((bfloat*)buffer)[index] != val * Maths._deg2radf)
                    {
                        ((bfloat*)buffer)[index] = val * Maths._deg2radf;
                        TargetNode.SignalPropertyChange();
                    }
                }
            }
            else if (AttributeArray[index]._type == 1) //int
            {
                int val;
                if (!int.TryParse(value, out val))
                    value = ((int)(((bint*)buffer)[index])).ToString();
                else
                {
                    if (((bint*)buffer)[index] != val)
                    {
                        ((bint*)buffer)[index] = val;
                        TargetNode.SignalPropertyChange();
                    }
                }
            }
            else //float/radians
            {
                float val;
                if (!float.TryParse(value, out val))
                    value = ((float)(((bfloat*)buffer)[index])).ToString();
                else
                {
                    if (((bfloat*)buffer)[index] != val)
                    {
                        ((bfloat*)buffer)[index] = val;
                        TargetNode.SignalPropertyChange();
                    }
                }
            }

            attributes.Rows[index][1] = value;
            if (CellEdited != null) CellEdited.Invoke(this, EventArgs.Empty);
        }

        private void dtgrdAttributes_CurrentCellChanged(object sender, EventArgs e)
        {
            if (dtgrdAttributes.CurrentCell == null) return;
            int index = dtgrdAttributes.CurrentCell.RowIndex;

            //Display description of the selected attribute.
            _updating = true;
            description.Text = AttributeArray[index]._description;
            _updating = false;

            (AttributeArray[index]._type == 1 ? rdoInt
                : AttributeArray[index]._type == 2 ? rdoDegrees
                : rdoFloat).Checked = true;
        }

        private bool _updating = false;
        private void description_TextChanged(object sender, EventArgs e)
        {
            if (_updating)
                return;

            int index = dtgrdAttributes.CurrentCell.RowIndex;
            if (index >= 0)
            {
                AttributeArray[index]._description = description.Text;
                if (DictionaryChanged != null) DictionaryChanged.Invoke(this, EventArgs.Empty);
            }
        }

        private void radioButtonsChanged(object sender, EventArgs e)
        {
            if (dtgrdAttributes.CurrentCell == null)
                return;
            int index = dtgrdAttributes.CurrentCell.RowIndex;
            int ntype =
                rdoFloat.Checked ? 0
                : rdoInt.Checked ? 1
                : rdoDegrees.Checked ? 2
                : -1;
			if (ntype != AttributeArray[index]._type) {
				AttributeArray[index]._type = ntype;
				if (DictionaryChanged != null) DictionaryChanged.Invoke(this, EventArgs.Empty);
				RefreshRow(index);
			}
        }
    }

    public enum ValType
    {
        Float = 0,
        Int = 1,
        Degrees = 2
    }
}
