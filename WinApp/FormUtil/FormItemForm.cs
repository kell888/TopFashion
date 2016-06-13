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
    public partial class FormItemForm : PermissionForm
    {
        public FormItemForm(User user, int selectIndex = 0)
        {
            this.User = user;
            InitializeComponent();
            this.tabControl1.SelectedIndex = selectIndex;
        }

        private void FormItemForm_Load(object sender, EventArgs e)
        {
            LoadFormItems();
            LoadSystemTypes();
        }

        private void LoadSystemTypes()
        {
            comboBox2.Items.Clear();
            comboBox2.Items.Add("--不限--");
            SystemType[] elements = (SystemType[])Enum.GetValues(typeof(SystemType));
            foreach (SystemType element in elements)
            {
                string type = element.ToString();
                comboBox2.Items.Add(type);
            }
            comboBox2.SelectedIndex = 0;
        }

        private void LoadFormItems()
        {
            List<FormItem> elements = FormItemLogic.GetInstance().GetAllFormItems();
            comboBox1.Items.Clear();
            foreach (FormItem element in elements)
            {
                comboBox1.Items.Add(element);
            }
            if (comboBox1.Items.Count > 0)
                comboBox1.SelectedIndex = 0;
            dataGridView1.DataSource = FormItemLogic.GetInstance().GetFormItems(string.Empty);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FormItem formItem = formEditControl1.Field;
            FormItemLogic al = FormItemLogic.GetInstance();
            int id = al.AddFormItem(formItem);
            if (id > 0)
            {
                formItem.ID = id;
                LoadFormItems();
                MessageBox.Show("添加成功！");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex > -1)
            {
                FormItem formItem = formEditControl1.Field;
                FormItemLogic al = FormItemLogic.GetInstance();
                if (al.UpdateFormItem(formItem))
                {
                    LoadFormItems();
                    MessageBox.Show("修改成功！");
                }
            }
            else
            {
                MessageBox.Show("先选定要修改的表单字段！");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex > -1)
            {
                if (MessageBox.Show("确定要删除该表单字段？", "删除提醒", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.OK)
                {
                    FormItem formItem = new FormItem();
                    formItem.ID = ((FormItem)comboBox1.SelectedItem).ID;
                    if (FormItemLogic.GetInstance().DeleteFormItem(formItem))
                    {
                        LoadFormItems();
                    }
                }
            }
            else
            {
                MessageBox.Show("先选定要删除的表单字段！");
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            DataTable dt = Search(textBox8.Text.Trim(), comboBox2.Text);
            dataGridView1.DataSource = dt;
        }

        private DataTable Search(string name, string type)
        {
            string nm = "";
            if (!string.IsNullOrEmpty(name) && name.Trim() != "")
            {
                nm = " and ItemName like '%" + name + "%'";
            }
            string ty = "";
            if (type != "--不限--")
            {
                ty = " and ItemType='" + Commons.GetType((SystemType)Enum.Parse(typeof(SystemType), comboBox2.Text)).FullName + "'";
            }
            string where = "(1=1)" + nm + ty;
            return FormItemLogic.GetInstance().GetFormItems(where);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            DataTable dt = dataGridView1.DataSource as DataTable;
            if (dt != null)
            {
                dt.TableName = "表单字段信息";
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
                FormItem formItem = comboBox1.SelectedItem as FormItem;
                formEditControl1.Field = formItem;
            }
        }
    }
}
