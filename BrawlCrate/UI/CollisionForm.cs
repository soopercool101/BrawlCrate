using BrawlLib.SSBB.ResourceNodes;

using System;
using System.Drawing;
using System.Windows.Forms;

namespace BrawlCrate.UI
{
    public class CollisionForm : Form
    {
		#region Designer

		private CollisionEditor CollisionEditorControl;
		private CollisionEditorOld CollisionEditorControlOld;

		private bool UseOld = true;

		private void InitializeComponent()
		{
			CollisionEditorControl = new CollisionEditor();
			CollisionEditorControlOld = new CollisionEditorOld();

			SuspendLayout();
			// 
			// CollisionEditorControl
			// 
			CollisionEditorControl.BackColor = Color.Lavender;
			CollisionEditorControl.Dock = DockStyle.Fill;
			CollisionEditorControl.Location = new Point(0, 0);
			CollisionEditorControl.Name = "collisionEditorControl";
			CollisionEditorControl.Size = new Size(800, 600);
			CollisionEditorControl.TabIndex = 0;
			// 
			// CollisionEditorControlOld
			// 
			CollisionEditorControlOld.BackColor = Color.Lavender;
			CollisionEditorControlOld.Dock = DockStyle.Fill;
			CollisionEditorControlOld.Location = new Point(0, 0);
			CollisionEditorControlOld.Name = "collisionEditorControlOld";
			CollisionEditorControlOld.Size = new Size(800, 600);
			CollisionEditorControlOld.TabIndex = 0;
			//
			// CollisionForm
			// 
			ClientSize = new Size(800, 600);

			if (UseOld)
				Controls.Add(CollisionEditorControlOld);
			else
				Controls.Add(CollisionEditorControl);

            Icon = BrawlLib.Properties.Resources.Icon;
            MinimizeBox = false;
            Name = "CollisionForm";

			ResumeLayout(false);
        }

        #endregion

        private CollisionNode _node;

        public CollisionForm()
        {
			this.UseOld = false;
            InitializeComponent();
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

			// The text will now tell you what kind of collision you are editing by using the collision node's name.
			Text = $"{Program.AssemblyTitleShort} - Editing {_node.Name} - Collision Editor";

			if (UseOld)
			{
				CollisionEditorControlOld.TargetNode = _node;
				CollisionEditorControlOld._modelPanel.Capture();
			}
			else
				CollisionEditorControl.CollisionFormShown(_node);
		}

		protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            base.OnClosing(e);

			if (UseOld)
			{
				CollisionEditorControlOld.TargetNode = null;
				CollisionEditorControlOld._modelPanel.Release();
			}
			else
			{
				// It was moved so that CollisionEditor takes care of other forms running inside
				// plus manage the ability to either cancel the window from closing or not.

				if (CollisionEditorControl.CollisionFormClosing())
					e.Cancel = true;
			}
		}

		protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            MainForm.Instance.Visible = true;
            MainForm.Instance.Refresh();
        }
    }
}