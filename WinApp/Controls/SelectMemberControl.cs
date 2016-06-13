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
    public partial class SelectMemberControl : UserControl
    {
        public SelectMemberControl()
        {
            InitializeComponent();
            this.label1.Text = "选择会员...";
            this.Click += new EventHandler(SelectMemberControl_Click);
        }

        public static SelectMemberControl CreateInstance(bool selectOnlyOne)
        {
            SelectMemberControl ssc = new SelectMemberControl();
            ssc.selectOnlyOne = selectOnlyOne;
            return ssc;
        }

        void SelectMemberControl_Click(object sender, EventArgs e)
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
        public List<Member> SelectedMembers
        {
            get
            {
                List<Member> sts = null;
                if (this.label1.Tag != null)
                {
                    List<Member> members = this.label1.Tag as List<Member>;
                    if (members != null)
                        sts = members;
                    else
                        sts = new List<Member>();
                }
                else
                {
                    sts = new List<Member>();
                }
                return sts;
            }
            set
            {
                this.label1.Tag = value;
                if (value != null)
                {
                    StringBuilder sb = new StringBuilder();
                    foreach (Member member in value)
                    {
                        if (sb.Length == 0)
                            sb.Append(member.姓名);
                        else
                            sb.Append("," + member.姓名);
                    }
                    if (sb.Length > 0)
                        this.label1.Text = sb.ToString();
                    else
                        this.label1.Text = "选择会员...";
                }
                else
                {
                    this.label1.Text = "选择会员...";
                }
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {
            SelectMemberForm f = new SelectMemberForm();
            if (f.ShowDialog() == DialogResult.OK)
            {
                this.label1.Tag = f.SelectedMembers;
                if (f.SelectedMembers.Count > 0)
                {
                    StringBuilder sb = new StringBuilder();
                    foreach (Member member in f.SelectedMembers)
                    {
                        if (sb.Length == 0)
                            sb.Append(member.姓名);
                        else
                            sb.Append("," + member.姓名);
                    }
                    this.label1.Text = sb.ToString();
                }
                else
                {
                    this.label1.Text = "选择会员...";
                }
            }
        }

        private void label1_MouseHover(object sender, EventArgs e)
        {
            toolTip1.SetToolTip(label1, label1.Text);
        }
    }
}
