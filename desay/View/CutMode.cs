using System;
using System.Enginee;
using System.Interfaces;
using System.Threading;
using System.Threading.Tasks;
using System.ToolKit;
using System.Windows.Forms;
using System.ToolKit.Helper;

namespace desay
{
    public partial class CutMode : UserControl
    {
        public CutMode()
        {
            InitializeComponent();
        }

        private void CutMode_Load(object sender, EventArgs e)
        {
            init();
        }

        public void init()
        {
            ckbnumPullBack.Checked = Position.Instance.IscAxisPullBack;
            State1.Checked = Position.Instance.Caxis[0].PosTorsion;
            State2.Checked = Position.Instance.Caxis[1].PosTorsion;
            State3.Checked = Position.Instance.Caxis[2].PosTorsion;
            State4.Checked = Position.Instance.Caxis[3].PosTorsion;
            numPullBackPos1.Value = (decimal)Position.Instance.cAxisPullBackPos[0];
            numPullBackPos2.Value = (decimal)Position.Instance.cAxisPullBackPos[1];
            numPullBackPos3.Value = (decimal)Position.Instance.cAxisPullBackPos[2];

            numCaxis1CountSet.Value = Config.Instance.CaxisCountSet[0];
            numCaxis2CountSet.Value = Config.Instance.CaxisCountSet[1];
            numCaxis3CountSet.Value = Config.Instance.CaxisCountSet[2];
            numCaxis4CountSet.Value = Config.Instance.CaxisCountSet[3];
            numCutDelay1.Value = (decimal)Delay.Instance.CutDelay[0] / 1000;
            numCutDelay2.Value = (decimal)Delay.Instance.CutDelay[1] / 1000;
            numCutDelay3.Value = (decimal)Delay.Instance.CutDelay[2] / 1000;
            numCutDelay4.Value = (decimal)Delay.Instance.CutDelay[3] / 1000;

            NumUpTeamp1.Value = (decimal)Config.Instance.TemperatureValue[0];
            NumUpTeamp2.Value = (decimal)Config.Instance.TemperatureValue[1];
            NumUpTeamp3.Value = (decimal)Config.Instance.TemperatureValue[2];
            NumUpTeamp4.Value = (decimal)Config.Instance.TemperatureValue[3];
            NumDownTeamp1.Value = (decimal)Config.Instance.TemperatureValue[4];
            NumDownTeamp2.Value = (decimal)Config.Instance.TemperatureValue[5];
            NumDownTeamp3.Value = (decimal)Config.Instance.TemperatureValue[6];
            NumDownTeamp4.Value = (decimal)Config.Instance.TemperatureValue[7];
            AlarmUpLimit1.Value = (decimal)Config.Instance.TemperatureAlarmLimit[0];           
            AlarmUpLimit2.Value = (decimal)Config.Instance.TemperatureAlarmLimit[1];            
            AlarmUpLimit3.Value = (decimal)Config.Instance.TemperatureAlarmLimit[2];            
            AlarmUpLimit4.Value = (decimal)Config.Instance.TemperatureAlarmLimit[3];
            AlarmDownLimit1.Value = (decimal)Config.Instance.TemperatureAlarmLimit[4];
            AlarmDownLimit2.Value = (decimal)Config.Instance.TemperatureAlarmLimit[5];
            AlarmDownLimit3.Value = (decimal)Config.Instance.TemperatureAlarmLimit[6];
            AlarmDownLimit4.Value = (decimal)Config.Instance.TemperatureAlarmLimit[7];
            TemperatureAlarm1.Checked = Config.Instance.TemperatureAlarmState[0];
            TemperatureAlarm2.Checked = Config.Instance.TemperatureAlarmState[1];
            TemperatureAlarm3.Checked = Config.Instance.TemperatureAlarmState[2];
            TemperatureAlarm4.Checked = Config.Instance.TemperatureAlarmState[3];
            numTeampUpOffset1.Value = (decimal)Config.Instance.TemperatureViewOffset[0] / 10;
            numTeampUpOffset2.Value = (decimal)Config.Instance.TemperatureViewOffset[1] / 10;
            numTeampUpOffset3.Value = (decimal)Config.Instance.TemperatureViewOffset[2] / 10;
            numTeampUpOffset4.Value = (decimal)Config.Instance.TemperatureViewOffset[3] / 10;
            numTeampDownOffset1.Value = (decimal)Config.Instance.TemperatureViewOffset[4] / 10;
            numTeampDownOffset2.Value = (decimal)Config.Instance.TemperatureViewOffset[5] / 10;
            numTeampDownOffset3.Value = (decimal)Config.Instance.TemperatureViewOffset[6] / 10;
            numTeampDownOffset4.Value = (decimal)Config.Instance.TemperatureViewOffset[7] / 10;

            numPressCut1.Value = (decimal)Config.Instance.PressCut[0];
            numPressCut2.Value = (decimal)Config.Instance.PressCut[1];
            numPressCut3.Value = (decimal)Config.Instance.PressCut[2];
            numPressCut4.Value = (decimal)Config.Instance.PressCut[3];
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            Position.Instance.IscAxisPullBack = ckbnumPullBack.Checked;
            Position.Instance.Caxis[0].PosTorsion = State1.Checked;
            Position.Instance.Caxis[1].PosTorsion = State2.Checked;
            Position.Instance.Caxis[2].PosTorsion = State3.Checked;
            Position.Instance.Caxis[3].PosTorsion = State4.Checked;
            Position.Instance.cAxisPullBackPos[0] = Convert.ToDouble(numPullBackPos1.Value);
            Position.Instance.cAxisPullBackPos[1] = Convert.ToDouble(numPullBackPos2.Value);
            Position.Instance.cAxisPullBackPos[2] = Convert.ToDouble(numPullBackPos3.Value);
            Config.Instance.CaxisCountSet[0] = Convert.ToInt32(numCaxis1CountSet.Value);
            Config.Instance.CaxisCountSet[1] = Convert.ToInt32(numCaxis2CountSet.Value);
            Config.Instance.CaxisCountSet[2] = Convert.ToInt32(numCaxis3CountSet.Value);
            Config.Instance.CaxisCountSet[3] = Convert.ToInt32(numCaxis4CountSet.Value);
            Delay.Instance.CutDelay[0] = Convert.ToInt32(Convert.ToDouble(numCutDelay1.Value) * 1000);
            Delay.Instance.CutDelay[1] = Convert.ToInt32(Convert.ToDouble(numCutDelay2.Value) * 1000);
            Delay.Instance.CutDelay[2] = Convert.ToInt32(Convert.ToDouble(numCutDelay3.Value) * 1000);
            Delay.Instance.CutDelay[3] = Convert.ToInt32(Convert.ToDouble(numCutDelay4.Value) * 1000);
            Config.Instance.PressCut[0] = Convert.ToInt32(Convert.ToDouble(numPressCut1.Value));
            Config.Instance.PressCut[1] = Convert.ToInt32(Convert.ToDouble(numPressCut2.Value));
            Config.Instance.PressCut[2] = Convert.ToInt32(Convert.ToDouble(numPressCut3.Value));
            Config.Instance.PressCut[3] = Convert.ToInt32(Convert.ToDouble(numPressCut4.Value));
            Config.Instance.TemperatureValue[0] = Convert.ToInt32(Convert.ToDouble(NumUpTeamp1.Value));
            Config.Instance.TemperatureValue[1] = Convert.ToInt32(Convert.ToDouble(NumUpTeamp2.Value));
            Config.Instance.TemperatureValue[2] = Convert.ToInt32(Convert.ToDouble(NumUpTeamp3.Value));
            Config.Instance.TemperatureValue[3] = Convert.ToInt32(Convert.ToDouble(NumUpTeamp4.Value));
            Config.Instance.TemperatureValue[4] = Convert.ToInt32(Convert.ToDouble(NumDownTeamp1.Value));
            Config.Instance.TemperatureValue[5] = Convert.ToInt32(Convert.ToDouble(NumDownTeamp2.Value));
            Config.Instance.TemperatureValue[6] = Convert.ToInt32(Convert.ToDouble(NumDownTeamp3.Value));
            Config.Instance.TemperatureValue[7] = Convert.ToInt32(Convert.ToDouble(NumDownTeamp4.Value));
            Config.Instance.TemperatureAlarmLimit[0] = Convert.ToInt32(Convert.ToDouble(AlarmUpLimit1.Value));
            Config.Instance.TemperatureAlarmLimit[1] = Convert.ToInt32(Convert.ToDouble(AlarmUpLimit2.Value));           
            Config.Instance.TemperatureAlarmLimit[2] = Convert.ToInt32(Convert.ToDouble(AlarmUpLimit3.Value));           
            Config.Instance.TemperatureAlarmLimit[3] = Convert.ToInt32(Convert.ToDouble(AlarmUpLimit4.Value));
            Config.Instance.TemperatureAlarmLimit[4] = Convert.ToInt32(Convert.ToDouble(AlarmDownLimit1.Value));
            Config.Instance.TemperatureAlarmLimit[5] = Convert.ToInt32(Convert.ToDouble(AlarmDownLimit2.Value));
            Config.Instance.TemperatureAlarmLimit[6] = Convert.ToInt32(Convert.ToDouble(AlarmDownLimit3.Value));
            Config.Instance.TemperatureAlarmLimit[7] = Convert.ToInt32(Convert.ToDouble(AlarmDownLimit4.Value));
            Config.Instance.TemperatureAlarmState[0] = TemperatureAlarm1.Checked;
            Config.Instance.TemperatureAlarmState[1] = TemperatureAlarm2.Checked;
            Config.Instance.TemperatureAlarmState[2] = TemperatureAlarm3.Checked;
            Config.Instance.TemperatureAlarmState[3] = TemperatureAlarm4.Checked;
            Config.Instance.TemperatureViewOffset[0] = Convert.ToInt32(Convert.ToDouble(numTeampUpOffset1.Value) * 10);
            Config.Instance.TemperatureViewOffset[1] = Convert.ToInt32(Convert.ToDouble(numTeampUpOffset2.Value) * 10);
            Config.Instance.TemperatureViewOffset[2] = Convert.ToInt32(Convert.ToDouble(numTeampUpOffset3.Value) * 10);
            Config.Instance.TemperatureViewOffset[3] = Convert.ToInt32(Convert.ToDouble(numTeampUpOffset4.Value) * 10);
            Config.Instance.TemperatureViewOffset[4] = Convert.ToInt32(Convert.ToDouble(numTeampDownOffset1.Value) * 10);
            Config.Instance.TemperatureViewOffset[5] = Convert.ToInt32(Convert.ToDouble(numTeampDownOffset2.Value) * 10);
            Config.Instance.TemperatureViewOffset[6] = Convert.ToInt32(Convert.ToDouble(numTeampDownOffset3.Value) * 10);
            Config.Instance.TemperatureViewOffset[7] = Convert.ToInt32(Convert.ToDouble(numTeampDownOffset4.Value) * 10);
            Global.WritePulse = true;

            Marking.ModifyParameterMarker = true;
        }
    }
}
