using BrawlLib.SSBBTypes;
using Ikarus.MovesetFile;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Ikarus.MovesetBuilder
{
    public abstract class BuilderBase
    {
        protected int Offset(VoidPtr address) { return (int)address - (int)_baseAddress; }
        protected VoidPtr Address(int offset) { return _baseAddress + offset; }

        /// <summary>
        /// _currentAddress as an offset from the moveset base address (header + 0x20)
        /// </summary>
        public int CurrentOffset { get { return Offset(_currentAddress); } }

        public BindingList<Script> Subroutines { get { return _moveset.SubRoutines; } }

        //Split size calculations up into sections to make debugging easier
        protected List<int> _lengths = new List<int>();
        List<int> _lookupCounts = new List<int>();
        public List<VoidPtr> _lookupAddresses;

        //Calculated per part, then added to _lookupCounts and _lengths respectively and reset
        int _currentLookupCount = 0, _currentSize = 0;

        //Calculated per part during writing process, then added to the lookup manager and reset
        List<VoidPtr> _currentLookup = new List<VoidPtr>();

        //Subroutines are built as soon as they are referenced by a script
        //This keeps track of which have been written already
        List<int> _builtSubroutines;

        public int _lookupCount;

        //Contains the functions for getting the size of and writing each part
        protected Action[] _getPartSize, _buildPart;
        protected VoidPtr _baseAddress, _currentAddress;
        
        protected MovesetNode _moveset;

        protected void Lookup(VoidPtr address)
        {
#if DEBUG
            if ((int)address < (int)_baseAddress)
                throw new Exception("Offset value set in lookup, not the address of the offset value.");
#endif
            if (_lookupAddresses.Contains(address))
                throw new Exception("Lookup list already contains this address.");

            _currentLookup.Add(address);
        }

        protected void Lookup(List<VoidPtr> values)
        {
            _currentLookup.AddRange(values);
        }

        //This calculates the size of child data only
        //The entry size is calculated in the node's OnGetSize() function
        public virtual int CalcSize()
        {
            _builtSubroutines = new List<int>();
            _lengths = new List<int>();
            _lookupCounts = new List<int>();
            _lookupAddresses = new List<VoidPtr>();

            //Add data sizes in order of appearance in file.
            //This is just so it's easier to keep track of things
            while (_lengths.Count < _getPartSize.Length)
            {
                //Add new starting length for this part
                _currentSize = 0;
                _currentLookupCount = 0;

                //Run the function that will calculate this part's size
                //The size of the part will be added to the last added value in _lengths.
                _getPartSize[_lengths.Count]();

                _lengths.Add(_currentSize);
                _lookupCounts.Add(_currentLookupCount);
            }

            //Add up all part sizes.
            //Individual part sizes are stored in _lengths so they can be referred to later when writing the format
            int totalSize = 0;
            foreach (int len in _lengths)
                totalSize += len;

            _lookupCount = 0;
            foreach (int count in _lookupCounts)
                _lookupCount += count;

            //Return total data size (this does not include the entry's header)
            return totalSize;
        }
        protected void AddSize(int amt)
        {
            if (amt > 0)
                _currentSize += amt;
            else
            {
                if (amt < 0)
                    throw new Exception("Trying to add a negative data size");
                
                //Ignore if the amount is zero, but point it out
                Console.WriteLine("Something has no size");
            }
        }
        protected void IncLookup(int count = 1) { _currentLookupCount += count; }
        /// <summary>
        /// If the entry isn't null,
        /// adds the size of the entry to the current part length
        /// and adds the entry's lookup count to the main lookup count.
        /// the incrementLookup argument will add a lookup offset for the offset to the entry
        /// </summary>
        protected void GetSize(SakuraiEntryNode entry, bool incrementLookup = false)
        {
            if (entry != null)
            {
                _currentSize += entry.GetSize();
                _currentLookupCount += entry.GetLookupCount();

                //Increment lookup for the offset in a header to this data
                if (incrementLookup && entry._calcSize > 0)
                    IncLookup();
            }
        }
        /// <summary>
        /// If the entry isn't null,
        /// adds the size of the entry to the current part length
        /// and adds the entry's lookup count to the main lookup count.
        /// </summary>
        protected bool GetScriptSize(Script script, bool incrementLookup = false)
        {
            //Build if it has data, if it's referenced, or if forced
            if (script.Count > 0 || script._actionRefs.Count > 0 || script._forceBuild)
            {
                //Collect script size
                GetSize(script);

                //Add lookup offset to the offset to this script
                if (incrementLookup)
                    _currentLookupCount++;

                foreach (Event e in script)
                    foreach (Parameter p in e)
                        if (p is EventOffset)
                        {
                            EventOffset o = (EventOffset)p;
                            bool useOffset = o._offsetInfo._list != ListValue.Null && o._offsetInfo._list != ListValue.References;
                            if (useOffset)
                            {
                                //Add offset to the script to lookup
                                IncLookup();

                                if (o._offsetInfo._list == ListValue.SubRoutines)
                                {
                                    int index = Subroutines.IndexOf(o._script);
                                    if (!_builtSubroutines.Contains(index))
                                    {
                                        GetScriptSize(o._script);
                                        _builtSubroutines.Add(index);
                                    }
                                }
                            }
                        }
                return true;
            }
            return false;
        }
        protected int WriteScript(Script script)
        {
            //Build if it has data, if it's referenced, or if forced
            if (script.Count > 0 || script._actionRefs.Count > 0 || script._forceBuild)
            {
                //Order: all parameters (of every event, in order) then event array
                int offset = Write(script);

                foreach (Event e in script)
                    foreach (Parameter p in e)
                        if (p is EventOffset)
                        {
                            EventOffset o = (EventOffset)p;
                            bool useOffset = o._offsetInfo._list != ListValue.Null && o._offsetInfo._list != ListValue.References;
                            //Action, subaction, flash overlay and screen tint scripts have already been written
                            //Null and reference/external event offsets don't actually use the offset or have a physical script
                            //That leaves only subroutines to write here
                            if (useOffset)
                            {
                                if (o._offsetInfo._list == ListValue.SubRoutines)
                                {
                                    int index = Subroutines.IndexOf(o._script);
                                    if (!_builtSubroutines.Contains(index))
                                    {
                                        //Write subroutine directly after first reference by another script
                                        WriteScript(o._script);
                                        _builtSubroutines.Add(index);
                                    }
                                }
                            }
                        }

                return offset;
            }
            return 0;
        }

        protected void Skip(int byteCount)
        {
            _currentAddress += byteCount;
        }

        public unsafe void WriteScriptArray(Script[] scripts, bint* addr)
        {
            foreach (Script s in scripts)
                *addr++ = WriteScript(s);
        }

        public virtual void Build(VoidPtr address)
        {
            _builtSubroutines = new List<int>();

            _baseAddress = address;
            _currentAddress = address;

            int calcOffset = 0;
            for (int i = 0; i < _lengths.Count; i++)
            {
                _currentLookup = new List<VoidPtr>();

                //Write the part to the address.
                //This function will increment _currentAddress
                _buildPart[i]();

                _lookupAddresses.AddRange(_currentLookup);

                //Add the previously calculated sizes for debugging purposes
                calcOffset += _lengths[i];

                if (CurrentOffset != calcOffset)
                    throw new Exception(String.Format("Part {0} written incorrectly!", i));

                if (_currentLookup.Count != _lookupCounts[i])
                    throw new Exception(String.Format("Part {0} lookup offset counts did not match!", i));
            }
        }

        protected int Write(SakuraiEntryNode entry, int incAmt = 0)
        {
            if (entry != null)
            {
#if DEBUG
                if (entry._calcSize == 0 || entry._calcSize != entry.TotalSize)
                    throw new Exception("Entry size issues");
#endif

                int offset = entry.Write(_currentAddress);

                _currentLookup.AddRange(entry.LookupAddresses);
                _currentAddress += incAmt > 0 ? incAmt : entry._calcSize;

                return offset;
            }
            return 0;
        }

        protected unsafe void Write(SakuraiEntryNode entry, VoidPtr offsetAddr)
        {
            if (entry != null)
            {
#if DEBUG
                if (entry._calcSize == 0)
                    throw new Exception("Entry size issues");
#endif

                int offset = entry.Write(_currentAddress);

                _currentLookup.AddRange(entry.LookupAddresses);
                _currentAddress += entry._calcSize;

                *(bint*)offsetAddr = offset;
                Lookup(offsetAddr);
            }
            else
                *(bint*)offsetAddr = 0;
        }

        protected unsafe void WriteEntryList<T>(EntryList<T> e, sListOffset* o) where T : SakuraiEntryNode
        {
            if (e.Count > 0 && e._calcSize > 0)
            {
                o->_listCount = e.Count;
                o->_startOffset = Write(e);
                Lookup(o->_startOffset.Address);
            }
            else
            {
                o->_listCount = 0;
                o->_startOffset = 0;
            }
        }
    }
}
