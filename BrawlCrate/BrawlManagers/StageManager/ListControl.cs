using BrawlLib.SSBB.ResourceNodes;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace BrawlCrate.BrawlManagers.StageManager
{
    public partial class ListControl : UserControl
    {
        /*public Control MainControl {
            get {
                ControlCollection controls = panel1.Controls;
                if (controls.Count > 0) {
                    return controls[0];
                } else {
                    return null;
                }
            }
            set {
                ControlCollection controls = panel1.Controls;
                controls.Clear();
                controls.Add(value);
            }
        }

        public ComboBox.ObjectCollection Items {
            get {
                return comboBox1.Items;
            }
        }

        public MSBinNode CurrentNode {
            get {
                return msBinEditor1.CurrentNode;
            }
            set {
                msBinEditor1.CurrentNode = value;
            }
        }*/

        public ListControl(List<MSBinNode> nodes)
        {
            InitializeComponent();

            comboBox1.Items.AddRange(nodes.ToArray());

            comboBox1.SelectedIndex = comboBox1.Items.Count - 1;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            msBinEditor1.CurrentNode = (MSBinNode) comboBox1.SelectedItem;
        }
    }
}