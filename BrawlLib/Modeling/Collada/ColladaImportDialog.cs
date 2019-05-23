using System;
using System.Windows.Forms;
using System.Reflection;

namespace BrawlLib.Modeling
{
    public unsafe partial class Collada : Form
    {
        public Collada() { InitializeComponent(); }
        public Collada(Form owner, string title)
            : this()
        {
            Owner = owner;
            Text = title;
        }
        private Button button1;
        private Button button2;
        private Panel panel1;

        private Label Status;
        private PropertyGrid propertyGrid1;
        private Panel panel2;
        public string _filePath;

        public void Say(string text)
        {
            Status.Text = text;
            Update();
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);

            var info = propertyGrid1.GetType().GetProperty("Controls");
            var collection = (Control.ControlCollection)info.GetValue(propertyGrid1, null);

            foreach (var control in collection)
            {
                var type = control.GetType();
                if ("DocComment" == type.Name)
                {
                    const BindingFlags Flags = BindingFlags.Instance | BindingFlags.NonPublic;
                    var field = type.BaseType.GetField("userSized", Flags);
                    field.SetValue(control, true);

                    info = type.GetProperty("Lines");
                    info.SetValue(control, 5, null);

                    propertyGrid1.HelpVisible = true;
                    break;
                }
            }
        }

        public IModel ShowDialog(string filePath, ImportType type)
        {
            _importOptions = BrawlLib.Properties.Settings.Default.ColladaImportOptions;
            propertyGrid1.SelectedObject = _importOptions;

            if (base.ShowDialog() == DialogResult.OK)
            {
                panel1.Visible = false;
                Height = 70;
                UseWaitCursor = true;
                Text = "Please wait...";
                Show();
                Update();
                IModel model = ImportModel(filePath, type);
                BrawlLib.Properties.Settings.Default.Save();
                Close();
                _importOptions = new ImportOptions();
                return model;
            }
            _importOptions = new ImportOptions();
            return null;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void InitializeComponent()
        {
            this.Status = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.propertyGrid1 = new System.Windows.Forms.PropertyGrid();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // Status
            // 
            this.Status.AutoSize = true;
            this.Status.Location = new System.Drawing.Point(12, 9);
            this.Status.Name = "Status";
            this.Status.Size = new System.Drawing.Size(107, 13);
            this.Status.TabIndex = 0;
            this.Status.Text = "Parsing DAE model...";
            this.Status.UseWaitCursor = true;
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Location = new System.Drawing.Point(231, 6);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(65, 23);
            this.button1.TabIndex = 9;
            this.button1.Text = "Okay";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button2.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button2.Location = new System.Drawing.Point(302, 6);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(65, 23);
            this.button2.TabIndex = 10;
            this.button2.Text = "Cancel";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.propertyGrid1);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(379, 464);
            this.panel1.TabIndex = 11;
            // 
            // propertyGrid1
            // 
            this.propertyGrid1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.propertyGrid1.Location = new System.Drawing.Point(0, 0);
            this.propertyGrid1.Name = "propertyGrid1";
            this.propertyGrid1.Size = new System.Drawing.Size(379, 429);
            this.propertyGrid1.TabIndex = 11;
            this.propertyGrid1.ToolbarVisible = false;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.button1);
            this.panel2.Controls.Add(this.button2);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 429);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(379, 35);
            this.panel2.TabIndex = 12;
            // 
            // Collada
            // 
            this.AcceptButton = this.button1;
            this.CancelButton = this.button2;
            this.ClientSize = new System.Drawing.Size(379, 464);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.Status);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.MaximizeBox = false;
            this.Name = "Collada";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Import Settings";
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }
    }
}
