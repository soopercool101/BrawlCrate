namespace BrawlLib.SSBB.ResourceNodes
{
    public class RELDeConStructorNode : RELMethodNode
    {
        public bool _destruct;
        public int _index;
        public override ResourceType ResourceFileType => ResourceType.Unknown;

        public override bool OnInitialize()
        {
            _name = string.Format("{0}[{1}]", _destruct ? "Destructor" : "Constructor", Index);

            base.OnInitialize();

            return false;
        }
    }
}