using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using KellWorkFlow;

namespace TopFashion
{
    [DefaultEvent("Selected")]
    public partial class LinkControl : UserControl
    {
        public LinkControl()
        {
            InitializeComponent();
        }

        int index = 0;

        public int Index
        {
            get { return index; }
            set { index = value; }
        }

        public string NodeName
        {
            get
            {
                return label1.Text;
            }
            set
            {
                label1.Text = value;
            }
        }

        private void Fixed_Resize(object sender, EventArgs e)
        {
            this.Height = 38;
        }
    }
}
