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
    public partial class WorklogForm : PermissionForm
    {
        public WorklogForm(User user, int selectIndex = 0)
        {
            this.User = user;
            InitializeComponent();
            this.selectStaffControl1.SelectOnlyOne = true;
            this.tabControl1.SelectedIndex = selectIndex;
            this.selectIndex = selectIndex;
            this.tabControl1.SelectedIndexChanged += new EventHandler(tabControl1_SelectedIndexChanged);
        }
        int selectIndex;

        void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.User.ID != this.AdminId && !this.User.GetAllPermissions().ContainsControl("WorklogForm", "销售", "tabControl1"))
                this.tabControl1.SelectedIndex = selectIndex;
        }

        private void WorklogForm_Load(object sender, EventArgs e)
        {
            base.DisableUserPermission(this);
            LoadWorklogs();
            comboBox4.SelectedIndex = 0;
        }

        private void LoadWorklogs()
        {
            List<Worklog> elements = WorklogLogic.GetInstance().GetAllWorklogs();
            comboBox1.Items.Clear();
            foreach (Worklog element in elements)
            {
                comboBox1.Items.Add(element);
            }
            dataGridView1.DataSource = WorklogLogic.GetInstance().GetWorklogs(string.Empty);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Worklog worklog = new Worklog();
            worklog.销售 = (selectStaffControl1.SelectedStaffs != null && selectStaffControl1.SelectedStaffs.Count > 0) ? selectStaffControl1.SelectedStaffs[0] : null;
            worklog.日期 = DateTime.Parse(textBox3.Text.Trim());
            worklog.客户 = textBox1.Text.Trim();
            worklog.电话 = textBox2.Text.Trim();
            worklog.是否自访 = checkBox1.Checked;
            worklog.是否老会员 = checkBox2.Checked;
            worklog.是否电话拜访 = checkBox3.Checked;
            worklog.性别 = (性别)Enum.ToObject(typeof(性别), comboBox4.SelectedIndex);
            worklog.意向 = textBox4.Text.Trim();
            worklog.住址 = textBox5.Text.Trim();
            worklog.备注 = textBox6.Text;
            WorklogLogic rl = WorklogLogic.GetInstance();
            int id = rl.AddWorklog(worklog);
            if (id > 0)
            {
                worklog.ID = id;
                LoadWorklogs();
                MessageBox.Show("添加成功！");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex > -1)
            {
                Worklog worklog = (Worklog)comboBox1.SelectedItem;
                worklog.销售 = (selectStaffControl1.SelectedStaffs != null && selectStaffControl1.SelectedStaffs.Count > 0) ? selectStaffControl1.SelectedStaffs[0] : null;
                worklog.日期 = DateTime.Parse(textBox3.Text.Trim());
                worklog.客户 = textBox1.Text.Trim();
                worklog.电话 = textBox2.Text.Trim();
                worklog.是否自访 = checkBox1.Checked;
                worklog.是否老会员 = checkBox2.Checked;
                worklog.是否电话拜访 = checkBox3.Checked;
                worklog.性别 = (性别)Enum.ToObject(typeof(性别), comboBox4.SelectedIndex);
                worklog.意向 = textBox4.Text.Trim();
                worklog.住址 = textBox5.Text.Trim();
                worklog.备注 = textBox6.Text;
                WorklogLogic rl = WorklogLogic.GetInstance();
                if (rl.UpdateWorklog(worklog))
                {
                    LoadWorklogs();
                    MessageBox.Show("修改成功！");
                }
            }
            else
            {
                MessageBox.Show("先选定要修改的工作日报！");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex > -1)
            {
                if (MessageBox.Show("确定要删除该工作日报？", "删除提醒", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.OK)
                {
                    Worklog worklog = (Worklog)comboBox1.SelectedItem;
                    if (WorklogLogic.GetInstance().DeleteWorklog(worklog))
                    {
                        LoadWorklogs();
                    }
                }
            }
            else
            {
                MessageBox.Show("先选定要删除的工作日报！");
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            DataTable dt = Search((selectStaffControl2.SelectedStaffs != null && selectStaffControl2.SelectedStaffs.Count > 0) ? selectStaffControl2.SelectedStaffs[0] : null, textBox7.Text.Trim(), textBox9.Text.Trim());
            dataGridView1.DataSource = dt;
        }

        private DataTable Search(Staff staff, string start, string end)
        {
            string nm = "";
            if (staff != null)
            {
                nm = " and 销售='" + staff.姓名 + "'";
            }
            string where = "(1=1)" + nm + " and 日期 between '" + start + "' and '" + end + "'";
            return WorklogLogic.GetInstance().GetWorklogs(where);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            DataTable dt = dataGridView1.DataSource as DataTable;
            if (dt != null)
            {
                dt.TableName = "工作日报";
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
                Worklog worklog = comboBox1.SelectedItem as Worklog;
                if (worklog != null)
                {
                    selectStaffControl1.SelectedStaffs = new List<Staff>(){ worklog.销售 };
                    textBox1.Text = worklog.客户;
                    textBox2.Text = worklog.电话;
                    monthCalendar1.SelectionStart = worklog.日期;
                    textBox3.Text = worklog.日期.ToString("yyyy-MM-dd");
                    checkBox1.Checked = worklog.是否自访;
                    checkBox2.Checked = worklog.是否老会员;
                    checkBox3.Checked = worklog.是否电话拜访;
                    comboBox4.SelectedIndex = (int)worklog.性别;
                    textBox4.Text = worklog.意向;
                    textBox5.Text = worklog.住址;
                    textBox6.Text = worklog.备注;
                }
            }
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

        private void textBox7_Click(object sender, EventArgs e)
        {
            monthCalendar2.Show();
        }

        private void textBox9_Click(object sender, EventArgs e)
        {
            monthCalendar3.Show();
        }

        private void monthCalendar2_DateSelected(object sender, DateRangeEventArgs e)
        {
            textBox7.Text = e.Start.ToString("yyyy-MM-dd");
        }

        private void monthCalendar3_DateSelected(object sender, DateRangeEventArgs e)
        {
            textBox9.Text = e.Start.ToString("yyyy-MM-dd");
        }

        private void monthCalendar2_Leave(object sender, EventArgs e)
        {
            monthCalendar2.Hide();
        }

        private void monthCalendar3_Leave(object sender, EventArgs e)
        {
            monthCalendar3.Hide();
        }

        private void monthCalendar1_Leave(object sender, EventArgs e)
        {
            monthCalendar1.Hide();
        }

        private void tabPage1_MouseClick(object sender, MouseEventArgs e)
        {
            monthCalendar1.Hide();
        }

        private void tabPage2_MouseClick(object sender, MouseEventArgs e)
        {
            monthCalendar2.Hide();
            monthCalendar3.Hide();
        }

        private void dataGridView1_MouseClick(object sender, MouseEventArgs e)
        {
            monthCalendar2.Hide();
            monthCalendar3.Hide();
        }
    }
}
