using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace BrawlCrate.BrawlManagers.CostumeManager
{
    public class PortraitMap
    {
        #region static

        public static string[] KirbyHats =
        {
            "donkey", "falco", "mewtwo", "pikmin", "purin", "snake"
        };

        public class Fighter
        {
            public string Name { get; private set; }

            public int CharBustTexIndex { get; private set; }
            //public int? FighterIndex, CSSSlot;

            public Fighter(string Name, int CharBustTexIndex)
            {
                this.Name = Name;
                this.CharBustTexIndex = CharBustTexIndex;
            }

            public Fighter(string Name, int CharBustTexIndex, int FighterIndex, int CSSSlot)
                : this(Name, CharBustTexIndex)
            {
                //this.FighterIndex = FighterIndex;
                //this.CSSSlot = CSSSlot;
            }
        }

        /// <summary>
        /// Name/index pairs that are known to be used in Brawl or Project M 3.0.
        /// </summary>
        private static Fighter[] KnownFighters =
        {
            new Fighter("mario", 0),
            new Fighter("donkey", 1),
            new Fighter("link", 2),
            new Fighter("samus", 3),
            new Fighter("yoshi", 4),
            new Fighter("kirby", 5),
            new Fighter("fox", 6),
            new Fighter("pikachu", 7),
            new Fighter("luigi", 8),
            new Fighter("captain", 9),
            new Fighter("ness", 10),
            new Fighter("koopa", 11),
            new Fighter("peach", 12),
            new Fighter("zelda", 13),
            new Fighter("sheik", 14),
            new Fighter("popo", 15),
            new Fighter("marth", 16),
            new Fighter("gamewatch", 17),
            new Fighter("falco", 18),
            new Fighter("ganon", 19),
            new Fighter("metaknight", 21),
            new Fighter("pit", 22),
            new Fighter("szerosuit", 23),
            new Fighter("pikmin", 24),
            new Fighter("lucas", 25),
            new Fighter("diddy", 26),
            new Fighter("mewtwo", 27), // PM 3.0
            new Fighter("poketrainer", 27),
            new Fighter("pokelizardon", 28),
            new Fighter("pokezenigame", 29),
            new Fighter("pokefushigisou", 30),
            new Fighter("dedede", 31),
            new Fighter("lucario", 32),
            new Fighter("ike", 33),
            new Fighter("robot", 34),
            new Fighter("purin", 36),
            new Fighter("wario", 37),
            new Fighter("roy", 39), // PM 3.0
            new Fighter("toonlink", 40),
            new Fighter("wolf", 43),
            new Fighter("snake", 45),
            new Fighter("sonic", 46)
        };

        // Fighter, CSSSlot, Cosmetic
        private static readonly int[][] IndexMappings =
        {
            new int[] {0x00, 0x00, 0x00},
            new int[] {0x01, 0x01, 0x01},
            new int[] {0x02, 0x02, 0x02},
            new int[] {0x03, 0x03, 0x03},
            new int[] {0x04, 0x05, 0x04},
            new int[] {0x05, 0x06, 0x05},
            new int[] {0x06, 0x07, 0x06},
            new int[] {0x07, 0x08, 0x07},
            new int[] {0x08, 0x09, 0x08},
            new int[] {0x09, 0x0A, 0x09},
            new int[] {0x0A, 0x0B, 0x0A},
            new int[] {0x0B, 0x0C, 0x0B},
            new int[] {0x0C, 0x0D, 0x0C},
            new int[] {0x0D, 0x0E, 0x0D},
            new int[] {0x0E, 0x0F, 0x0E},
            new int[] {0x0F, 0x10, 0x0F},
            new int[] {0x11, 0x11, 0x10},
            new int[] {0x12, 0x12, 0x11},
            new int[] {0x13, 0x13, 0x12},
            new int[] {0x14, 0x14, 0x13},
            new int[] {0x16, 0x16, 0x15},
            new int[] {0x17, 0x17, 0x16},
            new int[] {0x18, 0x04, 0x17},
            new int[] {0x19, 0x18, 0x18},
            new int[] {0x1A, 0x19, 0x19},
            new int[] {0x1B, 0x1A, 0x1A},
            new int[] {0x1C, 0x1B, 0x1B},
            new int[] {0x1D, 0x1C, 0x1C},
            new int[] {0x1E, 0x1D, 0x1D},
            new int[] {0x1F, 0x1E, 0x1E},
            new int[] {0x20, 0x1F, 0x1F},
            new int[] {0x21, 0x20, 0x20},
            new int[] {0x22, 0x21, 0x21},
            new int[] {0x23, 0x22, 0x22},
            new int[] {0x25, 0x23, 0x23},
            new int[] {0x15, 0x15, 0x14},
            new int[] {0x29, 0x24, 0x25},
            new int[] {0x2C, 0x25, 0x27},
            new int[] {0x2E, 0x26, 0x28},
            new int[] {0x2F, 0x27, 0x29}
        };

        private static int? GetCSSSlot(int fighterIndex)
        {
            if (fighterIndex >= 0x3F)
            {
                return fighterIndex;
            }

            IEnumerable<int> q = from a in IndexMappings
                                 where a[0] == fighterIndex
                                 select a[1];
            if (q.Any())
            {
                return q.First();
            }

            return null;
        }

        private static int? GetCosmeticSlot(int fighterIndex)
        {
            if (fighterIndex >= 0x3F)
            {
                return fighterIndex;
            }

            IEnumerable<int> q = from a in IndexMappings
                                 where a[0] == fighterIndex
                                 select a[2];
            if (q.Any())
            {
                return q.First();
            }

            return null;
        }

        private static Dictionary<int, int[]> PortraitToCostumeMappings = new Dictionary<int, int[]>
        {
            {0, new int[] {0, 6, 3, 4, 5, 2}},
            {1, new int[] {0, 4, 1, 3, 2, 5}},
            {2, new int[] {0, 1, 3, 5, 6, 4}},
            {3, new int[] {0, 3, 1, 5, 4, 2}},
            {4, new int[] {0, 1, 3, 4, 5, 6}},
            {5, new int[] {0, 4, 3, 1, 2, 5}},
            {6, new int[] {0, 4, 1, 2, 3, 5}},
            {7, new int[] {0, 1, 2, 3}},
            {8, new int[] {0, 5, 1, 3, 4, 6}},
            {9, new int[] {0, 4, 1, 2, 3, 5}},
            {10, new int[] {0, 5, 4, 2, 3, 6}},
            {11, new int[] {0, 4, 1, 3, 5, 6}},
            {12, new int[] {0, 5, 1, 3, 2, 4}},
            {13, new int[] {0, 1, 3, 5, 2, 4}},
            {14, new int[] {0, 1, 3, 5, 2, 4}},
            {15, new int[] {0, 1, 3, 4, 2, 5}},
            {16, new int[] {0, 1, 2, 4, 5, 3}},
            {17, new int[] {0, 1, 2, 3, 4, 5}},
            {18, new int[] {0, 5, 3, 1, 2, 4}},
            {19, new int[] {0, 4, 3, 2, 1, 5}},
            {21, new int[] {0, 4, 1, 2, 3, 5}},
            {22, new int[] {0, 4, 1, 2, 3, 5}},
            {23, new int[] {0, 3, 1, 5, 4, 2}},
            {24, new int[] {0, 4, 1, 5, 2, 3}},
            {25, new int[] {0, 4, 1, 3, 2, 5}},
            {26, new int[] {0, 5, 4, 6, 2, 3}},
            {27, new int[] {0, 1, 2, 3, 4}},
            {28, new int[] {0, 1, 2, 3, 4}},
            {29, new int[] {0, 1, 2, 3, 4}},
            {30, new int[] {0, 1, 2, 3, 4}},
            {31, new int[] {0, 6, 2, 5, 3, 4}},
            {32, new int[] {0, 1, 4, 5, 2}},
            {33, new int[] {0, 5, 1, 3, 2, 4}},
            {34, new int[] {0, 6, 5, 4, 3, 2}},
            {36, new int[] {0, 1, 4, 3, 2}},
            {37, new int[] {0, 1, 5, 2, 4, 3, 6, 7, 9, 8, 10, 11}},
            {40, new int[] {0, 1, 3, 4, 5, 6}},
            {43, new int[] {0, 1, 4, 2, 3, 5}},
            {45, new int[] {0, 1, 3, 4, 2, 5}},
            {46, new int[] {0, 5, 4, 2, 1}}
        };

        private static Dictionary<int, int[]> PM35Mappings = CompilePM35Mappings();

        private static Dictionary<int, int[]> CompilePM35Mappings()
        {
            Dictionary<int, int[]> ret = new Dictionary<int, int[]>();
            for (int key = 0; key <= 46; key++)
            {
                switch (key)
                {
                    // Some of these characters have their portraits in a different order than Brawl, or have their additional portraits "out of order."
                    case 0:
                        // Mario
                        ret.Add(key, new int[] {0, 6, 3, 2, 5, 7, 11, 8, 9, 10, /*end*/ 1, 4});
                        break;
                    case 3:
                        // Samus
                        ret.Add(key, new int[] {0, 3, 1, 4, 2, 5, 7, 8, 6});
                        break;
                    case 12:
                        // Fox
                        ret.Add(key, new int[] {0, 5, 1, 3, 2, 4, 7, 6});
                        break;
                    case 23:
                        // Zero Suit Samus
                        ret.Add(key, new int[] {0, 3, 1, 5, 4, 2, 7, 8, 6});
                        break;
                    case 27:
                        // Mewtwo - uses Pokémon Trainer's character index
                        ret.Add(key, new int[] {0, 2, 1, 3, 4, 5});
                        break;
                    case 28:
                        // Charizard
                        ret.Add(key, new int[] {0, 1, 3, 2, 4, 5, 6});
                        break;
                    case 29:
                        // Squirtle
                        ret.Add(key, new int[] {0, 2, 1, 3, 4, 5, 6, 7});
                        break;
                    case 37:
                        // Wario
                        ret.Add(key, new int[] {6, 7, 10, 8, 11, 9, 0, 1, 3, 2, 5, 4});
                        break;
                    case 39:
                        // Roy
                        ret.Add(key, new int[] {0, 2, 1, 3, 4, 5, 6});
                        break;
                    case 46:
                        // Sonic
                        ret.Add(key, new int[] {0, 1, 4, 2, 5, 6, 7, 8});
                        break;
                    default:
                        if (!PortraitToCostumeMappings.ContainsKey(key))
                        {
                            continue;
                        }

                        /* All other characters in PM 3.6 follow a pattern: the portraits start out in the same order Brawl has them,
                           and any additional portraits are in order after the highest-numbered original portrait. */
                        int[] arr1 = PortraitToCostumeMappings[key];
                        int max = arr1.Max();
                        int[] arr2 = new int[12];
                        Array.Copy(arr1, arr2, arr1.Length);
                        for (int i = arr1.Length; i < 12; i++)
                        {
                            arr2[i] = ++max;
                        }

                        ret.Add(key, arr2);
                        break;
                }
            }

            return ret;
        }

        #endregion

        #region special subclasses

        public class ProjectM : PortraitMap
        {
            public ProjectM()
                : base()
            {
                foreach (int i in PM35Mappings.Keys)
                {
                    AddPortraitMappings(i, PM35Mappings[i]);
                }
            }
        }

        public class CBliss : PortraitMap
        {
            public override bool ContainsMapping(int index)
            {
                /* Do not check PortraitToCostumeMappings if cBliss is selected.
                   The program will be "uncertain" of all portrait mappings (yellow label) and
                   it will look for each portrait at the costume index - which is how cBliss works. */
                return additionalMappings.ContainsKey(index);
            }
        }

        #endregion

        private List<Fighter> additionalFighters;
        private Dictionary<int, int[]> additionalMappings;

        public PortraitMap()
        {
            additionalFighters = new List<Fighter>();
            additionalMappings = new Dictionary<int, int[]>();
        }

        public int CharBustTexFor(string name)
        {
            return (from f in additionalFighters
                    where f.Name == name
                    select (int?) f.CharBustTexIndex).FirstOrDefault()
                   ?? (from f in KnownFighters
                       where f.Name == name
                       select (int?) f.CharBustTexIndex).FirstOrDefault()
                   ?? -1;
        }

        public IEnumerable<string> GetKnownFighterNames()
        {
            return (from f in KnownFighters select f.Name)
                   .Concat(from f in additionalFighters select f.Name)
                   .Distinct();
        }

        public virtual bool ContainsMapping(int index)
        {
            return additionalMappings.ContainsKey(index) || PortraitToCostumeMappings.ContainsKey(index);
        }

        public int[] GetPortraitMappings(int charBustTexIndex)
        {
            int[] arr;
            bool b = additionalMappings.TryGetValue(charBustTexIndex, out arr)
                     || PortraitToCostumeMappings.TryGetValue(charBustTexIndex, out arr);
            return arr;
        }

        private int GetCharBustTexIndex(string name)
        {
            IEnumerable<Fighter> q = additionalFighters.Concat(KnownFighters)
                                                       .Where(f => string.Equals(f.Name, name,
                                                           StringComparison.InvariantCultureIgnoreCase));
            if (!q.Any())
            {
                throw new Exception("No known fighter found with name " + name + ".");
            }

            return q.First().CharBustTexIndex;
        }

        public void SetCharBustTexIndex(string name, int index)
        {
            name = name.ToLower();
            additionalFighters.Add(new Fighter(name, index));
            string n = name;
            if (KnownFighters.Any(f => f.Name == name))
            {
                n += " (override)";
            }

            Console.WriteLine(n + ": char_bust_tex index = " + index);
        }

        public void AddPortraitMappings(int charBustTexIndex, int[] colors)
        {
            additionalMappings.Add(charBustTexIndex, colors);
            Console.WriteLine("Added color/portrait mappings for index " + charBustTexIndex);
        }

        public void AddPortraitMappings(string name, int[] colors)
        {
            AddPortraitMappings(GetCharBustTexIndex(name), colors);
        }

        public void ClearAll()
        {
            additionalFighters.Clear();
            additionalMappings.Clear();
        }

        private Dictionary<int, string> whoWasAdded_Debug;

        public void BrawlExScan(string brawlExDir)
        {
            if (whoWasAdded_Debug == null)
            {
                whoWasAdded_Debug = new Dictionary<int, string>();
            }

            if (Directory.Exists(brawlExDir))
            {
                foreach (string fitc in Directory.EnumerateFiles(brawlExDir + "/FighterConfig"))
                {
                    byte id;
                    if (byte.TryParse(fitc.Substring(fitc.Length - 6, 2), NumberStyles.HexNumber, null, out id))
                    {
                        byte[] fitc_data = File.ReadAllBytes(fitc);
                        StringBuilder sb = new StringBuilder();
                        for (int i = 0xb0; i < 0xc0; i++)
                        {
                            char c = (char) fitc_data[i];
                            if (c == 0)
                            {
                                break;
                            }

                            sb.Append(c);
                        }

                        string name = sb.ToString();

                        int? cosmeticIndex = GetCosmeticSlot(id);
                        if (cosmeticIndex != null)
                        {
                            string cosm = brawlExDir + "/CosmeticConfig/Cosmetic" + cosmeticIndex.Value.ToString("X2") +
                                          ".dat";
                            if (File.Exists(cosm))
                            {
                                Console.WriteLine("Cosmetic" + cosmeticIndex.Value.ToString("X2"));
                                byte[] cosm_data = File.ReadAllBytes(cosm);
                                SetCharBustTexIndex(name, cosm_data[0x10]);
                            }
                        }

                        int? cssSlotIndex = GetCSSSlot(id);
                        if (cssSlotIndex != null)
                        {
                            string cssc = brawlExDir + "/CSSSlotConfig/CSSSlot" + cssSlotIndex.Value.ToString("X2") +
                                          ".dat";
                            if (File.Exists(cssc))
                            {
                                byte[] cssc_data = File.ReadAllBytes(cssc);
                                List<int> colors = new List<int>();
                                for (int i = 0x20; i < 0x40; i += 2)
                                {
                                    if (cssc_data[i] == 0x0c)
                                    {
                                        break;
                                    }

                                    colors.Add(cssc_data[i + 1]);
                                }

                                try
                                {
                                    AddPortraitMappings(name, colors.ToArray());
                                }
                                catch (ArgumentException)
                                {
                                    string oldname = null;
                                    int i = GetCharBustTexIndex(name);
                                    whoWasAdded_Debug.TryGetValue(i, out oldname);
                                    System.Windows.Forms.MessageBox.Show("Could not add mappings for CSSSlot" +
                                                                         cssSlotIndex.Value.ToString("X2") +
                                                                         ".dat (" + name +
                                                                         ") - mappings were already added for " +
                                                                         (oldname ?? "-null-") +
                                                                         " (char_bust_tex index " + i + ")");
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}