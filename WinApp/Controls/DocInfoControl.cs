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
    public partial class DocInfoControl : UserControl
    {
        public DocInfoControl()
        {
            InitializeComponent();
            ac = new AttachmentControl();
            ac.Dock = DockStyle.Fill;
            ac.SendToBack();
            panel1.Controls.Add(ac);
            field = new FormItem();
        }

        AttachmentControl ac;
        FormItem field;

        [Browsable(false)]
        public FormItem Field
        {
            set
            {
                if (value != null)
                {
                    field = value;
                    label1.Text = field.ItemName + " ";
                    SetItemValue(Commons.GetSystemType(field.ItemType), field.ItemValue);
                }
                else
                {
                    label1.Text = "未知字段 ";
                    SetItemValue(SystemType.字符, "");
                    field.ID = 0;
                    field.ItemName = "未知字段";
                    field.ItemValue = "";
                    field.ItemType = Commons.GetType(SystemType.字符);
                }
            }
        }

        /// <summary>
        /// 设置字段的值
        /// </summary>
        /// <param name="type"></param>
        /// <param name="obj"></param>
        public void SetItemValue(SystemType type, string obj)
        {
            if (type == SystemType.附件)
            {
                label2.SendToBack();
                PermissionForm owner = null;
                Form f = this.FindForm();
                if (f != null && f is PermissionForm)
                {
                    owner = f as PermissionForm;
                }
                string[] ss = obj.Split("|".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                if (ss.Length > 1)
                {
                    int id;
                    if (int.TryParse(ss[0], out id) && id > 0)
                    {
                        Attachment a = Commons.GetAttachmentForDownload(id);
                        ac.SetAttachment(a);
                    }
                }
                ac.BringToFront();
            }
            else
            {
                ac.SendToBack();
                label2.Text = obj;
                label2.BringToFront();
            }
        }

        private void DocInfoControl_Resize(object sender, EventArgs e)
        {
            this.Height = 21;
        }
    }
}
