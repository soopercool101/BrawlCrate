using BrawlLib.SSBBTypes;
using System;
using System.Collections.Generic;

namespace Ikarus.MovesetFile
{
    public unsafe class MiscSoundData : ListOffset
    {
        public List<EntryList<IndexValue>> _entries;
        protected override void OnParse(VoidPtr address)
        {
            base.OnParse(address);
            _entries = new List<EntryList<IndexValue>>();
            sListOffset* offsets = (sListOffset*)Address(StartOffset);
            for (int i = 0; i < Count; i++, offsets++)
                _entries.Add(Parse<EntryList<IndexValue>>(offsets->_startOffset, 4, (int)offsets->_listCount));
        }

        protected override int OnGetLookupCount()
        {
            //Add offset to list offsets array
            int count = (_entries.Count > 0 ? 1 : 0);

            //Add offsets from lists offsets to value arrays
            foreach (EntryList<IndexValue> r in _entries)
                count += (r.Count > 0 ? 1 : 0);

            return count;
        }

        protected override int OnGetSize()
        {
            int size = sListOffset.Size;
            foreach (EntryList<IndexValue> r in _entries)
                size += sListOffset.Size + r.Count * 4;
            return size;
        }

        protected override void OnWrite(VoidPtr address)
        {
            int sndOff = 0, mainOff = 0;
            foreach (EntryList<IndexValue> r in _entries)
            {
                mainOff += 8;
                sndOff += r.Count * 4;
            }

            //indices
            //sound list offsets
            //header

            bint* indices = (bint*)address;
            sListOffset* sndLists = (sListOffset*)(address + sndOff);
            sListOffset* header = (sListOffset*)((VoidPtr)sndLists + mainOff);

            RebuildAddress = header;

            if (_entries.Count > 0)
            {
                header->_startOffset = Offset(sndLists);
                Lookup(&header->_startOffset);
            }

            header->_listCount = _entries.Count;

            foreach (EntryList<IndexValue> r in _entries)
            {
                if (r.Count > 0)
                {
                    sndLists->_startOffset = Offset(indices);
                    Lookup(&sndLists->_startOffset);
                }

                (sndLists++)->_listCount = r.Count;
                foreach (IndexValue b in r)
                {
                    b.RebuildAddress = indices;
                    *indices++ = (int)b;
                }
            }
        }
    }
}
