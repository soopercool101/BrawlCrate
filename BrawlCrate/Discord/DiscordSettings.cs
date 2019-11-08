using BrawlCrate.BrawlManagers.CostumeManager;
using BrawlCrate.BrawlManagers.SongManager;
using BrawlCrate.BrawlManagers.StageManager;
using BrawlCrate.UI;
using BrawlLib.Internal;
using BrawlLib.Internal.Windows.Forms;
using BrawlLib.SSBB.ResourceNodes;
using System;
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
        private static readonly string UserPickedImageKey = "";
        private static ModNameType _modNameType = ModNameType.Disabled;
        private static readonly string WorkString = "Working on";
        private static string _userNamedMod = "My Mod";
        private static readonly bool ShowTimeElapsed = true;

        private static bool _controllerSet;
        public static bool DiscordControllerSet => _controllerSet;
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
                _controllerSet = true;
            }

            DiscordController.presence = new DiscordRpc.RichPresence
            {
                smallImageKey = UserPickedImageKey,
                smallImageText = "",
#if CANARY
                largeImageKey = "canary",
                largeImageText = Program.AssemblyTitleShort
#else
                largeImageKey = "brawlcrate",
                largeImageText = Program.AssemblyTitleFull
#endif
            };
            ResourceNode root = MainForm.Instance?.RootNode?.Resource;
            string rootName = root?.Name ?? "<null>";
            bool hasGct = (rootName?.EndsWith(".gct") ?? false) || (rootName?.EndsWith(".txt") ?? false);
            bool usingManager = false;
            GCTEditor gctEditor = null;
            if (!hasGct)
            {
                FormCollection fc = Application.OpenForms;
                foreach (Form frm in fc)
                {
                    if (frm is CostumeManagerForm)
                    {
                        DiscordController.presence.details = "Managing Costumes";
                        usingManager = true;
                        break;
                    }

                    if (frm is SongManagerForm)
                    {
                        DiscordController.presence.details = "Managing Songs";
                        usingManager = true;
                        break;
                    }

                    if (frm is StageManagerForm)
                    {
                        DiscordController.presence.details = "Managing Stages";
                        usingManager = true;
                        break;
                    }

                    if (!(frm is GCTEditor editor))
                    {
                        continue;
                    }

                    hasGct = true;
                    gctEditor = editor;
                    break;
                }
            }

            if (!usingManager)
            {
                if (hasGct)
                {
                    DiscordController.presence.details = "Managing codes";
                }
                else if (root == null)
                {
                    DiscordController.presence.details = "Idling";
                }
                else if (Program.RootPath == null)
                {
                    if (((ARCNode) root).IsSubspace)
                    {
                        DiscordController.presence.details = WorkString + " a subspace stage";
                    }
                    else if (root is ARCNode && ((ARCNode) root).IsStage)
                    {
                        if (rootName.StartsWith("STGRESULT", StringComparison.OrdinalIgnoreCase))
                        {
                            DiscordController.presence.details = WorkString + " the results screen";
                        }
                        else
                        {
                            DiscordController.presence.details = WorkString + " a stage";
                        }
                    }
                    else
                    {
                        DiscordController.presence.details = WorkString + " a new mod";
                    }
                }
                else if (root is ARCNode)
                {
                    if (((ARCNode) root).IsSubspace)
                    {
                        DiscordController.presence.details = WorkString + " a subspace stage";
                    }
                    else if (((ARCNode) root).IsStage)
                    {
                        if (rootName.StartsWith("STGRESULT", StringComparison.OrdinalIgnoreCase))
                        {
                            DiscordController.presence.details = WorkString + " the results screen";
                        }
                        else
                        {
                            DiscordController.presence.details = WorkString + " a stage";
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
                            DiscordController.presence.details = WorkString + " a costume";
                        }
                        else if (rootName.Contains("motion", StringComparison.OrdinalIgnoreCase))
                        {
                            DiscordController.presence.details = WorkString + " animations";
                        }
                        else
                        {
                            DiscordController.presence.details = WorkString + " a fighter";
                        }
                    }
                    else if (rootName.StartsWith("sc_", StringComparison.OrdinalIgnoreCase) ||
                             rootName.StartsWith("common5", StringComparison.OrdinalIgnoreCase) ||
                             rootName.StartsWith("mu_", StringComparison.OrdinalIgnoreCase))
                    {
                        DiscordController.presence.details = WorkString + " menus";
                    }
                    else if (rootName.StartsWith("info", StringComparison.OrdinalIgnoreCase))
                    {
                        DiscordController.presence.details = WorkString + " UI";
                    }
                    else if (Program.RootPath.Substring(0, Program.RootPath.LastIndexOf('\\'))
                                    .EndsWith("\\stage\\adventure"))
                    {
                        DiscordController.presence.details = WorkString + " a subspace stage";
                    }
                    else if (rootName.StartsWith("common2", StringComparison.OrdinalIgnoreCase))
                    {
                        DiscordController.presence.details = WorkString + " single player";
                    }
                    else if (rootName.StartsWith("common3", StringComparison.OrdinalIgnoreCase))
                    {
                        DiscordController.presence.details = WorkString + " items";
                    }
                    else if (rootName.StartsWith("common", StringComparison.OrdinalIgnoreCase))
                    {
                        DiscordController.presence.details = WorkString + " animations";
                    }
                    else if (rootName.StartsWith("home_", StringComparison.OrdinalIgnoreCase)
                             && Program.RootPath.EndsWith("\\system\\homebutton"))
                    {
                        DiscordController.presence.details = WorkString + " the home menu";
                    }
                    else if (rootName.Equals("cs_pack", StringComparison.OrdinalIgnoreCase))
                    {
                        DiscordController.presence.details = WorkString + " coin launcher";
                    }
                    else if ((MainForm.Instance.RootNode.Name.StartsWith("Itm") ||
                              Program.RootPath.Substring(0, Program.RootPath.LastIndexOf('\\')).EndsWith("\\item") ||
                              Program.RootPath.Substring(0, Program.RootPath.LastIndexOf('\\'))
                                     .Substring(0, Program.RootPath.LastIndexOf('\\')).EndsWith("\\item"))
                             && (rootName.EndsWith("Brres") || rootName.EndsWith("Param")))
                    {
                        DiscordController.presence.details = WorkString + " an item";
                    }
                    else
                    {
                        DiscordController.presence.details = WorkString + " a mod";
                    }
                }
                else if (root is RELNode)
                {
                    DiscordController.presence.details = WorkString + " a module";
                }
                else if (root is RSTMNode)
                {
                    DiscordController.presence.details = WorkString + " a BRSTM";
                }
                else
                {
                    DiscordController.presence.details = WorkString + " a mod";
                }

                if (hasGct || MainForm.Instance.RootNode != null)
                {
                    switch (_modNameType)
                    {
                        case ModNameType.UserDefined:
                            DiscordController.presence.state = _userNamedMod;
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
                                    : Program.RootPath.Substring(Program.RootPath.LastIndexOf('\\') + 1,
                                        Program.RootPath.LastIndexOf('\\') -
                                        Program.RootPath.LastIndexOf('.'));
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
            }

            if (ShowTimeElapsed)
            {
                DiscordController.presence.startTimestamp = startTime;
            }

            DiscordRpc.UpdatePresence(DiscordController.presence);
        }

        public static void LoadSettings(bool update = false)
        {
            if (Properties.Settings.Default.DiscordRPCEnabled && !DiscordControllerSet)
            {
                DiscordController.Initialize();
            }

            _enabled = Properties.Settings.Default.DiscordRPCEnabled;
            _controllerSet = _enabled;
            if (Properties.Settings.Default.DiscordRPCNameType == null)
            {
                Properties.Settings.Default.DiscordRPCNameType = ModNameType.Disabled;
                Properties.Settings.Default.Save();
            }

            _modNameType = Properties.Settings.Default.DiscordRPCNameType ?? ModNameType.Disabled;
            _userNamedMod = Properties.Settings.Default.DiscordRPCNameCustom;
            if (update)
            {
                Update();
            }
        }
    }
}