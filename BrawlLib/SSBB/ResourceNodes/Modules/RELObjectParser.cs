using BrawlLib.Internal;
using System.Collections.Generic;

namespace BrawlLib.SSBB.ResourceNodes
{
    public class ObjectParser
    {
        private readonly ModuleSectionNode _objectSection;

        public Dictionary<int, RELType> _types = new Dictionary<int, RELType>();
        public List<RELObjectNode> _objects = new List<RELObjectNode>();

        public ObjectParser(ModuleSectionNode section)
        {
            _objects = new List<RELObjectNode>();
            _types = new Dictionary<int, RELType>();
            _objectSection = section;
        }

        private RelocationManager Manager => _objectSection._manager;

        public void Parse()
        {
            if (_objectSection == null)
            {
                return;
            }

            for (int rel = 0; rel < _objectSection._dataBuffer.Length / 4; rel++)
            {
                ParseDeclaration(rel);
            }

            for (int rel = 0; rel < _objectSection._dataBuffer.Length / 4; rel++)
            {
                ParseObject(ref rel);
            }
        }

        private unsafe RELType ParseDeclaration(int index)
        {
            if (_types.TryGetValue(index, out RELType type))
            {
                return type;
            }

            RelCommand cmd = Manager.GetCommand(index);

            RelocationTarget target = cmd?.GetTargetRelocation();
            if (target == null || target._sectionID != _objectSection.Index || target._moduleID != _objectSection.ModuleID)
            {
                return null;
            }

            uint relOffset = cmd.Apply(Manager.GetUint(index), 0);
            if (relOffset > _objectSection._dataOffset + _objectSection._dataSize || relOffset > _objectSection.WorkingUncompressed.Length)
            {
                return null;
            }

            string name = (_objectSection.Header + relOffset).GetUTF8String();

            if (string.IsNullOrWhiteSpace(name))
            {
                return null;
            }

            type = new RELType(name);

            //Get inheritances, if any.
            cmd = Manager.GetCommand(index + 1);
            if (cmd != null)
            {
                for (RelocationTarget r = cmd.GetTargetRelocation();
                    r != null && (cmd = Manager.GetCommand(r._index)) != null;
                    r._index += 2)
                {
                    RelocationTarget inheritTarget = cmd.GetTargetRelocation();
                    RELType inheritance = ParseDeclaration(inheritTarget._index);
                    if (inheritance != null)
                    {
                        InheritanceItemNode typeNode =
                            new InheritanceItemNode(inheritance, Manager.GetUint(r._index + 1));
                        typeNode.Initialize(null, _objectSection.Header + r._index * 4, 0);
                        type.Inheritance.Add(typeNode);
                        inheritance.Inherited = true;
                    }
                    else
                    {
                        break;
                    }
                }
            }

            Manager.AddTag(index, type.FormalName + " Declaration");
            Manager.AddTag(index + 1, type.FormalName + "->Inheritances");

            _types.Add(index, type);

            return type;
        }

        private RELObjectNode ParseObject(ref int rel)
        {
            RelCommand cmd = Manager.GetCommand(rel);

            RelocationTarget target = cmd?.GetTargetRelocation();
            if (target == null || target._sectionID != _objectSection.Index)
            {
                return null;
            }


            if (!_types.TryGetValue(target._index, out RELType declaration) || declaration.Inherited)
            {
                return null;
            }

            RELObjectNode obj = null;
            foreach (RELObjectNode node in _objects)
            {
                if (node._name == declaration.FullName)
                {
                    obj = node;
                    break;
                }
            }

            if (obj == null)
            {
                obj = new RELObjectNode(declaration)
                {
                    _parent = _objectSection
                };
                _objectSection._children.Add(obj);
                new RELGroupNode {_name = "Inheritance"}.Parent = obj;
                foreach (InheritanceItemNode n in declaration.Inheritance)
                {
                    n.Parent = obj.Children[0];
                }

                new RELGroupNode {_name = "Functions"}.Parent = obj;
            }

            int baseRel = rel;
            RelCommand baseCmd = Manager.GetCommand(baseRel);

            int methodIndex = 0;
            int setIndex = 0;

            //Read object methods.
            while ((cmd = Manager.GetCommand(rel)) != null)
            {
                RelocationTarget t = cmd.GetTargetRelocation();
                if (cmd.Apply(Manager.GetUint(rel), 0) != baseCmd.Apply(Manager.GetUint(baseRel), 0))
                {
                    string methodName = $"Function[{setIndex}][{methodIndex}]";
                    VoidPtr addr = null;
                    if (t != null && t._moduleID == (_objectSection.Root as ModuleNode).ID)
                    {
                        addr = _objectSection.Root.Children[t._sectionID].WorkingUncompressed.Address + t._index * 4;
                    }

                    new RELMethodNode
                        {
                            _name = methodName,
                            _cmd = cmd
                        }
                        .Initialize(obj.Children[1], addr, 0);

                    methodIndex++;
                }
                else
                {
                    if (Manager.GetUint(rel + 1) != 0)
                    {
                        setIndex++;
                    }

                    methodIndex = 0;
                    rel++;
                }

                rel++;
            }

            Manager.AddTag(baseRel, obj.Type.FullName);

            _objects.Add(obj);
            return obj;
        }

        public void Populate()
        {
            foreach (RELObjectNode obj in _objects)
            {
                obj._parent = null;
                _objectSection._children.Remove(obj);
                obj.Parent = _objectSection;
            }
        }
    }
}