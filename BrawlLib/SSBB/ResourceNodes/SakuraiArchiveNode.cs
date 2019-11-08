using BrawlLib.Internal;
using BrawlLib.SSBB.Types;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class SakuraiArchiveNode : ARCEntryNode
    {
        #region Variables & Properties

        [Browsable(true)]
        [Category("Sakurai Archive Node")]
        public int DataSize => _dataSize;

        public int _dataSize;

        [Browsable(false)] public bool Initializing => _initializing;
        public bool _initializing;

        /// <summary>
        /// Returns the address after the moveset header that all offsets use as a base.
        /// This should only be used when parsing or writing.
        /// </summary>
        [Browsable(false)]
        public VoidPtr BaseAddress =>
            Builder == null ? WorkingUncompressed.Address + SakuraiArchiveHeader.Size : Builder._baseAddress;

        /// <summary>
        /// Returns all entries in the moveset that have had a property changed.
        /// Changed entries do not necessarily mean that a rebuild is needed.
        /// </summary>
        [Browsable(false)]
        public BindingList<SakuraiEntryNode> ChangedEntries => _changedEntries;

        private BindingList<SakuraiEntryNode> _changedEntries = new BindingList<SakuraiEntryNode>();

        /// <summary>
        /// Returns all entries in the moveset that have have had a data size change
        /// A rebuild is needed. Can perform an inject, or can rebuild entire moveset.
        /// </summary>
        [Browsable(false)]
        public BindingList<SakuraiEntryNode> RebuildEntries => _rebuildEntries;

        private BindingList<SakuraiEntryNode> _rebuildEntries = new BindingList<SakuraiEntryNode>();

        /// <summary>
        /// True if the moveset file has had something added or removed and must be rebuilt.
        /// </summary>
        [Browsable(false)]
        public bool RebuildNeeded
        {
            get => _rebuildNeeded;
            set => _rebuildNeeded = value;
        }

        private bool _rebuildNeeded;

        /// <summary>
        /// List of external subroutines located in Fighter.pac.
        /// </summary>
        [Browsable(false)]
        public BindingList<TableEntryNode> ReferenceList => _referenceList;

        private BindingList<TableEntryNode> _referenceList;

        /// <summary>
        /// List of important entries located in this moveset file.
        /// </summary>
        [Browsable(false)]
        public BindingList<TableEntryNode> SectionList => _sectionList;

        private BindingList<TableEntryNode> _sectionList;

        /// <summary>
        /// Provides the size of any entry based on its offset.
        /// </summary>
        [Browsable(false)]
        public SortedList<int, int> LookupSizes => _lookupSizes;

        private SortedList<int, int> _lookupSizes;

        /// <summary>
        /// Provides easy access to any entry in the moveset using its original offset.
        /// Use only when parsing.
        /// </summary>
        public SortedDictionary<int, SakuraiEntryNode> EntryCache => _entryCache;

        private SortedDictionary<int, SakuraiEntryNode> _entryCache;

        public List<SakuraiEntryNode> _postParseEntries;

        #endregion

        #region Parsing

        public override bool OnInitialize()
        {
            //Start initializing. 
            //This enables some functions for use.
            _initializing = true;

            SakuraiArchiveHeader* hdr = (SakuraiArchiveHeader*) WorkingUncompressed.Address;

            InitData(hdr);
            GetLookupSizes(hdr);
            ParseExternals(hdr);
            ParseInternals(hdr);
            PostParse();

            return _initializing = false;
        }

        /// <summary>
        /// Initializes all variables.
        /// </summary>
        protected virtual void InitData(SakuraiArchiveHeader* hdr)
        {
            //Get header values
            _dataSize = hdr->_fileSize;

#if DEBUG
            for (int i = 0; i < 3; i++)
            {
                int value = (&hdr->_pad1)[i];
                if (value != 0)
                {
                    Console.WriteLine("MovesetNode InitData " + i);
                }
            }
#endif

            //Create lists
            _changedEntries = new BindingList<SakuraiEntryNode>();
            _rebuildEntries = new BindingList<SakuraiEntryNode>();
            _referenceList = new BindingList<TableEntryNode>();
            _sectionList = new BindingList<TableEntryNode>();
            _lookupSizes = new SortedList<int, int>();
            _entryCache = new SortedDictionary<int, SakuraiEntryNode>();
            _postParseEntries = new List<SakuraiEntryNode>();
        }

        /// <summary>
        /// Creates a table of offsets with a corresponding data size at each offset.
        /// </summary>
        private void GetLookupSizes(SakuraiArchiveHeader* hdr)
        {
            //Read lookup offsets first and use them to get entry sizes at each offset.
            bint* lookup = hdr->LookupEntries;
            //First add each offset to the dictionary with size of 0.
            //The dictionary will sort the offsets automatically, in case they aren't already.
            for (int i = 0; i < hdr->_lookupEntryCount; i++)
            {
                int w = *(bint*) Address(lookup[i]);
                if (!_lookupSizes.ContainsKey(w))
                {
                    _lookupSizes.Add(w, 0);
                }
            }

            //Now go through each offset and calculate the size with the offset difference.
            int prev = 0;
            bool first = true;
            int[] t = _lookupSizes.Keys.ToArray();
            for (int i = 0; i < t.Length; i++)
            {
                int off = t[i];
                if (first)
                {
                    first = false;
                }
                else
                {
                    _lookupSizes[prev] = off - prev;
                }

                prev = off;
            }

            //The last entry in the moveset file goes right up to the lookup offsets.
            _lookupSizes[prev] = Offset(lookup) - prev;
        }

        /// <summary>
        /// Reads external subroutine references
        /// </summary>
        private void ParseExternals(SakuraiArchiveHeader* hdr)
        {
            sStringTable* stringTable = hdr->StringTable;

            //Parse references
            int numRefs = hdr->_externalSubRoutineCount;
            if (numRefs > 0)
            {
                sStringEntry* entries = hdr->ExternalSubRoutines;
                for (int i = 0; i < numRefs; i++)
                {
                    TableEntryNode e = Parse<TableEntryNode>(entries[i]._dataOffset);
                    e._name = stringTable->GetString(entries[i]._stringOffset);
                    _referenceList.Add(e);
                }
            }
        }

        protected virtual void ParseInternals(SakuraiArchiveHeader* hdr)
        {
            sStringTable* stringTable = hdr->StringTable;

            _sectionList = new BindingList<TableEntryNode>();

            //Parse sections
            int numSections = hdr->_sectionCount;
            if (numSections > 0)
            {
                sStringEntry* entries = hdr->Sections;

                List<TableEntryNode> _specialSections = new List<TableEntryNode>();
                for (int i = 0; i < numSections; i++)
                {
                    int offset = entries[i]._dataOffset;
                    string name = stringTable->GetString(entries[i]._stringOffset);

                    TableEntryNode section = TableEntryNode.GetRaw(name);

                    //If null, this type of section doesn't have a dedicated class
                    if (section == null)
                    {
                        //Have the inheriting class handle it.
                        //Initialize the node with Parse() in here!!!
                        section = GetTableEntryNode(name, i);

                        //Still unhandled, so initialize as raw
                        if (section == null)
                        {
                            section = Parse<RawDataNode2>(offset);
                        }
                    }
                    else
                    {
                        _specialSections.Add(section);
                    }

                    section._name = name;
                    section._index = i;
                    section.DataOffsets.Add(offset);

                    _sectionList.Add(section);
                }

                //Now parse any dedicated-class nodes that may reference other sections.
                foreach (TableEntryNode section in _specialSections)
                {
                    section.ParseSelf(this, null, section.DataOffsets[0]);
                }

                HandleSpecialSections(_specialSections);
            }
        }

        protected virtual TableEntryNode GetTableEntryNode(string name, int index)
        {
            return null;
        }

        protected virtual void HandleSpecialSections(List<TableEntryNode> sections)
        {
        }

        protected virtual void PostParse()
        {
            while (_postParseEntries.Count > 0)
            {
                //Make a copy of the post process nodes
                SakuraiEntryNode[] arr = _postParseEntries.ToArray();
                //Clear the original array so it can be repopulated
                _postParseEntries.Clear();
                //Parse subroutines, etc.
                //May add more entries to post process
                foreach (SakuraiEntryNode e in arr)
                {
                    e.PostParse();
                }
            }
        }

        private class Temp
        {
            public int _offset;
            public int _size;

            public Temp(int offset, int size)
            {
                _offset = offset;
                _size = size;
            }
        }

        //This calculate data entry sizes.
        //One array will be initialized with each offset,
        //then another will be created and sorted using the same temp entries.
        //This will allow for sorted offsets and easy indexing of the same entries.
        public static int[] CalculateSizes(int end, bint* hdr, int count, bool data, params int[] ignore)
        {
            Temp[] t = new Temp[count];
            for (int i = 0; i < count; i++)
            {
                if (Array.IndexOf(ignore, i) < 0)
                {
                    t[i] = new Temp(hdr[i], 0);
                }
                else
                {
                    t[i] = null;
                }
            }

            if (data)
            {
                t[2]._offset = 1;
            }

            Temp[] sorted = t.Where(x => x != null).OrderBy(x => x._offset).ToArray();
            if (data)
            {
                t[2]._offset -= 1;
            }

            for (int i = 0; i < sorted.Length; i++)
            {
                sorted[i]._size = (i < sorted.Length - 1 ? sorted[i + 1]._offset : end) - sorted[i]._offset;
            }

            return t.Select(x => x._size).ToArray();
        }

        /// <summary>
        /// Returns a node of the given type at an offset in the moveset file.
        /// </summary>
        public T Parse<T>(int offset) where T : SakuraiEntryNode
        {
            return SakuraiEntryNode.Parse<T>(this, null, Address(offset));
        }

        /// <summary>
        /// Returns a node of the given type at an address in the moveset file.
        /// </summary>
        public T Parse<T>(VoidPtr address) where T : SakuraiEntryNode
        {
            return SakuraiEntryNode.Parse<T>(this, null, address);
        }

        #endregion

        #region Saving

        /// <summary>
        /// Returns the moveset builder of the moveset currently being written.
        /// This can only be used while calculating the size or rebuilding a moveset.
        /// </summary>
        public static SakuraiArchiveBuilder Builder => _currentlyBuilding == null ? null : _currentlyBuilding._builder;

        public static SakuraiArchiveNode _currentlyBuilding;

        public bool IsRebuilding => _builder != null && _builder.IsRebuilding;
        public bool IsCalculatingSize => _builder != null && _builder.IsCalculatingSize;

        internal SakuraiArchiveBuilder _builder;

        public override int OnCalculateSize(bool force)
        {
            _currentlyBuilding = this;
            int size = (_builder = new SakuraiArchiveBuilder(this)).GetSize();
            _currentlyBuilding = null;
            return size;
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            _currentlyBuilding = this;
            _builder.Write(this, address, length);
            _currentlyBuilding = null;
            _builder = null;
            _changedEntries.Clear();
            _rebuildEntries.Clear();
        }

        #endregion

        #region Parse functions

        /// <summary>
        /// Returns the offset of the given address from the base address.
        /// Use this only when parsing or writing.
        /// </summary>
        public int Offset(VoidPtr address)
        {
#if DEBUG
            if (!_initializing && _currentlyBuilding != this)
            {
                throw new Exception("Not initializing or rebuilding.");
            }
#endif
            return address - BaseAddress;
        }

        /// <summary>
        /// Returns the address at the given offset from the base address.
        /// Use this only when parsing or writing.
        /// </summary>
        public VoidPtr Address(int offset)
        {
#if DEBUG
            if (!_initializing && _currentlyBuilding != this)
            {
                throw new Exception("Not initializing or rebuilding.");
            }
#endif
            return BaseAddress + offset;
        }

        /// <summary>
        /// Returns the (assumed) size of the data at the given offset.
        /// Use this only when parsing.
        /// </summary>
        public int GetSize(int offset)
        {
#if DEBUG
            if (!_initializing)
            {
                throw new Exception("Not initializing.");
            }
#endif
            if (_lookupSizes.ContainsKey(offset))
            {
                int size = _lookupSizes[offset];
                _lookupSizes.Remove(offset);
                return size;
            }

            return -1;
        }

        /// <summary>
        /// Use this only when parsing.
        /// </summary>
        public TableEntryNode TryGetExternal(int offset)
        {
#if DEBUG
            if (!_initializing)
            {
                throw new Exception("Not initializing.");
            }
#endif
            foreach (TableEntryNode e in _referenceList)
            {
                foreach (int i in e.DataOffsets)
                {
                    if (i == offset)
                    {
                        return e;
                    }
                }
            }

            foreach (TableEntryNode e in _sectionList)
            {
                if (e != null && e.DataOffsets.Count > 0 && e.DataOffsets[0] == offset)
                {
                    return e;
                }
            }

            return null;
        }

        /// <summary>
        /// Returns any entry at the given offset that has been parsed already.
        /// Use this only when parsing.
        /// </summary>
        public SakuraiEntryNode GetEntry(int offset)
        {
#if DEBUG
            if (!_initializing)
            {
                throw new Exception("Not initializing.");
            }
#endif
            if (_entryCache.ContainsKey(offset))
            {
                return _entryCache[offset];
            }

            return null;
        }

        #endregion
    }

    public enum ListValue
    {
        Actions = 0,
        SubActions = 1,
        SubRoutines = 2,
        References = 3,
        Null = 4,
        FlashOverlays = 5,
        ScreenTints = 6
    }

    public enum TypeValue
    {
        None = -1,
        Main = 0,
        GFX = 1,
        SFX = 2,
        Other = 3,
        Entry = 0,
        Exit = 1
    }
}