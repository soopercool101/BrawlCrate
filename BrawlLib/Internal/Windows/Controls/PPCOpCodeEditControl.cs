using BrawlLib.Internal.PowerPCAssembly;
using System;
using System.Windows.Forms;

namespace BrawlLib.Internal.Windows.Controls
{
    public partial class PPCOpCodeEditControl : UserControl
    {
        public PPCOpCode _code;
        public bool _canFollowBranch;
        private bool _updating;

        public void SetCode(PPCOpCode code)
        {
            propertyGrid1.SelectedObject = _code = (uint) code;
            btnGoToBranch.Visible = _canFollowBranch && _code is PPCBranch;
            _updating = true;
            //cboOpCode.SelectedIndex = 
            _updating = false;
        }

        public PPCOpCodeEditControl()
        {
            InitializeComponent();
            cboOpCode.DataSource = Enum.GetNames(typeof(PPCMnemonic));
        }

        public event Action OnOpChanged, OnBranchFollowed;

        private void propertyGrid1_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            OnOpChanged?.Invoke();
        }

        private void btnGoToBranch_Click(object sender, EventArgs e)
        {
            OnBranchFollowed?.Invoke();
        }

        private void cboOpCode_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_updating)
            {
                return;
            }
        }
    }
}