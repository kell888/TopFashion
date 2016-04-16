using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.IO;

namespace TopFashion
{
    static class Program
    {
        [DllImport("user32.dll")]
        static extern bool FlashWindow(IntPtr hWnd, bool bInvert);
        /// <summary>
        /// 该函数设置由不同线程产生的窗口的显示状态
        /// </summary>
        /// <param name="hWnd">窗口句柄</param>
        /// <param name="cmdShow">指定窗口如何显示。查看允许值列表，请查阅ShowWlndow函数的说明部分</param>
        /// <returns>如果函数原来可见，返回值为非零；如果函数原来被隐藏，返回值为零</returns>
        [DllImport("User32.dll")]
        private static extern bool ShowWindowAsync(IntPtr hWnd, int cmdShow);
        /// <summary>
        ///  该函数将创建指定窗口的线程设置到前台，并且激活该窗口。键盘输入转向该窗口，并为用户改各种可视的记号。
        ///  系统给创建前台窗口的线程分配的权限稍高于其他线程。 
        /// </summary>
        /// <param name="hWnd">将被激活并被调入前台的窗口句柄</param>
        /// <returns>如果窗口设入了前台，返回值为非零；如果窗口未被设入前台，返回值为零</returns>
        [DllImport("User32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        private const int SW_SHOWNOMAL = 1;

        private static void HandleRunningInstance(Process instance)
        {
            ShowWindowAsync(instance.MainWindowHandle, SW_SHOWNOMAL);//显示
            SetForegroundWindow(instance.MainWindowHandle);//推到最前端
        }

        private static Process RuningInstance()
        {
            Process currentProcess = Process.GetCurrentProcess();
            Process[] Processes = Process.GetProcessesByName(currentProcess.ProcessName);
            foreach (Process process in Processes)
            {
                if (process.Id != currentProcess.Id)
                {
                    //string path = Assembly.GetExecutingAssembly().Location;
                    //if (path.Replace("/", "\\") == currentProcess.MainModule.FileName)
                    string current = new FileInfo(currentProcess.MainModule.FileName).Name.ToLower();
                    string This = new FileInfo(process.MainModule.FileName).Name.ToLower();
                    if (This == current)
                    {
                        return process;
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            //CreateDesktopShortCut();
            Process process = RuningInstance();
            if (process == null)
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(MainForm.GetInstance());
            }
            else
            {
                MessageBox.Show("该应用程序已经在运行中，请留意任务栏或托盘区！");
                HandleRunningInstance(process);
                FlashWindow(process.MainWindowHandle, true);
                //System.Threading.Thread.Sleep(1000);
                //System.Environment.Exit(1);
            }
            //bool createNew;
            //using (System.Threading.Mutex mutex = new System.Threading.Mutex(true, Application.ProductName, out createNew))
            //{
            //    if (createNew)
            //    {
            //        Application.EnableVisualStyles();
            //        Application.SetCompatibleTextRenderingDefault(false);
            //        Application.Run(Form1.GetInstance());
            //    }
            //    else
            //    {
            //        MessageBox.Show("应用程序已经在运行中...");
            //        System.Threading.Thread.Sleep(1000);
            //        System.Environment.Exit(1);
            //    }
            //}
        }

        private static void CreateDesktopShortCut()
        {
            string path = System.Environment.GetFolderPath(Environment.SpecialFolder.Programs) + @"\鼎峰时尚健身中心";
            if (System.IO.Directory.Exists(path))
            {
                string desktop = System.Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
                if (!desktop.EndsWith("\\"))
                {
                    desktop += "\\";
                }
                foreach (String file in System.IO.Directory.GetFiles(path))
                {
                    System.IO.FileInfo fi = new System.IO.FileInfo(file);
                    if (!System.IO.File.Exists(desktop + fi.Name))
                    {
                        fi.CopyTo(desktop + fi.Name);
                    }
                }
            }
        }
    }
}
