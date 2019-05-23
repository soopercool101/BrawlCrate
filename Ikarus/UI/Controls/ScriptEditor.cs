using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Windows.Forms;
using Ikarus.MovesetFile;
using BrawlLib.SSBBTypes;

namespace Ikarus.UI
{
    public class ScriptEditor : UserControl
    {
        #region Designer
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ScriptEditor));
            this.panel1 = new System.Windows.Forms.Panel();
            this.description = new System.Windows.Forms.Label();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.btnAdd = new System.Windows.Forms.ToolStripButton();
            this.btnRemove = new System.Windows.Forms.ToolStripButton();
            this.btnModify = new System.Windows.Forms.ToolStripButton();
            this.btnUp = new System.Windows.Forms.ToolStripButton();
            this.btnDown = new System.Windows.Forms.ToolStripButton();
            this.panel2 = new System.Windows.Forms.Panel();
            this.toolStrip2 = new System.Windows.Forms.ToolStrip();
            this.btnCopy = new System.Windows.Forms.ToolStripButton();
            this.btnCut = new System.Windows.Forms.ToolStripButton();
            this.btnPaste = new System.Windows.Forms.ToolStripButton();
            this.btnCopyText = new System.Windows.Forms.ToolStripButton();
            this.EventList = new System.Windows.Forms.ListBox();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.panel1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.toolStrip2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.description);
            this.panel1.Controls.Add(this.toolStrip1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(1, 188);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(324, 95);
            this.panel1.TabIndex = 2;
            // 
            // description
            // 
            this.description.BackColor = System.Drawing.SystemColors.Control;
            this.description.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.description.Dock = System.Windows.Forms.DockStyle.Fill;
            this.description.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.description.Location = new System.Drawing.Point(0, 25);
            this.description.Name = "description";
            this.description.Size = new System.Drawing.Size(324, 70);
            this.description.TabIndex = 59;
            // 
            // toolStrip1
            // 
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnAdd,
            this.btnRemove,
            this.btnModify,
            this.btnUp,
            this.btnDown});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(324, 25);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // btnAdd
            // 
            this.btnAdd.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnAdd.Image = ((System.Drawing.Image)(resources.GetObject("btnAdd.Image")));
            this.btnAdd.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(33, 22);
            this.btnAdd.Text = "Add";
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnRemove
            // 
            this.btnRemove.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnRemove.Image = ((System.Drawing.Image)(resources.GetObject("btnRemove.Image")));
            this.btnRemove.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new System.Drawing.Size(54, 22);
            this.btnRemove.Text = "Remove";
            this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
            // 
            // btnModify
            // 
            this.btnModify.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnModify.Image = ((System.Drawing.Image)(resources.GetObject("btnModify.Image")));
            this.btnModify.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnModify.Name = "btnModify";
            this.btnModify.Size = new System.Drawing.Size(49, 22);
            this.btnModify.Text = "Modify";
            this.btnModify.Click += new System.EventHandler(this.btnModify_Click);
            // 
            // btnUp
            // 
            this.btnUp.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnUp.Image = ((System.Drawing.Image)(resources.GetObject("btnUp.Image")));
            this.btnUp.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnUp.Name = "btnUp";
            this.btnUp.Size = new System.Drawing.Size(23, 22);
            this.btnUp.Text = "▲";
            this.btnUp.Click += new System.EventHandler(this.btnUp_Click);
            // 
            // btnDown
            // 
            this.btnDown.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnDown.Image = ((System.Drawing.Image)(resources.GetObject("btnDown.Image")));
            this.btnDown.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnDown.Name = "btnDown";
            this.btnDown.Size = new System.Drawing.Size(23, 22);
            this.btnDown.Text = "▼";
            this.btnDown.Click += new System.EventHandler(this.btnDown_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.toolStrip2);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(1, 1);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(324, 23);
            this.panel2.TabIndex = 15;
            // 
            // toolStrip2
            // 
            this.toolStrip2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStrip2.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnCopy,
            this.btnCut,
            this.btnPaste,
            this.btnCopyText});
            this.toolStrip2.Location = new System.Drawing.Point(0, 0);
            this.toolStrip2.Name = "toolStrip2";
            this.toolStrip2.Size = new System.Drawing.Size(324, 23);
            this.toolStrip2.TabIndex = 1;
            this.toolStrip2.Text = "toolStrip2";
            // 
            // btnCopy
            // 
            this.btnCopy.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnCopy.Image = ((System.Drawing.Image)(resources.GetObject("btnCopy.Image")));
            this.btnCopy.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnCopy.Name = "btnCopy";
            this.btnCopy.Size = new System.Drawing.Size(39, 20);
            this.btnCopy.Text = "Copy";
            this.btnCopy.Click += new System.EventHandler(this.btnCopy_Click);
            // 
            // btnCut
            // 
            this.btnCut.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnCut.Image = ((System.Drawing.Image)(resources.GetObject("btnCut.Image")));
            this.btnCut.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnCut.Name = "btnCut";
            this.btnCut.Size = new System.Drawing.Size(30, 20);
            this.btnCut.Text = "Cut";
            this.btnCut.Click += new System.EventHandler(this.btnCut_Click);
            // 
            // btnPaste
            // 
            this.btnPaste.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnPaste.Image = ((System.Drawing.Image)(resources.GetObject("btnPaste.Image")));
            this.btnPaste.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnPaste.Name = "btnPaste";
            this.btnPaste.Size = new System.Drawing.Size(39, 20);
            this.btnPaste.Text = "Paste";
            this.btnPaste.Click += new System.EventHandler(this.btnPaste_Click);
            // 
            // btnCopyText
            // 
            this.btnCopyText.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnCopyText.Image = ((System.Drawing.Image)(resources.GetObject("btnCopyText.Image")));
            this.btnCopyText.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnCopyText.Name = "btnCopyText";
            this.btnCopyText.Size = new System.Drawing.Size(64, 20);
            this.btnCopyText.Text = "Copy Text";
            this.btnCopyText.Click += new System.EventHandler(this.btnCopyText_Click);
            // 
            // EventList
            // 
            this.EventList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.EventList.FormattingEnabled = true;
            this.EventList.HorizontalScrollbar = true;
            this.EventList.IntegralHeight = false;
            this.EventList.Location = new System.Drawing.Point(1, 24);
            this.EventList.Name = "EventList";
            this.EventList.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.EventList.Size = new System.Drawing.Size(324, 161);
            this.EventList.TabIndex = 16;
            this.EventList.SelectedIndexChanged += new System.EventHandler(this.EventList_SelectedIndexChanged);
            this.EventList.DoubleClick += new System.EventHandler(this.EventList_DoubleClick);
            // 
            // splitter1
            // 
            this.splitter1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.splitter1.Location = new System.Drawing.Point(1, 185);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(324, 3);
            this.splitter1.TabIndex = 60;
            this.splitter1.TabStop = false;
            // 
            // ScriptEditor
            // 
            this.Controls.Add(this.EventList);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.splitter1);
            this.Controls.Add(this.panel1);
            this.Name = "ScriptEditor";
            this.Padding = new System.Windows.Forms.Padding(1);
            this.Size = new System.Drawing.Size(326, 284);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.toolStrip2.ResumeLayout(false);
            this.toolStrip2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Panel panel1;

        public ScriptPanel _mainWindow;

        private MovesetNode _mDef;
        private Panel panel2;
        public ListBox EventList;
        
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public MovesetNode MoveDef
        {
            get { return _mDef; }
            set { _mDef = value; }
        }

        private Script _targetNode;
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
    
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Script TargetNode
        {
            get { return _targetNode; }
            set { _targetNode = value; TargetChanged(); }
        }

        public ScriptEditor() { InitializeComponent(); }
        public ScriptEditor(ScriptPanel owner)
        {
            InitializeComponent();
            _mainWindow = owner; 
        }

        private void TargetChanged()
        {
            if (TargetNode != null)
            {
                MoveDef = TargetNode._root as MovesetNode;
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
                return;

            string[] script = new string[TargetNode.Count];
            int tabs = 0;

            for (int i = 0; i < TargetNode.Count; i++)
            {
                Event node = TargetNode[i] as Event;
                string arg = node._name;

                //Format the event and its parameters into a readable script.
                script[i] = ResolveEventSyntax(GetEventSyntax(node.EventID), node);
                if (script[i] == "") script[i] = GetDefaultSyntax(node);

                //Add tabs to the script in correspondence to the code Params.
                tabs -= Util.TabDownEvents(node.EventID);
                for (int i2 = 0; i2 < tabs; i2++) script[i] = "\t" + script[i];
                tabs += Util.TabUpEvents(node.EventID);
            }
            EventList.Items.Clear();
            EventList.Items.AddRange(script);
        }

        //  Return the event syntax corresponding to the event id passed
        public string GetEventSyntax(uint id)
        {
            if (Manager.Events.ContainsKey(id))
                return Manager.Events[id]._syntax;
            
            return "";
        }

        //Return the parameters contained in the keyword's parameter list.
        public string[] GetParameters(string strParams, Event Event)
        {
            string[] parameters = new string[0];
            char chrFound = '\0';
            int paramEnd = -1;
            int index = 0;
            int loc = 0;

            //Search for a ',' or a ')' and return the preceeding string.
            do
            {
                paramEnd = Util.FindFirstOfIgnoreNest(strParams, loc, new char[] { ',', ')' }, ref chrFound);
                if (paramEnd == -1) paramEnd = strParams.Length;

                Array.Resize<string>(ref parameters, index + 1);
                parameters[index] = strParams.Substring(loc, paramEnd - loc);
                parameters[index] = Util.ClearWhiteSpace(parameters[index]);

                loc = paramEnd + 1;
                index++;
            } while (chrFound != ')' && chrFound != '\0');

            //Check each parameter for keywords and resolve if they are present.
            for (int i = 0; i < parameters.Length; i++)
                if (parameters[i] != "") parameters[i] = ResolveEventSyntax(parameters[i], Event);

            return parameters;
        }

        //Return the collision status corresponding to the value passed.
        public string GetCollisionStatus(long value)
        {
            if (value > Manager.iCollisionStats.Length)
                return "Undefined(" + value + ")";

            return Manager.iCollisionStats[value];
        }

        //Return the air or ground status corresponding to the value passed.
        public string GetAirGroundStatus(long value)
        {
            if (value > Manager.iAirGroundStats.Length)
                return "Undefined(" + value + ")";

            return Manager.iAirGroundStats[value];
        }

        //Return the collision status corresponding to the value passed.
        public string GetEnum(int paramIndex, long value, Event e)
        {
            if (Manager.Events.ContainsKey(e.EventID))
            {
                Dictionary<int, List<string>> p = Manager.Events[e.EventID]._enums;
                if (p.ContainsKey(paramIndex))
                {
                    List<string> values = p[paramIndex];
                    if (values != null && values.Count > value)
                        return values[(int)value];
                }
            }
            return "Undefined(" + value + ")";
        }

        //Return the string result from the passed keyword and its parameters.
        public string ResolveKeyword(string keyword, string[] p, Event e)
        {
            switch (keyword)
            {
                case "\\value":
                    try { return ResolveParamTypes(e)[int.Parse(p[0])]; }
                    catch { return "Value-" + p[0]; }
                case "\\type":
                    try { return e[int.Parse(p[0])].ParamType.ToString(); }
                    catch { return "Type-" + p[0]; }
                case "\\if":
                    bool compare = false;
                    try
                    {
                        switch (p[1])
                        {
                            case "==": compare = int.Parse(p[0]) == int.Parse(p[2]); break;
                            case "!=": compare = int.Parse(p[0]) != int.Parse(p[2]); break;
                            case ">=": compare = int.Parse(p[0]) >= int.Parse(p[2]); break;
                            case "<=": compare = int.Parse(p[0]) <= int.Parse(p[2]); break;
                            case ">": compare = int.Parse(p[0]) > int.Parse(p[2]); break;
                            case "<": compare = int.Parse(p[0]) < int.Parse(p[2]); break;
                        }
                    }
                    finally { }
                    if (compare)
                        return p[3];
                    else
                        return p[4];
                case "\\bone":
                    try
                    {
                        int id = int.Parse(p[0]);
                        if (id >= 400)
                            ((MovesetNode)TargetNode._root).GetBoneIndex(ref id);
                        if (_targetNode.Model != null && _targetNode.Model._linker.BoneCache != null && _targetNode.Model._linker.BoneCache.Length > id && id >= 0)
                            return _targetNode.Model._linker.BoneCache[id].Name;
                        else return id.ToString();
                    }
                    catch { return int.Parse(p[0]).ToString(); }
                case "\\unhex":
                    /*try { return MParams.UnHex(Params[0]).ToString(); }
                    catch { */
                    return p[0];// }
                case "\\hex":
                    /*try { return MParams.Hex(int.Parse(Params[0])); }
                    catch { */
                    return p[0];// }
                case "\\hex8":
                    /*try { return MParams.Hex8(int.Parse(Params[0])); }
                    catch { */
                    return p[0];// }
                case "\\half1":
                    return (uint.Parse(p[0]) >> 16).ToString();
                case "\\half2":
                    return (uint.Parse(p[0]) & 0xFFFF).ToString();
                case "\\byte1":
                    return (uint.Parse(p[0]) >> 24).ToString();
                case "\\byte2":
                    return ((uint.Parse(p[0]) >> 16) & 0xFF).ToString();
                case "\\byte3":
                    return ((uint.Parse(p[0]) >> 8) & 0xFF).ToString();
                case "\\byte4":
                    return ((uint.Parse(p[0])) & 0xFF).ToString();
                case "\\collision":
                    try { return GetCollisionStatus(int.Parse(p[0])); }
                    catch { return p[0]; }
                case "\\airground":
                    try { return GetAirGroundStatus(int.Parse(p[0])); }
                    catch { return p[0]; }
                case "\\enum":
                    try { return GetEnum(int.Parse(p[1]), int.Parse(p[0]), e); }
                    catch { return "Undefined(" + p[1] + ")"; }
                case "\\cmpsign":
                    try { return Util.GetComparisonSign(int.Parse(p[0])); }
                    catch { return p[0]; }
                case "\\name":
                    return GetEventInfo(e.EventID)._name;
                case "\\white":
                    return " ";
                default:
                    return "";
            }
        }
        //Return a string of the parameter in the format corresponding to it's type.
        public string[] ResolveParamTypes(Event e)
        {
            string[] p = new string[e.Count];

            for (int i = 0; i < p.Length; i++)
            {
                int x = e[i].Data;
                switch ((int)e[i].ParamType)
                {
                    case 0: p[i] = GetValue(e.EventID, i, x); break;
                    case 1: p[i] = Util.UnScalar(x).ToString(); break;
                    case 2: p[i] = ResolvePointer(e[i] as EventOffset); break;
                    case 3: p[i] = (e[i].Data != 0 ? "true" : "false"); break;
                    case 4: p[i] = Util.Hex(x); break;
                    case 5: p[i] = ResolveVariable(x); break;
                    case 6: p[i] = GetRequirement(x); break;
                }
            }
            return p;
        }
        //Return the name of the external pointer corresponding to the address if 
        //one is available, otherwise return the string of the value passed.
        public string ResolvePointer(EventOffset node)
        {
            if (node._externalEntry != null)
                return "External: " + node._externalEntry.Name;

            if (node._offsetInfo._list == ListValue.Null)
                return "0x" + Util.Hex(node.Data);
            else
            {
                string name = "", t = "", grp = "";
                switch (node._offsetInfo._list)
                {
                    case ListValue.Actions:
                        grp = "Action ";
                        switch (node._offsetInfo._type)
                        {
                            case TypeValue.Entry: t = "Entry"; break;
                            case TypeValue.Exit: t = "Exit"; break;
                        }
                        break;
                    case ListValue.SubActions:
                        grp = "SubAction ";
                        switch (node._offsetInfo._type)
                        {
                            case TypeValue.Main: t = "Main"; break;
                            case TypeValue.GFX: t = "GFX"; break;
                            case TypeValue.SFX: t = "SFX"; break;
                            case TypeValue.Other: t = "Other"; break;
                        }
                        break;
                    case ListValue.SubRoutines:
                        grp = "SubRoutine ";
                        name = node.Index.ToString();
                        break;
                    case ListValue.ScreenTints:
                        grp = "Screen Tint ";
                        break;
                    case ListValue.FlashOverlays:
                        grp = "Flash Overlay ";
                        break;
                }

                return grp + node._offsetInfo._index.ToString() + ((int)node._offsetInfo._list >= 2 ? "" : " " + t);
            }
        }
         
        //Return the full name of the variable corresponding to the value passed.
        public string ResolveVariable(long value)
        {
            string variableName = "";
            long variableMemType = (value & 0xF0000000) / 0x10000000;
            long variableType = (value & 0xF000000) / 0x1000000;
            long variableNumber = (value & 0xFFFFFF);
            if (variableMemType == 0) variableName = "IC-";
            if (variableMemType == 1) variableName = "LA-";
            if (variableMemType == 2) variableName = "RA-";
            if (variableType == 0) variableName += "Basic";
            if (variableType == 1) variableName += "Float";
            if (variableType == 2) variableName += "Bit";
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
                        //if (TargetNode.Parent != null && TargetNode.Parent.Parent != null && TargetNode.Parent.Parent.Name.StartsWith("Article"))
                        //{
                        //    ResourceNode sa = TargetNode.Parent.Parent.FindChild("SubActions", false);
                        //    if (sa != null)
                        //        return sa.Children[(int)value].Name;
                        //}
                        //else 
                        {
                            DataSection data = ((MovesetNode)TargetNode._root).Data;
                            if (data != null && data.SubActions != null && value < data.SubActions.Count && value >= 0)
                                return data.SubActions[(int)value].Name;
                            else
                                return ((int)value).ToString();
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

            if (requirement > Manager.iRequirements.Length)
                return Util.Hex(requirement);

            if (not == true)
                return "Not " + Manager.iRequirements[requirement];

            return Manager.iRequirements[requirement];
        }
        public EventInformation GetEventInfo(long id)
        {
            if (Manager.Events.ContainsKey(id))
                return Manager.Events[id];

            return new EventInformation(id, id.ToString("X"), "No Description Available.", null, null);
        }

        //Return the event name followed by each parameter paired with its type.
        public string GetDefaultSyntax(Event e)
        {
            string script = GetEventInfo(e.EventID)._name + (e.Count > 0 ? ": " : "");
            for (int i = 0; i < e.Count; i++)
            {
                script += e[i].ParamType + "(";
                switch ((int)e[i].ParamType)
                {
                    case 0: script += GetValue(e.EventID, i, e[i].Data); break;
                    case 1: script += Util.UnScalar(e[i].Data).ToString(); break;
                    case 2: script += ResolvePointer(e[i] as EventOffset); break;
                    case 3: script += (e[i].Data != 0 ? "true" : "false"); break;
                    case 4: script += e[i].Data; break;
                    case 5: script += ResolveVariable(e[i].Data); break;
                    case 6: script += GetRequirement(e[i].Data); break;
                }
                script += ")";
                if (i != e.Count)
                    script += ", ";
            }
            return script;
        }

        //Return the passed syntax with all keywords replaced with their proper values.
        public string ResolveEventSyntax(string syntax, Event e)
        {
            while (true)
            {
                string keyword = "";
                string keyResult = "";
                string strParams = "";
                string[] kParams;

                int keyBegin = Util.FindFirst(syntax, 0, '\\');
                if (keyBegin == -1) break;

                int keyEnd = Util.FindFirst(syntax, keyBegin, '(');
                if (keyEnd == -1) keyEnd = syntax.Length;

                int paramsBegin = keyEnd + 1;

                int paramsEnd = Util.FindFirstIgnoreNest(syntax, paramsBegin, ')');
                if (paramsEnd == -1) paramsEnd = syntax.Length;

                keyword = syntax.Substring(keyBegin, keyEnd - keyBegin);

                strParams = syntax.Substring(paramsBegin, paramsEnd - paramsBegin);
                kParams = GetParameters(strParams, e);

                keyResult = ResolveKeyword(keyword, kParams, e);

                syntax = Util.DelSubstring(syntax, keyBegin, (paramsEnd + 1) - keyBegin);
                syntax = Util.InsString(syntax, keyResult, keyBegin);
            }

            return syntax;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            int highest = EventList.Items.Count;
            if (EventList.SelectedIndex != -1)
                highest = EventList.SelectedIndices[EventList.SelectedIndices.Count - 1];

            Event d = new Event();
            if (highest == EventList.Items.Count)
                TargetNode.Add(d);
            else
                TargetNode.Insert(highest + 1, d);
            TargetNode.SignalRebuildChange();
            d.EventID = 0x00020000;
            MakeScript();

            if (EventList.Items.Count > highest + 1)
                EventList.SelectedIndex = highest + 1;
            else
                EventList.SelectedIndex = EventList.Items.Count - 1;
        }

        private void btnModify_Click(object sender, EventArgs e)
        {
            if (EventList.SelectedIndex != -1)
                if (_mainWindow != null)
                {
                    _mainWindow.SelectedEvent = TargetNode[EventList.SelectedIndex];
                    _mainWindow.ModifyEvent();
                }
                else
                {
                    FormModifyEvent p = new FormModifyEvent();
                    p.eventModifier.Setup(TargetNode[EventList.SelectedIndex]);
                    if (p.ShowDialog() == DialogResult.OK)
                        MakeScript();
                }
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            int[] indices = new int[EventList.SelectedIndices.Count];
            EventList.SelectedIndices.CopyTo(indices, 0);
            for (int i = indices.Length - 1; i >= 0; i--)
                TargetNode.RemoveAt(indices[i]);
            MakeScript();
            if (TargetNode != null && indices.Length == 1)
                foreach (int i in indices)
                    if (i - indices.Length >= 0)
                        EventList.SetSelected(i - indices.Length, true);
                    else if (EventList.Items.Count > 0)
                        EventList.SetSelected(0, true);
        }

        private void btnUp_Click(object sender, EventArgs e)
        {
            int lowest = -1;
            if (EventList.SelectedIndex != -1)
                lowest = EventList.SelectedIndices[0];
            int[] indices = new int[EventList.SelectedIndices.Count];
            EventList.SelectedIndices.CopyTo(indices, 0);
            if (lowest != -1 && lowest != 0)
            {
                //TODO
                //for (int i = 0; i < EventList.SelectedIndices.Count; i++)
                //    TargetNode[EventList.SelectedIndices[i]].DoMoveUp(false);
                MakeScript();
                if (TargetNode != null)
                    foreach (int i in indices)
                        EventList.SetSelected(i - 1, true);
            }
        }

        private void btnDown_Click(object sender, EventArgs e)
        {
            int highest = -1;
            if (EventList.SelectedIndex != -1)
                highest = EventList.SelectedIndices[EventList.SelectedIndices.Count - 1];
            int[] indices = new int[EventList.SelectedIndices.Count];
            EventList.SelectedIndices.CopyTo(indices, 0);
            if (highest != -1 && highest != EventList.Items.Count - 1)
            {
                //TODO
                //for (int i = EventList.SelectedIndices.Count - 1; i >= 0; i--)
                //    TargetNode[EventList.SelectedIndices[i]].DoMoveDown(false);
                MakeScript();
                if (TargetNode != null)
                    foreach (int i in indices)
                        EventList.SetSelected(i + 1, true);
            }
        }

        private void btnCopy_Click(object sender, EventArgs e)
        {
            string s = "";
            foreach (int i in EventList.SelectedIndices)
            {
                s += TargetNode[i].Serialize();
                s += "/";
            }
            if (!String.IsNullOrEmpty(s))
                Clipboard.SetText(s);
        }

        private void btnPaste_Click(object sender, EventArgs e)
        {
            List<int> indices = new List<int>();

            int highest = EventList.Items.Count;
            if (EventList.SelectedIndex != -1)
                highest = EventList.SelectedIndices[EventList.SelectedIndices.Count - 1];

            string s = Clipboard.GetText();

            try
            {
                string[] Events = s.Split('/');
                foreach (string x in Events)
                {
                    Event y = Event.Deserialize(x, TargetNode._root as MovesetNode);
                    if (y != null)
                    {
                        if (highest == TargetNode.Count)
                            TargetNode.Add(y);
                        else
                            TargetNode.Insert(highest + 1, y);
                        indices.Add(y.Index);
                        highest++;
                    }
                }
            }
            finally
            {
                MakeScript(); 
                if (TargetNode != null)
                    foreach (int i in indices)
                        EventList.SetSelected(i, true);
            }
        }

        private void EventList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (EventList.SelectedIndex >= 0)
            {
                EventInformation info = (TargetNode[EventList.SelectedIndex]).Info;
                if (info != null && !String.IsNullOrEmpty(info._description))
                    description.Text = info._description;
                else
                    description.Text = "No Description Available.";
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
                s += EventList.Items[i].ToString() + Environment.NewLine;
            if (!String.IsNullOrEmpty(s))
                Clipboard.SetText(s);
        }

        private void btnCut_Click(object sender, EventArgs e)
        {
            btnCopy_Click(sender, e);
            btnRemove_Click(sender, e);
        }
    }
}
