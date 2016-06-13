using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace TopFashion
{
    public class FormObjectLogic
    {        
        SQLDBHelper sqlHelper;
        static FormObjectLogic instance;
        public static FormObjectLogic GetInstance()
        {
            if (instance == null)
                instance = new FormObjectLogic();

            return instance;
        }

        private FormObjectLogic()
        {
            sqlHelper = new SQLDBHelper();
        }

        public FormObject GetFormObject(int id)
        {
            string sql = "select * from TF_FormObject where ID=" + id;
            DataTable dt = sqlHelper.Query(sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                FormObject element = new FormObject();
                element.ID = id;
                element.FormName = dt.Rows[0]["FormName"].ToString();
                element.FormType = FormTypeLogic.GetInstance().GetFormType(Convert.ToInt32(dt.Rows[0]["FormType"]));
                element.FormItems = FormItemLogic.GetInstance().GetFormItemsByIds(dt.Rows[0]["FormItems"].ToString());
                element.Owner = UserLogic.GetInstance().GetUser(Convert.ToInt32(dt.Rows[0]["Owner"]));
                element.Remark = dt.Rows[0]["Remark"].ToString();
                return element;
            }
            return null;
        }

        public List<FormObject> GetAllFormObjects()
        {
            List<FormObject> elements = new List<FormObject>();
            string sql = "select * from TF_FormObject";
            DataTable dt = sqlHelper.Query(sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                FormTypeLogic ftl = FormTypeLogic.GetInstance();
                FormItemLogic fil = FormItemLogic.GetInstance();
                UserLogic ul = UserLogic.GetInstance();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    FormObject element = new FormObject();
                    element.ID = Convert.ToInt32(dt.Rows[i]["ID"]);
                    element.FormName = dt.Rows[i]["FormName"].ToString();
                    element.FormType = ftl.GetFormType(Convert.ToInt32(dt.Rows[i]["FormType"]));
                    element.FormItems = fil.GetFormItemsByIds(dt.Rows[i]["FormItems"].ToString());
                    element.Owner = ul.GetUser(Convert.ToInt32(dt.Rows[i]["Owner"]));
                    element.Remark = dt.Rows[i]["Remark"].ToString();
                    elements.Add(element);
                }
            }
            return elements;
        }

        public List<FormObject> GetFormObjectsByUser(User user)
        {
            List<FormObject> elements = new List<FormObject>();
            string sql = "select * from TF_FormObject where Owner=" + user.ID;
            DataTable dt = sqlHelper.Query(sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                FormTypeLogic ftl = FormTypeLogic.GetInstance();
                FormItemLogic fil = FormItemLogic.GetInstance();
                UserLogic ul = UserLogic.GetInstance();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    FormObject element = new FormObject();
                    element.ID = Convert.ToInt32(dt.Rows[i]["ID"]);
                    element.FormName = dt.Rows[i]["FormName"].ToString();
                    element.FormType = ftl.GetFormType(Convert.ToInt32(dt.Rows[i]["FormType"]));
                    element.FormItems = fil.GetFormItemsByIds(dt.Rows[i]["FormItems"].ToString());
                    element.Owner = ul.GetUser(Convert.ToInt32(dt.Rows[i]["Owner"]));
                    element.Remark = dt.Rows[i]["Remark"].ToString();
                    elements.Add(element);
                }
            }
            return elements;
        }

        public int AddFormObject(FormObject element)
        {
            string s = "";
            if (element.Owner != null)
                s = element.Owner.ID.ToString();
            else
                s = "0";
            string sql = "insert into TF_FormObject (FormName, FormType, FormItems, Owner, Remark) values ('" + element.FormName + "', " + element.FormType.ID + ", '" + FormItemLogic.GetItemString(element.FormItems) + "', " + s + ", '" + element.Remark + "'); select SCOPE_IDENTITY()";
            object obj = sqlHelper.ExecuteSqlReturn(sql);
            int R;
            if (obj != null && obj != DBNull.Value && int.TryParse(obj.ToString(), out R))
                return R;
            else
                return 0;
        }
        /// <summary>
        /// 快速复制一份表单模板（表单名在原名后加上时间，返回新表单的ID）
        /// </summary>
        /// <param name="element">原表单</param>
        /// <param name="newFormName">复制后的新的表单名</param>
        /// <returns></returns>
        public int CopyFormObject(FormObject element, out string newFormName)
        {
            if (element.FormName.Length > 20)
                element.FormName = element.FormName.Substring(0, element.FormName.Length - 17) + DateTime.Now.ToString("yyMMdd_HHmmss");
            else
                element.FormName = element.FormName + DateTime.Now.ToString("yyMMdd_HHmmss");
            newFormName = element.FormName;
            return AddFormObject(element);
        }

        public bool UpdateFormObject(FormObject element, User user)
        {
            if (user == null)
                return false;
            string s = "(1>1)";
            int adminId = Common.AdminId;
            if (user.ID == adminId)
                s = "(1=1)";
            string sql = "update TF_FormObject set FormName='" + element.FormName + "', FormType=" + element.FormType.ID + ", FormItems='" + FormItemLogic.GetItemString(element.FormItems) + "', Remark='" + element.Remark + "' where ID=" + element.ID + " and (Owner=" + user.ID + " or " + s + ")";
            int r = sqlHelper.ExecuteSql(sql);
            return r > 0;
        }

        public bool DeleteFormObject(FormObject element, User user)
        {
            if (user == null)
                return false;
            string s = "(1>1)";
            int adminId = Common.AdminId;
            if (user.ID == adminId)
                s = "(1=1)";
            string sql = "delete from TF_FormObject where ID=" + element.ID + " and (Owner=" + user.ID + " or " + s + ")";
            int r = sqlHelper.ExecuteSql(sql);
            if (r > 0)
            {
                FormItemLogic.GetInstance().DeleteFormItems(FormItemLogic.GetItemString(element.FormItems));
            }
            return r > 0;
        }
        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public bool UpgradeList(List<FormObject> list, User user)
        {
            if (user == null)
                return false;
            string s = "(1>1)";
            int adminId = Common.AdminId;
            if (user.ID == adminId)
                s = "(1=1)";
            int errCount = 0;
            foreach (FormObject element in list)
            {
                string sqlStr = "if exists (select 1 from TF_FormObject where ID=" + element.ID + ") update TF_FormObject set FormName='" + element.FormName + "', FormType=" + element.FormType.ID + ", FormItems='" + FormItemLogic.GetItemString(element.FormItems) + "', Remark='" + element.Remark + "' where ID=" + element.ID + " and (Owner=" + user.ID + " or " + s + ") else insert into TF_FormObject (FormName, FormType, FormItems, Owner, Remark) values ('" + element.FormName + "', " + element.FormType.ID + ", '" + FormItemLogic.GetItemString(element.FormItems) + "', " + element.Owner.ID + ", '" + element.Remark + "')";
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
            return sqlHelper.Exists("select 1 from TF_FormObject where FormName='" + name + "'");
        }

        /// <summary>
        /// 是否存在出了自己以外的同名
        /// </summary>
        /// <param name="name"></param>
        /// <param name="myId"></param>
        /// <returns></returns>
        public bool ExistsNameOther(string name, int myId)
        {
            return sqlHelper.Exists("select 1 from TF_FormObject where ID!=" + myId + " and FormName='" + name + "'");
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
                return sqlHelper.Exists("select 1 from TF_FormObject " + w);
            }
            return false;
        }

        public DataTable GetFormObjects(string where)
        {
            DataTable dt = null;
            string w = "";
            if (!string.IsNullOrEmpty(where))
            {
                w = where.Trim().ToLower();
                if (!w.StartsWith("where "))
                    w = "where " + w;
            }
            string sql = "select * from TF_FormObject " + w;
            dt = sqlHelper.Query(sql);
            return dt;
        }
    }
}
