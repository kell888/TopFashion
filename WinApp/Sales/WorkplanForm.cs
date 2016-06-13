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
    public partial class WorkplanForm : PermissionForm
    {
        public WorkplanForm(User user, int selectIndex = 0)
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
            if (this.User.ID != this.AdminId && !this.User.GetAllPermissions().ContainsControl("WorkplanForm", "销售", "tabControl1"))
                this.tabControl1.SelectedIndex = selectIndex;
        }

        private void WorkplanForm_Load(object sender, EventArgs e)
        {
            base.DisableUserPermission(this);
            LoadWorkplans();
        }

        private void LoadWorkplans()
        {
            List<Workplan> elements = WorkplanLogic.GetInstance().GetAllWorkplans();
            comboBox1.Items.Clear();
            foreach (Workplan element in elements)
            {
                comboBox1.Items.Add(element);
            }
            dataGridView1.DataSource = WorkplanLogic.GetInstance().GetWorkplans(string.Empty);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Workplan workplan = new Workplan();
            workplan.销售 = (selectStaffControl1.SelectedStaffs != null && selectStaffControl1.SelectedStaffs.Count > 0) ? selectStaffControl1.SelectedStaffs[0] : null;
            workplan.日期 = DateTime.Parse(textBox3.Text.Trim());
            workplan.带人数 = (int)numericUpDown1.Value;
            workplan.号码数 = (int)numericUpDown2.Value;
            workplan.成单数 = (int)numericUpDown3.Value;
            workplan.回访数 = (int)numericUpDown4.Value;
            workplan.备注 = textBox6.Text;
            WorkplanLogic rl = WorkplanLogic.GetInstance();
            int id = rl.AddWorkplan(workplan);
            if (id > 0)
            {
                workplan.ID = id;
                LoadWorkplans();
                MessageBox.Show("添加成功！");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex > -1)
            {
                Workplan workplan = (Workplan)comboBox1.SelectedItem;
                workplan.销售 = (selectStaffControl1.SelectedStaffs != null && selectStaffControl1.SelectedStaffs.Count > 0) ? selectStaffControl1.SelectedStaffs[0] : null;
                workplan.日期 = DateTime.Parse(textBox3.Text.Trim());
                workplan.带人数 = (int)numericUpDown1.Value;
                workplan.号码数 = (int)numericUpDown2.Value;
                workplan.成单数 = (int)numericUpDown3.Value;
                workplan.回访数 = (int)numericUpDown4.Value;
                workplan.备注 = textBox6.Text;
                WorkplanLogic rl = WorkplanLogic.GetInstance();
                if (rl.UpdateWorkplan(workplan))
                {
                    LoadWorkplans();
                    MessageBox.Show("修改成功！");
                }
            }
            else
            {
                MessageBox.Show("先选定要修改的工作计划！");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex > -1)
            {
                if (MessageBox.Show("确定要删除该工作计划？", "删除提醒", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.OK)
                {
                    Workplan workplan = (Workplan)comboBox1.SelectedItem;
                    if (WorkplanLogic.GetInstance().DeleteWorkplan(workplan))
                    {
                        LoadWorkplans();
                    }
                }
            }
            else
            {
                MessageBox.Show("先选定要删除的工作计划！");
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            DataTable dt = Search((selectStaffControl2.SelectedStaffs != null && selectStaffControl2.SelectedStaffs.Count > 0) ? selectStaffControl2.SelectedStaffs[0] : null, DateTime.Parse(textBox1.Text.Trim()), DateTime.Parse(textBox2.Text.Trim()));
            dataGridView1.DataSource = dt;
        }

        private DataTable Search(Staff staff, DateTime start, DateTime end)
        {
            string nm = "";
            if (staff != null)
            {
                nm = " and 销售='" + staff.姓名 + "'";
            }
            string where = "(1=1)" + nm + " and (日期 >= '" + start + "' and 日期 < '" + end.AddDays(1) + "')";
            return WorkplanLogic.GetInstance().GetWorkplans(where);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            DataTable dt = dataGridView1.DataSource as DataTable;
            if (dt != null)
            {
                dt.TableName = "工作计划";
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
                Workplan workplan = comboBox1.SelectedItem as Workplan;
                if (workplan != null)
                {
                    selectStaffControl1.SelectedStaffs = new List<Staff>(){ workplan.销售 };
                    monthCalendar1.SelectionStart = workplan.日期;
                    textBox3.Text = workplan.日期.ToString("yyyy-MM-dd");
                    numericUpDown1.Value = workplan.带人数;
                    numericUpDown2.Value = workplan.号码数;
                    numericUpDown3.Value = workplan.成单数;
                    numericUpDown4.Value = workplan.回访数;
                    textBox6.Text = workplan.备注;
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

        private void textBox1_Click(object sender, EventArgs e)
        {
            monthCalendar2.Show();
        }

        private void textBox2_Click(object sender, EventArgs e)
        {
            monthCalendar3.Show();
        }

        private void monthCalendar2_DateSelected(object sender, DateRangeEventArgs e)
        {
            textBox1.Text = e.Start.ToString("yyyy-MM-dd");
            monthCalendar2.Hide();
        }

        private void monthCalendar3_DateSelected(object sender, DateRangeEventArgs e)
        {
            textBox2.Text = e.Start.ToString("yyyy-MM-dd");
            monthCalendar3.Hide();
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
