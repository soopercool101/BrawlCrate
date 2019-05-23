using System;

namespace Ikarus.MovesetFile
{
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
