using BrawlLib.Internal;
using BrawlLib.SSBB.Types.Subspace.Hazards;
using System;
using System.ComponentModel;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class GEG1Node : ResourceNode
    {
        internal GEG1* Header => (GEG1*) WorkingUncompressed.Address;
        public override ResourceType ResourceFileType => ResourceType.GEG1;

        [Category("GEG1")]
        [DisplayName("Enemy Count")]
        public int EnemyCount => Children?.Count ?? 0;

        private const int _entrySize = 0x84; // The constant size of a child entry

        public override void OnPopulate()
        {
            for (int i = 0; i < Header->_count; i++)
            {
                DataSource source;
                if (i == Header->_count - 1)
                {
                    source = new DataSource((*Header)[i],
                        WorkingUncompressed.Address + WorkingUncompressed.Length - (*Header)[i]);
                }
                else
                {
                    source = new DataSource((*Header)[i], (*Header)[i + 1] - (*Header)[i]);
                }

                new GEG1EntryNode().Initialize(this, source);
            }
        }

        public override int OnCalculateSize(bool force)
        {
            return 0x08 + Children.Count * 4 + Children.Count * _entrySize;
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            GEG1* header = (GEG1*) address;
            *header = new GEG1(Children.Count);
            uint offset = (uint) (0x08 + Children.Count * 4);
            for (int i = 0; i < Children.Count; i++)
            {
                ResourceNode r = Children[i];
                *(buint*) (address + 0x08 + i * 4) = offset;
                r.Rebuild(address + offset, _entrySize, true);
                offset += _entrySize;
            }
        }

        public override bool OnInitialize()
        {
            base.OnInitialize();
            if (_name == null)
            {
                _name = "GEG1";
            }

            return Header->_count > 0;
        }

        internal static ResourceNode TryParse(DataSource source)
        {
            return ((GEG1*) source.Address)->_tag == GEG1.Tag ? new GEG1Node() : null;
        }
    }

    public enum EnemyList : byte
    {
        Goomba = 0,
        Poppant = 1,
        Feyesh = 2,
        Jyk = 3,
        Auroros = 4,
        Cymul = 5,
        Roturret = 6,
        Borboras = 7,
        GiantGoomba = 8,
        Buckot = 9,
        Bucculus = 10,
        Greap = 11,
        Armight = 12,
        BulletBill = 13,
        Roader = 14,
        Spaak = 15,
        Mite = 16,
        Ticken = 17,
        Towtow = 18,
        HammerBro = 19,
        Bytan = 20,
        Floow = 21,
        Puppit = 22,
        Primid = 23,
        Shellpod = 24,
        Koopa = 25,
        Shaydas = 26,
        Bombed = 27,
        MetalPrimid = 28,
        Nagagog = 29,
        Trowlon = 30,
        BigPrimid = 31,
        BoomerangPrimid = 32,
        FirePrimid = 33,
        ScopePrimid = 34,
        SwordPrimid = 35,
        Gamyga = 36,
        ROBBlaster = 37,
        ROBDistance = 38,
        ROBLauncher = 39,
        ROBSentry = 40,
        Autolance = 41,
        Armank = 42,
        Glire = 43,
        Glice = 44,
        Glunder = 45,
        PeteyPiranha = 46,
        GamygaBase01_Top = 47,
        GamygaBase02 = 48,
        GamygaBase03 = 49,
        GamygaBase04_Bottom = 50,
        Galleom = 51,
        Ridley = 52,
        Rayquaza = 53,
        Duon = 54,
        Porky = 55,
        MetaRidley = 56,
        FalconFlyer = 57,
        Tabuu = 58,
        MasterHand = 59,
        CrazyHand = 60
    }

    public unsafe class GEG1EntryNode : ResourceNode
    {
        internal GEG1Entry* Header => (GEG1Entry*) WorkingUncompressed.Address;
        public override ResourceType ResourceFileType => ResourceType.ENEMY;

        public void RegenName()
        {
            Name = EnemyNameList();
        }

        // I believe these are constant values for the Header
        public const uint Header1 = 0x0000803F; // 0x00

        public const uint Header2 = 0x00FF0100; // 0x04

        // Headers are known
        public uint _header1;

        public uint _header2;

        // Some form of byte flags
        public byte _extrahealth;
        public byte _flag0x09;
        public byte _flag0x0A;

        public byte _flag0x0B;

        // Another byte flag
        public byte _flag0x0C;

        // Another byte flag (Possibly unstable?)
        public byte _connectedenemyid;
        public byte _flag0x0E;
        public byte _flag0x0F;
        public byte _unknown0x10;
        public byte _unknown0x11;
        public byte _unknown0x12;
        public byte _unknown0x13;
        public byte _unknown0x14;
        public byte _unknown0x15;
        public byte _unknown0x16;
        public byte _unknown0x17;
        public byte _unknown0x18;
        public byte _unknown0x19;
        public byte _unknown0x1A;
        public byte _unknown0x1B;

        public byte _unknown0x1C;

        // EnemyID is known
        public byte _enemyID;
        public byte _unknown0x1E;
        public byte _unknown0x1F;
        public byte _unknown0x20;
        public byte _unknown0x21;

        public byte _unknown0x22;

        // Some form of byte flag
        public byte _startingaction;
        public byte _unknown0x24;
        public byte _unknown0x25;
        public byte _unknown0x26;

        public byte _unknown0x27;

        // Spawn Position is known
        public Vector2 _spawnPos;
        public byte _unknown0x30;
        public byte _unknown0x31;
        public byte _unknown0x32;
        public byte _unknown0x33;
        public byte _unknown0x34;
        public byte _unknown0x35;
        public byte _unknown0x36;
        public byte _unknown0x37;
        public byte _unknown0x38;
        public byte _unknown0x39;
        public byte _unknown0x3A;
        public byte _unknown0x3B;
        public byte _unknown0x3C;
        public byte _unknown0x3D;
        public byte _unknown0x3E;
        public byte _unknown0x3F;
        public byte _unknown0x40;
        public byte _unknown0x41;
        public byte _unknown0x42;
        public byte _unknown0x43;
        public byte _unknown0x44;
        public byte _unknown0x45;
        public byte _unknown0x46;
        public byte _unknown0x47;
        public byte _unknown0x48;
        public byte _unknown0x49;
        public byte _unknown0x4A;
        public byte _unknown0x4B;
        public byte _unknown0x4C;
        public byte _unknown0x4D;
        public byte _unknown0x4E;
        public byte _unknown0x4F;
        public byte _unknown0x50;
        public byte _unknown0x51;
        public byte _unknown0x52;
        public byte _unknown0x53;
        public byte _unknown0x54;
        public byte _unknown0x55;
        public byte _unknown0x56;
        public byte _unknown0x57;
        public byte _unknown0x58;
        public byte _unknown0x59;
        public byte _unknown0x5A;
        public byte _unknown0x5B;
        public byte _unknown0x5C;
        public byte _unknown0x5D;
        public byte _unknown0x5E;
        public byte _unknown0x5F;
        public byte _unknown0x60;
        public byte _unknown0x61;
        public byte _unknown0x62;

        public byte _unknown0x63;

        // Another flag
        public byte _flag0x64;
        public byte _unknown0x65;

        public byte _unknown0x66;

        // Another flag?
        public byte _flag0x67;
        public byte _unknown0x68;
        public byte _unknown0x69;
        public byte _unknown0x6A;
        public byte _unknown0x6B;
        public byte _unknown0x6C;
        public byte _unknown0x6D;
        public byte _unknown0x6E;
        public byte _unknown0x6F;
        public byte _unknown0x70;
        public byte _unknown0x71;
        public byte _unknown0x72;
        public byte _unknown0x73;
        public byte _unknown0x74;
        public byte _unknown0x75;
        public byte _unknown0x76;
        public byte _unknown0x77;
        public byte _unknown0x78;
        public byte _unknown0x79;
        public byte _unknown0x7A;
        public byte _unknown0x7B;

        public byte _unknown0x7C;

        // Spawn ID
        public byte _spawnid;
        public byte _flag0x7E;
        public byte _flag0x7F;
        public byte _flag0x80;
        public byte _flag0x81;
        public byte _flag0x82;
        public byte _flag0x83;

        [Browsable(true)]
        [Category("Enemy Info")]
        [DisplayName("Enemy Type")]
        public EnemyList EnemyName
        {
            get => (EnemyList) _enemyID;
            set
            {
                _enemyID = (byte) value;
                RegenName();
                SignalPropertyChange();
            }
        }

        [Category("Enemy Info")]
        [DisplayName("Enemy ID")]
        public byte EnemyID => _enemyID;

        [Category("Enemy Info")]
        [DisplayName("Enemy ARC ID")]
        public int EnemyArcID => _enemyID * 2;

        [Category("Enemy Info")]
        [DisplayName("Enemy BRRES ARC ID")]
        public int EnemyBrresID => _enemyID * 2 + 1;

        [Browsable(true)]
        [Category("Spawn Info")]
        [DisplayName("Spawn Position")]
        [TypeConverter(typeof(Vector2StringConverter))]
        public Vector2 EnemySpawnPos
        {
            get => _spawnPos;
            set
            {
                _spawnPos = value;
                SignalPropertyChange();
            }
        }

        [Browsable(true)]
        [Category("Spawn Info")]
        [DisplayName("Spawn ID")]
        public byte SpawnID
        {
            get => _spawnid;
            set
            {
                _spawnid = value;
                SignalPropertyChange();
            }
        }

        [Browsable(true)]
        [Category("Enemy Info")]
        [DisplayName("Additional HP")]
        public byte ExtraHealth
        {
            get => _extrahealth;
            set
            {
                _extrahealth = value;
                SignalPropertyChange();
            }
        }

        [Browsable(true)]
        [Category("Special Flags")]
        [DisplayName("Flag 0x09")]
        public byte Flag0x09
        {
            get => _flag0x09;
            set
            {
                _flag0x09 = value;
                SignalPropertyChange();
            }
        }

        [Browsable(true)]
        [Category("Special Flags")]
        [DisplayName("Flag 0x0A")]
        public byte Flag0x0A
        {
            get => _flag0x0A;
            set
            {
                _flag0x0A = value;
                SignalPropertyChange();
            }
        }

        [Browsable(true)]
        [Category("Special Flags")]
        [DisplayName("Flag 0x0B")]
        public byte Flag0x0B
        {
            get => _flag0x0B;
            set
            {
                _flag0x0B = value;
                SignalPropertyChange();
            }
        }

        [Browsable(true)]
        [Category("Special Flags")]
        [DisplayName("Flag 0x0C")]
        public byte Flag0x0C
        {
            get => _flag0x0C;
            set
            {
                _flag0x0C = value;
                SignalPropertyChange();
            }
        }

        [Browsable(true)]
        [Category("Spawn Info")]
        [DisplayName("Connected Enemy ID")]
        public byte ConnectedEnemyID
        {
            get => _connectedenemyid;
            set
            {
                _connectedenemyid = value;
                SignalPropertyChange();
            }
        }

        [Browsable(true)]
        [Category("Special Flags")]
        [DisplayName("Flag 0x0E")]
        public byte Flag0x0E
        {
            get => _flag0x0E;
            set
            {
                _flag0x0E = value;
                SignalPropertyChange();
            }
        }

        [Browsable(true)]
        [Category("Special Flags")]
        [DisplayName("Flag 0x0F")]
        public byte Flag0x0F
        {
            get => _flag0x0F;
            set
            {
                _flag0x0F = value;
                SignalPropertyChange();
            }
        }

        [Browsable(true)]
        [Category("Spawn Info")]
        [DisplayName("Starting Action")]
        public byte StartingAction
        {
            get => _startingaction;
            set
            {
                _startingaction = value;
                SignalPropertyChange();
            }
        }

        [Browsable(true)]
        [Category("Special Flags")]
        [DisplayName("Flag 0x64")]
        public byte Flag0x64
        {
            get => _flag0x64;
            set
            {
                _flag0x64 = value;
                SignalPropertyChange();
            }
        }

        [Browsable(true)]
        [Category("Special Flags")]
        [DisplayName("Flag 0x67")]
        public byte Flag0x67
        {
            get => _flag0x67;
            set
            {
                _flag0x67 = value;
                SignalPropertyChange();
            }
        }

        [Browsable(true)]
        [Category("Special Flags")]
        [DisplayName("Flag 0x7E")]
        public byte Flag0x7E
        {
            get => _flag0x7E;
            set
            {
                _flag0x7E = value;
                SignalPropertyChange();
            }
        }


        [Browsable(true)]
        [Category("Special Flags")]
        [DisplayName("Flag 0x80")]
        public byte Flag0x80
        {
            get => _flag0x80;
            set
            {
                _flag0x80 = value;
                SignalPropertyChange();
            }
        }

        [Browsable(true)]
        [Category("Special Flags")]
        [DisplayName("Flag 0x81")]
        public byte Flag0x81
        {
            get => _flag0x81;
            set
            {
                _flag0x81 = value;
                SignalPropertyChange();
            }
        }

        [Browsable(true)]
        [Category("Special Flags")]
        [DisplayName("Flag 0x82")]
        public byte Flag0x82
        {
            get => _flag0x82;
            set
            {
                _flag0x82 = value;
                SignalPropertyChange();
            }
        }

        [Browsable(true)]
        [Category("Special Flags")]
        [DisplayName("Flag 0x83")]
        public byte Flag0x83
        {
            get => _flag0x83;
            set
            {
                _flag0x83 = value;
                SignalPropertyChange();
            }
        }

        public override bool OnInitialize()
        {
            _header1 = Header->_header1;
            _header2 = Header->_header2;
            _extrahealth = Header->_extrahealth;
            _flag0x09 = Header->_flag0x09;
            _flag0x0A = Header->_flag0x0A;
            _flag0x0B = Header->_flag0x0B;
            _flag0x0C = Header->_flag0x0C;
            _connectedenemyid = Header->_connectedenemyid;
            _flag0x0E = Header->_flag0x0E;
            _flag0x0F = Header->_flag0x0F;
            _unknown0x10 = Header->_unknown0x10;
            _unknown0x11 = Header->_unknown0x11;
            _unknown0x12 = Header->_unknown0x12;
            _unknown0x13 = Header->_unknown0x13;
            _unknown0x14 = Header->_unknown0x14;
            _unknown0x15 = Header->_unknown0x15;
            _unknown0x16 = Header->_unknown0x16;
            _unknown0x17 = Header->_unknown0x17;
            _unknown0x18 = Header->_unknown0x18;
            _unknown0x19 = Header->_unknown0x19;
            _unknown0x1A = Header->_unknown0x1A;
            _unknown0x1B = Header->_unknown0x1B;
            _unknown0x1C = Header->_unknown0x1C;
            _enemyID = Header->_enemyID;
            _unknown0x1E = Header->_unknown0x1E;
            _unknown0x1F = Header->_unknown0x1F;
            _unknown0x20 = Header->_unknown0x20;
            _unknown0x21 = Header->_unknown0x21;
            _unknown0x22 = Header->_unknown0x22;
            _startingaction = Header->_startingaction;
            _unknown0x24 = Header->_unknown0x24;
            _unknown0x25 = Header->_unknown0x25;
            _unknown0x26 = Header->_unknown0x26;
            _unknown0x27 = Header->_unknown0x27;
            _spawnPos._x = Header->_spawnX;
            _spawnPos._y = Header->_spawnY;
            _unknown0x30 = Header->_unknown0x30;
            _unknown0x31 = Header->_unknown0x31;
            _unknown0x32 = Header->_unknown0x32;
            _unknown0x33 = Header->_unknown0x33;
            _unknown0x34 = Header->_unknown0x34;
            _unknown0x35 = Header->_unknown0x35;
            _unknown0x36 = Header->_unknown0x36;
            _unknown0x37 = Header->_unknown0x37;
            _unknown0x38 = Header->_unknown0x38;
            _unknown0x39 = Header->_unknown0x39;
            _unknown0x3A = Header->_unknown0x3A;
            _unknown0x3B = Header->_unknown0x3B;
            _unknown0x3C = Header->_unknown0x3C;
            _unknown0x3D = Header->_unknown0x3D;
            _unknown0x3E = Header->_unknown0x3E;
            _unknown0x3F = Header->_unknown0x3F;
            _unknown0x40 = Header->_unknown0x40;
            _unknown0x41 = Header->_unknown0x41;
            _unknown0x42 = Header->_unknown0x42;
            _unknown0x43 = Header->_unknown0x43;
            _unknown0x44 = Header->_unknown0x44;
            _unknown0x45 = Header->_unknown0x45;
            _unknown0x46 = Header->_unknown0x46;
            _unknown0x47 = Header->_unknown0x47;
            _unknown0x48 = Header->_unknown0x48;
            _unknown0x49 = Header->_unknown0x49;
            _unknown0x4A = Header->_unknown0x4A;
            _unknown0x4B = Header->_unknown0x4B;
            _unknown0x4C = Header->_unknown0x4C;
            _unknown0x4D = Header->_unknown0x4D;
            _unknown0x4E = Header->_unknown0x4E;
            _unknown0x4F = Header->_unknown0x4F;
            _unknown0x50 = Header->_unknown0x50;
            _unknown0x51 = Header->_unknown0x51;
            _unknown0x52 = Header->_unknown0x52;
            _unknown0x53 = Header->_unknown0x53;
            _unknown0x54 = Header->_unknown0x54;
            _unknown0x55 = Header->_unknown0x55;
            _unknown0x56 = Header->_unknown0x56;
            _unknown0x57 = Header->_unknown0x57;
            _unknown0x58 = Header->_unknown0x58;
            _unknown0x59 = Header->_unknown0x59;
            _unknown0x5A = Header->_unknown0x5A;
            _unknown0x5B = Header->_unknown0x5B;
            _unknown0x5C = Header->_unknown0x5C;
            _unknown0x5D = Header->_unknown0x5D;
            _unknown0x5E = Header->_unknown0x5E;
            _unknown0x5F = Header->_unknown0x5F;
            _unknown0x60 = Header->_unknown0x60;
            _unknown0x61 = Header->_unknown0x61;
            _unknown0x62 = Header->_unknown0x62;
            _unknown0x63 = Header->_unknown0x63;
            _flag0x64 = Header->_flag0x64;
            _unknown0x65 = Header->_unknown0x65;
            _unknown0x66 = Header->_unknown0x66;
            _flag0x67 = Header->_flag0x67;
            _unknown0x68 = Header->_unknown0x68;
            _unknown0x69 = Header->_unknown0x69;
            _unknown0x6A = Header->_unknown0x6A;
            _unknown0x6B = Header->_unknown0x6B;
            _unknown0x6C = Header->_unknown0x6C;
            _unknown0x6D = Header->_unknown0x6D;
            _unknown0x6E = Header->_unknown0x6E;
            _unknown0x6F = Header->_unknown0x6F;
            _unknown0x70 = Header->_unknown0x70;
            _unknown0x71 = Header->_unknown0x71;
            _unknown0x72 = Header->_unknown0x72;
            _unknown0x73 = Header->_unknown0x73;
            _unknown0x74 = Header->_unknown0x74;
            _unknown0x75 = Header->_unknown0x75;
            _unknown0x76 = Header->_unknown0x76;
            _unknown0x77 = Header->_unknown0x77;
            _unknown0x78 = Header->_unknown0x78;
            _unknown0x79 = Header->_unknown0x79;
            _unknown0x7A = Header->_unknown0x7A;
            _unknown0x7B = Header->_unknown0x7B;
            _unknown0x7C = Header->_unknown0x7C;
            _spawnid = Header->_spawnid;
            _flag0x7E = Header->_flag0x7E;
            _flag0x7F = Header->_flag0x7F;
            _flag0x80 = Header->_flag0x80;
            _flag0x81 = Header->_flag0x81;
            _flag0x82 = Header->_flag0x82;
            _flag0x83 = Header->_flag0x83;
            if (_name == null)
            {
                _name = EnemyNameList();
            }

            return false;
        }

        public override int OnCalculateSize(bool force)
        {
            return 0x84;
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            GEG1Entry* hdr = (GEG1Entry*) address;
            hdr->_header1 = _header1;
            hdr->_header2 = _header2;
            hdr->_extrahealth = _extrahealth;
            hdr->_flag0x09 = _flag0x09;
            hdr->_flag0x0A = _flag0x0A;
            hdr->_flag0x0B = _flag0x0B;
            hdr->_flag0x0C = _flag0x0C;
            hdr->_connectedenemyid = _connectedenemyid;
            hdr->_flag0x0E = _flag0x0E;
            hdr->_flag0x0F = _flag0x0F;
            hdr->_unknown0x10 = _unknown0x10;
            hdr->_unknown0x11 = _unknown0x11;
            hdr->_unknown0x12 = _unknown0x12;
            hdr->_unknown0x13 = _unknown0x13;
            hdr->_unknown0x14 = _unknown0x14;
            hdr->_unknown0x15 = _unknown0x15;
            hdr->_unknown0x16 = _unknown0x16;
            hdr->_unknown0x17 = _unknown0x17;
            hdr->_unknown0x18 = _unknown0x18;
            hdr->_unknown0x19 = _unknown0x19;
            hdr->_unknown0x1A = _unknown0x1A;
            hdr->_unknown0x1B = _unknown0x1B;
            hdr->_unknown0x1C = _unknown0x1C;
            hdr->_enemyID = _enemyID;
            hdr->_unknown0x1E = _unknown0x1E;
            hdr->_unknown0x1F = _unknown0x1F;
            hdr->_unknown0x20 = _unknown0x20;
            hdr->_unknown0x21 = _unknown0x21;
            hdr->_unknown0x22 = _unknown0x22;
            hdr->_startingaction = _startingaction;
            hdr->_unknown0x24 = _unknown0x24;
            hdr->_unknown0x25 = _unknown0x25;
            hdr->_unknown0x26 = _unknown0x26;
            hdr->_unknown0x27 = _unknown0x27;
            hdr->_spawnX = _spawnPos._x;
            hdr->_spawnY = _spawnPos._y;
            hdr->_unknown0x30 = _unknown0x30;
            hdr->_unknown0x31 = _unknown0x31;
            hdr->_unknown0x32 = _unknown0x32;
            hdr->_unknown0x33 = _unknown0x33;
            hdr->_unknown0x34 = _unknown0x34;
            hdr->_unknown0x35 = _unknown0x35;
            hdr->_unknown0x36 = _unknown0x36;
            hdr->_unknown0x37 = _unknown0x37;
            hdr->_unknown0x38 = _unknown0x38;
            hdr->_unknown0x39 = _unknown0x39;
            hdr->_unknown0x3A = _unknown0x3A;
            hdr->_unknown0x3B = _unknown0x3B;
            hdr->_unknown0x3C = _unknown0x3C;
            hdr->_unknown0x3D = _unknown0x3D;
            hdr->_unknown0x3E = _unknown0x3E;
            hdr->_unknown0x3F = _unknown0x3F;
            hdr->_unknown0x40 = _unknown0x40;
            hdr->_unknown0x41 = _unknown0x41;
            hdr->_unknown0x42 = _unknown0x42;
            hdr->_unknown0x43 = _unknown0x43;
            hdr->_unknown0x44 = _unknown0x44;
            hdr->_unknown0x45 = _unknown0x45;
            hdr->_unknown0x46 = _unknown0x46;
            hdr->_unknown0x47 = _unknown0x47;
            hdr->_unknown0x48 = _unknown0x48;
            hdr->_unknown0x49 = _unknown0x49;
            hdr->_unknown0x4A = _unknown0x4A;
            hdr->_unknown0x4B = _unknown0x4B;
            hdr->_unknown0x4C = _unknown0x4C;
            hdr->_unknown0x4D = _unknown0x4D;
            hdr->_unknown0x4E = _unknown0x4E;
            hdr->_unknown0x4F = _unknown0x4F;
            hdr->_unknown0x50 = _unknown0x50;
            hdr->_unknown0x51 = _unknown0x51;
            hdr->_unknown0x52 = _unknown0x52;
            hdr->_unknown0x53 = _unknown0x53;
            hdr->_unknown0x54 = _unknown0x54;
            hdr->_unknown0x55 = _unknown0x55;
            hdr->_unknown0x56 = _unknown0x56;
            hdr->_unknown0x57 = _unknown0x57;
            hdr->_unknown0x58 = _unknown0x58;
            hdr->_unknown0x59 = _unknown0x59;
            hdr->_unknown0x5A = _unknown0x5A;
            hdr->_unknown0x5B = _unknown0x5B;
            hdr->_unknown0x5C = _unknown0x5C;
            hdr->_unknown0x5D = _unknown0x5D;
            hdr->_unknown0x5E = _unknown0x5E;
            hdr->_unknown0x5F = _unknown0x5F;
            hdr->_unknown0x60 = _unknown0x60;
            hdr->_unknown0x61 = _unknown0x61;
            hdr->_unknown0x62 = _unknown0x62;
            hdr->_unknown0x63 = _unknown0x63;
            hdr->_flag0x64 = _flag0x64;
            hdr->_unknown0x65 = _unknown0x65;
            hdr->_unknown0x66 = _unknown0x66;
            hdr->_flag0x67 = _flag0x67;
            hdr->_unknown0x68 = _unknown0x68;
            hdr->_unknown0x69 = _unknown0x69;
            hdr->_unknown0x6A = _unknown0x6A;
            hdr->_unknown0x6B = _unknown0x6B;
            hdr->_unknown0x6C = _unknown0x6C;
            hdr->_unknown0x6D = _unknown0x6D;
            hdr->_unknown0x6E = _unknown0x6E;
            hdr->_unknown0x6F = _unknown0x6F;
            hdr->_unknown0x70 = _unknown0x70;
            hdr->_unknown0x71 = _unknown0x71;
            hdr->_unknown0x72 = _unknown0x72;
            hdr->_unknown0x73 = _unknown0x73;
            hdr->_unknown0x74 = _unknown0x74;
            hdr->_unknown0x75 = _unknown0x75;
            hdr->_unknown0x76 = _unknown0x76;
            hdr->_unknown0x77 = _unknown0x77;
            hdr->_unknown0x78 = _unknown0x78;
            hdr->_unknown0x79 = _unknown0x79;
            hdr->_unknown0x7A = _unknown0x7A;
            hdr->_unknown0x7B = _unknown0x7B;
            hdr->_unknown0x7C = _unknown0x7C;
            hdr->_spawnid = _spawnid;
            hdr->_flag0x7E = _flag0x7E;
            hdr->_flag0x7F = _flag0x7F;
            hdr->_flag0x80 = _flag0x80;
            hdr->_flag0x81 = _flag0x81;
            hdr->_flag0x82 = _flag0x82;
            hdr->_flag0x83 = _flag0x83;
        }

        private string EnemyNameList()
        {
            if (Enum.IsDefined(typeof(EnemyList), _enemyID))
            {
                switch (_enemyID)
                {
                    case 0:
                        return "Goomba [" + Index + "]";
                    case 1:
                        return "Poppant [" + Index + "]";
                    case 2:
                        return "Feyesh [" + Index + "]";
                    case 3:
                        return "Jyk [" + Index + "]";
                    case 4:
                        return "Auroros [" + Index + "]";
                    case 5:
                        return "Cymul [" + Index + "]";
                    case 6:
                        return "Roturret [" + Index + "]";
                    case 7:
                        return "Borboras [" + Index + "]";
                    case 8:
                        return "Giant Goomba [" + Index + "]";
                    case 9:
                        return "Buckot [" + Index + "]";
                    case 10:
                        return "Bucculus [" + Index + "]";
                    case 11:
                        return "Greap [" + Index + "]";
                    case 12:
                        return "Armight [" + Index + "]";
                    case 13:
                        return "Bullet Bill [" + Index + "]";
                    case 14:
                        return "Roader [" + Index + "]";
                    case 15:
                        return "Spaak [" + Index + "]";
                    case 16:
                        return "Mite [" + Index + "]";
                    case 17:
                        return "Ticken [" + Index + "]";
                    case 18:
                        return "Towtow [" + Index + "]";
                    case 19:
                        return "Hammer Bro [" + Index + "]";
                    case 20:
                        return "Bytan [" + Index + "]";
                    case 21:
                        return "Floow [" + Index + "]";
                    case 22:
                        return "Puppit [" + Index + "]";
                    case 23:
                        return "Primid [" + Index + "]";
                    case 24:
                        return "Shellpod [" + Index + "]";
                    case 25:
                        return "Koopa [" + Index + "]";
                    case 26:
                        return "Shaydas [" + Index + "]";
                    case 27:
                        return "Bombed [" + Index + "]";
                    case 28:
                        return "Metal Primid [" + Index + "]";
                    case 29:
                        return "Nagagog [" + Index + "]";
                    case 30:
                        return "Trowlon [" + Index + "]";
                    case 31:
                        return "Big Primid [" + Index + "]";
                    case 32:
                        return "Boomerang Primid [" + Index + "]";
                    case 33:
                        return "Fire Primid [" + Index + "]";
                    case 34:
                        return "Scope Primid [" + Index + "]";
                    case 35:
                        return "Sword Primid [" + Index + "]";
                    case 36:
                        return "Gamyga [" + Index + "]";
                    case 37:
                        return "R.O.B. Blaster [" + Index + "]";
                    case 38:
                        return "R.O.B. Distance (?) [" + Index + "]";
                    case 39:
                        return "R.O.B. Launcher [" + Index + "]";
                    case 40:
                        return "R.O.B. Sentry [" + Index + "]";
                    case 41:
                        return "Autolance [" + Index + "]";
                    case 42:
                        return "Armank [" + Index + "]";
                    case 43:
                        return "Glire [" + Index + "]";
                    case 44:
                        return "Glice [" + Index + "]";
                    case 45:
                        return "Glunder [" + Index + "]";
                    case 46:
                        return "Petey Piranha [" + Index + "]";
                    case 47:
                        return "Gamyga Base 01 (Top) [" + Index + "]";
                    case 48:
                        return "Gamyga Base 02 [" + Index + "]";
                    case 49:
                        return "Gamyga Base 03 [" + Index + "]";
                    case 50:
                        return "Gamyga Base 04 (Bottom) [" + Index + "]";
                    case 51:
                        return "Galleom [" + Index + "]";
                    case 52:
                        return "Ridley [" + Index + "]";
                    case 53:
                        return "Rayquaza [" + Index + "]";
                    case 54:
                        return "Duon [" + Index + "]";
                    case 55:
                        return "Porky [" + Index + "]";
                    case 56:
                        return "Meta Ridley [" + Index + "]";
                    case 57:
                        return "Falcon Flyer [" + Index + "]";
                    case 58:
                        return "Tabuu [" + Index + "]";
                    case 59:
                        return "Master Hand [" + Index + "]";
                    case 60:
                        return "Crazy Hand [" + Index + "]";
                    default:
                        return "Unknown Enemy [" + Index + "]";
                }
            }

            return "Unknown Enemy [" + Index + "]";
        }
    }
}