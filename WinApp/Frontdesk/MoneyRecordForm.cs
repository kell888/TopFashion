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
    public partial class MoneyRecordForm : PermissionForm
    {
        public MoneyRecordForm(User user, int selectIndex = 0)
        {
            this.User = user;
            InitializeComponent();
            this.selectMemberControl1.SelectOnlyOne = true;
            this.tabControl1.SelectedIndex = selectIndex;
            this.selectIndex = selectIndex;
            this.tabControl1.SelectedIndexChanged += new EventHandler(tabControl1_SelectedIndexChanged);
            if (this.User.ID == this.AdminId)
            {
                修改Button.Enabled = 删除Button.Enabled = comboBox1.Enabled = true;
                textBox3.ReadOnly = false;
            }
        }
        int selectIndex;

        void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.User.ID != this.AdminId && !this.User.GetAllPermissions().ContainsControl("MoneyRecordForm", "前台", "tabControl1"))
                this.tabControl1.SelectedIndex = selectIndex;
        }

        private void MoneyRecordForm_Load(object sender, EventArgs e)
        {
            base.DisableUserPermission(this);
            LoadMoneyRecords();
            dataGridView1.DataSource = MoneyRecordLogic.GetInstance().GetMoneyRecords(string.Empty);
            textBox3.Text = this.User.Username;
        }

        private void LoadMoneyRecords()
        {
            List<MoneyRecord> elements = MoneyRecordLogic.GetInstance().GetAllMoneyRecords();
            comboBox1.Items.Clear();
            foreach (MoneyRecord element in elements)
            {
                comboBox1.Items.Add(element);
            }
            dataGridView1.DataSource = MoneyRecordLogic.GetInstance().GetMoneyRecords(string.Empty);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (selectMemberControl1.SelectedMembers.Count == 0)
            {
                MessageBox.Show("请先指定一个会员！");
                selectMemberControl1.Focus();
                return;
            }
            Member member = selectMemberControl1.SelectedMembers[0];
            MemberMoneyLogic mml = MemberMoneyLogic.GetInstance();
            string name = member.姓名;
            string mobile = member.电话;
            MemberMoney mm = null;
            if (!mml.ExistsName(name, mobile))
            {
                mm = new MemberMoney();
                mm.会员姓名 = name;
                mm.会员电话 = mobile;
                mm.备注 = "账户创建于" + DateTime.Now.ToString();
                int id = mml.AddMemberMoney(mm);
                if (id > 0)
                    mm.ID = id;
            }
            else
            {
                mm = mml.GetMemberMoney(name, mobile);
            }
            if (mm != null)
            {
                MoneyRecord mr = new MoneyRecord();
                mr.会员账户 = mm;
                mr.发生金额 = numericUpDown1.Value;
                mr.是否充值 = true;
                mr.操作人 = textBox3.Text;
                if (MoneyRecordLogic.GetInstance().AddMoneyRecord(mr) > 0)
                {
                    LoadMoneyRecords();
                    MessageBox.Show("保存会员消费记录以及扣款成功！");
                }
                else
                {
                    MessageBox.Show("保存会员消费记录失败或者扣款失败！");
                }
            }
            else
            {
                MessageBox.Show("无法创建会员账户！");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex > -1)
            {
                if (selectMemberControl1.SelectedMembers.Count == 0)
                {
                    MessageBox.Show("请先指定一个会员！");
                    selectMemberControl1.Focus();
                    return;
                }
                MoneyRecord mr = (MoneyRecord)comboBox1.SelectedItem;
                mr.会员账户 = MemberMoneyLogic.GetInstance().GetMemberMoney(selectMemberControl1.SelectedMembers[0]);
                mr.发生金额 = numericUpDown1.Value;
                mr.是否充值 = true;
                mr.操作人 = textBox3.Text;
                MoneyRecordLogic rl = MoneyRecordLogic.GetInstance();
                if (rl.UpdateMoneyRecord(mr))
                {
                    LoadMoneyRecords();
                    MessageBox.Show("修改成功！");
                }
            }
            else
            {
                MessageBox.Show("先选定要修改的进出账！");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex > -1)
            {
                if (MessageBox.Show("确定要删除该进出账？", "删除提醒", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.OK)
                {
                    MoneyRecord record = (MoneyRecord)comboBox1.SelectedItem;
                    if (MoneyRecordLogic.GetInstance().DeleteMoneyRecord(record))
                    {
                        LoadMoneyRecords();
                    }
                }
            }
            else
            {
                MessageBox.Show("先选定要删除的进出账！");
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            DataTable dt = Search(textBox8.Text.Trim(), textBox7.Text.Trim(), textBox1.Text.Trim(), comboBox2.SelectedIndex);
            dataGridView1.DataSource = dt;
        }

        private DataTable Search(string name, string mobile, string operater, int action)
        {
            string nm = "";
            if (!string.IsNullOrEmpty(name) && name.Trim() != "")
            {
                nm = " and 会员姓名 like '%" + name.Trim() + "%'";
            }
            string mb = "";
            if (!string.IsNullOrEmpty(mobile) && mobile.Trim() != "")
            {
                mb = " and 会员电话 like '%" + mobile.Trim() + "%'";
            }
            string czr = "";
            if (!string.IsNullOrEmpty(name) && name.Trim() != "")
            {
                czr = " and 操作人 like '%" + operater.Trim() + "%'";
            }
            string act = "";
            if (action > 0)
            {
                act = " and 动作='" + (action == 1 ? "消费" : "充值") + "'";
            }
            string where = "(1=1)" + nm + mb + czr + act;
            return MoneyRecordLogic.GetInstance().GetMoneyRecords(where);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            DataTable dt = dataGridView1.DataSource as DataTable;
            if (dt != null)
            {
                dt.TableName = "进出账信息";
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
                MoneyRecord record = comboBox1.SelectedItem as MoneyRecord;
                if (record != null)
                {
                    
                    textBox3.Text = record.操作人;
                    selectMemberControl1.SelectedMembers = new List<Member>() { MemberMoneyLogic.GetInstance().GetMember(record.会员账户) };
                }
            }
        }
    }
}
