using BrawlLib.SSBBTypes;
using System;
using System.Collections.Generic;

namespace Ikarus.MovesetFile
{
    public unsafe class CmnLegBonesNode : MovesetEntryNode
    {
        List<string> _left, _right;
        protected override void OnParse(VoidPtr address)
        {
            _left = new List<string>();
            _right = new List<string>();

            sListOffset* hdr = (sListOffset*)address;
            bint* addr = (bint*)Address(hdr[0]._startOffset);
            for (int i = 0; i < hdr[0]._listCount; i++)
                _left.Add(new String((sbyte*)(Address(addr[i]))));
            addr = (bint*)Address(hdr[1]._startOffset);
            for (int i = 0; i < hdr[1]._listCount; i++)
                _right.Add(new String((sbyte*)(Address(addr[i]))));
        }

        protected override int OnGetLookupCount()
        {
            return
                (_right.Count > 0 ? _right.Count + 1 : 0) +
                (_left.Count > 0 ? _left.Count + 1 : 0);
        }
        protected override int OnGetSize()
        {
            _entryLength = 0x10; //Header size

            foreach (string s in _left)
                _childLength += (s.Length + 1).Align(4) + 4;
            foreach (string s in _right)
                _childLength += (s.Length + 1).Align(4) + 4;

            return _entryLength + _childLength;
        }
        protected override void OnWrite(VoidPtr address)
        {
            int leftOffset, rightOffset;

            List<int> offsets = new List<int>();
            sbyte* ptr = (sbyte*)address;
            foreach (string s in _left)
            {
                s.Write(ptr);
                offsets.Add(Offset(ptr));
                ptr += (s.Length + 1).Align(4);
            }

            bint* offPtr = (bint*)ptr;
            leftOffset = Offset(offPtr);
            foreach (int i in offsets)
            {
                Lookup(offPtr);
                *offPtr++ = i;
            }

            offsets.Clear();
            ptr = (sbyte*)offPtr;
            foreach (string s in _right)
            {
                s.Write(ptr);
                offsets.Add(Offset(ptr));
                ptr += (s.Length + 1).Align(4);
            }

            offPtr = (bint*)ptr;
            rightOffset = Offset(offPtr);
            foreach (int i in offsets)
            {
                Lookup(offPtr);
                *offPtr++ = i;
            }

            RebuildAddress = offPtr;

            if (_left.Count > 0)
                Lookup(offPtr);
            *offPtr++ = _left.Count > 0 ? leftOffset : 0;
            *offPtr++ = _left.Count;

            if (_right.Count > 0)
                Lookup(offPtr);
            *offPtr++ = _right.Count > 0 ? rightOffset : 0;
            *offPtr++ = _right.Count;
        }
    }
}
