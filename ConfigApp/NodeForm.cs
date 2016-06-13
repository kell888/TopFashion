using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using KellWorkFlow;

namespace TopFashion
{
    public partial class NodeForm : Form
    {
        public NodeForm(ConfigClient owner)
        {
            InitializeComponent();
            this.owner = owner;
        }

        ConfigClient owner;
        int templateId;

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Trim() == "")
            {
                MessageBox.Show("请输入节点名称！");
                textBox1.Focus();
                return;
            }
            //if (depUserControlEx1.SelectedUsers == null || depUserControlEx1.SelectedUsers.Count == 0)
            //{
            //    MessageBox.Show("请选定执行者！");
            //    depUserControlEx1.Focus();
            //    return;
            //}
            //if (depUserControlEx2.SelectedUsers == null || depUserControlEx2.SelectedUsers.Count == 0)
            //{
            //    MessageBox.Show("请选定审批者！");
            //    depUserControlEx2.Focus();
            //    return;
            //}
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        public TaskStageTemplate Template
        {
            get
            {
                return GetTemplate();
            }
            set
            {
                SetTemplate(value);
            }
        }

        private void SetTemplate(TaskStageTemplate value)
        {
            if (value != null)
            {
                templateId = value.ID;
                textBox1.Text = value.Name;
                depUserControlEx1.SelectedUsers = GetUsers(value.Executors);
                depUserControlEx2.SelectedUsers = GetUsers(value.Approvers);
            }
        }

        private List<User> GetUsers(List<string> list)
        {
            List<User> users = new List<User>();
            UserLogic ul = UserLogic.GetInstance();
            foreach (string id in list)
            {
                users.Add(ul.GetUser(Convert.ToInt32(id)));
            }
            return users;
        }

        private TaskStageTemplate GetTemplate()
        {
            return new TaskStageTemplate(templateId, textBox1.Text.Trim(), GetExecutors(), GetApprovers());
        }

        private List<string> GetExecutors()
        {
            List<string> users = new List<string>();
            foreach (User user in depUserControlEx1.SelectedUsers)
            {
                users.Add(user.ID.ToString());
            }
            return users;
        }

        private List<string> GetApprovers()
        {
            List<string> users = new List<string>();
            foreach (User user in depUserControlEx2.SelectedUsers)
            {
                users.Add(user.ID.ToString());
            }
            return users;
        }

        private void NodeForm_Load(object sender, EventArgs e)
        {
            LoadUsers();
        }

        private void LoadUsers()
        {
            depUserControlEx1.LoadDepartments(owner.CurrentArchitecture.Deps);
            depUserControlEx1.LoadUsers(owner.CurrentArchitecture.Users);
            depUserControlEx2.LoadDepartments(owner.CurrentArchitecture.Deps);
            depUserControlEx2.LoadUsers(owner.CurrentArchitecture.Users);
        }
    }
}
