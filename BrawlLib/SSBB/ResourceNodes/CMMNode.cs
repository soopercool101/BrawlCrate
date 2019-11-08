using BrawlLib.Internal;
using BrawlLib.SSBB.Types;
using System;
using System.ComponentModel;
using System.Linq;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class CMMNode : ResourceNode
    {
        internal VoidPtr Header => WorkingUncompressed.Address;
        public override Type[] AllowedChildTypes => new[] {typeof(CMMEntryNode)};
        public override ResourceType ResourceFileType => ResourceType.CMM;

        [Category("Custom My Music")]
        public string TracklistID
        {
            get => "0x" + _tracklistID.ToString("X2");
            set
            {
                string field0 = (value ?? "").Split(' ')[0];
                int fromBase = field0.StartsWith("0x", StringComparison.InvariantCultureIgnoreCase) ? 16 : 10;
                _tracklistID = Convert.ToByte(field0, fromBase);
                SignalPropertyChange();
            }
        }

        public byte _tracklistID;

        public override void OnPopulate()
        {
            for (int i = 0; i < WorkingUncompressed.Length / CMMEntry.Size; i++)
            {
                new CMMEntryNode().Initialize(this, new DataSource(Header[i, 8], 8));
                if (Children.Last() is CMMEntryNode c && c.TrackListID == 0xFF && c._songID == 0 && c.Unknown == 0 &&
                    c.SliderSetting == 0)
                {
                    Children.RemoveAt(Children.Count - 1);
                    break;
                }
            }
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            uint offset = 0x00;
            foreach (ResourceNode r in Children)
            {
                r.Rebuild(address + offset, CMMEntry.Size, true);
                offset += CMMEntry.Size;
            }

            CMMEntryNode end = new CMMEntryNode {TrackListID = 0xFF};
            while (offset < length)
            {
                end.Rebuild(address + offset, CMMEntry.Size, true);
                offset += CMMEntry.Size;
            }
        }

        public override int OnCalculateSize(bool force)
        {
            int size = Children.Count * CMMEntry.Size;

            while (size % 0x20 != 0)
            {
                size += CMMEntry.Size;
            }

            return size;
        }

        public override bool OnInitialize()
        {
            if (WorkingUncompressed.Length > 0x6)
            {
                _tracklistID = Header[1, 6].Byte;
                _name = $"tracklist_{_tracklistID.ToString("X2")}";
            }

            return true;
        }
    }

    public unsafe class CMMEntryNode : ResourceNode
    {
        internal CMMEntry* Header => (CMMEntry*) WorkingUncompressed.Address;
        public override ResourceType ResourceFileType => ResourceType.Unknown;

        public uint _songID;

        [Category("Custom My Music")]
        public string SongID
        {
            get => "0x" + _songID.ToString("X8");
            set
            {
                string field0 = (value ?? "").Split(' ')[0];
                int fromBase = field0.StartsWith("0x", StringComparison.InvariantCultureIgnoreCase) ? 16 : 10;
                _songID = Convert.ToUInt32(field0, fromBase);
                SignalPropertyChange();
                Name = SongID;
            }
        }

        private short _unknown;

        [Category("Custom My Music")]
        public short Unknown
        {
            get => _unknown;
            set
            {
                _unknown = value;
                SignalPropertyChange();
            }
        }

#if !DEBUG
        [Browsable(false)]
#endif
        [Category("Custom My Music")]
        public byte TrackListID { get; set; }

        private byte _sliderSetting;

        [Category("Custom My Music")]
        [Description("Between 0-64, the slider setting to use for My Music")]
        public byte SliderSetting
        {
            get => _sliderSetting;
            set
            {
                _sliderSetting = value;
                SignalPropertyChange();
            }
        }


        public override int OnCalculateSize(bool force)
        {
            return CMMEntry.Size;
        }

        public override bool OnInitialize()
        {
            _songID = Header->_songID;
            _unknown = Header->_unknown;
            // Get the Tracklist ID here, but only set it from the parent entry
            TrackListID = Header->_trackListID;
            _sliderSetting = Header->_sliderSetting;

            _name = SongID;

            return false;
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            CMMEntry* hdr = (CMMEntry*) address;
            hdr->_songID = _songID;
            hdr->_unknown = Unknown;
            hdr->_trackListID = Parent is CMMNode c ? c._tracklistID : TrackListID;
            hdr->_sliderSetting = SliderSetting;
        }
    }
}