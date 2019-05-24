using BrawlLib.SSBB.ResourceNodes;
using System.ComponentModel;
using System.Data;

namespace System.Windows.Forms
{
    public unsafe class AttributeGrid : UserControl
    {
        #region Designer

        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            dtgrdAttributes = new System.Windows.Forms.DataGridView();
            description = new System.Windows.Forms.RichTextBox();
            splitter1 = new System.Windows.Forms.Splitter();
            panel1 = new System.Windows.Forms.Panel();
            tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            rdoDegrees = new System.Windows.Forms.RadioButton();
            rdoInt = new System.Windows.Forms.RadioButton();
            rdoFloat = new System.Windows.Forms.RadioButton();
            ((System.ComponentModel.ISupportInitialize)(dtgrdAttributes)).BeginInit();
            panel1.SuspendLayout();
            tableLayoutPanel1.SuspendLayout();
            SuspendLayout();
            // 
            // dtgrdAttributes
            // 
            dtgrdAttributes.AllowUserToAddRows = false;
            dtgrdAttributes.AllowUserToDeleteRows = false;
            dtgrdAttributes.AllowUserToResizeRows = false;
            dtgrdAttributes.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            dtgrdAttributes.BackgroundColor = System.Drawing.SystemColors.ControlLightLight;
            dtgrdAttributes.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            dtgrdAttributes.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dtgrdAttributes.ColumnHeadersVisible = false;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.Format = "N4";
            dataGridViewCellStyle1.NullValue = null;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            dtgrdAttributes.DefaultCellStyle = dataGridViewCellStyle1;
            dtgrdAttributes.Dock = System.Windows.Forms.DockStyle.Fill;
            dtgrdAttributes.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnKeystroke;
            dtgrdAttributes.EnableHeadersVisualStyles = false;
            dtgrdAttributes.GridColor = System.Drawing.SystemColors.ControlLight;
            dtgrdAttributes.Location = new System.Drawing.Point(0, 0);
            dtgrdAttributes.MultiSelect = false;
            dtgrdAttributes.Name = "dtgrdAttributes";
            dtgrdAttributes.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dtgrdAttributes.RowHeadersWidth = 8;
            dtgrdAttributes.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            dtgrdAttributes.RowTemplate.Height = 16;
            dtgrdAttributes.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            dtgrdAttributes.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            dtgrdAttributes.Size = new System.Drawing.Size(479, 239);
            dtgrdAttributes.TabIndex = 5;
            dtgrdAttributes.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(dtgrdAttributes_CellEndEdit);
            dtgrdAttributes.CurrentCellChanged += new System.EventHandler(dtgrdAttributes_CurrentCellChanged);
            // 
            // description
            // 
            description.BackColor = System.Drawing.SystemColors.Control;
            description.BorderStyle = System.Windows.Forms.BorderStyle.None;
            description.Cursor = System.Windows.Forms.Cursors.Default;
            description.Dock = System.Windows.Forms.DockStyle.Fill;
            description.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            description.ForeColor = System.Drawing.Color.Black;
            description.Location = new System.Drawing.Point(0, 0);
            description.Name = "description";
            description.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            description.Size = new System.Drawing.Size(479, 63);
            description.TabIndex = 6;
            description.Text = "No Description Available.";
            description.TextChanged += new System.EventHandler(description_TextChanged);
            // 
            // splitter1
            // 
            splitter1.Dock = System.Windows.Forms.DockStyle.Bottom;
            splitter1.Location = new System.Drawing.Point(0, 239);
            splitter1.Name = "splitter1";
            splitter1.Size = new System.Drawing.Size(479, 3);
            splitter1.TabIndex = 7;
            splitter1.TabStop = false;
            // 
            // panel1
            // 
            panel1.Controls.Add(tableLayoutPanel1);
            panel1.Controls.Add(description);
            panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            panel1.Location = new System.Drawing.Point(0, 242);
            panel1.Name = "panel1";
            panel1.Size = new System.Drawing.Size(479, 63);
            panel1.TabIndex = 8;
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 1;
            tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            tableLayoutPanel1.Controls.Add(rdoDegrees, 0, 2);
            tableLayoutPanel1.Controls.Add(rdoInt, 0, 1);
            tableLayoutPanel1.Controls.Add(rdoFloat, 0, 0);
            tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Right;
            tableLayoutPanel1.Location = new System.Drawing.Point(458, 0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 3;
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            tableLayoutPanel1.Size = new System.Drawing.Size(21, 63);
            tableLayoutPanel1.TabIndex = 9;
            // 
            // rdoDegrees
            // 
            rdoDegrees.Appearance = System.Windows.Forms.Appearance.Button;
            rdoDegrees.AutoSize = true;
            rdoDegrees.Dock = System.Windows.Forms.DockStyle.Fill;
            rdoDegrees.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            rdoDegrees.Location = new System.Drawing.Point(0, 42);
            rdoDegrees.Margin = new System.Windows.Forms.Padding(0);
            rdoDegrees.Name = "rdoDegrees";
            rdoDegrees.Size = new System.Drawing.Size(21, 21);
            rdoDegrees.TabIndex = 2;
            rdoDegrees.TabStop = true;
            rdoDegrees.Text = "D";
            rdoDegrees.UseVisualStyleBackColor = true;
            rdoDegrees.CheckedChanged += new System.EventHandler(radioButtonsChanged);
            // 
            // rdoInt
            // 
            rdoInt.Appearance = System.Windows.Forms.Appearance.Button;
            rdoInt.AutoSize = true;
            rdoInt.Dock = System.Windows.Forms.DockStyle.Fill;
            rdoInt.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            rdoInt.Location = new System.Drawing.Point(0, 21);
            rdoInt.Margin = new System.Windows.Forms.Padding(0);
            rdoInt.Name = "rdoInt";
            rdoInt.Size = new System.Drawing.Size(21, 21);
            rdoInt.TabIndex = 1;
            rdoInt.TabStop = true;
            rdoInt.Text = "I";
            rdoInt.UseVisualStyleBackColor = true;
            rdoInt.CheckedChanged += new System.EventHandler(radioButtonsChanged);
            // 
            // rdoFloat
            // 
            rdoFloat.Appearance = System.Windows.Forms.Appearance.Button;
            rdoFloat.AutoSize = true;
            rdoFloat.Dock = System.Windows.Forms.DockStyle.Fill;
            rdoFloat.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            rdoFloat.Location = new System.Drawing.Point(0, 0);
            rdoFloat.Margin = new System.Windows.Forms.Padding(0);
            rdoFloat.Name = "rdoFloat";
            rdoFloat.Size = new System.Drawing.Size(21, 21);
            rdoFloat.TabIndex = 0;
            rdoFloat.TabStop = true;
            rdoFloat.Text = "F";
            rdoFloat.UseVisualStyleBackColor = true;
            rdoFloat.CheckedChanged += new System.EventHandler(radioButtonsChanged);
            // 
            // AttributeGrid
            // 
            Controls.Add(dtgrdAttributes);
            Controls.Add(splitter1);
            Controls.Add(panel1);
            Name = "AttributeGrid";
            Size = new System.Drawing.Size(479, 305);
            ((System.ComponentModel.ISupportInitialize)(dtgrdAttributes)).EndInit();
            panel1.ResumeLayout(false);
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            ResumeLayout(false);

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
            get => _targetNode;
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

        private readonly DataTable attributes = new DataTable();
        public unsafe void TargetChanged()
        {
            if (TargetNode == null)
            {
                return;
            }

            attributes.Rows.Clear();
            for (int i = 0; i < TargetNode.NumEntries; i++)
            {
                if (i < AttributeArray.Length)
                {
                    attributes.Rows.Add(AttributeArray[i]._name);
                }
                else
                {
                    attributes.Rows.Add("0x" + (i * 4).ToString("X"));
                }
            }

            //Add attributes to the attribute table.
            for (int i = 0; i < TargetNode.NumEntries; i++)
            {
                RefreshRow(i);
            }
        }

        private void RefreshRow(int i)
        {
            if (AttributeArray.Length <= i || AttributeArray[i]._type == 2)
            {
                attributes.Rows[i][1] = ((bfloat*)TargetNode.AttributeAddress)[i] * Maths._rad2degf;
            }
            else if (AttributeArray[i]._type == 1)
            {
                attributes.Rows[i][1] = (int)((bint*)TargetNode.AttributeAddress)[i];
            }
            else
            {
                attributes.Rows[i][1] = (float)((bfloat*)TargetNode.AttributeAddress)[i];
            }
        }

        public void SetFloat(int index, float value) { TargetNode.SetFloat(index, value); }
        public float GetFloat(int index) { return TargetNode.GetFloat(index); }
        public void SetInt(int index, int value) { TargetNode.SetInt(index, value); }
        public int GetInt(int index) { return TargetNode.GetInt(index); }

        private unsafe void dtgrdAttributes_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (dtgrdAttributes.CurrentCell == null)
            {
                return;
            }

            int index = dtgrdAttributes.CurrentCell.RowIndex;
            string value = attributes.Rows[index][1].ToString();

            string name = attributes.Rows[index][0].ToString();
            if (AttributeArray[index]._name != name)
            {
                AttributeArray[index]._name = name;
                if (DictionaryChanged != null)
                {
                    DictionaryChanged.Invoke(this, EventArgs.Empty);
                }

                return;
            }

            byte* buffer = (byte*)TargetNode.AttributeAddress;

            if (AttributeArray[index]._type == 2) //degrees
            {
                if (!float.TryParse(value, out float val))
                {
                    value = ((float)(((bfloat*)buffer)[index])).ToString();
                }
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
                if (!int.TryParse(value, out int val))
                {
                    value = ((int)(((bint*)buffer)[index])).ToString();
                }
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
                if (!float.TryParse(value, out float val))
                {
                    value = ((float)(((bfloat*)buffer)[index])).ToString();
                }
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
            if (CellEdited != null)
            {
                CellEdited.Invoke(this, EventArgs.Empty);
            }
        }

        private void dtgrdAttributes_CurrentCellChanged(object sender, EventArgs e)
        {
            if (dtgrdAttributes.CurrentCell == null)
            {
                return;
            }

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
            {
                return;
            }

            int index = dtgrdAttributes.CurrentCell.RowIndex;
            if (index >= 0)
            {
                AttributeArray[index]._description = description.Text;
                if (DictionaryChanged != null)
                {
                    DictionaryChanged.Invoke(this, EventArgs.Empty);
                }
            }
        }

        private void radioButtonsChanged(object sender, EventArgs e)
        {
            if (dtgrdAttributes.CurrentCell == null)
            {
                return;
            }

            int index = dtgrdAttributes.CurrentCell.RowIndex;
            int ntype =
                rdoFloat.Checked ? 0
                : rdoInt.Checked ? 1
                : rdoDegrees.Checked ? 2
                : -1;
            if (ntype != AttributeArray[index]._type)
            {
                AttributeArray[index]._type = ntype;
                if (DictionaryChanged != null)
                {
                    DictionaryChanged.Invoke(this, EventArgs.Empty);
                }

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
