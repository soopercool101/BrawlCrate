using BrawlLib.SSBB.ResourceNodes;
using BrawlLib.SSBB.Types.Subspace;

namespace BrawlLib.SSBB
{
    public class DifficultyRatiosClass
    {
        private ResourceNode _parent;
        private DifficultyRatios Data;
        
        public float Difficulty1
        {
            get => Data._difficulty1;
            set
            {
                Data._difficulty1 = value;
                _parent.SignalPropertyChange();
            }
        }

        public float Difficulty2
        {
            get => Data._difficulty2;
            set
            {
                Data._difficulty2 = value;
                _parent.SignalPropertyChange();
            }
        }

        public float Difficulty3
        {
            get => Data._difficulty3;
            set
            {
                Data._difficulty3 = value;
                _parent.SignalPropertyChange();
            }
        }

        public float Difficulty4
        {
            get => Data._difficulty4;
            set
            {
                Data._difficulty4 = value;
                _parent.SignalPropertyChange();
            }
        }

        public float Difficulty5
        {
            get => Data._difficulty5;
            set
            {
                Data._difficulty5 = value;
                _parent.SignalPropertyChange();
            }
        }

        public float Difficulty6
        {
            get => Data._difficulty6;
            set
            {
                Data._difficulty6 = value;
                _parent.SignalPropertyChange();
            }
        }

        public float Difficulty7
        {
            get => Data._difficulty7;
            set
            {
                Data._difficulty7 = value;
                _parent.SignalPropertyChange();
            }
        }

        public float Difficulty8
        {
            get => Data._difficulty8;
            set
            {
                Data._difficulty8 = value;
                _parent.SignalPropertyChange();
            }
        }

        public float Difficulty9
        {
            get => Data._difficulty9;
            set
            {
                Data._difficulty9 = value;
                _parent.SignalPropertyChange();
            }
        }

        public float Difficulty10
        {
            get => Data._difficulty10;
            set
            {
                Data._difficulty10 = value;
                _parent.SignalPropertyChange();
            }
        }

        public float Difficulty11
        {
            get => Data._difficulty11;
            set
            {
                Data._difficulty11 = value;
                _parent.SignalPropertyChange();
            }
        }

        public float Difficulty12
        {
            get => Data._difficulty12;
            set
            {
                Data._difficulty12 = value;
                _parent.SignalPropertyChange();
            }
        }

        public float Difficulty13
        {
            get => Data._difficulty13;
            set
            {
                Data._difficulty13 = value;
                _parent.SignalPropertyChange();
            }
        }

        public float Difficulty14
        {
            get => Data._difficulty14;
            set
            {
                Data._difficulty14 = value;
                _parent.SignalPropertyChange();
            }
        }

        public float Difficulty15
        {
            get => Data._difficulty15;
            set
            {
                Data._difficulty15 = value;
                _parent.SignalPropertyChange();
            }
        }

        public override string ToString()
        {
            return "";
        }

        public DifficultyRatiosClass(ResourceNode parent)
        {
            _parent = parent;
        }

        public DifficultyRatiosClass(ResourceNode parent, DifficultyRatios data)
        {
            _parent = parent;
            Data = data;
        }

        public static implicit operator DifficultyRatios(DifficultyRatiosClass val)
        {
            return val.Data;
        }
    }
}