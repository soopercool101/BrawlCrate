using System;
using System.Windows.Forms;
using BrawlLib.SSBB.ResourceNodes;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

namespace BrawlBox.NodeWrappers
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
    sealed class NodeWrapperAttribute : Attribute
    {
        ResourceType _type;
        public NodeWrapperAttribute(ResourceType type) { _type = type; }
        public ResourceType WrappedType { get { return _type; } }

        private static Dictionary<ResourceType, Type> _wrappers;
        public static Dictionary<ResourceType, Type> Wrappers
        {
            get
            {
                if (_wrappers == null)
                {
                    _wrappers = new Dictionary<ResourceType, Type>();
                    foreach (Type t in Assembly.GetExecutingAssembly().GetTypes())
                        foreach (NodeWrapperAttribute attr in t.GetCustomAttributes(typeof(NodeWrapperAttribute), true))
                            _wrappers[attr._type] = t;
                }
                return _wrappers;
            }
        }
    }
    [Serializable]
    public abstract class BaseWrapper : TreeNode
    {
        protected static readonly ContextMenuStrip _emptyMenu = new ContextMenuStrip();

        protected bool _discovered = false;

        protected ResourceNode _resource;
        public ResourceNode Resource
        {
            get { return _resource; }
            //set { Link(value); }
        }

        protected BaseWrapper() { }
        //protected BaseWrapper(ResourceNode resourceNode) { Link(resourceNode); }

        protected static T GetInstance<T>() where T : BaseWrapper { return MainForm.Instance.resourceTree.SelectedNode as T; }
        protected static IEnumerable<T> GetInstances<T>() where T : BaseWrapper { return MainForm.Instance.resourceTree.SelectedNodes.Select(n => n as T).Where(n => n != null); }

        public void Link(ResourceNode res)
        {
            Unlink();
            if (res != null)
            {
                this.Text = res.Name;
                TreeNodeCollection nodes = Nodes;

                //Should we continue down the tree?
                if ((IsExpanded) && (res.HasChildren))
                {
                    //Add/link each resource node
                    foreach (ResourceNode n in res.Children)
                    {
                        bool found = false;
                        foreach (BaseWrapper tn in nodes)
                            if (tn.Text == n.Name)
                            {
                                tn.Link(n);
                                found = true;
                                // Move node to bottom, to ensure that nodes are shown and saved in the same order as in the original data
                                nodes.Remove(tn);
                                nodes.Add(tn);
                                break;
                            }

                        if (!found)
                            nodes.Add(Wrap(_owner, n));
                    }

                    //Remove empty nodes
                    for (int i = 0; i < nodes.Count;)
                    {
                        BaseWrapper n = nodes[i] as BaseWrapper;
                        if (n._resource == null)
                            n.Remove();
                        else
                            i++;
                    }

                    _discovered = true;
                }
                else
                {
                    //Node will be reset and undiscovered
                    nodes.Clear();
                    //Collapse();
                    if (res.HasChildren)
                    {
                        nodes.Add(new GenericWrapper());
                        _discovered = false;
                    }
                    else
                        _discovered = true;
                }

                SelectedImageIndex = ImageIndex = (int)res.ResourceType & 0xFF;

                res.SelectChild += OnSelectChild;
                res.ChildAdded += OnChildAdded;
                res.ChildRemoved += OnChildRemoved;
                res.ChildInserted += OnChildInserted;
                res.Replaced += OnReplaced;
                res.Restored += OnRestored;
                res.Renamed += OnRenamed;
                res.MovedUp += OnMovedUp;
                res.MovedDown += OnMovedDown;
                res.PropertyChanged += OnPropertyChanged;
                res.UpdateProps += OnUpdateProperties;
                res.UpdateControl += OnUpdateCurrentControl;
            }
            _resource = res;
        }
        public void Unlink()
        {
            if (_resource != null)
            {
                _resource.SelectChild -= OnSelectChild;
                _resource.ChildAdded -= OnChildAdded;
                _resource.ChildRemoved -= OnChildRemoved;
                _resource.ChildInserted -= OnChildInserted;
                _resource.Replaced -= OnReplaced;
                _resource.Restored -= OnRestored;
                _resource.Renamed -= OnRenamed;
                _resource.MovedUp -= OnMovedUp;
                _resource.MovedDown -= OnMovedDown;
                _resource.PropertyChanged -= OnPropertyChanged;
                _resource.UpdateProps -= OnUpdateProperties;
                _resource.UpdateControl -= OnUpdateCurrentControl;
                _resource = null;
            }

            foreach (BaseWrapper n in Nodes)
                n.Unlink();
        }
        internal protected virtual void OnSelectChild(int index)
        {
            if (!(Nodes == null || index < 0 || index >= Nodes.Count))
                TreeView.SelectedNode = Nodes[index];
        }
        internal protected virtual void OnUpdateProperties(object sender, EventArgs e)
        {
            MainForm.Instance.propertyGrid1.Refresh();
        }
        internal protected virtual void OnUpdateCurrentControl(object sender, EventArgs e)
        {
            MainForm form = MainForm.Instance;
            //var g = form.propertyGrid1.SelectedGridItem;
            form._currentControl = null;
            form.resourceTree_SelectionChanged(this, null);
        }
        internal protected virtual void OnChildAdded(ResourceNode parent, ResourceNode child)
        {
            Nodes.Add(Wrap(_owner, child));
        }
        internal protected virtual void OnChildInserted(int index, ResourceNode parent, ResourceNode child)
        {
            Nodes.Insert(index, Wrap(_owner, child));
        }
        internal protected virtual void OnChildRemoved(ResourceNode parent, ResourceNode child)
        {
            foreach (BaseWrapper w in Nodes)
                if (w != null)
                    if (w._resource == child)
                    {
                        w.Unlink();
                        w.Remove();
                    }
        }
        internal protected void RefreshView(ResourceNode node)
        {
            Link(node);

            if ((TreeView != null) && (TreeView.SelectedNode == this))
            {
                ((ResourceTree)TreeView).SelectedNode = null;
                TreeView.SelectedNode = this;
            }
        }
        internal protected virtual void OnRestored(ResourceNode node)
        {
            RefreshView(node);
        }
        internal protected virtual void OnReplaced(ResourceNode node)
        {
            RefreshView(node);
        }
        internal protected virtual void OnRenamed(ResourceNode node) { Text = node.Name; }

        internal protected virtual void OnMovedUp(ResourceNode node, bool select)
        {
            GenericWrapper res = this.FindResource(node, false) as GenericWrapper;
            res.MoveUp(select);
            res.EnsureVisible();
            //res.TreeView.SelectedNode = res;
        }
        internal protected virtual void OnMovedDown(ResourceNode node, bool select)
        {
            GenericWrapper res = this.FindResource(node, false) as GenericWrapper;
            res.MoveDown(select);
            res.EnsureVisible();
            //res.TreeView.SelectedNode = res;
        }
        internal protected virtual void OnPropertyChanged(ResourceNode node) { }

        internal protected virtual void OnExpand()
        {
            if (!_discovered)
            {
                Nodes.Clear();

                if (_resource._isPopulating)
                    while (_resource._isPopulating) { Application.DoEvents(); }

                foreach (ResourceNode n in _resource.Children)
                    Nodes.Add(Wrap(_owner, n));
                _discovered = true;
            }
        }
        internal protected virtual void OnDoubleClick() { }

        internal BaseWrapper FindResource(ResourceNode n, bool searchChildren)
        {
            BaseWrapper node;
            if (_resource == n)
                return this;
            else
            {
                OnExpand();
                foreach (BaseWrapper c in Nodes)
                    if (c._resource == n)
                        return c;
                    else if ((searchChildren) && ((node = c.FindResource(n, true)) != null))
                        return node;
            }
            return null;
        }

        public static IWin32Window _owner;
        public static BaseWrapper Wrap(ResourceNode node) { return Wrap(null, node); }
        public static BaseWrapper Wrap(IWin32Window owner, ResourceNode node)
        {
            _owner = owner;
            BaseWrapper w;
            if (!NodeWrapperAttribute.Wrappers.ContainsKey(node.ResourceType))
                w = new GenericWrapper();
            else
                w = Activator.CreateInstance(NodeWrapperAttribute.Wrappers[node.ResourceType]) as BaseWrapper;
            w.Link(node);
            return w;
        }
    }
}
