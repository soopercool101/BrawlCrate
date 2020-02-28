using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace BrawlLib.BrawlManagerLib.GCT.ReadOnly
{
    /// <summary>
    /// Allows read-only access to the Tracklist Modifier code in a GCT codeset.
    /// </summary>
    public class TracklistModifier
    {
        private static byte[] HEADER = {0xC2, 0x10, 0xF9, 0xB4, 0x00, 0x00, 0x00};

        private Dictionary<byte, byte> StageIdMap { get; set; }

        public byte this[int stageId]
        {
            get
            {
                byte id = (byte) stageId;
                if (id == stageId && StageIdMap.TryGetValue(id, out byte newStageId))
                {
                    return newStageId;
                }

                return id;
            }
        }

        public TracklistModifier(byte[] data)
        {
            Dictionary<byte, byte> map1 = new Dictionary<byte, byte>();
            Dictionary<byte, byte> map2 = new Dictionary<byte, byte>();

            using (MemoryStream ms = new MemoryStream(data))
            {
                byte[] line = new byte[8];
                while (true)
                {
                    int read = ms.Read(line, 0, 8);
                    if (read < 8)
                    {
                        break;
                    }

                    if (ByteUtilities.ByteArrayEquals(line, 0, HEADER, 0, 7))
                    {
                        int lines = line[7];
                        for (int i = 0; i < lines; i++)
                        {
                            read = ms.Read(line, 0, 8);
                            if (read < 8)
                            {
                                throw new FormatException(
                                    $"Could not read all of Tracklist Modifier code (line {i + 1}, read {read} bytes)");
                            }

                            switch (line[0])
                            {
                                case 0x2C:
                                    map1.Add(line[3], line[7]);
                                    break;
                                case 0x48:
                                    map2.Add(line[3], line[7]);
                                    break;
                                case 0x7c:
                                    break;
                                default:
                                    throw new FormatException("Unrecognized line in Tracklist Modifier code: " +
                                                              string.Join("",
                                                                  line.Select(b => ((int) b).ToString("X2"))));
                            }
                        }

                        break;
                    }
                }
            }

            StageIdMap = new Dictionary<byte, byte>();
            foreach (KeyValuePair<byte, byte> pair in map1)
            {
                if (map2.TryGetValue(pair.Value, out byte newStageId))
                {
                    StageIdMap.Add(pair.Key, newStageId);
                }
                else
                {
                    throw new FormatException(
                        $"Part 2 of Tracklist Modifier uses ID {pair.Value} not present in Part 3");
                }
            }
        }
    }
}