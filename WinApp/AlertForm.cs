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
    public partial class AlertForm : Form
    {
        public AlertForm(int selectIndex = 0)
        {
            InitializeComponent();
            this.tabControl1.SelectedIndex = selectIndex;
        }

        private void AlertForm_Load(object sender, EventArgs e)
        {
            LoadAlerts();
            LoadAlertTypes();
        }

        private void LoadAlerts()
        {
            List<Alert> elements = AlertLogic.GetInstance().GetAllAlerts();
            comboBox1.Items.Clear();
            foreach (Alert element in elements)
            {
                comboBox1.Items.Add(element);
            }
        }

        private void LoadAlertTypes()
        {
            List<AlertType> elements = AlertTypeLogic.GetInstance().GetAllAlertTypes();
            comboBox3.Items.Clear();
            comboBox2.Items.Clear();
            comboBox2.Items.Add("--不限--");
            foreach (AlertType element in elements)
            {
                comboBox3.Items.Add(element);
                comboBox2.Items.Add(element);
            }
            if (comboBox3.Items.Count > 0)
                comboBox3.SelectedIndex = 0;
            comboBox2.SelectedIndex = 0;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Alert alert = new Alert();
            alert.提醒项目 = textBox1.Text.Trim();
            alert.提醒对象 = textBox2.Text.Trim();
            alert.提醒时间 = dateTimePicker1.Value;
            alert.提醒方式 = comboBox3.SelectedItem as AlertType;
            alert.备注 = textBox3.Text.Trim();
            AlertLogic al = AlertLogic.GetInstance();
            if (al.ExistsName(alert.提醒项目))
            {
                if (MessageBox.Show("系统中已经存在该提醒，确定还要继续保存么？", "重名提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.OK)
                {
                    int id = al.AddAlert(alert);
                    if (id > 0)
                    {
                        alert.ID = id;
                        LoadAlerts();
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
                int id = al.AddAlert(alert);
                if (id > 0)
                {
                    alert.ID = id;
                    LoadAlerts();
                    MessageBox.Show("添加成功！");
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex > -1)
            {
                Alert alert = new Alert();
                alert.ID = ((Alert)comboBox1.SelectedItem).ID;
                alert.提醒项目 = textBox1.Text.Trim();
                alert.提醒对象 = textBox2.Text.Trim();
                alert.提醒时间 = dateTimePicker1.Value;
                alert.提醒方式 = comboBox3.SelectedItem as AlertType;
                alert.备注 = textBox3.Text.Trim();
                AlertLogic al = AlertLogic.GetInstance();
                if (al.ExistsNameOther(alert.提醒项目, alert.ID))
                {
                    if (MessageBox.Show("系统中已经存在该提醒，确定还要继续保存么？", "重名提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.OK)
                    {
                        if (al.UpdateAlert(alert))
                        {
                            LoadAlerts();
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
                    if (al.UpdateAlert(alert))
                    {
                        LoadAlerts();
                        MessageBox.Show("修改成功！");
                    }
                }
            }
            else
            {
                MessageBox.Show("先选定要修改的提醒！");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex > -1)
            {
                if (MessageBox.Show("确定要删除该提醒？", "删除提醒", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.OK)
                {
                    Alert alert = new Alert();
                    alert.ID = ((Alert)comboBox1.SelectedItem).ID;
                    if (AlertLogic.GetInstance().DeleteAlert(alert))
                    {
                        LoadAlerts();
                    }
                }
            }
            else
            {
                MessageBox.Show("先选定要删除的提醒！");
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            DataTable dt = Search(textBox8.Text.Trim(), textBox9.Text.Trim(), comboBox2.SelectedItem as AlertType);
            dataGridView1.DataSource = dt;
        }

        private DataTable Search(string name = null, string subject = null, AlertType alertType = null)
        {
            string nm = "";
            if (!string.IsNullOrEmpty(name) && name.Trim() != "")
            {
                nm = " and 提醒项目 like '%" + name + "%'";
            }
            string sb = "";
            if (!string.IsNullOrEmpty(subject) && subject.Trim() != "")
            {
                sb = " and 提醒对象 like '%" + subject + "%'";
            }
            string at = "";
            if (alertType != null)
            {
                at = " and 提醒方式=" + alertType.ID;
            }
            string where = "(1=1)" + nm + sb + at + " order by ID desc";
            return AlertLogic.GetInstance().GetAlerts(where);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            DataTable dt = dataGridView1.DataSource as DataTable;
            if (dt != null)
            {
                dt.TableName = "提醒信息";
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
                Alert alert = comboBox1.SelectedItem as Alert;
                if (alert != null)
                {
                    textBox1.Text = alert.提醒项目;
                    dateTimePicker1.Value = alert.提醒时间;
                    comboBox3.SelectedIndex = GetIndexByAlertType(alert.提醒方式, comboBox3);
                    textBox2.Text = alert.提醒对象;
                    textBox3.Text = alert.备注;
                }
            }
        }

        private int GetIndexByAlertType(AlertType alertType, ComboBox comboBox3)
        {
            if (alertType != null && comboBox3 != null)
            {
                for (int i = 0; i < comboBox3.Items.Count; i++)
                {
                    AlertType at = comboBox3.Items[i] as AlertType;
                    if (at != null && alertType.ID == at.ID)
                        return i;
                }
            }
            return -1;
        }
    }
}
