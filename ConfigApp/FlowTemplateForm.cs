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
    public partial class FlowTemplateForm : Form
    {
        public FlowTemplateForm(ConfigClient owner)
        {
            InitializeComponent();
            this.flowTemplateControl1.TheOwner = owner;
        }

        int templateId;

        public FlowTemplate Template
        {
            get
            {
                return new FlowTemplate(templateId, textBox1.Text.Trim(), flowTemplateControl1.Nodes);
            }
            set
            {
                if (value != null)
                {
                    templateId = value.ID;
                    textBox1.Text = value.Name;
                    flowTemplateControl1.Nodes = value.Stages;
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FlowTemplate template = new FlowTemplate(0, textBox1.Text.Trim(), flowTemplateControl1.Nodes);
            int i = FlowTemplateLogic.GetInstance().AddFlowTemplate(template);
            if (i > 0)
            {
                MessageBox.Show("添加流程模板成功！");
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (templateId > 0)
            {
                FlowTemplate template = new FlowTemplate(templateId, textBox1.Text.Trim(), flowTemplateControl1.Nodes);
                if (FlowTemplateLogic.GetInstance().UpdateFlowTemplate(template))
                {
                    MessageBox.Show("修改流程模板成功！");
                    this.DialogResult = System.Windows.Forms.DialogResult.OK;
                }
            }
            else
            {
                MessageBox.Show("请先指定一个要修改的流程模板！或者当前是添加新的流程模板，请点击【添加】按钮。");
            }
        }
    }
}
