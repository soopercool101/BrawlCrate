using BrawlLib.SSBBTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Ikarus.MovesetFile
{
    public abstract class OffsetHolder
    {
        protected VoidPtr _address;
        protected DataSection _data;

        public void Parse(DataSection node, VoidPtr address)
        {
            _data = node;
            _address = address;

            TryGetValues();
            OnParse();

            int i = 0;
            foreach (var a in _data._articles)
                a._index = i++;

            _data._articles = _data._articles.OrderBy(x => x._offset).ToList();

            i = 0;
            foreach (var a in _data._paramLists)
                a._index = i++;

            _data._paramLists = _data._paramLists.OrderBy(x => x._offset).ToList();
        }

        List<VoidPtr> _lookupAddresses;

        protected void Lookup(VoidPtr address)
        {
            _lookupAddresses.Add(address);
        }

        protected void Lookup(List<VoidPtr> values)
        {
            _lookupAddresses.AddRange(values);
        }

        public List<VoidPtr> Write(DataSection node, VoidPtr address)
        {
            _lookupAddresses = new List<VoidPtr>();
            _data = node;
            _address = address;

            TrySetValues();
            OnWrite();

            return _lookupAddresses;
        }

        protected virtual Type OffsetsType { get { return null; } }

        //Used to parse extra things that are not parameters or articles
        protected virtual void OnParse() { }
        //Used to write extra things that are not parameters or articles
        protected virtual void OnWrite() { }

        public int Size { get { return OffsetsType == null ? 0 : Marshal.SizeOf(OffsetsType); } }
        
        protected unsafe int Get(int i)
        {
            return ((bint*)_address)[i];
        }
        protected unsafe SakuraiEntryNode GetNode<T>(int i) where T : SakuraiEntryNode
        {
            return _data.Parse<T>(Get(i));
        }
        protected unsafe ArticleNode GetArticle(int i)
        {
            return _data.Parse<ArticleNode>(Get(i));
        }
        protected unsafe RawParamList GetParamList(int i)
        {
            return _data.Parse<RawParamList>(Get(i));
        }
        protected void GetArticles(int startIndex, int count)
        {
            for (int i = startIndex; i < count; i++)
                _data._articles.Add(GetArticle(i));
        }
        protected void GetParamLists(int startIndex, int count)
        {
            for (int i = startIndex; i < count; i++)
                _data._paramLists.Add(GetParamList(i));
        }
        protected unsafe void TryGetValues()
        {
            if (OffsetsType == null)
                return;

            FieldInfo[] fields = OffsetsType.GetFields(BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic);

            int i = 0;
            foreach (FieldInfo info in fields)
            {
                if (info.Name.StartsWith("_article") && !info.Name.Contains("count"))
                    _data._articles.Add(_data.Parse<ArticleNode>(*(bint*)(_address + i)));
                else if (info.Name.StartsWith("_params") && !info.Name.Contains("count"))
                    _data._paramLists.Add(_data.Parse<RawParamList>(*(bint*)(_address + i)));
                i += Marshal.SizeOf(info.FieldType);
            }
        }
        protected unsafe void TrySetValues()
        {
            if (OffsetsType == null)
                return;

            FieldInfo[] fields = OffsetsType.GetFields(BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic);
            int i = 0;
            foreach (FieldInfo info in fields)
            {
                if (info.Name.StartsWith("_article") && !info.Name.Contains("count"))
                {
                    foreach (var a in _data._articles)
                        if (a.Index == i)
                        {
                            Set(i, a);
                            break;
                        }
                }
                else if (info.Name.StartsWith("_params") && !info.Name.Contains("count"))
                {
                    foreach (var a in _data._paramLists)
                        if (a.Index == i)
                        {
                            Set(i, a);
                            break;
                        }
                }
                i++;
            }
        }
        protected unsafe void Set(int i, SakuraiEntryNode node)
        {
            ((bint*)_address)[i] = node.RebuildOffset;
        }
    }
    public unsafe class ExtraDataOffsets
    {
        public static void ParseCharacter(CharName name, DataSection data, VoidPtr address)
        {
            OffsetHolder o = ExtraDataOffsets.GetOffsets(name);
            if (o != null)
                o.Parse(data, address);
        }

        //How to parse the articles for a character:
        //Using Brawlbox v0.68b, you can view a character's extra data offsets (header data specific to that character)
        //by opening the character's moveset file and going to MoveDef_FitChar->Sections->data
        //Go the properties and view the ExtraOffsets collection.
        //Use this to set up the struct for the character accordingly (find their struct by name below).
        //You can view the int offset of each article in the Articles list under data,
        //So just match those offsets to the list, get the index and use it to parse them.
        //Articles and parameter lists names must start with _article or _params
        public static OffsetHolder GetOffsets(CharName character)
        {
            Type[] types = Assembly.GetExecutingAssembly().GetTypes().Where(
                t => String.Equals(t.FullName,
                    "Ikarus.MovesetFile.ExtraDataOffsets+" + character.ToString(), 
                    StringComparison.Ordinal)).ToArray();

            if (types.Length != 0)
                return Activator.CreateInstance(types[0]) as OffsetHolder;
            
            return null;
        }

        public class CaptainFalcon : OffsetHolder
        {
            protected override Type OffsetsType { get { return typeof(Offsets); } }

            [StructLayout(LayoutKind.Sequential, Pack = 1)]
            public struct Offsets
            {
                buint _params0;
                buint _params1;
                buint _params2;
                buint _params3;
                buint _params4;
                buint _params5;
                buint _article0;
                buint _article1;
            }
        }

        public class KingDedede : OffsetHolder
        {
            protected override Type OffsetsType { get { return typeof(Offsets); } }

            [StructLayout(LayoutKind.Sequential, Pack = 1)]
            public struct Offsets
            {
                buint _params0;
                buint _params1;
                buint _params2;
                buint _params3;
                sListOffset _params4;
                sListOffset _article2;
                sListOffset _article0;
                sListOffset _article1;
                sListOffset _article3;
                buint _article4;
                buint _addAreaDataSet0; //points to ListOffset (8 bytes)
            }
        }

        public class DiddyKong : OffsetHolder
        {
            protected override Type OffsetsType { get { return typeof(Offsets); } }

            [StructLayout(LayoutKind.Sequential, Pack = 1)]
            public struct Offsets
            {

            }
        }
        
        public class DonkeyKong : OffsetHolder
        {
            protected override Type OffsetsType { get { return typeof(Offsets); } }

            [StructLayout(LayoutKind.Sequential, Pack = 1)]
            public struct Offsets
            {

            }
        }

        public class Falco : OffsetHolder
        {
            protected override Type OffsetsType { get { return typeof(Offsets); } }

            [StructLayout(LayoutKind.Sequential, Pack = 1)]
            public struct Offsets
            {

            }
        }

        public class Fox : OffsetHolder
        {
            protected override Type OffsetsType { get { return typeof(Offsets); } }

            [StructLayout(LayoutKind.Sequential, Pack = 1)]
            public struct Offsets
            {

            }
        }

        public class MrGameNWatch : OffsetHolder
        {
            protected override Type OffsetsType { get { return typeof(Offsets); } }

            [StructLayout(LayoutKind.Sequential, Pack = 1)]
            public struct Offsets
            {

            }
        }

        public class Ganondorf : OffsetHolder
        {
            protected override Type OffsetsType { get { return typeof(Offsets); } }

            [StructLayout(LayoutKind.Sequential, Pack = 1)]
            public struct Offsets
            {

            }
        }

        public class GigaBowser : OffsetHolder
        {
            protected override Type OffsetsType { get { return typeof(Offsets); } }

            [StructLayout(LayoutKind.Sequential, Pack = 1)]
            public struct Offsets
            {

            }
        }

        public class Ike : OffsetHolder
        {
            protected override Type OffsetsType { get { return typeof(Offsets); } }

            [StructLayout(LayoutKind.Sequential, Pack = 1)]
            public struct Offsets
            {

            }
        }

        public class Kirby : OffsetHolder
        {
            protected override Type OffsetsType { get { return typeof(Offsets); } }

            [StructLayout(LayoutKind.Sequential, Pack = 1)]
            public struct Offsets
            {

            }
        }

        public class Bowser : OffsetHolder
        {
            protected override Type OffsetsType { get { return typeof(Offsets); } }

            [StructLayout(LayoutKind.Sequential, Pack = 1)]
            public struct Offsets
            {

            }
        }

        public class Link : OffsetHolder
        {
            protected override Type OffsetsType { get { return typeof(Offsets); } }

            [StructLayout(LayoutKind.Sequential, Pack = 1)]
            public struct Offsets
            {
                buint _params1;
                buint _params3;
                buint _params4;
                buint _params2;
                buint _params5;
                buint _params0;
                buint _params6;
                sListOffset _hitDataList;
                uint _unk1; //0x90
                buint _unkCount;
                sListOffset _article0;
                sListOffset _article1;
                buint _article2;
                buint _article3;
                buint _article4;
                buint _article5;
                buint _article6;
            }
        }

        public class Lucario : OffsetHolder
        {
            protected override Type OffsetsType { get { return typeof(Offsets); } }

            [StructLayout(LayoutKind.Sequential, Pack = 1)]
            public struct Offsets
            {

            }
        }

        public class Lucas : OffsetHolder
        {
            protected override Type OffsetsType { get { return typeof(Offsets); } }

            [StructLayout(LayoutKind.Sequential, Pack = 1)]
            public struct Offsets
            {

            }
        }

        public class Luigi : OffsetHolder
        {
            protected override Type OffsetsType { get { return typeof(Offsets); } }

            [StructLayout(LayoutKind.Sequential, Pack = 1)]
            public struct Offsets
            {

            }
        }

        public class Mario : OffsetHolder
        {
            protected override Type OffsetsType { get { return typeof(Offsets); } }

            [StructLayout(LayoutKind.Sequential, Pack = 1)]
            public struct Offsets
            {
                buint _params2;
                buint _params3;
                buint _params0;
                buint _params4;
                buint _params1;
                buint _article1;
                buint _article0;
                buint _article2;
                buint _article3;
                buint _article4;
            }
        }

        public class Marth : OffsetHolder
        {
            protected override Type OffsetsType { get { return typeof(Offsets); } }

            [StructLayout(LayoutKind.Sequential, Pack = 1)]
            public struct Offsets
            {

            }
        }

        public class Metaknight : OffsetHolder
        {
            protected override Type OffsetsType { get { return typeof(Offsets); } }

            [StructLayout(LayoutKind.Sequential, Pack = 1)]
            public struct Offsets
            {

            }
        }

        public class Ness : OffsetHolder
        {
            protected override Type OffsetsType { get { return typeof(Offsets); } }

            [StructLayout(LayoutKind.Sequential, Pack = 1)]
            public struct Offsets
            {

            }
        }

        public class Peach : OffsetHolder
        {
            protected override Type OffsetsType { get { return typeof(Offsets); } }

            [StructLayout(LayoutKind.Sequential, Pack = 1)]
            public struct Offsets
            {
                buint _params0;
                buint _params1;
                buint _params2;
                buint _params3;
                buint _params4;
                buint _params5;
                sListOffset _hitDataList1;
                sListOffset _hitDataList2;
                buint _unknown; //0
                buint _article0;
                buint _article1;
                buint _article2;
            }
        }

        public class Pikachu : OffsetHolder
        {
            protected override Type OffsetsType { get { return typeof(Offsets); } }

            [StructLayout(LayoutKind.Sequential, Pack = 1)]
            public struct Offsets
            {

            }
        }

        public class Olimar : OffsetHolder
        {
            protected override Type OffsetsType { get { return typeof(Offsets); } }

            [StructLayout(LayoutKind.Sequential, Pack = 1)]
            public struct Offsets
            {

            }
        }

        public class Pit : OffsetHolder
        {
            protected override Type OffsetsType { get { return typeof(Offsets); } }

            [StructLayout(LayoutKind.Sequential, Pack = 1)]
            public struct Offsets
            {
                buint _params0;
                buint _params1;
                buint _params2;
                buint _params3;
                sListOffset _hitDataList1;
                uint _unknown;
                buint _specialHitDataList;
                buint _params4;
                buint _article0;
                buint _article1;
                buint _article2;
                buint _article3;
            }
        }

        public class Ivysaur : OffsetHolder
        {
            protected override Type OffsetsType { get { return typeof(Offsets); } }

            [StructLayout(LayoutKind.Sequential, Pack = 1)]
            public struct Offsets
            {

            }
        }

        public class Charizard : OffsetHolder
        {
            protected override Type OffsetsType { get { return typeof(Offsets); } }

            [StructLayout(LayoutKind.Sequential, Pack = 1)]
            public struct Offsets
            {

            }
        }

        public class PokemonTrainer : OffsetHolder
        {
            protected override Type OffsetsType { get { return typeof(Offsets); } }

            [StructLayout(LayoutKind.Sequential, Pack = 1)]
            public struct Offsets
            {

            }
        }

        public class Squirtle : OffsetHolder
        {
            protected override Type OffsetsType { get { return typeof(Offsets); } }

            [StructLayout(LayoutKind.Sequential, Pack = 1)]
            public struct Offsets
            {

            }
        }

        public class Popo : OffsetHolder
        {
            protected override Type OffsetsType { get { return typeof(Offsets); } }

            [StructLayout(LayoutKind.Sequential, Pack = 1)]
            public struct Offsets
            {

            }
        }

        public class Jigglypuff : OffsetHolder
        {
            protected override Type OffsetsType { get { return typeof(Offsets); } }

            [StructLayout(LayoutKind.Sequential, Pack = 1)]
            public struct Offsets
            {

            }
        }

        public class ROB : OffsetHolder
        {
            protected override Type OffsetsType { get { return typeof(Offsets); } }

            [StructLayout(LayoutKind.Sequential, Pack = 1)]
            public struct Offsets
            {
                buint _params0;
                buint _params1;
                buint _params2;
                buint _params3;
                sListOffset _params4;
                sListOffset _article0;
                sListOffset _article1;
                sListOffset _article2;
                buint _article3;
                buint _article4;
            }
        }

        public class Samus : OffsetHolder
        {
            protected override Type OffsetsType { get { return typeof(Offsets); } }

            [StructLayout(LayoutKind.Sequential, Pack = 1)]
            public struct Offsets
            {
                buint _params0;
                buint _params1;
                buint _params2;
                buint _params3;
                buint _params4;
                buint _params5;
                sListOffset _params6;
                sListOffset _article0;
                sListOffset _article1;
                sListOffset _article2;
                sListOffset _article3;
                sListOffset _article4;
                sListOffset _article5;
                buint _article6;
                buint _article7;
            }
        }

        public class Sheik : OffsetHolder
        {
            protected override Type OffsetsType { get { return typeof(Offsets); } }

            [StructLayout(LayoutKind.Sequential, Pack = 1)]
            public struct Offsets
            {
                buint _params0;
                buint _params1;
                buint _params2;
                buint _params3;
                buint _params4;
                buint _article0;
                buint _article1;
                buint _article2;
                buint _article3;
            }
        }

        public class Snake : OffsetHolder
        {
            protected override Type OffsetsType { get { return typeof(Offsets); } }

            [StructLayout(LayoutKind.Sequential, Pack = 1)]
            public struct Offsets
            {
                buint _params0;
                buint _params1;
                buint _params2;
                buint _params3;
                buint _params4;
                buint _article0;
                buint _article1;
                buint _article2;
                buint _article3;
                buint _article4;
                buint _article5;
                buint _article6;
                buint _article7;
                buint _article8;
                buint _article9;
                buint _article10;
                buint _article11;
                buint _article12;
                buint _article13;
                buint _article14;
            }
        }

        public class Sonic : OffsetHolder
        {
            protected override Type OffsetsType { get { return typeof(Offsets); } }

            [StructLayout(LayoutKind.Sequential, Pack = 1)]
            public struct Offsets
            {
                buint _params0;
                buint _params1;
                buint _params2;
                buint _params3;
                buint _params4;
                buint _params5;
                buint _article0;
                buint _article1;
            }
        }

        public class ZeroSuitSamus : OffsetHolder
        {
            protected override Type OffsetsType { get { return typeof(Offsets); } }

            [StructLayout(LayoutKind.Sequential, Pack = 1)]
            public struct Offsets
            {
                buint _params1;
                buint _params2;
                buint _params3;
                buint _params4;
                buint _article1;
                buint _article2;
                buint _article3;
                buint _article4;
                sListOffset _extraOffset8;
                sListOffset _params5;
            }
        }

        public class ToonLink : OffsetHolder
        {
            protected override Type OffsetsType { get { return typeof(Offsets); } }

            [StructLayout(LayoutKind.Sequential, Pack = 1)]
            public struct Offsets
            {
                buint _params1;
                buint _params3;
                buint _params4;
                buint _params2;
                buint _params5;
                buint _params0;
                buint _params7;
                buint _params6;
                buint _unkCount0;
                uint _unk; //0x90
                buint _unkCount1;
                sListOffset _article0;
                sListOffset _article1;
                buint _article2;
                buint _article3;
                buint _article4;
                buint _article5;
                buint _article6;
            }
        }

        public class Wario : OffsetHolder
        {
            protected override Type OffsetsType { get { return typeof(Offsets); } }

            [StructLayout(LayoutKind.Sequential, Pack = 1)]
            public struct Offsets
            {
                buint _params0;
                buint _params1;
                buint _params2;
                buint _params3;
                buint _article0;
                buint _article1;
                sListOffset _extraData6Offset;
                buint _boneIndexReplacementOffset; //8 bytes: two offsets
                buint _params4;
            }
        }

        public class WarioMan : OffsetHolder
        {
            protected override Type OffsetsType { get { return typeof(Offsets); } }

            [StructLayout(LayoutKind.Sequential, Pack = 1)]
            public struct Offsets
            {
                buint _params0;
                buint _params1;
                buint _params2;
                buint _params3;
                buint _params4;
                buint _article0;
                sListOffset _extraData6Offset;
                buint _boneIndexReplacementOffset; //8 bytes: two offsets
            }
        }

        public class Wolf : OffsetHolder
        {
            protected override Type OffsetsType { get { return typeof(Offsets); } }

            [StructLayout(LayoutKind.Sequential, Pack = 1)]
            public struct Offsets
            {
                buint _params0;
                buint _params1;
                buint _params2;
                buint _params3;
                buint _params6;
                buint _params4;
                sListOffset _params5;
                buint _article0;
                buint _article1;
                buint _article2;
                buint _article3;
                buint _article4;
            }
        }

        public class Yoshi : OffsetHolder
        {
            protected override Type OffsetsType { get { return typeof(Offsets); } }

            [StructLayout(LayoutKind.Sequential, Pack = 1)]
            public struct Offsets
            {
                buint _params0;
                buint _params1;
                buint _params2;
                buint _params3;
                buint _params4;
                buint _article0;
                buint _article1;
                buint _article2;
                buint _article3;
                buint _article4;
                buint _extraData9Offset0; //CatchData 12 bytes
                buint _extraData9Offset1; //CatchDashData 12 bytes
                buint _extraData9Offset2; //CatchTurnData 12 bytes
            }
        }

        public class GreenAlloy : OffsetHolder
        {
            protected override Type OffsetsType { get { return typeof(Offsets); } }

            [StructLayout(LayoutKind.Sequential, Pack = 1)]
            public struct Offsets
            {
                buint _pad; //0
            }
        }

        public class RedAlloy : OffsetHolder
        {
            protected override Type OffsetsType { get { return typeof(Offsets); } }

            [StructLayout(LayoutKind.Sequential, Pack = 1)]
            public struct Offsets
            {
                buint _pad; //0
            }
        }

        public class YellowAlloy : OffsetHolder
        {
            protected override Type OffsetsType { get { return typeof(Offsets); } }

            [StructLayout(LayoutKind.Sequential, Pack = 1)]
            public struct Offsets
            {
                buint _pad; //0
            }
        }
        
        public class BlueAlloy : OffsetHolder
        {
            protected override Type OffsetsType { get { return typeof(Offsets); } }

            [StructLayout(LayoutKind.Sequential, Pack = 1)]
            public struct Offsets
            {
                buint _pad; //0
            }
        }

        public class Zelda : OffsetHolder
        {
            protected override Type OffsetsType { get { return typeof(Offsets); } }

            [StructLayout(LayoutKind.Sequential, Pack = 1)]
            public struct Offsets
            {
                public buint _params0;
                public buint _params1;
                public buint _params2;
                public buint _params3;
                public buint _params4;
                public buint _params5;
                public buint _article0;
                public buint _article1;
                public buint _article2;
                public buint _article3;
            }
        }
    }
    //    case CharFolder.ZakoBall:
    //    case CharFolder.ZakoBoy:
    //    case CharFolder.ZakoGirl:
    //    case CharFolder.ZakoChild:
    //        ExtraDataOffsets._count = 1; break;
    //    case CharFolder.Purin:
    //        ExtraDataOffsets._count = 3; break;
    //    case CharFolder.Koopa:
    //    case CharFolder.Metaknight:
    //        ExtraDataOffsets._count = 5; break;
    //    case CharFolder.Ganon:
    //    case CharFolder.GKoopa:
    //    case CharFolder.Marth:
    //        ExtraDataOffsets._count = 6; break;
    //    case CharFolder.PokeFushigisou:
    //        ExtraDataOffsets._count = 7; break;
    //    case CharFolder.Captain:
    //    case CharFolder.Ike:
    //    case CharFolder.Luigi:
    //    case CharFolder.PokeLizardon:
    //    case CharFolder.PokeTrainer:
    //    case CharFolder.PokeZenigame:
    //    case CharFolder.Sonic:
    //        ExtraDataOffsets._count = 8; break;
    //    case CharFolder.Donkey:
    //    case CharFolder.Sheik:
    //    case CharFolder.WarioMan:
    //        ExtraDataOffsets._count = 9; break;
    //    case CharFolder.Mario:
    //    case CharFolder.Wario:
    //    case CharFolder.Zelda:
    //        ExtraDataOffsets._count = 10; break;
    //    case CharFolder.Falco:
    //    case CharFolder.Lucario:
    //    case CharFolder.Pikachu:
    //        ExtraDataOffsets._count = 11; break;
    //    case CharFolder.SZerosuit:
    //        ExtraDataOffsets._count = 12; break;
    //    case CharFolder.Diddy:
    //    case CharFolder.Fox:
    //    case CharFolder.Lucas:
    //    case CharFolder.Pikmin:
    //    case CharFolder.Pit:
    //    case CharFolder.Wolf:
    //    case CharFolder.Yoshi:
    //        ExtraDataOffsets._count = 13; break;
    //    case CharFolder.Ness:
    //    case CharFolder.Peach:
    //    case CharFolder.Robot:
    //        ExtraDataOffsets._count = 14; break;
    //    case CharFolder.Dedede:
    //    case CharFolder.Gamewatch:
    //        ExtraDataOffsets._count = 16; break;
    //    case CharFolder.Popo:
    //        ExtraDataOffsets._count = 18; break;
    //    case CharFolder.Link:
    //    case CharFolder.Snake:
    //    case CharFolder.ToonLink:
    //        ExtraDataOffsets._count = 20; break;
    //    case CharFolder.Samus:
    //        ExtraDataOffsets._count = 22; break;
    //    case CharFolder.Kirby:
    //        ExtraDataOffsets._count = 68; break;
    //    default: //Only works on movesets untouched by PSA
    //        ExtraDataOffsets._count = (Size - 124) / 4; break;
}
