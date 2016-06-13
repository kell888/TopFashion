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
    public partial class SelectFinanceDetailForm : Form
    {
        public SelectFinanceDetailForm()
        {
            InitializeComponent();
        }

        public List<FinanceDetail> SelectedDetails
        {
            get
            {
                List<FinanceDetail> details = new List<FinanceDetail>();
                List<FinanceDetail> ds = listBox1.Tag as List<FinanceDetail>;
                if (ds != null && ds.Count > 0)
                {
                    foreach (int index in listBox1.SelectedIndices)
                    {
                        if (index < ds.Count)
                        {
                            details.Add(ds[index]);
                        }
                    }
                }
                return details;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        private void SelectFinanceDetailForm_Load(object sender, EventArgs e)
        {
            LoadAllDetails();
        }

        private void LoadAllDetails()
        {
            listBox1.Items.Clear();
            List<FinanceDetail> details = FinanceDetailLogic.GetInstance().GetAllFinanceDetails();
            listBox1.Tag = details;
            foreach (FinanceDetail detail in details)
            {
                string info = detail.DetailInfo;
                listBox1.Items.Add(info);
            }
        }

        private void monthCalendar1_DateSelected(object sender, DateRangeEventArgs e)
        {
            textBox1.Text = e.Start.ToString("yyyy-MM-dd");
            monthCalendar1.Hide();
        }

        private void monthCalendar2_DateSelected(object sender, DateRangeEventArgs e)
        {
            textBox2.Text = e.Start.ToString("yyyy-MM-dd");
            monthCalendar2.Hide();
        }

        private void textBox1_Click(object sender, EventArgs e)
        {
            monthCalendar1.Show();
        }

        private void textBox2_Click(object sender, EventArgs e)
        {
            monthCalendar2.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Search(DateTime.Parse(textBox1.Text), DateTime.Parse(textBox2.Text), (selectStaffControl1.SelectedStaffs != null && selectStaffControl1.SelectedStaffs.Count > 0) ? selectStaffControl1.SelectedStaffs : null);
        }

        private void Search(DateTime start, DateTime end, List<Staff> staffs)
        {
            string zrr = "";
            if (staffs != null && staffs.Count > 0)
            {
                StringBuilder sb = new StringBuilder();
                foreach (Staff staff in staffs)
                {
                    if (sb.Length == 0)
                        sb.Append(staff.ID);
                    else
                        sb.Append("," + staff.ID);
                }
                zrr = " and 责任人 in (" + sb.ToString() + ")";
            }
            string where = "提交时间 between '" + start + "' and '" + end + "'" + zrr;
            listBox1.Items.Clear();
            List<FinanceDetail> details = FinanceDetailLogic.GetInstance().GetFinanceDetailList(where);
            listBox1.Tag = details;
            foreach (FinanceDetail detail in details)
            {
                string info = detail.DetailInfo;
                listBox1.Items.Add(info);
            }
        }
    }
}
