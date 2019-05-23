using System;
using BrawlLib.SSBB.ResourceNodes;
using System.Windows.Forms;
using System.ComponentModel;
using BrawlLib;

namespace BrawlBox.NodeWrappers
{
    [NodeWrapper(ResourceType.REL)]
    public class RELWrapper : GenericWrapper
    {
        #region Menu
        private static ContextMenuStrip _menu;
        static RELWrapper()
        {
            _menu = new ContextMenuStrip();
            _menu.Items.Add(new ToolStripMenuItem("Convert Stage Module", null, ConvertAction, Keys.Control | Keys.C));
            _menu.Items.Add(new ToolStripSeparator());
            _menu.Items.Add(new ToolStripMenuItem("Open Constructor Function", null, ConstructorAction));
            _menu.Items.Add(new ToolStripMenuItem("Open Destructor Function", null, DestructorAction));
            _menu.Items.Add(new ToolStripMenuItem("Open Unresolved Function", null, UnresolvedAction));
            _menu.Items.Add(new ToolStripSeparator());
            _menu.Items.Add(new ToolStripMenuItem("&Export", null, ExportAction, Keys.Control | Keys.E));
            _menu.Items.Add(new ToolStripMenuItem("&Replace", null, ReplaceAction, Keys.Control | Keys.R));
            _menu.Items.Add(new ToolStripMenuItem("Res&tore", null, RestoreAction, Keys.Control | Keys.T));
            _menu.Items.Add(new ToolStripSeparator());
            _menu.Items.Add(new ToolStripMenuItem("Move &Up", null, MoveUpAction, Keys.Control | Keys.Up));
            _menu.Items.Add(new ToolStripMenuItem("Move D&own", null, MoveDownAction, Keys.Control | Keys.Down));
            _menu.Items.Add(new ToolStripMenuItem("Re&name", null, RenameAction, Keys.Control | Keys.N));
            _menu.Items.Add(new ToolStripSeparator());
            _menu.Items.Add(new ToolStripMenuItem("&Delete", null, DeleteAction, Keys.Control | Keys.Delete));
            _menu.Opening += MenuOpening;
            _menu.Closing += MenuClosing;
        }
        protected static void ConvertAction(object sender, EventArgs e) { GetInstance<RELWrapper>().Convert(); }
        protected static void ConstructorAction(object sender, EventArgs e) { GetInstance<RELWrapper>().Constructor(); }
        protected static void DestructorAction(object sender, EventArgs e) { GetInstance<RELWrapper>().Destructor(); }
        protected static void UnresolvedAction(object sender, EventArgs e) { GetInstance<RELWrapper>().Unresolved(); }
        private static void MenuClosing(object sender, ToolStripDropDownClosingEventArgs e)
        {
            _menu.Items[7].Enabled = _menu.Items[8].Enabled = _menu.Items[10].Enabled = _menu.Items[11].Enabled = _menu.Items[14].Enabled = true;
        }
        private static void MenuOpening(object sender, CancelEventArgs e)
        {
            RELWrapper w = GetInstance<RELWrapper>();
            _menu.Items[7].Enabled = _menu.Items[14].Enabled = w.Parent != null;
            _menu.Items[8].Enabled = ((w._resource.IsDirty) || (w._resource.IsBranch));
            _menu.Items[10].Enabled = w.PrevNode != null;
            _menu.Items[11].Enabled = w.NextNode != null;
        }

        #endregion

        public RELWrapper() { ContextMenuStrip = _menu; }

        public void Convert()
        {
            using (StageModuleConverter dlg = new StageModuleConverter())
            {
                dlg.Path = _resource.FilePath;
                if (dlg.ShowDialog(null) == DialogResult.OK)
                {
                    _resource.ReplaceRaw(dlg.ToFileMap());
                    _resource.Name = dlg.OutputName;
                }
            }
        }

        public void Constructor()
        {
            RELNode r = _resource as RELNode;

                ModuleDataNode s = r.Sections[r._prologSection];

                foreach (SectionEditor l in SectionEditor._openedSections)
                    if (l._section == s)
                    {
                        l.Focus();
                        l.Position = r._prologOffset;
                        l.hexBox1.Focus();
                        return;
                    }

                SectionEditor e = new SectionEditor(s as ModuleSectionNode);
                e.Show();
                e.Position = r._prologOffset;
                e.hexBox1.Focus();
        }

        public void Destructor()
        {
            RELNode r = _resource as RELNode;

                ModuleDataNode s = r.Sections[r._epilogSection];

                foreach (SectionEditor l in SectionEditor._openedSections)
                    if (l._section == s)
                    {
                        l.Focus();
                        l.Position = r._epilogOffset;
                        l.hexBox1.Focus();
                        return;
                    }

                SectionEditor e = new SectionEditor(s as ModuleSectionNode);
                e.Show();
                e.Position = r._epilogOffset;
                e.hexBox1.Focus();

        }

        public void Unresolved()
        {
            RELNode r = _resource as RELNode;

                ModuleDataNode s = r.Sections[r._unresolvedSection];

                foreach (SectionEditor l in SectionEditor._openedSections)
                    if (l._section == s)
                    {
                        l.Focus();
                        l.Position = r._unresolvedOffset;
                        l.hexBox1.Focus();
                        return;
                    }

                SectionEditor e = new SectionEditor(s as ModuleSectionNode);
                e.Show();
                e.Position = r._unresolvedOffset;
                e.hexBox1.Focus();
        }

        public override string ExportFilter { get { return FileFilters.REL; } }
    }

    [NodeWrapper(ResourceType.RELSection)]
    class RELSectionWrapper : GenericWrapper
    {
        #region Menu
        private static ContextMenuStrip _menu;
        static RELSectionWrapper()
        {
            _menu = new ContextMenuStrip();
            _menu.Items.Add(new ToolStripMenuItem("O&pen in Memory Viewer", null, OpenAction, Keys.Control | Keys.P));
            _menu.Items.Add(new ToolStripSeparator());
            _menu.Items.Add(new ToolStripMenuItem("&Export", null, ExportAction, Keys.Control | Keys.E));
            _menu.Items.Add(new ToolStripMenuItem("Res&tore", null, RestoreAction, Keys.Control | Keys.T));
            _menu.Opening += MenuOpening;
            _menu.Closing += MenuClosing;
        }
        protected static void OpenAction(object sender, EventArgs e) { GetInstance<RELSectionWrapper>().Open(); }
        protected static void Export2Action(object sender, EventArgs e) { GetInstance<RELSectionWrapper>().Export2(); }
        private static void MenuClosing(object sender, ToolStripDropDownClosingEventArgs e)
        {
            _menu.Items[3].Enabled = true;
        }
        private static void MenuOpening(object sender, CancelEventArgs e)
        {
            RELSectionWrapper w = GetInstance<RELSectionWrapper>();
            _menu.Items[3].Enabled = ((w._resource.IsDirty) || (w._resource.IsBranch));
        }

        #endregion

        public RELSectionWrapper() { ContextMenuStrip = _menu; }

        public void Export2()
        {
            //if (_modelViewerOpen)
            //    return;

            //string outPath;
            //int index = Program.SaveFile(ExportFilter, Text, out outPath);
            //if (index != 0)
            //    (_resource as ModuleSectionNode).ExportInitialized(outPath);
        }

        public void Open()
        {
            ModuleSectionNode r = _resource as ModuleSectionNode;

            foreach (SectionEditor l in SectionEditor._openedSections)
                if (l._section == r)
                {
                    l.Focus();
                    return;
                }

            new SectionEditor(r).Show();
        }

        public override string ExportFilter { get { return FileFilters.Raw; } }
    }

    [NodeWrapper(ResourceType.RELMethod)]
    public class RELMethodWrapper : GenericWrapper
    {
        #region Menu
        private static ContextMenuStrip _menu;
        static RELMethodWrapper()
        {
            _menu = new ContextMenuStrip();
            _menu.Items.Add(new ToolStripMenuItem("O&pen in Memory Viewer", null, OpenAction, Keys.Control | Keys.P));
            _menu.Items.Add(new ToolStripSeparator());
            //_menu.Items.Add(new ToolStripMenuItem("&Export", null, ExportAction, Keys.Control | Keys.E));
            _menu.Items.Add(new ToolStripMenuItem("Res&tore", null, RestoreAction, Keys.Control | Keys.T));
            _menu.Opening += MenuOpening;
            _menu.Closing += MenuClosing;
        }
        protected static void OpenAction(object sender, EventArgs e) { GetInstance<RELMethodWrapper>().Open(); }
        //protected static void Export2Action(object sender, EventArgs e) { GetInstance<RELMethodWrapper>().Export2(); }
        private static void MenuClosing(object sender, ToolStripDropDownClosingEventArgs e)
        {
            _menu.Items[2].Enabled = true;
        }
        private static void MenuOpening(object sender, CancelEventArgs e)
        {
            RELMethodWrapper w = GetInstance<RELMethodWrapper>();
            _menu.Items[2].Enabled = ((w._resource.IsDirty) || (w._resource.IsBranch));
        }

        #endregion

        public RELMethodWrapper() { ContextMenuStrip = _menu; }

        public void Export2()
        {
            //if (_modelViewerOpen)
            //    return;

            //string outPath;
            //int index = Program.SaveFile(ExportFilter, Text, out outPath);
            //if (index != 0)
            //    (_resource as ModuleSectionNode).ExportInitialized(outPath);
        }

        public void Open()
        {
            RELMethodNode r = _resource as RELMethodNode;
            ModuleSectionNode section = r.Root.Children[(int)r.TargetSection] as ModuleSectionNode;

            foreach (SectionEditor l in SectionEditor._openedSections)
                if (l._section == section)
                {
                    if (l.Position != r.RootOffset - section.RootOffset)
                        l.Position = r.RootOffset - section.RootOffset;

                    l.Focus();
                    return;
                }

            SectionEditor x = new SectionEditor(section);
            x.Show();
            x.Text = String.Format("Module Section Editor - {0}->{1}", r.Object._name, r._name);

            x.Position = r.RootOffset - section.RootOffset;
            x.hexBox1.Focus();
        }

        public override string ExportFilter { get { return FileFilters.Raw; } }
    }

    [NodeWrapper(ResourceType.RELExternalMethod)]
    public class RELExternalMethodWrapper : GenericWrapper
    {
        public RELExternalMethodWrapper() { BackColor = System.Drawing.Color.FromArgb(255, 255, 180, 180); }

    }

    [NodeWrapper(ResourceType.RELInheritance)]
    public class RELInheritanceWrapper : GenericWrapper
    {
        #region Menu
        private static ContextMenuStrip _menu;
        static RELInheritanceWrapper()
        {
            _menu = new ContextMenuStrip();
            _menu.Items.Add(new ToolStripMenuItem("O&pen in Memory Viewer", null, OpenAction, Keys.Control | Keys.P));
            _menu.Items.Add(new ToolStripSeparator());
            _menu.Items.Add(new ToolStripMenuItem("&Export", null, ExportAction, Keys.Control | Keys.E));
            _menu.Items.Add(new ToolStripMenuItem("Res&tore", null, RestoreAction, Keys.Control | Keys.T));
            _menu.Opening += MenuOpening;
            _menu.Closing += MenuClosing;
        }
        protected static void OpenAction(object sender, EventArgs e) { GetInstance<RELInheritanceWrapper>().Open(); }
        protected static void Export2Action(object sender, EventArgs e) { GetInstance<RELInheritanceWrapper>().Export2(); }
        private static void MenuClosing(object sender, ToolStripDropDownClosingEventArgs e)
        {
            _menu.Items[3].Enabled = true;
        }
        private static void MenuOpening(object sender, CancelEventArgs e)
        {
            RELInheritanceWrapper w = GetInstance<RELInheritanceWrapper>();
            _menu.Items[3].Enabled = ((w._resource.IsDirty) || (w._resource.IsBranch));
        }

        #endregion

        public RELInheritanceWrapper() { ContextMenuStrip = _menu; }

        public void Export2()
        {
            //if (_modelViewerOpen)
            //    return;

            //string outPath;
            //int index = Program.SaveFile(ExportFilter, Text, out outPath);
            //if (index != 0)
            //    (_resource as ModuleSectionNode).ExportInitialized(outPath);
        }

        public void Open()
        {
            InheritanceItemNode r = _resource as InheritanceItemNode;
            ModuleSectionNode section = r.Root.Children[5] as ModuleSectionNode;


            foreach (SectionEditor l in SectionEditor._openedSections)
                if (l._section == section)
                {
                    l.Focus();
                    return;
                }



            SectionEditor x = new SectionEditor(section);
            x.Show();
            x.Text = String.Format("Module Section Editor - {0}", section._name);
            x.Position = r.RootOffset;
            x.hexBox1.Focus();
        }

        public override string ExportFilter { get { return FileFilters.Raw; } }
    }
}
