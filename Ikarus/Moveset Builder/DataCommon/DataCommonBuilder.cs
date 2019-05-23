using System;
using Ikarus.MovesetFile;

namespace Ikarus.MovesetBuilder
{
    public unsafe partial class DataCommonBuilder : BuilderBase
    {
        DataCommonSection _dataCommon;

        public DataCommonBuilder(DataCommonSection dataCommon)
        {
            _moveset = (_dataCommon = dataCommon)._root as MovesetNode;
            _getPartSize = new Action[]
            {
                GetSizePart1,
                GetSizePart2,
                GetSizePart3,
            };
            _buildPart = new Action[]
            {
                BuildPart1,
                BuildPart2,
                BuildPart3,
            };
        }

        DataCommonHeader* dataCommonHeader;

        public override void Build(VoidPtr address)
        {
            dataCommonHeader = (DataCommonHeader*)(address + _dataCommon._childLength);

            base.Build(address);
        }
    }
}