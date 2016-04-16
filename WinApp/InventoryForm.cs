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
    public partial class InventoryForm : Form
    {
        public InventoryForm(int selectIndex = 0)
        {
            InitializeComponent();
            this.tabControl1.SelectedIndex = selectIndex;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DataTable dt = Search(true, textBox1.Text.Trim(), dateTimePicker1.Value, dateTimePicker2.Value);
            dataGridView1.DataSource = dt;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DataTable dt = Search(false, textBox2.Text.Trim(), dateTimePicker4.Value, dateTimePicker3.Value);
            dataGridView2.DataSource = dt;
        }

        private DataTable Search(bool isProduct, string name, DateTime start, DateTime end)
        {
            DataTable dt = null;
            string where = " like '%" + name + "%' and 更新时间 between '" + start.ToString("yyyy-MM-dd HH:mm:ss.fff") + "' and '" + end.ToString("yyyy-MM-dd HH:mm:ss.fff") + "' order by 更新时间 desc";;
            if (isProduct)
            {
                where = "品名" + where;
                InventoryLogic.GetInstance().GetInventoryView_Product(where);
            }
            else
            {
                where = "名称" + where;
                dt = InventoryLogic.GetInstance().GetInventoryView_Property(where);
            }
            return dt;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            DataTable dt = dataGridView1.DataSource as DataTable;
            if (dt != null)
            {
                dt.TableName = "产品库存信息";
                KellPrinter.DataReporter printer = new KellPrinter.DataReporter(dt);
                PrintForm pf = new PrintForm(printer);
                pf.Show();
            }
            else
            {
                MessageBox.Show("当前没有数据可供打印导出！");
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            DataTable dt = dataGridView2.DataSource as DataTable;
            if (dt != null)
            {
                dt.TableName = "资产库存信息";
                KellPrinter.DataReporter printer = new KellPrinter.DataReporter(dt);
                PrintForm pf = new PrintForm(printer);
                pf.Show();
            }
            else
            {
                MessageBox.Show("当前没有数据可供打印导出！");
            }
        }
    }
}
