using BrawlLib.Internal.Windows.Controls.ModelViewer.MainWindowBase;
using BrawlLib.Modeling;
using BrawlLib.SSBB.ResourceNodes;
using BrawlLib.Wii.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace BrawlLib.Internal.Windows.Controls.ModelViewer.Editors
{
    public class WeightEditor : UserControl
    {
        #region Designer

        private void InitializeComponent()
        {
            btnSetWeight = new Button();
            btnBlend = new Button();
            btnAdd = new Button();
            btnSubtract = new Button();
            btnLock = new Button();
            lblBoneName = new Label();
            btnRemove = new Button();
            panel1 = new Panel();
            numMult = new NumericInputBox();
            btnMult = new Button();
            btnDiv = new Button();
            numAdd = new NumericInputBox();
            numWeight = new NumericInputBox();
            btnPaste = new Button();
            btnCopy = new Button();
            lstBoneWeights = new RefreshableListBox();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // btnSetWeight
            // 
            btnSetWeight.Enabled = false;
            btnSetWeight.Location = new Point(67, 28);
            btnSetWeight.Name = "btnSetWeight";
            btnSetWeight.Size = new Size(61, 22);
            btnSetWeight.TabIndex = 2;
            btnSetWeight.Text = "Set";
            btnSetWeight.UseVisualStyleBackColor = true;
            btnSetWeight.Click += new EventHandler(btnSetWeight_Click);
            // 
            // btnBlend
            // 
            btnBlend.Location = new Point(129, 28);
            btnBlend.Name = "btnBlend";
            btnBlend.Size = new Size(62, 22);
            btnBlend.TabIndex = 4;
            btnBlend.Text = "Blend";
            btnBlend.UseVisualStyleBackColor = true;
            btnBlend.Visible = false;
            btnBlend.Click += new EventHandler(btnBlend_Click);
            // 
            // btnAdd
            // 
            btnAdd.Enabled = false;
            btnAdd.Location = new Point(67, 52);
            btnAdd.Name = "btnAdd";
            btnAdd.Size = new Size(30, 22);
            btnAdd.TabIndex = 7;
            btnAdd.Text = "+";
            btnAdd.UseVisualStyleBackColor = true;
            btnAdd.Click += new EventHandler(btnAdd_Click);
            // 
            // btnSubtract
            // 
            btnSubtract.Enabled = false;
            btnSubtract.Location = new Point(98, 52);
            btnSubtract.Name = "btnSubtract";
            btnSubtract.Size = new Size(30, 22);
            btnSubtract.TabIndex = 8;
            btnSubtract.Text = "-";
            btnSubtract.UseVisualStyleBackColor = true;
            btnSubtract.Click += new EventHandler(btnSubtract_Click);
            // 
            // btnLock
            // 
            btnLock.Enabled = false;
            btnLock.Location = new Point(2, 4);
            btnLock.Name = "btnLock";
            btnLock.Size = new Size(64, 22);
            btnLock.TabIndex = 10;
            btnLock.Text = "Lock";
            btnLock.UseVisualStyleBackColor = true;
            btnLock.Click += new EventHandler(btnLock_Click);
            // 
            // lblBoneName
            // 
            lblBoneName.AutoSize = true;
            lblBoneName.Location = new Point(134, 9);
            lblBoneName.Name = "lblBoneName";
            lblBoneName.Size = new Size(32, 13);
            lblBoneName.TabIndex = 11;
            lblBoneName.Text = "Bone";
            // 
            // btnRemove
            // 
            btnRemove.Enabled = false;
            btnRemove.Location = new Point(67, 4);
            btnRemove.Name = "btnRemove";
            btnRemove.Size = new Size(61, 22);
            btnRemove.TabIndex = 12;
            btnRemove.Text = "Remove";
            btnRemove.UseVisualStyleBackColor = true;
            btnRemove.Click += new EventHandler(btnRemoveBone_Click);
            // 
            // panel1
            // 
            panel1.Controls.Add(numMult);
            panel1.Controls.Add(btnMult);
            panel1.Controls.Add(btnDiv);
            panel1.Controls.Add(numAdd);
            panel1.Controls.Add(btnAdd);
            panel1.Controls.Add(numWeight);
            panel1.Controls.Add(btnBlend);
            panel1.Controls.Add(btnPaste);
            panel1.Controls.Add(btnSetWeight);
            panel1.Controls.Add(btnCopy);
            panel1.Controls.Add(btnSubtract);
            panel1.Controls.Add(btnLock);
            panel1.Controls.Add(lblBoneName);
            panel1.Controls.Add(btnRemove);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(130, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(130, 103);
            panel1.TabIndex = 14;
            // 
            // numMult
            // 
            numMult.Enabled = false;
            numMult.Integral = false;
            numMult.Location = new Point(3, 77);
            numMult.MaximumValue = 3.402823E+38F;
            numMult.MinimumValue = -3.402823E+38F;
            numMult.Name = "numMult";
            numMult.Size = new Size(62, 20);
            numMult.TabIndex = 16;
            numMult.Text = "1.05";
            // 
            // btnMult
            // 
            btnMult.Enabled = false;
            btnMult.Location = new Point(67, 76);
            btnMult.Name = "btnMult";
            btnMult.Size = new Size(30, 22);
            btnMult.TabIndex = 14;
            btnMult.Text = "x";
            btnMult.UseVisualStyleBackColor = true;
            btnMult.Click += new EventHandler(btnMult_Click);
            // 
            // btnDiv
            // 
            btnDiv.Enabled = false;
            btnDiv.Location = new Point(98, 76);
            btnDiv.Name = "btnDiv";
            btnDiv.Size = new Size(30, 22);
            btnDiv.TabIndex = 15;
            btnDiv.Text = "/";
            btnDiv.UseVisualStyleBackColor = true;
            btnDiv.Click += new EventHandler(btnDiv_Click);
            // 
            // numAdd
            // 
            numAdd.Enabled = false;
            numAdd.Integral = false;
            numAdd.Location = new Point(3, 53);
            numAdd.MaximumValue = 3.402823E+38F;
            numAdd.MinimumValue = -3.402823E+38F;
            numAdd.Name = "numAdd";
            numAdd.Size = new Size(62, 20);
            numAdd.TabIndex = 13;
            numAdd.Text = "10";
            // 
            // numWeight
            // 
            numWeight.Enabled = false;
            numWeight.Integral = false;
            numWeight.Location = new Point(3, 29);
            numWeight.MaximumValue = 3.402823E+38F;
            numWeight.MinimumValue = -3.402823E+38F;
            numWeight.Name = "numWeight";
            numWeight.Size = new Size(62, 20);
            numWeight.TabIndex = 3;
            numWeight.Text = "100";
            numWeight.ValueChanged += new EventHandler(numWeight_ValueChanged);
            // 
            // btnPaste
            // 
            btnPaste.Location = new Point(129, 75);
            btnPaste.Name = "btnPaste";
            btnPaste.Size = new Size(62, 22);
            btnPaste.TabIndex = 6;
            btnPaste.Text = "Paste";
            btnPaste.UseVisualStyleBackColor = true;
            btnPaste.Visible = false;
            btnPaste.Click += new EventHandler(btnPaste_Click);
            // 
            // btnCopy
            // 
            btnCopy.Location = new Point(129, 51);
            btnCopy.Name = "btnCopy";
            btnCopy.Size = new Size(62, 22);
            btnCopy.TabIndex = 5;
            btnCopy.Text = "Copy";
            btnCopy.UseVisualStyleBackColor = true;
            btnCopy.Visible = false;
            btnCopy.Click += new EventHandler(btnCopy_Click);
            // 
            // lstBoneWeights
            // 
            lstBoneWeights.Dock = DockStyle.Left;
            lstBoneWeights.DrawMode = DrawMode.OwnerDrawFixed;
            lstBoneWeights.FormattingEnabled = true;
            lstBoneWeights.IntegralHeight = false;
            lstBoneWeights.Location = new Point(0, 0);
            lstBoneWeights.Name = "lstBoneWeights";
            lstBoneWeights.Size = new Size(130, 103);
            lstBoneWeights.TabIndex = 0;
            lstBoneWeights.DrawItem += new DrawItemEventHandler(lstBoneWeights_DrawItem);
            lstBoneWeights.SelectedIndexChanged += new EventHandler(lstBoneWeights_SelectedIndexChanged);
            // 
            // WeightEditor
            // 
            Controls.Add(panel1);
            Controls.Add(lstBoneWeights);
            MinimumSize = new Size(260, 103);
            Name = "WeightEditor";
            Size = new Size(260, 103);
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        public WeightEditor()
        {
            InitializeComponent();
        }

        private readonly BindingList<BoneWeight> _targetWeights = null;

        public IBoneNode[] Bones => _targetWeights.Select(x => x.Bone).ToArray();
        public float[] Weights => _targetWeights.Select(x => x.Weight).ToArray();

        public ModelEditorBase _mainWindow;
        private Button btnSetWeight;
        private NumericInputBox numWeight;
        private Button btnBlend;
        private Button btnAdd;
        private Label lblBoneName;
        private Button btnSubtract;

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int CurrentFrame
        {
            get => _mainWindow.CurrentFrame;
            set => _mainWindow.CurrentFrame = value;
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public IModel TargetModel
        {
            get => _mainWindow.TargetModel;
            set => _mainWindow.TargetModel = value;
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public IBoneNode SelectedBone
        {
            get => _mainWindow.SelectedBone;
            set => _mainWindow.SelectedBone = value;
        }

        private Button btnLock;
        private Button btnPaste;
        private Button btnCopy;
        private RefreshableListBox lstBoneWeights;

        public List<Vertex3> _targetVertices;

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public List<Vertex3> TargetVertices
        {
            get => _targetVertices;
            set
            {
                if (_targetVertices != value)
                {
                    SetVertices(value);
                }
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public BoneWeight TargetBoneWeight
        {
            get => _targetBoneWeight;
            set
            {
                if ((_targetBoneWeight = value) != null)
                {
                    _mainWindow.SelectedBone = TargetBoneWeight.Bone;
                }

                _mainWindow.ModelPanel.Invalidate();
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public IBoneNode TargetBone
        {
            get => _targetBone;
            set
            {
                _targetBone = value;

                if (_targetBone != null)
                {
                    lblBoneName.Text = _targetBone.Name;
                    btnLock.Enabled = true;
                    btnLock.Text = _targetBone.Locked ? "Unlock" : "Lock";

                    if (_bones != null)
                    {
                        int i = _bones.IndexOf(_targetBone);
                        lstBoneWeights.SelectedIndex = i;
                        if (i < 0)
                        {
                            numWeight.Value = 100.0f;
                        }

                        //numWeight.Value = i != -1 ? ((BoneWeight)lstBoneWeights.Items[i]).Weight * 100.0f : 0;
                    }
                    else
                    {
                        numWeight.Value = 100.0f;
                        lstBoneWeights.SelectedIndex = -1;
                    }
                }
                else
                {
                    btnLock.Enabled = false;
                    btnLock.Text = "Lock";
                    lblBoneName.Text = "(None)";
                    numWeight.Value = 0.0f;
                    lstBoneWeights.SelectedIndex = -1;
                }

                UpdateButtons();
            }
        }

        private void UpdateButtons()
        {
            bool canAdd = false, canSub = false, canMul = false, canDiv = false, canRemove = false;
            if (_targetBone != null)
            {
                foreach (BoneWeight b in lstBoneWeights.Items)
                {
                    if (b.Bone == _targetBone)
                    {
                        canAdd = b.Weight < 1.0f;
                        canSub = b.Weight > 0.0f;
                        canMul = b.Weight != 0.0f;
                        canDiv = b.Weight != 0.0f && numMult.Value != 0.0f;
                        canRemove = true;
                        break;
                    }
                }

                //Bone not found in array?
                if (!canRemove)
                {
                    canAdd = numAdd.Value != 0.0f;
                }
            }

            btnAdd.Enabled = canAdd;
            btnMult.Enabled = canMul;
            btnSubtract.Enabled = canSub;
            btnDiv.Enabled = canDiv;
            btnRemove.Enabled = canRemove;
            numWeight.Enabled = btnSetWeight.Enabled = _targetBone != null;
            numAdd.Enabled = canAdd || canSub;
            numMult.Enabled = canMul || canDiv;
        }

        public BoneWeight _targetBoneWeight;
        private Button btnRemove;
        private Panel panel1;
        private NumericInputBox numMult;
        private Button btnMult;
        private Button btnDiv;
        private NumericInputBox numAdd;
        public IBoneNode _targetBone;

        public void SetVertices(List<Vertex3> vertices)
        {
            _targetVertices = vertices.ToList();

            ResetList();
        }

        public float _weightTotal;
        private Dictionary<string, float[]> _totals = new Dictionary<string, float[]>();
        public List<IBoneNode> _bones;

        public void ResetList()
        {
            lstBoneWeights.Items.Clear();

            _bones = new List<IBoneNode>();
            _totals = new Dictionary<string, float[]>();
            _weightTotal = 0;
            if (_targetVertices != null)
            {
                foreach (Vertex3 v in _targetVertices)
                {
                    List<BoneWeight> array = v.GetBoneWeights();
                    foreach (BoneWeight b in array)
                    {
                        if (!_totals.ContainsKey(b.Bone.Name))
                        {
                            _bones.Add(b.Bone);
                            _totals.Add(b.Bone.Name, new float[] {b.Weight, 1});
                        }
                        else
                        {
                            _totals[b.Bone.Name][0] += b.Weight;
                            _totals[b.Bone.Name][1] += 1;
                        }

                        _weightTotal += b.Weight;
                    }
                }
            }

            foreach (MDL0BoneNode b in _bones)
            {
                lstBoneWeights.Items.Add(new BoneWeight(b, _totals[b.Name][0] / _weightTotal));
            }

            if (_bones.Contains(_mainWindow.SelectedBone))
            {
                lstBoneWeights.SelectedIndex = _bones.IndexOf(_mainWindow.SelectedBone);
            }
        }

        public bool _updating = false;
        public List<MDL0ObjectNode> _anyConverted;

        public void SetWeight(float value)
        {
            if (TargetVertices == null || TargetVertices.Count == 0)
            {
                return;
            }

            _anyConverted = new List<MDL0ObjectNode>();
            foreach (Vertex3 v in TargetVertices)
            {
                SetWeight(value, v);
            }

            UpdateBindState(TargetVertices.ToArray());
        }

        public void SetWeight(float value, Vertex3 vertex)
        {
            Weight(value, vertex, WeightType.Set);
        }

        public void IncrementWeight(float value)
        {
            if (TargetVertices == null || TargetVertices.Count == 0)
            {
                return;
            }

            _anyConverted = new List<MDL0ObjectNode>();
            foreach (Vertex3 v in TargetVertices)
            {
                IncrementWeight(value, v);
            }

            UpdateBindState(TargetVertices.ToArray());
        }

        public void IncrementWeight(float value, Vertex3 vertex)
        {
            Weight(value, vertex, WeightType.Add);
        }

        public void MultiplyWeight(float value)
        {
            if (TargetVertices == null || TargetVertices.Count == 0)
            {
                return;
            }

            _anyConverted = new List<MDL0ObjectNode>();
            foreach (Vertex3 v in TargetVertices)
            {
                MultiplyWeight(value, v);
            }

            UpdateBindState(TargetVertices.ToArray());
        }

        public void MultiplyWeight(float value, Vertex3 vertex)
        {
            Weight(value, vertex, WeightType.Multiply);
        }

        private void UpdateBindState(params Vertex3[] vertices)
        {
            List<MDL0ObjectNode> changed = new List<MDL0ObjectNode>();
            foreach (Vertex3 v in vertices)
            {
                if (!changed.Contains(v.Parent as MDL0ObjectNode))
                {
                    changed.Add(v.Parent as MDL0ObjectNode);
                }
            }

            foreach (MDL0ObjectNode obj in changed)
            {
                //See if the object can be made into a single bind
                obj.TryConvertMatrixToObject();

                if (_anyConverted.Contains(obj))
                {
                    obj.SetEditedAssets(false, true, true);
                }

                //Force full object rebuild and signal a change to the object
                obj._forceRebuild = true;
                obj.SignalPropertyChange();
            }

            //Update vertices with edited influences
            _mainWindow.UpdateModel();

            _anyConverted = null;
        }

        private float RoundValue(float value, float max)
        {
            return (float) Math.Round(value.Clamp(0.0f, max), 7);
        }

        private enum WeightType
        {
            Set,
            Multiply,
            Add
        }

        //Returns true if the vertex's matrix node is changed.
        private bool Weight(float value, Vertex3 vertex, WeightType type)
        {
            if (_targetBone == null || _targetBone.Locked)
            {
                return false;
            }

            Influence targetInf = null;
            BoneWeight targetWeight = null;
            float max = 1.0f;

            //Get the matrix that influences this vertex
            IMatrixNode node = vertex.GetMatrixNode();

            bool startsAsBone = false;

            //Convert a single bone into an influence so bones can be added to it later
            if (node is MDL0BoneNode)
            {
                startsAsBone = true;
                node = new Influence(node as MDL0BoneNode);
            }

            //Duplicate the influence if it affects more than just this vertex
            targetInf = node.Users.Count > 1 ? (node as Influence).Clone() : node as Influence;

            //Find or add the current bone to the influence
            List<BoneWeight> weights = targetInf.Weights;
            int selectedIndex = weights.Select(x => x.Bone).ToArray().IndexOf(TargetBone);
            if (selectedIndex < 0)
            {
                weights.Add(new BoneWeight(TargetBone, 0.0f));
                selectedIndex = weights.Count - 1;
            }

            //Get the weight at the index of the current bone
            targetWeight = targetInf.Weights[selectedIndex];

            //Can't do anything to a locked weight
            if (targetWeight.Locked)
            {
                return false;
            }

            //Get the sum of all weights that can be edited by subtracting all locked values from 1.0f
            max = 1.0f;
            foreach (BoneWeight b in weights)
            {
                if (b.Locked)
                {
                    max -= b.Weight;
                }
            }

            //Get the new value for the target weight
            //Clamp it between 0.0f and the max value
            switch (type)
            {
                default:
                    value = RoundValue(value, max);
                    break;
                case WeightType.Add:
                    value = RoundValue(targetWeight.Weight + value, max);
                    break;
                case WeightType.Multiply:
                    value = RoundValue(targetWeight.Weight * value, max);
                    break;
            }

            //Nothing to do if there's no change in value
            if (targetWeight.Weight == value)
            {
                return false;
            }

            //Collect all unlocked weights that are not the current weight
            //These are weights that will be changed to accomodate the current weight edit
            List<int> editableWeights = new List<int>();
            for (int i = 0; i < targetInf.Weights.Count; i++)
            {
                if (!targetInf.Weights[i].Locked && i != selectedIndex)
                {
                    editableWeights.Add(i);
                }
            }

            //Return if nothing can be edited
            if (editableWeights.Count == 0)
            {
                return false;
            }

            //Set the current weight with the calculated value
            targetWeight.Weight = value;

            //Get the change in value, divide it by all other editable weights,
            //and then add that value to those weights to bring the overall weight sum back to 1.0f
            float perBoneDiff = (targetWeight.Weight - value) / editableWeights.Count;
            if (value < max)
            {
                foreach (int i in editableWeights)
                {
                    targetInf.Weights[i].Weight = RoundValue(targetInf.Weights[i].Weight + perBoneDiff, 1.0f);
                }
            }
            else
            {
                foreach (int i in editableWeights)
                {
                    targetInf.Weights[i].Weight = 0.0f;
                }
            }

            //Normalize the influence just in case, this will scale all weights so they add up to 1.0f
            //Don't let the modified value be normalized, lock it
            bool locked = targetWeight.Locked;
            targetWeight.Locked = true;
            targetInf.Normalize();
            targetWeight.Locked = locked;

            //Clean influence by removing zero weights
            for (int i = 0; i < targetInf.Weights.Count; i++)
            {
                if (targetInf.Weights[i].Weight <= 0.0f)
                {
                    targetInf.Weights.RemoveAt(i--);
                }
            }

            MDL0ObjectNode obj = vertex.Parent as MDL0ObjectNode;
            MDL0Node model = obj.Model;
            IMatrixNode matrixNode;

            //See if the influence is just one bone
            if (targetInf.Weights.Count == 1)
            {
                matrixNode = targetInf.Weights[0].Bone;
                if (!startsAsBone && !_anyConverted.Contains(obj))
                {
                    _anyConverted.Add(obj);
                }
            }
            else
            {
                matrixNode = model._influences.FindOrCreate(targetInf);
                if (startsAsBone && !_anyConverted.Contains(obj))
                {
                    _anyConverted.Add(obj);
                }
            }

            //Move influence to each vertex before modifying the influence of one vertex
            if (obj.MatrixNode != null)
            {
                obj.TryConvertMatrixToVertex();
            }

            vertex.MatrixNode = matrixNode;

            if (obj.MatrixNode == null)
            {
                obj.TryConvertMatrixToObject();
            }

            return true;
        }

        public void UpdateValues()
        {
            lstBoneWeights.RefreshItems();
            btnAdd.Enabled = _targetBoneWeight.Weight != 1.0f;
            btnSubtract.Enabled = _targetBoneWeight.Weight != 0.0f;
            btnRemove.Enabled = _targetBoneWeight != null;
            if (_targetBoneWeight != null)
            {
                numWeight.Value = _targetBoneWeight.Weight * 100.0f;
            }
        }

        public void BoneChanged()
        {
            TargetBone = SelectedBone;
        }

        private void lstBoneWeights_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstBoneWeights.SelectedIndex >= 0)
            {
                TargetBoneWeight = lstBoneWeights.Items[lstBoneWeights.SelectedIndex] as BoneWeight;
            }
            else
            {
                TargetBoneWeight = null;
            }
        }

        private void btnCopy_Click(object sender, EventArgs e)
        {
        }

        private void btnPaste_Click(object sender, EventArgs e)
        {
        }

        private void btnBlend_Click(object sender, EventArgs e)
        {
        }

        private void btnRemoveBone_Click(object sender, EventArgs e)
        {
            if (TargetBoneWeight != null && lstBoneWeights.SelectedIndex != -1)
            {
                SetWeight(0.0f);
                ResetList();
            }
        }

        private void btnSubtract_Click(object sender, EventArgs e)
        {
            IncrementWeight(-numAdd.Value / 100.0f);
            ResetList();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            IncrementWeight(numAdd.Value / 100.0f);
            ResetList();
        }

        private void btnSetWeight_Click(object sender, EventArgs e)
        {
            SetWeight(numWeight.Value / 100.0f);
            ResetList();
        }

        private void btnLock_Click(object sender, EventArgs e)
        {
            int i = lstBoneWeights.SelectedIndex;
            TargetBone.Locked = !TargetBone.Locked;
            lstBoneWeights.RefreshItems();
            lstBoneWeights.SelectedIndex = i;
            btnLock.Text = _targetBone.Locked ? "Unlock" : "Lock";
        }

        private void lstBoneWeights_DrawItem(object sender, DrawItemEventArgs e)
        {
            e.DrawBackground();

            bool selected = (e.State & DrawItemState.Selected) == DrawItemState.Selected;

            int index = e.Index;
            if (index >= 0 && index < lstBoneWeights.Items.Count)
            {
                string text = lstBoneWeights.Items[index].ToString();
                Graphics g = e.Graphics;

                Color color = selected ? Color.FromKnownColor(KnownColor.Highlight) :
                    _bones[index].Locked ? Color.Red : Color.White;
                g.FillRectangle(new SolidBrush(color), e.Bounds);

                g.DrawString(text, e.Font, selected ? Brushes.White : Brushes.Black,
                    lstBoneWeights.GetItemRectangle(index).Location);
            }

            e.DrawFocusRectangle();
        }

        private void splitter2_SplitterMoving(object sender, SplitterEventArgs e)
        {
            //int diff = e.Y - e.SplitY;
            //_mainWindow.AnimCtrlPnl.Height += diff;
        }

        private bool _resizing;
        private int o;

        private void splitter2_MouseDown(object sender, MouseEventArgs e)
        {
            _resizing = true;
            o = e.Y;
        }

        private void splitter2_MouseMove(object sender, MouseEventArgs e)
        {
            if (_resizing)
            {
                //    _mainWindow.AnimEditors.Height += o - e.Y; 
            }
        }

        private void splitter2_MouseUp(object sender, MouseEventArgs e)
        {
            _resizing = false;
        }

        private void splitter1_MouseDown(object sender, MouseEventArgs e)
        {
            _resizing = true;
            o = e.X;
        }

        private void splitter1_MouseUp(object sender, MouseEventArgs e)
        {
            _resizing = false;
            //_mainWindow.CheckDimensions();
        }

        private void splitter1_MouseMove(object sender, MouseEventArgs e)
        {
            //if (_resizing)
            //    _mainWindow.AnimCtrlPnl.Width += e.X - o;
        }

        private void numWeight_ValueChanged(object sender, EventArgs e)
        {
            //btnSetWeight_Click(sender, e);
        }

        private void btnMult_Click(object sender, EventArgs e)
        {
            MultiplyWeight(numMult.Value);
        }

        private void btnDiv_Click(object sender, EventArgs e)
        {
            MultiplyWeight(1.0f / numMult.Value);
        }
    }

    public class RefreshableListBox : ListBox
    {
        public new void RefreshItem(int index)
        {
            base.RefreshItem(index);
        }

        public new void RefreshItems()
        {
            base.RefreshItems();
        }
    }
}