using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TopFashion
{
    public partial class SelectStaffForm : Form
    {
        public SelectStaffForm(bool selectOnlyOne, List<Staff> staffs, List<Department> allDeps, List<Staff> allStaffs)
        {
            InitializeComponent();
            this.Load += new EventHandler(SelectStaffForm_Load);
            depStaffControl1.SelectOnlyOne = selectOnlyOne;
            this.SelectedStaffs = staffs;
            depStaffControl1.LoadDepartments(allDeps);
            depStaffControl1.LoadStaffs(allStaffs);
        }

        void SelectStaffForm_Load(object sender, EventArgs e)
        {
        }

        public List<Staff> SelectedStaffs
        {
            get
            {
                return depStaffControl1.SelectedStaffs;
            }
            set
            {
                depStaffControl1.SelectedStaffs = value;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }
    }
}
