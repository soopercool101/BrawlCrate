using System;
using System.Collections.Generic;

namespace BrawlLib.SSBBTypes
{
    public class SakuraiArchiveBuilder
    {
        //Modifyable behavior!
        private const bool AddUnreferencedReferences = true;
        public VoidPtr _baseAddress, _currentAddress;
        public int _lookupCount, _lookupLen;

        //The lookup manager stores all offsets to offsets in the file.
        public LookupManager _lookupManager;

        public List<SakuraiEntryNode> _postProcessNodes;
        private CompactStringTable _referenceStringTable;

        public int _sectionCount, _referenceCount;

        private int _size;

        public SakuraiArchiveBuilder(SakuraiArchiveNode node)
        {
            RootNode = node;
        }

        public SakuraiArchiveNode RootNode { get; }

        public bool IsCalculatingSize { get; private set; }

        public bool IsRebuilding { get; } = false;

        /// <summary>
        ///     This gets the size using entries that need to be rebuilt
        ///     and the original lookup entries to inject the edited data
        ///     into the original file
        /// </summary>
        /// <returns></returns>
        public unsafe int QuickGetSize()
        {
            IsCalculatingSize = true;

            var origAddr = RootNode.WorkingUncompressed.Address;
            var hdr = (SakuraiArchiveHeader*) origAddr;
            var origBase = hdr->BaseAddress;

            _size = RootNode.WorkingUncompressed.Length;
            foreach (var entry in RootNode.RebuildEntries)
            {
                var newSize = entry.GetSize();
                var oldSize = entry._initSize;
                var diff = newSize - oldSize;

                var lookupCount = entry.GetLookupCount();

                var minOffset = entry._offset;
                var maxOffset = minOffset + oldSize;

                var lookup = hdr->LookupEntries;
                int currentOffset = *(bint*) (origBase + *lookup++);
                for (var i = 0; i < hdr->_lookupEntryCount - 1; i++, lookup++)
                {
                    int nextOffset = *(bint*) (origBase + *lookup);
                    if (minOffset >= currentOffset && minOffset < nextOffset)
                        for (var x = i; x < hdr->_lookupEntryCount; x++)
                        {
                            int insideOffset = *(bint*) (origBase + lookup[x]);
                        }
                }

                _size += diff;
            }

            IsCalculatingSize = false;

            return _size;
        }

        /// <summary>
        ///     This writes changed data without rewriting the entire file
        /// </summary>
        public unsafe void QuickWrite()
        {
            var origAddr = RootNode.WorkingUncompressed.Address;
            var hdr = (SakuraiArchiveHeader*) origAddr;
            var origBase = hdr->BaseAddress;

            var changed = RootNode.RebuildEntries;
            if (changed.Count != 0)
                foreach (var entry in changed)
                {
                    var eOffset = entry._offset;
                    var lookup = hdr->LookupEntries;
                    int currentOffset = *(bint*) (origBase + *lookup++);
                    for (var i = 0; i < hdr->_lookupEntryCount - 1; i++, lookup++)
                    {
                        int nextOffset = *(bint*) (origBase + *lookup);
                        if (eOffset >= currentOffset && eOffset < nextOffset)
                        {
                            var newSize = entry._calcSize;
                            var oldSize = entry._initSize;
                            var diff = newSize - oldSize;

                            for (var x = i; x < hdr->_lookupEntryCount; x++) lookup[x] += diff;
                        }

                        currentOffset = nextOffset;
                    }
                }
        }

        public int GetSize()
        {
            IsCalculatingSize = true;

            //Reset variables
            _lookupCount = _lookupLen = 0;
            _lookupManager = new LookupManager();
            _postProcessNodes = new List<SakuraiEntryNode>();
            _referenceStringTable = new CompactStringTable();

            //Add header size
            _size = SakuraiArchiveHeader.Size;

            //Calculate the size of each section and add names to string table
            foreach (var section in RootNode.SectionList)
            {
                SakuraiEntryNode entry = section;

                //Sections *usually* have only one reference in data or dataCommon
                //An example of an exception to this is the 'AnimCmd' section
                //If a reference exists, calculate the size of that reference instead
                if (section.References.Count > 0) entry = section.References[0];

                //Add the size of the entry's data, and the entry itself
                _size += entry.GetSize() + 8;

                //Add the lookup count
                _lookupCount += entry.GetLookupCount();

                //Add the section's name to the string table
                _referenceStringTable.Add(entry.Name);
            }

            //Calculate reference table size and add names to string table
            foreach (var reference in RootNode.ReferenceList)
                if (reference.References.Count > 0 || AddUnreferencedReferences)
                {
                    //Add entry name to string table
                    _referenceStringTable.Add(reference.Name);

                    //Add entry size (reference don't have any actual 'data' size)
                    _size += 8;
                }

            //TODO: Does each reference offset throughout the file
            //have a lookup offset?
            //Also, subtract 1 because the initial offset has no lookup entry
            //lookupCount += e.References.Count - 1;

            IsCalculatingSize = false;

            //Add the lookup size and reference table size
            _size += (_lookupLen = _lookupCount * 4) + _referenceStringTable.TotalSize;

            return _size;
        }

        public unsafe void Write(SakuraiArchiveNode node, VoidPtr address, int length)
        {
            _baseAddress = address + 0x20;
            _currentAddress = _baseAddress;

            //Write header
            var hdr = (SakuraiArchiveHeader*) address;
            hdr->_sectionCount = _sectionCount;
            hdr->_externalSubRoutineCount = _referenceCount;
            hdr->_lookupEntryCount = _lookupManager.Count;
            hdr->_fileSize = length;
            hdr->_pad1 = hdr->_pad2 = hdr->_pad3 = 0;

            var _sectionOffsets = new List<int>();

            //Write section data
            foreach (var section in node.SectionList)
            {
                SakuraiEntryNode entry = section;

                //If this section is referenced from an entry,
                //write that entry instead
                if (section.References.Count > 0) entry = section.References[0];

                _sectionOffsets.Add(entry.Write(_currentAddress));
                _currentAddress += entry.TotalSize;
            }

            //Write lookup values
            hdr->_lookupOffset = (int) _currentAddress - (int) _baseAddress;
            _lookupManager.Write(ref _currentAddress);

            //These can only be accessed after the lookup offset and count
            //have been written to the header.
            var sectionAddr = hdr->Sections;
            var refAddr = hdr->ExternalSubRoutines;
        }
    }
}