using System.Runtime.InteropServices;
using BrawlLib.Internal;
using static BrawlLib.SSBB.Types.ProjectPlus.ENOV;

namespace BrawlLib.SSBB.Types.ProjectPlus
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct ENOV
    {
        public enum ArchiveOverrideSetting : uint
        {
            None = 0x0,
            Brres = 0x1,
            Param = 0x2,
            Both = 0x3,
        }

        public const int Size = 0x59;
        public static readonly string Tag = "ENOV";
        public buint tag;
        public fixed byte _enmOverrideFolder[12];
        public fixed byte _stageItemFolder[12];

        /*
            char _ : 4
            bool m_useIndividualFolders : 1; // for Primids/Glire/Gamygabase
            bool m_overrideModuleCommon : 1;
            ArchiveOverrideSetting m_overrideCommon : 2;
        */
        public byte _common;

        public bool UseIndividualFolders
        {
            get => _common.GetBit(3);
            set => _common.SetBit(3, value);
        }

        public bool OverrideModuleCommon
        {
            get => _common.GetBit(2);
            set => _common.SetBit(2, value);
        }

        public ArchiveOverrideSetting OverrideSettingsCommon
        {
            get => (ArchiveOverrideSetting)(_common & 0b11);
            set
            {
                _common &= 0b11111100;
                _common |= (byte)value;
            }
        }
        public OverriddenEnemy _goomba;
        public OverriddenEnemy _poppant;
        public OverriddenEnemy _feyesh;
        public OverriddenEnemy _jyk;
        public OverriddenEnemy _auroros;
        public OverriddenEnemy _cymul;
        public OverriddenEnemy _roturret;
        public OverriddenEnemy _borboras;
        public OverriddenEnemy _giantGoomba;
        public OverriddenEnemy _buckot;
        public OverriddenEnemy _bucculus;
        public OverriddenEnemy _greap;
        public OverriddenEnemy _armight;
        public OverriddenEnemy _bulletBill;
        public OverriddenEnemy _roader;
        public OverriddenEnemy _spaak;
        public OverriddenEnemy _mite;
        public OverriddenEnemy _ticken;
        public OverriddenEnemy _towtow;
        public OverriddenEnemy _hammerBro;
        public OverriddenEnemy _bytan;
        public OverriddenEnemy _floow;
        public OverriddenEnemy _puppit;
        public OverriddenEnemy _primid;
        public OverriddenEnemy _shellpod;
        public OverriddenEnemy _koopa;
        public OverriddenEnemy _shaydas;
        public OverriddenEnemy _bombed;
        public OverriddenEnemy _metalPrimid;
        public OverriddenEnemy _nagagog;
        public OverriddenEnemy _trowlon;
        public OverriddenEnemy _bigPrimid;
        public OverriddenEnemy _boomPrimid;
        public OverriddenEnemy _firePrimid;
        public OverriddenEnemy _scopePrimid;
        public OverriddenEnemy _swordPrimid;
        public OverriddenEnemy _gamyga;
        public OverriddenEnemy _rOBBlasterStationary;
        public OverriddenEnemy _rOBBlasterMobile;
        public OverriddenEnemy _rOBLauncher;
        public OverriddenEnemy _rOBSentry;
        public OverriddenEnemy _autolance;
        public OverriddenEnemy _armank;
        public OverriddenEnemy _glire;
        public OverriddenEnemy _glice;
        public OverriddenEnemy _glunder;
        public OverriddenEnemy _peteyPiranha;
        public OverriddenEnemy _gamygaBase1;
        public OverriddenEnemy _gamygaBase2;
        public OverriddenEnemy _gamygaBase3;
        public OverriddenEnemy _gamygaBase4;
        public OverriddenEnemy _galleom;
        public OverriddenEnemy _ridley;
        public OverriddenEnemy _rayquaza;
        public OverriddenEnemy _duon;
        public OverriddenEnemy _porky;
        public OverriddenEnemy _metaRidley;
        public OverriddenEnemy _falconFlyer;
        public OverriddenEnemy _tabuu;
        public OverriddenEnemy _masterHand;
        public OverriddenEnemy _crazyHand;

    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct OverriddenEnemy
    {
        public byte _data;

        public byte FaceIndex
        {
            get => (byte)((_data & 0b11111000) >> 3);
            set
            {
                _data &= 0b111;
                _data |= (byte)(value.Clamp(0, 0b11111) << 3);
            }
        }

        public bool OverrideModule
        {
            get => _data.GetBit(2);
            set => _data.SetBit(2, value);
        }

        public ArchiveOverrideSetting OverrideSetting
        {
            get => (ArchiveOverrideSetting)(_data & 0b11);
            set
            {
                _data &= 0b11111100;
                _data |= (byte)value;
            }
        }

        public override string ToString()
        {
            return $"{FaceIndex} {OverrideModule} {OverrideSetting}";
        }
    }
}