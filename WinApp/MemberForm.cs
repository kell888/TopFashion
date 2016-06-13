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
    public partial class MemberForm : Form
    {
        public MemberForm(int selectIndex = 0)
        {
            InitializeComponent();
            this.tabControl1.SelectedIndex = selectIndex;
        }

        private void MemberForm_Load(object sender, EventArgs e)
        {
            LoadMembers();
            LoadCardTypes();
            comboBox2.SelectedIndex = 0;
            comboBox4.SelectedIndex = 0;
        }

        private void LoadMembers()
        {
            List<Member> elements = MemberLogic.GetInstance().GetAllMembers();
            comboBox1.Items.Clear();
            foreach (Member element in elements)
            {
                comboBox1.Items.Add(element);
            }
        }

        private void LoadCardTypes()
        {
            List<CardType> elements = CardTypeLogic.GetInstance().GetAllCardTypes();
            comboBox3.Items.Clear();
            comboBox5.Items.Clear();
            comboBox5.Items.Add("--不限--");
            foreach (CardType element in elements)
            {
                comboBox3.Items.Add(element);
                comboBox5.Items.Add(element);
            }
            if (comboBox3.Items.Count > 0)
                comboBox3.SelectedIndex = 0;
            comboBox5.SelectedIndex = 0;
        }

        private void button1_Click(object sender, EventArgs e)
        {
                Member member = new Member();
            member.姓名 = textBox6.Text.Trim();
            member.性别 = (性别)Enum.ToObject(typeof(性别), comboBox2.SelectedIndex);
            member.卡种 = comboBox3.SelectedItem as CardType;
            member.卡号 = textBox6.Text.Trim();
            member.到期日 = DateTime.Parse(textBox3.Text.Trim());
            member.生日 = DateTime.Parse(textBox4.Text.Trim());
            member.电话 = textBox5.Text.Trim();
            member.住址 = textBox6.Text.Trim();
            MemberLogic ml = MemberLogic.GetInstance();
            if (ml.ExistsName(member.姓名))
            {
                if (MessageBox.Show("系统中已经存在该会员，确定还要继续保存么？", "重名提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.OK)
                {
                    int id = ml.AddMember(member);
                    if (id > 0)
                    {
                        member.ID = id;
                        LoadMembers();
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
                int id = ml.AddMember(member);
                if (id > 0)
                {
                    member.ID = id;
                    LoadMembers();
                    MessageBox.Show("添加成功！");
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex > -1)
            {
                Member member = new Member();
                member.ID = ((Product)comboBox1.SelectedItem).ID;
                member.姓名 = textBox1.Text.Trim();
                member.性别 = (性别)Enum.ToObject(typeof(性别), comboBox2.SelectedIndex);
                member.卡种 = comboBox3.SelectedItem as CardType;
                member.卡号 = textBox2.Text.Trim();
                member.到期日 = DateTime.Parse(textBox3.Text.Trim());
                member.生日 = DateTime.Parse(textBox4.Text.Trim());
                member.电话 = textBox5.Text.Trim();
                member.住址 = textBox6.Text.Trim();
                MemberLogic ml = MemberLogic.GetInstance();
                if (ml.ExistsNameOther(member.姓名, member.ID))
                {
                    if (MessageBox.Show("系统中已经存在该会员，确定还要继续保存么？", "重名提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.OK)
                    {
                        if (ml.UpdateMember(member))
                        {
                            LoadMembers();
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
                    if (ml.UpdateMember(member))
                    {
                        LoadMembers();
                        MessageBox.Show("修改成功！");
                    }
                }
            }
            else
            {
                MessageBox.Show("先选定要修改的会员！");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex > -1)
            {
                if (MessageBox.Show("确定要删除该会员？", "删除提醒", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.OK)
                {
                    Member member = new Member();
                    member.ID = ((Member)comboBox1.SelectedItem).ID;
                    if (MemberLogic.GetInstance().DeleteMember(member))
                    {
                        LoadMembers();
                    }
                }
            }
            else
            {
                MessageBox.Show("先选定要删除的会员！");
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            DataTable dt = Search(textBox8.Text.Trim(), comboBox4.SelectedIndex, comboBox5.SelectedItem as CardType, textBox9.Text.Trim(), textBox7.Text.Trim());
            dataGridView1.DataSource = dt;
        }

        private DataTable Search(string name, int sex = 0, CardType cardType = null, string cardNo = null, string mobile = null)
        {
            string nm = "";
            if (!string.IsNullOrEmpty(name))
            {
                nm = " and 姓名 like '%" + name + "%'";
            }
            string sx = "";
            if (sex > 0)
            {
                sx = " and 性别=" + sex;
            }
            string ct = "";
            if (cardType != null)
            {
                ct = " and 卡种=" + cardType.ID;
            }
            string cn = "";
            if (!string.IsNullOrEmpty(cardNo) && cardNo.Trim() != "")
            {
                cn = " and 卡号 like '%" + cardNo.Trim() + "%'";
            }
            string mb = "";
            if (!string.IsNullOrEmpty(mb) && mb.Trim() != "")
            {
                mb = " and 电话 like '%" + mb.Trim() + "%'";
            }
            string where = "(1=1)" + nm + sx + ct + cn + mb + " order by ID desc";
            return MemberLogic.GetInstance().GetMembers(where);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            DataTable dt = dataGridView1.DataSource as DataTable;
            if (dt != null)
            {
                dt.TableName = "会员信息";
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
                Member member = comboBox1.SelectedItem as Member;
                if (member != null)
                {
                    textBox1.Text = member.姓名;
                    comboBox2.SelectedIndex = (int)member.性别;
                    comboBox3.SelectedIndex = GetIndexByCardType(member.卡种, comboBox3);
                    textBox2.Text = member.卡号;
                    textBox3.Text = member.到期日.ToString("yyyy-MM-dd");
                    monthCalendar1.SelectionStart = member.到期日;
                    textBox4.Text = member.生日.ToString("yyyy-MM-dd");
                    monthCalendar2.SelectionStart = member.生日;
                    textBox5.Text = member.电话;
                    textBox6.Text = member.住址;
                }
            }
        }

        private int GetIndexByCardType(CardType cardType, ComboBox comboBox3)
        {
            if (cardType != null && comboBox3 != null)
            {
                for (int i = 0; i < comboBox3.Items.Count; i++)
                {
                    CardType ct = comboBox3.Items[i] as CardType;
                    if (ct != null && ct.ID == cardType.ID)
                        return i;
                }
            }
            return -1;
        }

        private void textBox3_MouseClick(object sender, MouseEventArgs e)
        {
            monthCalendar1.Show();
        }

        private void monthCalendar1_DateSelected(object sender, DateRangeEventArgs e)
        {
            textBox3.Text = e.Start.ToString("yyyy-MM-dd");
        }

        private void textBox4_MouseClick(object sender, MouseEventArgs e)
        {
            monthCalendar2.Show();
        }

        private void monthCalendar2_DateSelected(object sender, DateRangeEventArgs e)
        {
            textBox4.Text = e.Start.ToString("yyyy-MM-dd");
        }
    }
}
