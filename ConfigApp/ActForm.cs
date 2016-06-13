using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace TopFashion
{
    public partial class ActForm : Form
    {
        public ActForm(List<Action> data)
        {
            InitializeComponent();
            this.data = data;
        }

        List<Action> data;

        public List<Action> Data
        {
            get { return data; }
        }

        private void ActForm_Load(object sender, EventArgs e)
        {
            RefreshInfo();
        }

        private void RefreshInfo()
        {
            comboBox1.Items.Clear();
            foreach (Action d in data)
            {
                comboBox1.Items.Add(d);
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex > -1)
            {
                Action d = comboBox1.SelectedItem as Action;
                if (d != null)
                {
                    LoadInfo(d);
                }
            }
        }

        private void LoadInfo(Action d)
        {
            textBox1.Text = d.Name;
            textBox3.Text = d.FormName;
            textBox4.Text = d.ControlName;
            textBox2.Text = d.Remark;
        }

        private void button19_Click(object sender, EventArgs e)
        {
            Action action = new Action();
            action.Name = textBox1.Text.Trim();
            action.FormName = textBox3.Text.Trim();
            action.ControlName = textBox4.Text.Trim();
            action.Remark = textBox2.Text;
            ActionLogic al = ActionLogic.GetInstance();
            if (al.ExistsName(action.Name))
            {
                if (MessageBox.Show("系统中已经存在该名称，确定还要继续保存么？", "重名提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.OK)
                {
                    int id = al.AddAction(action);
                    if (id > 0)
                    {
                        action.ID = id;
                        data.Add(action);
                        RefreshInfo();
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
                int id = al.AddAction(action);
                if (id > 0)
                {
                    action.ID = id;
                    data.Add(action);
                    RefreshInfo();
                    MessageBox.Show("添加成功！");
                }
            }
        }

        private void btn_Actn_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex > -1)
            {
                Action action = new Action();
                action.ID = data[comboBox1.SelectedIndex].ID;
                action.Name = textBox1.Text.Trim();
                action.FormName = textBox3.Text.Trim();
                action.ControlName = textBox4.Text.Trim();
                action.Remark = textBox2.Text;
                ActionLogic al = ActionLogic.GetInstance();
                if (al.ExistsNameOther(action.Name, action.ID))
                {
                    if (MessageBox.Show("系统中已经存在该名称，确定还要继续保存么？", "重名提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.OK)
                    {
                        if (al.UpdateAction(action))
                        {
                            data[comboBox1.SelectedIndex].Name = action.Name;
                            data[comboBox1.SelectedIndex].FormName = action.FormName;
                            data[comboBox1.SelectedIndex].ControlName = action.ControlName;
                            data[comboBox1.SelectedIndex].Remark = action.Remark;
                            RefreshInfo();
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
                    if (al.UpdateAction(action))
                    {
                        data[comboBox1.SelectedIndex].Name = action.Name;
                        data[comboBox1.SelectedIndex].FormName = action.FormName;
                        data[comboBox1.SelectedIndex].ControlName = action.ControlName;
                        data[comboBox1.SelectedIndex].Remark = action.Remark;
                        RefreshInfo();
                        MessageBox.Show("修改成功！");
                    }
                }
            }
            else
            {
                MessageBox.Show("先选定要修改的项目！");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex > -1)
            {
                if (MessageBox.Show("确定要删除该项目？", "删除提醒", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.OK)
                {
                    Action action = data[comboBox1.SelectedIndex];
                    if (ActionLogic.GetInstance().DeleteAction(action))
                    {
                        data.RemoveAt(comboBox1.SelectedIndex);
                        RefreshInfo();
                    }
                }
            }
            else
            {
                MessageBox.Show("先选定要删除的项目！");
            }
        }

        private void textBox4_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            SelectControlForm smf = new SelectControlForm(textBox3.Text.Trim());
            if (smf.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                textBox3.Text = smf.FormName;
                textBox4.Text = smf.SelectedControlName;
                textBox1.Text = GetMenuFirstChineseWord(textBox4.Text);
            }
            smf.Dispose();
        }

        private string GetMenuFirstChineseWord(string menu)
        {
            StringBuilder sb = new StringBuilder();
            int i = 0;
            while (i < menu.Length)
            {
                string s = menu.Substring(i, 1);
                if (IsChinese(s[0]))
                    sb.Append(s);
                i++;
            }
            if (sb.Length > 0)
                return sb.ToString();
            else
                return menu;
        }

        public static bool IsChinese(char c)
        {
            int chfrom = Convert.ToInt32("4e00", 16);    //范围（0x4e00～0x9fff）转换成int（chfrom～chend）
            int chend = Convert.ToInt32("9fff", 16);
            int code = (int)c;

            if (code >= chfrom && code <= chend)
            {
                return true;     //当code在中文范围内返回true

            }
            else
            {
                return false;    //当code不在中文范围内返回false
            }
        }
    }
}
