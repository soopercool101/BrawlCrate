using System;
using System.IO;
using System.Windows.Forms;

namespace BrawlCrate.Discord
{
    public static class DiscordController
    {
        public static DiscordRpc.RichPresence presence;
        private static DiscordRpc.EventHandlers handlers;

        private static readonly string applicationId =
            Directory.Exists(Application.StartupPath + "\\Canary") &&
            File.Exists(Application.StartupPath + "\\Canary\\Active")
                ? "545788780980994078"
                : "545732315658059801";

        /// <summary>
        ///     Initializes Discord RPC
        /// </summary>
        public static void Initialize()
        {
            DiscordRpc.ClearPresence();
            DiscordRpc.Shutdown();
            presence = new DiscordRpc.RichPresence();
            handlers = new DiscordRpc.EventHandlers
            {
                readyCallback = ReadyCallback
            };
            handlers.disconnectedCallback += DisconnectedCallback;
            handlers.errorCallback += ErrorCallback;
            DiscordRpc.Initialize(applicationId, ref handlers, true, "");
        }

        public static void ReadyCallback()
        {
            Console.WriteLine("Discord RPC is ready!");
        }

        public static void DisconnectedCallback(int errorCode, string message)
        {
            Console.WriteLine($"Disconnected Error: {errorCode} - {message}");
        }

        public static void ErrorCallback(int errorCode, string message)
        {
            MessageBox.Show("Discord Rich Presence Error " + errorCode + "\n\n" + message,
                "Discord Rich Presence", MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }
}