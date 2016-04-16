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
    public partial class RoleForm : Form
    {
        public RoleForm(List<Role> data)
        {
            InitializeComponent();
            this.data = data;
        }

        List<Role> data;

        public List<Role> Data
        {
            get { return data; }
        }

        private void RoleForm_Load(object sender, EventArgs e)
        {
            RefreshInfo();
        }

        private void RefreshInfo()
        {
            comboBox1.Items.Clear();
            foreach (Role d in data)
            {
                comboBox1.Items.Add(d);
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex > -1)
            {
                Role d = comboBox1.SelectedItem as Role;
                if (d != null)
                {
                    LoadInfo(d);
                }
            }
        }

        private void LoadInfo(Role d)
        {
            textBox1.Text = d.Name;
            textBox2.Text = d.Remark;
            checkBox1.Checked = d.Flag;
        }

        private void button10_Click(object sender, EventArgs e)
        {
            Role role = new Role();
            role.Name = textBox1.Text.Trim();
            role.Flag = checkBox1.Checked;
            role.Remark = textBox2.Text;
            RoleLogic rl = RoleLogic.GetInstance();
            if (rl.ExistsName(role.Name))
            {
                if (MessageBox.Show("系统中已经存在该名称，确定还要继续保存么？", "重名提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.OK)
                {
                    int id = rl.AddRole(role);
                    if (id > 0)
                    {
                        role.ID = id;
                        data.Add(role);
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
                int id = rl.AddRole(role);
                if (id > 0)
                {
                    role.ID = id;
                    data.Add(role);
                    RefreshInfo();
                    MessageBox.Show("添加成功！");
                }
            }
        }

        private void btn_Role_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex > -1)
            {
                Role role = new Role();
                role.ID = data[comboBox1.SelectedIndex].ID;
                role.Name = textBox1.Text.Trim();
                role.Flag = checkBox1.Checked;
                role.Remark = textBox2.Text;
                RoleLogic rl = RoleLogic.GetInstance();
                if (rl.ExistsNameOther(role.Name, role.ID))
                {
                    if (MessageBox.Show("系统中已经存在该名称，确定还要继续保存么？", "重名提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.OK)
                    {
                        if (rl.UpdateRole(role))
                        {
                            data[comboBox1.SelectedIndex].Name = role.Name;
                            data[comboBox1.SelectedIndex].Flag = role.Flag;
                            data[comboBox1.SelectedIndex].Remark = role.Remark;
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
                    if (rl.UpdateRole(role))
                    {
                        data[comboBox1.SelectedIndex].Name = role.Name;
                        data[comboBox1.SelectedIndex].Flag = role.Flag;
                        data[comboBox1.SelectedIndex].Remark = role.Remark;
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
                    Role role = data[comboBox1.SelectedIndex];
                    if (RoleLogic.GetInstance().DeleteRole(role))
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
