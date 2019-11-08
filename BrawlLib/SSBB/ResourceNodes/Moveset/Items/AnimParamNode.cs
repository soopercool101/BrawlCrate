using BrawlLib.Internal;
using BrawlLib.SSBB.Types;
using System.Collections.Generic;
using System.ComponentModel;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class MoveDefAnimParamNode : MoveDefEntryNode
    {
        internal AnimParamHeader* Header => (AnimParamHeader*) WorkingUncompressed.Address;

        [Category("Data Offsets")] public int SubactionFlags => Header->Unknown0;

        [Category("Data Offsets")] public int SubactionFlagsCount => Header->Unknown1;

        [Category("Data Offsets")] public int ActionFlags => Header->Unknown2;

        [Category("Data Offsets")] public int ActionFlagsCount => Header->Unknown3;

        [Category("Data Offsets")] public int Unk4 => Header->Unknown4;

        [Category("Data Offsets")] public int Unk5 => Header->Unknown5;

        [Category("Data Offsets")] public int Unk6 => Header->Unknown6;

        [Category("Data Offsets")] public int Unk7 => Header->Unknown7;

        [Category("Data Offsets")] public int Unk8 => Header->Unknown8;

        [Category("Data Offsets")] public int Unk9 => Header->Unknown9;

        [Category("Data Offsets")] public int Unk10 => Header->Unknown10;

        [Category("Data Offsets")] public int Unk11 => Header->Unknown11;

        [Category("Data Offsets")] public int HitData => Header->Unknown12;

        [Category("Data Offsets")] public int Unk13 => Header->Unknown13;

        [Category("Data Offsets")] public int CollisionData => Header->Unknown14;

        [Category("Data Offsets")] public int Unk15 => Header->Unknown15;

        public MoveDefAnimParamNode(string name)
        {
            _name = name;
        }

        public override bool OnInitialize()
        {
            base.OnInitialize();

            return true;
        }

        public VoidPtr dataHeaderAddr;

        public override void OnPopulate()
        {
            #region Populate

            #endregion

            SortChildren();
        }

        public void PopulateActionGroup(ResourceNode g, List<int> ActionOffsets, bool subactions, int index)
        {
            string innerName = "";
            if (subactions)
            {
                if (index == 0)
                {
                    innerName = "Main";
                }
                else if (index == 1)
                {
                    innerName = "GFX";
                }
                else if (index == 2)
                {
                    innerName = "SFX";
                }
                else if (index == 3)
                {
                    innerName = "Other";
                }
                else
                {
                    return;
                }
            }
            else if (index == 0)
            {
                innerName = "Entry";
            }
            else if (index == 1)
            {
                innerName = "Exit";
            }

            int i = 0;
            foreach (int offset in ActionOffsets)
            {
                //if (i >= g.Children.Count)
                //    if (subactions)
                //        g.Children.Add(new MoveDefSubActionGroupNode() { _name = "Extra" + i, _flags = new AnimationFlags(), _inTransTime = 0, _parent = g });
                //    else
                //        g.Children.Add(new MoveDefGroupNode() { _name = "Extra" + i, _parent = g });

                if (offset > 0)
                {
                    new MoveDefActionNode(innerName, false, g.Children[i]).Initialize(g.Children[i],
                        new DataSource(BaseAddress + offset, 0));
                }
                else
                {
                    g.Children[i].Children.Add(new MoveDefActionNode(innerName, true, g.Children[i]));
                }

                i++;
            }
        }
    }
}