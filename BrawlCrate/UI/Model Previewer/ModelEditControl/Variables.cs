using BrawlLib.Internal.Windows.Controls.Model_Panel;
using BrawlLib.Internal.Windows.Controls.ModelViewer.Editors;
using BrawlLib.Internal.Windows.Controls.ModelViewer.MainWindowBase;
using BrawlLib.Internal.Windows.Controls.ModelViewer.Panels;
using BrawlLib.SSBB.ResourceNodes;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;

namespace BrawlCrate.UI.Model_Previewer.ModelEditControl
{
    public partial class ModelEditControl : ModelEditorBase
    {
        public List<CollisionNode> _collisions = new List<CollisionNode>();
        private CollisionNode _targetCollision;

        private bool _syncTexToObj;
        public bool _maximize, _savePosition, _hideMainWindow;
        private bool _snapToCollisions;
        private bool _renderCollisions = true;

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool SnapBonesToCollisions
        {
            get => _snapToCollisions;
            set => _snapToCollisions = value;
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool RenderCollisions
        {
            get => _renderCollisions;
            set
            {
                _renderCollisions = value;
                OnRenderCollisionsChanged();
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public CollisionNode TargetCollision
        {
            get => _targetCollision;
            set => _targetCollision = value;
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override BonesPanel BonesPanel => rightPanel.pnlBones;

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override KeyframePanel KeyframePanel => rightPanel.pnlKeyframes;

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override int CurrentFrame
        {
            get => base.CurrentFrame;
            set
            {
                base.CurrentFrame = value;
                HandleFirstPersonCamera();
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool SyncTexturesToObjectList
        {
            get => _syncTexToObj;
            set
            {
                _syncTexToObj = value;
                leftPanel.UpdateTextures();
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override bool SyncVIS0
        {
            get => leftPanel.chkSyncVis.Checked;
            set => leftPanel.chkSyncVis.Checked = value;
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override bool EditingAll
        {
            get => base.EditingAll;
            set => chkEditAll.Checked = base.EditingAll = value;
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override string ScreenCaptureFolder
        {
            get => ScreenCapBgLocText.Text;
            set => ScreenCapBgLocText.Text = value;
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override bool InterpolationFormOpen
        {
            get => interpolationEditorToolStripMenuItem.Checked;
            set => interpolationEditorToolStripMenuItem.Checked = value;
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override TransformType ControlType
        {
            get => (TransformType) cboToolSelect.SelectedIndex;
            set
            {
                if ((TransformType) cboToolSelect.SelectedIndex == value)
                {
                    return;
                }

                cboToolSelect.SelectedIndex = (int) value;
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override NW4RAnimType TargetAnimType
        {
            get => _targetAnimType;
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

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override bool PlayCHR0
        {
            get => playCHR0ToolStripMenuItem.Checked;
            set => playCHR0ToolStripMenuItem.Checked = value;
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override bool PlaySRT0
        {
            get => playSRT0ToolStripMenuItem.Checked;
            set => playSRT0ToolStripMenuItem.Checked = value;
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override bool PlayPAT0
        {
            get => playPAT0ToolStripMenuItem.Checked;
            set => playPAT0ToolStripMenuItem.Checked = value;
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override bool PlayVIS0
        {
            get => playVIS0ToolStripMenuItem.Checked;
            set => playCHR0ToolStripMenuItem.Checked = value;
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override bool PlayCLR0
        {
            get => playCLR0ToolStripMenuItem.Checked;
            set => playCLR0ToolStripMenuItem.Checked = value;
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override bool PlaySCN0
        {
            get => playSCN0ToolStripMenuItem.Checked;
            set => playSCN0ToolStripMenuItem.Checked = value;
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override bool PlaySHP0
        {
            get => playSHP0ToolStripMenuItem.Checked;
            set => playSHP0ToolStripMenuItem.Checked = value;
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override ModelPlaybackPanel PlaybackPanel => pnlPlayback;

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override ModelPanel ModelPanel => _viewerForm == null ? modelPanel : _viewerForm.modelPanel1;

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override CHR0Editor CHR0Editor => chr0Editor;

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override SRT0Editor SRT0Editor => srt0Editor;

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override SHP0Editor SHP0Editor => shp0Editor;

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override VIS0Editor VIS0Editor => vis0Editor;

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override PAT0Editor PAT0Editor => pat0Editor;

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override SCN0Editor SCN0Editor => scn0Editor;

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override CLR0Editor CLR0Editor => clr0Editor;

        public override ColorDialog ColorDialog => dlgColor;
    }
}