using BrawlLib.Internal;
using BrawlLib.Internal.IO;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class GCTNode : ResourceNode
    {
        internal byte* Data => (byte*) WorkingUncompressed.Address;
        public override ResourceType ResourceFileType => ResourceType.Unknown;
        public override bool AllowNullNames => true;
        public override bool AllowDuplicateNames => true;

        public string _gameName;

        public string GameName
        {
            get => _gameName;
            set
            {
                _gameName = value;
                SignalPropertyChange();
            }
        }

        private GCTHeader* _header;

        public override bool OnInitialize()
        {
            base.OnInitialize();

            GCTCodeLine* lines = (GCTCodeLine*) Data + 1;
            while (true)
            {
                GCTCodeLine line = *lines++;
                if (line._1 == GCTCodeLine.End._1 && line._2 == GCTCodeLine.End._2)
                {
                    break;
                }
            }

            _header = (GCTHeader*) lines;
            if ((uint) _header - (uint) Data < WorkingUncompressed.Length)
            {
                if (_header->_nameOffset > 0)
                {
                    _gameName = _header->GameName;
                }

                _name = _header->GameID;
            }

            if (_name == null)
            {
                _name = Path.GetFileNameWithoutExtension(_origPath);
            }

            return WorkingUncompressed.Length > 16 && _header->_count > 0;
        }

        public override void OnPopulate()
        {
            GCTCodeEntry* entry = (GCTCodeEntry*) (_header + 1);
            for (int i = 0; i < _header->_count; i++)
            {
                new GCTCodeEntryNode(&entry[i]).Initialize(this, Data + entry[i]._codeOffset,
                    (int) entry[i]._lineCount * GCTCodeLine.Size);
            }
        }

        private CompactStringTable _stringTable;

        public bool _writeInfo = true;

        public override int OnCalculateSize(bool force)
        {
            int size = 16;
            foreach (GCTCodeEntryNode n in Children)
            {
                size += n._lines.Length * 8;
            }

            if (_writeInfo)
            {
                _stringTable = new CompactStringTable();
                _stringTable.Add(_name);
                if (!string.IsNullOrEmpty(_gameName))
                {
                    _stringTable.Add(_gameName);
                }

                size += 12;
                foreach (GCTCodeEntryNode n in Children)
                {
                    size += 16;
                    _stringTable.Add(n._name);
                    if (!string.IsNullOrEmpty(n._description))
                    {
                        _stringTable.Add(n._description);
                    }
                }

                size += _stringTable.TotalSize;
            }

            return size;
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            List<GCTCodeEntryNode> enabledChildren =
                Children.Select(e => (GCTCodeEntryNode) e).Where(e => e._enabled).ToList();

            GCTCodeLine* line = (GCTCodeLine*) address;
            *line++ = GCTCodeLine.Tag;
            foreach (GCTCodeEntryNode n in enabledChildren)
            {
                foreach (GCTCodeLine l in n._lines)
                {
                    *line++ = l;
                }
            }

            *line++ = GCTCodeLine.End;
            if (_writeInfo)
            {
                GCTHeader* hdr = (GCTHeader*) line;
                hdr->_count = (uint) enabledChildren.Count;

                int offset = 12 + enabledChildren.Count * 16;
                _stringTable.WriteTable((VoidPtr) hdr + offset);

                hdr->_idOffset = (uint) _stringTable[_name] - (uint) hdr;
                if (!string.IsNullOrEmpty(_gameName))
                {
                    hdr->_nameOffset = (uint) _stringTable[_gameName] - (uint) hdr;
                }
                else
                {
                    hdr->_nameOffset = 0;
                }

                GCTCodeEntry* entry = (GCTCodeEntry*) (hdr + 1);
                uint codeOffset = 8;
                foreach (GCTCodeEntryNode n in enabledChildren)
                {
                    entry->_codeOffset = codeOffset;
                    codeOffset += (uint) n._lines.Length * 8;

                    entry->_lineCount = (uint) n._lines.Length;
                    entry->_nameOffset = (uint) _stringTable[n._name] - (uint) entry;
                    if (!string.IsNullOrEmpty(n._description))
                    {
                        entry->_descOffset = (uint) _stringTable[n._description] - (uint) entry;
                    }
                    else
                    {
                        entry->_descOffset = 0;
                    }

                    entry++;
                }
            }
        }

        public void ToTXT()
        {
            SaveFileDialog d = new SaveFileDialog
            {
                Filter = "Text File|*.txt"
            };
            if (d.ShowDialog() == DialogResult.OK)
            {
                ToTXT(d.FileName);
            }
        }

        public void ToTXT(string path)
        {
            using (StreamWriter file = new StreamWriter(path))
            {
                file.WriteLine(_name);
                file.WriteLine(_gameName);
                foreach (GCTCodeEntryNode n in Children)
                {
                    file.WriteLine();
                    file.WriteLine(n._name);
                    file.Write(n.DisplayLines);
                    if (!string.IsNullOrWhiteSpace(n._description))
                    {
                        file.WriteLine(n._description);
                    }
                }
            }
        }

        public static GCTNode FromTXT(string path)
        {
            GCTNode node = new GCTNode();

            if (File.Exists(path))
            {
                bool anyEnabled = false;

                using (StreamReader sr = new StreamReader(path))
                {
                    for (int i = 0; !sr.EndOfStream; i++)
                    {
                        string lastLine;
                        while (string.IsNullOrEmpty(lastLine = sr.ReadLine()))
                        {
                            ;
                        }

                        if (!string.IsNullOrEmpty(lastLine))
                        {
                            node._name = lastLine;
                        }

                        while (!sr.EndOfStream)
                        {
                            while (string.IsNullOrEmpty(lastLine = sr.ReadLine()))
                            {
                                ;
                            }

                            GCTCodeEntryNode e = new GCTCodeEntryNode();
                            List<GCTCodeLine> g = new List<GCTCodeLine>();

                            if (!string.IsNullOrEmpty(lastLine))
                            {
                                e._name = lastLine;
                            }
                            else
                            {
                                break;
                            }

                            bool? codeEnabled = null;
                            while (true)
                            {
                                lastLine = sr.ReadLine();
                                if (string.IsNullOrEmpty(lastLine))
                                {
                                    break;
                                }

                                bool lineEnabled = lastLine.StartsWith("* ");
                                if (lineEnabled)
                                {
                                    anyEnabled = true;
                                    lastLine = lastLine.Substring(2);
                                }

                                if (codeEnabled == null)
                                {
                                    codeEnabled = lineEnabled;
                                }
                                else if (codeEnabled != lineEnabled)
                                {
                                    break;
                                }

                                string[] lines = lastLine.Split(' ');
                                if (lines.Length < 2)
                                {
                                    break;
                                }

                                if (uint.TryParse(lines[0], NumberStyles.HexNumber, CultureInfo.InvariantCulture,
                                    out uint val1))
                                {
                                    if (uint.TryParse(lines[1], NumberStyles.HexNumber, CultureInfo.InvariantCulture,
                                        out uint val2))
                                    {
                                        GCTCodeLine l = new GCTCodeLine(val1, val2);
                                        g.Add(l);
                                    }
                                    else
                                    {
                                        break;
                                    }
                                }
                                else
                                {
                                    break;
                                }
                            }

                            List<string> description = new List<string>();
                            while (!string.IsNullOrEmpty(lastLine))
                            {
                                description.Add(lastLine);
                                lastLine = sr.ReadLine();
                            }

                            e._enabled = codeEnabled ?? false;
                            e._lines = g.ToArray();
                            e._description = string.Join(Environment.NewLine, description);
                            if (e._lines.Length > 0)
                            {
                                node.AddChild(e, false);
                            }
                        }
                    }
                }

                if (anyEnabled == false)
                {
                    // No codes enabled in file - enable all codes
                    foreach (GCTCodeEntryNode e in node.Children)
                    {
                        e._enabled = true;
                    }
                }
            }

            node._origPath = path;
            return node;
        }

        public static GCTNode IsParsable(string path)
        {
            FileMap map = FileMap.FromFile(path, FileMapProtect.ReadWrite);

            GCTCodeLine* data = (GCTCodeLine*) map.Address;
            if (GCTCodeLine.Tag._1 != data->_1 || GCTCodeLine.Tag._2 != data->_2)
            {
                map.Dispose();
                return null;
            }

            data = (GCTCodeLine*) (map.Address + (uint) Helpers.RoundDown((uint) map.Length, 8) - GCTCodeLine.Size);
            bool endFound = false;
            int i = 0;
            while (!endFound)
            {
                GCTCodeLine line = *data--;
                if (line._1 == GCTCodeLine.End._1 && line._2 == GCTCodeLine.End._2)
                {
                    endFound = true;
                    break;
                }

                i++;
            }

            if (endFound && i <= 0)
            {
                data = (GCTCodeLine*) map.Address + 1;

                string s = "";
                while (true)
                {
                    GCTCodeLine line = *data++;
                    if (line._1 == GCTCodeLine.End._1 && line._2 == GCTCodeLine.End._2)
                    {
                        break;
                    }

                    s += line.ToStringNoSpace();
                }

                GCTNode g = new GCTNode {_origPath = path};

                List<string> _unrecognized = new List<string>();

                if (Properties.Settings.Default.Codes != null)
                {
                    foreach (CodeStorage c in Properties.Settings.Default.Codes)
                    {
                        int index = -1;
                        if ((index = s.IndexOf(c._code)) >= 0)
                        {
                            g.AddChild(new GCTCodeEntryNode
                            {
                                _name = c._name,
                                _description = c._description,
                                LinesNoSpaces = s.Substring(index, c._code.Length),
                                _enabled = true
                            }, false);
                            s = s.Remove(index, c._code.Length);
                        }
                    }
                }

                if (s.Length > 0)
                {
                    g.AddChild(
                        new GCTCodeEntryNode {_name = "Unrecognized Code(s)", LinesNoSpaces = s, _enabled = true},
                        false);
                }

                if (g._name == null)
                {
                    g._name = Path.GetFileNameWithoutExtension(path);
                }

                return g;
            }

            if (endFound && i > 0)
            {
                GCTNode g = new GCTNode();
                g.Initialize(null, new DataSource(map));
                return g;
            }

            map.Dispose();
            return null;
        }
    }

    public unsafe class GCTCodeEntryNode : ResourceNode
    {
        internal GCTCodeLine* Header => (GCTCodeLine*) WorkingUncompressed.Address;
        public override ResourceType ResourceFileType => ResourceType.Unknown;
        public override bool AllowNullNames => true;
        public override bool AllowDuplicateNames => true;

        public GCTCodeLine[] _lines = new GCTCodeLine[0];

        public string DisplayLines
        {
            get
            {
                string temp = "";
                foreach (GCTCodeLine c in _lines)
                {
                    temp += (_enabled ? "* " : "") + c + Environment.NewLine;
                }

                return temp;
            }
        }

        public string LinesNoSpaces
        {
            get
            {
                string temp = "";
                foreach (GCTCodeLine c in _lines)
                {
                    temp += c.ToStringNoSpace();
                }

                return temp;
            }
            set
            {
                int x = value.Length / 16;
                _lines = new GCTCodeLine[x];
                for (int i = 0; i < x; i++)
                {
                    _lines[i] = GCTCodeLine.FromStringNoSpace(value.Substring(i * 16, 16));
                }
            }
        }

        public string _description;
        public bool _enabled;

        public GCTCodeEntryNode()
        {
        }

        public GCTCodeEntryNode(GCTCodeEntry* entry)
        {
            _name = entry->CodeName;
            if (entry->_descOffset > 0)
            {
                _description = entry->CodeDesc;
            }

            _lines = new GCTCodeLine[entry->_lineCount];
        }

        public override bool OnInitialize()
        {
            for (int i = 0; i < _lines.Length; i++)
            {
                _lines[i] = Header[i];
            }

            return false;
        }
    }

    public unsafe struct GCTHeader
    {
        public buint _nameOffset;
        public buint _idOffset;
        public buint _count;

        public string GameName => new string((sbyte*) (Address + _nameOffset));
        public string GameID => new string((sbyte*) (Address + _idOffset));

        private VoidPtr Address
        {
            get
            {
                fixed (void* ptr = &this)
                {
                    return ptr;
                }
            }
        }
    }

    public unsafe struct GCTCodeEntry
    {
        public buint _codeOffset;
        public buint _lineCount;
        public buint _nameOffset;
        public buint _descOffset;

        public string CodeName => new string((sbyte*) (Address + _nameOffset));
        public string CodeDesc => new string((sbyte*) (Address + _descOffset));

        private VoidPtr Address
        {
            get
            {
                fixed (void* ptr = &this)
                {
                    return ptr;
                }
            }
        }
    }

    public struct GCTCodeLine
    {
        public static readonly GCTCodeLine Tag = new GCTCodeLine(0x00D0C0DE, 0x00D0C0DE);
        public static readonly GCTCodeLine End = new GCTCodeLine(0xF0000000, 0x00000000);
        public const int Size = 8;

        public buint _1;
        public buint _2;

        public GCTCodeLine(uint one, uint two)
        {
            _1 = one;
            _2 = two;
        }

        public override string ToString()
        {
            return ((uint) _1).ToString("X8") + " " + ((uint) _2).ToString("X8");
        }

        public string ToStringNoSpace()
        {
            return ((uint) _1).ToString("X8") + ((uint) _2).ToString("X8");
        }

        public static GCTCodeLine FromString(string s)
        {
            string[] l = s.Split(' ');
            return new GCTCodeLine(Convert.ToUInt32(l[0], 16), Convert.ToUInt32(l[1], 16));
        }

        public static GCTCodeLine FromStringNoSpace(string s)
        {
            return new GCTCodeLine(Convert.ToUInt32(s.Substring(0, 8), 16), Convert.ToUInt32(s.Substring(8, 8), 16));
        }
    }

    public class CodeStorage
    {
        public string _code;
        public string _name;
        public string _description;
    }
}