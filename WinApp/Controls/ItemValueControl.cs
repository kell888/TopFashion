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
    [DefaultEvent("ValueChanged")]
    public partial class ItemValueControl : UserControl
    {
        public ItemValueControl()
        {
            InitializeComponent();
            tb = new TextBox();
            tb.ForeColor = Color.Blue;
            tb.AcceptsReturn = true;
            tb.AcceptsTab = true;
            tb.Dock = DockStyle.Fill;
            nud = new NumericUpDown();
            nud.ForeColor = Color.Blue;
            nud.DecimalPlaces = 2;
            nud.Maximum = 1000000000;
            nud.Minimum = -999999999;
            nud.Dock = DockStyle.Fill;
            dtp = new DateTimePicker();
            dtp.ForeColor = Color.Blue;
            dtp.CustomFormat = "yyyy-MM-dd HH:mm:ss";
            dtp.Format = DateTimePickerFormat.Custom;
            dtp.Dock = DockStyle.Fill;
            uc = new UploadControl();
            uc.Dock = DockStyle.Fill;
            panel1.Controls.Add(tb);
            comboBox1.Items.Clear();
            SystemType[] types = (SystemType[])Enum.GetValues(typeof(SystemType));
            foreach (SystemType type in types)
            {
                comboBox1.Items.Add(type);
            }
            comboBox1.SelectedIndex = GetIndexByType(SystemType.字符, comboBox1);
            loadOver = true;
        }

        [Browsable(true)]
        [Description("修改字段值时触发的事件")]
        public event ValueChangedHandler ValueChanged;

        UploadControl uc;
        DateTimePicker dtp;
        NumericUpDown nud;
        TextBox tb;
        bool loadOver;

        /// <summary>
        /// 字段值
        /// </summary>
        [DefaultValue(2)]
        [Description("数字值的小数位数")]
        [DisplayName("小数位数")]
        [Browsable(true)]
        public int DecimalPlaces
        {
            get
            {
                return nud.DecimalPlaces;
            }
            set
            {
                nud.DecimalPlaces = value;
            }
        }

        /// <summary>
        /// 字段值
        /// </summary>
        [DefaultValue("")]
        [Description("表单中的字段值")]
        [DisplayName("字段值")]
        [Browsable(true)]
        public string ItemValue
        {
            get
            {
                if (comboBox1.SelectedIndex > -1)
                {
                    SystemType type = (SystemType)comboBox1.SelectedItem;
                    switch (type)
                    {
                        case SystemType.附件:
                            return uc.Attachments.Count > 0 ? uc.Attachments[0].AttachmentFilename : "";
                        case SystemType.时间:
                            return dtp.Value.ToString("yyyy-MM-dd HH:mm:ss");
                        case SystemType.数字:
                            return nud.Value.ToString();
                        case SystemType.字符:
                        default:
                            return tb.Text;
                    }
                }
                return "";
            }
            set
            {
                SetItemValue(ItemType, value);
            }
        }
        /// <summary>
        /// 字段的类型
        /// </summary>
        [DefaultValue(SystemType.字符)]
        [Browsable(true)]
        [Description("字段的类型")]
        [DisplayName("字段类型")]
        public SystemType ItemType
        {
            get
            {
                if (comboBox1.SelectedIndex > -1)
                {
                    SystemType type = (SystemType)comboBox1.SelectedItem;
                    return type;
                }
                return SystemType.字符;
            }
            set
            {
                SetType(value);
            }
        }
        /// <summary>
        /// 设置字段的值
        /// </summary>
        /// <param name="type"></param>
        /// <param name="obj"></param>
        public void SetItemValue(SystemType type, string obj)
        {
            comboBox1.SelectedIndex = GetIndexByType(type, comboBox1);
            switch (type)
            {
                case SystemType.附件:
                    PermissionForm owner = null;
                    Form f = this.FindForm();
                    if (f != null && f is PermissionForm)
                    {
                        owner = f as PermissionForm;
                    }
                    Attachment a = Commons.GetAttachment(obj, owner);
                    List<Attachment> atta = new List<Attachment>();
                    if (a != null) atta.Add(a);
                    uc.Attachments = atta;
                    break;
                case SystemType.时间:
                    dtp.Value = Convert.ToDateTime(obj);
                    break;
                case SystemType.数字:
                    nud.Value = Convert.ToDecimal(obj);
                    break;
                case SystemType.字符:
                default:
                    tb.Text = Convert.ToString(obj);
                    break;
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ValueChanged != null)
                ValueChanged(this);
            if (loadOver)
            {
                if (comboBox1.SelectedIndex > -1)
                {
                    SystemType type = (SystemType)comboBox1.SelectedItem;
                    SetType(type);
                }
            }
        }

        private void SetType(SystemType type)
        {
            loadOver = false;
            comboBox1.SelectedIndex = GetIndexByType(type, comboBox1);
            loadOver = true;
            panel1.Controls.Clear();
            Control c = null;
            switch (type)
            {
                case SystemType.附件:
                    c = uc;
                    break;
                case SystemType.时间:
                    c = dtp;
                    break;
                case SystemType.数字:
                    c = nud;
                    break;
                case SystemType.字符:
                default:
                    c = tb;
                    break;
            }
            if (c != null)
                panel1.Controls.Add(c);
        }

        private void ItemValueControl_Load(object sender, EventArgs e)
        {

        }

        private int GetIndexByType(SystemType systemType, ComboBox comboBox1)
        {
            for (int i = 0; i < comboBox1.Items.Count; i++)
            {
                SystemType type = (SystemType)comboBox1.Items[i];
                if (type == systemType)
                    return i;
            }
            return -1;
        }

        private void ItemValueControl_Resize(object sender, EventArgs e)
        {
            this.Height = 21;
        }
    }

    public delegate void ValueChangedHandler(ItemValueControl ivc);
}
