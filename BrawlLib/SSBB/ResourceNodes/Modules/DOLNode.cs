using System;
using System.Linq;
using BrawlLib.SSBBTypes;
using System.IO;
using System.ComponentModel;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class DOLNode : ResourceNode, ModuleNode
    {
        internal DOLHeader* Header { get { return (DOLHeader*)WorkingUncompressed.Address; } }
        public override ResourceType ResourceType { get { return ResourceType.Unknown; } }

        DOLHeader hdr;

        [Category("Relocatable Module"), Browsable(false)]
        public uint ModuleID { get { return 0; } }
        uint ModuleNode.ID
        {
            get
            {
                return 0;
            }
            set
            {
                
            }
        }

        [Browsable(false)]
        public ModuleSectionNode[] Sections { get { return Children.Select(x => x as ModuleSectionNode).ToArray(); } }

        [Category("Static Module")]
        public string Text0Offset { get { return "0x" + ((uint)hdr.Text0Offset).ToString("X"); } }
        [Category("Static Module")]
        public string Text1Offset { get { return "0x" + ((uint)hdr.Text1Offset).ToString("X"); } }
        [Category("Static Module")]
        public string Text2Offset { get { return "0x" + ((uint)hdr.Text2Offset).ToString("X"); } }
        [Category("Static Module")]
        public string Text3Offset { get { return "0x" + ((uint)hdr.Text3Offset).ToString("X"); } }
        [Category("Static Module")]
        public string Text4Offset { get { return "0x" + ((uint)hdr.Text4Offset).ToString("X"); } }
        [Category("Static Module")]
        public string Text5Offset { get { return "0x" + ((uint)hdr.Text5Offset).ToString("X"); } }
        [Category("Static Module")]
        public string Text6Offset { get { return "0x" + ((uint)hdr.Text6Offset).ToString("X"); } }

        [Category("Static Module")]
        public string Data0Offset { get { return "0x" + ((uint)hdr.Data0Offset).ToString("X"); } }
        [Category("Static Module")]
        public string Data1Offset { get { return "0x" + ((uint)hdr.Data1Offset).ToString("X"); } }
        [Category("Static Module")]
        public string Data2Offset { get { return "0x" + ((uint)hdr.Data2Offset).ToString("X"); } }
        [Category("Static Module")]
        public string Data3Offset { get { return "0x" + ((uint)hdr.Data3Offset).ToString("X"); } }
        [Category("Static Module")]
        public string Data4Offset { get { return "0x" + ((uint)hdr.Data4Offset).ToString("X"); } }
        [Category("Static Module")]
        public string Data5Offset { get { return "0x" + ((uint)hdr.Data5Offset).ToString("X"); } }
        [Category("Static Module")]
        public string Data6Offset { get { return "0x" + ((uint)hdr.Data6Offset).ToString("X"); } }
        [Category("Static Module")]
        public string Data7Offset { get { return "0x" + ((uint)hdr.Data7Offset).ToString("X"); } }
        [Category("Static Module")]
        public string Data8Offset { get { return "0x" + ((uint)hdr.Data8Offset).ToString("X"); } }
        [Category("Static Module")]
        public string Data9Offset { get { return "0x" + ((uint)hdr.Data9Offset).ToString("X"); } }
        [Category("Static Module")]
        public string Data10Offset { get { return "0x" + ((uint)hdr.Data10Offset).ToString("X"); } }

        [Category("Static Module")]
        public string Text0LoadAddr { get { return "0x" + ((uint)hdr.Text0LoadAddr).ToString("X"); } set { GetValue(value, out hdr.Text0LoadAddr); SignalPropertyChange(); } }
        [Category("Static Module")]
        public string Text1LoadAddr { get { return "0x" + ((uint)hdr.Text1LoadAddr).ToString("X"); } set { GetValue(value, out hdr.Text1LoadAddr); SignalPropertyChange(); } }
        [Category("Static Module")]
        public string Text2LoadAddr { get { return "0x" + ((uint)hdr.Text2LoadAddr).ToString("X"); } set { GetValue(value, out hdr.Text2LoadAddr); SignalPropertyChange(); } }
        [Category("Static Module")]
        public string Text3LoadAddr { get { return "0x" + ((uint)hdr.Text3LoadAddr).ToString("X"); } set { GetValue(value, out hdr.Text3LoadAddr); SignalPropertyChange(); } }
        [Category("Static Module")]
        public string Text4LoadAddr { get { return "0x" + ((uint)hdr.Text4LoadAddr).ToString("X"); } set { GetValue(value, out hdr.Text4LoadAddr); SignalPropertyChange(); } }
        [Category("Static Module")]
        public string Text5LoadAddr { get { return "0x" + ((uint)hdr.Text5LoadAddr).ToString("X"); } set { GetValue(value, out hdr.Text5LoadAddr); SignalPropertyChange(); } }
        [Category("Static Module")]
        public string Text6LoadAddr { get { return "0x" + ((uint)hdr.Text6LoadAddr).ToString("X"); } set { GetValue(value, out hdr.Text6LoadAddr); SignalPropertyChange(); } }

        [Category("Static Module")]
        public string Data0LoadAddr { get { return "0x" + ((uint)hdr.Data0LoadAddr).ToString("X"); } set { GetValue(value, out hdr.Data0LoadAddr); SignalPropertyChange(); } }
        [Category("Static Module")]
        public string Data1LoadAddr { get { return "0x" + ((uint)hdr.Data1LoadAddr).ToString("X"); } set { GetValue(value, out hdr.Data1LoadAddr); SignalPropertyChange(); } }
        [Category("Static Module")]
        public string Data2LoadAddr { get { return "0x" + ((uint)hdr.Data2LoadAddr).ToString("X"); } set { GetValue(value, out hdr.Data2LoadAddr); SignalPropertyChange(); } }
        [Category("Static Module")]
        public string Data3LoadAddr { get { return "0x" + ((uint)hdr.Data3LoadAddr).ToString("X"); } set { GetValue(value, out hdr.Data3LoadAddr); SignalPropertyChange(); } }
        [Category("Static Module")]
        public string Data4LoadAddr { get { return "0x" + ((uint)hdr.Data4LoadAddr).ToString("X"); } set { GetValue(value, out hdr.Data4LoadAddr); SignalPropertyChange(); } }
        [Category("Static Module")]
        public string Data5LoadAddr { get { return "0x" + ((uint)hdr.Data5LoadAddr).ToString("X"); } set { GetValue(value, out hdr.Data5LoadAddr); SignalPropertyChange(); } }
        [Category("Static Module")]
        public string Data6LoadAddr { get { return "0x" + ((uint)hdr.Data6LoadAddr).ToString("X"); } set { GetValue(value, out hdr.Data6LoadAddr); SignalPropertyChange(); } }
        [Category("Static Module")]
        public string Data7LoadAddr { get { return "0x" + ((uint)hdr.Data7LoadAddr).ToString("X"); } set { GetValue(value, out hdr.Data7LoadAddr); SignalPropertyChange(); } }
        [Category("Static Module")]
        public string Data8LoadAddr { get { return "0x" + ((uint)hdr.Data8LoadAddr).ToString("X"); } set { GetValue(value, out hdr.Data8LoadAddr); SignalPropertyChange(); } }
        [Category("Static Module")]
        public string Data9LoadAddr { get { return "0x" + ((uint)hdr.Data9LoadAddr).ToString("X"); } set { GetValue(value, out hdr.Data9LoadAddr); SignalPropertyChange(); } }
        [Category("Static Module")]
        public string Data10LoadAddr { get { return "0x" + ((uint)hdr.Data10LoadAddr).ToString("X"); } set { GetValue(value, out hdr.Data10LoadAddr); SignalPropertyChange(); } }

        [Category("Static Module")]
        public string Text0Size { get { return "0x" + ((uint)hdr.Text0Size).ToString("X"); } }
        [Category("Static Module")]
        public string Text1Size { get { return "0x" + ((uint)hdr.Text1Size).ToString("X"); } }
        [Category("Static Module")]
        public string Text2Size { get { return "0x" + ((uint)hdr.Text2Size).ToString("X"); } }
        [Category("Static Module")]
        public string Text3Size { get { return "0x" + ((uint)hdr.Text3Size).ToString("X"); } }
        [Category("Static Module")]
        public string Text4Size { get { return "0x" + ((uint)hdr.Text4Size).ToString("X"); } }
        [Category("Static Module")]
        public string Text5Size { get { return "0x" + ((uint)hdr.Text5Size).ToString("X"); } }
        [Category("Static Module")]
        public string Text6Size { get { return "0x" + ((uint)hdr.Text6Size).ToString("X"); } }

        [Category("Static Module")]
        public string Data0Size { get { return "0x" + ((uint)hdr.Data0Size).ToString("X"); } }
        [Category("Static Module")]
        public string Data1Size { get { return "0x" + ((uint)hdr.Data1Size).ToString("X"); } }
        [Category("Static Module")]
        public string Data2Size { get { return "0x" + ((uint)hdr.Data2Size).ToString("X"); } }
        [Category("Static Module")]
        public string Data3Size { get { return "0x" + ((uint)hdr.Data3Size).ToString("X"); } }
        [Category("Static Module")]
        public string Data4Size { get { return "0x" + ((uint)hdr.Data4Size).ToString("X"); } }
        [Category("Static Module")]
        public string Data5Size { get { return "0x" + ((uint)hdr.Data5Size).ToString("X"); } }
        [Category("Static Module")]
        public string Data6Size { get { return "0x" + ((uint)hdr.Data6Size).ToString("X"); } }
        [Category("Static Module")]
        public string Data7Size { get { return "0x" + ((uint)hdr.Data7Size).ToString("X"); } }
        [Category("Static Module")]
        public string Data8Size { get { return "0x" + ((uint)hdr.Data8Size).ToString("X"); } }
        [Category("Static Module")]
        public string Data9Size { get { return "0x" + ((uint)hdr.Data9Size).ToString("X"); } }
        [Category("Static Module")]
        public string Data10Size { get { return "0x" + ((uint)hdr.Data10Size).ToString("X"); } }

        [Category("Static Module")]
        public string bssOffset { get { return "0x" + ((uint)hdr.bssOffset).ToString("X"); } set { GetValue(value, out hdr.bssOffset); SignalPropertyChange(); } }
        [Category("Static Module")]
        public string bssSize { get { return "0x" + ((uint)hdr.bssSize).ToString("X"); } set { GetValue(value, out hdr.bssSize); SignalPropertyChange(); } }
        [Category("Static Module")]
        public string EntryPoint { get { return "0x" + ((uint)hdr.entryPoint).ToString("X"); } set { GetValue(value, out hdr.entryPoint); SignalPropertyChange(); } }

        bool GetValue(string v, out buint value)
        {
            string s = (v.StartsWith("0x") ? v.Substring(2, Math.Min(v.Length - 2, 8)) : v.Substring(0, Math.Min(v.Length, 8)));
            uint t;
            if (uint.TryParse(s, System.Globalization.NumberStyles.HexNumber, null, out t))
            {
                value = t;
                return true;
            }
            value = 0;
            return false;
        }

        public override bool OnInitialize()
        {
            _name = Path.GetFileName(_origPath);
            hdr = *Header;
            return true;
        }

        public override void OnPopulate()
        {
            int x = 0;
            for (int i = 0; i < 7; i++, x++)
                new ModuleSectionNode() { _isCodeSection = true, _name = String.Format("[{0}] Text{1}", x, i), _dataOffset = (int)Header->TextOffset[i], _dataSize = Header->TextSize[i] }.Initialize(this, (VoidPtr)Header + Header->TextOffset[i], (int)Header->TextSize[i]);

            for (int i = 0; i < 11; i++, x++)
                new ModuleSectionNode() { _name = String.Format("[{0}] Data{1}", x, i), _dataOffset = (int)Header->DataOffset[i], _dataSize = Header->DataSize[i] }.Initialize(this, (VoidPtr)Header + Header->DataOffset[i], (int)Header->DataSize[i]);
        }

        public override int OnCalculateSize(bool force)
        {
            int size = DOLHeader.Size;
            foreach (ModuleSectionNode s in Children)
                if (s._dataBuffer != null && s._dataBuffer.Length > 0)
                    size += s.CalculateSize(true);
            return size;
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            DOLHeader* header = (DOLHeader*)address;
            VoidPtr dataAddr = address + DOLHeader.Size;
            int offset = DOLHeader.Size;
            foreach (ModuleSectionNode s in Children)
            {
                int i = s.Index;
                header->TextLoadAddr[i] = hdr.TextLoadAddr[i];
                if (s._dataBuffer != null && s._dataBuffer.Length > 0)
                {
                    int size = s._calcSize;

                    header->TextOffset[i] = (uint)offset;
                    header->TextSize[i] = (uint)size;

                    s.Rebuild(address + offset, size, true);
                    offset += size;
                }
                else
                {
                    header->TextOffset[i] = 0;
                    header->TextSize[i] = 0;
                }
            }

            header->bssOffset = hdr.bssOffset;
            header->bssSize = hdr.bssSize;
            header->entryPoint = hdr.entryPoint;
            for (int i = 0; i < 28; i++)
                header->padding[i] = 0;
        }
    }

    public unsafe interface ModuleNode
    {
        uint ID { get; set; }
        ModuleSectionNode[] Sections { get; }
    }
}
