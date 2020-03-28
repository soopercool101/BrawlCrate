using BrawlLib.Internal;
using BrawlLib.SSBB.Types.ProjectPlus;
using System;
using System.ComponentModel;
using System.IO;

namespace BrawlLib.SSBB.ResourceNodes.ProjectPlus
{
    public unsafe class TLSTNode : ResourceNode
    {
        internal TLST* Header => (TLST*) WorkingUncompressed.Address;
        public override ResourceType ResourceFileType => ResourceType.TLST;

        public override Type[] AllowedChildTypes => new[] {typeof(TLSTEntryNode)};

        public override void OnPopulate()
        {
            for (int i = 0; i < Header->_count; i++)
            {
                DataSource source = new DataSource((*Header)[i], (int)TLSTEntry.Size);
                new TLSTEntryNode().Initialize(this, source);
            }
        }

        public override bool OnInitialize()
        {
            if (_name == null && _origPath != null)
            {
                _name = Path.GetFileNameWithoutExtension(_origPath);
            }
            return Header->_count > 0;
        }

        public override int OnCalculateSize(bool force)
        {
            int size = (int)TLST.HeaderSize;
            foreach (TLSTEntryNode n in Children)
            {
                size += (int) TLSTEntry.Size;
                if (!string.IsNullOrEmpty(n._name) && n._name != "<null>")
                {
                    size += n.Name.Length + 1;
                }

                if (!string.IsNullOrEmpty(n.SongFileName))
                {
                    size += n.SongFileName.Length + 1;
                }
            }

            return size;
        }

        internal ushort strOffset;
        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            strOffset = 0;
            TLST* header = (TLST*)address;
            *header = new TLST();
            header->_tag = TLST.Tag;
            header->_count = (uint)Children.Count;
            header->_fileSize = (ushort)length;
            header->_nameOffset = (ushort) (TLST.HeaderSize + Children.Count * TLSTEntry.Size);

            uint offset = TLST.HeaderSize;
            foreach (ResourceNode n in Children)
            {
                int size = n.CalculateSize(false);
                n.Rebuild(address + offset, size, true);
                offset += (uint)size;
            }
            foreach (TLSTEntryNode n in Children)
            {
                if (!string.IsNullOrEmpty(n.SongFileName))
                {
                    sbyte* ptr = (sbyte*)(address + offset);
                    string name = n.SongFileName;
                    for (int j = 0; j < name.Length; j++)
                    {
                        ptr[j] = (sbyte)name[j];
                    }
                    ptr[name.Length] = 0;
                    offset += (uint)(n.SongFileName.Length + 1);
                }
                if (!string.IsNullOrEmpty(n._name) && n._name != "<null>")
                {
                    sbyte* ptr = (sbyte*)(address + offset);
                    string name = n.Name;
                    for (int j = 0; j < name.Length; j++)
                    {
                        ptr[j] = (sbyte)name[j];
                    }
                    ptr[name.Length] = 0;
                    offset += (uint)(n.Name.Length + 1);
                }
            }
        }

        internal static ResourceNode TryParse(DataSource source)
        {
            return source.Tag == "TLST" ? new TLSTNode() : null;
        }
    }

    public unsafe class TLSTEntryNode : ResourceNode
    {
        internal TLSTEntry* Header => (TLSTEntry*)WorkingUncompressed.Address;

        private uint _songID;

        [Category("Song Entry")]
        public string SongID
        {
            get => "0x" + _songID.ToString("X8");
            set
            {
                string field0 = (value ?? "").Split(' ')[0];
                int fromBase = field0.StartsWith("0x", StringComparison.InvariantCultureIgnoreCase) ? 16 : 10;
                _songID = Convert.ToUInt32(field0, fromBase);
                SignalPropertyChange();
            }
        }

        private short _songDelay;

        [Category("Song Entry")]
        public short SongDelay
        {
            get => _songDelay;
            set
            {
                _songDelay = value;
                SignalPropertyChange();
            }
        }

        private byte _songVolume;

        [Category("Song Entry")]
        public byte Volume
        {
            get => _songVolume;
            set
            {
                _songVolume = value;
                SignalPropertyChange();
            }
        }

        private byte _songFrequency;

        [Category("Song Entry")]
        public byte Frequency
        {
            get => _songFrequency;
            set
            {
                _songFrequency = value;
                SignalPropertyChange();
            }
        }

        private string _fileName;

        [Category("Song Entry")]
        public string SongFileName
        {
            get => _fileName;
            set
            {
                _fileName = value;
                SignalPropertyChange();
            }
        }

        private ushort _songSwitch;

        [Category("Song Entry")]
        public ushort SongSwitch
        {
            get => _songSwitch;
            set
            {
                _songSwitch = value;
                SignalPropertyChange();
            }
        }

        private bool _disableStockPinch;

        [Category("Song Entry")]
        public bool DisableStockPinch
        {
            get => _disableStockPinch;
            set
            {
                _disableStockPinch = value;
                SignalPropertyChange();
            }
        }


        private bool _disableTracklistInclusion;

        [Category("Song Entry")]
        public bool HiddenFromTracklist
        {
            get => _disableTracklistInclusion;
            set
            {
                _disableTracklistInclusion = value;
                SignalPropertyChange();
            }
        }

        public override int OnCalculateSize(bool force)
        {
            return (int)TLSTEntry.Size;
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            TLSTEntry* header = (TLSTEntry*)address;
            *header = new TLSTEntry();
            if (string.IsNullOrEmpty(_fileName))
            {
                header->_fileName = 0xFFFF;
            }
            else
            {
                header->_fileName = ((TLSTNode)Parent).strOffset;
                ((TLSTNode)Parent).strOffset += (ushort)(SongFileName.Length + 1);
            }
            if (string.IsNullOrEmpty(_name) || _name == "<null>")
            {
                header->_title = 0xFFFF;
            }
            else
            {
                header->_title = ((TLSTNode)Parent).strOffset;
                ((TLSTNode)Parent).strOffset += (ushort)(Name.Length + 1);
            }

            header->_songID = _songID;
            header->_songDelay = _songDelay;
            header->_songVolume = _songVolume;
            header->_songFrequency = _songFrequency;
            header->_songSwitch = _songSwitch;
            header->_disableStockPinch = (byte)(_disableStockPinch ? 1 : 0);
            header->_disableTracklistInclusion = (byte)(_disableTracklistInclusion ? 1 : 0);
        }

        public override bool OnInitialize()
        {
            if (Header->_title != 0xFFFF)
            {
                _name = new string((sbyte*)((VoidPtr)((TLSTNode)Parent).Header) +
                                   ((TLSTNode)Parent).Header->_nameOffset + Header->_title);
            }

            if (Header->_fileName != 0xFFFF)
            {
                _fileName = new string((sbyte*)((VoidPtr)((TLSTNode)Parent).Header) +
                                   ((TLSTNode)Parent).Header->_nameOffset + Header->_fileName);
            }
            _songID = Header->_songID;
            _songDelay = Header->_songDelay;
            _songVolume = Header->_songVolume;
            _songFrequency = Header->_songFrequency;
            _songSwitch = Header->_songSwitch;
            _disableStockPinch = Header->_disableStockPinch != 0;
            _disableTracklistInclusion = Header->_disableTracklistInclusion != 0;
            return false;
        }
    }
}