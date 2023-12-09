using BrawlLib.Internal;
using System.Runtime.InteropServices;

namespace BrawlLib.SSBB.Types
{
    /// <summary>
    /// See: http://opensa.dantarion.com/wiki/Event_Match_Files
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct EventMatchTblHeader
    {
        public const int Size = 0x50;

        public bint _eventExtension; // 0 = 4 entries, 1 = 9 entries, 2 = 38 entries
        public bint _unknown04;
        public byte _matchType; // 0 = time, 1 = stock, 2 = coin
        public byte _unknown09;
        public byte _unknown0a;
        public byte _unknown0b;
        public bint _timeLimit; // if 0, time counts up
        public bint _flags10;   // 0x20000000: timer visible; 0x40000000: hide countdown
        public bfloat _unknown14;
        public byte _flags18; // 0x80: hide damage values
        public byte _isTeamGame;
        public bshort _itemLevel; // 0 = off, 1 = low, 2 = medium, 3 = high, 4 = raining
        public byte _unknown1c;
        public byte _unknown1d;
        public bushort _stageID;
        public Bin8 _flags20;
        public byte _flags21; // 0x?0 --> ?/2 is the number of players on the screen
        public bushort _stageExASL;
        public bint _unknown24;
        public Bin8 _itemExFlags0x28;
        public Bin8 _itemExFlags0x29;
        public Bin8 _itemExFlags0x2A;
        public Bin8 _itemExFlags0x2B;
        public Bin8 _itemExFlags0x2C;
        public Bin8 _itemExFlags0x2D;
        public Bin8 _itemExFlags0x2E;
        public Bin8 _itemExFlags0x2F;
        public Bin8 _itemExFlags0x30;
        public Bin8 _itemExFlags0x31;
        public Bin8 _itemExFlags0x32;
        public Bin8 _itemExFlags0x33;
        public Bin8 _itemExFlags0x34;
        public Bin8 _itemExFlags0x35;
        public Bin8 _itemExFlags0x36;
        public Bin8 _itemExFlags0x37;
        public bfloat _gameSpeed;
        public bfloat _cameraShakeControl;
        public bint _flags40;
        public bint _songID;
        public bshort _globalOffenseRatio;
        public bshort _globalDefenseRatio;
        public bint _unknown4c;

        private VoidPtr Address
        {
            get
            {
                fixed (void* ptr = &this)
                {
                    return ptr;
                }
            }
        }

        public EventMatchFighterData* FighterDataPtr
        {
            get
            {
                fixed (EventMatchTblHeader* ptr = &this)
                {
                    return (EventMatchFighterData*) (ptr + 1);
                }
            }
        }
    }

    public struct EventMatchFighterData
    {
        public const int Size = 0x36;

        public EventMatchFighterHeader _header;
        public EventMatchDifficultyData _easy;
        public EventMatchDifficultyData _normal;
        public EventMatchDifficultyData _hard;
    }

    public struct EventMatchFighterHeader
    {
        public const int Size = 0x0C;

        public byte _fighterID;
        public byte _status; // normal, metal, invisible
        public byte _unknown02;
        public byte _unknown03;
        public bfloat _scale;
        public byte _team;
        public byte _unknown09;
        public byte _unknown0a;
        public byte _unknown0b;
    }

    public struct EventMatchDifficultyData
    {
        public const int Size = 0x0E;

        public byte _cpuLevel;
        public byte _unknown01;
        public bushort _offenseRatio;
        public bushort _defenseRatio;
        public byte _aiType;
        public byte _costume;
        public byte _stockCount;
        public byte _unknown09;
        public bshort _initialHitPoints;
        public bshort _startingDamage;
    }
}