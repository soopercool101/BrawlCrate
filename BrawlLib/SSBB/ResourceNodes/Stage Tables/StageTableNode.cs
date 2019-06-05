using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using BrawlLib.Imaging;
using BrawlLib.Properties;

namespace BrawlLib.SSBB.ResourceNodes
{
    public abstract unsafe class StageTableNode : ARCEntryNode, IAttributeList
    {
        protected static readonly List<AttributeInterpretation> Formats = new List<AttributeInterpretation>();

        protected static readonly HashSet<string> configpaths_read = new HashSet<string>();

        // Internal buffer for editing - changes written back to WorkingUncompressed on rebuild
        internal UnsafeBuffer entries;

        internal int version, unk1, unk2, _entryOffset;

        internal static int EntrySize => 4;

        internal virtual string DocumentationSubDirectory => "";

        [Category("Stage Data Table")]
        public virtual int Version
        {
            get => version;
            set
            {
                version = value;
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

        [Category("Stage Data Table")] public virtual int NumEntries => entries.Length / EntrySize;

        [Browsable(false)] public VoidPtr AttributeAddress => entries.Address;

        public void SetFloat(int index, float value)
        {
            if (((bfloat*) AttributeAddress)[index] != value)
            {
                ((bfloat*) AttributeAddress)[index] = value;
                SignalPropertyChange();
            }
        }

        public float GetFloat(int index)
        {
            return ((bfloat*) AttributeAddress)[index];
        }

        public void SetDegrees(int index, float value)
        {
            if (((bfloat*) AttributeAddress)[index] != value * Maths._deg2radf)
            {
                ((bfloat*) AttributeAddress)[index] = value * Maths._deg2radf;
                SignalPropertyChange();
            }
        }

        public float GetDegrees(int index)
        {
            return ((bfloat*) AttributeAddress)[index] * Maths._rad2degf;
        }

        public void SetInt(int index, int value)
        {
            if (((bint*) AttributeAddress)[index] != value)
            {
                ((bint*) AttributeAddress)[index] = value;
                SignalPropertyChange();
            }
        }

        public int GetInt(int index)
        {
            return ((bint*) AttributeAddress)[index];
        }

        public void SetRGBAPixel(int index, string value)
        {
            var p = new RGBAPixel();

            var s = value;
            char[] delims = {',', 'R', 'G', 'B', 'A', ':', ' '};
            var arr = s.Split(delims, StringSplitOptions.RemoveEmptyEntries);

            if (arr.Length == 4)
            {
                byte.TryParse(arr[0], out p.R);
                byte.TryParse(arr[1], out p.G);
                byte.TryParse(arr[2], out p.B);
                byte.TryParse(arr[3], out p.A);
            }

            if (((RGBAPixel*) AttributeAddress)[index] != p)
            {
                ((RGBAPixel*) AttributeAddress)[index] = p;
                SignalPropertyChange();
            }
        }

        public void SetRGBAPixel(int index, RGBAPixel value)
        {
            if (((RGBAPixel*) AttributeAddress)[index] != value)
            {
                ((RGBAPixel*) AttributeAddress)[index] = value;
                SignalPropertyChange();
            }
        }

        public RGBAPixel GetRGBAPixel(int index)
        {
            return ((RGBAPixel*) AttributeAddress)[index];
        }

        public void SetHex(int index, string value)
        {
            var field0 = (value ?? "").Split(' ')[0];
            var fromBase = field0.StartsWith("0x", StringComparison.OrdinalIgnoreCase) ? 16 : 10;
            var temp = Convert.ToInt32(field0, fromBase);
            if (((bint*) AttributeAddress)[index] != temp)
            {
                ((bint*) AttributeAddress)[index] = temp;
                SignalPropertyChange();
            }
        }

        public string GetHex(int index)
        {
            return "0x" + ((int) ((bint*) AttributeAddress)[index]).ToString("X8");
        }

        ~StageTableNode()
        {
            entries.Dispose();
        }

        public override int OnCalculateSize(bool force)
        {
            return EntryOffset + entries.Length;
        }

        public virtual IEnumerable<AttributeInterpretation> GetPossibleInterpretations()
        {
            ReadConfig();
            ResourceNode root = this;
            while (root.Parent != null) root = root.Parent;

            var q = from f in Formats
                where EntryOffset + f.NumEntries * 4 == WorkingUncompressed.Length
                select f;

            var any_match_name = q.Any(f => string.Equals(
                Path.GetFileNameWithoutExtension(f.Filename),
                root.Name.ToUpper().Replace("STG", "") + "[" + FileIndex + "]",
                StringComparison.OrdinalIgnoreCase));
            if (!any_match_name) q = q.Concat(new[] {GenerateDefaultInterpretation()});

            q = q.OrderBy(f => !string.Equals(
                Path.GetFileNameWithoutExtension(f.Filename),
                root.Name.ToUpper().Replace("STG", "") + "[" + FileIndex + "]",
                StringComparison.OrdinalIgnoreCase));

            return q;
        }

        protected virtual AttributeInterpretation GenerateDefaultInterpretation()
        {
            var arr = new AttributeInfo[NumEntries];
            var pIn = (buint*) AttributeAddress;
            var index = EntryOffset;

            ResourceNode root = this;
            while (root.Parent != null) root = root.Parent;

            for (var i = 0; i < arr.Length; i++)
            {
                arr[i] = new AttributeInfo
                {
                    _name = "0x" + index.ToString("X3")
                };
                //Guess what type the attribute is
                uint u = *pIn;
                float f = *(bfloat*) pIn;
                var p = new RGBAPixel(u);
                if (*pIn == 0)
                {
                    arr[i]._type = 0;
                    arr[i]._description = "Default: 0 (Could be int or float - be careful)";
                }
                else if (((u >> 24) & 0xFF) != 0 && *(bint*) pIn != -1 && !float.IsNaN(f))
                {
                    var abs = Math.Abs(f);
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
                pIn++;
            }

            var temp = "";
            if (root != this) temp = "[" + FileIndex + "]";

            var filename = AppDomain.CurrentDomain.BaseDirectory + "InternalDocumentation\\" +
                           DocumentationSubDirectory + "\\" + root.Name.ToUpper().Replace("STG", "") + temp + ".txt";
            return new AttributeInterpretation(arr, filename);
        }

        protected virtual void ReadConfig()
        {
            if (Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + "InternalDocumentation"))
                if (Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + "InternalDocumentation\\" +
                                     DocumentationSubDirectory))
                    foreach (var path in Directory.EnumerateFiles(
                        AppDomain.CurrentDomain.BaseDirectory + "InternalDocumentation\\" + DocumentationSubDirectory,
                        "*.txt"))
                    {
                        if (configpaths_read.Contains(path)) continue;

                        configpaths_read.Add(path);
                        try
                        {
                            Formats.Add(new AttributeInterpretation(path, EntryOffset));
                        }
                        catch (FormatException ex)
                        {
                            if (Settings.Default.HideMDL0Errors)
                                Console.Error.WriteLine(ex.Message);
                            else
                                MessageBox.Show(ex.Message);
                        }
                    }
        }
    }
}