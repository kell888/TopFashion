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
    public partial class PrintForm : PermissionForm
    {
        public PrintForm(User user, KellPrinter.DataReporter report)
        {
            this.User = user;
            InitializeComponent();
            this.report = report;
        }

        KellPrinter.DataReporter report;


        private void PrintForm_Load(object sender, EventArgs e)
        {
            base.DisableUserPermission(this);
        }

        private void PrintForm_Shown(object sender, EventArgs e)
        {
            PrintPreviewDialog p = report.PreviewPrintReport();
            this.Controls.Add(p);
            p.WindowState = FormWindowState.Maximized;
            p.FormBorderStyle = FormBorderStyle.None;
            p.Show();
        }
    }
}
