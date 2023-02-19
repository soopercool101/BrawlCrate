using BrawlLib.Internal;
using BrawlLib.SSBB.ResourceNodes;
using BrawlLib.SSBB.Types;
using System;
using System.Linq;

namespace BrawlLib.SSBB
{
    public class MuEventTblNode : ResourceNode
    {
        public override Type[] AllowedChildTypes => new [] { typeof(MuEventNode) };

        public override bool OnInitialize()
        {
            base.OnInitialize();
            return WorkingUncompressed.Length >= MuEvent.Size;
        }

        public override int OnCalculateSize(bool force)
        {
            return MuEvent.Size * Children?.Count ?? 0;
        }

        public override void OnPopulate()
        {
            base.OnPopulate();
            for (int i = 0; i < WorkingUncompressed.Length; i += MuEvent.Size)
            {
                new MuEventNode().Initialize(this, WorkingUncompressed.Address + i, MuEvent.Size);
            }
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            var ptr = address;
            foreach (var child in Children)
            {
                var len = child.OnCalculateSize(force);
                child.OnRebuild(ptr, len, force);
                ptr += len;
            }
        }
    }

    public unsafe class MuEventNode : ResourceNode
    {
        public uint _titleMsgLineIndex;

        public uint TitleMsgLineIndex
        {
            get => _titleMsgLineIndex;
            set
            {
                _titleMsgLineIndex = value;
                SignalPropertyChange();
            }
        }

        public string TitleMsg
        {
            get
            {
                var msg = Parent?.Parent?.Parent?.FindChildrenByName("Misc Data [30]").FirstOrDefault() as MSBinNode;
                if (msg != null && _titleMsgLineIndex < msg._strings.Count)
                {
                    return msg._strings[(int)_titleMsgLineIndex];
                }

                return null;
            }
        }

        public uint _descriptionMsgLineIndex;

        public uint DescriptionMsgLineIndex
        {
            get => _descriptionMsgLineIndex;
            set
            {
                _descriptionMsgLineIndex = value;
                SignalPropertyChange();
            }
        }

        public string DescriptionMsg
        {
            get
            {
                var msg = Parent?.Parent?.Parent?.FindChildrenByName("Misc Data [30]").FirstOrDefault() as MSBinNode;
                if (msg != null && _descriptionMsgLineIndex < msg._strings.Count)
                {
                    return msg._strings[(int)_descriptionMsgLineIndex];
                }

                return null;
            }
        }

        public byte _p1CssId;

        public byte P1CssID
        {
            get => _p1CssId;
            set
            {
                _p1CssId = value;
                SignalPropertyChange();
            }
        }

        public byte _p2CssId;

        public byte P2CssID
        {
            get => _p2CssId;
            set
            {
                _p2CssId = value;
                SignalPropertyChange();
            }
        }

        public byte _scoreType;

        public byte ScoreType
        {
            get => _scoreType;
            set
            {
                _scoreType = value;
                SignalPropertyChange();
            }
        }

        public byte _eventId;

        public byte EventID
        {
            get => _eventId;
            set
            {
                _eventId = value;
                Name = $"Event [{_eventId}]";
                SignalPropertyChange();
            }
        }


        public byte _unknown0x09;
        public byte Unknown0x09
        {
            get => _unknown0x09;
            set
            {
                _unknown0x09 = value;
                SignalPropertyChange();
            }
        }
        public byte _unknown0x13;
        public byte Unknown0x13
        {
            get => _unknown0x13;
            set
            {
                _unknown0x13 = value;
                SignalPropertyChange();
            }
        }
        public byte _unknown0x14;
        public byte Unknown0x14
        {
            get => _unknown0x14;
            set
            {
                _unknown0x14 = value;
                SignalPropertyChange();
            }
        }
        public byte _unknown0x15;
        public byte Unknown0x15
        {
            get => _unknown0x15;
            set
            {
                _unknown0x15 = value;
                SignalPropertyChange();
            }
        }

        public override bool OnInitialize()
        {
            var hdr = (MuEvent*)WorkingUncompressed.Address;
            _titleMsgLineIndex = hdr->_titleMsgLineIndex;
            _descriptionMsgLineIndex = hdr->_descriptionMsgLineIndex;
            _p1CssId = hdr->_p1CssId;
            _unknown0x09 = hdr->_unknown0x09;
            _p2CssId = hdr->_p2CssId;
            _unknown0x13 = hdr->_unknown0x13;
            _scoreType = hdr->_scoreType;
            _eventId = hdr->_eventId;
            _unknown0x14 = hdr->_unknown0x14;
            _unknown0x15 = hdr->_unknown0x15;
            _name = $"Event [{_eventId}]";
            return false;
        }

        public override int OnCalculateSize(bool force)
        {
            return MuEvent.Size;
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            base.OnRebuild(address, length, force);
            MuEvent* hdr = (MuEvent*)address;
            hdr->_titleMsgLineIndex = _titleMsgLineIndex;
            hdr->_descriptionMsgLineIndex = _descriptionMsgLineIndex;
            hdr->_p1CssId = _p1CssId;
            hdr->_unknown0x09 = _unknown0x09;
            hdr->_p2CssId = _p2CssId;
            hdr->_unknown0x13 = _unknown0x13;
            hdr->_scoreType = _scoreType;
            hdr->_eventId = _eventId;
            hdr->_unknown0x14 = _unknown0x14;
            hdr->_unknown0x15 = _unknown0x15;
        }
    }
}
