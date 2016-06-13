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
    public partial class ModForm : Form
    {
        public ModForm(List<Module> data)
        {
            InitializeComponent();
            this.data = data;
        }

        List<Module> data;

        public List<Module> Data
        {
            get { return data; }
        }

        private void ModForm_Load(object sender, EventArgs e)
        {
            RefreshInfo();
        }

        private void RefreshInfo()
        {
            comboBox1.Items.Clear();
            foreach (Module d in data)
            {
                comboBox1.Items.Add(d);
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex > -1)
            {
                Module d = comboBox1.SelectedItem as Module;
                if (d != null)
                {
                    LoadInfo(d);
                }
            }
        }

        private void LoadInfo(Module d)
        {
            textBox1.Text = d.Name;
            textBox3.Text = d.FormName;
            textBox4.Text = d.ControlName;
            textBox2.Text = d.Remark;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex > -1)
            {
                if (MessageBox.Show("确定要删除该项目？", "删除提醒", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.OK)
                {
                    Module module = data[comboBox1.SelectedIndex];
                    if (ModuleLogic.GetInstance().DeleteModule(module))
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

        private void btn_Modu_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex > -1)
            {
                Module module = new Module();
                module.ID = data[comboBox1.SelectedIndex].ID;
                module.Name = textBox1.Text.Trim();
                module.FormName = textBox3.Text.Trim();
                module.ControlName = textBox4.Text.Trim();
                module.Remark = textBox2.Text;
                ModuleLogic ml = ModuleLogic.GetInstance();
                if (ml.ExistsNameOther(module.Name, module.ID))
                {
                    if (MessageBox.Show("系统中已经存在该名称，确定还要继续保存么？", "重名提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.OK)
                    {
                        if (ml.UpdateModule(module))
                        {
                            data[comboBox1.SelectedIndex].Name = module.Name;
                            data[comboBox1.SelectedIndex].FormName = module.FormName;
                            data[comboBox1.SelectedIndex].ControlName = module.ControlName;
                            data[comboBox1.SelectedIndex].Remark = module.Remark;
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
                    if (ml.UpdateModule(module))
                    {
                        data[comboBox1.SelectedIndex].Name = module.Name;
                        data[comboBox1.SelectedIndex].FormName = module.FormName;
                        data[comboBox1.SelectedIndex].ControlName = module.ControlName;
                        data[comboBox1.SelectedIndex].Remark = module.Remark;
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

        private void button18_Click(object sender, EventArgs e)
        {
            Module module = new Module();
            module.Name = textBox1.Text.Trim();
            module.FormName = textBox3.Text.Trim();
            module.ControlName = textBox4.Text.Trim();
            module.Remark = textBox2.Text;
            ModuleLogic ml = ModuleLogic.GetInstance();
            if (ml.ExistsName(module.Name))
            {
                if (MessageBox.Show("系统中已经存在该名称，确定还要继续保存么？", "重名提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.OK)
                {
                    int id = ml.AddModule(module);
                    if (id > 0)
                    {
                        module.ID = id;
                        data.Add(module);
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
                int id = ml.AddModule(module);
                if (id > 0)
                {
                    module.ID = id;
                    data.Add(module);
                    RefreshInfo();
                    MessageBox.Show("添加成功！");
                }
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
