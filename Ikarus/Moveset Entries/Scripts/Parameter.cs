using System;
using System.Linq;
using System.ComponentModel;
using System.Windows.Forms;
using BrawlLib.SSBBTypes;
using System.Runtime.InteropServices;
using Ikarus.ModelViewer;

namespace Ikarus.MovesetFile
{
    public unsafe class Parameter : MovesetEntryNode
    {
        public Event _event;

        [Browsable(false)]
        public override int Index { get { return Array.IndexOf(_event.ToArray(), this); } }

        /// <summary>
        /// Returns the real value for use in scripting as a generic float value.
        /// Returns 1.0 if true, 0.0 if false.
        /// The only event type that allows you to set this is Variable.
        /// </summary>
        [Browsable(false)]
        public virtual double RealValue { get { return _value; } set { } }

        [Browsable(false)]
        public virtual int Data { get { return _value; } set { _value = value; SignalPropertyChange(); } }
        private int _value = 0;

        public Parameter() { _value = 0; }
        public Parameter(int value) { _value = value; }

        public static implicit operator int(Parameter val) { return (int)val._value; }

        [Browsable(false)]
        public virtual ParamType ParamType { get { return ParamType.Value; } }

        /// <summary>
        /// Use this to compare this parameter's real value to another's.
        /// </summary>
        public bool Compare(Parameter param, int compare)
        {
            switch (compare)
            {
                case 0: return RealValue < param.RealValue;
                case 1: return RealValue <= param.RealValue;
                case 2: return RealValue == param.RealValue;
                case 3: return RealValue != param.RealValue;
                case 4: return RealValue >= param.RealValue;
                case 5: return RealValue > param.RealValue;
                default: return false;
            }
        }

        [Browsable(false)]
        public string Description 
        {
            get 
            {
                if (Manager.Events.ContainsKey(_event.EventID))
                {
                    EventInformation e = Manager.Events[_event.EventID];
                    if (e._paramDescs != null && Index < e._paramDescs.Length)
                        return e._paramDescs[Index];
                }
                return "No Description Available.";
            }
        }

        public override string ToString()
        {
            if (Manager.Events.ContainsKey(_event.EventID))
            {
                EventInformation f = Manager.Events[_event.EventID];
                if (Index >= 0 && Index < f._paramNames.Length)
                    return f._paramNames[Index];
            }
            return ParamType.ToString();
        }

        public virtual string GetArg()
        {
            return Data.ToString();
        }

        protected override void OnParse(VoidPtr address)
        {
            sParameter* hdr = (sParameter*)address;
            _value = hdr->_data;
        }
        protected override int OnGetSize() { return 8; }
        protected override void OnWrite(VoidPtr address)
        {
            RebuildAddress = address;
            sParameter* header = (sParameter*)address;
            header->_type = (int)ParamType;
            header->_data = _value;
        }
    }

    #region Value Nodes
    public unsafe class EventValue : Parameter
    {
        [Browsable(false)]
        public override ParamType ParamType { get { return ParamType.Value; } }

        public EventValue() { }
        public EventValue(int value) : base(value) { }

        [Category("Event Value")]
        public int Value { get { return Data; } set { Data = value; } }
    }
    public unsafe class EventEnumValue : Parameter
    {
        public string[] Enums = new string[0];

        [Browsable(false)]
        public override ParamType ParamType { get { return ParamType.Value; } }

        public EventEnumValue() { }
        public EventEnumValue(int value, string[] enums) : base(value) { Enums = enums; }

        [Category("Event Value"), TypeConverter(typeof(DropDownListEnumMDef))]
        public string Value
        {
            get
            {
                if (Data >= 0 && Data < Enums.Length)
                    return Enums[Data];
                else
                    return Data.ToString();
            }
            set
            {
                int x;
                if (!int.TryParse(value, out x))
                    Data = Array.IndexOf(Enums, value);
                else
                    Data = x;

                if (Data == -1)
                    Data = 0;
            }
        }

        public override string GetArg()
        {
            return "Enum." + Value.Replace(" ", "");
        }
    }
    public unsafe class EventValue2Half : Parameter
    {
        [Browsable(false)]
        public override ParamType ParamType { get { return ParamType.Value; } }

        public EventValue2Half() { }
        public EventValue2Half(int value) : base(value) { }

        [Category("Event Value")]
        public short Value1 { get { return (short)((Data >> 16) & 0xFFFF); } set { Data = (Data & 0xFFFF) | ((value & 0xFFFF) << 16); } }
        [Category("Event Value")]
        public short Value2 { get { return (short)((Data) & 0xFFFF); } set { Data = (int)((uint)Data & 0xFFFF0000) | (value & 0xFFFF); } }
    }
    public unsafe class EventValueGFX : Parameter
    {
        [Browsable(false)]
        public override ParamType ParamType { get { return ParamType.Value; } }

        public EventValueGFX() { }
        public EventValueGFX(int value) : base(value) { }

        [Category("Event Value"), TypeConverter(typeof(DropDownListGFXFilesMDef))]
        public string GFXFile 
        {
            get
            {
                int index = ((Data >> 16) & 0xFFFF);
                if (Manager.iGFXFiles != null && Manager.iGFXFiles.Length > 0 && index < Manager.iGFXFiles.Length)
                    return Manager.iGFXFiles[index];
                else return index.ToString();
            } 
            set
            {
                int index = 0;
                if (!int.TryParse(value, out index))
                    if (Manager.iGFXFiles != null && Manager.iGFXFiles.Length > 0 && Manager.iGFXFiles.Contains(value))
                        index = Array.IndexOf(Manager.iGFXFiles, value);
                Data = (Data & 0xFFFF) | ((index & 0xFFFF) << 16);
            } 
        }
        [Category("Event Value")]
        public short EFLSEntryIndex { get { return (short)((Data) & 0xFFFF); } set { Data = (int)((uint)Data & 0xFFFF0000) | (value & 0xFFFF); } }

        public override string GetArg()
        {
            return "File." + GFXFile + "[" + EFLSEntryIndex + "]";
        }
    }
    public unsafe class EventValueHalf2Byte : Parameter
    {
        [Browsable(false)]
        public override ParamType ParamType { get { return ParamType.Value; } }

        public EventValueHalf2Byte() { }
        public EventValueHalf2Byte(int value) : base(value) { }

        [Category("Event Value")]
        public short Value1 { get { return (short)((Data >> 16) & 0xFFFF); } set { Data = (Data & 0xFFFF) | ((value & 0xFFFF) << 16); } }
        [Category("Event Value")]
        public byte Value2 { get { return (byte)((Data >> 8) & 0xFF); } set { Data = (int)((uint)Data & 0xFFFF00FF) | ((value & 0xFF) << 8); } }
        [Category("Event Value")]
        public byte Value3 { get { return (byte)((Data) & 0xFF); } set { Data = (int)((uint)Data & 0xFFFFFF00) | (value & 0xFF); } }
    }
    public unsafe class EventValue2ByteHalf : Parameter
    {
        [Browsable(false)]
        public override ParamType ParamType { get { return ParamType.Value; } }

        public EventValue2ByteHalf() { }
        public EventValue2ByteHalf(int value) : base(value) { }

        [Category("Event Value")]
        public byte Value1 { get { return (byte)((Data >> 24) & 0xFF); } set { Data = (int)((uint)Data & 0x00FFFFFF) | ((value & 0xFF) << 24); SignalPropertyChange(); } }
        [Category("Event Value")]
        public byte Value2 { get { return (byte)((Data >> 16) & 0xFF); } set { Data = (int)((uint)Data & 0xFF00FFFF) | ((value & 0xFF) << 16); SignalPropertyChange(); } }
        [Category("Event Value")]
        public short Value3 { get { return (short)((Data) & 0xFFFF); } set { Data = (int)((uint)Data & 0xFFFF0000) | (value & 0xFFFF); SignalPropertyChange(); } }
    }
    public unsafe class EventValue4Byte : Parameter
    {
        [Browsable(false)]
        public override ParamType ParamType { get { return ParamType.Value; } }

        public EventValue4Byte() { }
        public EventValue4Byte(int value) : base(value) { }

        [Category("Event Value")]
        public byte Value1 { get { return (byte)((Data >> 24) & 0xFF); } set { Data = (int)((uint)Data & 0x00FFFFFF) | ((value & 0xFF) << 24); } }
        [Category("Event Value")]
        public byte Value2 { get { return (byte)((Data >> 16) & 0xFF); } set { Data = (int)((uint)Data & 0xFF00FFFF) | ((value & 0xFF) << 16); } }
        [Category("Event Value")]
        public byte Value3 { get { return (byte)((Data >> 8) & 0xFF); } set { Data = (int)((uint)Data & 0xFFFF00FF) | ((value & 0xFF) << 8); } }
        [Category("Event Value")]
        public byte Value4 { get { return (byte)((Data) & 0xFF); } set { Data = (int)((uint)Data & 0xFFFFFF00) | (value & 0xFF); } }
    }
    #endregion
    
    public unsafe class EventFile : Parameter
    {
        [Browsable(false)]
        public override ParamType ParamType { get { return ParamType.File; } }

        public EventFile() { }
        public EventFile(int value) : base(value) { }

        [Category("Event File")]
        public int Value { get { return Data; } set { Data = value; SignalPropertyChange(); } }

        public override string GetArg()
        {
            return Value.ToString() + "f";
        }
    }

    public unsafe class EventOffset : Parameter
    {
        [Browsable(false)]
        public override ParamType ParamType { get { return ParamType.Offset; } }

        public EventOffset() { }
        public EventOffset(int value) : base(value) { }

        public ScriptOffsetInfo _offsetInfo = new ScriptOffsetInfo();

        [Category("Event Offset")]
        public int RawOffset
        {
            get { return Data; }
            set
            {
                if (value < 0)
                {
                    Data = value;
                    _offsetInfo._list = ListValue.Null;
                    _offsetInfo._type = TypeValue.None;
                    _offsetInfo._index = -1;
                    SignalPropertyChange();
                    return;
                }
                SakuraiEntryNode r = _root.GetEntry(value);
                if (r != null && r is Script)
                    Data = value;
                else
                    MessageBox.Show("An action could not be located.");
            }
        }
        [Category("Event Offset"), Browsable(true), TypeConverter(typeof(DropDownListExtNodesMDef))]
        public string ExternalNode
        {
            get { return _externalEntry != null ? _externalEntry.Name : null; }
            set
            {
                if (_externalEntry != null)
                    if (_externalEntry.Name != value)
                        _externalEntry.References.Remove(this);

                foreach (TableEntryNode e in _root.ReferenceList)
                    if (e.Name == value)
                    {
                        _externalEntry = e;
                        e.References.Add(this);
                        _script = null;
                        _offsetInfo._list = ListValue.References;
                        _offsetInfo._index = _externalEntry.Index;
                    }
            }
        }

        /// <summary>
        /// Use this only when parsing.
        /// </summary>
        /// <returns></returns>
        internal Script GetScript() { return ((MovesetNode)_root).GetScript(RawOffset); }
        public Script _script;

        public override void PostParse()
        {
            //Get script node using raw offset
            //This happens in post parse so that all scripts have been parsed already

            MovesetNode node = (MovesetNode)_root;

            SakuraiEntryNode e = _root.GetEntry(RawOffset);
            bool exist = e != null && e is Event;

            _offsetInfo = node.GetScriptLocation(RawOffset);

            Script a;
            if (_offsetInfo._list == ListValue.Null && !exist)
                node.SubRoutines.Add(a = Parse<Script>(RawOffset));
            else if (_offsetInfo._list != ListValue.References)
                a = node.GetScript(_offsetInfo);
            else
            {
                if (_externalEntry == null && _offsetInfo._index >= 0 && _offsetInfo._index < _root.ReferenceList.Count)
                {
                    _externalEntry = _root.ReferenceList[_offsetInfo._index];
                    _externalEntry.References.Add(this);
                }
                return;
            }

            if (a == null)
                a = GetScript();

            if (a != null)
                a._actionRefs.Add(this);
            else
                throw new Exception("Script not found.");

            _script = a;
        }

        protected override int OnGetLookupCount() { return _script != null ? 1 : 0; }
        protected override int OnGetSize() { return 8; }
        protected override void PostProcess(LookupManager lookupOffsets)
        {
            sParameter* arg = (sParameter*)RebuildAddress;
            arg->_type = (int)ParamType;
            if (_script != null)
            {
                //if (action._entryOffset == 0)
                //    Console.WriteLine("Action offset = 0");
                
                arg->_data = Offset(_script.RebuildAddress);
                if (arg->_data > 0)
                    lookupOffsets.Add(arg->_data.Address);
            }
            else
            {
                arg->_data = -1;
                if (External)
                    RebuildAddress += 4;
                else
                    foreach (TableEntryNode e in _root.ReferenceList)
                        if (e.Name == this.Name)
                        {
                            _externalEntry = e;
                            //if (!e.References.Contains(this))
                                e.References.Add(this);
                            RebuildAddress += 4;
                            break;
                        }
            }
        }

        public override string GetArg()
        {
            if (_externalEntry != null)
                return "Ref." + _externalEntry.Name;
            string add = "";
            if (_offsetInfo._list != ListValue.Null)
                add += String.Format("[{0}]", _offsetInfo._index.ToString());
            if ((int)_offsetInfo._list < 2)
            {
                string v = _offsetInfo._type.ToString();
                if (_offsetInfo._list == ListValue.SubActions && (int)_offsetInfo._type < 2)
                    if (_offsetInfo._type == TypeValue.Entry) v = "Main"; else v = "GFX";
                add += String.Format(".{0}", v);
            }
            return "Script." + _offsetInfo._list.ToString() + add;
        }
    }

    public unsafe class EventScalar : Parameter
    {
        [Browsable(false)]
        public override ParamType ParamType { get { return ParamType.Scalar; } }

        public EventScalar() { }
        public EventScalar(int value) : base(value) { }

        [Category("Event Scalar Value")]
        public float Value { get { return Util.UnScalar(Data); } set { Data = Util.Scalar(value); } }

        public override double RealValue { get { return Value; } }

        public override string GetArg()
        {
            return ((Value % 1) == 0) ? Value.ToString("f1") : Value.ToString();
        }
    }
    
    public unsafe class EventBool : Parameter
    {
        [Browsable(false)]
        public override ParamType ParamType { get { return ParamType.Boolean; } }

        public EventBool() { }
        public EventBool(int value) : base(value) { }

        [Category("Event Boolean")]
        public bool Value { get { return Data == 1 ? true : false; } set { Data = value ? 1 : 0; } }

        public override string GetArg()
        {
            return Value.ToString().ToLower();
        }
    }

    public unsafe class EventVariable : Parameter
    {
        public override double RealValue
        {
            get { return RunTime.GetVar(type, mem, number); }
            set { RunTime.SetVar(type, mem, number, value); }
        }

        [Browsable(false)]
        public override ParamType ParamType { get { return ParamType.Variable; } }

        public EventVariable() { }
        public EventVariable(int value) : base(value)
        {
            _val = ResolveVariable(Data);
        }

        internal string _val;
        internal int number;
        internal VarMemType mem;
        internal VariableType type;

        [Category("Variable")]
        public VarMemType MemType { get { return mem; } set { mem = value; GetValue(); } }
        [Category("Variable")]
        public VariableType VarType { get { return type; } set { type = value; GetValue(); } }
        [Category("Variable")]
        public int Number { get { return number; } set { number = value; GetValue(); } }

        protected override void OnParse(VoidPtr address)
        {
            base.OnParse(address);
            _val = ResolveVariable(Data);
        }

        public void GetValue()
        {
            _val = ResolveVariable(Data = ((int)mem * 0x10000000) + ((int)type * 0x1000000) + number.Clamp(0, 0xFFFFFF));
        }

        public override string ToString()
        {
            return _val == null ? _val = ResolveVariable(Data) : _val;
        }

        public string ResolveVariable(long value)
        {
            string variableName = "";
            long variableMemType = (value >> 28) & 0xF;
            long variableType = (value >> 24) & 0xF;
            long variableNumber = (value & 0xFFFFFF);
            if (variableMemType == 0) { variableName = "IC."; mem = VarMemType.IC; }
            if (variableMemType == 1) { variableName = "LA."; mem = VarMemType.LA; }
            if (variableMemType == 2) { variableName = "RA."; mem = VarMemType.RA; }
            if (variableType == 0) { variableName += "Basic"; type = VariableType.Basic; }
            if (variableType == 1) { variableName += "Float"; type = VariableType.Float; }
            if (variableType == 2) { variableName += "Bit"; type = VariableType.Bit; }
            variableName += "[" + (number = (int)variableNumber) + "]";

            return variableName;
        }

        public override string GetArg()
        {
            return _val;
        }
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

    public unsafe class EventRequirement : Parameter
    {
        [Browsable(false)]
        public override ParamType ParamType { get { return ParamType.Requirement; } }

        public EventRequirement() { }
        public EventRequirement(int value) : base(value)
        {
            _val = GetRequirement(Data);
        }

        internal string _val;
        internal bool not;
        internal string arg;

        [Category("Requirement"), TypeConverter(typeof(DropDownListRequirementsMDef))]
        public string Requirement { get { return arg; } set { if (Array.IndexOf(Manager.iRequirements, value) == -1) return; arg = value; GetValue(); SignalPropertyChange(); } }
        [Category("Requirement")]
        public bool Not { get { return not; } set { not = value; GetValue(); } }

        protected override void OnParse(VoidPtr address)
        {
 	        base.OnParse(address);
            _val = GetRequirement(Data);
        }

        public override double RealValue
        {
            get
            {
                float value = Array.IndexOf(Manager.iRequirements, arg);
                return value * (not ? -1 : 1);
            }
        }

        public void GetValue()
        {
            long value = Array.IndexOf(Manager.iRequirements, arg);
            if (not) value |= (1 << 31);
            _val = GetRequirement(Data = (int)value);
        }

        public override string ToString()
        {
            return _val == null ? _val = GetRequirement(Data) : _val;
        }

        public string GetRequirement(int value)
        {
            not = ((value >> 31) & 1) == 1;
            int requirement = value & 0x7FFFFFFF;

            if (requirement >= Manager.iRequirements.Length)
                return requirement.ToString();

            if (not == true)
                return "Not " + (arg = Manager.iRequirements[requirement]);

            return (arg = Manager.iRequirements[requirement]);
        }

        public override string GetArg()
        {
            return (Not ? "!" : "") + "Req." + Requirement;
        }
    }

    #region htBoxes
    public unsafe class HitboxFlagsNode : Parameter
    {
        internal HitboxFlags val = new HitboxFlags();

        public string HexValue
        {
            get { return Data.ToString("X8"); }
            set { val._data = (uint)(Data = Int32.Parse(value, System.Globalization.NumberStyles.HexNumber)); }
        }

        [Category("Hitbox Flags")]
        public Util.HitboxEffect Effect { get { return (Util.HitboxEffect)val.Effect; } set { val.Effect = (uint)value; SetValue(); } }
        [Category("Hitbox Flags")]
        public bool Unk1 { get { return val.Unk1; } set { val.Unk1 = value; SetValue(); } }
        [Category("Hitbox Flags")]
        public Util.HitboxSFX Sound { get { return (Util.HitboxSFX)val.Sound; } set { val.Sound = (uint)value; SetValue(); } }
        [Category("Hitbox Flags")]
        public uint Unk2 { get { return val.Unk2; } set { val.Unk2 = value; SetValue(); } }
        [Category("Hitbox Flags")]
        public bool Grounded { get { return val.Grounded; } set { val.Grounded = value; SetValue(); } }
        [Category("Hitbox Flags")]
        public bool Aerial { get { return val.Aerial; } set { val.Aerial = value; SetValue(); } }
        [Category("Hitbox Flags")]
        public uint Unk3 { get { return val.Unk3; } set { val.Unk3 = value; SetValue(); } }
        [Category("Hitbox Flags")]
        public Util.HitboxType Type { get { return (Util.HitboxType)val.Type; } set { val.Type = (uint)value; SetValue(); } }
        [Category("Hitbox Flags")]
        public bool Clang { get { return val.Clang; } set { val.Clang = value; SetValue(); } }
        [Category("Hitbox Flags")]
        public bool Unk4 { get { return val.Unk4; } set { val.Unk4 = value; SetValue(); } }
        [Category("Hitbox Flags")]
        public bool Direct { get { return val.Direct; } set { val.Direct = value; SetValue(); } }
        [Category("Hitbox Flags")]
        public uint Unk5 { get { return val.Unk5; } set { val.Unk5 = value; SetValue(); } }
        
        public HitboxFlagsNode() { }
        public HitboxFlagsNode(int value) : base(value)
        {
            val._data = (uint)Data;
        }

        protected override void OnParse(VoidPtr address)
        {
            base.OnParse(address);
            val._data = (uint)Data;
        }

        private void SetValue()
        {
            Data = (int)(uint)val._data;
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

        public uint Effect { get { return _data[0, 5]; } set { _data[0, 5] = value.Clamp(0, 31); } }
        public bool Unk1 { get { return _data[5]; } set { _data[5] = value; } }
        public uint Sound { get { return _data[6, 8]; } set { _data[6, 8] = value.Clamp(0, 255); } }
        public uint Unk2 { get { return _data[14, 2]; } set { _data[14, 2] = value.Clamp(0, 3); } }
        public bool Grounded { get { return _data[16]; } set { _data[16] = value; } }
        public bool Aerial { get { return _data[17]; } set { _data[17] = value; } }
        public uint Unk3 { get { return _data[18, 4]; } set { _data[18, 4] = value.Clamp(0, 15); } }
        public uint Type { get { return _data[22, 5]; } set { _data[22, 5] = value.Clamp(0, 31); } }
        public bool Clang { get { return _data[27]; } set { _data[27] = value; } }
        public bool Unk4 { get { return _data[28]; } set { _data[28] = value; } }
        public bool Direct { get { return _data[29]; } set { _data[29] = value; } }
        public uint Unk5 { get { return _data[30, 2]; } set { _data[30, 2] = value.Clamp(0, 3); } }

        public Bin32 _data;
    }

    public unsafe class SpecialHitboxFlagsNode : Parameter
    {
        internal SpecialHitboxFlags val = new SpecialHitboxFlags();

        public string HexValue 
        {
            get { return Data.ToString("X8"); }
            set { val._data = (uint)(Data = Int32.Parse(value, System.Globalization.NumberStyles.HexNumber)); }
        }
        
        [Category("Special Hitbox Flags")]
        public uint AngleFlipping { get { return val.AngleFlipping; } set { val.AngleFlipping = value; SetValue(); } }
        [Category("Special Hitbox Flags")]
        public bool Unk1 { get { return val.Unk1; } set { val.Unk1 = value; SetValue(); } }
        [Category("Special Hitbox Flags")]
        public bool Stretches { get { return val.Stretches; } set { val.Stretches = value; SetValue(); } }
        [Category("Special Hitbox Flags")]
        public bool Unk2 { get { return val.Unk2; } set { val.Unk2 = value; SetValue(); } }

        [Category("Hit Flags")]
        public bool CanHitMultiplayerCharacters { get { return val.GetHitBit(0); } set { val.SetHitBit(0, value); SetValue(); } }
        [Category("Hit Flags")]
        public bool CanHitSSEenemies { get { return val.GetHitBit(1); } set { val.SetHitBit(1, value); SetValue(); } }
        [Category("Hit Flags")]
        public bool CanHitUnk1 { get { return val.GetHitBit(2); } set { val.SetHitBit(2, value); SetValue(); } }
        [Category("Hit Flags")]
        public bool CanHitUnk2 { get { return val.GetHitBit(3); } set { val.SetHitBit(3, value); SetValue(); } }
        [Category("Hit Flags")]
        public bool CanHitUnk3 { get { return val.GetHitBit(4); } set { val.SetHitBit(4, value); SetValue(); } }
        [Category("Hit Flags")]
        public bool CanHitUnk4 { get { return val.GetHitBit(5); } set { val.SetHitBit(5, value); SetValue(); } }
        [Category("Hit Flags")]
        public bool CanHitUnk5 { get { return val.GetHitBit(6); } set { val.SetHitBit(6, value); SetValue(); } }
        [Category("Hit Flags")]
        public bool CanHitDamageableCeilings { get { return val.GetHitBit(7); } set { val.SetHitBit(7, value); SetValue(); } }
        [Category("Hit Flags")]
        public bool CanHitDamageableWalls { get { return val.GetHitBit(8); } set { val.SetHitBit(8, value); SetValue(); } }
        [Category("Hit Flags")]
        public bool CanHitDamageableFloors { get { return val.GetHitBit(9); } set { val.SetHitBit(9, value); SetValue(); } }
        [Category("Hit Flags")]
        public bool CanHitUnk6 { get { return val.GetHitBit(10); } set { val.SetHitBit(10, value); SetValue(); } }
        [Category("Hit Flags")]
        public bool CanHitUnk7 { get { return val.GetHitBit(11); } set { val.SetHitBit(11, value); SetValue(); } }
        [Category("Hit Flags")]
        public bool CanHitUnk8 { get { return val.GetHitBit(12); } set { val.SetHitBit(12, value); SetValue(); } }
        [Category("Hit Flags")]
        public bool Enabled { get { return val.GetHitBit(13); } set { val.SetHitBit(13, value); SetValue(); } }

        [Category("Special Hitbox Flags")]
        public uint Unk3 { get { return val.Unk3; } set { val.Unk3 = value; SetValue(); } }
        [Category("Special Hitbox Flags")]
        public bool CanBeShielded { get { return val.Shieldable; } set { val.Shieldable = value; SetValue(); } }
        [Category("Special Hitbox Flags")]
        public bool CanBeAbsorbed { get { return val.Absorbable; } set { val.Absorbable = value; SetValue(); } }
        [Category("Special Hitbox Flags")]
        public bool CanBeReflected { get { return val.Reflectable; } set { val.Reflectable = value; SetValue(); } }
        [Category("Special Hitbox Flags")]
        public uint Unk4 { get { return val.Unk4; } set { val.Unk4 = value; SetValue(); } }
        [Category("Special Hitbox Flags")]
        public bool HittingGrippedCharacter { get { return val.Gripped; } set { val.Gripped = value; SetValue(); } }
        [Category("Special Hitbox Flags")]
        public bool IgnoreInvincibility { get { return val.IgnoreInv; } set { val.IgnoreInv = value; SetValue(); } }
        [Category("Special Hitbox Flags")]
        public bool FreezeFrameDisable { get { return val.NoFreeze; } set { val.NoFreeze = value; SetValue(); } }
        [Category("Special Hitbox Flags")]
        public bool PutsToSleep { get { return val.Sleep; } set { val.Sleep = value; SetValue(); } }
        [Category("Special Hitbox Flags")]
        public bool Flinchless { get { return val.Flinchless; } set { val.Flinchless = value; SetValue(); } }
        
        public SpecialHitboxFlagsNode() { }
        public SpecialHitboxFlagsNode(int value) : base(value)
        {
            val._data = (uint)Data;
        }

        protected override void OnParse(VoidPtr address)
        {
 	         base.OnParse(address);
             val._data = (uint)Data;
        }
        
        private void SetValue()
        {
            Data = (int)(uint)val._data;
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

        public uint AngleFlipping { get { return _data[0, 3]; } set { _data[0, 3] = value.Clamp(0, 7); } }
        //0, 2, 5: Regular angles; the target is always sent away from the attacker.
        //1, 3: The target is always sent the direction the attacker is facing.
        //4: The target is always sent the direction the attacker is not facing.
        //6, 7: The target is turned to the Z axis

        public bool Unk1 { get { return _data[3]; } set { _data[3] = value; } }
        public bool Stretches { get { return _data[4]; } set { _data[4] = value; } }
        public bool Unk2 { get { return _data[5]; } set { _data[5] = value; } }

        public bool GetHitBit(int index) { return _data[6 + index.Clamp(0, 13)]; }
        public void SetHitBit(int index, bool value) { _data[6 + index.Clamp(0, 13)] = value; }

        public uint Unk3 { get { return _data[20, 2]; } set { _data[20, 2] = value.Clamp(0, 3); } }
        public bool Shieldable { get { return _data[22]; } set { _data[22] = value; } }
        public bool Reflectable { get { return _data[23]; } set { _data[23] = value; } }
        public bool Absorbable { get { return _data[24]; } set { _data[24] = value; } }
        public uint Unk4 { get { return _data[25, 2]; } set { _data[25, 2] = value.Clamp(0, 3); } }
        public bool Gripped { get { return _data[27]; } set { _data[27] = value; } }
        public bool IgnoreInv { get { return _data[28]; } set { _data[28] = value; } }
        public bool NoFreeze { get { return _data[29]; } set { _data[29] = value; } }
        public bool Sleep { get { return _data[30]; } set { _data[30] = value; } }
        public bool Flinchless { get { return _data[31]; } set { _data[31] = value; } }

        public Bin32 _data;
    }
    #endregion
}
