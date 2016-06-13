using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using System.IO;

namespace TopFashion
{
    public partial class SelectMenuForm : Form
    {
        public SelectMenuForm(string typeName)
        {
            InitializeComponent();
            this.textBox2.Text = typeName;
        }

        public string SelectedMenuName
        {
            get
            {
                return textBox4.Text;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            LoadMenus(checkBox1.Checked);
        }

        private void LoadMenus(bool priveteOnly)
        {
            string assembly = textBox1.Text;
            string namespaceName = textBox3.Text.Trim();
            string formName = textBox2.Text.Trim();
            string typeName = namespaceName + "." + formName;
            if (assembly != "" && formName != "")
            {
                string assFile = AppDomain.CurrentDomain.BaseDirectory + assembly;
                if (!File.Exists(assFile))
                {
                    MessageBox.Show("指定的程序集[" + assembly + "]在当前目录下不存在！");
                    return;
                }
                Assembly ass = Assembly.LoadFrom(assFile);
                if (ass != null)
                {
                    Type type = ass.GetType(typeName, false, true);
                    if (type != null)
                    {
                        BindingFlags bf = BindingFlags.Instance;
                        if (priveteOnly)
                            bf |= BindingFlags.NonPublic;
                        else
                            bf |= BindingFlags.Public | BindingFlags.NonPublic;
                        FieldInfo[] fs = type.GetFields(bf);
                        if (fs != null)
                        {
                            comboBox1.Items.Clear();
                            foreach (FieldInfo fi in fs)
                            {
                                if (fi.FieldType.IsSubclassOf(typeof(ToolStrip)) || fi.FieldType.IsSubclassOf(typeof(ToolStripDropDown)))
                                {
                                    comboBox1.Items.Add(fi.Name);
                                }
                                else if (fi.FieldType.IsSubclassOf(typeof(ToolStripItem)))
                                {
                                    comboBox2.Items.Add(fi.Name);
                                }
                            }
                        }
                        else
                        {
                            MessageBox.Show("指定的类中找不到任何字段！");
                        }
                    }
                    else
                    {
                        MessageBox.Show("指定的程序集中不存在类[" + type + "]！");
                    }
                }
                else
                {
                    MessageBox.Show("指定的文件中不存在程序集[" + assembly + "]！");
                }
            }
            else
            {
                MessageBox.Show("程序集和窗体类名不能为空！");
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox4.Text = comboBox1.Text;
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox4.Text = comboBox2.Text;
        }
    }
}
