using BrawlLib.Internal.Windows.Forms;
using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace BrawlCrate.BrawlManagers.StageManager
{
    public partial class StageInfoControl : UserControl
    {
        private static string[] names =
        {
            "[none]",
            "sora_scene",
            "sora_menu_main",
            "sora_menu_tour",
            "sora_menu_qm",
            "sora_menu_edit",
            "sora_menu_collect_viewer",
            "sora_menu_replay",
            "sora_menu_snap_shot",
            "sora_menu_event",
            "sora_menu_sel_char",
            "sora_menu_sel_stage",
            "sora_menu_game_over",
            "sora_menu_intro",
            "sora_menu_friend_list",
            "sora_menu_watch",
            "sora_menu_name",
            "sora_menu_sel_char_access",
            "sora_menu_rule",
            "sora_menu_simple_ending",
            "sora_minigame",
            "sora_menu_time_result",
            "sora_menu_boot",
            "sora_menu_challenger",
            "sora_menu_title",
            "sora_menu_title_sunset",
            "sora_fig_get_demo",
            "sora_melee",
            "sora_adv_menu_name",
            "sora_adv_menu_visual",
            "sora_adv_menu_sel_char",
            "sora_adv_menu_sel_map",
            "sora_adv_menu_difficulty",
            "sora_adv_menu_game_over",
            "sora_adv_menu_result",
            "sora_adv_menu_save_load",
            "sora_adv_menu_seal",
            "sora_adv_menu_ending",
            "sora_adv_menu_telop",
            "sora_adv_menu_save_point",
            "sora_adv_stage",
            "sora_enemy",
            "st_battles",
            "st_battle",
            "st_config",
            "st_final",
            "st_dolpic",
            "st_mansion",
            "st_mariopast",
            "st_kart",
            "st_donkey",
            "st_jungle",
            "st_pirates",
            "st_oldin",
            "st_norfair",
            "st_orpheon",
            "st_crayon",
            "st_halberd",
            "st_starfox",
            "st_stadium",
            "st_tengan",
            "st_fzero",
            "st_ice",
            "st_gw",
            "st_emblem",
            "st_madein",
            "st_earth",
            "st_palutena",
            "st_famicom",
            "st_newpork",
            "st_village",
            "st_metalgear",
            "st_greenhill",
            "st_pictchat",
            "st_plankton",
            "st_dxshrine",
            "st_dxyorster",
            "st_dxgarden",
            "st_dxonett",
            "st_dxgreens",
            "st_dxrcruise",
            "st_dxbigblue",
            "st_dxcorneria",
            "st_dxpstadium",
            "st_dxzebes",
            "st_stageedit",
            "st_otrain",
            "st_heal",
            "st_homerun",
            "st_targetbreak",
            "st_croll",
            "ft_mario",
            "ft_donkey",
            "ft_link",
            "ft_samus",
            "ft_yoshi",
            "ft_kirby",
            "ft_fox",
            "ft_pikachu",
            "ft_luigi",
            "ft_captain",
            "ft_ness",
            "ft_koopa",
            "ft_peach",
            "ft_zelda",
            "ft_iceclimber",
            "ft_marth",
            "ft_gamewatch",
            "ft_falco",
            "ft_ganon",
            "ft_wario",
            "ft_metaknight",
            "ft_pit",
            "ft_pikmin",
            "ft_lucas",
            "ft_diddy",
            "ft_poke",
            "ft_dedede",
            "ft_lucario",
            "ft_ike",
            "ft_robot",
            "ft_toonlink",
            "ft_snake",
            "ft_sonic",
            "ft_purin",
            "ft_wolf",
            "ft_zako"
        };

        // _relFile should only be null on startup, when no stage is selected.
        // It's OK to have it pointing to a file that doesn't exist.
        private FileInfo _relFile;

        private bool _useRelDescription;

        public void setStageLabels(string v0, string v1, string v2)
        {
            stageFilename.Text = v0;
            stageName.Text = v1;
            stageInfo.Text = v2;
        }

        private void setRelLabels(string v0, string v1, string v2)
        {
            relFilename.Text = v0;
            relName.Text = v1;
            relInfo.Text = v2;
        }

        public string MD5
        {
            set => lblMD5.Text = value;
        }

        public FileInfo RelFile
        {
            get => _relFile;
            set
            {
                _relFile = value;
                refreshRelFile();
            }
        }

        public bool UseRelDescription
        {
            get => _useRelDescription;
            set
            {
                _useRelDescription = value;
                if (_relFile != null)
                {
                    refreshRelFile();
                }
            }
        }

        public StageInfoControl()
        {
            InitializeComponent();
            setStageLabels("", "", "");
            refreshRelFile();
        }

        private string getModuleName(FileInfo relFile)
        {
            FileStream stream = relFile.OpenRead();

            stream.Seek(3, SeekOrigin.Begin);
            int id = stream.ReadByte();

            stream.Close();
            /*return sb.ToString();*/
            string name = names[id];
            if (UseRelDescription)
            {
                foreach (StageModuleConverter.Stage s in StageModuleConverter.StageList)
                {
                    if (s.Filename == name + ".rel")
                    {
                        name = s.Name;
                        break;
                    }
                }
            }

            return name;
        }

        private void refreshRelFile()
        {
            if (_relFile == null)
            {
                // When no stage .pac has been selected
                setRelLabels("", "", "");
                MD5 = "";
                relButton.Visible = false;
            }
            else
            {
                if (_relFile.Exists)
                {
                    // If there is a rel, display the filename/name/size (this is done for .pac files in StageManagerForm)
                    _relFile.Refresh();
                    setRelLabels(_relFile.Name + ":", getModuleName(_relFile), "(" + _relFile.Length + " bytes)");
                    relName.ForeColor = Color.Black;
                    verifyIDs();
                }
                else
                {
                    setRelLabels(_relFile.Name, "doesn't exist", "");
                    relButton.BackColor = DefaultBackColor;
                    relName.ForeColor = Color.Red;
                }
            }
        }

        #region Stage ID verification

        private static int getIdealStageID(string filename)
        {
            if (filename.StartsWith("st_custom"))
            {
                return 0;
            }

            int L = filename.Length;
            if (filename[L - 6] == '_')
            {
                filename = filename.Substring(0, L - 6) + ".rel";
            }

            foreach (StageModuleConverter.Stage s in StageModuleConverter.StageList)
            {
                if (s.Filename == filename)
                {
                    return s.ID;
                }
            }

            return -1;
        }

        private static int getCurrentStageID(FileInfo relFile)
        {
            return stageIDScan(relFile, false);
        }

        private static int stageIDScan(FileInfo relFile, bool fix)
        {
            FileStream stream;
            stream = relFile.Open(FileMode.Open, FileAccess.ReadWrite);

            // search through pointer
            long length = relFile.Length;
            byte[] searchFor = {0x38, 0xa5, 0x00, 0x00, 0x38, 0x80, 0x00};
            int indexToCheck = 0;
            bool found = false;

            int i = 0;
            while (!found && i < length)
            {
                if (stream.ReadByte() == searchFor[indexToCheck])
                {
                    indexToCheck++;
                    if (indexToCheck == searchFor.Length)
                    {
                        if (StageModuleConverter.IndicesToIgnore.Contains(i + 1))
                        {
                            //MessageBox.Show("ignored " + (i + 1));
                            indexToCheck = 0;
                        }
                        else
                        {
                            found = true;
                            if (fix)
                            {
                                stream.WriteByte((byte) getIdealStageID(relFile.Name));
                            }
                        }
                    }
                }
                else
                {
                    indexToCheck = 0;
                }

                i++;
            }

            int b = stream.ReadByte();
            stream.Close();
            return b;
        }

        private void verifyIDs()
        {
            if (_relFile == null)
            {
                return;
            }

            int currentID = getCurrentStageID(_relFile);
            int idealID = getIdealStageID(_relFile.Name);
            if (currentID == idealID)
            {
                relButton.Visible = false;
            }
            else
            {
                relName.ForeColor = Color.Red;
                relInfo.Text = " (over ID " + currentID + ", should be " + idealID + ")";
                relButton.Visible = true;
            }
        }

        #endregion

        private void relButton_Click(object sender, EventArgs e)
        {
            if (_relFile != null && _relFile.Exists)
            {
                stageIDScan(_relFile, true);
            }

            refreshRelFile();
        }
    }
}