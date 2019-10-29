namespace System.Windows.Forms
{
    public class FormModifyEvent : ThemedForm
    {
        public EventModifier eventModifier1;

        public FormModifyEvent()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            eventModifier1 = new EventModifier();
            SuspendLayout();
            // 
            // eventModifier1
            // 
            eventModifier1.AutoSize = true;
            eventModifier1.Dock = DockStyle.Fill;
            eventModifier1.Location = new Drawing.Point(0, 0);
            eventModifier1.Name = "eventModifier1";
            eventModifier1.Size = new Drawing.Size(284, 262);
            eventModifier1.TabIndex = 0;
            eventModifier1.Completed += new EventHandler(eventModifier1_Completed);
            // 
            // FormModifyEvent
            // 
            ClientSize = new Drawing.Size(284, 262);
            Controls.Add(eventModifier1);
            Name = "FormModifyEvent";
            Text = "Event Modifier";
            ResumeLayout(false);
            PerformLayout();
        }

        private void eventModifier1_Completed(object sender, EventArgs e)
        {
            DialogResult = eventModifier1.status;
            Close();
        }
    }
}