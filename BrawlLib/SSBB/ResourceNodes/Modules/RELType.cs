using BrawlLib.Internal;
using System.Collections.Generic;

namespace BrawlLib.SSBB.ResourceNodes
{
    public class RELType
    {
        private string _fullName = "";
        private bool _inherited;
        private readonly List<InheritanceItemNode> _inheritance = new List<InheritanceItemNode>();

        public string FullName
        {
            get => _fullName;
            set => _fullName = value;
        }

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

        public bool Inherited
        {
            get => _inherited;
            set => _inherited = value;
        }

        public List<InheritanceItemNode> Inheritance => _inheritance;

        public RELType(string name)
        {
            _fullName = name;
        }

        private string GetName()
        {
            return FullName.Split('<')[0];
        }

        private void SetName(string name)
        {
            if (FormalName == name)
            {
                return;
            }

            if (!FullName.Contains('<'))
            {
                _fullName = name;
            }
            else
            {
                _fullName = name + '<' + FullName.Split('<')[1];
            }
        }

        private string[] GetArguments()
        {
            string[] arguments = null;

            if (_fullName.Contains('<') && _fullName.EndsWith(">"))
            {
                arguments = _fullName.Remove(_fullName.Length - 1).Split(new char[] {'<'}, 2)[1].Split(',');
                for (int i = 0; i < arguments.Length; i++)
                {
                    arguments[i] = arguments[i].Trim();
                }
            }

            return arguments;
        }

        private void SetArguments(string[] arguments)
        {
            if (FormalArguments == arguments)
            {
                return;
            }

            if (arguments == null)
            {
                _fullName = FormalName;
            }
            else
            {
                _fullName = FormalName + '<' + string.Join(", ", arguments) + '>';
            }
        }

        public override string ToString()
        {
            return ToString(true);
        }

        public string ToString(bool inheritance)
        {
            if (!inheritance || _inheritance.Count == 0)
            {
                return FullName;
            }

            string[] inheritTypes = new string[_inheritance.Count];
            for (int i = 0; i < inheritTypes.Length; i++)
            {
                inheritTypes[i] = _inheritance[i]._type.ToString(false);
            }

            return FullName + " : " + string.Join(", ", inheritTypes);
        }
    }

    public class InheritanceItemNode : RELEntryNode
    {
        public override ResourceType ResourceFileType => ResourceType.RELInheritance;

        public RELType _type;
        public uint _unknown;

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

        public InheritanceItemNode(RELType type, uint unknown)
        {
            _type = type;
            _name = _type.FullName;
            _unknown = unknown;
        }

        public override string ToString()
        {
            return _type.ToString();
        }
    }
}