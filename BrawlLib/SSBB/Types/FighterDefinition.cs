using BrawlLib.Internal;
using System.Runtime.InteropServices;

namespace BrawlLib.SSBB.Types
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct FDefHeader
    {
        public bint _fileSize;
        public bint _lookupOffset;
        public bint _lookupEntryCount;
        public bint _dataTableEntryCount;     //Has string entry
        public bint _externalSubRoutineCount; //Has string entry
        public int _pad1, _pad2, _pad3;

        //From here begins file data. All offsets are relative to this location (0x20).

        public FDefAttributes* Attributes => (FDefAttributes*) (Address + 0x20);

        public bint* LookupEntries => (bint*) (Address + _lookupOffset + 0x20);

        public FDefStringEntry* DataTable =>
            (FDefStringEntry*) (Address + _lookupOffset + 0x20 + _lookupEntryCount * 4);

        public FDefStringEntry* ExternalSubRoutines =>
            (FDefStringEntry*) (Address + _lookupOffset + 0x20 + _lookupEntryCount * 4 + _dataTableEntryCount * 8);

        //for DataTable and ExternalSubRoutines
        public FDefStringTable* StringTable =>
            (FDefStringTable*) (Address + _lookupOffset + 0x20 + _lookupEntryCount * 4 + _dataTableEntryCount * 8 +
                                _externalSubRoutineCount * 8);

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
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct FDefLookupOffset
    {
        public const int Size = 4;

        public bint _offset;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct FDefStringEntry
    {
        public bint _dataOffset;
        public bint _stringOffset; //Base is string table
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct FDefStringTable
    {
        public VoidPtr Address
        {
            get
            {
                fixed (void* ptr = &this)
                {
                    return ptr;
                }
            }
        }

        public string GetString(int offset)
        {
            return new string((sbyte*) Address + offset);
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct MovesetHeader
    {
        public const int Size = 0x8C;

        public bint SubactionFlagsStart;
        public bint ModelVisibilityStart;
        public bint AttributeStart;
        public bint SSEAttributeStart;

        public bint MiscSectionOffset;
        public bint CommonActionFlagsStart;
        public bint ActionFlagsStart;
        public bint Unknown7;

        public bint ActionInterrupts;
        public bint EntryActionsStart;
        public bint ExitActionsStart;
        public bint ActionPreStart;

        public bint SubactionMainStart;
        public bint SubactionGFXStart;
        public bint SubactionSFXStart;
        public bint SubactionOtherStart;

        public bint BoneFloats1;
        public bint BoneFloats2;
        public bint BoneRef1;
        public bint BoneRef2;

        public bint EntryActionOverrides;
        public bint ExitActionOverrides;
        public bint Unknown22;
        public bint BoneFloats3;

        public bint Unknown24;
        public bint StaticArticlesStart;
        public bint EntryArticleStart;

        public bint Unknown27;
        public bint Unknown28;
        public buint Flags1;
        public bint Flags2; //Sometimes -1

        public VoidPtr Address
        {
            get
            {
                fixed (void* ptr = &this)
                {
                    return ptr;
                }
            }
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct CommonMovesetHeader
    {
        public bint Unknown0;
        public bint Unknown1;
        public bint Unknown2;
        public bint Unknown3;
        public bint ActionsStart;
        public bint Actions2Start;
        public bint Unknown6;
        public bint Unknown7;
        public bint Unknown8;
        public bint Unknown9;
        public bint Unknown10;
        public bint Unknown11;
        public bint Unknown12;
        public bint Unknown13;
        public bint Unknown14;
        public bint Unknown15;
        public bint Unknown16;
        public bint Unknown17;
        public bint Unknown18;
        public bint Unknown19;
        public bint Unknown20;
        public bint Unknown21;
        public bint Unknown22;
        public bint Unknown23;
        public bint Unknown24;
        public bint Unknown25;

        public VoidPtr Address
        {
            get
            {
                fixed (void* ptr = &this)
                {
                    return ptr;
                }
            }
        }
    }


    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct patternPowerMul
    {
        public bfloat _unk1;
        public bfloat _unk2;
        public bfloat _unk3;
        public bfloat _unk4;

        public bfloat _unk5;
        public bfloat _unk6;
        public bfloat _unk7;
        public bfloat _unk8;

        public bfloat _unk9;
        public bfloat _unk10;

        //Four action nodes in a row start here
        public byte _first;

        public VoidPtr Address
        {
            get
            {
                fixed (void* ptr = &this)
                {
                    return ptr;
                }
            }
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct patternPowerMulEntry
    {
        public FDefEvent _event1;
        public FDefEvent _event2;
        public bint _pad1;
        public bint _pad2;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct AnimParamHeader
    {
        public bint Unknown0;
        public bint Unknown1;
        public bint Unknown2;
        public bint Unknown3;
        public bint Unknown4;
        public bint Unknown5;
        public bint Unknown6;
        public bint Unknown7;
        public bint Unknown8;
        public bint Unknown9;
        public bint Unknown10;
        public bint Unknown11;
        public bint Unknown12;
        public bint Unknown13;
        public bint Unknown14;
        public bint Unknown15;

        public VoidPtr Address
        {
            get
            {
                fixed (void* ptr = &this)
                {
                    return ptr;
                }
            }
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct SubParamHeader
    {
        public bint Unknown0;
        public bint Unknown1;
        public bint Unknown2;
        public bint Unknown3;
        public bint Unknown4;
        public bint Unknown5;

        public VoidPtr Address
        {
            get
            {
                fixed (void* ptr = &this)
                {
                    return ptr;
                }
            }
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct FDefCommonUnk7Entry
    {
        public FDefListOffset _list;
        public bshort _unk3;
        public bshort _unk4;

        public VoidPtr Address
        {
            get
            {
                fixed (void* ptr = &this)
                {
                    return ptr;
                }
            }
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct FDefCommonUnk7EntryListEntry
    {
        public bfloat _unk1;
        public bfloat _unk2;

        public VoidPtr Address
        {
            get
            {
                fixed (void* ptr = &this)
                {
                    return ptr;
                }
            }
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct FDefCommonUnk11Entry
    {
        public bint _unk1;
        public FDefListOffset _list;

        public VoidPtr Address
        {
            get
            {
                fixed (void* ptr = &this)
                {
                    return ptr;
                }
            }
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct ActionOverride
    {
        public bint _actionID;
        public bint _commandListOffset;

        public VoidPtr Address
        {
            get
            {
                fixed (void* ptr = &this)
                {
                    return ptr;
                }
            }
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct Article
    {
        public const int Size = 56;

        public bint _id;
        public bint _arcGroup;
        public bint _boneID;
        public bint _actionFlagsStart;

        public bint _subactionFlagsStart;
        public bint _actionsStart;
        public bint _subactionMainStart;
        public bint _subactionGFXStart;

        public bint _subactionSFXStart;
        public bint _modelVisibility;
        public bint _collisionData;
        public bint _unknownD2;

        public bint _unknownD3;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct FDefMiscUnk3
    {
        public const int Size = 28;

        public bint _haveNBoneIndex1;
        public bint _haveNBoneIndex2;
        public bint _throwNBoneIndex;
        public FDefListOffset _list;
        public bint _pad; //0
        public bint _haveNBoneIndex3;

        public VoidPtr Address
        {
            get
            {
                fixed (void* ptr = &this)
                {
                    return ptr;
                }
            }
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct FDefBoneRef2
    {
        public const int Size = 24;

        public bint _handNBoneIndex1;
        public bint _handNBoneIndex2;
        public bint _handNBoneIndex3;
        public bint _handNBoneIndex4;

        public bint _count;
        public bint _offset;

        public VoidPtr Address
        {
            get
            {
                fixed (void* ptr = &this)
                {
                    return ptr;
                }
            }
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct FDefMiscUnk3Entry
    {
        public const int Size = 16;

        public bint _unk1;
        public bint _unk2;
        public bint _pad1;
        public bint _pad2;

        public VoidPtr Address
        {
            get
            {
                fixed (void* ptr = &this)
                {
                    return ptr;
                }
            }
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct FDefMiscSection5
    {
        public bint _unk1; //0x2B
        public bint _unk2; //0x64
        public bint _unk3; //-1
        public bint _unk4; //-1

        public VoidPtr Address
        {
            get
            {
                fixed (void* ptr = &this)
                {
                    return ptr;
                }
            }
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct FDefMiscSection1
    {
        public bint _unk1;
        public bint _unk2;
        public bint _unk3;
        public bint _unk4;
        public bint _unk5;
        public bint _unk6;
        public bint _unk7;
        public bint _unk8;

        public VoidPtr Address
        {
            get
            {
                fixed (void* ptr = &this)
                {
                    return ptr;
                }
            }
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct collData0
    {
        public int type;
        public FDefListOffset _list;
        public bfloat unk1;
        public bfloat unk2;
        public bfloat unk3;

        public VoidPtr Address
        {
            get
            {
                fixed (void* ptr = &this)
                {
                    return ptr;
                }
            }
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct collData1
    {
        public int type;
        public bfloat unk1;
        public bfloat unk2;
        public bfloat unk3;

        public VoidPtr Address
        {
            get
            {
                fixed (void* ptr = &this)
                {
                    return ptr;
                }
            }
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct collData2
    {
        public int type;
        public bint flags; //Usually 1
        public bfloat unk1;
        public bfloat unk2;
        public bfloat unk3;
        public bfloat unk4; //Exists only if flags == 3

        public VoidPtr Address
        {
            get
            {
                fixed (void* ptr = &this)
                {
                    return ptr;
                }
            }
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct FDefSubActionFlag
    {
        public byte _InTranslationTime;
        public AnimationFlags _Flags;
        public short pad;

        //String spacing == (String length + 1).Align(4)
        public bint _stringOffset;

        public override string ToString()
        {
            return $"TransInTime:{_InTranslationTime}; Flags:{_Flags.ToString()}; StringOff:{(int) _stringOffset}";
        }

        public VoidPtr Address
        {
            get
            {
                fixed (void* ptr = &this)
                {
                    return ptr;
                }
            }
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct FDefSubActionString
    {
        public sbyte _data;

        private void* Address
        {
            get
            {
                fixed (void* p = &this)
                {
                    return p;
                }
            }
        }

        public sbyte* Data => (sbyte*) Address;

        public int Length => Value.Length;

        public string Value
        {
            get => new string(Data);
            set
            {
                if (value == null)
                {
                    value = "";
                }

                int len = value.Length;
                int ceil = (len + 1).Align(4);

                sbyte* ptr = Data;

                for (int i = 0; i < len;)
                {
                    ptr[i] = (sbyte) value[i++];
                }

                for (int i = len; i < ceil;)
                {
                    ptr[i++] = 0;
                }
            }
        }

        public FDefSubActionString* Next => (FDefSubActionString*) ((byte*) Address + (Length + 1).Align(4));

        public FDefSubActionString* End
        {
            get
            {
                FDefSubActionString* p = (FDefSubActionString*) Address;
                while (p->Length != 0)
                {
                    p = p->Next;
                }

                return p;
            }
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct FDefReferenceString
    {
        public sbyte _data;

        private void* Address
        {
            get
            {
                fixed (void* p = &this)
                {
                    return p;
                }
            }
        }

        public sbyte* Data => (sbyte*) Address;

        public int Length => Value.Length;

        public string Value
        {
            get => new string(Data);
            set
            {
                if (value == null)
                {
                    value = "";
                }

                int len = value.Length;
                int ceil = len + 1;

                sbyte* ptr = Data;

                for (int i = 0; i < len;)
                {
                    ptr[i] = (sbyte) value[i++];
                }

                for (int i = len; i < ceil;)
                {
                    ptr[i++] = 0;
                }
            }
        }

        public FDefReferenceString* Next => (FDefReferenceString*) ((byte*) Address + (Length + 1));

        public FDefReferenceString* End
        {
            get
            {
                FDefReferenceString* p = (FDefReferenceString*) Address;
                while (p->Length != 0)
                {
                    p = p->Next;
                }

                return p;
            }
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct FDefEvent
    {
        public byte _nameSpace;
        public byte _id;
        public byte _numArguments;
        public byte _unk1;
        public buint _argumentOffset;

        public VoidPtr Address
        {
            get
            {
                fixed (void* ptr = &this)
                {
                    return ptr;
                }
            }
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct FDefEventArgument
    {
        public bint _type;
        public bint _data;

        public VoidPtr Address
        {
            get
            {
                fixed (void* ptr = &this)
                {
                    return ptr;
                }
            }
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct FDefMiscSection
    {
        public const int Size = 0x4C;

        public bint UnknownSection1Offset;
        public bint UnkBoneSectionOffset;
        public bint UnkBoneSectionCount;
        public bint HurtBoxOffset;
        public bint HurtBoxCount;
        public bint LedgegrabOffset;
        public bint LedgegrabCount;
        public bint UnknownSection2Offset;
        public bint UnknownSection2Count;
        public bint BoneRef2Offset;
        public bint UnknownSection3Offset;
        public bint SoundDataOffset;
        public bint UnknownSection5Offset;
        public bint MultiJumpOffset;
        public bint GlideOffset;
        public bint CrawlOffset;
        public bint CollisionDataOffset;
        public bint TetherOffset;
        public bint UnknownSection12Offset;

        public VoidPtr Address
        {
            get
            {
                fixed (void* ptr = &this)
                {
                    return ptr;
                }
            }
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct FDefModelDisplayDefaults
    {
        public bint _switchIndex;
        public bint _defaultGroup;

        public VoidPtr Address
        {
            get
            {
                fixed (void* ptr = &this)
                {
                    return ptr;
                }
            }
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct FDefMiscSection9Data
    {
        public bint _unk1;
        public FDefListOffset _list;
        public bfloat _unk2;
        public bfloat _unk3;
        public bint _unk4;

        public VoidPtr Address
        {
            get
            {
                fixed (void* ptr = &this)
                {
                    return ptr;
                }
            }
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct FDefMiscUnkType1
    {
        public const int Size = 0x14;

        public bint _boneIndex;
        public bfloat _x;
        public bfloat _y;
        public bfloat _width;
        public bfloat _height;

        public VoidPtr Address
        {
            get
            {
                fixed (void* ptr = &this)
                {
                    return ptr;
                }
            }
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct FDefModelDisplay
    {
        public bint _entryOffset;
        public bint _entryCount;
        public bint _defaultsOffset;
        public bint _defaultsCount;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct FDefUnk22
    {
        public bint _unk1;
        public bint _unk2;
        public bint _actionOffset;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct Unk17Entry
    {
        public const int Size = 0x1C;

        public bint _boneIndex;
        public BVec3 _unkVec1;
        public BVec3 _unkVec2;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct DataUnk23
    {
        public const int Size = 0x20;

        public bint _boneIndex;
        public bfloat _unk1;
        public bfloat _unk2;
        public bfloat _unk3;

        public bfloat _unk4;
        public bfloat _unk5;
        public bfloat _unk6;
        public bfloat _unk7;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct ActionFlags
    {
        public bint _flags1; //Sometimes -1
        public bint _flags2; //Sometimes -1
        public bint _flags3; //Sometimes -1
        public buint _flags4;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct FDefHurtBox
    {
        public BVec3 _offset;
        public BVec3 _stretch;
        public bfloat _radius;
        public HurtBoxFlags _flags;

        public VoidPtr Address
        {
            get
            {
                fixed (void* ptr = &this)
                {
                    return ptr;
                }
            }
        }
    }

    public enum HurtBoxZone
    {
        Low = 0,
        Middle,
        High
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct HurtBoxFlags
    {
        //0000 0000 0000 0001   Enabled
        //0000 0000 0000 0110   Unused
        //0000 0000 0001 1000   Zone
        //0000 0000 0110 0000   Region
        //1111 1111 1000 0000   Bone Index

        private byte dat0, dat1, pad0, pad1;

        public int BoneIndex
        {
            get => (int) ((((dat1 >> 7) & 1) | (((int) dat0 << 1) & 0xFF)) & 0x1FF);
            set
            {
                dat0 = (byte) (value >> 1);
                dat1 = (byte) ((dat1 & 0x7F) | ((value & 1) << 7));
            }
        }

        public HurtBoxZone Zone
        {
            get => (HurtBoxZone) ((dat1 >> 3) & 3);
            set => dat1 = (byte) ((dat1 & 0xE7) | (((byte) value & 3) << 3));
        }

        public bool Enabled
        {
            get => (dat1 & 1) != 0;
            set => dat1 = (byte) ((dat1 & 0xFE) | ((byte) (value ? 1 : 0) & 1));
        }

        public int Region
        {
            get => (dat1 >> 5) & 3;
            set => dat1 = (byte) ((dat1 & 0x9F) | (((byte) value & 3) << 5));
        }

        public int Unk
        {
            get => (dat1 >> 1) & 3;
            set => dat1 = (byte) ((dat1 & 0xF9) | (((byte) value & 3) << 1));
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct FDefLedgegrab
    {
        public bfloat _x;
        public bfloat _y;
        public bfloat _width;
        public bfloat _height;

        public VoidPtr Address
        {
            get
            {
                fixed (void* ptr = &this)
                {
                    return ptr;
                }
            }
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct FDefMultiJump
    {
        public bfloat _unk1;
        public bfloat _unk2;
        public bfloat _unk3;
        public bfloat _horizontalBoost;

        //all can be either int or fixed float...
        //where are the flags?
        public bint _hopListOffset;
        public bint _unkListOffset;
        public bint _turnFrameOffset;

        public bool hopFixed
        {
            get
            {
                if ((((int) _hopListOffset >> 24) & 0xFF) != 0)
                {
                    return true;
                }

                return false;
            }
        }

        public bool unkFixed
        {
            get
            {
                if ((((int) _unkListOffset >> 24) & 0xFF) != 0)
                {
                    return true;
                }

                return false;
            }
        }

        public bool turnFixed
        {
            get
            {
                if ((((int) _turnFrameOffset >> 24) & 0xFF) != 0)
                {
                    return true;
                }

                return false;
            }
        }

        public VoidPtr Address
        {
            get
            {
                fixed (void* ptr = &this)
                {
                    return ptr;
                }
            }
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct FDefCrawl
    {
        public bfloat _forward;
        public bfloat _backward;

        public VoidPtr Address
        {
            get
            {
                fixed (void* ptr = &this)
                {
                    return ptr;
                }
            }
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct FDefTether
    {
        public bint _numHangFrame;
        public bfloat _unk1;

        public VoidPtr Address
        {
            get
            {
                fixed (void* ptr = &this)
                {
                    return ptr;
                }
            }
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct FDefListOffset
    {
        public bint _startOffset;
        public bint _listCount;

        public VoidPtr Address
        {
            get
            {
                fixed (void* ptr = &this)
                {
                    return ptr;
                }
            }
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct FDefAttributes
    {
        public const int Size = 0x2E4;

        //0x20
        public bfloat _walkInitVelocity;
        public bfloat _walkAcceleration;
        public bfloat _walkMaxVelocity;
        public bfloat _stopVelocity;

        //0x30
        public bfloat _dashInitVelocity;
        public bfloat _stopTurnDecel;
        public bfloat _stopTurnAccel;
        public bfloat _runInitVelocity;

        //0x40
        public bfloat _unk01; //30
        public bfloat _unk02; //3
        public bint _unk03;   //1
        public bfloat _unk04; //1.3

        //0x50
        public bint _unk05;   //5
        public bfloat _unk06; //0.9
        public bfloat _jumpYInitVelocity;
        public bfloat _unk07;

        //0x60
        public bfloat _jumpXInitVelocity;
        public bfloat _hopYInitVelocity;
        public bfloat _airJumpMultiplier;
        public bfloat _unk08;

        //0x70
        public bfloat _stoolYInitVelocity;
        public bfloat _unk09;
        public bfloat _unk10;
        public bfloat _unk11;

        //0x80
        public bint _unk12;
        public bfloat _gravity;
        public bfloat _termVelocity;
        public bfloat _unk13;

        //0x90
        public bfloat _unk14;
        public bfloat _airMobility;
        public bfloat _airStopMobility;
        public bfloat _airMaxXVelocity;

        //0xA0
        public bfloat _unk15;
        public bfloat _unk16;
        public bfloat _unk17;
        public bint _unk18;

        //0xB0
        public bfloat _unk19;
        public bfloat _unk20;
        public bfloat _unk21;
        public bint _unk22;

        //0xC0
        public bint _unk23;
        public bint _unk24;
        public bfloat _unk25;
        public bfloat _unk26;

        //0xD0
        public bfloat _weight;
        public bfloat _unk27;
        public bfloat _unk28;
        public bfloat _unk29;

        //0xE0
        public bfloat _unk30;
        public bfloat _shieldSize;
        public bfloat _shieldBreakBounce;
        public bfloat _unk31;

        //0xF0
        public bfloat _unk32;
        public bfloat _unk33;
        public bfloat _unk34;
        public bfloat _unk35;

        //0x100
        public bfloat _unk36;
        public bfloat _unk37;
        public bint _unk38;
        public bint _unk39;

        //0x110
        public bint _unk40;
        public bfloat _unk41;
        public bfloat _edgeJumpYVelocity;
        public bfloat _edgeJumpXVelocity;

        //0x120
        public bfloat _unk42;
        public bfloat _unk43;
        public bfloat _unk44;
        public bfloat _unk45;

        //0x130
        public bfloat _unk46;
        public bint _unk47;
        public bfloat _itemThrowStrength;
        public bfloat _unk48;

        //0x140
        public bfloat _unk49;
        public bfloat _unk50;
        public bfloat _fireMoveSpeed;
        public bfloat _fireFDashSpeed;

        //0x150
        public bfloat _fireBDashSpeed;
        public bfloat _unk51;
        public bfloat _unk52;
        public bfloat _unk53;

        //0x160
        public bfloat _unk54;
        public bfloat _unk55;
        public bfloat _unk56;
        public bfloat _unk57;

        //0x170
        public bfloat _unk58;
        public bint _unk59;
        public bint _unk60;
        public bfloat _unk61;

        //0x180
        public bfloat _unk62;
        public bfloat _wallJumpYVelocity;
        public bfloat _wallJumpXVelocity;
        public bfloat _unk63;

        //0x190
        public bfloat _unk64;
        public bint _unk65;
        public bfloat _unk66;
        public bfloat _unk67;

        //0x1A0
        public bint _unk68;
        public bint _unk69;
        public bfloat _unk70;
        public bfloat _unk71;

        //0x1B0
        public bfloat _unk72;
        public bfloat _unk73;
        public bfloat _unk74;
        public bfloat _unk75;

        //0x1C0
        public bfloat _unk76;
        public bfloat _unk77;
        public bint _unk78;
        public bfloat _unk79;

        //0x1D0
        public bint _unk80;
        public bfloat _unk81;
        public bfloat _unk82;
        public bfloat _unk83;

        //0x1E0
        public bfloat _unk84;
        public bfloat _unk85;
        public bfloat _unk86;
        public bint _unk89;

        //0x1F0
        public bfloat _unk90;
        public bfloat _unk91;
        public bfloat _unk92;
        public bfloat _unk93;

        //0x200
        public bint _unk94;
        public bfloat _unk95;
        public bfloat _unk96;
        public bfloat _unk97;

        //0x210
        public bfloat _unk98;
        public bfloat _unk99;
        public bfloat _unk100;
        public bfloat _unk101;

        //0x220
        public bfloat _unk102;
        public bfloat _unk103;
        public bfloat _unk104;
        public bfloat _unk105;

        //0x230
        public bfloat _unk106;
        public bfloat _unk107;
        public bfloat _unk108;
        public bfloat _unk109;

        //0x240
        public bint _unk110;
        public bfloat _unk111;
        public bfloat _unk112;
        public bfloat _unk113;

        //0x250
        public bfloat _unk114;
        public bint _unk115;
        public bfloat _unk116;
        public bfloat _unk117;

        //0x260
        public bfloat _unk118;
        public bfloat _unk119;
        public bfloat _unk120;
        public bfloat _unk121;

        //0x270
        public bfloat _unk122;
        public bfloat _unk123;
        public bfloat _unk124;
        public bfloat _unk125;

        //0x280
        public bfloat _unk126;
        public bfloat _unk127;
        public bfloat _unk128;
        public bfloat _unk129;

        //0x290
        public bfloat _unk130;
        public bfloat _unk131;
        public bfloat _unk132;
        public bfloat _unk133;

        //0x2A0
        public bfloat _unk134;
        public bfloat _unk135;
        public bfloat _unk136;
        public bint _unk137;

        //0x2B0
        public bint _unk138;
        public bint _unk139;
        public bfloat _unk140;
        public bint _unk141;

        //0x2C0
        public bfloat _unk142;
        public bfloat _unk143;
        public bfloat _unk144;
        public bfloat _unk145;

        //0x2D0
        public bfloat _unk146;
        public bfloat _unk147;
        public bfloat _unk148;
        public bfloat _unk149;

        //0x2E0
        public bint _unk150;
        public bint _unk151;
        public bint _unk152;
        public bint _unk153;

        //0x2F0
        public bint _unk154;
        public bint _unk155;
        public bint _unk156;
        public bint _unk157;

        //0x300
        public bint _unk158;
    }
}