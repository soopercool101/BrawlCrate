using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using OpenTK.Graphics.OpenGL;
using System.Collections;
using BrawlLib.SSBB.ResourceNodes;
using BrawlLib.OpenGL;
using Ikarus.ModelViewer;
using BrawlLib.Modeling;
using BrawlLib.SSBBTypes;

namespace Ikarus.MovesetFile
{
    public unsafe class Event : MovesetEntryNode, IEnumerable<Parameter>
    {
        #region Child Enumeration

        public IEnumerator<Parameter> GetEnumerator() { return _children.GetEnumerator(); }
        IEnumerator IEnumerable.GetEnumerator() { return this.GetEnumerator(); }

        private List<Parameter> _children = new List<Parameter>();

        public int Count { get { return _children.Count; } }
        public Parameter this[int i]
        {
            get
            {
                if (i >= 0 && i < Count)
                    return _children[i];
                return null;
            }
            set
            {
                if (i >= 0 && i < Count)
                {
                    SignalPropertyChange();
                    _children[i] = value;
                    value._event = this;
                }
            }
        }
        private void Insert(int i, Parameter e)
        {
            if (i >= 0)
            {
                if (i < Count)
                    _children.Insert(i, e);
                else
                    _children.Add(e);
                e._event = this;
                SignalRebuildChange();
            }
        }
        private void Add(int i, Parameter e)
        {
            _children.Add(e);
            e._event = this;
            SignalRebuildChange();
        }
        internal void RemoveAt(int i)
        {
            if (i >= 0 && i < Count)
            {
                _children.RemoveAt(i);
                SignalRebuildChange();
            }
        }
        private void Clear()
        {
            if (Count != 0)
            {
                _children.Clear();
                SignalRebuildChange();
            }
        }

        #endregion

        public Script _script;

        [Browsable(false)]
        public override int Index { get { return Array.IndexOf(_script.ToArray(), this); } }

        [Browsable(false)]
        public uint EventID
        {
            get { return _event; }
            set
            {
                _event = value;
                _nameSpace = (byte)((_event >> 24) & 0xFF);
                _id = (byte)((_event >> 16) & 0xFF);
                _numArgs = (byte)((_event >> 8) & 0xFF);
                _unknown = (byte)((_event >> 0) & 0xFF);

                _name = Manager.Events.ContainsKey(_event) ? 
                    Manager.Events[_event]._name :
                    Helpers.Hex8(_event);
            }
        }

        [Browsable(false)]
        public EventInformation Info
        {
            get
            {
                if (Manager.Events.ContainsKey(_event))
                    return Manager.Events[_event];
                else
                    return null;
            }
        }
        
        [Category("Event")]
        public byte NameSpace { get { return _nameSpace; } }
        [Category("Event")]
        public byte ID { get { return _id; } }
        [Category("Event")]
        public byte NumArguments { get { return _numArgs; } }
        [Category("Event")]
        public byte Unknown { get { return _unknown; } set { _unknown = value; SignalPropertyChange(); } }
        [Category("Event")]
        public string EventOffset { get { return "0x" + Util.Hex(_eventOffset); } }

        private uint _event;
        private uint _eventOffset = 0;
        private byte _nameSpace, _id, _numArgs;
        public byte _unknown;

        #region Serialization
        public string Serialize()
        {
            string s = "";
            s += Helpers.Hex8(EventID) + "|";
            foreach (Parameter p in _children)
            {
                s += ((int)p.ParamType).ToString() + "\\";
                if (p.ParamType == ParamType.Offset)
                {
                    EventOffset o = p as EventOffset;
                    s += o._offsetInfo._list + "," + o._offsetInfo._type + "," + o._offsetInfo._index;
                }
                else s += p.Data;
                s += "|";
            }
            return s;
        }

        public static Event Deserialize(string s, MovesetNode node)
        {
            if (String.IsNullOrEmpty(s))
                return null;

            try
            {
                string[] lines = s.Split('|');

                if (lines[0].Length != 8)
                    return null;

                Event newEv = new Event();

                string id = lines[0];
                uint idNumber = Convert.ToUInt32(id, 16);

                newEv.EventID = idNumber;
                
                uint _event = newEv.EventID;
                EventInformation info = newEv.Info;

                for (int i = 0; i < newEv._numArgs; i++)
                {
                    string[] pLines = lines[i + 1].Split('\\');

                    int type = int.Parse(pLines[0]);

                    if (type == 2) //Offset
                    {
                        string[] oLines = pLines[1].Split(',');
                        int list = int.Parse(oLines[0]), type2 = int.Parse(oLines[1]), index = int.Parse(oLines[2]);
                        ScriptOffsetInfo w = new ScriptOffsetInfo((ListValue)list, (TypeValue)type2, index);
                        EventOffset o = (EventOffset)newEv.NewParam(i, 0, ParamType.Offset);
                        o._script = node.GetScript(w);
                        o._offsetInfo = w;
                    }
                    else
                    {
                        int value = int.Parse(pLines[1]);
                        newEv.NewParam(i, value, type);
                    }
                }

                return newEv;
            }
            catch { return null; }
        }
        #endregion

        #region Parameters

        /// <summary>
        /// Resets all parameters to their default state with a value of 0.
        /// </summary>
        public void Reset()
        {
            _children.Clear();
            for (int i = 0; i < _numArgs; i++) 
                NewParam(i, 0);
        }

        public MovesetEntryNode NewParam(int i, int value) { return NewParam(i, value, -1); }
        public MovesetEntryNode NewParam(int i, int value, ParamType type) { return NewParam(i, value, (int)type); }
        public MovesetEntryNode NewParam(int i, int value, int typeOverride)
        {
            Parameter child = null;
            EventInformation info = Info;
            ParamType type = ParamType.Value;
            if (typeOverride >= 0)
                type = (ParamType)typeOverride;
            else if (info != null)
                type = (ParamType)info.GetDefaults(i);

            switch (type)
            {
                case ParamType.Offset:
                    child = new EventOffset(value);
                    break;
                case ParamType.Requirement:
                    child = new EventRequirement(value);
                    break;
                case ParamType.Variable:
                    child = new EventVariable(value);
                    break;
                case ParamType.Scalar:
                    child = new EventScalar(value);
                    break;
                case ParamType.File:
                    child = new EventFile(value);
                    break;
                case ParamType.Boolean:
                    child = new EventBool(value);
                    break;
                case ParamType.Value:
                    string name = info != null && i < info._paramNames.Length ? info._paramNames[i] : "Value";

                    //Check for some special types

                    if //Hitbox Flags
                        ((_event == 0x06000D00
                        || _event == 0x06150F00
                        || _event == 0x062B0D00)
                        && i == 12)
                        child = new HitboxFlagsNode(value);

                    else if (//Two Half Values
                        ((_event == 0x06000D00
                        || _event == 0x06150F00
                        || _event == 0x062B0D00)
                        && (i == 0 || i == 3 || i == 4)))
                        child = new EventValue2Half(value);

                    else if //GFX Selector
                        ((_event == 0x11150300
                        || _event == 0x11001000
                        || _event == 0x11010A00
                        || _event == 0x11020A00)
                        && i == 0)
                        child = new EventValueGFX(value);

                    else if (
                        i == 14 &&
                        _event == 0x06150F00)
                        child = new SpecialHitboxFlagsNode(value);

                    else //Not a special value
                    {
                        if (Info != null && Info._enums != null && Info._enums.ContainsKey(i))
                            child = new EventEnumValue(value, Info._enums[i].ToArray());
                        else
                            child = new EventValue(value);
                    }
                    break;
            }

            _children.Insert(i, child);
            child._event = this;
            child._root = _root;

            return child;
        }

        #endregion

        #region Reading/Writing
        protected override void OnParse(VoidPtr address)
        {
            sEvent* e = (sEvent*)address;
            _eventOffset = e->_argumentOffset;
            _nameSpace = e->_nameSpace;
            _id = e->_id;
            _numArgs = e->_numArguments;
            _unknown = e->_unk1;

            _event = ((uint)*(buint*)address) & 0xFFFFFF00;
            _name = Manager.Events.ContainsKey(_event) ? Manager.Events[_event]._name : Helpers.Hex8(_event);

            if (_name == "FADEF00D" || _name == "FADE0D8A")
                return;

            sParameter* args = (sParameter*)Address((int)e->_argumentOffset);
            for (int i = 0; i < _numArgs; i++)
            {
                sParameter* arg = &args[i];

                Parameter parameter = null;
                ParamType type = (ParamType)(int)arg->_type;

                //First check for special event parameters
                if ((_event == 0x06000D00 || _event == 0x06150F00 || _event == 0x062B0D00) && i == 12)
                    parameter = Parse<HitboxFlagsNode>(arg);
                else if (((_event == 0x06000D00 || _event == 0x06150F00 || _event == 0x062B0D00) && (i == 0 || i == 3 || i == 4)))
                    parameter = Parse<EventValue2Half>(arg);
                else if (((_event == 0x11150300 || _event == 0x11001000 || _event == 0x11010A00 || _event == 0x11020A00) && i == 0))
                    parameter = Parse<EventValueGFX>(arg);
                else if (i == 14 && _event == 0x06150F00)
                    parameter = Parse<SpecialHitboxFlagsNode>(arg);
                else
                    switch (type)
                    {
                        case ParamType.Scalar:
                            parameter = Parse<EventScalar>(arg);
                            break;
                        case ParamType.Boolean:
                            parameter = Parse<EventBool>(arg);
                            break;
                        case ParamType.File:
                            parameter = Parse<EventFile>(arg);
                            break;
                        case ParamType.Variable:
                            parameter = Parse<EventVariable>(arg);
                            break;
                        case ParamType.Requirement:
                            parameter = Parse<EventRequirement>(arg);
                            break;
                        case ParamType.Value:
                            if (Info != null && Info._enums != null && Info._enums.ContainsKey(i))
                            {
                                parameter = Parse<EventEnumValue>(arg);
                                ((EventEnumValue)parameter).Enums = Info._enums[i].ToArray();
                            }
                            else
                                parameter = Parse<EventValue>(arg);
                            break;
                        case ParamType.Offset:

                            //The offset stored in the offset parameter
                            int offset = arg->_data;

                            //If the offset is -1, this is probably an external reference.
                            //Otherwise, check to see if the offset point to an external entry.
                            TableEntryNode ext;
                            if (offset == -1)
                                ext = _root.TryGetExternal((int)_eventOffset + i * 8 + 4);
                            else
                                ext = _root.TryGetExternal(offset);

                            //Add ID node
                            parameter = Parse<EventOffset>(arg);
                            if (ext != null)
                            {
                                parameter._externalEntry = ext;
                                ext.References.Add(parameter);
                            }
                            else if (offset > 0)
                                _root._postParseEntries.Add(parameter);

                            break;
                    }
                    
                parameter._event = this;
                _children.Add(parameter);
            }
        }
        protected override int OnGetLookupCount()
        {
            int i = _children.Count > 0 ? 1 : 0;
            foreach (Parameter p in _children)
                i += p.GetLookupCount();
            return i;
        }

        protected override int OnGetSize()
        {
            int size = 8;
            foreach (Parameter p in _children)
                size += p.GetSize();
            return size;
        }
        #endregion

        #region Script Syntax
        private static string Comp(int i)
        {
            return Util.GetComparisonSign(i);
        }
        private static string Loop(int i)
        {
            if (i < 0)
                return "infinite";
            return i.ToString();
        }
        internal Event Prev { get { return Index > 0 ? _script[Index - 1] : null; } }
        internal Event Next { get { return Index < _script.Count - 1 ? _script[Index + 1] : null; } }
        internal string Arg(int i) { return this[i].GetArg(); }
        internal int Data(int i) { return this[i].Data; }
        public string GetFormattedSyntax()
        {
            string name = Info == null ? EventID.ToString("X8") : Info._compressedName;
            string n = Environment.NewLine;

            #region Special Events
            //This region contains code that displays certain events in a neat format.
            switch (_nameSpace)
            {
                case 0: //Execution Flow
                    switch (_id)
                    {
                        case 0x04: //Set Loop
                            return String.Format("loop ({0})", Loop(Data(0))) + n + "{";
                        case 0x05: //Execute Loop
                            return "}";
                        case 0x0A: //If
                        case 0x0D: //Else If
                            string val = (_id == 0xD) ? ("}" + n + "else if (") : ("if (");
                            Event ev = this;
                            List<string> args = new List<string>();
                            List<string> comps = new List<string>();
                    Top:
                            string thisArg = "";

                            //Requirement arguments use all the following parameters it requires
                            //A better way to handle them would be to store syntax for every requirement
                            //But this works for now

                            switch (ev.Count)
                            {
                                case 1:
                                    thisArg += ev.Arg(0);
                                    break;
                                case 2:
                                    thisArg += String.Format("{0}({1})", ev.Arg(0), ev.Arg(1));
                                    break;
                                case 3:
                                    thisArg += String.Format("{0}({1}), {2}", ev.Arg(0), ev.Arg(1), ev.Arg(2));
                                    break;
                                case 4:
                                    //thisArg += String.Format("{0}({1} {2} {3})", ev.Arg(0), ev.Arg(1), Comp(ev.Data(2)), ev.Arg(3));
                                    thisArg += String.Format("{0} {1} {2}", ev.Arg(1), Comp(ev.Data(2)), ev.Arg(3));
                                    break;
                            }

                            args.Add(thisArg);

                            //Check for following AND or OR 'if' events
                            if (ev.Next != null)
                            {
                                ev = ev.Next;
                                if (ev._nameSpace == 0)
                                    if (ev._id == 0xB)
                                    {
                                        comps.Add(" && ");
                                        goto Top;
                                    }
                                    else if (ev._id == 0xC)
                                    {
                                        comps.Add(" || ");
                                        goto Top;
                                    }
                            }
                            bool first = true;
                            int t = 0;
                            bool c = args.Count > 1;
                            foreach (string a in args)
                            {
                                if (first)
                                    first = false;
                                else
                                    val += n + comps[t++];

                                if (c) val += "(";
                                val += a;
                                if (c) val += ")";
                            }

                            val += ")" + n + "{";
                            return val;

                        case 0x0B: //And
                        case 0x0C: //Or
                            return null; //This is handled by the parent 'If' event above
                        case 0x0E: //Else
                            return "}" + n + "else" + n + "{";
                        case 0x0F: //End If
                            return "}";
                        case 0x10: //Switch
                            return String.Format("switch ({0}, {1})", Arg(1), Arg(0)) + n + "{";
                        case 0x11: //Case
                        case 0x12: //Default case

                            string start = "";
                            string mid = "";
                            string end = "";

                            if (Prev != null && Prev.EventID != 0x00100200 && Prev.EventID != 0x00110100)
                                start = "}" + n;

                            if (_id == 0x12)
                                mid = "default:";
                            else
                                mid = String.Format("case {0}:", Arg(0));

                            if (Next != null && Next.EventID != 0x00110100)
                                    end = n + "{";

                            return start + mid + end;

                        case 0x13: //End switch

                            string va2 = "";

                            if (Prev != null && Prev.EventID != 0x00100200)
                                va2 += "    }" + n; //Forced tab

                            return va2 + "}";
                        case 0x18: //break
                            return "break;";
                    }
                    break;
                case 0x12: //Variables
                    switch (_id)
                    {
                        case 0x00:
                        case 0x06:
                            return String.Format("{0} = {1};", Arg(1), Arg(0));
                        case 0x01:
                        case 0x07:
                            return String.Format("{0} += {1};", Arg(1), Arg(0));
                        case 0x02:
                        case 0x08:
                            return String.Format("{0} -= {1};", Arg(1), Arg(0));
                        case 0x03:
                            return String.Format("{0}++;", Arg(0));
                        case 0x04:
                            return String.Format("{0}--;", Arg(0));
                        case 0x0A:
                            return String.Format("{0} = true;", Arg(0));
                        case 0x0B:
                            return String.Format("{0} = false;", Arg(0));
                        case 0x0F:
                            return String.Format("{0} *= {1};", Arg(1), Arg(0));
                        case 0x10:
                            return String.Format("{0} /= {1};", Arg(1), Arg(0));
                        case 0x12:
                            return String.Format("{0} ?= {1};", Arg(1), Arg(0));
                    }
                    break;
            }
            #endregion

            string s = name + "(";
            int z = 0;
            foreach (Parameter p in this)
            {
                s += p.GetArg();
                if (++z != Count)
                    s += ", ";
            }
            s += ");";
            return s;
        }
        #endregion
    }

    public enum NameSpaceEnum : byte
    {
        ExecutionFlow = 0x00,
        LoopRest = 0x01,
        Actions = 0x02,
        SubActions = 0x04,
        Posture = 0x05,
        Collisions = 0x06,
        Controller = 0x07,
        EdgeInteraction = 0x08,
        Unknown09 = 0x09,
        Sounds = 0x0A,
        Models = 0x0B,
        CharacterSpecific = 0x0C,
        ConcurrentExecution = 0x0D,
        Movement = 0x0E,
        Unknown15 = 0x0F,
        Articles = 0x10,
        Graphics = 0x11,
        Variables = 0x12,
        Unknown19 = 0x13,
        AestheticWind = 0x14,
        Unknown21 = 0x15,
        Physics = 0x17,
        TerrainInteraction = 0x18,
        Unknown25 = 0x19,
        Camera = 0x1A,
        ProcedureCall = 0x1B,
        ArmorDamage = 0x1E,
        Items = 0x1F,
        Unknown32 = 0x20,
        FlashOverlays = 0x21,
        TeamAssociation = 0x22,
        Cancelling = 0x64,
        Unknown101 = 0x65,
        Unknown102 = 0x66,
        Unknown105 = 0x69,
        Unknown106 = 0x6A,
        Unknown107 = 0x6B,
        Unknown110 = 0x6E,
    }

    public class HitBox
    {
        //A seperate class for rendering hitboxes.
        //This allows the values to be modified by other events.
        public HitBox(Event ev, int articleIndex)
        {
            Root = ev._root as MovesetNode;
            _event = ev.EventID;
            if (_event != 0x060A0800)
                flags = ev[12] as HitboxFlagsNode;
            if (_event == 0x06150F00)
                specialFlags = ev[14] as SpecialHitboxFlagsNode;
            if ((_articleIndex = articleIndex) < 0)
                _model = RunTime.MainWindow.TargetModel as MDL0Node;
            else
                _model = RunTime._articles[_articleIndex]._model;
            _parameters = ev.Select(x => x.Data).ToArray();
        }

        public int _articleIndex;
        public MDL0Node _model;
        public MovesetNode Root;
        public int HitboxID = -1;
        public int HitboxSize = 0;
        public uint _event = 0;
        public HitboxFlagsNode flags;
        public SpecialHitboxFlagsNode specialFlags;
        public int[] _parameters;

        public bool IsOffensive(bool includeSpecial)
        {
            return (_event == 0x06000D00 || _event == 0x062B0D00) || (includeSpecial ? IsSpecialOffensive() : false);
        }
        public bool IsSpecialOffensive()
        {
            return _event == 0x06150F00;
        }
        public bool IsCatch()
        {
            return _event == 0x060A0800 || _event == 0x060A0900 || _event == 0x060A0A00;
        }

        public void Render(Vector3 cam)
        {
            if (IsOffensive(false))
                RenderOffensiveCollision(cam);
            else if (IsSpecialOffensive())
                RenderSpecialOffensiveCollision(cam);
            else if (IsCatch())
                RenderCatchCollision(cam);
        }

        #region Offensive Collision
        private unsafe void RenderOffensiveCollision(Vector3 cam)
        {
            MovesetNode node = Root;
            ResourceNode[] bl = _model._linker.BoneCache;

            int boneindex = (int)_parameters[0] >> 16;
            int size = HitboxSize;
            int angle = _parameters[2];

            node.GetBoneIndex(ref boneindex);

            if (boneindex == 0) //If a hitbox is on TopN, make it follow TransN
            {
                //Use assigned references
                if (node.Data != null)
                {
                    boneindex = node.Data._misc._boneRefs[4].boneIndex;
                    node.GetBoneIndex(ref boneindex);
                }
                else //Search manually
                {
                    int transindex = 0;
                    foreach (MDL0BoneNode bn in bl)
                    {
                        if (bn.Name.Equals("TransN")) break;
                        transindex++;
                    }
                    if (transindex != bl.Length)
                        boneindex = transindex;
                }
            }

            MDL0BoneNode b;
            b = bl[boneindex] as MDL0BoneNode;

            Matrix r = b.Matrix.GetRotationMatrix();
            FrameState state = b.Matrix.Derive();
            Vector3 bonePos = state._translate;
            Vector3 globalPos = r.Multiply(new Vector3(Util.UnScalar(_parameters[6]), Util.UnScalar(_parameters[7]), Util.UnScalar(_parameters[8])) / state._scale);

            Matrix m = Matrix.TransformMatrix(new Vector3(1), state._rotate, globalPos + bonePos);
            Vector3 resultPos = m.GetPoint();

            int id = (int)_parameters[0] & 0xFFFF;
            RunTime.MainWindow.ModelPanel.CurrentViewport.ScreenText[id.ToString()] = 
                RunTime.MainWindow.ModelPanel.CurrentViewport.Camera.Project(resultPos);

            m = Matrix.TransformMatrix(new Vector3(Util.UnScalar(size)), new Vector3(), resultPos);
            GL.PushMatrix();
            GL.MultMatrix((float*)&m);
            int res = 16;
            double drawAngle = 360.0 / res;

            Vector3 color = Util.GetTypeColor(flags.Type);
            GL.Color4((color._x / 255.0f), (color._y / 225.0f), (color._z / 255.0f), 0.5f);

            GLDisplayList spheres = TKContext.GetSphereList();
            spheres.Call();

            //Angle indicator
            double rangle = angle / 180.0 * Math.PI;

            //Apply color
            color = Util.GetEffectColor(flags.Effect);
            GL.Color4((color._x / 255.0f), (color._y / 225.0f), (color._z / 255.0f), 0.75f);
            
            GL.PushMatrix();
            if (angle == 361) //Sakurai angle
            {
                m = Matrix.TransformMatrix(new Vector3(0.5f), (globalPos + bonePos).LookatAngles(cam) * Maths._rad2degf, new Vector3(0));
                GL.MultMatrix((float*)&m);
                GL.Begin(BeginMode.Quads);
                for (int i = 0; i < 16; i += 2)
                {
                    GL.Vertex3(Math.Cos((i - 1) * Math.PI / 8) * 0.5, Math.Sin((i - 1) * Math.PI / 8) * 0.5, 0);
                    GL.Vertex3(Math.Cos(i * Math.PI / 8), Math.Sin(i * Math.PI / 8), 0);
                    GL.Vertex3(Math.Cos((i + 1) * Math.PI / 8) * 0.5, Math.Sin((i + 1) * Math.PI / 8) * 0.5, 0);
                    GL.Vertex3(0, 0, 0);
                }
                GL.End();
            }
            else
            {
                long a = -angle; //Otherwise 90 would point down
                int angleflip = 0;
                if (resultPos._z < 0)
                    angleflip = 180;
                m = Matrix.TransformMatrix(new Vector3(1), new Vector3(a, angleflip, 0), new Vector3());
                GL.MultMatrix((float*)&m);
                GL.Begin(BeginMode.Quads);
                // left face
                GL.Vertex3(0.1, 0.1, 0);
                GL.Vertex3(0.1, 0.1, 1);
                GL.Vertex3(0.1, -0.1, 1);
                GL.Vertex3(0.1, -0.1, 0);
                // right face
                GL.Vertex3(-0.1, -0.1, 0);
                GL.Vertex3(-0.1, -0.1, 1);
                GL.Vertex3(-0.1, 0.1, 1);
                GL.Vertex3(-0.1, 0.1, 0);
                // top face
                GL.Vertex3(-0.1, 0.1, 0);
                GL.Vertex3(-0.1, 0.1, 1);
                GL.Vertex3(0.1, 0.1, 1);
                GL.Vertex3(0.1, 0.1, 0);
                // bottom face
                GL.Vertex3(0.1, -0.1, 0);
                GL.Vertex3(0.1, -0.1, 1);
                GL.Vertex3(-0.1, -0.1, 1);
                GL.Vertex3(-0.1, -0.1, 0);
                // front face
                GL.Vertex3(-0.1, -0.1, 1);
                GL.Vertex3(0.1, -0.1, 1);
                GL.Vertex3(0.1, 0.1, 1);
                GL.Vertex3(-0.1, 0.1, 1);
                // back face
                GL.Vertex3(-0.1, 0.1, 0);
                GL.Vertex3(0.1, 0.1, 0);
                GL.Vertex3(0.1, -0.1, 0);
                GL.Vertex3(-0.1, -0.1, 0);
                GL.End();
            }
            GL.PopMatrix();

            // border
            GLDisplayList rings = TKContext.GetRingList();
            for (int i = -5; i <= 5; i++)
            {
                GL.PushMatrix();
                m = Matrix.TransformMatrix(new Vector3(1 + 0.0025f * i), (globalPos + bonePos).LookatAngles(cam) * Maths._rad2degf, new Vector3());
                GL.MultMatrix((float*)&m);
                if (flags.Clang)
                    rings.Call();
                else
                {
                    for (double j = 0; j < 360 / (drawAngle / 2); j += 2)
                    {
                        double ang1 = (j * (drawAngle / 2)) / 180 * Math.PI;
                        double ang2 = ((j + 1) * (drawAngle / 2)) / 180 * Math.PI;
                        GL.Begin(BeginMode.LineStrip);
                        GL.Vertex3(Math.Cos(ang1), Math.Sin(ang1), 0);
                        GL.Vertex3(Math.Cos(ang2), Math.Sin(ang2), 0);
                        GL.End();
                    }
                }
                GL.PopMatrix();
            }
            
            GL.PopMatrix();
            GL.PopMatrix();
        }

        #endregion

        #region Special Offensive Collision
        private unsafe void RenderSpecialOffensiveCollision(Vector3 cam)
        {
            ResourceNode[] bl = _model._linker.BoneCache;

            int boneindex = (int)_parameters[0] >> 16;
            int size = HitboxSize;
            int angle = _parameters[2];

            Root.GetBoneIndex(ref boneindex);

            if (boneindex == 0) //If a hitbox is on TopN, make it follow TransN
            {
                if (Root.Data != null)
                {
                    boneindex = Root.Data._misc._boneRefs[4].boneIndex;
                    Root.GetBoneIndex(ref boneindex);
                }
                else
                {
                    int transindex = 0;
                    foreach (MDL0BoneNode bn in bl)
                    {
                        if (bn.Name.Equals("TransN")) break;
                        transindex++;
                    }
                    if (transindex != bl.Length)
                        boneindex = transindex;
                }
            }
            MDL0BoneNode b;
            b = bl[boneindex] as MDL0BoneNode;

            Matrix r = b.Matrix.GetRotationMatrix();
            FrameState state = b.Matrix.Derive();
            Vector3 bonePos = state._translate;
            Vector3 pos = new Vector3(Util.UnScalar(_parameters[6]), Util.UnScalar(_parameters[7]), Util.UnScalar(_parameters[8])) / state._scale;
            Vector3 globalPos = r.Multiply(pos);

            Matrix m = Matrix.TransformMatrix(new Vector3(1), state._rotate, globalPos + bonePos);
            Vector3 resultPos = m.GetPoint();

            int id = (int)_parameters[0] & 0xFFFF;
            RunTime.MainWindow.ModelPanel.CurrentViewport.ScreenText[id.ToString()] = RunTime.MainWindow.ModelPanel.CurrentViewport.Project(resultPos);

            m = Matrix.TransformMatrix(new Vector3(Util.UnScalar(size)), new Vector3(), resultPos);
            GL.PushMatrix();
            GL.MultMatrix((float*)&m);
            int res = 16;
            double drawangle = 360.0 / res;

            Vector3 color = Util.GetTypeColor(flags.Type);
            GL.Color4((color._x / 255.0f), (color._y / 225.0f), (color._z / 255.0f), 0.5f);

            GLDisplayList spheres = TKContext.GetSphereList();
            spheres.Call();
            if (specialFlags.Stretches)
            {
                GL.PushMatrix();
                m = Matrix.TransformMatrix(new Vector3(1), state._rotate, new Vector3());
                GL.MultMatrix((float*)&m);
                Vector3 reversepos = new Vector3(-pos._x / Util.UnScalar(size), -pos._y / Util.UnScalar(size), -pos._z / Util.UnScalar(size));

                color = Util.GetEffectColor(flags.Effect);
                GL.Color4((color._x / 255.0f), (color._y / 225.0f), (color._z / 255.0f), 0.5f);
                
                GL.Translate(reversepos._x, reversepos._y, reversepos._z);
                GL.Begin(BeginMode.Lines); // stretch lines
                GL.Vertex3(1, 0, 0);
                GL.Vertex3(1 - reversepos._x, 0 - reversepos._y, 0 - reversepos._z);
                GL.Vertex3(-1, 0, 0);
                GL.Vertex3(-1 - reversepos._x, 0 - reversepos._y, 0 - reversepos._z);
                GL.Vertex3(0, 1, 0);
                GL.Vertex3(0 - reversepos._x, 1 - reversepos._y, 0 - reversepos._z);
                GL.Vertex3(0, -1, 0);
                GL.Vertex3(0 - reversepos._x, -1 - reversepos._y, 0 - reversepos._z);
                GL.Vertex3(0, 0, 1);
                GL.Vertex3(0 - reversepos._x, 0 - reversepos._y, 1 - reversepos._z);
                GL.Vertex3(0, 0, -1);
                GL.Vertex3(0 - reversepos._x, 0 - reversepos._y, -1 - reversepos._z);
                GL.End();

                color = Util.GetTypeColor(flags.Type);
                GL.Color4((color._x / 255.0f), (color._y / 225.0f), (color._z / 255.0f), 0.25f);
                
                spheres.Call(); // root sphere
                GL.Translate(-reversepos._x, -reversepos._y, -reversepos._z);
                GL.PopMatrix();
            }

            // angle indicator
            double rangle = angle / 180.0 * Math.PI;
            Vector3 effectcolour = Util.GetEffectColor(flags.Effect);
            GL.Color4((effectcolour._x / 255.0f), (effectcolour._y / 225.0f), (effectcolour._z / 255.0f), 0.75f);
            GL.PushMatrix();
            if (angle == 361)
            {
                m = Matrix.TransformMatrix(new Vector3(0.5f), (globalPos + bonePos).LookatAngles(cam) * Maths._rad2degf, new Vector3(0));
                GL.MultMatrix((float*)&m);
                GL.Begin(BeginMode.Quads);
                for (int i = 0; i < 16; i += 2)
                {
                    GL.Vertex3(Math.Cos((i - 1) * Math.PI / 8) * 0.5, Math.Sin((i - 1) * Math.PI / 8) * 0.5, 0);
                    GL.Vertex3(Math.Cos(i * Math.PI / 8), Math.Sin(i * Math.PI / 8), 0);
                    GL.Vertex3(Math.Cos((i + 1) * Math.PI / 8) * 0.5, Math.Sin((i + 1) * Math.PI / 8) * 0.5, 0);
                    GL.Vertex3(0, 0, 0);
                }
                GL.End();
            }
            else
            {
                long a = -angle; // otherwise 90 would point down
                int angleflip = 0;
                if (resultPos._z < 0)
                    angleflip = 180;
                m = Matrix.TransformMatrix(new Vector3(1), new Vector3(a, angleflip, 0), new Vector3());
                GL.MultMatrix((float*)&m);
                GL.Begin(BeginMode.Quads);
                // left face
                GL.Vertex3(0.1, 0.1, 0);
                GL.Vertex3(0.1, 0.1, 1);
                GL.Vertex3(0.1, -0.1, 1);
                GL.Vertex3(0.1, -0.1, 0);
                // right face
                GL.Vertex3(-0.1, -0.1, 0);
                GL.Vertex3(-0.1, -0.1, 1);
                GL.Vertex3(-0.1, 0.1, 1);
                GL.Vertex3(-0.1, 0.1, 0);
                // top face
                GL.Vertex3(-0.1, 0.1, 0);
                GL.Vertex3(-0.1, 0.1, 1);
                GL.Vertex3(0.1, 0.1, 1);
                GL.Vertex3(0.1, 0.1, 0);
                // bottom face
                GL.Vertex3(0.1, -0.1, 0);
                GL.Vertex3(0.1, -0.1, 1);
                GL.Vertex3(-0.1, -0.1, 1);
                GL.Vertex3(-0.1, -0.1, 0);
                // front face
                GL.Vertex3(-0.1, -0.1, 1);
                GL.Vertex3(0.1, -0.1, 1);
                GL.Vertex3(0.1, 0.1, 1);
                GL.Vertex3(-0.1, 0.1, 1);
                // back face
                GL.Vertex3(-0.1, 0.1, 0);
                GL.Vertex3(0.1, 0.1, 0);
                GL.Vertex3(0.1, -0.1, 0);
                GL.Vertex3(-0.1, -0.1, 0);
                GL.End();
            }
            GL.PopMatrix();

            // border
            GLDisplayList rings = TKContext.GetRingList();
            for (int i = -5; i <= 5; i++)
            {
                GL.PushMatrix();
                m = Matrix.TransformMatrix(new Vector3(1 + 0.0025f * i), (globalPos + bonePos).LookatAngles(cam) * Maths._rad2degf, new Vector3());
                GL.MultMatrix((float*)&m);
                if (flags.Clang)
                    rings.Call();
                else
                {
                    for (double j = 0; j < 360 / (drawangle / 2); j += 2)
                    {
                        double ang1 = (j * (drawangle / 2)) / 180 * Math.PI;
                        double ang2 = ((j + 1) * (drawangle / 2)) / 180 * Math.PI;
                        int q = 0;
                        GL.Begin(BeginMode.LineStrip);
                        GL.Vertex3(Math.Cos(ang1), Math.Sin(ang1), 0);
                        GL.Vertex3(Math.Cos(ang2), Math.Sin(ang2), 0);
                        GL.End();
                    }
                }
                GL.PopMatrix();
            }
            
            GL.PopMatrix();
            GL.PopMatrix();
        }
        #endregion

        #region Catch Collision
        private unsafe void RenderCatchCollision(Vector3 cam)
        {
            ResourceNode[] bl = _model._linker.BoneCache;

            int boneindex = _parameters[1];
            int size = HitboxSize;

            Root.GetBoneIndex(ref boneindex);

            if (boneindex == 0) // if a hitbox is on TopN, make it follow TransN
            {
                if (Root.Data != null)
                {
                    boneindex = Root.Data._misc._boneRefs[4].boneIndex;
                    Root.GetBoneIndex(ref boneindex);
                }
                else
                {
                    int transindex = 0;
                    foreach (MDL0BoneNode bn in bl)
                    {
                        if (bn.Name.Equals("TransN")) break;
                        transindex++;
                    }
                    if (transindex != bl.Length)
                        boneindex = transindex;
                }
            }
            MDL0BoneNode b = bl[boneindex] as MDL0BoneNode;

            Matrix r = b.Matrix.GetRotationMatrix();
            FrameState state = b.Matrix.Derive();
            Vector3 bonePos = state._translate;
            Vector3 pos = new Vector3(Util.UnScalar(_parameters[3]), Util.UnScalar(_parameters[4]), Util.UnScalar(_parameters[5])) / state._scale;
            Vector3 globalPos = r.Multiply(pos);

            Matrix m = Matrix.TransformMatrix(new Vector3(1), state._rotate, globalPos + bonePos);
            Vector3 resultPos = m.GetPoint();

            m = Matrix.TransformMatrix(new Vector3(Util.UnScalar(size)), new Vector3(), resultPos);
            GL.PushMatrix();
            GL.MultMatrix((float*)&m);
            int res = 16;
            double drawangle = 360.0 / res;

            Vector3 color = Util.GetTypeColor(Util.HitboxType.Throwing);
            GL.Color4((color._x / 255.0f), (color._y / 225.0f), (color._z / 255.0f), 0.375f);
            GLDisplayList spheres = TKContext.GetSphereList();
            spheres.Call();
            
            GL.PopMatrix();
        }
        #endregion

        #region General Collision
        //public unsafe virtual void RenderGeneralCollision(List<MDL0BoneNode> bl, GLContext c, Vector3 cam, DrawStyle style)
        //{
        //    MDL0BoneNode b = bl[0];
        //    Vector3 bonepos = b._frameMatrix.GetPoint();
        //    Vector3 pos = new Vector3(intToScalar(getXPos()), intToScalar(getYPos()), intToScalar(getZPos()));
        //    Vector3 bonerot = b._frameMatrix.GetAngles();
        //    Matrix r = b._frameMatrix.GetRotationMatrix();
        //    Vector3 globpos = r.Multiply(pos);
        //    Matrix m = Matrix.TransformMatrix(new Vector3(1), bonerot, globpos + bonepos);
        //    Vector3 result = new Vector3(m[12], m[13], m[14]);
        //    m = Matrix.TransformMatrix(new Vector3(intToScalar(getSize())), new Vector3(), result);
        //    GL.PushMatrix();
        //    GL.MultMatrix((float*)&m);
        //    int res = 16;
        //    double drawangle = 360.0 / res;
        //    // bubble
        //    Vector3 typecolour = new Vector3(0x7f, 0x7f, 0x7f);
        //    GL.Color4((typecolour._x / 255.0f), (typecolour._y / 225.0f), (typecolour._z / 255.0f), 0.375f);
        //    if (style == DrawStyle.SSB64)
        //    {
        //        GL.Color4(1.0f, 1.0f, 1.0f, 0.25f);
        //        c.DrawInvertedCube(new Vector3(0, 0, 0), 1.025f);
        //        GL.Color4(0.5f, 0.5f, 0.5f, 0.5f);
        //        c.DrawCube(new Vector3(0, 0, 0), 0.975f);
        //    }
        //    else
        //    {
        //        GLDisplayList spheres = c.GetSphereList();
        //        spheres.Call();
        //    }
        //    GL.PopMatrix();
        //}
        #endregion
    }
}
