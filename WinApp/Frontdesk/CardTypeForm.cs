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
    public partial class CardTypeForm : PermissionForm
    {
        public CardTypeForm(User user, int selectIndex = 0)
        {
            this.User = user;
            InitializeComponent();
            this.tabControl1.SelectedIndex = selectIndex;
            this.selectIndex = selectIndex;
            this.tabControl1.SelectedIndexChanged += new EventHandler(tabControl1_SelectedIndexChanged);
        }
        int selectIndex;

        void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.User.ID != this.AdminId && !this.User.GetAllPermissions().ContainsControl("CardTypeForm", "会员", "tabControl1"))
                this.tabControl1.SelectedIndex = selectIndex;
        }

        private void CardTypeForm_Load(object sender, EventArgs e)
        {
            base.DisableUserPermission(this);
            LoadCardTypes();
            comboBox2.SelectedIndex = 0;
        }

        private void LoadCardTypes()
        {
            List<CardType> elements = CardTypeLogic.GetInstance().GetAllCardTypes();
            comboBox1.Items.Clear();
            foreach (CardType element in elements)
            {
                comboBox1.Items.Add(element);
            }
            dataGridView1.DataSource = CardTypeLogic.GetInstance().GetCardTypes(string.Empty);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            CardType cardType = new CardType();
            cardType.卡种 = textBox1.Text.Trim();
            cardType.备注 = textBox2.Text;
            cardType.是否电子芯片 = checkBox1.Checked;
            CardTypeLogic al = CardTypeLogic.GetInstance();
            if (al.ExistsName(cardType.卡种))
            {
                if (MessageBox.Show("系统中已经存在该卡种，确定还要继续保存么？", "重名提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.OK)
                {
                    int id = al.AddCardType(cardType);
                    if (id > 0)
                    {
                        cardType.ID = id;
                        LoadCardTypes();
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
                int id = al.AddCardType(cardType);
                if (id > 0)
                {
                    cardType.ID = id;
                    LoadCardTypes();
                    MessageBox.Show("添加成功！");
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex > -1)
            {
                CardType cardType = (CardType)comboBox1.SelectedItem;
                cardType.卡种 = textBox1.Text.Trim();
                cardType.备注 = textBox2.Text;
                cardType.是否电子芯片 = checkBox1.Checked;
                CardTypeLogic al = CardTypeLogic.GetInstance();
                if (al.ExistsNameOther(cardType.卡种, cardType.ID))
                {
                    if (MessageBox.Show("系统中已经存在该卡种，确定还要继续保存么？", "重名提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.OK)
                    {
                        if (al.UpdateCardType(cardType))
                        {
                            LoadCardTypes();
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
                    if (al.UpdateCardType(cardType))
                    {
                        LoadCardTypes();
                        MessageBox.Show("修改成功！");
                    }
                }
            }
            else
            {
                MessageBox.Show("先选定要修改的卡种！");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex > -1)
            {
                if (MessageBox.Show("确定要删除该卡种？", "删除提醒", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.OK)
                {
                    CardType cardType = (CardType)comboBox1.SelectedItem;
                    if (CardTypeLogic.GetInstance().DeleteCardType(cardType))
                    {
                        LoadCardTypes();
                    }
                }
            }
            else
            {
                MessageBox.Show("先选定要删除的卡种！");
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            DataTable dt = Search(textBox8.Text.Trim());
            dataGridView1.DataSource = dt;
        }

        private DataTable Search(string name = null, int type = 0)
        {
            string nm = "";
            if (!string.IsNullOrEmpty(name))
            {
                nm = " and 卡种 like '%" + name + "%'";
            }
            string ty = "";
            if (type > 0)
            {
                ty = " and 是否电子芯片=" + (type == 1 ? "1" : "0");
            }
            string where = "(1=1)" + nm + ty;
            return CardTypeLogic.GetInstance().GetCardTypes(where);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            DataTable dt = dataGridView1.DataSource as DataTable;
            if (dt != null)
            {
                dt.TableName = "卡种信息";
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
                CardType cardType = comboBox1.SelectedItem as CardType;
                if (cardType != null)
                {
                    textBox1.Text = cardType.卡种;
                    textBox2.Text = cardType.备注;
                    checkBox1.Checked = cardType.是否电子芯片;
                }
            }
        }
    }
}
