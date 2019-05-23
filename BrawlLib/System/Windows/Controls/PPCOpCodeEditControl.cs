using System.PowerPcAssembly;

namespace System.Windows.Forms
{
    public partial class PPCOpCodeEditControl : UserControl
    {
        public PPCOpCode _code;
        public bool _canFollowBranch;
        bool _updating = false;

        public void SetCode(PPCOpCode code)
        {
            propertyGrid1.SelectedObject = _code = (uint)code;
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
            if (OnOpChanged != null)
                OnOpChanged();
        }

        private void btnGoToBranch_Click(object sender, EventArgs e)
        {
            if (OnBranchFollowed != null)
                OnBranchFollowed();
        }

        private void cboOpCode_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_updating)
                return;
        }
    }
}