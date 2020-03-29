using System.Collections.Generic;

namespace BrawlLib.Internal.Windows.Controls.Hex_Editor
{
    internal static class Util
    {
        /// <summary>
        /// Contains true, if we are in design mode of Visual Studio
        /// </summary>
        private static readonly bool _designMode;

        /// <summary>
        /// Initializes an instance of Util class
        /// </summary>
        static Util()
        {
            // design mode is true if host process is: Visual Studio, Visual Studio Express Versions (C#, VB, C++) or SharpDevelop
            List<string> designerHosts = new List<string>
                {"devenv", "vcsexpress", "vbexpress", "vcexpress", "sharpdevelop"};
            using (System.Diagnostics.Process process = System.Diagnostics.Process.GetCurrentProcess())
            {
                string processName = process.ProcessName.ToLower();
                _designMode = designerHosts.Contains(processName);
            }
        }

        /// <summary>
        /// Gets true, if we are in design mode of Visual Studio
        /// </summary>
        /// <remarks>
        /// In Visual Studio 2008 SP1 the designer is crashing sometimes on windows forms. 
        /// The DesignMode property of Control class is buggy and cannot be used, so use our own implementation instead.
        /// </remarks>
        public static bool DesignMode => _designMode;
    }
}