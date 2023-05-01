using BrawlLib.Internal;
using BrawlLib.Wii;
using System.Collections.Generic;
using System.IO;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class MSBinNode : ARCEntryNode
    {
        public override ResourceType ResourceFileType => ResourceType.MSBin;
        public List<string> _strings = new List<string>();

        public override bool OnInitialize()
        {
            base.OnInitialize();

            _strings.Clear();

            byte* floor = (byte*) WorkingUncompressed.Address;
            int length = WorkingUncompressed.Length;
            bint* offsets = (bint*) floor;
            int index, last, current;

            for (index = 1, last = offsets[0]; last != length; index++)
            {
                current = offsets[index];
                if (current < last || current > length)
                {
                    break;
                }

                _strings.Add(MSBinDecoder.DecodeString(floor + last, current - last));

                last = current;
            }

            return false;
        }

        public override int OnCalculateSize(bool force)
        {
            int len = (_strings.Count + 1) << 2;
            foreach (string s in _strings)
            {
                len += MSBinDecoder.GetStringSize(s);
            }

            return len;
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            bint* offsets = (bint*) address;
            byte* current = (byte*) (offsets + _strings.Count + 1);
            foreach (string s in _strings)
            {
                *offsets++ = (int) current - (int) address;
                current += MSBinDecoder.EncodeString(s, current);
            }

            *offsets = (int) current - (int) address;
        }

        internal static ResourceNode TryParse(DataSource source, ResourceNode parent)
        {
            int length = source.Length;
            bint* offsets = (bint*) source.Address;
            int index, last, current, offsetCount;

            for (index = 0, last = 0, offsetCount = 0, current = 0; last != length; index++)
            {
                if (index * 4 > source.Length)
                {
                    return null;
                }

                current = offsets[index];
                offsetCount++;
                if (current < last || current > length)
                {
                    return null;
                }

                last = current;
            }

            for (; last == current; index++)
            {
                if (index * 4 > source.Length)
                {
                    return null;
                }

                current = offsets[index];
                if(last == current)
                    offsetCount++;
            }

            return offsets[0] == offsetCount*4 ? new MSBinNode() : null;
        }

        public override void Export(string outPath)
        {
            if (outPath.EndsWith(".txt"))
            {
                using (StreamWriter sw = new StreamWriter(outPath))
                {
                    foreach (string s in _strings)
                    {
                        sw.WriteLine(s.Replace("\r\n", "<br/>"));
                    }
                }
            }
            else
            {
                base.Export(outPath);
            }
        }

        public override void Replace(string fileName)
        {
            if (fileName.EndsWith(".txt"))
            {
                List<string> list = new List<string>();
                foreach (string s in File.ReadAllLines(fileName))
                {
                    list.Add(s.Replace("<br/>", "\r\n"));
                }

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