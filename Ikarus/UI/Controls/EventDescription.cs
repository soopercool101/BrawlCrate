using System.ComponentModel;
using Ikarus;

namespace System.Windows.Forms
{
    public class EventDescription : UserControl
    {
        #region Designer

        private void InitializeComponent()
        {
            this.description = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
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
            this.description.ReadOnly = true;
            this.description.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.description.Size = new System.Drawing.Size(413, 284);
            this.description.TabIndex = 1;
            this.description.Text = "No Description Available.";
            this.description.TextChanged += new System.EventHandler(this.description_TextChanged);
            // 
            // EventDescription
            // 
            this.Controls.Add(this.description);
            this.Name = "EventDescription";
            this.Size = new System.Drawing.Size(413, 284);
            this.ResumeLayout(false);

        }

        #endregion

        public RichTextBox description;

        int index = -2;

        private EventInformation _targetNode;
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public EventInformation TargetNode
        {
            get { return _targetNode; }
            set { _targetNode = value; EventChanged(); }
        }

        public void SetTarget(EventInformation info, int i)
        {
            index = i;
            TargetNode = info;
        }

        public void EventChanged()
        {
            if (index == -2 || TargetNode == null)
                description.Text = "No Description Available.";
            else if (index == -1)
                description.Text = String.IsNullOrEmpty(TargetNode._description) ? "No Description Available." : TargetNode._description;
            else if (TargetNode._paramDescs != null && TargetNode._paramDescs.Length != 0 && TargetNode._paramDescs.Length > index)
                description.Text = String.IsNullOrEmpty(TargetNode._paramDescs[index]) ? "No Description Available." : TargetNode._paramDescs[index];
            else
                description.Text = "No Description Available.";
        }

        public EventDescription() { InitializeComponent(); }

        private void description_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
