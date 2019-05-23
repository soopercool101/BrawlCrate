using Ikarus.MovesetFile;

namespace Ikarus.MovesetBuilder
{
    public unsafe partial class DataBuilder : BuilderBase
    {
        private void GetSizePart2()
        {
            GetSize(_misc._unknown0, true);

            if (_misc._collisionData != null)
                foreach (var offset in _misc._collisionData._entries)
                    if (offset._bones.Count > 0 && !offset._bones[0].External)
                        AddSize(offset._bones.Count * 4);

            GetSize(_misc._hurtBoxes, true);

            GetSize(_misc._ledgeGrabs, true);
            GetSize(_misc._tether, true);
            GetSize(_misc._crawl, true);
            GetSize(_misc._multiJump, true);
            GetSize(_misc._glide, true);

            for (int i = 0; i < Subroutines.Count; i++)
                GetScriptSize(Subroutines[i], true);

            if (_data._unknown22 != null)
                GetSize(_data._unknown22._script, false);

            if (_data._entryOverrides != null)
                foreach (var e in _data._entryOverrides)
                    GetSize(e._script);

            if (_data._exitOverrides != null)
                foreach (var e in _data._entryOverrides)
                    GetSize(e._script);

            CalcSizeArticleActions(false, 0);

            //Action flags
            AddSize(_moveset.Actions.Count * sActionFlags.Size);
            IncLookup();
        }

        private void BuildPart2()
        {
            miscHeader->Unknown0 = Write(_misc._unknown0);

            if (_misc._collisionData != null)
                foreach (var offset in _misc._collisionData._entries)
                    if (offset._bones.Count > 0 && !offset._bones[0].External)
                        AddSize(offset._bones.Count * 4);

            Write(_misc._hurtBoxes);
            Write(_misc._ledgeGrabs);
            Write(_misc._tether);
            Write(_misc._crawl);
            Write(_misc._multiJump);
            Write(_misc._glide);

            for (int i = 0; i < Subroutines.Count; i++)
                WriteScript(Subroutines[i]);

            if (_data._unknown22 != null)
                WriteScript(_data._unknown22._script);

            if (_data._entryOverrides != null)
                foreach (var e in _data._entryOverrides)
                    WriteScript(e._script);

            if (_data._exitOverrides != null)
                foreach (var e in _data._entryOverrides)
                    WriteScript(e._script);

            WriteArticleActions(false, 0);

            foreach (var a in _moveset.Actions)
                Write(a, sActionFlags.Size);
        }
    }
}
