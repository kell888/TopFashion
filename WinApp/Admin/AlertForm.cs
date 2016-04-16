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
    public partial class AlertForm : PermissionForm
    {
        public AlertForm(User user, int selectIndex = 0)
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
            if (this.User.ID != this.AdminId && !this.User.GetAllPermissions().ContainsControl("AlertForm", "行政", "tabControl1"))
                this.tabControl1.SelectedIndex = selectIndex;
        }

        private void AlertForm_Load(object sender, EventArgs e)
        {
            base.DisableUserPermission(this);
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
            dataGridView1.DataSource = AlertLogic.GetInstance().GetAlerts(string.Empty);
        }

        private void LoadAlertTypes()
        {
            //List<AlertType> elements = AlertTypeLogic.GetInstance().GetAllAlertTypes();
            comboBox3.Items.Clear();
            comboBox2.Items.Clear();
            comboBox2.Items.Add("--不限--");
            提醒方式[] elements = (提醒方式[])Enum.GetValues(typeof(提醒方式));
            foreach (提醒方式 element in elements)
            {
                comboBox3.Items.Add(element);
                comboBox2.Items.Add(element);
            }
            comboBox2.SelectedIndex = 0;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Alert alert = new Alert();
            alert.提醒项目 = textBox1.Text.Trim();
            alert.提醒方式 = (提醒方式)comboBox3.SelectedItem;
            if (alert.提醒方式 == 提醒方式.系统提示 || alert.提醒方式 == 提醒方式.员工短信)
            {
                alert.提醒对象 = Commons.GetStaffIdStr(selectStaffControl1.SelectedStaffs);
            }
            else if (alert.提醒方式 == 提醒方式.会员短信)
            {
                alert.提醒对象 = Commons.GetMemberIdStr(selectMemberControl1.SelectedMembers);
            }
            alert.提醒时间 = dateTimePicker1.Value;
            alert.Flag = checkBox1.Checked ? 1 : 0;
            alert.备注 = textBox3.Text;
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
                Alert alert = (Alert)comboBox1.SelectedItem;
                alert.提醒项目 = textBox1.Text.Trim();
                alert.提醒方式 = (提醒方式)comboBox3.SelectedItem;
                if (alert.提醒方式 == 提醒方式.系统提示 || alert.提醒方式 == 提醒方式.员工短信)
                {
                    alert.提醒对象 = Commons.GetStaffIdStr(selectStaffControl1.SelectedStaffs);
                }
                else if (alert.提醒方式 == 提醒方式.会员短信)
                {
                    alert.提醒对象 = Commons.GetMemberIdStr(selectMemberControl1.SelectedMembers);
                }
                alert.提醒时间 = dateTimePicker1.Value;
                alert.Flag = checkBox1.Checked ? 1 : 0;
                alert.备注 = textBox3.Text;
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
                    Alert alert = (Alert)comboBox1.SelectedItem;
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
            DataTable dt = Search(textBox8.Text.Trim(), comboBox2.SelectedItem, textBox9.Text.Trim());
            dataGridView1.DataSource = dt;
        }

        private DataTable Search(string name, object alertType, string subject)
        {
            string nm = "";
            if (!string.IsNullOrEmpty(name) && name.Trim() != "")
            {
                nm = " and 提醒项目 like '%" + name.Trim() + "%'";
            }
            string at = "";
            string sb = "";
            if (alertType is 提醒方式)
            {
                提醒方式 type = (提醒方式)alertType;
                at = " and 提醒方式='" + type.ToString();
                if (!string.IsNullOrEmpty(subject) && subject.Trim() != "")
                {
                    int id = 0;
                    if (type == 提醒方式.系统提示 || type == 提醒方式.员工短信)
                    {
                        Staff staff = StaffLogic.GetInstance().GetStaffByName(subject.Trim());
                        if (staff != null)
                        {
                            id = staff.ID;
                        }
                    }
                    else if (type == 提醒方式.会员短信)
                    {
                        Member member = MemberLogic.GetInstance().GetMemberByName(subject.Trim());
                        if (member != null)
                        {
                            id = member.ID;
                        }
                    }
                    sb = " and ','+提醒对象+',' like '%," + id + ",%'";
                }
            }
            string where = "(1=1)" + nm + sb + at;
            return AlertLogic.GetInstance().GetAlerts(where);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            DataTable dt = dataGridView1.DataSource as DataTable;
            if (dt != null)
            {
                dt.TableName = "提醒信息";
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
                Alert alert = comboBox1.SelectedItem as Alert;
                if (alert != null)
                {
                    textBox1.Text = alert.提醒项目;
                    dateTimePicker1.Value = alert.提醒时间;
                    comboBox3.SelectedIndex = GetIndexByAlertType(alert.提醒方式, comboBox3);
                    提醒方式 type = (提醒方式)comboBox3.SelectedItem;
                    if (type == 提醒方式.系统提示 || type == 提醒方式.员工短信)
                    {
                        selectStaffControl1.SelectedStaffs = Commons.GetStaffByIdStr(alert.提醒对象);
                    }
                    else if (type == 提醒方式.会员短信)
                    {
                        selectMemberControl1.SelectedMembers = Commons.GetMemberByIdStr(alert.提醒对象);
                    }
                    checkBox1.Checked = alert.Flag == 1;
                    textBox3.Text = alert.备注;
                }
            }
        }

        private int GetIndexByAlertType(提醒方式 alertType, ComboBox comboBox3)
        {
            if (comboBox3 != null)
            {
                for (int i = 0; i < comboBox3.Items.Count; i++)
                {
                    提醒方式 at = (提醒方式)comboBox3.Items[i];
                    if (alertType == at)
                        return i;
                }
            }
            return -1;
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox3.SelectedIndex > -1)
            {
                提醒方式 type = (提醒方式)comboBox3.SelectedItem;
                if (type == 提醒方式.系统提示 || type == 提醒方式.员工短信)
                {
                    selectMemberControl1.SendToBack();
                    selectStaffControl1.BringToFront();
                }
                else if (type == 提醒方式.会员短信)
                {
                    selectStaffControl1.SendToBack();
                    selectMemberControl1.BringToFront();
                }
            }
        }
    }
}
