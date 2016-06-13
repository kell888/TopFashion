using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using KellWorkFlow;

namespace TopFashion
{
    public partial class FlowForm : Form
    {
        public FlowForm(ConfigClient owner)
        {
            InitializeComponent();
            this.flowControl1.TheOwner = owner;
        }

        public void SetFlow(Flow flow)
        {
            if (flow != null)
            {
                textBox1.Text = flow.Name;
                flowControl1.Nodes = flow.Stages;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
