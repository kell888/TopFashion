using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using KellWorkFlow;

namespace TopFashion
{
    [DefaultEvent("Selected")]
    public partial class NodeControl : UserControl
    {
        public NodeControl()
        {
            InitializeComponent();
        }

        int index = 0;

        public event EventHandler<NodeArgs> Selected;

        public int Index
        {
            get { return index; }
            set { index = value; }
        }

        public string NodeName
        {
            get
            {
                return label1.Text;
            }
            set
            {
                label1.Text = value;
            }
        }

        public void SetStatus(TaskStatus status)
        {
            if (status == TaskStatus.Initiative)
            {
                label1.ForeColor = Color.Green;
            }
            else if (status == TaskStatus.Processing)
            {
                label1.ForeColor = Color.Blue;
            }
            else if (status == TaskStatus.Processed)
            {
                label1.ForeColor = Color.Fuchsia;
            }
            else if (status == TaskStatus.Paused)
            {
                label1.ForeColor = Color.Red;
            }
            else if (status == TaskStatus.Finished)
            {
                label1.ForeColor = Color.Black;
            }
            label1.Refresh();
        }

        private void label1_Click(object sender, EventArgs e)
        {
            if (Selected != null)
                Selected(this, new NodeArgs(index));
        }

        private void Fixed_Resize(object sender, EventArgs e)
        {
            this.Height = 38;
        }
    }

    public class NodeArgs : EventArgs
    {
        int index;

        public int Index
        {
            get { return index; }
        }

        public NodeArgs(int index)
        {
            this.index = index;
        }
    }
}
