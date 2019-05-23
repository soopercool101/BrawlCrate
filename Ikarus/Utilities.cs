using System;
using System.Collections.Generic;
using System.Linq;

namespace Ikarus
{
    public static class Util
    {
        public const float ScalarFac = 60000f;
        public static float UnScalar(int value)
        {
            return (float)value / 60000f;
        }
        public static int Scalar(float value)
        {
            if (value * 60000f < (float)int.MaxValue) return Convert.ToInt32(value * 60000f); else return int.MaxValue;
        }

        const long S_WORD = 0x4;
        const long S_BLOCK = 0x8;

        public static string TruncateLeft(string strIn, int totalWidth)
        {
            if (totalWidth <= 0) return "";
            if (totalWidth > strIn.Length) return strIn;
            return strIn.Substring(strIn.Length - totalWidth);
        }

        public static long ToWord(long val) { return val * S_WORD; }
        public static long FromWord(long val) { return val / S_WORD; }
        public static long ToBlock(long val) { return val * S_BLOCK; }
        public static long FromBlock(long val) { return val / S_BLOCK; }
        public static long FromWordRU(long val) { return FromWord(RoundUp(val, S_WORD)); }
        public static long FromWordRD(long val) { return FromWord(RoundDown(val, S_WORD)); }
        public static long FromBlockRU(long val) { return FromBlock(RoundUp(val, S_BLOCK)); }
        public static long FromBlockRD(long val) { return FromBlock(RoundDown(val, S_BLOCK)); }

        public static string Hex(int val) { return TruncateLeft(val.ToString("X"), 8); }
        public static string Hex(long val) { return TruncateLeft(val.ToString("X"), 8); }
        public static string Hex8(int val) { return TruncateLeft(val.ToString("X"), 8).PadLeft(8, '0'); }
        public static string Hex8(long val) { return TruncateLeft(val.ToString("X"), 8).PadLeft(8, '0'); }
        public static int UnHex(string val) { if (val.StartsWith("0x")) val = val.Substring(2); return int.Parse(val, System.Globalization.NumberStyles.HexNumber); }

        public static string WordH(string val, int wordNum)
        {
            if (wordNum >= 2) return "";
            return TruncateLeft(val, 8).PadLeft(8, '0').Substring(wordNum * 4, 4);
        }

        public static string WordB(string val, int byteNum)
        {
            if (byteNum >= 4) return "";
            return TruncateLeft(val, 8).PadLeft(8, '0').Substring(byteNum * 2, 2);
        }

        public static long Float(float val)
        {
            if (val == 0) return 0;
            long sign = (val >= 0 ? 0 : 8);
            long exponent = 0x7F;
            float mantissa = Math.Abs(val);

            if (mantissa > 1)
                while (mantissa > 2)
                { mantissa /= 2; exponent++; }
            else
                while (mantissa < 1)
                { mantissa *= 2; exponent--; }
            mantissa -= 1;
            mantissa *= (float)Math.Pow(2, 23);

            return (
                  sign * 0x10000000
                + exponent * 0x800000
                + (long)mantissa);
        }
        public static float UnFloat(long val)
        {
            if (val == 0) return 0;
            float sign = ((val & 0x80000000) == 0 ? 1 : -1);
            int exponent = ((int)(val & 0x7F800000) / 0x800000) - 0x7F;
            float mantissa = (val & 0x7FFFFF);
            long mantissaBits = 23;

            if (mantissa != 0)
                while (((long)mantissa & 0x1) != 1)
                { mantissa /= 2; mantissaBits--; }
            mantissa /= (float)Math.Pow(2, mantissaBits);
            mantissa += 1;

            mantissa *= (float)Math.Pow(2, exponent);
            return mantissa *= sign;
        }

        public static long RoundUp(long val, long factor) { return val + (factor - 1) - (val + (factor - 1)) % factor; }
        public static long RoundDown(long val, long factor) { return val - val % factor; }

        public static string GetComparisonSign(long value)
        {
            switch (value)
            {
                case 0: return "<";
                case 1: return "<=";
                case 2: return "==";
                case 3: return "!=";
                case 4: return ">=";
                case 5: return ">";
                default: return "(" + value + ")";
            }
        }

        public static int TabUpEvents(uint eventId)
        {
            switch (eventId)
            {
                case 0x00040100:
                case 0x000A0100:
                case 0x000A0200:
                case 0x000A0300:
                case 0x000A0400:
                case 0x000B0100:
                case 0x000B0200:
                case 0x000B0300:
                case 0x000B0400:
                case 0x000C0100:
                case 0x000C0200:
                case 0x000C0300:
                case 0x000C0400:
                case 0x000D0100:
                case 0x000D0200:
                case 0x000D0400:
                case 0x000E0000:
                case 0x00110100: //Case
                case 0x00120000: //Default Case
                    return 1;
                case 0x00100200: //Switch
                    return 2;
                default:
                    return 0;
            }
        }

        public static int TabDownEvents(uint eventId)
        {
            switch (eventId)
            {
                case 0x00050000:
                case 0x000B0100:
                case 0x000B0200:
                case 0x000B0300:
                case 0x000B0400:
                case 0x000C0100:
                case 0x000C0200:
                case 0x000C0300:
                case 0x000C0400:
                case 0x000D0100:
                case 0x000D0200:
                case 0x000D0300:
                case 0x000D0400:
                case 0x000E0000:
                case 0x000F0000:
                case 0x00110100: //Case
                case 0x00120000: //Default Case
                    return 1;
                case 0x00130000: //End Switch 
                    return 2;
                default:
                    return 0;
            }
        }

        //Find the first occuring instance of the passed character.
        public static int FindFirst(string str, int begin, char chr)
        {
            for (int i = begin; i < str.Length; i++)
                if (str[i] == chr) return i;
            return -1;
        }

        //Find the last occuring instance of the passed character.
        public static int FindLast(string str, int begin, char chr)
        {
            for (int i = str.Length - 1; i >= begin; i--)
                if (str[i] == chr) return i;
            return -1;
        }

        //Find the number of instances of the passed character.
        public static int FindCount(string str, int begin, char chr)
        {
            int x = 0;
            for (int i = begin; i < str.Length; i++)
                if (str[i] == chr) x++;
            return x;
        }

        //Find the indices of instances of the passed character.
        public static int[] IndiciesOf(string str, int begin, char chr)
        {
            List<int> indices = new List<int>();
            for (int i = begin; i < str.Length; i++)
                if (str[i] == chr) indices.Add(i);
            return indices.ToArray();
        }

        //Find the first occuring instance of any of the passed characters.
        public static int FindFirstOf(string str, int begin, char[] chr, ref char chrFound)
        {
            int result = -1;
            chrFound = '\0';
            for (int i = 0; i < chr.Length; i++)
            {
                int temp = FindFirst(str, begin, chr[i]);

                if (temp != -1 && (temp < result || result == -1))
                {
                    result = temp;
                    chrFound = chr[i];
                }
            }
            return result;
        }

        //FindFirst ignoring any pairs of () and anything contained inside.
        public static int FindFirstIgnoreNest(string str, int begin, char chr)
        {
            if (chr == '(') return FindFirst(str, begin, chr);
            char[] searchCharacters = { chr, '(', ')' };
            char chrResult = '\0';
            int searchResult = begin;
            int nested = 0;

            do
            {
                if (chrResult == ')' && nested > 0) nested--;
                searchResult = FindFirstOf(str, searchResult, searchCharacters, ref chrResult);
                if (chrResult == '(') nested++;
                searchResult++;
            } while ((nested > 0 || chrResult != chr) && chrResult != '\0');
            searchResult--;

            return searchResult;
        }

        //FindFirstOf ignoring any paris of () and anything contained inside.
        public static int FindFirstOfIgnoreNest(string str, int begin, char[] chr, ref char chrFound)
        {
            int result = -1;
            chrFound = '\0';
            for (int i = 0; i < chr.Length; i++)
            {
                int temp = FindFirstIgnoreNest(str, begin, chr[i]);

                if (temp != -1 && (temp < result || result == -1))
                {
                    result = temp;
                    chrFound = chr[i];
                }
            }
            return result;
        }

        //Find the first instance that is not the character passed.
        public static int FindFirstNot(string str, int begin, char chr)
        {
            for (int i = begin; i < str.Length; i++)
                if (str[i] != chr) return i;

            return -1;
        }

        //Find the first instance that is not the character passed.
        public static int FindFirstNotDual(string str, int begin, char chr1, char chr2)
        {
            for (int i = begin; i < str.Length; i++)
                if (str[i] != chr1 && str[i] != chr2) return i;

            return -1;
        }

        //Return the string passed with the new string insterted into the specified position.
        public static string InsString(string str, string insString, int begin)
        {
            return str.Substring(0, begin) + insString + str.Substring(begin);
        }

        //Return the string passed minus the substring specified.
        public static string DelSubstring(string str, int begin, int len)
        {
            return str.Substring(0, begin) + str.Substring(begin + len);
        }

        //Delete any whitespace before and after the string.
        public static string ClearWhiteSpace(string str)
        {
            int whiteSpace = FindFirstNot(str, 0, ' ');
            if (whiteSpace > 0)
                str = DelSubstring(str, 0, whiteSpace);

            whiteSpace = -1;
            for (int i = str.Length - 1; i >= 0; i--)
                if (str[i] == ' ') whiteSpace = i;
                else break;
            if (whiteSpace != -1)
                str = DelSubstring(str, whiteSpace, str.Length - whiteSpace);

            return str;
        }

        public enum HitboxType { Typeless, Head, Body, Butt, Hand, Elbow, Foot, Knee, Throwing, Weapon, Sword, Hammer, Explosive, Spin, Bite, Magic, PK, Bow, Type18, Bat, Umbrella, Pikmin, Water, Whip, Tail, Energy, Type26, Type27, Type28, Type29, Type30, Type31 };
        public enum HitboxEffect { Normal, None, Slash, Electric, Freezing, Flame, Coin, Reverse, Slip, Sleep, Effect10, Bury, Stun, Effect13, Flower, Effect15, Effect16, Grass, Water, Darkness, Paralyze, Aura, Plunge, Down, Flinchless, Effect25, Effect26, Effect27, Effect28, Effect29, Effect30, Effect31 };
        public enum HitboxSFX { None, SFX1, Unique, SFX3, SFX4, SFX5, SFX6, SFX7, WeakPunch, MediumPunch, StrongPunch, SFX11, WeakPunch2, MediumPunch2, StrongPunch2, SFX15, WeakKick, MediumKick, StrongKick, SFX19, WeakKick2, MediumKick2, StrongKick2, SFX23, WeakSlash, MediumSlash, StrongSlash, SFX27, WeakSlash2, MediumSlash2, StrongSlash2, SFX31, Coin1, Coin2, Coin3, Coin4, Coin5, Coin6, Coin7, Coin8, MediumClonk, StrongClonk, Ping, SFX43, WeakClonk2, MediumClock2, Ping2, SFX47, WeakPaperHit, MediumPaperHit, StrongPaperHit, SFX51, SFX52, SFX53, SFX54, SFX55, WeakShock, MediumShock, StrongShock, SFX59, SFX60, SFX61, SFX62, SFX63, WeakBurn, MediumBurn, StrongBurn, SFX67, SFX68, SFX69, SFX70, SFX71, WeakSplash, MediumSplash, StrongSplash, SFX75, SFX76, SFX77, SFX78, SFX79, SFX80, SFX81, SFX82, SFX83, SFX84, SFX85, SFX86, SFX87, SmallExplosion, MediumExplosion, LargeExplosion, HugeExplosion, SFX92, SFX93, SFX94, SFX95, SFX96, SFX97, SFX98, SFX99, SFX100, SFX101, SFX102, SFX103, WeakThud, MediumThud, StrongThud, HugeThud, SFX108, SFX109, SFX110, SFX111, WeakSlam, MediumSlam, StrongSlam, HugeSlam, SFX116, SFX117, SFX118, SFX119, WeakThwomp, MediumThwomp, StrongThwomp, HugeThwomp, SFX124, SFX125, SFX126, SFX127, WeakMagicZap, MediumMagicZap, StrongMagicZap, HugeMagicZap, SFX132, SFX133, SFX134, SFX135, WeakShell, MediumShell, StrongShell, SFX139, SFX140, SFX141, SFX142, SFX143, WeakSlap, SFX145, StrongSlap, SFX147, SFX148, SFX149, SFX150, SFX151, FryingPan, SFX153, SFX154, SFX155, SFX156, SFX157, SFX158, SFX159, SFX160, WeakGolfClub, StrongGolfClub, SFX163, SFX164, SFX165, SFX166, SFX167, WeakRacket, SFX169, StrongRacket, SFX171, SFX172, SFX173, SFX174, SFX175, WeakAura, MediumAura, StrongAura, SFX179, SFX180, SFX181, SFX182, SFX183, SFX184, SFX185, SFX186, SFX187, SFX188, SFX189, SFX190, SFX191, SFX192, SFX193, SFX194, SFX195, SFX196, SFX197, SFX198, SFX199, SFX200, SFX201, SFX202, SFX203, SFX204, SFX205, SFX206, SFX207, SFX208, SFX209, SFX210, SFX211, SFX212, SFX213, SFX214, SFX215, SFX216, SFX217, BatCrack, SFX219, SFX220, SFX221, SFX222, SFX223, SFX224, SFX225, SFX226, SFX227, SFX228, SFX229, SFX230, SFX231, SFX232, SFX233, SFX234, SFX235, SFX236, SFX237, SFX238, SFX239, SFX240, SFX241, SFX242, SFX243, SFX244, SFX245, SFX246, SFX247, SFX248, SFX249, SFX250, SFX251, SFX252, SFX253, SFX254, SFX255 };
        public enum DrawStyle { SSB64, Melee, Brawl }

        public static Vector3 GetTypeColor(HitboxType t)
        {
            switch (t)
            {
                case HitboxType.Typeless: return new Vector3(0x9f, 0x9f, 0x9f);
                case HitboxType.Head: return new Vector3(0xff, 0x00, 0x00);
                case HitboxType.Body: return new Vector3(0xbf, 0x3f, 0x00);
                case HitboxType.Butt: return new Vector3(0xbf, 0x5f, 0x00);
                case HitboxType.Hand: return new Vector3(0xff, 0xff, 0x00);
                case HitboxType.Elbow: return new Vector3(0xff, 0xdf, 0x00);
                case HitboxType.Foot: return new Vector3(0xff, 0x7f, 0x00);
                case HitboxType.Knee: return new Vector3(0xff, 0x9f, 0x00);
                case HitboxType.Throwing: return new Vector3(0xff, 0x00, 0xff);
                case HitboxType.Weapon: return new Vector3(0xff, 0xbf, 0x7f);
                case HitboxType.Sword: return new Vector3(0xbf, 0xbf, 0xbf);
                case HitboxType.Hammer: return new Vector3(0xff, 0x9f, 0x3f);
                case HitboxType.Explosive: return new Vector3(0xbf, 0x7f, 0x00);
                case HitboxType.Spin: return new Vector3(0x7f, 0xff, 0x00);
                case HitboxType.Bite: return new Vector3(0x00, 0xff, 0x7f);
                case HitboxType.Magic: return new Vector3(0xbf, 0x00, 0xbf);
                case HitboxType.PK: return new Vector3(0xbf, 0x3f, 0xbf);
                case HitboxType.Bow: return new Vector3(0xdf, 0xbf, 0x3f);
                case HitboxType.Bat: return new Vector3(0xaf, 0x8f, 0x1f);
                case HitboxType.Umbrella: return new Vector3(0xff, 0x7f, 0x7f);
                case HitboxType.Pikmin: return new Vector3(0x00, 0xbf, 0x00);
                case HitboxType.Water: return new Vector3(0x3f, 0x3f, 0xff);
                case HitboxType.Whip: return new Vector3(0xff, 0xdf, 0x7f);
                case HitboxType.Tail: return new Vector3(0x00, 0xff, 0x00);
                case HitboxType.Energy: return new Vector3(0x00, 0xff, 0xff);
                default: return new Vector3(0x7f, 0x7f, 0x7f);
            }
        }
        public static Vector3 GetEffectColor(HitboxEffect t)
        {
            switch (t)
            {
                case HitboxEffect.Normal: return new Vector3(0xaf, 0xaf, 0x7f);
                case HitboxEffect.None: return new Vector3(0x7f, 0x7f, 0x7f);
                case HitboxEffect.Slash: return new Vector3(0xdf, 0xdf, 0xdf);
                case HitboxEffect.Electric: return new Vector3(0xff, 0xff, 0x7f);
                case HitboxEffect.Freezing: return new Vector3(0x7f, 0xbf, 0xff);
                case HitboxEffect.Flame: return new Vector3(0xff, 0x3f, 0x00);
                case HitboxEffect.Coin: return new Vector3(0xdf, 0xdf, 0x00);
                case HitboxEffect.Reverse: return new Vector3(0x7f, 0xdf, 0xdf);
                case HitboxEffect.Slip: return new Vector3(0x7f, 0x7f, 0x00);
                case HitboxEffect.Sleep: return new Vector3(0xff, 0x3f, 0xff);
                case HitboxEffect.Bury: return new Vector3(0x7f, 0x00, 0x00);
                case HitboxEffect.Stun: return new Vector3(0xbf, 0x00, 0x00);
                case HitboxEffect.Flower: return new Vector3(0x7f, 0x9f, 0x00);
                case HitboxEffect.Grass: return new Vector3(0x00, 0x7f, 0x00);
                case HitboxEffect.Water: return new Vector3(0x00, 0x00, 0xff);
                case HitboxEffect.Darkness: return new Vector3(0x3f, 0x00, 0x3f);
                case HitboxEffect.Paralyze: return new Vector3(0xbf, 0xbf, 0x3f);
                case HitboxEffect.Aura: return new Vector3(0x00, 0x7f, 0xff);
                case HitboxEffect.Plunge: return new Vector3(0x3f, 0x00, 0x00);
                case HitboxEffect.Down: return new Vector3(0x5f, 0x5f, 0x5f);
                case HitboxEffect.Flinchless: return new Vector3(0x3f, 0x3f, 0x3f);
                default: return new Vector3(0x7f, 0x7f, 0x7f);
            }
        }
    }

    public class EventInformation
    {
        public long _id;
        public string _name;
        public string _compressedName;
        public string _description;
        public string _syntax;
        public string[] _paramNames;
        public string[] _paramDescs;
        public long[] _defaultParams;
        public Dictionary<int, List<string>> _enums;

        public EventInformation() { _defaultParams = new long[0]; _syntax = ""; }
        public EventInformation(long id, string name, string description, string[] paramNames, string[] paramDesc)
        {
            _id = id;
            _name = name;
            _compressedName = new string(Manager.TextInfo.ToTitleCase(_name).Where(c => !char.IsWhiteSpace(c)).ToArray());
            _description = description;
            _syntax = "";
            _paramNames = paramNames;
            _paramDescs = paramDesc;
            _defaultParams = new long[0];
            _enums = new Dictionary<int, List<string>>();
        }
        public EventInformation(long id, string name, string description, string[] paramNames, string[] paramDesc, string syntax, long[] dfltParams)
        {
            _id = id;
            _name = name;
            _compressedName = new string(Manager.TextInfo.ToTitleCase(_name).Where(c => !char.IsWhiteSpace(c)).ToArray());
            _description = description;
            _syntax = syntax;
            _paramNames = paramNames;
            _paramDescs = paramDesc;
            _defaultParams = dfltParams;
            _enums = new Dictionary<int, List<string>>();
        }

        public void SetDefaults(string s)
        {
            if (s == null) return;

            Array.Resize<long>(ref _defaultParams, s.Length);
            for (int i = 0; i < s.Length; i++)
            {
                try { _defaultParams[i] = long.Parse(s.Substring(i, 1)); }
                catch { _defaultParams[i] = 0; }
            }
        }

        public long GetDefaults(int i)
        {
            if (i >= _defaultParams.Length) return 0;
            if (_defaultParams[i] > 6) return 0;
            return _defaultParams[i];
        }

        public override string ToString() { return _name; }
    }

    public enum ParamType : int
    {
        Value = 0,
        Scalar = 1,
        Offset = 2,
        Boolean = 3,
        File = 4,
        Variable = 5,
        Requirement = 6
    }
}
