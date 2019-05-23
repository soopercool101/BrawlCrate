using System.ComponentModel;
using System.Data;
using System.Globalization;
using Ikarus;
using Ikarus.MovesetFile;

namespace System.Windows.Forms
{
    public class ArticleAttributeGrid : UserControl
    {
        #region Designer

        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dtgrdAttributes = new System.Windows.Forms.DataGridView();
            this.description = new System.Windows.Forms.RichTextBox();
            this.splitter1 = new System.Windows.Forms.Splitter();
            ((System.ComponentModel.ISupportInitialize)(this.dtgrdAttributes)).BeginInit();
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
            this.description.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.description.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.description.ForeColor = System.Drawing.Color.Black;
            this.description.Location = new System.Drawing.Point(0, 242);
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
            // ArticleAttributeGrid
            // 
            this.Controls.Add(this.dtgrdAttributes);
            this.Controls.Add(this.splitter1);
            this.Controls.Add(this.description);
            this.Name = "ArticleAttributeGrid";
            this.Size = new System.Drawing.Size(479, 305);
            ((System.ComponentModel.ISupportInitialize)(this.dtgrdAttributes)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        public AttributeInfo[] AttributeArray;

        public ArticleAttributeGrid()
        {
            InitializeComponent();
        }

        private DataGridView dtgrdAttributes;
        public RichTextBox description;
        private Splitter splitter1;
        public bool called = false;

        private RawParamList _targetNode;
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public RawParamList TargetNode
        {
            get { return _targetNode; }
            set { _targetNode = value; TargetChanged(); }
        }

        DataTable attributes = new DataTable();
        public unsafe void TargetChanged()
        {
            if (TargetNode == null)
                return;

            AttributeArray = _targetNode._info.ToArray();

            attributes.Columns.Clear();
            attributes.Rows.Clear();

            //Setup Attribute Table.
            attributes.Columns.Add("Name");
            attributes.Columns.Add("Value");
            //attributes.Columns[0].ReadOnly = true;
            dtgrdAttributes.DataSource = attributes;

            for (int i = 0; i < AttributeArray.Length; i++)
                attributes.Rows.Add(AttributeArray[i]._name);

            byte* buffer = (byte*)TargetNode.AttributeBuffer.Address;

            //Add attributes to the attribute table.
            for (int i = 0; i < AttributeArray.Length; i++)
                if (AttributeArray[i]._type == 2)
                    attributes.Rows[i][1] = (float)((bfloat*)buffer)[i] * Maths._rad2degf;
                else if (AttributeArray[i]._type == 1 || AttributeArray[i]._type > 2)
                    attributes.Rows[i][1] = (int)((bint*)buffer)[i];
                else
                    attributes.Rows[i][1] = (float)((bfloat*)buffer)[i];
        }

        private unsafe void dtgrdAttributes_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (dtgrdAttributes.CurrentCell == null) return;
            int index = dtgrdAttributes.CurrentCell.RowIndex;
            string value = attributes.Rows[index][1].ToString();

            string name = attributes.Rows[index][0].ToString();
            if (AttributeArray[index]._name != name && Manager.Params.ContainsKey(TargetNode._nameID))
            {
                AttributeArray[index]._name = _targetNode._info[index]._name = Manager.Params[TargetNode._nameID]._attributes[index]._name = name;
                Manager._dictionaryChanged = true;
                return;
            }

            byte* buffer = (byte*)TargetNode.AttributeBuffer.Address;

            if (AttributeArray[index]._type == 2) //degrees
            {
                float val;
                if (!float.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out val))
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
            else if (AttributeArray[index]._type == 1 || AttributeArray[index]._type > 2) //int
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
                if (!float.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out val))
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
        }

        private void dtgrdAttributes_CurrentCellChanged(object sender, EventArgs e)
        {
            if (dtgrdAttributes.CurrentCell == null) return;
            int index = dtgrdAttributes.CurrentCell.RowIndex;

            //Display description of the selected attribute.
            _updating = true;
            description.Text = AttributeArray[index]._description;
            _updating = false;
        }

        private bool _updating = false;
        private void description_TextChanged(object sender, EventArgs e)
        {
            if (_updating)
                return;

            int index = dtgrdAttributes.CurrentCell.RowIndex;
            if (index >= 0)
            {
                AttributeArray[index]._description = _targetNode._info[index]._description = description.Text;
                if (Manager.Params.ContainsKey(TargetNode._nameID))
                {
                    Manager.Params[TargetNode._nameID]._attributes[index]._description = AttributeArray[index]._description;
                    Manager._dictionaryChanged = true;
                }
            }
        }
    }
}
