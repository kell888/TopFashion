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
    public partial class NewDocForm : PermissionForm
    {
        public NewDocForm(User user, MainForm owner)
        {
            this.User = user;
            InitializeComponent();
            this.owner = owner;
        }

        MainForm owner;

        private void button1_Click(object sender, EventArgs e)
        {
            this.DialogResult = Next();
        }

        private DialogResult Next()
        {
            if (listBox1.SelectedIndex > -1)
            {
                FormObject form = listBox1.SelectedItem as FormObject;
                if (form != null)
                {
                    this.owner.RefreshMsg("正在打开表单模板中，请稍候...");
                    DocEditForm def = new DocEditForm(this.User, this.owner, form);
                    this.owner.RefreshMsg("Ready...");
                    return def.ShowDialog();
                }
                else
                {
                    MessageBox.Show("您选择的表单模板为空！");
                }
            }
            else
            {
                MessageBox.Show("请先选择一个表单模板！");
            }
            return System.Windows.Forms.DialogResult.Cancel;
        }

        private void NewDocForm_Load(object sender, EventArgs e)
        {
            LoadAllFormObjects();
        }

        private void LoadAllFormObjects()
        {
            List<FormObject> forms = FormObjectLogic.GetInstance().GetAllFormObjects();
            listBox1.Items.Clear();
            foreach (FormObject form in forms)
            {
                listBox1.Items.Add(form);
            }
        }

        private void listBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.DialogResult = Next();
        }
    }
}
