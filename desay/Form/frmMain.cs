using System;
using System.Collections.Generic;
using System.Device;
using System.Diagnostics;
using System.Drawing;
using System.Enginee;
using System.Framework;
using System.Interfaces;
using System.Threading;
using System.ToolKit;
using System.ToolKit.Helper;
using System.Tray;
using System.Windows.Forms;
using System.Threading.Tasks;
using HslCommunication.ModBus;
using HslCommunication;
using License;
using HslCommunication.Serial;

namespace desay
{
    public partial class frmMain : Form
    {
        #region 字段
        //发送者    
        private AlarmType SpliceIsAlarm, BufferIsAlarm, FeederIsAlarm, MoveIsAlarm, LeftCIsAlarm
            , LeftCut1IsAlarm, LeftCut2IsAlarm, RightCut1IsAlarm, RightCut2IsAlarm,
            PlateformIsAlarm, StoringIsAlarm, MachineIsAlarm;

        private External m_External;
        private MachineOperate MachineOperation;

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
        private ManualResetEventSlim closeSafe = new ManualResetEventSlim();
        private LightButton StartButton, StopButton, PauseButton, ClearAlarmButton, GoHome;
        private LightButton ManualUpButton;
        private EventButton EstopButton;
        private LayerLight layerLight;
        private bool ManualAutoMode;
        private Thread threadMachineRun = null;
        private Thread threadAlarmCheck = null;
        private Thread threadStatusCheck = null;
        private Thread threadReshing = null;
        private frmTeach m_FrmTeach;    //教导
        /// <summary>
        /// 加密狗
        /// </summary>
        private Thread threadLicenseCheck = null;
        private Encryption hasp;
        private Registered HaspRegistered;
        private bool LicenseSheild = false;  //(true - 屏蔽，false - 不屏蔽)

        /// <summary>
        /// 模拟量用
        /// </summary>
        private ModbusRtu TisRtuClient = null;

        public bool uploadEnable = false;

        private event Action<string> LoadingMessage;
        private event Action<UserLevel> UserLevelChangeEvent;

        string LeftPV1, LeftPV2, LeftPV3, LeftPV4;
        string RightPV1, RightPV2, RightPV3, RightPV4;

        //温控器获取数据
        bool[] WriteSign = new bool[3];

        List<string> AlarmsName = new List<string>();

        Stopwatch watchCT1, watchCT, closeDoorTime;

        GTA2 TempGTA2;
        /// <summary>
        /// 模拟量通讯状态
        /// </summary>
        private bool AnalogConnect;

        dTrayPanel ConeTrayDisplay1;//托盘显示
        dTrayPanel ConeTrayDisplay2;//托盘显示    
        dTrayPanel ConeTrayDisplay3;//托盘显示
        dTrayPanel ConeTrayDisplay4;//托盘显示      
        uTrayPanel LeftConeTrayDisplay1;
        uTrayPanel LeftConeTrayDisplay2;
        uTrayPanel LeftConeTrayDisplay3;
        uTrayPanel LeftConeTrayDisplay4;
        #endregion

        #region 托盘显示
        //初始化托盘参数
        private void InitTray()
        {
            try
            {
                Global.BigTray = TrayFactory.GetTrayFactory(Config.Instance.BigPlateID);
                Global.SmallTray = TrayFactory.GetTrayFactory(Config.Instance.YoungPlateID);
                panel1.Controls.Clear();
                panel2.Controls.Clear();
                panel3.Controls.Clear();
                panel4.Controls.Clear();
                Marking.ClearMemory();
                if (Global.SmallTray != null)
                {
                    if (Global.BigTray != null)
                    {
                        LeftConeTrayDisplay1 = new uTrayPanel();
                        LeftConeTrayDisplay1.AutoSize = true;
                        LeftConeTrayDisplay1.SetTrayObj(Global.SmallTray, Color.Gray);
                        LeftConeTrayDisplay1.Dock = DockStyle.Fill;
                        Global.SmallTray.updateColor += LeftConeTrayDisplay1.UpdateColor;
                        LeftConeTrayDisplay2 = new uTrayPanel();
                        LeftConeTrayDisplay2.AutoSize = true;
                        LeftConeTrayDisplay2.SetTrayObj(Global.SmallTray, Color.Gray);
                        LeftConeTrayDisplay2.Dock = DockStyle.Fill;
                        Global.SmallTray.updateColor += LeftConeTrayDisplay2.UpdateColor;
                        LeftConeTrayDisplay3 = new uTrayPanel();
                        LeftConeTrayDisplay3.AutoSize = true;
                        LeftConeTrayDisplay3.SetTrayObj(Global.SmallTray, Color.Gray);
                        LeftConeTrayDisplay3.Dock = DockStyle.Fill;
                        Global.SmallTray.updateColor += LeftConeTrayDisplay3.UpdateColor;
                        LeftConeTrayDisplay4 = new uTrayPanel();
                        LeftConeTrayDisplay4.AutoSize = true;
                        LeftConeTrayDisplay4.SetTrayObj(Global.SmallTray, Color.Gray);
                        LeftConeTrayDisplay4.Dock = DockStyle.Fill;
                        Global.SmallTray.updateColor += LeftConeTrayDisplay4.UpdateColor;
                        panel1.Controls.Add(LeftConeTrayDisplay1);
                        panel2.Controls.Add(LeftConeTrayDisplay2);
                        panel3.Controls.Add(LeftConeTrayDisplay3);
                        panel4.Controls.Add(LeftConeTrayDisplay4);
                    }
                }
                #region 大托盘
                palBigTray1.Controls.Clear();
                palBigTray2.Controls.Clear();
                palBigTray3.Controls.Clear();
                palBigTray4.Controls.Clear();
                Marking.ClearMemory();
                if (Global.BigTray != null)
                {
                    if (Config.Instance.LeftTarySet.Length != Global.BigTray.Column * Global.BigTray.Row)
                    {
                        Config.Instance.LeftTarySet = new int[Global.BigTray.Column * Global.BigTray.Row];
                        for (int i = 0; i < Global.BigTray.Column * Global.BigTray.Row; i++)
                        {
                            Config.Instance.LeftTarySet[i] = i + 1;
                        }
                    }
                    if (Config.Instance.LeftTarySet1.Length != Global.BigTray.Column * Global.BigTray.Row)
                    {
                        Config.Instance.LeftTarySet1 = new int[Global.BigTray.Column * Global.BigTray.Row];
                        for (int i = 0; i < Global.BigTray.Column * Global.BigTray.Row; i++)
                        {
                            Config.Instance.LeftTarySet1[i] = Global.BigTray.Column * Global.BigTray.Row + i + 1;
                        }
                    }
                    if (Config.Instance.RightTarySet.Length != Global.BigTray.Column * Global.BigTray.Row)
                    {
                        Config.Instance.RightTarySet = new int[Global.BigTray.Column * Global.BigTray.Row];
                        for (int i = 0; i < Global.BigTray.Column * Global.BigTray.Row; i++)
                        {
                            Config.Instance.RightTarySet[i] = Global.BigTray.Column * Global.BigTray.Row * 2 + i + 1;
                        }
                    }
                    if (Config.Instance.RightTarySet1.Length != Global.BigTray.Column * Global.BigTray.Row)
                    {
                        Config.Instance.RightTarySet1 = new int[Global.BigTray.Column * Global.BigTray.Row];
                        for (int i = 0; i < Global.BigTray.Column * Global.BigTray.Row; i++)
                        {
                            Config.Instance.RightTarySet1[i] = Global.BigTray.Column * Global.BigTray.Row * 3 + i + 1;
                        }
                    }
                    ConeTrayDisplay1 = new dTrayPanel();
                    ConeTrayDisplay1.AutoSize = true;
                    ConeTrayDisplay1.SetTrayObj(Global.BigTray, Color.Gray, Config.Instance.LeftTarySet);
                    ConeTrayDisplay2 = new dTrayPanel();
                    ConeTrayDisplay2.AutoSize = true;
                    ConeTrayDisplay2.SetTrayObj(Global.BigTray, Color.Gray, Config.Instance.LeftTarySet1);
                    ConeTrayDisplay1.Dock = DockStyle.Fill;
                    ConeTrayDisplay2.Dock = DockStyle.Fill;
                    Global.BigTray.updateColor += ConeTrayDisplay1.UpdateColor;
                    Global.BigTray.updateColor += ConeTrayDisplay2.UpdateColor;
                    palBigTray1.Controls.Add(ConeTrayDisplay1);
                    palBigTray2.Controls.Add(ConeTrayDisplay2);

                    ConeTrayDisplay3 = new dTrayPanel();
                    ConeTrayDisplay3.AutoSize = true;
                    ConeTrayDisplay3.SetTrayObj(Global.BigTray, Color.Gray, Config.Instance.RightTarySet);
                    ConeTrayDisplay4 = new dTrayPanel();
                    ConeTrayDisplay4.AutoSize = true;
                    ConeTrayDisplay4.SetTrayObj(Global.BigTray, Color.Gray, Config.Instance.RightTarySet1);
                    ConeTrayDisplay3.Dock = DockStyle.Fill;
                    ConeTrayDisplay4.Dock = DockStyle.Fill;
                    Global.BigTray.updateColor += ConeTrayDisplay3.UpdateColor;
                    Global.BigTray.updateColor += ConeTrayDisplay4.UpdateColor;
                    palBigTray3.Controls.Add(ConeTrayDisplay3);
                    palBigTray4.Controls.Add(ConeTrayDisplay4);
                }
                #endregion
            }
            catch { LogHelper.Debug("托盘初始化失败"); }

            try
            {
                foreach (Control item in tbp.Controls)
                {
                    if (item is Button)
                    {
                        Button objControl = (Button)item;
                        objControl.Dispose();
                    }
                }
                tbp.Controls.Clear();
                tbp.RowStyles.Clear();
                tbp.ColumnStyles.Clear();
                Marking.ClearMemory();
                tbp.ColumnCount = 4;
                for (int i = 0; i < 4; i++)
                {
                    tbp.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
                }
                tbp.RowCount = Position.Instance.HoleNumber / 4;
                for (int i = 0; i < Position.Instance.HoleNumber / 4; i++)
                {
                    tbp.RowStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
                }
                for (int i = 0; i < Position.Instance.HoleNumber / 4; i++)
                {
                    Button btn = new Button();
                    Button btn1 = new Button();
                    Button btn2 = new Button();
                    Button btn3 = new Button();

                    btn.Name = "leftOne," + i.ToString();
                    btn1.Name = "leftTwo," + i.ToString();
                    btn2.Name = "leftThree," + i.ToString();
                    btn3.Name = "leftFour," + i.ToString();

                    btn.Dock = DockStyle.Top;
                    btn1.Dock = DockStyle.Top;
                    btn2.Dock = DockStyle.Top;
                    btn3.Dock = DockStyle.Top;

                    btn.Anchor = AnchorStyles.None;
                    btn1.Anchor = AnchorStyles.None;
                    btn2.Anchor = AnchorStyles.None;
                    btn3.Anchor = AnchorStyles.None;

                    btn.Text = Config.Instance.LeftTarySet[i].ToString();
                    btn1.Text = Config.Instance.LeftTarySet1[i].ToString();
                    btn2.Text = Config.Instance.RightTarySet[i].ToString();
                    btn3.Text = Config.Instance.RightTarySet1[i].ToString();

                    btn.BackColor = Position.Instance.C1axisSheild[i] ?  Color.Gray : Color.Transparent;
                    btn1.BackColor = Position.Instance.C2axisSheild[i] ? Color.Gray : Color.Transparent;
                    btn2.BackColor = Position.Instance.C3axisSheild[i] ? Color.Gray : Color.Transparent;
                    btn3.BackColor = Position.Instance.C4axisSheild[i] ? Color.Gray : Color.Transparent;
                                       
                    tbp.Controls.Add(btn, 3, i);
                    tbp.Controls.Add(btn1, 2, i);
                    tbp.Controls.Add(btn2, 1, i);
                    tbp.Controls.Add(btn3, 0, i);

                    btn.Click += new EventHandler(label_Click);
                    btn1.Click += new EventHandler(label_Click);
                    btn2.Click += new EventHandler(label_Click);
                    btn3.Click += new EventHandler(label_Click);
                }
            }
            catch (Exception ex)
            {
                AppendText(ex.ToString());
            }
        }

        private void label_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            try
            {
                if (btn.Name.Contains("leftOne"))
                {
                    var i = btn.Name.Split(',');
                    int i1 = Convert.ToInt32(i[1]);
                    Position.Instance.C1axisSheild[i1] = !Position.Instance.C1axisSheild[i1];
                    btn.BackColor = Position.Instance.C1axisSheild[i1] ? Color.Gray : Color.Transparent; 
                }
                if (btn.Name.Contains("leftTwo"))
                {
                    var i = btn.Name.Split(',');
                    int i1 = Convert.ToInt32(i[1]);
                    Position.Instance.C2axisSheild[i1] = !Position.Instance.C2axisSheild[i1];
                    btn.BackColor = Position.Instance.C2axisSheild[i1] ? Color.Gray : Color.Transparent; 
                }
                if (btn.Name.Contains("leftThree"))
                {
                    var i = btn.Name.Split(',');
                    int i1 = Convert.ToInt32(i[1]);
                    Position.Instance.C3axisSheild[i1] = !Position.Instance.C3axisSheild[i1];
                    btn.BackColor = Position.Instance.C3axisSheild[i1] ? Color.Gray : Color.Transparent; 
                }
                if (btn.Name.Contains("leftFour"))
                {
                    var i = btn.Name.Split(',');
                    int i1 = Convert.ToInt32(i[1]);
                    Position.Instance.C4axisSheild[i1] = !Position.Instance.C4axisSheild[i1];
                    btn.BackColor = Position.Instance.C4axisSheild[i1] ? Color.Gray : Color.Transparent; 
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("请正确输入数字");
            }
        }

        #endregion

        #region 构造函数

        public frmMain()
        {
            InitializeComponent();
            //SetStyle(ControlStyles.UserPaint, true);
            //SetStyle(ControlStyles.AllPaintingInWmPaint, true); // 禁止擦除背景.
            //SetStyle(ControlStyles.DoubleBuffer, true); // 双缓冲

            try
            {
                m_External = new External();
                hasp = new Encryption();
                hasp.ShowWindows += ShowWindows;
                hasp.MachineName = "0A02";
                HaspRegistered = new Registered(hasp);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }


        private void ShowWindows(object sender, EventArgs e)
        {
            try           
            {
                if (!HaspRegistered.Created)
                {
                    HaspRegistered.ShowDialog();
                }
            }
            catch(Exception ex)
            {
                throw(ex);
            }
        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x02000000;
                return cp;
            }
        }

        #endregion

        #region 用户权限

        private void UserLevelChange(UserLevel level)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<UserLevel>(UserLevelChange), level);
            }
            else
            {
                switch (level)
                {
                    case UserLevel.操作员:
                        for (int i = 0; i < 10; i++)
                        {
                            Config.Instance.UserL[i] = Config.Instance.userLevels[i] == UserLevel.操作员 ? true : false;
                        }
                        Config.Instance.UserL[10] = false;
                        Config.Instance.UserL[11] = false;
                        Config.Instance.UserL[12] = false;
                        btnOpenfile.Enabled = false;
                        btnSheild.Visible = false;
                        break;
                    case UserLevel.工程师:
                        for (int i = 0; i < 10; i++)
                        {
                            Config.Instance.UserL[i] = Config.Instance.userLevels[i] != UserLevel.工程师 ? true : false;
                        }
                        Config.Instance.UserL[10] = false;
                        Config.Instance.UserL[11] = false;
                        Config.Instance.UserL[12] = false;
                        btnOpenfile.Enabled = true;
                        btnSheild.Visible = false;
                        break;
                    case UserLevel.设计者:
                        for (int i = 0; i < 10; i++)
                        {
                            Config.Instance.UserL[i] = Config.Instance.userLevels[i] != UserLevel.设计者 ? true : false;
                        }
                        Config.Instance.UserL[10] = false;
                        Config.Instance.UserL[11] = false;
                        Config.Instance.UserL[12] = false;
                        btnOpenfile.Enabled = true;
                        btnSheild.Visible = false;
                        break;
                    case UserLevel.设备厂商:
                        for (int i = 0; i < 13; i++)
                        {
                            Config.Instance.UserL[i] = true;
                        }
                        btnOpenfile.Enabled = true;
                        btnSheild.Visible = true;
                        break;
                    default:
                        for (int i = 0; i < 13; i++)
                        {
                            Config.Instance.UserL[i] = false;
                        }
                        btnSheild.Visible = false;
                        break;
                }
            }
        }

        public void OnUserLevelChange(UserLevel level)
        {
            UserLevelChangeEvent?.Invoke(level);
        }

        #endregion

        #region 窗体加载

        private void frmMain_Load(object sender, EventArgs e)
        {
            #region 开机界面加载
            UserLevelChangeEvent += UserLevelChange;
            Config.Instance.userLevel = UserLevel.操作员;
            OnUserLevelChange(Config.Instance.userLevel);
            cmbOpation.SelectedIndex = 0;

            new Thread(new ThreadStart(() =>
            {
                frmStarting loading = new frmStarting(8);
                LoadingMessage += new Action<string>(loading.ShowMessage);
                loading.ShowDialog();
            })).Start();
            Thread.Sleep(2000);
            #endregion

            #region 加载总线控制卡

            LoadingMessage("加载总线控制卡信息");
            try
            {
                IoPoints.ApsController.Initialize();
                if (!IoPoints.ApsController.LoadParamFromFile(AppConfig.ConfigCardEniName, AppConfig.ConfigCardIniName, AppConfig.ConfigAxisIniName))
                {
                    AppendText("配置文件失败:请将轴卡的配置文件" + ".emi " + "拷贝到当前型号的路径下" + AppConfig.ConfigCardEniName);
                    AppendText("配置文件失败:请将轴卡的配置文件" + ".ini " + "拷贝到当前型号的路径下" + AppConfig.ConfigCardIniName);
                   //AppendText("配置文件失败:请将轴卡的配置文件" + ".ini " + "拷贝到当前型号的路径下" + AppConfig.ConfigAxisIniName);
                }
                IoPoints.ApsController.SetEquipmentParam(AxisParameter.Instance.SoftNlimit, AxisParameter.Instance.SoftPlimit);
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex.Message);
                AppendText("板卡初始化失败！请检查硬件！");
                timer1.Enabled = false;
            }
            #endregion

            #region 轴信息

            LoadingMessage("加载轴控制资源");

            var Yaxis = new ServoAxis(IoPoints.ApsController)
            {
                NoId = 0,
                Transmission = AxisParameter.Instance.YTransParams,
                HomeSped = AxisParameter.Instance.YHomeVelocityCurve,

                Speed = 10,
                HomeMode = 10,
                HomeDir = 0,
                Name = "Y轴"
            };
            Yaxis._condition = new Func<bool>(() => { return true; });

            var Xaxis = new ServoAxis(IoPoints.ApsController)
            {
                NoId = 1,
                Transmission = AxisParameter.Instance.XTransParams,
                HomeSped = AxisParameter.Instance.XHomeVelocityCurve,
                Speed = 11,
                HomeMode = 10,
                HomeDir = 1,
                Name = "X轴"
            };
            Xaxis._condition = new Func<bool>(() => { return true; });
            var Zaxis = new ServoAxis(IoPoints.ApsController)
            {
                NoId = 2,
                Transmission = AxisParameter.Instance.ZTransParams,
                HomeSped = AxisParameter.Instance.ZHomeVelocityCurve,

                Speed = 10,
                HomeMode = 10,
                HomeDir = 1,
                Name = "Z轴"
            };
            Zaxis._condition = new Func<bool>(() => { return true; });
            var LeftC1Axis = new ServoAxis(IoPoints.ApsController)
            {
                NoId = 3,
                Transmission = AxisParameter.Instance.C1TransParams,
                HomeSped = AxisParameter.Instance.C1HomeVelocityCurve,
                Speed = 10,
                HomeMode = 2,
                HomeDir = 1,
                Name = "C1#轴"
            };
            LeftC1Axis._condition = new Func<bool>(() => { return true; });
            var LeftC2Axis = new ServoAxis(IoPoints.ApsController)
            {
                NoId = 4,
                Transmission = AxisParameter.Instance.C2TransParams,
                HomeSped = AxisParameter.Instance.C2HomeVelocityCurve,
                Speed = 10,
                HomeMode = 2,
                HomeDir = 1,
                Name = "C2#轴"
            };
            LeftC2Axis._condition = new Func<bool>(() => { return true; });
            var LeftC3Axis = new ServoAxis(IoPoints.ApsController)
            {
                NoId = 5,
                Transmission = AxisParameter.Instance.C3TransParams,
                HomeSped = AxisParameter.Instance.C3HomeVelocityCurve,
                Speed = 10,
                HomeMode = 2,
                HomeDir = 1,
                Name = "C3#轴"
            };
            LeftC3Axis._condition = new Func<bool>(() => { return true; });
            var LeftC4Axis = new ServoAxis(IoPoints.ApsController)
            {
                NoId = 6,
                Transmission = AxisParameter.Instance.C4TransParams,
                HomeSped = AxisParameter.Instance.C4HomeVelocityCurve,
                Speed = 10,
                HomeMode = 2,
                HomeDir = 1,
                Name = "C4#轴"
            };
            LeftC4Axis._condition = new Func<bool>(() => { return true; });
            var LeftPuch1Axis = new ServoAxis(IoPoints.ApsController)
            {
                NoId = 7,
                Transmission = AxisParameter.Instance.Push1TransParams,
                HomeSped = AxisParameter.Instance.Push1HomeVelocityCurve,
                Speed = 10,
                HomeMode = 2,
                HomeDir = 1,
                Name = "P1#轴"
            };
            LeftPuch1Axis._condition = new Func<bool>(() => { return true; });
            var LeftPuch2Axis = new ServoAxis(IoPoints.ApsController)
            {
                NoId = 8,
                Transmission = AxisParameter.Instance.Push2TransParams,
                HomeSped = AxisParameter.Instance.Push2HomeVelocityCurve,
                Speed = 10,
                HomeMode = 2,
                HomeDir = 1,
                Name = "P2#轴"
            };
            LeftPuch2Axis._condition = new Func<bool>(() => { return true; });
            var LeftPuch3Axis = new ServoAxis(IoPoints.ApsController)
            {
                NoId = 9,
                Transmission = AxisParameter.Instance.Push3TransParams,
                HomeSped = AxisParameter.Instance.Push3HomeVelocityCurve,
                Speed = 10,
                HomeMode = 2,
                HomeDir = 1,
                Name = "P3#轴"
            };
            LeftPuch3Axis._condition = new Func<bool>(() => { return true; });
            var LeftPuch4Axis = new ServoAxis(IoPoints.ApsController)
            {
                NoId = 10,
                Transmission = AxisParameter.Instance.Push4TransParams,
                HomeSped = AxisParameter.Instance.Push4HomeVelocityCurve,
                Speed = 10,
                HomeMode = 2,
                HomeDir = 1,
                Name = "P4#轴"
            };
            LeftPuch4Axis._condition = new Func<bool>(() => { return true; });
            var LeftCutAxis1 = new StepAxis(IoPoints.ApsController)
            {
                NoId = 11,
                Transmission = AxisParameter.Instance.Cut1TransParams,
                HomeSped = AxisParameter.Instance.Cut1HomeVelocityCurve,

                Speed = 10,
                HomeMode = 10,
                HomeDir = 1,
                Name = "剪切1#轴"
            };
            LeftCutAxis1._condition = new Func<bool>(() => { return true; });
            var LeftCutAxis2 = new StepAxis(IoPoints.ApsController)
            {
                NoId = 12,
                Transmission = AxisParameter.Instance.Cut2TransParams,
                HomeSped = AxisParameter.Instance.Cut2HomeVelocityCurve,

                Speed = 10,
                HomeMode = 10,
                HomeDir = 1,
                Name = "剪切2#轴"
            };
            LeftCutAxis2._condition = new Func<bool>(() => { return true; });
            var LeftCutAxis3 = new StepAxis(IoPoints.ApsController)
            {
                NoId = 13,
                Transmission = AxisParameter.Instance.Cut3TransParams,
                HomeSped = AxisParameter.Instance.Cut3HomeVelocityCurve,

                Speed = 10,
                HomeMode = 10,
                HomeDir = 1,
                Name = "剪切3#轴"
            };
            LeftCutAxis3._condition = new Func<bool>(() => { return true; });
            var LeftCutAxis4 = new StepAxis(IoPoints.ApsController)
            {
                NoId = 14,
                Transmission = AxisParameter.Instance.Cut4TransParams,
                HomeSped = AxisParameter.Instance.Cut4HomeVelocityCurve,
                Speed = 10,
                HomeMode = 10,
                HomeDir = 1,
                Name = "剪切4#轴"
            };
            LeftCutAxis4._condition = new Func<bool>(() => { return true; });
            var Maxis = new StepAxis(IoPoints.ApsController)
            {
                NoId = 15,
                Transmission = AxisParameter.Instance.MTransParams,
                HomeSped = AxisParameter.Instance.MHomeVelocityCurve,
                Speed = 10,
                HomeMode = 7,
                HomeDir = 1,
                Name = "M轴"
            };
            Maxis._condition = new Func<bool>(() => { return IoPoints.T2IN18.Value && IoPoints.T2IN19.Value; });

            #endregion

            #region 气缸信息

            LoadingMessage("加载气缸资源");

            var MoveDownCylinder = new SingleCylinder(IoPoints.T1IN14, IoPoints.T1IN15, IoPoints.T1DO6)
            {
                Name = "移料上下气缸",
                Delay = Delay.Instance.MoveDownCylinderDelay,
                Condition = new CylinderCondition(() => { return true; }, () => { return true; }) { External = m_External }
            };
            var MoveLeftCylinder = new SingleCylinder2(IoPoints.T1IN10, IoPoints.T1IN12, IoPoints.T1IN11, IoPoints.T1IN13, IoPoints.T1DO5)
            {
                Name = "移料左右气缸",
                Delay = Delay.Instance.MoveLeftCylinderDelay,
                Condition = new CylinderCondition(() => { return MoveDownCylinder.OutOriginStatus; },
                () => { return MoveDownCylinder.OutOriginStatus; }) { External = m_External }
            };
            var MoveGripperCylinder = new SingleCylinder(IoPoints.T3IN31, IoPoints.T3IN31, IoPoints.T1DO7)
            {
                Name = "移料夹子气缸",
                Delay = Delay.Instance.MoveGripperCylinderDelay,
                Condition = new CylinderCondition(() => { return true; }, () => { return true; }) { External = m_External }
            };
            MoveGripperCylinder.Condition.NoMoveShield = true;
            MoveGripperCylinder.Condition.NoOriginShield = true;

            var FeederCylinder = new SingleCylinder(IoPoints.T1IN22, IoPoints.T1IN23, IoPoints.T1DO11)
            {
                Name = "进料气缸",
                Delay = Delay.Instance.FeederCylinderDelay,
                Condition = new CylinderCondition(() => { return true; }, () => { return true; }) { External = m_External }
            };

            var CutwasteCylinder1 = new SingleCylinder(IoPoints.T1IN4, IoPoints.T1IN5, IoPoints.T1DO8)
            {
                Name = "碎料气缸",
                Delay = Delay.Instance.CutwasteCylinder1Delay,
                Condition = new CylinderCondition(() => { return true; }, () => { return true; })
                {
                    External = m_External,
                    NoMoveShield = true
                }
            };
            var CutwasteCylinder2 = new SingleCylinder(IoPoints.T1IN6, IoPoints.T1IN7, IoPoints.T1DO9)
            {
                Name = "排料气缸",
                Delay = Delay.Instance.CutwasteCylinder2Delay,
                Condition = new CylinderCondition(() => { return true; }, () => { return true; })
                {
                    External = m_External,
                    NoOriginShield = true,
                    NoMoveShield = true
                }
            };
            var CutwasteCylinder3 = new SingleCylinder(IoPoints.T1IN8, IoPoints.T1IN9, IoPoints.T1DO10)
            {
                Name = "碎料盖子气缸",
                Delay = Delay.Instance.CutwasteCylinder3Delay,
                Condition = new CylinderCondition(() => { return true; }, () => { return true; }) { External = m_External }
            };
            var LeftCut1FrontCylinder = new SingleCylinder(IoPoints.T2IN0, IoPoints.T2IN1, IoPoints.T1DO16)
            {
                Name = "1#剪切前后气缸",
                Delay = Delay.Instance.LeftCut1FrontCylinderDelay,
                Condition = new CylinderCondition(() => { return true; }, () => { return true; }) { External = m_External }
            };
            var LeftCut1OverturnCylinder = new SingleCylinder(IoPoints.T1IN24, IoPoints.T1IN25, IoPoints.T1DO12)
            {
                Name = "1#剪切翻转气缸",
                Delay = Delay.Instance.LeftCut1OverturnCylinderDelay,
                Condition = new CylinderCondition(() => { return LeftCut1FrontCylinder.OutOriginStatus; }, () => { return LeftCut1FrontCylinder.OutOriginStatus; }) { External = m_External }
            };
            var LeftCut1GripperCylinder = new SingleCylinder(IoPoints.T3IN31, IoPoints.T2IN8, IoPoints.T1DO20)
            {
                Name = "1#剪切夹子气缸",
                Delay = Delay.Instance.LeftCut1GripperCylinderDelay,
                Condition = new CylinderCondition(() => { return true; }, () => { return true; }) { External = m_External }
            };
            LeftCut1GripperCylinder.Condition.NoMoveShield = true;
            LeftCut1GripperCylinder.Condition.NoOriginShield = true;
            var LeftCut2FrontCylinder = new SingleCylinder(IoPoints.T2IN2, IoPoints.T2IN3, IoPoints.T1DO17)
            {
                Name = "2#剪切前后气缸",
                Delay = Delay.Instance.LeftCut2FrontCylinderDelay,
                Condition = new CylinderCondition(() => { return true; }, () => { return true; }) { External = m_External }
            };
            var LeftCut2OverturnCylinder = new SingleCylinder(IoPoints.T1IN26, IoPoints.T1IN27, IoPoints.T1DO13)
            {
                Name = "2#剪切翻转气缸",
                Delay = Delay.Instance.LeftCut2OverturnCylinderDelay,
                Condition = new CylinderCondition(() => { return LeftCut2FrontCylinder.OutOriginStatus; }, () => { return LeftCut2FrontCylinder.OutOriginStatus; }) { External = m_External }
            };
            var LeftCut2GripperCylinder = new SingleCylinder(IoPoints.T3IN31, IoPoints.T2IN9, IoPoints.T1DO21)
            {
                Name = "2#剪切夹子气缸",
                Delay = Delay.Instance.LeftCut2GripperCylinderDelay,
                Condition = new CylinderCondition(() => { return true; }, () => { return true; }) { External = m_External }
            };
            LeftCut2GripperCylinder.Condition.NoMoveShield = true;
            LeftCut2GripperCylinder.Condition.NoOriginShield = true;
            var RightCut1FrontCylinder = new SingleCylinder(IoPoints.T2IN4, IoPoints.T2IN5, IoPoints.T1DO18)
            {
                Name = "3#剪切前后气缸",
                Delay = Delay.Instance.LeftCut3FrontCylinderDelay,
                Condition = new CylinderCondition(() => { return true; }, () => { return true; }) { External = m_External }
            };
            var RightCut1OverturnCylinder = new SingleCylinder(IoPoints.T1IN28, IoPoints.T1IN29, IoPoints.T1DO14)
            {
                Name = "3#剪切翻转气缸",
                Delay = Delay.Instance.LeftCut3OverturnCylinderDelay,
                Condition = new CylinderCondition(() => { return RightCut1FrontCylinder.OutOriginStatus; }, () => { return RightCut1FrontCylinder.OutOriginStatus; }) { External = m_External }
            };
            var RightCut1GripperCylinder = new SingleCylinder(IoPoints.T3IN31, IoPoints.T2IN10, IoPoints.T1DO22)
            {
                Name = "3#剪切夹子气缸",
                Delay = Delay.Instance.LeftCut3GripperCylinderDelay,
                Condition = new CylinderCondition(() => { return true; }, () => { return true; }) { External = m_External }
            };
            RightCut1GripperCylinder.Condition.NoMoveShield = true;
            RightCut1GripperCylinder.Condition.NoOriginShield = true;
            var RightCut2FrontCylinder = new SingleCylinder(IoPoints.T2IN6, IoPoints.T2IN7, IoPoints.T1DO19)
            {
                Name = "4#剪切前后气缸",
                Delay = Delay.Instance.LeftCut4FrontCylinderDelay,
                Condition = new CylinderCondition(() => { return true; }, () => { return true; }) { External = m_External }
            };
            var RightCut2OverturnCylinder = new SingleCylinder(IoPoints.T1IN30, IoPoints.T1IN31, IoPoints.T1DO15)
            {
                Name = "4#剪切翻转气缸",
                Delay = Delay.Instance.LeftCut4OverturnCylinderDelay,
                Condition = new CylinderCondition(() => { return RightCut2FrontCylinder.OutOriginStatus; }, () => { return RightCut2FrontCylinder.OutOriginStatus; }) { External = m_External }
            };
            var RightCut2GripperCylinder = new SingleCylinder(IoPoints.T3IN31, IoPoints.T2IN11, IoPoints.T1DO23)
            {
                Name = "4#剪切夹子气缸",
                Delay = Delay.Instance.LeftCut4GripperCylinderDelay,
                Condition = new CylinderCondition(() => { return true; }, () => { return true; }) { External = m_External }
            };
            RightCut2GripperCylinder.Condition.NoMoveShield = true;
            RightCut2GripperCylinder.Condition.NoOriginShield = true;
            var InhaleLeft1Cylinder = new SingleCylinder3(IoPoints.T2IN12, IoPoints.T2IN14, IoPoints.T2IN16, IoPoints.T2IN13, IoPoints.T2IN15, IoPoints.T2IN17, IoPoints.T2DO0)
            {
                Name = "1# 2# 4#吸笔左右气缸",
                Delay = Delay.Instance.InhaleLeft1CylinderDelay,
                Condition = new CylinderCondition(() => { return true; }, () => { return true; }) { External = m_External }
            };
            var Left1InhaleCylinder = new VacuoBrokenCylinder(IoPoints.T3IN31, IoPoints.T1DO24, IoPoints.T1DO25)
            {
                Name = "1#吸笔吸气",
                Delay = Delay.Instance.Left1InhaleCylinderDelay,
                Condition = new CylinderCondition(() => { return true; }, () => { return true; }) { External = m_External }
            };
            Left1InhaleCylinder.Delay.BrokenTime = 99999999;
            Left1InhaleCylinder.Delay.InhaleTime = 99999999;
            Left1InhaleCylinder.Delay.AlarmTime = 99999999;
            Left1InhaleCylinder.Condition.NoMoveShield = true;
            Left1InhaleCylinder.Condition.NoOriginShield = true;
            var Left2InhaleCylinder = new VacuoBrokenCylinder(IoPoints.T3IN31, IoPoints.T1DO26, IoPoints.T1DO27)
            {
                Name = "2#吸笔吸气",
                Delay = Delay.Instance.Left2InhaleCylinderDelay,
                Condition = new CylinderCondition(() => { return true; }, () => { return true; }) { External = m_External }
            };
            Left2InhaleCylinder.Delay.BrokenTime = 99999999;
            Left2InhaleCylinder.Delay.InhaleTime = 99999999;
            Left2InhaleCylinder.Delay.AlarmTime = 99999999;
            Left2InhaleCylinder.Condition.NoMoveShield = true;
            Left2InhaleCylinder.Condition.NoOriginShield = true;
            var Right1InhaleCylinder = new VacuoBrokenCylinder(IoPoints.T3IN31, IoPoints.T1DO28, IoPoints.T1DO29)
            {
                Name = "3#吸笔吸气",
                Delay = Delay.Instance.Left3InhaleCylinderDelay,
                Condition = new CylinderCondition(() => { return true; }, () => { return true; }) { External = m_External }
            };
            Right1InhaleCylinder.Delay.BrokenTime = 99999999;
            Right1InhaleCylinder.Delay.InhaleTime = 99999999;
            Right1InhaleCylinder.Delay.AlarmTime = 99999999;
            Right1InhaleCylinder.Condition.NoMoveShield = true;
            Right1InhaleCylinder.Condition.NoOriginShield = true;
            var Right2InhaleCylinder = new VacuoBrokenCylinder(IoPoints.T3IN31, IoPoints.T1DO30, IoPoints.T1DO31)
            {
                Name = "4#吸笔吸气",
                Delay = Delay.Instance.Left4InhaleCylinderDelay,
                Condition = new CylinderCondition(() => { return true; }, () => { return true; }) { External = m_External }
            };
            Right2InhaleCylinder.Delay.BrokenTime = 99999999;
            Right2InhaleCylinder.Delay.InhaleTime = 99999999;
            Right2InhaleCylinder.Delay.AlarmTime = 99999999;
            Right2InhaleCylinder.Condition.NoMoveShield = true;
            Right2InhaleCylinder.Condition.NoOriginShield = true;
            var GetTrayCylinder = new SingleCylinder2(IoPoints.T2IN20, IoPoints.T2IN22, IoPoints.T2IN21, IoPoints.T2IN23, IoPoints.T2DO1)
            {
                Name = "取放盘上下气缸",
                Delay = Delay.Instance.GetTrayCylinderDelay,
                Condition = new CylinderCondition(() => { return true; }, () => { return true; }) { External = m_External }
            };
            var LockCylinder = new SingleCylinder(IoPoints.T2IN24, IoPoints.T2IN25, IoPoints.T2DO2)
            {
                Name = "托盘定位气缸",
                Delay = Delay.Instance.LockCylinderDelay,
                Condition = new CylinderCondition(() => { return true; }, () => { return true; }) { External = m_External }
            };

            var SafeDoorCylinder = new DoubleCylinder(IoPoints.T2IN30, IoPoints.T2IN31, IoPoints.T2DO3, IoPoints.T2DO4)
            {
                Name = "安全门升降",
                Delay = Delay.Instance.SafeDoorCylinderDelay,
                Condition = new CylinderCondition(() => { return IoPoints.T3IN7.Value; }, () => { return IoPoints.T3IN7.Value; }) { External = m_External }
            };
            var bufDownCylider = new DoubleCylinder(IoPoints.T1IN16, IoPoints.T1IN17, IoPoints.T1DO2, IoPoints.T2DO11)
            {
                Name = "缓冲升降气缸",
                Delay = Delay.Instance.bufDownCyliderDelay,
                Condition = new CylinderCondition(() => { return true; }, () => { return true; }) { External = m_External }
            };
            var bufLeftCylider = new SingleCylinder(IoPoints.T1IN18, IoPoints.T1IN19, IoPoints.T1DO3)
            {
                Name = "缓冲左右气缸",
                Delay = Delay.Instance.bufLeftCyliderDelay,
                Condition = new CylinderCondition(() => { return bufDownCylider.OutOriginStatus; }, () => { return bufDownCylider.OutOriginStatus; }) { External = m_External }
            };
            var bufGripperCylider = new SingleCylinder(IoPoints.T1IN20, IoPoints.T3IN31, IoPoints.T1DO4)
            {
                Name = "缓冲夹子气缸",
                Delay = Delay.Instance.bufGripperCyliderDelay,
                Condition = new CylinderCondition(() => { return true; }, () => { return true; }) { External = m_External }
            };
            bufGripperCylider.Condition.NoMoveShield = true;
            var NoRodFeedCylinder = new DoubleCylinder(IoPoints.T1IN0, IoPoints.T1IN1, IoPoints.T1DO0, IoPoints.T1DO1)
            {
                Name = "接料无杆气缸",
                Delay = Delay.Instance.NoRodFeedCylinderDelay,
                Condition = new CylinderCondition(() => { return bufLeftCylider.OutOriginStatus; }, () => { return bufLeftCylider.OutOriginStatus; }) { External = m_External }
            };
            NoRodFeedCylinder.Condition.NoMoveShield = true;

            #endregion

            #region 工站模组操作

            LoadingMessage("加载模组操作资源");
            var spliceInitialize = new StationInitialize(
                () => { return !ManualAutoMode && (hasp.LicenseIsOK || LicenseSheild); },
                () => { return SpliceIsAlarm.IsAlarm; });
            var spliceOperate = new StationOperate(
                 () => { return spliceInitialize.InitializeDone && (hasp.LicenseIsOK || LicenseSheild); },
                 () => { return SpliceIsAlarm.IsAlarm; });
            m_Splice = new Splice(m_External, spliceInitialize, spliceOperate)
            {
                NoRodFeedCylinder = NoRodFeedCylinder,
            };
            m_Splice.Run(RunningModes.Online);
            m_Splice.AddAlarms();

            var bufferInitialize = new StationInitialize(
                 () => { return !ManualAutoMode && (hasp.LicenseIsOK || LicenseSheild); },
                 () => { return BufferIsAlarm.IsAlarm; });
            var bufferOperate = new StationOperate(
                 () => { return bufferInitialize.InitializeDone && (hasp.LicenseIsOK || LicenseSheild); },
                 () => { return BufferIsAlarm.IsAlarm; });
            m_Buffer = new Buffer(m_External, bufferInitialize, bufferOperate)
            {
                DownCylinder = bufDownCylider,
                LeftCylinder = bufLeftCylider,
                GripperCylinder = bufGripperCylider
            };
            m_Buffer.Run(RunningModes.Online);
            m_Buffer.AddAlarms();

            var feederInitialize = new StationInitialize(
                 () => { return !ManualAutoMode && (hasp.LicenseIsOK || LicenseSheild); },
                 () => { return FeederIsAlarm.IsAlarm; });
            var feederOperate = new StationOperate(
                 () => { return feederInitialize.InitializeDone && (hasp.LicenseIsOK || LicenseSheild); },
                 () => { return FeederIsAlarm.IsAlarm; });
            m_Feeder = new Feeder(m_External, feederInitialize, feederOperate)
            {
                FeederCylinder = FeederCylinder,
            };
            m_Feeder.Run(RunningModes.Online);
            m_Feeder.AddAlarms();

            var moveInitialize = new StationInitialize(
                 () => { return !ManualAutoMode && (hasp.LicenseIsOK || LicenseSheild); },
                 () => { return MoveIsAlarm.IsAlarm; });
            var moveOperate = new StationOperate(
                 () => { return moveInitialize.InitializeDone && (hasp.LicenseIsOK || LicenseSheild); },
                 () => { return MoveIsAlarm.IsAlarm; });
            m_Move = new Move(m_External, moveInitialize, moveOperate)
            {
                LeftCylinder = MoveLeftCylinder,
                DownCylinder = MoveDownCylinder,
                GripperCylinder = MoveGripperCylinder,
                CutwasteCylinder1 = CutwasteCylinder1,
                CutwasteCylinder2 = CutwasteCylinder2,
                CutwasteCylinder3 = CutwasteCylinder3,
                Push1Axis = LeftPuch1Axis,
                Push2Axis = LeftPuch2Axis,
                Push3Axis = LeftPuch3Axis,
                Push4Axis = LeftPuch4Axis
            };
            m_Move.Run(RunningModes.Online);
            m_Move.AddAlarms();

            var leftcInitialize = new StationInitialize(
                 () => { return !ManualAutoMode && (hasp.LicenseIsOK || LicenseSheild); },
                 () => { return LeftCIsAlarm.IsAlarm; });
            var leftcOperate = new StationOperate(
                 () => { return leftcInitialize.InitializeDone && (hasp.LicenseIsOK || LicenseSheild); },
                 () => { return LeftCIsAlarm.IsAlarm; });
            m_LeftC = new LeftC(m_External, leftcInitialize, leftcOperate)
            {
                C1Axis = LeftC1Axis,
                C2Axis = LeftC2Axis,
                C3Axis = LeftC3Axis,
                C4Axis = LeftC4Axis,
                Push1Axis = LeftPuch1Axis,
                Push2Axis = LeftPuch2Axis,
                Push3Axis = LeftPuch3Axis,
                Push4Axis = LeftPuch4Axis,
            };
            m_LeftC.Run(RunningModes.Online);
            m_LeftC.AddAlarms();

            var leftcut1Initialize = new StationInitialize(
                 () => { return !ManualAutoMode && (hasp.LicenseIsOK || LicenseSheild); },
                 () => { return LeftCut1IsAlarm.IsAlarm; });
            var leftcut1Operate = new StationOperate(
                 () => { return leftcut1Initialize.InitializeDone && (hasp.LicenseIsOK || LicenseSheild); },
                 () => { return LeftCut1IsAlarm.IsAlarm; });
            m_LeftCut1 = new LeftCut1(m_External, leftcut1Initialize, leftcut1Operate)
            {
                CutAxis = LeftCutAxis1,
                FrontCylinder = LeftCut1FrontCylinder,
                OverturnCylinder = LeftCut1OverturnCylinder,
                GripperCylinder = LeftCut1GripperCylinder
            };
            m_LeftCut1.Run(RunningModes.Online);
            m_LeftCut1.AddAlarms();


            var leftcut2Initialize = new StationInitialize(
                 () => { return !ManualAutoMode && (hasp.LicenseIsOK || LicenseSheild); },
                 () => { return LeftCut2IsAlarm.IsAlarm; });
            var leftcut2Operate = new StationOperate(
                 () => { return leftcut2Initialize.InitializeDone && (hasp.LicenseIsOK || LicenseSheild); },
                 () => { return LeftCut2IsAlarm.IsAlarm; });
            m_LeftCut2 = new LeftCut2(m_External, leftcut2Initialize, leftcut2Operate)
            {
                CutAxis = LeftCutAxis2,
                FrontCylinder = LeftCut2FrontCylinder,
                OverturnCylinder = LeftCut2OverturnCylinder,
                GripperCylinder = LeftCut2GripperCylinder
            };
            m_LeftCut2.Run(RunningModes.Online);
            m_LeftCut2.AddAlarms();

            var rightcut1Initialize = new StationInitialize(
                 () => { return !ManualAutoMode && (hasp.LicenseIsOK || LicenseSheild); },
                 () => { return RightCut1IsAlarm.IsAlarm; });
            var rightcut1Operate = new StationOperate(
                 () => { return rightcut1Initialize.InitializeDone && (hasp.LicenseIsOK || LicenseSheild); },
                 () => { return RightCut1IsAlarm.IsAlarm; });
            m_RightCut1 = new RightCut1(m_External, rightcut1Initialize, rightcut1Operate)
            {
                CutAxis = LeftCutAxis3,
                FrontCylinder = RightCut1FrontCylinder,
                OverturnCylinder = RightCut1OverturnCylinder,
                GripperCylinder = RightCut1GripperCylinder
            };
            m_RightCut1.Run(RunningModes.Online);
            m_RightCut1.AddAlarms();

            var rightcut2Initialize = new StationInitialize(
                 () => { return !ManualAutoMode && (hasp.LicenseIsOK || LicenseSheild); },
                 () => { return RightCut2IsAlarm.IsAlarm; });
            var rightcut2Operate = new StationOperate(
                 () => { return rightcut2Initialize.InitializeDone && (hasp.LicenseIsOK || LicenseSheild); },
                 () => { return RightCut2IsAlarm.IsAlarm; });
            m_RightCut2 = new RightCut2(m_External, rightcut2Initialize, rightcut2Operate)
            {
                CutAxis = LeftCutAxis4,
                FrontCylinder = RightCut2FrontCylinder,
                OverturnCylinder = RightCut2OverturnCylinder,
                GripperCylinder = RightCut2GripperCylinder
            };
            m_RightCut2.Run(RunningModes.Online);
            m_RightCut2.AddAlarms();

            var platformInitialize = new StationInitialize(
                  () => { return !ManualAutoMode && (hasp.LicenseIsOK || LicenseSheild); },
                  () => { return PlateformIsAlarm.IsAlarm; });
            var platformOperate = new StationOperate(
                  () => { return platformInitialize.InitializeDone && (hasp.LicenseIsOK || LicenseSheild); },
                  () => { return PlateformIsAlarm.IsAlarm; });
            m_Platform = new Platform(m_External, platformInitialize, platformOperate)
            {
                Xaxis = Xaxis,
                Yaxis = Yaxis,
                Zaxis = Zaxis,
                Left1Cylinder = InhaleLeft1Cylinder,
                Left1InhaleCylinder = Left1InhaleCylinder,
                Left2InhaleCylinder = Left2InhaleCylinder,
                Right1InhaleCylinder = Right1InhaleCylinder,
                Right2InhaleCylinder = Right2InhaleCylinder,
                GetTrayCylinder = GetTrayCylinder,
                LockCylinder = LockCylinder
            };
            m_Platform.Run(RunningModes.Online);
            m_Platform.AddAlarms();

            var storingInitialize = new StationInitialize(
                 () => { return !ManualAutoMode && (hasp.LicenseIsOK || LicenseSheild); },
                 () => { return StoringIsAlarm.IsAlarm; });
            var storingOperate = new StationOperate(
                 () => { return storingInitialize.InitializeDone && (hasp.LicenseIsOK || LicenseSheild); },
                 () => { return StoringIsAlarm.IsAlarm; });
            m_Storing = new Storing(m_External, storingInitialize, storingOperate)
            {
                SafeDoolCylinder = SafeDoorCylinder,
                MAxis = Maxis
            };
            m_Storing.Run(RunningModes.Online);
            m_Storing.AddAlarms();

            #endregion

            #region 通讯参数   

            TempGTA2 = new GTA2();
            TempGTA2.Name = "温控器通讯";
            TConnect();

            TisRtuClient?.Close();
            TisRtuClient = new ModbusRtu(1);
            TisRtuClient.AddressStartWithZero = true;
            try
            {
                TisRtuClient.SerialPortInni(sp =>
                {
                    sp.PortName = Config.Instance.AnalogConnetPortName1;
                    sp.BaudRate = 9600;
                    sp.DataBits = 8;
                    sp.StopBits = System.IO.Ports.StopBits.One;
                    sp.Parity = System.IO.Ports.Parity.None;
                });
                TisRtuClient.Open();
                AnalogConnect = true;
            }
            catch (Exception ex)
            {
                AnalogConnect = false;
                AppendText("MODBUS通信未连上！");
            }
            #endregion

            #region 模组信息加载、启动

            LoadingMessage("加载模组信息");

            MachineOperation = new MachineOperate(() =>
            {
                return spliceInitialize.InitializeDone & bufferInitialize.InitializeDone & feederInitialize.InitializeDone
                & moveInitialize.InitializeDone & leftcInitialize.InitializeDone & leftcut1Initialize.InitializeDone &
                leftcut2Initialize.InitializeDone & rightcut1Initialize.InitializeDone & rightcut2Initialize.InitializeDone
                & platformInitialize.InitializeDone & storingInitialize.InitializeDone && (hasp.LicenseIsOK || LicenseSheild);
            }, () =>
            {
                return MachineIsAlarm.IsAlarm | SpliceIsAlarm.IsAlarm | BufferIsAlarm.IsAlarm | FeederIsAlarm.IsAlarm
                | MoveIsAlarm.IsAlarm | LeftCIsAlarm.IsAlarm | LeftCut1IsAlarm.IsAlarm | LeftCut2IsAlarm.IsAlarm | RightCut1IsAlarm.IsAlarm
                | RightCut2IsAlarm.IsAlarm | PlateformIsAlarm.IsAlarm | StoringIsAlarm.IsAlarm;
            }, () =>
            {
                return MachineIsAlarm.IsWarning | SpliceIsAlarm.IsWarning | BufferIsAlarm.IsWarning | FeederIsAlarm.IsWarning
                | MoveIsAlarm.IsWarning | LeftCIsAlarm.IsWarning | LeftCut1IsAlarm.IsWarning | LeftCut2IsAlarm.IsWarning | RightCut1IsAlarm.IsWarning
                | RightCut2IsAlarm.IsWarning | PlateformIsAlarm.IsWarning | StoringIsAlarm.IsWarning;
            });
            AddAlarms();

            #endregion

            #region  故障代码建立
            int faultCode = 0;
            foreach (var arm in m_Splice.Alarms)
            {
                Fault fault = new Fault();
                faultCode++;
                fault.FaultCode = faultCode;
                fault.FaultMessage = arm.Name;
                fault.FaultCount = 0;
                Global.FaultDictionary.Add(arm.Name, fault);
            }
            foreach (var arm in m_Buffer.Alarms)
            {
                Fault fault = new Fault();
                faultCode++;
                fault.FaultCode = faultCode;
                fault.FaultMessage = arm.Name;
                fault.FaultCount = 0;
                Global.FaultDictionary.Add(arm.Name, fault);
            }
            foreach (var arm in m_Feeder.Alarms)
            {
                Fault fault = new Fault();
                faultCode++;
                fault.FaultCode = faultCode;
                fault.FaultMessage = arm.Name;
                fault.FaultCount = 0;
                Global.FaultDictionary.Add(arm.Name, fault);
            }
            foreach (var arm in m_Move.Alarms)
            {
                Fault fault = new Fault();
                faultCode++;
                fault.FaultCode = faultCode;
                fault.FaultMessage = arm.Name;
                fault.FaultCount = 0;
                Global.FaultDictionary.Add(arm.Name, fault);
            }
            foreach (var arm in m_LeftC.Alarms)
            {
                Fault fault = new Fault();
                faultCode++;
                fault.FaultCode = faultCode;
                fault.FaultMessage = arm.Name;
                fault.FaultCount = 0;
                Global.FaultDictionary.Add(arm.Name, fault);
            }
            foreach (var arm in m_LeftCut1.Alarms)
            {
                Fault fault = new Fault();
                faultCode++;
                fault.FaultCode = faultCode;
                fault.FaultMessage = arm.Name;
                fault.FaultCount = 0;
                Global.FaultDictionary.Add(arm.Name, fault);
            }
            foreach (var arm in m_LeftCut2.Alarms)
            {
                Fault fault = new Fault();
                faultCode++;
                fault.FaultCode = faultCode;
                fault.FaultMessage = arm.Name;
                fault.FaultCount = 0;
                Global.FaultDictionary.Add(arm.Name, fault);
            }
            foreach (var arm in m_RightCut1.Alarms)
            {
                Fault fault = new Fault();
                faultCode++;
                fault.FaultCode = faultCode;
                fault.FaultMessage = arm.Name;
                fault.FaultCount = 0;
                Global.FaultDictionary.Add(arm.Name, fault);
            }
            foreach (var arm in m_RightCut2.Alarms)
            {
                Fault fault = new Fault();
                faultCode++;
                fault.FaultCode = faultCode;
                fault.FaultMessage = arm.Name;
                fault.FaultCount = 0;
                Global.FaultDictionary.Add(arm.Name, fault);
            }
            foreach (var arm in m_Platform.Alarms)
            {
                Fault fault = new Fault();
                faultCode++;
                fault.FaultCode = faultCode;
                fault.FaultMessage = arm.Name;
                fault.FaultCount = 0;
                Global.FaultDictionary.Add(arm.Name, fault);
            }
            foreach (var arm in m_Storing.Alarms)
            {
                Fault fault = new Fault();
                faultCode++;
                fault.FaultCode = faultCode;
                fault.FaultMessage = arm.Name;
                fault.FaultCount = 0;
                Global.FaultDictionary.Add(arm.Name, fault);
            }

            foreach (var arm in MachineAlarms)
            {
                Fault fault = new Fault();
                faultCode++;
                fault.FaultCode = faultCode;
                fault.FaultMessage = arm.Name;
                fault.FaultCount = 0;
                Global.FaultDictionary.Add(arm.Name, fault);
            }

            #endregion

            #region 加载信号灯资源
            try
            {
                StartButton = new LightButton(IoPoints.T3IN0, IoPoints.T2DO16);
                ClearAlarmButton = new LightButton(IoPoints.T3IN4, IoPoints.T2DO20);  //报警复位
                PauseButton = new LightButton(IoPoints.T3IN1, IoPoints.T2DO17);  //暂停,停止一样
                EstopButton = new EventButton(IoPoints.T3IN2);
                StopButton = new LightButton(IoPoints.T3IN1, IoPoints.T2DO17);
                GoHome = new LightButton(IoPoints.T3IN3, IoPoints.T2DO18);
                ManualUpButton = new LightButton(IoPoints.T3IN9, IoPoints.T2DO17);

                layerLight = new LayerLight(IoPoints.T2DO23, IoPoints.T2DO22, IoPoints.T2DO21, IoPoints.T2DO24, Config.Instance.RunSturs, Config.Instance.SuspendSturs
                    , Config.Instance.StopSturs, Config.Instance.ErrSturs, Config.Instance.ResetSturs, Config.Instance.WarningSturs);

                StartButton.Pressed += btnStart_MouseDown;
                StartButton.Released += btnStart_MouseUp;
                PauseButton.Pressed += btnPause_MouseDown;
                PauseButton.Released += btnPause_MouseUp;
                ClearAlarmButton.Pressed += BtnAlReset_MouseDown;
                ClearAlarmButton.Released += BtnAlReset_MouseUp;
                StopButton.Pressed += btnStop_MouseDown;
                StopButton.Released += btnStop_MouseUp;
                GoHome.Pressed += btnReset_MouseDown;
                GoHome.Released += btnReset_MouseUp;

                ManualUpButton.Pressed += btnUp_MouseDown;
                ManualUpButton.Released += mbtnStop;

                MachineOperation.EButton = ManualUpButton;
                MachineOperation.StartButton = StartButton;
                MachineOperation.StopButton = StopButton;
                MachineOperation.PauseButton = PauseButton;
                MachineOperation.ResetButton = GoHome;
                MachineOperation.EstopButton = EstopButton;
                MachineOperation.ClearAlarm = ClearAlarmButton;
                ManualAutoMode = false;
                btnManualAuto.BackColor = ManualAutoMode ? Color.Red : Color.Green;
                layerLight.VoiceClosed = true;  //屏蔽蜂鸣器
                btnTricolorLamp.Text = layerLight.VoiceClosed ? "蜂铃静止" : "蜂铃正常";
                btnTricolorLamp.BackColor = layerLight.VoiceClosed ? Color.Red : Color.MediumBlue;
            }
            catch(Exception ex)
            {
                throw(ex);
            }

            #endregion

            try
            {
                LoadingMessage("加载子窗体资源");
                AddSubForm();

                LoadingMessage("加载线程资源");
                SerialStart();
                InitTray();

                IoPoints.T2DO6.Value = true;
                IoPoints.T2DO7.Value = true;
                IoPoints.T2DO8.Value = true;
                IoPoints.T2DO9.Value = true;
                IoPoints.T2DO5.Value = true;
                Cut4UpTheoreticalValue.Text = (Config.Instance.TemperatureValue[6]).ToString() + "度";
                Cut3UpTheoreticalValue.Text = (Config.Instance.TemperatureValue[4]).ToString() + "度";
                Cut2UpTheoreticalValue.Text = (Config.Instance.TemperatureValue[2]).ToString() + "度";
                Cut1UpTheoreticalValue.Text = (Config.Instance.TemperatureValue[0]).ToString() + "度";
                Cut4DownTheoreticalValue.Text = (Config.Instance.TemperatureValue[7]).ToString() + "度";
                Cut3DownTheoreticalValue.Text = (Config.Instance.TemperatureValue[5]).ToString() + "度";
                Cut2DownTheoreticalValue.Text = (Config.Instance.TemperatureValue[3]).ToString() + "度";
                Cut1DownTheoreticalValue.Text = (Config.Instance.TemperatureValue[1]).ToString() + "度";
                Global.WritePulse = true;

                LoadingMessage("加载托盘资源");
                Global.BigTray.CurrentPos = Config.Instance.BigPlatePos;
                Global.SmallTray.CurrentPos = Config.Instance.YoungPlatePos;
                Global.SpecialTray.CurrentPos = Config.Instance.SpecialPlatePos;

                timer1.Enabled = true;
            }
            catch(Exception ex)
            {
                throw(ex);
            }
        }
        #endregion

        #region 窗体关闭
        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                DialogResult result = MessageBox.Show("是否保存配置文件再退出？", "退出", MessageBoxButtons.YesNoCancel);
                if (result == DialogResult.Yes)
                {
                    SaveFile();
                    threadMachineRun?.Abort();
                    threadAlarmCheck?.Abort();
                    threadStatusCheck?.Abort();
                    IoPoints.T2DO10.Value = false;
                    IoPoints.ApsController.Dispose();
                    LogHelper.Debug("已保存配置文件");
                }
                else if (result == DialogResult.No)
                {
                    threadMachineRun?.Abort();
                    threadAlarmCheck?.Abort();
                    threadStatusCheck?.Abort();
                    IoPoints.T2DO10.Value = false;
                    LogHelper.Debug("不保存配置文件");
                }
                else
                {
                    e.Cancel = true;
                }
            }
            catch(Exception ex)
            {
                throw(ex);
            }
        }

        #endregion

        #region 窗体切换
        private void btnMain_Click(object sender, EventArgs e)
        {
            try
            {
                tbcMain.SelectedTab = tpgMain;
                LogHelper.Info("返回主窗体");
                btnMain.BackColor = Color.Green;
                btnWorkParam.BackColor = Color.MediumBlue;
                btnOffces.BackColor = Color.MediumBlue;
                btnMaintain.BackColor = Color.MediumBlue;
                btnSpeed.BackColor = Color.MediumBlue;
                btnTeam.BackColor = Color.MediumBlue;
                btnManual.BackColor = Color.MediumBlue;
                btnIOMonitor.BackColor = Color.MediumBlue;
                btnErrMessage.BackColor = Color.MediumBlue;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        private void btnManualOperate_Click(object sender, EventArgs e)
        {
            try
            {
                tbcMain.SelectedTab = tpgMaintain;
                LogHelper.Info("切换到维修设定");
                btnMain.BackColor = Color.MediumBlue;
                btnWorkParam.BackColor = Color.MediumBlue;
                btnOffces.BackColor = Color.MediumBlue;
                btnMaintain.BackColor = Color.Green;
                btnSpeed.BackColor = Color.MediumBlue;
                btnTeam.BackColor = Color.MediumBlue;
                btnManual.BackColor = Color.MediumBlue;
                btnIOMonitor.BackColor = Color.MediumBlue;
                btnErrMessage.BackColor = Color.MediumBlue;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        private void btnTeach_Click(object sender, EventArgs e)
        {
            try
            {
                tbcMain.SelectedTab = tpgSpeed;
                LogHelper.Info("切换到运作速度");
                btnMain.BackColor = Color.MediumBlue;
                btnWorkParam.BackColor = Color.MediumBlue;
                btnOffces.BackColor = Color.MediumBlue;
                btnMaintain.BackColor = Color.MediumBlue;
                btnSpeed.BackColor = Color.Green;
                btnTeam.BackColor = Color.MediumBlue;
                btnManual.BackColor = Color.MediumBlue;
                btnIOMonitor.BackColor = Color.MediumBlue;
                btnErrMessage.BackColor = Color.MediumBlue;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        private void btnParameter_Click(object sender, EventArgs e)
        {
            try
            {
                if (Marking.ThrowerMode)
                {
                    AppendText("设备抛料中，请先停止抛料!");
                    return;
                }
                tbcMain.SelectedTab = tpgManualOperate;
                LogHelper.Info("切换到手动界面");
                btnMain.BackColor = Color.MediumBlue;
                btnWorkParam.BackColor = Color.MediumBlue;
                btnOffces.BackColor = Color.MediumBlue;
                btnMaintain.BackColor = Color.MediumBlue;
                btnSpeed.BackColor = Color.MediumBlue;
                btnTeam.BackColor = Color.MediumBlue;
                btnManual.BackColor = Color.Green;
                btnIOMonitor.BackColor = Color.MediumBlue;
                btnErrMessage.BackColor = Color.MediumBlue;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        private void btnTraySetting_Click(object sender, EventArgs e)
        {
            try
            {
                if (Marking.ThrowerMode)
                {
                    AppendText("设备抛料中，请先停止抛料!");
                    return;
                }
                tbcMain.SelectedTab = tpgTeach;
                m_FrmTeach.RefreshHole();
                LogHelper.Info("切换到教导界面");
                btnMain.BackColor = Color.MediumBlue;
                btnWorkParam.BackColor = Color.MediumBlue;
                btnOffces.BackColor = Color.MediumBlue;
                btnMaintain.BackColor = Color.MediumBlue;
                btnSpeed.BackColor = Color.MediumBlue;
                btnTeam.BackColor = Color.Green;
                btnManual.BackColor = Color.MediumBlue;
                btnIOMonitor.BackColor = Color.MediumBlue;
                btnErrMessage.BackColor = Color.MediumBlue;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        private void btnIOMonitor_Click(object sender, EventArgs e)
        {
            try
            {
                tbcMain.SelectedTab = tabIOmonitor;
                LogHelper.Info("切换IO监视界面");
                btnMain.BackColor = Color.MediumBlue;
                btnWorkParam.BackColor = Color.MediumBlue;
                btnOffces.BackColor = Color.MediumBlue;
                btnMaintain.BackColor = Color.MediumBlue;
                btnSpeed.BackColor = Color.MediumBlue;
                btnTeam.BackColor = Color.MediumBlue;
                btnManual.BackColor = Color.MediumBlue;
                btnIOMonitor.BackColor = Color.Green;
                btnErrMessage.BackColor = Color.MediumBlue;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        private void btnMainView_Click(object sender, EventArgs e)
        {
            try
            {
                tbcMain.SelectedTab = tpgWorksetting;
                LogHelper.Info("切换工作参数");
                btnMain.BackColor = Color.MediumBlue;
                btnWorkParam.BackColor = Color.Green;
                btnOffces.BackColor = Color.MediumBlue;
                btnMaintain.BackColor = Color.MediumBlue;
                btnSpeed.BackColor = Color.MediumBlue;
                btnTeam.BackColor = Color.MediumBlue;
                btnManual.BackColor = Color.MediumBlue;
                btnIOMonitor.BackColor = Color.MediumBlue;
                btnErrMessage.BackColor = Color.MediumBlue;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                tbcMain.SelectedTab = tpgErrMessage;
                LogHelper.Info("切换故障信息");
                btnMain.BackColor = Color.MediumBlue;
                btnWorkParam.BackColor = Color.MediumBlue;
                btnOffces.BackColor = Color.MediumBlue;
                btnMaintain.BackColor = Color.MediumBlue;
                btnSpeed.BackColor = Color.MediumBlue;
                btnTeam.BackColor = Color.MediumBlue;
                btnManual.BackColor = Color.MediumBlue;
                btnIOMonitor.BackColor = Color.MediumBlue;
                btnErrMessage.BackColor = Color.Green;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        private void BtnOffces_Click(object sender, EventArgs e)
        {
            try
            {
                tbcMain.SelectedTab = tpgOffces;
                LogHelper.Info("切换到偏差参数");
                btnMain.BackColor = Color.MediumBlue;
                btnWorkParam.BackColor = Color.MediumBlue;
                btnOffces.BackColor = Color.Green;
                btnMaintain.BackColor = Color.MediumBlue;
                btnSpeed.BackColor = Color.MediumBlue;
                btnTeam.BackColor = Color.MediumBlue;
                btnManual.BackColor = Color.MediumBlue;
                btnIOMonitor.BackColor = Color.MediumBlue;
                btnErrMessage.BackColor = Color.MediumBlue;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        private void btnOpenfile_Click(object sender, EventArgs e)
        {
            try
            {
                frmParameter frm = new frmParameter();
                frm.ShowDialog();
                frm.Dispose();
                LogHelper.Info("切换文件开启操作");
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        private void butSaveFile_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult result = MessageBox.Show("是否保存配置文件", "提示", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    SaveFile();
                    LogHelper.Info("已保存配置文件");
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        private void SaveFile()
        {
            try
            {
                SerializerManager<Config>.Instance.Save(AppConfig.ConfigFileName, Config.Instance);
                SerializerManager<Delay>.Instance.Save(AppConfig.ConfigDelayName, Delay.Instance);
                SerializerManager<AxisParameter>.Instance.Save(AppConfig.ConfigAxisName, AxisParameter.Instance);
                TrayFactory.SaveTrayFactory(AppConfig.ConfigTrayName);
                SerializerManager<Position>.Instance.Save(AppConfig.ConfigPositionName, Position.Instance);
                Marking.ModifyParameterMarker = false;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
         
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            try
            {
                frmLogin frm = new frmLogin();
                frm.ShowDialog();
                frm.Dispose();
                OnUserLevelChange(Config.Instance.userLevel);
                LogHelper.Info("登录操作");
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        private void AddSubForm()
        {
            try
            {
                GenerateForm(new frmManualOperate(m_Splice, m_Buffer, m_Feeder, m_Move, m_LeftC,
                    m_LeftCut1, m_LeftCut2, m_RightCut1, m_RightCut2, m_Platform, m_Storing), tpgManualOperate);
                GenerateForm(new frmWorkParam(m_Splice, m_Buffer, m_Feeder, m_Move, m_LeftC,
                    m_LeftCut1, m_LeftCut2, m_RightCut1, m_RightCut2, m_Platform, m_Storing), tpgWorksetting);
                GenerateForm(new frmIOmonitor(), tabIOmonitor);
                m_FrmTeach = new frmTeach(m_Splice, m_Buffer, m_Feeder, m_Move, m_LeftC, m_LeftCut1, m_LeftCut2, m_RightCut1, m_RightCut2, m_Platform, m_Storing);
                GenerateForm(m_FrmTeach, tpgTeach);
                GenerateForm(new frmFaultView(new List<Alarm>[] { m_Splice .Alarms,m_Buffer.Alarms, m_Feeder.Alarms, m_Move.Alarms, m_LeftC.Alarms,
                m_LeftCut1.Alarms, m_LeftCut2.Alarms, m_RightCut1.Alarms, m_RightCut2.Alarms, m_Platform.Alarms, m_Storing.Alarms, MachineAlarms}), tpgErrMessage);
                GenerateForm(new frmSpeedSet(m_Splice, m_Buffer, m_Feeder, m_Move, m_LeftC,
                    m_LeftCut1, m_LeftCut2, m_RightCut1, m_RightCut2, m_Platform, m_Storing), tpgSpeed);
                GenerateForm(new frmMaintain(m_Splice, m_Buffer, m_Feeder, m_Move, m_LeftC,
                    m_LeftCut1, m_LeftCut2, m_RightCut1, m_RightCut2, m_Platform, m_Storing), tpgMaintain);
                GenerateForm(new frmOffces(), tpgOffces);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        /// <summary>
        /// 在选项卡中生成窗体
        /// </summary>
        private void GenerateForm(Form frm, TabPage sender)
        {
            try
            {
                //设置窗体没有边框 加入到选项卡中  
                frm.TopLevel = false;
                frm.FormBorderStyle = FormBorderStyle.None;
                frm.Parent = sender;
                frm.Dock = DockStyle.Fill;
                frm.Show();
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }


        #endregion

        #region 线程处理
        private void SerialStart()
        {
            try
            {
                threadMachineRun = new Thread(MachineRun);
                threadMachineRun.IsBackground = true;
                threadMachineRun.Start();

                threadAlarmCheck = new Thread(AlarmCheck);
                threadAlarmCheck.IsBackground = true;
                threadAlarmCheck.Start();

                threadStatusCheck = new Thread(StatusCheck);
                threadStatusCheck.IsBackground = true;
                threadStatusCheck.Start();

                threadReshing = new Thread(reshing1);
                threadReshing.IsBackground = true;
                threadReshing.Start();

                threadLicenseCheck = new Thread(LicenseCheck);
                threadLicenseCheck.IsBackground = true;
                threadLicenseCheck.Start();
            }
            catch (Exception ex)
            {
                AppendText("Server start Error: " + ex.Message);
            }
        }

        private void reshing1()
        {
            while (true)
            {
                if (TempGTA2.IsOpen)
                {
                    new Action(() =>
                    {
                        WriteSV();
                        TempGTA2.Trigger(new TriggerArgs()
                        {
                            sender = this,
                            tryTimes = 0x02,
                            message = "R1"
                        });
                        TempGTA2.Trigger(new TriggerArgs()
                        {
                            sender = this,
                            tryTimes = 0x02,
                            message = "R2"
                        });
                        TempGTA2.Trigger(new TriggerArgs()
                        {
                            sender = this,
                            tryTimes = 0x02,
                            message = "R3"
                        });
                        TempGTA2.Trigger(new TriggerArgs()
                        {
                            sender = this,
                            tryTimes = 0x02,
                            message = "R4"
                        });
                        TempGTA2.Trigger(new TriggerArgs()
                        {
                            sender = this,
                            tryTimes = 0x03,
                            message = "R1"
                        });
                        TempGTA2.Trigger(new TriggerArgs()
                        {
                            sender = this,
                            tryTimes = 0x03,
                            message = "R2"
                        });
                        TempGTA2.Trigger(new TriggerArgs()
                        {
                            sender = this,
                            tryTimes = 0x03,
                            message = "R3"
                        });
                        TempGTA2.Trigger(new TriggerArgs()
                        {
                            sender = this,
                            tryTimes = 0x03,
                            message = "R4"
                        });
                    }).Invoke();
                }
                Thread.Sleep(50);
                for (int i = 0; i < 4; i++)
                {
                    try
                    {
                        if (AnalogConnect)
                        {
                            writeResultRender(TisRtuClient.Write(Config.Instance.AnalogAddress[i], Config.Instance.PressCut[i] * 32767 / 10000), Config.Instance.AnalogAddress[i]);
                        }
                    }
                    catch
                    {
                        AnalogConnect = false;
                        //AppendText("模拟量未通讯上");
                    }
                }
            }
        }

        private void LicenseCheck()
        {
            while (true)
            {
                try
                {
                    if(!LicenseSheild) hasp.UnauthorizedDetection();
                    Thread.Sleep(600000);
                }
                catch (Exception ex) { MessageBox.Show(ex.ToString()); }
            }
        }

        private void btnHasp_Click(object sender, EventArgs e)
        {
            ShowWindows(null,null);
        }

        /// <summary>
        /// 统一的数据写入的结果显示
        /// </summary>
        /// <param name="result"></param>
        /// <param name="address"></param>
        private void writeResultRender(OperateResult result, string address)
        {
            if (result.IsSuccess)
            {
                //LogHelper.Debug("写入成功");
            }
            else
            {
                //AnalogConnect = false;
                //LogHelper.Debug("写入失败");
            }
        }

        private void TConnect()
        {
            try
            {                
                TempGTA2.SetConnectionParam(Config.Instance.AnalogConnetPortName + ",9600,None,8,One,1500,1500");
                TempGTA2.DeviceDataReceiveCompelete += new DataReceiveCompleteEventHandler(DealWithReceiveData);
                TempGTA2.DeviceOpen();
            }
            catch (Exception ex)
            {
                AppendText(string.Format("上料处读码器连接失败：{0}", ex.Message));
            }
        }

        void DealWithReceiveData(object sender, string result)
        {
            if (sender.GetType() != typeof(frmMain))
            {
                try
                {
                    if (result.Contains(UniversalFlags.errorStr))
                        throw new Exception(result);
                    string[] results = result.Split(',');
                    if (results.Length != 9)
                    {
                        throw new Exception("结果解析后长度不为8");
                    }
                    if (results[1] == "16")
                    {
                        if (results[3] == "12") WriteSign[0] = true;
                        if (results[3] == "13") WriteSign[1] = true;
                        if (results[3] == "14") WriteSign[2] = true;
                        if (results[3] == "15")
                        {
                            if (WriteSign[0] && WriteSign[1] && WriteSign[2])
                            {
                                WriteSign[0] = false;
                                WriteSign[1] = false;
                                WriteSign[2] = false;
                                //AppendText(string.Format("温控器{0}设置成功！", results[0]));
                            }
                            else AppendText(string.Format("温控器{0}设置失败！", results[0]));
                        }
                    }
                    else if (results[1] == "3")
                    {
                        if (results[0] == "2")
                        {
                            if (results[8] == "0")
                            {
                                var data = (byte.Parse(results[3]) << 8) | byte.Parse(results[4]);
                                LeftPV1 = data.ToString();
                                Marking.CurrTemperatureValue[0] = Convert.ToInt32(data);
                            }
                            if (results[8] == "1")
                            {
                                var data = (byte.Parse(results[3]) << 8) | byte.Parse(results[4]);
                                LeftPV2 = (data / 10.0).ToString("0.0");
                                Marking.CurrTemperatureValue[1] = Convert.ToInt32(data);
                            }
                            if (results[8] == "2")
                            {
                                var data = (byte.Parse(results[3]) << 8) | byte.Parse(results[4]);
                                LeftPV3 = (data / 10.0).ToString("0.0");
                                Marking.CurrTemperatureValue[2] = Convert.ToInt32(data);
                            }
                            if (results[8] == "3")
                            {
                                var data = (byte.Parse(results[3]) << 8) | byte.Parse(results[4]);
                                LeftPV4 = (data / 10.0).ToString("0.0");
                                Marking.CurrTemperatureValue[3] = Convert.ToInt32(data);
                            }
                        }
                        if (results[0] == "3")
                        {
                            if (results[8] == "0")
                            {
                                var data = (byte.Parse(results[3]) << 8) | byte.Parse(results[4]);
                                RightPV1 = (data / 10.0).ToString("0.0");
                                Marking.CurrTemperatureValue[4] = Convert.ToInt32(data);
                            }
                            if (results[8] == "1")
                            {
                                var data = (byte.Parse(results[3]) << 8) | byte.Parse(results[4]);
                                RightPV2 = (data / 10.0).ToString("0.0");
                                Marking.CurrTemperatureValue[5] = Convert.ToInt32(data);
                            }
                            if (results[8] == "2")
                            {
                                var data = (byte.Parse(results[3]) << 8) | byte.Parse(results[4]);
                                RightPV3 = (data / 10.0).ToString("0.0");
                                Marking.CurrTemperatureValue[6] = Convert.ToInt32(data);
                            }
                            if (results[8] == "3")
                            {
                                var data = (byte.Parse(results[3]) << 8) | byte.Parse(results[4]);
                                RightPV4 = (data / 10.0).ToString("0.0");
                                Marking.CurrTemperatureValue[7] = Convert.ToInt32(data);
                            }
                        }
                    }
                    else if (results[1] == "144") AppendText(string.Format("温控器{0}设置失败！", results[0]));
                    else if (results[1] == "131") AppendText(string.Format("温控器{0}读取失败！", results[0]));
                }
                catch (Exception ex)
                {
                    //AppendText("温控器通讯" + ex.Message);
                }
            }
        }

        private void WriteSV()
        {
            if (!Global.WritePulse) return;
            TempGTA2.Trigger(new TriggerArgs()
            {
                sender = this,
                tryTimes = 0x02,
                message = "W1," + Config.Instance.TemperatureValue[0].ToString()
            });
            TempGTA2.Trigger(new TriggerArgs()
            {
                sender = this,
                tryTimes = 0x02,
                message = "W2," + Config.Instance.TemperatureValue[1].ToString()
            });
            TempGTA2.Trigger(new TriggerArgs()
            {
                sender = this,
                tryTimes = 0x02,
                message = "W3," + Config.Instance.TemperatureValue[2].ToString()
            });
            TempGTA2.Trigger(new TriggerArgs()
            {
                sender = this,
                tryTimes = 0x02,
                message = "W4," + Config.Instance.TemperatureValue[3].ToString()
            });
            TempGTA2.Trigger(new TriggerArgs()
            {
                sender = this,
                tryTimes = 0x03,
                message = "W1," + Config.Instance.TemperatureValue[4].ToString()
            });
            TempGTA2.Trigger(new TriggerArgs()
            {
                sender = this,
                tryTimes = 0x03,
                message = "W2," + Config.Instance.TemperatureValue[5].ToString()
            });
            TempGTA2.Trigger(new TriggerArgs()
            {
                sender = this,
                tryTimes = 0x03,
                message = "W3," + Config.Instance.TemperatureValue[6].ToString()
            });
            TempGTA2.Trigger(new TriggerArgs()
            {
                sender = this,
                tryTimes = 0x03,
                message = "W4," + Config.Instance.TemperatureValue[7].ToString()
            });
            TempGTA2.Trigger(new TriggerArgs()
            {
                sender = this,
                tryTimes = 0x02,
                message = "WO1," + (Config.Instance.TemperatureViewOffset[0]/10).ToString()
            });
            TempGTA2.Trigger(new TriggerArgs()
            {
                sender = this,
                tryTimes = 0x02,
                message = "WO2," + (Config.Instance.TemperatureViewOffset[1]/10).ToString()
            });
            TempGTA2.Trigger(new TriggerArgs()
            {
                sender = this,
                tryTimes = 0x02,
                message = "WO3," + (Config.Instance.TemperatureViewOffset[2]/10).ToString()
            });
            TempGTA2.Trigger(new TriggerArgs()
            {
                sender = this,
                tryTimes = 0x02,
                message = "WO4," + (Config.Instance.TemperatureViewOffset[3]/10).ToString()
            });
            TempGTA2.Trigger(new TriggerArgs()
            {
                sender = this,
                tryTimes = 0x03,
                message = "WO1," + (Config.Instance.TemperatureViewOffset[4]/10).ToString()
            });
            TempGTA2.Trigger(new TriggerArgs()
            {
                sender = this,
                tryTimes = 0x03,
                message = "WO2," + (Config.Instance.TemperatureViewOffset[5]/10).ToString()
            });
            TempGTA2.Trigger(new TriggerArgs()
            {
                sender = this,
                tryTimes = 0x03,
                message = "WO3," + (Config.Instance.TemperatureViewOffset[6]/10).ToString()
            });
            TempGTA2.Trigger(new TriggerArgs()
            {
                sender = this,
                tryTimes = 0x03,
                message = "WO4," + (Config.Instance.TemperatureViewOffset[7]/10).ToString()
            });

            Cut4UpTheoreticalValue.Text = (Config.Instance.TemperatureValue[6]).ToString() + "度";
            Cut3UpTheoreticalValue.Text = (Config.Instance.TemperatureValue[4]).ToString() + "度";
            Cut2UpTheoreticalValue.Text = (Config.Instance.TemperatureValue[2]).ToString() + "度";
            Cut1UpTheoreticalValue.Text = (Config.Instance.TemperatureValue[0]).ToString() + "度";
            Cut4DownTheoreticalValue.Text = (Config.Instance.TemperatureValue[7]).ToString() + "度";
            Cut3DownTheoreticalValue.Text = (Config.Instance.TemperatureValue[5]).ToString() + "度";
            Cut2DownTheoreticalValue.Text = (Config.Instance.TemperatureValue[3]).ToString() + "度";
            Cut1DownTheoreticalValue.Text = (Config.Instance.TemperatureValue[1]).ToString() + "度";

            Global.WritePulse = false;

        }

        private void MachineRun()
        {
            watchCT = new Stopwatch();
            watchCT1 = new Stopwatch();
            closeDoorTime = new Stopwatch();
            watchCT.Start();
            watchCT1.Start();
            closeDoorTime.Start();
            while (true)
            {
                Thread.Sleep(5);
                TimeSpan ts1 = watchCT1.Elapsed;
                string elapsedTime1 = String.Format("{0:00}:{1:00}:{2:00}.{3:00}", ts1.Hours, ts1.Minutes, ts1.Seconds, ts1.Milliseconds / 10);
                Marking.StartRunTime = elapsedTime1;

                #region Y轴安全信号
                if (!IoPoints.T3IN7.Value && !Marking.AutoDoorSheild)
                {
                    Marking.ChangeTrayPadlock = true;
                }
                else
                {
                    Marking.ChangeTrayPadlock = false;
                }
                #endregion

                #region 自动关闭安全门
                if (!IoPoints.T3IN7.Value)
                {
                    IoPoints.T2DO3.Value = false;
                    IoPoints.T2DO4.Value = false;
                    closeDoorTime.Restart();
                }
                if(IoPoints.T3IN9.Value)
                {
                    closeDoorTime.Restart();
                }
                if (Delay.Instance.outoCloseDoorOpen && closeDoorTime.ElapsedMilliseconds >= Delay.Instance.outoCloseDoorTime)
                {
                    IoPoints.T2DO3.Value = false;
                    IoPoints.T2DO4.Value = true;
                }
                #endregion

                #region 左门禁打开关闭碎料电机
                if (1 == Position.Instance.FragmentationMode && Position.Instance.CheckMotorStart && !Marking.DoorSafeSensorSheild && !IoPoints.T3IN5.Value)
                {
                    IoPoints.T2DO10.Value = false; //关闭碎料电机
                }
                if (1 == Position.Instance.FragmentationMode && !IoPoints.T3IN15.Value && Position.Instance.CheckOverload)
                {
                    IoPoints.T2DO10.Value = false; //关闭碎料电机
                    Marking.MotorOverloadSign = true;
                }
                #endregion

                #region 状态刷新
                if (MachineOperation.Status == MachineStatus.设备运行中)
                {
                    watchCT.Start();
                }
                else { watchCT.Stop(); }
                TimeSpan ts = watchCT.Elapsed;
                string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}", ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds / 10);
                Marking.CycleRunTime = elapsedTime;


                m_External.AirSignal = !IoPoints.T3IN8.Value;
                m_External.ManualAutoMode = ManualAutoMode;

                MachineOperation.ManualAutoModel = ManualAutoMode;
                MachineOperation.CleanProductDone = Global.CleanProductDone;
                MachineOperation.Run();

                layerLight.Status = MachineOperation.Status;
                layerLight.Refreshing();

                m_Splice.stationOperate.ManualAutoMode = ManualAutoMode;
                m_Buffer.stationOperate.ManualAutoMode = ManualAutoMode;
                m_Feeder.stationOperate.ManualAutoMode = ManualAutoMode;
                m_Move.stationOperate.ManualAutoMode = ManualAutoMode;
                m_LeftC.stationOperate.ManualAutoMode = ManualAutoMode;
                m_LeftCut1.stationOperate.ManualAutoMode = ManualAutoMode;
                m_LeftCut2.stationOperate.ManualAutoMode = ManualAutoMode;
                m_RightCut1.stationOperate.ManualAutoMode = ManualAutoMode;
                m_RightCut2.stationOperate.ManualAutoMode = ManualAutoMode;
                m_Platform.stationOperate.ManualAutoMode = ManualAutoMode;
                m_Storing.stationOperate.ManualAutoMode = ManualAutoMode;

                m_Splice.stationOperate.AutoRun = MachineOperation.Running;
                m_Buffer.stationOperate.AutoRun = MachineOperation.Running;
                m_Feeder.stationOperate.AutoRun = MachineOperation.Running;
                m_Move.stationOperate.AutoRun = MachineOperation.Running;
                m_LeftC.stationOperate.AutoRun = MachineOperation.Running;
                m_LeftCut1.stationOperate.AutoRun = MachineOperation.Running;
                m_LeftCut2.stationOperate.AutoRun = MachineOperation.Running;
                m_RightCut1.stationOperate.AutoRun = MachineOperation.Running;
                m_RightCut2.stationOperate.AutoRun = MachineOperation.Running;
                m_Platform.stationOperate.AutoRun = MachineOperation.Running;
                m_Storing.stationOperate.AutoRun = MachineOperation.Running;


                m_Splice.stationInitialize.Run();
                m_Buffer.stationInitialize.Run();
                m_Feeder.stationInitialize.Run();
                m_Move.stationInitialize.Run();
                m_LeftC.stationInitialize.Run();
                m_LeftCut1.stationInitialize.Run();
                m_LeftCut2.stationInitialize.Run();
                m_RightCut1.stationInitialize.Run();
                m_RightCut2.stationInitialize.Run();
                m_Platform.stationInitialize.Run();
                m_Storing.stationInitialize.Run();

                m_Splice.stationOperate.Run();
                m_Feeder.stationOperate.Run();
                m_Buffer.stationOperate.Run();
                m_Move.stationOperate.Run();
                m_LeftC.stationOperate.Run();
                m_LeftCut1.stationOperate.Run();
                m_LeftCut2.stationOperate.Run();
                m_RightCut1.stationOperate.Run();
                m_RightCut2.stationOperate.Run();
                m_Platform.stationOperate.Run();
                m_Storing.stationOperate.Run();

                //周期停止
                if (Marking.SystemStop && m_Platform.stationOperate.step == 265 && m_Buffer.DownCylinder.OutOriginStatus
                    && m_Buffer.LeftCylinder.OutOriginStatus)
                {
                    MachineOperation.Start = false;
                    MachineOperation.Pause = true;
                    Marking.SystemStop = false;
                    btnStart.BackColor = Color.Green;
                    btnStop.BackColor = Color.Red;
                }
                //抽检停止
                if (Marking.SelectCheckMode && Marking.SelectCheckModeFinish)
                {
                    Marking.SelectCheckMode = false;
                    Marking.SelectCheckModeFinish = false;
                }                
                #endregion

                #region 急停中
                if (!EstopButton.PressedIO.Value)
                {
                    m_LeftC.C1Axis.Stop();
                    m_LeftC.C2Axis.Stop();
                    m_LeftC.C3Axis.Stop();
                    m_LeftC.C4Axis.Stop();
                    m_LeftCut1.CutAxis.Stop();
                    m_LeftCut2.CutAxis.Stop();
                    m_RightCut1.CutAxis.Stop();
                    m_RightCut2.CutAxis.Stop();
                    m_Platform.Xaxis.Stop();
                    m_Platform.Yaxis.Stop();
                    m_Platform.Zaxis.Stop();
                    m_Storing.MAxis.Stop();

                    m_Splice.stationInitialize.InitializeDone = false;
                    m_Buffer.stationInitialize.InitializeDone = false;
                    m_Move.stationInitialize.InitializeDone = false;
                    m_LeftC.stationInitialize.InitializeDone = false;
                    m_LeftCut1.stationInitialize.InitializeDone = false;
                    m_LeftCut2.stationInitialize.InitializeDone = false;
                    m_RightCut1.stationInitialize.InitializeDone = false;
                    m_RightCut2.stationInitialize.InitializeDone = false;
                    m_Platform.stationInitialize.InitializeDone = false;
                    m_Storing.stationInitialize.InitializeDone = true;
                    m_Feeder.stationInitialize.InitializeDone = false;
                    IoPoints.T2DO10.Value = false; //关闭碎料电机

                    if (LeftCIsAlarm.IsAlarm && LeftCut1IsAlarm.IsAlarm && LeftCut2IsAlarm.IsAlarm && RightCut1IsAlarm.IsAlarm
                        && RightCut2IsAlarm.IsAlarm && PlateformIsAlarm.IsAlarm)
                    {
                        IoPoints.T1DO0.Value = false;
                        IoPoints.T1DO1.Value = false;
                        IoPoints.T1DO2.Value = false;
                        IoPoints.T2DO3.Value = false;
                        IoPoints.T2DO4.Value = false;
                        IoPoints.T1DO5.Value = false;
                        IoPoints.T1DO6.Value = false;
                        IoPoints.T1DO7.Value = false;
                        IoPoints.T1DO8.Value = false;
                        IoPoints.T1DO9.Value = false;
                        IoPoints.T1DO10.Value = false;
                        IoPoints.T1DO11.Value = false;
                        IoPoints.T1DO12.Value = false;
                        IoPoints.T1DO13.Value = false;
                        IoPoints.T1DO14.Value = false;
                        IoPoints.T1DO15.Value = false;
                        IoPoints.T1DO16.Value = false;
                        IoPoints.T1DO17.Value = false;
                        IoPoints.T1DO18.Value = false;
                        IoPoints.T1DO19.Value = false;
                        IoPoints.T1DO20.Value = false;
                        IoPoints.T1DO21.Value = false;
                        IoPoints.T1DO22.Value = false;
                        IoPoints.T1DO23.Value = false;
                        IoPoints.T2DO0.Value = false;
                        IoPoints.T2DO1.Value = false;
                        IoPoints.T2DO2.Value = false;
                        IoPoints.T2DO3.Value = false;
                        IoPoints.T2DO4.Value = false;
                        IoPoints.T2DO5.Value = false;
                        IoPoints.T2DO6.Value = false;
                        IoPoints.T2DO7.Value = false;
                        IoPoints.T2DO8.Value = false;
                        IoPoints.T2DO9.Value = false;
                        IoPoints.T2DO10.Value = false;
                        IoPoints.T2DO11.Value = false;
                        IoPoints.T2DO12.Value = false;

                        MessageBox.Show("设备电源异常，请务必关闭软件！");
                        this.Invoke(new Action(() => this.Close()));
                    }
                    MachineOperation.IniliazieDone = false;
                }
                #endregion

                #region 设备复位中
                if (MachineOperation.Resetting)
                {
                    switch (MachineOperation.Flow)
                    {
                        case 0:
                            MachineOperation.IniliazieDone = false;
                            m_External.InitializingDone = false;
                            m_Splice.stationInitialize.InitializeDone = false;
                            m_Buffer.stationInitialize.InitializeDone = false;
                            m_Feeder.stationInitialize.InitializeDone = false;
                            m_Move.stationInitialize.InitializeDone = false;
                            m_LeftC.stationInitialize.InitializeDone = false;
                            m_LeftCut1.stationInitialize.InitializeDone = false;
                            m_LeftCut2.stationInitialize.InitializeDone = false;
                            m_RightCut1.stationInitialize.InitializeDone = false;
                            m_RightCut2.stationInitialize.InitializeDone = false;
                            m_Platform.stationInitialize.InitializeDone = false;
                            m_Storing.stationInitialize.InitializeDone = false;
                            m_Splice.stationInitialize.Start = false;
                            m_Buffer.stationInitialize.Start = false;
                            m_Feeder.stationInitialize.Start = false;
                            m_Move.stationInitialize.Start = false;
                            m_LeftC.stationInitialize.Start = false;                          
                            m_LeftCut1.stationInitialize.Start = false;
                            m_LeftCut2.stationInitialize.Start = false;
                            m_RightCut1.stationInitialize.Start = false;
                            m_RightCut2.stationInitialize.Start = false;
                            m_Platform.stationInitialize.Start = false;
                            m_Storing.stationInitialize.Start = false;
                            m_Splice.stationInitialize.Flow = 0;
                            m_Buffer.stationInitialize.Flow = 0;
                            m_Feeder.stationInitialize.Flow = 0;
                            m_Move.stationInitialize.Flow = 0;
                            m_LeftC.stationInitialize.Flow = 0;
                            m_LeftCut1.stationInitialize.Flow = 0;
                            m_LeftCut2.stationInitialize.Flow = 0;
                            m_RightCut1.stationInitialize.Flow = 0;
                            m_RightCut2.stationInitialize.Flow = 0;
                            m_Platform.stationInitialize.Flow = 0;
                            m_Storing.stationInitialize.Flow = 0;                         
                            IoPoints.T1DO0.Value = false;
                            IoPoints.T1DO1.Value = false;
                            m_Buffer.stationInitialize.Start = true;
                            m_Move.stationInitialize.Start = true;
                            m_LeftC.stationInitialize.Start = true;
                            m_Platform.stationInitialize.Start = true;
                            m_LeftCut1.stationInitialize.Start = true;
                            m_LeftCut2.stationInitialize.Start = true;
                            m_RightCut1.stationInitialize.Start = true;
                            m_RightCut2.stationInitialize.Start = true;                          
                            MachineOperation.Flow = 10;
                            break;
                        case 10:
                            if (m_Buffer.stationInitialize.Running  && m_Move.stationInitialize.Running && m_LeftC.stationInitialize.Running 
                                && m_Platform.stationInitialize.Running && m_LeftCut1.stationInitialize.Running && m_LeftCut2.stationInitialize.Running 
                                && m_RightCut1.stationInitialize.Running && m_RightCut2.stationInitialize.Running)
                            {
                                MachineOperation.Flow = 20;
                            }
                            break;
                        case 20:
                            if (m_Buffer.stationInitialize.Flow == -1 || m_Move.stationInitialize.Flow == -1 || m_LeftC.stationInitialize.Flow == -1 
                                || m_Platform.stationInitialize.Flow == -1 || m_LeftCut1.stationInitialize.Flow == -1 || m_LeftCut2.stationInitialize.Flow == -1 
                                || m_RightCut1.stationInitialize.Flow == -1 || m_RightCut2.stationInitialize.Flow == -1)
                            {
                                MachineOperation.IniliazieDone = false;
                                MachineOperation.Reset = false;
                                MachineOperation.Stop = true;
                                MachineOperation.Flow = 0;
                            }
                            else if (m_Buffer.stationInitialize.InitializeDone && m_Move.stationInitialize.InitializeDone && m_LeftC.stationInitialize.InitializeDone
                                && m_Platform.stationInitialize.InitializeDone && m_LeftCut1.stationInitialize.InitializeDone && m_LeftCut2.stationInitialize.InitializeDone
                                && m_RightCut1.stationInitialize.InitializeDone && m_RightCut2.stationInitialize.InitializeDone)
                            {
                                m_Splice.stationInitialize.Start = true;
                                m_Feeder.stationInitialize.Start = true;
                                MachineOperation.Flow = 30;
                            }
                            break;
                        case 30:                         
                            if (m_Splice.stationInitialize.Running && m_Feeder.stationInitialize.Running)
                            {
                                MachineOperation.Flow = 40;
                            }
                            break;
                        case 40:
                            if (m_Splice.stationInitialize.Flow == -1 || m_Feeder.stationInitialize.Flow == -1)
                            {
                                MachineOperation.IniliazieDone = false;
                                MachineOperation.Reset = false;
                                MachineOperation.Stop = true;
                                MachineOperation.Flow = 0;
                            }
                            else if(m_Splice.stationInitialize.InitializeDone && m_Feeder.stationInitialize.InitializeDone)
                            {
                                MachineOperation.Flow = 50;
                            }
                            break;
                        case 50:
                            if (m_Storing.stationInitialize.Running)
                            {
                                MachineOperation.Flow = 60;
                            }
                            break;
                        case 60:
                            if(m_Storing.stationInitialize.Flow == -1)
                            {
                                MachineOperation.IniliazieDone = false;
                                MachineOperation.Reset = false;
                                MachineOperation.Stop = true;
                                MachineOperation.Flow = 0;
                            }
                            else if (m_Storing.stationInitialize.InitializeDone)
                            {
                                MachineOperation.IniliazieDone = true;
                                MachineOperation.Flow = 70;
                            }
                            break;
                        default:
                            MachineOperation.IniliazieDone = false;
                            m_External.InitializingDone = false;                          
                            m_Buffer.stationInitialize.InitializeDone = false;
                            m_Feeder.stationInitialize.InitializeDone = false;
                            m_Move.stationInitialize.InitializeDone = false;
                            m_LeftC.stationInitialize.InitializeDone = false;
                            m_LeftCut1.stationInitialize.InitializeDone = false;
                            m_LeftCut2.stationInitialize.InitializeDone = false;
                            m_RightCut1.stationInitialize.InitializeDone = false;
                            m_RightCut2.stationInitialize.InitializeDone = false;
                            m_Platform.stationInitialize.InitializeDone = false;
                            m_Splice.stationInitialize.InitializeDone = false;
                            m_Storing.stationInitialize.InitializeDone = false;
                            break;
                    }
                }

                #endregion

                #region 回待机位中
                if(Marking.equipmentHomeWaitState[0] && Marking.equipmentHomeWaitState[1] && Marking.equipmentHomeWaitState[2]
                    && Marking.equipmentHomeWaitState[3] && Marking.equipmentHomeWaitState[4] && Marking.equipmentHomeWaitState[5]
                    && Marking.equipmentHomeWaitState[6] && Marking.equipmentHomeWaitState[7] && Marking.equipmentHomeWaitState[8]
                    && Marking.equipmentHomeWaitState[9])
                {
                    m_Splice.homeWaitStep = 0;
                    m_Buffer.homeWaitStep = 0;
                    m_Feeder.homeWaitStep = 0;
                    m_Move.homeWaitStep = 0;
                    m_LeftC.homeWaitStep = 0;
                    m_LeftCut1.homeWaitStep = 0;
                    m_LeftCut2.homeWaitStep = 0;
                    m_RightCut1.homeWaitStep = 0;
                    m_RightCut2.homeWaitStep = 0;
                    m_Platform.homeWaitStep = 0;
                    Marking.equipmentHomeWaitState[0] = false;
                    Marking.equipmentHomeWaitState[1] = false;
                    Marking.equipmentHomeWaitState[2] = false;
                    Marking.equipmentHomeWaitState[3] = false;
                    Marking.equipmentHomeWaitState[4] = false;
                    Marking.equipmentHomeWaitState[5] = false;
                    Marking.equipmentHomeWaitState[6] = false;
                    Marking.equipmentHomeWaitState[7] = false;
                    Marking.equipmentHomeWaitState[8] = false;
                    Marking.equipmentHomeWaitState[9] = false;
                    m_External.GoRristatus = false;
                    m_External.GoRriDone = true;
                }
                #endregion

                #region 设备停止中

                if (MachineOperation.Stopping)
                {
                    m_LeftC.C1Axis.Stop();
                    m_LeftC.C2Axis.Stop();
                    m_LeftC.C3Axis.Stop();
                    m_LeftC.C4Axis.Stop();
                    m_LeftCut1.CutAxis.Stop();
                    m_LeftCut2.CutAxis.Stop();
                    m_RightCut1.CutAxis.Stop();
                    m_RightCut2.CutAxis.Stop();
                    m_Platform.Xaxis.Stop();
                    m_Platform.Yaxis.Stop();
                    m_Platform.Zaxis.Stop();
                    m_Storing.MAxis.Stop();

                    m_Splice.stationInitialize.Estop = true;
                    m_Buffer.stationInitialize.Estop = true;
                    m_Feeder.stationInitialize.Estop = true;
                    m_Move.stationInitialize.Estop = true;
                    m_LeftC.stationInitialize.Estop = true;
                    m_LeftCut1.stationInitialize.Estop = true;
                    m_LeftCut2.stationInitialize.Estop = true;
                    m_RightCut1.stationInitialize.Estop = true;
                    m_RightCut2.stationInitialize.Estop = true;
                    m_Platform.stationInitialize.Estop = true;
                    m_Storing.stationInitialize.Estop = true;
                    if (!m_Splice.stationInitialize.Running && !m_Buffer.stationInitialize.Running 
                        && !m_Feeder.stationInitialize.Running && !m_Move.stationInitialize.Running
                        && !m_LeftC.stationInitialize.Running && !m_LeftCut1.stationInitialize.Running 
                        && !m_LeftCut2.stationInitialize.Running && !m_RightCut1.stationInitialize.Running 
                        && !m_RightCut2.stationInitialize.Running && !m_Platform.stationInitialize.Running 
                        && !m_Storing.stationInitialize.Running)
                    {
                        MachineOperation.IniliazieDone = false;
                        MachineOperation.Stopping = false;
                        m_Splice.stationInitialize.Estop = false;
                        m_Buffer.stationInitialize.Estop = false;
                        m_Feeder.stationInitialize.Estop = false;
                        m_Move.stationInitialize.Estop = false;
                        m_LeftC.stationInitialize.Estop = false;
                        m_LeftCut1.stationInitialize.Estop = false;
                        m_LeftCut2.stationInitialize.Estop = false;
                        m_RightCut1.stationInitialize.Estop = false;
                        m_RightCut2.stationInitialize.Estop = false;
                        m_Platform.stationInitialize.Estop = false;
                        m_Storing.stationInitialize.Estop = false;
                    }
                }
                #endregion
            }
        }

        #region 故障报警

        private void AlarmCheck()
        {
            while (true)
            {
                Thread.Sleep(50);
                SpliceIsAlarm = AlarmCheck(m_Splice.Alarms);
                BufferIsAlarm = AlarmCheck(m_Buffer.Alarms);
                FeederIsAlarm = AlarmCheck(m_Feeder.Alarms);
                MoveIsAlarm = AlarmCheck(m_Move.Alarms);
                LeftCIsAlarm = AlarmCheck(m_LeftC.Alarms);
                LeftCut1IsAlarm = AlarmCheck(m_LeftCut1.Alarms);
                LeftCut2IsAlarm = AlarmCheck(m_LeftCut2.Alarms);
                RightCut1IsAlarm = AlarmCheck(m_RightCut1.Alarms);
                RightCut2IsAlarm = AlarmCheck(m_RightCut2.Alarms);
                PlateformIsAlarm = AlarmCheck(m_Platform.Alarms);
                StoringIsAlarm = AlarmCheck(m_Storing.Alarms);
                MachineIsAlarm = AlarmCheck(MachineAlarms);
            }
        }

        public AlarmType AlarmCheck(IList<Alarm> Alarms)
        {
            var Alarm = new AlarmType();
            foreach (Alarm alarm in Alarms)
            {
                var AlarmNum = 0;
                var btemp = alarm.IsAlarm;
                if (alarm.AlarmLevel == AlarmLevels.Error)
                {
                    Alarm.IsAlarm |= btemp;
                    this.Invoke(new Action(() =>
                    {
                        if("控制卡网络掉线" == alarm.Name)
                        {
                            var Fau = Global.FaultDictionary[alarm.Name];
                            Fau.FaultMessage = alarm.Name + ",错误返回值:" + IoPoints.ApsController.NetworkError().ToString() + IoPoints.ApsController.NetworkErrorNode();
                            Msg(string.Format("{0},{1}", alarm.AlarmLevel.ToString(), Fau.FaultMessage), btemp);
                        }
                        else
                        {
                            Msg(string.Format("{0},{1}", alarm.AlarmLevel.ToString(), alarm.Name), btemp);
                        }        
                        if (btemp)
                        {
                            if (AlarmsName.Count <= 0)
                            {
                                AlarmsName.Add(alarm.Name);
                                if (Global.FaultDictionary.ContainsKey(alarm.Name))
                                {
                                    var Fau = Global.FaultDictionary[alarm.Name];
                                    Fau.FaultCount++;
                                }
                                Global.AlarmsFault.Add(new Fault1()
                                {
                                    FaultTime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"),
                                    FaultCode = Global.FaultDictionary[alarm.Name].FaultCode,
                                    FaultMessage = Global.FaultDictionary[alarm.Name].FaultMessage,
                                    FaultCount = Global.FaultDictionary[alarm.Name].FaultCount
                                });
                                if (Global.AlarmsFault.Count > 1000)
                                {
                                    Global.AlarmsFault.Clear();
                                }
                            }
                            else
                            {
                                foreach (string AlarmName in AlarmsName)
                                {
                                    if (AlarmName == alarm.Name)
                                    {
                                        AlarmNum++;
                                    }
                                }
                                if(0 == AlarmNum)
                                {
                                    AlarmsName.Add(alarm.Name);
                                    if (Global.FaultDictionary.ContainsKey(alarm.Name))
                                    {
                                        var Fau = Global.FaultDictionary[alarm.Name];
                                        Fau.FaultCount++;
                                    }
                                    Global.AlarmsFault.Add(new Fault1()
                                    {
                                        FaultTime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"),
                                        FaultCode = Global.FaultDictionary[alarm.Name].FaultCode,
                                        FaultMessage = Global.FaultDictionary[alarm.Name].FaultMessage,
                                        FaultCount = Global.FaultDictionary[alarm.Name].FaultCount
                                    });
                                    if(Global.AlarmsFault.Count > 1000)
                                    {
                                        Global.AlarmsFault.Clear();
                                    }
                                }
                            }
                        }
                        else if(AlarmsName.Count > 0)
                        {
                            foreach (string AlarmName in AlarmsName)
                            {
                                if (AlarmName == alarm.Name)
                                {
                                    AlarmsName.Remove(AlarmName);
                                    break;
                                }
                            }
                        }
                    }));
                }
                else if (alarm.AlarmLevel == AlarmLevels.None)
                {
                    Alarm.IsPrompt |= btemp;
                    this.Invoke(new Action(() =>
                    {
                        Msg(string.Format("{0},{1}", alarm.AlarmLevel.ToString(), alarm.Name), btemp);
                    }));
                }
                else
                {
                    Alarm.IsWarning |= btemp;
                    this.Invoke(new Action(() =>
                    {
                        Msg(string.Format("{0},{1}", alarm.AlarmLevel.ToString(), alarm.Name), btemp);
                    }));
                }
            }
            return Alarm;
        }

        private List<Alarm> MachineAlarms;
        public void AddAlarms()
        {
            MachineAlarms = new List<Alarm>();

            MachineAlarms.Add(new Alarm(() => !IoPoints.T3IN2.Value)
            {
                AlarmLevel = AlarmLevels.Error,
                Name = "急停按钮已按下，注意安全"
            });
            MachineAlarms.Add(new Alarm(() => IoPoints.T3IN8.Value)
            {
                AlarmLevel = AlarmLevels.Error,
                Name = "未检测到气压信号"
            });
            MachineAlarms.Add(new Alarm(() => !m_Splice.stationInitialize.InitializeDone)
            {
                AlarmLevel = AlarmLevels.Warrning,
                Name = "接料模组未回原点"
            });
            MachineAlarms.Add(new Alarm(() => !m_Buffer.stationInitialize.InitializeDone)
            {
                AlarmLevel = AlarmLevels.Warrning,
                Name = "缓冲模组未回原点"
            });
            MachineAlarms.Add(new Alarm(() => !m_Feeder.stationInitialize.InitializeDone)
            {
                AlarmLevel = AlarmLevels.Warrning,
                Name = "进料模组未回原点"
            });
            MachineAlarms.Add(new Alarm(() => !m_Move.stationInitialize.InitializeDone)
            {
                AlarmLevel = AlarmLevels.Warrning,
                Name = "移动模组未回原点"
            });
            MachineAlarms.Add(new Alarm(() => !m_LeftC.stationInitialize.InitializeDone)
            {
                AlarmLevel = AlarmLevels.Warrning,
                Name = "C轴模组未回原点"
            });
            MachineAlarms.Add(new Alarm(() => !m_LeftCut1.stationInitialize.InitializeDone)
            {
                AlarmLevel = AlarmLevels.Warrning,
                Name = "剪切1轴模组未回原点"
            });
            MachineAlarms.Add(new Alarm(() => !m_LeftCut2.stationInitialize.InitializeDone)
            {
                AlarmLevel = AlarmLevels.Warrning,
                Name = "剪切2轴模组未回原点"
            });
            MachineAlarms.Add(new Alarm(() => !m_RightCut1.stationInitialize.InitializeDone)
            {
                AlarmLevel = AlarmLevels.Warrning,
                Name = "剪切3轴模组未回原点"
            });
            MachineAlarms.Add(new Alarm(() => !m_RightCut2.stationInitialize.InitializeDone)
            {
                AlarmLevel = AlarmLevels.Warrning,
                Name = "剪切4轴模组未回原点"
            });
            MachineAlarms.Add(new Alarm(() => !m_Platform.stationInitialize.InitializeDone)
            {
                AlarmLevel = AlarmLevels.Warrning,
                Name = "XYZ模组未回原点"
            });
            MachineAlarms.Add(new Alarm(() => !m_Storing.stationInitialize.InitializeDone)
            {
                AlarmLevel = AlarmLevels.Warrning,
                Name = "仓储模组未回原点"
            });
            MachineAlarms.Add(new Alarm(() => m_Storing.MAxis.IsPEL || m_Storing.MAxis.IsMEL)
            {
                AlarmLevel = AlarmLevels.Error,
                Name = "M轴感应到限位"
            });
            MachineAlarms.Add(new Alarm(() => m_LeftC.Push1Axis.IsPEL || m_LeftC.Push1Axis.IsMEL)
            {
                AlarmLevel = AlarmLevels.Warrning,
                Name = "推进1轴感应到限位"
            });
            MachineAlarms.Add(new Alarm(() => m_LeftC.Push2Axis.IsPEL || m_LeftC.Push2Axis.IsMEL)
            {
                AlarmLevel = AlarmLevels.Warrning,
                Name = "推进2轴感应到限位"
            });
            MachineAlarms.Add(new Alarm(() => m_LeftC.Push3Axis.IsPEL || m_LeftC.Push3Axis.IsMEL)
            {
                AlarmLevel = AlarmLevels.Warrning,
                Name = "推进3轴感应到限位"
            });
            MachineAlarms.Add(new Alarm(() => m_LeftC.Push4Axis.IsPEL || m_LeftC.Push4Axis.IsMEL)
            {
                AlarmLevel = AlarmLevels.Warrning,
                Name = "推进4轴感应到限位"
            });
            MachineAlarms.Add(new Alarm(() => m_Platform.Xaxis.IsPEL || m_Platform.Xaxis.IsMEL)
            {
                AlarmLevel = AlarmLevels.Error,
                Name = "X轴感应到限位"
            });
            MachineAlarms.Add(new Alarm(() => m_Platform.Yaxis.IsPEL || m_Platform.Yaxis.IsMEL)
            {
                AlarmLevel = AlarmLevels.Error,
                Name = "Y轴感应到限位"
            });
            MachineAlarms.Add(new Alarm(() => m_Platform.Zaxis.IsPEL || m_Platform.Zaxis.IsMEL)
            {
                AlarmLevel = AlarmLevels.Error,
                Name = "Z轴感应到限位"
            });
            MachineAlarms.Add(new Alarm(() => m_LeftCut1.CutAxis.IsPEL || m_LeftCut1.CutAxis.IsMEL)
            {
                AlarmLevel = AlarmLevels.Error,
                Name = "剪切1#轴感应到限位"
            });
            MachineAlarms.Add(new Alarm(() => m_LeftCut2.CutAxis.IsPEL || m_LeftCut2.CutAxis.IsMEL)
            {
                AlarmLevel = AlarmLevels.Error,
                Name = "剪切2轴感应到限位"
            });
            MachineAlarms.Add(new Alarm(() => m_RightCut1.CutAxis.IsPEL || m_RightCut1.CutAxis.IsMEL)
            {
                AlarmLevel = AlarmLevels.Error,
                Name = "剪切3轴感应到限位"
            });
            MachineAlarms.Add(new Alarm(() => m_RightCut2.CutAxis.IsPEL || m_RightCut2.CutAxis.IsMEL)
            {
                AlarmLevel = AlarmLevels.Error,
                Name = "剪切4轴感应到限位"
            });
            MachineAlarms.Add(new Alarm(() => m_LeftC.C1Axis.IsPEL || m_LeftC.C1Axis.IsMEL)
            {
                AlarmLevel = AlarmLevels.Error,
                Name = "C1#轴感应到限位"
            });
            MachineAlarms.Add(new Alarm(() => m_LeftC.C2Axis.IsPEL || m_LeftC.C2Axis.IsMEL)
            {
                AlarmLevel = AlarmLevels.Error,
                Name = "C2#轴感应到限位"
            });
            MachineAlarms.Add(new Alarm(() => m_LeftC.C3Axis.IsPEL || m_LeftC.C3Axis.IsMEL)
            {
                AlarmLevel = AlarmLevels.Error,
                Name = "C3#轴感应到限位"
            });
            MachineAlarms.Add(new Alarm(() => m_LeftC.C4Axis.IsPEL || m_LeftC.C4Axis.IsMEL)
            {
                AlarmLevel = AlarmLevels.Error,
                Name = "C4#轴感应到限位"
            });
            MachineAlarms.Add(new Alarm(() => !Marking.DoorSafeSensorSheild && !IoPoints.T3IN5.Value)
            {
                AlarmLevel = AlarmLevels.Error,
                Name = "左安全门已打开"
            });
            MachineAlarms.Add(new Alarm(() => !Marking.DoorSafeSensorSheild && !IoPoints.T3IN6.Value)
            {
                AlarmLevel = AlarmLevels.Error,
                Name = "右安全门已打开"
            });
            MachineAlarms.Add(new Alarm(() => !IoPoints.T3IN7.Value)
            {
                AlarmLevel = AlarmLevels.Warrning,
                Name = "光幕已感应"
            });
            MachineAlarms.Add(new Alarm(() => Config.Instance.CutaxisCount[0] > Config.Instance.CaxisCountSet[0])
            {
                AlarmLevel = AlarmLevels.Error,
                Name = "剪切4数量达到请更换刀"
            });
            MachineAlarms.Add(new Alarm(() => Config.Instance.CutaxisCount[1] > Config.Instance.CaxisCountSet[1])
            {
                AlarmLevel = AlarmLevels.Error,
                Name = "剪切3数量达到请更换刀"
            });
            MachineAlarms.Add(new Alarm(() => Config.Instance.CutaxisCount[2] > Config.Instance.CaxisCountSet[2])
            {
                AlarmLevel = AlarmLevels.Error,
                Name = "剪切2数量达到请更换刀"
            });
            MachineAlarms.Add(new Alarm(() => Config.Instance.CutaxisCount[3] > Config.Instance.CaxisCountSet[3])
            {
                AlarmLevel = AlarmLevels.Error,
                Name = "剪切1数量达到请更换刀"
            });
            MachineAlarms.Add(new Alarm(() => 0 != IoPoints.ApsController.NetworkError())
            {
                AlarmLevel = AlarmLevels.Error,
                Name = "控制卡网络掉线"
            });
            if (Config.Instance.TemperatureAlarmState[0])
            {
                MachineAlarms.Add(new Alarm(() => (Marking.CurrTemperatureValue[0] > (Config.Instance.TemperatureValue[0] + Config.Instance.TemperatureAlarmLimit[0])) ||
                (Marking.CurrTemperatureValue[0] < (Config.Instance.TemperatureValue[0] + Config.Instance.TemperatureAlarmLimit[4])))
                {
                    AlarmLevel = AlarmLevels.Error,
                    Name = "剪刀1#上温度报警"
                });
                MachineAlarms.Add(new Alarm(() => (Marking.CurrTemperatureValue[1] > (Config.Instance.TemperatureValue[4] + Config.Instance.TemperatureAlarmLimit[0])) ||
                (Marking.CurrTemperatureValue[1] < (Config.Instance.TemperatureValue[4] + Config.Instance.TemperatureAlarmLimit[4])))
                {
                    AlarmLevel = AlarmLevels.Error,
                    Name = "剪刀1#下温度报警"
                });
            }
            if (Config.Instance.TemperatureAlarmState[1])
            {
                MachineAlarms.Add(new Alarm(() => (Marking.CurrTemperatureValue[2] > (Config.Instance.TemperatureValue[1] + Config.Instance.TemperatureAlarmLimit[1])) ||
                (Marking.CurrTemperatureValue[2] < (Config.Instance.TemperatureValue[1] + Config.Instance.TemperatureAlarmLimit[5])))
                {
                    AlarmLevel = AlarmLevels.Error,
                    Name = "剪刀2#上温度报警"
                });
                MachineAlarms.Add(new Alarm(() => (Marking.CurrTemperatureValue[3] > (Config.Instance.TemperatureValue[5] + Config.Instance.TemperatureAlarmLimit[1])) ||
                (Marking.CurrTemperatureValue[3] < (Config.Instance.TemperatureValue[5] + Config.Instance.TemperatureAlarmLimit[5])))
                {
                    AlarmLevel = AlarmLevels.Error,
                    Name = "剪刀2#下温度报警"
                });
            }
            if (Config.Instance.TemperatureAlarmState[2])
            {
                MachineAlarms.Add(new Alarm(() => (Marking.CurrTemperatureValue[4] > (Config.Instance.TemperatureValue[2] + Config.Instance.TemperatureAlarmLimit[2])) ||
                (Marking.CurrTemperatureValue[4] < (Config.Instance.TemperatureValue[2] + Config.Instance.TemperatureAlarmLimit[6])))
                {
                    AlarmLevel = AlarmLevels.Error,
                    Name = "剪刀3#上温度报警"
                });
                MachineAlarms.Add(new Alarm(() => (Marking.CurrTemperatureValue[5] > (Config.Instance.TemperatureValue[6] + Config.Instance.TemperatureAlarmLimit[2])) ||
                (Marking.CurrTemperatureValue[5] < (Config.Instance.TemperatureValue[6] + Config.Instance.TemperatureAlarmLimit[6])))
                {
                    AlarmLevel = AlarmLevels.Error,
                    Name = "剪刀3#下温度报警"
                });
            }
            if (Config.Instance.TemperatureAlarmState[3])
            {
                MachineAlarms.Add(new Alarm(() => (Marking.CurrTemperatureValue[6] > (Config.Instance.TemperatureValue[3] + Config.Instance.TemperatureAlarmLimit[3])) ||
                (Marking.CurrTemperatureValue[6] < (Config.Instance.TemperatureValue[3] + Config.Instance.TemperatureAlarmLimit[7])))
                {
                    AlarmLevel = AlarmLevels.Error,
                    Name = "剪刀4#上温度报警"
                });
                MachineAlarms.Add(new Alarm(() => (Marking.CurrTemperatureValue[7] > (Config.Instance.TemperatureValue[7] + Config.Instance.TemperatureAlarmLimit[3])) ||
                (Marking.CurrTemperatureValue[7] < (Config.Instance.TemperatureValue[7] + Config.Instance.TemperatureAlarmLimit[7])))
                {
                    AlarmLevel = AlarmLevels.Error,
                    Name = "剪刀4#下温度报警"
                });
            }
            MachineAlarms.Add(new Alarm(() => 1 == Position.Instance.FragmentationMode && (!IoPoints.T3IN15.Value || Marking.MotorOverloadSign) && Position.Instance.CheckOverload)
            {
                AlarmLevel = AlarmLevels.Error,
                Name = "碎料电机过载报警"
            });
            MachineAlarms.Add(new Alarm(() => !hasp.LicenseIsOK && !LicenseSheild && hasp.Duetime <= 0)
            {
                AlarmLevel = AlarmLevels.Error,
                Name = "该软件为试用软件，现已到期，或加密狗已拔出，请尽快联系厂商！"
            });
            MachineAlarms.Add(new Alarm(() => !hasp.LicenseIsOK && !LicenseSheild && hasp.Duetime > 0)
            {
                AlarmLevel = AlarmLevels.Error,
                Name = "加密狗无法授权，请检查加密狗或联系厂商！"
            });
        }
        #endregion

        #region 气缸状态刷新
        private void StatusCheck()
        {
            var list = new List<ICylinderStatusJugger>();
            m_Splice.stationInitialize.Estop = false;
            m_Buffer.stationInitialize.Estop = false;
            m_Feeder.stationInitialize.Estop = false;
            m_Move.stationInitialize.Estop = false;
            m_LeftC.stationInitialize.Estop = false;
            m_LeftCut1.stationInitialize.Estop = false;
            m_LeftCut2.stationInitialize.Estop = false;
            m_RightCut1.stationInitialize.Estop = false;
            m_RightCut2.stationInitialize.Estop = false;
            m_Platform.stationInitialize.Estop = false;
            m_Storing.stationInitialize.Estop = false;
            list.AddRange(m_Buffer.CylinderStatus);
            list.AddRange(m_Splice.CylinderStatus);
            list.AddRange(m_Feeder.CylinderStatus);
            list.AddRange(m_Move.CylinderStatus);
            list.AddRange(m_LeftC.CylinderStatus);
            list.AddRange(m_LeftCut1.CylinderStatus);
            list.AddRange(m_LeftCut2.CylinderStatus);
            list.AddRange(m_RightCut1.CylinderStatus);
            list.AddRange(m_RightCut2.CylinderStatus);
            list.AddRange(m_Platform.CylinderStatus);
            list.AddRange(m_Storing.CylinderStatus);
            while (true)
            {
                Thread.Sleep(100);
                foreach (var lst in list)
                {
                    lst.StatusJugger();
                }
            }
        }

        #endregion

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Enabled = false;
            this.Text = hasp.Nature ? "剪切机设备控制系统(0A02-10)" : $"剪切机设备控制系统(0A02-10) -- 到期还剩{hasp.Duetime}天";
            switch (Config.Instance.userLevel)
            {
                case UserLevel.操作员:
                    cmbOpation.SelectedIndex = 0;
                    break;
                case UserLevel.工程师:
                    cmbOpation.SelectedIndex = 1;
                    break;
                case UserLevel.设计者:
                    cmbOpation.SelectedIndex = 2;
                    break;
            }

            #region 主界面显示

            txtRunTimer.Text = Marking.CycleRunTime;
            txtUpTimer.Text = Marking.StartRunTime;
            txtlTrayFinishcount.Text = Config.Instance.TrayFinishcount.ToString();
            txtlFinishcount.Text = Config.Instance.Finishcount.ToString();
            txtlHoleFinishcount.Text = Config.Instance.HoleFinishCount.ToString();
            txtCut1Finishcount.Text = Config.Instance.CutaxisCount[0].ToString();
            txtCut2Finishcount.Text = Config.Instance.CutaxisCount[1].ToString();
            txtCut3Finishcount.Text = Config.Instance.CutaxisCount[2].ToString();
            txtCut4Finishcount.Text = Config.Instance.CutaxisCount[3].ToString();
            numModelCountSet.Value = (decimal)Position.Instance.ModelCountSet;
            numChangeTrayLayout.Value = (decimal)Position.Instance.ChangeTrayLayout;

            numBigTrayPos.Value = (decimal)(Global.BigTray.CurrentPos + 1);
            numYoungTrayPos.Value = (decimal)(Global.SmallTray.CurrentPos + 1);

            if (Marking.MSelectIndex)
            {
                textBox2.Text = 1.ToString();
            }
            else
            {
                textBox2.Text = Position.Instance.MLayerIndex.ToString();
            }

            //txtCut1CycTime.Text = Marking.watchCT1Value.ToString("0.00") + "s";
            //txtCut2CycTime.Text = Marking.watchCT2Value.ToString("0.00") + "s";
            //txtCut3CycTime.Text = Marking.watchCT3Value.ToString("0.00") + "s";
            //txtCut4CycTime.Text = Marking.watchCT4Value.ToString("0.00") + "s";
            //txtXYZCycTime.Text = Marking.watchXYZValue.ToString("0.00") + "s";
            txtModelCycTime.Text = Marking.watchModelValue.ToString("0.00") + "s";

            //换盘状态            
            numBigTrayPos.Enabled = !Marking.MancelChangeTray;
            numYoungTrayPos.Enabled = !Marking.MancelChangeTray;
            btnThrowerMode.Enabled = !Marking.MancelChangeTray;
            btnSelectCheckMode.Enabled = !Marking.MancelChangeTray;
            btnSystemStop.Enabled = !Marking.MancelChangeTray;
            #endregion

            #region  温度显示
            txtCut1UpTemperature.Text = (Marking.CurrTemperatureValue[0]).ToString() + "度";
            txtCut2UpTemperature.Text = (Marking.CurrTemperatureValue[1]).ToString() + "度";
            txtCut3UpTemperature.Text = (Marking.CurrTemperatureValue[2]).ToString() + "度";
            txtCut4UpTemperature.Text = (Marking.CurrTemperatureValue[3]).ToString() + "度";
            txtCut1DownTemperature.Text = (Marking.CurrTemperatureValue[4]).ToString() + "度";
            txtCut2DownTemperature.Text = (Marking.CurrTemperatureValue[5]).ToString() + "度";
            txtCut3DownTemperature.Text = (Marking.CurrTemperatureValue[6]).ToString() + "度";
            txtCut4DownTemperature.Text = (Marking.CurrTemperatureValue[7]).ToString() + "度";
            #endregion

            btnMachineStatus.Text = MachineOperation.Status.ToString();
            btnMachineStatus.ForeColor = MachineStatusColor(MachineOperation.Status);
            if (MachineOperation.Status == MachineStatus.设备运行中 && !Marking.MancelChangeTray && !Marking.SystemStop)
            {
                lblMachineStatus.Text = "设备运作中";
            }
            if (MachineOperation.Status == MachineStatus.设备报警中 && !Marking.MancelChangeTray && !Marking.SystemStop)
            {
                lblMachineStatus.Text = "设备报警中";
            }
            if (MachineOperation.Status == MachineStatus.设备复位中 && !Marking.MancelChangeTray && !Marking.SystemStop)
            {
                lblMachineStatus.Text = "设备复位中";
            }
            if ((MachineOperation.Status != MachineStatus.设备复位中) && (MachineOperation.Status != MachineStatus.设备报警中)
                && (MachineOperation.Status != MachineStatus.设备运行中) && !Marking.MancelChangeTray && !Marking.SystemStop)
            {
                lblMachineStatus.Text = "设备停止中";
            }
            if (Marking.MancelChangeTray)
            {
                lblMachineStatus.Text = "设备强制换盘中";
            }
            if (Marking.SystemStop)
            {
                lblMachineStatus.Text = "设备周期停止中";
            }

            btnSelectCheckMode.BackColor = Marking.SelectCheckMode ? Color.Red : Color.Green;
            btnChangeTray.BackColor = Marking.MancelChangeTray ? Color.Red : Color.Green;
            btnSystemStop.BackColor = Marking.SystemStop ? Color.Red : Color.Green;
            btnThrowerMode.BackColor = Marking.ThrowerMode ? Color.Red : Color.Green;

            lblProductType.Text = Config.Instance.CurrentProductType;
            picGreenLed.Image = IoPoints.T2DO23.Value ? Properties.Resources.LedGreen : Properties.Resources.LedNone;
            picYellowLed.Image = IoPoints.T2DO22.Value ? Properties.Resources.LedYellow : Properties.Resources.LedNone;
            picRedLed.Image = IoPoints.T2DO21.Value ? Properties.Resources.LedRed : Properties.Resources.LedNone;

            if (MachineOperation.Status == MachineStatus.设备运行中)
            {
                btnWorkParam.Visible = false;
                btnMaintain.Visible = false;
                btnOffces.Visible = false;
                btnTeam.Visible = false;
                btnSpeed.Visible = false;
                btnExit.Visible = true;
                btnIOMonitor.Visible = true;
                btnTricolorLamp.Visible = true;
                btnManual.Visible = false;
            }
            else
            {
                btnWorkParam.Visible = Config.Instance.UserL[0];
                btnMaintain.Visible = Config.Instance.UserL[4];
                btnOffces.Visible = Config.Instance.UserL[5];
                btnTeam.Visible = Config.Instance.UserL[7];
                btnSpeed.Visible = Config.Instance.UserL[6];
                btnExit.Visible = true;
                btnIOMonitor.Visible = Config.Instance.UserL[3];
                btnTricolorLamp.Visible = true;
                btnManual.Visible = true;
            }
            numYoungTrayPos.ReadOnly = ManualAutoMode ? true : false; //小盘手动可编辑，自动不能编辑

            btnLightOn.BackColor = IoPoints.T2DO5.Value ? Color.LightBlue : Color.Gray;
            btnLightOn.Text = IoPoints.T2DO5.Value ? "OFF" : "ON";
            btnTeamOpen1.BackColor = IoPoints.T2DO6.Value ? Color.LightBlue : Color.Gray;
            btnTeamOpen2.BackColor = IoPoints.T2DO7.Value ? Color.LightBlue : Color.Gray;
            btnTeamOpen3.BackColor = IoPoints.T2DO8.Value ? Color.LightBlue : Color.Gray;
            btnTeamOpen4.BackColor = IoPoints.T2DO9.Value ? Color.LightBlue : Color.Gray;
            reshing();

            if (Global.TrayDataRefresh)
            {
                InitTray();
                Global.TrayDataRefresh = false;
            }
            timer1.Enabled = true;
        }

        public void reshing()
        {
            lblXAxisName.Text = m_Platform.Xaxis.Name;
            lblXAxisCurPositon.Text = m_Platform.Xaxis.CurrentPos.ToString();
            lblXAxisBackPositon.Text = m_Platform.Xaxis.BackPos.ToString();
            btnReadyX.BackColor = m_Platform.Xaxis.IsServon ? Color.Green : Color.Gray;
            btnOriX.BackColor = m_Platform.Xaxis.IsOrign ? Color.Green : Color.Gray;
            btnMelX.BackColor = m_Platform.Xaxis.IsPEL ? Color.Green : Color.Gray;
            btnPelX.BackColor = m_Platform.Xaxis.IsMEL ? Color.Green : Color.Gray;
            btnEmgX.BackColor = m_Platform.Xaxis.IsEmg ? Color.Green : Color.Gray;
            btnDoneX.BackColor = m_Platform.Xaxis.IsINP ? Color.Green : Color.Gray;
            btnArmX.BackColor = m_Platform.Xaxis.IsAlarmed ? Color.Green : Color.Gray;
            btnErrX.BackColor = m_Platform.Xaxis.IsAlarmed ? Color.Green : Color.Gray;
            lblYAxisName.Text = m_Platform.Yaxis.Name;
            lblYAxisCurPositon.Text = m_Platform.Yaxis.CurrentPos.ToString();
            lblYAxisBackPositon.Text = m_Platform.Yaxis.BackPos.ToString();
            btnReadyY.BackColor = m_Platform.Yaxis.IsServon ? Color.Green : Color.Gray;
            btnOriY.BackColor = m_Platform.Yaxis.IsOrign ? Color.Green : Color.Gray;
            btnMelY.BackColor = m_Platform.Yaxis.IsPEL ? Color.Green : Color.Gray;
            btnPelY.BackColor = m_Platform.Yaxis.IsMEL ? Color.Green : Color.Gray;
            btnEmgY.BackColor = m_Platform.Yaxis.IsEmg ? Color.Green : Color.Gray;
            btnDoneY.BackColor = m_Platform.Yaxis.IsINP ? Color.Green : Color.Gray;
            btnArmY.BackColor = m_Platform.Yaxis.IsAlarmed ? Color.Green : Color.Gray;
            btnErrY.BackColor = m_Platform.Yaxis.IsAlarmed ? Color.Green : Color.Gray;

            lblZAxisName.Text = m_Platform.Zaxis.Name;
            lblZAxisCurPositon.Text = m_Platform.Zaxis.CurrentPos.ToString();
            lblZAxisBackPositon.Text = m_Platform.Zaxis.BackPos.ToString();
            btnReadyZ.BackColor = m_Platform.Zaxis.IsServon ? Color.Green : Color.Gray;
            btnOriZ.BackColor = m_Platform.Zaxis.IsOrign ? Color.Green : Color.Gray;
            btnMelZ.BackColor = m_Platform.Zaxis.IsPEL ? Color.Green : Color.Gray;
            btnPelZ.BackColor = m_Platform.Zaxis.IsMEL ? Color.Green : Color.Gray;
            btnEmgZ.BackColor = m_Platform.Zaxis.IsEmg ? Color.Green : Color.Gray;
            btnDoneZ.BackColor = m_Platform.Zaxis.IsINP ? Color.Green : Color.Gray;
            btnArmZ.BackColor = m_Platform.Zaxis.IsAlarmed ? Color.Green : Color.Gray;
            btnErrZ.BackColor = m_Platform.Zaxis.IsAlarmed ? Color.Green : Color.Gray;
            lblMAxisName.Text = m_Storing.MAxis.Name;
            lblMAxisCurPositon.Text = m_Storing.MAxis.CurrentPos.ToString();
            lblMAxisBackPositon.Text = m_Storing.MAxis.BackPos.ToString();
            btnReadyM.BackColor = m_Storing.MAxis.IsServon ? Color.Green : Color.Gray;
            btnOriM.BackColor = m_Storing.MAxis.IsOrign ? Color.Green : Color.Gray;
            btnMelM.BackColor = m_Storing.MAxis.IsPEL ? Color.Green : Color.Gray;
            btnPelM.BackColor = m_Storing.MAxis.IsMEL ? Color.Green : Color.Gray;
            btnEmgM.BackColor = m_Storing.MAxis.IsEmg ? Color.Green : Color.Gray;
            btnDoneM.BackColor = m_Storing.MAxis.IsINP ? Color.Green : Color.Gray;
            btnArmM.BackColor = m_Storing.MAxis.IsAlarmed ? Color.Green : Color.Gray;
            btnErrM.BackColor = m_Storing.MAxis.IsAlarmed ? Color.Green : Color.Gray;
            lblC1AxisName.Text = m_LeftC.C1Axis.Name;
            lblC1AxisCurPositon.Text = m_LeftC.C1Axis.CurrentPos.ToString();
            lblC1AxisBackPositon.Text = m_LeftC.C1Axis.BackPos.ToString();
            btnReadyC1.BackColor = m_LeftC.C1Axis.IsServon ? Color.Green : Color.Gray;
            btnOriC1.BackColor = m_LeftC.C1Axis.IsOrign ? Color.Green : Color.Gray;
            btnMelC1.BackColor = m_LeftC.C1Axis.IsPEL ? Color.Green : Color.Gray;
            btnPelC1.BackColor = m_LeftC.C1Axis.IsMEL ? Color.Green : Color.Gray;
            btnEmgC1.BackColor = m_LeftC.C1Axis.IsEmg ? Color.Green : Color.Gray;
            btnDoneC1.BackColor = m_LeftC.C1Axis.IsINP ? Color.Green : Color.Gray;
            btnArmC1.BackColor = m_LeftC.C1Axis.IsAlarmed ? Color.Green : Color.Gray;
            btnErrC1.BackColor = m_LeftC.C1Axis.IsAlarmed ? Color.Green : Color.Gray;
            lblC2AxisName.Text = m_LeftC.C2Axis.Name;
            lblC2AxisCurPositon.Text = m_LeftC.C2Axis.CurrentPos.ToString();
            lblC2AxisBackPositon.Text = m_LeftC.C2Axis.BackPos.ToString();
            btnReadyC2.BackColor = m_LeftC.C2Axis.IsServon ? Color.Green : Color.Gray;
            btnOriC2.BackColor = m_LeftC.C2Axis.IsOrign ? Color.Green : Color.Gray;
            btnMelC2.BackColor = m_LeftC.C2Axis.IsPEL ? Color.Green : Color.Gray;
            btnPelC2.BackColor = m_LeftC.C2Axis.IsMEL ? Color.Green : Color.Gray;
            btnEmgC2.BackColor = m_LeftC.C2Axis.IsEmg ? Color.Green : Color.Gray;
            btnDoneC2.BackColor = m_LeftC.C2Axis.IsINP ? Color.Green : Color.Gray;
            btnArmC2.BackColor = m_LeftC.C2Axis.IsAlarmed ? Color.Green : Color.Gray;
            btnErrC2.BackColor = m_LeftC.C2Axis.IsAlarmed ? Color.Green : Color.Gray;
            lblC3AxisName.Text = m_LeftC.C3Axis.Name;
            lblC3AxisCurPositon.Text = m_LeftC.C3Axis.CurrentPos.ToString();
            lblC3AxisBackPositon.Text = m_LeftC.C3Axis.BackPos.ToString();
            btnReadyC3.BackColor = m_LeftC.C3Axis.IsServon ? Color.Green : Color.Gray;
            btnOriC3.BackColor = m_LeftC.C3Axis.IsOrign ? Color.Green : Color.Gray;
            btnMelC3.BackColor = m_LeftC.C3Axis.IsPEL ? Color.Green : Color.Gray;
            btnPelC3.BackColor = m_LeftC.C3Axis.IsMEL ? Color.Green : Color.Gray;
            btnEmgC3.BackColor = m_LeftC.C3Axis.IsEmg ? Color.Green : Color.Gray;
            btnDoneC3.BackColor = m_LeftC.C3Axis.IsINP ? Color.Green : Color.Gray;
            btnArmC3.BackColor = m_LeftC.C3Axis.IsAlarmed ? Color.Green : Color.Gray;
            btnErrC3.BackColor = m_LeftC.C3Axis.IsAlarmed ? Color.Green : Color.Gray;
            lblC4AxisName.Text = m_LeftC.C4Axis.Name;
            lblC4AxisCurPositon.Text = m_LeftC.C4Axis.CurrentPos.ToString();
            lblC4AxisBackPositon.Text = m_LeftC.C4Axis.BackPos.ToString();
            btnReadyC4.BackColor = m_LeftC.C4Axis.IsServon ? Color.Green : Color.Gray;
            btnOriC4.BackColor = m_LeftC.C4Axis.IsOrign ? Color.Green : Color.Gray;
            btnMelC4.BackColor = m_LeftC.C4Axis.IsPEL ? Color.Green : Color.Gray;
            btnPelC4.BackColor = m_LeftC.C4Axis.IsMEL ? Color.Green : Color.Gray;
            btnEmgC4.BackColor = m_LeftC.C4Axis.IsEmg ? Color.Green : Color.Gray;
            btnDoneC4.BackColor = m_LeftC.C4Axis.IsINP ? Color.Green : Color.Gray;
            btnArmC4.BackColor = m_LeftC.C4Axis.IsAlarmed ? Color.Green : Color.Gray;
            btnErrC4.BackColor = m_LeftC.C4Axis.IsAlarmed ? Color.Green : Color.Gray;
            lblCut1AxisName.Text = m_LeftCut1.CutAxis.Name;
            lblCut1AxisCurPositon.Text = m_LeftCut1.CutAxis.CurrentPos.ToString();
            lblCut1AxisBackPositon.Text = m_LeftCut1.CutAxis.BackPos.ToString();
            btnReadyCut1.BackColor = m_LeftCut1.CutAxis.IsServon ? Color.Green : Color.Gray;
            btnOriCut1.BackColor = m_LeftCut1.CutAxis.IsOrign ? Color.Green : Color.Gray;
            btnMelCut1.BackColor = m_LeftCut1.CutAxis.IsPEL ? Color.Green : Color.Gray;
            btnPelCut1.BackColor = m_LeftCut1.CutAxis.IsMEL ? Color.Green : Color.Gray;
            btnEmgCut1.BackColor = m_LeftCut1.CutAxis.IsEmg ? Color.Green : Color.Gray;
            btnDoneCut1.BackColor = m_LeftCut1.CutAxis.IsINP ? Color.Green : Color.Gray;
            btnArmCut1.BackColor = m_LeftCut1.CutAxis.IsAlarmed ? Color.Green : Color.Gray;
            btnErrCut1.BackColor = m_LeftCut1.CutAxis.IsAlarmed ? Color.Green : Color.Gray;
            lblCut2AxisName.Text = m_LeftCut2.CutAxis.Name;
            lblCut2AxisCurPositon.Text = m_LeftCut2.CutAxis.CurrentPos.ToString();
            lblCut2AxisBackPositon.Text = m_LeftCut2.CutAxis.BackPos.ToString();
            btnReadyCut2.BackColor = m_LeftCut2.CutAxis.IsServon ? Color.Green : Color.Gray;
            btnOriCut2.BackColor = m_LeftCut2.CutAxis.IsOrign ? Color.Green : Color.Gray;
            btnMelCut2.BackColor = m_LeftCut2.CutAxis.IsPEL ? Color.Green : Color.Gray;
            btnPelCut2.BackColor = m_LeftCut2.CutAxis.IsMEL ? Color.Green : Color.Gray;
            btnEmgCut2.BackColor = m_LeftCut2.CutAxis.IsEmg ? Color.Green : Color.Gray;
            btnDoneCut2.BackColor = m_LeftCut2.CutAxis.IsINP ? Color.Green : Color.Gray;
            btnArmCut2.BackColor = m_LeftCut2.CutAxis.IsAlarmed ? Color.Green : Color.Gray;
            btnErrCut2.BackColor = m_LeftCut2.CutAxis.IsAlarmed ? Color.Green : Color.Gray;
            lblCut3AxisName.Text = m_RightCut1.CutAxis.Name;
            lblCut3AxisCurPositon.Text = m_RightCut1.CutAxis.CurrentPos.ToString();
            lblCut3AxisBackPositon.Text = m_RightCut1.CutAxis.BackPos.ToString();
            btnReadyCut3.BackColor = m_RightCut1.CutAxis.IsServon ? Color.Green : Color.Gray;
            btnOriCut3.BackColor = m_RightCut1.CutAxis.IsOrign ? Color.Green : Color.Gray;
            btnMelCut3.BackColor = m_RightCut1.CutAxis.IsPEL ? Color.Green : Color.Gray;
            btnPelCut3.BackColor = m_RightCut1.CutAxis.IsMEL ? Color.Green : Color.Gray;
            btnEmgCut3.BackColor = m_RightCut1.CutAxis.IsEmg ? Color.Green : Color.Gray;
            btnDoneCut3.BackColor = m_RightCut1.CutAxis.IsINP ? Color.Green : Color.Gray;
            btnArmCut3.BackColor = m_RightCut1.CutAxis.IsAlarmed ? Color.Green : Color.Gray;
            btnErrCut3.BackColor = m_RightCut1.CutAxis.IsAlarmed ? Color.Green : Color.Gray;
            lblCut4AxisName.Text = m_RightCut2.CutAxis.Name;
            lblCut4AxisCurPositon.Text = m_RightCut2.CutAxis.CurrentPos.ToString();
            lblCut4AxisBackPositon.Text = m_RightCut2.CutAxis.BackPos.ToString();
            btnReadyCut4.BackColor = m_RightCut2.CutAxis.IsServon ? Color.Green : Color.Gray;
            btnOriCut4.BackColor = m_RightCut2.CutAxis.IsOrign ? Color.Green : Color.Gray;
            btnMelCut4.BackColor = m_RightCut2.CutAxis.IsPEL ? Color.Green : Color.Gray;
            btnPelCut4.BackColor = m_RightCut2.CutAxis.IsMEL ? Color.Green : Color.Gray;
            btnEmgCut4.BackColor = m_RightCut2.CutAxis.IsEmg ? Color.Green : Color.Gray;
            btnDoneCut4.BackColor = m_RightCut2.CutAxis.IsINP ? Color.Green : Color.Gray;
            btnArmCut4.BackColor = m_RightCut2.CutAxis.IsAlarmed ? Color.Green : Color.Gray;
            btnErrCut4.BackColor = m_RightCut2.CutAxis.IsAlarmed ? Color.Green : Color.Gray;
        }

        #endregion

        #region 消息显示

        /// <summary>
        /// 使用委托方式更新AppendText显示
        /// </summary>
        /// <param name="txt">消息</param>
        public void AppendText(string txt)
        {
            try
            {
                if (InvokeRequired)
                {
                    this.Invoke(new Action<string>(AppendText), txt);
                }
                else
                {
                    listBox1.Items.Insert(0, string.Format("{0}-{1}" + Environment.NewLine, DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"), txt));
                    LogHelper.Debug(txt);
                }
            }
            catch(Exception ex)
            {
                throw(ex);
            }
        }

        private void Msg(string str, bool value)
        {
            string tempstr = null;
            bool sign = false;
            try
            {
                var arrRight = new List<object>();
                foreach (var tmpist in listBox1.Items)
                {
                    arrRight.Add(tmpist);
                }

                if (value)
                {
                    foreach (string tmplist in arrRight)
                    {
                        if (tmplist.IndexOf("-") > -1)
                        {
                            tempstr = tmplist.Substring(tmplist.IndexOf("-") + 1, tmplist.Length - tmplist.IndexOf("-") - 1);
                        }
                        if (tempstr == (str + "\r\n"))
                        {
                            sign = true;
                            break;
                        }
                    }
                    if (!sign)
                    {
                        listBox1.Items.Insert(0, (string.Format("{0}-{1}" + Environment.NewLine, DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"), str)));
                        //LogHelper.Error(str);
                    }
                }
                else
                {
                    foreach (string tmplist in arrRight)
                    {
                        if (tmplist.IndexOf("-") > -1)
                        {
                            tempstr = tmplist.Substring(tmplist.IndexOf("-") + 1, tmplist.Length - tmplist.IndexOf("-") - 1);
                            if (tempstr == (str + "\r\n"))
                            {
                                listBox1.Items.Remove(tmplist);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Debug(ex.ToString() + "消息显示异常");
            }
        }

        private Color MachineStatusColor(MachineStatus status)
        {
            try
            {
                switch (status)
                {
                    case MachineStatus.设备未准备好:
                        return Color.Orange;
                    case MachineStatus.设备准备好:
                        return Color.Blue;
                    case MachineStatus.设备运行中:
                        return Color.Green;
                    case MachineStatus.设备停止中:
                        return Color.Red;
                    case MachineStatus.设备暂停中:
                        return Color.Purple;
                    case MachineStatus.设备复位中:
                        return Color.OrangeRed;
                    case MachineStatus.设备报警中:
                        return Color.Red;
                    case MachineStatus.设备急停已按下:
                        return Color.Red;
                    default:
                        return Color.Red;
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
        #endregion

        #region 设备操作按钮

        #region  按钮操作按键

        //安全门按钮
        private void btnUp_MouseDown(object sender, EventArgs e)
        {
            try
            {
                if (m_Storing.SafeDoolCylinder.OutOriginStatus)
                {
                    m_Storing.SafeDoolCylinder.Set();
                }
                else
                {
                    m_Storing.SafeDoolCylinder.Reset();
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        private void mbtnStop(object sender, EventArgs e)
        {

        }

        #endregion

        //private void button12_Click_1(object sender, EventArgs e)
        //{
        //    new FrmTest(m_Splice, m_Buffer, m_Feeder, m_Move, m_LeftC
        //      , m_LeftCut1, m_LeftCut2, m_RightCut1, m_RightCut2, m_Platform, m_Storing).ShowDialog();
        //}

        #region 托盘位置更新

        /// <summary>
        /// 开机排料数量更新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NumModelCountSet_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                Position.Instance.ModelCountSet = Convert.ToInt32(numModelCountSet.Value);
            }
            catch(Exception ex)
            {
                throw(ex);
            }
        }

        /// <summary>
        /// 换盘排料数量更新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void numChangeTrayLayout_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                Position.Instance.ChangeTrayLayout = Convert.ToInt32(numChangeTrayLayout.Value);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
        /// <summary>
        /// 大盘当前位置更新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NumBigTrayPos_ValueChanged(object sender, EventArgs e)
        {
            //Global.BigTray.CurrentPos = Convert.ToInt32(numBigTrayPos.Value) - 1;
        }
        /// <summary>
        /// 小盘当前位置更新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NumYoungTrayPos_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                Global.SmallTray.CurrentPos = Convert.ToInt32(numYoungTrayPos.Value) - 1;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }


        #endregion

        #region  功能按键

        private void BtnStoringGohome_Click(object sender, EventArgs e)
        {
            try
            {
                if (Marking.ThrowerMode)
                {
                    AppendText("设备抛料中，请先停止抛料!");
                    return;
                }
                if (m_Platform.stationInitialize.InitializeDone)
                {
                    m_Storing.stationInitialize.InitializeDone = false;
                    m_Storing.stationInitialize.Flow = 0;
                    m_Storing.stationInitialize.Start = true;
                }
            }
            catch(Exception ex)
            {
                throw(ex);
            }
        }

        private void BtnGohome_Click(object sender, EventArgs e)
        {
            LogHelper.Info("回原点操作");
        }
        /// <summary>
        /// 回原点
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReset_MouseDown(object sender, EventArgs e)
        {
            try
            {
                GoHome.IsLight = true;
                MachineOperation.Stop = false;
                m_External.AlarmReset = true;

                if (Marking.ThrowerMode)
                {
                    AppendText("设备抛料中，请先停止抛料!");
                    return;
                }
                if (ManualAutoMode)
                {
                    if (!MachineIsAlarm.IsAlarm)
                    {
                        AppendText("设备手动状态时，才能复位。自动状态只能清除报警！");
                    }
                }
                else
                {
                    MachineOperation.IniliazieDone = false;
                    MachineOperation.Flow = 0;
                    MachineOperation.Reset = true;
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }

            LogHelper.Info("复位按钮操作");
        }
        /// <summary>
        /// 回原点
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReset_MouseUp(object sender, EventArgs e)
        {
            try
            {
                GoHome.IsLight = false;
                m_External.AlarmReset = false;
                MachineOperation.Reset = false;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        private void BtnSystemStop_Click(object sender, EventArgs e)
        {
            try
            {
                if (Marking.ThrowerMode || Marking.SelectCheckMode || Marking.MancelChangeTray) return;
                Marking.SystemStop = !Marking.SystemStop;
                LogHelper.Info("周期停止");
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        private void BtnSelectCheckMode_Click(object sender, EventArgs e)
        {
            try
            {
                if (Marking.traySensorSheild || Marking.ThrowerMode || Marking.SystemStop || Marking.MancelChangeTray) return;
                if (Config.Instance.SelectCheckRunState)
                {
                    Marking.SelectCheckMode = true;
                }
                else
                {
                    Marking.SelectCheckMode = !Marking.SelectCheckMode;
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
       
            LogHelper.Info("抽检操作");
        }

        private void BtnChangeTray_Click(object sender, EventArgs e)
        {
            try
            {
                if (Marking.traySensorSheild || Marking.ThrowerMode || Marking.SystemStop || Marking.SelectCheckMode) return;
                if (Marking.MancelChangeRunState)
                {
                    Marking.MancelChangeTray = true;
                }
                else
                {
                    Marking.MancelChangeTray = !Marking.MancelChangeTray;
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            
            LogHelper.Info("换盘操作");
        }

        private void BtnTricolorLamp_Click(object sender, EventArgs e)
        {
            try
            {
                layerLight.VoiceClosed = !layerLight.VoiceClosed;
                btnTricolorLamp.Text = layerLight.VoiceClosed ? "蜂铃静止" : "蜂铃正常";
                btnTricolorLamp.BackColor = layerLight.VoiceClosed ? Color.Red : Color.MediumBlue;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        private void LblProductType_TextChanged(object sender, EventArgs e)
        {
            LogHelper.Info("换型操作");
            //InitTray();
        }

        private void BtnThrowerMode_Click(object sender, EventArgs e)
        {
            try
            {
                if (m_External.GoRristatus) { AppendText("设备回待机中！"); return; }
                if (ManualAutoMode) { AppendText("手动模式方可操作！"); return; }
                Marking.ThrowerMode = !Marking.ThrowerMode;
                if (Marking.ThrowerMode)
                {
                    layerLight.VoiceClosed = false; //开启蜂鸣器
                    if (1 == Position.Instance.FragmentationMode && Position.Instance.CheckMotorStart)
                    {
                        IoPoints.T2DO10.Value = true; //开启碎料电机
                    }
                }
                else
                {
                    layerLight.VoiceClosed = true; //屏蔽蜂鸣器
                    if (1 == Position.Instance.FragmentationMode && Position.Instance.CheckMotorStart)
                    {
                        IoPoints.T2DO10.Value = false; //关闭碎料电机
                    }
                }
                btnTricolorLamp.Text = layerLight.VoiceClosed ? "蜂铃静止" : "蜂铃正常";
                btnTricolorLamp.BackColor = layerLight.VoiceClosed ? Color.Red : Color.MediumBlue;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            LogHelper.Info("抛料操作");
        }
        #endregion

        #region 清零功能
        private void BtnFinishcountClean_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(string.Format("是否清零数量"), "确定", MessageBoxButtons.OKCancel) == DialogResult.Cancel)
            {
                return;
            }
            Config.Instance.Finishcount = 0;
            LogHelper.Info("镜片数量清零操作");
        }

        private void BtnCut4FinishcountClean_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(string.Format("是否清零数量"), "确定", MessageBoxButtons.OKCancel) == DialogResult.Cancel)
            {
                return;
            }
            Config.Instance.CutaxisCount[3] = 0;
            LogHelper.Info("4#C轴剪切数量清零操作");
        }

        private void BtnCut3FinishcountClean_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(string.Format("是否清零数量"), "确定", MessageBoxButtons.OKCancel) == DialogResult.Cancel)
            {
                return;
            }
            Config.Instance.CutaxisCount[2] = 0;
            LogHelper.Info("3#C轴剪切数量清零操作");
        }

        private void BtnCut2Finishcount_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(string.Format("是否清零数量"), "确定", MessageBoxButtons.OKCancel) == DialogResult.Cancel)
            {
                return;
            }
            Config.Instance.CutaxisCount[1] = 0;
            LogHelper.Info("2#C轴剪切数量清零操作");
        }

        private void BtnCut1FinishcountClean_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(string.Format("是否清零数量"), "确定", MessageBoxButtons.OKCancel) == DialogResult.Cancel)
            {
                return;
            }
            Config.Instance.CutaxisCount[0] = 0;
            LogHelper.Info("1#C轴剪切数量清零操作");
        }

        private void BtnTrayFinishcountClean_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(string.Format("是否清零数量"), "确定", MessageBoxButtons.OKCancel) == DialogResult.Cancel)
            {
                return;
            }
            Config.Instance.TrayFinishcount = 0;
            LogHelper.Info("托盘数量清零操作");
        }
        private void btnRunTimerClean_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(string.Format("是否清零当前时间"), "确定", MessageBoxButtons.OKCancel) == DialogResult.Cancel)
            {
                return;
            }
            watchCT.Restart();
            LogHelper.Info("生产时间清零操作");
        }

        private void txtUpTimerClean_Click(object sender, EventArgs e)
        {
            watchCT1.Restart();
        }

        private void BtnHoleFinishcountClean_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(string.Format("是否清零数量"), "确定", MessageBoxButtons.OKCancel) == DialogResult.Cancel)
            {
                return;
            }
            Config.Instance.HoleFinishCount = 0;
            LogHelper.Info("模穴数量清零操作");
        }

        #endregion

        #region 弹窗界面
        private void btnSheild_Click(object sender, EventArgs e)
        {
            new FrmRunSet().ShowDialog();
        }
        private void btnExit_Click(object sender, EventArgs e)
        {  
            try
            {
                if (Config.Instance.SelectCheckRunState)
                {
                    Config.Instance.YoungPlatePos = Global.SelectCheckSmallPos;
                    Config.Instance.BigPlatePos = Global.SelectCheckBigPos;
                }
                else
                {
                    if (Global.BigTray.CurrentPos > 0)
                    {
                        Global.BigTray.CurrentPos = 0;
                        Config.Instance.YoungPlatePos = Global.SmallTray.CurrentPos + 1;
                    }
                    else
                    {
                        Config.Instance.YoungPlatePos = Global.SmallTray.CurrentPos;
                    }
                    Config.Instance.BigPlatePos = Global.BigTray.CurrentPos;
                    Config.Instance.SpecialPlatePos = Global.SpecialTray.CurrentPos;
                }
                SerializerManager<Config>.Instance.Save(AppConfig.ConfigFileName, Config.Instance);
                LogHelper.Info("退出操作");
                this.Close();
            }
            catch (Exception ex)
            {
                AppendText("软件退出异常" + ex.Message);
            }

        }

        #endregion

        #region 温度和灯开关
        private void BtnTeamOpen4_Click(object sender, EventArgs e)
        {
            IoPoints.T2DO9.Value = !IoPoints.T2DO9.Value;
           
        }
        private void BtnTeamOpen3_Click(object sender, EventArgs e)
        {                    
            IoPoints.T2DO8.Value = !IoPoints.T2DO8.Value;
        }

        private void BtnTeamOpen2_Click(object sender, EventArgs e)
        {
            IoPoints.T2DO7.Value = !IoPoints.T2DO7.Value;
        }

        private void BtnTeamOpen1_Click(object sender, EventArgs e)
        {
            IoPoints.T2DO6.Value = !IoPoints.T2DO6.Value;
        }

        private void BtnLightOn_Click(object sender, EventArgs e)
        {
            IoPoints.T2DO5.Value = !IoPoints.T2DO5.Value;
        }


        #endregion

        #region  常用基本按键

        /// <summary>
        /// 故障复位
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnAlReset_MouseDown(object sender, EventArgs e)
        {
            try
            {
                ClearAlarmButton.IsLight = true;
                Global.IsLocating = false; //清除手动死锁
                if (MachineOperation.Status == MachineStatus.设备准备好 || MachineOperation.Status == MachineStatus.设备警示中
                    || MachineOperation.Status == MachineStatus.设备暂停中)
                {
                    if (!MachineIsAlarm.IsAlarm && !SpliceIsAlarm.IsAlarm && !BufferIsAlarm.IsAlarm && !FeederIsAlarm.IsAlarm &&
                        !MoveIsAlarm.IsAlarm && !LeftCIsAlarm.IsAlarm && !LeftCut1IsAlarm.IsAlarm && !LeftCut2IsAlarm.IsAlarm &&
                        !RightCut1IsAlarm.IsAlarm && !RightCut2IsAlarm.IsAlarm && !PlateformIsAlarm.IsAlarm && !StoringIsAlarm.IsAlarm
                        && !ManualAutoMode && !Marking.ThrowerMode && !m_External.GoRristatus)
                    {
                        IoPoints.T1DO0.Value = false;
                        IoPoints.T1DO1.Value = false;
                        m_External.GoRristatus = true;
                    }
                }
                m_External.AlarmReset = true;
                #region 伺服报警复位
                IoPoints.ApsController.ClearError();//清除总线报警
                IoPoints.ApsController.SoftReset(); //软复位
                if (m_Platform.Xaxis.IsAlarmed)
                {
                    IoPoints.ApsController.ClearAxisError(1);
                }
                if (m_Platform.Yaxis.IsAlarmed)
                {
                    IoPoints.ApsController.ClearAxisError(0);
                }
                if (m_Platform.Zaxis.IsAlarmed)
                {
                    IoPoints.ApsController.ClearAxisError(2);
                }
                if (m_LeftC.C1Axis.IsAlarmed)
                {
                    IoPoints.ApsController.ClearAxisError(7);
                }
                if (m_LeftC.C2Axis.IsAlarmed)
                {
                    IoPoints.ApsController.ClearAxisError(8);
                }
                if (m_LeftC.C3Axis.IsAlarmed)
                {
                    IoPoints.ApsController.ClearAxisError(9);
                }
                if (m_LeftC.C4Axis.IsAlarmed)
                {
                    IoPoints.ApsController.ClearAxisError(10);
                }
                if (m_LeftCut1.CutAxis.IsAlarmed)
                {
                    IoPoints.ApsController.ClearAxisError(3);
                }
                if (m_LeftCut2.CutAxis.IsAlarmed)
                {
                    IoPoints.ApsController.ClearAxisError(4);
                }
                if (m_RightCut1.CutAxis.IsAlarmed)
                {
                    IoPoints.ApsController.ClearAxisError(5);
                }
                if (m_RightCut2.CutAxis.IsAlarmed)
                {
                    IoPoints.ApsController.ClearAxisError(6);
                }
                #endregion
                Marking.MotorOverloadSign = false; //清除碎料电机过载报警
                layerLight.VoiceClosed = true;     //屏蔽蜂鸣器
                btnTricolorLamp.Text = layerLight.VoiceClosed ? "蜂铃静止" : "蜂铃正常";
                //btnTricolorLamp.BackColor = layerLight.VoiceClosed ? Color.Red : Color.MediumBlue;
            }
            catch(Exception ex)
            {
                throw(ex);
            }

            LogHelper.Info("故障复位操作");
        }

        /// <summary>
        /// 故障复位
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnAlReset_MouseUp(object sender, EventArgs e)
        {
            try
            {
                ClearAlarmButton.IsLight = false;
                m_External.AlarmReset = false;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        private void btnStart_MouseDown(object sender, EventArgs e)
        {
            try
            {
                if (Marking.ModifyParameterMarker)
                {
                    SaveFile();
                    LogHelper.Info("已保存配置文件");
                }
                if (Config.Instance.SpecialTrayStart) { Global.SpecialTray = TrayFactory.GetTrayFactory(Config.Instance.SpecialPlateID); }

                if (m_External.GoRristatus) { AppendText("设备回待机中！"); return; }
                StartButton.IsLight = true;
                MachineOperation.Stop = false;
                MachineOperation.Pause = false;
                MachineOperation.Start = true;
                btnStart.BackColor = Color.Red;
                btnStop.BackColor = Color.Green;
                layerLight.VoiceClosed = false; //开启蜂鸣器
                btnTricolorLamp.Text = layerLight.VoiceClosed ? "蜂铃静止" : "蜂铃正常";
                btnTricolorLamp.BackColor = layerLight.VoiceClosed ? Color.Red : Color.MediumBlue;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            LogHelper.Info("启动按钮操作");
        }

        private void btnStart_MouseUp(object sender, EventArgs e)
        {
            try
            {
                StartButton.IsLight = false;
                MachineOperation.Start = false;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        private void btnStop_MouseDown(object sender, EventArgs e)
        {
            try
            {
                StopButton.IsLight = true;
                MachineOperation.Stop = true;
            }
            catch(Exception ex)
            {
                throw(ex);
            }
            LogHelper.Info("停止按钮操作");
        }

        private void btnStop_MouseUp(object sender, EventArgs e)
        {
            try
            {
                StopButton.IsLight = false;
                MachineOperation.Stop = false;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        private void btnPause_MouseDown(object sender, EventArgs e)
        {
            try
            {
                PauseButton.IsLight = true;
                MachineOperation.Start = false;
                MachineOperation.Pause = true;
                btnStart.BackColor = Color.Green;
                btnStop.BackColor = Color.Red;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            LogHelper.Info("暂停按钮操作");
        }

        private void btnPause_MouseUp(object sender, EventArgs e)
        {
            try
            {
                PauseButton.IsLight = false;
                MachineOperation.Pause = false;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        private void btnManualAuto_Click(object sender, EventArgs e)
        {
            try
            {
                if (ManualAutoMode && MachineOperation.Running) //自动模式不能直接切换为手动，需要先停止运行再切换模式
                {
                    AppendText("设备运行中，不能切换至手动模式!");
                    return;
                }
                if (Marking.ThrowerMode)
                {
                    AppendText("设备抛料中，请先停止抛料!");
                    return;
                }
                LogHelper.Info("手动按钮操作");
                ManualAutoMode = ManualAutoMode ? false : true;
                btnManualAuto.Text = ManualAutoMode ? "自动模式" : "手动模式";
                btnManualAuto.BackColor = ManualAutoMode ? Color.Red : Color.Green;
                if (ManualAutoMode)
                {
                    tbcMain.SelectedTab = tpgMain;
                    layerLight.VoiceClosed = false; //开启蜂鸣器
                    if (1 == Position.Instance.FragmentationMode && Position.Instance.CheckMotorStart)
                    {
                        IoPoints.T2DO10.Value = true; //开启碎料电机
                    }
                }
                else
                {
                    layerLight.VoiceClosed = true; //屏蔽蜂鸣器
                    if (1 == Position.Instance.FragmentationMode && Position.Instance.CheckMotorStart)
                    {
                        IoPoints.T2DO10.Value = false; //开启碎料电机
                    }
                }
                btnTricolorLamp.Text = layerLight.VoiceClosed ? "蜂铃静止" : "蜂铃正常";
                btnTricolorLamp.BackColor = layerLight.VoiceClosed ? Color.Red : Color.MediumBlue;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        private void frmMain_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                switch (e.KeyCode)
                {
                    case Keys.M:
                        if (ManualAutoMode && MachineOperation.Running) //自动模式不能直接切换为手动，需要先停止运行再切换模式
                        {
                            AppendText("设备运行中，不能切换至手动模式!");
                            return;
                        }
                        LogHelper.Info("手动按钮操作");
                        ManualAutoMode = ManualAutoMode ? false : true;
                        btnManualAuto.Text = ManualAutoMode ? "自动模式" : "手动模式";
                        btnManualAuto.BackColor = ManualAutoMode ? Color.Red : Color.Green;
                        if (ManualAutoMode)
                        {
                            tbcMain.SelectedTab = tpgMain;
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
        #endregion

        #endregion
    }
}

