using BrawlLib.Imaging;
using BrawlLib.Internal;
using BrawlLib.SSBB.ResourceNodes;
using BrawlLib.Wii.Animations;
using BrawlLib.Wii.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Windows.Forms;
using System.Xml;

namespace BrawlLib.Modeling.Collada
{
    public unsafe partial class Collada
    {
        private static readonly XmlWriterSettings _writerSettings = new XmlWriterSettings
            {Indent = true, IndentChars = "\t", NewLineChars = "\r\n", NewLineHandling = NewLineHandling.Replace};

        public static void Serialize(MDL0Node model, string outFile)
        {
            model.Populate();
            model.ApplyCHR(null, 0);

            using (FileStream stream = new FileStream(outFile, FileMode.Create, FileAccess.ReadWrite, FileShare.None,
                0x1000, FileOptions.SequentialScan))
            {
                using (XmlWriter writer = XmlWriter.Create(stream, _writerSettings))
                {
                    writer.Flush();
                    stream.Position = 0;

                    writer.WriteStartDocument();
                    writer.WriteStartElement("COLLADA", "http://www.collada.org/2008/03/COLLADASchema");
                    writer.WriteAttributeString("version", "1.5.0");

                    writer.WriteStartElement("asset");
                    {
                        writer.WriteStartElement("contributor");
                        writer.WriteElementString("authoring_tool", Application.ProductName);
                        writer.WriteEndElement();

                        writer.WriteStartElement("created");
                        writer.WriteString(DateTime.UtcNow.ToString("s", CultureInfo.InvariantCulture) + "Z");
                        writer.WriteEndElement();

                        writer.WriteStartElement("modified");
                        writer.WriteString(DateTime.UtcNow.ToString("s", CultureInfo.InvariantCulture) + "Z");
                        writer.WriteEndElement();

                        writer.WriteStartElement("unit");
                        writer.WriteAttributeString("meter", "0.01");
                        writer.WriteAttributeString("name", "centimeter");
                        writer.WriteEndElement();

                        writer.WriteElementString("up_axis", "Y_UP");
                    }
                    writer.WriteEndElement();

                    //Define images
                    WriteImages(model, writer);

                    //Define materials
                    WriteMaterials(model, writer);

                    //Define effects
                    WriteEffects(model, writer);

                    //Define geometry
                    //Create a geometry object for each polygon
                    WriteGeometry(model, writer);

                    //Define controllers
                    //Each weighted polygon needs a controller, which assigns weights to each vertex.
                    WriteControllers(model, writer);

                    //Define scenes
                    writer.WriteStartElement("library_visual_scenes");
                    {
                        writer.WriteStartElement("visual_scene");
                        {
                            //Attach nodes/bones to scene, starting with TopN
                            //Specify transform for each node
                            //Weighted polygons must use instance_controller
                            //Standard geometry uses instance_geometry

                            writer.WriteAttributeString("id", "RootNode");
                            writer.WriteAttributeString("name", "RootNode");

                            //Define bones and geometry instances
                            WriteNodes(model, writer);
                        }
                        writer.WriteEndElement();
                    }
                    writer.WriteEndElement();

                    writer.WriteStartElement("scene");
                    {
                        writer.WriteStartElement("instance_visual_scene");
                        writer.WriteAttributeString("url", "#RootNode");
                        writer.WriteEndElement(); //instance visual scene
                    }
                    writer.WriteEndElement(); //scene
                }
            }
        }

        private static void WriteImages(MDL0Node model, XmlWriter writer)
        {
            if (model._texList == null)
            {
                return;
            }

            writer.WriteStartElement("library_images");
            {
                foreach (MDL0TextureNode tex in model._texList)
                {
                    writer.WriteStartElement("image");
                    {
                        writer.WriteAttributeString("id", tex.Name + "-image");
                        writer.WriteAttributeString("name", tex.Name);
                        writer.WriteStartElement("init_from");
                        {
                            writer.WriteStartElement("ref");
                            {
                                writer.WriteString($"{tex.Name}.png");
                            }
                            writer.WriteEndElement(); //ref
                        }
                        writer.WriteEndElement(); //init_from
                    }
                    writer.WriteEndElement(); //image
                }
            }
            writer.WriteEndElement(); //library_images
        }

        private static void WriteMaterials(MDL0Node model, XmlWriter writer)
        {
            ResourceNode node = model._matGroup;
            if (node == null)
            {
                return;
            }

            writer.WriteStartElement("library_materials");
            {
                foreach (MDL0MaterialNode mat in node.Children)
                {
                    if (mat._objects.Count == 0)
                    {
                        continue;
                    }

                    writer.WriteStartElement("material");
                    {
                        writer.WriteAttributeString("id", mat._name);

                        writer.WriteStartElement("instance_effect");
                        writer.WriteAttributeString("url", $"#{mat._name}-fx");
                        writer.WriteEndElement();
                    }
                    writer.WriteEndElement();
                }
            }
            writer.WriteEndElement();
        }

        private static void WriteEffects(MDL0Node model, XmlWriter writer)
        {
            ResourceNode node = model._matGroup;
            if (node == null)
            {
                return;
            }

            writer.WriteStartElement("library_effects");

            foreach (MDL0MaterialNode mat in node.Children)
            {
                writer.WriteStartElement("effect");
                {
                    writer.WriteAttributeString("id", mat._name + "-fx");
                    writer.WriteAttributeString("name", mat._name);
                    writer.WriteStartElement("profile_COMMON");
                    {
                        if (mat.Children.Count > 0)
                        {
                            foreach (MDL0MaterialRefNode mr in mat.Children)
                            {
                                if (string.IsNullOrEmpty(mr._texture?.Name))
                                {
                                    continue;
                                }

                                writer.WriteStartElement("newparam");
                                writer.WriteAttributeString("sid", mr._texture.Name + "-surface");
                                {
                                    writer.WriteStartElement("surface");
                                    writer.WriteAttributeString("type", "2D");
                                    {
                                        writer.WriteStartElement("init_from");
                                        {
                                            writer.WriteStartElement("ref");
                                            {
                                                writer.WriteString(mr._texture.Name + "-image");
                                            }
                                            writer.WriteEndElement(); //ref
                                        }
                                        writer.WriteEndElement(); //init_from
                                    }
                                    writer.WriteEndElement(); //surface
                                }
                                writer.WriteEndElement(); //newparam

                                writer.WriteStartElement("newparam");
                                writer.WriteAttributeString("sid", mr._texture.Name + "-sampler");
                                {
                                    writer.WriteStartElement("sampler2D");
                                    {
                                        writer.WriteStartElement("source");
                                        {
                                            writer.WriteString(mr._texture.Name + "-surface");
                                        }
                                        writer.WriteEndElement(); //source
                                        writer.WriteStartElement("instance_image");
                                        writer.WriteAttributeString("url", "#" + mr._texture.Name + "-image");
                                        writer.WriteEndElement(); //instance_image
                                        string wrap = "WRAP";
                                        writer.WriteStartElement("wrap_s");
                                        switch (mr._uWrap)
                                        {
                                            case 0:
                                                wrap = "CLAMP";
                                                break;
                                            case 1:
                                                wrap = "WRAP";
                                                break;
                                            case 2:
                                                wrap = "MIRROR";
                                                break;
                                        }

                                        writer.WriteString(wrap);
                                        writer.WriteEndElement(); //wrap_s
                                        wrap = "REPEAT";
                                        writer.WriteStartElement("wrap_t");
                                        switch (mr._vWrap)
                                        {
                                            case 0:
                                                wrap = "CLAMP";
                                                break;
                                            case 1:
                                                wrap = "WRAP";
                                                break;
                                            case 2:
                                                wrap = "MIRROR";
                                                break;
                                        }

                                        writer.WriteString(wrap);
                                        writer.WriteEndElement(); //wrap_t
                                        writer.WriteStartElement("minfilter");
                                        writer.WriteString(mr.MinFilter.ToString().ToUpper());
                                        writer.WriteEndElement(); //minfilter
                                        writer.WriteStartElement("magfilter");
                                        writer.WriteString(mr.MagFilter.ToString().ToUpper());
                                        writer.WriteEndElement(); //magfilter
                                    }
                                    writer.WriteEndElement(); //sampler2D
                                }
                                writer.WriteEndElement(); //newparam
                            }
                        }

                        writer.WriteStartElement("technique");
                        writer.WriteAttributeString("sid", "COMMON");
                        {
                            writer.WriteStartElement("phong");
                            {
                                writer.WriteStartElement("diffuse");
                                {
                                    if (mat.Children.Count > 0)
                                    {
                                        MDL0MaterialRefNode mr = mat.Children[0] as MDL0MaterialRefNode;
                                        if (mr._texture != null)
                                        {
                                            writer.WriteStartElement("texture");
                                            {
                                                writer.WriteAttributeString("texture", mr._texture.Name + "-sampler");
                                                writer.WriteAttributeString("texcoord",
                                                    "TEXCOORD" + (mr.TextureCoordId < 0 ? 0 : mr.TextureCoordId));
                                            }
                                            writer.WriteEndElement(); //texture
                                        }
                                    }
                                }
                                writer.WriteEndElement(); //diffuse
                            }
                            writer.WriteEndElement(); //phong
                        }
                        writer.WriteEndElement(); //technique
                    }
                    writer.WriteEndElement(); //profile
                }
                writer.WriteEndElement(); //effect
            }

            writer.WriteEndElement(); //library
        }

        private static void WriteGeometry(MDL0Node model, XmlWriter writer)
        {
            ResourceNode grp = model._objGroup;
            if (grp == null)
            {
                return;
            }

            writer.WriteStartElement("library_geometries");

            foreach (MDL0ObjectNode poly in grp.Children)
            {
                PrimitiveManager manager = poly._manager;

                //Geometry
                writer.WriteStartElement("geometry");
                writer.WriteAttributeString("id", poly.Name);
                writer.WriteAttributeString("name", poly.Name);

                //Mesh
                writer.WriteStartElement("mesh");

                //Write vertex data first
                WriteVertices(poly.Name, manager._vertices, poly.MatrixNode, writer);

                //Face assets
                for (int i = 0; i < 12; i++)
                {
                    if (manager._faceData[i] == null)
                    {
                        continue;
                    }

                    switch (i)
                    {
                        case 0:
                            break;

                        case 1:
                            WriteNormals(poly.Name, writer, manager);
                            break;

                        case 2:
                        case 3:
                            WriteColors(poly.Name, manager, i - 2, writer);
                            break;

                        default:
                            WriteUVs(poly.Name, manager, i - 4, writer);
                            break;
                    }
                }

                //Vertices
                writer.WriteStartElement("vertices");
                writer.WriteAttributeString("id", poly.Name + "_Vertices");
                writer.WriteStartElement("input");
                writer.WriteAttributeString("semantic", "POSITION");
                writer.WriteAttributeString("source", "#" + poly.Name + "_Positions");
                writer.WriteEndElement(); //input
                writer.WriteEndElement(); //vertices

                //Faces
                foreach (DrawCall c in poly._drawCalls)
                {
                    if (manager._triangles != null)
                    {
                        WritePrimitive(poly, c.MaterialNode, manager._triangles, writer);
                    }

                    if (manager._lines != null)
                    {
                        WritePrimitive(poly, c.MaterialNode, manager._lines, writer);
                    }

                    if (manager._points != null)
                    {
                        WritePrimitive(poly, c.MaterialNode, manager._points, writer);
                    }
                }

                writer.WriteEndElement(); //mesh
                writer.WriteEndElement(); //geometry
            }

            writer.WriteEndElement();
        }

        private static void WriteVertices(string name, List<Vertex3> vertices, IMatrixNode singleBind, XmlWriter writer)
        {
            bool first = true;

            //Position source
            writer.WriteStartElement("source");
            writer.WriteAttributeString("id", name + "_Positions");

            //Array start
            writer.WriteStartElement("float_array");
            writer.WriteAttributeString("id", name + "_PosArr");
            writer.WriteAttributeString("count", (vertices.Count * 3).ToString());

            foreach (Vertex3 v in vertices)
            {
                if (first)
                {
                    first = false;
                }
                else
                {
                    writer.WriteString(" ");
                }

                Vector3 p = v.WeightedPosition;
                writer.WriteString(
                    $"{p._x.ToString(CultureInfo.InvariantCulture.NumberFormat)} {p._y.ToString(CultureInfo.InvariantCulture.NumberFormat)} {p._z.ToString(CultureInfo.InvariantCulture.NumberFormat)}");
            }

            writer.WriteEndElement(); //float_array

            //Technique
            writer.WriteStartElement("technique_common");

            writer.WriteStartElement("accessor");
            writer.WriteAttributeString("source", "#" + name + "_PosArr");
            writer.WriteAttributeString("count", vertices.Count.ToString());
            writer.WriteAttributeString("stride", "3");

            writer.WriteStartElement("param");
            writer.WriteAttributeString("name", "X");
            writer.WriteAttributeString("type", "float");
            writer.WriteEndElement(); //param
            writer.WriteStartElement("param");
            writer.WriteAttributeString("name", "Y");
            writer.WriteAttributeString("type", "float");
            writer.WriteEndElement(); //param
            writer.WriteStartElement("param");
            writer.WriteAttributeString("name", "Z");
            writer.WriteAttributeString("type", "float");
            writer.WriteEndElement(); //param

            writer.WriteEndElement(); //accessor
            writer.WriteEndElement(); //technique_common

            writer.WriteEndElement(); //source
        }

        private static Vector3[] _normals;
        private static List<int> _normRemap;

        private static void WriteNormals(string name, XmlWriter writer, PrimitiveManager p)
        {
            bool first = true;
            ushort* pIndex = (ushort*) p._indices.Address;
            Vector3* pData = (Vector3*) p._faceData[1].Address;
            Vector3 v;

            HashSet<Vector3> list = new HashSet<Vector3>();
            for (int i = 0; i < p._pointCount; i++)
            {
                if (p._vertices.Count > pIndex[i] && pData[i] != null)
                {
                    list.Add(p._vertices[pIndex[i]].GetMatrix().GetRotationMatrix() * pData[i]);
                }
            }

            if (list.Count <= 0)
            {
                return; // No normals to write; This likely should never happen, but prevents crashes if it does
            }

            _normals = new Vector3[list.Count];
            list.CopyTo(_normals);

            int count = _normals.Length;
            _normRemap = new List<int>();
            for (int i = 0; i < p._pointCount; i++)
            {
                if (p._vertices.Count > pIndex[i])
                {
                    _normRemap.Add(Array.IndexOf((Array) _normals,
                        p._vertices[pIndex[i]].GetMatrix().GetRotationMatrix() * pData[i]));
                }
            }

            //Position source
            writer.WriteStartElement("source");
            writer.WriteAttributeString("id", name + "_Normals");

            //Array start
            writer.WriteStartElement("float_array");
            writer.WriteAttributeString("id", name + "_NormArr");
            writer.WriteAttributeString("count", (count * 3).ToString());

            for (int i = 0; i < count; i++)
            {
                if (first)
                {
                    first = false;
                }
                else
                {
                    writer.WriteString(" ");
                }

                v = _normals[i];

                writer.WriteString(
                    $"{v._x.ToString(CultureInfo.InvariantCulture.NumberFormat)} {v._y.ToString(CultureInfo.InvariantCulture.NumberFormat)} {v._z.ToString(CultureInfo.InvariantCulture.NumberFormat)}");
            }

            writer.WriteEndElement(); //float_array

            //Technique
            writer.WriteStartElement("technique_common");

            writer.WriteStartElement("accessor");
            writer.WriteAttributeString("source", "#" + name + "_NormArr");
            writer.WriteAttributeString("count", count.ToString());
            writer.WriteAttributeString("stride", "3");

            writer.WriteStartElement("param");
            writer.WriteAttributeString("name", "X");
            writer.WriteAttributeString("type", "float");
            writer.WriteEndElement(); //param
            writer.WriteStartElement("param");
            writer.WriteAttributeString("name", "Y");
            writer.WriteAttributeString("type", "float");
            writer.WriteEndElement(); //param
            writer.WriteStartElement("param");
            writer.WriteAttributeString("name", "Z");
            writer.WriteAttributeString("type", "float");
            writer.WriteEndElement(); //param

            writer.WriteEndElement(); //accessor
            writer.WriteEndElement(); //technique_common

            writer.WriteEndElement(); //source
        }

        private static readonly RGBAPixel[][] _colors = new RGBAPixel[2][];
        private static readonly List<int>[] _colorRemap = new List<int>[2];
        private const float cFactor = 1.0f / 255.0f;

        private static void WriteColors(string name, PrimitiveManager p, int set, XmlWriter writer)
        {
            bool first = true;

            _colors[set] = p.GetColors(set, true);
            int count = _colors[set].Length;
            _colorRemap[set] = new List<int>();
            RGBAPixel* ptr = (RGBAPixel*) p._faceData[set + 2].Address;
            for (int i = 0; i < p._pointCount; i++)
            {
                _colorRemap[set].Add(Array.IndexOf(_colors[set], ptr[i]));
            }

            //Position source
            writer.WriteStartElement("source");
            writer.WriteAttributeString("id", name + "_Colors" + set);

            //Array start
            writer.WriteStartElement("float_array");
            writer.WriteAttributeString("id", name + "_ColorArr" + set);
            writer.WriteAttributeString("count", (count * 4).ToString());

            for (int i = 0; i < count; i++)
            {
                if (first)
                {
                    first = false;
                }
                else
                {
                    writer.WriteString(" ");
                }

                RGBAPixel r = _colors[set][i];

                writer.WriteString(
                    $"{(r.R * cFactor).ToString(CultureInfo.InvariantCulture.NumberFormat)} {(r.G * cFactor).ToString(CultureInfo.InvariantCulture.NumberFormat)} {(r.B * cFactor).ToString(CultureInfo.InvariantCulture.NumberFormat)} {(r.A * cFactor).ToString(CultureInfo.InvariantCulture.NumberFormat)}");
            }

            writer.WriteEndElement(); //int_array

            //Technique
            writer.WriteStartElement("technique_common");

            writer.WriteStartElement("accessor");
            writer.WriteAttributeString("source", "#" + name + "_ColorArr" + set);
            writer.WriteAttributeString("count", count.ToString());
            writer.WriteAttributeString("stride", "4");

            writer.WriteStartElement("param");
            writer.WriteAttributeString("name", "R");
            writer.WriteAttributeString("type", "float");
            writer.WriteEndElement(); //param
            writer.WriteStartElement("param");
            writer.WriteAttributeString("name", "G");
            writer.WriteAttributeString("type", "float");
            writer.WriteEndElement(); //param
            writer.WriteStartElement("param");
            writer.WriteAttributeString("name", "B");
            writer.WriteAttributeString("type", "float");
            writer.WriteEndElement(); //param
            writer.WriteStartElement("param");
            writer.WriteAttributeString("name", "A");
            writer.WriteAttributeString("type", "float");
            writer.WriteEndElement(); //param

            writer.WriteEndElement(); //accessor
            writer.WriteEndElement(); //technique_common

            writer.WriteEndElement(); //source
        }

        private static readonly Vector2[][] _uvs = new Vector2[8][];
        private static readonly List<int>[] _uvRemap = new List<int>[8];

        private static void WriteUVs(string name, PrimitiveManager p, int set, XmlWriter writer)
        {
            bool first = true;

            _uvs[set] = p.GetUVs(set, true);
            int count = _uvs[set].Length;
            _uvRemap[set] = new List<int>();
            Vector2* ptr = (Vector2*) p._faceData[set + 4].Address;
            for (int i = 0; i < p._pointCount; i++)
            {
                _uvRemap[set].Add(Array.IndexOf(_uvs[set], ptr[i]));
            }

            //Position source
            writer.WriteStartElement("source");
            writer.WriteAttributeString("id", name + "_UVs" + set);

            //Array start
            writer.WriteStartElement("float_array");
            writer.WriteAttributeString("id", name + "_UVArr" + set);
            writer.WriteAttributeString("count", (count * 2).ToString());

            for (int i = 0; i < count; i++)
            {
                if (first)
                {
                    first = false;
                }
                else
                {
                    writer.WriteString(" ");
                }

                //Reverse T component to a top-down form
                //writer.WriteString(String.Format("{0} {1}", pData->_x, 1.0 - pData->_y));
                //pData++;
                writer.WriteString(
                    $"{_uvs[set][i]._x.ToString(CultureInfo.InvariantCulture.NumberFormat)} {(1.0f - _uvs[set][i]._y).ToString(CultureInfo.InvariantCulture.NumberFormat)}");
            }

            writer.WriteEndElement(); //int_array

            //Technique
            writer.WriteStartElement("technique_common");

            writer.WriteStartElement("accessor");
            writer.WriteAttributeString("source", "#" + name + "_UVArr" + set);
            writer.WriteAttributeString("count", count.ToString());
            writer.WriteAttributeString("stride", "2");

            writer.WriteStartElement("param");
            writer.WriteAttributeString("name", "S");
            writer.WriteAttributeString("type", "float");
            writer.WriteEndElement(); //param
            writer.WriteStartElement("param");
            writer.WriteAttributeString("name", "T");
            writer.WriteAttributeString("type", "float");
            writer.WriteEndElement(); //param

            writer.WriteEndElement(); //accessor
            writer.WriteEndElement(); //technique_common

            writer.WriteEndElement(); //source
        }

        private static void WritePrimitive(MDL0ObjectNode poly, MDL0MaterialNode mat, GLPrimitive prim,
                                           XmlWriter writer)
        {
            PrimitiveManager manager = poly._manager;
            int count;
            int elements = 0, stride = 0;
            int set;
            bool first;
            uint[] pDataArr = prim._indices;
            uint pData = 0;
            ushort* pVert = (ushort*) poly._manager._indices.Address;

            switch (prim._type)
            {
                case OpenTK.Graphics.OpenGL.BeginMode.Triangles:
                    writer.WriteStartElement("triangles");
                    stride = 3;
                    break;

                case OpenTK.Graphics.OpenGL.BeginMode.Lines:
                    writer.WriteStartElement("lines");
                    stride = 2;
                    break;

                case OpenTK.Graphics.OpenGL.BeginMode.Points:
                    writer.WriteStartElement("points");
                    stride = 1;
                    break;
            }

            count = prim._indices.Length / stride;

            if (mat != null)
            {
                writer.WriteAttributeString("material", mat.Name);
            }

            writer.WriteAttributeString("count", count.ToString());

            List<int> elementType = new List<int>();
            for (int i = 0; i < 12; i++)
            {
                if (manager._faceData[i] == null)
                {
                    continue;
                }

                writer.WriteStartElement("input");

                switch (i)
                {
                    case 0:
                        writer.WriteAttributeString("semantic", "VERTEX");
                        writer.WriteAttributeString("source", "#" + poly._name + "_Vertices");
                        break;

                    case 1:
                        writer.WriteAttributeString("semantic", "NORMAL");
                        writer.WriteAttributeString("source", "#" + poly._name + "_Normals");
                        break;

                    case 2:
                    case 3:
                        set = i - 2;
                        writer.WriteAttributeString("semantic", "COLOR");
                        writer.WriteAttributeString("source", "#" + poly._name + "_Colors" + set);
                        writer.WriteAttributeString("set", set.ToString());
                        break;

                    default:
                        set = i - 4;
                        writer.WriteAttributeString("semantic", "TEXCOORD");
                        writer.WriteAttributeString("source", "#" + poly._name + "_UVs" + set);
                        writer.WriteAttributeString("set", set.ToString());
                        break;
                }

                writer.WriteAttributeString("offset", elements.ToString());
                writer.WriteEndElement(); //input

                elements++;
                elementType.Add(i);
            }

            writer.WriteStartElement("p");
            first = true;
            for (int i = 0; i < count; i++)
            {
                for (int x = 0; x < stride; x++)
                {
                    int index = (int) pDataArr[pData++];
                    for (int y = 0; y < elements; y++)
                    {
                        if (first)
                        {
                            first = false;
                        }
                        else
                        {
                            writer.WriteString(" ");
                        }

                        if (elementType[y] < 4)
                        {
                            if (elementType[y] < 2)
                            {
                                if (elementType[y] == 0)
                                {
                                    writer.WriteString(pVert[index]
                                        .ToString(CultureInfo.InvariantCulture.NumberFormat));
                                }
                                else
                                {
                                    writer.WriteString(_normRemap[index]
                                        .ToString(CultureInfo.InvariantCulture.NumberFormat));
                                }
                            }
                            else
                            {
                                writer.WriteString(_colorRemap[elementType[y] - 2][index]
                                    .ToString(CultureInfo.InvariantCulture.NumberFormat));
                            }
                        }
                        else
                        {
                            writer.WriteString(_uvRemap[elementType[y] - 4][index]
                                .ToString(CultureInfo.InvariantCulture.NumberFormat));
                        }
                    }
                }
            }

            writer.WriteEndElement(); //p
            writer.WriteEndElement(); //primitive
        }

        private static void WriteControllers(MDL0Node model, XmlWriter writer)
        {
            if (model._objList == null)
            {
                return;
            }

            writer.WriteStartElement("library_controllers");

            MDL0BoneNode[] bones = model._linker.BoneCache;
            HashSet<float> tempWeights = new HashSet<float>();
            Matrix m;
            bool first;

            foreach (MDL0ObjectNode poly in model._objList)
            {
                List<Vertex3> verts = poly._manager._vertices;

                writer.WriteStartElement("controller");
                writer.WriteAttributeString("id", poly.Name + "_Controller");
                writer.WriteStartElement("skin");
                writer.WriteAttributeString("source", "#" + poly.Name);

                writer.WriteStartElement("bind_shape_matrix");

                //Multiply per vertex instead of per object when single bound
                //Set bind pose matrix
                //if (poly._singleBind != null)
                //    m = poly._singleBind.Matrix;
                //else
                m = Matrix.Identity;

                writer.WriteString(WriteMatrix(m));

                writer.WriteEndElement();

                //Get list of used bones and weights
                if (poly._matrixNode != null)
                {
                    foreach (BoneWeight w in poly._matrixNode.Weights)
                    {
                        tempWeights.Add(w.Weight);
                    }
                }
                else
                {
                    foreach (Vertex3 v in verts)
                    {
                        foreach (BoneWeight w in v.MatrixNode.Weights)
                        {
                            tempWeights.Add(w.Weight);
                        }
                    }
                }

                float[] weightSet = new float[tempWeights.Count];
                tempWeights.CopyTo(weightSet);

                //Write joint source
                writer.WriteStartElement("source");
                writer.WriteAttributeString("id", poly.Name + "_Joints");

                //Node array
                writer.WriteStartElement("Name_array");
                writer.WriteAttributeString("id", poly.Name + "_JointArr");
                writer.WriteAttributeString("count", bones.Length.ToString());

                first = true;
                foreach (MDL0BoneNode b in bones)
                {
                    if (first)
                    {
                        first = false;
                    }
                    else
                    {
                        writer.WriteString(" ");
                    }

                    writer.WriteString(b.Name);
                }

                writer.WriteEndElement(); //Name_array

                //Technique
                writer.WriteStartElement("technique_common");
                writer.WriteStartElement("accessor");
                writer.WriteAttributeString("source", $"#{poly.Name}_JointArr");
                writer.WriteAttributeString("count", bones.Length.ToString());
                writer.WriteStartElement("param");
                writer.WriteAttributeString("name", "JOINT");
                writer.WriteAttributeString("type", "Name");
                writer.WriteEndElement(); //param
                writer.WriteEndElement(); //accessor
                writer.WriteEndElement(); //technique

                writer.WriteEndElement(); //joint source

                //Inverse matrices source
                writer.WriteStartElement("source");
                writer.WriteAttributeString("id", poly.Name + "_Matrices");

                writer.WriteStartElement("float_array");
                writer.WriteAttributeString("id", poly.Name + "_MatArr");
                writer.WriteAttributeString("count", (bones.Length * 16).ToString());

                first = true;
                foreach (MDL0BoneNode b in bones)
                {
                    if (first)
                    {
                        first = false;
                    }
                    else
                    {
                        writer.WriteString(" ");
                    }

                    writer.WriteString(WriteMatrix(b.InverseBindMatrix));
                }

                writer.WriteEndElement(); //float_array

                //Technique
                writer.WriteStartElement("technique_common");
                writer.WriteStartElement("accessor");
                writer.WriteAttributeString("source", $"#{poly.Name}_MatArr");
                writer.WriteAttributeString("count", bones.Length.ToString());
                writer.WriteAttributeString("stride", "16");
                writer.WriteStartElement("param");
                writer.WriteAttributeString("type", "float4x4");
                writer.WriteEndElement(); //param
                writer.WriteEndElement(); //accessor
                writer.WriteEndElement(); //technique

                writer.WriteEndElement(); //source

                //Weights source
                writer.WriteStartElement("source");
                writer.WriteAttributeString("id", poly.Name + "_Weights");

                writer.WriteStartElement("float_array");
                writer.WriteAttributeString("id", poly.Name + "_WeightArr");
                writer.WriteAttributeString("count", weightSet.Length.ToString());
                first = true;

                foreach (float f in weightSet)
                {
                    if (first)
                    {
                        first = false;
                    }
                    else
                    {
                        writer.WriteString(" ");
                    }

                    writer.WriteValue(f);
                }

                writer.WriteEndElement();

                //Technique
                writer.WriteStartElement("technique_common");
                writer.WriteStartElement("accessor");
                writer.WriteAttributeString("source", $"#{poly.Name}_WeightArr");
                writer.WriteAttributeString("count", weightSet.Length.ToString());
                writer.WriteStartElement("param");
                writer.WriteAttributeString("type", "float");
                writer.WriteEndElement(); //param
                writer.WriteEndElement(); //accessor
                writer.WriteEndElement(); //technique

                writer.WriteEndElement(); //source

                //Joint bindings
                writer.WriteStartElement("joints");
                writer.WriteStartElement("input");
                writer.WriteAttributeString("semantic", "JOINT");
                writer.WriteAttributeString("source", $"#{poly.Name}_Joints");
                writer.WriteEndElement(); //input
                writer.WriteStartElement("input");
                writer.WriteAttributeString("semantic", "INV_BIND_MATRIX");
                writer.WriteAttributeString("source", $"#{poly.Name}_Matrices");
                writer.WriteEndElement(); //input
                writer.WriteEndElement(); //joints

                //Vertex weights, one for each vertex in geometry
                writer.WriteStartElement("vertex_weights");
                writer.WriteAttributeString("count", verts.Count.ToString());
                writer.WriteStartElement("input");
                writer.WriteAttributeString("semantic", "JOINT");
                writer.WriteAttributeString("offset", "0");
                writer.WriteAttributeString("source", $"#{poly.Name}_Joints");
                writer.WriteEndElement(); //input
                writer.WriteStartElement("input");
                writer.WriteAttributeString("semantic", "WEIGHT");
                writer.WriteAttributeString("offset", "1");
                writer.WriteAttributeString("source", $"#{poly.Name}_Weights");
                writer.WriteEndElement(); //input

                writer.WriteStartElement("vcount");
                first = true;
                if (poly._matrixNode != null)
                {
                    for (int i = 0; i < verts.Count; i++)
                    {
                        if (first)
                        {
                            first = false;
                        }
                        else
                        {
                            writer.WriteString(" ");
                        }

                        writer.WriteString(
                            poly._matrixNode.Weights.Count.ToString(CultureInfo.InvariantCulture.NumberFormat));
                    }
                }
                else
                {
                    foreach (Vertex3 v in verts)
                    {
                        if (first)
                        {
                            first = false;
                        }
                        else
                        {
                            writer.WriteString(" ");
                        }

                        writer.WriteString(
                            v.MatrixNode.Weights.Count.ToString(CultureInfo.InvariantCulture.NumberFormat));
                    }
                }

                writer.WriteEndElement(); //vcount

                writer.WriteStartElement("v");

                first = true;
                if (poly._matrixNode != null)
                {
                    for (int i = 0; i < verts.Count; i++)
                    {
                        foreach (BoneWeight w in poly._matrixNode.Weights)
                        {
                            if (first)
                            {
                                first = false;
                            }
                            else
                            {
                                writer.WriteString(" ");
                            }

                            writer.WriteString(Array.IndexOf(bones, w.Bone)
                                .ToString(CultureInfo.InvariantCulture.NumberFormat));
                            writer.WriteString(" ");
                            writer.WriteString(Array.IndexOf(weightSet, w.Weight)
                                .ToString(CultureInfo.InvariantCulture.NumberFormat));
                        }
                    }
                }
                else
                {
                    foreach (Vertex3 v in verts)
                    {
                        foreach (BoneWeight w in v.MatrixNode.Weights)
                        {
                            if (first)
                            {
                                first = false;
                            }
                            else
                            {
                                writer.WriteString(" ");
                            }

                            writer.WriteString(Array.IndexOf(bones, w.Bone)
                                .ToString(CultureInfo.InvariantCulture.NumberFormat));
                            writer.WriteString(" ");
                            writer.WriteString(Array.IndexOf(weightSet, w.Weight)
                                .ToString(CultureInfo.InvariantCulture.NumberFormat));
                        }
                    }
                }

                writer.WriteEndElement(); //v
                writer.WriteEndElement(); //vertex_weights
                writer.WriteEndElement(); //skin
                writer.WriteEndElement(); //controller
            }

            writer.WriteEndElement();
        }

        private static void WriteNodes(MDL0Node model, XmlWriter writer)
        {
            if (model._boneList != null)
            {
                foreach (MDL0BoneNode bone in model._boneList)
                {
                    WriteBone(bone, writer);
                }
            }

            if (model._objList != null)
            {
                foreach (MDL0ObjectNode poly in model._objList)
                {
                    //if (poly._singleBind == null)
                    foreach (DrawCall c in poly._drawCalls)
                    {
                        WritePolyInstance(c, writer);
                    }
                }
            }
        }

        private static void WriteBone(MDL0BoneNode bone, XmlWriter writer, bool useMatrix = false)
        {
            writer.WriteStartElement("node");
            writer.WriteAttributeString("id", bone.Name);
            writer.WriteAttributeString("name", bone._name);
            writer.WriteAttributeString("sid", bone.Name);
            writer.WriteAttributeString("type", "JOINT");

            if (useMatrix)
            {
                writer.WriteStartElement("matrix");
                writer.WriteString(WriteMatrix(bone._bindState._transform));
                writer.WriteEndElement(); //matrix
            }
            else
            {
                if (bone._bindState._translate != new Vector3())
                {
                    writer.WriteStartElement("translate");
                    writer.WriteString(
                        bone._bindState._translate._x.ToString(CultureInfo.InvariantCulture.NumberFormat) + " " +
                        bone._bindState._translate._y.ToString(CultureInfo.InvariantCulture.NumberFormat) + " " +
                        bone._bindState._translate._z.ToString(CultureInfo.InvariantCulture.NumberFormat));
                    writer.WriteEndElement(); //translate
                }

                if (bone._bindState._rotate._z != 0)
                {
                    writer.WriteStartElement("rotate");
                    writer.WriteString("0 0 1 " +
                                       bone._bindState._rotate._z.ToString(CultureInfo.InvariantCulture.NumberFormat));
                    writer.WriteEndElement(); //rotate
                }

                if (bone._bindState._rotate._y != 0)
                {
                    writer.WriteStartElement("rotate");
                    writer.WriteString("0 1 0 " +
                                       bone._bindState._rotate._y.ToString(CultureInfo.InvariantCulture.NumberFormat));
                    writer.WriteEndElement(); //rotate
                }

                if (bone._bindState._rotate._x != 0)
                {
                    writer.WriteStartElement("rotate");
                    writer.WriteString("1 0 0 " +
                                       bone._bindState._rotate._x.ToString(CultureInfo.InvariantCulture.NumberFormat));
                    writer.WriteEndElement(); //rotate
                }

                if (bone._bindState._scale != new Vector3(1))
                {
                    writer.WriteStartElement("scale");
                    writer.WriteString(
                        bone._bindState._scale._x.ToString(CultureInfo.InvariantCulture.NumberFormat) + " " +
                        bone._bindState._scale._y.ToString(CultureInfo.InvariantCulture.NumberFormat) + " " +
                        bone._bindState._scale._z.ToString(CultureInfo.InvariantCulture.NumberFormat));
                    writer.WriteEndElement(); //scale
                }
            }

            //Write single-bind geometry
            //foreach (MDL0PolygonNode poly in bone._infPolys)
            //    WritePolyInstance(poly, writer);

            foreach (MDL0BoneNode b in bone.Children)
            {
                WriteBone(b, writer);
            }

            writer.WriteEndElement(); //node
        }

        private static void WritePolyInstance(DrawCall c, XmlWriter writer)
        {
            MDL0ObjectNode obj = c._parentObject;

            writer.WriteStartElement("node");
            writer.WriteAttributeString("id", obj.Name);
            writer.WriteAttributeString("name", obj.Name);
            writer.WriteAttributeString("type", "NODE");

            writer.WriteStartElement("instance_controller");
            writer.WriteAttributeString("url", $"#{obj.Name}_Controller");

            writer.WriteStartElement("skeleton");
            writer.WriteString("#" + obj.Model._linker.BoneCache[0].Name);
            writer.WriteEndElement();

            if (c.MaterialNode != null)
            {
                writer.WriteStartElement("bind_material");
                writer.WriteStartElement("technique_common");
                writer.WriteStartElement("instance_material");
                writer.WriteAttributeString("symbol", c.MaterialNode.Name);
                writer.WriteAttributeString("target", "#" + c.MaterialNode.Name);

                foreach (MDL0MaterialRefNode mr in c.MaterialNode.Children)
                {
                    writer.WriteStartElement("bind_vertex_input");
                    writer.WriteAttributeString("semantic",
                        "TEXCOORD" + (mr.TextureCoordId < 0 ? 0 : mr.TextureCoordId)); //Replace with true set id
                    writer.WriteAttributeString("input_semantic", "TEXCOORD");
                    writer.WriteAttributeString("input_set",
                        (mr.TextureCoordId < 0 ? 0 : mr.TextureCoordId).ToString()); //Replace with true set id
                    writer.WriteEndElement();                                        //bind_vertex_input
                }

                writer.WriteEndElement(); //instance_material
                writer.WriteEndElement(); //technique_common
                writer.WriteEndElement(); //bind_material
            }

            writer.WriteEndElement(); //instance_geometry
            writer.WriteEndElement(); //node
        }

        private static string WriteMatrix(Matrix m)
        {
            string s = "";
            float* p = (float*) &m;
            for (int y = 0; y < 4; y++)
            {
                for (int x = 0; x < 4; x++)
                {
                    if (x != 0 || y != 0)
                    {
                        s += " ";
                    }

                    s += p[(x << 2) + y].ToString(CultureInfo.InvariantCulture.NumberFormat);
                }
            }

            return s;
        }

        public static void Serialize(CHR0Node[] animations, float fps, bool bake, string outFile)
        {
            string[] types = new[] {"scale", "rotate", "translate"};
            string[] axes = new[] {"X", "Y", "Z"};
            bool first = true;

            using (FileStream stream = new FileStream(outFile, FileMode.Create, FileAccess.ReadWrite, FileShare.None,
                0x1000, FileOptions.SequentialScan))
            {
                using (XmlWriter writer = XmlWriter.Create(stream, _writerSettings))
                {
                    writer.Flush();
                    stream.Position = 0;

                    writer.WriteStartDocument();
                    writer.WriteStartElement("COLLADA", "http://www.collada.org/2008/03/COLLADASchema");
                    writer.WriteAttributeString("version", "1.4.1");

                    writer.WriteStartElement("asset");
                    writer.WriteStartElement("contributor");
                    writer.WriteElementString("authoring_tool", Application.ProductName);
                    writer.WriteEndElement();
                    writer.WriteEndElement();

                    writer.WriteStartElement("library_animations");
                    {
                        foreach (CHR0Node animation in animations)
                        {
                            string animName = animation.Name;

                            writer.WriteStartElement("animation");
                            writer.WriteAttributeString("name", animName);
                            writer.WriteAttributeString("id", animName);
                            {
                                foreach (CHR0EntryNode en in animation.Children)
                                {
                                    string bone = en.Name;
                                    KeyframeCollection keyframes = en.Keyframes;

                                    for (int index = 0; index < 9; index++)
                                    {
                                        int keyFrameCount = keyframes._keyArrays[index]._keyCount;
                                        KeyframeEntry root = keyframes._keyArrays[index]._keyRoot;

                                        if (keyFrameCount <= 0)
                                        {
                                            continue;
                                        }

                                        string type = types[index / 3];
                                        string axis = axes[index % 3];

                                        string name = $"{bone}_{type}{axis}";

                                        //writer.WriteStartElement("animation");
                                        //writer.WriteAttributeString("id", name);
                                        {
                                            #region Input source

                                            writer.WriteStartElement("source");
                                            writer.WriteAttributeString("id", name + "_input");
                                            {
                                                writer.WriteStartElement("float_array");
                                                writer.WriteAttributeString("id", name + "_inputArr");
                                                writer.WriteAttributeString("count", keyFrameCount.ToString());
                                                {
                                                    first = true;
                                                    for (KeyframeEntry entry = root._next;
                                                        entry != root;
                                                        entry = entry._next)
                                                    {
                                                        if (first)
                                                        {
                                                            first = false;
                                                        }
                                                        else
                                                        {
                                                            writer.WriteString(" ");
                                                        }

                                                        writer.WriteString(
                                                            (entry._index / fps).ToString(CultureInfo
                                                                .InvariantCulture
                                                                .NumberFormat));
                                                    }
                                                }
                                                writer.WriteEndElement(); //float_array

                                                writer.WriteStartElement("technique_common");
                                                {
                                                    writer.WriteStartElement("accessor");
                                                    writer.WriteAttributeString("source", "#" + name + "_inputArr");
                                                    writer.WriteAttributeString("count", keyFrameCount.ToString());
                                                    writer.WriteAttributeString("stride", "1");
                                                    {
                                                        writer.WriteStartElement("param");
                                                        writer.WriteAttributeString("name", "TIME");
                                                        writer.WriteAttributeString("type", "float");
                                                        writer.WriteEndElement(); //param
                                                    }
                                                    writer.WriteEndElement(); //accessor
                                                }
                                                writer.WriteEndElement(); //technique_common

                                                writer.WriteStartElement("technique");
                                                writer.WriteAttributeString("profile", "MAYA");
                                                {
                                                    writer.WriteStartElement("pre_infinity");
                                                    writer.WriteString("CONSTANT");
                                                    writer.WriteEndElement(); //pre_infinity

                                                    writer.WriteStartElement("post_infinity");
                                                    writer.WriteString("CONSTANT");
                                                    writer.WriteEndElement(); //post_infinity
                                                }
                                                writer.WriteEndElement(); //technique
                                            }
                                            writer.WriteEndElement(); //source

                                            #endregion

                                            #region Output source

                                            writer.WriteStartElement("source");
                                            writer.WriteAttributeString("id", name + "_output");
                                            {
                                                writer.WriteStartElement("float_array");
                                                writer.WriteAttributeString("id", name + "_outputArr");
                                                writer.WriteAttributeString("count", keyFrameCount.ToString());
                                                {
                                                    first = true;
                                                    for (KeyframeEntry entry = root._next;
                                                        entry != root;
                                                        entry = entry._next)
                                                    {
                                                        if (first)
                                                        {
                                                            first = false;
                                                        }
                                                        else
                                                        {
                                                            writer.WriteString(" ");
                                                        }

                                                        writer.WriteString(
                                                            entry._value.ToString(CultureInfo
                                                                .InvariantCulture.NumberFormat));
                                                    }
                                                }
                                                writer.WriteEndElement(); //float_array

                                                writer.WriteStartElement("technique_common");
                                                {
                                                    writer.WriteStartElement("accessor");
                                                    writer.WriteAttributeString("source", "#" + name + "_outputArr");
                                                    writer.WriteAttributeString("count", keyFrameCount.ToString());
                                                    writer.WriteAttributeString("stride", "1");
                                                    {
                                                        writer.WriteStartElement("param");
                                                        writer.WriteAttributeString("name", "TRANSFORM");
                                                        writer.WriteAttributeString("type", "float");
                                                        writer.WriteEndElement(); //param
                                                    }
                                                    writer.WriteEndElement(); //accessor
                                                }
                                                writer.WriteEndElement(); //technique_common
                                            }
                                            writer.WriteEndElement(); //source

                                            #endregion

                                            #region In Tangent source

                                            writer.WriteStartElement("source");
                                            writer.WriteAttributeString("id", name + "_inTan");
                                            {
                                                writer.WriteStartElement("float_array");
                                                writer.WriteAttributeString("id", name + "_inTanArr");
                                                writer.WriteAttributeString("count", keyFrameCount.ToString());
                                                {
                                                    first = true;
                                                    for (KeyframeEntry entry = root._next;
                                                        entry != root;
                                                        entry = entry._next)
                                                    {
                                                        if (first)
                                                        {
                                                            first = false;
                                                        }
                                                        else
                                                        {
                                                            writer.WriteString(" ");
                                                        }

                                                        writer.WriteString(
                                                            entry._tangent.ToString(CultureInfo
                                                                .InvariantCulture.NumberFormat));
                                                    }
                                                }
                                                writer.WriteEndElement(); //float_array

                                                writer.WriteStartElement("technique_common");
                                                {
                                                    writer.WriteStartElement("accessor");
                                                    writer.WriteAttributeString("source", "#" + name + "_inTanArr");
                                                    writer.WriteAttributeString("count", keyFrameCount.ToString());
                                                    writer.WriteAttributeString("stride", "1");
                                                    {
                                                        writer.WriteStartElement("param");
                                                        writer.WriteAttributeString("name", "IN_TANGENT");
                                                        writer.WriteAttributeString("type", "float");
                                                        writer.WriteEndElement(); //param
                                                    }
                                                    writer.WriteEndElement(); //accessor
                                                }
                                                writer.WriteEndElement(); //technique_common
                                            }
                                            writer.WriteEndElement(); //source

                                            #endregion

                                            #region Out Tangent source

                                            writer.WriteStartElement("source");
                                            writer.WriteAttributeString("id", name + "_outTan");
                                            {
                                                writer.WriteStartElement("float_array");
                                                writer.WriteAttributeString("id", name + "_outTanArr");
                                                writer.WriteAttributeString("count", keyFrameCount.ToString());
                                                {
                                                    first = true;
                                                    for (KeyframeEntry entry = root._next;
                                                        entry != root;
                                                        entry = entry._next)
                                                    {
                                                        if (first)
                                                        {
                                                            first = false;
                                                        }
                                                        else
                                                        {
                                                            writer.WriteString(" ");
                                                        }

                                                        writer.WriteString(
                                                            entry._tangent.ToString(CultureInfo
                                                                .InvariantCulture.NumberFormat));
                                                    }
                                                }
                                                writer.WriteEndElement(); //float_array

                                                writer.WriteStartElement("technique_common");
                                                {
                                                    writer.WriteStartElement("accessor");
                                                    writer.WriteAttributeString("source", "#" + name + "_outTanArr");
                                                    writer.WriteAttributeString("count", keyFrameCount.ToString());
                                                    writer.WriteAttributeString("stride", "1");
                                                    {
                                                        writer.WriteStartElement("param");
                                                        writer.WriteAttributeString("name", "OUT_TANGENT");
                                                        writer.WriteAttributeString("type", "float");
                                                        writer.WriteEndElement(); //param
                                                    }
                                                    writer.WriteEndElement(); //accessor
                                                }
                                                writer.WriteEndElement(); //technique_common
                                            }
                                            writer.WriteEndElement(); //source

                                            #endregion

                                            #region Interpolation source

                                            writer.WriteStartElement("source");
                                            writer.WriteAttributeString("id", name + "_interp");
                                            {
                                                writer.WriteStartElement("Name_array");
                                                writer.WriteAttributeString("id", name + "_interpArr");
                                                writer.WriteAttributeString("count", keyFrameCount.ToString());
                                                {
                                                    first = true;
                                                    for (KeyframeEntry entry = root._next;
                                                        entry != root;
                                                        entry = entry._next)
                                                    {
                                                        if (first)
                                                        {
                                                            first = false;
                                                        }
                                                        else
                                                        {
                                                            writer.WriteString(" ");
                                                        }

                                                        writer.WriteString("HERMITE");
                                                    }
                                                }
                                                writer.WriteEndElement(); //Name_array

                                                writer.WriteStartElement("technique_common");
                                                {
                                                    writer.WriteStartElement("accessor");
                                                    writer.WriteAttributeString("source", "#" + name + "_interpArr");
                                                    writer.WriteAttributeString("count", keyFrameCount.ToString());
                                                    writer.WriteAttributeString("stride", "1");
                                                    {
                                                        writer.WriteStartElement("param");
                                                        writer.WriteAttributeString("name", "INTERPOLATION");
                                                        writer.WriteAttributeString("type", "Name");
                                                        writer.WriteEndElement(); //param
                                                    }
                                                    writer.WriteEndElement(); //accessor
                                                }
                                                writer.WriteEndElement(); //technique_common
                                            }
                                            writer.WriteEndElement(); //source

                                            #endregion

                                            #region Sampler

                                            writer.WriteStartElement("sampler");
                                            writer.WriteAttributeString("id", name + "_sampler");
                                            {
                                                writer.WriteStartElement("input");
                                                writer.WriteAttributeString("semantic", "INPUT");
                                                writer.WriteAttributeString("source", "#" + name + "_input");
                                                writer.WriteEndElement(); //input

                                                writer.WriteStartElement("input");
                                                writer.WriteAttributeString("semantic", "OUTPUT");
                                                writer.WriteAttributeString("source", "#" + name + "_output");
                                                writer.WriteEndElement(); //input

                                                writer.WriteStartElement("input");
                                                writer.WriteAttributeString("semantic", "IN_TANGENT");
                                                writer.WriteAttributeString("source", "#" + name + "_inTan");
                                                writer.WriteEndElement(); //input

                                                writer.WriteStartElement("input");
                                                writer.WriteAttributeString("semantic", "OUT_TANGEN");
                                                writer.WriteAttributeString("source", "#" + name + "_outTan");
                                                writer.WriteEndElement(); //input

                                                writer.WriteStartElement("input");
                                                writer.WriteAttributeString("semantic", "INTERPOLATION");
                                                writer.WriteAttributeString("source", "#" + name + "_interp");
                                                writer.WriteEndElement(); //input
                                            }
                                            writer.WriteEndElement(); //sampler

                                            #endregion

                                            writer.WriteStartElement("channel");
                                            writer.WriteAttributeString("source", "#" + name + "_sampler");
                                            writer.WriteAttributeString("target",
                                                $"{bone}/{type}.{axis}");
                                            writer.WriteEndElement(); //channel
                                        }
                                        //writer.WriteEndElement(); //animation
                                    }
                                }
                            }
                            writer.WriteEndElement(); //animation
                        }

                        writer.WriteEndElement(); //library_animations
                    }
                }
            }
        }
    }
}