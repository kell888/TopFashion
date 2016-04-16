using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using KellWorkFlow;

namespace TopFashion
{
    public class FlowTemplateLogic
    {        
        SQLDBHelper sqlHelper;
        static FlowTemplateLogic instance;
        public static FlowTemplateLogic GetInstance()
        {
            if (instance == null)
                instance = new FlowTemplateLogic();

            return instance;
        }

        private FlowTemplateLogic()
        {
            sqlHelper = new SQLDBHelper();
        }

        public FlowTemplate GetFlowTemplate(int id)
        {
            string sql = "select * from FlowTemplate where ID=" + id;
            DataTable dt = sqlHelper.Query(sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                TaskStageTemplateLogic tsl = TaskStageTemplateLogic.GetInstance();
                List<TaskStageTemplate> stages = new List<TaskStageTemplate>();
                string stageIds = dt.Rows[0]["Stages"].ToString();
                List<string> idList = Flow.GetSatges(stageIds);
                foreach (string idStr in idList)
                {
                    TaskStageTemplate stage = tsl.GetTaskStageTemplate(Convert.ToInt32(idStr));
                    if (stage != null)
                        stages.Add(stage);
                }
                FlowTemplate element = new FlowTemplate(Convert.ToInt32(dt.Rows[0]["ID"]), dt.Rows[0]["Name"].ToString(), stages);
                return element;
            }
            return null;
        }

        public List<FlowTemplate> GetFlowTemplates(string name, int stageCount)
        {
            List<FlowTemplate> elements = new List<FlowTemplate>();
            string where = "where (1=1)";
            if (!string.IsNullOrEmpty(name))
            {
                where += " and Name like '%" + name + "%'";
            }
            string sql = "select * from FlowTemplate " + where + " order by ID desc";
            DataTable dt = sqlHelper.Query(sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                TaskStageTemplateLogic tsl = TaskStageTemplateLogic.GetInstance();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    List<TaskStageTemplate> stages = new List<TaskStageTemplate>();
                    string stageIds = dt.Rows[i]["Stages"].ToString();
                    List<string> idList = Flow.GetSatges(stageIds);
                    if (idList.Count == stageCount)
                    {
                        foreach (string id in idList)
                        {
                            TaskStageTemplate stage = tsl.GetTaskStageTemplate(Convert.ToInt32(id));
                            if (stage != null)
                                stages.Add(stage);
                        }
                        FlowTemplate element = new FlowTemplate(Convert.ToInt32(dt.Rows[i]["ID"]), dt.Rows[i]["Name"].ToString(), stages);
                        elements.Add(element);
                    }
                }
            }
            return elements;
        }

        public List<FlowTemplate> GetFlowTemplates(string where)
        {
            List<FlowTemplate> elements = new List<FlowTemplate>();
            if (!string.IsNullOrEmpty(where))
            {
                string w = where.Trim().ToLower();
                if (!w.StartsWith("where "))
                    w = "where " + w;
                string sql = "select * from FlowTemplate " + w + " order by ID desc";
                DataTable dt = sqlHelper.Query(sql);
                if (dt != null && dt.Rows.Count > 0)
                {
                    TaskStageTemplateLogic tsl = TaskStageTemplateLogic.GetInstance();
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        List<TaskStageTemplate> stages = new List<TaskStageTemplate>();
                        string stageIds = dt.Rows[i]["Stages"].ToString();
                        List<string> idList = Flow.GetSatges(stageIds);
                        foreach (string id in idList)
                        {
                            TaskStageTemplate stage = tsl.GetTaskStageTemplate(Convert.ToInt32(id));
                            if (stage != null)
                                stages.Add(stage);
                        }
                        FlowTemplate element = new FlowTemplate(Convert.ToInt32(dt.Rows[i]["ID"]), dt.Rows[i]["Name"].ToString(), stages);
                        elements.Add(element);
                    }
                }
            }
            return elements;
        }

        public List<FlowTemplate> GetAllFlowTemplates()
        {
            List<FlowTemplate> elements = new List<FlowTemplate>();
            string sql = "select * from FlowTemplate";
            DataTable dt = sqlHelper.Query(sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                TaskStageTemplateLogic tsl = TaskStageTemplateLogic.GetInstance();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    List<TaskStageTemplate> stages = new List<TaskStageTemplate>();
                    string stageIds = dt.Rows[i]["Stages"].ToString();
                    List<string> idList = Flow.GetSatges(stageIds);
                    foreach (string id in idList)
                    {
                        TaskStageTemplate stage = tsl.GetTaskStageTemplate(Convert.ToInt32(id));
                        if (stage != null)
                            stages.Add(stage);
                    }
                    FlowTemplate element = new FlowTemplate(Convert.ToInt32(dt.Rows[i]["ID"]), dt.Rows[i]["Name"].ToString(), stages);
                    elements.Add(element);
                }
            }
            return elements;
        }

        public int AddFlowTemplate(FlowTemplate element)
        {
            string sql = "insert into FlowTemplate (Name, Stages) values ('" + element.Name + "', '" + element.StageIds + "'); select SCOPE_IDENTITY()";
            object obj = sqlHelper.ExecuteSqlReturn(sql);
            int R;
            if (obj != null && obj != DBNull.Value && int.TryParse(obj.ToString(), out R))
                return R;
            else
                return 0;
        }

        public bool UpdateFlowTemplate(FlowTemplate element)
        {
            string sql = "update FlowTemplate set Name='" + element.Name + "', Stages='" + element.StageIds + "' where ID=" + element.ID;
            int r = sqlHelper.ExecuteSql(sql);
            return r > 0;
        }

        public bool DeleteFlowTemplate(FlowTemplate element)
        {
            string sql = "delete from FlowTemplate where ID=" + element.ID;
            int r = sqlHelper.ExecuteSql(sql);
            return r > 0;
        }
        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public bool UpgradeList(List<FlowTemplate> list)
        {
            int errCount = 0;
            foreach (FlowTemplate element in list)
            {
                string sqlStr = "if exists (select 1 from FlowTemplate where ID=" + element.ID + ") update FlowTemplate set Name='" + element.Name + "', Stages='" + element.StageIds + "' where ID=" + element.ID + " else insert into FlowTemplate (Name, Stages) values ('" + element.Name + "', '" + element.StageIds + "')";
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
            return sqlHelper.Exists("select 1 from FlowTemplate where Name='" + name + "'");
        }

        /// <summary>
        /// 是否存在出了自己以外的同名
        /// </summary>
        /// <param name="name"></param>
        /// <param name="myId"></param>
        /// <returns></returns>
        public bool ExistsNameOther(string name, int myId)
        {
            return sqlHelper.Exists("select 1 from FlowTemplate where ID!=" + myId + " and Name='" + name + "'");
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
                return sqlHelper.Exists("select 1 from FlowTemplate " + w);
            }
            return false;
        }
    }
}
