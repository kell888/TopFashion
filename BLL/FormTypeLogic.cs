using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace TopFashion
{
    public class FormTypeLogic
    {        
        SQLDBHelper sqlHelper;
        static FormTypeLogic instance;
        public static FormTypeLogic GetInstance()
        {
            if (instance == null)
                instance = new FormTypeLogic();

            return instance;
        }

        private FormTypeLogic()
        {
            sqlHelper = new SQLDBHelper();
        }

        public FormType GetFormType(int id)
        {
            string sql = "select * from TF_FormType where ID=" + id;
            DataTable dt = sqlHelper.Query(sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                FormType element = new FormType();
                element.ID = id;
                element.TypeName = dt.Rows[0]["TypeName"].ToString();
                element.Flag = Convert.ToInt32(dt.Rows[0]["Flag"]);
                element.Remark = dt.Rows[0]["Remark"].ToString();
                return element;
            }
            return null;
        }

        public List<FormType> GetAllFormTypes()
        {
            List<FormType> elements = new List<FormType>();
            string sql = "select * from TF_FormType";
            DataTable dt = sqlHelper.Query(sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    FormType element = new FormType();
                    element.ID = Convert.ToInt32(dt.Rows[i]["ID"]);
                    element.TypeName = dt.Rows[i]["TypeName"].ToString();
                    element.Flag = Convert.ToInt32(dt.Rows[i]["Flag"]);
                    element.Remark = dt.Rows[i]["Remark"].ToString();
                    elements.Add(element);
                }
            }
            return elements;
        }

        public int AddFormType(FormType element)
        {
            string sql = "insert into TF_FormType (TypeName, Remark) values ('" + element.TypeName + "', '" + element.Remark + "'); select SCOPE_IDENTITY()";
            object obj = sqlHelper.ExecuteSqlReturn(sql);
            int R;
            if (obj != null && obj != DBNull.Value && int.TryParse(obj.ToString(), out R))
                return R;
            else
                return 0;
        }

        public bool UpdateFormType(FormType element, User user)
        {
            int adminId = Common.AdminId;
            if (user.ID != adminId)
                return false;
            string sql = "update TF_FormType set TypeName='" + element.TypeName + "', Remark='" + element.Remark + "' where ID=" + element.ID;
            int r = sqlHelper.ExecuteSql(sql);
            return r > 0;
        }

        public bool SetFlag(int id, int flag)
        {
            string sql = "update TF_FormType set Flag=" + flag + " where ID=" + id;
            int r = sqlHelper.ExecuteSql(sql);
            return r > 0;
        }

        public bool DeleteFormType(FormType element, User user)
        {
            int adminId = Common.AdminId;
            if (user.ID != adminId)
                return false;
            string sql = "delete from TF_FormType where ID=" + element.ID;
            int r = sqlHelper.ExecuteSql(sql);
            return r > 0;
        }
        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public bool UpgradeList(List<FormType> list, User user)
        {
            int adminId = Common.AdminId;
            if (user.ID != adminId)
                return false;
            int errCount = 0;
            foreach (FormType element in list)
            {
                string sqlStr = "if exists (select 1 from TF_FormType where ID=" + element.ID + ") update TF_FormType set TypeName='" + element.TypeName + "', Flag=" + element.Flag + ", Remark='" + element.Remark + "' where ID=" + element.ID + " else insert into TF_FormType (TypeName, Flag, Remark) values ('" + element.TypeName + "', " + element.Flag + ", '" + element.Remark + "')";
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
            return sqlHelper.Exists("select 1 from TF_FormType where TypeName='" + name + "'");
        }

        /// <summary>
        /// 是否存在出了自己以外的同名
        /// </summary>
        /// <param name="name"></param>
        /// <param name="myId"></param>
        /// <returns></returns>
        public bool ExistsNameOther(string name, int myId)
        {
            return sqlHelper.Exists("select 1 from TF_FormType where ID!=" + myId + " and TypeName='" + name + "'");
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
                return sqlHelper.Exists("select 1 from TF_FormType " + w);
            }
            return false;
        }

        public DataTable GetFormTypes(string where)
        {
            DataTable dt = null;
            string w = "";
            if (!string.IsNullOrEmpty(where))
            {
                w = where.Trim().ToLower();
                if (!w.StartsWith("where "))
                    w = "where " + w;
            }
            string sql = "select * from TF_FormType " + w;
            dt = sqlHelper.Query(sql);
            return dt;
        }
    }
}
