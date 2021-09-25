using System;
using System.Collections.Generic;
using System.Enginee;
using System.Interfaces;
using System.Threading;
using System.Diagnostics;

namespace desay
{
    /// <summary>
    /// 接料模组
    /// </summary>
    public class Splice : ThreadPart
    {
        private SpliceAlarm m_Alarm;
        public List<Alarm> Alarms;
        private readonly Stopwatch _watch = new Stopwatch();
        public Splice(External ExternalSign, StationInitialize stationIni, StationOperate stationOpe)
        {
            externalSign = ExternalSign;
            stationInitialize = stationIni;
            stationOperate = stationOpe;
        }
        public StationInitialize stationInitialize { get; set; }
        public StationOperate stationOperate { get; set; }
        public External externalSign { get; set; }

        public ServoAxis LoadAxis { get; set; }
        /// <summary>
        /// 无杆接料气缸
        /// </summary>
        public DoubleCylinder NoRodFeedCylinder { get; set; }
        /// <summary>
        /// 停止时接料气缸移动待料位途中标记
        /// </summary>
        private bool stopRunningExcessiveSign = false;
        /// <summary>
        /// 回原点后接料气缸到待料位
        /// </summary>
        private bool homeWaitCylinderSign = false;
        /// <summary>
        /// 回待机位
        /// </summary>
        public int homeWaitStep = 0;

        public override void Running(RunningModes runningMode)
        {
            try
            {
                var step = 0;
                var throwerStep = 0;
                bool spliceSensorLevel = false; //来料光电电平
                int materialAbnormalCount = 0;  //接料异常计数
                Stopwatch watchSplice = new Stopwatch();
                Stopwatch noMaterialAlarm = new Stopwatch();
                watchSplice.Start();
                noMaterialAlarm.Start();

                while (true)
                {
                    Thread.Sleep(10);

                    #region  自动流程
                    if (stationOperate.Running)
                    {
                        switch (step)
                        {
                            case 0: //判断是否报警
                                if (Marking.AlarmStopRun)
                                {
                                    step = 5;
                                }
                                break;
                            case 5:
                                if (homeWaitCylinderSign)
                                {
                                    homeWaitCylinderSign = false;
                                    watchSplice.Restart();
                                    step = 50;
                                }
                                else
                                {
                                    step = 10;
                                }
                                break;
                            case 10://判断是否来料
                                if ((((IoPoints.T1IN3.Value && !Config.Instance.SignalReverseSplice) || (!IoPoints.T1IN3.Value && Config.Instance.SignalReverseSplice))
                                    || Marking.SpliceSensorSheild) && !Marking.NoRodFinish && IoPoints.T1IN18.Value)
                                {
                                    materialAbnormalCount = 0;
                                    NoRodFeedCylinder.Condition.NoMoveShield = false;
                                    NoRodFeedCylinder.Set();//上升                                                       
                                    step = 20;
                                }
                                break;
                            case 20://上升完成
                                if (NoRodFeedCylinder.OutMoveStatus && (((!IoPoints.T1IN3.Value && !Config.Instance.SignalReverseSplice)
                                    || (IoPoints.T1IN3.Value && Config.Instance.SignalReverseSplice)) || Marking.SpliceSensorSheild) && IoPoints.T1IN18.Value)
                                {
                                    NoRodFeedCylinder.Reset();
                                    step = 30;
                                }
                                break;
                            case 30: //下降完成
                                if (NoRodFeedCylinder.OutOriginStatus)
                                {
                                    Marking.NoRodFinish = true;
                                    step = 40;
                                }
                                break;
                            case 40://等待缓冲取料
                                if (!Marking.NoRodFinish)
                                {
                                    NoRodFeedCylinder.Condition.NoMoveShield = true;
                                    watchSplice.Restart();
                                    step = 50;
                                }
                                break;
                            case 50:
                                IoPoints.T1DO0.Value = false;
                                IoPoints.T1DO1.Value = true;
                                stopRunningExcessiveSign = true;
                                if (watchSplice.ElapsedMilliseconds >= Delay.Instance.SpliceDelay || IoPoints.T1IN2.Value || Marking.SpliceSensorSheild)
                                {
                                    IoPoints.T1DO0.Value = false;
                                    IoPoints.T1DO1.Value = false;
                                    stopRunningExcessiveSign = false;
                                    step = 60;
                                }
                                break;
                            default:
                                step = 0;
                                break;
                        }
                    }
                    else
                    {
                        if (IoPoints.T1IN2.Value && !Marking.ThrowerMode && stopRunningExcessiveSign)
                        {
                            IoPoints.T1DO0.Value = false;
                            IoPoints.T1DO1.Value = false;
                            stopRunningExcessiveSign = false;
                        }
                    }
                    #endregion

                    #region  抛料流程
                    if (!stationOperate.Running && Marking.ThrowerMode)
                    {
                        switch (throwerStep)
                        {
                            case 0://判断是否报警
                                if (Marking.AlarmStopRun)
                                {
                                    throwerStep = 5;
                                }
                                break;
                            case 5:
                                if (homeWaitCylinderSign)
                                {
                                    homeWaitCylinderSign = false;
                                    watchSplice.Restart();
                                    throwerStep = 50;
                                }
                                else
                                {
                                    throwerStep = 10;
                                }
                                break;
                            case 10://判断是否来料
                                if ((((IoPoints.T1IN3.Value && !Config.Instance.SignalReverseSplice) || (!IoPoints.T1IN3.Value && Config.Instance.SignalReverseSplice))
                                    || Marking.SpliceSensorSheild) && !Marking.NoRodFinish && IoPoints.T1IN18.Value)
                                {
                                    materialAbnormalCount = 0;
                                    NoRodFeedCylinder.Condition.NoMoveShield = false;
                                    NoRodFeedCylinder.Set();//上升                                                       
                                    throwerStep = 20;
                                }
                                break;
                            case 20://上升完成
                                if (NoRodFeedCylinder.OutMoveStatus && (((!IoPoints.T1IN3.Value && !Config.Instance.SignalReverseSplice)
                                    || (IoPoints.T1IN3.Value && Config.Instance.SignalReverseSplice)) || Marking.SpliceSensorSheild) && IoPoints.T1IN18.Value)
                                {
                                    NoRodFeedCylinder.Reset();
                                    throwerStep = 30;
                                }
                                break;
                            case 30: //下降完成
                                if (NoRodFeedCylinder.OutOriginStatus)
                                {
                                    Marking.NoRodFinish = true;
                                    throwerStep = 40;
                                }
                                break;
                            case 40: //等待缓冲取料
                                if (!Marking.NoRodFinish)
                                {
                                    NoRodFeedCylinder.Condition.NoMoveShield = true;
                                    watchSplice.Restart();
                                    throwerStep = 50;
                                }
                                break;
                            case 50:
                                IoPoints.T1DO0.Value = false;
                                IoPoints.T1DO1.Value = true;
                                stopRunningExcessiveSign = true;
                                if (watchSplice.ElapsedMilliseconds >= Delay.Instance.SpliceDelay || IoPoints.T1IN2.Value || Marking.SpliceSensorSheild)
                                {
                                    IoPoints.T1DO0.Value = false;
                                    IoPoints.T1DO1.Value = false;
                                    stopRunningExcessiveSign = false;
                                    throwerStep = 60;
                                }
                                break;
                            default:
                                throwerStep = 0;
                                break;
                        }
                    }
                    else
                    {
                        if (IoPoints.T1IN2.Value && Marking.ThrowerMode && stopRunningExcessiveSign)
                        {
                            IoPoints.T1DO0.Value = false;
                            IoPoints.T1DO1.Value = false;
                            stopRunningExcessiveSign = false;
                        }
                    }
                    #endregion

                    #region 无料超时报警和未接料报警
                    if (stationOperate.Running || Marking.ThrowerMode)
                    {
                        if (NoRodFeedCylinder.OutMoveStatus || Marking.SpliceSensorSheild)
                        {
                            noMaterialAlarm.Restart();
                        }
                        else if (noMaterialAlarm.ElapsedMilliseconds >= Delay.Instance.equiprmentWaitTime)
                        {
                            Marking.AlarmStopRun = false;
                            m_Alarm = SpliceAlarm.无料超时报警;
                        }

                        if (((IoPoints.T1IN3.Value && !Config.Instance.SignalReverseSplice) || (!IoPoints.T1IN3.Value && Config.Instance.SignalReverseSplice))
                            && Marking.NoRodFinish && !spliceSensorLevel)
                        {
                            spliceSensorLevel = true;
                            materialAbnormalCount++;
                            if (materialAbnormalCount >= 3)
                            {
                                Marking.AlarmStopRun = false;
                                m_Alarm = SpliceAlarm.接料异常报警;
                            }
                        }
                        else if ((IoPoints.T1IN3.Value && Config.Instance.SignalReverseSplice) || (!IoPoints.T1IN3.Value && !Config.Instance.SignalReverseSplice))
                        {
                            spliceSensorLevel = false;
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
                                throwerStep = 0;
                                materialAbnormalCount = 0;
                                Marking.AlarmStopRun = true;
                                Marking.SystemStop = false;
                                Marking.NoRodFinish = false;
                                NoRodFeedCylinder.InitExecute(); NoRodFeedCylinder.Reset();
                                m_Alarm = SpliceAlarm.无杆接料气缸复位中;
                                stationInitialize.Flow = 10;
                                break;
                            case 10:
                                if (NoRodFeedCylinder.OutOriginStatus)
                                {
                                    m_Alarm = SpliceAlarm.无消息;
                                    homeWaitCylinderSign = true;
                                    stationInitialize.InitializeDone = true;
                                    stationInitialize.Flow = 20;
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
                                if (Marking.equipmentHomeWaitState[1])
                                {
                                    step = 0;
                                    throwerStep = 0;
                                    materialAbnormalCount = 0;
                                    Marking.AlarmStopRun = true;
                                    Marking.SystemStop = false;
                                    Marking.NoRodFinish = false;
                                    NoRodFeedCylinder.InitExecute(); NoRodFeedCylinder.Reset();
                                    homeWaitCylinderSign = true;
                                    homeWaitStep = 10;
                                }
                                break;
                            case 10:
                                if (NoRodFeedCylinder.OutOriginStatus)
                                {
                                    Marking.equipmentHomeWaitState[0] = true;
                                    homeWaitStep = 20;
                                }
                                break;
                            default:
                                break;
                        }
                    }
                    #endregion

                    #region 故障清除
                    if (externalSign.AlarmReset && !stationInitialize.Running)
                    {
                        materialAbnormalCount = 0;
                        Marking.AlarmStopRun = true;
                        noMaterialAlarm.Restart();
                        m_Alarm = SpliceAlarm.无消息;
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
                list.Add(NoRodFeedCylinder);
                return list;
            }
        }

        /// <summary>
        /// 流程报警集合
        /// </summary>
        public void AddAlarms()
        {
            try
            {
                Alarms = new List<Alarm>();
                Alarms.Add(new Alarm(() => m_Alarm == SpliceAlarm.无杆接料气缸复位中)
                {
                    AlarmLevel = AlarmLevels.None,
                    Name = SpliceAlarm.无杆接料气缸复位中.ToString()
                });
                Alarms.Add(new Alarm(() => m_Alarm == SpliceAlarm.无料超时报警)
                {
                    AlarmLevel = AlarmLevels.Error,
                    Name = SpliceAlarm.无料超时报警.ToString()
                });
                Alarms.Add(new Alarm(() => m_Alarm == SpliceAlarm.接料异常报警)
                {
                    AlarmLevel = AlarmLevels.Error,
                    Name = SpliceAlarm.接料异常报警.ToString()
                });
                Alarms.AddRange(NoRodFeedCylinder.Alarms);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
    }
}
