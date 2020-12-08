using BrawlLib.Imaging;
using BrawlLib.Internal.Windows.Forms;
using BrawlLib.SSBB.ResourceNodes;
using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace BrawlLib.Internal.Windows.Controls
{
    public class AttributeGrid : UserControl
    {
        #region Designer

        private void InitializeComponent()
        {
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            dtgrdAttributes = new DataGridView();
            description = new RichTextBox();
            splitter1 = new Splitter();
            panel1 = new Panel();
            btnInf = new Button();
            btnMinusInf = new Button();
            lblColor = new Label();
            lblCNoA = new Label();
            tableLayoutPanel1 = new TableLayoutPanel();
            rdoFloat = new RadioButton();
            rdoInt = new RadioButton();
            rdoColor = new RadioButton();
            rdoFlags = new RadioButton();
            rdoDegrees = new RadioButton();
            rdoUnknown = new RadioButton();
            rdoBytes = new RadioButton();
            rdoShorts = new RadioButton();
            ((ISupportInitialize) dtgrdAttributes).BeginInit();
            panel1.SuspendLayout();
            tableLayoutPanel1.SuspendLayout();
            SuspendLayout();
            // 
            // dtgrdAttributes
            // 
            dtgrdAttributes.AllowUserToAddRows = false;
            dtgrdAttributes.AllowUserToDeleteRows = false;
            dtgrdAttributes.AllowUserToResizeRows = false;
            dtgrdAttributes.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dtgrdAttributes.BackgroundColor = SystemColors.ControlLightLight;
            dtgrdAttributes.BorderStyle = BorderStyle.Fixed3D;
            dtgrdAttributes.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dtgrdAttributes.ColumnHeadersVisible = false;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = SystemColors.Window;
            dataGridViewCellStyle1.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Regular,
                GraphicsUnit.Point, 0);
            dataGridViewCellStyle1.ForeColor = SystemColors.ControlText;
            dataGridViewCellStyle1.Format = "N4";
            dataGridViewCellStyle1.NullValue = null;
            dataGridViewCellStyle1.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.False;
            dtgrdAttributes.DefaultCellStyle = dataGridViewCellStyle1;
            dtgrdAttributes.Dock = DockStyle.Fill;
            dtgrdAttributes.EditMode = DataGridViewEditMode.EditOnKeystroke;
            dtgrdAttributes.EnableHeadersVisualStyles = false;
            dtgrdAttributes.GridColor = SystemColors.ControlLight;
            dtgrdAttributes.Location = new Point(0, 0);
            dtgrdAttributes.MultiSelect = false;
            dtgrdAttributes.Name = "dtgrdAttributes";
            dtgrdAttributes.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dtgrdAttributes.RowHeadersWidth = 8;
            dtgrdAttributes.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            dtgrdAttributes.RowTemplate.Height = 16;
            dtgrdAttributes.ScrollBars = ScrollBars.Vertical;
            dtgrdAttributes.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dtgrdAttributes.Size = new Size(479, 200);
            dtgrdAttributes.TabIndex = 5;
            dtgrdAttributes.CellEndEdit += new DataGridViewCellEventHandler(dtgrdAttributes_CellEndEdit);
            dtgrdAttributes.CurrentCellChanged += new EventHandler(dtgrdAttributes_CurrentCellChanged);
            // 
            // description
            // 
            description.Anchor = AnchorStyles.Top | AnchorStyles.Bottom
                                                  | AnchorStyles.Left
                                                  | AnchorStyles.Right;
            description.BackColor = SystemColors.Control;
            description.BorderStyle = BorderStyle.None;
            description.Cursor = Cursors.Default;
            description.Font = new Font("Microsoft Sans Serif", 11.25F, FontStyle.Regular, GraphicsUnit.Point,
                0);
            description.ForeColor = Color.Black;
            description.Location = new Point(0, 0);
            description.Name = "description";
            description.ScrollBars = RichTextBoxScrollBars.Vertical;
            description.Size = new Size(479, 74);
            description.TabIndex = 6;
            description.Text = "No Description Available.";
            description.LinkClicked += new LinkClickedEventHandler(description_LinkClicked);
            description.TextChanged += new EventHandler(description_TextChanged);
            // 
            // splitter1
            // 
            splitter1.Dock = DockStyle.Bottom;
            splitter1.Location = new Point(0, 200);
            splitter1.Name = "splitter1";
            splitter1.Size = new Size(479, 3);
            splitter1.TabIndex = 7;
            splitter1.TabStop = false;
            // 
            // panel1
            // 
            panel1.Controls.Add(btnInf);
            panel1.Controls.Add(btnMinusInf);
            panel1.Controls.Add(lblColor);
            panel1.Controls.Add(lblCNoA);
            panel1.Controls.Add(tableLayoutPanel1);
            panel1.Controls.Add(description);
            panel1.Dock = DockStyle.Bottom;
            panel1.Location = new Point(0, 203);
            panel1.Name = "panel1";
            panel1.Size = new Size(479, 102);
            panel1.TabIndex = 8;
            // 
            // btnInf
            // 
            btnInf.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnInf.Font = new Font("Microsoft Sans Serif", 11F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnInf.Location = new Point(446, 44);
            btnInf.Name = "btnInf";
            btnInf.Size = new Size(30, 30);
            btnInf.TabIndex = 13;
            btnInf.Text = "∞";
            btnInf.UseVisualStyleBackColor = true;
            btnInf.Visible = false;
            btnInf.Click += new EventHandler(btnInf_Click);
            // 
            // btnMinusInf
            // 
            btnMinusInf.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnMinusInf.Font = new Font("Microsoft Sans Serif", 11F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnMinusInf.Location = new Point(412, 44);
            btnMinusInf.Name = "btnMinusInf";
            btnMinusInf.Size = new Size(30, 30);
            btnMinusInf.TabIndex = 12;
            btnMinusInf.Text = "-∞";
            btnMinusInf.UseVisualStyleBackColor = true;
            btnMinusInf.Visible = false;
            btnMinusInf.Click += new EventHandler(btnMinusInf_Click);
            // 
            // lblColor
            // 
            lblColor.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            lblColor.BorderStyle = BorderStyle.FixedSingle;
            lblColor.Location = new Point(394, 60);
            lblColor.Name = "lblColor";
            lblColor.Size = new Size(41, 14);
            lblColor.TabIndex = 10;
            lblColor.Visible = false;
            lblColor.Click += new EventHandler(lblColor_Click);
            // 
            // lblCNoA
            // 
            lblCNoA.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            lblCNoA.BorderStyle = BorderStyle.FixedSingle;
            lblCNoA.Location = new Point(434, 60);
            lblCNoA.Name = "lblCNoA";
            lblCNoA.Size = new Size(41, 14);
            lblCNoA.TabIndex = 11;
            lblCNoA.Visible = false;
            lblCNoA.Click += new EventHandler(lblColor_Click);
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 8;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.Controls.Add(rdoFloat, 0, 0);
            tableLayoutPanel1.Controls.Add(rdoInt, 1, 0);
            tableLayoutPanel1.Controls.Add(rdoColor, 2, 0);
            tableLayoutPanel1.Controls.Add(rdoBytes, 3, 0);
            tableLayoutPanel1.Controls.Add(rdoShorts, 4, 0);
            tableLayoutPanel1.Controls.Add(rdoFlags, 5, 0);
            tableLayoutPanel1.Controls.Add(rdoDegrees, 6, 0);
            tableLayoutPanel1.Controls.Add(rdoUnknown, 7, 0);
            tableLayoutPanel1.Dock = DockStyle.Bottom;
            tableLayoutPanel1.Location = new Point(0, 77);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 1;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel1.Size = new Size(479, 25);
            tableLayoutPanel1.TabIndex = 9;
            // 
            // rdoFloat
            // 
            rdoFloat.Appearance = Appearance.Button;
            rdoFloat.AutoSize = true;
            rdoFloat.Dock = DockStyle.Fill;
            rdoFloat.Font = new Font("Microsoft Sans Serif", 8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            rdoFloat.Location = new Point(0, 0);
            rdoFloat.Margin = new Padding(0);
            rdoFloat.Name = "rdoFloat";
            rdoFloat.Size = new Size(79, 25);
            rdoFloat.TabIndex = 0;
            rdoFloat.TabStop = true;
            rdoFloat.Text = "Float";
            rdoFloat.UseVisualStyleBackColor = true;
            rdoFloat.CheckedChanged += new EventHandler(radioButtonsChanged);
            // 
            // rdoInt
            // 
            rdoInt.Appearance = Appearance.Button;
            rdoInt.AutoSize = true;
            rdoInt.Dock = DockStyle.Fill;
            rdoInt.Font = new Font("Microsoft Sans Serif", 8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            rdoInt.Location = new Point(79, 0);
            rdoInt.Margin = new Padding(0);
            rdoInt.Name = "rdoInt";
            rdoInt.Size = new Size(79, 25);
            rdoInt.TabIndex = 1;
            rdoInt.TabStop = true;
            rdoInt.Text = "Integer";
            rdoInt.UseVisualStyleBackColor = true;
            rdoInt.CheckedChanged += new EventHandler(radioButtonsChanged);
            // 
            // rdoColor
            // 
            rdoColor.Appearance = Appearance.Button;
            rdoColor.AutoSize = true;
            rdoColor.Dock = DockStyle.Fill;
            rdoColor.Font = new Font("Microsoft Sans Serif", 8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            rdoColor.Location = new Point(158, 0);
            rdoColor.Margin = new Padding(0);
            rdoColor.Name = "rdoColor";
            rdoColor.Size = new Size(79, 25);
            rdoColor.TabIndex = 2;
            rdoColor.TabStop = true;
            rdoColor.Text = "Color";
            rdoColor.UseVisualStyleBackColor = true;
            rdoColor.CheckedChanged += new EventHandler(radioButtonsChanged);
            // 
            // rdoBytes
            // 
            rdoBytes.Appearance = Appearance.Button;
            rdoBytes.AutoSize = true;
            rdoBytes.Dock = DockStyle.Fill;
            rdoBytes.Font = new Font("Microsoft Sans Serif", 8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            rdoBytes.Location = new Point(395, 0);
            rdoBytes.Margin = new Padding(0);
            rdoBytes.Name = "rdoBytes";
            rdoBytes.Size = new Size(84, 25);
            rdoBytes.TabIndex = 3;
            rdoBytes.TabStop = true;
            rdoBytes.Text = "Bytes";
            rdoBytes.UseVisualStyleBackColor = true;
            rdoBytes.CheckedChanged += new EventHandler(radioButtonsChanged);
            // 
            // rdoShorts
            // 
            rdoShorts.Appearance = Appearance.Button;
            rdoShorts.AutoSize = true;
            rdoShorts.Dock = DockStyle.Fill;
            rdoShorts.Font = new Font("Microsoft Sans Serif", 8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            rdoShorts.Location = new Point(395, 0);
            rdoShorts.Margin = new Padding(0);
            rdoShorts.Name = "rdoShorts";
            rdoShorts.Size = new Size(84, 25);
            rdoShorts.TabIndex = 4;
            rdoShorts.TabStop = true;
            rdoShorts.Text = "Shorts";
            rdoShorts.UseVisualStyleBackColor = true;
            rdoShorts.CheckedChanged += new EventHandler(radioButtonsChanged);
            // 
            // rdoFlags
            // 
            rdoFlags.Appearance = Appearance.Button;
            rdoFlags.AutoSize = true;
            rdoFlags.Dock = DockStyle.Fill;
            rdoFlags.Font = new Font("Microsoft Sans Serif", 8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            rdoFlags.Location = new Point(237, 0);
            rdoFlags.Margin = new Padding(0);
            rdoFlags.Name = "rdoFlags";
            rdoFlags.Size = new Size(79, 25);
            rdoFlags.TabIndex = 5;
            rdoFlags.TabStop = true;
            rdoFlags.Text = "Flags";
            rdoFlags.UseVisualStyleBackColor = true;
            rdoFlags.CheckedChanged += new EventHandler(radioButtonsChanged);
            // 
            // rdoDegrees
            // 
            rdoDegrees.Appearance = Appearance.Button;
            rdoDegrees.AutoSize = true;
            rdoDegrees.Dock = DockStyle.Fill;
            rdoDegrees.Font = new Font("Microsoft Sans Serif", 8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            rdoDegrees.Location = new Point(316, 0);
            rdoDegrees.Margin = new Padding(0);
            rdoDegrees.Name = "rdoDegrees";
            rdoDegrees.Size = new Size(79, 25);
            rdoDegrees.TabIndex = 6;
            rdoDegrees.TabStop = true;
            rdoDegrees.Text = "Degrees";
            rdoDegrees.UseVisualStyleBackColor = true;
            rdoDegrees.CheckedChanged += new EventHandler(radioButtonsChanged);
            // 
            // rdoUnknown
            // 
            rdoUnknown.Appearance = Appearance.Button;
            rdoUnknown.AutoSize = true;
            rdoUnknown.Dock = DockStyle.Fill;
            rdoUnknown.Font = new Font("Microsoft Sans Serif", 8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            rdoUnknown.Location = new Point(395, 0);
            rdoUnknown.Margin = new Padding(0);
            rdoUnknown.Name = "rdoUnknown";
            rdoUnknown.Size = new Size(84, 25);
            rdoUnknown.TabIndex = 7;
            rdoUnknown.TabStop = true;
            rdoUnknown.Text = "Hex";
            rdoUnknown.UseVisualStyleBackColor = true;
            rdoUnknown.CheckedChanged += new EventHandler(radioButtonsChanged);
            // 
            // AttributeGrid
            // 
            Controls.Add(dtgrdAttributes);
            Controls.Add(splitter1);
            Controls.Add(panel1);
            Name = "AttributeGrid";
            Size = new Size(479, 305);
            ((ISupportInitialize) dtgrdAttributes).EndInit();
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
            _dlgColor = new GoodColorDialog();
        }

        private DataGridView dtgrdAttributes;
        public RichTextBox description;
        private Splitter splitter1;
        public bool called;
        private Panel panel1;
        private TableLayoutPanel tableLayoutPanel1;
        private RadioButton rdoFloat;
        private RadioButton rdoInt;
        private RadioButton rdoColor;
        private RadioButton rdoFlags;
        private RadioButton rdoDegrees;
        private RadioButton rdoUnknown;
        private RadioButton rdoBytes;
        private RadioButton rdoShorts;
        private Label lblColor;
        private Label lblCNoA;
        private Button btnInf;
        private Button btnMinusInf;

        public event EventHandler CellEdited;
        public event EventHandler DictionaryChanged;

        private IAttributeList _targetNode;

        protected bool somethingChanged;

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public IAttributeList TargetNode
        {
            get => _targetNode;
            set
            {
                _targetNode = value;
                if (!called && value != null)
                {
                    LoadData();
                    called = true;
                }

                TargetChanged();
            }
        }

        public void LoadData()
        {
            somethingChanged = false;
            attributes.Columns.Clear();
            attributes.Rows.Clear();

            //Setup Attribute Table.
            attributes.Columns.Add("Name");
            attributes.Columns.Add("Value");
            dtgrdAttributes.DataSource = attributes;
            dtgrdAttributes.CellToolTipTextNeeded += dtgrdAttributes_CellToolTipTextNeeded;
        }

        private void dtgrdAttributes_CellToolTipTextNeeded(object sender,
                                                           DataGridViewCellToolTipTextNeededEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                string text = AttributeArray[e.RowIndex]._description;
                if (e.ColumnIndex > 0 && text.Contains("Default"))
                {
                    e.ToolTipText = text.Substring(text.LastIndexOf("Default"));
                }
                else
                {
                    e.ToolTipText = text;
                }
            }
        }

        private readonly DataTable attributes = new DataTable();

        public void TargetChanged()
        {
            lblColor.Visible = false;
            lblCNoA.Visible = false;
            btnInf.Visible = false;
            btnMinusInf.Visible = false;
            rdoColor.Enabled = rdoDegrees.Enabled =
                rdoFlags.Enabled = rdoFloat.Enabled = rdoInt.Enabled = rdoUnknown.Enabled = true;

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

            if (AttributeArray.Length <= 0)
            {
                rdoColor.Enabled = rdoDegrees.Enabled =
                    rdoFlags.Enabled = rdoFloat.Enabled = rdoInt.Enabled = rdoUnknown.Enabled = false;
            }
        }

        private void RefreshRow(int i)
        {
            if (AttributeArray.Length <= i || AttributeArray[i]._type == 2)
            {
                attributes.Rows[i][1] = TargetNode.GetDegrees(i);
            }
            else if (AttributeArray[i]._type == 1)
            {
                attributes.Rows[i][1] = TargetNode.GetInt(i);
            }
            else if (AttributeArray[i]._type == 3)
            {
                attributes.Rows[i][1] = TargetNode.GetRGBAPixel(i);
                lblColor.BackColor = (Color) TargetNode.GetRGBAPixel(i);
                lblCNoA.BackColor = Color.FromArgb(TargetNode.GetRGBAPixel(i).R, TargetNode.GetRGBAPixel(i).G,
                    TargetNode.GetRGBAPixel(i).B);
            }
            else if (AttributeArray[i]._type == 4)
            {
                attributes.Rows[i][1] = TargetNode.GetHex(i);
            }
            else if (AttributeArray[i]._type == 5)
            {
                attributes.Rows[i][1] = Convert.ToString(TargetNode.GetInt(i), 2).PadLeft(32, '0');
            }
            else if (AttributeArray[i]._type == 6)
            {
                attributes.Rows[i][1] = TargetNode.GetBytes(i);
            }
            else if (AttributeArray[i]._type == 7)
            {
                attributes.Rows[i][1] = TargetNode.GetShorts(i);
            }
            else
            {
                attributes.Rows[i][1] = TargetNode.GetFloat(i);
            }
        }

        private void dtgrdAttributes_CellEndEdit(object sender, DataGridViewCellEventArgs e)
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
                somethingChanged = true;
                AttributeArray[index]._name = name;
                DictionaryChanged?.Invoke(this, EventArgs.Empty);

                return;
            }

            lblColor.Visible = false;
            lblCNoA.Visible = false;
            btnInf.Visible = false;
            btnMinusInf.Visible = false;
            if (AttributeArray[index]._type == 7) // Shorts
            {
                string[] values = value.Split();
                if (values.Length >= 2)
                {
                    TargetNode.SetShorts(index, short.Parse(values[0].Trim(' ', ',')),
                        short.Parse(values[1].Trim(' ', ',')));
                }
                else if (values.Length == 1)
                {
                    TargetNode.SetShorts(index, short.Parse(values[0].Trim(' ', ',')),
                        short.Parse(values[0].Trim(' ', ',')));
                }
            }
            else if (AttributeArray[index]._type == 6) // Bytes
            {
                string[] values = value.Split();
                if (values.Length == 4)
                {
                    TargetNode.SetBytes(index, byte.Parse(values[0].Trim(' ', ',')),
                        byte.Parse(values[1].Trim(' ', ',')), byte.Parse(values[2].Trim(' ', ',')),
                        byte.Parse(values[3].Trim(' ', ',')));
                }
                else if (values.Length == 1)
                {
                    TargetNode.SetBytes(index, byte.Parse(values[0].Trim(' ', ',')),
                        byte.Parse(values[0].Trim(' ', ',')), byte.Parse(values[0].Trim(' ', ',')),
                        byte.Parse(values[0].Trim(' ', ',')));
                }
            }
            else if (AttributeArray[index]._type == 5) // Binary
            {
                string field0 = value.Replace(" ", string.Empty);
                TargetNode.SetInt(index, Convert.ToInt32(field0, 2));
                TargetNode.SignalPropertyChange();
            }
            else if (AttributeArray[index]._type == 4) // Hex
            {
                string field0 = (value ?? "").Split(' ')[0];
                int fromBase = field0.StartsWith("0x", StringComparison.InvariantCultureIgnoreCase) ? 16 : 10;
                int temp = Convert.ToInt32(field0, fromBase);
                if (TargetNode.GetInt(index) != temp)
                {
                    TargetNode.SetInt(index, temp);
                }

                if (fromBase == 10)
                {
                    AttributeArray[index]._type = 1;
                }
            }
            else if (AttributeArray[index]._type == 3) // color
            {
                lblColor.Visible = true;
                lblCNoA.Visible = true;
                RGBAPixel p = new RGBAPixel();

                string s = value;
                char[] delims = {',', 'R', 'G', 'B', 'A', ':', ' '};
                string[] arr = s.Split(delims, StringSplitOptions.RemoveEmptyEntries);

                if (arr.Length == 4)
                {
                    if (byte.TryParse(arr[0], out p.R) &&
                        byte.TryParse(arr[1], out p.G) &&
                        byte.TryParse(arr[2], out p.B) &&
                        byte.TryParse(arr[3], out p.A))
                    {
                        TargetNode.SetRGBAPixel(index, p);
                        lblColor.BackColor = (Color) p;
                        lblCNoA.BackColor = Color.FromArgb(p.R, p.G, p.B);
                    }
                }
                else if (arr.Length == 3)
                {
                    if (byte.TryParse(arr[0], out p.R) &&
                        byte.TryParse(arr[1], out p.G) &&
                        byte.TryParse(arr[2], out p.B))
                    {
                        TargetNode.SetRGBAPixel(index, p);
                        lblColor.BackColor = (Color) p;
                        lblCNoA.BackColor = Color.FromArgb(p.R, p.G, p.B);
                    }
                }
                else if (arr.Length == 1)
                {
                    if (byte.TryParse(arr[0], out p.R) &&
                        byte.TryParse(arr[0], out p.G) &&
                        byte.TryParse(arr[0], out p.B) &&
                        byte.TryParse(arr[0], out p.A))
                    {
                        TargetNode.SetRGBAPixel(index, p);
                        lblColor.BackColor = (Color) p;
                        lblCNoA.BackColor = Color.FromArgb(p.R, p.G, p.B);
                    }
                }
            }
            else if (AttributeArray[index]._type == 2) //degrees
            {
                if (!float.TryParse(value, out float val))
                {
                    value = TargetNode.GetDegrees(index).ToString();
                }
                else
                {
                    TargetNode.SetDegrees(index, val);
                }
            }
            else if (AttributeArray[index]._type == 1) //int
            {
                if (!int.TryParse(value, out int val))
                {
                    value = TargetNode.GetInt(index).ToString();
                }
                else
                {
                    TargetNode.SetInt(index, val);
                }
            }
            else //float/radians
            {
                btnInf.Visible = btnMinusInf.Visible = true;
                if (!float.TryParse(value, out float val))
                {
                    value = TargetNode.GetFloat(index).ToString();
                }
                else
                {
                    TargetNode.SetFloat(index, val);
                }
            }

            attributes.Rows[index][1] = value;
            if (AttributeArray[index]._type == 3)
            {
                attributes.Rows[index][1] = TargetNode.GetRGBAPixel(index).ToString();
            }

            CellEdited?.Invoke(this, EventArgs.Empty);
            RefreshRow(index);
        }

        private void dtgrdAttributes_CurrentCellChanged(object sender, EventArgs e)
        {
            lblColor.Visible = false;
            lblCNoA.Visible = false;
            btnInf.Visible = false;
            btnMinusInf.Visible = false;
            if (dtgrdAttributes.CurrentCell == null)
            {
                return;
            }

            if (AttributeArray.Length <= 0)
            {
                description.Text = "";
                return;
            }

            int index = dtgrdAttributes.CurrentCell.RowIndex;

            //Display description of the selected attribute.
            _updating = true;
            description.Text = AttributeArray[index]._description;
            _updating = false;

            (AttributeArray[index]._type == 1 ? rdoInt
                : AttributeArray[index]._type == 3 ? rdoColor
                : AttributeArray[index]._type == 2 ? rdoDegrees
                : AttributeArray[index]._type == 4 ? rdoUnknown
                : AttributeArray[index]._type == 5 ? rdoFlags
                : AttributeArray[index]._type == 6 ? rdoBytes
                : AttributeArray[index]._type == 7 ? rdoShorts
                : rdoFloat).Checked = true;

            switch (AttributeArray[index]._type)
            {
                case 3:
                    lblColor.Visible = true;
                    lblCNoA.Visible = true;
                    lblColor.BackColor = (Color) TargetNode.GetRGBAPixel(index);
                    lblCNoA.BackColor = Color.FromArgb(TargetNode.GetRGBAPixel(index).R,
                        TargetNode.GetRGBAPixel(index).G,
                        TargetNode.GetRGBAPixel(index).B);
                    break;
                case 0:
                case 2:
                    btnInf.Visible = true;
                    btnMinusInf.Visible = true;
                    break;
            }
        }

        private bool _updating;

        private void description_TextChanged(object sender, EventArgs e)
        {
            if (_updating || AttributeArray.Length == 0)
            {
                return;
            }

            int index = dtgrdAttributes.CurrentCell.RowIndex;
            if (index >= 0 && AttributeArray.Length > index)
            {
                AttributeArray[index]._description = description.Text;
                DictionaryChanged?.Invoke(this, EventArgs.Empty);
                somethingChanged = true;
            }
        }

        private void description_LinkClicked(object sender, LinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("explorer.exe", e.LinkText);
        }

        private void radioButtonsChanged(object sender, EventArgs e)
        {
            lblColor.Visible = false;
            lblCNoA.Visible = false;
            btnInf.Visible = false;
            btnMinusInf.Visible = false;
            if (dtgrdAttributes.CurrentCell == null)
            {
                return;
            }

            int index = dtgrdAttributes.CurrentCell.RowIndex;
            int nType =
                rdoFloat.Checked ? 0
                : rdoInt.Checked ? 1
                : rdoColor.Checked ? 3
                : rdoDegrees.Checked ? 2
                : rdoUnknown.Checked ? 4
                : rdoFlags.Checked ? 5
                : rdoBytes.Checked ? 6
                : rdoShorts.Checked ? 7
                : -1;
            if (nType != AttributeArray[index]._type)
            {
                AttributeArray[index]._type = nType;
                DictionaryChanged?.Invoke(this, EventArgs.Empty);

                RefreshRow(index);
            }

            switch (nType)
            {
                case 3:
                    lblColor.Visible = true;
                    lblCNoA.Visible = true;
                    break;
                case 0:
                case 2:
                    btnInf.Visible = true;
                    btnMinusInf.Visible = true;
                    break;
            }
        }

        private readonly GoodColorDialog _dlgColor;

        private void lblColor_Click(object sender, EventArgs e)
        {
            if (!lblColor.Visible)
            {
                return;
            }

            int index = dtgrdAttributes.CurrentCell.RowIndex;
            _dlgColor.Color = (Color) TargetNode.GetRGBAPixel(index);
            if (_dlgColor.ShowDialog(this) == DialogResult.OK)
            {
                TargetNode.SetRGBAPixel(index, (ARGBPixel) _dlgColor.Color);
                TargetNode.SignalPropertyChange();
                RefreshRow(index);
            }
        }

        private void btnMinusInf_Click(object sender, EventArgs e)
        {
            if (!btnMinusInf.Visible)
            {
                return;
            }

            int index = dtgrdAttributes.CurrentCell.RowIndex;
            TargetNode.SetFloat(index, float.NegativeInfinity);
            RefreshRow(index);
        }

        private void btnInf_Click(object sender, EventArgs e)
        {
            if (!btnInf.Visible)
            {
                return;
            }

            int index = dtgrdAttributes.CurrentCell.RowIndex;
            TargetNode.SetFloat(index, float.PositiveInfinity);
            RefreshRow(index);
        }
    }
}