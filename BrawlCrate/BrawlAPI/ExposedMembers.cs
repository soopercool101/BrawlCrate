using BrawlCrate.NodeWrappers;
using BrawlLib.SSBB.ResourceNodes;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

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
        public static ResourceNode RootNode =>
            MainForm.Instance.RootNode != null ? MainForm.Instance.RootNode.Resource : null;

        /// <summary>
        ///     The currently selected node on the Main Form. Useful for context menu items.
        ///
        ///     Returns null if there is no selected node.
        /// </summary>
        public static ResourceNode SelectedNode => MainForm.Instance.resourceTree.SelectedNode != null
            ? ((BaseWrapper) MainForm.Instance.resourceTree.SelectedNode).Resource
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
                if (MainForm.Instance.resourceTree.SelectedNodes != null)
                {
                    foreach (BaseWrapper b in MainForm.Instance.resourceTree.SelectedNodes)
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

        #endregion

        #region Wrappers

        /// <summary>
        ///     The wrapper for the root node of the opened file.
        ///
        ///     Returns null if there is no open file.
        /// </summary>
        public static BaseWrapper RootNodeWrapper => MainForm.Instance.RootNode;

        /// <summary>
        ///     The wrapper for the currently selected node on the Main Form. Useful for context menu items.
        ///
        ///     Returns null if there is no selected node.
        /// </summary>
        public static BaseWrapper SelectedNodeWrapper => (BaseWrapper) MainForm.Instance.resourceTree.SelectedNode;

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
                if (MainForm.Instance.resourceTree.SelectedNodes != null)
                {
                    foreach (TreeNode treeNode in MainForm.Instance.resourceTree.SelectedNodes)
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
                    MainForm.Instance.resourceTree.Hide();

                    TreeNodeCollection treeNodes = RootNodeWrapper.TreeView.Nodes;
                    foreach (TreeNode n in treeNodes)
                    {
                        if (n is BaseWrapper b)
                        {
                            wrappers.AddRange(BaseWrapper.GetAllNodes(b));
                        }
                    }

                    MainForm.Instance.resourceTree.Show();
                }

                return wrappers;
            }
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
            using (FolderBrowserDialog dlg = new FolderBrowserDialog())
            {
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
        /// </param>
        public static string OpenFolderDialog(string description)
        {
            using (FolderBrowserDialog dlg = new FolderBrowserDialog())
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

        #endregion

        #region Other

        /// <summary>
        ///     Adds an additional loader.
        ///
        ///     Not very necessary with the current loader implementation, kept for compatibility purposes with BBoxAPI.
        /// </summary>
        /// <param name="loader">
        ///     The message that will appear in the body of the MessageBox.
        /// </param>
        public static void AddLoader(PluginLoader loader)
        {
            Loaders.Add(loader);
        }

        #endregion
    }
}