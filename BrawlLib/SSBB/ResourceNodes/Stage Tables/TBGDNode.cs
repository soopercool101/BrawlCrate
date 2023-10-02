using BrawlLib.Internal;
using BrawlLib.SSBB.Types.Stage_Tables;
using System;
using System.ComponentModel;
using System.Linq;

namespace BrawlLib.SSBB.ResourceNodes
{
    public class TBGDNode : TBNode
    {
        public override ResourceType ResourceFileType => ResourceType.TBGD;
        public override Type SubEntryType => typeof(TBGDEntryNode);

        protected override string GetName()
        {
            return base.GetName("Smashville Cameo Attributes");
        }

        internal static ResourceNode TryParse(DataSource source, ResourceNode parent)
        {
            return source.Tag == "TBGD" ? new TBGDNode() : null;
        }
    }

    public unsafe class TBGDEntryNode : ResourceNode
    {
        public TBGDEntry Data;

        [Category("Cameo Attributes")]
        [TypeConverter(typeof(VillagerDropdown))]
        public byte Villager
        {
            get => Data._villagerId;
            set
            {
                Data._villagerId = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public byte Unknown0x01
        {
            get => Data._unk0x01;
            set
            {
                Data._unk0x01 = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public byte Unknown0x02
        {
            get => Data._unk0x02;
            set
            {
                Data._unk0x02 = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public byte Unknown0x03
        {
            get => Data._unk0x03;
            set
            {
                Data._unk0x03 = value;
                SignalPropertyChange();
            }
        }

        [Category("Cameo Attributes")]
        public float AnimationPlaybackSpeed
        {
            get => Data._animationPlaybackSpeed;
            set
            {
                Data._animationPlaybackSpeed = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Use StgVillageCommonWait")]
        [Category("Cameo Attributes")]
        public bool PlayAnim1
        {
            get => Data._playAnim1;
            set
            {
                Data._playAnim1 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Use StgVillageCommonHear")]
        [Category("Cameo Attributes")]
        public bool PlayAnim2
        {
            get => Data._playAnim2;
            set
            {
                Data._playAnim2 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Use StgVillageCommonClaps")]
        [Category("Cameo Attributes")]
        public bool PlayAnim3
        {
            get => Data._playAnim3;
            set
            {
                Data._playAnim3 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Use StgVillageCommonKomari")]
        [Category("Cameo Attributes")]
        public bool PlayAnim4
        {
            get => Data._playAnim4;
            set
            {
                Data._playAnim4 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Use StgVillageCommonMusu")]
        [Category("Cameo Attributes")]
        public bool PlayAnim5
        {
            get => Data._playAnim5;
            set
            {
                Data._playAnim5 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Use StgVillageCommonNiko")]
        [Category("Cameo Attributes")]
        public bool PlayAnim6
        {
            get => Data._playAnim6;
            set
            {
                Data._playAnim6 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Use StgVillageCommonWalk")]
        [Category("Cameo Attributes")]
        public bool PlayAnim7
        {
            get => Data._playAnim7;
            set
            {
                Data._playAnim7 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Use StgVillageCommonRun")]
        [Category("Cameo Attributes")]
        public bool PlayAnim8
        {
            get => Data._playAnim8;
            set
            {
                Data._playAnim8 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Use StgVillageCommonSleep")]
        [Category("Cameo Attributes")]
        public bool PlayAnim9
        {
            get => Data._playAnim9;
            set
            {
                Data._playAnim9 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Use StgVillageCommonSit")]
        [Category("Cameo Attributes")]
        public bool PlayAnim10
        {
            get => Data._playAnim10;
            set
            {
                Data._playAnim10 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Use StgVillageCommonPoseWait")]
        [Category("Cameo Attributes")]
        public bool PlayAnim11
        {
            get => Data._playAnim11;
            set
            {
                Data._playAnim11 = value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Use StgVillageCommonMonbanWait")]
        [Category("Cameo Attributes")]
        public bool PlayAnim12
        {
            get => Data._playAnim12;
            set
            {
                Data._playAnim12 = value;
                SignalPropertyChange();
            }
        }

        public override int OnCalculateSize(bool force)
        {
            return TBGDEntry.Size;
        }

        public override bool OnInitialize()
        {
            Data = *(TBGDEntry*)WorkingUncompressed.Address;

            if (_name == null)
            {
                if (Enum.GetValues(typeof(VillagerID)).Cast<VillagerID>().Any(o => (byte) o == Villager))
                {
                    var villagerName = ((VillagerNameAttribute) ((VillagerID)Villager).GetType().GetField(((VillagerID)Villager).ToString()).GetCustomAttributes(true)
                        .FirstOrDefault(a => a is VillagerNameAttribute))?.Name ?? ((VillagerID)Villager).ToString();
                    _name = $"{villagerName} [{Index}]";
                }
                else
                {
                    _name = $"Villager0x{Villager:X2} [{Index}]";
                }
            }

            return false;
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            *(TBGDEntry*)address = Data;
        }
    }
}