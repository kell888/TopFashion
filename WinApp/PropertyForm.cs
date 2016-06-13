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
    public partial class PropertyForm : Form
    {
        public PropertyForm(int selectIndex = 0)
        {
            InitializeComponent();
            this.tabControl1.SelectedIndex = selectIndex;
        }

        private void PropertyForm_Load(object sender, EventArgs e)
        {
            LoadPropertys();
        }

        private void LoadPropertys()
        {
            List<Property> elements = PropertyLogic.GetInstance().GetAllPropertys();
            comboBox1.Items.Clear();
            foreach (Property element in elements)
            {
                comboBox1.Items.Add(element);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Property property = new Property();
            property.名称 = textBox1.Text.Trim();
            property.单位 = textBox2.Text.Trim();
            property.用途 = textBox3.Text.Trim();
            property.备注 = textBox4.Text.Trim();
            PropertyLogic pl = PropertyLogic.GetInstance();
            if (pl.ExistsName(property.名称))
            {
                if (MessageBox.Show("系统中已经存在该名称，确定还要继续保存么？", "重名提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.OK)
                {
                    int id = pl.AddProperty(property);
                    if (id > 0)
                    {
                        property.ID = id;
                        LoadPropertys();
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
                int id = pl.AddProperty(property);
                if (id > 0)
                {
                    property.ID = id;
                    LoadPropertys();
                    MessageBox.Show("添加成功！");
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex > -1)
            {
                Property property = new Property();
                property.ID = ((Property)comboBox1.SelectedItem).ID;
                property.名称 = textBox1.Text.Trim();
                property.单位 = textBox2.Text.Trim();
                property.用途 = textBox3.Text.Trim();
                property.备注 = textBox4.Text.Trim();
                PropertyLogic pl = PropertyLogic.GetInstance();
                if (pl.ExistsNameOther(property.名称, property.ID))
                {
                    if (MessageBox.Show("系统中已经存在该名称，确定还要继续保存么？", "重名提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.OK)
                    {
                        if (pl.UpdateProperty(property))
                        {
                            LoadPropertys();
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
                    if (pl.UpdateProperty(property))
                    {
                        LoadPropertys();
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
                if (MessageBox.Show("确定要删除该资产？", "删除提醒", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.OK)
                {
                    Property property = new Property();
                    property.ID = ((Property)comboBox1.SelectedItem).ID;
                    if (PropertyLogic.GetInstance().DeleteProperty(property))
                    {
                        LoadPropertys();
                    }
                }
            }
            else
            {
                MessageBox.Show("先选定要删除的资产！");
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            DataTable dt = Search(textBox8.Text.Trim(), textBox9.Text.Trim());
            dataGridView1.DataSource = dt;
        }

        private DataTable Search(string name, string 用途 = null)
        {
            if (!string.IsNullOrEmpty(name))
            {
                string cj = "";
                if (!string.IsNullOrEmpty(用途))
                    cj = " and 用途 like '%" + 用途 + "%'";
                string where = "名称 like '%" + name + "%'" + cj + " order by ID desc";
                return PropertyLogic.GetInstance().GetPropertys(where);
            }
            return null;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            DataTable dt = dataGridView1.DataSource as DataTable;
            if (dt != null)
            {
                dt.TableName = "资产信息";
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
                Property property = comboBox1.SelectedItem as Property;
                if (property != null)
                {
                    textBox1.Text = property.名称;
                    textBox2.Text = property.单位;
                    textBox3.Text = property.用途;
                    textBox4.Text = property.备注;
                }
            }
        }
    }
}
