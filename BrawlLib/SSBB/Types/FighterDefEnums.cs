using System;

namespace BrawlLib.SSBB.Types
{
    public enum ArgVarType : int
    {
        Value = 0,
        Scalar = 1,
        Offset = 2,
        Boolean = 3,
        Unknown = 4,
        Variable = 5,
        Requirement = 6
    }

    [Flags]
    public enum AnimationFlags : byte
    {
        None = 0,
        NoOutTransition = 1,
        Loop = 2,
        MovesCharacter = 4,
        FixedTranslation = 8,
        FixedRotation = 16,
        FixedScale = 32,
        TransitionOutFromStart = 64,
        Unknown = 128
    }
}