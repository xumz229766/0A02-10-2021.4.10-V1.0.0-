namespace System.Tray
{
    partial class TestTray
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
            this.components = new System.ComponentModel.Container();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.nudRow = new System.Windows.Forms.NumericUpDown();
            this.nudCol = new System.Windows.Forms.NumericUpDown();
            this.btnCreate = new System.Windows.Forms.Button();
            this.chxAddSheild = new System.Windows.Forms.CheckBox();
            this.chxRemoveSheild = new System.Windows.Forms.CheckBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label9 = new System.Windows.Forms.Label();
            this.lblIsCalibration = new System.Windows.Forms.Label();
            this.label21 = new System.Windows.Forms.Label();
            this.ndnColumnYoffset = new System.Windows.Forms.NumericUpDown();
            this.ndnColumnXoffset = new System.Windows.Forms.NumericUpDown();
            this.ndnRowYoffset = new System.Windows.Forms.NumericUpDown();
            this.ndnRowXoffset = new System.Windows.Forms.NumericUpDown();
            this.label18 = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.label20 = new System.Windows.Forms.Label();
            this.ndnFinalBaseIndex = new System.Windows.Forms.NumericUpDown();
            this.label12 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.ndnFinalRowIndex = new System.Windows.Forms.NumericUpDown();
            this.label11 = new System.Windows.Forms.Label();
            this.ndnFinalColumnIndex = new System.Windows.Forms.NumericUpDown();
            this.cbUnregular2 = new System.Windows.Forms.CheckBox();
            this.cbUnregular1 = new System.Windows.Forms.CheckBox();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnSelect = new System.Windows.Forms.Button();
            this.txtNewPlateType = new System.Windows.Forms.TextBox();
            this.txtName = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.cmbId = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.chShow = new System.Windows.Forms.CheckBox();
            this.panel3 = new System.Windows.Forms.Panel();
            this.cmbChangeLine = new System.Windows.Forms.ComboBox();
            this.cmbDirect = new System.Windows.Forms.ComboBox();
            this.btnConfirm = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.cmbStart = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.btnSave = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.radioButton3 = new System.Windows.Forms.RadioButton();
            ((System.ComponentModel.ISupportInitialize)(this.nudRow)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudCol)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ndnColumnYoffset)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ndnColumnXoffset)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ndnRowYoffset)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ndnRowXoffset)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ndnFinalBaseIndex)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ndnFinalRowIndex)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ndnFinalColumnIndex)).BeginInit();
            this.tableLayoutPanel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(1, 1);
            this.panel1.Margin = new System.Windows.Forms.Padding(0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(983, 588);
            this.panel1.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(14, 173);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "行：";
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(14, 203);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "列：";
            // 
            // nudRow
            // 
            this.nudRow.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.nudRow.Location = new System.Drawing.Point(42, 169);
            this.nudRow.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudRow.Name = "nudRow";
            this.nudRow.Size = new System.Drawing.Size(174, 21);
            this.nudRow.TabIndex = 3;
            this.nudRow.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // nudCol
            // 
            this.nudCol.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.nudCol.Location = new System.Drawing.Point(42, 199);
            this.nudCol.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudCol.Name = "nudCol";
            this.nudCol.Size = new System.Drawing.Size(174, 21);
            this.nudCol.TabIndex = 3;
            this.nudCol.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // btnCreate
            // 
            this.btnCreate.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCreate.Location = new System.Drawing.Point(17, 226);
            this.btnCreate.Name = "btnCreate";
            this.btnCreate.Size = new System.Drawing.Size(200, 23);
            this.btnCreate.TabIndex = 4;
            this.btnCreate.Text = "修改托盘";
            this.btnCreate.UseVisualStyleBackColor = true;
            this.btnCreate.Click += new System.EventHandler(this.btnCreate_Click);
            // 
            // chxAddSheild
            // 
            this.chxAddSheild.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.chxAddSheild.AutoSize = true;
            this.chxAddSheild.Location = new System.Drawing.Point(18, 299);
            this.chxAddSheild.Name = "chxAddSheild";
            this.chxAddSheild.Size = new System.Drawing.Size(72, 16);
            this.chxAddSheild.TabIndex = 5;
            this.chxAddSheild.Text = "添加屏蔽";
            this.chxAddSheild.UseVisualStyleBackColor = true;
            this.chxAddSheild.Visible = false;
            this.chxAddSheild.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // chxRemoveSheild
            // 
            this.chxRemoveSheild.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.chxRemoveSheild.AutoSize = true;
            this.chxRemoveSheild.Location = new System.Drawing.Point(18, 321);
            this.chxRemoveSheild.Name = "chxRemoveSheild";
            this.chxRemoveSheild.Size = new System.Drawing.Size(72, 16);
            this.chxRemoveSheild.TabIndex = 5;
            this.chxRemoveSheild.Text = "移除屏蔽";
            this.chxRemoveSheild.UseVisualStyleBackColor = true;
            this.chxRemoveSheild.Visible = false;
            this.chxRemoveSheild.CheckedChanged += new System.EventHandler(this.checkBox2_CheckedChanged);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 80F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.panel2, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.btnSave, 1, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 90F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1232, 657);
            this.tableLayoutPanel1.TabIndex = 6;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.radioButton3);
            this.panel2.Controls.Add(this.radioButton2);
            this.panel2.Controls.Add(this.radioButton1);
            this.panel2.Controls.Add(this.groupBox1);
            this.panel2.Controls.Add(this.nudCol);
            this.panel2.Controls.Add(this.nudRow);
            this.panel2.Controls.Add(this.cbUnregular2);
            this.panel2.Controls.Add(this.cbUnregular1);
            this.panel2.Controls.Add(this.btnDelete);
            this.panel2.Controls.Add(this.btnAdd);
            this.panel2.Controls.Add(this.btnSelect);
            this.panel2.Controls.Add(this.txtNewPlateType);
            this.panel2.Controls.Add(this.txtName);
            this.panel2.Controls.Add(this.label6);
            this.panel2.Controls.Add(this.cmbId);
            this.panel2.Controls.Add(this.label8);
            this.panel2.Controls.Add(this.label5);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Controls.Add(this.chxRemoveSheild);
            this.panel2.Controls.Add(this.chxAddSheild);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.btnCreate);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(985, 1);
            this.panel2.Margin = new System.Windows.Forms.Padding(0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(246, 588);
            this.panel2.TabIndex = 1;
            // 
            // radioButton2
            // 
            this.radioButton2.AutoSize = true;
            this.radioButton2.Location = new System.Drawing.Point(116, 297);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(47, 16);
            this.radioButton2.TabIndex = 14;
            this.radioButton2.TabStop = true;
            this.radioButton2.Text = "小盘";
            this.radioButton2.UseVisualStyleBackColor = true;
            this.radioButton2.CheckedChanged += new System.EventHandler(this.RadioButton2_CheckedChanged);
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Location = new System.Drawing.Point(116, 276);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(47, 16);
            this.radioButton1.TabIndex = 13;
            this.radioButton1.TabStop = true;
            this.radioButton1.Text = "大盘";
            this.radioButton1.UseVisualStyleBackColor = true;
            this.radioButton1.CheckedChanged += new System.EventHandler(this.RadioButton1_CheckedChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.lblIsCalibration);
            this.groupBox1.Controls.Add(this.label21);
            this.groupBox1.Controls.Add(this.ndnColumnYoffset);
            this.groupBox1.Controls.Add(this.ndnColumnXoffset);
            this.groupBox1.Controls.Add(this.ndnRowYoffset);
            this.groupBox1.Controls.Add(this.ndnRowXoffset);
            this.groupBox1.Controls.Add(this.label18);
            this.groupBox1.Controls.Add(this.label19);
            this.groupBox1.Controls.Add(this.label20);
            this.groupBox1.Controls.Add(this.ndnFinalBaseIndex);
            this.groupBox1.Controls.Add(this.label12);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.ndnFinalRowIndex);
            this.groupBox1.Controls.Add(this.label11);
            this.groupBox1.Controls.Add(this.ndnFinalColumnIndex);
            this.groupBox1.Location = new System.Drawing.Point(17, 343);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(198, 229);
            this.groupBox1.TabIndex = 12;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "标定托盘参数";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(9, 198);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(83, 12);
            this.label9.TabIndex = 6;
            this.label9.Text = "托盘是否标定:";
            this.label9.Visible = false;
            // 
            // lblIsCalibration
            // 
            this.lblIsCalibration.AutoSize = true;
            this.lblIsCalibration.Font = new System.Drawing.Font("宋体", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblIsCalibration.ForeColor = System.Drawing.Color.Red;
            this.lblIsCalibration.Location = new System.Drawing.Point(87, 196);
            this.lblIsCalibration.Name = "lblIsCalibration";
            this.lblIsCalibration.Size = new System.Drawing.Size(55, 15);
            this.lblIsCalibration.TabIndex = 6;
            this.lblIsCalibration.Text = "未标定";
            this.lblIsCalibration.Visible = false;
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Location = new System.Drawing.Point(9, 169);
            this.label21.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(101, 12);
            this.label21.TabIndex = 24;
            this.label21.Text = "列方向Y偏差(mm):";
            // 
            // ndnColumnYoffset
            // 
            this.ndnColumnYoffset.DecimalPlaces = 3;
            this.ndnColumnYoffset.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ndnColumnYoffset.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.ndnColumnYoffset.Location = new System.Drawing.Point(115, 167);
            this.ndnColumnYoffset.Maximum = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.ndnColumnYoffset.Minimum = new decimal(new int[] {
            500,
            0,
            0,
            -2147483648});
            this.ndnColumnYoffset.Name = "ndnColumnYoffset";
            this.ndnColumnYoffset.Size = new System.Drawing.Size(68, 21);
            this.ndnColumnYoffset.TabIndex = 28;
            this.ndnColumnYoffset.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // ndnColumnXoffset
            // 
            this.ndnColumnXoffset.DecimalPlaces = 3;
            this.ndnColumnXoffset.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ndnColumnXoffset.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.ndnColumnXoffset.Location = new System.Drawing.Point(115, 142);
            this.ndnColumnXoffset.Maximum = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.ndnColumnXoffset.Minimum = new decimal(new int[] {
            500,
            0,
            0,
            -2147483648});
            this.ndnColumnXoffset.Name = "ndnColumnXoffset";
            this.ndnColumnXoffset.Size = new System.Drawing.Size(68, 21);
            this.ndnColumnXoffset.TabIndex = 29;
            this.ndnColumnXoffset.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // ndnRowYoffset
            // 
            this.ndnRowYoffset.DecimalPlaces = 3;
            this.ndnRowYoffset.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ndnRowYoffset.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.ndnRowYoffset.Location = new System.Drawing.Point(115, 117);
            this.ndnRowYoffset.Maximum = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.ndnRowYoffset.Minimum = new decimal(new int[] {
            500,
            0,
            0,
            -2147483648});
            this.ndnRowYoffset.Name = "ndnRowYoffset";
            this.ndnRowYoffset.Size = new System.Drawing.Size(68, 21);
            this.ndnRowYoffset.TabIndex = 30;
            this.ndnRowYoffset.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // ndnRowXoffset
            // 
            this.ndnRowXoffset.DecimalPlaces = 3;
            this.ndnRowXoffset.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ndnRowXoffset.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.ndnRowXoffset.Location = new System.Drawing.Point(115, 92);
            this.ndnRowXoffset.Maximum = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.ndnRowXoffset.Minimum = new decimal(new int[] {
            500,
            0,
            0,
            -2147483648});
            this.ndnRowXoffset.Name = "ndnRowXoffset";
            this.ndnRowXoffset.Size = new System.Drawing.Size(68, 21);
            this.ndnRowXoffset.TabIndex = 31;
            this.ndnRowXoffset.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(9, 94);
            this.label18.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(101, 12);
            this.label18.TabIndex = 25;
            this.label18.Text = "行方向X偏差(mm):";
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(9, 119);
            this.label19.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(101, 12);
            this.label19.TabIndex = 26;
            this.label19.Text = "行方向Y偏差(mm):";
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(9, 144);
            this.label20.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(101, 12);
            this.label20.TabIndex = 27;
            this.label20.Text = "列方向X偏差(mm):";
            // 
            // ndnFinalBaseIndex
            // 
            this.ndnFinalBaseIndex.Location = new System.Drawing.Point(115, 17);
            this.ndnFinalBaseIndex.Maximum = new decimal(new int[] {
            2500,
            0,
            0,
            0});
            this.ndnFinalBaseIndex.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.ndnFinalBaseIndex.Name = "ndnFinalBaseIndex";
            this.ndnFinalBaseIndex.Size = new System.Drawing.Size(48, 21);
            this.ndnFinalBaseIndex.TabIndex = 18;
            this.ndnFinalBaseIndex.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.ndnFinalBaseIndex.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(9, 19);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(77, 12);
            this.label12.TabIndex = 16;
            this.label12.Text = "基准点序号：";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(9, 44);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(77, 12);
            this.label10.TabIndex = 16;
            this.label10.Text = "行终点序号：";
            // 
            // ndnFinalRowIndex
            // 
            this.ndnFinalRowIndex.Location = new System.Drawing.Point(115, 42);
            this.ndnFinalRowIndex.Maximum = new decimal(new int[] {
            2500,
            0,
            0,
            0});
            this.ndnFinalRowIndex.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.ndnFinalRowIndex.Name = "ndnFinalRowIndex";
            this.ndnFinalRowIndex.Size = new System.Drawing.Size(48, 21);
            this.ndnFinalRowIndex.TabIndex = 18;
            this.ndnFinalRowIndex.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.ndnFinalRowIndex.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(9, 69);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(77, 12);
            this.label11.TabIndex = 17;
            this.label11.Text = "列终点序号：";
            // 
            // ndnFinalColumnIndex
            // 
            this.ndnFinalColumnIndex.Location = new System.Drawing.Point(115, 67);
            this.ndnFinalColumnIndex.Maximum = new decimal(new int[] {
            2500,
            0,
            0,
            0});
            this.ndnFinalColumnIndex.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.ndnFinalColumnIndex.Name = "ndnFinalColumnIndex";
            this.ndnFinalColumnIndex.Size = new System.Drawing.Size(48, 21);
            this.ndnFinalColumnIndex.TabIndex = 19;
            this.ndnFinalColumnIndex.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.ndnFinalColumnIndex.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // cbUnregular2
            // 
            this.cbUnregular2.AutoSize = true;
            this.cbUnregular2.Location = new System.Drawing.Point(18, 277);
            this.cbUnregular2.Name = "cbUnregular2";
            this.cbUnregular2.Size = new System.Drawing.Size(54, 16);
            this.cbUnregular2.TabIndex = 11;
            this.cbUnregular2.Text = "异形2";
            this.cbUnregular2.UseVisualStyleBackColor = true;
            this.cbUnregular2.CheckedChanged += new System.EventHandler(this.cbUnregular2_CheckedChanged);
            // 
            // cbUnregular1
            // 
            this.cbUnregular1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cbUnregular1.AutoSize = true;
            this.cbUnregular1.Location = new System.Drawing.Point(18, 255);
            this.cbUnregular1.Name = "cbUnregular1";
            this.cbUnregular1.Size = new System.Drawing.Size(54, 16);
            this.cbUnregular1.TabIndex = 11;
            this.cbUnregular1.Text = "异形1";
            this.cbUnregular1.UseVisualStyleBackColor = true;
            this.cbUnregular1.CheckedChanged += new System.EventHandler(this.cbUnregular1_CheckedChanged);
            // 
            // btnDelete
            // 
            this.btnDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDelete.Location = new System.Drawing.Point(132, 110);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(83, 23);
            this.btnDelete.TabIndex = 10;
            this.btnDelete.Text = "删除";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAdd.Location = new System.Drawing.Point(116, 10);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(100, 23);
            this.btnAdd.TabIndex = 10;
            this.btnAdd.Text = "添加";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnSelect
            // 
            this.btnSelect.Location = new System.Drawing.Point(16, 110);
            this.btnSelect.Name = "btnSelect";
            this.btnSelect.Size = new System.Drawing.Size(83, 23);
            this.btnSelect.TabIndex = 10;
            this.btnSelect.Text = "选择";
            this.btnSelect.UseVisualStyleBackColor = true;
            this.btnSelect.Click += new System.EventHandler(this.btnSelect_Click);
            // 
            // txtNewPlateType
            // 
            this.txtNewPlateType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtNewPlateType.Location = new System.Drawing.Point(16, 42);
            this.txtNewPlateType.Name = "txtNewPlateType";
            this.txtNewPlateType.Size = new System.Drawing.Size(201, 21);
            this.txtNewPlateType.TabIndex = 9;
            // 
            // txtName
            // 
            this.txtName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtName.Location = new System.Drawing.Point(70, 140);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(147, 21);
            this.txtName.TabIndex = 9;
            // 
            // label6
            // 
            this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(14, 144);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(65, 12);
            this.label6.TabIndex = 8;
            this.label6.Text = "托盘名称：";
            // 
            // cmbId
            // 
            this.cmbId.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbId.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbId.FormattingEnabled = true;
            this.cmbId.Location = new System.Drawing.Point(16, 85);
            this.cmbId.Name = "cmbId";
            this.cmbId.Size = new System.Drawing.Size(199, 20);
            this.cmbId.TabIndex = 7;
            // 
            // label8
            // 
            this.label8.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(14, 17);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(89, 12);
            this.label8.TabIndex = 6;
            this.label8.Text = "新增托盘型号：";
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(14, 70);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(65, 12);
            this.label5.TabIndex = 6;
            this.label5.Text = "托盘型号：";
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 75F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.Controls.Add(this.chShow, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.panel3, 1, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(1, 590);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(983, 66);
            this.tableLayoutPanel2.TabIndex = 2;
            // 
            // chShow
            // 
            this.chShow.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.chShow.AutoSize = true;
            this.chShow.Location = new System.Drawing.Point(86, 25);
            this.chShow.Name = "chShow";
            this.chShow.Size = new System.Drawing.Size(72, 16);
            this.chShow.TabIndex = 0;
            this.chShow.Text = "预览显示";
            this.chShow.UseVisualStyleBackColor = true;
            this.chShow.CheckedChanged += new System.EventHandler(this.chShow_CheckedChanged);
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.cmbChangeLine);
            this.panel3.Controls.Add(this.cmbDirect);
            this.panel3.Controls.Add(this.btnConfirm);
            this.panel3.Controls.Add(this.label3);
            this.panel3.Controls.Add(this.cmbStart);
            this.panel3.Controls.Add(this.label7);
            this.panel3.Controls.Add(this.label4);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Enabled = false;
            this.panel3.Location = new System.Drawing.Point(245, 0);
            this.panel3.Margin = new System.Windows.Forms.Padding(0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(738, 66);
            this.panel3.TabIndex = 1;
            // 
            // cmbChangeLine
            // 
            this.cmbChangeLine.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbChangeLine.FormattingEnabled = true;
            this.cmbChangeLine.Items.AddRange(new object[] {
            "Z",
            "S"});
            this.cmbChangeLine.Location = new System.Drawing.Point(355, 14);
            this.cmbChangeLine.Name = "cmbChangeLine";
            this.cmbChangeLine.Size = new System.Drawing.Size(75, 20);
            this.cmbChangeLine.TabIndex = 7;
            // 
            // cmbDirect
            // 
            this.cmbDirect.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDirect.FormattingEnabled = true;
            this.cmbDirect.Items.AddRange(new object[] {
            "行",
            "列"});
            this.cmbDirect.Location = new System.Drawing.Point(202, 14);
            this.cmbDirect.Name = "cmbDirect";
            this.cmbDirect.Size = new System.Drawing.Size(75, 20);
            this.cmbDirect.TabIndex = 7;
            // 
            // btnConfirm
            // 
            this.btnConfirm.Location = new System.Drawing.Point(451, 6);
            this.btnConfirm.Name = "btnConfirm";
            this.btnConfirm.Size = new System.Drawing.Size(62, 33);
            this.btnConfirm.TabIndex = 8;
            this.btnConfirm.Text = "确认";
            this.btnConfirm.UseVisualStyleBackColor = true;
            this.btnConfirm.Click += new System.EventHandler(this.btnConfirm_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(14, 16);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 12);
            this.label3.TabIndex = 6;
            this.label3.Text = "起始位置：";
            // 
            // cmbStart
            // 
            this.cmbStart.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbStart.FormattingEnabled = true;
            this.cmbStart.Items.AddRange(new object[] {
            "左上角",
            "左下角",
            "右上角",
            "右下角"});
            this.cmbStart.Location = new System.Drawing.Point(85, 14);
            this.cmbStart.Name = "cmbStart";
            this.cmbStart.Size = new System.Drawing.Size(73, 20);
            this.cmbStart.TabIndex = 7;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(301, 18);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(59, 12);
            this.label7.TabIndex = 6;
            this.label7.Text = "切换方式:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(164, 18);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(41, 12);
            this.label4.TabIndex = 6;
            this.label4.Text = "方向：";
            // 
            // btnSave
            // 
            this.btnSave.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnSave.Location = new System.Drawing.Point(1070, 604);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 38);
            this.btnSave.TabIndex = 3;
            this.btnSave.Text = "保存参数";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // radioButton3
            // 
            this.radioButton3.AutoSize = true;
            this.radioButton3.Location = new System.Drawing.Point(116, 255);
            this.radioButton3.Name = "radioButton3";
            this.radioButton3.Size = new System.Drawing.Size(59, 16);
            this.radioButton3.TabIndex = 15;
            this.radioButton3.TabStop = true;
            this.radioButton3.Text = "特殊盘";
            this.radioButton3.UseVisualStyleBackColor = true;
            // 
            // TestTray
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1232, 657);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "TestTray";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "托盘设置";
            this.Load += new System.EventHandler(this.TestTray_Load);
            this.Enter += new System.EventHandler(this.TestTray_Enter);
            ((System.ComponentModel.ISupportInitialize)(this.nudRow)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudCol)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ndnColumnYoffset)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ndnColumnXoffset)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ndnRowYoffset)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ndnRowXoffset)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ndnFinalBaseIndex)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ndnFinalRowIndex)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ndnFinalColumnIndex)).EndInit();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown nudRow;
        private System.Windows.Forms.NumericUpDown nudCol;
        private System.Windows.Forms.Button btnCreate;
        private System.Windows.Forms.CheckBox chxAddSheild;
        private System.Windows.Forms.CheckBox chxRemoveSheild;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.CheckBox chShow;
        private System.Windows.Forms.Button btnConfirm;
        private System.Windows.Forms.ComboBox cmbDirect;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cmbStart;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox cmbId;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnSelect;
        private System.Windows.Forms.CheckBox cbUnregular2;
        private System.Windows.Forms.CheckBox cbUnregular1;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.ComboBox cmbChangeLine;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtNewPlateType;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label lblIsCalibration;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Timer timer1;
        private Windows.Forms.GroupBox groupBox1;
        private Windows.Forms.NumericUpDown ndnFinalBaseIndex;
        private Windows.Forms.Label label12;
        private Windows.Forms.Label label10;
        private Windows.Forms.NumericUpDown ndnFinalRowIndex;
        private Windows.Forms.Label label11;
        private Windows.Forms.NumericUpDown ndnFinalColumnIndex;
        private Windows.Forms.Label label21;
        private Windows.Forms.NumericUpDown ndnColumnYoffset;
        private Windows.Forms.NumericUpDown ndnColumnXoffset;
        private Windows.Forms.NumericUpDown ndnRowYoffset;
        private Windows.Forms.NumericUpDown ndnRowXoffset;
        private Windows.Forms.Label label18;
        private Windows.Forms.Label label19;
        private Windows.Forms.Label label20;
        private Windows.Forms.RadioButton radioButton2;
        private Windows.Forms.RadioButton radioButton1;
        private Windows.Forms.RadioButton radioButton3;
    }
}