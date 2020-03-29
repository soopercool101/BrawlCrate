using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BrawlLib.BrawlManagerLib.GCT.ReadOnly
{
    public class CNMT
    {
        public readonly Dictionary<ushort, string> Map;

        public CNMT(byte[] data)
        {
            Map = new Dictionary<ushort, string>();
            for (int lineskip = 0; lineskip < data.Length; lineskip += 8)
            {
                if (data[lineskip] != 0x06)
                {
                    continue;
                }

                int id = (data[lineskip + 1] << 16)
                         | (data[lineskip + 2] << 8)
                         | data[lineskip + 3];

                int songId1 = (id - 4031616) / 144;
                if (songId1 < 0x286C || songId1 > 0x2BBD)
                {
                    continue;
                }

                int songId2 = songId1 + 0xBC94;

                if (data[lineskip + 4] != 0x00)
                {
                    continue;
                }

                if (data[lineskip + 5] != 0x00)
                {
                    continue;
                }

                if (data[lineskip + 6] != 0x00)
                {
                    continue;
                }

                int bytes = data[lineskip + 7];

                byte[] byteArray = data
                    .Skip(lineskip)
                    .Skip(8)
                    .Take(bytes)
                    .TakeWhile(c => c != 0)
                    .ToArray();
                string str = Encoding.UTF8.GetString(byteArray);
                if (Map.TryGetValue((ushort) songId1, out string existing) && existing != str)
                {
                    Console.Error.WriteLine(
                        $"Replacing already read song title \"{existing}\" for {songId1.ToString("X4")}/{songId2.ToString("X4")} with new song title \"{str}\"");
                }

                Map[(ushort) songId1] = str;
                Map[(ushort) songId2] = str;
            }
        }
    }
}