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
    public partial class FollowupResultForm : PermissionForm
    {
        public FollowupResultForm(User user, int selectIndex = 0)
        {
            this.User = user;
            InitializeComponent();
            this.tabControl1.SelectedIndex = selectIndex;
            this.selectIndex = selectIndex;
            this.tabControl1.SelectedIndexChanged += new EventHandler(tabControl1_SelectedIndexChanged);
        }
        int selectIndex;

        void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.User.ID != this.AdminId && !this.User.GetAllPermissions().ContainsControl("FollowupResultForm", "系统", "tabControl1"))
                this.tabControl1.SelectedIndex = selectIndex;
        }

        private void FollowupResultForm_Load(object sender, EventArgs e)
        {
            base.DisableUserPermission(this);
            LoadFollowupResults();
            comboBox2.SelectedIndex = 0;
        }

        private void LoadFollowupResults()
        {
            List<FollowupResult> elements = FollowupResultLogic.GetInstance().GetAllFollowupResults();
            comboBox1.Items.Clear();
            foreach (FollowupResult element in elements)
            {
                comboBox1.Items.Add(element);
            }
            dataGridView1.DataSource = FollowupResultLogic.GetInstance().GetFollowupResults(string.Empty);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FollowupResult followupResult = new FollowupResult();
            followupResult.结果 = textBox1.Text.Trim();
            followupResult.备注 = textBox2.Text.Trim();
            followupResult.Flag = checkBox1.Checked;
            FollowupResultLogic al = FollowupResultLogic.GetInstance();
            if (al.ExistsName(followupResult.结果))
            {
                if (MessageBox.Show("系统中已经存在该跟进结果，确定还要继续保存么？", "重名提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.OK)
                {
                    int id = al.AddFollowupResult(followupResult);
                    if (id > 0)
                    {
                        followupResult.ID = id;
                        LoadFollowupResults();
                        MessageBox.Show("添加成功！");
                    }
                }
                else
                {
                    textBox1.Focus();
                    textBox1.SelectAll();
                }
            }
            else
            {
                int id = al.AddFollowupResult(followupResult);
                if (id > 0)
                {
                    followupResult.ID = id;
                    LoadFollowupResults();
                    MessageBox.Show("添加成功！");
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex > -1)
            {
                FollowupResult followupResult = (FollowupResult)comboBox1.SelectedItem;
                followupResult.结果 = textBox1.Text.Trim();
                followupResult.备注 = textBox2.Text.Trim();
                followupResult.Flag = checkBox1.Checked;
                FollowupResultLogic al = FollowupResultLogic.GetInstance();
                if (al.ExistsNameOther(followupResult.结果, followupResult.ID))
                {
                    if (MessageBox.Show("系统中已经存在该跟进结果，确定还要继续保存么？", "重名提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.OK)
                    {
                        if (al.UpdateFollowupResult(followupResult))
                        {
                            LoadFollowupResults();
                            MessageBox.Show("修改成功！");
                        }
                    }
                    else
                    {
                        textBox1.Focus();
                        textBox1.SelectAll();
                    }
                }
                else
                {
                    if (al.UpdateFollowupResult(followupResult))
                    {
                        LoadFollowupResults();
                        MessageBox.Show("修改成功！");
                    }
                }
            }
            else
            {
                MessageBox.Show("先选定要修改的跟进结果！");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex > -1)
            {
                if (MessageBox.Show("确定要删除该跟进结果？", "删除提醒", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.OK)
                {
                    FollowupResult followupResult = (FollowupResult)comboBox1.SelectedItem;
                    if (FollowupResultLogic.GetInstance().DeleteFollowupResult(followupResult))
                    {
                        LoadFollowupResults();
                    }
                }
            }
            else
            {
                MessageBox.Show("先选定要删除的跟进结果！");
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            DataTable dt = Search(textBox8.Text.Trim(), comboBox2.SelectedIndex);
            dataGridView1.DataSource = dt;
        }

        private DataTable Search(string name = null, int flag = 0)
        {
            string nm = "";
            if (!string.IsNullOrEmpty(name) && name.Trim() != "")
            {
                nm = " and 结果 like '%" + name + "%'";
            }
            string jy = "";
            if (flag > 0)
            {
                jy = " and Flag=" + flag;
            }
            string where = "(1=1)" + nm + jy;
            return FollowupResultLogic.GetInstance().GetFollowupResults(where);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            DataTable dt = dataGridView1.DataSource as DataTable;
            if (dt != null)
            {
                dt.TableName = "跟进结果信息";
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
                FollowupResult followupResult = comboBox1.SelectedItem as FollowupResult;
                if (followupResult != null)
                {
                    textBox1.Text = followupResult.结果;
                    textBox2.Text = followupResult.备注;
                    checkBox1.Checked = followupResult.Flag;
                }
            }
        }
    }
}
