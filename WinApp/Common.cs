using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.IO;

namespace TopFashion
{
    public static class Common
    {
        public static void UploadAttachment(string localFilename, User user)
        {
            try
            {
                KellFileTransfer.FILELIST file = KellFileTransfer.Common.GetFILE(localFilename);
                if (KellFileTransfer.FileUploader.SendFile(localFilename, KellFileTransfer.Common.GetUploadIPEndPoint()))
                {
                    Attachment attch = new Attachment();
                    attch.AttachmentFilename = file.文件路径;
                    attch.Size = file.文件大小;
                    attch.Uploader = user;
                    int r = AttachmentLogic.GetInstance().AddAttachment(attch);
                    string s = r > 0 ? "":"，但保存到数据库失败！";
                    MessageBox.Show("上传成功！" + s);
                }
                else
                {
                    MessageBox.Show("上传失败！");
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("上传时出错：" + e.Message);
            }
        }

        public static void DownloadAttachment(string attachmentFileName, long size, IPEndPoint ipep = null)
        {
            try
            {
                KellFileTransfer.FileDownloadClient download = new KellFileTransfer.FileDownloadClient();
                download.DownloadFinishedSingle += new KellFileTransfer.DownloadSingleHandler(download_DownloadFinishedSingle);
                KellFileTransfer.FILELIST attachment = new KellFileTransfer.FILELIST();
                attachment.文件路径 = attachmentFileName;
                attachment.文件大小 = size;
                if (ipep == null)
                    ipep = KellFileTransfer.Common.GetDownloadIPEndPoint();
                if (!download.DownloadFileFromServer(attachment, ipep.Address, ipep.Port))
                    MessageBox.Show("下载失败！");
            }
            catch (Exception e)
            {
                MessageBox.Show("下载时出错：" + e.Message);
            }
        }

        static void download_DownloadFinishedSingle(KellFileTransfer.FileDownloadClient sender, string fileFullPath)
        {
            if (File.Exists(fileFullPath))
            {
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Title = "附件另存为...";
                string ext = Path.GetExtension(fileFullPath);
                sfd.Filter = "文件(*" + ext + ")|*" + ext;
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        File.Move(fileFullPath, sfd.FileName);
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show("下载成功，但移动文件到目录[" + sfd.FileName + "]失败：" + e.Message);
                    }
                }
                sfd.Dispose();
            }
            else
            {
                MessageBox.Show("下载附件到本地后，却找不到该文件[" + fileFullPath + "]！");
            }
        }
    }
}
