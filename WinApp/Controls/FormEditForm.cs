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
    public partial class FormEditForm : PermissionForm
    {
        public FormEditForm(User user)
        {
            this.User = user;
            InitializeComponent();
        }

        const string FieldName = "FieldName";
        int width = 4;
        int height = 6;
        List<FormItem> items;

        public FormObject Form
        {
            get
            {
                return GetFormObject();
            }
            set
            {
                if (value != null)
                {
                    LoadFormObject(value);
                }
            }
        }

        private void LoadFormObject(FormObject form)
        {
            textBox1.Text = form.FormName;
            textBox2.Text = form.Remark;
            comboBox1.SelectedIndex = GetIndexByFormType(form.FormType, comboBox1);
            items = form.FormItems;
            LoadItems();
        }

        private int GetIndexByFormType(FormType type, ComboBox comboBox1)
        {
            if (type != null && comboBox1 != null)
            {
                for (int i = 0; i < comboBox1.Items.Count; i++)
                {
                    FormType ft = comboBox1.Items[i] as FormType;
                    if (type.ID == ft.ID)
                        return i;
                }
            }
            return -1;
        }

        private void LoadItems()
        {
            if (items != null)
            {
                panel2.SuspendLayout();
                panel2.Controls.Clear();
                for (int i = 0; i < items.Count; i++)
                {
                    FormItem item = items[i];
                    FormEditControl fec = new FormEditControl();
                    fec.Field = item;
                    fec.Location = new Point(width, height + (height + fec.Height) * i);
                    fec.AddItemHandler += new EventHandler(fec_AddItemHandler);
                    fec.RemoveItemHandler += new EventHandler(fec_RemoveItemHandler);
                    fec.ChangeItemNameHandler += new FieldNameChangedHandler(fec_ItemNameChangedHandler);
                    if (items.Count == 1)
                        fec.CanRemove = false;//最后一个必须留着，免得无法再添加
                    panel2.Controls.Add(fec);
                }
                panel2.ResumeLayout(true);
            }
        }

        void fec_ItemNameChangedHandler(FormEditControl fec, string newName)
        {
            int index = items.GetIndex(fec.Field);
            if (index > -1)
            {
                items[index].ItemName = newName;
            }
        }

        void fec_RemoveItemHandler(object sender, EventArgs e)
        {
            FormEditControl thisItem = sender as FormEditControl;
            int index = items.GetIndex(thisItem.Field);
            if (index > -1)
            {
                items.RemoveAt(index);
                LoadItems();
            }
        }

        void fec_AddItemHandler(object sender, EventArgs e)
        {
            FormEditControl thisItem = sender as FormEditControl;
            string newName;
            int index = items.GetIndex(thisItem.Field, out newName);
            if (index > -1)
            {
                items.Insert(index + 1, FormItem.Empty(newName));
                LoadItems();
            }
        }

        private FormObject GetFormObject()
        {
            FormObject form = new FormObject();
            form.FormName = textBox1.Text.Trim();
            form.FormType = comboBox1.SelectedItem as FormType;
            form.FormItems = items;
            form.Owner = this.User;
            form.Remark = textBox2.Text;
            return form;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            SelectOneForm sof = new SelectOneForm();
            if (sof.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if (sof.SelectedForm != null)
                {
                    FormObject thisForm = sof.SelectedForm;
                    string name;
                    int id = FormObjectLogic.GetInstance().CopyFormObject(thisForm, out name);
                    thisForm.ID = id;
                    thisForm.FormName = name;
                    LoadFormObject(thisForm);
                }
                else
                {
                    MessageBox.Show("是否没选到合法的表单？要复制的表单为空！");
                }
            }
            sof.Dispose();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            using (FormTypeForm f = new FormTypeForm(this.User))
            {
                f.ShowDialog();
            }
            LoadFormTypes();
        }

        private void FormEditForm_Load(object sender, EventArgs e)
        {
            base.DisableUserPermission(this);
            items = new List<FormItem>();
            items.Add(formEditControl1.Field);
            LoadFormTypes();
        }

        private void LoadFormTypes()
        {
            comboBox1.Items.Clear();
            List<FormType> types = FormTypeLogic.GetInstance().GetAllFormTypes();
            foreach (FormType type in types)
            {
                comboBox1.Items.Add(type);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
