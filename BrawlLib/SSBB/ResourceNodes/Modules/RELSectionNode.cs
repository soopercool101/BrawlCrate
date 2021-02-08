using BrawlLib.Internal;
using BrawlLib.Internal.IO;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;

namespace BrawlLib.SSBB.ResourceNodes
{
    public class ModuleSectionNode : ModuleDataNode
    {
        private ObjectParser _parser;

        internal VoidPtr Header => WorkingUncompressed.Address;
        public override ResourceType ResourceFileType => ResourceType.RELSection;

        [Browsable(false)] public override uint ASMOffset => (uint) _dataOffset;

        public bool _isCodeSection = false;
        public bool _isBSSSection;
        public int _dataOffset;
        public int _endBufferSize;
        public uint _dataSize;
        public int _dataAlign;

        public string DataAlign => "0x" + _dataAlign.ToString("X");
        
        [Category("End Buffer")]
        public int EndBufferSize
        {
            get => _endBufferSize;
            set
            {
                if (value % 4 != 0 &&
                    MessageBox.Show(
                        "Buffers should generally be multiples of 0x4, are you sure you want to set this? (It may make the module unreadable!)",
                        "", MessageBoxButtons.YesNo) != DialogResult.Yes)
                {
                    return;
                }

                if (value < 0 && !ExpandSection)
                {
                    return;
                }

                _endBufferSize = value;
                SignalPropertyChange();
            }
        }


        private bool _expand = false;
        [Category("End Buffer")]
        public bool ExpandSection
        {
            get => _expand;
            set
            {
                _expand = value;
                SignalPropertyChange();
            }

        }

        [Category("REL Section")] public bool HasCommands => _manager._commands.Count > 0;

        [Category("REL Section")]
        [Browsable(true)]
        public override bool HasCode => _isCodeSection;

        [Category("REL Section")] public bool IsBSS => _isBSSSection;

        public ModuleSectionNode()
        {
        }

        public ModuleSectionNode(uint size)
        {
            InitBuffer(size);
        }

        public override bool OnInitialize()
        {
            _expand = false;
            if (_name == null)
            {
                if (_dataSize > 0)
                {
                    _name = $"Section [{Index}]";
                }
                else
                {
                    _name = $"null [{Index}]";
                }
            }

            if (_dataOffset == 0 && WorkingUncompressed.Length != 0)
            {
                _isBSSSection = true;
                InitBuffer(_dataSize);
            }
            else
            {
                _isBSSSection = false;
                InitBuffer(_dataSize, Header);
                if (Index == 5)
                {
                    _parser = new ObjectParser(this);
                }
            }

            return _parser != null;
        }

        public override void OnPopulate()
        {
            _parser?.Parse();
            _parser?.Populate();
        }

        public override int OnCalculateSize(bool force)
        {
            return _dataBuffer.Length + _endBufferSize;
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            Memory.Move(address, _dataBuffer.Address, (uint) length);
            address += _dataBuffer.Length;
            if (_endBufferSize > 0)
            {
                Memory.Fill(address, (uint)_endBufferSize, 0x00);
            }
        }

        public override void Dispose()
        {
            _dataBuffer?.Dispose();

            base.Dispose();
        }

        //public unsafe void ExportInitialized(string outPath)
        //{
        //    using (FileStream stream = new FileStream(outPath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None, 8, FileOptions.RandomAccess))
        //    {
        //        stream.SetLength(_dataBuffer.Length);
        //        using (FileMap map = FileMap.FromStream(stream))
        //        {
        //            buint* addr = (buint*)map.Address;
        //            foreach (Relocation loc in Relocations)
        //                *addr++ = loc.SectionOffset;
        //        }
        //    }
        //}

        public override void Export(string outPath)
        {
            using (FileStream stream = new FileStream(outPath, FileMode.OpenOrCreate, FileAccess.ReadWrite,
                FileShare.None, 8, FileOptions.RandomAccess))
            {
                stream.SetLength(_dataBuffer.Length);
                using (FileMap map = FileMap.FromStream(stream))
                {
                    Memory.Move(map.Address, _dataBuffer.Address, (uint) _dataBuffer.Length);
                }
            }
        }
    }

    //public class RELObjectSectionNode : ModuleSectionNode
    //{
    //    ObjectParser _objectParser;

    //    public void ParseObjects()
    //    {
    //        (_objectParser = new ObjectParser(this)).Parse();
    //    }

    //    public override bool OnInitialize()
    //    {
    //        base.OnInitialize();
    //        return _objectParser._objects.Count > 0;
    //    }

    //    public override void OnPopulate()
    //    {
    //        _objectParser.Populate();
    //    }
    //}

    //public unsafe class RELConstantsSectionNode : ModuleSectionNode
    //{
    //    float[] _values;
    //    public float[] Values { get { return _values; } set { _values = value; } }

    //    public override bool OnInitialize()
    //    {
    //        base.OnInitialize();
    //        _values = new float[_dataBuffer.Length / 4];
    //        bfloat* values = (bfloat*)_dataBuffer.Address;
    //        for (int i = 0; i < _values.Length; i++)
    //            _values[i] = values[i];
    //        return false;
    //    }
    //    public override void OnPopulate()
    //    {
    //        _values = new float[_dataBuffer.Length / 4];
    //        bfloat* values = (bfloat*)_dataBuffer.Address;
    //        for (int i = 0; i < _values.Length; i++)
    //            _values[i] = values[i];
    //    }

    //public override int OnCalculateSize(bool force)
    //{
    //    return _values.Length * 4;
    //}

    //public override void OnRebuild(VoidPtr address, int length, bool force)
    //{
    //    bfloat* values = (bfloat*)address;
    //    for (int i = 0; i < _values.Length; i++)
    //        values[i] = _values[i];
    //}
    //}

    //public class RELStructorSectionNode : ModuleSectionNode
    //{
    //    public bool _destruct;
    //    public override bool OnInitialize()
    //    {
    //        base.OnInitialize();
    //        for (int i = 0; i < _relocations.Count; i++)
    //            if (_relocations[i].RelOffset > 0)
    //                return true;
    //        return false;
    //    }
    //    public override void OnPopulate()
    //    {
    //        for (int i = 0; i < _relocations.Count; i++)
    //            if (_relocations[i].RelOffset > 0)
    //                new RELDeConStructorNode() { _destruct = _destruct, _index = i }.Initialize(this, (VoidPtr)BaseAddress + _relocations[i].RelOffset, 0);
    //    }
    //}
}