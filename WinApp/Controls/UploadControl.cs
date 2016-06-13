using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace TopFashion
{
    [DefaultEvent("Click")]
    [Serializable]
    public partial class UploadControl : UserControl
    {
        public UploadControl()
        {
            InitializeComponent();
            this.label1.Text = "上传附件...";
            this.Click += new EventHandler(UploadControl_Click);
        }

        private void UploadControl_Click(object sender, EventArgs e)
        {
            label1_Click(this, e);
        }

        bool canUpload = true;

        [Browsable(true)]
        [DefaultValue(true)]
        public bool CanUpload
        {
            get
            {
                return canUpload;
            }
            set
            {
                canUpload = value;
            }
        }

        bool multiselect;

        [Browsable(true)]
        [DefaultValue(false)]
        public bool MultiAttach
        {
            get
            {
                return multiselect;
            }
            set
            {
                multiselect = value;
            }
        }

        [Browsable(false)]
        public List<Attachment> Attachments
        {
            get
            {
                List<Attachment> atts = null;
                if (this.label1.Tag != null)
                {
                    List<Attachment> staffs = this.label1.Tag as List<Attachment>;
                    if (staffs != null)
                        atts = staffs;
                    else
                        atts = new List<Attachment>();
                }
                else
                {
                    atts = new List<Attachment>();
                }
                return atts;
            }
            set
            {
                this.label1.Tag = value;
                if (value != null)
                {
                    StringBuilder sb = new StringBuilder();
                    if (value.Count > 0)
                    {
                        foreach (Attachment a in value)
                        {
                            string name = Path.GetFileName(a.AttachmentFilename);
                            if (sb.Length == 0)
                                sb.Append(name);
                            else
                                sb.Append("," + name);
                        }
                    }
                    if (sb.Length == 0)
                        this.label1.Text = "上传附件...";
                    else
                        this.label1.Text = sb.ToString();
                }
                else
                {
                    this.label1.Text = "上传附件...";
                }
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {
            if (!canUpload)
                return;

            if (this.label1.Tag != null)
            {
                List<Attachment> atts = this.label1.Tag as List<Attachment>;
                if (atts != null && atts.Count > 0)
                {
                    if (MessageBox.Show("您之前已经选好了一些附件，【确定】续传(会覆盖原有的附件)？【取消】就重新上传其它附件。", "续传提醒", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                    {
                        UploadAttachments(atts);
                    }
                    else
                    {
                        SelectFilesToUpload();
                    }
                }
                else
                {
                    SelectFilesToUpload();
                }
            }
            else
            {
                SelectFilesToUpload();
            }
        }

        private void SelectFilesToUpload()
        {
            OpenFileDialog f = new OpenFileDialog();
            f.Multiselect = multiselect;
            f.Filter = "所有文件(*.*)|*.*";
            f.Title = "选择要上传的附件...";
            if (f.ShowDialog() == DialogResult.OK)
            {
                User user = null;
                Form owner = this.FindForm();
                if (owner != null)
                {
                    if (user == null)
                    {
                        PermissionForm pf = owner as PermissionForm;
                        if (pf != null)
                        {
                            user = pf.User;
                        }
                    }
                }
                string[] files = f.FileNames;
                List<Attachment> attachs = new List<Attachment>();
                StringBuilder sb = new StringBuilder();
                foreach (string file in files)
                {
                    FileInfo fi = new FileInfo(file);
                    Attachment attach = new Attachment();
                    attach.AttachmentFilename = file;
                    attach.Size = fi.Length;
                    attach.Uploader = user;
                    attachs.Add(attach);
                    if (sb.Length == 0)
                        sb.Append(fi.Name);
                    else
                        sb.Append("," + fi.Name);
                }
                this.label1.Tag = attachs;
                this.label1.Text = sb.ToString();
                if (MessageBox.Show("确定要上传这些附件吗？" + Environment.NewLine + sb.ToString(), "上传提醒", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                {
                    UploadAttachments(attachs);
                }
                else
                {
                    this.label1.Text = "上传附件...";
                    this.label1.Tag = null;
                }
            }
            f.Dispose();
        }

        private void UploadAttachments(List<Attachment> attachs)
        {
            if (attachs != null && attachs.Count > 0)
            {
                AttachmentLogic al = AttachmentLogic.GetInstance();
                SolidBrush brush = new SolidBrush(Color.FromArgb(100, Color.Green));
                SolidBrush errBrush = new SolidBrush(Color.FromArgb(100, Color.Red));
                int count = attachs.Count;
                int size = label1.Width / count;
                Rectangle rect = new Rectangle(0, 0, size, label1.Height);
                int err = 0;
                for (int i = 0; i < attachs.Count; i++)
                {
                    rect.X = size * i;
                    Attachment attach = attachs[i];
                    string dir = "";
                    if (attach.Uploader != null)
                        dir = attach.Uploader.ID.ToString();
                    if (KellFileTransfer.FileUploader.SendFile(attach.AttachmentFilename, KellFileTransfer.Common.GetUploadIPEndPoint(), dir))
                    {
                        string filename = Path.GetFileName(attach.AttachmentFilename);
                        Attachment a = new Attachment();
                        a.AttachmentFilename = dir + "\\" + filename;
                        a.Size = attach.Size;
                        a.Uploader = attach.Uploader;
                        int id = al.AddAttachment(a);
                        if (id > 0)
                        {
                            attach.ID = id;
                            attach.AttachmentFilename = a.AttachmentFilename;
                            using (Graphics g = label1.CreateGraphics())
                            {
                                g.FillRectangle(brush, rect);
                            }
                        }
                        else
                        {
                            err++;
                            using (Graphics g = label1.CreateGraphics())
                            {
                                g.FillRectangle(errBrush, rect);
                            }
                        }
                    }
                    else
                    {
                        err++;
                        using (Graphics g = label1.CreateGraphics())
                        {
                            g.FillRectangle(errBrush, rect);
                        }
                    }
                }
                brush.Dispose();
                errBrush.Dispose();
                string errStr = "";
                if (err > 0)
                    errStr = "但是有1个或多个附件上传失败。可能是保存附件记录到数据库失败，也可能是主机终结点[" + KellFileTransfer.Common.GetUploadIPEndPoint().ToString() + "]尚未开始服务...";
                MessageBox.Show("上传完毕！" + errStr);
            }
        }

        private void label1_MouseHover(object sender, EventArgs e)
        {
            toolTip1.SetToolTip(label1, label1.Text);
        }

        private void 删除附件ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确定要删除这些附件吗？", "删除提醒", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                    this.label1.Tag = null;
                    this.label1.Text = "上传附件...";
            }
        }
    }
}
