using System;
using System.ComponentModel;

namespace Ikarus.MovesetFile
{
    public unsafe class MiscGlide : MovesetEntryNode
    {
        internal float[] floatEntries;
        internal int intEntry1 = 0;

        [Category("Glide")]
        public float[] Entries { get { return floatEntries; } set { floatEntries = value; SignalPropertyChange(); } }
        [Category("Glide")]
        public int Unknown { get { return intEntry1; } set { intEntry1 = value; SignalPropertyChange(); } }

        protected override void OnParse(VoidPtr address)
        {
            bfloat* floatval = (bfloat*)address;
            bint intval1 = *(bint*)(address + 80);

            floatEntries = new float[20];
            for (int i = 0; i < floatEntries.Length; i++)
                floatEntries[i] = floatval[i];
            intEntry1 = intval1;
        }

        protected override int OnGetSize() { return 84; }

        protected override void OnWrite(VoidPtr address)
        {
            RebuildAddress = address;
            for (int i = 0; i < 20; i++)
                if (i < floatEntries.Length)
                    *(bfloat*)(address + i * 4) = floatEntries[i];
                else
                    *(bfloat*)(address + i * 4) = 0;
            *(bint*)(address + 80) = intEntry1;
        }
    }
}
