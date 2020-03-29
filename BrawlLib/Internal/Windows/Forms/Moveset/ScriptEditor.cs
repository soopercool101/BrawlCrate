using BrawlLib.SSBB.ResourceNodes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace BrawlLib.Internal.Windows.Forms.Moveset
{
    public partial class ScriptEditor : UserControl
    {
        #region Designer

        private void InitializeComponent()
        {
            ComponentResourceManager resources = new ComponentResourceManager(typeof(ScriptEditor));
            panel1 = new Panel();
            description = new Label();
            toolStrip1 = new ToolStrip();
            btnAdd = new ToolStripButton();
            btnRemove = new ToolStripButton();
            btnModify = new ToolStripButton();
            btnUp = new ToolStripButton();
            btnDown = new ToolStripButton();
            panel2 = new Panel();
            toolStrip2 = new ToolStrip();
            btnCopy = new ToolStripButton();
            btnCut = new ToolStripButton();
            btnPaste = new ToolStripButton();
            btnCopyText = new ToolStripButton();
            EventList = new ListBox();
            splitter1 = new Splitter();
            panel1.SuspendLayout();
            toolStrip1.SuspendLayout();
            panel2.SuspendLayout();
            toolStrip2.SuspendLayout();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.Controls.Add(description);
            panel1.Controls.Add(toolStrip1);
            panel1.Dock = DockStyle.Bottom;
            panel1.Location = new Point(1, 188);
            panel1.Name = "panel1";
            panel1.Size = new Size(324, 95);
            panel1.TabIndex = 2;
            // 
            // description
            // 
            description.BackColor = SystemColors.Control;
            description.BorderStyle = BorderStyle.Fixed3D;
            description.Dock = DockStyle.Fill;
            description.Font = new Font("Microsoft Sans Serif", 9.75F, FontStyle.Regular, GraphicsUnit.Point,
                (byte) 0);
            description.Location = new Point(0, 25);
            description.Name = "description";
            description.Size = new Size(324, 70);
            description.TabIndex = 59;
            // 
            // toolStrip1
            // 
            toolStrip1.GripStyle = ToolStripGripStyle.Hidden;
            toolStrip1.Items.AddRange(new ToolStripItem[]
            {
                btnAdd,
                btnRemove,
                btnModify,
                btnUp,
                btnDown
            });
            toolStrip1.Location = new Point(0, 0);
            toolStrip1.Name = "toolStrip1";
            toolStrip1.Size = new Size(324, 25);
            toolStrip1.TabIndex = 0;
            toolStrip1.Text = "toolStrip1";
            // 
            // btnAdd
            // 
            btnAdd.DisplayStyle = ToolStripItemDisplayStyle.Text;
            //btnAdd.Image = (Image) resources.GetObject("btnAdd.Image");
            btnAdd.ImageTransparentColor = Color.Magenta;
            btnAdd.Name = "btnAdd";
            btnAdd.Size = new Size(33, 22);
            btnAdd.Text = "Add";
            btnAdd.Click += new EventHandler(btnAdd_Click);
            // 
            // btnRemove
            // 
            btnRemove.DisplayStyle = ToolStripItemDisplayStyle.Text;
            //btnRemove.Image = (Image) resources.GetObject("btnRemove.Image");
            btnRemove.ImageTransparentColor = Color.Magenta;
            btnRemove.Name = "btnRemove";
            btnRemove.Size = new Size(54, 22);
            btnRemove.Text = "Remove";
            btnRemove.Click += new EventHandler(btnRemove_Click);
            // 
            // btnModify
            // 
            btnModify.DisplayStyle = ToolStripItemDisplayStyle.Text;
            //btnModify.Image = (Image) resources.GetObject("btnModify.Image");
            btnModify.ImageTransparentColor = Color.Magenta;
            btnModify.Name = "btnModify";
            btnModify.Size = new Size(49, 22);
            btnModify.Text = "Modify";
            btnModify.Click += new EventHandler(btnModify_Click);
            // 
            // btnUp
            // 
            btnUp.DisplayStyle = ToolStripItemDisplayStyle.Text;
            //btnUp.Image = (Image) resources.GetObject("btnUp.Image");
            btnUp.ImageTransparentColor = Color.Magenta;
            btnUp.Name = "btnUp";
            btnUp.Size = new Size(23, 22);
            btnUp.Text = "▲";
            btnUp.Click += new EventHandler(btnUp_Click);
            // 
            // btnDown
            // 
            btnDown.DisplayStyle = ToolStripItemDisplayStyle.Text;
            //btnDown.Image = (Image) resources.GetObject("btnDown.Image");
            btnDown.ImageTransparentColor = Color.Magenta;
            btnDown.Name = "btnDown";
            btnDown.Size = new Size(23, 22);
            btnDown.Text = "▼";
            btnDown.Click += new EventHandler(btnDown_Click);
            // 
            // panel2
            // 
            panel2.Controls.Add(toolStrip2);
            panel2.Dock = DockStyle.Top;
            panel2.Location = new Point(1, 1);
            panel2.Name = "panel2";
            panel2.Size = new Size(324, 23);
            panel2.TabIndex = 15;
            // 
            // toolStrip2
            // 
            toolStrip2.Dock = DockStyle.Fill;
            toolStrip2.GripStyle = ToolStripGripStyle.Hidden;
            toolStrip2.Items.AddRange(new ToolStripItem[]
            {
                btnCopy,
                btnCut,
                btnPaste,
                btnCopyText
            });
            toolStrip2.Location = new Point(0, 0);
            toolStrip2.Name = "toolStrip2";
            toolStrip2.Size = new Size(324, 23);
            toolStrip2.TabIndex = 1;
            toolStrip2.Text = "toolStrip2";
            // 
            // btnCopy
            // 
            btnCopy.DisplayStyle = ToolStripItemDisplayStyle.Text;
            //btnCopy.Image = (Image) resources.GetObject("btnCopy.Image");
            btnCopy.ImageTransparentColor = Color.Magenta;
            btnCopy.Name = "btnCopy";
            btnCopy.Size = new Size(39, 20);
            btnCopy.Text = "Copy";
            btnCopy.Click += new EventHandler(btnCopy_Click);
            // 
            // btnCut
            // 
            btnCut.DisplayStyle = ToolStripItemDisplayStyle.Text;
            //btnCut.Image = (Image) resources.GetObject("btnCut.Image");
            btnCut.ImageTransparentColor = Color.Magenta;
            btnCut.Name = "btnCut";
            btnCut.Size = new Size(30, 20);
            btnCut.Text = "Cut";
            btnCut.Click += new EventHandler(btnCut_Click);
            // 
            // btnPaste
            // 
            btnPaste.DisplayStyle = ToolStripItemDisplayStyle.Text;
            //btnPaste.Image = (Image) resources.GetObject("btnPaste.Image");
            btnPaste.ImageTransparentColor = Color.Magenta;
            btnPaste.Name = "btnPaste";
            btnPaste.Size = new Size(39, 20);
            btnPaste.Text = "Paste";
            btnPaste.Click += new EventHandler(btnPaste_Click);
            // 
            // btnCopyText
            // 
            btnCopyText.DisplayStyle = ToolStripItemDisplayStyle.Text;
            //btnCopyText.Image = (Image) resources.GetObject("btnCopyText.Image");
            btnCopyText.ImageTransparentColor = Color.Magenta;
            btnCopyText.Name = "btnCopyText";
            btnCopyText.Size = new Size(64, 20);
            btnCopyText.Text = "Copy Text";
            btnCopyText.Click += new EventHandler(btnCopyText_Click);
            // 
            // EventList
            // 
            EventList.Dock = DockStyle.Fill;
            EventList.FormattingEnabled = true;
            EventList.HorizontalScrollbar = true;
            EventList.IntegralHeight = false;
            EventList.Location = new Point(1, 24);
            EventList.Name = "EventList";
            EventList.SelectionMode = SelectionMode.MultiExtended;
            EventList.Size = new Size(324, 161);
            EventList.TabIndex = 16;
            EventList.SelectedIndexChanged += new EventHandler(EventList_SelectedIndexChanged);
            EventList.DoubleClick += new EventHandler(EventList_DoubleClick);
            // 
            // splitter1
            // 
            splitter1.Dock = DockStyle.Bottom;
            splitter1.Location = new Point(1, 185);
            splitter1.Name = "splitter1";
            splitter1.Size = new Size(324, 3);
            splitter1.TabIndex = 60;
            splitter1.TabStop = false;
            // 
            // ScriptEditor
            // 
            Controls.Add(EventList);
            Controls.Add(panel2);
            Controls.Add(splitter1);
            Controls.Add(panel1);
            Name = "ScriptEditor";
            Padding = new Padding(1);
            Size = new Size(326, 284);
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            toolStrip1.ResumeLayout(false);
            toolStrip1.PerformLayout();
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            toolStrip2.ResumeLayout(false);
            toolStrip2.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;

        //public ModelMovesetPanel form;

        private MoveDefNode _mDef;
        private Panel panel2;
        public ListBox EventList;

        public string[] iRequirements = new string[0];
        public string[] iAirGroundStats = new string[0];
        public string[] iCollisionStats = new string[0];

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public MoveDefNode MoveDef
        {
            get => _mDef;
            set => _mDef = value;
        }

        private MoveDefActionNode _targetNode;
        private Label description;
        private ToolStrip toolStrip1;
        private ToolStripButton btnDown;
        private ToolStripButton btnAdd;
        private ToolStripButton btnRemove;
        private ToolStripButton btnModify;
        private ToolStripButton btnUp;
        private ToolStrip toolStrip2;
        private ToolStripButton btnCopy;
        private ToolStripButton btnCut;
        private ToolStripButton btnPaste;
        private ToolStripButton btnCopyText;
        private Splitter splitter1;

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public MoveDefActionNode TargetNode
        {
            get => _targetNode;
            set
            {
                _targetNode = value;
                TargetChanged();
            }
        }

        public bool called;

        public ScriptEditor()
        {
            InitializeComponent();
        }

        private void TargetChanged()
        {
            if (TargetNode != null)
            {
                if (!called)
                {
                    called = true;
                    iRequirements = _targetNode.Root.iRequirements;
                    iAirGroundStats = _targetNode.Root.iAirGroundStats;
                    iCollisionStats = _targetNode.Root.iCollisionStats;
                }

                MoveDef = TargetNode.Root;
                //Offset.Text = TargetNode.HexOffset;
                MakeScript();

                description.Text = "No Description Available.";
            }
            else
            {
                MoveDef = null;
                EventList.Items.Clear();
                //Offset.Text = "";
            }
        }

        public void MakeScript()
        {
            if (TargetNode == null)
            {
                return;
            }

            string[] script = new string[TargetNode.Children.Count];
            int tabs = 0;

            for (int i = 0; i < TargetNode.Children.Count; i++)
            {
                MoveDefEventNode node = TargetNode.Children[i] as MoveDefEventNode;
                string arg = node._name;

                //Format the event and its parameters into a readable script.
                script[i] = ResolveEventSyntax(GetEventSyntax(node._event), node);
                if (script[i] == "")
                {
                    script[i] = GetDefaultSyntax(node);
                }

                //Add tabs to the script in correspondence to the code Params.
                tabs -= Helpers.TabDownEvents(node._event);
                for (int i2 = 0; i2 < tabs; i2++)
                {
                    script[i] = "\t" + script[i];
                }

                tabs += Helpers.TabUpEvents(node._event);
            }

            EventList.Items.Clear();
            EventList.Items.AddRange(script);
        }

        //  Return the event syntax corresponding to the event id passed
        public string GetEventSyntax(uint id)
        {
            if (MoveDefNode.EventDictionary.ContainsKey(id))
            {
                return MoveDefNode.EventDictionary[id]._syntax;
            }

            return "";
        }

        //Return the parameters contained in the keyword's parameter list.
        public string[] GetParameters(string strParams, MoveDefEventNode Event)
        {
            string[] parameters = new string[0];
            char chrFound = '\0';
            int paramEnd = -1;
            int index = 0;
            int loc = 0;

            //Search for a ',' or a ')' and return the preceeding string.
            do
            {
                paramEnd = Helpers.FindFirstOfIgnoreNest(strParams, loc, new char[] {',', ')'}, ref chrFound);
                if (paramEnd == -1)
                {
                    paramEnd = strParams.Length;
                }

                Array.Resize(ref parameters, index + 1);
                parameters[index] = strParams.Substring(loc, paramEnd - loc);
                parameters[index] = Helpers.ClearWhiteSpace(parameters[index]);

                loc = paramEnd + 1;
                index++;
            } while (chrFound != ')' && chrFound != '\0');

            //Check each parameter for keywords and resolve if they are present.
            for (int i = 0; i < parameters.Length; i++)
            {
                if (parameters[i] != "")
                {
                    parameters[i] = ResolveEventSyntax(parameters[i], Event);
                }
            }

            return parameters;
        }

        //Return the collision status corresponding to the value passed.
        public string GetCollisionStatus(long value)
        {
            if (value > iCollisionStats.Length)
            {
                return "Undefined(" + value + ")";
            }

            return iCollisionStats[value];
        }

        //Return the air or ground status corresponding to the value passed.
        public string GetAirGroundStatus(long value)
        {
            if (value > iAirGroundStats.Length)
            {
                return "Undefined(" + value + ")";
            }

            return iAirGroundStats[value];
        }

        //Return the collision status corresponding to the value passed.
        public string GetEnum(int paramIndex, long value, Event eventData)
        {
            if (MoveDefNode.EventDictionary.ContainsKey(eventData.eventEvent))
            {
                Dictionary<int, List<string>> Params = MoveDefNode.EventDictionary[eventData.eventEvent].Enums;
                if (Params.ContainsKey(paramIndex))
                {
                    List<string> values = Params[paramIndex];
                    if (values != null && values.Count > value)
                    {
                        return values[(int) value];
                    }
                }
            }

            return "Undefined(" + value + ")";
        }

        //Return the string result from the passed keyword and its parameters.
        public string ResolveKeyword(string keyword, string[] Params, MoveDefEventNode Event)
        {
            Event eventData = Event.EventData;
            switch (keyword)
            {
                case "\\value":
                    //try { 
                    return ResolveParamTypes(Event)[int.Parse(Params[0])]; //}
                //catch { return "Value-" + Params[0]; }
                case "\\type":
                    try
                    {
                        return eventData.parameters[int.Parse(Params[0])]._type.ToString();
                    }
                    catch
                    {
                        return "Type-" + Params[0];
                    }

                case "\\if":
                    bool compare = false;
                    try
                    {
                        switch (Params[1])
                        {
                            case "==":
                                compare = int.Parse(Params[0]) == int.Parse(Params[2]);
                                break;
                            case "!=":
                                compare = int.Parse(Params[0]) != int.Parse(Params[2]);
                                break;
                            case ">=":
                                compare = int.Parse(Params[0]) >= int.Parse(Params[2]);
                                break;
                            case "<=":
                                compare = int.Parse(Params[0]) <= int.Parse(Params[2]);
                                break;
                            case ">":
                                compare = int.Parse(Params[0]) > int.Parse(Params[2]);
                                break;
                            case "<":
                                compare = int.Parse(Params[0]) < int.Parse(Params[2]);
                                break;
                        }
                    }
                    finally
                    {
                    }

                    if (compare)
                    {
                        return Params[3];
                    }
                    else
                    {
                        return Params[4];
                    }

                case "\\bone":
                    try
                    {
                        int id = int.Parse(Params[0]);
                        if (id >= 400)
                        {
                            TargetNode.Root.GetBoneIndex(ref id);
                        }

                        if (_targetNode.Model?._linker.BoneCache != null &&
                            _targetNode.Model._linker.BoneCache.Length > id && id >= 0)
                        {
                            return _targetNode.Model._linker.BoneCache[id].Name;
                        }
                        else
                        {
                            return id.ToString();
                        }
                    }
                    catch
                    {
                        return int.Parse(Params[0]).ToString();
                    }

                case "\\unhex":
                    /*try { return MParams.UnHex(Params[0]).ToString(); }
                    catch { */
                    return Params[0]; // }
                case "\\hex":
                    /*try { return MParams.Hex(int.Parse(Params[0])); }
                    catch { */
                    return Params[0]; // }
                case "\\hex8":
                    /*try { return MParams.Hex8(int.Parse(Params[0])); }
                    catch { */
                    return Params[0]; // }
                case "\\half1":
                    return (uint.Parse(Params[0]) >> 16).ToString();
                case "\\half2":
                    return (uint.Parse(Params[0]) & 0xFFFF).ToString();
                case "\\byte1":
                    return (uint.Parse(Params[0]) >> 24).ToString();
                case "\\byte2":
                    return ((uint.Parse(Params[0]) >> 16) & 0xFF).ToString();
                case "\\byte3":
                    return ((uint.Parse(Params[0]) >> 8) & 0xFF).ToString();
                case "\\byte4":
                    return (uint.Parse(Params[0]) & 0xFF).ToString();
                case "\\collision":
                    try
                    {
                        return GetCollisionStatus(int.Parse(Params[0]));
                    }
                    catch
                    {
                        return Params[0];
                    }

                case "\\airground":
                    try
                    {
                        return GetAirGroundStatus(int.Parse(Params[0]));
                    }
                    catch
                    {
                        return Params[0];
                    }

                case "\\enum":
                    try
                    {
                        return GetEnum(int.Parse(Params[1]), int.Parse(Params[0]), eventData);
                    }
                    catch
                    {
                        return "Undefined(" + Params[1] + ")";
                    }

                case "\\cmpsign":
                    try
                    {
                        return Helpers.GetComparisonSign(int.Parse(Params[0]));
                    }
                    catch
                    {
                        return Params[0];
                    }

                case "\\name":
                    return GetEventInfo(eventData.eventEvent)._name;
                case "\\white":
                    return " ";
                default:
                    return "";
            }
        }

        //Return a string of the parameter in the format corresponding to it's type.
        public string[] ResolveParamTypes(MoveDefEventNode Event)
        {
            Event eventData = Event.EventData;
            string[] p = new string[eventData.parameters.Length];

            for (int i = 0; i < p.Length; i++)
            {
                switch ((int) eventData.parameters[i]._type)
                {
                    case 0:
                        p[i] = GetValue(eventData.eventEvent, i, eventData.parameters[i]._data);
                        break;
                    case 1:
                        p[i] = Helpers.UnScalar(eventData.parameters[i]._data).ToString();
                        break;
                    case 2:
                        p[i] = ResolvePointer(eventData.pParameters + i * 8 + 4, eventData.parameters[i],
                            Event.Children[i] as MoveDefEventOffsetNode);
                        break;
                    case 3:
                        p[i] = eventData.parameters[i]._data != 0 ? "true" : "false";
                        break;
                    case 4:
                        p[i] = Helpers.Hex(eventData.parameters[i]._data);
                        break;
                    case 5:
                        p[i] = ResolveVariable(eventData.parameters[i]._data);
                        break;
                    case 6:
                        p[i] = GetRequirement(eventData.parameters[i]._data);
                        break;
                }
            }

            return p;
        }

        //Return the name of the external pointer corresponding to the address if 
        //one is available, otherwise return the string of the value passed.
        public string ResolvePointer(long pointer, Param parameter, MoveDefEventOffsetNode node)
        {
            //MoveDefExternalNode ext;
            //if ((ext = _targetNode.Root.IsExternal((int)pointer)) != null || (ext = _targetNode.Root.IsExternal((int)parameter._data)) != null)
            //    return "External: " + ext.Name;

            if (node._extNode != null)
            {
                return "External: " + node._extNode.Name;
            }

            if (node.list == 4)
            {
                return "0x" + Helpers.Hex(parameter._data);
            }
            else
            {
                string name = "", t = "", grp = "";
                switch (node.list)
                {
                    case 0:
                        grp = "Actions";
                        name = _targetNode.Root._actions.Children[node.index].Name;
                        switch (node.type)
                        {
                            case 0:
                                t = "Entry";
                                break;
                            case 1:
                                t = "Exit";
                                break;
                        }

                        break;
                    case 1:
                        grp = "SubActions";
                        name = _targetNode.Root._subActions.Children[node.index].Name;
                        switch (node.type)
                        {
                            case 0:
                                t = "Main";
                                break;
                            case 1:
                                t = "GFX";
                                break;
                            case 2:
                                t = "SFX";
                                break;
                            case 3:
                                t = "Other";
                                break;
                        }

                        break;
                    case 2:
                        grp = "SubRoutines";
                        name = _targetNode.Root._subRoutineList[node.index].Name;
                        break;
                    case 3:
                        return "External: " + _targetNode.Root.references.Children[node.index].Name;
                    case 5:
                        grp = "Screen Tints";
                        name = _targetNode.Root.dataCommon._screenTint.Children[node.index].Name;
                        break;
                    case 6:
                        grp = "Flash Overlays";
                        name = _targetNode.Root.dataCommon._flashOverlay.Children[node.index].Name;
                        break;
                }

                return name + (node.list >= 2 ? "" : " - " + t) + " in the " + grp + " list";
            }
        }

        //Return the full name of the variable corresponding to the value passed.
        public string ResolveVariable(long value)
        {
            string variableName = "";
            long variableMemType = (value & 0xF0000000) / 0x10000000;
            long variableType = (value & 0xF000000) / 0x1000000;
            long variableNumber = value & 0xFFFFFF;
            if (variableMemType == 0)
            {
                variableName = "IC-";
            }

            if (variableMemType == 1)
            {
                variableName = "LA-";
            }

            if (variableMemType == 2)
            {
                variableName = "RA-";
            }

            if (variableType == 0)
            {
                variableName += "Basic";
            }

            if (variableType == 1)
            {
                variableName += "Float";
            }

            if (variableType == 2)
            {
                variableName += "Bit";
            }

            variableName += "[" + variableNumber + "]";

            return variableName;
        }

        public string GetValue(long eventId, int index, long value)
        {
            string s = null;
            switch (eventId)
            {
                case 0x04000100:
                case 0x04000200:
                    if (index == 0)
                    {
                        if (TargetNode.Parent?.Parent != null && TargetNode.Parent.Parent.Name.StartsWith("Article"))
                        {
                            ResourceNode sa = TargetNode.Parent.Parent.FindChild("SubActions", false);
                            if (sa != null)
                            {
                                return sa.Children[(int) value].Name;
                            }
                        }
                        else if (TargetNode.Root._subActions != null &&
                                 value < TargetNode.Root._subActions.Children.Count && value >= 0)
                        {
                            return TargetNode.Root._subActions.Children[(int) value].Name;
                        }
                        else
                        {
                            return ((int) value).ToString();
                        }
                    }

                    break;
                //case 0x02010200:
                //case 0x02010300:
                //case 0x02010500:
                //    if (index == 0)
                //        if (TargetNode.Parent != null && TargetNode.Parent.Parent != null && TargetNode.Parent.Parent.Name.StartsWith("Article"))
                //        {
                //            ResourceNode sa = TargetNode.Parent.Parent.FindChild("Actions", false);
                //            if (sa != null)
                //                return sa.Children[(int)value].Name;
                //        }
                //        else if (value < TargetNode.Root._actions.Children.Count)
                //            return TargetNode.Root._actions.Children[(int)value].Name;
                //    break;
            }

            return s == null ? value.ToString() : s;
        }

        //Return the requirement corresponding to the value passed.
        public string GetRequirement(long value)
        {
            bool not = (value & 0x80000000) == 0x80000000;
            long requirement = value & 0xFF;

            if (requirement > iRequirements.Length)
            {
                return Helpers.Hex(requirement);
            }

            if (not)
            {
                return "Not " + iRequirements[requirement];
            }

            return iRequirements[requirement];
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

        //Return the event name followed by each parameter paired with its type.
        public string GetDefaultSyntax(MoveDefEventNode Event)
        {
            Event eventData = Event.EventData;
            string script = GetEventInfo(eventData.eventEvent)._name + (eventData.lParameters > 0 ? ": " : "");
            for (int i = 0; i < eventData.lParameters; i++)
            {
                script += eventData.parameters[i]._type + "-";
                switch ((int) eventData.parameters[i]._type)
                {
                    case 0:
                        script += GetValue(eventData.eventEvent, i, eventData.parameters[i]._data);
                        break;
                    case 1:
                        script += Helpers.UnScalar(eventData.parameters[i]._data).ToString();
                        break;
                    case 2:
                        script += ResolvePointer(eventData.pParameters + i * 8 + 4, eventData.parameters[i],
                            Event.Children[i] as MoveDefEventOffsetNode);
                        break;
                    case 3:
                        script += eventData.parameters[i]._data != 0 ? "true" : "false";
                        break;
                    case 4:
                        script += eventData.parameters[i]._data;
                        break;
                    case 5:
                        script += ResolveVariable(eventData.parameters[i]._data);
                        break;
                    case 6:
                        script += GetRequirement(eventData.parameters[i]._data);
                        break;
                }

                if (i != eventData.lParameters)
                {
                    script += ", ";
                }
            }

            return script;
        }

        //Return the passed syntax with all keywords replaced with their proper values.
        public string ResolveEventSyntax(string syntax, MoveDefEventNode Event)
        {
            Event eventData = Event.EventData;
            while (true)
            {
                string keyword = "";
                string keyResult = "";
                string strParams = "";
                string[] kParams;

                int keyBegin = Helpers.FindFirst(syntax, 0, '\\');
                if (keyBegin == -1)
                {
                    break;
                }

                int keyEnd = Helpers.FindFirst(syntax, keyBegin, '(');
                if (keyEnd == -1)
                {
                    keyEnd = syntax.Length;
                }

                int paramsBegin = keyEnd + 1;

                int paramsEnd = Helpers.FindFirstIgnoreNest(syntax, paramsBegin, ')');
                if (paramsEnd == -1)
                {
                    paramsEnd = syntax.Length;
                }

                keyword = syntax.Substring(keyBegin, keyEnd - keyBegin);

                strParams = syntax.Substring(paramsBegin, paramsEnd - paramsBegin);
                kParams = GetParameters(strParams, Event);

                keyResult = ResolveKeyword(keyword, kParams, Event);

                syntax = Helpers.DelSubstring(syntax, keyBegin, paramsEnd + 1 - keyBegin);
                syntax = Helpers.InsString(syntax, keyResult, keyBegin);
            }

            return syntax;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            int highest = EventList.Items.Count;
            if (EventList.SelectedIndex != -1)
            {
                highest = EventList.SelectedIndices[EventList.SelectedIndices.Count - 1];
            }

            MoveDefEventNode d = new MoveDefEventNode();
            if (highest == EventList.Items.Count)
            {
                TargetNode.AddChild(d);
            }
            else
            {
                TargetNode.InsertChild(d, true, highest + 1);
            }

            d.EventID = 0x00020000;
            MakeScript();

            if (EventList.Items.Count > highest + 1)
            {
                EventList.SelectedIndex = highest + 1;
            }
            else
            {
                EventList.SelectedIndex = EventList.Items.Count - 1;
            }
        }

        private void btnModify_Click(object sender, EventArgs e)
        {
            if (EventList.SelectedIndex != -1)
            {
                FormModifyEvent p = new FormModifyEvent();
                p.eventModifier1.origEvent = TargetNode.Children[EventList.SelectedIndex] as MoveDefEventNode;
                if (p.ShowDialog() == DialogResult.OK)
                {
                    MakeScript();
                }
            }
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            int[] indices = new int[EventList.SelectedIndices.Count];
            EventList.SelectedIndices.CopyTo(indices, 0);
            for (int i = indices.Length - 1; i >= 0; i--)
            {
                TargetNode.Children[indices[i]].Remove();
            }

            MakeScript();
            if (TargetNode != null && indices.Length == 1)
            {
                foreach (int i in indices)
                {
                    if (i - indices.Length >= 0)
                    {
                        EventList.SetSelected(i - indices.Length, true);
                    }
                    else if (EventList.Items.Count > 0)
                    {
                        EventList.SetSelected(0, true);
                    }
                }
            }
        }

        private void btnUp_Click(object sender, EventArgs e)
        {
            int lowest = -1;
            if (EventList.SelectedIndex != -1)
            {
                lowest = EventList.SelectedIndices[0];
            }

            int[] indices = new int[EventList.SelectedIndices.Count];
            EventList.SelectedIndices.CopyTo(indices, 0);
            if (lowest != -1 && lowest != 0)
            {
                for (int i = 0; i < EventList.SelectedIndices.Count; i++)
                {
                    TargetNode.Children[EventList.SelectedIndices[i]].DoMoveUp(false);
                }

                MakeScript();
                if (TargetNode != null)
                {
                    foreach (int i in indices)
                    {
                        EventList.SetSelected(i - 1, true);
                    }
                }
            }
        }

        private void btnDown_Click(object sender, EventArgs e)
        {
            int highest = -1;
            if (EventList.SelectedIndex != -1)
            {
                highest = EventList.SelectedIndices[EventList.SelectedIndices.Count - 1];
            }

            int[] indices = new int[EventList.SelectedIndices.Count];
            EventList.SelectedIndices.CopyTo(indices, 0);
            if (highest != -1 && highest != EventList.Items.Count - 1)
            {
                for (int i = EventList.SelectedIndices.Count - 1; i >= 0; i--)
                {
                    TargetNode.Children[EventList.SelectedIndices[i]].DoMoveDown(false);
                }

                MakeScript();
                if (TargetNode != null)
                {
                    foreach (int i in indices)
                    {
                        EventList.SetSelected(i + 1, true);
                    }
                }
            }
        }

        private void btnCopy_Click(object sender, EventArgs e)
        {
            string s = "";
            foreach (int i in EventList.SelectedIndices)
            {
                s += (TargetNode.Children[i] as MoveDefEventNode).Serialize();
                s += "/";
            }

            if (!string.IsNullOrEmpty(s))
            {
                Clipboard.SetText(s);
            }
        }

        private void btnPaste_Click(object sender, EventArgs e)
        {
            List<int> indices = new List<int>();

            int highest = EventList.Items.Count;
            if (EventList.SelectedIndex != -1)
            {
                highest = EventList.SelectedIndices[EventList.SelectedIndices.Count - 1];
            }

            string s = Clipboard.GetText();

            try
            {
                string[] Events = s.Split('/');
                foreach (string x in Events)
                {
                    MoveDefEventNode y = MoveDefEventNode.Deserialize(x, TargetNode.Root);
                    if (y != null)
                    {
                        if (highest == TargetNode.Children.Count)
                        {
                            TargetNode.AddChild(y);
                        }
                        else
                        {
                            TargetNode.InsertChild(y, true, highest + 1);
                        }

                        indices.Add(y.Index);
                        highest++;
                    }
                }
            }
            finally
            {
                MakeScript();
                if (TargetNode != null)
                {
                    foreach (int i in indices)
                    {
                        EventList.SetSelected(i, true);
                    }
                }
            }
        }

        private void EventList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (EventList.SelectedIndex >= 0)
            {
                ActionEventInfo info = (TargetNode.Children[EventList.SelectedIndex] as MoveDefEventNode).EventInfo;
                if (info != null && !string.IsNullOrEmpty(info._description))
                {
                    description.Text = info._description;
                }
                else
                {
                    description.Text = "No Description Available.";
                }
            }
        }

        private void EventList_DoubleClick(object sender, EventArgs e)
        {
            btnModify_Click(sender, e);
        }

        private void btnCopyText_Click(object sender, EventArgs e)
        {
            string s = "";
            foreach (int i in EventList.SelectedIndices)
            {
                s += EventList.Items[i] + Environment.NewLine;
            }

            if (!string.IsNullOrEmpty(s))
            {
                Clipboard.SetText(s);
            }
        }

        private void btnCut_Click(object sender, EventArgs e)
        {
            btnCopy_Click(sender, e);
            btnRemove_Click(sender, e);
        }
    }
}