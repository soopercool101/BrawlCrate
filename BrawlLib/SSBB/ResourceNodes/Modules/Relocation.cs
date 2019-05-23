using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BrawlLib.SSBBTypes;
using System.IO;
using System.ComponentModel;
using System.PowerPcAssembly;
using System.Drawing;
using Be.Windows.Forms;

namespace BrawlLib.SSBB.ResourceNodes
{   
    ///<summary>Represents 4 bytes in a module memory section.</summary>
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public unsafe class Relocation
    {
        public Relocation(ModuleDataNode d, int i)
        {
            _section = d;
            _index = i;
        }

        public ModuleDataNode _section;
        public int _index;

        public bool 
            _prolog = false, 
            _epilog = false, 
            _unresolved = false, 
            _name = false,
            _initializer = false, 
            _finalizer = false;

        [Category("Relocation Data"), Browsable(false)]
        public RelCommand Command { get { return _command; } set { SetCommand(value); } }
        private RelCommand _command;

        [Browsable(false)]
        public PPCOpCode Code
        {
            get
            {
                return RawValue;
            }
        }

        [Browsable(false)]
        public Relocation Target { get { return _target; } set { SetTarget(value); } }
        private Relocation _target;

        [Browsable(false)]
        public VoidPtr Address { get { return _section.WorkingUncompressed.Address[_index, 4]; } }

        [Browsable(false)]
        public uint RawValue 
        {
            get
            {
                if (_section._linkedEditor == null)
                    return _section.BufferAddress[_index];
                else
                {
                    long t = _index * 4;
                    HexBox hexBox1 = _section._linkedEditor.hexBox1;
                    byte[] bytes = new byte[]
                    {
                        hexBox1.ByteProvider.ReadByte(t + 3),
                        hexBox1.ByteProvider.ReadByte(t + 2),
                        hexBox1.ByteProvider.ReadByte(t + 1),
                        hexBox1.ByteProvider.ReadByte(t + 0),
                    };

                    uint val = BitConverter.ToUInt32(bytes, 0);
                    if ((PPCOpCode)val is PPCBranch && !((PPCOpCode)val is PPCblr || (PPCOpCode)val is PPCbctr))
                    {
                        PPCBranch branchOp = (PPCBranch)val;
                        int index =
                    !branchOp.Absolute ?
                    (_index * 4 + branchOp.DataOffset).RoundDown(4) / 4 :
                    -1;
                        Relocation r = _section.GetRelocationAtIndex(index);
                        if (!r.Branched.Contains(this))
                            r.Branched.Add(this);
                    }

                    return val;
                }
            }
            set
            {
                if (_section._linkedEditor == null)
                    _section.BufferAddress[_index] = value;
                else
                {
                    long t = _index * 4;
                    HexBox hexBox1 = _section._linkedEditor.hexBox1;
                    byte[] bytes = BitConverter.GetBytes(value);
                    hexBox1.ByteProvider.WriteByte(t + 3, bytes[0]);
                    hexBox1.ByteProvider.WriteByte(t + 2, bytes[1]);
                    hexBox1.ByteProvider.WriteByte(t + 1, bytes[2]);
                    hexBox1.ByteProvider.WriteByte(t + 0, bytes[3]);
                }
            } 
        }
        [Browsable(false)]
        public uint RelOffset { get { return (Command != null ? Command.Apply(true) : RawValue); } }
        [Browsable(false)]
        public uint SectionOffset { get { return (Command != null ? Command.Apply(false) : RawValue); } }

        [Browsable(false)]
        public Relocation Next { get { return NextAt(1); } }
        [Browsable(false)]
        public Relocation Previous { get { return NextAt(-1); } }

        public Relocation NextAt(int count)
        {
            int newIndex = _index + count;
            if (newIndex < 0 || newIndex >= _section.Relocations.Count)
                return null;
            else
                return _section[newIndex];
        }

        private void SetCommand(RelCommand command)
        {
            if (_command == command)
                return;

            if (_command != null)
            {
                SetTarget(null);
                _command._parentRelocation = null;
            }

            if ((_command = command) != null)
            {
                _command._parentRelocation = this;
                SetTarget(_command.GetTargetRelocation());
            }

        }

        private void SetTarget(Relocation target)
        {
            if (_target == target)
                return;

            if (_target != null)
                _target.Unlink(this);

            _target = target;

            if (_target != null)
                _target.Link(this);
        }

        [Category("Relocation Data"), Browsable(true)]
        public BindingList<Relocation> Linked { get { return _linked; } }
        private BindingList<Relocation> _linked = new BindingList<Relocation>();

        public BindingList<Relocation> Branched { get { return _branched; } }
        private BindingList<Relocation> _branched = new BindingList<Relocation>();

        internal void Link(Relocation r) 
        {
            _linked.Add(r);
            //CheckStructors();
        }
        internal void Unlink(Relocation r) 
        {
            _linked.Remove(r);
            //CheckStructors();
        }

        private void CheckStructors()
        {
            _initializer = false;
            _finalizer = false;
            foreach (Relocation r1 in _linked)
                if (r1._section.Index == 2)
                    _initializer = true;
                else if (r1._section.Index == 3)
                    _finalizer = true;
        }

        private List<object> _tags = new List<object>();
        public List<object> Tags { get { return _tags; } }

        [Browsable(false)]
        public string Notes { get { return string.Join(", ", _tags.OfType<string>().ToArray()); ; } }
        [Browsable(false)]
        public string Description
        {
            get
            {
                if (_command == null && Notes == "")
                    return "";

                if (_command != null)
                {
                    string id;
                    //if (_command._moduleID == _data.Root.ModuleID)
                    //    id = _data.Root.Name;
                    //else
                    if (RELNode._idNames.ContainsKey((int)_command._moduleID))
                        id = RELNode._idNames[(int)_command._moduleID];
                    else
                        id = "m" + _command._moduleID.ToString();

                    return
                        (_command._moduleID != 0 ? String.Format("{0}[{1}]", id, _command._targetSectionId.ToString()) : "") +
                        ((_target == null || _target.Notes == "") ? "0x" + _command._addend.ToString("X2") : String.Format("[{0}]", _target.Notes));
                }
                else
                    return Notes;
            }
        }

        public override string ToString()
        {
            int i = (int)(_section.Root as ModuleNode).ID;
            string id = RELNode._idNames.ContainsKey(i) ? RELNode._idNames[i] : "m" + i.ToString();
            return String.Format("{0}[{1}]0x{2}", id, _section.Index, (_index * 4).ToString("X"));
        }

        private bool SpecificFunc()
        {
            return _prolog || _epilog || _unresolved || _initializer || _finalizer || _name;
        }

        public Color Color
        {
            get
            {
                if (!SpecificFunc())
                    return _color;
                else
                    return _funcColor;
            }
            set 
            {
                _color = value; 
                _colorBrush = new SolidBrush(_color);
            }
        }
        public Brush ColorBrush
        {
            get
            {
                if (!SpecificFunc())
                    return _colorBrush;
                else
                    return _funcBrush;
            }
        }

        public Color _color = Color.Transparent;
        public Brush _colorBrush;
        public static Color _funcColor = Color.FromArgb(255, 200, 255, 0);
        public static Brush _funcBrush = new SolidBrush(_funcColor);

        public bool _selected = false;
    }
}
