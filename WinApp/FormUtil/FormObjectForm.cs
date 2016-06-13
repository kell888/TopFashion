using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TopFashion
{
    public partial class FormObjectForm : PermissionForm
    {
        int selectIndex;
        KellControls.FloatingCircleLoading loading;
        System.Timers.Timer timer1;
        MainForm owner;
        
        public FormObjectForm(User user, MainForm owner = null, int selectIndex = 0)
        {
            this.User = user;
            this.owner = owner;
            InitializeComponent();
            this.tabControl1.SelectedIndex = selectIndex;
            this.selectIndex = selectIndex;
            this.tabControl1.SelectedIndexChanged += new EventHandler(tabControl1_SelectedIndexChanged);
            loading = new KellControls.FloatingCircleLoading(150);
            timer1 = new System.Timers.Timer(1000);
            timer1.AutoReset = true;
            timer1.Elapsed += new System.Timers.ElapsedEventHandler(timer1_Elapsed);
        }

        void timer1_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            MethodInvoker mi = new MethodInvoker(RefreshLoading);
            mi.BeginInvoke(null, null);
        }

        private void RefreshLoading()
        {
            if (loading.InvokeRequired)
            {
                loading.BeginInvoke(new System.Action(() => loading.Refresh()));
            }
            else
            {
                loading.Refresh();
            }
        }

        void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.User.ID != this.AdminId && !this.User.GetAllPermissions().ContainsControl("FormObjectForm", "系统", "tabControl1"))
                this.tabControl1.SelectedIndex = selectIndex;
        }

        FormObject form;

        private void FormObjectForm_Load(object sender, EventArgs e)
        {
            if (loading == null || loading.IsDisposed)
                loading = new KellControls.FloatingCircleLoading(150);
            loading.Show();
            loading.BringToFront();
            loading.Focus();
            loading.Refresh();
            timer1.Start();
            backgroundWorker1.RunWorkerAsync();
            timer1.Stop();
            loading.Hide();
        }

        private void LoadAll(BackgroundWorker bw)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new System.Action<BackgroundWorker>(LoadAll), bw);
            }
            else
            {
                LoadFormObjects();
                bw.ReportProgress(50);
                LoadFormTypes();
                bw.ReportProgress(100);
            }
        }

        private void LoadFormObjects()
        {
            List<FormObject> elements = FormObjectLogic.GetInstance().GetAllFormObjects();
            comboBox1.Items.Clear();
            foreach (FormObject element in elements)
            {
                comboBox1.Items.Add(element);
            }
            dataGridView1.DataSource = FormObjectLogic.GetInstance().GetFormObjects(string.Empty);
        }

        private void LoadFormTypes()
        {
            List<FormType> elements = FormTypeLogic.GetInstance().GetAllFormTypes();
            comboBox2.Items.Clear();
            comboBox2.Items.Add("--不限--");
            foreach (FormType element in elements)
            {
                comboBox2.Items.Add(element);
            }
            comboBox2.SelectedIndex = 0;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (form != null)
            {
                FormObjectLogic al = FormObjectLogic.GetInstance();
                if (al.ExistsName(form.FormName))
                {
                    if (MessageBox.Show("系统中已经存在该表单，确定还要继续保存么？", "重名提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.OK)
                    {
                        int id = al.AddFormObject(form);
                        if (id > 0)
                        {
                            form.ID = id;
                            LoadFormObjects();
                            MessageBox.Show("添加成功！");
                        }
                    }
                }
                else
                {
                    int id = al.AddFormObject(form);
                    if (id > 0)
                    {
                        form.ID = id;
                        LoadFormObjects();
                        MessageBox.Show("添加成功！");
                    }
                }
            }
            else
            {
                MessageBox.Show("请先编辑好表单！");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (form != null)
            {
                FormObjectLogic al = FormObjectLogic.GetInstance();
                if (al.ExistsNameOther(form.FormName, form.ID))
                {
                    if (MessageBox.Show("系统中已经存在该表单，确定还要继续保存么？", "重名提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.OK)
                    {
                        if (al.UpdateFormObject(form, this.User))
                        {
                            LoadFormObjects();
                            MessageBox.Show("修改成功！");
                        }
                        else
                        {
                            MessageBox.Show("修改失败或者您没有权限修改别人的表单！");
                        }
                    }
                }
                else
                {
                    if (al.UpdateFormObject(form, this.User))
                    {
                        LoadFormObjects();
                        MessageBox.Show("修改成功！");
                    }
                    else
                    {
                        MessageBox.Show("修改失败或者您没有权限修改别人的表单！");
                    }
                }
            }
            else
            {
                MessageBox.Show("所选择的表单为空！");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex > -1)
            {
                if (MessageBox.Show("确定要删除该表单？", "删除表单", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.OK)
                {
                    FormObject formObject = (FormObject)comboBox1.SelectedItem;
                    if (FormObjectLogic.GetInstance().DeleteFormObject(formObject, this.User))
                    {
                        MessageBox.Show("删除表单成功！");
                        LoadFormObjects();
                    }
                    else
                    {
                        MessageBox.Show("删除失败或者您没有权限删除别人的表单！");
                    }
                }
            }
            else
            {
                MessageBox.Show("先选定要删除的表单！");
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            DataTable dt = Search(textBox8.Text.Trim(), comboBox2.SelectedItem as FormType);
            dataGridView1.DataSource = dt;
        }

        private DataTable Search(string name = null, FormType formType = null)
        {
            string nm = "";
            if (!string.IsNullOrEmpty(name) && name.Trim() != "")
            {
                nm = " and FormName like '%" + name + "%'";
            }
            string ft = "";
            if (formType != null)
            {
                ft = " and FormType=" + formType.ID;
            }
            string where = "(1=1)" + nm + ft;
            return FormObjectLogic.GetInstance().GetFormObjects(where);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            DataTable dt = dataGridView1.DataSource as DataTable;
            if (dt != null)
            {
                dt.TableName = "表单信息";
                KellPrinter.DataReporter printer = new KellPrinter.DataReporter(dt);
                PrintForm pf = new PrintForm(this.User, printer);
                pf.Show();
            }
            else
            {
                MessageBox.Show("当前没有数据可供打印导出！");
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex > -1)
            {
                form = comboBox1.SelectedItem as FormObject;
                if (form != null)
                {
                    button6.Text = form.FormInfo;
                }
            }
        }

        private int GetIndexByFormObject(FormType formType, ComboBox comboBox3)
        {
            if (formType != null && comboBox3 != null)
            {
                for (int i = 0; i < comboBox3.Items.Count; i++)
                {
                    FormType at = comboBox3.Items[i] as FormType;
                    if (at != null && formType.ID == at.ID)
                        return i;
                }
            }
            return -1;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            FormEditForm fef = new FormEditForm(this.User);
            fef.Form = form;
            if (fef.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                form = fef.Form;
                if (form != null)
                    button6.Text = form.FormInfo;
            }
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker bw = sender as BackgroundWorker;
            LoadAll(bw);
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (owner != null)
                owner.RefreshMsg("加载中..." + e.ProgressPercentage + "%");
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                MessageBox.Show("载入出错！");
            }
            else if (e.Cancelled)
            {
                MessageBox.Show("终止载入！");
            }
            else
            {
                //MessageBox.Show("载入完成！");
            }
        }
    }
}
