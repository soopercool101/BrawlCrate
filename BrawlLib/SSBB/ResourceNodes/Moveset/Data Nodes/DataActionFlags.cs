using BrawlLib.Internal;
using BrawlLib.SSBB.Types;
using System.Collections.Generic;
using System.ComponentModel;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class MoveDefActionFlagsNode : MoveDefEntryNode
    {
        internal ActionFlags* First => (ActionFlags*) WorkingUncompressed.Address;
        private int Count;

        public MoveDefActionFlagsNode(string name, int count)
        {
            _name = name;
            Count = count;
        }

        public override bool OnInitialize()
        {
            base.OnInitialize();
            return Count > 0;
        }

        public override void OnPopulate()
        {
            ActionFlags* addr = First;
            for (int i = 0; i < Count; i++)
            {
                new MoveDefActionFlagsEntryNode().Initialize(this, addr++, 16);
            }
        }

        public override int OnCalculateSize(bool force)
        {
            _lookupCount = 0;
            return Children.Count * 16;
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            _entryOffset = address;

            ActionFlags* data = (ActionFlags*) address;
            foreach (MoveDefActionFlagsEntryNode e in Children)
            {
                e.Rebuild(data++, 16, true);
            }
        }
    }

    public unsafe class MoveDefActionFlagsEntryNode : MoveDefEntryNode
    {
        internal ActionFlags* Header => (ActionFlags*) WorkingUncompressed.Address;
        public override ResourceType ResourceFileType => ResourceType.Unknown;

        public int flags1, flags2, flags3;
        public uint flags4;

        [Category("Raw Flags Binary")]
        [TypeConverter(typeof(Bin32StringConverter))]
        public Bin32 Flags1b
        {
            get => new Bin32((uint) flags1);
            set
            {
                flags1 = (int) value._data;
                SignalPropertyChange();
            }
        }

        [Category("Raw Flags Binary")]
        [TypeConverter(typeof(Bin32StringConverter))]
        public Bin32 Flags2b
        {
            get => new Bin32((uint) flags2);
            set
            {
                flags2 = (int) value._data;
                SignalPropertyChange();
            }
        }

        [Category("Raw Flags Binary")]
        [TypeConverter(typeof(Bin32StringConverter))]
        public Bin32 Flags3b
        {
            get => new Bin32((uint) flags3);
            set
            {
                flags3 = (int) value._data;
                SignalPropertyChange();
            }
        }

        [Category("Raw Flags Binary")]
        [TypeConverter(typeof(Bin32StringConverter))]
        public Bin32 Flags4b
        {
            get => new Bin32(flags4);
            set
            {
                flags4 = value._data;
                SignalPropertyChange();
            }
        }

        [Category("Raw Flags Int")]
        public int Flags1i
        {
            get => flags1;
            set
            {
                flags1 = value;
                SignalPropertyChange();
            }
        }

        [Category("Raw Flags Int")]
        public int Flags2i
        {
            get => flags2;
            set
            {
                flags2 = value;
                SignalPropertyChange();
            }
        }

        [Category("Raw Flags Int")]
        public int Flags3i
        {
            get => flags3;
            set
            {
                flags3 = value;
                SignalPropertyChange();
            }
        }

        [Category("Raw Flags Int")]
        public int Flags4i
        {
            get => (int) flags4;
            set
            {
                flags4 = (uint) value;
                SignalPropertyChange();
            }
        }

        public override bool OnInitialize()
        {
            base.OnInitialize();
            _name = "Action" + (Index + (Parent.Name == "Action Flags" ? 274 : 0));
            flags1 = Header->_flags1;
            flags2 = Header->_flags2;
            flags3 = Header->_flags3;
            flags4 = Header->_flags4;
            return false;
        }

        public override int OnCalculateSize(bool force)
        {
            _lookupCount = 0;
            return 16;
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            _entryOffset = address;
            ActionFlags* header = (ActionFlags*) address;
            header->_flags1 = flags1;
            header->_flags2 = flags2;
            header->_flags3 = flags3;
            header->_flags4 = flags4;
        }
    }

    public unsafe class MoveDefFlagsNode : MoveDefEntryNode
    {
        internal FDefSubActionFlag* Header => (FDefSubActionFlag*) WorkingUncompressed.Address;

        public override ResourceType ResourceFileType => ResourceType.Unknown;

        //public Dictionary<string, FDefSubActionFlag> Flags { get { return flags; } set { flags = value; } }

        public List<FDefSubActionFlag> FlagsList
        {
            get => _flags;
            set => _flags = value;
        }

        public List<string> NamesList
        {
            get => _names;
            set => _names = value;
        }

        //internal Dictionary<string, FDefSubActionFlag> flags = new Dictionary<string, FDefSubActionFlag>();

        public List<string> _names = new List<string>();
        public List<FDefSubActionFlag> _flags = new List<FDefSubActionFlag>();

        public override bool OnInitialize()
        {
            base.OnInitialize();
            _name = "SubAction Flags";
            for (int i = 0; i < WorkingUncompressed.Length / 8; i++)
            {
                string name = null;

                if (Header[i]._stringOffset > 0)
                {
                    name = new string((sbyte*) BaseAddress + Header[i]._stringOffset);
                }
                else
                {
                    name = "<null>";
                }

                //if (!flags.ContainsKey(name) && Header[i]._stringOffset > 0)
                //    flags.Add(name, Header[i]);

                //These are used for naming the subactions in order.
                _names.Add(name);
                _flags.Add(Header[i]);
            }

            return false;
        }
    }
}