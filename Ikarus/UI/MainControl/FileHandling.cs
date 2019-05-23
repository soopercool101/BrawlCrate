using System;
using BrawlLib.SSBB.ResourceNodes;
using System.Windows.Forms;

namespace Ikarus.UI
{
    public partial class MainControl : ModelEditorBase
    {
        private void ModelEditControl_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Copy;
            else
                e.Effect = DragDropEffects.None;
        }

        private void ModelEditControl_DragDrop(object sender, DragEventArgs e)
        {
            Array a = (Array)e.Data.GetData(DataFormats.FileDrop);
            if (a != null)
            {
                string s = null;
                for (int i = 0; i < a.Length; i++)
                {
                    s = a.GetValue(i).ToString();
                    this.BeginInvoke(m_DelegateOpenFile, new Object[] { s });
                }
            }
        }

        public void RemoveTarget(MDL0Node model)
        {
            if (_targetModels.Contains(model))
                _targetModels.Remove(model);
            modelPanel.RemoveTarget(model);
        }

        public bool CloseExternal()
        {
            return true;
        }

        public bool CloseFiles() 
        {
            try
            {
                if (TargetModel != null)
                    TargetModel.ApplyCHR(null, 0);
                ResetBoneColors();
                return CloseExternal() && scriptPanel.scriptPanel.CloseReferences();
            }
            catch { return true; }
        }
    }
}
