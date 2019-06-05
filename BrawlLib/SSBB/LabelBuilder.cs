using System;
using System.Collections.Generic;
using BrawlLib.SSBBTypes;

namespace BrawlLib.SSBB
{
    public unsafe class LabelBuilder
    {
        private readonly List<LabelItem> _labels = new List<LabelItem>();

        public int Count => _labels.Count;

        public void Clear()
        {
            _labels.Clear();
        }

        public void Add(uint tag, string str)
        {
            _labels.Add(new LabelItem {Tag = tag, String = str});
        }

        public int GetSize()
        {
            var len = 12;
            foreach (var label in _labels) len += label.DataLen + 4;

            return len.Align(0x20);
        }

        public void Write(VoidPtr address)
        {
            var header = (LABLHeader*) address;
            var count = _labels.Count;
            var dataAddr = address + 12 + count * 4;
            var list = (bint*) (address + 8);
            LabelItem label;
            int size;
            byte* pad;

            for (var i = 0; i < count;)
            {
                label = _labels[i++];
                list[i] = (int) dataAddr - (int) list;
                ((LABLEntry*) dataAddr)->Set(label.Tag, label.String);
                dataAddr += label.DataLen;
            }

            pad = (byte*) dataAddr;
            for (size = dataAddr - address; (size & 0x1F) != 0; size++) *pad++ = 0;

            header->Set(size, count);
        }
    }

    public struct LabelItem
    {
        public uint Tag;
        public string String;

        public int DataLen => (String.Length + 9).Align(4);
    }
}