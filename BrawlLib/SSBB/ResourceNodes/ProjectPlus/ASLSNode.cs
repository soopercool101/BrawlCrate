using BrawlLib.Internal;
using BrawlLib.SSBB.Types.ProjectPlus;
using BrawlLib.SSBB.Types.Subspace;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrawlLib.SSBB.ResourceNodes.ProjectPlus
{
    public unsafe class ASLSNode : ResourceNode
    {
        internal ASLS* Header => (ASLS*) WorkingUncompressed.Address;
        public override ResourceType ResourceFileType => ResourceType.ASLS;

        public override void OnPopulate()
        {
            for (int i = 0; i < Header->_count; i++)
            {
                DataSource source = new DataSource((*Header)[i], 0x04);
                new ASLSEntryNode().Initialize(this, source);
            }
        }

        public override bool OnInitialize()
        {
            if (_name == null && _origPath != null)
            {
                _name = Path.GetFileNameWithoutExtension(_origPath);
            }
            return Header->_count > 0;
        }

        public override int OnCalculateSize(bool force)
        {
            int size = (int)ASLS.HeaderSize;
            foreach (ResourceNode n in Children)
            {
                size += (int) ASLSEntry.Size + n.Name.Length + 1;
            }

            return size;
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            ASLS* header = (ASLS*)address;
            *header = new ASLS();
            header->_tag = ASLS.Tag;
            header->_count = (short)Children.Count;
            header->_nameOffset = (short) (ASLS.HeaderSize + Children.Count * ASLSEntry.Size);

            uint offset = ASLS.HeaderSize;
            int strOffset = 0;
            foreach (ResourceNode n in Children)
            {
                int size = n.CalculateSize(false);
                ((ASLSEntryNode) n).StrOffset = (ushort)strOffset;
                n.Rebuild(address + offset, size, true);
                offset += (uint)size;
                strOffset += n.Name.Length + 1;
            }
            foreach (ResourceNode n in Children)
            {
                sbyte* ptr = (sbyte*)(address + offset);
                string name = n.Name;
                for (int j = 0; j < name.Length; j++)
                {
                    ptr[j] = (sbyte)name[j];
                }
                ptr[name.Length] = 0;
                offset += (uint)(n.Name.Length + 1);
            }
        }

        internal static ResourceNode TryParse(DataSource source)
        {
            return source.Tag == "ASLS" ? new ASLSNode() : null;
        }
    }

    public unsafe class ASLSEntryNode : ResourceNode
    {
        internal ASLSEntry* Header => (ASLSEntry*)WorkingUncompressed.Address;

        public ushort ButtonFlags
        {
            get => _buttonFlags;
            set
            {
                _buttonFlags = value;
                SignalPropertyChange();
            }
        }

        [Flags]
        public enum WiiNunchukButtons : ushort
        {
            Left = 0x0001,
            Right = 0x0002,
            Down = 0x0004,
            Up = 0x0008,
            Plus = 0x0010,
            Unused0x0020 = 0x0020,
            Unused0x0040 = 0x0040,
            Unused0x0080 = 0x0080,
            Button2 = 0x0100,
            Button1 = 0x0200,
            B = 0x0400,
            A = 0x0800,
            Minus = 0x1000,
            Z = 0x2000,
            C = 0x4000,
            Home = 0x8000
        }

        [Flags]
        public enum GameCubeButtons : ushort
        {
            Left = 0x0001,
            Right = 0x0002,
            Down = 0x0004,
            Up = 0x0008,
            Z = 0x0010,
            R = 0x0020,
            L = 0x0040,
            Unused0x0080 = 0x0080,
            A = 0x0100,
            B = 0x0200,
            X = 0x0400,
            Y = 0x0800,
            Start = 0x1000,
            Unused0x2000 = 0x2000,
            Unused0x4000 = 0x4000,
            Unused0x8000 = 0x8000
        }

        [Flags]
        public enum ClassicButtons : ushort
        {
            Up = 0x0001,
            Left = 0x0002,
            ZR = 0x0004,
            X = 0x0008,
            A = 0x0010,
            Y = 0x0020,
            B = 0x0040,
            ZL = 0x0080,
            Unused0x0100 = 0x0100,
            R = 0x0200,
            Plus = 0x0400,
            Unused0x0800 = 0x0800,
            Minus = 0x1000,
            L = 0x2000,
            Down = 0x4000,
            Right = 0x8000
        }

        private ushort _buttonFlags;
        public ushort StrOffset;

        public override int OnCalculateSize(bool force)
        {
            return (int)ASLSEntry.Size;
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            ASLSEntry* header = (ASLSEntry*)address;
            *header = new ASLSEntry();
            header->_buttonFlags = _buttonFlags;
            header->_nameOffset = StrOffset;
        }

        public override bool OnInitialize()
        {
            _buttonFlags = Header->_buttonFlags;
            _name = new string((sbyte*) ((VoidPtr) ((ASLSNode) Parent).Header) +
                               ((ASLSNode) Parent).Header->_nameOffset + Header->_nameOffset);
            return false;
        }
    }
}