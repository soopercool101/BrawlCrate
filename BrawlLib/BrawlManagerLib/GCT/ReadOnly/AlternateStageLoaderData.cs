using System;
using System.Collections.Generic;
using System.Linq;

namespace BrawlLib.BrawlManagerLib.GCT.ReadOnly
{
    public class AlternateStageLoaderData
    {
        private static byte[] HEADER_BRAWL =
        {
            0x46, 0x00, 0x00, 0x10,
            0x00, 0x00, 0x00, 0x00,
            0x44, 0x00, 0x00, 0x00,
            0x00, 0x5A, 0x7D, 0x00
        };

        private static byte[] HEADER_PM36 =
        {
            0x46, 0x00, 0x00, 0x10,
            0x00, 0x00, 0x00, 0x00,
            0x44, 0x00, 0x00, 0x00,
            0x00, 0x5A, 0x7C, 0xB0
        };

        private Dictionary<string, AlternateStageEntry> AlternatesByStage { get; set; }

        public AlternateStageLoaderData(byte[] data)
        {
            AlternatesByStage = new Dictionary<string, AlternateStageEntry>();

            for (int index = 0; index < data.Length; index += 8)
            {
                if (ByteUtilities.ByteArrayEquals(data, index, HEADER_BRAWL, 0, HEADER_BRAWL.Length) ||
                    ByteUtilities.ByteArrayEquals(data, index, HEADER_PM36, 0, HEADER_PM36.Length))
                {
                    int countbyte = data[index + 19];
                    index += 24;
                    int endIndex = index + 8 * countbyte - 8;
                    while (index < endIndex)
                    {
                        char[] name = new char[4];
                        name[0] = (char) data[index];
                        name[1] = (char) data[index + 1];
                        name[2] = (char) data[index + 2];
                        name[3] = (char) data[index + 3];
                        if (name[0] == '\0')
                        {
                            throw new Exception("Invalid stage name in alternate stage loader data: " +
                                                string.Join("", name.Select(c => ((int) c).ToString("X2"))));
                        }

                        Console.WriteLine(new string(name) + " " + data[index + 3].ToString("X2"));

                        int buttonActivatedCount = data[index + 4];
                        if (buttonActivatedCount > 26)
                        {
                            throw new Exception("There are more than 26 button activated alternate stages for stage " +
                                                new string(name) + ". This is probably incorrect.");
                        }

                        int randomCount = data[index + 5];
                        if (randomCount > 26)
                        {
                            throw new Exception("There are more than 26 random alternate stages for stage " +
                                                new string(name) + ". This is probably incorrect.");
                        }

                        index += 8;

                        List<AlternateStageEntry.Alternate> buttonActivated = new List<AlternateStageEntry.Alternate>();
                        for (int j = 0; j < buttonActivatedCount; j++)
                        {
                            int buttonMask = data[index] << (8 + data[index + 1]);
                            char letter = (char) ('A' + data[index + 3]);

                            buttonActivated.Add(new AlternateStageEntry.Alternate
                            {
                                ButtonMask = (ushort) buttonMask,
                                Letter = letter
                            });

                            index += 8;
                        }

                        List<AlternateStageEntry.Alternate> random = new List<AlternateStageEntry.Alternate>();
                        for (int j = 0; j < randomCount; j++)
                        {
                            char letter = (char) ('A' + j);
                            random.Add(new AlternateStageEntry.Alternate
                            {
                                Letter = letter,
                                ButtonMask = 0
                            });
                        }

                        AlternatesByStage.Add(new string(name), new AlternateStageEntry
                        {
                            Random = random,
                            ButtonActivated = buttonActivated
                        });
                    }

                    break;
                }
            }
        }

        public bool TryGetDefinition(string key, out AlternateStageEntry value)
        {
            // remove "STG" at start
            if (key.StartsWith("stg", StringComparison.InvariantCultureIgnoreCase))
            {
                key = key.Substring(3);
            }

            // remove extension
            int index = key.IndexOf('.');
            if (index >= 0)
            {
                key = key.Substring(0, index);
            }

            if (key.Length > 4)
            {
                key = key.Substring(0, 4);
            }

            key = key.ToUpperInvariant();

            // Special handling is needed for short stage names.
            if (key == "ICE")
            {
                key = "ICE\x0e"; // Thank you Project M
            }
            // Not sure about GW yet

            return AlternatesByStage.TryGetValue(key, out value);
        }
    }
}