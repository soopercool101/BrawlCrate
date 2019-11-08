using BrawlLib.Internal;
using BrawlLib.SSBB.ResourceNodes;
using BrawlLib.SSBB.Types;
using System.Collections.Generic;

namespace BrawlLib.SSBB
{
    public class SakuraiArchiveBuilder
    {
        //Modifyable behavior!
        private const bool AddUnreferencedReferences = true;

        public SakuraiArchiveBuilder(SakuraiArchiveNode node)
        {
            _rootNode = node;
        }

        public SakuraiArchiveNode RootNode => _rootNode;
        public bool IsCalculatingSize => _calculatingSize;
        public bool IsRebuilding => _rebuilding;

        private int _size;
        private readonly SakuraiArchiveNode _rootNode;
        private readonly bool _rebuilding = false;
        private bool _calculatingSize;
        private CompactStringTable _referenceStringTable;

        public List<SakuraiEntryNode> _postProcessNodes;
        public VoidPtr _baseAddress, _currentAddress;

        //The lookup manager stores all offsets to offsets in the file.
        public LookupManager _lookupManager;
        public int _lookupCount, _lookupLen;

        public int _sectionCount, _referenceCount;

        /// <summary>
        /// This gets the size using entries that need to be rebuilt 
        /// and the original lookup entries to inject the edited data
        /// into the original file
        /// </summary>
        /// <returns></returns>
        public unsafe int QuickGetSize()
        {
            _calculatingSize = true;

            VoidPtr origAddr = _rootNode.WorkingUncompressed.Address;
            SakuraiArchiveHeader* hdr = (SakuraiArchiveHeader*) origAddr;
            VoidPtr origBase = hdr->BaseAddress;

            _size = _rootNode.WorkingUncompressed.Length;
            foreach (SakuraiEntryNode entry in _rootNode.RebuildEntries)
            {
                int newSize = entry.GetSize();
                int oldSize = entry._initSize;
                int diff = newSize - oldSize;

                int lookupCount = entry.GetLookupCount();

                int minOffset = entry._offset;
                int maxOffset = minOffset + oldSize;

                bint* lookup = hdr->LookupEntries;
                int currentOffset = *(bint*) (origBase + *lookup++);
                for (int i = 0; i < hdr->_lookupEntryCount - 1; i++, lookup++)
                {
                    int nextOffset = *(bint*) (origBase + *lookup);
                    if (minOffset >= currentOffset && minOffset < nextOffset)
                    {
                        for (int x = i; x < hdr->_lookupEntryCount; x++)
                        {
                            int insideOffset = *(bint*) (origBase + lookup[x]);
                        }
                    }
                }

                _size += diff;
            }

            _calculatingSize = false;

            return _size;
        }

        /// <summary>
        /// This writes changed data without rewriting the entire file
        /// </summary>
        public unsafe void QuickWrite()
        {
            VoidPtr origAddr = _rootNode.WorkingUncompressed.Address;
            SakuraiArchiveHeader* hdr = (SakuraiArchiveHeader*) origAddr;
            VoidPtr origBase = hdr->BaseAddress;

            System.ComponentModel.BindingList<SakuraiEntryNode> changed = _rootNode.RebuildEntries;
            if (changed.Count != 0)
            {
                foreach (SakuraiEntryNode entry in changed)
                {
                    int eOffset = entry._offset;
                    bint* lookup = hdr->LookupEntries;
                    int currentOffset = *(bint*) (origBase + *lookup++);
                    for (int i = 0; i < hdr->_lookupEntryCount - 1; i++, lookup++)
                    {
                        int nextOffset = *(bint*) (origBase + *lookup);
                        if (eOffset >= currentOffset && eOffset < nextOffset)
                        {
                            int newSize = entry._calcSize;
                            int oldSize = entry._initSize;
                            int diff = newSize - oldSize;

                            for (int x = i; x < hdr->_lookupEntryCount; x++)
                            {
                                lookup[x] += diff;
                            }
                        }

                        currentOffset = nextOffset;
                    }
                }
            }
        }

        public int GetSize()
        {
            _calculatingSize = true;

            //Reset variables
            _lookupCount = _lookupLen = 0;
            _lookupManager = new LookupManager();
            _postProcessNodes = new List<SakuraiEntryNode>();
            _referenceStringTable = new CompactStringTable();

            //Add header size
            _size = SakuraiArchiveHeader.Size;

            //Calculate the size of each section and add names to string table
            foreach (TableEntryNode section in _rootNode.SectionList)
            {
                SakuraiEntryNode entry = section;

                //Sections *usually* have only one reference in data or dataCommon
                //An example of an exception to this is the 'AnimCmd' section
                //If a reference exists, calculate the size of that reference instead
                if (section.References.Count > 0)
                {
                    entry = section.References[0];
                }

                //Add the size of the entry's data, and the entry itself
                _size += entry.GetSize() + 8;

                //Add the lookup count
                _lookupCount += entry.GetLookupCount();

                //Add the section's name to the string table
                _referenceStringTable.Add(entry.Name);
            }

            //Calculate reference table size and add names to string table
            foreach (TableEntryNode reference in _rootNode.ReferenceList)
            {
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
            }

            _calculatingSize = false;

            //Add the lookup size and reference table size
            _size += (_lookupLen = _lookupCount * 4) + _referenceStringTable.TotalSize;

            return _size;
        }

        public unsafe void Write(SakuraiArchiveNode node, VoidPtr address, int length)
        {
            _baseAddress = address + 0x20;
            _currentAddress = _baseAddress;

            //Write header
            SakuraiArchiveHeader* hdr = (SakuraiArchiveHeader*) address;
            hdr->_sectionCount = _sectionCount;
            hdr->_externalSubRoutineCount = _referenceCount;
            hdr->_lookupEntryCount = _lookupManager.Count;
            hdr->_fileSize = length;
            hdr->_pad1 = hdr->_pad2 = hdr->_pad3 = 0;

            List<int> _sectionOffsets = new List<int>();

            //Write section data
            foreach (TableEntryNode section in node.SectionList)
            {
                SakuraiEntryNode entry = section;

                //If this section is referenced from an entry,
                //write that entry instead
                if (section.References.Count > 0)
                {
                    entry = section.References[0];
                }

                _sectionOffsets.Add(entry.Write(_currentAddress));
                _currentAddress += entry.TotalSize;
            }

            //Write lookup values
            hdr->_lookupOffset = (int) _currentAddress - (int) _baseAddress;
            _lookupManager.Write(ref _currentAddress);

            //These can only be accessed after the lookup offset and count
            //have been written to the header.
            sStringEntry* sectionAddr = hdr->Sections;
            sStringEntry* refAddr = hdr->ExternalSubRoutines;
        }
    }
}