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
    public partial class DepartForm : Form
    {
        public DepartForm(List<Department> data)
        {
            InitializeComponent();
            this.data = data;
        }

        List<Department> data;

        public List<Department> Data
        {
            get { return data; }
        }

        private void DepartForm_Load(object sender, EventArgs e)
        {
            RefreshInfo();
        }

        private void RefreshInfo()
        {
            comboBox1.Items.Clear();
            comboBox11.Items.Clear();
            comboBox11.Items.Add("--公司--");
            foreach (Department d in data)
            {
                comboBox1.Items.Add(d);
                comboBox11.Items.Add(d);
            }
            comboBox11.SelectedIndex = 0;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex > -1)
            {
                Department d = comboBox1.SelectedItem as Department;
                if (d != null)
                {
                    LoadInfo(d);
                }
            }
        }

        private void LoadInfo(Department d)
        {
            textBox1.Text = d.Name;
            textBox2.Text = d.Remark;
            textBox3.Text = d.Manager;
            bool find = false;
            for (int i = 0; i < comboBox11.Items.Count; i++)
            {
                Department dep = comboBox11.Items[i] as Department;
                if (dep != null && d.ParentID > 0)//d.Parent != null)
                {
                    if (d.ParentID == dep.ID)
                    {
                        comboBox11.SelectedIndex = i;
                        find = true;
                        break;
                    }
                }
            }
            if (!find)
                comboBox11.SelectedIndex = 0;
        }

        private void button12_Click(object sender, EventArgs e)
        {
            Department depart = new Department();
            depart.Name = textBox1.Text.Trim();
            depart.Manager = textBox3.Text.Trim();
            Department parent = comboBox11.SelectedItem as Department;
            if (parent != null)
                depart.ParentID = parent.ID;
            depart.Remark = textBox2.Text;
            DepartmentLogic dl = DepartmentLogic.GetInstance();
            if (dl.ExistsName(depart.Name))
            {
                if (MessageBox.Show("系统中已经存在该名称，确定还要继续保存么？", "重名提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.OK)
                {
                    int id = dl.AddDepartment(depart);
                    if (id > 0)
                    {
                        depart.ID = id;
                        data.Add(depart);
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
                int id = dl.AddDepartment(depart);
                if (id > 0)
                {
                    depart.ID = id;
                    data.Add(depart);
                    RefreshInfo();
                    MessageBox.Show("添加成功！");
                }
            }
        }

        private void btn_Dept_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex > -1)
            {
                Department depart = new Department();
                depart.ID = data[comboBox1.SelectedIndex].ID;
                depart.Name = textBox1.Text.Trim();
                depart.Manager = textBox3.Text.Trim();
                Department parent = comboBox11.SelectedItem as Department;
                if (parent != null)
                    depart.ParentID = parent.ID;
                depart.Remark = textBox2.Text;
                DepartmentLogic dl = DepartmentLogic.GetInstance();
                if (dl.ExistsNameOther(depart.Name, depart.ID))
                {
                    if (MessageBox.Show("系统中已经存在该名称，确定还要继续保存么？", "重名提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.OK)
                    {
                        if (dl.UpdateDepartment(depart))
                        {
                            data[comboBox1.SelectedIndex].Name = depart.Name;
                            data[comboBox1.SelectedIndex].Manager = depart.Manager;
                            data[comboBox1.SelectedIndex].ParentID = depart.ParentID;
                            data[comboBox1.SelectedIndex].Remark = depart.Remark;
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
                    if (dl.UpdateDepartment(depart))
                    {
                        data[comboBox1.SelectedIndex].Name = depart.Name;
                        data[comboBox1.SelectedIndex].Manager = depart.Manager;
                        data[comboBox1.SelectedIndex].ParentID = depart.ParentID;
                        data[comboBox1.SelectedIndex].Remark = depart.Remark;
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
                    Department depart = data[comboBox1.SelectedIndex];
                    if (DepartmentLogic.GetInstance().DeleteDepartment(depart))
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
