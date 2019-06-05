using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using BrawlLib.SSBBTypes;

namespace BrawlLib.SSBB.ResourceNodes
{
    public class UserDataCollectionPropertyDescriptor : PropertyDescriptor
    {
        private readonly UserDataCollection collection;
        private readonly int index = -1;

        public UserDataCollectionPropertyDescriptor(UserDataCollection coll, int idx) : base("#" + idx, null)
        {
            collection = coll;
            index = idx;
        }

        public override AttributeCollection Attributes => new AttributeCollection(null);
        public override Type ComponentType => collection.GetType();

        public override string DisplayName =>
            index >= collection.Count || index < 0 ? null : collection[index].ToString();

        public override string Description => null;
        public override bool IsReadOnly => true;
        public override string Name => "#" + index;
        public override Type PropertyType => collection[index].GetType();

        public override bool CanResetValue(object component)
        {
            return true;
        }

        public override object GetValue(object component)
        {
            return collection[index];
        }

        public override void ResetValue(object component)
        {
        }

        public override bool ShouldSerializeValue(object component)
        {
            return true;
        }

        public override void SetValue(object component, object value)
        {
            collection[index] = (UserDataClass) value;
        }
    }

    public unsafe class UserDataCollection : CollectionBase, ICustomTypeDescriptor
    {
        public UserDataClass this[int index]
        {
            get => index >= List.Count || index < 0 ? null : (UserDataClass) List[index];
            set => List[index] = value;
        }

        public string GetClassName()
        {
            return TypeDescriptor.GetClassName(this, true);
        }

        public AttributeCollection GetAttributes()
        {
            return TypeDescriptor.GetAttributes(this, true);
        }

        public string GetComponentName()
        {
            return TypeDescriptor.GetComponentName(this, true);
        }

        public TypeConverter GetConverter()
        {
            return TypeDescriptor.GetConverter(this, true);
        }

        public EventDescriptor GetDefaultEvent()
        {
            return TypeDescriptor.GetDefaultEvent(this, true);
        }

        public PropertyDescriptor GetDefaultProperty()
        {
            return TypeDescriptor.GetDefaultProperty(this, true);
        }

        public object GetEditor(Type editorBaseType)
        {
            return TypeDescriptor.GetEditor(this, editorBaseType, true);
        }

        public EventDescriptorCollection GetEvents(Attribute[] attributes)
        {
            return TypeDescriptor.GetEvents(this, attributes, true);
        }

        public EventDescriptorCollection GetEvents()
        {
            return TypeDescriptor.GetEvents(this, true);
        }

        public object GetPropertyOwner(PropertyDescriptor pd)
        {
            return this;
        }

        public PropertyDescriptorCollection GetProperties(Attribute[] attributes)
        {
            return GetProperties();
        }

        public PropertyDescriptorCollection GetProperties()
        {
            var pds = new PropertyDescriptorCollection(null);
            for (var i = 0; i < List.Count; i++)
            {
                var pd = new UserDataCollectionPropertyDescriptor(this, i);
                pds.Add(pd);
            }

            return pds;
        }

        public void Read(VoidPtr userDataAddr)
        {
            if (userDataAddr == null) return;

            var data = (UserData*) userDataAddr;
            var group = data->Group;
            var pEntry = &group->_first + 1;
            int count = group->_numEntries;
            for (var i = 0; i < count; i++, pEntry++)
            {
                var entry = (UserDataEntry*) ((VoidPtr) group + pEntry->_dataOffset);
                var d = new UserDataClass {_name = new string((sbyte*) group + pEntry->_stringOffset)};
                var addr = (VoidPtr) entry + entry->_dataOffset;
                d._type = entry->Type;
                if (d._type != UserValueType.String)
                    for (var x = 0; x < entry->_entryCount; x++)
                        switch (entry->Type)
                        {
                            case UserValueType.Float:
                                d._entries.Add(((float) *(bfloat*) addr).ToString());
                                addr += 4;
                                break;
                            case UserValueType.Int:
                                d._entries.Add(((int) *(bint*) addr).ToString());
                                addr += 4;
                                break;
                        }
                else
                    d._entries.Add(new string((sbyte*) addr));

                Add(d);
            }
        }

        public void GetStrings(StringTable table)
        {
            foreach (UserDataClass s in this)
            {
                table.Add(s._name);
                if (s._type == UserValueType.String && s._entries.Count > 0) table.Add(s._entries[0]);
            }
        }

        public int GetSize()
        {
            if (Count == 0) return 0;

            var len = 0x1C + Count * 0x28;
            foreach (UserDataClass c in this)
            foreach (var s in c._entries)
                if (c.DataType != UserValueType.String)
                    len += 4;

            return len;
        }

        public void Write(VoidPtr userDataAddr)
        {
            if (Count == 0 || userDataAddr == null) return;

            var data = (UserData*) userDataAddr;

            var pGroup = data->Group;
            var pEntry = &pGroup->_first + 1;
            *pGroup = new ResourceGroup(Count);

            var pData = (byte*) pGroup + pGroup->_totalSize;

            var id = 0;
            foreach (UserDataClass s in this)
            {
                (pEntry++)->_dataOffset = (int) pData - (int) pGroup;
                var p = (UserDataEntry*) pData;
                *p = new UserDataEntry(
                    s.DataType != UserValueType.String ? s._entries.Count : s._entries.Count > 0 ? 1 : 0, s._type,
                    id++);
                pData += 0x18;
                if (s.DataType != UserValueType.String)
                    for (var i = 0; i < s._entries.Count; i++)
                        if (s.DataType == UserValueType.Float)
                        {
                            if (!float.TryParse(s._entries[i], out var x)) x = 0;

                            *(bfloat*) pData = x;
                            pData += 4;
                        }
                        else if (s.DataType == UserValueType.Int)
                        {
                            if (!int.TryParse(s._entries[i], out var x)) x = 0;

                            *(bint*) pData = x;
                            pData += 4;
                        }

                p->_totalLen = (int) pData - (int) p;
            }

            data->_totalLen = (int) pData - (int) userDataAddr;
        }

        public void PostProcess(VoidPtr userDataAddr, StringTable stringTable)
        {
            if (Count == 0 || userDataAddr == null) return;

            var data = (UserData*) userDataAddr;

            var pGroup = data->Group;
            var pEntry = &pGroup->_first;
            int count = pGroup->_numEntries;
            (*pEntry++) = new ResourceEntry(0xFFFF, 0, 0, 0, 0);

            for (var i = 0; i < count; i++)
            {
                var entry = (UserDataEntry*) ((byte*) pGroup + (pEntry++)->_dataOffset);
                if (entry->Type == UserValueType.String && entry->_entryCount > 0)
                    entry->_dataOffset = stringTable[this[i]._entries[0]] + 4 - entry;

                ResourceEntry.Build(pGroup, i + 1, entry, (BRESString*) stringTable[this[i]._name]);
                entry->ResourceStringAddress = stringTable[this[i]._name] + 4;
            }
        }

        public void Add(UserDataClass u)
        {
            List.Add(u);
        }

        public void Remove(UserDataClass u)
        {
            List.Remove(u);
        }
    }

    [TypeConverter(typeof(UserDataConverter))]
    public class UserDataClass
    {
        public List<string> _entries = new List<string>();
        public string _name = "";

        public UserValueType _type;

        [Category("User Data")]
        public string Name
        {
            get => _name;
            set => _name = value;
        }

        [Category("User Data")]
        public string[] Entries
        {
            get => _entries.ToArray();
            set
            {
                _entries = value.ToList();
                if (DataType != UserValueType.String)
                    for (var i = 0; i < _entries.Count; i++)
                        if (DataType == UserValueType.Float)
                        {
                            if (!float.TryParse(_entries[i], out var x)) _entries[i] = "0";
                        }
                        else if (DataType == UserValueType.Int)
                        {
                            if (!int.TryParse(_entries[i], out var x)) _entries[i] = "0";
                        }
            }
        }

        [Category("User Data")]
        public UserValueType DataType
        {
            get => _type;
            set => _type = value;
        }

        public override string ToString()
        {
            var s = _name + ":";
            foreach (var i in Entries) s += i + ",";

            return s.Substring(0, s.Length - 1);
        }
    }

    public enum UserValueType
    {
        Int = 0,
        Float,
        String
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct UserData
    {
        public bint _totalLen; //Of everything user data related

        private VoidPtr Address
        {
            get
            {
                fixed (void* ptr = &this)
                {
                    return ptr;
                }
            }
        }

        public ResourceGroup* Group => (ResourceGroup*) (Address + 4);
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct UserDataEntry
    {
        public const int Size = 0x18;

        public bint _totalLen;
        public bint _dataOffset;
        public bint _entryCount;
        public bint _type;
        public bint _stringOffset; //same as entry
        public bint _id;

        //Entries, in the specified type

        public UserValueType Type
        {
            get => (UserValueType) (int) _type;
            set => _type = (int) value;
        }

        public UserDataEntry(int entries, UserValueType type, int id)
        {
            _totalLen = 0;
            _dataOffset = type == UserValueType.String ? 0 : 0x18;
            _entryCount = entries;
            _type = (int) type;
            _stringOffset = 0;
            _id = id;
        }

        private VoidPtr Address
        {
            get
            {
                fixed (void* ptr = &this)
                {
                    return ptr;
                }
            }
        }

        public string ResourceString => new string((sbyte*) ResourceStringAddress);

        public VoidPtr ResourceStringAddress
        {
            get => Address + _stringOffset;
            set => _stringOffset = (int) value - (int) Address;
        }
    }
}