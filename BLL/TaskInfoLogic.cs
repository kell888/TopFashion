using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using KellWorkFlow;

namespace TopFashion
{
    public class TaskInfoLogic
    {        
        SQLDBHelper sqlHelper;
        static TaskInfoLogic instance;
        public static TaskInfoLogic GetInstance()
        {
            if (instance == null)
                instance = new TaskInfoLogic();

            return instance;
        }

        private TaskInfoLogic()
        {
            sqlHelper = new SQLDBHelper();
        }

        public TaskInfo GetTaskInfo(int id)
        {
            string sql = "select * from TaskInfo where ID=" + id;
            DataTable dt = sqlHelper.Query(sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                FlowLogic ftl = FlowLogic.GetInstance();
                TaskInfo element = new TaskInfo(Convert.ToInt32(dt.Rows[0]["ID"]), Convert.ToInt32(dt.Rows[0]["EntityId"]), ftl.GetFlow(Convert.ToInt32(dt.Rows[0]["FlowID"])), dt.Rows[0]["Sponsor"].ToString(), dt.Rows[0]["Remark"].ToString());
                return element;
            }
            return null;
        }

        public TaskInfo GetTaskInfoByEntityId(int entityId)
        {
            string sql = "select * from TaskInfo where EntityId=" + entityId;
            DataTable dt = sqlHelper.Query(sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                FlowLogic ftl = FlowLogic.GetInstance();
                TaskInfo element = new TaskInfo(Convert.ToInt32(dt.Rows[0]["ID"]), Convert.ToInt32(dt.Rows[0]["EntityId"]), ftl.GetFlow(Convert.ToInt32(dt.Rows[0]["FlowID"])), dt.Rows[0]["Sponsor"].ToString(), dt.Rows[0]["Remark"].ToString());
                return element;
            }
            return null;
        }

        public List<TaskInfo> GetAllTaskInfos()
        {
            List<TaskInfo> elements = new List<TaskInfo>();
            string sql = "select * from TaskInfo";
            DataTable dt = sqlHelper.Query(sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                FlowLogic ftl = FlowLogic.GetInstance();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    TaskInfo element = new TaskInfo(Convert.ToInt32(dt.Rows[i]["ID"]), Convert.ToInt32(dt.Rows[i]["EntityId"]), ftl.GetFlow(Convert.ToInt32(dt.Rows[i]["FlowID"])), dt.Rows[i]["Sponsor"].ToString(), dt.Rows[i]["Remark"].ToString());
                    elements.Add(element);
                }
            }
            return elements;
        }

        public List<TaskInfo> GetTaskInfosBySponsor(User user)
        {
            List<TaskInfo> elements = new List<TaskInfo>();
            string sql = "select * from TaskInfo where ','+Sponsor+',' like '%" + user.ID + "%'";
            DataTable dt = sqlHelper.Query(sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                FlowLogic ftl = FlowLogic.GetInstance();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    TaskInfo element = new TaskInfo(Convert.ToInt32(dt.Rows[i]["ID"]), Convert.ToInt32(dt.Rows[i]["EntityId"]), ftl.GetFlow(Convert.ToInt32(dt.Rows[i]["FlowID"])), dt.Rows[i]["Sponsor"].ToString(), dt.Rows[i]["Remark"].ToString());
                    element.Remark = dt.Rows[i]["Remark"].ToString();
                    elements.Add(element);
                }
            }
            return elements;
        }

        public int AddTaskInfo(TaskInfo element)
        {
            string sql = "insert into TaskInfo (EntityId, FlowID, Sponsor, Remark) values (" + element.EntityId + ", " + element.Flow.ID + ", '" + element.Sponsor + "', '" + element.Remark + "'); select SCOPE_IDENTITY()";
            object obj = sqlHelper.ExecuteSqlReturn(sql);
            int R;
            if (obj != null && obj != DBNull.Value && int.TryParse(obj.ToString(), out R))
            {
                Flow flow = FlowLogic.GetInstance().GetFlow(element.Flow.ID);
                if (flow != null)
                {
                    FlowTemplate temp = FlowTemplateLogic.GetInstance().GetFlowTemplate(flow.Template.ID);
                    if (temp != null)
                    {
                        Alert alert = new Alert();
                        alert.提醒对象 = string.Join(",", temp.Stages[flow.CurrentIndex].Executors.ToArray());
                        alert.提醒方式 = 提醒方式.执行流程;
                        alert.提醒时间 = DateTime.Now;
                        alert.提醒项目 = flow.Name;
                        alert.备注 = flow.ID.ToString();
                        AlertLogic.GetInstance().AddAlert(alert);
                        Alert alert2 = new Alert();
                        alert2.提醒对象 = string.Join(",", temp.Stages[flow.CurrentIndex].Approvers.ToArray());
                        alert2.提醒方式 = 提醒方式.审批流程;
                        alert2.提醒时间 = DateTime.Now;
                        alert2.提醒项目 = flow.Name;
                        alert.备注 = flow.ID.ToString();
                        AlertLogic.GetInstance().AddAlert(alert2);
                        return R;
                    }
                }
            }
            return 0;
        }

        public bool UpdateTaskInfo(TaskInfo element)
        {
            string sql = "update TaskInfo set EntityId=" + element.EntityId + ", FlowID=" + element.Flow.ID + ", Sponsor='" + element.Sponsor + "', Remark='" + element.Remark + "' where ID=" + element.ID;
            int r = sqlHelper.ExecuteSql(sql);
            return r > 0;
        }

        public bool DeleteTaskInfo(TaskInfo element)
        {
            string sql = "delete from TaskInfo where ID=" + element.ID;
            int r = sqlHelper.ExecuteSql(sql);
            return r > 0;
        }
        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public bool UpgradeList(List<TaskInfo> list)
        {
            int errCount = 0;
            foreach (TaskInfo element in list)
            {
                string sqlStr = "if exists (select 1 from TaskInfo where ID=" + element.ID + ") update TaskInfo set EntityId=" + element.EntityId + ", FlowID=" + element.Flow.ID + ", Sponsor='" + element.Sponsor + "', Remark='" + element.Remark + "' where ID=" + element.ID + " else insert into TaskInfo (EntityId, FlowID, Sponsor, Remark) values (" + element.EntityId + ", " + element.Flow.ID + ", '" + element.Sponsor + "', '" + element.Remark + "')";
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
            return sqlHelper.Exists("select 1 from TaskInfo where Name='" + name + "'");
        }

        /// <summary>
        /// 是否存在出了自己以外的同名
        /// </summary>
        /// <param name="name"></param>
        /// <param name="myId"></param>
        /// <returns></returns>
        public bool ExistsNameOther(string name, int myId)
        {
            return sqlHelper.Exists("select 1 from TaskInfo where ID!=" + myId + " and Name='" + name + "'");
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
                return sqlHelper.Exists("select 1 from TaskInfo " + w);
            }
            return false;
        }
    }
}
