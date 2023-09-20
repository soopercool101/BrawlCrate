using BrawlLib.Imaging;
using BrawlLib.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace BrawlLib.SSBB.ResourceNodes
{
    public abstract class StageTableNode : ARCEntryNode, MultipleInterpretationIAttributeList
    {
        internal virtual int EntrySize => 4;

        internal virtual string DocumentationSubDirectory => "";

        internal int unk0;
        internal int unk1;
        internal int unk2;
        internal int _entryOffset;

        [Browsable(false)]
        [Category("Stage Data Table")]
        public int NumEntries => EntryList.Count / EntrySize;

        [Category("Stage Data Table")]
        public virtual int Unknown0
        {
            get => unk0;
            set
            {
                unk0 = value;
                SignalPropertyChange();
            }
        }

        [Category("Stage Data Table")]
        public virtual int Unknown1
        {
            get => unk1;
            set
            {
                unk1 = value;
                SignalPropertyChange();
            }
        }

        [Category("Stage Data Table")]
        public virtual int Unknown2
        {
            get => unk2;
            set
            {
                unk2 = value;
                SignalPropertyChange();
            }
        }

        [Category("Stage Data Table")]
        [Browsable(false)]
        internal virtual int EntryOffset
        {
            get => _entryOffset;
            set
            {
                _entryOffset = value;
                SignalPropertyChange();
            }
        }

        public override int OnCalculateSize(bool force)
        {
            return EntryOffset + EntryList.Count;
        }

        [Browsable(false)] public List<byte> EntryList = new List<byte>();

        public void SetFloat(int index, float value)
        {
            float oldValue = BitConverter.ToSingle(EntryList.ToArray(), index * 4);
            if (oldValue != value)
            {
                byte[] newValue = BitConverter.GetBytes(value);
                EntryList[index * 4] = newValue[0];
                EntryList[index * 4 + 1] = newValue[1];
                EntryList[index * 4 + 2] = newValue[2];
                EntryList[index * 4 + 3] = newValue[3];
                SignalPropertyChange();
            }
        }

        public float GetFloat(int index)
        {
            return BitConverter.ToSingle(EntryList.ToArray(), index * 4);
        }

        public void SetDegrees(int index, float value)
        {
            float oldValue = BitConverter.ToSingle(EntryList.ToArray(), index * 4);
            if (oldValue != value * Maths._deg2radf)
            {
                byte[] newValue = BitConverter.GetBytes(value * Maths._deg2radf);
                EntryList[index * 4] = newValue[0];
                EntryList[index * 4 + 1] = newValue[1];
                EntryList[index * 4 + 2] = newValue[2];
                EntryList[index * 4 + 3] = newValue[3];
                SignalPropertyChange();
            }
        }

        public float GetDegrees(int index)
        {
            return BitConverter.ToSingle(EntryList.ToArray(), index * 4) * Maths._rad2degf;
        }

        public void SetInt(int index, int value)
        {
            int oldValue = BitConverter.ToInt32(EntryList.ToArray(), index * 4);
            if (oldValue != value)
            {
                byte[] newValue = BitConverter.GetBytes(value);
                EntryList[index * 4] = newValue[0];
                EntryList[index * 4 + 1] = newValue[1];
                EntryList[index * 4 + 2] = newValue[2];
                EntryList[index * 4 + 3] = newValue[3];
                SignalPropertyChange();
            }
        }

        public int GetInt(int index)
        {
            return BitConverter.ToInt32(EntryList.ToArray(), index * 4);
        }

        public void SetRGBAPixel(int index, string value)
        {
            RGBAPixel p = new RGBAPixel();
            RGBAPixel pOld = BitConverter.ToUInt32(EntryList.ToArray(), index * 4);

            char[] delims = {',', 'R', 'G', 'B', 'A', ':', ' '};
            string[] arr = value.Split(delims, StringSplitOptions.RemoveEmptyEntries);

            if (arr.Length == 4)
            {
                byte.TryParse(arr[0], out p.R);
                byte.TryParse(arr[1], out p.G);
                byte.TryParse(arr[2], out p.B);
                byte.TryParse(arr[3], out p.A);
            }

            if (pOld != p)
            {
                byte[] newValue = BitConverter.GetBytes(p);
                EntryList[index * 4] = newValue[3];
                EntryList[index * 4 + 1] = newValue[2];
                EntryList[index * 4 + 2] = newValue[1];
                EntryList[index * 4 + 3] = newValue[0];
                SignalPropertyChange();
            }
        }

        public void SetRGBAPixel(int index, RGBAPixel value)
        {
            RGBAPixel pOld = BitConverter.ToUInt32(EntryList.ToArray(), index * 4);

            if (pOld != value)
            {
                byte[] newValue = BitConverter.GetBytes(value);
                EntryList[index * 4] = newValue[3];
                EntryList[index * 4 + 1] = newValue[2];
                EntryList[index * 4 + 2] = newValue[1];
                EntryList[index * 4 + 3] = newValue[0];
                SignalPropertyChange();
            }
        }

        public RGBAPixel GetRGBAPixel(int index)
        {
            return BitConverter.ToUInt32(EntryList.ToArray(), index * 4); //((RGBAPixel*) AttributeAddress)[index];
        }

        public void SetHex(int index, string value)
        {
            string field0 = (value ?? "").Split(' ')[0];
            int fromBase = field0.StartsWith("0x", StringComparison.OrdinalIgnoreCase) ? 16 : 10;
            int temp = Convert.ToInt32(field0, fromBase);
            SetInt(index, temp);
        }

        public string GetHex(int index)
        {
            string hex = "0x";
            for (int curIndex = index * 4 + 3; curIndex >= index * 4; curIndex--)
            {
                hex += EntryList[curIndex].ToString("X2");
            }

            return hex;
        }

        public void SetBytes(int index, params byte[] values)
        {
            if (values.Length < 4)
            {
                return;
            }

            int i = 0;
            for (int curIndex = index * 4 + 3; curIndex >= index * 4; curIndex--, i++)
            {
                EntryList[curIndex] = values[i];
            }
            SignalPropertyChange();
        }

        public string GetBytes(int index)
        {
            string bytes = "";
            for (int curIndex = index * 4 + 3; curIndex >= index * 4; curIndex--)
            {
                bytes += EntryList[curIndex] + ", ";
            }

            return bytes.TrimEnd(' ', ',');
        }

        public void SetShorts(int index, short value1, short value2)
        {
            if (!GetShorts(index).Equals($"{value1}, {value2}"))
            {
                byte[] newValue = BitConverter.GetBytes(value2);
                EntryList[index * 4] = newValue[0];
                EntryList[index * 4 + 1] = newValue[1];
                newValue = BitConverter.GetBytes(value1);
                EntryList[index * 4 + 2] = newValue[0];
                EntryList[index * 4 + 3] = newValue[1];
                SignalPropertyChange();
            }
        }

        public string GetShorts(int index)
        {
            return
                $"{BitConverter.ToInt16(EntryList.ToArray(), index * 4 + 2)}, {BitConverter.ToInt16(EntryList.ToArray(), index * 4)}";
        }

        public IEnumerable<AttributeInterpretation> GetPossibleInterpretations()
        {
            ReadConfig();
            ResourceNode root = this;
            while (root.Parent != null)
            {
                root = root.Parent;
            }

            IEnumerable<AttributeInterpretation> q = from f in Formats
                                                     where NumEntries == f.NumEntries
                                                     select f;

            bool any_match_name = q.Any(f => string.Equals(
                Path.GetFileNameWithoutExtension(f.Filename),
                root.Name.ToUpper().Replace("STG", "") + "[" + FileIndex + "]",
                StringComparison.OrdinalIgnoreCase));
            if (!any_match_name)
            {
                q = q.Concat(new[] {GenerateDefaultInterpretation()});
            }

            q = q.OrderBy(f => !string.Equals(
                Path.GetFileNameWithoutExtension(f.Filename),
                root.Name.ToUpper().Replace("STG", "") + "[" + FileIndex + "]",
                StringComparison.OrdinalIgnoreCase));

            return q;
        }

        protected virtual AttributeInterpretation GenerateDefaultInterpretation()
        {
            AttributeInfo[] arr = new AttributeInfo[NumEntries];
            int index = EntryOffset;

            ResourceNode root = RootNode;

            for (int i = 0; i < arr.Length; i++)
            {
                arr[i] = new AttributeInfo
                {
                    _name = "0x" + index.ToString("X3")
                };
                //Guess what type the attribute is
                uint u = BitConverter.ToUInt32(EntryList.ToArray(), i * 4);
                int i32 = BitConverter.ToInt32(EntryList.ToArray(), i * 4);
                float f = BitConverter.ToSingle(EntryList.ToArray(), i * 4);
                RGBAPixel p = new RGBAPixel(u);
                if (u == 0)
                {
                    arr[i]._type = 0;
                    arr[i]._description = "Default: 0 (Could be int or float - be careful)";
                }
                else if (((u >> 24) & 0xFF) != 0 && i32 != -1 && !float.IsNaN(f))
                {
                    float abs = Math.Abs(f);
                    if (abs > 0.0000001 && abs < 10000000 || float.IsInfinity(abs))
                    {
                        arr[i]._type = 0;
                        arr[i]._description = "Default (Float): " + f + " (0x" + u.ToString("X8") + ")";
                    }
                    else if ((p.R % 5 == 0 || p.R % 3 == 0) && (p.B % 5 == 0 || p.B % 3 == 0) &&
                             (p.G % 5 == 0 || p.G % 3 == 0) && (p.A == 0 || p.A == 255))
                    {
                        arr[i]._type = 3;
                        arr[i]._description = "Default (Color): " + p + " (0x" + u.ToString("X8") + ")";
                        arr[i]._name = arr[i]._name;
                    }
                    else
                    {
                        arr[i]._type = 4;
                        arr[i]._description = "Default (Unknown Type): (0x" + u.ToString("X8") + ")";
                        arr[i]._name = "~" + arr[i]._name;
                    }
                }
                else
                {
                    arr[i]._type = 1;
                    arr[i]._description = "Default (Integer): " + u + " (0x" + u.ToString("X8") + ")";
                    arr[i]._name = "*" + arr[i]._name;
                }

                index += 4;
            }

            string temp = "";
            if (root != this)
            {
                temp = "[" + FileIndex + "]";
            }

#if DEBUG
            DirectoryInfo startingDirectory = new DirectoryInfo(Application.StartupPath);
            string filename = Path.Combine(startingDirectory.Parent.Parent.Parent.Parent.Parent.FullName, "BrawlLib",
                "InternalDocumentation", DocumentationSubDirectory,
                root.Name.ToUpper().Replace("STG", "") + temp + ".txt");
#else
            string filename = Path.Combine(Application.StartupPath, "InternalDocumentation", DocumentationSubDirectory,
                root.Name.ToUpper().Replace("STG", "") + temp + ".txt");
#endif
            return new AttributeInterpretation(arr, filename);
        }

        protected static readonly List<AttributeInterpretation> Formats = new List<AttributeInterpretation>();
        protected static readonly HashSet<string> ConfigPathsRead = new HashSet<string>();

        protected virtual void ReadConfig()
        {
#if DEBUG
            string startPath =
                Path.Combine(new DirectoryInfo(Application.StartupPath).Parent.Parent.Parent.Parent.Parent.FullName,
                    "BrawlLib");
#else
            string startPath = Application.StartupPath;
#endif
            if (Directory.Exists(startPath + "\\InternalDocumentation"))
            {
                if (Directory.Exists(startPath + "\\InternalDocumentation\\" +
                                     DocumentationSubDirectory))
                {
                    foreach (string path in Directory.EnumerateFiles(
                        startPath + "\\InternalDocumentation\\" + DocumentationSubDirectory,
                        "*.txt"))
                    {
                        if (ConfigPathsRead.Contains(path))
                        {
                            continue;
                        }

                        ConfigPathsRead.Add(path);
                        try
                        {
                            Formats.Add(new AttributeInterpretation(path, EntryOffset));
                        }
                        catch (FormatException ex)
                        {
                            if (Properties.Settings.Default.HideMDL0Errors)
                            {
                                Console.Error.WriteLine(ex.Message);
                            }
                            else
                            {
                                MessageBox.Show(ex.Message);
                            }
                        }
                    }
                }
            }
        }

        public override void Replace(string fileName)
        {
            EntryList.Clear();
            base.Replace(fileName);
        }
    }
}