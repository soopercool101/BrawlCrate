using BrawlLib.Modeling;
using BrawlLib.Modeling.Triangle_Converter;
using BrawlLib.SSBB.ResourceNodes;
using BrawlLib.Wii.Models;
using System.Collections.Generic;
using System.ComponentModel;

namespace System.Windows.Forms
{
    public partial class ObjectOptimizerForm : Form
    {
        public ObjectOptimizerForm()
        {
            InitializeComponent();
        }

        ResourceNode _target;
        int _originalPointCount, _newPointCount;
        List<ObjectOptimization> _results = new List<ObjectOptimization>();
        bool _processPending = false, _endPending = false;

        public DialogResult ShowDialog(ResourceNode o)
        {
            _target = o;

            b = new BackgroundWorker();
            b.RunWorkerCompleted += b_RunWorkerCompleted;
            b.DoWork += b_DoWork;
            b.WorkerSupportsCancellation = true;
            b.WorkerReportsProgress = false;

            _updating = true;
            Collada.ImportOptions i = BrawlLib.Properties.Settings.Default.ColladaImportOptions;
            numCacheSize.Value = i._cacheSize;
            numMinStripLen.Value = i._minStripLen;
            chkPushCacheHits.Checked = i._pushCacheHits;
            chkUseStrips.Checked = i._useTristrips;
            if (_target is MDL0Node)
            {
                if (((MDL0Node)_target)._objList != null)
                    foreach (MDL0ObjectNode w in ((MDL0Node)_target)._objList)
                        _results.Add(new ObjectOptimization(w));
                lblOldCount.Text = (_originalPointCount = ((MDL0Node)o)._numFacepoints).ToString();
            }
            else
            {
                _results.Add(new ObjectOptimization((MDL0ObjectNode)_target));
                lblOldCount.Text = (_originalPointCount = ((MDL0ObjectNode)o)._numFacepoints).ToString();
            }

            chkForceCCW.Checked = i._forceCCW;
            _updating = false;

            Optimize();

            return base.ShowDialog();
        }

        void b_DoWork(object sender, DoWorkEventArgs e)
        {
            if (e == null) return;
            ObjectOptimizerForm form = e.Argument as ObjectOptimizerForm;
            form._newPointCount = 0;

            foreach (ObjectOptimization a in form._results)
            {
                if (a == null ||
                    a._object == null ||
                    a._object._manager == null ||
                    a._object._manager._triangles == null ||
                    a._facepoints == null)
                    return;

                TriangleConverter triConverter = new TriangleConverter(chkUseStrips.Checked, (uint)numCacheSize.Value, (uint)numMinStripLen.Value, chkPushCacheHits.Checked);
                Facepoint[] points = new Facepoint[a._object._manager._triangles._indices.Length];
                uint[] indices = a._object._manager._triangles._indices;
                bool ccw = Collada._importOptions._forceCCW;

                //Indices are written in reverse for each triangle, 
                //so they need to be set to a triangle in reverse if not CCW
                for (int t = 0; t < a._object._manager._triangles._indices.Length; t++)
                    points[ccw ? t : (t - (t % 3)) + (2 - (t % 3))] = a._facepoints[indices[t]];

                int pc, fc;
                List<PrimitiveGroup> p = triConverter.GroupPrimitives(points, out pc, out fc);

                if (chkAllowIncrease.Checked || pc < a._pointCount)
                {
                    a._pointCount = pc;
                    a._faceCount = fc;
                    a._groups = p;
                }

                form._newPointCount += a._pointCount;
            }
            e.Result = form;
        }

        void b_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            float percentChange = 100.0f - ((float)_newPointCount / (float)_originalPointCount) * 100.0f;
            lblPercentChange.Text = String.Format("{0}% {1}", Math.Round(Math.Abs(percentChange), 3), percentChange < 0 ? "Increase" : "Decrease");
            lblNewCount.Text = _newPointCount.ToString();

            if (_processPending)
            {
                _processPending = false;
                Optimize();
            }
            else if (_endPending)
                btnOkay_Click(null, null);
        }

        class ObjectOptimization
        {
            public ObjectOptimization(MDL0ObjectNode o)
            {
                _object = o;
                _facepoints = _object._manager.MergeExternalFaceData(_object);
                _pointCount = _object._numFacepoints;
                _faceCount = _object._numFaces;
            }

            public MDL0ObjectNode _object;
            public Facepoint[] _facepoints;

            public int _pointCount, _faceCount;
            public List<PrimitiveGroup> _groups;
        }

        private void Optimize()
        {
            lblPercentChange.Text = "Working...";
            if (b.IsBusy)
                _processPending = true;
            else
                b.RunWorkerAsync(this);
        }

        private void btnOkay_Click(object sender, EventArgs e)
        {
            if (b.IsBusy)
            {
                _endPending = true;
                return;
            }

            foreach (ObjectOptimization a in _results)
            {
                if (a._groups == null)
                    continue;

                a._object._manager._primGroups = a._groups;
                a._object._numFacepoints = a._pointCount;
                a._object._numFaces = a._faceCount;
                a._object._reOptimized = true;
                a._object.SignalPropertyChange();

                if (a._object._vertexNode.Format != WiiVertexComponentType.Float)
                    a._object._vertexNode._forceRebuild = a._object._vertexNode._forceFloat = chkUseStrips.Checked;
            }

            if (_target is MDL0Node)
                ((MDL0Node)_target)._numFacepoints = _newPointCount;

            _target.SignalPropertyChange();
            _target.UpdateProperties();
            BrawlLib.Properties.Settings.Default.Save();
            DialogResult = Forms.DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = Forms.DialogResult.Cancel;
            Close();
        }

        bool _updating = false;
        BackgroundWorker b;
        private void Update(object sender, EventArgs e)
        {
            if (_updating)
                return;

            _updating = true;
            if (numCacheSize.Value < 0)
                numCacheSize.Value = 0;
            if (numMinStripLen.Value < 2)
                numMinStripLen.Value = 2;
            _updating = false;

            BrawlLib.Properties.Settings.Default.ColladaImportOptions._cacheSize = (uint)numCacheSize.Value;
            BrawlLib.Properties.Settings.Default.ColladaImportOptions._minStripLen = (uint)numMinStripLen.Value;
            BrawlLib.Properties.Settings.Default.ColladaImportOptions._pushCacheHits = chkPushCacheHits.Checked;
            BrawlLib.Properties.Settings.Default.ColladaImportOptions._useTristrips = chkUseStrips.Checked;
            BrawlLib.Properties.Settings.Default.ColladaImportOptions._forceCCW = chkForceCCW.Checked;

            Optimize();
        }
    }
}
