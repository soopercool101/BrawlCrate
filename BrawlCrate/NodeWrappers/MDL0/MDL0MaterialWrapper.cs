using BrawlCrate.UI;
using BrawlLib.SSBB;
using BrawlLib.SSBB.ResourceNodes;
using BrawlLib.Wii.Graphics;
using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace BrawlCrate.NodeWrappers
{
    [NodeWrapper(ResourceType.MDL0Material)]
    public class MDL0MaterialWrapper : GenericWrapper
    {
        #region Menu

        private static readonly ContextMenuStrip _menu;

        private static readonly ToolStripMenuItem _addNewRefToolStripMenuItem;

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

        static MDL0MaterialWrapper()
        {
            _menu = new ContextMenuStrip();
            _menu.Items.Add(new ToolStripMenuItem("&Export", null, ExportAction, Keys.Control | Keys.E));
            _menu.Items.Add(DuplicateToolStripMenuItem);
            _menu.Items.Add(ReplaceToolStripMenuItem);
            _menu.Items.Add(new ToolStripMenuItem("Re&name", null, RenameAction, Keys.Control | Keys.N));
            _menu.Items.Add(new ToolStripSeparator());
            _menu.Items.Add(MoveUpToolStripMenuItem);
            _menu.Items.Add(MoveDownToolStripMenuItem);
            _menu.Items.Add(new ToolStripSeparator());
            _menu.Items.Add(_addNewRefToolStripMenuItem = new ToolStripMenuItem("Add New Reference", null, CreateAction,
                Keys.Control | Keys.Alt | Keys.N));
            _menu.Items.Add(new ToolStripMenuItem("Export GLSL Shader", null, ExportShaderAction));
            _menu.Items.Add(new ToolStripSeparator());
            _menu.Items.Add(DeleteToolStripMenuItem);
            _menu.Opening += MenuOpening;
            _menu.Closing += MenuClosing;
        }

        protected static void CreateAction(object sender, EventArgs e)
        {
            GetInstance<MDL0MaterialWrapper>().CreateRef();
        }

        protected static void ExportShaderAction(object sender, EventArgs e)
        {
            GetInstance<MDL0MaterialWrapper>().ExportShader();
        }

        private static void MenuClosing(object sender, ToolStripDropDownClosingEventArgs e)
        {
            MoveUpToolStripMenuItem.Enabled = true;
            MoveDownToolStripMenuItem.Enabled = true;
            _addNewRefToolStripMenuItem.Enabled = true;
        }

        private static void MenuOpening(object sender, CancelEventArgs e)
        {
            MDL0MaterialWrapper w = GetInstance<MDL0MaterialWrapper>();
            MoveUpToolStripMenuItem.Enabled = w.PrevNode != null;
            MoveDownToolStripMenuItem.Enabled = w.NextNode != null;
            _addNewRefToolStripMenuItem.Enabled = w._resource.Children.Count < 8; //8 mat refs max!
        }

        private void CreateRef()
        {
            if (_resource.Children.Count < 8)
            {
                MDL0MaterialRefNode node = new MDL0MaterialRefNode();
                _resource.AddChild(node);
                node.Default();
                _resource.SignalPropertyChange();

                //if (node.Model.AutoMetalMaterials && ((MDL0MaterialNode)node.Parent).MetalMaterial != null)
                //    ((MDL0MaterialNode)node.Parent).MetalMaterial.UpdateAsMetal();

                Nodes[Nodes.Count - 1].EnsureVisible();
                //TreeView.SelectedNode = Nodes[Nodes.Count - 1];
            }
        }

        private void ExportShader()
        {
            MDL0MaterialNode mat = _resource as MDL0MaterialNode;

            ShaderGenerator.SetTarget(mat);

            ShaderGenerator.UsePixelLighting = MessageBox.Show(MainForm.Instance, "Use per-pixel lighting?", "",
                                                   MessageBoxButtons.YesNo, MessageBoxIcon.Question) ==
                                               DialogResult.Yes;

            SaveFileDialog s = new SaveFileDialog
            {
                Filter = "Text File (*.txt)|*.txt",
                FileName = _resource.Name + " vertex shader",
                Title = "Choose a place to save the vertex shader"
            };
            if (s.ShowDialog() == DialogResult.OK)
            {
                System.IO.File.WriteAllText(s.FileName,
                    ShaderGenerator.GenVertexShader().Replace("\n", Environment.NewLine));
            }

            s.Filter = "Text File (*.txt)|*.txt";
            s.FileName = _resource.Name + " fragment shader";
            s.Title = "Choose a place to save the fragment shader";
            if (s.ShowDialog() == DialogResult.OK)
            {
                string m = ShaderGenerator.GenMaterialFragShader();
                string[] t = ShaderGenerator.GenTEVFragShader();
                System.IO.File.WriteAllText(s.FileName,
                    ShaderGenerator.CombineFragShader(m, t, mat.ActiveShaderStages)
                        .Replace("\n", Environment.NewLine));
            }

            ShaderGenerator.ClearTarget();
            ShaderGenerator._forceRecompile = false;
        }

        #endregion

        public override string ExportFilter => FileFilters.MDL0Material;

        public MDL0MaterialWrapper()
        {
            ContextMenuStrip = _menu;
        }

        public override ResourceNode Duplicate()
        {
            ResourceNode rNode = base.Duplicate();

            if (rNode is MDL0MaterialNode mat)
            {
                mat.Shader = (_resource as MDL0MaterialNode).Shader;
            }

            for (int i = 0; i < rNode.Children.Count && i < Resource.Children.Count; i++)
            {
                rNode.Children[i].Name = Resource.Children[i].Name;
            }

            return rNode;
        }
    }

    [NodeWrapper(ResourceType.MDL0MaterialEntry)]
    public class MDL0MaterialRefWrapper : GenericWrapper
    {
        public override ResourceNode Duplicate()
        {
            if (_resource is MDL0MaterialRefNode matRef && matRef.Model != null)
            {
                MDL0MaterialRefNode matRef2 = new MDL0MaterialRefNode();
                matRef.Parent.InsertChild(matRef2, true, matRef.Index + 1);
                matRef2.Default();
                // Misc
                matRef2.Name = matRef.Name;
                matRef2.Texture = matRef.Texture;
                matRef2.Palette = matRef.Palette;
                // Texture Coordinates
                matRef2.Scale = matRef.Scale;
                matRef2.Rotation = matRef.Rotation;
                matRef2.Translation = matRef.Translation;
                // Texture Matrix Effect
                matRef2.HasTextureMatrix = matRef.HasTextureMatrix;
                matRef2.SCN0RefCamera = matRef.SCN0RefCamera;
                matRef2.SCN0RefLight = matRef.SCN0RefLight;
                // Texture Reference
                matRef2.UWrapMode = matRef.UWrapMode;
                matRef2.VWrapMode = matRef.VWrapMode;
                matRef2.MinFilter = matRef.MinFilter;
                matRef2.MagFilter = matRef.MagFilter;
                matRef2.LODBias = matRef.LODBias;
                matRef2.MaxAnisotropy = matRef.MaxAnisotropy;
                matRef2.ClampBias = matRef.ClampBias;
                matRef2.TexelInterpolate = matRef.TexelInterpolate;
                // XF TexGen Flags
                matRef2.Projection = matRef.Projection;
                matRef2.InputForm = matRef.InputForm;
                matRef2.Type = matRef.Type;
                matRef2.Coordinates = matRef.Coordinates;
                matRef2.EmbossSource = matRef.EmbossSource;
                matRef2.EmbossLight = matRef.EmbossLight;
                matRef2.Normalize = matRef.Normalize;

                return matRef2;
            }

            return null;
        }
    }
}