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
    public partial class ProductTypeForm : PermissionForm
    {
        public ProductTypeForm(User user, int selectIndex = 0)
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
            if (this.User.ID != this.AdminId && !this.User.GetAllPermissions().ContainsControl("ProductTypeForm", "行政", "tabControl1"))
                this.tabControl1.SelectedIndex = selectIndex;
        }

        private void ProductTypeForm_Load(object sender, EventArgs e)
        {
            base.DisableUserPermission(this);
            LoadProductTypes();
            comboBox2.SelectedIndex = 0;
        }

        private void LoadProductTypes()
        {
            List<ProductType> elements = ProductTypeLogic.GetInstance().GetAllProductTypes();
            comboBox1.Items.Clear();
            foreach (ProductType element in elements)
            {
                comboBox1.Items.Add(element);
            }
            dataGridView1.DataSource = ProductTypeLogic.GetInstance().GetProductTypes(string.Empty);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ProductType productType = new ProductType();
            productType.类型 = textBox1.Text.Trim();
            productType.备注 = textBox2.Text.Trim();
            productType.Flag = checkBox1.Checked;
            ProductTypeLogic al = ProductTypeLogic.GetInstance();
            if (al.ExistsName(productType.类型))
            {
                if (MessageBox.Show("系统中已经存在该产品类型，确定还要继续保存么？", "重名提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.OK)
                {
                    int id = al.AddProductType(productType);
                    if (id > 0)
                    {
                        productType.ID = id;
                        LoadProductTypes();
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
                int id = al.AddProductType(productType);
                if (id > 0)
                {
                    productType.ID = id;
                    LoadProductTypes();
                    MessageBox.Show("添加成功！");
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex > -1)
            {
                ProductType productType = (ProductType)comboBox1.SelectedItem;
                productType.类型 = textBox1.Text.Trim();
                productType.备注 = textBox2.Text.Trim();
                productType.Flag = checkBox1.Checked;
                ProductTypeLogic al = ProductTypeLogic.GetInstance();
                if (al.ExistsNameOther(productType.类型, productType.ID))
                {
                    if (MessageBox.Show("系统中已经存在该产品类型，确定还要继续保存么？", "重名提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.OK)
                    {
                        if (al.UpdateProductType(productType))
                        {
                            LoadProductTypes();
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
                    if (al.UpdateProductType(productType))
                    {
                        LoadProductTypes();
                        MessageBox.Show("修改成功！");
                    }
                }
            }
            else
            {
                MessageBox.Show("先选定要修改的产品类型！");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex > -1)
            {
                if (MessageBox.Show("确定要删除该产品类型？", "删除提醒", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.OK)
                {
                    ProductType productType = (ProductType)comboBox1.SelectedItem;
                    if (ProductTypeLogic.GetInstance().DeleteProductType(productType))
                    {
                        LoadProductTypes();
                    }
                }
            }
            else
            {
                MessageBox.Show("先选定要删除的产品类型！");
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            DataTable dt = Search(textBox8.Text.Trim(), comboBox2.SelectedIndex);
            dataGridView1.DataSource = dt;
        }

        private DataTable Search(string name = null, int flag = 0)
        {
            string nm = "";
            if (!string.IsNullOrEmpty(name) && name.Trim() != "")
            {
                nm = " and 类型 like '%" + name + "%'";
            }
            string jy = "";
            if (flag > 0)
            {
                jy = " and Flag=" + flag;
            }
            string where = "(1=1)" + nm + jy;
            return ProductTypeLogic.GetInstance().GetProductTypes(where);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            DataTable dt = dataGridView1.DataSource as DataTable;
            if (dt != null)
            {
                dt.TableName = "产品类型信息";
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
                ProductType productType = comboBox1.SelectedItem as ProductType;
                if (productType != null)
                {
                    textBox1.Text = productType.类型;
                    textBox2.Text = productType.备注;
                    checkBox1.Checked = productType.Flag;
                }
            }
        }
    }
}
