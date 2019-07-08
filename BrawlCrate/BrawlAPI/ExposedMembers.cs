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
        /// <summary>
        ///     The root node of the opened file.
        ///
        ///     Returns null if there is no open file.
        /// </summary>
        public static ResourceNode RootNode =>
            MainForm.Instance.RootNode != null ? MainForm.Instance.RootNode.Resource : null;

        /// <summary>
        ///     The wrapper for the root node of the opened file.
        ///
        ///     Returns null if there is no open file.
        /// </summary>
        public static BaseWrapper RootNodeWrapper => MainForm.Instance.RootNode;

        /// <summary>
        ///     The currently selected node on the Main Form. Useful for context menu items.
        ///
        ///     Returns null if there is no selected file.
        /// </summary>
        public static ResourceNode SelectedNode => MainForm.Instance.resourceTree.SelectedNode != null
            ? ((BaseWrapper) MainForm.Instance.resourceTree.SelectedNode).Resource
            : null;

        /// <summary>
        ///     The wrapper for the currently selected node on the Main Form. Useful for context menu items.
        ///
        ///     Returns null if there is no selected file.
        /// </summary>
        public static BaseWrapper SelectedNodeWrapper => (BaseWrapper) MainForm.Instance.resourceTree.SelectedNode;

        /// <summary>
        ///     Returns a list of all nodes in the open file.
        ///
        ///     Returns null if there is no open file.
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
        ///     Returns a list of all node wrappers in the open file.
        ///
        ///     Note that this function is generally slow due to the implementation of ResourceTrees; for general usage, NodeList works best.
        ///
        ///     Returns null if there is no open file.
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
            MessageBox.Show(msg, title);
        }

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
        public static bool? ShowYesNoPrompt(string msg, string title)
        {
            return MessageBox.Show(msg, title, MessageBoxButtons.YesNo) == DialogResult.Yes;
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
        public static bool? ShowOKCancelPrompt(string msg, string title)
        {
            return MessageBox.Show(msg, title, MessageBoxButtons.OKCancel) == DialogResult.OK;
        }

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
        ///     Opens an open file dialog, prompting the user to select a file.
        ///
        ///     Returns the path of the file that the user chose, or an empty string if the dialog was cancelled.
        /// </summary>
        public static string OpenFileDialog()
        {
            using (OpenFileDialog dlg = new OpenFileDialog())
            {
                return dlg.ShowDialog() == DialogResult.OK ? dlg.FileName : string.Empty;
            }
        }

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
        ///     Opens a save file dialog, prompting the user to save a file.
        ///
        ///     *DOES NOT ACTUALLY SAVE ANYTHING BY ITSELF!*
        ///     Scripting must be done if you want to actually save anything.
        ///
        ///     Returns the path of the file that the user chose, or an empty string if the dialog was cancelled.
        /// </summary>
        public static string SaveFileDialog()
        {
            using (SaveFileDialog dlg = new SaveFileDialog())
            {
                return dlg.ShowDialog() == DialogResult.OK ? dlg.FileName : string.Empty;
            }
        }

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
    }
}