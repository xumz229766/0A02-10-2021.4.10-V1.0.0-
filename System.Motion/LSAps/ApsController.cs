using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Interfaces;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace Motion.LSAps
{
    /// <summary>
    ///     雷塞EtherCAT总线。修改于2019.6.13 aiwen
    /// </summary>
    public class ApsController : Automatic, ISwitchController, IMotionController, INeedInitialization, IDisposable
    {
        /// <summary>
        /// 轴号
        /// </summary>
        private readonly Dictionary<int, LSCard> m_Axises = new Dictionary<int, LSCard>();
        /// <summary>
        /// 卡号
        /// </summary>
        private readonly List<int> m_Devices;


        private bool _disposed;

       

        public bool IsLoadXmlFile { get; private set; }

        public ApsController()
        {
            m_Devices = new List<int>();
            m_Axises = new Dictionary<int, LSCard>();
        }

        #region Implementation of INeedInitialization

        /// <summary>
        /// 板卡加载初始化
        /// </summary>
        public void Initialize()
        {
             InitializeCard();
        }
        /// <summary>
        /// 下载参数文件
        /// </summary>
        /// <param name="step">卡位置</param>
        /// <param name="xmlfilename">文件路径</param>
        /// <returns></returns>
        public bool LoadParamFromFile(string CardEniName, string AxisIniName)
        {
            for (ushort i = 0; i < m_Devices.Count; i++)
            {
                byte[] buffer = Encoding.UTF8.GetBytes(CardEniName);
                byte[] fileincontrol = Encoding.UTF8.GetBytes("");
                ushort filetype = 200;
                short eniret = LTDMC.dmc_download_memfile(i, buffer, (uint)buffer.Length, fileincontrol, filetype);
                if (eniret != 0)
                {
                    throw new ApsException(string.Format("运动控制卡总线配置错误:{0}", ErrMessage(eniret)));
                }
                short iniret = LTDMC.dmc_download_configfile(i, AxisIniName);
                if (iniret != 0)
                {
                    throw new ApsException(string.Format("运动控制卡轴配置错误:{0}", ErrMessage(iniret)));
                }
            }
            return true;
        }

        #endregion

        #region 错误代码
        private string ErrMessage(short ret)
        {
            string str = "";
            switch (ret)
            {
                case 0:
                    str = "成功";
                    break;
                case 1:
                    str = "未知错误";
                    break;
                case 2:
                    str = "参数错误";
                    break;
                case 3:
                    str = "操作超时";
                    break;
                case 4:
                    str = "控制卡状态忙";
                    break;
                case 6:
                    str = "连续插补错误";
                    break;
                case 8:
                    str = "无法连接错误";
                    break;
                case 9:
                    str = "卡号错误";
                    break;
                case 10:
                    str = "数据传输错误，PCI插槽可能松动";
                    break;
                case 12:
                    str = "固件文件错误";
                    break;
                case 14:
                    str = "固件文件不匹配";
                    break;
                case 20:
                    str = "固件参数错误";
                    break;
                case 22:
                    str = "固件当前状态不允许操作";
                    break;
                case 24:
                    str = "不支持功能";
                    break;
            }
            return str;
        }

        #endregion


        #region 获取当前轴的 IO 信号
        /// <summary>
        ///     是否报警
        /// </summary>
        /// <returns></returns>
        public bool IsAlm(int axisNo)
        {
            return (((int)AxisStatus(axisNo) >> (int)0) & 1) == 1;
        }
        /// <summary>
        ///     是否到达正限位
        /// </summary>
        /// <returns></returns>
        public bool IsPel(int axisNo)
        {
            return (((int)AxisStatus(axisNo) >> (int)1) & 1) == 1;
        }
        /// <summary>
        ///     是否到达正负位
        /// </summary>
        /// <returns></returns>
        public bool IsMel(int axisNo)
        {
            return (((int)AxisStatus(axisNo) >> (int)2) & 1) == 1;
        }
        /// <summary>
        ///     是否在轴原点
        /// </summary>
        /// <param name="axisNo"></param>
        /// <returns></returns>
        public bool IsOrg(int axisNo)
        {
            return (((int)AxisStatus(axisNo) >> (int)4) & 1) == 1;
        }
        /// <summary>
        ///     是否急停
        /// </summary>
        /// <returns></returns>
        public bool IsEmg(int axisNo)
        {
            return (((int)AxisStatus(axisNo) >> (int)3) & 1) == 1;
        }
        /// <summary>
        /// 是否在轴Z相
        /// </summary>
        /// <param name="axisNo"></param>
        /// <returns></returns>
        public bool IsSZ(int axisNo)
        {
            return (((int)AxisStatus(axisNo) >> (int)9) & 1) == 1;
        }
        /// <summary>
        /// 是否在轴Z相
        /// </summary>
        /// <param name="axisNo"></param>
        /// <returns></returns>
        public bool IsINP(int axisNo)
        {
            return (((int)AxisStatus(axisNo) >> (int)8) & 1) == 1;
        }
        /// <summary>
        ///     急停
        /// </summary>
        /// <returns></returns>
        public void Emg_stop()
        {
            short ret = 0;
            foreach (ushort a in m_Devices)
            { ret = LTDMC.dmc_emg_stop(a); }
        }
        /// <summary>
        /// 减速停止指定轴
        /// </summary>
        /// <param name="axisNo"></param>
        public bool Stop(int axisNo)
        {
            short ret = 0;
            ret = LTDMC.dmc_stop(m_Axises[axisNo].Card, m_Axises[axisNo].Axis, 0);
            return true;
        }
        /// <summary>
        ///    Erc端口状态
        /// </summary>
        /// <returns></returns>
        public bool IsErc(int axisNo)
        {
            short ret = 0;
            ret = LTDMC.dmc_read_erc_pin(m_Axises[axisNo].Card, m_Axises[axisNo].Axis);
            return ret == 1 ? true : false;
        }
        /// <summary>
        /// 返回轴的运动信号的状态
        /// </summary>
        /// <param name="axisNo"></param>
        /// <returns></returns>
        public double AxisStatus(int axisNo)
        {
            double ret = 0;
            ret = LTDMC.dmc_axis_io_status(m_Axises[axisNo].Card, m_Axises[axisNo].Axis);
            return ret;
        }

        /// <summary>
        ///  轴是否准备好
        /// </summary>
        /// <returns></returns>
        public bool IsReady(int axisNo)
        {
            short ret = 0;
            ret = LTDMC.dmc_read_rdy_pin(m_Axises[axisNo].Card, m_Axises[axisNo].Axis);
            return ret == 1 ? true : false;
        }

        /// <summary>
        /// 轴是否停止运动
        /// </summary>
        /// <param name="axisNo"></param>
        /// <returns></returns>
        public bool IsAxisStop(int axisNo)
        {
            short ret = 0;
            ret = LTDMC.dmc_check_done(m_Axises[axisNo].Card, m_Axises[axisNo].Axis);
            return ret == 1 ? true : false;
        }
        /// <summary>
        ///     获取电机励磁状态。
        /// </summary>
        /// <param name="axisNo"></param>
        /// <returns></returns>
        public bool GetServo(int axisNo)
        {
            ushort AxisServo = 0;
            var ret = LTDMC.nmc_get_axis_state_machine(m_Axises[axisNo].Card, m_Axises[axisNo].Axis,ref AxisServo);
            return AxisServo == 4 ? true : false;
        }
        /// <summary>
        ///     设置电机励磁状态。
        /// </summary>
        /// <param name="axisNo"></param>
        /// <returns></returns>
        public bool ServoOn(int axisNo)
        {
            short ret = 0;
            ret = LTDMC.nmc_set_axis_enable(m_Axises[axisNo].Card, m_Axises[axisNo].Axis);
            return ret == 0 ? true : false;
        }
        /// <summary>
        ///     轴掉电
        /// </summary>
        /// <param name="noId"></param>
        public bool ServoOff(int axisNo)
        {
            short ret = 0;
            ret = LTDMC.nmc_set_axis_disable(m_Axises[axisNo].Card, m_Axises[axisNo].Axis);
            return ret == 0 ? true : false;
        }
        /// <summary>
        ///     设置电机励磁状态。
        /// </summary>
        /// <param name="axisNo"></param>
        /// <param name="isOn"></param>
        public bool SetServo(int axisNo, bool isOn)
        {
            short ret = 0;
            if (isOn)
            {
                ret = LTDMC.nmc_set_axis_enable(m_Axises[axisNo].Card, m_Axises[axisNo].Axis);
            }
            else
            {
                ret = LTDMC.nmc_set_axis_disable(m_Axises[axisNo].Card, m_Axises[axisNo].Axis);
            }
            return ret == 0 ? true : false;

        }

        #endregion
        /// <summary>
        ///     获取轴当前位置
        /// </summary>
        /// <param name="axisNo">轴标识</param>
        /// <returns>当前位置</returns>
        public double GetCurrentCommandPosition(int axisNo)
        {
            double pos = 0;
            var ret = LTDMC.dmc_get_position_unit(m_Axises[axisNo].Card, m_Axises[axisNo].Axis,ref pos);
            return pos;
        }


        /// <summary>
        /// 获取编码器计数
        /// </summary>
        /// <param name="axisNo"></param>
        /// <returns></returns>
        public double GetCurrentFeedbackPosition(int axisNo)
        {
            double pos = 0;
            var ret = LTDMC.dmc_get_encoder_unit(m_Axises[axisNo].Card, m_Axises[axisNo].Axis,ref pos);
            return pos;
        }
        /// <summary>
        /// 获取当前速度
        /// </summary>
        /// <param name="axisNo"></param>
        /// <returns></returns>
        public double GetCurrentCommandSpeed(int axisNo)
        {
            double speed = 0;
            var ret = LTDMC.dmc_read_current_speed_unit(m_Axises[axisNo].Card, m_Axises[axisNo].Axis, ref speed);
            return speed;
        }

        /// <summary>
        ///     设置指令位置计数器计数值
        /// </summary>
        /// <param name="axisNo"></param>
        /// <param name="position"></param>
        public bool SetCommandPosition(int axisNo, int position)
        {

            int ret = LTDMC.dmc_set_position_unit(m_Axises[axisNo].Card, m_Axises[axisNo].Axis, position);
            return true;

        }
        /// <summary>
        ///     设置编码器计数值
        /// </summary>
        /// <param name="axisNo"></param>
        /// <param name="position"></param>
        public bool SetFeedbackPosition(int axisNo, int position)
        {
            int ret = LTDMC.dmc_set_encoder_unit(m_Axises[axisNo].Card, m_Axises[axisNo].Axis, position);
            return true;
        }

        #region 运动控制

        #endregion


        /// <summary>
        ///     单轴相对运动
        /// </summary>
        /// <param name="axisNo"></param>
        /// <param name="pulseNum">位置</param>
        /// <param name="velocityCurveParams"></param>
        /// <returns></returns>
        public void MoveRelPulse(int axisNo, int pulseNum, VelocityCurve velocityCurveParams)
        {

            //设置速度
            SetAxisVelocity(axisNo, velocityCurveParams);
            LTDMC.dmc_pmove_unit(m_Axises[axisNo].Card, m_Axises[axisNo].Axis, pulseNum, 0);

        }
        /// <summary>
        /// 在线变速
        /// </summary>
        /// <param name="axisNo"></param>
        /// <param name="velocityCurveParams"></param>
        public void ChangeSpeed(int axisNo, int pulseNum, VelocityCurve velocityCurveParams)
        {

            LTDMC.dmc_change_speed_unit(m_Axises[axisNo].Card, m_Axises[axisNo].Axis, velocityCurveParams.Maxvel, velocityCurveParams.Tacc);

        }
        /// <summary>
        /// 在线变位置
        /// </summary>
        /// <param name="axisNo"></param>
        /// <param name="pulseNum"></param>
        /// <param name="velocityCurveParams"></param>
        public void ChangePosition(int axisNo, int pulseNum, VelocityCurve velocityCurveParams)
        {

            LTDMC.dmc_reset_target_position_unit(m_Axises[axisNo].Card, m_Axises[axisNo].Axis, pulseNum);

        }

        /// <summary>
        ///     单轴绝对运动
        /// </summary>
        /// <param name="axisNo"></param>
        /// <param name="pulseNum"></param>
        /// <param name="velocityCurveParams"></param>
        public void MoveAbsPulse(int axisNo, int pulseNum, VelocityCurve velocityCurveParams)
        {
            //设置速度
            SetAxisVelocity(axisNo, velocityCurveParams);
            LTDMC.dmc_pmove_unit(m_Axises[axisNo].Card, m_Axises[axisNo].Axis, pulseNum, 1);

        }

        /// <summary>
        ///     连续运动
        /// </summary>
        /// <param name="axisNo"></param>
        /// <param name="moveDirection"></param>
        /// <param name="velocityCurveParams"></param>
        /// <returns></returns>
        public void ContinuousMove(int axisNo, MoveDirection moveDirection, VelocityCurve velocityCurveParams)
        {
            //设置速度
            SetAxisVelocity(axisNo, velocityCurveParams);
            ushort dir = 1;
            if (moveDirection == MoveDirection.Postive) { dir = 1; } else { dir = 0; }
            LTDMC.dmc_vmove(m_Axises[axisNo].Card, m_Axises[axisNo].Axis, dir);

        }
        /// <summary>
        ///     立即停止
        /// </summary>
        /// <param name="axisNo"></param>
        public bool ImmediateStop(int axisNo)
        {
            short ret = 0;
            ret = LTDMC.dmc_stop(m_Axises[axisNo].Card, m_Axises[axisNo].Axis, 1);
            return true;


        }

        /// <summary>
        ///     减速停止指定机构轴脉冲输出
        /// </summary>
        /// <param name="axisNo"></param>
        public bool DecelStop(int axisNo)
        {
            short ret = 0;

            ret = LTDMC.dmc_stop(m_Axises[axisNo].Card, m_Axises[axisNo].Axis, 0);
            return true;

        }

        /// <summary>
        ///     是否停止移动
        /// </summary>
        /// <returns></returns>
        public bool IsDown(int axisNo, bool hasExtEncode = false)
        {

            return IsAxisStop(axisNo) && GetCurrentCommandSpeed(axisNo) == 0;
        }



        /// <summary>
        /// 两轴做插补相对移动
        /// </summary>
        /// <param name="axisNo1">轴1ID</param>
        /// <param name="axisNo2">轴2ID</param>
        /// <param name="pulseNum1">坐标1</param>
        /// <param name="pulseNum2">坐标2</param>
        /// <param name="velocityCurveParams">速度参数</param>
        public void MoveLine2Relative(int axisNo1, int axisNo2, int pulseNum1, int pulseNum2,
            VelocityCurve velocityCurveParams)
        {

        }
        /// <summary>
        ///     两轴直线插补绝对移动
        /// </summary>
        /// <param name="axisNo1">轴1ID</param>
        /// <param name="axisNo2">轴2ID</param>
        /// <param name="pulseNum1">坐标1</param>
        /// <param name="pulseNum2">坐标2</param>
        /// <param name="velocityCurveParams">速度参数</param>
        public void MoveLine2Absolute(int axisNo1, int axisNo2, int pulseNum1, int pulseNum2,
            VelocityCurve velocityCurveParams)
        {

        }
        /// <summary>
        /// 两轴直线插补轨迹移动
        /// </summary>
        /// <param name="axisNo1">轴1ID</param>
        /// <param name="axisNo2">轴2ID</param>
        /// <param name="pulseNum1">坐标1</param>
        /// <param name="pulseNum2">坐标2</param>
        /// <param name="velocityCurveParams">速度参数</param>
        /// <param name="Option">位集指定选项，该选项可以启用指定的参数和函数。</param>
        public void MoveLine2(int axisNo1, int axisNo2, double pulseNum1, double pulseNum2,
            VelocityCurve velocityCurveParams, int Option)
        {
        }
        /// <summary>
        /// 两轴圆弧插补相对移动
        /// </summary>
        /// <param name="axisNo1">轴1ID</param>
        /// <param name="axisNo2">轴2ID</param>
        /// <param name="centreNum1">圆心1</param>
        /// <param name="centreNum2">圆心2</param>
        /// <param name="Angle">角度</param>
        /// <param name="velocityCurveParams">速度参数</param>
        public void MoveArc2Relative(int axisNo1, int axisNo2, int centreNum1, int centreNum2, int Angle, VelocityCurve velocityCurveParams)
        {
        }

        /// <summary>
        /// 两轴圆弧插补绝对移动
        /// </summary>
        /// <param name="axisNo1">轴1ID</param>
        /// <param name="axisNo2">轴2ID</param>
        /// <param name="centreNum1">圆心1</param>
        /// <param name="centreNum2">圆心2</param>
        /// <param name="Angle">角度</param>
        /// <param name="velocityCurveParams">速度参数</param>
        public void MoveArc2Absolute(int axisNo1, int axisNo2, int centreNum1, int centreNum2, int Angle, VelocityCurve velocityCurveParams)
        {
        }
        /// <summary>
        /// 两轴圆弧插补轨迹移动
        /// </summary>
        /// <param name="axisNo1">轴1ID</param>
        /// <param name="axisNo2">轴2ID</param>
        /// <param name="centreNum1">圆心1</param>
        /// <param name="centreNum2">圆心2</param>
        /// <param name="endNum1">终点1</param>
        /// <param name="endNum2">终点1</param>
        /// <param name="dir">方向</param>
        /// <param name="velocityCurveParams">速度参数</param>
        /// <param name="Option">位集指定选项，该选项可以启用指定的参数和函数。</param>
        public void MoveArc2(int axisNo1, int axisNo2, double centreNum1, double centreNum2, double endNum1, double endNum2, short dir, VelocityCurve velocityCurveParams, int Option)
        {
        }
        /// <summary>
        ///     回零
        /// </summary>
        /// <param name="axisNo"></param>
        public void BackHome(int axisNo,uint HomeMode,uint HomeDir, VelocityCurve HomeVelocityCurve)
        {
            LTDMC.nmc_set_home_profile(m_Axises[axisNo].Card, m_Axises[axisNo].Axis,1, HomeVelocityCurve.Strvel, HomeVelocityCurve.Maxvel, HomeVelocityCurve.Tacc, HomeVelocityCurve.Tdec,0);
            LTDMC.dmc_home_move(m_Axises[axisNo].Card, m_Axises[axisNo].Axis);
        }

        /// <summary>
        ///     检查回零是否完成
        /// </summary>
        /// <param name="axisNo"></param>
        /// <returns></returns>
        public int CheckHomeDone(int axisNo, double timeoutLimit)
        {
            var strtime = new Stopwatch();
            strtime.Start();
            do
            {
                if (IsAlm(axisNo)) { return -1; }
                if (IsOrg(axisNo) && IsAxisStop(axisNo)) { return 0; }
                //检查是否超时
                strtime.Stop();
                if (strtime.ElapsedMilliseconds / 1000.0 > timeoutLimit)
                {
                    Stop(axisNo);
                    return -2;
                }
                strtime.Start();

                //延时
                Thread.Sleep(20);
            } while (true);
        }

        /// <summary>
        ///     轴卡电平信号配置(板卡初始化后必须配置)
        /// </summary>
        /// <param name="axisNo"></param>
        /// <param name="axisSignalParams"></param>
        public void SetAxisSignalConfig(int axisNo, AxisSignalParams axisSignalParams)
        {

        }

        /// <summary>
        ///     回零配置
        /// </summary>
        /// <param name="axisNo"></param>
        /// <param name="homeConfigParams"></param>
        public void SetAxisHomeConfig(int axisNo, HomeParams homeConfigParams)
        {

        }

        /// <summary>
        ///     清除限位配置
        /// </summary>
        /// <param name="axisNo"></param>
        public void ClearSoftConfig(int axisNo)
        {

        }

        /// <summary>
        ///     限位配置
        /// </summary>
        /// <param name="axisNo"></param>
        /// <param name="softLimitParams"></param>
        public void SetSoftELConfig(int axisNo, SoftLimitParams softLimitParams)
        {

        }

        private void SetAxisVelocity(int axisNo, VelocityCurve velocityCurveParams)
        {
            LTDMC.dmc_set_profile_unit(m_Axises[axisNo].Card, m_Axises[axisNo].Axis, velocityCurveParams.Strvel, velocityCurveParams.Maxvel, velocityCurveParams.Tacc, velocityCurveParams.Tdec, 1);//设置速度参数

            LTDMC.dmc_set_s_profile(m_Axises[axisNo].Card, m_Axises[axisNo].Axis, 0, velocityCurveParams.dS_para);//设置S段速度参数

            LTDMC.dmc_set_dec_stop_time(m_Axises[axisNo].Card, m_Axises[axisNo].Axis, velocityCurveParams.dTdec); //设置减速停止时间

        }

        /// <summary>
        ///     专用输入口使能(不是所有板卡都有此功能)
        /// </summary>
        /// <param name="cardNo"></param>
        /// <param name="inputEn"></param>
        /// <returns></returns>
        public void InputEnable(int cardNo, int inputEn)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///     配置限位停止方式
        /// </summary>
        /// <param name="axisNo"></param>
        /// <param name="ELStopType"></param>
        public void ConfigELStopType(int axisNo, ELStopType ELStopType)
        {
            throw new NotImplementedException();
        }


        private bool InitializeCard()
        {
            //var ret = LTDMC.dmc_board_init();//获取卡数量
            //if (ret <= 0 || ret > 8)
            //{
            //    return false;
            //    //MessageBox.Show("初始卡失败!", "出错");
            //}
            //else { return true; }

            uint AxisTotalNumer = 0;
            var cards = LTDMC.dmc_board_init();
            int I2 = 0;
            LSCard lS = new LSCard();
            if (cards > 0)
            {
                for (ushort i = 0; i < cards; i++)
                {
                    var Axis = LTDMC.nmc_get_total_axes(i, ref AxisTotalNumer);
                    if (Axis == 0)
                    {
                        for (ushort i1 = 0; i1 < AxisTotalNumer; i1++)
                        {
                            lS.Card = i;
                            lS.Axis = i1;
                            I2++;
                            m_Axises.Add(I2, lS);
                        }
                    }
                    else { throw new ApsException(string.Format("运动控制卡功错误:{0}", ErrMessage(Axis))); }
                    m_Devices.Add(i);
                }
            }
            else
            {
                throw new ApsException(string.Format("未找到控制卡"));
            }

            return true;
        }

        #region  CAN连接函数
        /// <summary>
        /// 设置Can连接状态
        /// </summary>
        /// <param name="CardNo">板卡号</param>
        /// <param name="NoteName">CAN节点数</param>
        /// <param name="status">通讯状态，true连接false断开</param>
        /// <param name="Baud">波特率</param>
        /// <returns></returns>
        public bool setCanConnet(ushort CardNo, ushort NoteName, bool status, int Baud)
        {
            ushort val = 0;
            if (status) { val = 1; } else { val = 0; }
            LTDMC.nmc_set_connect_state(CardNo, NoteName, val, (ushort)Baud);
            return true;
        }
        /// <summary>
        /// 获取Can连接状态
        /// </summary>
        /// <param name="CardNo"></param>
        /// <param name="NoteName"></param>
        /// <param name="status"></param>      
        /// <returns></returns>
        public bool getCanConnet(ushort CardNo, ref ushort NoteName)
        {
            ushort val = 0;
            LTDMC.nmc_get_connect_state(CardNo, ref NoteName, ref val);
            return val == 1 ? true : false;
        }
        /// <summary>
        /// 设置CANIO的某个输出端口
        /// </summary>
        /// <param name="CardNo"></param>
        /// <param name="Node"></param>
        /// <param name="BitNo"></param>
        /// <param name="on_off"></param>
        /// <returns></returns>
        public bool WriteCanoutIO(ushort CardNo, ushort Node, ushort BitNo, bool on_off)
        {
            ushort var = 0;
            if (on_off) { var = 1; } else { var = 0; }
            LTDMC.nmc_write_outbit(CardNo, Node, BitNo, var);
            return false;
        }
        /// <summary>
        /// 读取CANIO的某个输出端口
        /// </summary>
        /// <param name="CardNo"></param>
        /// <param name="Node"></param>
        /// <param name="BitNo"></param>
        /// <returns></returns>
        public bool ReadCanoutIO(ushort CardNo, ushort Node, ushort BitNo)
        {
            ushort var = 0;
            LTDMC.nmc_read_outbit(CardNo, Node, BitNo, ref var);
            return var == 1 ? true : false;
        }
        /// <summary>
        /// 读取CANIO的某个输入端口
        /// </summary>
        /// <param name="CardNo"></param>
        /// <param name="Node"></param>
        /// <param name="BitNo"></param>
        /// <returns></returns>
        public bool ReadCanintIO(ushort CardNo, ushort Node, ushort BitNo)
        {
            ushort var = 0;
            LTDMC.nmc_read_inbit(CardNo, Node, BitNo, ref var);
            return var == 1 ? true : false;
        }
        /// <summary>
        /// 批量读取CANIO的输出端口
        /// </summary>
        /// <param name="CardNo"></param>
        /// <param name="Node"></param>
        /// <param name="PortNo">端口号 0表示0~31 1表示32~63</param>
        /// <param name="value">bit0~bit31</param>
        /// <returns></returns>
        public bool WriteCanOutPort(ushort CardNo, ushort Node, ushort PortNo, uint value)
        {
            short var = LTDMC.nmc_write_outport(CardNo, Node, PortNo, value);
            return var == 0 ? true : false;
        }
        /// <summary>
        /// 批量写入CanIO的输出端口
        /// </summary>
        /// <param name="CardNo"></param>
        /// <param name="Node"></param>
        /// <param name="PortNo"></param>
        /// <returns></returns>
        public uint ReadCanOutPort(ushort CardNo, ushort Node, ushort PortNo)
        {
            uint value = 0;
            var var = LTDMC.nmc_read_outport(CardNo, Node, PortNo, ref value);
            return value;
        }
        /// <summary>
        /// 批量写入CanIO的输入端口
        /// </summary>
        /// <param name="CardNo"></param>
        /// <param name="Node"></param>
        /// <param name="PortNo"></param>
        /// <returns></returns>
        public uint ReadCanInPort(ushort CardNo, ushort Node, ushort PortNo)
        {
            uint value = 0;
            var var = LTDMC.nmc_read_inport(CardNo, Node, PortNo, ref value);
            return value;
        }

        /// <summary>
        /// 模拟量输出设定
        /// </summary>
        /// <param name="CardNo"></param>
        /// <param name="Node"></param>
        /// <param name="Channel">通道</param>
        /// <param name="value">单位mV/mA</param>
        /// <returns></returns>
        public bool WriteCanADDAOut(ushort CardNo, ushort Node, ushort Channel, double value)
        {
            short var = 0;
            var = LTDMC.nmc_set_da_output(CardNo, Node, Channel, value);
            return var == 0 ? true : false;
        }
        /// <summary>
        /// 模拟量输出读取
        /// </summary>
        /// <param name="CardNo"></param>
        /// <param name="Node"></param>
        /// <param name="Channel">通道</param>
        /// <param name="value">单位mV/mA</param>
        /// <returns></returns>
        public double ReadCanADDAOut(ushort CardNo, ushort Node, ushort Channel)
        {
            double value = 0;
            var var = LTDMC.nmc_get_da_output(CardNo, Node, Channel, ref value);
            return value;
        }
        /// <summary>
        /// 模拟量输入读取
        /// </summary>
        /// <param name="CardNo"></param>
        /// <param name="Node"></param>
        /// <param name="Channel">通道</param>
        /// <param name="value">单位mV/mA</param>
        /// <returns></returns>
        public double ReadCanADDAInt(ushort CardNo, ushort Node, ushort Channel)
        {
            double value = 0;
            var var = LTDMC.nmc_get_ad_input(CardNo, Node, Channel, ref value);
            return value;
        }
        #endregion

        #region Implementation of IDisposable

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        ///     线性差补方式运动。
        /// </summary>
        /// <param name="axisNo1"></param>
        /// <param name="axisNo2"></param>
        /// <param name="pulseNum1"></param>
        /// <param name="pulseNum2"></param>
        /// <param name="velocityCurve"></param>
        public void MoveLine(int axisNo1, int axisNo2, int pulseNum1, int pulseNum2, VelocityCurve velocityCurve)
        {
            throw new NotImplementedException();
        }

        ~ApsController()
        {
            Dispose(false);
        }

        /// <summary>
        ///     Dispose
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;
            if (disposing)
            {
            }
            try
            {
                //APS168.APS_close();
            }
            catch (Exception)
            {
                //ignorl
            }          
            _disposed = true;
        }

        #endregion

        #region Implementation of ISwitchController

        public bool Read(IoPoint ioPoint)
        {
            short value = 0;
            bool var = false;
            if (ioPoint.CanNode == 0)
            {
                if ((ioPoint.IoMode & IoModes.Responser) != 0)
                {
                    value = LTDMC.dmc_read_outbit((ushort)ioPoint.BoardNo, (ushort)ioPoint.PortNo);
                }
                else if ((ioPoint.IoMode & IoModes.Senser) != 0)
                {
                    value = LTDMC.dmc_read_inbit((ushort)ioPoint.BoardNo, (ushort)ioPoint.PortNo);
                }
            }
            else
            {
                if ((ioPoint.IoMode & IoModes.Responser) != 0)
                {
                    var = ReadCanoutIO((ushort)ioPoint.BoardNo, (ushort)ioPoint.PortNo, ioPoint.CanNode);
                }
                else if ((ioPoint.IoMode & IoModes.Senser) != 0)
                {
                    var = ReadCanintIO((ushort)ioPoint.BoardNo, (ushort)ioPoint.PortNo, ioPoint.CanNode);
                }
                if (var) { value = 1; } else { value = 0; }
            }

            return value == 1 ? true : false;
        }


        public void Write(IoPoint ioPoint, bool value)
        {
            ushort value1 = 0;
            if (value) { value1 = 1; } else { value1 = 0; }
            if (ioPoint.CanNode == 0)
            {
                var ret = LTDMC.dmc_write_outbit((ushort)ioPoint.BoardNo, (ushort)ioPoint.PortNo, value1);
                if (ret != 0)
                {
                    throw new ApsException(string.Format("运动控制卡错误:{0}", ErrMessage(ret)));
                }
            }
            else
            {
                var ret = WriteCanoutIO((ushort)ioPoint.BoardNo, (ushort)ioPoint.PortNo, ioPoint.CanNode, value);
            }

        }

        #endregion

        /// <summary>
        /// 设置软限位
        /// </summary>
        /// <param name="axisNo"></param>
        /// <param name="nlimit">负限位位置</param>
        /// <param name="plimit">正限位位置</param>
        /// <returns></returns>
        public bool SetSoftLimit(int axisNo,ref int nlimit, ref int plimit)
        {
            short var = 0;
            var = LTDMC.dmc_set_softlimit(m_Axises[axisNo].Card, m_Axises[axisNo].Axis, 1, 1, 1, nlimit, plimit);
            return var == 0 ? true : false;
        }

        /// <summary>
        /// 读取软限位设置
        /// </summary>
        /// <param name="axisNo"></param>
        /// <param name="nlimit">返回负限位脉冲数</param>
        /// <param name="plimit">返回正限位脉冲数</param>
        /// <returns></returns>
        public bool GetSoftLimit(int axisNo,ref int nlimit, ref int plimit)
        {
            ushort enable = 0;
            ushort sourcesel = 0;
            ushort slaction = 0;
            var var = LTDMC.dmc_get_softlimit(m_Axises[axisNo].Card, m_Axises[axisNo].Axis,ref enable, ref sourcesel, ref slaction, ref nlimit, ref plimit);
            return var == 0 ? true : false;
        }

        #region 扭力控制(总线）

        /// <summary>
        /// 读取伺服驱动器参数
        /// </summary>
        /// <param name="axisNo"></param>
        /// <param name="index">伺服参数目录索引</param>
        /// <param name="subindex">伺服参数目录索引下编号</param>
        /// <returns></returns>
        public uint ReadServoParameter(int axisNo, ushort index, ushort subindex)
        {
            uint paradata = 0;
            var var = LTDMC.nmc_read_parameter(m_Axises[axisNo].Card, m_Axises[axisNo].Axis, index, subindex, ref paradata);
            return paradata;
        }

        /// <summary>
        /// 读取伺服驱动器参数
        /// </summary>
        /// <param name="axisNo"></param>
        /// <param name="index">伺服参数目录索引</param>
        /// <param name="subindex">伺服参数目录索引下编号</param>
        /// <param name="paradata">伺服参数写入值</param>
        /// <returns></returns>
        public bool WriteServoParameter(int axisNo, ushort index, ushort subindex, uint paradata)
        {
            short var = 0;
            var = LTDMC.nmc_write_parameter(m_Axises[axisNo].Card, m_Axises[axisNo].Axis, index, subindex, paradata);
            return var == 0 ? true : false;
        }

        /// <summary>
        /// 伺服参数写 EEPROM 操作
        /// </summary>
        /// <param name="axisNo"></param>
        /// <returns></returns>
        public bool WriteSlaveEeprom(int axisNo)
        {
            short var = 0;
            var = LTDMC.nmc_write_slave_eeprom(m_Axises[axisNo].Card, m_Axises[axisNo].Axis);
            return var == 0 ? true : false;
        }

        /// <summary>
        /// 获取当前轴的转矩值
        /// </summary>
        /// <param name="axisNo"></param>
        /// <returns></returns>
        public int GetTorque(int axisNo)
        {
            int torque = 0;
 //           var var = LTDMC.nmc_get_torque(m_Axises[axisNo].Card, m_Axises[axisNo].Axis,ref torque);
            return torque;
        }

        /// <summary>
        /// 启动转矩模式下的运动
        /// </summary>
        /// <param name="axisNo"></param>
        /// <param name="torque">扭力</param>
        /// <param name="PosLimitValid">位置限制开关</param>
        /// <param name="PosLimitValue">限制值</param>
        /// <param name="PosMode">位置模式</param>
        /// <returns></returns>
        public bool TorqueMove(int axisNo, int torque, ushort PosLimitValid, double PosLimitValue, ushort PosMode)
        {
            short var = 0;
  //          var = LTDMC.nmc_torque_move(m_Axises[axisNo].Card, m_Axises[axisNo].Axis, torque, PosLimitValid, PosLimitValue, PosMode);
            return var == 0 ? true : false;
        }

        /// <summary>
        /// 转矩控制过程中在线调整转矩值
        /// </summary>
        /// <param name="axisNo"></param>
        /// <param name="torque">扭力</param>
        /// <returns></returns>
        public bool ChangeTorque(int axisNo, int torque)
        {
            short var = 0;
 //           var = LTDMC.nmc_change_torque(m_Axises[axisNo].Card, m_Axises[axisNo].Axis, torque); 
            return var == 0 ? true : false;
        }

        #endregion

    }

    public struct LSCard
    {
        public ushort Axis;
        public ushort Card;
    }
}