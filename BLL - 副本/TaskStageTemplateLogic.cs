using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using KellWorkFlow;

namespace TopFashion
{
    public class TaskStageTemplateLogic
    {        
        SQLDBHelper sqlHelper;
        static TaskStageTemplateLogic instance;
        public static TaskStageTemplateLogic GetInstance()
        {
            if (instance == null)
                instance = new TaskStageTemplateLogic();

            return instance;
        }

        private TaskStageTemplateLogic()
        {
            sqlHelper = new SQLDBHelper();
        }

        public TaskStageTemplate GetTaskStageTemplate(int id)
        {
            string sql = "select * from TaskStageTemplate where ID=" + id;
            DataTable dt = sqlHelper.Query(sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                TaskStageTemplate element = new TaskStageTemplate(Convert.ToInt32(dt.Rows[0]["ID"]), dt.Rows[0]["Name"].ToString(), TaskStage.GetUsers(dt.Rows[0]["Executors"].ToString()), TaskStage.GetUsers(dt.Rows[0]["Approvers"].ToString()));
                return element;
            }
            return null;
        }

        public List<TaskStageTemplate> GetAllTaskStageTemplates()
        {
            List<TaskStageTemplate> elements = new List<TaskStageTemplate>();
            string sql = "select * from TaskStageTemplate";
            DataTable dt = sqlHelper.Query(sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    TaskStageTemplate element = new TaskStageTemplate(Convert.ToInt32(dt.Rows[i]["ID"]), dt.Rows[i]["Name"].ToString(), TaskStage.GetUsers(dt.Rows[i]["Executors"].ToString()), TaskStage.GetUsers(dt.Rows[i]["Approvers"].ToString()));
                    elements.Add(element);
                }
            }
            return elements;
        }

        public List<TaskStageTemplate> GetTaskStageTemplatesByExecutor(User user)
        {
            List<TaskStageTemplate> elements = new List<TaskStageTemplate>();
            string sql = "select * from TaskStageTemplate where ','+Executors+',' like '%," + user.ID + ",%'";
            DataTable dt = sqlHelper.Query(sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    TaskStageTemplate element = new TaskStageTemplate(Convert.ToInt32(dt.Rows[i]["ID"]), dt.Rows[i]["Name"].ToString(), TaskStage.GetUsers(dt.Rows[i]["Executors"].ToString()), TaskStage.GetUsers(dt.Rows[i]["Approvers"].ToString()));
                    elements.Add(element);
                }
            }
            return elements;
        }

        public List<TaskStageTemplate> GetTaskStageTemplatesByApprover(User user)
        {
            List<TaskStageTemplate> elements = new List<TaskStageTemplate>();
            string sql = "select * from TaskStageTemplate where ','+Approvers+',' like '%," + user.ID + ",%'";
            DataTable dt = sqlHelper.Query(sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    TaskStageTemplate element = new TaskStageTemplate(Convert.ToInt32(dt.Rows[i]["ID"]), dt.Rows[i]["Name"].ToString(), TaskStage.GetUsers(dt.Rows[i]["Executors"].ToString()), TaskStage.GetUsers(dt.Rows[i]["Approvers"].ToString()));
                    elements.Add(element);
                }
            }
            return elements;
        }

        public int AddTaskStageTemplate(TaskStageTemplate element)
        {
            string sql = "insert into TaskStageTemplate (Name, Executors, Approvers) values ('" + element.Name + "', '" + TaskStage.GetUserIds(element.Executors) + "', '" + TaskStage.GetUserIds(element.Approvers) + "'); select SCOPE_IDENTITY()";
            object obj = sqlHelper.ExecuteSqlReturn(sql);
            int R;
            if (obj != null && obj != DBNull.Value && int.TryParse(obj.ToString(), out R))
                return R;
            else
                return 0;
        }

        public bool UpdateTaskStageTemplate(TaskStageTemplate element)
        {
            string sql = "update TaskStageTemplate set Name='" + element.Name + "', Executors='" + element.Executors + "', Approvers='" + element.Approvers + "' where ID=" + element.ID;
            int r = sqlHelper.ExecuteSql(sql);
            return r > 0;
        }

        public bool DeleteTaskStageTemplate(TaskStageTemplate element)
        {
            string sql = "delete from TaskStageTemplate where ID=" + element.ID;
            int r = sqlHelper.ExecuteSql(sql);
            return r > 0;
        }
        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public bool UpgradeList(List<TaskStageTemplate> list)
        {
            int errCount = 0;
            foreach (TaskStageTemplate element in list)
            {
                string sqlStr = "if exists (select 1 from TaskStageTemplate where ID=" + element.ID + ") update TaskStageTemplate set Name='" + element.Name + "', Executors='" + element.Executors + "', Approvers='" + element.Approvers + "' where ID=" + element.ID + " else insert into TaskStageTemplate (Name, Executors, Approvers) values ('" + element.Name + "', '" + TaskStage.GetUserIds(element.Executors) + "', '" + TaskStage.GetUserIds(element.Approvers) + "')";
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
            return sqlHelper.Exists("select 1 from TaskStageTemplate where Name='" + name + "'");
        }

        /// <summary>
        /// 是否存在出了自己以外的同名
        /// </summary>
        /// <param name="name"></param>
        /// <param name="myId"></param>
        /// <returns></returns>
        public bool ExistsNameOther(string name, int myId)
        {
            return sqlHelper.Exists("select 1 from TaskStageTemplate where ID!=" + myId + " and Name='" + name + "'");
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
                return sqlHelper.Exists("select 1 from TaskStageTemplate " + w);
            }
            return false;
        }
    }
}
