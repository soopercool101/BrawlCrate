using BrawlLib.Internal;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace BrawlLib.SSBB.Types.Audio
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal unsafe struct RSARHeader
    {
        public const int Size = 0x40;
        public const uint Tag = 0x52415352;

        public NW4RCommonHeader _header;

        public bint _symbOffset;
        public bint _symbLength;
        public bint _infoOffset;
        public bint _infoLength;
        public bint _fileOffset;
        public bint _fileLength;
        private readonly int _pad1, _pad2, _pad3, _pad4, _pad5, _pad6;

        public void Set(int symbLen, int infoLen, int fileLen, byte vMinor)
        {
            int offset = 0x40;

            _header._tag = Tag;
            _header.Endian = Endian.Big;
            _header._version = (ushort) (0x100 + vMinor);
            _header._firstOffset = 0x40;
            _header._numEntries = 3;

            _symbOffset = offset;
            _symbLength = symbLen;
            _infoOffset = offset += symbLen;
            _infoLength = infoLen;
            _fileOffset = offset += infoLen;
            _fileLength = fileLen;

            _header._length = offset + fileLen;
        }

        private VoidPtr Address
        {
            get
            {
                fixed (RSARHeader* ptr = &this)
                {
                    return ptr;
                }
            }
        }

        public SYMBHeader* SYMBBlock => (SYMBHeader*) _header.Entries[0].Address;
        public INFOHeader* INFOBlock => (INFOHeader*) _header.Entries[1].Address;
        public FILEHeader* FILEBlock => (FILEHeader*) _header.Entries[2].Address;
    }

    #region SYMB

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct SYMBHeader
    {
        public const uint Tag = 0x424D5953;

        public SSBBEntryHeader _header;
        public bint _stringOffset;

        public bint _maskOffset1; //For sounds
        public bint _maskOffset2; //For types
        public bint _maskOffset3; //For groups
        public bint _maskOffset4; //For banks

        public SYMBHeader(int length)
        {
            _header._tag = Tag;
            _header._length = length;
            _stringOffset = 0x14;
            _maskOffset1 = _maskOffset2 = _maskOffset3 = _maskOffset4 = 0;
        }

        private VoidPtr Address
        {
            get
            {
                fixed (void* ptr = &this)
                {
                    return ptr;
                }
            }
        }

        //public VoidPtr StringData { get { return Address + 8 + _stringOffset; } }
        public SYMBMaskHeader* MaskData1 => (SYMBMaskHeader*) (Address + 8 + _maskOffset1);
        public SYMBMaskHeader* MaskData2 => (SYMBMaskHeader*) (Address + 8 + _maskOffset2);
        public SYMBMaskHeader* MaskData3 => (SYMBMaskHeader*) (Address + 8 + _maskOffset3);
        public SYMBMaskHeader* MaskData4 => (SYMBMaskHeader*) (Address + 8 + _maskOffset4);

        public uint StringCount => StringOffsets[-1];
        public buint* StringOffsets => (buint*) (Address + 8 + _stringOffset + 4);

        //Gets names of file paths separated by an underscore
        public string GetStringEntry(int index)
        {
            if (index < 0)
            {
                return "<null>";
            }

            return new string(GetStringEntryAddr(index));
        }

        public sbyte* GetStringEntryAddr(int index)
        {
            return (sbyte*) (Address + 8 + StringOffsets[index]);
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct SYMBMaskHeader
    {
        public const int Size = 8;

        public bint _rootId; //index of the first entry non leafed entry with the lowest bit value
        public bint _numEntries;

        private VoidPtr Address
        {
            get
            {
                fixed (SYMBMaskHeader* ptr = &this)
                {
                    return ptr;
                }
            }
        }

        public SYMBMaskEntry* Entries => (SYMBMaskEntry*) (Address + 8);
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct SYMBMaskEntry //Like a ResourceEntry
    {
        public const int Size = 0x14;

        public bushort _flags;
        public bshort _bit;   //ResourceEntry _id
        public bint _leftId;  //ResourceEntry _leftIndex
        public bint _rightId; //ResourceEntry _rightIndex
        public bint _stringId;
        public bint _index;

        public SYMBMaskEntry(short bit, int left, int right) : this(0, bit, left, right, 0, 0)
        {
        }

        public SYMBMaskEntry(ushort flags, short bit, int left, int right, int id, int index)
        {
            _flags = flags;
            _bit = bit;
            _leftId = left;
            _rightId = right;
            _stringId = id;
            _index = index;
        }

        private SYMBMaskEntry* Address
        {
            get
            {
                fixed (SYMBMaskEntry* ptr = &this)
                {
                    return ptr;
                }
            }
        }

        public SYMBMaskHeader* Parent
        {
            get
            {
                SYMBMaskEntry* entry = Address;
                while (entry->_flags != 1 && entry[-1]._flags != 1)
                {
                    entry--;
                }

                return (SYMBMaskHeader*) ((VoidPtr) (--entry) - 8);
            }
        }

        //Code written by Mawootad
        public static void Build(int[] indices, SYMBHeader* header, SYMBMaskHeader* maskHeader, SYMBMaskEntry* entries)
        {
            //initialization
            maskHeader->_rootId = 0;
            maskHeader->_numEntries = 0;

            //Loop over indicies and add them.  This seems to be roughly how the file is normally built, as it has the same resulting leaf-node-leaf-node pattern
            foreach (int id in indices)
            {
                AddToTrie(entries, maskHeader, id, header);
            }
        }

        private static bool CheckBit(string val, int bit)
        {
            //Returns true if the given bit in the string is 1 and false if the bit is 0
            return (val[bit / 8] & ((1 << 7) >> (bit % 8))) != 0;
        }

        //Lookup table to compute the 
        public static readonly byte[] clz8 =
        {
            8, 7, 6, 6, 5, 5, 5, 5, 4, 4, 4, 4, 4, 4, 4, 4,
            3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3,
            2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2,
            2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2,
            1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
            1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
            1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
            1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0
        };

        private static void AddToTrie(SYMBMaskEntry* trie, SYMBMaskHeader* head, int id, SYMBHeader* table)
        {
            //If list is empty add the node and quit
            if (head->_numEntries == 0)
            {
                trie[head->_numEntries++] = new SYMBMaskEntry(1, -1, -1, -1, id, 0);
                return;
            }

            string value = table->GetStringEntry(id);

            //String.IsNullOrEmpty(value)
            if ((value ?? "") == "")
            {
                throw new ArgumentException("String is null or whitespace");
            }

            SYMBMaskEntry search = trie[head->_rootId];
            List<int> path = new List<int>
            {
                head->_rootId
            };

            //Find the string that matches the current string in the trie.  Needs to be done in order to determine where the important bit is in the string
            while (search._flags == 0)
            {
                //Assume that strings are treated as having an infinite number of null chars following them
                if (search._bit / 8 >= value.Length)
                {
                    path.Add(search._leftId);
                    search = trie[search._leftId];
                    continue;
                }

                // _leftId corresponds to bit=0, _rightId corresponds to bit=1
                if (CheckBit(value, search._bit))
                {
                    path.Add(search._rightId);
                    search = trie[search._rightId];
                }
                else
                {
                    path.Add(search._leftId);
                    search = trie[search._leftId];
                }
            }

            string searchVal = table->GetStringEntry(search._stringId);

            //Can't add duplicate strings
            if (searchVal == value)
            {
                throw new ArgumentException("Duplicate string");
            }

            bool mismatch = false;
            int minLength = Math.Min(searchVal.Length, value.Length);
            short bit = 0;

            //Locate mismatching character between the two strings
            for (short i = 0; i < minLength; i++)
            {
                if (value[i] != searchVal[i])
                {
                    mismatch = true;
                    bit = (short) (8 * i);
                    break;
                }
            }

            bool right;

            //If a char was different one string does not contain the other
            if (mismatch)
            {
                //Find where the bits differed
                int cmpint = value[bit / 8] ^ searchVal[bit / 8];
                bit += clz8[cmpint];

                //If the bit is 1 the string being added takes the left fork
                right = CheckBit(value, bit);

                if (head->_numEntries == 1)
                {
                    trie[1] = new SYMBMaskEntry(1, -1, -1, -1, id, 1);
                    trie[2] = new SYMBMaskEntry(0, bit, right ? 0 : 1, right ? 1 : 0, -1, -1);
                    head->_numEntries = 3;
                    head->_rootId = 2;
                    return;
                }

                //If the mismatch bit is lower than the first mismatch bit the new branch will be the root of the tree
                if (bit < trie[path[0]]._bit)
                {
                    trie[head->_numEntries++] = new SYMBMaskEntry(1, -1, -1, -1, id, head->_numEntries / 2);

                    if (right)
                    {
                        trie[head->_numEntries++] = new SYMBMaskEntry(0, bit, path[0], head->_numEntries - 2, -1, -1);
                    }
                    else
                    {
                        trie[head->_numEntries++] = new SYMBMaskEntry(0, bit, head->_numEntries - 2, path[0], -1, -1);
                    }

                    head->_rootId = head->_numEntries - 1;
                    return;
                }

                //Locate where the branch needs to be inserted
                for (int i = 1; i < path.Count; i++)
                {
                    if (trie[path[i]]._bit > bit || trie[path[i]]._flags == 1)
                    {
                        //Add leaf
                        trie[head->_numEntries++] = new SYMBMaskEntry(1, -1, -1, -1, id, head->_numEntries / 2);

                        //Remap previous branch to point to new branch
                        if (trie[path[i - 1]]._leftId == path[i])
                        {
                            trie[path[i - 1]]._leftId = head->_numEntries;
                        }
                        else
                        {
                            trie[path[i - 1]]._rightId = head->_numEntries;
                        }

                        //Create new branch
                        if (right)
                        {
                            trie[head->_numEntries++] =
                                new SYMBMaskEntry(0, bit, path[i], head->_numEntries - 2, -1, -1);
                        }
                        else
                        {
                            trie[head->_numEntries++] =
                                new SYMBMaskEntry(0, bit, head->_numEntries - 2, path[i], -1, -1);
                        }

                        return;
                    }
                }

                //This should never happen
                throw new Exception("Error building tree, unexpected structure");
            }

            //Since mismatch is false, one string is a substring of the other

            //The longer string is the one that takes the left branch
            right = value.Length > searchVal.Length;
            bit = (short) (minLength * 8);

            if (right)
            {
                //Find the first bit after the substring that's 1.  Will always occur in the first 8 bits because 0x00 denotes string termination and thus isn't in value
                bit += clz8[value[bit / 8]];

                //If path.Count == 1 the only value is a leaf
                if (path.Count == 1)
                {
                    trie[1] = new SYMBMaskEntry(1, -1, -1, -1, id, 1);
                    trie[2] = new SYMBMaskEntry(0, bit, 0, 1, -1, -1);
                    head->_numEntries = 3;
                    head->_rootId = 2;
                    return;
                }

                //Update old branch, insert new branch and node, and quit.  trie[path[path.Count-2]] is the last branch that was a comparison.
                trie[head->_numEntries++] = new SYMBMaskEntry(1, -1, -1, -1, id, head->_numEntries / 2);

                int trace = path.Count - 2;

                if (trie[path[trace]]._leftId == path[trace + 1])
                {
                    //Handling an extremely specific and annoying edge case
                    while (trie[path[trace]]._bit > bit)
                    {
                        trace--;
                        if (trace < 0)
                        {
                            //This node is actually the root of the tree
                            trie[head->_numEntries++] =
                                new SYMBMaskEntry(0, bit, path[0], head->_numEntries - 2, -1, -1);
                            head->_rootId = head->_numEntries - 2;
                            return;
                        }
                    }

                    trie[path[trace]]._leftId = head->_numEntries;
                }
                else
                {
                    trie[path[trace]]._rightId = head->_numEntries;
                }

                trie[head->_numEntries++] = new SYMBMaskEntry(0, bit, path[trace + 1], head->_numEntries - 2, -1, -1);
                return;
            }

            //Find first bit comparison that happens after the substring ends
            int index;
            for (index = 0; trie[path[index]]._flags == 0 && trie[path[index]]._bit <= bit; index++)
            {
            }

            //Find the first bit that's 1 and isn't already used in the trie
            int cmpVal = searchVal[bit / 8];
            byte clzVal;
            bool test = trie[path[index]]._flags == 0;
            while (true)
            {
                clzVal = clz8[cmpVal];
                if (clzVal == 8)
                {
                    bit += 8;
                    cmpVal = searchVal[bit / 8];
                    continue;
                }

                if (test && trie[path[index]]._bit <= bit + clzVal)
                {
                    if (trie[path[index]]._bit == bit + clzVal)
                    {
                        cmpVal ^= (1 << 7) >> clzVal;
                    }

                    test = trie[path[++index]]._flags == 0;
                    continue;
                }

                bit += clzVal;
                break;
            }

            //If the trie is a single leaf the new branch is the root of the trie
            if (head->_numEntries == 1)
            {
                trie[1] = new SYMBMaskEntry(1, -1, -1, -1, id, 1);
                trie[2] = new SYMBMaskEntry(0, bit, 1, 0, -1, -1);
                head->_numEntries = 3;
                head->_rootId = 2;
                return;
            }

            //Update old branch, insert new branch and node, and quit
            trie[head->_numEntries++] = new SYMBMaskEntry(1, -1, -1, -1, id, head->_numEntries / 2);

            if (trie[path[index - 1]]._leftId == path[index])
            {
                trie[path[index - 1]]._leftId = head->_numEntries;
            }
            else
            {
                trie[path[index - 1]]._rightId = head->_numEntries;
            }

            trie[head->_numEntries++] = new SYMBMaskEntry(0, bit, head->_numEntries - 2, path[index], -1, -1);

            return;
        }
    }

    #endregion

    #region INFO

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal unsafe struct INFOHeader
    {
        public const uint Tag = 0x4F464E49;

        public SSBBEntryHeader _header;
        public RuintCollection _collection;

        private VoidPtr Address
        {
            get
            {
                fixed (void* ptr = &this)
                {
                    return ptr;
                }
            }
        }

        public RuintList* Sounds => (RuintList*) _collection[0];     //INFOSoundEntry
        public RuintList* Banks => (RuintList*) _collection[1];      //INFOBankEntry
        public RuintList* PlayerInfo => (RuintList*) _collection[2]; //INFOPlayerInfoEntry
        public RuintList* Files => (RuintList*) _collection[3];      //INFOFileEntry
        public RuintList* Groups => (RuintList*) _collection[4];     //INFOGroupHeader
        public INFOFooter* Footer => (INFOFooter*) _collection[5];

        public INFOSoundEntry* GetSound(int i)
        {
            return (INFOSoundEntry*) (_collection.Address + (*Sounds)[i]);
        }

        public INFOBankEntry* GetBank(int i)
        {
            return (INFOBankEntry*) (_collection.Address + (*Banks)[i]);
        }

        public INFOPlayerInfoEntry* GetPlayerInfo(int i)
        {
            return (INFOPlayerInfoEntry*) (_collection.Address + (*PlayerInfo)[i]);
        }

        public INFOFileEntry* GetFile(int i)
        {
            return (INFOFileEntry*) (_collection.Address + (*Files)[i]);
        }

        public INFOGroupHeader* GetGroup(int i)
        {
            return (INFOGroupHeader*) (_collection.Address + (*Groups)[i]);
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal struct INFOFooter
    {
        public const int Size = 0x10;

        public bushort _seqSoundCount;
        public bushort _seqTrackCount;
        public bushort _strmSoundCount;
        public bushort _strmTrackCount;
        public bushort _strmChannelCount;
        public bushort _waveSoundCount;
        public bushort _waveTrackCount;
        public bushort _padding;
        public buint _reserved;
    }

    #region Sounds

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal unsafe struct INFOSoundEntry
    {
        public const int Size = 0x2C;

        public bint _stringId;
        public bint _fileId;
        public bint _playerId; // 0
        public ruint _param3dRefOffset;
        public byte _volume;         //0x20
        public byte _playerPriority; //0x40
        public byte _soundType;
        public byte _remoteFilter;  //0x00
        public ruint _soundInfoRef; //dataType: 0 = null, 1 = SeqSoundInfo, 2 = StrmSoundInfo, 3 = WaveSoundInfo
        public bint _userParam1;
        public bint _userParam2;
        public byte _panMode;
        public byte _panCurve;
        public byte _actorPlayerId;
        public byte _reserved;

        public Sound3DParam* GetParam3dRef(VoidPtr baseAddr)
        {
            return (Sound3DParam*) (baseAddr + _param3dRefOffset);
        }

        public VoidPtr GetSoundInfoRef(VoidPtr baseAddr)
        {
            return baseAddr + _soundInfoRef;
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct Sound3DParam
    {
        public const int Size = 0xC;

        public buint _flags;
        public byte _decayCurve;
        public byte _decayRatio;
        public byte _dopplerFactor;
        public byte _padding;
        public buint _reserved;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SeqSoundInfo
    {
        public const int Size = 0x14;

        public buint _dataID;
        public bint _bankId;
        public buint _allocTrack;
        public byte _channelPriority;
        public byte _releasePriorityFix;
        public byte _pad1;
        public byte _pad2;
        public buint _reserved;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct StrmSoundInfo
    {
        public const int Size = 0xC;

        public buint _startPosition;
        public bushort _allocChannelCount; // Prior to version 0x0104, this was a bit flag
        public bushort _allocTrackFlag;
        public buint _reserved;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct WaveSoundInfo
    {
        public const int Size = 0x10;

        public bint _soundIndex;
        public buint _allocTrack;
        public byte _channelPriority;
        public byte _releasePriorityFix;
        public byte _pad1;
        public byte _pad2;
        public buint _reserved;
    }

    #endregion

    #region Banks

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal struct INFOBankEntry
    {
        public const int Size = 0xC;

        public bint _stringId;
        public bint _fileId;
        public bint _padding;
    }

    #endregion

    #region Player Info

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal struct INFOPlayerInfoEntry
    {
        public const int Size = 0x10;

        public bint _stringId;
        public byte _playableSoundCount;
        public byte _padding;     //0
        public bushort _padding2; //0
        public buint _heapSize;   //0
        public buint _reserved;   //0
    }

    #endregion

    #region Files

    //Files can be a group of raw sounds, sequences, or external audio streams.
    //Audio streams (RSTM) can be loaded as BGMs using external files, referenced by the _stringOffset field.
    //Raw sounds (RWSD) contain sounds used in action scripts (usually mono).
    //Banks (RBNK) contain sounds that are played in sequence during gameplay.
    //Sequences (RSEQ) control the progression of banks.

    //Files can be referenced multiple times using loading groups. The _listOffset field contains a list of those references.
    //When a file is referenced by a group, it is copied to each group's header and data block.

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal unsafe struct INFOFileHeader
    {
        public const int Size = 0x1C;

        public buint _headerLen;    //Includes padding. Set to file size if external file.
        public buint _dataLen;      //Includes padding. Zero if external file.
        public bint _entryNumber;   //-1
        public ruint _stringOffset; //External file path, only for BGMs. Path is relative to sound folder
        public ruint _listOffset;   //List of groups this file belongs to. Empty if external file.

        public RuintList* GetList(VoidPtr baseAddr)
        {
            return (RuintList*) (baseAddr + _listOffset);
        }

        public string GetPath(VoidPtr baseAddr)
        {
            return _stringOffset == 0 ? null : new string((sbyte*) (baseAddr + _stringOffset));
        }
    }

    //Attached to a RuintList from INFOSetHeader
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal struct INFOFileEntry
    {
        public const int Size = 0x8;

        public bint _groupId;
        public bint _index;

        public int GroupId
        {
            get => _groupId;
            set => _groupId = value;
        }

        public int Index
        {
            get => _index;
            set => _index = value;
        }

        public override string ToString()
        {
            return $"[{GroupId}, {Index}]";
        }
    }

    #endregion

    #region Groups

    //Groups are a collection of sound files.
    //Files can appear in multiple groups, but the data is actually copied to each group.
    //Groups are laid out in two blocks, first the header block, then the data block.
    //The header block holds all the headers belonging to each file, in sequential order.
    //The data block holds all the audio data belonging to each file, in sequential order.
    //Data referenced in the WAVE section is relative to the file's data, not the whole group block.
    //This means that the headers/data can simply be copied without changing anything.

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal unsafe struct INFOGroupHeader
    {
        public const int Size = 0x28;

        public bint _stringId;        //string id
        public bint _entryNum;        //always -1
        public ruint _extFilePathRef; //0
        public bint _headerOffset;    //Absolute offset from RSAR file. //RWSD Location
        public bint _headerLength;    //Total length of all headers in contained sets.
        public bint _waveDataOffset;  //Absolute offset from RSAR file.
        public bint _waveDataLength;  //Total length of all data in contained sets.
        public ruint _listOffset;

        public RuintList* GetCollection(VoidPtr offset)
        {
            return (RuintList*) (offset + _listOffset);
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal struct INFOGroupEntry
    {
        public const int Size = 0x18;

        public bint _fileId;
        public bint _headerOffset; //Offset to data, relative to headerOffset above
        public bint _headerLength; //File data length, excluding labels and audio
        public bint _dataOffset;   //Offset to audio, relative to waveDataOffset above
        public bint _dataLength;   //Audio length
        public bint _reserved;

        public override string ToString()
        {
            return $"[{(uint) _fileId:X}]";
        }
    }

    #endregion

    #endregion

    #region FILE

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal unsafe struct FILEHeader //Holds all files directly after this header
    {
        public const uint Tag = 0x454C4946;
        public const int Size = 0x20;

        public SSBBEntryHeader _header;
        private fixed int _padding[6];

        public void Set(int length)
        {
            _header._tag = Tag;
            _header._length = length;
        }
    }

    #endregion
}