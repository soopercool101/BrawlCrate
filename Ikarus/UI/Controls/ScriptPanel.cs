using System;
using BrawlLib.SSBB.ResourceNodes;
using System.ComponentModel;
using System.Windows.Forms;
using Ikarus.MovesetFile;
using Ikarus.ModelViewer;

namespace Ikarus.UI
{
    public enum ScriptType
    {
        Actions,
        Subactions,
        Subroutines
    }

    public class ScriptPanel : UserControl
    {
        public delegate void ReferenceEventHandler(ResourceNode node);

        #region Designer

        private OpenFileDialog dlgOpen;
        private ContextMenuStrip ctxSubActions;
        private ToolStripMenuItem add;
        private ToolStripMenuItem subtract;
        private ToolStripMenuItem sourceToolStripMenuItem;
        private ToolStripSeparator toolStripMenuItem1;
        private ToolStripMenuItem exportToolStripMenuItem;
        private ToolStripMenuItem replaceToolStripMenuItem;
        private ToolStripMenuItem portToolStripMenuItem;
        private SaveFileDialog dlgSave;
        private IContainer components;
        private ToolStripMenuItem toolStripMenuItem3;
        private ToolStripMenuItem toolStripMenuItem4;
        private ToolStripMenuItem toolStripMenuItem5;
        private ToolStripMenuItem toolStripMenuItem6;
        private ToolStripMenuItem toolStripMenuItem7;
        private ToolStripMenuItem toolStripMenuItem8;
        private ToolStripMenuItem Source;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripMenuItem removeAllToolStripMenuItem;
        private ToolStripMenuItem addCustomAmountToolStripMenuItem;
        private Panel ActionEditor;
        private Panel SubActionFlagsPanel;
        private NumericInputBox inTransTime;
        private CheckBox chkNoOutTrans;
        private Label label1;
        private Button flagsToggle;
        private CheckBox chkMovesChar;
        private CheckBox chkTransOutStart;
        private CheckBox chkUnk;
        private CheckBox chkLoop;
        private CheckBox chkFixedTrans;
        private CheckBox chkFixedRot;
        private CheckBox chkFixedScale;
        private Panel panel2;
        public ComboBox comboActionEntry;
        private Panel ActionFlagsPanel;
        public EventModifier eventModifier;
        private ToolStripMenuItem renameToolStripMenuItem;
        private NewScriptEditor scriptEditor1;
        private ScriptEditor scriptEditor2;
        internal Label lblActionName;
        private Button button1;
        private Splitter spltEventMod;

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.sourceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.exportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.replaceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.portToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ctxSubActions = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.renameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.Source = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.add = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem7 = new System.Windows.Forms.ToolStripMenuItem();
            this.subtract = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem5 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem6 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem8 = new System.Windows.Forms.ToolStripMenuItem();
            this.removeAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addCustomAmountToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dlgOpen = new System.Windows.Forms.OpenFileDialog();
            this.dlgSave = new System.Windows.Forms.SaveFileDialog();
            this.ActionEditor = new System.Windows.Forms.Panel();
            this.scriptEditor1 = new NewScriptEditor();
            this.scriptEditor2 = new ScriptEditor();
            this.button1 = new System.Windows.Forms.Button();
            this.ActionFlagsPanel = new System.Windows.Forms.Panel();
            this.SubActionFlagsPanel = new System.Windows.Forms.Panel();
            this.chkUnk = new System.Windows.Forms.CheckBox();
            this.chkLoop = new System.Windows.Forms.CheckBox();
            this.chkFixedTrans = new System.Windows.Forms.CheckBox();
            this.chkFixedRot = new System.Windows.Forms.CheckBox();
            this.chkFixedScale = new System.Windows.Forms.CheckBox();
            this.chkMovesChar = new System.Windows.Forms.CheckBox();
            this.chkTransOutStart = new System.Windows.Forms.CheckBox();
            this.inTransTime = new System.Windows.Forms.NumericInputBox();
            this.chkNoOutTrans = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.lblActionName = new System.Windows.Forms.Label();
            this.comboActionEntry = new System.Windows.Forms.ComboBox();
            this.flagsToggle = new System.Windows.Forms.Button();
            this.spltEventMod = new System.Windows.Forms.Splitter();
            this.eventModifier = new EventModifier();
            this.ctxSubActions.SuspendLayout();
            this.ActionEditor.SuspendLayout();
            this.SubActionFlagsPanel.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // sourceToolStripMenuItem
            // 
            this.sourceToolStripMenuItem.Name = "sourceToolStripMenuItem";
            this.sourceToolStripMenuItem.Size = new System.Drawing.Size(32, 19);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(6, 6);
            // 
            // exportToolStripMenuItem
            // 
            this.exportToolStripMenuItem.Name = "exportToolStripMenuItem";
            this.exportToolStripMenuItem.Size = new System.Drawing.Size(32, 19);
            // 
            // replaceToolStripMenuItem
            // 
            this.replaceToolStripMenuItem.Name = "replaceToolStripMenuItem";
            this.replaceToolStripMenuItem.Size = new System.Drawing.Size(32, 19);
            // 
            // portToolStripMenuItem
            // 
            this.portToolStripMenuItem.Name = "portToolStripMenuItem";
            this.portToolStripMenuItem.Size = new System.Drawing.Size(32, 19);
            // 
            // ctxSubActions
            // 
            this.ctxSubActions.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.renameToolStripMenuItem});
            this.ctxSubActions.Name = "ctxBox";
            this.ctxSubActions.Size = new System.Drawing.Size(118, 26);
            this.ctxSubActions.Text = "Subaction";
            // 
            // renameToolStripMenuItem
            // 
            this.renameToolStripMenuItem.Name = "renameToolStripMenuItem";
            this.renameToolStripMenuItem.Size = new System.Drawing.Size(117, 22);
            this.renameToolStripMenuItem.Text = "Rename";
            this.renameToolStripMenuItem.Click += new System.EventHandler(this.renameToolStripMenuItem_Click);
            // 
            // Source
            // 
            this.Source.Name = "Source";
            this.Source.Size = new System.Drawing.Size(32, 19);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 6);
            // 
            // add
            // 
            this.add.Name = "add";
            this.add.Size = new System.Drawing.Size(32, 19);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(32, 19);
            // 
            // toolStripMenuItem4
            // 
            this.toolStripMenuItem4.Name = "toolStripMenuItem4";
            this.toolStripMenuItem4.Size = new System.Drawing.Size(32, 19);
            // 
            // toolStripMenuItem7
            // 
            this.toolStripMenuItem7.Name = "toolStripMenuItem7";
            this.toolStripMenuItem7.Size = new System.Drawing.Size(32, 19);
            // 
            // subtract
            // 
            this.subtract.Name = "subtract";
            this.subtract.Size = new System.Drawing.Size(32, 19);
            // 
            // toolStripMenuItem5
            // 
            this.toolStripMenuItem5.Name = "toolStripMenuItem5";
            this.toolStripMenuItem5.Size = new System.Drawing.Size(32, 19);
            // 
            // toolStripMenuItem6
            // 
            this.toolStripMenuItem6.Name = "toolStripMenuItem6";
            this.toolStripMenuItem6.Size = new System.Drawing.Size(32, 19);
            // 
            // toolStripMenuItem8
            // 
            this.toolStripMenuItem8.Name = "toolStripMenuItem8";
            this.toolStripMenuItem8.Size = new System.Drawing.Size(32, 19);
            // 
            // removeAllToolStripMenuItem
            // 
            this.removeAllToolStripMenuItem.Name = "removeAllToolStripMenuItem";
            this.removeAllToolStripMenuItem.Size = new System.Drawing.Size(32, 19);
            // 
            // addCustomAmountToolStripMenuItem
            // 
            this.addCustomAmountToolStripMenuItem.Name = "addCustomAmountToolStripMenuItem";
            this.addCustomAmountToolStripMenuItem.Size = new System.Drawing.Size(32, 19);
            // 
            // ActionEditor
            // 
            this.ActionEditor.Controls.Add(this.scriptEditor1);
            this.ActionEditor.Controls.Add(this.scriptEditor2);
            this.ActionEditor.Controls.Add(this.button1);
            this.ActionEditor.Controls.Add(this.ActionFlagsPanel);
            this.ActionEditor.Controls.Add(this.SubActionFlagsPanel);
            this.ActionEditor.Controls.Add(this.panel2);
            this.ActionEditor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ActionEditor.Location = new System.Drawing.Point(0, 0);
            this.ActionEditor.Name = "ActionEditor";
            this.ActionEditor.Size = new System.Drawing.Size(229, 355);
            this.ActionEditor.TabIndex = 26;
            // 
            // scriptEditor1
            // 
            this.scriptEditor1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.scriptEditor1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.scriptEditor1.Location = new System.Drawing.Point(0, 21);
            this.scriptEditor1.Name = "scriptEditor1";
            this.scriptEditor1.Size = new System.Drawing.Size(229, 310);
            this.scriptEditor1.TabIndex = 0;
            // 
            // scriptEditor2
            // 
            this.scriptEditor2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.scriptEditor2.Location = new System.Drawing.Point(0, 21);
            this.scriptEditor2.Name = "scriptEditor2";
            this.scriptEditor2.Padding = new System.Windows.Forms.Padding(1);
            this.scriptEditor2.Size = new System.Drawing.Size(229, 310);
            this.scriptEditor2.TabIndex = 37;
            this.scriptEditor2.Visible = false;
            // 
            // button1
            // 
            this.button1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.button1.Location = new System.Drawing.Point(0, 331);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(229, 24);
            this.button1.TabIndex = 38;
            this.button1.Text = "Switch Editor";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // ActionFlagsPanel
            // 
            this.ActionFlagsPanel.Location = new System.Drawing.Point(0, 168);
            this.ActionFlagsPanel.Margin = new System.Windows.Forms.Padding(0);
            this.ActionFlagsPanel.Name = "ActionFlagsPanel";
            this.ActionFlagsPanel.Size = new System.Drawing.Size(201, 147);
            this.ActionFlagsPanel.TabIndex = 37;
            this.ActionFlagsPanel.Visible = false;
            // 
            // SubActionFlagsPanel
            // 
            this.SubActionFlagsPanel.Controls.Add(this.chkUnk);
            this.SubActionFlagsPanel.Controls.Add(this.chkLoop);
            this.SubActionFlagsPanel.Controls.Add(this.chkFixedTrans);
            this.SubActionFlagsPanel.Controls.Add(this.chkFixedRot);
            this.SubActionFlagsPanel.Controls.Add(this.chkFixedScale);
            this.SubActionFlagsPanel.Controls.Add(this.chkMovesChar);
            this.SubActionFlagsPanel.Controls.Add(this.chkTransOutStart);
            this.SubActionFlagsPanel.Controls.Add(this.inTransTime);
            this.SubActionFlagsPanel.Controls.Add(this.chkNoOutTrans);
            this.SubActionFlagsPanel.Controls.Add(this.label1);
            this.SubActionFlagsPanel.Location = new System.Drawing.Point(0, 21);
            this.SubActionFlagsPanel.Margin = new System.Windows.Forms.Padding(0);
            this.SubActionFlagsPanel.Name = "SubActionFlagsPanel";
            this.SubActionFlagsPanel.Size = new System.Drawing.Size(201, 147);
            this.SubActionFlagsPanel.TabIndex = 27;
            this.SubActionFlagsPanel.Visible = false;
            // 
            // chkUnk
            // 
            this.chkUnk.AutoSize = true;
            this.chkUnk.Location = new System.Drawing.Point(63, 125);
            this.chkUnk.Name = "chkUnk";
            this.chkUnk.Size = new System.Drawing.Size(72, 17);
            this.chkUnk.TabIndex = 36;
            this.chkUnk.Text = "Unknown";
            this.chkUnk.UseVisualStyleBackColor = true;
            this.chkUnk.CheckedChanged += new System.EventHandler(this.chkUnk_CheckedChanged);
            // 
            // chkLoop
            // 
            this.chkLoop.AutoSize = true;
            this.chkLoop.Location = new System.Drawing.Point(7, 125);
            this.chkLoop.Name = "chkLoop";
            this.chkLoop.Size = new System.Drawing.Size(50, 17);
            this.chkLoop.TabIndex = 35;
            this.chkLoop.Text = "Loop";
            this.chkLoop.UseVisualStyleBackColor = true;
            this.chkLoop.CheckedChanged += new System.EventHandler(this.chkLoop_CheckedChanged);
            // 
            // chkFixedTrans
            // 
            this.chkFixedTrans.AutoSize = true;
            this.chkFixedTrans.Location = new System.Drawing.Point(7, 108);
            this.chkFixedTrans.Name = "chkFixedTrans";
            this.chkFixedTrans.Size = new System.Drawing.Size(106, 17);
            this.chkFixedTrans.TabIndex = 34;
            this.chkFixedTrans.Text = "Fixed Translation";
            this.chkFixedTrans.UseVisualStyleBackColor = true;
            this.chkFixedTrans.CheckedChanged += new System.EventHandler(this.chkFixedTrans_CheckedChanged);
            // 
            // chkFixedRot
            // 
            this.chkFixedRot.AutoSize = true;
            this.chkFixedRot.Location = new System.Drawing.Point(7, 91);
            this.chkFixedRot.Name = "chkFixedRot";
            this.chkFixedRot.Size = new System.Drawing.Size(94, 17);
            this.chkFixedRot.TabIndex = 33;
            this.chkFixedRot.Text = "Fixed Rotation";
            this.chkFixedRot.UseVisualStyleBackColor = true;
            this.chkFixedRot.CheckedChanged += new System.EventHandler(this.chkFixedRot_CheckedChanged);
            // 
            // chkFixedScale
            // 
            this.chkFixedScale.AutoSize = true;
            this.chkFixedScale.Location = new System.Drawing.Point(7, 74);
            this.chkFixedScale.Name = "chkFixedScale";
            this.chkFixedScale.Size = new System.Drawing.Size(81, 17);
            this.chkFixedScale.TabIndex = 32;
            this.chkFixedScale.Text = "Fixed Scale";
            this.chkFixedScale.UseVisualStyleBackColor = true;
            this.chkFixedScale.CheckedChanged += new System.EventHandler(this.chkFixedScale_CheckedChanged);
            // 
            // chkMovesChar
            // 
            this.chkMovesChar.AutoSize = true;
            this.chkMovesChar.Location = new System.Drawing.Point(7, 57);
            this.chkMovesChar.Name = "chkMovesChar";
            this.chkMovesChar.Size = new System.Drawing.Size(107, 17);
            this.chkMovesChar.TabIndex = 31;
            this.chkMovesChar.Text = "Moves Character";
            this.chkMovesChar.UseVisualStyleBackColor = true;
            this.chkMovesChar.CheckedChanged += new System.EventHandler(this.chkMovesChar_CheckedChanged);
            // 
            // chkTransOutStart
            // 
            this.chkTransOutStart.AutoSize = true;
            this.chkTransOutStart.Location = new System.Drawing.Point(7, 40);
            this.chkTransOutStart.Name = "chkTransOutStart";
            this.chkTransOutStart.Size = new System.Drawing.Size(143, 17);
            this.chkTransOutStart.TabIndex = 30;
            this.chkTransOutStart.Text = "Transition Out From Start";
            this.chkTransOutStart.UseVisualStyleBackColor = true;
            this.chkTransOutStart.CheckedChanged += new System.EventHandler(this.chkTransOutStart_CheckedChanged);
            // 
            // inTransTime
            // 
            this.inTransTime.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.inTransTime.Integral = false;
            this.inTransTime.Location = new System.Drawing.Point(107, 4);
            this.inTransTime.MaximumValue = 3.402823E+38F;
            this.inTransTime.MinimumValue = -3.402823E+38F;
            this.inTransTime.Name = "inTransTime";
            this.inTransTime.Size = new System.Drawing.Size(89, 20);
            this.inTransTime.TabIndex = 29;
            this.inTransTime.Text = "0";
            this.inTransTime.ValueChanged += new System.EventHandler(this.inTransTime_ValueChanged);
            // 
            // chkNoOutTrans
            // 
            this.chkNoOutTrans.AutoSize = true;
            this.chkNoOutTrans.Location = new System.Drawing.Point(7, 24);
            this.chkNoOutTrans.Name = "chkNoOutTrans";
            this.chkNoOutTrans.Size = new System.Drawing.Size(109, 17);
            this.chkNoOutTrans.TabIndex = 2;
            this.chkNoOutTrans.Text = "No Out Transition";
            this.chkNoOutTrans.UseVisualStyleBackColor = true;
            this.chkNoOutTrans.CheckedChanged += new System.EventHandler(this.chkNoOutTrans_CheckedChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 7);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(100, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "In Translation Time:";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.lblActionName);
            this.panel2.Controls.Add(this.comboActionEntry);
            this.panel2.Controls.Add(this.flagsToggle);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Margin = new System.Windows.Forms.Padding(0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(229, 21);
            this.panel2.TabIndex = 37;
            // 
            // lblActionName
            // 
            this.lblActionName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblActionName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblActionName.Location = new System.Drawing.Point(114, 0);
            this.lblActionName.Name = "lblActionName";
            this.lblActionName.Size = new System.Drawing.Size(115, 21);
            this.lblActionName.TabIndex = 2;
            this.lblActionName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // comboActionEntry
            // 
            this.comboActionEntry.Dock = System.Windows.Forms.DockStyle.Left;
            this.comboActionEntry.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboActionEntry.FormattingEnabled = true;
            this.comboActionEntry.Items.AddRange(new object[] {
            "Main",
            "GFX",
            "SFX",
            "Other"});
            this.comboActionEntry.Location = new System.Drawing.Point(60, 0);
            this.comboActionEntry.Name = "comboActionEntry";
            this.comboActionEntry.Size = new System.Drawing.Size(54, 21);
            this.comboActionEntry.TabIndex = 1;
            this.comboActionEntry.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // flagsToggle
            // 
            this.flagsToggle.Cursor = System.Windows.Forms.Cursors.Default;
            this.flagsToggle.Dock = System.Windows.Forms.DockStyle.Left;
            this.flagsToggle.Location = new System.Drawing.Point(0, 0);
            this.flagsToggle.Name = "flagsToggle";
            this.flagsToggle.Size = new System.Drawing.Size(60, 21);
            this.flagsToggle.TabIndex = 0;
            this.flagsToggle.Text = "[+] Flags";
            this.flagsToggle.UseVisualStyleBackColor = true;
            this.flagsToggle.Click += new System.EventHandler(this.flagsToggle_Click);
            // 
            // spltEventMod
            // 
            this.spltEventMod.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.spltEventMod.Location = new System.Drawing.Point(0, 355);
            this.spltEventMod.Name = "spltEventMod";
            this.spltEventMod.Size = new System.Drawing.Size(229, 3);
            this.spltEventMod.TabIndex = 26;
            this.spltEventMod.TabStop = false;
            this.spltEventMod.Visible = false;
            // 
            // eventModifier
            // 
            this.eventModifier.AutoSize = true;
            this.eventModifier.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.eventModifier.Location = new System.Drawing.Point(0, 358);
            this.eventModifier.Name = "eventModifier";
            this.eventModifier.Size = new System.Drawing.Size(229, 257);
            this.eventModifier.TabIndex = 37;
            this.eventModifier.Visible = false;
            // 
            // ScriptPanel
            // 
            this.Controls.Add(this.ActionEditor);
            this.Controls.Add(this.spltEventMod);
            this.Controls.Add(this.eventModifier);
            this.MinimumSize = new System.Drawing.Size(185, 0);
            this.Name = "ScriptPanel";
            this.Size = new System.Drawing.Size(229, 615);
            this.ctxSubActions.ResumeLayout(false);
            this.ActionEditor.ResumeLayout(false);
            this.SubActionFlagsPanel.ResumeLayout(false);
            this.SubActionFlagsPanel.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        public MainControl _mainWindow;

        public ScriptPanel() 
        {
            InitializeComponent();
            SubActionFlagsPanel.Dock = ActionFlagsPanel.Dock = DockStyle.Top;
            scriptEditor2._mainWindow = this;
            //scriptEditor2.label1.Visible = false;
            //scriptEditor2.Offset.Visible = false;
            eventModifier.Completed += eventModifier_Completed;

            //bg.DoWork += b_DoWork;
        }

        //void b_DoWork(object sender, DoWorkEventArgs e)
        //{
        //    RunTime.Run();
        //}

        void eventModifier_Completed(object sender, EventArgs e)
        {
            if (eventModifier._status == DialogResult.OK)
            {
                scriptEditor2.MakeScript();
                _mainWindow.UpdateModel();
            }

            spltEventMod.Visible = eventModifier.Visible = false;
        }

        public bool CloseReferences()
        {
            return true;
        }

        public int _actionIndex = 0, _subActionIndex = 0;
        public void ActionGroupChanged()
        {
            if (_scriptType != ScriptType.Actions)
                return;

            if (comboActionEntry.SelectedIndex == _actionIndex)
                comboBox1_SelectedIndexChanged(null, null);
            else
                comboActionEntry.SelectedIndex = _actionIndex;
        }

        public void SubRoutineChanged()
        {
            if (_scriptType != ScriptType.Subroutines)
                return;

            if (RunTime.CurrentSubRoutine != null)
            {
                scriptEditor1.TargetNode = scriptEditor2.TargetNode = RunTime.CurrentSubRoutine;
                lblActionName.Text = RunTime.CurrentSubRoutine.Name;
            }
        }

        public void SubactionGroupChanged()
        {
            if (_scriptType != ScriptType.Subactions)
                return;

            if (comboActionEntry.SelectedIndex == _subActionIndex)
                comboBox1_SelectedIndexChanged(null, null);
            else
                comboActionEntry.SelectedIndex = _subActionIndex;

            SubActionEntry grp = RunTime.CurrentSubaction;
            if (grp != null)
            {
                inTransTime.Value = grp._inTransTime;
                chkNoOutTrans.Checked = grp._flags.HasFlag(AnimationFlags.NoOutTransition);
                chkTransOutStart.Checked = grp._flags.HasFlag(AnimationFlags.TransitionOutFromStart);
                chkMovesChar.Checked = grp._flags.HasFlag(AnimationFlags.MovesCharacter);
                chkLoop.Checked = grp._flags.HasFlag(AnimationFlags.Loop);
                chkUnk.Checked = grp._flags.HasFlag(AnimationFlags.Unknown);
                chkFixedScale.Checked = grp._flags.HasFlag(AnimationFlags.FixedScale);
                chkFixedRot.Checked = grp._flags.HasFlag(AnimationFlags.FixedRotation);
                chkFixedTrans.Checked = grp._flags.HasFlag(AnimationFlags.FixedTranslation);
                lblActionName.Text = grp.Name;
            }
        }

        /// <summary>
        /// Opens the event editor for the currently selected event.
        /// </summary>
        public void ModifyEvent()
        {
            eventModifier.Setup(_selectedEvent);
            spltEventMod.Visible = eventModifier.Visible = true;
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public MDL0Node TargetModel
        {
            get { return _mainWindow.TargetModel as MDL0Node; }
            set { _mainWindow.TargetModel = value; }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Event SelectedEvent
        {
            get { return _selectedEvent; }
            set { _selectedEvent = value; }
        }
        private Event _selectedEvent;

        public bool _updating = false;

        ScriptType _scriptType = ScriptType.Subactions;
        public ScriptType ScriptType 
        {
            get { return _scriptType; }
            set
            {
                SubActionFlagsPanel.Visible = false;
                ActionFlagsPanel.Visible = false;
                flagsToggle.Text = "[+] Flags";
                comboActionEntry.Items.Clear();
                if ((_scriptType = value) == ScriptType.Subactions)
                {
                    panel2.Visible = true;
                    flagsToggle.Visible = true;
                    comboActionEntry.Visible = true;
                    comboActionEntry.Items.AddRange(new object[] {
                        "Main",
                        "GFX",
                        "SFX",
                        "Other"});
                    comboActionEntry.SelectedIndex = _subActionIndex;
                }
                else if (_scriptType == ScriptType.Actions)
                {
                    panel2.Visible = true;
                    flagsToggle.Visible = true;
                    comboActionEntry.Visible = true;
                    comboActionEntry.Items.AddRange(new object[] {
                        "Entry",
                        "Exit"});
                    comboActionEntry.SelectedIndex = _actionIndex;
                }
                else
                {
                    panel2.Visible = false;
                    flagsToggle.Visible = false;
                    comboActionEntry.Visible = false;
                }
            }
        }
        private void flagsToggle_Click(object sender, EventArgs e)
        {
            if (ScriptType == ScriptType.Subactions)
                if (SubActionFlagsPanel.Visible)
                {
                    SubActionFlagsPanel.Visible = false;
                    flagsToggle.Text = "[+] Flags";
                }
                else
                {
                    SubActionFlagsPanel.Visible = true;
                    flagsToggle.Text = "[-] Flags";
                }
            else
                if (ActionFlagsPanel.Visible)
                {
                    ActionFlagsPanel.Visible = false;
                    flagsToggle.Text = "[+] Flags";
                }
                else
                {
                    ActionFlagsPanel.Visible = true;
                    flagsToggle.Text = "[-] Flags";
                }
        }

        public void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboActionEntry.SelectedIndex != -1)
                if (ScriptType == ScriptType.Subactions)
                {
                    if (RunTime.CurrentSubaction != null)
                    {
                        Script s = RunTime.CurrentSubaction.GetWithType(comboActionEntry.SelectedIndex);
                        scriptEditor1.TargetNode = scriptEditor2.TargetNode = s;
                    }
                    _subActionIndex = comboActionEntry.SelectedIndex;
                }
                else
                {
                    if (RunTime.CurrentAction != null)
                    {
                        Script s = RunTime.CurrentAction.GetWithType(comboActionEntry.SelectedIndex);
                        scriptEditor1.TargetNode = scriptEditor2.TargetNode = s;
                        lblActionName.Text = s.External ? s._externalEntry.Name : "";
                    }
                    _actionIndex = comboActionEntry.SelectedIndex;
                }
        }

        public void UpdateScriptEditor(Script a)
        {
            if (scriptEditor2.Visible && a == scriptEditor2.TargetNode && a._scriptor._eventIndex - 1 < scriptEditor2.EventList.Items.Count)
            {
                scriptEditor2.EventList.SelectedIndices.Clear();
                scriptEditor2.EventList.SelectedIndex = a._scriptor._eventIndex - 1;
            }
        }

        public CheckBox[] _subactionFlags;
        private void chkUnk_CheckedChanged(object sender, EventArgs e)
        {
            if (RunTime.CurrentSubaction != null)
            {
                if (chkUnk.Checked)
                    RunTime.CurrentSubaction._flags |= AnimationFlags.Unknown;
                else
                    RunTime.CurrentSubaction._flags &= ~AnimationFlags.Unknown;
                RunTime.CurrentSubaction.SignalPropertyChange();
            }
        }

        private void chkLoop_CheckedChanged(object sender, EventArgs e)
        {
            if (_updating)
                return;

            if (RunTime.CurrentSubaction != null)
            {
                if (chkLoop.Checked)
                    RunTime.CurrentSubaction._flags |= AnimationFlags.Loop;
                else
                    RunTime.CurrentSubaction._flags &= ~AnimationFlags.Loop;
                RunTime.CurrentSubaction.SignalPropertyChange();
            }
        }

        private void chkFixedTrans_CheckedChanged(object sender, EventArgs e)
        {
            if (RunTime.CurrentSubaction != null)
            {
                if (chkFixedTrans.Checked)
                    RunTime.CurrentSubaction._flags |= AnimationFlags.FixedTranslation;
                else
                    RunTime.CurrentSubaction._flags &= ~AnimationFlags.FixedTranslation;
                RunTime.CurrentSubaction.SignalPropertyChange();
            }
        }

        private void chkFixedRot_CheckedChanged(object sender, EventArgs e)
        {
            if (RunTime.CurrentSubaction != null)
            {
                if (chkFixedRot.Checked)
                    RunTime.CurrentSubaction._flags |= AnimationFlags.FixedRotation;
                else
                    RunTime.CurrentSubaction._flags &= ~AnimationFlags.FixedRotation;
                RunTime.CurrentSubaction.SignalPropertyChange();
            }
        }

        private void chkFixedScale_CheckedChanged(object sender, EventArgs e)
        {
            if (RunTime.CurrentSubaction != null)
            {
                if (chkFixedScale.Checked)
                    RunTime.CurrentSubaction._flags |= AnimationFlags.FixedScale;
                else
                    RunTime.CurrentSubaction._flags &= ~AnimationFlags.FixedScale;
                RunTime.CurrentSubaction.SignalPropertyChange();
            }
        }

        private void chkMovesChar_CheckedChanged(object sender, EventArgs e)
        {
            if (RunTime.CurrentSubaction != null)
            {
                if (chkMovesChar.Checked)
                    RunTime.CurrentSubaction._flags |= AnimationFlags.MovesCharacter;
                else
                    RunTime.CurrentSubaction._flags &= ~AnimationFlags.MovesCharacter;
                RunTime.CurrentSubaction.SignalPropertyChange();
            }
        }

        private void chkTransOutStart_CheckedChanged(object sender, EventArgs e)
        {
            if (RunTime.CurrentSubaction != null)
            {
                if (chkTransOutStart.Checked)
                    RunTime.CurrentSubaction._flags |= AnimationFlags.TransitionOutFromStart;
                else
                    RunTime.CurrentSubaction._flags &= ~AnimationFlags.TransitionOutFromStart;
                RunTime.CurrentSubaction.SignalPropertyChange();
            }
        }

        private void chkNoOutTrans_CheckedChanged(object sender, EventArgs e)
        {
            if (RunTime.CurrentSubaction != null)
            {
                if (chkNoOutTrans.Checked)
                    RunTime.CurrentSubaction._flags |= AnimationFlags.NoOutTransition;
                else
                    RunTime.CurrentSubaction._flags &= ~AnimationFlags.NoOutTransition;
                RunTime.CurrentSubaction.SignalPropertyChange();
            }
        }

        private void inTransTime_ValueChanged(object sender, EventArgs e)
        {
            if (RunTime.CurrentSubaction != null)
            {
                RunTime.CurrentSubaction._inTransTime = (byte)inTransTime.Value;
                RunTime.CurrentSubaction.SignalPropertyChange();
            }
        }

        private void renameToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            scriptEditor1.Visible = !scriptEditor1.Visible;
            scriptEditor2.Visible = !scriptEditor2.Visible;
            if (scriptEditor1.Visible)
                scriptEditor1.MakeScript();
            else
                scriptEditor2.MakeScript();
        }
    }
}
