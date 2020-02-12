using BrawlLib.SSBB.ResourceNodes;

namespace System.Windows.Forms
{
	public class TEST_selectedLinkPlacement : Form
	{
		private CollisionLink _selectedLink = null;

		//0 = unselected, 1 = left, 2 = right
		//this selection only represents how many collision planes that this link has.
		private int[] mySelection = null;

		public void UpdateSelection(ref CollisionLink Link)
		{
			this._selectedLink = Link;
			this.mySelection = new int[this._selectedLink._members.Count];

			for (int i = 0; i < _selectedLink._members.Count; i++)
			{
				CollisionPlane p = _selectedLink._members[i];

				if (ReferenceEquals(this._selectedLink, p._linkLeft))
					this.mySelection[i] = 1;
				else if (ReferenceEquals(this._selectedLink, p._linkRight))
					this.mySelection[i] = 2;
				else
					this.mySelection[i] = 0;
			}

			this.Invalidate();
			this.Update();
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			if (this.mySelection != null && this.mySelection.Length > 0)
			{
				int maxH = 10;
				int y = 0;

				for (int ms = 0; ms < this.mySelection.Length; ms++)
				{
					int order = this.mySelection[ms];

					if (y > this.ClientSize.Height)
						break;

					Drawing.Brush brush = ((order == 1) ? Drawing.Brushes.Aqua : ((order == 2) ? Drawing.Brushes.Turquoise : Drawing.Brushes.Black));
					int dvBy = ((order == 1 || order == 2) ? 2 : 1);
					int locatX = (order == 2) ? (this.ClientSize.Width / dvBy) : 0;
					int szw = (order == 0) ? this.ClientSize.Width : (this.ClientSize.Width / dvBy);

					e.Graphics.FillRectangle(brush, locatX, y, szw, maxH);
					e.Graphics.DrawLine(new Drawing.Pen(Drawing.Color.Black, 1), 0, y + maxH, this.ClientSize.Width, y + maxH);

					y = y + maxH + 1;
				} 
			}

			base.OnPaint(e);
		}

		public TEST_selectedLinkPlacement()
		{
			this.InitializeComponent();
		}


		#region Designer



		public void InitializeComponent()
		{
			this.ResumeLayout(true);

			this.Name = "TEST_selectedLinkPlacement";
			this.Text = "Link Placement Test";
			this.FormBorderStyle = FormBorderStyle.SizableToolWindow;
			this.TopMost = true;
			this.Opacity = 0.8d;
			this.Width = 200;
			this.Height = 400;

			this.PerformLayout();
			this.ResumeLayout(false);
		}
		#endregion
	}
}
