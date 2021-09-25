using System;
using System.Interfaces;
using System.Diagnostics;
namespace System.Enginee
{
    /// <summary>
    ///     多层指示灯
    /// </summary>
    public class LayerLight : Automatic, IRefreshing
    {
        private readonly Stopwatch _watch = new Stopwatch();
        private bool intervalSign;
        /// <summary>
        /// 运行时三色灯的状态
        /// </summary>
        public TricolorLamp RunSturs;
        /// <summary>
        /// 暂停时三色灯的状态
        /// </summary>
        public TricolorLamp SuspendSturs;
        /// <summary>
        /// 停止时三色灯的状态
        /// </summary>
        public TricolorLamp StopSturs;
        /// <summary>
        /// 故障时三色灯的状态
        /// </summary>
        public TricolorLamp ErrSturs;
        /// <summary>
        /// 复位时三色灯的状态
        /// </summary>
        public TricolorLamp ResetSturs;
        /// <summary>
        /// 有警示时三色灯的状态
        /// </summary>
        public TricolorLamp WarningSturs;

        public LayerLight(IoPoint greenIo, IoPoint yellowIo, IoPoint redIo, IoPoint speakerIo, TricolorLamp mRunSturs, TricolorLamp mSuspendSturs,
            TricolorLamp mStopSturs, TricolorLamp mErrSturs, TricolorLamp mResetSturs, TricolorLamp mWarningSturs)
        {
            GreenLamp = greenIo;
            YellowLamp = yellowIo;
            RedLamp = redIo;
            Speeker = speakerIo;
            RunSturs = mRunSturs;
            SuspendSturs = mSuspendSturs;
            StopSturs = mStopSturs;
            ErrSturs = mErrSturs;
            ResetSturs = mResetSturs;
            WarningSturs = mWarningSturs;
        }

        private IoPoint GreenLamp { get; set; }
        private IoPoint YellowLamp { get; set; }
        private IoPoint RedLamp { get; set; }
        private IoPoint Speeker { get; set; }
        public bool VoiceClosed { get; set; }
        public MachineStatus Status { private get; set; }

        public void Refreshing()
        {
            #region 间隔

            _watch.Stop();
            if (intervalSign)
            {
                if (_watch.ElapsedMilliseconds / 1000.0 > 0.5)
                {
                    intervalSign = false;
                    _watch.Restart();
                }
            }
            else
            {
                if (_watch.ElapsedMilliseconds / 1000.0 > 0.5)
                {
                    intervalSign = true;
                    _watch.Restart();
                }
            }
            _watch.Start();

            #endregion

            if (Status == MachineStatus.设备运行中)
            {
                switch (RunSturs.isGreen)
                {
                    case light.常亮:
                        GreenLamp.Value = true;
                        break;
                    case light.闪烁:
                        GreenLamp.Value = intervalSign ? true : false;
                        break;
                    case light.常灭:
                        GreenLamp.Value = false;
                        break;
                }
                switch (RunSturs.isYellow)
                {
                    case light.常亮:
                        YellowLamp.Value = true;
                        break;
                    case light.闪烁:
                        YellowLamp.Value = intervalSign ? true : false;
                        break;
                    case light.常灭:
                        YellowLamp.Value = false;
                        break;
                }
                switch (RunSturs.isRed)
                {
                    case light.常亮:
                        RedLamp.Value = true;
                        break;
                    case light.闪烁:
                        RedLamp.Value = intervalSign ? true : false;
                        break;
                    case light.常灭:
                        RedLamp.Value = false;
                        break;
                }
                if (!VoiceClosed)
                {
                    switch (RunSturs.Buzzer)
                    {
                        case Buzzer.长音:
                            Speeker.Value = true;
                            break;
                        case Buzzer.间隔音:
                            Speeker.Value = intervalSign ? true : false;
                            break;
                        case Buzzer.静音:
                            Speeker.Value = false;
                            break;

                    }
                }
                else { Speeker.Value = false; }
            }

            if (Status == MachineStatus.设备暂停中)
            {
                switch (SuspendSturs.isGreen)
                {
                    case light.常亮:
                        GreenLamp.Value = true;
                        break;
                    case light.闪烁:
                        GreenLamp.Value = intervalSign ? true : false;
                        break;
                    case light.常灭:
                        GreenLamp.Value = false;
                        break;
                }
                switch (SuspendSturs.isYellow)
                {
                    case light.常亮:
                        YellowLamp.Value = true;
                        break;
                    case light.闪烁:
                        YellowLamp.Value = intervalSign ? true : false;
                        break;
                    case light.常灭:
                        YellowLamp.Value = false;
                        break;
                }
                switch (SuspendSturs.isRed)
                {
                    case light.常亮:
                        RedLamp.Value = true;
                        break;
                    case light.闪烁:
                        RedLamp.Value = intervalSign ? true : false;
                        break;
                    case light.常灭:
                        RedLamp.Value = false;
                        break;
                }
                switch (SuspendSturs.Buzzer)
                {
                    case Buzzer.长音:
                        Speeker.Value = true;
                        break;
                    case Buzzer.间隔音:
                        Speeker.Value = intervalSign ? true : false;
                        break;
                    case Buzzer.静音:
                        Speeker.Value = false;
                        break;
                }
            }
            if (Status == MachineStatus.设备复位中)
            {
                switch (ResetSturs.isGreen)
                {
                    case light.常亮:
                        GreenLamp.Value = true;
                        break;
                    case light.闪烁:
                        GreenLamp.Value = intervalSign ? true : false;
                        break;
                    case light.常灭:
                        GreenLamp.Value = false;
                        break;
                }
                switch (ResetSturs.isYellow)
                {
                    case light.常亮:
                        YellowLamp.Value = true;
                        break;
                    case light.闪烁:
                        YellowLamp.Value = intervalSign ? true : false;
                        break;
                    case light.常灭:
                        YellowLamp.Value = false;
                        break;
                }
                switch (ResetSturs.isRed)
                {
                    case light.常亮:
                        RedLamp.Value = true;
                        break;
                    case light.闪烁:
                        RedLamp.Value = intervalSign ? true : false;
                        break;
                    case light.常灭:
                        RedLamp.Value = false;
                        break;
                }
                switch (ResetSturs.Buzzer)
                {
                    case Buzzer.长音:
                        Speeker.Value = true;
                        break;
                    case Buzzer.间隔音:
                        Speeker.Value = intervalSign ? true : false;
                        break;
                    case Buzzer.静音:
                        Speeker.Value = false;
                        break;
                }
            }
            if (Status == MachineStatus.设备报警中)
            {
                switch (ErrSturs.isGreen)
                {
                    case light.常亮:
                        GreenLamp.Value = true;
                        break;
                    case light.闪烁:
                        GreenLamp.Value = intervalSign ? true : false;
                        break;
                    case light.常灭:
                        GreenLamp.Value = false;
                        break;
                }
                switch (ErrSturs.isYellow)
                {
                    case light.常亮:
                        YellowLamp.Value = true;
                        break;
                    case light.闪烁:
                        YellowLamp.Value = intervalSign ? true : false;
                        break;
                    case light.常灭:
                        YellowLamp.Value = false;
                        break;
                }
                switch (ErrSturs.isRed)
                {
                    case light.常亮:
                        RedLamp.Value = true;
                        break;
                    case light.闪烁:
                        RedLamp.Value = intervalSign ? true : false;
                        break;
                    case light.常灭:
                        RedLamp.Value = false;
                        break;
                }
                if (!VoiceClosed)
                {
                    switch (ErrSturs.Buzzer)
                    {
                        case Buzzer.长音:
                            Speeker.Value = true;
                            break;
                        case Buzzer.间隔音:
                            Speeker.Value = intervalSign ? true : false;
                            break;
                        case Buzzer.静音:
                            Speeker.Value = false;
                            break;
                    }
                }
                else { Speeker.Value = false; }
            }
            if (Status == MachineStatus.设备停止中 || Status == MachineStatus.设备准备好)
            {
                switch (StopSturs.isGreen)
                {
                    case light.常亮:
                        GreenLamp.Value = true;
                        break;
                    case light.闪烁:
                        GreenLamp.Value = intervalSign ? true : false;
                        break;
                    case light.常灭:
                        GreenLamp.Value = false;
                        break;
                }
                switch (StopSturs.isYellow)
                {
                    case light.常亮:
                        YellowLamp.Value = true;
                        break;
                    case light.闪烁:
                        YellowLamp.Value = intervalSign ? true : false;
                        break;
                    case light.常灭:
                        YellowLamp.Value = false;
                        break;
                }
                switch (StopSturs.isRed)
                {
                    case light.常亮:
                        RedLamp.Value = true;
                        break;
                    case light.闪烁:
                        RedLamp.Value = intervalSign ? true : false;
                        break;
                    case light.常灭:
                        RedLamp.Value = false;
                        break;
                }
                if (!VoiceClosed)
                {
                    switch (StopSturs.Buzzer)
                    {
                        case Buzzer.长音:
                            Speeker.Value = true;
                            break;
                        case Buzzer.间隔音:
                            Speeker.Value = intervalSign ? true : false;
                            break;
                        case Buzzer.静音:
                            Speeker.Value = false;
                            break;
                    }
                }
                else { Speeker.Value = false; }
            }
            if (Status == MachineStatus.设备警示中 || Status == MachineStatus.设备急停已按下 || Status == MachineStatus.设备未准备好)
            {
                switch (WarningSturs.isGreen)
                {
                    case light.常亮:
                        GreenLamp.Value = true;
                        break;
                    case light.闪烁:
                        GreenLamp.Value = intervalSign ? true : false;
                        break;
                    case light.常灭:
                        GreenLamp.Value = false;
                        break;
                }
                switch (WarningSturs.isYellow)
                {
                    case light.常亮:
                        YellowLamp.Value = true;
                        break;
                    case light.闪烁:
                        YellowLamp.Value = intervalSign ? true : false;
                        break;
                    case light.常灭:
                        YellowLamp.Value = false;
                        break;
                }
                switch (WarningSturs.isRed)
                {
                    case light.常亮:
                        RedLamp.Value = true;
                        break;
                    case light.闪烁:
                        RedLamp.Value = intervalSign ? true : false;
                        break;
                    case light.常灭:
                        RedLamp.Value = false;
                        break;
                }
                if (!VoiceClosed)
                {
                    switch (WarningSturs.Buzzer)
                    {
                        case Buzzer.长音:
                            Speeker.Value = true;
                            break;
                        case Buzzer.间隔音:
                            Speeker.Value = intervalSign ? true : false;
                            break;
                        case Buzzer.静音:
                            Speeker.Value = false;
                            break;
                    }
                }
                else { Speeker.Value = false; }

            }


            //红灯
            //if (Status == MachineStatus.设备未准备好 ||
            //    (intervalSign && (Status == MachineStatus.设备报警中 || Status == MachineStatus.设备急停已按下)))
            //{
            //    RedLamp.Value = true;
            //}
            //else
            //{
            //    RedLamp.Value = false;
            //}

            ////蜂鸣器
            //if (RedLamp.Value && !VoiceClosed && Status != MachineStatus.设备未准备好 && Status != MachineStatus.设备复位中)
            //{
            //    Speeker.Value = true;
            //}
            //else
            //{
            //    Speeker.Value = false;
            //}

            ////黄灯
            //if (!RedLamp.Value &&
            //    (((Status == MachineStatus.设备停止中 || Status == MachineStatus.设备暂停中
            //    || Status == MachineStatus.设备复位中) && intervalSign)
            //    || Status == MachineStatus.设备准备好))
            //{
            //    YellowLamp.Value = true;
            //}
            //else
            //{
            //    YellowLamp.Value = false;
            //}

            ////绿灯
            //if (!RedLamp.Value && Status == MachineStatus.设备运行中)
            //{
            //    GreenLamp.Value = true;
            //}
            //else
            //{
            //    GreenLamp.Value = false;
            //}
        }
    }
    /// <summary>
    /// 三色灯
    /// </summary>
    public struct TricolorLamp
    {
        public light isGreen;
        public light isYellow;
        public light isRed;
        public Buzzer Buzzer;
    }
    public enum light : int
    {
        常亮,
        闪烁,
        常灭

    }

    public enum Buzzer : int
    {
        静音,
        长音,
        间隔音

    }
}