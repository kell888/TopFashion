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
    public partial class SelectLogTypeForm : Form
    {
        public SelectLogTypeForm()
        {
            InitializeComponent();
        }

        private void SelectLogTypeForm_Load(object sender, EventArgs e)
        {
            List<string> logTypes = WriteLog.GetLogTypes();
            comboBox1.Items.Clear();
            comboBox1.Items.Add("--不限--");
            foreach (string logT in logTypes)
            {
                comboBox1.Items.Add(logT);
            }
            comboBox1.SelectedIndex = 0;
        }

        public string LogType { get { return comboBox1.SelectedItem.ToString(); } }
    }
}
