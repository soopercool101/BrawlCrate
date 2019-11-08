using BrawlLib.Internal;
using BrawlLib.SSBB.Types;
using System.ComponentModel;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class MoveDefUnk22Node : MoveDefEntryNode
    {
        internal FDefUnk22* Header => (FDefUnk22*) WorkingUncompressed.Address;

        private int _unk1, _unk2, _actionOffset;

        [Category("Unknown Offset 22")]
        public int Unknown1
        {
            get => _unk1;
            set
            {
                _unk1 = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown Offset 22")]
        public int Unknown2
        {
            get => _unk2;
            set
            {
                _unk2 = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown Offset 22")] public int ActionOffset => _actionOffset;

        public override bool OnInitialize()
        {
            base.OnInitialize();
            _name = "Unknown 22";

            _unk1 = Header->_unk1;
            _unk2 = Header->_unk2;
            _actionOffset = Header->_actionOffset;

            return _actionOffset > 0;
        }

        public override void OnPopulate()
        {
            new MoveDefActionNode("Action", false, this).Initialize(this, BaseAddress + _actionOffset, 0);
        }

        public override int OnCalculateSize(bool force)
        {
            _lookupCount = Children.Count > 0 ? 1 : 0;
            return 12;
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            _entryOffset = address;
            FDefUnk22* data = (FDefUnk22*) address;
            data->_unk1 = _unk1;
            data->_unk2 = _unk2;
            data->_actionOffset = (int) (Children[0] as MoveDefActionNode)._entryOffset - (int) _rebuildBase;

            _lookupOffsets.Add((int) data->_actionOffset.Address - (int) _rebuildBase);
        }
    }
}