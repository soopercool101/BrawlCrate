using System.ComponentModel;
using BrawlLib.Internal;
using BrawlLib.SSBB.Types.ProjectPlus;
using static BrawlLib.SSBB.Types.ProjectPlus.ENOV;

namespace BrawlLib.SSBB.ResourceNodes.ProjectPlus
{
    public unsafe class ENOVNode : ARCEntryNode
    {
        public ENOV Data;

        private string _enemyOverrideFolder;

        [Category("Enemy Override")]
        public string EnemyOverrideFolder
        {
            get => _enemyOverrideFolder;
            set
            {
                _enemyOverrideFolder = value;
                SignalPropertyChange();
            }
        }

        private string _stageItemFolder;
        [Category("Enemy Override")]
        public string StageItemFolder
        {
            get => _stageItemFolder;
            set
            {
                _stageItemFolder = value;
                SignalPropertyChange();
            }
        }


        [Category("Enemy Override")]
        public bool UseIndividualFolders
        {
            get => Data.UseIndividualFolders;
            set
            {
                Data.UseIndividualFolders = value;
                SignalPropertyChange();
            }
        }

        [Category("Enemy Override")]
        public bool OverrideModuleCommon
        {
            get => Data.OverrideModuleCommon;
            set
            {
                Data.OverrideModuleCommon = value;
                SignalPropertyChange();
            }
        }

        [Category("Enemy Override")]
        public ArchiveOverrideSetting OverrideSettingsCommon
        {
            get => Data.OverrideSettingsCommon;
            set
            {
                Data.OverrideSettingsCommon = value;
                SignalPropertyChange();
            }
        }
        private OverriddenEnemyClass _goomba;

        [Category("Enemy Override")]
        [TypeConverter(typeof(ExpandableObjectCustomConverter))]
        public OverriddenEnemyClass Goomba
        {
            get => _goomba;
            set
            {
                _goomba = value;
                SignalPropertyChange();
            }
        }

        private OverriddenEnemyClass _poppant;

        [Category("Enemy Override")]
        [TypeConverter(typeof(ExpandableObjectCustomConverter))]
        public OverriddenEnemyClass Poppant
        {
            get => _poppant;
            set
            {
                _poppant = value;
                SignalPropertyChange();
            }
        }

        private OverriddenEnemyClass _feyesh;

        [Category("Enemy Override")]
        [TypeConverter(typeof(ExpandableObjectCustomConverter))]
        public OverriddenEnemyClass Feyesh
        {
            get => _feyesh;
            set
            {
                _feyesh = value;
                SignalPropertyChange();
            }
        }

        private OverriddenEnemyClass _jyk;

        [Category("Enemy Override")]
        [TypeConverter(typeof(ExpandableObjectCustomConverter))]
        public OverriddenEnemyClass Jyk
        {
            get => _jyk;
            set
            {
                _jyk = value;
                SignalPropertyChange();
            }
        }

        private OverriddenEnemyClass _auroros;

        [Category("Enemy Override")]
        [TypeConverter(typeof(ExpandableObjectCustomConverter))]
        public OverriddenEnemyClass Auroros
        {
            get => _auroros;
            set
            {
                _auroros = value;
                SignalPropertyChange();
            }
        }

        private OverriddenEnemyClass _cymul;

        [Category("Enemy Override")]
        [TypeConverter(typeof(ExpandableObjectCustomConverter))]
        public OverriddenEnemyClass Cymul
        {
            get => _cymul;
            set
            {
                _cymul = value;
                SignalPropertyChange();
            }
        }

        private OverriddenEnemyClass _roturret;

        [Category("Enemy Override")]
        [TypeConverter(typeof(ExpandableObjectCustomConverter))]
        public OverriddenEnemyClass Roturret
        {
            get => _roturret;
            set
            {
                _roturret = value;
                SignalPropertyChange();
            }
        }

        private OverriddenEnemyClass _borboras;

        [Category("Enemy Override")]
        [TypeConverter(typeof(ExpandableObjectCustomConverter))]
        public OverriddenEnemyClass Borboras
        {
            get => _borboras;
            set
            {
                _borboras = value;
                SignalPropertyChange();
            }
        }

        private OverriddenEnemyClass _giantGoomba;

        [Category("Enemy Override")]
        [TypeConverter(typeof(ExpandableObjectCustomConverter))]
        [DisplayName("Giant Goomba")]
        public OverriddenEnemyClass GiantGoomba
        {
            get => _giantGoomba;
            set
            {
                _giantGoomba = value;
                SignalPropertyChange();
            }
        }

        private OverriddenEnemyClass _buckot;

        [Category("Enemy Override")]
        [TypeConverter(typeof(ExpandableObjectCustomConverter))]
        public OverriddenEnemyClass Buckot
        {
            get => _buckot;
            set
            {
                _buckot = value;
                SignalPropertyChange();
            }
        }

        private OverriddenEnemyClass _bucculus;

        [Category("Enemy Override")]
        [TypeConverter(typeof(ExpandableObjectCustomConverter))]
        public OverriddenEnemyClass Bucculus
        {
            get => _bucculus;
            set
            {
                _bucculus = value;
                SignalPropertyChange();
            }
        }

        private OverriddenEnemyClass _greap;

        [Category("Enemy Override")]
        [TypeConverter(typeof(ExpandableObjectCustomConverter))]
        public OverriddenEnemyClass Greap
        {
            get => _greap;
            set
            {
                _greap = value;
                SignalPropertyChange();
            }
        }

        private OverriddenEnemyClass _armight;

        [Category("Enemy Override")]
        [TypeConverter(typeof(ExpandableObjectCustomConverter))]
        public OverriddenEnemyClass Armight
        {
            get => _armight;
            set
            {
                _armight = value;
                SignalPropertyChange();
            }
        }

        private OverriddenEnemyClass _bulletBill;

        [Category("Enemy Override")]
        [TypeConverter(typeof(ExpandableObjectCustomConverter))]
        [DisplayName("Bullet Bill")]
        public OverriddenEnemyClass BulletBill
        {
            get => _bulletBill;
            set
            {
                _bulletBill = value;
                SignalPropertyChange();
            }
        }

        private OverriddenEnemyClass _roader;

        [Category("Enemy Override")]
        [TypeConverter(typeof(ExpandableObjectCustomConverter))]
        public OverriddenEnemyClass Roader
        {
            get => _roader;
            set
            {
                _roader = value;
                SignalPropertyChange();
            }
        }

        private OverriddenEnemyClass _spaak;

        [Category("Enemy Override")]
        [TypeConverter(typeof(ExpandableObjectCustomConverter))]
        public OverriddenEnemyClass Spaak
        {
            get => _spaak;
            set
            {
                _spaak = value;
                SignalPropertyChange();
            }
        }

        private OverriddenEnemyClass _mite;

        [Category("Enemy Override")]
        [TypeConverter(typeof(ExpandableObjectCustomConverter))]
        public OverriddenEnemyClass Mite
        {
            get => _mite;
            set
            {
                _mite = value;
                SignalPropertyChange();
            }
        }

        private OverriddenEnemyClass _ticken;

        [Category("Enemy Override")]
        [TypeConverter(typeof(ExpandableObjectCustomConverter))]
        public OverriddenEnemyClass Ticken
        {
            get => _ticken;
            set
            {
                _ticken = value;
                SignalPropertyChange();
            }
        }

        private OverriddenEnemyClass _towtow;

        [Category("Enemy Override")]
        [TypeConverter(typeof(ExpandableObjectCustomConverter))]
        public OverriddenEnemyClass Towtow
        {
            get => _towtow;
            set
            {
                _towtow = value;
                SignalPropertyChange();
            }
        }

        private OverriddenEnemyClass _hammerBro;

        [Category("Enemy Override")]
        [TypeConverter(typeof(ExpandableObjectCustomConverter))]
        [DisplayName("Hammer Bro")]
        public OverriddenEnemyClass HammerBro
        {
            get => _hammerBro;
            set
            {
                _hammerBro = value;
                SignalPropertyChange();
            }
        }

        private OverriddenEnemyClass _bytan;

        [Category("Enemy Override")]
        [TypeConverter(typeof(ExpandableObjectCustomConverter))]
        public OverriddenEnemyClass Bytan
        {
            get => _bytan;
            set
            {
                _bytan = value;
                SignalPropertyChange();
            }
        }

        private OverriddenEnemyClass _floow;

        [Category("Enemy Override")]
        [TypeConverter(typeof(ExpandableObjectCustomConverter))]
        public OverriddenEnemyClass Floow
        {
            get => _floow;
            set
            {
                _floow = value;
                SignalPropertyChange();
            }
        }

        private OverriddenEnemyClass _puppit;

        [Category("Enemy Override")]
        [TypeConverter(typeof(ExpandableObjectCustomConverter))]
        public OverriddenEnemyClass Puppit
        {
            get => _puppit;
            set
            {
                _puppit = value;
                SignalPropertyChange();
            }
        }

        private OverriddenEnemyClass _primid;

        [Category("Enemy Override")]
        [TypeConverter(typeof(ExpandableObjectCustomConverter))]
        public OverriddenEnemyClass Primid
        {
            get => _primid;
            set
            {
                _primid = value;
                SignalPropertyChange();
            }
        }

        private OverriddenEnemyClass _shellpod;

        [Category("Enemy Override")]
        [TypeConverter(typeof(ExpandableObjectCustomConverter))]
        public OverriddenEnemyClass Shellpod
        {
            get => _shellpod;
            set
            {
                _shellpod = value;
                SignalPropertyChange();
            }
        }

        private OverriddenEnemyClass _koopa;

        [Category("Enemy Override")]
        [TypeConverter(typeof(ExpandableObjectCustomConverter))]
        public OverriddenEnemyClass Koopa
        {
            get => _koopa;
            set
            {
                _koopa = value;
                SignalPropertyChange();
            }
        }

        private OverriddenEnemyClass _shaydas;

        [Category("Enemy Override")]
        [TypeConverter(typeof(ExpandableObjectCustomConverter))]
        public OverriddenEnemyClass Shaydas
        {
            get => _shaydas;
            set
            {
                _shaydas = value;
                SignalPropertyChange();
            }
        }

        private OverriddenEnemyClass _bombed;

        [Category("Enemy Override")]
        [TypeConverter(typeof(ExpandableObjectCustomConverter))]
        public OverriddenEnemyClass Bombed
        {
            get => _bombed;
            set
            {
                _bombed = value;
                SignalPropertyChange();
            }
        }

        private OverriddenEnemyClass _metalPrimid;

        [Category("Enemy Override")]
        [TypeConverter(typeof(ExpandableObjectCustomConverter))]
        [DisplayName("Metal Primid")]
        public OverriddenEnemyClass MetalPrimid
        {
            get => _metalPrimid;
            set
            {
                _metalPrimid = value;
                SignalPropertyChange();
            }
        }

        private OverriddenEnemyClass _nagagog;

        [Category("Enemy Override")]
        [TypeConverter(typeof(ExpandableObjectCustomConverter))]
        public OverriddenEnemyClass Nagagog
        {
            get => _nagagog;
            set
            {
                _nagagog = value;
                SignalPropertyChange();
            }
        }

        private OverriddenEnemyClass _trowlon;

        [Category("Enemy Override")]
        [TypeConverter(typeof(ExpandableObjectCustomConverter))]
        public OverriddenEnemyClass Trowlon
        {
            get => _trowlon;
            set
            {
                _trowlon = value;
                SignalPropertyChange();
            }
        }

        private OverriddenEnemyClass _bigPrimid;

        [Category("Enemy Override")]
        [TypeConverter(typeof(ExpandableObjectCustomConverter))]
        [DisplayName("Big Primid")]
        public OverriddenEnemyClass BigPrimid
        {
            get => _bigPrimid;
            set
            {
                _bigPrimid = value;
                SignalPropertyChange();
            }
        }

        private OverriddenEnemyClass _boomPrimid;

        [Category("Enemy Override")]
        [TypeConverter(typeof(ExpandableObjectCustomConverter))]
        [DisplayName("Boom Primid")]
        public OverriddenEnemyClass BoomPrimid
        {
            get => _boomPrimid;
            set
            {
                _boomPrimid = value;
                SignalPropertyChange();
            }
        }

        private OverriddenEnemyClass _firePrimid;

        [Category("Enemy Override")]
        [TypeConverter(typeof(ExpandableObjectCustomConverter))]
        [DisplayName("Fire Primid")]
        public OverriddenEnemyClass FirePrimid
        {
            get => _firePrimid;
            set
            {
                _firePrimid = value;
                SignalPropertyChange();
            }
        }

        private OverriddenEnemyClass _scopePrimid;

        [Category("Enemy Override")]
        [TypeConverter(typeof(ExpandableObjectCustomConverter))]
        [DisplayName("Scope Primid")]
        public OverriddenEnemyClass ScopePrimid
        {
            get => _scopePrimid;
            set
            {
                _scopePrimid = value;
                SignalPropertyChange();
            }
        }

        private OverriddenEnemyClass _swordPrimid;

        [Category("Enemy Override")]
        [TypeConverter(typeof(ExpandableObjectCustomConverter))]
        [DisplayName("Sword Primid")]
        public OverriddenEnemyClass SwordPrimid
        {
            get => _swordPrimid;
            set
            {
                _swordPrimid = value;
                SignalPropertyChange();
            }
        }

        private OverriddenEnemyClass _gamyga;

        [Category("Enemy Override")]
        [TypeConverter(typeof(ExpandableObjectCustomConverter))]
        public OverriddenEnemyClass Gamyga
        {
            get => _gamyga;
            set
            {
                _gamyga = value;
                SignalPropertyChange();
            }
        }

        private OverriddenEnemyClass _rObBlasterStationary;

        [Category("Enemy Override")]
        [TypeConverter(typeof(ExpandableObjectCustomConverter))]
        [DisplayName("R.O.B. Blaster (Stationary)")]
        public OverriddenEnemyClass ROBBlasterStationary
        {
            get => _rObBlasterStationary;
            set
            {
                _rObBlasterStationary = value;
                SignalPropertyChange();
            }
        }

        private OverriddenEnemyClass _rObBlasterMobile;

        [Category("Enemy Override")]
        [TypeConverter(typeof(ExpandableObjectCustomConverter))]
        [DisplayName("R.O.B. Blaster (Mobile)")]
        public OverriddenEnemyClass ROBBlasterMobile
        {
            get => _rObBlasterMobile;
            set
            {
                _rObBlasterMobile = value;
                SignalPropertyChange();
            }
        }

        private OverriddenEnemyClass _rOBLauncher;

        [Category("Enemy Override")]
        [TypeConverter(typeof(ExpandableObjectCustomConverter))]
        [DisplayName("R.O.B. Launcher")]
        public OverriddenEnemyClass ROBLauncher
        {
            get => _rOBLauncher;
            set
            {
                _rOBLauncher = value;
                SignalPropertyChange();
            }
        }

        private OverriddenEnemyClass _rOBSentry;

        [Category("Enemy Override")]
        [TypeConverter(typeof(ExpandableObjectCustomConverter))]
        [DisplayName("R.O.B. Sentry")]
        public OverriddenEnemyClass ROBSentry
        {
            get => _rOBSentry;
            set
            {
                _rOBSentry = value;
                SignalPropertyChange();
            }
        }

        private OverriddenEnemyClass _autolance;

        [Category("Enemy Override")]
        [TypeConverter(typeof(ExpandableObjectCustomConverter))]
        public OverriddenEnemyClass Autolance
        {
            get => _autolance;
            set
            {
                _autolance = value;
                SignalPropertyChange();
            }
        }

        private OverriddenEnemyClass _armank;

        [Category("Enemy Override")]
        [TypeConverter(typeof(ExpandableObjectCustomConverter))]
        public OverriddenEnemyClass Armank
        {
            get => _armank;
            set
            {
                _armank = value;
                SignalPropertyChange();
            }
        }

        private OverriddenEnemyClass _glire;

        [Category("Enemy Override")]
        [TypeConverter(typeof(ExpandableObjectCustomConverter))]
        public OverriddenEnemyClass Glire
        {
            get => _glire;
            set
            {
                _glire = value;
                SignalPropertyChange();
            }
        }

        private OverriddenEnemyClass _glice;

        [Category("Enemy Override")]
        [TypeConverter(typeof(ExpandableObjectCustomConverter))]
        public OverriddenEnemyClass Glice
        {
            get => _glice;
            set
            {
                _glice = value;
                SignalPropertyChange();
            }
        }

        private OverriddenEnemyClass _glunder;

        [Category("Enemy Override")]
        [TypeConverter(typeof(ExpandableObjectCustomConverter))]
        public OverriddenEnemyClass Glunder
        {
            get => _glunder;
            set
            {
                _glunder = value;
                SignalPropertyChange();
            }
        }

        private OverriddenEnemyClass _peteyPiranha;

        [Category("Enemy Override")]
        [TypeConverter(typeof(ExpandableObjectCustomConverter))]
        [DisplayName("Petey Piranha")]
        public OverriddenEnemyClass PeteyPiranha
        {
            get => _peteyPiranha;
            set
            {
                _peteyPiranha = value;
                SignalPropertyChange();
            }
        }

        private OverriddenEnemyClass _gamygaBase1;

        [Category("Enemy Override")]
        [TypeConverter(typeof(ExpandableObjectCustomConverter))]
        [DisplayName("Gamyga Base 1")]
        public OverriddenEnemyClass GamygaBase1
        {
            get => _gamygaBase1;
            set
            {
                _gamygaBase1 = value;
                SignalPropertyChange();
            }
        }

        private OverriddenEnemyClass _gamygaBase2;

        [Category("Enemy Override")]
        [TypeConverter(typeof(ExpandableObjectCustomConverter))]
        [DisplayName("Gamyga Base 2")]
        public OverriddenEnemyClass GamygaBase2
        {
            get => _gamygaBase2;
            set
            {
                _gamygaBase2 = value;
                SignalPropertyChange();
            }
        }

        private OverriddenEnemyClass _gamygaBase3;

        [Category("Enemy Override")]
        [TypeConverter(typeof(ExpandableObjectCustomConverter))]
        [DisplayName("Gamyga Base 3")]
        public OverriddenEnemyClass GamygaBase3
        {
            get => _gamygaBase3;
            set
            {
                _gamygaBase3 = value;
                SignalPropertyChange();
            }
        }

        private OverriddenEnemyClass _gamygaBase4;

        [Category("Enemy Override")]
        [TypeConverter(typeof(ExpandableObjectCustomConverter))]
        [DisplayName("Gamyga Base 4")]
        public OverriddenEnemyClass GamygaBase4
        {
            get => _gamygaBase4;
            set
            {
                _gamygaBase4 = value;
                SignalPropertyChange();
            }
        }

        private OverriddenEnemyClass _galleom;

        [Category("Enemy Override")]
        [TypeConverter(typeof(ExpandableObjectCustomConverter))]
        public OverriddenEnemyClass Galleom
        {
            get => _galleom;
            set
            {
                _galleom = value;
                SignalPropertyChange();
            }
        }

        private OverriddenEnemyClass _ridley;

        [Category("Enemy Override")]
        [TypeConverter(typeof(ExpandableObjectCustomConverter))]
        public OverriddenEnemyClass Ridley
        {
            get => _ridley;
            set
            {
                _ridley = value;
                SignalPropertyChange();
            }
        }

        private OverriddenEnemyClass _rayquaza;

        [Category("Enemy Override")]
        [TypeConverter(typeof(ExpandableObjectCustomConverter))]
        public OverriddenEnemyClass Rayquaza
        {
            get => _rayquaza;
            set
            {
                _rayquaza = value;
                SignalPropertyChange();
            }
        }

        private OverriddenEnemyClass _duon;

        [Category("Enemy Override")]
        [TypeConverter(typeof(ExpandableObjectCustomConverter))]
        public OverriddenEnemyClass Duon
        {
            get => _duon;
            set
            {
                _duon = value;
                SignalPropertyChange();
            }
        }

        private OverriddenEnemyClass _porky;

        [Category("Enemy Override")]
        [TypeConverter(typeof(ExpandableObjectCustomConverter))]
        public OverriddenEnemyClass Porky
        {
            get => _porky;
            set
            {
                _porky = value;
                SignalPropertyChange();
            }
        }

        private OverriddenEnemyClass _metaRidley;

        [Category("Enemy Override")]
        [TypeConverter(typeof(ExpandableObjectCustomConverter))]
        [DisplayName("Meta Ridley")]
        public OverriddenEnemyClass MetaRidley
        {
            get => _metaRidley;
            set
            {
                _metaRidley = value;
                SignalPropertyChange();
            }
        }

        private OverriddenEnemyClass _falconFlyer;

        [Category("Enemy Override")]
        [TypeConverter(typeof(ExpandableObjectCustomConverter))]
        [DisplayName("Falcon Flyer")]
        public OverriddenEnemyClass FalconFlyer
        {
            get => _falconFlyer;
            set
            {
                _falconFlyer = value;
                SignalPropertyChange();
            }
        }

        private OverriddenEnemyClass _tabuu;

        [Category("Enemy Override")]
        [TypeConverter(typeof(ExpandableObjectCustomConverter))]
        public OverriddenEnemyClass Tabuu
        {
            get => _tabuu;
            set
            {
                _tabuu = value;
                SignalPropertyChange();
            }
        }

        private OverriddenEnemyClass _masterHand;

        [Category("Enemy Override")]
        [TypeConverter(typeof(ExpandableObjectCustomConverter))]
        [DisplayName("Master Hand")]
        public OverriddenEnemyClass MasterHand
        {
            get => _masterHand;
            set
            {
                _masterHand = value;
                SignalPropertyChange();
            }
        }

        private OverriddenEnemyClass _crazyHand;

        [Category("Enemy Override")]
        [TypeConverter(typeof(ExpandableObjectCustomConverter))]
        [DisplayName("Crazy Hand")]
        public OverriddenEnemyClass CrazyHand
        {
            get => _crazyHand;
            set
            {
                _crazyHand = value;
                SignalPropertyChange();
            }
        }

        public override string FileTypeString => "Enemy Override";

        public override bool OnInitialize()
        {
            Data = *(ENOV*)WorkingUncompressed.Address;
            _enemyOverrideFolder = WorkingUncompressed.Address.GetUTF8String(4, 12);
            _stageItemFolder = WorkingUncompressed.Address.GetUTF8String(16, 12);
            _goomba = new OverriddenEnemyClass(this, Data._goomba);
            _poppant = new OverriddenEnemyClass(this, Data._poppant);
            _feyesh = new OverriddenEnemyClass(this, Data._feyesh);
            _jyk = new OverriddenEnemyClass(this, Data._jyk);
            _auroros = new OverriddenEnemyClass(this, Data._auroros);
            _cymul = new OverriddenEnemyClass(this, Data._cymul);
            _roturret = new OverriddenEnemyClass(this, Data._roturret);
            _borboras = new OverriddenEnemyClass(this, Data._borboras);
            _giantGoomba = new OverriddenEnemyClass(this, Data._giantGoomba);
            _buckot = new OverriddenEnemyClass(this, Data._buckot);
            _bucculus = new OverriddenEnemyClass(this, Data._bucculus);
            _greap = new OverriddenEnemyClass(this, Data._greap);
            _armight = new OverriddenEnemyClass(this, Data._armight);
            _bulletBill = new OverriddenEnemyClass(this, Data._bulletBill);
            _roader = new OverriddenEnemyClass(this, Data._roader);
            _spaak = new OverriddenEnemyClass(this, Data._spaak);
            _mite = new OverriddenEnemyClass(this, Data._mite);
            _ticken = new OverriddenEnemyClass(this, Data._ticken);
            _towtow = new OverriddenEnemyClass(this, Data._towtow);
            _hammerBro = new OverriddenEnemyClass(this, Data._hammerBro);
            _bytan = new OverriddenEnemyClass(this, Data._bytan);
            _floow = new OverriddenEnemyClass(this, Data._floow);
            _puppit = new OverriddenEnemyClass(this, Data._puppit);
            _primid = new OverriddenEnemyClass(this, Data._primid);
            _shellpod = new OverriddenEnemyClass(this, Data._shellpod);
            _koopa = new OverriddenEnemyClass(this, Data._koopa);
            _shaydas = new OverriddenEnemyClass(this, Data._shaydas);
            _bombed = new OverriddenEnemyClass(this, Data._bombed);
            _metalPrimid = new OverriddenEnemyClass(this, Data._metalPrimid);
            _nagagog = new OverriddenEnemyClass(this, Data._nagagog);
            _trowlon = new OverriddenEnemyClass(this, Data._trowlon);
            _bigPrimid = new OverriddenEnemyClass(this, Data._bigPrimid);
            _boomPrimid = new OverriddenEnemyClass(this, Data._boomPrimid);
            _firePrimid = new OverriddenEnemyClass(this, Data._firePrimid);
            _scopePrimid = new OverriddenEnemyClass(this, Data._scopePrimid);
            _swordPrimid = new OverriddenEnemyClass(this, Data._swordPrimid);
            _gamyga = new OverriddenEnemyClass(this, Data._gamyga);
            _rObBlasterStationary = new OverriddenEnemyClass(this, Data._rOBBlasterStationary);
            _rObBlasterMobile = new OverriddenEnemyClass(this, Data._rOBBlasterMobile);
            _rOBLauncher = new OverriddenEnemyClass(this, Data._rOBLauncher);
            _rOBSentry = new OverriddenEnemyClass(this, Data._rOBSentry);
            _autolance = new OverriddenEnemyClass(this, Data._autolance);
            _armank = new OverriddenEnemyClass(this, Data._armank);
            _glire = new OverriddenEnemyClass(this, Data._glire);
            _glice = new OverriddenEnemyClass(this, Data._glice);
            _glunder = new OverriddenEnemyClass(this, Data._glunder);
            _peteyPiranha = new OverriddenEnemyClass(this, Data._peteyPiranha);
            _gamygaBase1 = new OverriddenEnemyClass(this, Data._gamygaBase1);
            _gamygaBase2 = new OverriddenEnemyClass(this, Data._gamygaBase2);
            _gamygaBase3 = new OverriddenEnemyClass(this, Data._gamygaBase3);
            _gamygaBase4 = new OverriddenEnemyClass(this, Data._gamygaBase4);
            _galleom = new OverriddenEnemyClass(this, Data._galleom);
            _ridley = new OverriddenEnemyClass(this, Data._ridley);
            _rayquaza = new OverriddenEnemyClass(this, Data._rayquaza);
            _duon = new OverriddenEnemyClass(this, Data._duon);
            _porky = new OverriddenEnemyClass(this, Data._porky);
            _metaRidley = new OverriddenEnemyClass(this, Data._metaRidley);
            _falconFlyer = new OverriddenEnemyClass(this, Data._falconFlyer);
            _tabuu = new OverriddenEnemyClass(this, Data._tabuu);
            _masterHand = new OverriddenEnemyClass(this, Data._masterHand);
            _crazyHand = new OverriddenEnemyClass(this, Data._crazyHand);

            return false;
        }

        public override int OnCalculateSize(bool force)
        {
            return ENOV.Size;
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            Data._goomba = _goomba;
            Data._poppant = _poppant;
            Data._feyesh = _feyesh;
            Data._jyk = _jyk;
            Data._auroros = _auroros;
            Data._cymul = _cymul;
            Data._roturret = _roturret;
            Data._borboras = _borboras;
            Data._giantGoomba = _giantGoomba;
            Data._buckot = _buckot;
            Data._bucculus = _bucculus;
            Data._greap = _greap;
            Data._armight = _armight;
            Data._bulletBill = _bulletBill;
            Data._roader = _roader;
            Data._spaak = _spaak;
            Data._mite = _mite;
            Data._ticken = _ticken;
            Data._towtow = _towtow;
            Data._hammerBro = _hammerBro;
            Data._bytan = _bytan;
            Data._floow = _floow;
            Data._puppit = _puppit;
            Data._primid = _primid;
            Data._shellpod = _shellpod;
            Data._koopa = _koopa;
            Data._shaydas = _shaydas;
            Data._bombed = _bombed;
            Data._metalPrimid = _metalPrimid;
            Data._nagagog = _nagagog;
            Data._trowlon = _trowlon;
            Data._bigPrimid = _bigPrimid;
            Data._boomPrimid = _boomPrimid;
            Data._firePrimid = _firePrimid;
            Data._scopePrimid = _scopePrimid;
            Data._swordPrimid = _swordPrimid;
            Data._gamyga = _gamyga;
            Data._rOBBlasterStationary = _rObBlasterStationary;
            Data._rOBBlasterMobile = _rObBlasterMobile;
            Data._rOBLauncher = _rOBLauncher;
            Data._rOBSentry = _rOBSentry;
            Data._autolance = _autolance;
            Data._armank = _armank;
            Data._glire = _glire;
            Data._glice = _glice;
            Data._glunder = _glunder;
            Data._peteyPiranha = _peteyPiranha;
            Data._gamygaBase1 = _gamygaBase1;
            Data._gamygaBase2 = _gamygaBase2;
            Data._gamygaBase3 = _gamygaBase3;
            Data._gamygaBase4 = _gamygaBase4;
            Data._galleom = _galleom;
            Data._ridley = _ridley;
            Data._rayquaza = _rayquaza;
            Data._duon = _duon;
            Data._porky = _porky;
            Data._metaRidley = _metaRidley;
            Data._falconFlyer = _falconFlyer;
            Data._tabuu = _tabuu;
            Data._masterHand = _masterHand;
            Data._crazyHand = _crazyHand;
            *(ENOV*)address = Data;
            address.WriteUTF8String(ENOV.Tag, false, 0, 4);
            address.WriteUTF8String(EnemyOverrideFolder, false, 4, 12);
            address.WriteUTF8String(StageItemFolder, false, 16, 12);
        }

        internal static ResourceNode TryParse(DataSource source, ResourceNode parent)
        {
            return source.Tag == ENOV.Tag ? new ENOVNode() : null;
        }
    }

    public class OverriddenEnemyClass
    {
        public ENOVNode _parent;
        public OverriddenEnemy Data;

        public byte FaceIndex
        {
            get => Data.FaceIndex;
            set
            {
                Data.FaceIndex = value;
                _parent.SignalPropertyChange();
            }
        }

        public bool OverrideModule
        {
            get => Data.OverrideModule;
            set
            {
                Data.OverrideModule = value;
                _parent.SignalPropertyChange();
            }
        }

        public ArchiveOverrideSetting OverrideSetting
        {
            get => Data.OverrideSetting;
            set
            {
                Data.OverrideSetting = value;
                _parent.SignalPropertyChange();
            }
        }

        public OverriddenEnemyClass()
        {
            Data = new OverriddenEnemy();
        }

        public OverriddenEnemyClass(ENOVNode parent, OverriddenEnemy e)
        {
            _parent = parent;
            Data = e;
        }

        public OverriddenEnemyClass(OverriddenEnemy e)
        {
            Data = e;
        }

        public override string ToString()
        {
            // If settings are default, don't bother showing summary. Helps show what's actually overridden at a glance
            return Data._data == 0 ? string.Empty : Data.ToString();
        }

        public static implicit operator OverriddenEnemy(OverriddenEnemyClass val)
        {
            return val.Data;
        }

        public static implicit operator OverriddenEnemyClass(OverriddenEnemy val)
        {
            return new OverriddenEnemyClass(val);
        }
    }
}
