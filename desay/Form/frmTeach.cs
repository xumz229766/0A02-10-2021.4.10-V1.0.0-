using System;
using System.Diagnostics;
using System.Drawing;
using System.Enginee;
using System.Interfaces;
using System.Threading;
using System.Threading.Tasks;
using System.ToolKit;
using System.ToolKit.Helper;
using System.Windows.Forms;

namespace desay
{
    public partial class frmTeach : Form
    {
        private PositionName name = 0;
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

        #region 控件

        private Panel m_PanelJogType, m_panelOperate;
        private MoveSelectHorizontal moveselect;

        /// <summary>
        /// 仓储显示
        /// </summary>
        private MLayerView<double> mLayerView;

        /// <summary>
        /// 默认显示
        /// </summary>
        private PositionDefaultView defaultView;

        /// <summary>
        /// 退盘
        /// </summary>  
        private ExitTrayOpenView<Double> ExitTrayPositionView;
        /// <summary>
        /// Z安全高度
        /// </summary>
        private Position3DView<Double> ZSafePositionView;
        /// <summary>
        /// 勾盘
        /// </summary>
        private PackTrayOpenView<Double> GTrayPositionView;

        /// <summary>
        /// 剪切位置
        /// </summary>
        private CutControlView<Double>[] CutPosition = new CutControlView<double>[4];
        /// <summary>
        /// XYZ取产品位置
        /// </summary>
        private Position4DEditView<double> GetProductPositionView;
        /// <summary>
        /// XYZ放产品位置（第一个）
        /// </summary>
        private Position4DEditView<double> PuchProductPositionView;
        /// <summary>
        /// C轴设定
        /// </summary>
        private CAxisPositionSet mCAxisPositionSet;

        /// <summary>
        /// 推料P轴位置
        /// </summary>
        private PushSet<Double> PuchPositionView;

        /// <summary>
        /// 推料位置
        /// </summary>
        private Pos<Position1DView<Double>>[] PuchPos = new Pos<Position1DView<double>>[3];

        /// <summary>
        /// 抽检设定
        /// </summary>
        private Position3DView<double> SelectCheckPositionView;

        /// <summary>
        ///烫刀次数
        /// </summary>
        private int HotCutCount;

        Stopwatch stopWatch = new Stopwatch();

        #endregion

        public frmTeach()
        {
            InitializeComponent();
        }

        public frmTeach(Splice Splice, Buffer Buffer, Feeder Feeder, Move Move, LeftC LeftC,
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
        }

        public void RefreshHole()
        {
            mCAxisPositionSet.InitdgvPlatePositionRows();
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node.Text == "0A02剪切机")
            {
                return;
            }

            foreach (TreeNode node in treeView1.Nodes[0].Nodes)
            {
                if (node.Text == e.Node.Text)
                {
                    node.ForeColor = Color.White;
                    node.BackColor = Color.DodgerBlue;
                }
                else
                {
                    node.ForeColor = Color.Black;
                    node.BackColor = Color.Transparent;
                }
            }

            var nodetext = (PositionName)Enum.Parse(typeof(PositionName), e.Node.Text);
            panelVeiw.Controls.Clear();
            m_panelOperate.Controls.Clear();

            m_PanelJogType.Controls.Add(moveselect);
            panelVeiw.Controls.Add(m_PanelJogType, 0, 0);
            switch (nodetext)
            {
                case PositionName.C轴设定:
                    m_panelOperate.Controls.Add(mCAxisPositionSet);
                    m_panelOperate.Dock = DockStyle.Fill;
                    panelVeiw.Controls.Add(m_panelOperate, 0, 1);
                    mCAxisPositionSet.InitdgvPlatePositionRows();
                    break;
                case PositionName.勾盘位置设定:
                    m_panelOperate.Controls.Add(GTrayPositionView);
                    panelVeiw.Controls.Add(m_panelOperate, 0, 1);
                    break;
                case PositionName.退盘位置设定:
                    m_panelOperate.Controls.Add(ExitTrayPositionView);
                    panelVeiw.Controls.Add(m_panelOperate, 0, 1);
                    break;
                case PositionName.推进P轴位置:
                    m_panelOperate.Controls.Add(PuchPositionView);
                    panelVeiw.Controls.Add(m_panelOperate, 0, 1);
                    break;
                case PositionName.Z1剪切位置:
                    m_panelOperate.Controls.Add(CutPosition[0]);
                    panelVeiw.Controls.Add(m_panelOperate, 0, 1);
                    break;
                case PositionName.Z2剪切位置:
                    m_panelOperate.Controls.Add(CutPosition[1]);
                    panelVeiw.Controls.Add(m_panelOperate, 0, 1);
                    break;
                case PositionName.Z3剪切位置:
                    m_panelOperate.Controls.Add(CutPosition[2]);
                    panelVeiw.Controls.Add(m_panelOperate, 0, 1);
                    break;
                case PositionName.Z4剪切位置:
                    m_panelOperate.Controls.Add(CutPosition[3]);
                    panelVeiw.Controls.Add(m_panelOperate, 0, 1);
                    break;
                //case PositionName.一号推料原点位置:
                //    m_panelOperate.Controls.Add(PuchPos[0].Origin);
                //    panelVeiw.Controls.Add(m_panelOperate, 0, 1);
                //    break;
                //case PositionName.一号推料终点位置:
                //    m_panelOperate.Controls.Add(PuchPos[0].Move);
                //    panelVeiw.Controls.Add(m_panelOperate, 0, 1);
                //    break;
                //case PositionName.二号推料原点位置:
                //    m_panelOperate.Controls.Add(PuchPos[1].Origin);
                //    panelVeiw.Controls.Add(m_panelOperate, 0, 1);
                //    break;
                //case PositionName.二号推料终点位置:
                //    m_panelOperate.Controls.Add(PuchPos[1].Move);
                //    panelVeiw.Controls.Add(m_panelOperate, 0, 1);
                //    break;
                //case PositionName.三号推料原点位置:
                //    m_panelOperate.Controls.Add(PuchPos[2].Origin);
                //    panelVeiw.Controls.Add(m_panelOperate, 0, 1);
                //    break;
                //case PositionName.三号推料终点位置:
                //    m_panelOperate.Controls.Add(PuchPos[2].Move);
                //    panelVeiw.Controls.Add(m_panelOperate, 0, 1);
                //    break;
                case PositionName.Z轴安全位置:
                    m_panelOperate.Controls.Add(ZSafePositionView);
                    panelVeiw.Controls.Add(m_panelOperate, 0, 1);
                    break;
                case PositionName.XYZ模组取产品位置:
                    m_panelOperate.Controls.Add(GetProductPositionView);
                    panelVeiw.Controls.Add(m_panelOperate, 0, 1);
                    break;
                case PositionName.XYZ模组放产品第一个位置:
                    m_panelOperate.Controls.Add(PuchProductPositionView);
                    panelVeiw.Controls.Add(m_panelOperate, 0, 1);
                    break;
                case PositionName.料仓位置:
                    m_panelOperate.Controls.Add(mLayerView);
                    m_panelOperate.Dock = DockStyle.Fill;
                    panelVeiw.Controls.Add(m_panelOperate, 0, 1);
                    break;
                case PositionName.抽检位置:
                    m_panelOperate.Controls.Add(SelectCheckPositionView);
                    m_panelOperate.Dock = DockStyle.Fill;
                    panelVeiw.Controls.Add(m_panelOperate, 0, 1);
                    break;
                default:
                    panelVeiw.Controls.Add(defaultView);
                    break;
            }
            name = nodetext;

            Marking.ClearMemory();
        }

        private void frmTeach_Load(object sender, EventArgs e)
        {
            defaultView = new PositionDefaultView();

            #region  C轴设定
            mCAxisPositionSet = new CAxisPositionSet(m_LeftC)
            {
                Dock = DockStyle.Fill
            };
            #endregion

            #region 仓储
            mLayerView = new MLayerView<double>(m_Storing.MAxis, new CylinderOperate[] { new CylinderOperate(m_Platform.GetTrayCylinder),
                new CylinderOperate(m_Platform.LockCylinder) })
            {
                Dock = DockStyle.Fill
            };
            #endregion

            #region  抽检位置
            SelectCheckPositionView = new Position3DView<double>(new ApsAxis[] { m_Platform.Xaxis, m_Platform.Yaxis, m_Platform.Zaxis },
          () =>
          {
              Position.Instance.SelectCheckPosition.X = SelectCheckPositionView.Point[0];
              Position.Instance.SelectCheckPosition.Y = SelectCheckPositionView.Point[1];
              Position.Instance.SelectCheckPosition.Z = SelectCheckPositionView.Point[2];
              Marking.ModifyParameterMarker = true;
          },
          () =>
          {
              MoveTo3Pos(m_Platform.Xaxis, m_Platform.Yaxis, m_Platform.Zaxis, Position.Instance.SelectCheckPosition,true);
          })
            {
                Point = new double[] { Position.Instance.SelectCheckPosition.X, Position.Instance.SelectCheckPosition.Y, Position.Instance.SelectCheckPosition.Z },
                Dock = DockStyle.Fill
            };
            #endregion

            #region  Z轴安全位置
            ZSafePositionView = new Position3DView<double>(new ApsAxis[] { m_Platform.Xaxis, m_Platform.Yaxis, m_Platform.Zaxis },
          () =>
          {
              Position.Instance.ZsafePosition.X = ZSafePositionView.Point[0];
              Position.Instance.ZsafePosition.Y = ZSafePositionView.Point[1];
              Position.Instance.ZsafePosition.Z = ZSafePositionView.Point[2];
              Marking.ModifyParameterMarker = true;
          },
          () =>
          {
              MoveTo3Pos(m_Platform.Xaxis, m_Platform.Yaxis, m_Platform.Zaxis, Position.Instance.ZsafePosition,true);
          })
            {
                Point = new double[] { Position.Instance.ZsafePosition.X, Position.Instance.ZsafePosition.Y, Position.Instance.ZsafePosition.Z },
                Dock = DockStyle.Fill
            };
            #endregion

            #region  XYZ模组取产品位置
            GetProductPositionView = new Position4DEditView<double>(new ApsAxis[] { m_Platform.Xaxis, m_Platform.Yaxis, m_Platform.Zaxis },
                new CylinderOperate[] {new CylinderOperate(m_LeftCut1.OverturnCylinder),new CylinderOperate(m_LeftCut2.OverturnCylinder)
                ,new CylinderOperate(m_RightCut1.OverturnCylinder),new CylinderOperate(m_RightCut2.OverturnCylinder) },
           () =>
           {
               Position.Instance.GetProductPosition.X = GetProductPositionView.Point[0];
               Position.Instance.GetProductPosition.Y = GetProductPositionView.Point[1];
               Position.Instance.GetProductPosition.Z = GetProductPositionView.Point[2];
               Position.Instance.GetSafetyZ = GetProductPositionView.Point[3];
               Marking.ModifyParameterMarker = true;
           },
           () =>
           {
               MoveTo3Pos(m_Platform.Xaxis, m_Platform.Yaxis, m_Platform.Zaxis, Position.Instance.GetProductPosition,true);
           })
            {
                Point = new double[] { Position.Instance.GetProductPosition.X, Position.Instance.GetProductPosition.Y, Position.Instance.GetProductPosition.Z, Position.Instance.GetSafetyZ },
                Dock = DockStyle.Fill
            };
            #endregion

            #region  XYZ模组放产品第一个位置
            PuchProductPositionView = new Position4DEditView<double>(new ApsAxis[] { m_Platform.Xaxis, m_Platform.Yaxis, m_Platform.Zaxis },
                new CylinderOperate[] {new CylinderOperate(m_LeftCut1.OverturnCylinder),new CylinderOperate(m_LeftCut2.OverturnCylinder)
                ,new CylinderOperate(m_RightCut1.OverturnCylinder),new CylinderOperate(m_RightCut2.OverturnCylinder) }, 
           () =>
           {
               Position.Instance.PuchProductPosition.X = PuchProductPositionView.Point[0];
               Position.Instance.PuchProductPosition.Y = PuchProductPositionView.Point[1];
               Position.Instance.PuchProductPosition.Z = PuchProductPositionView.Point[2];
               Position.Instance.PuchSafetyZ = PuchProductPositionView.Point[3];
               Marking.ModifyParameterMarker = true;
           },
           () =>
           {
               MoveTo3Pos(m_Platform.Xaxis, m_Platform.Yaxis, m_Platform.Zaxis, Position.Instance.PuchProductPosition,false);
           })
            {
                Point = new double[] { Position.Instance.PuchProductPosition.X, Position.Instance.PuchProductPosition.Y, Position.Instance.PuchProductPosition.Z, Position.Instance.PuchSafetyZ },
                Dock = DockStyle.Fill
            };
            #endregion

            #region 剪切轴位置 
            CutPosition[0] = new CutControlView<double>
            (
                new ApsAxis[] { m_LeftCut1.CutAxis, m_LeftC.C1Axis, m_LeftC.Push1Axis }, new CylinderOperate[] { new CylinderOperate(m_LeftCut1.OverturnCylinder), new CylinderOperate(m_LeftCut1.FrontCylinder)
                , new CylinderOperate(m_LeftCut1.GripperCylinder)},
            () =>
            {
                Position.Instance.PosCut[0].Origin = CutPosition[0].Point[0];
                Position.Instance.PosCut[0].Move = CutPosition[0].Point[1];
                Position.Instance.PosCutEnd[0] = CutPosition[0].Point[2];
                Marking.ModifyParameterMarker = true;
            },
            () =>
            {
                AxisParameter.Instance.BtnTestState = false;
                CutTestOne(0);
                AxisParameter.Instance.BtnTestState = true;
            },
            () =>
            {
                AxisParameter.Instance.BtnTestState = false;
                CAxisRotate(0);
                AxisParameter.Instance.BtnTestState = true;
            })
            {
                Point = new double[] { Position.Instance.PosCut[0].Origin, Position.Instance.PosCut[0].Move, Position.Instance.PosCutEnd[0] },
                Dock = DockStyle.Fill
            };

            CutPosition[1] = new CutControlView<double>
            (
                new ApsAxis[] { m_LeftCut2.CutAxis, m_LeftC.C2Axis, m_LeftC.Push2Axis }, new CylinderOperate[] { new CylinderOperate(m_LeftCut2.OverturnCylinder), new CylinderOperate(m_LeftCut2.FrontCylinder)
                 , new CylinderOperate(m_LeftCut2.GripperCylinder)},
            () =>
            {
                Position.Instance.PosCut[1].Origin = CutPosition[1].Point[0];
                Position.Instance.PosCut[1].Move = CutPosition[1].Point[1];
                Position.Instance.PosCutEnd[1] = CutPosition[1].Point[2];
                Marking.ModifyParameterMarker = true;
            },
           () =>
           {
               AxisParameter.Instance.BtnTestState = false;
               CutTestOne(1);
               AxisParameter.Instance.BtnTestState = true;
           },
           () =>
           {
               AxisParameter.Instance.BtnTestState = false;
               CAxisRotate(1);
               AxisParameter.Instance.BtnTestState = true;
           })
            {
                Point = new double[] { Position.Instance.PosCut[1].Origin, Position.Instance.PosCut[1].Move, Position.Instance.PosCutEnd[1] },
                Dock = DockStyle.Fill
            };

            CutPosition[2] = new CutControlView<double>
            (
                new ApsAxis[] { m_RightCut1.CutAxis, m_LeftC.C3Axis, m_LeftC.Push3Axis }, new CylinderOperate[] { new CylinderOperate(m_RightCut1.OverturnCylinder), new CylinderOperate(m_RightCut1.FrontCylinder)
                , new CylinderOperate(m_RightCut1.GripperCylinder)},
            () =>
            {
                Position.Instance.PosCut[2].Origin = CutPosition[2].Point[0];
                Position.Instance.PosCut[2].Move = CutPosition[2].Point[1];
                Position.Instance.PosCutEnd[2] = CutPosition[2].Point[2];
                Marking.ModifyParameterMarker = true;
            },
            () =>
            {
                AxisParameter.Instance.BtnTestState = false;
                CutTestOne(2);
                AxisParameter.Instance.BtnTestState = true;
            },
            () =>
            {
                AxisParameter.Instance.BtnTestState = false;
                CAxisRotate(2);
                AxisParameter.Instance.BtnTestState = true;
            })
            {
                Point = new double[] { Position.Instance.PosCut[2].Origin, Position.Instance.PosCut[2].Move, Position.Instance.PosCutEnd[2] },
                Dock = DockStyle.Fill
            };

            CutPosition[3] = new CutControlView<double>
           (
                new ApsAxis[] { m_RightCut2.CutAxis, m_LeftC.C4Axis, m_LeftC.Push4Axis }, new CylinderOperate[] { new CylinderOperate(m_RightCut2.OverturnCylinder), new CylinderOperate(m_RightCut2.FrontCylinder)
                , new CylinderOperate(m_RightCut2.GripperCylinder)},
            () =>
            {
                Position.Instance.PosCut[3].Origin = CutPosition[3].Point[0];
                Position.Instance.PosCut[3].Move = CutPosition[3].Point[1];
                Position.Instance.PosCutEnd[3] = CutPosition[3].Point[2];
                Marking.ModifyParameterMarker = true;
            },
            () =>
            {
                AxisParameter.Instance.BtnTestState = false;
                CutTestOne(3);
                AxisParameter.Instance.BtnTestState = true;
            },
            () =>
            {
                AxisParameter.Instance.BtnTestState = false;
                CAxisRotate(3);
                AxisParameter.Instance.BtnTestState = true;
            })
            {
                Point = new double[] { Position.Instance.PosCut[3].Origin, Position.Instance.PosCut[3].Move, Position.Instance.PosCutEnd[3] },
                Dock = DockStyle.Fill
            };

            #endregion

            #region 推进P轴位置
            PuchPositionView = new PushSet<double>(new ApsAxis[] { m_LeftC.Push4Axis, m_LeftC.Push3Axis, m_LeftC.Push2Axis, m_LeftC.Push1Axis,
            m_LeftC.C4Axis, m_LeftC.C3Axis, m_LeftC.C2Axis, m_LeftC.C1Axis},
            () =>
            {
                Position.Instance.PosPush[0].Origin = PuchPositionView.Point[0];
                Position.Instance.PosPush[0].Move = PuchPositionView.Point[1];
                Position.Instance.PosPush[1].Origin = PuchPositionView.Point[2];
                Position.Instance.PosPush[1].Move = PuchPositionView.Point[3];
                Position.Instance.PosPush[2].Origin = PuchPositionView.Point[4];
                Position.Instance.PosPush[2].Move = PuchPositionView.Point[5];
                Position.Instance.PosPush[3].Origin = PuchPositionView.Point[6];
                Position.Instance.PosPush[3].Move = PuchPositionView.Point[7];
                Marking.ModifyParameterMarker = true;
            })
            {
                Point = new double[] { Position.Instance.PosPush[0].Origin, Position.Instance.PosPush[0].Move, Position.Instance.PosPush[1].Origin, Position.Instance.PosPush[1].Move,
                                       Position.Instance.PosPush[2].Origin, Position.Instance.PosPush[2].Move, Position.Instance.PosPush[3].Origin, Position.Instance.PosPush[3].Move},
                Dock = DockStyle.Fill
            };

            #endregion

            #region 勾盘位置

            GTrayPositionView = new PackTrayOpenView<double>(new ApsAxis[] { m_Platform.Yaxis, m_Platform.Xaxis, m_Platform.Zaxis },
              new CylinderOperate[] { new CylinderOperate(m_Platform.GetTrayCylinder), new CylinderOperate(m_Platform.LockCylinder) },
                () =>
               {
                   LogHelper.Info("勾盘保存操作");
                   Position.Instance.PosGTrayOriPosition[0].Y = GTrayPositionView.Point[0];
                   Position.Instance.PosGTrayMovePosition[0].Y = GTrayPositionView.Point[1];
                   Position.Instance.PosGTrayOriPosition[1].Y = GTrayPositionView.Point[2];
                   Position.Instance.PosGTrayMovePosition[1].Y = GTrayPositionView.Point[3];
                   Marking.ModifyParameterMarker = true;
               },
               () =>
               {
                   LogHelper.Info("勾盘操作");
                   AxisParameter.Instance.BtnTestState = false;
                   GtrayTestOne();
                   AxisParameter.Instance.BtnTestState = true;                   
               })
            {
                Point = new double[] { Position.Instance.PosGTrayOriPosition[0].Y, Position.Instance.PosGTrayMovePosition[0].Y,
                    Position.Instance.PosGTrayOriPosition[1].Y, Position.Instance.PosGTrayMovePosition[1].Y },
                Dock = DockStyle.Fill
            };

            #endregion

            #region 退盘位置

            ExitTrayPositionView = new ExitTrayOpenView<double>(new ApsAxis[] { m_Platform.Yaxis, m_Platform.Xaxis, m_Platform.Zaxis },
              new CylinderOperate[] { new CylinderOperate(m_Platform.GetTrayCylinder), new CylinderOperate(m_Platform.LockCylinder) },
                () =>
                {
                    Position.Instance.PosExitTrayOriPosition[0].Y = ExitTrayPositionView.Point[0];
                    Position.Instance.PosExitTrayMovePosition[0].Y = ExitTrayPositionView.Point[1];
                    Position.Instance.PosExitTrayOriPosition[1].Y = ExitTrayPositionView.Point[2];
                    Position.Instance.PosExitTrayMovePosition[1].Y = ExitTrayPositionView.Point[3];
                    Marking.ModifyParameterMarker = true;
                },
                () =>
                {
                    AxisParameter.Instance.BtnTestState = false;
                    ExittrayTestOne();
                    AxisParameter.Instance.BtnTestState = true;                    
                })
            {
                Point = new double[] { Position.Instance.PosExitTrayOriPosition[0].Y, Position.Instance.PosExitTrayMovePosition[0].Y,
                    Position.Instance.PosExitTrayOriPosition[1].Y, Position.Instance.PosExitTrayMovePosition[1].Y },
                Dock = DockStyle.Fill
            };

            #endregion

            moveselect = new MoveSelectHorizontal();
            moveselect.Dock = DockStyle.Fill;

            m_PanelJogType = new Panel();
            m_PanelJogType.Size = new Size(133, 60);
            m_PanelJogType.Dock = DockStyle.Top;
            m_PanelJogType.Controls.Add(moveselect);

            m_panelOperate = new Panel();
            m_panelOperate.Dock = DockStyle.Fill;

            treeView1.Nodes.Add(Global.MACHINNAME);
            foreach (string str in Enum.GetNames(typeof(PositionName)))
            {
                treeView1.Nodes[0].Nodes.Add(str);
            }

            treeView1.ExpandAll();
            timer1.Enabled = true;
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
                            case 0://动作判断是否原点
                                if (OverturnCylinder.OutOriginStatus && FrontCylinder.OutOriginStatus && GripperCylinder.OutOriginStatus 
                                && GripperCylinder.Condition.IsOnCondition && axis.IsInPosition(Position.Instance.PosCut[Step].Origin))
                                {
                                    step1 = 10;
                                }
                                else
                                {
                                    axis.MoveTo(Position.Instance.PosCut[Step].Origin, velocityCurve);
                                    OverturnCylinder.InitExecute();
                                    FrontCylinder.InitExecute();
                                    GripperCylinder.InitExecute();
                                    OverturnCylinder.Reset();
                                    FrontCylinder.Reset();
                                    GripperCylinder.Reset();
                                }
                                break;
                            case 10://前后气缸动作
                                FrontCylinder.Set();
                                step1 = 20;
                                break;
                            case 20://夹子动作
                                if (FrontCylinder.OutMoveStatus && OverturnCylinder.OutOriginStatus && GripperCylinder.Condition.IsOnCondition)
                                {
                                    GripperCylinder.Set();
                                    step1 = 30;
                                }
                                break;
                            case 30://剪刀预动作位置
                                if (GripperCylinder.OutMoveStatus && GripperCylinder.Condition.IsOffCondition)
                                {
                                    if (!Position.Instance.Caxis[Step].PosTorsion)
                                    {
                                        step1 = 60;
                                    }
                                    else
                                    {
                                        step1 = 50;
                                    }
                                }
                                break;
                            case 50://启动伺服1#扭力剪切控制
                                if(axis.TorqueControl(Position.Instance.PosCut[Step].Move, Config.Instance.PressCut[Step], Position.Instance.PosCutEnd[Step], 200, velocityCurve))
                                {
                                    step1 = 80; 
                                }
                                else //扭力止动
                                {
                                    MessageBox.Show("扭力剪切未剪断!");
                                    Global.IsLocating = false;
                                    return 0;                                 
                                }                               
                                break;
                            case 60://伺服1#移动到剪切，启动运行
                                if(Position.Instance.PosCutEnd[Step] > Position.Instance.PosCut[Step].Move)
                                {
                                    axis.MoveToExtern(Position.Instance.PosCut[Step].Move, Position.Instance.PosCutEnd[Step], velocityCurve);
                                    step1 = 80;
                                }
                                else
                                {
                                    MessageBox.Show("闭合位需大于缓冲位!");
                                    Global.IsLocating = false;
                                    return 0;
                                }
                                break;
                            case 80://伺服1#到位。
                                if (axis.IsInPosition(Position.Instance.PosCutEnd[Step]))
                                {
                                    stopwatch.Restart();
                                    step1 = 90;
                                }
                                break;
                            case 90://剪切闭合延时
                                if (stopwatch.ElapsedMilliseconds >= Delay.Instance.CutDelay[Step])
                                {
                                    FrontCylinder.Reset();
                                    step1 = 100;
                                }
                                break;
                            case 100://前后气缸到位，上下气缸为ON
                                if (FrontCylinder.OutOriginStatus)
                                {
                                    HotCutCount = 0;
                                    step1 = 110;
                                }
                                break;
                            case 110:
                                if (Position.Instance.Caxis[Step].HotCut)
                                {                                  
                                    step1 = 120;
                                }
                                else 
                                {
                                    step1 = 150;
                                }
                                break;
                            case 120://上下气缸到位，判断是否烫刀，前后气缸为On
                                if (FrontCylinder.Condition.IsOnCondition)
                                {
                                    FrontCylinder.Set();
                                    stopwatch.Restart();
                                    step1 = 130;
                                }
                                break;
                            case 130://前后气缸到位，前后气缸为OFF
                                if (FrontCylinder.OutMoveStatus && FrontCylinder.Condition.IsOffCondition)
                                {
                                    if (stopwatch.ElapsedMilliseconds >= Position.Instance.Caxis[Step].HotCutTime)
                                    {
                                        FrontCylinder.Reset();
                                        HotCutCount++;
                                        stopwatch.Stop();
                                        step1 = 140;
                                    }
                                }
                                break;
                            case 140://前后气缸到位，前后气缸为OFF
                                if (FrontCylinder.OutOriginStatus)
                                {
                                    if (HotCutCount >= Position.Instance.Caxis[Step].HotCutCount)
                                    {
                                        step1 = 150;
                                    }
                                    else
                                    {
                                        step1 = 120;
                                    }
                                }
                                break;
                            case 150:
                                if (Position.Instance.OverturnOpen[Step])
                                {
                                    OverturnCylinder.Set();
                                }
                                step1 = 160;
                                break;
                            case 160:
                                axis.MoveTo(Position.Instance.PosCut[Step].Origin, AxisParameter.Instance.Cut1VelocityCurve);
                                step1 = 165;
                                break;
                            case 165:
                                if (Position.Instance.OverturnOpen[Step])
                                {
                                    if (OverturnCylinder.OutMoveStatus)
                                    {
                                        step1 = 170;
                                    }
                                }
                                else
                                {
                                    step1 = 170;
                                }
                                break;
                            case 170://判断剪切前后气缸off条件                            
                                if (axis.IsInPosition(Position.Instance.PosCut[Step].Origin) && FrontCylinder.OutOriginStatus)
                                {
                                    stopwatch.Restart();
                                    step1 = 180;
                                }
                                break;
                            case 180://动作完成下降
                                if (stopwatch.ElapsedMilliseconds >= 2000)
                                {
                                    stopwatch.Stop();
                                    GripperCylinder.Reset();
                                    step1 = 190;
                                }
                                break;
                            case 190://下降完成
                                if (GripperCylinder.OutOriginStatus)
                                {
                                    if (Position.Instance.OverturnOpen[Step])
                                    {
                                        OverturnCylinder.Reset();
                                    }
                                    step1 = 200;
                                }
                                break;
                            case 200:
                                if(OverturnCylinder.OutOriginStatus)
                                {
                                    step1 = 210;
                                }
                                break;
                            default:
                                step1 = 0;
                                Global.IsLocating = false;
                                return 0;
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

        private int[] CAxis = new int[4];
        /// <summary>
        /// C轴旋转
        /// </summary>
        /// <param name="axis">轴</param> 
        /// <returns></returns>
        private int CAxisRotate(int axis)
        {
            if (Global.IsLocating) return -1;
            Global.IsLocating = true;
            if (CAxis[axis] >= Position.Instance.HoleNumber / 4)
            {
                CAxis[axis] = 0;
            }
            switch (axis)
            {
                case 0:
                    Task.Factory.StartNew(() =>
                    {
                        m_LeftC.C1Axis.MoveTo(Position.Instance.C1holes[CAxis[axis]] + Position.Instance.C1HolesOffset[CAxis[axis]], AxisParameter.Instance.C1VelocityCurve);
                        while (true)
                        {
                            if (m_LeftC.C1Axis.IsInPosition(Position.Instance.C1holes[CAxis[axis]] + Position.Instance.C1HolesOffset[CAxis[axis]]))
                            {
                                Global.IsLocating = false;
                                CAxis[axis]++;
                                return;
                            }
                            if (!Global.IsLocating) return;
                        }
                    }, TaskCreationOptions.AttachedToParent | TaskCreationOptions.LongRunning);
                    break;
                case 1:
                    Task.Factory.StartNew(() =>
                    {
                        m_LeftC.C2Axis.MoveTo(Position.Instance.C2holes[CAxis[axis]] + Position.Instance.C2HolesOffset[CAxis[axis]], AxisParameter.Instance.C2VelocityCurve);
                        while (true)
                        {
                            if (m_LeftC.C2Axis.IsInPosition(Position.Instance.C2holes[CAxis[axis]] + Position.Instance.C2HolesOffset[CAxis[axis]]))
                            {
                                Global.IsLocating = false;
                                CAxis[axis]++;
                                return;
                            }
                            if (!Global.IsLocating) return;
                        }
                    }, TaskCreationOptions.AttachedToParent | TaskCreationOptions.LongRunning);
                    break;
                case 2:
                    Task.Factory.StartNew(() =>
                    {
                        m_LeftC.C3Axis.MoveTo(Position.Instance.C3holes[CAxis[axis]] + Position.Instance.C3HolesOffset[CAxis[axis]], AxisParameter.Instance.C3VelocityCurve);
                        while (true)
                        {
                            if (m_LeftC.C3Axis.IsInPosition(Position.Instance.C3holes[CAxis[axis]] + Position.Instance.C3HolesOffset[CAxis[axis]]))
                            {
                                Global.IsLocating = false;
                                CAxis[axis]++;
                                return;
                            }
                            if (!Global.IsLocating) return;
                        }
                    }, TaskCreationOptions.AttachedToParent | TaskCreationOptions.LongRunning);
                    break;
                case 3:
                    Task.Factory.StartNew(() =>
                    {
                        m_LeftC.C4Axis.MoveTo(Position.Instance.C4holes[CAxis[axis]] + Position.Instance.C4HolesOffset[CAxis[axis]], AxisParameter.Instance.C4VelocityCurve);
                        while (true)
                        {
                            if (m_LeftC.C4Axis.IsInPosition(Position.Instance.C4holes[CAxis[axis]] + Position.Instance.C4HolesOffset[CAxis[axis]]))
                            {
                                Global.IsLocating = false;
                                CAxis[axis]++;
                                return;
                            }
                            if (!Global.IsLocating) return;
                        }
                    }, TaskCreationOptions.AttachedToParent | TaskCreationOptions.LongRunning);
                    break;
                default:
                    Global.IsLocating = false;
                    break;
            }

            return 0;
        }
        /// <summary>
        /// 勾盘一次
        /// </summary>
        /// <param name="axis"></param>
        /// <param name="pos"></param>
        /// <returns></returns>
        private int GtrayTestOne()
        {
            if (Global.IsLocating) return -1;
            if (!m_Platform.stationInitialize.InitializeDone)
            {
                return -1;
            }
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
                        Thread.Sleep(10);
                        if (!Global.IsLocating) return -1;
                        switch (step1)
                        {
                            case 0://判断是否安全
                                   //if ((IoPoints.T2IN26.Value || IoPoints.T2IN27.Value || IoPoints.T2IN28.Value || IoPoints.T2IN29.Value)|| Marking.HaveTraySensorSheild)
                                   //{
                                step1 = 10;
                                //}
                                break;
                            case 10://判断XYZ是否安全
                                if (m_Platform.Yaxis.IsInPosition(Position.Instance.ZsafePosition.Y) && m_Platform.Zaxis.IsInPosition(Position.Instance.ZsafePosition.Z))
                                {
                                    step1 = 20;
                                }
                                else
                                {
                                    step1 = 15;
                                }
                                break;
                            case 15:
                                m_Platform.Zaxis.MoveTo(Position.Instance.ZsafePosition.Z, new VelocityCurve(0, (double)m_Platform.Zaxis.Speed, 0));
                                step1 = 16;
                                break;
                            case 16:
                                if (m_Platform.Zaxis.IsInPosition(Position.Instance.ZsafePosition.Z))
                                {
                                    m_Platform.Yaxis.MoveTo(Position.Instance.ZsafePosition.Y, new VelocityCurve(0, (double)m_Platform.Yaxis.Speed, 0));
                                    step1 = 20;
                                }
                                break;
                            case 20://锁紧气缸打开
                                if (m_Platform.Yaxis.IsInPosition(Position.Instance.ZsafePosition.Y) && m_Platform.Zaxis.IsInPosition(Position.Instance.ZsafePosition.Z))
                                {
                                    m_Platform.LockCylinder.Set();
                                    m_Platform.Yaxis.MoveTo(Position.Instance.PosGTrayOriPosition[0].Y, new VelocityCurve(0, (double)m_Platform.Yaxis.Speed, 0));
                                    step1 = 30;
                                }
                                break;
                            case 30://取盘上下气缸动作
                                if (m_Platform.LockCylinder.OutMoveStatus && m_Platform.Yaxis.IsInPosition(Position.Instance.PosGTrayOriPosition[0].Y))
                                {
                                    m_Platform.GetTrayCylinder.Set();
                                    step1 = 40;
                                }
                                break;
                            case 40://Y轴移动
                                if (m_Platform.GetTrayCylinder.OutMoveStatus)
                                {
                                    m_Platform.Yaxis.MoveTo(Position.Instance.PosGTrayMovePosition[0].Y, new VelocityCurve(0, AxisParameter.Instance.SlowvelocityMax, 0));
                                    step1 = 50;
                                }
                                break;
                            case 50://气缸上升
                                if (m_Platform.Yaxis.IsInPosition(Position.Instance.PosGTrayMovePosition[0].Y))
                                {
                                    m_Platform.GetTrayCylinder.Reset();
                                    step1 = 60;
                                }
                                break;
                            case 60://去第二次勾盘原位
                                if (m_Platform.GetTrayCylinder.OutOriginStatus)
                                {
                                    m_Platform.Yaxis.MoveTo(Position.Instance.PosGTrayOriPosition[1].Y, new VelocityCurve(0, (double)m_Platform.Yaxis.Speed, 0));
                                    step1 = 70;
                                }
                                break;
                            case 70://气缸下降
                                if (m_Platform.Yaxis.IsInPosition(Position.Instance.PosGTrayOriPosition[1].Y))
                                {
                                    m_Platform.GetTrayCylinder.Set();
                                    step1 = 80;
                                }
                                break;
                            case 80://Y轴移动
                                if (m_Platform.GetTrayCylinder.OutMoveStatus)
                                {
                                    m_Platform.Yaxis.MoveTo(Position.Instance.PosGTrayMovePosition[1].Y, new VelocityCurve(0, AxisParameter.Instance.SlowvelocityMax, 0));
                                    step1 = 90;
                                }
                                break;
                            case 90://气缸上升
                                if (m_Platform.Yaxis.IsInPosition(Position.Instance.PosGTrayMovePosition[1].Y))
                                {
                                    m_Platform.GetTrayCylinder.Reset();
                                    step1 = 100;
                                }
                                break;
                            case 100://Y轴安全位
                                if (m_Platform.GetTrayCylinder.OutOriginStatus)
                                {
                                    m_Platform.Yaxis.MoveTo(Position.Instance.ZsafePosition.Y, new VelocityCurve(0, (double)m_Platform.Yaxis.Speed, 0));
                                    step1 = 110;
                                }
                                break;
                            case 110://锁紧气缸动作
                                if (m_Platform.Yaxis.IsInPosition(Position.Instance.ZsafePosition.Y))
                                {
                                    m_Platform.LockCylinder.Reset();
                                    step1 = 120;
                                }
                                break;
                            case 120://松开气缸动作
                                if (m_Platform.LockCylinder.OutOriginStatus)
                                {
                                    m_Platform.LockCylinder.Set();
                                    step1 = 130;
                                }
                                break;
                            case 130://锁紧气缸动作
                                if (m_Platform.LockCylinder.OutMoveStatus)
                                {
                                    m_Platform.LockCylinder.Reset();
                                    step1 = 140;
                                }
                                break;
                            case 140:
                                if (m_Platform.LockCylinder.OutOriginStatus)
                                {
                                    step1 = 150;
                                }
                                break;
                            default:
                                step1 = 0;
                                Global.IsLocating = false;
                                return 0;
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
        /// <summary>
        /// 退盘一次
        /// </summary>
        /// <param name="axis"></param>
        /// <param name="pos"></param>
        /// <returns></returns>
        private int ExittrayTestOne()
        {
            if (Global.IsLocating) { return -1; }
            if (!m_Platform.stationInitialize.InitializeDone)
            {
                return -1;
            }
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
                        Thread.Sleep(10);
                        if (!Global.IsLocating) return -1;
                        switch (step1)
                        {
                            case 0://判断是否安全
                                   //if (IoPoints.T2IN26.Value || IoPoints.T2IN27.Value || IoPoints.T2IN28.Value || IoPoints.T2IN29.Value)
                                   //{
                                step1 = 10;
                                //}
                                break;
                            case 10://判断XYZ是否安全
                                if (m_Platform.Yaxis.IsInPosition(Position.Instance.ZsafePosition.Y) && m_Platform.Zaxis.IsInPosition(Position.Instance.ZsafePosition.Z))
                                {
                                    step1 = 20;
                                }
                                else
                                {
                                    step1 = 15;
                                }
                                break;
                            case 15:
                                m_Platform.Zaxis.MoveTo(Position.Instance.ZsafePosition.Z, new VelocityCurve(0, (double)m_Platform.Zaxis.Speed, 0));
                                step1 = 16;
                                break;
                            case 16:
                                if (m_Platform.Zaxis.IsInPosition(Position.Instance.ZsafePosition.Z))
                                {
                                    m_Platform.Yaxis.MoveTo(Position.Instance.ZsafePosition.Y, new VelocityCurve(0, (double)m_Platform.Yaxis.Speed, 0));
                                    step1 = 20;
                                }
                                break;
                            case 20://锁紧气缸打开
                                if (m_Platform.Yaxis.IsInPosition(Position.Instance.ZsafePosition.Y) && m_Platform.Zaxis.IsInPosition(Position.Instance.ZsafePosition.Z))
                                {
                                    m_Platform.LockCylinder.Set();
                                    m_Platform.Yaxis.MoveTo(Position.Instance.PosExitTrayOriPosition[0].Y, new VelocityCurve(0, (double)m_Platform.Yaxis.Speed, 0));
                                    step1 = 30;
                                }
                                break;
                            case 30://取盘上下气缸动作
                                if (m_Platform.LockCylinder.OutMoveStatus && m_Platform.Yaxis.IsInPosition(Position.Instance.PosExitTrayOriPosition[0].Y))
                                {
                                    m_Platform.GetTrayCylinder.Set();
                                    step1 = 40;
                                }
                                break;
                            case 40://Y轴移动
                                if (m_Platform.GetTrayCylinder.OutMoveStatus)
                                {
                                    m_Platform.Yaxis.MoveTo(Position.Instance.PosExitTrayMovePosition[0].Y, new VelocityCurve(0, AxisParameter.Instance.SlowvelocityMax, 0));
                                    step1 = 50;
                                }
                                break;
                            case 50://气缸上升
                                if (m_Platform.Yaxis.IsInPosition(Position.Instance.PosExitTrayMovePosition[0].Y))
                                {
                                    m_Platform.GetTrayCylinder.Reset();
                                    step1 = 60;
                                }
                                break;
                            case 60://去第二次勾盘原位
                                if (m_Platform.GetTrayCylinder.OutOriginStatus)
                                {
                                    m_Platform.Yaxis.MoveTo(Position.Instance.PosExitTrayOriPosition[1].Y, new VelocityCurve(0, (double)m_Platform.Yaxis.Speed, 0));
                                    step1 = 70;
                                }
                                break;
                            case 70://气缸下降
                                if (m_Platform.Yaxis.IsInPosition(Position.Instance.PosExitTrayOriPosition[1].Y))
                                {
                                    m_Platform.GetTrayCylinder.Set();
                                    step1 = 80;
                                }
                                break;
                            case 80://Y轴移动
                                if (m_Platform.GetTrayCylinder.OutMoveStatus)
                                {
                                    m_Platform.Yaxis.MoveTo(Position.Instance.PosExitTrayMovePosition[1].Y, new VelocityCurve(0, AxisParameter.Instance.SlowvelocityMax, 0));
                                    step1 = 90;
                                }
                                break;
                            case 90://气缸上升
                                if (m_Platform.Yaxis.IsInPosition(Position.Instance.PosExitTrayMovePosition[1].Y))
                                {
                                    m_Platform.GetTrayCylinder.Reset();
                                    step1 = 100;
                                }
                                break;
                            case 100://Y轴安全位
                                if (m_Platform.GetTrayCylinder.OutOriginStatus)
                                {
                                    m_Platform.Yaxis.MoveTo(Position.Instance.ZsafePosition.Y, new VelocityCurve(0, (double)m_Platform.Yaxis.Speed, 0));
                                    step1 = 110;
                                }
                                break;
                            case 110://锁紧气缸动作
                                if (m_Platform.Yaxis.IsInPosition(Position.Instance.ZsafePosition.Y))
                                {
                                    m_Platform.LockCylinder.Reset();
                                    step1 = 120;
                                }
                                break;
                            case 120:
                                if (m_Platform.LockCylinder.OutOriginStatus)
                                {
                                    step1 = 130;
                                }
                                break;
                            default:
                                step1 = 0;
                                Global.IsLocating = false;
                                return 0;
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

        private int MoveToPos(ApsAxis axis, double pos)
        {
            if (Global.IsLocating) return -1;
            if (!m_Platform.stationInitialize.InitializeDone || !m_LeftC.stationInitialize.InitializeDone || !m_RightCut1.stationInitialize.InitializeDone ||
                !m_RightCut2.stationInitialize.InitializeDone || !m_LeftCut1.stationInitialize.InitializeDone || !m_LeftCut2.stationInitialize.InitializeDone)
            {
                return -1;
            }
            Global.IsLocating = true;
            Task.Factory.StartNew(() =>
            {
                try
                {
                    //将X、Y移动到指定位置
                    if (!axis.IsInPosition(pos))
                    {
                        axis.MoveTo(pos, new VelocityCurve(0, (double)axis.Speed, 0));
                    }

                    while (true)
                    {
                        Thread.Sleep(10);
                        if (axis.IsInPosition(pos))
                        {
                            break;
                        }

                        if (axis.IsAlarmed || axis.IsMEL || axis.IsPEL || !axis.IsServon || axis.IsEmg)
                        {
                            axis.Stop();
                            Global.IsLocating = false;
                            return -4;
                        }
                        if (!Global.IsLocating) return -1;
                    }

                    return 0;
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

        private int MoveTo3Pos(ApsAxis Xaxis, ApsAxis Yaxis, ApsAxis Zaxis, Point3D<double> pos, bool CylinderReset)
        {
            if (Global.IsLocating) return -1;
            if (!m_Platform.stationInitialize.InitializeDone || !m_LeftC.stationInitialize.InitializeDone || !m_RightCut1.stationInitialize.InitializeDone ||
                !m_RightCut2.stationInitialize.InitializeDone || !m_LeftCut1.stationInitialize.InitializeDone || !m_LeftCut2.stationInitialize.InitializeDone)
            {
                return -1;
            }
            stopWatch.Start();
            Global.IsLocating = true;
            Task.Factory.StartNew(() =>
            {
                try
                {
                    Zaxis.MoveTo(0, new VelocityCurve(0, (double)Zaxis.Speed, 0));
                    while (true)
                    {
                        if (!Global.IsLocating) { stopWatch.Stop(); return -1; }
                        Thread.Sleep(5);
                        if (Zaxis.IsInPosition(0))
                        {
                            break;
                        }
                        if (Zaxis.IsAlarmed || Zaxis.IsMEL || Zaxis.IsPEL || !Zaxis.IsServon || Zaxis.IsEmg)
                        {
                            Zaxis.Stop();
                            Global.IsLocating = false;
                            stopWatch.Stop();
                            return -4;
                        }
                    }
                    if(CylinderReset)
                    {
                        m_Platform.Left1Cylinder.Reset();
                        stopWatch.Restart();
                        while (true)
                        {
                            Thread.Sleep(5);
                            if (m_Platform.Left1Cylinder.OutOriginStatus) break;                            
                            if(stopWatch.ElapsedMilliseconds >= 30000)
                            {
                                Global.IsLocating = false;
                                stopWatch.Stop();
                                return -1;
                            }
                            if (!Global.IsLocating) { stopWatch.Stop(); return -1; }
                        }
                    }
                    else
                    {
                        m_Platform.Left1Cylinder.Set();
                        stopWatch.Restart();
                        while (true)
                        {
                            Thread.Sleep(5);
                            if (m_Platform.Left1Cylinder.OutMoveStatus) break;                           
                            if (stopWatch.ElapsedMilliseconds >= 30000)
                            {
                                Global.IsLocating = false;
                                stopWatch.Stop();
                                return -1;
                            }
                            if (!Global.IsLocating) { stopWatch.Stop(); return -1; }
                        }
                    }
                    Xaxis.MoveTo(pos.X, new VelocityCurve(0, (double)Xaxis.Speed, 0));
                    Yaxis.MoveTo(pos.Y, new VelocityCurve(0, (double)Yaxis.Speed, 0));
                    while (true)
                    {
                        if (!Global.IsLocating) { stopWatch.Stop(); return -1; }
                        Thread.Sleep(5);
                        if (Xaxis.IsInPosition(pos.X) && Yaxis.IsInPosition(pos.Y))
                        {
                            break;
                        }
                        if (Xaxis.IsAlarmed || Xaxis.IsMEL || Xaxis.IsPEL || !Xaxis.IsServon || Xaxis.IsEmg ||
                        Yaxis.IsAlarmed || Yaxis.IsMEL || Yaxis.IsPEL || !Yaxis.IsServon || Yaxis.IsEmg)
                        {
                            Xaxis.Stop();
                            Yaxis.Stop();
                            Global.IsLocating = false;
                            stopWatch.Stop();
                            return -4;
                        }
                    }
                    Zaxis.MoveTo(pos.Z, new VelocityCurve(0, (double)Zaxis.Speed, 0));
                    while (true)
                    {
                        if (!Global.IsLocating) { stopWatch.Stop(); return -1; }
                        Thread.Sleep(5);
                        if (Zaxis.IsInPosition(pos.Z))
                        {
                            break;
                        }
                        if (Zaxis.IsAlarmed || Zaxis.IsMEL || Zaxis.IsPEL || !Zaxis.IsServon || Zaxis.IsEmg)
                        {
                            Zaxis.Stop();
                            Global.IsLocating = false;
                            stopWatch.Stop();
                            return -4;
                        }
                    }
                    Global.IsLocating = false;
                    stopWatch.Stop();
                    return 0;
                }
                catch (Exception ex)
                {
                    Global.IsLocating = false;
                    LogHelper.Fatal("设备驱动程序异常");
                    stopWatch.Stop();
                    return -2;
                }
            }, TaskCreationOptions.AttachedToParent | TaskCreationOptions.LongRunning);

            stopWatch.Stop();
            return 0;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Enabled = false;
            switch (name)
            {
                case PositionName.勾盘位置设定:
                    GTrayPositionView.MoveMode = moveselect.MoveMode;
                    GTrayPositionView.Refreshing();
                    break;
                case PositionName.退盘位置设定:
                    ExitTrayPositionView.MoveMode = moveselect.MoveMode;
                    ExitTrayPositionView.Refreshing();
                    break;
                case PositionName.推进P轴位置:
                    PuchPositionView.MoveMode = moveselect.MoveMode;
                    PuchPositionView.Refreshing();
                    break;
                case PositionName.Z1剪切位置:
                    CutPosition[0].MoveMode = moveselect.MoveMode;
                    CutPosition[0].Refreshing();
                    break;
                case PositionName.Z2剪切位置:
                    CutPosition[1].MoveMode = moveselect.MoveMode;
                    CutPosition[1].Refreshing();
                    break;
                case PositionName.Z3剪切位置:
                    CutPosition[2].MoveMode = moveselect.MoveMode;
                    CutPosition[2].Refreshing();
                    break;
                case PositionName.Z4剪切位置:
                    CutPosition[3].MoveMode = moveselect.MoveMode;
                    CutPosition[3].Refreshing();
                    break;
                //case PositionName.一号推料原点位置:
                //    PuchPos[0].Origin.MoveMode = moveselect.MoveMode;
                //    PuchPos[0].Origin.Refreshing();
                //    break;
                //case PositionName.一号推料终点位置:
                //    PuchPos[0].Move.MoveMode = moveselect.MoveMode;
                //    PuchPos[0].Move.Refreshing();
                //    break;
                //case PositionName.二号推料原点位置:
                //    PuchPos[1].Origin.MoveMode = moveselect.MoveMode;
                //    PuchPos[1].Origin.Refreshing();
                //    break;
                //case PositionName.二号推料终点位置:
                //    PuchPos[1].Move.MoveMode = moveselect.MoveMode;
                //    PuchPos[1].Move.Refreshing();
                //    break;
                //case PositionName.三号推料原点位置:
                //    PuchPos[2].Origin.MoveMode = moveselect.MoveMode;
                //    PuchPos[2].Origin.Refreshing();
                //    break;
                //case PositionName.三号推料终点位置:
                //    PuchPos[2].Move.MoveMode = moveselect.MoveMode;
                //    PuchPos[2].Move.Refreshing();
                //    break;
                case PositionName.Z轴安全位置:
                    ZSafePositionView.MoveMode = moveselect.MoveMode;
                    ZSafePositionView.Refreshing();
                    break;
                case PositionName.XYZ模组取产品位置:
                    GetProductPositionView.MoveMode = moveselect.MoveMode;
                    GetProductPositionView.Refreshing();
                    break;
                case PositionName.XYZ模组放产品第一个位置:
                    PuchProductPositionView.MoveMode = moveselect.MoveMode;
                    PuchProductPositionView.Refreshing();
                    break;
                case PositionName.料仓位置:
                    mLayerView.MoveMode = moveselect.MoveMode;
                    mLayerView.Refreshing();
                    break;
                case PositionName.抽检位置:
                    SelectCheckPositionView.MoveMode = moveselect.MoveMode;
                    SelectCheckPositionView.Refreshing();
                    break;
                case PositionName.C轴设定:
                    mCAxisPositionSet.MoveMode = moveselect.MoveMode;
                    mCAxisPositionSet.Refreshing();
                    break;
                default:
                    break;
            }
            timer1.Enabled = true;
        }

        private bool ServoAxisIsReady(ServoAxis axis)
        {
            return !axis.IsServon || axis.IsAlarmed || axis.IsEmg || axis.IsMEL || axis.IsPEL;
        }

        public enum PositionName
        {
            勾盘位置设定,
            退盘位置设定,
            推进P轴位置,
            Z1剪切位置,
            Z2剪切位置,
            Z3剪切位置,
            Z4剪切位置,
            Z轴安全位置,
            XYZ模组取产品位置,
            料仓位置,
            抽检位置,
            C轴设定,
            XYZ模组放产品第一个位置,
        }
    }
}
