using BrawlLib.SSBBTypes;
using System;
using System.ComponentModel;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe abstract class ClassMemberInstanceNode : HavokClassNode
    {
        [Browsable(false)]
        protected VoidPtr Data { get { return WorkingUncompressed.Address; } }

        public bool _isZero;
        public hkClassMember.Type _memberType;
        public hkClassMember.Flags _memberFlags;
        public hkClassNode _classNode;
        public hkClassEnumNode _enumNode;

        [Category("Class Member Instance")]
        public string Inheritance { get { return _classNode == null ? null : _classNode.Inheritance; } }
        [Category("Class Member Instance")]
        public bool SerializedAsZero { get { return _isZero; } }
#if DEBUG
        [Category("Class Member Instance")]
        public string TypeName { get { return GetType().ToString(); } }
#endif

        //Size of this member in bytes
        public virtual int GetSize() { return 0; }

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
