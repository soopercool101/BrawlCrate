namespace BrawlLib.SSBB.ResourceNodes
{
    public class RELDeConStructorNode : RELMethodNode
    {
        public override ResourceType ResourceFileType => ResourceType.Unknown;

        public bool _destruct;
        public int _index;

        public override bool OnInitialize()
        {
            _name = $"{(_destruct ? "Destructor" : "Constructor")}[{Index}]";

            base.OnInitialize();

            return false;
        }
    }
}