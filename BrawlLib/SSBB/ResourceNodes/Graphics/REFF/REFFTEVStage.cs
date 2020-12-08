using BrawlLib.Wii.Graphics;
using System.ComponentModel;

namespace BrawlLib.SSBB.ResourceNodes
{
    public class REFFTEVStage : ResourceNode
    {
        public REFFTEVStage(int index)
        {
            _name = $"Stage{index}";
        }

        public override ResourceType ResourceFileType => ResourceType.TEVStage;

        [Category("c TEV Color Env")]
        [Browsable(true)]
        public string ColorOutput
        {
            get
            {
                int op = (int) ColorOperation;
                if (op < 2)
                {
                    return (ColorClamp ? "clamp(" : "") + "(d " + (op == 1 ? "-" : "+") + " ((1 - c) * a + c * b) + " +
                           ((int) ColorBias == 1 ? "0.5" : (int) ColorBias == 2 ? "-0.5" : "0") + ") * " +
                           ((int) ColorScale == 3 ? "0.5" : ColorScale == 0 ? "1" : ((int) ColorScale * 2).ToString()) +
                           (ColorClamp ? ");" : ";");
                }

                if (op > 13)
                {
                    return "d[x] + ((a[x] " + (op % 2 == 0 ? ">" : "==") + " b[x]) ? c[x] : 0 );";
                }

                return "d + ((a[" + (op < 10 ? "R" : op < 12 ? "GR" : "BGR") + "] " + (op % 2 == 0 ? ">" : "==") +
                       " b[" + (op < 10 ? "R" : op < 12 ? "GR" : "BGR") + "]) ? c : 0 );";
            }
        }

        [Category("d TEV Alpha Env")]
        [Browsable(true)]
        public string AlphaOutput
        {
            get
            {
                int op = (int) AlphaOperation;
                if (op < 2)
                {
                    return (AlphaClamp ? "clamp(" : "") + "(d " + (op == 1 ? "-" : "+") + " ((1 - c) * a + c * b) + " +
                           ((int) AlphaBias == 1 ? "0.5" : (int) AlphaBias == 2 ? "-0.5" : "0") + ") * " +
                           ((int) AlphaScale == 3 ? "0.5" : AlphaScale == 0 ? "1" : ((int) AlphaScale * 2).ToString()) +
                           (AlphaClamp ? ");" : ";");
                }

                if (op > 13)
                {
                    return "d[x] + ((a[x] " + (op % 2 == 0 ? ">" : "==") + " b[x]) ? c[x] : 0 );";
                }

                return "d + ((a[" + (op < 10 ? "R" : op < 12 ? "GR" : "BGR") + "] " + (op % 2 == 0 ? ">" : "==") +
                       " b[" + (op < 10 ? "R" : op < 12 ? "GR" : "BGR") + "]) ? c : 0 );";
            }
        }

        //KSel Values
        public int kcsel, kasel;

        //TRef Values
        public int ti, tc, cc;
        public bool te;

        //Color Env Values
        public int cseld, cselc, cselb, csela, cbias, cshift, cdest, cop, cclamp;

        //Alpha Env Values
        public int aseld, aselc, aselb, asela, abias, ashift, adest, aop, aclamp;

        [Category("a TEV KSel")]
        [Browsable(true)]
        public TevKColorSel KonstantColorSelection
        {
            get => (TevKColorSel) kcsel;
            set
            {
                kcsel = (int) value;
                SignalPropertyChange();
            }
        }

        [Category("a TEV KSel")]
        [Browsable(true)]
        public TevKAlphaSel KonstantAlphaSelection
        {
            get => (TevKAlphaSel) kasel;
            set
            {
                kasel = (int) value;
                SignalPropertyChange();
            }
        }

        //[Category("b TEV RAS1 TRef"), Browsable(true)]
        //public TexMapID TextureMapID { get { return (TexMapID)ti; } set { ti = (int)value; SignalPropertyChange(); } }
        //[Category("b TEV RAS1 TRef"), Browsable(true)]
        //public TexCoordID TextureCoord { get { return (TexCoordID)tc; } set { tc = (int)value; SignalPropertyChange(); } }
        //[Category("b TEV RAS1 TRef"), Browsable(true)]
        //public bool TextureEnabled { get { return te; } set { te = value; SignalPropertyChange(); } }
        //[Category("b TEV RAS1 TRef"), Browsable(true)]
        //public ColorSelChan ColorChannel { get { return (ColorSelChan)cc; } set { cc = (int)value; SignalPropertyChange(); } }

        [Category("c TEV Color Env")]
        [Browsable(true)]
        public ColorArg ColorSelectionA
        {
            get => (ColorArg) csela;
            set
            {
                csela = (int) value;
                SignalPropertyChange();
            }
        }

        [Category("c TEV Color Env")]
        [Browsable(true)]
        public ColorArg ColorSelectionB
        {
            get => (ColorArg) cselb;
            set
            {
                cselb = (int) value;
                SignalPropertyChange();
            }
        }

        [Category("c TEV Color Env")]
        [Browsable(true)]
        public ColorArg ColorSelectionC
        {
            get => (ColorArg) cselc;
            set
            {
                cselc = (int) value;
                SignalPropertyChange();
            }
        }

        [Category("c TEV Color Env")]
        [Browsable(true)]
        public ColorArg ColorSelectionD
        {
            get => (ColorArg) cseld;
            set
            {
                cseld = (int) value;
                SignalPropertyChange();
            }
        }

        [Category("c TEV Color Env")]
        [Browsable(true)]
        public Bias ColorBias
        {
            get => (Bias) cbias;
            set
            {
                cbias = (int) value;
                SignalPropertyChange();
            }
        }

        [Category("c TEV Color Env")]
        [Browsable(true)]
        public TevColorOp ColorOperation
        {
            get => (TevColorOp) cop;
            set
            {
                cop = (int) value;
                SignalPropertyChange();
            }
        }

        [Category("c TEV Color Env")]
        [Browsable(true)]
        public bool ColorClamp
        {
            get => cclamp != 0;
            set
            {
                cclamp = value ? 1 : 0;
                SignalPropertyChange();
            }
        }

        [Category("c TEV Color Env")]
        [Browsable(true)]
        public TevScale ColorScale
        {
            get => (TevScale) cshift;
            set
            {
                cshift = (int) value;
                SignalPropertyChange();
            }
        }

        [Category("c TEV Color Env")]
        [Browsable(true)]
        public TevColorRegID ColorRegister
        {
            get => (TevColorRegID) cdest;
            set
            {
                cdest = (int) value;
                SignalPropertyChange();
            }
        }

        [Category("d TEV Alpha Env")]
        [Browsable(true)]
        public AlphaArg AlphaSelectionA
        {
            get => (AlphaArg) asela;
            set
            {
                asela = (int) value;
                SignalPropertyChange();
            }
        }

        [Category("d TEV Alpha Env")]
        [Browsable(true)]
        public AlphaArg AlphaSelectionB
        {
            get => (AlphaArg) aselb;
            set
            {
                aselb = (int) value;
                SignalPropertyChange();
            }
        }

        [Category("d TEV Alpha Env")]
        [Browsable(true)]
        public AlphaArg AlphaSelectionC
        {
            get => (AlphaArg) aselc;
            set
            {
                aselc = (int) value;
                SignalPropertyChange();
            }
        }

        [Category("d TEV Alpha Env")]
        [Browsable(true)]
        public AlphaArg AlphaSelectionD
        {
            get => (AlphaArg) aseld;
            set
            {
                aseld = (int) value;
                SignalPropertyChange();
            }
        }

        [Category("d TEV Alpha Env")]
        [Browsable(true)]
        public Bias AlphaBias
        {
            get => (Bias) abias;
            set
            {
                abias = (int) value;
                SignalPropertyChange();
            }
        }

        [Category("d TEV Alpha Env")]
        [Browsable(true)]
        public TevColorOp AlphaOperation
        {
            get => (TevColorOp) aop;
            set
            {
                aop = (int) value;
                SignalPropertyChange();
            }
        }

        [Category("d TEV Alpha Env")]
        [Browsable(true)]
        public bool AlphaClamp
        {
            get => aclamp != 0;
            set
            {
                aclamp = value ? 1 : 0;
                SignalPropertyChange();
            }
        }

        [Category("d TEV Alpha Env")]
        [Browsable(true)]
        public TevScale AlphaScale
        {
            get => (TevScale) ashift;
            set
            {
                ashift = (int) value;
                SignalPropertyChange();
            }
        }

        [Category("d TEV Alpha Env")]
        [Browsable(true)]
        public TevAlphaRegID AlphaRegister
        {
            get => (TevAlphaRegID) adest;
            set
            {
                adest = (int) value;
                SignalPropertyChange();
            }
        }

        public void Default()
        {
            Name = $"Stage{Index}";

            AlphaSelectionA = AlphaArg.Zero;
            AlphaSelectionB = AlphaArg.Zero;
            AlphaSelectionC = AlphaArg.Zero;
            AlphaSelectionD = AlphaArg.Zero;
            AlphaBias = Bias.Zero;
            AlphaClamp = true;

            ColorSelectionA = ColorArg.Zero;
            ColorSelectionB = ColorArg.Zero;
            ColorSelectionC = ColorArg.Zero;
            ColorSelectionD = ColorArg.Zero;
            ColorBias = Bias.Zero;
            ColorClamp = true;

            //TextureMapID = TexMapID.TexMap7;
            //TextureCoord = TexCoordID.TexCoord7;
            //ColorChannel = ColorSelChan.Zero;
        }

        //public void SignalPropertyChange()
        //{
        //    //((MDL0ShaderNode)Parent)._renderUpdate = true;
        //    base.SignalPropertyChange();
        //}
    }
}