using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TopFashion
{
    public partial class ConfigClient : Form
    {
        private bool sure;
        private List<ConfigEntity> configs;
        private ConfigClient()
        {
            InitializeComponent();
        }
        static ConfigClient instance;

        public static ConfigClient GetInstance()
        {
            if (instance == null || instance.IsDisposed)
            {
                instance = new ConfigClient();
            }
            if (!instance.Visible)
                instance.Show();
            instance.WindowState = FormWindowState.Normal;
            instance.BringToFront();
            instance.Focus();
            return instance;
        }

        private void ConfigClient_Load(object sender, EventArgs e)
        {
            LoginForm f8 = new LoginForm(this);
            if (f8.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                this.panel2.SendToBack();
                this.panel2.Hide();
                configs = GetRemoteConfig();
                LoadConfigs(configs);
            }
            else
            {
                notifyIcon1.Dispose();
                Environment.Exit(0);
            }
        }

        private void LoadConfigs(List<ConfigEntity> configs)
        {
            
        }

        private List<ConfigEntity> GetConfigs()
        {
            List<ConfigEntity> configs = new List<ConfigEntity>();            

            return configs;
        }

        private static List<ConfigEntity> GetRemoteConfig()
        {
            List<ConfigEntity> configs = new List<ConfigEntity>();
            SQLDBHelper sqlHelper = new SQLDBHelper();
            DataTable dt = sqlHelper.Query("select * from TF_Config");
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ConfigEntity config = new ConfigEntity();
                    config.id = Convert.ToInt32(dt.Rows[i]["ID"]);
                    config.configname = dt.Rows[i]["ConfigName"].ToString();
                    config.configvalue = dt.Rows[i]["ConfigValue"].ToString();
                    config.remark = dt.Rows[i]["Remark"].ToString();
                    config.extension = Convert.ToInt32(dt.Rows[i]["Extension"]);
                    config.flag = Convert.ToInt32(dt.Rows[i]["Flag"]);
                    configs.Add(config);
                }
            }
            return configs;
        }

        private static bool SaveConfigToRemote(List<ConfigEntity> configs)
        {
            if (configs != null)
            {
                try
                {
                    SQLDBHelper sqlHelper = new SQLDBHelper();
                    foreach (ConfigEntity config in configs)
                    {
                        string sql = "if exists (select 1 from TF_Config where ID=" + config.id + ") update TF_Config set ConfigName='',ConfigValue='',Remark='',Extension=,Flag= where ID=" + config.id + " else insert into TF_Config(ConfigName,ConfigValue,Remark,Extension,Flag) values ('" + config.configname + "','" + config.configvalue + "','" + config.remark + "'," + config.extension + "," + config.flag + ")";
                        sqlHelper.ExecuteSql(sql);
                    }
                    return true;
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }
            }
            return false;
        }

        private void 远程服务器ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RemoteDB remote = new RemoteDB();
            remote.ShowDialog();
        }

        private void 数据库管理ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DBForm db = new DBForm();
            db.ShowDialog();
        }

        private void ConfigClient_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (sure)
            {
                if (MessageBox.Show("确定要退出程序吗？", "退出提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.OK)
                {
                    notifyIcon1.Dispose();
                    Environment.Exit(0);
                }
                else
                {
                    e.Cancel = true;
                    sure = false;
                }
            }
            else
            {
                e.Cancel = true;
                this.Hide();
            }
        }

        private void Exit()
        {
            sure = true;
            this.Close();
        }

        private void 退出ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Exit();
        }

        private void 退出ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Exit();
        }

        private void ShowUI()
        {
            if (this.WindowState == FormWindowState.Minimized)
                this.WindowState = FormWindowState.Normal;
            this.Show();
            this.BringToFront();
        }

        private void 显示ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowUI();
        }

        private void notifyIcon1_DoubleClick(object sender, EventArgs e)
        {
            ShowUI();
        }

        private void 锁定ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LockSystem();
        }

        private void 锁定ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            LockSystem();
        }

        private void LockSystem()
        {
            this.textBox1.Clear();
            this.textBox1.Focus();
            this.textBox1.SelectAll();
            this.WindowState = FormWindowState.Normal;
            this.BringToFront();
            this.Show();
            this.panel2.Show();
            this.panel2.BringToFront();
            DisableMenus();
        }

        private void UnlockSystem()
        {
            this.panel2.SendToBack();
            this.panel2.Hide();
            EnableMenus();
        }

        private void DisableMenus()
        {
            foreach (ToolStripMenuItem c in menuStrip1.Items)
            {
                if (c.Name != "锁定ToolStripMenuItem")
                    c.Enabled = false;
            }
            foreach (ToolStripItem c in contextMenuStrip1.Items)
            {
                if (c.Name != "锁定ToolStripMenuItem1")
                    c.Enabled = false;
            }
        }

        private void EnableMenus()
        {
            foreach (ToolStripMenuItem c in menuStrip1.Items)
            {
                if (c.Name != "锁定ToolStripMenuItem")
                    c.Enabled = true;
            }
            foreach (ToolStripItem c in contextMenuStrip1.Items)
            {
                if (c.Name != "锁定ToolStripMenuItem1")
                    c.Enabled = true;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {//解锁
            Login(textBox1);
        }

        public bool Login(TextBox textBox1)
        {
            string auth = textBox1.Text.ToLower();
            if (auth != string.Empty)
            {
                if (auth.Length >= 8)
                {
                    if (auth == DateTime.Now.ToString("yyMMddHH") + Configs.Auth.ToLower())
                    {
                        UnlockSystem();
                        return true;
                    }
                }
                else
                {
                    MessageBox.Show("授权码错误！");
                    textBox1.SelectAll();
                    textBox1.Focus();
                }
            }
            else
            {
                MessageBox.Show("请输入授权码！");
                textBox1.Focus();
            }
            return false;
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\r')
                Login(textBox1);
        }

        public static DataTable LoadTableFromFile(string filename)
        {
            DataTable data = new DataTable();
            try
            {
                XmlReadMode xrm = data.ReadXml(filename);
            }
            catch (Exception e)
            {
                MessageBox.Show("读取文件出错：" + e.Message);
            }
            return data;
        }

        public static bool SaveTableToFile(DataTable data, string filename)
        {
            if (data == null || data.Columns.Count == 0)
                return false;
            try
            {
                data.WriteXml(filename, XmlWriteMode.WriteSchema);
                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show("写入文件出错：" + e.Message);
                return false;
            }
        }

        private void 载入本地配置ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "配置文档(*.cfg)|*.cfg";
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                DataTable dt = LoadTableFromFile(openFileDialog1.FileName);
                configs = CovertToConfigEntity(dt);
                LoadConfigs(configs);
            }
            openFileDialog1.Dispose();
        }

        private List<ConfigEntity> CovertToConfigEntity(DataTable dt)
        {
            List<ConfigEntity> configs = new List<ConfigEntity>();
            if (dt != null && dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ConfigEntity config = new ConfigEntity();
                    config.id = Convert.ToInt32(dt.Rows[i]["ID"]);
                    config.configname = dt.Rows[i]["ConfigName"].ToString();
                    config.configvalue = dt.Rows[i]["ConfigValue"].ToString();
                    config.remark = dt.Rows[i]["Remark"].ToString();
                    config.extension = Convert.ToInt32(dt.Rows[i]["Extension"]);
                    config.flag = Convert.ToInt32(dt.Rows[i]["Flag"]);
                    configs.Add(config);
                }
            }
            return configs;
        }

        private DataTable ConvertToDataTable(List<ConfigEntity> configs)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("ID", typeof(int));
            dt.Columns.Add("ConfigName", typeof(string));
            dt.Columns.Add("ConfigValue", typeof(string));
            dt.Columns.Add("Remark", typeof(string));
            dt.Columns.Add("Extension", typeof(int));
            dt.Columns.Add("Flag", typeof(int));
            dt.PrimaryKey = new DataColumn[] { dt.Columns[0] };
            foreach (ConfigEntity config in configs)
            {
                DataRow row = dt.NewRow();
                row.ItemArray = new object[] { config.id, config.configname, config.configvalue, config.remark, config.extension, config.flag };
                dt.Rows.Add(row);
            }
            return dt;
        }

        private void 获取远程配置ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            configs = GetRemoteConfig();
            LoadConfigs(configs);
        }

        private void 保存到本地ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFileDialog1.Filter = "配置文档(*.cfg)|*.cfg";
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                configs = GetConfigs();
                DataTable dt = ConvertToDataTable(configs);
                SaveTableToFile(dt, saveFileDialog1.FileName);
            }
            saveFileDialog1.Dispose();
        }

        private void 更新到远程ToolStripMenuItem_Click(object sender, EventArgs e)
        {
             bool f = SaveConfigToRemote(configs);
             if (f)
                 MessageBox.Show("更新成功！");
             else
                 MessageBox.Show("更新失败！");
        }
    }
}
