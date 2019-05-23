using System;
using System.Collections.Generic;
using BrawlLib.SSBB.ResourceNodes;
using System.IO;
using System.Windows.Forms;
using System.ComponentModel;
using BrawlLib.SSBBTypes;
using Ikarus.MovesetFile;
using Ikarus.ModelViewer;
using Ikarus.UI;

namespace Ikarus
{
    public class CharacterInfo
    {
        private int _charId;
        public CharName Name { get { return (CharName)_charId; } set { _charId = (int)value; } }
        public CharFolder Folder { get { return (CharFolder)_charId; } set { _charId = (int)value; } }

        private Dictionary<int, Dictionary<ARCFileType, List<ARCEntryNode>>> _characterEffectFiles;
        private Dictionary<int, Dictionary<ARCFileType, List<ARCEntryNode>>> _characterEtcFiles;
        private Dictionary<int, Dictionary<ARCFileType, List<ARCEntryNode>>> _characterFinalFiles;
        private Dictionary<int, Dictionary<ARCFileType, List<ARCEntryNode>>> _characterFiles;
        
        public Dictionary<int, Dictionary<ARCFileType, List<ARCEntryNode>>> CharacterEffectFiles
        {
            get
            {
                if (_characterEffectFiles != null)
                    return _characterEffectFiles;

                ARCNode m = MovesetArc;
                if (m == null) return null;
                if (m.Children.Count < 2) return null;
                ARCNode entry = m.Children[1] as ARCNode;
                LoadArcFiles(entry, ref _characterEffectFiles);
                return _characterEffectFiles;
            }
        }
        public Dictionary<int, Dictionary<ARCFileType, List<ARCEntryNode>>> CharacterFinalFiles
        {
            get
            {
                if (_characterFinalFiles != null)
                    return _characterFinalFiles;

                ARCNode m = FinalArc;
                if (m == null) return null;
                LoadArcFiles(m, ref _characterFinalFiles);
                return _characterFinalFiles;
            }
        }
        public Dictionary<int, Dictionary<ARCFileType, List<ARCEntryNode>>> CharacterEtcFiles { get { return _characterEtcFiles; } }
        public Dictionary<int, Dictionary<ARCFileType, List<ARCEntryNode>>> CharacterFiles { get { return _characterFiles; } }
        
        private BRRESNode _animations = null;
        private MovesetNode _moveset = null;

        private string FileName(int i) { return (RunTime._IsRoot ? "/fighter/{0}/" : "/") + _fileNames[i]; }
        private static readonly string[] _fileNames = 
        { 
            "Fit{0}.pac",
            "Fit{0}MotionEtc.pac",
            "Fit{0}Entry.pac",
            "Fit{0}Final.pac",
            "Fit{0}{1}.pcs",
            "Fit{0}{1}.pac",
            "Fit{0}Motion.pac",
        };
        ARCNode _movesetArc, _motionEtcArc, _entryArc, _finalArc;
        ARCNode[] _modelArcs = new ARCNode[12];

        public ARCNode MovesetArc
        {
            get
            {
                return _movesetArc == null ?
                    _movesetArc = Manager.TryGet<ARCNode>(true, true, String.Format(FileName(0), Folder)) :
                    _movesetArc; 
            }
        }
        public ARCNode MotionEtcArc
        {
            get
            {
                return _motionEtcArc == null ?
                    _motionEtcArc = Manager.TryGet<ARCNode>(true, true, String.Format(FileName(1), Folder), String.Format(FileName(6), Folder)) :
                    _motionEtcArc; 
            } 
        }
        public ARCNode EntryArc
        {
            get
            {
                return _entryArc == null ? 
                    _entryArc = Manager.TryGet<ARCNode>(true, true, String.Format(FileName(2), Folder)) :
                    _entryArc; 
            }
        }
        public ARCNode FinalArc
        {
            get
            {
                return _finalArc == null ?
                    _finalArc = Manager.TryGet<ARCNode>(true, true, String.Format(FileName(3), Folder)) :
                    _finalArc; 
            }
        }
        public ARCNode GetModelArc(int index)
        {
            if (_modelArcs[index] != null) 
                return _modelArcs[index];
            return _modelArcs[index] = Manager.TryGet<ARCNode>(true, true, String.Format(FileName(4), Folder, index.ToString().PadLeft(2, '0')), String.Format(FileName(5), Folder, index.ToString().PadLeft(2, '0')));
        }
        public bool ModelArcExists(int index, bool searchDir, bool searchOpened)
        {
            if (_modelArcs[index] != null)
                return true;
            return Manager.FileExists(searchDir, searchOpened, String.Format(FileName(4), Folder, index.ToString().PadLeft(2, '0')), String.Format(FileName(5), Folder, index.ToString().PadLeft(2, '0')));
        }

        internal BRRESNode Animations 
        {
            get
            {
                if (_animations != null) 
                    return _animations;

                ARCNode m = MotionEtcArc;
                if (m != null && m.Children.Count >= 2 && m.Children[1].Children.Count > 0)
                    return _animations = m.Children[1].Children[0] as BRRESNode;
                else if (m != null && m.Children.Count == 1 && m.Children[0].Children.Count > 0)
                    return _animations = m.Children[0] as BRRESNode;
                return _animations = null;
            }
        }

        internal MovesetNode Moveset
        {
            get
            {
                if (_moveset != null)
                    return _moveset;

                ARCNode m = MovesetArc;
                if (m == null) return null;
                if (m.Children.Count == 0) return null;
                ARCEntryNode entry = m.Children[0] as ARCEntryNode;
                (_moveset = new MovesetNode(Name)).Initialize(null, entry.WorkingUncompressed);
                _moveset.Populate(0);
                return _moveset;
            }
        }

        public BindingList<int> _usableModels;

        public CharacterInfo(CharName character)
        {
            Name = character;
            _usableModels = new BindingList<int>();
            GetModelIndices();
            LoadParameters();

            if (MotionEtcArc != null && MotionEtcArc.Children.Count >= 2)
                LoadArcFiles(MotionEtcArc.Children[0] as ARCNode, ref _characterEtcFiles);
            
            MainForm.Instance._mainControl.comboMdl.DataSource = _usableModels;
            if (_usableModels.Count > 0 && ModelArcExists(_usableModels[0], true, true))
                MainForm.Instance._mainControl.comboMdl.SelectedIndex = _usableModels[0];
        }

        public void GetModelIndices()
        {
            _usableModels.Clear();
            for (int i = 0; i < 12; i++)
                if (ModelArcExists(i, true, true))
                    _usableModels.Add(i);
        }

        /// <summary>
        /// Unloads all loaded files in the file collection.
        /// Use this when the character is deselected to save memory.
        /// This will not unload modified files.
        /// </summary>
        public void Close()
        {
            if (_movesetArc != null && !_movesetArc.IsDirty)
                Manager.RemoveFile(_movesetArc);
            if (_motionEtcArc != null && !_motionEtcArc.IsDirty)
                Manager.RemoveFile(_motionEtcArc);
            if (_entryArc != null && !_entryArc.IsDirty)
                Manager.RemoveFile(_entryArc);
            if (_finalArc != null && !_finalArc.IsDirty)
                Manager.RemoveFile(_finalArc);
            for (int i = 0; i < 12; i++)
                if (_modelArcs[i] != null && !_modelArcs[i].IsDirty)
                    Manager.RemoveFile(_modelArcs[i]);

            if (_parametersChanged)
                SaveParameters();
        }

        public unsafe MDL0Node SelectedModel 
        {
            get 
            {
                if (_characterFiles == null)
                    ModelIndexChanged();
                if (_characterFiles == null)
                    return null;
                return _characterFiles[0][ARCFileType.ModelData][0].Children[0].Children[0] as MDL0Node;
            } 
        }

        internal void ClearModel()
        {
            int i;
            if (_usableModels.Count > 0 && _mdlIndex >= 0 && _mdlIndex < _usableModels.Count)
            {
                i = _usableModels[_mdlIndex];
                ARCNode a = GetModelArc(i);
                if (a != null && !a.IsDirty)
                {
                    Manager.RemoveFile(a);
                    _modelArcs[i] = null;
                }
            }
            _mdlIndex = -1;
        }

        int _mdlIndex = -1;
        internal void ModelIndexChanged()
        {
            ClearModel();

            int i;
            _mdlIndex = MainForm.Instance._mainControl.comboMdl.SelectedIndex;
            if (_usableModels.Count > 0 && _mdlIndex < _usableModels.Count && _mdlIndex >= 0 && ModelArcExists(i = _usableModels[_mdlIndex], true, true))
                LoadArcFiles(GetModelArc(i), ref _characterFiles);
        }

        private void LoadArcFiles(ARCNode arc, ref Dictionary<int, Dictionary<ARCFileType, List<ARCEntryNode>>> files)
        {
            if (arc == null)
                return;

            files = new Dictionary<int, Dictionary<ARCFileType, List<ARCEntryNode>>>();
            foreach (ARCEntryNode e in arc.Children)
            {
                int grp = e.GroupID;

                if (!files.ContainsKey(grp))
                    files[grp] = new Dictionary<ARCFileType, List<ARCEntryNode>>();

                ARCFileType type = e.FileType;

                if (!files[grp].ContainsKey(type))
                    files[grp].Add(type, new List<ARCEntryNode>());

                files[grp][type].Add(e);
            }
        }

        #region Parameters

        public bool _parametersChanged = false;
        public Dictionary<string, SectionParamInfo> _parameters = null;
        public void LoadParameters()
        {
            _parameters = new Dictionary<string, SectionParamInfo>();
            string loc = Application.StartupPath + "/MovesetData/CharSpecific/Fit" + ((CharFolder)(int)_charId).ToString() + ".txt";
            string n = "", attrName = "";
            if (File.Exists(loc))
                using (StreamReader sr = new StreamReader(loc))
                    while (!sr.EndOfStream)
                    {
                        n = sr.ReadLine();
                        SectionParamInfo info = new SectionParamInfo();
                        info._newName = sr.ReadLine();
                        info._attributes = new List<AttributeInfo>();
                        while (true && !sr.EndOfStream)
                        {
                            if (String.IsNullOrEmpty(attrName = sr.ReadLine()))
                                break;
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
                        if (!_parameters.ContainsKey(n))
                            _parameters.Add(n, info);
                    }
        }
        public void SaveParameters()
        {
            string Params = Application.StartupPath + "/MovesetData/CharSpecific/fit" + ((CharFolder)(int)_charId).ToString() + ".txt";

            if (!Directory.Exists(Application.StartupPath + "/MovesetData/CharSpecific"))
                Directory.CreateDirectory(Application.StartupPath + "/MovesetData/CharSpecific");

            bool go = true;
            if (File.Exists(Params))
                if (MessageBox.Show("Do you want to overwrite Fit" + ((CharFolder)_charId).ToString() + ".txt in the MovesetData/CharSpecific folder?", "Overwrite Permission", MessageBoxButtons.YesNo) == DialogResult.No)
                    go = false;
            if (go)
                using (StreamWriter file = new StreamWriter(Params))
                {
                    foreach (var i in _parameters)
                    {
                        file.WriteLine(i.Key);
                        file.WriteLine(i.Value._newName);
                        foreach (AttributeInfo a in i.Value._attributes)
                        {
                            file.WriteLine(a._name);
                            file.WriteLine(a._description);
                            file.WriteLine(a._type);
                            file.WriteLine();
                        }
                        file.WriteLine();
                    }
                }
        }

        #endregion

        public override string ToString() { return Name.ToString(); }
    }

    public enum CharFolder
    {
        Captain = 0,
        Dedede,
        Diddy,
        Donkey,
        Falco,
        Fox = 5,
        Gamewatch,
        Ganon,
        GKoopa,
        Ike,
        Kirby = 10,
        Koopa,
        Link,
        Lucario,
        Lucas,
        Luigi = 15,
        Mario,
        Marth,
        Metaknight,
        Ness,
        Peach = 20,
        Pikachu,
        Pikmin,
        Pit,
        PokeFushigisou,
        PokeLizardon = 25,
        PokeTrainer,
        PokeZenigame,
        Popo,
        Purin,
        Robot = 30,
        Samus,
        Sheik,
        Snake,
        Sonic,
        SZerosuit = 35,
        ToonLink,
        Wario,
        WarioMan,
        Wolf,
        Yoshi = 40,
        ZakoBall,
        ZakoBoy,
        ZakoChild,
        ZakoGirl,
        Zelda = 45
    }
    public enum CharName
    {
        None = -1,
        CaptainFalcon = 0,
        KingDedede,
        DiddyKong,
        DonkeyKong,
        Falco,
        Fox = 5,
        MrGameNWatch,
        Ganondorf,
        GigaBowser,
        Ike,
        Kirby = 10,
        Bowser,
        Link,
        Lucario,
        Lucas,
        Luigi = 15,
        Mario,
        Marth,
        Metaknight,
        Ness,
        Peach = 20,
        Pikachu,
        Olimar,
        Pit,
        Ivysaur,
        Charizard = 25,
        PokemonTrainer,
        Squirtle,
        Popo,
        Jigglypuff,
        ROB = 30,
        Samus,
        Sheik,
        Snake,
        Sonic,
        ZeroSuitSamus = 35,
        ToonLink,
        Wario,
        WarioMan,
        Wolf,
        Yoshi = 40,
        GreenAlloy,
        RedAlloy,
        YellowAlloy,
        BlueAlloy,
        Zelda = 45,
    }
}
