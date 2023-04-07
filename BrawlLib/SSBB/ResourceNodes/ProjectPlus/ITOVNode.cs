using BrawlLib.Internal;
using BrawlLib.SSBB.Types.ProjectPlus;
using System.ComponentModel;

namespace BrawlLib.SSBB.ResourceNodes.ProjectPlus
{
    public unsafe class ITOVNode : ARCEntryNode
    {
        public ITOV Data;

        [Category("ITOV")]
        public string CommonItemOverride
        {
            get => Data.CommonOverride;
            set
            {
                Data.CommonOverride = value;
                SignalPropertyChange();
            }
        }

        [Category("ITOV")]
        public string PokeAssistOverride
        {
            get => Data.PokeAssistOverride;
            set
            {
                Data.PokeAssistOverride = value;
                SignalPropertyChange();
            }
        }

        public override bool OnInitialize()
        {
            Data = *(ITOV*)WorkingUncompressed.Address;
            return false;
        }

        public override int OnCalculateSize(bool force)
        {
            return ITOV.Size;
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            *(ITOV*) address = Data;
        }

        internal static ResourceNode TryParse(DataSource source, ResourceNode parent)
        {
            return source.Tag == ITOV.Tag ? new ITOVNode() : null;
        }
    }
}
