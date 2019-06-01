using BrawlLib.SSBB.ResourceNodes;
using System;
using System.Linq;

namespace BrawlCrate.Discord
{
    internal class DiscordSettings
    {
        public enum ModNameType
        {
            Disabled,
            UserDefined,
            AutoInternal,
            AutoExternal
        }

        // Fields to be saved between runs
        public static bool enabled = true;
        public static string userPickedImageKey = "brawlcrate";
        public static ModNameType modNameType = ModNameType.Disabled;
        public static string workString = "Working on";
        public static string userNamedMod = "My Mod";
        public static bool showTimeElapsed = true;
        public static DiscordController DiscordController;

        public static void Update()
        {
            if (!enabled)
            {
                DiscordRpc.ClearPresence();
                DiscordRpc.Shutdown();
                return;
            }

            if (enabled && DiscordController == null)
            {
                DiscordController = new BrawlCrate.Discord.DiscordController();
                DiscordController.Initialize();
            }
            DiscordController.presence = new DiscordRpc.RichPresence()
            {
                smallImageKey = "",
                smallImageText = "",
                largeImageKey = "brawlcrate",
                largeImageText = Program.AssemblyTitle
            };
            ResourceNode root = MainForm.Instance.RootNode?.Resource;
            string rootName = root?.Name;
            if (MainForm.Instance.RootNode == null)
            {
                DiscordController.presence.details = "Idling";
            }
            else if (Program.RootPath == null)
            {
                if (root is ARCNode && ((ARCNode)root).IsStage)
                {
                    if (rootName.StartsWith("STGRESULT", StringComparison.OrdinalIgnoreCase))
                    {
                        DiscordController.presence.details = workString + " the results screen";
                    }
                    else
                    {
                        DiscordController.presence.details = workString + " a stage";
                    }
                }
                else
                {
                    DiscordController.presence.details = workString + " a new mod";
                }
            }
            else if (root is ARCNode)
            {
                if (((ARCNode)root).IsStage)
                {
                    if (rootName.StartsWith("STGRESULT", StringComparison.OrdinalIgnoreCase))
                    {
                        DiscordController.presence.details = workString + " the results screen";
                    }
                    else
                    {
                        DiscordController.presence.details = workString + " a stage";
                    }
                }
                else if (((ARCNode)root).IsFighter)
                {
                    if (rootName.EndsWith("0") ||
                       rootName.EndsWith("1") ||
                       rootName.EndsWith("2") ||
                       rootName.EndsWith("3") ||
                       rootName.EndsWith("4") ||
                       rootName.EndsWith("5") ||
                       rootName.EndsWith("6") ||
                       rootName.EndsWith("7") ||
                       rootName.EndsWith("8") ||
                       rootName.EndsWith("9"))
                    {
                        DiscordController.presence.details = workString + " a costume";
                    }
                    else if (rootName.Contains("motion", StringComparison.OrdinalIgnoreCase))
                    {
                        DiscordController.presence.details = workString + " animations";
                    }
                    else
                    {
                        DiscordController.presence.details = workString + " a fighter";
                    }
                }
                else if (rootName.StartsWith("sc_", StringComparison.OrdinalIgnoreCase) ||
                        rootName.StartsWith("common5", StringComparison.OrdinalIgnoreCase) ||
                        rootName.StartsWith("mu_", StringComparison.OrdinalIgnoreCase))
                {
                    DiscordController.presence.details = workString + " menus";
                }
                else if (rootName.StartsWith("info", StringComparison.OrdinalIgnoreCase))
                {
                    DiscordController.presence.details = workString + " UI";
                }
                else if (Program.RootPath.Substring(0, Program.RootPath.LastIndexOf('\\')).EndsWith("\\stage\\adventure"))
                {
                    DiscordController.presence.details = workString + " a subspace stage";
                }
                else if (rootName.StartsWith("common2", StringComparison.OrdinalIgnoreCase))
                {
                    DiscordController.presence.details = workString + " single player";
                }
                else if (rootName.StartsWith("common3", StringComparison.OrdinalIgnoreCase))
                {
                    DiscordController.presence.details = workString + " items";
                }
                else if (rootName.StartsWith("common", StringComparison.OrdinalIgnoreCase))
                {
                    DiscordController.presence.details = workString + " animations";
                }
                else if (rootName.StartsWith("home_", StringComparison.OrdinalIgnoreCase)
                    && Program.RootPath.Substring(0, Program.RootPath.LastIndexOf('\\')).EndsWith("\\system\\homebutton"))
                {
                    DiscordController.presence.details = workString + " the home menu";
                }
                else if (rootName.Equals("cs_pack", StringComparison.OrdinalIgnoreCase))
                {
                    DiscordController.presence.details = workString + " coin launcher";
                }
                else if ((MainForm.Instance.RootNode.Name.StartsWith("Itm") || (Program.RootPath.Substring(0, Program.RootPath.LastIndexOf('\\')).EndsWith("\\item") ||
                    Program.RootPath.Substring(0, Program.RootPath.LastIndexOf('\\')).Substring(0, Program.RootPath.LastIndexOf('\\')).EndsWith("\\item")))
                    && (rootName.EndsWith("Brres") || rootName.EndsWith("Param")))
                {
                    DiscordController.presence.details = workString + " an item";
                }
                else
                {
                    DiscordController.presence.details = workString + " a mod";
                }
            }
            else if (root is RELNode)
            {
                DiscordController.presence.details = workString + " a module";
            }
            else if (root is RSTMNode)
            {
                DiscordController.presence.details = workString + " a BRSTM";
            }
            else
            {
                DiscordController.presence.details = workString + " a mod";
            }

            if (MainForm.Instance.RootNode != null)
            {
                string tabName = MainForm.Instance.RootNode.Text;
                switch (modNameType)
                {
                    case ModNameType.Disabled:
                        DiscordController.presence.state = "";
                        break;
                    case ModNameType.UserDefined:
                        DiscordController.presence.state = userNamedMod;
                        break;
                    case ModNameType.AutoInternal:
                        DiscordController.presence.state = ((MainForm.Instance.RootNode == null || MainForm.Instance.RootNode.Name == null || rootName.Equals("<null>", StringComparison.OrdinalIgnoreCase)) ? "" : rootName);
                        break;
                    case ModNameType.AutoExternal:
                        DiscordController.presence.state = ((Program.RootPath == null || Program.RootPath == "") ? "" : Program.RootPath.Substring(Program.RootPath.LastIndexOf('\\') + 1));
                        break;
                    default:
                        DiscordController.presence.state = "";
                        break;
                }
            }

            if (showTimeElapsed)
            {
                DiscordController.presence.startTimestamp = startTime;
            }

            DiscordRpc.UpdatePresence(DiscordController.presence);
        }

        public static void LoadSettings(bool update = false)
        {
            if (BrawlCrate.Properties.Settings.Default.DiscordRPCEnabled && DiscordController == null)
            {
                DiscordController = new BrawlCrate.Discord.DiscordController();
                DiscordController.Initialize();
            }
            else if ((enabled != BrawlCrate.Properties.Settings.Default.DiscordRPCEnabled && enabled == false))
            {
                DiscordController.Initialize();
            }

            enabled = BrawlCrate.Properties.Settings.Default.DiscordRPCEnabled;
            modNameType = BrawlCrate.Properties.Settings.Default.DiscordRPCNameType;
            userNamedMod = BrawlCrate.Properties.Settings.Default.DiscordRPCNameCustom;
            if (update)
            {
                Update();
            }
        }

        //Temporary, don't save to config
        public static string lastFileOpened = null;
        public static long startTime;
    }
}