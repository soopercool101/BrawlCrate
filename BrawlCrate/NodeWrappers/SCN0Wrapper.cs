using BrawlLib;
using BrawlLib.SSBB.ResourceNodes;
using BrawlLib.Wii.Animations;
using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace BrawlCrate.NodeWrappers
{
    [NodeWrapper(ResourceType.SCN0)]
    public class SCN0Wrapper : GenericWrapper
    {
        #region Menu

        private static readonly ContextMenuStrip _menu;
        private static ToolStripMenuItem _replaceToolStripMenuItem;
        private static ToolStripMenuItem _restoreToolStripMenuItem;
        private static ToolStripMenuItem _moveUpToolStripMenuItem;
        private static ToolStripMenuItem _moveDownToolStripMenuItem;
        private static ToolStripMenuItem _deleteToolStripMenuItem;

        static SCN0Wrapper()
        {
            _menu = new ContextMenuStrip();
            _menu.Items.Add(new ToolStripMenuItem("&New LightSet", null, newLightSetAction, Keys.Control | Keys.L));
            _menu.Items.Add(new ToolStripMenuItem("&New Ambient Light", null, newAmbLightAction,
                Keys.Control | Keys.B));
            _menu.Items.Add(new ToolStripMenuItem("&New Light", null, newLightAction, Keys.Control | Keys.H));
            _menu.Items.Add(new ToolStripMenuItem("&New FogSet", null, newFogSetAction, Keys.Control | Keys.F));
            _menu.Items.Add(new ToolStripMenuItem("&New Camera", null, newCameraAction, Keys.Control | Keys.C));
            _menu.Items.Add(new ToolStripSeparator());
            _menu.Items.Add(new ToolStripMenuItem("&Export", null, ExportAction, Keys.Control | Keys.E));
            _menu.Items.Add(_replaceToolStripMenuItem = new ToolStripMenuItem("&Replace", null, ReplaceAction, Keys.Control | Keys.R));
            _menu.Items.Add(_restoreToolStripMenuItem = new ToolStripMenuItem("Res&tore", null, RestoreAction, Keys.Control | Keys.T));
            _menu.Items.Add(new ToolStripSeparator());
            _menu.Items.Add(_moveUpToolStripMenuItem = new ToolStripMenuItem("Move &Up", null, MoveUpAction, Keys.Control | Keys.Up));
            _menu.Items.Add(_moveDownToolStripMenuItem = new ToolStripMenuItem("Move D&own", null, MoveDownAction, Keys.Control | Keys.Down));
            _menu.Items.Add(new ToolStripMenuItem("Re&name", null, RenameAction, Keys.Control | Keys.N));
            _menu.Items.Add(new ToolStripSeparator());
            _menu.Items.Add(_deleteToolStripMenuItem = new ToolStripMenuItem("&Delete", null, DeleteAction, Keys.Control | Keys.Delete));
            _menu.Opening += MenuOpening;
            _menu.Closing += MenuClosing;
        }

        protected static void newLightSetAction(object sender, EventArgs e)
        {
            GetInstance<SCN0Wrapper>().newLightSet();
        }

        protected static void newAmbLightAction(object sender, EventArgs e)
        {
            GetInstance<SCN0Wrapper>().newAmbLight();
        }

        protected static void newLightAction(object sender, EventArgs e)
        {
            GetInstance<SCN0Wrapper>().newLight();
        }

        protected static void newFogSetAction(object sender, EventArgs e)
        {
            GetInstance<SCN0Wrapper>().newFogSet();
        }

        protected static void newCameraAction(object sender, EventArgs e)
        {
            GetInstance<SCN0Wrapper>().newCamera();
        }

        private static void MenuClosing(object sender, ToolStripDropDownClosingEventArgs e)
        {
            if (_replaceToolStripMenuItem != null)
            {
                _replaceToolStripMenuItem.Enabled = true;
            }

            if (_restoreToolStripMenuItem != null)
            {
                _restoreToolStripMenuItem.Enabled = true;
            }

            if (_moveUpToolStripMenuItem != null)
            {
                _moveUpToolStripMenuItem.Enabled = true;
            }

            if (_moveDownToolStripMenuItem != null)
            {
                _moveDownToolStripMenuItem.Enabled = true;
            }

            if (_deleteToolStripMenuItem != null)
            {
                _deleteToolStripMenuItem.Enabled = true;
            }
        }

        private static void MenuOpening(object sender, CancelEventArgs e)
        {
            var w = GetInstance<SCN0Wrapper>();

            if (_replaceToolStripMenuItem != null)
            {
                _replaceToolStripMenuItem.Enabled = w.Parent != null;
            }

            if (_restoreToolStripMenuItem != null)
            {
                _restoreToolStripMenuItem.Enabled = w._resource.IsDirty || w._resource.IsBranch;
            }

            if (_moveUpToolStripMenuItem != null)
            {
                _moveUpToolStripMenuItem.Enabled = w.PrevNode != null;
            }

            if (_moveDownToolStripMenuItem != null)
            {
                _moveDownToolStripMenuItem.Enabled = w.NextNode != null;
            }

            if (_deleteToolStripMenuItem != null)
            {
                _deleteToolStripMenuItem.Enabled = w.Parent != null;
            }
        }

        #endregion

        public SCN0Wrapper()
        {
            ContextMenuStrip = _menu;
        }

        public override string ExportFilter => FileFilters.SCN0;

        public void newLightSet()
        {
            SCN0EntryNode node = ((SCN0Node) _resource).CreateResource<SCN0LightSetNode>("NewLightSet");
            BaseWrapper res = FindResource(node, true);
            res.EnsureVisible();
            //res.TreeView.SelectedNode = res;
        }

        public void newAmbLight()
        {
            SCN0EntryNode node = ((SCN0Node) _resource).CreateResource<SCN0AmbientLightNode>("NewAmbientLight");
            BaseWrapper res = FindResource(node, true);
            res.EnsureVisible();
            //res.TreeView.SelectedNode = res;
        }

        public void newLight()
        {
            SCN0EntryNode node = ((SCN0Node) _resource).CreateResource<SCN0LightNode>("NewLight");
            BaseWrapper res = FindResource(node, true);
            res.EnsureVisible();
            //res.TreeView.SelectedNode = res;
        }

        public void newFogSet()
        {
            SCN0EntryNode node = ((SCN0Node) _resource).CreateResource<SCN0FogNode>("NewFogSet");
            BaseWrapper res = FindResource(node, true);
            res.EnsureVisible();
            //res.TreeView.SelectedNode = res;
        }

        public void newCamera()
        {
            SCN0EntryNode node = ((SCN0Node) _resource).CreateResource<SCN0CameraNode>("NewCamera");
            BaseWrapper res = FindResource(node, true);
            res.EnsureVisible();
            //res.TreeView.SelectedNode = res;
        }
    }

    [NodeWrapper(ResourceType.SCN0Camera)]
    public class SCN0CameraWrapper : GenericWrapper
    {
        #region Menu

        private static readonly ContextMenuStrip _menu;
        private static ToolStripMenuItem _replaceToolStripMenuItem;
        private static ToolStripMenuItem _restoreToolStripMenuItem;
        private static ToolStripMenuItem _moveUpToolStripMenuItem;
        private static ToolStripMenuItem _moveDownToolStripMenuItem;
        private static ToolStripMenuItem _deleteToolStripMenuItem;

        static SCN0CameraWrapper()
        {
            _menu = new ContextMenuStrip();
            _menu.Items.Add(new ToolStripMenuItem("View Interpolation", null, ViewInterp, Keys.Control | Keys.T));
            _menu.Items.Add(new ToolStripSeparator());
            _menu.Items.Add(new ToolStripMenuItem("&Export", null, ExportAction, Keys.Control | Keys.E));
            _menu.Items.Add(_replaceToolStripMenuItem = new ToolStripMenuItem("&Replace", null, ReplaceAction, Keys.Control | Keys.R));
            _menu.Items.Add(_restoreToolStripMenuItem = new ToolStripMenuItem("Res&tore", null, RestoreAction, Keys.Control | Keys.T));
            _menu.Items.Add(new ToolStripSeparator());
            _menu.Items.Add(_moveUpToolStripMenuItem = new ToolStripMenuItem("Move &Up", null, MoveUpAction, Keys.Control | Keys.Up));
            _menu.Items.Add(_moveDownToolStripMenuItem = new ToolStripMenuItem("Move D&own", null, MoveDownAction, Keys.Control | Keys.Down));
            _menu.Items.Add(new ToolStripMenuItem("Re&name", null, RenameAction, Keys.Control | Keys.N));
            _menu.Items.Add(new ToolStripSeparator());
            _menu.Items.Add(_deleteToolStripMenuItem = new ToolStripMenuItem("&Delete", null, DeleteAction, Keys.Control | Keys.Delete));
            _menu.Opening += MenuOpening;
            _menu.Closing += MenuClosing;
        }

        protected static void ViewInterp(object sender, EventArgs e)
        {
            GetInstance<SCN0CameraWrapper>().ViewInterp();
        }

        private void ViewInterp()
        {
            InterpolationForm f = MainForm.Instance.InterpolationForm;
            if (f != null)
            {
                InterpolationEditor e = f._interpolationEditor;
                if (e != null)
                {
                    e.SetTarget(_resource as IKeyframeSource);
                }
            }
        }

        private static void MenuClosing(object sender, ToolStripDropDownClosingEventArgs e)
        {
            if (_replaceToolStripMenuItem != null)
            {
                _replaceToolStripMenuItem.Enabled = true;
            }

            if (_restoreToolStripMenuItem != null)
            {
                _restoreToolStripMenuItem.Enabled = true;
            }

            if (_moveUpToolStripMenuItem != null)
            {
                _moveUpToolStripMenuItem.Enabled = true;
            }

            if (_moveDownToolStripMenuItem != null)
            {
                _moveDownToolStripMenuItem.Enabled = true;
            }

            if (_deleteToolStripMenuItem != null)
            {
                _deleteToolStripMenuItem.Enabled = true;
            }
        }

        private static void MenuOpening(object sender, CancelEventArgs e)
        {
            var w = GetInstance<SCN0CameraWrapper>();

            if (_replaceToolStripMenuItem != null)
            {
                _replaceToolStripMenuItem.Enabled = w.Parent != null;
            }

            if (_restoreToolStripMenuItem != null)
            {
                _restoreToolStripMenuItem.Enabled = w._resource.IsDirty || w._resource.IsBranch;
            }

            if (_moveUpToolStripMenuItem != null)
            {
                _moveUpToolStripMenuItem.Enabled = w.PrevNode != null;
            }

            if (_moveDownToolStripMenuItem != null)
            {
                _moveDownToolStripMenuItem.Enabled = w.NextNode != null;
            }

            if (_deleteToolStripMenuItem != null)
            {
                _deleteToolStripMenuItem.Enabled = w.Parent != null;
            }
        }

        #endregion

        public SCN0CameraWrapper(IWin32Window owner)
        {
            _owner = owner;
            ContextMenuStrip = _menu;
        }

        public SCN0CameraWrapper()
        {
            _owner = null;
            ContextMenuStrip = _menu;
        }
    }

    [NodeWrapper(ResourceType.SCN0Fog)]
    public class SCN0FogWrapper : GenericWrapper
    {
        #region Menu

        private static readonly ContextMenuStrip _menu;
        private static ToolStripMenuItem _replaceToolStripMenuItem;
        private static ToolStripMenuItem _restoreToolStripMenuItem;
        private static ToolStripMenuItem _moveUpToolStripMenuItem;
        private static ToolStripMenuItem _moveDownToolStripMenuItem;
        private static ToolStripMenuItem _deleteToolStripMenuItem;

        static SCN0FogWrapper()
        {
            _menu = new ContextMenuStrip();
            _menu.Items.Add(new ToolStripMenuItem("View Interpolation", null, ViewInterp, Keys.Control | Keys.T));
            _menu.Items.Add(new ToolStripSeparator());
            _menu.Items.Add(new ToolStripMenuItem("&Export", null, ExportAction, Keys.Control | Keys.E));
            _menu.Items.Add(_replaceToolStripMenuItem = new ToolStripMenuItem("&Replace", null, ReplaceAction, Keys.Control | Keys.R));
            _menu.Items.Add(_restoreToolStripMenuItem = new ToolStripMenuItem("Res&tore", null, RestoreAction, Keys.Control | Keys.T));
            _menu.Items.Add(new ToolStripSeparator());
            _menu.Items.Add(_moveUpToolStripMenuItem = new ToolStripMenuItem("Move &Up", null, MoveUpAction, Keys.Control | Keys.Up));
            _menu.Items.Add(_moveDownToolStripMenuItem = new ToolStripMenuItem("Move D&own", null, MoveDownAction, Keys.Control | Keys.Down));
            _menu.Items.Add(new ToolStripMenuItem("Re&name", null, RenameAction, Keys.Control | Keys.N));
            _menu.Items.Add(new ToolStripSeparator());
            _menu.Items.Add(_deleteToolStripMenuItem = new ToolStripMenuItem("&Delete", null, DeleteAction, Keys.Control | Keys.Delete));
            _menu.Opening += MenuOpening;
            _menu.Closing += MenuClosing;
        }

        protected static void ViewInterp(object sender, EventArgs e)
        {
            GetInstance<SCN0FogWrapper>().ViewInterp();
        }

        private void ViewInterp()
        {
            InterpolationForm f = MainForm.Instance.InterpolationForm;
            if (f != null)
            {
                InterpolationEditor e = f._interpolationEditor;
                if (e != null)
                {
                    e.SetTarget(_resource as IKeyframeSource);
                }
            }
        }

        private static void MenuClosing(object sender, ToolStripDropDownClosingEventArgs e)
        {
            if (_replaceToolStripMenuItem != null)
            {
                _replaceToolStripMenuItem.Enabled = true;
            }

            if (_restoreToolStripMenuItem != null)
            {
                _restoreToolStripMenuItem.Enabled = true;
            }

            if (_moveUpToolStripMenuItem != null)
            {
                _moveUpToolStripMenuItem.Enabled = true;
            }

            if (_moveDownToolStripMenuItem != null)
            {
                _moveDownToolStripMenuItem.Enabled = true;
            }

            if (_deleteToolStripMenuItem != null)
            {
                _deleteToolStripMenuItem.Enabled = true;
            }
        }

        private static void MenuOpening(object sender, CancelEventArgs e)
        {
            var w = GetInstance<SCN0FogWrapper>();

            if (_replaceToolStripMenuItem != null)
            {
                _replaceToolStripMenuItem.Enabled = w.Parent != null;
            }

            if (_restoreToolStripMenuItem != null)
            {
                _restoreToolStripMenuItem.Enabled = w._resource.IsDirty || w._resource.IsBranch;
            }

            if (_moveUpToolStripMenuItem != null)
            {
                _moveUpToolStripMenuItem.Enabled = w.PrevNode != null;
            }

            if (_moveDownToolStripMenuItem != null)
            {
                _moveDownToolStripMenuItem.Enabled = w.NextNode != null;
            }

            if (_deleteToolStripMenuItem != null)
            {
                _deleteToolStripMenuItem.Enabled = w.Parent != null;
            }
        }

        #endregion

        public SCN0FogWrapper(IWin32Window owner)
        {
            _owner = owner;
            ContextMenuStrip = _menu;
        }

        public SCN0FogWrapper()
        {
            _owner = null;
            ContextMenuStrip = _menu;
        }
    }

    [NodeWrapper(ResourceType.SCN0Light)]
    public class SCN0LightWrapper : GenericWrapper
    {
        #region Menu

        private static readonly ContextMenuStrip _menu;
        private static ToolStripMenuItem _replaceToolStripMenuItem;
        private static ToolStripMenuItem _restoreToolStripMenuItem;
        private static ToolStripMenuItem _moveUpToolStripMenuItem;
        private static ToolStripMenuItem _moveDownToolStripMenuItem;
        private static ToolStripMenuItem _deleteToolStripMenuItem;

        static SCN0LightWrapper()
        {
            _menu = new ContextMenuStrip();
            _menu.Items.Add(new ToolStripMenuItem("View Interpolation", null, ViewInterp, Keys.Control | Keys.T));
            _menu.Items.Add(new ToolStripSeparator());
            _menu.Items.Add(new ToolStripMenuItem("&Export", null, ExportAction, Keys.Control | Keys.E));
            _menu.Items.Add(_replaceToolStripMenuItem = new ToolStripMenuItem("&Replace", null, ReplaceAction, Keys.Control | Keys.R));
            _menu.Items.Add(_restoreToolStripMenuItem = new ToolStripMenuItem("Res&tore", null, RestoreAction, Keys.Control | Keys.T));
            _menu.Items.Add(new ToolStripSeparator());
            _menu.Items.Add(_moveUpToolStripMenuItem = new ToolStripMenuItem("Move &Up", null, MoveUpAction, Keys.Control | Keys.Up));
            _menu.Items.Add(_moveDownToolStripMenuItem = new ToolStripMenuItem("Move D&own", null, MoveDownAction, Keys.Control | Keys.Down));
            _menu.Items.Add(new ToolStripMenuItem("Re&name", null, RenameAction, Keys.Control | Keys.N));
            _menu.Items.Add(new ToolStripSeparator());
            _menu.Items.Add(_deleteToolStripMenuItem = new ToolStripMenuItem("&Delete", null, DeleteAction, Keys.Control | Keys.Delete));
            _menu.Opening += MenuOpening;
            _menu.Closing += MenuClosing;
        }

        protected static void ViewInterp(object sender, EventArgs e)
        {
            GetInstance<SCN0LightWrapper>().ViewInterp();
        }

        private void ViewInterp()
        {
            InterpolationForm f = MainForm.Instance.InterpolationForm;
            if (f != null)
            {
                InterpolationEditor e = f._interpolationEditor;
                if (e != null)
                {
                    e.SetTarget(_resource as IKeyframeSource);
                }
            }
        }

        private static void MenuClosing(object sender, ToolStripDropDownClosingEventArgs e)
        {
            if (_replaceToolStripMenuItem != null)
            {
                _replaceToolStripMenuItem.Enabled = true;
            }

            if (_restoreToolStripMenuItem != null)
            {
                _restoreToolStripMenuItem.Enabled = true;
            }

            if (_moveUpToolStripMenuItem != null)
            {
                _moveUpToolStripMenuItem.Enabled = true;
            }

            if (_moveDownToolStripMenuItem != null)
            {
                _moveDownToolStripMenuItem.Enabled = true;
            }

            if (_deleteToolStripMenuItem != null)
            {
                _deleteToolStripMenuItem.Enabled = true;
            }
        }

        private static void MenuOpening(object sender, CancelEventArgs e)
        {
            var w = GetInstance<SCN0LightWrapper>();

            if (_replaceToolStripMenuItem != null)
            {
                _replaceToolStripMenuItem.Enabled = w.Parent != null;
            }

            if (_restoreToolStripMenuItem != null)
            {
                _restoreToolStripMenuItem.Enabled = w._resource.IsDirty || w._resource.IsBranch;
            }

            if (_moveUpToolStripMenuItem != null)
            {
                _moveUpToolStripMenuItem.Enabled = w.PrevNode != null;
            }

            if (_moveDownToolStripMenuItem != null)
            {
                _moveDownToolStripMenuItem.Enabled = w.NextNode != null;
            }

            if (_deleteToolStripMenuItem != null)
            {
                _deleteToolStripMenuItem.Enabled = w.Parent != null;
            }
        }

        #endregion

        public SCN0LightWrapper(IWin32Window owner)
        {
            _owner = owner;
            ContextMenuStrip = _menu;
        }

        public SCN0LightWrapper()
        {
            _owner = null;
            ContextMenuStrip = _menu;
        }
    }
}