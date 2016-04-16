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
    public partial class FlowTemplateControl : UserControl
    {
        public FlowTemplateControl()
        {
            InitializeComponent();
            nodes = new List<TaskStageTemplate>();
        }

        const int left = 10;
        const int top = 10;
        const int interval = 10;
        ConfigClient owner;
        TaskStageTemplate selected;
        List<TaskStageTemplate> nodes;

        private void RefreshLayout()
        {
            panel2.SuspendLayout();
            panel2.Controls.Clear();
            int x = left;
            int y = top;
            int offset = 0;
            int nextPos = left;
            int nextPosY = 0;
            for (int i = 0; i < nodes.Count; i++)
            {
                TaskStageTemplate node = nodes[i];
                LinkControl lc = new LinkControl();
                lc.Index = i;
                NodeControl nc = new NodeControl();
                nc.Index = i;
                nc.NodeName = node.Name;
                nc.Selected += new EventHandler<NodeArgs>(nc_Selected);
                offset = nc.Width + lc.Width + interval;
                int cols = nextPos % this.Width;
                int rows = (nextPos + offset) / this.Width;
                if (rows > 0)
                {
                    nextPosY++;
                    x = left;
                }
                else
                {
                    x = cols;
                    if (cols < offset)
                    {
                        x = left;
                    }
                }
                nextPos = x + offset;
                int maxHeight = Math.Max(nc.Height, lc.Height);
                y = top + (maxHeight + interval) * nextPosY;
                lc.Location = new Point(x, y);
                nc.Location = new Point(x + lc.Width, y);
                nc.Tag = node;
                panel2.Controls.Add(lc);
                panel2.Controls.Add(nc);
            }
            panel2.ResumeLayout(true);
        }

        void nc_Selected(object sender, NodeArgs e)
        {
            NodeControl nc = sender as NodeControl;
            TaskStageTemplate tmp = nc.Tag as TaskStageTemplate;
            bool canceled = false;
            if (tmp == selected)
            {
                canceled = true;
                selected = null;
            }
            else
            {
                selected = tmp;
            }
            foreach (Control c in panel2.Controls)
            {
                if (c is NodeControl)
                {
                    NodeControl ncc = c as NodeControl;
                    TaskStageTemplate node = ncc.Tag as TaskStageTemplate;
                    if (!canceled && node == selected)
                        ncc.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
                    else
                        ncc.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
                }
            }
        }

        public ConfigClient TheOwner
        {
            get { return owner; }
            set { owner = value; }
        }

        [Browsable(false)]
        public List<TaskStageTemplate> Nodes
        {
            get
            {
                TaskStageTemplateLogic tstl = TaskStageTemplateLogic.GetInstance();
                foreach (TaskStageTemplate node in nodes)
                {
                    if (tstl.ExistsWhere("where ID=" + node.ID))
                    {
                        tstl.UpdateTaskStageTemplate(node);
                    }
                    else
                    {
                        int id = tstl.AddTaskStageTemplate(node);
                        node.ID = id;
                    }
                }
                return nodes;
            }
            set
            {
                nodes.Clear();
                if (value != null)
                {
                    foreach (TaskStageTemplate node in value)
                    {
                        nodes.Add(node);
                    }
                }
                RefreshLayout();
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

        private void UpdateNode(int index, TaskStageTemplate node)
        {
            if (index > -1)
            {
                nodes[index] = node;
                RefreshLayout();
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

        private NodeControl SelectedNode
        {
            get
            {
                if (selected != null)
                {
                    foreach (Control c in panel2.Controls)
                    {
                        if (c is NodeControl)
                        {
                            NodeControl ncc = c as NodeControl;
                            TaskStageTemplate node = ncc.Tag as TaskStageTemplate;
                            if (node == selected)
                            {
                                return ncc;
                            }
                        }
                    }
                }
                return null;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (selected != null && SelectedNode != null)
            {
                NodeForm nf = new NodeForm(owner);
                nf.Template = selected;
                if (nf.ShowDialog() == DialogResult.OK)
                {
                    UpdateNode(SelectedNode.Index, nf.Template);
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
                if (MessageBox.Show("确定要删除该节点么？", "删除提醒", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                {
                    RemoveNode(selected);
                }
            }
            else
            {
                MessageBox.Show("请先选定一个要删除的节点！");
            }
        }

        private void panel2_Resize(object sender, EventArgs e)
        {
            RefreshLayout();
        }
    }
}
