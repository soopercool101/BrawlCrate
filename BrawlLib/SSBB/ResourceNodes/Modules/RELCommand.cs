using BrawlLib.Internal;
using BrawlLib.SSBB.Types;
using System;
using System.ComponentModel;

namespace BrawlLib.SSBB.ResourceNodes
{
    [TypeConverter(typeof(ExpandableObjectCustomConverter))]
    public class RelCommand
    {
        private readonly ModuleDataNode _section;

        private ModuleSectionNode[] Sections => (_section.Root as ModuleNode).Sections;

        [Category("Relocation Command")]
        [Browsable(false)]
        public bool IsBranchSet => _command >= RELCommandType.SetBranchDestination &&
                                   _command <= RELCommandType.SetBranchConditionDestination3;

        [Category("Relocation Command")]
        [Browsable(false)]
        public bool IsHalf => _command >= RELCommandType.WriteLowerHalf1 &&
                              _command <= RELCommandType.WriteUpperHalfandBit1;

        [Category("Relocation Command")]
        [Description("The offset relative to the start of the target section.")]
        [TypeConverter(typeof(HexUIntConverter))]
        public uint TargetOffset
        {
            get => _addend;
            set
            {
                if ((_section.Root as ModuleNode).ID == _moduleID)
                {
                    ModuleSectionNode section = Sections[TargetSectionID];
                    int x = section._dataBuffer.Length - 2;
                    value = value.Clamp(0, (uint) (x < 0 ? 0 : x));
                }

                _addend = value;
                _section.SignalPropertyChange();
            }
        }

        [Category("Relocation Command")]
        [Description("Determines how the offset should be written.")]
        public RELCommandType Command
        {
            get => _command;
            set
            {
                _command = value;
                _section.SignalPropertyChange();
            }
        }

        [Category("Relocation Command")]
        [Description("The index of the section to offset into.")]
        public uint TargetSectionID
        {
            get => _targetSectionId;
            set
            {
                if ((_section.Root as ModuleNode).ID == _moduleID)
                {
                    _targetSectionId = value.Clamp(0, (uint) Sections.Length - 1);
                    ModuleSectionNode section = Sections[TargetSectionID];
                    int x = section._dataBuffer.Length - 2;
                    _addend = _addend.Clamp(0, (uint) (x < 0 ? 0 : x));
                }
                else
                {
                    _targetSectionId = value;
                }

                _section.SignalPropertyChange();
            }
        }

        [Category("Relocation Command")]
        [Description("The ID of the target module.")]
        [TypeConverter(typeof(DropDownListRELModuleIDs))]
        public string TargetModuleID
        {
            get => RELNode._idNames.ContainsKey(_moduleID) ? RELNode._idNames[_moduleID] : _moduleID.ToString();
            set
            {
                if (!uint.TryParse(value, out uint id) && RELNode._idNames.ContainsValue(value))
                {
                    id = RELNode._idNames.Keys[RELNode._idNames.IndexOfValue(value)];
                }

                _moduleID = id;
                _section.SignalPropertyChange();
            }
        }

        [Category("Relocation Command")]
        [Description("The targetted function name (if known).")]
        public string TargetFunction =>
            (TargetSectionID == 1 || TargetModuleID.Equals("main.dol")) &&
            ModuleMapLoader.MapFiles.ContainsKey(TargetModuleID) &&
            ModuleMapLoader.MapFiles[TargetModuleID].ContainsKey(TargetOffset)
                ? ModuleMapLoader.MapFiles[TargetModuleID][TargetOffset]
                : "";

        public RELCommandType _command;
        public int _modifiedSectionId;
        public uint _targetSectionId;
        public uint _moduleID;

        //Addend is an offset relative to the start of the section
        public uint _addend;
        public bool _initialized = false;

        public RelCommand(uint fileId, ModuleDataNode section, RELLink link)
        {
            _moduleID = fileId;
            _section = section;
            _modifiedSectionId = section.Index;
            _targetSectionId = link._section;
            _command = (RELCommandType) (int) link._type;
            _addend = link._value;
        }

        public RelocationTarget GetTargetRelocation()
        {
            return new RelocationTarget(_moduleID, (int) _targetSectionId, (int) (_addend.RoundDown(4) / 4));
        }

        public void SetTargetRelocation(RelocationTarget e)
        {
            if (e == null)
            {
                return;
            }

            _addend = (uint) e._index * 4;
        }

        public uint Apply(uint newValue, uint baseOffset)
        {
            uint addend = _addend + baseOffset;
            switch (_command)
            {
                case RELCommandType.Nop: // 0x0
                    break;
                case RELCommandType.WriteWord: //0x1
                    newValue = addend;
                    break;

                case RELCommandType.SetBranchOffset: //0x2
                    newValue &= 0xFC000003;
                    newValue |= addend & 0x03FFFFFC;
                    break;

                case RELCommandType.WriteLowerHalf1: //0x3
                case RELCommandType.WriteLowerHalf2: //0x4
                    newValue &= 0xFFFF0000;
                    newValue |= (ushort) (addend & 0xFFFF);
                    break;

                case RELCommandType.WriteUpperHalf: //0x5
                    newValue &= 0xFFFF0000;
                    newValue |= (ushort) (addend >> 16);
                    break;

                case RELCommandType.WriteUpperHalfandBit1: //0x6
                    newValue &= 0xFFFF0000;
                    newValue |= (ushort) ((addend >> 16) | (addend & 0x1));
                    break;

                case RELCommandType.SetBranchConditionOffset1: //0x7
                case RELCommandType.SetBranchConditionOffset2: //0x8
                case RELCommandType.SetBranchConditionOffset3: //0x9
                    newValue &= 0xFFFF0003;
                    newValue |= addend & 0xFFFC;
                    break;

                case RELCommandType.SetBranchDestination: //0xA
                    //Console.WriteLine("SetBranchDestination");
                    break;

                case RELCommandType.SetBranchConditionDestination1: //0xB
                case RELCommandType.SetBranchConditionDestination2: //0xC
                case RELCommandType.SetBranchConditionDestination3: //0xD
                    //Console.WriteLine("SetBranchConditionDestination" + ((int)(_command - RELCommandType.SetBranchConditionDestination1)).ToString());
                    break;

                default:
                    throw new Exception("Unknown Relocation Command.");
            }

            return newValue;
        }
    }

    public enum RELCommandType : byte
    {
        Nop = 0x0,
        WriteWord = 0x1,
        SetBranchOffset = 0x2,
        WriteLowerHalf1 = 0x3,
        WriteLowerHalf2 = 0x4,
        WriteUpperHalf = 0x5,
        WriteUpperHalfandBit1 = 0x6,
        SetBranchConditionOffset1 = 0x7,
        SetBranchConditionOffset2 = 0x8,
        SetBranchConditionOffset3 = 0x9,
        SetBranchDestination = 0xA,
        SetBranchConditionDestination1 = 0xB,
        SetBranchConditionDestination2 = 0xC,
        SetBranchConditionDestination3 = 0xD
    }
}