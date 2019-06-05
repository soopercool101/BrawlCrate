using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Windows.Forms;
using BrawlLib.Imaging;
using BrawlLib.IO;

namespace BrawlLib.Modeling
{
    public unsafe partial class Collada : Form
    {
        private class DecoderShell : IDisposable
        {
            internal readonly List<EffectEntry> _effects = new List<EffectEntry>();
            internal readonly List<GeometryEntry> _geometry = new List<GeometryEntry>();
            internal readonly List<ImageEntry> _images = new List<ImageEntry>();
            internal readonly List<MaterialEntry> _materials = new List<MaterialEntry>();
            internal readonly List<NodeEntry> _nodes = new List<NodeEntry>();
            internal readonly XmlReader _reader;

            public float _scale = 1;
            internal readonly List<SceneEntry> _scenes = new List<SceneEntry>();
            internal readonly List<SkinEntry> _skins = new List<SkinEntry>();
            internal int _v1, _v2, _v3;

            private DecoderShell(XmlReader reader)
            {
                _reader = reader;

                while (reader.BeginElement())
                {
                    if (reader.Name.Equals("COLLADA", true)) ParseMain();

                    reader.EndElement();
                }

                _reader = null;
            }

            public void Dispose()
            {
                foreach (var geo in _geometry) geo.Dispose();
            }

            public static DecoderShell Import(string path)
            {
                using (var map = FileMap.FromFile(path))
                using (var reader = new XmlReader(map.Address, map.Length))
                {
                    return new DecoderShell(reader);
                }
            }

            ~DecoderShell()
            {
                Dispose();
            }

            private void Output(string message)
            {
                MessageBox.Show(message);
            }

            public NodeEntry FindNode(string name)
            {
                NodeEntry n;
                foreach (var scene in _scenes)
                foreach (var node in scene._nodes)
                    if ((n = FindNodeInternal(name, node)) != null)
                        return n;

                return null;
            }

            internal static NodeEntry FindNodeInternal(string name, NodeEntry node)
            {
                NodeEntry e;

                if (node._name == name || node._sid == name || node._id == name) return node;

                foreach (var n in node._children)
                    if ((e = FindNodeInternal(name, n)) != null)
                        return e;

                return null;
            }

            private void ParseMain()
            {
                while (_reader.ReadAttribute())
                    if (_reader.Name.Equals("version", true))
                    {
                        var v = (string) _reader.Value;
                        var s = v.Split('.');
                        int.TryParse(s[0], NumberStyles.Number, CultureInfo.InvariantCulture.NumberFormat, out _v1);
                        int.TryParse(s[1], NumberStyles.Number, CultureInfo.InvariantCulture.NumberFormat, out _v2);
                        int.TryParse(s[2], NumberStyles.Number, CultureInfo.InvariantCulture.NumberFormat, out _v3);
                    }

                while (_reader.BeginElement())
                {
                    if (_reader.Name.Equals("asset", true))
                        ParseAsset();
                    else if (_reader.Name.Equals("library_images", true))
                        ParseLibImages();
                    else if (_reader.Name.Equals("library_materials", true))
                        ParseLibMaterials();
                    else if (_reader.Name.Equals("library_effects", true))
                        ParseLibEffects();
                    else if (_reader.Name.Equals("library_geometries", true))
                        ParseLibGeometry();
                    else if (_reader.Name.Equals("library_controllers", true))
                        ParseLibControllers();
                    else if (_reader.Name.Equals("library_visual_scenes", true))
                        ParseLibScenes();
                    else if (_reader.Name.Equals("library_nodes", true)) ParseLibNodes();

                    _reader.EndElement();
                }
            }

            private void ParseAsset()
            {
                while (_reader.BeginElement())
                {
                    if (_reader.Name.Equals("unit", true))
                        while (_reader.ReadAttribute())
                            if (_reader.Name.Equals("meter", true))
                                float.TryParse((string) _reader.Value, NumberStyles.Any,
                                    CultureInfo.InvariantCulture.NumberFormat, out _scale);

                    _reader.EndElement();
                }
            }

            private void ParseLibImages()
            {
                ImageEntry img;
                while (_reader.BeginElement())
                {
                    if (_reader.Name.Equals("image", true))
                    {
                        img = new ImageEntry();
                        while (_reader.ReadAttribute())
                            if (_reader.Name.Equals("id", true))
                                img._id = (string) _reader.Value;
                            else if (_reader.Name.Equals("name", true)) img._name = (string) _reader.Value;

                        while (_reader.BeginElement())
                        {
                            img._path = null;
                            if (_reader.Name.Equals("init_from", true))
                            {
                                if (_v2 < 5)
                                    img._path = _reader.ReadElementString();
                                else
                                    while (_reader.BeginElement())
                                    {
                                        if (_reader.Name.Equals("ref", true)) img._path = _reader.ReadElementString();

                                        _reader.EndElement();
                                    }
                            }

                            _reader.EndElement();
                        }

                        _images.Add(img);
                    }

                    _reader.EndElement();
                }
            }

            private void ParseLibMaterials()
            {
                MaterialEntry mat;
                while (_reader.BeginElement())
                {
                    if (_reader.Name.Equals("material", true))
                    {
                        mat = new MaterialEntry();
                        while (_reader.ReadAttribute())
                            if (_reader.Name.Equals("id", true))
                                mat._id = (string) _reader.Value;
                            else if (_reader.Name.Equals("name", true)) mat._name = (string) _reader.Value;

                        while (_reader.BeginElement())
                        {
                            if (_reader.Name.Equals("instance_effect", true))
                                while (_reader.ReadAttribute())
                                    if (_reader.Name.Equals("url", true))
                                        mat._effect = _reader.Value[0] == '#'
                                            ? (string) (_reader.Value + 1)
                                            : (string) _reader.Value;

                            _reader.EndElement();
                        }

                        _materials.Add(mat);
                    }

                    _reader.EndElement();
                }
            }

            private void ParseLibEffects()
            {
                while (_reader.BeginElement())
                {
                    if (_reader.Name.Equals("effect", true)) _effects.Add(ParseEffect());

                    _reader.EndElement();
                }
            }

            private EffectEntry ParseEffect()
            {
                var eff = new EffectEntry();

                while (_reader.ReadAttribute())
                    if (_reader.Name.Equals("id", true))
                        eff._id = (string) _reader.Value;
                    else if (_reader.Name.Equals("name", true)) eff._name = (string) _reader.Value;

                while (_reader.BeginElement())
                {
                    //Only common is supported
                    if (_reader.Name.Equals("profile_COMMON", true))
                        while (_reader.BeginElement())
                        {
                            if (_reader.Name.Equals("newparam", true))
                                eff._newParams.Add(ParseNewParam());
                            else if (_reader.Name.Equals("technique", true))
                                while (_reader.BeginElement())
                                {
                                    if (_reader.Name.Equals("phong", true))
                                        eff._shader = ParseShader(ShaderType.phong);
                                    else if (_reader.Name.Equals("lambert", true))
                                        eff._shader = ParseShader(ShaderType.lambert);
                                    else if (_reader.Name.Equals("blinn", true))
                                        eff._shader = ParseShader(ShaderType.blinn);

                                    _reader.EndElement();
                                }

                            _reader.EndElement();
                        }

                    _reader.EndElement();
                }

                return eff;
            }

            private EffectNewParam ParseNewParam()
            {
                var p = new EffectNewParam();

                while (_reader.ReadAttribute())
                    if (_reader.Name.Equals("sid", true))
                        p._sid = (string) _reader.Value;
                    else if (_reader.Name.Equals("id", true)) p._id = (string) _reader.Value;
                while (_reader.BeginElement())
                {
                    if (_reader.Name.Equals("surface", true))
                        while (_reader.BeginElement())
                        {
                            p._path = null;
                            if (_reader.Name.Equals("init_from", true))
                            {
                                if (_v2 < 5)
                                    p._path = _reader.ReadElementString();
                                else
                                    while (_reader.BeginElement())
                                    {
                                        if (_reader.Name.Equals("ref", true)) p._path = _reader.ReadElementString();

                                        _reader.EndElement();
                                    }
                            }

                            _reader.EndElement();
                        }
                    else if (_reader.Name.Equals("sampler2D", true)) p._sampler2D = ParseSampler2D();

                    _reader.EndElement();
                }

                return p;
            }

            private EffectSampler2D ParseSampler2D()
            {
                var s = new EffectSampler2D();

                while (_reader.BeginElement())
                {
                    if (_reader.Name.Equals("source", true))
                        s._source = _reader.ReadElementString();
                    else if (_reader.Name.Equals("instance_image", true))
                        while (_reader.ReadAttribute())
                            if (_reader.Name.Equals("url", true))
                                s._url = _reader.Value[0] == '#'
                                    ? (string) (_reader.Value + 1)
                                    : (string) _reader.Value;
                    else if (_reader.Name.Equals("wrap_s", true))
                        s._wrapS = _reader.ReadElementString();
                    else if (_reader.Name.Equals("wrap_t", true))
                        s._wrapT = _reader.ReadElementString();
                    else if (_reader.Name.Equals("minfilter", true))
                        s._minFilter = _reader.ReadElementString();
                    else if (_reader.Name.Equals("magfilter", true)) s._magFilter = _reader.ReadElementString();

                    _reader.EndElement();
                }

                return s;
            }

            private EffectShaderEntry ParseShader(ShaderType type)
            {
                var s = new EffectShaderEntry
                {
                    _type = type
                };
                float v;

                while (_reader.BeginElement())
                {
                    if (_reader.Name.Equals("ambient", true))
                        s._effects.Add(ParseLightEffect(LightEffectType.ambient));
                    else if (_reader.Name.Equals("diffuse", true))
                        s._effects.Add(ParseLightEffect(LightEffectType.diffuse));
                    else if (_reader.Name.Equals("emission", true))
                        s._effects.Add(ParseLightEffect(LightEffectType.emission));
                    else if (_reader.Name.Equals("reflective", true))
                        s._effects.Add(ParseLightEffect(LightEffectType.reflective));
                    else if (_reader.Name.Equals("specular", true))
                        s._effects.Add(ParseLightEffect(LightEffectType.specular));
                    else if (_reader.Name.Equals("transparent", true))
                        s._effects.Add(ParseLightEffect(LightEffectType.transparent));
                    else if (_reader.Name.Equals("shininess", true))
                        while (_reader.BeginElement())
                        {
                            if (_reader.Name.Equals("float", true))
                                if (_reader.ReadValue(&v))
                                    s._shininess = v;

                            _reader.EndElement();
                        }
                    else if (_reader.Name.Equals("reflectivity", true))
                        while (_reader.BeginElement())
                        {
                            if (_reader.Name.Equals("float", true))
                                if (_reader.ReadValue(&v))
                                    s._reflectivity = v;

                            _reader.EndElement();
                        }
                    else if (_reader.Name.Equals("transparency", true))
                        while (_reader.BeginElement())
                        {
                            if (_reader.Name.Equals("float", true))
                                if (_reader.ReadValue(&v))
                                    s._transparency = v;

                            _reader.EndElement();
                        }

                    _reader.EndElement();
                }

                return s;
            }

            private LightEffectEntry ParseLightEffect(LightEffectType type)
            {
                var eff = new LightEffectEntry
                {
                    _type = type
                };

                while (_reader.BeginElement())
                {
                    if (_reader.Name.Equals("color", true))
                        eff._color = ParseColor();
                    else if (_reader.Name.Equals("texture", true))
                        while (_reader.ReadAttribute())
                            if (_reader.Name.Equals("texture", true))
                                eff._texture = (string) _reader.Value;
                            else if (_reader.Name.Equals("texcoord", true)) eff._texCoord = (string) _reader.Value;

                    _reader.EndElement();
                }

                return eff;
            }

            private void ParseLibGeometry()
            {
                GeometryEntry geo;
                while (_reader.BeginElement())
                {
                    if (_reader.Name.Equals("geometry", true))
                    {
                        geo = new GeometryEntry();
                        while (_reader.ReadAttribute())
                            if (_reader.Name.Equals("id", true))
                                geo._id = (string) _reader.Value;
                            else if (_reader.Name.Equals("name", true)) geo._name = (string) _reader.Value;

                        while (_reader.BeginElement())
                        {
                            if (_reader.Name.Equals("mesh", true))
                                while (_reader.BeginElement())
                                {
                                    if (_reader.Name.Equals("source", true))
                                    {
                                        geo._sources.Add(ParseSource());
                                    }
                                    else if (_reader.Name.Equals("vertices", true))
                                    {
                                        while (_reader.ReadAttribute())
                                            if (_reader.Name.Equals("id", true))
                                                geo._verticesId = (string) _reader.Value;

                                        while (_reader.BeginElement())
                                        {
                                            if (_reader.Name.Equals("input", true)) geo._verticesInput = ParseInput();

                                            _reader.EndElement();
                                        }
                                    }
                                    else if (_reader.Name.Equals("polygons", true))
                                    {
                                        geo._primitives.Add(ParsePrimitive(ColladaPrimitiveType.polygons));
                                    }
                                    else if (_reader.Name.Equals("polylist", true))
                                    {
                                        geo._primitives.Add(ParsePrimitive(ColladaPrimitiveType.polylist));
                                    }
                                    else if (_reader.Name.Equals("triangles", true))
                                    {
                                        geo._primitives.Add(ParsePrimitive(ColladaPrimitiveType.triangles));
                                    }
                                    else if (_reader.Name.Equals("tristrips", true))
                                    {
                                        geo._primitives.Add(ParsePrimitive(ColladaPrimitiveType.tristrips));
                                    }
                                    else if (_reader.Name.Equals("trifans", true))
                                    {
                                        geo._primitives.Add(ParsePrimitive(ColladaPrimitiveType.trifans));
                                    }
                                    else if (_reader.Name.Equals("lines", true))
                                    {
                                        geo._primitives.Add(ParsePrimitive(ColladaPrimitiveType.lines));
                                    }
                                    else if (_reader.Name.Equals("linestrips", true))
                                    {
                                        geo._primitives.Add(ParsePrimitive(ColladaPrimitiveType.linestrips));
                                    }

                                    _reader.EndElement();
                                }

                            _reader.EndElement();
                        }

                        _geometry.Add(geo);
                    }

                    _reader.EndElement();
                }
            }

            private PrimitiveEntry ParsePrimitive(ColladaPrimitiveType type)
            {
                var prim = new PrimitiveEntry {_type = type};
                PrimitiveFace p;
                int val;
                int stride = 0, elements = 0;

                switch (type)
                {
                    case ColladaPrimitiveType.trifans:
                    case ColladaPrimitiveType.tristrips:
                    case ColladaPrimitiveType.triangles:
                        stride = 3;
                        break;
                    case ColladaPrimitiveType.lines:
                    case ColladaPrimitiveType.linestrips:
                        stride = 2;
                        break;
                    case ColladaPrimitiveType.polygons:
                    case ColladaPrimitiveType.polylist:
                        stride = 4;
                        break;
                }

                while (_reader.ReadAttribute())
                    if (_reader.Name.Equals("material", true))
                        prim._material = (string) _reader.Value;
                    else if (_reader.Name.Equals("count", true)) prim._entryCount = int.Parse((string) _reader.Value);

                prim._faces.Capacity = prim._entryCount;

                while (_reader.BeginElement())
                {
                    if (_reader.Name.Equals("input", true))
                    {
                        prim._inputs.Add(ParseInput());
                        elements++;
                    }
                    else if (_reader.Name.Equals("p", true))
                    {
                        var indices = new List<ushort>(stride * elements);

                        p = new PrimitiveFace();
                        //p._pointIndices.Capacity = stride * elements;
                        while (_reader.ReadValue(&val)) indices.Add((ushort) val);

                        p._pointCount = indices.Count / elements;
                        p._pointIndices = indices.ToArray();

                        switch (type)
                        {
                            case ColladaPrimitiveType.trifans:
                            case ColladaPrimitiveType.tristrips:
                            case ColladaPrimitiveType.polygons:
                            case ColladaPrimitiveType.polylist:
                                p._faceCount = p._pointCount - 2;
                                break;

                            case ColladaPrimitiveType.triangles:
                                p._faceCount = p._pointCount / 3;
                                break;

                            case ColladaPrimitiveType.lines:
                                p._faceCount = p._pointCount / 2;
                                break;

                            case ColladaPrimitiveType.linestrips:
                                p._faceCount = p._pointCount - 1;
                                break;
                        }

                        prim._faceCount += p._faceCount;
                        prim._pointCount += p._pointCount;
                        prim._faces.Add(p);
                    }

                    _reader.EndElement();
                }

                prim._entryStride = elements;

                return prim;
            }

            private InputEntry ParseInput()
            {
                var inp = new InputEntry();

                while (_reader.ReadAttribute())
                    if (_reader.Name.Equals("id", true))
                        inp._id = (string) _reader.Value;
                    else if (_reader.Name.Equals("name", true))
                        inp._name = (string) _reader.Value;
                    else if (_reader.Name.Equals("semantic", true))
                        inp._semantic = (SemanticType) Enum.Parse(typeof(SemanticType), (string) _reader.Value, true);
                    else if (_reader.Name.Equals("set", true))
                        inp._set = int.Parse((string) _reader.Value);
                    else if (_reader.Name.Equals("offset", true))
                        inp._offset = int.Parse((string) _reader.Value);
                    else if (_reader.Name.Equals("source", true))
                        inp._source = _reader.Value[0] == '#' ? (string) (_reader.Value + 1) : (string) _reader.Value;

                return inp;
            }

            private SourceEntry ParseSource()
            {
                var src = new SourceEntry();

                while (_reader.ReadAttribute())
                    if (_reader.Name.Equals("id", true))
                        src._id = (string) _reader.Value;

                while (_reader.BeginElement())
                {
                    if (_reader.Name.Equals("float_array", true))
                    {
                        if (src._arrayType == SourceType.None)
                        {
                            src._arrayType = SourceType.Float;

                            while (_reader.ReadAttribute())
                                if (_reader.Name.Equals("id", true))
                                    src._arrayId = (string) _reader.Value;
                                else if (_reader.Name.Equals("count", true))
                                    src._arrayCount = int.Parse((string) _reader.Value);

                            var buffer = new UnsafeBuffer(src._arrayCount * 4);
                            src._arrayData = buffer;

                            var pOut = (float*) buffer.Address;
                            for (var i = 0; i < src._arrayCount; i++)
                                if (!_reader.ReadValue(pOut++))
                                    break;
                        }
                    }
                    else if (_reader.Name.Equals("int_array", true))
                    {
                        if (src._arrayType == SourceType.None)
                        {
                            src._arrayType = SourceType.Int;

                            while (_reader.ReadAttribute())
                                if (_reader.Name.Equals("id", true))
                                    src._arrayId = (string) _reader.Value;
                                else if (_reader.Name.Equals("count", true))
                                    src._arrayCount = int.Parse((string) _reader.Value);

                            var buffer = new UnsafeBuffer(src._arrayCount * 4);
                            src._arrayData = buffer;

                            var pOut = (int*) buffer.Address;
                            for (var i = 0; i < src._arrayCount; i++)
                                if (!_reader.ReadValue(pOut++))
                                    break;
                        }
                    }
                    else if (_reader.Name.Equals("Name_array", true))
                    {
                        if (src._arrayType == SourceType.None)
                        {
                            src._arrayType = SourceType.Name;

                            while (_reader.ReadAttribute())
                                if (_reader.Name.Equals("id", true))
                                    src._arrayId = (string) _reader.Value;
                                else if (_reader.Name.Equals("count", true))
                                    src._arrayCount = int.Parse((string) _reader.Value);

                            var list = new string[src._arrayCount];
                            src._arrayData = list;

                            var tempPtr = _reader._ptr;
                            var tempInTag = _reader._inTag;
                            src._arrayDataString = _reader.ReadElementString();
                            _reader._ptr = tempPtr;
                            _reader._inTag = tempInTag;

                            for (var i = 0; i < src._arrayCount; i++)
                                if (!_reader.ReadStringSingle())
                                    break;
                                else
                                    list[i] = (string) _reader.Value;
                        }
                    }
                    else if (_reader.Name.Equals("technique_common", true))
                    {
                        while (_reader.BeginElement())
                        {
                            if (_reader.Name.Equals("accessor", true))
                                while (_reader.ReadAttribute())
                                    if (_reader.Name.Equals("source", true))
                                        src._accessorSource = _reader.Value[0] == '#'
                                            ? (string) (_reader.Value + 1)
                                            : (string) _reader.Value;
                                    else if (_reader.Name.Equals("count", true))
                                        src._accessorCount = int.Parse((string) _reader.Value);
                                    else if (_reader.Name.Equals("stride", true))
                                        src._accessorStride = int.Parse((string) _reader.Value);

                            //Ignore params

                            _reader.EndElement();
                        }
                    }

                    _reader.EndElement();
                }

                return src;
            }

            private void ParseLibControllers()
            {
                while (_reader.BeginElement())
                {
                    if (_reader.Name.Equals("controller", false))
                    {
                        string id = null;
                        while (_reader.ReadAttribute())
                            if (_reader.Name.Equals("id", false))
                                id = (string) _reader.Value;

                        while (_reader.BeginElement())
                        {
                            if (_reader.Name.Equals("skin", false)) _skins.Add(ParseSkin(id));

                            _reader.EndElement();
                        }
                    }

                    _reader.EndElement();
                }
            }

            private SkinEntry ParseSkin(string id)
            {
                var skin = new SkinEntry
                {
                    _id = id
                };

                while (_reader.ReadAttribute())
                    if (_reader.Name.Equals("source", false))
                        skin._skinSource = _reader.Value[0] == '#'
                            ? (string) (_reader.Value + 1)
                            : (string) _reader.Value;

                while (_reader.BeginElement())
                {
                    if (_reader.Name.Equals("bind_shape_matrix", false))
                    {
                        skin._bindMatrix = ParseMatrix();
                    }
                    else if (_reader.Name.Equals("source", false))
                    {
                        skin._sources.Add(ParseSource());
                    }
                    else if (_reader.Name.Equals("joints", false))
                    {
                        while (_reader.BeginElement())
                        {
                            if (_reader.Name.Equals("input", false)) skin._jointInputs.Add(ParseInput());

                            _reader.EndElement();
                        }
                    }
                    else if (_reader.Name.Equals("vertex_weights", false))
                    {
                        while (_reader.ReadAttribute())
                            if (_reader.Name.Equals("count", false))
                                skin._weightCount = int.Parse((string) _reader.Value);

                        skin._weights = new int[skin._weightCount][];

                        while (_reader.BeginElement())
                        {
                            if (_reader.Name.Equals("input", false))
                                skin._weightInputs.Add(ParseInput());
                            else if (_reader.Name.Equals("vcount", false))
                                for (var i = 0; i < skin._weightCount; i++)
                                {
                                    int val;
                                    _reader.ReadValue(&val);
                                    skin._weights[i] = new int[val * skin._weightInputs.Count];
                                }
                            else if (_reader.Name.Equals("v", false))
                                for (var i = 0; i < skin._weightCount; i++)
                                {
                                    var weights = skin._weights[i];
                                    fixed (int* p = weights)
                                    {
                                        for (var x = 0; x < weights.Length; x++) _reader.ReadValue(&p[x]);
                                    }
                                }

                            _reader.EndElement();
                        }
                    }

                    _reader.EndElement();
                }

                return skin;
            }

            private void ParseLibNodes()
            {
                while (_reader.BeginElement())
                {
                    if (_reader.Name.Equals("node", true)) _nodes.Add(ParseNode());

                    _reader.EndElement();
                }
            }

            private void ParseLibScenes()
            {
                while (_reader.BeginElement())
                {
                    if (_reader.Name.Equals("visual_scene", true)) _scenes.Add(ParseScene());

                    _reader.EndElement();
                }
            }

            private SceneEntry ParseScene()
            {
                var sc = new SceneEntry();

                while (_reader.ReadAttribute())
                    if (_reader.Name.Equals("id", true))
                        sc._id = (string) _reader.Value;

                while (_reader.ReadAttribute())
                    if (_reader.Name.Equals("name", true))
                        sc._name = (string) _reader.Value;

                while (_reader.BeginElement())
                {
                    if (_reader.Name.Equals("node", true)) sc._nodes.Add(ParseNode());

                    _reader.EndElement();
                }

                return sc;
            }

            private NodeEntry ParseNode()
            {
                var node = new NodeEntry();

                while (_reader.ReadAttribute())
                    if (_reader.Name.Equals("id", true))
                        node._id = (string) _reader.Value;
                    else if (_reader.Name.Equals("name", true))
                        node._name = (string) _reader.Value;
                    else if (_reader.Name.Equals("sid", true))
                        node._sid = (string) _reader.Value;
                    else if (_reader.Name.Equals("type", true))
                        node._type = (NodeType) Enum.Parse(typeof(NodeType), (string) _reader.Value, true);

                var m = Matrix.Identity;
                var mInv = Matrix.Identity;
                while (_reader.BeginElement())
                {
                    if (_reader.Name.Equals("matrix", true))
                    {
                        var matrix = ParseMatrix();
                        m *= matrix;
                        //mInv *= matrix.Invert();
                    }
                    else if (_reader.Name.Equals("rotate", true))
                    {
                        var v = ParseVec4();
                        m *= Matrix.RotationMatrix(v._x * v._w, v._y * v._w, v._z * v._w);
                        //mInv *= Matrix.ReverseRotationMatrix(v._x * v._w, v._y * v._w, v._z * v._w);
                    }
                    else if (_reader.Name.Equals("scale", true))
                    {
                        var scale = ParseVec3();
                        m *= Matrix.ScaleMatrix(scale);
                        //mInv *= Matrix.ScaleMatrix(1.0f / scale);
                    }
                    else if (_reader.Name.Equals("translate", true))
                    {
                        var translate = ParseVec3();
                        m *= Matrix.TranslationMatrix(translate);
                        //mInv *= Matrix.TranslationMatrix(-translate);
                    }
                    else if (_reader.Name.Equals("node", true))
                    {
                        node._children.Add(ParseNode());
                    }
                    else if (_reader.Name.Equals("instance_controller", true))
                    {
                        node._instances.Add(ParseInstance(InstanceType.Controller));
                    }
                    else if (_reader.Name.Equals("instance_geometry", true))
                    {
                        node._instances.Add(ParseInstance(InstanceType.Geometry));
                    }
                    else if (_reader.Name.Equals("instance_node", true))
                    {
                        node._instances.Add(ParseInstance(InstanceType.Node));
                    }
                    else if (_reader.Name.Equals("extra", true))
                    {
                        while (_reader.BeginElement())
                        {
                            if (_reader.Name.Equals("technique", true))
                                while (_reader.BeginElement())
                                {
                                    if (_reader.Name.Equals("visibility", true))
                                    {
                                    }

                                    _reader.EndElement();
                                }

                            _reader.EndElement();
                        }
                    }

                    _reader.EndElement();
                }

                node._matrix = m;
                node._invMatrix = mInv;
                return node;
            }

            private InstanceEntry ParseInstance(InstanceType type)
            {
                var c = new InstanceEntry
                {
                    _type = type
                };

                while (_reader.ReadAttribute())
                    if (_reader.Name.Equals("url", true))
                        c._url = _reader.Value[0] == '#' ? (string) (_reader.Value + 1) : (string) _reader.Value;

                while (_reader.BeginElement())
                {
                    if (_reader.Name.Equals("skeleton", true))
                        c.skeletons.Add(_reader.Value[0] == '#'
                            ? (string) (_reader.Value + 1)
                            : (string) _reader.Value);

                    if (_reader.Name.Equals("bind_material", true))
                        while (_reader.BeginElement())
                        {
                            if (_reader.Name.Equals("technique_common", true))
                                while (_reader.BeginElement())
                                {
                                    if (_reader.Name.Equals("instance_material", true))
                                        c._material = ParseMatInstance();

                                    _reader.EndElement();
                                }

                            _reader.EndElement();
                        }

                    _reader.EndElement();
                }

                return c;
            }

            private InstanceMaterial ParseMatInstance()
            {
                var mat = new InstanceMaterial();

                while (_reader.ReadAttribute())
                    if (_reader.Name.Equals("symbol", true))
                        mat._symbol = (string) _reader.Value;
                    else if (_reader.Name.Equals("target", true))
                        mat._target = _reader.Value[0] == '#' ? (string) (_reader.Value + 1) : (string) _reader.Value;

                while (_reader.BeginElement())
                {
                    if (_reader.Name.Equals("bind_vertex_input", true)) mat._vertexBinds.Add(ParseVertexInput());

                    _reader.EndElement();
                }

                return mat;
            }

            private VertexBind ParseVertexInput()
            {
                var v = new VertexBind();

                while (_reader.ReadAttribute())
                    if (_reader.Name.Equals("semantic", true))
                        v._semantic = (string) _reader.Value;
                    else if (_reader.Name.Equals("input_semantic", true))
                        v._inputSemantic = (string) _reader.Value;
                    else if (_reader.Name.Equals("input_set", true)) v._inputSet = int.Parse((string) _reader.Value);

                return v;
            }

            private Matrix ParseMatrix()
            {
                Matrix m;
                var pM = (float*) &m;
                for (var y = 0; y < 4; y++)
                for (var x = 0; x < 4; x++)
                    _reader.ReadValue(&pM[x * 4 + y]);

                return m;
            }

            private RGBAPixel ParseColor()
            {
                float f;
                RGBAPixel c;
                var p = (byte*) &c;
                for (var i = 0; i < 4; i++)
                    if (!_reader.ReadValue(&f))
                        p[i] = 255;
                    else
                        p[i] = (byte) (f * 255.0f + 0.5f);
                return c;
            }

            private Vector3 ParseVec3()
            {
                float f;
                Vector3 c;
                var p = (float*) &c;
                for (var i = 0; i < 3; i++)
                    if (!_reader.ReadValue(&f))
                        p[i] = 0;
                    else
                        p[i] = f;
                return c;
            }

            private Vector4 ParseVec4()
            {
                float f;
                Vector4 c;
                var p = (float*) &c;
                for (var i = 0; i < 4; i++)
                    if (!_reader.ReadValue(&f))
                        p[i] = 0;
                    else
                        p[i] = f;
                return c;
            }
        }

        private class ColladaEntry : IDisposable
        {
            internal string _id, _name, _sid;
            internal object _node;

            public virtual void Dispose()
            {
                GC.SuppressFinalize(this);
            }

            ~ColladaEntry()
            {
                Dispose();
            }
        }

        private class ImageEntry : ColladaEntry
        {
            internal string _path;
        }

        private class MaterialEntry : ColladaEntry
        {
            internal string _effect;
        }

        private class EffectEntry : ColladaEntry
        {
            internal readonly List<EffectNewParam> _newParams = new List<EffectNewParam>();
            internal EffectShaderEntry _shader;
        }

        private class GeometryEntry : ColladaEntry
        {
            internal int _faces = 0, _lines = 0;
            internal readonly List<PrimitiveEntry> _primitives = new List<PrimitiveEntry>();
            internal readonly List<SourceEntry> _sources = new List<SourceEntry>();

            internal string _verticesId;
            internal InputEntry _verticesInput;

            public override void Dispose()
            {
                foreach (var p in _sources) p.Dispose();

                GC.SuppressFinalize(this);
            }
        }

        private class SourceEntry : ColladaEntry
        {
            internal int _accessorCount;

            internal string _accessorSource;
            internal int _accessorStride;
            internal int _arrayCount;
            internal object _arrayData; //Parser must dispose!
            internal string _arrayDataString;
            internal string _arrayId;
            internal SourceType _arrayType;

            public override void Dispose()
            {
                if (_arrayData is UnsafeBuffer) ((UnsafeBuffer) _arrayData).Dispose();

                _arrayData = null;
                GC.SuppressFinalize(this);
            }
        }

        private class InputEntry : ColladaEntry
        {
            internal int _offset;
            internal int _outputOffset;
            internal SemanticType _semantic;
            internal int _set;
            internal string _source;
        }

        private class PrimitiveEntry
        {
            internal int _entryCount;
            internal int _entryStride;
            internal int _faceCount, _pointCount;

            internal readonly List<PrimitiveFace> _faces = new List<PrimitiveFace>();

            internal readonly List<InputEntry> _inputs = new List<InputEntry>();

            internal string _material;
            internal ColladaPrimitiveType _type;
        }

        private class PrimitiveFace
        {
            internal int _faceCount;
            internal int _pointCount;
            internal ushort[] _pointIndices;
        }

        private class SkinEntry : ColladaEntry
        {
            internal Matrix _bindMatrix = Matrix.Identity;
            internal readonly List<InputEntry> _jointInputs = new List<InputEntry>();
            internal string _skinSource;

            //internal Matrix _bindShape;
            internal readonly List<SourceEntry> _sources = new List<SourceEntry>();
            internal int _weightCount;

            internal readonly List<InputEntry> _weightInputs = new List<InputEntry>();
            internal int[][] _weights;

            public override void Dispose()
            {
                foreach (var src in _sources) src.Dispose();

                GC.SuppressFinalize(this);
            }
        }

        private class SceneEntry : ColladaEntry
        {
            internal readonly List<NodeEntry> _nodes = new List<NodeEntry>();

            public NodeEntry FindNode(string name)
            {
                NodeEntry n;
                foreach (var node in _nodes)
                    if ((n = DecoderShell.FindNodeInternal(name, node)) != null)
                        return n;

                return null;
            }
        }

        private class NodeEntry : ColladaEntry
        {
            internal readonly List<NodeEntry> _children = new List<NodeEntry>();
            internal readonly List<InstanceEntry> _instances = new List<InstanceEntry>();
            internal Matrix _invMatrix = Matrix.Identity;
            internal Matrix _matrix = Matrix.Identity;
            internal NodeType _type = NodeType.NONE;
        }

        private enum InstanceType
        {
            Controller,
            Geometry,
            Node
        }

        private class InstanceEntry : ColladaEntry
        {
            internal InstanceMaterial _material;
            internal InstanceType _type;
            internal string _url;
            internal readonly List<string> skeletons = new List<string>();
        }

        private class InstanceMaterial : ColladaEntry
        {
            internal string _symbol, _target;
            internal readonly List<VertexBind> _vertexBinds = new List<VertexBind>();
        }

        private class VertexBind : ColladaEntry
        {
            internal string _inputSemantic;
            internal int _inputSet;
            internal string _semantic;
        }

        private class EffectSampler2D
        {
            public string _minFilter, _magFilter;
            public string _source;
            public string _url;
            public string _wrapS, _wrapT;
        }

        private class EffectNewParam : ColladaEntry
        {
            public string _path;
            public EffectSampler2D _sampler2D;
        }

        private class EffectShaderEntry : ColladaEntry
        {
            internal readonly List<LightEffectEntry> _effects = new List<LightEffectEntry>();
            internal float _shininess, _reflectivity, _transparency;
            internal ShaderType _type;
        }

        private class LightEffectEntry : ColladaEntry
        {
            internal RGBAPixel _color;
            internal string _texCoord;

            internal string _texture;
            internal LightEffectType _type;
        }

        private enum ShaderType
        {
            None,
            phong,
            lambert,
            blinn
        }

        private enum LightEffectType
        {
            None,
            ambient,
            diffuse,
            emission,
            reflective,
            specular,
            transparent
        }

        private enum ColladaPrimitiveType
        {
            None,
            polygons,
            polylist,
            triangles,
            trifans,
            tristrips,
            lines,
            linestrips
        }

        private enum SemanticType
        {
            None,
            POSITION,
            VERTEX,
            NORMAL,
            TEXCOORD,
            COLOR,
            WEIGHT,
            JOINT,
            INV_BIND_MATRIX,
            TEXTANGENT,
            TEXBINORMAL
        }

        private enum SourceType
        {
            None,
            Float,
            Int,
            Name
        }

        private enum NodeType
        {
            NODE,
            JOINT,
            NONE
        }
    }
}