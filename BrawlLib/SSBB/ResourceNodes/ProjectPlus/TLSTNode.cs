using BrawlLib.Internal;
using BrawlLib.Internal.Audio;
using BrawlLib.SSBB.Types.ProjectPlus;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;

namespace BrawlLib.SSBB.ResourceNodes.ProjectPlus
{
    public unsafe class TLSTNode : ResourceNode
    {
        internal TLST* Header => (TLST*) WorkingUncompressed.Address;
        public override ResourceType ResourceFileType => ResourceType.TLST;

        public override Type[] AllowedChildTypes => new[] {typeof(TLSTEntryNode)};

        public string STRMPath => string.IsNullOrEmpty(DirectoryName) ? string.Empty : Path.Combine(new DirectoryInfo(DirectoryName).Parent?.FullName ?? "", "strm");

        public override void OnPopulate()
        {
            for (int i = 0; i < Header->_count; i++)
            {
                DataSource source = new DataSource((*Header)[i], (int) TLSTEntry.Size);
                new TLSTEntryNode().Initialize(this, source);
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
            int size = (int) TLST.HeaderSize;
            foreach (TLSTEntryNode n in Children)
            {
                size += (int) TLSTEntry.Size;
                if (!string.IsNullOrEmpty(n._name) && n._name != "<null>")
                {
                    size += n._name.UTF8Length() + 1;
                }

                if (!string.IsNullOrEmpty(n.SongFileName))
                {
                    size += n.SongFileName.UTF8Length() + 1;
                }
            }

            return size;
        }

        internal ushort strOffset;

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            strOffset = 0;
            TLST* header = (TLST*) address;
            *header = new TLST();
            header->_tag = TLST.Tag;
            header->_count = (uint) Children.Count;
            header->_fileSize = (ushort) length;
            header->_nameOffset = (ushort) (TLST.HeaderSize + Children.Count * TLSTEntry.Size);

            uint offset = TLST.HeaderSize;
            foreach (ResourceNode n in Children)
            {
                int size = n.CalculateSize(true);
                n.Rebuild(address + offset, size, true);
                offset += (uint) size;
            }

            foreach (TLSTEntryNode n in Children)
            {
                if (!string.IsNullOrEmpty(n.SongFileName))
                {
                    offset += address.WriteUTF8String(n.SongFileName, true, offset);
                }

                if (!string.IsNullOrEmpty(n._name) && n._name != "<null>")
                {
                    offset += address.WriteUTF8String(n._name, true, offset);
                }
            }
        }

        internal static ResourceNode TryParse(DataSource source, ResourceNode parent)
        {
            return source.Tag == "TLST" ? new TLSTNode() : null;
        }
    }

    public unsafe class TLSTEntryNode : ResourceNode, IAudioSource
    {
        internal TLSTEntry* Header => (TLSTEntry*) WorkingUncompressed.Address;
        [Browsable(false)] public override bool AllowNullNames => true;
        [Browsable(false)] public override bool AllowDuplicateNames => true;

        private RSTMNode linkedNode;

        public string rstmPath => (!(Parent is TLSTNode t) || string.IsNullOrEmpty(t.STRMPath)) ? string.Empty : Path.Combine(((TLSTNode) Parent).STRMPath, (string.IsNullOrEmpty(_fileName) && BrawlBRSTMs.ContainsKey(_songID) ? BrawlBRSTMs[_songID] : _fileName) + ".brstm");

        public IAudioStream[] CreateStreams()
        {
            return linkedNode?.CreateStreams();
        }

        public bool IsLooped => linkedNode?.IsLooped ?? false;

        public uint _songID;

        [Category("Song Entry")]
        [TypeConverter(typeof(HexUIntConverter))]
        public uint SongID
        {
            get => _songID;
            set
            {
                _songID = value;

                updateBRSTM();

                SignalPropertyChange();
            }
        }

        private short _songDelay;

        [Category("Song Entry")]
        public short SongDelay
        {
            get => _songDelay;
            set
            {
                _songDelay = value;
                SignalPropertyChange();
            }
        }

        private byte _songVolume;

        [Category("Song Entry")]
        public byte Volume
        {
            get => _songVolume;
            set
            {
                _songVolume = value;
                SignalPropertyChange();
            }
        }

        private byte _songFrequency;

        [Category("Song Entry")]
        public byte Frequency
        {
            get => _songFrequency;
            set
            {
                _songFrequency = value;
                SignalPropertyChange();
            }
        }

        private string _fileName;

        [Category("Song Entry")]
        public string SongFileName
        {
            get => _fileName;
            set
            {
                _fileName = value;

                updateBRSTM();

                SignalPropertyChange();
            }
        }

        private void updateBRSTM()
        {
            RSTMNode temp = linkedNode;
            linkedNode = null;
            UpdateCurrentControl();
            temp?.Dispose();
            if (File.Exists(rstmPath))
            {
                try
                {
                    linkedNode = (RSTMNode)NodeFactory.FromFile(null, rstmPath, typeof(RSTMNode));
                }
                catch
                {

                }
            }
            UpdateCurrentControl();
        }

        private ushort _songSwitch;

        [Category("Song Entry")]
        public ushort SongSwitch
        {
            get => _songSwitch;
            set
            {
                _songSwitch = value;
                SignalPropertyChange();
            }
        }

        private bool _disableStockPinch;

        [Category("Song Entry")]
        public bool DisableStockPinch
        {
            get => _disableStockPinch;
            set
            {
                _disableStockPinch = value;
                SignalPropertyChange();
            }
        }


        private bool _disableTracklistInclusion;

        [Category("Song Entry")]
        public bool HiddenFromTracklist
        {
            get => _disableTracklistInclusion;
            set
            {
                _disableTracklistInclusion = value;
                SignalPropertyChange();
            }
        }

        public override int OnCalculateSize(bool force)
        {
            return (int) TLSTEntry.Size;
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            TLSTEntry* header = (TLSTEntry*) address;
            *header = new TLSTEntry();
            if (string.IsNullOrEmpty(_fileName))
            {
                header->_fileName = 0xFFFF;
            }
            else
            {
                header->_fileName = ((TLSTNode) Parent).strOffset;
                ((TLSTNode) Parent).strOffset += (ushort) (_fileName.UTF8Length() + 1);
            }
            if (string.IsNullOrEmpty(_name) || _name == "<null>")
            {
                header->_title = 0xFFFF;
            }
            else
            {
                header->_title = ((TLSTNode) Parent).strOffset;
                ((TLSTNode) Parent).strOffset += (ushort) (_name.UTF8Length() + 1);
            }

            header->_songID = _songID;
            header->_songDelay = _songDelay;
            header->_songVolume = _songVolume;
            header->_songFrequency = _songFrequency;
            header->_songSwitch = _songSwitch;
            header->_disableStockPinch = (byte) (_disableStockPinch ? 1 : 0);
            header->_disableTracklistInclusion = (byte) (_disableTracklistInclusion ? 1 : 0);
        }

        public override bool OnInitialize()
        {
            if (Header->_title != 0xFFFF)
            {
                _name = Parent.WorkingUncompressed.Address.GetUTF8String((uint)(((TLSTNode)Parent).Header->_nameOffset + Header->_title));
            }

            if (Header->_fileName != 0xFFFF)
            {
                _fileName = Parent.WorkingUncompressed.Address.GetUTF8String((uint)((TLSTNode)Parent).Header->_nameOffset + Header->_fileName);
            }

            _songID = Header->_songID;

            if (File.Exists(rstmPath))
            {
                try
                {
                    linkedNode = (RSTMNode) NodeFactory.FromFile(null, rstmPath, typeof(RSTMNode));
                }
                catch
                {
                    // ignore
                }
            }

            _songDelay = Header->_songDelay;
            _songVolume = Header->_songVolume;
            _songFrequency = Header->_songFrequency;
            _songSwitch = Header->_songSwitch;
            _disableStockPinch = Header->_disableStockPinch != 0;
            _disableTracklistInclusion = Header->_disableTracklistInclusion != 0;
            return false;
        }

        public static readonly Dictionary<uint, string> BrawlBRSTMs = new Dictionary<uint, string>
        {
            {0x000026F9, "X01"},
            {0x000026FA, "X02"},
            {0x000026FB, "X03"},
            {0x000026FC, "X04"},
            {0x000026FD, "X05"},
            {0x000026FE, "X06"},
            {0x000026FF, "X07"},
            {0x00002700, "X08"},
            {0x00002701, "X09"},
            {0x00002702, "X10"},
            {0x00002703, "X11"},
            {0x00002704, "X12"},
            {0x00002705, "X13"},
            {0x00002706, "X14"},
            {0x00002707, "X15"},
            {0x00002708, "X16"},
            {0x00002709, "X17"},
            {0x0000270A, "X18"},
            {0x0000270B, "X19"},
            {0x0000270C, "X20"},
            {0x0000270D, "X21"},
            {0x0000270E, "X22"},
            {0x0000270F, "X23"},
            {0x00002710, "X24"},
            {0x00002711, "X25"},
            {0x00002712, "X26"},
            {0x00002713, "X27"},
            {0x00002714, "A01"},
            {0x00002715, "A02"},
            {0x00002716, "A03"},
            {0x00002717, "A04"},
            {0x00002718, "A05"},
            {0x00002719, "A06"},
            {0x0000271A, "A07"},
            {0x0000271B, "A08"},
            {0x0000271C, "A09"},
            {0x0000271D, "A10"},
            {0x0000271E, "A11"},
            {0x0000271F, "A12"},
            {0x00002720, "A13"},
            {0x00002721, "A14"},
            {0x00002722, "A15"},
            {0x00002723, "A16"},
            {0x00002724, "A17"},
            {0x00002725, "A20"},
            {0x00002726, "A21"},
            {0x00002727, "A22"},
            {0x00002728, "A23"},
            {0x00002729, "B01"},
            {0x0000272A, "B02"},
            {0x0000272B, "B03"},
            {0x0000272C, "B04"},
            {0x0000272D, "B05"},
            {0x0000272E, "B06"},
            {0x0000272F, "B07"},
            {0x00002730, "B08"},
            {0x00002731, "B09"},
            {0x00002732, "B10"},
            {0x00002733, "C01"},
            {0x00002734, "C02"},
            {0x00002735, "C03"},
            {0x00002736, "C04"},
            {0x00002737, "C05"},
            {0x00002738, "C06"},
            {0x00002739, "C07"},
            {0x0000273A, "C08"},
            {0x0000273B, "C09"},
            {0x0000273C, "C10"},
            {0x0000273D, "C11"},
            {0x0000273E, "C12"},
            {0x0000273F, "C13"},
            {0x00002740, "C14"},
            {0x00002741, "C15"},
            {0x00002742, "C16"},
            {0x00002743, "C17"},
            {0x00002744, "C18"},
            {0x00002745, "C19"},
            {0x00002746, "D01"},
            {0x00002747, "D02"},
            {0x00002748, "D03"},
            {0x00002749, "D04"},
            {0x0000274A, "D05"},
            {0x0000274B, "D06"},
            {0x0000274C, "D07"},
            {0x0000274D, "D08"},
            {0x0000274E, "D09"},
            {0x0000274F, "D10"},
            {0x00002750, "E01"},
            {0x00002751, "E02"},
            {0x00002752, "E03"},
            {0x00002753, "E04"},
            {0x00002754, "E05"},
            {0x00002755, "E06"},
            {0x00002756, "E07"},
            {0x00002757, "F01"},
            {0x00002758, "F02"},
            {0x00002759, "F03"},
            {0x0000275A, "F04"},
            {0x0000275B, "F05"},
            {0x0000275C, "F06"},
            {0x0000275D, "F07"},
            {0x0000275E, "F08"},
            {0x0000275F, "F09"},
            {0x00002760, "F10"},
            {0x00002761, "F11"},
            {0x00002762, "F12"},
            {0x00002763, "G01"},
            {0x00002764, "G02"},
            {0x00002765, "G03"},
            {0x00002766, "G04"},
            {0x00002767, "G05"},
            {0x00002768, "G06"},
            {0x00002769, "G07"},
            {0x0000276A, "G08"},
            {0x0000276B, "G09"},
            {0x0000276C, "G10"},
            {0x0000276D, "G11"},
            {0x0000276E, "H01"},
            {0x0000276F, "H02"},
            {0x00002770, "H03"},
            {0x00002771, "H04"},
            {0x00002772, "H05"},
            {0x00002773, "H06"},
            {0x00002774, "H07"},
            {0x00002775, "H08"},
            {0x00002776, "H09"},
            {0x00002777, "H10"},
            {0x00002778, "I01"},
            {0x00002779, "I02"},
            {0x0000277A, "I03"},
            {0x0000277B, "I04"},
            {0x0000277C, "I05"},
            {0x0000277D, "I06"},
            {0x0000277E, "I07"},
            {0x0000277F, "I08"},
            {0x00002780, "I09"},
            {0x00002781, "I10"},
            {0x00002782, "J01"},
            {0x00002783, "J02"},
            {0x00002784, "J03"},
            {0x00002785, "J04"},
            {0x00002786, "J05"},
            {0x00002787, "J06"},
            {0x00002788, "J07"},
            {0x00002789, "J08"},
            {0x0000278A, "J09"},
            {0x0000278B, "J10"},
            {0x0000278C, "J11"},
            {0x0000278D, "J12"},
            {0x0000278E, "J13"},
            {0x0000278F, "K01"},
            {0x00002790, "K02"},
            {0x00002791, "K03"},
            {0x00002792, "K04"},
            {0x00002793, "K05"},
            {0x00002794, "K06"},
            {0x00002795, "K07"},
            {0x00002796, "K08"},
            {0x00002797, "K09"},
            {0x00002798, "K10"},
            {0x00002799, "L01"},
            {0x0000279A, "L02"},
            {0x0000279B, "L03"},
            {0x0000279C, "L04"},
            {0x0000279D, "L05"},
            {0x0000279E, "L06"},
            {0x0000279F, "L07"},
            {0x000027A0, "L08"},
            {0x000027A1, "M01"},
            {0x000027A2, "M02"},
            {0x000027A3, "M03"},
            {0x000027A4, "M04"},
            {0x000027A5, "M05"},
            {0x000027A6, "M06"},
            {0x000027A7, "M07"},
            {0x000027A8, "M08"},
            {0x000027A9, "M09"},
            {0x000027AA, "M10"},
            {0x000027AB, "M11"},
            {0x000027AC, "M12"},
            {0x000027AD, "M13"},
            {0x000027AE, "M14"},
            {0x000027AF, "M15"},
            {0x000027B0, "M16"},
            {0x000027B1, "M17"},
            {0x000027B2, "M18"},
            {0x000027B3, "N01"},
            {0x000027B4, "N02"},
            {0x000027B5, "N03"},
            {0x000027B6, "N04"},
            {0x000027B7, "N05"},
            {0x000027B8, "N06"},
            {0x000027B9, "N07"},
            {0x000027BA, "N08"},
            {0x000027BB, "N09"},
            {0x000027BC, "N10"},
            {0x000027BD, "N11"},
            {0x000027BE, "N12"},
            {0x000027BF, "P01"},
            {0x000027C0, "P02"},
            {0x000027C1, "P03"},
            {0x000027C2, "P04"},
            {0x000027C3, "Q01"},
            {0x000027C4, "Q02"},
            {0x000027C5, "Q03"},
            {0x000027C6, "Q04"},
            {0x000027C7, "Q05"},
            {0x000027C8, "Q06"},
            {0x000027C9, "Q07"},
            {0x000027CA, "Q08"},
            {0x000027CB, "Q09"},
            {0x000027CC, "Q10"},
            {0x000027CD, "Q11"},
            {0x000027CE, "Q12"},
            {0x000027CF, "Q13"},
            {0x000027D0, "Q14"},
            {0x000027D1, "R01"},
            {0x000027D2, "R02"},
            {0x000027D3, "R03"},
            {0x000027D4, "R04"},
            {0x000027D5, "R05"},
            {0x000027D6, "R06"},
            {0x000027D7, "R07"},
            {0x000027D8, "R08"},
            {0x000027D9, "R09"},
            {0x000027DA, "R10"},
            {0x000027DB, "R11"},
            {0x000027DC, "R12"},
            {0x000027DD, "R13"},
            {0x000027DE, "R14"},
            {0x000027DF, "R15"},
            {0x000027E0, "R16"},
            {0x000027E1, "R17"},
            {0x000027E2, "S01"},
            {0x000027E3, "S02"},
            {0x000027E4, "S03"},
            {0x000027E5, "S04"},
            {0x000027E6, "S05"},
            {0x000027E7, "S06"},
            {0x000027E8, "S07"},
            {0x000027E9, "S08"},
            {0x000027EA, "S09"},
            {0x000027EB, "S10"},
            {0x000027EC, "S11"},
            {0x000027ED, "T01"},
            {0x000027EE, "T02"},
            {0x000027EF, "T03"},
            {0x000027F0, "T04"},
            {0x000027F1, "T05"},
            {0x000027F2, "U01"},
            {0x000027F3, "U02"},
            {0x000027F4, "U03"},
            {0x000027F5, "U04"},
            {0x000027F6, "U05"},
            {0x000027F7, "U06"},
            {0x000027F8, "U07"},
            {0x000027F9, "U08"},
            {0x000027FA, "U09"},
            {0x000027FB, "U10"},
            {0x000027FC, "U11"},
            {0x000027FD, "U12"},
            {0x000027FE, "U13"},
            {0x000027FF, "W01"},
            {0x00002800, "W02"},
            {0x00002801, "W03"},
            {0x00002802, "W04"},
            {0x00002803, "W05"},
            {0x00002804, "W06"},
            {0x00002805, "W07"},
            {0x00002806, "W08"},
            {0x00002807, "W09"},
            {0x00002808, "W10"},
            {0x00002809, "W11"},
            {0x0000280A, "W12"},
            {0x0000280B, "W13"},
            {0x0000280C, "W14"},
            {0x0000280D, "W15"},
            {0x0000280E, "W16"},
            {0x0000280F, "W17"},
            {0x00002810, "W18"},
            {0x00002811, "W19"},
            {0x00002812, "W20"},
            {0x00002813, "W21"},
            {0x00002814, "W22"},
            {0x00002815, "W23"},
            {0x00002816, "W24"},
            {0x00002817, "W25"},
            {0x00002818, "W26"},
            {0x00002819, "W27"},
            {0x0000281A, "W28"},
            {0x0000281B, "W29"},
            {0x0000281C, "W30"},
            {0x0000281D, "W31"},
            {0x0000281E, "W32"},
            {0x0000281F, "Y01"},
            {0x00002820, "Y02"},
            {0x00002821, "Y03"},
            {0x00002822, "Y04"},
            {0x00002823, "Y05"},
            {0x00002824, "Y06"},
            {0x00002825, "Y07"},
            {0x00002826, "Y08"},
            {0x00002827, "Y09"},
            {0x00002828, "Y10"},
            {0x00002829, "Y11"},
            {0x0000282A, "Y12"},
            {0x0000282B, "Y13"},
            {0x0000282C, "Y14"},
            {0x0000282D, "Y15"},
            {0x0000282E, "Y16"},
            {0x0000282F, "Y17"},
            {0x00002830, "Y18"},
            {0x00002831, "Y19"},
            {0x00002832, "Y20"},
            {0x00002833, "Y21"},
            {0x00002834, "Y22"},
            {0x00002835, "Y23"},
            {0x00002836, "Y24"},
            {0x00002837, "Y25"},
            {0x00002838, "Y26"},
            {0x00002839, "Y27"},
            {0x0000283A, "Y28"},
            {0x0000283B, "Y29"},
            {0x0000283C, "Y30"},
            {0x0000283D, "Z01"},
            {0x0000283E, "Z02"},
            {0x0000283F, "Z03"},
            {0x00002840, "Z04"},
            {0x00002841, "Z05"},
            {0x00002842, "Z06"},
            {0x00002843, "Z07"},
            {0x00002844, "Z08"},
            {0x00002845, "Z09"},
            {0x00002846, "Z10"},
            {0x00002847, "Z11"},
            {0x00002848, "Z12"},
            {0x00002849, "Z13"},
            {0x0000284A, "Z14"},
            {0x0000284B, "Z15"},
            {0x0000284C, "Z16"},
            {0x0000284D, "Z17"},
            {0x0000284E, "Z18"},
            {0x0000284F, "Z19"},
            {0x00002850, "Z20"},
            {0x00002851, "Z21"},
            {0x00002852, "Z22"},
            {0x00002853, "Z23"},
            {0x00002854, "Z24"},
            {0x00002855, "Z25"},
            {0x00002856, "Z26"},
            {0x00002857, "Z27"},
            {0x00002858, "Z28"},
            {0x00002859, "Z32"},
            {0x0000285A, "Z33"},
            {0x0000285B, "Z34"},
            {0x0000285C, "Z35"},
            {0x0000285D, "Z37"},
            {0x0000285E, "Z38"},
            {0x0000285F, "Z39"},
            {0x00002860, "Z41"},
            {0x00002861, "Z46"},
            {0x00002862, "Z47"},
            {0x00002863, "Z50"},
            {0x00002864, "Z51"},
            {0x00002865, "Z52"},
            {0x00002866, "Z53"},
            {0x00002867, "Z54"},
            {0x00002868, "Z55"},
            {0x00002869, "Z56"},
            {0x0000286A, "Z57"},
            {0x0000286B, "Z58"}
        };
    }
}