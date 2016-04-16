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
    public partial class InventoryForm : PermissionForm
    {
        public InventoryForm(User user, int selectIndex = 0)
        {
            this.User = user;
            InitializeComponent();
            this.tabControl1.SelectedIndex = selectIndex;
            this.selectIndex = selectIndex;
            this.tabControl1.SelectedIndexChanged += new EventHandler(tabControl1_SelectedIndexChanged);
            string start = DateTime.Now.ToString("yyyy-MM-dd") + " 00:00";
            dateTimePicker1.Value = DateTime.Parse(start);
            dateTimePicker4.Value = DateTime.Parse(start);
        }
        int selectIndex;

        void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.User.ID != this.AdminId && !this.User.GetAllPermissions().ContainsControl("InventoryForm", "行政", "tabControl1"))
                this.tabControl1.SelectedIndex = selectIndex;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DataTable dt = Search(true, textBox1.Text.Trim(), dateTimePicker1.Value, dateTimePicker2.Value, comboBox1.SelectedIndex, comboBox2.SelectedItem as ProductType);
            dataGridView1.DataSource = dt;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DataTable dt = Search(false, textBox2.Text.Trim(), dateTimePicker4.Value, dateTimePicker3.Value, comboBox3.SelectedIndex);
            dataGridView2.DataSource = dt;
        }

        private DataTable Search(bool isProduct, string name, DateTime start, DateTime end, int action, ProductType pt = null)
        {
            DataTable dt = null;
            string time = " and 更新时间 between '" + start.ToString("yyyy-MM-dd HH:mm") + "' and '" + end.ToString("yyyy-MM-dd HH:mm") + "'";
            string act = "";
            if (action > 0)
            {
                act = " and 动作='" + (action == 1 ? "入库" : "出库") + "'";
            }
            if (isProduct)
            {
                string nm = "";
                if (!string.IsNullOrEmpty(name) && name.Trim() != "")
                {
                    nm = " and 品名 like '%" + name + "%'";
                }
                string type = "";
                if (pt != null)
                    type = " and 种类='" + pt.类型 + "'";
                string where = "(1=1)" + nm + time + act + type;
                dt = InventoryLogic.GetInstance().GetInventoryView_Product(where);
            }
            else
            {
                string nm = "";
                if (!string.IsNullOrEmpty(name) && name.Trim() != "")
                {
                    nm = " and 名称 like '%" + name + "%'";
                }
                string where = "(1=1)" + nm + time + act;
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
                PrintForm pf = new PrintForm(this.User, printer);
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
                PrintForm pf = new PrintForm(this.User, printer);
                pf.Show();
            }
            else
            {
                MessageBox.Show("当前没有数据可供打印导出！");
            }
        }

        private void InventoryForm_Load(object sender, EventArgs e)
        {
            base.DisableUserPermission(this);
            LoadInventorys();
            LoadProductTypes();
            comboBox1.SelectedIndex = comboBox3.SelectedIndex = 0;
        }

        private void LoadProductTypes()
        {
            List<ProductType> elements = ProductTypeLogic.GetInstance().GetAllProductTypes();
            comboBox2.Items.Clear();
            comboBox2.Items.Add("--不限--");
            foreach (ProductType element in elements)
            {
                comboBox2.Items.Add(element);
            }
            comboBox2.SelectedIndex = 0;
        }

        private void LoadInventorys()
        {
            DataTable dt1 = InventoryLogic.GetInstance().GetInventoryView_Product(string.Empty);
            DataTable dt2 = InventoryLogic.GetInstance().GetInventoryView_Property(string.Empty);
            dataGridView1.DataSource = dt1;
            dataGridView2.DataSource = dt2;
        }
    }
}
