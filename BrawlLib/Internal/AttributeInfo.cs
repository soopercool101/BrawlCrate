using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace BrawlLib.Internal
{
    public class AttributeInfo
    {
        public string _name;
        public string _description;
        public int _type;

        //0 == float/radians
        //1 == int
        //2 == float radians to degrees
        //3 == color
        //4 == unknown (parse as hexadecimal)
        //5 == flags (parse as binary)
    }

    public class AttributeInterpretation
    {
        public AttributeInfo[] Array { get; private set; }
        public string Filename { get; private set; }
        public int NumEntries => Array != null ? Array.Length : 0;

        public AttributeInterpretation(AttributeInfo[] array, string saveToFile)
        {
            Array = array;
            Filename = saveToFile;
        }

        public AttributeInterpretation(string filename, int index)
        {
            Filename = filename;

            List<AttributeInfo> list = new List<AttributeInfo>();
            if (filename != null && File.Exists(filename))
            {
                using (StreamReader sr = new StreamReader(filename))
                {
                    for (int i = 0; !sr.EndOfStream; i++)
                    {
                        AttributeInfo attr = new AttributeInfo
                        {
                            _name = sr.ReadLine(),
                            _description = ""
                        };
                        string temp = sr.ReadLine();
                        while (temp != null && !temp.Equals("\t/EndDescription"))
                        {
                            attr._description += temp;
                            attr._description += '\n';
                            temp = sr.ReadLine();
                        }

                        if (temp == null)
                        {
                            return;
                        }

                        if (attr._description.EndsWith("\n"))
                        {
                            attr._description = attr._description.Substring(0, attr._description.Length - 1);
                        }

                        string num = sr.ReadLine();
                        try
                        {
                            attr._type = int.Parse(num);
                        }
                        catch (FormatException ex)
                        {
                            throw new FormatException(
                                "Invalid type \"" + num + "\" in " + Path.GetFileName(filename) + ".", ex);
                        }

                        if (attr._description == "")
                        {
                            attr._description = "No Description Available.";
                        }

                        list.Add(attr);
                        sr.ReadLine();
                        index++;
                    }
                }
            }

            Array = list.ToArray();
        }

        public override string ToString()
        {
            return Path.GetFileNameWithoutExtension(Filename);
        }

        public void Save()
        {
            string dir = Path.GetDirectoryName(Filename);
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            if (File.Exists(Filename))
            {
                if (MessageBox.Show("Overwrite " + Filename + "?", "Overwrite", MessageBoxButtons.YesNo)
                    != DialogResult.Yes)
                {
                    return;
                }
            }

            using (StreamWriter sw = new StreamWriter(Filename))
            {
                for (int i = 0; i < Array.Length; i++)
                {
                    AttributeInfo attr = Array[i];
                    sw.WriteLine(attr._name);
                    sw.WriteLine(attr._description);
                    sw.WriteLine("\t/EndDescription");
                    if (i == Array.Length - 1)
                    {
                        sw.Write(attr._type);
                    }
                    else
                    {
                        sw.WriteLine(attr._type);
                        sw.WriteLine();
                    }
                }

                sw.Close();
            }
        }
    }
}