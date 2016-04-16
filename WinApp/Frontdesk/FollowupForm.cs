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
    public partial class FollowupForm : PermissionForm
    {
        public FollowupForm(User user, int selectIndex = 0)
        {
            this.User = user;
            InitializeComponent();
            this.selectStaffControl1.SelectOnlyOne = true;
            this.tabControl1.SelectedIndex = selectIndex;
            this.selectIndex = selectIndex;
            this.tabControl1.SelectedIndexChanged += new EventHandler(tabControl1_SelectedIndexChanged);
        }
        int selectIndex;

        void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.User.ID != this.AdminId && !this.User.GetAllPermissions().ContainsControl("FollowupForm", "前台", "tabControl1"))
                this.tabControl1.SelectedIndex = selectIndex;
        }

        private void FollowupForm_Load(object sender, EventArgs e)
        {
            base.DisableUserPermission(this);
            LoadFollowups();
            //LoadMembers();
            LoadFollowupTypes();
            LoadFollowupResults();
        }

        private void LoadMembers()
        {
            List<Member> elements = MemberLogic.GetInstance().GetAllMembers();
            comboBox2.Items.Clear();
            foreach (Member element in elements)
            {
                comboBox2.Items.Add(element);
            }
        }

        private void LoadFollowupTypes()
        {
            List<FollowupType> elements = FollowupTypeLogic.GetInstance().GetAllFollowupTypes();
            comboBox3.Items.Clear();
            comboBox5.Items.Clear();
            comboBox5.Items.Add("--不限--");
            foreach (FollowupType element in elements)
            {
                comboBox3.Items.Add(element);
                comboBox5.Items.Add(element);
            }
            comboBox5.SelectedIndex = 0;
        }

        private void LoadFollowupResults()
        {
            List<FollowupResult> elements = FollowupResultLogic.GetInstance().GetAllFollowupResults();
            comboBox4.Items.Clear();
            comboBox4.Items.Add("--不限--");
            comboBox7.Items.Clear();
            foreach (FollowupResult element in elements)
            {
                comboBox4.Items.Add(element);
                comboBox7.Items.Add(element);
            }
            comboBox4.SelectedIndex = 0;
        }

        private void LoadFollowups()
        {
            List<Followup> elements = FollowupLogic.GetInstance().GetAllFollowups();
            comboBox1.Items.Clear();
            foreach (Followup element in elements)
            {
                comboBox1.Items.Add(element);
            }
            dataGridView1.DataSource = FollowupLogic.GetInstance().GetFollowups(string.Empty);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (selectMemberControl1.SelectedMembers.Count == 0)
            {
                MessageBox.Show("请选择会员！");
                selectMemberControl1.Focus();
                return;
            }
            Followup followup = new Followup();
            followup.Member = selectMemberControl1.SelectedMembers[0];//comboBox2.SelectedItem as Member;
            followup.回访方式 = comboBox3.SelectedItem as FollowupType;
            followup.跟进结果 = comboBox7.SelectedItem as FollowupResult;
            followup.跟进时间 = DateTime.Parse(textBox3.Text.Trim());
            followup.跟进人 = (selectStaffControl1.SelectedStaffs != null && selectStaffControl1.SelectedStaffs.Count > 0) ? selectStaffControl1.SelectedStaffs[0] : null;
            followup.备注 = textBox6.Text;
            FollowupLogic rl = FollowupLogic.GetInstance();
            int id = rl.AddFollowup(followup);
            if (id > 0)
            {
                followup.ID = id;
                LoadFollowups();
                MessageBox.Show("添加成功！");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex > -1)
            {
                if (selectMemberControl1.SelectedMembers.Count == 0)
                {
                    MessageBox.Show("请选择会员！");
                    selectMemberControl1.Focus();
                    return;
                }
                Followup followup = (Followup)comboBox1.SelectedItem;
                followup.Member = selectMemberControl1.SelectedMembers[0];//comboBox2.SelectedItem as Member;
                followup.回访方式 = comboBox3.SelectedItem as FollowupType;
                followup.跟进结果 = comboBox7.SelectedItem as FollowupResult;
                followup.跟进时间 = DateTime.Parse(textBox3.Text.Trim());
                followup.跟进人 = (selectStaffControl1.SelectedStaffs != null && selectStaffControl1.SelectedStaffs.Count > 0) ? selectStaffControl1.SelectedStaffs[0] : null;
                followup.备注 = textBox6.Text;
                FollowupLogic rl = FollowupLogic.GetInstance();
                if (rl.UpdateFollowup(followup))
                {
                    LoadFollowups();
                    MessageBox.Show("修改成功！");
                }
            }
            else
            {
                MessageBox.Show("先选定要修改的回访记录！");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex > -1)
            {
                if (MessageBox.Show("确定要删除该回访记录？", "删除提醒", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.OK)
                {
                    Followup followup = (Followup)comboBox1.SelectedItem;
                    if (FollowupLogic.GetInstance().DeleteFollowup(followup))
                    {
                        LoadFollowups();
                    }
                }
            }
            else
            {
                MessageBox.Show("先选定要删除的回访记录！");
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            DataTable dt = Search(textBox8.Text.Trim(), comboBox6.SelectedIndex, comboBox5.SelectedItem as FollowupType, comboBox4.SelectedItem as FollowupResult, textBox7.Text.Trim());
            dataGridView1.DataSource = dt;
        }

        private DataTable Search(string name, int sex = 0, FollowupType follType = null, FollowupResult follResult = null, string mobile = null)
        {
            string nm = "";
            if (!string.IsNullOrEmpty(name) && name.Trim() != "")
            {
                nm = " and 会员 like '%" + name + "%'";
            }
            string sx = "";
            if (sex > 0)
            {
                sx = " and 性别='" + (性别)Enum.ToObject(typeof(性别), (sex - 1)) + "'";
            }
            string ft = "";
            if (follType != null)
            {
                ft = " and 回访方式='" + follType.方式 + "'";
            }
            string fr = "";
            if (follResult != null)
            {
                fr = " and 跟进结果='" + follResult.结果 + "'";
            }
            string mb = "";
            if (!string.IsNullOrEmpty(mobile) && mobile.Trim() != "")
            {
                mb = " and 电话 like '%" + mobile.Trim() + "%'";
            }
            string where = "(1=1)" + nm + sx + ft + fr + mb;
            return FollowupLogic.GetInstance().GetFollowups(where);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            DataTable dt = dataGridView1.DataSource as DataTable;
            if (dt != null)
            {
                dt.TableName = "回访记录信息";
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
                Followup followup = comboBox1.SelectedItem as Followup;
                if (followup != null)
                {
                    //comboBox2.SelectedIndex = GetIndexByMember(followup.Member, comboBox2);
                    selectMemberControl1.SelectedMembers = new List<Member>() { followup.Member };
                    comboBox3.SelectedIndex = GetIndexByFollowupType(followup.回访方式, comboBox3);
                    comboBox4.SelectedIndex = GetIndexByFollowupResult(followup.跟进结果, comboBox4);
                    textBox3.Text = followup.跟进时间.ToString("yyyy-MM-dd");
                    monthCalendar1.SelectionStart = followup.跟进时间;
                    selectStaffControl1.SelectedStaffs = new List<Staff>() { followup.跟进人 };
                    textBox6.Text = followup.备注;
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

        private int GetIndexByFollowupType(FollowupType follType, ComboBox comboBox3)
        {
            if (follType != null && comboBox3 != null)
            {
                for (int i = 0; i < comboBox3.Items.Count; i++)
                {
                    FollowupType ct = comboBox3.Items[i] as FollowupType;
                    if (ct != null && ct.ID == follType.ID)
                        return i;
                }
            }
            return -1;
        }

        private int GetIndexByFollowupResult(FollowupResult follResult, ComboBox comboBox4)
        {
            if (follResult != null && comboBox4 != null)
            {
                for (int i = 0; i < comboBox4.Items.Count; i++)
                {
                    FollowupResult ct = comboBox4.Items[i] as FollowupResult;
                    if (ct != null && ct.ID == follResult.ID)
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
            monthCalendar1.Hide();
        }

        private void monthCalendar1_Leave(object sender, EventArgs e)
        {
            monthCalendar1.Hide();
        }

        private void tabPage1_MouseClick(object sender, MouseEventArgs e)
        {
            monthCalendar1.Hide();
        }
    }
}
