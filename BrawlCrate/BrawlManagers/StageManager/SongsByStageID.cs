using BrawlLib.BrawlManagerLib;
using BrawlLib.BrawlManagerLib.GCT.ReadOnly;
using BrawlLib.BrawlManagerLib.GCT.ReadWrite;
using BrawlLib.BrawlManagerLib.Songs;
using BrawlLib.SSBB;
using System.Collections.Generic;
using System.Linq;

namespace BrawlCrate.BrawlManagers.StageManager
{
    public static class SongsByStageID
    {
        #region mapping

        private static Dictionary<int, string[]> dict = new Dictionary<int, string[]>
        {
            {
                1, new[]
                {
                    // Battlefield
                    "X04",
                    "T02",
                    "X25",
                    "W21",
                    "W23"
                }
            },
            {
                2, new[]
                {
                    // Final Destination
                    "X05",
                    "T01",
                    "T03",
                    "W25",
                    "W31"
                }
            },
            {
                3, new[]
                {
                    // Delfino Plaza
                    "A13",
                    "A07",
                    "A08",
                    "A14",
                    "A15"
                }
            },
            {
                4, new[]
                {
                    // Luigi's Mansion
                    "A09",
                    "A06",
                    "A05",
                    "Q10",
                    "Q11"
                }
            },
            {
                5, new[]
                {
                    // Mushroomy Kingdom
                    "A01",
                    "A16",
                    "A10",
                    "A02",
                    "A03",
                    "A04"
                }
            },
            {
                6, new[]
                {
                    // Mario Circuit
                    "A20",
                    "A21",
                    "A22",
                    "A23",
                    "R05",
                    "R14",
                    "Q09"
                }
            },
            {
                8, new[]
                {
                    // Rumble Falls
                    "B01",
                    "B08",
                    "B05",
                    "B06",
                    "B07",
                    "B10",
                    "B02"
                }
            },
            {
                51, new[]
                {
                    // Bridge of Eldin
                    "C02",
                    "C09",
                    "C01",
                    "C04",
                    "C05",
                    "C08",
                    "C17",
                    "C18",
                    "C19"
                }
            },
            {
                9, new[]
                {
                    // Pirate Ship
                    "C15",
                    "C16",
                    "C07",
                    "C10",
                    "C13",
                    "C11",
                    "C12",
                    "C14"
                }
            },
            {
                11, new[]
                {
                    // Norfair
                    "D01",
                    "D03",
                    "D02",
                    "D05",
                    "R12",
                    "R07"
                }
            },
            {
                12, new[]
                {
                    // Frigate Orpheon
                    "D04",
                    "D08",
                    "D07",
                    "D06",
                    "D09",
                    "D10"
                }
            },
            {
                13, new[]
                {
                    // Yoshi's Island (Brawl)
                    "E02",
                    "E07",
                    "E01",
                    "E03",
                    "E05",
                    "E06"
                }
            },
            {
                14, new[]
                {
                    // Halberd
                    "F06",
                    "F01",
                    "F05",
                    "F04",
                    "F02",
                    "F12",
                    "F07",
                    "F08",
                    "F03",
                    "F10",
                    "F09",
                    "F11"
                }
            },
            {
                19, new[]
                {
                    // Lylat Cruise
                    "G10",
                    "G02",
                    "G01",
                    "G03",
                    "G04",
                    "G11",
                    "G05",
                    "G09",
                    "G07",
                    "G08",
                    "Q12"
                }
            },
            {
                20, new[]
                {
                    // Pokemon Stadium 2
                    "H01",
                    "H03",
                    "H02",
                    "H04",
                    "H05"
                }
            },
            {
                21, new[]
                {
                    // Spear Pillar
                    "H06",
                    "H08",
                    "H07",
                    "H09",
                    "H10"
                }
            },
            {
                22, new[]
                {
                    // Port Town Aero Dive
                    "I01",
                    "I03",
                    "I02",
                    "I04",
                    "I05",
                    "I06",
                    "I07",
                    "I08",
                    "I09",
                    "I10",
                    "R09",
                    "W18"
                }
            },
            {
                25, new[]
                {
                    // Castle Siege
                    "J02",
                    "J04",
                    "J08",
                    "J06",
                    "J07",
                    "J03",
                    "J13",
                    "J09",
                    "J10",
                    "J11",
                    "J12",
                    "W17"
                }
            },
            {
                28, new[]
                {
                    // WarioWare Inc.
                    "M01",
                    "M02",
                    "M08",
                    "M07",
                    "M06",
                    "M05",
                    "M04",
                    "M03",
                    "M09",
                    "M10",
                    "M11",
                    "M12",
                    "M13",
                    "M15",
                    "M16",
                    "M17",
                    "M18"
                }
            },
            {
                29, new[]
                {
                    // Distant Planet
                    "L06",
                    "L01",
                    "L07",
                    "L02",
                    "L04",
                    "L08",
                    "L05",
                    "L03",
                    "R08"
                }
            },
            {
                33, new[]
                {
                    // Smashville
                    "N01",
                    "N02",
                    "N03",
                    "N06",
                    "N05",
                    "N07",
                    "N08",
                    "N09",
                    "N10",
                    "N11",
                    "N12"
                }
            },
            {
                32, new[]
                {
                    // New Pork City
                    "K07",
                    "K09",
                    "K08",
                    "K10",
                    "K05",
                    "K01"
                }
            },
            {
                23, new[]
                {
                    // Summit
                    "Q07",
                    "Q06",
                    "Q08",
                    "Q05",
                    "W13"
                }
            },
            {
                30, new[]
                {
                    // Skyworld
                    "P01",
                    "P03",
                    "P02",
                    "P04"
                }
            },
            {
                7, new[]
                {
                    // 75 m
                    "B04",
                    "B03",
                    "B09"
                }
            },
            {
                31, new[]
                {
                    // Mario Bros.
                    "A17",
                    "Q02",
                    "Q01",
                    "Q13",
                    "Q14"
                }
            },
            {
                24, new[]
                {
                    // Flat Zone 2
                    "R04",
                    "Q04",
                    "W14"
                }
            },
            {
                36, new[]
                {
                    // PictoChat
                    "R02",
                    "R10",
                    "R11",
                    "R15",
                    "R16",
                    "R17",
                    "R13",
                    "R06",
                    "W20"
                }
            },
            {
                37, new[]
                {
                    // Hanenbow
                    "R03"
                }
            },
            {
                34, new[]
                {
                    // Shadow Moses Island
                    "S06",
                    "S02",
                    "S03",
                    "S08",
                    "S04",
                    "S07",
                    "S05",
                    "S10",
                    "S11"
                }
            },
            {
                35, new[]
                {
                    // Green Hill Zone
                    "U01",
                    "U04",
                    "U02",
                    "U03",
                    "U06",
                    "U07",
                    "U08",
                    "U09",
                    "U10",
                    "U11",
                    "U12",
                    "U13"
                }
            },
            {
                41, new[]
                {
                    // Temple
                    "C03",
                    "W24"
                }
            },
            {
                42, new[]
                {
                    // Yoshi's Island (Melee)
                    "W05",
                    "W15"
                }
            },
            {
                43, new[]
                {
                    // Jungle Japes
                    "W03",
                    "W26"
                }
            },
            {
                44, new[]
                {
                    // Onett
                    "W12",
                    "W19"
                }
            },
            {
                48, new[]
                {
                    // Corneria
                    "W08",
                    "W28"
                }
            },
            {
                47, new[]
                {
                    // Rainbow Cruise
                    "W02",
                    "W01"
                }
            },
            {
                45, new[]
                {
                    // Green Greens
                    "W07",
                    "W06"
                }
            },
            {
                49, new[]
                {
                    // Big Blue
                    "W11",
                    "W29"
                }
            },
            {
                50, new[]
                {
                    // Brinstar
                    "W27",
                    "W04"
                }
            },
            {
                46, new[]
                {
                    // Pokemon Stadium
                    "W09",
                    "W16",
                    "W10"
                }
            },
            {
                55, new[]
                {
                    // Online Training
                    "X07"
                }
            }
        };

        #endregion

        public static string[] ForPac(CustomSSSCodeset sss, string filename)
        {
            return ForPac(sss.TracklistModifier, sss.CMM, filename);
        }

        public static string[] ForPac(TracklistModifier modifier, CMM cmm, string filename)
        {
            filename = filename.ToLower();
            string key = filename.Substring(0, filename.IndexOfAny("_.".ToCharArray()));
            int stageId = StageIDMap.StageIDForPac(filename);
            if (cmm != null && cmm.Map.TryGetValue(stageId, out IEnumerable<Song> songs))
            {
                return songs.Select(s => s.Filename).ToArray();
            }

            // Tracklist Modifier 1.0 [Phantom Wings]
            if (cmm.TracklistModifier != null)
            {
                byte currentTracklistId = cmm.TracklistModifier[stageId];
                for (int i = 0; i < CMM.StandardCMMTracklistModifierData.Length; i++)
                {
                    if (CMM.StandardCMMTracklistModifierData[i] == currentTracklistId)
                    {
                        int originalStageId = i;
                        // Make sure the index is a valid stage ID. Some bytes
                        // appear more than once, the first time at an index
                        // that's not a valid stage ID (e.g. Battlefield's 00
                        // at index 0 or Bridge of Eldin's 0A at index 10.)
                        if (Stage.Stages.Any(s => s.ID == i))
                        {
                            stageId = originalStageId;
                            break;
                        }
                    }
                }
            }

            // Tracklist Modifier [standardtoaster]
            stageId = modifier[stageId];
            if (stageId == 5 && filename.StartsWith("stgmariopast_00"))
            {
                return new[]
                {
                    "A01",
                    "A16",
                    "A10"
                };
            }
            else if (stageId == 5 && filename.StartsWith("stgmariopast_01"))
            {
                return new[]
                {
                    "A02",
                    "A03",
                    "A04"
                };
            }
            else if (dict.TryGetValue(stageId, out string[] ret))
            {
                return ret;
            }
            else
            {
                return new string[0];
            }
        }
    }
}