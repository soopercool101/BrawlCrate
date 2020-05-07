using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;

using BrawlLib.Modeling;

namespace System.Windows.Forms
{
	public class CollisionEditor_UndoRedoMenu : Form
	{
		#region Designer
		private UndoRedoUIListBox UndoRedoListBox;

		private Label ActionLabel;

		private Button ClearHistoryButton;
		private Button CollisionStateInfoButton;
		private Button UndoRedoSetMaxLimitButton;

		private void InitializeComponent()
		{
			this.UndoRedoListBox = new UndoRedoUIListBox();
			this.ActionLabel = new Label();
			this.CollisionStateInfoButton = new Button();
			this.ClearHistoryButton = new Button();
			this.UndoRedoSetMaxLimitButton = new Button();

			this.SuspendLayout();

			// UndoRedoListBox
			this.UndoRedoListBox.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
			this.UndoRedoListBox.FormattingEnabled = true;
			this.UndoRedoListBox.IntegralHeight = false;
			this.UndoRedoListBox.Location = new Drawing.Point(12, 12);
			this.UndoRedoListBox.Size = new Drawing.Size(246, 263);
			this.UndoRedoListBox.Name = "UndoRedoListBox";
			this.UndoRedoListBox.TabIndex = 0;
			this.UndoRedoListBox.SelectedIndexChanged += UndoRedoListBox_SelectedIndexChanged;
			this.UndoRedoListBox.MouseDoubleClick += UndoRedoListBox_MouseDoubleClick;
			this.UndoRedoListBox.KeyPress += UndoRedoListBox_KeyPress;

			// ActionLabel
			this.ActionLabel.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
			this.ActionLabel.Location = new Drawing.Point(12, 278);
			this.ActionLabel.TextAlign = ContentAlignment.MiddleCenter;
			this.ActionLabel.BorderStyle = BorderStyle.FixedSingle;
			this.ActionLabel.Size = new Drawing.Size(246, 21);
			this.ActionLabel.Text = "Undo/Redo ?? Steps";
			this.ActionLabel.Name = "ActionLabel";
			this.ActionLabel.AutoSize = false;

			// CollisionStateInfoButton
			this.CollisionStateInfoButton.Enabled = false;
			this.CollisionStateInfoButton.Location = new Drawing.Point(11, 302);
			this.CollisionStateInfoButton.Size = new Drawing.Size(248, 24);
			this.CollisionStateInfoButton.TabIndex = 1;
			this.CollisionStateInfoButton.Text = "Selected State Info";
			this.CollisionStateInfoButton.Name = "CollisionStateInfoButton";
			this.CollisionStateInfoButton.Click += CollisionStateInfoButton_Click;

			// ClearHistoryButton
			this.ClearHistoryButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
			this.ClearHistoryButton.Enabled = false;
			this.ClearHistoryButton.Location = new Drawing.Point(137, 328);
			this.ClearHistoryButton.Size = new Drawing.Size(122, 24);
			this.ClearHistoryButton.TabIndex = 2;
			this.ClearHistoryButton.Text = "Clear History";
			this.ClearHistoryButton.Name = "ClearHistoryButton";
			this.ClearHistoryButton.Click += ClearHistoryButton_Click;

			// UndoRedoSetMaxLimitButton
			this.UndoRedoSetMaxLimitButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
			this.UndoRedoSetMaxLimitButton.Enabled = true;
			this.UndoRedoSetMaxLimitButton.Location = new Drawing.Point(11, 328);
			this.UndoRedoSetMaxLimitButton.Size = new Drawing.Size(122, 24);
			this.UndoRedoSetMaxLimitButton.TabIndex = 2;
			this.UndoRedoSetMaxLimitButton.Text = "Set Max Undo Limit";
			this.UndoRedoSetMaxLimitButton.Name = "UndoRedoSetMaxLimitButton";
			this.UndoRedoSetMaxLimitButton.Click += UndoRedoSetMaxLimitButton_Click; ;

			// CollisionEditor_UndoRedoMenu
			this.ClientSize = new Drawing.Size(270, 362);
			this.FormBorderStyle = FormBorderStyle.FixedToolWindow;
			this.Icon = BrawlLib.Properties.Resources.Icon;
			this.Name = "CollisionEditor_UndoRedoMenu";
			this.Text = "Undo/Redo History";
			this.AutoScaleMode = AutoScaleMode.Font;
			this.AutoScaleDimensions = new SizeF(6, 13);
			this.MaximizeBox = false;
			this.MinimumSize = this.Size;
			this.MaximumSize = this.Size;

			this.Controls.AddRange(new Control[]
			{
				this.UndoRedoListBox,
				this.ActionLabel,
				this.CollisionStateInfoButton,
				this.ClearHistoryButton,
				this.UndoRedoSetMaxLimitButton
			});

			this.ResumeLayout(false);
		}

		#endregion

		private CollisionEditor parentEditor;

		public CollisionEditor_UndoRedoMenu(CollisionEditor parentEditor)
		{
			this.parentEditor = parentEditor;

			InitializeComponent();
			UpdateMenu();
		}

		private void UndoRedoListBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			UpdateUndoRedoSteps();
		}
		private void UndoRedoListBox_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			UpdateCollisionEditorStates(UndoRedoListBox.UndoRedoSteps, UndoRedoListBox.CurrentUndoRedoIndex);
		}
		private void UndoRedoListBox_KeyPress(object sender, KeyPressEventArgs e)
		{
			//Trace.WriteLine("KeyChar: " + e.KeyChar+" | KeyInt: "+(int)e.KeyChar);

			if (e.KeyChar == 13)
			{
				UpdateCollisionEditorStates(UndoRedoListBox.UndoRedoSteps, UndoRedoListBox.CurrentUndoRedoIndex);
			}
		}

		private void ClearHistoryButton_Click(object sender, EventArgs e)
		{
			if (MessageBox.Show("Would you like to clear states history? If you do, then you will not be able to " +
							"undo or redo it.", "Clear Undo/Redo History", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
			{
				parentEditor.saveIndex = 0;

				parentEditor.undoSaves.Clear();
				parentEditor.redoSaves.Clear();
				parentEditor.undoSaves = new List<CollisionState>();
				parentEditor.redoSaves = new List<CollisionState>();

				parentEditor.CheckSaveIndex();

				UpdateMenu();
			}
		}

		private void CollisionStateInfoButton_Click(object sender, EventArgs e)
		{
			
		}

		private void UndoRedoSetMaxLimitButton_Click(object sender, EventArgs e)
		{
			
		}

		#region Menu Updates
		/// <summary> Updates this menu to either contain/remove states and update the selected indexing. </summary>
		public void UpdateMenu()
		{
			// Clear all items and reset their ordering.
			UndoRedoListBox.Items.Clear();
			//UndoRedoListBox.CurrentHoveredIndex = -1;
			UndoRedoListBox.CurrentUndoRedoIndex = -1;

			// Retrieve all available undo/redo items and start creating items
			// for the list box.
			List<CollisionState> UndoStates = parentEditor.undoSaves;
			List<CollisionState> RedoStates = parentEditor.redoSaves;

			List<CollisionState> CombinedStates = new List<CollisionState>();
			CombinedStates.AddRange(UndoStates);
			//CombinedStates.AddRange(RedoStates);


			string displayText = "Initial State";
			CollisionState State = (CombinedStates.Count > 0 && CombinedStates[0] != null) ? CombinedStates[0] : null;

			if (State != null && State._tag != null && State._tag.GetType() == typeof(CollisionStateAction))
			{
				CollisionStateAction stateAction = (CollisionStateAction)State._tag;
				displayText += " - " + GetCollisionStateActionText(stateAction);
			}

			StateItem CollisionStateItem = new StateItem();
			CollisionStateItem.State = State;
			CollisionStateItem.StateDisplay = displayText;

			UndoRedoListBox.Items.Add(CollisionStateItem);


			Trace.WriteLine(String.Format("UndoStates: {0} | RedoStates: {1} | Combined: {2}", UndoStates.Count, RedoStates.Count, CombinedStates.Count));

			for (int i = 0; i < CombinedStates.Count; ++i)
			{
				State = CombinedStates[i];
				//CollisionState State = CombinedStates[i];

				displayText = "Unknown";
				//string displayText = "Unknown";

				if (State._tag != null && State._tag.GetType() == typeof(CollisionStateAction))
				{
					CollisionStateAction stateAction = (CollisionStateAction)State._tag;
					displayText = GetCollisionStateActionText(stateAction);
				}

				CollisionStateItem = new StateItem();
				//StateItem CollisionStateItem = new StateItem();
				CollisionStateItem.State = State;
				CollisionStateItem.StateDisplay = displayText;
				CollisionStateItem.isRedo = i >= UndoStates.Count;

				UndoRedoListBox.Items.Add(CollisionStateItem);
			}

			// Set the current undo/redo index based on the CollisionEditor form.
			int ToIndex = parentEditor.saveIndex;
			//int ToIndex = parentEditor.saveIndex - 1;

			UndoRedoListBox.CurrentUndoRedoIndex = ToIndex;
			//Trace.WriteLine("CurrentUndoRedoIndex: " + ToIndex + " | ParentEditorSaveIndex: " + parentEditor.saveIndex);

			ClearHistoryButton.Enabled = UndoRedoListBox.Items.Count > 0;

			UpdateUndoRedoSteps();
		}

		private string GetCollisionStateActionText(CollisionStateAction Action)
		{
			switch (Action)
			{
				case CollisionStateAction.MoveUp:
					return "Move Up";
				case CollisionStateAction.MoveDown:
					return "Move Down";
				case CollisionStateAction.MoveLeft:
					return "Move Left";
				case CollisionStateAction.MoveRight:
					return "Move Right";

				case CollisionStateAction.MoveUpLong:
					return "Move Up - Long";
				case CollisionStateAction.MoveDownLong:
					return "Move Down - Long";
				case CollisionStateAction.MoveLeftLong:
					return "Move Left - Long";
				case CollisionStateAction.MoveRightLong:
					return "Move Right - Long";

				case CollisionStateAction.ChangeXCoordinate:
					return "Change X Position Coordinate";
				case CollisionStateAction.ChangeYCoordinate:
					return "Change Y Position Coordinate";

				case CollisionStateAction.ChangeXCoordinateMultiples:
					return "Change Multiple X Position Coordinates";
				case CollisionStateAction.ChangeYCoordinateMultiples:
					return "Change Multiple Y Position Coordinates";

				case CollisionStateAction.AlignX:
					return "Align Collision X";
				case CollisionStateAction.AlignY:
					return "Align Collision Y";

				case CollisionStateAction.MoveSelectedCollisions:
					return "Move Selected Collision(s)";
					

				default: return "";
			}
		}

		public void UpdateUndoRedoSteps()
		{
			int Steps = UndoRedoListBox.UndoRedoSteps;

			// The steps are negative, meaning that it is undoing.
			if (Steps < 0)
			{
				ActionLabel.Text = $"Undo {-Steps} {ActionText(-Steps)}";
			}
			// The steps are positive, meaning that it is redoing.
			else if (Steps > 0)
			{
				ActionLabel.Text = $"Redo {Steps} {ActionText(Steps)}";
			}
			// If there are no steps going on, then declare it as "no steps".
			else
			{
				ActionLabel.Text = "Undo/Redo no actions";
			}
		}
		#endregion

		#region Update To Collision Editor control

		// If Steps is negative, it is undoing; if 0, then nothing will happen; if
		// positive, then it is redoing.
		// CurrentIndex is used to know where in which part of the index is the
		// current undo/redo state is on.
		public void UpdateCollisionEditorStates(int Steps, int CurrentIndex)
		{
			Trace.WriteLine("Steps: " + Steps + " | Current Index: " + CurrentIndex);

			if (Steps == 0 || CurrentIndex < 0)
				return;

			int undoSavesCount = parentEditor.undoSaves.Count;

			parentEditor._selectedLinks.Clear();

			// Undoing steps
			if (Steps < 0)
			{
				// This sets the steps to be set into positive
				int StepsAbs = Steps * -1;

				List<CollisionState> StatesUndone = new List<CollisionState>();

				for (int step = 0; step < StepsAbs; ++step)
				{
					// Takes the current step (which is an index used in the current undo/redo state) 
					// and reduce it by the amount of steps to get the item.
					int UndoIndex = CurrentIndex - step - 1;

					// Make sure that undo indexing is not less than zero. If it is, then
					// stop the step process.
					if (UndoIndex < 0)
						break;

					if (UndoIndex >= undoSavesCount)
						continue;

					// This is such a weird issue, why do you have to clear the selected
					// links just to fix the bug that moves all the associated links?
					parentEditor._selectedLinks.Clear();

					// Create a state to store it into StatesUndone.
					var UndoSave = parentEditor.CreateUndoState(UndoIndex);
					// It is added as a item into the list.
					StatesUndone.Add(UndoSave);

					//UndoSave = null;
				}

				parentEditor.redoSaves.AddRange(StatesUndone);

				parentEditor.toolsStrip_Redo.Enabled = true;
				parentEditor.toolsStrip_UndoRedoMenu.Enabled = true;

				int index = CurrentIndex - StatesUndone.Count;
				int finalIndex = (index >= 0) ? index : 0;
				//int index = CurrentIndex - 1 - StatesUndone.Count;
				//int index = CurrentIndex + 1 - StatesUndone.Count;
				parentEditor.saveIndex = finalIndex;

				if (finalIndex <= 0)
					parentEditor.toolsStrip_Undo.Enabled = false;

				parentEditor._modelPanel.Invalidate();
				parentEditor.UpdatePropPanels();

				UpdateMenu();
			}
			// Redoing steps
			else
			{
				int StepsRedo = Steps;
				//int StepsRedo = UndoStates.Count - CurrentStep - 1;

				Trace.WriteLine("StepsRedo: "+StepsRedo+" | RedoSaves: "+parentEditor.redoSaves.Count);

				if (StepsRedo > parentEditor.redoSaves.Count)
				{
					parentEditor.toolsStrip_Redo.Enabled = false;
					return;
				}

				int StatesRedone = 0;
				int CurrentStateCount = undoSavesCount - CurrentIndex - 1;

				//for (int step = StepsRedo - 1; step >= 0; --step)
				for (int step = 0; step < StepsRedo; ++step)
				{
					int RedoIndex = CurrentStateCount - step;

					if (RedoIndex < 0 || RedoIndex >= parentEditor.redoSaves.Count)
						continue;	

					parentEditor._selectedLinks.Clear();

					parentEditor.PerformRedoState(RedoIndex);
					parentEditor.redoSaves.RemoveAt(RedoIndex);

					++StatesRedone;
				}

				parentEditor.toolsStrip_Redo.Enabled = parentEditor.redoSaves.Count > 0;

				parentEditor.toolsStrip_Undo.Enabled = true;
				parentEditor.toolsStrip_UndoRedoMenu.Enabled = true;

				int index = CurrentIndex + StatesRedone;
				parentEditor.saveIndex = index;

				parentEditor._modelPanel.Invalidate();
				parentEditor.UpdatePropPanels();

				UpdateMenu();
			}
		}
		#endregion

		#region Form Events
		private bool FormIsClosing = false;

		protected override void OnActivated(EventArgs e)
		{
			base.OnActivated(e);

			// Set the window opacity full if active (focused).
			// It only takes percentages or 1.0 (100%) as maximum.
			Opacity = 1.0f;
		}
		protected override void OnDeactivate(EventArgs e)
		{
			base.OnDeactivate(e);

			// Set the window opacity 60% if not active (focused).
			if (!IsDisposed && !FormIsClosing)
				Opacity = 0.6f;
		}
		protected override void OnClosing(CancelEventArgs e)
		{
			base.OnClosing(e);

			FormIsClosing = true;
		}
		protected override void OnClosed(EventArgs e)
		{
			parentEditor.NullifyUndoRedoMenu();

			base.OnClosed(e);
		}
		#endregion

		#region Miscellaneous

		private string ActionText(int Steps) { return (Steps > 1) ? "Actions" : "Action"; }

		public class UndoRedoUIListBox : ListBox
		{
			private int _CurrentHoveredIndex = -1;
			private int _CurrentUndoRedoIndex = -1;

			/// <summary> Get or set the current hover index. </summary>
			public int CurrentHoveredIndex
			{
				get { return _CurrentHoveredIndex; }
				set { _CurrentHoveredIndex = value; Invalidate(); }
			}
			/// <summary> 
			/// Get or set the current undo/redo index. This will be used as a primary source for 
			/// <see cref="UndoRedoSteps"/> and many other things that make use of the current undo/redo index.
			/// </summary>
			public int CurrentUndoRedoIndex
			{
				get { return _CurrentUndoRedoIndex; }
				set { _CurrentUndoRedoIndex = value; Invalidate(); }
			}

			/// <summary>
			/// Get the steps that the undo/redo action is being done.
			/// 
			/// If the value returns negative, then that means that it is being undone (undo);
			/// else if it returns positive, then is being redone (redo). If it returns zero (0),
			/// then that is because <see cref="CurrentUndoRedoIndex"/> or <see cref="CurrentHoveredIndex"/>
			/// is -1 in value or because it happens to be that <see cref="CurrentHoveredIndex"/> is on
			/// the same index as <see cref="CurrentUndoRedoIndex"/>.
			/// </summary>
			public int UndoRedoSteps
			{
				get
				{
					if (_CurrentUndoRedoIndex < 0 || _CurrentHoveredIndex < 0)
						return 0;

					return _CurrentHoveredIndex - _CurrentUndoRedoIndex;
				}
			}

			// ListBoxMouseLocation will always be empty if the mouse is currently not inside.
			private Drawing.Point? ListBoxMouseLocation = null;

			public UndoRedoUIListBox()
			{
				IntegralHeight = false;
				DrawMode = DrawMode.OwnerDrawVariable;
			}

			protected override void OnSelectedIndexChanged(EventArgs e)
			{
				base.OnSelectedIndexChanged(e);

				// Required to rerender all drawn items as they do not reset
				// everytime an index gets changed, leaving the visuals glitched
				// out.
				this.Invalidate();
			}

			protected override void OnMouseWheel(MouseEventArgs e)
			{
				base.OnMouseWheel(e);
			}
			protected override void OnMouseMove(MouseEventArgs e)
			{
				base.OnMouseMove(e);
				ListBoxMouseLocation = e.Location;
			}
			protected override void OnMouseEnter(EventArgs e)
			{
				base.OnMouseEnter(e);
			}
			protected override void OnMouseLeave(EventArgs e)
			{
				ListBoxMouseLocation = null;
				base.OnMouseLeave(e);
			}

			protected override void OnDrawItem(DrawItemEventArgs e)
			{
				// Make sure that the current undo/redo index is available. If not then do not bother
				// making any attempts.
				if (_CurrentUndoRedoIndex >= 0)
				{
					int index = e.Index;

					if (index <= -1)
					{
						base.OnDrawItem(e);
						return;
					}

					Rectangle bounds = e.Bounds;
					Rectangle boundsOriginal = e.Bounds;
					Graphics graphics = e.Graphics;

					bool Hovered = e.State.HasFlag(DrawItemState.HotLight);
					bool Selected = e.State.HasFlag(DrawItemState.Selected);

					Color rectangleColor = ((StateItem)Items[index]).isRedo ? Color.SkyBlue : this.BackColor;

					//e.DrawBackground();

					bool MouseTakesPriorty = false;

					if (ListBoxMouseLocation != null)
					{
						if (ListBoxMouseLocation.Value.X > bounds.X && ListBoxMouseLocation.Value.X < bounds.X + bounds.Width &&
							ListBoxMouseLocation.Value.Y > bounds.Y && ListBoxMouseLocation.Value.Y < bounds.Y + bounds.Height)
						{
							_CurrentHoveredIndex = index;
							MouseTakesPriorty = true;
						}
					}

					bool InSelection = false;

					if (!MouseTakesPriorty && (Hovered || Selected))
					{
						if (index >= 0)
						{
							_CurrentHoveredIndex = index;
						}
					}


					if (index == _CurrentUndoRedoIndex)
					{
						rectangleColor = SystemColors.HotTrack;
						InSelection = true;
					}
					else if (index >= 0 && _CurrentUndoRedoIndex >= 0)
					{
						if ((index > _CurrentUndoRedoIndex && index <= _CurrentHoveredIndex) || (index >= _CurrentHoveredIndex && index < _CurrentUndoRedoIndex))
						{
							rectangleColor = SystemColors.Highlight;

							InSelection = true;
						}
					}

					// Let brawlcrate take care of the string and what item should be used.

					string i = ((StateItem)Items[index]).StateDisplay;
					
					SolidBrush sb = new SolidBrush(rectangleColor);

					graphics.FillRectangle(sb, bounds);
					sb.Dispose();

					Color col = InSelection ? SystemColors.HighlightText : ForeColor;

					sb = new SolidBrush(col);
					graphics.DrawString(i, Font, sb, new PointF(boundsOriginal.X, boundsOriginal.Y));
					sb.Dispose();
				}
				else
				{
					base.OnDrawItem(e);
				}
			}
		}

		private class StateItem
		{
			public CollisionState State;
			public string StateDisplay;

			public bool isRedo;
		}

		#endregion
	}
}
