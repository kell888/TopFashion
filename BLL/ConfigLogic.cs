using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace TopFashion
{
    /// <summary>
    /// 配置表(TF_Config)操作类
    /// Time:2015-11-05
    /// Author:Kell
    /// </summary>
    public class ConfigLogic
    {
        private static SQLDBHelper sqlHelper = null;
        private static ConfigLogic Config;
        private static object lockObj = new object();
        private ConfigLogic()
        {
            sqlHelper = new SQLDBHelper();
        }
        public static ConfigLogic GetInstance()
        {
            sqlHelper = new SQLDBHelper(); if (Config == null)
            {
                lock (lockObj)
                {
                    if (Config == null)
                        Config = new ConfigLogic();
                }
            } return Config;
        }
        /// <summary>
        /// 增加数据的方法
        /// </summary>
        public int InsertConfigEntity(ConfigEntity config)
        {
            string sqlStr = "insert into TF_Config(configname,configvalue,configtype,remark,extension,flag) values (@ConfigName,@ConfigValue,@ConfigType,@Remark,@Extension,@Flag); select SCOPE_IDENTITY()";
            SqlParameter[] parms ={new SqlParameter("@configname",config.configname)
,new SqlParameter("@configvalue",config.configvalue)
,new SqlParameter("@configtype",config.configtype)
,new SqlParameter("@remark",config.remark)
,new SqlParameter("@extension",config.extension)
,new SqlParameter("@flag",config.flag)
};
            object obj = sqlHelper.ExecuteSqlReturn(sqlStr, false, parms);
            int R;
            if (obj != null && obj != DBNull.Value && int.TryParse(obj.ToString(), out R))
                return R;
            else
                return 0;
        }
        /// <summary>
        /// 删除数据的方法 
        /// </summary>
        public int DeleteConfig(int id)
        {
            int resultRow = 0;
            string sqlStr = "delete from TF_Config where ID=@ID";
            SqlParameter[] parms = { new SqlParameter("@ID", id) };
            resultRow = sqlHelper.ExecuteSql(sqlStr, false, parms);
            return resultRow;
        }
        /// <summary>
        /// 修改数据的方法 
        /// </summary>
        public int UpdateConfigEntity(ConfigEntity config)
        {
            int resultRow = 0;
            string sqlStr = "update TF_Config set configname=@configname,configvalue=@configvalue,configtype=@configtype,remark=@remark,extension=@extension,flag=@flag where ID=@ID"; SqlParameter[] parms ={new SqlParameter("@configname",config.configname)
,new SqlParameter("@configvalue",config.configvalue)
,new SqlParameter("@configtype",config.configtype)
,new SqlParameter("@remark",config.remark)
,new SqlParameter("@extension",config.extension)
,new SqlParameter("@flag",config.flag)
,new SqlParameter("@ID",config.id)};
            resultRow = sqlHelper.ExecuteSql(sqlStr, false, parms);
            return resultRow;
        }
        /// <summary>
        /// 查询单个数据的方法 
        /// </summary>
        public DataTable GetOneTable(int id)
        {
            string sqlStr = "select ID as ID,ConfigName as ConfigName,ConfigValue as ConfigValue,ConfigType as ConfigType,Remark as Remark,Extension as Extension,Flag as Flag from TF_Config where ID=@ID";
            SqlParameter[] parms = { new SqlParameter("@ID", id) };
            DataTable dtResult = sqlHelper.Query(sqlStr, false, parms);
            return dtResult;
        }
        /// <summary>
        /// 查询单个数据的方法 
        /// </summary>
        public ConfigEntity GetOneConfig(int id)
        {
            ConfigEntity config = null;
            string sqlStr = "select configname,configvalue,configtype,remark,extension,flag from TF_Config where ID=@ID";
            SqlParameter[] parms = { new SqlParameter("@ID", id) };
            DataTable dtResult = sqlHelper.Query(sqlStr, false, parms);
            if (dtResult != null && dtResult.Rows.Count > 0)
            {
                config = new ConfigEntity();
                foreach (DataRow dr in dtResult.Rows)
                {
                    config.id = Convert.ToInt32(dr["id"].ToString());
                    config.configname = dr["configname"].ToString();
                    config.configvalue = dr["configvalue"].ToString();
                    config.configtype = Convert.ToInt32(dr["configtype"].ToString());
                    config.remark = dr["remark"].ToString();
                    config.extension = Convert.ToInt32(dr["extension"].ToString());
                    config.flag = Convert.ToInt32(dr["flag"].ToString());
                }
            }
            return config;
        }
        /// <summary>
        /// 查询数据的方法 
        /// </summary>
        public static List<ConfigEntity> GetConfigs(List<ConfigEntity> list, Predicate<ConfigEntity> match)
        {
            return list.FindAll(match);
        }
        /// <summary>
        /// 查询所有数据的方法
        /// </summary>
        public DataTable GetAllConfig()
        {
            string sqlStr = "select configname,configvalue,configtype,remark,extension,flag from TF_Config";
            DataTable dtResult = sqlHelper.Query(sqlStr);
            dtResult.TableName = "dtConfig";
            return dtResult;
        }
        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public bool UpgradeList(List<ConfigEntity> list)
        {
            int errCount = 0;
            foreach (ConfigEntity config in list)
            {
                string sqlStr = "if exists (select 1 from TF_Config where ID=@ID) update TF_Config set configname=@configname,configvalue=@configvalue,configtype=@configtype,remark=@remark,extension=@extension,flag=@flag where ID=@ID else insert into TF_Config(configname,configvalue,configtype,remark,extension,flag) values (@configname,@configvalue,@cnfigtype,@remark,@extension,@flag)";
                try
                {
                    SqlParameter[] parms = {
new SqlParameter("@configname",config.configname)
,new SqlParameter("@configvalue",config.configvalue)
,new SqlParameter("@configtype",config.configtype)
,new SqlParameter("@remark",config.remark)
,new SqlParameter("@extension",config.extension)
,new SqlParameter("@flag",config.flag)
,new SqlParameter("@ID",config.id)};
                    sqlHelper.ExecuteSql(sqlStr, false, parms);
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
            return sqlHelper.Exists("select 1 from TF_Config where Name='" + name + "'");
        }

        /// <summary>
        /// 是否存在出了自己以外的同名
        /// </summary>
        /// <param name="name"></param>
        /// <param name="myId"></param>
        /// <returns></returns>
        public bool ExistsNameOther(string name, int myId)
        {
            return sqlHelper.Exists("select 1 from TF_Config where ID!=" + myId + " and Name='" + name + "'");
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
                return sqlHelper.Exists("select 1 from TF_Config " + w);
            }
            return false;
        }
    }
}
