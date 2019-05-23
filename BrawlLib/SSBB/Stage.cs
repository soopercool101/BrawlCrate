using System;

namespace BrawlLib.SSBB
{
    public class Stage {
        /// <summary>
        /// The stage ID, as used by the module files and the Custom SSS code.
        /// </summary>
        public byte ID { get; private set; }
        /// <summary>
        /// The stage name (e.g. "Flat Zone 2").
        /// </summary>
        public string Name { get; private set; }
        /// <summary>
        /// The .rel filename (e.g. "st_gw.rel").
        /// </summary>
        public string RelName { get; private set; }
        /// <summary>
        /// The name of the .pac file, minus "STG" and anything    
        /// after an underscore (e.g. "gw" or "emblem").
        /// </summary>
        public string PacBasename { get; private set; }

        public bool ContainsPac(string filename) {
            int i = filename.IndexOfAny(new char[] { '.', '_' });
            if (filename.Length < 3 || i < 0) return false;

            string input_basename = filename.Substring(3, i - 3);
            return String.Equals(input_basename.ToLower(), PacBasename.ToLower(), StringComparison.InvariantCultureIgnoreCase);
        }

        public Stage(byte id, string name, string relname, string pac_basename) {
            this.ID = id;
            this.Name = name;
            this.RelName = relname;
            this.PacBasename = pac_basename;
        }

        public override string ToString() { return Name; }

        public string[] PacNames {
            get {
                string s = PacBasename;
                return s == "starfox" ? new string[] { "STGSTARFOX_GDIFF.pac" } :
                        s == "emblem" ? new string[] {
                                "STGEMBLEM_00.pac",
                                "STGEMBLEM_01.pac",
                                "STGEMBLEM_02.pac" } :
                        s == "mariopast" ? new string[] {
                                "STGMARIOPAST_00.pac",
                                "STGMARIOPAST_01.pac" } :
                        s == "metalgear" ? new string[] {
                                "STGMETALGEAR_00.pac",
                                "STGMETALGEAR_01.pac",
                                "STGMETALGEAR_02.pac" } :
                        s == "tengan" ? new string[] {
                                "STGTENGAN_1.pac",
                                "STGTENGAN_2.pac",
                                "STGTENGAN_3.pac" } :
                        s == "village" ? new string[] {
                                "STGVILLAGE_00.pac",
                                "STGVILLAGE_01.pac",
                                "STGVILLAGE_02.pac",
                                "STGVILLAGE_03.pac",
                                "STGVILLAGE_04.pac" } :
                        s == "custom" ? new string[0] :
                        new string[] { "STG" + s.ToUpper() + ".pac" };
            }
        }

        public readonly static Stage[] Stages = new Stage[] {
            //        ID    Display Name              .rel filename        Name without STG
            new Stage(0x00, "STGCUSTOM##.pac",        "st_custom##.rel",   "custom"),
            new Stage(0x01, "Battlefield",            "st_battle.rel",     "battlefield"),
            new Stage(0x02, "Final Destination",      "st_final.rel",      "final"),
            new Stage(0x03, "Delfino Plaza",          "st_dolpic.rel",     "dolpic"),
            new Stage(0x04, "Luigi's Mansion",        "st_mansion.rel",    "mansion"),
            new Stage(0x05, "Mushroomy Kingdom",      "st_mariopast.rel",  "mariopast"),
            new Stage(0x06, "Mario Circuit",          "st_kart.rel",       "kart"),
            new Stage(0x07, "75 m",                   "st_donkey.rel",     "donkey"),
            new Stage(0x08, "Rumble Falls",           "st_jungle.rel",     "jungle"),
            new Stage(0x09, "Pirate Ship",            "st_pirates.rel",    "pirates"),
            new Stage(0x0B, "Norfair",                "st_norfair.rel",    "norfair"),
            new Stage(0x0C, "Frigate Orpheon",        "st_orpheon.rel",    "orpheon"),
            new Stage(0x0D, "Yoshi's Island (Brawl)", "st_crayon.rel",     "crayon"),
            new Stage(0x0E, "Halberd",                "st_halberd.rel",    "halberd"),
            new Stage(0x13, "Lylat Cruise",           "st_starfox.rel",    "starfox"),
            new Stage(0x14, "Pokemon Stadium 2",      "st_stadium.rel",    "stadium"),
            new Stage(0x15, "Spear Pillar",           "st_tengan.rel",     "tengan"),
            new Stage(0x16, "Port Town Aero Dive",    "st_fzero.rel",      "fzero"),
            new Stage(0x17, "Summit",                 "st_ice.rel",        "ice"),
            new Stage(0x18, "Flat Zone 2",            "st_gw.rel",         "gw"),
            new Stage(0x19, "Castle Siege",           "st_emblem.rel",     "emblem"),
            new Stage(0x1C, "WarioWare Inc.",         "st_madein.rel",     "madein"),
            new Stage(0x1D, "Distant Planet",         "st_earth.rel",      "earth"),
            new Stage(0x1E, "Skyworld",               "st_palutena.rel",   "palutena"),
            new Stage(0x1F, "Mario Bros.",            "st_famicom.rel",    "famicom"),
            new Stage(0x20, "New Pork City",          "st_newpork.rel",    "newpork"),
            new Stage(0x21, "Smashville",             "st_village.rel",    "village"),
            new Stage(0x22, "Shadow Moses Island",    "st_metalgear.rel",  "metalgear"),
            new Stage(0x23, "Green Hill Zone",        "st_greenhill.rel",  "greenhill"),
            new Stage(0x24, "PictoChat",              "st_pictchat.rel",   "pictchat"),
            new Stage(0x25, "Hanenbow",               "st_plankton.rel",   "plankton"),
            new Stage(0x26, "ConfigTest",             "st_config.rel",     "configtest"),
            new Stage(0x29, "Temple",                 "st_dxshrine.rel",   "dxshrine"),
            new Stage(0x2A, "Yoshi's Island (Melee)", "st_dxyorster.rel",  "dxyorster"),
            new Stage(0x2B, "Jungle Japes",           "st_dxgarden.rel",   "dxgarden"),
            new Stage(0x2C, "Onett",                  "st_dxonett.rel",    "dxonett"),
            new Stage(0x2D, "Green Greens",           "st_dxgreens.rel",   "dxgreens"),
            new Stage(0x2E, "Pokemon Stadium",        "st_dxpstadium.rel", "dxpstadium"),
            new Stage(0x2F, "Rainbow Cruise",         "st_dxrcruise.rel",  "dxrcruise"),
            new Stage(0x30, "Corneria",               "st_dxcorneria.rel", "dxcorneria"),
            new Stage(0x31, "Big Blue",               "st_dxbigblue.rel",  "dxbigblue"),
            new Stage(0x32, "Brinstar",               "st_dxzebes.rel",    "dxzebes"),
            new Stage(0x33, "Bridge of Eldin",        "st_oldin.rel",      "oldin"),
            new Stage(0x34, "Homerun",                "st_homerun.rel",    "homerun"),
            new Stage(0x35, "Edit",                   "st_stageedit.rel",  "edit"),
            new Stage(0x36, "Heal",                   "st_heal.rel",       "heal"),
            new Stage(0x37, "Online Training",        "st_otrain.rel",     "onlinetraining"),
            new Stage(0x38, "TargetBreak",            "st_tbreak.rel",     "targetlv"),
            new Stage(0x39, "Classic mode credits",   "st_croll.rel",      "chararoll"),
        };
    }
}
