namespace Ikarus.MovesetBuilder
{
    public unsafe partial class DataBuilder : BuilderBase
    {
        private void GetSizePart4()
        {
            //Subaction main, gfx, sfx, other arrays
            IncLookup(4); //Offset to each array
            AddSize(_data._subActions.Count * 16); //count * 4 bytes * 4 arrays

            GetSize(_data._modelVis, true);
            GetSize(_misc._unknown10, true);
            GetSize(_data._boneRef2, true);

            //TODO
            //if (_data._nanaSubActions != null)
            //{
            //    foreach (var g in _data._nanaSubActions.Children)
            //        if (g.Name != "<null>")
            //        {
            //            foreach (Script a in g.Scripts)
            //                if (a.Count > 0 || a._actionRefs.Count > 0 || a._build)
            //                {
            //                    GetScriptSize(a);
            //                    IncLookup();
            //                }
            //        }
            //    IncLookup(4);
            //    AddSize(_data._nanaSubActions.Children.Count * 16);
            //}

            GetSize(_misc._collisionData, true);
            GetSize(_data._unknown24, true);
            GetSize(_misc._unknown18, true);
            GetSize(_data._unknown22, true);
        }

        private void BuildPart4()
        {
            Skip(_data._subActions.Count * 16); //handled by DataBuilder

            Write(_data._modelVis);
            Write(_misc._unknown10);
            Write(_data._boneRef2);

            //TODO: Write nana's subactions and offsets here

            Write(_misc._collisionData);
            Write(_data._unknown24);
            Write(_misc._unknown18);
            Write(_data._unknown22);
        }
    }
}
