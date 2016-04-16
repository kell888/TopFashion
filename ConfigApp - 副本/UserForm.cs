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
    public partial class UserForm : Form
    {
        public UserForm(List<User> data, List<Department> deps, List<ConfigEntity> userStatuses)
        {
            InitializeComponent();
            this.data = data;
            this.deps = deps;
            this.userStatuses = userStatuses;
        }

        List<User> data;

        public List<User> Data
        {
            get { return data; }
        }
        List<Department> deps;
        List<ConfigEntity> userStatuses;

        private void UserForm_Load(object sender, EventArgs e)
        {
            RefreshInfo();
        }

        private void RefreshInfo()
        {
            comboBox1.Items.Clear();
            comboBox2.Items.Clear();
            comboBox3.Items.Clear();
            foreach (User d in data)
            {
                comboBox1.Items.Add(d);
            }
            foreach (Department d in deps)
            {
                comboBox2.Items.Add(d);
            }
            foreach (ConfigEntity d in userStatuses)
            {
                comboBox3.Items.Add(d);
            }
            if (comboBox3.Items.Count > 0)
                comboBox3.SelectedIndex = 0;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex > -1)
            {
                User d = comboBox1.SelectedItem as User;
                if (d != null)
                {
                    LoadInfo(d);
                }
            }
        }

        private void LoadInfo(User d)
        {
            textBox1.Text = d.Username;
            textBox2.Text = d.Remark;
            textBox3.Text = d.Password;
            for (int i = 0; i < comboBox2.Items.Count; i++)
            {
                Department dep = comboBox2.Items[i] as Department;
                if (dep != null)
                {
                    if (d.Departments.ContainsDepartment(dep.ID))
                    {
                        comboBox2.SelectedIndex = i;
                        break;
                    }
                }
            }
            for (int i = 0; i < comboBox3.Items.Count; i++)
            {
                ConfigEntity ce = comboBox3.Items[i] as ConfigEntity;
                if (ce != null)
                {
                    if (d.Flag == ce.extension)
                    {
                        comboBox3.SelectedIndex = i;
                        break;
                    }
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            User user = new User();
            user.Username = textBox1.Text.Trim();
            user.Remark = textBox2.Text;
            user.Flag = 0;
            ConfigEntity ce = comboBox3.SelectedItem as ConfigEntity;
            if (ce != null)
                user.Flag = ce.extension;
            user.Password = textBox3.Text;
            Department dep = comboBox2.SelectedItem as Department;
            if (dep != null)
            {
                user.Departments.Clear();
                user.Departments.Add(dep.ID);
            }
            UserLogic ul = UserLogic.GetInstance();
            if (ul.ExistsName(user.Username))
            {
                if (MessageBox.Show("系统中已经存在该名称，确定还要继续保存么？", "重名提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.OK)
                {
                    int id = ul.AddUser(user);
                    if (id > 0)
                    {
                        user.ID = id;
                        data.Add(user);
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
                int id = ul.AddUser(user);
                if (id > 0)
                {
                    user.ID = id;
                    data.Add(user);
                    RefreshInfo();
                    MessageBox.Show("添加成功！");
                }
            }
        }

        private void btn_User_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex > -1)
            {
                User user = new User();
                user.ID = data[comboBox1.SelectedIndex].ID;
                user.Username = textBox1.Text.Trim();
                user.Remark = textBox2.Text;
                user.Password = textBox3.Text;
                Department dep = comboBox2.SelectedItem as Department;
                if (dep != null)
                {
                    user.Departments.Clear();
                    user.Departments.Add(dep.ID);
                }
                user.Flag = 0;
                ConfigEntity ce = comboBox3.SelectedItem as ConfigEntity;
                if (ce != null)
                    user.Flag = ce.extension;
                UserLogic ul = UserLogic.GetInstance();
                if (ul.ExistsNameOther(user.Username, user.ID))
                {
                    if (MessageBox.Show("系统中已经存在该名称，确定还要继续保存么？", "重名提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.OK)
                    {
                        if (ul.UpdateUser(user))
                        {
                            data[comboBox1.SelectedIndex].Username = user.Username;
                            data[comboBox1.SelectedIndex].Password = user.Password;
                            data[comboBox1.SelectedIndex].Flag = user.Flag;
                            data[comboBox1.SelectedIndex].Departments = user.Departments;
                            data[comboBox1.SelectedIndex].Remark = user.Remark;
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
                    if (ul.UpdateUser(user))
                    {
                        data[comboBox1.SelectedIndex].Username = user.Username;
                        data[comboBox1.SelectedIndex].Password = user.Password;
                        data[comboBox1.SelectedIndex].Flag = user.Flag;
                        data[comboBox1.SelectedIndex].Departments = user.Departments;
                        data[comboBox1.SelectedIndex].Remark = user.Remark;
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

        private void button7_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex > -1)
            {
                if (MessageBox.Show("确定要删除该项目？", "删除提醒", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.OK)
                {
                    User user = data[comboBox1.SelectedIndex];
                    if (UserLogic.GetInstance().DeleteUser(user))
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
