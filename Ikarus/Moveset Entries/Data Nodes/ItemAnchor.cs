using System;
using System.ComponentModel;
using BrawlLib.SSBB.ResourceNodes;

namespace Ikarus.MovesetFile
{
    //public unsafe class MoveDefItemAnchorListNode : MovesetEntry
    //{
    //    internal FDefItemAnchor* First { get { return (FDefItemAnchor*)WorkingUncompressed.Address; } }
    //    int Count = 0;

    //    public override bool OnInitialize()
    //    {
    //        _extOverride = true;
    //        base.OnInitialize();

    //        //if (Size % 0x1C != 0 && Size % 0x1C != 4)
    //        //    Console.WriteLine(Size % 0x1C);

    //        Count = WorkingUncompressed.Length / 0x1C;
    //        return Count > 0;
    //    }

    //    public override void Parse(VoidPtr address)
    //    {
    //        FDefItemAnchor* addr = First;
    //        for (int i = 0; i < Count; i++)
    //            new MoveDefItemAnchorNode() { _hasName = _offsetID == 16 }.Initialize(this, addr++, 28);

    //        if (_offsetID == 16)
    //        {
    //            Children[0]._name = "FranklinBadge/ScrewAttack";
    //            Children[1]._name = "LipStickFlower/BunnyHood";
    //            Children[2]._name = "SuperSpicyCurry";
    //        }
    //        //else if (offsetID == 17)
    //        //{

    //        //}
    //        //else if (offsetID == 23)
    //        //{

    //        //}
    //    }

    //    public override int GetSize()
    //    {
    //        _lookupCount = 0;
    //        return Children.Count * 0x1C;
    //    }

    //    protected override void Write(VoidPtr address)
    //    {
    //        _rebuildAddr = address;
    //        FDefItemAnchor* data = (FDefItemAnchor*)address;
    //        foreach (MoveDefItemAnchorNode e in Children)
    //            e.Rebuild(data++, 0x1C, true);
    //    }
    //}

    public unsafe class ItemAnchor : MovesetEntryNode
    {
        int _boneIndex;
        Vector3 _trans, _rot;

        [Browsable(false)]
        public MDL0BoneNode BoneNode
        {
            get { if (Model == null) return null; if (_boneIndex >= Model._linker.BoneCache.Length || _boneIndex < 0) return null; return (MDL0BoneNode)Model._linker.BoneCache[_boneIndex]; }
            set { _boneIndex = value.BoneIndex; _name = value.Name; }
        }

        [Category("Item Anchor"), Browsable(true), TypeConverter(typeof(DropDownListBonesMDef))]
        public string Bone
        {
            get { return BoneNode == null ? _boneIndex.ToString() : BoneNode.Name; }
            set
            {
                if (Model == null)
                    _boneIndex = Convert.ToInt32(value); 
                else
                    BoneNode = String.IsNullOrEmpty(value) ? BoneNode : Model.FindBone(value); 

                SignalPropertyChange();
            }
        }
        [Category("Item Anchor")]
        public Vector3 Translation { get { return _trans; } set { _trans = value; SignalPropertyChange(); } }
        [Category("Item Anchor")]
        public Vector3 Rotation { get { return _rot; } set { _rot = value; SignalPropertyChange(); } }

        public override string Name
        {
            get
            {
                return String.IsNullOrEmpty(_name) ? Bone : _name;
            }
        }

        protected override void OnParse(VoidPtr address)
        {
            sItemAnchor* hdr = (sItemAnchor*)address;

            _boneIndex = hdr->_boneIndex;

            if (_name == null)
                _name = Bone;

            _trans = hdr->_translation;
            _rot = hdr->_rotation;
        }

        protected override int OnGetSize() { return 0x1C; }

        protected override void OnWrite(VoidPtr address)
        {
            RebuildAddress = address;
            sItemAnchor* data = (sItemAnchor*)address;
            data->_boneIndex = _boneIndex;
            data->_translation = _trans;
            data->_rotation = _rot;
        }
    }
}
