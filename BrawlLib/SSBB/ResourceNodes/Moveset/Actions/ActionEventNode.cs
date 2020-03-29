using BrawlLib.Internal;
using BrawlLib.SSBB.Types;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class MoveDefEventNode : MoveDefEntryNode
    {
        internal FDefEvent* Header => (FDefEvent*) WorkingUncompressed.Address;
        internal FDefEventArgument* ArgumentHeader => (FDefEventArgument*) (BaseAddress + Header->_argumentOffset);

        internal byte nameSpace, id, numArguments, unk1;
        internal List<FDefEventArgument> arguments = new List<FDefEventArgument>();

        public override ResourceType ResourceFileType => ResourceType.Event;

        public override int OnCalculateSize(bool force)
        {
            int size = 8;
            _lookupCount = Children.Count > 0 ? 1 : 0;
            foreach (MoveDefEventParameterNode p in Children)
            {
                size += p.CalculateSize(true);
                _lookupCount += p._lookupCount;
            }

            return size;
        }

        [Browsable(false)]
        public ActionEventInfo EventInfo
        {
            get
            {
                if (MoveDefNode.EventDictionary == null)
                {
                    MoveDefNode.LoadEventDictionary();
                }

                if (MoveDefNode.EventDictionary.ContainsKey(_event))
                {
                    return MoveDefNode.EventDictionary[_event];
                }

                return null;
            }
        }

        public uint _event;

        [Browsable(false)]
        public uint EventID
        {
            get => _event;
            set
            {
                _event = value;
                string ev = Helpers.Hex8(_event);
                nameSpace = byte.Parse(ev.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
                id = byte.Parse(ev.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
                numArguments = byte.Parse(ev.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
                unk1 = byte.Parse(ev.Substring(6, 2), System.Globalization.NumberStyles.HexNumber);
                if (MoveDefNode.EventDictionary.ContainsKey(_event))
                {
                    Name = MoveDefNode.EventDictionary[_event]._name;
                }
                else
                {
                    Name = ev;
                }
            }
        }

        [Browsable(false)]
        public Event EventData
        {
            get
            {
                Event e = new Event();
                e.SetEventEvent(_event);
                e.pParameters = ArgumentOffset;
                int i = 0;
                foreach (ResourceNode r in Children)
                {
                    if (r is MoveDefEventParameterNode)
                    {
                        e.parameters[i]._type = (r as MoveDefEventParameterNode)._type;
                        e.parameters[i++]._data = (r as MoveDefEventParameterNode)._value;
                    }
                }

                return e;
            }
        }

        public string Serialize()
        {
            string s = "";
            s += Helpers.Hex8(EventID) + "|";
            foreach (MoveDefEventParameterNode p in Children)
            {
                s += (int) p._type + "\\";
                if (p._type == ArgVarType.Offset)
                {
                    MoveDefEventOffsetNode o = p as MoveDefEventOffsetNode;
                    s += o.list + "," + o.type + "," + o.index;
                }
                else
                {
                    s += p._value;
                }

                s += "|";
            }

            return s;
        }

        public static MoveDefEventNode Deserialize(string s, MoveDefNode node)
        {
            if (string.IsNullOrEmpty(s))
            {
                return null;
            }

            try
            {
                string[] lines = s.Split('|');

                if (lines[0].Length != 8)
                {
                    return null;
                }

                MoveDefEventNode newEv = new MoveDefEventNode {_parent = node};

                string id = lines[0];
                uint idNumber = Convert.ToUInt32(id, 16);

                newEv.EventID = idNumber;

                uint _event = newEv.EventID;
                ActionEventInfo info = newEv.EventInfo;

                for (int i = 0; i < newEv.numArguments; i++)
                {
                    string[] pLines = lines[i + 1].Split('\\');

                    int type = int.Parse(pLines[0]);

                    if (type == 2) //Offset
                    {
                        string[] oLines = pLines[1].Split(',');
                        int list = int.Parse(oLines[0]), type2 = int.Parse(oLines[1]), index = int.Parse(oLines[2]);
                        MoveDefEventOffsetNode o = (MoveDefEventOffsetNode) newEv.NewParam(i, 0, 2);
                        o.action = node.GetAction(list, type2, index);
                        o.list = list;
                        o.type = type2;
                        o.index = index;
                    }
                    else
                    {
                        int value = int.Parse(pLines[1]);
                        newEv.NewParam(i, value, type);
                    }
                }

                newEv._parent = null;
                return newEv;
            }
            catch
            {
                return null;
            }
        }

        public void NewChildren()
        {
            while (Children.Count > 0)
            {
                RemoveChild(Children[0]);
            }

            for (int i = 0; i < numArguments; i++)
            {
                NewParam(i, 0, -1);
            }
        }

        public MoveDefEntryNode NewParam(int i, int value, int typeOverride)
        {
            MoveDefEntryNode child = null;
            ActionEventInfo info = EventInfo;
            ArgVarType type = ArgVarType.Value;
            if (typeOverride >= 0)
            {
                type = (ArgVarType) typeOverride;
            }
            else if (info != null)
            {
                type = (ArgVarType) info.GetDfltParameter(i);
            }

            if ((ArgVarType) (int) type == ArgVarType.Value)
            {
                if ((_event == 0x06000D00 || _event == 0x06150F00 || _event == 0x062B0D00) && i == 12)
                {
                    child = new HitboxFlagsNode(info != null && i < info.Params.Length ? info.Params[i] : "Value")
                        {_value = value, val = new HitboxFlags {data = value}};
                    (child as HitboxFlagsNode).GetFlags();
                }
                else if ((_event == 0x06000D00 || _event == 0x06150F00 || _event == 0x062B0D00) &&
                         (i == 0 || i == 3 || i == 4))
                {
                    child = new MoveDefEventValue2HalfNode(info != null && i < info.Params.Length
                        ? info.Params[i]
                        : "Value") {_value = value};
                }
                else if ((_event == 0x11150300 || _event == 0x11001000 || _event == 0x11010A00 ||
                          _event == 0x11020A00) && i == 0)
                {
                    child = new MoveDefEventValue2HalfGFXNode(info != null && i < info.Params.Length
                        ? info.Params[i]
                        : "Value") {_value = value};
                }
                else if (i == 14 && _event == 0x06150F00)
                {
                    child =
                        new SpecialHitboxFlagsNode(info != null && i < info.Params.Length ? info.Params[i] : "Value")
                            {_value = value, val = new SpecialHitboxFlags {data = value}};
                    (child as SpecialHitboxFlagsNode).GetFlags();
                }
                else //Not a special value
                {
                    if (EventInfo?.Enums != null && EventInfo.Enums.ContainsKey(i))
                    {
                        child = new MoveDefEventValueEnumNode(info != null && i < info.Params.Length
                            ? info.Params[i]
                            : "Value") {Enums = EventInfo.Enums[i].ToArray()};
                    }
                    else
                    {
                        child = new MoveDefEventValueNode(info != null && i < info.Params.Length
                            ? info.Params[i]
                            : "Value") {_value = value};
                    }
                }
            }
            else if ((ArgVarType) (int) type == ArgVarType.Scalar)
            {
                child = new MoveDefEventScalarNode(info != null && i < info.Params.Length ? info.Params[i] : "Scalar")
                    {_value = value};
            }
            else if ((ArgVarType) (int) type == ArgVarType.Boolean)
            {
                child = new MoveDefEventBoolNode(info != null && i < info.Params.Length ? info.Params[i] : "Boolean")
                    {_value = value};
            }
            else if ((ArgVarType) (int) type == ArgVarType.Unknown)
            {
                child = new MoveDefEventUnkNode(info != null && i < info.Params.Length ? info.Params[i] : "Unknown")
                    {_value = value};
            }
            else if ((ArgVarType) (int) type == ArgVarType.Requirement)
            {
                MoveDefEventRequirementNode r =
                    new MoveDefEventRequirementNode(info != null && i < info.Params.Length
                        ? info.Params[i]
                        : "Requirement") {_value = value};
                child = r;
                r._parent = Root;
                r.val = r.GetRequirement(r._value);
            }
            else if ((ArgVarType) (int) type == ArgVarType.Variable)
            {
                MoveDefEventVariableNode v =
                    new MoveDefEventVariableNode(info != null && i < info.Params.Length ? info.Params[i] : "Variable")
                        {_value = value};
                child = v;
                v._parent = Root;
                v.val = v.ResolveVariable(v._value);
            }
            else if ((ArgVarType) (int) type == ArgVarType.Offset)
            {
                child = new MoveDefEventOffsetNode(info != null && i < info.Params.Length ? info.Params[i] : "Offset")
                    {_value = value};
            }

            child._parent = null;
            if (i == Children.Count)
            {
                AddChild(child);
            }
            else
            {
                InsertChild(child, true, i);
            }

            return child;
        }

        [Category("MoveDef Event")]
        public byte NameSpace //set { nameSpace = value; SignalPropertyChange(); } }
            =>
                nameSpace;

        [Category("MoveDef Event")]
        public byte ID //set { id = value; SignalPropertyChange(); } }
            =>
                id;

        [Category("MoveDef Event")]
        public byte NumArguments //set { numArguments = value; SignalPropertyChange(); } }
            =>
                numArguments;

        [Category("MoveDef Event")]
        public byte Unknown
        {
            get => unk1;
            set
            {
                unk1 = value;
                SignalPropertyChange();
            }
        }

        [Category("MoveDef Event")] public uint ArgumentOffset => argOffset;

        public uint argOffset;

        [Category("MoveDef Event Argument")]
        public ArgVarType[] Type
        {
            get
            {
                IEnumerable<ArgVarType> array = from x in arguments select (ArgVarType) (int) x._type;
                return array.ToArray();
            }
        }

        [Category("MoveDef Event Argument")]
        public int[] Value
        {
            get
            {
                IEnumerable<int> array = from x in arguments select (int) x._data;
                return array.ToArray();
            }
        }

        [Browsable(false)]
        public List<FDefEventArgument> Arguments
        {
            get => arguments;
            set => arguments = value;
        }

        public override bool OnInitialize()
        {
            if ((int) Header == (int) BaseAddress)
            {
                return false;
            }

            argOffset = Header->_argumentOffset;

            nameSpace = Header->_nameSpace;
            id = Header->_id;
            numArguments = Header->_numArguments;
            unk1 = Header->_unk1;

            //Merge values to create ID and match with events to get name
            _event = uint.Parse($"{nameSpace:X02}{id:X02}{numArguments:X02}{unk1:X02}",
                System.Globalization.NumberStyles.HexNumber);
            if (MoveDefNode.EventDictionary.ContainsKey(_event))
            {
                _name = MoveDefNode.EventDictionary[_event]._name;
            }
            else
            {
                if (unk1 > 0)
                {
                    uint temp = uint.Parse(
                        $"{nameSpace:X02}{id:X02}{numArguments:X02}{0:X02}",
                        System.Globalization.NumberStyles.HexNumber);
                    if (MoveDefNode.EventDictionary.ContainsKey(temp))
                    {
                        _name = MoveDefNode.EventDictionary[temp]._name + " (Unknown == " + unk1 + ")";
                        _event = temp;
                    }
                    else
                    {
                        _name = Helpers.Hex8(_event);
                    }
                }
                else
                {
                    _name = Helpers.Hex8(_event);
                }
            }

            _extOverride = Index == 0;
            base.OnInitialize();

            if (!Root._events.ContainsKey(_event))
            {
                Root._events.Add(_event, new List<MoveDefEventNode> {this});
            }
            else
            {
                Root._events[_event].Add(this);
            }

            if (_name == "FADEF00D" || _name == "FADE0D8A")
            {
                Remove();
                return false;
            }

            for (int i = 0; i < numArguments; i++)
            {
                FDefEventArgument e;
                FDefEventArgument* header = &ArgumentHeader[i];
                arguments.Add(e = *header);

                string param = null;
                if (EventInfo?.Params != null && EventInfo.Params.Length != 0 && EventInfo.Params.Length > i)
                {
                    param = string.IsNullOrEmpty(EventInfo.Params[i]) ? null : EventInfo.Params[i];
                }

                if ((_event == 0x06000D00 || _event == 0x06150F00 || _event == 0x062B0D00) && i == 12)
                {
                    new HitboxFlagsNode(param).Initialize(this, header, 8);
                }
                else if ((_event == 0x06000D00 || _event == 0x06150F00 || _event == 0x062B0D00) &&
                         (i == 0 || i == 3 || i == 4))
                {
                    new MoveDefEventValue2HalfNode(param).Initialize(this, header, 8);
                }
                else if ((_event == 0x11150300 || _event == 0x11001000 || _event == 0x11010A00 ||
                          _event == 0x11020A00) && i == 0)
                {
                    new MoveDefEventValue2HalfGFXNode(param).Initialize(this, header, 8);
                }
                else if (i == 14 && _event == 0x06150F00)
                {
                    new SpecialHitboxFlagsNode(param).Initialize(this, header, 8);
                }
                else if ((ArgVarType) (int) e._type == ArgVarType.Value)
                {
                    if (EventInfo?.Enums != null && EventInfo.Enums.ContainsKey(i))
                    {
                        new MoveDefEventValueEnumNode(param) {Enums = EventInfo.Enums[i].ToArray()}.Initialize(this,
                            header, 8);
                    }
                    else
                    {
                        new MoveDefEventValueNode(param).Initialize(this, header, 8);
                    }
                }
                else if ((ArgVarType) (int) e._type == ArgVarType.Unknown)
                {
                    new MoveDefEventUnkNode(param).Initialize(this, header, 8);
                }
                else if ((ArgVarType) (int) e._type == ArgVarType.Scalar)
                {
                    new MoveDefEventScalarNode(param).Initialize(this, header, 8);
                }
                else if ((ArgVarType) (int) e._type == ArgVarType.Boolean)
                {
                    new MoveDefEventBoolNode(param).Initialize(this, header, 8);
                }
                else if ((ArgVarType) (int) e._type == ArgVarType.Requirement)
                {
                    new MoveDefEventRequirementNode(param).Initialize(this, header, 8);
                }
                else if ((ArgVarType) (int) e._type == ArgVarType.Variable)
                {
                    new MoveDefEventVariableNode(param).Initialize(this, header, 8);
                }
                else if ((ArgVarType) (int) e._type == ArgVarType.Offset)
                {
                    int offset = -1;
                    MoveDefExternalNode ext;
                    int paramOffset = e._data;

                    if (paramOffset == -1)
                    {
                        ext = Root.IsExternal((int) ArgumentOffset + i * 8 + 4);
                    }
                    else
                    {
                        ext = Root.IsExternal(paramOffset);
                    }

                    if (ext == null)
                    {
                        offset = e._data;
                    }

                    if (offset > 0)
                    {
                        MoveDefActionNode a;
                        int list, index, type;
                        Root.GetLocation(offset, out list, out type, out index);

                        if (list == 4) //Offset not found in existing nodes
                        {
                            Root._subRoutines[offset] =
                                a = new MoveDefActionNode("SubRoutine" + Root._subRoutineList.Count, false, null);
                            a.Initialize(Root._subRoutineGroup, new DataSource((sbyte*) BaseAddress + offset, 8));
                            //if (offset != (Parent as MoveDefEntryNode)._offset)
                            //    a.Populate();
                            a._actionRefs.Add(this);
                        }
                        else
                        {
                            MoveDefActionNode n = Root.GetAction(list, type, index);
                            n?._actionRefs.Add(this);
                        }
                    }

                    //Add ID node
                    if (ext != null)
                    {
                        MoveDefEventOffsetNode x = new MoveDefEventOffsetNode(param)
                            {_name = ext.Name, _extNode = ext, _extOverride = true};
                        x.Initialize(this, header, 8);
                        ext._refs.Add(x);
                    }
                    else
                    {
                        new MoveDefEventOffsetNode(param).Initialize(this, header, 8);
                    }
                }
            }

            return arguments.Count > 0;
        }

        public override string ToString()
        {
            if (Children.Count > 0 && (Children[0] is MoveDefEventOffsetNode ||
                                       EventID == 0x0D000200 && Children[1] is MoveDefEventOffsetNode))
            {
                return TreePath;
            }

            return base.ToString();
        }

        public override void Remove()
        {
            foreach (MoveDefEventParameterNode p in Children)
            {
                if (p.External)
                {
                    p._extNode._refs.Remove(p);
                }
            }

            base.Remove();
        }
    }

    public class HitBox
    {
        //A separate class for rendering hitboxes so that the values can be modified by other events.
        public HitBox(MoveDefEventNode ev)
        {
            Root = ev.Root;
            _event = ev._event;
            EventData = ev.EventData;
            if (_event != 101320704)
            {
                flags = ev.Children[12] as HitboxFlagsNode;
            }

            if (_event == 0x06150F00)
            {
                specialFlags = ev.Children[14] as SpecialHitboxFlagsNode;
            }
        }

        public MoveDefNode Root;
        public int HitboxID = -1, HitboxSize = 0;
        public uint _event;
        public Event EventData;
        public HitboxFlagsNode flags;
        public SpecialHitboxFlagsNode specialFlags;

        /*
        #region Offensive Collision

        public unsafe void RenderOffensiveCollision(ResourceNode[] bl, TKContext c, Vector3 cam,
                                                    Helpers.DrawStyle style)
        {
            //Coded by Toomai
            //Modified for release v0.67

            if (_event != 0x06000D00) //Offensive Collision
            {
                return;
            }

            Event e = EventData;
            //HitboxFlagsNode flags = Children[12] as HitboxFlagsNode;

            int boneindex = (int) e.parameters[0]._data >> 16;
            long size = HitboxSize;
            long angle = e.parameters[2]._data;

            Root.GetBoneIndex(ref boneindex);

            if (boneindex == 0) // if a hitbox is on TopN, make it follow TransN
            {
                if (Root.data != null)
                {
                    boneindex = (Root.data.misc.boneRefs.Children[4] as MoveDefBoneIndexNode).boneIndex;
                    Root.GetBoneIndex(ref boneindex);
                }
                else
                {
                    int transindex = 0;
                    foreach (MDL0BoneNode bn in bl)
                    {
                        if (bn.Name.Equals("TransN"))
                        {
                            break;
                        }

                        transindex++;
                    }

                    if (transindex != bl.Length)
                    {
                        boneindex = transindex;
                    }
                }
            }

            MDL0BoneNode b;
            b = bl[boneindex] as MDL0BoneNode;
            Vector3 bonepos = b._frameMatrix.GetPoint();
            Vector3 pos = new Vector3(Helpers.UnScalar(e.parameters[6]._data), Helpers.UnScalar(e.parameters[7]._data),
                Helpers.UnScalar(e.parameters[8]._data));
            Vector3 bonerot = b._frameMatrix.GetAngles();
            Matrix r = b._frameMatrix.GetRotationMatrix();
            Vector3 bonescl = b.RecursiveScale();
            pos._x /= bonescl._x;
            pos._y /= bonescl._y;
            pos._z /= bonescl._z;
            Vector3 globpos = r.Multiply(pos);
            Matrix m = Matrix.TransformMatrix(new Vector3(1), bonerot, globpos + bonepos);
            Vector3 resultpos = new Vector3(m[12], m[13], m[14]);
            m = Matrix.TransformMatrix(new Vector3(Helpers.UnScalar(size)), new Vector3(), resultpos);
            GL.PushMatrix();
            GL.MultMatrix((float*) &m);
            int res = 16;
            double drawangle = 360.0 / res;
            // bubble
            if (style == Helpers.DrawStyle.SSB64)
            {
                GL.Color4(1.0f, 1.0f, 1.0f, 0.25f);
                c.DrawInvertedCube(new Vector3(0, 0, 0), 1.025f);
                GL.Color4(1.0f, 0.0f, 0.0f, 0.5f);
                c.DrawCube(new Vector3(0, 0, 0), 0.975f);
            }
            else
            {
                if (style == Helpers.DrawStyle.Melee)
                {
                    GL.Color4(1.0f, 0.0f, 0.0f, 0.5f);
                }
                else
                {
                    Vector3 typecolour = Helpers.getTypeColour(flags.Type);
                    GL.Color4(typecolour._x / 255.0f, typecolour._y / 225.0f, typecolour._z / 255.0f, 0.5f);
                }

                GLDisplayList spheres = c.GetSphereList();
                spheres.Call();
            }

            if (style == Helpers.DrawStyle.Brawl)
            {
                // angle indicator
                double rangle = angle / 180.0 * Math.PI;
                Vector3 effectcolour = Helpers.getEffectColour(flags.Effect);
                GL.Color4(effectcolour._x / 255.0f, effectcolour._y / 225.0f, effectcolour._z / 255.0f, 0.75f);
                GL.PushMatrix();
                if (angle == 361)
                {
                    m = Matrix.TransformMatrix(new Vector3(0.5f),
                        (globpos + bonepos).LookatAngles(cam) * Maths._rad2degf, new Vector3(0));
                    GL.MultMatrix((float*) &m);
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
                    if (resultpos._z < 0)
                    {
                        angleflip = 180;
                    }

                    m = Matrix.TransformMatrix(new Vector3(1), new Vector3(a, angleflip, 0), new Vector3());
                    GL.MultMatrix((float*) &m);
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
                GLDisplayList rings = c.GetRingList();
                for (int i = -5; i <= 5; i++)
                {
                    GL.PushMatrix();
                    m = Matrix.TransformMatrix(new Vector3(1 + 0.0025f * i),
                        (globpos + bonepos).LookatAngles(cam) * Maths._rad2degf, new Vector3());
                    GL.MultMatrix((float*) &m);
                    if (flags.Clang)
                    {
                        rings.Call();
                    }
                    else
                    {
                        for (double j = 0; j < 360 / (drawangle / 2); j += 2)
                        {
                            double ang1 = j * (drawangle / 2) / 180 * Math.PI;
                            double ang2 = (j + 1) * (drawangle / 2) / 180 * Math.PI;
                            int q = 0;
                            GL.Begin(BeginMode.LineStrip);
                            GL.Vertex3(Math.Cos(ang1), Math.Sin(ang1), 0);
                            GL.Vertex3(Math.Cos(ang2), Math.Sin(ang2), 0);
                            GL.End();
                        }
                    }

                    GL.PopMatrix();
                }
            }

            GL.PopMatrix();
            GL.PopMatrix();
        }

        #endregion

        #region Special Offensive Collision

        public unsafe void RenderSpecialOffensiveCollision(ResourceNode[] bl, TKContext c, Vector3 cam,
                                                           Helpers.DrawStyle style)
        {
            //Coded by Toomai
            //Modified for release v0.67

            if (_event != 0x06150F00) //Special Offensive Collision
            {
                return;
            }

            Event e = EventData;
            //HitboxFlagsNode flags = Children[12] as HitboxFlagsNode;
            //SpecialHitboxFlagsNode specialFlags = Children[14] as SpecialHitboxFlagsNode;

            int boneindex = (int) e.parameters[0]._data >> 16;
            long size = HitboxSize;
            long angle = e.parameters[2]._data;

            Root.GetBoneIndex(ref boneindex);

            if (boneindex == 0) // if a hitbox is on TopN, make it follow TransN
            {
                if (Root.data != null)
                {
                    boneindex = (Root.data.misc.boneRefs.Children[4] as MoveDefBoneIndexNode).boneIndex;
                    Root.GetBoneIndex(ref boneindex);
                }
                else
                {
                    int transindex = 0;
                    foreach (MDL0BoneNode bn in bl)
                    {
                        if (bn.Name.Equals("TransN"))
                        {
                            break;
                        }

                        transindex++;
                    }

                    if (transindex != bl.Length)
                    {
                        boneindex = transindex;
                    }
                }
            }

            MDL0BoneNode b;
            b = bl[boneindex] as MDL0BoneNode;
            Vector3 bonepos = b._frameMatrix.GetPoint();
            Vector3 pos = new Vector3(Helpers.UnScalar(e.parameters[6]._data), Helpers.UnScalar(e.parameters[7]._data),
                Helpers.UnScalar(e.parameters[8]._data));
            Vector3 bonerot = b._frameMatrix.GetAngles();
            Matrix r = b._frameMatrix.GetRotationMatrix();
            Vector3 bonescl = b.RecursiveScale();
            pos._x /= bonescl._x;
            pos._y /= bonescl._y;
            pos._z /= bonescl._z;
            Vector3 globpos = r.Multiply(pos);
            Matrix m = Matrix.TransformMatrix(new Vector3(1), bonerot, globpos + bonepos);
            Vector3 resultpos = new Vector3(m[12], m[13], m[14]);
            m = Matrix.TransformMatrix(new Vector3(Helpers.UnScalar(size)), new Vector3(), resultpos);
            GL.PushMatrix();
            GL.MultMatrix((float*) &m);
            int res = 16, stretchres = 10;
            double drawangle = 360.0 / res;
            // bubble
            if (style == Helpers.DrawStyle.SSB64)
            {
                GL.Color4(1.0f, 1.0f, 1.0f, 0.25f);
                c.DrawInvertedCube(new Vector3(0, 0, 0), 1.025f);
                GL.Color4(1.0f, 0.0f, 0.0f, 0.5f);
                c.DrawCube(new Vector3(0, 0, 0), 0.975f);
                if (specialFlags.Stretches)
                {
                    Vector3 reversepos = new Vector3(-globpos._x / Helpers.UnScalar(size),
                        -globpos._y / Helpers.UnScalar(size), -globpos._z / Helpers.UnScalar(size));
                    GL.Translate(reversepos._x, reversepos._y, reversepos._z);
                    GL.Color4(1.0f, 0.0f, 0.0f, 0.5f);
                    GL.Begin(BeginMode.Lines);
                    GL.Vertex3(-1, -1, -1); // stretch lines
                    GL.Vertex3(-1 - reversepos._x, -1 - reversepos._y, -1 - reversepos._z);
                    GL.Vertex3(-1, -1, 1);
                    GL.Vertex3(-1 - reversepos._x, -1 - reversepos._y, 1 - reversepos._z);
                    GL.Vertex3(-1, 1, -1);
                    GL.Vertex3(-1 - reversepos._x, 1 - reversepos._y, -1 - reversepos._z);
                    GL.Vertex3(-1, 1, 1);
                    GL.Vertex3(-1 - reversepos._x, 1 - reversepos._y, 1 - reversepos._z);
                    GL.Vertex3(1, -1, -1);
                    GL.Vertex3(1 - reversepos._x, -1 - reversepos._y, -1 - reversepos._z);
                    GL.Vertex3(1, -1, 1);
                    GL.Vertex3(1 - reversepos._x, -1 - reversepos._y, 1 - reversepos._z);
                    GL.Vertex3(1, 1, -1);
                    GL.Vertex3(1 - reversepos._x, 1 - reversepos._y, -1 - reversepos._z);
                    GL.Vertex3(1, 1, 1);
                    GL.Vertex3(1 - reversepos._x, 1 - reversepos._y, 1 - reversepos._z);
                    GL.End();
                    GL.Begin(BeginMode.LineLoop); // root box
                    GL.Vertex3(-1, -1, -1);
                    GL.Vertex3(-1, -1, 1);
                    GL.Vertex3(-1, 1, 1);
                    GL.Vertex3(-1, 1, -1);
                    GL.End();
                    GL.Begin(BeginMode.LineLoop);
                    GL.Vertex3(1, -1, -1);
                    GL.Vertex3(1, -1, 1);
                    GL.Vertex3(1, 1, 1);
                    GL.Vertex3(1, 1, -1);
                    GL.End();
                    GL.Begin(BeginMode.Lines);
                    GL.Vertex3(-1, -1, -1);
                    GL.Vertex3(1, -1, -1);
                    GL.Vertex3(-1, -1, 1);
                    GL.Vertex3(1, -1, 1);
                    GL.Vertex3(-1, 1, -1);
                    GL.Vertex3(1, 1, -1);
                    GL.Vertex3(-1, 1, 1);
                    GL.Vertex3(1, 1, 1);
                    GL.End();
                    GL.Translate(-reversepos._x, -reversepos._y, -reversepos._z);
                }
            }
            else
            {
                if (style == Helpers.DrawStyle.Melee)
                {
                    GL.Color4(1.0f, 0.0f, 0.0f, 0.5f);
                }
                else
                {
                    Vector3 typecolour = Helpers.getTypeColour(flags.Type);
                    GL.Color4(typecolour._x / 255.0f, typecolour._y / 225.0f, typecolour._z / 255.0f, 0.5f);
                }

                GLDisplayList spheres = c.GetSphereList();
                spheres.Call();
                if (specialFlags.Stretches)
                {
                    GL.PushMatrix();
                    m = Matrix.TransformMatrix(new Vector3(1), bonerot, new Vector3());
                    GL.MultMatrix((float*) &m);
                    Vector3 reversepos = new Vector3(-pos._x / Helpers.UnScalar(size), -pos._y / Helpers.UnScalar(size),
                        -pos._z / Helpers.UnScalar(size));
                    if (style == Helpers.DrawStyle.Melee)
                    {
                        GL.Color4(1.0f, 0.0f, 0.0f, 0.5f);
                    }
                    else
                    {
                        Vector3 effectcolour = Helpers.getEffectColour(flags.Effect);
                        GL.Color4(effectcolour._x / 255.0f, effectcolour._y / 225.0f, effectcolour._z / 255.0f, 0.5f);
                    }

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
                    if (style == Helpers.DrawStyle.Melee)
                    {
                        GL.Color4(1.0f, 0.0f, 0.0f, 0.25f);
                    }
                    else
                    {
                        Vector3 typecolour = Helpers.getTypeColour(flags.Type);
                        GL.Color4(typecolour._x / 255.0f, typecolour._y / 225.0f, typecolour._z / 255.0f, 0.25f);
                    }

                    spheres.Call(); // root sphere
                    GL.Translate(-reversepos._x, -reversepos._y, -reversepos._z);
                    GL.PopMatrix();
                }
            }

            if (style == Helpers.DrawStyle.Brawl)
            {
                // angle indicator
                double rangle = angle / 180.0 * Math.PI;
                Vector3 effectcolour = Helpers.getEffectColour(flags.Effect);
                GL.Color4(effectcolour._x / 255.0f, effectcolour._y / 225.0f, effectcolour._z / 255.0f, 0.75f);
                GL.PushMatrix();
                if (angle == 361)
                {
                    m = Matrix.TransformMatrix(new Vector3(0.5f),
                        (globpos + bonepos).LookatAngles(cam) * Maths._rad2degf, new Vector3(0));
                    GL.MultMatrix((float*) &m);
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
                    if (resultpos._z < 0)
                    {
                        angleflip = 180;
                    }

                    m = Matrix.TransformMatrix(new Vector3(1), new Vector3(a, angleflip, 0), new Vector3());
                    GL.MultMatrix((float*) &m);
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
                GLDisplayList rings = c.GetRingList();
                for (int i = -5; i <= 5; i++)
                {
                    GL.PushMatrix();
                    m = Matrix.TransformMatrix(new Vector3(1 + 0.0025f * i),
                        (globpos + bonepos).LookatAngles(cam) * Maths._rad2degf, new Vector3());
                    GL.MultMatrix((float*) &m);
                    if (flags.Clang)
                    {
                        rings.Call();
                    }
                    else
                    {
                        for (double j = 0; j < 360 / (drawangle / 2); j += 2)
                        {
                            double ang1 = j * (drawangle / 2) / 180 * Math.PI;
                            double ang2 = (j + 1) * (drawangle / 2) / 180 * Math.PI;
                            int q = 0;
                            GL.Begin(BeginMode.LineStrip);
                            GL.Vertex3(Math.Cos(ang1), Math.Sin(ang1), 0);
                            GL.Vertex3(Math.Cos(ang2), Math.Sin(ang2), 0);
                            GL.End();
                        }
                    }

                    GL.PopMatrix();
                }
            }

            GL.PopMatrix();
            GL.PopMatrix();
        }

        #endregion

        #region Catch Collision

        public unsafe void RenderCatchCollision(ResourceNode[] bl, TKContext c, Vector3 cam, Helpers.DrawStyle style)
        {
            //Coded by Toomai
            //Modified for release v0.67

            if (_event != 0x060A0800 && _event != 0x060A0900 && _event != 0x060A0A00)
            {
                return;
            }

            Event e = EventData;

            int boneindex = (int) e.parameters[1]._data;
            long size = HitboxSize;

            Root.GetBoneIndex(ref boneindex);

            if (boneindex == 0) // if a hitbox is on TopN, make it follow TransN
            {
                if (Root.data != null)
                {
                    boneindex = (Root.data.misc.boneRefs.Children[4] as MoveDefBoneIndexNode).boneIndex;
                    Root.GetBoneIndex(ref boneindex);
                }
                else
                {
                    int transindex = 0;
                    foreach (MDL0BoneNode bn in bl)
                    {
                        if (bn.Name.Equals("TransN"))
                        {
                            break;
                        }

                        transindex++;
                    }

                    if (transindex != bl.Length)
                    {
                        boneindex = transindex;
                    }
                }
            }

            MDL0BoneNode b = bl[boneindex] as MDL0BoneNode;
            Vector3 bonepos = b._frameMatrix.GetPoint();
            Vector3 pos = new Vector3(Helpers.UnScalar(e.parameters[3]._data), Helpers.UnScalar(e.parameters[4]._data),
                Helpers.UnScalar(e.parameters[5]._data));
            Vector3 bonerot = b._frameMatrix.GetAngles();
            Matrix r = b._frameMatrix.GetRotationMatrix();
            Vector3 bonescl = b.RecursiveScale();
            pos._x /= bonescl._x;
            pos._y /= bonescl._y;
            pos._z /= bonescl._z;
            Vector3 globpos = r.Multiply(pos);
            Matrix m = Matrix.TransformMatrix(new Vector3(1), bonerot, globpos + bonepos);
            Vector3 resultpos = new Vector3(m[12], m[13], m[14]);
            m = Matrix.TransformMatrix(new Vector3(Helpers.UnScalar(size)), new Vector3(), resultpos);
            GL.PushMatrix();
            GL.MultMatrix((float*) &m);
            int res = 16;
            double drawangle = 360.0 / res;
            // bubble
            if (style == Helpers.DrawStyle.SSB64)
            {
                GL.Color4(1.0f, 1.0f, 1.0f, 0.25f);
                c.DrawInvertedCube(new Vector3(0, 0, 0), 1.025f);
                GL.Color4(1.0f, 0.0f, 0.0f, 0.5f);
                c.DrawCube(new Vector3(0, 0, 0), 0.975f);
            }
            else
            {
                Vector3 typecolour = Helpers.getTypeColour(Helpers.HitboxType.Throwing);
                GL.Color4(typecolour._x / 255.0f, typecolour._y / 225.0f, typecolour._z / 255.0f, 0.375f);
                GLDisplayList spheres = c.GetSphereList();
                spheres.Call();
            }

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
        */
    }
}