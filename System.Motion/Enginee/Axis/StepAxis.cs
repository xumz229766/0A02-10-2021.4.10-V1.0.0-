using System.AdvantechAps;
namespace System.Enginee
{
    /// <summary>
    ///     雷塞
    /// </summary>
    public class StepAxis : ApsAxis
    {
        public StepAxis(ApsController apsController) : base(apsController)
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
                return ApsController.GetCurrentCommandPosition(NoId) * Transmission.PulseEquivalent;
            }
        }
        /// <summary>
        ///     是否原点
        /// </summary>
        public bool IsOrigin
        {
            get { return ApsController.IsOrg(NoId); }
        }

        public override void APS_set_command(int pos)
        {
            //base.APS_set_command(pos);
            ApsController.SetCommandPosition(NoId, pos);
        }
        /// <summary>
        ///     是否到位。
        /// </summary>
        public override bool IsInPosition(double pos)
        {
            bool I = ApsController.IsDown(NoId);
            bool I1 = (CurrentPos - 0.01 < pos && CurrentPos + 0.1 > pos);
            return ApsController.IsDown(NoId) & (CurrentPos - 0.055 < pos && CurrentPos + 0.55 > pos) && (CurrentSpeed == 0);
        }
    }
}