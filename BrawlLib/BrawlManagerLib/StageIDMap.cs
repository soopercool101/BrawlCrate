using BrawlLib.BrawlManagerLib.GCT.ReadWrite;
using BrawlLib.SSBB;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace BrawlLib.BrawlManagerLib
{
    public class StageIDMap
    {
        public static Stage[] Stages => Stage.Stages;
        public static ReadOnlyCollection<KeyValuePair<byte, string>> StagesByID { get; private set; }

        static StageIDMap()
        {
            // static initializer

            List<KeyValuePair<byte, string>> byID = new List<KeyValuePair<byte, string>>();
            for (byte b = 1; b <= 103; b++)
            {
                string pac = PacBasenameForStageID(b);
                if (pac != null)
                {
                    byID.Add(new KeyValuePair<byte, string>(b, pac.ToUpper()));
                }
            }

            StagesByID = byID.AsReadOnly();
        }

        public static List<string> PacFilesBySSSOrder(CustomSSSCodeset sss)
        {
            List<string> list = new List<string>();
            foreach (int stage_id in sss.StageIDsInOrder)
            {
                if (stage_id >= 0x40)
                {
                    list.Add("STGCUSTOM" + (stage_id - 0x3F).ToString("X2") + ".pac");
                }
                else
                {
                    IEnumerable<string[]> q = from s in Stages
                                              where s.ID == stage_id
                                              select s.PacNames;
                    foreach (string[] ss in q)
                    {
                        list.AddRange(ss);
                    }
                }
            }

            return list;
        }

        public static int StageIDForPac(string filename)
        {
            int stageID = -1;
            if (filename.StartsWith("STGCUSTOM", StringComparison.InvariantCultureIgnoreCase))
            {
                stageID = Convert.ToInt32(filename.Substring(9, 2), 16) + 0x3F;
            }
            else
            {
                IEnumerable<byte> q = from s in Stages
                                      where s.ContainsPac(filename)
                                      select s.ID;
                if (q.Count() > 1)
                {
                    Console.WriteLine("More than one stage matches the search pattern: " + filename);
                    return q.First();
                }

                if (q.Count() < 1)
                {
                    Console.WriteLine("No stage matches the search pattern: " + filename);
                    return -1;
                }

                stageID = q.First();
            }

            return stageID;
        }

        public static string PacBasenameForStageID(int stageID)
        {
            if (stageID >= 0x40)
            {
                return "custom" + (stageID - 0x3F).ToString("X2");
            }

            IEnumerable<string> q = from s in Stages
                                    where s.ID == stageID
                                    select s.PacBasename;
            if (!q.Any())
            {
                return null;
            }

            return q.First();
        }

        public static string RelNameForPac(string filename, bool differentForAlternateStage)
        {
            string asl_append = "";
            if (differentForAlternateStage)
            {
                string lastsix = ("++++++" + filename).Substring(filename.Length, 6);
                if (lastsix[0] == '_')
                {
                    asl_append = "_" + lastsix[1];
                }
            }

            if (filename.StartsWith("STGCUSTOM", StringComparison.InvariantCultureIgnoreCase))
            {
                return "st_custom" + filename.Substring(9, 2) + asl_append + ".rel";
            }

            IEnumerable<string> q = from s in Stages
                                    where s.ContainsPac(filename)
                                    select s.RelName.Replace(".rel", "");
            if (q.Count() > 1)
            {
                Console.WriteLine("More than one stage matches the search pattern: " + filename);
            }
            else if (q.Count() < 1)
            {
                Console.WriteLine("No stage matches the search pattern: " + filename);
                return "none";
            }

            return q.First() + asl_append + ".rel";
        }

        #region sc_selcharacter2

        // the sss3 index is also the index in Misc Data [1] in sc_selcharacter2
        private static int[] sc_selcharacter2_icon_from_sss3_index =
        {
            1,  // Battlefield
            2,  // Final Destination
            3,  // Delfino Plaza
            4,  // Luigi's Mansion
            5,  // Mushroomy Kingdom
            6,  // Mario Circuit
            25, // 75 m
            7,  // Rumble Falls
            9,  // Pirate Ship
            8,  // Bridge of Eldin
            10, // Norfair
            11, // Frigate Orpheon
            12, // Yoshi's Island (Brawl)
            13, // Halberd
            14, // Lylat Cruise
            15, // Pokemon Stadium 2
            16, // Spear Pillar
            17, // Port Town Aero Dive
            23, // Summit
            27, // Flat Zone 2
            18, // Castle Siege
            19, // WarioWare Inc.
            20, // Distant Planet
            24, // Skyworld
            26, // Mario Bros.
            22, // New Pork City
            21, // Smashville
            30, // Shadow Moses Island
            31, // Green Hill Zone
            28, // PictoChat
            29, // Hanenbow
            50, // Temple
            51, // Yoshi's Island (Melee)
            52, // Jungle Japes
            53, // Onett
            54, // Green Greens
            55, // Rainbow Cruise
            56, // Corneria
            57, // Big Blue
            58, // Brinstar
            59  // Pokemon Stadium
        };

        public static int sssPositionForSelcharacter2Icon(int selcharacter2Icon)
        {
            int index = -1;
            for (int i = 0; i < sc_selcharacter2_icon_from_sss3_index.Length; i++)
            {
                if (sc_selcharacter2_icon_from_sss3_index[i] == selcharacter2Icon)
                {
                    index = i;
                    break;
                }
            }

            return index;
        }

        #endregion
    }
}