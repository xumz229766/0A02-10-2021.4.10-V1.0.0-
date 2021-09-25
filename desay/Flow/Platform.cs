using System.Collections.Generic;
using System.Enginee;
using System.Interfaces;
using System.Threading;
using System.Drawing;
using System.ToolKit;
using System;
using System.Diagnostics;
namespace desay
{
    /// <summary>
    /// XYZ平台模组
    /// </summary>
    public class Platform : ThreadPart
    {
        private PlateformAlarm m_Alarm, m_AlarmX, m_AlarmY;
        public List<Alarm> Alarms;
        public Platform(External ExternalSign, StationInitialize stationIni, StationOperate stationOpe)
        {
            externalSign = ExternalSign;
            stationInitialize = stationIni;
            stationOperate = stationOpe;
        }
        public StationInitialize stationInitialize { get; set; }
        public StationOperate stationOperate { get; set; }
        public External externalSign { get; set; }

        #region 属性
        /// <summary>
        /// X轴
        /// </summary>
        public ServoAxis Xaxis { get; set; }
        /// <summary>
        /// Y轴
        /// </summary>
        public ServoAxis Yaxis { get; set; }
        /// <summary>
        /// Z轴
        /// </summary>
        public ServoAxis Zaxis { get; set; }
        /// <summary>
        /// 左右气缸(1#,2#,4#)
        /// </summary>
        public SingleCylinder3 Left1Cylinder { get; set; }
        /// <summary>
        /// 左右2#气缸
        /// </summary>
  //      public SingleCylinder2 Left2Cylinder { get; set; }
        /// <summary>
        /// 1#吸笔吸气电磁阀
        /// </summary>
        public VacuoBrokenCylinder Left1InhaleCylinder { get; set; }
        /// <summary>
        /// 2#吸笔吸气电磁阀
        /// </summary>
        public VacuoBrokenCylinder Left2InhaleCylinder { get; set; }
        /// <summary>
        /// 3#吸笔吸气电磁阀
        /// </summary>
        public VacuoBrokenCylinder Right1InhaleCylinder { get; set; }
        /// <summary>
        /// 4#吸笔吸气电磁阀
        /// </summary>
        public VacuoBrokenCylinder Right2InhaleCylinder { get; set; }
        /// <summary>
        /// 取放盘上下气缸
        /// </summary>
        public SingleCylinder2 GetTrayCylinder { get; set; }
        /// <summary>
        /// 摆盘卡紧气缸
        /// </summary>
        public SingleCylinder LockCylinder { get; set; }

        public static int step = 0;
        /// <summary>
        /// 抽检开始
        /// </summary>
        private bool SelectCheckStart;

        /// <summary>
        /// 托盘定位次数
        /// </summary>   
        private int TrayPositonNum = 1;

        /// <summary>
        /// 回待机位
        /// </summary>   
        public int homeWaitStep = 0;

        #endregion

        public override void Running(RunningModes runningMode)
        {
            try
            {
                Stopwatch watch = new Stopwatch();
                watch.Start();
                Point3D<double> pos = new Point3D<double>();
                Marking.watchXYZ.Restart();

                while (true)
                {
                    Thread.Sleep(10);
                    if (!stationOperate.Running) { Marking.watchXYZ.Stop(); } else { Marking.watchXYZ.Start(); }

                    #region 自动流程
                    if (stationOperate.Running)
                    {
                        switch (stationOperate.step)
                        {
                            case 0://判断是否有托盘                          
                                if (GetTrayCylinder.OutOriginStatus)
                                {
                                    if ((IoPoints.T2IN26.Value && IoPoints.T2IN27.Value && !IoPoints.T2IN28.Value && !IoPoints.T2IN29.Value) || Marking.traySensorSheild)
                                    {
                                        if (LockCylinder.OutOriginStatus)
                                        {
                                            if (Config.Instance.SelectCheckRunState)
                                            {
                                                Marking.IsMMoveChangeTrayPos = 4;
                                                if (Marking.StoreFinish)
                                                {
                                                    Config.Instance.SelectCheckRunState = false;
                                                    if (Global.BigTray.CurrentPos < Global.BigTray.EndPos && Global.SmallTray.CurrentPos < Global.SmallTray.EndPos)
                                                    {
                                                        Marking.HookLayerPlate = true;
                                                    }
                                                    stationOperate.step = 270;
                                                }
                                            }
                                            else
                                            {
                                                Marking.IsMMoveChangeTrayPos = 1;
                                                stationOperate.step = 130;
                                            }
                                        }
                                        else
                                        {
                                            LockCylinder.Reset();
                                        }
                                    }
                                    else if (!IoPoints.T2IN26.Value && !IoPoints.T2IN27.Value && !IoPoints.T2IN28.Value && !IoPoints.T2IN29.Value)
                                    {
                                        TrayPositonNum = 1;         //初始化托盘定位次数
                                        Marking.IsMMoveChangeTrayPos = 2;
                                        stationOperate.step = 10;
                                    }
                                    else
                                    {
                                        if (!IoPoints.T2IN26.Value || !IoPoints.T2IN27.Value)
                                        {
                                            m_Alarm = IoPoints.T2IN26.Value ? PlateformAlarm.T2IN26摆盘前感应故障 : PlateformAlarm.T2IN27摆盘前感应故障;
                                        }
                                        else if (IoPoints.T2IN28.Value || IoPoints.T2IN29.Value)
                                        {
                                            m_Alarm = IoPoints.T2IN28.Value ? PlateformAlarm.T2IN28摆盘后感应故障 : PlateformAlarm.T2IN29摆盘后感应故障;
                                        }
                                    }
                                }
                                else
                                {
                                    m_Alarm = PlateformAlarm.取放盘气缸没有复位;
                                }
                                break;
                            case 10://判断气缸是否在原点，Z轴是否在安全位置
                                if (GetTrayCylinder.OutOriginStatus && Marking.StoreFinish)
                                {
                                    Marking.GettingPlate = true;
                                    if (!Zaxis.IsInPosition(Position.Instance.ZsafePosition.Z))
                                    {
                                        Zaxis.MoveTo(Position.Instance.ZsafePosition.Z, AxisParameter.Instance.ZVelocityCurve);
                                    }
                                    stationOperate.step = 20;
                                }
                                break;
                            case 20://检测Z轴是否到达安全位置，判断Y轴是否在安全位置
                                if (Zaxis.IsInPosition(Position.Instance.ZsafePosition.Z))
                                {
                                    if (!Yaxis.IsInPosition(Position.Instance.ZsafePosition.Y))
                                    {
                                        if (!Marking.MIsNoMoving) Yaxis.MoveTo(Position.Instance.ZsafePosition.Y, AxisParameter.Instance.YVelocityCurve);
                                    }
                                    else
                                    {
                                        stationOperate.step = 30;
                                    }
                                }
                                break;
                            case 30://检测Y轴是否到达安全位置,卡紧气缸为ON
                                if (Yaxis.IsInPosition(Position.Instance.ZsafePosition.Y) && LockCylinder.Condition.IsOnCondition)
                                {
                                    LockCylinder.Set();//卡紧松开
                                    stationOperate.step = 40;
                                }
                                break;
                            case 40://判断M轴是否移动，仓储是否准备好
                                if (LockCylinder.OutMoveStatus && Marking.StoreFinish && !Marking.ChangeTrayPadlock)
                                {
                                    Yaxis.MoveTo(Position.Instance.PosGTrayOriPosition[0].Y, AxisParameter.Instance.YVelocityCurve);
                                    stationOperate.step = 50;
                                }
                                break;
                            case 50://判断Y轴是否到达首次取Tray位置，取放料盘气缸为ON
                                if (Yaxis.IsInPosition(Position.Instance.PosGTrayOriPosition[0].Y) && GetTrayCylinder.Condition.IsOnCondition)
                                {
                                    GetTrayCylinder.Set();
                                    stationOperate.step = 60;
                                }
                                break;
                            case 60://取放料盘气缸到位，Y轴移动首次放Tray盘位置
                                if (GetTrayCylinder.OutMoveStatus && !Marking.ChangeTrayPadlock)
                                {
                                    Yaxis.MoveTo(Position.Instance.PosGTrayMovePosition[0].Y, AxisParameter.Instance.SlowvelocityCurve);
                                    stationOperate.step = 70;
                                }
                                break;
                            case 70://判断Y轴是否到达首次放Tray盘位置,取放料盘气缸为OFF
                                if (Yaxis.IsInPosition(Position.Instance.PosGTrayMovePosition[0].Y) && GetTrayCylinder.Condition.IsOffCondition)
                                {
                                    if ((IoPoints.T2IN28.Value && IoPoints.T2IN29.Value) || Marking.traySensorSheild)
                                    {
                                        GetTrayCylinder.Reset();
                                        stationOperate.step = 80;
                                    }
                                    else
                                    {
                                        if (!IoPoints.T2IN28.Value || !IoPoints.T2IN29.Value)
                                        {
                                            m_Alarm = IoPoints.T2IN28.Value ? PlateformAlarm.T2IN28摆盘后感应故障 : PlateformAlarm.T2IN29摆盘后感应故障;
                                        }
                                    }
                                }
                                break;
                            case 80://取放料盘气缸到位，Y轴移动二次放Tray盘位置
                                if (GetTrayCylinder.OutOriginStatus && !Marking.ChangeTrayPadlock)
                                {
                                    Yaxis.MoveTo(Position.Instance.PosGTrayOriPosition[1].Y, AxisParameter.Instance.YVelocityCurve);
                                    stationOperate.step = 90;
                                }
                                break;
                            case 90://判断Y轴是否到达二次取Tray位置，取放料盘气缸为ON
                                if (Yaxis.IsInPosition(Position.Instance.PosGTrayOriPosition[1].Y) && GetTrayCylinder.Condition.IsOnCondition)
                                {
                                    GetTrayCylinder.Set();
                                    stationOperate.step = 100;
                                }
                                break;
                            case 100://判断取放盘气缸到位，Y轴移动二次放Tray盘位置
                                if (GetTrayCylinder.OutMoveStatus && !Marking.ChangeTrayPadlock)
                                {
                                    Yaxis.MoveTo(Position.Instance.PosGTrayMovePosition[1].Y, AxisParameter.Instance.SlowvelocityCurve);
                                    stationOperate.step = 103;
                                }
                                break;
                            case 103://判断Y轴是否到达二次放Tray盘位置,卡紧气缸为OFF
                                if (Yaxis.IsInPosition(Position.Instance.PosGTrayMovePosition[1].Y))
                                {
                                    GetTrayCylinder.Reset();
                                    stationOperate.step = 105;
                                }
                                break;
                            case 105://卡紧气缸为ON                            
                                if (GetTrayCylinder.OutOriginStatus && LockCylinder.Condition.IsOffCondition)
                                {
                                    if (TrayPositonNum < Position.Instance.numTrayPositon)
                                    {
                                        if (LockCylinder.OutMoveStatus)
                                        {
                                            TrayPositonNum++;
                                            LockCylinder.Reset();
                                            stationOperate.step = 106;
                                        }
                                    }
                                    else
                                    {
                                        stationOperate.step = 110;
                                    }
                                }
                                break;
                            case 106://卡紧气缸为ON                            
                                if (LockCylinder.Condition.IsOnCondition && LockCylinder.OutOriginStatus)
                                {
                                    LockCylinder.Set();
                                    stationOperate.step = 105;
                                }
                                break;
                            case 110://卡紧气缸为OFF，取放盘气缸为OFF
                                if (LockCylinder.Condition.IsOffCondition && LockCylinder.OutMoveStatus)
                                {
                                    LockCylinder.Reset();
                                    stationOperate.step = 120;
                                }
                                break;
                            case 120://卡紧气缸，取放盘气缸到达
                                if (LockCylinder.OutOriginStatus)
                                {
                                    if ((IoPoints.T2IN26.Value && IoPoints.T2IN27.Value && !IoPoints.T2IN28.Value && !IoPoints.T2IN29.Value) || Marking.traySensorSheild)
                                    {
                                        Marking.MancelChangeTray = false;
                                        Marking.MancelChangeRunState = false;
                                        Marking.IsMMoveChangeTrayPos = 3;
                                        if (Config.Instance.SpecialTrayStart) Global.SpecialTray.CurrentPos = 0; //开启特殊盘
                                        if (Marking.SelectTarySign)
                                        {
                                            Marking.SelectTarySign = false;
                                            Global.BigTray.CurrentPos = Global.SelectCheckBigPos;
                                            Global.SmallTray.CurrentPos = Global.SelectCheckSmallPos;
                                        }
                                        else
                                        {
                                            Global.BigTray.CurrentPos = 0;
                                            Global.SmallTray.CurrentPos = 0;
                                        }
                                        Global.SmallTray.ResetTrayColor(Color.Gray);
                                        Global.SmallTray.updateColor();
                                        stationOperate.step = 130;
                                    }
                                    else
                                    {
                                        if (!IoPoints.T2IN26.Value || !IoPoints.T2IN27.Value)
                                        {
                                            m_Alarm = IoPoints.T2IN26.Value ? PlateformAlarm.T2IN26摆盘前感应故障 : PlateformAlarm.T2IN27摆盘前感应故障;
                                        }
                                        else if (IoPoints.T2IN28.Value || IoPoints.T2IN29.Value)
                                        {
                                            m_Alarm = IoPoints.T2IN28.Value ? PlateformAlarm.T2IN28摆盘后感应故障 : PlateformAlarm.T2IN29摆盘后感应故障;
                                        }
                                    }
                                }
                                break;
                            case 130://判断Z轴是否在安全位置
                                Marking.watchXYZValue = Marking.watchXYZ.ElapsedMilliseconds / 1000.0;
                                Marking.watchXYZ.Restart();
                                Marking.GettingPlate = false;
                                if (!Zaxis.IsInPosition(Position.Instance.PuchSafetyZ))
                                {
                                    Zaxis.MoveTo(Position.Instance.PuchSafetyZ, AxisParameter.Instance.ZVelocityCurve);
                                }
                                try//获取大盘的第一个位置,通过第一个位置来获取小盘实际位置
                                {
                                    if(((Position.Instance.MLayerFillCount[Position.Instance.MLayerIndex] == 0 && Global.SmallTray.CurrentPos >= (Global.SmallTray.EndPos - 1)) ||
                                       (Position.Instance.MLayerFillCount[Position.Instance.MLayerIndex] != 0 && Global.SmallTray.CurrentPos >= (Position.Instance.MLayerFillCount[0] - 1)) ||
                                       (SelectCheckStart && Global.SmallTray.CurrentPos >= (Position.Instance.SelectCheckModulus - 1)) ||
                                       (!(Marking.SelectCheckMode && SelectCheckStart) && Marking.SelectCheckMode) || Marking.MancelChangeTray) && !Marking.traySensorSheild)
                                    {
                                        Marking.changeTrayLayoutWaitSign = true;
                                    }

                                    if (!SelectCheckStart) //摆盘坐标计算
                                    {
                                        if (Config.Instance.SpecialTrayStart) //开启特殊盘，计算坐标
                                        {
                                            if (Global.SpecialTray.CurrentPos >= Global.SpecialTray.EndPos) { Global.SpecialTray.CurrentPos = 0; }
                                            Point3D<double> pos1 = Global.SpecialTray.GetPosition(Position.Instance.PuchProductPosition, Global.SpecialTray.CurrentPos + 1);
                                            if (Global.BigTray.CurrentPos < Global.BigTray.EndPos)
                                            {
                                                Point3D<double> pos2 = Global.BigTray.GetPosition(pos1, Global.BigTray.CurrentPos + 1);
                                                if (Global.SmallTray.CurrentPos < Global.SmallTray.EndPos)
                                                {
                                                    pos = Global.SmallTray.GetPosition(pos2, Global.SmallTray.CurrentPos + 1);
                                                }
                                                else
                                                {
                                                    Global.SmallTray.CurrentPos = 0;
                                                    pos = Global.SmallTray.GetPosition(pos2, Global.SmallTray.CurrentPos + 1);
                                                }
                                            }
                                            else
                                            {
                                                Global.BigTray.CurrentPos = 0;
                                                Point3D<double> pos2 = Global.BigTray.GetPosition(pos1, Global.BigTray.CurrentPos + 1);
                                                if (Global.SmallTray.CurrentPos < Global.SmallTray.EndPos)
                                                {
                                                    pos = Global.SmallTray.GetPosition(pos2, Global.SmallTray.CurrentPos + 1);
                                                }
                                                else
                                                {
                                                    Global.SmallTray.CurrentPos = 0;
                                                    pos = Global.SmallTray.GetPosition(pos2, Global.SmallTray.CurrentPos + 1);
                                                }
                                            }
                                        }
                                        else
                                        {
                                            if (Global.BigTray.CurrentPos >= Global.BigTray.EndPos) { Global.BigTray.CurrentPos = 0; }
                                            Point3D<double> pos1 = Global.BigTray.GetPosition(Position.Instance.PuchProductPosition, Global.BigTray.CurrentPos + 1);
                                            if (Global.SmallTray.CurrentPos < Global.SmallTray.EndPos)
                                            {
                                                pos = Global.SmallTray.GetPosition(pos1, Global.SmallTray.CurrentPos + 1);
                                            }
                                            else
                                            {
                                                Global.SmallTray.CurrentPos = 0;
                                                pos = Global.SmallTray.GetPosition(pos1, Global.SmallTray.CurrentPos + 1);
                                            }
                                        }
                                    }
                                    else if (Position.Instance.SelectCheckModulus < Global.SmallTray.EndPos) //抽检坐标计算
                                    {
                                        if (Global.BigTray.CurrentPos >= Global.BigTray.EndPos) { Global.BigTray.CurrentPos = 0; }
                                        Point3D<double> pos1 = Global.BigTray.GetPosition(Position.Instance.SelectCheckPosition, Global.BigTray.CurrentPos + 1);
                                        if (Global.SmallTray.CurrentPos < Global.SmallTray.EndPos)
                                        {
                                            pos = Global.SmallTray.GetPosition(pos1, Global.SmallTray.CurrentPos + 1);
                                        }
                                        else
                                        {
                                            Global.SmallTray.CurrentPos = 0;
                                            pos = Global.SmallTray.GetPosition(pos1, Global.SmallTray.CurrentPos + 1);
                                        }
                                    }
                                    else
                                    {
                                        m_Alarm = PlateformAlarm.抽检大盘穴数超出最大穴数;
                                    }
                                    stationOperate.step = 135;
                                }
                                catch
                                {
                                    m_Alarm = PlateformAlarm.获取Tray盘坐标点错误;
                                }
                                break;
                            case 135://取料时左右气缸移动到动点
                                if (Zaxis.IsInPosition(Position.Instance.PuchSafetyZ))
                                {
                                    Left1Cylinder.Reset();
                                    stationOperate.step = 140;
                                }
                                break;
                            case 140://XY轴移动取产品位置                               
                                if (!Marking.LeftCut1Done && !Marking.LeftCut2Done && !Marking.RightCut1Done && !Marking.RightCut2Done)//剪切数量完成
                                {
                                    Xaxis.MoveTo(Position.Instance.GetProductPosition.X, AxisParameter.Instance.XVelocityCurve);
                                    Yaxis.MoveTo(Position.Instance.GetProductPosition.Y, AxisParameter.Instance.YVelocityCurve);
                                    Zaxis.MoveTo(Position.Instance.GetSafetyZ, AxisParameter.Instance.ZVelocityCurve);
                                    stationOperate.step = 150;
                                }
                                else
                                {
                                    Marking.IsMMoveChangeTrayPos = 4;
                                    stationOperate.step = 270;
                                }
                                break;
                            case 150:////XY到达取产品位置,判断左右剪切是否准备好                          
                                if (Xaxis.IsInPosition(Position.Instance.GetProductPosition.X) && Yaxis.IsInPosition(Position.Instance.GetProductPosition.Y) &&
                                    Zaxis.IsInPosition(Position.Instance.GetSafetyZ) && Left1Cylinder.OutOriginStatus && Left1Cylinder.Condition.IsOffCondition)
                                {
                                    if ((Marking.LeftCut1Finish && Marking.LeftCut2Finish && Marking.RightCut1Finish && Marking.RightCut2Finish))
                                    {
                                        if (Marking.CutCount[0] == Global.BigTray.CurrentPos && Marking.CutCount[1] == Global.BigTray.CurrentPos &&
                                            Marking.CutCount[2] == Global.BigTray.CurrentPos && Marking.CutCount[3] == Global.BigTray.CurrentPos)
                                        {
                                            Zaxis.MoveTo(Position.Instance.GetProductPosition.Z, AxisParameter.Instance.ZVelocityCurve);
                                            Left1InhaleCylinder.Delay.InhaleTime = Delay.Instance.InspireDelay[0];
                                            Left2InhaleCylinder.Delay.InhaleTime = Delay.Instance.InspireDelay[0];
                                            Right1InhaleCylinder.Delay.InhaleTime = Delay.Instance.InspireDelay[0];
                                            Right2InhaleCylinder.Delay.InhaleTime = Delay.Instance.InspireDelay[0];
                                            Left1InhaleCylinder.Set();
                                            Left2InhaleCylinder.Set();
                                            Right1InhaleCylinder.Set();
                                            Right2InhaleCylinder.Set();
                                            stationOperate.step = 160;
                                        }
                                        else if (Marking.CutCount[0] == Marking.CutCount[1] && Marking.CutCount[1] == Marking.CutCount[2]
                                            && Marking.CutCount[2] == Marking.CutCount[3])
                                        {
                                            Global.BigTray.CurrentPos = Marking.CutCount[0];
                                        }
                                        else
                                        {
                                            m_Alarm = PlateformAlarm.剪切穴号错误;
                                        }
                                    }
                                }
                                break;
                            case 160://判断Z轴到达，左右吸笔为ON
                                if (Zaxis.IsInPosition(Position.Instance.GetProductPosition.Z))
                                {
                                    Thread.Sleep(Delay.Instance.ZaxisDoneDelay);
                                    watch.Restart();
                                    stationOperate.step = 170;
                                }
                                break;
                            case 170://判断左右吸笔到位，吸笔吸气标志输出
                                if (watch.ElapsedMilliseconds > Delay.Instance.InspireDelay[0])
                                {
                                    Marking.XYZLeftInhale1Sign = true;
                                    Marking.XYZLeftInhale2Sign = true;
                                    Marking.XYZRightInhale1Sign = true;
                                    Marking.XYZRightInhale2Sign = true;
                                    stationOperate.step = 180;
                                }
                                break;
                            case 180://等待左右剪切清除吸笔吸气输出标志，Z轴移动安全位置
                                if (Marking.XYZCut1Finish && Marking.XYZCut2Finish && Marking.XYZCut3Finish && Marking.XYZCut4Finish)
                                {
                                    Zaxis.MoveTo(Position.Instance.GetSafetyZ, AxisParameter.Instance.ZVelocityCurve);
                                    stationOperate.step = 185;
                                }
                                break;
                            case 185:
                                if ((Zaxis.BackPos < (Position.Instance.GetProductPosition.Z - Position.Instance.ZInformHeight)) || Zaxis.IsInPosition(Position.Instance.GetSafetyZ))
                                {
                                    Marking.XYZLeftInhale1Sign = false;
                                    Marking.XYZLeftInhale2Sign = false;
                                    Marking.XYZRightInhale1Sign = false;
                                    Marking.XYZRightInhale2Sign = false;
                                    stationOperate.step = 190;
                                }
                                break;
                            case 190://Z轴到达安全位置，XY移动对应点位置,吸笔左右气缸为ON
                                if (Left1Cylinder.OutOriginStatus && (!Marking.XYZCut1Finish && !Marking.XYZCut2Finish && !Marking.XYZCut3Finish && !Marking.XYZCut4Finish))
                                {
                                    Marking.ZUpTrayLensFinish[0] = true;
                                    Marking.ZUpTrayLensFinish[1] = true;
                                    Marking.ZUpTrayLensFinish[2] = true;
                                    Marking.ZUpTrayLensFinish[3] = true;
                                    if (Zaxis.IsInPosition(Position.Instance.GetSafetyZ))
                                    {
                                        pos.X = pos.X + Position.Instance.trayOffces[Global.BigTray.CurrentPos].X;
                                        pos.Y = pos.Y + Position.Instance.trayOffces[Global.BigTray.CurrentPos].Y;
                                        Yaxis.MoveTo(pos.Y, AxisParameter.Instance.YVelocityCurve);
                                        Zaxis.MoveTo(Position.Instance.PuchSafetyZ, AxisParameter.Instance.ZVelocityCurve);
                                        stationOperate.step = 200;
                                    }
                                }
                                break;
                            case 200:
                                if (Yaxis.BackPos < (Position.Instance.GetProductPosition.Y - Position.Instance.YAvoidDistance) || Yaxis.IsInPosition(pos.Y))
                                {
                                    Xaxis.MoveTo(pos.X, AxisParameter.Instance.XVelocityCurve);
                                    Left1Cylinder.Set();
                                    stationOperate.step = 210;
                                }
                                break;
                            case 210://XY到达对应点位置，吸笔左右气缸到达，Z轴移动对应点位置
                                if (Xaxis.IsInPosition(pos.X) && Yaxis.IsInPosition(pos.Y) && Zaxis.IsInPosition(Position.Instance.PuchSafetyZ)
                                    && Left1Cylinder.OutMoveStatus)
                                {
                                    if (Position.Instance.ZxiasUp > 0)
                                    {
                                        Zaxis.MoveToExtern(pos.Z + Position.Instance.trayOffces[Global.BigTray.CurrentPos].Z - Position.Instance.ZxiasUp,
                                                           pos.Z + Position.Instance.trayOffces[Global.BigTray.CurrentPos].Z, AxisParameter.Instance.ZVelocityCurve);
                                    }
                                    else
                                    {
                                        Zaxis.MoveTo(pos.Z + Position.Instance.trayOffces[Global.BigTray.CurrentPos].Z, AxisParameter.Instance.ZVelocityCurve);
                                    }
                                    stationOperate.step = 220;
                                }
                                break;
                            case 220://Z轴到达对应点位置，左右吸笔为OFF，破真空
                                if (Zaxis.IsInPosition(pos.Z + Position.Instance.trayOffces[Global.BigTray.CurrentPos].Z))
                                {
                                    Thread.Sleep(Delay.Instance.ZaxisDoneDelay);
                                    Left1InhaleCylinder.Delay.BrokenTime = Delay.Instance.SoproDelay[0] + Delay.Instance.ZSoproDelay[0];
                                    Left2InhaleCylinder.Delay.BrokenTime = Delay.Instance.SoproDelay[0] + Delay.Instance.ZSoproDelay[0];
                                    Right1InhaleCylinder.Delay.BrokenTime = Delay.Instance.SoproDelay[0] + Delay.Instance.ZSoproDelay[0];
                                    Right2InhaleCylinder.Delay.BrokenTime = Delay.Instance.SoproDelay[0] + Delay.Instance.ZSoproDelay[0];
                                    Left1InhaleCylinder.Reset();
                                    Left2InhaleCylinder.Reset();
                                    Right1InhaleCylinder.Reset();
                                    Right2InhaleCylinder.Reset();
                                    watch.Restart();
                                    stationOperate.step = 230;
                                }
                                break;
                            case 230://左右吸笔到达
                                if (watch.ElapsedMilliseconds > Delay.Instance.SoproDelay[0])
                                {
                                    Thread.Sleep(Delay.Instance.InspireStopDelay[0]);
                                    Zaxis.MoveTo(Position.Instance.PuchSafetyZ, AxisParameter.Instance.ZVelocityCurve);
                                    try
                                    {
                                        Global.BigTray.SetNumColor(Global.BigTray.CurrentPos + 1, Color.Green);
                                        Global.BigTray.updateColor();
                                        if (!SelectCheckStart)//抽检不刷新状态
                                        {
                                            if (Config.Instance.SpecialTrayStart)
                                            {
                                                if (Global.SpecialTray.CurrentPos >= (Global.SpecialTray.EndPos - 1))
                                                {
                                                    Global.SmallTray.SetNumColor(Global.SmallTray.CurrentPos + 1, Color.Green);
                                                    Global.SmallTray.updateColor();
                                                }
                                            }
                                            else
                                            {
                                                Global.SmallTray.SetNumColor(Global.SmallTray.CurrentPos + 1, Color.Green);
                                                Global.SmallTray.updateColor();
                                            }
                                        }
                                        Global.BigTray.CurrentPos++;
                                    }
                                    catch (Exception ex) { throw ex; }
                                    stationOperate.step = 240;
                                }
                                break;
                            case 240://Z轴到达安全位置
                                try
                                {
                                    if (Zaxis.IsInPosition(Position.Instance.PuchSafetyZ))
                                    {
                                        //判断大盘是否完成数量
                                        if (Global.BigTray.CurrentPos < Global.BigTray.EndPos)
                                        {
                                            stationOperate.step = 130;
                                        }
                                        else
                                        {
                                            if (Config.Instance.SpecialTrayStart && !SelectCheckStart) Global.SpecialTray.CurrentPos++;
                                            stationOperate.step = 260;
                                        }
                                    }
                                }
                                catch (Exception ex) { throw ex; }
                                break;
                            case 260: //复位托盘颜色，小盘计算+1
                                Global.BigTray.ResetTrayColor(Color.Gray);
                                Global.BigTray.updateColor();
                                if (SelectCheckStart)
                                {
                                    Global.SmallTray.CurrentPos++;
                                    if (Global.SmallTray.CurrentPos < Position.Instance.SelectCheckModulus)//判断抽检是否完成
                                    {
                                        Global.BigTray.CurrentPos = 0;
                                        stationOperate.step = 130;
                                    }
                                    else
                                    {
                                        stationOperate.step = 265;
                                    }
                                }
                                else
                                {
                                    Global.BigTray.CurrentPos = 0;
                                    if (Config.Instance.SpecialTrayStart)
                                    {
                                        if (Global.SpecialTray.CurrentPos >= Global.SpecialTray.EndPos)
                                        {
                                            Global.SpecialTray.CurrentPos = 0;
                                            Global.SmallTray.CurrentPos++;
                                        }
                                    }
                                    else
                                    {
                                        Global.SmallTray.CurrentPos++;
                                    }
                                    stationOperate.step = 265;
                                }
                                break;
                            case 265://抽检、强制换盘、周期停止
                                if (Marking.SelectCheckMode)
                                {
                                    if (SelectCheckStart && Marking.SelectCheckMode)
                                    {
                                        SelectCheckStart = false;
                                        Marking.SelectCheckModeFinish = true;
                                        Config.Instance.SelectCheckRunState = false;
                                        if (Global.SelectCheckBigPos < Global.BigTray.EndPos && Global.SelectCheckSmallPos < Global.SmallTray.EndPos)
                                        {
                                            Marking.SelectTarySign = true;
                                            Marking.HookLayerPlate = true;
                                        }
                                        Marking.IsMMoveChangeTrayPos = 4;
                                        stationOperate.step = 270;
                                        break;
                                    }//抽检一穴完
                                    if (Marking.SelectCheckMode && !SelectCheckStart && !Marking.SelectCheckModeFinish)//抽检模式
                                    {
                                        if (!SelectCheckStart)
                                        {
                                            SelectCheckStart = true;
                                            Config.Instance.SelectCheckRunState = true;
                                            Global.SelectCheckBigPos = Global.BigTray.CurrentPos;
                                            Global.SelectCheckSmallPos = Global.SmallTray.CurrentPos;
                                            Marking.IsMMoveChangeTrayPos = 4;
                                            stationOperate.step = 270;
                                            break;
                                        }
                                    }
                                }
                                else { SelectCheckStart = false; }
                                if (Marking.MancelChangeTray)//手动强制换盘
                                {
                                    Marking.MancelChangeRunState = true;
                                    Marking.IsMMoveChangeTrayPos = 4;
                                    stationOperate.step = 270;
                                    break;
                                }
                                if (!Marking.SystemStop)
                                {
                                    if (Position.Instance.MLayerFillCount[Position.Instance.MLayerIndex] != 0)//如果仓储换盘数量设定了，则跑设定数量
                                    {
                                        if ((Global.SmallTray.CurrentPos < Position.Instance.MLayerFillCount[0]))//判断
                                        {
                                            Global.BigTray.CurrentPos = 0;
                                            stationOperate.step = 130;
                                        }
                                        else
                                        {
                                            Marking.IsMMoveChangeTrayPos = 4;
                                            stationOperate.step = 270;
                                        }
                                        break;
                                    }
                                    if (Global.SmallTray.CurrentPos < Global.SmallTray.EndPos)//判断一盘数量是否完成
                                    {
                                        Global.BigTray.CurrentPos = 0;
                                        stationOperate.step = 130;
                                    }
                                    else
                                    {
                                        Marking.IsMMoveChangeTrayPos = 4;
                                        stationOperate.step = 270;
                                    }
                                }
                                break;
                            case 270://Tray盘大于设定值，判断仓储轴是否动作,卡紧气缸为ON
                                if (!Marking.MIsNoMoving && LockCylinder.Condition.IsOnCondition && Marking.StoreFinish)
                                {
                                    if (!Marking.MSelectIndex) Config.Instance.TrayFinishcount++;
                                    Marking.ModelCount = 0;
                                    Marking.changeTrayLayoutSign = true;
                                    Marking.PuttingPlate = true;
                                    LockCylinder.Set();
                                    stationOperate.step = 280;
                                }
                                break;
                            case 280://判断卡紧气缸气缸到位，Y轴移动二次放Tray盘位置
                                if (LockCylinder.OutMoveStatus && !Marking.ChangeTrayPadlock)
                                {
                                    Yaxis.MoveTo(Position.Instance.PosExitTrayOriPosition[0].Y, AxisParameter.Instance.YVelocityCurve);
                                    stationOperate.step = 290;
                                }
                                break;
                            case 290://判断Y轴是否到达二次放Tray盘位置,取放盘气缸为ON
                                if (Yaxis.IsInPosition(Position.Instance.PosExitTrayOriPosition[0].Y) && GetTrayCylinder.Condition.IsOnCondition)
                                {
                                    GetTrayCylinder.Set();
                                    stationOperate.step = 300;
                                }
                                break;
                            case 300://判断取放盘气缸到位，Y轴移动二次取Tray盘位置
                                if (GetTrayCylinder.OutMoveStatus && !Marking.ChangeTrayPadlock)
                                {
                                    Yaxis.MoveTo(Position.Instance.PosExitTrayMovePosition[0].Y, AxisParameter.Instance.SlowExitvelocityCurve);
                                    stationOperate.step = 310;
                                }
                                break;
                            case 310://判断Y轴是否到达二次取Tray盘位置,取放盘气缸为OFF
                                if (Yaxis.IsInPosition(Position.Instance.PosExitTrayMovePosition[0].Y) && GetTrayCylinder.Condition.IsOffCondition)
                                {
                                    GetTrayCylinder.Reset();
                                    stationOperate.step = 320;
                                }
                                break;
                            case 320://判断取放盘气缸到位，Y轴移动首次放Tray盘位置
                                if (GetTrayCylinder.OutOriginStatus && !Marking.ChangeTrayPadlock)
                                {
                                    Yaxis.MoveTo(Position.Instance.PosExitTrayOriPosition[1].Y, AxisParameter.Instance.YVelocityCurve);
                                    stationOperate.step = 330;
                                }
                                break;
                            case 330://判断Y轴是否到达首次放Tray盘位置,取放料气缸为ON
                                if (Yaxis.IsInPosition(Position.Instance.PosExitTrayOriPosition[1].Y) && GetTrayCylinder.Condition.IsOnCondition)
                                {
                                    GetTrayCylinder.Set();
                                    stationOperate.step = 340;
                                }
                                break;
                            case 340://判断取放盘气缸到位，Y轴移动首次取Tray盘位置
                                if (GetTrayCylinder.OutMoveStatus && !Marking.ChangeTrayPadlock)
                                {
                                    Yaxis.MoveTo(Position.Instance.PosExitTrayMovePosition[1].Y, AxisParameter.Instance.SlowExitvelocityCurve);
                                    stationOperate.step = 350;
                                }
                                break;
                            case 350://判断Y轴是否到达首次取Tray盘位置,取放料气缸为Off
                                if (Yaxis.IsInPosition(Position.Instance.PosExitTrayMovePosition[1].Y) && GetTrayCylinder.Condition.IsOffCondition)
                                {
                                    GetTrayCylinder.Reset();
                                    stationOperate.step = 360;
                                }
                                break;
                            case 360://判断取放盘气缸到位，Y轴移动安全位置
                                if (GetTrayCylinder.OutOriginStatus && !Marking.ChangeTrayPadlock)
                                {

                                    Yaxis.MoveTo(Position.Instance.ZsafePosition.Y, AxisParameter.Instance.YVelocityCurve);
                                    stationOperate.step = 370;
                                }
                                break;
                            case 370://判断Y轴是否到达安全位置,复位取Tray完成信号、仓储准备好信号
                                if (Yaxis.IsInPosition(Position.Instance.ZsafePosition.Y) && !Marking.ChangeTrayPadlock)
                                {
                                    if ((!IoPoints.T2IN26.Value && !IoPoints.T2IN27.Value && !IoPoints.T2IN28.Value &&
                                        !IoPoints.T2IN29.Value && IoPoints.T2IN18.Value && IoPoints.T2IN19.Value) || Marking.traySensorSheild)
                                    {
                                        Marking.LeftCut1Done = false;
                                        Marking.LeftCut2Done = false;
                                        Marking.RightCut1Done = false;
                                        Marking.RightCut2Done = false;
                                        Marking.IsMMoveChangeTrayPos = 5;
                                        Marking.StoreFinish = false;
                                        Marking.PuttingPlate = false;
                                        stationOperate.step = 380;
                                    }
                                    else
                                    {
                                        if (IoPoints.T2IN26.Value || IoPoints.T2IN27.Value)
                                        {
                                            m_Alarm = IoPoints.T2IN26.Value ? PlateformAlarm.T2IN26摆盘前感应故障 : PlateformAlarm.T2IN27摆盘前感应故障;
                                        }
                                        else if (IoPoints.T2IN28.Value || IoPoints.T2IN29.Value)
                                        {
                                            m_Alarm = IoPoints.T2IN28.Value ? PlateformAlarm.T2IN28摆盘后感应故障 : PlateformAlarm.T2IN29摆盘后感应故障;
                                        }
                                        else if (!IoPoints.T2IN18.Value || !IoPoints.T2IN19.Value)
                                        {
                                            m_Alarm = !IoPoints.T2IN18.Value ? PlateformAlarm.T2IN18左卡盘感应故障 : PlateformAlarm.T2IN19右卡盘感应故障;
                                        }
                                    }
                                }
                                break;
                            case 380:
                                if (!Marking.StoreFinish) stationOperate.step = 390;
                                break;
                            default:
                                stationOperate.RunningSign = false;
                                stationOperate.step = 0;
                                break;
                        }
                    }
                    #endregion

                    #region 初始化流程
                    if (stationInitialize.Running)
                    {
                        switch (stationInitialize.Flow)
                        {
                            case 0:   //清除所有标志位的状态
                                stationInitialize.InitializeDone = false;
                                stationOperate.RunningSign = false;
                                stationOperate.step = 0;
                                Marking.StoreFinish = false;
                                Marking.SelectCheckMode = false;
                                Marking.SelectCheckModeFinish = false;
                                SelectCheckStart = false;
                                Marking.XYZLeftInhale1Sign = false;
                                Marking.XYZLeftInhale2Sign = false;
                                Marking.XYZRightInhale1Sign = false;
                                Marking.XYZRightInhale2Sign = false;
                                Marking.ZUpTrayLensFinish[0] = false;
                                Marking.ZUpTrayLensFinish[1] = false;
                                Marking.ZUpTrayLensFinish[2] = false;
                                Marking.ZUpTrayLensFinish[3] = false;
                                Marking.GettingPlate = false;
                                Marking.PuttingPlate = false;
                                Marking.MancelChangeTray = false;
                                Marking.IsMMoveChangeTrayPos = 0;
                                Global.SpecialTray.CurrentPos = 0;
                                if (Global.BigTray.CurrentPos > 0)
                                {
                                    Global.SmallTray.CurrentPos++;
                                }
                                Global.BigTray.CurrentPos = 0;
                                GetTrayCylinder.InitExecute(); GetTrayCylinder.Reset();
                                m_Alarm = PlateformAlarm.取放盘上下气缸复位中;
                                stationInitialize.Flow = 10;
                                break;
                            case 10:
                                if (GetTrayCylinder.OutOriginStatus && !Xaxis.IsAlarmed && !Yaxis.IsAlarmed && !Zaxis.IsAlarmed)
                                {
                                    m_Alarm = PlateformAlarm.Z轴复位中;
                                    Xaxis.IsServon = true;
                                    Yaxis.IsServon = true;
                                    Zaxis.IsServon = true;
                                    Thread.Sleep(500);
                                    stationInitialize.Flow = 20;
                                }
                                break;
                            case 20:
                                if (Xaxis.IsDone && Yaxis.IsDone && Zaxis.IsDone && Xaxis.IsInPosition(Xaxis.CurrentPos)
                                    && Yaxis.IsInPosition(Yaxis.CurrentPos) && Zaxis.IsInPosition(Zaxis.CurrentPos))
                                {
                                    stationInitialize.Flow = 30;
                                }
                                else
                                {
                                    Xaxis.Stop();
                                    Yaxis.Stop();
                                    Zaxis.Stop();
                                }
                                break;
                            case 30:  //若Z轴在原点位异常，启动Z轴寸动循环                          
                                Zaxis.BackHome();
                                Thread.Sleep(30);
                                stationInitialize.Flow = 40;
                                break;
                            case 40://判断Z轴是否动作完成
                                if (Zaxis.IsInPosition(0))
                                {
                                    Zaxis.MoveTo(Position.Instance.ZsafePosition.Z, AxisParameter.Instance.ZHomeVelocityCurve);
                                    stationInitialize.Flow = 50;
                                }
                                break;
                            case 50://启动Z轴回原点
                                if (Zaxis.IsInPosition(Position.Instance.ZsafePosition.Z))
                                {
                                    m_Alarm = PlateformAlarm.无消息;
                                    Xaxis.BackHome(); m_AlarmX = PlateformAlarm.X轴复位中;
                                    Yaxis.BackHome(); m_AlarmY = PlateformAlarm.Y轴复位中;
                                    stationInitialize.Flow = 60;
                                }
                                break;
                            case 60://判断Z轴回原点是否完成，Z轴移动安全位置
                                if (Yaxis.IsInPosition(0) && Xaxis.IsInPosition(0))
                                {
                                    Xaxis.MoveTo(Position.Instance.ZsafePosition.X, AxisParameter.Instance.XVelocityCurve);
                                    Yaxis.MoveTo(Position.Instance.ZsafePosition.Y, AxisParameter.Instance.YVelocityCurve);
                                    stationInitialize.Flow = 70;
                                }
                                break;
                            case 70://复位所有气缸的动作
                                if (Yaxis.IsInPosition(Position.Instance.ZsafePosition.Y) && Xaxis.IsInPosition(Position.Instance.ZsafePosition.X))
                                {
                                    Left1Cylinder.InitExecute(); Left1Cylinder.Reset();
                                    m_Alarm = PlateformAlarm.吸笔左右气缸复位中;
                                    Left1InhaleCylinder.InitExecute(); Left1InhaleCylinder.Reset();
                                    Left2InhaleCylinder.InitExecute(); Left2InhaleCylinder.Reset();
                                    Right1InhaleCylinder.InitExecute(); Right1InhaleCylinder.Reset();
                                    Right2InhaleCylinder.InitExecute(); Right2InhaleCylinder.Reset();
                                    IoPoints.T1DO31.Value = false;
                                    IoPoints.T1DO29.Value = false;
                                    IoPoints.T1DO27.Value = false;
                                    IoPoints.T1DO25.Value = false;
                                    stationInitialize.Flow = 80;
                                }
                                if (Xaxis.IsInPosition(Position.Instance.ZsafePosition.X)) m_AlarmX = PlateformAlarm.无消息;
                                if (Yaxis.IsInPosition(Position.Instance.ZsafePosition.Y)) m_AlarmY = PlateformAlarm.无消息;
                                break;
                            case 80:    //判断所有气缸到位，启动Z轴回原点
                                if (Left1Cylinder.OutOriginStatus)
                                {
                                    LockCylinder.InitExecute(); LockCylinder.Reset();
                                    m_Alarm = PlateformAlarm.摆盘卡紧气缸复位中;
                                    stationInitialize.Flow = 90;
                                }
                                break;
                            case 90:
                                if (LockCylinder.OutOriginStatus)
                                {
                                    m_Alarm = PlateformAlarm.无消息;
                                    stationInitialize.InitializeDone = true;
                                }
                                break;
                            default:
                                break;
                        }
                    }
                    #endregion

                    #region  回初始位
                    if (externalSign.GoRristatus)
                    {
                        switch (homeWaitStep)
                        {
                            case 0:
                                stationOperate.step = 0;
                                Marking.StoreFinish = false;
                                Marking.SelectCheckMode = false;
                                Marking.SelectCheckModeFinish = false;
                                SelectCheckStart = false;
                                Marking.XYZLeftInhale1Sign = false;
                                Marking.XYZLeftInhale2Sign = false;
                                Marking.XYZRightInhale1Sign = false;
                                Marking.XYZRightInhale2Sign = false;
                                Marking.ZUpTrayLensFinish[0] = false;
                                Marking.ZUpTrayLensFinish[1] = false;
                                Marking.ZUpTrayLensFinish[2] = false;
                                Marking.ZUpTrayLensFinish[3] = false;
                                Marking.GettingPlate = false;
                                Marking.PuttingPlate = false;
                                Marking.MancelChangeTray = false;
                                Marking.IsMMoveChangeTrayPos = 0;
                                GetTrayCylinder.InitExecute(); GetTrayCylinder.Reset();
                                LockCylinder.InitExecute(); LockCylinder.Reset();
                                Xaxis.IsServon = true;
                                Yaxis.IsServon = true;
                                Zaxis.IsServon = true;
                                Thread.Sleep(500);
                                Zaxis.MoveTo(Position.Instance.ZsafePosition.Z, AxisParameter.Instance.ZHomeVelocityCurve);
                                homeWaitStep = 10;
                                break;
                            case 10:
                                if (Zaxis.IsInPosition(Position.Instance.ZsafePosition.Z) && GetTrayCylinder.OutOriginStatus)
                                {
                                    Xaxis.MoveTo(Position.Instance.ZsafePosition.X, AxisParameter.Instance.XVelocityCurve);
                                    Yaxis.MoveTo(Position.Instance.ZsafePosition.Y, AxisParameter.Instance.YVelocityCurve);
                                    homeWaitStep = 20;
                                }
                                break;
                            case 20:
                                if (Yaxis.IsInPosition(Position.Instance.ZsafePosition.Y) && Xaxis.IsInPosition(Position.Instance.ZsafePosition.X))
                                {
                                    Left1Cylinder.InitExecute(); Left1Cylinder.Reset();
                                    Left1InhaleCylinder.InitExecute(); Left1InhaleCylinder.Reset();
                                    Left2InhaleCylinder.InitExecute(); Left2InhaleCylinder.Reset();
                                    Right1InhaleCylinder.InitExecute(); Right1InhaleCylinder.Reset();
                                    Right2InhaleCylinder.InitExecute(); Right2InhaleCylinder.Reset();
                                    IoPoints.T1DO31.Value = false;
                                    IoPoints.T1DO29.Value = false;
                                    IoPoints.T1DO27.Value = false;
                                    IoPoints.T1DO25.Value = false;
                                    Marking.equipmentHomeWaitState[8] = true;
                                    homeWaitStep = 30;
                                }
                                break;
                            default:
                                break;
                        }
                    }
                    #endregion

                    #region  故障清除
                    if (externalSign.AlarmReset && !stationInitialize.Running)
                    {
                        m_Alarm = PlateformAlarm.无消息;
                    }
                    #endregion
                }
            }
            catch(Exception ex)
            {
                throw(ex);
            }
        }

        /// <summary>
        /// 气缸状态集合
        /// </summary>
        public IList<ICylinderStatusJugger> CylinderStatus
        {
            get
            {
                var list = new List<ICylinderStatusJugger>();
                list.Add(Left1Cylinder);
                list.Add(Left1InhaleCylinder);
                list.Add(Left2InhaleCylinder);
                list.Add(Right1InhaleCylinder);
                list.Add(Right2InhaleCylinder);
                list.Add(GetTrayCylinder);
                list.Add(LockCylinder);
                return list;
            }
        }

        public void AddAlarms()
        {
            try 
            { 
            
            }
            catch(Exception ex)
            {
                throw(ex);
            }
            Alarms = new List<Alarm>();
            Alarms.Add(new Alarm(() => m_Alarm == PlateformAlarm.取放盘气缸没有复位)
            {
                AlarmLevel = AlarmLevels.Error,
                Name = PlateformAlarm.取放盘气缸没有复位.ToString()
            });
            Alarms.Add(new Alarm(() => m_Alarm == PlateformAlarm.T2IN26摆盘前感应故障)
            {
                AlarmLevel = AlarmLevels.Error,
                Name = PlateformAlarm.T2IN26摆盘前感应故障.ToString()
            });
            Alarms.Add(new Alarm(() => m_Alarm == PlateformAlarm.T2IN27摆盘前感应故障)
            {
                AlarmLevel = AlarmLevels.Error,
                Name = PlateformAlarm.T2IN27摆盘前感应故障.ToString()
            });
            Alarms.Add(new Alarm(() => m_Alarm == PlateformAlarm.T2IN28摆盘后感应故障)
            {
                AlarmLevel = AlarmLevels.Error,
                Name = PlateformAlarm.T2IN28摆盘后感应故障.ToString()
            });
            Alarms.Add(new Alarm(() => m_Alarm == PlateformAlarm.T2IN29摆盘后感应故障)
            {
                AlarmLevel = AlarmLevels.Error,
                Name = PlateformAlarm.T2IN29摆盘后感应故障.ToString()
            });
            Alarms.Add(new Alarm(() => m_Alarm == PlateformAlarm.T2IN18左卡盘感应故障)
            {
                AlarmLevel = AlarmLevels.Error,
                Name = PlateformAlarm.T2IN18左卡盘感应故障.ToString()
            });
            Alarms.Add(new Alarm(() => m_Alarm == PlateformAlarm.T2IN19右卡盘感应故障)
            {
                AlarmLevel = AlarmLevels.Error,
                Name = PlateformAlarm.T2IN19右卡盘感应故障.ToString()
            });
            Alarms.Add(new Alarm(() => m_Alarm == PlateformAlarm.获取Tray盘坐标点错误)
            {
                AlarmLevel = AlarmLevels.Error,
                Name = PlateformAlarm.获取Tray盘坐标点错误.ToString()
            });
            Alarms.Add(new Alarm(() => m_Alarm == PlateformAlarm.抽检大盘穴数超出最大穴数)
            {
                AlarmLevel = AlarmLevels.Error,
                Name = PlateformAlarm.抽检大盘穴数超出最大穴数.ToString()
            });
            Alarms.Add(new Alarm(() => m_Alarm == PlateformAlarm.取放盘上下气缸复位中) { AlarmLevel = AlarmLevels.None, Name = PlateformAlarm.取放盘上下气缸复位中.ToString() });
            Alarms.Add(new Alarm(() => m_Alarm == PlateformAlarm.Z轴复位中) { AlarmLevel = AlarmLevels.None, Name = PlateformAlarm.Z轴复位中.ToString() });
            Alarms.Add(new Alarm(() => m_AlarmX == PlateformAlarm.X轴复位中) { AlarmLevel = AlarmLevels.None, Name = PlateformAlarm.X轴复位中.ToString() });
            Alarms.Add(new Alarm(() => m_AlarmY == PlateformAlarm.Y轴复位中) { AlarmLevel = AlarmLevels.None, Name = PlateformAlarm.Y轴复位中.ToString() });
            Alarms.Add(new Alarm(() => m_Alarm == PlateformAlarm.吸笔左右气缸复位中) { AlarmLevel = AlarmLevels.None, Name = PlateformAlarm.吸笔左右气缸复位中.ToString() });
            Alarms.Add(new Alarm(() => m_Alarm == PlateformAlarm.摆盘卡紧气缸复位中) { AlarmLevel = AlarmLevels.None, Name = PlateformAlarm.摆盘卡紧气缸复位中.ToString() });
            Alarms.Add(new Alarm(() => m_Alarm == PlateformAlarm.剪切穴号错误) { AlarmLevel = AlarmLevels.None, Name = PlateformAlarm.剪切穴号错误.ToString() });
            Alarms.AddRange(Xaxis.Alarms);
            Alarms.AddRange(Yaxis.Alarms);
            Alarms.AddRange(Zaxis.Alarms);
            Alarms.AddRange(Left1Cylinder.Alarms);
            Alarms.AddRange(GetTrayCylinder.Alarms);
            Alarms.AddRange(LockCylinder.Alarms);
            Alarms.AddRange(Left1InhaleCylinder.Alarms);
            Alarms.AddRange(Left2InhaleCylinder.Alarms);
            Alarms.AddRange(Right1InhaleCylinder.Alarms);
            Alarms.AddRange(Right2InhaleCylinder.Alarms);
        }
    }
}
