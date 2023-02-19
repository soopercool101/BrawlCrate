using BrawlLib.Internal;
using BrawlLib.SSBB.Types.Subspace.Triggers;

namespace BrawlLib.SSBB.Types.Subspace.Objects
{
    public unsafe struct GMPSEntry
    {
        public const int Size = 0x120;

        public MotionPathData _motionPathData;  // 0x000
        public MotionPathData _sliderPathData;  // 0x008
        public HitData _hitData;                // 0x010
        public buint _unknown0x030;             // 0x030
        public AttackData _attackData;          // 0x034
        public buint _unknown0x08C;
        public buint _unknown0x090;
        public bfloat _unknown0x094;
        public bfloat _unknown0x098;
        public bfloat _unknown0x09C;
        public bfloat _unknown0x0A0;
        public bfloat _unknown0x0A4;
        public bfloat _unknown0x0A8;
        public bfloat _difficultyMotionRatio1;  // 0x0AC
        public bfloat _difficultyMotionRatio2;  // 0x0B0
        public bfloat _difficultyMotionRatio3;  // 0x0B4
        public bfloat _difficultyMotionRatio4;  // 0x0B8
        public bfloat _difficultyMotionRatio5;  // 0x0BC
        public bfloat _difficultyMotionRatio6;  // 0x0C0
        public bfloat _difficultyMotionRatio7;  // 0x0C4
        public bfloat _difficultyMotionRatio8;  // 0x0C8
        public bfloat _difficultyMotionRatio9;  // 0x0CC
        public bfloat _difficultyMotionRatio10; // 0x0D0
        public bfloat _difficultyMotionRatio11; // 0x0D4
        public bfloat _difficultyMotionRatio12; // 0x0D8
        public bfloat _difficultyMotionRatio13; // 0x0DC
        public bfloat _difficultyMotionRatio14; // 0x0E0
        public bfloat _difficultyMotionRatio15; // 0x0E4
        public TriggerData _triggerData;        // 0x0E8
        public uint _unknown0x0EC;
        public uint _unknown0x0F0;
        public uint _unknown0x0F4;
        public byte _modelIndex;                // 0x0F8
        public byte _unknown0x0F9;
        public byte _unknown0x0FA;
        public byte _unknown0x0FB;
        public fixed byte _boneName[0x20];      // 0x0FC
        public buint _unknown0x11C;

        public string BoneName
        {
            get
            {
                string s = "";
                for(int i = 0; i < 0x20; i++)
                {
                    char c = (char)_boneName[i];
                    if (c == '\0')
                        break;
                    s += c;
                }
                return s;
            }
            set
            {
                for (int i = 0; i < 0x20; i++)
                {
                    if (value.Length > i)
                    {
                        _boneName[i] = (byte)value[i];
                    }
                    else
                    {
                        _boneName[i] = 0;
                    }
                }
            }
        }
    }
}