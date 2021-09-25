using System;
using System.Collections.Generic;
using System.Enginee;
using System.Interfaces;
using System.Threading;
using System.Diagnostics;
namespace desay
{
    /// <summary>
    /// 进料模块          
    /// </summary>
    public class Feeder : ThreadPart
    {
        private FeederAlarm m_Alarm;
        public List<Alarm> Alarms;
        public Feeder(External ExternalSign, StationInitialize stationIni, StationOperate stationOpe)
        {
            externalSign = ExternalSign;
            stationInitialize = stationIni;
            stationOperate = stationOpe;
        }
        public StationInitialize stationInitialize { get; set; }
        public StationOperate stationOperate { get; set; }
        public External externalSign { get; set; }
        /// <summary>
        /// 进料气缸
        /// </summary>
        public SingleCylinder FeederCylinder { get; set; }

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
                Stopwatch IntWatch = new Stopwatch();
                IntWatch.Start();

                while (true)
                {
                    Thread.Sleep(10);

                    #region 自动流程
                    if (stationOperate.Running)
                    {
                        switch (step)
                        {
                            case 0://判断是否报警
                                if (Marking.AlarmStopRun)
                                {
                                    step = 10;
                                }
                                break;
                            case 10: //移料左右气缸在原点
                                if (!Marking.FeederFinish && IoPoints.T1IN10.Value && IoPoints.T1IN12.Value)
                                {
                                    FeederCylinder.Reset();
                                    step = 20;
                                }
                                break;
                            case 20:
                                if (FeederCylinder.OutOriginStatus)
                                {
                                    Marking.BufferFinish = false;
                                    step = 30;
                                }
                                break;
                            case 30:
                                if (Marking.BufferFinish && IoPoints.T1IN10.Value && IoPoints.T1IN12.Value)
                                {
                                    FeederCylinder.Set();
                                    step = 40;
                                }
                                break;
                            case 40:
                                if (FeederCylinder.OutMoveStatus)
                                {
                                    Marking.FeederFinish = true;
                                    step = 50;
                                }
                                break;
                            default:
                                step = 0;
                                break;
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
                                    throwerStep = 10;
                                }
                                break;
                            case 10: //移料左右气缸在原点
                                if (!Marking.FeederFinish && IoPoints.T1IN10.Value && IoPoints.T1IN12.Value)
                                {
                                    FeederCylinder.Reset();
                                    throwerStep = 20;
                                }
                                break;
                            case 20:
                                if (FeederCylinder.OutOriginStatus)
                                {
                                    Marking.BufferFinish = false;
                                    throwerStep = 30;
                                }
                                break;
                            case 30:
                                if (Marking.BufferFinish && IoPoints.T1IN10.Value && IoPoints.T1IN12.Value)
                                {
                                    FeederCylinder.Set();
                                    throwerStep = 40;
                                }
                                break;
                            case 40:
                                if (FeederCylinder.OutMoveStatus)
                                {
                                    Marking.FeederFinish = true;
                                    throwerStep = 50;
                                }
                                break;
                            default:
                                throwerStep = 0;
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
                                Marking.FeederFinish = false;
                                FeederCylinder.InitExecute(); FeederCylinder.Reset();
                                m_Alarm = FeederAlarm.进料气缸复位中;
                                stationInitialize.Flow = 10;
                                break;
                            case 10://复位完成，置位初始化标志
                                if (FeederCylinder.OutOriginStatus)
                                {
                                    m_Alarm = FeederAlarm.无消息;
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
                                step = 0;
                                Marking.FeederFinish = false;
                                FeederCylinder.InitExecute();
                                FeederCylinder.Reset();
                                Marking.equipmentHomeWaitState[9] = true;
                                homeWaitStep = 10;
                                break;
                            default:
                                break;
                        }
                    }
                    #endregion

                    #region 故障清除
                    if (externalSign.AlarmReset && !stationInitialize.Running)
                    {
                        m_Alarm = FeederAlarm.无消息;
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
                list.Add(FeederCylinder);
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
                Alarms.Add(new Alarm(() => m_Alarm == FeederAlarm.进料气缸复位中)
                {
                    AlarmLevel = AlarmLevels.None,
                    Name = FeederAlarm.进料气缸复位中.ToString()
                });
                Alarms.AddRange(FeederCylinder.Alarms);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
    }
}
