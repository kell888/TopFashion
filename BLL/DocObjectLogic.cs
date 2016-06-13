using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace TopFashion
{
    public class DocObjectLogic
    {        
        SQLDBHelper sqlHelper;
        static DocObjectLogic instance;
        public static DocObjectLogic GetInstance()
        {
            if (instance == null)
                instance = new DocObjectLogic();

            return instance;
        }

        private DocObjectLogic()
        {
            sqlHelper = new SQLDBHelper();
        }

        public DocObject GetDocObject(int id)
        {
            string sql = "select * from TF_DocObject where ID=" + id;
            DataTable dt = sqlHelper.Query(sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                DocObject element = new DocObject();
                element.ID = id;
                element.Name = dt.Rows[0]["Name"].ToString();
                element.Form = FormObjectLogic.GetInstance().GetFormObject(Convert.ToInt32(dt.Rows[0]["FormID"]));
                element.DocItems = FormItemLogic.GetInstance().GetFormItemsByIds(dt.Rows[0]["DocItems"].ToString());
                element.Owner = UserLogic.GetInstance().GetUser(Convert.ToInt32(dt.Rows[0]["Owner"]));
                element.Remark = dt.Rows[0]["Remark"].ToString();
                element.Flag = Convert.ToInt32(dt.Rows[0]["Flag"]);
                return element;
            }
            return null;
        }

        public List<DocObject> GetAllDocObjects()
        {
            List<DocObject> elements = new List<DocObject>();
            string sql = "select * from TF_DocObject";
            DataTable dt = sqlHelper.Query(sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                FormObjectLogic fol = FormObjectLogic.GetInstance();
                FormItemLogic fil = FormItemLogic.GetInstance();
                UserLogic ul = UserLogic.GetInstance();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DocObject element = new DocObject();
                    element.ID = Convert.ToInt32(dt.Rows[i]["ID"]);
                    element.Name = dt.Rows[i]["Name"].ToString();
                    element.Form = fol.GetFormObject(Convert.ToInt32(dt.Rows[i]["FormID"]));
                    element.DocItems = fil.GetFormItemsByIds(dt.Rows[i]["DocItems"].ToString());
                    element.Owner = ul.GetUser(Convert.ToInt32(dt.Rows[i]["Owner"]));
                    element.Remark = dt.Rows[i]["Remark"].ToString();
                    element.Flag = Convert.ToInt32(dt.Rows[i]["Flag"]);
                    elements.Add(element);
                }
            }
            return elements;
        }

        public List<DocObject> GetDocObjectsByOwner(User user)
        {
            List<DocObject> elements = new List<DocObject>();
            string sql = "select * from TF_DocObject where Owner=" + user.ID;
            DataTable dt = sqlHelper.Query(sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                FormObjectLogic fol = FormObjectLogic.GetInstance();
                FormItemLogic fil = FormItemLogic.GetInstance();
                UserLogic ul = UserLogic.GetInstance();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DocObject element = new DocObject();
                    element.ID = Convert.ToInt32(dt.Rows[i]["ID"]);
                    element.Name = dt.Rows[i]["Name"].ToString();
                    element.Form = fol.GetFormObject(Convert.ToInt32(dt.Rows[i]["FormID"]));
                    element.DocItems = fil.GetFormItemsByIds(dt.Rows[i]["DocItems"].ToString());
                    element.Owner = ul.GetUser(Convert.ToInt32(dt.Rows[i]["Owner"]));
                    element.Remark = dt.Rows[i]["Remark"].ToString();
                    element.Flag = Convert.ToInt32(dt.Rows[i]["Flag"]);
                    elements.Add(element);
                }
            }
            return elements;
        }

        public List<DocObject> GetDocObjectsByTemplateId(List<int> flowTemplateIds)
        {
            List<DocObject> elements = new List<DocObject>();
            if (flowTemplateIds != null && flowTemplateIds.Count > 0)
            {
                string sql = "select TF_DocObject.* from TF_DocObject,TaskInfo,Flow,FlowTemplate where TF_DocObject.ID=TaskInfo.EntityId and TaskInfo.FlowID=Flow.ID and Flow.TemplateID in (" + flowTemplateIds + ")";
                DataTable dt = sqlHelper.Query(sql);
                if (dt != null && dt.Rows.Count > 0)
                {
                    FormObjectLogic fol = FormObjectLogic.GetInstance();
                    FormItemLogic fil = FormItemLogic.GetInstance();
                    UserLogic ul = UserLogic.GetInstance();
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DocObject element = new DocObject();
                        element.ID = Convert.ToInt32(dt.Rows[i]["ID"]);
                        element.Name = dt.Rows[i]["Name"].ToString();
                        element.Form = fol.GetFormObject(Convert.ToInt32(dt.Rows[i]["FormID"]));
                        element.DocItems = fil.GetFormItemsByIds(dt.Rows[i]["DocItems"].ToString());
                        element.Owner = ul.GetUser(Convert.ToInt32(dt.Rows[i]["Owner"]));
                        element.Remark = dt.Rows[i]["Remark"].ToString();
                        element.Flag = Convert.ToInt32(dt.Rows[i]["Flag"]);
                        elements.Add(element);
                    }
                }
            }
            return elements;
        }

        public int AddDocObject(DocObject element)
        {
            string s = "";
            if (element.Owner != null)
                s = element.Owner.ID.ToString();
            else
                s = "0";
            string sql = "insert into TF_DocObject (Name, FormID, DocItems, Owner, Remark) values ('" + element.Name + "', " + element.Form.ID + ", '" + FormItemLogic.GetItemString(element.DocItems) + "', " + s + ", '" + element.Remark + "'); select SCOPE_IDENTITY()";
            object obj = sqlHelper.ExecuteSqlReturn(sql);
            int R;
            if (obj != null && obj != DBNull.Value && int.TryParse(obj.ToString(), out R))
                return R;
            else
                return 0;
        }
        /// <summary>
        /// 快速复制一份文档（文档名在原名后加上时间，返回新文档的ID）
        /// </summary>
        /// <param name="element">原表单</param>
        /// <param name="newDocName">复制后的新的文档名</param>
        /// <returns></returns>
        public int CopyDocObject(DocObject element, out string newDocName)
        {
            if (element.Name.Length > 20)
                element.Name = element.Name.Substring(0, element.Name.Length - 17) + DateTime.Now.ToString("yy_MM_dd_HH_mm_ss");
            else
                element.Name = element.Name + DateTime.Now.ToString("yy_MM_dd_HH_mm_ss");
            newDocName = element.Name;
            return AddDocObject(element);
        }

        public bool UpdateDocObject(DocObject element, User user)
        {
            if (user == null)
                return false;
            string s = "(1>1)";
            int adminId = Common.AdminId;
            if (user.ID == adminId)
                s = "(1=1)";
            string sql = "update TF_DocObject set Name='" + element.Name + "', FormID=" + element.Form.ID + ", DocItems='" + FormItemLogic.GetItemString(element.DocItems) + "', Remark='" + element.Remark + "' where ID=" + element.ID + " and (Owner=" + user.ID + " or " + s + ")";
            int r = sqlHelper.ExecuteSql(sql);
            return r > 0;
        }

        public bool DeleteDocObject(DocObject element, User user)
        {
            if (user == null)
                return false;
            string s = "(1>1)";
            int adminId = Common.AdminId;
            if (user.ID == adminId)
                s = "(1=1)";
            string sql = "delete from TF_DocObject where ID=" + element.ID + " and (Owner=" + user.ID + " or " + s + ")";
            int r = sqlHelper.ExecuteSql(sql);
            if (r > 0)
            {
                FormItemLogic.GetInstance().DeleteFormItems(FormItemLogic.GetItemString(element.DocItems));//删除字段信息
                AttachmentLogic al = AttachmentLogic.GetInstance();
                foreach (FormItem item in element.DocItems)
                {
                    if (item.ItemType == "System.Object")//是附件，顺带把附件记录也删除
                    {
                        string[] ss = item.ItemValue.Split("|".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                        if (ss.Length > 1)
                        {
                            int id;
                            if (int.TryParse(ss[0], out id))
                            {
                                al.DeleteAttachment(id);
                            }
                        }
                    }
                }
            }
            return r > 0;
        }
        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public bool UpgradeList(List<DocObject> list, User user)
        {
            if (user == null)
                return false;
            string s = "(1>1)";
            int adminId = Common.AdminId;
            if (user.ID == adminId)
                s = "(1=1)";
            int errCount = 0;
            foreach (DocObject element in list)
            {
                string sqlStr = "if exists (select 1 from TF_DocObject where ID=" + element.ID + ") update TF_DocObject set Name='" + element.Name + "', FormID=" + element.Form.ID + ", DocItems='" + FormItemLogic.GetItemString(element.DocItems) + "', Remark='" + element.Remark + "' where ID=" + element.ID + " and (Owner=" + user.ID + " or " + s + ") else insert into TF_DocObject (Name, FormID, DocItems, Owner, Remark) values ('" + element.Name + "', " + element.Form.ID + ", '" + FormItemLogic.GetItemString(element.DocItems) + "', " + element.Owner.ID + ", '" + element.Remark + "')";
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
            return sqlHelper.Exists("select 1 from TF_DocObject where Name='" + name + "'");
        }

        /// <summary>
        /// 是否存在出了自己以外的同名
        /// </summary>
        /// <param name="name"></param>
        /// <param name="myId"></param>
        /// <returns></returns>
        public bool ExistsNameOther(string name, int myId)
        {
            return sqlHelper.Exists("select 1 from TF_DocObject where ID!=" + myId + " and Name='" + name + "'");
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
                return sqlHelper.Exists("select 1 from TF_DocObject " + w);
            }
            return false;
        }

        public DataTable GetDocObjects(string where)
        {
            DataTable dt = null;
            string w = "";
            if (!string.IsNullOrEmpty(where))
            {
                w = where.Trim().ToLower();
                if (!w.StartsWith("where "))
                    w = "where " + w;
            }
            string sql = "select * from TF_DocObject " + w;
            dt = sqlHelper.Query(sql);
            return dt;
        }

        public bool Increase(int id)
        {
            string sql = "update TF_DocObject set Flag=Flag+1 where ID=" + id;
            int r = sqlHelper.ExecuteSql(sql);
            return r > 0;
        }
    }
}
