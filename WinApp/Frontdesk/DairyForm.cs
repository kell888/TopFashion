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
    public partial class DairyForm : PermissionForm
    {
        public DairyForm(User user, int selectIndex = 0)
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
            if (this.User.ID != this.AdminId && !this.User.GetAllPermissions().ContainsControl("DairyForm", "前台", "tabControl1"))
                this.tabControl1.SelectedIndex = selectIndex;
        }

        private void DairyForm_Load(object sender, EventArgs e)
        {
            base.DisableUserPermission(this);
            LoadDairys();
        }

        private void LoadDairys()
        {
            List<Dairy> elements = DairyLogic.GetInstance().GetAllDairys();
            comboBox1.Items.Clear();
            foreach (Dairy element in elements)
            {
                comboBox1.Items.Add(element);
            }
            dataGridView1.DataSource = DairyLogic.GetInstance().GetDairys(string.Empty);
        }

        private void 搜索Button_Click(object sender, EventArgs e)
        {
            DataTable dt = Search(dateTimePicker1.Value, dateTimePicker2.Value, textBox1.Text.Trim());
            dataGridView1.DataSource = dt;
        }

        private DataTable Search(DateTime start, DateTime end, string staff)
        {
            string jsr = "";
            if (!string.IsNullOrEmpty(staff) && staff.Trim() != "")
            {
                jsr = " and 经手人 like '%" + staff.Trim() + "%'";
            }
            string where = "(1=1)" + jsr + " and (日期 >= '" + start + "' and 日期 < '" + end.AddDays(1) + "')";
            return DairyLogic.GetInstance().GetDairys(where);
        }

        private void 打印Button_Click(object sender, EventArgs e)
        {
            DataTable dt = dataGridView1.DataSource as DataTable;
            if (dt != null)
            {
                dt.TableName = "每日业绩信息";
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
                Dairy dairy = comboBox1.SelectedItem as Dairy;
                if (dairy != null)
                {
                    numericUpDown1.Value = dairy.Pos机会籍;
                    numericUpDown2.Value = dairy.Pos机私教;
                    numericUpDown3.Value = dairy.现金会籍;
                    numericUpDown4.Value = dairy.现金私教;
                    numericUpDown5.Value = dairy.存水费;
                    numericUpDown6.Value = dairy.水吧余;
                    numericUpDown7.Value = dairy.总金额;
                    selectStaffControl1.SelectedStaffs = new List<Staff>() { dairy.经手人 };
                    dateTimePicker3.Value = dairy.日期;
                    textBox2.Text = dairy.备注;
                }
            }
        }

        private void 添加Button_Click(object sender, EventArgs e)
        {
            Dairy dairy = new Dairy();
            dairy.Pos机会籍 = numericUpDown1.Value;
            dairy.Pos机私教 = numericUpDown2.Value;
            dairy.现金会籍 = numericUpDown3.Value;
            dairy.现金私教 = numericUpDown4.Value;
            dairy.存水费 = numericUpDown5.Value;
            dairy.水吧余 = numericUpDown6.Value;
            dairy.总金额 = numericUpDown7.Value;
            dairy.经手人 = (selectStaffControl1.SelectedStaffs != null && selectStaffControl1.SelectedStaffs.Count > 0) ? selectStaffControl1.SelectedStaffs[0] : null;
            dairy.日期 = dateTimePicker3.Value;
            dairy.备注 = textBox2.Text;
            DairyLogic al = DairyLogic.GetInstance();
            int id = al.AddDairy(dairy);
            if (id > 0)
            {
                dairy.ID = id;
                LoadDairys();
                MessageBox.Show("添加成功！");
            }
        }

        private void 修改Button_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex > -1)
            {
                Dairy dairy = (Dairy)comboBox1.SelectedItem;
                dairy.Pos机会籍 = numericUpDown1.Value;
                dairy.Pos机私教 = numericUpDown2.Value;
                dairy.现金会籍 = numericUpDown3.Value;
                dairy.现金私教 = numericUpDown4.Value;
                dairy.存水费 = numericUpDown5.Value;
                dairy.水吧余 = numericUpDown6.Value;
                dairy.总金额 = numericUpDown7.Value;
                dairy.经手人 = (selectStaffControl1.SelectedStaffs != null && selectStaffControl1.SelectedStaffs.Count > 0) ? selectStaffControl1.SelectedStaffs[0] : null;
                dairy.日期 = dateTimePicker3.Value;
                dairy.备注 = textBox2.Text;
                DairyLogic al = DairyLogic.GetInstance();
                if (al.UpdateDairy(dairy))
                {
                    LoadDairys();
                    MessageBox.Show("修改成功！");
                }
            }
            else
            {
                MessageBox.Show("先选定要修改的业绩！");
            }
        }

        private void 删除Button_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex > -1)
            {
                if (MessageBox.Show("确定要删除该业绩？", "删除提醒", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.OK)
                {
                    Alert alert = (Alert)comboBox1.SelectedItem;
                    if (AlertLogic.GetInstance().DeleteAlert(alert))
                    {
                        LoadDairys();
                    }
                }
            }
            else
            {
                MessageBox.Show("先选定要删除的业绩！");
            }
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            numericUpDown7.Value = numericUpDown1.Value + numericUpDown2.Value + numericUpDown3.Value + numericUpDown4.Value;
        }
    }
}
