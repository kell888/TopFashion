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
    public partial class UgForm : Form
    {
        public UgForm(List<UserGroup> data)
        {
            InitializeComponent();
            this.data = data;
        }

        List<UserGroup> data;

        public List<UserGroup> Data
        {
            get { return data; }
        }

        private void UgForm_Load(object sender, EventArgs e)
        {
            RefreshInfo();
        }

        private void RefreshInfo()
        {
            comboBox1.Items.Clear();
            foreach (UserGroup d in data)
            {
                comboBox1.Items.Add(d);
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex > -1)
            {
                UserGroup d = comboBox1.SelectedItem as UserGroup;
                if (d != null)
                {
                    LoadInfo(d);
                }
            }
        }

        private void LoadInfo(UserGroup d)
        {
            textBox1.Text = d.Name;
            textBox2.Text = d.Remark;
        }

        private void button11_Click(object sender, EventArgs e)
        {
            UserGroup ug = new UserGroup();
            ug.Name = textBox1.Text.Trim();
            ug.Remark = textBox2.Text;
            UserGroupLogic ul = UserGroupLogic.GetInstance();
            if (ul.ExistsName(ug.Name))
            {
                if (MessageBox.Show("系统中已经存在该名称，确定还要继续保存么？", "重名提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.OK)
                {
                    int id = ul.AddUserGroup(ug);
                    if (id > 0)
                    {
                        ug.ID = id;
                        data.Add(ug);
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
                int id = ul.AddUserGroup(ug);
                if (id > 0)
                {
                    ug.ID = id;
                    data.Add(ug);
                    RefreshInfo();
                    MessageBox.Show("添加成功！");
                }
            }
        }

        private void button22_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex > -1)
            {
                UserGroup ug = new UserGroup();
                ug.ID = data[comboBox1.SelectedIndex].ID;
                ug.Name = textBox1.Text.Trim();
                ug.Remark = textBox2.Text;
                UserGroupLogic ul = UserGroupLogic.GetInstance();
                if (ul.ExistsNameOther(ug.Name, ug.ID))
                {
                    if (MessageBox.Show("系统中已经存在该名称，确定还要继续保存么？", "重名提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.OK)
                    {
                        if (ul.UpdateUserGroup(ug))
                        {
                            data[comboBox1.SelectedIndex].Name = ug.Name;
                            data[comboBox1.SelectedIndex].Remark = ug.Remark;
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
                    if (ul.UpdateUserGroup(ug))
                    {
                        data[comboBox1.SelectedIndex].Name = ug.Name;
                        data[comboBox1.SelectedIndex].Remark = ug.Remark;
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
                    UserGroup ug = data[comboBox1.SelectedIndex];
                    if (UserGroupLogic.GetInstance().DeleteUserGroup(ug))
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
    }
}
