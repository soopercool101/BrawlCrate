using System.ComponentModel;

namespace BrawlLib.SSBB.ResourceNodes
{
    public class RELObjectNode : RELEntryNode
    {
        private readonly RELType _type;

        [Browsable(false)] public RELType Type => _type;

        public int InheritanceCount => _type.Inheritance.Count;

        public RELObjectNode(RELType type)
        {
            _type = type;
            _name = _type.FullName;
        }

        public override string ToString()
        {
            return _type.ToString();
        }
    }
}