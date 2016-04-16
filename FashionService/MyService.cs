using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Timers;
using TopFashion;
using System.Threading;
using System.Configuration;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace FashionService
{
    public partial class MyService : ServiceBase
    {
        public MyService()
        {
            InitializeComponent();
        }
        KellFileTransfer.ReceiveListenerArgs rl;
        private System.Timers.Timer triggerTimer;

        protected override void OnStart(string[] args)
        {
            triggerTimer = new System.Timers.Timer();
            // 循环间隔时间(1分钟)
            triggerTimer.Interval = 60000;
            triggerTimer.Elapsed += new ElapsedEventHandler(triggerTimer_Elapsed);
            triggerTimer.Start();
            ThreadPool.QueueUserWorkItem(
                delegate
                {
                    string dir = ConfigurationManager.AppSettings["dir"];
                    if (string.IsNullOrEmpty(dir) || !Directory.Exists(dir))
                    {
                        dir = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                    }
                    if (!Directory.Exists(dir))
                        Directory.CreateDirectory(dir);
                    KellFileTransfer.Common.SaveAppSettingConfig("dir", dir);
                    IPEndPoint ipepUpload = KellFileTransfer.Common.GetUploadIPEndPoint();
                    IPEndPoint ipepDownload = KellFileTransfer.Common.GetDownloadIPEndPoint();
                    try
                    {
                        rl = KellFileTransfer.FileUploader.StartReceiveFile(dir, ipepUpload.Address, ipepUpload.Port);
                        if (rl.Thread.ThreadState != System.Threading.ThreadState.Running || rl.Listener == null)
                            WriteLog.CreateLog("服务程序", "StartReceiveFile", "error", "开启上传监听服务失败！[" + ipepUpload.ToString() + "]");
                        KellFileTransfer.FileDownloadServer server = new KellFileTransfer.FileDownloadServer(dir);
                        if (!server.StartDownloadListen(ipepDownload.Address, ipepDownload.Port))
                            WriteLog.CreateLog("服务程序", "StartDownloadListen", "error", "开启下载监听服务失败！[" + ipepDownload.ToString() + "]");
                    }
                    catch (Exception e)
                    {
                        WriteLog.CreateLog("服务程序", "鼎峰健身服务OnStart", "error", e.ToString());
                    }
                });
            WriteLog.CreateLog("服务程序", "鼎峰健身服务", "log", "服务启动...");
        }

        void triggerTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            triggerTimer.Stop();
            try
            {
                DateTime dtNow = DateTime.Now;
                AlertLogic al = AlertLogic.GetInstance();
                //发短信
                List<Alert> alerts = al.GetAlertsByType((int)提醒方式.员工短信);//Configs.SmsAlertTypeStaff);
                foreach (Alert a in alerts)
                {
                    if (a.Flag == 0 && a.提醒时间 > dtNow)
                    {
                        string[] dest = a.提醒对象.Split(",".ToArray(), StringSplitOptions.RemoveEmptyEntries);
                        List<string> mobiles = new List<string>();
                        List<string> greets = new List<string>();
                        StaffLogic sl = StaffLogic.GetInstance();
                        foreach (string d in dest)
                        {
                            Staff s = sl.GetStaff(Convert.ToInt32(d));
                            mobiles.Add(s.电话);
                            greets.Add(s.姓名);
                        }
                        if (SMSLogic.SendSMS(a.提醒项目, mobiles, greets))
                        {
                            al.SetFlag(a.ID, 1);
                        }
                    }
                }
                List<Alert> alerts2 = al.GetAlertsByType((int)提醒方式.会员短信);//Configs.SmsAlertTypeMember);
                foreach (Alert a in alerts2)
                {
                    if (a.Flag == 0 && a.提醒时间 > dtNow)
                    {
                        string[] dest = a.提醒对象.Split(",".ToArray(), StringSplitOptions.RemoveEmptyEntries);
                        List<string> mobiles = new List<string>();
                        List<string> greets = new List<string>();
                        foreach (string d in dest)
                        {
                            Member m = MemberLogic.GetInstance().GetMember(Convert.ToInt32(d));
                            mobiles.Add(m.电话);
                            greets.Add(m.姓名);
                        }
                        if (SMSLogic.SendSMS(a.提醒项目, mobiles, greets))
                        {
                            al.SetFlag(a.ID, 1);
                        }
                    }
                }
                //int hour = dtNow.Hour;
                //if (hour == 2)  //凌晨2点钟计算财务
                //{
                //    WriteLog.CreateLog("定时服务", "log", "执行任务开始...");


                //    WriteLog.CreateLog("定时服务", "log", dtNow.ToString() + "执行任务完毕.");
                //}
            }
            catch (Exception ex)
            {
                WriteLog.CreateLog("服务程序", "MyService.triggerTimer_Elapsed", "error", ex.ToString());
            }
            finally
            {
                triggerTimer.Start();
            }
        }

        protected override void OnStop()
        {
            try
            {
                if (triggerTimer.Enabled)
                    triggerTimer.Stop();
                triggerTimer.Dispose();
                bool flag = false;
                if (rl != null)
                {
                    flag = KellFileTransfer.FileUploader.StopReceiveFile(rl);
                }
                if (!flag)
                    WriteLog.CreateLog("服务程序", "鼎峰健身服务", "log", "服务已停止。但是文件传送服务停止失败！");
                else
                    WriteLog.CreateLog("服务程序", "鼎峰健身服务", "log", "服务顺利停止。");
            }
            catch (Exception e)
            {
                WriteLog.CreateLog("服务程序", "鼎峰健身服务OnStop", "error", e.ToString());
            }
        }
    }
}
