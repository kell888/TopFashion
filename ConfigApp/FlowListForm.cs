using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using KellWorkFlow;
using TopFashion;

namespace TopFashion
{
    public partial class FlowListForm : Form
    {
        public FlowListForm(ConfigClient owner)
        {
            InitializeComponent();
            this.owner = owner;
        }

        ConfigClient owner;

        private void FlowListForm_Load(object sender, EventArgs e)
        {
            List<Flow> flows = FlowLogic.GetInstance().GetAllFlows();
            LoadFlows(flows);
            LoadFlowStatuses();
        }

        private void LoadFlows(List<Flow> flows)
        {
            listBox1.Items.Clear();
            foreach (Flow flow in flows)
            {
                listBox1.Items.Add(flow);
            }
        }

        private void LoadFlowStatuses()
        {
            domainUpDown1.Items.Clear();
            domainUpDown1.Items.Add("--不限--");
            domainUpDown1.Items.Add("未完成");
            domainUpDown1.Items.Add("已完成");
            domainUpDown1.SelectedIndex = 0;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;
            try
            {
                int C = domainUpDown1.SelectedIndex;
                List<Flow> result = Search(textBox1.Text.Trim(), C);
                LoadFlows(result);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                button1.Enabled = true;
            }
        }

        private List<Flow> Search(string name, int status)
        {
            return FlowLogic.GetInstance().GetFlows(name, status);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex > -1)
            {
                MessageBox.Show("警告：删除流程后将会影响到该流程模板中涉及到的所有执行者和审批者的工作！");
                if (MessageBox.Show("确定要删除该流程么？", "删除提醒", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.OK)
                {
                    Flow flow = (Flow)listBox1.SelectedItem;
                    if (flow != null)
                    {
                        if (FlowLogic.GetInstance().DeleteFlow(flow))
                        {
                            MessageBox.Show("删除成功！");
                            int C = domainUpDown1.SelectedIndex;
                            List<Flow> result = Search(textBox1.Text.Trim(), C);
                            LoadFlows(result);
                        }
                        else
                        {
                            MessageBox.Show("删除失败！");
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("请先选定要删除的流程！");
            }
        }

        private void listBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (listBox1.SelectedIndex > -1)
            {
                Flow flow = listBox1.SelectedItem as Flow;
                if (flow != null)
                {
                    FlowForm ff = new FlowForm(this.owner);
                    ff.ShowDialog();
                }
            }
        }
    }
}
