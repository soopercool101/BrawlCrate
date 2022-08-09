using BrawlLib.Internal;
using BrawlLib.SSBB.Types;
using BrawlLib.SSBB.Types.Audio;
using BrawlLib.Wii.Audio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class RSARNode : NW4RArcEntryNode
    {
        internal RSARHeader* Header => (RSARHeader*) WorkingSource.Address;
        public override ResourceType ResourceFileType => ResourceType.RSAR;

        public override bool IsDirty
        {
            get
            {
                if (!AllowSaving)
                {
                    return false;
                }

                if (HasChanged)
                {
                    return true;
                }

                if (_children != null)
                {
                    foreach (ResourceNode n in _children)
                    {
                        if (n.HasChanged || n.IsBranch || n.IsDirty)
                        {
                            return true;
                        }
                    }
                }

                if (Files != null)
                {
                    foreach (var n in Files)
                    {
                        if (n.HasChanged || n.IsBranch || n.IsDirty)
                        {
                            return true;
                        }
                    }
                }

                return false;
            }
            set
            {
                _changed = value;
                if (_children != null)
                {
                    foreach (ResourceNode r in Children)
                    {
                        if (r._children != null)
                        {
                            r.IsDirty = value;
                        }
                        else
                        {
                            r._changed = value;
                        }
                    }
                }

                if (Files != null)
                {
                    foreach (var r in Files)
                    {
                        if (r._children != null)
                        {
                            r.IsDirty = value;
                        }
                        else
                        {
                            r._changed = value;
                        }
                    }
                }
            }
        }

        [Category("Sound Archive")]
        public ushort SeqSoundCount
        {
            get => _ftr._seqSoundCount;
            set
            {
                _ftr._seqSoundCount = value;
                SignalPropertyChange();
            }
        }

        [Category("Sound Archive")]
        public ushort SeqTrackCount
        {
            get => _ftr._seqTrackCount;
            set
            {
                _ftr._seqTrackCount = value;
                SignalPropertyChange();
            }
        }

        [Category("Sound Archive")]
        public ushort StrmSoundCount
        {
            get => _ftr._strmSoundCount;
            set
            {
                _ftr._strmSoundCount = value;
                SignalPropertyChange();
            }
        }

        [Category("Sound Archive")]
        public ushort StrmTrackCount
        {
            get => _ftr._strmTrackCount;
            set
            {
                _ftr._strmTrackCount = value;
                SignalPropertyChange();
            }
        }

        [Category("Sound Archive")]
        public ushort StrmChannelCount
        {
            get => _ftr._strmChannelCount;
            set
            {
                _ftr._strmChannelCount = value;
                SignalPropertyChange();
            }
        }

        [Category("Sound Archive")]
        public ushort WaveSoundCount
        {
            get => _ftr._waveSoundCount;
            set
            {
                _ftr._waveSoundCount = value;
                SignalPropertyChange();
            }
        }

        [Category("Sound Archive")]
        public ushort WaveTrackCount
        {
            get => _ftr._waveTrackCount;
            set
            {
                _ftr._waveTrackCount = value;
                SignalPropertyChange();
            }
        }

        public List<RSAREntryNode>[] _infoCache = new List<RSAREntryNode>[5];

        internal INFOFooter _ftr;

        private BindingList<RSARFileNode> _files;
        [Browsable(false)] public BindingList<RSARFileNode> Files => _files;

        public override bool OnInitialize()
        {
            base.OnInitialize();

            if (_name == null)
            {
                if (_origPath != null)
                {
                    _name = Path.GetFileNameWithoutExtension(_origPath);
                }
                else
                {
                    _name = "Sound Archive";
                }
            }

            _files = new BindingList<RSARFileNode>();
            _children = new List<ResourceNode>();

            //Retrieve all files to attach to entries
            GetFiles();
            OnPopulate();
            return true;
        }

        //public RSARGroupNode _nullGroup;
        public override void OnPopulate()
        {
            //Enumerate entries, attaching them to the files.
            RSARHeader* rsar = Header;
            SYMBHeader* symb = rsar->SYMBBlock;
            sbyte* offset = (sbyte*) symb + 8;
            buint* stringOffsets = symb->StringOffsets;

            VoidPtr baseAddr = (VoidPtr) rsar->INFOBlock + 8;
            ruint* typeList = (ruint*) baseAddr;

            //Iterate through group types
            for (int i = 0; i < 5; i++)
            {
                Type t = null;
                switch (i)
                {
                    case 0:
                        t = typeof(RSARSoundNode);
                        break;
                    case 1:
                        t = typeof(RSARBankNode);
                        break;
                    case 2:
                        t = typeof(RSARPlayerInfoNode);
                        break;
                    case 3: continue; //Files
                    case 4:
                        t = typeof(RSARGroupNode);
                        break; //Last group entry has no name
                }

                _infoCache[i] = new List<RSAREntryNode>();
                RuintList* list = (RuintList*) ((uint) baseAddr + typeList[i]);
                sbyte* str, end;

                for (int x = 0; x < list->_numEntries; x++)
                {
                    VoidPtr addr = list->Get(baseAddr, x);

                    ResourceNode parent = this;
                    RSAREntryNode n = Activator.CreateInstance(t) as RSAREntryNode;
                    n._origSource = n._uncompSource = new DataSource(addr, 0);

                    if (n.StringId >= 0)
                    {
                        str = offset + stringOffsets[n.StringId];

                        for (end = str; *end != 0; end++)
                        {
                            ;
                        }

                        while (--end > str && *end != '_')
                        {
                            ;
                        }

                        if (end > str)
                        {
                            parent = CreatePath(parent, str, (int) end - (int) str);
                            n._name = new string(end + 1);
                        }
                        else
                        {
                            n._name = new string(str);
                        }
                    }
                    else
                    {
                        n._name = null;
                    }

                    n._infoIndex = x;
                    n.Initialize(parent, addr, 0);
                    _infoCache[i].Add(n);
                }

                _ftr = *(INFOFooter*) ((uint) baseAddr + typeList[5]);

                foreach (RSARFileNode n in Files)
                {
                    if (!(n is RSARExtFileNode))
                    {
                        n.GetName();
                    }
                }
            }
        }

        private void GetFiles()
        {
            INFOFileHeader* fileHeader;
            INFOFileEntry* fileEntry;
            RuintList* entryList;
            INFOGroupHeader* group;
            INFOGroupEntry* gEntry;
            RSARFileNode n;
            DataSource source;
            RSARHeader* rsar = Header;

            //sounds, banks, types, files, groups

            //Get ruint collection from info header
            VoidPtr infoCollection = rsar->INFOBlock->_collection.Address;
            //Convert to ruint buffer
            ruint* groups = (ruint*) infoCollection;
            //Get file ruint list at file offset (groups[3])
            RuintList* fileList = (RuintList*) ((uint) groups + groups[3]);
            //Get the info list
            RuintList* groupList = rsar->INFOBlock->Groups;

            //Loop through the ruint offsets to get all files
            for (int x = 0; x < fileList->_numEntries; x++)
            {
                //Get the file header for the file info
                fileHeader = (INFOFileHeader*) (infoCollection + fileList->Entries[x]);
                entryList = fileHeader->GetList(infoCollection);
                if (entryList->_numEntries == 0)
                {
                    //Must be external file.
                    n = new RSARExtFileNode();
                    source = new DataSource(fileHeader, 0);
                    if (fileHeader->_dataLen > 0)
                    {
                        Console.WriteLine(x + " " + fileHeader->_dataLen);
                    }
                }
                else
                {
                    //Use first entry
                    fileEntry = (INFOFileEntry*) entryList->Get(infoCollection, 0);
                    //Find group with matching ID
                    group = (INFOGroupHeader*) groupList->Get(infoCollection, fileEntry->_groupId);
                    //Find group entry with matching index
                    gEntry = (INFOGroupEntry*) group->GetCollection(infoCollection)->Get(infoCollection,
                        fileEntry->_index);

                    //Create node and parse
                    source = new DataSource((int) rsar + group->_headerOffset + gEntry->_headerOffset,
                        gEntry->_headerLength);
                    if ((n = NodeFactory.GetRaw(source, this) as RSARFileNode) == null)
                    {
                        n = new RSARFileNode();
                    }

                    n._audioSource = new DataSource((int) rsar + group->_waveDataOffset + gEntry->_dataOffset,
                        gEntry->_dataLength);
                    n._infoHdr = fileHeader;
                }

                n._fileIndex = x;
                n._entryNumber = fileHeader->_entryNumber;
                n._parent = this; //This is so that the node won't add itself to the child list.
                n.Initialize(this, source);
                _files.Add(n);
            }

            //foreach (ResourceNode r in _files)
            //    r.Populate();
        }

        private ResourceNode CreatePath(ResourceNode parent, sbyte* str, int length)
        {
            ResourceNode current;

            int len;
            char* cPtr;
            sbyte* start, end;
            sbyte* ceil = str + length;
            while (str < ceil)
            {
                for (end = str; end < ceil && *end != '_'; end++)
                {
                    ;
                }

                len = (int) end - (int) str;

                current = null;
                foreach (ResourceNode n in parent._children)
                {
                    if (n._name.Length != len || !(n is RSARFolderNode))
                    {
                        continue;
                    }

                    fixed (char* p = n._name)
                    {
                        for (cPtr = p, start = str; start < end && *start == *cPtr; start++, cPtr++)
                        {
                            ;
                        }
                    }

                    if (start == end)
                    {
                        current = n;
                        break;
                    }
                }

                if (current == null)
                {
                    current = new RSARFolderNode
                    {
                        _name = new string(str, 0, len),
                        _parent = parent
                    };
                    parent._children.Add(current);
                }

                str = end + 1;
                parent = current;
            }

            return parent;
        }

        private readonly RSAREntryList _entryList = new RSAREntryList();
        private RSARConverter _converter = new RSARConverter();

        public override int OnCalculateSize(bool force)
        {
            _entryList.Clear();
            _entryList._files = Files;
            _converter = new RSARConverter();

            List<string> unusedFolders = new List<string>();
            foreach (ResourceNode n in Children)
            {
                if (n is RSARFolderNode)
                {
                    ((RSARFolderNode) n).GetStrings(null, 0, _entryList, ref unusedFolders);
                }
                else
                {
                    (n as RSAREntryNode)?.GetStrings(null, 0, _entryList);
                }
            }

            if (unusedFolders.Count > 0)
            {
                MessageBox.Show(_mainForm,
                    string.Format("The following path{0} ha{1} no entries and will be lost:\n" +
                                  unusedFolders.Aggregate((current, next) => current + "\n" + next),
                        unusedFolders.Count > 1 ? "s" : "",
                        unusedFolders.Count > 1 ? "ve" : "s"),
                    "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            _entryList.SortStrings();

            return _converter.CalculateSize(_entryList, this);
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            int symbLen, infoLen, fileLen;

            RSARHeader* rsar = (RSARHeader*) address;
            SYMBHeader* symb = (SYMBHeader*) (address + 0x40);
            INFOHeader* info;
            FILEHeader* data;

            info = (INFOHeader*) ((int) symb + (symbLen = _converter.EncodeSYMBBlock(symb, _entryList, this)));
            data = (FILEHeader*) ((int) info + (infoLen = _converter.EncodeINFOBlock(info, _entryList, this)));
            fileLen = _converter.EncodeFILEBlock(data, address, _entryList, this);

            rsar->Set(symbLen, infoLen, fileLen, VersionMinor);

            _entryList.Clear();
        }

        internal static ResourceNode TryParse(DataSource source, ResourceNode parent)
        {
            return ((RSARHeader*) source.Address)->_header._tag == RSARHeader.Tag ? new RSARNode() : null;
        }
    }

    public enum PanMode
    {
        Dual,   // Perform position processing for stereo as two monaural channels.
        Balance // Process the volume balance for the left and right channels.
    }

    public enum PanCurve
    {
        Sqrt,           // Square root curve. The volume will be -3 dB in the center and 0 dB at both ends.
        Sqrt0DB,        // Square root curve. The volume will be 0 dB in the center and +3 dB at both ends.
        Sqrt0DBClamp,   // Square root curve. The volume will be 0 dB in the center and 0 dB at both ends.
        SinCos,         // Trigonometric curve. The volume will be -3 dB in the center and 0 dB at both ends.
        SinCos0DB,      // Trigonometric curve. The volume will be 0 dB in the center and +3 dB at both ends.
        SinCos0DBClamp, // Trigonometric curve. The volume will be 0 dB in the center and 0 dB at both ends.
        Linear,         // Linear curve. The volume will be -6 dB in the center and 0 dB at both ends.
        Linear0DB,      // Linear curve. The volume will be 0 dB in the center and +6 dB at both ends.
        Linear0DBClamp  // Linear curve. The volume will be 0 dB in the center and 0 dB at both ends.
    }

    public enum DecayCurve
    {
        Logarithmic = 1, // Logarithmic curve
        Linear = 2       // Linear curve
    }

    public enum RegionTableType
    {
        Invalid = 0,
        Direct = 1,
        RangeTable = 2,
        IndexTable = 3,
        Null = 4
    }
}