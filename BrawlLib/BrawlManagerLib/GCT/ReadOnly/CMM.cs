using BrawlLib.BrawlManagerLib.Songs;
using BrawlLib.SSBB;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace BrawlLib.BrawlManagerLib.GCT.ReadOnly
{
    public class CMM
    {
        private static IEnumerable<byte> StandardCMMTracklistModifierDataSeq()
        {
            string s = @"
00000102 03040607
08090A0B 0C0D0E58
595A5B0F 10111213
14151515 16171819
1A1B1C1D 1E1F5200
53202122 23242526
2728290A 54005556
57000000 00000000
2D2E2F30 31323334
35363738 393A3B3C
3D3E3F40 41424344
45464748 494A4B4C
4D4E4F50 515C5D5E
5F".Replace(" ", "").Replace("\r", "").Replace("\n", "");
            for (int i = 0; i < s.Length; i += 2)
            {
                yield return (byte) int.Parse(s.Substring(i, 2), System.Globalization.NumberStyles.HexNumber);
            }
        }

        public static readonly byte[] StandardCMMTracklistModifierData =
            StandardCMMTracklistModifierDataSeq().ToArray();

        private IEnumerable<Stage> GetStageByIdInTracklist(int id)
        {
            byte[] b = TracklistModifier ?? StandardCMMTracklistModifierData;
            for (int i = 0; i < b.Length; i++)
            {
                if (b[i] == id)
                {
                    foreach (Stage s in Stage.Stages.Where(s => s.ID == i))
                    {
                        yield return s;
                    }
                }
            }
        }

        public readonly byte[] TracklistModifier;
        public readonly Dictionary<int, IEnumerable<Song>> Map;

        public CMM(byte[] data)
        {
            for (int lineskip = 0; lineskip < data.Length; lineskip += 8)
            {
                if (ByteUtilities.ByteArrayEquals(data, lineskip, new byte[] {0x16, 0x1A, 0x47, 0xE8, 0, 0, 0}, 0, 7))
                {
                    int byteCount = data[lineskip + 7];
                    TracklistModifier = data.Skip(lineskip + 8).Take(byteCount).ToArray();
                }
            }

            Map = new Dictionary<int, IEnumerable<Song>>();
            for (int lineskip = 0; lineskip < data.Length; lineskip += 8)
            {
                if (data[lineskip] == 0x00 && data[lineskip + 1] == 0x53 && data[lineskip + 2] == 0xCE)
                {
                    int tracklistIndex = data[lineskip + 3] - 0xA0;
                    int tracks = data[lineskip + 7];
                    foreach (Stage stage in GetStageByIdInTracklist(tracklistIndex))
                    {
                        IEnumerable<Song> songs = ReadSongs(data, lineskip + 16, tracks);
                        if (Map.TryGetValue(stage.ID, out IEnumerable<Song> existing))
                        {
                            Console.Error.WriteLine(
                                $"Replacing already read {existing.Count()}-song tracklist for {stage} with newly read {songs.Count()}-song tracklist");
                        }

                        Map[stage.ID] = songs;
                    }
                }
            }
        }

        private static IEnumerable<Song> ReadSongs(byte[] data, int offset, int tracks)
        {
            using (MemoryStream ms = new MemoryStream(data, offset, data.Length - offset))
            {
                for (int i = 0; i < tracks; i++)
                {
                    for (int j = 0; j < 6; j++)
                    {
                        ms.ReadByte();
                    }

                    int songId = 0;
                    songId |= ms.ReadByte() << 8;
                    songId |= ms.ReadByte();
                    Song song = SongIDMap.Songs.SingleOrDefault(s => s.ID == songId);
                    if (song != null)
                    {
                        yield return song;
                    }
                }
            }
        }
    }
}