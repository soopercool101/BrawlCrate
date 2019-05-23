using System;
using System.Windows.Forms;
using System.ComponentModel;
using BrawlLib.SSBB.ResourceNodes;
using System.Drawing;
using BrawlBox.Properties;
using System.Runtime.InteropServices;
using BrawlBox.NodeWrappers;

namespace BrawlBox
{
    public class ResourceTree : TreeViewMS
    {
        private static ImageList _imgList;
        public static ImageList Images
        {
            get
            {
                if (_imgList == null)
                {
                    _imgList = new ImageList();
                    _imgList.ImageSize = new Size(24, 24);
                    _imgList.ColorDepth = ColorDepth.Depth32Bit;
                    _imgList.Images.AddRange(new Image[]{
                        Resources.Unknown, //0
                        Resources.Folder,
                        Resources.ARC,
                        Resources.BRES,
                        Resources.MSG,
                        Resources.TEX0, //5
                        Resources.PLT0,
                        Resources.MDL0,
                        Resources.CHR,
                        Resources.CLR,
                        Resources.VIS, //10
                        Resources.SHP,
                        Resources.SRT,
                        Resources.RSAR,
                        Resources.RSTM,
                        Resources.S, //15
                        Resources.G,
                        Resources.T,
                        Resources.B,
                        Resources.EFLS,
                        Resources.Coll, //20
                        Resources.REFF,
                        Resources.AI,
                        Resources.AIPD,
                        Resources.ATKD,
                        Resources.CE, //25
                        Resources.MDef,
                        Resources.Event,
                        Resources.REFT,
                        Resources.PAT,
                        Resources.IMG, //30
                        Resources.SCN0,
                        Resources.STPM,
                        Resources.TPL,
                        Resources.Palette,
                        Resources.U8, //35
                        Resources.REFFEntry,
                        Resources.BLOC,
                        Resources.STDT,
                        Resources.GDOR,
                        Resources.GEG1,//40
                        Resources.ENEMY,
                        Resources.GMOV,
                        Resources.GSND,
                        Resources.GMOT,
                        Resources.ADSJ,//45
                        Resources.GBLK,
                        Resources.GMPS,
                        Resources.Redirect,
                        Resources.REL,
                        Resources.BGMG,//50
                        Resources.SharedTEX0,
                        Resources.SCLA
                    });
                }
                return _imgList;
            }
        }

        public event EventHandler SelectionChanged;

        private bool _allowContextMenus = true;
        [DefaultValue(true)]
        public bool AllowContextMenus
        {
            get { return _allowContextMenus; }
            set { _allowContextMenus = value; }
        }

        private bool _allowIcons = false;
        [DefaultValue(false)]
        public bool ShowIcons
        {
            get { return _allowIcons; }
            set { ImageList = (_allowIcons = value) ? Images : null; }
        }

        private TreeNode _selected;
        new public TreeNode SelectedNode 
        { 
            get { return base.SelectedNode; } 
            set 
            {
                if (_selected == value)
                    return;

                _selected = base.SelectedNode = value;
                if (SelectionChanged != null)
                    SelectionChanged(this, null);
            } 
        }

        public ResourceTree()
        {
            SetStyle(ControlStyles.UserMouse, true);

            _timer.Interval = 200;
            _timer.Tick += new EventHandler(timer_Tick);

            AllowDrop = true;

            ItemDrag += new ItemDragEventHandler(treeView_ItemDrag);
            DragOver += new DragEventHandler(treeView1_DragOver);
            DragDrop += new DragEventHandler(treeView1_DragDrop);
            DragEnter += new DragEventHandler(treeView1_DragEnter);
            DragLeave += new EventHandler(treeView1_DragLeave);
            GiveFeedback += new GiveFeedbackEventHandler(treeView1_GiveFeedback);

            m_DelegateOpenFile = new DelegateOpenFile(ImportFile);
        }

        public BaseWrapper FindResource(ResourceNode node)
        {
            BaseWrapper w = null;
            foreach (BaseWrapper n in Nodes)
                if ((w = n.FindResource(node, true)) != null)
                    break;
            return w;
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == 0x204)
            {
                int x = (int)m.LParam & 0xFFFF, y = (int)m.LParam >> 16;

                TreeNode n = GetNodeAt(x, y);
                if (n != null)
                {
                    Rectangle r = n.Bounds;
                    r.X -= 25; r.Width += 25;
                    if (r.Contains(x, y))
                        SelectedNode = n;
                }

                m.Result = IntPtr.Zero;
                return;
            }
            else if (m.Msg == 0x205)
            {
                int x = (int)m.LParam & 0xFFFF, y = (int)m.LParam >> 16;
                
                if ((_allowContextMenus) && (_selected != null))
                {
                    var menuStrip = this.SelectedNodes.Count > 1
                        ? GetMultiSelectMenuStrip()
                        : _selected.ContextMenuStrip;
                    if (menuStrip != null)
                    {
                        Rectangle r = _selected.Bounds;
                        r.X -= 25; r.Width += 25;
                        if (r.Contains(x, y))
                            menuStrip.Show(this, x, y);
                    }
                }
            }

            base.WndProc(ref m);
        }

        private ContextMenuStrip GetMultiSelectMenuStrip() {
            var nodes = this.SelectedNodes;
            var singleNode = this.SelectedNode as MultiSelectableWrapper;
            if (singleNode == null) return null;
            
            foreach (var node in nodes) {
                var type = node.GetType();
                if (!type.IsAssignableFrom(singleNode.GetType())) {
                    // More than one type of node is selected
                    return null;
                }
            }

            return singleNode.MultiSelectMenuStrip;
        }

        public void Clear()
        {
            BeginUpdate();
            foreach (BaseWrapper n in Nodes) n.Unlink();
            Nodes.Clear();
            EndUpdate();
        }

        protected override void OnAfterSelect(TreeViewEventArgs e)
        {
            SelectedNode = e.Node;
            base.OnAfterSelect(e);
        }

        protected override void OnBeforeExpand(TreeViewCancelEventArgs e)
        {
            base.OnBeforeExpand(e);
            if (e.Node is BaseWrapper)
                ((BaseWrapper)e.Node).OnExpand();
        }

        protected override void OnMouseDoubleClick(MouseEventArgs e)
        {
            if ((e.Button == MouseButtons.Left) && (SelectedNode is BaseWrapper))
                ((BaseWrapper)SelectedNode).OnDoubleClick();
            else
                base.OnMouseDoubleClick(e);
        }

        protected override void Dispose(bool disposing) { Clear(); base.Dispose(disposing); }

        private TreeNode _dragNode = null;
        private TreeNode _tempDropNode = null;
        private Timer _timer = new Timer();

        private ImageList imageListDrag = new ImageList();

        private void treeView_ItemDrag(object sender, ItemDragEventArgs e)
        {
            _dragNode = (TreeNode)e.Item;
            SelectedNode = _dragNode;

            imageListDrag.Images.Clear();
            imageListDrag.ImageSize = new Size(
                (_dragNode.Bounds.Size.Width + Indent + 7).Clamp(1, 256), 
                _dragNode.Bounds.Height.Clamp(1, 256));

            Bitmap bmp = new Bitmap(_dragNode.Bounds.Width + Indent + 7, _dragNode.Bounds.Height);

            Graphics gfx = Graphics.FromImage(bmp);

            gfx.DrawImage(Images.Images[SelectedNode.ImageIndex], 0, 0);

            gfx.DrawString(_dragNode.Text,
                Font,
                new SolidBrush(ForeColor),
                (float)Indent + 7.0f, 4.0f);

            imageListDrag.Images.Add(bmp);

            Point p = PointToClient(Control.MousePosition);

            int dx = p.X + Indent - _dragNode.Bounds.Left;
            int dy = p.Y - _dragNode.Bounds.Top - 25;

            if (DragHelper.ImageList_BeginDrag(imageListDrag.Handle, 0, dx, dy))
            {
                DoDragDrop(bmp, DragDropEffects.Move);
                DragHelper.ImageList_EndDrag();
            }
        }

        private void ImportFile(string file, TreeNode t)
        {
            ResourceNode node = null;

            if (t == null)
                Program.Open(file);
            else
            {
                ResourceNode dest = ((BaseWrapper)t).Resource;
                try
                {
                    if ((node = NodeFactory.FromFile(null, file)) != null)
                    {
                        bool ok = false;
                        if (ModifierKeys == Keys.Shift || dest.Parent == null)
                            ok = TryAddChild(node, dest);
                        else
                            ok = TryDrop(node, dest);
                        if (!ok)
                        {
                            node.Dispose();
                            node = null;
                        }
                        else
                        {
                            BaseWrapper b = FindResource(node);
                            if (b != null)
                            {
                                b.EnsureVisible();
                                SelectedNode = b;
                            }
                        }
                    }
                    else if (dest is TEX0Node || dest is REFTEntryNode || dest is TPLTextureNode)
                    {
                        if (ModifierKeys == Keys.Control)
                        {
                            using (TextureConverterDialog dlg = new TextureConverterDialog())
                            {
                                dlg.ImageSource = file;
                                if (dest is TEX0Node)
                                    dlg.ShowDialog(MainForm.Instance, dest as TEX0Node);
                                else if (dest is REFTEntryNode)
                                    dlg.ShowDialog(MainForm.Instance, dest as REFTEntryNode);
                                else
                                    dlg.ShowDialog(MainForm.Instance, dest as TPLTextureNode);
                            }
                        }
                        else
                            dest.Replace(file);
                    }
                }
                catch { }
            }

            _timer.Enabled = false;
            _dragNode = null;
        }

        private delegate void DelegateOpenFile(String s, TreeNode t);
        private DelegateOpenFile m_DelegateOpenFile;
        private void treeView1_DragOver(object sender, DragEventArgs e)
        {
            Array a = (Array)e.Data.GetData(DataFormats.FileDrop);

            Point formP = PointToClient(new Point(e.X, e.Y));
            DragHelper.ImageList_DragMove(formP.X - Left, formP.Y - Top);

            TreeNode dropNode = GetNodeAt(PointToClient(new Point(e.X, e.Y)));
            if (dropNode == null && a == null)
            {
                e.Effect = DragDropEffects.None;
                return;
            }

            e.Effect = DragDropEffects.Move;

            if (_tempDropNode != dropNode)
            {
                DragHelper.ImageList_DragShowNolock(false);
                SelectedNode = dropNode;
                DragHelper.ImageList_DragShowNolock(true);
                _tempDropNode = dropNode;
            }

            TreeNode tmpNode = dropNode;
            if (tmpNode != null)
            while (tmpNode.Parent != null)
            {
                if (tmpNode.Parent == _dragNode)
                    e.Effect = DragDropEffects.None;

                tmpNode = tmpNode.Parent;
            }
        }

        private static bool CompareToType(Type compared, Type to)
        {
            Type bType;
            if (compared == to)
                return true;
            else
            {
                bType = compared.BaseType;
                while (bType != null && bType != typeof(ResourceNode))
                {
                    if (to == bType)
                        return true;
                    
                    bType = bType.BaseType;
                }
            }
            return false;
        }
        private static bool CompareTypes(ResourceNode r1, ResourceNode r2)
        {
            return CompareTypes(r1.GetType(), r2.GetType());
        }
        private static bool CompareTypes(Type type1, Type type2)
        {
            Type bType1, bType2;
            if (type1 == type2)
                return true;
            else
            {
                bType2 = type2.BaseType;
                while (bType2 != null && bType2 != typeof(ResourceNode))
                {
                    bType1 = type1.BaseType;
                    while (bType1 != null && bType1 != typeof(ResourceNode))
                    {
                        if (bType1 == bType2)
                            return true;
                        bType1 = bType1.BaseType;
                    }
                    bType2 = bType2.BaseType;
                }
            }
            return false;
        }

        private static bool TryDrop(ResourceNode dragging, ResourceNode dropping)
        {
            if (dropping.Parent == null)
                return false;

            bool good = false;
            int destIndex = dropping.Index;

            good = CompareTypes(dragging, dropping);

            //if (dropping.Parent is BRESGroupNode)
                foreach (Type t in dropping.Parent.AllowedChildTypes)
                    if (good = CompareToType(dragging.GetType(), t))
                        break;

            if (good)
            {
                if (dragging.Parent != null)
                    dragging.Parent.RemoveChild(dragging);
                if (destIndex < dropping.Parent.Children.Count)
                    dropping.Parent.InsertChild(dragging, true, destIndex);
                else
                    dropping.Parent.AddChild(dragging, true);

                dragging.OnMoved();
            }

            return good;
        }

        private static bool TryAddChild(ResourceNode dragging, ResourceNode dropping)
        {
            bool good = false;

            Type dt = dragging.GetType();
            if (dropping.Children.Count != 0)
                good = CompareTypes(dropping.Children[0].GetType(), dt);
            else
                foreach (Type t in dropping.AllowedChildTypes)
                    if (good = CompareToType(dt, t))
                        break;

            if (good)
            {
                if (dragging.Parent != null)
                    dragging.Parent.RemoveChild(dragging);
                dropping.AddChild(dragging);

                dragging.OnMoved();
            }

            return good;
        }

        private void treeView1_DragDrop(object sender, DragEventArgs e)
        {
            Array a = (Array)e.Data.GetData(DataFormats.FileDrop);

            DragHelper.ImageList_DragLeave(Handle);
            TreeNode dropNode = GetNodeAt(PointToClient(new Point(e.X, e.Y)));

            if (a != null)
            {
                string s = null;
                for (int i = 0; i < a.Length; i++)
                {
                    s = a.GetValue(i).ToString();
                    this.BeginInvoke(m_DelegateOpenFile, s, dropNode);
                }
            }
            else
            {
                if (_dragNode != dropNode)
                {
                    BaseWrapper drag = ((BaseWrapper)_dragNode);
                    BaseWrapper drop = ((BaseWrapper)dropNode);
                    ResourceNode dragging = drag.Resource;
                    ResourceNode dropping = drop.Resource;

                    if (dropping.Parent == null)
                        goto End;

                    bool ok = false;
                    if (ModifierKeys == Keys.Shift)
                        ok = TryAddChild(dragging, dropping);
                    else
                        ok = TryDrop(dragging, dropping);

                    if (ok)
                    {
                        BaseWrapper b = FindResource(dragging);
                        if (b != null)
                        {
                            b.EnsureVisible();
                            SelectedNode = b;
                        }
                    }

                End:
                    _dragNode = null;
                    _timer.Enabled = false;
                }
            }
        }

        private void treeView1_DragEnter(object sender, DragEventArgs e)
        {
            DragHelper.ImageList_DragEnter(Handle, e.X - Left, e.Y - Top);
            _timer.Enabled = true;
        }

        private void treeView1_DragLeave(object sender, EventArgs e)
        {
            DragHelper.ImageList_DragLeave(Handle);
            _timer.Enabled = false;
        }

        private void treeView1_GiveFeedback(object sender, GiveFeedbackEventArgs e)
        {
            if (e.Effect == DragDropEffects.Move)
            {
                e.UseDefaultCursors = false;
                Cursor = Cursors.Default;
            }
            else e.UseDefaultCursors = true;

        }

        private void timer_Tick(object sender, EventArgs e)
        {
            Point pt = PointToClient(Control.MousePosition);
            TreeNode node = GetNodeAt(pt);

            if (node == null) return;
            if (pt.Y < 30)
            {
                if (node.PrevVisibleNode != null)
                {
                    node = node.PrevVisibleNode;

                    DragHelper.ImageList_DragShowNolock(false);
                    node.EnsureVisible();
                    Refresh();
                    DragHelper.ImageList_DragShowNolock(true);
                }
            }
            else if (pt.Y > Size.Height - 30)
            {
                if (node.NextVisibleNode != null)
                {
                    node = node.NextVisibleNode;

                    DragHelper.ImageList_DragShowNolock(false);
                    node.EnsureVisible();
                    Refresh();
                    DragHelper.ImageList_DragShowNolock(true);
                }
            }
        }
    }

    public class DragHelper
    {
        [DllImport("comctl32.dll")]
        public static extern bool InitCommonControls();

        [DllImport("comctl32.dll", CharSet = CharSet.Auto)]
        public static extern bool ImageList_BeginDrag(IntPtr himlTrack, int
            iTrack, int dxHotspot, int dyHotspot);

        [DllImport("comctl32.dll", CharSet = CharSet.Auto)]
        public static extern bool ImageList_DragMove(int x, int y);

        [DllImport("comctl32.dll", CharSet = CharSet.Auto)]
        public static extern void ImageList_EndDrag();

        [DllImport("comctl32.dll", CharSet = CharSet.Auto)]
        public static extern bool ImageList_DragEnter(IntPtr hwndLock, int x, int y);

        [DllImport("comctl32.dll", CharSet = CharSet.Auto)]
        public static extern bool ImageList_DragLeave(IntPtr hwndLock);

        [DllImport("comctl32.dll", CharSet = CharSet.Auto)]
        public static extern bool ImageList_DragShowNolock(bool fShow);

        static DragHelper()
        {
            InitCommonControls();
        }
    }
}
