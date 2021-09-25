using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Enginee;
using System.Interfaces;
using System.Threading;

namespace desay
{
    /// <summary>
    /// 左剪切模块
    /// </summary>
    public class LeftCut1 : ThreadPart
    {
        private Left1CutAlarm m_Alarm;
        public List<Alarm> Alarms;
        public LeftCut1(External ExternalSign, StationInitialize stationIni, StationOperate stationOpe)
        {
            externalSign = ExternalSign;
            stationInitialize = stationIni;
            stationOperate = stationOpe;
        }
        public StationInitialize stationInitialize { get; set; }
        public StationOperate stationOperate { get; set; }
        public External externalSign { get; set; }
        /// <summary>
        /// 剪切轴
        /// </summary>
        public StepAxis CutAxis { get; set; }
        /// <summary>
        /// 前后气缸
        /// </summary>
        public SingleCylinder FrontCylinder { get; set; }
        /// <summary>
        /// 上下气缸
        /// </summary>
        public SingleCylinder OverturnCylinder { get; set; }
        /// <summary>
        /// 夹爪气缸
        /// </summary>
        public SingleCylinder GripperCylinder { get; set; }
        /// <summary>
        ///烫刀次数
        /// </summary>
        private int HotCutCount;
        /// <summary>
        /// 回待机位
        /// </summary>
        public int homeWaitStep = 0;

        public override void Running(RunningModes runningMode)
        {
            try
            {
                var step = 0;
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();

                while (true)
                {
                    Thread.Sleep(10);

                    #region 自动流程
                    if (stationOperate.Running)
                    {
                        switch (step)
                        {
                            case 0://判断是否有料
                                if (CutAxis.IsInPosition(Position.Instance.PosCut[0].Origin))
                                {
                                    step = 10;
                                }
                                else
                                {
                                    CutAxis.MoveTo(Position.Instance.PosCut[0].Origin, AxisParameter.Instance.Cut1VelocityCurve);
                                }
                                break;
                            case 10://前后气缸为ON
                                if (FrontCylinder.OutOriginStatus && OverturnCylinder.OutOriginStatus
                                    && GripperCylinder.OutOriginStatus && GripperCylinder.Condition.IsOnCondition && Marking.CAxisFinish)
                                {
                                    FrontCylinder.Set();
                                    Marking.CutSheild[0] = false;
                                    step = 20;
                                }
                                break;
                            case 20://判断蜂窝该穴位是否屏蔽
                                if (FrontCylinder.OutMoveStatus)
                                {
                                    if (Marking.CutCount[0] >= Position.Instance.HoleNumber / 4) { Marking.CutCount[0] = 0; }
                                    if (!Position.Instance.Caxis[0].IsSheild && !Position.Instance.C1axisSheild[Marking.CutCount[0]])
                                    {
                                        step = 30;
                                    }
                                    else
                                    {
                                        Marking.CutSheild[0] = true;
                                        FrontCylinder.Reset();
                                        step = 160;
                                    }
                                }
                                break;
                            case 30://上下气缸到位，夹子气缸为ON
                                if (GripperCylinder.Condition.IsOffCondition)
                                {
                                    GripperCylinder.Set();
                                    step = 40;
                                }
                                break;
                            case 40://夹子气缸到位,判断是否扭力剪切
                                if (GripperCylinder.OutMoveStatus)
                                {
                                    if (!Position.Instance.Caxis[0].PosTorsion)
                                    {
                                        step = 60;
                                    }
                                    else
                                    {
                                        step = 50;
                                    }
                                }
                                break;
                            case 50://启动伺服1#扭力剪切控制
                                if (CutAxis.TorqueControl(Position.Instance.PosCut[0].Move, Config.Instance.PressCut[0], Position.Instance.PosCutEnd[0], 3000, AxisParameter.Instance.Cut1VelocityCurve))
                                {
                                    step = 80;
                                }
                                else //扭力止动
                                {
                                    m_Alarm = Left1CutAlarm.Z1力矩剪切未切断;
                                    FrontCylinder.Reset();
                                    step = 150;
                                }
                                break;
                            case 60://伺服1#移动到剪切，启动运行
                                if (Position.Instance.PosCutEnd[0] > Position.Instance.PosCut[0].Move)
                                {
                                    CutAxis.MoveToExtern(Position.Instance.PosCut[0].Move, Position.Instance.PosCutEnd[0], AxisParameter.Instance.Cut1VelocityCurve);
                                    step = 80;
                                }
                                else
                                {
                                    m_Alarm = Left1CutAlarm.Z1闭合位需大于缓冲位;
                                    step = 150;
                                }
                                break;
                            case 80://伺服1#到位。
                                if (CutAxis.IsInPosition(Position.Instance.PosCutEnd[0]))
                                {
                                    stopwatch.Restart();
                                    step = 90;
                                }
                                break;
                            case 90://剪切闭合延时 判断是否烫修及烫修模式
                                if (stopwatch.ElapsedMilliseconds >= Delay.Instance.CutDelay[0])
                                {
                                    if (Position.Instance.Caxis[0].HotCut)
                                    {
                                        HotCutCount = 0;
                                        step = 100;
                                    }
                                    else
                                    {
                                        FrontCylinder.Reset();
                                        step = 140;
                                    }
                                }
                                break;
                            case 100:
                                if (Position.Instance.Caxis[0].ControlOpen)
                                {
                                    FrontCylinder.Condition.NoOriginShield = true;
                                    stopwatch.Restart();
                                    IoPoints.T1DO16.Value = false;
                                }
                                else
                                {
                                    FrontCylinder.Reset();
                                }
                                step = 110;
                                break;
                            case 110:
                                if (Position.Instance.Caxis[0].ControlOpen)
                                {
                                    if (stopwatch.ElapsedMilliseconds >= Position.Instance.Caxis[0].JourneyControl && !IoPoints.T2IN1.Value)
                                    {
                                        FrontCylinder.Set();
                                        step = 130;
                                    }
                                }
                                else if (FrontCylinder.OutOriginStatus)
                                {
                                    OverturnCylinder.Set();
                                    step = 120;
                                }
                                break;
                            case 120:
                                if (OverturnCylinder.OutMoveStatus)
                                {
                                    FrontCylinder.Set();
                                    stopwatch.Restart();
                                    step = 130;
                                }
                                break;
                            case 130:
                                if (FrontCylinder.OutMoveStatus && stopwatch.ElapsedMilliseconds >= Position.Instance.Caxis[0].HotCutTime)
                                {
                                    HotCutCount++;
                                    if (HotCutCount >= Position.Instance.Caxis[0].HotCutCount)
                                    {
                                        if (Position.Instance.Caxis[0].ControlOpen)
                                        {
                                            FrontCylinder.Condition.NoOriginShield = false;
                                        }
                                        FrontCylinder.Reset();
                                        step = 140;
                                    }
                                    else
                                    {
                                        step = 100;
                                    }
                                }
                                break;
                            case 140:
                                if (Position.Instance.OverturnOpen[0])
                                {
                                    OverturnCylinder.Set();
                                }
                                step = 150;
                                break;
                            case 150:
                                CutAxis.MoveTo(Position.Instance.PosCut[0].Origin, AxisParameter.Instance.Cut1VelocityCurve);
                                step = 155;
                                break;
                            case 155:
                                if (Position.Instance.OverturnOpen[0])
                                {
                                    if (OverturnCylinder.OutMoveStatus)
                                    {
                                        step = 160;
                                    }
                                }
                                else
                                {
                                    step = 160;
                                }
                                break;
                            case 160:
                                if (CutAxis.IsInPosition(Position.Instance.PosCut[0].Origin) && FrontCylinder.OutOriginStatus)
                                {
                                    Marking.LeftCut1Finish = true;
                                    step = 170;
                                }
                                break;
                            case 170://判断切料准备好，吸笔吸气标志状态，夹子气缸为OFF
                                if (Marking.XYZLeftInhale1Sign && GripperCylinder.Condition.IsOffCondition)
                                {
                                    GripperCylinder.Reset();
                                    step = 180;
                                }
                                break;
                            case 180:
                                if (GripperCylinder.OutOriginStatus && GripperCylinder.Condition.IsOnCondition)
                                {
                                    Marking.XYZCut1Finish = true;
                                    step = 190;
                                }
                                break;
                            case 190://夹子气缸到位，切料准备状态复位
                                if (!Marking.XYZLeftInhale1Sign)
                                {
                                    if (Position.Instance.OverturnOpen[0])
                                    {
                                        OverturnCylinder.Reset();
                                    }
                                    Marking.XYZCut1Finish = false;
                                    step = 200;
                                }
                                break;
                            case 200://判断吸笔吸气状态
                                if (!Marking.XYZLeftInhale1Sign && OverturnCylinder.OutOriginStatus && Marking.ZUpTrayLensFinish[0])
                                {
                                    Marking.ZUpTrayLensFinish[0] = false;
                                    step = 210;
                                }
                                break;
                            case 210://前后气缸到位，输出完成状态
                                if (CutAxis.IsInPosition(Position.Instance.PosCut[0].Origin))
                                {
                                    Config.Instance.CutaxisCount[0]++;
                                    Config.Instance.CutaxisCountTotal[0]++;

                                    Marking.LeftCut1Finish = false;
                                    Marking.CutCount[0]++;
                                    step = 220;
                                }
                                break;
                            default:
                                step = 0;
                                break;
                        }
                    }
                    #endregion

                    #region 初始化流程
                    if (stationInitialize.Running)
                    {
                        switch (stationInitialize.Flow)
                        {
                            case 0:
                                stationInitialize.InitializeDone = false;
                                stationOperate.RunningSign = false;
                                step = 0;
                                Marking.CutCount[0] = 0;
                                Marking.LeftCut1HaveProduct = false;
                                Marking.LeftCut1Finish = false;
                                Marking.LeftCut1Done = false;
                                Marking.DuplicateClipReset = false;
                                FrontCylinder.InitExecute(); FrontCylinder.Reset();
                                m_Alarm = Left1CutAlarm.Z1前后气缸复位中;
                                stationInitialize.Flow = 10;
                                break;
                            case 10:
                                if (FrontCylinder.OutOriginStatus)
                                {
                                    OverturnCylinder.InitExecute(); OverturnCylinder.Reset();
                                    m_Alarm = Left1CutAlarm.Z1翻转气缸复位中;
                                    stationInitialize.Flow = 15;
                                }
                                break;
                            case 15:
                                if (OverturnCylinder.OutOriginStatus)
                                {
                                    GripperCylinder.InitExecute(); GripperCylinder.Reset();
                                    m_Alarm = Left1CutAlarm.Z1夹爪气缸复位中;
                                    stationInitialize.Flow = 20;
                                }
                                break;
                            case 20:
                                if (GripperCylinder.OutOriginStatus)
                                {
                                    m_Alarm = Left1CutAlarm.Z1剪切轴复位中;
                                    CutAxis.IsServon = true;
                                    Thread.Sleep(500);
                                    stationInitialize.Flow = 25;
                                }
                                break;
                            case 25:
                                if (CutAxis.IsDone && CutAxis.IsInPosition(CutAxis.CurrentPos))
                                {
                                    stationInitialize.Flow = 30;
                                }
                                else
                                {
                                    CutAxis.Stop();
                                }
                                break;
                            case 30:
                                CutAxis.BackHome();
                                stationInitialize.Flow = 40;
                                Thread.Sleep(200);
                                break;
                            case 40:
                                Thread.Sleep(10);
                                if (CutAxis.IsInPosition(0))
                                {
                                    CutAxis.MoveTo(Position.Instance.PosCut[0].Origin, AxisParameter.Instance.Cut1VelocityCurve);
                                    stationInitialize.Flow = 50;
                                }
                                break;
                            case 50:
                                Thread.Sleep(10);
                                if (CutAxis.IsInPosition(Position.Instance.PosCut[0].Origin))
                                {
                                    m_Alarm = Left1CutAlarm.无消息;
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
                                step = 0;
                                Marking.CutCount[0] = Global.BigTray.CurrentPos;
                                Marking.LeftCut1HaveProduct = false;
                                Marking.LeftCut1Finish = false;
                                Marking.LeftCut1Done = false;
                                Marking.DuplicateClipReset = false;
                                CutAxis.IsServon = true;
                                Thread.Sleep(500);
                                CutAxis.MoveTo(Position.Instance.PosCut[0].Origin, AxisParameter.Instance.Cut1VelocityCurve);
                                homeWaitStep = 10;
                                break;
                            case 10:
                                if (CutAxis.IsInPosition(Position.Instance.PosCut[0].Origin))
                                {
                                    OverturnCylinder.InitExecute(); OverturnCylinder.Reset();
                                    FrontCylinder.InitExecute(); FrontCylinder.Reset();
                                    GripperCylinder.InitExecute(); GripperCylinder.Reset();
                                    Marking.equipmentHomeWaitState[4] = true;
                                    homeWaitStep = 20;
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
                        m_Alarm = Left1CutAlarm.无消息;
                    }
                    #endregion
                }
            }
            catch (Exception ex)
            {
                throw (ex);
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
                list.Add(FrontCylinder);
                list.Add(OverturnCylinder);
                list.Add(GripperCylinder);
                return list;
            }
        }

        public void AddAlarms()
        {
            try
            {
                Alarms = new List<Alarm>();
                Alarms.Add(new Alarm(() => m_Alarm == Left1CutAlarm.Z1力矩剪切未切断) { AlarmLevel = AlarmLevels.Error, Name = Left1CutAlarm.Z1力矩剪切未切断.ToString() });
                Alarms.Add(new Alarm(() => m_Alarm == Left1CutAlarm.Z1闭合位需大于缓冲位) { AlarmLevel = AlarmLevels.Error, Name = Left1CutAlarm.Z1闭合位需大于缓冲位.ToString() });
                Alarms.Add(new Alarm(() => m_Alarm == Left1CutAlarm.Z1前后气缸复位中) { AlarmLevel = AlarmLevels.None, Name = Left1CutAlarm.Z1前后气缸复位中.ToString() });
                Alarms.Add(new Alarm(() => m_Alarm == Left1CutAlarm.Z1翻转气缸复位中) { AlarmLevel = AlarmLevels.None, Name = Left1CutAlarm.Z1翻转气缸复位中.ToString() });
                Alarms.Add(new Alarm(() => m_Alarm == Left1CutAlarm.Z1夹爪气缸复位中) { AlarmLevel = AlarmLevels.None, Name = Left1CutAlarm.Z1夹爪气缸复位中.ToString() });
                Alarms.Add(new Alarm(() => m_Alarm == Left1CutAlarm.Z1剪切轴复位中) { AlarmLevel = AlarmLevels.None, Name = Left1CutAlarm.Z1剪切轴复位中.ToString() });
                Alarms.AddRange(CutAxis.Alarms);
                Alarms.AddRange(FrontCylinder.Alarms);
                Alarms.AddRange(OverturnCylinder.Alarms);
                Alarms.AddRange(GripperCylinder.Alarms);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

    }
}
