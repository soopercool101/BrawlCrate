using System;
using System.Reflection;
using System.Windows.Forms;

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

            PropertyInfo info = propertyGrid1.GetType().GetProperty("Controls");
            Control.ControlCollection collection = (Control.ControlCollection)info.GetValue(propertyGrid1, null);

            foreach (object control in collection)
            {
                Type type = control.GetType();
                if ("DocComment" == type.Name)
                {
                    const BindingFlags Flags = BindingFlags.Instance | BindingFlags.NonPublic;
                    FieldInfo field = type.BaseType.GetField("userSized", Flags);
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
            Status = new System.Windows.Forms.Label();
            button1 = new System.Windows.Forms.Button();
            button2 = new System.Windows.Forms.Button();
            panel1 = new System.Windows.Forms.Panel();
            propertyGrid1 = new System.Windows.Forms.PropertyGrid();
            panel2 = new System.Windows.Forms.Panel();
            panel1.SuspendLayout();
            panel2.SuspendLayout();
            SuspendLayout();
            // 
            // Status
            // 
            Status.AutoSize = true;
            Status.Location = new System.Drawing.Point(12, 9);
            Status.Name = "Status";
            Status.Size = new System.Drawing.Size(107, 13);
            Status.TabIndex = 0;
            Status.Text = "Parsing DAE model...";
            Status.UseWaitCursor = true;
            // 
            // button1
            // 
            button1.Anchor = (System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right);
            button1.Location = new System.Drawing.Point(231, 6);
            button1.Name = "button1";
            button1.Size = new System.Drawing.Size(65, 23);
            button1.TabIndex = 9;
            button1.Text = "Okay";
            button1.UseVisualStyleBackColor = true;
            button1.Click += new System.EventHandler(button1_Click);
            // 
            // button2
            // 
            button2.Anchor = (System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right);
            button2.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            button2.Location = new System.Drawing.Point(302, 6);
            button2.Name = "button2";
            button2.Size = new System.Drawing.Size(65, 23);
            button2.TabIndex = 10;
            button2.Text = "Cancel";
            button2.UseVisualStyleBackColor = true;
            button2.Click += new System.EventHandler(button2_Click);
            // 
            // panel1
            // 
            panel1.Controls.Add(propertyGrid1);
            panel1.Controls.Add(panel2);
            panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            panel1.Location = new System.Drawing.Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new System.Drawing.Size(379, 464);
            panel1.TabIndex = 11;
            // 
            // propertyGrid1
            // 
            propertyGrid1.Dock = System.Windows.Forms.DockStyle.Fill;
            propertyGrid1.Location = new System.Drawing.Point(0, 0);
            propertyGrid1.Name = "propertyGrid1";
            propertyGrid1.Size = new System.Drawing.Size(379, 429);
            propertyGrid1.TabIndex = 11;
            propertyGrid1.ToolbarVisible = false;
            // 
            // panel2
            // 
            panel2.Controls.Add(button1);
            panel2.Controls.Add(button2);
            panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            panel2.Location = new System.Drawing.Point(0, 429);
            panel2.Name = "panel2";
            panel2.Size = new System.Drawing.Size(379, 35);
            panel2.TabIndex = 12;
            // 
            // Collada
            // 
            AcceptButton = button1;
            CancelButton = button2;
            ClientSize = new System.Drawing.Size(379, 464);
            Controls.Add(panel1);
            Controls.Add(Status);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            MaximizeBox = false;
            Name = "Collada";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            Text = "Import Settings";
            panel1.ResumeLayout(false);
            panel2.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();

        }
    }
}
