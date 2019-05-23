namespace System.Windows.Forms
{
    public class FormModifyEvent : Form
    {
        public EventModifier eventModifier;
        public FormModifyEvent() { InitializeComponent(); }
        private void InitializeComponent()
        {
            this.eventModifier = new System.Windows.Forms.EventModifier();
            this.SuspendLayout();
            // 
            // eventModifier1
            // 
            this.eventModifier.AutoSize = true;
            this.eventModifier.Dock = System.Windows.Forms.DockStyle.Fill;
            this.eventModifier.Location = new System.Drawing.Point(0, 0);
            this.eventModifier.Name = "eventModifier1";
            this.eventModifier.Size = new System.Drawing.Size(284, 262);
            this.eventModifier.TabIndex = 0;
            this.eventModifier.Completed += new System.EventHandler(this.eventModifier1_Completed);
            // 
            // FormModifyEvent
            // 
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.eventModifier);
            this.Name = "FormModifyEvent";
            this.Text = "Event Modifier";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private void eventModifier1_Completed(object sender, EventArgs e)
        {
            DialogResult = eventModifier._status; Close();
        }
    }
}
