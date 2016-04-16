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
    [DefaultEvent("Click")]
    [Serializable]
    public partial class SelectStaffControl : UserControl
    {
        public SelectStaffControl()
        {
            InitializeComponent();
            this.label1.Text = "选择员工...";
            this.Click += new EventHandler(SelectStaffControl_Click);
        }

        public static SelectStaffControl CreateInstance(bool selectOnlyOne)
        {
            SelectStaffControl ssc = new SelectStaffControl();
            ssc.selectOnlyOne = selectOnlyOne;
            return ssc;
        }

        void SelectStaffControl_Click(object sender, EventArgs e)
        {
            label1_Click(this, e);
        }

        bool selectOnlyOne;
        /// <summary>
        /// 获取或设置是否为单选
        /// </summary>
        [Browsable(true)]
        public bool SelectOnlyOne
        {
            get { return selectOnlyOne; }
            set { selectOnlyOne = value; }
        }

        [Browsable(false)]
        public List<Staff> SelectedStaffs
        {
            get
            {
                List<Staff> sts = null;
                if (this.label1.Tag != null)
                {
                    List<Staff> staffs = this.label1.Tag as List<Staff>;
                    if (staffs != null)
                        sts = staffs;
                    else
                        sts = new List<Staff>();
                }
                else
                {
                    sts = new List<Staff>();
                }
                return sts;
            }
            set
            {
                this.label1.Tag = value;
                if (value != null)
                {
                    StringBuilder sb = new StringBuilder();
                    foreach (Staff staff in value)
                    {
                        if (sb.Length == 0)
                            sb.Append(staff.姓名);
                        else
                            sb.Append("," + staff.姓名);
                    }
                    if (sb.Length > 0)
                        this.label1.Text = sb.ToString();
                    else
                        this.label1.Text = "选择员工...";
                }
                else
                {
                    this.label1.Text = "选择员工...";
                }
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {
            List<Department> allDeps = DepartmentLogic.GetInstance().GetAllDepartments();
            List<Staff> allStaffs = StaffLogic.GetInstance().GetAllStaffs();
            SelectStaffForm f = new SelectStaffForm(selectOnlyOne, this.SelectedStaffs, allDeps, allStaffs);
            if (f.ShowDialog() == DialogResult.OK)
            {
                this.label1.Tag = f.SelectedStaffs;
                if (f.SelectedStaffs != null && f.SelectedStaffs.Count > 0)
                {
                    StringBuilder sb = new StringBuilder();
                    foreach (Staff staff in f.SelectedStaffs)
                    {
                        if (sb.Length == 0)
                            sb.Append(staff.姓名);
                        else
                            sb.Append("," + staff.姓名);
                    }
                    this.label1.Text = sb.ToString();
                }
                else
                {
                    this.label1.Text = "选择员工...";
                }
            }
        }

        private void label1_MouseHover(object sender, EventArgs e)
        {
            toolTip1.SetToolTip(label1, label1.Text);
        }
    }
}
