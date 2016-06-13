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
    public partial class PerForm : Form
    {
        public PerForm(List<Permission> data, List<Module> mods, List<Action> acts)
        {
            InitializeComponent();
            this.data = data;
            this.mods = mods;
            this.acts = acts;
        }

        List<Permission> data;

        public List<Permission> Data
        {
            get { return data; }
        }
        List<Module> mods;
        List<Action> acts;

        private void PerForm_Load(object sender, EventArgs e)
        {
            RefreshInfo();
        }

        private void RefreshInfo()
        {
            comboBox1.Items.Clear();
            comboBox9.Items.Clear();
            comboBox10.Items.Clear();
            foreach (Permission d in data)
            {
                comboBox1.Items.Add(d);
            }
            foreach (Module m in mods)
            {
                comboBox9.Items.Add(m);
            }
            foreach (Action a in acts)
            {
                comboBox10.Items.Add(a);
            }
            comboBox10.Items.Insert(0, "--无--");
            comboBox10.SelectedIndex = 0;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex > -1)
            {
                Permission d = comboBox1.SelectedItem as Permission;
                if (d != null)
                {
                    LoadInfo(d);
                }
            }
        }

        private void LoadInfo(Permission d)
        {
            textBox1.Text = d.Name;
            checkBox1.Checked = d.IsExcept;
            textBox2.Text = d.Remark;
            for (int i = 0; i < mods.Count; i++)
            {
                if (d.TheModule.ID == mods[i].ID)
                {
                    comboBox9.SelectedIndex = i;
                    break;
                }
            }
            if (d.TheAction != null)
            {
                for (int i = 0; i < acts.Count; i++)
                {
                    if (d.TheAction.ID == acts[i].ID)
                    {
                        comboBox10.SelectedIndex = i + 1;
                        break;
                    }
                }
            }
            else
            {
                comboBox10.SelectedIndex = 0;
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            Module mod = comboBox9.SelectedItem as Module;
            Action act = comboBox10.SelectedItem as Action;
            if (mod != null)
            {
                Permission per = new Permission();
                per.Name = textBox1.Text.Trim();
                per.IsExcept = checkBox1.Checked;
                per.TheModule = mod;
                per.TheAction = act;
                per.Remark = textBox2.Text;
                PermissionLogic pl = PermissionLogic.GetInstance();
                if (pl.ExistsName(per.Name))
                {
                    if (MessageBox.Show("系统中已经存在该名称，确定还要继续保存么？", "重名提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.OK)
                    {
                        int id = pl.AddPermission(per);
                        if (id > 0)
                        {
                            per.ID = id;
                            data.Add(per);
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
                    int id = pl.AddPermission(per);
                    if (id > 0)
                    {
                        per.ID = id;
                        data.Add(per);
                        RefreshInfo();
                        MessageBox.Show("添加成功！");
                    }
                }
            }
            else
            {
                 MessageBox.Show("请选定模块！");
            }
        }

        private void btn_Perm_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex > -1)
            {
                Permission per = new Permission();
                per.ID = data[comboBox1.SelectedIndex].ID;
                per.Name = textBox1.Text.Trim();
                per.IsExcept = checkBox1.Checked;
                Module mod = comboBox9.SelectedItem as Module;
                if (mod != null)
                {
                    per.TheModule = mod;
                    Action act = comboBox10.SelectedItem as Action;
                    per.TheAction = act;
                    per.Remark = textBox2.Text;
                    PermissionLogic pl = PermissionLogic.GetInstance();
                    if (pl.ExistsNameOther(per.Name, per.ID))
                    {
                        if (MessageBox.Show("系统中已经存在该名称，确定还要继续保存么？", "重名提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.OK)
                        {
                            if (pl.UpdatePermission(per))
                            {
                                data[comboBox1.SelectedIndex].Name = per.Name;
                                data[comboBox1.SelectedIndex].TheModule = per.TheModule;
                                data[comboBox1.SelectedIndex].TheAction = per.TheAction;
                                data[comboBox1.SelectedIndex].Remark = per.Remark;
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
                        if (pl.UpdatePermission(per))
                        {
                            data[comboBox1.SelectedIndex].Name = per.Name;
                            data[comboBox1.SelectedIndex].TheModule = per.TheModule;
                            data[comboBox1.SelectedIndex].TheAction = per.TheAction;
                            data[comboBox1.SelectedIndex].Remark = per.Remark;
                            RefreshInfo();
                            MessageBox.Show("修改成功！");
                        }
                    }
                }
                else
                {
                    MessageBox.Show("请选定模块！");
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
                    Permission per = data[comboBox1.SelectedIndex];
                    if (PermissionLogic.GetInstance().DeletePermission(per))
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
