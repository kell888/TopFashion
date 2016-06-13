using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace TopFashion
{
    public partial class ImportForm : Form
    {
        public ImportForm()
        {
            InitializeComponent();
        }

        FieldMap<string, string> map;
        DataTable data;

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex > -1)
            {
                label3.Text = listBox1.SelectedItem.ToString();
            }
        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox2.SelectedIndex > -1)
            {
                label5.Text = listBox2.SelectedItem.ToString();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (data == null || data.Rows.Count == 0)
            {
                MessageBox.Show("该Excel文件中无任何数据！");
                return;
            }
            if (map.Count == 0)
            {
                MessageBox.Show("请先编辑字段映射！");
                return;
            }
            if (MessageBox.Show("确定已经编辑好字段映射了么？" + Environment.NewLine + "本操作会清空原来的" + comboBox2.Text + "资料，请谨慎操作！", "导入提醒", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.OK)
            {
                progressBar1.Visible = true;
                progressBar1.BringToFront();
                progressBar1.Value = 0;
                progressBar1.Maximum = data.Rows.Count;
                ImportData(data, map);
                progressBar1.Visible = false;
                progressBar1.SendToBack();
            }
        }

        private void LoadMap(FieldMap<string, string> map)
        {
            if (map != null)
            {
                comboBox3.Items.Clear();
                Dictionary<string, string>.Enumerator ie = map.GetEnumerator();
                while (ie.MoveNext())
                {
                    comboBox3.Items.Add(ie.Current.ToString());
                }
            }
            else
            {
                MessageBox.Show("映射为空，载入失败！");
            }
        }

        private void LoadExpElement(List<string> elements)
        {
            listBox2.Items.Clear();
            foreach (string element in elements)
            {
                listBox2.Items.Add(element);
            }
        }

        private void LoadSysElement(List<string> elements)
        {
            listBox1.Items.Clear();
            foreach (string element in elements)
            {
                listBox1.Items.Add(element);
            }
        }            

        private void LoadRelation(KeyValuePair<string, string> relation)
        {
            if (listBox1.Items.Count > 0)
            {
                int index = listBox1.Items.IndexOf(relation.Key);
                if (index > -1)
                {
                    listBox1.SelectedIndex = index;
                }
                else
                {
                    MessageBox.Show("没找到对应的系统项！");
                    return;
                }
            }
            else
            {
                MessageBox.Show("系统项为空！");
                return;
            }
            if (listBox2.Items.Count > 0)
            {
                int index = listBox2.Items.IndexOf(relation.Value);
                if (index > -1)
                {
                    listBox2.SelectedIndex = index;
                }
                else
                {
                    MessageBox.Show("没找到对应的导入项！");
                    return;
                }
            }
            else
            {
                MessageBox.Show("导入项为空！");
                return;
            }
        }

        private void ImportData(DataTable data, FieldMap<string, string> map, bool clearOldData = true)
        {
            ImportDataProcessHandler process = new ImportDataProcessHandler(ImportProcess);
            bool f = Commons.ImportData(comboBox2.Text, clearOldData, data, map, process);
        }

        private void ImportProcess(int percent)
        {
            progressBar1.Value = percent;
            progressBar1.Refresh();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            openFileDialog1.Title = "打开Excel文件...";
            openFileDialog1.Filter = "Excel2003以下文件(*.xls)|*.xls|Excel2007以上文件(*.xlsx)|*.xlsx";
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string excel = textBox1.Text = openFileDialog1.FileName;
                List<string> fields;
                data = Commons.ReadDataFromExcel(excel, true, out fields);
                if (fields != null)
                {
                    LoadExpElement(fields);
                }
            }
            openFileDialog1.Dispose();
        }

        private void ImportForm_Load(object sender, EventArgs e)
        {
            comboBox2.SelectedIndex = 0;
            LoadAllMaps();
        }

        private void LoadAllMaps()
        {
            List<FieldMap<string, string>> maps = FieldMapLogic<string, string>.GetInstance().GetAllFieldMaps();
            comboBox1.Items.Clear();
            foreach (FieldMap<string, string> map in maps)
            {
                comboBox1.Items.Add(map);
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex > -1)
            {
                map = comboBox1.SelectedItem as FieldMap<string, string>;
                if (map != null)
                {
                    LoadMap(map);
                }
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox2.SelectedIndex > -1)
            {
                List<string> fields = Commons.GetFieldsBySysElement(comboBox2.Text);
                if (fields != null)
                {
                    LoadSysElement(fields);
                }
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            //添加映射
            map.Name = textBox2.Text.Trim();
            FieldMapLogic<string, string> fml = FieldMapLogic<string, string>.GetInstance();
            if (fml.ExistsName(map.Name))
            {
                if (MessageBox.Show("系统中已经存在该提醒，确定还要继续保存么？", "重名提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.OK)
                {
                    int i = fml.AddFieldMap(map);
                    if (i > 0)
                    {
                        LoadAllMaps();
                        MessageBox.Show("添加成功！");
                    }
                }
                else
                {
                    textBox2.Focus();
                    textBox2.SelectAll();
                }
            }
            else
            {
                int i = fml.AddFieldMap(map);
                if (i > 0)
                {
                    LoadAllMaps();
                    MessageBox.Show("添加成功！");
                }
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            //修改映射
            map.Name = textBox2.Text.Trim();
            FieldMapLogic<string, string> fml = FieldMapLogic<string, string>.GetInstance();
            if (fml.ExistsNameOther(map.Name, map.ID))
            {
                if (MessageBox.Show("系统中已经存在该提醒，确定还要继续保存么？", "重名提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.OK)
                {
                    bool f = fml.UpdateFieldMap(map);
                    if (f)
                    {
                        LoadAllMaps();
                        MessageBox.Show("修改成功！");
                    }
                }
                else
                {
                    textBox2.Focus();
                    textBox2.SelectAll();
                }
            }
            else
            {
                bool f = fml.UpdateFieldMap(map);
                if (f)
                {
                    LoadAllMaps();
                    MessageBox.Show("修改成功！");
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {//添加关系
            if (listBox1.SelectedIndex > -1 && listBox2.SelectedIndex > -1)
            {
                if (map.Add(listBox1.SelectedItem.ToString(), listBox2.SelectedItem.ToString()))
                    LoadMap(map);
                else
                    MessageBox.Show("添加关系失败，可能已经存在该关系！如若修改，请点击修改按钮。");
            }
            else
            {
                MessageBox.Show("请先选定系统项和导入项！");
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {//修改关系
            if (listBox1.SelectedIndex > -1 && listBox2.SelectedIndex > -1)
            {
                if (comboBox3.SelectedIndex > -1)
                {
                    string key = listBox1.SelectedItem.ToString();
                    if (map.Keys.Contains<string>(key))
                    {
                        map[key] = listBox2.SelectedItem.ToString();
                        LoadMap(map);
                    }
                    else
                    {
                        MessageBox.Show("修改关系失败，目前不存在该关系！如若添加，请点击添加按钮。");
                    }
                }
                else
                {
                    MessageBox.Show("请先选定要修改的关系！");
                }
            }
            else
            {
                MessageBox.Show("请先选定系统项和导入项！");
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {//删除关系
            if (comboBox3.SelectedIndex > -1)
            {
                string key = ((KeyValuePair<string, string>)comboBox3.SelectedItem).Key;
                if (!string.IsNullOrEmpty(key))
                {
                    if (map.Remove(key))
                    {
                        LoadMap(map);
                        listBox1.SelectedIndex = -1;
                        listBox2.SelectedIndex = -1;
                    }
                    else
                    {
                        MessageBox.Show("目前不存在该关系，删除失败！");
                    }
                }
                else
                {
                    MessageBox.Show("找不到[" + comboBox3.SelectedIndex + "]索引出的关系！");
                }
            }
            else
            {
                MessageBox.Show("请先选定要删除的关系！");
            }
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox3.SelectedIndex > -1)
            {
                KeyValuePair<string, string> relation = (KeyValuePair<string, string>)comboBox3.SelectedItem;
                LoadRelation(relation);
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            openFileDialog1.Title = "打开映射文件...";
            openFileDialog1.Filter = "映射文件(*.map)|*.map";
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                FieldMap<string, string> map = Commons.LoadMapFromLocal<string, string>(openFileDialog1.FileName);
                LoadMap(map);
            }
            openFileDialog1.Dispose();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            saveFileDialog1.Title = "保存映射文件到...";
            saveFileDialog1.Filter = "映射文件(*.map)|*.map";
            if (saveFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if (Commons.SaveMapToLocal<string, string>(map, saveFileDialog1.FileName))
                    MessageBox.Show("保存成功！");
                else
                    MessageBox.Show("保存失败！");
            }
            saveFileDialog1.Dispose();
        }
    }
}
