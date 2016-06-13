using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace TopFashion
{
    public class AttachmentLogic
    {
        SQLDBHelper sqlHelper;
        static AttachmentLogic instance;
        public static AttachmentLogic GetInstance()
        {
            if (instance == null)
                instance = new AttachmentLogic();

            return instance;
        }

        private AttachmentLogic()
        {
            sqlHelper = new SQLDBHelper();
        }

        public Attachment GetAttachment(int id)
        {
            string sql = "select * from TF_Attachment where ID=" + id;
            DataTable dt = sqlHelper.Query(sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                Attachment attach = new Attachment();
                attach.ID = id;
                attach.AttachmentFilename = dt.Rows[0]["AttachmentFilename"].ToString();
                attach.Size = Convert.ToInt64(dt.Rows[0]["Size"]);
                attach.Uploader = UserLogic.GetInstance().GetUser(Convert.ToInt32(dt.Rows[0]["Uploader"]));
                attach.UploadTime = Convert.ToDateTime(dt.Rows[0]["UploadTime"]);
                attach.Flag = Convert.ToInt32(dt.Rows[0]["Flag"]);
                return attach;
            }
            return null;
        }

        public List<Attachment> GetAllAttachments()
        {
            List<Attachment> attachs = new List<Attachment>();
            string sql = "select * from TF_Attachment";
            DataTable dt = sqlHelper.Query(sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Attachment attach = new Attachment();
                    attach.ID = Convert.ToInt32(dt.Rows[i]["ID"]);
                    attach.AttachmentFilename = dt.Rows[i]["AttachmentFilename"].ToString();
                    attach.Size = Convert.ToInt64(dt.Rows[i]["Size"]);
                    attach.Uploader = UserLogic.GetInstance().GetUser(Convert.ToInt32(dt.Rows[i]["Uploader"]));
                    attach.UploadTime = Convert.ToDateTime(dt.Rows[i]["UploadTime"]);
                    attach.Flag = Convert.ToInt32(dt.Rows[i]["Flag"]);
                    attachs.Add(attach);
                }
            }
            return attachs;
        }

        public static string GetAttachmentString(List<Attachment> items)
        {
            if (items != null)
            {
                StringBuilder sb = new StringBuilder();
                foreach (Attachment item in items)
                {
                    if (sb.Length == 0)
                        sb.Append(item.ID);
                    else
                        sb.Append("," + item.ID);
                }
                return sb.ToString();
            }
            return "";
        }

        public List<Attachment> GetAttachmentsByIds(string items)
        {
            List<Attachment> attachs = new List<Attachment>();
            if (!string.IsNullOrEmpty(items))
            {
                string[] ss = items.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                if (ss.Length > 0)
                {
                    StringBuilder sb = new StringBuilder();
                    foreach (string s in ss)
                    {
                        string id = s.Trim();
                        int ID;
                        if (int.TryParse(id, out ID))
                        {
                            if (sb.Length == 0)
                                sb.Append(id);
                            else
                                sb.Append("," + id);
                        }
                    }
                    if (sb.Length > 0)
                    {
                        string sql = "select * from TF_Attachment where ID in (" + sb.ToString() + ")";
                        DataTable dt = sqlHelper.Query(sql);
                        if (dt != null && dt.Rows.Count > 0)
                        {
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                Attachment attach = new Attachment();
                                attach.ID = Convert.ToInt32(dt.Rows[i]["ID"]);
                                attach.AttachmentFilename = dt.Rows[i]["AttachmentFilename"].ToString();
                                attach.Size = Convert.ToInt64(dt.Rows[i]["Size"]);
                                attach.Uploader = UserLogic.GetInstance().GetUser(Convert.ToInt32(dt.Rows[i]["Uploader"]));
                                attach.UploadTime = Convert.ToDateTime(dt.Rows[i]["UploadTime"]);
                                attach.Flag = Convert.ToInt32(dt.Rows[i]["Flag"]);
                                attachs.Add(attach);
                            }
                        }
                    }
                }
            }
            return attachs;
        }

        public int AddAttachment(Attachment attach)
        {
            string sql = "insert into TF_Attachment (AttachmentFilename, Size, Uploader) values ('" + attach.AttachmentFilename + "', " + attach.Size + ", " + attach.Uploader.ID + "); select SCOPE_IDENTITY()";
            object obj = sqlHelper.ExecuteSqlReturn(sql);
            int R;
            if (obj != null && obj != DBNull.Value && int.TryParse(obj.ToString(), out R))
                return R;
            else
                return 0;
        }

        public bool UpdateAttachment(Attachment attach)
        {
            string sql = "update TF_Attachment set AttachmentFilename='" + attach.AttachmentFilename + "', Size=" + attach.Size + ", Uploader='" + attach.Uploader.ID + "' where ID=" + attach.ID;
            int r = sqlHelper.ExecuteSql(sql);
            return r > 0;
        }
        /// <summary>
        /// 客户端下载附件后，累计下载的次数
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool AfterDownload(int id)
        {
            string sql = "update TF_Attachment set Flag=Flag+1 where ID=" + id;
            int r = sqlHelper.ExecuteSql(sql);
            return r > 0;
        }

        public bool DeleteAttachment(int id)
        {
            string sql = "delete from TF_Attachment where ID=" + id;
            int r = sqlHelper.ExecuteSql(sql);
            return r > 0;
        }
        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public bool UpgradeList(List<Attachment> list)
        {
            int errCount = 0;
            foreach (Attachment attach in list)
            {
                string sqlStr = "if exists (select 1 from TF_Attachment where ID=" + attach.ID + ") update TF_Attachment set AttachmentFilename='" + attach.AttachmentFilename + "', Size=" + attach.Size + ", Uploader='" + attach.Uploader.ID + "' where ID=" + attach.ID + " else insert into TF_Attachment (AttachmentFilename, Size, Uploader) values ('" + attach.AttachmentFilename + "', Size=" + attach.Size + ", " + attach.Uploader.ID + ")";
                try
                {
                    sqlHelper.ExecuteSql(sqlStr);
                }
                catch (Exception)
                {
                    errCount++;
                }
            }
            return errCount == 0;
        }

        /// <summary>
        /// 是否存在同名
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool ExistsName(string name)
        {
            return sqlHelper.Exists("select 1 from TF_Attachment where Name='" + name + "'");
        }

        /// <summary>
        /// 是否存在出了自己以外的同名
        /// </summary>
        /// <param name="name"></param>
        /// <param name="myId"></param>
        /// <returns></returns>
        public bool ExistsNameOther(string name, int myId)
        {
            return sqlHelper.Exists("select 1 from TF_Attachment where ID!=" + myId + " and Name='" + name + "'");
        }

        /// <summary>
        /// 是否存在指定条件的记录
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool ExistsWhere(string where)
        {
            if (!string.IsNullOrEmpty(where))
            {
                string w = where.Trim().ToLower();
                if (!w.StartsWith("where "))
                    w = "where " + w;
                return sqlHelper.Exists("select 1 from TF_Attachment " + w);
            }
            return false;
        }

        public DataTable GetAttachments(string where)
        {
            DataTable dt = null;
            string w = "";
            if (!string.IsNullOrEmpty(where))
            {
                w = where.Trim().ToLower();
                if (!w.StartsWith("where "))
                    w = "where " + w;
            }
            string sql = "select * from TF_Attachment " + w;
            dt = sqlHelper.Query(sql);
            return dt;
        }
    }
}
