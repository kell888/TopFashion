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
    public partial class PersonalTrainForm : Form
    {
        public PersonalTrainForm(int selectIndex = 0)
        {
            InitializeComponent();
            this.tabControl1.SelectedIndex = selectIndex;
            this.selectStaffControl1.SelectOnlyOne = true;
        }

        private void PersonalTrainForm_Load(object sender, EventArgs e)
        {
            LoadPersonalTrains();
            comboBox2.SelectedIndex = 0;
            comboBox4.SelectedIndex = 0;
        }

        private void LoadPersonalTrains()
        {
            List<PersonalTrain> elements = PersonalTrainLogic.GetInstance().GetAllPersonalTrains();
            comboBox1.Items.Clear();
            foreach (PersonalTrain element in elements)
            {
                comboBox1.Items.Add(element);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            PersonalTrain personalTrain = new PersonalTrain();
            personalTrain.Member = comboBox2.SelectedItem as Member;
            personalTrain.私教项目 = textBox1.Text.Trim();
            personalTrain.次数 = (int)numericUpDown1.Value;
            personalTrain.开始日期 = DateTime.Parse(textBox3.Text.Trim());
            personalTrain.结束日期 = DateTime.Parse(textBox4.Text.Trim());
            personalTrain.教练 = (selectStaffControl1.SelectedStaffs != null && selectStaffControl1.SelectedStaffs.Count > 0) ? selectStaffControl1.SelectedStaffs[0] : null;
            personalTrain.备注 = textBox6.Text;
            PersonalTrainLogic rl = PersonalTrainLogic.GetInstance();
            int id = rl.AddPersonalTrain(personalTrain);
            if (id > 0)
            {
                personalTrain.ID = id;
                LoadPersonalTrains();
                MessageBox.Show("添加成功！");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex > -1)
            {
                PersonalTrain personalTrain = new PersonalTrain();
                personalTrain.ID = ((Product)comboBox1.SelectedItem).ID;
                personalTrain.Member = comboBox2.SelectedItem as Member;
                personalTrain.私教项目 = textBox1.Text.Trim();
                personalTrain.次数 = (int)numericUpDown1.Value;
                personalTrain.开始日期 = DateTime.Parse(textBox3.Text.Trim());
                personalTrain.结束日期 = DateTime.Parse(textBox4.Text.Trim());
                personalTrain.教练 = (selectStaffControl1.SelectedStaffs != null && selectStaffControl1.SelectedStaffs.Count > 0) ? selectStaffControl1.SelectedStaffs[0] : null;
                personalTrain.备注 = textBox6.Text;
                PersonalTrainLogic rl = PersonalTrainLogic.GetInstance();
                if (rl.UpdatePersonalTrain(personalTrain))
                {
                    LoadPersonalTrains();
                    MessageBox.Show("修改成功！");
                }
            }
            else
            {
                MessageBox.Show("先选定要修改的私教！");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex > -1)
            {
                if (MessageBox.Show("确定要删除该私教？", "删除提醒", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.OK)
                {
                    PersonalTrain personalTrain = new PersonalTrain();
                    personalTrain.ID = ((PersonalTrain)comboBox1.SelectedItem).ID;
                    if (PersonalTrainLogic.GetInstance().DeletePersonalTrain(personalTrain))
                    {
                        LoadPersonalTrains();
                    }
                }
            }
            else
            {
                MessageBox.Show("先选定要删除的私教！");
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            DataTable dt = Search(textBox8.Text.Trim(), comboBox4.SelectedIndex, textBox9.Text.Trim(), (selectStaffControl1.SelectedStaffs != null && selectStaffControl1.SelectedStaffs.Count > 0) ? selectStaffControl1.SelectedStaffs[0] : null);
            dataGridView1.DataSource = dt;
        }

        private DataTable Search(string name, int sex = 0, string personalTr = null, Staff trainer = null)
        {
            string nm = "";
            if (!string.IsNullOrEmpty(name))
            {
                nm = " and TF_Member.姓名 like '%" + name + "%'";
            }
            string sx = "";
            if (sex > 0)
            {
                sx = " and TF_Member.性别=" + sex;
            }
            string cn = "";
            if (!string.IsNullOrEmpty(personalTr) && personalTr.Trim() != "")
            {
                cn = " and TF_PersonalTrain.私教项目 like '%" + personalTr.Trim() + "%'";
            }
            string mb = "";
            if (trainer != null)
            {
                mb = " and TF_PersonalTrain.教练=" + trainer.ID;
            }
            string where = nm + sx + cn + mb + " order by TF_PersonalTrain.ID desc";
            return PersonalTrainLogic.GetInstance().GetPersonalTrains(where);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            DataTable dt = dataGridView1.DataSource as DataTable;
            if (dt != null)
            {
                dt.TableName = "私教信息";
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
                PersonalTrain personalTrain = comboBox1.SelectedItem as PersonalTrain;
                if (personalTrain != null)
                {
                    comboBox2.SelectedIndex = GetIndexByMember(personalTrain.Member, comboBox2);
                    textBox1.Text = personalTrain.私教项目;
                    numericUpDown1.Value = personalTrain.次数;
                    textBox3.Text = personalTrain.开始日期.ToString("yyyy-MM-dd");
                    textBox4.Text = personalTrain.结束日期.ToString("yyyy-MM-dd");
                    monthCalendar1.SelectionStart = personalTrain.开始日期;
                    monthCalendar2.SelectionStart = personalTrain.结束日期;
                    selectStaffControl1.SelectedStaffs = new List<Staff>(){personalTrain.教练};
                    textBox6.Text = personalTrain.备注;
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
