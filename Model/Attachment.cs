using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace TopFashion
{
    [Serializable]
    public class Attachment
    {
        int id;

        public int ID
        {
            get { return id; }
            set { id = value; }
        }
        string attachmentFilename;

        public string AttachmentFilename
        {
            get { return attachmentFilename; }
            set { attachmentFilename = value; }
        }
        long size;

        public long Size
        {
            get { return size; }
            set { size = value; }
        }
        User uploader;

        public User Uploader
        {
            get { return uploader; }
            set { uploader = value; }
        }
        DateTime uploadTime;

        public DateTime UploadTime
        {
            get { return uploadTime; }
            set { uploadTime = value; }
        }
        int flag;
        /// <summary>
        /// 下载次数
        /// </summary>
        public int Flag
        {
            get { return flag; }
            set { flag = value; }
        }
        /// <summary>
        /// 显示当前附件的全面信息
        /// </summary>
        public string AttachmentInfo
        {
            get
            {
                string name = "[未知附件]";
                if (!string.IsNullOrEmpty(attachmentFilename))
                {
                    name = Path.GetFileName(attachmentFilename);
                }
                string upload = "[未知上传人]";
                if (uploader != null)
                    upload = uploader.Username;
                return name + "(大小：" + size + "，上传人：" + upload + "，下载次数：" + flag + ")";
            }
        }

        public override string ToString()
        {
            if (!string.IsNullOrEmpty(attachmentFilename))
                return this.ID + "|" + Path.GetFileName(attachmentFilename);
            else
                return "0|[未知附件]";
        }
    }
}
