using System;
using System.Collections.Generic;
using System.Linq;
using System.Interfaces;
using System.AdvantechAps;
namespace System.Enginee
{
    /// <summary>
    ///     表示一个运动轴。
    /// </summary>
    public abstract class Axis : Axis<string, VelocityCurve>, IAxis
    {
        public abstract void Initialize();
    }

    /// <summary>
    /// 表示一个运动轴。
    /// </summary>
    /// <typeparam name="TMotionPathId"></typeparam>
    /// <typeparam name="TVelocityCurve">运行时参数。</typeparam>
    public abstract class Axis<TMotionPathId, TVelocityCurve> : Automatic, IAxis<TMotionPathId, TVelocityCurve>
    {
        protected Axis()
        {
            MotionPaths = new List<MotionPath<TMotionPathId, TVelocityCurve>>();
        }

        #region Implementation of IAxis

        /// <summary>
        ///     轴号。
        /// </summary>
        public int NoId { get; set; }

        /// <summary>
        ///     回原点速度
        /// </summary>
        public VelocityCurve HomeSped { get; set; }
        /// <summary>
        ///     运行速度
        /// </summary>
        public VelocityCurve RunSped { get; set; }
        /// <summary>
        /// 手动动作
        /// </summary>
        public bool ManualAutoMode { get; set; }
        /// <summary>
        ///     当前 Absolute 位置。
        /// </summary>
        public abstract double CurrentPos { get; }
        /// <summary>
        ///     当前 编码器返回位置。
        /// </summary>
        public abstract double BackPos { get; }
        /// <summary>
        /// 当前速度
        /// </summary>
        public abstract double CurrentSpeed { get; }
        /// <summary>
        ///     运转速度（转/秒）。
        /// </summary>
        public double? Speed { get; set; }
        /// <summary>
        ///     是否已完成最后运动指令。
        /// </summary>
        public abstract bool IsDone { get; }
        /// <summary>
        ///     是否报警
        /// </summary>
        public abstract bool IsAlarmed { get; }

        /// <summary>
        ///     是否急停
        /// </summary>
        public abstract bool IsEmg { get; }

        /// <summary>
        ///     点位。
        /// </summary>
        public IList<MotionPath<TMotionPathId, TVelocityCurve>> MotionPaths { get; private set; }

        /// <summary>
        ///     运动轴轴移动到指定的位置。
        /// </summary>
        /// <param name="value">将要移动到的位置。</param>
        /// <param name="velocityCurve">移动时的运行参数。</param>
        public abstract void MoveTo(double value, TVelocityCurve velocityCurve = default(TVelocityCurve));

        /// <summary>
        ///     运动轴轴移动到指定的位置(软着陆)。
        /// </summary>
        /// <param name="value1">将要移动到的位置第一段。</param>
        /// <param name="value2">将要移动到的位置第二段。</param>
        /// <param name="velocityCurve">移动时的运行参数。</param>
        public abstract void MoveToExtern(double value1, double value2, VelocityCurve velocityCurve = null);

        /// <summary>
        ///     运动轴相对移动到指定位置。
        /// </summary>
        /// <param name="value">要移动到的距离。</param>
        /// <param name="velocityCurve"></param>
        public abstract void MoveDelta(double value, TVelocityCurve velocityCurve = default(TVelocityCurve));

        /// <summary>
        ///     正向移动。
        /// </summary>
        public abstract void Postive();

        /// <summary>
        ///     反向移动。
        /// </summary>
        public abstract void Negative();
        /// <summary>
        /// 回零
        /// </summary>
        /// <param name="homeMode"></param>
        /// <param name="DirMode"></param>
        /// <returns></returns>
        public abstract void BackHome();
        /// <summary>
        /// 回零判断
        /// </summary>
        /// <param name="axisNo"></param>
        /// <param name="timeoutLimit"></param>
        /// <returns></returns>
        public abstract int CheckHomeDone(double timeoutLimit);
        /// <summary>
        ///     轴停止运动。
        /// </summary>
        /// <param name="velocityCurve"></param>
        public abstract void Stop(TVelocityCurve velocityCurve = default(TVelocityCurve));

        public abstract void APS_set_command(int pos);
        public abstract StopReasons GetStopReasons { get; }
        public abstract bool IsInPosition(double pos);
        #endregion
    }
}