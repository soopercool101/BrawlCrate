using BrawlLib.BrawlManagerLib.GCT.ReadOnly;
using BrawlLib.BrawlManagerLib.Songs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace BrawlLib.BrawlManagerLib.GCT.ReadWrite
{
    /// <summary>
    /// Allows read-write access to Custom Song Volume code.
    /// This class will store the entire codeset, split between itself (the CSV code) and the portions before/after (in raw byte[]).
    /// </summary>
    public class CustomSongVolumeCodeset
    {
        public byte[] DataBefore { get; private set; } // Contains the GCT header
        public byte[] DataAfter { get; private set; }  // Contains the GCT footer

        public Dictionary<ushort, byte> Settings { get; private set; }

        private CNMT _cnmt;

        /// <summary>
        /// Gets read-only access to instances of the CMM Setting Code [JOJI] in this codeset.
        /// </summary>
        public CNMT CNMT
        {
            get
            {
                if (_cnmt == null)
                {
                    _cnmt = new CNMT(DataBefore.Concat(DataAfter).ToArray());
                }

                return _cnmt;
            }
        }

        public CustomSongVolumeCodeset(string[] s)
        {
            init(s);
        }

        public CustomSongVolumeCodeset(byte[] data)
        {
            init(data);
        }

        public CustomSongVolumeCodeset(string filename)
        {
            if (filename.EndsWith("gct", StringComparison.InvariantCultureIgnoreCase))
            {
                init(File.ReadAllBytes(filename));
            }
            else
            {
                init(File.ReadAllLines(filename));
            }
        }

        private static byte[] gctheader = {0x00, 0xd0, 0xc0, 0xde, 0x00, 0xd0, 0xc0, 0xde};
        private static byte[] gctfooter = {0xf0, 0, 0, 0, 0, 0, 0, 0};

        private void init(string[] s)
        {
            Regex r = new Regex(@"(\* )?[A-Fa-f0-9]{8} [A-Fa-f0-9]{8}");
            IEnumerable<string> matching_lines =
                from line in s
                where r.IsMatch(line)
                select line;

            byte[] core = ByteUtilities.StringToByteArray(string.Join("\n", matching_lines));
            byte[] data = new byte[core.Length + 16];
            Array.ConstrainedCopy(gctheader, 0, data, 0, 8);
            Array.ConstrainedCopy(core, 0, data, 8, core.Length);
            Array.ConstrainedCopy(gctfooter, 0, data, data.Length - 8, 8);
            init(data);
        }

        private static byte[] CSV_HEADER = ByteUtilities.StringToByteArray(@"
C21C744C 00000006
3D80901C 618C3FFC
A7AC0004 2C1D7FFF
41820014 7C1DF000
4082FFF0 A00C0002
48000008 88030014
60000000 00000000
4A000000 90000000
161C4000");

        private void init(byte[] data)
        {
            Settings = new Dictionary<ushort, byte>();

            int index = -1;
            for (int line = 0; line < data.Length; line += 8)
            {
                if (ByteUtilities.ByteArrayEquals(data, line, CSV_HEADER, 0, CSV_HEADER.Length))
                {
                    index = line;
                }
            }

            if (index < 0)
            {
                Console.WriteLine("No Custom Song Volume code found. An empty code will be created.");

                DataBefore = gctheader.ToArray();
                DataAfter = data.Skip(gctheader.Length).ToArray();
            }
            else
            {
                int start = index;
                DataBefore = new byte[start];
                Array.ConstrainedCopy(data, 0, DataBefore, 0, start);

                index += 9 * 8;
                byte byte_count = data[index - 1];
                bool found_terminator = false;
                for (int i = 0; i < byte_count; i += 4)
                {
                    ushort u = (ushort) (data[index + i] * 0x100 + data[index + i + 1]);
                    if (u == 0x7FFF)
                    {
                        if (found_terminator)
                        {
                            throw new InvalidDataException("Two terminators");
                        }

                        found_terminator = true;
                    }
                    else
                    {
                        Settings.Add(u, data[index + i + 3]);
                    }
                }

                if (!found_terminator)
                {
                    throw new InvalidDataException("No terminators");
                }

                index += byte_count;
                while (index % 8 != 0)
                {
                    index++;
                }

                DataAfter = new byte[data.Length - index];
                Array.ConstrainedCopy(data, index, DataAfter, 0, data.Length - index);
            }

            bool footer_found = false;
            for (int i = 0; i < DataAfter.Length; i += 8)
            {
                if (footer_found)
                {
                    MessageBox.Show("Extra data found after GCT footer - this will be discarded if you save the GCT.");
                    DataAfter = DataAfter.Take(i).ToArray();
                    break;
                }

                if (ByteUtilities.ByteArrayEquals(DataAfter, i, gctfooter, 0, 8))
                {
                    footer_found = true;
                }
            }
        }

        public override string ToString()
        {
            return string.Join("\n",
                from p in Settings
                join s in SongIDMap.Songs on p.Key equals s.ID
                select p.Value / 127.0 + " <= " + s.DefaultName
            );
        }

        public byte[] ExportCode()
        {
            if (Settings.Count == 0)
            {
                return new byte[0];
            }

            int len = 0x48 + 4 * Settings.Count + 4;
            while (len % 8 != 0)
            {
                len++;
            }

            byte[] b = new byte[len];
            Array.Copy(CSV_HEADER, b, 0x44);
            b[0x47] = (byte) (4 * Settings.Count + 4);

            int index = 0x48;
            foreach (KeyValuePair<ushort, byte> s in Settings)
            {
                b[index] = (byte) (s.Key / 0x100);
                b[index + 1] = (byte) (s.Key % 0x100);
                b[index + 3] = (byte) s.Value;
                index += 4;
            }

            b[index] = 0x7F;
            b[index + 1] = 0xFF;
            return b;
        }

        public string ExportCodeString()
        {
            StringBuilder sb = new StringBuilder("Custom Song Volume [standardtoaster, Magus]\n");
            byte[] data = ExportCode();
            int i = 0;
            foreach (byte b in data)
            {
                sb.Append(b.ToString("X2"));
                i++;
                if (i % 8 == 4)
                {
                    sb.Append(' ');
                }
                else if (i % 8 == 0)
                {
                    sb.AppendLine();
                }
            }

            return sb.ToString();
        }

        public byte[] ExportGCT()
        {
            List<byte> l = new List<byte>();
            foreach (byte[] b in new byte[][] {DataBefore, ExportCode(), DataAfter})
            {
                l.AddRange(b);
            }

            return l.ToArray();
        }
    }
}