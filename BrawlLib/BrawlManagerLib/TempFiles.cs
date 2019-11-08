using BrawlLib.Internal.IO;
using BrawlLib.SSBB.ResourceNodes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;

namespace BrawlLib.BrawlManagerLib
{
    public class TempFileCleanupException : AggregateException
    {
        public TempFileCleanupException(IEnumerable<Exception> exceptions)
            : base("One or more errors occured while attempting to remove BrawlManagerLib's temporary files.",
                exceptions)
        {
        }
    }

    public class TempFiles
    {
        public static readonly string SUBDIR = "BrawlManagerLib-" + Guid.NewGuid();

        static TempFiles()
        {
            string p = Path.Combine(Path.GetTempPath(), SUBDIR);
            if (Directory.Exists(p))
            {
                try
                {
                    Directory.Delete(p, true);
                }
                catch (IOException)
                {
                    Console.WriteLine("could not clear out " + p + " (files probably in use)");
                }
            }

            Directory.CreateDirectory(p);
        }

        /// <summary>
        /// Creates a temporary file in the BrawlManagerLib subdirectory of the system temporary folder.
        /// This function can be used for image files. For files that can be opened as a ResourceNode, consider using the MakeTempNode function instead.
        /// </summary>
        public static string Create(string extension)
        {
            if (!extension.StartsWith("."))
            {
                extension = "." + extension;
            }

            return Path.Combine(Path.GetTempPath(), SUBDIR, Guid.NewGuid() + extension);
        }

        /// <summary>
        /// Deletes all files created by the Create function (but not those created by MakeTempNode.)
        /// </summary>
        public static void DeleteAll()
        {
            List<Exception> exceptions = new List<Exception>();
            try
            {
                foreach (string s in Directory.EnumerateFiles(Path.Combine(Path.GetTempPath(), SUBDIR)))
                {
                    try
                    {
                        File.Delete(s);
                    }
                    catch (Exception e)
                    {
                        exceptions.Add(e);
                    }
                }
            }
            catch (Exception e)
            {
                exceptions.Add(e);
            }

            if (exceptions.Count == 0)
            {
                try
                {
                    Directory.Delete(Path.Combine(Path.GetTempPath(), SUBDIR));
                }
                catch (Exception e)
                {
                    exceptions.Add(e);
                }
            }

            //if (exceptions.Count > 0)
            //{
            //    throw new TempFileCleanupException(exceptions);
            //}
        }

        /// <summary>
        /// Creates a ResourceNode from a temporary file using FileMap.FromTempFile. The file will be deleted when the underlying FileMap's stream is closed.
        /// </summary>
        public static ResourceNode MakeTempNode(string path)
        {
            byte[] data = File.ReadAllBytes(path);

            FileMap map = FileMap.FromTempFile(data.Length);
            Console.WriteLine(path + " -> FromTempFile -> " + map.FilePath);
            Marshal.Copy(data, 0, map.Address, data.Length);
            return NodeFactory.FromSource(null, new DataSource(map));
        }
    }
}