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
    public partial class ConfigForm : Form
    {
        public ConfigForm(ConfigClient owner)
        {
            InitializeComponent();
            this.owner = owner;
        }

        ConfigClient owner;
        bool change;

        private void button4_Click(object sender, EventArgs e)
        {
            openFileDialog1.Title = "载入本地配置";
            openFileDialog1.Filter = "配置文档(*.cfg)|*.cfg";
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                List<ConfigEntity> configs = owner.LoadConfigFromLocal(openFileDialog1.FileName);
                LoadConfigs(configs);
            }
            openFileDialog1.Dispose();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            List<ConfigEntity> configs = GetConfigs();
            owner.SaveConfigToLocal(configs);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            LoadConfigs(owner.Configs);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            owner.Configs = GetConfigs();
            Exception ex;
            if (owner.UpgradeToRemote(out ex))
            {
                change = false;
                MessageBox.Show("更新成功！");
            }
            else
            {
                MessageBox.Show("更新失败：" + ex.Message);
            }
        }

        private void LoadConfigs(List<ConfigEntity> configs)
        {
            DataTable dt = owner.ConvertToDataTable(configs);
            dataGridView1.DataSource = dt;
        }

        private List<ConfigEntity> GetConfigs()
        {
            List<ConfigEntity> configs = new List<ConfigEntity>();
            DataTable dt = dataGridView1.DataSource as DataTable;
            if (dt != null)
            {
                configs = owner.CovertToConfigEntity(dt);
            }
            return configs;
        }

        private void ConfigForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (change && MessageBox.Show("当前配置可能已经发生改动，需要将最新的配置信息更新到远程服务器么？", "保存提醒", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.OK)
            {
                owner.Configs = GetConfigs();
                Exception ex;
                if (owner.UpgradeToRemote(out ex))
                {
                    change = false;
                    MessageBox.Show("更新成功！");
                }
                else
                {
                    MessageBox.Show("更新失败：" + ex.Message);
                }
            }
        }

        private void ConfigForm_Load(object sender, EventArgs e)
        {
            LoadConfigs(owner.Configs);
        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            change = true;
        }

        private void dataGridView1_UserAddedRow(object sender, DataGridViewRowEventArgs e)
        {
            change = true;
        }

        private void dataGridView1_UserDeletedRow(object sender, DataGridViewRowEventArgs e)
        {
            change = true;
        }
    }
}
