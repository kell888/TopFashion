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
            items = new List<FormItem>();
            items.Add(formEditControl1.Field);
        }

        const string FieldName = "FieldName";
        int width = 4;
        int height = 6;
        List<FormItem> items;

        int formId;

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
            formId = form.ID;
            textBox1.Text = form.FormName;
            textBox2.Text = form.Remark;
            if (form.FormItems != null && form.FormItems.Count > 0)
            {
                items.Clear();
                foreach (FormItem item in form.FormItems)
                {
                    items.Add(item);
                }
            }
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
            if (items != null && items.Count > 0)
            {
                panel2.SuspendLayout();
                panel2.Controls.Clear();
                for (int i = 0; i < items.Count; i++)
                {
                    FormItem item = items[i];
                    FormEditControl fec = new FormEditControl();
                    fec.Field = item;
                    fec.Location = new Point(width, height + (height + fec.Height) * i);
                    fec.Width = panel2.Width - width * 2;
                    fec.Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right;
                    fec.AddItemHandler += new EventHandler(fec_AddItemHandler);
                    fec.RemoveItemHandler += new EventHandler(fec_RemoveItemHandler);
                    fec.ChangeItemNameHandler += new FieldNameChangedHandler(fec_ItemNameChangedHandler);
                    fec.ChangeItemValueHandler += new FieldValueChangedHandler(fec_ChangeItemValueHandler);
                    if (items.Count == 1)
                        fec.CanRemove = false;//最后一个必须留着，免得无法再添加
                    panel2.Controls.Add(fec);
                }
                panel2.ResumeLayout(true);
            }
        }

        void fec_ChangeItemValueHandler(FormEditControl fec, ItemValueControl value)
        {
            int index = items.GetIndex(fec.Field);
            if (index > -1)
            {
                items[index].ItemType = Commons.GetType(value.ItemType);
                items[index].ItemValue = value.ItemValue;
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
            string name = textBox1.Text.Trim();
            if (name == "")
                name = "未命名表单";

            FormObject form = new FormObject();
            form.ID = formId;
            form.FormName = name;
            form.FormType = comboBox1.SelectedItem as FormType;
            form.FormItems = items;
            if (formId == 0)
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
                    if (this.User != null)
                        thisForm.Owner = this.User;
                    if (MessageBox.Show("确定只提取原表单的格式（不要原来的数据）？", "模板提取提醒", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.OK)
                    {
                        thisForm = Commons.GetFormModule(sof.SelectedForm, this.User);
                    }
                    string name;
                    int id = FormObjectLogic.GetInstance().CopyFormObject(thisForm, out name);
                    thisForm.ID = id;
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
            LoadFormTypes();
            if (formId > 0)
            {
                FormObject form = FormObjectLogic.GetInstance().GetFormObject(formId);
                if (form != null)
                    comboBox1.SelectedIndex = GetIndexByFormType(form.FormType, comboBox1);
            }
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
            FormItemLogic fil = FormItemLogic.GetInstance();
            foreach (FormItem item in items)
            {
                int id = fil.AddFormItem(item);
                if (id > 0)
                {
                    item.ID = id;
                }
            }
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            panel2.Controls.Clear();
            items.Clear();
            panel2.Controls.Add(formEditControl1);
            items.Add(formEditControl1.Field);
        }
    }
}
