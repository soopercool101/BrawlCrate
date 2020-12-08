using BrawlLib.Internal;
using BrawlLib.SSBB.Types;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace BrawlLib.SSBB.ResourceNodes
{
    public class MDL0DefNode : MDL0EntryNode
    {
        internal List<object> _items = new List<object>();
        private int _len;

        //internal List<MDL0Node2Class> _items2 = new List<MDL0Node2Class>();
        //internal List<MDL0Node3Class> _items3 = new List<MDL0Node3Class>();
        //internal List<MDL0NodeType4> _items4 = new List<MDL0NodeType4>();
        //internal List<MDL0NodeType5> _items5 = new List<MDL0NodeType5>();

        //[Category("MDL0 Nodes")]
        //public List<MDL0Node2Class> NodeType2Items { get { return _items2; } }
        //[Category("MDL0 Nodes")]
        //public List<MDL0Node3Class> NodeType3Items { get { return _items3; } }
        //[Category("MDL0 Nodes")]
        //public List<MDL0NodeType4> NodeType4Items { get { return _items4; } }
        //[Category("MDL0 Nodes")]
        //public List<MDL0NodeType5> NodeType5Items { get { return _items5; } }

        [Category("MDL0 Nodes")] public int DataLength => _len;

        [Category("MDL0 Nodes")]
        public object[] Items
        {
            get => _items.ToArray();
            set => _items = value.ToList();
        }

        public override bool OnInitialize()
        {
            VoidPtr addr = WorkingUncompressed.Address;
            object n = null;
            while ((n = MDL0NodeClass.Create(ref addr)) != null)
            {
                _items.Add(n);
            }

            //while ((n = MDL0NodeClass.Create(ref addr)) != null)
            //{
            //    if (n is MDL0Node2Class)
            //        _items2.Add(n as MDL0Node2Class);
            //    else if (n is MDL0Node3Class)
            //        _items3.Add(n as MDL0Node3Class);
            //    else if (n is MDL0NodeType4)
            //        _items4.Add((MDL0NodeType4)n);
            //    else if (n is MDL0NodeType5)
            //        _items5.Add((MDL0NodeType5)n);
            //}

            _len = addr - WorkingUncompressed.Address;
            SetSizeInternal(_len);

            return false;
        }
    }
}