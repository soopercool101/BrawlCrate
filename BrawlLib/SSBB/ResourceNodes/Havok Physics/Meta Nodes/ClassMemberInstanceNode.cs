using BrawlLib.Internal;
using BrawlLib.SSBB.Types;
using System.ComponentModel;

namespace BrawlLib.SSBB.ResourceNodes
{
    public abstract class ClassMemberInstanceNode : HavokClassNode
    {
        [Browsable(false)] protected VoidPtr Data => WorkingUncompressed.Address;

        public bool _isZero;
        public hkClassMember.Type _memberType;
        public hkClassMember.Flags _memberFlags;
        public hkClassNode _classNode;
        public hkClassEnumNode _enumNode;

        [Category("Class Member Instance")]
        public string Inheritance => _classNode == null ? null : _classNode.Inheritance;

        [Category("Class Member Instance")] public bool SerializedAsZero => _isZero;
#if DEBUG
        [Category("Class Member Instance")] public string TypeName => GetType().ToString();
#endif

        //Size of this member in bytes
        public virtual int GetSize()
        {
            return 0;
        }

        public override int OnCalculateSize(bool force)
        {
            return GetSize();
        }

        public bool IsValid()
        {
            return WorkingUncompressed != DataSource.Empty && WorkingUncompressed.Length > 0;
        }

        public VoidPtr GetAddress()
        {
            return WorkingUncompressed.Address;
        }

        public int GetLength()
        {
            return WorkingUncompressed.Length;
        }
    }
}