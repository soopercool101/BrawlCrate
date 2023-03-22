using BrawlLib.Internal;
using BrawlLib.SSBB.Types.Subspace.Navigation;
using System.ComponentModel;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class ADSJNode : ARCEntryNode
    {
        internal ADSJ* Header => (ADSJ*) WorkingUncompressed.Address;
        public override ResourceType ResourceFileType => ResourceType.ADSJ;

        public override void OnPopulate()
        {
            for (int i = 0; i < Header->_count; i++)
            {
                DataSource source;
                if (i == Header->_count - 1)
                {
                    source = new DataSource((*Header)[i],
                        WorkingUncompressed.Address + WorkingUncompressed.Length - (*Header)[i]);
                }
                else
                {
                    source = new DataSource((*Header)[i], (*Header)[i + 1] - (*Header)[i]);
                }

                new ADSJEntryNode().Initialize(this, source);
            }
        }
        protected override string GetName()
        {
            return base.GetName("Stepjumps");
        }

        public override bool OnInitialize()
        {
            return Header->_count > 0;
        }

        public override int OnCalculateSize(bool force)
        {
            int size = ADSJ.Size + Children.Count * 4;
            foreach (ResourceNode node in Children)
            {
                size += node.CalculateSize(force);
            }

            return size;
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            ADSJ* header = (ADSJ*) address;
            *header = new ADSJ(_children.Count);
            uint offset = (uint) (0x10 + Children.Count * 4);
            for (int i = 0; i < Children.Count; i++)
            {
                if (i > 0)
                {
                    offset += (uint) Children[i - 1].CalculateSize(false);
                }

                *(buint*) (address + 0x10 + i * 4) = offset;
                _children[i].Rebuild(address + offset, _children[i].CalculateSize(false), true);
            }
        }

        internal static ResourceNode TryParse(DataSource source, ResourceNode parent)
        {
            return ((ADSJ*) source.Address)->_tag == ADSJ.Tag ? new ADSJNode() : null;
        }
    }

    public unsafe class ADSJEntryNode : ResourceNode
    {
        internal ADSJEntry Data;
        public override ResourceType ResourceFileType => ResourceType.Unknown;

        public override int MaxNameLength => 0x1F;
        [Browsable(false)]
        public override string Name
        {
            get => string.IsNullOrEmpty(JumpBone) ? "<null>" : JumpBone;
            set
            {
                if (JumpBone == value)
                {
                    return;
                }

                JumpBone = value;
            }
        }

        [Category("Jump Info")]
        [DisplayName("Corrosponding GDOR")]
        public string DoorID
        {
            get => Data.DoorID;
            set
            {
                Data.DoorID = value;
                SignalPropertyChange();
            }
        }

        [Category("Jump Info")]
        [DisplayName("File ID (hex)")]
        public string SendingID
        {
            get => Data.SendStage;
            set
            {
                Data.SendStage = value;
                SignalPropertyChange();
            }
        }
        
        [Category("Jump Info")]
        [DisplayName("Jump Bone")]
        public string JumpBone
        {
            get => Data.JumpBone;
            set
            {
                Data.JumpBone = value;
                SignalPropertyChange();
                OnRenamed();
            }
        }
        
        [Category("Jump Flags")]
        public byte Flag0
        {
            get => Data._unk0;
            set
            {
                Data._unk0 = value;
                SignalPropertyChange();
            }
        }
        
        [Category("Jump Flags")]
        public byte Flag1
        {
            get => Data._unk1;
            set
            {
                Data._unk1 = value;
                SignalPropertyChange();
            }
        }

        [Category("Jump Flags")]
        public byte Flag2
        {
            get => Data._unk2;
            set
            {
                Data._unk2 = value;
                SignalPropertyChange();
            }
        }

        [Category("Jump Flags")]
        public sbyte Flag3
        {
            get => Data._unk3;
            set
            {
                Data._unk3 = value;
                SignalPropertyChange();
            }
        }

        public override bool OnInitialize()
        {
            base.OnInitialize();
            Data = *(ADSJEntry*) WorkingUncompressed.Address;
            return false;
        }

        public override int OnCalculateSize(bool force)
        {
            return ADSJEntry.Size;
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            ADSJEntry* header = (ADSJEntry*) address;
            *header = Data;
        }
    }
}