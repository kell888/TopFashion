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
    public partial class OutcomeForm : PermissionForm
    {
        public OutcomeForm(User user, int selectIndex = 0)
        {
            this.User = user;
            InitializeComponent();
            this.tabControl1.SelectedIndex = selectIndex;
            this.selectIndex = selectIndex;
            this.tabControl1.SelectedIndexChanged += new EventHandler(tabControl1_SelectedIndexChanged);
            if (this.User.ID == this.AdminId)
                textBox3.ReadOnly = textBox6.ReadOnly = false;
        }
        int selectIndex;

        void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.User.ID != this.AdminId && !this.User.GetAllPermissions().ContainsControl("OutcomeForm", "行政", "tabControl1"))
                this.tabControl1.SelectedIndex = selectIndex;
        }

        private void OutcomeForm_Load(object sender, EventArgs e)
        {
            base.DisableUserPermission(this);
            LoadProducts();
            LoadPropertys();
            textBox3.Text = textBox6.Text = this.User.Username;
        }

        private void LoadProducts()
        {
            List<Product> ps = ProductLogic.GetInstance().GetAllProducts();
            comboBox1.Items.Clear();
            foreach (Product p in ps)
            {
                comboBox1.Items.Add(p);
            }
        }

        private void LoadPropertys()
        {
            List<Property> ps = PropertyLogic.GetInstance().GetAllPropertys();
            comboBox2.Items.Clear();
            foreach (Property p in ps)
            {
                comboBox2.Items.Add(p);
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex > -1)
            {
                Product element = comboBox1.SelectedItem as Product;
                if (element != null)
                {
                    label7.Text = element.单位;
                    textBox2.Text = element.售价.ToString();
                }
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox2.SelectedIndex > -1)
            {
                Property element = comboBox2.SelectedItem as Property;
                if (element != null)
                {
                    label8.Text = element.单位;
                    textBox7.Text = element.价格.ToString();
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == -1)
            {
                MessageBox.Show("请先选择要出库的产品！");
                comboBox1.Focus();
                return;
            }
            int num = 0;
            int R;
            if (int.TryParse(textBox1.Text.Trim(), out R))
            {
                num = R;
            }
            else
            {
                MessageBox.Show("数量必须为整数！");
                textBox1.Focus();
                textBox1.SelectAll();
                return;
            }
            decimal price = 0;
            decimal r;
            if (decimal.TryParse(textBox2.Text.Trim(), out r))
            {
                price = r;
            }
            else
            {
                MessageBox.Show("实价必须为整数！");
                textBox2.Focus();
                textBox2.SelectAll();
                return;
            }
            if (checkBox1.Checked)
            {
                if (selectMemberControl1.SelectedMembers.Count == 0)
                {
                    MessageBox.Show("请选择当前消费的会员！");
                    selectMemberControl1.Focus();
                    return;
                }
            }
            Income element = new Income();
            element.PID = ((Product)comboBox1.SelectedItem).ID;
            element.IsProduct = true;
            element.IsIncome = false;
            element.数量 = num;
            element.实价 = price;
            element.经手人 = textBox3.Text.Trim();
            element.备注 = textBox4.Text.Trim();
            if (IncomeLogic.GetInstance().AddIncome(element) > 0)
            {
                MessageBox.Show("登记成功！");
                if (checkBox1.Checked)
                {
                    Member member = selectMemberControl1.SelectedMembers[0];
                    decimal sum = num * price;
                    if (sum > 0)
                    {
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
                            mml.AddMemberMoney(mm);
                        }
                        else
                        {
                            mm = mml.GetMemberMoney(name, mobile);
                        }
                        if (mm != null)
                        {
                            MoneyRecord mr = new MoneyRecord();
                            mr.会员账户 = mm;
                            mr.发生金额 = sum;
                            mr.是否充值 = false;
                            mr.操作人 = element.经手人;
                            if (MoneyRecordLogic.GetInstance().AddMoneyRecord(mr) > 0)
                            {
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
                    else
                    {
                        MessageBox.Show("消费额不能为负！");
                    }
                }
            }
            else
            {
                MessageBox.Show("登记失败！");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (comboBox2.SelectedIndex == -1)
            {
                MessageBox.Show("请先选择要出库的资产！");
                comboBox2.Focus();
                return;
            }
            int num = 0;
            int R;
            if (int.TryParse(textBox8.Text.Trim(), out R))
            {
                num = R;
            }
            else
            {
                MessageBox.Show("数量必须为整数！");
                textBox8.Focus();
                textBox8.SelectAll();
                return;
            }
            decimal price = 0;
            decimal r;
            if (decimal.TryParse(textBox7.Text.Trim(), out r))
            {
                price = r;
            }
            else
            {
                MessageBox.Show("实价必须为整数！");
                textBox7.Focus();
                textBox7.SelectAll();
                return;
            }
            Income element = new Income();
            element.PID = ((Property)comboBox2.SelectedItem).ID;
            element.IsProduct = false;
            element.IsIncome = false;
            element.数量 = num;
            element.实价 = price;
            element.经手人 = textBox6.Text.Trim();
            element.备注 = textBox5.Text.Trim();
            if (IncomeLogic.GetInstance().AddIncome(element) > 0)
                MessageBox.Show("登记成功！");
            else
                MessageBox.Show("登记失败！");
        }

        private void SumProduct(string num, string price)
        {
            decimal nm, pri;
            if (decimal.TryParse(num, out nm) && decimal.TryParse(price, out pri))
            {
                decimal sum = nm * pri;
                产品出库Button.Text = "出库(" + sum + "元)";
            }
        }

        private void SumProperty(string num, string price)
        {
            decimal nm, pri;
            if (decimal.TryParse(num, out nm) && decimal.TryParse(price, out pri))
            {
                decimal sum = nm * pri;
                资产出库Button.Text = "出库(" + sum + "元)";
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            selectMemberControl1.Enabled = checkBox1.Checked;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            SumProduct(textBox1.Text.Trim(), textBox2.Text.Trim());
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            SumProduct(textBox1.Text.Trim(), textBox2.Text.Trim());
        }

        private void textBox8_TextChanged(object sender, EventArgs e)
        {
            SumProperty(textBox8.Text.Trim(), textBox7.Text.Trim());
        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {
            SumProperty(textBox8.Text.Trim(), textBox7.Text.Trim());
        }
    }
}
