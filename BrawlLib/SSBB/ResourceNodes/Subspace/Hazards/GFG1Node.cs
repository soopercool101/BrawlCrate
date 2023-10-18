using BrawlLib.CustomLists;
using BrawlLib.Internal;
using BrawlLib.SSBB.Types.Subspace.Hazards;
using System;
using System.ComponentModel;

namespace BrawlLib.SSBB.ResourceNodes
{
    public class GFG1Node : BLOCEntryNode
    {
        public override Type SubEntryType => typeof(GFG1EntryNode);
        protected override string baseName => "Fighter Spawns";

        internal static ResourceNode TryParse(DataSource source, ResourceNode parent)
        {
            return source.Tag == "GFG1" ? new GFG1Node() : null;
        }
    }

    public unsafe class GFG1EntryNode : ResourceNode
    {
        protected internal GFG1Entry Data;

        [Category("Fighter")]
        public byte FighterID
        {
            get => Data._fighterID;
            set
            {
                Data._fighterID = value;
                Name = FighterNameGenerators.FromID(Data._fighterID, FighterNameGenerators.slotIDIndex, "+S") +
                       $" [{Index}]";
                SignalPropertyChange();
            }
        }

        [Category("Fighter")]
        public byte StockCount
        {
            get => Data._stockCount;
            set
            {
                Data._stockCount = value;
                SignalPropertyChange();
            }
        }

        [Category("Fighter")]
        public byte CostumeID
        {
            get => Data._costumeID;
            set
            {
                Data._costumeID = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x03 (byte)")]
        [Category("Unknown")]
        public byte Unknown0x03
        {
            get => Data._unknown0x03;
            set
            {
                Data._unknown0x03 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x04 (byte)")]
        [Category("Unknown")]
        public byte Unknown0x04
        {
            get => Data._unknown0x04;
            set
            {
                Data._unknown0x04 = value;
                SignalPropertyChange();
            }
        }

        [Category("Fighter")]
        public bool IsTeamMember
        {
            get => Data._isTeamMember;
            set
            {
                Data._isTeamMember = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x06 (byte)")]
        [Category("Unknown")]
        public byte Unknown0x06
        {
            get => Data._unknown0x06;
            set
            {
                Data._unknown0x06 = value;
                SignalPropertyChange();
            }
        }

        [Category("Fighter")]
        public bool IsMetal
        {
            get => Data._isMetal;
            set
            {
                Data._isMetal = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x08 (byte)")]
        [Category("Unknown")]
        public byte Unknown0x08
        {
            get => Data._unknown0x08;
            set
            {
                Data._unknown0x08 = value;
                SignalPropertyChange();
            }
        }

        [Category("Fighter")]
        public bool IsSpy
        {
            get => Data._isSpy;
            set
            {
                Data._isSpy = value;
                SignalPropertyChange();
            }
        }

        [Category("Fighter")]
        public bool IsLowGravity
        {
            get => Data._isLowGravity;
            set
            {
                Data._isLowGravity = value;
                SignalPropertyChange();
            }
        }

        [Category("Fighter")]
        public bool HasNoVoice
        {
            get => Data._hasNoVoice;
            set
            {
                Data._hasNoVoice = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x0C (byte)")]
        [Category("Unknown")]
        public byte Unknown0x0C
        {
            get => Data._unknown0x0C;
            set
            {
                Data._unknown0x0C = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x0D (byte)")]
        [Category("Unknown")]
        public byte Unknown0x0D
        {
            get => Data._unknown0x0D;
            set
            {
                Data._unknown0x0D = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x0E (byte)")]
        [Category("Unknown")]
        public byte Unknown0x0E
        {
            get => Data._unknown0x0E;
            set
            {
                Data._unknown0x0E = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x0F (byte)")]
        [Category("Unknown")]
        public byte Unknown0x0F
        {
            get => Data._unknown0x0F;
            set
            {
                Data._unknown0x0F = value;
                SignalPropertyChange();
            }
        }

        [Category("Fighter")]
        public bool DisplayStaminaHitPoints
        {
            get => Data._displayStaminaHitPoints;
            set
            {
                Data._displayStaminaHitPoints = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x11 (byte)")]
        [Category("Unknown")]
        public byte Unknown0x11
        {
            get => Data._unknown0x11;
            set
            {
                Data._unknown0x11 = value;
                SignalPropertyChange();
            }
        }

        [Category("Fighter")]
        public byte CpuType
        {
            get => Data._cpuType;
            set
            {
                Data._cpuType = value;
                SignalPropertyChange();
            }
        }

        [Category("Fighter")]
        public byte CpuRank
        {
            get => Data._cpuRank;
            set
            {
                Data._cpuRank = value;
                SignalPropertyChange();
            }
        }

        [Category("Fighter")]
        public byte PlayerNumber
        {
            get => Data._playerNumber;
            set
            {
                Data._playerNumber = value;
                SignalPropertyChange();
            }
        }
        public enum CostumeTypes : byte
        {
            Normal = 0,
            Dark = 1,
            Fake = 2
        }

        [Category("Fighter")]
        public CostumeTypes CostumeType
        {
            get => (CostumeTypes)Data._costumeType;
            set
            {
                Data._costumeType = (byte)value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x16 (byte)")]
        [Category("Unknown")]
        public byte Unknown0x16
        {
            get => Data._unknown0x16;
            set
            {
                Data._unknown0x16 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x17 (byte)")]
        [Category("Unknown")]
        public byte Unknown0x17
        {
            get => Data._unknown0x17;
            set
            {
                Data._unknown0x17 = value;
                SignalPropertyChange();
            }
        }

        [Category("Fighter")]
        public short StartDamage
        {
            get => Data._startDamage;
            set
            {
                Data._startDamage = value;
                SignalPropertyChange();
            }
        }

        [Category("Fighter")]
        public short HitPointMax
        {
            get => Data._hitPointMax;
            set
            {
                Data._hitPointMax = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x1C (uint)")]
        [Category("Unknown")]
        public uint Unknown0x1C
        {
            get => Data._unknown0x1C;
            set
            {
                Data._unknown0x1C = value;
                SignalPropertyChange();
            }
        }

        [Category("Fighter")]
        public short GlowAttack
        {
            get => Data._glowAttack;
            set
            {
                Data._glowAttack = value;
                SignalPropertyChange();
            }
        }

        [Category("Fighter")]
        public short GlowDefense
        {
            get => Data._glowDefense;
            set
            {
                Data._glowDefense = value;
                SignalPropertyChange();
            }
        }

        [Category("Fighter")]
        public float OffensiveKnockbackMultiplier
        {
            get => Data._offensiveKnockbackMultiplier;
            set
            {
                Data._offensiveKnockbackMultiplier = value;
                SignalPropertyChange();
            }
        }

        [Category("Fighter")]
        public float DefensiveKnockbackMultiplier
        {
            get => Data._defensiveKnockbackMultiplier;
            set
            {
                Data._defensiveKnockbackMultiplier = value;
                SignalPropertyChange();
            }
        }

        [Category("Fighter")]
        public float Scale
        {
            get => Data._scale;
            set
            {
                Data._scale = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x30 (uint)")]
        [Category("Unknown")]
        public uint Unknown0x30
        {
            get => Data._unknown0x30;
            set
            {
                Data._unknown0x30 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x34 (uint)")]
        [Category("Unknown")]
        public uint Unknown0x34
        {
            get => Data._unknown0x34;
            set
            {
                Data._unknown0x34 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x38 (uint)")]
        [Category("Unknown")]
        public uint Unknown0x38
        {
            get => Data._unknown0x38;
            set
            {
                Data._unknown0x38 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x3C (uint)")]
        [Category("Unknown")]
        public uint Unknown0x3C
        {
            get => Data._unknown0x3C;
            set
            {
                Data._unknown0x3C = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x40 (uint)")]
        [Category("Unknown")]
        public uint Unknown0x40
        {
            get => Data._unknown0x40;
            set
            {
                Data._unknown0x40 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x44 (uint)")]
        [Category("Unknown")]
        public uint Unknown0x44
        {
            get => Data._unknown0x44;
            set
            {
                Data._unknown0x44 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Unknown0x48 (uint)")]
        [Category("Unknown")]
        public uint Unknown0x48
        {
            get => Data._unknown0x48;
            set
            {
                Data._unknown0x48 = value;
                SignalPropertyChange();
            }
        }

        public TriggerDataClass _unknown0x4C;
        [TypeConverter(typeof(ExpandableObjectCustomConverter))]
        [DisplayName("Unknown0x4C (TriggerData)")]
        [Category("Unknown")]
        public TriggerDataClass Unknown0x4C
        {
            get => _unknown0x4C ?? new TriggerDataClass(this);
            set
            {
                _unknown0x4C = value;
                Data._unknown0x4C = value;
                SignalPropertyChange();
            }
        }

        public TriggerDataClass _unknown0x50;
        [TypeConverter(typeof(ExpandableObjectCustomConverter))]
        [DisplayName("Unknown0x50 (TriggerData)")]
        [Category("Unknown")]
        public TriggerDataClass Unknown0x50
        {
            get => _unknown0x50 ?? new TriggerDataClass(this);
            set
            {
                _unknown0x50 = value;
                Data._unknown0x50 = value;
                SignalPropertyChange();
            }
        }

        public override int OnCalculateSize(bool force)
        {
            return GFG1Entry.Size;
        }

        public override bool OnInitialize()
        {
            Data = *(GFG1Entry*) WorkingUncompressed.Address;
            _unknown0x4C = new TriggerDataClass(this, Data._unknown0x4C);
            _unknown0x50 = new TriggerDataClass(this, Data._unknown0x50);

            if (_name == null)
            {
                _name = FighterNameGenerators.FromID(Data._fighterID, FighterNameGenerators.slotIDIndex, "+S") +
                        $" [{Index}]";
            }

            return base.OnInitialize();
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            GFG1Entry* hdr = (GFG1Entry*)address;
            Data._unknown0x4C = _unknown0x4C;
            Data._unknown0x50 = _unknown0x50;
            *hdr = Data;
        }
    }
}