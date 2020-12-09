using BrawlLib.Internal;
using BrawlLib.SSBB.Types.Stage_Tables;
using System;
using System.ComponentModel;

namespace BrawlLib.SSBB.ResourceNodes
{
    public class TBLVNode : TBNode
    {
        public override ResourceType ResourceFileType => ResourceType.TBLV;
        public override Type SubEntryType => typeof(TBLVEntryNode);

        internal static ResourceNode TryParse(DataSource source, ResourceNode parent)
        {
            return source.Tag == "TBLV" ? new TBLVNode() : null;
        }
    }

    public unsafe class TBLVEntryNode : ResourceNode
    {
        public float _height;

        [Category("TBLV Entry")]
        [DisplayName("Lava Height")]
        public float LavaHeight
        {
            get => _height;
            set
            {
                _height = value;
                SignalPropertyChange();
            }
        }

        public float _unk0x4;

        [Category("TBLV Entry")]
        public float Unknown0x4
        {
            get => _unk0x4;
            set
            {
                _unk0x4 = value;
                SignalPropertyChange();
            }
        }

        public float _unk0x8;

        [Category("TBLV Entry")]
        public float Unknown0x8
        {
            get => _unk0x8;
            set
            {
                _unk0x8 = value;
                SignalPropertyChange();
            }
        }


        public override int OnCalculateSize(bool force)
        {
            return TBLVEntry.Size;
        }

        public override bool OnInitialize()
        {
            TBLVEntry* header = (TBLVEntry*)WorkingUncompressed.Address;

            _height = header->_height;
            _unk0x4 = header->_unk0x4;
            _unk0x8 = header->_unk0x8;

            return false;
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            TBLVEntry* hdr = (TBLVEntry*)address;

            hdr->_height = _height;
            hdr->_unk0x4 = _unk0x4;
            hdr->_unk0x8 = _unk0x8;
        }
    }
}