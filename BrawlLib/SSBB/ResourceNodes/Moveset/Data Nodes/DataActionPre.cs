using BrawlLib.Internal;
using System.ComponentModel;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class MoveDefActionPreNode : MoveDefEntryNode
    {
        internal bint* Start => (bint*) WorkingUncompressed.Address;
        internal int Count;

        public MoveDefActionPreNode(int count)
        {
            Count = count;
        }

        public override bool OnInitialize()
        {
            _extOverride = true;
            base.OnInitialize();
            _name = "Action Pre";
            return Count > 0;
        }

        public override void OnPopulate()
        {
            bint* entry = Start;
            for (int i = 0; i < Count; i++)
            {
                new MoveDefActionPreEntryNode().Initialize(this, new DataSource((VoidPtr) entry++, 0x4));
            }
        }

        public override int OnCalculateSize(bool force)
        {
            _lookupCount = 0;
            return Children.Count * 4;
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            _entryOffset = address;
            bint* addr = (bint*) address;
            foreach (MoveDefActionPreEntryNode b in Children)
            {
                b._entryOffset = addr;
                *addr++ = b.External ? -1 : 0;
            }
        }
    }

    public unsafe class MoveDefActionPreEntryNode : MoveDefEntryNode
    {
        internal bint* Header => (bint*) WorkingUncompressed.Address;
        public override ResourceType ResourceFileType => ResourceType.Unknown;

        internal int i;

        [Category("Action Pre")] public int Value => i;

        [Category("Action Pre")]
        [Browsable(true)]
        [TypeConverter(typeof(DropDownListExtNodesMDef))]
        public string ExternalNode
        {
            get => _extNode != null ? _extNode.Name : null;
            set
            {
                if (_extNode != null)
                {
                    if (_extNode.Name != value)
                    {
                        _extNode._refs.Remove(this);
                    }
                }

                foreach (MoveDefExternalNode e in Root._externalRefs)
                {
                    if (e.Name == value)
                    {
                        _extNode = e;
                        e._refs.Add(this);
                        Name = e.Name;
                    }
                }

                if (_extNode == null)
                {
                    Name = "Action" + Index;
                }
            }
        }

        public override bool OnInitialize()
        {
            base.OnInitialize();
            if (_name == null)
            {
                _name = "Action" + Index;
            }

            i = *Header;
            return false;
        }

        //-1 if external, 0 if none.
        //offsets to the next ref of the same name until -1. Last ref is always -1

        public override int OnCalculateSize(bool force)
        {
            _lookupCount = 0;
            return 4;
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            _entryOffset = address;
            *(bint*) address = i;
        }
    }
}