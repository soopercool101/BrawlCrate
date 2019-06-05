using System;
using System.Collections.Generic;
using System.IO;
using BrawlLib.Wii;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class MSBinNode : ARCEntryNode
    {
        public List<string> _strings = new List<string>();
        public override ResourceType ResourceFileType => ResourceType.MSBin;

        public override bool OnInitialize()
        {
            base.OnInitialize();

            _strings.Clear();

            var floor = (byte*) WorkingUncompressed.Address;
            var length = WorkingUncompressed.Length;
            var offsets = (bint*) floor;
            int index, last, current;

            for (index = 1, last = offsets[0]; last != length; index++)
            {
                current = offsets[index];
                if (current < last || current > length) break;

                _strings.Add(MSBinDecoder.DecodeString(floor + last, current - last));

                last = current;
            }

            return false;
        }

        public override int OnCalculateSize(bool force)
        {
            var len = (_strings.Count + 1) << 2;
            foreach (var s in _strings) len += MSBinDecoder.GetStringSize(s);

            return len;
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            var offsets = (bint*) address;
            var current = (byte*) (offsets + _strings.Count + 1);
            foreach (var s in _strings)
            {
                *offsets++ = (int) current - (int) address;
                current += MSBinDecoder.EncodeString(s, current);
            }

            *offsets = (int) current - (int) address;
        }

        internal static ResourceNode TryParse(DataSource source)
        {
            var length = source.Length;
            var offsets = (bint*) source.Address;
            int index, last, current;

            for (index = 0, last = 0; last != length; index++)
            {
                if (index * 4 > source.Length) return null;

                current = offsets[index];
                if (current < last || current > length) return null;

                last = current;
            }

            return offsets[0] == index << 2 ? new MSBinNode() : null;
        }

        public override void Export(string outPath)
        {
            if (outPath.EndsWith(".txt"))
                using (var sw = new StreamWriter(outPath))
                {
                    foreach (var s in _strings) sw.WriteLine(s.Replace("\r\n", "<br/>"));
                }
            else
                base.Export(outPath);
        }

        public override void Replace(string fileName)
        {
            if (fileName.EndsWith(".txt"))
            {
                var list = new List<string>();
                foreach (var s in File.ReadAllLines(fileName)) list.Add(s.Replace("<br/>", "\r\n"));
                _strings = list;
                SignalPropertyChange();
                ForceReplacedEvent();
            }
            else
            {
                base.Replace(fileName);
            }
        }
    }
}