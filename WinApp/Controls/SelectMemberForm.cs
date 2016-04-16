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
    public partial class SelectMemberForm : Form
    {
        public SelectMemberForm()
        {
            InitializeComponent();
        }

        public List<Member> SelectedMembers
        {
            get
            {
                List<Member> ret = new List<Member>();
                DataTable dt = dataGridView1.DataSource as DataTable;
                if (dt != null)
                {
                    foreach (DataGridViewRow row in dataGridView1.SelectedRows)
                    {
                        if (row.Index > -1 && row.Index < dt.Rows.Count)
                        {
                            DataRow dr = dt.Rows[row.Index];
                            if (dr != null)
                            {
                                Member member = MemberLogic.GetInstance().GetMemberByDataRow(dr);
                                if (member != null)
                                {
                                    ret.Add(member);
                                }
                            }
                        }
                    }
                }
                return ret;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        private void SelectMemberForm_Load(object sender, EventArgs e)
        {
            LoadAllMembers();
            LoadCardTypes();
        }

        private void LoadCardTypes()
        {
            List<CardType> elements = CardTypeLogic.GetInstance().GetAllCardTypes();
            comboBox2.Items.Clear();
            comboBox2.Items.Add("--不限--");
            foreach (CardType element in elements)
            {
                comboBox2.Items.Add(element);
            }
            comboBox2.SelectedIndex = 0;
        }

        private void LoadAllMembers()
        {
            DataTable dt = MemberLogic.GetInstance().GetMembers(string.Empty);
            dataGridView1.DataSource = dt;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DataTable dt = Search(comboBox1.SelectedIndex, textBox1.Text.Trim(), textBox2.Text.Trim(), comboBox2.SelectedItem as CardType, textBox3.Text.Trim(), checkBox1.Checked);
            dataGridView1.DataSource = dt;
        }

        private DataTable Search(int sex, string name, string mobile, CardType cardType, string cardNo, bool deadline)
        {
            string nm = "";
            if (!string.IsNullOrEmpty(name) && name.Trim() != "")
            {
                nm = " and 姓名 like '%" + name + "%'";
            }
            string sx = "";
            if (sex > 0)
            {
                sx = " and 性别='" + Enum.GetName(typeof(性别), Convert.ToInt32(sex - 1)) + "'";
            }
            string ct = "";
            if (cardType != null)
            {
                ct = " and 卡种='" + cardType.卡种 + "'";
            }
            string cn = "";
            if (!string.IsNullOrEmpty(cardNo) && cardNo.Trim() != "")
            {
                cn = " and 卡号 like '%" + cardNo.Trim() + "%'";
            }
            string mb = "";
            if (!string.IsNullOrEmpty(mobile) && mobile.Trim() != "")
            {
                mb = " and 电话 like '%" + mobile.Trim() + "%'";
            }
            string dl = "";
            if (deadline)
                dl = " and 到期日<'" + DateTime.Now.ToString("yyyy-MM-dd") + "'";
            string where = "(1=1)" + nm + sx + ct + cn + mb + dl;
            return MemberLogic.GetInstance().GetMembers(where);
        }
    }
}
