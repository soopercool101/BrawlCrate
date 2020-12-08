using BrawlLib.Internal;
using BrawlLib.SSBB.Types;
using System.ComponentModel;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class SCLANode : ARCEntryNode
    {
        internal SCLA* Header => (SCLA*) WorkingUncompressed.Address;
        public override ResourceType ResourceFileType => ResourceType.SCLA;

        public SCLANode()
        {
        }

        public SCLANode(uint newNodesToCreate)
        {
            for (uint i = 0; i < newNodesToCreate; i++)
            {
                SCLAEntryNode node = new SCLAEntryNode(i);
                AddChild(node);
            }
        }

        public override bool OnInitialize()
        {
            base.OnInitialize();

            return Header->_count > 0;
        }

        private const int _entrySize = 0x54;

        public override void OnPopulate()
        {
            for (int i = 0; i < Header->_count; i++)
            {
                new SCLAEntryNode().Initialize(this, new DataSource((*Header)[i], _entrySize));
            }
        }

        protected override string GetName()
        {
            return base.GetName("Stage Collision Attributes");
        }

        public override int OnCalculateSize(bool force)
        {
            return 0x10 + Children.Count * 4 + Children.Count * _entrySize;
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            SCLA* header = (SCLA*) address;
            *header = new SCLA(Children.Count);
            uint offset = (uint) (0x10 + Children.Count * 4);
            for (int i = 0; i < Children.Count; i++)
            {
                ResourceNode r = Children[i];
                *(buint*) (address + 0x10 + i * 4) = offset;
                r.Rebuild(address + offset, _entrySize, true);
                offset += _entrySize;
            }
        }

        public SCLAEntryNode FindSCLAEntry(uint id)
        {
            if (Children == null || Children.Count == 0)
            {
                return null;
            }

            foreach (SCLAEntryNode se in Children)
            {
                if (se.CollisionMaterialID == id)
                {
                    return se;
                }
            }

            return null;
        }

        // Fill missing SCLA entries with default values
        public void FillSCLA(uint amount)
        {
            bool indexFound = false;
            // Go through and add each index with default values as necessary
            for (uint i = 0; i < amount; i++)
            {
                indexFound = false;
                // Figure out if child already exists
                for (int j = 0; j < Children.Count; j++)
                {
                    if (indexFound == false)
                    {
                        if (((SCLAEntryNode) Children[j]).getSCLAIndex() == i)
                        {
                            indexFound = true;
                        }
                    }
                }

                if (indexFound == false)
                {
                    SCLAEntryNode node = new SCLAEntryNode(i);
                    InsertChild(node, true, (int) i);
                }
            }
        }

        internal static ResourceNode TryParse(DataSource source, ResourceNode parent)
        {
            return ((SCLA*) source.Address)->_tag == SCLA.Tag ? new SCLANode() : null;
        }
    }

    public unsafe class SCLAEntryNode : ResourceNode
    {
        internal SCLAEntry* Header => (SCLAEntry*) WorkingUncompressed.Address;
        public override ResourceType ResourceFileType => ResourceType.Unknown;

        public int getSCLAIndex()
        {
            if (_index > 255)
            {
                return -1;
            }

            return (int) _index;
        }

        [Category("SCLA Entry")]
        public uint CollisionMaterialID
        {
            get => _index;
            set
            {
                _index = value;
                generateSCLAEntryName();
                SignalPropertyChange();
            }
        }

        [Category("SCLA Entry")]
        public float Traction
        {
            get => _unk1;
            set
            {
                _unk1 = value;
                SignalPropertyChange();
            }
        }

        [Category("SCLA Entry")]
        public uint HitDataSet
        {
            get => _unk2;
            set
            {
                _unk2 = value;
                SignalPropertyChange();
            }
        }

        [Category("SCLA Entry")]
        [TypeConverter(typeof(ExpandableObjectCustomConverter))]
        public SCLASubEntryClass WalkRun
        {
            get => _sub1;
            set
            {
                _sub1 = value;
                SignalPropertyChange();
            }
        }

        [Category("SCLA Entry")]
        [TypeConverter(typeof(ExpandableObjectCustomConverter))]
        public SCLASubEntryClass JumpLand
        {
            get => _sub2;
            set
            {
                _sub2 = value;
                SignalPropertyChange();
            }
        }

        [Category("SCLA Entry")]
        [TypeConverter(typeof(ExpandableObjectCustomConverter))]
        public SCLASubEntryClass TumbleLand
        {
            get => _sub3;
            set
            {
                _sub3 = value;
                SignalPropertyChange();
            }
        }

        public uint _index;
        public float _unk1;
        public uint _unk2;
        private SCLASubEntryClass _sub1, _sub2, _sub3;

        // Generate with initial values
        public SCLAEntryNode()
        {
            _index = 0;
            _unk1 = 1;
            _unk2 = 0;
            _sub1 = new SCLASubEntryClass();
            _sub2 = new SCLASubEntryClass();
            _sub3 = new SCLASubEntryClass();

            _sub1._parent =
                _sub2._parent =
                    _sub3._parent = this;

            generateSCLAEntry_name();
        }

        // Generate with initial values from a given index ID
        public SCLAEntryNode(uint newIndex)
        {
            _index = newIndex;
            _unk1 = 1;
            _unk2 = 0;
            _sub1 = new SCLASubEntryClass();
            _sub2 = new SCLASubEntryClass();
            _sub3 = new SCLASubEntryClass();

            _sub1._parent =
                _sub2._parent =
                    _sub3._parent = this;

            generateSCLAEntry_name();
        }

        public override bool OnInitialize()
        {
            //_name = "Entry" + Index;
            _index = Header->_index;
            _unk1 = Header->_unk1;
            _unk2 = Header->_unk2;
            _sub1 = Header->_entry1;
            _sub2 = Header->_entry2;
            _sub3 = Header->_entry3;

            _sub1._parent =
                _sub2._parent =
                    _sub3._parent = this;

            generateSCLAEntry_name();

            return false;
        }

        private void generateSCLAEntry_name()
        {
            if (_index >= 0 && _index <= 255)
            {
                _name = CollisionTerrain.Terrains[_index].Name + " [" + _index + "]";
            }
            else
            {
                _name = "Entry [" + _index + "]";
            }
        }

        private void generateSCLAEntryName()
        {
            if (_index >= 0 && _index <= 255)
            {
                Name = CollisionTerrain.Terrains[_index].Name + " [" + _index + "]";
            }
            else
            {
                Name = "Entry [" + _index + "]";
            }
        }

        public override int OnCalculateSize(bool force)
        {
            return 0x54;
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            SCLAEntry* hdr = (SCLAEntry*) address;
            hdr->_index = _index;
            generateSCLAEntry_name();
            hdr->_unk1 = _unk1;
            hdr->_unk2 = _unk2;
            hdr->_entry1 = _sub1;
            hdr->_entry2 = _sub2;
            hdr->_entry3 = _sub3;
        }

        public class SCLASubEntryClass
        {
            public SCLAEntryNode _parent;

            [Category("SCLA Sub Entry")]
            public byte CreatesDust
            {
                get => _unk1;
                set
                {
                    _unk1 = value;
                    _parent.SignalPropertyChange();
                }
            }

            [Category("SCLA Sub Entry")]
            public byte Unknown2
            {
                get => _unk2;
                set
                {
                    _unk2 = value;
                    _parent.SignalPropertyChange();
                }
            }

            [Category("SCLA Sub Entry")]
            public ushort Unknown3
            {
                get => _unk3;
                set
                {
                    _unk3 = value;
                    _parent.SignalPropertyChange();
                }
            }

            [Category("SCLA Sub Entry")]
            public uint GFXFlag
            {
                get => _unk4;
                set
                {
                    _unk4 = value;
                    _parent.SignalPropertyChange();
                }
            }

            [Category("SCLA Sub Entry")]
            public int SFXFlag
            {
                get => _index1;
                set
                {
                    _index1 = value;
                    _index2 = value;
                    _index3 = value;
                    _index4 = value;
                    _parent.SignalPropertyChange();
                }
            }
#if DEBUG
            [Category("SCLA Sub Entry")]
            public int Index1
            {
                get => _index1;
                set
                {
                    _index1 = value;
                    _parent.SignalPropertyChange();
                }
            }

            [Category("SCLA Sub Entry")]
            public int Index2
            {
                get => _index2;
                set
                {
                    _index2 = value;
                    _parent.SignalPropertyChange();
                }
            }

            [Category("SCLA Sub Entry")]
            public int Index3
            {
                get => _index3;
                set
                {
                    _index3 = value;
                    _parent.SignalPropertyChange();
                }
            }

            [Category("SCLA Sub Entry")]
            public int Index4
            {
                get => _index4;
                set
                {
                    _index4 = value;
                    _parent.SignalPropertyChange();
                }
            }
#endif

            public byte _unk1;
            public byte _unk2;
            public ushort _unk3;
            public uint _unk4;
            public int _index1;
            public int _index2;
            public int _index3;
            public int _index4;

            public SCLASubEntryClass()
            {
                _unk1 = 1;
                _unk2 = 1;
                _unk3 = 0;
                _unk4 = 0;
                _index1 = -1;
                _index2 = -1;
                _index3 = -1;
                _index4 = -1;
            }

            public SCLASubEntryClass(SCLASubEntry e)
            {
                _unk1 = e._unk1;
                _unk2 = e._unk2;
                _unk3 = e._unk3;
                _unk4 = e._unk4;
                _index1 = e._index1;
                _index2 = e._index2;
                _index3 = e._index3;
                _index4 = e._index4;
            }

            public override string ToString()
            {
                // If index 1 - 4 are different, return full string
                if (_index1 != _index2 || _index1 != _index3 || _index1 != _index4)
                {
                    return $"{_unk1} {_unk2} {_unk3} {_unk4} {_index1} {_index2} {_index3} {_index4}";
                }

                // Otherwise only show the first index (All seem to be edited in tandem)
                return $"{_unk1} {_unk2} {_unk3} {_unk4} {_index1}";
            }

            public static implicit operator SCLASubEntry(SCLASubEntryClass val)
            {
                return new SCLASubEntry(val);
            }

            public static implicit operator SCLASubEntryClass(SCLASubEntry val)
            {
                return new SCLASubEntryClass(val);
            }
        }
    }
}