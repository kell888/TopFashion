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
    [DefaultEvent("NameChanged")]
    public partial class ItemNameControl : UserControl
    {
        public ItemNameControl()
        {
            InitializeComponent();
        }

        [Browsable(true)]
        [Description("修改字段名时触发的事件")]
        public event NameChangedHandler NameChanged;

        /// <summary>
        /// 获取或设置字段名
        /// </summary>
        [DefaultValue("Field1")]
        [Description("表单中的字段名")]
        [DisplayName("字段名")]
        [Browsable(true)]
        public string ItemName
        {
            get
            {
                return textBox1.Text.Trim();
            }
            set
            {
                textBox1.Text = value;
            }
        }

        private void ItemNameControl_Resize(object sender, EventArgs e)
        {
            this.Height = 21;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (NameChanged != null)
                NameChanged(this, textBox1.Text.Trim());
        }
    }

    public delegate void NameChangedHandler(ItemNameControl inc, string name);
}
