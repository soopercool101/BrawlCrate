using BrawlLib.Internal;
using BrawlLib.SSBB.Types;
using BrawlLib.Wii.Graphics;
using System;
using System.Runtime.InteropServices;

namespace BrawlLib.Wii.Animations
{
    [Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct CHRAnimationFrame
    {
        public static readonly CHRAnimationFrame Identity =
            new CHRAnimationFrame(new Vector3(1.0f), new Vector3(), new Vector3());

        public static readonly CHRAnimationFrame Empty;

        public Vector3 Scale;
        public Vector3 Rotation;
        public Vector3 Translation;

        public bool hasSx;
        public bool hasSy;
        public bool hasSz;

        public bool hasRx;
        public bool hasRy;
        public bool hasRz;

        public bool hasTx;
        public bool hasTy;
        public bool hasTz;

        public bool HasKeys => hasSx || hasSy || hasSz || hasRx || hasRy || hasRz || hasTx || hasTy || hasTz;

        public void SetBool(int index, bool val)
        {
            switch (index)
            {
                case 0:
                    hasSx = val;
                    break;
                case 1:
                    hasSy = val;
                    break;
                case 2:
                    hasSz = val;
                    break;
                case 3:
                    hasRx = val;
                    break;
                case 4:
                    hasRy = val;
                    break;
                case 5:
                    hasRz = val;
                    break;
                case 6:
                    hasTx = val;
                    break;
                case 7:
                    hasTy = val;
                    break;
                case 8:
                    hasTz = val;
                    break;
            }
        }

        public bool GetBool(int index)
        {
            switch (index)
            {
                case 0:
                    return hasSx;
                case 1:
                    return hasSy;
                case 2:
                    return hasSz;
                case 3:
                    return hasRx;
                case 4:
                    return hasRy;
                case 5:
                    return hasRz;
                case 6:
                    return hasTx;
                case 7:
                    return hasTy;
                case 8:
                    return hasTz;
            }

            return false;
        }

        public void ResetBools()
        {
            hasRx = hasRy = hasRz =
                hasSx = hasSy = hasSz =
                    hasTx = hasTy = hasTz = false;
        }

        public float this[int index]
        {
            get
            {
                switch (index)
                {
                    case 0:  return Scale._x;
                    case 1:  return Scale._y;
                    case 2:  return Scale._z;
                    case 3:  return Rotation._x;
                    case 4:  return Rotation._y;
                    case 5:  return Rotation._z;
                    case 6:  return Translation._x;
                    case 7:  return Translation._y;
                    case 8:  return Translation._z;
                    default: return float.NaN;
                }
            }
            set
            {
                switch (index)
                {
                    case 0:
                        Scale._x = value;
                        break;
                    case 1:
                        Scale._y = value;
                        break;
                    case 2:
                        Scale._z = value;
                        break;
                    case 3:
                        Rotation._x = value;
                        break;
                    case 4:
                        Rotation._y = value;
                        break;
                    case 5:
                        Rotation._z = value;
                        break;
                    case 6:
                        Translation._x = value;
                        break;
                    case 7:
                        Translation._y = value;
                        break;
                    case 8:
                        Translation._z = value;
                        break;
                }
            }
        }

        public CHRAnimationFrame(Vector3 scale, Vector3 rotation, Vector3 translation)
        {
            Scale = scale;
            Rotation = rotation;
            Translation = translation;
            Index = 0;
            hasSx = hasSy = hasSz = hasRx = hasRy = hasRz = hasTx = hasTy = hasTz = false;
        }

        public int Index;
        private const int len = 6;
        private static readonly string empty = new string('_', len);

        public override string ToString()
        {
            return
                $"[{(Index + 1).ToString().PadLeft(5)}]({(!hasSx ? empty : Scale._x.ToString().TruncateAndFill(len, ' '))},{(!hasSy ? empty : Scale._y.ToString().TruncateAndFill(len, ' '))},{(!hasSz ? empty : Scale._z.ToString().TruncateAndFill(len, ' '))})({(!hasRx ? empty : Rotation._x.ToString().TruncateAndFill(len, ' '))},{(!hasRy ? empty : Rotation._y.ToString().TruncateAndFill(len, ' '))},{(!hasRz ? empty : Rotation._z.ToString().TruncateAndFill(len, ' '))})({(!hasTx ? empty : Translation._x.ToString().TruncateAndFill(len, ' '))},{(!hasTy ? empty : Translation._y.ToString().TruncateAndFill(len, ' '))},{(!hasTz ? empty : Translation._z.ToString().TruncateAndFill(len, ' '))})";
        }

        //public override string ToString()
        //{
        //    return String.Format("{0}\r\n{1}\r\n{2}", Scale, Rotation, Translation);
        //}
    }

    [Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SRTAnimationFrame
    {
        public static readonly SRTAnimationFrame Identity = new SRTAnimationFrame(new Vector2(1.0f), 0, new Vector2());
        public static readonly SRTAnimationFrame Empty;

        public Vector2 Scale;
        public float Rotation;
        public Vector2 Translation;

        public bool hasSx;
        public bool hasSy;
        public bool hasRx;
        public bool hasTx;
        public bool hasTy;

        public bool HasKeys => hasSx || hasSy || hasRx || hasTx || hasTy;

        public void SetBool(int index, bool val)
        {
            switch (index)
            {
                case 0:
                    hasSx = val;
                    break;
                case 1:
                    hasSy = val;
                    break;
                case 2:
                    hasRx = val;
                    break;
                case 3:
                    hasTx = val;
                    break;
                case 4:
                    hasTy = val;
                    break;
            }
        }

        public bool GetBool(int index)
        {
            switch (index)
            {
                case 0: return hasSx;
                case 1: return hasSy;
                case 2: return hasRx;
                case 3: return hasTx;
                case 4: return hasTy;
            }

            return false;
        }

        public void ResetBools()
        {
            hasRx = hasSx = hasSy = hasTx = hasTy = false;
        }

        public float this[int index]
        {
            get
            {
                switch (index)
                {
                    case 0:  return Scale._x;
                    case 1:  return Scale._y;
                    case 2:  return Rotation;
                    case 3:  return Translation._x;
                    case 4:  return Translation._y;
                    default: return float.NaN;
                }
            }
            set
            {
                switch (index)
                {
                    case 0:
                        Scale._x = value;
                        break;
                    case 1:
                        Scale._y = value;
                        break;
                    case 2:
                        Rotation = value;
                        break;
                    case 3:
                        Translation._x = value;
                        break;
                    case 4:
                        Translation._y = value;
                        break;
                }
            }
        }

        public SRTAnimationFrame(Vector2 scale, float rotation, Vector2 translation)
        {
            Scale = scale;
            Rotation = rotation;
            Translation = translation;
            Index = 0;
            hasSx = hasSy = hasRx = hasTx = hasTy = false;
        }

        public int Index;
        private const int len = 6;
        private static readonly string empty = new string('_', len);

        public override string ToString()
        {
            return
                $"[{(Index + 1).ToString().PadLeft(5)}]({(!hasSx ? empty : Scale._x.ToString().TruncateAndFill(len, ' '))},{(!hasSy ? empty : Scale._y.ToString().TruncateAndFill(len, ' '))})({(!hasRx ? empty : Rotation.ToString().TruncateAndFill(len, ' '))})({(!hasTx ? empty : Translation._x.ToString().TruncateAndFill(len, ' '))},{(!hasTy ? empty : Translation._y.ToString().TruncateAndFill(len, ' '))})";
        }

        //public override string ToString()
        //{
        //    return String.Format("{0}\r\n{1}\r\n{2}", Scale, Rotation, Translation);
        //}
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct FogAnimationFrame
    {
        public static readonly FogAnimationFrame Empty;

        public float Start;
        public float End;
        public Vector3 Color;
        public FogType Type;

        public bool hasS;
        public bool hasE;

        public bool HasKeys => hasS || hasE;

        public void SetBools(int index, bool val)
        {
            switch (index)
            {
                case 0:
                    hasS = val;
                    break;
                case 1:
                    hasE = val;
                    break;
            }
        }

        public void ResetBools()
        {
            hasS = hasE = false;
        }

        public float this[int index]
        {
            get
            {
                switch (index)
                {
                    case 0:  return Start;
                    case 1:  return End;
                    default: return float.NaN;
                }
            }
            set
            {
                switch (index)
                {
                    case 0:
                        Start = value;
                        break;
                    case 1:
                        End = value;
                        break;
                }
            }
        }

        public FogAnimationFrame(float start, float end, Vector3 color, FogType type)
        {
            Start = start;
            End = end;
            Color = color;
            Type = type;
            Index = 0;
            hasS = hasE = false;
        }

        public float Index;
        private const int len = 6;
        private static readonly string empty = new string('_', len);

        public override string ToString()
        {
            return
                $"[{(Index + 1).ToString().PadLeft(5)}] StartZ={(!hasS ? empty : Start.ToString().TruncateAndFill(len, ' '))}, EndZ={(!hasE ? empty : End.ToString().TruncateAndFill(len, ' '))}";
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct GLSLLightFrame
    {
        public static readonly GLSLLightFrame Empty;

        //These three vars are used by both specular and diffuse
        public int Enabled; //Don't apply this light if 0
        public Vector3 Position;
        public Vector3 Direction;

        //Diffuse
        public Vector4 DiffColor;
        public Vector3 DiffA; //Attenuation A
        public Vector3 DiffK; //Attenuation K

        //Specular (a different light, but uses the same start, direction, enable, and color/alpha enable)
        public int SpecEnabled;

        //public Vector3 SpecA; //Attenuation A = constant 0,0,1
        public Vector3 SpecK; //Attenuation K
        public Vector4 SpecColor;

        public GLSLLightFrame(
            bool enabled,
            LightType type,
            Vector3 pos,
            Vector3 aim,
            Vector4 color,
            Vector3 distCoefs,
            Vector3 spotCoefs,
            bool specEnabled,
            Vector4 specColor,
            Vector3 distCoefsSpec)
        {
            Enabled = enabled ? 1 : 0;
            switch (type)
            {
                case LightType.Point:
                    Position = pos;
                    Direction = new Vector3();
                    break;
                case LightType.Directional:
                    Position = (pos - aim) * 1e10f;
                    Direction = new Vector3();
                    break;
                case LightType.Spotlight:
                    Position = pos;
                    Direction = (pos - aim).Normalize();
                    break;
                default:
                    Position = new Vector3();
                    Direction = new Vector3();
                    break;
            }

            DiffColor = color;
            DiffK = distCoefs;
            DiffA = spotCoefs;

            SpecEnabled = specEnabled ? 1 : 0;
            SpecColor = specColor;
            SpecK = distCoefsSpec;
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct LightAnimationFrame
    {
        public static readonly LightAnimationFrame Empty;

        public Vector3 Start;
        public Vector3 End;
        public float RefDist;
        public float RefBright;
        public float SpotCutoff;
        public float SpotBright;

        public bool Enabled;

        public Vector4 Color;
        public Vector4 SpecularColor;

        public bool hasSx;
        public bool hasSy;
        public bool hasSz;

        public bool hasEx;
        public bool hasEy;
        public bool hasEz;

        public bool hasSC;
        public bool hasSB;
        public bool hasRD;
        public bool hasRB;

        public bool HasKeys => hasSx || hasSy || hasSz || hasEx || hasEy || hasEz || hasSC || hasSB || hasRD || hasRB;

        public void SetBools(int index, bool val)
        {
            switch (index)
            {
                case 0:
                    hasSx = val;
                    break;
                case 1:
                    hasSy = val;
                    break;
                case 2:
                    hasSz = val;
                    break;
                case 3:
                    hasEx = val;
                    break;
                case 4:
                    hasEy = val;
                    break;
                case 5:
                    hasEz = val;
                    break;
                case 6:
                    hasRD = val;
                    break;
                case 7:
                    hasRB = val;
                    break;
                case 8:
                    hasSC = val;
                    break;
                case 9:
                    hasSB = val;
                    break;
            }
        }

        public void ResetBools()
        {
            hasEx = hasEy = hasEz =
                hasSx = hasSy = hasSz =
                    hasSC = hasSB = hasRD = hasRB = false;
        }

        public float this[int index]
        {
            get
            {
                switch (index)
                {
                    case 0:  return Start._x;
                    case 1:  return Start._y;
                    case 2:  return Start._z;
                    case 3:  return End._x;
                    case 4:  return End._y;
                    case 5:  return End._z;
                    case 6:  return RefDist;
                    case 7:  return RefBright;
                    case 8:  return SpotCutoff;
                    case 9:  return SpotBright;
                    default: return float.NaN;
                }
            }
            set
            {
                switch (index)
                {
                    case 0:
                        Start._x = value;
                        break;
                    case 1:
                        Start._y = value;
                        break;
                    case 2:
                        Start._z = value;
                        break;
                    case 3:
                        End._x = value;
                        break;
                    case 4:
                        End._y = value;
                        break;
                    case 5:
                        End._z = value;
                        break;
                    case 6:
                        RefDist = value;
                        break;
                    case 7:
                        RefBright = value;
                        break;
                    case 8:
                        SpotCutoff = value;
                        break;
                    case 9:
                        SpotBright = value;
                        break;
                }
            }
        }

        public LightAnimationFrame(Vector3 start, Vector3 end, float sc, float sb, float rd, float rb, bool enabled)
        {
            Start = start;
            End = end;
            SpotCutoff = sc;
            SpotBright = sb;
            RefDist = rd;
            RefBright = rb;
            Enabled = enabled;
            Color = new Vector4();
            SpecularColor = new Vector4();
            Index = 0;
            hasSx = hasSy = hasSz = hasEx = hasEy = hasEz = hasSC = hasSB = hasRD = hasRB = false;
        }

        public float Index;
        private const int len = 6;
        private static readonly string empty = new string('_', len);

        public override string ToString()
        {
            return
                $"[{(Index + 1).ToString().PadLeft(5)}] Start=({(!hasSx ? empty : Start._x.ToString().TruncateAndFill(len, ' '))},{(!hasSy ? empty : Start._y.ToString().TruncateAndFill(len, ' '))},{(!hasSz ? empty : Start._z.ToString().TruncateAndFill(len, ' '))}), End=({(!hasEx ? empty : End._x.ToString().TruncateAndFill(len, ' '))},{(!hasEy ? empty : End._y.ToString().TruncateAndFill(len, ' '))},{(!hasEz ? empty : End._z.ToString().TruncateAndFill(len, ' '))}), SC={(!hasSC ? empty : SpotCutoff.ToString().TruncateAndFill(len, ' '))}, SB={(!hasSB ? empty : SpotBright.ToString().TruncateAndFill(len, ' '))} RD={(!hasRD ? empty : RefDist.ToString().TruncateAndFill(len, ' '))}, RB={(!hasRB ? empty : RefBright.ToString().TruncateAndFill(len, ' '))}";
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct CameraAnimationFrame
    {
        public static readonly CameraAnimationFrame Empty;

        public Vector3 Pos;
        public float Aspect;
        public float NearZ;
        public float FarZ;
        public Vector3 Rot;
        public Vector3 Aim;
        public float Twist;
        public float FovY;
        public float Height;

        public bool hasPx;
        public bool hasPy;
        public bool hasPz;

        public bool hasRx;
        public bool hasRy;
        public bool hasRz;

        public bool hasAx;
        public bool hasAy;
        public bool hasAz;

        public bool hasT;
        public bool hasF;
        public bool hasH;
        public bool hasA;
        public bool hasNz;
        public bool hasFz;

        public bool HasKeys => hasPx || hasPy || hasPz || hasRx || hasRy || hasRz || hasAx || hasAy || hasAz || hasT ||
                               hasF || hasH || hasA || hasNz || hasFz;

        public void SetBools(int index, bool val)
        {
            switch (index)
            {
                case 0:
                    hasPx = val;
                    break;
                case 1:
                    hasPy = val;
                    break;
                case 2:
                    hasPz = val;
                    break;
                case 3:
                    hasA = val;
                    break;
                case 4:
                    hasNz = val;
                    break;
                case 5:
                    hasFz = val;
                    break;
                case 6:
                    hasRx = val;
                    break;
                case 7:
                    hasRy = val;
                    break;
                case 8:
                    hasRz = val;
                    break;
                case 9:
                    hasAx = val;
                    break;
                case 10:
                    hasAy = val;
                    break;
                case 11:
                    hasAz = val;
                    break;
                case 12:
                    hasT = val;
                    break;
                case 13:
                    hasF = val;
                    break;
                case 14:
                    hasH = val;
                    break;
            }
        }

        public void ResetBools()
        {
            hasRx = hasRy = hasRz =
                hasPx = hasPy = hasPz =
                    hasAx = hasAy = hasAz =
                        hasT = hasF = hasH =
                            hasA = hasNz = hasFz = false;
        }

        public float this[int index]
        {
            get
            {
                switch (index)
                {
                    case 0:  return Pos._x;
                    case 1:  return Pos._y;
                    case 2:  return Pos._z;
                    case 3:  return Aspect;
                    case 4:  return NearZ;
                    case 5:  return FarZ;
                    case 6:  return Rot._x;
                    case 7:  return Rot._y;
                    case 8:  return Rot._z;
                    case 9:  return Aim._x;
                    case 10: return Aim._y;
                    case 11: return Aim._z;
                    case 12: return Twist;
                    case 13: return FovY;
                    case 14: return Height;
                    default: return float.NaN;
                }
            }
            set
            {
                switch (index)
                {
                    case 0:
                        Pos._x = value;
                        break;
                    case 1:
                        Pos._y = value;
                        break;
                    case 2:
                        Pos._z = value;
                        break;
                    case 3:
                        Aspect = value;
                        break;
                    case 4:
                        NearZ = value;
                        break;
                    case 5:
                        FarZ = value;
                        break;
                    case 6:
                        Rot._x = value;
                        break;
                    case 7:
                        Rot._y = value;
                        break;
                    case 8:
                        Rot._z = value;
                        break;
                    case 9:
                        Aim._x = value;
                        break;
                    case 10:
                        Aim._y = value;
                        break;
                    case 11:
                        Aim._z = value;
                        break;
                    case 12:
                        Twist = value;
                        break;
                    case 13:
                        FovY = value;
                        break;
                    case 14:
                        Height = value;
                        break;
                }
            }
        }

        public Vector3 GetRotation(SCN0CameraType type)
        {
            if (type == SCN0CameraType.Rotate)
            {
                return Rot;
            }

            return GetRotationMatrix(type).GetAngles();
        }

        public Matrix GetRotationMatrix(SCN0CameraType type)
        {
            if (type == SCN0CameraType.Rotate)
            {
                return Matrix.RotationMatrix(Rot);
            }

            return Matrix.RotationMatrix(Aim.LookatAngles(Pos) * Maths._rad2degf) * Matrix.RotationAboutZ(Twist);
        }

        public CameraAnimationFrame(Vector3 pos, Vector3 rot, Vector3 aim, float t, float f, float h, float a, float nz,
                                    float fz)
        {
            Pos = pos;
            Rot = rot;
            Aim = aim;
            Twist = t;
            FovY = f;
            Height = h;
            Aspect = a;
            NearZ = nz;
            FarZ = fz;
            Index = 0;
            hasRx = hasRy = hasRz =
                hasPx = hasPy = hasPz =
                    hasAx = hasAy = hasAz =
                        hasT = hasF = hasH =
                            hasA = hasNz = hasFz = false;
        }

        public float Index;
        private const int len = 6;
        private static readonly string empty = new string('_', len);

        public override string ToString()
        {
            return
                $"[{(Index + 1).ToString().PadLeft(5)}] Pos=({(!hasPx ? empty : Pos._x.ToString().TruncateAndFill(len, ' '))},{(!hasPy ? empty : Pos._y.ToString().TruncateAndFill(len, ' '))},{(!hasPz ? empty : Pos._z.ToString().TruncateAndFill(len, ' '))}), Rot=({(!hasRx ? empty : Rot._x.ToString().TruncateAndFill(len, ' '))},{(!hasRy ? empty : Rot._y.ToString().TruncateAndFill(len, ' '))},{(!hasRz ? empty : Rot._z.ToString().TruncateAndFill(len, ' '))}), Aim=({(!hasAx ? empty : Aim._x.ToString().TruncateAndFill(len, ' '))},{(!hasAy ? empty : Aim._y.ToString().TruncateAndFill(len, ' '))},{(!hasAz ? empty : Aim._z.ToString().TruncateAndFill(len, ' '))}), Twist={(!hasT ? empty : Twist.ToString().TruncateAndFill(len, ' '))}, FovY={(!hasF ? empty : FovY.ToString().TruncateAndFill(len, ' '))}, Height={(!hasH ? empty : Height.ToString().TruncateAndFill(len, ' '))}, Aspect={(!hasA ? empty : Aspect.ToString().TruncateAndFill(len, ' '))}, NearZ={(!hasNz ? empty : NearZ.ToString().TruncateAndFill(len, ' '))}, FarZ={(!hasFz ? empty : FarZ.ToString().TruncateAndFill(len, ' '))}";
        }
    }
}