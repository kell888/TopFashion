using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Net;
using System.Diagnostics;

namespace TopFashion
{
    [Serializable]
    public partial class DownloadControl : UserControl
    {
        public DownloadControl()
        {
            InitializeComponent();
        }

        int selectedCount;

        public void LoadAttachments(List<Attachment> attachs)
        {
            if (attachs != null && attachs.Count > 0)
            {
                listBox1.Tag = attachs;
                listBox1.Items.Clear();
                foreach (Attachment attach in attachs)
                {
                    listBox1.Items.Add(attach.AttachmentInfo);
                }
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex > -1)
            {
                button1.Enabled = true;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndices.Count > 0)
            {
                List<Attachment> all = listBox1.Tag as List<Attachment>;
                List<Attachment> selected = new List<Attachment>();
                foreach (int index in listBox1.SelectedIndices)
                {
                    selected.Add(all[index]);
                }
                DownloadAttachments(selected);
            }
            else
            {
                MessageBox.Show("请先选择要下载的附件！");
            }
        }

        private void DownloadAttachments(List<Attachment> attachs)
        {
            selectedCount = attachs.Count;
            IPEndPoint ipep = KellFileTransfer.Common.GetDownloadIPEndPoint();
            KellFileTransfer.FileDownloadClient client = new KellFileTransfer.FileDownloadClient();
            client.DownloadFinishedSingle += new KellFileTransfer.DownloadSingleHandler(client_DownloadFinishedSingle);
            client.DownloadingError += new KellFileTransfer.DownloadErrorHandler(client_DownloadingError);
            foreach (Attachment attach in attachs)
            {
                User user = attach.Uploader;
                //以下为禁止非原上传用户下载附件的代码，由于现在运行所有用户下载查看附件，所以屏蔽！
                //int index = attach.AttachmentFilename.IndexOf("\\");// 322\123.abc
                //if (index > 0 && user != null)
                //{
                //    string userId = attach.AttachmentFilename.Substring(0, index);
                //    int R;
                //    if (int.TryParse(userId, out R))
                //    {
                //        if (user.ID != R)
                //        {
                //            continue;//非法用户
                //        }
                //    }
                //}
                KellFileTransfer.FILELIST file = new KellFileTransfer.FILELIST();
                if (user != null)
                    file.文件路径 = user.ID + "\\" + Path.GetFileName(attach.AttachmentFilename);
                else
                    file.文件路径 = Path.GetFileName(attach.AttachmentFilename);
                file.文件大小 = attach.Size;
                bool f = client.DownloadFileFromServer(file, ipep.Address, ipep.Port);
                if (!f)
                {
                    MessageBox.Show("下载附件失败！很有可能连接到附件服务器的网络已断开。");
                }
            }
            button1.Refresh();
            //SolidBrush Backbrush = new SolidBrush(SystemColors.Control);
            //SolidBrush Forebrush = new SolidBrush(SystemColors.ControlText);
            //Rectangle rect = new Rectangle(0, 0, button1.Width, button1.Height);
            //using (Graphics g = button1.CreateGraphics())
            //{
            //    g.FillRectangle(Backbrush, rect);
            //    SizeF size = g.MeasureString("下载附件",button1.Font);
            //    PointF point = new PointF(button1.Width / 2 - size.Width / 2, button1.Height / 2 - size.Height / 2);
            //    g.DrawString("下载附件", button1.Font, Forebrush, point);
            //}
            //Backbrush.Dispose();
            //Forebrush.Dispose();
        }

        void client_DownloadingError(KellFileTransfer.FileDownloadClient sender, Exception e)
        {
            SolidBrush errBrush = new SolidBrush(Color.FromArgb(100, Color.Red));
            int size = button1.Width / selectedCount;
            Rectangle rect = new Rectangle(0, 0, size, button1.Height);
            using (Graphics g = button1.CreateGraphics())
            {
                g.FillRectangle(errBrush, rect);
            }
            errBrush.Dispose();
            MessageBox.Show("下载附件[" + sender.FileName + "]时出错：" + e.Message);
        }

        void client_DownloadFinishedSingle(KellFileTransfer.FileDownloadClient sender, string fileFullPath)
        {
            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Title = "确定将附件保存到指定位置，取消就立即打开...";
                string ext = Path.GetExtension(fileFullPath);
                if (!string.IsNullOrEmpty(ext))
                    sfd.Filter = "文件(*" + ext + ")|*" + ext;
                else
                    sfd.Filter = "文件(*.*)|*.*";
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        File.Move(fileFullPath, sfd.FileName);
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show("下载成功，但另存附件到目录[" + sfd.FileName + "]失败：" + e.Message);
                    }
                }
                else
                {
                    try
                    {
                        Process.Start(fileFullPath);
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show("下载附件成功，但打开附件[" + fileFullPath + "]时出错：" + e.Message);
                    }
                }
            }
            SolidBrush brush = new SolidBrush(Color.FromArgb(100, Color.Green));
            int size = button1.Width / selectedCount;
            Rectangle rect = new Rectangle(0, 0, size, button1.Height);
            using (Graphics g = button1.CreateGraphics())
            {
                g.FillRectangle(brush, rect);
            }
            brush.Dispose();
        }
    }
}
