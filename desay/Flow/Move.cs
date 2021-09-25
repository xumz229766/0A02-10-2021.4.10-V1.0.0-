using System;
using System.Collections.Generic;
using System.Enginee;
using System.Interfaces;
using System.Threading;
using System.Diagnostics;

namespace desay
{
    /// <summary>
    /// 移动模组
    /// </summary>
    public class Move : ThreadPart
    {
        public List<Alarm> Alarms;
        private MoveAlarm m_Alarm;
        public Move(External ExternalSign, StationInitialize stationIni, StationOperate stationOpe)
        {
            externalSign = ExternalSign;
            stationInitialize = stationIni;
            stationOperate = stationOpe;
        }
        public StationInitialize stationInitialize { get; set; }
        public StationOperate stationOperate { get; set; }
        public External externalSign { get; set; }
        /// <summary>
        /// 左右气缸
        /// </summary>
        public SingleCylinder2 LeftCylinder { get; set; }
        /// <summary>
        /// 上下气缸
        /// </summary>
        public SingleCylinder DownCylinder { get; set; }
        /// <summary>
        /// 夹子气缸
        /// </summary>
        public SingleCylinder GripperCylinder { get; set; }
        /// <summary>
        /// 碎料气缸
        /// </summary>
        public SingleCylinder CutwasteCylinder1 { get; set; }
        /// <summary>
        /// 排料气缸
        /// </summary>      
        public SingleCylinder CutwasteCylinder2 { get; set; }
        /// <summary>
        /// 碎料盖子气缸
        /// </summary>      
        public SingleCylinder CutwasteCylinder3 { get; set; }
        /// <summary>
        /// 推进轴1
        /// </summary>
        public ServoAxis Push1Axis { get; set; }
        /// <summary>
        /// 推进轴2
        /// </summary>
        public ServoAxis Push2Axis { get; set; }
        /// <summary>
        /// 推进轴3
        /// </summary>
        public ServoAxis Push3Axis { get; set; }
        /// <summary>
        /// 推进轴3
        /// </summary>
        public ServoAxis Push4Axis { get; set; }
        /// <summary>
        /// 碎料次数
        /// </summary>     
        private int ShearWasteNumber;
        /// <summary>
        /// 回待机位
        /// </summary> 
        public int homeWaitStep = 0;

        public override void Running(RunningModes runningMode)
        {
            try
            {
                var cutWasteStep = 0;
                var throwerStep = 0;
                Marking.watchModel.Restart();
                bool Cylinder3Open = false; //碎料盖子打开
                bool CutwasteHaveProduct = false;//用于判断碎料是否完成
                Stopwatch watchCutaste = new Stopwatch(); //碎料掉落延时
                watchCutaste.Start();

                while (true)
                {
                    Thread.Sleep(10);

                    #region  自动流程
                    if (stationOperate.Running)
                    {
                        switch (stationOperate.step)
                        {
                            case 0://判断是否报警
                                if (Marking.AlarmStopRun && (IoPoints.T3IN10.Value || Marking.MoveProductSensorSheild) && Marking.FeederFinish)
                                {
                                    stationOperate.step = 10;
                                }
                                break;
                            case 10://判断缓冲完成，左右C轴是否有料，移料左右气缸为ON
                                if (LeftCylinder.OutOriginStatus && DownCylinder.OutOriginStatus && GripperCylinder.OutOriginStatus && LeftCylinder.Condition.IsOnCondition)
                                {
                                    Cylinder3Open = true;
                                    LeftCylinder.Set();
                                    stationOperate.step = 20;
                                }
                                break;
                            case 20://移料左右到位，/移料上下为OFF
                                if (LeftCylinder.OutMoveStatus && DownCylinder.Condition.IsOffCondition)
                                {
                                    DownCylinder.Set();
                                    stationOperate.step = 30;
                                }
                                break;
                            case 30://移料上下到位，夹子气缸为ON
                                if (DownCylinder.OutMoveStatus && GripperCylinder.Condition.IsOnCondition && Marking.MoveFinish)
                                {
                                    if (1 == Position.Instance.FragmentationMode && Position.Instance.CheckMotorStart)
                                    {
                                        IoPoints.T2DO10.Value = false; //关闭碎料电机
                                    }
                                    stationOperate.step = 40;
                                }
                                break;
                            case 40:
                                if (Marking.ModelCount >= Position.Instance.ModelCountSet && Marking.ThrowerModeFrist) //开机排料  
                                {
                                    Marking.ThrowerModeFrist = false;
                                    GripperCylinder.Set();
                                    stationOperate.step = 50;
                                }
                                else if (!Marking.ThrowerModeFrist && Marking.ModelCount >= Position.Instance.ChangeTrayLayout && Marking.changeTrayLayoutSign)//换盘排料
                                {
                                    Marking.changeTrayLayoutSign = false;
                                    Marking.changeTrayLayoutWaitSign = false;
                                    GripperCylinder.Set();
                                    stationOperate.step = 50;
                                }
                                else if (Marking.ThrowerModeFrist || !Marking.changeTrayLayoutWaitSign || Marking.changeTrayLayoutSign)
                                {
                                    GripperCylinder.Set();
                                    stationOperate.step = 50;
                                }
                                break;
                            case 50://夹子气缸到位，移料上下为OFF
                                if (GripperCylinder.OutMoveStatus)
                                {
                                    DownCylinder.Reset();
                                    stationOperate.step = 60;
                                }
                                break;
                            case 60://移料上下到位，移料左右为ON
                                if (DownCylinder.OutOriginStatus && LeftCylinder.Condition.IsOnCondition)
                                {
                                    LeftCylinder.Reset();
                                    stationOperate.step = 70;
                                }
                                break;
                            case 70://移料左右到位，移料上下为ON
                                if (LeftCylinder.OutOriginStatus && DownCylinder.Condition.IsOffCondition && !CutwasteHaveProduct)
                                {
                                    Marking.FeederFinish = false;
                                    DownCylinder.Set();
                                    stationOperate.step = 80;
                                }
                                break;
                            case 80://移料上下到位，夹子气缸为OFF
                                if (DownCylinder.OutMoveStatus && GripperCylinder.Condition.IsOffCondition)
                                {
                                    Config.Instance.Finishcount++;
                                    GripperCylinder.Reset();
                                    stationOperate.step = 90;
                                }
                                break;
                            case 90:
                                if (GripperCylinder.OutOriginStatus)
                                {
                                    if (Marking.ThrowerModeFrist || Marking.changeTrayLayoutSign) //用于第一次启动废料计数和换盘排废料计数
                                    {
                                        Marking.ModelCount++;
                                        CutwasteHaveProduct = true;
                                        DownCylinder.Reset();
                                        stationOperate.step = 100;
                                    }
                                    else
                                    {
                                        Marking.MoveFinish = false;
                                        if (Marking.CAxisFinish)
                                        {
                                            CutwasteHaveProduct = true;
                                            DownCylinder.Reset();
                                            stationOperate.step = 100;
                                        }
                                    }
                                    if (1 == Position.Instance.FragmentationMode && Position.Instance.CheckMotorStart)
                                    {
                                        IoPoints.T2DO10.Value = true; //开启碎料电机
                                    }
                                }
                                break;
                            case 100://移料上下到位
                                if (DownCylinder.OutOriginStatus)
                                {
                                    stationOperate.step = 110;
                                }
                                break;
                            default:
                                stationOperate.step = 0;
                                break;
                        }
                    }
                    #endregion}

                    #region  冲压碎料
                    if (0 == Position.Instance.FragmentationMode)
                    {
                        switch (cutWasteStep)
                        {
                            case 0://气缸在原位
                                if (CutwasteCylinder1.OutOriginStatus && CutwasteCylinder2.OutOriginStatus && CutwasteCylinder3.OutOriginStatus)
                                {
                                    cutWasteStep = 5;
                                }
                                break;
                            case 5:
                                if (Cylinder3Open)
                                {
                                    CutwasteCylinder3.Set();
                                    Cylinder3Open = false;
                                    cutWasteStep = 10;
                                }
                                break;
                            case 10://废料位置有料 
                                if (CutwasteCylinder3.OutMoveStatus && CutwasteHaveProduct)
                                {
                                    watchCutaste.Restart();
                                    cutWasteStep = 20;
                                }
                                break;
                            case 20://等待料掉落盖盖子
                                if (watchCutaste.ElapsedMilliseconds >= Delay.Instance.SpliceDelay)
                                {
                                    CutwasteCylinder3.Reset();
                                    cutWasteStep = 30;
                                }
                                break;
                            case 30:
                                if (CutwasteCylinder3.OutOriginStatus)
                                {
                                    ShearWasteNumber = 1;
                                    cutWasteStep = 40;
                                }
                                break;
                            case 40:
                                if (CutwasteCylinder1.OutOriginStatus)
                                {
                                    CutwasteCylinder1.Set();
                                    cutWasteStep = 50;
                                }
                                break;
                            case 50://按设定的剪废料次数剪
                                if (ShearWasteNumber < Delay.Instance.WasteNumber)
                                {
                                    if (CutwasteCylinder1.OutMoveStatus)
                                    {
                                        CutwasteCylinder1.Reset();
                                        ShearWasteNumber++;
                                        cutWasteStep = 40;
                                    }
                                }
                                else
                                {
                                    cutWasteStep = 60;
                                }
                                break;
                            case 60://废料气缸到位,排料气缸打开
                                if (CutwasteCylinder1.OutMoveStatus)
                                {
                                    CutwasteCylinder2.Set();
                                    cutWasteStep = 70;
                                }
                                break;
                            case 70://排料气缸打开到位,废料复位
                                if (CutwasteCylinder2.OutMoveStatus)
                                {
                                    CutwasteCylinder1.Reset();
                                    cutWasteStep = 80;
                                }
                                break;
                            case 80://废料到位,排料复位
                                if (CutwasteCylinder1.OutOriginStatus)
                                {
                                    CutwasteCylinder2.Reset();
                                    cutWasteStep = 90;
                                }
                                break;
                            case 90://排料到位,废料完成
                                if (CutwasteCylinder2.OutOriginStatus)
                                {
                                    CutwasteHaveProduct = false;
                                    cutWasteStep = 100;
                                }
                                break;
                            default:
                                if (!CutwasteHaveProduct)
                                {
                                    cutWasteStep = 0;
                                }
                                break;
                        }
                    }
                    else
                    {
                        CutwasteHaveProduct = false;
                        Cylinder3Open = false;
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
                            case 5://判断C轴是否在原点   
                                if (Push1Axis.IsInPosition(Position.Instance.PosPush[0].Origin) && Push2Axis.IsInPosition(Position.Instance.PosPush[1].Origin) &&
                                    Push3Axis.IsInPosition(Position.Instance.PosPush[2].Origin) && Push4Axis.IsInPosition(Position.Instance.PosPush[3].Origin))
                                {
                                    throwerStep = 10;
                                }
                                else
                                {
                                    m_Alarm = MoveAlarm.C轴推进气缸不在原点;
                                }
                                break;
                            case 10://判断缓冲完成，左右C轴是否有料，移料左右气缸为ON
                                if ((IoPoints.T3IN10.Value || Marking.MoveProductSensorSheild) && Marking.FeederFinish && LeftCylinder.OutOriginStatus 
                                    && DownCylinder.OutOriginStatus && GripperCylinder.OutOriginStatus && LeftCylinder.Condition.IsOnCondition)
                                {
                                    Cylinder3Open = true;
                                    LeftCylinder.Set();
                                    throwerStep = 30;
                                }
                                break;
                            case 30://移料左右到位，/移料上下为OFF
                                if (LeftCylinder.OutMoveStatus && DownCylinder.Condition.IsOffCondition)
                                {
                                    DownCylinder.Set();
                                    throwerStep = 40;
                                }
                                break;
                            case 40://移料上下到位，夹子气缸为ON
                                if (DownCylinder.OutMoveStatus && GripperCylinder.Condition.IsOnCondition)
                                {
                                    if (1 == Position.Instance.FragmentationMode && Position.Instance.CheckMotorStart)
                                    {
                                        IoPoints.T2DO10.Value = false; //关闭碎料电机
                                    }
                                    GripperCylinder.Set();
                                    throwerStep = 50;
                                }
                                break;
                            case 50://夹子气缸到位，移料上下为OFF
                                if (GripperCylinder.OutMoveStatus)
                                {
                                    DownCylinder.Reset();
                                    throwerStep = 60;
                                }
                                break;
                            case 60://移料上下到位，移料左右为ON
                                if (DownCylinder.OutOriginStatus && LeftCylinder.Condition.IsOnCondition)
                                {
                                    LeftCylinder.Reset();
                                    throwerStep = 70;
                                }
                                break;
                            case 70://移料左右到位，移料上下为ON
                                if (LeftCylinder.OutOriginStatus && DownCylinder.Condition.IsOffCondition)
                                {
                                    Marking.FeederFinish = false;
                                    DownCylinder.Set();
                                    throwerStep = 80;
                                }
                                break;
                            case 80://移料上下到位，夹子气缸为OFF
                                if (DownCylinder.OutMoveStatus && GripperCylinder.Condition.IsOffCondition)
                                {
                                    Config.Instance.Finishcount++;
                                    GripperCylinder.Reset();
                                    throwerStep = 90;
                                }
                                break;
                            case 90:
                                if (GripperCylinder.OutOriginStatus)
                                {
                                    if (1 == Position.Instance.FragmentationMode && Position.Instance.CheckMotorStart)
                                    {
                                        IoPoints.T2DO10.Value = true; //开启碎料电机
                                    }
                                    DownCylinder.Reset();
                                    throwerStep = 100;
                                }
                                break;
                            case 100://移料上下到位，夹子气缸为OFF
                                if (DownCylinder.OutOriginStatus && LeftCylinder.OutOriginStatus && GripperCylinder.OutOriginStatus)
                                {
                                    CutwasteHaveProduct = true;
                                    throwerStep = 110;
                                }
                                break;
                            default:
                                if (!CutwasteHaveProduct)
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
                                stationOperate.step = 0;
                                cutWasteStep = 0;
                                throwerStep = 0;
                                Marking.ModelCount = 0;
                                Marking.ThrowerModeFrist = true;
                                Marking.changeTrayLayoutSign = false;
                                Marking.changeTrayLayoutWaitSign = false;
                                Cylinder3Open = false;
                                CutwasteHaveProduct = false;
                                Marking.MoveFinish = false;
                                stationInitialize.Flow = 10;
                                break;
                            case 10:
                                GripperCylinder.InitExecute(); GripperCylinder.Reset();
                                m_Alarm = MoveAlarm.移料夹子气缸复位中;
                                stationInitialize.Flow = 20;
                                break;
                            case 20:
                                if (GripperCylinder.OutOriginStatus)
                                {
                                    DownCylinder.InitExecute(); DownCylinder.Reset();
                                    m_Alarm = MoveAlarm.移料上下气缸复位中;
                                    stationInitialize.Flow = 30;
                                }
                                break;
                            case 30:
                                if (DownCylinder.OutOriginStatus)
                                {
                                    LeftCylinder.InitExecute(); LeftCylinder.Reset();
                                    m_Alarm = MoveAlarm.移料左右气缸复位中;
                                    stationInitialize.Flow = 40;
                                }
                                break;
                            case 40:
                                if (LeftCylinder.OutOriginStatus && 0 == Position.Instance.FragmentationMode)
                                {
                                    CutwasteCylinder1.InitExecute(); CutwasteCylinder1.Reset();
                                    m_Alarm = MoveAlarm.碎料气缸复位中;
                                    stationInitialize.Flow = 50;
                                }
                                else
                                {
                                    m_Alarm = MoveAlarm.无消息;
                                    stationInitialize.InitializeDone = true;
                                    stationInitialize.Flow = 80;
                                }
                                break;
                            case 50:
                                if (CutwasteCylinder1.OutOriginStatus)
                                {
                                    CutwasteCylinder2.InitExecute(); CutwasteCylinder2.Reset();
                                    m_Alarm = MoveAlarm.排料气缸复位中;
                                    stationInitialize.Flow = 60;
                                }
                                break;
                            case 60:
                                if (CutwasteCylinder2.OutOriginStatus)
                                {
                                    CutwasteCylinder3.InitExecute(); CutwasteCylinder3.Reset();
                                    m_Alarm = MoveAlarm.碎料盖子气缸复位中;
                                    stationInitialize.Flow = 70;
                                }
                                break;
                            case 70:
                                if (CutwasteCylinder3.OutOriginStatus)
                                {
                                    m_Alarm = MoveAlarm.无消息;
                                    stationInitialize.InitializeDone = true;
                                    stationInitialize.Flow = 80;
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
                                cutWasteStep = 0;
                                throwerStep = 0;
                                Marking.ModelCount = 0;
                                Marking.changeTrayLayoutSign = false;
                                Marking.changeTrayLayoutWaitSign = false;
                                Cylinder3Open = false;
                                CutwasteHaveProduct = false;
                                Marking.MoveFinish = false;
                                GripperCylinder.InitExecute(); GripperCylinder.Reset();
                                homeWaitStep = 10;
                                break;
                            case 10:
                                if (GripperCylinder.OutOriginStatus)
                                {
                                    DownCylinder.InitExecute(); DownCylinder.Reset();
                                    homeWaitStep = 20;
                                }
                                break;
                            case 20:
                                if (DownCylinder.OutOriginStatus)
                                {
                                    LeftCylinder.InitExecute(); LeftCylinder.Reset();
                                    if (0 == Position.Instance.FragmentationMode)
                                    {
                                        CutwasteCylinder1.InitExecute(); CutwasteCylinder1.Reset();
                                        CutwasteCylinder2.InitExecute(); CutwasteCylinder2.Reset();
                                        CutwasteCylinder3.InitExecute(); CutwasteCylinder3.Reset();
                                    }
                                    Marking.equipmentHomeWaitState[2] = true;
                                    homeWaitStep = 30;
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
                        m_Alarm = MoveAlarm.无消息;
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
                if (0 == Position.Instance.FragmentationMode)
                {
                    list.Add(CutwasteCylinder1);
                    list.Add(CutwasteCylinder2);
                    list.Add(CutwasteCylinder3);
                }
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
                Alarms.Add(new Alarm(() => m_Alarm == MoveAlarm.移料上下气缸复位中) { AlarmLevel = AlarmLevels.None, Name = MoveAlarm.移料上下气缸复位中.ToString() });
                Alarms.Add(new Alarm(() => m_Alarm == MoveAlarm.移料左右气缸复位中) { AlarmLevel = AlarmLevels.None, Name = MoveAlarm.移料左右气缸复位中.ToString() });
                Alarms.Add(new Alarm(() => m_Alarm == MoveAlarm.移料夹子气缸复位中) { AlarmLevel = AlarmLevels.None, Name = MoveAlarm.移料夹子气缸复位中.ToString() });
                if (0 == Position.Instance.FragmentationMode)
                {
                    Alarms.Add(new Alarm(() => m_Alarm == MoveAlarm.碎料气缸复位中) { AlarmLevel = AlarmLevels.None, Name = MoveAlarm.碎料气缸复位中.ToString() });
                    Alarms.Add(new Alarm(() => m_Alarm == MoveAlarm.排料气缸复位中) { AlarmLevel = AlarmLevels.None, Name = MoveAlarm.排料气缸复位中.ToString() });
                    Alarms.Add(new Alarm(() => m_Alarm == MoveAlarm.碎料盖子气缸复位中) { AlarmLevel = AlarmLevels.None, Name = MoveAlarm.碎料盖子气缸复位中.ToString() });
                }
                Alarms.Add(new Alarm(() => m_Alarm == MoveAlarm.C轴推进气缸不在原点) { AlarmLevel = AlarmLevels.Error, Name = MoveAlarm.C轴推进气缸不在原点.ToString() });
                Alarms.AddRange(LeftCylinder.Alarms);
                Alarms.AddRange(DownCylinder.Alarms);
                Alarms.AddRange(GripperCylinder.Alarms);
                if (0 == Position.Instance.FragmentationMode)
                {
                    Alarms.AddRange(CutwasteCylinder1.Alarms);
                    Alarms.AddRange(CutwasteCylinder2.Alarms);
                    Alarms.AddRange(CutwasteCylinder3.Alarms);
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
    }
}
