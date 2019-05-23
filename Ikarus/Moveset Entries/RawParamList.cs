using System;
using System.Collections.Generic;
using System.ComponentModel;
using BrawlLib.SSBB.ResourceNodes;

namespace Ikarus.MovesetFile
{
    public unsafe class BoneIndexValue : MovesetEntryNode
    {
        [Browsable(false)]
        public MDL0BoneNode BoneNode
        {
            get
            {
                if (//ParentArticle == null && 
                    Model == null)
                    return null;

                MDL0Node model;
                //if (ParentArticle != null && ParentArticle._info != null)
                //    model = ParentArticle._info._model;
                //else
                model = Model;

                if (boneIndex >= model._linker.BoneCache.Length || boneIndex < 0)
                    return null;

                return (MDL0BoneNode)model._linker.BoneCache[boneIndex];
            }
            set
            {
                boneIndex = value.BoneIndex;
                _name = value.Name;
            }
        }

        [Category("Bone Index"), TypeConverter(typeof(DropDownListBonesMDef))]
        public string Bone { get { return BoneNode == null ? boneIndex.ToString() : BoneNode.Name; } set { if (Model == null) { boneIndex = Convert.ToInt32(value); _name = boneIndex.ToString(); } else { BoneNode = String.IsNullOrEmpty(value) ? BoneNode : Model.FindBone(value); } SignalPropertyChange(); } }
        internal int boneIndex = 0;

        public override string Name { get { return Bone; } }

        protected override void OnParse(VoidPtr address) { boneIndex = *(bint*)address; }
        protected override int OnGetSize() { return 4; }
        protected override void OnWrite(VoidPtr address)
        {
            *(bint*)(RebuildAddress = address) = boneIndex;
        }
    }

    public unsafe class RawParamList : MovesetEntryNode
    {
        public RawParamList() { }
        public RawParamList(int size) { _initSize = size; }

        public List<AttributeInfo> _info;
        public string _nameID;

        public new string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                Manager.Params[_nameID]._newName = value;
                Manager._dictionaryChanged = true;
            }
        }

        [Browsable(false)]
        public UnsafeBuffer AttributeBuffer { get { return attributeBuffer; } }
        private UnsafeBuffer attributeBuffer;

        protected override void OnParse(VoidPtr address)
        {
            _nameID = _name;

            if (_initSize == 0)
                throw new Exception("Nothing to read");

            CharacterInfo cInfo = Manager.SelectedInfo;

            SectionParamInfo data = null;
            if (_name != null && cInfo._parameters.ContainsKey(_name))
            {
                data = cInfo._parameters[_name];
                _info = data._attributes;
                if (!String.IsNullOrEmpty(data._newName))
                    _name = data._newName;
            }
            else _info = new List<AttributeInfo>();

            if (_initSize > 0)
            {
                attributeBuffer = new UnsafeBuffer(_initSize);
                byte* pOut = (byte*)attributeBuffer.Address;
                byte* pIn = (byte*)address;

                for (int i = 0; i < _initSize; i++)
                {
                    if (i % 4 == 0)
                    {
                        if (data == null)
                        {
                            AttributeInfo info = new AttributeInfo();

                            //Guess if the value is a an integer or float
                            if (((((uint)*((buint*)pIn)) >> 24) & 0xFF) != 0 && *((bint*)pIn) != -1 && !float.IsNaN(((float)*((bfloat*)pIn))))
                                info._type = 0;
                            else
                                info._type = 1;

                            info._name = (info._type == 1 ? "*" : "") + "0x" + i.ToString("X");
                            info._description = "No Description Available.";

                            _info.Add(info);
                        }
                    }
                    *pOut++ = *pIn++;
                }
            }
        }

        protected override void OnWrite(VoidPtr address)
        {
            RebuildAddress = address;
            byte* pIn = (byte*)attributeBuffer.Address;
            byte* pOut = (byte*)address;
            for (int i = 0; i < attributeBuffer.Length; i++)
                *pOut++ = *pIn++;
        }

        protected override int OnGetSize()
        {
            _entryLength = attributeBuffer.Length;
            return _entryLength;
        }
    }
}
