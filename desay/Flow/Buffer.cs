using System;
using System.Collections.Generic;
using System.Enginee;
using System.Interfaces;
using System.Threading;
namespace desay
{
    /// <summary>
    /// 缓冲模组
    /// </summary>
    public class Buffer : ThreadPart
    {
        private BufferAlarm m_Alarm;
        public List<Alarm> Alarms;
        public Buffer(External ExternalSign, StationInitialize stationIni, StationOperate stationOpe)
        {
            externalSign = ExternalSign;
            stationInitialize = stationIni;
            stationOperate = stationOpe;
        }
        public StationInitialize stationInitialize { get; set; }
        public StationOperate stationOperate { get; set; }
        public External externalSign { get; set; }
        /// <summary>
        /// 缓冲升降气缸
        /// </summary>
        public DoubleCylinder DownCylinder { get; set; }
        /// <summary>
        /// 缓冲左右气缸
        /// </summary>
        public SingleCylinder LeftCylinder { get; set; }
        /// <summary>
        /// 缓冲夹子气缸
        /// </summary>
        public SingleCylinder GripperCylinder { get; set; }

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

                while (true)
                {
                    Thread.Sleep(10);

                    #region  自动流程
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
                            case 10://接料处来料，左右气缸动作
                                if (Marking.NoRodFinish && DownCylinder.OutOriginStatus)
                                {
                                    LeftCylinder.Set();
                                    step = 20;
                                }
                                break;
                            case 20://上下气缸动作
                                if (LeftCylinder.OutMoveStatus && DownCylinder.Condition.IsOnCondition)
                                {
                                    DownCylinder.Set();
                                    step = 30;
                                }
                                break;
                            case 30://夹子气缸动作
                                if (DownCylinder.OutMoveStatus && GripperCylinder.Condition.IsOnCondition)
                                {
                                    GripperCylinder.Set();
                                    step = 40;
                                }
                                break;
                            case 40://夹料完成上升
                                if (GripperCylinder.OutMoveStatus && DownCylinder.Condition.IsOffCondition)
                                {
                                    DownCylinder.Reset();
                                    step = 50;
                                }
                                break;
                            case 50://上升完成前移动
                                if (DownCylinder.OutOriginStatus && LeftCylinder.Condition.IsOffCondition)
                                {
                                    LeftCylinder.Reset();
                                    step = 60;
                                }
                                break;
                            case 60://接料完成信号取消
                                if (LeftCylinder.OutOriginStatus)
                                {
                                    Marking.NoRodFinish = false;
                                    step = 70;
                                }
                                break;
                            case 70://进料准备好，下降
                                if (!Marking.BufferFinish && DownCylinder.Condition.IsOnCondition)
                                {
                                    DownCylinder.Set();
                                    step = 80;
                                }
                                break;
                            case 80://下降完松夹
                                if (DownCylinder.OutMoveStatus && GripperCylinder.Condition.IsOffCondition)
                                {
                                    GripperCylinder.Reset();
                                    step = 90;
                                }
                                break;
                            case 90://松夹完上升
                                if (GripperCylinder.OutOriginStatus && DownCylinder.Condition.IsOffCondition)
                                {
                                    DownCylinder.Reset();
                                    step = 100;
                                }
                                break;
                            case 100://松夹子完成，缓存完成动作
                                if (DownCylinder.OutOriginStatus)
                                {
                                    Marking.BufferFinish = true;
                                    step = 110;
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
                            case 10://接料处来料，左右气缸动作
                                if (Marking.NoRodFinish && DownCylinder.OutOriginStatus)
                                {
                                    LeftCylinder.Set();
                                    throwerStep = 20;
                                }
                                break;
                            case 20://上下气缸动作
                                if (LeftCylinder.OutMoveStatus && DownCylinder.Condition.IsOnCondition)
                                {
                                    DownCylinder.Set();
                                    throwerStep = 30;
                                }
                                break;
                            case 30://夹子气缸动作
                                if (DownCylinder.OutMoveStatus && GripperCylinder.Condition.IsOnCondition)
                                {
                                    GripperCylinder.Set();
                                    throwerStep = 40;
                                }
                                break;
                            case 40://夹料完成上升
                                if (GripperCylinder.OutMoveStatus && DownCylinder.Condition.IsOffCondition)
                                {
                                    DownCylinder.Reset();
                                    throwerStep = 50;
                                }
                                break;
                            case 50://上升完成前移动
                                if (DownCylinder.OutOriginStatus && LeftCylinder.Condition.IsOffCondition)
                                {
                                    LeftCylinder.Reset();
                                    throwerStep = 60;
                                }
                                break;
                            case 60://接料完成信号取消
                                if (LeftCylinder.OutOriginStatus)
                                {
                                    Marking.NoRodFinish = false;
                                    throwerStep = 70;
                                }
                                break;
                            case 70://进料准备好，下降
                                if (!Marking.BufferFinish && DownCylinder.Condition.IsOnCondition)
                                {
                                    DownCylinder.Set();
                                    throwerStep = 80;
                                }
                                break;
                            case 80://下降完松夹
                                if (DownCylinder.OutMoveStatus && GripperCylinder.Condition.IsOffCondition)
                                {
                                    GripperCylinder.Reset();
                                    throwerStep = 90;
                                }
                                break;
                            case 90://松夹完上升
                                if (GripperCylinder.OutOriginStatus && DownCylinder.Condition.IsOffCondition)
                                {
                                    DownCylinder.Reset();
                                    throwerStep = 100;
                                }
                                break;
                            case 100://松夹子完成，缓存完成动作
                                if (DownCylinder.OutOriginStatus)
                                {
                                    Marking.BufferFinish = true;
                                    throwerStep = 110;
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
                                throwerStep = 0;
                                Marking.BufferFinish = false;
                                stationInitialize.Flow = 10;
                                break;
                            case 10:
                                if (DownCylinder.Condition.IsOffCondition)
                                {
                                    DownCylinder.InitExecute(); DownCylinder.Reset();
                                    m_Alarm = BufferAlarm.缓冲升降气缸复位中;
                                    stationInitialize.Flow = 20;
                                }
                                break;
                            case 20:
                                if (DownCylinder.OutOriginStatus)
                                {
                                    GripperCylinder.InitExecute(); GripperCylinder.Reset();
                                    m_Alarm = BufferAlarm.缓冲夹子气缸复位中;
                                    stationInitialize.Flow = 30;
                                }
                                break;
                            case 30:
                                if (GripperCylinder.OutOriginStatus)
                                {
                                    LeftCylinder.InitExecute(); LeftCylinder.Reset();
                                    m_Alarm = BufferAlarm.缓冲左右气缸复位中;
                                    stationInitialize.Flow = 40;
                                }
                                break;
                            case 40:
                                if (LeftCylinder.OutOriginStatus)
                                {
                                    m_Alarm = BufferAlarm.无消息;
                                    stationInitialize.InitializeDone = true;
                                    stationInitialize.Flow = 50;
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
                                throwerStep = 0;
                                Marking.BufferFinish = false;
                                DownCylinder.InitExecute(); DownCylinder.Reset();
                                homeWaitStep = 10;
                                break;
                            case 10:
                                if (DownCylinder.OutOriginStatus)
                                {
                                    GripperCylinder.InitExecute(); GripperCylinder.Reset();
                                    homeWaitStep = 20;
                                }
                                break;
                            case 20:
                                if (GripperCylinder.OutOriginStatus)
                                {
                                    LeftCylinder.InitExecute(); LeftCylinder.Reset();
                                    Marking.equipmentHomeWaitState[1] = true;
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
                        m_Alarm = BufferAlarm.无消息;
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
                list.Add(LeftCylinder);
                list.Add(DownCylinder);
                list.Add(GripperCylinder);
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
                Alarms.Add(new Alarm(() => m_Alarm == BufferAlarm.缓冲升降气缸复位中)
                {
                    AlarmLevel = AlarmLevels.None,
                    Name = BufferAlarm.缓冲升降气缸复位中.ToString()
                });
                Alarms.Add(new Alarm(() => m_Alarm == BufferAlarm.缓冲左右气缸复位中)
                {
                    AlarmLevel = AlarmLevels.None,
                    Name = BufferAlarm.缓冲左右气缸复位中.ToString()
                });
                Alarms.Add(new Alarm(() => m_Alarm == BufferAlarm.缓冲夹子气缸复位中)
                {
                    AlarmLevel = AlarmLevels.None,
                    Name = BufferAlarm.缓冲夹子气缸复位中.ToString()
                });
                Alarms.AddRange(LeftCylinder.Alarms);
                Alarms.AddRange(DownCylinder.Alarms);
                Alarms.AddRange(GripperCylinder.Alarms);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
    }
}
