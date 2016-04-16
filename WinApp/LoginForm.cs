using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace TopFashion
{
    public partial class LoginForm : Form
    {
        public LoginForm(MainForm owner)
        {
            InitializeComponent();
            this.owner = owner;
        }

        MainForm owner;

        private void button1_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;
            button1.Text = "登录中...";
            button1.Refresh();
            if (owner.Login(textBox1, textBox2))
            {
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
            }
            else
            {
                button1.Enabled = true;
                button1.Text = "登  录";
                button1.Refresh();
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            textBox2.Clear();
        }
    }
}
