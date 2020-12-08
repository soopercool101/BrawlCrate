using BrawlLib.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace BrawlLib.SSBB.ResourceNodes
{
    public abstract class SakuraiEntryNode
    {
        public string EntryOffset => "0x" + _offset.ToString("X");
        public string EntrySize => "0x" + _initSize.ToString("X");

        [Browsable(false)]
        public int RebuildOffset => RebuildAddress == null
                                    || BaseAddress == null
                                    || RebuildAddress < BaseAddress
            ? -1
            : Offset(RebuildAddress);

        [Browsable(false)] public VoidPtr BaseAddress => _root.BaseAddress;
        [Browsable(false)] public bool External => _externalEntry != null;

        [Browsable(false)]
        public bool HasChanged
        {
            get => _root != null && _root.ChangedEntries.Contains(this);
            set
            {
                if (_root != null)
                {
                    if (value)
                    {
                        if (!_root.ChangedEntries.Contains(this))
                        {
                            _root.ChangedEntries.Add(this);
                        }
                    }
                    else
                    {
                        if (_root.ChangedEntries.Contains(this))
                        {
                            _root.ChangedEntries.Remove(this);
                        }
                    }
                }
            }
        }

        [Browsable(false)] public virtual string Name => _name;

        /// <summary>
        /// This is where the data for this node was written during the last rebuild.
        /// Don't forget to set this when rebuilding a node!
        /// </summary>
        [Browsable(false)]
        public VoidPtr RebuildAddress
        {
            get => _rebuildAddress;
            set
            {
                if (_root.IsRebuilding)
                {
                    _rebuildAddress = value;
                }
#if DEBUG
                else
                {
                    throw new Exception("Can't set rebuild address when the file isn't being rebuilt.");
                }
#endif
            }
        }

        [Browsable(false)]
        public virtual bool IsDirty
        {
            get => _root.ChangedEntries.Contains(this);
            set => HasChanged = value;
        }

        [Browsable(false)] public virtual int Index => _index;
        [Browsable(false)] public int TotalSize => _entryLength + _childLength;

        public string _name;
        public SakuraiEntryNode _parent;
        public SakuraiArchiveNode _root;

        public int
            _offset,        //The initial offset of this entry when first parsed
            _index,         //The entry's child index when first parsed
            _initSize = -1, //The size of this entry when first parsed.
            _calcSize;      //This size of this entry after GetSize() has been called.

        //Sometimes a section will reference an entry contained in another section.
        //This keeps track of that
        public TableEntryNode _externalEntry;

        private VoidPtr _rebuildAddress = null;
        public int _entryLength, _childLength;

        [Browsable(false)] public int LookupCount => _lookupCount;
        private int _lookupCount;

        private List<VoidPtr> _lookupAddresses;

        //Functions
        /// <summary>
        /// Call this when an entry's size changes
        /// </summary>
        public void SignalRebuildChange()
        {
            _root?.RebuildEntries.Add(this);

            HasChanged = true;
        }

        /// <summary>
        /// Call this when a property has been changed but the size remains the same
        /// </summary>
        public void SignalPropertyChange()
        {
            HasChanged = true;
        }

        /// <summary>
        /// Returns an offset of the given address relative to the base address.
        /// </summary>
        public int Offset(VoidPtr address)
        {
            return _root.Offset(address);
        }

        /// <summary>
        /// Returns an address of the given offset relative to the base address.
        /// </summary>
        public VoidPtr Address(int offset)
        {
            return BaseAddress + offset;
        }

        /// <summary>
        /// Returns the size of the entry at the given offset.
        /// </summary>
        public int GetSize(int offset)
        {
            return _root.GetSize(offset);
        }

        /// <summary>
        /// Use this to parse a node of a specific type at the given offset.
        /// This will automatically add the node to the entry cache, get its size,
        /// set its offset value, and attach its external entry if it has one.
        /// Be sure to send the proper constructor parameters for the given type
        /// as well, or an error will be thrown.
        /// </summary>
        public T Parse<T>(int offset, params object[] parameters) where T : SakuraiEntryNode
        {
            return CommonInit<T>(_root, this, Address(offset), parameters);
        }

        /// <summary>
        /// Use this to parse a node of a specific type at the given address.
        /// This will automatically add the node to the entry cache, get its size,
        /// set its offset value, and attach its external entry if it has one.
        /// Be sure to send the proper constructor parameters for the given type
        /// as well, or an error will be thrown.
        /// </summary>
        public T Parse<T>(VoidPtr address, params object[] parameters) where T : SakuraiEntryNode
        {
            return CommonInit<T>(_root, this, address, parameters);
        }

        /// <summary>
        /// Use this to parse a node of a specific type at the given address.
        /// This will automatically add the node to the entry cache, get its size,
        /// set its offset value, and attach its external entry if it has one.
        /// Be sure to send the proper constructor parameters for the given type
        /// as well, or an error will be thrown.
        /// </summary>
        public static T Parse<T>(SakuraiArchiveNode root, SakuraiEntryNode parent, VoidPtr address,
                                 params object[] parameters) where T : SakuraiEntryNode
        {
            return CommonInit<T>(root, parent, address, parameters);
        }

        /// <summary>
        /// Don't call this outside of the Parse functions. 
        /// This is here to eliminate redundant code.
        /// </summary>
        private static T CommonInit<T>(SakuraiArchiveNode root, SakuraiEntryNode parent, VoidPtr addr,
                                       params object[] parameters) where T : SakuraiEntryNode
        {
            int offset = root.Offset(addr);
            bool attributes = parameters.Contains("Attributes");
            if (offset <= 0 && !attributes)
            {
                return null;
            }

            if (attributes)
            {
                parameters = new object[0];
            }

            T n = Activator.CreateInstance(typeof(T), parameters) as T;
            n.Setup(root, parent, offset);
            n.OnParse(addr);
            return n;
        }

        public void ParseSelf(SakuraiArchiveNode root, SakuraiEntryNode parent, int offset)
        {
            Setup(root, parent, offset);
            OnParse(Address(offset));
        }

        public void ParseSelf(SakuraiArchiveNode root, SakuraiEntryNode parent, VoidPtr address)
        {
            Setup(root, parent, Offset(address));
            OnParse(address);
        }

        private void Setup(SakuraiArchiveNode node, SakuraiEntryNode parent, int offset)
        {
            Setup(node, parent, offset, null);
        }

        private void Setup(SakuraiArchiveNode node, SakuraiEntryNode parent, int offset, string name)
        {
            _name = name;
            _root = node;
            _offset = offset;
            _parent = parent;
            if (_initSize <= 0)
            {
                _initSize = _root.GetSize(_offset);
            }

            _root.EntryCache[_offset] = this;
            if ((_externalEntry = _root.TryGetExternal(offset)) != null)
            {
                _externalEntry.References.Add(this);
            }
        }

        public int GetSize()
        {
            _entryLength = 0;
            _childLength = 0;
            return _calcSize = OnGetSize();
        }

        /// <summary>
        /// Writes this node's data at the given address.
        /// Because most entries write their children before their header,
        /// this returns the offset of the header.
        /// Also resets the lookup count for the next rebuild.
        /// </summary>
        public int Write(VoidPtr address)
        {
            if (External)
            {
                throw new Exception("Trying to write an external data entry inside of a section's child data!");
            }

            //Reset list of lookup offsets
            //Addresses will be added in OnWrite.
            _lookupAddresses = new List<VoidPtr>();

            //Reset the rebuild address to be set in OnWrite
            //Set to 'address' instead? I just don't want to forget to set this 
            //in nodes that don't use the start address
            RebuildAddress = null;

            //Write this node's data to the address.
            //Sets RebuildAddress to the location of the header.
            //The header is often not the first thing written to the given address.
            //Children are always written first.
            OnWrite(address);

#if DEBUG
            if (_lookupAddresses.Count != _lookupCount)
            {
                throw new Exception("Number of actual lookup offsets does not match the calculated count.");
            }

            //We could just set the rebuild address to the address the node was written at by default
            //But I'm going to through an exception anyway just to be sure everything is coded properly
            if (!RebuildAddress)
            {
                throw new Exception("RebuildAddress was not set.");
            }
#endif

            //Reset for next calc size
            _lookupCount = 0;

            //Return the offset to the header
            return RebuildOffset;
        }

        public int GetLookupCount()
        {
            if (_lookupCount == 0)
            {
                _lookupCount = OnGetLookupCount();
            }

            return _lookupCount;
        }

        //Call this function on the addresses of all offsets.
        //DO NOT send the offset itself as the address!
        protected void Lookup(VoidPtr address)
        {
#if DEBUG
            if ((int) address < (int) BaseAddress)
            {
                throw new Exception("Offset value set in lookup, not the address of the offset value.");
            }

            //TODO: check if the added address is within the node's header + data start and end addresses?
#endif

            _lookupAddresses.Add(address);
        }

        protected void Lookup(List<VoidPtr> values)
        {
            _lookupAddresses.AddRange(values);
        }

        [Browsable(false)] public List<VoidPtr> LookupAddresses => _lookupAddresses;

        //Overridable functions
        protected virtual void OnParse(VoidPtr address)
        {
        }

        protected virtual void OnWrite(VoidPtr address)
        {
        }

        protected virtual int OnGetSize()
        {
            return 0;
        }

        protected virtual int OnGetLookupCount()
        {
            return 0;
        }

        protected virtual void PostProcess(LookupManager lookupOffsets)
        {
        }

        public override string ToString()
        {
            return string.IsNullOrEmpty(Name) ? base.ToString() : Name;
        }

        public virtual void PostParse()
        {
        }
    }
}