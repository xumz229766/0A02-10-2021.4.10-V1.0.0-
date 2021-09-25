using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Enginee;
using System.Interfaces;
using System.Threading;

namespace desay
{
    /// <summary>
    /// 仓储模块
    /// </summary>
    public class Storing : ThreadPart
    {
        private StoreAlarm m_Alarm;
        public List<Alarm> Alarms;
        public Storing(External ExternalSign, StationInitialize stationIni, StationOperate stationOpe)
        {
            externalSign = ExternalSign;
            stationInitialize = stationIni;
            stationOperate = stationOpe;
        }
        public StationInitialize stationInitialize { get; set; }
        public StationOperate stationOperate { get; set; }
        public External externalSign { get; set; }
        /// <summary>
        /// 仓储升降轴
        /// </summary>
        public StepAxis MAxis { get; set; }
        /// <summary>
        /// 安全门升降
        /// </summary>
        public DoubleCylinder SafeDoolCylinder { get; set; }


        public override void Running(RunningModes runningMode)
        {
            try
            {
                bool RawMStandbyPositionOpen = Position.Instance.MStandbyPositionOpen; //记录旧状态判断是否修改
                var step = 0;
                double pos = 0;

                var strtime1 = new Stopwatch();
                var strtime2 = new Stopwatch();
                strtime1.Start();
                strtime2.Start();

                while (true)
                {
                    Thread.Sleep(10);

                    #region  自动流程
                    if (stationOperate.Running && !Marking.ChangeTrayPadlock)
                    {
                        switch (step)
                        {
                            case 0:
                                if (0 == Marking.IsMMoveChangeTrayPos) break;
                                if (Position.Instance.MStandbyPositionOpen && (1 == Marking.IsMMoveChangeTrayPos || 3 == Marking.IsMMoveChangeTrayPos))
                                {
                                    step = 10;
                                }
                                else
                                {
                                    step = 40;
                                }
                                break;
                            case 10://判断是否安全。
                                if (!Marking.GettingPlate && !Marking.PuttingPlate)
                                {
                                    if (IoPoints.T2IN18.Value && IoPoints.T2IN19.Value)
                                    {
                                        Marking.MIsNoMoving = true;
                                        MAxis.MoveTo(Position.Instance.MStandbyPosition, AxisParameter.Instance.MVelocityCurve);
                                        step = 20;
                                    }
                                    else
                                    {
                                        m_Alarm = StoreAlarm.设备气压低故障;
                                    }
                                }
                                break;
                            case 20:
                                if (MAxis.IsInPosition(Position.Instance.MStandbyPosition))
                                {
                                    Marking.MIsNoMoving = false;
                                    Marking.StoreFinish = false;
                                    step = 30;
                                }
                                break;
                            case 30:
                                if (2 == Marking.IsMMoveChangeTrayPos || 4 == Marking.IsMMoveChangeTrayPos)
                                {
                                    if (RawMStandbyPositionOpen != Position.Instance.MStandbyPositionOpen)
                                    {
                                        RawMStandbyPositionOpen = Position.Instance.MStandbyPositionOpen;
                                    }
                                    step = 0;
                                }
                                break;
                            case 40:
                                if (!Marking.GettingPlate && !Marking.PuttingPlate)
                                {
                                    if (IoPoints.T2IN18.Value && IoPoints.T2IN19.Value)
                                    {
                                        Marking.MIsNoMoving = true;
                                        if (Config.Instance.SelectCheckRunState)
                                        {
                                            pos = Position.Instance.MStartPosition;
                                            MAxis.MoveTo(pos, AxisParameter.Instance.MVelocityCurve);
                                        }
                                        else
                                        {
                                            pos = (Position.Instance.MLayerIndex - 1) * Position.Instance.MDistance + Position.Instance.MStartPosition;
                                            MAxis.MoveTo(pos, AxisParameter.Instance.MVelocityCurve);
                                        }
                                        step = 50;
                                    }
                                    else
                                    {
                                        m_Alarm = StoreAlarm.设备气压低故障;
                                    }
                                }
                                break;
                            case 50:
                                if (MAxis.IsInPosition(pos))
                                {
                                    Marking.MIsNoMoving = false;
                                    Marking.StoreFinish = true;
                                    step = 60;
                                }
                                break;
                            case 60:
                                if (RawMStandbyPositionOpen != Position.Instance.MStandbyPositionOpen)
                                {
                                    RawMStandbyPositionOpen = Position.Instance.MStandbyPositionOpen;
                                    Marking.StoreFinish = false;
                                    step = 0;
                                }
                                else if (5 == Marking.IsMMoveChangeTrayPos)
                                {
                                    step = 80;
                                }
                                else if (Position.Instance.MStandbyPositionOpen && 2 == Marking.IsMMoveChangeTrayPos)
                                {
                                    step = 70;
                                }
                                break;
                            case 70://先勾盘2才能到结束3
                                if (Position.Instance.MStandbyPositionOpen && 3 == Marking.IsMMoveChangeTrayPos)
                                {
                                    Marking.StoreFinish = false;
                                    step = 0;
                                }
                                break;
                            case 80:
                                if (!Marking.StoreFinish)
                                {
                                    Thread.Sleep(150);
                                    if (Config.Instance.SelectCheckRunState)
                                    {
                                        Marking.MSelectIndex = true;
                                    }
                                    else if (Marking.HookLayerPlate)
                                    {
                                        Marking.MSelectIndex = false;
                                        Marking.HookLayerPlate = false;
                                    }
                                    else
                                    {
                                        Marking.MSelectIndex = false;
                                        Position.Instance.MLayerIndex++;
                                    }
                                    step = 0;
                                }
                                break;
                        }
                    }

                    #endregion

                    #region  气压检测
                    if (Position.Instance.MLayerIndex > Position.Instance.MLayerCount)
                    {
                        if (!Config.Instance.ChangeTrayArl)
                        {
                            m_Alarm = StoreAlarm.仓储已满盘;
                        }
                        Position.Instance.MLayerIndex = 2;
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
                                Marking.StoreFinish = false;
                                Marking.MIsNoMoving = false;
                                SafeDoolCylinder.InitExecute();
                                IoPoints.T2DO3.Value = false;
                                IoPoints.T2DO4.Value = false;
                                m_Alarm = StoreAlarm.仓储轴复位中;
                                MAxis.IsServon = true;
                                Thread.Sleep(500);
                                MAxis.Stop();
                                stationInitialize.Flow = 5;
                                break;
                            case 5:  //若M轴在原点位异常，启动Z轴寸动循环
                                if (IoPoints.T2IN18.Value && IoPoints.T2IN19.Value && MAxis.IsDone)
                                {
                                    MAxis.BackHome();
                                    stationInitialize.Flow = 10;
                                }
                                else
                                {
                                    m_Alarm = StoreAlarm.左右卡盘感应到信号;
                                }
                                break;
                            case 10:
                                if (MAxis.CheckHomeDone(200.0) == 0)
                                {
                                    if (Position.Instance.MStandbyPositionOpen)
                                    {
                                        MAxis.MoveTo(Position.Instance.MStandbyPosition, AxisParameter.Instance.MVelocityCurve);
                                        stationInitialize.Flow = 20;
                                    }
                                    else
                                    {
                                        if (Config.Instance.SelectCheckRunState)
                                        {
                                            pos = Position.Instance.MStartPosition;
                                            MAxis.MoveTo(pos, AxisParameter.Instance.MVelocityCurve);
                                            Marking.MSelectIndex = true;
                                        }
                                        else
                                        {
                                            pos = (Position.Instance.MLayerIndex - 1) * Position.Instance.MDistance + Position.Instance.MStartPosition;
                                            MAxis.MoveTo(pos, AxisParameter.Instance.MVelocityCurve);
                                        }
                                        stationInitialize.Flow = 30;
                                    }
                                }
                                else
                                {
                                    m_Alarm = StoreAlarm.仓储初始化故障;
                                    stationInitialize.InitializeDone = false;
                                    stationInitialize.Flow = -1;
                                }
                                break;
                            case 20://判断M轴是否动作完成
                                Thread.Sleep(10);
                                if (MAxis.IsInPosition(Position.Instance.MStandbyPosition))
                                {
                                    m_Alarm = StoreAlarm.无消息;
                                    stationInitialize.InitializeDone = true;
                                    stationInitialize.Flow = 40;
                                }
                                break;
                            case 30://判断M轴是否动作完成
                                Thread.Sleep(10);
                                if (MAxis.IsInPosition(pos))
                                {
                                    if (Config.Instance.SelectCheckRunState) Marking.MSelectIndex = true;
                                    m_Alarm = StoreAlarm.无消息;
                                    stationInitialize.InitializeDone = true;
                                    stationInitialize.Flow = 40;
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
                        step = 0;
                        Marking.StoreFinish = false;
                        Marking.MIsNoMoving = false;
                    }
                    #endregion

                    #region 故障清除
                    if (externalSign.AlarmReset && !stationInitialize.Running)
                    {
                        m_Alarm = StoreAlarm.无消息;
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
                list.Add(SafeDoolCylinder);
                return list;
            }
        }

        public void AddAlarms()
        {
            try
            {
                Alarms = new List<Alarm>();
                Alarms.Add(new Alarm(() => m_Alarm == StoreAlarm.左右卡盘感应到信号) { AlarmLevel = AlarmLevels.Error, Name = StoreAlarm.左右卡盘感应到信号.ToString() });
                Alarms.Add(new Alarm(() => m_Alarm == StoreAlarm.设备气压低故障) { AlarmLevel = AlarmLevels.Error, Name = StoreAlarm.设备气压低故障.ToString() });
                Alarms.Add(new Alarm(() => m_Alarm == StoreAlarm.仓储初始化故障) { AlarmLevel = AlarmLevels.Error, Name = StoreAlarm.仓储初始化故障.ToString() });
                Alarms.Add(new Alarm(() => m_Alarm == StoreAlarm.仓储已满盘) { AlarmLevel = AlarmLevels.Error, Name = StoreAlarm.仓储已满盘.ToString() });
                Alarms.Add(new Alarm(() => m_Alarm == StoreAlarm.仓储轴复位中) { AlarmLevel = AlarmLevels.None, Name = StoreAlarm.仓储轴复位中.ToString() });
                Alarms.AddRange(MAxis.Alarms);
                Alarms.AddRange(SafeDoolCylinder.Alarms);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
    }
}
