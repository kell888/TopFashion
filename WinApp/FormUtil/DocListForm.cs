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
    public partial class DocListForm : PermissionForm
    {
        private bool allDocs;
        MainForm owner;
        KellControls.FloatingCircleLoading loading;
        System.Timers.Timer timer1;
        public DocListForm(User user, MainForm owner, bool allDocs = false)
        {
            this.User = user;
            InitializeComponent();
            this.owner = owner;
            this.allDocs = allDocs;
            if (this.User.ID == this.AdminId)
                删除ToolStripMenuItem.Enabled = true;
            loading = new KellControls.FloatingCircleLoading(150);
            timer1 = new System.Timers.Timer(1000);
            timer1.AutoReset = true;
            timer1.Elapsed += new System.Timers.ElapsedEventHandler(timer1_Elapsed);
        }

        void timer1_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            MethodInvoker mi = new MethodInvoker(RefreshLoading);
            mi.BeginInvoke(null, null);
        }

        private void RefreshLoading()
        {
            if (loading.InvokeRequired)
            {
                loading.BeginInvoke(new System.Action(() => loading.Refresh()));
            }
            else
            {
                loading.Refresh();
            }
        }

        private void LoadDocObjects(List<DocObject> docs)
        {
            if (docs != null)
                LoadItems(docs);
        }

        private void LoadItems(List<DocObject> items)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action<List<DocObject>>(LoadItems), items);
            }
            else
            {
                listBox1.Items.Clear();
                if (items != null)
                {
                    foreach (DocObject doc in items)
                    {
                        listBox1.Items.Add(doc);
                    }
                }
            }
        }

        private void DocListForm_Load(object sender, EventArgs e)
        {
            if (loading == null || loading.IsDisposed)
                loading = new KellControls.FloatingCircleLoading(150);
            loading.Show();
            loading.BringToFront();
            loading.Focus();
            loading.Refresh();
            timer1.Start();
            base.DisableUserPermission(this);
            backgroundWorker1.RunWorkerAsync();
            timer1.Stop();
            loading.Hide();
        }

        private void listBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            ShowDoc();
        }

        private void ShowDoc()
        {
            DocObject doc = listBox1.SelectedItem as DocObject;
            if (doc != null)
            {
                DocInfoForm dif = new DocInfoForm(this.User, this.owner, doc);
                dif.ShowDialog();
            }
            else
            {
                MessageBox.Show("请先选定要查看的文档！");
            }
        }

        private void 查看ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowDoc();
        }

        private void 修改ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EditDoc();
        }

        private void EditDoc()
        {
            DocObject doc = listBox1.SelectedItem as DocObject;
            if (doc != null)
            {
                DocEditForm def = new DocEditForm(this.User, this.owner, doc.Form, doc);
                def.ShowDialog();
                LoadAllDocs(null);
            }
            else
            {
                MessageBox.Show("请先选定要修改的文档！");
            }
        }

        private void 删除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DelDoc();
        }

        private void DelDoc()
        {
            DocObject doc = listBox1.SelectedItem as DocObject;
            if (doc != null)
            {
                if (MessageBox.Show("确定要删除该文档[" + doc.Name + "]吗？", "删除提醒", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.OK)
                {
                    if (DocObjectLogic.GetInstance().DeleteDocObject(doc, this.User))
                    {
                        MessageBox.Show("删除文档成功！");
                        listBox1.Items.RemoveAt(listBox1.SelectedIndex);
                    }
                    else
                    {
                        MessageBox.Show("删除失败或者您没有权限删除别人的文档！");
                    }
                }
            }
            else
            {
                MessageBox.Show("请先选定要删除的文档！");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ShowDoc();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            EditDoc();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            NewDoc();
        }

        private void NewDoc()
        {
            NewDocForm ndf = new NewDocForm(this.User, this.owner);
            if (ndf.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                LoadAllDocs(null);
            }
        }

        private void 添加新文档ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NewDoc();
        }

        private void LoadAllDocs(BackgroundWorker bw)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new System.Action<BackgroundWorker>(LoadAllDocs), bw);
            }
            else
            {
                List<DocObject> docs = null;
                if (allDocs)
                {
                    docs = DocObjectLogic.GetInstance().GetAllDocObjects();
                    bw.ReportProgress(50);
                }
                else
                {
                    docs = new List<DocObject>();
                    List<DocObject> doc = DocObjectLogic.GetInstance().GetDocObjectsByOwner(this.User);
                    docs.AddRange(doc);
                    bw.ReportProgress(30);
                    List<int> tempIds = FlowTemplateLogic.GetInstance().GetTepmIdsByExecOrAppr(this.User.ID.ToString());
                    bw.ReportProgress(50);
                    List<DocObject> doc2 = DocObjectLogic.GetInstance().GetDocObjectsByTemplateId(tempIds);
                    docs.AddRange(doc2);
                    bw.ReportProgress(70);
                }
                LoadDocObjects(docs);
                if (owner != null) owner.RefreshMsg("载入完毕");
            }
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker bw = sender as BackgroundWorker;
            LoadAllDocs(bw);
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (owner != null) owner.RefreshMsg("载入中..." + e.ProgressPercentage + "%");
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                MessageBox.Show("载入出错！");
            }
            else if (e.Cancelled)
            {
                MessageBox.Show("终止载入！");
            }
            else
            {
                //MessageBox.Show("载入完成！");
            }
        }
    }
}
