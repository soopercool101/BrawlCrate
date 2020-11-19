using BrawlLib.Modeling;
using BrawlLib.SSBB.ResourceNodes;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
#if DEBUG
using System;
#endif

namespace BrawlLib.Internal.Windows.Controls.ModelViewer.MainWindowBase
{
    public partial class ModelEditorBase : UserControl
    {
        public uint _allowedUndos = 50;
        public List<SaveState> _undoSaves = new List<SaveState>();
        public List<SaveState> _redoSaves = new List<SaveState>();
        public int _saveIndex;
        public bool AwaitingRedoSave => _currentUndo != null;
        public bool _undoing;

        public SaveState _currentUndo;

        private void AddUndo(SaveState save)
        {
            if (AwaitingRedoSave)
            {
#if DEBUG
                throw new Exception("Waiting for redo save to be added");
#else
                return;
#endif
            }

            save._isUndo = true;
            _currentUndo = save;
        }

        private void AddRedo(SaveState save)
        {
            if (!AwaitingRedoSave)
            {
#if DEBUG
                throw new Exception("Waiting for undo save to be added");
#else
                return;
#endif
            }

            save._isUndo = false;

            //Remove changes made after the current state
            //BEFORE adding new saves for this state
            int i = _saveIndex + 1;
            if (_undoSaves.Count > i && _undoSaves.Count - i > 0)
            {
                _undoSaves.RemoveRange(i, _undoSaves.Count - i);
                _redoSaves.RemoveRange(i, _redoSaves.Count - i);
            }

            //Add the saves
            _redoSaves.Add(save);
            _undoSaves.Add(_currentUndo);

            //Remove the oldest saves if over the max undo count
            if (_undoSaves.Count > _allowedUndos)
            {
                _undoSaves.RemoveAt(0);
                _redoSaves.RemoveAt(0);
            }
            else //Otherwise, move the save index to the current saves
            {
                _saveIndex++;
            }

            _currentUndo = null;
            UpdateUndoButtons();
        }

        private void AddState(SaveState state)
        {
            if (!AwaitingRedoSave)
            {
                AddUndo(state);
            }
            else
            {
                AddRedo(state);
            }
        }

        /// <summary>
        /// Call twice; before and after changes
        /// </summary>
        public void BoneChange(params IBoneNode[] bones)
        {
            SaveState state = new BoneState
            {
                _bones = bones,
                _frameStates = bones.Select(x => x.FrameState).ToArray(),
                _animation = SelectedCHR0,
                _frameIndex = CurrentFrame,
                _updateBoneOnly = CHR0Editor.chkMoveBoneOnly.Checked,
                _updateBindState = CHR0Editor.chkUpdateBindPose.Checked
            };

            AddState(state);
        }

        /// <summary>
        /// Call twice; before and after changes
        /// </summary>
        public void VertexChange(List<Vertex3> vertices)
        {
            SaveState state = new VertexState
            {
                _chr0 = _chr0,
                _animFrame = CurrentFrame,
                _vertices = vertices,
                _weightedPositions = vertices.Select(x => x.WeightedPosition).ToList(),
                _targetModel = TargetModel
            };

            AddState(state);
        }

        public void CancelChangeState()
        {
            if (!AwaitingRedoSave)
            {
#if DEBUG
                throw new Exception("Nothing to cancel.");
#else
                return;
#endif
            }

            _currentUndo = null;
        }

        public bool CanUndo => _saveIndex >= 0;
        public bool CanRedo => _saveIndex < _undoSaves.Count;

        public void Undo()
        {
            _boneSelection.ResetActions();
            _vertexSelection.ResetActions();

            if (AwaitingRedoSave)
            {
                CancelChangeState();
            }
            else if (CanUndo)
            {
                ModelPanel.BeginUpdate();

                if (!_undoing)
                {
                    _saveIndex--;
                }

                _undoing = true;

                if (_saveIndex < _undoSaves.Count && _saveIndex >= 0)
                {
                    ApplyState();
                }

                //Decrement index after applying save
                _saveIndex--;

                UpdateUndoButtons();

                ModelPanel.EndUpdate();
            }
        }

        public void Redo()
        {
            _boneSelection.ResetActions();
            _vertexSelection.ResetActions();

            if (AwaitingRedoSave)
            {
                CancelChangeState();
            }

            if (CanRedo)
            {
                ModelPanel.BeginUpdate();

                if (_undoing)
                {
                    _saveIndex++;
                }

                _undoing = false;

                if (_saveIndex < _redoSaves.Count && _saveIndex >= 0)
                {
                    ApplyState();
                }

                //Increment index after applying save
                _saveIndex++;

                UpdateUndoButtons();

                ModelPanel.EndUpdate();
            }
        }

        private void ApplyBoneState(BoneState state)
        {
            if (TargetModel != state._targetModel)
            {
                _resetCamera = false;
                TargetModel = state._targetModel;
            }

            SelectedCHR0 = state._animation;
            CurrentFrame = state._frameIndex;
            CHR0Editor.chkUpdateBindPose.Checked = state._updateBindState;
            CHR0Editor.chkMoveBoneOnly.Checked = state._updateBoneOnly;
            for (int i = 0; i < state._bones.Length; i++)
            {
                SelectedBone = state._bones[i];
                CHR0Editor.ApplyState(state._frameStates[i]);
            }
        }

        private void ApplyVertexState(VertexState state)
        {
            IModel model = TargetModel;
            CHR0Node n = _chr0;
            int frame = CurrentFrame;

            if (TargetModel != state._targetModel)
            {
                _resetCamera = false;
                TargetModel = state._targetModel;
            }

            SelectedCHR0 = state._chr0;
            CurrentFrame = state._animFrame;

            for (int i = 0; i < state._vertices.Count; i++)
            {
                state._vertices[i].WeightedPosition = state._weightedPositions[i];
            }

            SetSelectedVertices(state._vertices);

            _vertexLoc = null;

            if (TargetModel != model)
            {
                _resetCamera = false;
                TargetModel = model;
            }

            SelectedCHR0 = n;
            CurrentFrame = frame;

            UpdateModel();
        }

        public SaveState RedoSave => _saveIndex < _redoSaves.Count && _saveIndex >= 0 ? _redoSaves[_saveIndex] : null;
        public SaveState UndoSave => _saveIndex < _undoSaves.Count && _saveIndex >= 0 ? _undoSaves[_saveIndex] : null;

        private void ApplyState()
        {
            SaveState current = _undoing ? UndoSave : RedoSave;

            if (current is BoneState)
            {
                ApplyBoneState((BoneState) current);
            }
            else if (current is VertexState)
            {
                ApplyVertexState((VertexState) current);
            }
        }

        public virtual void UpdateUndoButtons()
        {
        }
    }
}