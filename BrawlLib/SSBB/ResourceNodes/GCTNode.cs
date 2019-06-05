using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using BrawlLib.IO;
using BrawlLib.Properties;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class GCTNode : ResourceNode
    {
        public string _gameName;

        private GCTHeader* _header;

        private CompactStringTable _stringTable;

        public bool _writeInfo = true;
        internal byte* Data => (byte*) WorkingUncompressed.Address;
        public override ResourceType ResourceFileType => ResourceType.Unknown;
        public override bool AllowNullNames => true;
        public override bool AllowDuplicateNames => true;

        public string GameName
        {
            get => _gameName;
            set
            {
                _gameName = value;
                SignalPropertyChange();
            }
        }

        public override bool OnInitialize()
        {
            base.OnInitialize();

            var lines = (GCTCodeLine*) Data + 1;
            while (true)
            {
                var line = *lines++;
                if (line._1 == GCTCodeLine.End._1 && line._2 == GCTCodeLine.End._2) break;
            }

            _header = (GCTHeader*) lines;
            if ((uint) _header - (uint) Data < WorkingUncompressed.Length)
            {
                if (_header->_nameOffset > 0) _gameName = _header->GameName;

                _name = _header->GameID;
            }

            if (_name == null) _name = Path.GetFileNameWithoutExtension(_origPath);

            return WorkingUncompressed.Length > 16 && _header->_count > 0;
        }

        public override void OnPopulate()
        {
            var entry = (GCTCodeEntry*) (_header + 1);
            for (var i = 0; i < _header->_count; i++)
                new GCTCodeEntryNode(&entry[i]).Initialize(this, Data + entry[i]._codeOffset,
                    (int) entry[i]._lineCount * GCTCodeLine.Size);
        }

        public override int OnCalculateSize(bool force)
        {
            var size = 16;
            foreach (GCTCodeEntryNode n in Children) size += n._lines.Length * 8;

            if (_writeInfo)
            {
                _stringTable = new CompactStringTable();
                _stringTable.Add(_name);
                if (!string.IsNullOrEmpty(_gameName)) _stringTable.Add(_gameName);

                size += 12;
                foreach (GCTCodeEntryNode n in Children)
                {
                    size += 16;
                    _stringTable.Add(n._name);
                    if (!string.IsNullOrEmpty(n._description)) _stringTable.Add(n._description);
                }

                size += _stringTable.TotalSize;
            }

            return size;
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            var enabledChildren = Children.Select(e => (GCTCodeEntryNode) e).Where(e => e._enabled).ToList();

            var line = (GCTCodeLine*) address;
            *line++ = GCTCodeLine.Tag;
            foreach (var n in enabledChildren)
            foreach (var l in n._lines)
                *line++ = l;

            *line++ = GCTCodeLine.End;
            if (_writeInfo)
            {
                var hdr = (GCTHeader*) line;
                hdr->_count = (uint) enabledChildren.Count;

                var offset = 12 + enabledChildren.Count * 16;
                _stringTable.WriteTable((VoidPtr) hdr + offset);

                hdr->_idOffset = (uint) _stringTable[_name] - (uint) hdr;
                if (!string.IsNullOrEmpty(_gameName))
                    hdr->_nameOffset = (uint) _stringTable[_gameName] - (uint) hdr;
                else
                    hdr->_nameOffset = 0;

                var entry = (GCTCodeEntry*) (hdr + 1);
                uint codeOffset = 8;
                foreach (var n in enabledChildren)
                {
                    entry->_codeOffset = codeOffset;
                    codeOffset += (uint) n._lines.Length * 8;

                    entry->_lineCount = (uint) n._lines.Length;
                    entry->_nameOffset = (uint) _stringTable[n._name] - (uint) entry;
                    if (!string.IsNullOrEmpty(n._description))
                        entry->_descOffset = (uint) _stringTable[n._description] - (uint) entry;
                    else
                        entry->_descOffset = 0;

                    entry++;
                }
            }
        }

        public void ToTXT()
        {
            var d = new SaveFileDialog
            {
                Filter = "Text File|*.txt"
            };
            if (d.ShowDialog() == DialogResult.OK) ToTXT(d.FileName);
        }

        public void ToTXT(string path)
        {
            using (var file = new StreamWriter(path))
            {
                file.WriteLine(_name);
                file.WriteLine(_gameName);
                foreach (GCTCodeEntryNode n in Children)
                {
                    file.WriteLine();
                    file.WriteLine(n._name);
                    file.Write(n.DisplayLines);
                    if (!string.IsNullOrWhiteSpace(n._description)) file.WriteLine(n._description);
                }
            }
        }

        public static GCTNode FromTXT(string path)
        {
            var node = new GCTNode();

            if (File.Exists(path))
            {
                var anyEnabled = false;

                using (var sr = new StreamReader(path))
                {
                    for (var i = 0; !sr.EndOfStream; i++)
                    {
                        string lastLine;
                        while (string.IsNullOrEmpty(lastLine = sr.ReadLine())) ;

                        if (!string.IsNullOrEmpty(lastLine)) node._name = lastLine;

                        lastLine = sr.ReadLine();
                        if (!string.IsNullOrEmpty(lastLine)) node._gameName = lastLine;

                        while (!sr.EndOfStream)
                        {
                            while (string.IsNullOrEmpty(lastLine = sr.ReadLine())) ;

                            var e = new GCTCodeEntryNode();
                            var g = new List<GCTCodeLine>();

                            if (!string.IsNullOrEmpty(lastLine))
                                e._name = lastLine;
                            else
                                break;

                            bool? codeEnabled = null;
                            while (true)
                            {
                                lastLine = sr.ReadLine();
                                if (string.IsNullOrEmpty(lastLine)) break;

                                var lineEnabled = lastLine.StartsWith("* ");
                                if (lineEnabled)
                                {
                                    anyEnabled = true;
                                    lastLine = lastLine.Substring(2);
                                }

                                if (codeEnabled == null)
                                    codeEnabled = lineEnabled;
                                else if (codeEnabled != lineEnabled) break;

                                var lines = lastLine.Split(' ');
                                if (lines.Length < 2) break;

                                if (uint.TryParse(lines[0], NumberStyles.HexNumber, CultureInfo.InvariantCulture,
                                    out var val1))
                                {
                                    if (uint.TryParse(lines[1], NumberStyles.HexNumber, CultureInfo.InvariantCulture,
                                        out var val2))
                                    {
                                        var l = new GCTCodeLine(val1, val2);
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

                            var description = new List<string>();
                            while (!string.IsNullOrEmpty(lastLine))
                            {
                                description.Add(lastLine);
                                lastLine = sr.ReadLine();
                            }

                            e._enabled = codeEnabled ?? false;
                            e._lines = g.ToArray();
                            e._description = string.Join(Environment.NewLine, description);
                            node.AddChild(e, false);
                        }
                    }
                }

                if (anyEnabled == false)
                    // No codes enabled in file - enable all codes
                    foreach (GCTCodeEntryNode e in node.Children)
                        e._enabled = true;
            }

            return node;
        }

        public static GCTNode IsParsable(string path)
        {
            var map = FileMap.FromFile(path, FileMapProtect.ReadWrite);

            var data = (GCTCodeLine*) map.Address;
            if (GCTCodeLine.Tag._1 != data->_1 || GCTCodeLine.Tag._2 != data->_2)
            {
                map.Dispose();
                return null;
            }

            data = (GCTCodeLine*) (map.Address + (uint) Helpers.RoundDown((uint) map.Length, 8) - GCTCodeLine.Size);
            var endFound = false;
            var i = 0;
            while (!endFound)
            {
                var line = *data--;
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

                var s = "";
                while (true)
                {
                    var line = *data++;
                    if (line._1 == GCTCodeLine.End._1 && line._2 == GCTCodeLine.End._2) break;

                    s += line.ToStringNoSpace();
                }

                var g = new GCTNode {_origPath = path};

                var _unrecognized = new List<string>();

                foreach (var c in Settings.Default.Codes)
                {
                    var index = -1;
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

                if (g.Children.Count > 0)
                {
                    if (s.Length > 0)
                        MessageBox.Show(string.Format("{0} code{1} w{2} recognized.", g.Children.Count.ToString(),
                            g.Children.Count > 1 ? "s" : "", g.Children.Count > 1 ? "ere" : "as"));
                }
                else
                {
                    MessageBox.Show("This GCT does not contain any recognizable codes.");
                }

                if (s.Length > 0)
                    g.AddChild(
                        new GCTCodeEntryNode {_name = "Unrecognized Code(s)", LinesNoSpaces = s, _enabled = true},
                        false);

                if (g._name == null) g._name = Path.GetFileNameWithoutExtension(path);

                return g;
            }

            if (endFound && i > 0)
            {
                var g = new GCTNode();
                g.Initialize(null, new DataSource(map));
                return g;
            }

            map.Dispose();
            return null;
        }
    }

    public unsafe class GCTCodeEntryNode : ResourceNode
    {
        public string _description;
        public bool _enabled;

        public GCTCodeLine[] _lines = new GCTCodeLine[0];

        public GCTCodeEntryNode()
        {
        }

        public GCTCodeEntryNode(GCTCodeEntry* entry)
        {
            _name = entry->CodeName;
            if (entry->_descOffset > 0) _description = entry->CodeDesc;

            _lines = new GCTCodeLine[entry->_lineCount];
        }

        internal GCTCodeLine* Header => (GCTCodeLine*) WorkingUncompressed.Address;
        public override ResourceType ResourceFileType => ResourceType.Unknown;
        public override bool AllowNullNames => true;
        public override bool AllowDuplicateNames => true;

        public string DisplayLines
        {
            get
            {
                var temp = "";
                foreach (var c in _lines) temp += (_enabled ? "* " : "") + c + Environment.NewLine;

                return temp;
            }
        }

        public string LinesNoSpaces
        {
            get
            {
                var temp = "";
                foreach (var c in _lines) temp += c.ToStringNoSpace();

                return temp;
            }
            set
            {
                var x = value.Length / 16;
                _lines = new GCTCodeLine[x];
                for (var i = 0; i < x; i++) _lines[i] = GCTCodeLine.FromStringNoSpace(value.Substring(i * 16, 16));
            }
        }

        public override bool OnInitialize()
        {
            for (var i = 0; i < _lines.Length; i++) _lines[i] = Header[i];

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
            var l = s.Split(' ');
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
        public string _description;
        public string _name;
    }
}