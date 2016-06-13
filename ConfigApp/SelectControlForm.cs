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
    public partial class SelectControlForm : Form
    {
        public SelectControlForm(string typeName)
        {
            InitializeComponent();
            comboBox3.SelectedIndex = 0;
            this.textBox2.Text = typeName;
        }

        public string SelectedControlName
        {
            get
            {
                return textBox4.Text;
            }
        }

        public string FormName
        {
            get
            {
                return textBox2.Text;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            LoadControls(checkBox1.Checked);
        }

        private void LoadControls(bool priveteOnly)
        {
            string assembly = textBox1.Text;
            string namespaceName = textBox3.Text.Trim();
            string formName = textBox2.Text.Trim();
            string typeName = namespaceName + "." + formName;
            int controlType = comboBox3.SelectedIndex;
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
                            comboBox2.Items.Clear();
                            List<string> main = new List<string>();
                            List<string> sub = new List<string>();
                            foreach (FieldInfo fi in fs)
                            {
                                GetControls(fi, main, sub, controlType);
                            }
                            main.Sort();
                            sub.Sort();
                            comboBox1.Items.AddRange(main.ToArray());
                            comboBox2.Items.AddRange(sub.ToArray());
                        }
                        else
                        {
                            MessageBox.Show("指定的类中找不到任何字段！");
                        }
                    }
                    else
                    {
                        MessageBox.Show("指定的程序集中不存在类[" + typeName + "]！");
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

        private void GetControls(FieldInfo fi, List<string> main,  List<string> sub, int controlType = 0)
        {
            if (fi.FieldType.IsSubclassOf(typeof(Component)))
            {
                switch (controlType)
                {
                    case 0://菜单
                        if (fi.FieldType.IsSubclassOf(typeof(ToolStrip)))
                        {
                            main.Add(fi.Name);
                        }
                        else if (fi.FieldType.IsSubclassOf(typeof(ToolStripItem)))
                        {
                            sub.Add(fi.Name);
                        }
                        break;
                    case 1://选项卡
                        if (fi.FieldType.Equals(typeof(TabControl)))
                        {
                            main.Add(fi.Name);
                        }
                        else if (fi.FieldType.Equals(typeof(TabPage)))
                        {
                            sub.Add(fi.Name);
                        }
                        break;
                    case 2://按钮
                        if (fi.FieldType.IsSubclassOf(typeof(ButtonBase)))
                        {
                            main.Add(fi.Name);
                        }
                        break;
                    case 3://全部
                    default:
                        main.Add(fi.Name);
                        break;
                }
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

        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox2.Text = comboBox4.Text;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            List<string> types = GetTypes();
            types.Sort();
            comboBox4.Items.Clear();
            comboBox4.Items.AddRange(types.ToArray());
        }

        private List<string> GetTypes()
        {
            List<string> ts = new List<string>();
            string assembly = textBox1.Text;
            if (assembly != "")
            {
                string assFile = AppDomain.CurrentDomain.BaseDirectory + assembly;
                if (File.Exists(assFile))
                {
                    Assembly ass = Assembly.LoadFrom(assFile);
                    if (ass != null)
                    {
                        Type[] types = ass.GetTypes();
                        if (types != null)
                        {
                            foreach (Type t in types)
                            {
                                if (t.IsSubclassOf(typeof(Form)))
                                {
                                    ts.Add(t.Name);
                                }
                            }
                        }
                        else
                        {
                            MessageBox.Show("指定的程序集中没有任何类！");
                        }
                    }
                }
                else
                {
                    MessageBox.Show("指定的程序集[" + assembly + "]在当前目录下不存在！");
                }
            }
            else
            {
                MessageBox.Show("程序集不能为空！");
            }
            return ts;
        }
    }
}
