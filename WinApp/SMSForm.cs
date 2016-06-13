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
    public partial class SMSForm : PermissionForm
    {
        public SMSForm(User user)
        {
            this.User = user;
            InitializeComponent();
        }

        List<string> mobiles;

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            selectStaffControl1.Enabled = radioButton2.Checked;
            selectMemberControl1.Enabled = !radioButton2.Checked;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            List<string> mobile = new List<string>();
            List<string> greet = new List<string>();
            if (radioButton1.Checked)
            {
                List<Member> members = selectMemberControl1.SelectedMembers;
                foreach (Member m in members)
                {
                    mobile.Add(m.电话);
                    greet.Add(m.姓名);
                }
            }
            else
            {
                List<Staff> staffs = selectStaffControl1.SelectedStaffs;
                foreach (Staff s in staffs)
                {
                    mobile.Add(s.电话);
                    greet.Add(s.姓名);
                }
            }
            Commons.SendSMS(textBox3.Text, mobile, greet);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                StringBuilder sb = new StringBuilder();
                List<Member> members = selectMemberControl1.SelectedMembers;
                foreach (Member m in members)
                {
                    if (sb.Length == 0)
                        sb.Append(m.ID);
                    else
                        sb.Append("," + m.ID);
                }
                Alert alert = new Alert();
                alert.提醒对象 = sb.ToString();
                alert.提醒方式 = 提醒方式.会员短信;
                alert.提醒时间 = dateTimePicker2.Value;
                alert.提醒项目 = textBox3.Text;
                AlertLogic.GetInstance().AddAlert(alert);
            }
            else
            {
                StringBuilder sb = new StringBuilder();
                List<Staff> staffs = selectStaffControl1.SelectedStaffs;
                foreach (Staff s in staffs)
                {
                    if (sb.Length == 0)
                        sb.Append(s.ID);
                    else
                        sb.Append("," + s.ID);
                }
                Alert alert = new Alert();
                alert.提醒对象 = sb.ToString();
                alert.提醒方式 = 提醒方式.员工短信;
                alert.提醒时间 = dateTimePicker2.Value;
                alert.提醒项目 = textBox3.Text;
                AlertLogic.GetInstance().AddAlert(alert);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            openFileDialog1.Title = "选择导入号码的Excel文件...";
            openFileDialog1.Filter = "Excel2003及以前(*.xls)|*.xls|Excel2007及以后(*.xlsx)|*.xlsx";
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                mobiles = Commons.GetMobilesFromExcel(openFileDialog1.FileName);
                button1.Text = "浏览(" + mobiles.Count + ")";
            }
            openFileDialog1.Dispose();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (mobiles != null && mobiles.Count > 0)
                Commons.SendSMS(textBox2.Text, mobiles);
            else
                MessageBox.Show("请先打开Excel文件，或者打开的Excel文件中没有找到任何电话号码！");
        }

        private void SMSForm_Load(object sender, EventArgs e)
        {
            base.DisableUserPermission(this);
        }
    }
}
