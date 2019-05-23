using BrawlLib.SSBB.ResourceNodes;
using BrawlLib.SSBBTypes;
using Ikarus.ModelViewer;
using Ikarus.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Ikarus.MovesetFile
{
    public unsafe class MovesetNode : SakuraiArchiveNode
    {
        public MovesetNode(CharName character) { _character = character; }

        /// <summary>
        /// Returns the name of the character this moveset is for.
        /// This can be directed converted into CharFolder for easy access to necessary files
        /// </summary>
        [Browsable(false)]
        public CharName Character { get { return _character; } }
        private CharName _character;

        /// <summary>
        /// The section that contains all information for a character's specific moveset.
        /// </summary>
        public DataSection Data { get { return _data; } }
        private DataSection _data = null;
        /// <summary>
        /// The section that contains all common information located in Fighter.pac
        /// </summary>
        public DataCommonSection DataCommon { get { return _dataCommon; } }
        private DataCommonSection _dataCommon = null;
        /// <summary>
        /// The the first section that contains information for item movesets.
        /// </summary>
        public AnimParamSection AnimParam { get { return _animParam; } }
        private AnimParamSection _animParam = null;
        /// <summary>
        /// The the second section that contains information for item movesets.
        /// </summary>
        //public SubParamSection SubParam { get { return _subParam; } }
        //private SubParamSection _subParam = null;

        /// <summary>
        /// A list of all action scripts referenced only through offset events.
        /// </summary>
        [Browsable(false)]
        public BindingList<Script> SubRoutines { get { return _subRoutines; } }
        private BindingList<Script> _subRoutines;

        /// <summary>
        /// A list of all action scripts referenced in the section list.
        /// </summary>
        [Browsable(false)]
        public BindingList<Script> CommonSubRoutines { get { return _commonSubRoutines; } }
        private BindingList<Script> _commonSubRoutines;

        [Browsable(false)]
        public BindingList<ActionEntry> Actions { get { return _actions; } }
        internal BindingList<ActionEntry> _actions;

        /// <summary>
        /// Provides easy access to the model that this moveset will affect.
        /// </summary>
        public MDL0Node Model { get { return MainForm.Instance._mainControl.TargetModel as MDL0Node; } }
        internal List<List<int>>[] _scriptOffsets;

        protected override void InitData(SakuraiArchiveHeader* hdr)
        {
            base.InitData(hdr);

            _subRoutines = new BindingList<Script>();
            _commonSubRoutines = new BindingList<Script>();
            _actions = new BindingList<ActionEntry>();
            _scriptOffsets = new List<List<int>>[5];
            for (int i = 0; i < 5; i++)
            {
                _scriptOffsets[i] = new List<List<int>>();
                for (int x = 0; x < (i == 0 ? 2 : i == 1 ? 4 : 1); x++)
                    _scriptOffsets[i].Add(new List<int>());
            }
        }

        protected override void PostParse()
        {
            base.PostParse();

            //Sort subroutines by offset
            _subRoutines = new BindingList<Script>(_subRoutines.OrderBy(x => x._offset).ToList());
            //Add the proper information to the sorted subroutines
            int q = 0;
            foreach (Script s in _subRoutines)
            {
                s._name = String.Format("[{0}] SubRoutine", s._index = q++);
                foreach (EventOffset e in s.ActionRefs)
                    e._offsetInfo = new ScriptOffsetInfo(ListValue.SubRoutines, TypeValue.None, s.Index);
            }
        }

        //protected override void ParseInternals(SakuraiArchiveHeader* hdr)
        //{
        //    base.ParseInternals(hdr);
        //}

        protected override TableEntryNode GetTableEntryNode(string name, int index)
        {
            TableEntryNode section = null;
            if (name.Contains("AnimCmd"))
            {
                section = new Script();
                //_commonSubRoutines.Add(entry as Script);
            }

            return section;
        }

        protected override void HandleSpecialSections(List<TableEntryNode> sections)
        {
            foreach (TableEntryNode section in sections)
            {
                if (section is DataSection)
                    _data = (DataSection)section;
                else if (section is DataCommonSection)
                    _dataCommon = (DataCommonSection)section;
            }
        }

        /// <summary>
        /// Use this only when parsing.
        /// </summary>
        public Script GetScript(int offset)
        {
            if (!_initializing)
                throw new Exception("Not initializing.");

            if (offset < 0)
                return null;

            SakuraiEntryNode e = GetEntry(offset);

            if (e is Script)
                return e as Script;
            else if (e is Event)
                return ((Event)e)._script;

            return null;
        }
        /// <summary>
        /// Use this only when parsing.
        /// </summary>
        internal ScriptOffsetInfo GetScriptLocation(int offset)
        {
            if (!_initializing)
                throw new Exception("Not initializing.");

            //Create new offset info
            ScriptOffsetInfo info = new ScriptOffsetInfo();

            //Check if the offset is legit
            if (offset <= 0)
                return info;

            info._list = ListValue.Actions;

            //Search action offsets
            for (info._type = 0; (int)info._type < 2; info._type++)
                if ((info._index = _scriptOffsets[(int)info._list][(int)info._type].IndexOf(offset)) != -1)
                    return info;

            info._list++;

            //Search subaction offsets
            for (info._type = 0; (int)info._type < 4; info._type++)
                if ((info._index = _scriptOffsets[(int)info._list][(int)info._type].IndexOf(offset)) != -1)
                    return info;

            info._type = TypeValue.None;
            info._list++;

            //Search subroutine offsets
            if ((info._index = _scriptOffsets[(int)info._list][0].IndexOf(offset)) != -1)
                return info;

            info._list++;

            //Search reference entry offsets
            SakuraiEntryNode e = GetEntry(offset);
            if (e is TableEntryNode && e != null)
            {
                info._index = e.Index;
                return info;
            }

            //Set values to null
            info._list++;
            info._type = TypeValue.None;
            info._index = -1;

            //Continue searching dataCommon
            if (_dataCommon != null)
            {
                info._list++;

                //Search screen tint offsets
                if ((info._index = _scriptOffsets[3][0].IndexOf(offset)) != -1)
                    return info;

                info._list++;

                //Search flash overlay offsets
                if ((info._index = _scriptOffsets[4][0].IndexOf(offset)) != -1)
                    return info;

                info._list = ListValue.Null;
            }
            return info;
        }

        /// <summary>
        /// Returns the script at the given location.
        /// </summary>
        public Script GetScript(ScriptOffsetInfo info)
        {
            ListValue list = info._list;
            TypeValue type = info._type;
            int index = info._index;

            if ((list > ListValue.References && _dataCommon == null) || list == ListValue.Null || index == -1)
                return null;

            switch (list)
            {
                case ListValue.Actions:
                    if ((type == TypeValue.Entry || type == TypeValue.Exit) && index >= 0 && index < Actions.Count)
                        return (Script)Actions[index].GetWithType((int)type);
                    break;
                case ListValue.SubActions:
                    if (_data != null && index >= 0 && index < _data.SubActions.Count)
                        return (Script)_data.SubActions[index].GetWithType((int)type);
                    break;
                case ListValue.SubRoutines:
                    if (index >= 0 && index < _subRoutines.Count)
                        return (Script)_subRoutines[index];
                    break;
                case ListValue.FlashOverlays:
                    if (_dataCommon != null && index >= 0 && index < _dataCommon.FlashOverlays.Count)
                        return (Script)_dataCommon.FlashOverlays[index];
                    break;
                case ListValue.ScreenTints:
                    if (_dataCommon != null && index >= 0 && index < _dataCommon.ScreenTints.Count)
                        return (Script)_dataCommon.ScreenTints[index];
                    break;
            }
            return null;
        }
        /// <summary>
        /// Characters like kirby and wario need to swap bone indices. 
        /// Use this function to get the proper index.
        /// </summary>
        public void GetBoneIndex(ref int boneIndex)
        {
            //if (Character == CharName.Wario || Character == CharName.Kirby)
            //{
            //    if (_data != null)
            //        if (_data.warioParams8 != null)
            //        {
            //            RawParamList p1 = _data.warioParams8.Children[0] as RawParamList;
            //            RawParamList p2 = _data.warioParams8.Children[1] as RawParamList;
            //            bint* values = (bint*)p2.AttributeBuffer.Address;
            //            int i = 0;
            //            for (; i < p2.AttributeBuffer.Length / 4; i++)
            //                if (values[i] == boneIndex)
            //                    break;
            //            if (p1.AttributeBuffer.Length / 4 > i)
            //            {
            //                int value = -1;
            //                if ((value = (int)(((bint*)p1.AttributeBuffer.Address)[i])) >= 0)
            //                {
            //                    boneIndex = value;
            //                    return;
            //                }
            //                else
            //                    boneIndex -= 400;
            //            }
            //        }
            //}
        }
        /// <summary>
        /// Characters like kirby and wario need to swap bone indices. 
        /// Use this function to set the proper index.
        /// </summary>
        public void SetBoneIndex(ref int boneIndex)
        {
            //if (Character == CharName.Wario || Character == CharName.Kirby)
            //{
            //    if (_data != null)
            //        if (_data.warioParams8 != null)
            //        {
            //            RawParamList p1 = _data.warioParams8.Children[0] as RawParamList;
            //            RawParamList p2 = _data.warioParams8.Children[1] as RawParamList;
            //            bint* values = (bint*)p2.AttributeBuffer.Address;
            //            int i = 0;
            //            for (; i < p1.AttributeBuffer.Length / 4; i++)
            //                if (values[i] == boneIndex)
            //                    break;
            //            if (p2.AttributeBuffer.Length / 4 > i)
            //            {
            //                int value = -1;
            //                if ((value = ((bint*)p2.AttributeBuffer.Address)[i]) >= 0)
            //                {
            //                    boneIndex = value;
            //                    return;
            //                }
            //            }
            //        }
            //}
        }
    }

    public unsafe class MovesetEntryNode : SakuraiEntryNode
    {
        [Browsable(false)]
        public MDL0Node Model
        {
            get
            {
                ArticleNode article = ParentArticle;
                if (article != null)
                {
                    if (article._info != null)
                        return article._info._model;
                }
                else if (_root != null)
                    return ((MovesetNode)_root).Model;

                return null;
            }
        }

        [Browsable(false)]
        public ArticleNode ParentArticle
        {
            get
            {
                SakuraiEntryNode n = _parent;
                while (!(n is ArticleNode) && n != null)
                    n = n._parent;
                return n as ArticleNode;
            }
        }
    }
}
