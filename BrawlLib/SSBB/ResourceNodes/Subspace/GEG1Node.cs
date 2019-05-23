using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BrawlLib.SSBBTypes;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class GEG1Node : ResourceNode
    {
        internal GEG1* Header { get { return (GEG1*)WorkingUncompressed.Address; } }
        public override ResourceType ResourceType { get { return ResourceType.GEG1; } }

        [Category("GEG1")]
        [DisplayName("Enemy Count")]
        public int count { get { return Header->_count; } }
        public override void OnPopulate()
        {
            for (int i = 0; i < Header->_count; i++)
            {
                DataSource source;
                if (i == Header->_count - 1)
                { source = new DataSource((*Header)[i], WorkingUncompressed.Address + WorkingUncompressed.Length - (*Header)[i]); }
                else { source = new DataSource((*Header)[i], (*Header)[i + 1] - (*Header)[i]); }
                new GEG1EntryNode().Initialize(this, source);
            }
        }
        public override bool OnInitialize()
        {
            base.OnInitialize();
            if (_name == null)
                _name = "GEG1";
            return Header->_count > 0;
        }

        internal static ResourceNode TryParse(DataSource source) { return ((GEG1*)source.Address)->_tag == GEG1.Tag ? new GEG1Node() : null; }
    }

    public unsafe class GEG1EntryNode : ResourceNode
    {
        internal GEG1Entry* Header { get { return (GEG1Entry*)WorkingUncompressed.Address; } }
        public override ResourceType ResourceType { get { return ResourceType.ENEMY; } }

        [Browsable(true), TypeConverter(typeof(DropDownListEnemies))]
        [Category("Enemy Info")]
        [DisplayName("Enemy Type")]
        public string Enemy
        {
            get
            {
                int Enemy = *(byte*)(WorkingUncompressed.Address + 0x1D);
                switch (Enemy)
                {
                    case 15: return "Spaak";
                    case 23: return "Prim";
                    case 20: return "BoxerPrim";
                    case 32: return "BoomPrim";
                    case 35: return "SwordPrim";
                    default: return "Unknown";
                }
            }
        }
        public override bool OnInitialize()
        {
            base.OnInitialize();
            if (_name == null)
                _name = "Enemy[" + Index + ']';
            return false;
        }
    }
}
