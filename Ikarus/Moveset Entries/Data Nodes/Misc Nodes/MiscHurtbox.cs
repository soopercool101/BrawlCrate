using BrawlLib.Modeling;
using BrawlLib.SSBB.ResourceNodes;
using OpenTK.Graphics.OpenGL;
using System;
using System.ComponentModel;

namespace Ikarus.MovesetFile
{
    public unsafe class MiscHurtBox : MovesetEntryNode
    {
        internal Vector3 _posOffset, _stretch;
        internal float _radius;
        internal sHurtBoxFlags _flags = new sHurtBoxFlags();

        [Browsable(false)]
        public MDL0BoneNode BoneNode
        {
            get { if (Model == null) return null; if (_flags.BoneIndex >= Model._linker.BoneCache.Length || _flags.BoneIndex < 0) return null; return (MDL0BoneNode)Model._linker.BoneCache[_flags.BoneIndex]; }
            set { _flags.BoneIndex = value.BoneIndex; _name = value.Name; }
        }

        [Category("HurtBox"), TypeConverter(typeof(Vector3StringConverter))]
        public Vector3 PosOffset { get { return _posOffset; } set { _posOffset = value; SignalPropertyChange(); } }
        [Category("HurtBox"), TypeConverter(typeof(Vector3StringConverter))]
        public Vector3 Stretch { get { return _stretch; } set { _stretch = value; SignalPropertyChange(); } }
        [Category("HurtBox")]
        public float Radius { get { return _radius; } set { _radius = value; SignalPropertyChange(); } }
        [Category("HurtBox"), Browsable(true), TypeConverter(typeof(DropDownListBonesMDef))]
        public string Bone { get { return BoneNode == null ? _flags.BoneIndex.ToString() : BoneNode.Name; } set { if (Model == null) { _flags.BoneIndex = Convert.ToInt32(value); _name = _flags.BoneIndex.ToString(); } else { BoneNode = String.IsNullOrEmpty(value) ? BoneNode : Model.FindBone(value); } SignalPropertyChange(); } }
        [Category("HurtBox")]
        public bool Enabled { get { return _flags.Enabled; } set { _flags.Enabled = value; SignalPropertyChange(); } }
        [Category("HurtBox")]
        public HurtBoxZone Zone { get { return _flags.Zone; } set { _flags.Zone = value; SignalPropertyChange(); } }
        [Category("HurtBox")]
        public int Region { get { return _flags.Region; } set { _flags.Region = value; SignalPropertyChange(); } }
        [Category("HurtBox")]
        public int Unknown { get { return _flags.Unk; } set { _flags.Unk = value; SignalPropertyChange(); } }

        public override string Name
        {
            get { return Bone; }
        }

        protected override void OnParse(VoidPtr address)
        {
            sHurtBox* hdr = (sHurtBox*)address;
            _posOffset = hdr->_offset;
            _stretch = hdr->_stretch;
            _radius = hdr->_radius;
            _flags = hdr->_flags;
        }

        protected override int OnGetSize()
        {
            return 0x20;
        }

        protected override void OnWrite(VoidPtr address)
        {
            sHurtBox* header = (sHurtBox*)(RebuildAddress = address);
            header->_offset = _posOffset;
            header->_stretch = _stretch;
            header->_radius = _radius;
            header->_flags = _flags;
        }

        #region Rendering
        public unsafe void Render(bool selected, int type)
        {
            if (BoneNode == null)
                return;

            //Disable all things that could be enabled
            GL.Disable(EnableCap.CullFace);
            GL.Disable(EnableCap.Lighting);
            GL.Disable(EnableCap.DepthTest);
            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);

            switch (type)
            {
                case 0: //normal - yellow
                    switch ((int)Zone)
                    {
                        case 0:
                            GL.Color4(selected ? 0.0f : 0.5f, 0.5f, 0.0f, 0.5f);
                            break;
                        default:
                            GL.Color4(selected ? 0.0f : 1.0f, 1.0f, 0.0f, 0.5f);
                            break;
                        case 2:
                            GL.Color4(selected ? 0.0f : 1.0f, 1.0f, 0.25f, 0.5f);
                            break;
                    }
                    break;
                case 1: //invincible - green
                    switch ((int)Zone)
                    {
                        case 0:
                            GL.Color4(selected ? 0.0f : 0.0f, 0.5f, 0.0f, 0.5f);
                            break;
                        default:
                            GL.Color4(selected ? 0.0f : 0.0f, 1.0f, 0.0f, 0.5f);
                            break;
                        case 2:
                            GL.Color4(selected ? 0.0f : 0.0f, 1.0f, 0.25f, 0.5f);
                            break;
                    }
                    break;
                default: //intangible - blue
                    switch ((int)Zone)
                    {
                        case 0:
                            GL.Color4(0.0f, selected ? 0.5f : 0.0f, selected ? 0.0f : 0.5f, 0.5f);
                            break;
                        default:
                            GL.Color4(0.0f, selected ? 1.0f : 0.0f, selected ? 0.0f : 1.0f, 0.5f);
                            break;
                        case 2:
                            GL.Color4(0.0f, selected ? 1.0f : 0.25f, selected ? 0.25f : 1.0f, 0.5f);
                            break;
                    }
                    break;
            }

            FrameState frame = BoneNode.Matrix.Derive();
            Vector3 bonescl = frame._scale * _radius;

            Matrix m = Matrix.TransformMatrix(bonescl, frame._rotate, frame._translate);

            GL.PushMatrix();
            GL.MultMatrix((float*)&m);

            Vector3 stretchfac = _stretch / bonescl;
            GL.Translate(_posOffset._x / bonescl._x, _posOffset._y / bonescl._y, _posOffset._z / bonescl._z);

            int res = 16;
            double angle = 360.0 / res;

            // eight corners: XYZ, XYz, XyZ, Xyz, xYZ, xYz, xyZ, xyz
            for (int quadrant = 0; quadrant < 8; quadrant++)
            {
                for (double i = 0; i < 180 / angle; i++)
                {
                    double ringang1 = (i * angle) / 180 * Math.PI;
                    double ringang2 = ((i + 1) * angle) / 180 * Math.PI;

                    for (double j = 0; j < 360 / angle; j++)
                    {
                        double ang1 = (j * angle) / 180 * Math.PI;
                        double ang2 = ((j + 1) * angle) / 180 * Math.PI;

                        int q = 0;
                        Vector3 stretch = new Vector3(0, 0, 0);

                        if (Math.Cos(ang2) >= 0) // X
                        {
                            q += 4;
                            if (_stretch._x > 0)
                                stretch._x = stretchfac._x;
                        }
                        else
                        {
                            if (_stretch._x < 0)
                                stretch._x = stretchfac._x;
                        }
                        if (Math.Sin(ang2) >= 0) // Y
                        {
                            q += 2;
                            if (_stretch._y > 0)
                                stretch._y = stretchfac._y;
                        }
                        else
                        {
                            if (_stretch._y < 0)
                                stretch._y = stretchfac._y;
                        }
                        if (Math.Cos(ringang2) >= 0) // Z
                        {
                            q += 1;
                            if (_stretch._z > 0)
                                stretch._z = stretchfac._z;
                        }
                        else
                        {
                            if (_stretch._z < 0)
                                stretch._z = stretchfac._z;
                        }
                        if (quadrant == q)
                        {
                            GL.Translate(stretch._x, stretch._y, stretch._z);
                            GL.Begin(BeginMode.Quads);
                            GL.Vertex3(Math.Cos(ang1) * Math.Sin(ringang2), Math.Sin(ang1) * Math.Sin(ringang2), Math.Cos(ringang2));
                            GL.Vertex3(Math.Cos(ang2) * Math.Sin(ringang2), Math.Sin(ang2) * Math.Sin(ringang2), Math.Cos(ringang2));
                            GL.Vertex3(Math.Cos(ang2) * Math.Sin(ringang1), Math.Sin(ang2) * Math.Sin(ringang1), Math.Cos(ringang1));
                            GL.Vertex3(Math.Cos(ang1) * Math.Sin(ringang1), Math.Sin(ang1) * Math.Sin(ringang1), Math.Cos(ringang1));
                            GL.End();
                            GL.Translate(-stretch._x, -stretch._y, -stretch._z);
                        }
                    }
                }
            }

            // twelve edges
            double x1, x2, y1, y2, z1, z2;

            // x-axis edges
            for (double i = 0; i < 360 / angle; i++)
            {
                double ang1 = (i * angle) / 180 * Math.PI;
                double ang2 = ((i + 1) * angle) / 180 * Math.PI;

                z1 = Math.Cos(ang1);
                z2 = Math.Cos(ang2);
                y1 = Math.Sin(ang1);
                y2 = Math.Sin(ang2);

                x1 = _stretch._x < 0 ? stretchfac._x : 0;
                x2 = _stretch._x > 0 ? stretchfac._x : 0;

                if (y2 >= 0 && _stretch._y > 0)
                {
                    y1 += stretchfac._y;
                    y2 += stretchfac._y;
                }
                if (y2 <= 0 && _stretch._y < 0)
                {
                    y1 += stretchfac._y;
                    y2 += stretchfac._y;
                }
                if (z2 >= 0 && _stretch._z > 0)
                {
                    z1 += stretchfac._z;
                    z2 += stretchfac._z;
                }
                if (z2 <= 0 && _stretch._z < 0)
                {
                    z1 += stretchfac._z;
                    z2 += stretchfac._z;
                }

                GL.Begin(BeginMode.Quads);
                GL.Vertex3(x1, y1, z1);
                GL.Vertex3(x2, y1, z1);
                GL.Vertex3(x2, y2, z2);
                GL.Vertex3(x1, y2, z2);
                GL.End();
            }

            // y-axis edges
            for (double i = 0; i < 360 / angle; i++)
            {
                double ang1 = (i * angle) / 180 * Math.PI;
                double ang2 = ((i + 1) * angle) / 180 * Math.PI;

                x1 = Math.Cos(ang1);
                x2 = Math.Cos(ang2);
                z1 = Math.Sin(ang1);
                z2 = Math.Sin(ang2);

                y1 = _stretch._y < 0 ? stretchfac._y : 0;
                y2 = _stretch._y > 0 ? stretchfac._y : 0;

                if (x2 >= 0 && _stretch._x > 0)
                {
                    x1 += stretchfac._x;
                    x2 += stretchfac._x;
                }
                if (x2 <= 0 && _stretch._x < 0)
                {
                    x1 += stretchfac._x;
                    x2 += stretchfac._x;
                }
                if (z2 >= 0 && _stretch._z > 0)
                {
                    z1 += stretchfac._z;
                    z2 += stretchfac._z;
                }
                if (z2 <= 0 && _stretch._z < 0)
                {
                    z1 += stretchfac._z;
                    z2 += stretchfac._z;
                }

                GL.Begin(BeginMode.Quads);
                GL.Vertex3(x1, y1, z1);
                GL.Vertex3(x1, y2, z1);
                GL.Vertex3(x2, y2, z2);
                GL.Vertex3(x2, y1, z2);
                GL.End();
            }

            // z-axis edges
            for (double i = 0; i < 360 / angle; i++)
            {
                double ang1 = (i * angle) / 180 * Math.PI;
                double ang2 = ((i + 1) * angle) / 180 * Math.PI;

                x1 = Math.Cos(ang1);
                x2 = Math.Cos(ang2);
                y1 = Math.Sin(ang1);
                y2 = Math.Sin(ang2);

                z1 = _stretch._z < 0 ? stretchfac._z : 0;
                z2 = _stretch._z > 0 ? stretchfac._z : 0;

                if (x2 >= 0 && _stretch._x > 0)
                {
                    x1 += stretchfac._x;
                    x2 += stretchfac._x;
                }
                if (x2 <= 0 && _stretch._x < 0)
                {
                    x1 += stretchfac._x;
                    x2 += stretchfac._x;
                }
                if (y2 >= 0 && _stretch._y > 0)
                {
                    y1 += stretchfac._y;
                    y2 += stretchfac._y;
                }
                if (y2 <= 0 && _stretch._y < 0)
                {
                    y1 += stretchfac._y;
                    y2 += stretchfac._y;
                }

                GL.Begin(BeginMode.Quads);
                GL.Vertex3(x2, y2, z1);
                GL.Vertex3(x2, y2, z2);
                GL.Vertex3(x1, y1, z2);
                GL.Vertex3(x1, y1, z1);
                GL.End();
            }

            Vector3 scale = frame._scale;

            // six faces
            GL.Begin(BeginMode.Quads);
            float outpos;

            // left face
            outpos = _radius / bonescl._x * scale._x;
            if (_stretch._x > 0)
                outpos = (_stretch._x + _radius) / bonescl._x;

            GL.Vertex3(outpos, 0, 0);
            GL.Vertex3(outpos, stretchfac._y, 0);
            GL.Vertex3(outpos, stretchfac._y, stretchfac._z);
            GL.Vertex3(outpos, 0, stretchfac._z);

            // right face
            outpos = -_radius / bonescl._x * scale._x;
            if (_stretch._x < 0)
                outpos = (_stretch._x - _radius) / bonescl._x;

            GL.Vertex3(outpos, 0, 0);
            GL.Vertex3(outpos, 0, stretchfac._z);
            GL.Vertex3(outpos, stretchfac._y, stretchfac._z);
            GL.Vertex3(outpos, stretchfac._y, 0);

            // top face
            outpos = _radius / bonescl._y * scale._y;
            if (_stretch._y > 0)
                outpos = (_stretch._y + _radius) / bonescl._y;

            GL.Vertex3(0, outpos, 0);
            GL.Vertex3(0, outpos, stretchfac._z);
            GL.Vertex3(stretchfac._x, outpos, stretchfac._z);
            GL.Vertex3(stretchfac._x, outpos, 0);

            // bottom face
            outpos = -_radius / bonescl._y * scale._y;
            if (_stretch._y < 0)
                outpos = (_stretch._y - _radius) / bonescl._y;

            GL.Vertex3(0, outpos, 0);
            GL.Vertex3(stretchfac._x, outpos, 0);
            GL.Vertex3(stretchfac._x, outpos, stretchfac._z);
            GL.Vertex3(0, outpos, stretchfac._z);

            // front face
            outpos = _radius / bonescl._z * scale._z;
            if (_stretch._z > 0)
                outpos = (_stretch._z + _radius) / bonescl._z;

            GL.Vertex3(0, 0, outpos);
            GL.Vertex3(stretchfac._x, 0, outpos);
            GL.Vertex3(stretchfac._x, stretchfac._y, outpos);
            GL.Vertex3(0, stretchfac._y, outpos);

            // right face
            outpos = -_radius / bonescl._z * scale._z;
            if (_stretch._z < 0)
                outpos = (_stretch._z - _radius) / bonescl._z;

            GL.Vertex3(0, 0, outpos);
            GL.Vertex3(0, stretchfac._y, outpos);
            GL.Vertex3(stretchfac._x, stretchfac._y, outpos);
            GL.Vertex3(stretchfac._x, 0, outpos);
            GL.End();

            GL.PopMatrix();
        }
        #endregion
    }
}
