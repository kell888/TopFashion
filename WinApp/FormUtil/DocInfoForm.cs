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
    public partial class DocInfoForm : PermissionForm
    {
        public DocInfoForm(User user, MainForm owner, DocObject doc)
        {
            this.User = user;
            InitializeComponent();
            this.owner = owner;
            this.doc = doc;
            if (doc != null)
            {
                DocObjectLogic.GetInstance().Increase(doc.ID);
                this.Text = doc.DocInfo;
            }
        }

        MainForm owner;
        DocObject doc;
        int width = 4;
        int height = 6;
        
        private void LoadDocObject(DocObject doc)
        {
            if (doc != null)
                LoadItems(doc.DocItems);
        }

        private void LoadItems(List<FormItem> items)
        {
            if (items != null)
            {
                panel2.SuspendLayout();
                panel2.Controls.Clear();
                for (int i = 0; i < items.Count; i++)
                {
                    FormItem item = items[i];
                    DocInfoControl dic = new DocInfoControl();
                    dic.Field = item;
                    dic.Location = new Point(width, height + (height + dic.Height) * i);
                    dic.Width = panel2.Width - width * 2;
                    dic.Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right;
                    panel2.Controls.Add(dic);
                }
                panel2.ResumeLayout(true);
            }
        }

        private void DocInfoForm_Load(object sender, EventArgs e)
        {
            if (this.owner != null)
                this.owner.RefreshMsg("正在打开文档中，请稍候...");
            base.DisableUserPermission(this);
            LoadDocObject(doc);
            if (this.owner != null)
                this.owner.RefreshMsg("Ready...");
        }
    }
}
