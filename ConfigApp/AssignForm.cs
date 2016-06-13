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
    public partial class AssignForm : Form
    {
        KellControls.FloatingCircleLoading loading;
        System.Timers.Timer timer1;
        public AssignForm(ConfigClient owner)
        {
            InitializeComponent();
            this.owner = owner;
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

        ConfigClient owner;
        int lastIndex = -1;

        private void button2_Click(object sender, EventArgs e)
        {
            owner.SaveArchitectureToLocal();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                owner.LoadArchitectureFromLocal();
                RefreshAll();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void RefreshAll()
        {
            owner.SetStatusText("正在刷新架构信息，请稍候...");
            backgroundWorker1.RunWorkerAsync();
            label5.Text = "查看权限=>";
            listBox1.Items.Clear();
            owner.SetStatusText("Ready...");
        }

        private void RefreshDepStaff()
        {
            depUserControl1.LoadDepartments(owner.CurrentArchitecture.Deps);
            depUserControl1.LoadUsers(owner.CurrentArchitecture.Users);
        }

        private void RefreshRoles()
        {
            comboBox7.Items.Clear();
            Architecture a = owner.CurrentArchitecture;
            List<object> tmp = new List<object>();
            foreach (Role r in a.Roles)
            {
                tmp.Add(r);
                comboBox7.Items.Add(r);
            }
            listBoxSelecter3.AllItems = tmp;
        }

        private void RefreshPrms()
        {
            Architecture a = owner.CurrentArchitecture;
            List<object> tmp = new List<object>();
            foreach (Permission p in a.Pers)
            {
                tmp.Add(p);
            }
            listBoxSelecter2.AllItems = tmp;
        }

        private void RefreshDeps()
        {
            Architecture a = owner.CurrentArchitecture;
            List<object> tmp = new List<object>();
            foreach (User u in a.Users)
            {
                tmp.Add(u);
            }
            listBoxSelecter1.AllItems = tmp;
            comboBox6.Items.Clear();
            comboBox3.Items.Clear();
            foreach (Department d in a.Deps)
            {
                comboBox6.Items.Add(d);
                comboBox3.Items.Add(d);
            }
        }

        private void RefreshUgrps()
        {
            Architecture a = owner.CurrentArchitecture;
            List<object> tmp = new List<object>();
            foreach (User u in a.Users)
            {
                tmp.Add(u);
            }
            listBoxSelecter4.AllItems = tmp;
            comboBox5.Items.Clear();
            comboBox12.Items.Clear();
            foreach (UserGroup u in a.Ugroups)
            {
                comboBox5.Items.Add(u);
                comboBox12.Items.Add(u);
            }
        }

        private void RefreshUsers()
        {
            comboBox13.Items.Clear();
            comboBox1.Items.Clear();
            Architecture a = owner.CurrentArchitecture;
            foreach (User u in a.Users)
            {
                comboBox13.Items.Add(u);
                comboBox1.Items.Add(u);
            }
        }

        private void AssignForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("是否已经改动架构？如果已经改动，请更新架构到远程服务器，以免丢失！", "保存提醒", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.OK)
            {
                Exception ex;
                if (owner.UpgradeArchitectureToRemote(out ex))
                {
                    MessageBox.Show("更新成功！");
                }
                else
                {
                    MessageBox.Show("更新失败：" + ex.Message);
                }
            }
        }

        private void AssignForm_Load(object sender, EventArgs e)
        {
            RefreshAll();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (comboBox3.SelectedIndex > -1)
            {
                Department dep = owner.CurrentArchitecture.Deps[comboBox3.SelectedIndex];
                foreach (object o in listBoxSelecter1.SelectedItems)
                {
                    User u = o as User;
                    if (u != null)
                    {
                        if (!u.Departments.ContainsDepartment(dep))
                            u.Departments.Add(dep);
                    }
                }
                MessageBox.Show("添加完毕！");
            }
        }

        private void button13_Click(object sender, EventArgs e)
        {
            if (comboBox12.SelectedIndex > -1)
            {
                UserGroup ug = owner.CurrentArchitecture.Ugroups[comboBox12.SelectedIndex];
                foreach (object o in listBoxSelecter4.SelectedItems)
                {
                    User u = o as User;
                    if (u != null)
                    {
                        if (!u.Usergroups.ContainsUserGroup(ug))
                            u.Usergroups.Add(ug);
                    }
                }
                MessageBox.Show("添加完毕！");
                owner.DownloadRemoteArchitecture();
                RefreshAll();
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (comboBox7.SelectedIndex > -1)
            {
                Role role = owner.CurrentArchitecture.Roles[comboBox7.SelectedIndex];
                role.Permissions.Clear();
                foreach (object o in listBoxSelecter2.SelectedItems)
                {
                    Permission p = o as Permission;
                    if (p != null)
                    {
                        role.Permissions.Add(p);
                    }
                }
                MessageBox.Show("添加完毕！");
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (comboBox5.SelectedIndex > -1)
            {
                UserGroup ug = owner.CurrentArchitecture.Ugroups[comboBox5.SelectedIndex];
                ug.Roles.Clear();
                foreach (object o in listBoxSelecter3.SelectedItems)
                {
                    Role r = o as Role;
                    if (r != null)
                    {
                        ug.Roles.Add(r);
                    }
                }
                MessageBox.Show("添加完毕！");
                owner.DownloadRemoteArchitecture();
                RefreshAll();
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (comboBox6.SelectedIndex > -1)
            {
                Department dep = owner.CurrentArchitecture.Deps[comboBox6.SelectedIndex];
                dep.Roles.Clear();
                foreach (object o in listBoxSelecter3.SelectedItems)
                {
                    Role r = o as Role;
                    if (r != null)
                    {
                        dep.Roles.Add(r);
                    }
                }
                MessageBox.Show("添加完毕！");
                owner.DownloadRemoteArchitecture();
                RefreshAll();
            }
        }

        private void button14_Click(object sender, EventArgs e)
        {
            if (comboBox13.SelectedIndex > -1)
            {
                User user = owner.CurrentArchitecture.Users[comboBox13.SelectedIndex];
                user.Roles.Clear();
                foreach (object o in listBoxSelecter3.SelectedItems)
                {
                    Role r = o as Role;
                    if (r != null)
                    {
                        user.Roles.Add(r);
                    }
                }
                MessageBox.Show("添加完毕！");
            }
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox3.SelectedIndex > -1)
            {
                listBoxSelecter1.ReloadSourceData();
                Department dep = comboBox3.SelectedItem as Department;
                if (dep != null)
                {
                    List<object> some = new List<object>();
                    foreach (User u in owner.CurrentArchitecture.Users)
                    {
                        if (u.Departments.ContainsDepartment(dep))
                            some.Add(u);
                    }
                    listBoxSelecter1.SelectSome(some);
                }
            }
        }

        private void comboBox12_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox12.SelectedIndex > -1)
            {
                listBoxSelecter4.ReloadSourceData();
                UserGroup ug = comboBox12.SelectedItem as UserGroup;
                if (ug != null)
                {
                    List<object> some = new List<object>();
                    foreach (User u in owner.CurrentArchitecture.Users)
                    {
                        if (u.Usergroups.ContainsUserGroup(ug))
                            some.Add(u);
                    }
                    listBoxSelecter4.SelectSome(some);
                }
            }
        }

        private void comboBox7_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox7.SelectedIndex > -1)
            {
                listBoxSelecter2.ReloadSourceData();
                Role role = comboBox7.SelectedItem as Role;
                if (role != null)
                {
                    List<object> some = new List<object>();
                    foreach (Permission p in owner.CurrentArchitecture.Pers)
                    {
                        if (role.Permissions.ContainsPermission(p))
                            some.Add(p);
                    }
                    listBoxSelecter2.SelectSome(some);
                }
            }
        }

        private void comboBox5_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox5.SelectedIndex > -1)
            {
                listBoxSelecter3.ReloadSourceData();
                UserGroup ug = comboBox5.SelectedItem as UserGroup;
                if (ug != null)
                {
                    List<object> some = new List<object>();
                    foreach (Role r in owner.CurrentArchitecture.Roles)
                    {
                        if (ug.Roles.ContainsRole(r))
                            some.Add(r);
                    }
                    listBoxSelecter3.SelectSome(some);
                }
            }
        }

        private void comboBox6_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox6.SelectedIndex > -1)
            {
                listBoxSelecter3.ReloadSourceData();
                Department dep = comboBox6.SelectedItem as Department;
                if (dep != null)
                {
                    List<object> some = new List<object>();
                    foreach (Role r in owner.CurrentArchitecture.Roles)
                    {
                        if (dep.Roles.ContainsRole(r))
                            some.Add(r);
                    }
                    listBoxSelecter3.SelectSome(some);
                }
            }
        }

        private void comboBox13_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox13.SelectedIndex > -1)
            {
                listBoxSelecter3.ReloadSourceData();
                User user = comboBox13.SelectedItem as User;
                if (user != null)
                {
                    List<object> some = new List<object>();
                    foreach (Role r in owner.CurrentArchitecture.Roles)
                    {
                        if (user.Roles.ContainsRole(r))
                            some.Add(r);
                    }
                    listBoxSelecter3.SelectSome(some);
                }
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex > -1)
            {
                User user = comboBox1.SelectedItem as User;
                if (user != null)
                {
                    listBox1.Items.Clear();
                    PermissionCollection ps = user.GetAllPermissionsByUser();
                    foreach (Permission per in ps)
                    {
                        listBox1.Items.Add(per);
                    }
                    label5.Text = "查看权限(" + ps.Count + ")";
                }
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (loading == null || loading.IsDisposed)
                loading = new KellControls.FloatingCircleLoading(150);
            loading.Show();
            loading.BringToFront();
            loading.Focus();
            loading.Refresh();
            timer1.Start();
            owner.DownloadRemoteArchitecture();
            RefreshAll();
            timer1.Stop();
            loading.Hide();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            if (loading == null || loading.IsDisposed)
                loading = new KellControls.FloatingCircleLoading(150);
            loading.Show();
            loading.BringToFront();
            loading.Focus();
            loading.Refresh();
            timer1.Start();
            Exception ex;
            if (owner.UpgradeArchitectureToRemote(out ex))
            {
                MessageBox.Show("更新成功！");
            }
            else
            {
                MessageBox.Show("更新失败：" + ex.Message);
            }
            timer1.Stop();
            loading.Hide();
        }
        
        private void listBox1_MouseMove(object sender, MouseEventArgs e)
        {
            int index = listBox1.IndexFromPoint(e.Location);
            if (index > -1 && index != lastIndex)
            {
                object item = listBox1.Items[index];
                if (item is Permission)
                {
                    Permission per = item as Permission;
                    try
                    {
                        if (!string.IsNullOrEmpty(per.Remark))
                            richTextBox1.Text = per.PermissionValue + Environment.NewLine + Environment.NewLine + per.Remark;
                        else
                            richTextBox1.Text = per.PermissionValue;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker bw = sender as BackgroundWorker;
            RefreshUI(bw);
        }

        private void RefreshUI(BackgroundWorker bw)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new System.Action<BackgroundWorker>(RefreshUI), bw);
            }
            else
            {
                RefreshUsers();
                bw.ReportProgress(15);
                RefreshUgrps();
                bw.ReportProgress(30);
                RefreshDeps();
                bw.ReportProgress(45);
                RefreshPrms();
                bw.ReportProgress(60);
                RefreshRoles();
                bw.ReportProgress(80);
                RefreshDepStaff();
                bw.ReportProgress(100);
            }
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            owner.SetStatusText("加载中..." + e.ProgressPercentage + "%");
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
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
