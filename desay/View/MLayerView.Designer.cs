namespace desay
{
    partial class MLayerView<T>
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
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.MStandbyPosOpen = new System.Windows.Forms.CheckBox();
            this.MLayerDistance = new System.Windows.Forms.TextBox();
            this.btnMLayerCountSave = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.numUpMLayerCount = new System.Windows.Forms.NumericUpDown();
            this.MLayerDistanceSave = new System.Windows.Forms.Button();
            this.btnMStandbySave = new System.Windows.Forms.Button();
            this.btnMLayerStartPositionGet = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label27 = new System.Windows.Forms.Label();
            this.btnMGetplate = new System.Windows.Forms.Button();
            this.btnMStandbyPos = new System.Windows.Forms.Button();
            this.ndnMIndex = new System.Windows.Forms.NumericUpDown();
            this.btnMLayerStartPosition = new System.Windows.Forms.Button();
            this.label42 = new System.Windows.Forms.Label();
            this.label41 = new System.Windows.Forms.Label();
            this.MStandbyPos = new System.Windows.Forms.Label();
            this.lblMLayerStartPosition = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.groupBox6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numUpMLayerCount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ndnMIndex)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.MStandbyPosOpen);
            this.groupBox6.Controls.Add(this.MLayerDistance);
            this.groupBox6.Controls.Add(this.btnMLayerCountSave);
            this.groupBox6.Controls.Add(this.label1);
            this.groupBox6.Controls.Add(this.numUpMLayerCount);
            this.groupBox6.Controls.Add(this.MLayerDistanceSave);
            this.groupBox6.Controls.Add(this.btnMStandbySave);
            this.groupBox6.Controls.Add(this.btnMLayerStartPositionGet);
            this.groupBox6.Controls.Add(this.label2);
            this.groupBox6.Controls.Add(this.label27);
            this.groupBox6.Controls.Add(this.btnMGetplate);
            this.groupBox6.Controls.Add(this.btnMStandbyPos);
            this.groupBox6.Controls.Add(this.ndnMIndex);
            this.groupBox6.Controls.Add(this.btnMLayerStartPosition);
            this.groupBox6.Controls.Add(this.label42);
            this.groupBox6.Controls.Add(this.label41);
            this.groupBox6.Controls.Add(this.MStandbyPos);
            this.groupBox6.Controls.Add(this.lblMLayerStartPosition);
            this.groupBox6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox6.Location = new System.Drawing.Point(3, 3);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(294, 178);
            this.groupBox6.TabIndex = 6;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "点位设置";
            // 
            // MStandbyPosOpen
            // 
            this.MStandbyPosOpen.AutoSize = true;
            this.MStandbyPosOpen.Location = new System.Drawing.Point(280, 152);
            this.MStandbyPosOpen.Name = "MStandbyPosOpen";
            this.MStandbyPosOpen.Size = new System.Drawing.Size(15, 14);
            this.MStandbyPosOpen.TabIndex = 35;
            this.MStandbyPosOpen.UseVisualStyleBackColor = true;
            this.MStandbyPosOpen.CheckedChanged += new System.EventHandler(this.MStandbyPosOpen_CheckedChanged);
            // 
            // MLayerDistance
            // 
            this.MLayerDistance.Location = new System.Drawing.Point(91, 55);
            this.MLayerDistance.Name = "MLayerDistance";
            this.MLayerDistance.Size = new System.Drawing.Size(51, 21);
            this.MLayerDistance.TabIndex = 34;
            // 
            // btnMLayerCountSave
            // 
            this.btnMLayerCountSave.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnMLayerCountSave.Location = new System.Drawing.Point(215, 88);
            this.btnMLayerCountSave.Name = "btnMLayerCountSave";
            this.btnMLayerCountSave.Size = new System.Drawing.Size(63, 23);
            this.btnMLayerCountSave.TabIndex = 33;
            this.btnMLayerCountSave.Text = "保存";
            this.btnMLayerCountSave.UseVisualStyleBackColor = true;
            this.btnMLayerCountSave.Click += new System.EventHandler(this.button1_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(8, 92);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(67, 14);
            this.label1.TabIndex = 32;
            this.label1.Text = "仓储格数";
            // 
            // numUpMLayerCount
            // 
            this.numUpMLayerCount.Location = new System.Drawing.Point(91, 88);
            this.numUpMLayerCount.Maximum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.numUpMLayerCount.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numUpMLayerCount.Name = "numUpMLayerCount";
            this.numUpMLayerCount.Size = new System.Drawing.Size(53, 21);
            this.numUpMLayerCount.TabIndex = 31;
            this.numUpMLayerCount.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.numUpMLayerCount.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // MLayerDistanceSave
            // 
            this.MLayerDistanceSave.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.MLayerDistanceSave.Location = new System.Drawing.Point(215, 54);
            this.MLayerDistanceSave.Name = "MLayerDistanceSave";
            this.MLayerDistanceSave.Size = new System.Drawing.Size(63, 23);
            this.MLayerDistanceSave.TabIndex = 30;
            this.MLayerDistanceSave.Text = "保存";
            this.MLayerDistanceSave.UseVisualStyleBackColor = true;
            this.MLayerDistanceSave.Click += new System.EventHandler(this.MLayerDistanceSave_Click);
            // 
            // btnMStandbySave
            // 
            this.btnMStandbySave.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnMStandbySave.Location = new System.Drawing.Point(215, 147);
            this.btnMStandbySave.Name = "btnMStandbySave";
            this.btnMStandbySave.Size = new System.Drawing.Size(63, 23);
            this.btnMStandbySave.TabIndex = 29;
            this.btnMStandbySave.Text = "保存";
            this.btnMStandbySave.UseVisualStyleBackColor = true;
            this.btnMStandbySave.Click += new System.EventHandler(this.btnMStandbySave_Click);
            // 
            // btnMLayerStartPositionGet
            // 
            this.btnMLayerStartPositionGet.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnMLayerStartPositionGet.Location = new System.Drawing.Point(215, 20);
            this.btnMLayerStartPositionGet.Name = "btnMLayerStartPositionGet";
            this.btnMLayerStartPositionGet.Size = new System.Drawing.Size(63, 23);
            this.btnMLayerStartPositionGet.TabIndex = 29;
            this.btnMLayerStartPositionGet.Text = "保存";
            this.btnMLayerStartPositionGet.UseVisualStyleBackColor = true;
            this.btnMLayerStartPositionGet.Click += new System.EventHandler(this.btnMLayerStartPositionGet_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(8, 151);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(82, 14);
            this.label2.TabIndex = 28;
            this.label2.Text = "仓储待机位";
            // 
            // label27
            // 
            this.label27.AutoSize = true;
            this.label27.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label27.Location = new System.Drawing.Point(8, 121);
            this.label27.Name = "label27";
            this.label27.Size = new System.Drawing.Size(67, 14);
            this.label27.TabIndex = 28;
            this.label27.Text = "当前格数";
            // 
            // btnMGetplate
            // 
            this.btnMGetplate.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnMGetplate.Location = new System.Drawing.Point(148, 117);
            this.btnMGetplate.Name = "btnMGetplate";
            this.btnMGetplate.Size = new System.Drawing.Size(63, 23);
            this.btnMGetplate.TabIndex = 27;
            this.btnMGetplate.Text = "定位";
            this.btnMGetplate.UseVisualStyleBackColor = true;
            this.btnMGetplate.Click += new System.EventHandler(this.btnMLayerCurrentIndex_Click);
            // 
            // btnMStandbyPos
            // 
            this.btnMStandbyPos.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnMStandbyPos.Location = new System.Drawing.Point(148, 147);
            this.btnMStandbyPos.Name = "btnMStandbyPos";
            this.btnMStandbyPos.Size = new System.Drawing.Size(63, 23);
            this.btnMStandbyPos.TabIndex = 9;
            this.btnMStandbyPos.Text = "定位";
            this.btnMStandbyPos.UseVisualStyleBackColor = true;
            this.btnMStandbyPos.Click += new System.EventHandler(this.btnMStandbyPos_Click);
            // 
            // ndnMIndex
            // 
            this.ndnMIndex.Location = new System.Drawing.Point(91, 118);
            this.ndnMIndex.Maximum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.ndnMIndex.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.ndnMIndex.Name = "ndnMIndex";
            this.ndnMIndex.Size = new System.Drawing.Size(53, 21);
            this.ndnMIndex.TabIndex = 8;
            this.ndnMIndex.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.ndnMIndex.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // btnMLayerStartPosition
            // 
            this.btnMLayerStartPosition.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnMLayerStartPosition.Location = new System.Drawing.Point(148, 20);
            this.btnMLayerStartPosition.Name = "btnMLayerStartPosition";
            this.btnMLayerStartPosition.Size = new System.Drawing.Size(63, 23);
            this.btnMLayerStartPosition.TabIndex = 9;
            this.btnMLayerStartPosition.Text = "定位";
            this.btnMLayerStartPosition.UseVisualStyleBackColor = true;
            this.btnMLayerStartPosition.Click += new System.EventHandler(this.btnMLayerStartPosition_Click);
            // 
            // label42
            // 
            this.label42.AutoSize = true;
            this.label42.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label42.Location = new System.Drawing.Point(10, 58);
            this.label42.Name = "label42";
            this.label42.Size = new System.Drawing.Size(69, 14);
            this.label42.TabIndex = 6;
            this.label42.Text = "间距(mm)";
            // 
            // label41
            // 
            this.label41.AutoSize = true;
            this.label41.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label41.Location = new System.Drawing.Point(10, 25);
            this.label41.Name = "label41";
            this.label41.Size = new System.Drawing.Size(67, 14);
            this.label41.TabIndex = 4;
            this.label41.Text = "进料起点";
            // 
            // MStandbyPos
            // 
            this.MStandbyPos.AutoSize = true;
            this.MStandbyPos.BackColor = System.Drawing.Color.Gray;
            this.MStandbyPos.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.MStandbyPos.ForeColor = System.Drawing.Color.White;
            this.MStandbyPos.Location = new System.Drawing.Point(91, 153);
            this.MStandbyPos.Name = "MStandbyPos";
            this.MStandbyPos.Size = new System.Drawing.Size(53, 12);
            this.MStandbyPos.TabIndex = 3;
            this.MStandbyPos.Text = "0000.000";
            this.MStandbyPos.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblMLayerStartPosition
            // 
            this.lblMLayerStartPosition.AutoSize = true;
            this.lblMLayerStartPosition.BackColor = System.Drawing.Color.Gray;
            this.lblMLayerStartPosition.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblMLayerStartPosition.ForeColor = System.Drawing.Color.White;
            this.lblMLayerStartPosition.Location = new System.Drawing.Point(91, 25);
            this.lblMLayerStartPosition.Name = "lblMLayerStartPosition";
            this.lblMLayerStartPosition.Size = new System.Drawing.Size(53, 12);
            this.lblMLayerStartPosition.TabIndex = 3;
            this.lblMLayerStartPosition.Text = "0000.000";
            this.lblMLayerStartPosition.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.groupBox6, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel1, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(601, 369);
            this.tableLayoutPanel1.TabIndex = 7;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(303, 3);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel1.SetRowSpan(this.tableLayoutPanel2, 2);
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(295, 363);
            this.tableLayoutPanel2.TabIndex = 7;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(3, 187);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(294, 179);
            this.flowLayoutPanel1.TabIndex = 8;
            // 
            // MLayerView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "MLayerView";
            this.Size = new System.Drawing.Size(601, 369);
            this.Load += new System.EventHandler(this.MLayerView_Load);
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numUpMLayerCount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ndnMIndex)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.Label label27;
        private System.Windows.Forms.Button btnMGetplate;
        private System.Windows.Forms.NumericUpDown ndnMIndex;
        private System.Windows.Forms.Button btnMLayerStartPosition;
        private System.Windows.Forms.Label label42;
        private System.Windows.Forms.Label label41;
        private System.Windows.Forms.Label lblMLayerStartPosition;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button MLayerDistanceSave;
        private System.Windows.Forms.Button btnMLayerStartPositionGet;
        private System.Windows.Forms.Button btnMLayerCountSave;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown numUpMLayerCount;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.TextBox MLayerDistance;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label MStandbyPos;
        private System.Windows.Forms.Button btnMStandbySave;
        private System.Windows.Forms.Button btnMStandbyPos;
        private System.Windows.Forms.CheckBox MStandbyPosOpen;
    }
}
