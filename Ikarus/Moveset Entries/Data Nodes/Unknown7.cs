using System;
using System.ComponentModel;

namespace Ikarus.MovesetFile
{
    //public unsafe class MoveDefUnk7Node : MovesetEntry
    //{
    //    internal long* Start { get { return (long*)WorkingUncompressed.Address; } }
    //    internal int Count = 0;

    //    public MoveDefUnk7Node(int count) { Count = count; }

    //    public override bool OnInitialize()
    //    {
    //        base.OnInitialize();
    //        if (_name == null)
    //            _name = "Unknown 7";
    //        return Count > 0;
    //    }

    //    public override void Parse(VoidPtr address)
    //    {
    //        long* entry = Start;
    //        for (int i = 0; i < Count; i++)
    //            new MoveDefUnk7EntryNode().Initialize(this, new DataSource((VoidPtr)(entry++), 8));
    //    }

    //    public override int GetSize()
    //    {
    //        _lookupCount = 0;
    //        return Children.Count * 8;
    //    }

    //    protected override void Write(VoidPtr address)
    //    {
    //        _rebuildAddr = address;

    //        long* addr = (long*)(address);
    //        foreach (MoveDefUnk7EntryNode b in Children)
    //            b.Rebuild(addr++, 8, true);
    //    }
    //}

    public unsafe class Unknown7Entry : MovesetEntryNode
    {
        Bin32 v1, v2;

        [Category("Unknown 7 Entry"), TypeConverter(typeof(Bin32StringConverter))]
        public Bin32 Flags1 { get { return v1; } set { v1 = value; SignalPropertyChange(); } }
        [Category("Unknown 7 Entry"), TypeConverter(typeof(Bin32StringConverter))]
        public Bin32 Flags2 { get { return v2; } set { v2 = value; SignalPropertyChange(); } }

        protected override void OnParse(VoidPtr address)
        {
            v1 = new Bin32(*(buint*)address);
            v2 = new Bin32(*(buint*)(address + 4));
        }
        protected override int OnGetSize() { return 8; }
        protected override void OnWrite(VoidPtr address)
        {
            RebuildAddress = address;

            *(buint*)address = v1._data;
            *(buint*)(address + 4) = v2._data;
        }
    }
}
