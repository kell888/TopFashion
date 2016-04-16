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
    public partial class AlertTypeForm : Form
    {
        public AlertTypeForm(int selectIndex = 0)
        {
            InitializeComponent();
            this.tabControl1.SelectedIndex = selectIndex;
        }

        private void AlertForm_Load(object sender, EventArgs e)
        {
            LoadAlertTypes();
            comboBox2.SelectedIndex = 0;
        }

        private void LoadAlertTypes()
        {
            List<AlertType> elements = AlertTypeLogic.GetInstance().GetAllAlertTypes();
            comboBox1.Items.Clear();
            foreach (AlertType element in elements)
            {
                comboBox1.Items.Add(element);
            }
            if (comboBox1.Items.Count > 0)
                comboBox1.SelectedIndex = 0;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            AlertType alertType = new AlertType();
            alertType.方式 = textBox1.Text.Trim();
            alertType.备注 = textBox2.Text.Trim();
            alertType.Flag = checkBox1.Checked;
            AlertTypeLogic al = AlertTypeLogic.GetInstance();
            if (al.ExistsName(alertType.方式))
            {
                if (MessageBox.Show("系统中已经存在该提醒方式，确定还要继续保存么？", "重名提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.OK)
                {
                    int id = al.AddAlertType(alertType);
                    if (id > 0)
                    {
                        alertType.ID = id;
                        LoadAlertTypes();
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
                int id = al.AddAlertType(alertType);
                if (id > 0)
                {
                    alertType.ID = id;
                    LoadAlertTypes();
                    MessageBox.Show("添加成功！");
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex > -1)
            {
                AlertType alertType = new AlertType();
                alertType.ID = ((AlertType)comboBox1.SelectedItem).ID;
                alertType.方式 = textBox1.Text.Trim();
                alertType.备注 = textBox2.Text.Trim();
                alertType.Flag = checkBox1.Checked;
                AlertTypeLogic al = AlertTypeLogic.GetInstance();
                if (al.ExistsNameOther(alertType.方式, alertType.ID))
                {
                    if (MessageBox.Show("系统中已经存在该提醒方式，确定还要继续保存么？", "重名提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.OK)
                    {
                        if (al.UpdateAlertType(alertType))
                        {
                            LoadAlertTypes();
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
                    if (al.UpdateAlertType(alertType))
                    {
                        LoadAlertTypes();
                        MessageBox.Show("修改成功！");
                    }
                }
            }
            else
            {
                MessageBox.Show("先选定要修改的提醒方式！");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex > -1)
            {
                if (MessageBox.Show("确定要删除该提醒方式？", "删除提醒", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.OK)
                {
                    AlertType alertType = new AlertType();
                    alertType.ID = ((AlertType)comboBox1.SelectedItem).ID;
                    if (AlertTypeLogic.GetInstance().DeleteAlertType(alertType))
                    {
                        LoadAlertTypes();
                    }
                }
            }
            else
            {
                MessageBox.Show("先选定要删除的提醒方式！");
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
                nm = " and 方式 like '%" + name + "%'";
            }
            string jy = "";
            if (flag > 0)
            {
                jy = " and Flag=" + flag;
            }
            string where = "(1=1)" + nm + jy + " order by ID desc";
            return AlertTypeLogic.GetInstance().GetAlertTypes(where);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            DataTable dt = dataGridView1.DataSource as DataTable;
            if (dt != null)
            {
                dt.TableName = "提醒方式信息";
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
                AlertType alertType = comboBox1.SelectedItem as AlertType;
                if (alertType != null)
                {
                    textBox1.Text = alertType.方式;
                    textBox2.Text = alertType.备注;
                    checkBox1.Checked = alertType.Flag;
                }
            }
        }
    }
}
