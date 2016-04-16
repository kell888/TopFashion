using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;

namespace TopFashion
{
    public interface IPermission
    {
        User User { get; set; }
        int AdminId { get; }
        void CheckUserPermission(Form child);
        void DisableUserPermission(Form child);
    }

    public class PermissionForm : Form, IPermission
    {
        private User user;
        /// <summary>
        /// 当前用户
        /// </summary>
        [Browsable(false)]
        public User User
        {
            get { return user; }
            set { user = value; }
        }
        /// <summary>
        /// 管理员ID
        /// </summary>
        public int AdminId
        {
            get
            {
                return Common.AdminId;
            }
        }
        /// <summary>
        /// 用于主窗体的包含权限
        /// </summary>
        /// <param name="child"></param>
        public void CheckUserPermission(Form child)
        {
            child.EnableChildrenForUser();
        }
        /// <summary>
        /// 用于子窗体的排除权限
        /// </summary>
        /// <param name="child"></param>
        public void DisableUserPermission(Form child)
        {
            child.DisableForUser();
        }
    }
}
