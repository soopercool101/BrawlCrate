using System.ComponentModel;
using BrawlLib.SSBB.ResourceNodes;
using System.Collections.Generic;

namespace System.Windows.Forms
{
    public partial class ModelEditControl : ModelEditorBase
    {
        public List<CollisionNode> _collisions = new List<CollisionNode>();
        private CollisionNode _targetCollision;

        private bool _syncTexToObj;
        public bool _maximize, _savePosition, _hideMainWindow;
        private bool _snapToCollisions;
        private bool _renderCollisions = true;

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool SnapBonesToCollisions
        {
            get { return _snapToCollisions; }
            set { _snapToCollisions = value; }
        }
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool RenderCollisions
        {
            get { return _renderCollisions; }
            set { _renderCollisions = value; OnRenderCollisionsChanged(); }
        }
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public CollisionNode TargetCollision
        {
            get { return _targetCollision; }
            set { _targetCollision = value; }
        }
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override BonesPanel BonesPanel { get { return rightPanel.pnlBones; } }
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override KeyframePanel KeyframePanel { get { return rightPanel.pnlKeyframes; } }
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override int CurrentFrame
        {
            get { return base.CurrentFrame; }
            set
            {
                base.CurrentFrame = value;
                HandleFirstPersonCamera();
            }
        }
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool SyncTexturesToObjectList
        {
            get { return _syncTexToObj; }
            set { _syncTexToObj = value; leftPanel.UpdateTextures(); }
        }
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override bool SyncVIS0
        {
            get { return leftPanel.chkSyncVis.Checked; }
            set { leftPanel.chkSyncVis.Checked = value; }
        }
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override bool EditingAll
        {
            get { return base.EditingAll; }
            set { chkEditAll.Checked = base.EditingAll = value; }
        }
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override string ScreenCaptureFolder
        {
            get { return ScreenCapBgLocText.Text; }
            set { ScreenCapBgLocText.Text = value; }
        }
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override bool InterpolationFormOpen
        {
            get { return interpolationEditorToolStripMenuItem.Checked; }
            set { interpolationEditorToolStripMenuItem.Checked = value; }
        }
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override TransformType ControlType
        {
            get { return (TransformType)cboToolSelect.SelectedIndex; }
            set
            {
                if ((TransformType)cboToolSelect.SelectedIndex == value)
                    return;

                cboToolSelect.SelectedIndex = (int)value;
            }
        }
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override NW4RAnimType TargetAnimType
        {
            get { return _targetAnimType; }
            set
            {
                if (_targetAnimType == value)
                {
                    SetCurrentControl();
                    return;
                }

                int frame = CurrentFrame;
                _targetAnimType = value;
                leftPanel.TargetAnimType = TargetAnimType;
                SetCurrentControl();
                SetFrame(frame);
            }
        }
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override bool PlayCHR0 { get { return playCHR0ToolStripMenuItem.Checked; } set { playCHR0ToolStripMenuItem.Checked = value; } }
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override bool PlaySRT0 { get { return playSRT0ToolStripMenuItem.Checked; } set { playSRT0ToolStripMenuItem.Checked = value; } }
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override bool PlayPAT0 { get { return playPAT0ToolStripMenuItem.Checked; } set { playPAT0ToolStripMenuItem.Checked = value; } }
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override bool PlayVIS0 { get { return playVIS0ToolStripMenuItem.Checked; } set { playCHR0ToolStripMenuItem.Checked = value; } }
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override bool PlayCLR0 { get { return playCLR0ToolStripMenuItem.Checked; } set { playCLR0ToolStripMenuItem.Checked = value; } }
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override bool PlaySCN0 { get { return playSCN0ToolStripMenuItem.Checked; } set { playSCN0ToolStripMenuItem.Checked = value; } }
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override bool PlaySHP0 { get { return playSHP0ToolStripMenuItem.Checked; } set { playSHP0ToolStripMenuItem.Checked = value; } }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override ModelPlaybackPanel PlaybackPanel { get { return pnlPlayback; } }
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override ModelPanel ModelPanel { get { return _viewerForm == null ? modelPanel : _viewerForm.modelPanel1; } }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override CHR0Editor CHR0Editor { get { return chr0Editor; } }
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override SRT0Editor SRT0Editor { get { return srt0Editor; } }
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override SHP0Editor SHP0Editor { get { return shp0Editor; } }
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override VIS0Editor VIS0Editor { get { return vis0Editor; } }
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override PAT0Editor PAT0Editor { get { return pat0Editor; } }
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override SCN0Editor SCN0Editor { get { return scn0Editor; } }
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override CLR0Editor CLR0Editor { get { return clr0Editor; } }

        public override ColorDialog ColorDialog { get { return dlgColor; } }
    }
}