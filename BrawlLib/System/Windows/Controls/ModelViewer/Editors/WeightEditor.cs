using BrawlLib.SSBB.ResourceNodes;
using BrawlLib.Modeling;
using System.ComponentModel;
using System.Drawing;
using System.Collections.Generic;
using BrawlLib.Wii.Models;
using System.Linq;

namespace System.Windows.Forms
{
    public class WeightEditor : UserControl
    {
        #region Designer

        private System.ComponentModel.IContainer components;
        private void InitializeComponent()
        {
            this.btnSetWeight = new System.Windows.Forms.Button();
            this.btnBlend = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnSubtract = new System.Windows.Forms.Button();
            this.btnLock = new System.Windows.Forms.Button();
            this.lblBoneName = new System.Windows.Forms.Label();
            this.btnRemove = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.numMult = new System.Windows.Forms.NumericInputBox();
            this.btnMult = new System.Windows.Forms.Button();
            this.btnDiv = new System.Windows.Forms.Button();
            this.numAdd = new System.Windows.Forms.NumericInputBox();
            this.numWeight = new System.Windows.Forms.NumericInputBox();
            this.btnPaste = new System.Windows.Forms.Button();
            this.btnCopy = new System.Windows.Forms.Button();
            this.lstBoneWeights = new System.Windows.Forms.RefreshableListBox();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnSetWeight
            // 
            this.btnSetWeight.Enabled = false;
            this.btnSetWeight.Location = new System.Drawing.Point(67, 28);
            this.btnSetWeight.Name = "btnSetWeight";
            this.btnSetWeight.Size = new System.Drawing.Size(61, 22);
            this.btnSetWeight.TabIndex = 2;
            this.btnSetWeight.Text = "Set";
            this.btnSetWeight.UseVisualStyleBackColor = true;
            this.btnSetWeight.Click += new System.EventHandler(this.btnSetWeight_Click);
            // 
            // btnBlend
            // 
            this.btnBlend.Location = new System.Drawing.Point(129, 28);
            this.btnBlend.Name = "btnBlend";
            this.btnBlend.Size = new System.Drawing.Size(62, 22);
            this.btnBlend.TabIndex = 4;
            this.btnBlend.Text = "Blend";
            this.btnBlend.UseVisualStyleBackColor = true;
            this.btnBlend.Visible = false;
            this.btnBlend.Click += new System.EventHandler(this.btnBlend_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Enabled = false;
            this.btnAdd.Location = new System.Drawing.Point(67, 52);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(30, 22);
            this.btnAdd.TabIndex = 7;
            this.btnAdd.Text = "+";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnSubtract
            // 
            this.btnSubtract.Enabled = false;
            this.btnSubtract.Location = new System.Drawing.Point(98, 52);
            this.btnSubtract.Name = "btnSubtract";
            this.btnSubtract.Size = new System.Drawing.Size(30, 22);
            this.btnSubtract.TabIndex = 8;
            this.btnSubtract.Text = "-";
            this.btnSubtract.UseVisualStyleBackColor = true;
            this.btnSubtract.Click += new System.EventHandler(this.btnSubtract_Click);
            // 
            // btnLock
            // 
            this.btnLock.Enabled = false;
            this.btnLock.Location = new System.Drawing.Point(2, 4);
            this.btnLock.Name = "btnLock";
            this.btnLock.Size = new System.Drawing.Size(64, 22);
            this.btnLock.TabIndex = 10;
            this.btnLock.Text = "Lock";
            this.btnLock.UseVisualStyleBackColor = true;
            this.btnLock.Click += new System.EventHandler(this.btnLock_Click);
            // 
            // lblBoneName
            // 
            this.lblBoneName.AutoSize = true;
            this.lblBoneName.Location = new System.Drawing.Point(134, 9);
            this.lblBoneName.Name = "lblBoneName";
            this.lblBoneName.Size = new System.Drawing.Size(32, 13);
            this.lblBoneName.TabIndex = 11;
            this.lblBoneName.Text = "Bone";
            // 
            // btnRemove
            // 
            this.btnRemove.Enabled = false;
            this.btnRemove.Location = new System.Drawing.Point(67, 4);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new System.Drawing.Size(61, 22);
            this.btnRemove.TabIndex = 12;
            this.btnRemove.Text = "Remove";
            this.btnRemove.UseVisualStyleBackColor = true;
            this.btnRemove.Click += new System.EventHandler(this.btnRemoveBone_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.numMult);
            this.panel1.Controls.Add(this.btnMult);
            this.panel1.Controls.Add(this.btnDiv);
            this.panel1.Controls.Add(this.numAdd);
            this.panel1.Controls.Add(this.btnAdd);
            this.panel1.Controls.Add(this.numWeight);
            this.panel1.Controls.Add(this.btnBlend);
            this.panel1.Controls.Add(this.btnPaste);
            this.panel1.Controls.Add(this.btnSetWeight);
            this.panel1.Controls.Add(this.btnCopy);
            this.panel1.Controls.Add(this.btnSubtract);
            this.panel1.Controls.Add(this.btnLock);
            this.panel1.Controls.Add(this.lblBoneName);
            this.panel1.Controls.Add(this.btnRemove);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(130, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(130, 103);
            this.panel1.TabIndex = 14;
            // 
            // numMult
            // 
            this.numMult.Enabled = false;
            this.numMult.Integral = false;
            this.numMult.Location = new System.Drawing.Point(3, 77);
            this.numMult.MaximumValue = 3.402823E+38F;
            this.numMult.MinimumValue = -3.402823E+38F;
            this.numMult.Name = "numMult";
            this.numMult.Size = new System.Drawing.Size(62, 20);
            this.numMult.TabIndex = 16;
            this.numMult.Text = "1.05";
            // 
            // btnMult
            // 
            this.btnMult.Enabled = false;
            this.btnMult.Location = new System.Drawing.Point(67, 76);
            this.btnMult.Name = "btnMult";
            this.btnMult.Size = new System.Drawing.Size(30, 22);
            this.btnMult.TabIndex = 14;
            this.btnMult.Text = "x";
            this.btnMult.UseVisualStyleBackColor = true;
            this.btnMult.Click += new System.EventHandler(this.btnMult_Click);
            // 
            // btnDiv
            // 
            this.btnDiv.Enabled = false;
            this.btnDiv.Location = new System.Drawing.Point(98, 76);
            this.btnDiv.Name = "btnDiv";
            this.btnDiv.Size = new System.Drawing.Size(30, 22);
            this.btnDiv.TabIndex = 15;
            this.btnDiv.Text = "/";
            this.btnDiv.UseVisualStyleBackColor = true;
            this.btnDiv.Click += new System.EventHandler(this.btnDiv_Click);
            // 
            // numAdd
            // 
            this.numAdd.Enabled = false;
            this.numAdd.Integral = false;
            this.numAdd.Location = new System.Drawing.Point(3, 53);
            this.numAdd.MaximumValue = 3.402823E+38F;
            this.numAdd.MinimumValue = -3.402823E+38F;
            this.numAdd.Name = "numAdd";
            this.numAdd.Size = new System.Drawing.Size(62, 20);
            this.numAdd.TabIndex = 13;
            this.numAdd.Text = "10";
            // 
            // numWeight
            // 
            this.numWeight.Enabled = false;
            this.numWeight.Integral = false;
            this.numWeight.Location = new System.Drawing.Point(3, 29);
            this.numWeight.MaximumValue = 3.402823E+38F;
            this.numWeight.MinimumValue = -3.402823E+38F;
            this.numWeight.Name = "numWeight";
            this.numWeight.Size = new System.Drawing.Size(62, 20);
            this.numWeight.TabIndex = 3;
            this.numWeight.Text = "100";
            this.numWeight.ValueChanged += new System.EventHandler(this.numWeight_ValueChanged);
            // 
            // btnPaste
            // 
            this.btnPaste.Location = new System.Drawing.Point(129, 75);
            this.btnPaste.Name = "btnPaste";
            this.btnPaste.Size = new System.Drawing.Size(62, 22);
            this.btnPaste.TabIndex = 6;
            this.btnPaste.Text = "Paste";
            this.btnPaste.UseVisualStyleBackColor = true;
            this.btnPaste.Visible = false;
            this.btnPaste.Click += new System.EventHandler(this.btnPaste_Click);
            // 
            // btnCopy
            // 
            this.btnCopy.Location = new System.Drawing.Point(129, 51);
            this.btnCopy.Name = "btnCopy";
            this.btnCopy.Size = new System.Drawing.Size(62, 22);
            this.btnCopy.TabIndex = 5;
            this.btnCopy.Text = "Copy";
            this.btnCopy.UseVisualStyleBackColor = true;
            this.btnCopy.Visible = false;
            this.btnCopy.Click += new System.EventHandler(this.btnCopy_Click);
            // 
            // lstBoneWeights
            // 
            this.lstBoneWeights.Dock = System.Windows.Forms.DockStyle.Left;
            this.lstBoneWeights.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.lstBoneWeights.FormattingEnabled = true;
            this.lstBoneWeights.IntegralHeight = false;
            this.lstBoneWeights.Location = new System.Drawing.Point(0, 0);
            this.lstBoneWeights.Name = "lstBoneWeights";
            this.lstBoneWeights.Size = new System.Drawing.Size(130, 103);
            this.lstBoneWeights.TabIndex = 0;
            this.lstBoneWeights.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.lstBoneWeights_DrawItem);
            this.lstBoneWeights.SelectedIndexChanged += new System.EventHandler(this.lstBoneWeights_SelectedIndexChanged);
            // 
            // WeightEditor
            // 
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.lstBoneWeights);
            this.MinimumSize = new System.Drawing.Size(260, 103);
            this.Name = "WeightEditor";
            this.Size = new System.Drawing.Size(260, 103);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        public WeightEditor() { InitializeComponent(); }
        private BindingList<BoneWeight> _targetWeights;

        public IBoneNode[] Bones { get { return _targetWeights.Select(x => x.Bone).ToArray(); } }
        public float[] Weights { get { return _targetWeights.Select(x => x.Weight).ToArray(); } }

        public ModelEditorBase _mainWindow;
        private Button btnSetWeight;
        private NumericInputBox numWeight;
        private Button btnBlend;
        private Button btnAdd;
        private Label lblBoneName;
        private Button btnSubtract;

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int CurrentFrame
        {
            get { return _mainWindow.CurrentFrame; }
            set { _mainWindow.CurrentFrame = value; }
        }
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public IModel TargetModel
        {
            get { return _mainWindow.TargetModel; }
            set { _mainWindow.TargetModel = value; }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public IBoneNode SelectedBone { get { return _mainWindow.SelectedBone; } set { _mainWindow.SelectedBone = value; } }

        private Vertex3 _vertex = null;
        private Button btnLock;
        private Button btnPaste;
        private Button btnCopy;
        private RefreshableListBox lstBoneWeights;

        public List<Vertex3> _targetVertices;
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public List<Vertex3> TargetVertices
        {
            get { return _targetVertices; }
            set { if (_targetVertices != value) SetVertices(value); }
        }
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public BoneWeight TargetBoneWeight
        {
            get { return _targetBoneWeight; }
            set 
            {
                if ((_targetBoneWeight = value) != null)
                    _mainWindow.SelectedBone = TargetBoneWeight.Bone;
                _mainWindow.ModelPanel.Invalidate();
            }
        }
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public IBoneNode TargetBone
        {
            get { return _targetBone; }
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
                            numWeight.Value = 100.0f;
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
                    canAdd = numAdd.Value != 0.0f;
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
        public float _weightTotal = 0;
        Dictionary<string, float[]> _totals = new Dictionary<string, float[]>();
        public List<IBoneNode> _bones;
        public void ResetList()
        {
            lstBoneWeights.Items.Clear();

            _bones = new List<IBoneNode>();
            _totals = new Dictionary<string, float[]>();
            _weightTotal = 0;
            if (_targetVertices != null)
                foreach (Vertex3 v in _targetVertices)
                {
                    List<BoneWeight> array = v.GetBoneWeights();
                    foreach (BoneWeight b in array)
                    {
                        if (!_totals.ContainsKey(b.Bone.Name))
                        {
                            _bones.Add(b.Bone);
                            _totals.Add(b.Bone.Name, new float[] { b.Weight, 1 });
                        }
                        else
                        {
                            _totals[b.Bone.Name][0] += b.Weight;
                            _totals[b.Bone.Name][1] += 1;
                        }
                        _weightTotal += b.Weight;
                    }
                }

            foreach (MDL0BoneNode b in _bones)
                lstBoneWeights.Items.Add(new BoneWeight(b, (_totals[b.Name][0] / _weightTotal)));
            
            if (_bones.Contains(_mainWindow.SelectedBone))
                lstBoneWeights.SelectedIndex = _bones.IndexOf(_mainWindow.SelectedBone);
        }

        public bool _updating = false;
        public List<MDL0ObjectNode> _anyConverted = null;

        public void SetWeight(float value)
        {
            if (TargetVertices == null || TargetVertices.Count == 0)
                return;

            _anyConverted = new List<MDL0ObjectNode>();
            foreach (Vertex3 v in TargetVertices)
                SetWeight(value, v);

            UpdateBindState(TargetVertices.ToArray());
        }
        public void SetWeight(float value, Vertex3 vertex)
        {
            Weight(value, vertex, WeightType.Set);
        }
        public void IncrementWeight(float value)
        {
            if (TargetVertices == null || TargetVertices.Count == 0)
                return;

            _anyConverted = new List<MDL0ObjectNode>();
            foreach (Vertex3 v in TargetVertices) 
                IncrementWeight(value, v);

            UpdateBindState(TargetVertices.ToArray());
        }
        public void IncrementWeight(float value, Vertex3 vertex)
        {
            Weight(value, vertex, WeightType.Add);
        }
        public void MultiplyWeight(float value)
        {
            if (TargetVertices == null || TargetVertices.Count == 0)
                return;

            _anyConverted = new List<MDL0ObjectNode>();
            foreach (Vertex3 v in TargetVertices)
                MultiplyWeight(value, v);
            
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
                if (!changed.Contains(v.Parent as MDL0ObjectNode))
                    changed.Add(v.Parent as MDL0ObjectNode);
            foreach (MDL0ObjectNode obj in changed)
            {
                //See if the object can be made into a single bind
                obj.TryConvertMatrixToObject();

                if (_anyConverted.Contains(obj))
                    obj.SetEditedAssets(false, true, true);

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
            return (float)Math.Round(value.Clamp(0.0f, max), 7);
        }
        enum WeightType
        {
            Set,
            Multiply,
            Add
        }

        //Returns true if the vertex's matrix node is changed.
        private bool Weight(float value, Vertex3 vertex, WeightType type)
        {
            if (_targetBone == null || _targetBone.Locked)
                return false;

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
            targetInf = node.Users.Count > 1 ?
                (node as Influence).Clone() :
                node as Influence;

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
                return false;

            //Get the sum of all weights that can be edited by subtracting all locked values from 1.0f
            max = 1.0f;
            foreach (BoneWeight b in weights)
                if (b.Locked)
                    max -= b.Weight;

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
                return false;

            //Collect all unlocked weights that are not the current weight
            //These are weights that will be changed to accomodate the current weight edit
            List<int> editableWeights = new List<int>();
            for (int i = 0; i < targetInf.Weights.Count; i++)
                if (!targetInf.Weights[i].Locked && i != selectedIndex)
                    editableWeights.Add(i);

            //Return if nothing can be edited
            if (editableWeights.Count == 0) 
                return false;

            //Set the current weight with the calculated value
            targetWeight.Weight = value;

            //Get the change in value, divide it by all other editable weights,
            //and then add that value to those weights to bring the overall weight sum back to 1.0f
            float perBoneDiff = (targetWeight.Weight - value) / editableWeights.Count;
            if (value < max)
                foreach (int i in editableWeights)
                    targetInf.Weights[i].Weight = RoundValue(targetInf.Weights[i].Weight + perBoneDiff, 1.0f);
            else
                foreach (int i in editableWeights)
                    targetInf.Weights[i].Weight = 0.0f;

            //Normalize the influence just in case, this will scale all weights so they add up to 1.0f
            //Don't let the modified value be normalized, lock it
            bool locked = targetWeight.Locked;
            targetWeight.Locked = true;
            targetInf.Normalize();
            targetWeight.Locked = locked;

            //Clean influence by removing zero weights
            for (int i = 0; i < targetInf.Weights.Count; i++)
                if (targetInf.Weights[i].Weight <= 0.0f)
                    targetInf.Weights.RemoveAt(i--);

            MDL0ObjectNode obj = vertex.Parent as MDL0ObjectNode;
            MDL0Node model = obj.Model;
            IMatrixNode matrixNode;

            //See if the influence is just one bone
            if (targetInf.Weights.Count == 1)
            {
                matrixNode = targetInf.Weights[0].Bone;
                if (!startsAsBone && !_anyConverted.Contains(obj))
                    _anyConverted.Add(obj);
            }
            else
            {
                matrixNode = model._influences.FindOrCreate(targetInf);
                if (startsAsBone && !_anyConverted.Contains(obj))
                    _anyConverted.Add(obj);
            }

            //Move influence to each vertex before modifying the influence of one vertex
            if (obj.MatrixNode != null)
                obj.TryConvertMatrixToVertex();
            
            vertex.MatrixNode = matrixNode;

            if (obj.MatrixNode == null)
                obj.TryConvertMatrixToObject();

            return true;
        }
        public void UpdateValues()
        {
            lstBoneWeights.RefreshItems();
            btnAdd.Enabled = _targetBoneWeight.Weight != 1.0f;
            btnSubtract.Enabled = _targetBoneWeight.Weight != 0.0f;
            btnRemove.Enabled = _targetBoneWeight != null;
            if (_targetBoneWeight != null)
                numWeight.Value = _targetBoneWeight.Weight * 100.0f;
        }
        public void BoneChanged() { TargetBone = SelectedBone; }

        private void lstBoneWeights_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstBoneWeights.SelectedIndex >= 0)
                TargetBoneWeight = lstBoneWeights.Items[lstBoneWeights.SelectedIndex] as BoneWeight;
            else
                TargetBoneWeight = null;
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

            bool selected = ((e.State & DrawItemState.Selected) == DrawItemState.Selected);

            int index = e.Index;
            if (index >= 0 && index < lstBoneWeights.Items.Count)
            {
                string text = lstBoneWeights.Items[index].ToString();
                Graphics g = e.Graphics;

                Color color = selected ? Color.FromKnownColor(KnownColor.Highlight) : _bones[index].Locked ? Color.Red : Color.White;
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

        bool _resizing = false;
        int o = 0;
        private void splitter2_MouseDown(object sender, MouseEventArgs e)
        {
            _resizing = true;
            o = e.Y;
        }

        private void splitter2_MouseMove(object sender, MouseEventArgs e)
        {
            //if (_resizing)
            //    _mainWindow.AnimEditors.Height += o - e.Y; 
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
        public new void RefreshItem(int index) { base.RefreshItem(index); }
        public new void RefreshItems() { base.RefreshItems(); }
    }
}
