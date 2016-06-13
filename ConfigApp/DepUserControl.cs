using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TopFashion
{
    [Serializable]
    public partial class DepUserControl : UserControl
    {
        public DepUserControl()
        {
            InitializeComponent();
        }

        public event EventHandler<UserArgs> SelectedUser;

        [Browsable(false)]
        public List<User> SelectedUsers
        {
            get
            {
                List<User> staffs = new List<User>();
                foreach (object o in listBox1.SelectedItems)
                {
                    User u = o as User;
                    if (u != null)
                    {
                        staffs.Add(u);
                    }
                }
                return staffs;
            }
            set
            {
                listBox1.Items.Clear();
                if (value != null)
                {
                    listBox1.Items.AddRange(value.ToArray());
                    for (int index = 0; index < listBox1.Items.Count; index++)
                    {
                        listBox1.SetSelected(index, true);
                    }
                    foreach (User user in value)
                    {
                        foreach (TreeNode node in treeView1.Nodes)
                        {
                            ExpandDep(user, node);
                        }
                    }
                }
            }
        }

        private static void ExpandDep(User user, TreeNode node)
        {
            Department dep = node.Tag as Department;
            if (dep != null)
            {
                if (user.BelongToDepart(dep, false))
                {
                    node.Expand();
                }
                else
                {
                    foreach (TreeNode sub in node.Nodes)
                    {
                        ExpandDep(user, node);
                    }
                }
            }
        }
        /// <summary>
        /// 获取或设置是否为单选
        /// </summary>
        [Browsable(true)]
        public bool SelectOnlyOne
        {
            get
            {
                return listBox1.SelectionMode == SelectionMode.One;
            }
            set
            {
                if (value)
                    listBox1.SelectionMode = SelectionMode.One;
                else
                    listBox1.SelectionMode = SelectionMode.MultiExtended;
            }
        }

        private bool findAllUserByDep = false;

        [Browsable(true)]
        public bool FindAllUserByDep
        {
            get { return findAllUserByDep; }
            set { findAllUserByDep = value; }
        }

        public void LoadDepartments(List<Department> deps)
        {
            LoadDepsToTree(deps);
        }

        public void LoadUsers(List<User> users)
        {
            listBox1.Tag = users;
        }

        internal void LoadDepsToTree(List<Department> deps)
        {
            treeView1.Nodes.Clear();
            if (deps != null)
            {
                foreach (Department dep in deps)
                {
                    if (dep.Parent == null)
                    {
                        TreeNode node = new TreeNode(dep.Name);
                        node.Name = dep.ID.ToString();
                        node.Tag = dep;
                        treeView1.Nodes.Add(node);
                        if (dep != null)
                        {
                            LoadChildren(node, dep, deps);
                        }
                    }
                }
            }
            treeView1.Tag = deps;
        }

        private void treeView1_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            TreeNode node = e.Node;
            List<Department> deps = node.TreeView.Tag as List<Department>;
            if (deps != null)
            {
                foreach (TreeNode nod in node.Nodes)
                {
                    Department dep = nod.Tag as Department;
                    if (dep != null)
                    {
                        LoadChildren(nod, dep, deps);
                    }
                }
            }
        }

        private static void LoadChildren(TreeNode node, Department dep, List<Department> deps)
        {
            List<TreeNode> children = new List<TreeNode>();
            foreach (Department d in deps)
            {
                if (dep.IsMyChildren(d, false))
                {
                    TreeNode nod = new TreeNode(d.Name);
                    nod.Name = d.ID.ToString();
                    nod.Tag = d;
                    children.Add(nod);
                }
            }
            node.Nodes.AddRange(children.ToArray());
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            List<User> users = listBox1.Tag as List<User>;
            if (users != null && e.Node != null)
            {
                Department dep = e.Node.Tag as Department;
                if (dep != null)
                {
                    List<User> urs = GetUsersByDep(dep, users);
                    if (findAllUserByDep)
                    {
                        List<Department> deps = e.Node.TreeView.Tag as List<Department>;
                        if (deps != null)
                        {
                            foreach (Department d in deps)
                            {
                                if (dep.IsMyChildren(d))
                                {
                                    List<User> ss = GetUsersByDep(d, users);
                                    foreach (User s in ss)
                                    {
                                        if (!urs.Contains(s))
                                            urs.Add(s);
                                    }
                                }
                            }
                        }
                    }
                    listBox1.Items.Clear();
                    listBox1.Items.AddRange(urs.ToArray());
                }
            }
        }

        private List<User> GetUsersByDep(Department dep, List<User> users)
        {
            List<User> urs = new List<User>();
            foreach (User user in users)
            {
                if (user.Departments.ContainsDepartment(dep))
                {
                    urs.Add(user);
                }
            }
            return urs;
        }

        private void DepUserControl_Resize(object sender, EventArgs e)
        {
            panel1.Width = this.Width / 2;
        }

        private void listBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (listBox1.SelectedIndex > -1)
            {
                if (SelectedUser != null)
                    SelectedUser(this, new UserArgs(listBox1.SelectedItem as User));
            }
        }
    }

    public class UserArgs : EventArgs
    {
        User user;

        public User User
        {
            get { return user; }
        }

        public UserArgs(User user)
        {
            this.user = user;
        }
    }
}
