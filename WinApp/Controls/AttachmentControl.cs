using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;

namespace TopFashion
{
    public partial class AttachmentControl : UserControl
    {
        public AttachmentControl()
        {
            InitializeComponent();
        }

        public void SetAttachment(Attachment attach)
        {
            if (attach != null)
            {
                this.label1.Tag = attach;
                this.label1.Text = Path.GetFileName(attach.AttachmentFilename);
            }
            else
            {
                this.label1.Text = "[空]";
                this.label1.Tag = null;
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {
            if (this.label1.Tag != null && this.label1.Tag is Attachment)
            {
                if (MessageBox.Show("确定要打开该附件吗？", "打开提醒", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)//"确定要下载该附件到本地吗？", "下载提醒"
                {
                    DownloadAttachment(this.label1.Tag as Attachment);
                }
            }
            else
            {
                MessageBox.Show("该附件为空，无法打开！");//"该附件为空，无法下载！"
            }
        }

        private void DownloadAttachment(Attachment attachment, bool open = true)
        {
            if (attachment != null)
            {
                Commons.DownloadAttachment(open, attachment.Uploader, attachment.ID, attachment.AttachmentFilename, attachment.Size);
            }
        }

        private void label1_MouseHover(object sender, EventArgs e)
        {
            toolTip1.SetToolTip(label1, label1.Text);
        }

        private void 另存为ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.label1.Tag != null && this.label1.Tag is Attachment)
            {
                Attachment atta = this.label1.Tag as Attachment;
                DownloadAttachment(atta, false);
                string fileFullPath = Directory.GetCurrentDirectory() + "\\" + atta.AttachmentFilename;
                if (File.Exists(fileFullPath))
                {
                    using (SaveFileDialog sfd = new SaveFileDialog())
                    {
                        sfd.Title = "确定将附件保存到指定位置...";
                        string ext = Path.GetExtension(fileFullPath);
                        if (!string.IsNullOrEmpty(ext))
                            sfd.Filter = "文件(*" + ext + ")|*" + ext;
                        else
                            sfd.Filter = "文件(*.*)|*.*";
                        if (sfd.ShowDialog() == DialogResult.OK)
                        {
                            File.Move(fileFullPath, sfd.FileName);
                            if (MessageBox.Show("是否立即打开附件？", "打开提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                            {
                                try
                                {
                                    Process.Start(sfd.FileName);
                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show("另存附件成功，但打开附件[" + sfd.FileName + "]时出错：" + ex.Message);
                                }
                            }
                        }
                    }
                }
                else
                {
                    MessageBox.Show("找不到下载的附件！[" + fileFullPath + "]");
                }
            }
            else
            {
                MessageBox.Show("附件为空，无法下载！");
            }
        }
    }
}
