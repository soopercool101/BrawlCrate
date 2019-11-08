using BrawlLib.Internal;
using BrawlLib.SSBB.Types;
using System.ComponentModel;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class MoveDefActionOverrideNode : MoveDefEntryNode
    {
        internal ActionOverride* Start => (ActionOverride*) WorkingUncompressed.Address;
        public override ResourceType ResourceFileType => ResourceType.MDefActionOverrideList;

        public override bool OnInitialize()
        {
            base.OnInitialize();
            return true;
        }

        public override void OnPopulate()
        {
            ActionOverride* entry = Start;
            while (entry->_commandListOffset > 0)
            {
                new MoveDefActionOverrideEntryNode().Initialize(this, new DataSource((VoidPtr) entry++, 8));
            }

            //new MoveDefActionOverrideEntryNode().Initialize(this, new DataSource((VoidPtr)(entry), 8));
        }

        public override int OnCalculateSize(bool force)
        {
            _lookupCount = 0;
            int size = 8; //Size of terminator entry
            foreach (MoveDefActionOverrideEntryNode e in Children)
            {
                size += 8;
                if (e.Children.Count > 0)
                {
                    _lookupCount++;
                }
            }

            return _entryLength = size;
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            _entryOffset = address;

            //Don't forget to add terminator entry (action id == -1, offset == 0)
            foreach (MoveDefActionOverrideEntryNode e in Children)
            {
                e._entryOffset = address;
                ActionOverride* addr = (ActionOverride*) address;
                addr->_actionID = e._actionId;
                Root._postProcessNodes.Add(this);
                if (e.Children.Count > 0)
                {
                    _lookupOffsets.Add((int) addr->_commandListOffset.Address - (int) _rebuildBase);
                }

                address += 8;
            }

            ActionOverride* end = (ActionOverride*) address;
            end->_actionID = -1;
            end->_commandListOffset = 0;
        }

        public override void PostProcess()
        {
            foreach (MoveDefActionOverrideEntryNode e in Children)
            {
                if (e.Children.Count > 0)
                {
                    ((ActionOverride*) e._entryOffset)->_commandListOffset =
                        (int) (e.Children[0] as MoveDefActionNode)._entryOffset - (int) _rebuildBase;
                }
            }
        }
    }

    public unsafe class MoveDefActionOverrideEntryNode : MoveDefEntryNode
    {
        internal ActionOverride* Header => (ActionOverride*) WorkingUncompressed.Address;
        public override ResourceType ResourceFileType => ResourceType.NoEditEntry;

        public int _actionId, _commandListOffset;

        [Category("Action Override")]
        public int ActionID
        {
            get => _actionId;
            set
            {
                _actionId = value;
                Name = "Action" + _actionId + " Override";
                Children[0].Name = "Action" + _actionId;
                SignalPropertyChange();
            }
        }

        [Category("Action Override")] public int CommandListOffset => _commandListOffset;

        public override bool OnInitialize()
        {
            _actionId = Header->_actionID;
            _commandListOffset = Header->_commandListOffset;
            _extOverride = true;
            base.OnInitialize();
            if (_actionId != -1)
            {
                _name = "Action" + _actionId + " Override";
            }
            else
            {
                _name = "List Terminator";
            }

            return _commandListOffset > 0;
        }

        public override void OnPopulate()
        {
            new MoveDefActionNode("Action" + _actionId, false, this).Initialize(this,
                new DataSource((VoidPtr) (BaseAddress + _commandListOffset), 0));
        }
    }
}