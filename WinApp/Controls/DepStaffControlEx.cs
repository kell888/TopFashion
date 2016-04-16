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
    public partial class DepStaffControlEx : UserControl
    {
        public DepStaffControlEx()
        {
            InitializeComponent();
        }

        public void LoadDepartments(List<Department> deps)
        {
            depStaffControl1.LoadDepsToTree(deps);
        }

        public void LoadUsers(List<User> users)
        {
            depStaffControl1.listBox1.Tag = users;
        }

        [Browsable(false)]
        public List<Staff> SelectedStaffs
        {
            get
            {
                List<Staff> staffs = new List<Staff>();
                foreach (object o in listBox1.Items)
                {
                    Staff u = o as Staff;
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
                }
            }
        }

        private void listBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (listBox1.SelectedIndex > -1)
            {
                listBox1.Items.RemoveAt(listBox1.SelectedIndex);
            }
        }

        private void depStaffControl1_SelectedStaff(object sender, StaffArgs e)
        {
            if (!SelectedStaffs.Exists(u => u.ID == e.Staff.ID))
                listBox1.Items.Add(e.Staff);
        }
    }
}
