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
    public partial class OutcomeForm : Form
    {
        public OutcomeForm(int selectIndex = 0)
        {
            InitializeComponent();
            this.tabControl1.SelectedIndex = selectIndex;
        }

        private void OutcomeForm_Load(object sender, EventArgs e)
        {
            LoadProducts();
            LoadPropertys();
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
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
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
            element.IsProduct = true;
            element.IsIncome = false;
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
            element.IsProduct = false;
            element.IsIncome = false;
            element.数量 = num;
            element.实价 = price;
            element.经手人 = textBox6.Text.Trim();
            element.备注 = textBox5.Text.Trim();
            if (IncomeLogic.GetInstance().AddIncome(element) > 0)
                MessageBox.Show("登记成功！");
            else
                MessageBox.Show("登记失败！");
        }
    }
}
