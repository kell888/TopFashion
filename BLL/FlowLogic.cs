using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using KellWorkFlow;

namespace TopFashion
{
    public class FlowLogic
    {        
        SQLDBHelper sqlHelper;
        static FlowLogic instance;
        public static FlowLogic GetInstance()
        {
            if (instance == null)
                instance = new FlowLogic();

            return instance;
        }

        private FlowLogic()
        {
            sqlHelper = new SQLDBHelper();
        }

        public Flow GetFlow(int id)
        {
            string sql = "select * from Flow where ID=" + id;
            DataTable dt = sqlHelper.Query(sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                int templateId = Convert.ToInt32(dt.Rows[0]["TemplateID"]);
                FlowTemplate template = FlowTemplateLogic.GetInstance().GetFlowTemplate(templateId);
                List<TaskStage> stages = new List<TaskStage>();
                string stagesIds = dt.Rows[0]["Stages"].ToString();
                string[] stageIdList = stagesIds.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                foreach (string stageId in stageIdList)
                {
                    TaskStage stage = TaskStageLogic.GetInstance().GetTaskStage(Convert.ToInt32(stageId));
                    if (stage != null)
                    {
                        stages.Add(stage);
                    }
                }
                Flow element = new Flow(Convert.ToInt32(dt.Rows[0]["ID"]), dt.Rows[0]["Name"].ToString(), template, Convert.ToInt32(dt.Rows[0]["CurrentIndex"]), dt.Rows[0]["Remark"].ToString(), stages);
                return element;
            }
            return null;
        }

        /// <summary>
        /// 获取指定的流程
        /// </summary>
        /// <param name="name"></param>
        /// <param name="status">1为未完成，2为已完成，其他为所有</param>
        /// <returns></returns>
        public List<Flow> GetFlows(string name, int status)
        {
            List<Flow> elements = new List<Flow>();
            string where = "where (1=1)";
            if (!string.IsNullOrEmpty(name))
            {
                where += " and Name like '%" + name + "%'";
            }
            string sql = "select * from Flow " + where + " order by ID desc";
            DataTable dt = sqlHelper.Query(sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                TaskStageLogic tsl = TaskStageLogic.GetInstance();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    FlowTemplate template = FlowTemplateLogic.GetInstance().GetFlowTemplate(Convert.ToInt32(dt.Rows[i]["TemplateID"]));
                    List<TaskStage> stages = new List<TaskStage>();
                    string stagesIds = dt.Rows[i]["Stages"].ToString();
                    string[] stageIdList = stagesIds.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                    foreach (string stageId in stageIdList)
                    {
                        TaskStage stage = TaskStageLogic.GetInstance().GetTaskStage(Convert.ToInt32(stageId));
                        if (stage != null)
                        {
                            stages.Add(stage);
                        }
                    }
                    int currentIndex = Convert.ToInt32(dt.Rows[i]["CurrentIndex"]);
                    Flow element = new Flow(Convert.ToInt32(dt.Rows[i]["ID"]), dt.Rows[i]["Name"].ToString(), template, currentIndex, dt.Rows[i]["Remark"].ToString(), stages);
                    if (status == 1)
                    {
                        if ((currentIndex < stages.Count - 1) || (currentIndex == stages.Count - 1 && stages[stages.Count - 1].Status != TaskStatus.Finished))
                        {
                            elements.Add(element);
                        }
                    }
                    else if (status == 2)
                    {
                        if (currentIndex == stages.Count - 1 && stages[stages.Count - 1].Status == TaskStatus.Finished)
                        {
                            elements.Add(element);
                        }
                    }
                    else
                    {
                        elements.Add(element);
                    }
                }
            }
            return elements;
        }

        /// <summary>
        /// 获取指定的流程
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public List<Flow> GetFlows(string where)
        {
            List<Flow> elements = new List<Flow>();
            string w = "";
            if (!string.IsNullOrEmpty(where))
            {
                w = where.Trim().ToLower();
                if (!w.StartsWith("where "))
                    w = "where " + w;
            }
            string sql = "select * from Flow" + w + " order by ID desc";
            DataTable dt = sqlHelper.Query(sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                TaskStageLogic tsl = TaskStageLogic.GetInstance();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    FlowTemplate template = FlowTemplateLogic.GetInstance().GetFlowTemplate(Convert.ToInt32(dt.Rows[i]["TemplateID"]));
                    List<TaskStage> stages = new List<TaskStage>();
                    string stagesIds = dt.Rows[i]["Stages"].ToString();
                    string[] stageIdList = stagesIds.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                    foreach (string stageId in stageIdList)
                    {
                        TaskStage stage = TaskStageLogic.GetInstance().GetTaskStage(Convert.ToInt32(stageId));
                        if (stage != null)
                        {
                            stages.Add(stage);
                        }
                    }
                    Flow element = new Flow(Convert.ToInt32(dt.Rows[i]["ID"]), dt.Rows[i]["Name"].ToString(), template, Convert.ToInt32(dt.Rows[i]["CurrentIndex"]), dt.Rows[i]["Remark"].ToString(), stages);
                    elements.Add(element);
                }
            }
            return elements;
        }

        public List<Flow> GetAllFlows()
        {
            List<Flow> elements = new List<Flow>();
            string sql = "select * from Flow order by ID desc";
            DataTable dt = sqlHelper.Query(sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                TaskStageLogic tsl = TaskStageLogic.GetInstance();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    FlowTemplate template = FlowTemplateLogic.GetInstance().GetFlowTemplate(Convert.ToInt32(dt.Rows[i]["TemplateID"]));
                    List<TaskStage> stages = new List<TaskStage>();
                    string stagesIds = dt.Rows[i]["Stages"].ToString();
                    string[] stageIdList = stagesIds.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                    foreach (string stageId in stageIdList)
                    {
                        TaskStage stage = TaskStageLogic.GetInstance().GetTaskStage(Convert.ToInt32(stageId));
                        if (stage != null)
                        {
                            stages.Add(stage);
                        }
                    }
                    Flow element = new Flow(Convert.ToInt32(dt.Rows[i]["ID"]), dt.Rows[i]["Name"].ToString(), template, Convert.ToInt32(dt.Rows[i]["CurrentIndex"]), dt.Rows[i]["Remark"].ToString(), stages);
                    elements.Add(element);
                }
            }
            return elements;
        }

        public int AddFlow(Flow element)
        {
            string sql = "insert into Flow (Name, TemplateID, CurrentIndex, Stages, Remark) values ('" + element.Name + "', " + element.Template.ID + ", " + element.CurrentIndex + ", '" + element.StageIds + "', '" + element.Remark + "'); select SCOPE_IDENTITY()";
            object obj = sqlHelper.ExecuteSqlReturn(sql);
            int R;
            if (obj != null && obj != DBNull.Value && int.TryParse(obj.ToString(), out R))
                return R;
            else
                return 0;
        }

        public bool UpdateFlow(Flow element)
        {
            string sql = "update Flow set Name='" + element.Name + "', TemplateID=" + element.Template.ID + ", CurrentIndex=" + element.CurrentIndex + ", Stages='" + element.StageIds + "', Remark='" + element.Remark + "' where ID=" + element.ID;
            int r = sqlHelper.ExecuteSql(sql);
            return r > 0;
        }

        public bool StartFlow(int id)
        {
            string sql = "update Flow set CurrentIndex=0 where ID=" + id;
            int r = sqlHelper.ExecuteSql(sql);
            return r > 0;
        }

        public bool DeleteFlow(Flow element)
        {
            string sql = "delete from Flow where ID=" + element.ID;
            int r = sqlHelper.ExecuteSql(sql);
            return r > 0;
        }
        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public bool UpgradeList(List<Flow> list)
        {
            int errCount = 0;
            foreach (Flow element in list)
            {
                string sqlStr = "if exists (select 1 from Flow where ID=" + element.ID + ") update Flow set Name='" + element.Name + "', TemplateID=" + element.Template.ID + ", CurrentIndex=" + element.CurrentIndex + ", Stages='" + element.StageIds + "', Remark='" + element.Remark + "' where ID=" + element.ID + " else insert into Flow (Name, TemplateID, CurrentIndex, Stages, Remark) values ('" + element.Name + "', " + element.Template.ID + ", " + element.CurrentIndex + ", '" + element.StageIds + "', '" + element.Remark + "')";
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
            return sqlHelper.Exists("select 1 from Flow where Name='" + name + "'");
        }

        /// <summary>
        /// 是否存在出了自己以外的同名
        /// </summary>
        /// <param name="name"></param>
        /// <param name="myId"></param>
        /// <returns></returns>
        public bool ExistsNameOther(string name, int myId)
        {
            return sqlHelper.Exists("select 1 from Flow where ID!=" + myId + " and Name='" + name + "'");
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
                return sqlHelper.Exists("select 1 from Flow " + w);
            }
            return false;
        }
    }
}
