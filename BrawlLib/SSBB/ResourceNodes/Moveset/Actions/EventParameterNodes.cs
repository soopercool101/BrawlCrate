using BrawlLib.Internal;
using BrawlLib.SSBB.Types;
using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class MoveDefEventParameterNode : MoveDefEntryNode
    {
        internal FDefEventArgument* Header => (FDefEventArgument*) WorkingUncompressed.Address;
        public override ResourceType ResourceFileType => ResourceType.Parameter;

        public int _value;

        [Browsable(false)] public virtual ArgVarType _type => ArgVarType.Value;

        [Browsable(false)] public virtual float RealValue => _value;

        public bool Compare(MoveDefEventParameterNode param, int compare)
        {
            switch (compare)
            {
                case 0:  return RealValue < param.RealValue;
                case 1:  return RealValue <= param.RealValue;
                case 2:  return RealValue == param.RealValue;
                case 3:  return RealValue != param.RealValue;
                case 4:  return RealValue >= param.RealValue;
                case 5:  return RealValue > param.RealValue;
                default: return false;
            }
        }

        public override bool OnInitialize()
        {
            _value = Header->_data;
            return base.OnInitialize();
        }

        [Browsable(false)]
        public string Description =>
            (Parent as MoveDefEventNode).EventInfo != null &&
            Index < (Parent as MoveDefEventNode).EventInfo.pDescs.Length
                ? (Parent as MoveDefEventNode).EventInfo.pDescs[Index]
                : "No Description Available.";

        public MoveDefEventParameterNode()
        {
        }

        public override int OnCalculateSize(bool force)
        {
            _lookupCount = 0;
            return 8;
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            _entryOffset = address;
            FDefEventArgument* header = (FDefEventArgument*) address;
            header->_type = (int) _type;
            header->_data = _value;
        }
    }

    #region Value Nodes

    public class MoveDefEventValueNode : MoveDefEventParameterNode
    {
        [Browsable(false)] public override ArgVarType _type => ArgVarType.Value;

        [Category("MoveDef Event Value")]
        public int Value
        {
            get => _value;
            set
            {
                if (_value < int.MaxValue)
                {
                    _value = value;
                    SignalPropertyChange();
                }
            }
        }

        public MoveDefEventValueNode(string name)
        {
            _name = name != null ? name : "Value";
        }

        public override bool OnInitialize()
        {
            if (_name == null)
            {
                _name = "Value";
            }

            base.OnInitialize();
            return false;
        }
    }

    public class MoveDefEventValueEnumNode : MoveDefEventParameterNode
    {
        public string[] Enums = new string[0];

        [Browsable(false)] public override ArgVarType _type => ArgVarType.Value;

        [Category("MoveDef Event Value")]
        [TypeConverter(typeof(DropDownListEnumMDef))]
        public string Value
        {
            get
            {
                if (_value >= 0 && _value < Enums.Length)
                {
                    return Enums[_value];
                }

                return _value.ToString();
            }
            set
            {
                if (!int.TryParse(value, out _value))
                {
                    _value = Array.IndexOf(Enums, value);
                }

                if (_value == -1)
                {
                    _value = 0;
                }

                SignalPropertyChange();
            }
        }

        public MoveDefEventValueEnumNode(string name)
        {
            _name = name != null ? name : "Value";
        }

        public override bool OnInitialize()
        {
            if (_name == null)
            {
                _name = "Value";
            }

            base.OnInitialize();
            return false;
        }
    }

    public class MoveDefEventValue2HalfNode : MoveDefEventParameterNode
    {
        [Browsable(false)] public override ArgVarType _type => ArgVarType.Value;

        [Category("MoveDef Event Value")]
        public short Value1
        {
            get => (short) ((_value >> 16) & 0xFFFF);
            set
            {
                _value = (_value & 0xFFFF) | ((value & 0xFFFF) << 16);
                SignalPropertyChange();
            }
        }

        [Category("MoveDef Event Value")]
        public short Value2
        {
            get => (short) (_value & 0xFFFF);
            set
            {
                _value = (int) ((uint) _value & 0xFFFF0000) | (value & 0xFFFF);
                SignalPropertyChange();
            }
        }

        public MoveDefEventValue2HalfNode(string name)
        {
            _name = name != null ? name : "Value";
        }

        public override bool OnInitialize()
        {
            if (_name == null)
            {
                _name = "Value";
            }

            base.OnInitialize();
            return false;
        }
    }

    public class MoveDefEventValue2HalfGFXNode : MoveDefEventParameterNode
    {
        [Browsable(false)] public override ArgVarType _type => ArgVarType.Value;

        [Category("MoveDef Event Value")]
        [TypeConverter(typeof(DropDownListGFXFilesMDef))]
        public string GFXFile
        {
            get
            {
                int index = (_value >> 16) & 0xFFFF;
                if (Root.iGFXFiles != null && Root.iGFXFiles.Length > 0 && index < Root.iGFXFiles.Length)
                {
                    return Root.iGFXFiles[index];
                }

                return index.ToString();
            }
            set
            {
                int index = 0;
                if (!int.TryParse(value, out index))
                {
                    if (Root.iGFXFiles != null && Root.iGFXFiles.Length > 0 && Root.iGFXFiles.Contains(value))
                    {
                        index = Array.IndexOf(Root.iGFXFiles, value);
                    }
                }

                _value = (_value & 0xFFFF) | ((index & 0xFFFF) << 16);
                SignalPropertyChange();
            }
        }

        [Category("MoveDef Event Value")]
        public short EFLSEntryIndex
        {
            get => (short) (_value & 0xFFFF);
            set
            {
                _value = (int) ((uint) _value & 0xFFFF0000) | (value & 0xFFFF);
                SignalPropertyChange();
            }
        }

        public MoveDefEventValue2HalfGFXNode(string name)
        {
            _name = name != null ? name : "Value";
        }

        public override bool OnInitialize()
        {
            if (_name == null)
            {
                _name = "Value";
            }

            base.OnInitialize();
            return false;
        }
    }

    public class MoveDefEventValueHalf2ByteNode : MoveDefEventParameterNode
    {
        [Browsable(false)] public override ArgVarType _type => ArgVarType.Value;

        [Category("MoveDef Event Value")]
        public short Value1
        {
            get => (short) ((_value >> 16) & 0xFFFF);
            set
            {
                _value = (_value & 0xFFFF) | ((value & 0xFFFF) << 16);
                SignalPropertyChange();
            }
        }

        [Category("MoveDef Event Value")]
        public byte Value2
        {
            get => (byte) ((_value >> 8) & 0xFF);
            set
            {
                _value = (int) ((uint) _value & 0xFFFF00FF) | ((value & 0xFF) << 8);
                SignalPropertyChange();
            }
        }

        [Category("MoveDef Event Value")]
        public byte Value3
        {
            get => (byte) (_value & 0xFF);
            set
            {
                _value = (int) ((uint) _value & 0xFFFFFF00) | (value & 0xFF);
                SignalPropertyChange();
            }
        }

        public MoveDefEventValueHalf2ByteNode(string name)
        {
            _name = name != null ? name : "Value";
        }

        public override bool OnInitialize()
        {
            if (_name == null)
            {
                _name = "Value";
            }

            base.OnInitialize();
            return false;
        }
    }

    public class MoveDefEventValue2ByteHalfNode : MoveDefEventParameterNode
    {
        [Browsable(false)] public override ArgVarType _type => ArgVarType.Value;

        [Category("MoveDef Event Value")]
        public byte Value1
        {
            get => (byte) ((_value >> 24) & 0xFF);
            set
            {
                _value = (int) ((uint) _value & 0x00FFFFFF) | ((value & 0xFF) << 24);
                SignalPropertyChange();
            }
        }

        [Category("MoveDef Event Value")]
        public byte Value2
        {
            get => (byte) ((_value >> 16) & 0xFF);
            set
            {
                _value = (int) ((uint) _value & 0xFF00FFFF) | ((value & 0xFF) << 16);
                SignalPropertyChange();
            }
        }

        [Category("MoveDef Event Value")]
        public short Value3
        {
            get => (short) (_value & 0xFFFF);
            set
            {
                _value = (int) ((uint) _value & 0xFFFF0000) | (value & 0xFFFF);
                SignalPropertyChange();
            }
        }

        public MoveDefEventValue2ByteHalfNode(string name)
        {
            _name = name != null ? name : "Value";
        }

        public override bool OnInitialize()
        {
            if (_name == null)
            {
                _name = "Value";
            }

            base.OnInitialize();
            return false;
        }
    }

    public class MoveDefEventValue4ByteNode : MoveDefEventParameterNode
    {
        [Browsable(false)] public override ArgVarType _type => ArgVarType.Value;

        [Category("MoveDef Event Value")]
        public byte Value1
        {
            get => (byte) ((_value >> 24) & 0xFF);
            set
            {
                _value = (int) ((uint) _value & 0x00FFFFFF) | ((value & 0xFF) << 24);
                SignalPropertyChange();
            }
        }

        [Category("MoveDef Event Value")]
        public byte Value2
        {
            get => (byte) ((_value >> 16) & 0xFF);
            set
            {
                _value = (int) ((uint) _value & 0xFF00FFFF) | ((value & 0xFF) << 16);
                SignalPropertyChange();
            }
        }

        [Category("MoveDef Event Value")]
        public byte Value3
        {
            get => (byte) ((_value >> 8) & 0xFF);
            set
            {
                _value = (int) ((uint) _value & 0xFFFF00FF) | ((value & 0xFF) << 8);
                SignalPropertyChange();
            }
        }

        [Category("MoveDef Event Value")]
        public byte Value4
        {
            get => (byte) (_value & 0xFF);
            set
            {
                _value = (int) ((uint) _value & 0xFFFFFF00) | (value & 0xFF);
                SignalPropertyChange();
            }
        }

        public MoveDefEventValue4ByteNode(string name)
        {
            _name = name != null ? name : "Value";
        }

        public override bool OnInitialize()
        {
            if (_name == null)
            {
                _name = "Value";
            }

            base.OnInitialize();
            return false;
        }
    }

    #endregion

    public class MoveDefEventUnkNode : MoveDefEventParameterNode
    {
        [Browsable(false)] public override ArgVarType _type => ArgVarType.Unknown;

        [Category("MoveDef Event File")]
        public int Value
        {
            get => _value;
            set
            {
                if (_value < int.MaxValue)
                {
                    _value = value;
                    SignalPropertyChange();
                }
            }
        }

        public MoveDefEventUnkNode(string name)
        {
            _name = name != null ? name : "Unknown";
        }

        public override bool OnInitialize()
        {
            if (_name == null)
            {
                _name = "Unknown";
            }

            //MessageBox.Show(TreePath);
            base.OnInitialize();
            return false;
        }
    }

    public unsafe class MoveDefEventOffsetNode : MoveDefEventParameterNode
    {
        [Browsable(false)] public override ArgVarType _type => ArgVarType.Offset;

        public int list, type, index;

        [Category("MoveDef Event Offset")]
        public int RawOffset
        {
            get => _value;
            set
            {
                //if (value < 0)
                //{
                //    _value = value;
                //    list = 4;
                //    type = -1;
                //    index = -1;
                //    SignalPropertyChange();
                //    return;
                //}
                ResourceNode r = Root.FindNode(value);
                if (r is MoveDefActionNode)
                {
                    _value = value;
                    SignalPropertyChange();
                }
                else
                {
                    MessageBox.Show("An action could not be located.");
                }
            }
        }

        [Category("MoveDef Event Offset")]
        [Browsable(true)]
        [TypeConverter(typeof(DropDownListExtNodesMDef))]
        public string ExternalNode
        {
            get => _extNode != null ? _extNode.Name : null;
            set
            {
                if (_extNode != null)
                {
                    if (_extNode.Name != value)
                    {
                        _extNode._refs.Remove(this);
                    }
                }

                foreach (MoveDefExternalNode e in Root._externalRefs)
                {
                    if (e.Name == value)
                    {
                        _extNode = e;
                        e._refs.Add(this);
                        Name = e.Name;
                        action = null;
                        list = 3;
                        index = _extNode.Index;
                    }
                }

                if (_extNode == null)
                {
                    Name = "Offset";
                }
            }
        }

        public MoveDefActionNode GetAction()
        {
            ResourceNode r = Root.FindNode(RawOffset);
            return r as MoveDefActionNode;
        }

        public MoveDefActionNode action;

        public MoveDefEventOffsetNode(string name)
        {
            _name = name != null ? name : "Offset";
        }

        public override bool OnInitialize()
        {
            base.OnInitialize();

            if (RawOffset > 0)
            {
                Root.GetLocation(RawOffset, out list, out type, out index);
                if (!External)
                {
                    action = Root.GetAction(list, type, index);
                    if (action == null)
                    {
                        action = GetAction();
                    }
                }
            }
            else if (RawOffset < 0 && External)
            {
                action = null;
                index = _extNode.Index;
                list = 3;
                type = -1;
            }
            else
            {
                action = null;
                index = -1;
                list = 4;
                type = -1;
            }

            if (_name == null)
            {
                _name = "Offset";
            }

            return false;
        }

        public override int OnCalculateSize(bool force)
        {
            if (action != null)
            {
                _lookupCount = 1;
            }

            return 8;
        }

        public override void PostProcess()
        {
            FDefEventArgument* arg = (FDefEventArgument*) _entryOffset;
            arg->_type = (int) _type;
            if (action != null)
            {
                if (action._entryOffset == 0)
                {
                    Console.WriteLine("Action offset = 0");
                }

                arg->_data = (int) action._entryOffset - (int) action._rebuildBase;
                if (arg->_data > 0)
                {
                    MoveDefNode._lookupOffsets.Add((int) arg->_data.Address - (int) _rebuildBase);
                }
            }
            else
            {
                arg->_data = -1;
                if (External)
                {
                    if (_extNode is MoveDefReferenceEntryNode)
                    {
                        _entryOffset += 4;
                    }
                }
                else
                {
                    foreach (MoveDefReferenceEntryNode e in Root.references.Children)
                    {
                        if (e.Name == Name)
                        {
                            _extNode = e;
                            //if (!e._refs.Contains(this))
                            e._refs.Add(this);
                            _entryOffset += 4;
                            break;
                        }
                    }
                }
            }
        }
    }

    public class MoveDefEventScalarNode : MoveDefEventParameterNode
    {
        [Browsable(false)] public override ArgVarType _type => ArgVarType.Scalar;

        [Category("MoveDef Event Scalar Value")]
        public float Value
        {
            get => (float) _value / 60000f;
            set
            {
                if (value * 60000f < int.MaxValue)
                {
                    _value = Convert.ToInt32(value * 60000f);
                    SignalPropertyChange();
                }
            }
        }

        public MoveDefEventScalarNode(string name)
        {
            _name = name != null ? name : "Scalar";
        }

        public override bool OnInitialize()
        {
            if (_name == null)
            {
                _name = "Scalar";
            }

            base.OnInitialize();
            return false;
        }
    }

    public class MoveDefEventBoolNode : MoveDefEventParameterNode
    {
        [Browsable(false)] public override ArgVarType _type => ArgVarType.Boolean;

        [Category("MoveDef Event Boolean")]
        public bool Value
        {
            get => _value == 1 ? true : false;
            set
            {
                _value = value ? 1 : 0;
                SignalPropertyChange();
            }
        }

        public MoveDefEventBoolNode(string name)
        {
            _name = name != null ? name : "Boolean";
        }

        public override bool OnInitialize()
        {
            if (_name == null)
            {
                _name = "Boolean";
            }

            base.OnInitialize();
            return false;
        }
    }

    public class MoveDefEventVariableNode : MoveDefEventParameterNode
    {
        internal string val;

        [Browsable(false)] public override ArgVarType _type => ArgVarType.Variable;

        internal int number;
        internal VarMemType mem;
        internal VariableType type;

        [Category("MoveDef Event Variable")]
        public VarMemType MemType
        {
            get => mem;
            set
            {
                mem = value;
                GetValue();
                SignalPropertyChange();
            }
        }

        [Category("MoveDef Event Variable")]
        public VariableType VarType
        {
            get => type;
            set
            {
                type = value;
                GetValue();
                SignalPropertyChange();
            }
        }

        [Category("MoveDef Event Variable")]
        public int Number
        {
            get => number;
            set
            {
                number = value;
                GetValue();
                SignalPropertyChange();
            }
        }

        public MoveDefEventVariableNode(string name)
        {
            _name = name != null ? name : "Variable";
        }

        public override float RealValue => base.RealValue;

        public override bool OnInitialize()
        {
            if (_name == null)
            {
                _name = "Variable";
            }

            base.OnInitialize();
            val = ResolveVariable((long) _value);
            return false;
        }

        public void GetValue()
        {
            val = ResolveVariable(_value = (int) mem * 0x10000000 + (int) type * 0x1000000 + number.Clamp(0, 0xFFFFFF));
        }

        public enum VarMemType
        {
            IC,
            LA,
            RA
        }

        public enum VariableType
        {
            Basic,
            Float,
            Bit
        }

        public override string ToString()
        {
            return val == null ? val = ResolveVariable(_value) : val;
        }

        public string ResolveVariable(long value)
        {
            string variableName = "";
            long variableMemType = (value & 0xF0000000) / 0x10000000;
            long variableType = (value & 0xF000000) / 0x1000000;
            long variableNumber = value & 0xFFFFFF;
            if (variableMemType == 0)
            {
                variableName = "IC-";
                mem = VarMemType.IC;
            }

            if (variableMemType == 1)
            {
                variableName = "LA-";
                mem = VarMemType.LA;
            }

            if (variableMemType == 2)
            {
                variableName = "RA-";
                mem = VarMemType.RA;
            }

            if (variableType == 0)
            {
                variableName += "Basic";
                type = VariableType.Basic;
            }

            if (variableType == 1)
            {
                variableName += "Float";
                type = VariableType.Float;
            }

            if (variableType == 2)
            {
                variableName += "Bit";
                type = VariableType.Bit;
            }

            variableName += "[" + (number = (int) variableNumber) + "]";

            return variableName;
        }
    }

    public class MoveDefEventRequirementNode : MoveDefEventParameterNode
    {
        internal string val;

        [Browsable(false)] public override ArgVarType _type => ArgVarType.Requirement;

        internal bool not;
        internal string arg;

        [Category("MoveDef Event Requirement")]
        [TypeConverter(typeof(DropDownListRequirementsMDef))]
        public string Requirement
        {
            get => arg;
            set
            {
                if (Array.IndexOf(Root.iRequirements, value) == -1)
                {
                    return;
                }

                arg = value;
                GetValue();
                SignalPropertyChange();
            }
        }

        [Category("MoveDef Event Requirement")]
        public bool Not
        {
            get => not;
            set
            {
                not = value;
                GetValue();
                SignalPropertyChange();
            }
        }

        public MoveDefEventRequirementNode(string name)
        {
            _name = name != null ? name : "Requirement";
        }

        public override bool OnInitialize()
        {
            if (_name == null)
            {
                _name = "Requirement";
            }

            base.OnInitialize();
            val = GetRequirement((long) _value);
            return false;
        }

        public override float RealValue
        {
            get
            {
                float value = Array.IndexOf(Root.iRequirements, arg);
                return value * (not ? -1 : 1);
            }
        }

        public void GetValue()
        {
            long value = Array.IndexOf(Root.iRequirements, arg);
            if (not)
            {
                value |= 0x80000000;
            }

            val = GetRequirement(_value = (int) value);
        }

        public override string ToString()
        {
            return val == null ? val = GetRequirement(_value) : val;
        }

        public string GetRequirement(long value)
        {
            not = (value & 0x80000000) == 0x80000000;
            long requirement = value & 0xFF;

            if (requirement > 0x7F)
            {
                return requirement.ToString("X");
            }

            if (not)
            {
                return "Not " + (arg = Root.iRequirements[requirement]);
            }

            return arg = Root.iRequirements[requirement];
        }
    }

    #region htBoxes

    public class HitboxFlagsNode : MoveDefEventParameterNode
    {
        internal HitboxFlags val;

        public string HexValue
        {
            get => _value.ToString("X");
            set
            {
                val.data = _value = int.Parse(value, System.Globalization.NumberStyles.HexNumber);
                GetFlags();
            }
        }

        public int effect, unk1, sound, unk2, ground, air, unk3, type, clang, unk4, direct, unk5;

        [Category("MoveDef Hitbox Flags")]
        public HitboxEffect Effect
        {
            get => (HitboxEffect) val.Effect;
            set
            {
                effect = (int) value;
                CalcFlags();
            }
        }

        [Category("MoveDef Hitbox Flags")]
        public bool Unk1
        {
            get => val.Unk1 != 0;
            set
            {
                unk1 = value ? 1 : 0;
                CalcFlags();
            }
        }

        [Category("MoveDef Hitbox Flags")]
        public HitboxSFX Sound
        {
            get => (HitboxSFX) val.Sound;
            set
            {
                sound = (int) value;
                CalcFlags();
            }
        }

        [Category("MoveDef Hitbox Flags")]
        public int Unk2
        {
            get => val.Unk2;
            set
            {
                unk2 = (int) value;
                CalcFlags();
            }
        }

        [Category("MoveDef Hitbox Flags")]
        public bool Grounded
        {
            get => val.Grounded != 0;
            set
            {
                ground = value ? 1 : 0;
                CalcFlags();
            }
        }

        [Category("MoveDef Hitbox Flags")]
        public bool Aerial
        {
            get => val.Aerial != 0;
            set
            {
                air = value ? 1 : 0;
                CalcFlags();
            }
        }

        [Category("MoveDef Hitbox Flags")]
        public int Unk3
        {
            get => val.Unk3;
            set
            {
                unk3 = (int) value;
                CalcFlags();
            }
        }

        [Category("MoveDef Hitbox Flags")]
        public HitboxType Type
        {
            get => (HitboxType) val.Type;
            set
            {
                type = (int) value;
                CalcFlags();
            }
        }

        [Category("MoveDef Hitbox Flags")]
        public bool Clang
        {
            get => val.Clang != 0;
            set
            {
                clang = value ? 1 : 0;
                CalcFlags();
            }
        }

        [Category("MoveDef Hitbox Flags")]
        public bool Unk4
        {
            get => val.Unk4 != 0;
            set
            {
                unk4 = value ? 1 : 0;
                CalcFlags();
            }
        }

        [Category("MoveDef Hitbox Flags")]
        public bool Direct
        {
            get => val.Direct != 0;
            set
            {
                direct = value ? 1 : 0;
                CalcFlags();
            }
        }

        [Category("MoveDef Hitbox Flags")]
        public int Unk5
        {
            get => val.Unk5;
            set
            {
                unk5 = (int) value;
                CalcFlags();
            }
        }

        public HitboxFlagsNode(string name)
        {
            _name = name != null ? name : "Flags";
        }

        public override bool OnInitialize()
        {
            if (_name == null)
            {
                _name = "Flags";
            }

            base.OnInitialize();
            val.data = _value;
            GetFlags();
            return false;
        }

        private void CalcFlags()
        {
            val.data = (effect << 0) |
                       (unk1 << 5) |
                       (sound << 6) |
                       (unk2 << 14) |
                       (ground << 16) |
                       (air << 17) |
                       (unk3 << 18) |
                       (type << 22) |
                       (clang << 27) |
                       (unk4 << 28) |
                       (direct << 29) |
                       (unk5 << 30);

            _value = val.data;

            GetFlags();

            SignalPropertyChange();
        }

        public void GetFlags()
        {
            effect = val.Effect;
            unk1 = val.Unk1;
            sound = val.Sound;
            unk2 = val.Unk2;
            ground = val.Grounded;
            air = val.Aerial;
            unk3 = val.Unk3;
            type = val.Type;
            clang = val.Clang;
            unk4 = val.Unk4;
            direct = val.Direct;
            unk5 = val.Unk5;
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct HitboxFlags
    {
        //0000 0000 0000 0000 0000 0000 0001 1111   Effect
        //0000 0000 0000 0000 0000 0000 0010 0000   Unknown1
        //0000 0000 0000 0000 0011 1111 1100 0000   Sound
        //0000 0000 0000 0000 1100 0000 0000 0000   Unknown2
        //0000 0000 0000 0001 0000 0000 0000 0000   Grounded
        //0000 0000 0000 0010 0000 0000 0000 0000   Aerial
        //0000 0000 0011 1100 0000 0000 0000 0000   Unknown3
        //0000 0111 1100 0000 0000 0000 0000 0000   Type
        //0000 1000 0000 0000 0000 0000 0000 0000   Clang
        //0001 0000 0000 0000 0000 0000 0000 0000   Unknown4
        //0010 0000 0000 0000 0000 0000 0000 0000   Direct
        //1100 0000 0000 0000 0000 0000 0000 0000   Unknown5

        public int Effect => data & 0x1F;
        public int Unk1 => (data >> 5) & 1;
        public int Sound => (data >> 6) & 0xFF;
        public int Unk2 => (data >> 14) & 3;
        public int Grounded => (data >> 16) & 1;
        public int Aerial => (data >> 17) & 1;
        public int Unk3 => (data >> 18) & 0xF;
        public int Type => (data >> 22) & 0x1F;
        public int Clang => (data >> 27) & 1;
        public int Unk4 => (data >> 28) & 1;
        public int Direct => (data >> 29) & 1;
        public int Unk5 => (data >> 30) & 3;

        public int data;
    }

    public class SpecialHitboxFlagsNode : MoveDefEventParameterNode
    {
        internal SpecialHitboxFlags val;

        public string HexValue
        {
            get => _value.ToString("X");
            set
            {
                val.data = _value = int.Parse(value, System.Globalization.NumberStyles.HexNumber);
                GetFlags();
            }
        }

        public int angleFlipping,
            unk1,
            stretches,
            unk2,
            shield,
            absorb,
            reflect,
            unk3,
            invinc,
            grip,
            unk4,
            freeze,
            sleep,
            flinch;

        public Bin16 hitBits;

        [Category("Special Hitbox Flags")]
        public int AngleFlipping
        {
            get => val.AngleFlipping;
            set
            {
                angleFlipping = value;
                CalcFlags();
            }
        }

        [Category("Special Hitbox Flags")]
        public bool Unk1
        {
            get => val.Unk1 != 0;
            set
            {
                unk1 = value ? 1 : 0;
                CalcFlags();
            }
        }

        [Category("Special Hitbox Flags")]
        public bool Stretches
        {
            get => val.Stretches != 0;
            set
            {
                stretches = value ? 1 : 0;
                CalcFlags();
            }
        }

        [Category("Special Hitbox Flags")]
        public bool Unk2
        {
            get => val.Unk2 != 0;
            set
            {
                unk2 = value ? 1 : 0;
                CalcFlags();
            }
        }

        [Category("Hit Flags")]
        public bool CanHitMultiplayerCharacters
        {
            get => val.GetHitBit(0) != 0;
            set
            {
                hitBits[0] = value;
                CalcFlags();
            }
        }

        [Category("Hit Flags")]
        public bool CanHitSSEenemies
        {
            get => val.GetHitBit(1) != 0;
            set
            {
                hitBits[1] = value;
                CalcFlags();
            }
        }

        [Category("Hit Flags")]
        public bool CanHitUnk1
        {
            get => val.GetHitBit(2) != 0;
            set
            {
                hitBits[2] = value;
                CalcFlags();
            }
        }

        [Category("Hit Flags")]
        public bool CanHitUnk2
        {
            get => val.GetHitBit(3) != 0;
            set
            {
                hitBits[3] = value;
                CalcFlags();
            }
        }

        [Category("Hit Flags")]
        public bool CanHitUnk3
        {
            get => val.GetHitBit(4) != 0;
            set
            {
                hitBits[4] = value;
                CalcFlags();
            }
        }

        [Category("Hit Flags")]
        public bool CanHitUnk4
        {
            get => val.GetHitBit(5) != 0;
            set
            {
                hitBits[5] = value;
                CalcFlags();
            }
        }

        [Category("Hit Flags")]
        public bool CanHitUnk5
        {
            get => val.GetHitBit(6) != 0;
            set
            {
                hitBits[6] = value;
                CalcFlags();
            }
        }

        [Category("Hit Flags")]
        public bool CanHitDamageableCeilings
        {
            get => val.GetHitBit(7) != 0;
            set
            {
                hitBits[7] = value;
                CalcFlags();
            }
        }

        [Category("Hit Flags")]
        public bool CanHitDamageableWalls
        {
            get => val.GetHitBit(8) != 0;
            set
            {
                hitBits[8] = value;
                CalcFlags();
            }
        }

        [Category("Hit Flags")]
        public bool CanHitDamageableFloors
        {
            get => val.GetHitBit(9) != 0;
            set
            {
                hitBits[9] = value;
                CalcFlags();
            }
        }

        [Category("Hit Flags")]
        public bool CanHitUnk6
        {
            get => val.GetHitBit(10) != 0;
            set
            {
                hitBits[10] = value;
                CalcFlags();
            }
        }

        [Category("Hit Flags")]
        public bool CanHitUnk7
        {
            get => val.GetHitBit(11) != 0;
            set
            {
                hitBits[11] = value;
                CalcFlags();
            }
        }

        [Category("Hit Flags")]
        public bool CanHitUnk8
        {
            get => val.GetHitBit(12) != 0;
            set
            {
                hitBits[12] = value;
                CalcFlags();
            }
        }

        [Category("Hit Flags")]
        public bool Enabled
        {
            get => val.GetHitBit(13) != 0;
            set
            {
                hitBits[13] = value;
                CalcFlags();
            }
        }

        [Category("Special Hitbox Flags")]
        public int Unk3
        {
            get => val.Unk3;
            set
            {
                unk3 = (int) value;
                CalcFlags();
            }
        }

        [Category("Special Hitbox Flags")]
        public bool CanBeShielded
        {
            get => val.Shieldable != 0;
            set
            {
                shield = value ? 1 : 0;
                CalcFlags();
            }
        }

        [Category("Special Hitbox Flags")]
        public bool CanBeAbsorbed
        {
            get => val.Absorbable != 0;
            set
            {
                absorb = value ? 1 : 0;
                CalcFlags();
            }
        }

        [Category("Special Hitbox Flags")]
        public bool CanBeReflected
        {
            get => val.Reflectable != 0;
            set
            {
                reflect = value ? 1 : 0;
                CalcFlags();
            }
        }

        [Category("Special Hitbox Flags")]
        public int Unk4
        {
            get => val.Unk4;
            set
            {
                unk4 = (int) value;
                CalcFlags();
            }
        }

        [Category("Special Hitbox Flags")]
        public bool HittingGrippedCharacter
        {
            get => val.Gripped != 0;
            set
            {
                grip = value ? 1 : 0;
                CalcFlags();
            }
        }

        [Category("Special Hitbox Flags")]
        public bool IgnoreInvincibility
        {
            get => val.IgnoreInv != 0;
            set
            {
                invinc = value ? 1 : 0;
                CalcFlags();
            }
        }

        [Category("Special Hitbox Flags")]
        public bool FreezeFrameDisable
        {
            get => val.NoFreeze != 0;
            set
            {
                freeze = value ? 1 : 0;
                CalcFlags();
            }
        }

        [Category("Special Hitbox Flags")]
        public bool PutsToSleep
        {
            get => val.Sleep != 0;
            set
            {
                sleep = value ? 1 : 0;
                CalcFlags();
            }
        }

        [Category("Special Hitbox Flags")]
        public bool Flinchless
        {
            get => val.Flinchless != 0;
            set
            {
                flinch = value ? 1 : 0;
                CalcFlags();
            }
        }

        public SpecialHitboxFlagsNode(string name)
        {
            _name = name != null ? name : "Special Flags";
        }

        public override bool OnInitialize()
        {
            if (_name == null)
            {
                _name = "Special Flags";
            }

            base.OnInitialize();
            val.data = _value;
            GetFlags();
            return false;
        }

        private void CalcFlags()
        {
            val.data = (angleFlipping << 0) |
                       (unk1 << 3) |
                       (stretches << 4) |
                       (unk2 << 5) |
                       ((hitBits[0] ? 1 : 0) << 6) |
                       ((hitBits[1] ? 1 : 0) << 7) |
                       ((hitBits[2] ? 1 : 0) << 8) |
                       ((hitBits[3] ? 1 : 0) << 9) |
                       ((hitBits[4] ? 1 : 0) << 10) |
                       ((hitBits[5] ? 1 : 0) << 11) |
                       ((hitBits[6] ? 1 : 0) << 12) |
                       ((hitBits[7] ? 1 : 0) << 13) |
                       ((hitBits[8] ? 1 : 0) << 14) |
                       ((hitBits[9] ? 1 : 0) << 15) |
                       ((hitBits[10] ? 1 : 0) << 16) |
                       ((hitBits[11] ? 1 : 0) << 17) |
                       ((hitBits[12] ? 1 : 0) << 18) |
                       ((hitBits[13] ? 1 : 0) << 19) |
                       (unk3 << 20) |
                       (shield << 22) |
                       (reflect << 23) |
                       (absorb << 24) |
                       (unk4 << 25) |
                       (grip << 27) |
                       (invinc << 28) |
                       (freeze << 29) |
                       (sleep << 30) |
                       (flinch << 31);

            _value = val.data;

            GetFlags();

            SignalPropertyChange();
        }

        public void GetFlags()
        {
            angleFlipping = val.AngleFlipping;
            unk1 = val.Unk1;
            stretches = val.Stretches;
            unk2 = val.Unk2;
            hitBits[0] = val.GetHitBit(0) != 0;
            hitBits[1] = val.GetHitBit(1) != 0;
            hitBits[2] = val.GetHitBit(2) != 0;
            hitBits[3] = val.GetHitBit(3) != 0;
            hitBits[4] = val.GetHitBit(4) != 0;
            hitBits[5] = val.GetHitBit(5) != 0;
            hitBits[6] = val.GetHitBit(6) != 0;
            hitBits[7] = val.GetHitBit(7) != 0;
            hitBits[8] = val.GetHitBit(8) != 0;
            hitBits[9] = val.GetHitBit(9) != 0;
            hitBits[10] = val.GetHitBit(10) != 0;
            hitBits[11] = val.GetHitBit(11) != 0;
            hitBits[12] = val.GetHitBit(12) != 0;
            hitBits[13] = val.GetHitBit(13) != 0;
            unk3 = val.Unk3;
            shield = val.Shieldable;
            reflect = val.Reflectable;
            absorb = val.Absorbable;
            unk4 = val.Unk4;
            grip = val.Gripped;
            invinc = val.IgnoreInv;
            freeze = val.NoFreeze;
            sleep = val.Sleep;
            flinch = val.Flinchless;
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SpecialHitboxFlags
    {
        //0000 0000 0000 0000 0000 0000 0000 0111   Angle Flipping
        //0000 0000 0000 0000 0000 0000 0000 1000   Unknown1
        //0000 0000 0000 0000 0000 0000 0001 0000   Stretches
        //0000 0000 0000 0000 0000 0000 0010 0000   Unknown2

        //Hit Bits
        //               0000 0000 0000 01          Can Hit Multiplayer Characters
        //               0000 0000 0000 10          Can Hit SSE Enemies
        //               0000 0000 0001 00          Can Hit Unknown1
        //               0000 0000 0010 00          Can Hit Unknown2
        //               0000 0000 0100 00          Can Hit Unknown3
        //               0000 0000 1000 00          Can Hit Unknown4
        //               0000 0001 0000 00          Can Hit Unknown5
        //               0000 0010 0000 00          Can Hit Damageable Ceilings
        //               0000 0100 0000 00          Can Hit Damageable Walls
        //               0000 1000 0000 00          Can Hit Damageable Floors
        //               0001 0000 0000 00          Can Hit Unknown6
        //               0010 0000 0000 00          Can Hit Unknown7
        //               0100 0000 0000 00          Can Hit Unknown8
        //               1000 0000 0000 00          Enabled

        //0000 0000 0011 0000 0000 0000 0000 0000   Unknown3
        //0000 0000 0100 0000 0000 0000 0000 0000   Can be Shielded
        //0000 0000 1000 0000 0000 0000 0000 0000   Can be Reflected 
        //0000 0001 0000 0000 0000 0000 0000 0000   Can be Absorbed 
        //0000 0110 0000 0000 0000 0000 0000 0000   Unknown4
        //0000 1000 0000 0000 0000 0000 0000 0000   Hitting a gripped character
        //0001 0000 0000 0000 0000 0000 0000 0000   Ignore Invincibility
        //0010 0000 0000 0000 0000 0000 0000 0000   Freeze Frame Disable
        //0100 0000 0000 0000 0000 0000 0000 0000   Unknown5
        //1000 0000 0000 0000 0000 0000 0000 0000   Flinchless

        public int AngleFlipping => data & 7;

        //0, 2, 5: Regular angles; the target is always sent away from the attacker.
        //1, 3: The target is always sent the direction the attacker is facing.
        //4: The target is always sent the direction the attacker is not facing.
        //6, 7: The target is turned to the Z axis
        public int Unk1 => (data >> 3) & 1;
        public int Stretches => (data >> 4) & 1;
        public int Unk2 => (data >> 5) & 1;

        public int GetHitBit(int index)
        {
            return (data >> (6 + index)) & 1;
        } //Max index is 13, starting with 0

        public int Unk3 => (data >> 20) & 3;
        public int Shieldable => (data >> 22) & 1;
        public int Reflectable => (data >> 23) & 1;
        public int Absorbable => (data >> 24) & 1;
        public int Unk4 => (data >> 25) & 3;
        public int Gripped => (data >> 27) & 1;
        public int IgnoreInv => (data >> 28) & 1;
        public int NoFreeze => (data >> 29) & 1;
        public int Sleep => (data >> 30) & 1;
        public int Flinchless => (data >> 31) & 1;

        public int data;
    }

    #endregion
}