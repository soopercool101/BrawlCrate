using Ikarus.MovesetFile;

namespace Ikarus.MovesetBuilder
{
    public unsafe partial class DataBuilder : BuilderBase
    {
        private void GetSizePart3()
        {
            IncLookup(); //Entry array offset
            IncLookup(); //Exit array offset
            AddSize(_moveset.Actions.Count * 8); //Add size of both arrays (count * 4 * 2)

            GetSize(_data._actionPre);

            IncLookup(); //Subaction flags offset
            AddSize(_data._subActions.Count * sSubActionFlags.Size);

            //Generate subaction string table
            foreach (SubActionEntry subaction in _data._subActions)
                if (subaction.Name != "<null>")
                {
                    IncLookup(); //String offset
                    _subActionTable.Add(subaction.Name);
                }

            //Add string table size
            AddSize(_subActionTable.TotalSize);
        }

        private void BuildPart3()
        {

        }
    }
}
