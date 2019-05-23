using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BrawlLib.SSBBTypes;
using System.ComponentModel;
using System.Runtime.InteropServices;
using BrawlLib.Imaging;
using BrawlLib.Wii;
using System.Windows.Forms;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class GDORNode : ResourceNode
    {
        internal GDOR* Header { get { return (GDOR*)WorkingUncompressed.Address; } }
        public override ResourceType ResourceType { get { return ResourceType.GDOR; } }
        [Category("GDOR")]
        public int Doors { get { return Header->_count; } }

        public override void OnPopulate()
        {
            for (int i = 0; i < Header->_count; i++)
            {
                DataSource source;
                if (i == Header->_count - 1)
                { source = new DataSource((*Header)[i], WorkingUncompressed.Address + WorkingUncompressed.Length - (*Header)[i]); }
                else { source = new DataSource((*Header)[i], (*Header)[i + 1] - (*Header)[i]); }
                new GDOREntryNode().Initialize(this, source);

            }
        }
        public override bool OnInitialize()
        {
            base.OnInitialize();
            if (_name == null)
                _name = "Adventure Doors";
            return Header->_count > 0;
        }
        public override int OnCalculateSize(bool force)
        {
            int size = GDOR.Size + (Children.Count * 4);
            foreach (ResourceNode node in Children)
                size += node.CalculateSize(force);
            return size;
        }
        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            GDOR* header = (GDOR*)address;
            *header = new GDOR(Children.Count);
            uint offset = (uint)(0x08 + (Children.Count * 4));
            for (int i = 0; i < Children.Count; i++)
            {
                if (i > 0) { offset += (uint)(Children[i - 1].CalculateSize(false)); }
                *(buint*)((VoidPtr)address + 0x08 + i * 4) = offset;
                _children[i].Rebuild((VoidPtr)address + offset, _children[i].CalculateSize(false), true);
            }
        }

        internal static ResourceNode TryParse(DataSource source) { return ((GDOR*)source.Address)->_tag == GDOR.Tag ? new GDORNode() : null; }
    }

    public unsafe class GDOREntryNode : ResourceNode
    {
        internal GDOREntry* Header { get { return (GDOREntry*)WorkingUncompressed.Address; } }
        public override ResourceType ResourceType { get { return ResourceType.Unknown; } }
        internal UnsafeBuffer entries;
        [Category("Door Info")]
        [DisplayName("Stage ID")]
        public string FileID
        {
            get
            {
                string s1 = "";
                for (int i = 0; i < 3; i++)
                {
                    int i1 = *(byte*)(Header + 0x30 + i);
                    if (i1 < 10) { s1 += i1.ToString("x").PadLeft(2, '0'); } else { s1 += i1.ToString("x"); }
                }
                return s1;
            }
           
        }

        [Category("Door Info")]
        [DisplayName("Door Index")]
        public string DoorID { get { int i = *(byte*)(Header + 0x33); return i.ToString("x"); } }

        public override bool OnInitialize()
        {
            if (_name == null)
                _name = "Door["+(Index+1)+']';
            entries = new UnsafeBuffer(WorkingUncompressed.Length);
            Memory.Move(entries.Address, Header, (uint)entries.Length);
            return false;
        }
        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            GDOREntry* header = (GDOREntry*)address;
            Memory.Move(header, entries.Address, (uint)entries.Length);
        }
    }
}
