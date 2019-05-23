using System;
using BrawlLib.SSBBTypes;
using System.ComponentModel;
using BrawlLib.IO;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class CENode : ARCEntryNode
    {
        internal CEHeader* Header { get { return (CEHeader*)WorkingUncompressed.Address; } }

        internal int unk1, unk2, unk3;

        public override ResourceType ResourceType { get { return ResourceType.CE; } }

        [Category("Offensive AI Node")]
        public int NumEntries { get { return Children.Count; } }

        public override bool OnInitialize()
        {
            if (_name == null)
                this._name = "CE " + this.Parent.Name.Replace("ai_", "");
            unk1 = Header->_unk1;
            unk2 = Header->_unk2;
            unk3 = Header->_unk3;

            return Header->_numEntries > 0;
        }

        public override void OnPopulate()
        {
            new CEGroupNode("Events").Initialize(this, new DataSource(Header, 0x0));
            new CEGroupNode("Strings").Initialize(this, new DataSource(Header, 0x0));
        }

        internal static ResourceNode TryParse(DataSource source)
        {
            CEHeader* header = (CEHeader*)source.Address;
            if (header->_numEntries <= 0 || header->_numEntries > 0x100 || header->_unk1 != 0 || header->_unk2 != 0x1000000 || header->_unk3 != 0)
                return null;

            return new CENode();
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            CEHeader* header = (CEHeader*)address;
            header->_unk1 = unk1;
            header->_unk2 = unk2;
            header->_unk3 = unk3;
            header->_numEntries = Children[0].Children.Count;
            Children[0].Rebuild(address, 0x0, true);//rebuild CEEntries
            int EntrySize = Children[0].CalculateSize(true);//Caluculate CEEntry's size
            int offset = EntrySize + header->entryOffsets[0];//set first CEString offset
            for (int i = 0; i < Children[1].Children.Count; i++)
            {
                header->entryOffsets[Children[0].Children.Count + i] = offset;
                offset += Children[1].Children[i].CalculateSize(true);
            }
            Children[1].Rebuild(address + header->entryOffsets[0] + EntrySize, 0x0, true);
        }

        public CEEntryNode CreateCEEntryNode()
        {
            CEEntryNode n = new CEEntryNode();
            this.Children[0].AddChild(n);
            return n;
        }

        public CEStringNode CreateCEStringNode()
        {
            CEStringNode n = new CEStringNode();
            this.Children[1].AddChild(n);
            return n;
        }

        public override int OnCalculateSize(bool force)
        {
            int size = 0x10 + Children[0].Children.Count * 0x4 + Children[1].Children.Count * 0x4;//CEheader size
            if (size % 0x10 != 0)
                size = size + 0x10 - size % 0x10;
            size += Children[0].CalculateSize(true);
            size += Children[1].CalculateSize(true);
            return size;
        }
    }

    public unsafe class CEGroupNode : ResourceNode
    {
        internal CEHeader* Header { get { return (CEHeader*)WorkingUncompressed.Address; } }

        public CEGroupNode() : base() { }
        public CEGroupNode(string name) : base() { this._name = name; }


        public override bool OnInitialize()
        {
            return true;
        }
        //when use this you must send first address or string address
        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            CEHeader* header = (CEHeader*)address;
            VoidPtr entry = null;
            int offset = 0x10 + Children.Count * 0x4 + Parent.Children[1].Children.Count * 0x4;
            if (offset % 0x10 != 0)
                entry = address + offset + 0x10 - offset % 0x10;
            else
                entry = address + offset;
            int count = 0;
            header->entryOffsets[count] = (int)(entry - address);//write offset of first entry
            if (this._name == "Events")
                foreach (CEEntryNode n in Children)
                {
                    int entrySize = n.CalculateSize(true);
                    n.Rebuild(entry, entrySize, true);
                    entry += entrySize;
                    count++;
                    header->entryOffsets[count] = (int)(entry - address);//write each offset of entries
                }
            if (this._name == "Strings")
                foreach (CEStringNode n in Children)//"address" is first address of CEString
                {
                    int entrySize = n.CalculateSize(true);
                    n.Rebuild(address, entrySize, true);
                    address += entrySize;
                }
        }

        public override void OnPopulate()
        {
            Type t = null;//switch by name
            if (_name == "Events")
                t = typeof(CEEntryNode);
            else if (_name == "Strings")
                t = typeof(CEStringNode);
            else
                return;

            if (t == typeof(CEEntryNode))//is this CEEntryNode?
                for (int i = 0; i < Header->_numEntries; i++)
                {
                    VoidPtr entry = (VoidPtr)Header + Header->entryOffsets[i];
                    CEEntryNode node = new CEEntryNode();
                    node.NextAddress = (VoidPtr)Header + Header->entryOffsets[i + 1];
                    node.Initialize(this, new DataSource(entry, node.NextAddress - entry));
                }
            else//this is CEStringNode
            {
                bint* offset = &Header->entryOffsets[Header->_numEntries];
                for (; (VoidPtr)offset < (VoidPtr)Header + Header->entryOffsets[0]; offset++)
                {
                    if (*offset != 0x0)
                    {
                        CEStringNode node = new CEStringNode();
                        node.Initialize(this, new DataSource((VoidPtr)Header + *offset, 0x0));
                    }
                }
            }

        }
        public override int OnCalculateSize(bool force)
        {
            int size = 0;
            if (this._name == "Events")//calculate size of all CEEntryNode
            {
                foreach (CEEntryNode n in Children)
                    size += n.CalculateSize(true);
            }
            else//calculate size of all CEStringNode
                foreach (CEStringNode n in Children)
                    size += n.CalculateSize(true);
            return size;
        }
    }

    public unsafe class CEEntryNode : ResourceNode
    {
        internal CEEntry* Header { get { return (CEEntry*)WorkingUncompressed.Address; } }
        private List<float> entries = new List<float>();
        private List<int> IndexList = new List<int>();
        private int id, EventsOffset, part2Offset, unknown;
        public VoidPtr NextAddress = 0;

        public override ResourceType ResourceType
        {
            get
            {
                return ResourceType.CEEntry;
            }
        }

        [Category("CEEntry")]
        public string ID { get { return id.ToString("X"); } set { id = Convert.ToInt32(value, 16); SignalPropertyChange(); } }
        [Category("CEEntry")]
        public string TrueID { get { return (id & 0x7FFF).ToString("X"); } }
        [Category("CEEntry")]
        public int Unknown { get { return unknown; } set { unknown = value; SignalPropertyChange(); } }
        [Category("CEEntry")]
        public int NumEntries { get { return this.Children.Count; } }
        [Category("CEEntry")]
        public List<float> Part2Entries
        {
            get { return entries; }
            set { entries = value; SignalPropertyChange(); }
        }


        public override bool OnInitialize()
        {
            entries.Clear();
            if (_name == null || _name == "")
                _name = ((int)Header->_ID).ToString("X");

            bfloat* part2 = Header->part2;
            while (part2 < (System.bfloat*)((VoidPtr)Header) + WorkingUncompressed.Length)
            {
                entries.Add(*part2);
                part2++;
            }
            id = Header->_ID;
            EventsOffset = Header->_EventsOffset;
            part2Offset = Header->_part2Offset;
            unknown = Header->_unknown;
            _name = TrueID;
            return true;
        }


        public override void OnPopulate()
        {
            CEEvent* currentEvent = null;
            VoidPtr current = Header->Event;
            while ((System.bfloat*)current < Header->part2)
            {
                currentEvent = (CEEvent*)current;
                CEEventNode temp = new CEEventNode();
                temp.Initialize(this, new DataSource(current, currentEvent->_entrySize));
                current += 0x4 + 0x4 * temp.NumEntries;
            }
            for (int i = 0; i < Part2Entries.Count; i++)
                IndexList.Add(i);
        }

        public float? GetPart2(int id)
        {
            if (IndexList.IndexOf(id - 0x2000) >= 0)
                return Part2Entries[IndexList.IndexOf(id - 0x2000)];
            else
                return null;
        }
        public int SetPart2(float value, int insertID)//returns id
        {
            if (GetPart2(insertID) != value && GetPart2(insertID) != null)
            {
                IndexList.Insert(insertID - 0x2000, Part2Entries.Count);
                Part2Entries.Insert(insertID - 0x2000, value);
                return IndexList[insertID - 0x2000] + 0x2000;
            }
            else if (GetPart2(insertID) == null)//if insertID is out of range of Part2Entries
            {
                IndexList.Add(Part2Entries.Count);
                Part2Entries.Add(value);
                return IndexList[IndexList.Count - 1] + 0x2000;
            }
            return insertID;//if there's no change
        }
        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            CEEntry* header = (CEEntry*)address;
            int eventSize = 0;
            header->_EventsOffset = EventsOffset;
            header->_ID = id;
#if DEBUG
            header->_ID = Convert.ToInt32(_name, 16);//Debug Code
#endif
            header->_unknown = unknown;
            VoidPtr currentAddress = header->Event;
            foreach (CEEventNode n in Children)
            {
                int Size = n.CalculateSize(true);
                eventSize += Size;
                for (int i = 0; i < n.Entries.Count; i++)
                {
                    if (n.Entries[i] >= 0x2000)
                        n.Entries[i] = IndexList.IndexOf(n.Entries[i] - 0x2000) + 0x2000;
                }
                n.Rebuild(currentAddress, Size, true);
                currentAddress += Size;
            }
            header->_part2Offset = eventSize + 0x10;//header size
            for (int i = 0; i < Part2Entries.Count; i++)
                header->part2[i] = Part2Entries[i];
        }

        public override int OnCalculateSize(bool force)
        {
            int eventSize = 0;
            int part2Size = 0;
            int headerSize = 0x10;
            foreach (CEEventNode n in Children)
            {
                eventSize += n.EntrySize;
            }
            part2Size += Part2Entries.Count * 0x4;
            if ((eventSize + part2Size + headerSize) % 0x10 != 0)
                return ((eventSize + part2Size + headerSize) + 0x10 - (eventSize + part2Size + headerSize) % 0x10);
            else
                return (eventSize + part2Size + headerSize);
        }

    }
    public unsafe class CEEventNode : ResourceNode
    {
        internal CEEvent* Header { get { return (CEEvent*)WorkingUncompressed.Address; } }
        public List<int> Entries = new List<int>();
        private List<float> param = new List<float>();
        sbyte type;

        public override ResourceType ResourceType
        {
            get
            {
                return ResourceType.CEEvent;
            }
        }

        [Category("CE Event"), Description("Entry Type")]
        public sbyte Type { get { return type; } set { type = value; SignalPropertyChange(); } }
        [Category("CE Event")]
        public int NumEntries { get { return Entries.Count; } }
        [Category("CE Event")]
        public int EntrySize { get { return Entries.Count * 0x4 + 0x4; } }
        [Category("CE Event")]
        public float[] Parameters
        {
            get
            {
                param.Clear();
                foreach (int i in Entries)
                {
                    if (i >= 0x2000)
                        param.Add(((CEEntryNode)Parent).GetPart2(i) ?? 0);
                }
                return param.ToArray();
            }
            set
            {
                param.Clear();
                foreach (float f in value)
                    param.Add(f);
                for (int i = 0x1ffE + ((CEEntryNode)Parent).Part2Entries.Count, j = 0, k = 0; k < param.Count; j++)
                {
                    Action function = () =>//sorry for bad code...
                    {
                        if (i >= 0x2000)
                            if (j < Entries.Count)
                            {
                                Entries[j] = ((CEEntryNode)Parent).SetPart2(param[k], i);
                                k++;
                            }
                            else
                            {
                                Entries.Add(((CEEntryNode)Parent).SetPart2(param[k], i));
                                k++;
                            }
                    };

                    if (j < Entries.Count)
                    {
                        if (Entries[j] >= 0x2000)
                        {
                            i = Entries[j]; function();
                        }
                    }
                    else
                    {
                        i++; function();
                    }

                }
                SignalPropertyChange();
            }
        }


        public override bool OnInitialize()
        {
            if (_name == null || _name == "")
                _name = "Event " + ((int)Header->_type).ToString("X");
            for (int i = 0; i < Header->_numEntries; i++)
                Entries.Add(Header->Entries[i]);
            type = Header->_type;
            return false;
        }


        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            CEEvent* e = (CEEvent*)address;
            e->_entrySize = (short)EntrySize;
            e->_numEntries = (sbyte)Entries.Count;
            e->_type = type;
            for (int i = 0; i < Entries.Count; i++)
                e->Entries[i] = Entries[i];
        }

        public override int OnCalculateSize(bool force)
        {
            return EntrySize;
        }
    }

    public unsafe class CEStringNode : ResourceNode
    {
        internal CEString* Header { get { return (CEString*)WorkingUncompressed.Address; } }
        // internal int[,] entries;
        internal int[][] entries;
        internal string[] strings;
        internal int unk1, unk2, unk3;

        public override ResourceType ResourceType
        {
            get
            {
                return ResourceType.CEString;
            }
        }

        [Category("AI StringNode Entry"), Description("Each entries are related to Strings")]
        public int[][] Entries { get { return entries; } set { entries = value; SignalPropertyChange(); } }
        [Category("AI StringNode Entry"), Description("Unknown strings")]
        public string[] Strings { get { return strings; } set { strings = value; SignalPropertyChange(); } }

        public override bool OnInitialize()
        {
            if (_name == null || _name == "")
                _name = "StringEntry" + Parent.Children.IndexOf(this).ToString();
            entries = new int[Header->_numEntries][];
            for (int i = 0; i < entries.Length; i++)
                entries[i] = new int[2];
            for (int i = 0, j = 0; j < Header->_numEntries; i += 0x2, j++)
            {
                entries[j][0] = Header->Entries[i];
                entries[j][1] = Header->Entries[i + 1];
            }
            strings = GetStrings();
            unk1 = Header->_unk1;
            unk2 = Header->_unk2;
            unk3 = Header->_unk3;
            return false;
        }

        internal string[] GetStrings()
        {
            List<byte> s = new List<byte>();
            string[] returnStrings = new string[Header->_numEntries];
            byte* word = (byte*)(&Header->Entries[Header->_numEntries * 2]);
            for (int i = 0; i < Header->_numEntries; i++)
            {
                for (; *word != 0x0; word++)
                {
                    s.Add(*word);
                }
                Encoding sjisEnc = Encoding.GetEncoding("Shift_JIS");
                returnStrings[i] = sjisEnc.GetString(s.ToArray());
                s.Clear();
                word++;
            }
            return returnStrings;
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            CEString* strings = (CEString*)address;
            strings->_numEntries = Strings.Length;
            strings->_unk1 = unk1;
            strings->_unk2 = unk2;
            strings->_unk3 = unk3;
            for (int i = 0, j = 0; j < Entries.Length; i += 0x2, j++)
            {
                strings->Entries[i] = Entries[j][0];
                strings->Entries[i + 1] = Entries[j][1];
            }
            byte* word = (byte*)(&strings->Entries[strings->_numEntries * 2]);
            List<byte[]> ByteStrLine = new List<byte[]>();
            Encoding sjisEnc = Encoding.GetEncoding("Shift_JIS");
            foreach (string s in Strings)
                ByteStrLine.Add(sjisEnc.GetBytes(s));
            foreach (byte[] bline in ByteStrLine)
            {
                foreach (byte b in bline)
                {
                    *word = b;
                    word++;
                }
                *word = 0x0; word++;
            }
        }

        public override int OnCalculateSize(bool force)
        {
            int size = 0x10;
            foreach (int[] array in Entries)
                foreach (int entry in array)
                    size += 0x4;
            foreach (string s in Strings)
                size += s.Length;// + 0x1;
            if (Strings.Length > 1)
                size += Strings.Length - 1;
            if (size % 0x10 != 0)
                return size + 0x10 - size % 0x10;
            else
                return size;
        }

        public override unsafe void Export(string outPath)
        {
            uint dataLen = (uint)OnCalculateSize(true);
            using (FileStream stream = new FileStream(outPath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None, 8, FileOptions.RandomAccess))
            {
                stream.SetLength(dataLen);
                using (FileMap map = FileMap.FromStream(stream))
                {
                    Rebuild(map.Address, (int)dataLen, true);
                }
            }
        }
    }
}
