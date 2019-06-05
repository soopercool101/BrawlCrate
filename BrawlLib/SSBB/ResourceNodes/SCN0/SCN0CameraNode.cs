using System;
using System.ComponentModel;
using System.Windows.Forms;
using BrawlLib.OpenGL;
using BrawlLib.SSBBTypes;
using BrawlLib.Wii.Animations;
using BrawlLib.Wii.Graphics;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class SCN0CameraNode : SCN0EntryNode, IKeyframeSource
    {
        public static bool _generateTangents = true;
        public SCN0CameraFlags _flags1 = (SCN0CameraFlags) 0xFFFE;
        public ushort _flags2 = 1;

        private KeyframeCollection _keyframes;
        public ProjectionType _projType;

        public SCN0CameraType _type = SCN0CameraType.Aim;
        internal UserDataCollection _userEntries = new UserDataCollection();

        public SCN0CameraFlags[] Ordered =
        {
            SCN0CameraFlags.PosXConstant,
            SCN0CameraFlags.PosYConstant,
            SCN0CameraFlags.PosZConstant,
            SCN0CameraFlags.AspectConstant,
            SCN0CameraFlags.NearConstant,
            SCN0CameraFlags.FarConstant,
            SCN0CameraFlags.RotXConstant,
            SCN0CameraFlags.RotYConstant,
            SCN0CameraFlags.RotZConstant,
            SCN0CameraFlags.AimXConstant,
            SCN0CameraFlags.AimYConstant,
            SCN0CameraFlags.AimZConstant,
            SCN0CameraFlags.TwistConstant,
            SCN0CameraFlags.PerspFovYConstant,
            SCN0CameraFlags.OrthoHeightConstant
        };

        internal SCN0Camera* Data => (SCN0Camera*) WorkingUncompressed.Address;
        public override ResourceType ResourceFileType => ResourceType.SCN0Camera;

        [Category("User Data")]
        [TypeConverter(typeof(ExpandableObjectCustomConverter))]
        public UserDataCollection UserEntries
        {
            get => _userEntries;
            set
            {
                _userEntries = value;
                SignalPropertyChange();
            }
        }

        [Category("Camera")]
        public SCN0CameraType Type
        {
            get => _type;
            set
            {
                _type = value;
                SignalPropertyChange();
            }
        }

        [Category("Camera")]
        public ProjectionType ProjectionType
        {
            get => _projType;
            set
            {
                _projType = value;
                SignalPropertyChange();
            }
        }

        [Browsable(false)] public KeyframeArray PosX => Keyframes[0];

        [Browsable(false)] public KeyframeArray PosY => Keyframes[1];

        [Browsable(false)] public KeyframeArray PosZ => Keyframes[2];

        [Browsable(false)] public KeyframeArray Aspect => Keyframes[3];

        [Browsable(false)] public KeyframeArray NearZ => Keyframes[4];

        [Browsable(false)] public KeyframeArray FarZ => Keyframes[5];

        [Browsable(false)] public KeyframeArray RotX => Keyframes[6];

        [Browsable(false)] public KeyframeArray RotY => Keyframes[7];

        [Browsable(false)] public KeyframeArray RotZ => Keyframes[8];

        [Browsable(false)] public KeyframeArray AimX => Keyframes[9];

        [Browsable(false)] public KeyframeArray AimY => Keyframes[10];

        [Browsable(false)] public KeyframeArray AimZ => Keyframes[11];

        [Browsable(false)] public KeyframeArray Twist => Keyframes[12];

        [Browsable(false)] public KeyframeArray FovY => Keyframes[13];

        [Browsable(false)] public KeyframeArray Height => Keyframes[14];

        [Browsable(false)]
        public KeyframeCollection Keyframes
        {
            get
            {
                if (_keyframes == null)
                {
                    _keyframes =
                        new KeyframeCollection(15, Scene == null ? 1 : Scene.FrameCount + (Scene.Loop ? 1 : 0));
                    if (Data != null && Name != "<null>")
                        for (var i = 0; i < 15; i++)
                            DecodeKeyframes(
                                Keyframes[i],
                                Data->_position._x.Address + i * 4,
                                (int) _flags1,
                                (int) Ordered[i]);
                }

                return _keyframes;
            }
        }

        [Browsable(false)] public int FrameCount => Keyframes.FrameLimit;

        [Browsable(false)] public KeyframeArray[] KeyArrays => Keyframes._keyArrays;

        public override bool OnInitialize()
        {
            //Read common header
            base.OnInitialize();

            if (_name == "<null>") return false;

            //Read header data
            _flags1 = (SCN0CameraFlags) (ushort) Data->_flags1;
            _flags2 = Data->_flags2;
            _type = (SCN0CameraType) (_flags2 & 1);
            _projType = (ProjectionType) (int) Data->_projectionType;

            //Read user data
            (_userEntries = new UserDataCollection()).Read(Data->UserData);

            return false;
        }

        internal override void GetStrings(StringTable table)
        {
            if (Name == "<null>") return;

            table.Add(Name);
            foreach (UserDataClass s in _userEntries)
            {
                table.Add(s._name);
                if (s._type == UserValueType.String && s._entries.Count > 0) table.Add(s._entries[0]);
            }
        }

        public override int OnCalculateSize(bool force)
        {
            //Reset data lengths
            for (var i = 0; i < 3; i++) _dataLengths[i] = 0;

            var size = SCN0Camera.Size;

            if (_name != "<null>")
            {
                //Get the total data size of all keyframes
                for (var i = 0; i < 15; i++) CalcKeyLen(Keyframes[i]);

                //Add the size of the user entries
                size += _userEntries.GetSize();
            }

            return size;
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            base.OnRebuild(address, length, force);

            if (_name == "<null>") return;

            var header = (SCN0Camera*) address;

            header->_projectionType = (int) _projType;
            header->_flags2 = (ushort) (2 + (int) _type);
            header->_userDataOffset = 0;

            var newFlags1 = 0;

            for (var i = 0; i < 15; i++)
                _dataAddrs[0] += EncodeKeyframes(
                    Keyframes[i],
                    _dataAddrs[0],
                    header->_position._x.Address + i * 4,
                    ref newFlags1,
                    (int) Ordered[i]);

            header->_flags1 = (ushort) newFlags1;

            if (_userEntries.Count > 0) _userEntries.Write(header->UserData = (VoidPtr) header + SCN0Camera.Size);
        }

        protected internal override void PostProcess(VoidPtr scn0Address, VoidPtr dataAddress, StringTable stringTable)
        {
            base.PostProcess(scn0Address, dataAddress, stringTable);

            if (_name != "<null>") _userEntries.PostProcess(((SCN0Camera*) dataAddress)->UserData, stringTable);
        }

        public void SetCamera(ModelPanelViewport v, float frame, bool retainAspect)
        {
            var proj = (ViewportProjection) (int) ProjectionType;
            if (v.ViewType != proj) v.SetProjectionType(proj);

            var cam = v.Camera;
            var f = GetAnimFrame(frame);
            cam.Reset();
            cam.Translate(f.Pos);

            var rotate = f.GetRotation(Type);
            cam.Rotate(rotate);

            var aspect = retainAspect ? cam.Aspect : f.Aspect;
            cam.SetProjectionParams(aspect, f.FovY, f.FarZ, f.NearZ);
        }

        /// <summary>
        ///     mtx is from camera to world space (a point in camera space)
        ///     inverse is from world to camera space (a point in the world)
        /// </summary>
        public void GetModelViewMatrix(float frame, out Matrix mtx, out Matrix inverse)
        {
            var f = GetAnimFrame(frame);
            var r = f.GetRotation(Type);
            var t = f.Pos;

            mtx = Matrix.ReverseTransformMatrix(new Vector3(1.0f), r, t);
            inverse = Matrix.TransformMatrix(new Vector3(1.0f), r, t);
        }

        public Vector3 GetStart(float frame)
        {
            return new Vector3(
                PosX.GetFrameValue(frame),
                PosY.GetFrameValue(frame),
                PosZ.GetFrameValue(frame));
        }

        public Vector3 GetEnd(float frame)
        {
            return new Vector3(
                AimX.GetFrameValue(frame),
                AimY.GetFrameValue(frame),
                AimZ.GetFrameValue(frame));
        }

        public CameraAnimationFrame GetAnimFrame(float index)
        {
            CameraAnimationFrame frame;
            var dPtr = (float*) &frame;
            for (var x = 0; x < 15; x++)
            {
                var a = Keyframes[x];
                *dPtr++ = a.GetFrameValue(index);
                frame.SetBools(x, a.GetKeyframe((int) index) != null);
                frame.Index = index;
            }

            return frame;
        }

        internal KeyframeEntry GetKeyframe(CameraKeyframeMode keyFrameMode, int index)
        {
            return Keyframes[(int) keyFrameMode].GetKeyframe(index);
        }

        public float GetFrameValue(CameraKeyframeMode keyFrameMode, float index)
        {
            return Keyframes[(int) keyFrameMode].GetFrameValue(index);
        }

        internal void RemoveKeyframe(CameraKeyframeMode keyFrameMode, int index)
        {
            var k = Keyframes[(int) keyFrameMode].Remove(index);
            if (k != null && _generateTangents)
            {
                k._prev.GenerateTangent();
                k._next.GenerateTangent();
                SignalPropertyChange();
            }
        }

        internal void SetKeyframe(CameraKeyframeMode keyFrameMode, int index, float value)
        {
            var keys = Keyframes[(int) keyFrameMode];
            var exists = keys.GetKeyframe(index) != null;
            var k = keys.SetFrameValue(index, value);

            if (!exists && !_generateTangents) k.GenerateTangent();

            if (_generateTangents)
            {
                k.GenerateTangent();
                k._prev.GenerateTangent();
                k._next.GenerateTangent();
            }

            SignalPropertyChange();
        }

        internal void SetSize(int numFrames, bool looped)
        {
            Keyframes.FrameLimit = numFrames + (looped ? 1 : 0);
        }

        internal Vector3 GetPosition(float index)
        {
            return new Vector3(PosX.GetFrameValue(index), PosY.GetFrameValue(index), PosZ.GetFrameValue(index));
        }

        public Vector3 GetRotate(float frame)
        {
            return new Vector3(RotX.GetFrameValue(frame), RotY.GetFrameValue(frame), RotZ.GetFrameValue(frame));
        }
    }

    public enum CameraKeyframeMode
    {
        PosX,
        PosY,
        PosZ,
        Aspect,
        NearZ,
        FarZ,
        RotX,
        RotY,
        RotZ,
        AimX,
        AimY,
        AimZ,
        Twist,
        FovY,
        Height
    }
}