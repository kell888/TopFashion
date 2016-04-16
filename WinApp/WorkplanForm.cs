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
        public WorkplanForm(int selectIndex = 0)
        {
            InitializeComponent();
            this.tabControl1.SelectedIndex = selectIndex;
            this.selectStaffControl1.SelectOnlyOne = true;
        }

        private void WorkplanForm_Load(object sender, EventArgs e)
        {
            base.CheckUserPermission(this);
            LoadWorkplans();
            LoadCardTypes();
            comboBox2.SelectedIndex = 0;
            comboBox4.SelectedIndex = 0;
        }

        private void LoadWorkplans()
        {
            List<Workplan> elements = WorkplanLogic.GetInstance().GetAllWorkplans();
            comboBox1.Items.Clear();
            foreach (Workplan element in elements)
            {
                comboBox1.Items.Add(element);
            }
        }

        private void LoadCardTypes()
        {
            List<CardType> elements = CardTypeLogic.GetInstance().GetAllCardTypes();
            comboBox3.Items.Clear();
            comboBox5.Items.Clear();
            comboBox5.Items.Add("--不限--");
            foreach (CardType element in elements)
            {
                comboBox3.Items.Add(element);
                comboBox5.Items.Add(element);
            }
            if (comboBox3.Items.Count > 0)
                comboBox3.SelectedIndex = 0;
            comboBox5.SelectedIndex = 0;
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
                Workplan workplan = new Workplan();
                workplan.ID = ((Product)comboBox1.SelectedItem).ID;
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
                    Workplan workplan = new Workplan();
                    workplan.ID = ((Workplan)comboBox1.SelectedItem).ID;
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
            DataTable dt = Search(textBox8.Text.Trim(), comboBox4.SelectedIndex, comboBox5.SelectedItem as CardType, textBox9.Text.Trim(), textBox7.Text.Trim());
            dataGridView1.DataSource = dt;
        }

        private DataTable Search(string name, int sex = 0, CardType cardType = null, string cardNo = null, string mobile = null)
        {
            string nm = "";
            if (!string.IsNullOrEmpty(name) && name.Trim() != "")
            {
                nm = " and TF_Member.姓名 like '%" + name + "%'";
            }
            string sx = "";
            if (sex > 0)
            {
                sx = " and TF_Member.性别=" + sex;
            }
            string ct = "";
            if (cardType != null)
            {
                ct = " and TF_Member.卡种=" + cardType.ID;
            }
            string cn = "";
            if (!string.IsNullOrEmpty(cardNo) && cardNo.Trim() != "")
            {
                cn = " and TF_Member.卡号 like '%" + cardNo.Trim() + "%'";
            }
            string mb = "";
            if (!string.IsNullOrEmpty(mobile) && mobile.Trim() != "")
            {
                mb = " and TF_Member.电话 like '%" + mobile.Trim() + "%'";
            }
            string where = nm + sx + ct + cn + mb + " order by TF_Workplan.ID desc";
            return WorkplanLogic.GetInstance().GetWorkplans(where);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            DataTable dt = dataGridView1.DataSource as DataTable;
            if (dt != null)
            {
                dt.TableName = "工作计划信息";
                KellPrinter.DataReporter printer = new KellPrinter.DataReporter(dt);
                PrintForm pf = new PrintForm(printer);
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
        }
    }
}
