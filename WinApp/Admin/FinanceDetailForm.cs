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
    public partial class FinanceDetailForm : PermissionForm
    {
        public FinanceDetailForm(int selectIndex = 0)
        {
            InitializeComponent();
            this.tabControl1.SelectedIndex = selectIndex;
        }

        private void FinanceDetailForm_Load(object sender, EventArgs e)
        {
            base.CheckUserPermission(this);
            LoadFinanceDetails();
            comboBox2.SelectedIndex = 0;
        }

        private void LoadFinanceDetails()
        {
            List<FinanceDetail> elements = FinanceDetailLogic.GetInstance().GetAllFinanceDetails();
            comboBox1.Items.Clear();
            foreach (FinanceDetail element in elements)
            {
                comboBox1.Items.Add(element);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            decimal JE = 0;
            decimal d = 0;
            string jj = textBox2.Text.Trim();
            if (string.IsNullOrEmpty(jj) || !decimal.TryParse(jj, out d))
            {
                MessageBox.Show("金额必须为数字！");
                textBox2.Focus();
                textBox2.SelectAll();
            }
                JE=d;
            FinanceDetail finance = new FinanceDetail();
            finance.项目 = textBox1.Text.Trim();
            finance.金额 = JE;
            finance.是否进账 = checkBox1.Checked;
            finance.责任人 = textBox3.Text.Trim();
            finance.备注 = textBox4.Text;
            FinanceDetailLogic pl = FinanceDetailLogic.GetInstance();
                    int id = pl.AddFinanceDetail(finance);
                    if (id > 0)
                    {
                        finance.ID = id;
                        LoadFinanceDetails();
                        MessageBox.Show("添加成功！");
                    }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex > -1)
            {
                decimal JE = 0;
                decimal d = 0;
                string jj = textBox2.Text.Trim();
                if (string.IsNullOrEmpty(jj) || !decimal.TryParse(jj, out d))
                {
                    MessageBox.Show("金额必须为数字！");
                    textBox2.Focus();
                    textBox2.SelectAll();
                }
                JE = d;
                FinanceDetail finance = new FinanceDetail();
                finance.项目 = textBox1.Text.Trim();
                finance.金额 = JE;
                finance.是否进账 = checkBox1.Checked;
                finance.责任人 = textBox3.Text.Trim();
                finance.备注 = textBox4.Text;
                FinanceDetailLogic pl = FinanceDetailLogic.GetInstance();
                        if (pl.UpdateFinanceDetail(finance))
                        {
                            LoadFinanceDetails();
                            MessageBox.Show("修改成功！");
                        }
            }
            else
            {
                MessageBox.Show("先选定要修改的流水明细！");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex > -1)
            {
                if (MessageBox.Show("确定要删除该流水明细？", "删除提醒", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.OK)
                {
                    FinanceDetail finance = new FinanceDetail();
                    finance.ID = ((FinanceDetail)comboBox1.SelectedItem).ID;
                    if (FinanceDetailLogic.GetInstance().DeleteFinanceDetail(finance))
                    {
                        LoadFinanceDetails();
                    }
                }
            }
            else
            {
                MessageBox.Show("先选定要删除的流水明细！");
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            DataTable dt = Search(textBox8.Text.Trim(), textBox9.Text.Trim(), comboBox2.SelectedIndex);
            dataGridView1.DataSource = dt;
        }

        private DataTable Search(string name, string man, int isIncome)
        {
            string nm = "";
            if (!string.IsNullOrEmpty(name) && name.Trim() != "")
            {
                nm = " and 项目 like '%" + name + "%'";
            }
            string mn = "";
            if (!string.IsNullOrEmpty(man) && man.Trim() != "")
            {
                mn = " and 责任人 like '%" + man.Trim() + "%'";
            }
            string ii = "";
            if (isIncome > 0)
            {
                ii = " and 是否进账=" + (isIncome == 1 ? "1" : "0");
            }
            string where = "(1=1)" + nm + mn + ii + " order by ID desc";
            return FinanceDetailLogic.GetInstance().GetFinanceDetails(where);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            DataTable dt = dataGridView1.DataSource as DataTable;
            if (dt != null)
            {
                dt.TableName = "流水明细信息";
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
                FinanceDetail finance = comboBox1.SelectedItem as FinanceDetail;
                if (finance != null)
                {
                    textBox1.Text = finance.项目;
                    textBox2.Text = finance.金额.ToString();
                    checkBox1.Checked = finance.是否进账;
                    textBox3.Text = finance.责任人;
                    textBox4.Text = finance.备注;
                }
            }
        }
    }
}
