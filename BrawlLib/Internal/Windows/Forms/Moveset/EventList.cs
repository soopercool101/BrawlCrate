using BrawlLib.SSBB.ResourceNodes;
using System;
using System.Windows.Forms;

namespace BrawlLib.Internal.Windows.Forms.Moveset
{
    public partial class FormEventList : Form
    {
        public DialogResult status;
        public long eventEvent;
        public MoveDefNode p;

        public FormEventList()
        {
            InitializeComponent();
        }

        public void Setup()
        {
            //Add each event to the event list, but omit any events lacking a formal name.
            if (lstEvents.Items.Count <= 0)
            {
                foreach (ActionEventInfo e in MoveDefNode.EventDictionary.Values)
                {
                    if (!(e._name == null || e._name == ""))
                    {
                        lstEvents.Items.Add(e);
                    }
                }
            }

            txtEventId.Text = Helpers.Hex8(eventEvent);
            status = DialogResult.Cancel;
        }

        private void FormEventList_Load(object sender, EventArgs e)
        {
            Setup();
        }

        private void FormEventList_FormClosing(object sender, FormClosingEventArgs e)
        {
        }

        private void lstEvents_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstEvents.SelectedIndex == -1)
            {
                return;
            }

            txtEventId.Text = Helpers.Hex8((lstEvents.SelectedItem as ActionEventInfo).idNumber);
        }

        private void txtEventId_TextChanged(object sender, EventArgs e)
        {
            if (txtEventId.Text.Length != 8)
            {
                return;
            }

            string eventId = txtEventId.Text;

            //Select the event corresponding to the id input.
            lstEvents.SelectedIndex = -1;
            for (int i = 0; i < lstEvents.Items.Count; i++)
            {
                if (eventId == Helpers.Hex8((lstEvents.Items[i] as ActionEventInfo).idNumber))
                {
                    lstEvents.SelectedIndex = i;
                    break;
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            status = DialogResult.Cancel;
            Close();
        }

        private void btnDone_Click(object sender, EventArgs e)
        {
            try
            {
                eventEvent = Helpers.UnHex(txtEventId.Text);
            }
            catch
            {
                eventEvent = 0;
            }

            if (eventEvent == 0)
            {
                MessageBox.Show("Invalid event Id.", "Invalid Id");
                return;
            }

            status = DialogResult.OK;
            Close();
        }
    }
}