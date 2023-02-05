using BrawlLib.CustomLists;
using BrawlLib.Internal;
using BrawlLib.Internal.IO;
using BrawlLib.SSBB.Types;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class ClassicStageBlockNode : ResourceNode
    {
        private ClassicStageBlockStageData data;

        public override ResourceType ResourceFileType => ResourceType.Container;

        [Category("Classic Mode Battle")]
        public ClassicBattleType BattleType
        {
            get => (ClassicBattleType) (int) data._battleType;
            set
            {
                data._battleType = (int) value;
                SignalPropertyChange();
            }
        }

        [Category("Stage List")]
        [TypeConverter(typeof(DropDownListStageIDs))]
        public int StageID1
        {
            get => data._stageID1;
            set
            {
                data._stageID1 = (ushort) value;
                SignalPropertyChange();
            }
        }

        [Category("Stage List")]
        [TypeConverter(typeof(DropDownListStageIDs))]
        public int StageID2
        {
            get => data._stageID2;
            set
            {
                data._stageID2 = (ushort) value;
                SignalPropertyChange();
            }
        }

        [Category("Stage List")]
        [TypeConverter(typeof(DropDownListStageIDs))]
        public int StageID3
        {
            get => data._stageID3;
            set
            {
                data._stageID3 = (ushort) value;
                SignalPropertyChange();
            }
        }

        [Category("Stage List")]
        [TypeConverter(typeof(DropDownListStageIDs))]
        public int StageID4
        {
            get => data._stageID4;
            set
            {
                data._stageID4 = (ushort) value;
                SignalPropertyChange();
            }
        }

        public enum ClassicBattleType : int
        {
            FreeForAll = 0,
            TeamBattle = 1,
            HordeBattle = 2
        }

        public override bool OnInitialize()
        {
            base.OnInitialize();

            if (WorkingUncompressed.Length != sizeof(ClassicStageBlock))
            {
                throw new Exception("Wrong size for ClassicStageBlockNode");
            }

            // Copy the data from the address
            data = ((ClassicStageBlock*) WorkingUncompressed.Address)->_stages;

            List<string> stageList = new List<string>();
            foreach (int stageID in new int[] {StageID1, StageID2, StageID3, StageID4})
            {
                if (stageID == 255)
                {
                    continue;
                }

                Stage found = Stage.Stages.FirstOrDefault(s => s.ID == stageID);
                stageList.Add(found == null ? stageID.ToString() : found.PacBasename);
            }

            _name = "Classic Stage Block (" + string.Join(", ", stageList) + ")";

            return true;
        }

        public override void OnPopulate()
        {
            ClassicFighterData* ptr = &((ClassicStageBlock*) WorkingUncompressed.Address)->_opponent1;
            for (int i = 0; i < 3; i++)
            {
                DataSource source = new DataSource(ptr, sizeof(ClassicFighterData));
                new ClassicFighterNode().Initialize(this, source);
                ptr++;
            }
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            // Copy the data back to the address
            ClassicStageBlock* dataPtr = (ClassicStageBlock*) address;
            dataPtr->_stages._battleType = data._battleType;
            dataPtr->_stages._stageID1 = data._stageID1;
            dataPtr->_stages._stageID2 = data._stageID2;
            dataPtr->_stages._stageID3 = data._stageID3;
            dataPtr->_stages._stageID4 = data._stageID4;

            // Rebuild children using new address
            ClassicFighterData* ptr = &((ClassicStageBlock*) address)->_opponent1;
            for (int i = 0; i < Children.Count; i++)
            {
                Children[i].Rebuild(ptr, sizeof(ClassicFighterData), true);
                ptr++;
            }
        }

        public override int OnCalculateSize(bool force)
        {
            // Constant size (260 bytes)
            return sizeof(ClassicStageBlock);
        }
    }

    public unsafe class ClassicDifficultyNode : ResourceNode
    {
        private ClassicDifficultyData data;

        [Browsable(false)]
        [Category("Fighter")]
        [DisplayName("Ally Status")]
        public byte Unknown00
        {
            get => data._unknown00;
            set
            {
                data._unknown00 = value;
                SignalPropertyChange();
            }
        }

        [Category("Fighter")]
        [DisplayName("CostumeID")]
        public byte CostumeID
        {
            get => data._color;
            set
            {
                data._color = value;
                SignalPropertyChange();
            }
        }

        [Category("AI")]
        [DisplayName("CPU Level")]
        public byte CPULevel
        {
            get => data._aiLevel;
            set
            {
                data._aiLevel = value;
                SignalPropertyChange();
            }
        }

        [Category("AI")]
        [DisplayName("AI Type")]
        [TypeConverter(typeof(DropDownListAITypes))]
        public byte AIType
        {
            get => data._aiType;
            set
            {
                data._aiType = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        [DisplayName("Unknown 03")]
        public byte Unknown03
        {
            get => data._unknown03;
            set
            {
                data._unknown03 = value;
                SignalPropertyChange();
            }
        }

        [Category("Fighter")]
        [DisplayName("Stocks")]
        public byte Stock
        {
            get => data._stock;
            set
            {
                data._stock = value;
                SignalPropertyChange();
            }
        }

        [Category("Fighter")]
        [DisplayName("Handicap")]
        public byte Handicap
        {
            get => data._handicap;
            set
            {
                data._handicap = value;
                SignalPropertyChange();
            }
        }

        [Category("Fighter")]
        [DisplayName("Offense Ratio")]
        public short OffenseRatio
        {
            get => data._offenseRatio;
            set
            {
                data._offenseRatio = value;
                SignalPropertyChange();
            }
        }

        [Category("Fighter")]
        [DisplayName("Defense Ratio")]
        public short DefenseRatio
        {
            get => data._defenseRatio;
            set
            {
                data._defenseRatio = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        [DisplayName("Unknown 0b")]
        public byte Unknown0b
        {
            get => data._unknown0b;
            set
            {
                data._unknown0b = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        [DisplayName("Unknown 0c")]
        public short Unknown0c
        {
            get => data._unknown0c;
            set
            {
                data._unknown0c = value;
                SignalPropertyChange();
            }
        }

        public override bool OnInitialize()
        {
            base.OnInitialize();

            if (WorkingUncompressed.Length != sizeof(ClassicDifficultyData))
            {
                throw new Exception("Wrong size for ClassicDifficultyNode");
            }

            // Copy the data from the address
            data = *(ClassicDifficultyData*) WorkingUncompressed.Address;

            return false;
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            // Copy the data back to the address
            *(ClassicDifficultyData*) address = data;
        }

        public override int OnCalculateSize(bool force)
        {
            return sizeof(ClassicDifficultyData);
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct ClassicTblHeader
    {
        public const int Size = 0x50;

        public byte _fighterID;
        public byte _fighterstatus;
        public byte _unknown02;
        public byte _unknown03;
        public float _fighterscale;

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
    }

    public unsafe class ClassicFighterNode : ResourceNode
    {
        public override bool supportsCompression => false;
        private ClassicTblHeader _header;

        public enum StatusEnum : byte
        {
            Normal = 0,
            Metal = 1,
            Invisible = 2
        }

        [TypeConverter(typeof(DropDownListBrawlExSlotIDsSinglePlayer))]
        [Category("Fighter")]
        [DisplayName("Fighter ID")]
        public byte FighterID
        {
            get => _header._fighterID;
            set
            {
                _header._fighterID = value;
                if (value == 0xFF)
                {
                    Name = "None";
                }
                else
                {
                    Name = FighterNameGenerators.FromID(_header._fighterID,
                        FighterNameGenerators.slotIDIndex, "-S");
                }

                SignalPropertyChange();
            }
        }

        [Category("Fighter")]
        [DisplayName("Fighter Status")]
        public StatusEnum FighterStatus
        {
            get => (StatusEnum) _header._fighterstatus;
            set
            {
                _header._fighterstatus = (byte) value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        [DisplayName("Unknown 02")]
        public byte Unknown02
        {
            get => _header._unknown02;
            set
            {
                _header._unknown02 = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        [DisplayName("Unknown 03")]
        public byte Unknown03
        {
            get => _header._unknown03;
            set
            {
                _header._unknown03 = value;
                SignalPropertyChange();
            }
        }

        [Category("Fighter")]
        [DisplayName("Fighter Scale")]
        public float FighterScale
        {
            get => _header._fighterscale;
            set
            {
                _header._fighterscale = value;
                SignalPropertyChange();
            }
        }

        [Category("Fighter")]
        [DisplayName("Player Ally")]
        public bool IsAlly
        {
            get
            {
                if (Children.Count > 0)
                {
                    return ((ClassicDifficultyNode) Children[0]).Unknown00 == 0;
                }

                return false;
            }
            set
            {
                if (Children.Count > 0)
                {
                    ((ClassicDifficultyNode) Children[0]).Unknown00 = value ? (byte) 0 : (byte) 1;
                }
            }
        }

        public override bool OnInitialize()
        {
            base.OnInitialize();

            if (WorkingUncompressed.Length != sizeof(ClassicFighterData))
            {
                throw new Exception("Wrong size for ClassicFighterNode");
            }

            // Copy the data from the address
            ClassicFighterData* ptr = (ClassicFighterData*) WorkingUncompressed.Address;
            _header._fighterID = ptr->_fighterID;
            _header._fighterstatus = ptr->_fighterstatus;
            _header._unknown02 = ptr->_unknown02;
            _header._unknown03 = ptr->_unknown03;
            _header._fighterscale = ptr->_fighterscale;

            if (_name == null)
            {
                _name = FighterNameGenerators.FromID(_header._fighterID,
                    FighterNameGenerators.slotIDIndex, "-S");
            }

            if (_header._fighterID == 0xFF)
            {
                _name = "None";
            }

            return true;
        }

        public override void OnPopulate()
        {
            VoidPtr ptr = WorkingUncompressed.Address + 8;
            foreach (string s in new string[] {"Easy", "Normal", "Hard", "Very Hard", "Intense"})
            {
                DataSource source = new DataSource(ptr, sizeof(ClassicDifficultyData));
                ClassicDifficultyNode node = new ClassicDifficultyNode();
                node.Initialize(this, source);
                node.Name = s;
                node.IsDirty = false;
                ptr += sizeof(ClassicDifficultyData);
            }
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            // Copy the data back to the address
            ClassicFighterData* header_ptr = (ClassicFighterData*) address;
            header_ptr->_fighterID = _header._fighterID;
            header_ptr->_fighterstatus = _header._fighterstatus;
            header_ptr->_unknown02 = _header._unknown02;
            header_ptr->_unknown03 = _header._unknown03;
            header_ptr->_fighterscale = _header._fighterscale;

            // Rebuild children using new address
            VoidPtr ptr = address + 8;
            for (int i = 0; i < Children.Count; i++)
            {
                Children[i].Rebuild(ptr, sizeof(ClassicDifficultyData), true);
                ptr += sizeof(ClassicDifficultyData);
            }
        }

        public override int OnCalculateSize(bool force)
        {
            return sizeof(ClassicFighterData);
        }
    }

    public unsafe class ClassicStageTblNode : ResourceNode
    {
        public override ResourceType ResourceFileType => ResourceType.ClassicStageTbl;

        private List<int> _padding;

        public string Padding => string.Join(", ", _padding);

        public override bool OnInitialize()
        {
            base.OnInitialize();

            VoidPtr ptr = WorkingUncompressed.Address;
            int numEntries = WorkingUncompressed.Length / sizeof(ClassicStageBlock);
            for (int i = 0; i < numEntries; i++)
            {
                ptr += sizeof(ClassicStageBlock);
            }

            _padding = new List<int>();
            bint* ptr2 = (bint*) ptr;
            while (ptr2 < WorkingUncompressed.Address + WorkingUncompressed.Length)
            {
                _padding.Add(*ptr2++);
            }

            return true;
        }

        public override void OnPopulate()
        {
            int numEntries = WorkingUncompressed.Length / sizeof(ClassicStageBlock);

            ClassicStageBlock* ptr = (ClassicStageBlock*) WorkingUncompressed.Address;
            for (int i = 0; i < numEntries; i++)
            {
                DataSource source = new DataSource(ptr, sizeof(ClassicStageBlock));
                new ClassicStageBlockNode().Initialize(this, source);
                ptr++;
            }
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            // Rebuild children using new address
            ClassicStageBlock* ptr = (ClassicStageBlock*) address;
            for (int i = 0; i < Children.Count; i++)
            {
                Children[i].Rebuild(ptr, sizeof(ClassicStageBlock), true);
                ptr++;
            }

            bint* ptr2 = (bint*) ptr;
            foreach (int pad in Padding)
            {
                *ptr2++ = pad;
            }
        }

        public override int OnCalculateSize(bool force)
        {
            return sizeof(ClassicStageBlock) * Children.Count + Padding.Length * sizeof(bint);
        }

        public ResourceNode CreateEntry()
        {
            FileMap tempFile = FileMap.FromTempFile(sizeof(ClassicStageBlock));
            // Is this the right way to add a new child node?
            ClassicStageBlockNode node = new ClassicStageBlockNode();
            node.Initialize(null, tempFile);
            AddChild(node, true);
            return node;
        }
    }

    public class BrawlAITypes
    {
        public byte AIID;
        public string Name;

        public BrawlAITypes(byte id, string name)
        {
            AIID = id;
            Name = name;
        }

        public static readonly BrawlAITypes[] AITypes = new BrawlAITypes[]
        {
            new BrawlAITypes(0x00, "Normal"),
            new BrawlAITypes(0x02, "Walk"),
            new BrawlAITypes(0x03, "Run"),
            new BrawlAITypes(0x04, "Jump")
        };
    }

    public class ClassicStageTblSizeTblNode : RawDataNode
    {
    }
}