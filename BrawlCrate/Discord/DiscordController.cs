using System;
using System.IO;

namespace BrawlCrate.Discord
{
    public static class DiscordController
    {
        public static DiscordRpc.RichPresence presence;
        private static DiscordRpc.EventHandlers handlers;
        private static readonly string applicationId = (Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + "\\Canary") && File.Exists(AppDomain.CurrentDomain.BaseDirectory + "\\Canary\\Active")) ? "545788780980994078" : "545732315658059801";

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
            System.Windows.Forms.MessageBox.Show("Discord Rich Presence Error " + errorCode + "\n\n" + message, "Discord Rich Presence", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
        }
    }
}
