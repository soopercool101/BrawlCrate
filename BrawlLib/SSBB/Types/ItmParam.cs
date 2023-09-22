using BrawlLib.Internal;
using System.Runtime.InteropServices;

namespace BrawlLib.SSBB.Types
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct ItmParamEntry
    {
        public const int Size = 0xF0;

        public bfloat _hurtboxSize;
        public bfloat _throwSpeedMultiplier;
        public bfloat _unknown0x08;
        public bfloat _throwSpinRateX;
        public bfloat _throwSpinRateY;
        public bfloat _throwSpinRateZ;
        public bfloat _unknown0x18;
        public bfloat _gravity;
        public bfloat _terminalVelocity;
        public bfloat _unknown0x24;
        public bfloat _characterColliderY1;
        public bfloat _characterColliderY2;
        public bfloat _characterColliderX1;
        public bfloat _characterColliderX2;
        public bfloat _grabRangeOffsetX;
        public bfloat _grabRangeOffsetY;
        public bfloat _grabRangeDistanceX;
        public bfloat _grabRangeDistanceY;
        public bfloat _ecbHeight;
        public bfloat _ecbOffsetY;
        public bfloat _ecbWidth;
        public bfloat _unknown0x54;
        public bfloat _unknown0x58;
        public bfloat _slideAngle;
        public bfloat _slideGravity;
        public bfloat _floorBounceSpeedMultiplierX;
        public bfloat _floorBounceSpeedMultiplierY;
        public bfloat _unknown0x6C;
        public bfloat _wallBounceSpeedMultiplierX;
        public bfloat _wallBounceSpeedMultiplierY;
        public bfloat _ceilingBounceSpeedMultiplierX;
        public bfloat _ceilingBounceSpeedMultiplierY;
        public bfloat _fighterBounceSpeedMultiplierX;
        public bfloat _fighterBounceSpeedMultiplierY;
        public bfloat _knockbackAirFriction;
        public bfloat _knockbackGroundFriction;
        public bfloat _unknown0x90;
        public bfloat _unknown0x94;
        public bfloat _baseDamageMultiplier;
        public bfloat _speedDamageMultiplier;
        public bfloat _itemScale;
        public bfloat _itemHealth;
        public bint _unknown0xA8;
        public bool32 _isHeavy;
        public bint _pickupType;
        public Bin8 _flags0xB4;
        public Bin8 _flags0xB5;
        public Bin8 _flags0xB6;
        public Bin8 _flags0xB7;
        public buint _unknown0xB8;
        public bool32 _unknown0xBC;
        public bint _unknown0xC0;
        public bool32 _unknown0xC4;
        public bint _unknown0xC8;
        public bint _unknown0xCC;
        public bint _unknown0xD0;
        public bint _unknown0xD4;
        public bint _unknown0xD8;
        public bint _unknown0xDC;
        public bint _unknown0xE0;
        public bint _unknown0xE4;
        public bint _unknown0xE8;
        public bint _unknown0xEC;
    }
}
