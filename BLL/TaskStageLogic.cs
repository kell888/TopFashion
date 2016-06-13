using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using KellWorkFlow;

namespace TopFashion
{
    public class TaskStageLogic
    {        
        SQLDBHelper sqlHelper;
        static TaskStageLogic instance;
        public static TaskStageLogic GetInstance()
        {
            if (instance == null)
                instance = new TaskStageLogic();

            return instance;
        }

        private TaskStageLogic()
        {
            sqlHelper = new SQLDBHelper();
        }

        public TaskStage GetTaskStage(int id)
        {
            string sql = "select * from TaskStage where ID=" + id;
            DataTable dt = sqlHelper.Query(sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                TaskStageTemplate template = TaskStageTemplateLogic.GetInstance().GetTaskStageTemplate(Convert.ToInt32(dt.Rows[0]["TemplateID"]));
                string ActualAppr = "";
                if (dt.Rows[0]["ActualAppr"] != null && dt.Rows[0]["ActualAppr"] != DBNull.Value)
                    ActualAppr = dt.Rows[0]["ActualAppr"].ToString();
                string ActualExec = "";
                if (dt.Rows[0]["ActualExec"] != null && dt.Rows[0]["ActualExec"] != DBNull.Value)
                    ActualExec = dt.Rows[0]["ActualExec"].ToString();
                DateTime ApprTime = DateTime.MinValue;
                if (dt.Rows[0]["ApprTime"] != null && dt.Rows[0]["ApprTime"] != DBNull.Value)
                    ApprTime = Convert.ToDateTime(dt.Rows[0]["ApprTime"]);
                DateTime ExecTime = DateTime.MinValue;
                if (dt.Rows[0]["ExecTime"] != null && dt.Rows[0]["ExecTime"] != DBNull.Value)
                    ExecTime = Convert.ToDateTime(dt.Rows[0]["ExecTime"]);
                TaskStage element = new TaskStage(Convert.ToInt32(dt.Rows[0]["ID"]), dt.Rows[0]["Name"].ToString(), template, TaskStage.GetTaskStatus(Convert.ToInt32(dt.Rows[0]["TaskStatus"])), ActualExec, ActualAppr, ExecTime, ApprTime, dt.Rows[0]["Remark"].ToString());
                return element;
            }
            return null;
        }

        public List<TaskStage> GetAllTaskStages()
        {
            List<TaskStage> elements = new List<TaskStage>();
            string sql = "select * from TaskStage";
            DataTable dt = sqlHelper.Query(sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                TaskStageTemplateLogic tstl = TaskStageTemplateLogic.GetInstance();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    TaskStageTemplate template = tstl.GetTaskStageTemplate(Convert.ToInt32(dt.Rows[i]["TemplateID"]));
                    string ActualAppr = "";
                    if (dt.Rows[i]["ActualAppr"] != null && dt.Rows[i]["ActualAppr"] != DBNull.Value)
                        ActualAppr = dt.Rows[0]["ActualAppr"].ToString();
                    string ActualExec = "";
                    if (dt.Rows[i]["ActualExec"] != null && dt.Rows[i]["ActualExec"] != DBNull.Value)
                        ActualExec = dt.Rows[0]["ActualExec"].ToString();
                    DateTime ApprTime = DateTime.MinValue;
                    if (dt.Rows[i]["ApprTime"] != null && dt.Rows[i]["ApprTime"] != DBNull.Value)
                        ApprTime = Convert.ToDateTime(dt.Rows[i]["ApprTime"]);
                    DateTime ExecTime = DateTime.MinValue;
                    if (dt.Rows[i]["ExecTime"] != null && dt.Rows[i]["ExecTime"] != DBNull.Value)
                        ExecTime = Convert.ToDateTime(dt.Rows[i]["ExecTime"]);
                    TaskStage element = new TaskStage(Convert.ToInt32(dt.Rows[i]["ID"]), dt.Rows[i]["Name"].ToString(), template, TaskStage.GetTaskStatus(Convert.ToInt32(dt.Rows[i]["TaskStatus"])), ActualExec, ActualAppr, ExecTime, ApprTime, dt.Rows[i]["Remark"].ToString());
                    elements.Add(element);
                }
            }
            return elements;
        }

        public int AddTaskStage(TaskStage element)
        {
            string sql = "insert into TaskStage (Name, TaskStatus, TemplateID, Remark) values ('" + element.Name + "', " + (int)element.Status + ", " + element.Template.ID + ", '" + element.Remark + "'); select SCOPE_IDENTITY()";
            object obj = sqlHelper.ExecuteSqlReturn(sql);
            int R;
            if (obj != null && obj != DBNull.Value && int.TryParse(obj.ToString(), out R))
                return R;
            else
                return 0;
        }

        public bool UpdateTaskStage(TaskStage element)
        {
            string sql = "update TaskStage set Name='" + element.Name + "', TaskStatus=" + (int)element.Status + ", TemplateID=" + element.Template.ID + ", Remark='" + element.Remark + "' where ID=" + element.ID;
            int r = sqlHelper.ExecuteSql(sql);
            return r > 0;
        }

        public bool SetReceiveToExec(int id)
        {
            string sql = "update TaskStage set TaskStatus=" + (int)TaskStatus.Processing + " where ID=" + id;
            int r = sqlHelper.ExecuteSql(sql);
            return r > 0;
        }

        public bool SetActualExec(int id, User user)
        {
            string sql = "update TaskStage set TaskStatus=" + (int)TaskStatus.Processed + ", ActualExec='" + user.ID + "', ExecTime=getdate() where ID=" + id;
            int r = sqlHelper.ExecuteSql(sql);
            return r > 0;
        }

        public bool SetActualAppr(int id, User user)
        {
            string sql = "update TaskStage set TaskStatus=" + (int)TaskStatus.Finished + ", ActualAppr='" + user.ID + "', ApprTime=getdate() where ID=" + id;
            int r = sqlHelper.ExecuteSql(sql);
            return r > 0;
        }

        public bool DeleteTaskStage(TaskStage element)
        {
            string sql = "delete from TaskStage where ID=" + element.ID;
            int r = sqlHelper.ExecuteSql(sql);
            return r > 0;
        }
        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public bool UpgradeList(List<TaskStage> list)
        {
            int errCount = 0;
            foreach (TaskStage element in list)
            {
                string sqlStr = "if exists (select 1 from TaskStage where ID=" + element.ID + ") update TaskStage set Name='" + element.Name + "', TaskStatus=" + (int)element.Status + ", TemplateID=" + element.Template.ID + ", Remark='" + element.Remark + "' where ID=" + element.ID + " else insert into TaskStage (Name, TaskStatus, TemplateID, Remark) values ('" + element.Name + "', " + (int)element.Status + ", " + element.Template.ID + ", '" + element.Remark + "')";
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
            return sqlHelper.Exists("select 1 from TaskStage where Name='" + name + "'");
        }

        /// <summary>
        /// 是否存在出了自己以外的同名
        /// </summary>
        /// <param name="name"></param>
        /// <param name="myId"></param>
        /// <returns></returns>
        public bool ExistsNameOther(string name, int myId)
        {
            return sqlHelper.Exists("select 1 from TaskStage where ID!=" + myId + " and Name='" + name + "'");
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
                return sqlHelper.Exists("select 1 from TaskStage " + w);
            }
            return false;
        }
    }
}
