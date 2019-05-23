using System.ComponentModel;
using BrawlLib.SSBB.ResourceNodes;
using System.Windows.Forms;
using Ikarus.MovesetFile;

namespace Ikarus.UI
{
    public partial class MainControl : ModelEditorBase
    {
        private DelegateOpenFile m_DelegateOpenFile;

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override TransformType ControlType
        {
            get { return _controlType; }
            set
            {
                if (_controlType == value)
                    return;
                
                _controlType = value;

                _updating = true;
                rotationToolStripMenuItem.Checked = _controlType == TransformType.Rotation;
                translationToolStripMenuItem.Checked = _controlType == TransformType.Rotation;
                scaleToolStripMenuItem.Checked = _controlType == TransformType.Rotation;
                _updating = false;
            }
        }

        TransformType _controlType;

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ResourceNode ExternalAnimationsNode { get { return Manager.Animations; } }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Panel AnimCtrlPnl { get { return panel3; } }
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Panel AnimEditors { get { return animEditors; } }

        public bool
            _renderHurtboxes,
            _renderHitboxes;

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
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), DefaultValue(true)]
        public override bool PlaySCN0 { get { return false; } set { } }
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override bool PlaySHP0 { get { return playSHP0ToolStripMenuItem.Checked; } set { playSHP0ToolStripMenuItem.Checked = value; } }
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override KeyframePanel KeyframePanel { get { return null; } }
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override BonesPanel BonesPanel { get { return modelListsPanel1.bonesPanel1; } }

        public MiscHurtBox _selectedHurtbox;
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public MiscHurtBox SelectedHurtbox
        {
            get { return _selectedHurtbox; }
            set 
            {
                if ((_selectedHurtbox = value) != null)
                {
                    EnableHurtboxEditor();
                    hurtboxEditor.TargetHurtBox = value;
                }
                else
                {
                    DisableHurtboxEditor();
                    hurtboxEditor.TargetHurtBox = null;
                }
            }
        }
    }
}
