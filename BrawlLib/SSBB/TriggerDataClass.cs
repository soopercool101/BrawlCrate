using BrawlLib.Internal;
using BrawlLib.SSBB.ResourceNodes;
using BrawlLib.SSBB.Types.Subspace.Triggers;
using System.ComponentModel;

namespace BrawlLib.SSBB
{
    internal class TriggerDataClass
    {
        private ResourceNode _parent;
        private TriggerData Data = new TriggerData();

        public ushort TriggerID
        {
            get => Data._triggerId;
            set
            {
                Data._triggerId = value;
                _parent?.SignalPropertyChange();
            }
        }

        public bool IsValid
        {
            get => Data._isValid == 1;
            set
            {
                Data._isValid = value ? (byte)1 : (byte)0;
                _parent?.SignalPropertyChange();
            }
        }

        [TypeConverter(typeof(HexByteConverter))]
        public byte Unknown0x3
        {
            get => Data._unknown0x3;
            set
            {
                Data._unknown0x3 = value;
                _parent?.SignalPropertyChange();
            }
        }

        public TriggerDataClass(ResourceNode parent)
        {
            _parent = parent;
        }

        public TriggerDataClass(ResourceNode parent, TriggerData data)
        {
            _parent = parent;
            Data = data;
        }

        public static implicit operator TriggerData(TriggerDataClass val)
        {
            return val.Data;
        }
    }
}
