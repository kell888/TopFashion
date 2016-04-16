using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using Microsoft.Win32;
using System.IO;
using System.Drawing;
using System.Diagnostics;

namespace TopFashion
{
    //系统关机类
    public static class OSOperation
    {
        //关机方法
        public static void ShutDown()
        {
            Process myProcess = new Process();//定义一个线程实例；
            myProcess.StartInfo.FileName = "cmd.exe";//启动名为"cmd.exe"的线程，就相当于你点击电脑左下角的"开机"-"运行"-输入cmd后回车，也就是启动了命令提示符界面；
            myProcess.StartInfo.UseShellExecute = false;//关闭Shell的使用
            myProcess.StartInfo.RedirectStandardInput = true;//重定向标准输入
            myProcess.StartInfo.RedirectStandardOutput = true;//重定向标准输出
            myProcess.StartInfo.RedirectStandardError = true;//重定向错误输出
            myProcess.StartInfo.CreateNoWindow = true;//设置不显示窗口
            myProcess.Start();//此处才是启动了该线程
            myProcess.StandardInput.WriteLine("shutdown -s -t 10");//相当于你在命令提示符界面输入"shutdown -s -t 0"，只不过不显示窗口界面。
        }

        #region 关机、注销、重启的相关方法
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        internal struct TokPriv1Luid
        {
            public int Count;
            public long Luid;
            public int Attr;
        }

        [DllImport("kernel32.dll", ExactSpelling = true)]
        internal static extern IntPtr GetCurrentProcess();

        [DllImport("advapi32.dll", ExactSpelling = true, SetLastError = true)]
        internal static extern bool OpenProcessToken(IntPtr h, int acc, ref IntPtr phtok);

        [DllImport("advapi32.dll", SetLastError = true)]
        internal static extern bool LookupPrivilegeValue(string host, string name, ref long pluid);

        [DllImport("advapi32.dll", ExactSpelling = true, SetLastError = true)]
        internal static extern bool AdjustTokenPrivileges(IntPtr htok, bool disall,
        ref TokPriv1Luid newst, int len, IntPtr prev, IntPtr relen);

        [DllImport("user32.dll", ExactSpelling = true, SetLastError = true)]
        internal static extern bool ExitWindowsEx(int DoFlag, int rea);

        internal const int SE_PRIVILEGE_ENABLED = 0x00000002;
        internal const int TOKEN_QUERY = 0x00000008;
        internal const int TOKEN_ADJUST_PRIVILEGES = 0x00000020;
        internal const string SE_SHUTDOWN_NAME = "SeShutdownPrivilege";
        internal const int EWX_LOGOFF = 0x00000000;
        internal const int EWX_SHUTDOWN = 0x00000001;
        internal const int EWX_REBOOT = 0x00000002;
        internal const int EWX_FORCE = 0x00000004;
        internal const int EWX_POWEROFF = 0x00000008;
        internal const int EWX_FORCEIFHUNG = 0x00000010;

        private static void DoExitWin(int DoFlag)
        {
            bool ok;
            TokPriv1Luid tp;
            IntPtr hproc = GetCurrentProcess();
            IntPtr htok = IntPtr.Zero;
            ok = OpenProcessToken(hproc, TOKEN_ADJUST_PRIVILEGES | TOKEN_QUERY, ref htok);
            tp.Count = 1;
            tp.Luid = 0;
            tp.Attr = SE_PRIVILEGE_ENABLED;
            ok = LookupPrivilegeValue(null, SE_SHUTDOWN_NAME, ref tp.Luid);
            ok = AdjustTokenPrivileges(htok, false, ref tp, 0, IntPtr.Zero, IntPtr.Zero);
            ok = ExitWindowsEx(DoFlag, 0);
        }

        /// <summary>
        /// 重新启动
        /// </summary>
        public static void Reboot()
        {
            DoExitWin(EWX_FORCE | EWX_REBOOT);
        }

        /// <summary>
        /// 关机
        /// </summary>
        public static void PowerOff()
        {
            DoExitWin(EWX_FORCE | EWX_POWEROFF);
        }
        public static void GuanJi()
        {
            try
            {
                Process.Start("Shutdown.exe", "-s -t 0");
            }
            catch (Exception ex)
            {
                WriteLog.CreateLog("内部操作", "OSOperation类的GuanJi函数", "error", ex.Message); 
            }
        }

        /// <summary>
        /// 注销
        /// </summary>
        public static void LogOff()
        {
            DoExitWin(EWX_FORCE | EWX_LOGOFF);
        }
        public static void ZhuXiao()
        {
            ExitWindowsEx(0, 0);
        }

        public const uint WM_SYSCOMMAND = 0x0112;
        public const uint SC_MONITORPOWER = 0xF170;
        [DllImport("user32")]
        public static extern IntPtr SendMessage(IntPtr hwnd, uint wMsg, uint wParam, int lParam);
        /// <summary>
        /// 关闭,打开显示器
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void CloseLCD(object sender, EventArgs e)
        {
            SendMessage(IntPtr.Zero, WM_SYSCOMMAND, SC_MONITORPOWER, 2);
        }
        public static void OpenLCD(object sender, EventArgs e)
        {
            SendMessage(IntPtr.Zero, WM_SYSCOMMAND, SC_MONITORPOWER, -1);
        }

        /// <summary>
        /// 打开，关闭光驱
        /// </summary>
        private static bool _cdOpen = true;
        public static bool CdOpen
        {
            get { return _cdOpen; }
            set { _cdOpen = value; }
        }
        [DllImport("winmm.dll", EntryPoint = "mciSendStringA")]
        public static extern int mciSendString(string lpstrCommand, StringBuilder lpstrReturnString, int uReturnLength, IntPtr hwndCallback);
        #endregion

        #region
        /// <summary>
        /// 截屏的方法
        /// </summary>
        public static void CutScreen(string templatePath, int width, int height, int x, int y)
        {
            try
            {
                string tempImagePath = templatePath;
                string temp = tempImagePath + "\\CurrentScreenImage\\";
                if (!Directory.Exists(temp))
                    Directory.CreateDirectory(@temp);
                Image i = new Bitmap(width, height);
                Graphics g = Graphics.FromImage(i);
                g.CopyFromScreen(new Point(x, y), new Point(0, 0), new Size(width, height));
                i.Save(@temp + DateTime.Now.ToLocalTime().Ticks.ToString() + ".jpg");
            }
            catch (Exception ex)
            {
                WriteLog.CreateLog("内部操作", "OSOperation类的CutScreen函数", "error", ex.Message); 
            }
        }
        #endregion

        #region
        /// <summary>
        /// 锁定系统的方法
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct LASTINPUTINFO
        {
            [MarshalAs(UnmanagedType.U4)]
            public int cbSize;
            [MarshalAs(UnmanagedType.U4)]
            public uint dwTime;
        }

        [DllImport("user32.dll")]
        public static extern bool GetLastInputInfo(ref LASTINPUTINFO plii);
        public static long getIdleTick()
        {
            LASTINPUTINFO vLastInputInfo = new LASTINPUTINFO();
            vLastInputInfo.cbSize = Marshal.SizeOf(vLastInputInfo);
            if (!GetLastInputInfo(ref　 vLastInputInfo)) return 0;
            return Environment.TickCount - (long)vLastInputInfo.dwTime;
        }
        #endregion

        #region
        /// <summary>
        /// 开机自动启动的方法
        /// </summary>
        /// <param name="strName">程序路径</param>
        /// <param name="isStart">是否开机启动</param>
        public static void OnStart(string strName, bool isStart)
        {
            //string strName = Application.ExecutablePath;
            string strnewName = strName.Substring(strName.LastIndexOf(@"\") + 1);
            if (isStart)
            {
                if (System.IO.File.Exists(strName))
                {
                    RegistryKey Rkey = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true);
                    if (Rkey == null)
                    {
                        Rkey = Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run");
                    }
                    Rkey.SetValue(strnewName, strName);
                }
            }
            else
            {
                Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run").DeleteValue(strnewName, false);
            }
        }
        #endregion

        #region
        /// <summary>
        /// 检测是否联网
        /// </summary>
        private const int INTERNET_CONNECTION_MODEN = 1;
        private const int INTERNET_CONNECTION_LAN = 2;
        [DllImport("winInet.dll")]
        private static extern bool InternetGetConnectedState(ref int dwFlag, int dwReserved);
        /// <summary>
        /// 判断是否联网的方法
        /// </summary>
        /// <returns>0：未联网，1：调制解调器上网，2：网卡上网</returns>
        public static int JudgeInternetGetConnectedState()
        {
            System.Int32 flag = new int();
            if (!InternetGetConnectedState(ref flag, 0))
                flag = 0;
            else
                if ((flag & INTERNET_CONNECTION_MODEN) != 0)
                    flag = 1;
                else
                    if ((flag & INTERNET_CONNECTION_LAN) != 0)
                        flag = 2;

            return flag;
        }
        #endregion
    }
}
