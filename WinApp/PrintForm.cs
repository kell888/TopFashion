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
        public PrintForm(KellPrinter.DataReporter report)
        {
            InitializeComponent();
            this.report = report;
        }

        KellPrinter.DataReporter report;


        private void PrintForm_Load(object sender, EventArgs e)
        {
            base.CheckUserPermission(this);
            PrintPreviewDialog p = report.PreviewPrintReport();
            this.Controls.Add(p);
            p.Show();
            //this.Refresh();
        }
    }
}
