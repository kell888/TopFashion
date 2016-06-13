using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace TopFashion
{
    public partial class RemoteDB : Form
    {
        public RemoteDB()
        {
            InitializeComponent();
        }

        private SqlConnectionStringBuilder GetConnectStringBuilder()
        {
            SqlConnectionStringBuilder scsb = new SqlConnectionStringBuilder();
            scsb.DataSource = textBox1.Text.Trim();
            scsb.UserID = textBox2.Text.Trim();
            scsb.Password = textBox3.Text;
            scsb.InitialCatalog = textBox4.Text.Trim();
            return scsb;
        }

        private SqlConnectionStringBuilder GetBackupConnectStringBuilder()
        {
            SqlConnectionStringBuilder scsb = new SqlConnectionStringBuilder();
            scsb.DataSource = textBox6.Text.Trim();
            scsb.UserID = textBox8.Text.Trim();
            scsb.Password = textBox7.Text;
            scsb.InitialCatalog = textBox5.Text.Trim();
            return scsb;
        }

        private void LoadConnectionString()
        {
            string connString = Configs.ConnString;
            SqlConnectionStringBuilder scsb = new SqlConnectionStringBuilder(connString);
            textBox1.Text = scsb.DataSource;
            textBox2.Text = scsb.UserID;
            textBox3.Text = scsb.Password;
            textBox4.Text = scsb.InitialCatalog;
        }

        private void LoadBackupConnectionString()
        {
            string connString = Configs.BackupConnString;
            SqlConnectionStringBuilder scsb = new SqlConnectionStringBuilder(connString);
            textBox6.Text = scsb.DataSource;
            textBox8.Text = scsb.UserID;
            textBox7.Text = scsb.Password;
            textBox5.Text = scsb.InitialCatalog;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SqlConnectionStringBuilder scsb = GetConnectStringBuilder();
            try
            {
                Configs.SaveConnectionString("connString", scsb);//, Application.ExecutablePath + ".config"
                MessageBox.Show("保存成功！");
            }
            catch (Exception ex)
            {
                MessageBox.Show("保存参数失败：" + ex.Message);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadConnectionString();
            LoadBackupConnectionString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SqlConnectionStringBuilder scsb = GetConnectStringBuilder();
            bool flag = TopFashion.HelpConnection.TestConnect(scsb.ConnectionString);
            if (flag)
                MessageBox.Show("连接成功！");
            else
                MessageBox.Show("连接失败！");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            SqlConnectionStringBuilder scsb = GetConnectStringBuilder();
            bool flag = TopFashion.HelpConnection.TestConnect(scsb.ConnectionString);
            if (flag)
                MessageBox.Show("连接成功！");
            else
                MessageBox.Show("连接失败！");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            SqlConnectionStringBuilder scsb = GetBackupConnectStringBuilder();
            try
            {
                Configs.SaveConnectionString("backupConnString", scsb);//, Application.ExecutablePath + ".config"
                MessageBox.Show("保存成功！");
            }
            catch (Exception ex)
            {
                MessageBox.Show("保存参数失败：" + ex.Message);
            }
        }
    }
}