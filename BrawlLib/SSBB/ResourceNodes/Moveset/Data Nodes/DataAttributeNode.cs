using BrawlLib.Internal;
using BrawlLib.SSBB.Types;
using System.ComponentModel;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class MoveDefAttributeNode : MoveDefEntryNode
    {
        internal FDefAttributes* Header => (FDefAttributes*) WorkingUncompressed.Address;
        public override ResourceType ResourceFileType => ResourceType.Unknown;

        private UnsafeBuffer attributeBuffer;

        [Browsable(false)]
        public UnsafeBuffer AttributeBuffer
        {
            get
            {
                if (attributeBuffer != null)
                {
                    return attributeBuffer;
                }

                return attributeBuffer = new UnsafeBuffer(0x2E4);
            }
        }

        public MoveDefAttributeNode(string name)
        {
            _name = name;
        }

        #region Attributes

        //public float WalkInitialVelocity { get { return _walkInitVelocity; } set { _walkInitVelocity = value; SignalPropertyChange(); } }
        //public float WalkAcceleration { get { return _walkAcceleration; } set { _walkAcceleration = value; SignalPropertyChange(); } }
        //public float WalkMaxVelocity { get { return _walkMaxVelocity; } set { _walkMaxVelocity = value; SignalPropertyChange(); } }
        //public float StopVelocity { get { return _stopVelocity; } set { _stopVelocity = value; SignalPropertyChange(); } }
        //public float DashInitialVelocity { get { return _dashInitVelocity; } set { _dashInitVelocity = value; SignalPropertyChange(); } }
        //public float StopTurnDeceleration { get { return _stopTurnDecel; } set { _stopTurnDecel = value; SignalPropertyChange(); } }
        //public float StopTurnAcceleration { get { return _stopTurnAccel; } set { _stopTurnAccel = value; SignalPropertyChange(); } }
        //public float RunInitialVelocity { get { return _runInitVelocity; } set { _runInitVelocity = value; SignalPropertyChange(); } }

        //public float Unk09 { get { return _unk01; } set { _unk01 = value; SignalPropertyChange(); } }
        //public float Unk10 { get { return _unk02; } set { _unk02 = value; SignalPropertyChange(); } }
        //public int Unk11 { get { return _unk03; } set { _unk03 = value; SignalPropertyChange(); } }
        //public float Unk12 { get { return _unk04; } set { _unk04 = value; SignalPropertyChange(); } }
        //public int Unk13 { get { return _unk05; } set { _unk05 = value; SignalPropertyChange(); } }
        //public float Unk14 { get { return _unk06; } set { _unk06 = value; SignalPropertyChange(); } }

        //public float JumpYInitialVelocity { get { return _jumpXInitVelocity; } set { _jumpXInitVelocity = value; SignalPropertyChange(); } }

        //public float Unk16 { get { return _unk07; } set { _unk07 = value; SignalPropertyChange(); } }

        //public float JumpXInitialVelocity { get { return _jumpYInitVelocity; } set { _jumpYInitVelocity = value; SignalPropertyChange(); } }
        //public float HopYInitialVelocity { get { return _hopYInitVelocity; } set { _hopYInitVelocity = value; SignalPropertyChange(); } }
        //public float AirJumpMultiplier { get { return _airJumpMultiplier; } set { _airJumpMultiplier = value; SignalPropertyChange(); } }

        //public float Unk20 { get { return _unk08; } set { _unk08 = value; SignalPropertyChange(); } }

        //public float StoolYInitVelocity { get { return _stoolYInitVelocity; } set { _stoolYInitVelocity = value; SignalPropertyChange(); } }

        //public float Unk22 { get { return _unk09; } set { _unk09 = value; SignalPropertyChange(); } }
        //public float Unk23 { get { return _unk10; } set { _unk10 = value; SignalPropertyChange(); } }
        //public float Unk24 { get { return _unk11; } set { _unk11 = value; SignalPropertyChange(); } }
        //public int Unk25 { get { return _unk12; } set { _unk12 = value; SignalPropertyChange(); } }

        //public float Gravity { get { return _gravity; } set { _gravity = value; SignalPropertyChange(); } }
        //public float TerminalVelocity { get { return _termVelocity; } set { _termVelocity = value; SignalPropertyChange(); } }

        //public float Unk28 { get { return _unk13; } set { _unk13 = value; SignalPropertyChange(); } }
        //public float Unk29 { get { return _unk14; } set { _unk14 = value; SignalPropertyChange(); } }

        //public float AirMobility { get { return _airMobility; } set { _airMobility = value; SignalPropertyChange(); } }
        //public float AirStopMobility { get { return _airStopMobility; } set { _airStopMobility = value; SignalPropertyChange(); } }
        //public float AirMaxXVelocity { get { return _airMaxXVelocity; } set { _airMaxXVelocity = value; SignalPropertyChange(); } }

        //public float Unk33 { get { return _unk15; } set { _unk15 = value; SignalPropertyChange(); } }
        //public float Unk34 { get { return _unk16; } set { _unk16 = value; SignalPropertyChange(); } }
        //public float Unk35 { get { return _unk17; } set { _unk17 = value; SignalPropertyChange(); } }
        //public int Unk36 { get { return _unk18; } set { _unk18 = value; SignalPropertyChange(); } }
        //public float Unk37 { get { return _unk19; } set { _unk19 = value; SignalPropertyChange(); } }
        //public float Unk38 { get { return _unk20; } set { _unk20 = value; SignalPropertyChange(); } }
        //public float Unk39 { get { return _unk21; } set { _unk21 = value; SignalPropertyChange(); } }
        //public int Unk40 { get { return _unk22; } set { _unk22 = value; SignalPropertyChange(); } }
        //public int Unk41 { get { return _unk23; } set { _unk23 = value; SignalPropertyChange(); } }
        //public int Unk42 { get { return _unk24; } set { _unk24 = value; SignalPropertyChange(); } }
        //public float Unk43 { get { return _unk25; } set { _unk25 = value; SignalPropertyChange(); } }
        //public float Unk44 { get { return _unk26; } set { _unk26 = value; SignalPropertyChange(); } }

        //public float Weight { get { return _weight; } set { _weight = value; SignalPropertyChange(); } }

        //public float Unk46 { get { return _unk27; } set { _unk27 = value; SignalPropertyChange(); } }
        //public float Unk47 { get { return _unk28; } set { _unk28 = value; SignalPropertyChange(); } }
        //public float Unk48 { get { return _unk29; } set { _unk29 = value; SignalPropertyChange(); } }
        //public float Unk49 { get { return _unk30; } set { _unk30 = value; SignalPropertyChange(); } }

        //public float ShieldSize { get { return _shieldSize; } set { _shieldSize = value; SignalPropertyChange(); } }
        //public float ShieldBreakBounce { get { return _shieldBreakBounce; } set { _shieldBreakBounce = value; SignalPropertyChange(); } }

        //public float Unk52 { get { return _unk31; } set { _unk31 = value; SignalPropertyChange(); } }
        //public float Unk53 { get { return _unk32; } set { _unk32 = value; SignalPropertyChange(); } }
        //public float Unk54 { get { return _unk33; } set { _unk33 = value; SignalPropertyChange(); } }
        //public float Unk55 { get { return _unk34; } set { _unk34 = value; SignalPropertyChange(); } }
        //public float Unk56 { get { return _unk35; } set { _unk35 = value; SignalPropertyChange(); } }
        //public float Unk57 { get { return _unk36; } set { _unk36 = value; SignalPropertyChange(); } }
        //public float Unk58 { get { return _unk37; } set { _unk37 = value; SignalPropertyChange(); } }
        //public int Unk59 { get { return _unk38; } set { _unk38 = value; SignalPropertyChange(); } }
        //public int Unk60 { get { return _unk39; } set { _unk39 = value; SignalPropertyChange(); } }
        //public int Unk61 { get { return _unk40; } set { _unk40 = value; SignalPropertyChange(); } }
        //public float Unk62 { get { return _unk41; } set { _unk41 = value; SignalPropertyChange(); } }

        //public float EdgeJumpYVelocity { get { return _edgeJumpYVelocity; } set { _edgeJumpYVelocity = value; SignalPropertyChange(); } }
        //public float EdgeJumpXVelocity { get { return _edgeJumpXVelocity; } set { _edgeJumpXVelocity = value; SignalPropertyChange(); } }

        //public float Unk65 { get { return _unk42; } set { _unk42 = value; SignalPropertyChange(); } }
        //public float Unk66 { get { return _unk43; } set { _unk43 = value; SignalPropertyChange(); } }
        //public float Unk67 { get { return _unk44; } set { _unk44 = value; SignalPropertyChange(); } }
        //public float Unk68 { get { return _unk45; } set { _unk45 = value; SignalPropertyChange(); } }
        //public float Unk69 { get { return _unk46; } set { _unk46 = value; SignalPropertyChange(); } }
        //public int Unk70 { get { return _unk47; } set { _unk47 = value; SignalPropertyChange(); } }

        //public float ItemThrowStrength { get { return _itemThrowStrength; } set { _itemThrowStrength = value; SignalPropertyChange(); } }

        //public float Unk72 { get { return _unk48; } set { _unk48 = value; SignalPropertyChange(); } }
        //public float Unk73 { get { return _unk49; } set { _unk49 = value; SignalPropertyChange(); } }
        //public float Unk74 { get { return _unk50; } set { _unk50 = value; SignalPropertyChange(); } }

        //public float FireMoveSpeed { get { return _fireMoveSpeed; } set { _fireMoveSpeed = value; SignalPropertyChange(); } }
        //public float FireFDashSpeed { get { return _fireFDashSpeed; } set { _fireFDashSpeed = value; SignalPropertyChange(); } }
        //public float FireBDashSpeed { get { return _fireBDashSpeed; } set { _fireBDashSpeed = value; SignalPropertyChange(); } }

        //public float Unk78 { get { return _unk51; } set { _unk51 = value; SignalPropertyChange(); } }
        //public float Unk79 { get { return _unk52; } set { _unk52 = value; SignalPropertyChange(); } }
        //public float Unk80 { get { return _unk53; } set { _unk53 = value; SignalPropertyChange(); } }
        //public float Unk81 { get { return _unk54; } set { _unk54 = value; SignalPropertyChange(); } }
        //public float Unk82 { get { return _unk55; } set { _unk55 = value; SignalPropertyChange(); } }
        //public float Unk83 { get { return _unk56; } set { _unk56 = value; SignalPropertyChange(); } }
        //public float Unk84 { get { return _unk57; } set { _unk57 = value; SignalPropertyChange(); } }
        //public float Unk85 { get { return _unk58; } set { _unk58 = value; SignalPropertyChange(); } }
        //public int Unk86 { get { return _unk59; } set { _unk59 = value; SignalPropertyChange(); } }
        //public int Unk87 { get { return _unk60; } set { _unk60 = value; SignalPropertyChange(); } }
        //public float Unk88 { get { return _unk61; } set { _unk61 = value; SignalPropertyChange(); } }
        //public float Unk89 { get { return _unk62; } set { _unk62 = value; SignalPropertyChange(); } }

        //public float WallJumpYVelocity { get { return _wallJumpYVelocity; } set { _wallJumpYVelocity = value; SignalPropertyChange(); } }
        //public float WallJumpXVelocity { get { return _wallJumpXVelocity; } set { _wallJumpXVelocity = value; SignalPropertyChange(); } }

        //public float Unk92 { get { return _unk63; } set { _unk63 = value; SignalPropertyChange(); } }
        //public float Unk93 { get { return _unk64; } set { _unk64 = value; SignalPropertyChange(); } }
        //public int Unk94 { get { return _unk65; } set { _unk65 = value; SignalPropertyChange(); } }
        //public float Unk95 { get { return _unk66; } set { _unk66 = value; SignalPropertyChange(); } }
        //public float Unk96 { get { return _unk67; } set { _unk67 = value; SignalPropertyChange(); } }
        //public int Unk97 { get { return _unk68; } set { _unk68 = value; SignalPropertyChange(); } }
        //public int Unk98 { get { return _unk69; } set { _unk69 = value; SignalPropertyChange(); } }
        //public float Unk99 { get { return _unk70; } set { _unk70 = value; SignalPropertyChange(); } }
        //public float Unk100 { get { return _unk71; } set { _unk71 = value; SignalPropertyChange(); } }
        //public float Unk101 { get { return _unk72; } set { _unk72 = value; SignalPropertyChange(); } }
        //public float Unk102 { get { return _unk73; } set { _unk73 = value; SignalPropertyChange(); } }
        //public float Unk103 { get { return _unk74; } set { _unk74 = value; SignalPropertyChange(); } }
        //public float Unk104 { get { return _unk75; } set { _unk75 = value; SignalPropertyChange(); } }
        //public float Unk105 { get { return _unk76; } set { _unk76 = value; SignalPropertyChange(); } }
        //public float Unk106 { get { return _unk77; } set { _unk77 = value; SignalPropertyChange(); } }
        //public int Unk107 { get { return _unk78; } set { _unk78 = value; SignalPropertyChange(); } }
        //public float Unk108 { get { return _unk79; } set { _unk79 = value; SignalPropertyChange(); } }
        //public int Unk109 { get { return _unk80; } set { _unk80 = value; SignalPropertyChange(); } }

        //public float Unk110 { get { return _unk81; } set { _unk81 = value; SignalPropertyChange(); } }
        //public float Unk111 { get { return _unk82; } set { _unk82 = value; SignalPropertyChange(); } }
        //public float Unk112 { get { return _unk83; } set { _unk83 = value; SignalPropertyChange(); } }
        //public float Unk113 { get { return _unk84; } set { _unk84 = value; SignalPropertyChange(); } }
        //public float Unk114 { get { return _unk85; } set { _unk85 = value; SignalPropertyChange(); } }
        //public float Unk115 { get { return _unk86; } set { _unk86 = value; SignalPropertyChange(); } }
        //public int Unk116 { get { return _unk89; } set { _unk89 = value; SignalPropertyChange(); } }
        //public float Unk117 { get { return _unk90; } set { _unk90 = value; SignalPropertyChange(); } }
        //public float Unk118 { get { return _unk91; } set { _unk91 = value; SignalPropertyChange(); } }
        //public float Unk119 { get { return _unk92; } set { _unk92 = value; SignalPropertyChange(); } }
        //public float Unk120 { get { return _unk93; } set { _unk93 = value; SignalPropertyChange(); } }
        //public int Unk121 { get { return _unk94; } set { _unk94 = value; SignalPropertyChange(); } }
        //public float Unk122 { get { return _unk95; } set { _unk95 = value; SignalPropertyChange(); } }
        //public float Unk123 { get { return _unk96; } set { _unk96 = value; SignalPropertyChange(); } }
        //public float Unk124 { get { return _unk97; } set { _unk97 = value; SignalPropertyChange(); } }
        //public float Unk125 { get { return _unk98; } set { _unk98 = value; SignalPropertyChange(); } }
        //public float Unk126 { get { return _unk99; } set { _unk99 = value; SignalPropertyChange(); } }
        //public float Unk127 { get { return _unk100; } set { _unk100 = value; SignalPropertyChange(); } }
        //public float Unk128 { get { return _unk101; } set { _unk101 = value; SignalPropertyChange(); } }
        //public float Unk129 { get { return _unk102; } set { _unk102 = value; SignalPropertyChange(); } }
        //public float Unk130 { get { return _unk103; } set { _unk103 = value; SignalPropertyChange(); } }
        //public float Unk131 { get { return _unk104; } set { _unk104 = value; SignalPropertyChange(); } }
        //public float Unk132 { get { return _unk105; } set { _unk105 = value; SignalPropertyChange(); } }
        //public float Unk133 { get { return _unk106; } set { _unk106 = value; SignalPropertyChange(); } }
        //public float Unk134 { get { return _unk107; } set { _unk107 = value; SignalPropertyChange(); } }
        //public float Unk135 { get { return _unk108; } set { _unk108 = value; SignalPropertyChange(); } }
        //public float Unk136 { get { return _unk109; } set { _unk109 = value; SignalPropertyChange(); } }
        //public int Unk137 { get { return _unk110; } set { _unk110 = value; SignalPropertyChange(); } }
        //public float Unk138 { get { return _unk111; } set { _unk111 = value; SignalPropertyChange(); } }
        //public float Unk139 { get { return _unk112; } set { _unk112 = value; SignalPropertyChange(); } }
        //public float Unk140 { get { return _unk113; } set { _unk113 = value; SignalPropertyChange(); } }
        //public float Unk141 { get { return _unk114; } set { _unk114 = value; SignalPropertyChange(); } }
        //public int Unk142 { get { return _unk115; } set { _unk115 = value; SignalPropertyChange(); } }
        //public float Unk143 { get { return _unk116; } set { _unk116 = value; SignalPropertyChange(); } }
        //public float Unk144 { get { return _unk117; } set { _unk117 = value; SignalPropertyChange(); } }
        //public float Unk145 { get { return _unk118; } set { _unk118 = value; SignalPropertyChange(); } }
        //public float Unk146 { get { return _unk119; } set { _unk119 = value; SignalPropertyChange(); } }
        //public float Unk147 { get { return _unk120; } set { _unk120 = value; SignalPropertyChange(); } }
        //public float Unk148 { get { return _unk121; } set { _unk121 = value; SignalPropertyChange(); } }
        //public float Unk149 { get { return _unk122; } set { _unk122 = value; SignalPropertyChange(); } }
        //public float Unk150 { get { return _unk123; } set { _unk123 = value; SignalPropertyChange(); } }
        //public float Unk151 { get { return _unk124; } set { _unk124 = value; SignalPropertyChange(); } }
        //public float Unk152 { get { return _unk125; } set { _unk125 = value; SignalPropertyChange(); } }
        //public float Unk153 { get { return _unk126; } set { _unk126 = value; SignalPropertyChange(); } }
        //public float Unk154 { get { return _unk127; } set { _unk127 = value; SignalPropertyChange(); } }
        //public float Unk155 { get { return _unk128; } set { _unk128 = value; SignalPropertyChange(); } }
        //public float Unk156 { get { return _unk129; } set { _unk129 = value; SignalPropertyChange(); } }
        //public float Unk157 { get { return _unk130; } set { _unk130 = value; SignalPropertyChange(); } }
        //public float Unk158 { get { return _unk131; } set { _unk131 = value; SignalPropertyChange(); } }
        //public float Unk159 { get { return _unk132; } set { _unk132 = value; SignalPropertyChange(); } }
        //public float Unk160 { get { return _unk133; } set { _unk133 = value; SignalPropertyChange(); } }
        //public float Unk161 { get { return _unk134; } set { _unk134 = value; SignalPropertyChange(); } }
        //public float Unk162 { get { return _unk135; } set { _unk135 = value; SignalPropertyChange(); } }
        //public float Unk163 { get { return _unk136; } set { _unk136 = value; SignalPropertyChange(); } }
        //public int Unk164 { get { return _unk137; } set { _unk137 = value; SignalPropertyChange(); } }
        //public int Unk165 { get { return _unk138; } set { _unk138 = value; SignalPropertyChange(); } }
        //public int Unk166 { get { return _unk139; } set { _unk139 = value; SignalPropertyChange(); } }
        //public float Unk167 { get { return _unk140; } set { _unk140 = value; SignalPropertyChange(); } }
        //public int Unk168 { get { return _unk141; } set { _unk141 = value; SignalPropertyChange(); } }
        //public float Unk169 { get { return _unk142; } set { _unk142 = value; SignalPropertyChange(); } }
        //public float Unk170 { get { return _unk143; } set { _unk143 = value; SignalPropertyChange(); } }
        //public float Unk171 { get { return _unk144; } set { _unk144 = value; SignalPropertyChange(); } }
        //public float Unk172 { get { return _unk145; } set { _unk145 = value; SignalPropertyChange(); } }
        //public float Unk173 { get { return _unk146; } set { _unk146 = value; SignalPropertyChange(); } }
        //public float Unk174 { get { return _unk147; } set { _unk147 = value; SignalPropertyChange(); } }
        //public float Unk175 { get { return _unk148; } set { _unk148 = value; SignalPropertyChange(); } }
        //public float Unk176 { get { return _unk149; } set { _unk149 = value; SignalPropertyChange(); } }
        //public int Unk177 { get { return _unk150; } set { _unk150 = value; SignalPropertyChange(); } }
        //public int Unk178 { get { return _unk151; } set { _unk151 = value; SignalPropertyChange(); } }
        //public int Unk179 { get { return _unk152; } set { _unk152 = value; SignalPropertyChange(); } }
        //public int Unk180 { get { return _unk153; } set { _unk153 = value; SignalPropertyChange(); } }
        //public int Unk181 { get { return _unk154; } set { _unk154 = value; SignalPropertyChange(); } }
        //public int Unk182 { get { return _unk155; } set { _unk155 = value; SignalPropertyChange(); } }
        //public int Unk183 { get { return _unk156; } set { _unk156 = value; SignalPropertyChange(); } }
        //public int Unk184 { get { return _unk157; } set { _unk157 = value; SignalPropertyChange(); } }
        //public int Unk185 { get { return _unk158; } set { _unk158 = value; SignalPropertyChange(); } }

        //private float _walkInitVelocity;
        //private float _walkAcceleration;
        //private float _walkMaxVelocity;
        //private float _stopVelocity;
        //private float _dashInitVelocity;
        //private float _stopTurnDecel;
        //private float _stopTurnAccel;
        //private float _runInitVelocity;
        //private float _unk01;
        //private float _unk02;
        //private int _unk03;
        //private float _unk04;
        //private int _unk05;
        //private float _unk06;
        //private float _jumpYInitVelocity;
        //private float _unk07;
        //private float _jumpXInitVelocity;
        //private float _hopYInitVelocity;
        //private float _airJumpMultiplier;
        //private float _unk08;
        //private float _stoolYInitVelocity;
        //private float _unk09;
        //private float _unk10;
        //private float _unk11;
        //private int _unk12;
        //private float _gravity;
        //private float _termVelocity;
        //private float _unk13;
        //private float _unk14;
        //private float _airMobility;
        //private float _airStopMobility;
        //private float _airMaxXVelocity;
        //private float _unk15;
        //private float _unk16;
        //private float _unk17;
        //private int _unk18;
        //private float _unk19;
        //private float _unk20;
        //private float _unk21;
        //private int _unk22;
        //private int _unk23;
        //private int _unk24;
        //private float _unk25;
        //private float _unk26;
        //private float _weight;
        //private float _unk27;
        //private float _unk28;
        //private float _unk29;
        //private float _unk30;
        //private float _shieldSize;
        //private float _shieldBreakBounce;
        //private float _unk31;
        //private float _unk32;
        //private float _unk33;
        //private float _unk34;
        //private float _unk35;
        //private float _unk36;
        //private float _unk37;
        //private int _unk38;
        //private int _unk39;
        //private int _unk40;
        //private float _unk41;
        //private float _edgeJumpYVelocity;
        //private float _edgeJumpXVelocity;
        //private float _unk42;
        //private float _unk43;
        //private float _unk44;
        //private float _unk45;
        //private float _unk46;
        //private int _unk47;
        //private float _itemThrowStrength;
        //private float _unk48;
        //private float _unk49;
        //private float _unk50;
        //private float _fireMoveSpeed;
        //private float _fireFDashSpeed;
        //private float _fireBDashSpeed;
        //private float _unk51;
        //private float _unk52;
        //private float _unk53;
        //private float _unk54;
        //private float _unk55;
        //private float _unk56;
        //private float _unk57;
        //private float _unk58;
        //private int _unk59;
        //private int _unk60;
        //private float _unk61;
        //private float _unk62;
        //private float _wallJumpYVelocity;
        //private float _wallJumpXVelocity;
        //private float _unk63;
        //private float _unk64;
        //private int _unk65;
        //private float _unk66;
        //private float _unk67;
        //private int _unk68;
        //private int _unk69;
        //private float _unk70;
        //private float _unk71;
        //private float _unk72;
        //private float _unk73;
        //private float _unk74;
        //private float _unk75;
        //private float _unk76;
        //private float _unk77;
        //private int _unk78;
        //private float _unk79;
        //private int _unk80;
        //private float _unk81;
        //private float _unk82;
        //private float _unk83;
        //private float _unk84;
        //private float _unk85;
        //private float _unk86;
        //private int _unk89;
        //private float _unk90;
        //private float _unk91;
        //private float _unk92;
        //private float _unk93;
        //private int _unk94;
        //private float _unk95;
        //private float _unk96;
        //private float _unk97;
        //private float _unk98;
        //private float _unk99;
        //private float _unk100;
        //private float _unk101;
        //private float _unk102;
        //private float _unk103;
        //private float _unk104;
        //private float _unk105;
        //private float _unk106;
        //private float _unk107;
        //private float _unk108;
        //private float _unk109;
        //private int _unk110;
        //private float _unk111;
        //private float _unk112;
        //private float _unk113;
        //private float _unk114;
        //private int _unk115;
        //private float _unk116;
        //private float _unk117;
        //private float _unk118;
        //private float _unk119;
        //private float _unk120;
        //private float _unk121;
        //private float _unk122;
        //private float _unk123;
        //private float _unk124;
        //private float _unk125;
        //private float _unk126;
        //private float _unk127;
        //private float _unk128;
        //private float _unk129;
        //private float _unk130;
        //private float _unk131;
        //private float _unk132;
        //private float _unk133;
        //private float _unk134;
        //private float _unk135;
        //private float _unk136;
        //private int _unk137;
        //private int _unk138;
        //private int _unk139;
        //private float _unk140;
        //private int _unk141;
        //private float _unk142;
        //private float _unk143;
        //private float _unk144;
        //private float _unk145;
        //private float _unk146;
        //private float _unk147;
        //private float _unk148;
        //private float _unk149;
        //private int _unk150;
        //private int _unk151;
        //private int _unk152;
        //private int _unk153;
        //private int _unk154;
        //private int _unk155;
        //private int _unk156;
        //private int _unk157;
        //private int _unk158;

        #endregion

        #region OnInitRebuildCalc

        public override bool OnInitialize()
        {
            base.OnInitialize();
            attributeBuffer = new UnsafeBuffer(0x2E4);
            byte* pOut = (byte*) attributeBuffer.Address;
            byte* pIn = (byte*) Header;
            for (int i = 0; i < 0x2E4; i++)
            {
                *pOut++ = *pIn++;
            }

            //FDefAttributes* attributes = Header;

            //_walkInitVelocity = attributes->_walkInitVelocity;
            //_walkAcceleration = attributes->_walkAcceleration;
            //_walkMaxVelocity = attributes->_walkMaxVelocity;
            //_stopVelocity = attributes->_stopVelocity;
            //_dashInitVelocity = attributes->_dashInitVelocity;
            //_stopTurnDecel = attributes->_stopTurnDecel;
            //_stopTurnAccel = attributes->_stopTurnAccel;
            //_runInitVelocity = attributes->_runInitVelocity;
            //_unk01 = attributes->_unk01;
            //_unk02 = attributes->_unk02;
            //_unk03 = attributes->_unk03;
            //_unk04 = attributes->_unk04;
            //_unk05 = attributes->_unk05;
            //_unk06 = attributes->_unk06;
            //_jumpYInitVelocity = attributes->_jumpYInitVelocity;
            //_unk07 = attributes->_unk07;
            //_jumpXInitVelocity = attributes->_jumpXInitVelocity;
            //_hopYInitVelocity = attributes->_hopYInitVelocity;
            //_airJumpMultiplier = attributes->_airJumpMultiplier;
            //_unk08 = attributes->_unk08;
            //_stoolYInitVelocity = attributes->_stoolYInitVelocity;
            //_unk09 = attributes->_unk09;
            //_unk10 = attributes->_unk10;
            //_unk11 = attributes->_unk11;
            //_unk12 = attributes->_unk12;
            //_gravity = attributes->_gravity;
            //_termVelocity = attributes->_termVelocity;
            //_unk13 = attributes->_unk13;
            //_unk14 = attributes->_unk14;
            //_airMobility = attributes->_airMobility;
            //_airStopMobility = attributes->_airStopMobility;
            //_airMaxXVelocity = attributes->_airMaxXVelocity;
            //_unk15 = attributes->_unk15;
            //_unk16 = attributes->_unk16;
            //_unk17 = attributes->_unk17;
            //_unk18 = attributes->_unk18;
            //_unk19 = attributes->_unk19;
            //_unk20 = attributes->_unk20;
            //_unk21 = attributes->_unk21;
            //_unk22 = attributes->_unk22;
            //_unk23 = attributes->_unk23;
            //_unk24 = attributes->_unk24;
            //_unk25 = attributes->_unk25;
            //_unk26 = attributes->_unk26;
            //_weight = attributes->_weight;
            //_unk27 = attributes->_unk27;
            //_unk28 = attributes->_unk28;
            //_unk29 = attributes->_unk29;
            //_unk30 = attributes->_unk30;
            //_shieldSize = attributes->_shieldSize;
            //_shieldBreakBounce = attributes->_shieldBreakBounce;
            //_unk31 = attributes->_unk31;
            //_unk32 = attributes->_unk32;
            //_unk33 = attributes->_unk33;
            //_unk34 = attributes->_unk34;
            //_unk35 = attributes->_unk35;
            //_unk36 = attributes->_unk36;
            //_unk37 = attributes->_unk37;
            //_unk38 = attributes->_unk38;
            //_unk39 = attributes->_unk39;
            //_unk40 = attributes->_unk40;
            //_unk41 = attributes->_unk41;
            //_edgeJumpYVelocity = attributes->_edgeJumpYVelocity;
            //_edgeJumpXVelocity = attributes->_edgeJumpXVelocity;
            //_unk42 = attributes->_unk42;
            //_unk43 = attributes->_unk43;
            //_unk44 = attributes->_unk44;
            //_unk45 = attributes->_unk45;
            //_unk46 = attributes->_unk46;
            //_unk47 = attributes->_unk47;
            //_itemThrowStrength = attributes->_itemThrowStrength;
            //_unk48 = attributes->_unk48;
            //_unk49 = attributes->_unk49;
            //_unk50 = attributes->_unk50;
            //_fireMoveSpeed = attributes->_fireMoveSpeed;
            //_fireFDashSpeed = attributes->_fireFDashSpeed;
            //_fireBDashSpeed = attributes->_fireBDashSpeed;
            //_unk51 = attributes->_unk51;
            //_unk52 = attributes->_unk52;
            //_unk53 = attributes->_unk53;
            //_unk54 = attributes->_unk54;
            //_unk55 = attributes->_unk55;
            //_unk56 = attributes->_unk56;
            //_unk57 = attributes->_unk57;
            //_unk58 = attributes->_unk58;
            //_unk59 = attributes->_unk59;
            //_unk60 = attributes->_unk60;
            //_unk61 = attributes->_unk61;
            //_unk62 = attributes->_unk62;
            //_wallJumpYVelocity = attributes->_wallJumpYVelocity;
            //_wallJumpXVelocity = attributes->_wallJumpXVelocity;
            //_unk63 = attributes->_unk63;
            //_unk64 = attributes->_unk64;
            //_unk65 = attributes->_unk65;
            //_unk66 = attributes->_unk66;
            //_unk67 = attributes->_unk67;
            //_unk68 = attributes->_unk68;
            //_unk69 = attributes->_unk69;
            //_unk70 = attributes->_unk70;
            //_unk71 = attributes->_unk71;
            //_unk72 = attributes->_unk72;
            //_unk73 = attributes->_unk73;
            //_unk74 = attributes->_unk74;
            //_unk75 = attributes->_unk75;
            //_unk76 = attributes->_unk76;
            //_unk77 = attributes->_unk77;
            //_unk78 = attributes->_unk78;
            //_unk79 = attributes->_unk79;
            //_unk80 = attributes->_unk80;
            //_unk81 = attributes->_unk81;
            //_unk82 = attributes->_unk82;
            //_unk83 = attributes->_unk83;
            //_unk84 = attributes->_unk84;
            //_unk85 = attributes->_unk85;
            //_unk86 = attributes->_unk86;

            //_unk89 = attributes->_unk89;
            //_unk90 = attributes->_unk90;
            //_unk91 = attributes->_unk91;
            //_unk92 = attributes->_unk92;
            //_unk93 = attributes->_unk93;
            //_unk94 = attributes->_unk94;
            //_unk95 = attributes->_unk95;
            //_unk96 = attributes->_unk96;
            //_unk97 = attributes->_unk97;
            //_unk98 = attributes->_unk98;
            //_unk99 = attributes->_unk99;
            //_unk100 = attributes->_unk100;
            //_unk101 = attributes->_unk101;
            //_unk102 = attributes->_unk102;
            //_unk103 = attributes->_unk103;
            //_unk104 = attributes->_unk104;
            //_unk105 = attributes->_unk105;
            //_unk106 = attributes->_unk106;
            //_unk107 = attributes->_unk107;
            //_unk108 = attributes->_unk108;
            //_unk109 = attributes->_unk109;
            //_unk110 = attributes->_unk110;
            //_unk111 = attributes->_unk111;
            //_unk112 = attributes->_unk112;
            //_unk113 = attributes->_unk113;
            //_unk114 = attributes->_unk114;
            //_unk115 = attributes->_unk115;
            //_unk116 = attributes->_unk116;
            //_unk117 = attributes->_unk117;
            //_unk118 = attributes->_unk118;
            //_unk119 = attributes->_unk119;
            //_unk120 = attributes->_unk120;
            //_unk121 = attributes->_unk121;
            //_unk122 = attributes->_unk122;
            //_unk123 = attributes->_unk123;
            //_unk124 = attributes->_unk124;
            //_unk125 = attributes->_unk125;
            //_unk126 = attributes->_unk126;
            //_unk127 = attributes->_unk127;
            //_unk128 = attributes->_unk128;
            //_unk129 = attributes->_unk129;
            //_unk130 = attributes->_unk130;
            //_unk131 = attributes->_unk131;
            //_unk132 = attributes->_unk132;
            //_unk133 = attributes->_unk133;
            //_unk134 = attributes->_unk134;
            //_unk135 = attributes->_unk135;
            //_unk136 = attributes->_unk136;
            //_unk137 = attributes->_unk137;
            //_unk138 = attributes->_unk138;
            //_unk139 = attributes->_unk139;
            //_unk140 = attributes->_unk140;
            //_unk141 = attributes->_unk141;
            //_unk142 = attributes->_unk142;
            //_unk143 = attributes->_unk143;
            //_unk144 = attributes->_unk144;
            //_unk145 = attributes->_unk145;
            //_unk146 = attributes->_unk146;
            //_unk147 = attributes->_unk147;
            //_unk148 = attributes->_unk148;
            //_unk149 = attributes->_unk149;
            //_unk150 = attributes->_unk150;
            //_unk151 = attributes->_unk151;
            //_unk152 = attributes->_unk152;
            //_unk153 = attributes->_unk153;
            //_unk154 = attributes->_unk154;
            //_unk155 = attributes->_unk155;
            //_unk156 = attributes->_unk156;
            //_unk157 = attributes->_unk157;
            //_unk158 = attributes->_unk158;

            return false;
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            _entryOffset = address;
            byte* pIn = (byte*) attributeBuffer.Address;
            byte* pOut = (byte*) address;
            for (int i = 0; i < 0x2E4; i++)
            {
                *pOut++ = *pIn++;
            }

            //FDefAttributes* attributes = (FDefAttributes*)address;

            //attributes->_walkInitVelocity = _walkInitVelocity;
            //attributes->_walkAcceleration = _walkAcceleration;
            //attributes->_walkMaxVelocity = _walkMaxVelocity;
            //attributes->_stopVelocity = _stopVelocity;
            //attributes->_dashInitVelocity = _dashInitVelocity;
            //attributes->_stopTurnDecel = _stopTurnDecel;
            //attributes->_stopTurnAccel = _stopTurnAccel;
            //attributes->_runInitVelocity = _runInitVelocity;
            //attributes->_unk01 = _unk01;
            //attributes->_unk02 = _unk02;
            //attributes->_unk03 = _unk03;
            //attributes->_unk04 = _unk04;
            //attributes->_unk05 = _unk05;
            //attributes->_unk06 = _unk06;
            //attributes->_jumpYInitVelocity = _jumpYInitVelocity;
            //attributes->_unk07 = _unk07;
            //attributes->_jumpXInitVelocity = _jumpXInitVelocity;
            //attributes->_hopYInitVelocity = _hopYInitVelocity;
            //attributes->_airJumpMultiplier = _airJumpMultiplier;
            //attributes->_unk08 = _unk08;
            //attributes->_stoolYInitVelocity = _stoolYInitVelocity;
            //attributes->_unk09 = _unk09;
            //attributes->_unk10 = _unk10;
            //attributes->_unk11 = _unk11;
            //attributes->_unk12 = _unk12;
            //attributes->_gravity = _gravity;
            //attributes->_termVelocity = _termVelocity;
            //attributes->_unk13 = _unk13;
            //attributes->_unk14 = _unk14;
            //attributes->_airMobility = _airMobility;
            //attributes->_airStopMobility = _airStopMobility;
            //attributes->_airMaxXVelocity = _airMaxXVelocity;
            //attributes->_unk15 = _unk15;
            //attributes->_unk16 = _unk16;
            //attributes->_unk17 = _unk17;
            //attributes->_unk18 = _unk18;
            //attributes->_unk19 = _unk19;
            //attributes->_unk20 = _unk20;
            //attributes->_unk21 = _unk21;
            //attributes->_unk22 = _unk22;
            //attributes->_unk23 = _unk23;
            //attributes->_unk24 = _unk24;
            //attributes->_unk25 = _unk25;
            //attributes->_unk26 = _unk26;
            //attributes->_weight = _weight;
            //attributes->_unk27 = _unk27;
            //attributes->_unk28 = _unk28;
            //attributes->_unk29 = _unk29;
            //attributes->_unk30 = _unk30;
            //attributes->_shieldSize = _shieldSize;
            //attributes->_shieldBreakBounce = _shieldBreakBounce;
            //attributes->_unk31 = _unk31;
            //attributes->_unk32 = _unk32;
            //attributes->_unk33 = _unk33;
            //attributes->_unk34 = _unk34;
            //attributes->_unk35 = _unk35;
            //attributes->_unk36 = _unk36;
            //attributes->_unk37 = _unk37;
            //attributes->_unk38 = _unk38;
            //attributes->_unk39 = _unk39;
            //attributes->_unk40 = _unk40;
            //attributes->_unk41 = _unk41;
            //attributes->_edgeJumpYVelocity = _edgeJumpYVelocity;
            //attributes->_edgeJumpXVelocity = _edgeJumpXVelocity;
            //attributes->_unk42 = _unk42;
            //attributes->_unk43 = _unk43;
            //attributes->_unk44 = _unk44;
            //attributes->_unk45 = _unk45;
            //attributes->_unk46 = _unk46;
            //attributes->_unk47 = _unk47;
            //attributes->_itemThrowStrength = _itemThrowStrength;
            //attributes->_unk48 = _unk48;
            //attributes->_unk49 = _unk49;
            //attributes->_unk50 = _unk50;
            //attributes->_fireMoveSpeed = _fireMoveSpeed;
            //attributes->_fireFDashSpeed = _fireFDashSpeed;
            //attributes->_fireBDashSpeed = _fireBDashSpeed;
            //attributes->_unk51 = _unk51;
            //attributes->_unk52 = _unk52;
            //attributes->_unk53 = _unk53;
            //attributes->_unk54 = _unk54;
            //attributes->_unk55 = _unk55;
            //attributes->_unk56 = _unk56;
            //attributes->_unk57 = _unk57;
            //attributes->_unk58 = _unk58;
            //attributes->_unk59 = _unk59;
            //attributes->_unk60 = _unk60;
            //attributes->_unk61 = _unk61;
            //attributes->_unk62 = _unk62;
            //attributes->_wallJumpYVelocity = _wallJumpYVelocity;
            //attributes->_wallJumpXVelocity = _wallJumpXVelocity;
            //attributes->_unk63 = _unk63;
            //attributes->_unk64 = _unk64;
            //attributes->_unk65 = _unk65;
            //attributes->_unk66 = _unk66;
            //attributes->_unk67 = _unk67;
            //attributes->_unk68 = _unk68;
            //attributes->_unk69 = _unk69;
            //attributes->_unk70 = _unk70;
            //attributes->_unk71 = _unk71;
            //attributes->_unk72 = _unk72;
            //attributes->_unk73 = _unk73;
            //attributes->_unk74 = _unk74;
            //attributes->_unk75 = _unk75;
            //attributes->_unk76 = _unk76;
            //attributes->_unk77 = _unk77;
            //attributes->_unk78 = _unk78;
            //attributes->_unk79 = _unk79;
            //attributes->_unk80 = _unk80;
            //attributes->_unk81 = _unk81;
            //attributes->_unk82 = _unk82;
            //attributes->_unk83 = _unk83;
            //attributes->_unk84 = _unk84;
            //attributes->_unk85 = _unk85;
            //attributes->_unk86 = _unk86;

            //attributes->_unk89 = _unk89;
            //attributes->_unk90 = _unk90;
            //attributes->_unk91 = _unk91;
            //attributes->_unk92 = _unk92;
            //attributes->_unk93 = _unk93;
            //attributes->_unk94 = _unk94;
            //attributes->_unk95 = _unk95;
            //attributes->_unk96 = _unk96;
            //attributes->_unk97 = _unk97;
            //attributes->_unk98 = _unk98;
            //attributes->_unk99 = _unk99;
            //attributes->_unk100 = _unk100;
            //attributes->_unk101 = _unk101;
            //attributes->_unk102 = _unk102;
            //attributes->_unk103 = _unk103;
            //attributes->_unk104 = _unk104;
            //attributes->_unk105 = _unk105;
            //attributes->_unk106 = _unk106;
            //attributes->_unk107 = _unk107;
            //attributes->_unk108 = _unk108;
            //attributes->_unk109 = _unk109;
            //attributes->_unk110 = _unk110;
            //attributes->_unk111 = _unk111;
            //attributes->_unk112 = _unk112;
            //attributes->_unk113 = _unk113;
            //attributes->_unk114 = _unk114;
            //attributes->_unk115 = _unk115;
            //attributes->_unk116 = _unk116;
            //attributes->_unk117 = _unk117;
            //attributes->_unk118 = _unk118;
            //attributes->_unk119 = _unk119;
            //attributes->_unk120 = _unk120;
            //attributes->_unk121 = _unk121;
            //attributes->_unk122 = _unk122;
            //attributes->_unk123 = _unk123;
            //attributes->_unk124 = _unk124;
            //attributes->_unk125 = _unk125;
            //attributes->_unk126 = _unk126;
            //attributes->_unk127 = _unk127;
            //attributes->_unk128 = _unk128;
            //attributes->_unk129 = _unk129;
            //attributes->_unk130 = _unk130;
            //attributes->_unk131 = _unk131;
            //attributes->_unk132 = _unk132;
            //attributes->_unk133 = _unk133;
            //attributes->_unk134 = _unk134;
            //attributes->_unk135 = _unk135;
            //attributes->_unk136 = _unk136;
            //attributes->_unk137 = _unk137;
            //attributes->_unk138 = _unk138;
            //attributes->_unk139 = _unk139;
            //attributes->_unk140 = _unk140;
            //attributes->_unk141 = _unk141;
            //attributes->_unk142 = _unk142;
            //attributes->_unk143 = _unk143;
            //attributes->_unk144 = _unk144;
            //attributes->_unk145 = _unk145;
            //attributes->_unk146 = _unk146;
            //attributes->_unk147 = _unk147;
            //attributes->_unk148 = _unk148;
            //attributes->_unk149 = _unk149;
            //attributes->_unk150 = _unk150;
            //attributes->_unk151 = _unk151;
            //attributes->_unk152 = _unk152;
            //attributes->_unk153 = _unk153;
            //attributes->_unk154 = _unk154;
            //attributes->_unk155 = _unk155;
            //attributes->_unk156 = _unk156;
            //attributes->_unk157 = _unk157;
            //attributes->_unk158 = _unk158;
        }

        public override int OnCalculateSize(bool force)
        {
            _lookupCount = 0;
            return 0x2E4;
        }

        #endregion
    }
}