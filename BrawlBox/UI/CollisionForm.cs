using BrawlLib.SSBB.ResourceNodes;
using BrawlBox;

namespace System.Windows.Forms
{
    public class CollisionForm : Form
    {
        #region Designer

        private CollisionEditor collisionEditor1;
    
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CollisionForm));
            this.collisionEditor1 = new System.Windows.Forms.CollisionEditor();
            this.SuspendLayout();
            // 
            // collisionEditor1
            // 
            this.collisionEditor1.BackColor = System.Drawing.Color.Lavender;
            this.collisionEditor1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.collisionEditor1.Location = new System.Drawing.Point(0, 0);
            this.collisionEditor1.Name = "collisionEditor1";
            this.collisionEditor1.Size = new System.Drawing.Size(800, 600);
            this.collisionEditor1.TabIndex = 0;
            // 
            // CollisionForm
            // 
            this.ClientSize = new System.Drawing.Size(800, 600);
            this.Controls.Add(this.collisionEditor1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimizeBox = false;
            this.Name = "CollisionForm";
            this.Text = "Collision Editor";
            this.ResumeLayout(false);

        }

        #endregion

        CollisionNode _node;

        public CollisionForm() { InitializeComponent(); Text = Program.AssemblyTitle + " - Collision Editor"; }

        public DialogResult ShowDialog(IWin32Window owner, CollisionNode node)
        {
            _node = node;
            try { return ShowDialog(owner); }
            finally {  _node = null; }
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            collisionEditor1.TargetNode = _node;
            collisionEditor1._modelPanel.Capture();
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            base.OnClosing(e);
            collisionEditor1.TargetNode = null;
            collisionEditor1._modelPanel.Release();
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            MainForm.Instance.Visible = true;
            MainForm.Instance.Refresh();
        }
    }
}
