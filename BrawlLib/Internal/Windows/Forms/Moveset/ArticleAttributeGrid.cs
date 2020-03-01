using BrawlLib.SSBB.ResourceNodes;
using System;
using System.ComponentModel;
using System.Data;
using System.Globalization;
using System.Windows.Forms;

namespace BrawlLib.Internal.Windows.Forms.Moveset
{
    public class ArticleAttributeGrid : UserControl
    {
        #region Designer

        private void InitializeComponent()
        {
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            dtgrdAttributes = new DataGridView();
            description = new RichTextBox();
            splitter1 = new Splitter();
            ((ISupportInitialize) dtgrdAttributes).BeginInit();
            SuspendLayout();
            // 
            // dtgrdAttributes
            // 
            dtgrdAttributes.AllowUserToAddRows = false;
            dtgrdAttributes.AllowUserToDeleteRows = false;
            dtgrdAttributes.AllowUserToResizeRows = false;
            dtgrdAttributes.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dtgrdAttributes.BackgroundColor = System.Drawing.SystemColors.ControlLightLight;
            dtgrdAttributes.BorderStyle = BorderStyle.Fixed3D;
            dtgrdAttributes.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dtgrdAttributes.ColumnHeadersVisible = false;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F,
                System.Drawing.FontStyle.Regular,
                System.Drawing.GraphicsUnit.Point, (byte) 0);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.Format = "N4";
            dataGridViewCellStyle1.NullValue = null;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.False;
            dtgrdAttributes.DefaultCellStyle = dataGridViewCellStyle1;
            dtgrdAttributes.Dock = DockStyle.Fill;
            dtgrdAttributes.EditMode = DataGridViewEditMode.EditOnKeystroke;
            dtgrdAttributes.EnableHeadersVisualStyles = false;
            dtgrdAttributes.GridColor = System.Drawing.SystemColors.ControlLight;
            dtgrdAttributes.Location = new System.Drawing.Point(0, 0);
            dtgrdAttributes.MultiSelect = false;
            dtgrdAttributes.Name = "dtgrdAttributes";
            dtgrdAttributes.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dtgrdAttributes.RowHeadersWidth = 8;
            dtgrdAttributes.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            dtgrdAttributes.RowTemplate.Height = 16;
            dtgrdAttributes.ScrollBars = ScrollBars.Vertical;
            dtgrdAttributes.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dtgrdAttributes.Size = new System.Drawing.Size(479, 239);
            dtgrdAttributes.TabIndex = 5;
            dtgrdAttributes.CellEndEdit += new DataGridViewCellEventHandler(dtgrdAttributes_CellEndEdit);
            dtgrdAttributes.CurrentCellChanged += new EventHandler(dtgrdAttributes_CurrentCellChanged);
            // 
            // description
            // 
            description.BackColor = System.Drawing.SystemColors.Control;
            description.BorderStyle = BorderStyle.None;
            description.Cursor = Cursors.Default;
            description.Dock = DockStyle.Bottom;
            description.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular,
                System.Drawing.GraphicsUnit.Point, (byte) 0);
            description.ForeColor = System.Drawing.Color.Black;
            description.Location = new System.Drawing.Point(0, 242);
            description.Name = "description";
            description.ScrollBars = RichTextBoxScrollBars.Vertical;
            description.Size = new System.Drawing.Size(479, 63);
            description.TabIndex = 6;
            description.Text = "No Description Available.";
            description.TextChanged += new EventHandler(description_TextChanged);
            // 
            // splitter1
            // 
            splitter1.Dock = DockStyle.Bottom;
            splitter1.Location = new System.Drawing.Point(0, 239);
            splitter1.Name = "splitter1";
            splitter1.Size = new System.Drawing.Size(479, 3);
            splitter1.TabIndex = 7;
            splitter1.TabStop = false;
            // 
            // ArticleAttributeGrid
            // 
            Controls.Add(dtgrdAttributes);
            Controls.Add(splitter1);
            Controls.Add(description);
            Name = "ArticleAttributeGrid";
            Size = new System.Drawing.Size(479, 305);
            ((ISupportInitialize) dtgrdAttributes).EndInit();
            ResumeLayout(false);
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

        private MoveDefSectionParamNode _targetNode;

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public MoveDefSectionParamNode TargetNode
        {
            get => _targetNode;
            set
            {
                _targetNode = value;
                TargetChanged();
            }
        }

        private DataTable attributes = new DataTable();

        public unsafe void TargetChanged()
        {
            if (TargetNode == null)
            {
                return;
            }

            AttributeArray = _targetNode._info.ToArray();

            attributes.Columns.Clear();
            attributes.Rows.Clear();

            //Setup Attribute Table.
            attributes.Columns.Add("Name");
            attributes.Columns.Add("Value");
            //attributes.Columns[0].ReadOnly = true;
            dtgrdAttributes.DataSource = attributes;

            for (int i = 0; i < AttributeArray.Length; i++)
            {
                attributes.Rows.Add(AttributeArray[i]._name);
            }

            byte* buffer = (byte*) TargetNode.AttributeBuffer.Address;

            //Add attributes to the attribute table.
            for (int i = 0; i < AttributeArray.Length; i++)
            {
                if (AttributeArray[i]._type == 2)
                {
                    attributes.Rows[i][1] = (float) ((bfloat*) buffer)[i] * Maths._rad2degf;
                }
                else if (AttributeArray[i]._type == 1 || AttributeArray[i]._type > 2)
                {
                    attributes.Rows[i][1] = (int) ((bint*) buffer)[i];
                }
                else
                {
                    attributes.Rows[i][1] = (float) ((bfloat*) buffer)[i];
                }
            }
        }

        private unsafe void dtgrdAttributes_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (dtgrdAttributes.CurrentCell == null)
            {
                return;
            }

            int index = dtgrdAttributes.CurrentCell.RowIndex;
            string value = attributes.Rows[index][1].ToString();

            string name = attributes.Rows[index][0].ToString();
            if (AttributeArray[index]._name != name && TargetNode.Root.Params.ContainsKey(TargetNode.OldName))
            {
                AttributeArray[index]._name = _targetNode._info[index]._name =
                    TargetNode.Root.Params[TargetNode.OldName]._attributes[index]._name = name;
                MoveDefNode._dictionaryChanged = true;
                return;
            }

            byte* buffer = (byte*) TargetNode.AttributeBuffer.Address;

            if (AttributeArray[index]._type == 2) //degrees
            {
                float val;
                if (!float.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out val))
                {
                    value = ((float) ((bfloat*) buffer)[index]).ToString();
                }
                else
                {
                    if (((bfloat*) buffer)[index] != val * Maths._deg2radf)
                    {
                        ((bfloat*) buffer)[index] = val * Maths._deg2radf;
                        TargetNode.SignalPropertyChange();
                    }
                }
            }
            else if (AttributeArray[index]._type == 1 || AttributeArray[index]._type > 2) //int
            {
                int val;
                if (!int.TryParse(value, out val))
                {
                    value = ((int) ((bint*) buffer)[index]).ToString();
                }
                else
                {
                    if (((bint*) buffer)[index] != val)
                    {
                        ((bint*) buffer)[index] = val;
                        TargetNode.SignalPropertyChange();
                    }
                }
            }
            else //float/radians
            {
                float val;
                if (!float.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out val))
                {
                    value = ((float) ((bfloat*) buffer)[index]).ToString();
                }
                else
                {
                    if (((bfloat*) buffer)[index] != val)
                    {
                        ((bfloat*) buffer)[index] = val;
                        TargetNode.SignalPropertyChange();
                    }
                }
            }

            attributes.Rows[index][1] = value;
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
        }

        private bool _updating;

        private void description_TextChanged(object sender, EventArgs e)
        {
            if (_updating)
            {
                return;
            }

            int index = dtgrdAttributes.CurrentCell.RowIndex;
            if (index >= 0)
            {
                AttributeArray[index]._description = _targetNode._info[index]._description = description.Text;
                if (TargetNode.Root.Params.ContainsKey(TargetNode.OldName))
                {
                    TargetNode.Root.Params[TargetNode.OldName]._attributes[index]._description =
                        AttributeArray[index]._description;
                    MoveDefNode._dictionaryChanged = true;
                }
            }
        }
    }
}