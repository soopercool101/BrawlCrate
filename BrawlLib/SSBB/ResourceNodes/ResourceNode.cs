using BrawlLib.Internal;
using BrawlLib.Internal.IO;
using BrawlLib.Wii;
using BrawlLib.Wii.Compression;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

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

        public string Tag
        {
            get
            {
                if (Length < 4)
                {
                    return null;
                }

                return Address.GetUTF8String(0, 4);
            }
        }

        public DataSource(VoidPtr addr, int len) : this(addr, len, CompressionType.None)
        {
        }

        public DataSource(VoidPtr addr, int len, CompressionType compression)
        {
            Address = addr;
            Length = len;
            Map = null;
            Compression = compression;
        }

        public DataSource(FileMap map) : this(map, CompressionType.None)
        {
        }

        public DataSource(FileMap map, CompressionType compression)
        {
            Address = map.Address;
            Length = map.Length;
            Map = map;
            Compression = compression;
        }

        public DataSource(MemoryStream ms) : this(ms, CompressionType.None)
        {
        }

        public DataSource(MemoryStream ms, CompressionType compression)
        {
            ms.Position = 0;
            Address = Marshal.AllocHGlobal((int) ms.Length);
            Marshal.Copy(ms.ToArray(), 0, Address, (int) ms.Length);
            Length = (int) ms.Length;
            Map = null;
            Compression = compression;
        }

        public void Close()
        {
            if (Map != null)
            {
                Map.Dispose();
                Map = null;
            }

            Address = null;
            Length = 0;
            Compression = CompressionType.None;
        }

        public static bool operator ==(DataSource src1, DataSource src2)
        {
            return src1.Address == src2.Address && src1.Length == src2.Length && src1.Map == src2.Map;
        }

        public static bool operator !=(DataSource src1, DataSource src2)
        {
            return src1.Address != src2.Address || src1.Length != src2.Length || src1.Map != src2.Map;
        }

        public override bool Equals(object obj)
        {
            if (obj is DataSource)
            {
                return this == (DataSource) obj;
            }

            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public byte this[uint i] => (Address + i).Byte;
    }

    public abstract class ResourceNode : IDisposable
    {
        public Form _mainForm;

        //Need to modulate these sources, create a new class.
        protected internal DataSource _origSource, _uncompSource;
        protected internal DataSource _replSrc, _replUncompSrc;

        protected internal bool _changed, _merged, _disposed;
        protected internal CompressionType _compression;

        public string _name, _origPath;


        public ResourceNode _parent;
        public List<ResourceNode> _children = new List<ResourceNode>();

        public int _calcSize;

        protected internal ResourceNode _first, _last;
        protected internal bool _hasChildren;

        public event SelectEventHandler SelectChild;
        public event EventHandler UpdateProps, UpdateControl;
        public event MoveEventHandler MovedUp, MovedDown;
        public event ResourceEventHandler Disposing, Renamed, PropertyChanged, Replaced, Restored;
        public event ResourceChildEventHandler ChildAdded, ChildRemoved;
        public event ResourceChildInsertEventHandler ChildInserted;

        #region Properties

#if !DEBUG
        [Browsable(false)]
#endif
        public string FilePath => _origPath;
#if !DEBUG
        [Browsable(false)]
#endif
        public string FileName => Path.GetFileName(_origPath);
#if !DEBUG
        [Browsable(false)]
#endif
        public string DirectoryName => Path.GetDirectoryName(_origPath);

#if !DEBUG
        [Browsable(false)]
#endif
        public ResourceNode RootNode =>
            _parent == null || _parent == this || _parent is FolderNode ? this : _parent.RootNode;

        [Browsable(false)] public DataSource OriginalSource => _origSource;
        [Browsable(false)] public DataSource UncompressedSource => _uncompSource;
        [Browsable(false)] public DataSource WorkingSource => _replSrc != DataSource.Empty ? _replSrc : _origSource;

        [Browsable(false)]
        public DataSource WorkingUncompressed => _replUncompSrc != DataSource.Empty ? _replUncompSrc : _uncompSource;

        [Browsable(false)] public virtual bool HasChildren => _children == null || _children.Count != 0;

#if DEBUG
        [Category("DEBUG")]
        [Browsable(true)]
#else
        [Browsable(false)]
#endif
        public virtual ResourceType ResourceFileType => ResourceType.Unknown;

        public string NodeType => GetType().ToString();

        [Browsable(false)]
        public virtual string TreePathAbsolute => _parent == null ? Name : _parent.TreePathAbsolute + "/" + Name;

        [Browsable(false)]
        public virtual string TreePath
        {
            get
            {
                string path = TreePathAbsolute;
                int index = path.IndexOf('/');
                if (index > 0)
                {
                    path = path.Substring(index + 1);
                }

                return path;
            }
        }

        [Browsable(false)] public virtual int Level => _parent == null ? 0 : _parent.Level + 1;
        [Browsable(false)] public virtual int MaxNameLength => 255;
        [Browsable(false)] public virtual bool AllowDuplicateNames => false;
        [Browsable(false)] public virtual bool AllowNullNames => false;

        [Browsable(false)]
        public virtual string Name
        {
            get => string.IsNullOrEmpty(_name) ? _name = "<null>" : _name;
            set
            {
                if (_name == value)
                {
                    return;
                }

                _name = value;
                _changed = true;
                OnRenamed();
            }
        }

        public void OnRenamed()
        {
            Renamed?.Invoke(this);
        }

        [Browsable(false)]
        public ResourceNode Parent
        {
            get => _parent;
            set
            {
                if (_parent == value)
                {
                    return;
                }

                Remove();
                _parent = value;
                _parent?.Children.Add(this);
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
                    {
                        OnPopulate();
                    }
                }

                return _children;
            }
        }

        /// <summary>
        ///     Used primarily to get bone lists. Kept for all resource nodes for utility.
        /// </summary>
        public List<ResourceNode> GetChildrenRecursive()
        {
            List<ResourceNode> childrenAndSubchildren = new List<ResourceNode>();
            if (Children == null)
            {
                Populate();
            }

            if (Children != null)
            {
                for (int i = 0; i < Children.Count; i++) //ResourceNode r in Children)
                {
                    childrenAndSubchildren.Add(Children[i]);
                    childrenAndSubchildren.AddRange(Children[i].GetChildrenRecursive());
                }
            }

            return childrenAndSubchildren;
        }

        public int Index => _parent == null ? -1 : _parent.Children.IndexOf(this);
        [TypeConverter(typeof(HexIntConverter))] [DisplayName("Index (Hex)")] public int HexIndex => Index;
        [Browsable(false)] public bool IsCompressed => _compression != CompressionType.None;

        //Properties or compression have changed
        [Browsable(false)]
        public bool HasChanged
        {
            get => _changed;
            set => _changed = value;
        }

        public virtual void SignalPropertyChange()
        {
            PropertyChanged?.Invoke(this);

            _changed = true;
        }

        //Has the node deviated from its parent?
        [Browsable(false)] public bool IsBranch => _replSrc.Map != null;

        [Browsable(false)] public bool HasMerged => _merged;

        [Category("Saving")]
        [Description("If false, the node and its children will not be allowed to be rebuilt on save")]
        public virtual bool AllowSaving
        {
            get => _allowSave && (Parent?.AllowSaving ?? true);
            set => _allowSave = value;
        }

        private bool _allowSave = true;

        //Can be any of the following: children have branched, children have changed, current has changed
        //Node needs to be rebuilt.
#if DEBUG
        [Browsable(true)]
        [Category("DEBUG")]
#else
        [Browsable(false)]
#endif
        public virtual bool IsDirty
        {
            get
            {
                if (!AllowSaving)
                {
                    return false;
                }

                if (HasChanged)
                {
                    return true;
                }

                if (_children != null)
                {
                    foreach (ResourceNode n in _children)
                    {
                        if (n.HasChanged || n.IsBranch || n.IsDirty)
                        {
                            return true;
                        }
                    }
                }

                return false;
            }
            set
            {
                _changed = value;
                if (_children != null)
                {
                    foreach (ResourceNode r in Children)
                    {
                        if (r._children != null)
                        {
                            r.IsDirty = value;
                        }
                        else
                        {
                            r._changed = value;
                        }
                    }
                }
            }
        }


        [DisplayName("Uncompressed Size (Bytes)")]
        [Description("For stability, this value is only updated on save.")]
        public virtual uint UncompressedSize => (uint) WorkingUncompressed.Length;

        [Browsable(false)] public virtual Type[] AllowedChildTypes => _allowedChildTypes;
        private readonly Type[] _allowedChildTypes = new Type[] { };

        [Browsable(false)]
        [TypeConverter(typeof(DropDownListCompression))]
        public virtual string Compression
        {
            get => _compression.ToString();
            set
            {
                if (Enum.TryParse(value, out CompressionType type))
                {
                    if (type == _compression)
                    {
                        return;
                    }

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

        ~ResourceNode()
        {
            Dispose();
        }

        public virtual void Dispose()
        {
            if (_disposed)
            {
                return;
            }

            _disposed = true;

            Disposing?.Invoke(this);

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
                {
                    node.Dispose();
                }

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
            SelectChild?.Invoke(index);
        }

        public void UpdateProperties()
        {
            UpdateProps?.Invoke(this, null);
        }

        public void UpdateCurrentControl()
        {
            UpdateControl?.Invoke(this, null);
        }

        #region Moving

        public virtual bool MoveUp()
        {
            if (Parent == null)
            {
                return false;
            }

            int index = Index - 1;
            if (index < 0)
            {
                return false;
            }

            Parent.Children.Remove(this);
            Parent.Children.Insert(index, this);
            Parent._changed = true;
            return true;
        }

        public virtual bool MoveDown()
        {
            if (Parent == null)
            {
                return false;
            }

            int index = Index + 1;
            if (index >= Parent.Children.Count)
            {
                return false;
            }

            Parent.Children.Remove(this);
            Parent.Children.Insert(index, this);
            Parent._changed = true;
            return true;
        }

        public virtual void OnMoved()
        {
        }

        public virtual void DoMoveDown()
        {
            DoMoveDown(true);
        }

        public virtual void DoMoveDown(bool select)
        {
            if (MovedDown != null)
            {
                MovedDown(this, select);
            }
            else
            {
                MoveDown();
            }
        }

        public virtual void DoMoveUp()
        {
            DoMoveUp(true);
        }

        public virtual void DoMoveUp(bool select)
        {
            if (MovedUp != null)
            {
                MovedUp(this, select);
            }
            else
            {
                MoveUp();
            }
        }

        public virtual bool AddUp()
        {
            int index = Index - 1;
            if (index < 0)
            {
                return false;
            }

            if (Parent.Children[index] is MDL0BoneNode)
            {
                return true;
            }

            return false;
        }

        public virtual bool AddDown()
        {
            int index = Index + 1;
            if (index >= Parent.Children.Count)
            {
                return false;
            }

            if (Parent.Children[index] is MDL0BoneNode)
            {
                return true;
            }

            return false;
        }

        public virtual bool ToParent()
        {
            if (Parent is MDL0BoneNode)
            {
                return true;
            }

            return false;
        }

        #endregion

        #region Child Population

        public bool _isPopulating;

        public void Populate(int levels = -1)
        {
            _isPopulating = true;
            if (levels > 0)
            {
                foreach (ResourceNode r in Children)
                {
                    r.Populate(levels - 1);
                }
            }
            else if (levels < 0)
            {
                for (int i = 0; i < Children.Count; i++)
                {
                    Children[i].Populate();
                }
            }
            else if (_children == null || _children.Count == 0)
            {
                _children = new List<ResourceNode>();
                if (WorkingSource != DataSource.Empty)
                {
                    OnPopulate();
                }
            }

            _isPopulating = false;
        }

        //Called when children are first requested. Allows node to cache child nodes.
        public virtual void OnPopulate()
        {
        }

        #endregion

        #region Initialization

        public void Initialize(ResourceNode parent, FileMap source)
        {
            Initialize(parent, new DataSource(source));
        }

        public void Initialize(ResourceNode parent, VoidPtr address, int length)
        {
            Initialize(parent, new DataSource(address, length));
        }

        public void Initialize(ResourceNode parent, DataSource origSource)
        {
            Initialize(parent, origSource, origSource);
        }

        public virtual void Initialize(ResourceNode parent, DataSource origSource, DataSource uncompSource)
        {
            _origSource = origSource;
            _uncompSource = uncompSource;
            _compression = _origSource.Compression;

            if (origSource.Map != null)
            {
                _origPath = origSource.Map.FilePath;
            }

            if (_parent != parent)
            {
                _parent = parent;
                _parent?.Children.Add(this);
            }
            
            _children = null;

            if (Parent != null && Parent._replaced)
            {
                _replaced = true;
            }

            if (!OnInitialize())
            {
                _children = new List<ResourceNode>();
            }

            _replaced = false;
        }

        //Called when property values are requested. Allows node to cache values from source data.
        //Return true to indicate there are child nodes.
        public virtual bool OnInitialize()
        {
            return false;
        }

        //Restores node to its original form using the backing tree. 
        public virtual void Restore()
        {
            if (!IsDirty && !IsBranch)
            {
                return;
            }

            if (_children != null)
            {
                foreach (ResourceNode node in _children)
                {
                    node.Dispose();
                }

                _children.Clear();
                _children = null;
            }

            _replUncompSrc.Close();
            _replSrc.Close();
            _compression = _origSource.Compression;

            if (_origSource != DataSource.Empty && !OnInitialize())
            {
                _children = new List<ResourceNode>();
            }

            _changed = false;
            Restored?.Invoke(this);
        }

        #endregion

        #region Adding/Removing

        public virtual void Remove()
        {
            _parent?.RemoveChild(this);
        }

        public virtual void RemoveChild(ResourceNode child)
        {
            if (_children != null && _children.Remove(child))
            {
                child._parent = null;
                ChildRemoved?.Invoke(this, child);

                _changed = true;
            }
        }

        public virtual void AddChild(ResourceNode child)
        {
            AddChild(child, true);
        }

        public virtual void AddChild(ResourceNode child, bool change)
        {
            Children.Add(child);
            child._parent = this;
            ChildAdded?.Invoke(this, child);

            if (change)
            {
                _changed = true;
            }
        }

        public virtual void InsertChild(ResourceNode child, int index)
        {
            InsertChild(child, true, index);
        }

        public virtual void InsertChild(ResourceNode child, bool change, int index)
        {
            if (index < 0 || index >= Children.Count)
            {
                AddChild(child, change);
                return;
            }

            Children.Insert(index, child);
            child._parent = this;
            ChildInserted?.Invoke(index, this, child);

            if (change)
            {
                _changed = true;
            }
        }

        #endregion

        public void SetSizeInternal(int size)
        {
            if (IsBranch)
            {
                if (IsCompressed)
                {
                    _replUncompSrc.Length = size;
                }
                else
                {
                    _replSrc.Length = _replUncompSrc.Length = size;
                }
            }
            else if (IsCompressed)
            {
                _uncompSource.Length = size;
            }
            else
            {
                _origSource.Length = _uncompSource.Length = size;
            }
        }

        #region Replacing

        //Causes a deviation in the resource tree. This node and all child nodes will be backed by a temporary file until the tree is merged.
        //Causes parent node(s) to become dirty.
        //Replace will reference the file in a new DataSource.
        public bool _replaced;

        public virtual void Replace(string fileName)
        {
            Replace(fileName, FileMapProtect.Read, FileOptions.SequentialScan);
        }

        public virtual void Replace(ResourceNode node)
        {
            string path = Path.GetTempFileName();
            node.Export(path);
            Replace(path);
        }

        public virtual void Replace(string fileName, FileMapProtect prot, FileOptions options)
        {
            //Name = Path.GetFileNameWithoutExtension(fileName);
            ReplaceRaw(FileMap.FromFile(fileName, prot, 0, 0, options));
        }

        public virtual void ReplaceRaw(VoidPtr address, int length)
        {
            FileMap map = FileMap.FromTempFile(length);
            Memory.Move(map.Address, address, (uint) length);
            ReplaceRaw(map);
        }

        [Browsable(false)] public virtual bool RetainChildrenOnReplace => false;

        [Browsable(false)] public virtual bool supportsCompression => true;

        public virtual void ReplaceRaw(FileMap map)
        {
            if (_children != null && !RetainChildrenOnReplace)
            {
                foreach (ResourceNode node in _children)
                {
                    node.Dispose();
                }

                _children.Clear();
                _children = null;
            }

            _replUncompSrc.Close();
            _replSrc.Close();

            _replSrc = new DataSource(map);

            if (supportsCompression)
            {
                FileMap uncompMap = Compressor.TryExpand(ref _replSrc, false);
                _compression = _replSrc.Compression;
                _replUncompSrc = uncompMap != null ? new DataSource(uncompMap) : _replSrc;
            }
            else
            {
                _compression = CompressionType.None;
                _replUncompSrc = _replSrc;
            }

            _replaced = true;
            if (!OnInitialize() && !RetainChildrenOnReplace)
            {
                _children = new List<ResourceNode>();
            }

            _replaced = false;

            _changed = false;
            Replaced?.Invoke(this);
        }

        protected void ForceReplacedEvent()
        {
            Replaced?.Invoke(this);
        }

        #endregion

        #region Export

        public virtual void Export(string outPath)
        {
            Rebuild(); //Apply changes the user has made by rebuilding.
#if !DEBUG
            try
            {
#endif
            using (FileStream stream = new FileStream(outPath, FileMode.OpenOrCreate, FileAccess.ReadWrite,
                FileShare.ReadWrite, 8, FileOptions.SequentialScan))
            {
                Export(stream);
            }
#if !DEBUG
            }
            catch
            {
                MessageBox.Show("Unable to open file for write access.");
                IsDirty = true;
            }
#endif
        }

        public void Export(FileStream outStream)
        {
            if (WorkingSource.Length != 0)
            {
                outStream.SetLength(WorkingSource.Length);
                using (FileMap map = FileMap.FromStream(outStream))
                {
                    Memory.Move(map.Address, WorkingSource.Address, (uint) WorkingSource.Length);
                }
            }
            else
            {
                MessageBox.Show("Data was empty!");
            }
        }

        public virtual void ExportUncompressed(string outPath)
        {
            Rebuild(); //Apply changes the user has made by rebuilding.
            try
            {
                using (FileStream stream = new FileStream(outPath, FileMode.OpenOrCreate, FileAccess.ReadWrite,
                    FileShare.None, 8, FileOptions.SequentialScan))
                {
                    ExportUncompressed(stream);
                }
            }
            catch
            {
                MessageBox.Show("Unable to open file for write access.");
            }
        }

        public void ExportUncompressed(FileStream outStream)
        {
            if (WorkingUncompressed.Length != 0)
            {
                outStream.SetLength(WorkingUncompressed.Length);
                using (FileMap map = FileMap.FromStream(outStream))
                {
                    Memory.Move(map.Address, WorkingUncompressed.Address, (uint) WorkingUncompressed.Length);
                }
            }
            else
            {
                MessageBox.Show("Data was empty!");
            }
        }

        #endregion

        #region Rebuilding

        //Combines node and children into single (temp) file map.
        //Does nothing if node is not dirty or rebuild is not forced.
        //Calls OnCalculateSize on self, which will allow the node to cache any values for OnRebuild
        public virtual void Rebuild()
        {
            Rebuild(false);
        }

        public virtual void Rebuild(bool force)
        {
            if (!IsDirty && !force)
            {
                return;
            }

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
                FileStream stream = new FileStream(Path.GetTempFileName(), FileMode.Open, FileAccess.ReadWrite,
                    FileShare.None, 0x8, FileOptions.DeleteOnClose | FileOptions.SequentialScan);
                try
                {
                    Compressor.Compact(_compression, uncompMap.Address, uncompMap.Length, stream, this);
                    _replSrc = new DataSource(
                        FileMap.FromStreamInternal(stream, FileMapProtect.Read, 0, (int) stream.Length), _compression);
                }
                catch (Exception)
                {
                    stream.Dispose();
                    throw;
                }
            }
        }

        //Called on child nodes in order to rebuild them at a specified address.
        //This will occur after CalculateSize, so compressed nodes will already be rebuilt.
        public virtual void Rebuild(VoidPtr address, int length, bool force)
        {
            if (!IsDirty && !force)
            {
                MoveRaw(address, length);
            }
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
            Memory.Move(address, WorkingSource.Address, (uint) length);
            DataSource newsrc = new DataSource(address, length);
            if (_compression == CompressionType.None)
            {
                if (_children != null)
                {
                    int offset = address - WorkingSource.Address;
                    foreach (ResourceNode n in _children)
                    {
                        n.OnParentMoved(offset);
                    }
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
                {
                    _replSrc.Address = _replUncompSrc.Address += offset;
                }
                else if (_origSource != DataSource.Empty)
                {
                    _origSource.Address = _uncompSource.Address += offset;
                }

                if (_children != null)
                {
                    foreach (ResourceNode n in _children)
                    {
                        n.OnParentMoved(offset);
                    }
                }
            }
            else
            {
                if (_replSrc != DataSource.Empty)
                {
                    _replSrc.Address += offset;
                }
                else if (_origSource != DataSource.Empty)
                {
                    _origSource.Address += offset;
                }
            }
        }

        internal virtual void MoveRawUncompressed(VoidPtr address, int length)
        {
            Memory.Move(address, WorkingUncompressed.Address, (uint) length);
            DataSource newsrc = new DataSource(address, length);

            int offset = address - WorkingUncompressed.Address;
            foreach (ResourceNode n in Children)
            {
                n.OnParentMovedUncompressed(offset);
            }

            _replUncompSrc.Close();
            _replUncompSrc = newsrc;
        }

        internal virtual void OnParentMovedUncompressed(int offset)
        {
            if (_replUncompSrc != DataSource.Empty)
            {
                _replUncompSrc.Address += offset;
            }
            else if (_uncompSource != DataSource.Empty)
            {
                _uncompSource.Address += offset;
            }

            if (_children != null)
            {
                foreach (ResourceNode n in _children)
                {
                    n.OnParentMovedUncompressed(offset);
                }
            }
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
                {
                    return _calcSize = OnCalculateSize(force);
                }

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
        public void Merge()
        {
            Merge(false);
        }

        public void Merge(bool forceBuild)
        {
            if (_parent != null)
            {
                throw new InvalidOperationException("Merge can only be called on the root node!");
            }

            if (forceBuild || IsDirty)
            {
                Rebuild(forceBuild);
            }

            MergeInternal();
            _merged = true;
        }

        //Swap data sources to only use new temp file. Closes original sources.
        protected virtual void MergeInternal()
        {
            if (_children != null)
            {
                foreach (ResourceNode n in Children)
                {
                    n.MergeInternal();
                }
            }

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

        public static ResourceNode[] FindAllSubNodes(ResourceNode root)
        {
            List<ResourceNode> resourceNodes = new List<ResourceNode>();
            if (root != null)
            {
                resourceNodes.Add(root);
                if (root.HasChildren)
                {
                    foreach (ResourceNode r in root.Children)
                    {
                        resourceNodes.AddRange(FindAllSubNodes(r));
                    }
                }
            }

            return resourceNodes.ToArray();
        }

        public static ResourceNode FindNode(ResourceNode root, string path, bool searchChildren,
                                            StringComparison compare)
        {
            if (string.IsNullOrEmpty(path) || root == null ||
                root.Name.Equals(path, StringComparison.OrdinalIgnoreCase))
            {
                return root;
            }

            if (path.Contains("/") && path.Substring(0, path.IndexOf('/'))
                .Equals(root.Name, compare))
            {
                return root.FindChild(path.Substring(path.IndexOf('/') + 1), searchChildren, compare);
            }

            return root.FindChild(path, searchChildren, compare);
        }

        public ResourceNode FindChildByType(string path, bool searchChildren, params ResourceType[] types)
        {
            return FindChildByType(path, searchChildren, StringComparison.Ordinal, types);
        }

        public ResourceNode FindChildByType(string path, bool searchChildren, StringComparison compare,
                                            params ResourceType[] types)
        {
            if (path == null)
            {
                return null;
            }

            if (types.Contains(ResourceType.TEX0) && !types.Contains(ResourceType.SharedTEX0))
            {
                List<ResourceType> t = types.ToList();
                t.Add(ResourceType.SharedTEX0);
                types = t.ToArray();
            }

            ResourceNode node = null;

            if (path.Contains("/"))
            {
                string next = path.Substring(0, path.IndexOf('/'));
                foreach (ResourceNode n in Children)
                {
                    if (n.Name != null && n.Name.Equals(next, compare))
                    {
                        if ((node = FindNode(n, path.Substring(next.Length + 1), searchChildren, compare)) != null &&
                            types.Any(t => t == node.ResourceFileType))
                        {
                            return node;
                        }
                    }
                }
            }
            else
            {
                //Search direct children first
                foreach (ResourceNode n in Children)
                {
                    if (n.Name != null && n.Name.Equals(path, compare) &&
                        types.Any(t => t == n.ResourceFileType))
                    {
                        return n;
                    }
                }
            }

            if (searchChildren)
            {
                foreach (ResourceNode n in Children)
                {
                    if ((node = n.FindChildByType(path, true, compare, types)) != null &&
                        types.Any(t => t == node.ResourceFileType))
                    {
                        return node;
                    }
                }
            }

            return null;
        }

        public ResourceNode FindChild(string path)
        {
            return FindChild(path, false);
        }

        public ResourceNode FindChild(string path, bool searchChildren)
        {
            return FindChild(path, searchChildren, StringComparison.OrdinalIgnoreCase);
        }

        public ResourceNode FindChild(string path, StringComparison compare)
        {
            return FindChild(path, false, compare);
        }

        public ResourceNode FindChild(string path, bool searchChildren, StringComparison compare)
        {
            ResourceNode node = null;
            if (path == null)
            {
                return null;
            }

            if (path.Contains("/"))
            {
                string next = path.Substring(0, path.IndexOf('/'));
                foreach (ResourceNode n in Children)
                {
                    if (n.Name != null && n.Name.Equals(next, compare))
                    {
                        if ((node = FindNode(n, path.Substring(next.Length + 1), searchChildren, compare)) != null)
                        {
                            return node;
                        }
                    }
                }
            }
            else
            {
                //Search direct children first
                foreach (ResourceNode n in Children)
                {
                    if (n.Name != null && n.Name.Equals(path, compare))
                    {
                        return n;
                    }
                }
            }

            if (searchChildren)
            {
                foreach (ResourceNode n in Children.ToArray())
                {
                    if ((node = n.FindChild(path, true, compare)) != null)
                    {
                        return node;
                    }
                }
            }

            return null;
        }

        public ResourceNode[] FindChildrenByClassType(string path, Type type)
        {
            if (!string.IsNullOrEmpty(path))
            {
                ResourceNode node = FindChild(path, false);
                if (node != null)
                {
                    return node.FindChildrenByClassType(null, type);
                }
            }

            List<ResourceNode> nodes = new List<ResourceNode>();
            EnumClassTypeInternal(nodes, type);
            return nodes.ToArray();
        }

        private void EnumClassTypeInternal(List<ResourceNode> list, Type type)
        {
            if (GetType() == type)
            {
                list.Add(this);
            }

            foreach (ResourceNode n in Children)
            {
                n.EnumClassTypeInternal(list, type);
            }
        }

        public ResourceNode[] FindChildrenByType(string path, ResourceType type)
        {
            if (!string.IsNullOrEmpty(path))
            {
                ResourceNode node = FindChild(path, false);
                if (node != null)
                {
                    return node.FindChildrenByType(null, type);
                }
            }

            List<ResourceNode> nodes = new List<ResourceNode>();
            EnumTypeInternal(nodes, type);
            return nodes.ToArray();
        }

        // Currently only works for MDL0
        public ResourceNode[] FindChildrenByTypeInGroup(string path, ResourceType type, byte group)
        {
            if (!string.IsNullOrEmpty(path))
            {
                ResourceNode node = FindChild(path, false);
                if (node != null)
                {
                    if (!(node is ARCEntryNode && ((ARCEntryNode) node).GroupID != group))
                    {
                        return node.FindChildrenByTypeInGroup(null, type, group);
                    }
                }
            }

            List<ResourceNode> nodes = new List<ResourceNode>();
            ResourceNode attemptedArc = null;
            EnumTypeInternal(nodes, type);
            if (nodes[0] is BRESEntryNode)
            {
                attemptedArc = ((BRESEntryNode) nodes[0]).BRESNode.Parent;
            }
            else if (nodes[0] is ARCEntryNode)
            {
                attemptedArc = nodes[0].Parent;
            }

            try
            {
                if (this is ARCNode)
                {
                    attemptedArc = this;
                }
                else if (nodes[0] is BRESEntryNode)
                {
                    attemptedArc = ((BRESEntryNode) nodes[0]).BRESNode.Parent;
                }
                else if (nodes[0] is ARCEntryNode)
                {
                    attemptedArc = nodes[0].Parent;
                }

                nodes = new List<ResourceNode>();
                if (attemptedArc != null && type == ResourceType.MDL0)
                {
                    foreach (ARCEntryNode a in attemptedArc.Children)
                    {
                        if (a.GroupID == group)
                        {
                            if (a is BRRESNode)
                            {
                                foreach (MDL0Node m in ((BRRESNode) a)?.GetFolder<MDL0Node>()?.Children ?? new List<ResourceNode>())
                                {
                                    nodes.Add(m);
                                }
                            }
                            else if (a.RedirectNode != null)
                            {
                                try
                                {
                                    ARCEntryNode tempBres = a.RedirectNode as ARCEntryNode;
                                    RedirectStart:
                                    if (tempBres.GroupID != group)
                                    {
                                        if (tempBres.RedirectNode != null)
                                        {
                                            tempBres = tempBres.RedirectNode as ARCEntryNode;
                                            goto RedirectStart;
                                        }
                                        else if (tempBres is BRRESNode)
                                        {
                                            foreach (MDL0Node m in ((BRRESNode) tempBres)?.GetFolder<MDL0Node>()?.Children ?? new List<ResourceNode>())
                                            {
                                                nodes.Add(m);
                                            }
                                        }
                                    }
                                }
                                catch
                                {
                                    // ignored
                                }
                            }
                        }
                    }
                }
            }
            catch
            {
                // ignored
            }

            return nodes.ToArray();
        }

        private void EnumTypeInternal(List<ResourceNode> list, ResourceType type)
        {
            if (ResourceFileType == type)
            {
                list.Add(this);
            }

            foreach (ResourceNode n in Children)
            {
                n.EnumTypeInternal(list, type);
            }
        }

        public ResourceNode[] FindChildrenByName(string name)
        {
            List<ResourceNode> nodes = new List<ResourceNode>();
            EnumNameInternal(nodes, name);
            return nodes.ToArray();
        }

        private void EnumNameInternal(List<ResourceNode> list, string name)
        {
            if (Name == name)
            {
                list.Add(this);
            }

            foreach (ResourceNode n in Children)
            {
                n.EnumNameInternal(list, name);
            }
        }

        public unsafe string FindName(string name)
        {
            int index = -1;

            if (string.IsNullOrEmpty(name))
            {
                name = "NewNode";
            }

            int len = name.Length;
            sbyte* charList = stackalloc sbyte[len + 3];
            PString pStr = charList;

            for (int i = 0; i < len; i++)
            {
                charList[i] = (sbyte) name[i];
            }

            Top:

            if (index < 0)
            {
                charList[len] = 0;
            }
            else
            {
                charList[len] = (sbyte) ((index % 10) | 0x30);
                if (index < 10)
                {
                    charList[len + 1] = 0;
                }
                else
                {
                    charList[len + 1] = (sbyte) ((index / 10) | 0x30);
                    charList[len + 2] = 0;
                }
            }

            index++;
            foreach (ResourceNode node in Children)
            {
                if (pStr == node.Name)
                {
                    goto Top;
                }
            }

            return new string(charList);
        }

        public ResourceNode FindEmbeddedIndex(int index)
        {
            int count = -1;
            return FindEmbeddedInternal(this, index, ref count);
        }

        private static ResourceNode FindEmbeddedInternal(ResourceNode node, int index, ref int count)
        {
            if (count++ >= index)
            {
                return node;
            }

            foreach (ResourceNode n in node.Children)
            {
                if ((node = FindEmbeddedInternal(n, index, ref count)) != null)
                {
                    return node;
                }
            }

            return null;
        }

        #endregion

        #region MD5

        private static MD5CryptoServiceProvider _md5provider;

        protected static MD5CryptoServiceProvider MD5Provider
        {
            get
            {
                if (_md5provider == null)
                {
                    _md5provider = new MD5CryptoServiceProvider();
                }

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
            DataSource data = OriginalSource;
            if (data.Address == null || data.Length == 0)
            {
                return null;
            }

            UnmanagedMemoryStream stream = new UnmanagedMemoryStream((byte*) data.Address, data.Length);
            return MD5Provider.ComputeHash(stream);
        }

        /// <summary>
        /// Get the result of the MD5() function as a string of hexadecimal digits.
        /// If MD5() returns null, this method will return an empty string.
        /// </summary>
        [System.Runtime.ExceptionServices.HandleProcessCorruptedStateExceptions]
        public string MD5Str()
        {
            try
            {
                byte[] checksum = MD5();
                if (checksum == null)
                {
                    return string.Empty;
                }

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

        public ResourceNode PrevSibling()
        {
            if (_parent == null)
            {
                return null;
            }

            int siblingIndex = Index - 1;
            if (siblingIndex < 0)
            {
                return null;
            }

            return Parent.Children[siblingIndex];
        }

        public ResourceNode NextSibling()
        {
            if (_parent == null)
            {
                return null;
            }

            int siblingIndex = Index + 1;
            if (siblingIndex >= Parent.Children.Count)
            {
                return null;
            }

            return Parent.Children[siblingIndex];
        }

        public override string ToString()
        {
            return Name;
        }

        public virtual void SortChildren()
        {
            if (Children == null || Children.Count <= 0)
            {
                return;
            }

            _children = _children.OrderBy(o => o.Name).ToList();
            SignalPropertyChange();
        }
    }

    public class NodeComparer : IComparer<ResourceNode>
    {
        public static NodeComparer Instance = new NodeComparer();

        public int Compare(ResourceNode x, ResourceNode y)
        {
            return string.CompareOrdinal(x.Name, y.Name);
        }
    }
}