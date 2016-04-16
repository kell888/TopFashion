using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TopFashion
{
    /// <summary>
    /// WPF 定义刷新父界面
    /// </summary>
    /// <param name="sender">发布者</param>
    /// <param name="cname">控件名</param>
    public delegate void RefreshControlSender(object sender, object cname);
    public class RefreshControlModel
    {
        public static event RefreshControlSender refreshSender;
        public static void RefreshControl(object sender, object cname)
        {
            if (refreshSender != null)
                refreshSender(sender, cname);
        }
    }
}
