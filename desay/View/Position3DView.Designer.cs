namespace desay
{
    partial class Position3DView<T>
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
            this.flpView = new System.Windows.Forms.FlowLayoutPanel();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnGoto = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.gbxPosition = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.lblName1 = new System.Windows.Forms.Label();
            this.lblName2 = new System.Windows.Forms.Label();
            this.lblPointX = new System.Windows.Forms.Label();
            this.lblPointY = new System.Windows.Forms.Label();
            this.lblName3 = new System.Windows.Forms.Label();
            this.lblPointZ = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.panel1.SuspendLayout();
            this.gbxPosition.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // flpView
            // 
            this.flpView.Dock = System.Windows.Forms.DockStyle.Top;
            this.flpView.Location = new System.Drawing.Point(0, 0);
            this.flpView.Margin = new System.Windows.Forms.Padding(2);
            this.flpView.Name = "flpView";
            this.flpView.Size = new System.Drawing.Size(736, 296);
            this.flpView.TabIndex = 0;
            // 
            // btnSave
            // 
            this.btnSave.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnSave.Location = new System.Drawing.Point(4, 4);
            this.btnSave.Margin = new System.Windows.Forms.Padding(2);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(96, 42);
            this.btnSave.TabIndex = 1;
            this.btnSave.Text = "保存";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnGoto
            // 
            this.btnGoto.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnGoto.Location = new System.Drawing.Point(127, 4);
            this.btnGoto.Margin = new System.Windows.Forms.Padding(2);
            this.btnGoto.Name = "btnGoto";
            this.btnGoto.Size = new System.Drawing.Size(96, 42);
            this.btnGoto.TabIndex = 1;
            this.btnGoto.Text = "定位";
            this.btnGoto.UseVisualStyleBackColor = true;
            this.btnGoto.Click += new System.EventHandler(this.btnGoto_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnSave);
            this.panel1.Controls.Add(this.btnGoto);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 466);
            this.panel1.Margin = new System.Windows.Forms.Padding(2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(736, 49);
            this.panel1.TabIndex = 1;
            // 
            // gbxPosition
            // 
            this.gbxPosition.Controls.Add(this.tableLayoutPanel2);
            this.gbxPosition.Location = new System.Drawing.Point(2, 2);
            this.gbxPosition.Margin = new System.Windows.Forms.Padding(2);
            this.gbxPosition.Name = "gbxPosition";
            this.gbxPosition.Padding = new System.Windows.Forms.Padding(2);
            this.gbxPosition.Size = new System.Drawing.Size(160, 87);
            this.gbxPosition.TabIndex = 0;
            this.gbxPosition.TabStop = false;
            this.gbxPosition.Text = "位置";
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Controls.Add(this.lblName1, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.lblName2, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.lblPointX, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.lblPointY, 1, 1);
            this.tableLayoutPanel2.Controls.Add(this.lblName3, 0, 2);
            this.tableLayoutPanel2.Controls.Add(this.lblPointZ, 1, 2);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(2, 16);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 3;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(156, 69);
            this.tableLayoutPanel2.TabIndex = 1;
            // 
            // lblName1
            // 
            this.lblName1.AutoSize = true;
            this.lblName1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblName1.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblName1.Location = new System.Drawing.Point(4, 1);
            this.lblName1.Name = "lblName1";
            this.lblName1.Size = new System.Drawing.Size(70, 21);
            this.lblName1.TabIndex = 0;
            this.lblName1.Text = "X轴";
            this.lblName1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblName2
            // 
            this.lblName2.AutoSize = true;
            this.lblName2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblName2.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblName2.Location = new System.Drawing.Point(4, 23);
            this.lblName2.Name = "lblName2";
            this.lblName2.Size = new System.Drawing.Size(70, 21);
            this.lblName2.TabIndex = 1;
            this.lblName2.Text = "Y轴";
            this.lblName2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblPointX
            // 
            this.lblPointX.AutoSize = true;
            this.lblPointX.BackColor = System.Drawing.Color.Transparent;
            this.lblPointX.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblPointX.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblPointX.ForeColor = System.Drawing.Color.Black;
            this.lblPointX.Location = new System.Drawing.Point(81, 1);
            this.lblPointX.Name = "lblPointX";
            this.lblPointX.Size = new System.Drawing.Size(71, 21);
            this.lblPointX.TabIndex = 2;
            this.lblPointX.Text = "0000.000";
            this.lblPointX.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblPointY
            // 
            this.lblPointY.AutoSize = true;
            this.lblPointY.BackColor = System.Drawing.Color.Transparent;
            this.lblPointY.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblPointY.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblPointY.ForeColor = System.Drawing.Color.Black;
            this.lblPointY.Location = new System.Drawing.Point(81, 23);
            this.lblPointY.Name = "lblPointY";
            this.lblPointY.Size = new System.Drawing.Size(71, 21);
            this.lblPointY.TabIndex = 3;
            this.lblPointY.Text = "0000.000";
            this.lblPointY.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblName3
            // 
            this.lblName3.AutoSize = true;
            this.lblName3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblName3.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblName3.Location = new System.Drawing.Point(4, 45);
            this.lblName3.Name = "lblName3";
            this.lblName3.Size = new System.Drawing.Size(70, 23);
            this.lblName3.TabIndex = 1;
            this.lblName3.Text = "Z轴";
            this.lblName3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblPointZ
            // 
            this.lblPointZ.AutoSize = true;
            this.lblPointZ.BackColor = System.Drawing.Color.Transparent;
            this.lblPointZ.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblPointZ.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblPointZ.ForeColor = System.Drawing.Color.Black;
            this.lblPointZ.Location = new System.Drawing.Point(81, 45);
            this.lblPointZ.Name = "lblPointZ";
            this.lblPointZ.Size = new System.Drawing.Size(71, 23);
            this.lblPointZ.TabIndex = 3;
            this.lblPointZ.Text = "0000.000";
            this.lblPointZ.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 70F));
            this.tableLayoutPanel1.Controls.Add(this.gbxPosition, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel1, 1, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 296);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(736, 170);
            this.tableLayoutPanel1.TabIndex = 2;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(223, 3);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(510, 164);
            this.flowLayoutPanel1.TabIndex = 1;
            // 
            // Position3DView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.flpView);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "Position3DView";
            this.Size = new System.Drawing.Size(736, 515);
            this.Load += new System.EventHandler(this.uc3DPointView_Load);
            this.panel1.ResumeLayout(false);
            this.gbxPosition.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel flpView;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnGoto;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.GroupBox gbxPosition;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Label lblName1;
        private System.Windows.Forms.Label lblName2;
        private System.Windows.Forms.Label lblPointX;
        private System.Windows.Forms.Label lblPointY;
        private System.Windows.Forms.Label lblName3;
        private System.Windows.Forms.Label lblPointZ;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
    }
}
