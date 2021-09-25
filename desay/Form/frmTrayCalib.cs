using System;
using System.Drawing;
using System.Windows.Forms;
using System.AdvantechAps;
using System.Interfaces;
using System.ToolKit;
using System.Enginee;
using System.Tray;
using System.Threading;
using System.Threading.Tasks;
using log4net;
namespace desay
{
    /// <summary>
    /// 托盘标定类
    /// </summary>
    public partial class frmTrayCalib : Form
    {
        static ILog log = LogManager.GetLogger(typeof(frmTrayCalib));
        Tray tray = null;//当前托盘对象
        bool IsWrite = false;
        private string PlateKey;
        ApsAxis m_Xaxis;
        ApsAxis m_Yaxis;
        ApsAxis m_Zaxis;
        private readonly Func<bool> _condition;
        TrayPanel TrayDisplay = null;//托盘显示
        public frmTrayCalib()
        {
            InitializeComponent();
        }
        public frmTrayCalib(string Key, ApsAxis Xaxis, ApsAxis Yaxis, ApsAxis Zaxis, Func<bool> Condition) : this()
        {
            PlateKey = Key;
            m_Xaxis = Xaxis;
            m_Yaxis = Yaxis;
            m_Zaxis = Zaxis;
            _condition = Condition;
        }
        #region 托盘显示
        //初始化托盘参数
        private void InitTray()
        {
            if (tray != null)
            {
                TrayDisplay = new TrayPanel();
                TrayDisplay.SetTrayObj(tray, Color.Gray);
                TrayDisplay.Dock = DockStyle.Fill;
                tray.updateColor += TrayDisplay.UpdateColor;
                gpbPlate.Controls.Clear();
                gpbPlate.Controls.Add(TrayDisplay);
            }
        }
        #endregion
        public bool Xdir { get; set; }
        public bool Ydir { get; set; }
        public bool Zdir { get; set; }
        private void frmMotionCardSetting_Load(object sender, EventArgs e)
        {
            var Keys = TrayFactory.GetTrayDict.Keys;
            var count = Keys.Count;
            foreach (var key in Keys)
            {
                if (PlateKey == key)
                {
                    tray = TrayFactory.GetTrayFactory(PlateKey);
                    ndnRowXoffset.Value = (decimal)tray.RowXoffset;
                    ndnRowYoffset.Value = (decimal)tray.RowYoffset;
                    ndnColumnXoffset.Value = (decimal)tray.ColumnXoffset;
                    ndnColumnYoffset.Value = (decimal)tray.ColumnYoffset;
                    lblPlateID.Text = PlateKey;
                    lblTrayName.Text = tray.Name;
                    InitdgvPlatePositionRows();
                    InitTray();
                    tray.SetNumColor(tray.BaseIndex, Color.Blue);
                    tray.SetNumColor(tray.RowIndex, Color.Blue);
                    tray.SetNumColor(tray.ColumnIndex, Color.Blue);
                    tray.updateColor();
                };
            }
            if (tray == null)
            {
                MessageBox.Show("托盘不存在！");
                this.Close();
            }
            else
            {
                lblJogSpeed.Text = "点动速度:" + tbrJogSpeed.Value.ToString("0.00") + "mm/s";
                m_Xaxis.Speed = tbrJogSpeed.Value;
                m_Yaxis.Speed = tbrJogSpeed.Value;
                m_Zaxis.Speed = tbrJogSpeed.Value;

                timer1.Enabled = true;
            }
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            #region 轴状态
            lblCurrentPositionX.Text = m_Xaxis.CurrentPos.ToString("0.000");
            lblCurrentPositionY.Text = m_Yaxis.CurrentPos.ToString("0.000");
            lblCurrentPositionZ.Text = m_Zaxis.CurrentPos.ToString("0.000");
            lblCurrentSpeedX.Text = m_Xaxis.CurrentSpeed.ToString("0.000");
            lblCurrentSpeedY.Text = m_Yaxis.CurrentSpeed.ToString("0.000");
            lblCurrentSpeedZ.Text = m_Zaxis.CurrentSpeed.ToString("0.000");
            #endregion

            checkBox6.Text = "是否伺服ON";
            checkBox2.Text = "是否伺服ON";
            checkBox4.Text = "是否伺服ON";

            checkBox2.Checked = m_Xaxis.IsServon;
            checkBox4.Checked = m_Yaxis.IsServon;
            checkBox6.Checked = m_Zaxis.IsServon;

            if (!IsWrite)
            {
                ndnRowXoffset.Value = (decimal)tray.RowXoffset;
                ndnRowYoffset.Value = (decimal)tray.RowYoffset;
                ndnColumnXoffset.Value = (decimal)tray.ColumnXoffset;
                ndnColumnYoffset.Value = (decimal)tray.ColumnYoffset;

                lblIsCalibration.Text = tray.IsCalibration ? "已标定" : "未标定";
                lblIsCalibration.ForeColor = tray.IsCalibration ? Color.Green : Color.Red;
            }
        }

        #region 手动操作

        #region 三轴定距移动
        private void btnXdec_Click(object sender, EventArgs e)
        {
            if (moveSelectHorizontal1.MoveMode.Continue || Global.IsLocating) return;
            var velocityCurve = new VelocityCurve { Maxvel = m_Xaxis.Speed ?? 0 };
            m_Xaxis.MoveDelta(-1 * moveSelectHorizontal1.MoveMode.Distance, velocityCurve);
        }
        private void btnXadd_Click(object sender, EventArgs e)
        {
            if (moveSelectHorizontal1.MoveMode.Continue || Global.IsLocating) return;
            var velocityCurve = new VelocityCurve { Maxvel = m_Xaxis.Speed ?? 0 };
            m_Xaxis.MoveDelta(1 * moveSelectHorizontal1.MoveMode.Distance, velocityCurve);
        }

        private void btnYdec_Click(object sender, EventArgs e)
        {
            if (moveSelectHorizontal1.MoveMode.Continue || Global.IsLocating) return;
            var velocityCurve = new VelocityCurve { Maxvel = m_Yaxis.Speed ?? 0 };
            m_Yaxis.MoveDelta(1 * moveSelectHorizontal1.MoveMode.Distance, velocityCurve);
        }

        private void btnYadd_Click(object sender, EventArgs e)
        {
            if (moveSelectHorizontal1.MoveMode.Continue || Global.IsLocating) return;
            var velocityCurve = new VelocityCurve { Maxvel = m_Yaxis.Speed ?? 0 };
            m_Yaxis.MoveDelta(-1 * moveSelectHorizontal1.MoveMode.Distance, velocityCurve);
        }

        private void btnZadd_Click(object sender, EventArgs e)
        {
            if (moveSelectHorizontal1.MoveMode.Continue || Global.IsLocating) return;
            var velocityCurve = new VelocityCurve { Maxvel = m_Zaxis.Speed ?? 0 };
            m_Zaxis.MoveDelta(1 * moveSelectHorizontal1.MoveMode.Distance, velocityCurve);
        }

        private void btnZdec_Click(object sender, EventArgs e)
        {
            if (moveSelectHorizontal1.MoveMode.Continue || Global.IsLocating) return;
            var velocityCurve = new VelocityCurve { Maxvel = m_Zaxis.Speed ?? 0 };
            m_Zaxis.MoveDelta(-1 * moveSelectHorizontal1.MoveMode.Distance, velocityCurve);
        }
        #endregion

        #region 三轴连续移动
        private void btnXdec_MouseDown(object sender, MouseEventArgs e)
        {
            if (!moveSelectHorizontal1.MoveMode.Continue || Global.IsLocating) return;
            m_Xaxis.Negative();
        }

        private void btnXdec_MouseUp(object sender, MouseEventArgs e)
        {
            if (!moveSelectHorizontal1.MoveMode.Continue || Global.IsLocating) return;
            m_Xaxis.Stop();
        }

        private void btnXadd_MouseDown(object sender, MouseEventArgs e)
        {
            if (!moveSelectHorizontal1.MoveMode.Continue || Global.IsLocating) return;
            m_Xaxis.Postive();
        }

        private void btnXadd_MouseUp(object sender, MouseEventArgs e)
        {
            if (!moveSelectHorizontal1.MoveMode.Continue || Global.IsLocating) return;
            m_Xaxis.Stop();
        }

        private void btnYdec_MouseDown(object sender, MouseEventArgs e)
        {
            if (!moveSelectHorizontal1.MoveMode.Continue || Global.IsLocating) return;
            m_Yaxis.Postive();
        }

        private void btnYdec_MouseUp(object sender, MouseEventArgs e)
        {
            if (!moveSelectHorizontal1.MoveMode.Continue || Global.IsLocating) return;
            m_Yaxis.Stop();
        }

        private void btnYadd_MouseDown(object sender, MouseEventArgs e)
        {
            if (!moveSelectHorizontal1.MoveMode.Continue || Global.IsLocating) return;
            m_Yaxis.Negative();
        }

        private void btnYadd_MouseUp(object sender, MouseEventArgs e)
        {
            if (!moveSelectHorizontal1.MoveMode.Continue || Global.IsLocating) return;
            m_Yaxis.Stop();
        }
        private void btnZdec_MouseDown(object sender, MouseEventArgs e)
        {
            if (!moveSelectHorizontal1.MoveMode.Continue || Global.IsLocating) return;
            m_Zaxis.Negative();
        }

        private void btnZdec_MouseUp(object sender, MouseEventArgs e)
        {
            if (!moveSelectHorizontal1.MoveMode.Continue || Global.IsLocating) return;
            m_Zaxis.Stop();
        }

        private void btnZadd_MouseDown(object sender, MouseEventArgs e)
        {
            if (!moveSelectHorizontal1.MoveMode.Continue || Global.IsLocating) return;
            m_Zaxis.Postive();
        }

        private void btnZadd_MouseUp(object sender, MouseEventArgs e)
        {
            if (!moveSelectHorizontal1.MoveMode.Continue || Global.IsLocating) return;
            m_Zaxis.Stop();
        }
        #endregion

        private void tbrJogSpeed_Scroll(object sender, EventArgs e)
        {
            if (Global.IsLocating) return;
            lblJogSpeed.Text = "点动速度:" + tbrJogSpeed.Value.ToString("0.00") + "mm/s";
            m_Xaxis.Speed = (double)tbrJogSpeed.Value;
            m_Yaxis.Speed = (double)tbrJogSpeed.Value;
            m_Zaxis.Speed = (double)tbrJogSpeed.Value;
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            m_Xaxis.IsServon = checkBox2.Checked;
        }
        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            m_Yaxis.IsServon = checkBox4.Checked;
        }
        private void checkBox6_CheckedChanged(object sender, EventArgs e)
        {
            m_Zaxis.IsServon = checkBox6.Checked;
        }
        #endregion

        private void dgvPlatePosition_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            switch (e.ColumnIndex)
            {
                case 5:
                    if (MessageBox.Show(string.Format("是否保存{0}的数据", dgvPlatePosition.Rows[e.RowIndex].Cells[0].Value),
                        "保存", MessageBoxButtons.OKCancel) == DialogResult.Cancel) break;
                    if (dgvPlatePosition.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString() == "Save")
                    {
                        var pos = new Point3D<double>();
                        switch (e.RowIndex)
                        {
                            case 0://基准点
                                pos.X = m_Xaxis.CurrentPos;
                                pos.Y = m_Yaxis.CurrentPos;
                                pos.Z = m_Zaxis.CurrentPos;
                                tray.BasePosition = pos;
                                break;
                            case 1://行方向终点
                                pos.X = m_Xaxis.CurrentPos;
                                pos.Y = m_Yaxis.CurrentPos;
                                pos.Z = m_Zaxis.CurrentPos;
                                tray.RowPosition = pos;
                                break;
                            case 2://列方向终点
                                pos.X = m_Xaxis.CurrentPos;
                                pos.Y = m_Yaxis.CurrentPos;
                                pos.Z = m_Zaxis.CurrentPos;
                                tray.ColumnPosition = pos;
                                break;
                            default: break;
                        }
                        RefreshdgvPlatePositionRows(e.RowIndex);
                    }
                    break;
                case 6:
                    if (MessageBox.Show(string.Format("是否定位到{0}", dgvPlatePosition.Rows[e.RowIndex].Cells[0].Value),
                        "保存", MessageBoxButtons.OKCancel) == DialogResult.Cancel) break;
                    if (dgvPlatePosition.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString() == "Goto")
                    {
                        var ret = 0;
                        switch (e.RowIndex)
                        {
                            case 0://基准点
                                ret = MoveToPoint(tray.BasePosition);
                                break;
                            case 1://行方向终点
                                ret = MoveToPoint(tray.RowPosition);
                                break;
                            case 2://列方向终点
                                ret = MoveToPoint(tray.ColumnPosition);
                                break;
                            default: break;
                        }
                        switch (ret)
                        {
                            case -2:
                                MessageBox.Show("伺服定位异常失败！");
                                break;
                            case -3:
                                MessageBox.Show("伺服未使能！");
                                break;
                            case -4:
                                MessageBox.Show("伺服忙碌中！");
                                break;
                        }
                    }
                    break;
                default: break;
            }
        }

        #region 数据表格初始化
        /// <summary>
        /// 数据初始化
        /// </summary>
        private void InitdgvPlatePositionRows()
        {
            this.dgvPlatePosition.Rows.Clear();
            //in a real scenario, you may need to add different rows
            dgvPlatePosition.Rows.Add(new object[] {
                    "基准点位置",
                    tray.BaseIndex.ToString(),
                    tray.BasePosition.X.ToString("0.000"),
                    tray.BasePosition.Y.ToString("0.000"),
                    tray.BasePosition.Z.ToString("0.000"),
                    "Save",
                    "Goto"
                });
            dgvPlatePosition.Rows.Add(new object[] {
                    "行终点位置",
                    tray.RowIndex.ToString(),
                    tray.RowPosition.X.ToString("0.000"),
                    tray.RowPosition.Y.ToString("0.000"),
                    tray.RowPosition.Z.ToString("0.000"),
                    "Save",
                    "Goto"
                });
            dgvPlatePosition.Rows.Add(new object[] {
                    "列终点位置",
                    tray.ColumnIndex.ToString(),
                    tray.ColumnPosition.X.ToString("0.000"),
                    tray.ColumnPosition.Y.ToString("0.000"),
                    tray.ColumnPosition.Z.ToString("0.000"),
                    "Save",
                    "Goto"
                });
        }
        #endregion

        #region 数据表格刷新
        /// <summary>
        /// 准数据刷新
        /// </summary>
        private void RefreshdgvPlatePositionRows(int i)
        {
            switch (i)
            {
                case 0:
                    dgvPlatePosition.Rows[i].SetValues(new object[] {
                        "基准点位置",
                        tray.BaseIndex.ToString(),
                        tray.BasePosition.X.ToString("0.000"),
                        tray.BasePosition.Y.ToString("0.000"),
                        tray.BasePosition.Z.ToString("0.000"),
                        "Save",
                        "Goto"
                    });
                    break;
                case 1:
                    dgvPlatePosition.Rows[i].SetValues(new object[] {
                        "行终点位置",
                        tray.RowIndex.ToString(),
                        tray.RowPosition.X.ToString("0.000"),
                        tray.RowPosition.Y.ToString("0.000"),
                        tray.RowPosition.Z.ToString("0.000"),
                        "Save",
                        "Goto"
                    });
                    break;
                case 2:
                    dgvPlatePosition.Rows[i].SetValues(new object[] {
                        "列终点位置",
                        tray.ColumnIndex.ToString(),
                        tray.ColumnPosition.X.ToString("0.000"),
                        tray.ColumnPosition.Y.ToString("0.000"),
                        tray.ColumnPosition.Z.ToString("0.000"),
                        "Save",
                        "Goto"
                    });
                    break;
                default: break;
            }
        }
        #endregion

        private int MoveToPoint(Point3D<double> pos)
        {
            if (!_condition()) return -1;
            Global.IsLocating = true;
            Task.Factory.StartNew(() =>
            {
                try
                {
                    //判断Z轴是否在零点
                    if (!m_Zaxis.IsInPosition(0))
                        m_Zaxis.MoveTo(0, new VelocityCurve(0, (double)m_Zaxis.Speed, 0));
                    while (true)
                    {
                        if (!Global.IsLocating) return -1;
                        Thread.Sleep(10);
                        if (m_Zaxis.IsInPosition(0)) break;
                        if (ServoAxisIsReady(m_Zaxis))
                        {
                            m_Zaxis.Stop();
                            Global.IsLocating = false;
                            return -4;
                        }
                    }
                    //将X、Y移动到指定位置
                    if (!m_Xaxis.IsInPosition(pos.X))
                        m_Xaxis.MoveTo(pos.X, new VelocityCurve(0, (double)m_Xaxis.Speed, 0));
                    if (!m_Yaxis.IsInPosition(pos.Y))
                        m_Yaxis.MoveTo(pos.Y, new VelocityCurve(0, (double)m_Yaxis.Speed, 0));
                    while (true)
                    {
                        if (!Global.IsLocating) return -1;
                        Thread.Sleep(10);
                        if (m_Xaxis.IsInPosition(pos.X) && m_Yaxis.IsInPosition(pos.Y)) break;
                        if (ServoAxisIsReady(m_Xaxis) || ServoAxisIsReady(m_Yaxis))
                        {
                            m_Xaxis.Stop();
                            m_Yaxis.Stop();
                            Global.IsLocating = false;
                            return -4;
                        }
                    }
                    //将Z轴移动到指定位置
                    m_Zaxis.MoveTo(pos.Z, new VelocityCurve(0, (double)m_Zaxis.Speed, 0));
                    while (true)
                    {
                        if (!Global.IsLocating) return -1;
                        Thread.Sleep(10);
                        if (m_Zaxis.IsInPosition(pos.Z)) break;
                        if (ServoAxisIsReady(m_Zaxis))
                        {
                            m_Zaxis.Stop();
                            Global.IsLocating = false;
                            return -4;
                        }
                    }
                    Global.IsLocating = false;
                    return 0;
                }
                catch (Exception ex)
                {
                    Global.IsLocating = false;
                    log.Fatal("设备驱动程序异常", ex);
                    return -2;
                }
            }, TaskCreationOptions.AttachedToParent | TaskCreationOptions.LongRunning);

            return 0;
        }

        private void btnCalibCal_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("是否进行标定计算？", "标定计算", MessageBoxButtons.OKCancel);
            if (result == DialogResult.Cancel) return;
            if (tray == null)
            {
                MessageBox.Show("托盘信息为空！");
                return;
            }
            try
            {
                IsWrite = true;
                var ret = TrayFactory.Calibration(PlateKey);
                if (!ret) MessageBox.Show("托盘标定失败！");
            }
            catch (Exception ex)
            {
                MessageBox.Show("托盘标定失败！" + ex.ToString());
            }
            IsWrite = false;
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            tray.RowXoffset = (double)ndnRowXoffset.Value;
            tray.RowYoffset = (double)ndnRowYoffset.Value;
            tray.ColumnXoffset = (double)ndnColumnXoffset.Value;
            tray.ColumnYoffset = (double)ndnColumnYoffset.Value;
        }
        private bool ServoAxisIsReady(ApsAxis axis)
        {
            return !axis.IsServon || axis.IsAlarmed || axis.IsEmg || axis.IsMEL || axis.IsPEL;
        }

        private void frmTrayCalib_FormClosing(object sender, FormClosingEventArgs e)
        {
            tray.ResetTrayColor(Color.Gray);
            tray.updateColor();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            int Pos = Convert.ToInt32(numericUpDown1.Value);
            Point3D<double> pos1 = tray.GetPosition(tray.BasePosition, Pos);
            MoveToPoint(pos1);
        }
    }
}
