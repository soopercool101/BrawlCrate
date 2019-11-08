using BrawlLib.Internal;
using BrawlLib.SSBB.Types;
using System.ComponentModel;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class SCN0LightSetNode : SCN0EntryNode
    {
        internal SCN0LightSet* Data => (SCN0LightSet*) WorkingUncompressed.Address;
        public override ResourceType ResourceFileType => ResourceType.SCN0LightSet;

        public SCN0LightNode[] _lights = new SCN0LightNode[8];
        public SCN0AmbientLightNode _ambient;
        private byte numLights;

        //private string _ambientLight;
        private string GetLightName(int index)
        {
            if (_lights[index] != null)
            {
                return _lights[index].Name;
            }

            return null;
        }

        //Remaps light entries to an array with no space between entries.
        private void SetLight(int index, string value)
        {
            //int i = 0;
            int i = index.Clamp(0, 8);

            if (value == "<null>")
            {
                value = null;
            }

            if ( /*!*/string.IsNullOrEmpty(value))
                //{
                //    //for (; i < 8; i++)
                //    //    if (GetLightName(i) == value && i != index)
                //    //        return;
                //    for (i = 0; i < index; i++)
                //        if (String.IsNullOrEmpty(GetLightName(i)))
                //            break;
                //}
                //else
            {
                _lights[index] = null;
                //int amt = 0;
                //for (int x = 0; x < 8; x++)
                //    if (_lights[x] == null)
                //        amt++;
                //    else if (amt != 0)
                //    {
                //        _lights[x - amt] = _lights[x];
                //        _lights[x] = null;
                //    }
                UpdateProperties();
                SignalPropertyChange();
                //return;
            }
            //if (i <= index)
            else
            {
                _lights[i] =
                    ((SCN0Node) Parent.Parent).GetFolder<SCN0LightNode>().FindChild(value, false) as SCN0LightNode;
                //if (_lights[i] == null)
                //{
                //    int amt = 0;
                //    for (int x = 0; x < 8; x++)
                //        if (_lights[x] == null)
                //            amt++;
                //        else if (amt != 0)
                //        {
                //            _lights[x - amt] = _lights[x];
                //            _lights[x] = null;
                //        }
                //}
                UpdateProperties();
                SignalPropertyChange();
            }
        }

        [Category("Light Set")]
        [TypeConverter(typeof(DropDownListSCN0Ambience))]
        public string Ambience
        {
            get => _ambient != null ? _ambient.Name : null;
            set
            {
                _ambient =
                    ((SCN0Node) Parent.Parent).GetFolder<SCN0AmbientLightNode>().FindChild(value, false) as
                    SCN0AmbientLightNode;
                SignalPropertyChange();
            }
        }

        [Category("Light Set")]
        [TypeConverter(typeof(DropDownListSCN0Light))]
        public string Light0
        {
            get => GetLightName(0);
            set => SetLight(0, value);
        }

        [Category("Light Set")]
        [TypeConverter(typeof(DropDownListSCN0Light))]
        public string Light1
        {
            get => GetLightName(1);
            set => SetLight(1, value);
        }

        [Category("Light Set")]
        [TypeConverter(typeof(DropDownListSCN0Light))]
        public string Light2
        {
            get => GetLightName(2);
            set => SetLight(2, value);
        }

        [Category("Light Set")]
        [TypeConverter(typeof(DropDownListSCN0Light))]
        public string Light3
        {
            get => GetLightName(3);
            set => SetLight(3, value);
        }

        [Category("Light Set")]
        [TypeConverter(typeof(DropDownListSCN0Light))]
        public string Light4
        {
            get => GetLightName(4);
            set => SetLight(4, value);
        }

        [Category("Light Set")]
        [TypeConverter(typeof(DropDownListSCN0Light))]
        public string Light5
        {
            get => GetLightName(5);
            set => SetLight(5, value);
        }

        [Category("Light Set")]
        [TypeConverter(typeof(DropDownListSCN0Light))]
        public string Light6
        {
            get => GetLightName(6);
            set => SetLight(6, value);
        }

        [Category("Light Set")]
        [TypeConverter(typeof(DropDownListSCN0Light))]
        public string Light7
        {
            get => GetLightName(7);
            set => SetLight(7, value);
        }

        public override bool OnInitialize()
        {
            base.OnInitialize();

            numLights = Data->_numLights;

            return false;
        }

        public void AttachNodes()
        {
            SCN0Node node = (SCN0Node) Parent.Parent;
            SCN0GroupNode g = node.GetFolder<SCN0LightNode>();
            bint* strings = Data->StringOffsets;
            if (g != null && !_replaced)
            {
                for (int i = 0; i < Data->_numLights && i < 8; i++)
                {
                    _lights[i] = g.FindChild(new string((sbyte*) strings + strings[i]), false) as SCN0LightNode;
                }
            }

            g = node.GetFolder<SCN0AmbientLightNode>();
            if (g != null && Data->_ambNameOffset != 0 && !_replaced)
            {
                _ambient = g.FindChild(Data->AmbientString, false) as SCN0AmbientLightNode;
            }
        }

        internal override void GetStrings(StringTable table)
        {
            if (Name != "<null>")
            {
                table.Add(Name);
            }
            else
            {
                return;
            }

            if (_ambient != null && !string.IsNullOrEmpty(_ambient.Name))
            {
                table.Add(_ambient.Name);
            }

            for (int i = 0; i < 8; i++)
            {
                if (!string.IsNullOrEmpty(GetLightName(i)))
                {
                    table.Add(GetLightName(i));
                }
            }
        }

        public override int OnCalculateSize(bool force)
        {
            return SCN0LightSet.Size;
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            base.OnRebuild(address, length, force);

            SCN0LightSet* header = (SCN0LightSet*) address;

            int x = 0;
            for (int i = 0; i < 8; i++)
            {
                if (!string.IsNullOrEmpty(GetLightName(i)))
                {
                    x++;
                }
            }

            header->_pad = 0;
            header->_id = -1;
            header->_numLights = (byte) x;
            bshort* ids = header->IDs;
            int e = 0;
            while (e < 8)
            {
                ids[e++] = -1;
            }
        }

        protected internal override void PostProcess(VoidPtr scn0Address, VoidPtr dataAddress, StringTable stringTable)
        {
            base.PostProcess(scn0Address, dataAddress, stringTable);

            SCN0LightSet* header = (SCN0LightSet*) dataAddress;

            if (_ambient != null && !string.IsNullOrEmpty(_ambient.Name))
            {
                header->AmbientStringAddress = stringTable[_ambient.Name] + 4;
            }
            else
            {
                header->_ambNameOffset = 0;
            }

            int i;
            bint* strings = header->StringOffsets;
            for (i = 0; i < 8; i++)
            {
                if (!string.IsNullOrEmpty(GetLightName(i)))
                {
                    strings[i] = (int) stringTable[GetLightName(i)] + 4 - (int) strings;
                }
                else
                {
                    break;
                }
            }

            while (i < 8)
            {
                strings[i++] = 0;
            }
        }
    }
}