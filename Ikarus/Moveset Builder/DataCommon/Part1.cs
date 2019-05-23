using Ikarus.MovesetFile;

namespace Ikarus.MovesetBuilder
{
    public unsafe partial class DataCommonBuilder : BuilderBase
    {
        private void GetSizePart1()
        {
            GetSize(_dataCommon._unk23);

            if (_dataCommon._unk7 != null)
                foreach (CommonUnk7Entry e in _dataCommon._unk7)
                    foreach (CommonUnk7EntryListEntry e2 in e._children)
                        GetSize(e2);
            
            GetSize(_dataCommon._unk8);
            GetSize(_dataCommon._unk10);
            GetSize(_dataCommon._unk16);
            GetSize(_dataCommon._unk18);
            GetSize(_dataCommon._globalICs);
            GetSize(_dataCommon._ICs);
            GetSize(_dataCommon._unk12);
            GetSize(_dataCommon._unk13);
            GetSize(_dataCommon._unk14);
            GetSize(_dataCommon._unk15);
            GetSize(_dataCommon._globalsseICs);
            GetSize(_dataCommon._sseICs);
        }

        private void BuildPart1()
        {
            dataCommonHeader->Unknown23 = Write(_dataCommon._unk23);

            if (_dataCommon._unk7 != null)
                foreach (CommonUnk7Entry e in _dataCommon._unk7)
                    foreach (CommonUnk7EntryListEntry e2 in e._children)
                        Write(e2, 8);

            dataCommonHeader->Unknown8 = Write(_dataCommon._unk8);
            dataCommonHeader->Unknown10 = Write(_dataCommon._unk10);
            dataCommonHeader->Unknown16 = Write(_dataCommon._unk16);
            dataCommonHeader->Unknown18 = Write(_dataCommon._unk18);
            dataCommonHeader->GlobalICs = Write(_dataCommon._globalICs);
            dataCommonHeader->ICs = Write(_dataCommon._ICs);
            dataCommonHeader->Unknown12 = Write(_dataCommon._unk12);
            dataCommonHeader->Unknown13 = Write(_dataCommon._unk13);
            dataCommonHeader->Unknown14 = Write(_dataCommon._unk14);
            dataCommonHeader->Unknown15 = Write(_dataCommon._unk15);
            dataCommonHeader->SSEGlobalICs = Write(_dataCommon._globalsseICs);
            dataCommonHeader->SSEICs = Write(_dataCommon._sseICs);
        }
    }
}
