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
    public partial class FormTypeForm : PermissionForm
    {
        public FormTypeForm(User user, int selectIndex = 0)
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
            if (this.User.ID != this.AdminId && !this.User.GetAllPermissions().ContainsControl("FormTypeForm", "系统", "tabControl1"))
                this.tabControl1.SelectedIndex = selectIndex;
        }

        private void FormTypeForm_Load(object sender, EventArgs e)
        {
            LoadFormTypes();
            comboBox2.SelectedIndex = 0;
        }

        private void LoadFormTypes()
        {
            List<FormType> elements = FormTypeLogic.GetInstance().GetAllFormTypes();
            comboBox1.Items.Clear();
            foreach (FormType element in elements)
            {
                comboBox1.Items.Add(element);
            }
            dataGridView1.DataSource = FormTypeLogic.GetInstance().GetFormTypes(string.Empty);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FormType formType = new FormType();
            formType.TypeName = textBox1.Text.Trim();
            formType.Remark = textBox2.Text;
            FormTypeLogic al = FormTypeLogic.GetInstance();
            if (al.ExistsName(formType.TypeName))
            {
                if (MessageBox.Show("系统中已经存在该表单类型，确定还要继续保存么？", "重名提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.OK)
                {
                    int id = al.AddFormType(formType);
                    if (id > 0)
                    {
                        formType.ID = id;
                        LoadFormTypes();
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
                int id = al.AddFormType(formType);
                if (id > 0)
                {
                    formType.ID = id;
                    LoadFormTypes();
                    MessageBox.Show("添加成功！");
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex > -1)
            {
                FormType formType = (FormType)comboBox1.SelectedItem;
                formType.TypeName = textBox1.Text.Trim();
                formType.Remark = textBox2.Text;
                FormTypeLogic al = FormTypeLogic.GetInstance();
                if (al.ExistsNameOther(formType.TypeName, formType.ID))
                {
                    if (MessageBox.Show("系统中已经存在该表单类型，确定还要继续保存么？", "重名提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.OK)
                    {
                        if (al.UpdateFormType(formType, this.User))
                        {
                            LoadFormTypes();
                            MessageBox.Show("修改成功！");
                        }
                        else
                        {
                            MessageBox.Show("修改失败或者您不是管理员！");
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
                    if (al.UpdateFormType(formType, this.User))
                    {
                        LoadFormTypes();
                        MessageBox.Show("修改成功！");
                    }
                    else
                    {
                        MessageBox.Show("修改失败或者您不是管理员！");
                    }
                }
            }
            else
            {
                MessageBox.Show("先选定要修改的表单类型！");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex > -1)
            {
                if (MessageBox.Show("确定要删除该表单类型？", "删除提醒", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.OK)
                {
                    FormType formType = (FormType)comboBox1.SelectedItem;
                    if (FormTypeLogic.GetInstance().DeleteFormType(formType, this.User))
                    {
                        LoadFormTypes();
                    }
                    else
                    {
                        MessageBox.Show("删除失败或者您不是管理员！");
                    }
                }
            }
            else
            {
                MessageBox.Show("先选定要删除的表单类型！");
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            DataTable dt = Search(textBox8.Text.Trim());
            dataGridView1.DataSource = dt;
        }

        private DataTable Search(string name)
        {
            string nm = "";
            if (!string.IsNullOrEmpty(name) && name.Trim() != "")
            {
                nm = " and TypeName like '%" + name + "%'";
            }
            string where = "(1=1)" + nm;
            return FormTypeLogic.GetInstance().GetFormTypes(where);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            DataTable dt = dataGridView1.DataSource as DataTable;
            if (dt != null)
            {
                dt.TableName = "表单类型信息";
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
                FormType formType = comboBox1.SelectedItem as FormType;
                if (formType != null)
                {
                    textBox1.Text = formType.TypeName;
                    textBox2.Text = formType.Remark;
                }
            }
        }
    }
}
