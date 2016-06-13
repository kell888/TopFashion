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
    public partial class FlowControl : UserControl
    {
        public FlowControl()
        {
            InitializeComponent();
            nodes = new List<TaskStage>();
        }

        const int left = 10;
        const int top = 10;
        const int interval = 10;
        ConfigClient owner;
        TaskStage selected;
        List<TaskStage> nodes;

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
                TaskStage node = nodes[i];
                LinkControl lc = new LinkControl();
                lc.Index = i;
                NodeControl nc = new NodeControl();
                nc.Index = i;
                nc.NodeName = node.Name;
                nc.SetStatus(node.Status);
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
            TaskStage tmp = nc.Tag as TaskStage;
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
                    TaskStage node = nc.Tag as TaskStage;
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
        public List<TaskStage> Nodes
        {
            get
            {
                TaskStageLogic tstl = TaskStageLogic.GetInstance();
                foreach (TaskStage node in nodes)
                {
                    if (tstl.ExistsWhere("where ID=" + node.ID))
                    {
                        tstl.UpdateTaskStage(node);
                    }
                    else
                    {
                        int id = tstl.AddTaskStage(node);
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
                    foreach (TaskStage node in value)
                    {
                        nodes.Add(node);
                    }
                }
                RefreshLayout();
            }
        }

        private void AddNode(TaskStage node)
        {
            nodes.Add(node);
            RefreshLayout();
        }

        private void RemoveNode(TaskStage selected)
        {
            if (nodes.Remove(selected))
            {
                RefreshLayout();
            }
        }

        private void UpdateNode(int id, TaskStage node)
        {
            int index = nodes.FindIndex(a => a.ID == id);
            if (index > -1)
            {
                nodes[index] = node;
                RefreshLayout();
            }
        }

        private void panel2_Resize(object sender, EventArgs e)
        {
            RefreshLayout();
        }
    }
}
