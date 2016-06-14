using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using Echevil;

namespace TopFashion
{
    public partial class MainForm : PermissionForm
    {
        const int CLOSE_SIZE = 12;
        private bool sure;
        NetworkMonitor netMonitor;
        System.Timers.Timer timer2;

        internal void SureExit()
        {
            if (Configs.MonitorNetwork)
            {
                if (netMonitor.Adapters.Count() > 0)
                    netMonitor.StopMonitoring();
                if (timer2.Enabled)
                    timer2.Stop();
            }
            notifyIcon1.Dispose();
            Environment.Exit(0);
        }

        private MainForm()
        {
            InitializeComponent();
            this.tabControl1.DrawMode = TabDrawMode.OwnerDrawFixed;//由自己绘制标题
            this.tabControl1.Padding = new System.Drawing.Point(CLOSE_SIZE + 2, 2);
            this.tabControl1.DrawItem += new DrawItemEventHandler(this.tabControl1_DrawItem);
            this.tabControl1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.tabControl1_MouseDown);
        }

        static MainForm instance;

        public static MainForm GetInstance()
        {
            if (instance == null || instance.IsDisposed)
            {
                instance = new MainForm();
            }
            if (!instance.Visible)
                instance.Show();
            instance.WindowState = FormWindowState.Maximized;
            instance.BringToFront();
            instance.Focus();
            return instance;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            if (Configs.MonitorNetwork)
            {
                netMonitor = new NetworkMonitor();
                if (netMonitor.Adapters.Count() > 0)
                    netMonitor.StartMonitoring();
                timer2 = new System.Timers.Timer(1000);
                timer2.AutoReset = true;
                timer2.Elapsed += new System.Timers.ElapsedEventHandler(timer2_Elapsed);
                timer2.Start();
            }
            RefreshMsg("正在初始化系统...");
            ThreadPool.QueueUserWorkItem(delegate
            {
                int a = this.AdminId;//获取管理员ID缓存以便后面用...
            });
            LoginForm f8 = new LoginForm(this);
            if (f8.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                this.panel2.SendToBack();
                this.panel2.Hide();
            }
            else
            {
                notifyIcon1.Dispose();
                Environment.Exit(0);
            }
        }

        void timer2_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            MethodInvoker mi = new MethodInvoker(RefreshNetworkInfo);
            mi.BeginInvoke(null, null);
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (sure)
            {
                if (MessageBox.Show("确定要退出程序吗？", "退出提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.OK)
                {
                    WriteLog.CreateLog(this.User.Username, "MainForm.FormClosing", "log", "用户：" + this.User.Username);
                    LockSystem();
                    if (Configs.MonitorNetwork)
                    {
                        if (netMonitor.Adapters.Count() > 0)
                            netMonitor.StopMonitoring();
                        if (timer2.Enabled)
                            timer2.Stop();
                    }
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
            WriteLog.CreateLog(this.User.Username, "MainForm.锁定ToolStripMenuItem", "log", "用户：" + this.User.Username);
            LockSystem();
        }

        private void 锁定ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            WriteLog.CreateLog(this.User.Username, "MainForm.锁定ToolStripMenuItem1", "log", "用户：" + this.User.Username);
            LockSystem();
        }

        private void LockSystem()
        {
            this.textBox1.Clear();
            this.textBox2.Clear();
            this.textBox1.Focus();
            this.WindowState = FormWindowState.Maximized;
            this.BringToFront();
            this.Show();
            this.panel2.Show();
            this.panel2.BringToFront();
            DisableMenus();
            if (timer1.Enabled)
                timer1.Stop();
        }

        private void UnlockSystem()
        {
            this.panel2.SendToBack();
            this.panel2.Hide();
            EnableMenus();
            if (!timer1.Enabled)
                timer1.Start();
        }

        private void DisableMenus()
        {
            foreach (ToolStripMenuItem c in menuStrip1.Items)
            {
                if (c.Name != "锁定ToolStripMenuItem")
                    c.Enabled = false;
            }
            panel2.EnableAllChildren();
        }

        private void EnableMenus()
        {
            RefreshMsg("正在为用户[" + this.User.Username + "]授权中...");
            base.CheckUserPermission(this);
        }

        private void button1_Click(object sender, EventArgs e)
        {//解锁
            Login(textBox1, textBox2);
        }

        public bool Login(TextBox textBox1, TextBox textBox2)
        {
            string use = textBox1.Text.Trim();
            string pwd = textBox2.Text;
            if (use != string.Empty)
            {
                RefreshMsg("正在获取用户信息...");
                User user = UserLogic.GetInstance().GetUser(use);
                if (user != null && user.Flag == 0)
                {
                    if (user.Password == pwd)
                    {
                        if (this.User != null && this.User.ID != user.ID)
                        {
                            //关掉所有的已经打开的窗口...
                            foreach (TabPage page in tabControl1.TabPages)
                            {
                                CloseSubForm(page);
                            }
                        }
                        this.User = user;
                        toolStripStatusLabel3.Text = "当前用户：" + user.Username;
                        WriteLog.CreateLog(this.User.Username, "MainForm.Login", "log", "用户：" + user.Username);
                        UnlockSystem();
                        RefreshMsg("Ready...");
                        return true;
                    }
                    else
                    {
                        MessageBox.Show("密码错误！");
                        textBox2.SelectAll();
                        textBox2.Focus();
                    }
                }
                else
                {
                    MessageBox.Show("用户名错误或者该用户已经被禁用！");
                    textBox1.SelectAll();
                    textBox1.Focus();
                }
            }
            else
            {
                MessageBox.Show("请输入用户名！");
                textBox1.Focus();
            }
            return false;
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (textBox1.Text != "" && e.KeyChar == '\r')
                Login(textBox1, textBox2);
        }
        #region
        private void AddTabPage(string str, Form form)
        {
            ThreadPool.QueueUserWorkItem(delegate
            {
                RefreshMsg("正在拼命加载中，请稍候...");
                try
                {
                    AddTabForm(str, form);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    RefreshMsg("Ready...");
                }
            });
        }

        private void AddTabForm(string str, Form form)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action<string, Form>(AddTabForm), str, form);
            }
            else
            {
                int have = TabControlCheckHave(this.tabControl1, form.GetType().FullName);
                if (have > -1)
                {
                    tabControl1.SelectTab(have);
                    tabControl1.SelectedTab.Text = str;
                    tabControl1.SelectedTab.Controls.Clear();
                    form.TopLevel = false;//设置非顶级窗口
                    form.Parent = tabControl1.SelectedTab;
                    form.WindowState = FormWindowState.Maximized;
                    form.FormBorderStyle = FormBorderStyle.None;
                    form.Show();
                }
                else
                {
                    tabControl1.TabPages.Add(str);
                    tabControl1.SelectTab(tabControl1.TabPages.Count - 1);
                    tabControl1.SelectedTab.Tag = form.GetType().FullName;
                    tabControl1.SelectedTab.Name = "Tab_" + form.Name;
                    form.TopLevel = false;//设置非顶级窗口
                    form.Parent = tabControl1.SelectedTab;
                    form.WindowState = FormWindowState.Maximized;
                    form.FormBorderStyle = FormBorderStyle.None;
                    form.Show();
                }
            }
        }

        internal void RefreshMsg(string msg)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action<string>(RefreshMsg), msg);
            }
            else
            {
                toolStripStatusLabel1.Text = msg;
                statusStrip1.Refresh();
            }
        }

        //判断TabPage是否已创建
        private int TabControlCheckHave(TabControl tab, string tabPage)
        {
            int index = -1;
            for (int i = 0; i < tab.TabPages.Count; i++)
            {
                TabPage tp = tab.TabPages[i];
                if (tp.Tag.ToString() == tabPage)
                {
                    index = i;
                    break;
                }
            }
            return index;
        }
        #endregion
        #region tabpage页面的关闭按钮及事件
        private void tabControl1_DrawItem(object sender, System.Windows.Forms.DrawItemEventArgs e)
        {
            try
            {
                Rectangle myTabRect = this.tabControl1.GetTabRect(e.Index);
                e.Graphics.FillRectangle(new SolidBrush(SystemColors.Control), myTabRect);

                //先添加TabPage属性   
                e.Graphics.DrawString(this.tabControl1.TabPages[e.Index].Text
                , this.Font, SystemBrushes.ControlText, myTabRect.X + 2, myTabRect.Y + 2);

                //再画一个矩形框
                using (Pen p = new Pen(Color.Black))
                {
                    myTabRect.Offset(myTabRect.Width - (CLOSE_SIZE + 3), 2);
                    myTabRect.Width = CLOSE_SIZE;
                    myTabRect.Height = CLOSE_SIZE;
                    e.Graphics.DrawRectangle(p, myTabRect);
                }

                //填充矩形框
                Color recColor = e.State == DrawItemState.Selected ? Color.MediumVioletRed : Color.DarkGray;
                using (Brush b = new SolidBrush(recColor))
                {
                    e.Graphics.FillRectangle(b, myTabRect);
                }

                //画关闭符号
                using (Pen p = new Pen(Color.White))
                {
                    //画"\"线
                    Point p1 = new Point(myTabRect.X + 3, myTabRect.Y + 3);
                    Point p2 = new Point(myTabRect.X + myTabRect.Width - 3, myTabRect.Y + myTabRect.Height - 3);
                    e.Graphics.DrawLine(p, p1, p2);

                    //画"/"线
                    Point p3 = new Point(myTabRect.X + 3, myTabRect.Y + myTabRect.Height - 3);
                    Point p4 = new Point(myTabRect.X + myTabRect.Width - 3, myTabRect.Y + 3);
                    e.Graphics.DrawLine(p, p3, p4);
                }

                e.Graphics.Dispose();
            }
            catch (Exception)
            {

            }
        }
        private void tabControl1_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                int x = e.X, y = e.Y;

                //计算关闭区域   
                Rectangle myTabRect = this.tabControl1.GetTabRect(this.tabControl1.SelectedIndex);

                myTabRect.Offset(myTabRect.Width - (CLOSE_SIZE + 3), 2);
                myTabRect.Width = CLOSE_SIZE;
                myTabRect.Height = CLOSE_SIZE;

                //如果鼠标在区域内就关闭选项卡   
                bool isClose = x > myTabRect.X && x < myTabRect.Right
                 && y > myTabRect.Y && y < myTabRect.Bottom;

                if (isClose)
                {
                    CloseSubForm(this.tabControl1.SelectedTab);
                }
            }
        }

        private void CloseSubForm(TabPage page)
        {
            string name = page.Name.Substring(4);
            Control[] cs = page.Controls.Find(name, false);
            if (cs != null && cs.Length > 0)
            {
                if (cs[0] is Form)
                {
                    Form f = cs[0] as Form;
                    f.Close();
                }
            }
            this.tabControl1.TabPages.Remove(page);
        }

        #endregion

        private void timer1_Tick(object sender, EventArgs e)
        {
            List<Alert> alerts = AlertLogic.GetInstance().GetSysAlertsByUser(this.User);
            foreach (Alert a in alerts)
            {
                if (a.提醒时间 < DateTime.Now)
                {
                    AlertMyForm amf = AlertMyForm.Instance(this.User, this, a);
                    amf.Show();
                }
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            textBox2.Clear();
        }

        private void 查询会员ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MemberForm mf = new MemberForm(this.User, this, 1);
            AddTabPage("会员信息", mf);
        }

        private void 编辑会员ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MemberForm mf = new MemberForm(this.User, this);
            AddTabPage("会员信息", mf);
        }

        private void 导入会员ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //ImportForm im = new ImportForm(this.User);
            //AddTabPage("导入会员", im);
        }

        private void 短信平台ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SMSForm sms = new SMSForm(this.User);
            AddTabPage("短信平台", sms);
        }

        private void 查询员工ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StaffForm sf = new StaffForm(this.User, 1);
            AddTabPage("员工信息", sf);
        }

        private void 编辑员工ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StaffForm sf = new StaffForm(this.User);
            AddTabPage("员工信息", sf);
        }

        private void 会籍卡种ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CardTypeForm ctf = new CardTypeForm(this.User);
            AddTabPage("会籍卡种", ctf);
        }

        private void 流水账ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FinanceForm ff = new FinanceForm(this.User);
            AddTabPage("流水账", ff);
        }

        private void 流水明细ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FinanceDetailForm fdf = new FinanceDetailForm(this.User);
            AddTabPage("流水明细", fdf);
        }

        private void 回访登记ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FollowupForm ff = new FollowupForm(this.User);
            AddTabPage("回访登记", ff);
        }

        private void 入库ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IncomeForm of = new IncomeForm(this.User);
            AddTabPage("入库登记", of);
        }

        private void 报销明细ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FinanceDetailForm fdf = new FinanceDetailForm(this.User);
            AddTabPage("报销明细", fdf);
        }

        private void 出库登记ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OutcomeForm of = new OutcomeForm(this.User);
            AddTabPage("消费登记", of);
        }

        private void 出库ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OutcomeForm of = new OutcomeForm(this.User);
            AddTabPage("消费登记", of);
        }

        private void 库存查询ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            InventoryForm inf = new InventoryForm(this.User);
            AddTabPage("库存查询", inf);
        }

        private void 产品维护ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ProductForm pf = new ProductForm(this.User);
            AddTabPage("产品维护", pf);
        }

        private void 资产维护ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PropertyForm pf = new PropertyForm(this.User);
            AddTabPage("资产维护", pf);
        }

        private void 提醒ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AlertForm af = new AlertForm(this.User);
            AddTabPage("提醒", af);
        }

        private void 产品类型ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ProductTypeForm atf = new ProductTypeForm(this.User);
            AddTabPage("产品类型", atf);
        }

        private void 表单管理ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormObjectForm fof = new FormObjectForm(this.User, this);
            AddTabPage("表单管理", fof);
        }

        private void 字段管理ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //FormItemForm fif = new FormItemForm(this.User);
            //AddTabPage("字段管理", fif);
        }

        private void 表单类型管理ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormTypeForm ftf = new FormTypeForm(this.User);
            AddTabPage("表单类型管理", ftf);
        }

        private void 回访类型管理ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FollowupTypeForm ftf = new FollowupTypeForm(this.User);
            AddTabPage("回访类型管理", ftf);
        }

        private void 回访结果管理ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FollowupResultForm frf = new FollowupResultForm(this.User);
            AddTabPage("回访结果管理", frf);
        }

        private void 续卡登记ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RenewForm rf = new RenewForm(this.User);
            AddTabPage("续卡信息", rf);
        }

        private void 私教登记ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PersonalTrainForm ptf = new PersonalTrainForm(this.User);
            AddTabPage("私教信息", ptf);
        }

        private void 私教查询ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PersonalTrainForm ptf = new PersonalTrainForm(this.User, 1);
            AddTabPage("私教信息", ptf);
        }

        private void 工作计划ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            WorkplanForm ptf = new WorkplanForm(this.User);
            AddTabPage("工作计划", ptf);
        }

        private void 工作日报ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            WorklogForm ptf = new WorklogForm(this.User);
            AddTabPage("工作日报", ptf);
        }

        private void 员工状态ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StaffConditionForm scf = new StaffConditionForm(this.User);
            AddTabPage("员工状态", scf);
        }

        private void 文档列表ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DocListForm dlf = new DocListForm(this.User, this);
            AddTabPage("财务文档列表", dlf);
        }

        private void 新建文档ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NewDocForm ndf = new NewDocForm(this.User, this);
            AddTabPage("新建财务文档", ndf);
        }

        private void 文档列表ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            DocListForm dlf = new DocListForm(this.User, this);
            AddTabPage("前台文档列表", dlf);
        }

        private void 新建文档ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            NewDocForm ndf = new NewDocForm(this.User, this);
            AddTabPage("新建前台文档", ndf);
        }

        private void 文档列表ToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            DocListForm dlf = new DocListForm(this.User, this);
            AddTabPage("行政文档列表", dlf);
        }

        private void 新建文档ToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            NewDocForm ndf = new NewDocForm(this.User, this);
            AddTabPage("新建行政文档", ndf);
        }

        private void 文档总览ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DocListForm dlf = new DocListForm(this.User, this, true);
            AddTabPage("全部文档列表", dlf);
        }

        private void 每日业绩ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DairyForm df = new DairyForm(this.User);
            AddTabPage("每日业绩", df);
        }

        private void 充值ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MoneyRecordForm mrf = new MoneyRecordForm(this.User);
            AddTabPage("充值登记", mrf);
        }

        private void 会员账户ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MemberMoneyForm mmf = new MemberMoneyForm(this.User);
            AddTabPage("会员账户", mmf);
        }

        private void 修改密码ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ChangePwdForm cpf = new ChangePwdForm(this.User);
            cpf.ShowDialog();
        }

        private void RefreshNetworkInfo()
        {
            if (netMonitor.Adapters.Count() > 0)
            {
                StringBuilder sb = new StringBuilder();
                int i = 0;
                foreach (NetworkAdapter adp in netMonitor.Adapters)
                {
                    i++;
                    string netAvanda = "[网卡(" + i.ToString() + "): " + Math.Round(adp.DownloadSpeedKbps, 2) + "Kbps/" + Math.Round(adp.UploadSpeedKbps, 2) + "Kbps]";
                    if (sb.Length == 0)
                        sb.Append(netAvanda);
                    else
                        sb.Append(" " + netAvanda);
                }
                Action<string> a = new Action<string>(RefreshNet);
                a.BeginInvoke(sb.ToString(), null, null);
            }
        }
        private void RefreshNet(string info)
        {
            if (statusStrip1.InvokeRequired)
            {
                statusStrip1.BeginInvoke(new Action<string>(a =>
                {
                    toolStripStatusLabel5.Text = a;
                    statusStrip1.Refresh();
                }), info);
            }
            else
            {
                toolStripStatusLabel5.Text = info;
                statusStrip1.Refresh();
            }
        }
    }
}
