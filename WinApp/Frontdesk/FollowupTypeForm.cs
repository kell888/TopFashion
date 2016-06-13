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
    public partial class FollowupTypeForm : PermissionForm
    {
        public FollowupTypeForm(User user, int selectIndex = 0)
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
            if (this.User.ID != this.AdminId && !this.User.GetAllPermissions().ContainsControl("FollowupTypeForm", "系统", "tabControl1"))
                this.tabControl1.SelectedIndex = selectIndex;
        }

        private void FollowupTypeForm_Load(object sender, EventArgs e)
        {
            base.DisableUserPermission(this);
            LoadFollowupTypes();
            comboBox2.SelectedIndex = 0;
        }

        private void LoadFollowupTypes()
        {
            List<FollowupType> elements = FollowupTypeLogic.GetInstance().GetAllFollowupTypes();
            comboBox1.Items.Clear();
            foreach (FollowupType element in elements)
            {
                comboBox1.Items.Add(element);
            }
            dataGridView1.DataSource = FollowupTypeLogic.GetInstance().GetFollowupTypes(string.Empty);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FollowupType followupType = new FollowupType();
            followupType.方式 = textBox1.Text.Trim();
            followupType.备注 = textBox2.Text.Trim();
            followupType.Flag = checkBox1.Checked;
            FollowupTypeLogic al = FollowupTypeLogic.GetInstance();
            if (al.ExistsName(followupType.方式))
            {
                if (MessageBox.Show("系统中已经存在该回访方式，确定还要继续保存么？", "重名提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.OK)
                {
                    int id = al.AddFollowupType(followupType);
                    if (id > 0)
                    {
                        followupType.ID = id;
                        LoadFollowupTypes();
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
                int id = al.AddFollowupType(followupType);
                if (id > 0)
                {
                    followupType.ID = id;
                    LoadFollowupTypes();
                    MessageBox.Show("添加成功！");
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex > -1)
            {
                FollowupType followupType = (FollowupType)comboBox1.SelectedItem;
                followupType.方式 = textBox1.Text.Trim();
                followupType.备注 = textBox2.Text.Trim();
                followupType.Flag = checkBox1.Checked;
                FollowupTypeLogic al = FollowupTypeLogic.GetInstance();
                if (al.ExistsNameOther(followupType.方式, followupType.ID))
                {
                    if (MessageBox.Show("系统中已经存在该回访方式，确定还要继续保存么？", "重名提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.OK)
                    {
                        if (al.UpdateFollowupType(followupType))
                        {
                            LoadFollowupTypes();
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
                    if (al.UpdateFollowupType(followupType))
                    {
                        LoadFollowupTypes();
                        MessageBox.Show("修改成功！");
                    }
                }
            }
            else
            {
                MessageBox.Show("先选定要修改的回访方式！");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex > -1)
            {
                if (MessageBox.Show("确定要删除该回访方式？", "删除提醒", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.OK)
                {
                    FollowupType followupType = (FollowupType)comboBox1.SelectedItem;
                    if (FollowupTypeLogic.GetInstance().DeleteFollowupType(followupType))
                    {
                        LoadFollowupTypes();
                    }
                }
            }
            else
            {
                MessageBox.Show("先选定要删除的回访方式！");
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
                nm = " and 方式 like '%" + name + "%'";
            }
            string jy = "";
            if (flag > 0)
            {
                jy = " and Flag=" + flag;
            }
            string where = "(1=1)" + nm + jy;
            return FollowupTypeLogic.GetInstance().GetFollowupTypes(where);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            DataTable dt = dataGridView1.DataSource as DataTable;
            if (dt != null)
            {
                dt.TableName = "回访方式信息";
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
                FollowupType followupType = comboBox1.SelectedItem as FollowupType;
                if (followupType != null)
                {
                    textBox1.Text = followupType.方式;
                    textBox2.Text = followupType.备注;
                    checkBox1.Checked = followupType.Flag;
                }
            }
        }
    }
}
