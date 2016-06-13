namespace TopFashion
{
    partial class FormEditControl
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.itemValueControl1 = new TopFashion.ItemValueControl();
            this.itemNameControl1 = new TopFashion.ItemNameControl();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Dock = System.Windows.Forms.DockStyle.Right;
            this.button1.Location = new System.Drawing.Point(419, 0);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(21, 21);
            this.button1.TabIndex = 1;
            this.button1.Text = "-";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Dock = System.Windows.Forms.DockStyle.Right;
            this.button2.Location = new System.Drawing.Point(398, 0);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(21, 21);
            this.button2.TabIndex = 2;
            this.button2.Text = "+";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // itemValueControl1
            // 
            this.itemValueControl1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.itemValueControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.itemValueControl1.Location = new System.Drawing.Point(146, 0);
            this.itemValueControl1.MinimumSize = new System.Drawing.Size(240, 21);
            this.itemValueControl1.Name = "itemValueControl1";
            this.itemValueControl1.Size = new System.Drawing.Size(252, 21);
            this.itemValueControl1.TabIndex = 3;
            this.itemValueControl1.ValueChanged += new TopFashion.ValueChangedHandler(this.itemValueControl1_ValueChanged);
            // 
            // itemNameControl1
            // 
            this.itemNameControl1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.itemNameControl1.Dock = System.Windows.Forms.DockStyle.Left;
            this.itemNameControl1.Location = new System.Drawing.Point(0, 0);
            this.itemNameControl1.MinimumSize = new System.Drawing.Size(146, 21);
            this.itemNameControl1.Name = "itemNameControl1";
            this.itemNameControl1.Size = new System.Drawing.Size(146, 21);
            this.itemNameControl1.TabIndex = 0;
            this.itemNameControl1.NameChanged += new TopFashion.NameChangedHandler(this.itemNameControl1_NameChanged);
            // 
            // FormEditControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.itemValueControl1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.itemNameControl1);
            this.MinimumSize = new System.Drawing.Size(440, 21);
            this.Name = "FormEditControl";
            this.Size = new System.Drawing.Size(440, 21);
            this.Resize += new System.EventHandler(this.FormEditControl_Resize);
            this.ResumeLayout(false);

        }

        #endregion

        private ItemNameControl itemNameControl1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private ItemValueControl itemValueControl1;

    }
}
