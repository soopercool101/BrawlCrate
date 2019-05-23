using System;
using System.Collections.Generic;
using BrawlLib.IO;
using System.ComponentModel;
using BrawlLib.Wii.Compression;
using System.IO;
using System.Windows.Forms;
using System.Security.Cryptography;
using System.Text;

namespace BrawlLib.SSBB.ResourceNodes
{
    public delegate void SelectEventHandler(int index);
    public delegate void MoveEventHandler(ResourceNode node, bool select);
    public delegate void ResourceEventHandler(ResourceNode node);
    public delegate void ResourceChildEventHandler(ResourceNode node, ResourceNode child);
    public delegate void ResourceChildInsertEventHandler(int index, ResourceNode node, ResourceNode child);
    public unsafe struct DataSource
    {
        public static readonly DataSource Empty = new DataSource();

        public VoidPtr Address;
        public int Length;
        public FileMap Map;
        public CompressionType Compression;

        public DataSource(VoidPtr addr, int len) : this(addr, len, CompressionType.None) { }
        public DataSource(VoidPtr addr, int len, CompressionType compression)
        {
            Address = addr;
            Length = len;
            Map = null;
            Compression = compression;
        }
        public DataSource(FileMap map) : this(map, CompressionType.None) { }
        public DataSource(FileMap map, CompressionType compression)
        {
            Address = map.Address;
            Length = map.Length;
            Map = map;
            Compression = compression;
        }

        public void Close()
        {
            if (Map != null) { Map.Dispose(); Map = null; }
            Address = null;
            Length = 0;
            Compression = CompressionType.None;
        }

        public static bool operator ==(DataSource src1, DataSource src2) { return (src1.Address == src2.Address) && (src1.Length == src2.Length) && (src1.Map == src2.Map); }
        public static bool operator !=(DataSource src1, DataSource src2) { return (src1.Address != src2.Address) || (src1.Length != src2.Length) || (src1.Map != src2.Map); }
        public override bool Equals(object obj)
        {
            if (obj is DataSource)
                return this == (DataSource)obj;
            return base.Equals(obj);
        }
        public override int GetHashCode() { return base.GetHashCode(); }
    }

    public abstract class ResourceNode : IDisposable
    {
        public Form _mainForm;

        //Need to modulate these sources, create a new class.
        internal protected DataSource _origSource, _uncompSource;
        internal protected DataSource _replSrc, _replUncompSrc;

        internal protected bool _changed, _merged, _disposed = false;
        internal protected CompressionType _compression;

        public string _name, _origPath;
        public ResourceNode _parent;
        public List<ResourceNode> _children = new List<ResourceNode>();

        public int _calcSize;

        internal protected ResourceNode _first, _last;
        internal protected bool _hasChildren;

        public event SelectEventHandler SelectChild;
        public event EventHandler UpdateProps, UpdateControl;
        public event MoveEventHandler MovedUp, MovedDown;
        public event ResourceEventHandler Disposing, Renamed, PropertyChanged, Replaced, Restored;
        public event ResourceChildEventHandler ChildAdded, ChildRemoved;
        public event ResourceChildInsertEventHandler ChildInserted;

        #region Properties

        [Browsable(false)]
        public string FilePath { get { return _origPath; } }
        [Browsable(false)]
        public ResourceNode RootNode { get { return _parent == null || _parent == this ? this : _parent.RootNode; } }
        [Browsable(false)]
        public DataSource OriginalSource { get { return _origSource; } }
        [Browsable(false)]
        public DataSource UncompressedSource { get { return _uncompSource; } }
        [Browsable(false)]
        public DataSource WorkingSource { get { return _replSrc != DataSource.Empty ? _replSrc : _origSource; } }
        [Browsable(false)]
        public DataSource WorkingUncompressed { get { return _replUncompSrc != DataSource.Empty ? _replUncompSrc : _uncompSource; } }
        [Browsable(false)]
        public virtual bool HasChildren { get { return (_children == null) || (_children.Count != 0); } }
        [Browsable(false)]
        public virtual ResourceType ResourceType { get { return ResourceType.Unknown; } }
        [Browsable(false)]
        public virtual string TreePathAbsolute { get { return _parent == null ? Name : _parent.TreePathAbsolute + "/" + Name; } }
        [Browsable(false)]
        public virtual string TreePath
        {
            get
            {
                string path = TreePathAbsolute;
                int index = path.IndexOf('/');
                if (index > 0)
                    path = path.Substring(index + 1);
                return path;
            }
        }
        [Browsable(false)]
        public virtual int Level { get { return _parent == null ? 0 : _parent.Level + 1; } }
        [Browsable(false)]
        public virtual bool AllowDuplicateNames { get { return false; } }
        [Browsable(false)]
        public virtual bool AllowNullNames { get { return false; } }
        [Browsable(false)]
        public virtual string Name
        {
            get { return String.IsNullOrEmpty(_name) ? _name = "<null>" : _name; }
            set
            {
                if (_name == value)
                    return;

                _name = value;
                _changed = true;
                if (Renamed != null)
                    Renamed(this);
            }
        }
        [Browsable(false)]
        public ResourceNode Parent
        {
            get { return _parent; }
            set
            {
                if (_parent == value)
                    return;

                Remove();
                _parent = value;
                if (_parent != null)
                    _parent.Children.Add(this);
            }
        }
        [Browsable(false)]
        public List<ResourceNode> Children
        {
            get
            {
                if (_children == null)
                {
                    _children = new List<ResourceNode>();
                    if (WorkingSource != DataSource.Empty)
                        OnPopulate();
                }
                return _children;
            }
        }
        [Browsable(false)]
        public int Index { get { return _parent == null ? -1 : _parent.Children.IndexOf(this); } }
        [Browsable(false)]
        public bool IsCompressed { get { return _compression != CompressionType.None; } }

        //Properties or compression have changed
        [Browsable(false)]
        public bool HasChanged { get { return _changed; } set { _changed = value; } }

        public virtual void SignalPropertyChange()
        {
            if (PropertyChanged != null)
                PropertyChanged(this);
            _changed = true;
        }

        //Has the node deviated from its parent?
        [Browsable(false)]
        public bool IsBranch { get { return _replSrc.Map != null; } }

        [Browsable(false)]
        public bool HasMerged { get { return _merged; } }

        //Can be any of the following: children have branched, children have changed, current has changed
        //Node needs to be rebuilt.
#if DEBUG
        [Browsable(true), Category("DEBUG")]
#else
        [Browsable(false)]
#endif
        public bool IsDirty
        {
            get
            {
                if (HasChanged)
                    return true;
                if (_children != null)
                    foreach (ResourceNode n in _children)
                        if (n.HasChanged || n.IsBranch || n.IsDirty)
                            return true;
                return false;
            }
            set
            {
                _changed = value;
                if (_children != null)
                    foreach (ResourceNode r in Children)
                        if (r._children != null)
                            r.IsDirty = value;
                        else
                            r._changed = value;
            }
        }

        [Browsable(false)]
        public virtual Type[] AllowedChildTypes { get { return _allowedChildTypes; } }
        private Type[] _allowedChildTypes = new Type[] { };

        [Browsable(false), TypeConverter(typeof(DropDownListCompression))]
        public virtual string Compression
        {
            get { return _compression.ToString(); }
            set
            {
                CompressionType type;
                if (Enum.TryParse(value, out type))
                {
                    if (type == _compression)
                        return;

                    if (Array.IndexOf(Compressor._supportedCompressionTypes, type) != -1)
                    {
                        _compression = type;
                        _changed = true;
                    }
                }
            }
        }

#endregion

#region Disposal

        ~ResourceNode() { Dispose(); }
        public virtual void Dispose()
        {
            if (_disposed)
                return;

            _disposed = true;

            if (Disposing != null)
                Disposing(this);

            //if (_parent != null)
            //{
            //    _parent._children.Remove(this);
            //    _parent = null;
            //}

            if (_children != null)
            {
                //while (_children.Count > 0)
                //    _children[0].Dispose();

                foreach (ResourceNode node in _children)
                    node.Dispose();
                //_children.Clear();
                //_children = null;
            }

            //_currentSource.Close();
            _uncompSource.Close();
            _origSource.Close();
            _replUncompSrc.Close();
            _replSrc.Close();

            GC.SuppressFinalize(this);
        }

#endregion

        public void SelectChildAtIndex(int index)
        {
            if (SelectChild != null)
                SelectChild(index);
        }   
        public void UpdateProperties()
        {
            if (UpdateProps != null)
                UpdateProps(this, null);
        }
        public void UpdateCurrentControl()
        {
            if (UpdateControl != null)
                UpdateControl(this, null);
        }

#region Moving

        public virtual bool MoveUp()
        {
            if (Parent == null)
                return false;

            int index = Index - 1;
            if (index < 0)
                return false;

            Parent.Children.Remove(this);
            Parent.Children.Insert(index, this);
            Parent._changed = true;
            return true;
        }

        public virtual bool MoveDown()
        {
            if (Parent == null)
                return false;

            int index = Index + 1;
            if (index >= Parent.Children.Count)
                return false;

            Parent.Children.Remove(this);
            Parent.Children.Insert(index, this);
            Parent._changed = true;
            return true;
        }

        public virtual void OnMoved() { }

        public virtual void DoMoveDown() { DoMoveDown(true); }
        public virtual void DoMoveDown(bool select)
        {
            if (MovedDown != null)
                MovedDown(this, select);
            else
                MoveDown();
        }

        public virtual void DoMoveUp() { DoMoveUp(true); }
        public virtual void DoMoveUp(bool select)
        {
            if (MovedUp != null)
                MovedUp(this, select);
            else
                MoveUp();
        }

        public virtual bool AddUp()
        {
            int index = Index - 1;
            if (index < 0)
                return false;

            if (Parent.Children[index] is MDL0BoneNode)
                return true;
            else
                return false;
        }

        public virtual bool AddDown()
        {
            int index = Index + 1;
            if (index >= Parent.Children.Count)
                return false;

            if (Parent.Children[index] is MDL0BoneNode)
                return true;
            else
                return false;
        }

        public virtual bool ToParent()
        {
            if (Parent != null && Parent is MDL0BoneNode)
                return true;
            else
                return false;
        }

#endregion

#region Child Population
        public bool _isPopulating;
        public void Populate(int levels = -1)
        {
            _isPopulating = true;
            if (levels > 0)
                foreach (ResourceNode r in Children)
                    r.Populate(levels - 1);
            else if (levels < 0)
                foreach (ResourceNode r in Children)
                    r.Populate();
            else if (_children == null || _children.Count == 0)
            {
                _children = new List<ResourceNode>();
                if (WorkingSource != DataSource.Empty)
                    OnPopulate();
            }
            _isPopulating = false;
        }
        //Called when children are first requested. Allows node to cache child nodes.
        public virtual void OnPopulate() { }
#endregion

#region Initialization
        public void Initialize(ResourceNode parent, FileMap source) { Initialize(parent, new DataSource(source)); }
        public void Initialize(ResourceNode parent, VoidPtr address, int length) { Initialize(parent, new DataSource(address, length)); }
        public void Initialize(ResourceNode parent, DataSource origSource) { Initialize(parent, origSource, origSource); }
        public virtual void Initialize(ResourceNode parent, DataSource origSource, DataSource uncompSource)
        {
            _origSource = origSource;
            _uncompSource = uncompSource;
            _compression = _origSource.Compression;

            if (origSource.Map != null)
                _origPath = origSource.Map.FilePath;

            Parent = parent;

            _children = null;

            if (Parent != null && Parent._replaced)
                _replaced = true;

            if (!OnInitialize())
                _children = new List<ResourceNode>();

            _replaced = false;
        }
        //Called when property values are requested. Allows node to cache values from source data.
        //Return true to indicate there are child nodes.
        public virtual bool OnInitialize() { return false; }
        //Restores node to its original form using the backing tree. 
        public virtual void Restore()
        {
            if ((!IsDirty) && (!IsBranch))
                return;

            if (_children != null)
            {
                foreach (ResourceNode node in _children)
                    node.Dispose();
                _children.Clear();
                _children = null;
            }

            _replUncompSrc.Close();
            _replSrc.Close();
            _compression = _origSource.Compression;

            if (_origSource != DataSource.Empty && !OnInitialize())
                _children = new List<ResourceNode>();

            _changed = false;
            if (Restored != null)
                Restored(this);
        }
#endregion

#region Adding/Removing

        public virtual void Remove()
        {
            if (_parent != null)
                _parent.RemoveChild(this);
        }

        public virtual void RemoveChild(ResourceNode child)
        {
            if ((_children != null) && (_children.Remove(child)))
            {
                child._parent = null;
                if (ChildRemoved != null)
                    ChildRemoved(this, child);
                _changed = true;
            }
        }

        public virtual void AddChild(ResourceNode child) { AddChild(child, true); }
        public virtual void AddChild(ResourceNode child, bool change)
        {
            Children.Add(child);
            child._parent = this;
            if (ChildAdded != null)
                ChildAdded(this, child);
            if (change)
                _changed = true;
        }
        public virtual void InsertChild(ResourceNode child, bool change, int index)
        {
            Children.Insert(index, child);
            child._parent = this;
            if (ChildInserted != null)
                ChildInserted(index, this, child);
            if (change)
                _changed = true;
        }

#endregion

        public void SetSizeInternal(int size)
        {
            if (IsBranch)
                if (IsCompressed)
                    _replUncompSrc.Length = size;
                else
                    _replSrc.Length = _replUncompSrc.Length = size;
            else
                if (IsCompressed)
                    _uncompSource.Length = size;
                else
                    _origSource.Length = _uncompSource.Length = size;
        }

#region Replacing

        //Causes a deviation in the resource tree. This node and all child nodes will be backed by a temporary file until the tree is merged.
        //Causes parent node(s) to become dirty.
        //Replace will reference the file in a new DataSource.
        public bool _replaced = false;
        public unsafe virtual void Replace(string fileName) { Replace(fileName, FileMapProtect.Read, FileOptions.SequentialScan); }
        public unsafe virtual void Replace(string fileName, FileMapProtect prot, FileOptions options)
        {
            //Name = Path.GetFileNameWithoutExtension(fileName);
            ReplaceRaw(FileMap.FromFile(fileName, prot, 0, 0, options));
        }
        public unsafe virtual void ReplaceRaw(VoidPtr address, int length)
        {
            FileMap map = FileMap.FromTempFile(length);
            Memory.Move(map.Address, address, (uint)length);
            ReplaceRaw(map);
        }
        [Browsable(false)]
        public virtual bool RetainChildrenOnReplace { get { return false; } }
        public unsafe virtual void ReplaceRaw(FileMap map)
        {
            if (_children != null && !RetainChildrenOnReplace)
            {
                foreach (ResourceNode node in _children)
                    node.Dispose();
                _children.Clear();
                _children = null;
            }

            _replUncompSrc.Close();
            _replSrc.Close();

            _replSrc = new DataSource(map);

            FileMap uncompMap = Compressor.TryExpand(ref _replSrc, false);
            _compression = _replSrc.Compression;
            _replUncompSrc = uncompMap != null ? new DataSource(uncompMap) : _replSrc;

            _replaced = true;
            if (!OnInitialize() && !RetainChildrenOnReplace)
                _children = new List<ResourceNode>();
            _replaced = false;

            _changed = false;
            if (Replaced != null)
                Replaced(this);
        }

        protected void ForceReplacedEvent()
        {
            if (Replaced != null)
                Replaced(this);
        }

#endregion

#region Export

        public unsafe virtual void Export(string outPath)
        {
            Rebuild(); //Apply changes the user has made by rebuilding.
#if !DEBUG
            try
            {
#endif
                using (FileStream stream = new FileStream(outPath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite, 8, FileOptions.SequentialScan))
                    Export(stream);
#if !DEBUG
            }
            catch { MessageBox.Show("Unable to open file for write access."); }
#endif
        }

        public void Export(FileStream outStream)
        {
            if (WorkingSource.Length != 0)
            {
                outStream.SetLength(WorkingSource.Length);
                using (FileMap map = FileMap.FromStream(outStream))
                    Memory.Move(map.Address, WorkingSource.Address, (uint)WorkingSource.Length);
            }
            else
                MessageBox.Show("Data was empty!");
        }

        public virtual void ExportUncompressed(string outPath)
        {
            Rebuild(); //Apply changes the user has made by rebuilding.
            try
            {
                using (FileStream stream = new FileStream(outPath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None, 8, FileOptions.SequentialScan))
                    ExportUncompressed(stream);
            }
            catch { MessageBox.Show("Unable to open file for write access."); }
        }
        public void ExportUncompressed(FileStream outStream)
        {
            if (WorkingUncompressed.Length != 0)
            {
                outStream.SetLength(WorkingUncompressed.Length);
                using (FileMap map = FileMap.FromStream(outStream))
                    Memory.Move(map.Address, WorkingUncompressed.Address, (uint)WorkingUncompressed.Length);
            }
            else
                MessageBox.Show("Data was empty!");
        }

#endregion

#region Rebuilding

        //Combines node and children into single (temp) file map.
        //Does nothing if node is not dirty or rebuild is not forced.
        //Calls OnCalculateSize on self, which will allow the node to cache any values for OnRebuild
        public virtual void Rebuild() { Rebuild(false); }
        public virtual void Rebuild(bool force)
        {
            if (!IsDirty && !force)
                return;

            //Get uncompressed size
            int size = OnCalculateSize(force);

            //Create temp map
            FileMap uncompMap = FileMap.FromTempFile(size);

            //Rebuild node (uncompressed)
            Rebuild(uncompMap.Address, size, force);
            _replSrc.Map = _replUncompSrc.Map = uncompMap;

            //If compressed, compress resulting data.
            if (_compression != CompressionType.None)
            {
                //Compress node to temp file
                FileStream stream = new FileStream(Path.GetTempFileName(), FileMode.Open, FileAccess.ReadWrite, FileShare.None, 0x8, FileOptions.DeleteOnClose | FileOptions.SequentialScan);
                try
                {
                    Compressor.Compact(_compression, uncompMap.Address, uncompMap.Length, stream, this);
                    _replSrc = new DataSource(FileMap.FromStreamInternal(stream, FileMapProtect.Read, 0, (int)stream.Length), _compression);
                }
                catch (Exception) { stream.Dispose(); throw; }
            }
        }

        //Called on child nodes in order to rebuild them at a specified address.
        //This will occur after CalculateSize, so compressed nodes will already be rebuilt.
        public virtual void Rebuild(VoidPtr address, int length, bool force)
        {
            if (!IsDirty && !force)
                MoveRaw(address, length);
            else
            {
                OnRebuild(address, length, force);

                IsDirty = false;

                //Code has been moved here, because all overrides are doing the same thing.
                _replSrc.Close();
                _replUncompSrc.Close();
                _replSrc = _replUncompSrc = new DataSource(address, length);
            }
        }
        //Overridden by derived nodes in order to rebuild children.
        //Size is the value returned by OnCalculateSize (or _calcSize)
        //Node MUST dispose of and assign both repl sources before exiting. (Not exactly, see Rebuild())
        public virtual void OnRebuild(VoidPtr address, int length, bool force)
        {
            MoveRawUncompressed(address, length);
        }

        //Shouldn't this move compressed data? YES!
        internal virtual void MoveRaw(VoidPtr address, int length)
        {
            Memory.Move(address, WorkingSource.Address, (uint)length);
            DataSource newsrc = new DataSource(address, length);
            if (_compression == CompressionType.None)
            {
                if (_children != null)
                {
                    int offset = address - WorkingSource.Address;
                    foreach (ResourceNode n in _children)
                        n.OnParentMoved(offset);
                }

                _replSrc.Close();
                _replUncompSrc.Close();
                _replSrc = _replUncompSrc = newsrc;
            }
            else
            {
                _replSrc.Close();
                _replSrc = newsrc;
            }
        }
        internal virtual void OnParentMoved(int offset)
        {
            if (_compression == CompressionType.None)
            {
                if (_replSrc != DataSource.Empty)
                    _replSrc.Address = _replUncompSrc.Address += offset;
                else if (_origSource != DataSource.Empty)
                    _origSource.Address = _uncompSource.Address += offset;

                if (_children != null)
                    foreach (ResourceNode n in _children)
                        n.OnParentMoved(offset);
            }
            else
            {
                if (_replSrc != DataSource.Empty)
                    _replSrc.Address += offset;
                else if (_origSource != DataSource.Empty)
                    _origSource.Address += offset;
            }
        }

        internal virtual void MoveRawUncompressed(VoidPtr address, int length)
        {
            Memory.Move(address, WorkingUncompressed.Address, (uint)length);
            DataSource newsrc = new DataSource(address, length);

            int offset = address - WorkingUncompressed.Address;
            foreach (ResourceNode n in Children)
                n.OnParentMovedUncompressed(offset);
            
            _replUncompSrc.Close();
            _replUncompSrc = newsrc;
        }
        internal virtual void OnParentMovedUncompressed(int offset)
        {
            if (_replUncompSrc != DataSource.Empty)
                _replUncompSrc.Address += offset;
            else if (_uncompSource != DataSource.Empty)
                _uncompSource.Address += offset;

            if (_children != null)
                foreach (ResourceNode n in _children)
                    n.OnParentMovedUncompressed(offset);
        }

#endregion

#region Size Calculation

        //Calculate size to be passed to parent node.
        //If node is compressed, rebuild now and compress to temp file. Return temp file size.
        //Called on child nodes only, because it can trigger a rebuild.
        public virtual int CalculateSize(bool force)
        {
            if (IsDirty || force)
            {
                if (_compression == CompressionType.None)
                    return _calcSize = OnCalculateSize(force);
                Rebuild(force);
            }
            return _calcSize = WorkingSource.Length;
        }

        //Returns uncompressed size of node data.
        //It's up to the child nodes to return compressed sizes.
        //If this has been called, it means a rebuild must happen.
        public virtual int OnCalculateSize(bool force)
        {
            return WorkingUncompressed.Length;
        }

#endregion

#region Merging

        //Combines deviated tree into backing tree. Backing tree will have moved completely to a temporary file.
        //All references to backing tree will be gone! Including file handles.
        public void Merge() { Merge(false); }
        public void Merge(bool forceBuild)
        {
            if (_parent != null)
                throw new InvalidOperationException("Merge can only be called on the root node!");

            if (forceBuild || IsDirty)
                Rebuild(forceBuild);

            MergeInternal();
            _merged = true;
        }

        //Swap data sources to only use new temp file. Closes original sources.
        protected virtual void MergeInternal()
        {
            if (_children != null)
                foreach (ResourceNode n in Children)
                    n.MergeInternal();

            if (_replSrc != DataSource.Empty)
            {
                _origSource.Close();
                _origSource = _replSrc;
                _replSrc = DataSource.Empty;

                if (_replUncompSrc != DataSource.Empty)
                {
                    _uncompSource.Close();
                    _uncompSource = _replUncompSrc;
                    _replUncompSrc = DataSource.Empty;
                }
            }
        }

#endregion

#region Child Node Searches

        public static ResourceNode FindNode(ResourceNode root, string path, bool searchChildren)
        {
            if (String.IsNullOrEmpty(path))
                return root;

            if (root.Name.Equals(path, StringComparison.OrdinalIgnoreCase))
                return root;

            if ((path.Contains("/")) && (path.Substring(0, path.IndexOf('/')).Equals(root.Name, StringComparison.OrdinalIgnoreCase)))
                return root.FindChild(path.Substring(path.IndexOf('/') + 1), searchChildren);

            return root.FindChild(path, searchChildren);
        }

        public ResourceNode FindChildByType(string path, bool searchChildren, ResourceType type)
        {
            if (path == null)
                return null;

            ResourceNode node = null;
            if (path.Contains("/"))
            {
                string next = path.Substring(0, path.IndexOf('/'));
                foreach (ResourceNode n in Children)
                    if (n.Name != null && n.Name.Equals(next, StringComparison.OrdinalIgnoreCase))
                        if ((node = FindNode(n, path.Substring(next.Length + 1), searchChildren)) != null && node.ResourceType == type)
                            return node;
            }
            else
            {
                //Search direct children first
                foreach (ResourceNode n in Children)
                    if (n.Name != null && n.Name.Equals(path, StringComparison.OrdinalIgnoreCase) && n.ResourceType == type)
                        return n;

            }
            if (searchChildren)
                foreach (ResourceNode n in Children)
                    if ((node = n.FindChildByType(path, true, type)) != null && node.ResourceType == type)
                        return node;

            return null;
        }

        public ResourceNode FindChild(string path, bool searchChildren)
        {
            ResourceNode node = null;
            if (path == null)
                return null;
            if (path.Contains("/"))
            {
                string next = path.Substring(0, path.IndexOf('/'));
                foreach (ResourceNode n in Children)
                    if (n.Name != null && n.Name.Equals(next, StringComparison.OrdinalIgnoreCase))
                        if ((node = FindNode(n, path.Substring(next.Length + 1), searchChildren)) != null)
                            return node;
            }
            else
            {
                //Search direct children first
                foreach (ResourceNode n in Children)
                    if (n.Name != null && n.Name.Equals(path, StringComparison.OrdinalIgnoreCase))
                        return n;

            }
            if (searchChildren)
                foreach (ResourceNode n in Children.ToArray())
                    if ((node = n.FindChild(path, true)) != null)
                        return node;

            return null;
        }

        public ResourceNode[] FindChildrenByClassType(string path, Type type)
        {
            if (!String.IsNullOrEmpty(path))
            {
                ResourceNode node = FindChild(path, false);
                if (node != null)
                    return node.FindChildrenByClassType(null, type);
            }

            List<ResourceNode> nodes = new List<ResourceNode>();
            this.EnumClassTypeInternal(nodes, type);
            return nodes.ToArray();
        }
        private void EnumClassTypeInternal(List<ResourceNode> list, Type type)
        {
            if (GetType() == type)
                list.Add(this);
            foreach (ResourceNode n in Children)
                n.EnumClassTypeInternal(list, type);
        }

        public ResourceNode[] FindChildrenByType(string path, ResourceType type)
        {
            if (!String.IsNullOrEmpty(path))
            {
                ResourceNode node = FindChild(path, false);
                if (node != null)
                    return node.FindChildrenByType(null, type);
            }

            List<ResourceNode> nodes = new List<ResourceNode>();
            this.EnumTypeInternal(nodes, type);
            return nodes.ToArray();
        }
        private void EnumTypeInternal(List<ResourceNode> list, ResourceType type)
        {
            if (this.ResourceType == type)
                list.Add(this);
            foreach (ResourceNode n in Children)
                n.EnumTypeInternal(list, type);
        }

        public ResourceNode[] FindChildrenByName(string name)
        {
            List<ResourceNode> nodes = new List<ResourceNode>();
            this.EnumNameInternal(nodes, name);
            return nodes.ToArray();
        }
        private void EnumNameInternal(List<ResourceNode> list, string name)
        {
            if (this.Name == name)
                list.Add(this);
            foreach (ResourceNode n in Children)
                n.EnumNameInternal(list, name);
        }

        public unsafe string FindName(string name)
        {
            int index = -1;

            if (string.IsNullOrEmpty(name))
                name = "NewNode";

            int len = name.Length;
            sbyte* charList = stackalloc sbyte[len + 3];
            PString pStr = charList;

            for (int i = 0; i < len; i++)
                charList[i] = (sbyte)name[i];

        Top:

            if (index < 0)
                charList[len] = 0;
            else
            {
                charList[len] = (sbyte)((index % 10) | 0x30);
                if (index < 10)
                    charList[len + 1] = 0;
                else
                {
                    charList[len + 1] = (sbyte)((index / 10) | 0x30);
                    charList[len + 2] = 0;
                }
            }

            index++;
            foreach (ResourceNode node in Children)
                if (pStr == node.Name)
                    goto Top;

            return new String(charList);
        }

        public ResourceNode FindEmbeddedIndex(int index)
        {
            int count = -1;
            return FindEmbeddedInternal(this, index, ref count);
        }
        private static ResourceNode FindEmbeddedInternal(ResourceNode node, int index, ref int count)
        {
            if (count++ >= index)
                return node;

            foreach (ResourceNode n in node.Children)
                if ((node = FindEmbeddedInternal(n, index, ref count)) != null)
                    return node;

            return null;
        }

#endregion

#region MD5
        private static MD5CryptoServiceProvider _md5provider;
        protected static MD5CryptoServiceProvider MD5Provider
        {
            get
            {
                if (_md5provider == null) _md5provider = new MD5CryptoServiceProvider();
                return _md5provider;
            }
        }

        /// <summary>
        /// Find the MD5 checksum of this node's data.
        /// If this node doesn't have any data (BRESGroupNode, for example),
        /// this method will return null.
        /// </summary>
        public virtual unsafe byte[] MD5()
        {
            DataSource data = this.OriginalSource;
            if (data.Address == null || data.Length == 0)
            {
                return null;
            }
            else
            {
                UnmanagedMemoryStream stream = new UnmanagedMemoryStream((byte*)data.Address, data.Length);
                return MD5Provider.ComputeHash(stream);
            }
        }

        /// <summary>
        /// Get the result of the MD5() function as a string of hexadecimal digits.
        /// If MD5() returns null, this method will return an empty string.
        /// </summary>
        [System.Runtime.ExceptionServices.HandleProcessCorruptedStateExceptions]
        public unsafe string MD5Str()
        {
            try
            {
                byte[] checksum = this.MD5();
                if (checksum == null) return string.Empty;
                StringBuilder sb = new StringBuilder(checksum.Length * 2);
                for (int i = 0; i < checksum.Length; i++)
                {
                    sb.Append(checksum[i].ToString("X2"));
                }
                return sb.ToString();
            }
            catch (AccessViolationException)
            {
                return "----AccessViolationException----";
            }
        }
#endregion

        public ResourceNode NextSibling()
        {
            if (_parent == null)
                return null;
            int siblingIndex = Index + 1;
            if (siblingIndex >= Parent.Children.Count)
                return null;

            return Parent.Children[siblingIndex];
        }

        public override string ToString()
        {
            return Name;
        }
    }

    public class NodeComparer : IComparer<ResourceNode>
    {
        public static NodeComparer Instance = new NodeComparer();
        public int Compare(ResourceNode x, ResourceNode y) { return String.CompareOrdinal(x.Name, y.Name); }
    }
}
