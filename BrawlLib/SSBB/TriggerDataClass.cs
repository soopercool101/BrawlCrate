using BrawlLib.Internal;
using BrawlLib.SSBB.ResourceNodes;
using BrawlLib.SSBB.Types.Subspace.Triggers;
using System.ComponentModel;

namespace BrawlLib.SSBB
{
    public class TriggerDataClass
    {
        private ResourceNode _parent;
        private TriggerData Data;

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
            get => Data._isValid;
            set
            {
                Data._isValid = value;
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

        public override string ToString()
        {
            string validState = IsValid ? "Valid" : "Invalid";
            string unknownValue = Unknown0x3 == 0 ? "" : $" 0x{Unknown0x3:X2}";
            return $"Trigger #{TriggerID} [{validState}]{unknownValue}";
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
