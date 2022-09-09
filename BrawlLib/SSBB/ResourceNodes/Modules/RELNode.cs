using BrawlLib.Internal;
using BrawlLib.Internal.PowerPCAssembly;
using BrawlLib.Internal.Windows.Controls.Hex_Editor;
using BrawlLib.SSBB.Types;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace BrawlLib.SSBB.ResourceNodes
{
    //Credit to PhantomWings for researching RELs and coding Module Editors 1, 2 & 3
    public unsafe class RELNode : ARCEntryNode, ModuleNode
    {
        internal RELHeader* Header => (RELHeader*) WorkingUncompressed.Address;
        public override ResourceType ResourceFileType => ResourceType.REL;

        [Browsable(false)]
        public ModuleSectionNode[] Sections
        {
            get
            {
                if (_sections == null)
                {
                    Populate();
                }

                return _sections;
            }
        }

        public ModuleSectionNode[] _sections;

        public uint _id;
        public int _linkNext; //0
        public int _linkPrev; //0
        public uint _numSections;

        public uint _infoOffset;
        public uint _nameOffset;
        public uint _nameSize;
        public uint _version;

        public uint _bssSize;
        public uint _relOffset;
        public uint _impOffset;
        public uint _impSize;

        public byte _prologSection;
        public byte _epilogSection;
        public byte _unresolvedSection;
        public byte _bssSection;

        public uint _prologOffset;
        public uint _epilogOffset;
        public uint _unresolvedOffset;

        public uint _moduleAlign = 32;
        public uint _bssAlign = 8;
        public uint _fixSize;

        public byte? _stageID;  // null if it's not a stage .rel
        public byte[] _itemIDs; // null if it's not an online training room .rel

        [Category("Relocatable Module")]
        public uint ModuleID
        {
            get => _id;
            set
            {
                if (!_files.ContainsKey(value))
                {
                    //TODO: correct all opened modules that refer to this one via module id
                    //Now won't that be entertaining to code

                    _files.Remove(_id);
                    _id = value;
                    SignalPropertyChange();
                    _files.Add(_id, this);
                }
                else
                {
                    MessageBox.Show("");
                }
            }
        }

        [Browsable(false)]
        public new uint ID
        {
            get => _id;
            set => _id = value;
        }

        [Category("Relocatable Module")]
        public string ModuleName => _idNames.ContainsKey(ModuleID) ? _idNames[ModuleID] : "m" + ModuleID;

        //[Category("REL")]
        //public int NextLink { get { return _linkNext; } }
        //[Category("REL")]
        //public int PrevLink { get { return _linkPrev; } }
        //[Category("REL")]
        //public uint SectionCount { get { return _numSections; } }

        //[Category("REL")]
        //public uint SectionInfoOffset { get { return _infoOffset; } }
        [Category("Relocatable Module")]
        public uint NameOffset
        {
            get => _nameOffset;
            set
            {
                _nameOffset = value;
                SignalPropertyChange();
            }
        }

        [Category("Relocatable Module")]
        public uint NameSize
        {
            get => _nameSize;
            set
            {
                _nameSize = value;
                SignalPropertyChange();
            }
        }

        [Category("Relocatable Module")] public uint Version => _version;

        [Category("Relocatable Module")] public uint PrologSection => _prologSection;
        [Category("Relocatable Module")] public uint EpilogSection => _epilogSection;
        [Category("Relocatable Module")] public uint UnresolvedSection => _unresolvedSection;
        [Category("Relocatable Module")] public uint BSSSection => _bssSection;

        [Category("Relocatable Module")] public uint ModuleAlign => _moduleAlign;
        [Category("Relocatable Module")] public uint BSSAlign => _bssAlign;
        [Category("Relocatable Module")] public uint FixSize => _fixSize;

        [Category("Relocatable Module")]
        [DisplayName("Imported Modules")]
        public string[] ImportedModules { get; private set; }

        #region Stage module conversion - designer properties

        [Category("Brawl Stage Module")]
        [TypeConverter(typeof(DropDownListStageRelIDs))]
        public int? StageID
        {
            get => _stageID;
            set
            {
                if (_stageID == null || value == null)
                {
                    return;
                }

                _stageID = (byte) value;
                SignalPropertyChange();
            }
        }

        public void UpdateItemIDs()
        {
            for (int i = 0; i < _itemIDOffsets?.Length; i++)
            {
                _itemIDs[i] = Sections[1]._dataBuffer[_itemIDOffsets[i], 1].Byte;
            }
        }

        public void SaveItems()
        {
            if (_itemIDOffsets.Length == 0)
            {
                return;
            }

            ModuleSectionNode section = Sections[1];

            DynamicFileByteProvider d =
                new DynamicFileByteProvider(new UnmanagedMemoryStream((byte*) section._dataBuffer.Address,
                        section._dataBuffer.Length, section._dataBuffer.Length, FileAccess.ReadWrite))
                    { _supportsInsDel = false };

            for (int i = 0; i < _itemIDOffsets.Length; i++)
            {
                d.WriteByte(_itemIDOffsets[i], _itemIDs[i]);
            }
            
            if (!d.HasChanges())
            {
                return;
            }
            
            UnsafeBuffer newBuffer = new UnsafeBuffer((int)d.Length);

            int amt = Math.Min(section._dataBuffer.Length, newBuffer.Length);
            if (amt > 0)
            {
                Memory.Move(newBuffer.Address, section._dataBuffer.Address, (uint)amt);
                if (newBuffer.Length - amt > 0)
                {
                    Memory.Fill(newBuffer.Address + amt, (uint)(newBuffer.Length - amt), 0);
                }
            }

            d._stream?.Dispose();

            d._stream = new UnmanagedMemoryStream((byte*)newBuffer.Address, newBuffer.Length, newBuffer.Length,
                FileAccess.ReadWrite);

            d.ApplyChanges();

            section._dataBuffer.Dispose();
            section._dataBuffer = newBuffer;
            section.SignalPropertyChange();
            d.Dispose();
        }

        [Category("Brawl Stage Module")]
        [TypeConverter(typeof(DropDownListByteItemIDs))]
        public byte? ItemID1
        {
            get => _itemIDs?.Length > 0 ? _itemIDs?[0] : null;
            set
            {
                // Don't try to set the item ID if there are none known
                if (_itemIDs == null || _itemIDs.Length <= 0 || value == null)
                {
                    return;
                }

                _itemIDs[0] = value.Value;
                SaveItems();
            }
        }

        [Category("Brawl Stage Module")]
        [TypeConverter(typeof(DropDownListByteItemIDs))]
        public byte? ItemID2
        {
            get => _itemIDs?.Length > 1 ? _itemIDs?[1] : null;
            set
            {
                // Don't try to set the item ID if there are not 2 known
                if (_itemIDs == null || _itemIDs.Length <= 1 || value == null)
                {
                    return;
                }

                _itemIDs[1] = value.Value;
                SaveItems();
            }
        }

        [Category("Brawl Stage Module")]
        [TypeConverter(typeof(DropDownListByteItemIDs))]
        public byte? ItemID3
        {
            get => _itemIDs?.Length > 2 ? _itemIDs?[2] : null;
            set
            {
                // Don't try to set the item ID if there are not 3 known
                if (_itemIDs == null || _itemIDs.Length <= 2 || value == null)
                {
                    return;
                }

                _itemIDs[2] = value.Value;
                SaveItems();
            }
        }

        [Category("Brawl Stage Module")]
        [TypeConverter(typeof(DropDownListByteItemIDs))]
        public byte? ItemID4
        {
            get => _itemIDs?.Length > 3 ? _itemIDs?[3] : null;
            set
            {
                // Don't try to set the item ID if there are not 4 known
                if (_itemIDs == null || _itemIDs.Length <= 3 || value == null)
                {
                    return;
                }

                _itemIDs[3] = value.Value;
                SaveItems();
            }
        }

        #endregion


        public override void Dispose()
        {
            while (!_populated)
            {
                // Do nothing until population is complete, fixing API enumeration errors
            }
            _files.Remove(ModuleID);
            base.Dispose();
        }

        public override bool OnInitialize()
        {
            _id = Header->_info._id;
            _linkNext = Header->_info._link._linkNext; //0
            _linkPrev = Header->_info._link._linkPrev; //0
            _numSections = Header->_info._numSections;
            _infoOffset = Header->_info._sectionInfoOffset;
            _nameOffset = Header->_info._nameOffset;
            _nameSize = Header->_info._nameSize;
            _version = Header->_info._version;

            _bssSize = Header->_bssSize;
            _relOffset = Header->_relOffset;
            _impOffset = Header->_impOffset;
            _impSize = Header->_impSize;
            _prologSection = Header->_prologSection;
            _epilogSection = Header->_epilogSection;
            _unresolvedSection = Header->_unresolvedSection;
            _bssSection = Header->_bssSection;
            _prologOffset = Header->_prologOffset;
            _epilogOffset = Header->_epilogOffset;
            _unresolvedOffset = Header->_unresolvedOffset;

            _moduleAlign = Header->_moduleAlign;
            _bssAlign = Header->_bssAlign;
            _fixSize = Header->_commandOffset;

            _imports = new SortedDictionary<uint, List<RELLink>>();
            string[] impNames = new string[Header->ImportListCount];

            for (int i = 0; i < Header->ImportListCount; i++)
            {
                RELImportEntry* entry = &Header->Imports[i];
                uint id = entry->_moduleId;
                _imports.Add(id, new List<RELLink>());
                impNames[i] = _idNames.ContainsKey(id) ? _idNames[id] : $"Module{id}";

                RELLink* link = (RELLink*) (WorkingUncompressed.Address + entry->_offset);
                do
                {
                    _imports[id].Add(*link);
                } while ((link++)->_type != RELLinkType.End);
            }

            ImportedModules = impNames;

            if (_name == null)
            {
                _name = _idNames.ContainsKey(_id) ? _idNames[_id] : Path.GetFileName(_origPath);
            }

            if (!_files.ContainsKey(ModuleID))
            {
                _files.Add(ModuleID, this);
            }
            else
            {
                _files[ModuleID] = this;
            }

            return true;
        }

        public SortedDictionary<uint, List<RELLink>> _imports = new SortedDictionary<uint, List<RELLink>>();

        private int[] _itemIDOffsets;

        private bool _populated = false;
        public override void OnPopulate()
        {
            _sections = new ModuleSectionNode[_numSections];
            int prevOffset = RELHeader.Size + RELSectionEntry.Size * (int) _numSections;
            int lastDataSection = -1;
            for (int i = 0; i < _numSections; i++)
            {
                RELSectionEntry entry = Header->SectionInfo[i];
                ModuleSectionNode section = _sections[i] = new ModuleSectionNode();

                int dataOffset = entry.Offset, dataSize = (int) (uint) entry._size;

                section._isCodeSection = entry.IsCodeSection;
                section._dataOffset = dataOffset;
                section._dataSize = entry._size;

                // Calculate buffer
                if (!Properties.Settings.Default.AutoCompressModules && i > 0 && dataOffset > 0)
                {
                    _sections[i - 1]._endBufferSize = (dataOffset - prevOffset).ClampMin(0);
                    lastDataSection = i;
                }

                section.Initialize(this, WorkingUncompressed.Address + dataOffset, dataSize);

                if (dataOffset > 0)
                {
                    section._dataAlign = dataOffset - prevOffset;
                    prevOffset = dataOffset + dataSize;
                }
            }

            if (lastDataSection != -1)
            {
                // Calculate buffer between last section and imports
                RELSectionEntry entry = Header->SectionInfo[lastDataSection];
                int dataOffset = entry.Offset, dataSize = (int) (uint) entry._size;
                _sections[lastDataSection]._endBufferSize =
                    ((int) (_impOffset - (dataOffset + dataSize))).ClampMin(0);
            }

            //Larger modules may take slightly longer to relocate
            //Use a background worker so the UI thread isn't suspended
            Action<object, DoWorkEventArgs> work = (object sender, DoWorkEventArgs e) =>
            {
                Stopwatch watch = Stopwatch.StartNew();

                ApplyRelocations();

                //Scan for branches, add extra tags
                foreach (ModuleSectionNode s in Sections)
                {
                    if (s.HasCode)
                    {
                        PPCOpCode code;
                        buint* opPtr = s.BufferAddress;
                        for (int i = 0; i < s._dataBuffer.Length / 4; i++)
                        {
                            if ((code = (uint) *opPtr++) is PPCBranch && !(code is PPCblr || code is PPCbctr))
                            {
                                s._manager.LinkBranch(i, true);
                            }
                        }

                        KeyValuePair<int, RelCommand>[] cmds = s._manager.GetCommands();
                        foreach (KeyValuePair<int, RelCommand> x in cmds)
                        {
                            RelocationTarget target = x.Value.GetTargetRelocation();
                            string value = null;
                            if (target.Section != null && target._sectionID == 5 &&
                                !string.IsNullOrEmpty(value = target.Section._manager.GetString(target._index)))
                            {
                                s._manager.AddTag(x.Key, value);
                            }
                        }
                    }
                }

                Sections[5].Populate();

                watch.Stop();
                Console.WriteLine("Took {0} seconds to relocate {1} module", watch.ElapsedMilliseconds / 1000d, Name);
                _populated = true;
            };

            using (BackgroundWorker b = new BackgroundWorker())
            {
                b.DoWork += new DoWorkEventHandler(work);
                b.RunWorkerAsync();
            }

            // Stage module conversion
            byte* bptr = (byte*) WorkingUncompressed.Address;
            int offset = findStageIDOffset();
            _stageID = offset < 0 ? (byte?) null : bptr[offset];
            _itemIDOffsets = null;
            // ReSharper disable once StringLiteralTypo (Typo is present in vBrawl)
            if (nodeContainsString("stOnlineTrainning"))
            {
                // File must be online training room .rel file
                _itemIDs = new byte[OTrainItemOffsets.Length];
                _itemIDOffsets = OTrainItemOffsets;
            }
            else if (nodeContainsString("stDxGreens"))
            {
                _itemIDs = new byte[DxGreensItemOffsets.Length];
                _itemIDOffsets = DxGreensItemOffsets;
            }
            else if (nodeContainsString("stEarth"))
            {
                _itemIDs = new byte[EarthItemOffsets.Length];
                _itemIDOffsets = EarthItemOffsets;
            }
            else if (nodeContainsString("stVillage"))
            {
                _itemIDs = new byte[VillageItemOffsets.Length];
                _itemIDOffsets = VillageItemOffsets;
            }
            else if (nodeContainsString("stCrayon"))
            {
                _itemIDs = new byte[CrayonItemOffsets.Length];
                _itemIDOffsets = CrayonItemOffsets;
            }
            UpdateItemIDs();
        }

        public void ApplyRelocations()
        {
            foreach (ModuleSectionNode r in Sections)
            {
                r._manager.ClearCommands();
            }

            int offset = 0;
            int i = 0;
            foreach (uint x in _imports.Keys)
            {
                List<RELLink> cmds = _imports[x];
                ModuleSectionNode section = null;
                foreach (RELLink link in cmds)
                {
                    if (link._type == RELLinkType.Section)
                    {
                        offset = 0;
                        section = Sections[link._section];
                    }
                    else
                    {
                        offset += link._prevOffset;

                        if (link._type == RELLinkType.End || link._type == RELLinkType.IncrementOffset)
                        {
                            continue;
                        }

                        if (link._type == RELLinkType.MrkRef)
                        {
                            Console.WriteLine("Mark Ref");
                            continue;
                        }

                        section?._manager.SetCommand(offset.RoundDown(4) / 4, new RelCommand(x, section, link));
                    }
                }

                i++;
            }

            ModuleDataNode s;

            s = _sections[_prologSection];
            offset = (int) _prologOffset;
            //_prologReloc = s.GetRelocationAtOffset(offset);
            //if (_prologReloc != null)
            //    _prologReloc._prolog = true;


            s = _sections[_epilogSection];
            offset = (int) _epilogOffset;

            //_epilogReloc = s.GetRelocationAtOffset(offset);
            //if (_epilogReloc != null)
            //    _epilogReloc._epilog = true;


            s = _sections[_unresolvedSection];
            offset = (int) _unresolvedOffset;

            //_unresReloc = s.GetRelocationAtOffset(offset);
            //if (_unresReloc != null)
            //    _unresReloc._unresolved = true;
        }

        private class ImportData
        {
            public bool _newSection = true;
            public uint _lastOffset;
        }

        public void GenerateImports()
        {
            _imports.Clear();
            Dictionary<uint, ImportData> tempImports = new Dictionary<uint, ImportData>();
            foreach (ModuleSectionNode s in _sections)
            {
                foreach (ImportData e in tempImports.Values)
                {
                    e._newSection = true;
                    e._lastOffset = 0;
                }

                uint offset = 0;
                List<RELLink> links;

                //Iterate through each command in the section
                KeyValuePair<int, RelCommand>[] commands = s._manager.GetCommands();
                foreach (KeyValuePair<int, RelCommand> r in commands)
                {
                    RelCommand command = r.Value;
                    int index = r.Key;

                    ImportData impData;
                    uint moduleID = command._moduleID;

                    //Check if an import has been created for the target module.
                    if (_imports.ContainsKey(moduleID))
                    {
                        //An import already exists, so we'll add to it.
                        links = _imports[moduleID];
                        impData = tempImports[moduleID];
                    }
                    else
                    {
                        //An import does not exist, so it must be made.
                        _imports.Add(moduleID, links = new List<RELLink>());

                        //Create new temporary import data
                        tempImports.Add(moduleID, impData = new ImportData {_newSection = true, _lastOffset = 0});
                    }

                    //This is true when a new section is being evaluated.
                    if (impData._newSection)
                    {
                        links.Add(new RELLink {_type = RELLinkType.Section, _section = (byte) s.Index});
                        impData._newSection = false;
                    }

                    //Get the offset of the command within the section.
                    offset = (uint) index * 4 + (command.IsHalf ? 2u : 0);

                    //Get the offset to this address relative to the last written link offset.
                    uint diff = offset - impData._lastOffset;

                    //If the difference is greater than ushort allows, 
                    //add increment links until the difference works
                    while (diff > 0xFFFF)
                    {
                        impData._lastOffset += 0xFFFF;
                        diff = offset - impData._lastOffset;

                        links.Add(new RELLink
                            {_type = RELLinkType.IncrementOffset, _section = 0, _value = 0, _prevOffset = 0xFFFF});
                    }

                    //Gather the link information
                    byte targetSection = (byte) command._targetSectionId;
                    RELLinkType type = (RELLinkType) command._command;
                    uint val = command._addend;

                    //Write command link
                    links.Add(new RELLink
                        {_type = type, _section = targetSection, _value = val, _prevOffset = (ushort) diff});

                    //Don't bother adding the difference, 
                    //just set the exact offset as the last offset
                    impData._lastOffset = offset;
                }
            }

            foreach (List<RELLink> cmds in _imports.Values)
            {
                cmds.Add(new RELLink {_type = RELLinkType.End});
            }
        }

        public override int OnCalculateSize(bool force)
        {
            GenerateImports();

            int size = RELHeader.Size + Children.Count * RELSectionEntry.Size +
                       _imports.Keys.Count * RELImportEntry.Size;
            foreach (ModuleSectionNode s in Children)
            {
                //Section 4 and 5 seem to be the only ones that are aligned.
                //I don't know the exact alignment procedure. It's not consistent
                //if (s.Index > 3)
                //    size = size.Align(8);

                int r = s.CalculateSize(true);
                if (!s._isBSSSection)
                {
                    size += r;
                }
            }

            foreach (List<RELLink> s in _imports.Values)
            {
                size += s.Count * RELLink.Size;
            }

            return size;
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            RELHeader* header = (RELHeader*) address;

            header->_info._id = _id;
            header->_info._link._linkNext = 0;
            header->_info._link._linkPrev = 0;
            header->_info._numSections = (uint) Children.Count;
            header->_info._sectionInfoOffset = RELHeader.Size;
            header->_info._nameOffset = _nameOffset;
            header->_info._nameSize = _nameSize;
            header->_info._version = _version;

            header->_prologSection = _prologSection;
            header->_prologOffset = _prologOffset;
            header->_epilogSection = _epilogSection;
            header->_epilogOffset = _epilogOffset;
            header->_unresolvedSection = _unresolvedSection;
            header->_unresolvedOffset = _unresolvedOffset;

            header->_moduleAlign = 0x20;
            header->_bssAlign = 0x8;
            header->_commandOffset = 0;

            header->_bssSize = 0;
            header->_bssSection = 0;

            RELSectionEntry* sections = (RELSectionEntry*) (address + RELHeader.Size);
            VoidPtr dataAddr = address + RELHeader.Size + Children.Count * RELSectionEntry.Size;
            foreach (ModuleSectionNode s in Children)
            {
                if (s._dataBuffer.Length != 0)
                {
                    int i = s.Index;

                    sections[i]._size = (uint) (s._calcSize - (s.ExpandSection ? 0 : s.EndBufferSize));

                    //Align sections 4 and 5?
                    //if (i > 3)
                    //{
                    //    int off = (int)(dataAddr - address);
                    //    int aligned = off.Align(8);
                    //    int diff = aligned - off;
                    //    dataAddr += diff;
                    //}

                    if (!s._isBSSSection)
                    {
                        sections[i]._offset = (uint) (dataAddr - address);
                        sections[i].IsCodeSection = s.HasCode;

                        s.Rebuild(dataAddr, s._calcSize, true);

                        dataAddr += s._calcSize;
                    }
                    else
                    {
                        sections[i]._offset = 0;

                        header->_bssSection = 0; //This is always 0 it seems
                        header->_bssSize = (uint) (s._calcSize - (s.ExpandSection ? 0 : s.EndBufferSize));
                    }
                }
            }

            RELImportEntry* imports = (RELImportEntry*) dataAddr;
            header->_impOffset = (uint) (dataAddr - address);
            dataAddr = (VoidPtr) imports + (header->_impSize = (uint) _imports.Keys.Count * RELImportEntry.Size);
            header->_relOffset = (uint) (dataAddr - address);

            List<uint> k = new List<uint>();
            foreach (uint s in _imports.Keys)
            {
                if (s != ModuleID && s != 0)
                {
                    k.Add(s);
                }
            }

            k.Sort();

            foreach (uint s in _imports.Keys)
            {
                if (s == ModuleID)
                {
                    k.Add(s);
                    break;
                }
            }

            foreach (uint s in _imports.Keys)
            {
                if (s == 0)
                {
                    k.Add(s);
                    break;
                }
            }

            for (int i = 0; i < k.Count; i++)
            {
                uint id = k[i];
                uint offset = (uint) (dataAddr - address);
                if (id == ModuleID)
                {
                    header->_commandOffset = offset;
                }

                imports[i]._moduleId = id;
                imports[i]._offset = offset;

                RELLink* link = (RELLink*) dataAddr;
                foreach (RELLink n in _imports[k[i]])
                {
                    *link++ = n;
                }

                dataAddr = link;
            }

            // Stage module conversion
            byte* bptr = (byte*) address;
            if (_stageID != null)
            {
                bptr[findStageIDOffset()] = _stageID.Value;
            }
        }

        public static Dictionary<uint, RELNode> _files = new Dictionary<uint, RELNode>();

        #region Stage module conversion

        private static int arrayIndexOf(void* haystack, int length, byte[] needle)
        {
            byte?[] b = new byte?[needle.Length];
            for (int i = 0; i < b.Length; i++)
            {
                b[i] = needle[i];
            }

            return arrayIndexOf(haystack, length, b);
        }

        private static int arrayIndexOf(void* haystack, int length, byte?[] needle)
        {
            byte* ptr = (byte*) haystack;
            int indexToCheck = 0;
            for (int i = 0; i < length; i++)
            {
                byte? b = needle[indexToCheck];
                if ((b ?? ptr[i]) == ptr[i])
                {
                    indexToCheck++;
                    if (indexToCheck == needle.Length)
                    {
                        return i + 1 - needle.Length;
                    }
                }
                else
                {
                    indexToCheck = 0;
                }
            }

            return -1;
        }

        private int findStageIDOffset()
        {
            byte?[] searchFor =
            {
                0x38, null, 0x00, null,
                0x38, 0xA5, 0x00, 0x00,
                0x38, 0x80, 0x00
            };
            int index = arrayIndexOf(WorkingUncompressed.Address, WorkingUncompressed.Length, searchFor);
            return index < 0
                ? -1
                : index + 11;
        }

        private bool nodeContainsString(string s)
        {
            return arrayIndexOf(WorkingUncompressed.Address,
                WorkingUncompressed.Length,
                Encoding.UTF8.GetBytes(s)) > 0;
        }

        /* These are section 1 offsets*/
        private static readonly int[] OTrainItemOffsets =
        {
            // Changing some values but not others has strange effects
            0x3FB,
            0x477, // this appears to be some sort of "if" condition
            0x48F,
            0x58F
        };

        /* These are section 1 offsets*/
        private static readonly int[] DxGreensItemOffsets =
        {
            0x1587,
            0xB923,
            0xB993
        };

        /* These are section 1 offsets*/
        private static readonly int[] EarthItemOffsets =
        {
            0x29AF,
            0x878F,
            0xF78F
        };

        /* These are section 1 offsets*/
        private static readonly int[] CrayonItemOffsets =
        {
            0x4743,
            0x3787
        };

        /* These are section 1 offsets*/
        private static readonly int[] VillageItemOffsets =
        {
            0x7083,
            0x74c3
        };

        #endregion

        internal static ResourceNode TryParse(DataSource source, ResourceNode parent)
        {
            RELHeader* header = (RELHeader*) source.Address;
            return header->_info._id <= 0x7E &&
                   header->_info._numSections <= 20 &&
                   header->_bssAlign == 8 &&
                   header->_moduleAlign == 32
                ? new RELNode()
                : null;
        }

        #region Module ID Names

        public static SortedList<uint, string> _idNames = new SortedList<uint, string>
        {
            {0, "main.dol"},
            {1, "sora_scene"},
            {2, "sora_menu_main"},
            {3, "sora_menu_tour"},
            {4, "sora_menu_qm"},
            {5, "sora_menu_edit"},
            {6, "sora_menu_collect_viewer"},
            {7, "sora_menu_replay"},
            {8, "sora_menu_snap_shot"},
            {9, "sora_menu_event"},
            {10, "sora_menu_sel_char"},
            {11, "sora_menu_sel_stage"},
            {12, "sora_menu_game_over"},
            {13, "sora_menu_intro"},
            {14, "sora_menu_friend_list"},
            {15, "sora_menu_watch"},
            {16, "sora_menu_name"},
            {17, "sora_menu_sel_char_access"},
            {18, "sora_menu_rule"},
            {19, "sora_menu_simple_ending"},
            {20, "sora_minigame"},
            {21, "sora_menu_time_result"},
            {22, "sora_menu_boot"},
            {23, "sora_menu_challenger"},
            {24, "sora_menu_title"},
            {25, "sora_menu_title_sunset"},
            {26, "sora_menu_fig_get_demo"},
            {27, "sora_melee"},
            {28, "sora_adv_menu_name"},
            {29, "sora_adv_menu_visual"},
            {30, "sora_adv_menu_sel_char"},
            {31, "sora_adv_menu_sel_map"},
            {32, "sora_adv_menu_difficulty"},
            {33, "sora_adv_menu_game_over"},
            {34, "sora_adv_menu_result"},
            {35, "sora_adv_menu_save_load"},
            {36, "sora_adv_menu_seal"},
            {37, "sora_adv_menu_ending"},
            {38, "sora_adv_menu_telop"},
            {39, "sora_adv_menu_save_point"},
            {40, "sora_adv_stage"},
            {41, "sora_enemy"},
            {42, "st_battles"},
            {43, "st_battle"},
            {44, "st_config"},
            {45, "st_final"},
            {46, "st_dolpic"},
            {47, "st_mansion"},
            {48, "st_mariopast"},
            {49, "st_kart"},
            {50, "st_donkey"},
            {51, "st_jungle"},
            {52, "st_pirates"},
            {53, "st_oldin"},
            {54, "st_norfair"},
            {55, "st_orpheon"},
            {56, "st_crayon"},
            {57, "st_halberd"},
            {58, "st_starfox"},
            {59, "st_stadium"},
            {60, "st_tengan"},
            {61, "st_fzero"},
            {62, "st_ice"},
            {63, "st_gw"},
            {64, "st_emblem"},
            {65, "st_madein"},
            {66, "st_earth"},
            {67, "st_palutena"},
            {68, "st_famicom"},
            {69, "st_newpork"},
            {70, "st_village"},
            {71, "st_metalgear"},
            {72, "st_greenhill"},
            {73, "st_pictchat"},
            {74, "st_plankton"},
            {75, "st_dxshrine"},
            {76, "st_dxyorster"},
            {77, "st_dxgarden"},
            {78, "st_dxonett"},
            {79, "st_dxgreens"},
            {80, "st_dxrcruise"},
            {81, "st_dxbigblue"},
            {82, "st_dxcorneria"},
            {83, "st_dxpstadium"},
            {84, "st_dxzebes"},
            {85, "st_stageedit"},
            {86, "st_otrain"},
            {87, "st_heal"},
            {88, "st_homerun"},
            {89, "st_tbreak"},
            {90, "st_croll"},
            {91, "ft_mario"},
            {92, "ft_donkey"},
            {93, "ft_link"},
            {94, "ft_samus"},
            {95, "ft_yoshi"},
            {96, "ft_kirby"},
            {97, "ft_fox"},
            {98, "ft_pikachu"},
            {99, "ft_luigi"},
            {100, "ft_captain"},
            {101, "ft_ness"},
            {102, "ft_koopa"},
            {103, "ft_peach"},
            {104, "ft_zelda"},
            {105, "ft_iceclimber"},
            {106, "ft_marth"},
            {107, "ft_gamewatch"},
            {108, "ft_falco"},
            {109, "ft_ganon"},
            {110, "ft_wario"},
            {111, "ft_metaknight"},
            {112, "ft_pit"},
            {113, "ft_pikmin"},
            {114, "ft_lucas"},
            {115, "ft_diddy"},
            {116, "ft_poke"},
            {117, "ft_dedede"},
            {118, "ft_lucario"},
            {119, "ft_ike"},
            {120, "ft_robot"},
            {121, "ft_toonlink"},
            {122, "ft_snake"},
            {123, "ft_sonic"},
            {124, "ft_purin"},
            {125, "ft_wolf"},
            {126, "ft_zako"}
        };

        #endregion
    }
}