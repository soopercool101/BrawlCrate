using BrawlLib.SSBB.ResourceNodes;
using Ikarus.MovesetFile;
using System.Windows.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using WiimoteLib;
using Ikarus.UI;

namespace Ikarus.ModelViewer
{
    /// <summary>
    /// A static class containing universally available variables and functions for what is going on in the viewer.
    /// </summary>
    public static class RunTime
    {
        public static bool _IsRoot;
        static RunTime()
        {
            Timer.RenderFrame += RenderFrame;
            Timer.UpdateFrame += UpdateFrame;
        }

        private static RunTimeAccessor _instance;
        public static RunTimeAccessor Instance { get { return _instance == null ? _instance = new RunTimeAccessor() : _instance; } }

        public static MainControl MainWindow { get { return MainForm.Instance._mainControl; } }
        public static ScriptPanel ScriptWindow { get { return MainWindow.MovesetPanel; } }

        public static BindingList<string> _log = new BindingList<string>();
        public static void Log(string message) 
        {
            _log.Add(message);
            Console.WriteLine(message);
        }
        public static void ClearLog() { _log.Clear(); }

        public static float GetVar(VariableType var, VarMemType mem, int num)
        {
            switch (mem)
            {
                case VarMemType.IC: return IC.Get(var, num);
                case VarMemType.LA: return LA.Get(var, num);
                case VarMemType.RA: return RA.Get(var, num);
            }
            return 0.0f;
        }
        public static void SetVar(VariableType var, VarMemType mem, int num, double value)
        {
            switch (mem)
            {
                case VarMemType.IC: break;
                case VarMemType.LA: LA.Set(var, num, value); break;
                case VarMemType.RA: RA.Set(var, num, value); break;
            }
        }

        private static List<ActionChangeInfo> _actionChanges = new List<ActionChangeInfo>();
        private static List<SubActionChangeInfo> _subActionChanges = new List<SubActionChangeInfo>();
        public static ActionChangeInfo GetActionChangeInfo(uint statusID)
        {
            foreach (ActionChangeInfo a in _actionChanges)
                if (a._statusID == statusID)
                    return a;
            return null;
        }
        
        public static void AddActionChangeInfo(ActionChangeInfo info)
        {
            _actionChanges.Add(info);

            //Is sorting "prioritized" action changes needed, or is it not actually prioritized at all?
            //If they are prioritized, then where do action changes without a status ID fit in to the sorting, first or last?
            //_actionChanges = _actionChanges.OrderBy(x => x._statusID).ToList();
        }
        public static void AddSubActionChangeInfo(SubActionChangeInfo info)
        {
            _subActionChanges.Add(info);
        }

        public static bool _allowInterrupt = false;
        public static Location _location = Location.Ground;
        public enum Location
        {
            Air = 0,
            Ground = 1
        }

        public static Dictionary<int, Script> _concurrentLoopScripts = new Dictionary<int, Script>();
        public static List<Script> _runningScripts = new List<Script>();
        public static Dictionary<int, List<AudioInfo>> _playingSounds = new Dictionary<int, List<AudioInfo>>();
        public static ArticleInfo[] _articles;
        public static List<HitBox> _hitBoxes = new List<HitBox>();

#if DEBUG
        public static bool _muteSFX = true;
#else
        public static bool _muteSFX = false;
#endif

        public static CoolTimer Timer { get { return MainWindow._timer; } }
        public static bool IsRunning { get { return Timer.IsRunning; } }
        private static void Run()
        {
            if (CurrentFrame >= MaxFrame)
                SetFrame(1);

            //LoadSubactionScripts();
            //ResetSubactionVariables();
            //SetFrame(CurrentFrame);

            //Run the timer.
            //This will run scripts and animations until the user stops it or everything ends itself.
            //Then the code directly after this line will be executed.
            Playing = true;
            Timer.Run(0, FramesPerSecond);

            //This next section of code will not run until the timer stops.
            //If we have any sounds still playing, stop and dispose of them.
            //This should only be necessary if the user manually stops the timer,
            //as the timer will not stop automatically until all sounds are completed and disposed of.
            if (_playingSounds.Count != 0)
            {
                List<int> keys = new List<int>();
                foreach (var b in _playingSounds)
                    foreach (AudioInfo info in b.Value)
                        if (info._buffer != null && info._buffer.Owner != null)
                        {
                            info._buffer.Stop();
                            info._buffer.Dispose();
                            info._stream.Dispose();
                        }
                _playingSounds.Clear();
            }
        }
        private static void Stop()
        {
            Timer.Stop(); 
            Playing = false; 
            //if (MainWindow._capture)
            //{
            //    MainWindow.RenderToGIF(MainWindow.images.ToArray());
            //    MainWindow.images.Clear();
            //    MainWindow._capture = false;
            //}
        }

        public static void TogglePlay()
        {
            if (IsRunning)
                Stop();
            else
                Run();
        }

        /// <summary>
        /// This determines how many frames should be rendered per second.
        /// </summary>
        public static double FramesPerSecond 
        {
            get { return Timer.TargetRenderFrequency; }
            set { Timer.TargetRenderFrequency = value; }
        }

        /// <summary>
        /// This determines how many updates should take place per second.
        /// Updates include things like executing button input and action changes.
        /// </summary>
        public static double UpdatesPerSecond
        {
            get { return Timer.TargetUpdateFrequency; }
            set { Timer.TargetUpdateFrequency = value; }
        }

        public static bool _passFrame = false;
        
        private static SubActionEntry _currentSubaction;
        private static ActionEntry _currentAction, _prevAction;
        private static Script _currentSubRoutine;

        public static BindingList<SubActionEntry> Subactions { get { return Manager.Moveset == null || Manager.Moveset.Data == null ? null : Manager.Moveset.Data.SubActions; } }
        public static ActionOverrideList EntryOverrides { get { return Manager.Moveset == null || Manager.Moveset.Data == null ? null : Manager.Moveset.Data._entryOverrides; } }
        public static ActionOverrideList ExitOverrides { get { return Manager.Moveset == null || Manager.Moveset.Data == null ? null : Manager.Moveset.Data._exitOverrides; } }
        public static BindingList<CommonAction> FlashOverlays { get { return Manager.CommonMoveset == null || Manager.CommonMoveset.DataCommon == null ? null : Manager.CommonMoveset.DataCommon.FlashOverlays; } }
        public static BindingList<CommonAction> ScreenTints { get { return Manager.CommonMoveset == null || Manager.CommonMoveset.DataCommon == null ? null : Manager.CommonMoveset.DataCommon.ScreenTints; } }
        public static BindingList<ActionEntry> CommonActions { get { return Manager.CommonMoveset == null ? null : Manager.CommonMoveset.Actions; } }
        public static BindingList<ActionEntry> Actions { get { return Manager.Moveset == null ? null : Manager.Moveset.Actions; } }
        public static BindingList<Script> Subroutines { get { return Manager.Moveset == null ? null : Manager.Moveset.SubRoutines; } }
        public static BindingList<Script> CommonSubroutines { get { return Manager.CommonMoveset == null ? null : Manager.CommonMoveset.CommonSubRoutines; } }
        
        public static int CurrentSubactionIndex
        {
            get { return _currentSubaction == null ? -1 : _currentSubaction.ID; }
            set
            {
                if (Subactions == null)
                    return;
                if (value >= 0 && value < Subactions.Count)
                    CurrentSubaction = Subactions[value];
            }
        }
        public static int CurrentActionIndex
        {
            get { return _currentAction == null ? -1 : _currentAction.ID; }
            set
            {
                if (value >= 274)
                {
                    if (Actions == null)
                        return;

                    if (value < Actions.Count)
                        CurrentAction = Actions[value];
                }
                else if (value >= 0)
                {
                    if (CommonActions == null)
                        return;

                    CurrentAction = CommonActions[value];
                }
            }
        }
        public static SubActionEntry CurrentSubaction
        {
            get { return _currentSubaction; }
            set
            {
                if (_currentSubaction == value)
                    return;

                _currentSubaction = value;

                //Reset all of the subaction-dependent variables
                ResetSubactionVariables();

                ScriptWindow.SubactionGroupChanged();

                UpdateCharPos();
            }
        }

        public static ActionEntry PreviousAction
        {
            get { return _prevAction; }
            set { _prevAction = value; }
        }
        public static ActionEntry CurrentAction
        {
            get { return _currentAction; }
            set
            {
                _prevAction = _currentAction;
                _currentAction = value;
                ScriptWindow.ActionGroupChanged();
            }
        }

        public static Script CurrentSubRoutine
        {
            get { return _currentSubRoutine; }
            set
            {
                _currentSubRoutine = value;
                ScriptWindow.SubRoutineChanged();
            }
        }

        public static void LoadSubactionScripts()
        {
            foreach (Script a in _runningScripts) 
                a.Reset(); 

            _runningScripts.Clear();
            if (CurrentSubaction != null)
                foreach (Script a in CurrentSubaction.GetScriptArray()) 
                {
                    _runningScripts.Add(a); 
                    a.Reset();
                }
        }

        public static void ResetSubactionVariables()
        {
            LoadSubactionScripts();

            foreach (MDL0BoneNode bone in Scriptor._boneCollisions)
                bone._nodeColor = bone._boneColor = Color.Transparent;
            Scriptor._boneCollisions = new List<MDL0BoneNode>();
            Scriptor._hurtBoxType = 0;
            _hitBoxes = new List<HitBox>();
            _allowInterrupt = false;

            //Reset articles
            if (_articles != null)
                foreach (ArticleInfo i in _articles)
                    if (i != null)
                        i.ResetSubactionVariables();

            //Reset model visiblity to its default state
            if (MainWindow.TargetModel != null && 
                MainWindow.TargetModel.Objects.Length > 0 && 
                Manager.Moveset != null &&
                Manager.Moveset.Data != null &&
                Manager.Moveset.Data._modelVis != null)
                Manager.Moveset.Data._modelVis.ResetVisibility();
        }

        public static void ResetCharPos()
        {
            if (Manager.Moveset == null || Manager.Moveset.Data == null) return;
            MDL0BoneNode TopN = Manager.Moveset.Data._boneRef1[0].BoneNode;
            TopN._overrideLocalTranslate = new Vector3();
        }

        public static void UpdateCharPos()
        {
            //This needs to happen every time the animation loops or the subaction is changed.
            //For now emulation doesn't work and this just proves to be really tragic,
            //so it's commented out

            //MDL0BoneNode TopN = Manager.Moveset.Data._boneRef1[0].BoneNode;
            //MDL0BoneNode TransN = Manager.Moveset.Data._misc._boneRefs[4].BoneNode;
            //Vector3 v = TransN._frameMatrix.GetPoint();
            //TopN._overrideTranslate = v;
        }

        #region Frames
        public static void SetFrame(int frame)
        {
            if (frame == 1 && Loop && RunTime.CurrentSubaction != null && RunTime.CurrentSubaction.Flags.HasFlag(AnimationFlags.MovesCharacter))
                UpdateCharPos();

            //Set the animation frame
            int oldFrame = CurrentFrame;
            CurrentFrame = frame.Clamp(0, MaxFrame);
            bool forward = oldFrame == frame - 1;

            //Reset only if the on the first frame or the animation is going backward
            if (frame <= 1 || (!Playing && !forward))
                ResetSubactionVariables();

            //The next two things work for editing, but are technically wrong for emulation.
            //The animation and the scripts need to work seperately, meaning while the 
            //emulation is playing, the animation may loop but the scripts
            //progress forward always. At the moment if the animation loops, the scripts
            //will be reset and also loop.

            //Set the scripts to the current frame
            UpdateScripts(frame - 1);

            //Update the article models
            //Do this after applying the scripts!
            //If going backwards, the current frame will be set by the script
            //and the models can't be updated before that
            if (RunTime._articles != null)
                foreach (ArticleInfo a in RunTime._articles)
                    if (a != null && a.Running)
                        a.SetFrame(frame - 1);

            //if (MainWindow._capture && Playing)
            //    MainWindow.images.Add(MainWindow.ModelPanel.GetScreenshot(MainWindow.ModelPanel.ClientRectangle, false));
        }

        private static void UpdateScripts(int index)
        {
            //Update main model scripts first
            for (int i = 0; i < _runningScripts.Count; i++)
            {
                Script a = _runningScripts[i];
                if (a.ParentArticle != null)
                    continue;

                a.SetFrame(index);

                MainWindow.MovesetPanel.UpdateScriptEditor(a);
            }
            //Now update article scripts
            for (int i = 0; i < _runningScripts.Count; i++)
            {
                Script a = _runningScripts[i];
                if (a.ParentArticle == null)
                    continue;

                a.SetFrame(index - _articles[a.ParentArticle.Index]._setAt);
                
                MainWindow.MovesetPanel.UpdateScriptEditor(a);
            }
            //Finally, run concurrent scripts
            //They shouldn't have any wait times in them, 
            //so setting to frame 0 should run everything
            foreach (Script s in _concurrentLoopScripts.Values)
                s.SetFrame(0);
        }

        #endregion

        #region Rendering

        public static int CurrentFrame
        {
            get { return MainWindow.CurrentFrame; }
            set
            {
                MainWindow.CurrentFrame = value;
                MainWindow.UpdatePropDisplay();
                MainWindow.UpdatePlaybackPanel();
            }
        }

        public static int MaxFrame { get { return MainWindow.MaxFrame; } set { MainWindow.MaxFrame = value; } }
        public static bool Loop { get { return MainWindow.Loop; } set { MainWindow.Loop = value; } }
        public static bool Playing { get { return MainWindow.Playing; } set { MainWindow.Playing = value; MainWindow.pnlPlayback.btnPlay.Text = Playing ? "Stop" : "Play"; } }

        //public static int _animFrame = -1, _maxFrame;
        //public static bool _loop, _playing;

        public static void RenderFrame(object sender, FrameEventArgs e)
        {
            if (!IsRunning)
                return;

            int i = 0;
            if (MainWindow.TargetAnimation != null && ModelEditorBase.Interpolated.Contains(MainWindow.TargetAnimation.GetType()) && MainWindow.TargetAnimation.Loop)
                i = 1;

            if (CurrentFrame >= MainWindow.MaxFrame - i)
            {
                if (!MainWindow.Loop)
                {
                    if (_playingSounds.Count == 0)
                    {
                        Stop();
                        //ResetSubactionVariables();
                        //MainWindow.ModelPanel.Invalidate();
                    }

                    //Wait for playing sounds to finish, but don't update the frame.
                }
                else
                    SetFrame(1);
            }
            else
                SetFrame(CurrentFrame + 1);

            //Rendering text on the screen makes the FPS drop by nearly 20
            //So ironically, rendering the FPS on the screen slows the FPS
            //MainWindow.ModelPanel.ScreenText[String.Format("FPS: {0}", Math.Round(_timer.RenderFrequency, 3).ToString())] = new Vector3(5.0f, 10.0f, 0.5f);

            //Check if any sounds are done playing so they can be disposed of
            DisposeSounds();
        }

        /// <summary>
        /// Disposes of all sounds that are done playing.
        /// </summary>
        private static void DisposeSounds()
        {
            if (_playingSounds.Count != 0)
            {
                Dictionary<int, List<int>> keys = new Dictionary<int, List<int>>();
                foreach (var b in _playingSounds)
                {
                    int l = 0;
                    foreach (AudioInfo info in b.Value)
                    {
                        if (info._buffer != null)
                        {
                            if (info._buffer.Owner != null)
                                info._buffer.Fill();

                            if (info._buffer.ReadSample >= info._stream.Samples)
                            {
                                if (info._buffer.Owner != null)
                                    info._buffer.Stop();

                                info._buffer.Dispose();
                                info._stream.Dispose();

                                if (!keys.ContainsKey(b.Key))
                                    keys[b.Key] = new List<int>();

                                keys[b.Key].Add(l);
                            }
                        }
                        l++;
                    }
                }
                foreach (var i in keys)
                {
                    List<int> list = i.Value;
                    int b = i.Key;

                    if (_playingSounds.ContainsKey(b))
                    {
                        foreach (int l in list)
                            if (l < _playingSounds[b].Count && l >= 0)
                                _playingSounds[b].RemoveAt(l);
                        if (_playingSounds[b].Count == 0)
                            _playingSounds.Remove(b);
                    }
                }
            }
        }

        #endregion

        /*
            -- Button Press Values --
            00 A (normal attack)
            01 B (special attack)
            02 X, Y (jump)
            03 R, L, Z (shield)
            04 R, L, Z
            05 ?
            06 D-Up
            07 D-Down
            08 D-Left, D-Right
            09 D-Left
            10 D-Right
            11 B, X, D-Right, D-Left
            12 Y, X, D-Up, D-Right
            13 B, Y, X, D-Up, D-Right, D-Left
            14 A + B together
            15 C-Stick, any direction
            16 Tap jump setting. Always "held" while on and not while off. 
            17 ?
            18 ?
         
            -- Movement Values --
            Forward: IC-Basic[1011]
            Backward: IC-Basic[1012]
            Upward: IC-Basic[1018]
            Downward: IC-Basic[1020]
         
        */

        public static ButtonManager ButtonManager = new ButtonManager();

        public static readonly Keys[] Buttons = new Keys[]
        {
            Keys.J, //Attack
            Keys.K, //Special
            Keys.W, //Jump
            Keys.L, //Shield
            Keys.L, //Dodge?
            Keys.None,
            Keys.Up, //Up taunt
            Keys.Down, //Down taunt
            Keys.Left | Keys.Right, //Side taunt
            Keys.Left, //Side taunt L
            Keys.Right, //Side taunt R
            Keys.K | Keys.W | Keys.Right | Keys.Left,
            Keys.W | Keys.Up | Keys.Right,
            Keys.K | Keys.W | Keys.Up | Keys.Right | Keys.Left,
        };

        private static void UpdateFrame(object sender, FrameEventArgs e)
        {
            foreach (ActionChangeInfo info in _actionChanges)
                if (info._enabled && info.Evaluate())
                    CurrentActionIndex = info._newID;
            foreach (SubActionChangeInfo info in _subActionChanges)
                if (info.Evaluate())
                    CurrentSubactionIndex = info._newID;

            //Read controller input here
        }
    }

    public class ButtonManager
    {
        internal KeyMessageFilter _keyFilter = new KeyMessageFilter();
        internal Wiimote wm = new Wiimote();
        private WiimoteState wmState = null;
        private WiimoteExtensionChangedEventArgs wmExtLastChange = null;

        private Controller _controller = Controller.Keyboard;
        public Controller InputController
        {
            get { return _controller; }
            set 
            {
                if (_controller == value)
                    return;

                if (value == Controller.Keyboard)
                    DisconnectWiimote();
                else
                    if (!wm.Connected && !ConnectWiimote())
                    {
                        _controller = Controller.Keyboard;
                        return;
                    }

                _controller = value;
            }
        }

        public bool ConnectWiimote()
        {
            try { wm.Connect(); }
            catch { return false; }
            wm.SetReportType(InputReport.IRAccel, true);
            wm.SetLEDs(false, true, true, false);
            return true;
        }

        public void DisconnectWiimote()
        {
            wm.Disconnect();
            wm.SetReportType(InputReport.IRAccel, true);
            wm.SetLEDs(false, true, true, false);
        }

        public ButtonManager()
        {
            wm.WiimoteChanged += wm_WiimoteChanged;
            wm.WiimoteExtensionChanged += wm_WiimoteExtensionChanged;
        }

        private void wm_WiimoteChanged(object sender, WiimoteChangedEventArgs args)
        {
            wmState = args.WiimoteState;
        }

        private void wm_WiimoteExtensionChanged(object sender, WiimoteExtensionChangedEventArgs args)
        {
            wmExtLastChange = args;

            if (args.Inserted)
                wm.SetReportType(InputReport.IRExtensionAccel, true);
            else
                wm.SetReportType(InputReport.IRAccel, true);
        }

        public bool GetButtonPressed(int button)
        {
            if (InputController == Controller.Keyboard && !RunTime.MainWindow.ModelPanel.Focused)
                return false;

            return false;
        }

        public enum Controller
        {
            Keyboard,
            Wiimote,
            Nunchuk,
            Classic
        }
    }

    public class RunTimeAccessor
    {
        public RunTime.Location CharacterLocation { get { return RunTime._location; } set { RunTime._location = value; } }

        public List<Script> RunningScripts { get { return RunTime._runningScripts; } }
        
        public Dictionary<int, List<AudioInfo>> PlayingSounds { get { return RunTime._playingSounds; } }
        public ArticleInfo[] Articles { get { return RunTime._articles; } }
    }

    public class KeyMessageFilter : IMessageFilter
    {
        private Dictionary<Keys, bool> m_keyTable = new Dictionary<Keys, bool>();
        public Dictionary<Keys, bool> KeyTable
        {
            get { return m_keyTable; }
            private set { m_keyTable = value; }
        }

        public bool IsKeyPressed() { return m_keyPressed; }
        public bool IsKeyPressed(Keys k)
        {
            bool pressed = false;
            if (KeyTable.TryGetValue(k, out pressed))
                return pressed;

            return false;
        }

        private const int WM_KEYDOWN = 0x0100;
        private const int WM_KEYUP = 0x0101;
        private bool m_keyPressed = false;

        public bool PreFilterMessage(ref Message m)
        {
            if (m.Msg == WM_KEYDOWN)
            {
                KeyTable[(Keys)m.WParam] = true;
                m_keyPressed = true;
            }

            if (m.Msg == WM_KEYUP)
            {
                KeyTable[(Keys)m.WParam] = false;
                m_keyPressed = false;
            }

            return false;
        }
    }
}