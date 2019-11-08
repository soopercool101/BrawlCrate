using BrawlLib.Internal;
using BrawlLib.SSBB.Types.Subspace.Triggers;
using System.ComponentModel;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class GET1Node : ResourceNode
    {
        protected internal GET1* Header => (GET1*) WorkingUncompressed.Address;

        public override bool OnInitialize()
        {
            if (_name == null)
            {
                _name = "Area Triggers";
            }

            return Header->_entryCount > 0;
        }

        public override void OnPopulate()
        {
            for (int i = 0; i < Header->_entryCount; i++)
            {
                DataSource source;
                if (i == Header->_entryCount - 1)
                {
                    source = new DataSource((*Header)[i],
                        WorkingUncompressed.Address + WorkingUncompressed.Length - (*Header)[i]);
                }
                else
                {
                    source = new DataSource((*Header)[i], (*Header)[i + 1] - (*Header)[i]);
                }

                new GET1EntryNode().Initialize(this, source);
            }
        }

        public override int OnCalculateSize(bool force)
        {
            return GET1.SIZE + Children.Count * 4 + Children.Count * GET1Entry.SIZE;
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            GET1* header = (GET1*) address;
            *header = new GET1();
            header->_tag = GET1.TAG;
            header->_entryCount = Children.Count;

            uint offset = (uint) (0x08 + Children.Count * 4);
            for (int i = 0; i < Children.Count; i++)
            {
                if (i > 0)
                {
                    offset += (uint) Children[i - 1].CalculateSize(false);
                }

                *(buint*) (address + 0x08 + i * 4) = offset;
                _children[i].Rebuild(address + offset, _children[i].CalculateSize(false), true);
            }
        }

        internal static ResourceNode TryParse(DataSource source)
        {
            return ((GET1*) source.Address)->_tag == GET1.TAG ? new GET1Node() : null;
        }
    }

    public unsafe class GET1EntryNode : ResourceNode
    {
        protected internal GET1Entry* Entry => (GET1Entry*) WorkingUncompressed.Address;

        [Category("General")]
        [DisplayName("Activation Coord 1")]
        public Vector2 Point1
        {
            get => _p1;
            set
            {
                _p1 = value;
                SignalPropertyChange();
            }
        }

        private Vector2 _p1 = new Vector2();

        [Category("General")]
        [DisplayName("Activation Coord 2")]
        public Vector2 Point2
        {
            get => _p2;
            set
            {
                _p2 = value;
                SignalPropertyChange();
            }
        }

        private Vector2 _p2 = new Vector2();

        [Category("Triggers")]
        [DisplayName("Trigger1")]
        //[TypeConverter(typeof(UInt32HexTypeConverter))]
        public uint Trigger
        {
            get => _trigger1;
            set
            {
                _trigger1 = value;
                SignalPropertyChange();
            }
        }

        private uint _trigger1 = 0x0;

        [Category("Triggers")]
        [DisplayName("Trigger2")]
        //[TypeConverter(typeof(UInt32HexTypeConverter))]
        public uint Trigger2
        {
            get => _trigger2;
            set
            {
                _trigger2 = value;
                SignalPropertyChange();
            }
        }

        private uint _trigger2 = 0x0;

        public override bool OnInitialize()
        {
            if (_name == null)
            {
                _name = $"Area[{Index}]";
            }

            _trigger1 = Entry->_trigger1;
            _p1 = new Vector2(Entry->_x1, Entry->_y1);
            _p2 = new Vector2(Entry->_x2, Entry->_y2);
            _trigger2 = Entry->_trigger2;

            return false;
        }

        public override int OnCalculateSize(bool force)
        {
            return GET1Entry.SIZE;
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            GET1Entry* header = (GET1Entry*) address;
            *header = new GET1Entry();
            header->_trigger1 = Trigger;
            header->_x1 = Point1._x;
            header->_y1 = Point1._y;
            header->_x2 = Point2._x;
            header->_y2 = Point2._y;
            header->_trigger2 = Trigger2;
        }
    }
}