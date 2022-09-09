using BrawlLib.Internal;
using BrawlLib.SSBB.Types;
using System.ComponentModel;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class ADPMNode : ARCEntryNode
    {
        internal Parameter* Header => (Parameter*) WorkingUncompressed.Address;
        public override ResourceType ResourceFileType => ResourceType.ADPM;

        public override bool OnInitialize()
        {
            base.OnInitialize();

            return Header->_count > 0;
        }

        protected override string GetName()
        {
            return base.GetName("Adventure Parameters");
        }

        private const int _entrySize = 260;

        public override void OnPopulate()
        {
            for (int i = 0; i < Header->_count; i++)
            {
                new ADPMEntryNode().Initialize(this, new DataSource((*Header)[i], _entrySize));
            }
        }

        public override int OnCalculateSize(bool force)
        {
            return 0x10 + Children.Count * 4 + Children.Count * _entrySize;
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            Parameter* header = (Parameter*) address;
            *header = new Parameter(Parameter.TagADPM, Children.Count);
            uint offset = (uint) (0x10 + Children.Count * 4);
            for (int i = 0; i < Children.Count; i++)
            {
                ResourceNode r = Children[i];
                *(buint*) (address + 0x10 + i * 4) = offset;
                r.Rebuild(address + offset, _entrySize, true);
                offset += _entrySize;
            }
        }

        internal static ResourceNode TryParse(DataSource source, ResourceNode parent)
        {
            return ((Parameter*) source.Address)->_tag == Parameter.TagADPM ? new ADPMNode() : null;
        }
    }

    public unsafe class ADPMEntryNode : ResourceNode
    {
        internal StageParameterEntry* Header => (StageParameterEntry*) WorkingUncompressed.Address;
        public override ResourceType ResourceFileType => ResourceType.Unknown;

        private byte echo;
        private byte id2;
        private ushort id;

        private ParameterValueManager _values = new ParameterValueManager(null);

        [Category("ADPM Data")]
        public byte Unknown
        {
            get => echo;
            set
            {
                echo = value;
                SignalPropertyChange();
            }
        }

        [Category("ADPM Data")]
        public ushort Id1
        {
            get => id;
            set
            {
                id = value;
                SignalPropertyChange();
            }
        }

        [Category("ADPM Data")]
        public byte Id2
        {
            get => id2;
            set
            {
                id2 = value;
                SignalPropertyChange();
            }
        }

        [Category("ADPM Values")]
        public float Value1
        {
            get => _values.GetFloat(0);
            set
            {
                _values.SetFloat(0, value);
                SignalPropertyChange();
            }
        }

        [Category("ADPM Values")]
        public float Value2
        {
            get => _values.GetFloat(1);
            set
            {
                _values.SetFloat(1, value);
                SignalPropertyChange();
            }
        }

        [Category("ADPM Values")]
        public float Value3
        {
            get => _values.GetFloat(2);
            set
            {
                _values.SetFloat(2, value);
                SignalPropertyChange();
            }
        }

        [Category("ADPM Values")]
        public float Value4
        {
            get => _values.GetFloat(3);
            set
            {
                _values.SetFloat(3, value);
                SignalPropertyChange();
            }
        }

        [Category("ADPM Values")]
        public float Value5
        {
            get => _values.GetFloat(4);
            set
            {
                _values.SetFloat(4, value);
                SignalPropertyChange();
            }
        }

        [Category("ADPM Values")]
        public float Value6
        {
            get => _values.GetFloat(5);
            set
            {
                _values.SetFloat(5, value);
                SignalPropertyChange();
            }
        }

        [Category("ADPM Values")]
        public float Value7
        {
            get => _values.GetFloat(6);
            set
            {
                _values.SetFloat(6, value);
                SignalPropertyChange();
            }
        }

        [Category("ADPM Values")]
        public float Value8
        {
            get => _values.GetFloat(7);
            set
            {
                _values.SetFloat(7, value);
                SignalPropertyChange();
            }
        }

        [Category("ADPM Values")]
        public float Value9
        {
            get => _values.GetFloat(8);
            set
            {
                _values.SetFloat(8, value);
                SignalPropertyChange();
            }
        }

        [Category("ADPM Values")]
        public float Value10
        {
            get => _values.GetFloat(9);
            set
            {
                _values.SetFloat(9, value);
                SignalPropertyChange();
            }
        }

        [Category("ADPM Values")]
        public float Value11
        {
            get => _values.GetFloat(10);
            set
            {
                _values.SetFloat(10, value);
                SignalPropertyChange();
            }
        }

        [Category("ADPM Values")]
        public float Value12
        {
            get => _values.GetFloat(11);
            set
            {
                _values.SetFloat(11, value);
                SignalPropertyChange();
            }
        }

        [Category("ADPM Values")]
        public float Value13
        {
            get => _values.GetFloat(12);
            set
            {
                _values.SetFloat(12, value);
                SignalPropertyChange();
            }
        }

        [Category("ADPM Values")]
        public float Value14
        {
            get => _values.GetFloat(13);
            set
            {
                _values.SetFloat(13, value);
                SignalPropertyChange();
            }
        }

        [Category("ADPM Values")]
        public float Value15
        {
            get => _values.GetFloat(14);
            set
            {
                _values.SetFloat(14, value);
                SignalPropertyChange();
            }
        }

        [Category("ADPM Values")]
        public float Value16
        {
            get => _values.GetFloat(15);
            set
            {
                _values.SetFloat(15, value);
                SignalPropertyChange();
            }
        }

        [Category("ADPM Values")]
        public float Value17
        {
            get => _values.GetFloat(16);
            set
            {
                _values.SetFloat(16, value);
                SignalPropertyChange();
            }
        }

        [Category("ADPM Values")]
        public float Value18
        {
            get => _values.GetFloat(17);
            set
            {
                _values.SetFloat(17, value);
                SignalPropertyChange();
            }
        }

        [Category("ADPM Values")]
        public float Value19
        {
            get => _values.GetFloat(18);
            set
            {
                _values.SetFloat(18, value);
                SignalPropertyChange();
            }
        }

        [Category("ADPM Values")]
        public float Value20
        {
            get => _values.GetFloat(19);
            set
            {
                _values.SetFloat(19, value);
                SignalPropertyChange();
            }
        }

        [Category("ADPM Values")]
        public float Value21
        {
            get => _values.GetFloat(20);
            set
            {
                _values.SetFloat(20, value);
                SignalPropertyChange();
            }
        }

        [Category("ADPM Values")]
        public float Value22
        {
            get => _values.GetFloat(21);
            set
            {
                _values.SetFloat(21, value);
                SignalPropertyChange();
            }
        }

        [Category("ADPM Values")]
        public float Value23
        {
            get => _values.GetFloat(22);
            set
            {
                _values.SetFloat(22, value);
                SignalPropertyChange();
            }
        }

        [Category("ADPM Values")]
        public float Value24
        {
            get => _values.GetFloat(23);
            set
            {
                _values.SetFloat(23, value);
                SignalPropertyChange();
            }
        }

        [Category("ADPM Values")]
        public float Value25
        {
            get => _values.GetFloat(24);
            set
            {
                _values.SetFloat(24, value);
                SignalPropertyChange();
            }
        }

        [Category("ADPM Values")]
        public float Value26
        {
            get => _values.GetFloat(25);
            set
            {
                _values.SetFloat(25, value);
                SignalPropertyChange();
            }
        }

        [Category("ADPM Values")]
        public float Value27
        {
            get => _values.GetFloat(26);
            set
            {
                _values.SetFloat(26, value);
                SignalPropertyChange();
            }
        }

        [Category("ADPM Values")]
        public float Value28
        {
            get => _values.GetFloat(27);
            set
            {
                _values.SetFloat(27, value);
                SignalPropertyChange();
            }
        }

        [Category("ADPM Values")]
        public float Value29
        {
            get => _values.GetFloat(28);
            set
            {
                _values.SetFloat(28, value);
                SignalPropertyChange();
            }
        }

        [Category("ADPM Values")]
        public float Value30
        {
            get => _values.GetFloat(29);
            set
            {
                _values.SetFloat(29, value);
                SignalPropertyChange();
            }
        }

        [Category("ADPM Values")]
        public float Value31
        {
            get => _values.GetFloat(30);
            set
            {
                _values.SetFloat(30, value);
                SignalPropertyChange();
            }
        }

        [Category("ADPM Values")]
        public string Value32
        {
            get => _values.GetHex(31);
            set
            {
                _values.SetHex(31, value);
                SignalPropertyChange();
            }
        }

        [Category("ADPM Values")]
        public string Value33
        {
            get => _values.GetHex(32);
            set
            {
                _values.SetHex(32, value);
                SignalPropertyChange();
            }
        }

        [Category("ADPM Values")]
        public string Value34
        {
            get => _values.GetHex(33);
            set
            {
                _values.SetHex(33, value);
                SignalPropertyChange();
            }
        }

        [Category("ADPM Values")]
        public string Value35
        {
            get => _values.GetHex(34);
            set
            {
                _values.SetHex(34, value);
                SignalPropertyChange();
            }
        }

        [Category("ADPM Values")]
        public string Value36
        {
            get => _values.GetHex(35);
            set
            {
                _values.SetHex(35, value);
                SignalPropertyChange();
            }
        }

        [Category("ADPM Values")]
        public string Value37
        {
            get => _values.GetHex(36);
            set
            {
                _values.SetHex(36, value);
                SignalPropertyChange();
            }
        }

        [Category("ADPM Values")]
        public string Value38
        {
            get => _values.GetHex(37);
            set
            {
                _values.SetHex(37, value);
                SignalPropertyChange();
            }
        }

        [Category("ADPM Values")]
        public string Value39
        {
            get => _values.GetHex(38);
            set
            {
                _values.SetHex(38, value);
                SignalPropertyChange();
            }
        }

        [Category("ADPM Values")]
        public string Value40
        {
            get => _values.GetHex(39);
            set
            {
                _values.SetHex(39, value);
                SignalPropertyChange();
            }
        }

        [Category("ADPM Values")]
        public string Value41
        {
            get => _values.GetHex(40);
            set
            {
                _values.SetHex(40, value);
                SignalPropertyChange();
            }
        }

        [Category("ADPM Values")]
        public string Value42
        {
            get => _values.GetHex(41);
            set
            {
                _values.SetHex(41, value);
                SignalPropertyChange();
            }
        }

        [Category("ADPM Values")]
        public string Value43
        {
            get => _values.GetHex(42);
            set
            {
                _values.SetHex(42, value);
                SignalPropertyChange();
            }
        }

        [Category("ADPM Values")]
        public float Value44
        {
            get => _values.GetFloat(43);
            set
            {
                _values.SetFloat(43, value);
                SignalPropertyChange();
            }
        }

        [Category("ADPM Values")]
        public float Value45
        {
            get => _values.GetFloat(44);
            set
            {
                _values.SetFloat(44, value);
                SignalPropertyChange();
            }
        }

        [Category("ADPM Values")]
        public float Value46
        {
            get => _values.GetFloat(45);
            set
            {
                _values.SetFloat(45, value);
                SignalPropertyChange();
            }
        }

        [Category("ADPM Values")]
        public string Value47
        {
            get => _values.GetHex(46);
            set
            {
                _values.SetHex(46, value);
                SignalPropertyChange();
            }
        }

        [Category("ADPM Values")]
        public float Value48
        {
            get => _values.GetFloat(47);
            set
            {
                _values.SetFloat(47, value);
                SignalPropertyChange();
            }
        }

        [Category("ADPM Values")]
        public int Value49
        {
            get => _values.GetInt(48);
            set
            {
                _values.SetInt(48, value);
                SignalPropertyChange();
            }
        }

        [Category("ADPM Values")]
        public string Value50
        {
            get => _values.GetHex(49);
            set
            {
                _values.SetHex(49, value);
                SignalPropertyChange();
            }
        }

        [Category("ADPM Values")]
        public int Value51
        {
            get => _values.GetInt(50);
            set
            {
                _values.SetInt(50, value);
                SignalPropertyChange();
            }
        }

        [Category("ADPM Values")]
        public float Value52
        {
            get => _values.GetFloat(51);
            set
            {
                _values.SetFloat(51, value);
                SignalPropertyChange();
            }
        }

        [Category("ADPM Values")]
        public int Value53
        {
            get => _values.GetInt(52);
            set
            {
                _values.SetInt(52, value);
                SignalPropertyChange();
            }
        }

        [Category("ADPM Values")]
        public string Value54
        {
            get => _values.GetHex(53);
            set
            {
                _values.SetHex(53, value);
                SignalPropertyChange();
            }
        }

        [Category("ADPM Values")]
        public int Value55
        {
            get => _values.GetInt(54);
            set
            {
                _values.SetInt(54, value);
                SignalPropertyChange();
            }
        }

        [Category("ADPM Values")]
        public string Value56
        {
            get => _values.GetHex(55);
            set
            {
                _values.SetHex(55, value);
                SignalPropertyChange();
            }
        }

        [Category("ADPM Values")]
        public string Value57
        {
            get => _values.GetHex(56);
            set
            {
                _values.SetHex(56, value);
                SignalPropertyChange();
            }
        }

        [Category("ADPM Values")]
        public string Value58
        {
            get => _values.GetHex(57);
            set
            {
                _values.SetHex(57, value);
                SignalPropertyChange();
            }
        }

        [Category("ADPM Values")]
        public string Value59
        {
            get => _values.GetHex(58);
            set
            {
                _values.SetHex(58, value);
                SignalPropertyChange();
            }
        }

        [Category("ADPM Values")]
        public short Value60a
        {
            get => _values.GetShort(59, 0);
            set
            {
                _values.SetShort(59, 0, value);
                SignalPropertyChange();
            }
        }

        [Category("ADPM Values")]
        public short TimeLimitSeconds
        {
            get => _values.GetShort(59, 1);
            set
            {
                _values.SetShort(59, 1, value);
                SignalPropertyChange();
            }
        }

        [Category("ADPM Values")]
        public float Value61
        {
            get => _values.GetFloat(60);
            set
            {
                _values.SetFloat(60, value);
                SignalPropertyChange();
            }
        }

        [Category("ADPM Values")]
        public float Value62
        {
            get => _values.GetFloat(61);
            set
            {
                _values.SetFloat(61, value);
                SignalPropertyChange();
            }
        }

        [Category("ADPM Values")]
        public float Value63
        {
            get => _values.GetFloat(62);
            set
            {
                _values.SetFloat(62, value);
                SignalPropertyChange();
            }
        }

        [Category("ADPM Values")]
        public float Value64
        {
            get => _values.GetFloat(63);
            set
            {
                _values.SetFloat(63, value);
                SignalPropertyChange();
            }
        }

        public override bool OnInitialize()
        {
            id = Header->_id;
            echo = Header->_echo;
            id2 = Header->_id2;

            if (_name == null)
            {
                _name = "ADPMEntry " + id;
            }

            _values = new ParameterValueManager((VoidPtr) Header + 4);

            return false;
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            StageParameterEntry* header = (StageParameterEntry*) address;
            *header = new StageParameterEntry(id, echo, id2);
            byte* pOut = (byte*) header + 4;
            byte* pIn = (byte*) _values._values.Address;
            for (int i = 0; i < 64 * 4; i++)
            {
                *pOut++ = *pIn++;
            }
        }
    }
}