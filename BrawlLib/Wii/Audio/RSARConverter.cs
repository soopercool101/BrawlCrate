using BrawlLib.Internal;
using BrawlLib.SSBB.ResourceNodes;
using BrawlLib.SSBB.Types;
using BrawlLib.SSBB.Types.Audio;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace BrawlLib.Wii.Audio
{
    public unsafe class RSARConverter
    {
        public int _headerLen, _fileLen, _infoLen, _symbLen;

        internal int CalculateSize(RSAREntryList entries, RSARNode node)
        {
            //Header
            _headerLen = 0x40;

            //SYMB, INFO, FILE Headers
            _symbLen = 0x20;
            _infoLen = 0x8;
            _fileLen = 0x20;

            #region SYMB

            //String offsets
            _symbLen += entries._strings.Count * 4;

            //Strings are packed tightly with no trailing pad
            _symbLen += entries._stringLength;

            //Mask entries
            _symbLen += 32;                                    //Headers
            _symbLen += (entries._strings.Count * 2 - 4) * 20; //Entries

            //Align
            _symbLen = _symbLen.Align(0x20);

            #endregion

            #region Info

            //Info ruint collection and ruint list counts
            _infoLen += 0x30;

            int sounds = 4, playerInfo = 4, banks = 4, groups = 4, files = 4;

            //ruint sizes
            sounds += entries._sounds.Count * 8;
            playerInfo += entries._playerInfo.Count * 8;
            groups += entries._groups.Count * 8;
            banks += entries._banks.Count * 8 + 8;
            files += entries._files.Count * 8;

            //Evaluate entries with child offsets
            foreach (RSAREntryNode s in entries._sounds)
            {
                sounds += s.CalculateSize(true);
            }

            foreach (RSAREntryNode s in entries._playerInfo)
            {
                playerInfo += s.CalculateSize(true);
            }

            foreach (RSAREntryNode s in entries._banks)
            {
                banks += s.CalculateSize(true);
            }

            foreach (RSAREntryNode s in entries._groups)
            {
                groups += s.CalculateSize(true);
            }

            foreach (RSARFileNode s in entries._files)
            {
                files += INFOFileHeader.Size + 4 + (!(s is RSARExtFileNode)
                    ? s._groupRefs.Count * (8 + INFOFileEntry.Size)
                    : (((RSARExtFileNode) s)._extPath.Length + 1).Align(4));
            }

            //Footer and Align
            _infoLen = ((_infoLen += sounds + banks + playerInfo + files + groups) + 0x10).Align(0x20);

            #endregion

            #region File

            foreach (RSARGroupNode g in entries._groups)
            {
                foreach (RSARFileNode f in g._files)
                {
                    _fileLen += f.CalculateSize(true);
                }
            }

            //Align
            _fileLen = _fileLen.Align(0x20);

            #endregion

            return _headerLen + _symbLen + _infoLen + _fileLen;
        }

        internal int EncodeSYMBBlock(SYMBHeader* header, RSAREntryList entries, RSARNode node)
        {
            int len = 0;
            int count = entries._strings.Count;
            VoidPtr baseAddr = (VoidPtr) header + 8, dataAddr;
            bint* strEntry = (bint*) (baseAddr + 0x18);
            PString pStr = (byte*) strEntry + (count << 2);

            //Strings
            header->_stringOffset = 0x14;
            strEntry[-1] = entries._strings.Count;
            foreach (string s in entries._strings)
            {
                *strEntry++ = pStr - baseAddr;
                pStr.Write(s, 0, s.Length + 1);
                pStr += s.Length + 1;
            }

            dataAddr = pStr;

            //Sounds
            header->_maskOffset1 = dataAddr - baseAddr;
            dataAddr += EncodeMaskGroup(header, (SYMBMaskHeader*) dataAddr, entries._sounds, node, 0);

            //Player Info
            header->_maskOffset2 = dataAddr - baseAddr;
            dataAddr += EncodeMaskGroup(header, (SYMBMaskHeader*) dataAddr, entries._playerInfo, node, 1);

            //Groups
            header->_maskOffset3 = dataAddr - baseAddr;
            dataAddr += EncodeMaskGroup(header, (SYMBMaskHeader*) dataAddr, entries._groups, node, 2);

            //Banks
            header->_maskOffset4 = dataAddr - baseAddr;
            dataAddr += EncodeMaskGroup(header, (SYMBMaskHeader*) dataAddr, entries._banks, node, 3);

            int temp = (int) dataAddr - (int) header;
            len = temp.Align(0x20);

            //Fill padding
            byte* p = (byte*) dataAddr;
            for (int i = temp; i < len; i++)
            {
                *p++ = 0;
            }

            //Set header
            header->_header._tag = SYMBHeader.Tag;
            header->_header._length = len;

            return len;
        }

        internal int EncodeINFOBlock(INFOHeader* header, RSAREntryList entries, RSARNode node)
        {
            int len = 0;

            VoidPtr baseAddr = header->_collection.Address;
            ruint* values = (ruint*) baseAddr;
            VoidPtr dataAddr = baseAddr + 0x30;
            RuintList* entryList;
            int index = 0;

            //Set up sound ruint list
            values[0] = (uint) dataAddr - (uint) baseAddr;
            entryList = (RuintList*) dataAddr;
            entryList->_numEntries = entries._sounds.Count;
            dataAddr += entries._sounds.Count * 8 + 4;

            //Write sound entries
            foreach (RSAREntryNode r in entries._sounds)
            {
                r._rebuildBase = baseAddr;
                entryList->Entries[index++] = (uint) dataAddr - (uint) baseAddr;
                r.Rebuild(dataAddr, r._calcSize, true);
                dataAddr += r._calcSize;
            }

            index = 0;
            //Set up bank ruint list
            values[1] = (uint) dataAddr - (uint) baseAddr;
            entryList = (RuintList*) dataAddr;
            entryList->_numEntries = entries._banks.Count;
            dataAddr += entries._banks.Count * 8 + 4;

            //Write bank entries
            foreach (RSAREntryNode r in entries._banks)
            {
                r._rebuildBase = baseAddr;
                entryList->Entries[index++] = (uint) dataAddr - (uint) baseAddr;
                r.Rebuild(dataAddr, r._calcSize, true);
                dataAddr += r._calcSize;
            }

            index = 0;
            //Set up playerInfo ruint list
            values[2] = (uint) dataAddr - (uint) baseAddr;
            entryList = (RuintList*) dataAddr;
            entryList->_numEntries = entries._playerInfo.Count;
            dataAddr += entries._playerInfo.Count * 8 + 4;

            //Write playerInfo entries
            foreach (RSAREntryNode r in entries._playerInfo)
            {
                r._rebuildBase = baseAddr;
                entryList->Entries[index++] = (uint) dataAddr - (uint) baseAddr;
                r.Rebuild(dataAddr, r._calcSize, true);
                dataAddr += r._calcSize;
            }

            index = 0;
            //Set up file ruint list
            values[3] = (uint) dataAddr - (uint) baseAddr;
            entryList = (RuintList*) dataAddr;
            entryList->_numEntries = entries._files.Count;
            dataAddr += entries._files.Count * 8 + 4;

            //Write file entries
            foreach (RSARFileNode file in entries._files)
            {
                //if (file._groupRefs.Count == 0 && !(file is RSARExtFileNode))
                //    continue;

                entryList->Entries[index++] = (uint) dataAddr - (uint) baseAddr;
                INFOFileHeader* fileHdr = (INFOFileHeader*) dataAddr;
                dataAddr += INFOFileHeader.Size;
                RuintList* list = (RuintList*) dataAddr;
                fileHdr->_entryNumber = -1;
                if (file is RSARExtFileNode)
                {
                    uint extFileSize = 0;

                    RSARExtFileNode ext = file as RSARExtFileNode;

                    //Make an attempt to get current file size
                    if (ext.ExternalFileInfo.Exists)
                    {
                        extFileSize = (uint) ext.ExternalFileInfo.Length;
                    }

                    if (ext._extFileSize != extFileSize && extFileSize != 0)
                    {
                        ext._extFileSize = extFileSize;
                    }

                    //Shouldn't matter if 0
                    fileHdr->_headerLen = ext._extFileSize;

                    fileHdr->_dataLen = 0;
                    fileHdr->_stringOffset = (uint) (list - baseAddr);

                    sbyte* dPtr = (sbyte*) list;
                    ext._extPath.Write(ref dPtr);
                    dataAddr += (dPtr - dataAddr).Align(4);

                    fileHdr->_listOffset = (uint) (dataAddr - baseAddr);
                    dataAddr += 4; //Empty list
                }
                else
                {
                    fileHdr->_headerLen = (uint) file._headerLen;
                    fileHdr->_dataLen = (uint) file._audioLen;
                    //fileHdr->_stringOffset = 0;
                    fileHdr->_listOffset = (uint) (list - baseAddr);
                    list->_numEntries = file._groupRefs.Count;
                    INFOFileEntry* fileEntries = (INFOFileEntry*) ((VoidPtr) list + 4 + file._groupRefs.Count * 8);
                    int z = 0;
                    List<int> used = new List<int>();
                    foreach (RSARGroupNode g in file._groupRefs)
                    {
                        list->Entries[z] = (uint) (&fileEntries[z] - baseAddr);
                        fileEntries[z]._groupId = g._rebuildIndex;
                        int[] all = g._files.FindAllOccurences(file);
                        bool done = false;
                        foreach (int i in all)
                        {
                            if (!used.Contains(i))
                            {
                                fileEntries[z]._index = i;
                                used.Add(i);
                                done = true;
                                break;
                            }
                        }

                        if (!done)
                        {
                            fileEntries[z]._index = g._files.IndexOf(file);
                        }

                        z++;
                    }

                    dataAddr = (VoidPtr) fileEntries + file._groupRefs.Count * INFOFileEntry.Size;
                }
            }

            index = 0;
            //Set up group ruint list
            values[4] = (uint) dataAddr - (uint) baseAddr;
            entryList = (RuintList*) dataAddr;
            entryList->_numEntries = entries._groups.Count;
            dataAddr += entries._groups.Count * 8 + 4;

            //Write group entries
            foreach (RSAREntryNode r in entries._groups)
            {
                r._rebuildBase = baseAddr;
                entryList->Entries[index++] = (uint) dataAddr - (uint) baseAddr;
                r.Rebuild(dataAddr, r._calcSize, true);
                dataAddr += r._calcSize;
            }

            //Write footer
            values[5] = (uint) dataAddr - (uint) baseAddr;
            *(INFOFooter*) dataAddr = node._ftr;

            //Set header
            header->_header._tag = INFOHeader.Tag;
            header->_header._length = len = (dataAddr + INFOFooter.Size - (baseAddr - 8)).Align(0x20);

            return len;
        }

        internal int EncodeFILEBlock(FILEHeader* header, VoidPtr baseAddress, RSAREntryList entries, RSARNode node)
        {
            int len = 0;
            VoidPtr baseAddr = (VoidPtr) header + 0x20;
            VoidPtr addr = baseAddr;

            //Build files - order by groups
            foreach (RSARGroupNode g in entries._groups)
            {
                int headerLen = 0, audioLen = 0, i = 0;
                INFOGroupEntry* e =
                    (INFOGroupEntry*) ((VoidPtr) g._headerAddr + INFOGroupHeader.Size + 4 + g._files.Count * 8);
                g._headerAddr->_headerOffset = addr - baseAddress;
                foreach (RSARFileNode f in g._files)
                {
                    e[i]._headerLength = f._headerLen;
                    e[i]._headerOffset = headerLen;

                    headerLen += f._headerLen;

                    ++i;
                }

                i = 0;
                VoidPtr wave = addr + headerLen;
                g._headerAddr->_waveDataOffset = wave - baseAddress;
                foreach (RSARFileNode f in g._files)
                {
                    f._rebuildAudioAddr = wave + audioLen;
                    f.Rebuild(addr, f._headerLen, true);

                    e[i]._dataOffset = f._audioLen == 0 ? 0 : audioLen;
                    e[i]._dataLength = f._audioLen;

                    addr += f._headerLen;
                    audioLen += f._audioLen;

                    ++i;
                }

                addr += audioLen;
                g._headerAddr->_headerLength = headerLen;
                g._headerAddr->_waveDataLength = audioLen;
            }

            len = ((int) addr - (int) (VoidPtr) header).Align(0x20);

            //Set header
            header->_header._tag = FILEHeader.Tag;
            header->_header._length = len;

            return len;
        }

        private static int EncodeMaskGroup(SYMBHeader* symb, SYMBMaskHeader* header, List<RSAREntryNode> gList,
                                           RSARNode n, int grp)
        {
            int[] stringIds = gList.Select(x => x._rebuildStringId).Where(x => x >= 0).ToArray();
            SYMBMaskEntry.Build(stringIds, symb, header, header->Entries);
            return SYMBMaskHeader.Size + (stringIds.Length * 2 - 1) * SYMBMaskEntry.Size;
        }
    }

    public class RSARStringEntryState
    {
        public int _type;
        public int _index;
        public string _name;
    }

    public class RSAREntryList
    {
        public int _stringLength;
        public List<string> _strings = new List<string>();
        public List<RSARStringEntryState> _tempStrings = new List<RSARStringEntryState>();
        public List<RSAREntryNode> _sounds = new List<RSAREntryNode>();
        public List<RSAREntryNode> _playerInfo = new List<RSAREntryNode>();
        public List<RSAREntryNode> _groups = new List<RSAREntryNode>();
        public List<RSAREntryNode> _banks = new List<RSAREntryNode>();
        public BindingList<RSARFileNode> _files;

        public void AddEntry(string path, RSAREntryNode node)
        {
            RSARStringEntryState str = new RSARStringEntryState();

            if (node._name != "<null>")
            {
                str._name = path;
            }
            else
            {
                str._name = null;
            }

            int type = -1;
            List<RSAREntryNode> group;
            if (node is RSARSoundNode)
            {
                group = _sounds;
                type = 3;
            }
            else if (node is RSARGroupNode)
            {
                group = _groups;
                type = 1;
            }
            else if (node is RSARPlayerInfoNode)
            {
                group = _playerInfo;
                type = 0;
            }
            else
            {
                group = _banks;
                type = 2;
            }

            str._type = type;
            str._index = node._infoIndex;

            group.Add(node);

            if (string.IsNullOrEmpty(str._name))
            {
                node._rebuildStringId = -1;
            }
            else
            {
                _tempStrings.Add(str);
            }
        }

        public void Clear()
        {
            _sounds.Clear();
            _playerInfo.Clear();
            _groups.Clear();
            _banks.Clear();
            _stringLength = 0;
            _strings = new List<string>();
            _tempStrings = new List<RSARStringEntryState>();
        }

        internal void SortStrings()
        {
            _stringLength = 0;
            _strings = new List<string>();

            for (int i = 0; i < 4; ++i)
            {
                _strings.AddRange(_tempStrings
                    .Where(x => x._type == i)
                    .OrderBy(x => x._index)
                    .Select(x => x._name
                        .ToString()));
            }

            foreach (string s in _strings)
            {
                _stringLength += s.Length + 1;
            }

            foreach (RSAREntryNode s in _sounds)
            {
                s._rebuildStringId = _strings.IndexOf(s._fullPath);
            }

            foreach (RSAREntryNode s in _playerInfo)
            {
                s._rebuildStringId = _strings.IndexOf(s._fullPath);
            }

            foreach (RSAREntryNode s in _groups)
            {
                s._rebuildStringId = _strings.IndexOf(s._fullPath);
            }

            foreach (RSAREntryNode s in _banks)
            {
                s._rebuildStringId = _strings.IndexOf(s._fullPath);
            }

            _sounds.Sort(Compare);
            _playerInfo.Sort(Compare);
            _groups.Sort(Compare);
            _banks.Sort(Compare);

            int m = 0;
            foreach (RSAREntryNode r in _sounds)
            {
                r._rebuildIndex = m++;
            }

            m = 0;
            foreach (RSAREntryNode r in _playerInfo)
            {
                r._rebuildIndex = m++;
            }

            m = 0;
            foreach (RSAREntryNode r in _groups)
            {
                r._rebuildIndex = m++;
            }

            m = 0;
            foreach (RSAREntryNode r in _banks)
            {
                r._rebuildIndex = m++;
            }
        }

        public static int Compare(RSAREntryNode n1, RSAREntryNode n2)
        {
            return n2._infoIndex < n1._infoIndex ? 1 : n2._infoIndex > n1._infoIndex ? -1 : 0;
        }
    }
}