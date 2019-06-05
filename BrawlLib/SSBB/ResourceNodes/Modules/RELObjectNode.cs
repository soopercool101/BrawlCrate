using System.ComponentModel;

namespace BrawlLib.SSBB.ResourceNodes
{
    public class RELObjectNode : RELEntryNode
    {
        public RELObjectNode(RELType type)
        {
            Type = type;
            _name = Type.FullName;
        }

        [Browsable(false)] public RELType Type { get; }

        public int InheritanceCount => Type.Inheritance.Count;

        public override string ToString()
        {
            return Type.ToString();
        }
    }
}