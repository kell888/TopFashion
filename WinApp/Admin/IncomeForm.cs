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
    public partial class IncomeForm : PermissionForm
    {
        public IncomeForm(User user, int selectIndex = 0)
        {
            this.User = user;
            InitializeComponent();
            this.tabControl1.SelectedIndex = selectIndex;
            this.selectIndex = selectIndex;
            this.tabControl1.SelectedIndexChanged += new EventHandler(tabControl1_SelectedIndexChanged);
            if (this.User.ID == this.AdminId)
                textBox3.ReadOnly = textBox6.ReadOnly = false;
        }
        int selectIndex;

        void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.User.ID != this.AdminId && !this.User.GetAllPermissions().ContainsControl("IncomeForm", "行政", "tabControl1"))
                this.tabControl1.SelectedIndex = selectIndex;
        }

        private void IncomeForm_Load(object sender, EventArgs e)
        {
            base.DisableUserPermission(this);
            LoadProducts();
            LoadPropertys();
            textBox3.Text = textBox6.Text = this.User.Username;
        }

        private void LoadProducts()
        {
            List<Product> ps = ProductLogic.GetInstance().GetAllProducts();
            comboBox1.Items.Clear();
            foreach (Product p in ps)
            {
                comboBox1.Items.Add(p);
            }
        }

        private void LoadPropertys()
        {
            List<Property> ps = PropertyLogic.GetInstance().GetAllPropertys();
            comboBox2.Items.Clear();
            foreach (Property p in ps)
            {
                comboBox2.Items.Add(p);
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex > -1)
            {
                Product element = comboBox1.SelectedItem as Product;
                if (element != null)
                {
                    label7.Text = element.单位;
                    textBox2.Text = element.进价.ToString();
                }
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox2.SelectedIndex > -1)
            {
                Property element = comboBox2.SelectedItem as Property;
                if (element != null)
                {
                    label8.Text = element.单位;
                    textBox7.Text = element.价格.ToString();
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == -1)
            {
                MessageBox.Show("请先选择要入库的产品！");
                comboBox1.Focus();
                return;
            }
            int num = 0;
            int R;
            if (int.TryParse(textBox1.Text.Trim(), out R))
            {
                num = R;
            }
            else
            {
                MessageBox.Show("数量必须为整数！");
                textBox1.Focus();
                textBox1.SelectAll();
                return;
            }
            decimal price = 0;
            decimal r;
            if (decimal.TryParse(textBox2.Text.Trim(), out r))
            {
                price = r;
            }
            else
            {
                MessageBox.Show("实价必须为整数！");
                textBox2.Focus();
                textBox2.SelectAll();
                return;
            }
            Income element = new Income();
            element.PID = ((Product)comboBox1.SelectedItem).ID;
            element.IsProduct = true;
            element.IsIncome = true;
            element.数量 = num;
            element.实价 = price;
            element.经手人 = textBox3.Text.Trim();
            element.备注 = textBox4.Text.Trim();
            if (IncomeLogic.GetInstance().AddIncome(element) > 0)
                MessageBox.Show("登记成功！");
            else
                MessageBox.Show("登记失败！");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (comboBox2.SelectedIndex == -1)
            {
                MessageBox.Show("请先选择要入库的资产！");
                comboBox2.Focus();
                return;
            }
            int num = 0;
            int R;
            if (int.TryParse(textBox8.Text.Trim(), out R))
            {
                num = R;
            }
            else
            {
                MessageBox.Show("数量必须为整数！");
                textBox8.Focus();
                textBox8.SelectAll();
                return;
            }
            decimal price = 0;
            decimal r;
            if (decimal.TryParse(textBox7.Text.Trim(), out r))
            {
                price = r;
            }
            else
            {
                MessageBox.Show("实价必须为整数！");
                textBox7.Focus();
                textBox7.SelectAll();
                return;
            }
            Income element = new Income();
            element.PID = ((Property)comboBox2.SelectedItem).ID;
            element.IsProduct = false;
            element.IsIncome = true;
            element.数量 = num;
            element.实价 = price;
            element.经手人 = textBox6.Text.Trim();
            element.备注 = textBox5.Text.Trim();
            if (IncomeLogic.GetInstance().AddIncome(element) > 0)
                MessageBox.Show("登记成功！");
            else
                MessageBox.Show("登记失败！");
        }

        private void SumProduct(string num, string price)
        {
            decimal nm, pri;
            if (decimal.TryParse(num, out nm) && decimal.TryParse(price, out pri))
            {
                decimal sum = nm * pri;
                产品入库Button.Text = "入库(" + sum + "元)";
            }
        }

        private void SumProperty(string num, string price)
        {
            decimal nm, pri;
            if (decimal.TryParse(num, out nm) && decimal.TryParse(price, out pri))
            {
                decimal sum = nm * pri;
                资产入库Button.Text = "入库(" + sum + "元)";
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            SumProduct(textBox1.Text.Trim(), textBox2.Text.Trim());
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            SumProduct(textBox1.Text.Trim(), textBox2.Text.Trim());
        }

        private void textBox8_TextChanged(object sender, EventArgs e)
        {
            SumProperty(textBox8.Text.Trim(), textBox7.Text.Trim());
        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {
            SumProperty(textBox8.Text.Trim(), textBox7.Text.Trim());
        }
    }
}
