using Ikarus.MovesetFile;

namespace Ikarus.MovesetBuilder
{
    public unsafe partial class DataCommonBuilder : BuilderBase
    {
        private void GetSizePart2()
        {
            foreach (CommonAction a in _dataCommon.FlashOverlays)
            {
                GetScriptSize(a, true);
                AddSize(8); //offset and flags
            }

            if (_dataCommon._ppMul != null)
                foreach (Script s in _dataCommon._ppMul._scripts)
                    foreach (Event e in s)
                        foreach (Parameter p in e)
                            GetSize(p);

            foreach (CommonAction a in _dataCommon.ScreenTints)
            {
                GetScriptSize(a, true);
                AddSize(8); //offset and flags
            }
        }

        private void BuildPart2()
        {
            foreach (CommonAction a in _dataCommon.FlashOverlays)
                Write(a);

            if (_dataCommon._ppMul != null)
                foreach (Script s in _dataCommon._ppMul._scripts)
                    foreach (Event e in s)
                        foreach (Parameter p in e)
                            Write(p);

            foreach (CommonAction a in _dataCommon.ScreenTints)
                Write(a);


        }
    }
}
