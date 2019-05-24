using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace System
{
    public class AttributeInfo
    {
        public string _name;
        public string _description;
        public int _type;

        //0 == float/radians
        //1 == int
        //2 == float radians to degrees
    }

    public class AttributeInterpretation
    {
        public AttributeInfo[] Array { get; private set; }
        public string Filename { get; private set; }
        public int NumEntries => Array.Length;

        public AttributeInterpretation(AttributeInfo[] array, string saveToFile)
        {
            Array = array;
            Filename = saveToFile;
        }

        public AttributeInterpretation(string filename)
        {
            Filename = filename;

            List<AttributeInfo> list = new List<AttributeInfo>();
            int index = 0x14;
            if (filename != null && File.Exists(filename))
            {
                using (StreamReader sr = new StreamReader(filename))
                {
                    for (int i = 0; !sr.EndOfStream /*&& i < NumEntries*/; i++)
                    {
                        AttributeInfo attr = new AttributeInfo
                        {
                            _name = sr.ReadLine(),
                            _description = sr.ReadLine()
                        };
                        string num = sr.ReadLine();
                        try
                        {
                            attr._type = int.Parse(num);
                        }
                        catch (FormatException ex)
                        {
                            throw new FormatException("Invalid type \"" + num + "\" in " + Path.GetFileName(filename) + ".", ex);
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
                if (DialogResult.Yes != MessageBox.Show("Overwrite " + Filename + "?", "Overwrite",
                    MessageBoxButtons.YesNo))
                {
                    return;
                }
            }
            using (StreamWriter sw = new StreamWriter(Filename))
            {
                foreach (AttributeInfo attr in Array)
                {
                    sw.WriteLine(attr._name);
                    sw.WriteLine(attr._description);
                    sw.WriteLine(attr._type);
                    sw.WriteLine();
                }
            }
        }
    }
}
