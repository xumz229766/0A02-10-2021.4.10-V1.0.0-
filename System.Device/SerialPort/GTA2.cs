using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;
using System.Windows.Forms;
using System.Framework;
namespace System.Device
{
    public class GTA2 : SerialPort, ISerialPortTriggerModel
    {
        /*
         * Enter Service Mode $S<CR(0x0D)>
         * Exit Service Mode $Ar<CR(0x0D)>
         * OperatingModels - Serial Online - $CSNRM01<CR(0x0D)>
         * ManualTriggerControl - Enable - $CSOTM01<CR(0x0D)>
         * TRG - <STX(0x02)><CR(0x0D)>
         * STOP - <ETX(0x03)><CR(0x0D)>    
         */

        #region 变量

        string receiveString = "";

        #endregion

        #region 构造函数

        public GTA2() : base() { }

        /// <summary>
        /// 使用指定的端口名称、波特率、奇偶校验位、数据位和停止位初始化 System.IO.Ports.SerialPort 类的新实例。
        /// </summary>
        /// <param name="portName">要使用的端口（例如 COM1）。</param>
        /// <param name="baudRate">波特率。</param>
        /// <param name="parity"> System.IO.Ports.SerialPort.Parity 值之一。</param>
        /// <param name="dataBits">数据位值。</param>
        /// <param name="stopBits">System.IO.Ports.SerialPort.StopBits 值之一。</param>
        public GTA2(string portName, int baudRate, Parity parity, int dataBits, StopBits stopBits) : base(portName, baudRate, parity, dataBits, stopBits) { }

        #endregion

        #region 方法

        #endregion

        #region 接口

        public event DataReceiveCompleteEventHandler DeviceDataReceiveCompelete;

        public string Name { get; set; }
        public string ConnectionParam { get; set; }

        public void SetConnectionParam(string str)
        {
            ConnectionParam = str;
            string[] param = ConnectionParam.Split(',');
            PortName = param[0];
            BaudRate = int.Parse(param[1]);
            Parity = (Parity)Enum.Parse(typeof(Parity), param[2]);
            DataBits = int.Parse(param[3]);
            StopBits = (StopBits)Enum.Parse(typeof(StopBits), param[4]);
            ReadTimeout = int.Parse(param[5]);
            WriteTimeout = int.Parse(param[6]);
            RtsEnable = true;
            DtrEnable = true;
        }

        public void DeviceOpen()
        {
            if (IsOpen)
                throw new Exception("设备已经连接\n");
            Open();
        }

        public void DeviceClose()
        {
            if (IsOpen)
            {
                Application.DoEvents();
                Close();
            }
        }

        delegate void TriggerDelegate(TriggerArgs args);
        TriggerDelegate triggerMethod;
        public IAsyncResult TriggerResult;
        public IAsyncResult BeginTrigger(TriggerArgs args)
        {
            if (triggerMethod == null)
                triggerMethod = new TriggerDelegate(Trigger);

            if (TriggerResult != null && !TriggerResult.IsCompleted)
                return TriggerResult;

            TriggerResult = triggerMethod.BeginInvoke(args, null, null);

            return TriggerResult;
        }

        [ExecuteInfo("TRG", "读码器开始解码", "TRG")]
        public void Trigger(TriggerArgs args)
        {
            var Num = 0;
            var msg = args.message.Split(',');
            if (msg[0].Contains("W")) Num = 11;
            else Num = 8;
            try
            {
                var WriteBuffer = new byte[Num];
                WriteBuffer[0] = args.tryTimes;
                if(msg[0].Contains("WO"))
                {
                    WriteBuffer[2] = 0x21;
                }
                else
                {
                    WriteBuffer[2] = 0x20;
                }              
                switch(msg[0])
                {
                    case "R1": WriteBuffer[3] = 0x00; break;
                    case "R2": WriteBuffer[3] = 0x01; break;
                    case "R3": WriteBuffer[3] = 0x02; break;
                    case "R4": WriteBuffer[3] = 0x03; break;
                    case "W1": WriteBuffer[3] = 0x0C; break;
                    case "W2": WriteBuffer[3] = 0x0D; break;
                    case "W3": WriteBuffer[3] = 0x0E; break;
                    case "W4": WriteBuffer[3] = 0x0F; break;
                    case "WO1": WriteBuffer[3] = 0x10; break;
                    case "WO2": WriteBuffer[3] = 0x11; break;
                    case "WO3": WriteBuffer[3] = 0x12; break;
                    case "WO4": WriteBuffer[3] = 0x13; break;
                }
                WriteBuffer[4] = 0x00;
                WriteBuffer[5] = 0x01;
                if (msg[0].Contains("W"))
                {
                    var Value = int.Parse(msg[1]);
                    WriteBuffer[1] = 0x10;
                    WriteBuffer[6] = 0x02;
                    WriteBuffer[7] = (byte)(Value >> 8); 
                    WriteBuffer[8] = (byte)(Value & 0xFF);
                    var CRC = Get_CRC(WriteBuffer, 9);
                    WriteBuffer[9] = (byte)(CRC & 0xFF);
                    WriteBuffer[10] = (byte)(CRC >> 8);
                }
                else
                {
                    WriteBuffer[1] = 0x03;
                    var CRC = Get_CRC(WriteBuffer, 6);
                    WriteBuffer[6] = (byte)(CRC & 0xFF);
                    WriteBuffer[7] = (byte)(CRC >> 8);
                }
                receiveString = "";
                DiscardInBuffer();
                Write(WriteBuffer, 0, Num);
                var ReadBuffer = new byte[8];
                Threading.Thread.Sleep(100);
                Read(ReadBuffer, 0, 8);
                receiveString = ReadBuffer[0].ToString() + "," +ReadBuffer[1].ToString() + "," +
                    ReadBuffer[2].ToString() + "," +ReadBuffer[3].ToString() + "," +
                    ReadBuffer[4].ToString() + "," +ReadBuffer[5].ToString() + "," +
                    ReadBuffer[6].ToString() + "," + ReadBuffer[7].ToString()+","+ WriteBuffer[3].ToString();
            }
            catch (Exception ex)
            {
                receiveString = UniversalFlags.errorStr + ex.Message;
            }
            if (args.sender.GetType() != typeof(GTA2))
            {
                if (DeviceDataReceiveCompelete != null)
                    DeviceDataReceiveCompelete(this, receiveString);
            }
        }

        [ExecuteInfo("RST", "读码器停止解码", "RST")]
        public string StopTrigger()
        {
            try
            {
                WriteLine("-\r\n");
                return UniversalFlags.successStr;
            }
            catch (Exception ex)
            {
                return UniversalFlags.errorStr + ex.Message;
            }
        }

        public string Execute(string cmd)
        {
            string result = "";
            cmd = cmd.ToUpper().Trim();
            if ("HEL" == cmd.Substring(0, 3))
            {
                Attribute[] attribs = typeof(DM50S).GetMethods().Select(s =>
                    Attribute.GetCustomAttribute(s, typeof(ExecuteInfo))
                    ).Where(s => s != null).ToArray();

                result = "该设备支持以下指令：" + Environment.NewLine;
                foreach (Attribute attrib in attribs)
                {
                    ExecuteInfo exe = (ExecuteInfo)attrib;
                    result += Environment.NewLine + exe.Command + " - " + exe.Describe + Environment.NewLine
                        + "示例：" + exe.Example + Environment.NewLine;
                }
            }
            else if ("TRG" == cmd.Substring(0, 3))
            {
                Trigger(new TriggerArgs()
                {
                    sender = this,
                    tryTimes = 1
                });
                result = receiveString;
            }
            else if ("RST" == cmd.Substring(0, 3))
            {
                result = StopTrigger();
            }
            else
            {
                result = "不支持指令" + cmd;
            }
            return result;
        }

        private UInt32 Get_CRC(byte[] pBuf, int num)
        {
            UInt16 i, j;
            UInt32 wCrc = 0xFFFF;
            for (i = 0; i < num; i++)
            {
                wCrc ^= (UInt32)pBuf[i];
                for (j = 0; j < 8; j++)
                {
                    if ((wCrc & 1) > 0)
                    {
                        wCrc >>= 1;
                        wCrc ^= 0xA001;
                    }
                    else wCrc >>= 1;
                }
            }
            return wCrc;
        }

        #endregion
    }    
}
