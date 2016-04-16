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
    public partial class NewFlowForm : PermissionForm
    {
        public NewFlowForm(User user, DocObject doc, int maxApprLevel)
        {
            if (doc == null)
                throw new ArgumentNullException("doc");

            this.User = user;
            InitializeComponent();
            this.doc = doc;
            this.maxApprLevel = maxApprLevel;
        }

        DocObject doc;
        int maxApprLevel;

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Next();
        }

        private void Next()
        {
            if (listBox1.SelectedIndex > -1)
            {
                FlowTemplate temp = listBox1.SelectedItem as FlowTemplate;
                if (temp != null)
                {
                    List<TaskStage> stages = new List<TaskStage>();
                    foreach (TaskStageTemplate stage in temp.Stages)
                    {
                        TaskStage s = new TaskStage(0, stage.Name, stage, TaskStatus.Initiative, "", "", DateTime.MinValue, DateTime.MinValue, "");
                        int i = TaskStageLogic.GetInstance().AddTaskStage(s);
                        if (i > 0)
                        {
                            s.ID = i;
                            stages.Add(s);
                        }
                    }
                    string docName = doc.Name;
                    Flow flow = new Flow(0, docName + "(" + temp.Name + ")", temp, -1, "", stages);
                    int r = FlowLogic.GetInstance().AddFlow(flow);
                    if (r > 0)
                    {
                        flow.ID = r;
                        if (FlowLogic.GetInstance().StartFlow(r))
                        {
                            flow.StartFlow();
                            TaskInfo task = new TaskInfo(0, doc.ID, flow, this.User.Username, "创建时间：" + DateTime.Now.ToString());
                            if (TaskInfoLogic.GetInstance().AddTaskInfo(task) > 0)
                            {
                                MessageBox.Show("建立流程任务成功！");
                                this.DialogResult = System.Windows.Forms.DialogResult.OK;
                            }
                            else
                            {
                                MessageBox.Show("建立流程任务失败！");
                                this.DialogResult = System.Windows.Forms.DialogResult.Ignore;
                            }
                        }
                        else
                        {
                            MessageBox.Show("启动流程失败！");
                            this.DialogResult = System.Windows.Forms.DialogResult.Ignore;
                        }
                    }
                    else
                    {
                        MessageBox.Show("建立流程到数据库失败！");
                        this.DialogResult = System.Windows.Forms.DialogResult.Ignore;
                    }
                    //this.Close();
                }
                else
                {
                    MessageBox.Show("您选择的流程模板为空！");
                }
            }
            else
            {
                MessageBox.Show("请先选择一个流程模板！");
            }
        }

        private void NewFlowForm_Load(object sender, EventArgs e)
        {
            if (doc.ID == 0)
            {
                MessageBox.Show("文档为空，或者尚未保存成功！");
                this.Close();
            }
            int stageCount = 0;
            if (maxApprLevel > 5)
                stageCount = maxApprLevel - 5;
            if (stageCount > 0)
                LoadFormObjects(stageCount);
        }

        private void LoadFormObjects(int stageCount)
        {
            List<FlowTemplate> temps = FlowTemplateLogic.GetInstance().GetFlowTemplates(null, stageCount);
            listBox1.Items.Clear();
            foreach (FlowTemplate temp in temps)
            {
                listBox1.Items.Add(temp);
            }
        }

        private void listBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Next();
        }
    }
}
