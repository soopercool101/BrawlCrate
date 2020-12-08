using BrawlLib.Internal;
using BrawlLib.SSBB.Types.Subspace.Triggers;
using System;
using System.ComponentModel;

namespace BrawlLib.SSBB.ResourceNodes
{
    public class GET1Node : BLOCEntryNode
    {
        protected override Type SubEntryType => typeof(GET1EntryNode);
        protected override string baseName => "Area Triggers";

        internal static ResourceNode TryParse(DataSource source, ResourceNode parent)
        {
            return source.Tag == "GET1" ? new GET1Node() : null;
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
        [TypeConverter(typeof(HexUIntConverter))]
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
        [TypeConverter(typeof(HexUIntConverter))]
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
                _name = $"Area [{Index}]";
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