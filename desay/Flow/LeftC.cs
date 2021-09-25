using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Enginee;
using System.Interfaces;
using System.Threading;
using System.ToolKit;

namespace desay
{
    /// <summary>
    /// C轴模块
    /// </summary>
    public class LeftC : ThreadPart
    {
        public List<Alarm> Alarms;
        private LeftCAlarm m_Alarm, m_Alarm1, m_Alarm2, m_Alarm3, m_Alarm4;

        public LeftC(External ExternalSign, StationInitialize stationIni, StationOperate stationOpe)
        {
            externalSign = ExternalSign;
            stationInitialize = stationIni;
            stationOperate = stationOpe;
        }
        public StationInitialize stationInitialize { get; set; }
        public StationOperate stationOperate { get; set; }
        public External externalSign { get; set; }
        /// <summary>
        /// C1轴
        /// </summary>
        public ServoAxis C1Axis { get; set; }
        /// <summary>
        /// C2轴
        /// </summary>
        public ServoAxis C2Axis { get; set; }
        /// <summary>
        /// C3轴
        /// </summary>
        public ServoAxis C3Axis { get; set; }
        /// <summary>
        /// C4轴
        /// </summary>
        public ServoAxis C4Axis { get; set; }
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
        /// 回待机位
        /// </summary>
        public int homeWaitStep = 0;

        public override void Running(RunningModes runningMode)
        {
            try
            {
                var step = 0;   //C轴流程    
                double C1Pos = 0; //C1位置
                double C2Pos = 0; //C2位置
                double C3Pos = 0; //C3位置
                double C4Pos = 0; //C4位置
                double P1Pos = 0; //P1位置
                double P2Pos = 0; //P2位置
                double P3Pos = 0; //P3位置
                double P4Pos = 0; //P4位置

                Stopwatch IntWatch = new Stopwatch();
                IntWatch.Start();
                while (true)
                {
                    Thread.Sleep(5);
                    if (!stationOperate.Running) { Marking.watchModel.Stop(); } else { Marking.watchModel.Start(); }

                    #region  自动流程
                    if (stationOperate.Running)
                    {
                        switch (step)
                        {
                            case 0://判断是否有料                       
                                step = 10;
                                break;
                            case 10://判断C轴推出气缸状态是否在原点，然后将C轴移动1穴位置
                                if (C1Axis.IsInPosition(Position.Instance.Caxis[0].Startangle) && C2Axis.IsInPosition(Position.Instance.Caxis[1].Startangle) &&
                                    C3Axis.IsInPosition(Position.Instance.Caxis[2].Startangle) && C4Axis.IsInPosition(Position.Instance.Caxis[3].Startangle) &&
                                    !Marking.CAxisFinish)
                                {
                                    step = 20;
                                }
                                else
                                {
                                    C1Axis.MoveTo(Position.Instance.Caxis[0].Startangle, AxisParameter.Instance.C1VelocityCurve);
                                    C2Axis.MoveTo(Position.Instance.Caxis[1].Startangle, AxisParameter.Instance.C2VelocityCurve);
                                    C3Axis.MoveTo(Position.Instance.Caxis[2].Startangle, AxisParameter.Instance.C3VelocityCurve);
                                    C4Axis.MoveTo(Position.Instance.Caxis[3].Startangle, AxisParameter.Instance.C4VelocityCurve);
                                    step = 20;
                                }
                                break;
                            case 20:
                                if (Push1Axis.IsInPosition(Position.Instance.PosPush[0].Origin) && Push2Axis.IsInPosition(Position.Instance.PosPush[1].Origin) &&
                                    Push3Axis.IsInPosition(Position.Instance.PosPush[2].Origin) && Push4Axis.IsInPosition(Position.Instance.PosPush[3].Origin))
                                {
                                    step = 30;
                                }
                                else
                                {
                                    Push1Axis.MoveTo(Position.Instance.PosPush[0].Origin, AxisParameter.Instance.Push1VelocityCurve);
                                    Push2Axis.MoveTo(Position.Instance.PosPush[1].Origin, AxisParameter.Instance.Push2VelocityCurve);
                                    Push3Axis.MoveTo(Position.Instance.PosPush[2].Origin, AxisParameter.Instance.Push3VelocityCurve);
                                    Push4Axis.MoveTo(Position.Instance.PosPush[3].Origin, AxisParameter.Instance.Push4VelocityCurve);
                                }
                                break;
                            case 30:
                                Marking.MoveFinish = true;
                                Marking.watchModelValue = Marking.watchModel.ElapsedMilliseconds / 1000.0;
                                Marking.watchModel.Restart();
                                if (stationOperate.SingleRunning)
                                {
                                    Thread.Sleep(100);
                                }
                                step = 35;
                                break;
                            case 35:
                                if((!(Marking.SelectCheckMode && Config.Instance.SelectCheckRunState) && Marking.SelectCheckMode) || Marking.MancelChangeTray)
                                {
                                    if(Marking.changeTrayLayoutSign)
                                    {
                                        step = 40;
                                    }
                                }
                                else
                                {
                                    step = 40;
                                }
                                break;
                            case 40://判断放好料、等待来料标志为false,旋转到设定的第一个料号
                                try
                                {
                                    if (!Marking.ThrowerModeFrist && !Marking.changeTrayLayoutWaitSign && !Marking.changeTrayLayoutSign  && !Marking.MoveFinish)
                                    {
                                        C1Pos = Position.Instance.C1holes[Config.Instance.CaxisCount[0]] + Position.Instance.C1HolesOffset[Config.Instance.CaxisCount[0]]; //通过磨具号找到对应的C轴位置
                                        C2Pos = Position.Instance.C2holes[Config.Instance.CaxisCount[1]] + Position.Instance.C2HolesOffset[Config.Instance.CaxisCount[1]];
                                        C3Pos = Position.Instance.C3holes[Config.Instance.CaxisCount[2]] + Position.Instance.C3HolesOffset[Config.Instance.CaxisCount[2]];
                                        C4Pos = Position.Instance.C4holes[Config.Instance.CaxisCount[3]] + Position.Instance.C4HolesOffset[Config.Instance.CaxisCount[3]];
                                        C1Axis.MoveTo(C1Pos, AxisParameter.Instance.C1VelocityCurve);
                                        C2Axis.MoveTo(C2Pos, AxisParameter.Instance.C2VelocityCurve);
                                        C3Axis.MoveTo(C3Pos, AxisParameter.Instance.C3VelocityCurve);
                                        C4Axis.MoveTo(C4Pos, AxisParameter.Instance.C4VelocityCurve);
                                        P1Pos = Position.Instance.PosPush[0].Move + Position.Instance.P1HolesOffset[Config.Instance.CaxisCount[0]]; //通过磨具号找到对应的P轴位置
                                        P2Pos = Position.Instance.PosPush[1].Move + Position.Instance.P2HolesOffset[Config.Instance.CaxisCount[1]];
                                        P3Pos = Position.Instance.PosPush[2].Move + Position.Instance.P3HolesOffset[Config.Instance.CaxisCount[2]];
                                        P4Pos = Position.Instance.PosPush[3].Move + Position.Instance.P4HolesOffset[Config.Instance.CaxisCount[3]];
                                        Push1Axis.MoveTo(P1Pos, AxisParameter.Instance.Push1VelocityCurve);
                                        Push2Axis.MoveTo(P2Pos, AxisParameter.Instance.Push2VelocityCurve);
                                        Push3Axis.MoveTo(P3Pos, AxisParameter.Instance.Push3VelocityCurve);
                                        Push4Axis.MoveTo(P4Pos, AxisParameter.Instance.Push4VelocityCurve);
                                        step = 50;
                                    }
                                }
                                catch (Exception ex)
                                {
                                    m_Alarm = LeftCAlarm.角度计算异常;
                                }
                                break;
                            case 50://推出气缸到位，C轴准备好标志置位
                                if (C1Axis.IsInPosition(C1Pos) && C2Axis.IsInPosition(C2Pos) && C3Axis.IsInPosition(C3Pos) && C4Axis.IsInPosition(C4Pos) &&
                                    Push1Axis.IsInPosition(P1Pos) && Push2Axis.IsInPosition(P2Pos) && Push3Axis.IsInPosition(P3Pos) && Push4Axis.IsInPosition(P4Pos))
                                {
                                    Thread.Sleep(Delay.Instance.CaxisFrontDelay);
                                    Marking.CAxisFinish = true;
                                    step = 60;
                                }
                                break;
                            case 60:
                                if (!Marking.LeftCut1Finish && !Marking.LeftCut2Finish && !Marking.RightCut1Finish && !Marking.RightCut1Finish)
                                {
                                    step = 70;
                                }
                                break;
                            case 70://判断C轴准备好标志复位，穴数+1
                                if ((Marking.LeftCut1Finish && Marking.LeftCut2Finish && Marking.RightCut1Finish && Marking.RightCut2Finish) || stationOperate.SingleRunning)
                                {
                                    Marking.CAxisFinish = false;
                                    Config.Instance.CaxisCount[0]++;
                                    Config.Instance.CaxisCount[1]++;
                                    Config.Instance.CaxisCount[2]++;
                                    Config.Instance.CaxisCount[3]++;
                                    if (Config.Instance.CaxisCount[0] < (Position.Instance.HoleNumber / 4))
                                    {
                                        if (Position.Instance.CAxisOrgRotate)
                                        {
                                            Push1Axis.MoveTo(Position.Instance.PosPush[0].Origin, AxisParameter.Instance.Push1VelocityCurve);
                                            Push2Axis.MoveTo(Position.Instance.PosPush[1].Origin, AxisParameter.Instance.Push2VelocityCurve);
                                            Push3Axis.MoveTo(Position.Instance.PosPush[2].Origin, AxisParameter.Instance.Push3VelocityCurve);
                                            Push4Axis.MoveTo(Position.Instance.PosPush[3].Origin, AxisParameter.Instance.Push4VelocityCurve);
                                        }
                                        step = 80;
                                    }
                                    else
                                    {
                                        step = 110;
                                    }
                                }
                                break;
                            case 80://C轴移动到下一穴，C轴准备好标志置位。                           
                                try
                                {
                                    if ((Push1Axis.IsInPosition(Position.Instance.PosPush[0].Origin) && Push2Axis.IsInPosition(Position.Instance.PosPush[1].Origin) &&
                                        Push3Axis.IsInPosition(Position.Instance.PosPush[2].Origin) && Push4Axis.IsInPosition(Position.Instance.PosPush[3].Origin))
                                        || !Position.Instance.CAxisOrgRotate)
                                    {
                                        C1Pos = Position.Instance.C1holes[Config.Instance.CaxisCount[0]] + Position.Instance.C1HolesOffset[Config.Instance.CaxisCount[0]];                      //通过磨具号找到对应的C轴位置
                                        C2Pos = Position.Instance.C2holes[Config.Instance.CaxisCount[1]] + Position.Instance.C2HolesOffset[Config.Instance.CaxisCount[1]];
                                        C3Pos = Position.Instance.C3holes[Config.Instance.CaxisCount[2]] + Position.Instance.C3HolesOffset[Config.Instance.CaxisCount[2]];
                                        C4Pos = Position.Instance.C4holes[Config.Instance.CaxisCount[3]] + Position.Instance.C4HolesOffset[Config.Instance.CaxisCount[3]];
                                        C1Axis.MoveTo(C1Pos, AxisParameter.Instance.C1VelocityCurve);
                                        C2Axis.MoveTo(C2Pos, AxisParameter.Instance.C2VelocityCurve);
                                        C3Axis.MoveTo(C3Pos, AxisParameter.Instance.C3VelocityCurve);
                                        C4Axis.MoveTo(C4Pos, AxisParameter.Instance.C4VelocityCurve);
                                        P1Pos = Position.Instance.PosPush[0].Move + Position.Instance.P1HolesOffset[Config.Instance.CaxisCount[0]];
                                        P2Pos = Position.Instance.PosPush[1].Move + Position.Instance.P2HolesOffset[Config.Instance.CaxisCount[1]];
                                        P3Pos = Position.Instance.PosPush[2].Move + Position.Instance.P3HolesOffset[Config.Instance.CaxisCount[2]];
                                        P4Pos = Position.Instance.PosPush[3].Move + Position.Instance.P4HolesOffset[Config.Instance.CaxisCount[3]];
                                        Push1Axis.MoveTo(P1Pos, AxisParameter.Instance.Push1VelocityCurve);
                                        Push2Axis.MoveTo(P2Pos, AxisParameter.Instance.Push2VelocityCurve);
                                        Push3Axis.MoveTo(P3Pos, AxisParameter.Instance.Push3VelocityCurve);
                                        Push4Axis.MoveTo(P4Pos, AxisParameter.Instance.Push4VelocityCurve);
                                        step = 90;
                                    }
                                }
                                catch (Exception ex)
                                {
                                    m_Alarm = LeftCAlarm.角度计算异常;
                                }
                                break;
                            case 90:
                                if (C1Axis.IsInPosition(C1Pos) && C2Axis.IsInPosition(C2Pos) && C3Axis.IsInPosition(C3Pos) && C4Axis.IsInPosition(C4Pos) &&
                                    Push1Axis.IsInPosition(P1Pos) && Push2Axis.IsInPosition(P2Pos) && Push3Axis.IsInPosition(P3Pos) && Push4Axis.IsInPosition(P4Pos))
                                {
                                    if (Position.Instance.CAxisOrgRotate)
                                    {
                                        P1Pos = Position.Instance.PosPush[0].Move + Position.Instance.P1HolesOffset[Config.Instance.CaxisCount[0]];
                                        P2Pos = Position.Instance.PosPush[1].Move + Position.Instance.P2HolesOffset[Config.Instance.CaxisCount[1]];
                                        P3Pos = Position.Instance.PosPush[2].Move + Position.Instance.P3HolesOffset[Config.Instance.CaxisCount[2]];
                                        P4Pos = Position.Instance.PosPush[3].Move + Position.Instance.P4HolesOffset[Config.Instance.CaxisCount[3]];
                                        Push1Axis.MoveTo(P1Pos, AxisParameter.Instance.Push1VelocityCurve);
                                        Push2Axis.MoveTo(P2Pos, AxisParameter.Instance.Push2VelocityCurve);
                                        Push3Axis.MoveTo(P3Pos, AxisParameter.Instance.Push3VelocityCurve);
                                        Push4Axis.MoveTo(P4Pos, AxisParameter.Instance.Push4VelocityCurve);
                                    }
                                    step = 100;
                                }
                                break;
                            case 100:
                                if ((Push1Axis.IsInPosition(P1Pos) && Push2Axis.IsInPosition(P2Pos) && Push3Axis.IsInPosition(P3Pos) && Push4Axis.IsInPosition(P4Pos))
                                    || !Position.Instance.CAxisOrgRotate)
                                {
                                    Thread.Sleep(Delay.Instance.CaxisFrontDelay);
                                    if (stationOperate.SingleRunning)
                                    {
                                        Thread.Sleep(100);
                                    }
                                    Marking.CAxisFinish = true;
                                    step = 60;
                                }
                                break;
                            case 110://C轴准备好标志复位，推进轴返回待机位
                                if (!Marking.CAxisFinish)
                                {
                                    Config.Instance.CaxisCount[0] = 0;
                                    Config.Instance.CaxisCount[1] = 0;
                                    Config.Instance.CaxisCount[2] = 0;
                                    Config.Instance.CaxisCount[3] = 0;
                                    C1Axis.MoveTo(Position.Instance.Caxis[0].Startangle, AxisParameter.Instance.C1VelocityCurve);
                                    C2Axis.MoveTo(Position.Instance.Caxis[1].Startangle, AxisParameter.Instance.C2VelocityCurve);
                                    C3Axis.MoveTo(Position.Instance.Caxis[2].Startangle, AxisParameter.Instance.C3VelocityCurve);
                                    C4Axis.MoveTo(Position.Instance.Caxis[3].Startangle, AxisParameter.Instance.C4VelocityCurve);
                                    Push1Axis.MoveTo(Position.Instance.PosPush[0].Origin, AxisParameter.Instance.Push1VelocityCurve);
                                    Push2Axis.MoveTo(Position.Instance.PosPush[1].Origin, AxisParameter.Instance.Push2VelocityCurve);
                                    Push3Axis.MoveTo(Position.Instance.PosPush[2].Origin, AxisParameter.Instance.Push3VelocityCurve);
                                    Push4Axis.MoveTo(Position.Instance.PosPush[3].Origin, AxisParameter.Instance.Push4VelocityCurve);
                                    step = 120;
                                }
                                break;
                            case 120://推出气缸到达
                                if (C1Axis.IsInPosition(Position.Instance.Caxis[0].Startangle) && C2Axis.IsInPosition(Position.Instance.Caxis[1].Startangle) &&
                                    C3Axis.IsInPosition(Position.Instance.Caxis[2].Startangle) && C4Axis.IsInPosition(Position.Instance.Caxis[3].Startangle) &&
                                    Push1Axis.IsInPosition(Position.Instance.PosPush[0].Origin) && Push2Axis.IsInPosition(Position.Instance.PosPush[1].Origin) &&
                                    Push3Axis.IsInPosition(Position.Instance.PosPush[2].Origin) && Push4Axis.IsInPosition(Position.Instance.PosPush[3].Origin))
                                {
                                    step = 130;
                                }
                                break;
                            default:
                                if (!Marking.SystemStop)
                                {
                                    step = 0;
                                }
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
                                Marking.CAxisFinish = false;
                                Marking.MoveFinish = false;
                                Config.Instance.CaxisCount[0] = 0;
                                Config.Instance.CaxisCount[1] = 0;
                                Config.Instance.CaxisCount[2] = 0;
                                Config.Instance.CaxisCount[3] = 0;
                                m_Alarm1 = LeftCAlarm.推进1轴复位中;
                                m_Alarm2 = LeftCAlarm.推进2轴复位中;
                                m_Alarm3 = LeftCAlarm.推进3轴复位中;
                                m_Alarm4 = LeftCAlarm.推进4轴复位中;
                                Push1Axis.IsServon = true;
                                Push2Axis.IsServon = true;
                                Push3Axis.IsServon = true;
                                Push4Axis.IsServon = true;
                                Thread.Sleep(500);
                                stationInitialize.Flow = 10;
                                break;
                            case 10:
                                if (Push1Axis.IsDone && Push2Axis.IsDone && Push3Axis.IsDone && Push4Axis.IsDone && Push1Axis.IsInPosition(Push1Axis.CurrentPos)
                                     && Push2Axis.IsInPosition(Push2Axis.CurrentPos) && Push3Axis.IsInPosition(Push3Axis.CurrentPos) && Push4Axis.IsInPosition(Push4Axis.CurrentPos))
                                {
                                    stationInitialize.Flow = 20;
                                }
                                else
                                {
                                    Push1Axis.Stop();
                                    Push2Axis.Stop();
                                    Push3Axis.Stop();
                                    Push4Axis.Stop();
                                }
                                break;
                            case 20:
                                Push1Axis.BackHome();
                                Push2Axis.BackHome();
                                Push3Axis.BackHome();
                                Push4Axis.BackHome();
                                stationInitialize.Flow = 30;
                                break;
                            case 30:
                                Thread.Sleep(50);
                                if (Push1Axis.IsInPosition(0) && Push2Axis.IsInPosition(0) && Push3Axis.IsInPosition(0) && Push4Axis.IsInPosition(0))
                                {
                                    m_Alarm1 = LeftCAlarm.C1轴复位中;
                                    m_Alarm2 = LeftCAlarm.C2轴复位中;
                                    m_Alarm3 = LeftCAlarm.C3轴复位中;
                                    m_Alarm4 = LeftCAlarm.C4轴复位中;
                                    C1Axis.IsServon = true;
                                    C2Axis.IsServon = true;
                                    C3Axis.IsServon = true;
                                    C4Axis.IsServon = true;
                                    Thread.Sleep(500);
                                    stationInitialize.Flow = 40;
                                }
                                break;
                            case 40:
                                if (C1Axis.IsDone && C2Axis.IsDone && C3Axis.IsDone && C4Axis.IsDone && C1Axis.IsInPosition(C1Axis.CurrentPos)
                                     && C2Axis.IsInPosition(C2Axis.CurrentPos) && C3Axis.IsInPosition(C3Axis.CurrentPos) && C4Axis.IsInPosition(C4Axis.CurrentPos))
                                {
                                    stationInitialize.Flow = 50;
                                }
                                else
                                {
                                    C1Axis.Stop();
                                    C2Axis.Stop();
                                    C3Axis.Stop();
                                    C4Axis.Stop();
                                }
                                break;
                            case 50:
                                C1Axis.BackHome();
                                C2Axis.BackHome();
                                C3Axis.BackHome();
                                C4Axis.BackHome();
                                IntWatch.Restart();
                                stationInitialize.Flow = 60;
                                break;
                            case 60:
                                Thread.Sleep(50);
                                if (C1Axis.IsInPosition(0) && C2Axis.IsInPosition(0) && C3Axis.IsInPosition(0) && C4Axis.IsInPosition(0))
                                {
                                    if (!C1Axis.IsInPosition(Position.Instance.Caxis[0].Startangle)) { C1Axis.MoveTo(Position.Instance.Caxis[0].Startangle, AxisParameter.Instance.C1VelocityCurve); }
                                    if (!C2Axis.IsInPosition(Position.Instance.Caxis[1].Startangle)) { C2Axis.MoveTo(Position.Instance.Caxis[1].Startangle, AxisParameter.Instance.C2VelocityCurve); }
                                    if (!C3Axis.IsInPosition(Position.Instance.Caxis[2].Startangle)) { C3Axis.MoveTo(Position.Instance.Caxis[2].Startangle, AxisParameter.Instance.C3VelocityCurve); }
                                    if (!C4Axis.IsInPosition(Position.Instance.Caxis[3].Startangle)) { C4Axis.MoveTo(Position.Instance.Caxis[3].Startangle, AxisParameter.Instance.C4VelocityCurve); }
                                    Marking.MoveFinish = false;
                                    stationInitialize.Flow = 70;
                                }
                                break;
                            case 70:
                                Thread.Sleep(10);
                                if (C1Axis.IsInPosition(Position.Instance.Caxis[0].Startangle) && C2Axis.IsInPosition(Position.Instance.Caxis[1].Startangle) &&
                                    C3Axis.IsInPosition(Position.Instance.Caxis[2].Startangle) && C4Axis.IsInPosition(Position.Instance.Caxis[3].Startangle))
                                {
                                    stationInitialize.InitializeDone = true;
                                    stationInitialize.Flow = 80;
                                }
                                if (C1Axis.IsInPosition(Position.Instance.Caxis[0].Startangle)) m_Alarm1 = LeftCAlarm.无消息;
                                if (C2Axis.IsInPosition(Position.Instance.Caxis[1].Startangle)) m_Alarm2 = LeftCAlarm.无消息;
                                if (C3Axis.IsInPosition(Position.Instance.Caxis[2].Startangle)) m_Alarm3 = LeftCAlarm.无消息;
                                if (C4Axis.IsInPosition(Position.Instance.Caxis[3].Startangle)) m_Alarm4 = LeftCAlarm.无消息;
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
                                if (Marking.equipmentHomeWaitState[4] && Marking.equipmentHomeWaitState[5]
                                    && Marking.equipmentHomeWaitState[6] && Marking.equipmentHomeWaitState[7])
                                {
                                    step = 0;
                                    Marking.CAxisFinish = false;                              
                                    Marking.MoveFinish = false;
                                    Config.Instance.CaxisCount[0] = Global.BigTray.CurrentPos;
                                    Config.Instance.CaxisCount[1] = Global.BigTray.CurrentPos;
                                    Config.Instance.CaxisCount[2] = Global.BigTray.CurrentPos;
                                    Config.Instance.CaxisCount[3] = Global.BigTray.CurrentPos;
                                    Push1Axis.IsServon = true;
                                    Push2Axis.IsServon = true;
                                    Push3Axis.IsServon = true;
                                    Push4Axis.IsServon = true;
                                    C1Axis.IsServon = true;
                                    C2Axis.IsServon = true;
                                    C3Axis.IsServon = true;
                                    C4Axis.IsServon = true;
                                    Thread.Sleep(500);
                                    homeWaitStep = 10;
                                }
                                break;
                            case 10:
                                Push1Axis.MoveTo(Position.Instance.PosPush[0].Origin, AxisParameter.Instance.Push1VelocityCurve);
                                Push2Axis.MoveTo(Position.Instance.PosPush[1].Origin, AxisParameter.Instance.Push2VelocityCurve);
                                Push3Axis.MoveTo(Position.Instance.PosPush[2].Origin, AxisParameter.Instance.Push3VelocityCurve);
                                Push4Axis.MoveTo(Position.Instance.PosPush[3].Origin, AxisParameter.Instance.Push4VelocityCurve);
                                homeWaitStep = 20;
                                break;
                            case 20:
                                if (Push1Axis.IsInPosition(Position.Instance.PosPush[0].Origin) && Push2Axis.IsInPosition(Position.Instance.PosPush[1].Origin) &&
                                    Push3Axis.IsInPosition(Position.Instance.PosPush[2].Origin) && Push4Axis.IsInPosition(Position.Instance.PosPush[3].Origin) &&
                                    IoPoints.T1IN14.Value)
                                {
                                    C1Axis.MoveTo(Position.Instance.Caxis[0].Startangle, AxisParameter.Instance.C1VelocityCurve);
                                    C2Axis.MoveTo(Position.Instance.Caxis[1].Startangle, AxisParameter.Instance.C2VelocityCurve);
                                    C3Axis.MoveTo(Position.Instance.Caxis[2].Startangle, AxisParameter.Instance.C3VelocityCurve);
                                    C4Axis.MoveTo(Position.Instance.Caxis[3].Startangle, AxisParameter.Instance.C4VelocityCurve);
                                    Marking.equipmentHomeWaitState[3] = true;
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
                        m_Alarm = LeftCAlarm.无消息;
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
                return list;
            }
        }
        public void AddAlarms()
        {
            try
            {
                Alarms = new List<Alarm>();
                Alarms.Add(new Alarm(() => m_Alarm == LeftCAlarm.角度计算异常) { AlarmLevel = AlarmLevels.Error, Name = LeftCAlarm.角度计算异常.ToString() });
                Alarms.Add(new Alarm(() => m_Alarm1 == LeftCAlarm.推进1轴复位中) { AlarmLevel = AlarmLevels.None, Name = LeftCAlarm.推进1轴复位中.ToString() });
                Alarms.Add(new Alarm(() => m_Alarm2 == LeftCAlarm.推进2轴复位中) { AlarmLevel = AlarmLevels.None, Name = LeftCAlarm.推进2轴复位中.ToString() });
                Alarms.Add(new Alarm(() => m_Alarm3 == LeftCAlarm.推进3轴复位中) { AlarmLevel = AlarmLevels.None, Name = LeftCAlarm.推进3轴复位中.ToString() });
                Alarms.Add(new Alarm(() => m_Alarm4 == LeftCAlarm.推进4轴复位中) { AlarmLevel = AlarmLevels.None, Name = LeftCAlarm.推进4轴复位中.ToString() });
                Alarms.Add(new Alarm(() => m_Alarm1 == LeftCAlarm.C1轴复位中) { AlarmLevel = AlarmLevels.None, Name = LeftCAlarm.C1轴复位中.ToString() });
                Alarms.Add(new Alarm(() => m_Alarm2 == LeftCAlarm.C2轴复位中) { AlarmLevel = AlarmLevels.None, Name = LeftCAlarm.C2轴复位中.ToString() });
                Alarms.Add(new Alarm(() => m_Alarm3 == LeftCAlarm.C3轴复位中) { AlarmLevel = AlarmLevels.None, Name = LeftCAlarm.C3轴复位中.ToString() });
                Alarms.Add(new Alarm(() => m_Alarm4 == LeftCAlarm.C4轴复位中) { AlarmLevel = AlarmLevels.None, Name = LeftCAlarm.C4轴复位中.ToString() });
                Alarms.AddRange(Push1Axis.Alarms);
                Alarms.AddRange(Push2Axis.Alarms);
                Alarms.AddRange(Push3Axis.Alarms);
                Alarms.AddRange(Push4Axis.Alarms);
                Alarms.AddRange(C1Axis.Alarms);
                Alarms.AddRange(C2Axis.Alarms);
                Alarms.AddRange(C3Axis.Alarms);
                Alarms.AddRange(C4Axis.Alarms);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
    }
}
