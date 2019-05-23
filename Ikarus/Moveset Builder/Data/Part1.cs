using Ikarus.MovesetFile;
using System.Linq;

namespace Ikarus.MovesetBuilder
{
    public unsafe partial class DataBuilder : BuilderBase
    {
        private void GetSizePart1()
        {
            GetSize(_data._attributes);
            GetSize(_data._sseAttributes);
            GetSize(_data._commonActionFlags);
            GetSize(_data._unknown7);

            //Add subaction data size (not including offset arrays to the data)
            foreach (SubActionEntry g in _data._subActions)
                if (g.Enabled)
                    foreach (Script c in g.GetScriptArray())
                        GetScriptSize(c);

            CalcSizeArticleActions(true, 0); //Main
            CalcSizeArticleActions(true, 1); //GFX
            CalcSizeArticleActions(true, 2); //SFX

            GetSize(_misc._finalSmashAura, true);

            if (_misc._soundData != null)
                foreach (var r in _misc._soundData._entries)
                    AddSize(r.Count * 4);

            foreach (ActionEntry a in _moveset._actions)
            {
                GetScriptSize(a._entry);
                GetScriptSize(a._exit);
            }
        }

        private void BuildPart1()
        {
            Write(_data._attributes, &dataHeader->AttributeStart);
            Write(_data._sseAttributes, &dataHeader->SSEAttributeStart);
            Write(_data._commonActionFlags, &dataHeader->CommonActionFlagsStart);
            Write(_data._unknown7, &dataHeader->Unknown7);

            //Other
            WriteScriptArray(_data._subActions.Select(x => x.Enabled ? x._other : null).ToArray(), subActionArrays[3]);

            //GFX
            WriteScriptArray(_data._subActions.Select(x => x.Enabled ? x._sfx : null).ToArray(), subActionArrays[2]);
            WriteArticleActions(true, 2);

            //SFX
            WriteScriptArray(_data._subActions.Select(x => x.Enabled ? x._gfx : null).ToArray(), subActionArrays[1]);
            WriteArticleActions(true, 1);

            //Main
            WriteScriptArray(_data._subActions.Select(x => x.Enabled ? x._main : null).ToArray(), subActionArrays[0]);
            WriteArticleActions(true, 0);

            WriteEntryList(_misc._finalSmashAura, &miscHeader->FinalSmashAura);

            if (_misc._soundData != null)
                foreach (var r in _misc._soundData._entries)
                    Write(r);

            //Action script data alternates entry -> exit -> entry etc but the offset arrays are separated by entry and exit
            int w = 0;
            foreach (ActionEntry a in _moveset._actions)
            {
                actionArrays[0][w] = WriteScript(a._entry);
                actionArrays[1][w] = WriteScript(a._exit);
                w++;
            }
        }
    }
}
