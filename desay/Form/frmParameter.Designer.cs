namespace desay
{
    partial class frmParameter
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.groupBox8 = new System.Windows.Forms.GroupBox();
            this.btnNew = new System.Windows.Forms.Button();
            this.btnSelectProductType = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.txtProductType = new System.Windows.Forms.TextBox();
            this.lbxProductType = new System.Windows.Forms.ListBox();
            this.lblCurrentProductType = new System.Windows.Forms.Label();
            this.label60 = new System.Windows.Forms.Label();
            this.label57 = new System.Windows.Forms.Label();
            this.groupBox8.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox8
            // 
            this.groupBox8.BackColor = System.Drawing.Color.LightGray;
            this.groupBox8.Controls.Add(this.btnNew);
            this.groupBox8.Controls.Add(this.btnSelectProductType);
            this.groupBox8.Controls.Add(this.btnDelete);
            this.groupBox8.Controls.Add(this.txtProductType);
            this.groupBox8.Controls.Add(this.lbxProductType);
            this.groupBox8.Controls.Add(this.lblCurrentProductType);
            this.groupBox8.Controls.Add(this.label60);
            this.groupBox8.Controls.Add(this.label57);
            this.groupBox8.Dock = System.Windows.Forms.DockStyle.Left;
            this.groupBox8.Location = new System.Drawing.Point(0, 0);
            this.groupBox8.Name = "groupBox8";
            this.groupBox8.Size = new System.Drawing.Size(289, 228);
            this.groupBox8.TabIndex = 38;
            this.groupBox8.TabStop = false;
            // 
            // btnNew
            // 
            this.btnNew.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnNew.Location = new System.Drawing.Point(148, 97);
            this.btnNew.Name = "btnNew";
            this.btnNew.Size = new System.Drawing.Size(138, 36);
            this.btnNew.TabIndex = 28;
            this.btnNew.Text = "新增";
            this.btnNew.UseVisualStyleBackColor = true;
            this.btnNew.Click += new System.EventHandler(this.btnNew_Click);
            // 
            // btnSelectProductType
            // 
            this.btnSelectProductType.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnSelectProductType.Location = new System.Drawing.Point(148, 181);
            this.btnSelectProductType.Name = "btnSelectProductType";
            this.btnSelectProductType.Size = new System.Drawing.Size(138, 36);
            this.btnSelectProductType.TabIndex = 29;
            this.btnSelectProductType.Text = "切换型号";
            this.btnSelectProductType.UseVisualStyleBackColor = true;
            this.btnSelectProductType.Click += new System.EventHandler(this.btnSelectProductType_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnDelete.Location = new System.Drawing.Point(148, 139);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(138, 36);
            this.btnDelete.TabIndex = 30;
            this.btnDelete.Text = "删除";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // txtProductType
            // 
            this.txtProductType.Location = new System.Drawing.Point(148, 65);
            this.txtProductType.Name = "txtProductType";
            this.txtProductType.Size = new System.Drawing.Size(138, 21);
            this.txtProductType.TabIndex = 27;
            // 
            // lbxProductType
            // 
            this.lbxProductType.FormattingEnabled = true;
            this.lbxProductType.ItemHeight = 12;
            this.lbxProductType.Location = new System.Drawing.Point(11, 65);
            this.lbxProductType.Name = "lbxProductType";
            this.lbxProductType.Size = new System.Drawing.Size(131, 148);
            this.lbxProductType.TabIndex = 26;
            // 
            // lblCurrentProductType
            // 
            this.lblCurrentProductType.AutoSize = true;
            this.lblCurrentProductType.Location = new System.Drawing.Point(101, 22);
            this.lblCurrentProductType.Name = "lblCurrentProductType";
            this.lblCurrentProductType.Size = new System.Drawing.Size(41, 12);
            this.lblCurrentProductType.TabIndex = 23;
            this.lblCurrentProductType.Text = "ABC123";
            // 
            // label60
            // 
            this.label60.AutoSize = true;
            this.label60.Location = new System.Drawing.Point(10, 45);
            this.label60.Name = "label60";
            this.label60.Size = new System.Drawing.Size(77, 12);
            this.label60.TabIndex = 24;
            this.label60.Text = "产品型号列表";
            // 
            // label57
            // 
            this.label57.AutoSize = true;
            this.label57.Location = new System.Drawing.Point(10, 22);
            this.label57.Name = "label57";
            this.label57.Size = new System.Drawing.Size(83, 12);
            this.label57.TabIndex = 25;
            this.label57.Text = "当前产品型号:";
            // 
            // frmParameter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(292, 228);
            this.Controls.Add(this.groupBox8);
            this.Name = "frmParameter";
            this.Text = "型号参数";
            this.Load += new System.EventHandler(this.frmParameter_Load);
            this.groupBox8.ResumeLayout(false);
            this.groupBox8.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox8;
        private System.Windows.Forms.Button btnNew;
        private System.Windows.Forms.Button btnSelectProductType;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.TextBox txtProductType;
        private System.Windows.Forms.ListBox lbxProductType;
        private System.Windows.Forms.Label lblCurrentProductType;
        private System.Windows.Forms.Label label60;
        private System.Windows.Forms.Label label57;
    }
}