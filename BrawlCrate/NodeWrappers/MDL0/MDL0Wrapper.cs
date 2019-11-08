using BrawlCrate.UI;
using BrawlLib.Imaging;
using BrawlLib.Internal;
using BrawlLib.Internal.Windows.Forms;
using BrawlLib.SSBB;
using BrawlLib.SSBB.ResourceNodes;
using BrawlLib.SSBB.Types;
using BrawlLib.Wii.Models;
using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace BrawlCrate.NodeWrappers
{
    [NodeWrapper(ResourceType.MDL0)]
    public class MDL0Wrapper : GenericWrapper
    {
        #region Menu

        private static readonly ContextMenuStrip _menu;

        private static readonly ToolStripMenuItem _newShaderToolStripMenuItem;
        private static readonly ToolStripMenuItem _importShaderToolStripMenuItem;

        private static readonly ToolStripMenuItem DuplicateToolStripMenuItem =
            new ToolStripMenuItem("&Duplicate", null, DuplicateAction, Keys.Control | Keys.D);

        private static readonly ToolStripMenuItem ReplaceToolStripMenuItem =
            new ToolStripMenuItem("&Replace", null, ReplaceAction, Keys.Control | Keys.R);

        private static readonly ToolStripMenuItem RestoreToolStripMenuItem =
            new ToolStripMenuItem("Res&tore", null, RestoreAction, Keys.Control | Keys.T);

        private static readonly ToolStripMenuItem MoveUpToolStripMenuItem =
            new ToolStripMenuItem("Move &Up", null, MoveUpAction, Keys.Control | Keys.Up);

        private static readonly ToolStripMenuItem MoveDownToolStripMenuItem =
            new ToolStripMenuItem("Move D&own", null, MoveDownAction, Keys.Control | Keys.Down);

        private static readonly ToolStripMenuItem DeleteToolStripMenuItem =
            new ToolStripMenuItem("&Delete", null, DeleteAction, Keys.Control | Keys.Delete);

        static MDL0Wrapper()
        {
            _menu = new ContextMenuStrip();
            _menu.Items.Add(new ToolStripMenuItem("&Preview", null, PreviewAction, Keys.Control | Keys.P));
            _menu.Items.Add(new ToolStripSeparator());
            _menu.Items.Add(new ToolStripMenuItem("&Export", null, ExportAction, Keys.Control | Keys.E));
            _menu.Items.Add(DuplicateToolStripMenuItem);
            _menu.Items.Add(ReplaceToolStripMenuItem);
            _menu.Items.Add(RestoreToolStripMenuItem);
            _menu.Items.Add(new ToolStripSeparator());
            _menu.Items.Add(MoveUpToolStripMenuItem);
            _menu.Items.Add(MoveDownToolStripMenuItem);
            _menu.Items.Add(new ToolStripMenuItem("Re&name", null, RenameAction, Keys.Control | Keys.N));
            _menu.Items.Add(new ToolStripSeparator());
            _menu.Items.Add(new ToolStripMenuItem("Ne&w Asset", null,
                new ToolStripMenuItem("&Material", null, NewMaterialAction, Keys.Control | Keys.M),
                _newShaderToolStripMenuItem =
                    new ToolStripMenuItem("S&hader", null, NewShaderAction, Keys.Control | Keys.H),
                new ToolStripMenuItem("Vertices", null, NewVertexAction),
                new ToolStripMenuItem("Normals", null, NewNormalAction),
                new ToolStripMenuItem("Colors", null, NewColorAction),
                new ToolStripMenuItem("UVs", null, NewUVAction)
            ));
            _menu.Items.Add(new ToolStripMenuItem("&Import Asset", null,
                new ToolStripMenuItem("&Material", null, ImportMaterialAction),
                _importShaderToolStripMenuItem = new ToolStripMenuItem("S&hader", null, ImportShaderAction),
                new ToolStripMenuItem("Vertices", null, ImportVertexAction),
                new ToolStripMenuItem("Normals", null, ImportNormalAction),
                new ToolStripMenuItem("Colors", null, ImportColorAction),
                new ToolStripMenuItem("UVs", null, ImportUVAction),
                new ToolStripMenuItem("Object", null, ImportObjectAction)
            ));
            _menu.Items.Add(new ToolStripMenuItem("&Sort Assets", null,
                new ToolStripMenuItem("Materials", null, SortMaterialAction),
                new ToolStripMenuItem("Vertices", null, SortVertexAction),
                new ToolStripMenuItem("Normals", null, SortNormalAction),
                new ToolStripMenuItem("Colors", null, SortColorAction),
                new ToolStripMenuItem("UVs", null, SortUVAction),
                new ToolStripMenuItem("Objects", null, SortObjectAction),
                new ToolStripMenuItem("Textures", null, SortTextureAction)
            ));
            _menu.Items.Add(new ToolStripMenuItem("&Auto Name Assets", null,
                new ToolStripMenuItem("Materials", null, NameMaterialAction),
                new ToolStripMenuItem("Vertices", null, NameVertexAction),
                new ToolStripMenuItem("Normals", null, NameNormalAction),
                new ToolStripMenuItem("Colors", null, NameColorAction),
                new ToolStripMenuItem("UVs", null, NameUVAction),
                new ToolStripMenuItem("Objects", null, NameObjectAction)
            ));
            _menu.Items.Add(new ToolStripSeparator());
            _menu.Items.Add(new ToolStripMenuItem("&Edit Materials", null,
                new ToolStripMenuItem("&Characters", null,
                    new ToolStripMenuItem("&Convert To Spy Model", null, SpyConvertAction),
                    new ToolStripMenuItem("(&Re)generate Metal Materials", null, MetalAction,
                        Keys.Control | Keys.Shift | Keys.M)
                ),
                new ToolStripMenuItem("&Stages", null,
                    new ToolStripMenuItem("&Convert To Shadow Model", null, ShadowConvertAction)
                ),
                new ToolStripMenuItem("&Culling", null,
                    new ToolStripMenuItem("Invert &Culling", null, InvertMaterialsAction),
                    new ToolStripMenuItem("Set all (Cull &None)", null, CullNoneAction,
                        Keys.Control | Keys.Shift | Keys.D0),
                    new ToolStripMenuItem("Set all (Cull &Outside)", null, CullOutsideAction,
                        Keys.Control | Keys.Shift | Keys.D1),
                    new ToolStripMenuItem("Set all (Cull &Inside)", null, CullInsideAction,
                        Keys.Control | Keys.Shift | Keys.D2),
                    new ToolStripMenuItem("Set all (Cull &All)", null, CullAllAction,
                        Keys.Control | Keys.Shift | Keys.D3)
                )
            ));
            _menu.Items.Add(new ToolStripMenuItem("&Reimport Meshes", null, ReimportAction));
            _menu.Items.Add(new ToolStripMenuItem("&Optimize Meshes", null, OptimizeAction));
            _menu.Items.Add(new ToolStripMenuItem("&Recalculate Bounding Boxes", null, RecalcBBsOption));
            _menu.Items.Add(new ToolStripSeparator());
            _menu.Items.Add(DeleteToolStripMenuItem);
            _menu.Items.Add(new ToolStripMenuItem("Delete All But Bones", null, StripAction,
                Keys.Control | Keys.Shift | Keys.Delete));
            _menu.Opening += MenuOpening;
            _menu.Closing += MenuClosing;
        }

        private static void ReimportAction(object sender, EventArgs e)
        {
            GetInstance<MDL0Wrapper>().ReimportMeshes();
        }

        private static void OptimizeAction(object sender, EventArgs e)
        {
            GetInstance<MDL0Wrapper>().Optimize();
        }

        protected static void PreviewAction(object sender, EventArgs e)
        {
            GetInstance<MDL0Wrapper>().Preview();
        }

        protected static void ImportObjectAction(object sender, EventArgs e)
        {
            GetInstance<MDL0Wrapper>().ImportObject();
        }

        protected static void RecalcBBsOption(object sender, EventArgs e)
        {
            GetInstance<MDL0Wrapper>().RecalcBoundingBoxes();
        }

        protected static void MetalAction(object sender, EventArgs e)
        {
            GetInstance<MDL0Wrapper>().AutoMetal();
        }

        protected static void NewShaderAction(object sender, EventArgs e)
        {
            GetInstance<MDL0Wrapper>().NewShader();
        }

        protected static void NewMaterialAction(object sender, EventArgs e)
        {
            GetInstance<MDL0Wrapper>().NewMaterial();
        }

        protected static void NewVertexAction(object sender, EventArgs e)
        {
            GetInstance<MDL0Wrapper>().NewVertex();
        }

        protected static void NewNormalAction(object sender, EventArgs e)
        {
            GetInstance<MDL0Wrapper>().NewNormal();
        }

        protected static void NewColorAction(object sender, EventArgs e)
        {
            GetInstance<MDL0Wrapper>().NewColor();
        }

        protected static void NewUVAction(object sender, EventArgs e)
        {
            GetInstance<MDL0Wrapper>().NewUV();
        }

        protected static void NameMaterialAction(object sender, EventArgs e)
        {
            GetInstance<MDL0Wrapper>().NameMaterial();
        }

        protected static void NameVertexAction(object sender, EventArgs e)
        {
            GetInstance<MDL0Wrapper>().NameVertex();
        }

        protected static void NameNormalAction(object sender, EventArgs e)
        {
            GetInstance<MDL0Wrapper>().NameNormal();
        }

        protected static void NameColorAction(object sender, EventArgs e)
        {
            GetInstance<MDL0Wrapper>().NameColor();
        }

        protected static void NameUVAction(object sender, EventArgs e)
        {
            GetInstance<MDL0Wrapper>().NameUV();
        }

        protected static void NameObjectAction(object sender, EventArgs e)
        {
            GetInstance<MDL0Wrapper>().NameObject();
        }

        protected static void ImportVertexAction(object sender, EventArgs e)
        {
            GetInstance<MDL0Wrapper>().ImportVertex();
        }

        protected static void ImportNormalAction(object sender, EventArgs e)
        {
            GetInstance<MDL0Wrapper>().ImportNormal();
        }

        protected static void ImportColorAction(object sender, EventArgs e)
        {
            GetInstance<MDL0Wrapper>().ImportColor();
        }

        protected static void ImportUVAction(object sender, EventArgs e)
        {
            GetInstance<MDL0Wrapper>().ImportUV();
        }

        protected static void ImportMaterialAction(object sender, EventArgs e)
        {
            GetInstance<MDL0Wrapper>().ImportMaterial();
        }

        protected static void ImportShaderAction(object sender, EventArgs e)
        {
            GetInstance<MDL0Wrapper>().ImportShader();
        }

        protected static void SortMaterialAction(object sender, EventArgs e)
        {
            GetInstance<MDL0Wrapper>().SortMaterial();
        }

        protected static void SortVertexAction(object sender, EventArgs e)
        {
            GetInstance<MDL0Wrapper>().SortVertex();
        }

        protected static void SortNormalAction(object sender, EventArgs e)
        {
            GetInstance<MDL0Wrapper>().SortNormal();
        }

        protected static void SortColorAction(object sender, EventArgs e)
        {
            GetInstance<MDL0Wrapper>().SortColor();
        }

        protected static void SortUVAction(object sender, EventArgs e)
        {
            GetInstance<MDL0Wrapper>().SortUV();
        }

        protected static void SortObjectAction(object sender, EventArgs e)
        {
            GetInstance<MDL0Wrapper>().SortObject();
        }

        protected static void SortTextureAction(object sender, EventArgs e)
        {
            GetInstance<MDL0Wrapper>().SortTexture();
        }

        protected static void InvertMaterialsAction(object sender, EventArgs e)
        {
            GetInstance<MDL0Wrapper>().InvertMaterials();
            MainForm.Instance.resourceTree_SelectionChanged(sender, e);
        }

        protected static void CullNoneAction(object sender, EventArgs e)
        {
            GetInstance<MDL0Wrapper>().CullMaterials(CullMode.Cull_None);
            MainForm.Instance.resourceTree_SelectionChanged(sender, e);
        }

        protected static void CullOutsideAction(object sender, EventArgs e)
        {
            GetInstance<MDL0Wrapper>().CullMaterials(CullMode.Cull_Outside);
            MainForm.Instance.resourceTree_SelectionChanged(sender, e);
        }

        protected static void CullInsideAction(object sender, EventArgs e)
        {
            GetInstance<MDL0Wrapper>().CullMaterials(CullMode.Cull_Inside);
            MainForm.Instance.resourceTree_SelectionChanged(sender, e);
        }

        protected static void CullAllAction(object sender, EventArgs e)
        {
            GetInstance<MDL0Wrapper>().CullMaterials(CullMode.Cull_All);
            MainForm.Instance.resourceTree_SelectionChanged(sender, e);
        }

        protected static void ShadowConvertAction(object sender, EventArgs e)
        {
            GetInstance<MDL0Wrapper>().ShadowConvert();
            MainForm.Instance.resourceTree_SelectionChanged(sender, e);
        }

        protected static void SpyConvertAction(object sender, EventArgs e)
        {
            GetInstance<MDL0Wrapper>().SpyConvert();
            MainForm.Instance.resourceTree_SelectionChanged(sender, e);
        }

        protected static void StripAction(object sender, EventArgs e)
        {
            GetInstance<MDL0Wrapper>().StripModel();
            MainForm.Instance.resourceTree_SelectionChanged(sender, e);
        }

        private static void MenuClosing(object sender, ToolStripDropDownClosingEventArgs e)
        {
            DuplicateToolStripMenuItem.Enabled = true;
            ReplaceToolStripMenuItem.Enabled = true;
            DeleteToolStripMenuItem.Enabled = true;
            RestoreToolStripMenuItem.Enabled = true;
            MoveUpToolStripMenuItem.Enabled = true;
            MoveDownToolStripMenuItem.Enabled = true;
            _newShaderToolStripMenuItem.Enabled = true;
            _importShaderToolStripMenuItem.Enabled = true;
        }

        private static void MenuOpening(object sender, CancelEventArgs e)
        {
            MDL0Wrapper w = GetInstance<MDL0Wrapper>();
            DuplicateToolStripMenuItem.Enabled = w.Parent != null;
            ReplaceToolStripMenuItem.Enabled = w.Parent != null;
            DeleteToolStripMenuItem.Enabled = w.Parent != null;
            RestoreToolStripMenuItem.Enabled = w._resource.IsDirty || w._resource.IsBranch;
            MoveUpToolStripMenuItem.Enabled = w.PrevNode != null;
            MoveDownToolStripMenuItem.Enabled = w.NextNode != null;
            if (((MDL0Node) w._resource)._shadList != null && ((MDL0Node) w._resource)._matList != null)
            {
                _newShaderToolStripMenuItem.Enabled =
                    ((MDL0Node) w._resource)._shadList.Count < ((MDL0Node) w._resource)._matList.Count;
                _importShaderToolStripMenuItem.Enabled =
                    ((MDL0Node) w._resource)._shadList.Count < ((MDL0Node) w._resource)._matList.Count;
            }
            else
            {
                _newShaderToolStripMenuItem.Enabled = ((MDL0Node) w._resource)._matList != null;
                _importShaderToolStripMenuItem.Enabled = ((MDL0Node) w._resource)._matList != null;
            }
        }

        #endregion

        public override string ExportFilter => FileFilters.MDL0Export;
        public override string ImportFilter => FileFilters.MDL0Import;

        public MDL0Wrapper()
        {
            ContextMenuStrip = _menu;
        }

        public void ReimportMeshes()
        {
            OpenFileDialog ofd = new OpenFileDialog
            {
                Filter = SupportedFilesHandler.GetCompleteFilter("mdl0", "dae"),
                Title = "Please select a model to reimport meshes from."
            };
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                MDL0Node replacement = MDL0Node.FromFile(ofd.FileName);
                if (replacement != null)
                {
                    ((MDL0Node) _resource).ReplaceMeshes(replacement, true, true, true);
                    replacement.Dispose();
                    _resource.UpdateCurrentControl();
                }
            }
        }

        public void Preview()
        {
            new ModelForm().Show(_owner, (MDL0Node) _resource);
        }

        private void Optimize()
        {
            new ObjectOptimizerForm().ShowDialog(_resource);
        }

        public MDL0ShaderNode NewShader()
        {
            MDL0Node model = (MDL0Node) _resource;

            if (model._shadGroup == null)
            {
                MDL0GroupNode g = model._shadGroup;
                if (g == null)
                {
                    model.AddChild(g = new MDL0GroupNode(MDLResourceType.Shaders), true);
                    model._shadGroup = g;
                    model._shadList = g.Children;
                }
            }

            if (model._shadList != null &&
                model._matList != null &&
                model._shadList.Count < model._matList.Count)
            {
                MDL0ShaderNode shader = new MDL0ShaderNode();
                model._shadGroup.AddChild(shader);
                shader.Default();
                shader.Rebuild(true);

                BaseWrapper b = FindResource(shader, true);
                b?.EnsureVisible();

                return shader;
            }

            MessageBox.Show("Shader could not be added. Make sure that you do not have more shaders than materials",
                "Error");
            return null;
        }

        public MDL0MaterialNode NewMaterial()
        {
            MDL0Node model = (MDL0Node) _resource;

            if (model._matGroup == null)
            {
                MDL0GroupNode g = model._matGroup;
                if (g == null)
                {
                    model.AddChild(g = new MDL0GroupNode(MDLResourceType.Materials), true);
                    model._matGroup = g;
                    model._matList = g.Children;
                }
            }

            MDL0MaterialNode mat = new MDL0MaterialNode();
            model._matGroup.AddChild(mat);
            mat.Name = "Material" + mat.Index;

            if (model._shadGroup == null)
            {
                MDL0GroupNode g = model._shadGroup;
                if (g == null)
                {
                    model.AddChild(g = new MDL0GroupNode(MDLResourceType.Shaders), true);
                    model._shadGroup = g;
                    model._shadList = g.Children;
                }
            }

            if (model._shadList.Count == 0)
            {
                NewShader();
            }

            mat.ShaderNode = (MDL0ShaderNode) model._shadList[0];
            MDL0MaterialRefNode mr = new MDL0MaterialRefNode();
            mat.AddChild(mr);
            mr.Name = "MatRef0";
            mat.Rebuild(true);

            BaseWrapper b = FindResource(mat, true);
            b?.EnsureVisible();

            return mat;
        }

        public void AutoMetal()
        {
            if (MessageBox.Show(null,
                    "Are you sure you want to (re)generate metal materials for Brawl?\nAll existing metal materials and shaders will be reset.",
                    "", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                ((MDL0Node) _resource).GenerateMetalMaterials();
            }
        }

        public void NameMaterial()
        {
            MDL0Node model = (MDL0Node) _resource;
            MDL0GroupNode g = model._matGroup;
            int i = 0;
            if (g != null)
            {
                foreach (MDL0MaterialNode m in g.Children)
                {
                    m.Name = "Material_" + i++;
                }
            }
        }

        public void NameObject()
        {
            MDL0Node model = (MDL0Node) _resource;
            MDL0GroupNode g = model._objGroup;
            int i = 0;
            if (g != null)
            {
                foreach (MDL0ObjectNode o in g.Children)
                {
                    o.Name = "polygon" + i++;
                }
            }
        }

        public void NameVertex()
        {
            MDL0Node model = (MDL0Node) _resource;
            MDL0GroupNode g = model._vertGroup;
            if (g != null)
            {
                foreach (MDL0VertexNode v in g.Children)
                {
                    string name = model.Name + "_";
                    if (v._objects.Count > 0)
                    {
                        MDL0ObjectNode o = v._objects[0];
                        name += o.Name;
                        if (o._drawCalls.Count > 0)
                        {
                            DrawCall c = o._drawCalls[0];
                            if (c.MaterialNode != null && c.VisibilityBoneNode != null)
                            {
                                name += "_" + c.Material + "_" + c.VisibilityBone;
                            }
                        }
                    }
                    else
                    {
                        name += "VertexArray";
                    }

                    v.Name = g.FindName(name);
                }
            }
        }

        public void NameNormal()
        {
            MDL0Node model = (MDL0Node) _resource;
            MDL0GroupNode g = model._normGroup;
            if (g != null)
            {
                foreach (MDL0NormalNode v in g.Children)
                {
                    string name = model.Name + "_";
                    if (v._objects.Count > 0)
                    {
                        MDL0ObjectNode o = v._objects[0];
                        name += o.Name;
                        if (o._drawCalls.Count > 0)
                        {
                            DrawCall c = o._drawCalls[0];
                            if (c.MaterialNode != null && c.VisibilityBoneNode != null)
                            {
                                name += "_" + c.Material + "_" + c.VisibilityBone;
                            }
                        }
                    }
                    else
                    {
                        name += "NormalArray";
                    }

                    v.Name = g.FindName(name);
                }
            }
        }

        public void NameColor()
        {
            MDL0Node model = (MDL0Node) _resource;
            MDL0GroupNode g = model._colorGroup;
            if (g != null)
            {
                foreach (MDL0ColorNode v in g.Children)
                {
                    string name = model.Name + "_";
                    if (v._objects.Count > 0)
                    {
                        MDL0ObjectNode o = v._objects[0];
                        name += o.Name;
                        if (o._drawCalls.Count > 0)
                        {
                            DrawCall c = o._drawCalls[0];
                            if (c.MaterialNode != null && c.VisibilityBoneNode != null)
                            {
                                name += "_" + c.Material + "_" + c.VisibilityBone;
                            }
                        }
                    }
                    else
                    {
                        name += "ColorArray";
                    }

                    v.Name = g.FindName(name);
                }
            }
        }

        public void NameUV()
        {
            MDL0Node model = (MDL0Node) _resource;
            MDL0GroupNode g = model._uvGroup;
            int i = 0;
            if (g != null)
            {
                foreach (MDL0UVNode v in g.Children)
                {
                    v.Name = "#" + i++;
                }
            }
        }

        public MDL0VertexNode NewVertex()
        {
            MDL0Node model = (MDL0Node) _resource;

            MDL0GroupNode g = model._vertGroup;
            if (g == null)
            {
                model.AddChild(g = new MDL0GroupNode(MDLResourceType.Vertices), true);
                model._vertGroup = g;
                model._vertList = g.Children;
            }

            MDL0VertexNode node = new MDL0VertexNode {Name = "VertexSet" + ((MDL0Node) _resource)._vertList.Count};
            node.Vertices = new Vector3[] {new Vector3(0)};
            g.AddChild(node, true);
            node.ForceRebuild = true;
            node.Rebuild(true);
            node.SignalPropertyChange();

            FindResource(node, true).EnsureVisible();

            return node;
        }

        public MDL0NormalNode NewNormal()
        {
            MDL0Node model = (MDL0Node) _resource;

            MDL0GroupNode g = model._normGroup;
            if (g == null)
            {
                model.AddChild(g = new MDL0GroupNode(MDLResourceType.Normals), true);
                model._normGroup = g;
                model._normList = g.Children;
            }

            MDL0NormalNode node = new MDL0NormalNode {Name = "NormalSet" + ((MDL0Node) _resource)._normList.Count};
            node.Normals = new Vector3[] {new Vector3(0)};
            g.AddChild(node, true);
            node._forceRebuild = true;
            node.Rebuild(true);
            node.SignalPropertyChange();

            FindResource(node, true).EnsureVisible();

            return node;
        }

        public MDL0ColorNode NewColor()
        {
            MDL0Node model = (MDL0Node) _resource;

            MDL0GroupNode g = model._colorGroup;
            if (g == null)
            {
                model.AddChild(g = new MDL0GroupNode(MDLResourceType.Colors), true);
                model._colorGroup = g;
                model._colorList = g.Children;
            }

            MDL0ColorNode node = new MDL0ColorNode {Name = "ColorSet" + ((MDL0Node) _resource)._colorList.Count};
            node.Colors = new RGBAPixel[] {new RGBAPixel {A = 255, R = 128, G = 128, B = 128}};
            g.AddChild(node, true);

            node.Rebuild(true);
            node.SignalPropertyChange();

            FindResource(node, true).EnsureVisible();

            return node;
        }

        public MDL0UVNode NewUV()
        {
            MDL0Node model = (MDL0Node) _resource;

            MDL0GroupNode g = model._uvGroup;
            if (g == null)
            {
                model.AddChild(g = new MDL0GroupNode(MDLResourceType.UVs), true);
                model._uvGroup = g;
                model._uvList = g.Children;
            }

            MDL0UVNode node = new MDL0UVNode {Name = "#" + ((MDL0Node) _resource)._uvList.Count};
            node.Points = new Vector2[] {new Vector2(0)};
            g.AddChild(node, true);
            node._forceRebuild = true;
            node.Rebuild(true);
            node.SignalPropertyChange();

            FindResource(node, true).EnsureVisible();

            return node;
        }

        public void ImportMaterial()
        {
            if (Program.OpenFile(FileFilters.MDL0Material, out string path))
            {
                NewMaterial().Replace(path);
            }
        }

        public void ImportShader()
        {
            if (Program.OpenFile(FileFilters.MDL0Shader, out string path))
            {
                NewShader()?.Replace(path);
            }
        }

        public void ImportVertex()
        {
            OpenFileDialog o = new OpenFileDialog
            {
                Filter = "Raw Vertex Set (*.*)|*.*",
                Title = "Please select a vertex set to import."
            };
            if (o.ShowDialog() == DialogResult.OK)
            {
                NewVertex().Replace(o.FileName);
            }
        }

        public void ImportNormal()
        {
            OpenFileDialog o = new OpenFileDialog
            {
                Filter = "Raw Normal Set (*.*)|*.*",
                Title = "Please select a normal set to import."
            };
            if (o.ShowDialog() == DialogResult.OK)
            {
                NewNormal().Replace(o.FileName);
            }
        }

        public void ImportColor()
        {
            OpenFileDialog o = new OpenFileDialog
            {
                Filter = "Raw Color Set (*.*)|*.*",
                Title = "Please select a color set to import."
            };
            if (o.ShowDialog() == DialogResult.OK)
            {
                NewColor().Replace(o.FileName);
            }
        }

        public void ImportUV()
        {
            OpenFileDialog o = new OpenFileDialog
            {
                Filter = "Raw Vertex Set (*.*)|*.*",
                Title = "Please select a vertex set to import."
            };
            if (o.ShowDialog() == DialogResult.OK)
            {
                NewUV().Replace(o.FileName);
            }
        }

        public void ImportObject()
        {
            MDL0Node external = null;
            OpenFileDialog o = new OpenFileDialog
            {
                Filter = "MDL0 Raw Model (*.mdl0)|*.mdl0",
                Title = "Please select a model to import an object from."
            };
            if (o.ShowDialog() == DialogResult.OK)
            {
                if ((external = (MDL0Node) NodeFactory.FromFile(null, o.FileName)) != null)
                {
                    new ObjectImporter().ShowDialog((MDL0Node) _resource, external);
                }
            }
        }

        public void SortMaterial()
        {
            int index = Index;
            ((MDL0Node) _resource).MaterialGroup.SortChildren();
            RefreshView(_resource);
        }

        public void SortVertex()
        {
            int index = Index;
            ((MDL0Node) _resource).VertexGroup.SortChildren();
            RefreshView(_resource);
        }

        public void SortNormal()
        {
            int index = Index;
            ((MDL0Node) _resource).NormalGroup.SortChildren();
            RefreshView(_resource);
        }

        public void SortColor()
        {
            int index = Index;
            ((MDL0Node) _resource).ColorGroup.SortChildren();
            RefreshView(_resource);
        }

        public void SortUV()
        {
            int index = Index;
            ((MDL0Node) _resource).UVGroup.SortChildren();
            RefreshView(_resource);
        }

        public void SortObject()
        {
            int index = Index;
            ((MDL0Node) _resource).PolygonGroup.SortChildren();
            RefreshView(_resource);
        }

        public void SortTexture()
        {
            int index = Index;
            ((MDL0Node) _resource).TextureGroup.SortChildren();
            RefreshView(_resource);
        }

        private void RecalcBoundingBoxes()
        {
            MDL0Node model = _resource as MDL0Node;
            model?.CalculateBoundingBoxes();
        }

        public void ShadowConvert()
        {
            ((MDL0Node) _resource).ConvertToShadowModel();
        }

        public void SpyConvert()
        {
            ((MDL0Node) _resource).ConvertToSpyModel();
        }

        public void InvertMaterials()
        {
            if (_resource is MDL0Node mdl)
            {
                if (mdl.MaterialList == null)
                {
                    return;
                }

                foreach (MDL0MaterialNode mat in mdl.MaterialList)
                {
                    if (mat.CullMode == CullMode.Cull_Inside)
                    {
                        mat.CullMode = CullMode.Cull_Outside;
                    }
                    else if (mat.CullMode == CullMode.Cull_Outside)
                    {
                        mat.CullMode = CullMode.Cull_Inside;
                    }
                }
            }
        }

        public void CullMaterials(CullMode mode)
        {
            if (_resource is MDL0Node mdl)
            {
                if (mdl.MaterialList == null)
                {
                    return;
                }

                foreach (MDL0MaterialNode mat in mdl.MaterialList)
                {
                    mat.CullMode = mode;
                }
            }
        }

        private void StripModel()
        {
            MDL0Node model = _resource as MDL0Node;
            if (model != null)
            {
                model.StripModel();
            }
        }
    }
}