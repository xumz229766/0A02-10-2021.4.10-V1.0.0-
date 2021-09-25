using System;
using System.Diagnostics;
using System.Device;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.ToolKit;

namespace desay
{
    public class Marking
    {

        /// <summary>
        /// 左1剪切数量完成
        /// </summary>
        public static bool LeftCut1Done;
        /// <summary>
        /// 左2剪切数量完成
        /// </summary>
        public static bool LeftCut2Done;
        /// <summary>
        /// 右1剪切数量完成
        /// </summary>
        public static bool RightCut1Done;
        /// <summary>
        ///  右2剪切数量完成
        /// </summary>
        public static bool RightCut2Done;

        /// <summary>
        ///  手动重复夹复位
        /// </summary>
        public static bool DuplicateClipReset;

        #region 工作信号
        /// <summary>
        /// 接料完成信号
        /// </summary>
        public static bool NoRodFinish;
        /// <summary>
        /// 进料完成信号
        /// </summary>
        public static bool FeederFinish;
        /// <summary>
        /// 缓存完成信号
        /// </summary>
        public static bool BufferFinish;
        /// <summary>
        /// 移料完成信号
        /// </summary>
        public static bool MoveFinish;
        /// <summary>
        /// 左C轴有料
        /// </summary>
        public static bool LeftCAxisHaveProduct;
        /// <summary>
        /// 剪切屏蔽
        /// </summary>
        public static bool[] CutSheild = new bool[4];
      

        /// <summary>
        /// 中C轴有料
        /// </summary>
        public static bool MiddleCAxisHaveProduct;
        /// <summary>
        /// 右C轴有料
        /// </summary>
        public static bool RightCAxisHaveProduct;

        /// <summary>
        /// C轴送料完成信号
        /// </summary>
        public static bool CAxisFinish;

        /// <summary>
        /// 左1#剪切轴切料完成
        /// </summary>
        public static bool LeftCut1Finish;
        /// <summary>
        /// 左1#剪切右产品
        /// </summary>
        public static bool LeftCut1HaveProduct;
        /// <summary>
        /// 左2#剪切轴切料完成
        /// </summary>
        public static bool LeftCut2Finish;
        /// <summary>
        /// 左2#剪切右产品
        /// </summary>
        public static bool LeftCut2HaveProduct;
        /// <summary>
        /// 右1#剪切轴切料完成
        /// </summary>
        public static bool RightCut1Finish;
        /// <summary>
        /// 右1#剪切右产品
        /// </summary>
        public static bool RightCut1HaveProduct;
        /// <summary>
        /// 右2#剪切轴切料完成
        /// </summary>
        public static bool RightCut2Finish;
        /// <summary>
        /// 右2#剪切右产品
        /// </summary>
        public static bool RightCut2HaveProduct;
        /// <summary>
        /// 左1#吸笔吸气标记
        /// </summary>
        public static bool XYZLeftInhale1Sign;
        /// <summary>
        /// 左2#吸笔吸气标记
        /// </summary>
        public static bool XYZLeftInhale2Sign;
        /// <summary>
        /// 右1#吸笔吸气标记
        /// </summary>
        public static bool XYZRightInhale1Sign;
        /// <summary>
        /// 右2#吸笔吸气标记
        /// </summary>
        public static bool XYZRightInhale2Sign;

        public static bool XYZCut1Finish;
        public static bool XYZCut2Finish;
        public static bool XYZCut3Finish;
        public static bool XYZCut4Finish;
        public static bool ForceInPlate;

        /// <summary>
        /// Z轴吸笔抬起完成
        /// </summary>
        public static bool[] ZUpTrayLensFinish = new bool[4];
        /// <summary>
        /// 仓储移位完成
        /// </summary>
        public static bool StoreFinish;
        public static bool PlateLittle;

        /// <summary>
        /// M轴移动到换盘位(0-初始化 1-到待机位 2-去勾盘位 3-勾盘后回待机位 4-去退盘位 5-退盘后到下一格)
        /// </summary>   
        public static int IsMMoveChangeTrayPos;

        /// <summary>
        /// 取托盘完成
        /// </summary>
        public static bool GetPlateFinish;

        /// <summary>
        /// 启动废料计数
        /// </summary>
        public static int ModelCount;

        #endregion

        #region 防呆信号
        /// <summary>
        /// 勾盘标志
        /// </summary>
        public static bool GettingPlate;
        /// <summary>
        /// 退盘标志
        /// </summary>
        public static bool PuttingPlate;
        /// <summary>
        /// M轴正在移动,不移动为true
        /// </summary>
        public static bool MIsNoMoving;
        #endregion
        /// <summary>
        /// 抽检开启
        /// </summary>
        public static bool SelectCheckMode;
        /// <summary>
        /// 抽检完成
        /// </summary>
        public static bool SelectCheckModeFinish;
        /// <summary>
        /// 换盘
        /// </summary>
        public static bool MancelChangeTray;
        /// <summary>
        /// 抛料模式
        /// </summary>
        public static bool ThrowerMode;
        /// <summary>
        /// 周期停止
        /// </summary>
        public static bool SystemStop;
        /// <summary>
        /// 清料计数
        /// </summary>
        public static int SystemStopCount;
        /// <summary>
        /// 开机排料标记
        /// </summary>
        public static bool ThrowerModeFrist;
        /// <summary>
        /// 换盘排料标记 
        /// </summary> 
        public static bool changeTrayLayoutSign;
        /// <summary>
        /// 换盘排料等待标记 
        /// </summary> 
        public static bool changeTrayLayoutWaitSign;

        /// <summary>
        ///   换盘禁止动作
        /// </summary>
        public static bool ChangeTrayPadlock;
      
        #region 模组屏蔽信号


        #endregion

        #region 屏蔽
        /// <summary>
        /// 安全光幕屏蔽
        /// </summary>
        public static bool AutoDoorSheild;
        /// <summary>
        ///     料盘感应屏蔽
        /// </summary>
        public static bool traySensorSheild;
        /// <summary>
        ///     安全门屏蔽
        /// </summary>
        public static bool DoorSafeSensorSheild;
        /// <summary>
        /// 移料光纤屏蔽
        /// </summary>
        public static bool MoveProductSensorSheild;
        /// <summary>
        /// 来料光电屏蔽
        /// </summary>
        public static bool SpliceSensorSheild;
        /// <summary>
        /// 清料位置
        /// </summary>
        public static bool[] CleanSign = new bool[10];
        /// <summary>
        /// 清料完成标志
        /// </summary>
        public static bool CleanProductDone;
        #endregion

        /// <summary>
        /// 运行时间
        /// </summary>
        public static string CycleRunTime;
        /// <summary>
        /// 运行时间
        /// </summary>
        public static string StartRunTime;

        /// <summary>
        /// C1轴计时器
        /// </summary>
        public static Stopwatch watchCT1 = new Stopwatch();
        /// <summary>
        /// C1轴周期时间
        /// </summary>
        public static double watchCT1Value;
        /// <summary>
        /// C2轴计时器
        /// </summary>
        public static Stopwatch watchCT2 = new Stopwatch();
        /// <summary>
        /// C2轴周期时间
        /// </summary>
        public static double watchCT2Value;
        /// <summary>
        /// C3轴计时器
        /// </summary>
        public static Stopwatch watchCT3 = new Stopwatch();
        /// <summary>
        /// C3轴周期时间
        /// </summary>
        public static double watchCT3Value;
        /// <summary>
        /// C4轴计时器
        /// </summary>
        public static Stopwatch watchCT4 = new Stopwatch();
        /// <summary>
        /// C4轴周期时间
        /// </summary>
        public static double watchCT4Value;
        /// <summary>
        /// XYZ轴计时器
        /// </summary>
        public static Stopwatch watchXYZ = new Stopwatch();
        /// <summary>
        /// XYZ轴周期时间
        /// </summary>
        public static double watchXYZValue;
        /// <summary>
        /// 一模计时器
        /// </summary>
        public static Stopwatch watchModel = new Stopwatch();
        /// <summary>
        /// 一模生产周期时间
        /// </summary>
        public static double watchModelValue;

        public static int[] CurrTemperatureValue = new int[8];

        /// <summary>
        /// 抽检完成续原摆盘标记
        /// </summary>
        public static bool SelectTarySign = false;
        /// <summary>
        /// 换盘执行状态(控制界面按键状态)
        /// </summary>
        public static bool MancelChangeRunState = false;
        /// <summary>
        /// 抽检仓储位置索引
        /// </summary>
        public static bool MSelectIndex = false;
        /// <summary>
        /// 抽检退盘判别勾那层盘
        /// </summary>
        public static bool HookLayerPlate = false;

        /// <summary>
        /// 设备回待机位状态
        /// </summary>
        public static bool[] equipmentHomeWaitState = new bool[10] { false,false, false, false, false, false, false, false, false, false };

        /// <summary>
        /// 修改参数标记
        /// </summary>
        public static bool ModifyParameterMarker = false;

        /// <summary>
        /// 碎料电机过载报警标记
        /// </summary>
        public static bool MotorOverloadSign = false;

        /// <summary>
        /// 报警停止运行
        /// </summary>
        public static bool AlarmStopRun = true;

        /// <summary>
        /// 剪切次数
        /// </summary>
        public static int[] CutCount = new int[4] { 0, 0, 0, 0 };

        [DllImport("psapi.dll")]
        static extern int EmptyWorkingSet(IntPtr hwProc);

        public static void ClearMemory()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            try
            {
                Process process = Process.GetCurrentProcess();
                EmptyWorkingSet(process.Handle);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "内存清理报错", MessageBoxButtons.OK);
            }
        }
    }
}
