using BrawlLib.Internal;
using System.Runtime.InteropServices;

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

        public ArchiveOverrideSetting OverrideCommon
        {
            get => (ArchiveOverrideSetting)(_common & 0b11);
            set
            {
                _common &= 0b11111100;
                _common |= (byte)value;
            }
        }

        public byte _goomba;

        public byte FaceIndexGoomba
        {
            get => (byte)((_goomba & 0b11111000) >> 3);
            set
            {
                _goomba &= 0b111;
                _goomba |= (byte)(value.Clamp(0, 0b11111) << 3);
            }
        }

        public bool OverrideModuleGoomba
        {
            get => _goomba.GetBit(2);
            set => _goomba.SetBit(2, value);
        }

        public ArchiveOverrideSetting OverrideGoomba
        {
            get => (ArchiveOverrideSetting)(_goomba & 0b11);
            set
            {
                _goomba &= 0b11111100;
                _goomba |= (byte)value;
            }
        }

        public byte _poppant;

        public byte FaceIndexPoppant
        {
            get => (byte)((_poppant & 0b11111000) >> 3);
            set
            {
                _poppant &= 0b111;
                _poppant |= (byte)(value.Clamp(0, 0b11111) << 3);
            }
        }

        public bool OverrideModulePoppant
        {
            get => _poppant.GetBit(2);
            set => _poppant.SetBit(2, value);
        }

        public ArchiveOverrideSetting OverridePoppant
        {
            get => (ArchiveOverrideSetting)(_poppant & 0b11);
            set
            {
                _poppant &= 0b11111100;
                _poppant |= (byte)value;
            }
        }

        public byte _feyesh;

        public byte FaceIndexFeyesh
        {
            get => (byte)((_feyesh & 0b11111000) >> 3);
            set
            {
                _feyesh &= 0b111;
                _feyesh |= (byte)(value.Clamp(0, 0b11111) << 3);
            }
        }

        public bool OverrideModuleFeyesh
        {
            get => _feyesh.GetBit(2);
            set => _feyesh.SetBit(2, value);
        }

        public ArchiveOverrideSetting OverrideFeyesh
        {
            get => (ArchiveOverrideSetting)(_feyesh & 0b11);
            set
            {
                _feyesh &= 0b11111100;
                _feyesh |= (byte)value;
            }
        }

        public byte _jyk;

        public byte FaceIndexJyk
        {
            get => (byte)((_jyk & 0b11111000) >> 3);
            set
            {
                _jyk &= 0b111;
                _jyk |= (byte)(value.Clamp(0, 0b11111) << 3);
            }
        }

        public bool OverrideModuleJyk
        {
            get => _jyk.GetBit(2);
            set => _jyk.SetBit(2, value);
        }

        public ArchiveOverrideSetting OverrideJyk
        {
            get => (ArchiveOverrideSetting)(_jyk & 0b11);
            set
            {
                _jyk &= 0b11111100;
                _jyk |= (byte)value;
            }
        }

        public byte _auroros;

        public byte FaceIndexAuroros
        {
            get => (byte)((_auroros & 0b11111000) >> 3);
            set
            {
                _auroros &= 0b111;
                _auroros |= (byte)(value.Clamp(0, 0b11111) << 3);
            }
        }

        public bool OverrideModuleAuroros
        {
            get => _auroros.GetBit(2);
            set => _auroros.SetBit(2, value);
        }

        public ArchiveOverrideSetting OverrideAuroros
        {
            get => (ArchiveOverrideSetting)(_auroros & 0b11);
            set
            {
                _auroros &= 0b11111100;
                _auroros |= (byte)value;
            }
        }

        public byte _cymul;

        public byte FaceIndexCymul
        {
            get => (byte)((_cymul & 0b11111000) >> 3);
            set
            {
                _cymul &= 0b111;
                _cymul |= (byte)(value.Clamp(0, 0b11111) << 3);
            }
        }

        public bool OverrideModuleCymul
        {
            get => _cymul.GetBit(2);
            set => _cymul.SetBit(2, value);
        }

        public ArchiveOverrideSetting OverrideCymul
        {
            get => (ArchiveOverrideSetting)(_cymul & 0b11);
            set
            {
                _cymul &= 0b11111100;
                _cymul |= (byte)value;
            }
        }

        public byte _roturret;

        public byte FaceIndexRoturret
        {
            get => (byte)((_roturret & 0b11111000) >> 3);
            set
            {
                _roturret &= 0b111;
                _roturret |= (byte)(value.Clamp(0, 0b11111) << 3);
            }
        }

        public bool OverrideModuleRoturret
        {
            get => _roturret.GetBit(2);
            set => _roturret.SetBit(2, value);
        }

        public ArchiveOverrideSetting OverrideRoturret
        {
            get => (ArchiveOverrideSetting)(_roturret & 0b11);
            set
            {
                _roturret &= 0b11111100;
                _roturret |= (byte)value;
            }
        }

        public byte _borboras;

        public byte FaceIndexBorboras
        {
            get => (byte)((_borboras & 0b11111000) >> 3);
            set
            {
                _borboras &= 0b111;
                _borboras |= (byte)(value.Clamp(0, 0b11111) << 3);
            }
        }

        public bool OverrideModuleBorboras
        {
            get => _borboras.GetBit(2);
            set => _borboras.SetBit(2, value);
        }

        public ArchiveOverrideSetting OverrideBorboras
        {
            get => (ArchiveOverrideSetting)(_borboras & 0b11);
            set
            {
                _borboras &= 0b11111100;
                _borboras |= (byte)value;
            }
        }

        public byte _giantGoomba;

        public byte FaceIndexGiantGoomba
        {
            get => (byte)((_giantGoomba & 0b11111000) >> 3);
            set
            {
                _giantGoomba &= 0b111;
                _giantGoomba |= (byte)(value.Clamp(0, 0b11111) << 3);
            }
        }

        public bool OverrideModuleGiantGoomba
        {
            get => _giantGoomba.GetBit(2);
            set => _giantGoomba.SetBit(2, value);
        }

        public ArchiveOverrideSetting OverrideGiantGoomba
        {
            get => (ArchiveOverrideSetting)(_giantGoomba & 0b11);
            set
            {
                _giantGoomba &= 0b11111100;
                _giantGoomba |= (byte)value;
            }
        }

        public byte _buckot;

        public byte FaceIndexBuckot
        {
            get => (byte)((_buckot & 0b11111000) >> 3);
            set
            {
                _buckot &= 0b111;
                _buckot |= (byte)(value.Clamp(0, 0b11111) << 3);
            }
        }

        public bool OverrideModuleBuckot
        {
            get => _buckot.GetBit(2);
            set => _buckot.SetBit(2, value);
        }

        public ArchiveOverrideSetting OverrideBuckot
        {
            get => (ArchiveOverrideSetting)(_buckot & 0b11);
            set
            {
                _buckot &= 0b11111100;
                _buckot |= (byte)value;
            }
        }

        public byte _bucculus;

        public byte FaceIndexBucculus
        {
            get => (byte)((_bucculus & 0b11111000) >> 3);
            set
            {
                _bucculus &= 0b111;
                _bucculus |= (byte)(value.Clamp(0, 0b11111) << 3);
            }
        }

        public bool OverrideModuleBucculus
        {
            get => _bucculus.GetBit(2);
            set => _bucculus.SetBit(2, value);
        }

        public ArchiveOverrideSetting OverrideBucculus
        {
            get => (ArchiveOverrideSetting)(_bucculus & 0b11);
            set
            {
                _bucculus &= 0b11111100;
                _bucculus |= (byte)value;
            }
        }

        public byte _greap;

        public byte FaceIndexGreap
        {
            get => (byte)((_greap & 0b11111000) >> 3);
            set
            {
                _greap &= 0b111;
                _greap |= (byte)(value.Clamp(0, 0b11111) << 3);
            }
        }

        public bool OverrideModuleGreap
        {
            get => _greap.GetBit(2);
            set => _greap.SetBit(2, value);
        }

        public ArchiveOverrideSetting OverrideGreap
        {
            get => (ArchiveOverrideSetting)(_greap & 0b11);
            set
            {
                _greap &= 0b11111100;
                _greap |= (byte)value;
            }
        }

        public byte _armight;

        public byte FaceIndexArmight
        {
            get => (byte)((_armight & 0b11111000) >> 3);
            set
            {
                _armight &= 0b111;
                _armight |= (byte)(value.Clamp(0, 0b11111) << 3);
            }
        }

        public bool OverrideModuleArmight
        {
            get => _armight.GetBit(2);
            set => _armight.SetBit(2, value);
        }

        public ArchiveOverrideSetting OverrideArmight
        {
            get => (ArchiveOverrideSetting)(_armight & 0b11);
            set
            {
                _armight &= 0b11111100;
                _armight |= (byte)value;
            }
        }

        public byte _bulletBill;

        public byte FaceIndexBulletBill
        {
            get => (byte)((_bulletBill & 0b11111000) >> 3);
            set
            {
                _bulletBill &= 0b111;
                _bulletBill |= (byte)(value.Clamp(0, 0b11111) << 3);
            }
        }

        public bool OverrideModuleBulletBill
        {
            get => _bulletBill.GetBit(2);
            set => _bulletBill.SetBit(2, value);
        }

        public ArchiveOverrideSetting OverrideBulletBill
        {
            get => (ArchiveOverrideSetting)(_bulletBill & 0b11);
            set
            {
                _bulletBill &= 0b11111100;
                _bulletBill |= (byte)value;
            }
        }

        public byte _roader;

        public byte FaceIndexRoader
        {
            get => (byte)((_roader & 0b11111000) >> 3);
            set
            {
                _roader &= 0b111;
                _roader |= (byte)(value.Clamp(0, 0b11111) << 3);
            }
        }

        public bool OverrideModuleRoader
        {
            get => _roader.GetBit(2);
            set => _roader.SetBit(2, value);
        }

        public ArchiveOverrideSetting OverrideRoader
        {
            get => (ArchiveOverrideSetting)(_roader & 0b11);
            set
            {
                _roader &= 0b11111100;
                _roader |= (byte)value;
            }
        }

        public byte _spaak;

        public byte FaceIndexSpaak
        {
            get => (byte)((_spaak & 0b11111000) >> 3);
            set
            {
                _spaak &= 0b111;
                _spaak |= (byte)(value.Clamp(0, 0b11111) << 3);
            }
        }

        public bool OverrideModuleSpaak
        {
            get => _spaak.GetBit(2);
            set => _spaak.SetBit(2, value);
        }

        public ArchiveOverrideSetting OverrideSpaak
        {
            get => (ArchiveOverrideSetting)(_spaak & 0b11);
            set
            {
                _spaak &= 0b11111100;
                _spaak |= (byte)value;
            }
        }

        public byte _mite;

        public byte FaceIndexMite
        {
            get => (byte)((_mite & 0b11111000) >> 3);
            set
            {
                _mite &= 0b111;
                _mite |= (byte)(value.Clamp(0, 0b11111) << 3);
            }
        }

        public bool OverrideModuleMite
        {
            get => _mite.GetBit(2);
            set => _mite.SetBit(2, value);
        }

        public ArchiveOverrideSetting OverrideMite
        {
            get => (ArchiveOverrideSetting)(_mite & 0b11);
            set
            {
                _mite &= 0b11111100;
                _mite |= (byte)value;
            }
        }

        public byte _ticken;

        public byte FaceIndexTicken
        {
            get => (byte)((_ticken & 0b11111000) >> 3);
            set
            {
                _ticken &= 0b111;
                _ticken |= (byte)(value.Clamp(0, 0b11111) << 3);
            }
        }

        public bool OverrideModuleTicken
        {
            get => _ticken.GetBit(2);
            set => _ticken.SetBit(2, value);
        }

        public ArchiveOverrideSetting OverrideTicken
        {
            get => (ArchiveOverrideSetting)(_ticken & 0b11);
            set
            {
                _ticken &= 0b11111100;
                _ticken |= (byte)value;
            }
        }

        public byte _towtow;

        public byte FaceIndexTowtow
        {
            get => (byte)((_towtow & 0b11111000) >> 3);
            set
            {
                _towtow &= 0b111;
                _towtow |= (byte)(value.Clamp(0, 0b11111) << 3);
            }
        }

        public bool OverrideModuleTowtow
        {
            get => _towtow.GetBit(2);
            set => _towtow.SetBit(2, value);
        }

        public ArchiveOverrideSetting OverrideTowtow
        {
            get => (ArchiveOverrideSetting)(_towtow & 0b11);
            set
            {
                _towtow &= 0b11111100;
                _towtow |= (byte)value;
            }
        }

        public byte _hammerBro;

        public byte FaceIndexHammerBro
        {
            get => (byte)((_hammerBro & 0b11111000) >> 3);
            set
            {
                _hammerBro &= 0b111;
                _hammerBro |= (byte)(value.Clamp(0, 0b11111) << 3);
            }
        }

        public bool OverrideModuleHammerBro
        {
            get => _hammerBro.GetBit(2);
            set => _hammerBro.SetBit(2, value);
        }

        public ArchiveOverrideSetting OverrideHammerBro
        {
            get => (ArchiveOverrideSetting)(_hammerBro & 0b11);
            set
            {
                _hammerBro &= 0b11111100;
                _hammerBro |= (byte)value;
            }
        }

        public byte _bytan;

        public byte FaceIndexBytan
        {
            get => (byte)((_bytan & 0b11111000) >> 3);
            set
            {
                _bytan &= 0b111;
                _bytan |= (byte)(value.Clamp(0, 0b11111) << 3);
            }
        }

        public bool OverrideModuleBytan
        {
            get => _bytan.GetBit(2);
            set => _bytan.SetBit(2, value);
        }

        public ArchiveOverrideSetting OverrideBytan
        {
            get => (ArchiveOverrideSetting)(_bytan & 0b11);
            set
            {
                _bytan &= 0b11111100;
                _bytan |= (byte)value;
            }
        }

        public byte _floow;

        public byte FaceIndexFloow
        {
            get => (byte)((_floow & 0b11111000) >> 3);
            set
            {
                _floow &= 0b111;
                _floow |= (byte)(value.Clamp(0, 0b11111) << 3);
            }
        }

        public bool OverrideModuleFloow
        {
            get => _floow.GetBit(2);
            set => _floow.SetBit(2, value);
        }

        public ArchiveOverrideSetting OverrideFloow
        {
            get => (ArchiveOverrideSetting)(_floow & 0b11);
            set
            {
                _floow &= 0b11111100;
                _floow |= (byte)value;
            }
        }

        public byte _puppit;

        public byte FaceIndexPuppit
        {
            get => (byte)((_puppit & 0b11111000) >> 3);
            set
            {
                _puppit &= 0b111;
                _puppit |= (byte)(value.Clamp(0, 0b11111) << 3);
            }
        }

        public bool OverrideModulePuppit
        {
            get => _puppit.GetBit(2);
            set => _puppit.SetBit(2, value);
        }

        public ArchiveOverrideSetting OverridePuppit
        {
            get => (ArchiveOverrideSetting)(_puppit & 0b11);
            set
            {
                _puppit &= 0b11111100;
                _puppit |= (byte)value;
            }
        }

        public byte _primid;

        public byte FaceIndexPrimid
        {
            get => (byte)((_primid & 0b11111000) >> 3);
            set
            {
                _primid &= 0b111;
                _primid |= (byte)(value.Clamp(0, 0b11111) << 3);
            }
        }

        public bool OverrideModulePrimid
        {
            get => _primid.GetBit(2);
            set => _primid.SetBit(2, value);
        }

        public ArchiveOverrideSetting OverridePrimid
        {
            get => (ArchiveOverrideSetting)(_primid & 0b11);
            set
            {
                _primid &= 0b11111100;
                _primid |= (byte)value;
            }
        }

        public byte _shellpod;

        public byte FaceIndexShellpod
        {
            get => (byte)((_shellpod & 0b11111000) >> 3);
            set
            {
                _shellpod &= 0b111;
                _shellpod |= (byte)(value.Clamp(0, 0b11111) << 3);
            }
        }

        public bool OverrideModuleShellpod
        {
            get => _shellpod.GetBit(2);
            set => _shellpod.SetBit(2, value);
        }

        public ArchiveOverrideSetting OverrideShellpod
        {
            get => (ArchiveOverrideSetting)(_shellpod & 0b11);
            set
            {
                _shellpod &= 0b11111100;
                _shellpod |= (byte)value;
            }
        }

        public byte _koopa;

        public byte FaceIndexKoopa
        {
            get => (byte)((_koopa & 0b11111000) >> 3);
            set
            {
                _koopa &= 0b111;
                _koopa |= (byte)(value.Clamp(0, 0b11111) << 3);
            }
        }

        public bool OverrideModuleKoopa
        {
            get => _koopa.GetBit(2);
            set => _koopa.SetBit(2, value);
        }

        public ArchiveOverrideSetting OverrideKoopa
        {
            get => (ArchiveOverrideSetting)(_koopa & 0b11);
            set
            {
                _koopa &= 0b11111100;
                _koopa |= (byte)value;
            }
        }

        public byte _shaydas;

        public byte FaceIndexShaydas
        {
            get => (byte)((_shaydas & 0b11111000) >> 3);
            set
            {
                _shaydas &= 0b111;
                _shaydas |= (byte)(value.Clamp(0, 0b11111) << 3);
            }
        }

        public bool OverrideModuleShaydas
        {
            get => _shaydas.GetBit(2);
            set => _shaydas.SetBit(2, value);
        }

        public ArchiveOverrideSetting OverrideShaydas
        {
            get => (ArchiveOverrideSetting)(_shaydas & 0b11);
            set
            {
                _shaydas &= 0b11111100;
                _shaydas |= (byte)value;
            }
        }

        public byte _bombed;

        public byte FaceIndexBombed
        {
            get => (byte)((_bombed & 0b11111000) >> 3);
            set
            {
                _bombed &= 0b111;
                _bombed |= (byte)(value.Clamp(0, 0b11111) << 3);
            }
        }

        public bool OverrideModuleBombed
        {
            get => _bombed.GetBit(2);
            set => _bombed.SetBit(2, value);
        }

        public ArchiveOverrideSetting OverrideBombed
        {
            get => (ArchiveOverrideSetting)(_bombed & 0b11);
            set
            {
                _bombed &= 0b11111100;
                _bombed |= (byte)value;
            }
        }

        public byte _primidMetal;

        public byte FaceIndexPrimidMetal
        {
            get => (byte)((_primidMetal & 0b11111000) >> 3);
            set
            {
                _primidMetal &= 0b111;
                _primidMetal |= (byte)(value.Clamp(0, 0b11111) << 3);
            }
        }

        public bool OverrideModulePrimidMetal
        {
            get => _primidMetal.GetBit(2);
            set => _primidMetal.SetBit(2, value);
        }

        public ArchiveOverrideSetting OverridePrimidMetal
        {
            get => (ArchiveOverrideSetting)(_primidMetal & 0b11);
            set
            {
                _primidMetal &= 0b11111100;
                _primidMetal |= (byte)value;
            }
        }

        public byte _nagagog;

        public byte FaceIndexNagagog
        {
            get => (byte)((_nagagog & 0b11111000) >> 3);
            set
            {
                _nagagog &= 0b111;
                _nagagog |= (byte)(value.Clamp(0, 0b11111) << 3);
            }
        }

        public bool OverrideModuleNagagog
        {
            get => _nagagog.GetBit(2);
            set => _nagagog.SetBit(2, value);
        }

        public ArchiveOverrideSetting OverrideNagagog
        {
            get => (ArchiveOverrideSetting)(_nagagog & 0b11);
            set
            {
                _nagagog &= 0b11111100;
                _nagagog |= (byte)value;
            }
        }

        public byte _trowlon;

        public byte FaceIndexTrowlon
        {
            get => (byte)((_trowlon & 0b11111000) >> 3);
            set
            {
                _trowlon &= 0b111;
                _trowlon |= (byte)(value.Clamp(0, 0b11111) << 3);
            }
        }

        public bool OverrideModuleTrowlon
        {
            get => _trowlon.GetBit(2);
            set => _trowlon.SetBit(2, value);
        }

        public ArchiveOverrideSetting OverrideTrowlon
        {
            get => (ArchiveOverrideSetting)(_trowlon & 0b11);
            set
            {
                _trowlon &= 0b11111100;
                _trowlon |= (byte)value;
            }
        }

        public byte _primidBig;

        public byte FaceIndexPrimidBig
        {
            get => (byte)((_primidBig & 0b11111000) >> 3);
            set
            {
                _primidBig &= 0b111;
                _primidBig |= (byte)(value.Clamp(0, 0b11111) << 3);
            }
        }

        public bool OverrideModulePrimidBig
        {
            get => _primidBig.GetBit(2);
            set => _primidBig.SetBit(2, value);
        }

        public ArchiveOverrideSetting OverridePrimidBig
        {
            get => (ArchiveOverrideSetting)(_primidBig & 0b11);
            set
            {
                _primidBig &= 0b11111100;
                _primidBig |= (byte)value;
            }
        }

        public byte _primidBoom;

        public byte FaceIndexPrimidBoom
        {
            get => (byte)((_primidBoom & 0b11111000) >> 3);
            set
            {
                _primidBoom &= 0b111;
                _primidBoom |= (byte)(value.Clamp(0, 0b11111) << 3);
            }
        }

        public bool OverrideModulePrimidBoom
        {
            get => _primidBoom.GetBit(2);
            set => _primidBoom.SetBit(2, value);
        }

        public ArchiveOverrideSetting OverridePrimidBoom
        {
            get => (ArchiveOverrideSetting)(_primidBoom & 0b11);
            set
            {
                _primidBoom &= 0b11111100;
                _primidBoom |= (byte)value;
            }
        }

        public byte _primidFire;

        public byte FaceIndexPrimidFire
        {
            get => (byte)((_primidFire & 0b11111000) >> 3);
            set
            {
                _primidFire &= 0b111;
                _primidFire |= (byte)(value.Clamp(0, 0b11111) << 3);
            }
        }

        public bool OverrideModulePrimidFire
        {
            get => _primidFire.GetBit(2);
            set => _primidFire.SetBit(2, value);
        }

        public ArchiveOverrideSetting OverridePrimidFire
        {
            get => (ArchiveOverrideSetting)(_primidFire & 0b11);
            set
            {
                _primidFire &= 0b11111100;
                _primidFire |= (byte)value;
            }
        }

        public byte _primidScope;

        public byte FaceIndexPrimidScope
        {
            get => (byte)((_primidScope & 0b11111000) >> 3);
            set
            {
                _primidScope &= 0b111;
                _primidScope |= (byte)(value.Clamp(0, 0b11111) << 3);
            }
        }

        public bool OverrideModulePrimidScope
        {
            get => _primidScope.GetBit(2);
            set => _primidScope.SetBit(2, value);
        }

        public ArchiveOverrideSetting OverridePrimidScope
        {
            get => (ArchiveOverrideSetting)(_primidScope & 0b11);
            set
            {
                _primidScope &= 0b11111100;
                _primidScope |= (byte)value;
            }
        }

        public byte _primidSword;

        public byte FaceIndexPrimidSword
        {
            get => (byte)((_primidSword & 0b11111000) >> 3);
            set
            {
                _primidSword &= 0b111;
                _primidSword |= (byte)(value.Clamp(0, 0b11111) << 3);
            }
        }

        public bool OverrideModulePrimidSword
        {
            get => _primidSword.GetBit(2);
            set => _primidSword.SetBit(2, value);
        }

        public ArchiveOverrideSetting OverridePrimidSword
        {
            get => (ArchiveOverrideSetting)(_primidSword & 0b11);
            set
            {
                _primidSword &= 0b11111100;
                _primidSword |= (byte)value;
            }
        }

        public byte _gamyga;

        public byte FaceIndexGamyga
        {
            get => (byte)((_gamyga & 0b11111000) >> 3);
            set
            {
                _gamyga &= 0b111;
                _gamyga |= (byte)(value.Clamp(0, 0b11111) << 3);
            }
        }

        public bool OverrideModuleGamyga
        {
            get => _gamyga.GetBit(2);
            set => _gamyga.SetBit(2, value);
        }

        public ArchiveOverrideSetting OverrideGamyga
        {
            get => (ArchiveOverrideSetting)(_gamyga & 0b11);
            set
            {
                _gamyga &= 0b11111100;
                _gamyga |= (byte)value;
            }
        }

        public byte _rOBBlaster;

        public byte FaceIndexROBBlaster
        {
            get => (byte)((_rOBBlaster & 0b11111000) >> 3);
            set
            {
                _rOBBlaster &= 0b111;
                _rOBBlaster |= (byte)(value.Clamp(0, 0b11111) << 3);
            }
        }

        public bool OverrideModuleROBBlaster
        {
            get => _rOBBlaster.GetBit(2);
            set => _rOBBlaster.SetBit(2, value);
        }

        public ArchiveOverrideSetting OverrideROBBlaster
        {
            get => (ArchiveOverrideSetting)(_rOBBlaster & 0b11);
            set
            {
                _rOBBlaster &= 0b11111100;
                _rOBBlaster |= (byte)value;
            }
        }

        public byte _rOBDistance;

        public byte FaceIndexROBDistance
        {
            get => (byte)((_rOBDistance & 0b11111000) >> 3);
            set
            {
                _rOBDistance &= 0b111;
                _rOBDistance |= (byte)(value.Clamp(0, 0b11111) << 3);
            }
        }

        public bool OverrideModuleROBDistance
        {
            get => _rOBDistance.GetBit(2);
            set => _rOBDistance.SetBit(2, value);
        }

        public ArchiveOverrideSetting OverrideROBDistance
        {
            get => (ArchiveOverrideSetting)(_rOBDistance & 0b11);
            set
            {
                _rOBDistance &= 0b11111100;
                _rOBDistance |= (byte)value;
            }
        }

        public byte _rOBLauncher;

        public byte FaceIndexROBLauncher
        {
            get => (byte)((_rOBLauncher & 0b11111000) >> 3);
            set
            {
                _rOBLauncher &= 0b111;
                _rOBLauncher |= (byte)(value.Clamp(0, 0b11111) << 3);
            }
        }

        public bool OverrideModuleROBLauncher
        {
            get => _rOBLauncher.GetBit(2);
            set => _rOBLauncher.SetBit(2, value);
        }

        public ArchiveOverrideSetting OverrideROBLauncher
        {
            get => (ArchiveOverrideSetting)(_rOBLauncher & 0b11);
            set
            {
                _rOBLauncher &= 0b11111100;
                _rOBLauncher |= (byte)value;
            }
        }

        public byte _rOBSentry;

        public byte FaceIndexROBSentry
        {
            get => (byte)((_rOBSentry & 0b11111000) >> 3);
            set
            {
                _rOBSentry &= 0b111;
                _rOBSentry |= (byte)(value.Clamp(0, 0b11111) << 3);
            }
        }

        public bool OverrideModuleROBSentry
        {
            get => _rOBSentry.GetBit(2);
            set => _rOBSentry.SetBit(2, value);
        }

        public ArchiveOverrideSetting OverrideROBSentry
        {
            get => (ArchiveOverrideSetting)(_rOBSentry & 0b11);
            set
            {
                _rOBSentry &= 0b11111100;
                _rOBSentry |= (byte)value;
            }
        }

        public byte _autolance;

        public byte FaceIndexAutolance
        {
            get => (byte)((_autolance & 0b11111000) >> 3);
            set
            {
                _autolance &= 0b111;
                _autolance |= (byte)(value.Clamp(0, 0b11111) << 3);
            }
        }

        public bool OverrideModuleAutolance
        {
            get => _autolance.GetBit(2);
            set => _autolance.SetBit(2, value);
        }

        public ArchiveOverrideSetting OverrideAutolance
        {
            get => (ArchiveOverrideSetting)(_autolance & 0b11);
            set
            {
                _autolance &= 0b11111100;
                _autolance |= (byte)value;
            }
        }

        public byte _armank;

        public byte FaceIndexArmank
        {
            get => (byte)((_armank & 0b11111000) >> 3);
            set
            {
                _armank &= 0b111;
                _armank |= (byte)(value.Clamp(0, 0b11111) << 3);
            }
        }

        public bool OverrideModuleArmank
        {
            get => _armank.GetBit(2);
            set => _armank.SetBit(2, value);
        }

        public ArchiveOverrideSetting OverrideArmank
        {
            get => (ArchiveOverrideSetting)(_armank & 0b11);
            set
            {
                _armank &= 0b11111100;
                _armank |= (byte)value;
            }
        }

        public byte _glire;

        public byte FaceIndexGlire
        {
            get => (byte)((_glire & 0b11111000) >> 3);
            set
            {
                _glire &= 0b111;
                _glire |= (byte)(value.Clamp(0, 0b11111) << 3);
            }
        }

        public bool OverrideModuleGlire
        {
            get => _glire.GetBit(2);
            set => _glire.SetBit(2, value);
        }

        public ArchiveOverrideSetting OverrideGlire
        {
            get => (ArchiveOverrideSetting)(_glire & 0b11);
            set
            {
                _glire &= 0b11111100;
                _glire |= (byte)value;
            }
        }

        public byte _glice;

        public byte FaceIndexGlice
        {
            get => (byte)((_glice & 0b11111000) >> 3);
            set
            {
                _glice &= 0b111;
                _glice |= (byte)(value.Clamp(0, 0b11111) << 3);
            }
        }

        public bool OverrideModuleGlice
        {
            get => _glice.GetBit(2);
            set => _glice.SetBit(2, value);
        }

        public ArchiveOverrideSetting OverrideGlice
        {
            get => (ArchiveOverrideSetting)(_glice & 0b11);
            set
            {
                _glice &= 0b11111100;
                _glice |= (byte)value;
            }
        }

        public byte _glunder;

        public byte FaceIndexGlunder
        {
            get => (byte)((_glunder & 0b11111000) >> 3);
            set
            {
                _glunder &= 0b111;
                _glunder |= (byte)(value.Clamp(0, 0b11111) << 3);
            }
        }

        public bool OverrideModuleGlunder
        {
            get => _glunder.GetBit(2);
            set => _glunder.SetBit(2, value);
        }

        public ArchiveOverrideSetting OverrideGlunder
        {
            get => (ArchiveOverrideSetting)(_glunder & 0b11);
            set
            {
                _glunder &= 0b11111100;
                _glunder |= (byte)value;
            }
        }

        public byte _peteyPiranha;

        public byte FaceIndexPeteyPiranha
        {
            get => (byte)((_peteyPiranha & 0b11111000) >> 3);
            set
            {
                _peteyPiranha &= 0b111;
                _peteyPiranha |= (byte)(value.Clamp(0, 0b11111) << 3);
            }
        }

        public bool OverrideModulePeteyPiranha
        {
            get => _peteyPiranha.GetBit(2);
            set => _peteyPiranha.SetBit(2, value);
        }

        public ArchiveOverrideSetting OverridePeteyPiranha
        {
            get => (ArchiveOverrideSetting)(_peteyPiranha & 0b11);
            set
            {
                _peteyPiranha &= 0b11111100;
                _peteyPiranha |= (byte)value;
            }
        }

        public byte _gamygaBase01;

        public byte FaceIndexGamygaBase01
        {
            get => (byte)((_gamygaBase01 & 0b11111000) >> 3);
            set
            {
                _gamygaBase01 &= 0b111;
                _gamygaBase01 |= (byte)(value.Clamp(0, 0b11111) << 3);
            }
        }

        public bool OverrideModuleGamygaBase01
        {
            get => _gamygaBase01.GetBit(2);
            set => _gamygaBase01.SetBit(2, value);
        }

        public ArchiveOverrideSetting OverrideGamygaBase01
        {
            get => (ArchiveOverrideSetting)(_gamygaBase01 & 0b11);
            set
            {
                _gamygaBase01 &= 0b11111100;
                _gamygaBase01 |= (byte)value;
            }
        }

        public byte _gamygaBase02;

        public byte FaceIndexGamygaBase02
        {
            get => (byte)((_gamygaBase02 & 0b11111000) >> 3);
            set
            {
                _gamygaBase02 &= 0b111;
                _gamygaBase02 |= (byte)(value.Clamp(0, 0b11111) << 3);
            }
        }

        public bool OverrideModuleGamygaBase02
        {
            get => _gamygaBase02.GetBit(2);
            set => _gamygaBase02.SetBit(2, value);
        }

        public ArchiveOverrideSetting OverrideGamygaBase02
        {
            get => (ArchiveOverrideSetting)(_gamygaBase02 & 0b11);
            set
            {
                _gamygaBase02 &= 0b11111100;
                _gamygaBase02 |= (byte)value;
            }
        }

        public byte _gamygaBase03;

        public byte FaceIndexGamygaBase03
        {
            get => (byte)((_gamygaBase03 & 0b11111000) >> 3);
            set
            {
                _gamygaBase03 &= 0b111;
                _gamygaBase03 |= (byte)(value.Clamp(0, 0b11111) << 3);
            }
        }

        public bool OverrideModuleGamygaBase03
        {
            get => _gamygaBase03.GetBit(2);
            set => _gamygaBase03.SetBit(2, value);
        }

        public ArchiveOverrideSetting OverrideGamygaBase03
        {
            get => (ArchiveOverrideSetting)(_gamygaBase03 & 0b11);
            set
            {
                _gamygaBase03 &= 0b11111100;
                _gamygaBase03 |= (byte)value;
            }
        }

        public byte _gamygaBase04;

        public byte FaceIndexGamygaBase04
        {
            get => (byte)((_gamygaBase04 & 0b11111000) >> 3);
            set
            {
                _gamygaBase04 &= 0b111;
                _gamygaBase04 |= (byte)(value.Clamp(0, 0b11111) << 3);
            }
        }

        public bool OverrideModuleGamygaBase04
        {
            get => _gamygaBase04.GetBit(2);
            set => _gamygaBase04.SetBit(2, value);
        }

        public ArchiveOverrideSetting OverrideGamygaBase04
        {
            get => (ArchiveOverrideSetting)(_gamygaBase04 & 0b11);
            set
            {
                _gamygaBase04 &= 0b11111100;
                _gamygaBase04 |= (byte)value;
            }
        }

        public byte _galleom;

        public byte FaceIndexGalleom
        {
            get => (byte)((_galleom & 0b11111000) >> 3);
            set
            {
                _galleom &= 0b111;
                _galleom |= (byte)(value.Clamp(0, 0b11111) << 3);
            }
        }

        public bool OverrideModuleGalleom
        {
            get => _galleom.GetBit(2);
            set => _galleom.SetBit(2, value);
        }

        public ArchiveOverrideSetting OverrideGalleom
        {
            get => (ArchiveOverrideSetting)(_galleom & 0b11);
            set
            {
                _galleom &= 0b11111100;
                _galleom |= (byte)value;
            }
        }

        public byte _ridley;

        public byte FaceIndexRidley
        {
            get => (byte)((_ridley & 0b11111000) >> 3);
            set
            {
                _ridley &= 0b111;
                _ridley |= (byte)(value.Clamp(0, 0b11111) << 3);
            }
        }

        public bool OverrideModuleRidley
        {
            get => _ridley.GetBit(2);
            set => _ridley.SetBit(2, value);
        }

        public ArchiveOverrideSetting OverrideRidley
        {
            get => (ArchiveOverrideSetting)(_ridley & 0b11);
            set
            {
                _ridley &= 0b11111100;
                _ridley |= (byte)value;
            }
        }

        public byte _rayquaza;

        public byte FaceIndexRayquaza
        {
            get => (byte)((_rayquaza & 0b11111000) >> 3);
            set
            {
                _rayquaza &= 0b111;
                _rayquaza |= (byte)(value.Clamp(0, 0b11111) << 3);
            }
        }

        public bool OverrideModuleRayquaza
        {
            get => _rayquaza.GetBit(2);
            set => _rayquaza.SetBit(2, value);
        }

        public ArchiveOverrideSetting OverrideRayquaza
        {
            get => (ArchiveOverrideSetting)(_rayquaza & 0b11);
            set
            {
                _rayquaza &= 0b11111100;
                _rayquaza |= (byte)value;
            }
        }

        public byte _duon;

        public byte FaceIndexDuon
        {
            get => (byte)((_duon & 0b11111000) >> 3);
            set
            {
                _duon &= 0b111;
                _duon |= (byte)(value.Clamp(0, 0b11111) << 3);
            }
        }

        public bool OverrideModuleDuon
        {
            get => _duon.GetBit(2);
            set => _duon.SetBit(2, value);
        }

        public ArchiveOverrideSetting OverrideDuon
        {
            get => (ArchiveOverrideSetting)(_duon & 0b11);
            set
            {
                _duon &= 0b11111100;
                _duon |= (byte)value;
            }
        }

        public byte _porky;

        public byte FaceIndexPorky
        {
            get => (byte)((_porky & 0b11111000) >> 3);
            set
            {
                _porky &= 0b111;
                _porky |= (byte)(value.Clamp(0, 0b11111) << 3);
            }
        }

        public bool OverrideModulePorky
        {
            get => _porky.GetBit(2);
            set => _porky.SetBit(2, value);
        }

        public ArchiveOverrideSetting OverridePorky
        {
            get => (ArchiveOverrideSetting)(_porky & 0b11);
            set
            {
                _porky &= 0b11111100;
                _porky |= (byte)value;
            }
        }

        public byte _metaRidley;

        public byte FaceIndexMetaRidley
        {
            get => (byte)((_metaRidley & 0b11111000) >> 3);
            set
            {
                _metaRidley &= 0b111;
                _metaRidley |= (byte)(value.Clamp(0, 0b11111) << 3);
            }
        }

        public bool OverrideModuleMetaRidley
        {
            get => _metaRidley.GetBit(2);
            set => _metaRidley.SetBit(2, value);
        }

        public ArchiveOverrideSetting OverrideMetaRidley
        {
            get => (ArchiveOverrideSetting)(_metaRidley & 0b11);
            set
            {
                _metaRidley &= 0b11111100;
                _metaRidley |= (byte)value;
            }
        }

        public byte _falconFlyer;

        public byte FaceIndexFalconFlyer
        {
            get => (byte)((_falconFlyer & 0b11111000) >> 3);
            set
            {
                _falconFlyer &= 0b111;
                _falconFlyer |= (byte)(value.Clamp(0, 0b11111) << 3);
            }
        }

        public bool OverrideModuleFalconFlyer
        {
            get => _falconFlyer.GetBit(2);
            set => _falconFlyer.SetBit(2, value);
        }

        public ArchiveOverrideSetting OverrideFalconFlyer
        {
            get => (ArchiveOverrideSetting)(_falconFlyer & 0b11);
            set
            {
                _falconFlyer &= 0b11111100;
                _falconFlyer |= (byte)value;
            }
        }

        public byte _tabuu;

        public byte FaceIndexTabuu
        {
            get => (byte)((_tabuu & 0b11111000) >> 3);
            set
            {
                _tabuu &= 0b111;
                _tabuu |= (byte)(value.Clamp(0, 0b11111) << 3);
            }
        }

        public bool OverrideModuleTabuu
        {
            get => _tabuu.GetBit(2);
            set => _tabuu.SetBit(2, value);
        }

        public ArchiveOverrideSetting OverrideTabuu
        {
            get => (ArchiveOverrideSetting)(_tabuu & 0b11);
            set
            {
                _tabuu &= 0b11111100;
                _tabuu |= (byte)value;
            }
        }

        public byte _masterhand;

        public byte FaceIndexMasterhand
        {
            get => (byte)((_masterhand & 0b11111000) >> 3);
            set
            {
                _masterhand &= 0b111;
                _masterhand |= (byte)(value.Clamp(0, 0b11111) << 3);
            }
        }

        public bool OverrideModuleMasterhand
        {
            get => _masterhand.GetBit(2);
            set => _masterhand.SetBit(2, value);
        }

        public ArchiveOverrideSetting OverrideMasterhand
        {
            get => (ArchiveOverrideSetting)(_masterhand & 0b11);
            set
            {
                _masterhand &= 0b11111100;
                _masterhand |= (byte)value;
            }
        }

        public byte _crazyhand;

        public byte FaceIndexCrazyhand
        {
            get => (byte)((_crazyhand & 0b11111000) >> 3);
            set
            {
                _crazyhand &= 0b111;
                _crazyhand |= (byte)(value.Clamp(0, 0b11111) << 3);
            }
        }

        public bool OverrideModuleCrazyhand
        {
            get => _crazyhand.GetBit(2);
            set => _crazyhand.SetBit(2, value);
        }

        public ArchiveOverrideSetting OverrideCrazyhand
        {
            get => (ArchiveOverrideSetting)(_crazyhand & 0b11);
            set
            {
                _crazyhand &= 0b11111100;
                _crazyhand |= (byte)value;
            }
        }
    }
}
