using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace BrawlLib.Internal.Windows.Forms.Moveset
{
    public class EventDescription : UserControl
    {
        #region Designer

        private void InitializeComponent()
        {
            description = new RichTextBox();
            SuspendLayout();
            // 
            // description
            // 
            description.BackColor = SystemColors.Control;
            description.BorderStyle = BorderStyle.None;
            description.Cursor = Cursors.Default;
            description.Dock = DockStyle.Fill;
            description.Font = new Font("Microsoft Sans Serif", 11.25F, FontStyle.Regular, GraphicsUnit.Point,
                (byte) 0);
            description.ForeColor = Color.Black;
            description.Location = new Point(0, 0);
            description.Name = "description";
            description.ReadOnly = true;
            description.ScrollBars = RichTextBoxScrollBars.Vertical;
            description.Size = new Size(413, 284);
            description.TabIndex = 1;
            description.Text = "No Description Available.";
            description.TextChanged += new EventHandler(description_TextChanged);
            // 
            // EventDescription
            // 
            Controls.Add(description);
            Name = "EventDescription";
            Size = new Size(413, 284);
            ResumeLayout(false);
        }

        #endregion

        public RichTextBox description;

        private int index = -2;

        private ActionEventInfo _targetNode;

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ActionEventInfo TargetNode
        {
            get => _targetNode;
            set
            {
                _targetNode = value;
                EventChanged();
            }
        }

        public void SetTarget(ActionEventInfo info, int i)
        {
            index = i;
            TargetNode = info;
        }

        public void EventChanged()
        {
            if (index == -2 || TargetNode == null)
            {
                description.Text = "No Description Available.";
            }
            else if (index == -1)
            {
                description.Text = string.IsNullOrEmpty(TargetNode._description)
                    ? "No Description Available."
                    : TargetNode._description;
            }
            else if (TargetNode.pDescs != null && TargetNode.pDescs.Length != 0 && TargetNode.pDescs.Length > index)
            {
                description.Text = string.IsNullOrEmpty(TargetNode.pDescs[index])
                    ? "No Description Available."
                    : TargetNode.pDescs[index];
            }
            else
            {
                description.Text = "No Description Available.";
            }
        }

        public EventDescription()
        {
            InitializeComponent();
        }

        private void description_TextChanged(object sender, EventArgs e)
        {
        }
    }
}