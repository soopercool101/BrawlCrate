using BrawlLib.Internal;
using BrawlLib.SSBB.Types;
using System.ComponentModel;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class MVPMNode : ARCEntryNode
    {
        internal Parameter* Header => (Parameter*) WorkingUncompressed.Address;
        //public override ResourceType ResourceFileType => ResourceType.ADPM;

        public override bool OnInitialize()
        {
            base.OnInitialize();

            return Header->_count > 0;
        }

        protected override string GetName()
        {
            return base.GetName("Movie Parameters");
        }

        private const int _entrySize = 96;

        public override void OnPopulate()
        {
            for (int i = 0; i < Header->_count; i++)
            {
                new MVPMEntryNode().Initialize(this, new DataSource((*Header)[i], _entrySize));
            }
        }

        public override int OnCalculateSize(bool force)
        {
            return 0x10 + Children.Count * 4 + Children.Count * _entrySize;
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            Parameter* header = (Parameter*) address;
            *header = new Parameter(Parameter.TagMVPM, Children.Count);
            uint offset = (uint) (0x10 + Children.Count * 4);
            for (int i = 0; i < Children.Count; i++)
            {
                ResourceNode r = Children[i];
                *(buint*) (address + 0x10 + i * 4) = offset;
                r.Rebuild(address + offset, _entrySize, true);
                offset += _entrySize;
            }
        }

        internal static ResourceNode TryParse(DataSource source)
        {
            return ((Parameter*) source.Address)->_tag == Parameter.TagMVPM ? new MVPMNode() : null;
        }
    }

    public unsafe class MVPMEntryNode : ResourceNode
    {
        internal MovParameterEntry* Header => (MovParameterEntry*) WorkingUncompressed.Address;
        public override ResourceType ResourceFileType => ResourceType.Unknown;

        private byte echo;
        private byte id2;
        private ushort id;

        private ParameterValueManager _values = new ParameterValueManager(null);

        [Category("MVPM Values")]
        public int Value1
        {
            get => _values.GetInt(0);
            set
            {
                _values.SetInt(0, value);
                SignalPropertyChange();
            }
        }

        [Category("MVPM Values")]
        public int Value2
        {
            get => _values.GetInt(1);
            set
            {
                _values.SetInt(1, value);
                SignalPropertyChange();
            }
        }

        [Category("MVPM Values")]
        public int Value3
        {
            get => _values.GetInt(2);
            set
            {
                _values.SetInt(2, value);
                SignalPropertyChange();
            }
        }

        [Category("MVPM Values")]
        public int Value4
        {
            get => _values.GetInt(3);
            set
            {
                _values.SetInt(3, value);
                SignalPropertyChange();
            }
        }

        [Category("MVPM Values")]
        public int Value5
        {
            get => _values.GetInt(4);
            set
            {
                _values.SetInt(4, value);
                SignalPropertyChange();
            }
        }

        [Category("MVPM Values")]
        public int Value6
        {
            get => _values.GetInt(5);
            set
            {
                _values.SetInt(5, value);
                SignalPropertyChange();
            }
        }

        [Category("MVPM Values")]
        public int Value7
        {
            get => _values.GetInt(6);
            set
            {
                _values.SetInt(6, value);
                SignalPropertyChange();
            }
        }

        [Category("MVPM Values")]
        public int Value8
        {
            get => _values.GetInt(7);
            set
            {
                _values.SetInt(7, value);
                SignalPropertyChange();
            }
        }

        [Category("MVPM Values")]
        public int Value9
        {
            get => _values.GetInt(8);
            set
            {
                _values.SetInt(8, value);
                SignalPropertyChange();
            }
        }

        [Category("MVPM Values")]
        public int Value10
        {
            get => _values.GetInt(9);
            set
            {
                _values.SetInt(9, value);
                SignalPropertyChange();
            }
        }

        [Category("MVPM Values")]
        public int Value11
        {
            get => _values.GetInt(10);
            set
            {
                _values.SetInt(10, value);
                SignalPropertyChange();
            }
        }

        [Category("MVPM Values")]
        public int Value12
        {
            get => _values.GetInt(11);
            set
            {
                _values.SetInt(11, value);
                SignalPropertyChange();
            }
        }

        [Category("MVPM Values")]
        public int Value13
        {
            get => _values.GetInt(12);
            set
            {
                _values.SetInt(12, value);
                SignalPropertyChange();
            }
        }

        [Category("MVPM Values")]
        public int Value14
        {
            get => _values.GetInt(13);
            set
            {
                _values.SetInt(13, value);
                SignalPropertyChange();
            }
        }

        [Category("MVPM Values")]
        public int Value15
        {
            get => _values.GetInt(14);
            set
            {
                _values.SetInt(14, value);
                SignalPropertyChange();
            }
        }

        [Category("MVPM Values")]
        public int Value16
        {
            get => _values.GetInt(15);
            set
            {
                _values.SetInt(15, value);
                SignalPropertyChange();
            }
        }

        [Category("MVPM Values")]
        public int Value17
        {
            get => _values.GetInt(16);
            set
            {
                _values.SetInt(16, value);
                SignalPropertyChange();
            }
        }

        [Category("MVPM Values")]
        public int Value18
        {
            get => _values.GetInt(17);
            set
            {
                _values.SetInt(17, value);
                SignalPropertyChange();
            }
        }

        [Category("MVPM Values")]
        public int Value19
        {
            get => _values.GetInt(18);
            set
            {
                _values.SetInt(18, value);
                SignalPropertyChange();
            }
        }

        [Category("MVPM Values")]
        public int Value20
        {
            get => _values.GetInt(19);
            set
            {
                _values.SetInt(19, value);
                SignalPropertyChange();
            }
        }

        [Category("MVPM Values")]
        public int Value21
        {
            get => _values.GetInt(20);
            set
            {
                _values.SetInt(20, value);
                SignalPropertyChange();
            }
        }

        [Category("MVPM Values")]
        public int Value22
        {
            get => _values.GetInt(21);
            set
            {
                _values.SetInt(21, value);
                SignalPropertyChange();
            }
        }

        [Category("MVPM Values")]
        public int Value23
        {
            get => _values.GetInt(22);
            set
            {
                _values.SetInt(22, value);
                SignalPropertyChange();
            }
        }

        [Category("MVPM Values")]
        public int Value24
        {
            get => _values.GetInt(23);
            set
            {
                _values.SetInt(23, value);
                SignalPropertyChange();
            }
        }

        public override bool OnInitialize()
        {
            if (_name == null)
            {
                _name = "MVPMEntry " + id;
            }

            _values = new ParameterValueManager((VoidPtr) Header);

            return false;
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            MovParameterEntry* header = (MovParameterEntry*) address;
            *header = new MovParameterEntry();
            byte* pOut = (byte*) header;
            byte* pIn = (byte*) _values._values.Address;
            for (int i = 0; i < 24 * 4; i++)
            {
                *pOut++ = *pIn++;
            }
        }
    }
}