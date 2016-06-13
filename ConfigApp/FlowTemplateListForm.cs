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
    public partial class FlowTemplateListForm : Form
    {
        public FlowTemplateListForm(ConfigClient owner)
        {
            InitializeComponent();
            this.owner = owner;
        }

        ConfigClient owner;

        private void FlowListForm_Load(object sender, EventArgs e)
        {
            LoadAllTemps();
        }

        private void LoadAllTemps()
        {
            List<FlowTemplate> templates = FlowTemplateLogic.GetInstance().GetAllFlowTemplates();
            LoadFlowTemplates(templates);
        }

        private void LoadFlowTemplates(List<FlowTemplate> templates)
        {
            listBox1.Items.Clear();
            foreach (FlowTemplate temp in templates)
            {
                listBox1.Items.Add(temp);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            List<FlowTemplate> result = Search(textBox1.Text.Trim(), (int)numericUpDown1.Value);
            LoadFlowTemplates(result);
        }

        private List<FlowTemplate> Search(string name, int stageCount)
        {
            return FlowTemplateLogic.GetInstance().GetFlowTemplates(name, stageCount);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex > -1)
            {
                if (MessageBox.Show("确定要删除该流程模板么？", "删除提醒", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.OK)
                {
                    FlowTemplate temp = (FlowTemplate)listBox1.SelectedItem;
                    if (temp != null)
                    {
                        if (FlowTemplateLogic.GetInstance().DeleteFlowTemplate(temp))
                        {
                            LoadAllTemps();
                            MessageBox.Show("删除成功！");
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
                MessageBox.Show("请先选定要删除的流程模板！");
            }
        }

        private void listBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            EditTemplate();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            EditTemplate();
        }

        private void EditTemplate()
        {
            if (listBox1.SelectedIndex > -1)
            {
                FlowTemplate temp = listBox1.SelectedItem as FlowTemplate;
                if (temp != null)
                {
                    FlowTemplateForm ftf = new FlowTemplateForm(this.owner);
                    ftf.Template = temp;
                    if (ftf.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                        LoadAllTemps();
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            FlowTemplateForm ftf = new FlowTemplateForm(this.owner);
            if (ftf.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                LoadAllTemps();
            }
        }
    }
}
