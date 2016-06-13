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
    public partial class MemberMoneyForm : PermissionForm
    {
        public MemberMoneyForm(User user)
        {
            this.User = user;
            InitializeComponent();
        }

        private void MemberMoneyForm_Load(object sender, EventArgs e)
        {
            base.DisableUserPermission(this);
            LoadAllMemberMoneys();
        }

        private void LoadAllMemberMoneys()
        {
            DataTable dt = MemberMoneyLogic.GetInstance().GetMemberMoneysBy(string.Empty);
            dataGridView1.DataSource = dt;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DataTable dt = Search(textBox1.Text.Trim(), textBox2.Text.Trim(), numericUpDown1.Value);
            dataGridView1.DataSource = dt;
        }

        private DataTable Search(string name, string mobile, decimal lessThan)
        {
            return MemberMoneyLogic.GetInstance().GetMemberMoneysBy(name, mobile, lessThan);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DataTable dt = dataGridView1.DataSource as DataTable;
            if (dt != null)
            {
                dt.TableName = "会员账户信息";
                KellPrinter.DataReporter printer = new KellPrinter.DataReporter(dt);
                PrintForm pf = new PrintForm(this.User, printer);
                pf.Show();
            }
            else
            {
                MessageBox.Show("当前没有数据可供打印导出！");
            }
        }
    }
}
