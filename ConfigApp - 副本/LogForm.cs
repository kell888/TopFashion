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
    public partial class LogForm : Form
    {
        public LogForm(string logtype)
        {
            InitializeComponent();
            this.logtype = logtype;
        }

        int index;
        int size = 20;
        int allCount;
        int pageCount;
        string logtype;
        bool loadOver;

        private void LogForm_Load(object sender, EventArgs e)
        {
            First(logtype);
        }

        private void Prex(string logtype = null)
        {
            if (index > 0)
            {
                index--;
                LoadLogs(logtype);
                button3.Enabled = true;
                if (index == 0)
                {
                    button1.Enabled = button2.Enabled = false;
                }
                if (index < pageCount - 1)
                {
                    button3.Enabled = button4.Enabled = true;
                }
            }
        }

        private void Next(string logtype = null)
        {
            if (index < pageCount - 1)
            {
                index++;
                LoadLogs(logtype);
                button2.Enabled = true;
                if (index == allCount / size)
                {
                    button3.Enabled = button4.Enabled = false;
                }
                if (index > 0)
                {
                    button1.Enabled = button2.Enabled = true;
                }
            }
        }

        private void Last(string logtype = null)
        {
            index = pageCount - 1;
            LoadLogs(logtype);
            button3.Enabled = button4.Enabled = false;
            if (pageCount == 1)
            {
                button1.Enabled = button2.Enabled = false;
            }
            else
            {
                button1.Enabled = button2.Enabled = true;
            }
        }

        private void First(string logtype = null)
        {
            index = 0;
            LoadLogs(logtype);
            button1.Enabled = button2.Enabled = false;
            if (pageCount == 1)
            {
                button3.Enabled = button4.Enabled = false;
            }
            else
            {
                button3.Enabled = button4.Enabled = true;
            }
        }

        private int LoadLogs(string logtype = null)
        {
            List<string> logTypes = WriteLog.GetLogTypes();
            comboBox1.Items.Clear();
            comboBox1.Items.Add("--不限--");
            foreach (string logT in logTypes)
            {
                comboBox1.Items.Add(logT);
            }
            loadOver = false;
            if (logtype == null)
                comboBox1.SelectedIndex = 0;
            else
                comboBox1.SelectedIndex = comboBox1.Items.IndexOf(logtype);
            loadOver = true;
            string logType = null;
            if (comboBox1.SelectedItem.ToString() != "--不限--")
                logType = comboBox1.SelectedItem.ToString();
            dataGridView1.DataSource = WriteLog.GetLogs(index, size, logType, out allCount);
            pageCount = allCount / size;
            int R = allCount % size;
            if (R > 0)
                pageCount++;
            int begin = size * index;
            if (pageCount == 0)
            {
                textBox1.Text = "0/0";
            }
            else
            {
                int r = begin + size;
                if (pageCount == 1)
                {
                    r = begin + R;
                }
                else if (pageCount > 1 && index == pageCount - 1)
                {
                    if (R == 0)
                        r = begin + size;
                    else
                        r = begin + R;
                }
                textBox1.Text = Convert.ToString(begin + 1) + "-" + r + "/" + allCount;
            }
            return pageCount;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            First(logtype);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Prex(logtype);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Next(logtype);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Last(logtype);
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (loadOver)
            {
                logtype = comboBox1.SelectedItem.ToString();
            }
        }
    }
}
