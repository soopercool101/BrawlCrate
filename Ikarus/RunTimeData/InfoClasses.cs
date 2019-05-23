using BrawlLib.SSBB.ResourceNodes;
using Ikarus.MovesetFile;
using System;
using System.Audio;
using System.Collections.Generic;
using System.ComponentModel;
using Ikarus.UI;
using BrawlLib.SSBBTypes;
using BrawlLib.OpenGL;

namespace Ikarus.ModelViewer
{
    /// <summary>
    /// Info for displaying and animating articles.
    /// </summary>
    public class ArticleInfo
    {
        //Articles are like models with their own mini-movesets.
        //There are a lot of things here that are copied directly from the main window

        [Browsable(false)]
        public ArticleNode _article;
        [Browsable(false)]
        public MDL0Node _model;
        [Browsable(false)]
        public Dictionary<int, CHR0Node> _chr0List;
        [Browsable(false)]
        public Dictionary<int, SRT0Node> _srt0List;
        [Browsable(false)]
        public Dictionary<int, SHP0Node> _shp0List;
        [Browsable(false)]
        public Dictionary<int, VIS0Node> _vis0List;
        [Browsable(false)]
        public Dictionary<int, PAT0Node> _pat0List;
        [Browsable(false)]
        public Dictionary<int, CLR0Node> _clr0List;

        public int _animFrame = 0, _maxFrame, _setAt;

        public CHR0Node _chr0;
        public SRT0Node _srt0;
        public SHP0Node _shp0;
        public PAT0Node _pat0;
        public VIS0Node _vis0;
        public CLR0Node _clr0;

        public SubActionEntry _currentSubaction = null;
        public ActionEntry _currentAction = null;
        public SubActionEntry CurrentSubaction
        {
            get { return _currentSubaction; }
            set { _currentSubaction = value; }
        }

        public ActionEntry CurrentAction
        {
            get { return _currentAction; }
            set { _currentAction = value; }
        }

        private void LoadSubactionScripts()
        {
            if (CurrentSubaction != null)
            {
                for (int i = 0; i < RunTime._runningScripts.Count; i++)
                    if (RunTime._runningScripts[i].ParentArticle != null)
                        RunTime._runningScripts.RemoveAt(i);
                
                foreach (Script script in CurrentSubaction.GetScriptArray())
                    if (script != null)
                    {
                        RunTime._runningScripts.Add(script);
                        script.Reset();
                    }
            }
        }

        public void ResetSubactionVariables()
        {
            //Reload scripts
            LoadSubactionScripts();

            SubactionIndex = -1; //Reset the current animation
            Running = !_etcModel; //Set default visibility
        }

        private bool _running = false;
        public bool _etcModel = true;
        public int CurrentFrame
        {
            get { return _animFrame; }
            set
            {
                _animFrame = value;

                UpdateModel();
            }
        }

        public void ProgressFrame()
        {
            CurrentFrame = _animFrame + 1;
        }

        public void SetFrame(int index)
        {
            CurrentFrame = index - _setAt;
        }

        private int _subaction = -1;
        public int SubactionIndex 
        {
            get { return _subaction; } 
            set
            {
                if ((_subaction = value) >= 0 && _article._subActions != null && _subaction < _article._subActions.Count)
                {
                    CurrentSubaction = _article._subActions[_subaction] as SubActionEntry;
                    if (CurrentSubaction != null)
                    {
                        int index = CurrentSubaction.Index;
                        _chr0 = _chr0List.ContainsKey(index) ? _chr0List[index] : null;
                        _srt0 = _srt0List.ContainsKey(index) ? _srt0List[index] : null;
                        _pat0 = _pat0List.ContainsKey(index) ? _pat0List[index] : null;
                        _vis0 = _vis0List.ContainsKey(index) ? _vis0List[index] : null;
                        _shp0 = _shp0List.ContainsKey(index) ? _shp0List[index] : null;
                        _clr0 = _clr0List.ContainsKey(index) ? _clr0List[index] : null;

                        _maxFrame = _chr0.FrameCount;
                    }
                    else
                    {
                        _chr0 = null; _srt0 = null; _vis0 = null; _pat0 = null; _shp0 = null; _clr0 = null;
                        _maxFrame = 0;
                    }
                }
                else
                {
                    _chr0 = null; _srt0 = null; _vis0 = null; _pat0 = null; _shp0 = null; _clr0 = null;
                    _maxFrame = 0;
                }
                CurrentFrame = 0;
            }
        }

        public bool Running 
        {
            get { return _running; } 
            set { ModelVisible = _running = value; }
        }

        public bool ModelVisible
        {
            get { return _model == null ? false : _model.IsRendering; }
            set
            {
                if (_model != null && (_model.IsRendering = value))
                {
                    _model.ResetToBindState();

                    //Reset model visiblity to its default state
                    if (_model != null && _model._objList != null)
                        if (_article._mdlVis != null)
                            _article._mdlVis.ResetVisibility();
                        else
                            foreach (DrawCall b in _model.DrawCalls)
                                b._render = b.VisibilityBoneNode == null ? true : b.VisibilityBoneNode._boneFlags.HasFlag(BoneFlags.Visible);
                }
            }
        }

        public void UpdateModel()
        {
            if (_model == null)
                return;

            MainControl ctrl = MainForm.Instance._mainControl;
            int frame = _animFrame + 1;

            _model.ApplyCHR(ctrl.PlayCHR0 ? _chr0 : null, frame);
            _model.ApplySRT(ctrl.PlaySRT0 ? _srt0 : null, frame);
            _model.ApplySHP(ctrl.PlaySHP0 ? _shp0 : null, frame);
            _model.ApplyPAT(ctrl.PlayPAT0 ? _pat0 : null, frame);
            _model.ApplyVIS(ctrl.PlayVIS0 ? _vis0 : null, frame);
            _model.ApplyCLR(ctrl.PlayCLR0 ? _clr0 : null, frame);
        }

        public ArticleInfo(ArticleNode article, MDL0Node model, bool running)
        {
            _article = article;
            _model = model;
            _running = running;

            _chr0List = new Dictionary<int, CHR0Node>();
            _srt0List = new Dictionary<int, SRT0Node>();
            _shp0List = new Dictionary<int, SHP0Node>();
            _vis0List = new Dictionary<int, VIS0Node>();
            _pat0List = new Dictionary<int, PAT0Node>();
            _clr0List = new Dictionary<int, CLR0Node>();

            _article._info = this;
        }

        public override string ToString()
        {
            return String.Format("[{0}] {1}", _article.Index, _model == null ? "Article" : _model.Name);
        }
    }

    /// <summary>
    /// Info for properly managing audio streams.
    /// </summary>
    public class AudioInfo
    {
        public AudioBuffer _buffer;
        public IAudioStream _stream;

        public AudioInfo(AudioBuffer buffer, IAudioStream stream)
        {
            _buffer = buffer;
            _stream = stream;
        }
    }

    public class RequirementInfo
    {
        public RequirementInfo(int req)
        {
            _requirement = req;
        }

        public int _requirement;
        public List<Parameter> _values = new List<Parameter>();
    }

    /// <summary>
    /// Info for executing an 'if' event successfully.
    /// </summary>
    public class IfInfo
    {
        //Indices of the first event of each if case in the sequence
        public List<int> _reqIndices = new List<int>();

        //List of requirements for each if statement (if, else if)
        //"And" event adds requirements to a requirement list
        public List<List<RequirementInfo>> _requirements = new List<List<RequirementInfo>>();

        //Event index of the first event of the else event, if there is an else event
        //Run code after this event if all other requirements are false
        public int _elseIndex = -1;

        //Index of the the endif
        //Go past here to end the if cases
        public int _endIndex;

        /// <summary>
        /// Runs all requirements and returns the event index of the code to be executed.
        /// </summary>
        /// <returns></returns>
        public int Run()
        {
            int index = 0;
            foreach (List<RequirementInfo> list in _requirements)
            {
                bool failed = false;
                foreach (RequirementInfo req in list)
                {
                    bool isTrue = Scriptor.ApplyRequirement(req);
                    if (!isTrue)
                    {
                        failed = true;
                        break;
                    }
                }
                if (!failed)
                    return _reqIndices[index];
                index++;
            }
            if (_elseIndex >= 0)
                return _elseIndex;
            return _endIndex;
        }
    }

    public class ActionChangeInfo
    {
        public bool _enabled = true;
        public bool _prioritized = true;
        public uint _statusID; //Used to enable 
        public int _newID = 0;
        public List<RequirementInfo> _requirements;

        public ActionChangeInfo(int newID)
        {
            _newID = newID;
        }

        public bool Evaluate()
        {
            if (!_enabled)
                return false;

            foreach (RequirementInfo i in _requirements)
                if (!Scriptor.ApplyRequirement(i))
                    return false;

            return true;
        }
    }

    public class SubActionChangeInfo
    {
        public int _newID = 0;
        public List<RequirementInfo> _requirements;
        public bool _passFrame;

        public SubActionChangeInfo(int newID, bool passFrame)
        {
            _newID = newID;
            _passFrame = passFrame;
        }

        public bool Evaluate()
        {
            foreach (RequirementInfo i in _requirements)
                if (!Scriptor.ApplyRequirement(i))
                    return false;

            return true;
        }
    }

    public class ScriptOffsetInfo
    {
        public ListValue _list;
        public TypeValue _type;
        public int _index;

        public ScriptOffsetInfo() { _list = ListValue.Null; _type = TypeValue.None; _index = -1; }
        public ScriptOffsetInfo(ListValue l, TypeValue t, int i)
        {
            _list = l;
            _type = t;
            _index = i;
        }
    }
}
