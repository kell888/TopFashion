using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using KellWorkFlow;

namespace TopFashion
{
    public partial class ChangePwdForm : Form
    {
        public ChangePwdForm(User user)
        {
            InitializeComponent();
            this.label5.Text = user.Username;
            this.user = user;
        }

        User user;

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string oldPwd = textBox1.Text;
            if (!this.user.Password.Equals(oldPwd, StringComparison.InvariantCultureIgnoreCase))
            {
                MessageBox.Show("原密码有误！请重新输入。");
                textBox1.Focus();
                textBox1.SelectAll();
                return;
            }
            string newPwd = textBox2.Text;
            string newPwd2 = textBox3.Text;
            if (newPwd != newPwd2)
            {
                MessageBox.Show("新密码确认有误！请重新确认。");
                return;
            }
            if (UserLogic.GetInstance().ChangePwd(this.user.ID, newPwd))
            {
                this.user.Password = newPwd;
                MessageBox.Show("密码修改成功！");
            }
            else
            {
                MessageBox.Show("密码修改失败！");
            }
            this.Close();
        }
    }
}
