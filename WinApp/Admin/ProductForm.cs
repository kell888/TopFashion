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
    public partial class ProductForm : PermissionForm
    {
        public ProductForm(User user, int selectIndex = 0)
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
            if (this.User.ID != this.AdminId && !this.User.GetAllPermissions().ContainsControl("ProductForm", "行政", "tabControl1"))
                this.tabControl1.SelectedIndex = selectIndex;
        }

        private void ProductForm_Load(object sender, EventArgs e)
        {
            base.DisableUserPermission(this);
            LoadProducts();
            LoadProductTypes();
        }

        private void LoadProductTypes()
        {
            List<ProductType> elements = ProductTypeLogic.GetInstance().GetAllProductTypes();
            comboBox2.Items.Clear();
            comboBox3.Items.Clear();
            comboBox3.Items.Add("--不限--");
            foreach (ProductType element in elements)
            {
                comboBox2.Items.Add(element);
                comboBox3.Items.Add(element);
            }
            comboBox3.SelectedIndex = 0;
        }

        private void LoadProducts()
        {
            List<Product> elements = ProductLogic.GetInstance().GetAllProducts();
            comboBox1.Items.Clear();
            foreach (Product element in elements)
            {
                comboBox1.Items.Add(element);
            }
            dataGridView1.DataSource = ProductLogic.GetInstance().GetProducts(string.Empty);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (comboBox2.SelectedIndex == -1)
            {
                MessageBox.Show("请选择产品的种类！如若没有种类可选，请先维护好产品种类！");
                comboBox2.Focus();
                return;
            }
            string jj = textBox3.Text.Trim();
            string sj = textBox4.Text.Trim();
            decimal JJ = 0;
            decimal SJ = 0;
            decimal d = 0;
            if (string.IsNullOrEmpty(jj) || !decimal.TryParse(jj, out d))
            {
                MessageBox.Show("进价必须为数字！");
                textBox3.Focus();
                textBox3.SelectAll();
                return;
            }
            JJ = d;
            if (string.IsNullOrEmpty(sj) || !decimal.TryParse(sj, out d))
            {
                MessageBox.Show("售价必须为数字！");
                textBox4.Focus();
                textBox4.SelectAll();
                return;
            }
            SJ = d;
            Product product = new Product();
            product.品名 = textBox1.Text.Trim();
            product.种类 = comboBox2.SelectedItem as ProductType;
            product.单位 = textBox2.Text.Trim();
            product.进价 = JJ;
            product.售价 = SJ;
            product.厂家 = textBox5.Text.Trim();
            product.姓名 = textBox6.Text.Trim();
            product.电话 = textBox7.Text.Trim();
            product.地址 = textBox10.Text.Trim();
            product.备注 = textBox11.Text.Trim();
            ProductLogic pl = ProductLogic.GetInstance();
            if (pl.ExistsName(product.品名))
            {
                if (MessageBox.Show("系统中已经存在该品名，确定还要继续保存么？", "重名提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.OK)
                {
                    int id = pl.AddProduct(product);
                    if (id > 0)
                    {
                        product.ID = id;
                        LoadProducts();
                        MessageBox.Show("添加成功！");
                    }
                }
                else
                {
                    textBox1.Focus();
                    textBox1.SelectAll();
                }
            }
            else
            {
                int id = pl.AddProduct(product);
                if (id > 0)
                {
                    product.ID = id;
                    LoadProducts();
                    MessageBox.Show("添加成功！");
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex > -1)
            {
                if (comboBox2.SelectedIndex == -1)
                {
                    MessageBox.Show("请选择产品的种类！如若没有种类可选，请先维护好产品种类！");
                    comboBox2.Focus();
                    return;
                }
                string jj = textBox3.Text.Trim();
                string sj = textBox4.Text.Trim();
                decimal JJ = 0;
                decimal SJ = 0;
                decimal d = 0;
                if (string.IsNullOrEmpty(jj) || !decimal.TryParse(jj, out d))
                {
                    MessageBox.Show("进价必须为数字！");
                    textBox3.Focus();
                    textBox3.SelectAll();
                    return;
                }
                JJ = d;
                if (string.IsNullOrEmpty(sj) || !decimal.TryParse(sj, out d))
                {
                    MessageBox.Show("售价必须为数字！");
                    textBox4.Focus();
                    textBox4.SelectAll();
                    return;
                }
                SJ = d;
                Product product = (Product)comboBox1.SelectedItem;
                product.品名 = textBox1.Text.Trim();
                product.种类 = comboBox2.SelectedItem as ProductType;
                product.单位 = textBox2.Text.Trim();
                product.进价 = JJ;
                product.售价 = SJ;
                product.厂家 = textBox5.Text.Trim();
                product.姓名 = textBox6.Text.Trim();
                product.电话 = textBox7.Text.Trim();
                product.地址 = textBox10.Text.Trim();
                product.备注 = textBox11.Text.Trim();
                ProductLogic pl = ProductLogic.GetInstance();
                if (pl.ExistsNameOther(product.品名, product.ID))
                {
                    if (MessageBox.Show("系统中已经存在该品名，确定还要继续保存么？", "重名提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.OK)
                    {
                        if (pl.UpdateProduct(product))
                        {
                            LoadProducts();
                            MessageBox.Show("修改成功！");
                        }
                    }
                    else
                    {
                        textBox1.Focus();
                        textBox1.SelectAll();
                    }
                }
                else
                {
                    if (pl.UpdateProduct(product))
                    {
                        LoadProducts();
                        MessageBox.Show("修改成功！");
                    }
                }
            }
            else
            {
                MessageBox.Show("先选定要修改的产品！");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex > -1)
            {
                if (MessageBox.Show("确定要删除该产品？", "删除提醒", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.OK)
                {
                    Product product = (Product)comboBox1.SelectedItem;
                    if (ProductLogic.GetInstance().DeleteProduct(product))
                    {
                        LoadProducts();
                    }
                }
            }
            else
            {
                MessageBox.Show("先选定要删除的产品！");
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            DataTable dt = Search(textBox8.Text.Trim(), textBox9.Text.Trim(), comboBox3.SelectedItem as ProductType);
            dataGridView1.DataSource = dt;
        }

        private DataTable Search(string name, string 厂家, ProductType type)
        {
            string nm = "";
            if (!string.IsNullOrEmpty(name))
                nm = " and 品名 like '%" + name + "%'";

            string cj = "";
            if (!string.IsNullOrEmpty(厂家))
                cj = " and 厂家 like '%" + 厂家 + "%'";

            string ty = "";
            if (type != null)
                cj = " and 种类='" + type.类型 + "'";

            string where = "(1=1)" + nm + cj + ty;
            return ProductLogic.GetInstance().GetProducts(where);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            DataTable dt = dataGridView1.DataSource as DataTable;
            if (dt != null)
            {
                dt.TableName = "产品信息";
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
                Product product = comboBox1.SelectedItem as Product;
                if (product != null)
                {
                    textBox1.Text = product.品名;
                    comboBox2.SelectedIndex = GetIndexByProductType(product.种类, comboBox2);
                    textBox2.Text = product.单位;
                    textBox3.Text = product.进价.ToString();
                    textBox4.Text = product.售价.ToString();
                    textBox5.Text = product.厂家;
                    textBox6.Text = product.姓名;
                    textBox7.Text = product.电话;
                    textBox10.Text = product.地址;
                    textBox11.Text = product.备注;
                }
            }
        }

        private int GetIndexByProductType(ProductType productType, ComboBox comboBox2)
        {
            if (comboBox2 != null)
            {
                for (int i = 0; i < comboBox2.Items.Count; i++)
                {
                    ProductType pt = (ProductType)comboBox2.Items[i];
                    if (productType.ID == pt.ID)
                        return i;
                }
            }
            return -1;
        }
    }
}
