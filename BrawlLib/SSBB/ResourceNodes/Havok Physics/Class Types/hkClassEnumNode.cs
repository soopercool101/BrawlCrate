using BrawlLib.Internal;
using BrawlLib.SSBB.Types;
using System.Collections.Generic;

namespace BrawlLib.SSBB.ResourceNodes
{
    //Class enums appear in classes.
    //A duplicate copy (written again at at another address) 
    //is placed in the overall class list directly after the class it first appears in
    public unsafe class hkClassEnumNode : HavokClassNode
    {
        internal hkClassEnum* Header => (hkClassEnum*) WorkingUncompressed.Address;

        public override bool OnInitialize()
        {
            _className = "hkClassEnum";
            _name = new string((sbyte*) Header->_namePtr.OffsetAddress);

            int size = 16;
            HavokClassEnumEntry* entry = (HavokClassEnumEntry*) Header->_entriesPtr.OffsetAddress;
            for (int i = 0; i < Header->_enumCount; i++, entry++)
            {
                size += 8 + (new string((sbyte*) entry->_namePtr.OffsetAddress).Length + 1).Align(0x10);
            }

            SetSizeInternal(size);

            return Header->_enumCount > 0;
        }

        public override void OnPopulate()
        {
            HavokClassEnumEntry* entry = (HavokClassEnumEntry*) Header->_entriesPtr.OffsetAddress;
            for (int i = 0; i < Header->_enumCount; i++, entry++)
            {
                new hkClassEnumEntryNode().Initialize(this, entry, 8);
            }
        }

        public override void WriteParams(System.Xml.XmlWriter writer, Dictionary<HavokClassNode, int> classNodes)
        {
            writer.WriteStartElement("hkparam");
            writer.WriteAttributeString("name", "name");
            writer.WriteString(Name);
            writer.WriteEndElement();

            writer.WriteStartElement("hkparam");
            writer.WriteAttributeString("name", "items");
            writer.WriteAttributeString("numelements", Children.Count.ToString());
            {
                foreach (hkClassEnumEntryNode e in Children)
                {
                    writer.WriteStartElement("hkobject");
                    {
                        writer.WriteStartElement("hkparam");
                        writer.WriteAttributeString("name", "value");
                        writer.WriteString(e.Value.ToString());
                        writer.WriteEndElement();

                        writer.WriteStartElement("hkparam");
                        writer.WriteAttributeString("name", "name");
                        writer.WriteString(e.Name);
                        writer.WriteEndElement();
                    }
                    writer.WriteEndElement();
                }
            }
            writer.WriteEndElement();
        }
    }

    public unsafe class hkClassEnumEntryNode : HavokEntryNode
    {
        internal HavokClassEnumEntry* Header => (HavokClassEnumEntry*) WorkingUncompressed.Address;

        private int _value;
        public int Value => _value;

        public override bool OnInitialize()
        {
            _name = new string((sbyte*) Header->_namePtr.OffsetAddress);
            _value = Header->_value;

            return false;
        }
    }
}