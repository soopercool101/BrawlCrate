using BrawlLib.Internal;
using BrawlLib.Internal.Windows.Controls.Hex_Editor;
using System.ComponentModel;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class ModuleDataNode : RELEntryNode
    {
        public RelocationManager _manager;

        [Browsable(false)] public virtual bool HasCode => true;
        [Browsable(false)] public virtual uint ASMOffset => RootOffset;

        [DisplayName("Size")]
        public string DataSize => "0x" + (_dataBuffer != null ? _dataBuffer.Length.ToString("X") : "0");

        public UnsafeBuffer _dataBuffer;
        public SectionEditor _linkedEditor = null;

        public override void Dispose()
        {
            if (_dataBuffer != null)
            {
                _dataBuffer.Dispose();
                _dataBuffer = null;
            }

            base.Dispose();
        }

        [Browsable(false)] public buint* BufferAddress => (buint*) _dataBuffer.Address;

        //public Relocation this[int index] 
        //{
        //    get
        //    {
        //        List<Relocation> l = _linkedEditor != null ? _linkedEditor._relocations : _relocations;

        //        if (index < l.Count && index >= 0)
        //            return l[index]; 
        //        return null;
        //    }
        //    set
        //    {
        //        List<Relocation> l = _linkedEditor != null ? _linkedEditor._relocations : _relocations;

        //        if (index < l.Count && index >= 0)
        //            l[index] = value;
        //    }
        //}

        /// <summary>
        /// Fills the data buffer with the specified amount of data from an address.
        /// </summary>
        public void InitBuffer(uint size, VoidPtr address)
        {
            _dataBuffer = new UnsafeBuffer((int) size.RoundUp(4));
            Memory.Move(_dataBuffer.Address, address, size);
            _manager = new RelocationManager(this);
        }

        /// <summary>
        /// Fills the data buffer with the specified amount of zerobytes.
        /// </summary>
        public void InitBuffer(uint size)
        {
            _dataBuffer = new UnsafeBuffer((int) size.RoundUp(4));
            Memory.Fill(_dataBuffer.Address, size, 0);
            _manager = new RelocationManager(this);
        }

        //private void ApplyTags()
        //{
        //    if (HasCode)
        //    {
        //        int i = 0;
        //        foreach (Relocation r in Relocations)
        //        {
        //            PPCOpCode op = r.Code;
        //            if (op is BranchOpcode)
        //            {
        //                BranchOpcode b = op as BranchOpcode;
        //                if (!b.Absolute)
        //                {
        //                    int offset = b.Offset;
        //                    int iOff = offset.RoundDown(4) / 4;
        //                    int index = i + iOff;
        //                    if (index >= 0 && index < _relocations.Count)
        //                        Relocations[index].Tags.Add(String.Format("Sub 0x{0}", PPCFormat.Hex(ASMOffset + (uint)i * 4)));
        //                }
        //            }
        //            i++;
        //        }
        //    }
        //}

        //public void RemoveAtIndex(int index)
        //{
        //    if (_dataBuffer.Length < 4)
        //        return;

        //    UnsafeBuffer newBuffer = new UnsafeBuffer(_dataBuffer.Length - 4);
        //    _relocations.RemoveAt(index);

        //    for (int i = index; i < _relocations.Count; i++)
        //    {
        //        Relocation r = _relocations[i];
        //        foreach (Relocation l in r.Linked)
        //            if (l.Command != null && l.Command.TargetRelocation._section == this)
        //                l.Command._addend -= 4;
        //        r._index--;
        //    }

        //    int offset = index * 4;

        //    //Move memory before the removed value
        //    if (offset > 0)
        //        Memory.Move(newBuffer.Address, _dataBuffer.Address, (uint)offset);

        //    //Move memory after the removed value
        //    if (offset + 4 < _dataBuffer.Length)
        //        Memory.Move(newBuffer.Address + offset, _dataBuffer.Address + offset + 4, (uint)_dataBuffer.Length - (uint)(offset + 4));

        //    _dataBuffer.Dispose();
        //    _dataBuffer = newBuffer;
        //}

        //public void InsertAtIndex(int index, Relocation r)
        //{
        //    UnsafeBuffer newBuffer = new UnsafeBuffer(_dataBuffer.Length + 4);
        //    _relocations.Insert(index, r);
        //    r._index = index;

        //    for (int i = index + 1; i < _relocations.Count; i++)
        //    {
        //        Relocation e = _relocations[i];
        //        foreach (Relocation l in e.Linked)
        //            if (l.Command != null && l.Command.TargetRelocation._section == this)
        //                l.Command._addend += 4;
        //        e._index++;
        //    }

        //    int offset = index * 4;

        //    //Move memory before the inserted value
        //    if (offset > 0)
        //        Memory.Move(newBuffer.Address, _dataBuffer.Address, (uint)offset);

        //    //Move memory after the inserted value
        //    if (offset + 4 < _dataBuffer.Length)
        //        Memory.Move(newBuffer.Address + offset + 4, _dataBuffer.Address + offset + 4, (uint)_dataBuffer.Length - (uint)(offset + 4));

        //    //Clear the new value
        //    *(uint*)(newBuffer.Address + offset) = 0;

        //    _dataBuffer.Dispose();
        //    _dataBuffer = newBuffer;
        //}

        //public void Resize(int newSize)
        //{
        //    int diff = (newSize.RoundDown(4) - _dataBuffer.Length) / 4;
        //    if (diff == 0)
        //        return;
        //    if (diff > 0)
        //        for (int i = 0; i < diff; i++)
        //            _relocations.Add(new Relocation(this, _relocations.Count));
        //    else if (diff < 0)
        //        _relocations.RemoveRange(_relocations.Count + diff, -diff);
        //
        //    UnsafeBuffer newBuffer = new UnsafeBuffer(newSize);
        //    int max = Math.Min(_dataBuffer.Length, newBuffer.Length);
        //    if (max > 0)
        //        Memory.Move(newBuffer.Address, _dataBuffer.Address, (uint)max);
        //
        //    _dataBuffer.Dispose();
        //    _dataBuffer = newBuffer;
        //}
    }
}