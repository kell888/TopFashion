using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;

namespace TopFashion
{
    internal static class ControlExtensions
    {
        /// <summary>
        /// 用于子窗体的排除权限
        /// </summary>
        /// <param name="control"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        internal static bool DisableForUser(this Control control, User user = null)
        {
            if (Configs.UseObsolete)
            {
                return Disable10(control, user);
            }
            else
            {
                return Disable1(control, user);
            }
        }

        private static bool Disable1(Control control, User user)
        {
            Form owner = control.FindForm();
            if (owner != null)
            {
                if (user == null)
                {
                    PermissionForm pf = owner as PermissionForm;
                    if (pf != null)
                    {
                        user = pf.User;
                    }
                }
                if (user != null)
                {
                    control.Enabled = true;
                    string formName = owner.Name;
                    string controlName = control.Name;
                    List<int> ps = user.GetAllPermissions();
                    if (ps.ExceptControl(formName, controlName))
                    {
                        control.Enabled = false;
                        return true;
                    }
                    else
                    {
                        foreach (Control c in control.Controls)
                        {
                            controlName = c.Name;
                            if (ps.ExceptControl(formName, control.Name, controlName))
                            {
                                c.Enabled = false;
                            }
                            else
                            {
                                ToolStrip ms = c as ToolStrip;
                                if (ms != null)
                                {
                                    ms.DisableForUser(user);
                                }
                                else
                                {
                                    c.DisableForUser(user);
                                }
                            }
                        }
                    }
                }
            }
            return false;
        }

        private static bool Disable10(Control control, User user)
        {
            Form owner = control.FindForm();
            if (owner != null)
            {
                if (user == null)
                {
                    PermissionForm pf = owner as PermissionForm;
                    if (pf != null)
                    {
                        user = pf.User;
                    }
                }
                if (user != null)
                {
                    control.Enabled = true;
                    string formName = owner.Name;
                    string controlName = control.Name;
                    PermissionCollection ps = user.GetAllPermissions(false);
                    if (ps.ExceptControl(formName, controlName))
                    {
                        control.Enabled = false;
                        return true;
                    }
                    else
                    {
                        foreach (Control c in control.Controls)
                        {
                            controlName = c.Name;
                            if (ps.ExceptControl(formName, control.Name, controlName))
                            {
                                c.Enabled = false;
                            }
                            else
                            {
                                ToolStrip ms = c as ToolStrip;
                                if (ms != null)
                                {
                                    ms.DisableForUser(user);
                                }
                                else
                                {
                                    c.DisableForUser(user);
                                }
                            }
                        }
                    }
                }
            }
            return false;
        }
        /// <summary>
        /// 用于主窗体的包含权限
        /// </summary>
        /// <param name="control"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        internal static bool EnableChildrenForUser(this Control control, User user = null)
        {
            if (Configs.UseObsolete)
            {
                return Enable10(control, user);
            }
            else
            {
                return Enable1(control, user);
            }
        }

        private static bool Enable1(Control control, User user)
        {
            Form owner = control.FindForm();
            if (owner != null)
            {
                if (user == null)
                {
                    PermissionForm pf = owner as PermissionForm;
                    if (pf != null)
                    {
                        user = pf.User;
                    }
                }
                if (user != null)
                {
                    control.Enabled = true;
                    int nofinded = 0;
                    string formName = owner.Name;
                    List<int> ps = user.GetAllPermissions();
                    foreach (Control c in control.Controls)
                    {
                        string controlName = c.Name;
                        if (ps.ContainsControl(formName, control.Name, controlName))
                        {
                            c.Enabled = true;
                        }
                        else
                        {
                            c.Enabled = false;
                            nofinded++;
                            ToolStrip ms = c as ToolStrip;
                            if (ms != null)
                            {
                                if (ms.EnableChildrenForUser(user)) nofinded--;
                            }
                            else
                            {
                                if (c.EnableChildrenForUser(user)) nofinded--;
                            }
                        }
                    }
                    if (nofinded == control.Controls.Count) { control.Enabled = false; return false; }
                    else { return true; }
                }
            }
            return false;
        }

        private static bool Enable10(Control control, User user)
        {
            Form owner = control.FindForm();
            if (owner != null)
            {
                if (user == null)
                {
                    PermissionForm pf = owner as PermissionForm;
                    if (pf != null)
                    {
                        user = pf.User;
                    }
                }
                if (user != null)
                {
                    control.Enabled = true;
                    int nofinded = 0;
                    string formName = owner.Name;
                    PermissionCollection ps = user.GetAllPermissions(false);
                    foreach (Control c in control.Controls)
                    {
                        string controlName = c.Name;
                        if (ps.ContainsControl(formName, control.Name, controlName))
                        {
                            c.Enabled = true;
                        }
                        else
                        {
                            c.Enabled = false;
                            nofinded++;
                            ToolStrip ms = c as ToolStrip;
                            if (ms != null)
                            {
                                if (ms.EnableChildrenForUser(user)) nofinded--;
                            }
                            else
                            {
                                if (c.EnableChildrenForUser(user)) nofinded--;
                            }
                        }
                    }
                    if (nofinded == control.Controls.Count) { control.Enabled = false; return false; }
                    else { return true; }
                }
            }
            return false;
        }
        /// <summary>
        /// 用于主窗体的包含权限
        /// </summary>
        /// <param name="control"></param>
        internal static void EnableAllChildren(this Control control)
        {
            List<Control> cs = new List<Control>();
            if (!control.Enabled)
            {
                EnableParent(control, ref cs);
            }
            if (cs.Count > 0)
            {
                cs.Reverse();
                foreach (Control parent in cs)
                {
                    parent.Enabled = true;
                }
            }
            control.Enabled = true;
            foreach (Control c in control.Controls)
            {
                c.Enabled = true;
            }
        }

        private static void EnableParent(Control control, ref List<Control> cs)
        {
            Control parent = control.Parent;
            if (parent != null && !parent.Enabled)
            {
                cs.Add(parent);
                EnableParent(parent, ref cs);
            }
        }
    }

    internal static class ToolStripItemExtensions
    {
        /// <summary>
        /// 用于子窗体的排除权限
        /// </summary>
        /// <param name="menu"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        internal static bool DisableForUser(this ToolStripItem menu, User user = null)
        {
            Form owner = null;
            if (menu.Owner != null)
            {
                if (menu.Owner.Parent != null)
                {
                    owner = menu.Owner.Parent.FindForm();
                }
            }
            if (owner != null)
            {
                if (user == null)
                {
                    PermissionForm pf = owner as PermissionForm;
                    if (pf != null)
                    {
                        user = pf.User;
                    }
                }
                if (user != null)
                {
                    menu.Enabled = true;
                    string formName = owner.Name;
                    string controlName = menu.Name;
                    List<int> ps = user.GetAllPermissions();
                    if (ps.ExceptControl(formName, controlName))
                    {
                        menu.Enabled = false;
                        return true;
                    }
                    else
                    {
                        ToolStripDropDownItem dropmenu = menu as ToolStripDropDownItem;
                        if (dropmenu != null)
                        {
                            ToolStripItemCollection items = dropmenu.DropDownItems;
                            if (items != null)
                            {
                                foreach (ToolStripItem c in items)
                                {
                                    controlName = c.Name;
                                    if (ps.ExceptControl(formName, dropmenu.Name, controlName))
                                    {
                                        c.Enabled = false;
                                    }
                                    else
                                    {
                                        c.DisableForUser(user);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return false;
        }
        /// <summary>
        /// 用于子窗体的排除权限
        /// </summary>
        /// <param name="menu"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        internal static bool DisableForUser2(this ToolStripItem menu, User user = null)
        {
            Form owner = null;
            if (menu.Owner != null)
            {
                if (menu.Owner.Parent != null)
                {
                    owner = menu.Owner.Parent.FindForm();
                }
            }
            if (owner != null)
            {
                if (user == null)
                {
                    PermissionForm pf = owner as PermissionForm;
                    if (pf != null)
                    {
                        user = pf.User;
                    }
                }
                if (user != null)
                {
                    menu.Enabled = true;
                    string formName = owner.Name;
                    string controlName = menu.Name;
                    PermissionCollection ps = user.GetAllPermissions(false);
                    if (ps.ExceptControl(formName, controlName))
                    {
                        menu.Enabled = false;
                        return true;
                    }
                    else
                    {
                        ToolStripDropDownItem dropmenu = menu as ToolStripDropDownItem;
                        if (dropmenu != null)
                        {
                            ToolStripItemCollection items = dropmenu.DropDownItems;
                            if (items != null)
                            {
                                foreach (ToolStripItem c in items)
                                {
                                    controlName = c.Name;
                                    if (ps.ExceptControl(formName, dropmenu.Name, controlName))
                                    {
                                        c.Enabled = false;
                                    }
                                    else
                                    {
                                        c.DisableForUser2(user);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return false;
        }
        /// <summary>
        /// 用于子窗体的排除权限
        /// </summary>
        /// <param name="menuStrip"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        internal static bool DisableForUser(this ToolStrip menuStrip, User user = null)
        {
            if (Configs.UseObsolete)
            {
                return Disable20(menuStrip, user);
            }
            else
            {
                return Disable2(menuStrip, user);
            }
        }

        private static bool Disable2(ToolStrip menuStrip, User user)
        {
            Form owner = menuStrip.FindForm();
            if (owner != null)
            {
                if (user == null)
                {
                    PermissionForm pf = owner as PermissionForm;
                    if (pf != null)
                    {
                        user = pf.User;
                    }
                }
                if (user != null)
                {
                    menuStrip.Enabled = true;
                    string formName = owner.Name;
                    string controlName = menuStrip.Name;
                    List<int> ps = user.GetAllPermissions();
                    if (ps.ExceptControl(formName, controlName))
                    {
                        menuStrip.Enabled = false;
                        return true;
                    }
                    else
                    {
                        foreach (ToolStripItem c in menuStrip.Items)
                        {
                            controlName = c.Name;
                            if (ps.ExceptControl(formName, menuStrip.Name, controlName))
                            {
                                c.Enabled = false;
                            }
                            else
                            {
                                c.DisableForUser(user);
                            }
                        }
                    }
                }
            }
            return false;
        }

        private static bool Disable20(ToolStrip menuStrip, User user)
        {
            Form owner = menuStrip.FindForm();
            if (owner != null)
            {
                if (user == null)
                {
                    PermissionForm pf = owner as PermissionForm;
                    if (pf != null)
                    {
                        user = pf.User;
                    }
                }
                if (user != null)
                {
                    menuStrip.Enabled = true;
                    string formName = owner.Name;
                    string controlName = menuStrip.Name;
                    PermissionCollection ps = user.GetAllPermissions(false);
                    if (ps.ExceptControl(formName, controlName))
                    {
                        menuStrip.Enabled = false;
                        return true;
                    }
                    else
                    {
                        foreach (ToolStripItem c in menuStrip.Items)
                        {
                            controlName = c.Name;
                            if (ps.ExceptControl(formName, menuStrip.Name, controlName))
                            {
                                c.Enabled = false;
                            }
                            else
                            {
                                c.DisableForUser2(user);
                            }
                        }
                    }
                }
            }
            return false;
        }
        /// <summary>
        /// 用于主窗体的包含权限
        /// </summary>
        /// <param name="menu"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        internal static bool EnableChildrenForUser(this ToolStripItem menu, User user = null)
        {
            Form owner = null;
            if (menu.Owner != null)
            {
                if (menu.Owner.Parent != null)
                {
                    owner = menu.Owner.Parent.FindForm();
                }
            }
            if (owner != null)
            {
                if (user == null)
                {
                    PermissionForm pf = owner as PermissionForm;
                    if (pf != null)
                    {
                        user = pf.User;
                    }
                }
                if (user != null)
                {
                    ToolStripDropDownItem dropmenu = menu as ToolStripDropDownItem;
                    if (dropmenu != null)
                    {
                        ToolStripItemCollection items = dropmenu.DropDownItems;
                        if (items != null)
                        {
                            dropmenu.Enabled = true;
                            int nofinded = 0;
                            string formName = owner.Name;
                            List<int> ps = user.GetAllPermissions();
                            foreach (ToolStripItem c in items)
                            {
                                string controlName = c.Name;
                                if (ps.ContainsControl(formName, dropmenu.Name, controlName))
                                {
                                    c.Enabled = true;
                                }
                                else
                                {
                                    c.Enabled = false;
                                    nofinded++;
                                    if (c.EnableChildrenForUser(user)) nofinded--;
                                }
                            }
                            if (nofinded == items.Count) { dropmenu.Enabled = false; return false; }
                            else { return true; }
                        }
                    }
                }
            }
            return false;
        }
        /// <summary>
        /// 用于主窗体的包含权限
        /// </summary>
        /// <param name="menu"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        internal static bool EnableChildrenForUser2(this ToolStripItem menu, User user = null)
        {
            Form owner = null;
            if (menu.Owner != null)
            {
                if (menu.Owner.Parent != null)
                {
                    owner = menu.Owner.Parent.FindForm();
                }
            }
            if (owner != null)
            {
                if (user == null)
                {
                    PermissionForm pf = owner as PermissionForm;
                    if (pf != null)
                    {
                        user = pf.User;
                    }
                }
                if (user != null)
                {
                    ToolStripDropDownItem dropmenu = menu as ToolStripDropDownItem;
                    if (dropmenu != null)
                    {
                        ToolStripItemCollection items = dropmenu.DropDownItems;
                        if (items != null)
                        {
                            dropmenu.Enabled = true;
                            int nofinded = 0;
                            string formName = owner.Name;
                            PermissionCollection ps = user.GetAllPermissions(false);
                            foreach (ToolStripItem c in items)
                            {
                                string controlName = c.Name;
                                if (ps.ContainsControl(formName, dropmenu.Name, controlName))
                                {
                                    c.Enabled = true;
                                }
                                else
                                {
                                    c.Enabled = false;
                                    nofinded++;
                                    if (c.EnableChildrenForUser2(user)) nofinded--;
                                }
                            }
                            if (nofinded == items.Count) { dropmenu.Enabled = false; return false; }
                            else { return true; }
                        }
                    }
                }
            }
            return false;
        }
        /// <summary>
        /// 用于主窗体的包含权限
        /// </summary>
        /// <param name="menuStrip"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        internal static bool EnableChildrenForUser(this ToolStrip menuStrip, User user = null)
        {
            if (Configs.UseObsolete)
            {
                return Enable20(menuStrip, user);
            }
            else
            {
                return Enable2(menuStrip, user);
            }
        }

        private static bool Enable2(ToolStrip menuStrip, User user)
        {
            Form owner = menuStrip.FindForm();
            if (owner != null)
            {
                if (user == null)
                {
                    PermissionForm pf = owner as PermissionForm;
                    if (pf != null)
                    {
                        user = pf.User;
                    }
                }
                if (user != null)
                {
                    menuStrip.Enabled = true;
                    int nofinded = 0;
                    string formName = owner.Name;
                    List<int> ps = user.GetAllPermissions();
                    foreach (ToolStripItem c in menuStrip.Items)
                    {
                        string controlName = c.Name;
                        if (ps.ContainsControl(formName, menuStrip.Name, controlName))
                        {
                            c.Enabled = true;
                        }
                        else
                        {
                            c.Enabled = false;
                            nofinded++;
                            if (c.EnableChildrenForUser(user)) nofinded--;
                        }
                    }
                    if (nofinded == menuStrip.Items.Count) { menuStrip.Enabled = false; return false; }
                    else { return true; }
                }
            }
            return false;
        }

        private static bool Enable20(ToolStrip menuStrip, User user)
        {
            Form owner = menuStrip.FindForm();
            if (owner != null)
            {
                if (user == null)
                {
                    PermissionForm pf = owner as PermissionForm;
                    if (pf != null)
                    {
                        user = pf.User;
                    }
                }
                if (user != null)
                {
                    menuStrip.Enabled = true;
                    int nofinded = 0;
                    string formName = owner.Name;
                    PermissionCollection ps = user.GetAllPermissions(false);
                    foreach (ToolStripItem c in menuStrip.Items)
                    {
                        string controlName = c.Name;
                        if (ps.ContainsControl(formName, menuStrip.Name, controlName))
                        {
                            c.Enabled = true;
                        }
                        else
                        {
                            c.Enabled = false;
                            nofinded++;
                            if (c.EnableChildrenForUser2(user)) nofinded--;
                        }
                    }
                    if (nofinded == menuStrip.Items.Count) { menuStrip.Enabled = false; return false; }
                    else { return true; }
                }
            }
            return false;
        }
    }
}
