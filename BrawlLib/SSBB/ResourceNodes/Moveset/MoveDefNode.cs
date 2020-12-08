using BrawlLib.Internal;
using BrawlLib.Internal.IO;
using BrawlLib.SSBB.Types;
using BrawlLib.Wii;
using BrawlLib.Wii.Compression;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace BrawlLib.SSBB.ResourceNodes
{
    public class MoveDefEntryNode : ResourceNode
    {
        //Variables specific for rebuilding
        [Browsable(false)] public VoidPtr _rebuildBase => Root._rebuildBase;

        public int _lookupCount;
        public List<int> _lookupOffsets = new List<int>();

        public VoidPtr _entryOffset = 0;
        public int _entryLength, _childLength;

        [Browsable(false)] public int _rebuildOffset => (int) _entryOffset - (int) _rebuildBase;

        [Browsable(false)] public VoidPtr Data => (VoidPtr) WorkingUncompressed.Address;

        [Browsable(false)]
        public VoidPtr BaseAddress
        {
            get
            {
                if (Root == null)
                {
                    return 0;
                }

                return Root.BaseAddress;
            }
        }

        [Browsable(false)] public MDL0Node Model => Root.Model;

        [Browsable(false)]
        public MoveDefNode Root
        {
            get
            {
                ResourceNode n = _parent;
                while (!(n is MoveDefNode) && n != null)
                {
                    n = n._parent;
                }

                return n as MoveDefNode;
            }
        }

        [Category("Moveset Entry")]
        [Browsable(true)]
        public int IntOffset => _offset;

        [Browsable(false)]
        public int _offset
        {
            get
            {
                if (Data != null)
                {
                    return (int) Data - (int) BaseAddress;
                }
                else
                {
                    return 0;
                }
            }
        }

        [Category("Moveset Entry")]
        [Browsable(false)]
        public string HexOffset => "0x" + _offset.ToString("X");

        [Category("Moveset Entry")]
        [Browsable(true)]
        public int Size => WorkingUncompressed.Length;

        [Category("Moveset Entry")]
        [Browsable(true)]
        public bool External => _extNode != null;

        public override void Rebuild(bool force)
        {
            if (!IsDirty && !force)
            {
                return;
            }

            //Get uncompressed size
            int size = OnCalculateSize(force);

            //Create temp map
            FileMap uncompMap = FileMap.FromTempFile(size);

            //Rebuild node (uncompressed)
            Rebuild(uncompMap.Address, size, force);
            _replSrc.Map = _replUncompSrc.Map = uncompMap;

            //If compressed, compress resulting data.
            if (_compression != CompressionType.None)
            {
                //Compress node to temp file
                FileStream stream = new FileStream(Path.GetTempFileName(), FileMode.Open, FileAccess.ReadWrite,
                    FileShare.None, 0x8, FileOptions.DeleteOnClose | FileOptions.SequentialScan);
                try
                {
                    Compressor.Compact(_compression, _entryOffset, _entryLength, stream, this);
                    _replSrc = new DataSource(
                        FileMap.FromStreamInternal(stream, FileMapProtect.Read, 0, (int) stream.Length), _compression);
                }
                catch (Exception x)
                {
                    stream.Dispose();
                    throw x;
                }
            }
        }

        public MoveDefExternalNode _extNode;
        public bool _extOverride = false;

        private VoidPtr data = null;
        private VoidPtr dAddr => data == null ? data = Data : data;

        public int offsetID;
        public bool isExtra = false;

        public override ResourceType ResourceFileType => ResourceType.NoEditEntry;

        public override bool OnInitialize()
        {
            if (Root == null)
            {
                return base.OnInitialize();
            }

            if (_extNode == null)
            {
                _extNode = Root.IsExternal(_offset);
                if (_extNode != null && !_extOverride)
                {
                    _name = _extNode.Name;
                    _extNode._refs.Add(this);
                }
            }

            //if (Index <= 30)
            //    Root._paths[_offset] = TreePath;
            if (!MoveDefNode.nodeDictionary.ContainsKey(_offset))
            {
                MoveDefNode.nodeDictionary.Add(_offset, this);
            }

            if (Size == 0)
            {
                int size = Root.GetSize(_offset);
                if (size > 0)
                {
                    SetSizeInternal(size);
                }
            }

            return base.OnInitialize();
        }

        public ActionEventInfo GetEventInfo(long id)
        {
            if (MoveDefNode.EventDictionary == null)
            {
                MoveDefNode.LoadEventDictionary();
            }

            if (MoveDefNode.EventDictionary.ContainsKey(id))
            {
                return MoveDefNode.EventDictionary[id];
            }

            return new ActionEventInfo(id, id.ToString("X"), "No Description Available.", null, null);
        }

        public override void SortChildren()
        {
            _children.Sort(Compare);
        }

        public static int Compare(ResourceNode n1, ResourceNode n2)
        {
            if (((MoveDefEntryNode) n1)._offset < ((MoveDefEntryNode) n2)._offset)
            {
                return -1;
            }

            if (((MoveDefEntryNode) n1)._offset > ((MoveDefEntryNode) n2)._offset)
            {
                return 1;
            }

            return 0;
        }

        public static int ActionCompare(ResourceNode n1, ResourceNode n2)
        {
            if (((MoveDefEntryNode) n1.Children[0])._offset < ((MoveDefEntryNode) n2.Children[0])._offset)
            {
                return -1;
            }

            if (((MoveDefEntryNode) n1.Children[0])._offset > ((MoveDefEntryNode) n2.Children[0])._offset)
            {
                return 1;
            }

            return 0;
        }

        public ResourceNode FindNode(int offset)
        {
            ResourceNode n;
            if (offset == _offset)
            {
                return this;
            }
            else
            {
                foreach (MoveDefEntryNode e in Children)
                {
                    if ((n = e.FindNode(offset)) != null)
                    {
                        return n;
                    }
                }
            }

            return null;
        }

        public ResourceNode GetByOffsetID(int id)
        {
            foreach (MoveDefEntryNode e in Children)
            {
                if (e.offsetID == id)
                {
                    return e;
                }
            }

            return null;
        }

        public virtual void PostProcess()
        {
        }
    }

    public abstract class MoveDefExternalNode : MoveDefEntryNode
    {
        public override ResourceType ResourceFileType => ResourceType.NoEditEntry;

        public List<int> _offsets = new List<int>();
        public List<MoveDefEntryNode> _refs = new List<MoveDefEntryNode>();

        public MoveDefEntryNode[] References => _refs.ToArray();

        public override void Remove()
        {
            foreach (MoveDefEntryNode e in _refs)
            {
                e._extNode = null;
            }

            base.Remove();
        }
    }

    public unsafe class MoveDefNode : ARCEntryNode
    {
        internal FDefHeader* Header => (FDefHeader*) WorkingUncompressed.Address;
        internal int dataSize, lookupOffset, numLookupEntries, numDataTable, numExternalSubRoutine;

        internal static ResourceNode TryParseGeneric(DataSource source, ResourceNode parent)
        {
            VoidPtr addr = source.Address;
            FDefHeader* header = (FDefHeader*) addr;

            if (header->_pad1 != 0 || header->_pad2 != 0 || header->_pad3 != 0 || header->_fileSize != source.Length ||
                header->_lookupOffset > source.Length || !Properties.Settings.Default.ParseMoveDef)
            {
                return null;
            }

            return new MoveDefNode();
        }

        public static SortedDictionary<long, ActionEventInfo> EventDictionary =
            new SortedDictionary<long, ActionEventInfo>();

        public static bool _dictionaryChanged = false;

        #region Event Dictionary

        public static void LoadEventDictionary()
        {
            EventDictionary = new SortedDictionary<long, ActionEventInfo>();
            EventDictionary.Add(0x00010100, new ActionEventInfo(0x00010100, "Synchronous Timer",
                "Pause the current flow of events until the set time is reached. Synchronous timers count down when they are reached in the code.",
                new string[] {"Frames"},
                new string[] {"The number of frames to wait."},
                "\\name(): frames=\\value(0)",
                new long[] {1}));
            EventDictionary.Add(0x00020000, new ActionEventInfo(0x00020000, "Nop",
                "No action.",
                new string[] { },
                new string[] { },
                "",
                new long[] { }));
            EventDictionary.Add(0x00020100, new ActionEventInfo(0x00020100, "Asynchronous Timer",
                "Pause the current flow of events until the set time is reached. Asynchronous Timers start counting from the beginning of the animation.",
                new string[] {"Frames"},
                new string[] {"The number of frames to wait."},
                "\\name(): frames=\\value(0)",
                new long[] {1}));
            EventDictionary.Add(0x00040100, new ActionEventInfo(0x00040100, "Set Loop",
                "Set a loop for X iterations.",
                new string[] {"Iterations"},
                new string[] {"The number of times to loop."},
                "\\name() \\if(\\unhex(\\value(0)),==,-1, Infinite, \\unhex(\\value(0)) Times)",
                new long[] {0}));
            EventDictionary.Add(0x00050000, new ActionEventInfo(0x00050000, "Execute Loop",
                "Execute the the previously set loop.",
                new string[] { },
                new string[] { },
                "",
                new long[] { }));
            EventDictionary.Add(0x00070100, new ActionEventInfo(0x00070100, "Subroutine",
                "Enter the event routine specified and return after ending.",
                new string[] {"Offset"},
                new string[] {"The offset inside the file to jump to."},
                "\\name() \\value(0)",
                new long[] {2}));
            EventDictionary.Add(0x00080000, new ActionEventInfo(0x00080000, "Return",
                "Return from a Subroutine.",
                new string[] { },
                new string[] { },
                "",
                new long[] { }));
            EventDictionary.Add(0x00090100, new ActionEventInfo(0x00090100, "Goto",
                "Goto the event location specified and execute.",
                new string[] {"Offset"},
                new string[] {"The offset inside the file to jump to."},
                "\\name() \\value(0)",
                new long[] {2}));
            EventDictionary.Add(0x000A0100, new ActionEventInfo(0x000A0100, "If",
                "Start an If block until an Else Or an EndIf is reached.",
                new string[] {"Requirement"},
                new string[] {"The form of requirement used in evaluation of the if statement."},
                "\\name() \\value(0):",
                new long[] {6}));
            EventDictionary.Add(0x000A0200, new ActionEventInfo(0x000A0200, "If Value",
                "Start an If block until an Else Or an EndIf is reached.",
                new string[] {"Requirement", "Variable"},
                new string[]
                {
                    "The form of requirement used in evaluation of the event.",
                    "The variable applied to the requirement."
                },
                "If \\value(0): \\value(1)",
                new long[] {6, 0}));
            EventDictionary.Add(0x000A0400, new ActionEventInfo(0x000A0400, "If Comparison",
                "Start an If block until an Else Or an EndIf is reached.",
                new string[] {"Requirement", "Variable", "Comparison Method", "Variable"},
                new string[]
                {
                    "The form of requirement used in evaluation of the event.",
                    "The first variable in the comparison requirement.",
                    "The method used to compare the two variables. From 0 to 5: <, ≤, =, ≠, ≥, >",
                    "The second variable in the comparison requirement."
                },
                "If \\value(0): \\value(1) \\cmpsign(\\value(2)) \\value(3)",
                new long[] {6, 5, 0, 5}));
            EventDictionary.Add(0x000E0000, new ActionEventInfo(0x000E0000, "Else",
                "Insert an Else block inside an If block.",
                new string[] { },
                new string[] { },
                "",
                new long[] { }));
            EventDictionary.Add(0x000B0400, new ActionEventInfo(0x000B0400, "And Comparison",
                "Seems to be an \"And\" to an If statement.",
                new string[] {"Requirement", "Variable", "Comparison Method", "Variable"},
                new string[]
                {
                    "The form of requirement used in evaluation of the event.",
                    "The first variable in the comparison requirement.",
                    "The method used to compare the two variables. From 0 to 5: <, ≤, =, ≠, ≥, > ",
                    "The second variable in the comparison requirement."
                },
                "And \\value(0): \\value(1) \\cmpsign(\\value(2)) \\value(3)",
                new long[] {6, 5, 0, 5}));
            EventDictionary.Add(0x000D0400, new ActionEventInfo(0x000D0400, "Else If Comparison",
                "Insert an Else If block inside of an If block.",
                new string[] {"Requirement", "Variable", "Comparison Method", "Variable"},
                new string[]
                {
                    "The form of requirement used in evaluation of the event.",
                    "The first variable in the comparison requirement.",
                    "The method used to compare the two variables. From 0 to 5: <, ≤, =, ≠, ≥, > ",
                    "The second variable in the comparison requirement."
                },
                "Else If \\value(0): \\value(1) \\cmpsign(\\value(2)) \\value(3)",
                new long[] {6, 5, 0, 5}));
            EventDictionary.Add(0x000F0000, new ActionEventInfo(0x000F0000, "End If",
                "End an If block.",
                new string[] { },
                new string[] { },
                "",
                new long[] { }));
            EventDictionary.Add(0x00100200, new ActionEventInfo(0x00100200, "Switch",
                "Begin a multiple case Switch block.",
                new string[] {"Undefined", "Undefined"},
                new string[]
                {
                    "Any type? Has shown to be an IC-Basic \"disguised\" as a value. For example, set as Value 0x3E9 but really uses IC-Basic[1001] (the equivalent).",
                    "Any Type? Has shown to be a Requirement \"disguised\" as a value. For example, set as Value 2B but really uses 2B (Roll A Die)."
                },
                "\\name() (\\unhex(\\value(0)), \\value(1))",
                new long[] {0, 0}));
            EventDictionary.Add(0x00110100, new ActionEventInfo(0x00110100, "Case",
                "Handler for if the variable in the switch statement equals the specified value.",
                new string[] {"Value"},
                new string[] {"The value applied to the argument."},
                "\\name() \\unhex(\\value(0)):",
                new long[] {0}));
            EventDictionary.Add(0x00120000, new ActionEventInfo(0x00120000, "Default Case",
                "The case chosen if none of the others are executed.",
                new string[] { },
                new string[] { },
                "\\name():",
                new long[] { }));
            EventDictionary.Add(0x00130000, new ActionEventInfo(0x00130000, "End Switch",
                "End a Switch block.",
                new string[] { },
                new string[] { },
                "",
                new long[] { }));
            EventDictionary.Add(0x01010000, new ActionEventInfo(0x01010000, "Loop Rest",
                "Briefly return execution back to the system to prevent crashes during infinite loops.",
                new string[] { },
                new string[] { },
                "",
                new long[] { }));
            EventDictionary.Add(0x02000300, new ActionEventInfo(0x02000300, "Change Action Status",
                "Change the current action upon the specified requirement being met (the requirement does not have to be met at the time this ID is executed - it can be used anytime after execution.)",
                new string[] {"Status ID", "Action", "Requirement"},
                new string[]
                {
                    "The four-digit status ID of the change action event. Can later be disabled via 02080100 (Disable Action Status ID).",
                    "The ID of the action that the character will execute.",
                    "The form of requirement used in evaluation of the event."
                },
                "Prioritized Change Action: priority=\\value(0), action=\\unhex(\\value(1)), requirement=\\value(2)",
                new long[] {0, 0, 6}));
            EventDictionary.Add(0x02010200, new ActionEventInfo(0x02010200, "Change Action",
                "Change the current action upon the specified requirement being met. (the requirement does not have to be met at the time this ID is executed - it can be used anytime after execution.)",
                new string[] {"Action", "Requirement"},
                new string[]
                {
                    "The id of the action that the character will execute.",
                    "The form of requirement used in evaluation of the event."
                },
                "\\name() action=\\unhex(\\value(0)), requirement=\\value(1)",
                new long[] {0, 6}));
            EventDictionary.Add(0x02010300, new ActionEventInfo(0x02010300, "Change Action",
                "Change the current action upon the specified requirement being met. (the requirement does not have to be met at the time this ID is executed - it can be used anytime after execution.)",
                new string[] {"Action", "Requirement", "Variable"},
                new string[]
                {
                    "The id of the action that the character will execute.",
                    "The form of requirement used in evaluation of the event.",
                    "The variable applied to the requirement."
                },
                "\\name() action=\\unhex(\\value(0)), requirement=\\value(1): \\value(2)",
                new long[] {0, 6, 5}));
            EventDictionary.Add(0x02010500, new ActionEventInfo(0x02010500, "Change Action",
                "Change the current action upon the specified requirement being met. (the requirement does not have to be met at the time this ID is executed - it can be used anytime after execution.)",
                new string[] {"Action", "Requirement", "Variable", "Comparison Method", "Variable"},
                new string[]
                {
                    "The id of the action that the character will execute.",
                    "The form of requirement used in evaluation of the event.",
                    "The first variable in the comparison requirement.",
                    "The method used to compare the two variables. From 0 to 5: <, ≤, =, ≠, ≥, > ",
                    "The second variable in the comparison requirement."
                },
                "\\name() action=\\unhex(\\value(0)), requirement=\\value(1): \\value(2) \\cmpsign(\\value(3)) \\value(4)",
                new long[] {0, 6, 5, 0, 5}));
            EventDictionary.Add(0x02040100, new ActionEventInfo(0x02040100, "Additional Change Action Requirement",
                "Add an additional requirement to the preceeding Change Action statement.",
                new string[] {"Requirement"},
                new string[] {"The form of requirement used in evaluation of the event."},
                "\\name() \\value(0)",
                new long[] {6}));
            EventDictionary.Add(0x02040200, new ActionEventInfo(0x02040200,
                "Additional Change Action Requirement Value",
                "Add an additional requirement to the preceeding Change Action statement.",
                new string[] {"Requirement", "Variable"},
                new string[]
                {
                    "The form of requirement used in evaluation of the event.",
                    "The variable applied to the requirement."
                },
                "\\name() \\value(0): \\value(1)",
                new long[] {6, 5}));
            EventDictionary.Add(0x02040400, new ActionEventInfo(0x02040400,
                "Additional Change Action Requirement Comparison",
                "Add an additional requirement to the preceeding Change Action statement.",
                new string[] {"Requirement", "Variable", "Comparison Method", "Variable"},
                new string[]
                {
                    "The form of requirement used in evaluation of the event.",
                    "The first variable in the comparison requirement.",
                    "The method used to compare the two variables. From 0 to 5: <, ≤, =, ≠, ≥, > ",
                    "The second variable in the comparison requirement."
                },
                "\\name() \\value(0): \\value(1) \\cmpsign(\\value(2)) \\value(3)",
                new long[] {6, 5, 0, 5}));
            EventDictionary.Add(0x02060100, new ActionEventInfo(0x02060100, "Enable Action Status ID",
                "Enables the given Status ID.",
                new string[] {"Status ID"},
                new string[] {"The Status ID to enable."},
                "\\name(): \\unhex(\\value(0))",
                new long[] {0}));
            EventDictionary.Add(0x04000100, new ActionEventInfo(0x04000100, "Change Subaction",
                "Change the current subaction.",
                new string[] {"Subaction"},
                new string[] {"The ID of the subaction that the character will execute."},
                "\\name(): sub action=\\value(0)",
                new long[] {0}));
            EventDictionary.Add(0x04000200, new ActionEventInfo(0x04000200, "Change Subaction",
                "Change the current subaction. Specifies whether or not to pass the current frame or start the animation over.",
                new string[] {"Subaction", "Pass Frame"},
                new string[]
                {
                    "The ID of the subaction that the character will execute.",
                    "Whether to pass the current frame of the current animation onto the new animation or not."
                },
                "\\name(): sub action=\\value(0), pass frame=\\value(1)",
                new long[] {0, 3}));
            EventDictionary.Add(0x05000000, new ActionEventInfo(0x05000000, "Reverse Direction",
                "Reverse the direction the character is facing after the animation ends.",
                new string[] { },
                new string[] { },
                "",
                new long[] { }));
            EventDictionary.Add(0x06000D00, new ActionEventInfo(0x06000D00, "Offensive Collision",
                "Generate an offensive collision bubble with the specified parameters.",
                new string[]
                {
                    "Bone/Id", "Damage", "Trajectory", "Weight Knockback/Knockback Growth",
                    "Shield Damage/Base Knockback", "Size", "X Offset", "Y Offset", "Z Offset", "Tripping Rate",
                    "Hitlag Multiplier", "Directional Influence Multiplier", "Flags"
                },
                new string[]
                {
                    "Value1 = The bone the collision bubble is attached to. Value2 = The id number of the collision bubble.",
                    "The amount of damage inflicted to the target upon collision. ",
                    "The direction in which a target gets launched.",
                    "Value1 = The distance the target is launched proportional to weight for fixed knockback hits. Value2 = The additional distance the target is launched proportional to its damage (launch force for fixed knockback hits).",
                    "Value1 = The amount of damage dealt to the target's shield if it is up. Value2 = The distance the target is launched regardless of its damage (zero for fixed knockback hits).",
                    "The size of the collision bubble.",
                    "The amount the collision bubble is transitioned relative to the currently attached bone.",
                    "The amount the collision bubble is transitioned relative to the currently attached bone.",
                    "The amount the collision bubble is transitioned relative to the currently attached bone.",
                    "The percent possibility of the collision bubble inducing a trip, provided the target doesn't leave the ground from the knockback.",
                    "A multiplier affecting the time in which both parties pause when the collision bubble connects.",
                    "A multiplier affecting the ability for the character to maneuver themselves while suffering from the hitlag generated by this collision bubble.",
                    "Flags for various parameters such as hit effects and sound effects."
                },
                "\\name(): Id=\\unhex(\\half2(\\value(0))), Bone=\\bone(\\half1(\\value(0))), Damage=\\unhex(\\value(1)), ShieldDamage=\\unhex(\\half1(\\value(4))), Direction=\\unhex(\\value(2)), BaseKnockback=\\unhex(\\half2(\\value(4))), WeightKnockback=\\unhex(\\half1(\\value(3))), KnockbackGrowth=\\unhex(\\half2(\\value(3))), Size=\\value(5), Z Offset=\\value(6), Y Offset=\\value(7), X Offset=\\value(8), TripRate=\\value(9)%, HitlagMultiplier=x\\value(10), SDIMultiplier=x\\value(11), Flags=\\hex8(\\unhex(\\value(12)))",
                new long[] {0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 0}));
            EventDictionary.Add(0x06040000, new ActionEventInfo(0x06040000, "Terminate Collisions",
                "Remove all currently present collision bubbles",
                new string[] { },
                new string[] { },
                "",
                new long[] { }));
            EventDictionary.Add(0x06050100, new ActionEventInfo(0x06050100, "Body Collision",
                "Change how the character's own collision bubbles act.",
                new string[] {"State"},
                new string[]
                {
                    "0 = normal, 1 = invincible, 2 = intangible, 3 = intangible (no flashing), 4 = intangible (quick flashing)"
                },
                "\\name(): status=\\collision(\\value(0))",
                new long[] {0}));
            EventDictionary.Add(0x06080200, new ActionEventInfo(0x06080200, "Bone Collision",
                "Sets specific bones to a type of body collision.",
                new string[] {"Bone", "State"},
                new string[]
                {
                    "The bone to be affected.",
                    "The type of body collision. 0 = normal, 1 = invincible, 2 = intangible, 3 = intangible (no flashing), 4 = intangible (quick flashing)"
                },
                "\\name(): bone=\\bone(\\value(0)), status=\\collision(\\value(1))",
                new long[] { }));
            EventDictionary.Add(0x06060100, new ActionEventInfo(0x06060100, "Undo Bone Collision",
                "Sets bones to their normal collision type.",
                new string[] { },
                new string[] { },
                "",
                new long[] { }));
            EventDictionary.Add(0x060A0800, new ActionEventInfo(0x060A0800, "Catch Collision",
                "Generate a grabbing collision bubble with the specified parameters",
                new string[] {"ID", "Bone", "Scale", "X offset", "Y Offset", "Z Offset", "Action", "Air/Ground"},
                new string[]
                {
                    "ID of catch collision.", "The bone the grab is attached to.",
                    "The size of the catch collision bubble.", "Transition relative to the currently attached bone.",
                    "Transition relative to the currently attached bone.",
                    "Transition relative to the currently attached bone.",
                    "The Action ID that the foe executes if successfully grabbed.",
                    "0 = grabs nothing, 1 = grabs grounded only, 2 = grabs aerial only, 3 = grabs aerial and grounded."
                },
                "\\name(): ID=\\value(0), Bone=\\bone(\\value(1)), Scale=\\value(2), Offset=(\\value(3), \\value(4), \\value(5)), Action=\\unhex(\\value(6)), Type=\\value(7)",
                new long[] {0, 0, 1, 1, 1, 1, 0, 0}));
            EventDictionary.Add(0x060D0000, new ActionEventInfo(0x060D0000, "Terminate Catch Collisions",
                "Remove all currently present grab collision bubbles",
                new string[] { },
                new string[] { },
                "",
                new long[] { }));
            EventDictionary.Add(0x060E1100, new ActionEventInfo(0x060E1100, "Throw Specifier",
                "Specify the properties of the throw to be used when 060F0500 is executed. Used for other things as well, such as some Final Smashes.",
                new string[]
                {
                    "ID", "Bone?", "Damage", "Trajectory", "Knockback Growth", "Weight Knockback", "Base Knockback",
                    "Effect", "Undefined", "Undefined", "Undefined", "Undefined", "SFX", "Air/Ground", "Undefined",
                    "Undefined", "Invincibility Frames?"
                },
                new string[]
                {
                    "ID of throw data. Seemingly, a \"0\" indicates this is the throw data, while a \"1\" indicates this is used if the opponent escapes during the throw. \"2\" has also been seen (by Light Arrow).",
                    "Possibly bone used by collision.", "The amount of damage inflicted to the target on throw.",
                    "The direction in which the target gets launched.",
                    "The additional distance the target is launched proportional to its damage.",
                    "The distance the target is launched proportional to weight. Set to non-zero values only for fixed knockback throws.",
                    "The distance the target is launched regardless of its damage. Set to zero for fixed knockback throws.",
                    "The effect of the throw. See the [[Hitbox Flags (Brawl)#Bits 28-32 (Effect)", "Undefined.",
                    "Undefined.", "Undefined.", "Undefined.", "Sound effect played upon throw.",
                    "0 = Never Grabs, 1 = Only Grabs Grounded Foes, 2 = Only Grabs Aerial Foes, 3= Grabs Aerial and Grounded Foes.",
                    "Undefined.", "Undefined.",
                    "The number of invincibility frames the thrower gains when this command is executed?"
                },
                "\\name():ID=\\value(0), Bone?=\\value(1), Damage=\\unhex(\\value(2)), Direction=\\unhex(\\value(3)), KnockbackGrowth=\\unhex(\\value(4)), WeightKnockback=\\unhex(\\value(5)),BaseKnockback=\\unhex(\\value(6)), Element=\\value(7), UnknownA=\\value(8), UnknownB=\\value(9), UnknownC=\\value(10), UnknownD=\\value(11), SFX=\\value(12), Direction?=\\value(13), UnknownE=\\value(14), UnknownF=\\value(15), UnknownG=\\value(16)",
                new long[] {0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 0, 0, 0, 3, 3, 0}));
            EventDictionary.Add(0x060F0500, new ActionEventInfo(0x060F0500, "Throw Applier",
                "Throws an opponent based on data provided by 060E1100 (Throw Specifier).",
                new string[] {"ID?", "Bone", "Undefined", "Undefined", "Undefined"},
                new string[]
                {
                    "Undefined.", "Appears to always be the bone the thrown character is attached to.", "Undefined.",
                    "Undefined.", "Undefined"
                },
                "",
                new long[] {0, 0, 5, 5, 5}));
            EventDictionary.Add(0x06150F00, new ActionEventInfo(0x06150F00, "Special Offensive Collision",
                "Generate an offensive collision bubble - is able to achieve unique effects.",
                new string[]
                {
                    "Bone/Id", "Damage", "Trajectory", "Weight Knockback/Knockback Growth",
                    "Shield Damage/Base Knockback", "Size", "X Offset", "Y Offset", "Z Offset", "Tripping Rate",
                    "Hitlag Multiplier", "Directional Influence Multiplier", "Flags", "Rehit Rate", "Special Flags"
                },
                new string[]
                {
                    "Value1 = The bone the collision bubble is attached to. Value2 = The id number of the collision bubble.",
                    "The amount of damage inflicted to the target upon collision. ",
                    "The direction in which a target gets launched.",
                    "Value1 = The distance the target is launched proportional to weight for fixed knockback hits. Value2 = The additional distance the target is launched proportional to its damage (launch force for fixed knockback hits).",
                    "Value1 = The amount of damage dealt to the target's shield if it is up. Value2 = The distance the target is launched regardless of its damage (zero for fixed knockback hits).",
                    "The size of the collision bubble.", "Transition relative to the currently attached bone.",
                    "Transition relative to the currently attached bone.",
                    "Transition relative to the currently attached bone.",
                    "The percent possibility of the collision bubble inducing a trip, provided the target doesn't leave the ground from the knockback.",
                    "A multiplier affecting the time in which both parties pause when the collision bubble connects.",
                    "A multiplier affecting the ability for the character maneuver themselves while suffering from the hitlag generated by this collision bubble.",
                    "Flags for various parameters such as hit effects and sound effects.",
                    "How many frames between each hitbox refresh; for example a value of 8 will cause the hitbox to hit every 9 frames. A value of 0 disables refreshing; the hitbox will only hit once.",
                    "Flags for hitbox type and attributes such as susceptibility to reflection and absorption."
                },
                "\\name(): Id=\\unhex(\\half2(\\value(0))), Bone=\\bone(\\half1(\\value(0))), Damage=\\unhex(\\value(1)), ShieldDamage=\\unhex(\\half1(\\value(4))), Direction=\\unhex(\\value(2)), BaseKnockback=\\unhex(\\half2(\\value(4))), WeightKnockback=\\unhex(\\half1(\\value(3))), KnockbackGrowth=\\unhex(\\half2(\\value(3))), Size=\\value(5), Z Offset=\\value(6), Y Offset=\\value(7), X Offset=\\value(8), TripRate=\\value(9)%, HitlagMultiplier=x\\value(10), SDIMultiplier=x\\value(11), Flags=\\hex8(\\unhex(\\value(12)))",
                new long[] {0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0}));
            EventDictionary.Add(0x0C0D0F00, new ActionEventInfo(0x0C0D0F00,
                "Character Specific Special Offensive Collision",
                "Generate an offensive collision bubble - is able to achieve unique effects.",
                new string[]
                {
                    "Undefined", "Damage", "Trajectory", "Weight Knockback/Knockback Growth",
                    "Shield Damage/Base Knockback", "Size", "X Offset", "Y Offset", "Z Offset", "Tripping Rate",
                    "Hitlag Multiplier", "Directional Influence Multiplier", "Flags", "Rehit Rate", "Special Flags"
                },
                new string[]
                {
                    "Unknown.", "The amount of damage inflicted to the target upon collision. ",
                    "The direction in which a target gets launched.",
                    "Value1 = The distance the target is launched proportional to weight for fixed knockback hits. Value2 = The additional distance the target is launched proportional to its damage (launch force for fixed knockback hits).",
                    "Value1 = The amount of damage dealt to the target's shield if it is up. Value2 = The distance the target is launched regardless of its damage (zero for fixed knockback hits).",
                    "The size of the collision bubble.", "Transition relative to the currently attached bone.",
                    "Transition relative to the currently attached bone.",
                    "Transition relative to the currently attached bone.",
                    "The percent possibility of the collision bubble inducing a trip, provided the target doesn't leave the ground from the knockback.",
                    "A multiplier affecting the time in which both parties pause when the collision bubble connects.",
                    "A multiplier affecting the ability for the character maneuver themselves while suffering from the hitlag generated by this collision bubble.",
                    "Flags for various parameters such as hit effects and sound effects.",
                    "How many frames between each hitbox refresh; for example a value of 8 will cause the hitbox to hit every 9 frames. A value of 0 disables refreshing; the hitbox will only hit once.",
                    "Flags for hitbox type and attributes such as susceptibility to reflection and absorption."
                },
                "\\name(): Id=\\unhex(\\half2(\\value(0))), Bone=\\bone(\\half1(\\value(0))), Damage=\\unhex(\\value(1)), ShieldDamage=\\unhex(\\half1(\\value(4))), Direction=\\unhex(\\value(2)), BaseKnockback=\\unhex(\\half2(\\value(4))), WeightKnockback=\\unhex(\\half1(\\value(3))), KnockbackGrowth=\\unhex(\\half2(\\value(3))), Size=\\value(5), Z Offset=\\value(6), Y Offset=\\value(7), X Offset=\\value(8), TripRate=\\value(9)%, HitlagMultiplier=x\\value(10), SDIMultiplier=x\\value(11), Flags=\\hex8(\\unhex(\\value(12)))",
                new long[] {0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0}));
            EventDictionary.Add(0x06170300, new ActionEventInfo(0x06170300, "Defensive Collision",
                "Generate a defensive collision bubble.",
                new string[] {"Undefined", "Undefined", "Undefined"},
                new string[] {"Undefined.", "Undefined.", "Undefined"},
                "",
                new long[] {0, 0, 0}));
            EventDictionary.Add(0x06180300, new ActionEventInfo(0x06180300, "Terminate Defensive Collision",
                "Removes defensive collisions.",
                new string[] {"Undefined", "Undefined", "Undefined"},
                new string[] {"Undefined.", "Undefined.", "Undefined"},
                "",
                new long[] {0, 0, 0}));
            EventDictionary.Add(0x061B0500, new ActionEventInfo(0x061B0500, "Move Hitbox",
                "Repositions an already-existing hitbox.",
                new string[] {"Hitbox ID", "New Bone", "New X Offset", "New Y Offset", "New Z Offset"},
                new string[]
                {
                    "The ID of the hitbox to modify.", "The ID of the bone to attach to.", "The new X translation.",
                    "The new Y translation.", "The new Z translation"
                },
                "",
                new long[] {0, 0, 1, 1, 1}));
            EventDictionary.Add(0x062B0D00, new ActionEventInfo(0x062B0D00, "Thrown Collision",
                "Generate a damage collision bubble surrounding the character being thrown.",
                new string[]
                {
                    "Bone/Id", "Damage", "Trajectory", "Weight Knockback/Knockback Growth",
                    "Shield Damage/Base Knockback", "Size", "X Offset", "Y Offset", "Z Offset", "Tripping Rate",
                    "Hitlag Multiplier", "Directional Influence Multiplier", "Flags"
                },
                new string[]
                {
                    "The bone the collision bubble is attached to/The id number of the collision bubble. Where XXXXYYYY is X=Bone, Y=Id.",
                    "The amount of damage inflicted to the target upon collision.",
                    "The direction in which a target gets launched.",
                    "The distance the target is launched proportional to weight for fixed knockback hits/The additional distance the target is launched proportional to its damage (launch force for fixed knockback hits). XXXXYYYY is X=Weight Knockback, Y=Knockback Growth.",
                    "The amount of damage dealt to the target's shield if it is up/The distance the target is launched regardless of its damage (zero for fixed knockback hits). XXXXYYYY is X=Shield Damage, Y=Base Knockback.",
                    "The size of the collision bubble.",
                    "The amount the collision bubble is transitioned relative to the currently attached bone.",
                    "The amount the collision bubble is transitioned relative to the currently attached bone.",
                    "The amount the collision bubble is transitioned relative to the currently attached bone.",
                    "The percent possibility of the collision bubble inducing a trip, proving the target doesn't leave the ground from the knockback.",
                    "A multiplier affecting the time in which both parties pause when the collision bubble connects.",
                    "A multiplier affecting the ability for the character maneuver themselves while suffering from the hitlag generated by this collision bubble.",
                    "Flags for various parameters such as hit effects and sound effects."
                },
                "",
                new long[] {0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 0}));
            EventDictionary.Add(0x0A000100, new ActionEventInfo(0x0A000100, "Sound Effect",
                "Play a specified sound effect.",
                new string[] {"Sound Effect"},
                new string[] {"The ID number for the sound effect called."},
                "\\name(): \\value(0)",
                new long[] {0}));
            EventDictionary.Add(0x0A010100, new ActionEventInfo(0x0A010100, "Sound Effect 2",
                "Play a specified sound effect.",
                new string[] {"Sound Effect"},
                new string[] {"The ID number for the sound effect called."},
                "\\name(): \\value(0)",
                new long[] {0}));
            EventDictionary.Add(0x0A020100, new ActionEventInfo(0x0A020100, "Sound Effect (Transient)",
                "Play a specified sound effect. The sound effect ends with the animation.",
                new string[] {"Sound Effect"},
                new string[] {"The ID number for the sound effect called."},
                "\\name(): \\value(0)",
                new long[] {0}));
            EventDictionary.Add(0x0A050100, new ActionEventInfo(0x0A050100, "Sounds 05",
                "Is used during victory poses.",
                new string[] {"Undefined"},
                new string[] {"Undefined"},
                "",
                new long[] {0}));
            EventDictionary.Add(0x0A070100, new ActionEventInfo(0x0A070100, "Sounds 07",
                "Undefined.",
                new string[] {"Undefined"},
                new string[] {"Undefined"},
                "",
                new long[] {0}));
            EventDictionary.Add(0x0A090100, new ActionEventInfo(0x0A090100, "Other Sound Effect 1",
                "Play a specified sound effect.",
                new string[] {"Sound Effect"},
                new string[] {"The ID number of the sound effect to be called."},
                "\\name(): \\value(0)",
                new long[] {0}));
            EventDictionary.Add(0x0A0A0100, new ActionEventInfo(0x0A0A0100, "Other Sound Effect 2",
                "Play a specified sound effect.",
                new string[] {"Sound Effect"},
                new string[] {"The ID number of the sound effect to be called."},
                "\\name(): \\value(0)",
                new long[] {0}));
            EventDictionary.Add(0x0A030100, new ActionEventInfo(0x0A030100, "Stop Sound Effect",
                "Stops the specified sound effect immediately.",
                new string[] {"Sound Effect"},
                new string[] {"The ID number of the sound effect to be called."},
                "",
                new long[] {0}));
            EventDictionary.Add(0x0C050000, new ActionEventInfo(0x0C050000, "Terminate Instance",
                "Causes the acting instance to terminate (if possible). Will load secondary instance if available.",
                new string[] { },
                new string[] { },
                "",
                new long[] { }));
            EventDictionary.Add(0x0C0B0000, new ActionEventInfo(0x0C0B0000, "Low Voice Clip",
                "Play a random voice clip from the selection of low voice clips.",
                new string[] { },
                new string[] { },
                "",
                new long[] { }));
            EventDictionary.Add(0x0C190000, new ActionEventInfo(0x0C190000, "Damage Voice Clip",
                "Play a random voice clip from the selection of damage voice clips.",
                new string[] { },
                new string[] { },
                "",
                new long[] { }));
            EventDictionary.Add(0x0C1D0000, new ActionEventInfo(0x0C1D0000, "Ottotto Voice Clip",
                "Play the voice clip for Ottotto.",
                new string[] { },
                new string[] { },
                "",
                new long[] { }));
            EventDictionary.Add(0x04070100, new ActionEventInfo(0x04070100, "Frame Speed Modifier",
                "Dictates the frame speed of the subaction. Example: setting to 2 makes the animation and timers occur twice as fast.",
                new string[] {"Multiplier"},
                new string[] {"How many times faster the frames are."},
                "\\name(): Multiplier=\\value(0)x",
                new long[] {1}));
            EventDictionary.Add(0x0C230200, new ActionEventInfo(0x0C230200, "Time Manipulation",
                "Change the speed of time for various parts of the environment.",
                new string[] {"Multiplier", "Frames"},
                new string[] {"How many times faster the frames are.", "How long the time is multiplied."},
                "\\name(): Amount=\\value(0), Frames=\\value(0)",
                new long[] {0, 0}));
            EventDictionary.Add(0x0E000100, new ActionEventInfo(0x0E000100, "Set Air/Ground",
                "Specify whether the character is on or off the ground.",
                new string[] {"State"},
                new string[] {"The state of the character's air/ground status. 0 = In Air, 1 = On Ground"},
                "\\name(): \\airground(\\value(0))",
                new long[] {0}));
            EventDictionary.Add(0x08000100, new ActionEventInfo(0x08000100, "Set Edge Slide",
                "Determines whether or not the character will slide off the edge.",
                new string[] {"Character State"},
                new string[]
                {
                    "1: Can drop off side of stage.  2: Can't drop off side of stage.  5: Treated as in air; can leave stage vertically.  Other states currently unknown."
                },
                "\\name(): \\enum(\\value(0), 0)",
                new long[] {0})
            {
                Enums = new Dictionary<int, List<string>>()
                {
                    {
                        0,
                        new List<string>()
                        {
                            "Undefined(0)", "Can drop off side of stage", "Can't drop off side of stage",
                            "Undefined(3)",
                            "Undefined(4)", "In Air; Can leave stage vertically",
                        }
                    }
                }
            });
            EventDictionary.Add(0x10000100, new ActionEventInfo(0x10000100, "Generate Article",
                "Generate a pre-made prop effect from the prop library.",
                new string[] {"Article ID"},
                new string[] {"The id of the prop article to be called."},
                "\\name(): \\value(0)",
                new long[] {0}));
            EventDictionary.Add(0x10030100, new ActionEventInfo(0x10030100, "Remove Article",
                "Removes an article.",
                new string[] {"Article"},
                new string[] {"ID of the article to be affected."},
                "\\name(): \\value(0)",
                new long[] {0}));
            EventDictionary.Add(0x10050200, new ActionEventInfo(0x10050200, "Article Visibility",
                "Makes an article visible or invisible.",
                new string[] {"Article", "Visibility"},
                new string[] {"ID of the article to be affected.", "Set Boolean: True = Visible, False = Invisible"},
                "\\name(): Article ID=\\value(0), Visible=\\value(1)",
                new long[] {0, 3}));
            EventDictionary.Add(0x100A0000, new ActionEventInfo(0x100A0000, "Generate Prop Effect",
                "Generate a prop effect with the specified parameters.",
                new string[] { },
                new string[] { },
                "",
                new long[] { }));
            EventDictionary.Add(0x11001000, new ActionEventInfo(0x11001000, "External Graphic Effect",
                "Generate a generic graphical effect with the specified parameters.",
                new string[]
                {
                    "Graphic", "Bone", "Z Offset", "Y Offset", "X Offset", "Z Rotation", "Y Rotation", "X Rotation",
                    "Scale", "Random Z Offset", "Random Y Offset", "Random Z Offset", "Random Z Rotation",
                    "Random Y Rotation", "Random X Rotation", "Terminate With Animation"
                },
                new string[]
                {
                    "The file from which to call from/The graphical effect to call. Value1 = File#, Value2 = Graphic ID",
                    "The bone to attach the graphical effect to.",
                    "Transition from the attached bone along the Z axis.",
                    "Transition from the attached bone along the Y axis.",
                    "Transition from the attached bone along the X axis.", "Rotation along the Z axis.",
                    "Rotation along the Y axis.", "Rotation along the X axis.", "The size of the graphic.",
                    "A random value lesser than the specified value and added to the Z Offset.",
                    "A random value lesser than the specified value and added to the Y Offset.",
                    "A random value lesser than the specified value and added to the X Offset.",
                    "A random value lesser than the specified value and added to the Z Rotation.",
                    "A random value lesser than the specified value and added to the Y Rotation.",
                    "A random value lesser than the specified value and added to the X Rotation.",
                    "Sets whether or not this graphic effect terminates when the animation ends."
                },
                "\\name(): File=\\unhex(\\half1(\\value(0))), Graphic ID=\\unhex(\\half2(\\value(0))), Bone=\\bone(\\value(1)), Translation=(\\value(4), \\value(3), \\value(2)), Rotation=(\\value(7), \\value(6), \\value(5)), Scale=\\value(8), Random Translation=(\\value(11), \\value(10), \\value(9)), Random Rotation=(\\value(14), \\value(13), \\value(12)), Anchored=\\value(15)",
                new long[] {0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 3}));
            EventDictionary.Add(0x11010A00, new ActionEventInfo(0x11010A00, "External Graphic Effect",
                "Generate a graphical effect from an external file. (usually the Ef_ file)",
                new string[]
                {
                    "Graphic", "Bone", "Z Offset", "Y Offset", "X Offset", "Z Rotation", "Y Rotation", "X Rotation",
                    "Scale", "Terminate With Animation"
                },
                new string[]
                {
                    "The file from which to call from/The graphical effect to call. Value1 = File#, Value2 = Graphic ID",
                    "The bone to attach the graphical effect to.",
                    "Transition from the attached bone along the Z axis.",
                    "Transition from the attached bone along the Y axis.",
                    "Transition from the attached bone along the X axis.", "Rotation along the Z axis.",
                    "Rotation along the Y axis.", "Rotation along the X axis.", "The size of the graphic.",
                    "Sets whether or not this graphic effect terminates when the animation ends."
                },
                "\\name(): File=\\unhex(\\half1(\\value(0))), Graphic ID=\\unhex(\\half2(\\value(0))), Bone=\\bone(\\value(1)), Translation=(\\value(4), \\value(3), \\value(2)), Rotation=(\\value(7), \\value(6), \\value(5)), Scale=\\value(8), Anchored=\\value(9)",
                new long[] {0, 0, 1, 1, 1, 1, 1, 1, 1, 3}));
            EventDictionary.Add(0x11020A00, new ActionEventInfo(0x11020A00, "External Graphic Effect",
                "Generate a graphical effect from an external file. (usually the Ef_ file)",
                new string[]
                {
                    "Graphic", "Bone", "Z Offset", "Y Offset", "X Offset", "Z Rotation", "Y Rotation", "X Rotation",
                    "Scale", "Terminate With Animation"
                },
                new string[]
                {
                    "The file from which to call from/The graphical effect to call. Value1 = File#, Value2 = Graphic ID",
                    "The bone to attach the graphical effect to.",
                    "Transition from the attached bone along the Z axis.",
                    "Transition from the attached bone along the Y axis.",
                    "Transition from the attached bone along the X axis.", "Rotation along the Z axis.",
                    "Rotation along the Y axis.", "Rotation along the X axis.", "The size of the graphic.",
                    "Sets whether or not this graphic effect terminates when the animation ends."
                },
                "\\name(): File=\\unhex(\\half1(\\value(0))), Graphic ID=\\unhex(\\half2(\\value(0))), Bone=\\bone(\\value(1)), Translation=(\\value(4), \\value(3), \\value(2)), Rotation=(\\value(7), \\value(6), \\value(5)), Scale=\\value(8), Anchored=\\value(9)",
                new long[] {0, 0, 1, 1, 1, 1, 1, 1, 1, 3}));
            EventDictionary.Add(0x11170700, new ActionEventInfo(0x11170700, "Limited Screen Tint",
                "Tint the screen to the specified color.",
                new string[]
                    {"Transition In Time", "Red", "Green", "Blue", "Alpha", "Frame Count", "Transition Out Time"},
                new string[]
                {
                    "The time taken to transition from the current color to the specified color.", "The red value.",
                    "The green value.", "The blue value.", "The transparency.",
                    "The amount of frames that the color lasts.", "The amount of time it takes the color to fade out."
                },
                "\\name(): TransInTime=\\value(0), RGBA=(\\value(1), \\value(2), \\value(3), \\value(4)), FrameCount=\\value(5), TransOutTime=\\value(6)",
                new long[] {0, 0, 0, 0, 0, 0, 0}));
            EventDictionary.Add(0x111A1000, new ActionEventInfo(0x111A1000, "Graphic Effect",
                "Generate a generic graphical effect with the specified parameters.",
                new string[]
                {
                    "Graphic", "Bone", "Z Offset", "Y Offset", "X Offset", "Z Rotation", "Y Rotation", "X Rotation",
                    "Scale", "Random Z Offset", "Random Y Offset", "Random Z Offset", "Random Z Rotation",
                    "Random Y Rotation", "Random X Rotation", "Terminate With Animation"
                },
                new string[]
                {
                    "The graphical effect to call.", "The bone to attach the graphical effect to.",
                    "Transition from the attached bone along the Z axis.",
                    "Transition from the attached bone along the Y axis.",
                    "Transition from the attached bone along the X axis.", "Rotation along the Z axis.",
                    "Rotation along the Y axis.", "Rotation along the X axis.", "The size of the graphic.",
                    "A random value lesser than the specified value and added to the Z Offset.",
                    "A random value lesser than the specified value and added to the Y Offset.",
                    "A random value lesser than the specified value and added to the X Offset.",
                    "A random value lesser than the specified value and added to the Z Rotation.",
                    "A random value lesser than the specified value and added to the Y Rotation.",
                    "A random value lesser than the specified value and added to the X Rotation.",
                    "Sets whether or not this graphic  effect terminates when the animation ends."
                },
                "\\name(): Graphic=\\value(0), Bone=\\bone(\\value(1)), Translation=(\\value(4), \\value(3), \\value(2)), Rotation=(\\value(7), \\value(6), \\value(5)), Scale=\\value(8), Random Translation=(\\value(11), \\value(10), \\value(9)), Random Rotation=(\\value(14), \\value(13), \\value(12)), Anchored=\\value(15)",
                new long[] {0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 3}));
            EventDictionary.Add(0x111B1000, new ActionEventInfo(0x111B1000, "Graphic Effect",
                "Generate a generic graphical effect with the specified parameters.",
                new string[]
                {
                    "Graphic", "Bone", "Z Offset", "Y Offset", "X Offset", "Z Rotation", "Y Rotation", "X Rotation",
                    "Scale", "Random Z Offset", "Random Y Offset", "Random Z Offset", "Random Z Rotation",
                    "Random Y Rotation", "Random X Rotation", "Terminate With Animation"
                },
                new string[]
                {
                    "The graphical effect to call.", "The bone to attach the graphical effect to.",
                    "Transition from the attached bone along the Z axis.",
                    "Transition from the attached bone along the Y axis.",
                    "Transition from the attached bone along the X axis.", "Rotation along the Z axis.",
                    "Rotation along the Y axis.", "Rotation along the X axis.", "The size of the graphic.",
                    "A random value lesser than the specified value and added to the Z Offset.",
                    "A random value lesser than the specified value and added to the Y Offset.",
                    "A random value lesser than the specified value and added to the X Offset.",
                    "A random value lesser than the specified value and added to the Z Rotation.",
                    "A random value lesser than the specified value and added to the Y Rotation.",
                    "A random value lesser than the specified value and added to the X Rotation.",
                    "Sets whether or not this graphic  effect terminates when the animation ends."
                },
                "\\name(): Graphic=\\value(0), Bone=\\bone(\\value(1)), Translation=(\\value(4), \\value(3), \\value(2)), Rotation=(\\value(7), \\value(6), \\value(5)), Scale=\\value(8), Random Translation=(\\value(11), \\value(10), \\value(9)), Random Rotation=(\\value(14), \\value(13), \\value(12)), Anchored=\\value(15)",
                new long[] {0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 3}));
            EventDictionary.Add(0x11031400, new ActionEventInfo(0x11031400, "Sword Glow",
                "Creates glow of sword.  Only usable when the proper effects are loaded by their respective characters.",
                new string[]
                {
                    "Color", "Blur Length", "Trail Bone #1", "X Offset", "Y Offset", "Z Offset", "Trail Bone #2",
                    "X Offset", "Y Offset", "Z Offset", "Glow State", "Graphic ID", "Glow Bone", "X Offset", "Y Offset",
                    "Z Offset", "X Rotation", "Y Rotation", "Z Rotation", "Glow Length"
                },
                new string[]
                {
                    "Controls the hue of the glow.", "The length of the glow's aftershadow.",
                    "Bone the 1st point of the sword trail follows.", "X offset of the 1st point of the sword trail.",
                    "Y offset of the 1st point of the sword trail.", "Z offset of the 1st point of the sword trail.",
                    "Bone the 2nd point of the  sword trail follows.", "X offset of the 2nd point of the sword trail.",
                    "Y offset of the 2nd point of the sword trail.", "Z offset of the 2nd point of the sword trail.",
                    "If set to true, glow/trail disappears at the end  of the subaction.",
                    "The ID of  the External Graphic Effect referenced.", "Bone the sword glow follows.",
                    "X offset of the sword glow.", "Y offset  of the sword glow.", "Z offset  of the sword glow.",
                    "Rotation of the sword glow around the X axis.", "Rotation  of the sword glow around the Y axis.",
                    "Rotation  of the sword glow around the Z axis.",
                    "Length of the sword glow, i.e. halving this  value will make it half the  sword's length."
                },
                "\\name(): Hue=\\value(0), Length=\\value(1), Bone #1=\\bone(\\value(2)), Translation=(\\value(3), \\value(4), \\value(5)), Bone #2=\\bone(\\value(6)), Translation=(\\value(7), \\value(8), \\value(9)), State=\\value(10), Graphic=\\value(11), Bone #3=\\bone(\\value(12)), Translation=(\\value(13), \\value(14), \\value(15)), Rotation=(\\value(16), \\value(17), \\value(18)), Length=\\value(19)",
                new long[] {0, 0, 0, 1, 1, 1, 0, 1, 1, 1, 3, 0, 0, 1, 1, 1, 1, 1, 1, 1}));
            EventDictionary.Add(0x11050100, new ActionEventInfo(0x11050100, "Terminate Sword Glow",
                "Eliminates sword glow graphics when set to 1. May have unknown applications.",
                new string[] {"Fade Time"},
                new string[] {"The time it takes for the sword glow to fade out."},
                "\\name(): Fade Time=\\value(0)",
                new long[] {0}));
            EventDictionary.Add(0x14070A00, new ActionEventInfo(0x14070A00, "Aesthetic Wind Effect",
                "Moves nearby movable model parts (capes, hair, etc) with a wind specified by the parameters.",
                new string[]
                    {"Undefined", "Undefined", "Strength", "Speed", "Size?", "Undefined", "Undefined", "Undefined"},
                new string[]
                {
                    "Undefined.", "Undefined.", "The power of the wind.", "The speed of the wind.",
                    "Perhaps the size of the wind.", "Undefined.", "Undefined.", "Undefined"
                },
                "",
                new long[] {0, 1, 1, 1, 1, 1, 1, 0}));
            EventDictionary.Add(0x12000200, new ActionEventInfo(0x12000200, "Basic Variable Set",
                "Set a basic variable to the specified value.",
                new string[] {"Value", "Variable"},
                new string[]
                    {"The value to place inside the specified variable.", "The Basic type variable to access."},
                "\\name(): \\value(1) = \\unhex(\\value(0))",
                new long[] {0, 5}));
            EventDictionary.Add(0x12010200, new ActionEventInfo(0x12010200, "Basic Variable Add",
                "Add a specified value to a basic variable.",
                new string[] {"Value", "Variable"},
                new string[] {"The value add to the specified variable.", "The Basic type variable to access."},
                "\\name(): \\value(1) += \\unhex(\\value(0))",
                new long[] {0, 5}));
            EventDictionary.Add(0x12020200, new ActionEventInfo(0x12020200, "Basic Variable Subtract",
                "Subtract a specified value from a basic variable.",
                new string[] {"Value", "Variable"},
                new string[]
                    {"The value to subtract from the specified variable.", "The Basic type variable to access."},
                "\\name(): \\value(1) -= \\unhex(\\value(0))",
                new long[] {0, 5}));
            EventDictionary.Add(0x12060200, new ActionEventInfo(0x12060200, "Float Variable Set",
                "Set a floating point variable to the specified value.",
                new string[] {"Value", "Variable"},
                new string[]
                {
                    "The floating point value to place inside the specified variable.",
                    "The Float type variable to access."
                },
                "\\name(): \\value(1) = \\unhex(\\value(0))",
                new long[] {1, 5}));
            EventDictionary.Add(0x12070200, new ActionEventInfo(0x12070200, "Float Variable Add",
                "Add a specified value to a float variable.",
                new string[] {"Value", "Variable"},
                new string[]
                {
                    "The floating point value to add to the specified variable.", "The Float type variable to access."
                },
                "\\name(): \\value(1) += \\unhex(\\value(0))",
                new long[] {1, 5}));
            EventDictionary.Add(0x12080200, new ActionEventInfo(0x12080200, "Float Variable Subtract",
                "Subtract a specified value from a float variable.",
                new string[] {"Value", "Variable"},
                new string[]
                {
                    "The floating point value to subtract from the specified variable.",
                    "The Float type variable to access."
                },
                "\\name(): \\value(1) -= \\unhex(\\value(0))",
                new long[] {1, 5}));
            EventDictionary.Add(0x120A0100, new ActionEventInfo(0x120A0100, "Bit Variable Set",
                "Set a bit variable to true.",
                new string[] {"Variable"},
                new string[] {"The Bit type variable to set."},
                "\\name(): \\value(0) = true",
                new long[] {5}));
            EventDictionary.Add(0x120B0100, new ActionEventInfo(0x120B0100, "Bit Variable Clear",
                "Set a bit variable to false.",
                new string[] {"Variable"},
                new string[] {"The Bit type variable to clear."},
                "\\name(): \\value(0) = false",
                new long[] {5}));
            EventDictionary.Add(0x1A040500, new ActionEventInfo(0x1A040500, "Camera Closeup",
                "Zoom the camera on the character.",
                new string[] {"Zoom Time", "Undefined", "Distance", "X Angle", "Y Angle"},
                new string[]
                {
                    "The time it takes to zoom in on the target.", "Undefined.",
                    "How far away the camera is from the character.", "The horizontal rotation around the character.",
                    "The vertical rotation around the character."
                },
                "\\name(): Zoom Time=\\value(0), Undefined=\\value(1), Distance=\\value(2), X Rotation=\\value(3), Y Rotation=\\value(4)",
                new long[] {0, 0, 1, 1, 1}));
            EventDictionary.Add(0x1A080000, new ActionEventInfo(0x1A080000, "Normal Camera",
                "Return the camera to its normal settings.",
                new string[] { },
                new string[] { },
                "",
                new long[] { }));
            EventDictionary.Add(0x1F000100, new ActionEventInfo(0x1F000100, "Pickup Item",
                "Cause the character to recieve the closest item in range.",
                new string[] {"Undefined"},
                new string[] {"Undefined"},
                "",
                new long[] {0}));
            EventDictionary.Add(0x1F000200, new ActionEventInfo(0x1F000200, "Pickup Item",
                "Cause the character to recieve the closest item in range.",
                new string[] {"Undefined", "Undefined"},
                new string[] {"Undefined.", "Undefined"},
                "",
                new long[] {0, 0}));
            EventDictionary.Add(0x1F010300, new ActionEventInfo(0x1F010300, "Throw Item",
                "Cause the character to throw the currently held item.",
                new string[] {"Undefined", "Undefined", "Undefined"},
                new string[] {"Undefined.", "Undefined.", "Undefined"},
                "",
                new long[] {5, 5, 5}));
            EventDictionary.Add(0x1F020000, new ActionEventInfo(0x1F020000, "Drop Item",
                "Cause the character to drop any currently held item.",
                new string[] { },
                new string[] { },
                "",
                new long[] { }));
            EventDictionary.Add(0x1F030100, new ActionEventInfo(0x1F030100, "Consume Item",
                "Cause the character to consume the currently held item.",
                new string[] {"Undefined"},
                new string[] {"Undefined"},
                "",
                new long[] {0}));
            EventDictionary.Add(0x1F040200, new ActionEventInfo(0x1F040200, "Item Property",
                "Modify a property of the currently held item.",
                new string[] {"Undefined", "Undefined"},
                new string[] {"Undefined.", "Undefined"},
                "",
                new long[] {0, 1}));
            EventDictionary.Add(0x1F070100, new ActionEventInfo(0x1F070100, "Items 1F",
                "Is used when firing a cracker launcher.",
                new string[] {"Undefined"},
                new string[] {"Undefined"},
                "",
                new long[] {5}));
            EventDictionary.Add(0x1F080100, new ActionEventInfo(0x1F080100, "Generate Item",
                "Generate an item in the character's hand.",
                new string[] {"Undefined"},
                new string[] {"Undefined"},
                "",
                new long[] {0}));
            EventDictionary.Add(0x1F0C0100, new ActionEventInfo(0x1F0C0100, "Beam Sword Trail",
                "Creates a beam sword trail. Probably has more uses among battering weapons.",
                new string[] {"Undefined"},
                new string[] {"Undefined"},
                "",
                new long[] {0}));
            EventDictionary.Add(0x1F0E0500, new ActionEventInfo(0x1F0E0500, "Throw Item",
                "Causes the character to throw the currently held item.",
                new string[] {"Undefined", "Undefined", "Undefined", "Undefined", "Undefined"},
                new string[] {"Undefined.", "Undefined.", "Undefined.", "Undefined.", "Undefined"},
                "",
                new long[] {1, 1, 5, 5, 5}));
            EventDictionary.Add(0x1F090100, new ActionEventInfo(0x1F090100, "Item Visibility",
                "Determines visibilty of the currently held item.",
                new string[] {"Item Visibility"},
                new string[] {"Set Boolean: True = Visible, False = Invisible"},
                "",
                new long[] {3}));
            EventDictionary.Add(0x1F050000, new ActionEventInfo(0x1F050000, "Fire Weapon",
                "Fires a shot from the currently held item.  (May have other unknown applications)",
                new string[] { },
                new string[] { },
                "",
                new long[] { }));
            EventDictionary.Add(0x1F060100, new ActionEventInfo(0x1F060100, "Fire Projectile",
                "Fires a projectile of the specified degree of power.",
                new string[] { },
                new string[] { },
                "",
                new long[] { }));
            EventDictionary.Add(0x21000000, new ActionEventInfo(0x21000000, "Terminate Flash Effect",
                "Terminate all currently active flash effects.",
                new string[] { },
                new string[] { },
                "",
                new long[] { }));
            EventDictionary.Add(0x21010400, new ActionEventInfo(0x21010400, "Flash Overlay Effect",
                "Generate a flash overlay effect over the character with the specified colors and opacity. Replaces any currently active flash effects.",
                new string[] {"R", "G", "B", "A"},
                new string[]
                {
                    "The red value from 0-255.", "The green value from 0-255.", "The blue value from 0-255.",
                    "The alpha value from 0-255 (0 = fully transparent, 255 = fully opaque)."
                },
                "\\name(): RGBA=(\\value(0), \\value(1), \\value(2), \\value(3))",
                new long[] {0, 0, 0, 0}));
            EventDictionary.Add(0x21020500, new ActionEventInfo(0x21020500, "Change Flash Overlay Color",
                "Changes the color of the current flash overlay effect.",
                new string[] {"Transition Time", "R", "G", "B", "A"},
                new string[]
                {
                    "The number of frames the colour change takes.", "The red value (0-255) of the target colour.",
                    "The green value (0-255) of the target colour.", "The blue value (0-255) of the target colour.",
                    "The alpha value (0-255) of the target colour."
                },
                "\\name(): Transition Time=\\value(0), RGBA=(\\value(1), \\value(2), \\value(3), \\value(4))",
                new long[] {0, 0, 0, 0, 0}));
            EventDictionary.Add(0x21050600, new ActionEventInfo(0x21050600, "Flash Light Effect",
                "Generate a flash lighting effect over the character with the specified colors, opacity and angle.  Replaces any currently active flash effects.",
                new string[] {"R", "G", "B", "A", "Angle", "Unknown"},
                new string[]
                {
                    "The red value from 0-255.", "The green value from 0-255.", "The blue value from 0-255.",
                    "The alpha value from 0-255 (0 = fully transparent, 255 = fully opaque).",
                    "The angle in degrees of the virtual light source.",
                    "Possibly the distance of the virtual light source?"
                },
                "\\name(): RGBA=(\\value(0), \\value(1), \\value(2), \\value(3)), Light Source X=\\value(4), Light Source Y=\\value(5)",
                new long[] {0, 0, 0, 0, 1, 1}));
            EventDictionary.Add(0x21070500, new ActionEventInfo(0x21070500, "Change Flash Light Color",
                "Changes the color of the current flash light effect.",
                new string[] {"Transition Time", "R", "G", "B", "A"},
                new string[]
                {
                    "The number of frames the color change takes.", "The red value (0-255) of the target color.",
                    "The green value (0-255) of the target color.", "The blue value (0-255) of the target color.",
                    "The alpha value (0-255) of the target color."
                },
                "\\name(): Transition Time=\\value(0), RGBA=(\\value(1), \\value(2), \\value(3), \\value(4))",
                new long[] {0, 0, 0, 0, 0}));
            EventDictionary.Add(0x64000000, new ActionEventInfo(0x64000000, "Allow Interrupt",
                "Allow the current action to be interrupted by another action.",
                new string[] { },
                new string[] { },
                "",
                new long[] { }));
            EventDictionary.Add(0x020A0100, new ActionEventInfo(0x020A0100, "Allow Specific Interrupt",
                "Allows interruption only by specific commands.  See parameters for list of possible interrupts.",
                new string[] {"Interrupt ID"},
                new string[]
                {
                    "List of types of commands: 1-Ground Special, 2-Ground Item, 3-Ground Catch, 4-Ground Attack, 5-Ground Escape, 6-Ground Guard, 7-Ground Jump, 8-Ground (other), 9-Air Landing, A-Grab Edge, B-Air Special, C-Air Item Throw, D-Air Lasso, E-Air Dodge, F-Air Attack, 10-Air Tread Jump, 11-Air Walljump, 12-Air Jump Aerial, 13-Fall Through Plat(only works in squat)."
                },
                "\\name(): \\enum(\\value(0), 0)",
                new long[] {0})
            {
                Enums = new Dictionary<int, List<string>>()
                {
                    {
                        0,
                        new List<string>()
                        {
                            "Main", "Ground Special", "Ground Item", "Ground Catch", "Ground Attack", "Ground Escape",
                            "Ground Guard", "Ground Jump", "Ground (Other)", "Air Landing", "Grab Edge", "Air Special",
                            "Air Item Throw", "Air Lasso", "Air Dodge", "Air Attack", "Air Tread Jump", "Air Walljump",
                            "Air Jump Aerial", "Fall Through Platform (Squat Only)"
                        }
                    }
                }
            });
            EventDictionary.Add(0x1A000100, new ActionEventInfo(0x1A000100, "Screenshake",
                "Shakes the screen.",
                new string[] {"Magnitude"},
                new string[] {"The intensity of the screenshake."},
                "\\name(): Magnitude=\\value(0)",
                new long[] {1}));
            EventDictionary.Add(0x0B020100, new ActionEventInfo(0x0B020100, "Visibility",
                "Changes whether the model is visible or not.",
                new string[] {"Visibility"},
                new string[] {"Set Boolean: True = Visible, False = Invisible"},
                "Visible: \\value(0)",
                new long[] {3}));
            EventDictionary.Add(0x07070200, new ActionEventInfo(0x07070200, "Rumble",
                "Undefined. Affects the rumble feature of the controller.",
                new string[] {"Undefined", "Undefined"},
                new string[] {"Undefined.", "Undefined"},
                "",
                new long[] {0, 0}));
            EventDictionary.Add(0x0E080400, new ActionEventInfo(0x0E080400, "Set/Add Momentum",
                "Controls the movement velocity of the object.",
                new string[] {"Horizontal Velocity", "Vertical Velocity", "Set/Add Horizontal", "Set/Add Vertical"},
                new string[]
                {
                    "The speed of the object moving left/right.", "The speed of the object moving up/down.",
                    "0 = Add, 1 = Set", "0 = Add, 1 = Set"
                },
                "\\name(): Horizontal=\\value(0), Vertical=\\value(1), Add/Set Horiz=\\value(2), Add/Set Vert=\\value(3)",
                new long[] {1, 1, 0, 0}));
            EventDictionary.Add(0x0E010200, new ActionEventInfo(0x0E010200, "Add/Subtract Character Momentum",
                "Adds or subtracts speed to the character's current momentum.",
                new string[] {"Horizontal Velocity", "Vertical Velocity"},
                new string[]
                    {"The speed of the character moving left/right.", "The speed of the character moving up/down."},
                "\\name(): Horizontal Speed=\\value(0), Vertical Speed=\\value(1)",
                new long[] {1, 1}));
            EventDictionary.Add(0x0E060100, new ActionEventInfo(0x0E060100, "Disallow Certain Movements",
                "Does not allow the specified type of movement.",
                new string[] {"Type"},
                new string[]
                {
                    "When set to 1, vertical movement is disallowed. When set to 2, horizontal movement is disallowed."
                },
                "\\name(): \\enum(\\value(0), 0)",
                new long[] {0})
            {
                Enums = new Dictionary<int, List<string>>()
                    {{0, new List<string>() {"Undefined(0)", "Vertical", "Horizontal"}}}
            });
            EventDictionary.Add(0x0E070100, new ActionEventInfo(0x0E070100, "Disallow Certain Movements 2",
                "This must be set to the same value as Disallow Certain Movements to work.",
                new string[] { },
                new string[] { },
                "",
                new long[] {0}));
            EventDictionary.Add(0x0E020100, new ActionEventInfo(0x0E020100, "Disallow Vertical Movement",
                "When set to 1, vertical speed and acceleration are reset back to 0.",
                new string[] { },
                new string[] { },
                "\\name(): \\unhex(\\value(0))",
                new long[] { }));
            EventDictionary.Add(0x0C250100, new ActionEventInfo(0x0C250100, "Tag Display",
                "Disables or enables tag display for the current subaction.",
                new string[] {"Tag On/Off"},
                new string[] {"True = On, False = Off"},
                "\\name(): \\value(0)",
                new long[] {3}));
            EventDictionary.Add(0x1E000200, new ActionEventInfo(0x1E000200, "Super/Heavy Armor",
                "Begins super armor or heavy armor.  Set both parameters to 0 to end the armor.",
                new string[] {"Armor State", "Heavy Armor Tolerance"},
                new string[]
                {
                    "0 = None, 1 = Super Armor, 2 = Knockback Based Heavy Armor, 3 = Damage Based Heavy Armor",
                    "The minimum damage or KB that will cause the character to flinch when using heavy armor."
                },
                "\\name(): State=\\enum(\\value(0), 0), Tolerance=\\value(1)",
                new long[] {0, 1})
            {
                Enums = new Dictionary<int, List<string>>()
                {
                    {
                        0,
                        new List<string>()
                            {"None", "Super Armor", "Knockback Based Heavy Armor", "Damage Based Heavy Armor"}
                    }
                }
            });
            EventDictionary.Add(0x1E030100, new ActionEventInfo(0x1E030100, "Add/Subtract Damage",
                "Adds or subtracts the specified amount of damage from the character's current percentage.",
                new string[] {"Damage"},
                new string[] {"The amount of damage to add or subtract."},
                "\\name(): \\value(0)",
                new long[] {1}));
            EventDictionary.Add(0x06010200, new ActionEventInfo(0x06010200, "Change Hitbox Damage",
                "Changes a specific hitbox's damage to the new amount. Only guaranteed to work on Offensive Collisions.",
                new string[] {"Hitbox", "Damage"},
                new string[] {"ID of the hitbox to be changed.", "New damage of the hitbox."},
                "\\name(): ID=\\value(0), Damage=\\value(1)",
                new long[] {0, 0}));
            EventDictionary.Add(0x06030100, new ActionEventInfo(0x06030100, "Delete Hitbox",
                "Deletes a hitbox of the specified ID.  Only guaranteed to work on Offensive Collisions.",
                new string[] {"Hitbox"},
                new string[] {"ID of the hitbox to be deleted."},
                "\\name(): \\value(0)",
                new long[] {0}));
            EventDictionary.Add(0x0B000200, new ActionEventInfo(0x0B000200, "Model Changer 1",
                "Changes the visibility of certain bones attached to objects. Uses bone groups and switches set in Reference 1 of the Model Visibility section.",
                new string[] {"Switch Index", "Bone Group Index"},
                new string[]
                {
                    "The index of the switch group in Reference 1 to modify.",
                    "The index of the group of bones in the switch. A value less than 0 or greater than the amount of groups will disable the visibility of all objects. All other groups will be turned off when switching this one on."
                },
                "\\name(): Switch=\\value(0), Group=\\value(1)",
                new long[] {0, 0}));
            EventDictionary.Add(0x0B010200, new ActionEventInfo(0x0B010200, "Model Changer 2",
                "Changes the visibility of certain bones attached to objects. Uses bone groups and switches set in Reference 2 of the Model Visibility section.",
                new string[] {"Switch Index", "Bone Group Index"},
                new string[]
                {
                    "The index of the switch group in Reference 2 to modify.",
                    "The index of the group of bones in the switch. A value less than 0 or greater than the amount of groups will disable the visibility of all objects. All other groups will be turned off when switching this one on."
                },
                "\\name(): Switch=\\value(0), Group=\\value(1)",
                new long[] { }));
            EventDictionary.Add(0x10040100, new ActionEventInfo(0x10040100, "Model Event 1",
                "This affects an article/model action. (This only works with characters who have articles in one of their files.",
                new string[] {"Model ID"},
                new string[]
                {
                    "Model. (Only Summons the Id of the article/model to call. Only summons a FitChar##.pac. For example, Pit's bow is article/model 0)"
                },
                "\\name(): Model ID=\\value(0)",
                new long[] { }));
            EventDictionary.Add(0x10040200, new ActionEventInfo(0x10040200, "Set Anchored Article SubAction",
                "Sets the specified article to execute the specified action immediately. Only works on anchored articles (Cape, FLUDD, not fireball, water).",
                new string[] {"Article ID", "Action"},
                new string[]
                {
                    "The ID of the article you would like to edit.", "The action you would like the article to execute."
                },
                "\\name(): article=\\value(0), action=\\unhex(\\value(1))",
                new long[] {0, 0}));
            EventDictionary.Add(0x10040300, new ActionEventInfo(0x10040300, "Set Anchored Article SubAction",
                "Sets the specified article to execute the specified action immediately. Only works on anchored articles (Cape, FLUDD, not fireball, water).",
                new string[] {"Article ID", "Action", "Subaction Exclusive?"},
                new string[]
                {
                    "The ID of the article you would like to edit.",
                    "The action you would like the article to execute.",
                    "Whether or not you want the article to be automatically deleted when the subaction ends."
                },
                "\\name(): article=\\value(0), action=\\unhex(\\value(1)): Delete At End=\\value(2)",
                new long[] {0, 0, 3}));
            EventDictionary.Add(0x14040100, new ActionEventInfo(0x14040100, "Terminate Wind Effect",
                "Ends the wind effect spawned by the \"Aesthetic Wind Effect\" event.",
                new string[] {"Undefined"},
                new string[] {"Usually set to 0."},
                "",
                new long[] {0}));
            EventDictionary.Add(0x070B0200, new ActionEventInfo(0x070B0200, "Rumble Loop",
                "Creates a rumble loop on the controller.",
                new string[] {"Undefined", "Undefined"},
                new string[] {"Undefined.", "Undefined"},
                "",
                new long[] {0, 0}));
            EventDictionary.Add(0x18000100, new ActionEventInfo(0x18000100, "Slope Contour Stand",
                "Moves specific parts of the character if on sloped ground?",
                new string[] {"Parameter 0"},
                new string[] {"Unknown."},
                "",
                new long[] { }));
            EventDictionary.Add(0x18010200, new ActionEventInfo(0x18010200, "Slope Contour Full?",
                "Moves entire character to match sloped ground?",
                new string[] {"Parameter 0", "Parameter 1"},
                new string[] {"Unknown.", "Unknown."},
                "",
                new long[] { }));
            EventDictionary.Add(0x10000200, new ActionEventInfo(0x10000200, "Generate Article",
                "Generate a pre-made prop effect from the prop library.",
                new string[] {"Article ID", "Subaction Exclusive?"},
                new string[]
                {
                    "The id of the prop article to be called.",
                    "Whether or not you want the article to be automatically deleted when the subaction ends."
                },
                "",
                new long[] {0, 3}));
            EventDictionary.Add(0x10010100, new ActionEventInfo(0x10010100, "Article Event 02",
                "Makes the article preform an animation when set to 1.",
                new string[] {"Article ID"},
                new string[] {"ID of the article."},
                "",
                new long[] { }));
            EventDictionary.Add(0x00030000, new ActionEventInfo(0x00030000, "Flow 03",
                "Undefined",
                new string[] { },
                new string[] { },
                "",
                new long[] { }));
            EventDictionary.Add(0x000B0100, new ActionEventInfo(0x000B0100, "And",
                "Seems to be an \"and\" to an If statement.",
                new string[] { },
                new string[] { },
                "And \\value(0):",
                new long[] { }));
            EventDictionary.Add(0x000B0200, new ActionEventInfo(0x000B0200, "And Value",
                "Seems to be an \"and\" to an If statement.",
                new string[] {"Requirement", "Variable"},
                new string[]
                {
                    "The form of requirement used in evaluation of the event.",
                    "The variable applied to the requirement."
                },
                "And \\value(0): \\value(1)",
                new long[] {6, 0}));
            EventDictionary.Add(0x000B0300, new ActionEventInfo(0x000B0300, "And Unknown",
                "Seems to be an \"and\" to an If statement.",
                new string[] {"Requirement", "Variable", "Unknown"},
                new string[]
                {
                    "The form of requirement used in evaluation of the event.",
                    "The variable applied to the requirement.", "Unknown"
                },
                "And \\value(0): \\value(1); Unknown=\\value(2)",
                new long[] {6, 0, 0}));
            EventDictionary.Add(0x000C0100, new ActionEventInfo(0x000C0100, "Or",
                "Insert an alternate requirement to fall back on if the above requirement(s) are not met.",
                new string[] {"Requirement"},
                new string[] {"The form of requirement used in evaluation of the event."},
                "Or \\value(0):",
                new long[] {6}));
            EventDictionary.Add(0x000C0200, new ActionEventInfo(0x000C0200, "Or Value",
                "Insert an alternate requirement to fall back on if the above requirement(s) are not met.",
                new string[] {"Requirement", "Variable"},
                new string[]
                {
                    "The form of requirement used in evaluation of the event.",
                    "The variable applied to the requirement."
                },
                "Or \\value(0): \\value(1)",
                new long[] {6, 0}));
            EventDictionary.Add(0x000C0300, new ActionEventInfo(0x000C0300, "Or Unknown",
                "Insert an alternate requirement to fall back on if the above requirement(s) are not met.",
                new string[] {"Requirement", "Variable", "Unknown"},
                new string[]
                {
                    "The form of requirement used in evaluation of the event.",
                    "The variable applied to the requirement.", "Undefined"
                },
                "Or \\value(0): \\value(1); Unknown=\\value(2)",
                new long[] {6, 0, 0}));
            EventDictionary.Add(0x000C0400, new ActionEventInfo(0x000C0400, "Or Comparison",
                "Insert an alternate requirement to fall back on if the above requirement(s) are not met.",
                new string[] {"Requirement", "Variable", "Comparison Method", "Variable"},
                new string[]
                {
                    "The form of requirement used in evaluation of the event.",
                    "The first variable in the comparison requirement.",
                    "The method used to compare the two variables. From 0 to 5: <, ≤, =, ≠, ≥, > ",
                    "The second variable in the comparison requirement."
                },
                "Or \\value(0): \\value(1) \\cmpsign(\\value(2)) \\value(3)",
                new long[] {6, 5, 0, 5}));
            EventDictionary.Add(0x000D0200, new ActionEventInfo(0x000D0200, "Else If Value",
                "Insert an Else If block inside of an If block.",
                new string[] {"Requirement", "Variable"},
                new string[]
                {
                    "The form of requirement used in evaluation of the event.",
                    "The variable applied to the requirement."
                },
                "Else If \\value(0): \\value(1)",
                new long[] {6, 0}));
            EventDictionary.Add(0x000D0300, new ActionEventInfo(0x000D0300, "Else If Unknown",
                "Insert an Else If block inside of an If block.",
                new string[] {"Requirement", "Variable", "Unknown"},
                new string[]
                {
                    "The form of requirement used in evaluation of the event.",
                    "The variable applied to the requirement.", "Undefined"
                },
                "Else If \\value(0): \\value(1); Unknown=\\value(2)",
                new long[] {6, 0, 0}));
            EventDictionary.Add(0x00180000, new ActionEventInfo(0x00180000, "Break",
                "Appears within Case statements; exits the switch event completely. All code located in the same case block after this event will not be executed.",
                new string[] { },
                new string[] { },
                "",
                new long[] { }));
            EventDictionary.Add(0x02000400, new ActionEventInfo(0x02000400, "Change Action Status Value",
                "Change the current action upon the specified requirement being met (the requirement does not have to be met at the time this ID is executed - it can be used anytime after execution.)",
                new string[] {"Status ID", "Action", "Requirement", "Variable"},
                new string[]
                {
                    "The four-digit status ID of the change action event. Can later be disabled via 02080100 (Disable Action Status ID)",
                    "The ID of the action that the character will execute.",
                    "The form of requirement used in evaluation of the event.",
                    "The variable applied to the requirement."
                },
                "Prioritized Change Action: priority=\\unhex(\\value(0)), action=\\unhex(\\value(1)), requirement=\\value(2): \\value(3)",
                new long[] {0, 0, 6, 0}));
            EventDictionary.Add(0x02000500, new ActionEventInfo(0x02000500, "Change Action Status Unknown",
                "Change the current action upon the specified requirement being met (the requirement does not have to be met at the time this ID is executed - it can be used anytime after execution.)",
                new string[] {"Status ID", "Action", "Requirement", "Variable", "Unknown"},
                new string[]
                {
                    "The four-digit status ID of the change action event. Can later be disabled via 02080100 (Disable Action Status ID)",
                    "The ID of the action that the character will execute.",
                    "The form of requirement used in evaluation of the event.",
                    "The variable applied to the requirement.",
                    "Used as a specification for the requirement when necessary (for example, if the requirement is \"button pressed\", this denotes which button)."
                },
                "Prioritized Change Action: priority=\\unhex(\\value(0)), action=\\unhex(\\value(1)), requirement=\\value(2): \\value(3); Unknown=\\value(4)",
                new long[] {0, 0, 6, 0, 0}));
            EventDictionary.Add(0x02000600, new ActionEventInfo(0x02000600, "Change Action Status Comparison",
                "Change the current action upon the specified requirement being met (the requirement does not have to be met at the time this ID is executed - it can be used anytime after execution.)",
                new string[] {"Status ID", "Action", "Requirement", "Variable", "Comparison", "Variable"},
                new string[]
                {
                    "The four-digit status ID of the change action event. Can later be disabled via 02080100 (Disable Action Status ID)",
                    "The ID of the action that the character will execute.",
                    "The form of requirement used in evaluation of the event.",
                    "The variable applied to the requirement.", "The first variable in the comparison requirement.",
                    "The method used to compare the two variables. From 0 to 5: <, ≤, =, ≠, ≥, >",
                    "The second variable in the comparison requirement."
                },
                "Prioritized Change Action: priority=\\unhex(\\value(0)), action=\\unhex(\\value(1)), requirement=\\value(2): \\value(3) \\cmpsign(\\value(4)) \\value(5)",
                new long[] {0, 0, 6, 0, 0}));
            EventDictionary.Add(0x02080100, new ActionEventInfo(0x02080100, "Disable Action Status ID",
                "Disables the Action associated with the given Status ID.",
                new string[] {"Status ID"},
                new string[] {"The Status ID to disable. After this command, the associated Action will not activate."},
                "\\name(): \\unhex(\\value(0))",
                new long[] {0}));
            EventDictionary.Add(0x02090200, new ActionEventInfo(0x02090200, "Invert Action Status ID",
                "Appears to invert (or possibly only disable) a specific Status ID's enabled/disabled status. For example, if a character can crawl, this is used to disable the ability to dash when crouched, even though naturally crouching allows dashing through 020A (Allow Specific Interrupt).",
                new string[] {"Interrupt ID?", "Status ID?"},
                new string[]
                {
                    "Appears to be a Interrupt ID as used by 020A (Allow Specific Interrupt)",
                    "Appears to be a Status ID."
                },
                "\\name(): Interrupt ID=\\enum(\\value(0), 0), Status ID=\\unhex(\\value(1))",
                new long[] {0, 0})
            {
                Enums = new Dictionary<int, List<string>>()
                {
                    {
                        0,
                        new List<string>()
                        {
                            "Main", "Ground Special", "Ground Item", "Ground Catch", "Ground Attack", "Ground Escape",
                            "Ground Guard", "Ground Jump", "Ground (Other)", "Air Landing", "Grab Edge", "Air Special",
                            "Air Item Throw", "Air Lasso", "Air Dodge", "Air Attack", "Air Tread Jump", "Air Walljump",
                            "Air Jump Aerial", "Fall Through Platform (Squat Only)"
                        }
                    }
                }
            });
            EventDictionary.Add(0x020B0100, new ActionEventInfo(0x020B0100, "Disallow Specific Interrupt",
                "Closes the specific interruption window. Must be set to the same thing as the allow specific interrupt that you wish to cancel.",
                new string[] {"Interrupt ID"},
                new string[]
                {
                    "List of types of commands: 1-Ground Special, 2-Ground Item, 3-Ground Catch, 4-Ground Attack, 5-Ground Escape, 6-Ground Guard, 7-Ground Jump, 8-Ground (other), 9-Air Landing, A-Grab Edge, B-Air Special, C-Air Item Throw, D-Air Lasso, E-Air Dodge, F-Air Attack, 10-Air Tread Jump, 11-Air Walljump, 12-Air Jump Aerial, 13-Fall Through Plat(only works in squat)."
                },
                "\\name(): \\enum(\\value(0), 0)",
                new long[] {0})
            {
                Enums = new Dictionary<int, List<string>>()
                {
                    {
                        0,
                        new List<string>()
                        {
                            "Main", "Ground Special", "Ground Item", "Ground Catch", "Ground Attack", "Ground Escape",
                            "Ground Guard", "Ground Jump", "Ground (Other)", "Air Landing", "Grab Edge", "Air Special",
                            "Air Item Throw", "Air Lasso", "Air Dodge", "Air Attack", "Air Tread Jump", "Air Walljump",
                            "Air Jump Aerial", "Fall Through Platform (Squat Only)"
                        }
                    }
                }
            });
            EventDictionary.Add(0x020C0100, new ActionEventInfo(0x020C0100, "Unregister Interrupt?",
                "Possibly unregisters a previously created interrupt.",
                new string[] {"Interrupt ID"},
                new string[] {"Possibly the Interrupt ID to unregister."},
                "\\name(): \\enum(\\value(0), 0)",
                new long[] {0})
            {
                Enums = new Dictionary<int, List<string>>()
                {
                    {
                        0,
                        new List<string>()
                        {
                            "Main", "Ground Special", "Ground Item", "Ground Catch", "Ground Attack", "Ground Escape",
                            "Ground Guard", "Ground Jump", "Ground (Other)", "Air Landing", "Grab Edge", "Air Special",
                            "Air Item Throw", "Air Lasso", "Air Dodge", "Air Attack", "Air Tread Jump", "Air Walljump",
                            "Air Jump Aerial", "Fall Through Platform (Squat Only)"
                        }
                    }
                }
            });
            EventDictionary.Add(0x04020100, new ActionEventInfo(0x04020100, "Additional Subaction Change Requirement",
                "",
                new string[] {"Requirement"},
                new string[] {"The form of requirement used in evaluation of the event."},
                "\\name(): \\value(0)",
                new long[] {6}));
            EventDictionary.Add(0x04020200, new ActionEventInfo(0x04020200,
                "Additional Subaction Change Requirement Value",
                "",
                new string[] {"Requirement", "Variable"},
                new string[]
                {
                    "The form of requirement used in evaluation of the event.",
                    "The variable applied to the requirement."
                },
                "\\name(): \\value(0): \\value(1)",
                new long[] {6, 5}));
            EventDictionary.Add(0x04020300, new ActionEventInfo(0x04020300,
                "Additional Subaction Change Requirement Unknown",
                "",
                new string[] {"Requirement", "Variable", "Undefined"},
                new string[]
                {
                    "The form of requirement used in evaluation of the event.",
                    "The variable applied to the requirement.", "Undefined"
                },
                "\\name(): \\value(0): \\value(1), \\value(2)",
                new long[] {6, 5, 0}));
            EventDictionary.Add(0x04020400, new ActionEventInfo(0x04020400,
                "Additional Subaction Change Requirement Compare",
                "",
                new string[] {"Requirement", "Variable", "Comparison Method", "Variable"},
                new string[]
                {
                    "The form of requirement used in evaluation of the event.",
                    "The first variable in the comparison requirement.",
                    "The method used to compare the two variables. From 0 to 5: <, ≤, =, ≠, ≥, >",
                    "The second variable in the comparison requirement."
                },
                "\\name(): \\value(0): \\value(1) \\cmpsign(\\value(2)) \\value(3)",
                new long[] {6, 5, 0, 5}));
            EventDictionary.Add(0x04030100, new ActionEventInfo(0x04030100, "Extra Subaction Change Requirement",
                "Seems to act as an additional requirement for Additional Subaction Change Requirement.",
                new string[] {"Requirement"},
                new string[] {"The form of requirement used in evaluation of the event."},
                "\\name(): \\value(0)",
                new long[] {6}));
            EventDictionary.Add(0x04030200, new ActionEventInfo(0x04030200, "Extra Subaction Change Requirement Value",
                "",
                new string[] {"Requirement", "Variable"},
                new string[]
                {
                    "The form of requirement used in evaluation of the event.",
                    "The variable applied to the requirement."
                },
                "\\name(): \\value(0): \\value(1)",
                new long[] {6, 5}));
            EventDictionary.Add(0x04030300, new ActionEventInfo(0x04030300,
                "Extra Subaction Change Requirement Unknown",
                "",
                new string[] {"Requirement", "Variable", "Undefined"},
                new string[]
                {
                    "The form of requirement used in evaluation of the event.",
                    "The variable applied to the requirement.", "Undefined"
                },
                "\\name(): \\value(0): \\value(1), \\value(2)",
                new long[] {6, 5, 0}));
            EventDictionary.Add(0x04030400, new ActionEventInfo(0x04030400,
                "Extra Subaction Change Requirement Compare",
                "",
                new string[] {"Requirement", "Variable", "Comparison Method", "Variable"},
                new string[]
                {
                    "The form of requirement used in evaluation of the event.",
                    "The first variable in the comparison requirement.",
                    "The method used to compare the two variables. From 0 to 5: <, ≤, =, ≠, ≥, > ",
                    "The second variable in the comparison requirement."
                },
                "\\name(): \\value(0): \\value(1) \\cmpsign(\\value(2)) \\value(3)",
                new long[] {6, 5, 0, 5}));
            EventDictionary.Add(0x04060100, new ActionEventInfo(0x04060100, "Set Animation Frame",
                "Changes the current frame of the animation. Does not change the frame of the subaction (i.e. timers and such are unaffected).",
                new string[] {"Frame"},
                new string[] {"The frame to skip to."},
                "\\name(): \\value(0)",
                new long[] {1}));
            EventDictionary.Add(0x040A0100, new ActionEventInfo(0x040A0100, "Subactions 0A",
                "",
                new string[] {"Undefined"},
                new string[] {"Undefined"},
                "",
                new long[] {0}));
            EventDictionary.Add(0x040B0100, new ActionEventInfo(0x040B0100, "Subactions 0B",
                "",
                new string[] {"Undefined"},
                new string[] {"Undefined"},
                "",
                new long[] {1}));
            EventDictionary.Add(0x040C0100, new ActionEventInfo(0x040C0100, "Subactions 0C",
                "",
                new string[] {"Subaction"},
                new string[] {"A subaction ID."},
                "",
                new long[] {0}));
            EventDictionary.Add(0x040D0100, new ActionEventInfo(0x040D0100, "Subactions 0D",
                "",
                new string[] {"Undefined"},
                new string[] {"Undefined"},
                "",
                new long[] {0}));
            EventDictionary.Add(0x04140100, new ActionEventInfo(0x04140100, "Subactions 14",
                "",
                new string[] {"Undefined"},
                new string[] {"Undefined"},
                "",
                new long[] {1}));
            EventDictionary.Add(0x04180100, new ActionEventInfo(0x04180100, "Subactions 18",
                "",
                new string[] {"Undefined"},
                new string[] {"Undefined"},
                "",
                new long[] {5}));
            EventDictionary.Add(0x05010000, new ActionEventInfo(0x05010000, "Posture 01",
                "Undefined",
                new string[] { },
                new string[] { },
                "",
                new long[] { }));
            EventDictionary.Add(0x05020000, new ActionEventInfo(0x05020000, "Posture 02",
                "Undefined",
                new string[] { },
                new string[] { },
                "",
                new long[] { }));
            EventDictionary.Add(0x05030000, new ActionEventInfo(0x05030000, "Posture 03",
                "Undefined",
                new string[] { },
                new string[] { },
                "",
                new long[] { }));
            EventDictionary.Add(0x05040000, new ActionEventInfo(0x05040000, "Posture 04",
                "Undefined",
                new string[] { },
                new string[] { },
                "",
                new long[] { }));
            EventDictionary.Add(0x05070300, new ActionEventInfo(0x05070300, "Posture 07",
                "",
                new string[] {"Undefined", "Undefined", "Undefined"},
                new string[] {"Undefined.", "Undefined.", "Undefined"},
                "",
                new long[] {1, 5, 1}));
            EventDictionary.Add(0x050D0100, new ActionEventInfo(0x050D0100, "Posture 0D",
                "",
                new string[] {"Undefined"},
                new string[] {"Undefined"},
                "",
                new long[] {0}));
            EventDictionary.Add(0x06020200, new ActionEventInfo(0x06020200, "Change Hitbox Size",
                "Changes a specific hitbox's size to the new amount. Only guaranteed to work on Offensive Collisions.",
                new string[] {"Hitbox", "Size"},
                new string[] {"ID of the hitbox to be changed.", "New size of the hitbox."},
                "\\name(): ID=\\value(1), Size=\\value(1)",
                new long[] {0, 0}));
            EventDictionary.Add(0x060C0100, new ActionEventInfo(0x060C0100, "Delete Catch Collision",
                "Deletes the catch collision with the specified ID.",
                new string[] {"ID"},
                new string[] {"ID of the collision to delete"},
                "\\name(): \\value(0)",
                new long[] {0}));
            EventDictionary.Add(0x06101100, new ActionEventInfo(0x06101100, "Inert Collision",
                "Generates an oblivious hitbox only used to detect collision with other characters/objects.",
                new string[]
                {
                    "Undefined", "Id", "Bone", "Size", "X Offset", "Y Offset", "Z Offset", "Flags", "F", "Air/Ground",
                    "Undefined", "Undefined", "Undefined", "Undefined", "Rehit Rate?", "Affects Self?", "Undefined"
                },
                new string[]
                {
                    "When messed with, seemed to affect the accuracy of the collision detection. Should be set to 0 to be safe.",
                    "The ID of the hitbox", "The bone that the hitbubble is attached to.", "The size of the hitbubble.",
                    "Transition relative to the currently attached bone.",
                    "Transition relative to the currently attached bone.",
                    "Transition relative to the currently attached bone.",
                    "+02 = Hits Normally, +04=Can be reflected....", "Undefined",
                    "1 = hits grounded, 2 = hits aerial, 3 = hits grounded and aerial", "Undefined", "Undefined",
                    "Undefined", "Undefined", "The rehit rate of the hitbubble?",
                    "Possibly if the uninteractive collision affects the host character.", "Undefined"
                },
                "\\name(): Id=\\value(1), Bone=\\bone(\\value(2)), Size=\\value(3), Z Offset=\\value(4), Y Offset=\\value(5), X Offset=\\value(6), Air/Ground=\\value(9),Self-Affliction=\\value(15)",
                new long[] {0, 0, 0, 1, 1, 1, 1, 0, 0, 0, 3, 0, 0, 3, 0, 3, 0}));
            EventDictionary.Add(0x062C0F00, new ActionEventInfo(0x062C0F00, "\"Bump\" Collision?",
                "Possibly the \"bump\" collisions that occur when a character in hitstun collides with another body.",
                new string[]
                {
                    "Bone/ID?", "Damage", "Trajectory", "Weight Knockback/Knockback Growth?",
                    "Shield Damage/Base Knockback?", "Size?", "X Offset?", "Y Offset?", "Z Offset?", "Tripping Rate?",
                    "Hitlag Multiplier?", "Directional Influence Multiplier?", "Flags 1", "Undefined", "Flags 2"
                },
                new string[]
                {
                    "The bone the collision bubble is attached to/The ID number of the collision bubble. Where XXXXYYYY is X=Bone, Y=Id.",
                    "The amount of damage inflicted to the target upon collision.",
                    "The direction in which a target gets launched.",
                    "The distance the target is launched proportional to weight for fixed knockback hits/The additional distance the target is launched proportional to its damage (launch force for fixed knockback hits). XXXXYYYY is X=Weight Knockback, Y=Knockback Growth.",
                    "The amount of damage dealt to the target's shield if it is up/The distance the target is launched regardless of its damage (zero for fixed knockback hits). XXXXYYYY is X=Shield Damage, Y=Base Knockback.",
                    "The size of the collision bubble.",
                    "The amount the collision bubble is transitioned relative to the currently attached bone.",
                    "The amount the collision bubble is transitioned relative to the currently attached bone.",
                    "The amount the collision bubble is transitioned relative to the currently attached bone.",
                    "The percent possibility of the collision bubble inducing a trip, provided the target doesn't leave the ground from the knockback.",
                    "A multiplier affecting the time in which both parties pause when the collision bubble connects.",
                    "A multiplier affecting the ability for the character maneuver themselves while suffering from the hitlag generated by this collision bubble.",
                    "Flags for various parameters such as hit effects and sound effects.", "Undefined.",
                    "Flags for various parameters such as hit effects and sound effects."
                },
                "",
                new long[] {0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0}));
            EventDictionary.Add(0x062D0000, new ActionEventInfo(0x062D0000, "Collisions 2D",
                "Undefined",
                new string[] { },
                new string[] { },
                "",
                new long[] { }));
            EventDictionary.Add(0x07000000, new ActionEventInfo(0x07000000, "Clear Buffer?",
                "Possibly clears the controller buffer.",
                new string[] { },
                new string[] { },
                "",
                new long[] { }));
            EventDictionary.Add(0x07010000, new ActionEventInfo(0x07010000, "Controller 01",
                "Undefined",
                new string[] { },
                new string[] { },
                "",
                new long[] { }));
            EventDictionary.Add(0x07020000, new ActionEventInfo(0x07020000, "Controller 02",
                "Undefined",
                new string[] { },
                new string[] { },
                "",
                new long[] { }));
            EventDictionary.Add(0x07060100, new ActionEventInfo(0x07060100, "Controller 06",
                "",
                new string[] {"Undefined"},
                new string[] {"Undefined"},
                "",
                new long[] {3}));
            EventDictionary.Add(0x070C0000, new ActionEventInfo(0x070C0000, "Controller 0C",
                "Undefined",
                new string[] { },
                new string[] { },
                "",
                new long[] { }));
            EventDictionary.Add(0x08010100, new ActionEventInfo(0x08010100, "Edge Interaction 01",
                "",
                new string[] {"Undefined"},
                new string[] {"Undefined"},
                "",
                new long[] {0}));
            EventDictionary.Add(0x08020100, new ActionEventInfo(0x08020100, "Edge Interaction 02",
                "",
                new string[] {"Character State?"},
                new string[] {"Appears to use similar values to \"Set Edge Slide.\""},
                "",
                new long[] {0}));
            EventDictionary.Add(0x08040100, new ActionEventInfo(0x08040100, "Edge Interaction 04",
                "",
                new string[] {"Undefined"},
                new string[] {"Undefined"},
                "",
                new long[] {3}));
            EventDictionary.Add(0x08070000, new ActionEventInfo(0x08070000, "Edge Interaction 07",
                "Undefined",
                new string[] { },
                new string[] { },
                "",
                new long[] { }));
            EventDictionary.Add(0x09000100, new ActionEventInfo(0x09000100, "Module09 00",
                "",
                new string[] {"Undefined"},
                new string[] {"Undefined"},
                "",
                new long[] {0}));
            EventDictionary.Add(0x0C010000, new ActionEventInfo(0x0C010000, "Character Specific 01",
                "Undefined",
                new string[] { },
                new string[] { },
                "",
                new long[] { }));
            EventDictionary.Add(0x0C040000, new ActionEventInfo(0x0C040000, "Character Specific 04",
                "Undefined",
                new string[] { },
                new string[] { },
                "",
                new long[] { }));
            EventDictionary.Add(0x0C060000, new ActionEventInfo(0x0C060000, "Enter Final Smash State",
                "Allows use of Final Smash locked articles, variables, etc. Highly unstable.",
                new string[] { },
                new string[] { },
                "",
                new long[] { }));
            EventDictionary.Add(0x0C070000, new ActionEventInfo(0x0C070000, "Exit Final Smash State",
                "Undefined",
                new string[] { },
                new string[] { },
                "",
                new long[] { }));
            EventDictionary.Add(0x0C080000, new ActionEventInfo(0x0C080000, "Terminate Self",
                "Used by certain article instances to remove themselves.",
                new string[] { },
                new string[] { },
                "",
                new long[] { }));
            EventDictionary.Add(0x0C090100, new ActionEventInfo(0x0C090100, "Allow/Disallow Ledgegrab",
                "Allow or disallow grabbing ledges during the current subaction.",
                new string[] {"Allow/Disallow"},
                new string[] {"0 = cannot, 1 = Only in front, 2 = Always"},
                "",
                new long[] {0}));
            EventDictionary.Add(0x0C0A0100, new ActionEventInfo(0x0C0A0100, "Character Specific 0A",
                "",
                new string[] {"Undefined"},
                new string[] {"Undefined"},
                "",
                new long[] {0}));
            EventDictionary.Add(0x0C130000, new ActionEventInfo(0x0C130000, "Character Specific 13",
                "Undefined",
                new string[] { },
                new string[] { },
                "",
                new long[] { }));
            EventDictionary.Add(0x0C160000, new ActionEventInfo(0x0C160000, "Character Specific 16",
                "Undefined",
                new string[] { },
                new string[] { },
                "",
                new long[] { }));
            EventDictionary.Add(0x0C170100, new ActionEventInfo(0x0C170100, "Character Specific 17",
                "Undefined. Often appears before 0C25 (Tag Display)",
                new string[] {"Undefined"},
                new string[] {"Undefined."},
                "",
                new long[] {3}));
            EventDictionary.Add(0x0C170200, new ActionEventInfo(0x0C170200, "Character Specific 17 Variable",
                "Undefined. Often appears before 0C25 (Tag Display)",
                new string[] {"Undefined", "Undefined"},
                new string[] {"Undefined.", "Undefined"},
                "",
                new long[] {3, 5}));
            EventDictionary.Add(0x0C1A0200, new ActionEventInfo(0x0C1A0200, "Character Specific 1A",
                "",
                new string[] {"Undefined", "Undefined"},
                new string[] {"Undefined.", "Undefined"},
                "",
                new long[] {0, 0}));
            EventDictionary.Add(0x0C1B0100, new ActionEventInfo(0x0C1B0100, "Character Specific 1B",
                "",
                new string[] {"Undefined"},
                new string[] {"Undefined"},
                "",
                new long[] {5}));
            EventDictionary.Add(0x0C1C0200, new ActionEventInfo(0x0C1C0200, "Character Specific 1C",
                "",
                new string[] {"Undefined", "Undefined"},
                new string[] {"Undefined.", "Undefined"},
                "",
                new long[] {0, 0}));
            EventDictionary.Add(0x0C1C0300, new ActionEventInfo(0x0C1C0300, "Character Specific 1C Boolean",
                "",
                new string[] {"Undefined", "Undefined", "Undefined"},
                new string[] {"Undefined.", "Undefined.", "Undefined"},
                "",
                new long[] {0, 0, 3}));
            EventDictionary.Add(0x0C1F0000, new ActionEventInfo(0x0C1F0000, "Eating Voice Clip",
                "Play a random voice clip from the selection of eating voice clips.",
                new string[] { },
                new string[] { },
                "",
                new long[] { }));
            EventDictionary.Add(0x0C200200, new ActionEventInfo(0x0C200200, "Character Specific 20",
                "",
                new string[] {"Undefined", "Undefined"},
                new string[] {"Undefined.", "Undefined"},
                "",
                new long[] {0, 3}));
            EventDictionary.Add(0x0C240100, new ActionEventInfo(0x0C240100, "Character Specific 24",
                "",
                new string[] {"Undefined"},
                new string[] {"Undefined"},
                "",
                new long[] {3}));
            EventDictionary.Add(0x0C260100, new ActionEventInfo(0x0C260100, "Character Specific 26",
                "",
                new string[] {"Undefined"},
                new string[] {"Undefined"},
                "",
                new long[] {3}));
            EventDictionary.Add(0x0C270000, new ActionEventInfo(0x0C270000, "Character Specific 27",
                "Undefined. Often appears within Switch statements.",
                new string[] { },
                new string[] { },
                "",
                new long[] { }));
            EventDictionary.Add(0x0C290000, new ActionEventInfo(0x0C290000, "Character Specific 29",
                "Undefined",
                new string[] { },
                new string[] { },
                "",
                new long[] { }));
            EventDictionary.Add(0x0C2B0000, new ActionEventInfo(0x0C2B0000, "Character Specific 2B",
                "Undefined",
                new string[] { },
                new string[] { },
                "",
                new long[] { }));
            EventDictionary.Add(0x0D000200, new ActionEventInfo(0x0D000200, "Concurrent Infinite Loop",
                "Runs a subroutine once per frame for the current action.",
                new string[] {"Thread ID?", "Offset"},
                new string[]
                {
                    "Possibly the thread ID of the concurrent routine. Sometimes 4, sometimes 6, sometimes 9.",
                    "The subroutine location that contains the events that you would like to loop infinitely."
                },
                "\\name() (Type \\value(0)) @\\value(1)",
                new long[] {0, 2}));
            EventDictionary.Add(0x0D010100, new ActionEventInfo(0x0D010100, "Terminate Concurrent Infinite Loop?",
                "Seems to stop the execution of a loop created with event 0D000200.",
                new string[] {"Thread ID?"},
                new string[]
                    {"Possibly the thread ID of the concurrent routine. Sometimes 4, sometimes 6, sometimes 9."},
                "",
                new long[] {0}));
            EventDictionary.Add(0x0F030200, new ActionEventInfo(0x0F030200, "Link 03",
                "",
                new string[] {"Undefined", "Undefined"},
                new string[] {"Undefined.", "Undefined"},
                "",
                new long[] {0, 0}));
            EventDictionary.Add(0x11180200, new ActionEventInfo(0x11180200, "End Unlimited Screen Tint",
                "Terminates an unlimited screen tint with the specified ID.",
                new string[] {"ID", "Frames"},
                new string[]
                    {"The ID of the screen tint to end.", "The amount of frames that the color will take to fade out."},
                "\\name(): ID=\\value(0), TransOutTime=\\value(1)",
                new long[] {0, 0}));
            EventDictionary.Add(0x12030100, new ActionEventInfo(0x12030100, "Basic Variable Increment",
                "Variable++",
                new string[] {"Variable"},
                new string[] {"The variable to increment."},
                "\\name(): \\value(0)++",
                new long[] {5}));
            EventDictionary.Add(0x12040100, new ActionEventInfo(0x12040100, "Basic Variable Decrement",
                "Variable--",
                new string[] {"Variable"},
                new string[] {"The variable to decrement."},
                "\\name(): \\value(0)--",
                new long[] {5}));
            EventDictionary.Add(0x120F0200, new ActionEventInfo(0x120F0200, "Float Variable Multiply",
                "Multiply a specified value with a float variable.",
                new string[] {"Value", "Variable"},
                new string[]
                {
                    "The floating point value to multiply with the specified variable.",
                    "The Float type variable to access."
                },
                "\\name(): \\value(1) *= \\unhex(\\value(0))",
                new long[] {1, 5}));
            EventDictionary.Add(0x17010000, new ActionEventInfo(0x17010000, "Physics 01",
                "Undefined",
                new string[] { },
                new string[] { },
                "",
                new long[] { }));
            EventDictionary.Add(0x17050000, new ActionEventInfo(0x17050000, "Physics 05",
                "Undefined",
                new string[] { },
                new string[] { },
                "",
                new long[] { }));
            EventDictionary.Add(0x19010000, new ActionEventInfo(0x19010000, "Module19 01",
                "Undefined",
                new string[] { },
                new string[] { },
                "",
                new long[] { }));
            EventDictionary.Add(0x1A030400, new ActionEventInfo(0x1A030400, "Set Camera Boundaries",
                "Changes the camera boundaries of your character. Does not reset the camera boundaries; rather, it adds to them. Reverts to normal when the animation ends.",
                new string[] {"Front Boundary", "Back Boundary", "Top Boundary", "Bottom Boundary"},
                new string[]
                {
                    "The boundary in front of the character.", "The boundary behind the character.",
                    "The boundary above the character.", "The boundary below the character."
                },
                "\\name(): \\value(0) x \\value(1) x \\value(2); size \\value(3)",
                new long[] {1, 1, 1, 1}));
            EventDictionary.Add(0x1A060100, new ActionEventInfo(0x1A060100, "Detach/Attach Camera (Close)",
                "Causes the camera to follow or stop following a character.",
                new string[] {"Detached/Attached", "Detached/Attached"},
                new string[] {"False = detached, True = attached.", "False = detached, True = attached."},
                "",
                new long[] {3, 3}));
            EventDictionary.Add(0x1A090000, new ActionEventInfo(0x1A090000, "Camera 09",
                "Undefined",
                new string[] { },
                new string[] { },
                "",
                new long[] { }));
            EventDictionary.Add(0x1A0A0100, new ActionEventInfo(0x1A0A0100, "Camera 0A",
                "",
                new string[] {"Undefined"},
                new string[] {"Undefined"},
                "",
                new long[] {0}));
            EventDictionary.Add(0x1A0B0000, new ActionEventInfo(0x1A0B0000, "Force Camera Control",
                "Appears to override any other settings in favor of the character's preference.",
                new string[] { },
                new string[] { },
                "",
                new long[] { }));
            EventDictionary.Add(0x1A0C0000, new ActionEventInfo(0x1A0C0000, "Camera 0C",
                "Undefined",
                new string[] { },
                new string[] { },
                "",
                new long[] { }));
            EventDictionary.Add(0x1F0A0000, new ActionEventInfo(0x1F0A0000, "Obliterate Held Item",
                "Deletes the item that the character is holding",
                new string[] { },
                new string[] { },
                "",
                new long[] { }));
            EventDictionary.Add(0x20000200, new ActionEventInfo(0x20000200, "Turn 00",
                "",
                new string[] {"Undefined", "Undefined"},
                new string[] {"Undefined.", "Undefined"},
                "",
                new long[] {5, 3}));
            EventDictionary.Add(0x64010000, new ActionEventInfo(0x64010000, "Cancel 01",
                "Undefined",
                new string[] { },
                new string[] { },
                "",
                new long[] { }));
            EventDictionary.Add(0x64020000, new ActionEventInfo(0x64020000, "Cancel 02",
                "Undefined",
                new string[] { },
                new string[] { },
                "",
                new long[] { }));
            EventDictionary.Add(0x14050100, new ActionEventInfo(0x14050100, "Aesthetic Wind 05",
                "",
                new string[] {"Undefined"},
                new string[] {"Undefined"},
                "",
                new long[] {3}));
            EventDictionary.Add(0x10070200, new ActionEventInfo(0x10070200, "Set Remote Article SubAction",
                "Does the same thing as Set Article Action but seems to work on articles that aren't attached to the character.",
                new string[] {"Article ID", "SubAction"},
                new string[]
                    {"ID of the article to be affected.", "The subaction you would like the article to execute."},
                "",
                new long[] {0, 0}));
            EventDictionary.Add(0x111D0100, new ActionEventInfo(0x111D0100, "Effect ID",
                "Undefined.",
                new string[] { },
                new string[] { },
                "",
                new long[] { }));
            EventDictionary.Add(0x1F110100, new ActionEventInfo(0x1F110100, "Item 11",
                "Undefined.",
                new string[] { },
                new string[] { },
                "",
                new long[] { }));
            EventDictionary.Add(0x06130000, new ActionEventInfo(0x06130000, "Collisions 13",
                "Undefined.",
                new string[] { },
                new string[] { },
                "",
                new long[] { }));
            EventDictionary.Add(0x18030200, new ActionEventInfo(0x18030200, "Character Specific Samus",
                "Used in samus.",
                new string[] { },
                new string[] { },
                "",
                new long[] { }));
            EventDictionary.Add(0x1F0F0100, new ActionEventInfo(0x1F0F0100, "Morph Model Event",
                "If false model will appear else if true model will disappear.",
                new string[] { },
                new string[] { },
                "",
                new long[] { }));
            EventDictionary.Add(0x01020000, new ActionEventInfo(0x01020000, "Loop Rest 2 for Goto",
                "Often follows 01000000 (Loop Rest 1 for Goto)",
                new string[] { },
                new string[] { },
                "",
                new long[] { }));
            EventDictionary.Add(0x01000000, new ActionEventInfo(0x01000000, "Loop Rest 1 for Goto",
                "Appears to work like 01010000, except is used for loops created by 00090100 (Goto)",
                new string[] { },
                new string[] { },
                "",
                new long[] { }));
            EventDictionary.Add(0x0E0B0200, new ActionEventInfo(0x0E0B0200, "Graphic Model Specf",
                "Appears to control posture graphics.",
                new string[] { },
                new string[] { },
                "",
                new long[] { }));
            EventDictionary.Add(0x11150300, new ActionEventInfo(0x11150300, "Terminate Graphic Effect",
                "Terminate all instances of a lingering graphical effect.",
                new string[] {"Graphic", "Undefined", "Undefined"},
                new string[]
                {
                    "The file from which to call from/The graphical effect to call. Value1 = File#, Value2 = Graphic ID",
                    "Undefined.", "Undefined"
                },
                "\\name(): File=\\unhex(\\half1(\\value(0))), Graphic ID=\\unhex(\\half2(\\value(0))), Undefined1=\\value(1), Undefined2=\\value(2)",
                new long[] {0, 3, 3}));
            EventDictionary.Add(0x18010300, new ActionEventInfo(0x18010300, "Character Spef GFX 02",
                "Appears to control posture graphics.",
                new string[] { },
                new string[] { },
                "",
                new long[] { }));
            EventDictionary.Add(0x17000000, new ActionEventInfo(0x17000000, "Physics Normalize",
                "Returns to normal physics.",
                new string[] { },
                new string[] { },
                "",
                new long[] { }));
            EventDictionary.Add(0x10080200, new ActionEventInfo(0x10080200, "Set Remote Article SubAction",
                "Does the same thing as Set Article Action but seems to work on articles that aren't attached to the character. (Used on Snake's downB)",
                new string[] {"Article ID", "SubAction"},
                new string[]
                    {"ID of the article to be affected.", "The subaction you would like the article to execute."},
                "",
                new long[] {0, 0}));
            EventDictionary.Add(0x10020100, new ActionEventInfo(0x10020100, "ONLY Article Event",
                "Article Animation.",
                new string[] { },
                new string[] { },
                "",
                new long[] { }));
            EventDictionary.Add(0x060A0900, new ActionEventInfo(0x060A0900, "Catch Collision 2",
                "Generate a grabbing collision bubble with the specified parameters",
                new string[]
                    {"ID", "Bone", "Size", "X offset", "Y Offset", "Z Offset", "Action", "Air/Ground", "Unknown"},
                new string[]
                {
                    "ID of catch collision.", "The bone the grab is attached to.",
                    "The size of the catch collision bubble.", "Transition relative to the currently attached bone.",
                    "Transition relative to the currently attached bone.",
                    "Transition relative to the currently attached bone.",
                    "The Action ID that the foe executes if successfully grabbed.",
                    "0 = grabs nothing, 1 = grabs grounded only, 2 = grabs aerial only, 3 = grabs aerial and grounded.",
                    "???"
                },
                "",
                new long[] {0, 0, 1, 1, 1, 1, 0, 0, 0}));
            EventDictionary.Add(0x00060000, new ActionEventInfo(0x00060000, "Loop Break?",
                "Breaks out of the current loop?",
                new string[] { },
                new string[] { },
                "",
                new long[] { }));
            EventDictionary.Add(0x000A0300, new ActionEventInfo(0x000A0300, "If Unknown",
                "Start an If block until an Else Or an EndIf is reached.",
                new string[] {"Requirement", "Variable", "Unknown"},
                new string[]
                {
                    "The form of requirement used in evaluation of the event.",
                    "The variable applied to the requirement.", "Undefined"
                },
                "If \\value(0): \\value(1); Unknown=\\value(2)",
                new long[] {6, 0, 0}));
            EventDictionary.Add(0x000D0100, new ActionEventInfo(0x000D0100, "Else If",
                "Insert an Else If block inside of an If block.",
                new string[] {"Requirement"},
                new string[] {"The form of requirement used in evaluation of the event."},
                "Else If \\value(0):",
                new long[] {6}));
            EventDictionary.Add(0x02010400, new ActionEventInfo(0x02010400, "Change Action",
                "Change the current action upon the specified requirement being met. (the requirement does not have to be met at the time this ID is executed - it can be used anytime after execution.)",
                new string[] {"Action", "Requirement", "Variable", "Unknown"},
                new string[]
                {
                    "The id of the action that the character will execute.",
                    "The form of requirement used in evaluation of the event.",
                    "The variable applied to the requirement.",
                    "Used as a specification for the requirement when necessary (for example, if the requirement is \"button pressed\", this denotes which button)."
                },
                "\\name(): \\value(0): \\value(1); Unknown=\\value(2)",
                new long[] {0, 6, 5, 0}));
            EventDictionary.Add(0x02020300, new ActionEventInfo(0x02020300, "Unknown",
                "Used in the Primid file as alternatives to calling an AI procedure.",
                new string[] {"Undefined", "Undefined", "Undefined"},
                new string[]
                {
                    "In the Primid file, seems to always equal 200.",
                    "An action? (equals 7 when current action is not 7)",
                    "In the Primid file, seems to always be Article Exists (15)."
                },
                "",
                new long[] {0, 0, 6}));
            EventDictionary.Add(0x02040300, new ActionEventInfo(0x02040300,
                "Additional Change Action Requirement Value",
                "Add an additional requirement to the preceeding Change Action statement.",
                new string[] {"Requirement", "Variable", "Undefined"},
                new string[]
                {
                    "The form of requirement used in evaluation of the event.",
                    "The variable applied to the requirement.", "Undefined"
                },
                "",
                new long[] {6, 5, 0}));
            EventDictionary.Add(0x020E0000, new ActionEventInfo(0x020E0000, "Unknown",
                "Used when the Blast Box detonates from a flame attack just before the change to the explosion action. Could be some sort of \"halt current action immediately\" or \"disable all possible statusID-based action changes\".",
                new string[] { },
                new string[] { },
                "",
                new long[] { }));
            EventDictionary.Add(0x04090100, new ActionEventInfo(0x04090100, "Unknown",
                "Used a few times in the Primid file.",
                new string[] {"Undefined"},
                new string[] {"Undefined"},
                "",
                new long[] {0}));
            EventDictionary.Add(0x04100200, new ActionEventInfo(0x04100200, "Subactions 10",
                "",
                new string[] {"Undefined", "Undefined"},
                new string[] {"Undefined.", "Undefined"},
                "",
                new long[] {0, 0}));
            EventDictionary.Add(0x05050100, new ActionEventInfo(0x05050100, "Posture 05",
                "",
                new string[] {"Undefined"},
                new string[] {"Undefined"},
                "",
                new long[] {1}));
            EventDictionary.Add(0x06140200, new ActionEventInfo(0x06140200, "?",
                "Used to increase the damage of the Giant Punch when not fully charged.",
                new string[] {"Id", "Source"},
                new string[]
                {
                    "The ID of the hitbox to change the damage of.",
                    "The variable to read to find out how much to change the damage."
                },
                "",
                new long[] {0, 5}));
            EventDictionary.Add(0x06192F00, new ActionEventInfo(0x06192F00, "SSE Hitbox",
                "The type of hitboxes used for enemies in the Subspace Emissary.",
                new string[]
                {
                    "ID", "Undefined", "Bone", "Damage?", "Damage ramp?", "Angle", "Knockback growth?",
                    "Knockback growth ramp?", "Weight-based knockback?", "WBK ramp?", "Base knockback?",
                    "Base knockback ramp?", "Size?", "Size ramp?", "X Pos?", "Y Pos?", "Z Pos?", "Effect",
                    "Trip chance?", "Freeze frames multiplier?", "SDI multiplier?", "Undefined", "Undefined",
                    "Undefined", "Undefined", "Undefined", "Undefined", "Undefined", "Undefined", "Undefined",
                    "Undefined", "Undefined", "Undefined", "Undefined", "Undefined", "Undefined", "Rehit rate?",
                    "Undefined", "Undefined", "Undefined", "Undefined", "Undefined", "Undefined", "Undefined",
                    "Undefined", "Undefined", "Difficulty level?"
                },
                new string[]
                {
                    "Undefined.", "Undefined.", "Undefined.", "Undefined.", "Undefined.", "Undefined.", "Undefined.",
                    "Undefined.", "Undefined.", "Undefined.", "Undefined.", "Undefined.", "Undefined.", "Undefined.",
                    "Undefined.", "Undefined.", "Undefined.", "i.e. Electric (3), Flame (5), etc", "Undefined.",
                    "Undefined.", "Undefined.", "Undefined.", "Undefined.", "Undefined.", "Undefined.",
                    "Aside from effect, the only value to vary between Jyk types (for stage 040201 anyway).",
                    "Undefined.", "Undefined.", "Undefined.", "Undefined.", "Undefined.", "Undefined.", "Undefined.",
                    "Undefined.", "Undefined.", "Undefined.", "Undefined.", "Undefined.", "Undefined.", "Undefined.",
                    "Undefined.", "Undefined.", "Undefined.", "Undefined.", "Undefined.", "Undefined.", "Undefined."
                },
                "",
                new long[]
                {
                    0, 0, 0, 0, 1, 0, 0, 1, 0, 1, 0, 1, 1, 1, 1, 1, 1, 0, 1, 1, 1, 3, 3, 0, 0, 0, 0, 3, 0, 0, 3, 0, 3,
                    3, 3, 3, 0, 3, 3, 0, 3, 3, 3, 3, 3, 0, 5
                }));
            EventDictionary.Add(0x06240F00, new ActionEventInfo(0x06240F00, "Unknown",
                "Used a single time in the Primid file.",
                new string[]
                {
                    "Undefined", "Undefined", "Undefined", "Size?", "Undefined", "Undefined", "Undefined", "Undefined",
                    "Undefined", "Undefined", "Undefined", "Undefined", "Undefined", "Undefined", "Undefined"
                },
                new string[]
                {
                    "Undefined.", "Undefined.", "Undefined.", "Undefined.", "Undefined.", "Undefined.", "Undefined.",
                    "Undefined.", "Undefined.", "Undefined.", "Undefined.", "Undefined.", "Undefined.", "Undefined.",
                    "Undefined."
                },
                "",
                new long[] {0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 3, 1}));
            EventDictionary.Add(0x0E040100, new ActionEventInfo(0x0E040100, "Disable Horizontal Gravity",
                "When set to 1, horizontal speed and decay rate are reset back to 0.",
                new string[] { },
                new string[] { },
                "\\name(): \\unhex(\\value(0))",
                new long[] { }));
            EventDictionary.Add(0x0E050100, new ActionEventInfo(0x0E040100, "Enable Horizontal Gravity?",
                "Undefined.",
                new string[] { },
                new string[] { },
                "\\name(): \\unhex(\\value(0))",
                new long[] { }));
            EventDictionary.Add(0x0E080200, new ActionEventInfo(0x0E080200, "Set Momentum",
                "Controls the movement velocity of the object.",
                new string[] {"Horizontal Velocity", "Vertical Velocity"},
                new string[] {"The speed of the object moving left/right.", "The speed of the object moving up/down."},
                "",
                new long[] {1, 1}));
            EventDictionary.Add(0x10010200, new ActionEventInfo(0x10010200, "Set Ex-Anchored Article Action",
                "Does the same thing as Set Article Action and Set Article Action 2 but seems to work on articles that are only initially attached to the character. (Used on Lucario's Aura Sphere)",
                new string[] {"Article ID", "Action"},
                new string[] {"ID of the article to be affected.", "The action you would like the article to execute."},
                "",
                new long[] {0, 0}));
            EventDictionary.Add(0x0000100A, new ActionEventInfo(0x0000100A, "Undefined",
                "Generate a prop effect with the specified parameters.",
                new string[] { },
                new string[] { },
                "",
                new long[] { }));
            EventDictionary.Add(0x10130100, new ActionEventInfo(0x10130100, "Link Character and Article?",
                "Seems to be used whenever a detached article needs to change its action...",
                new string[] {"Article ID"},
                new string[] {"ID of the article to be affected."},
                "",
                new long[] {0}));
            EventDictionary.Add(0x110D0100, new ActionEventInfo(0x110D0100, "Unknown",
                "",
                new string[] {"Undefined"},
                new string[] {"Undefined"},
                "",
                new long[] {0}));
            EventDictionary.Add(0x11170600, new ActionEventInfo(0x11170600, "Unlimited Screen Tint",
                "Tint the screen to the specified color until terminated by 11180200 (End Screen Tint).",
                new string[] {"ID", "Transition Time", "Red", "Green", "Blue", "Alpha"},
                new string[]
                {
                    "The ID of the screen tint.",
                    "The time taken to transition from its current color to the specified color.", "The red value.",
                    "The green value.", "The blue value.", "The transperency."
                },
                "\\name(): ID=\\value(0), TransitionTime=\\value(1), RGBA=(\\value(2), \\value(3), \\value(4), \\value(5))",
                new long[] {0, 0, 0, 0, 0, 0}));
            EventDictionary.Add(0x12100200, new ActionEventInfo(0x12100200, "Float Variable Divide",
                "Divide a specified value with a float variable.",
                new string[] {"Value", "Variable"},
                new string[]
                {
                    "The floating point value to divide with the specified variable.",
                    "The Float type variable to access."
                },
                "\\name(): \\value(1) /= \\unhex(\\value(0))",
                new long[] {1, 5}));
            EventDictionary.Add(0x15000000, new ActionEventInfo(0x15000000, "Unknown",
                "Used in the Goomba file in places where Req[0x11] is true.",
                new string[] { },
                new string[] { },
                "",
                new long[] { }));
            EventDictionary.Add(0x1B020100, new ActionEventInfo(0x1B020100, "Procedure Call?",
                "",
                new string[] {"Target Procedure"},
                new string[] {"Undefined"},
                "",
                new long[] {4}));
            EventDictionary.Add(0x1E010100, new ActionEventInfo(0x1E010100, "Set Damage Immunity?",
                "Used at the start of Withdraw; might have something to do with Squirtle's immunity to damage during the move.",
                new string[] {"Undefined"},
                new string[] {"Undefined"},
                "",
                new long[] {3}));
            EventDictionary.Add(0x1F000400, new ActionEventInfo(0x1F000400, "Pickup Item",
                "Cause the character to receive the closest item in range.",
                new string[] {"Undefined", "Undefined", "Undefined", "Undefined"},
                new string[] {"Undefined.", "Undefined.", "Undefined.", "Undefined"},
                "",
                new long[] {0, 0, 0, 0}));
            EventDictionary.Add(0x22000100, new ActionEventInfo(0x22000100, "Set Team?",
                "Used with a parameter of -1 for a few explosives just before they go off (possibly to remove team allegiance and therefore hit the user).",
                new string[] {"Undefined"},
                new string[] {"Undefined"},
                "",
                new long[] {0}));
            EventDictionary.Add(0x22020100, new ActionEventInfo(0x22020100, "Unknown",
                "",
                new string[] {"Undefined"},
                new string[] {"Undefined"},
                "",
                new long[] {0}));
            EventDictionary.Add(0x65000000, new ActionEventInfo(0x65000000, "Item Self-Delete?",
                "Undefined",
                new string[] { },
                new string[] { },
                "",
                new long[] { }));
            EventDictionary.Add(0x65030200, new ActionEventInfo(0x65030200, "Unknown",
                "Something with rotation on item spawning.",
                new string[] {"Undefined", "Undefined"},
                new string[] {"Undefined.", "Undefined"},
                "",
                new long[] {1, 1}));
            EventDictionary.Add(0x65040100, new ActionEventInfo(0x65040100, "Deletion Timer?",
                "Sets how many frames an item has to live? (Also used in enemy files.)",
                new string[] {"Lifetime (frames)?"},
                new string[] {"Undefined"},
                "",
                new long[] {0}));
            EventDictionary.Add(0x65050100, new ActionEventInfo(0x65050100, "Unknown",
                "Unknown. Appears to be another timer.",
                new string[] {"Undefined"},
                new string[] {"Undefined"},
                "",
                new long[] {0}));
            EventDictionary.Add(0x65070200, new ActionEventInfo(0x65070200, "Unknown",
                "Unknown. Appears to affect float variables. Used twice in the Jyk file.",
                new string[] {"Undefined", "Undefined"},
                new string[] {"Undefined.", "Undefined"},
                "",
                new long[] {5, 5}));
            EventDictionary.Add(0x65090200, new ActionEventInfo(0x65090200, "Unknown",
                "Unknown. Appears to affect float variables.",
                new string[] {"Undefined", "Undefined"},
                new string[] {"Undefined.", "Undefined"},
                "",
                new long[] {5, 5}));
            EventDictionary.Add(0x650F0200, new ActionEventInfo(0x650F0200, "Unknown",
                "",
                new string[] {"Undefined", "Undefined"},
                new string[] {"Undefined.", "Undefined"},
                "",
                new long[] {5, 0}));
            EventDictionary.Add(0x65130900, new ActionEventInfo(0x65130900, "Generate Ratio-Based Random Number",
                "Generates a random number from 0 to [number of arguments-2], with the arguments starting at 1 being based on ratios.",
                new string[]
                {
                    "Result Variable", "Ratio 1", "Ratio 2", "Ratio 3", "Ratio 4", "Ratio 5", "Ratio 6", "Ratio 7",
                    "Ratio 8"
                },
                new string[]
                {
                    "The result of the function is put in this variable.", "Undefined.", "Undefined.", "Undefined.",
                    "Undefined.", "Undefined.", "Undefined.", "Undefined.", "Undefined"
                },
                "",
                new long[] {5, 1, 1, 1, 1, 1, 1, 1, 1}));
            EventDictionary.Add(0x65170200, new ActionEventInfo(0x65170200, "Unknown",
                "Has something to do with sounds?",
                new string[] {"Undefined", "Undefined"},
                new string[] {"Undefined.", "Undefined"},
                "",
                new long[] {0, 0}));
            EventDictionary.Add(0x651B0100, new ActionEventInfo(0x651B0100, "Activate slow motion?",
                "Used in the Dragoon.",
                new string[] {"Duration?"},
                new string[] {"Undefined"},
                "",
                new long[] {0}));
            EventDictionary.Add(0x651C0000, new ActionEventInfo(0x651C0000, "Deactivate slow motion?",
                "Used in the Dragoon.",
                new string[] { },
                new string[] { },
                "",
                new long[] { }));
            EventDictionary.Add(0x651F0100, new ActionEventInfo(0x651F0100, "Unknown",
                "Unknown. Used in the bumper item at least.",
                new string[] {"Undefined"},
                new string[] {"Undefined"},
                "",
                new long[] {0}));
            EventDictionary.Add(0x652C0000, new ActionEventInfo(0x652C0000, "Unknown",
                "Unknown. In charizard's sideB subaction. If you nop it, he will not make rock shards or play the rock break sfx. wtf.",
                new string[] { },
                new string[] { },
                "",
                new long[] { }));
            EventDictionary.Add(0x66000200, new ActionEventInfo(0x66000200, "Unknown",
                "",
                new string[] {"Undefined", "Undefined"},
                new string[] {"Undefined.", "Undefined"},
                "",
                new long[] {0, 0}));
            EventDictionary.Add(0x66000400, new ActionEventInfo(0x66000400, "Unknown",
                "Unknown. Used five times in the Jyk file with the values increasing somewhat constantly for each one (difficulty-related?).",
                new string[] {"Undefined", "Undefined", "Undefined", "Undefined"},
                new string[] {"Undefined.", "Undefined.", "Undefined.", "Undefined"},
                "",
                new long[] {0, 0, 1, 0}));
            EventDictionary.Add(0x66060100, new ActionEventInfo(0x66060100, "Unknown",
                "Unknown. Used in action C of bumper at least.",
                new string[] {"Undefined"},
                new string[] {"Undefined"},
                "",
                new long[] {0}));
            EventDictionary.Add(0x66070100, new ActionEventInfo(0x66070100, "Unknown",
                "",
                new string[] {"Undefined"},
                new string[] {"Undefined"},
                "",
                new long[] {0}));
            EventDictionary.Add(0x66090200, new ActionEventInfo(0x66090200, "Unknown",
                "",
                new string[] {"Undefined", "Undefined"},
                new string[] {"Undefined.", "Undefined"},
                "",
                new long[] {0, 0}));
            EventDictionary.Add(0x660A0200, new ActionEventInfo(0x660A0200, "Unknown",
                "",
                new string[] {"Undefined", "Undefined"},
                new string[] {"Undefined.", "Undefined"},
                "",
                new long[] {0, 0}));
            EventDictionary.Add(0x660B0200, new ActionEventInfo(0x660B0200, "Unknown",
                "",
                new string[] {"Undefined", "Undefined"},
                new string[] {"Undefined.", "Undefined"},
                "",
                new long[] {0, 0}));
            EventDictionary.Add(0x660B0300, new ActionEventInfo(0x660B0300, "Unknown",
                "Unknown. Something with spawn rotation.",
                new string[] {"Undefined", "Undefined", "Undefined"},
                new string[] {"Undefined.", "Undefined.", "Undefined"},
                "",
                new long[] {1, 1, 1}));
            EventDictionary.Add(0x69000100, new ActionEventInfo(0x69000100, "Unknown",
                "Only known to be used in cases where \"Req[0x12], 1, \" is true.",
                new string[] {"Undefined"},
                new string[] {"Undefined"},
                "",
                new long[] {5}));
            EventDictionary.Add(0x6A000100, new ActionEventInfo(0x6A000100, "Unknown",
                "",
                new string[] {"Undefined"},
                new string[] {"Undefined"},
                "",
                new long[] {0}));
            EventDictionary.Add(0x6A000200, new ActionEventInfo(0x6A000200, "Unknown",
                "",
                new string[] {"Undefined", "Undefined"},
                new string[] {"Undefined.", "Undefined"},
                "",
                new long[] {0, 0}));
            EventDictionary.Add(0x6A000400, new ActionEventInfo(0x6A000400, "Unknown",
                "",
                new string[] {"Undefined", "Undefined", "Undefined", "Undefined"},
                new string[] {"Undefined.", "Undefined.", "Undefined.", "Undefined"},
                "",
                new long[] {0, 0, 0, 0}));
            EventDictionary.Add(0x6A010100, new ActionEventInfo(0x6A010100, "Unknown",
                "",
                new string[] {"Undefined"},
                new string[] {"Undefined"},
                "",
                new long[] {0}));
            EventDictionary.Add(0x6A020000, new ActionEventInfo(0x6A020000, "Unknown",
                "Undefined",
                new string[] { },
                new string[] { },
                "",
                new long[] { }));
            EventDictionary.Add(0x6A030000, new ActionEventInfo(0x6A030000, "Unknown",
                "Undefined",
                new string[] { },
                new string[] { },
                "",
                new long[] { }));
            EventDictionary.Add(0x6B020300, new ActionEventInfo(0x6B020300, "Unknown",
                "",
                new string[] {"Undefined", "Undefined", "Undefined"},
                new string[] {"Undefined.", "Undefined.", "Undefined."},
                "",
                new long[] {3, 5, 5}));
            EventDictionary.Add(0x6E000200, new ActionEventInfo(0x6E000200, "Unknown",
                "",
                new string[] {"Undefined", "Undefined"},
                new string[] {"Undefined.", "Undefined"},
                "",
                new long[] {0, 0}));
            EventDictionary.Add(0x000A0500, new ActionEventInfo(0x000A0500, "Unknown",
                "",
                new string[] { },
                new string[] { },
                "If \\value(0): \\value(1); Unknown=\\value(2), \\value(3), & \\value(4)",
                new long[] { }));
            EventDictionary.Add(0x000B0500, new ActionEventInfo(0x000B0500, "Unknown",
                "",
                new string[] { },
                new string[] { },
                "Or \\value(0): \\value(1); Unknown=\\value(2), \\value(3), & \\value(4)",
                new long[] { }));
            EventDictionary.Add(0x000D0500, new ActionEventInfo(0x000D0500, "Unknown",
                "",
                new string[] { },
                new string[] { },
                "Else If \\value(0): \\value(1); Unknown=\\value(2), \\value(3), & \\value(4)",
                new long[] { }));
            EventDictionary.Add(0x02050600, new ActionEventInfo(02050600, "Actions 05 Compare",
                "Undefined.",
                new string[] {"Interrupt ID?", "Status ID?", "Requirement", "Variable", "Comparison", "Variable"},
                new string[]
                {
                    "Undefined.", "Undefined.", "Undefined.", "First variable to compare.",
                    "From 0 to 5: <, ≤, =, ≠, ≥, >.", "Second variable to compare."
                },
                "\\name(): Interrupt=\\unhex(\\value(0)), Status ID=\\unhex(\\value(1)), Requirement=\\value(2): \\value(3) \\cmpsign(\\value(4)) \\value(5)",
                new long[] { }));
            EventDictionary.Add(0x02050400, new ActionEventInfo(02050400, "Actions 05 Value",
                "Undefined.",
                new string[] {"Interrupt ID?", "Status ID?", "Requirement", "Value"},
                new string[] {"Undefined.", "Undefined.", "Undefined.", "Value applied to the requirement.",},
                "\\name(): Interrupt=\\unhex(\\value(0)), Status ID=\\unhex(\\value(1)), Requirement=\\value(2): \\value(3)",
                new long[] { }));

            //Now add on to events with user inputted data

            StreamReader sr = null;
            long idNumber = 0;
            string loc, id;

            //Read known events and their descriptions.
            loc = Application.StartupPath + "/MovesetData/Events.txt";
            if (File.Exists(loc))
            {
                using (sr = new StreamReader(loc))
                {
                    for (int i = 0; !sr.EndOfStream; i++)
                    {
                        id = sr.ReadLine();
                        idNumber = Convert.ToInt32(id, 16);

                        if (!EventDictionary.Keys.Contains(idNumber))
                        {
                            EventDictionary.Add(idNumber, new ActionEventInfo());
                        }

                        EventDictionary[idNumber].idNumber = idNumber;
                        EventDictionary[idNumber]._name = sr.ReadLine();
                        EventDictionary[idNumber]._description = sr.ReadLine();
                        EventDictionary[idNumber].SetDfltParameters(sr.ReadLine());
                        sr.ReadLine();
                    }
                }
            }

            //Read known parameters and their descriptions.
            loc = Application.StartupPath + "/MovesetData/Parameters.txt";
            if (File.Exists(loc))
            {
                using (sr = new StreamReader(loc))
                {
                    for (int i = 0; !sr.EndOfStream; i++)
                    {
                        id = sr.ReadLine();
                        idNumber = Convert.ToInt32(id, 16);

                        if (!EventDictionary.Keys.Contains(idNumber))
                        {
                            EventDictionary.Add(idNumber, new ActionEventInfo());
                        }

                        for (int i2 = 0;; i2++)
                        {
                            string name = sr.ReadLine();
                            if (name == null)
                            {
                                name = "";
                            }

                            if (name != "")
                            {
                                Array.Resize(ref EventDictionary[idNumber].Params, i2 + 1);
                                Array.Resize(ref EventDictionary[idNumber].pDescs, i2 + 1);
                                EventDictionary[idNumber].Params[i2] = name;
                                EventDictionary[idNumber].pDescs[i2] = sr.ReadLine();
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                }
            }

            //Read the list containing the syntax to display each event with.
            loc = Application.StartupPath + "/MovesetData/EventSyntax.txt";
            if (File.Exists(loc))
            {
                using (sr = new StreamReader(loc))
                {
                    for (int i = 0; !sr.EndOfStream; i++)
                    {
                        string syntax = "";
                        id = sr.ReadLine();
                        try
                        {
                            idNumber = Convert.ToInt32(id, 16);
                        }
                        catch
                        {
                            syntax = id;
                            goto AddSyntax;
                        } //Most likely syntax where the id should be

                        //Clear the existing syntax
                        EventDictionary[idNumber]._syntax = "";

                        syntax = sr.ReadLine();

                        if (!EventDictionary.Keys.Contains(idNumber))
                        {
                            EventDictionary.Add(idNumber, new ActionEventInfo());
                        }

                        EventDictionary[idNumber].idNumber = idNumber;

                        AddSyntax:
                        while (syntax != "" && syntax != null)
                        {
                            EventDictionary[idNumber]._syntax += syntax;
                            syntax = sr.ReadLine();
                        }
                    }
                }
            }

            Dictionary<long, List<int>> list = new Dictionary<long, List<int>>();
            List<string> enums = new List<string>();
            loc = Application.StartupPath + "/MovesetData/Enums.txt";
            if (File.Exists(loc))
            {
                using (sr = new StreamReader(loc))
                {
                    while (!sr.EndOfStream)
                    {
                        list = new Dictionary<long, List<int>>();
                        enums = new List<string>();
                        while (!string.IsNullOrEmpty(id = sr.ReadLine()))
                        {
                            idNumber = Convert.ToInt32(id, 16);

                            if (!list.ContainsKey(idNumber))
                            {
                                list.Add(idNumber, new List<int>());
                            }

                            string p = null;
                            while (!string.IsNullOrEmpty(p = sr.ReadLine()))
                            {
                                list[idNumber].Add(int.Parse(p));
                            }
                        }

                        string val = null;
                        while (!string.IsNullOrEmpty(val = sr.ReadLine()))
                        {
                            enums.Add(val);
                        }

                        foreach (long ev in list.Keys)
                        {
                            if (EventDictionary.ContainsKey(ev))
                            {
                                EventDictionary[ev].Enums = new Dictionary<int, List<string>>();
                                foreach (int p in list[ev])
                                {
                                    EventDictionary[ev].Enums.Add(p, enums);
                                }
                            }
                        }
                    }
                }
            }

            //CreateEventDictionary();
        }

        public void CreateEventDictionary()
        {
            string p1 = "EventDictionary.Add(";
            //idNumber
            string p2 = ", new ActionEventInfo(";
            //idNumber
            string p3 = ", \"";
            //name
            string p4 = "\",\n    \"";
            //description
            string p5 = "\",\n    new string[] {";
            //param name array
            string p6 = " },\n    new string[] {";
            //param description array
            string p7 = " },\n    \"";
            //syntax
            string p8 = "\",\n    new long[] {";
            //default params
            string p9 = " }));";

            string dic = "", idString = "";
            foreach (ActionEventInfo i in EventDictionary.Values)
            {
                idString = "0x" + i.idNumber.ToString("X").PadLeft(8, '0');
                dic += p1 + idString + p2 + idString + p3 + i._name.Replace("\"", "\\\"") + p4 +
                       i._description.Replace("\"", "\\\"") + p5;
                bool first = true;
                if (i.Params != null)
                {
                    foreach (string s in i.Params)
                    {
                        if (!first)
                        {
                            dic += ",";
                        }
                        else
                        {
                            first = false;
                        }

                        dic += " \"" + s.Replace("\"", "\\\"") + "\"";
                    }
                }
                else
                {
                    dic += " ";
                }

                dic += p6;
                first = true;
                if (i.pDescs != null)
                {
                    foreach (string s in i.pDescs)
                    {
                        if (!first)
                        {
                            dic += ",";
                        }
                        else
                        {
                            first = false;
                        }

                        dic += " \"" + s.Replace("\"", "\\\"") + "\"";
                    }
                }
                else
                {
                    dic += " ";
                }

                dic += p7 + i._syntax.Replace("\\", "\\\\") + p8;
                first = true;
                if (i.defaultParams != null)
                {
                    foreach (long s in i.defaultParams)
                    {
                        if (!first)
                        {
                            dic += ",";
                        }
                        else
                        {
                            first = false;
                        }

                        dic += " " + s;
                    }
                }
                else
                {
                    dic += " ";
                }

                dic += p9;
                Console.WriteLine(dic);
                dic = "";
            }
        }

        #endregion

        #region Stuff to find other stuff

        public List<ResourceNode> _externalRefs;
        public List<MoveDefExternalNode> _externalSections;

        public MoveDefExternalNode IsExternal(int offset)
        {
            foreach (MoveDefExternalNode e in _externalRefs)
            {
                foreach (int i in e._offsets)
                {
                    if (i == offset)
                    {
                        return e;
                    }
                }
            }

            foreach (MoveDefExternalNode e in _externalSections)
            {
                foreach (int i in e._offsets)
                {
                    if (i == offset)
                    {
                        return e;
                    }
                }
            }

            return null;
        }

        public ResourceNode FindNode(int offset)
        {
            ResourceNode n;
            if (offset == 0)
            {
                return this;
            }
            else
            {
                foreach (MoveDefEntryNode e in Children)
                {
                    if ((n = e.FindNode(offset)) != null)
                    {
                        return n;
                    }
                }
            }

            return null;
        }

        public MoveDefActionNode GetAction(int offset)
        {
            int list, type, index;
            GetLocation(offset, out list, out type, out index);
            return GetAction(list, type, index);
        }

        public MoveDefActionNode GetAction(int list, int type, int index)
        {
            if (list >= 3 && dataCommon == null || list == 4 || index == -1)
            {
                return null;
            }

            if (list > 4 && dataCommon != null)
            {
                if (list == 5 && type >= 0 && index < dataCommon._flashOverlay.Children.Count)
                {
                    return (MoveDefActionNode) dataCommon._flashOverlay.Children[index]; //.Children[0];
                }

                if (list == 6 && type >= 0 && index < dataCommon._screenTint.Children.Count)
                {
                    return (MoveDefActionNode) dataCommon._screenTint.Children[index]; //.Children[0];
                }
            }

            if (list == 0 && type >= 0 && index < _actions.Children.Count)
            {
                return (MoveDefActionNode) _actions.Children[index].Children[type];
            }

            if (list == 1 && type >= 0 && index < _subActions.Children.Count)
            {
                return (MoveDefActionNode) _subActions.Children[index].Children[type];
            }

            if (list == 2 && _subRoutineList.Count > index)
            {
                return (MoveDefActionNode) _subRoutineList[index];
            }

            return null;
        }

        public int GetOffset(int list, int type, int index)
        {
            if (list == 4 || index == -1)
            {
                return -1;
            }

            if (list == 0 && type >= 0 && type < _actions.ActionOffsets.Count)
            {
                if (_actions.ActionOffsets[type].Count > index)
                {
                    return _actions.ActionOffsets[type][index];
                }
            }

            if (list == 1 && type >= 0 && type < _subActions.ActionOffsets.Count)
            {
                if (_subActions.ActionOffsets[type].Count > index)
                {
                    return _subActions.ActionOffsets[type][index];
                }
            }

            if (list == 2)
            {
                if (_subRoutineList.Count > index)
                {
                    return ((MoveDefEntryNode) _subRoutineList[index])._offset;
                }
            }

            if (list == 3)
            {
                if (_externalRefs.Count > index)
                {
                    return ((MoveDefEntryNode) _externalRefs[index])._offset;
                }
            }

            return -1;
        }

        public void GetLocation(int offset, out int list, out int type, out int index)
        {
            list = 0;
            type = -1;
            index = -1;

            bool done = false;

            if (dataCommon == null && data == null || offset <= 0)
            {
                list = 4; //Null
                done = true;
            }

            if (!done && _actions != null) //Search actions
            {
                for (type = 0; type < _actions.ActionOffsets.Count; type++)
                {
                    if ((index = _actions.ActionOffsets[type].IndexOf(offset)) != -1)
                    {
                        done = true;
                        break;
                    }
                }
            }

            if (!done) //Search sub actions
            {
                list++;
                if (_subActions != null)
                {
                    for (type = 0; type < _subActions.ActionOffsets.Count; type++)
                    {
                        if ((index = _subActions.ActionOffsets[type].IndexOf(offset)) != -1)
                        {
                            done = true;
                            break;
                        }
                    }
                }
            }

            if (!done) //Search subroutines
            {
                list++;
                if (_subRoutines.ContainsKey(offset))
                {
                    index = _subRoutines[offset].Index;
                    type = -1;
                    done = true;
                }
            }

            if (!done)
            {
                list++;
                MoveDefExternalNode e = IsExternal(offset);
                if (e != null)
                {
                    index = e.Index;
                    type = -1;
                    done = true;
                }
            }

            if (!done)
            {
                list++;
                type = -1;
                index = -1;
            }

            if (dataCommon != null && data == null && offset > 0)
            {
                if (dataCommon._screenTint != null && !done)
                {
                    list++;
                    if ((index = dataCommon._screenTint.ActionOffsets.IndexOf((uint) offset)) != -1)
                    {
                        return;
                    }
                }

                if (dataCommon._flashOverlay != null && !done)
                {
                    list++;
                    if ((index = dataCommon._flashOverlay.ActionOffsets.IndexOf((uint) offset)) != -1)
                    {
                        return;
                    }
                }
            }

            if (!done)
            {
                list = 4;
            }
        }

        #endregion

        public int GetSize(int offset)
        {
            if (_lookupSizes.ContainsKey(offset))
            {
                //_lookupSizes[offset].remove = true;
                return _lookupSizes[offset].DataSize;
            }

            return -1;
        }

        public void GetBoneIndex(ref int boneIndex)
        {
            if (RootNode.Name.StartsWith("FitWario") || RootNode.Name == "FitKirby")
            {
                if (data != null)
                {
                    if (data.warioParams8 != null)
                    {
                        MoveDefSectionParamNode p1 = data.warioParams8.Children[0] as MoveDefSectionParamNode;
                        MoveDefSectionParamNode p2 = data.warioParams8.Children[1] as MoveDefSectionParamNode;
                        bint* values = (bint*) p2.AttributeBuffer.Address;
                        int i = 0;
                        for (; i < p2.AttributeBuffer.Length / 4; i++)
                        {
                            if (values[i] == boneIndex)
                            {
                                break;
                            }
                        }

                        if (p1.AttributeBuffer.Length / 4 > i)
                        {
                            int value = -1;
                            if ((value = (int) ((bint*) p1.AttributeBuffer.Address)[i]) >= 0)
                            {
                                boneIndex = value;
                                return;
                            }
                            else
                            {
                                boneIndex -= 400;
                            }
                        }
                    }
                }
            }
        }

        public void SetBoneIndex(ref int boneIndex)
        {
            if (RootNode.Name.StartsWith("FitWario") || RootNode.Name == "FitKirby")
            {
                if (data != null)
                {
                    if (data.warioParams8 != null)
                    {
                        MoveDefSectionParamNode p1 = data.warioParams8.Children[0] as MoveDefSectionParamNode;
                        MoveDefSectionParamNode p2 = data.warioParams8.Children[1] as MoveDefSectionParamNode;
                        bint* values = (bint*) p2.AttributeBuffer.Address;
                        int i = 0;
                        for (; i < p1.AttributeBuffer.Length / 4; i++)
                        {
                            if (values[i] == boneIndex)
                            {
                                break;
                            }
                        }

                        if (p2.AttributeBuffer.Length / 4 > i)
                        {
                            int value = -1;
                            if ((value = ((bint*) p2.AttributeBuffer.Address)[i]) >= 0)
                            {
                                boneIndex = value;
                                return;
                            }
                        }
                    }
                }
            }
        }

        public bool[] StatusIDs;

        public Dictionary<uint, List<MoveDefEventNode>> _events;

        public SortedList<int, string> _paths = new SortedList<int, string>();
        public SortedList<int, string> Paths => _paths;

        public string[] iRequirements;
        public string[] iAirGroundStats;
        public string[] iCollisionStats;
        public string[] iGFXFiles;
        public AttributeInfo[] AttributeArray;
        public Dictionary<string, SectionParamInfo> Params;

        public MoveDefActionListNode _subActions;
        public MoveDefActionListNode _actions;

        public SortedDictionary<int, MoveDefActionNode> _subRoutines;
        public List<ResourceNode> _subRoutineList;
        public ResourceNode _subRoutineGroup;

        public MoveDefDataNode data;
        public MoveDefDataCommonNode dataCommon;

        public MoveDefReferenceNode references;
        public MoveDefSectionNode sections;
        public MoveDefLookupNode lookupNode;

        public CompactStringTable refTable;

        public Dictionary<int, MoveDefLookupOffsetNode> _lookupSizes;

        public MDL0Node _model = null;

        [Category("Moveset Definition")] public int LookupOffset => lookupOffset;

        [Category("Moveset Definition")] public int LookupCount => numLookupEntries;

        [Category("Moveset Definition")] public int DataTableCount => numDataTable;

        [Category("Moveset Definition")] public int ExtSubRoutines => numExternalSubRoutine;

        public MDL0Node Model => _model;

        public override ResourceType ResourceFileType => ResourceType.MDef;
        public VoidPtr BaseAddress;

        [Category("Moveset Definition")] public string DataSize => "0x" + dataSize.ToString("X");

        public SortedDictionary<int, MoveDefEntryNode> NodeDictionary => nodeDictionary;

        public static SortedDictionary<int, MoveDefEntryNode> nodeDictionary =
            new SortedDictionary<int, MoveDefEntryNode>();

        public override bool OnInitialize()
        {
            if (_name == null)
            {
                _name = "MoveDef_" + Parent.Name;
            }

            nodeDictionary = new SortedDictionary<int, MoveDefEntryNode>();

            dataSize = Header->_fileSize;
            lookupOffset = Header->_lookupOffset;
            numLookupEntries = Header->_lookupEntryCount;
            numDataTable = Header->_dataTableEntryCount;
            numExternalSubRoutine = Header->_externalSubRoutineCount;

            BaseAddress = (VoidPtr) Header + 0x20;
            return true;
        }

        //Offset - Size
        public Dictionary<int, int> _lookupEntries;

        #region Other Data

        public void LoadOtherData()
        {
            StreamReader sr = null;
            string loc;

            //Read the list of Event Requirements.
            loc = Application.StartupPath + "/MovesetData/Requirements.txt";
            if (File.Exists(loc))
            {
                using (sr = new StreamReader(loc))
                {
                    for (int i = 0; !sr.EndOfStream; i++)
                    {
                        Array.Resize(ref iRequirements, i + 1);
                        iRequirements[i] = sr.ReadLine();
                    }
                }
            }
            else
            {
                iRequirements = new string[124];
                iRequirements[0] = "Character Exists?";
                iRequirements[1] = "Animation End";
                iRequirements[2] = "Animation has looped?";
                iRequirements[3] = "On Ground";
                iRequirements[4] = "In Air";
                iRequirements[5] = "Holding a Ledge";
                iRequirements[6] = "On a Passable Floor";
                iRequirements[7] = "Compare";
                iRequirements[8] = "Bit is Set";
                iRequirements[9] = "Facing Right";
                iRequirements[10] = "Facing Left";
                iRequirements[11] = "Hitbox Connects";
                iRequirements[12] = "Touching a Floor, Wall, or Ceiling";
                iRequirements[13] = "Is Throwing Someone";
                iRequirements[14] = "0E";
                iRequirements[15] = "Button Tap?";
                iRequirements[16] = "10";
                iRequirements[17] = "11";
                iRequirements[18] = "12";
                iRequirements[19] = "13";
                iRequirements[20] = "Entering Hitlag; Is in Hitlag";
                iRequirements[21] = "Article Exists";
                iRequirements[22] = "Is Overstepping an Edge";
                iRequirements[23] = "Has a Floor Below the Player";
                iRequirements[24] = "18";
                iRequirements[25] = "19";
                iRequirements[26] = "1A";
                iRequirements[27] = "Change in Air/Ground State";
                iRequirements[28] = "Article Available";
                iRequirements[29] = "1D";
                iRequirements[30] = "1E";
                iRequirements[31] = "Holding Item";
                iRequirements[32] = "Holding Item of Type";
                iRequirements[33] = "Light Item is in Grabbing Range";
                iRequirements[34] = "Heavy Item is in Grabbing Range";
                iRequirements[35] = "Item of Type(?) is in Grabbing Range";
                iRequirements[36] = "Turning with Item";
                iRequirements[37] = "25";
                iRequirements[38] = "26";
                iRequirements[39] = "27";
                iRequirements[40] = "28";
                iRequirements[41] = "29";
                iRequirements[42] = "Is in Water";
                iRequirements[43] = "Roll A Die";
                iRequirements[44] = "Subaction Exists";
                iRequirements[45] = "2D";
                iRequirements[46] = "Button Mashing? Status Expired (sleep/bury/freeze)?";
                iRequirements[47] = "Is Not in Damaging Magnifier";
                iRequirements[48] = "Button Press";
                iRequirements[49] = "Button Release";
                iRequirements[50] = "Button Pressed";
                iRequirements[51] = "Button Not Pressed";
                iRequirements[52] = "Stick Direction Pressed?";
                iRequirements[53] = "Stick Direction Not Pressed?";
                iRequirements[54] = "36";
                iRequirements[55] = "Is Being Thrown by Someone (1)";
                iRequirements[56] = "Is Being Thrown by Someone (2)";
                iRequirements[57] = "Hasn't Tethered 3 Times";
                iRequirements[58] = "Has Passed Over an Edge (Forward)";
                iRequirements[59] = "Has Passed Over an Edge (Backward)";
                iRequirements[60] = "Is Holding Someone in a Grab";
                iRequirements[61] = "Hitbox has Connected";
                iRequirements[62] = "3E";
                iRequirements[63] = "3F";
                iRequirements[64] = "40";
                iRequirements[65] = "41";
                iRequirements[66] = "42";
                iRequirements[67] = "43";
                iRequirements[68] = "44";
                iRequirements[69] = "45";
                iRequirements[70] = "46";
                iRequirements[71] = "Pick up an Item";
                iRequirements[72] = "48";
                iRequirements[73] = "49";
                iRequirements[74] = "4A";
                iRequirements[75] = "4B";
                iRequirements[76] = "Has Been Hit by Cape Effect";
                iRequirements[77] = "4D";
                iRequirements[78] = "4E";
                iRequirements[79] = "4F";
                iRequirements[80] = "50";
                iRequirements[81] = "51";
                iRequirements[82] = "52";
                iRequirements[83] = "53";
                iRequirements[84] = "54";
                iRequirements[85] = "55";
                iRequirements[86] = "56";
                iRequirements[87] = "57";
                iRequirements[88] = "58";
                iRequirements[89] = "59";
                iRequirements[90] = "5A";
                iRequirements[91] = "5B";
                iRequirements[92] = "5C";
                iRequirements[93] = "5D";
                iRequirements[94] = "5E";
                iRequirements[95] = "5F";
                iRequirements[96] = "60";
                iRequirements[97] = "61";
                iRequirements[98] = "62";
                iRequirements[99] = "63";
                iRequirements[100] = "64";
                iRequirements[101] = "270F";
                iRequirements[102] = "2710";
                iRequirements[103] = "In walljump situation";
                iRequirements[104] = "In wallcling situation";
                iRequirements[105] = "Is within footstool range.";
                iRequirements[106] = "2714";
                iRequirements[107] = "2715";
                iRequirements[108] = "Is falling+hit down";
                iRequirements[109] = "Has Smash Ball";
                iRequirements[110] = "2718";
                iRequirements[111] = "Can Pick Up Another Item";
                iRequirements[112] = "271A";
                iRequirements[113] = "271B";
                iRequirements[114] = "271C";
                iRequirements[115] = "F-Smash Shortcut";
                iRequirements[116] = "271E";
                iRequirements[117] = "271F";
                iRequirements[118] = "2720";
                iRequirements[119] = "2721";
                iRequirements[120] = "2722";
                iRequirements[121] = "2723";
                iRequirements[122] = "2724";
                iRequirements[123] = "Tap Jump on";
            }

            //Read the list of Air Ground Stats.
            loc = Application.StartupPath + "/MovesetData/AirGroundStats.txt";
            if (File.Exists(loc))
            {
                using (sr = new StreamReader(loc))
                {
                    for (int i = 0; !sr.EndOfStream; i++)
                    {
                        Array.Resize(ref iAirGroundStats, i + 1);
                        iAirGroundStats[i] = sr.ReadLine();
                    }
                }
            }
            else
            {
                iAirGroundStats = new string[7];
                iAirGroundStats[0] = "In Air";
                iAirGroundStats[1] = "Floating On Ground";
                iAirGroundStats[2] = "Undefined(2)";
                iAirGroundStats[3] = "Undefined(3)";
                iAirGroundStats[4] = "Undefined(4)";
                iAirGroundStats[5] = "Undefined(5)";
                iAirGroundStats[6] = "On Ground";
            }

            //Read the list of Collision Stats.
            loc = Application.StartupPath + "/MovesetData/CollisionStats.txt";
            if (File.Exists(loc))
            {
                using (sr = new StreamReader(loc))
                {
                    for (int i = 0; !sr.EndOfStream; i++)
                    {
                        Array.Resize(ref iCollisionStats, i + 1);
                        iCollisionStats[i] = sr.ReadLine();
                    }
                }
            }
            else
            {
                iCollisionStats = new string[5];
                iCollisionStats[0] = "Normal";
                iCollisionStats[1] = "Invincible";
                iCollisionStats[2] = "Intangible";
                iCollisionStats[3] = "Intangible No Flashing";
                iCollisionStats[4] = "Intangible Quick Flashing";
            }

            //Read the list of GFX Files.
            loc = Application.StartupPath + "/MovesetData/GFXFiles.txt";
            if (File.Exists(loc))
            {
                using (sr = new StreamReader(loc))
                {
                    for (int i = 0; !sr.EndOfStream; i++)
                    {
                        Array.Resize(ref iGFXFiles, i + 1);
                        iGFXFiles[i] = sr.ReadLine();
                    }
                }
            }
            else
            {
                iGFXFiles = new string[259];
                iGFXFiles[0] = "Common";
                iGFXFiles[1] = "Mario";
                iGFXFiles[2] = "Donkey Kong";
                iGFXFiles[3] = "Link  ";
                iGFXFiles[4] = "Samus";
                iGFXFiles[5] = "Yoshi";
                iGFXFiles[6] = "Kirby";
                iGFXFiles[7] = "Fox";
                iGFXFiles[8] = "Pikachu";
                iGFXFiles[9] = "Luigi";
                iGFXFiles[10] = "Falcon";
                iGFXFiles[11] = "Ness";
                iGFXFiles[12] = "Bowser";
                iGFXFiles[13] = "Peach";
                iGFXFiles[14] = "Zelda";
                iGFXFiles[15] = "15 Shiek?";
                iGFXFiles[16] = "Popo";
                iGFXFiles[17] = "17 Nana?";
                iGFXFiles[18] = "Marth";
                iGFXFiles[19] = "19";
                iGFXFiles[20] = "Falco";
                iGFXFiles[21] = "Ganondorf";
                iGFXFiles[22] = "Wario";
                iGFXFiles[23] = "Meta Knight";
                iGFXFiles[24] = "Pit";
                iGFXFiles[25] = "Zerosuit Samus";
                iGFXFiles[26] = "Olimar";
                iGFXFiles[27] = "Lucas";
                iGFXFiles[28] = "Diddy";
                iGFXFiles[29] = "Pokemon Trainer";
                iGFXFiles[30] = "Charizard";
                iGFXFiles[31] = "Squirtle";
                iGFXFiles[32] = "Ivysaur";
                iGFXFiles[33] = "Dedede";
                iGFXFiles[34] = "Lucario";
                iGFXFiles[35] = "Ike";
                iGFXFiles[36] = "Rob";
                iGFXFiles[37] = "37";
                iGFXFiles[38] = "Jigglypuff";
                iGFXFiles[39] = "39";
                iGFXFiles[40] = "40";
                iGFXFiles[41] = "41";
                iGFXFiles[42] = "Toon Link";
                iGFXFiles[43] = "43";
                iGFXFiles[44] = "44";
                iGFXFiles[45] = "Wolf";
                iGFXFiles[46] = "46";
                iGFXFiles[47] = "Snake";
                iGFXFiles[48] = "Sonic";
                iGFXFiles[49] = "49";
                iGFXFiles[50] = "50";
                iGFXFiles[51] = "51";
                iGFXFiles[52] = "52";
                iGFXFiles[53] = "53";
                iGFXFiles[54] = "54";
                iGFXFiles[55] = "55";
                iGFXFiles[56] = "56";
                iGFXFiles[57] = "57";
                iGFXFiles[58] = "58";
                iGFXFiles[59] = "59";
                iGFXFiles[60] = "60";
                iGFXFiles[61] = "61";
                iGFXFiles[62] = "62";
                iGFXFiles[63] = "63";
                iGFXFiles[64] = "64";
                iGFXFiles[65] = "65";
                iGFXFiles[66] = "66";
                iGFXFiles[67] = "67";
                iGFXFiles[68] = "68";
                iGFXFiles[69] = "69";
                iGFXFiles[70] = "70";
                iGFXFiles[71] = "71";
                iGFXFiles[72] = "72";
                iGFXFiles[73] = "73";
                iGFXFiles[74] = "74";
                iGFXFiles[75] = "75";
                iGFXFiles[76] = "76";
                iGFXFiles[77] = "77";
                iGFXFiles[78] = "78";
                iGFXFiles[79] = "79";
                iGFXFiles[80] = "80";
                iGFXFiles[81] = "81";
                iGFXFiles[82] = "82";
                iGFXFiles[83] = "83";
                iGFXFiles[84] = "84";
                iGFXFiles[85] = "85";
                iGFXFiles[86] = "86";
                iGFXFiles[87] = "87";
                iGFXFiles[88] = "88";
                iGFXFiles[89] = "89";
                iGFXFiles[90] = "90";
                iGFXFiles[91] = "91";
                iGFXFiles[92] = "92";
                iGFXFiles[93] = "93";
                iGFXFiles[94] = "94";
                iGFXFiles[95] = "95";
                iGFXFiles[96] = "96";
                iGFXFiles[97] = "97";
                iGFXFiles[98] = "98";
                iGFXFiles[99] = "99";
                iGFXFiles[100] = "100";
                iGFXFiles[101] = "101";
                iGFXFiles[102] = "102";
                iGFXFiles[103] = "103";
                iGFXFiles[104] = "104";
                iGFXFiles[105] = "105";
                iGFXFiles[106] = "106";
                iGFXFiles[107] = "107";
                iGFXFiles[108] = "108";
                iGFXFiles[109] = "109";
                iGFXFiles[110] = "110";
                iGFXFiles[111] = "111";
                iGFXFiles[112] = "112";
                iGFXFiles[113] = "113";
                iGFXFiles[114] = "114";
                iGFXFiles[115] = "115";
                iGFXFiles[116] = "116";
                iGFXFiles[117] = "117";
                iGFXFiles[118] = "118";
                iGFXFiles[119] = "119";
                iGFXFiles[120] = "120";
                iGFXFiles[121] = "121";
                iGFXFiles[122] = "122";
                iGFXFiles[123] = "123";
                iGFXFiles[124] = "124";
                iGFXFiles[125] = "125";
                iGFXFiles[126] = "126";
                iGFXFiles[127] = "127";
                iGFXFiles[128] = "128";
                iGFXFiles[129] = "129";
                iGFXFiles[130] = "130";
                iGFXFiles[131] = "131";
                iGFXFiles[132] = "132";
                iGFXFiles[133] = "133";
                iGFXFiles[134] = "134";
                iGFXFiles[135] = "135";
                iGFXFiles[136] = "136";
                iGFXFiles[137] = "137";
                iGFXFiles[138] = "138";
                iGFXFiles[139] = "139";
                iGFXFiles[140] = "140";
                iGFXFiles[141] = "141";
                iGFXFiles[142] = "142";
                iGFXFiles[143] = "143";
                iGFXFiles[144] = "144";
                iGFXFiles[145] = "145";
                iGFXFiles[146] = "146";
                iGFXFiles[147] = "147";
                iGFXFiles[148] = "148";
                iGFXFiles[149] = "149";
                iGFXFiles[150] = "150";
                iGFXFiles[151] = "151";
                iGFXFiles[152] = "152";
                iGFXFiles[153] = "153";
                iGFXFiles[154] = "154";
                iGFXFiles[155] = "155";
                iGFXFiles[156] = "156";
                iGFXFiles[157] = "157";
                iGFXFiles[158] = "158";
                iGFXFiles[159] = "159";
                iGFXFiles[160] = "160";
                iGFXFiles[161] = "161";
                iGFXFiles[162] = "162";
                iGFXFiles[163] = "163";
                iGFXFiles[164] = "164";
                iGFXFiles[165] = "165";
                iGFXFiles[166] = "166";
                iGFXFiles[167] = "167";
                iGFXFiles[168] = "168";
                iGFXFiles[169] = "169";
                iGFXFiles[170] = "170";
                iGFXFiles[171] = "171";
                iGFXFiles[172] = "172";
                iGFXFiles[173] = "173";
                iGFXFiles[174] = "174";
                iGFXFiles[175] = "175";
                iGFXFiles[176] = "176";
                iGFXFiles[177] = "177";
                iGFXFiles[178] = "178";
                iGFXFiles[179] = "179";
                iGFXFiles[180] = "180";
                iGFXFiles[181] = "181";
                iGFXFiles[182] = "182";
                iGFXFiles[183] = "183";
                iGFXFiles[184] = "184";
                iGFXFiles[185] = "185";
                iGFXFiles[186] = "186";
                iGFXFiles[187] = "187";
                iGFXFiles[188] = "188";
                iGFXFiles[189] = "189";
                iGFXFiles[190] = "190";
                iGFXFiles[191] = "191";
                iGFXFiles[192] = "192";
                iGFXFiles[193] = "193";
                iGFXFiles[194] = "194";
                iGFXFiles[195] = "195";
                iGFXFiles[196] = "196";
                iGFXFiles[197] = "197";
                iGFXFiles[198] = "198";
                iGFXFiles[199] = "Knuckle Joe";
                iGFXFiles[200] = "200";
                iGFXFiles[201] = "Mr. Resetti";
                iGFXFiles[202] = "202";
                iGFXFiles[203] = "Gray Fox";
                iGFXFiles[204] = "Shadow";
                iGFXFiles[205] = "205";
                iGFXFiles[206] = "Devil";
                iGFXFiles[207] = "207";
                iGFXFiles[208] = "Metroid";
                iGFXFiles[209] = "209";
                iGFXFiles[210] = "210";
                iGFXFiles[211] = "Starfy";
                iGFXFiles[212] = "Tingle";
                iGFXFiles[213] = "Kat & Ana";
                iGFXFiles[214] = "Lyn";
                iGFXFiles[215] = "Custom Robo";
                iGFXFiles[216] = "Little Mac";
                iGFXFiles[217] = "Soldier";
                iGFXFiles[218] = "Jeff";
                iGFXFiles[219] = "219";
                iGFXFiles[220] = "Barbara";
                iGFXFiles[221] = "Issac";
                iGFXFiles[222] = "Saki";
                iGFXFiles[223] = "Kururi";
                iGFXFiles[224] = "Mario FS";
                iGFXFiles[225] = "DK FS";
                iGFXFiles[226] = "Link FS";
                iGFXFiles[227] = "Toon Link FS";
                iGFXFiles[228] = "Samus FS";
                iGFXFiles[229] = "Yoshi FS";
                iGFXFiles[230] = "Kirby FS";
                iGFXFiles[231] = "Fox/Falco FS";
                iGFXFiles[232] = "Pikachu FS?";
                iGFXFiles[233] = "Luigi FS?";
                iGFXFiles[234] = "Falcon FS?";
                iGFXFiles[235] = "Ness FS";
                iGFXFiles[236] = "Bowser FS";
                iGFXFiles[237] = "Peach FS";
                iGFXFiles[238] = "Zelda/Sheik FS";
                iGFXFiles[239] = "Popo/Nana FS";
                iGFXFiles[240] = "Marth FS";
                iGFXFiles[241] = "241";
                iGFXFiles[242] = "Ganondorf FS";
                iGFXFiles[243] = "Wario FS";
                iGFXFiles[244] = "Meta Knight FS";
                iGFXFiles[245] = "Pit FS";
                iGFXFiles[246] = "Zerosuit Samus FS";
                iGFXFiles[247] = "Olimar's FS?";
                iGFXFiles[248] = "Lucas FS";
                iGFXFiles[249] = "Diddy FS";
                iGFXFiles[250] = "Pokemon Trainer FS";
                iGFXFiles[251] = "Dedede FS?";
                iGFXFiles[252] = "Lucario FS";
                iGFXFiles[253] = "Ike FS";
                iGFXFiles[254] = "Rob FS?";
                iGFXFiles[255] = "Jigglypuff FS";
                iGFXFiles[256] = "Wolf FS?";
                iGFXFiles[257] = "Snake FS";
                iGFXFiles[258] = "Sonic FS";
            }

            //string s = "iGFXFiles = new string[" + iGFXFiles.Length + "];";
            //string e = "";
            //int x = 0;
            //foreach (string v in iGFXFiles)
            //    e += "\niGFXFiles[" + x++ + "] = \"" + v + "\";";
            //Console.WriteLine(s + e);

            //string s = "iRequirements = new string[" + iRequirements.Length + "];";
            //string e = "";
            //int x = 0;
            //foreach (string v in iRequirements)
            //    e += "\niRequirements[" + x++ + "] = \"" + v + "\";";
            //Console.WriteLine(s + e);

            //s = "iAirGroundStats = new string[" + iAirGroundStats.Length + "];";
            //e = "";
            //x = 0;
            //foreach (string v in iAirGroundStats)
            //    e += "\niAirGroundStats[" + x++ + "] = \"" + v + "\";";
            //Console.WriteLine(s + e);

            //s = "iCollisionStats = new string[" + iCollisionStats.Length + "];";
            //e = "";
            //x = 0;
            //foreach (string v in iCollisionStats)
            //    e += "\niCollisionStats[" + x++ + "] = \"" + v + "\";";
            //Console.WriteLine(s + e);

            AttributeArray = new AttributeInfo[185];
            sr = null;
            loc = Application.StartupPath + "/MovesetData/Attributes.txt";

            //Read known attributes and their descriptions.
            if (File.Exists(loc))
            {
                using (sr = new StreamReader(loc))
                {
                    for (int i = 0; !sr.EndOfStream && i < 185; i++)
                    {
                        AttributeArray[i] = new AttributeInfo();
                        AttributeArray[i]._name = sr.ReadLine();
                        AttributeArray[i]._description = sr.ReadLine();
                        AttributeArray[i]._type = int.Parse(sr.ReadLine());

                        if (AttributeArray[i]._description == "")
                        {
                            AttributeArray[i]._description = "No Description Available.";
                        }

                        sr.ReadLine();
                    }
                }
            }
            else
            {
                AttributeArray = new AttributeInfo[185];
                AttributeArray[0] = new AttributeInfo()
                {
                    _name = "0x000 Walk Initial Velocity",
                    _description = "The starting velocity obtained the moment the character starts walking.", _type = 0
                };
                AttributeArray[1] = new AttributeInfo()
                {
                    _name = "0x004 Walk Acceleration", _description = "The speed of acceleration while walking.",
                    _type = 0
                };
                AttributeArray[2] = new AttributeInfo()
                {
                    _name = "0x008 Walk Maximum Velocity",
                    _description = "The maximum velocity obtainable while walking.", _type = 0
                };
                AttributeArray[3] = new AttributeInfo()
                {
                    _name = "0x00C Stopping Velocity",
                    _description = "The speed at which the character is able to stop at.", _type = 0
                };
                AttributeArray[4] = new AttributeInfo()
                {
                    _name = "0x010 Dash & StopTurn Initial Velocity",
                    _description = "The starting velocity obtained the moment the character starts a Dash.", _type = 0
                };
                AttributeArray[5] = new AttributeInfo()
                {
                    _name = "0x014 StopTurn Deceleration",
                    _description = "The speed at which the character decelerates upon performing a StopTurn.", _type = 0
                };
                AttributeArray[6] = new AttributeInfo()
                {
                    _name = "0x018 StopTurn Acceleration",
                    _description = "The speed at which the character accelerates after performing a StopTurn", _type = 0
                };
                AttributeArray[7] = new AttributeInfo()
                {
                    _name = "0x01C Run Initial Velocity",
                    _description = "The starting velocity obtained the moment the Dash turns into a Run.", _type = 0
                };
                AttributeArray[8] = new AttributeInfo()
                {
                    _name = "0x020 Run Acceleration?",
                    _description = "Possibly the time it takes for the character to reach full speed during a run?",
                    _type = 0
                };
                AttributeArray[9] = new AttributeInfo()
                    {_name = "0x024", _description = "No Description Available.", _type = 0};
                AttributeArray[10] = new AttributeInfo()
                {
                    _name = "*0x028 Dash Cancel Frame Window?",
                    _description = "Possibly the amount of frames you have to cancel your dash animation.", _type = 1
                };
                AttributeArray[11] = new AttributeInfo()
                {
                    _name = "0x02C Guard On Max Momentum",
                    _description = "The maximum horizontal momentum you can have when entering shield.", _type = 0
                };
                AttributeArray[12] = new AttributeInfo()
                {
                    _name = "*0x030 Jump Startup Time",
                    _description =
                        "The time in frames it takes for your character to life off of the ground when jumping.",
                    _type = 1
                };
                AttributeArray[13] = new AttributeInfo()
                {
                    _name = "0x034 Jump H Initial Velocity",
                    _description = "The starting horizontal velocity the character obtains when jumping.", _type = 0
                };
                AttributeArray[14] = new AttributeInfo()
                {
                    _name = "0x038 Jump V Initial Velocity",
                    _description = "The starting vertical velocity the character obtains when jumping.", _type = 0
                };
                AttributeArray[15] = new AttributeInfo()
                {
                    _name = "0x03C Ground to Air Jump Momentum Multiplier",
                    _description = "The amount of horizontal momentum from the ground that gets added onto a jump.",
                    _type = 0
                };
                AttributeArray[16] = new AttributeInfo()
                {
                    _name = "0x040 Jump H Maximum Velocity",
                    _description =
                        "The maximum horizontal velocity the character is able to obtain at the start of a jump.",
                    _type = 0
                };
                AttributeArray[17] = new AttributeInfo()
                {
                    _name = "0x044 Hop V Initial Velocity",
                    _description = "The maximum vertical velocity the character obtains when hopping.", _type = 0
                };
                AttributeArray[18] = new AttributeInfo()
                {
                    _name = "0x048 Air Jump Multiplier",
                    _description =
                        "The velocity obtained from an air jump proportional to the Jump V Initial Velocity.",
                    _type = 0
                };
                AttributeArray[19] = new AttributeInfo()
                    {_name = "0x04C Some Kind of Multiplier?", _description = "No Description Available.", _type = 0};
                AttributeArray[20] = new AttributeInfo()
                {
                    _name = "0x050 Footstool V Initial Velocity",
                    _description =
                        "The starting vertical velocity the character obtains upon performing a footstool jump.",
                    _type = 0
                };
                AttributeArray[21] = new AttributeInfo()
                    {_name = "0x054", _description = "No Description Available.", _type = 0};
                AttributeArray[22] = new AttributeInfo()
                    {_name = "0x058", _description = "No Description Available.", _type = 0};
                AttributeArray[23] = new AttributeInfo()
                    {_name = "0x05C", _description = "No Description Available.", _type = 0};
                AttributeArray[24] = new AttributeInfo()
                {
                    _name = "*0x060 Jumps",
                    _description = "The number of consecutive jumps the character is able to perform.", _type = 1
                };
                AttributeArray[25] = new AttributeInfo()
                {
                    _name = "0x064 Gravity", _description = "The speed at which the character accelerates downward.",
                    _type = 0
                };
                AttributeArray[26] = new AttributeInfo()
                {
                    _name = "0x068 Terminal Velocity", _description = "The maximum velocity obtainable due to gravity.",
                    _type = 0
                };
                AttributeArray[27] = new AttributeInfo()
                    {_name = "0x06C", _description = "No Description Available.", _type = 0};
                AttributeArray[28] = new AttributeInfo()
                    {_name = "0x070", _description = "No Description Available.", _type = 0};
                AttributeArray[29] = new AttributeInfo()
                {
                    _name = "0x074 Air Mobility",
                    _description = "The speed at which the character is able to maneuver in air.", _type = 0
                };
                AttributeArray[30] = new AttributeInfo()
                {
                    _name = "0x078 Air Stopping Mobility",
                    _description = "The speed at which the character is able to stop themselves in air.", _type = 0
                };
                AttributeArray[31] = new AttributeInfo()
                {
                    _name = "0x07C Maximum H Air Velocity",
                    _description = "The maximum horizontal velocity the character is able to obtain in air.", _type = 0
                };
                AttributeArray[32] = new AttributeInfo()
                {
                    _name = "0x080 Horizontal Momentum Decay",
                    _description = "The speed at which the character's horizontal momentum decreases on its own.",
                    _type = 0
                };
                AttributeArray[33] = new AttributeInfo()
                {
                    _name = "0x084 Fastfall Terminal Velocity",
                    _description = "The initial fastfalling speed of the character. ", _type = 0
                };
                AttributeArray[34] = new AttributeInfo()
                    {_name = "0x088", _description = "No Description Available.", _type = 0};
                AttributeArray[35] = new AttributeInfo()
                {
                    _name = "*0x08C Glide Frame Window",
                    _description =
                        "The amount of time (in frames) that the character has to begin a glide after jumping. Leave at 0 for no glide.",
                    _type = 1
                };
                AttributeArray[36] = new AttributeInfo()
                    {_name = "0x090", _description = "No Description Available.", _type = 0};
                AttributeArray[37] = new AttributeInfo()
                    {_name = "0x094", _description = "No Description Available.", _type = 0};
                AttributeArray[38] = new AttributeInfo()
                    {_name = "0x098", _description = "No Description Available.", _type = 0};
                AttributeArray[39] = new AttributeInfo()
                    {_name = "*0x09C Forward Tilt 2 Flag", _description = "?", _type = 1};
                AttributeArray[40] = new AttributeInfo()
                    {_name = "*0x0A0 Forward Tilt 3 Flag", _description = "?", _type = 1};
                AttributeArray[41] = new AttributeInfo()
                    {_name = "0x0A4 Forward Smash 2 Flag", _description = "No Description Available.", _type = 0};
                AttributeArray[42] = new AttributeInfo()
                    {_name = "0x0A8", _description = "No Description Available.", _type = 0};
                AttributeArray[43] = new AttributeInfo()
                    {_name = "0x0AC", _description = "No Description Available.", _type = 0};
                AttributeArray[44] = new AttributeInfo()
                    {_name = "0x0B0 Weight", _description = "How resiliant the character is to knockback.", _type = 0};
                AttributeArray[45] = new AttributeInfo()
                {
                    _name = "0x0B4 Size",
                    _description = "The scaling of the character from their original model size. 1 = 100%, 0.5 = 50%.",
                    _type = 0
                };
                AttributeArray[46] = new AttributeInfo()
                {
                    _name = "0x0B8 Size on Results Screen",
                    _description = "The scaling of the character on the results screen. 1 = 100%, 0.5 = 50%.", _type = 0
                };
                AttributeArray[47] = new AttributeInfo()
                    {_name = "0x0BC", _description = "No Description Available.", _type = 0};
                AttributeArray[48] = new AttributeInfo()
                    {_name = "0x0C0", _description = "No Description Available.", _type = 0};
                AttributeArray[49] = new AttributeInfo()
                {
                    _name = "0x0C4 Shield Size",
                    _description = "The size of the character's shield bubble when it is at full strength.", _type = 0
                };
                AttributeArray[50] = new AttributeInfo()
                {
                    _name = "0x0C8 Shield Break Bounce Velocity",
                    _description =
                        "The velocity at which the character bounces upwards upon having their shield broken.",
                    _type = 0
                };
                AttributeArray[51] = new AttributeInfo()
                    {_name = "0x0CC", _description = "No Description Available.", _type = 0};
                AttributeArray[52] = new AttributeInfo()
                    {_name = "0x0D0", _description = "No Description Available.", _type = 0};
                AttributeArray[53] = new AttributeInfo()
                    {_name = "0x0D4", _description = "No Description Available.", _type = 0};
                AttributeArray[54] = new AttributeInfo()
                    {_name = "0x0D8", _description = "No Description Available.", _type = 0};
                AttributeArray[55] = new AttributeInfo()
                    {_name = "0x0DC", _description = "No Description Available.", _type = 0};
                AttributeArray[56] = new AttributeInfo()
                    {_name = "0x0E0", _description = "No Description Available.", _type = 0};
                AttributeArray[57] = new AttributeInfo()
                    {_name = "0x0E4", _description = "No Description Available.", _type = 0};
                AttributeArray[58] = new AttributeInfo()
                    {_name = "*0x0E8", _description = "No Description Available.", _type = 1};
                AttributeArray[59] = new AttributeInfo()
                    {_name = "*0x0EC", _description = "No Description Available.", _type = 1};
                AttributeArray[60] = new AttributeInfo()
                    {_name = "*0x0F0", _description = "No Description Available.", _type = 1};
                AttributeArray[61] = new AttributeInfo()
                    {_name = "0x0F4", _description = "No Description Available.", _type = 0};
                AttributeArray[62] = new AttributeInfo()
                {
                    _name = "0x0F8 Edge Jump H Velocity",
                    _description = "The starting horizontal velocity obtained from an Edge Jump.", _type = 0
                };
                AttributeArray[63] = new AttributeInfo()
                {
                    _name = "0x0FC Edge Jump V Velocity",
                    _description = "The starting vertical velocity obtained from an Edge Jump.", _type = 0
                };
                AttributeArray[64] = new AttributeInfo()
                    {_name = "0x100", _description = "No Description Available.", _type = 0};
                AttributeArray[65] = new AttributeInfo()
                    {_name = "0x104", _description = "No Description Available.", _type = 0};
                AttributeArray[66] = new AttributeInfo()
                    {_name = "0x108", _description = "No Description Available.", _type = 0};
                AttributeArray[67] = new AttributeInfo()
                    {_name = "0x10C", _description = "No Description Available.", _type = 0};
                AttributeArray[68] = new AttributeInfo()
                    {_name = "0x110", _description = "No Description Available.", _type = 0};
                AttributeArray[69] = new AttributeInfo()
                    {_name = "*0x114", _description = "No Description Available.", _type = 1};
                AttributeArray[70] = new AttributeInfo()
                {
                    _name = "0x118 Item Throw Strength",
                    _description = "The speed at which an item is projected when thrown.", _type = 0
                };
                AttributeArray[71] = new AttributeInfo()
                    {_name = "0x11C", _description = "No Description Available.", _type = 0};
                AttributeArray[72] = new AttributeInfo()
                    {_name = "0x120", _description = "No Description Available.", _type = 0};
                AttributeArray[73] = new AttributeInfo()
                    {_name = "0x124", _description = "No Description Available.", _type = 0};
                AttributeArray[74] = new AttributeInfo()
                {
                    _name = "0x128 Projectile Weapon Use Move Speed",
                    _description =
                        "The speed at which the character is able to move at while firing a projectile weapon.",
                    _type = 0
                };
                AttributeArray[75] = new AttributeInfo()
                {
                    _name = "0x12C Projectile Weapon Use F Dash Speed",
                    _description =
                        "The speed at which a character moves during a forward dash while firing a projectile weapon.",
                    _type = 0
                };
                AttributeArray[76] = new AttributeInfo()
                {
                    _name = "0x130 Projectile Weapon Use B Dash Speed",
                    _description =
                        "The speed at which a character moves during a backward dash while firing a projectile weapon.",
                    _type = 0
                };
                AttributeArray[77] = new AttributeInfo()
                    {_name = "0x134", _description = "No Description Available.", _type = 0};
                AttributeArray[78] = new AttributeInfo()
                    {_name = "0x138", _description = "No Description Available.", _type = 0};
                AttributeArray[79] = new AttributeInfo()
                {
                    _name = "0x13C Normal Landing Lag",
                    _description = "The length of the character's normal landing lag in frames.", _type = 0
                };
                AttributeArray[80] = new AttributeInfo()
                {
                    _name = "0x140 Nair Landing Lag?",
                    _description = "The length of the character's nair landing lag animation in frames.", _type = 0
                };
                AttributeArray[81] = new AttributeInfo()
                {
                    _name = "0x144 Fair Landing Lag?",
                    _description = "The length of the character's fair landing lag animation in frames.", _type = 0
                };
                AttributeArray[82] = new AttributeInfo()
                {
                    _name = "0x148 Bair Landing Lag?",
                    _description = "The length of the character's bair landing lag animation in frames.", _type = 0
                };
                AttributeArray[83] = new AttributeInfo()
                {
                    _name = "0x14C Uair Landing Lag?",
                    _description = "The length of the character's uair landing lag animation in frames.", _type = 0
                };
                AttributeArray[84] = new AttributeInfo()
                {
                    _name = "0x150 Dair Landing Lag",
                    _description = "The length of the character's dair landing lag animation in frames.", _type = 0
                };
                AttributeArray[85] = new AttributeInfo()
                    {_name = "*0x154", _description = "No Description Available.", _type = 1};
                AttributeArray[86] = new AttributeInfo()
                    {_name = "*0x158", _description = "No Description Available.", _type = 1};
                AttributeArray[87] = new AttributeInfo()
                    {_name = "0x15C", _description = "No Description Available.", _type = 0};
                AttributeArray[88] = new AttributeInfo()
                    {_name = "0x160", _description = "No Description Available.", _type = 0};
                AttributeArray[89] = new AttributeInfo()
                {
                    _name = "0x164 Walljump H Velocity",
                    _description = "The starting horazontal velocity obtained from a Walljump.", _type = 0
                };
                AttributeArray[90] = new AttributeInfo()
                {
                    _name = "0x168 Walljump V Velocity",
                    _description = "The starting vertical velocity obtained from a Walljump.", _type = 0
                };
                AttributeArray[91] = new AttributeInfo()
                    {_name = "0x16C", _description = "No Description Available.", _type = 0};
                AttributeArray[92] = new AttributeInfo()
                    {_name = "0x170", _description = "No Description Available.", _type = 0};
                AttributeArray[93] = new AttributeInfo()
                    {_name = "*0x174", _description = "No Description Available.", _type = 1};
                AttributeArray[94] = new AttributeInfo()
                    {_name = "0x178", _description = "No Description Available.", _type = 0};
                AttributeArray[95] = new AttributeInfo()
                    {_name = "0x17C", _description = "No Description Available.", _type = 0};
                AttributeArray[96] = new AttributeInfo()
                    {_name = "*0x180", _description = "No Description Available.", _type = 1};
                AttributeArray[97] = new AttributeInfo()
                    {_name = "*0x184", _description = "No Description Available.", _type = 1};
                AttributeArray[98] = new AttributeInfo()
                    {_name = "0x188", _description = "No Description Available.", _type = 0};
                AttributeArray[99] = new AttributeInfo()
                    {_name = "0x18C", _description = "No Description Available.", _type = 0};
                AttributeArray[100] = new AttributeInfo()
                    {_name = "0x190", _description = "No Description Available.", _type = 0};
                AttributeArray[101] = new AttributeInfo()
                    {_name = "0x194", _description = "No Description Available.", _type = 0};
                AttributeArray[102] = new AttributeInfo()
                    {_name = "0x198", _description = "No Description Available.", _type = 0};
                AttributeArray[103] = new AttributeInfo()
                    {_name = "0x19C", _description = "No Description Available.", _type = 0};
                AttributeArray[104] = new AttributeInfo()
                    {_name = "0x1A0", _description = "No Description Available.", _type = 0};
                AttributeArray[105] = new AttributeInfo()
                    {_name = "0x1A4", _description = "No Description Available.", _type = 0};
                AttributeArray[106] = new AttributeInfo()
                    {_name = "*0x1A8", _description = "No Description Available.", _type = 1};
                AttributeArray[107] = new AttributeInfo()
                    {_name = "0x1AC", _description = "No Description Available.", _type = 0};
                AttributeArray[108] = new AttributeInfo()
                    {_name = "*0x1B0", _description = "No Description Available.", _type = 1};
                AttributeArray[109] = new AttributeInfo()
                    {_name = "0x1B4", _description = "No Description Available.", _type = 0};
                AttributeArray[110] = new AttributeInfo()
                    {_name = "*0x1B8", _description = "No Description Available.", _type = 1};
                AttributeArray[111] = new AttributeInfo()
                    {_name = "*0x1BC", _description = "No Description Available.", _type = 1};
                AttributeArray[112] = new AttributeInfo()
                    {_name = "0x1C0", _description = "No Description Available.", _type = 0};
                AttributeArray[113] = new AttributeInfo()
                    {_name = "*0x1C4", _description = "No Description Available.", _type = 1};
                AttributeArray[114] = new AttributeInfo()
                    {_name = "0x1C8", _description = "No Description Available.", _type = 0};
                AttributeArray[115] = new AttributeInfo()
                    {_name = "*0x1CC", _description = "No Description Available.", _type = 1};
                AttributeArray[116] = new AttributeInfo()
                    {_name = "*0x1D0", _description = "No Description Available.", _type = 1};
                AttributeArray[117] = new AttributeInfo()
                    {_name = "*0x1D4", _description = "No Description Available.", _type = 1};
                AttributeArray[118] = new AttributeInfo()
                    {_name = "*0x1D8", _description = "No Description Available.", _type = 1};
                AttributeArray[119] = new AttributeInfo()
                    {_name = "0x1DC", _description = "No Description Available.", _type = 0};
                AttributeArray[120] = new AttributeInfo()
                    {_name = "*0x1E0", _description = "No Description Available.", _type = 1};
                AttributeArray[121] = new AttributeInfo()
                    {_name = "0x1E4", _description = "No Description Available.", _type = 0};
                AttributeArray[122] = new AttributeInfo()
                    {_name = "0x1E8", _description = "No Description Available.", _type = 0};
                AttributeArray[123] = new AttributeInfo()
                    {_name = "0x1EC", _description = "No Description Available.", _type = 0};
                AttributeArray[124] = new AttributeInfo()
                    {_name = "0x1F0", _description = "No Description Available.", _type = 0};
                AttributeArray[125] = new AttributeInfo()
                    {_name = "0x1F4", _description = "No Description Available.", _type = 0};
                AttributeArray[126] = new AttributeInfo()
                    {_name = "0x1F8", _description = "No Description Available.", _type = 0};
                AttributeArray[127] = new AttributeInfo()
                {
                    _name = "0x1FC Camera Size Front",
                    _description = "The camera pushing effect's distance in front of the character.", _type = 0
                };
                AttributeArray[128] = new AttributeInfo()
                {
                    _name = "0x200 Camera Size Back",
                    _description = "The camera pushing effect's distance behind the character.", _type = 0
                };
                AttributeArray[129] = new AttributeInfo()
                {
                    _name = "0x204 Camera Size Top",
                    _description = "The camera pushing effect's distance above the character.", _type = 0
                };
                AttributeArray[130] = new AttributeInfo()
                {
                    _name = "0x208 Camera Size Bottom",
                    _description = "The camera pushing effect's distance below the character.", _type = 0
                };
                AttributeArray[131] = new AttributeInfo()
                    {_name = "0x20C Opposite of previous?", _description = "?", _type = 0};
                AttributeArray[132] = new AttributeInfo()
                {
                    _name = "0x210 Zoom Camera Size Front",
                    _description = "The zoom boundaries on the character in the front.", _type = 0
                };
                AttributeArray[133] = new AttributeInfo()
                {
                    _name = "0x214 Zoom Camera Size Back",
                    _description = "The zoom boundaries on the character in the back.", _type = 0
                };
                AttributeArray[134] = new AttributeInfo()
                {
                    _name = "0x218 Zoom Camera Size Top",
                    _description = "The zoom boundaries on the character on the top.", _type = 0
                };
                AttributeArray[135] = new AttributeInfo()
                {
                    _name = "0x21C Zoom Camera Size Bottom",
                    _description = "The zoom boundaries on the character on the bottom.", _type = 0
                };
                AttributeArray[136] = new AttributeInfo()
                    {_name = "*0x220", _description = "No Description Available.", _type = 1};
                AttributeArray[137] = new AttributeInfo()
                    {_name = "0x224", _description = "No Description Available.", _type = 0};
                AttributeArray[138] = new AttributeInfo()
                    {_name = "0x228", _description = "No Description Available.", _type = 0};
                AttributeArray[139] = new AttributeInfo()
                    {_name = "0x22C", _description = "No Description Available.", _type = 0};
                AttributeArray[140] = new AttributeInfo()
                    {_name = "0x230", _description = "No Description Available.", _type = 0};
                AttributeArray[141] = new AttributeInfo()
                    {_name = "*0x234", _description = "No Description Available.", _type = 1};
                AttributeArray[142] = new AttributeInfo()
                    {_name = "0x238", _description = "No Description Available.", _type = 0};
                AttributeArray[143] = new AttributeInfo()
                    {_name = "0x23C", _description = "No Description Available.", _type = 0};
                AttributeArray[144] = new AttributeInfo()
                    {_name = "0x240", _description = "No Description Available.", _type = 0};
                AttributeArray[145] = new AttributeInfo()
                {
                    _name = "0x244 Magnifying Glass Shrink Ratio",
                    _description = "The size of the character in the magifying glass.", _type = 0
                };
                AttributeArray[146] = new AttributeInfo()
                    {_name = "0x248", _description = "No Description Available.", _type = 0};
                AttributeArray[147] = new AttributeInfo()
                    {_name = "0x24C", _description = "No Description Available.", _type = 0};
                AttributeArray[148] = new AttributeInfo()
                    {_name = "0x250", _description = "No Description Available.", _type = 0};
                AttributeArray[149] = new AttributeInfo()
                    {_name = "0x254", _description = "No Description Available.", _type = 0};
                AttributeArray[150] = new AttributeInfo()
                    {_name = "0x258", _description = "No Description Available.", _type = 0};
                AttributeArray[151] = new AttributeInfo()
                    {_name = "0x25C", _description = "No Description Available.", _type = 0};
                AttributeArray[152] = new AttributeInfo()
                    {_name = "0x260", _description = "No Description Available.", _type = 0};
                AttributeArray[153] = new AttributeInfo()
                    {_name = "0x264", _description = "No Description Available.", _type = 0};
                AttributeArray[154] = new AttributeInfo()
                    {_name = "0x268", _description = "No Description Available.", _type = 0};
                AttributeArray[155] = new AttributeInfo()
                    {_name = "0x26C", _description = "No Description Available.", _type = 0};
                AttributeArray[156] = new AttributeInfo()
                    {_name = "0x270", _description = "No Description Available.", _type = 0};
                AttributeArray[157] = new AttributeInfo()
                    {_name = "0x274", _description = "No Description Available.", _type = 0};
                AttributeArray[158] = new AttributeInfo()
                    {_name = "0x278", _description = "No Description Available.", _type = 0};
                AttributeArray[159] = new AttributeInfo()
                    {_name = "0x27C", _description = "No Description Available.", _type = 0};
                AttributeArray[160] = new AttributeInfo()
                    {_name = "0x280", _description = "No Description Available.", _type = 0};
                AttributeArray[161] = new AttributeInfo()
                    {_name = "0x284", _description = "No Description Available.", _type = 0};
                AttributeArray[162] = new AttributeInfo()
                    {_name = "0x288", _description = "No Description Available.", _type = 0};
                AttributeArray[163] = new AttributeInfo()
                    {_name = "*0x28C", _description = "No Description Available.", _type = 1};
                AttributeArray[164] = new AttributeInfo()
                    {_name = "*0x290", _description = "No Description Available.", _type = 1};
                AttributeArray[165] = new AttributeInfo()
                    {_name = "*0x294", _description = "No Description Available.", _type = 1};
                AttributeArray[166] = new AttributeInfo()
                    {_name = "0x298", _description = "No Description Available.", _type = 0};
                AttributeArray[167] = new AttributeInfo()
                    {_name = "*0x29C", _description = "No Description Available.", _type = 1};
                AttributeArray[168] = new AttributeInfo()
                    {_name = "0x2A0", _description = "No Description Available.", _type = 0};
                AttributeArray[169] = new AttributeInfo()
                    {_name = "0x2A4", _description = "No Description Available.", _type = 0};
                AttributeArray[170] = new AttributeInfo()
                    {_name = "0x2A8", _description = "No Description Available.", _type = 0};
                AttributeArray[171] = new AttributeInfo()
                    {_name = "0x2AC", _description = "No Description Available.", _type = 0};
                AttributeArray[172] = new AttributeInfo()
                    {_name = "0x2B0", _description = "No Description Available.", _type = 0};
                AttributeArray[173] = new AttributeInfo()
                    {_name = "0x2B4", _description = "No Description Available.", _type = 0};
                AttributeArray[174] = new AttributeInfo()
                    {_name = "0x2B8", _description = "No Description Available.", _type = 0};
                AttributeArray[175] = new AttributeInfo()
                    {_name = "0x2BC", _description = "No Description Available.", _type = 0};
                AttributeArray[176] = new AttributeInfo()
                    {_name = "*0x2C0", _description = "No Description Available.", _type = 1};
                AttributeArray[177] = new AttributeInfo()
                    {_name = "*0x2C4", _description = "No Description Available.", _type = 1};
                AttributeArray[178] = new AttributeInfo()
                    {_name = "*0x2C8", _description = "No Description Available.", _type = 1};
                AttributeArray[179] = new AttributeInfo()
                    {_name = "*0x2CC", _description = "No Description Available.", _type = 1};
                AttributeArray[180] = new AttributeInfo()
                    {_name = "*0x2D0", _description = "No Description Available.", _type = 1};
                AttributeArray[181] = new AttributeInfo()
                    {_name = "*0x2D4", _description = "No Description Available.", _type = 1};
                AttributeArray[182] = new AttributeInfo()
                    {_name = "*0x2D8", _description = "No Description Available.", _type = 1};
                AttributeArray[183] = new AttributeInfo()
                    {_name = "*0x2DC", _description = "No Description Available.", _type = 1};
                AttributeArray[184] = new AttributeInfo()
                    {_name = "*0x2E0", _description = "No Description Available.", _type = 1};
            }

            //string s = "AttributeArray = new AttributeInfo[185];";
            //string e = "";
            //int x = 0;
            //foreach (AttributeInfo v in AttributeArray)
            //    e += "\nAttributeArray[" + x++ + "] = new AttributeInfo() { _name = \"" + v._name + "\", _description = \"" + (v._description == "" ? "No Description Available." : v._description) + "\", _type = " + v._type.ToString().ToLower() + " };";
            //Console.WriteLine(s + e);

            Params = new Dictionary<string, SectionParamInfo>();
            sr = null;
            loc = Application.StartupPath + "/MovesetData/CharSpecific/" + Parent.Name + ".txt";
            string name = "", attrName = "";
            if (File.Exists(loc))
            {
                using (sr = new StreamReader(loc))
                {
                    while (!sr.EndOfStream)
                    {
                        name = sr.ReadLine();
                        SectionParamInfo info = new SectionParamInfo();
                        info._newName = sr.ReadLine();
                        info._attributes = new List<AttributeInfo>();
                        while (true && !sr.EndOfStream)
                        {
                            if (string.IsNullOrEmpty(attrName = sr.ReadLine()))
                            {
                                break;
                            }
                            else
                            {
                                AttributeInfo i = new AttributeInfo();
                                i._name = attrName;
                                i._description = sr.ReadLine();
                                i._type = int.Parse(sr.ReadLine());
                                info._attributes.Add(i);
                                sr.ReadLine();
                            }
                        }

                        if (!Params.ContainsKey(name))
                        {
                            Params.Add(name, info);
                        }
                    }
                }
            }
        }

        #endregion

        public override void OnPopulate()
        {
            _subRoutines = new SortedDictionary<int, MoveDefActionNode>();
            _externalRefs = new List<ResourceNode>();
            _externalSections = new List<MoveDefExternalNode>();
            _lookupSizes = new Dictionary<int, MoveDefLookupOffsetNode>();
            _events = new Dictionary<uint, List<MoveDefEventNode>>();
            StatusIDs = new bool[0];

            LoadEventDictionary();
            LoadOtherData();

            //Parse references first but don't add to children yet
            if (numExternalSubRoutine > 0)
            {
                (references = new MoveDefReferenceNode(Header->StringTable) {_parent = this}).Initialize(this,
                    new DataSource(Header->ExternalSubRoutines, numExternalSubRoutine * 8));
                _externalRefs = references.Children;
            }

            (sections = new MoveDefSectionNode(Header->_fileSize, (VoidPtr) Header->StringTable)).Initialize(this,
                new DataSource(Header->DataTable, Header->_dataTableEntryCount * 8));
            (lookupNode = new MoveDefLookupNode(Header->_lookupEntryCount) {_parent = this}).Initialize(this,
                new DataSource(Header->LookupEntries, Header->_lookupEntryCount * 4));

            //Now add to children
            if (references != null)
            {
                Children.Add(references);
            }

            MoveDefSubRoutineListNode g = new MoveDefSubRoutineListNode() {_name = "SubRoutines", _parent = this};

            _subRoutineGroup = g;
            _subRoutineList = g.Children;

            //Load subroutines
            //if (!RootNode._origPath.Contains("Test"))
            {
                sections.Populate();
                foreach (MoveDefEntryNode p in sections._sectionList)
                {
                    if (p is MoveDefExternalNode && (p as MoveDefExternalNode)._refs.Count == 0)
                    {
                        sections.Children.Add(p);
                    }
                }
            }
            g._name = "[" + g.Children.Count + "] " + g._name;

            _children.Add(g);

            _children.Sort(MoveDefEntryNode.Compare);

            _children[0]._children.Sort(MoveDefEntryNode.Compare);
            for (int i = 0; i < _children[0]._children.Count; i++)
            {
                _children[0]._children[i]._name = "SubRoutine" + i;
            }

            int x = 0;
            {
                foreach (MoveDefActionNode i in _subRoutines.Values)
                {
                    i._name = "SubRoutine" + x;
                    foreach (MoveDefEventNode e in i._actionRefs)
                    {
                        if (e.EventID == 218104320)
                        {
                            if (e.Children[1] is MoveDefEventOffsetNode mdo)
                            {
                                mdo.index = x;
                            }
                        }
                        else
                        {
                            if (e.Children[0] is MoveDefEventOffsetNode mdo)
                            {
                                mdo.index = x;
                            }
                        }
                    }

                    x++;
                }
            }

            //for (int i = 0; i < lookupNode.Children.Count; i++)
            //    if ((lookupNode.Children[i] as MoveDefLookupOffsetNode).remove)
            //        lookupNode.Children[i--].Remove();

            //foreach (var i in Paths)
            //    Console.WriteLine(i.ToString());
        }

        public List<MoveDefEntryNode> _postProcessNodes;
        public VoidPtr _rebuildBase;
        public static LookupManager _lookupOffsets;
        public int lookupCount, lookupLen;

        public override int OnCalculateSize(bool force)
        {
            int size = 0x20;
            _postProcessNodes = new List<MoveDefEntryNode>();
            _lookupOffsets = new LookupManager();
            lookupCount = 0;
            lookupLen = 0;
            refTable = new CompactStringTable();
            foreach (MoveDefEntryNode e in sections._sectionList)
            {
                e._lookupCount = 0;
                if (e is MoveDefExternalNode)
                {
                    MoveDefExternalNode ext = e as MoveDefExternalNode;
                    if (ext._refs.Count > 0)
                    {
                        MoveDefEntryNode entry = ext._refs[0];

                        if ((entry.Parent is MoveDefDataNode || entry.Parent is MoveDefMiscNode) && !entry.isExtra)
                        {
                            lookupCount++;
                        }

                        if (!(entry is MoveDefRawDataNode))
                        {
                            entry.CalculateSize(true);
                        }
                        else if (entry.Children.Count > 0)
                        {
                            int off = 0;
                            foreach (MoveDefEntryNode n in entry.Children)
                            {
                                off += n.CalculateSize(true);
                                entry._lookupCount += n._lookupCount;
                            }

                            entry._entryLength = entry._calcSize = off;
                        }
                        else
                        {
                            entry.CalculateSize(true);
                        }

                        e._lookupCount = entry._lookupCount;
                        e._childLength = entry._childLength;
                        e._entryLength = entry._entryLength;
                        e._calcSize = entry._calcSize;
                    }
                    else
                    {
                        e.CalculateSize(true);
                    }
                }
                else
                {
                    e.CalculateSize(true);
                }

                size += (e._calcSize == 0 ? e._childLength + e._entryLength : e._calcSize) + 8;
                lookupCount += e._lookupCount;
                refTable.Add(e.Name);
            }

            refCount = 0;
            if (references != null)
            {
                foreach (MoveDefExternalNode e in references.Children)
                {
                    if (e._refs.Count > 0)
                    {
                        refTable.Add(e.Name);
                        size += 8;
                        refCount++;
                    }

                    //references don't use lookup table
                    //lookupCount += e._refs.Count - 1;
                }
            }

            return size + (lookupLen = lookupCount * 4) + refTable.TotalSize;
        }

        private int refCount;

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            //Children are built in order but before their parent! 

            _rebuildBase = address + 0x20;

            FDefHeader* header = (FDefHeader*) address;
            header->_fileSize = length;
            header->_externalSubRoutineCount = refCount;
            header->_dataTableEntryCount = sections._sectionList.Count;
            header->_lookupEntryCount = lookupCount;
            header->_pad1 = header->_pad2 = header->_pad3 = 0;

            VoidPtr dataAddress = _rebuildBase;

            int lookupOffset = 0, sectionsOffset = 0;
            foreach (MoveDefEntryNode e in sections._sectionList)
            {
                lookupOffset += e._calcSize == 0 ? e._childLength + e._entryLength : e._calcSize;
                sectionsOffset += e._childLength;
            }

            VoidPtr lookupAddress = dataAddress + lookupOffset;
            VoidPtr sectionsAddr = dataAddress + sectionsOffset;
            VoidPtr dataHeaderAddr = dataAddress;

            foreach (MoveDefEntryNode e in sections._sectionList)
            {
                e._lookupOffsets.Clear();
                if (e.Name == "data" || e.Name == "dataCommon")
                {
                    dataHeaderAddr = sectionsAddr; //Don't rebuild yet
                    sectionsAddr += e._entryLength;
                }
                else //Rebuild other sections first
                {
                    if (e is MoveDefExternalNode)
                    {
                        MoveDefExternalNode ext = e as MoveDefExternalNode;
                        if (ext._refs.Count > 0)
                        {
                            MoveDefEntryNode entry = ext._refs[0];

                            if (!(entry is MoveDefRawDataNode))
                            {
                                entry.Rebuild(sectionsAddr, entry._calcSize, true);
                            }
                            else if (entry.Children.Count > 0)
                            {
                                entry._entryOffset = sectionsAddr;
                                int off = 0;
                                foreach (MoveDefEntryNode n in entry.Children)
                                {
                                    n.Rebuild(sectionsAddr + off, n._calcSize, true);
                                    off += n._calcSize;
                                    entry._lookupOffsets.AddRange(n._lookupOffsets);
                                }
                            }
                            else
                            {
                                entry.Rebuild(sectionsAddr, entry._calcSize, true);
                            }

                            e._entryOffset = entry._entryOffset;
                            e._lookupOffsets = entry._lookupOffsets;
                        }
                        else
                        {
                            e.Rebuild(sectionsAddr, e._calcSize, true);
                        }
                    }
                    else
                    {
                        e.Rebuild(sectionsAddr, e._calcSize, true);
                    }

                    if (e._lookupCount != e._lookupOffsets.Count &&
                        !((e as MoveDefExternalNode)._refs[0] is MoveDefActionNode))
                    {
                        Console.WriteLine();
                    }

                    _lookupOffsets.AddRange(e._lookupOffsets.ToArray());
                    sectionsAddr += e._calcSize;
                }
            }

            if (data != null)
            {
                data.dataHeaderAddr = dataHeaderAddr;
                data.Rebuild(address + 0x20, data._childLength, true);
            }
            else if (dataCommon != null)
            {
                dataCommon.dataHeaderAddr = dataHeaderAddr;
                dataCommon.Rebuild(address + 0x20, dataCommon._childLength, true);
            }

            foreach (MoveDefExternalNode e in references.Children)
            {
                for (int i = 0; i < e._refs.Count; i++)
                {
                    bint* addr = (bint*) e._refs[i]._entryOffset;
                    if (i == e._refs.Count - 1)
                    {
                        *addr = -1;
                    }
                    else
                    {
                        *addr = (int) e._refs[i + 1]._entryOffset - (int) _rebuildBase;

                        //references don't use lookup table
                        //_lookupOffsets.Add((int)addr - (int)_rebuildBase);
                    }
                }
            }

            _lookupOffsets.values.Sort();

            if (lookupCount != _lookupOffsets.Count)
            {
                Console.WriteLine(lookupCount - _lookupOffsets.Count);
            }

            header->_lookupOffset = (int) lookupAddress - (int) _rebuildBase;
            header->_lookupEntryCount = _lookupOffsets.Count;

            if (data != null && data.warioSwing4StringOffset > 0 && data.warioParams6 != null)
            {
                ((WarioExtraParams6*) data.warioParams6._entryOffset)->_offset = data.warioSwing4StringOffset;
            }

            int val = -1;
            if (data != null && data.zssFirstOffset > 0)
            {
                val = data.zssFirstOffset;
            }

            bint* values = (bint*) lookupAddress;
            foreach (int i in _lookupOffsets.values)
            {
                if (val == i && data != null && data.zssParams8 != null)
                {
                    *(bint*) data.zssParams8._entryOffset = 29;
                    *((bint*) data.zssParams8._entryOffset + 1) = (int) values - (int) _rebuildBase;
                }

                *values++ = i;
            }

            dataAddress = (VoidPtr) values;
            VoidPtr refTableAddr = dataAddress + sections._sectionList.Count * 8 + refCount * 8;
            refTable.WriteTable(refTableAddr);

            foreach (MoveDefEntryNode e in sections._sectionList)
            {
                *values++ = (int) e._entryOffset - (int) _rebuildBase;
                *values++ = (int) refTable[e.Name] - (int) refTableAddr;
            }

            foreach (MoveDefExternalNode e in references.Children)
            {
                if (e._refs.Count > 0)
                {
                    *values++ = (int) e._refs[0]._entryOffset - (int) _rebuildBase;
                    *values++ = (int) refTable[e.Name] - (int) refTableAddr;
                }
            }

            //Some nodes handle rebuilding their own children, 
            //so if one of those children has changed, the node will stay dirty and may rebuild over itself.
            //Manually set IsDirty to false to avoid that.
            IsDirty = false;

            BaseAddress = _rebuildBase;
        }
    }

    public class LookupManager
    {
        public List<int> values = new List<int>();
        public int Count => values.Count;

        public void Add(int value)
        {
            if (value > 0 && !values.Contains(value))
            {
                if (value < 1480)
                {
                    Console.WriteLine(value);
                }
                else
                {
                    values.Add(value);
                }
            }
            else
            {
                Console.WriteLine(value);
            }
        }

        public void AddRange(int[] vals)
        {
            foreach (int value in vals)
            {
                if (value > 0 && !values.Contains(value))
                {
                    if (value < 1480)
                    {
                        Console.WriteLine(value);
                    }
                    else
                    {
                        values.Add(value);
                    }
                }
                else
                {
                    Console.WriteLine(value);
                }
            }
        }
    }

    public class NameSizeGroup
    {
        public string Name;
        public int Size;

        public NameSizeGroup(string name, int size)
        {
            Name = name;
            Size = size;
        }
    }

    public unsafe class MoveDefSectionNode : MoveDefEntryNode
    {
        internal FDefStringEntry* Header => (FDefStringEntry*) WorkingUncompressed.Address;
        private Dictionary<NameSizeGroup, FDefStringEntry> DataTable = new Dictionary<NameSizeGroup, FDefStringEntry>();
        private FDefStringTable* stringTable;
        public int DataSize, dataOffset;

        public MoveDefSectionNode(int dataSize, VoidPtr table)
        {
            DataSize = dataSize;
            stringTable = (FDefStringTable*) table;
        }

        public override bool OnInitialize()
        {
            base.OnInitialize();

            _name = "Sections";

            for (int i = 0; i < WorkingUncompressed.Length / 8; i++)
            {
                DataTable.Add(new NameSizeGroup(stringTable->GetString(Header[i]._stringOffset), 0), Header[i]);
            }

            CalculateDataLen();

            foreach (KeyValuePair<NameSizeGroup, FDefStringEntry> data in DataTable)
            {
                if (data.Key.Name == "data" || data.Key.Name == "dataCommon")
                {
                    dataOffset = data.Value._dataOffset;
                }
            }

            return true;
        }

        private void CalculateDataLen()
        {
            List<KeyValuePair<NameSizeGroup, FDefStringEntry>> sorted =
                DataTable.OrderBy(x => (int) x.Value._dataOffset).ToList();
            for (int i = 0; i < sorted.Count; i++)
            {
                if (i < sorted.Count - 1)
                {
                    sorted[i].Key.Size = (int) (sorted[i + 1].Value._dataOffset - sorted[i].Value._dataOffset);
                }
                else
                {
                    sorted[i].Key.Size = (int) (((MoveDefNode) Parent).lookupOffset - sorted[i].Value._dataOffset);
                }

                //Console.WriteLine(sorted[i].ToString());
            }
        }

        public List<MoveDefEntryNode> _sectionList;

        public override void OnPopulate()
        {
            _sectionList = new List<MoveDefEntryNode>();

            int offsetID = 0;

            //Parse external offsets first
            foreach (KeyValuePair<NameSizeGroup, FDefStringEntry> data in DataTable)
            {
                if (data.Key.Name != "data" && data.Key.Name != "dataCommon" && data.Key.Name != "animParam" &&
                    data.Key.Name != "subParam")
                {
                    MoveDefRawDataNode r = new MoveDefRawDataNode(data.Key.Name) {_parent = this, offsetID = offsetID};
                    r.Initialize(this, new DataSource(BaseAddress + data.Value._dataOffset, data.Key.Size));
                    Root._externalSections.Add(r);
                    _sectionList.Add(r);
                }

                offsetID++;
            }

            offsetID = 0;

            //Now add the data node
            foreach (KeyValuePair<NameSizeGroup, FDefStringEntry> data in DataTable)
            {
                if (data.Key.Name == "data")
                {
                    (Root.data = new MoveDefDataNode((uint) DataSize, data.Key.Name) {offsetID = offsetID}).Initialize(
                        this, new DataSource(BaseAddress + data.Value._dataOffset, data.Key.Size));
                    _sectionList.Add(Root.data);
                    break;
                }
                else if (data.Key.Name == "dataCommon")
                {
                    (Root.dataCommon = new MoveDefDataCommonNode((uint) DataSize, data.Key.Name) {offsetID = offsetID})
                        .Initialize(this, new DataSource(BaseAddress + data.Value._dataOffset, data.Key.Size));
                    _sectionList.Add(Root.dataCommon);
                    break;
                }

                //else if (data.Key.Name == "animParam")
                //{
                //    (Root.animParam = new MoveDefAnimParamNode(data.Key.Name) { offsetID = offsetID }).Initialize(this, new DataSource(BaseAddress + data.Value._dataOffset, data.Key.Size));
                //    _sectionList.Add(Root.animParam);
                //}
                //else if (data.Key.Name == "subParam")
                //{
                //    (Root.subParam = new MoveDefSubParamNode(data.Key.Name) { offsetID = offsetID }).Initialize(this, new DataSource(BaseAddress + data.Value._dataOffset, data.Key.Size));
                //    _sectionList.Add(Root.subParam);
                //}
                offsetID++;
            }

            SortChildren();
            _sectionList.Sort(Compare);
        }
    }

    public class SpecialOffset
    {
        public int Index;
        public int Offset;
        public int Size;

        public override string ToString()
        {
            return string.Format("[{2}] Offset={0} Size={1}", Offset, Size, Index);
        }
    }

    public unsafe class MoveDefActionsNode : MoveDefEntryNode
    {
        internal bint* Header => (bint*) WorkingUncompressed.Address;

        internal List<int> ActionOffsets = new List<int>();

        public MoveDefActionsNode(string name)
        {
            _name = name;
        }

        public override bool OnInitialize()
        {
            base.OnInitialize();
            for (int i = 0; i < WorkingUncompressed.Length / 4; i++)
            {
                ActionOffsets.Add(Header[i]);
            }

            return true;
        }

        public override void OnPopulate()
        {
            int i = 0;
            foreach (int offset in ActionOffsets)
            {
                if (offset > 0)
                {
                    new MoveDefActionNode("Action" + i, false, null).Initialize(this,
                        new DataSource(BaseAddress + offset, 0));
                }
                else
                {
                    Children.Add(new MoveDefActionNode("Action" + i, true, this));
                }

                i++;
            }
        }
    }
}