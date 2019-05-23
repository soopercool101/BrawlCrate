using System;
using System.Collections.Generic;
using BrawlLib.SSBBTypes;
using System.ComponentModel;

namespace Ikarus.MovesetFile
{
    public unsafe class CollisionData : ListOffset
    {
        public List<CollisionDataEntry> _entries;
        protected override void OnParse(VoidPtr address)
        {
            base.OnParse(address);
            _entries = new List<CollisionDataEntry>();
            if (StartOffset > 0)
            {
                bint* addr = (bint*)(BaseAddress + StartOffset);
                for (int i = 0; i < Count; i++)
                    if (addr[i] > 0)
                        _entries.Add(Parse<CollisionDataEntry>(addr[i]));
            }
        }
        protected override int OnGetLookupCount()
        {
            int count = 0;
            if (_entries.Count > 0)
            {
                count++; //offset to collision entries
                foreach (var o in _entries)
                    count += o.GetLookupCount();
            }
            return base.OnGetLookupCount();
        }

        protected override void OnWrite(VoidPtr address)
        {
            bint* offsets = (bint*)address;
            VoidPtr dataAddr = address;
            if (_entries.Count > 0)
            {
                foreach (CollisionDataEntry o in _entries)
                    if (!o.External)
                    {
                        o.Write(dataAddr);
                        Lookup(o.LookupAddresses);
                        dataAddr += o._calcSize;
                    }
                offsets = (bint*)dataAddr;
                foreach (CollisionDataEntry o in _entries)
                {
                    Lookup(&offsets);
                    *offsets++ = o.RebuildOffset;
                }
            }

            RebuildAddress = offsets;
            sListOffset* header = (sListOffset*)offsets;

            header->_listCount = _entries.Count;
            if (_entries.Count > 0)
            {
                header->_startOffset = Offset(dataAddr);
                Lookup(header->_startOffset.Address);
            }
        }

        protected override int OnGetSize()
        {
            _entryLength = 8;
            _childLength = 0;
            if (_entries.Count > 0)
                foreach (var o in _entries)
                    _childLength += 4 + o._bones.Count * 4;
            return _childLength + _entryLength;
        }
    }

    public enum CollisionType : int
    {
        Type0,
        Type1,
        Type2
    }

    public unsafe class CollisionDataEntry : MovesetEntryNode
    {
        public List<BoneIndexValue> _bones;
        public int _dataOffset, _count, _flags;
        public CollisionType _type;
        public float _length, _width, _height, _unknown;

        [Category("Collision Data")]
        public CollisionType Type { get { return _type; } }
        [Category("Collision Data")]
        public float Length { get { return _length; } set { _length = value; SignalPropertyChange(); } }
        [Category("Collision Data")]
        public float Width { get { return _width; } set { _width = value; SignalPropertyChange(); } }
        [Category("Collision Data")]
        public float Height { get { return _height; } set { _height = value; SignalPropertyChange(); } }
        [Category("Collision Data")]
        public int Flags { get { return _flags; } set { _flags = value; SignalPropertyChange(); } }

        protected override void OnParse(VoidPtr address)
        {
            _bones = new List<BoneIndexValue>();
            _type = *(CollisionType*)address;
            switch (_type)
            {
                case CollisionType.Type0:

                    sCollData0* hdr1 = (sCollData0*)address;
                    _dataOffset = hdr1->_list._startOffset;
                    _count = hdr1->_list._listCount;
                    _length = hdr1->unk1;
                    _width = hdr1->unk2;
                    _height = hdr1->unk3;

                    for (int i = 0; i < _count; i++)
                        _bones.Add(Parse<BoneIndexValue>(_dataOffset + i * 4));

                    break;

                case CollisionType.Type1:

                    sCollData1* hdr2 = (sCollData1*)address;
                    _length = hdr2->unk1;
                    _width = hdr2->unk2;
                    _height = hdr2->unk3;

                    break;

                case CollisionType.Type2:

                    sCollData2* hdr3 = (sCollData2*)address;
                    _flags = hdr3->flags;
                    _length = hdr3->unk1;
                    _width = hdr3->unk2;
                    _height = hdr3->unk3;

                    if ((_flags & 2) == 2)
                        _unknown = hdr3->unk4;

                    if (_initSize != 24 && _initSize != 20)
                        throw new Exception("Incorrect size");

                    break;
            }
        }

        protected override int OnGetLookupCount()
        {
            switch (_type)
            {
                case CollisionType.Type0:
                    return (_bones.Count > 0 ? 1 : 0);
                case CollisionType.Type1:
                case CollisionType.Type2:
                    return 0;
            }
            throw new Exception("Unsupported collision type");
        }

        protected override int OnGetSize()
        {
            switch (_type)
            {
                case CollisionType.Type0: return 24 + _bones.Count * 4;
                case CollisionType.Type1: return 16;
                case CollisionType.Type2: return ((_flags & 2) == 2 ? 24 : 20);
            }
            throw new Exception("Unsupported collision type");
        }

        protected override void OnWrite(VoidPtr address)
        {
            *(CollisionType*)address = _type;
            switch (_type)
            {
                case CollisionType.Type0:

                    bint* addr = (bint*)address;
                    foreach (BoneIndexValue b in _bones)
                        b.Write(addr++);

                    RebuildAddress = addr;

                    sCollData0* data1 = (sCollData0*)addr;
                    data1->unk1 = _length;
                    data1->unk2 = _width;
                    data1->unk3 = _height;

                    if (_bones.Count > 0)
                    {
                        data1->_list._startOffset = Offset(address);
                        Lookup(&data1->_list._startOffset);
                    }
                    data1->_list._listCount = _bones.Count;

                    
                    break;

                case CollisionType.Type1:

                    RebuildAddress = address;

                    sCollData1* data2 = (sCollData1*)address;
                    data2->unk1 = _length;
                    data2->unk2 = _width;
                    data2->unk3 = _height;

                    break;

                case CollisionType.Type2:

                    RebuildAddress = address;

                    sCollData2* data3 = (sCollData2*)address;
                    data3->flags = _flags;
                    data3->unk1 = _length;
                    data3->unk2 = _width;
                    data3->unk3 = _height;

                    if ((_flags & 2) == 2)
                        data3->unk4 = _unknown;

                    break;
            }
        }
    }
}
