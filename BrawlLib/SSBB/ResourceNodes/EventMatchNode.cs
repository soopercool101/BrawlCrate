using BrawlLib.CustomLists;
using BrawlLib.Internal;
using BrawlLib.SSBB.ResourceNodes.ProjectPlus;
using BrawlLib.SSBB.Types;
using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class EventMatchDifficultyNode : ResourceNode
    {
        private EventMatchDifficultyData data;

        public byte CpuLevel
        {
            get => data._cpuLevel;
            set
            {
                data._cpuLevel = value;
                SignalPropertyChange();
            }
        }

        public ushort OffenseRatio
        {
            get => data._offenseRatio;
            set
            {
                data._offenseRatio = value;
                SignalPropertyChange();
            }
        }

        public ushort DefenseRatio
        {
            get => data._defenseRatio;
            set
            {
                data._defenseRatio = value;
                SignalPropertyChange();
            }
        }

        public byte AiType
        {
            get => data._aiType;
            set
            {
                data._aiType = value;
                SignalPropertyChange();
            }
        }

        public byte Costume
        {
            get => data._costume;
            set
            {
                data._costume = value;
                SignalPropertyChange();
            }
        }

        public byte StockCount
        {
            get => data._stockCount;
            set
            {
                data._stockCount = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public byte Unknown09
        {
            get => data._unknown09;
            set
            {
                data._unknown09 = value;
                SignalPropertyChange();
            }
        }

        public short InitialHitPoints
        {
            get => data._initialHitPoints;
            set
            {
                data._initialHitPoints = value;
                SignalPropertyChange();
            }
        }

        public short StartingDamage
        {
            get => data._startingDamage;
            set
            {
                data._startingDamage = value;
                SignalPropertyChange();
            }
        }

        public override bool OnInitialize()
        {
            base.OnInitialize();

            if (WorkingUncompressed.Length != sizeof(EventMatchDifficultyData))
            {
                throw new Exception("Wrong size for EventMatchDifficultyNode");
            }

            // Copy the data from the address
            data = *(EventMatchDifficultyData*) WorkingUncompressed.Address;

            return false;
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            // Copy the data back to the address
            *(EventMatchDifficultyData*) address = data;
        }

        public override int OnCalculateSize(bool force)
        {
            // Constant size (14 bytes)
            return sizeof(EventMatchDifficultyData);
        }
    }

    public unsafe class EventMatchFighterNode : ResourceNode
    {
        public enum StatusEnum : byte
        {
            Normal = 0,
            Metal = 1,
            Clear = 2,
            PMTurbo = 3
        }

        private EventMatchFighterHeader data;

        [TypeConverter(typeof(DropDownListBrawlExSlotIDsSinglePlayer))]
        public byte FighterID
        {
            get => data._fighterID;
            set
            {
                data._fighterID = value;
                Name = GenerateName();
                SignalPropertyChange();
            }
        }

        public StatusEnum Status
        {
            get => (StatusEnum) data._status;
            set
            {
                data._status = (byte) value;
                Name = GenerateName();
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public byte Unknown02
        {
            get => data._unknown02;
            set
            {
                data._unknown02 = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public byte Unknown03
        {
            get => data._unknown03;
            set
            {
                data._unknown03 = value;
                SignalPropertyChange();
            }
        }

        public float Scale
        {
            get => data._scale;
            set
            {
                data._scale = value;
                Name = GenerateName();
                SignalPropertyChange();
            }
        }

        public byte Team
        {
            get => data._team;
            set
            {
                data._team = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public byte Unknown09
        {
            get => data._unknown09;
            set
            {
                data._unknown09 = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public byte Unknown0a
        {
            get => data._unknown0a;
            set
            {
                data._unknown0a = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public byte Unknown0b
        {
            get => data._unknown0b;
            set
            {
                data._unknown0b = value;
                SignalPropertyChange();
            }
        }

        public override bool OnInitialize()
        {
            base.OnInitialize();

            if (WorkingUncompressed.Length != sizeof(EventMatchFighterData))
            {
                throw new Exception("Wrong size for EventMatchFighterNode");
            }

            // Copy the data from the address
            data = *(EventMatchFighterHeader*) WorkingUncompressed.Address;

            if (_name == null)
            {
                _name = GenerateName();
            }

            return true;
        }

        public override void OnPopulate()
        {
            VoidPtr ptr = WorkingUncompressed.Address + sizeof(EventMatchFighterHeader);
            foreach (string s in new string[] {"Easy", "Normal", "Hard"})
            {
                DataSource source = new DataSource(ptr, sizeof(EventMatchDifficultyData));
                EventMatchDifficultyNode node = new EventMatchDifficultyNode();
                node.Initialize(this, source);
                node.Name = s;
                node.IsDirty = false;
                ptr += sizeof(EventMatchDifficultyData);
            }
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            // Copy the data back to the address
            *(EventMatchFighterHeader*) address = data;

            // Rebuild children using new address
            VoidPtr ptr = address + sizeof(EventMatchFighterHeader);
            for (int i = 0; i < Children.Count; i++)
            {
                Children[i].Rebuild(ptr, sizeof(EventMatchDifficultyData), true);
                ptr += sizeof(EventMatchDifficultyData);
            }
        }

        public override int OnCalculateSize(bool force)
        {
            // Constant size (48 bytes)
            return sizeof(EventMatchFighterData);
        }

        public string GenerateName()
        {
            string newName = "";

            if (Scale > 1.0f)
            {
                newName += "Giant ";
            }
            else if (Scale < 1.0f)
            {
                newName += "Tiny ";
            }

            if (Status == StatusEnum.Metal)
            {
                newName += "Metal ";
            }
            else if (Status == StatusEnum.Clear)
            {
                newName += "Clear ";
            }

            if (FighterID == 0x3E)
            {
                if (Index == 0 || Parent != null && Parent.Name.EndsWith("_2p") && Index == 1)
                {
                    newName += "Select Character";
                }
                else
                {
                    return "None";
                }
            }
            else
            {
                newName += FighterNameGenerators.FromID(FighterID, FighterNameGenerators.slotIDIndex, "-S");
            }

            newName += $" (Team {Team})";

            return newName;
        }
    }

    public unsafe class EventMatchNode : ResourceNode
    {
        public override ResourceType ResourceFileType => ResourceType.Container;

        public enum ItemLevelEnum : short
        {
            Off = 0,
            Low = 1,
            Medium = 2,
            High = 3,
            VeryHighProjectPlus = 4,
            IntenseProjectPlus = 5,
            BombRainProjectPlus = 6
        }

        public enum MatchTypeEnum : byte
        {
            Time = 0,
            Stock = 1,
            Coin = 2
        }

        private EventMatchTblHeader _header;

        [Category("Event Match")]
        [DisplayName("Event Extension")]
        public bint EventExtension => _header._eventExtension;

        [Category("Event Match")]
        [DisplayName("Event Match Number")]
        public int EventNumber => Index + 1;

        [Category("Unknown")]
        public int Unknown04
        {
            get => _header._unknown04;
            set => _header._unknown04 = value;
        }

        [Category("Event Match")]
        [DisplayName("Match Type")]
        public MatchTypeEnum MatchType
        {
            get => (MatchTypeEnum) _header._matchType;
            set
            {
                _header._matchType = (byte) value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public byte Unknown09
        {
            get => _header._unknown09;
            set
            {
                _header._unknown09 = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public byte Unknown0a
        {
            get => _header._unknown0a;
            set
            {
                _header._unknown0a = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public byte Unknown0b
        {
            get => _header._unknown0b;
            set
            {
                _header._unknown0b = value;
                SignalPropertyChange();
            }
        }

        [Category("Event Match")]
        [DisplayName("Time Limit")]
        public int TimeLimit
        {
            get => _header._timeLimit;
            set
            {
                _header._timeLimit = value;
                SignalPropertyChange();
            }
        }

        [Category("Event Match")]
        [DisplayName("Timer Visible")]
        public bool TimerVisible
        {
            get => (_header._flags10 & 0x20000000) != 0;
            set
            {
                SignalPropertyChange();
                if (value)
                {
                    _header._flags10 |= 0x20000000;
                }
                else
                {
                    _header._flags10 &= ~0x20000000;
                }
            }
        }

        [Category("Event Match")]
        [DisplayName("Hide Countdown")]
        public bool HideCountdown
        {
            get => (_header._flags10 & 0x40000000) != 0;
            set
            {
                SignalPropertyChange();
                if (value)
                {
                    _header._flags10 |= 0x40000000;
                }
                else
                {
                    _header._flags10 &= ~0x40000000;
                }
            }
        }

        [Category("Unknown")]
        public bool UnknownFlag_10_80000000
        {
            get => (_header._flags10 & 0x80000000) != 0;
            set
            {
                SignalPropertyChange();
                if (value)
                {
                    _header._flags10 |= unchecked((int) 0x80000000);
                }
                else
                {
                    _header._flags10 &= ~unchecked((int) 0x80000000);
                }
            }
        }

        [Category("Unknown")]
        public float Unknown14
        {
            get => _header._unknown14;
            set
            {
                _header._unknown14 = value;
                SignalPropertyChange();
            }
        }

        [Category("Event Match")]
        [DisplayName("Hide Damage Values")]
        public bool HideDamageValues
        {
            get => (_header._flags18 & 0x80) != 0;
            set
            {
                SignalPropertyChange();
                if (value)
                {
                    _header._flags18 |= 0x80;
                }
                else
                {
                    _header._flags18 &= unchecked((byte) ~0x80);
                }
            }
        }

        [Category("Event Match")]
        [DisplayName("Team Match")]
        public bool IsTeamGame
        {
            get => _header._isTeamGame != 0;
            set
            {
                SignalPropertyChange();
                _header._isTeamGame = (byte) (value ? 1 : 0);
            }
        }

        [Category("Event Match")]
        [DisplayName("Item Level")]
        public ItemLevelEnum ItemLevel
        {
            get => (ItemLevelEnum) (short) _header._itemLevel;
            set
            {
                _header._itemLevel = (short) value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public byte Unknown1c
        {
            get => _header._unknown1c;
            set
            {
                _header._unknown1c = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public byte Unknown1d
        {
            get => _header._unknown1d;
            set
            {
                _header._unknown1d = value;
                SignalPropertyChange();
            }
        }

        [Category("Event Match")]
        [DisplayName("Stage")]
        [TypeConverter(typeof(DropDownListStageIDs))]
        public int StageID
        {
            get => _header._stageID;
            set
            {
                _header._stageID = (ushort) value;
                SignalPropertyChange();
            }
        }

        [Category("Event Match")]
        [DisplayName("Players On Screen")]
        public byte PlayersOnScreen
        {
            get => (byte) (_header._flags21.GetUpper() / 2);
            set
            {
                _header._flags21.SetUpper((byte)(value * 2));
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public bool UnknownFlag_20_10000000
        {
            get => _header._flags20[4];
            set
            {
                _header._flags20[4] = value;
                SignalPropertyChange();
            }
        }


        [TypeConverter(typeof(HexUShortConverter))]
        [Category("Event Match: ASL (Requires StageEx)")]
        public ushort ButtonFlags
        {
            get => _header._stageExASL;
            set
            {
                _header._stageExASL = value;
                SignalPropertyChange();
            }
        }

        [Category("Event Match: ASL (Requires StageEx)")]
        [DisplayName("Left")]
        public bool ASL_Left
        {
            get => (ButtonFlags & (ushort)ASLSEntryNode.GameCubeButtons.Left) == (ushort)ASLSEntryNode.GameCubeButtons.Left;
            set
            {
                if (value == ASL_Left)
                    return;
                if (value)
                    ButtonFlags += (ushort) ASLSEntryNode.GameCubeButtons.Left;
                else
                    ButtonFlags -= (ushort) ASLSEntryNode.GameCubeButtons.Left;
            }
        }

        [Category("Event Match: ASL (Requires StageEx)")]
        [DisplayName("Right")]
        public bool ASL_Right
        {
            get => (ButtonFlags & (ushort)ASLSEntryNode.GameCubeButtons.Right) == (ushort)ASLSEntryNode.GameCubeButtons.Right;
            set
            {
                if (value == ASL_Right)
                    return;
                if (value)
                    ButtonFlags += (ushort)ASLSEntryNode.GameCubeButtons.Right;
                else
                    ButtonFlags -= (ushort)ASLSEntryNode.GameCubeButtons.Right;
            }
        }

        [Category("Event Match: ASL (Requires StageEx)")]
        [DisplayName("Down")]
        public bool ASL_Down
        {
            get => (ButtonFlags & (ushort)ASLSEntryNode.GameCubeButtons.Down) == (ushort)ASLSEntryNode.GameCubeButtons.Down;
            set
            {
                if (value == ASL_Down)
                    return;
                if (value)
                    ButtonFlags += (ushort)ASLSEntryNode.GameCubeButtons.Down;
                else
                    ButtonFlags -= (ushort)ASLSEntryNode.GameCubeButtons.Down;
            }
        }

        [Category("Event Match: ASL (Requires StageEx)")]
        [DisplayName("Up")]
        public bool ASL_Up
        {
            get => (ButtonFlags & (ushort)ASLSEntryNode.GameCubeButtons.Up) == (ushort)ASLSEntryNode.GameCubeButtons.Up;
            set
            {
                if (value == ASL_Up)
                    return;
                if (value)
                    ButtonFlags += (ushort)ASLSEntryNode.GameCubeButtons.Up;
                else
                    ButtonFlags -= (ushort)ASLSEntryNode.GameCubeButtons.Up;
            }
        }

        [Category("Event Match: ASL (Requires StageEx)")]
        [DisplayName("Z")]
        public bool ASL_Z
        {
            get => (ButtonFlags & (ushort)ASLSEntryNode.GameCubeButtons.Z) == (ushort)ASLSEntryNode.GameCubeButtons.Z;
            set
            {
                if (value == ASL_Z)
                    return;
                if (value)
                    ButtonFlags += (ushort)ASLSEntryNode.GameCubeButtons.Z;
                else
                    ButtonFlags -= (ushort)ASLSEntryNode.GameCubeButtons.Z;
            }
        }

        [Category("Event Match: ASL (Requires StageEx)")]
        [DisplayName("R")]
        public bool ASL_R
        {
            get => (ButtonFlags & (ushort)ASLSEntryNode.GameCubeButtons.R) == (ushort)ASLSEntryNode.GameCubeButtons.R;
            set
            {
                if (value == ASL_R)
                    return;
                if (value)
                    ButtonFlags += (ushort)ASLSEntryNode.GameCubeButtons.R;
                else
                    ButtonFlags -= (ushort)ASLSEntryNode.GameCubeButtons.R;
            }
        }

        [Category("Event Match: ASL (Requires StageEx)")]
        [DisplayName("L")]
        public bool ASL_L
        {
            get => (ButtonFlags & (ushort)ASLSEntryNode.GameCubeButtons.L) == (ushort)ASLSEntryNode.GameCubeButtons.L;
            set
            {
                if (value == ASL_L)
                    return;
                if (value)
                    ButtonFlags += (ushort)ASLSEntryNode.GameCubeButtons.L;
                else
                    ButtonFlags -= (ushort)ASLSEntryNode.GameCubeButtons.L;
            }
        }

        [Category("Event Match: ASL (Requires StageEx)")]
        [DisplayName("A")]
        public bool ASL_A
        {
            get => (ButtonFlags & (ushort)ASLSEntryNode.GameCubeButtons.A) == (ushort)ASLSEntryNode.GameCubeButtons.A;
            set
            {
                if (value == ASL_A)
                    return;
                if (value)
                    ButtonFlags += (ushort)ASLSEntryNode.GameCubeButtons.A;
                else
                    ButtonFlags -= (ushort)ASLSEntryNode.GameCubeButtons.A;
            }
        }

        [Category("Event Match: ASL (Requires StageEx)")]
        [DisplayName("B")]
        public bool ASL_B
        {
            get => (ButtonFlags & (ushort)ASLSEntryNode.GameCubeButtons.B) == (ushort)ASLSEntryNode.GameCubeButtons.B;
            set
            {
                if (value == ASL_B)
                    return;
                if (value)
                    ButtonFlags += (ushort)ASLSEntryNode.GameCubeButtons.B;
                else
                    ButtonFlags -= (ushort)ASLSEntryNode.GameCubeButtons.B;
            }
        }

        [Category("Event Match: ASL (Requires StageEx)")]
        [DisplayName("X")]
        public bool ASL_X
        {
            get => (ButtonFlags & (ushort)ASLSEntryNode.GameCubeButtons.X) == (ushort)ASLSEntryNode.GameCubeButtons.X;
            set
            {
                if (value == ASL_X)
                    return;
                if (value)
                    ButtonFlags += (ushort)ASLSEntryNode.GameCubeButtons.X;
                else
                    ButtonFlags -= (ushort)ASLSEntryNode.GameCubeButtons.X;
            }
        }

        [Category("Event Match: ASL (Requires StageEx)")]
        [DisplayName("Y")]
        public bool ASL_Y
        {
            get => (ButtonFlags & (ushort)ASLSEntryNode.GameCubeButtons.Y) == (ushort)ASLSEntryNode.GameCubeButtons.Y;
            set
            {
                if (value == ASL_Y)
                    return;
                if (value)
                    ButtonFlags += (ushort)ASLSEntryNode.GameCubeButtons.Y;
                else
                    ButtonFlags -= (ushort)ASLSEntryNode.GameCubeButtons.Y;
            }
        }

        [Category("Event Match: ASL (Requires StageEx)")]
        [DisplayName("Start")]
        public bool ASL_Start
        {
            get => (ButtonFlags & (ushort)ASLSEntryNode.GameCubeButtons.Start) == (ushort)ASLSEntryNode.GameCubeButtons.Start;
            set
            {
                if (value == ASL_Start)
                    return;
                if (value)
                    ButtonFlags += (ushort)ASLSEntryNode.GameCubeButtons.Start;
                else
                    ButtonFlags -= (ushort)ASLSEntryNode.GameCubeButtons.Start;
            }
        }

        [Category("Unknown")]
        public int Unknown24
        {
            get => _header._unknown24;
            set
            {
                _header._unknown24 = value;
                SignalPropertyChange();
            }
        }

        [Category("Event Match: Item Toggles (Requires ItemEx)")]
        public bool Extra3
        {
            get => _header._itemExFlags0x28[7];
            set
            {
                _header._itemExFlags0x28[7] = value;
                SignalPropertyChange();
            }
        }

        [Category("Event Match: Item Toggles (Requires ItemEx)")]
        public bool Extra2
        {
            get => _header._itemExFlags0x28[6];
            set
            {
                _header._itemExFlags0x28[6] = value;
                SignalPropertyChange();
            }
        }

        [Category("Event Match: Item Toggles (Requires ItemEx)")]
        public bool Extra1
        {
            get => _header._itemExFlags0x28[5];
            set
            {
                _header._itemExFlags0x28[5] = value;
                SignalPropertyChange();
            }
        }

        [Category("Event Match: Item Toggles (Requires ItemEx)")]
        public bool PartyBalls
        {
            get => _header._itemExFlags0x28[4];
            set
            {
                _header._itemExFlags0x28[4] = value;
                SignalPropertyChange();
            }
        }

        [Category("Event Match: Item Toggles (Requires ItemEx)")]
        public bool RollingCrates
        {
            get => _header._itemExFlags0x28[3];
            set
            {
                _header._itemExFlags0x28[3] = value;
                SignalPropertyChange();
            }
        }

        [Category("Event Match: Item Toggles (Requires ItemEx)")]
        public bool Crates
        {
            get => _header._itemExFlags0x28[2];
            set
            {
                _header._itemExFlags0x28[2] = value;
                SignalPropertyChange();
            }
        }

        [Category("Event Match: Item Toggles (Requires ItemEx)")]
        public bool Barrels
        {
            get => _header._itemExFlags0x28[1];
            set
            {
                _header._itemExFlags0x28[1] = value;
                SignalPropertyChange();
            }
        }

        [Category("Event Match: Item Toggles (Requires ItemEx)")]
        public bool Capsule
        {
            get => _header._itemExFlags0x28[0];
            set
            {
                _header._itemExFlags0x28[0] = value;
                SignalPropertyChange();
            }
        }

        [Category("Event Match: Item Toggles (Requires ItemEx)")]
        public bool ContainerExplode
        {
            get => _header._itemExFlags0x29[7];
            set
            {
                _header._itemExFlags0x29[7] = value;
                SignalPropertyChange();
            }
        }

        [Category("Event Match: Item Toggles (Requires ItemEx)")]
        public bool ContainerEnemies
        {
            get => _header._itemExFlags0x29[6];
            set
            {
                _header._itemExFlags0x29[6] = value;
                SignalPropertyChange();
            }
        }

        [Category("Event Match: Item Toggles (Requires ItemEx)")]
        public bool ContainerItems
        {
            get => _header._itemExFlags0x29[5];
            set
            {
                _header._itemExFlags0x29[5] = value;
                SignalPropertyChange();
            }
        }

        [Category("Event Match: Item Toggles (Requires ItemEx)")]
        public bool PassiveAggression
        {
            get => _header._itemExFlags0x29[4];
            set
            {
                _header._itemExFlags0x29[4] = value;
                SignalPropertyChange();
            }
        }

        [Category("Event Match: Item Toggles (Requires ItemEx)")]
        public bool Mayhem
        {
            get => _header._itemExFlags0x29[3];
            set
            {
                _header._itemExFlags0x29[3] = value;
                SignalPropertyChange();
            }
        }

        [Category("Event Match: Item Toggles (Requires ItemEx)")]
        public bool ExtraItems
        {
            get => _header._itemExFlags0x29[2];
            set
            {
                _header._itemExFlags0x29[2] = value;
                SignalPropertyChange();
            }
        }

        [Category("Event Match: Item Toggles (Requires ItemEx)")]
        public bool StageItems
        {
            get => _header._itemExFlags0x29[1];
            set
            {
                _header._itemExFlags0x29[1] = value;
                SignalPropertyChange();
            }
        }

        [Category("Event Match: Item Toggles (Requires ItemEx)")]
        public bool ScrewAttack
        {
            get => _header._itemExFlags0x29[0];
            set
            {
                _header._itemExFlags0x29[0] = value;
                SignalPropertyChange();
            }
        }

        [Category("Event Match: Item Toggles (Requires ItemEx)")]
        public bool FranklinBadge
        {
            get => _header._itemExFlags0x2A[7];
            set
            {
                _header._itemExFlags0x2A[7] = value;
                SignalPropertyChange();
            }
        }

        [Category("Event Match: Item Toggles (Requires ItemEx)")]
        public bool TeamHealer
        {
            get => _header._itemExFlags0x2A[6];
            set
            {
                _header._itemExFlags0x2A[6] = value;
                SignalPropertyChange();
            }
        }

        [Category("Event Match: Item Toggles (Requires ItemEx)")]
        public bool SoccerBall
        {
            get => _header._itemExFlags0x2A[5];
            set
            {
                _header._itemExFlags0x2A[5] = value;
                SignalPropertyChange();
            }
        }

        [Category("Event Match: Item Toggles (Requires ItemEx)")]
        public bool Unira
        {
            get => _header._itemExFlags0x2A[4];
            set
            {
                _header._itemExFlags0x2A[4] = value;
                SignalPropertyChange();
            }
        }

        [Category("Event Match: Item Toggles (Requires ItemEx)")]
        public bool Spring
        {
            get => _header._itemExFlags0x2A[3];
            set
            {
                _header._itemExFlags0x2A[3] = value;
                SignalPropertyChange();
            }
        }

        [Category("Event Match: Item Toggles (Requires ItemEx)")]
        public bool Bumper
        {
            get => _header._itemExFlags0x2A[2];
            set
            {
                _header._itemExFlags0x2A[2] = value;
                SignalPropertyChange();
            }
        }

        [Category("Event Match: Item Toggles (Requires ItemEx)")]
        public bool BananaPeel
        {
            get => _header._itemExFlags0x2A[1];
            set
            {
                _header._itemExFlags0x2A[1] = value;
                SignalPropertyChange();
            }
        }

        [Category("Event Match: Item Toggles (Requires ItemEx)")]
        public bool GreenShell
        {
            get => _header._itemExFlags0x2A[0];
            set
            {
                _header._itemExFlags0x2A[0] = value;
                SignalPropertyChange();
            }
        }

        [Category("Event Match: Item Toggles (Requires ItemEx)")]
        public bool MrSaturn
        {
            get => _header._itemExFlags0x2B[7];
            set
            {
                _header._itemExFlags0x2B[7] = value;
                SignalPropertyChange();
            }
        }

        [Category("Event Match: Item Toggles (Requires ItemEx)")]
        public bool Hothead
        {
            get => _header._itemExFlags0x2B[6];
            set
            {
                _header._itemExFlags0x2B[6] = value;
                SignalPropertyChange();
            }
        }

        [Category("Event Match: Item Toggles (Requires ItemEx)")]
        public bool Pitfall
        {
            get => _header._itemExFlags0x2B[5];
            set
            {
                _header._itemExFlags0x2B[5] = value;
                SignalPropertyChange();
            }
        }

        [Category("Event Match: Item Toggles (Requires ItemEx)")]
        public bool SmokeBall
        {
            get => _header._itemExFlags0x2B[4];
            set
            {
                _header._itemExFlags0x2B[4] = value;
                SignalPropertyChange();
            }
        }

        [Category("Event Match: Item Toggles (Requires ItemEx)")]
        public bool Freezie
        {
            get => _header._itemExFlags0x2B[3];
            set
            {
                _header._itemExFlags0x2B[3] = value;
                SignalPropertyChange();
            }
        }

        [Category("Event Match: Item Toggles (Requires ItemEx)")]
        public bool DekuNut
        {
            get => _header._itemExFlags0x2B[2];
            set
            {
                _header._itemExFlags0x2B[2] = value;
                SignalPropertyChange();
            }
        }

        [Category("Event Match: Item Toggles (Requires ItemEx)")]
        public bool SmartBomb
        {
            get => _header._itemExFlags0x2B[1];
            set
            {
                _header._itemExFlags0x2B[1] = value;
                SignalPropertyChange();
            }
        }

        [Category("Event Match: Item Toggles (Requires ItemEx)")]
        public bool GooeyBomb
        {
            get => _header._itemExFlags0x2B[0];
            set
            {
                _header._itemExFlags0x2B[0] = value;
                SignalPropertyChange();
            }
        }

        [Category("Event Match: Item Toggles (Requires ItemEx)")]
        public bool MotionSensorBomb
        {
            get => _header._itemExFlags0x2C[7];
            set
            {
                _header._itemExFlags0x2C[7] = value;
                SignalPropertyChange();
            }
        }

        [Category("Event Match: Item Toggles (Requires ItemEx)")]
        public bool BobOmb
        {
            get => _header._itemExFlags0x2C[6];
            set
            {
                _header._itemExFlags0x2C[6] = value;
                SignalPropertyChange();
            }
        }

        [Category("Event Match: Item Toggles (Requires ItemEx)")]
        public bool CrackerLauncher
        {
            get => _header._itemExFlags0x2C[5];
            set
            {
                _header._itemExFlags0x2C[5] = value;
                SignalPropertyChange();
            }
        }

        [Category("Event Match: Item Toggles (Requires ItemEx)")]
        public bool FireFlower
        {
            get => _header._itemExFlags0x2C[4];
            set
            {
                _header._itemExFlags0x2C[4] = value;
                SignalPropertyChange();
            }
        }

        [Category("Event Match: Item Toggles (Requires ItemEx)")]
        public bool RayGun
        {
            get => _header._itemExFlags0x2C[3];
            set
            {
                _header._itemExFlags0x2C[3] = value;
                SignalPropertyChange();
            }
        }

        [Category("Event Match: Item Toggles (Requires ItemEx)")]
        public bool SuperScope
        {
            get => _header._itemExFlags0x2C[2];
            set
            {
                _header._itemExFlags0x2C[2] = value;
                SignalPropertyChange();
            }
        }

        [Category("Event Match: Item Toggles (Requires ItemEx)")]
        public bool GoldenHammer
        {
            get => _header._itemExFlags0x2C[1];
            set
            {
                _header._itemExFlags0x2C[1] = value;
                SignalPropertyChange();
            }
        }

        [Category("Event Match: Item Toggles (Requires ItemEx)")]
        public bool Hammer
        {
            get => _header._itemExFlags0x2C[0];
            set
            {
                _header._itemExFlags0x2C[0] = value;
                SignalPropertyChange();
            }
        }

        [Category("Event Match: Item Toggles (Requires ItemEx)")]
        public bool StarRod
        {
            get => _header._itemExFlags0x2D[7];
            set
            {
                _header._itemExFlags0x2D[7] = value;
                SignalPropertyChange();
            }
        }

        [Category("Event Match: Item Toggles (Requires ItemEx)")]
        public bool LipStick
        {
            get => _header._itemExFlags0x2D[6];
            set
            {
                _header._itemExFlags0x2D[6] = value;
                SignalPropertyChange();
            }
        }

        [Category("Event Match: Item Toggles (Requires ItemEx)")]
        public bool Fan
        {
            get => _header._itemExFlags0x2D[5];
            set
            {
                _header._itemExFlags0x2D[5] = value;
                SignalPropertyChange();
            }
        }

        [Category("Event Match: Item Toggles (Requires ItemEx)")]
        public bool HomeRunBat
        {
            get => _header._itemExFlags0x2D[4];
            set
            {
                _header._itemExFlags0x2D[4] = value;
                SignalPropertyChange();
            }
        }

        [Category("Event Match: Item Toggles (Requires ItemEx)")]
        public bool BeamSword
        {
            get => _header._itemExFlags0x2D[3];
            set
            {
                _header._itemExFlags0x2D[3] = value;
                SignalPropertyChange();
            }
        }

        [Category("Event Match: Item Toggles (Requires ItemEx)")]
        public bool Lightning
        {
            get => _header._itemExFlags0x2D[2];
            set
            {
                _header._itemExFlags0x2D[2] = value;
                SignalPropertyChange();
            }
        }

        [Category("Event Match: Item Toggles (Requires ItemEx)")]
        public bool Timer
        {
            get => _header._itemExFlags0x2D[1];
            set
            {
                _header._itemExFlags0x2D[1] = value;
                SignalPropertyChange();
            }
        }

        [Category("Event Match: Item Toggles (Requires ItemEx)")]
        public bool SuperspicyCurry
        {
            get => _header._itemExFlags0x2D[0];
            set
            {
                _header._itemExFlags0x2D[0] = value;
                SignalPropertyChange();
            }
        }

        [Category("Event Match: Item Toggles (Requires ItemEx)")]
        public bool BunnyHood
        {
            get => _header._itemExFlags0x2E[7];
            set
            {
                _header._itemExFlags0x2E[7] = value;
                SignalPropertyChange();
            }
        }

        [Category("Event Match: Item Toggles (Requires ItemEx)")]
        public bool MetalBox
        {
            get => _header._itemExFlags0x2E[6];
            set
            {
                _header._itemExFlags0x2E[6] = value;
                SignalPropertyChange();
            }
        }

        [Category("Event Match: Item Toggles (Requires ItemEx)")]
        public bool Starman
        {
            get => _header._itemExFlags0x2E[5];
            set
            {
                _header._itemExFlags0x2E[5] = value;
                SignalPropertyChange();
            }
        }

        [Category("Event Match: Item Toggles (Requires ItemEx)")]
        public bool WarpStar
        {
            get => _header._itemExFlags0x2E[4];
            set
            {
                _header._itemExFlags0x2E[4] = value;
                SignalPropertyChange();
            }
        }

        [Category("Event Match: Item Toggles (Requires ItemEx)")]
        public bool PoisonMushroom
        {
            get => _header._itemExFlags0x2E[3];
            set
            {
                _header._itemExFlags0x2E[3] = value;
                SignalPropertyChange();
            }
        }

        [Category("Event Match: Item Toggles (Requires ItemEx)")]
        public bool SuperMushroom
        {
            get => _header._itemExFlags0x2E[2];
            set
            {
                _header._itemExFlags0x2E[2] = value;
                SignalPropertyChange();
            }
        }

        [Category("Event Match: Item Toggles (Requires ItemEx)")]
        public bool DragoonParts
        {
            get => _header._itemExFlags0x2E[1];
            set
            {
                _header._itemExFlags0x2E[1] = value;
                SignalPropertyChange();
            }
        }

        [Category("Event Match: Item Toggles (Requires ItemEx)")]
        public bool HeartContainer
        {
            get => _header._itemExFlags0x2E[0];
            set
            {
                _header._itemExFlags0x2E[0] = value;
                SignalPropertyChange();
            }
        }

        [Category("Event Match: Item Toggles (Requires ItemEx)")]
        public bool MaximTomato
        {
            get => _header._itemExFlags0x2F[7];
            set
            {
                _header._itemExFlags0x2F[7] = value;
                SignalPropertyChange();
            }
        }

        [Category("Event Match: Item Toggles (Requires ItemEx)")]
        public bool Food
        {
            get => _header._itemExFlags0x2F[6];
            set
            {
                _header._itemExFlags0x2F[6] = value;
                SignalPropertyChange();
            }
        }

        [Category("Event Match: Item Toggles (Requires ItemEx)")]
        public bool SandBag
        {
            get => _header._itemExFlags0x2F[5];
            set
            {
                _header._itemExFlags0x2F[5] = value;
                SignalPropertyChange();
            }
        }

        [Category("Event Match: Item Toggles (Requires ItemEx)")]
        public bool BlastBox
        {
            get => _header._itemExFlags0x2F[4];
            set
            {
                _header._itemExFlags0x2F[4] = value;
                SignalPropertyChange();
            }
        }

        [Category("Event Match: Item Toggles (Requires ItemEx)")]
        public bool Containers
        {
            get => _header._itemExFlags0x2F[3];
            set
            {
                _header._itemExFlags0x2F[3] = value;
                SignalPropertyChange();
            }
        }

        [Category("Event Match: Item Toggles (Requires ItemEx)")]
        public bool PokeBall
        {
            get => _header._itemExFlags0x2F[2];
            set
            {
                _header._itemExFlags0x2F[2] = value;
                SignalPropertyChange();
            }
        }

        [Category("Event Match: Item Toggles (Requires ItemEx)")]
        public bool AssistTrophy
        {
            get => _header._itemExFlags0x2F[1];
            set
            {
                _header._itemExFlags0x2F[1] = value;
                SignalPropertyChange();
            }
        }

        [Category("Event Match: Item Toggles (Requires ItemEx)")]
        public bool SmashBall
        {
            get => _header._itemExFlags0x2F[0];
            set
            {
                _header._itemExFlags0x2F[0] = value;
                SignalPropertyChange();
            }
        }

        [Category("Event Match: Summon (Poké Ball) Toggles (Requires ItemEx)")]
        public bool PokeBallUncapped
        {
            get => _header._itemExFlags0x30[7];
            set
            {
                _header._itemExFlags0x30[7] = value;
                SignalPropertyChange();
            }
        }

        [Category("Event Match: Summon (Poké Ball) Toggles (Requires ItemEx)")]
        public bool PokeBallExtra4
        {
            get => _header._itemExFlags0x30[6];
            set
            {
                _header._itemExFlags0x30[6] = value;
                SignalPropertyChange();
            }
        }

        [Category("Event Match: Summon (Poké Ball) Toggles (Requires ItemEx)")]
        public bool PokeBallExtra3
        {
            get => _header._itemExFlags0x30[5];
            set
            {
                _header._itemExFlags0x30[5] = value;
                SignalPropertyChange();
            }
        }

        [Category("Event Match: Summon (Poké Ball) Toggles (Requires ItemEx)")]
        public bool PokeBallExtra2
        {
            get => _header._itemExFlags0x30[4];
            set
            {
                _header._itemExFlags0x30[4] = value;
                SignalPropertyChange();
            }
        }

        [Category("Event Match: Summon (Poké Ball) Toggles (Requires ItemEx)")]
        public bool PokeBallExtra1
        {
            get => _header._itemExFlags0x30[3];
            set
            {
                _header._itemExFlags0x30[3] = value;
                SignalPropertyChange();
            }
        }

        [Category("Event Match: Summon (Poké Ball) Toggles (Requires ItemEx)")]
        public bool Bonsly
        {
            get => _header._itemExFlags0x30[2];
            set
            {
                _header._itemExFlags0x30[2] = value;
                SignalPropertyChange();
            }
        }

        [Category("Event Match: Summon (Poké Ball) Toggles (Requires ItemEx)")]
        public bool Suicune
        {
            get => _header._itemExFlags0x30[1];
            set
            {
                _header._itemExFlags0x30[1] = value;
                SignalPropertyChange();
            }
        }

        [Category("Event Match: Summon (Poké Ball) Toggles (Requires ItemEx)")]
        public bool Wobuffet
        {
            get => _header._itemExFlags0x30[0];
            set
            {
                _header._itemExFlags0x30[0] = value;
                SignalPropertyChange();
            }
        }

        [Category("Event Match: Summon (Poké Ball) Toggles (Requires ItemEx)")]
        public bool Gardevoir
        {
            get => _header._itemExFlags0x31[7];
            set
            {
                _header._itemExFlags0x31[7] = value;
                SignalPropertyChange();
            }
        }

        [Category("Event Match: Summon (Poké Ball) Toggles (Requires ItemEx)")]
        public bool Goldeen
        {
            get => _header._itemExFlags0x31[6];
            set
            {
                _header._itemExFlags0x31[6] = value;
                SignalPropertyChange();
            }
        }

        [Category("Event Match: Summon (Poké Ball) Toggles (Requires ItemEx)")]
        public bool Togepi
        {
            get => _header._itemExFlags0x31[5];
            set
            {
                _header._itemExFlags0x31[5] = value;
                SignalPropertyChange();
            }
        }

        [Category("Event Match: Summon (Poké Ball) Toggles (Requires ItemEx)")]
        public bool Piplup
        {
            get => _header._itemExFlags0x31[4];
            set
            {
                _header._itemExFlags0x31[4] = value;
                SignalPropertyChange();
            }
        }

        [Category("Event Match: Summon (Poké Ball) Toggles (Requires ItemEx)")]
        public bool Meowth
        {
            get => _header._itemExFlags0x31[3];
            set
            {
                _header._itemExFlags0x31[3] = value;
                SignalPropertyChange();
            }
        }

        [Category("Event Match: Summon (Poké Ball) Toggles (Requires ItemEx)")]
        public bool Metagross
        {
            get => _header._itemExFlags0x31[2];
            set
            {
                _header._itemExFlags0x31[2] = value;
                SignalPropertyChange();
            }
        }

        [Category("Event Match: Summon (Poké Ball) Toggles (Requires ItemEx)")]
        public bool Electrode
        {
            get => _header._itemExFlags0x31[1];
            set
            {
                _header._itemExFlags0x31[1] = value;
                SignalPropertyChange();
            }
        }

        [Category("Event Match: Summon (Poké Ball) Toggles (Requires ItemEx)")]
        public bool Weavile
        {
            get => _header._itemExFlags0x31[0];
            set
            {
                _header._itemExFlags0x31[0] = value;
                SignalPropertyChange();
            }
        }

        [Category("Event Match: Summon (Poké Ball) Toggles (Requires ItemEx)")]
        public bool Manaphy
        {
            get => _header._itemExFlags0x32[7];
            set
            {
                _header._itemExFlags0x32[7] = value;
                SignalPropertyChange();
            }
        }

        [Category("Event Match: Summon (Poké Ball) Toggles (Requires ItemEx)")]
        public bool Lugia
        {
            get => _header._itemExFlags0x32[6];
            set
            {
                _header._itemExFlags0x32[6] = value;
                SignalPropertyChange();
            }
        }

        [Category("Event Match: Summon (Poké Ball) Toggles (Requires ItemEx)")]
        public bool LatiasLatios
        {
            get => _header._itemExFlags0x32[5];
            set
            {
                _header._itemExFlags0x32[5] = value;
                SignalPropertyChange();
            }
        }

        [Category("Event Match: Summon (Poké Ball) Toggles (Requires ItemEx)")]
        public bool Kyogre
        {
            get => _header._itemExFlags0x32[4];
            set
            {
                _header._itemExFlags0x32[4] = value;
                SignalPropertyChange();
            }
        }

        [Category("Event Match: Summon (Poké Ball) Toggles (Requires ItemEx)")]
        public bool Bellosom
        {
            get => _header._itemExFlags0x32[3];
            set
            {
                _header._itemExFlags0x32[3] = value;
                SignalPropertyChange();
            }
        }

        [Category("Event Match: Summon (Poké Ball) Toggles (Requires ItemEx)")]
        public bool Snorlax
        {
            get => _header._itemExFlags0x32[2];
            set
            {
                _header._itemExFlags0x32[2] = value;
                SignalPropertyChange();
            }
        }

        [Category("Event Match: Summon (Poké Ball) Toggles (Requires ItemEx)")]
        public bool HoOh
        {
            get => _header._itemExFlags0x32[1];
            set
            {
                _header._itemExFlags0x32[1] = value;
                SignalPropertyChange();
            }
        }

        [Category("Event Match: Summon (Poké Ball) Toggles (Requires ItemEx)")]
        public bool Staryu
        {
            get => _header._itemExFlags0x32[0];
            set
            {
                _header._itemExFlags0x32[0] = value;
                SignalPropertyChange();
            }
        }

        [Category("Event Match: Summon (Poké Ball) Toggles (Requires ItemEx)")]
        public bool Gulpin
        {
            get => _header._itemExFlags0x33[7];
            set
            {
                _header._itemExFlags0x33[7] = value;
                SignalPropertyChange();
            }
        }

        [Category("Event Match: Summon (Poké Ball) Toggles (Requires ItemEx)")]
        public bool Groudon
        {
            get => _header._itemExFlags0x33[6];
            set
            {
                _header._itemExFlags0x33[6] = value;
                SignalPropertyChange();
            }
        }

        [Category("Event Match: Summon (Poké Ball) Toggles (Requires ItemEx)")]
        public bool Deoxys
        {
            get => _header._itemExFlags0x33[5];
            set
            {
                _header._itemExFlags0x33[5] = value;
                SignalPropertyChange();
            }
        }

        [Category("Event Match: Summon (Poké Ball) Toggles (Requires ItemEx)")]
        public bool Munchlax
        {
            get => _header._itemExFlags0x33[4];
            set
            {
                _header._itemExFlags0x33[4] = value;
                SignalPropertyChange();
            }
        }

        [Category("Event Match: Summon (Poké Ball) Toggles (Requires ItemEx)")]
        public bool Moltres
        {
            get => _header._itemExFlags0x33[3];
            set
            {
                _header._itemExFlags0x33[3] = value;
                SignalPropertyChange();
            }
        }

        [Category("Event Match: Summon (Poké Ball) Toggles (Requires ItemEx)")]
        public bool Entei
        {
            get => _header._itemExFlags0x33[2];
            set
            {
                _header._itemExFlags0x33[2] = value;
                SignalPropertyChange();
            }
        }

        [Category("Event Match: Summon (Poké Ball) Toggles (Requires ItemEx)")]
        public bool Chikorita
        {
            get => _header._itemExFlags0x33[1];
            set
            {
                _header._itemExFlags0x33[1] = value;
                SignalPropertyChange();
            }
        }

        [Category("Event Match: Summon (Poké Ball) Toggles (Requires ItemEx)")]
        public bool Torchic
        {
            get => _header._itemExFlags0x33[0];
            set
            {
                _header._itemExFlags0x33[0] = value;
                SignalPropertyChange();
            }
        }

        [Category("Event Match: Summon (Assist Trophy) Toggles (Requires ItemEx)")]
        public bool AssistTrophyUncapped
        {
            get => _header._itemExFlags0x34[7];
            set
            {
                _header._itemExFlags0x34[7] = value;
                SignalPropertyChange();
            }
        }

        [Category("Event Match: Summon (Assist Trophy) Toggles (Requires ItemEx)")]
        public bool AssistTrophyExtra4
        {
            get => _header._itemExFlags0x34[6];
            set
            {
                _header._itemExFlags0x34[6] = value;
                SignalPropertyChange();
            }
        }

        [Category("Event Match: Summon (Assist Trophy) Toggles (Requires ItemEx)")]
        public bool AssistTrophyExtra3
        {
            get => _header._itemExFlags0x34[5];
            set
            {
                _header._itemExFlags0x34[5] = value;
                SignalPropertyChange();
            }
        }

        [Category("Event Match: Summon (Assist Trophy) Toggles (Requires ItemEx)")]
        public bool AssistTrophyExtra2
        {
            get => _header._itemExFlags0x34[4];
            set
            {
                _header._itemExFlags0x34[4] = value;
                SignalPropertyChange();
            }
        }

        [Category("Event Match: Summon (Assist Trophy) Toggles (Requires ItemEx)")]
        public bool AssistTrophyExtra1
        {
            get => _header._itemExFlags0x34[3];
            set
            {
                _header._itemExFlags0x34[3] = value;
                SignalPropertyChange();
            }
        }

        [Category("Event Match: Summon (Assist Trophy) Toggles (Requires ItemEx)")]
        public bool DrWright
        {
            get => _header._itemExFlags0x34[2];
            set
            {
                _header._itemExFlags0x34[2] = value;
                SignalPropertyChange();
            }
        }

        [Category("Event Match: Summon (Assist Trophy) Toggles (Requires ItemEx)")]
        public bool Waluigi
        {
            get => _header._itemExFlags0x34[1];
            set
            {
                _header._itemExFlags0x34[1] = value;
                SignalPropertyChange();
            }
        }

        [Category("Event Match: Summon (Assist Trophy) Toggles (Requires ItemEx)")]
        public bool Tingle
        {
            get => _header._itemExFlags0x34[0];
            set
            {
                _header._itemExFlags0x34[0] = value;
                SignalPropertyChange();
            }
        }

        [Category("Event Match: Summon (Assist Trophy) Toggles (Requires ItemEx)")]
        public bool InfantryTank
        {
            get => _header._itemExFlags0x35[7];
            set
            {
                _header._itemExFlags0x35[7] = value;
                SignalPropertyChange();
            }
        }

        [Category("Event Match: Summon (Assist Trophy) Toggles (Requires ItemEx)")]
        public bool Starfy
        {
            get => _header._itemExFlags0x35[6];
            set
            {
                _header._itemExFlags0x35[6] = value;
                SignalPropertyChange();
            }
        }

        [Category("Event Match: Summon (Assist Trophy) Toggles (Requires ItemEx)")]
        public bool Shadow
        {
            get => _header._itemExFlags0x35[5];
            set
            {
                _header._itemExFlags0x35[5] = value;
                SignalPropertyChange();
            }
        }

        [Category("Event Match: Summon (Assist Trophy) Toggles (Requires ItemEx)")]
        public bool Saki
        {
            get => _header._itemExFlags0x35[4];
            set
            {
                _header._itemExFlags0x35[4] = value;
                SignalPropertyChange();
            }
        }

        [Category("Event Match: Summon (Assist Trophy) Toggles (Requires ItemEx)")]
        public bool Isaac
        {
            get => _header._itemExFlags0x35[3];
            set
            {
                _header._itemExFlags0x35[3] = value;
                SignalPropertyChange();
            }
        }

        [Category("Event Match: Summon (Assist Trophy) Toggles (Requires ItemEx)")]
        public bool MrResetti
        {
            get => _header._itemExFlags0x35[2];
            set
            {
                _header._itemExFlags0x35[2] = value;
                SignalPropertyChange();
            }
        }

        [Category("Event Match: Summon (Assist Trophy) Toggles (Requires ItemEx)")]
        public bool Nintendog
        {
            get => _header._itemExFlags0x35[1];
            set
            {
                _header._itemExFlags0x35[1] = value;
                SignalPropertyChange();
            }
        }

        [Category("Event Match: Summon (Assist Trophy) Toggles (Requires ItemEx)")]
        public bool Metroid
        {
            get => _header._itemExFlags0x35[0];
            set
            {
                _header._itemExFlags0x35[0] = value;
                SignalPropertyChange();
            }
        }

        [Category("Event Match: Summon (Assist Trophy) Toggles (Requires ItemEx)")]
        public bool LittleMac
        {
            get => _header._itemExFlags0x36[7];
            set
            {
                _header._itemExFlags0x36[7] = value;
                SignalPropertyChange();
            }
        }

        [Category("Event Match: Summon (Assist Trophy) Toggles (Requires ItemEx)")]
        public bool Lyn
        {
            get => _header._itemExFlags0x36[6];
            set
            {
                _header._itemExFlags0x36[6] = value;
                SignalPropertyChange();
            }
        }

        [Category("Event Match: Summon (Assist Trophy) Toggles (Requires ItemEx)")]
        public bool JillDozer
        {
            get => _header._itemExFlags0x36[5];
            set
            {
                _header._itemExFlags0x36[5] = value;
                SignalPropertyChange();
            }
        }

        [Category("Event Match: Summon (Assist Trophy) Toggles (Requires ItemEx)")]
        public bool KatAna
        {
            get => _header._itemExFlags0x36[4];
            set
            {
                _header._itemExFlags0x36[4] = value;
                SignalPropertyChange();
            }
        }

        [Category("Event Match: Summon (Assist Trophy) Toggles (Requires ItemEx)")]
        public bool Helirin
        {
            get => _header._itemExFlags0x36[3];
            set
            {
                _header._itemExFlags0x36[3] = value;
                SignalPropertyChange();
            }
        }

        [Category("Event Match: Summon (Assist Trophy) Toggles (Requires ItemEx)")]
        public bool HammerBro
        {
            get => _header._itemExFlags0x36[2];
            set
            {
                _header._itemExFlags0x36[2] = value;
                SignalPropertyChange();
            }
        }

        [Category("Event Match: Summon (Assist Trophy) Toggles (Requires ItemEx)")]
        public bool KnuckleJoe
        {
            get => _header._itemExFlags0x36[1];
            set
            {
                _header._itemExFlags0x36[1] = value;
                SignalPropertyChange();
            }
        }

        [Category("Event Match: Summon (Assist Trophy) Toggles (Requires ItemEx)")]
        public bool Lakitu
        {
            get => _header._itemExFlags0x36[0];
            set
            {
                _header._itemExFlags0x36[0] = value;
                SignalPropertyChange();
            }
        }

        [Category("Event Match: Summon (Assist Trophy) Toggles (Requires ItemEx)")]
        public bool Jeff
        {
            get => _header._itemExFlags0x37[7];
            set
            {
                _header._itemExFlags0x37[7] = value;
                SignalPropertyChange();
            }
        }

        [Category("Event Match: Summon (Assist Trophy) Toggles (Requires ItemEx)")]
        public bool Excitebike
        {
            get => _header._itemExFlags0x37[6];
            set
            {
                _header._itemExFlags0x37[6] = value;
                SignalPropertyChange();
            }
        }

        [Category("Event Match: Summon (Assist Trophy) Toggles (Requires ItemEx)")]
        public bool Devil
        {
            get => _header._itemExFlags0x37[5];
            set
            {
                _header._itemExFlags0x37[5] = value;
                SignalPropertyChange();
            }
        }

        [Category("Event Match: Summon (Assist Trophy) Toggles (Requires ItemEx)")]
        public bool SamuraiGoroh
        {
            get => _header._itemExFlags0x37[4];
            set
            {
                _header._itemExFlags0x37[4] = value;
                SignalPropertyChange();
            }
        }

        [Category("Event Match: Summon (Assist Trophy) Toggles (Requires ItemEx)")]
        public bool RayMKII
        {
            get => _header._itemExFlags0x37[3];
            set
            {
                _header._itemExFlags0x37[3] = value;
                SignalPropertyChange();
            }
        }

        [Category("Event Match: Summon (Assist Trophy) Toggles (Requires ItemEx)")]
        public bool GrayFox
        {
            get => _header._itemExFlags0x37[2];
            set
            {
                _header._itemExFlags0x37[2] = value;
                SignalPropertyChange();
            }
        }

        [Category("Event Match: Summon (Assist Trophy) Toggles (Requires ItemEx)")]
        public bool Barbara
        {
            get => _header._itemExFlags0x37[1];
            set
            {
                _header._itemExFlags0x37[1] = value;
                SignalPropertyChange();
            }
        }

        [Category("Event Match: Summon (Assist Trophy) Toggles (Requires ItemEx)")]
        public bool Andross
        {
            get => _header._itemExFlags0x37[0];
            set
            {
                _header._itemExFlags0x37[0] = value;
                SignalPropertyChange();
            }
        }

        [Category("Event Match")]
        [DisplayName("Game Speed")]
        public float GameSpeed
        {
            get => _header._gameSpeed;
            set
            {
                _header._gameSpeed = value;
                SignalPropertyChange();
            }
        }

        [Category("Event Match")]
        [DisplayName("Camera Shake Control")]
        public float CameraShakeControl
        {
            get => _header._cameraShakeControl;
            set
            {
                _header._cameraShakeControl = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public bool UnknownFlag_40_80000000
        {
            get => (_header._flags40 & 0x80000000) != 0;
            set
            {
                SignalPropertyChange();
                if (value)
                {
                    _header._flags40 |= unchecked((int) 0x80000000);
                }
                else
                {
                    _header._flags40 &= ~unchecked((int) 0x80000000);
                }
            }
        }

        [Category("Event Match")]
        [DisplayName("Song ID")]
        public int SongID
        {
            get => _header._songID;
            set
            {
                _header._songID = value;
                SignalPropertyChange();
            }
        }

        [Category("Event Match")]
        [DisplayName("Global Offense Ratio")]
        public short GlobalOffenseRatio
        {
            get => _header._globalOffenseRatio;
            set
            {
                _header._globalOffenseRatio = value;
                SignalPropertyChange();
            }
        }

        [Category("Event Match")]
        [DisplayName("Global Defense Ratio")]
        public short GlobalDefenseRatio
        {
            get => _header._globalDefenseRatio;
            set
            {
                _header._globalDefenseRatio = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public int Unknown4c
        {
            get => _header._unknown4c;
            set
            {
                _header._unknown4c = value;
                SignalPropertyChange();
            }
        }

        public override bool OnInitialize()
        {
            base.OnInitialize();

            // Copy the data from the address
            EventMatchTblHeader* dataPtr = (EventMatchTblHeader*) WorkingUncompressed.Address;
            _header = *dataPtr;

            return true;
        }

        public override void OnPopulate()
        {
            int numFighters =
                _header._eventExtension == 0 ? 4
                : _header._eventExtension == 1 ? 9
                : _header._eventExtension == 2 ? 38
                : 0;

            if (numFighters == 0)
            {
                MessageBox.Show($"Unknown event extension {_header._eventExtension} (expected 0, 1, or 2)", "Warning",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                numFighters = (WorkingUncompressed.Length - sizeof(EventMatchTblHeader)) /
                              sizeof(EventMatchFighterData);
            }

            VoidPtr ptr = WorkingUncompressed.Address + sizeof(EventMatchTblHeader);
            for (int i = 0; i < numFighters; i++)
            {
                DataSource source = new DataSource(ptr, sizeof(EventMatchFighterData));
                new EventMatchFighterNode().Initialize(this, source);
                ptr += sizeof(EventMatchFighterData);
            }
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            switch (Children.Count)
            {
                case 4:
                    _header._eventExtension = 0;
                    break;
                case 9:
                    _header._eventExtension = 1;
                    break;
                case 38:
                    _header._eventExtension = 2;
                    break;
                default:
                    throw new Exception("Invalid number of children for EventMatchNode (must be 4, 9, or 38)");
            }

            // Copy the data back to the address
            EventMatchTblHeader* dataPtr = (EventMatchTblHeader*) address;
            *dataPtr = _header;

            // Rebuild children using new address
            VoidPtr ptr = address + sizeof(EventMatchTblHeader);
            for (int i = 0; i < Children.Count; i++)
            {
                Children[i].Rebuild(ptr, sizeof(EventMatchFighterData), true);
                ptr += sizeof(EventMatchFighterData);
            }
        }

        public override int OnCalculateSize(bool force)
        {
            int size = sizeof(EventMatchTblHeader);
            foreach (ResourceNode node in Children)
            {
                size += node.CalculateSize(true);
            }

            return size;
        }
    }
}