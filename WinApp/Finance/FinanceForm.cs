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
    public partial class FinanceForm : PermissionForm
    {
        public FinanceForm(User user, int selectIndex = 0)
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
            if (this.User.ID != this.AdminId && !this.User.GetAllPermissions().ContainsControl("FinanceForm", "报表", "tabControl1"))
                this.tabControl1.SelectedIndex = selectIndex;
        }

        private void FinanceForm_Load(object sender, EventArgs e)
        {
            base.DisableUserPermission(this);
            LoadFinances();
            comboBox2.SelectedIndex = 0;
            textBox7.Text = DateTime.Now.ToString("yyyy-MM-dd");
        }

        private void LoadFinances()
        {
            List<Finance> elements = FinanceLogic.GetInstance().GetAllFinances();
            comboBox1.Items.Clear();
            foreach (Finance element in elements)
            {
                comboBox1.Items.Add(element);
            }
            dataGridView1.DataSource = FinanceLogic.GetInstance().GetFinances(string.Empty);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string jj = textBox2.Text.Trim();
            string sj = textBox6.Text.Trim();
            decimal JJ = 0;
            decimal SJ = 0;
            decimal d = 0;
            if (string.IsNullOrEmpty(jj) || !decimal.TryParse(jj, out d))
            {
                MessageBox.Show("金额必须为数字！");
                textBox2.Focus();
                textBox2.SelectAll();
            }
            JJ = d;
            if (string.IsNullOrEmpty(sj) || !decimal.TryParse(sj, out d))
            {
                MessageBox.Show("余款必须为数字！");
                textBox6.Focus();
                textBox6.SelectAll();
            }
            SJ = d;
            Finance finance = new Finance();
            finance.项目 = textBox1.Text.Trim();
            finance.金额 = JJ;
            finance.是否进账 = checkBox1.Checked;
            finance.余款 = SJ;
            finance.经手人 = textBox3.Text.Trim();
            finance.接收人 = textBox4.Text.Trim();
            finance.日期 = DateTime.Parse(textBox7.Text.Trim());
            finance.Detail = textBox10.Text;
            FinanceLogic pl = FinanceLogic.GetInstance();
            int id = pl.AddFinance(finance);
            if (id > 0)
            {
                finance.ID = id;
                LoadFinances();
                MessageBox.Show("添加成功！");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex > -1)
            {
                string jj = textBox2.Text.Trim();
                string sj = textBox6.Text.Trim();
                decimal JJ = 0;
                decimal SJ = 0;
                decimal d = 0;
                if (string.IsNullOrEmpty(jj) || !decimal.TryParse(jj, out d))
                {
                    MessageBox.Show("金额必须为数字！");
                    textBox2.Focus();
                    textBox2.SelectAll();
                }
                JJ = d;
                if (string.IsNullOrEmpty(sj) || !decimal.TryParse(sj, out d))
                {
                    MessageBox.Show("余款必须为数字！");
                    textBox6.Focus();
                    textBox6.SelectAll();
                }
                SJ = d;
                Finance finance = (Finance)comboBox1.SelectedItem;
                finance.项目 = textBox1.Text.Trim();
                finance.金额 = JJ;
                finance.是否进账 = checkBox1.Checked;
                finance.余款 = SJ;
                finance.经手人 = textBox3.Text.Trim();
                finance.接收人 = textBox4.Text.Trim();
                finance.日期 = DateTime.Parse(textBox7.Text.Trim());
                finance.Detail = textBox10.Text;
                FinanceLogic pl = FinanceLogic.GetInstance();
                if (pl.UpdateFinance(finance))
                {
                    LoadFinances();
                    MessageBox.Show("修改成功！");
                }
            }
            else
            {
                MessageBox.Show("先选定要修改的流水帐！");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex > -1)
            {
                if (MessageBox.Show("确定要删除该流水帐？", "删除提醒", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.OK)
                {
                    Finance finance = (Finance)comboBox1.SelectedItem;
                    if (FinanceLogic.GetInstance().DeleteFinance(finance))
                    {
                        LoadFinances();
                    }
                }
            }
            else
            {
                MessageBox.Show("先选定要删除的流水帐！");
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            DataTable dt = Search(textBox8.Text.Trim(), textBox9.Text.Trim(), textBox5.Text.Trim(), comboBox2.SelectedIndex);
            dataGridView1.DataSource = dt;
        }

        private DataTable Search(string name, string man, string rec, int isIncome)
        {
            string nm = "";
            if (!string.IsNullOrEmpty(name) && name.Trim() != "")
            {
                nm = " and 项目 like '%" + name + "%'";
            }
            string mn = "";
            if (!string.IsNullOrEmpty(man) && man.Trim() != "")
            {
                mn = " and 经手人 like '%" + man.Trim() + "%'";
            }
            string rc = "";
            if (!string.IsNullOrEmpty(rec) && rec.Trim() != "")
            {
                rc = " and 接收人 like '%" + rec.Trim() + "%'";
            }
            string ii = "";
            if (isIncome > 0)
            {
                ii = " and 进账='" + (isIncome == 1 ? "是" : "否") + "'";
            }
            string where = "(1=1)" + nm + mn + ii;
            return FinanceLogic.GetInstance().GetFinances(where);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            DataTable dt = dataGridView1.DataSource as DataTable;
            if (dt != null)
            {
                dt.TableName = "流水帐信息";
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
                Finance finance = comboBox1.SelectedItem as Finance;
                if (finance != null)
                {
                    textBox1.Text = finance.项目;
                    textBox2.Text = finance.金额.ToString();
                    textBox3.Text = finance.经手人;
                    textBox4.Text = finance.接收人;
                    textBox6.Text = finance.余款.ToString();
                    monthCalendar1.SelectionStart = finance.日期;
                    textBox7.Text = finance.日期.ToString("yyyy-MM-dd");
                    checkBox1.Checked = finance.是否进账;
                    textBox10.Text = finance.Detail;
                    textBox10.Tag = Commons.GetDetailsByStr(finance.Detail);
                }
            }
        }

        private void textBox7_Click(object sender, EventArgs e)
        {
            monthCalendar1.Show();
        }

        private void monthCalendar1_DateSelected(object sender, DateRangeEventArgs e)
        {
            textBox7.Text = e.Start.ToString("yyyy-MM-dd");
            monthCalendar1.Hide();
        }

        private void ShowDetail()
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                object obj = dataGridView1.SelectedRows[0].Cells[0].Value;
                if (obj is int)
                {
                    int id = Convert.ToInt32(obj);
                    FinanceDetailForm fdf = new FinanceDetailForm(this.User, 1, id);
                    fdf.ShowDialog();
                }
                else
                {
                    MessageBox.Show("获取不到合法的ID值！");
                }
            }
            else
            {
                MessageBox.Show("请先选择一个流水账！");
            }
        }

        private void 添加明细Button_Click(object sender, EventArgs e)
        {
            SelectFinanceDetailForm sfdf = new SelectFinanceDetailForm();
            if (sfdf.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                textBox10.Tag = sfdf.SelectedDetails;
                textBox10.Text = Commons.GetDetailsStr(sfdf.SelectedDetails);
            }
            sfdf.Dispose();
        }

        private void dataGridView1_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                if (e.RowIndex > -1)
                {
                    ShowDetail();
                }
            }
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