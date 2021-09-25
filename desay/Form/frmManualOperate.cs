using System;
using System.Diagnostics;
using System.Drawing;
using System.Enginee;
using System.Interfaces;
using System.Threading.Tasks;
using System.ToolKit;
using System.Windows.Forms;
using System.Threading;
namespace desay
{
    public partial class frmManualOperate : Form
    {

        private Splice m_Splice;
        private Buffer m_Buffer;
        private Feeder m_Feeder;
        private Move m_Move;
        private LeftC m_LeftC;
        private LeftCut1 m_LeftCut1;
        private LeftCut2 m_LeftCut2;
        private RightCut1 m_RightCut1;
        private RightCut2 m_RightCut2;
        private Platform m_Platform;
        private Storing m_Storing;

        CylinderOperate Cut1GrripperCylinderOperate, Cut1OverturnCylinderOperate, Cut1FrontCylinderOperate;
        CylinderOperate Cut2GrripperCylinderOperate, Cut2OverturnCylinderOperate, Cut2FrontCylinderOperate;
        CylinderOperate Cut3GrripperCylinderOperate, Cut3OverturnCylinderOperate, Cut3FrontCylinderOperate;
        CylinderOperate Cut4GrripperCylinderOperate, Cut4OverturnCylinderOperate, Cut4FrontCylinderOperate;

        CylinderOperate CutwasteCylinderOperate1, CutwasteCylinderOperate2, CutwasteCylinderOperate3;
        CylinderOperate MoveUpCylinderOperate, MoveGripperCylinderOperate, MoveLeftCylinderOperate;

        CylinderOperate BufUpCylinderOperate, BufGripperCylinderOperate, BufLeftCylinderOperate;

        CylinderOperate FeederCylinderOperate;

        CylinderOperate SpliCylinderOperate;

        CylinderOperate DownXyzCylinderOperate, LuckXyzCylinderOperate, Left1XyzCylinderOperate;

        CylinderOperate SafeDoolCylinderOperate;

        #region 控件
        #endregion

        private frmManualOperate()
        {
            InitializeComponent();
        }

        public frmManualOperate(Splice Splice, Buffer Buffer, Feeder Feeder, Move Move, LeftC LeftC,
           LeftCut1 LeftCut1, LeftCut2 LeftCut2, RightCut1 RightCut1, RightCut2 RightCut2, Platform Platform, Storing Storing) : this()
        {
            m_Splice = Splice;
            m_Buffer = Buffer;
            m_Feeder = Feeder;
            m_Move = Move;
            m_LeftC = LeftC;
            m_LeftCut1 = LeftCut1;
            m_LeftCut2 = LeftCut2;
            m_RightCut1 = RightCut1;
            m_RightCut2 = RightCut2;
            m_Platform = Platform;
            m_Storing = Storing;
            Caxis[0] = 0;
            Caxis[1] = 0;
            Caxis[2] = 0;
            Caxis[3] = 0;
        }

        private void frmTeach_Load(object sender, EventArgs e)
        {
            Cut1FrontCylinderOperate = new CylinderOperate(m_LeftCut1.FrontCylinder);
            Cut1OverturnCylinderOperate = new CylinderOperate(m_LeftCut1.OverturnCylinder);
            Cut1GrripperCylinderOperate = new CylinderOperate(m_LeftCut1.GripperCylinder);
            Cut1FrontCylinderOperate.Size = new Size(105, 23);
            Cut1OverturnCylinderOperate.Size = new Size(105, 23);
            Cut1GrripperCylinderOperate.Size = new Size(105, 23);
            GrboxCut1.Controls.Add(Cut1FrontCylinderOperate);
            GrboxCut1.Controls.Add(Cut1OverturnCylinderOperate);
            GrboxCut1.Controls.Add(Cut1GrripperCylinderOperate);

            Cut2FrontCylinderOperate = new CylinderOperate(m_LeftCut2.FrontCylinder);
            Cut2OverturnCylinderOperate = new CylinderOperate(m_LeftCut2.OverturnCylinder);
            Cut2GrripperCylinderOperate = new CylinderOperate(m_LeftCut2.GripperCylinder);
            Cut2FrontCylinderOperate.Size = new Size(105, 23);
            Cut2OverturnCylinderOperate.Size = new Size(105, 23);
            Cut2GrripperCylinderOperate.Size = new Size(105, 23);
            GrboxCut2.Controls.Add(Cut2FrontCylinderOperate);
            GrboxCut2.Controls.Add(Cut2OverturnCylinderOperate);
            GrboxCut2.Controls.Add(Cut2GrripperCylinderOperate);

            Cut3FrontCylinderOperate = new CylinderOperate(m_RightCut1.FrontCylinder);
            Cut3OverturnCylinderOperate = new CylinderOperate(m_RightCut1.OverturnCylinder);
            Cut3GrripperCylinderOperate = new CylinderOperate(m_RightCut1.GripperCylinder);
            Cut3FrontCylinderOperate.Size = new Size(105, 23);
            Cut3OverturnCylinderOperate.Size = new Size(105, 23);
            Cut3GrripperCylinderOperate.Size = new Size(105, 23);
            GrboxCut3.Controls.Add(Cut3FrontCylinderOperate);
            GrboxCut3.Controls.Add(Cut3OverturnCylinderOperate);
            GrboxCut3.Controls.Add(Cut3GrripperCylinderOperate);

            Cut4FrontCylinderOperate = new CylinderOperate(m_RightCut2.FrontCylinder);
            Cut4OverturnCylinderOperate = new CylinderOperate(m_RightCut2.OverturnCylinder);
            Cut4GrripperCylinderOperate = new CylinderOperate(m_RightCut2.GripperCylinder);
            Cut4FrontCylinderOperate.Size = new Size(105, 23);
            Cut4OverturnCylinderOperate.Size = new Size(105, 23);
            Cut4GrripperCylinderOperate.Size = new Size(105, 23);
            GrboxCut4.Controls.Add(Cut4FrontCylinderOperate);
            GrboxCut4.Controls.Add(Cut4OverturnCylinderOperate);
            GrboxCut4.Controls.Add(Cut4GrripperCylinderOperate);

            if (0 == Position.Instance.FragmentationMode)
            {
                CutwasteCylinderOperate1 = new CylinderOperate(m_Move.CutwasteCylinder1);
                CutwasteCylinderOperate2 = new CylinderOperate(m_Move.CutwasteCylinder2);
                CutwasteCylinderOperate3 = new CylinderOperate(m_Move.CutwasteCylinder3);
                CutwasteCylinderOperate1.Size = new Size(100, 23);
                CutwasteCylinderOperate2.Size = new Size(100, 23);
                CutwasteCylinderOperate3.Size = new Size(100, 23);
                Cutwaste.Controls.Add(CutwasteCylinderOperate1);
                Cutwaste.Controls.Add(CutwasteCylinderOperate2);
                Cutwaste.Controls.Add(CutwasteCylinderOperate3);
            }

            MoveGripperCylinderOperate = new CylinderOperate(m_Move.GripperCylinder);
            MoveLeftCylinderOperate = new CylinderOperate(m_Move.LeftCylinder);
            MoveUpCylinderOperate = new CylinderOperate(m_Move.DownCylinder);
            MoveGripperCylinderOperate.Size = new Size(100, 23);
            MoveLeftCylinderOperate.Size = new Size(100, 23);
            MoveUpCylinderOperate.Size = new Size(100, 23);
            Move.Controls.Add(MoveGripperCylinderOperate);
            Move.Controls.Add(MoveLeftCylinderOperate);
            Move.Controls.Add(MoveUpCylinderOperate);

            BufUpCylinderOperate = new CylinderOperate(m_Buffer.DownCylinder);
            BufLeftCylinderOperate = new CylinderOperate(m_Buffer.LeftCylinder);
            BufGripperCylinderOperate = new CylinderOperate(m_Buffer.GripperCylinder);
            BufUpCylinderOperate.Size = new Size(100, 23);
            BufLeftCylinderOperate.Size = new Size(100, 23);
            BufGripperCylinderOperate.Size = new Size(100, 23);
            Buf.Controls.Add(BufUpCylinderOperate);
            Buf.Controls.Add(BufLeftCylinderOperate);
            Buf.Controls.Add(BufGripperCylinderOperate);

            FeederCylinderOperate = new CylinderOperate(m_Feeder.FeederCylinder);
            FeederCylinderOperate.Size = new Size(100, 23);
            flpFeeder.Controls.Add(FeederCylinderOperate);

            SpliCylinderOperate = new CylinderOperate(m_Splice.NoRodFeedCylinder);
            SpliCylinderOperate.Size = new Size(100, 23);
            Splice.Controls.Add(SpliCylinderOperate);

            LuckXyzCylinderOperate = new CylinderOperate(m_Platform.LockCylinder);
            LuckXyzCylinderOperate.Size = new Size(105, 23);
            Luck.Controls.Add(LuckXyzCylinderOperate);

            DownXyzCylinderOperate = new CylinderOperate(m_Platform.GetTrayCylinder);
            DownXyzCylinderOperate.Size = new Size(105, 23);
            XYZdown.Controls.Add(DownXyzCylinderOperate);

            Left1XyzCylinderOperate = new CylinderOperate(m_Platform.Left1Cylinder);
            Left1XyzCylinderOperate.Size = new Size(160, 23);
            left1.Controls.Add(Left1XyzCylinderOperate);

            SafeDoolCylinderOperate = new CylinderOperate(m_Storing.SafeDoolCylinder);
            SafeDoolCylinderOperate.Size = new Size(105, 23);
            left2.Controls.Add(SafeDoolCylinderOperate);

            timer1.Enabled = true;
        }
        int Pos;
        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Enabled = false;

            Cut1GrripperCylinderOperate.Refreshing();
            Cut2GrripperCylinderOperate.Refreshing();
            Cut3GrripperCylinderOperate.Refreshing();
            Cut4GrripperCylinderOperate.Refreshing();
            Cut1FrontCylinderOperate.Refreshing();
            Cut2FrontCylinderOperate.Refreshing();
            Cut3FrontCylinderOperate.Refreshing();
            Cut4FrontCylinderOperate.Refreshing();
            Cut1OverturnCylinderOperate.Refreshing();
            Cut2OverturnCylinderOperate.Refreshing();
            Cut3OverturnCylinderOperate.Refreshing();
            Cut4OverturnCylinderOperate.Refreshing();
            if (0 == Position.Instance.FragmentationMode)
            {
                CutwasteCylinderOperate1.Refreshing();
                CutwasteCylinderOperate2.Refreshing();
                CutwasteCylinderOperate3.Refreshing();
            }
            MoveGripperCylinderOperate.Refreshing();
            MoveLeftCylinderOperate.Refreshing();
            MoveUpCylinderOperate.Refreshing();
            BufUpCylinderOperate.Refreshing();
            BufLeftCylinderOperate.Refreshing();
            BufGripperCylinderOperate.Refreshing();
            FeederCylinderOperate.Refreshing();
            SpliCylinderOperate.Refreshing();
            LuckXyzCylinderOperate.Refreshing();
            DownXyzCylinderOperate.Refreshing();
            Left1XyzCylinderOperate.Refreshing();
            SafeDoolCylinderOperate.Refreshing();
            Pos = Convert.ToInt32((m_Storing.MAxis.CurrentPos - Position.Instance.MStartPosition) / Position.Instance.MDistance);

            lblcurrentOperation.Text = (Position.Instance.MLayerIndex).ToString();
            lblOperationMView.Text = (Pos + 1).ToString();

            btnInhale1On.BackColor = IoPoints.T1DO31.Value ? Color.Green : Color.Gray;
            btnInhale1OFF.BackColor = IoPoints.T1DO30.Value ? Color.Green : Color.Gray;
            btnInhale2On.BackColor = IoPoints.T1DO29.Value ? Color.Green : Color.Gray;
            btnInhale2OFF.BackColor = IoPoints.T1DO28.Value ? Color.Green : Color.Gray;
            btnInhale3On.BackColor = IoPoints.T1DO27.Value ? Color.Green : Color.Gray;
            btnInhale3OFF.BackColor = IoPoints.T1DO26.Value ? Color.Green : Color.Gray;
            btnInhale4On.BackColor = IoPoints.T1DO25.Value ? Color.Green : Color.Gray;
            btnInhale4OFF.BackColor = IoPoints.T1DO24.Value ? Color.Green : Color.Gray;
            
            btnLightOn.BackColor = IoPoints.T2DO5.Value ? Color.Green : Color.Gray;
            btnFeed.BackColor = IoPoints.T2DO12.Value ? Color.Green : Color.Gray;
            btnHoot4.BackColor = IoPoints.T2DO9.Value ? Color.Green : Color.Gray;
            btnHoot3.BackColor = IoPoints.T2DO8.Value ? Color.Green : Color.Gray;
            btnHoot2.BackColor = IoPoints.T2DO7.Value ? Color.Green : Color.Gray;
            btnHoot1.BackColor = IoPoints.T2DO6.Value ? Color.Green : Color.Gray;

            if (0 != Caxis[0]) lblC1Number.Text = Caxis[0].ToString();
            if (0 != Caxis[1]) lblC2Number.Text = Caxis[1].ToString();
            if (0 != Caxis[2]) lblC3Number.Text = Caxis[2].ToString();
            if (0 != Caxis[3]) lblC4Number.Text = Caxis[3].ToString();

            timer1.Enabled = true;
        }


        private void BtnInhale1On_Click(object sender, EventArgs e)
        {
            m_Platform.Right2InhaleCylinder.ClearBrokenTime();
            m_Platform.Right2InhaleCylinder.Delay.BrokenTime = 99999999;
            IoPoints.T1DO31.Value = !IoPoints.T1DO31.Value;
        }

        private void BtnInhale1OFF_Click(object sender, EventArgs e)
        {
            IoPoints.T1DO30.Value = !IoPoints.T1DO30.Value;
        }

        private void BtnInhale2On_Click(object sender, EventArgs e)
        {
            m_Platform.Right1InhaleCylinder.ClearBrokenTime();
            m_Platform.Right1InhaleCylinder.Delay.BrokenTime = 99999999;
            IoPoints.T1DO29.Value = !IoPoints.T1DO29.Value;
        }

        private void BtnInhale2OFF_Click(object sender, EventArgs e)
        {
            IoPoints.T1DO28.Value = !IoPoints.T1DO28.Value;
        }

        private void BtnInhale3On_Click(object sender, EventArgs e)
        {
            m_Platform.Left2InhaleCylinder.ClearBrokenTime();
            m_Platform.Left2InhaleCylinder.Delay.BrokenTime = 99999999;
            IoPoints.T1DO27.Value = !IoPoints.T1DO27.Value;
        }

        private void BtnInhale3OFF_Click(object sender, EventArgs e)
        {
            IoPoints.T1DO26.Value = !IoPoints.T1DO26.Value;
        }

        private void BtnInhale4On_Click(object sender, EventArgs e)
        {
            m_Platform.Left1InhaleCylinder.ClearBrokenTime();
            m_Platform.Left1InhaleCylinder.Delay.BrokenTime = 99999999;
            IoPoints.T1DO25.Value = !IoPoints.T1DO25.Value;
        }

        private void BtnInhale4OFF_Click(object sender, EventArgs e)
        {
            IoPoints.T1DO24.Value = !IoPoints.T1DO24.Value;
        }

        private void BtnLightOn_Click(object sender, EventArgs e)
        {
            IoPoints.T2DO5.Value = !IoPoints.T2DO5.Value;
        }

        private void BtnFeed_Click(object sender, EventArgs e)
        {
            IoPoints.T2DO12.Value = !IoPoints.T2DO12.Value;
        }

        private void BtnHoot1_Click(object sender, EventArgs e)
        {
            IoPoints.T2DO6.Value = !IoPoints.T2DO6.Value;
        }

        private void BtnHoot2_Click(object sender, EventArgs e)
        {           
            IoPoints.T2DO7.Value = !IoPoints.T2DO7.Value;
        }

        private void BtnHoot3_Click(object sender, EventArgs e)
        {
            IoPoints.T2DO8.Value = !IoPoints.T2DO8.Value;
        }

        private void BtnHoot4_Click(object sender, EventArgs e)
        {            
            IoPoints.T2DO9.Value = !IoPoints.T2DO9.Value;
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            LogHelper.Info("手动第一层");
            if (Global.IsLocating) return;
            Global.IsLocating = true;

            Task.Factory.StartNew(() =>
            {
                m_Storing.MAxis.MoveTo(Position.Instance.MStartPosition, AxisParameter.Instance.MVelocityCurve);
                while (true)
                {
                    Thread.Sleep(10);
                    if (m_Storing.MAxis.IsDone && m_Storing.MAxis.CurrentSpeed == 0)
                    {
                        Global.mViewOperation = 1;
                        Global.IsLocating = false;
                        return;
                    }
                    if (!Global.IsLocating) return;
                }
            }, TaskCreationOptions.AttachedToParent | TaskCreationOptions.LongRunning);
        }

        private void BtnFlowTest_Click(object sender, EventArgs e)
        {
            FrmTest frmtest = new FrmTest(m_Splice, m_Buffer, m_Feeder, m_Move, m_LeftC, m_LeftCut1, m_LeftCut2, m_RightCut1, m_RightCut2, m_Platform, m_Storing);
            frmtest.ShowDialog();
        }

        private void ClipReturn4_CheckedChanged(object sender, EventArgs e)
        {
            Task.Factory.StartNew(() =>
            {
                while (ClipReturn4.Checked)
                {
                    Thread.Sleep(10);
                    if (m_RightCut2.GripperCylinder.OutOriginStatus)
                    {
                        m_RightCut2.GripperCylinder.Set();
                        Thread.Sleep(Delay.Instance.GripperCylinderTime);
                    }
                    if (m_RightCut2.GripperCylinder.OutMoveStatus)
                    {
                        m_RightCut2.GripperCylinder.Reset();
                        Thread.Sleep(Delay.Instance.GripperCylinderTime);
                    }
                }
            }, TaskCreationOptions.AttachedToParent | TaskCreationOptions.LongRunning);
        }

        private void ClipReturn3_CheckedChanged(object sender, EventArgs e)
        {
            Task.Factory.StartNew(() =>
            {
                while (ClipReturn3.Checked)
                {
                    Thread.Sleep(10);
                    if (m_RightCut1.GripperCylinder.OutOriginStatus)
                    {
                        m_RightCut1.GripperCylinder.Set();
                        Thread.Sleep(Delay.Instance.GripperCylinderTime);
                    }
                    if (m_RightCut1.GripperCylinder.OutMoveStatus)
                    {
                        m_RightCut1.GripperCylinder.Reset();
                        Thread.Sleep(Delay.Instance.GripperCylinderTime);
                    }
                }
            }, TaskCreationOptions.AttachedToParent | TaskCreationOptions.LongRunning);
        }

        private void ClipReturn2_CheckedChanged(object sender, EventArgs e)
        {
            Task.Factory.StartNew(() =>
            {
                while (ClipReturn2.Checked)
                {
                    Thread.Sleep(10);
                    if (m_LeftCut2.GripperCylinder.OutOriginStatus)
                    {
                        m_LeftCut2.GripperCylinder.Set();
                        Thread.Sleep(Delay.Instance.GripperCylinderTime);
                    }
                    if (m_LeftCut2.GripperCylinder.OutMoveStatus)
                    {
                        m_LeftCut2.GripperCylinder.Reset();
                        Thread.Sleep(Delay.Instance.GripperCylinderTime);
                    }
                }
                Global.IsLocating = false;
            }, TaskCreationOptions.AttachedToParent | TaskCreationOptions.LongRunning);
        }

        private void ClipReturn1_CheckedChanged(object sender, EventArgs e)
        {
            Task.Factory.StartNew(() =>
            {
                while (ClipReturn1.Checked)
                {
                    Thread.Sleep(10);
                    if (m_LeftCut1.GripperCylinder.OutOriginStatus)
                    {
                        m_LeftCut1.GripperCylinder.Set();
                        Thread.Sleep(Delay.Instance.GripperCylinderTime);
                    }
                    if (m_LeftCut1.GripperCylinder.OutMoveStatus)
                    {
                        m_LeftCut1.GripperCylinder.Reset();
                        Thread.Sleep(Delay.Instance.GripperCylinderTime);
                    }
                }
            }, TaskCreationOptions.AttachedToParent | TaskCreationOptions.LongRunning);
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            if (Global.IsLocating) return;
            Global.IsLocating = true;
            Global.mViewOperation = Pos + 1;
            Task.Factory.StartNew(() =>
            {
                if (Global.mViewOperation >= (Position.Instance.MLayerCount)) { Global.mViewOperation = Position.Instance.MLayerCount - 1; }
                double pos = Position.Instance.MDistance * (Global.mViewOperation) + Position.Instance.MStartPosition;
                m_Storing.MAxis.MoveTo(pos, AxisParameter.Instance.MVelocityCurve);
                while (true)
                {
                    if (m_Storing.MAxis.IsDone && m_Storing.MAxis.CurrentSpeed == 0)
                    {
                        Global.IsLocating = false;
                        return;
                    }
                    if (!Global.IsLocating) return;
                }
            }, TaskCreationOptions.AttachedToParent | TaskCreationOptions.LongRunning);
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            if (Global.IsLocating) return;
            Global.IsLocating = true;

            Task.Factory.StartNew(() =>
            {
                Global.mViewOperation = Pos - 1;
                if (Global.mViewOperation <= 0) { Global.mViewOperation = 0; }
                double pos = Position.Instance.MDistance * (Global.mViewOperation) + Position.Instance.MStartPosition;
                m_Storing.MAxis.MoveTo(pos, AxisParameter.Instance.MVelocityCurve);
                while (true)
                {
                    Thread.Sleep(10);
                    if (m_Storing.MAxis.IsDone && m_Storing.MAxis.CurrentSpeed == 0)
                    {
                        Global.IsLocating = false;
                        return;
                    }
                    if (!Global.IsLocating) return;
                }
            }, TaskCreationOptions.AttachedToParent | TaskCreationOptions.LongRunning);
        }

        private void Button4_Click(object sender, EventArgs e)
        {
            LogHelper.Info("设定为作业层");
            if(0 != Pos)
            {
                Position.Instance.MLayerIndex = Pos + 1;
            }
            else
            {
                MessageBox.Show("第一层为抽检层,无法设置作业层");
            }
        }

        private void Button23_Click(object sender, EventArgs e)
        {
            button23.BackColor = System.Drawing.Color.Green;            
            CutTestOne(0);
            button23.BackColor = Color.FromKnownColor(System.Drawing.KnownColor.Control);
            LogHelper.Info("1#手动剪切操作");
        }

        /// <summary>
        /// 剪切一次
        /// </summary>
        /// <param name="Step">位置</param> 
        /// <returns></returns>
        private int CutTestOne(int Step)
        {
            if (Global.IsLocating) return -1;
            ApsAxis axis = null;
            SingleCylinder OverturnCylinder = null, FrontCylinder = null, GripperCylinder = null;
            VelocityCurve velocityCurve = null;
            switch (Step)
            {
                case 0:
                    axis = m_LeftCut1.CutAxis;
                    OverturnCylinder = m_LeftCut1.OverturnCylinder;
                    FrontCylinder = m_LeftCut1.FrontCylinder;
                    GripperCylinder = m_LeftCut1.GripperCylinder;
                    velocityCurve = AxisParameter.Instance.Cut1VelocityCurve;
                    break;
                case 1:
                    axis = m_LeftCut2.CutAxis;
                    OverturnCylinder = m_LeftCut2.OverturnCylinder;
                    FrontCylinder = m_LeftCut2.FrontCylinder;
                    GripperCylinder = m_LeftCut2.GripperCylinder;
                    velocityCurve = AxisParameter.Instance.Cut2VelocityCurve;
                    break;
                case 2:
                    axis = m_RightCut1.CutAxis;
                    OverturnCylinder = m_RightCut1.OverturnCylinder;
                    FrontCylinder = m_RightCut1.FrontCylinder;
                    GripperCylinder = m_RightCut1.GripperCylinder;
                    velocityCurve = AxisParameter.Instance.Cut3VelocityCurve;
                    break;
                case 3:
                    axis = m_RightCut2.CutAxis;
                    OverturnCylinder = m_RightCut2.OverturnCylinder;
                    FrontCylinder = m_RightCut2.FrontCylinder;
                    GripperCylinder = m_RightCut2.GripperCylinder;
                    velocityCurve = AxisParameter.Instance.Cut4VelocityCurve;
                    break;
            }

            try
            {
                if (!m_LeftCut1.stationInitialize.InitializeDone || !m_LeftCut2.stationInitialize.InitializeDone || !m_RightCut1.stationInitialize.InitializeDone ||
                    !m_RightCut2.stationInitialize.InitializeDone)
                {
                    MessageBox.Show("原点未完成！");
                    return -1;
                }
            }
            catch { return -1; }
            Global.IsLocating = true;
            Task.Factory.StartNew(() =>
            {
                try
                {
                    var step1 = 0;
                    Stopwatch stopwatch = new Stopwatch();
                    stopwatch.Start();
                    while (true)
                    {
                        if (!Global.IsLocating) return -1;
                        switch (step1)
                        {
                            case 0://剪切轴到预剪位
                                if (axis.IsInPosition(Position.Instance.PosCut[Step].Origin))
                                {
                                    step1 = 10;
                                }
                                else
                                {
                                    axis.MoveTo(Position.Instance.PosCut[Step].Origin, velocityCurve);
                                }
                                break;
                            case 10://剪刀预动作位置,判断剪切方式
                                if (!Position.Instance.Caxis[Step].PosTorsion)
                                {
                                    step1 = 30;
                                }
                                else
                                {
                                    step1 = 20;
                                }
                                break;
                            case 20://启动伺服1#扭力剪切控制
                                if (axis.TorqueControl(Position.Instance.PosCut[Step].Move, Config.Instance.PressCut[Step], Position.Instance.PosCutEnd[Step], 200, velocityCurve))
                                {
                                    step1 = 50;
                                }
                                else //扭力止动
                                {
                                    MessageBox.Show("扭力剪切未剪断!");
                                    Global.IsLocating = false;
                                    return 0;
                                }
                                break;
                            case 30://伺服移动到剪切，启动运行
                                if (Position.Instance.PosCutEnd[Step] > Position.Instance.PosCut[Step].Move)
                                {
                                    axis.MoveToExtern(Position.Instance.PosCut[Step].Move, Position.Instance.PosCutEnd[Step], velocityCurve);
                                    step1 = 50;
                                }
                                else
                                {
                                    MessageBox.Show("闭合位需大于缓冲位!");
                                    Global.IsLocating = false;
                                    return 0;
                                }
                                break;
                            case 50://剪刀松开
                                if (axis.IsInPosition(Position.Instance.PosCutEnd[Step]))
                                {
                                    Thread.Sleep(Delay.Instance.ShearTime);
                                    axis.MoveTo(Position.Instance.PosCut[Step].Origin, velocityCurve);
                                    step1 = 60;
                                }
                                break;
                            case 60:
                                if (axis.IsInPosition(Position.Instance.PosCut[Step].Origin))
                                {
                                    step1 = 0;
                                    Global.IsLocating = false;
                                    return 0;
                                }
                                break;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Global.IsLocating = false;
                    LogHelper.Fatal("设备驱动程序异常");
                    return -2;
                }
            }, TaskCreationOptions.AttachedToParent | TaskCreationOptions.LongRunning);

            return 0;
        }

        private void Button25_Click(object sender, EventArgs e)
        {            
            button25.BackColor = System.Drawing.Color.Green;
            CutTestOne(1);
            button25.BackColor = Color.FromKnownColor(System.Drawing.KnownColor.Control);
            LogHelper.Info("2#手动剪切操作");

        }

        private void Button39_Click(object sender, EventArgs e)
        {
            button39.BackColor = System.Drawing.Color.Green;           
            CutTestOne(2);
            button39.BackColor = Color.FromKnownColor(System.Drawing.KnownColor.Control);
            LogHelper.Info("3#手动剪切操作");
        }

        private void Button43_Click(object sender, EventArgs e)
        {
            button43.BackColor = System.Drawing.Color.Green;
            CutTestOne(3);            
            button43.BackColor = Color.FromKnownColor(System.Drawing.KnownColor.Control);
            LogHelper.Info("4#手动剪切操作");

        }

        private int[] Caxis = new int[4];

        private void Button30_MouseDown(object sender, MouseEventArgs e)
        {
            if (Global.IsLocating) return;
            if (Caxis[0] >= Position.Instance.HoleNumber / 4)
            {
                Caxis[0] = 0;
            }          
            Global.IsLocating = true;
            Task.Factory.StartNew(() =>
            {
                double CPos = Position.Instance.C1holes[Caxis[0]] + Position.Instance.C1HolesOffset[Caxis[0]];
                m_LeftC.C1Axis.MoveTo(CPos, AxisParameter.Instance.C1VelocityCurve);
                while (true)
                {
                    if (m_LeftC.C1Axis.IsInPosition(CPos))
                    {                        
                        Global.IsLocating = false;
                        Caxis[0]++;
                        return;
                    }
                    if (!Global.IsLocating) return;
                }
            }, TaskCreationOptions.AttachedToParent | TaskCreationOptions.LongRunning);
        }

        private void Button31_MouseDown(object sender, MouseEventArgs e)
        {
            if (Global.IsLocating) return;        
            if (Caxis[1] >= Position.Instance.HoleNumber / 4)
            {
                Caxis[1] = 0;
            }
            Global.IsLocating = true;
            Task.Factory.StartNew(() =>
            {
                double CPos = Position.Instance.C2holes[Caxis[1]] + Position.Instance.C2HolesOffset[Caxis[1]];
                m_LeftC.C2Axis.MoveTo(CPos, AxisParameter.Instance.C2VelocityCurve);
                while (true)
                {
                    if (m_LeftC.C2Axis.IsInPosition(CPos))
                    {
                        Global.IsLocating = false;
                        Caxis[1]++;
                        return;
                    }
                    if (!Global.IsLocating) return;
                }
            }, TaskCreationOptions.AttachedToParent | TaskCreationOptions.LongRunning);
        }

        private void Button32_MouseDown(object sender, MouseEventArgs e)
        {
            if (Global.IsLocating) return;
            if (Caxis[2] >= Position.Instance.HoleNumber / 4)
            {
                Caxis[2] = 0;
            }          
            Global.IsLocating = true;
            Task.Factory.StartNew(() =>
            {
                double CPos = Position.Instance.C3holes[Caxis[2]] + Position.Instance.C3HolesOffset[Caxis[2]];
                m_LeftC.C3Axis.MoveTo(CPos, AxisParameter.Instance.C3VelocityCurve);
                while (true)
                {
                    if (m_LeftC.C3Axis.IsInPosition(CPos))
                    {
                        Global.IsLocating = false;
                        Caxis[2]++;
                        return;
                    }
                    if (!Global.IsLocating) return;
                }
            }, TaskCreationOptions.AttachedToParent | TaskCreationOptions.LongRunning);
        }

        private void Button33_MouseDown(object sender, MouseEventArgs e)
        {
            if (Global.IsLocating) return;
            if (Caxis[3] >= Position.Instance.HoleNumber / 4)
            {
                Caxis[3] = 0;
            }
            Global.IsLocating = true;
            Task.Factory.StartNew(() =>
            {
                double CPos = Position.Instance.C4holes[Caxis[3]] + Position.Instance.C4HolesOffset[Caxis[3]];
                m_LeftC.C4Axis.MoveTo(CPos, AxisParameter.Instance.C4VelocityCurve);
                while (true)
                {
                    if (m_LeftC.C4Axis.IsInPosition(CPos))
                    {                      
                        Global.IsLocating = false;
                        Caxis[3]++;
                        return;
                    }
                    if (!Global.IsLocating) return;
                }
            }, TaskCreationOptions.AttachedToParent | TaskCreationOptions.LongRunning);
        }

        private void btnPush1Axis_Click(object sender, EventArgs e)
        {
            if (Global.IsLocating) return;
            Global.IsLocating = true;
            Task.Factory.StartNew(() =>
            {
                double PPos = 0;
                if (!m_LeftC.Push1Axis.IsInPosition(Position.Instance.PosPush[0].Origin))
                {
                    PPos = Position.Instance.PosPush[0].Origin;
                    m_LeftC.Push1Axis.MoveTo(PPos, AxisParameter.Instance.Push1VelocityCurve);
                    lalPush1Axis.Text = "原点";
                }
                else
                {
                    PPos = Position.Instance.PosPush[0].Move + Position.Instance.P1HolesOffset[Convert.ToInt32(lblC1Number.Text) - 1];
                    m_LeftC.Push1Axis.MoveTo(PPos, AxisParameter.Instance.Push1VelocityCurve);
                    lalPush1Axis.Text = "动点";
                }
                while (true)
                {
                    if (m_LeftC.Push1Axis.IsInPosition(PPos) && m_LeftC.Push1Axis.CurrentSpeed == 0)
                    {
                        Global.IsLocating = false;
                        return;
                    }
                }
            }, TaskCreationOptions.AttachedToParent | TaskCreationOptions.LongRunning);
        }

        private void btnPush2Axis_Click(object sender, EventArgs e)
        {
            if (Global.IsLocating) return;
            Global.IsLocating = true;
            Task.Factory.StartNew(() =>
            {
                double PPos = 0;
                if (!m_LeftC.Push2Axis.IsInPosition(Position.Instance.PosPush[1].Origin))
                {
                    PPos = Position.Instance.PosPush[1].Origin;
                    m_LeftC.Push2Axis.MoveTo(PPos, AxisParameter.Instance.Push2VelocityCurve);
                    lalPush2Axis.Text = "原点";
                }
                else
                {
                    PPos = Position.Instance.PosPush[1].Move + Position.Instance.P2HolesOffset[Convert.ToInt32(lblC2Number.Text) - 1];
                    m_LeftC.Push2Axis.MoveTo(PPos, AxisParameter.Instance.Push2VelocityCurve);
                    lalPush2Axis.Text = "动点";
                }
                while (true)
                {
                    if (m_LeftC.Push2Axis.IsInPosition(PPos) && m_LeftC.Push2Axis.CurrentSpeed == 0)
                    {
                        Global.IsLocating = false;
                        return;
                    }
                }
            }, TaskCreationOptions.AttachedToParent | TaskCreationOptions.LongRunning);
        }

        private void btnPush3Axis_Click(object sender, EventArgs e)
        {
            if (Global.IsLocating) return;
            Global.IsLocating = true;
            Task.Factory.StartNew(() =>
            {
                double PPos = 0;
                if (!m_LeftC.Push3Axis.IsInPosition(Position.Instance.PosPush[2].Origin))
                {
                    PPos = Position.Instance.PosPush[2].Origin;
                    m_LeftC.Push3Axis.MoveTo(PPos, AxisParameter.Instance.Push3VelocityCurve);
                    lalPush3Axis.Text = "原点";
                }
                else
                {
                    PPos = Position.Instance.PosPush[2].Move + Position.Instance.P3HolesOffset[Convert.ToInt32(lblC3Number.Text) - 1];
                    m_LeftC.Push3Axis.MoveTo(PPos, AxisParameter.Instance.Push3VelocityCurve);
                    lalPush3Axis.Text = "动点";
                }
                while (true)
                {
                    if (m_LeftC.Push3Axis.IsInPosition(PPos) && m_LeftC.Push3Axis.CurrentSpeed == 0)
                    {
                        Global.IsLocating = false;
                        return;
                    }
                }
            }, TaskCreationOptions.AttachedToParent | TaskCreationOptions.LongRunning);
        }

        private void btnPush4Axis_Click(object sender, EventArgs e)
        {
            if (Global.IsLocating) return;
            Global.IsLocating = true;
            Task.Factory.StartNew(() =>
            {
                double PPos = 0;
                if (!m_LeftC.Push4Axis.IsInPosition(Position.Instance.PosPush[3].Origin))
                {
                    PPos = Position.Instance.PosPush[3].Origin;
                    m_LeftC.Push4Axis.MoveTo(PPos, AxisParameter.Instance.Push4VelocityCurve);
                    lalPush4Axis.Text = "原点";
                }
                else
                {
                    PPos = Position.Instance.PosPush[3].Move + Position.Instance.P4HolesOffset[Convert.ToInt32(lblC4Number.Text) - 1];
                    m_LeftC.Push4Axis.MoveTo(PPos, AxisParameter.Instance.Push4VelocityCurve);
                    lalPush4Axis.Text = "动点";
                }
                while (true)
                {
                    if (m_LeftC.Push4Axis.IsInPosition(PPos) && m_LeftC.Push4Axis.CurrentSpeed == 0)
                    {
                        Global.IsLocating = false;
                        return;
                    }
                }
            }, TaskCreationOptions.AttachedToParent | TaskCreationOptions.LongRunning);
        }
    }
}
