namespace System.Enginee
{
    partial class MoveSelectHorizontal
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
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.rbnPos10um = new System.Windows.Forms.RadioButton();
            this.rbnPos1um = new System.Windows.Forms.RadioButton();
            this.rbnPos100um = new System.Windows.Forms.RadioButton();
            this.rbnPos1000um = new System.Windows.Forms.RadioButton();
            this.panel1 = new System.Windows.Forms.Panel();
            this.rbnContinueMoveSelect = new System.Windows.Forms.RadioButton();
            this.rbnLocationMoveSelect = new System.Windows.Forms.RadioButton();
            this.groupBox4.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.groupBox5);
            this.groupBox4.Controls.Add(this.panel1);
            this.groupBox4.Location = new System.Drawing.Point(3, 3);
            this.groupBox4.Margin = new System.Windows.Forms.Padding(0);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(417, 51);
            this.groupBox4.TabIndex = 4;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "选择";
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.rbnPos10um);
            this.groupBox5.Controls.Add(this.rbnPos1um);
            this.groupBox5.Controls.Add(this.rbnPos100um);
            this.groupBox5.Controls.Add(this.rbnPos1000um);
            this.groupBox5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox5.Location = new System.Drawing.Point(105, 17);
            this.groupBox5.Margin = new System.Windows.Forms.Padding(0);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(309, 31);
            this.groupBox5.TabIndex = 1;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "定距选择";
            // 
            // rbnPos10um
            // 
            this.rbnPos10um.AutoSize = true;
            this.rbnPos10um.Location = new System.Drawing.Point(85, 13);
            this.rbnPos10um.Name = "rbnPos10um";
            this.rbnPos10um.Size = new System.Drawing.Size(59, 16);
            this.rbnPos10um.TabIndex = 0;
            this.rbnPos10um.TabStop = true;
            this.rbnPos10um.Text = "0.01mm";
            this.rbnPos10um.UseVisualStyleBackColor = true;
            this.rbnPos10um.CheckedChanged += new System.EventHandler(this.rbnPos10um_CheckedChanged);
            // 
            // rbnPos1um
            // 
            this.rbnPos1um.AutoSize = true;
            this.rbnPos1um.Location = new System.Drawing.Point(16, 13);
            this.rbnPos1um.Name = "rbnPos1um";
            this.rbnPos1um.Size = new System.Drawing.Size(65, 16);
            this.rbnPos1um.TabIndex = 0;
            this.rbnPos1um.TabStop = true;
            this.rbnPos1um.Text = "0.001mm";
            this.rbnPos1um.UseVisualStyleBackColor = true;
            this.rbnPos1um.CheckedChanged += new System.EventHandler(this.rbnPos1um_CheckedChanged);
            // 
            // rbnPos100um
            // 
            this.rbnPos100um.AutoSize = true;
            this.rbnPos100um.Location = new System.Drawing.Point(148, 13);
            this.rbnPos100um.Name = "rbnPos100um";
            this.rbnPos100um.Size = new System.Drawing.Size(59, 16);
            this.rbnPos100um.TabIndex = 0;
            this.rbnPos100um.TabStop = true;
            this.rbnPos100um.Text = "0.10mm";
            this.rbnPos100um.UseVisualStyleBackColor = true;
            this.rbnPos100um.CheckedChanged += new System.EventHandler(this.rbnPos100um_CheckedChanged);
            // 
            // rbnPos1000um
            // 
            this.rbnPos1000um.AutoSize = true;
            this.rbnPos1000um.Location = new System.Drawing.Point(211, 13);
            this.rbnPos1000um.Name = "rbnPos1000um";
            this.rbnPos1000um.Size = new System.Drawing.Size(59, 16);
            this.rbnPos1000um.TabIndex = 0;
            this.rbnPos1000um.TabStop = true;
            this.rbnPos1000um.Text = "1.00mm";
            this.rbnPos1000um.UseVisualStyleBackColor = true;
            this.rbnPos1000um.CheckedChanged += new System.EventHandler(this.rbnPos1000um_CheckedChanged);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.rbnContinueMoveSelect);
            this.panel1.Controls.Add(this.rbnLocationMoveSelect);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(3, 17);
            this.panel1.Margin = new System.Windows.Forms.Padding(2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(102, 31);
            this.panel1.TabIndex = 0;
            // 
            // rbnContinueMoveSelect
            // 
            this.rbnContinueMoveSelect.AutoSize = true;
            this.rbnContinueMoveSelect.Location = new System.Drawing.Point(3, 7);
            this.rbnContinueMoveSelect.Name = "rbnContinueMoveSelect";
            this.rbnContinueMoveSelect.Size = new System.Drawing.Size(47, 16);
            this.rbnContinueMoveSelect.TabIndex = 0;
            this.rbnContinueMoveSelect.TabStop = true;
            this.rbnContinueMoveSelect.Text = "连续";
            this.rbnContinueMoveSelect.UseVisualStyleBackColor = true;
            this.rbnContinueMoveSelect.CheckedChanged += new System.EventHandler(this.rbnContinueMoveSelect_CheckedChanged);
            // 
            // rbnLocationMoveSelect
            // 
            this.rbnLocationMoveSelect.AutoSize = true;
            this.rbnLocationMoveSelect.Location = new System.Drawing.Point(52, 8);
            this.rbnLocationMoveSelect.Name = "rbnLocationMoveSelect";
            this.rbnLocationMoveSelect.Size = new System.Drawing.Size(47, 16);
            this.rbnLocationMoveSelect.TabIndex = 0;
            this.rbnLocationMoveSelect.TabStop = true;
            this.rbnLocationMoveSelect.Text = "定距";
            this.rbnLocationMoveSelect.UseVisualStyleBackColor = true;
            this.rbnLocationMoveSelect.CheckedChanged += new System.EventHandler(this.rbnLocationMoveSelect_CheckedChanged);
            // 
            // MoveSelectHorizontal
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox4);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "MoveSelectHorizontal";
            this.Size = new System.Drawing.Size(425, 59);
            this.groupBox4.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.RadioButton rbnPos10um;
        private System.Windows.Forms.RadioButton rbnPos1um;
        private System.Windows.Forms.RadioButton rbnPos100um;
        private System.Windows.Forms.RadioButton rbnPos1000um;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.RadioButton rbnContinueMoveSelect;
        private System.Windows.Forms.RadioButton rbnLocationMoveSelect;
    }
}
