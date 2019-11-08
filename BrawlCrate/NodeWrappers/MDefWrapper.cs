using BrawlCrate.UI;
using BrawlLib.Internal;
using BrawlLib.Internal.Windows.Forms;
using BrawlLib.SSBB;
using BrawlLib.SSBB.ResourceNodes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;

namespace BrawlCrate.NodeWrappers
{
    [NodeWrapper(ResourceType.MDef)]
    internal class MDefWrapper : GenericWrapper
    {
        #region Menu

        private static ContextMenuStrip _menu;

        static MDefWrapper()
        {
            _menu = new ContextMenuStrip();
            _menu.Items.Add(new ToolStripMenuItem("&Load Model", null, LoadModelAction, Keys.Control | Keys.L));
            _menu.Items.Add(new ToolStripMenuItem("&Save Info to Text Files", null, SaveDataAction,
                Keys.Control | Keys.T));
            _menu.Items.Add(new ToolStripSeparator());
            _menu.Items.Add(new ToolStripMenuItem("&Replace", null, ReplaceAction, Keys.Control | Keys.R));
            _menu.Items.Add(new ToolStripMenuItem("&Export", null, ExportAction, Keys.Control | Keys.E));
            _menu.Items.Add(new ToolStripMenuItem("&Delete", null, DeleteAction, Keys.Control | Keys.D));
            _menu.Opening += MenuOpening;
            _menu.Closing += MenuClosing;
        }

        protected static void LoadModelAction(object sender, EventArgs e)
        {
            GetInstance<MDefWrapper>().LoadModel();
        }

        protected static void SaveDataAction(object sender, EventArgs e)
        {
            GetInstance<MDefWrapper>().SaveMovesetData();
        }

        private static void MenuClosing(object sender, ToolStripDropDownClosingEventArgs e)
        {
            _menu.Items[5].Enabled = true;
        }

        private static void MenuOpening(object sender, CancelEventArgs e)
        {
            MDefWrapper w = GetInstance<MDefWrapper>();
            _menu.Items[5].Enabled = w.Parent != null;
        }

        #endregion

        public MDefWrapper()
        {
            ContextMenuStrip = _menu;
        }

        public override string ExportFilter => FileFilters.MDef;

        public void LoadModel()
        {
            OpenFileDialog o = new OpenFileDialog();
            o.Filter = "All Available Formats (*.mdl0, *.pac)|*.mdl0;*.pac|" + "MDL0 Raw Model (*.mdl0)|*.mdl0|" +
                       "PAC Archive (*.pac)|*.pac";
            o.Title = "Please select a file to load a model from.";
            if (o.ShowDialog() == DialogResult.OK)
            {
                if (o.FileName.EndsWith(".pac", StringComparison.OrdinalIgnoreCase))
                {
                    ResourceNode r = (ARCNode) NodeFactory.FromFile(null, o.FileName);
                    for (int i = 0; i < 3; i++)
                    {
                        if (r.Children.Count > 0)
                        {
                            r = r.Children[0];
                        }
                    }

                    if (r is MDL0Node)
                    {
                        if ((((MoveDefNode) _resource)._model = (MDL0Node) r) != null)
                        {
                            ((MoveDefNode) _resource)._model.Populate();
                        }
                    }
                }
                else if (o.FileName.EndsWith(".mdl0", StringComparison.OrdinalIgnoreCase))
                {
                    if ((((MoveDefNode) _resource)._model = (MDL0Node) NodeFactory.FromFile(null, o.FileName)) != null)
                    {
                        ((MoveDefNode) _resource)._model.Populate();
                    }
                }
            }
        }

        public void SaveEvents()
        {
            if (!Directory.Exists(Application.StartupPath + "/MovesetData"))
            {
                Directory.CreateDirectory(Application.StartupPath + "/MovesetData");
            }

            MoveDefNode node = _resource as MoveDefNode;

            if (MoveDefNode.EventDictionary == null)
            {
                MoveDefNode.LoadEventDictionary();
            }

            bool go = true;
            string events = Application.StartupPath + "/MovesetData/Events.txt";
            if (File.Exists(events))
            {
                if (MessageBox.Show("Do you want to overwrite Events.txt in the MovesetData folder?",
                        "Overwrite Permission", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    go = true;
                }
                else
                {
                    go = false;
                }
            }
            else
            {
                go = true;
            }

            if (go)
            {
                using (StreamWriter file = new StreamWriter(events))
                {
                    foreach (ActionEventInfo info in MoveDefNode.EventDictionary.Values)
                    {
                        file.WriteLine(Helpers.Hex8(info.idNumber));
                        file.WriteLine(info._name);
                        file.WriteLine(info._description);
                        string s = "";
                        foreach (int i in info.defaultParams)
                        {
                            s += i;
                        }

                        file.WriteLine(s);
                        file.WriteLine();
                    }
                }
            }
        }

        public void SaveParameters()
        {
            if (!Directory.Exists(Application.StartupPath + "/MovesetData"))
            {
                Directory.CreateDirectory(Application.StartupPath + "/MovesetData");
            }

            MoveDefNode node = _resource as MoveDefNode;

            if (MoveDefNode.EventDictionary == null)
            {
                MoveDefNode.LoadEventDictionary();
            }

            bool go = true;
            string parameters = Application.StartupPath + "/MovesetData/Parameters.txt";

            if (File.Exists(parameters))
            {
                if (MessageBox.Show("Do you want to overwrite Parameters.txt in the MovesetData folder?",
                        "Overwrite Permission", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    go = true;
                }
                else
                {
                    go = false;
                }
            }
            else
            {
                go = true;
            }

            if (go)
            {
                using (StreamWriter file = new StreamWriter(parameters))
                {
                    foreach (ActionEventInfo info in MoveDefNode.EventDictionary.Values)
                    {
                        if (info.Params != null && info.Params.Length > 0)
                        {
                            file.WriteLine(Helpers.Hex8(info.idNumber));
                            for (int i = 0; i < info.Params.Length; i++)
                            {
                                file.WriteLine(info.Params[i]);
                                if (info.pDescs.Length > i)
                                {
                                    file.WriteLine(info.pDescs[i]);
                                }
                                else
                                {
                                    file.WriteLine();
                                }
                            }

                            file.WriteLine();
                        }
                    }
                }
            }
        }

        public void SaveEventSyntax()
        {
            if (!Directory.Exists(Application.StartupPath + "/MovesetData"))
            {
                Directory.CreateDirectory(Application.StartupPath + "/MovesetData");
            }

            MoveDefNode node = _resource as MoveDefNode;

            if (MoveDefNode.EventDictionary == null)
            {
                MoveDefNode.LoadEventDictionary();
            }

            bool go = true;
            string syntax = Application.StartupPath + "/MovesetData/EventSyntax.txt";

            if (File.Exists(syntax))
            {
                if (MessageBox.Show("Do you want to overwrite EventSyntax.txt in the MovesetData folder?",
                        "Overwrite Permission", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    go = true;
                }
                else
                {
                    go = false;
                }
            }
            else
            {
                go = true;
            }

            if (go)
            {
                using (StreamWriter file = new StreamWriter(syntax))
                {
                    foreach (ActionEventInfo info in MoveDefNode.EventDictionary.Values)
                    {
                        if (string.IsNullOrEmpty(info._syntax))
                        {
                            continue;
                        }

                        file.WriteLine(Helpers.Hex8(info.idNumber));
                        file.WriteLine(info._syntax);
                        file.WriteLine();
                    }
                }
            }
        }

        public void SaveAttributes()
        {
            if (!Directory.Exists(Application.StartupPath + "/MovesetData"))
            {
                Directory.CreateDirectory(Application.StartupPath + "/MovesetData");
            }

            MoveDefNode node = _resource as MoveDefNode;

            if (node.iRequirements == null ||
                node.AttributeArray == null ||
                node.iAirGroundStats == null ||
                node.iCollisionStats == null)
            {
                node.LoadOtherData();
            }

            bool go = true;
            string attributes = Application.StartupPath + "/MovesetData/Attributes.txt";

            if (File.Exists(attributes))
            {
                if (MessageBox.Show("Do you want to overwrite Attributes.txt in the MovesetData folder?",
                        "Overwrite Permission", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    go = true;
                }
                else
                {
                    go = false;
                }
            }
            else
            {
                go = true;
            }

            if (go)
            {
                using (StreamWriter file = new StreamWriter(attributes))
                {
                    foreach (AttributeInfo i in node.AttributeArray)
                    {
                        file.WriteLine(i._name);
                        file.WriteLine(i._description);
                        file.WriteLine(i._type);
                        file.WriteLine();
                    }
                }
            }

            go = true;

            if (!Directory.Exists(Application.StartupPath + "/MovesetData/CharSpecific"))
            {
                Directory.CreateDirectory(Application.StartupPath + "/MovesetData/CharSpecific");
            }

            string Params = Application.StartupPath + "/MovesetData/CharSpecific/" + _resource.Parent.Name + ".txt";

            if (File.Exists(Params))
            {
                if (MessageBox.Show(
                        "Do you want to overwrite " + _resource.Parent.Name +
                        ".txt in the MovesetData/CharSpecific folder?", "Overwrite Permission",
                        MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    go = true;
                }
                else
                {
                    go = false;
                }
            }
            else
            {
                go = true;
            }

            if (go)
            {
                using (StreamWriter file = new StreamWriter(Params))
                {
                    foreach (KeyValuePair<string, SectionParamInfo> i in node.Params)
                    {
                        file.WriteLine(i.Key);
                        file.WriteLine(i.Value._newName);
                        foreach (AttributeInfo a in i.Value._attributes)
                        {
                            file.WriteLine(a._name);
                            file.WriteLine(a._description);
                            file.WriteLine(a._type);
                            file.WriteLine();
                        }

                        file.WriteLine();
                    }
                }
            }
        }

        public void SaveRequirements()
        {
            if (!Directory.Exists(Application.StartupPath + "/MovesetData"))
            {
                Directory.CreateDirectory(Application.StartupPath + "/MovesetData");
            }

            MoveDefNode node = _resource as MoveDefNode;

            if (node.iRequirements == null ||
                node.AttributeArray == null ||
                node.iAirGroundStats == null ||
                node.iCollisionStats == null)
            {
                node.LoadOtherData();
            }

            bool go = true;
            string requirements = Application.StartupPath + "/MovesetData/Requirements.txt";

            if (File.Exists(requirements))
            {
                if (MessageBox.Show("Do you want to overwrite Requirements.txt in the MovesetData folder?",
                        "Overwrite Permission", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    go = true;
                }
                else
                {
                    go = false;
                }
            }
            else
            {
                go = true;
            }

            if (go)
            {
                using (StreamWriter file = new StreamWriter(requirements))
                {
                    foreach (string i in node.iRequirements)
                    {
                        file.WriteLine(i);
                    }
                }
            }
        }

        public void SaveEnums()
        {
            if (!Directory.Exists(Application.StartupPath + "/MovesetData"))
            {
                Directory.CreateDirectory(Application.StartupPath + "/MovesetData");
            }

            MoveDefNode node = _resource as MoveDefNode;

            if (node.iRequirements == null ||
                node.AttributeArray == null ||
                node.iAirGroundStats == null ||
                node.iCollisionStats == null)
            {
                node.LoadOtherData();
            }

            if (MoveDefNode.EventDictionary == null)
            {
                MoveDefNode.LoadEventDictionary();
            }

            bool go = true;
            string airground = Application.StartupPath + "/MovesetData/AirGroundStats.txt";
            string collision = Application.StartupPath + "/MovesetData/CollisionStats.txt";
            string gfx = Application.StartupPath + "/MovesetData/GFXFiles.txt";
            string enums = Application.StartupPath + "/MovesetData/Enums.txt";

            if (File.Exists(airground))
            {
                if (MessageBox.Show("Do you want to overwrite AirGroundStats.txt in the MovesetData folder?",
                        "Overwrite Permission", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    go = true;
                }
                else
                {
                    go = false;
                }
            }
            else
            {
                go = true;
            }

            if (go)
            {
                using (StreamWriter file = new StreamWriter(airground))
                {
                    foreach (string i in node.iAirGroundStats)
                    {
                        file.WriteLine(i);
                    }
                }
            }

            if (File.Exists(collision))
            {
                if (MessageBox.Show("Do you want to overwrite CollisionStats.txt in the MovesetData folder?",
                        "Overwrite Permission", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    go = true;
                }
                else
                {
                    go = false;
                }
            }
            else
            {
                go = true;
            }

            if (go)
            {
                using (StreamWriter file = new StreamWriter(collision))
                {
                    foreach (string i in node.iCollisionStats)
                    {
                        file.WriteLine(i);
                    }
                }
            }

            if (File.Exists(gfx))
            {
                if (MessageBox.Show("Do you want to overwrite GFXFiles.txt in the MovesetData folder?",
                        "Overwrite Permission", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    go = true;
                }
                else
                {
                    go = false;
                }
            }
            else
            {
                go = true;
            }

            if (go)
            {
                using (StreamWriter file = new StreamWriter(gfx))
                {
                    foreach (string i in node.iGFXFiles)
                    {
                        file.WriteLine(i);
                    }
                }
            }

            if (File.Exists(enums))
            {
                if (MessageBox.Show("Do you want to overwrite Enums.txt in the MovesetData folder?",
                        "Overwrite Permission", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    go = true;
                }
                else
                {
                    go = false;
                }
            }
            else
            {
                go = true;
            }

            if (go)
            {
                //list of enum dictionaries
                List<Dictionary<int, List<string>>> EnumList = new List<Dictionary<int, List<string>>>();
                //enum index, event id, param index list
                Dictionary<int, Dictionary<long, List<int>>> EnumIds =
                    new Dictionary<int, Dictionary<long, List<int>>>();
                //remap events and enums
                foreach (ActionEventInfo info in MoveDefNode.EventDictionary.Values)
                {
                    if (info.Enums != null && info.Enums.Count > 0)
                    {
                        List<int> p = new List<int>();

                        bool has = false;
                        int index = -1;
                        foreach (Dictionary<int, List<string>> t in EnumList)
                        {
                            bool match = true;
                            index++;
                            foreach (int g in info.Enums.Keys)
                            {
                                if (t.ContainsKey(g))
                                {
                                    int r = 0;
                                    foreach (string s in t[g])
                                    {
                                        if (s != info.Enums[g][r++])
                                        {
                                            match = false;
                                            break;
                                        }
                                    }
                                }
                                else
                                {
                                    match = false;
                                }
                            }

                            if (match)
                            {
                                has = true;
                                break;
                            }
                            else
                            {
                                continue;
                            }
                        }

                        if (!has)
                        {
                            EnumList.Add(info.Enums);
                            index = EnumList.Count - 1;
                        }

                        if (!EnumIds.ContainsKey(index))
                        {
                            EnumIds.Add(index, new Dictionary<long, List<int>>());
                        }

                        foreach (int i in info.Enums.Keys)
                        {
                            p.Add(i);
                        }

                        EnumIds[index].Add(info.idNumber, p);
                    }
                }

                using (StreamWriter file = new StreamWriter(enums))
                {
                    int i = 0;
                    foreach (Dictionary<int, List<string>> d in EnumList)
                    {
                        Dictionary<long, List<int>> events = EnumIds[i];
                        foreach (KeyValuePair<long, List<int>> ev in events)
                        {
                            file.WriteLine(Helpers.Hex8(ev.Key));
                            foreach (int v in ev.Value)
                            {
                                file.WriteLine(v);
                            }

                            file.WriteLine();
                        }

                        file.WriteLine();
                        foreach (int x in d.Keys)
                        {
                            if (d[x] != null && d[x].Count > 0)
                            {
                                foreach (string value in d[x])
                                {
                                    file.WriteLine(value);
                                }
                            }
                        }

                        file.WriteLine();
                        i++;
                    }
                }
            }
        }

        public void SaveMovesetData()
        {
            SaveEvents();
            SaveParameters();
            SaveEventSyntax();
            SaveAttributes();
            SaveRequirements();
            SaveEnums();
        }
    }

    [NodeWrapper(ResourceType.Event)]
    internal class MDefEventWrapper : GenericWrapper
    {
        #region Menu

        private static ContextMenuStrip _menu;

        static MDefEventWrapper()
        {
            _menu = new ContextMenuStrip();
            _menu.Items.Add(new ToolStripMenuItem("&Edit Dictionary", null,
                new ToolStripMenuItem("Name", null, EditDictionaryName, Keys.Control | Keys.N),
                new ToolStripMenuItem("Description", null, EditDictionaryDescription, Keys.Control | Keys.D),
                new ToolStripMenuItem("Syntax", null, EditDictionarySyntax, Keys.Control | Keys.T)));
            _menu.Items.Add(new ToolStripSeparator());
            _menu.Items.Add(new ToolStripMenuItem("Move &Up", null, MoveUpAction, Keys.Control | Keys.Up));
            _menu.Items.Add(new ToolStripMenuItem("Move D&own", null, MoveDownAction, Keys.Control | Keys.Down));
            _menu.Items.Add(new ToolStripSeparator());
            _menu.Items.Add(new ToolStripMenuItem("&Export", null, ExportAction, Keys.Control | Keys.E));
            _menu.Items.Add(new ToolStripMenuItem("&Delete", null, DeleteAction, Keys.Control | Keys.Delete));
            _menu.Opening += MenuOpening;
            _menu.Closing += MenuClosing;
        }

        protected static void EditDictionaryName(object sender, EventArgs e)
        {
            GetInstance<MDefEventWrapper>().EditDictionaryName();
        }

        protected static void EditDictionaryDescription(object sender, EventArgs e)
        {
            GetInstance<MDefEventWrapper>().EditDictionaryDescription();
        }

        protected static void EditDictionarySyntax(object sender, EventArgs e)
        {
            GetInstance<MDefEventWrapper>().EditDictionarySyntax();
        }

        private static void MenuClosing(object sender, ToolStripDropDownClosingEventArgs e)
        {
            _menu.Items[2].Enabled = _menu.Items[3].Enabled = _menu.Items[6].Enabled = true;
        }

        private static void MenuOpening(object sender, CancelEventArgs e)
        {
            MDefEventWrapper w = GetInstance<MDefEventWrapper>();
            _menu.Items[6].Enabled = w.Parent != null;
            _menu.Items[2].Enabled = w.PrevNode != null;
            _menu.Items[3].Enabled = w.NextNode != null;
        }

        #endregion

        public MDefEventWrapper()
        {
            ContextMenuStrip = _menu;
        }

        public void EditDictionaryName()
        {
            DialogResult res;
            MoveDefEventNode e = _resource as MoveDefEventNode;
            uint ev = e._event;
            MoveDefEntryNode temp = new MoveDefEntryNode();
            if (MoveDefNode.EventDictionary.ContainsKey(ev))
            {
                temp.Name = MoveDefNode.EventDictionary[ev]._name;
            }
            else
            {
                temp.Name = "";
            }

            using (RenameDialog dlg = new RenameDialog())
            {
                res = dlg.ShowDialog(MainForm.Instance, temp);
            }

            if (res == DialogResult.OK)
            {
                if (MoveDefNode.EventDictionary.ContainsKey(ev))
                {
                    MoveDefNode.EventDictionary[ev]._name = temp.Name;
                }
                else
                {
                    MoveDefNode.EventDictionary.Add(ev,
                        new ActionEventInfo
                        {
                            Params = new string[_resource.Children.Count],
                            pDescs = new string[_resource.Children.Count], _name = temp.Name, idNumber = ev
                        });
                }

                MoveDefNode._dictionaryChanged = true;
                foreach (MoveDefEventNode n in e.Root._events[ev])
                {
                    n.Name = temp.Name;
                    n.HasChanged = false;
                }
            }

            temp.Dispose();
            temp = null;
        }

        public void EditDictionaryDescription()
        {
            DialogResult res;
            MoveDefEventNode e = _resource as MoveDefEventNode;
            long ev = e._event;
            MoveDefEntryNode temp = new MoveDefEntryNode();
            if (MoveDefNode.EventDictionary.ContainsKey(ev))
            {
                temp.Name = MoveDefNode.EventDictionary[ev]._description;
            }
            else
            {
                temp.Name = "";
            }

            using (RenameDialog dlg = new RenameDialog())
            {
                res = dlg.ShowDialog(MainForm.Instance, temp);
            }

            if (res == DialogResult.OK)
            {
                if (MoveDefNode.EventDictionary.ContainsKey(ev))
                {
                    MoveDefNode.EventDictionary[ev]._description = temp.Name;
                }
                else
                {
                    MoveDefNode.EventDictionary.Add(ev,
                        new ActionEventInfo
                        {
                            Params = new string[_resource.Children.Count],
                            pDescs = new string[_resource.Children.Count], _description = temp.Name,
                            idNumber = ev
                        });
                }

                MoveDefNode._dictionaryChanged = true;
            }

            temp.Dispose();
            temp = null;
        }

        public void EditDictionarySyntax()
        {
            DialogResult res;
            MoveDefEventNode e = _resource as MoveDefEventNode;
            long ev = e._event;
            MoveDefEntryNode temp = new MoveDefEntryNode();
            if (MoveDefNode.EventDictionary.ContainsKey(ev))
            {
                temp.Name = MoveDefNode.EventDictionary[ev]._syntax;
            }
            else
            {
                temp.Name = "";
            }

            using (RenameDialog dlg = new RenameDialog())
            {
                res = dlg.ShowDialog(MainForm.Instance, temp);
            }

            if (res == DialogResult.OK)
            {
                if (MoveDefNode.EventDictionary.ContainsKey(ev))
                {
                    MoveDefNode.EventDictionary[ev]._syntax = temp.Name;
                }
                else
                {
                    MoveDefNode.EventDictionary.Add(ev,
                        new ActionEventInfo
                        {
                            Params = new string[_resource.Children.Count],
                            pDescs = new string[_resource.Children.Count], _syntax = temp.Name,
                            idNumber = ev
                        });
                }

                MoveDefNode._dictionaryChanged = true;
            }

            temp.Dispose();
            temp = null;
        }
    }

    [NodeWrapper(ResourceType.Parameter)]
    internal class MDefEventParameterWrapper : GenericWrapper
    {
        #region Menu

        private static ContextMenuStrip _menu;

        static MDefEventParameterWrapper()
        {
            _menu = new ContextMenuStrip();
            _menu.Items.Add(new ToolStripMenuItem("&Edit Dictionary", null,
                new ToolStripMenuItem("Name", null, EditDictionaryName, Keys.Control | Keys.N),
                new ToolStripMenuItem("Description", null, EditDictionaryDescription, Keys.Control | Keys.D)));
            _menu.Items.Add(new ToolStripSeparator());
            _menu.Items.Add(new ToolStripMenuItem("&Export", null, ExportAction, Keys.Control | Keys.E));
            _menu.Opening += MenuOpening;
            _menu.Closing += MenuClosing;
        }

        protected static void EditDictionaryName(object sender, EventArgs e)
        {
            GetInstance<MDefEventParameterWrapper>().EditDictionaryName();
        }

        protected static void EditDictionaryDescription(object sender, EventArgs e)
        {
            GetInstance<MDefEventParameterWrapper>().EditDictionaryDescription();
        }

        private static void MenuClosing(object sender, ToolStripDropDownClosingEventArgs e)
        {
            _menu.Items[2].Enabled = true;
        }

        private static void MenuOpening(object sender, CancelEventArgs e)
        {
            MDefEventParameterWrapper w = GetInstance<MDefEventParameterWrapper>();
            _menu.Items[2].Enabled = w.Parent != null;
        }

        #endregion

        public MDefEventParameterWrapper()
        {
            ContextMenuStrip = _menu;
        }

        public void EditDictionaryName()
        {
            DialogResult res;
            MoveDefEventParameterNode e = _resource as MoveDefEventParameterNode;
            uint ev = (e.Parent as MoveDefEventNode)._event;
            MoveDefEntryNode temp = new MoveDefEntryNode();
            if (MoveDefNode.EventDictionary.ContainsKey(ev))
            {
                temp.Name = MoveDefNode.EventDictionary[ev].Params[_resource.Index];
            }
            else
            {
                temp.Name = "";
            }

            using (RenameDialog dlg = new RenameDialog())
            {
                res = dlg.ShowDialog(MainForm.Instance, temp);
            }

            if (res == DialogResult.OK)
            {
                if (!MoveDefNode.EventDictionary.ContainsKey(ev))
                {
                    MoveDefNode.EventDictionary.Add(ev,
                        new ActionEventInfo
                        {
                            Params = new string[_resource.Parent.Children.Count],
                            pDescs = new string[_resource.Parent.Children.Count], idNumber = ev
                        });
                }

                MoveDefNode.EventDictionary[ev].Params[_resource.Index] = temp.Name;
                MoveDefNode._dictionaryChanged = true;
                foreach (MoveDefEventNode n in e.Root._events[ev])
                {
                    n.Children[_resource.Index].Name = temp.Name;
                    n.Children[_resource.Index].HasChanged = false;
                }
            }

            temp.Dispose();
            temp = null;
        }

        public void EditDictionaryDescription()
        {
            DialogResult res;
            MoveDefEventParameterNode e = _resource as MoveDefEventParameterNode;
            long ev = (e.Parent as MoveDefEventNode)._event;
            MoveDefEntryNode temp = new MoveDefEntryNode();
            if (MoveDefNode.EventDictionary.ContainsKey(ev))
            {
                temp.Name = MoveDefNode.EventDictionary[ev].pDescs[_resource.Index];
            }
            else
            {
                temp.Name = "";
            }

            using (RenameDialog dlg = new RenameDialog())
            {
                res = dlg.ShowDialog(MainForm.Instance, temp);
            }

            if (res == DialogResult.OK)
            {
                if (!MoveDefNode.EventDictionary.ContainsKey(ev))
                {
                    MoveDefNode.EventDictionary.Add(ev,
                        new ActionEventInfo
                        {
                            Params = new string[_resource.Parent.Children.Count],
                            pDescs = new string[_resource.Parent.Children.Count], idNumber = ev
                        });
                }

                MoveDefNode.EventDictionary[ev].pDescs[_resource.Index] = temp.Name;
                MoveDefNode._dictionaryChanged = true;
            }

            temp.Dispose();
            temp = null;
        }
    }

    [NodeWrapper(ResourceType.MDefHurtboxList)]
    internal class MDefHurtboxListWrapper : GenericWrapper
    {
        #region Menu

        private static ContextMenuStrip _menu;

        static MDefHurtboxListWrapper()
        {
            _menu = new ContextMenuStrip();
            _menu.Items.Add(new ToolStripMenuItem("Ne&w Hurtbox", null, NewHurtboxAction, Keys.Control | Keys.H));
            _menu.Items.Add(new ToolStripSeparator());
            _menu.Items.Add(new ToolStripMenuItem("&Export", null, ExportAction, Keys.Control | Keys.E));
            _menu.Opening += MenuOpening;
            _menu.Closing += MenuClosing;
        }

        protected static void NewHurtboxAction(object sender, EventArgs e)
        {
            GetInstance<MDefHurtboxListWrapper>().NewHurtbox();
        }

        private static void MenuClosing(object sender, ToolStripDropDownClosingEventArgs e)
        {
        }

        private static void MenuOpening(object sender, CancelEventArgs e)
        {
        }

        #endregion

        public MDefHurtboxListWrapper()
        {
            ContextMenuStrip = _menu;
        }

        public void NewHurtbox()
        {
            MoveDefHurtBoxNode node = new MoveDefHurtBoxNode {Enabled = true};
            _resource.AddChild(node);
            BaseWrapper res = FindResource(node, false);
            res.EnsureVisible();
            res.TreeView.SelectedNode = res;
        }
    }

    [NodeWrapper(ResourceType.MDefActionList)]
    internal class MDefActionListWrapper : GenericWrapper
    {
        #region Menu

        private static ContextMenuStrip _menu;

        static MDefActionListWrapper()
        {
            _menu = new ContextMenuStrip();
            _menu.Items.Add(new ToolStripMenuItem("Add Ne&w Action Group", null, NewAction, Keys.Control | Keys.H));
            _menu.Items.Add(new ToolStripSeparator());
            _menu.Items.Add(new ToolStripMenuItem("&Export", null, ExportAction, Keys.Control | Keys.E));
            _menu.Opening += MenuOpening;
            _menu.Closing += MenuClosing;
        }

        protected static void NewAction(object sender, EventArgs e)
        {
            GetInstance<MDefActionListWrapper>().NewActionGroup();
        }

        private static void MenuClosing(object sender, ToolStripDropDownClosingEventArgs e)
        {
        }

        private static void MenuOpening(object sender, CancelEventArgs e)
        {
        }

        #endregion

        public MDefActionListWrapper()
        {
            ContextMenuStrip = _menu;
        }

        public void NewActionGroup()
        {
            ResourceNode node = null;
            if ((node = _resource.Parent) is MoveDefArticleNode ||
                (node = _resource.Parent.Parent) is MoveDefArticleNode)
            {
                MoveDefArticleNode article = node as MoveDefArticleNode;
                if (_resource.Children[0] is MoveDefSubActionGroupNode)
                {
                    node = new MoveDefSubActionGroupNode {Name = "SubAction" + _resource.Children.Count};
                    node.AddChild(new MoveDefActionNode("Main", true, _resource));
                    node.AddChild(new MoveDefActionNode("GFX", true, _resource));
                    node.AddChild(new MoveDefActionNode("SFX", true, _resource));
                }
                else if (_resource.Children[0] is MoveDefActionGroupNode || _resource.Children[0] is MoveDefActionNode)
                {
                    if (article.pikmin)
                    {
                        node = new MoveDefActionGroupNode {Name = "Action" + _resource.Children.Count};
                        node.AddChild(new MoveDefActionNode("Entry", true, _resource));
                        node.AddChild(new MoveDefActionNode("Exit", true, _resource));
                    }
                    else
                    {
                        node = new MoveDefActionNode("Action" + _resource.Children.Count, true, _resource);
                    }

                    article.actionFlags.AddChild(new MoveDefActionFlagsEntryNode
                        {Name = "Action" + article.actionFlags.Children.Count});
                }
            }
            else if (_resource.Children[0] is MoveDefSubActionGroupNode)
            {
                node = new MoveDefSubActionGroupNode {Name = "SubAction" + _resource.Children.Count};
                node.AddChild(new MoveDefActionNode("Main", true, _resource));
                node.AddChild(new MoveDefActionNode("GFX", true, _resource));
                node.AddChild(new MoveDefActionNode("SFX", true, _resource));
                node.AddChild(new MoveDefActionNode("Other", true, _resource));
            }
            else if (_resource.Children[0] is MoveDefActionGroupNode)
            {
                node = new MoveDefActionGroupNode {Name = "Action" + (_resource.Children.Count + 274)};
                node.AddChild(new MoveDefActionNode("Entry", true, _resource));
                node.AddChild(new MoveDefActionNode("Exit", true, _resource));
                (_resource as MoveDefEntryNode).Root.data.actionFlags.AddChild(new MoveDefActionFlagsEntryNode
                {
                    Name = "Action" + ((_resource as MoveDefEntryNode).Root.data.actionFlags.Children.Count + 274)
                });
                (_resource as MoveDefEntryNode).Root.data.actionPre.AddChild(new MoveDefActionPreEntryNode
                {
                    Name = "Action" + (_resource as MoveDefEntryNode).Root.data.actionPre.Children.Count
                });
            }

            _resource.AddChild(node);
            BaseWrapper res = FindResource(node, false);
            res.EnsureVisible();
            res.TreeView.SelectedNode = res;
        }
    }

    [NodeWrapper(ResourceType.MDefSubActionGroup)]
    internal class MDefSubActionGroupWrapper : GenericWrapper
    {
        #region Menu

        private static ContextMenuStrip _menu;

        static MDefSubActionGroupWrapper()
        {
            _menu = new ContextMenuStrip();
            _menu.Items.Add(new ToolStripMenuItem("Move &Up", null, MoveUpAction, Keys.Control | Keys.Up));
            _menu.Items.Add(new ToolStripMenuItem("Move D&own", null, MoveDownAction, Keys.Control | Keys.Down));
            _menu.Items.Add(new ToolStripMenuItem("Re&name", null, RenameAction, Keys.Control | Keys.N));
            _menu.Items.Add(new ToolStripSeparator());
            _menu.Items.Add(new ToolStripMenuItem("&Delete", null, DeleteAction, Keys.Control | Keys.Delete));
            _menu.Opening += MenuOpening;
            _menu.Closing += MenuClosing;
        }

        private static void MenuClosing(object sender, ToolStripDropDownClosingEventArgs e)
        {
            _menu.Items[0].Enabled = _menu.Items[1].Enabled = _menu.Items[4].Enabled = true;
        }

        private static void MenuOpening(object sender, CancelEventArgs e)
        {
            MDefSubActionGroupWrapper w = GetInstance<MDefSubActionGroupWrapper>();
            _menu.Items[4].Enabled = w.Parent != null;
            _menu.Items[0].Enabled = w.PrevNode != null;
            _menu.Items[1].Enabled = w.NextNode != null;
        }

        #endregion

        public MDefSubActionGroupWrapper()
        {
            ContextMenuStrip = _menu;
        }
    }

    [NodeWrapper(ResourceType.MDefActionGroup)]
    internal class MDefActionGroupWrapper : GenericWrapper
    {
        #region Menu

        private static ContextMenuStrip _menu;

        static MDefActionGroupWrapper()
        {
            _menu = new ContextMenuStrip();
            _menu.Items.Add(new ToolStripMenuItem("Move &Up", null, MoveUp2Action, Keys.Control | Keys.Up));
            _menu.Items.Add(new ToolStripMenuItem("Move D&own", null, MoveDown2Action, Keys.Control | Keys.Down));
            _menu.Items.Add(new ToolStripSeparator());
            _menu.Items.Add(new ToolStripMenuItem("&Delete", null, Delete2Action, Keys.Control | Keys.Delete));
            _menu.Opening += MenuOpening;
            _menu.Closing += MenuClosing;
        }

        protected static void MoveUp2Action(object sender, EventArgs e)
        {
            GetInstance<MDefActionGroupWrapper>().MoveUp2();
        }

        protected static void MoveDown2Action(object sender, EventArgs e)
        {
            GetInstance<MDefActionGroupWrapper>().MoveDown2();
        }

        protected static void Delete2Action(object sender, EventArgs e)
        {
            GetInstance<MDefActionGroupWrapper>().Delete2();
        }

        private static void MenuClosing(object sender, ToolStripDropDownClosingEventArgs e)
        {
            _menu.Items[0].Enabled = _menu.Items[1].Enabled = _menu.Items[3].Enabled = true;
        }

        private static void MenuOpening(object sender, CancelEventArgs e)
        {
            MDefActionGroupWrapper w = GetInstance<MDefActionGroupWrapper>();
            _menu.Items[3].Enabled = w.Parent != null;
            _menu.Items[0].Enabled = w.PrevNode != null;
            _menu.Items[1].Enabled = w.NextNode != null;
        }

        #endregion

        public MDefActionGroupWrapper()
        {
            ContextMenuStrip = _menu;
        }

        public void MoveUp2()
        {
            ResourceNode n = null;
            if ((n = _resource.Parent.Parent) is MoveDefArticleNode ||
                (n = _resource.Parent.Parent.Parent) is MoveDefArticleNode)
            {
                MoveDefArticleNode article = n as MoveDefArticleNode;
                article.actionFlags.Children[_resource.Index].DoMoveUp(false);
                foreach (MoveDefActionFlagsEntryNode p in article.actionFlags.Children)
                {
                    p.Name = "Action" + p.Index;
                }
            }
            else
            {
                MoveDefNode node = (_resource as MoveDefEntryNode).Root;
                node.data.actionPre.Children[_resource.Index + 274].DoMoveUp(false);
                node.data.unk7.Children[_resource.Index + 274].DoMoveUp(false);
                node.data.actionFlags.Children[_resource.Index].DoMoveUp(false);
                foreach (MoveDefActionPreEntryNode p in node.data.actionPre.Children)
                {
                    if (!p.External)
                    {
                        p.Name = "Action" + p.Index;
                    }
                }

                foreach (MoveDefActionPreEntryNode p in node.data.unk7.Children)
                {
                    p.Name = "Action" + p.Index;
                }

                foreach (MoveDefActionFlagsEntryNode p in node.data.actionFlags.Children)
                {
                    p.Name = "Action" + (p.Index + 274);
                }
            }

            MoveUp();
            foreach (MoveDefActionGroupNode p in _resource.Parent.Children)
            {
                p.Name = "Action" + p.Index;
            }
        }

        public void MoveDown2()
        {
            ResourceNode n = null;
            if ((n = _resource.Parent.Parent) is MoveDefArticleNode ||
                (n = _resource.Parent.Parent.Parent) is MoveDefArticleNode)
            {
                MoveDefArticleNode article = n as MoveDefArticleNode;
                article.actionFlags.Children[_resource.Index].DoMoveDown(false);
                foreach (MoveDefActionFlagsEntryNode p in article.actionFlags.Children)
                {
                    p.Name = "Action" + p.Index;
                }
            }
            else
            {
                MoveDefNode node = (_resource as MoveDefEntryNode).Root;
                node.data.actionPre.Children[_resource.Index + 274].DoMoveDown(false);
                node.data.unk7.Children[_resource.Index + 274].DoMoveDown(false);
                node.data.actionFlags.Children[_resource.Index].DoMoveDown(false);
                foreach (MoveDefActionPreEntryNode p in node.data.actionPre.Children)
                {
                    if (!p.External)
                    {
                        p.Name = "Action" + p.Index;
                    }
                }

                foreach (MoveDefActionPreEntryNode p in node.data.unk7.Children)
                {
                    p.Name = "Action" + p.Index;
                }

                foreach (MoveDefActionFlagsEntryNode p in node.data.actionFlags.Children)
                {
                    p.Name = "Action" + (p.Index + 274);
                }
            }

            MoveDown();
            foreach (MoveDefActionGroupNode p in _resource.Parent.Children)
            {
                p.Name = "Action" + p.Index;
            }
        }

        public void Delete2()
        {
            ResourceNode n = null;
            if ((n = _resource.Parent.Parent) is MoveDefArticleNode ||
                (n = _resource.Parent.Parent.Parent) is MoveDefArticleNode)
            {
                MoveDefArticleNode article = n as MoveDefArticleNode;
                article.actionFlags.Children[_resource.Index].Remove();
                foreach (MoveDefActionFlagsEntryNode p in article.actionFlags.Children)
                {
                    p.Name = "Action" + p.Index;
                }
            }
            else
            {
                MoveDefNode node = (_resource as MoveDefEntryNode).Root;
                node.data.actionPre.Children[_resource.Index + 274].Remove();
                node.data.unk7.Children[_resource.Index + 274].Remove();
                node.data.actionFlags.Children[_resource.Index].Remove();
                foreach (MoveDefActionPreEntryNode p in node.data.actionPre.Children)
                {
                    if (!p.External)
                    {
                        p.Name = "Action" + p.Index;
                    }
                }

                foreach (MoveDefActionPreEntryNode p in node.data.unk7.Children)
                {
                    p.Name = "Action" + p.Index;
                }

                foreach (MoveDefActionFlagsEntryNode p in node.data.actionFlags.Children)
                {
                    p.Name = "Action" + (p.Index + 274);
                }
            }

            int sub = 0;
            foreach (MoveDefActionGroupNode p in _resource.Parent.Children)
            {
                if (p.Index == _resource.Index)
                {
                    sub = 1;
                }

                p.Name = "Action" + (p.Index - sub);
            }

            Delete();
        }
    }

    [NodeWrapper(ResourceType.MDefMdlVisRef)]
    internal class MDefMdlVisRefWrapper : GenericWrapper
    {
        #region Menu

        private static ContextMenuStrip _menu;

        static MDefMdlVisRefWrapper()
        {
            _menu = new ContextMenuStrip();
            _menu.Items.Add(new ToolStripMenuItem("Add New Switch", null, NewSwitchAction, Keys.Control | Keys.A));
            _menu.Items.Add(new ToolStripSeparator());
            _menu.Items.Add(new ToolStripMenuItem("Move &Up", null, MoveUpAction, Keys.Control | Keys.Up));
            _menu.Items.Add(new ToolStripMenuItem("Move D&own", null, MoveDownAction, Keys.Control | Keys.Down));
            _menu.Items.Add(new ToolStripMenuItem("Re&name", null, RenameAction, Keys.Control | Keys.N));
            _menu.Items.Add(new ToolStripSeparator());
            _menu.Items.Add(new ToolStripMenuItem("&Delete", null, DeleteAction, Keys.Control | Keys.Delete));
            _menu.Opening += MenuOpening;
            _menu.Closing += MenuClosing;
        }

        protected static void NewSwitchAction(object sender, EventArgs e)
        {
            GetInstance<MDefMdlVisRefWrapper>().NewSwitch();
        }

        private static void MenuClosing(object sender, ToolStripDropDownClosingEventArgs e)
        {
            _menu.Items[2].Enabled = _menu.Items[3].Enabled = _menu.Items[6].Enabled = true;
        }

        private static void MenuOpening(object sender, CancelEventArgs e)
        {
            MDefMdlVisRefWrapper w = GetInstance<MDefMdlVisRefWrapper>();
            _menu.Items[6].Enabled = w.Parent != null;
            _menu.Items[2].Enabled = w.PrevNode != null;
            _menu.Items[3].Enabled = w.NextNode != null;
        }

        #endregion

        public MDefMdlVisRefWrapper()
        {
            ContextMenuStrip = _menu;
        }

        public void NewSwitch()
        {
            foreach (MoveDefModelVisRefNode r in _resource.Parent.Children)
            {
                MoveDefBoneSwitchNode node = new MoveDefBoneSwitchNode {Name = "BoneSwitch" + r.Children.Count};
                r.AddChild(node);
            }
        }
    }

    [NodeWrapper(ResourceType.MDefMdlVisSwitch)]
    internal class MDefMdlVisSwitchWrapper : GenericWrapper
    {
        #region Menu

        private static ContextMenuStrip _menu;

        static MDefMdlVisSwitchWrapper()
        {
            _menu = new ContextMenuStrip();
            _menu.Items.Add(new ToolStripMenuItem("Add New Group", null, NewGroupAction, Keys.Control | Keys.A));
            _menu.Items.Add(new ToolStripSeparator());
            _menu.Items.Add(new ToolStripMenuItem("Move &Up", null, MoveUp2Action, Keys.Control | Keys.Up));
            _menu.Items.Add(new ToolStripMenuItem("Move D&own", null, MoveDown2Action, Keys.Control | Keys.Down));
            _menu.Items.Add(new ToolStripSeparator());
            _menu.Items.Add(new ToolStripMenuItem("&Delete", null, Delete2Action, Keys.Control | Keys.Delete));
            _menu.Opening += MenuOpening;
            _menu.Closing += MenuClosing;
        }

        protected static void NewGroupAction(object sender, EventArgs e)
        {
            GetInstance<MDefMdlVisSwitchWrapper>().NewGroup();
        }

        protected static void MoveUp2Action(object sender, EventArgs e)
        {
            GetInstance<MDefMdlVisSwitchWrapper>().MoveUp2();
        }

        protected static void MoveDown2Action(object sender, EventArgs e)
        {
            GetInstance<MDefMdlVisSwitchWrapper>().MoveDown2();
        }

        protected static void Delete2Action(object sender, EventArgs e)
        {
            GetInstance<MDefMdlVisSwitchWrapper>().Delete2();
        }

        private static void MenuClosing(object sender, ToolStripDropDownClosingEventArgs e)
        {
            _menu.Items[2].Enabled = _menu.Items[3].Enabled = _menu.Items[5].Enabled = true;
        }

        private static void MenuOpening(object sender, CancelEventArgs e)
        {
            MDefMdlVisSwitchWrapper w = GetInstance<MDefMdlVisSwitchWrapper>();
            _menu.Items[5].Enabled = w.Parent != null;
            _menu.Items[2].Enabled = w.PrevNode != null;
            _menu.Items[3].Enabled = w.NextNode != null;
        }

        #endregion

        public MDefMdlVisSwitchWrapper()
        {
            ContextMenuStrip = _menu;
        }

        public void NewGroup()
        {
            MoveDefModelVisGroupNode node = new MoveDefModelVisGroupNode
                {Name = "BoneGroup" + _resource.Children.Count};
            _resource.AddChild(node);
            BaseWrapper res = FindResource(node, false);
            res.EnsureVisible();
            res.TreeView.SelectedNode = res;
        }

        public void MoveUp2()
        {
            int i = _resource.Index;
            foreach (MoveDefModelVisRefNode r in _resource.Parent.Parent.Children)
            {
                if (i < r.Children.Count)
                {
                    r.Children[i].DoMoveUp();
                }
            }
        }

        public void MoveDown2()
        {
            int i = _resource.Index;
            foreach (MoveDefModelVisRefNode r in _resource.Parent.Parent.Children)
            {
                if (i < r.Children.Count)
                {
                    r.Children[i].DoMoveDown();
                }
            }
        }

        public void Delete2()
        {
            int i = _resource.Index;
            foreach (MoveDefModelVisRefNode r in _resource.Parent.Parent.Children)
            {
                if (i < r.Children.Count)
                {
                    r.Children[i].Remove();
                }
            }
        }
    }

    [NodeWrapper(ResourceType.MDefMdlVisGroup)]
    internal class MDefMdlVisGroupWrapper : GenericWrapper
    {
        #region Menu

        private static ContextMenuStrip _menu;

        static MDefMdlVisGroupWrapper()
        {
            _menu = new ContextMenuStrip();
            _menu.Items.Add(new ToolStripMenuItem("Add New Bone", null, NewBoneAction, Keys.Control | Keys.A));
            _menu.Items.Add(new ToolStripSeparator());
            _menu.Items.Add(new ToolStripMenuItem("Move &Up", null, MoveUpAction, Keys.Control | Keys.Up));
            _menu.Items.Add(new ToolStripMenuItem("Move D&own", null, MoveDownAction, Keys.Control | Keys.Down));
            _menu.Items.Add(new ToolStripMenuItem("Re&name", null, RenameAction, Keys.Control | Keys.N));
            _menu.Items.Add(new ToolStripSeparator());
            _menu.Items.Add(new ToolStripMenuItem("&Delete", null, DeleteAction, Keys.Control | Keys.Delete));
            _menu.Opening += MenuOpening;
            _menu.Closing += MenuClosing;
        }

        protected static void NewBoneAction(object sender, EventArgs e)
        {
            GetInstance<MDefMdlVisGroupWrapper>().NewBone();
        }

        private static void MenuClosing(object sender, ToolStripDropDownClosingEventArgs e)
        {
            _menu.Items[2].Enabled = _menu.Items[3].Enabled = _menu.Items[6].Enabled = true;
        }

        private static void MenuOpening(object sender, CancelEventArgs e)
        {
            MDefMdlVisGroupWrapper w = GetInstance<MDefMdlVisGroupWrapper>();
            _menu.Items[6].Enabled = w.Parent != null;
            _menu.Items[2].Enabled = w.PrevNode != null;
            _menu.Items[3].Enabled = w.NextNode != null;
        }

        #endregion

        public MDefMdlVisGroupWrapper()
        {
            ContextMenuStrip = _menu;
        }

        public void NewBone()
        {
            MoveDefBoneIndexNode node = new MoveDefBoneIndexNode();
            _resource.AddChild(node);
            BaseWrapper res = FindResource(node, false);
            res.EnsureVisible();
            res.TreeView.SelectedNode = res;
        }
    }

    [NodeWrapper(ResourceType.MDefSubroutineList)]
    internal class MDefSubroutineListWrapper : GenericWrapper
    {
        #region Menu

        private static ContextMenuStrip _menu;

        static MDefSubroutineListWrapper()
        {
            _menu = new ContextMenuStrip();
            _menu.Items.Add(new ToolStripMenuItem("Add Ne&w Subroutine", null, NewAction, Keys.Control | Keys.H));
            _menu.Items.Add(new ToolStripSeparator());
            _menu.Items.Add(new ToolStripMenuItem("&Export", null, ExportAction, Keys.Control | Keys.E));
            _menu.Opening += MenuOpening;
            _menu.Closing += MenuClosing;
        }

        protected static void NewAction(object sender, EventArgs e)
        {
            GetInstance<MDefSubroutineListWrapper>().NewActionGroup();
        }

        private static void MenuClosing(object sender, ToolStripDropDownClosingEventArgs e)
        {
        }

        private static void MenuOpening(object sender, CancelEventArgs e)
        {
        }

        #endregion

        private static bool shown;

        public MDefSubroutineListWrapper()
        {
            ContextMenuStrip = _menu;
        }

        public void NewActionGroup()
        {
            MoveDefActionNode node = new MoveDefActionNode("SubRoutine" + _resource.Children.Count, true, _resource);
            _resource.AddChild(node);
            _resource.Name = "[" + _resource.Children.Count + "] SubRoutines";
            if (!shown)
            {
                MessageBox.Show(
                    "Be sure to actually use this subroutine using an event with an offset or it will be removed after you save.\nThis message will not be shown again.");
                shown = true;
            }

            BaseWrapper res = FindResource(node, false);
            res.EnsureVisible();
            res.TreeView.SelectedNode = res;
        }
    }

    [NodeWrapper(ResourceType.MDefActionOverrideList)]
    internal class MDefActionOverrideListWrapper : GenericWrapper
    {
        #region Menu

        private static ContextMenuStrip _menu;

        static MDefActionOverrideListWrapper()
        {
            _menu = new ContextMenuStrip();
            _menu.Items.Add(new ToolStripMenuItem("Add Ne&w Action Override", null, NewAction, Keys.Control | Keys.H));
            _menu.Items.Add(new ToolStripSeparator());
            _menu.Items.Add(new ToolStripMenuItem("&Export", null, ExportAction, Keys.Control | Keys.E));
            _menu.Opening += MenuOpening;
            _menu.Closing += MenuClosing;
        }

        protected static void NewAction(object sender, EventArgs e)
        {
            GetInstance<MDefActionOverrideListWrapper>().NewActionGroup();
        }

        private static void MenuClosing(object sender, ToolStripDropDownClosingEventArgs e)
        {
        }

        private static void MenuOpening(object sender, CancelEventArgs e)
        {
        }

        #endregion

        public MDefActionOverrideListWrapper()
        {
            ContextMenuStrip = _menu;
        }

        public void NewActionGroup()
        {
            MoveDefActionOverrideEntryNode node = new MoveDefActionOverrideEntryNode {Name = "Action0 Override"};
            node.Children.Add(new MoveDefActionNode("Action0", true, node));
            _resource.AddChild(node);
            BaseWrapper res = FindResource(node, false);
            res.EnsureVisible();
            res.TreeView.SelectedNode = res;
        }
    }

    [NodeWrapper(ResourceType.MDefRefList)]
    internal class MDefRefListWrapper : GenericWrapper
    {
        private static bool shown;

        #region Menu

        private static ContextMenuStrip _menu;

        static MDefRefListWrapper()
        {
            _menu = new ContextMenuStrip();
            _menu.Items.Add(new ToolStripMenuItem("Add New Reference", null, NewRefAction, Keys.Control | Keys.A));
        }

        protected static void NewRefAction(object sender, EventArgs e)
        {
            GetInstance<MDefRefListWrapper>().NewRef();
        }

        #endregion

        public MDefRefListWrapper()
        {
            ContextMenuStrip = _menu;
        }

        public void NewRef()
        {
            MoveDefReferenceEntryNode node = new MoveDefReferenceEntryNode {Name = "NewRef"};
            _resource.AddChild(node);
            if (!shown)
            {
                MessageBox.Show(
                    "Be sure that the name of this reference has the same name as a section entry in Fighter.pac.\nAlso be sure to actually use this reference using an event with an offset or it will be removed after you save.\nThis message will not be shown again.");
                shown = true;
            }

            BaseWrapper res = FindResource(node, false);
            res.EnsureVisible();
            res.TreeView.SelectedNode = res;
        }
    }
}