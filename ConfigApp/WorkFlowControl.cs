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
    public partial class WorkFlowControl : UserControl
    {
        public WorkFlowControl()
        {
            InitializeComponent();
            nodes = new List<TaskStageTemplate>();
        }

        ConfigClient owner;
        TaskStageTemplate selected;
        List<TaskStageTemplate> nodes;

        public ConfigClient TheOwner
        {
            get { return owner; }
            set { owner = value; }
        }

        [Browsable(false)]
        public List<TaskStageTemplate> Nodes
        {
            get { return nodes; }
            set
            {
                nodes.Clear();
                if (value != null)
                {
                    foreach (TaskStageTemplate node in value)
                    {
                        nodes.Add(node);
                    }
                    RefreshLayout();
                }
            }
        }

        private void AddNode(TaskStageTemplate node)
        {
            nodes.Add(node);
            RefreshLayout();
        }

        private void RemoveNode(TaskStageTemplate selected)
        {
            if (nodes.Remove(selected))
            {
                RefreshLayout();
            }
        }

        private void UpdateNode(int id, TaskStageTemplate node)
        {
            int index = nodes.FindIndex(a => a.ID == id);
            if (index > -1)
            {
                nodes[index] = node;
                RefreshLayout();
            }
        }

        private void RefreshLayout()
        {
            foreach (TaskStageTemplate node in nodes)
            {

            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            NodeForm nf = new NodeForm(owner);
            if (nf.ShowDialog() == DialogResult.OK)
            {
                AddNode(nf.Template);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            NodeForm nf = new NodeForm(owner);
            if (nf.ShowDialog() == DialogResult.OK)
            {
                AddNode(nf.Template);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            NodeForm nf = new NodeForm(owner);
            if (nf.ShowDialog() == DialogResult.OK)
            {
                AddNode(nf.Template);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (selected != null)
            {
                NodeForm nf = new NodeForm(owner);
                nf.Template = selected;
                if (nf.ShowDialog() == DialogResult.OK)
                {
                    UpdateNode(selected.ID, nf.Template);
                }
            }
            else
            {
                MessageBox.Show("请先选定一个要修改的节点！");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (selected != null)
            {
                RemoveNode(selected);
            }
            else
            {
                MessageBox.Show("请先选定一个要删除的节点！");
            }
        }
    }
}
