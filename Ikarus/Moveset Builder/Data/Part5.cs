using Ikarus.MovesetFile;

namespace Ikarus.MovesetBuilder
{
    public unsafe partial class DataBuilder : BuilderBase
    {
        private void GetSizePart5()
        {
            //Articles here, in this order
            foreach (ArticleNode a in _data._staticArticles) GetSize(a);
            GetSize(_data._entryArticle);
            foreach (ArticleNode a in _data._articles) GetSize(a);

            GetSize(_misc._unknown7);

            //if (_data.nanaSoundData != null)
            //{
            //    foreach (MoveDefSoundDataNode r in node.nanaSoundData.Children)
            //    {
            //        lookupCount += (r.Children.Count > 0 ? 1 : 0);
            //        8 + r.Children.Count * 4;
            //    }
            //}

            GetSize(_data._actionInterrupts);
            GetSize(_data._anchoredItems);
            GetSize(_data._gooeyBomb);
            GetSize(_data._samusCannon);
            GetSize(_data._boneRef1);
            GetSize(_misc._boneRefs);
            GetSize(_misc._unknown12);
            GetSize(_misc._soundData);
        }

        private void BuildPart5()
        {
            //Articles here, in this order
            foreach (ArticleNode a in _data._staticArticles) Write(a);
            Write(_data._entryArticle);
            foreach (ArticleNode a in _data._articles) Write(a);

            Write(_misc._unknown7);

            //if (_data.nanaSoundData != null)
            //{
            //    foreach (MoveDefSoundDataNode r in node.nanaSoundData.Children)
            //    {
            //        lookupCount += (r.Children.Count > 0 ? 1 : 0);
            //        8 + r.Children.Count * 4;
            //    }
            //}

            Write(_data._actionInterrupts);
            Write(_data._anchoredItems);
            Write(_data._gooeyBomb);
            Write(_data._samusCannon);
            Write(_data._boneRef1);
            Write(_misc._boneRefs);
            Write(_misc._unknown12);
            Write(_misc._soundData);
        }
    }
}
