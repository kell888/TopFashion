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
    public partial class DocEditControl : UserControl
    {
        public DocEditControl()
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
            comboBox1.SelectedIndex = 0;
            field = new FormItem();
        }

        UploadControl uc;
        DateTimePicker dtp;
        NumericUpDown nud;
        TextBox tb;

        //public event FieldTypeChangedHandler ChangeItemValueHandler;

        [DefaultValue(true)]
        [Description("字段值是否能编辑")]
        [DisplayName("能否编辑")]
        [Browsable(true)]
        public bool CanEdit
        {
            get { return tb.Enabled; }
            set { tb.Enabled = nud.Enabled = dtp.Enabled = uc.Enabled = value; }
        }

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
        
        [DefaultValue(0)]
        [Description("设置字段的执行级别，最大可以设置为5级")]
        [DisplayName("执行级别设置")]
        [Browsable(true)]
        public int Excution
        {
            get
            {
                int index = comboBox1.SelectedIndex;
                if (index < 6)
                    return index;
                else
                    return 0;
            }
            set
            {
                if (value > -1)
                    comboBox1.SelectedIndex = value;
                else
                    comboBox1.SelectedIndex = 0;
            }
        }

        [DefaultValue(0)]
        [Description("设置字段的审批级别，最大可以设置为5级")]
        [DisplayName("审批级别设置")]
        [Browsable(true)]
        public int Approval
        {
            get
            {
                int index = comboBox1.SelectedIndex;
                if (index > 5)
                    return index - 5;
                else
                    return 0;
            }
            set
            {
                if (value > 0)
                    comboBox1.SelectedIndex = value + 5;
                else
                    comboBox1.SelectedIndex = 0;
            }
        }

        public void GetExecAppr(out bool isAppr, out int level)
        {
            int index = comboBox1.SelectedIndex;
            isAppr = false;
            level = 0;
            if (index > -1)
            {//特殊字段，进行特殊处理
                level = index % 6;
                if (index > 5)
                {
                    level++;
                    isAppr = true;
                }
                if (isAppr)
                {
                    Approval = level;
                }
                else
                {
                    Excution = level;
                }
            }
        }

        public SystemType ValueType
        {
            get
            {
                SystemType valueType = SystemType.字符;
                if (panel1.Controls.Count == 1)
                {
                    Control c = panel1.Controls[0];
                    if (c is TextBox)
                        valueType = SystemType.字符;
                    else if (c is NumericUpDown)
                        valueType = SystemType.数字;
                    else if (c is DateTimePicker)
                        valueType = SystemType.时间;
                    else if (c is UploadControl)
                        valueType = SystemType.附件;
                }
                return valueType;
            }
        }

        [Browsable(false)]
        public int FieldId
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
                field.ID = FieldId;
                field.ItemName = label1.Text.Trim();
                field.ItemValue = GetValue(ValueType);
                field.ItemType = Commons.GetType(ValueType);
                field.Flag = comboBox1.SelectedIndex;
                return field;
            }
            set
            {
                if (value != null)
                {
                    field = value;
                    FieldId = field.ID;
                    label1.Text = field.ItemName + " ";
                    comboBox1.SelectedIndex = field.Flag;
                    SetItemValue(Commons.GetSystemType(field.ItemType), field.ItemValue);
                }
                else
                {
                    label1.Text = "未知字段 ";
                    comboBox1.SelectedIndex = 0;
                    SetItemValue(SystemType.字符, "");
                    field.ID = 0;
                    field.ItemName = "未知字段";
                    field.ItemValue = "";
                    field.ItemType = Commons.GetType(SystemType.字符);
                }
            }
        }

        private string GetValue(SystemType type)
        {
            switch (type)
            {
                case SystemType.附件:
                    return uc.Attachments.Count > 0 ? (uc.Attachments[0].ID + "|" + uc.Attachments[0].AttachmentFilename) : "";
                case SystemType.时间:
                    return dtp.Value.ToString("yyyy-MM-dd HH:mm:ss");
                case SystemType.数字:
                    return nud.Value.ToString();
                case SystemType.字符:
                default:
                    return tb.Text;
            }
        }

        /// <summary>
        /// 设置字段的值（注意：只能自己上传附件，无法修改别人上传的附件，其他字段类型的值就能修改）
        /// </summary>
        /// <param name="type"></param>
        /// <param name="obj"></param>
        public void SetItemValue(SystemType type, string obj)
        {
            panel1.Controls.Clear();
            Control c = null;
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
                    c = uc;
                    break;
                case SystemType.时间:
                    dtp.Value = Convert.ToDateTime(obj);
                    c = dtp;
                    break;
                case SystemType.数字:
                    nud.Value = Convert.ToDecimal(obj);
                    c = nud;
                    break;
                case SystemType.字符:
                default:
                    tb.Text = Convert.ToString(obj);
                    c = tb;
                    break;
            }
            if (c != null)
            {
                panel1.Controls.Add(c);
            }
        }

        //private void itemValueControl1_ValueChanged(ItemValueControl ivc)
        //{
        //    if (ChangeItemValueHandler != null)
        //        ChangeItemValueHandler(this, ivc);
        //}

        private void DocEditControl_Resize(object sender, EventArgs e)
        {
            this.Height = 21;
        }
    }
    //public delegate void FieldTypeChangedHandler(DocEditControl dec, ItemValueControl value);
}
