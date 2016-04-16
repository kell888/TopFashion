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
    public partial class SelectOneForm : Form
    {
        public SelectOneForm()
        {
            InitializeComponent();
        }

        FormObject selectedForm;

        public FormObject SelectedForm
        {
            get
            {
                return selectedForm;
            }
        }

        private void listBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                int index = listBox1.SelectedIndex;
                if (index > -1)
                {
                    List<FormObject> forms = listBox1.Tag as List<FormObject>;
                    if (forms != null && forms.Count > 0 && index < forms.Count)
                    {
                        selectedForm = forms[index];
                        this.DialogResult = System.Windows.Forms.DialogResult.OK;
                    }
                    else
                    {
                        MessageBox.Show("没有选择到合适的表单！");
                    }
                }
            }
        }

        private void SelectOneForm_Load(object sender, EventArgs e)
        {
            LoadAllFormObjects();
        }

        private void LoadAllFormObjects()
        {
            listBox1.Items.Clear();
            List<FormObject> forms = FormObjectLogic.GetInstance().GetAllFormObjects();
            listBox1.Tag = forms;
            foreach (FormObject form in forms)
            {
                string info = form.FormInfo;
                listBox1.Items.Add(info);
            }
        }
    }
}
