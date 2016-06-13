using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace TopFashion
{
    [Serializable]
    public class ConfigEntity
    {
        int _id;

        public int id
        {
            get { return _id; }
            set { _id = value; }
        }
        string configName;

        public string configname
        {
            get { return configName; }
            set { configName = value; }
        }
        string configValue = string.Empty;

        public string configvalue
        {
            get { return configValue; }
            set { configValue = value; }
        }
        string _remark = string.Empty;

        public string remark
        {
            get { return _remark; }
            set { _remark = value; }
        }
        int _extension;

        public int extension
        {
            get { return _extension; }
            set { _extension = value; }
        }
        int _flag;

        public int flag
        {
            get { return _flag; }
            set { _flag = value; }
        }
        int _configtype;

        public int configtype
        {
            get { return _configtype; }
            set { _configtype = value; }
        }

        public override string ToString()
        {
            return configName;
        }
    }
}
