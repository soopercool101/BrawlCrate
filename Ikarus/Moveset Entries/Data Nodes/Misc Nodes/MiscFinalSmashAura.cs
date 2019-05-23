using BrawlLib.SSBB.ResourceNodes;
using System;
using System.ComponentModel;

namespace Ikarus.MovesetFile
{
    public unsafe class MiscFinalSmashAura : MovesetEntryNode
    {
        internal int _boneIndex = 0;
        internal float _x, _y, _width, _height;

        [Browsable(false)]
        public MDL0BoneNode BoneNode
        {
            get { if (Model == null) return null; if (_boneIndex >= Model._linker.BoneCache.Length || _boneIndex < 0) return null; return (MDL0BoneNode)Model._linker.BoneCache[_boneIndex]; }
            set { _boneIndex = value.BoneIndex; _name = value.Name; }
        }

        [Category("Final Smash Aura"), Browsable(true), TypeConverter(typeof(DropDownListBonesMDef))]
        public string Bone { get { return BoneNode == null ? _boneIndex.ToString() : BoneNode.Name; } set { if (Model == null) { _boneIndex = Convert.ToInt32(value); _name = _boneIndex.ToString(); } else { BoneNode = String.IsNullOrEmpty(value) ? BoneNode : Model.FindBone(value); } SignalPropertyChange(); } }
        [Category("Final Smash Aura"), TypeConverter(typeof(Vector2StringConverter))]
        public Vector2 XY { get { return new Vector2(_x, _y); } set { _x = value._x; _y = value._y; SignalPropertyChange(); } }
        [Category("Final Smash Aura")]
        public float Height { get { return _width; } set { _height = value; SignalPropertyChange(); } }
        [Category("Final Smash Aura")]
        public float Width { get { return _height; } set { _width = value; SignalPropertyChange(); } }

        public override string Name { get { return Bone; } }

        protected override void OnParse(VoidPtr address)
        {
            sMiscFSAura* hdr = (sMiscFSAura*)address;

            _boneIndex = hdr->_boneIndex;
            _x = hdr->_x;
            _y = hdr->_y;
            _width = hdr->_width;
            _height = hdr->_height;
        }

        protected override int OnGetSize() { return 0x14; }

        protected override void OnWrite(VoidPtr address)
        {
            RebuildAddress = address;
            sMiscFSAura* header = (sMiscFSAura*)address;
            header->_boneIndex = _boneIndex;
            header->_height = _height;
            header->_width = _width;
            header->_x = _x;
            header->_y = _y;
        }
    }
}
