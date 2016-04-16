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
    public partial class RenewForm : Form
    {
        public RenewForm(int selectIndex = 0)
        {
            InitializeComponent();
            this.tabControl1.SelectedIndex = selectIndex;
            this.selectStaffControl1.SelectOnlyOne = true;
        }

        private void RenewForm_Load(object sender, EventArgs e)
        {
            LoadRenews();
            LoadCardTypes();
            comboBox2.SelectedIndex = 0;
            comboBox4.SelectedIndex = 0;
        }

        private void LoadRenews()
        {
            List<Renew> elements = RenewLogic.GetInstance().GetAllRenews();
            comboBox1.Items.Clear();
            foreach (Renew element in elements)
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
            Renew renew = new Renew();
            renew.Member = comboBox2.SelectedItem as Member;
            renew.卡种 = renew.Member.卡种;
            renew.卡号 = textBox2.Text.Trim();
            renew.续卡时间 = DateTime.Parse(textBox3.Text.Trim());
            renew.经手人 = (selectStaffControl1.SelectedStaffs != null && selectStaffControl1.SelectedStaffs.Count > 0) ? selectStaffControl1.SelectedStaffs[0] : null;
            renew.备注 = textBox6.Text;
            RenewLogic rl = RenewLogic.GetInstance();
            int id = rl.AddRenew(renew);
            if (id > 0)
            {
                renew.ID = id;
                LoadRenews();
                MessageBox.Show("添加成功！");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex > -1)
            {
                Renew renew = new Renew();
                renew.ID = ((Product)comboBox1.SelectedItem).ID;
                renew.Member = comboBox2.SelectedItem as Member;
                renew.卡种 = renew.Member.卡种;
                renew.卡号 = textBox2.Text.Trim();
                renew.续卡时间 = DateTime.Parse(textBox3.Text.Trim());
                renew.经手人 = (selectStaffControl1.SelectedStaffs != null && selectStaffControl1.SelectedStaffs.Count > 0) ? selectStaffControl1.SelectedStaffs[0] : null;
                renew.备注 = textBox6.Text;
                RenewLogic rl = RenewLogic.GetInstance();
                if (rl.UpdateRenew(renew))
                {
                    LoadRenews();
                    MessageBox.Show("修改成功！");
                }
            }
            else
            {
                MessageBox.Show("先选定要修改的续卡记录！");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex > -1)
            {
                if (MessageBox.Show("确定要删除该续卡记录？", "删除提醒", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.OK)
                {
                    Renew renew = new Renew();
                    renew.ID = ((Renew)comboBox1.SelectedItem).ID;
                    if (RenewLogic.GetInstance().DeleteRenew(renew))
                    {
                        LoadRenews();
                    }
                }
            }
            else
            {
                MessageBox.Show("先选定要删除的续卡记录！");
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
            if (!string.IsNullOrEmpty(name) && name.Trim() != "")
            {
                nm = " and TF_Member.姓名 like '%" + name + "%'";
            }
            string sx = "";
            if (sex > 0)
            {
                sx = " and TF_Member.性别=" + sex;
            }
            string ct = "";
            if (cardType != null)
            {
                ct = " and TF_Member.卡种=" + cardType.ID;
            }
            string cn = "";
            if (!string.IsNullOrEmpty(cardNo) && cardNo.Trim() != "")
            {
                cn = " and TF_Member.卡号 like '%" + cardNo.Trim() + "%'";
            }
            string mb = "";
            if (!string.IsNullOrEmpty(mobile) && mobile.Trim() != "")
            {
                mb = " and TF_Member.电话 like '%" + mobile.Trim() + "%'";
            }
            string where = nm + sx + ct + cn + mb + " order by TF_Renew.ID desc";
            return RenewLogic.GetInstance().GetRenews(where);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            DataTable dt = dataGridView1.DataSource as DataTable;
            if (dt != null)
            {
                dt.TableName = "续卡记录信息";
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
                Renew renew = comboBox1.SelectedItem as Renew;
                if (renew != null)
                {
                    comboBox2.SelectedIndex = GetIndexByMember(renew.Member, comboBox2);
                    comboBox3.SelectedIndex = GetIndexByCardType(renew.卡种, comboBox3);
                    textBox2.Text = renew.卡号;
                    textBox3.Text = renew.续卡时间.ToString("yyyy-MM-dd");
                    monthCalendar1.SelectionStart = renew.续卡时间;
                    selectStaffControl1.SelectedStaffs = new List<Staff>(){renew.经手人};
                    textBox6.Text = renew.备注;
                }
            }
        }

        private int GetIndexByMember(Member member, ComboBox comboBox2)
        {
            if (member != null && comboBox2 != null)
            {
                for (int i = 0; i < comboBox2.Items.Count; i++)
                {
                    Member mb = comboBox2.Items[i] as Member;
                    if (mb != null && mb.ID == member.ID)
                        return i;
                }
            }
            return -1;
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
    }
}
