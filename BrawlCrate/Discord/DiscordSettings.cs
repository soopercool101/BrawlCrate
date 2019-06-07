using BrawlLib.SSBB.ResourceNodes;
using System;
using System.Linq;
using System.Windows.Forms;

namespace BrawlCrate.Discord
{
    internal static class DiscordSettings
    {
        public enum ModNameType
        {
            Disabled,
            UserDefined,
            AutoInternal,
            AutoExternal
        }

        // Fields to be saved between runs
        private static bool _enabled = true;
        private static string userPickedImageKey = "";
        private static ModNameType modNameType = ModNameType.Disabled;
        private static readonly string workString = "Working on";
        private static string userNamedMod = "My Mod";
        private static bool showTimeElapsed = true;

        private static bool controllerSet;
        public static bool DiscordControllerSet => controllerSet;
        public static bool DiscordRPCEnabled => _enabled;

        // Should be initialized when the program starts
        public static readonly long startTime = (long) DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;

        public static void Update()
        {
            if (!_enabled)
            {
                DiscordRpc.ClearPresence();
                DiscordRpc.Shutdown();
                return;
            }

            if (!DiscordControllerSet)
            {
                DiscordController.Initialize();
                controllerSet = true;
            }

            DiscordController.presence = new DiscordRpc.RichPresence
            {
                smallImageKey = userPickedImageKey,
                smallImageText = "",
                largeImageKey = "brawlcrate",
                largeImageText = Program.AssemblyTitle
            };
            ResourceNode root = MainForm.Instance?.RootNode?.Resource;
            string rootName = root?.Name;
            bool hasGct = (rootName?.EndsWith(".gct") ?? false) || (rootName?.EndsWith(".txt") ?? false);
            GCTEditor gctEditor = null;
            if (!hasGct)
            {
                FormCollection fc = Application.OpenForms;
                foreach (Form frm in fc)
                {
                    if (frm is GCTEditor)
                    {
                        hasGct = true;
                        gctEditor = frm as GCTEditor;
                        break;
                    }
                }
            }

            if (hasGct)
            {
                DiscordController.presence.details = workString + " codes";
            }
            else if (root == null)
            {
                DiscordController.presence.details = "Idling";
            }
            else if (Program.RootPath == null)
            {
                if (root is ARCNode && ((ARCNode) root).IsStage)
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
                if (((ARCNode) root).IsStage)
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
                else if (((ARCNode) root).IsFighter)
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
                else if (Program.RootPath.Substring(0, Program.RootPath.LastIndexOf('\\'))
                                .EndsWith("\\stage\\adventure"))
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
                         && Program.RootPath.Substring(0, Program.RootPath.LastIndexOf('\\'))
                                   .EndsWith("\\system\\homebutton"))
                {
                    DiscordController.presence.details = workString + " the home menu";
                }
                else if (rootName.Equals("cs_pack", StringComparison.OrdinalIgnoreCase))
                {
                    DiscordController.presence.details = workString + " coin launcher";
                }
                else if ((MainForm.Instance.RootNode.Name.StartsWith("Itm") ||
                          Program.RootPath.Substring(0, Program.RootPath.LastIndexOf('\\')).EndsWith("\\item") ||
                          Program.RootPath.Substring(0, Program.RootPath.LastIndexOf('\\'))
                                 .Substring(0, Program.RootPath.LastIndexOf('\\')).EndsWith("\\item"))
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

            if (hasGct || MainForm.Instance.RootNode != null)
            {
                switch (modNameType)
                {
                    case ModNameType.UserDefined:
                        DiscordController.presence.state = userNamedMod;
                        break;
                    case ModNameType.AutoInternal:
                        if (gctEditor != null)
                        {
                            DiscordController.presence.state = gctEditor.TargetNode?.Name ?? "";
                        }
                        else if (hasGct)
                        {
                            DiscordController.presence.state = string.IsNullOrEmpty(Program.RootPath)
                                ? ""
                                : Program.RootPath.Substring(Program.RootPath.LastIndexOf('\\') + 1, Program.RootPath.LastIndexOf('\\') - Program.RootPath.LastIndexOf('.'));
                        }
                        else
                        {
                            DiscordController.presence.state =
                                MainForm.Instance.RootNode == null || MainForm.Instance.RootNode.Name == null ||
                                rootName.Equals("<null>", StringComparison.OrdinalIgnoreCase)
                                    ? ""
                                    : rootName;
                        }
                        break;
                    case ModNameType.AutoExternal:
                        if (gctEditor != null)
                        {
                            try
                            {
                                DiscordController.presence.state =
                                    gctEditor.TargetNode._origPath.Substring(
                                        gctEditor.TargetNode._origPath.LastIndexOf('\\') + 1);
                            }
                            catch
                            {
                                DiscordController.presence.state = "";
                            }
                        }
                        else
                        {
                            DiscordController.presence.state = string.IsNullOrEmpty(Program.RootPath)
                                ? ""
                                : Program.RootPath.Substring(Program.RootPath.LastIndexOf('\\') + 1);
                        }
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
            if ((Properties.Settings.Default.DiscordRPCEnabled && !DiscordControllerSet) || (_enabled != Properties.Settings.Default.DiscordRPCEnabled && _enabled == false))
            {
                DiscordController.Initialize();
                controllerSet = true;
            }

            _enabled = Properties.Settings.Default.DiscordRPCEnabled;
            if (Properties.Settings.Default.DiscordRPCNameType == null)
            {
                Properties.Settings.Default.DiscordRPCNameType = ModNameType.Disabled;
                Properties.Settings.Default.Save();
            }

            modNameType = Properties.Settings.Default.DiscordRPCNameType ?? ModNameType.Disabled;
            userNamedMod = Properties.Settings.Default.DiscordRPCNameCustom;
            if (update)
            {
                Update();
            }
        }
    }
}