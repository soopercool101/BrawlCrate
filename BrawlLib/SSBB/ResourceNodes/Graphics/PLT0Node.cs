using BrawlLib.Imaging;
using BrawlLib.Internal;
using BrawlLib.SSBB.Types;
using BrawlLib.Wii.Textures;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class PLT0Node : BRESEntryNode, IColorSource
    {
        internal PLT0v1* Header1 => (PLT0v1*) WorkingUncompressed.Address;
        internal PLT0v3* Header3 => (PLT0v3*) WorkingUncompressed.Address;
        internal BRESCommonHeader* Header => (BRESCommonHeader*) WorkingUncompressed.Address;
        public override ResourceType ResourceFileType => ResourceType.PLT0;
        public override int DataAlign => 0x20;
        public override int[] SupportedVersions => new int[] {1, 3};
        public override string Tag => "PLT0";

        public PLT0Node()
        {
            _version = 1;
        }

        private WiiPaletteFormat _format;
        private ColorPalette _palette;

        [Browsable(false)]
        public ColorPalette Palette
        {
            get => _palette ?? (_palette = TextureConverter.DecodePalette(Header1));
            set
            {
                _palette = value;
                SignalPropertyChange();
            }
        }

        [Category("G3D Palette")] public int Colors => Palette.Entries.Length; // set { _numColors = value; } }

        [Category("G3D Palette")]
        public WiiPaletteFormat Format
        {
            get => _format;
            set
            {
                _format = value;
                SignalPropertyChange();
            }
        }

        public override bool OnInitialize()
        {
            base.OnInitialize();

            _palette = null;

            if (_name == null && Header1->_stringOffset != 0)
            {
                _name = Header1->ResourceString;
            }

            _format = Header1->PaletteFormat;

            if (_version == 3)
            {
                (_userEntries = new UserDataCollection()).Read(Header3->UserData, WorkingUncompressed);
            }

            return false;
        }

        internal override void GetStrings(StringTable table)
        {
            table.Add(Name);

            if (_version == 3)
            {
                _userEntries.GetStrings(table);
            }

            if (!string.IsNullOrEmpty(_originalPath))
            {
                table.Add(_originalPath);
            }
        }

        public override int OnCalculateSize(bool force)
        {
            int count = Palette.Entries.Length.Align(16);
            return 0x40 + count * 2;
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            PLT0v1* header = (PLT0v1*) address;
            *header = new PLT0v1(Palette.Entries.Length, _format);

            TextureConverter.EncodePalette(address + 0x40, Palette, _format);
        }

        protected internal override void PostProcess(VoidPtr bresAddress, VoidPtr dataAddress, int dataLength,
                                                     StringTable stringTable)
        {
            base.PostProcess(bresAddress, dataAddress, dataLength, stringTable);

            PLT0v1* header = (PLT0v1*) dataAddress;
            header->ResourceStringAddress = stringTable[Name] + 4;
            if (!string.IsNullOrEmpty(_originalPath))
            {
                header->OrigPathAddress = stringTable[_originalPath] + 4;
            }
        }

        #region IColorSource Members

        public bool HasPrimary(int id)
        {
            return false;
        }

        public ARGBPixel GetPrimaryColor(int id)
        {
            return new ARGBPixel();
        }

        public void SetPrimaryColor(int id, ARGBPixel color)
        {
        }

        [Browsable(false)]
        public string PrimaryColorName(int id)
        {
            return null;
        }

        [Browsable(false)] public int TypeCount => 1;

        [Browsable(false)]
        public int ColorCount(int id)
        {
            return Palette.Entries.Length;
        }

        public ARGBPixel GetColor(int index, int id)
        {
            return (ARGBPixel) Palette.Entries[index];
        }

        public void SetColor(int index, int id, ARGBPixel color)
        {
            Palette.Entries[index] = (Color) color;
            SignalPropertyChange();
        }

        public bool GetColorConstant(int id)
        {
            return false;
        }

        public void SetColorConstant(int id, bool constant)
        {
        }

        #endregion

        internal static ResourceNode TryParse(DataSource source, ResourceNode parent)
        {
            return ((PLT0v1*) source.Address)->_bresEntry._tag == PLT0v1.Tag ? new PLT0Node() : null;
        }
    }
}