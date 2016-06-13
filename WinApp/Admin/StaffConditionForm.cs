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
    public partial class StaffConditionForm : PermissionForm
    {
        public StaffConditionForm(User user, int selectIndex = 0)
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
            if (this.User.ID != this.AdminId && !this.User.GetAllPermissions().ContainsControl("StaffConditionForm", "行政", "tabControl1"))
                this.tabControl1.SelectedIndex = selectIndex;
        }

        private void StaffConditionForm_Load(object sender, EventArgs e)
        {
            base.DisableUserPermission(this);
            LoadStaffConditions();
            comboBox2.SelectedIndex = 0;
        }

        private void LoadStaffConditions()
        {
            List<StaffCondition> elements = StaffConditionLogic.GetInstance().GetAllStaffConditions();
            comboBox1.Items.Clear();
            foreach (StaffCondition element in elements)
            {
                comboBox1.Items.Add(element);
            }
            dataGridView1.DataSource = StaffConditionLogic.GetInstance().GetStaffConditions(string.Empty);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            StaffCondition staffCondition = new StaffCondition();
            staffCondition.状态 = textBox1.Text.Trim();
            staffCondition.备注 = textBox2.Text.Trim();
            staffCondition.是否在职 = checkBox1.Checked;
            StaffConditionLogic scl = StaffConditionLogic.GetInstance();
            if (scl.ExistsName(staffCondition.状态))
            {
                if (MessageBox.Show("系统中已经存在该员工状态，确定还要继续保存么？", "重名提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.OK)
                {
                    int id = scl.AddStaffCondition(staffCondition);
                    if (id > 0)
                    {
                        staffCondition.ID = id;
                        LoadStaffConditions();
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
                int id = scl.AddStaffCondition(staffCondition);
                if (id > 0)
                {
                    staffCondition.ID = id;
                    LoadStaffConditions();
                    MessageBox.Show("添加成功！");
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex > -1)
            {
                StaffCondition staffCondition = (StaffCondition)comboBox1.SelectedItem;
                staffCondition.状态 = textBox1.Text.Trim();
                staffCondition.备注 = textBox2.Text.Trim();
                staffCondition.是否在职 = checkBox1.Checked;
                StaffConditionLogic scl = StaffConditionLogic.GetInstance();
                if (scl.ExistsNameOther(staffCondition.状态, staffCondition.ID))
                {
                    if (MessageBox.Show("系统中已经存在该员工状态，确定还要继续保存么？", "重名提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.OK)
                    {
                        if (scl.UpdateStaffCondition(staffCondition))
                        {
                            LoadStaffConditions();
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
                    if (scl.UpdateStaffCondition(staffCondition))
                    {
                        LoadStaffConditions();
                        MessageBox.Show("修改成功！");
                    }
                }
            }
            else
            {
                MessageBox.Show("先选定要修改的员工状态！");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex > -1)
            {
                if (MessageBox.Show("确定要删除该员工状态？", "删除提醒", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.OK)
                {
                    StaffCondition staffCondition = (StaffCondition)comboBox1.SelectedItem;
                    if (StaffConditionLogic.GetInstance().DeleteStaffCondition(staffCondition))
                    {
                        LoadStaffConditions();
                    }
                }
            }
            else
            {
                MessageBox.Show("先选定要删除的员工状态！");
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
                nm = " and 状态 like '%" + name + "%'";
            }
            string jy = "";
            if (flag > 0)
            {
                jy = " and 是否在职=" + (flag == 1 ? "1" : "0");
            }
            string where = "(1=1)" + nm + jy;
            return StaffConditionLogic.GetInstance().GetStaffConditions(where);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            DataTable dt = dataGridView1.DataSource as DataTable;
            if (dt != null)
            {
                dt.TableName = "员工状态信息";
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
                StaffCondition staffCondition = comboBox1.SelectedItem as StaffCondition;
                if (staffCondition != null)
                {
                    textBox1.Text = staffCondition.状态;
                    textBox2.Text = staffCondition.备注;
                    checkBox1.Checked = staffCondition.是否在职;
                }
            }
        }
    }
}
