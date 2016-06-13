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
    public partial class AlertMyForm : PermissionForm
    {
        Alert alert;
        MainForm owner;
        static AlertMyForm instance;
        private AlertMyForm(User user, MainForm owner, Alert alert)
        {
            this.User = user;
            InitializeComponent();
            this.owner = owner;
            this.alert = alert;
            this.Text = alert.提醒方式.ToString();
            this.label1.Text = alert.提醒项目;
            if (alert != null)
            {
                switch (alert.提醒方式)
                {
                    case 提醒方式.系统提示:
                        button1.Text = "已阅";
                        break;
                    case 提醒方式.执行流程:
                        button1.Text = "去执行";
                        break;
                    case 提醒方式.审批流程:
                        button1.Text = "去审批";
                        break;
                    default:
                        break;
                }
            }
        }

        public static AlertMyForm Instance(User user, MainForm owner, Alert alert)
        {
            if (instance == null)
            {
                instance = new AlertMyForm(user, owner, alert);
            }
            else
            {
                instance.User = user;
                instance.alert = alert;
                instance.Text = alert.提醒方式.ToString();
                instance.label1.Text = alert.提醒项目;
                if (alert != null)
                {
                    switch (alert.提醒方式)
                    {
                        case 提醒方式.系统提示:
                            instance.button1.Text = "已阅";
                            break;
                        case 提醒方式.执行流程:
                            instance.button1.Text = "去执行";
                            break;
                        case 提醒方式.审批流程:
                            instance.button1.Text = "去审批";
                            break;
                        default:
                            break;
                    }
                }
            }
            instance.BringToFront();
            return instance;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (alert != null)
            {
                switch (alert.提醒方式)
                {
                    case 提醒方式.系统提示:
                        bool f = AlertLogic.GetInstance().SetFlag(alert.ID, 1);
                        if (!f)
                        {
                            MessageBox.Show("已阅置位失败！");
                        }
                        else
                        {
                            this.Close();
                        }
                        break;
                    case 提醒方式.执行流程:
                        DocObject doc = DocObjectLogic.GetInstance().GetDocObject(Convert.ToInt32(alert.备注));
                        if (doc != null)
                        {
                            TaskInfo task = TaskInfoLogic.GetInstance().GetTaskInfoByEntityId(doc.ID);
                            if (task != null)
                            {
                                TaskStageLogic.GetInstance().SetReceiveToExec(task.Flow.Current.ID);
                            }
                            DocEditForm def = new DocEditForm(this.User, this.owner, doc.Form, doc, alert.ID);
                            if (def.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                                this.Close();
                        }
                        break;
                    case 提醒方式.审批流程:
                        DocObject doc2 = DocObjectLogic.GetInstance().GetDocObject(Convert.ToInt32(alert.备注));
                        if (doc2 != null)
                        {
                            TaskInfo task = TaskInfoLogic.GetInstance().GetTaskInfoByEntityId(doc2.ID);
                            if (task != null)
                            {
                                TaskStageLogic.GetInstance().SetReceiveToExec(task.Flow.Current.ID);
                            }
                            DocEditForm def = new DocEditForm(this.User, this.owner, doc2.Form, doc2, alert.ID);
                            if (def.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                                this.Close();
                        }
                        break;
                    default:
                        break;
                }
            }
        }

        private void AlertMyForm_Load(object sender, EventArgs e)
        {
            base.DisableUserPermission(this);
            Rectangle rect = Screen.PrimaryScreen.WorkingArea;
            this.Location = new Point(rect.X + rect.Width - this.Width, rect.Y + rect.Height - this.Height);
        }

        private void AlertMyForm_FormClosing(object sender, FormClosingEventArgs e)
        {
        }
    }
}
