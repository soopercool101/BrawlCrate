using System;
using System.IO;

namespace BrawlCrate.Discord
{
    public class DiscordController
    {
        public DiscordRpc.RichPresence presence;
        private DiscordRpc.EventHandlers handlers;
        public string applicationId = (Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + '\\' + "Canary") && File.Exists(AppDomain.CurrentDomain.BaseDirectory + '\\' + "Canary" + '\\' + "Active")) ? "545788780980994078" : "545732315658059801";
        public string optionalSteamId;

        /// <summary>
        ///     Initializes Discord RPC
        /// </summary>
        public void Initialize()
        {
            presence = new DiscordRpc.RichPresence();
            handlers = new DiscordRpc.EventHandlers
            {
                readyCallback = ReadyCallback
            };
            handlers.disconnectedCallback += DisconnectedCallback;
            handlers.errorCallback += ErrorCallback;
            DiscordRpc.Initialize(applicationId, ref handlers, true, optionalSteamId);
        }

        public void ReadyCallback()
        {
            Console.WriteLine("Discord RPC is ready!");
        }

        public void DisconnectedCallback(int errorCode, string message)
        {
            Console.WriteLine($"Error: {errorCode} - {message}");
        }

        public void ErrorCallback(int errorCode, string message)
        {
            Console.WriteLine($"Error: {errorCode} - {message}");
        }
    }
}
