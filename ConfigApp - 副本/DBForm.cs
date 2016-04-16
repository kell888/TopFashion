using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.IO;

namespace TopFashion
{
    public partial class DBForm : Form
    {
        static SQLDMO.BackupSink_PercentCompleteEventHandler backupProgress;
        static SQLDMO.RestoreSink_PercentCompleteEventHandler restoreProgress;
        static SQLDMO.Backup oBackup;
        static SQLDMO.Restore oRestore;

        public DBForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Backup(textBox2.Text.Trim());
        }

        private void Backup(string path)
        {
            button1.Enabled = false;
            toolStripProgressBar1.Visible = true;
            toolStripStatusLabel1.Text = "数据库备份进行中...";
            statusStrip1.Refresh();
            Application.DoEvents();
            ThreadPool.QueueUserWorkItem(delegate
            {
                if (this.InvokeRequired)
                    this.Invoke(new ProcessBackupHandler(ProcessBackup), path);
                else
                    ProcessBackup(path);
            });
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //要先选定备份的记录
            if (dataGridView1.SelectedRows.Count > 0)
            {
                //选多行时，只取第一行
                int id = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[0].Value);
                Restore(id);
            }
            else
            {
                MessageBox.Show("先选定一个您要恢复的备份记录！");
            }
        }

        private void Restore(int id)
        {
            string path = "";
            string originPath = BackupRestore.GetBackupPathById(id);
            if (Directory.Exists(originPath))
            {
                path = originPath;
            }
            else
            {
                MessageBox.Show("原来的备份目录被意外删除或者存储设备已经被拔除，请接下来指定一个新的目录，以供恢复。");
                if (folderBrowserDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    path = folderBrowserDialog1.SelectedPath;
                }
                folderBrowserDialog1.Dispose();
            }
            button2.Enabled = false;
            toolStripProgressBar1.Visible = true;
            toolStripStatusLabel1.Text = "数据库恢复进行中...";
            statusStrip1.Refresh();
            Application.DoEvents();
            ThreadPool.QueueUserWorkItem(delegate
            {
                if (this.InvokeRequired)
                    this.Invoke(new ProcessRestoreHandler(ProcessRestore), id, path);
                else
                    ProcessRestore(id, path);
            });
        }

        public delegate void ProcessRestoreHandler(int id, string path);
        private void ProcessRestore(int id, string path)
        {
            bool db = true;
            db = BackupRestore.SQLDbRestore(id, restoreProgress, out oRestore);
            toolStripProgressBar1.Visible = false;
            statusStrip1.Refresh();
            Application.DoEvents();
            if (db)
            {
                MessageBox.Show("恢复完毕！");
                toolStripStatusLabel1.Text = "恢复完成";
            }
            else
            {
                MessageBox.Show("数据库恢复失败！");
                toolStripStatusLabel1.Text = "数据库恢复失败";
            }
            button2.Enabled = true;
        }

        void Restore_PercentComplete(string Message, int Percent)
        {
            UpdateProgress(Message, Percent);
        }

        private void UpdateProgress(string Message, int Percent)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new MethodInvoker(delegate
                {
                    toolStripProgressBar1.Value = Percent;
                    toolStripProgressBar1.ToolTipText = Message;
                    statusStrip1.Refresh();
                    Application.DoEvents();
                }));
            }
            else
            {
                toolStripProgressBar1.Value = Percent;
                toolStripProgressBar1.ToolTipText = Message;
                statusStrip1.Refresh();
                Application.DoEvents();
            }
        }

        void Backup_PercentComplete(string Message, int Percent)
        {
            UpdateProgress(Message, Percent);
        }

        public delegate void ProcessBackupHandler(string path);
        private void ProcessBackup(string path)
        {
            bool db = true;
            db = BackupRestore.SQLDbBackup(path, backupProgress, out oBackup, textBox1.Text.Trim());
            toolStripProgressBar1.Visible = false;
            statusStrip1.Refresh();
            Application.DoEvents();
            if (db)
            {
                MessageBox.Show("备份完毕！");
                toolStripStatusLabel1.Text = "备份完成";
            }
            else
            {
                MessageBox.Show("备份数据库失败！");
                toolStripStatusLabel1.Text = "数据库备份失败";
            }
            DataTable dt = BackupRestore.LoadBackupHistory();
            dataGridView1.DataSource = dt;
            button1.Enabled = true;
        }

        private void DBForm_Load(object sender, EventArgs e)
        {
            backupProgress = new SQLDMO.BackupSink_PercentCompleteEventHandler(Backup_PercentComplete);
            restoreProgress = new SQLDMO.RestoreSink_PercentCompleteEventHandler(Restore_PercentComplete);
            DataTable dt = BackupRestore.LoadBackupHistory();
            dataGridView1.DataSource = dt;
        }
    }
}
