using BrawlLib.Internal;
using BrawlLib.Internal.PowerPCAssembly;
using System.ComponentModel;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class RELMethodNode : RELEntryNode
    {
        internal VoidPtr Header => WorkingUncompressed.Address;

        public override ResourceType ResourceFileType =>
            WorkingUncompressed.Address ? ResourceType.RELMethod : ResourceType.RELExternalMethod;

        [Browsable(false)] public RELObjectNode Object => Parent.Parent as RELObjectNode;

        [Browsable(false)] public string FullName => (Object != null ? Object.Type.FullName + "." : "") + _name;

        public RelCommand _cmd;

        [Category("Method")]
        [DisplayName("Data Size")]
        [Description("The data length of the code in this method")]
        public string DataSize => "0x" + _codeLen.ToString("X");

        [Category("Method")]
        [DisplayName("Module ID")]
        [Description("Name of the target module which the assembly code for this method resides")]
        public string TargetModule =>
            RELNode._idNames.ContainsKey(_cmd._moduleID) ? RELNode._idNames[_cmd._moduleID] : "";

        [Category("Method")]
        [DisplayName("Section Index")]
        [Description("The section in which this method's assembly code is located")]
        public uint TargetSection => _cmd.TargetSectionID;

        [Category("Method")]
        [DisplayName("Section Offset")]
        [Description("Offset of the method's asssembly code within the target module, relative to the target section")]
        [TypeConverter(typeof(HexUIntConverter))]
        public uint TargetOffset => _cmd.TargetOffset;

        public int _codeStart, _codeLen;
        public RelocationManager _manager;

        public override bool OnInitialize()
        {
            ModuleSectionNode section = Location;

            if ((TargetSection == 1 || TargetModule.Equals("main.dol")) && ModuleMapLoader.MapFiles.ContainsKey(TargetModule))
            {
                if (ModuleMapLoader.MapFiles[TargetModule].ContainsKey(TargetOffset))
                {
                    _name = ModuleMapLoader.MapFiles[TargetModule][TargetOffset];
                }
            }

            if (section == null || !Header)
            {
                return false;
            }

            //Don't make a copy buffer here.
            //Use the original buffer to save memory

            buint* sPtr = (buint*) Header;
            VoidPtr ceil = section.Header + section._dataSize;

            while (!(PowerPC.Disassemble(*sPtr++) is PPCblr) && (int) sPtr < (int) ceil)
            {
                ;
            }

            _codeStart = (int) Header - (int) section.Header;
            _codeLen = (int) sPtr - (int) Header;

            _manager = new RelocationManager(null);
            _manager.UseReference(section, _codeStart);

            if (_codeLen > 0)
            {
                _manager.AddTag(0, FullName + " Start");
                _manager.AddTag(_codeLen / 4 - 1, FullName + " End");
            }

            return false;
        }

        public override void Export(string outPath)
        {
            //using (FileStream stream = new FileStream(outPath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None, 8, FileOptions.RandomAccess))
            //{
            //    stream.SetLength(_dataBuffer.Length);
            //    using (FileMap map = FileMap.FromStream(stream))
            //        Memory.Move(map.Address, _dataBuffer.Address, (uint)_dataBuffer.Length);
            //}
        }
    }
}