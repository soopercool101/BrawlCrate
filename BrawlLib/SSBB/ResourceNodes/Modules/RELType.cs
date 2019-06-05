using System;
using System.Collections.Generic;

namespace BrawlLib.SSBB.ResourceNodes
{
    public class RELType
    {
        public RELType(string name)
        {
            FullName = name;
        }

        public string FullName { get; set; } = "";

        public string FormalName
        {
            get => GetName();
            set => SetName(value);
        }

        public string[] FormalArguments
        {
            get => GetArguments();
            set => SetArguments(value);
        }

        public bool Inherited { get; set; } = false;
        public List<InheritanceItemNode> Inheritance { get; } = new List<InheritanceItemNode>();

        private string GetName()
        {
            return FullName.Split('<')[0];
        }

        private void SetName(string name)
        {
            if (FormalName == name) return;

            if (!FullName.Contains('<'))
                FullName = name;
            else
                FullName = name + '<' + FullName.Split('<')[1];
        }

        private string[] GetArguments()
        {
            string[] arguments = null;

            if (FullName.Contains('<') && FullName.EndsWith(">"))
            {
                arguments = FullName.Remove(FullName.Length - 1).Split(new[] {'<'}, 2)[1].Split(',');
                for (var i = 0; i < arguments.Length; i++) arguments[i] = arguments[i].Trim();
            }

            return arguments;
        }

        private void SetArguments(string[] arguments)
        {
            if (FormalArguments == arguments) return;

            if (arguments == null)
                FullName = FormalName;
            else
                FullName = FormalName + '<' + string.Join(", ", arguments) + '>';
        }

        public override string ToString()
        {
            return ToString(true);
        }

        public string ToString(bool inheritance)
        {
            if (!inheritance || Inheritance.Count == 0) return FullName;

            var inheritTypes = new string[Inheritance.Count];
            for (var i = 0; i < inheritTypes.Length; i++) inheritTypes[i] = Inheritance[i]._type.ToString(false);

            return FullName + " : " + string.Join(", ", inheritTypes);
        }
    }

    public class InheritanceItemNode : RELEntryNode
    {
        public RELType _type;
        public uint _unknown;

        public InheritanceItemNode(RELType type, uint unknown)
        {
            _type = type;
            _name = _type.FullName;
            _unknown = unknown;
        }

        public override ResourceType ResourceFileType => ResourceType.RELInheritance;

        public RELType Type
        {
            get => _type;
            set
            {
                _type = value;
                SignalPropertyChange();
            }
        }

        public uint Unknown => _unknown;

        public override string ToString()
        {
            return _type.ToString();
        }
    }
}