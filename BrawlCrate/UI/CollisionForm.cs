using BrawlCrate;

using BrawlLib.SSBB.ResourceNodes;
using System.ComponentModel;

namespace System.Windows.Forms
{
    public class CollisionForm : Form
    {
        #region Designer

        private CollisionEditor CollisionEditorControl;

        private void InitializeComponent()
        {
            CollisionEditorControl = new CollisionEditor();
            SuspendLayout();
			// 
			// CollisionEditorControl
			// 
			CollisionEditorControl.BackColor = Drawing.Color.Lavender;
			CollisionEditorControl.Dock = DockStyle.Fill;
			CollisionEditorControl.Location = new Drawing.Point(0, 0);
			CollisionEditorControl.Name = "collisionEditor1";
			CollisionEditorControl.Size = new Drawing.Size(800, 600);
			CollisionEditorControl.TabIndex = 0;
            // 
            // CollisionForm
            // 
            ClientSize = new Drawing.Size(800, 600);
            Controls.Add(CollisionEditorControl);
            Icon = BrawlLib.Properties.Resources.Icon;
            MinimizeBox = false;
            Name = "CollisionForm";
            Text = "Collision Editor";
            ResumeLayout(false);
        }

        #endregion

        private CollisionNode _node;

        public CollisionForm()
        {
            InitializeComponent();
            Text = $"{Program.AssemblyTitleShort} - Collision Editor";
        }

        public DialogResult ShowDialog(IWin32Window owner, CollisionNode node)
        {
            _node = node;
            try
            {
                return ShowDialog(owner);
            }
            finally
            {
                _node = null;
            }
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);

			CollisionEditorControl.CollisionFormShown(_node);

			// The text will now tell you what kind of collision you are editing by using the node's name.
			Text = $"{Program.AssemblyTitleShort} - Editing {_node.Name} - Collision Editor";
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);

			// It was moved so that CollisionEditor takes care of other forms running inside. 
			CollisionEditorControl.CollisionFormClosing();
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            MainForm.Instance.Visible = true;
            MainForm.Instance.Refresh();
        }

		//protected override void OnLostFocus(EventArgs e)
		//{
		//	base.OnLostFocus(e);
		//	this.Focus();
		//}

		//// These lets us know if the collision form is activated. 
		//// Useful for showing/hiding other windows that CollisionEditor makes use of.
		//protected override void OnActivated(EventArgs e)
		//{
		//	base.OnActivated(e);

		//	//CollisionEditorControl.EditorFocused();
		//}
		//protected override void OnDeactivate(EventArgs e)
		//{
		//	base.OnDeactivate(e);
		//	this.Focus();

		//	CollisionEditorControl.EditorUnfocused();
		//}
	}
}