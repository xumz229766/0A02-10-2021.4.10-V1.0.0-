using System.Threading;
using System.AdvantechAps;
namespace System.Enginee
{
    /// <summary>
    ///     伺服马达驱动轴
    /// </summary>
    public class ServoAxis : ApsAxis
    {
        public ServoAxis(ApsController apsController) : base(apsController)
        {
        }
        public override double CurrentPos
        {
            get
            {
                return ApsController.GetCurrentCommandPosition(NoId) * Transmission.PulseEquivalent;
            }
        }
        public override double BackPos
        {
            get
            {
                return ApsController.GetCurrentFeedbackPosition(NoId) * Transmission.PulseEquivalent;
            }
        }
        /// <summary>
        ///     是否原点
        /// </summary>
        public bool IsOrigin
        {
            get { return ApsController.IsOrg(NoId); }
        }
        /// <summary>
        ///     是否报警
        /// </summary>
        public override bool IsAlarmed
        {
            get { return ApsController.IsAlm(NoId); }
        }

        /// <summary>
        ///     是否急停
        /// </summary>
        public override bool IsEmg
        {
            get { return ApsController.IsEmg(NoId); }
        }
        public override void APS_set_command(int pos)
        {
            //base.APS_set_command(pos);
            ApsController.SetCommandPosition(NoId, pos);
            ApsController.SetFeedbackPosition(NoId, pos);
        }
        /// <summary>
        ///     是否到位。
        /// </summary>
        public override bool IsInPosition(double pos)
        {            
            bool i = ApsController.IsDown(NoId);
            bool i1 = (BackPos <= (pos + 0.2) && BackPos >= (pos - 0.2));
            return i & i1 && (CurrentSpeed == 0);        
        }
    }
}