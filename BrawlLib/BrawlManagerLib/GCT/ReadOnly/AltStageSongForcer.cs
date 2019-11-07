using BrawlLib.BrawlManagerLib.Songs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace BrawlLib.BrawlManagerLib.GCT.ReadOnly
{
    /// <summary>
    /// Allows read-only access to the alt stage song forcer in a GCT codeset.
    /// </summary>
    public class AltStageSongForcer
    {
        private static byte[] HEADER = new byte[]
        {
            0xC2, 0x10, 0xF9, 0xFC, 0x00, 0x00, 0x00, 0x2B,
            0x3D, 0x80, 0x81, 0x5F, 0xA1, 0x8C, 0x84, 0x22,
            0x54, 0x00, 0x80, 0x1E, 0x54, 0x00, 0x84, 0x3E
        };

        private class SongReplacement
        {
            public byte stageID;
            public ushort buttons;
            public ushort? songID;
            public Song song;
        }

        private List<SongReplacement> songReplacements;

        public AltStageSongForcer(byte[] data)
        {
            songReplacements = new List<SongReplacement>();

            for (int lineskip = 0; lineskip < data.Length; lineskip += 8)
            {
                if (ByteUtilities.ByteArrayEquals(data, lineskip, HEADER, 0, HEADER.Length))
                {
                    lineskip += HEADER.Length;
                    using (MemoryStream ms = new MemoryStream(data, lineskip, data.Length - lineskip))
                    {
                        byte stageID = 0;
                        ushort buttons = 0;
                        ushort? songID = null;

                        byte[] line = new byte[4];
                        while (true)
                        {
                            int read = ms.Read(line, 0, 4);
                            if (read < 8)
                            {
                                throw new FormatException();
                            }

                            if (ByteUtilities.ByteArrayEquals(line, 0, new byte[] {0x2C, 0x1B, 0x00}, 0, 3))
                            {
                                stageID = line[3];
                                buttons = 0;
                                songID = null;
                            }
                            else if (ByteUtilities.ByteArrayEquals(line, 0, new byte[] {0x2C, 0x0C}, 0, 2))
                            {
                                buttons = (ushort) (line[2] << (8 + line[3]));
                            }
                            else if (ByteUtilities.ByteArrayEquals(line, 0, new byte[] {0x2C, 0x00}, 0, 2))
                            {
                                songID = (ushort) (line[2] << (8 + line[3]));
                            }
                            else if (ByteUtilities.ByteArrayEquals(line, 0, new byte[] {0x38, 0x00}, 0, 2))
                            {
                                ushort replacementSongID = (ushort) (line[2] << (8 + line[3]));
                                songReplacements.Add(new SongReplacement
                                {
                                    stageID = stageID,
                                    buttons = buttons,
                                    songID = songID,
                                    song = (from g in SongIDMap.Songs
                                            where g.ID == replacementSongID
                                            select g).First()
                                });
                            }
                            else if (ByteUtilities.ByteArrayEquals(line, 0, new byte[] {0x40, 0x82, 0x00}, 0, 3))
                            {
                            }
                            else if (ByteUtilities.ByteArrayEquals(line, 0, new byte[] {0x90, 0x1D, 0x00, 0x00}, 0, 4))
                            {
                                break;
                            }
                            else
                            {
                                throw new FormatException();
                            }
                        }
                    }

                    break;
                }
            }
        }

        public Song GetSong(int stageID, ushort buttons)
        {
            Song song = null;
            foreach (SongReplacement r in songReplacements)
            {
                if (stageID == r.stageID && buttons == r.buttons && r.songID == null)
                {
                    song = r.song;
                }
            }

            return song;
        }

        public Song GetSong(int stageID, ushort buttons, Song originalSong)
        {
            Song song = originalSong;
            foreach (SongReplacement r in songReplacements)
            {
                if (stageID == r.stageID && buttons == r.buttons)
                {
                    if (song.ID == r.songID || r.songID == null)
                    {
                        song = r.song;
                    }
                }
            }

            return song;
        }
    }
}