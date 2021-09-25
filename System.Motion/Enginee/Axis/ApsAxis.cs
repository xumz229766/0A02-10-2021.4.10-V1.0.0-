using System;
using System.AdvantechAps;
using System.Interfaces;
using System.Collections.Generic;
using System.Threading;

namespace System.Enginee
{
    /// <summary>
    ///     雷赛轴,修改于2019.6.11 finley jiang
    /// </summary>
    public class ApsAxis : Axis, INeedClean
    {
        protected readonly ApsController ApsController;
        public ApsAxis(ApsController apsController)
        {
            ApsController = apsController;
        }

        #region Overrides of Axis

        public Func<bool> _condition;
        /// <summary>
        ///     当前 Absolute 位置。
        /// </summary>
        public override double CurrentPos { get; }
        public override double BackPos { get; }

        public override double CurrentSpeed
        {
            get
            {
                return Convert.ToDouble(ApsController.GetCurrentCommandSpeed(NoId)) / Transmission.EquivalentPulse;
            }
        }

        public bool isCondition
        {
            get
            {
                try
                {
                    return _condition();
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }
        /// <summary>
        /// 轴传动参数
        /// </summary>
        public TransmissionParams Transmission { get; set; }

        #region 获取当前轴的 IO 信号
        /// <summary>
        ///     是否已励磁。
        /// </summary>
        public bool IsServon
        {
            get { return ApsController.GetServo(NoId); }
            set
            {
                if (value)
                {
                    ApsController.ServoOn(NoId);
                }
                else
                {
                    ApsController.ServoOff(NoId);
                }
            }
        }
        public uint HomeMode { get; set; }
        public uint HomeDir { get; set; }
        /// <summary>
        ///     是否到达正限位
        /// </summary>
        /// <returns></returns>
        public bool IsPEL
        {
            get { return ApsController.IsPel(NoId); }
        }
        /// <summary>
        ///     是否到达正负位
        /// </summary>
        /// <returns></returns>
        public bool IsMEL
        {
            get { return ApsController.IsMel(NoId); }
        }

        /// <summary>
        ///     是否在轴原点
        /// </summary>
        /// <param name="axisNo"></param>
        /// <returns></returns>
        public bool IsOrign
        {
            get { return ApsController.IsOrg(NoId); }
        }
        /// <summary>
        /// 是否在轴Z相
        /// </summary>
        /// <param name="axisNo"></param>
        /// <returns></returns>
        public bool IsSZ
        {
            get { return ApsController.IsSZ(NoId); }
        }
        /// <summary>
        /// 是否在轴Z相
        /// </summary>
        /// <param name="axisNo"></param>
        /// <returns></returns>
        public bool IsINP
        {
            get { return ApsController.IsINP(NoId); }
        }

        public override bool IsAlarmed { get; }

        public override bool IsEmg { get; }
        #endregion
        /// <summary>
        ///     是否已完成最后运动指令。
        /// </summary>
        /// <code>? + var isReach = Math.Abs(commandPosition - currentPosition) &lt; Precision;</code>
        public override bool IsDone
        {
            get { return ApsController.IsDown(NoId); }
        }

        /// <summary>
        /// 运动轴轴移动到指定的位置。
        /// </summary>
        /// <param name="value">将要移动到的位置。</param>
        /// <param name="velocityCurve">移动时的运行参数。</param>
        public override void MoveTo(double value, VelocityCurve velocityCurve = null)
        {
            if (!isCondition) { return; }
            var Data = value * Transmission.EquivalentPulse;
            var velocity = velocityCurve;
            velocity.Maxvel = velocityCurve.Maxvel * Transmission.EquivalentPulse;
            ApsController.MoveAbsPulse(NoId, (int)Data, velocity);
        }

        /// <summary>
        /// 运动轴轴移动到指定的位置(软着陆)。
        /// </summary>
        /// <param name="value">将要移动到的位置。</param>
        /// <param name="velocityCurve">移动时的运行参数。</param>
        public override void MoveToExtern(double value1, double value2, VelocityCurve velocityCurve = null)
        {
            if (!isCondition) { return; }
            var Data1 = value1 * Transmission.EquivalentPulse;
            var Data2 = value2 * Transmission.EquivalentPulse;
            var velocity = velocityCurve;
            velocity.Maxvel = velocityCurve.Maxvel * Transmission.EquivalentPulse;
            velocity.Stopvel = velocityCurve.Stopvel * Transmission.EquivalentPulse;
            ApsController.MoveAbsPulseExtern(NoId, (int)Data1, (int)Data2, velocity);
        }

        /// <summary>
        /// 改变运行速度。
        /// </summary>
        /// <param name="velocityCurve">移动时的运行参数。</param>
        public void AlterVelocit(VelocityCurve velocityCurve = null)
        {
            if (!isCondition) { return; }
            var velocity = velocityCurve;
            velocity.Maxvel = velocityCurve.Maxvel * Transmission.EquivalentPulse;
            ApsController.AlterAxisVelocity(NoId, velocity);
        }

        /// <summary>
        ///     运动轴相对移动到指定位置。
        /// </summary>
        /// <param name="value">要移动到的距离。</param>
        /// <param name="velocityCurve"></param>
        public override void MoveDelta(double value, VelocityCurve velocityCurve = null)
        {
            if (!isCondition) { return; }
            var Data = value * Transmission.EquivalentPulse;
            var velocity = velocityCurve;
            velocity.Maxvel = velocityCurve.Maxvel * Transmission.EquivalentPulse;
            ApsController.MoveRelPulse(NoId, (int)Data, velocity);
        }

        /// <summary>
        ///     正向移动。
        /// </summary>
        public override void Postive()
        {
            if (!isCondition) { return; }
            var velocityCurve = new VelocityCurve { Strvel = 100, Maxvel = (Speed ?? 1) * Transmission.EquivalentPulse, Tacc = 0.15, Tdec = 0.15 };
            ApsController.ContinuousMove(NoId, MoveDirection.Postive, velocityCurve);
        }

        /// <summary>
        ///     反向移动。
        /// </summary>
        public override void Negative()
        {
            if (!isCondition) { return; }
            var velocityCurve = new VelocityCurve { Strvel = 100, Maxvel = (Speed ?? 1) * Transmission.EquivalentPulse, Tacc = 0.15, Tdec = 0.15 };
            ApsController.ContinuousMove(NoId, MoveDirection.Negative, velocityCurve);
        }

        /// <summary>
        ///     轴停止运动。
        /// </summary>
        /// <param name="velocityCurve"></param>
        public override void Stop(VelocityCurve velocityCurve = null)
        {
            ApsController.ImmediateStop(NoId);
        }

        public override void Initialize()
        {
            //ApsController.MoveOrigin(NoId);
        }

        #endregion

        #region Implementation of INeedInitialization

        #endregion

        #region Implementation of INeedClean

        /// <summary>
        ///      清除
        /// </summary>
        public void Clean()
        {
            //Stop();
            //ApsController.CleanError(NoId);
        }

        public override bool IsInPosition(double pos)
        {
            throw new NotImplementedException();
        }

        public override void BackHome() => ApsController.BackHome(NoId, HomeMode, HomeDir, HomeSped);

        public override int CheckHomeDone(double timeoutLimit) => ApsController.CheckHomeDone(NoId, timeoutLimit);

        public override void APS_set_command(int pos)
        {
            throw new NotImplementedException();
        }

        public override StopReasons GetStopReasons
        {
            get
            {
                return (StopReasons)0;
            }
        }
        /// <summary>
        /// 轴报警集合
        /// </summary>
        public IList<Alarm> Alarms
        {
            get
            {
                var list = new List<Alarm>();
                list.Add(new Alarm(() => ApsController.IsAlm(NoId)) { AlarmLevel = AlarmLevels.Error, Name = Name + "伺服报警,重启电源,重设"+ Name + "原点和限位" });
                list.Add(new Alarm(() => ApsController.IsErr(NoId)) { AlarmLevel = AlarmLevels.Error, Name = Name + " -- 总线错误" });
                return list;
            }
        }
        #endregion

        #region EtherCAT总线
        /// <summary>
        ///  剪切轴扭力控制。
        /// </summary>
        /// <param name="Value">要移动到的距离。</param>
        /// <param name="Torque">控制的扭力。</param>
        /// <param name="PosLimitValue">扭力限制位置。</param>
        /// <param name="Time">暂停时间。</param>
        /// <param name="maxvel">限制最大速度。</param>
        /// <param name="velocityCurve"></param>
        public bool TorqueControl(double Value, int Torque, double PosLimitValue, ushort maxvel, VelocityCurve velocityCurve = null)
        {
            var Data = PosLimitValue * Transmission.EquivalentPulse;
            MoveTo(Value, velocityCurve);
           
            while (true)
            {
                //if ((Value * Transmission.EquivalentPulse) < ApsController.GetCurrentFeedbackPosition(NoId))
                //{
                Thread.Sleep(10);
                //    break;
                //}
                if (ApsController.IsDown(NoId))
                {
                    break;
                }
            }
            ushort NodeNum = (ushort)(1001 + NoId);
            ApsController.SetNodeOd(NodeNum, 24704, 0, 32, maxvel);
            ushort PosLimitValid = 1; // 停止在限位位置
            PosLimitValid = (UInt16)(PosLimitValid | 0x80); //强制切换成扭力模式
            ApsController.EtherCATTorqueMove(NoId, Torque, PosLimitValid, Data-100, 1); // 绝对位置
            int FeedbackPos = 0;
            do
            {
                FeedbackPos = ApsController.GetCurrentFeedbackPosition(NoId);
                Thread.Sleep(10);
            }
            while ((Math.Abs(ApsController.GetCurrentFeedbackPosition(NoId) - FeedbackPos)) > 5);             
            ApsController.DecelStop(NoId);
            ApsController.SetNodeOd(NodeNum, 24704, 0, 32,Convert.ToInt32(velocityCurve.Maxvel));
            if (ApsController.GetCurrentFeedbackPosition(NoId) <= (Data + 100) && ApsController.GetCurrentFeedbackPosition(NoId) >= (Data - 100))
            {
                return true;
            }
            return false;
        }

        #endregion
    }
}