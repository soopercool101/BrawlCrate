using System;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class RELDeConStructorNode : RELMethodNode
    {
        public override ResourceType ResourceType { get { return ResourceType.Unknown; } }

        public bool _destruct;
        public int _index;

        public override bool OnInitialize()
        {
            _name = String.Format("{0}[{1}]", _destruct ? "Destructor" : "Constructor", Index);
            
            base.OnInitialize();

            return false;
        }
    }
}
