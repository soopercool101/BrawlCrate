using BrawlLib.BrawlManagerLib.Songs;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BrawlLib.BrawlManagerLib.GCT.ReadOnly
{
    /// <summary>
    /// Allows read-only access to all Stage-Dependent Song Loader instances in a GCT codeset.
    /// </summary>
    public class StageDependentSongLoader
    {
        private static byte[] SDSL_HEADER = {0x28, 0x70, 0x8c, 0xeb, 0x00, 0x00, 0x00};
        private static byte[] SONG_ID_CONDITION = {0x28, 0x70, 0x8d, 0x2e, 0x00, 0x00};

        private class SongReplacement
        {
            public byte stageID;
            public ushort? songID;
            public Song song;
        }

        private List<SongReplacement> songReplacements;

        public StageDependentSongLoader(byte[] data)
        {
            songReplacements = new List<SongReplacement>();

            for (int line = 0; line < data.Length; line += 8)
            {
                if (ByteUtilities.ByteArrayEquals(data, line, SDSL_HEADER, 0, SDSL_HEADER.Length))
                {
                    byte stageID = data[line + 0x7];
                    ushort? songIDIn = null;
                    ushort songIDOut;
                    if (ByteUtilities.ByteArrayEquals(data, line + 8, SONG_ID_CONDITION, 0, SONG_ID_CONDITION.Length))
                    {
                        songIDIn = (ushort) (0x100 * data[line + 14] + data[line + 15]);
                        songIDOut = (ushort) (0x100 * data[line + 30] + data[line + 31]);
                    }
                    else
                    {
                        songIDOut = (ushort) (0x100 * data[line + 22] + data[line + 23]);
                    }

                    Song s = (from g in SongIDMap.Songs
                              where g.ID == songIDOut
                              select g).FirstOrDefault();
                    if (s != null)
                    {
                        songReplacements.Add(new SongReplacement
                        {
                            stageID = stageID,
                            songID = songIDIn,
                            song = s
                        });
                        line += 24;
                    }
                    else
                    {
                        Console.WriteLine("Unknown song ID " + songIDOut.ToString("X4") +
                                          " - not currently supported.");
                    }
                }
            }
        }

        public Song GetSong(int stageID)
        {
            Song song = null;
            foreach (SongReplacement r in songReplacements)
            {
                if (stageID == r.stageID && r.songID == null)
                {
                    song = r.song;
                }
            }

            return song;
        }

        public Song GetSong(int stageID, Song originalSong)
        {
            Song song = originalSong;
            foreach (SongReplacement r in songReplacements)
            {
                if (stageID == r.stageID)
                {
                    if (song.ID == r.songID)
                    {
                        song = r.song;
                    }
                }
            }

            return song;
        }
    }
}