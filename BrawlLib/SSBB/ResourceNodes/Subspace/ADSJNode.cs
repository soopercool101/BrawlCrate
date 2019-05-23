using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BrawlLib.SSBBTypes;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class ADSJNode : ARCEntryNode
    {
        internal ADSJ* Header { get { return (ADSJ*)WorkingUncompressed.Address; } }
        public override ResourceType ResourceType { get { return ResourceType.ADSJ; } }

        [Category("ADSJ")]
        [DisplayName("Entries")]
        public int count { get { return Header->_count; } }
        public override void OnPopulate()
        {
            for (int i = 0; i < Header->_count; i++)
            {
                DataSource source;
                if (i == Header->_count - 1)
                { source = new DataSource((*Header)[i], WorkingUncompressed.Address + WorkingUncompressed.Length - (*Header)[i]); }
                else { source = new DataSource((*Header)[i], (*Header)[i + 1] - (*Header)[i]); }
                new ADSJEntryNode().Initialize(this, source);
            }
        }
        public override bool OnInitialize()
        {
            base.OnInitialize();
            return Header->_count > 0;
        }

        internal static ResourceNode TryParse(DataSource source) { 
            return ((ADSJ*)source.Address)->_tag == ADSJ.Tag ? new ADSJNode() : null; }
    }

    public unsafe class ADSJEntryNode : ResourceNode
    {
        internal ADSJEntry* Header { get { return (ADSJEntry*)WorkingUncompressed.Address; } }
        public override ResourceType ResourceType { get { return ResourceType.Unknown; } }
        [Category("Jump Info")]
        [DisplayName("Hexadecimal ID")]
        public string HexID
        {
            get
            {
                string s1 = "";
                for (int i = 0; i < 4; i++)
                {
                    int i1 = *(byte*)(Header + 0x08 + i);
                    if (i1 < 10) { s1 += i1.ToString("x").PadLeft(2, '0'); } else { s1 += i1.ToString("x"); }
                }
                return s1;
            }
        }
        
        [Category("Jump Info")]
        [DisplayName("Decimal ID")]
        public string DecID
        {
            get
            {
                string s1 = "";
                for (int i = 0; i < 4; i++)
                {
                    int i1 = *(byte*)(Header + i);
                    if (i1 < 10) { s1 += i1.ToString("x").PadLeft(2, '0'); } else { s1 += i1.ToString("x"); }
                }
                return s1; 
            }
        }
        
              
        [Category("Jump Flags")]
        public byte Flag1
        {
            get
            {
                return *(byte*)(Header+0x04);
            }
        }
        [Category("Jump Flags")]
        public byte Flag2
        {
            get
            {
                return *(byte*)(Header + 0x05);
            }
        }
        [Category("Jump Flags")]
        public byte Flag3
        {
            get
            {
                return *(byte*)(Header + 0x06);
            }
        }
        [Category("Jump Flags")]
        public byte Flag4
        {
            get
            {
                return *(byte*)(Header + 0x07);
            }
        }

        public override bool OnInitialize()
        {
            base.OnInitialize();
            if (_name == null)
                _name = new String((sbyte*)(Header + 0x0C));
            return false;
        }
    }
}