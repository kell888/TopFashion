using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TopFashion
{
    [Serializable]
    public partial class FormEditControl : UserControl
    {
        public FormEditControl()
        {
            InitializeComponent();
            Form owner = this.FindForm();
            int count = Commons.GetControlMaxNumInForm<FormEditControl>(owner, "formEditControl");
            field = FormItem.Empty(FormItem.FieldNamePrex + Convert.ToString(count + 1));
        }

        public event EventHandler AddItemHandler;
        public event EventHandler RemoveItemHandler;
        public event FieldNameChangedHandler ChangeItemNameHandler;
        public event FieldValueChangedHandler ChangeItemValueHandler;

        /// <summary>
        /// 是否允许添加字段控件
        /// </summary>
        [DefaultValue(true)]
        [Browsable(true)]
        [DisplayName("可添加性")]
        public bool CanAdd
        {
            get
            {
                return button2.Enabled;
            }
            set
            {
                button2.Enabled = value;
            }
        }

        /// <summary>
        /// 是否允许移除自身控件
        /// </summary>
        [DefaultValue(true)]
        [Browsable(true)]
        [DisplayName("可移除性")]
        public bool CanRemove
        {
            get
            {
                return button1.Enabled;
            }
            set
            {
                button1.Enabled = value;
            }
        }

        [Browsable(false)]
        public int ItemId
        {
            get { return field.ID; }
            set { field.ID = value; }
        }

        FormItem field;

        [Browsable(false)]
        public FormItem Field
        {
            get
            {
                field.ItemName = itemNameControl1.ItemName;
                field.ItemValue = itemValueControl1.ItemValue;
                field.ItemType = Commons.GetType(itemValueControl1.ItemType);
                return field;
            }
            set
            {
                if (value != null)
                {
                    field = value;
                    itemNameControl1.ItemName = field.ItemName;
                    itemValueControl1.SetItemValue(Commons.GetSystemType(field.ItemType), field.ItemValue);
                }
                else
                {
                    Form owner = this.FindForm();
                    int count = Commons.GetControlMaxNumInForm<FormEditControl>(owner, "formEditControl");
                    string name = FormItem.FieldNamePrex + Convert.ToString(count + 1);
                    itemNameControl1.ItemName = name;
                    itemValueControl1.SetItemValue(SystemType.字符, "");
                    field.ID = 0;
                    field.ItemName = name;
                    field.ItemValue = "";
                    field.ItemType = Commons.GetType(SystemType.字符);
                }
            }
        }

        private void FormEditControl_Resize(object sender, EventArgs e)
        {
            this.Height = 21;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (AddItemHandler != null)
                AddItemHandler(this, EventArgs.Empty);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (RemoveItemHandler != null)
                RemoveItemHandler(this, EventArgs.Empty);
        }

        private void itemNameControl1_NameChanged(ItemNameControl inc, string name)
        {
            if (ChangeItemNameHandler != null)
                ChangeItemNameHandler(this, name);
        }

        private void itemValueControl1_ValueChanged(ItemValueControl ivc)
        {
            if (ChangeItemValueHandler != null)
                ChangeItemValueHandler(this, ivc);
        }
    }

    public delegate void FieldNameChangedHandler(FormEditControl fec, string newName);

    public delegate void FieldValueChangedHandler(FormEditControl fec, ItemValueControl value);
}
