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
    public partial class DepStaffControl : UserControl
    {
        public DepStaffControl()
        {
            InitializeComponent();
        }

        public event EventHandler<StaffArgs> SelectedStaff;

        [Browsable(false)]
        public List<Staff> SelectedStaffs
        {
            get
            {
                List<Staff> staffs = new List<Staff>();
                foreach (object o in listBox1.SelectedItems)
                {
                    Staff s = o as Staff;
                    if (s != null)
                    {
                        staffs.Add(s);
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
                    foreach (Staff staff in value)
                    {
                        foreach (TreeNode node in treeView1.Nodes)
                        {
                            ExpandDep(staff, node);
                        }
                    }
                }
            }
        }

        private static void ExpandDep(Staff staff, TreeNode node)
        {
            Department dep = node.Tag as Department;
            if (dep != null)
            {
                if (staff.BelongToDepart(dep, false))
                {
                    node.Expand();
                }
                else
                {
                    foreach (TreeNode sub in node.Nodes)
                    {
                        ExpandDep(staff, node);
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

        private bool findAllStaffByDep = false;

        [Browsable(true)]
        public bool FindAllStaffByDep
        {
            get { return findAllStaffByDep; }
            set { findAllStaffByDep = value; }
        }

        public void LoadDepartments(List<Department> deps)
        {
            LoadDepsToTree(deps);
        }

        public void LoadStaffs(List<Staff> staffs)
        {
            listBox1.Tag = staffs;
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
            List<Staff> staffs = listBox1.Tag as List<Staff>;
            if (staffs != null && e.Node != null)
            {
                Department dep = e.Node.Tag as Department;
                if (dep != null)
                {
                    List<Staff> sts = GetStaffsByDep(dep, staffs);
                    if (findAllStaffByDep)
                    {
                        List<Department> deps = e.Node.TreeView.Tag as List<Department>;
                        if (deps != null)
                        {
                            foreach (Department d in deps)
                            {
                                if (dep.IsMyChildren(d))
                                {
                                    List<Staff> ss = GetStaffsByDep(d, staffs);
                                    foreach (Staff s in ss)
                                    {
                                        if (!sts.Contains(s))
                                            sts.Add(s);
                                    }
                                }
                            }
                        }
                    }
                    listBox1.Items.Clear();
                    listBox1.Items.AddRange(sts.ToArray());
                }
            }
        }

        private List<Staff> GetStaffsByDep(Department dep, List<Staff> staffs)
        {
            List<Staff> sts = new List<Staff>();
            foreach (Staff staff in staffs)
            {
                if (staff.Depart.ID == dep.ID)
                {
                    sts.Add(staff);
                }
            }
            return sts;
        }

        private void DepSatffControl_Resize(object sender, EventArgs e)
        {
            panel1.Width = this.Width / 2;
        }

        private void listBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (listBox1.SelectedIndex > -1)
            {
                if (SelectedStaff != null)
                    SelectedStaff(this, new StaffArgs(listBox1.SelectedItem as Staff));
            }
        }
    }

    public class StaffArgs : EventArgs
    {
        Staff staff;

        public Staff Staff
        {
            get { return staff; }
        }

        public StaffArgs(Staff staff)
        {
            this.staff = staff;
        }
    }
}
