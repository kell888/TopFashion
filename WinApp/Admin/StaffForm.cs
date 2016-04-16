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
    public partial class StaffForm : PermissionForm
    {
        public StaffForm(User user, int selectIndex = 0)
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
            if (this.User.ID != this.AdminId && !this.User.GetAllPermissions().ContainsControl("StaffForm", "行政", "tabControl1"))
                this.tabControl1.SelectedIndex = selectIndex;
        }

        private void StaffForm_Load(object sender, EventArgs e)
        {
            base.DisableUserPermission(this);
            LoadStaffs();
            LoadDeparts();
            LoadStaffConditions();
            comboBox2.SelectedIndex = 0;
        }

        private void LoadStaffs()
        {
            List<Staff> elements = StaffLogic.GetInstance().GetAllStaffs();
            comboBox1.Items.Clear();
            foreach (Staff element in elements)
            {
                comboBox1.Items.Add(element);
            }
            dataGridView1.DataSource = StaffLogic.GetInstance().GetStaffs(string.Empty);
        }

        private void LoadStaffConditions()
        {
            List<StaffCondition> elements = StaffConditionLogic.GetInstance().GetAllStaffConditions();
            comboBox6.Items.Clear();
            comboBox7.Items.Clear();
            comboBox7.Items.Add("--不限--");
            foreach (StaffCondition element in elements)
            {
                comboBox6.Items.Add(element);
                comboBox7.Items.Add(element);
            }
            comboBox7.SelectedIndex = 0;
        }

        private void LoadDeparts()
        {
            List<Department> elements = DepartmentLogic.GetInstance().GetAllDepartments();
            comboBox3.Items.Clear();
            comboBox5.Items.Clear();
            comboBox5.Items.Add("--不限--");
            foreach (Department element in elements)
            {
                comboBox3.Items.Add(element);
                comboBox5.Items.Add(element);
            }
            comboBox5.SelectedIndex = 0;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Staff staff = new Staff();
            staff.姓名 = textBox1.Text.Trim();
            staff.性别 = (性别)Enum.ToObject(typeof(性别), comboBox2.SelectedIndex);
            staff.Depart = comboBox3.SelectedItem as Department;
            staff.Condition = comboBox6.SelectedItem as StaffCondition;
            staff.生日 = DateTime.Parse(textBox3.Text.Trim());
            staff.电话 = textBox5.Text.Trim();
            staff.宿舍 = textBox4.Text.Trim();
            staff.备注 = textBox6.Text;
            staff.钥匙数 = (int)numericUpDown1.Value;
            staff.工衣数 = (int)numericUpDown2.Value;
            staff.工牌数 = (int)numericUpDown3.Value;
            staff.是否全部回收 = checkBox1.Checked;
            StaffLogic ml = StaffLogic.GetInstance();
            if (ml.ExistsName(staff.姓名))
            {
                if (MessageBox.Show("系统中已经存在该员工，确定还要继续保存么？", "重名提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.OK)
                {
                    int id = ml.AddStaff(staff);
                    if (id > 0)
                    {
                        staff.ID = id;
                        LoadStaffs();
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
                int id = ml.AddStaff(staff);
                if (id > 0)
                {
                    staff.ID = id;
                    LoadStaffs();
                    MessageBox.Show("添加成功！");
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex > -1)
            {
                Staff staff = (Staff)comboBox1.SelectedItem;
                staff.姓名 = textBox1.Text.Trim();
                staff.性别 = (性别)Enum.ToObject(typeof(性别), comboBox2.SelectedIndex);
                staff.Depart = comboBox3.SelectedItem as Department;
                staff.Condition = comboBox6.SelectedItem as StaffCondition;
                staff.生日 = DateTime.Parse(textBox3.Text.Trim());
                staff.电话 = textBox5.Text.Trim();
                staff.宿舍 = textBox4.Text.Trim();
                staff.备注 = textBox6.Text;
                staff.钥匙数 = (int)numericUpDown1.Value;
                staff.工衣数 = (int)numericUpDown2.Value;
                staff.工牌数 = (int)numericUpDown3.Value;
                staff.是否全部回收 = checkBox1.Checked;
                StaffLogic ml = StaffLogic.GetInstance();
                if (ml.ExistsNameOther(staff.姓名, staff.ID))
                {
                    if (MessageBox.Show("系统中已经存在该员工，确定还要继续保存么？", "重名提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.OK)
                    {
                        if (ml.UpdateStaff(staff))
                        {
                            LoadStaffs();
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
                    if (ml.UpdateStaff(staff))
                    {
                        LoadStaffs();
                        MessageBox.Show("修改成功！");
                    }
                }
            }
            else
            {
                MessageBox.Show("先选定要修改的员工！");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex > -1)
            {
                if (MessageBox.Show("确定要删除该员工？", "删除提醒", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.OK)
                {
                    Staff staff = (Staff)comboBox1.SelectedItem;
                    if (StaffLogic.GetInstance().DeleteStaff(staff))
                    {
                        LoadStaffs();
                    }
                }
            }
            else
            {
                MessageBox.Show("先选定要删除的员工！");
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            DataTable dt = Search(textBox8.Text.Trim(), comboBox4.SelectedIndex, comboBox5.SelectedItem as Department, comboBox7.SelectedItem as StaffCondition, textBox7.Text.Trim());
            dataGridView1.DataSource = dt;
        }

        private DataTable Search(string name, int sex = 0, Department dep = null, StaffCondition condition = null, string mobile = null)
        {
            string nm = "";
            if (!string.IsNullOrEmpty(name))
            {
                nm = " and 姓名 like '%" + name + "%'";
            }
            string sx = "";
            if (sex > 0)
            {
                sx = " and 性别='" + (性别)Enum.ToObject(typeof(性别), (sex - 1)) + "'";
            }
            string dp = "";
            if (dep != null)
            {
                dp = " and 部门='" + dep.Name + "'";
            }
            string cd = "";
            if (condition != null)
            {
                cd = " and 状态='" + condition.状态+ "'";
            }
            string mb = "";
            if (!string.IsNullOrEmpty(mb) && mb.Trim() != "")
            {
                mb = " and 电话 like '%" + mb.Trim() + "%'";
            }
            string where = "(1=1)" + nm + sx + dp + cd + mb;
            return StaffLogic.GetInstance().GetStaffs(where);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            DataTable dt = dataGridView1.DataSource as DataTable;
            if (dt != null)
            {
                dt.TableName = "员工信息";
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
                Staff staff = comboBox1.SelectedItem as Staff;
                if (staff != null)
                {
                    textBox1.Text = staff.姓名;
                    comboBox2.SelectedIndex = (int)staff.性别;
                    comboBox3.SelectedIndex = GetIndexByDepart(staff.Depart, comboBox3);
                    comboBox6.SelectedIndex = GetIndexByCondition(staff.Condition, comboBox6);
                    textBox3.Text = staff.生日.ToString("yyyy-MM-dd");
                    monthCalendar1.SelectionStart = staff.生日;
                    textBox5.Text = staff.电话;
                    textBox4.Text = staff.宿舍;
                    textBox6.Text = staff.备注;
                    numericUpDown1.Value = staff.钥匙数;
                    numericUpDown2.Value = staff.工衣数;
                    numericUpDown3.Value = staff.工牌数;
                    checkBox1.Checked = staff.是否全部回收;
                }
            }
        }

        private int GetIndexByCondition(StaffCondition condition, ComboBox comboBox6)
        {
            if (condition != null && comboBox6 != null)
            {
                for (int i = 0; i < comboBox6.Items.Count; i++)
                {
                    StaffCondition ct = comboBox6.Items[i] as StaffCondition;
                    if (ct != null && ct.ID == condition.ID)
                        return i;
                }
            }
            return -1;
        }

        private int GetIndexByDepart(Department department, ComboBox comboBox3)
        {
            if (department != null && comboBox3 != null)
            {
                for (int i = 0; i < comboBox3.Items.Count; i++)
                {
                    Department ct = comboBox3.Items[i] as Department;
                    if (ct != null && ct.ID == department.ID)
                        return i;
                }
            }
            return -1;
        }

        private void textBox3_MouseClick(object sender, MouseEventArgs e)
        {
            monthCalendar1.Show();
        }

        private void monthCalendar1_DateSelected(object sender, DateRangeEventArgs e)
        {
            textBox3.Text = e.Start.ToString("yyyy-MM-dd");
            monthCalendar1.Hide();
        }

        private void monthCalendar1_Leave(object sender, EventArgs e)
        {
            monthCalendar1.Hide();
        }

        private void tabPage1_MouseClick(object sender, MouseEventArgs e)
        {
            monthCalendar1.Hide();
        }
    }
}
