using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace BrawlCrate.SSSEditor
{
    public class Resources
    {
        public static string PairList => GetEmbeddedResource("BrawlCrate.SSSEditor.PairList.cshtml");

        public static string About => GetEmbeddedResource("BrawlCrate.SSSEditor.About.html");

        public static string JQuery => GetEmbeddedResource("BrawlCrate.SSSEditor.jquery.min.js");

        private static string GetEmbeddedResource(string fullname)
        {
            Assembly a = Assembly.GetAssembly(typeof(Resources));
            string[] ssd = a.GetManifestResourceNames();
            using (Stream stream = a.GetManifestResourceStream(fullname))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    return reader.ReadToEnd();
                }
            }
        }
    }
}