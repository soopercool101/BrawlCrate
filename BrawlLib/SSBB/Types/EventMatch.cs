using System;
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
        public bint _flags10; // 0x20000000: timer visible; 0x40000000: hide countdown
        public bfloat _unknown14;
        public byte _flags18; // 0x80: hide damage values
        public byte _isTeamGame;
        public bshort _itemLevel; // 0 = off, 1 = low, 2 = medium, 3 = high, 4 = raining
        public byte _unknown1c;
        public byte _unknown1d;
        public bushort _stageID;
        public bint _flags20; // 0x00?00000 --> ?/2 is the number of players on the screen
        public bint _unknown24;
        public bint _unknown28;
        public bint _unknown2c;
        public bint _unknown30;
        public bint _unknown34;
        public bfloat _gameSpeed;
        public bfloat _cameraShakeControl;
        public bint _flags40;
        public bint _songID;
        public bshort _globalOffenseRatio;
        public bshort _globalDefenseRatio;
        public bint _unknown4c;

        private VoidPtr Address { get { fixed (void* ptr = &this)return ptr; } }

        public EventMatchFighterData* FighterDataPtr
        {
            get
            {
                fixed (EventMatchTblHeader* ptr = &this)
                {
                    return (EventMatchFighterData*)(ptr + 1);
                }
            }
        }
    }

    public unsafe struct EventMatchFighterData
    {
        public const int Size = 0x36;

        public EventMatchFighterHeader _header;
        public EventMatchDifficultyData _easy;
        public EventMatchDifficultyData _normal;
        public EventMatchDifficultyData _hard;
    }

    public unsafe struct EventMatchFighterHeader {
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

    public unsafe struct EventMatchDifficultyData
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
