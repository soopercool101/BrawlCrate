using System;
using BrawlLib.SSBBTypes;
using System.ComponentModel;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class GSNDNode : ResourceNode
    {
        internal GSND* Header { get { return (GSND*)WorkingUncompressed.Address; } }
        public override ResourceType ResourceType { get { return ResourceType.GSND; } }

        [Category("GSND")]
        [DisplayName("Entries")]
        public int count { get { return Header->_count; } }
        public override void OnPopulate()
        {
            for (int i = 0; i < Header->_count; i++)
            {
                DataSource source;
                if (i == Header->_count - 1)
                { source = new DataSource((*Header)[i], WorkingUncompressed.Address + WorkingUncompressed.Length - (*Header)[i]); }
                else { source = new DataSource((*Header)[i], (*Header)[i + 1] - (*Header)[i]); }
                new GSNDEntryNode().Initialize(this, source);
            }
        }
        public override bool OnInitialize()
        {
            base.OnInitialize();
            if (_name == null)
                _name = "Sound Effects";
            return Header->_count > 0;
        }
        public override int OnCalculateSize(bool force)
        {
            int size = 0x08 + (Children.Count * 4);
            foreach (ResourceNode node in Children)
                size += 0x50;
            return size;
        }
        public override void OnRebuild(VoidPtr address, int length, bool force)
        {

            GSND* header = (GSND*)address;
            *header = new GSND(Children.Count);
            uint offset = (uint)(0x08 + (Children.Count * 4));
            for (int i = 0; i < Children.Count; i++)
            {            
                if (i > 0){offset += (uint)(Children[i - 1].CalculateSize(false));}
                *(buint*)((VoidPtr)address + 0x08 + i * 4) = offset;
                _children[i].Rebuild((VoidPtr)address + offset, _children[i].CalculateSize(false), true);
            }
        }
        

        internal static ResourceNode TryParse(DataSource source) { return ((GSND*)source.Address)->_tag == GSND.Tag ? new GSNDNode() : null; }
    }

    public unsafe class GSNDEntryNode : ResourceNode
    {
        internal GSNDEntry* Header { get { return (GSNDEntry*)WorkingUncompressed.Address; } }
        public override ResourceType ResourceType { get { return ResourceType.Unknown; } }

        internal string _Bname;
        [Category("Sound")]
        [DisplayName("Bone Name")]
        public string BName { get { return _Bname; } set {_Bname = value; SignalPropertyChange(); } }

        internal int _infoindex;
        [Category("Sound")]
        [DisplayName("Info Index")]
        public int infoIndex { get { return _infoindex; } set {_infoindex = value; SignalPropertyChange(); } }

        internal float _unkfloat0;
        [Category("Misc")]
        [DisplayName("unknown")]
        public float unkFloat0 { get { return _unkfloat0; } set { _unkfloat0 = value; SignalPropertyChange(); } }

        internal float _unkfloat1;
        [Category("Misc")]
        [DisplayName("unknown")]
        public float unkFloat1 { get { return _unkfloat1; } set { _unkfloat1 = value; SignalPropertyChange(); } }

        internal string _trigger;
        [Category("Misc")]
        [DisplayName("Trigger ID?")]
        public string Trigger { get { return _trigger; } set {_trigger = value; SignalPropertyChange(); } }

        public override bool OnInitialize()
        {
            base.OnInitialize();
            if (_name == null)
                _name = "Sound[" + Index + ']';

            _Bname = Header->Name;
            _infoindex = Header->_infoIndex;
            _unkfloat0 = Header->_unkFloat0;
            _unkfloat1 = Header->_unkFloat1;
            _trigger = Header->Trigger;
            return false;
        }
        public override int OnCalculateSize(bool force)
        {
            return 0x50;
        }
        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            GSNDEntry* header = (GSNDEntry*)address;
            *header = new GSNDEntry();
            header->_infoIndex = _infoindex;
            header->_pad0 = header->_pad1 =
            header->_pad2 = header->Pad4 = 0;
            header->_unkFloat0 = _unkfloat0;
            header->_unkFloat1 = _unkfloat1;
            header->Name = _Bname;
            header->Trigger = _trigger;
            header->unk0 = 0x00000001;
        }
    }
}