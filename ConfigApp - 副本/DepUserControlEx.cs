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
    public partial class DepUserControlEx : UserControl
    {
        public DepUserControlEx()
        {
            InitializeComponent();
        }

        public void LoadDepartments(List<Department> deps)
        {
            depUserControl1.LoadDepsToTree(deps);
        }

        public void LoadUsers(List<User> users)
        {
            depUserControl1.listBox1.Tag = users;
        }

        [Browsable(false)]
        public List<User> SelectedUsers
        {
            get
            {
                List<User> staffs = new List<User>();
                foreach (object o in listBox1.Items)
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

        private void depUserControl1_SelectedUser(object sender, UserArgs e)
        {
            if (!SelectedUsers.Exists(u => u.ID == e.User.ID))
                listBox1.Items.Add(e.User);
        }
    }
}
