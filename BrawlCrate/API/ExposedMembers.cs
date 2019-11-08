using BrawlCrate.NodeWrappers;
using BrawlCrate.UI;
using BrawlLib.Internal;
using BrawlLib.SSBB.ResourceNodes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

#if !MONO
using BrawlLib.Internal.Windows.Forms.Ookii.Dialogs;
#endif

// ReSharper disable UnusedMember.Global

namespace BrawlCrate.API
{
    public static partial class BrawlAPI
    {
        #region Nodes

        /// <summary>
        ///     The root node of the opened file.
        ///
        ///     Returns null if there is no open file.
        /// </summary>
        public static ResourceNode RootNode => MainForm.Instance?.RootNode?.Resource;

        /// <summary>
        ///     The currently selected node on the Main Form. Useful for context menu items.
        ///
        ///     Returns null if there is no selected node.
        /// </summary>
        public static ResourceNode SelectedNode => MainForm.Instance?.resourceTree.SelectedNode != null
            ? ((BaseWrapper) MainForm.Instance?.resourceTree.SelectedNode).Resource
            : null;

        /// <summary>
        ///     The currently selected nodes on the Main Form, if multiple are selected
        ///
        ///     Returns an empty list if there are no selected nodes.
        /// </summary>
        public static List<ResourceNode> SelectedNodes
        {
            get
            {
                List<ResourceNode> nodes = new List<ResourceNode>();
                if (MainForm.Instance?.resourceTree.SelectedNodes != null)
                {
                    foreach (BaseWrapper b in MainForm.Instance?.resourceTree.SelectedNodes)
                    {
                        nodes.Add(b.Resource);
                    }
                }

                return nodes;
            }
        }

        /// <summary>
        ///     Returns a full list of all nodes in the open file.
        ///
        ///     Returns an empty list if there is no open file.
        /// </summary>
        public static List<ResourceNode> NodeList
        {
            get
            {
                List<ResourceNode> nodes = new List<ResourceNode>();

                if (RootNode != null)
                {
                    nodes.AddRange(ResourceNode.FindAllSubNodes(RootNode));
                }

                return nodes;
            }
        }

        /// <summary>
        ///     Returns a full list of all nodes of a given type in the open file.
        ///
        ///     Returns an empty list if there is no open file
        /// </summary>
        public static List<T> NodeListOfType<T>() where T : ResourceNode
        {
            List<T> nodes = new List<T>();

            if (RootNode != null)
            {
                nodes = ResourceNode.FindAllSubNodes(RootNode).Where(n => n.GetType().IsInstanceOfType(typeof(T)))
                                    .Cast<T>().ToList();
            }

            return nodes;
        }

        #endregion

        #region Wrappers

        /// <summary>
        ///     The wrapper for the root node of the opened file.
        ///
        ///     Returns null if there is no open file.
        /// </summary>
        public static BaseWrapper RootNodeWrapper => MainForm.Instance?.RootNode;

        /// <summary>
        ///     The wrapper for the currently selected node on the Main Form. Useful for context menu items.
        ///
        ///     Returns null if there is no selected node.
        /// </summary>
        public static BaseWrapper SelectedNodeWrapper => (BaseWrapper) MainForm.Instance?.resourceTree.SelectedNode;

        /// <summary>
        ///     The wrappers for the currently selected nodes on the Main Form.
        ///
        ///     Returns null if there are no selected nodes.
        /// </summary>
        public static List<BaseWrapper> SelectedNodeWrappers
        {
            get
            {
                List<BaseWrapper> wrappers = new List<BaseWrapper>();
                if (MainForm.Instance?.resourceTree.SelectedNodes != null)
                {
                    foreach (TreeNode treeNode in MainForm.Instance?.resourceTree.SelectedNodes)
                    {
                        if (treeNode is BaseWrapper b)
                        {
                            wrappers.Add(b);
                        }
                    }
                }

                return wrappers;
            }
        }

        /// <summary>
        ///     Returns a full list of all node wrappers in the open file.
        ///
        ///     Note that this function is generally slow due to the implementation of ResourceTrees; for general usage, NodeList
        ///     works best.
        ///
        ///     Returns an empty list if there is no open file.
        /// </summary>
        public static List<BaseWrapper> NodeWrapperList
        {
            get
            {
                List<BaseWrapper> wrappers = new List<BaseWrapper>();

                if (RootNodeWrapper != null)
                {
                    MainForm.Instance?.resourceTree.Hide();

                    TreeNodeCollection treeNodes = RootNodeWrapper.TreeView.Nodes;
                    foreach (TreeNode n in treeNodes)
                    {
                        if (n is BaseWrapper b)
                        {
                            wrappers.AddRange(BaseWrapper.GetAllNodes(b));
                        }
                    }

                    MainForm.Instance?.resourceTree.Show();
                }

                return wrappers;
            }
        }

        /// <summary>
        ///     Returns a full list of all node wrappers of a given type in the open file.
        ///
        ///     Returns an empty list if there is no open file
        /// </summary>
        public static List<T> NodeWrapperListOfType<T>() where T : BaseWrapper
        {
            List<T> nodes = new List<T>();

            if (RootNodeWrapper != null)
            {
                nodes = NodeWrapperList.Where(n => n.GetType().IsInstanceOfType(typeof(T))).Cast<T>().ToList();
            }

            return nodes;
        }

        #endregion

        #region Dialog Boxes

        #region No Input

        /// <summary>
        ///     Shows a MessageBox with the given message and title, and the default "OK" option.
        ///
        ///     Doesn't return a value, and is just used to show the user messages.
        /// </summary>
        /// <param name="msg">
        ///     The message that will appear in the body of the MessageBox.
        /// </param>
        /// <param name="title">
        ///     The title text that will show in the TitleBar of the MessageBox.
        /// </param>
        public static void ShowMessage(string msg, string title)
        {
            MessageBox.Show(MainForm.Instance, msg, title);
        }

        /// <summary>
        ///     Shows a MessageBox with the given message and title, the default "OK" option, and a Warning symbol.
        ///
        ///     Doesn't return a value, and is just used to show the user messages.
        /// </summary>
        /// <param name="msg">
        ///     The message that will appear in the body of the MessageBox.
        /// </param>
        /// <param name="title">
        ///     The title text that will show in the TitleBar of the MessageBox.
        /// </param>
        public static void ShowWarning(string msg, string title)
        {
            MessageBox.Show(MainForm.Instance, msg, title, MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        /// <summary>
        ///     Shows a MessageBox with the given message and title, the default "OK" option, and an Error symbol.
        ///
        ///     Doesn't return a value, and is just used to show the user messages.
        /// </summary>
        /// <param name="msg">
        ///     The message that will appear in the body of the MessageBox.
        /// </param>
        /// <param name="title">
        ///     The title text that will show in the TitleBar of the MessageBox.
        /// </param>
        public static void ShowError(string msg, string title)
        {
            MessageBox.Show(MainForm.Instance, msg, title, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        #endregion

        #region User Input

        /// <summary>
        ///     Shows a MessageBox with the given message and title, with options for "Yes" or "No".
        ///
        ///     Returns true if "Yes" is clicked, and false otherwise.
        /// </summary>
        /// <param name="msg">
        ///     The message that will appear in the body of the MessageBox.
        /// </param>
        /// <param name="title">
        ///     The title text that will show in the TitleBar of the MessageBox.
        /// </param>
        public static bool ShowYesNoPrompt(string msg, string title)
        {
            return MessageBox.Show(MainForm.Instance, msg, title, MessageBoxButtons.YesNo) == DialogResult.Yes;
        }

        /// <summary>
        ///     Shows a MessageBox with the given message and title, "Yes/No" options, and a Warning symbol.
        ///
        ///     Returns true if "Yes" is clicked, and false otherwise.
        /// </summary>
        /// <param name="msg">
        ///     The message that will appear in the body of the MessageBox.
        /// </param>
        /// <param name="title">
        ///     The title text that will show in the TitleBar of the MessageBox.
        /// </param>
        public static bool ShowYesNoWarning(string msg, string title)
        {
            return MessageBox.Show(MainForm.Instance, msg, title, MessageBoxButtons.YesNo, MessageBoxIcon.Warning)
                   == DialogResult.Yes;
        }

        /// <summary>
        ///     Shows a MessageBox with the given message and title, "Yes/No" options, and an Error symbol.
        ///
        ///     Returns true if "Yes" is clicked, and false otherwise.
        /// </summary>
        /// <param name="msg">
        ///     The message that will appear in the body of the MessageBox.
        /// </param>
        /// <param name="title">
        ///     The title text that will show in the TitleBar of the MessageBox.
        /// </param>
        public static bool ShowYesNoError(string msg, string title)
        {
            return MessageBox.Show(MainForm.Instance, msg, title, MessageBoxButtons.YesNo, MessageBoxIcon.Error)
                   == DialogResult.Yes;
        }

        /// <summary>
        ///     Shows a MessageBox with the given message and title, with options for "OK" or "Cancel".
        ///
        ///     Returns true if "OK" is clicked, and false otherwise.
        /// </summary>
        /// <param name="msg">
        ///     The message that will appear in the body of the MessageBox.
        /// </param>
        /// <param name="title">
        ///     The title text that will show in the TitleBar of the MessageBox.
        /// </param>
        public static bool ShowOKCancelPrompt(string msg, string title)
        {
            return MessageBox.Show(MainForm.Instance, msg, title, MessageBoxButtons.OKCancel) == DialogResult.OK;
        }

        /// <summary>
        ///     Shows a MessageBox with the given message and title, "OK/Cancel" options, and a Warning symbol.
        ///
        ///     Returns true if "OK" is clicked, and false otherwise.
        /// </summary>
        /// <param name="msg">
        ///     The message that will appear in the body of the MessageBox.
        /// </param>
        /// <param name="title">
        ///     The title text that will show in the TitleBar of the MessageBox.
        /// </param>
        public static bool ShowOKCancelWarning(string msg, string title)
        {
            return MessageBox.Show(MainForm.Instance, msg, title, MessageBoxButtons.OKCancel, MessageBoxIcon.Warning)
                   == DialogResult.OK;
        }

        /// <summary>
        ///     Shows a MessageBox with the given message and title, "OK/Cancel" options, and an Error symbol.
        ///
        ///     Returns true if "OK" is clicked, and false otherwise.
        /// </summary>
        /// <param name="msg">
        ///     The message that will appear in the body of the MessageBox.
        /// </param>
        /// <param name="title">
        ///     The title text that will show in the TitleBar of the MessageBox.
        /// </param>
        public static bool ShowOKCancelError(string msg, string title)
        {
            return MessageBox.Show(MainForm.Instance, msg, title, MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                   == DialogResult.OK;
        }

        #endregion

        #region Save/Open Dialogs

        #region Open

        #region Single File

        /// <summary>
        ///     Opens an open file dialog, prompting the user to select a file.
        ///
        ///     Returns the path of the file that the user chose, or an empty string if the dialog was cancelled.
        /// </summary>
        public static string OpenFileDialog()
        {
            using (OpenFileDialog dlg = new OpenFileDialog())
            {
                dlg.Title = "Open File";
                dlg.Multiselect = false;
                return dlg.ShowDialog() == DialogResult.OK ? dlg.FileName : string.Empty;
            }
        }

        /// <summary>
        ///     Opens an open file dialog, prompting the user to select a file.
        ///
        ///     Returns the path of the file that the user chose, or an empty string if the dialog was cancelled.
        /// </summary>
        /// <param name="title">
        ///     The title text that will show in the TitleBar of the form.
        /// </param>
        public static string OpenFileDialog(string title)
        {
            using (OpenFileDialog dlg = new OpenFileDialog())
            {
                dlg.Multiselect = false;
                dlg.Title = title;
                return dlg.ShowDialog() == DialogResult.OK ? dlg.FileName : string.Empty;
            }
        }

        /// <summary>
        ///     Opens an open file dialog, prompting the user to select a file.
        ///
        ///     Returns the path of the file that the user chose, or an empty string if the dialog was cancelled.
        /// </summary>
        /// <param name="title">
        ///     The title text that will show in the TitleBar of the form.
        /// </param>
        /// <param name="filter">
        ///     The file filter to use for the open file dialog. Use the formatting found here:
        ///     https://docs.microsoft.com/en-us/dotnet/api/system.windows.forms.filedialog.filter
        ///
        ///     You can also reference the pre-programmed filters from FileFilters.cs
        /// </param>
        public static string OpenFileDialog(string title, string filter)
        {
            using (OpenFileDialog dlg = new OpenFileDialog())
            {
                dlg.Multiselect = false;
                dlg.Title = title;
                dlg.Filter = filter;
                return dlg.ShowDialog() == DialogResult.OK ? dlg.FileName : string.Empty;
            }
        }

        #endregion

        #region Multiple Files

        /// <summary>
        ///     Opens an open file dialog, prompting the user to select multiple files
        ///
        ///     Returns the paths of the files that the user chose, or null if the dialog was cancelled.
        /// </summary>
        public static string[] OpenMultiFileDialog()
        {
            using (OpenFileDialog dlg = new OpenFileDialog())
            {
                dlg.Title = "Open Files";
                dlg.Multiselect = true;
                return dlg.ShowDialog() == DialogResult.OK ? dlg.FileNames : null;
            }
        }

        /// <summary>
        ///     Opens an open file dialog, prompting the user to select multiple files
        ///
        ///     Returns the paths of the files that the user chose, or null if the dialog was cancelled.
        /// </summary>
        /// <param name="title">
        ///     The title text that will show in the TitleBar of the form.
        /// </param>
        public static string[] OpenMultiFileDialog(string title)
        {
            using (OpenFileDialog dlg = new OpenFileDialog())
            {
                dlg.Multiselect = true;
                dlg.Title = title;
                return dlg.ShowDialog() == DialogResult.OK ? dlg.FileNames : null;
            }
        }

        /// <summary>
        ///     Opens an open file dialog, prompting the user to select multiple files
        ///
        ///     Returns the paths of the files that the user chose, or null if the dialog was cancelled.
        /// </summary>
        /// <param name="title">
        ///     The title text that will show in the TitleBar of the form.
        /// </param>
        /// <param name="filter">
        ///     The file filter to use for the open file dialog. Use the formatting found here:
        ///     https://docs.microsoft.com/en-us/dotnet/api/system.windows.forms.filedialog.filter
        ///
        ///     You can also reference the pre-programmed filters from FileFilters.cs
        /// </param>
        public static string[] OpenMultiFileDialog(string title, string filter)
        {
            using (OpenFileDialog dlg = new OpenFileDialog())
            {
                dlg.Multiselect = true;
                dlg.Title = title;
                dlg.Filter = filter;
                return dlg.ShowDialog() == DialogResult.OK ? dlg.FileNames : null;
            }
        }

        #endregion

        #region Folders

        /// <summary>
        ///     Opens an Open Folder Dialog, prompting the user to select a folder.
        ///
        ///     Returns the path of the folder that the user chose, or an empty string if the dialog was cancelled.
        /// </summary>
        public static string OpenFolderDialog()
        {
#if !MONO
            using (VistaFolderBrowserDialog dlg = new VistaFolderBrowserDialog { UseDescriptionForTitle = true })
#else
            using (FolderBrowserDialog dlg = new FolderBrowserDialog())
#endif
            {
                dlg.Description = "Open Folder";
                return dlg.ShowDialog() == DialogResult.OK ? dlg.SelectedPath : string.Empty;
            }
        }

        /// <summary>
        ///     Opens an Open Folder Dialog, prompting the user to select a folder.
        ///
        ///     Returns the path of the folder that the user chose, or an empty string if the dialog was cancelled.
        /// </summary>
        /// <param name="description">
        ///     The description of the folder dialog.
        ///
        ///     For Windows-based devices, this will appear as the title of the window.
        ///     For others, this will appear as a description box.
        /// </param>
        public static string OpenFolderDialog(string description)
        {
#if !MONO
            using (VistaFolderBrowserDialog dlg = new VistaFolderBrowserDialog { UseDescriptionForTitle = true })
#else
            using (FolderBrowserDialog dlg = new FolderBrowserDialog())
#endif
            {
                dlg.Description = description;
                return dlg.ShowDialog() == DialogResult.OK ? dlg.SelectedPath : string.Empty;
            }
        }

        #endregion

        #endregion

        #region Save

        /// <summary>
        ///     Opens a save file dialog, prompting the user to save a file.
        ///
        ///     *DOES NOT ACTUALLY SAVE ANYTHING BY ITSELF!*
        ///     Additional scripting must be done if you want to actually save anything.
        ///
        ///     Returns the save path that the user chose, or an empty string if the dialog was cancelled.
        /// </summary>
        public static string SaveFileDialog()
        {
            using (SaveFileDialog dlg = new SaveFileDialog())
            {
                return dlg.ShowDialog() == DialogResult.OK ? dlg.FileName : string.Empty;
            }
        }

        /// <summary>
        ///     Opens a save file dialog, prompting the user to save a file.
        ///
        ///     *DOES NOT ACTUALLY SAVE ANYTHING BY ITSELF!*
        ///     Additional scripting must be done if you want to actually save anything.
        ///
        ///     Returns the save path that the user chose, or an empty string if the dialog was cancelled.
        /// </summary>
        /// <param name="title">
        ///     The title text that will show in the TitleBar of the form.
        /// </param>
        public static string SaveFileDialog(string title)
        {
            using (SaveFileDialog dlg = new SaveFileDialog())
            {
                dlg.Title = title;
                return dlg.ShowDialog() == DialogResult.OK ? dlg.FileName : string.Empty;
            }
        }

        /// <summary>
        ///     Opens a save file dialog, prompting the user to save a file.
        ///
        ///     *DOES NOT ACTUALLY SAVE ANYTHING BY ITSELF!*
        ///     Additional scripting must be done if you want to actually save anything.
        ///
        ///     Returns the save path that the user chose, or an empty string if the dialog was cancelled.
        /// </summary>
        /// <param name="title">
        ///     The title text that will show in the TitleBar of the form.
        /// </param>
        /// <param name="filter">
        ///     The file filter to use for the open file dialog. Use the formatting found here:
        ///     https://docs.microsoft.com/en-us/dotnet/api/system.windows.forms.filedialog.filter
        ///
        ///     You can also reference the pre-programmed filters from FileFilters.cs
        /// </param>
        public static string SaveFileDialog(string title, string filter)
        {
            using (SaveFileDialog dlg = new SaveFileDialog())
            {
                dlg.Title = title;
                dlg.Filter = filter;
                return dlg.ShowDialog() == DialogResult.OK ? dlg.FileName : string.Empty;
            }
        }

        #endregion

        #endregion

        #endregion

        #region Context Menus

        /// <summary>
        ///     To be called by API, adds context menu items to a wrapper.
        ///
        ///     This is kept for compatibility purposes, the second definition for this function offers far more extensibility.
        /// </summary>
        /// <param name="wrapper">
        ///     The wrapper which new items will be added to.
        /// </param>
        /// <param name="items">
        ///     One or more ToolStrip menu items that will be added to the context menu.
        ///     These should be defined as much as possible in the script itself.
        /// </param>
        public static void AddContextMenuItem(Type wrapper, params ToolStripMenuItem[] items)
        {
            if (ContextMenuHooks.ContainsKey(wrapper))
            {
                if (items.Length == 1 && items[0].HasDropDownItems)
                {
                    // Combine same-named submenus
                    for (int i = 0; i < ContextMenuHooks[wrapper].Length; i++)
                    {
                        ToolStripMenuItem item = ContextMenuHooks[wrapper][i];
                        if (!item.HasDropDownItems || item.Text != items[0].Text)
                        {
                            continue;
                        }

                        if (string.IsNullOrEmpty(item.ToolTipText))
                        {
                            item.ToolTipText = items[0].ToolTipText;
                        }

                        for (int j = 0; j < items[0].DropDownItems.Count; j++)
                        {
                            ToolStripItem item2 = items[0].DropDownItems[j];
                            item.DropDownItems.Add(item2);
                        }

                        return;
                    }
                }

                ContextMenuHooks[wrapper] = ContextMenuHooks[wrapper].Append(items);
            }
            else
            {
                ContextMenuHooks.Add(wrapper, items);
            }
        }

        /// <summary>
        ///     To be called by API, adds context menu items to a wrapper with additional options.
        /// </summary>
        /// <param name="wrapper">
        ///     The wrapper which new items will be added to.
        /// </param>
        /// <param name="subMenuName">
        ///     (Optional) If not null or empty, a submenu which nodes will be defined under.
        /// </param>
        /// <param name="description">
        ///     (Optional) If not null or empty, a string that will appear on mouseover.
        ///     Will be added to the menu if the items are in a submenu, or added to all items otherwise.
        /// </param>
        /// <param name="conditional">
        ///     (Optional) If not null, a function that will be run every time the dropdown is activated.
        ///     Most useful to change an item's enabled state.
        /// </param>
        /// <param name="items">
        ///     One or more ToolStrip menu items that will be added to the context menu.
        ///     These should be defined as much as possible in the script itself.
        /// </param>
        public static void AddContextMenuItem(Type wrapper, string subMenuName, string description,
                                              EventHandler conditional, params ToolStripMenuItem[] items)
        {
            if (conditional != null)
            {
                foreach (ToolStripMenuItem item in items)
                {
                    item.EnabledChanged += conditional;
                }
            }

            if (!string.IsNullOrEmpty(subMenuName))
            {
                ToolStripMenuItem t = new ToolStripMenuItem(subMenuName);
                foreach (ToolStripMenuItem item in items)
                {
                    t.DropDownItems.Add(item);
                }

                if (!string.IsNullOrEmpty(description))
                {
                    t.ToolTipText = description;
                }

                AddContextMenuItem(wrapper, t);
                return;
            }

            if (!string.IsNullOrEmpty(description))
            {
                foreach (ToolStripMenuItem item in items)
                {
                    item.ToolTipText += description;
                }
            }

            AddContextMenuItem(wrapper, items);
        }

        /// <summary>
        ///     To be called by API, adds context menu items to a wrapper's multi-selection menu.
        ///
        ///     The second definition for this function offers far more extensibility.
        /// </summary>
        /// <param name="wrapper">
        ///     The wrapper which new items will be added to.
        /// </param>
        /// <param name="items">
        ///     One or more ToolStrip menu items that will be added to the context menu.
        ///     These should be defined as much as possible in the script itself.
        /// </param>
        public static void AddMultiSelectContextMenuItem(Type wrapper, params ToolStripMenuItem[] items)
        {
            if (MultiSelectContextMenuHooks.ContainsKey(wrapper))
            {
                if (items.Length == 1 && items[0].HasDropDownItems)
                {
                    // Combine same-named submenus
                    for (int i = 0; i < MultiSelectContextMenuHooks[wrapper].Length; i++)
                    {
                        ToolStripMenuItem item = MultiSelectContextMenuHooks[wrapper][i];
                        if (!item.HasDropDownItems || item.Text != items[0].Text)
                        {
                            continue;
                        }

                        if (string.IsNullOrEmpty(item.ToolTipText))
                        {
                            item.ToolTipText = items[0].ToolTipText;
                        }

                        for (int j = 0; j < items[0].DropDownItems.Count; j++)
                        {
                            ToolStripItem item2 = items[0].DropDownItems[j];
                            item.DropDownItems.Add(item2);
                        }

                        return;
                    }
                }

                MultiSelectContextMenuHooks[wrapper] = MultiSelectContextMenuHooks[wrapper].Append(items);
            }
            else
            {
                MultiSelectContextMenuHooks.Add(wrapper, items);
            }
        }

        /// <summary>
        ///     To be called by API, adds context menu items to a wrapper's multi-selection menu with additional options.
        /// </summary>
        /// <param name="wrapper">
        ///     The wrapper which new items will be added to.
        /// </param>
        /// <param name="subMenuName">
        ///     (Optional) If not null or empty, a submenu which nodes will be defined under.
        /// </param>
        /// <param name="description">
        ///     (Optional) If not null or empty, a string that will appear on mouseover.
        ///     Will be added to the menu if the items are in a submenu, or added to all items otherwise.
        /// </param>
        /// <param name="conditional">
        ///     (Optional) If not null, a function that will be run every time the dropdown is activated.
        ///     Most useful to change an item's enabled state.
        /// </param>
        /// <param name="items">
        ///     One or more ToolStrip menu items that will be added to the context menu.
        ///     These should be defined as much as possible in the script itself.
        /// </param>
        public static void AddMultiSelectContextMenuItem(Type wrapper, string subMenuName, string description,
                                                         EventHandler conditional, params ToolStripMenuItem[] items)
        {
            if (conditional != null)
            {
                foreach (ToolStripMenuItem item in items)
                {
                    item.EnabledChanged += conditional;
                }
            }

            if (!string.IsNullOrEmpty(subMenuName))
            {
                ToolStripMenuItem t = new ToolStripMenuItem(subMenuName);
                foreach (ToolStripMenuItem item in items)
                {
                    t.DropDownItems.Add(item);
                }

                if (!string.IsNullOrEmpty(description))
                {
                    t.ToolTipText = description;
                }

                AddMultiSelectContextMenuItem(wrapper, t);
                return;
            }

            if (!string.IsNullOrEmpty(description))
            {
                foreach (ToolStripMenuItem item in items)
                {
                    item.ToolTipText += description;
                }
            }

            AddMultiSelectContextMenuItem(wrapper, items);
        }

        #endregion

        #region Program

        /// <summary>
        ///     The folder in which the BrawlCrate installation is located.
        /// </summary>
        public static string AppPath => Path.GetFullPath(Program.AppPath);

        /// <summary>
        ///     The folder in which the API folders are located.
        /// </summary>
        public static string APIPath => Path.GetFullPath(Program.ApiPath);

        /// <summary>
        ///     The folder in which plugins are located.
        /// </summary>
        public static string PluginPath => Path.GetFullPath(Program.ApiPluginPath);

        /// <summary>
        ///     The folder in which loaders are located.
        /// </summary>
        public static string LoaderPath => Path.GetFullPath(Program.ApiLoaderPath);

        /// <summary>
        ///     Attempts to open the file using a given path.
        ///
        ///     Returns true if the file is successfully loaded, and false otherwise.
        /// </summary>
        /// <param name="path">
        ///     The path of the file that is to be opened.
        /// </param>
        public static bool OpenFile(string path)
        {
            return Program.Open(path);
        }

        /// <summary>
        ///     Attempts to open the file using a given path.
        ///     Any error messages (including "file not found") are not shown to the user.
        ///
        ///     Returns true if the file is successfully loaded, and false otherwise.
        /// </summary>
        /// <param name="path">
        ///     The path of the file that is to be opened.
        /// </param>
        public static bool OpenFileNoErrors(string path)
        {
            return Program.Open(path, false);
        }

        /// <summary>
        ///     Attempts to open the file as a template using a given path.
        ///
        ///     Templates do not set the save path, meaning that the user will be prompted for "Save As" on first save.
        ///
        ///     Returns true if the file is successfully loaded, and false otherwise.
        /// </summary>
        /// <param name="path">
        ///     The path of the template file that is to be opened.
        /// </param>
        public static bool OpenTemplate(string path)
        {
            return Program.OpenTemplate(path);
        }

        /// <summary>
        ///     Attempts to open the file using a given path.
        ///     Any error messages (including "file not found") are not shown to the user.
        ///
        ///     Templates do not set the save path, meaning that the user will be prompted for "Save As" on first save.
        ///
        ///     Returns true if the file is successfully loaded, and false otherwise.
        /// </summary>
        /// <param name="path">
        ///     The path of the template file that is to be opened.
        /// </param>
        public static bool OpenTemplateNoErrors(string path)
        {
            return Program.OpenTemplate(path, false);
        }

        /// <summary>
        ///     Attempts to save the opened file.
        ///
        ///     Returns true if the file is successfully saved, and false otherwise.
        /// </summary>
        public static bool SaveFile()
        {
            return Program.Save();
        }

        /// <summary>
        ///     Prompts the user to save the open file to a directory.
        ///
        ///     Returns true if the file is successfully saved, and false otherwise.
        /// </summary>
        public static bool SaveFileAs()
        {
            return Program.SaveAs();
        }

        /// <summary>
        ///     Attempts to save the opened file to a given path.
        ///
        ///     Returns true if the file is successfully saved, and false otherwise.
        /// </summary>
        /// <param name="path">
        ///     The path to which the file should be saved
        /// </param>
        public static bool SaveFileAs(string path)
        {
            try
            {
                MainForm.Instance?.RootNode.Resource.Export(path);
                Program._rootPath = path;
                MainForm.Instance?.UpdateName();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }

        /// <summary>
        ///     Closes the open file, leaving the editor on the idle screen.
        ///
        ///     If the open file has unsaved changes, prompts the user to save them.
        ///
        ///     Returns true if the file is successfully closed, and false otherwise.
        /// </summary>
        public static bool CloseFile()
        {
            return Program.Close();
        }

        /// <summary>
        ///     Closes the open file, leaving the editor on the idle screen.
        ///
        ///     Will close regardless of unsaved changes.
        ///
        ///     Returns true if the file is successfully closed, and false otherwise.
        /// </summary>
        public static bool ForceCloseFile()
        {
            return Program.Close(true);
        }

        #endregion

        #region Parsing

        /// <summary>
        ///     Adds parsing for a specific user-defined file type.
        ///
        ///     In bboxapi, this was originally "AddLoader"
        /// </summary>
        /// <param name="resourceParser">
        ///     The PluginResourceParser to be added.
        /// </param>
        public static void AddResourceParser(PluginResourceParser resourceParser)
        {
            ResourceParsers.Add(resourceParser);
        }

        /// <summary>
        ///     Adds a wrapper for a specific user-defined file type.
        ///
        ///     This variation requires reference to a pre-existing ResourceType.
        ///     Will overwrite pre-existing ResourceType-based wrappers.
        /// </summary>
        /// <param name="resourceType">
        ///     The resource file type to attach the wrapper to.
        /// </param>
        /// <param name="wrapper">
        ///     The wrapper to be used.
        /// </param>
        public static void AddWrapper(ResourceType resourceType, PluginWrapper wrapper)
        {
            NodeWrapperAttribute.AddWrapper(resourceType, wrapper);
        }

        /// <summary>
        ///     Adds a wrapper for a specific user-defined node type.
        ///
        ///     This variation bases wrappers off of the type of the resource node, offering more flexibility for new types.
        ///     Will overwrite pre-existing Type-based wrappers. Type-based wrappers take priority over ResourceType-based wrappers.
        /// </summary>
        /// <typeparam name="TypeNode">
        ///     The ResourceNode type to attach the wrapper to.
        /// </typeparam>
        /// <param name="wrapper">
        ///     The wrapper to be used.
        /// </param>
        public static void AddWrapper<TypeNode>(PluginWrapper wrapper) where TypeNode : ResourceNode
        {
            NodeWrapperAttribute.AddWrapper<TypeNode>(wrapper);
        }

        #endregion

        #region Debugging

        /// <summary>
        ///     Writes a message to the the console and debug log (if debugging) and the Trace log (if enabled).
        ///
        ///     To be used for debugging purposes only. This function call will do nothing in release builds.
        ///     For compatibility purposes, this function will still be callable in release builds.
        /// </summary>
        /// <param name="msg">
        ///     The string to be written to the console.
        /// </param>
        public static void WriteToConsole(string msg)
        {
#if DEBUG
            Console.WriteLine(msg);
            System.Diagnostics.Debug.WriteLine(msg);
#endif
#if TRACE
            System.Diagnostics.Trace.WriteLine(msg);
#endif
        }

        #endregion
    }
}