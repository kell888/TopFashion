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
    public partial class DocEditForm : PermissionForm
    {
        public DocEditForm(User user, MainForm owner, FormObject form, DocObject doc = null, int appr = 0, int exeORapp = 0)
        {
            this.User = user;
            InitializeComponent();
            this.owner = owner;
            this.form = form;
            this.doc = doc;
            this.appr = appr;
            this.exeORapp = exeORapp;
            if (appr > 0)
                button1.Text = "审批";
        }

        MainForm owner;
        int width = 4;
        int height = 6;
        FormObject form;
        DocObject doc;
        int appr, exeORapp;

        public DocObject Document
        {
            get
            {
                return GetDocObject();
            }
            set
            {
                doc = value;
                if (value != null)
                {
                    form = value.Form;
                    LoadItems(value.DocItems);
                }
            }
        }

        private void LoadItems(List<FormItem> items)
        {
            if (items != null)
            {
                panel2.SuspendLayout();
                panel2.Controls.Clear();
                for (int i = 0; i < items.Count; i++)
                {
                    FormItem item = items[i];
                    if (doc == null)
                    {
                        SystemType type = Commons.GetSystemType(item.ItemType);
                        switch (type)
                        {
                            case SystemType.附件:
                                break;
                            case SystemType.时间:
                                item.ItemValue = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                                break;
                            case SystemType.数字:
                                item.ItemValue = "0.00";
                                break;
                            case SystemType.字符:
                                item.ItemValue = string.Empty;
                                break;
                            default:
                                break;
                        }
                    }
                    DocEditControl dec = new DocEditControl();
                    dec.Field = item;
                    dec.Location = new Point(width, height + (height + dec.Height) * i);
                    dec.Width = panel2.Width - width * 2;
                    dec.Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right;
                    dec.comboBox1.Enabled = (doc == null);
                    if (!dec.comboBox1.Enabled)
                    {//已经存在的文档，限制编辑
                        bool canEdit = false;
                        bool isAppr;
                        int level;
                        dec.GetExecAppr(out isAppr, out level);
                        TaskInfo task = TaskInfoLogic.GetInstance().GetTaskInfoByEntityId(doc.ID);
                        if (task != null)
                        {
                            TaskStageTemplate stageTemp = task.Flow.Template.Stages[task.Flow.CurrentIndex];
                            TaskStatus currentStatus = task.Flow.Current.Status;
                            if (level == 0)
                            {
                                if (task.Sponsor == this.User.Username && currentStatus == TaskStatus.Initiative)
                                {
                                    canEdit = true;//只有未被接收（即初始化状态）的文档允许文档发起者修改普通字段
                                }
                            }
                            else//level>0是执行字段和审批字段...
                            {
                                if (isAppr)//审批
                                {
                                    if (stageTemp.Approvers.Contains(this.User.ID.ToString()) && currentStatus == TaskStatus.Processed)
                                    {
                                        canEdit = true;
                                    }
                                }
                                else//执行
                                {
                                    if (stageTemp.Executors.Contains(this.User.ID.ToString()) && currentStatus == TaskStatus.Processing)
                                    {
                                        canEdit = true;
                                    }
                                }
                            }
                        }
                        else
                        {
                            canEdit = true;//没启动流程之前的文档可以修改任何字段
                        }
                        dec.CanEdit = canEdit;
                    }
                    panel2.Controls.Add(dec);
                }
                panel2.ResumeLayout(true);
            }
        }

        private DocObject GetDocObject()
        {
            string name = textBox1.Text.Trim();
            if (name == "")
                name = "未命名文档";

            DocObject document = new DocObject();
            document.ID = (doc == null ? 0 : doc.ID);
            document.Name = name;
            document.Form = form;
            document.Owner = this.User;//文档的作者自动更新为最后修改文档的用户
            document.Remark = textBox2.Text;
            FormItemLogic fil = FormItemLogic.GetInstance();
            foreach (Control c in panel2.Controls)
            {
                if (c is DocEditControl)
                {
                    DocEditControl dec = c as DocEditControl;
                    FormItem item = dec.Field;
                    int id = 0;
                    if (doc != null)
                        id = fil.SaveFormItem(item);
                    else
                        id = fil.AddFormItem(item);
                    if (id > 0)
                    {
                        item.ID = id;
                        document.DocItems.Add(item);
                    }
                }
            }
            doc = document;
            return document;
        }

        private void DocEditForm_Load(object sender, EventArgs e)
        {
            if (this.owner != null)
                this.owner.RefreshMsg("正在打开文档中，请稍候...");
            base.DisableUserPermission(this);
            if (doc == null)
            {
                if (form == null)
                    throw new ArgumentNullException("form");

                this.Text = "文档信息 -- 新建";
                button3.Enabled = true;
                LoadItems(form.FormItems);
                if (this.User == null)
                    textBox1.Text = form.FormName + "-" + DateTime.Now.ToString("yyyyMMddHHmmss");
                else
                    textBox1.Text = form.FormName + "-" + this.User.Username;
                textBox1.Focus();
                textBox1.SelectAll();
            }
            else
            {
                if (appr > 0)
                    this.Text = "文档信息 -- 审批";
                else
                    this.Text = "文档信息 -- 修改";
                button3.Enabled = false;
                LoadItems(doc.DocItems);
                textBox1.Text = doc.Name;
                textBox2.Text = doc.Remark;
            }
            if (this.owner != null)
                this.owner.RefreshMsg("Ready...");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DocObjectLogic dol = DocObjectLogic.GetInstance();
            if (doc != null)
            {
                if (dol.UpdateDocObject(Document, this.User))
                {
                    MessageBox.Show("保存文档成功！");
                    if (appr > 0)
                    {
                        TaskInfo task = TaskInfoLogic.GetInstance().GetTaskInfoByEntityId(doc.ID);
                        if (task != null)
                        {
                            bool flag = false;
                            string err = "";
                            if (exeORapp == 0)
                            {
                                if (task.Flow.Execute())
                                {
                                    if (FlowLogic.GetInstance().UpdateFlow(task.Flow))
                                    {
                                        if (TaskStageLogic.GetInstance().SetActualExec(task.Flow.Current.ID, this.User))
                                        {
                                            flag = true;
                                        }
                                        else
                                        {
                                            err = "SetActualExec失败";
                                        }
                                    }
                                    else
                                    {
                                        err = "UpdateFlow失败";
                                    }
                                }
                                else
                                {
                                    err = "Execute失败";
                                }
                            }
                            else if (exeORapp == 1)
                            {
                                if (task.Flow.Approve())
                                {
                                    if (FlowLogic.GetInstance().UpdateFlow(task.Flow))
                                    {
                                        if (TaskStageLogic.GetInstance().SetActualAppr(task.Flow.Current.ID, this.User))
                                        {
                                            flag = true;
                                        }
                                        else
                                        {
                                            err = "SetActualAppr失败";
                                        }
                                    }
                                    else
                                    {
                                        err = "UpdateFlow失败";
                                    }
                                }
                                else
                                {
                                    err = "Approve失败";
                                }
                            }
                            if (flag)
                            {
                                AlertLogic.GetInstance().SetFlag(appr, 1);
                                MessageBox.Show("执行审批成功！");
                                this.DialogResult = System.Windows.Forms.DialogResult.OK;
                            }
                            else
                            {
                                MessageBox.Show("执行审批失败。[exeORapp=" + exeORapp + ", err=" + err + "]");
                            }
                        }
                        else
                        {
                            MessageBox.Show("执行审批失败。");
                        }
                    }
                    else
                    {
                        this.DialogResult = System.Windows.Forms.DialogResult.OK;
                    }
                }
                else
                {
                    MessageBox.Show("保存文档失败或者您没有权限修改别人的文档！");
                }
            }
            else
            {
                int id = dol.AddDocObject(Document);
                if (id > 0)
                {
                    doc.ID = id;
                    MessageBox.Show("新建文档成功！如果需要对该文档做流程，请在右下角点击【设置审批】来生成流程！");
                }
                else
                {
                    MessageBox.Show("新建文档失败！");
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (doc == null)
            {
                MessageBox.Show("请先保存文档后再进行审批设置！");
            }
            List<int> apprs = new List<int>();
            foreach (FormItem item in doc.DocItems)
            {
                if (item.Flag > 0)
                    apprs.Add(item.Flag);
            }
            //判断是否存在执行审批：
            if (apprs.Count > 0)
            {
                apprs.Sort();
                int minApprLevel = apprs[0];
                if (minApprLevel == 1 || minApprLevel == 6)
                {
                    //审批级别是否连贯并从1级开始：
                    bool check = true;
                    for (int i = 0; i < apprs.Count; i++)
                    {
                        if (apprs[i] != i + minApprLevel)
                            check = false;
                    }
                    if (check)
                    {
                        int maxApprLevel = apprs[apprs.Count - 1];
                        //把执行审批最高级别作为参数传递：
                        NewFlowForm nff = new NewFlowForm(this.User, doc, maxApprLevel);
                        if (nff.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                        {
                            this.DialogResult = System.Windows.Forms.DialogResult.OK;
                        }
                    }
                    else
                    {
                        MessageBox.Show("当前的文档中的字段没有满足条件：审批级别要连贯！");
                    }
                }
                else
                {
                    MessageBox.Show("当前的文档中的字段没有满足条件：审批级别要从1级开始！");
                }
            }
            else
            {
                MessageBox.Show("当前的文档中没有需要审批的字段！");
            }
        }
    }
}
