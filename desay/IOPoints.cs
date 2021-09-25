
using System.Interfaces;
using System.AdvantechAps;
namespace desay
{
    /// <summary>
    ///     设备 IO 项
    /// </summary>
    public class IoPoints
    {
        private const string ApsControllerName = "雷赛总线控制卡";

        public static ApsController ApsController = new ApsController() { Name = ApsControllerName };

        #region IO输入 - 1

        /// <summary>
        ///   接料原点信号
        /// </summary>
        public static IoPoint T1IN0 = new IoPoint(ApsController, 0, 8, IoModes.Senser)
        {
            Name = "T1IN0",
            Description = "接料原点信号"
        };

        /// <summary>
        ///   接料动点信号
        /// </summary>
        public static IoPoint T1IN1 = new IoPoint(ApsController, 0, 9, IoModes.Senser)
        {
            Name = "T1IN1",
            Description = "接料动点信号"
        };

        /// <summary>
        ///   接料中点信号
        /// </summary>
        public static IoPoint T1IN2 = new IoPoint(ApsController, 0, 10, IoModes.Senser)
        {
            Name = "T1IN2",
            Description = "接料中点信号"
        };

        /// <summary>
        ///   1#来料光电感应
        /// </summary>
        public static IoPoint T1IN3 = new IoPoint(ApsController, 0, 11, IoModes.Senser)
        {
            Name = "T1IN3",
            Description = "1#来料光电感应"
        };

        /// <summary>
        ///   碎料气缸原点
        /// </summary>
        public static IoPoint T1IN4 = new IoPoint(ApsController, 0, 12, IoModes.Senser)
        {
            Name = "T1IN4",
            Description = "碎料气缸原点"
        };

        /// <summary>
        ///   碎料气缸动点
        /// </summary>
        public static IoPoint T1IN5 = new IoPoint(ApsController, 0, 13, IoModes.Senser)
        {
            Name = "T1IN5",
            Description = "碎料气缸动点"
        };

        /// <summary>
        ///   排料气缸原点
        /// </summary>
        public static IoPoint T1IN6 = new IoPoint(ApsController, 0, 14, IoModes.Senser)
        {
            Name = "T1IN6",
            Description = "排料气缸原点"
        };

        /// <summary>
        ///   排料气缸动点
        /// </summary>
        public static IoPoint T1IN7 = new IoPoint(ApsController, 0, 15, IoModes.Senser)
        {
            Name = "T1IN7",
            Description = "排料气缸动点"
        };
        /// <summary>
        ///   碎料盖子气缸原点
        /// </summary>
        public static IoPoint T1IN8 = new IoPoint(ApsController, 0, 16, IoModes.Senser)
        {
            Name = "T1IN8",
            Description = "碎料盖子气缸原点"
        };
        /// <summary>
        ///   碎料盖子气缸动点
        /// </summary>
        public static IoPoint T1IN9 = new IoPoint(ApsController, 0, 17, IoModes.Senser)
        {
            Name = "T1IN9",
            Description = "碎料盖子气缸动点"
        };
        /// <summary>
        ///   1#移料左右原点信号
        /// </summary>
        public static IoPoint T1IN10 = new IoPoint(ApsController, 0, 18, IoModes.Senser)
        {
            Name = "T1IN10",
            Description = "1#移料左右原点信号"
        };
        /// <summary>
        ///  1#移料左右动点信号
        /// </summary>
        public static IoPoint T1IN11 = new IoPoint(ApsController, 0, 19, IoModes.Senser)
        {
            Name = "T1IN11",
            Description = "1#移料左右动点信号"
        };
        /// <summary>
        ///   2#移料左右原点信号
        /// </summary>
        public static IoPoint T1IN12 = new IoPoint(ApsController, 0, 20, IoModes.Senser)
        {
            Name = "T1IN12",
            Description = "2#移料左右原点信号"
        };

        /// <summary>
        ///   2#移料左右动点信号
        /// </summary>
        public static IoPoint T1IN13 = new IoPoint(ApsController, 0, 21, IoModes.Senser)
        {
            Name = "T1IN13",
            Description = "2#移料左右动点信号"
        };
        /// <summary>
        ///   移料上下原点信号
        /// </summary>
        public static IoPoint T1IN14 = new IoPoint(ApsController, 0, 22, IoModes.Senser)
        {
            Name = "T1IN14",
            Description = "移料上下原点信号"
        };
        /// <summary>
        ///   移料上下动点信号
        /// </summary>
        public static IoPoint T1IN15 = new IoPoint(ApsController, 0, 23, IoModes.Senser)
        {
            Name = "T1IN15",
            Description = "移料上下动点信号"
        };

        /// <summary>
        ///   缓冲升降气缸原点
        /// </summary>
        public static IoPoint T1IN16 = new IoPoint(ApsController, 0, 24, IoModes.Senser)
        {
            Name = "T1IN16",
            Description = "缓冲升降气缸原点"
        };

        /// <summary>
        ///   缓冲升降气缸动点
        /// </summary>
        public static IoPoint T1IN17 = new IoPoint(ApsController, 0, 25, IoModes.Senser)
        {
            Name = "T1IN17",
            Description = "缓冲升降气缸动点"
        };

        /// <summary>
        ///   缓冲左右气缸原点
        /// </summary>
        public static IoPoint T1IN18 = new IoPoint(ApsController, 0, 26, IoModes.Senser)
        {
            Name = "T1IN18",
            Description = "缓冲左右气缸原点"
        };

        /// <summary>
        ///   缓冲左右气缸动点
        /// </summary>
        public static IoPoint T1IN19 = new IoPoint(ApsController, 0, 27, IoModes.Senser)
        {
            Name = "T1IN19",
            Description = "缓冲左右气缸动点"
        };

        /// <summary>
        ///   缓冲夹子气缸原点（备用）
        /// </summary>
        public static IoPoint T1IN20 = new IoPoint(ApsController, 0, 28, IoModes.Senser)
        {
            Name = "T1IN20",
            Description = "缓冲夹子气缸原点（备用）"
        };

        /// <summary>
        ///   缓冲夹子气缸动点（备用）
        /// </summary>
        public static IoPoint T1IN21 = new IoPoint(ApsController, 0, 29, IoModes.Senser)
        {
            Name = "T1IN21",
            Description = "缓冲夹子气缸动点（备用）"
        };

        /// <summary>
        ///   进料气缸原点
        /// </summary>
        public static IoPoint T1IN22 = new IoPoint(ApsController, 0, 30, IoModes.Senser)
        {
            Name = "T1IN22",
            Description = "进料气缸原点"
        };

        /// <summary>
        ///  进料气缸动点
        /// </summary>
        public static IoPoint T1IN23 = new IoPoint(ApsController, 0, 31, IoModes.Senser)
        {
            Name = "T1IN23",
            Description = "进料气缸动点"
        };
        /// <summary>
        ///   1#剪切翻转气缸原点
        /// </summary>
        public static IoPoint T1IN24 = new IoPoint(ApsController, 0, 32, IoModes.Senser)
        {
            Name = "T1IN24",
            Description = "1#剪切翻转气缸原点"
        };
        /// <summary>
        ///   1#剪切翻转气缸动点
        /// </summary>
        public static IoPoint T1IN25 = new IoPoint(ApsController, 0, 33, IoModes.Senser)
        {
            Name = "T1IN25",
            Description = "1#剪切翻转气缸动点"
        };
        /// <summary>
        ///   2#剪切夹料上下原点
        /// </summary>
        public static IoPoint T1IN26 = new IoPoint(ApsController, 0,34, IoModes.Senser)
        {
            Name = "T1IN26",
            Description = "2#剪切翻转气缸原点"
        };
        /// <summary>
        ///  2#剪切夹料上下动点
        /// </summary>
        public static IoPoint T1IN27 = new IoPoint(ApsController, 0, 35, IoModes.Senser)
        {
            Name = "T1IN27",
            Description = "2#剪切翻转气缸动点"
        };
        /// <summary>
        ///   3#剪切夹料上下原点
        /// </summary>
        public static IoPoint T1IN28 = new IoPoint(ApsController, 0, 36, IoModes.Senser)
        {
            Name = "T1IN28",
            Description = "3#剪切翻转气缸原点"
        };

        /// <summary>
        ///   3#剪切夹料上下动点
        /// </summary>
        public static IoPoint T1IN29 = new IoPoint(ApsController, 0, 37, IoModes.Senser)
        {
            Name = "T1IN29",
            Description = "3#剪切翻转气缸动点"
        };
        /// <summary>
        ///  4#剪切夹料上下原点
        /// </summary>
        public static IoPoint T1IN30 = new IoPoint(ApsController, 0, 38, IoModes.Senser)
        {
            Name = "T1IN30",
            Description = "4#剪切翻转气缸原点"
        };
        /// <summary>
        ///   4#剪切夹料上下动点
        /// </summary>
        public static IoPoint T1IN31 = new IoPoint(ApsController, 0, 39, IoModes.Senser)
        {
            Name = "T1IN31",
            Description = "4#剪切翻转气缸动点"
        };

        #endregion

        #region IO输入 - 2

        /// <summary>
        ///   1#剪切夹料前后原点信号
        /// </summary>
        public static IoPoint T2IN0 = new IoPoint(ApsController, 0, 40, IoModes.Senser)
        {
            Name = "T2IN0",
            Description = "1#剪切夹料前后原点信号"
        };

        /// <summary>
        ///   1#剪切夹料前后动点信号
        /// </summary>
        public static IoPoint T2IN1 = new IoPoint(ApsController, 0, 41, IoModes.Senser)
        {
            Name = "T2IN1",
            Description = "1#剪切夹料前后动点信号"
        };

        /// <summary>
        ///   2#剪切夹料前后原点信号
        /// </summary>
        public static IoPoint T2IN2 = new IoPoint(ApsController, 0, 42, IoModes.Senser)
        {
            Name = "T2IN2",
            Description = "2#剪切夹料前后原点信号"
        };

        /// <summary>
        ///   2#剪切夹料前后动点信号
        /// </summary>
        public static IoPoint T2IN3 = new IoPoint(ApsController, 0, 43, IoModes.Senser)
        {
            Name = "T2IN3",
            Description = "2#剪切夹料前后动点信号"
        };

        /// <summary>
        ///   3#剪切夹料前后原点信号
        /// </summary>
        public static IoPoint T2IN4 = new IoPoint(ApsController, 0, 44, IoModes.Senser)
        {
            Name = "T2IN4",
            Description = "3#剪切夹料前后原点信号"
        };

        /// <summary>
        ///   3#剪切夹料前后动点信号
        /// </summary>
        public static IoPoint T2IN5 = new IoPoint(ApsController, 0, 45, IoModes.Senser)
        {
            Name = "T2IN5",
            Description = "3#剪切夹料前后动点信号"
        };

        /// <summary>
        ///   4#剪切夹料前后原点信号
        /// </summary>
        public static IoPoint T2IN6 = new IoPoint(ApsController, 0, 46, IoModes.Senser)
        {
            Name = "T2IN6",
            Description = "4#剪切夹料前后原点信号"
        };

        /// <summary>
        ///  4#剪切夹料前后动点信号
        /// </summary>
        public static IoPoint T2IN7 = new IoPoint(ApsController, 0, 47, IoModes.Senser)
        {
            Name = "T2IN7",
            Description = "4#剪切夹料前后动点信号"
        };
        /// <summary>
        ///   1#夹子动点信号
        /// </summary>
        public static IoPoint T2IN8 = new IoPoint(ApsController, 0, 48, IoModes.Senser)
        {
            Name = "T2IN8",
            Description = "1#夹子动点信号"
        };
        /// <summary>
        ///  2#夹子动点信号
        /// </summary>
        public static IoPoint T2IN9 = new IoPoint(ApsController, 0, 49, IoModes.Senser)
        {
            Name = "T2IN9",
            Description = "2#夹子动点信号"
        };
        /// <summary>
        ///  3#夹子动点信号
        /// </summary>
        public static IoPoint T2IN10 = new IoPoint(ApsController, 0, 50, IoModes.Senser)
        {
            Name = "T2IN10",
            Description = "3#夹子动点信号"
        };
        /// <summary>
        ///   4#夹子动点信号
        /// </summary>
        public static IoPoint T2IN11 = new IoPoint(ApsController, 0, 51, IoModes.Senser)
        {
            Name = "T2IN11",
            Description = "4#夹子动点信号"
        };
        /// <summary>
        ///  1#吸笔左右原点信号
        /// </summary>
        public static IoPoint T2IN12 = new IoPoint(ApsController, 0, 52, IoModes.Senser)
        {
            Name = "T2IN12",
            Description = "1#吸笔左右原点信号"
        };

        /// <summary>
        ///   1#吸笔左右动点信号
        /// </summary>
        public static IoPoint T2IN13 = new IoPoint(ApsController, 0, 53, IoModes.Senser)
        {
            Name = "T2IN13",
            Description = "1#吸笔左右动点信号"
        };
        /// <summary>
        ///  2#吸笔左右原点信号
        /// </summary>
        public static IoPoint T2IN14 = new IoPoint(ApsController, 0, 54, IoModes.Senser)
        {
            Name = "T2IN14",
            Description = "2#吸笔左右原点信号"
        };
        /// <summary>
        ///   2#吸笔左右动点信号
        /// </summary>
        public static IoPoint T2IN15 = new IoPoint(ApsController, 0, 55, IoModes.Senser)
        {
            Name = "T2IN15",
            Description = "2#吸笔左右动点信号"
        };
        /// <summary>
        ///   4#吸笔左右原点信号
        /// </summary>
        public static IoPoint T2IN16 = new IoPoint(ApsController, 0, 56, IoModes.Senser)
        {
            Name = "T2IN16",
            Description = "4#吸笔左右原点信号"
        };

        /// <summary>
        ///  4#吸笔左右动点信号
        /// </summary>
        public static IoPoint T2IN17 = new IoPoint(ApsController, 0, 57, IoModes.Senser)
        {
            Name = "T2IN17",
            Description = "4#吸笔左右动点信号"
        };

        /// <summary>
        ///   M左卡盘检测信号
        /// </summary>
        public static IoPoint T2IN18 = new IoPoint(ApsController, 0, 58, IoModes.Senser)
        {
            Name = "T2IN18",
            Description = "M左卡盘检测信号"
        };

        /// <summary>
        ///   M右卡盘检测信号
        /// </summary>
        public static IoPoint T2IN19 = new IoPoint(ApsController, 0, 59, IoModes.Senser)
        {
            Name = "T2IN19",
            Description = "M右卡盘检测信号"
        };

        /// <summary>
        ///   左拉盘上下原点信号
        /// </summary>
        public static IoPoint T2IN20 = new IoPoint(ApsController, 0, 60, IoModes.Senser)
        {
            Name = "T2IN20",
            Description = "左拉盘上下原点信号"
        };

        /// <summary>
        ///   左拉盘上下动点信号
        /// </summary>
        public static IoPoint T2IN21 = new IoPoint(ApsController, 0, 61, IoModes.Senser)
        {
            Name = "T2IN21",
            Description = "左拉盘上下动点信号"
        };

        /// <summary>
        ///   右拉盘上下原点信号
        /// </summary>
        public static IoPoint T2IN22 = new IoPoint(ApsController, 0, 62, IoModes.Senser)
        {
            Name = "T2IN22",
            Description = "右拉盘上下原点信号"
        };

        /// <summary>
        ///   右拉盘上下动点信号
        /// </summary>
        public static IoPoint T2IN23 = new IoPoint(ApsController, 0, 63, IoModes.Senser)
        {
            Name = "T2IN23",
            Description = "右拉盘上下动点信号"
        };
        /// <summary>
        ///  摆盘卡紧原点信号
        /// </summary>
        public static IoPoint T2IN24 = new IoPoint(ApsController, 0, 64, IoModes.Senser)
        {
            Name = "T2IN24",
            Description = "摆盘卡紧原点信号"
        };
        /// <summary>
        ///   摆盘卡紧动点信号
        /// </summary>
        public static IoPoint T2IN25 = new IoPoint(ApsController, 0, 65, IoModes.Senser)
        {
            Name = "T2IN25",
            Description = "摆盘卡紧动点信号"
        };
        /// <summary>
        ///   1#摆盘前感应
        /// </summary>
        public static IoPoint T2IN26 = new IoPoint(ApsController, 0, 66, IoModes.Senser)
        {
            Name = "T2IN26",
            Description = "1#摆盘前感应"
        };
        /// <summary>
        ///   2#摆盘前感应
        /// </summary>
        public static IoPoint T2IN27 = new IoPoint(ApsController, 0, 67, IoModes.Senser)
        {
            Name = "T2IN27",
            Description = "2#摆盘前感应"
        };
        /// <summary>
        ///   1#摆盘后感应
        /// </summary>
        public static IoPoint T2IN28 = new IoPoint(ApsController, 0, 68, IoModes.Senser)
        {
            Name = "T2IN28",
            Description = "1#摆盘后感应"
        };

        /// <summary>
        ///  2#摆盘后感应
        /// </summary>
        public static IoPoint T2IN29 = new IoPoint(ApsController, 0, 69, IoModes.Senser)
        {
            Name = "T2IN29",
            Description = "2#摆盘后感应"
        };
        /// <summary>
        ///   仓储自动门原点信号
        /// </summary>
        public static IoPoint T2IN30 = new IoPoint(ApsController, 0, 70, IoModes.Senser)
        {
            Name = "T2IN30",
            Description = "仓储自动门原点信号"
        };
        /// <summary>
        ///   仓储自动门动点信号
        /// </summary>
        public static IoPoint T2IN31 = new IoPoint(ApsController, 0, 71, IoModes.Senser)
        {
            Name = "T2IN31",
            Description = "仓储自动门动点信号"
        };

        #endregion

        #region IO输入 - 3

        /// <summary>
        ///   启动按钮
        /// </summary>
        public static IoPoint T3IN0 = new IoPoint(ApsController, 0, 72, IoModes.Senser)
        {
            Name = "T3IN0",
            Description = "启动按钮"
        };

        /// <summary>
        ///   停止按钮
        /// </summary>
        public static IoPoint T3IN1 = new IoPoint(ApsController, 0, 73, IoModes.Senser)
        {
            Name = "T3IN1",
            Description = "停止按钮"
        };

        /// <summary>
        ///   急停按钮
        /// </summary>
        public static IoPoint T3IN2 = new IoPoint(ApsController, 0, 74, IoModes.Senser)
        {
            Name = "T3IN2",
            Description = "急停按钮"
        };

        /// <summary>
        ///   复位按钮
        /// </summary>
        public static IoPoint T3IN3 = new IoPoint(ApsController, 0, 75, IoModes.Senser)
        {
            Name = "T3IN3",
            Description = "复位按钮"
        };

        /// <summary>
        ///  报警清除按钮
        /// </summary>
        public static IoPoint T3IN4 = new IoPoint(ApsController, 0, 76, IoModes.Senser)
        {
            Name = "T3IN4",
            Description = "报警清除按钮"
        };

        /// <summary>
        ///   左门禁信号
        /// </summary>
        public static IoPoint T3IN5 = new IoPoint(ApsController, 0, 77, IoModes.Senser)
        {
            Name = "T3IN5",
            Description = "左门禁信号"
        };

        /// <summary>
        ///   右门禁信号
        /// </summary>
        public static IoPoint T3IN6 = new IoPoint(ApsController, 0, 78, IoModes.Senser)
        {
            Name = "T3IN6",
            Description = "右门禁信号"
        };

        /// <summary>
        /// 安全光幕
        /// </summary>
        public static IoPoint T3IN7 = new IoPoint(ApsController, 0, 79, IoModes.Senser)
        {
            Name = "T3IN7",
            Description = "安全光幕"
        };
        /// <summary>
        ///   气压检测信号
        /// </summary>
        public static IoPoint T3IN8 = new IoPoint(ApsController, 0, 80, IoModes.Senser)
        {
            Name = "T3IN8",
            Description = "气压检测信号"
        };
        /// <summary>
        ///  仓储自动门按钮
        /// </summary>
        public static IoPoint T3IN9 = new IoPoint(ApsController, 0, 81, IoModes.Senser)
        {
            Name = "T3IN9",
            Description = "仓储自动门按钮"
        };
        /// <summary>
        ///  移料光纤感应
        /// </summary>
        public static IoPoint T3IN10 = new IoPoint(ApsController, 0, 82, IoModes.Senser)
        {
            Name = "T3IN10",
            Description = "移料光纤感应"
        };
        /// <summary>
        ///   备用
        /// </summary>
        public static IoPoint T3IN11 = new IoPoint(ApsController, 0, 83, IoModes.Senser)
        {
            Name = "T3IN11",
            Description = "备用"
        };
        /// <summary>
        /// 备用
        /// </summary>
        public static IoPoint T3IN12 = new IoPoint(ApsController, 0, 84, IoModes.Senser)
        {
            Name = "T3IN12",
            Description = "备用"
        };

        /// <summary>
        ///  备用
        /// </summary>
        public static IoPoint T3IN13 = new IoPoint(ApsController, 0, 85, IoModes.Senser)
        {
            Name = "T3IN13",
            Description = "备用"
        };
        /// <summary>
        ///  备用
        /// </summary>
        public static IoPoint T3IN14 = new IoPoint(ApsController, 0, 86, IoModes.Senser)
        {
            Name = "T3IN14",
            Description = "备用"
        };
        /// <summary>
        ///  碎料电机过载报警
        /// </summary>
        public static IoPoint T3IN15 = new IoPoint(ApsController, 0, 87, IoModes.Senser)
        {
            Name = "T3IN15",
            Description = "碎料电机过载报警"
        };
        /// <summary>
        ///   备用
        /// </summary>
        public static IoPoint T3IN16 = new IoPoint(ApsController, 0, 88, IoModes.Senser)
        {
            Name = "T3IN16",
            Description = "备用"
        };

        /// <summary>
        /// 备用
        /// </summary>
        public static IoPoint T3IN17 = new IoPoint(ApsController, 0, 89, IoModes.Senser)
        {
            Name = "T3IN17",
            Description = "备用"
        };

        /// <summary>
        ///   备用
        /// </summary>
        public static IoPoint T3IN18 = new IoPoint(ApsController, 0, 90, IoModes.Senser)
        {
            Name = "T3IN18",
            Description = "备用"
        };

        /// <summary>
        ///   备用
        /// </summary>
        public static IoPoint T3IN19 = new IoPoint(ApsController, 0, 91, IoModes.Senser)
        {
            Name = "T3IN19",
            Description = "备用"
        };

        /// <summary>
        ///   备用
        /// </summary>
        public static IoPoint T3IN20 = new IoPoint(ApsController, 0, 92, IoModes.Senser)
        {
            Name = "T3IN20",
            Description = "备用"
        };

        /// <summary>
        ///   备用
        /// </summary>
        public static IoPoint T3IN21 = new IoPoint(ApsController, 0, 93, IoModes.Senser)
        {
            Name = "T3IN21",
            Description = "备用"
        };

        /// <summary>
        ///   备用
        /// </summary>
        public static IoPoint T3IN22 = new IoPoint(ApsController, 0, 94, IoModes.Senser)
        {
            Name = "T3IN22",
            Description = "备用"
        };

        /// <summary>
        ///   备用
        /// </summary>
        public static IoPoint T3IN23 = new IoPoint(ApsController, 0, 95, IoModes.Senser)
        {
            Name = "T3IN23",
            Description = "备用"
        };
        /// <summary>
        ///  备用
        /// </summary>
        public static IoPoint T3IN24 = new IoPoint(ApsController, 0, 96, IoModes.Senser)
        {
            Name = "T3IN24",
            Description = "备用"
        };
        /// <summary>
        ///   备用
        /// </summary>
        public static IoPoint T3IN25 = new IoPoint(ApsController, 0, 07, IoModes.Senser)
        {
            Name = "T3IN25",
            Description = "备用"
        };
        /// <summary>
        ///   备用
        /// </summary>
        public static IoPoint T3IN26 = new IoPoint(ApsController, 0, 99, IoModes.Senser)
        {
            Name = "T3IN26",
            Description = "备用"
        };
        /// <summary>
        ///   备用
        /// </summary>
        public static IoPoint T3IN27 = new IoPoint(ApsController, 0, 100, IoModes.Senser)
        {
            Name = "T3IN27",
            Description = "备用"
        };
        /// <summary>
        ///   备用
        /// </summary>
        public static IoPoint T3IN28 = new IoPoint(ApsController, 0, 101, IoModes.Senser)
        {
            Name = "T3IN28",
            Description = "备用"
        };

        /// <summary>
        ///  备用
        /// </summary>
        public static IoPoint T3IN29 = new IoPoint(ApsController, 0, 102, IoModes.Senser)
        {
            Name = "T3IN29",
            Description = "备用"
        };
        /// <summary>
        ///   备用
        /// </summary>
        public static IoPoint T3IN30 = new IoPoint(ApsController, 0, 103, IoModes.Senser)
        {
            Name = "T3IN30",
            Description = "备用"
        };
        /// <summary>
        /// 虚似输入端子(请勿接线)
        /// </summary>
        public static IoPoint T3IN31 = new IoPoint(ApsController, 0, 104, IoModes.Senser)
        {
            Name = "T3IN31",
            Description = "虚似输入端子(请勿接线)"
        };

        #endregion

        #region IO输出 - 1
        /// <summary>
        /// 接料原点动作
        /// </summary>
        public static IoPoint T1DO0 = new IoPoint(ApsController, 0, 8, IoModes.Signal)
        {
            Name = "T1DO0",
            Description = "接料原点动作"
        };

        /// <summary>
        ///   接料动点动作
        /// </summary>
        public static IoPoint T1DO1 = new IoPoint(ApsController, 0, 9, IoModes.Signal)
        {
            Name = "T1DO1",
            Description = "接料动点动作"
        };

        /// <summary>
        /// 缓冲升降原点动作
        /// </summary>
        public static IoPoint T1DO2 = new IoPoint(ApsController, 0, 10, IoModes.Signal)
        {
            Name = "T1DO2",
            Description = "缓冲升降原点动作"
        };

        /// <summary>
        ///   缓冲左右气缸
        /// </summary>
        public static IoPoint T1DO3 = new IoPoint(ApsController, 0, 11, IoModes.Signal)
        {
            Name = "T1DO3",
            Description = "缓冲左右气缸"
        };

        /// <summary>
        ///  缓冲夹子气缸
        /// </summary>
        public static IoPoint T1DO4 = new IoPoint(ApsController, 0, 12, IoModes.Signal)
        {
            Name = "T1DO4",
            Description = "缓冲夹子气缸"
        };

        /// <summary>
        ///移料左右
        /// </summary>
        public static IoPoint T1DO5 = new IoPoint(ApsController, 0, 13, IoModes.Signal)
        {
            Name = "T1DO5",
            Description = "移料左右"
        };

        /// <summary>
        ///   移料上下
        /// </summary>
        public static IoPoint T1DO6 = new IoPoint(ApsController, 0, 14, IoModes.Signal)
        {
            Name = "T1DO6",
            Description = "移料上下"
        };

        /// <summary>
        ///    移料夹子
        /// </summary>
        public static IoPoint T1DO7 = new IoPoint(ApsController, 0, 15, IoModes.Signal)
        {
            Name = "T2DO7",
            Description = "移料夹子"
        };

        /// <summary>
        ///  碎料气缸
        /// </summary>
        public static IoPoint T1DO8 = new IoPoint(ApsController, 0, 16, IoModes.Signal)
        {
            Name = "T1DO8",
            Description = "碎料气缸"
        };

        /// <summary>
        /// 排料气缸
        /// </summary>
        public static IoPoint T1DO9 = new IoPoint(ApsController, 0, 17, IoModes.Signal)
        {
            Name = "T1DO9",
            Description = "排料气缸"
        };

        /// <summary>
        /// 碎料盖子气缸
        /// </summary>
        public static IoPoint T1DO10 = new IoPoint(ApsController, 0, 18, IoModes.Signal)
        {
            Name = "T1DO10",
            Description = "碎料盖子气缸"
        };

        /// <summary>
        ///  进料气缸
        /// </summary>
        public static IoPoint T1DO11 = new IoPoint(ApsController, 0, 19, IoModes.Signal)
        {
            Name = "T1DO11",
            Description = "进料气缸"
        };

        /// <summary>
        ///   1#剪刀夹料上下
        /// </summary>
        public static IoPoint T1DO12 = new IoPoint(ApsController, 0, 20, IoModes.Signal)
        {
            Name = "T1DO12",
            Description = "1#剪刀翻转气缸"
        };

        /// <summary>
        ///  2#剪刀夹料上下
        /// </summary>
        public static IoPoint T1DO13 = new IoPoint(ApsController, 0, 21, IoModes.Signal)
        {
            Name = "T1DO13",
            Description = "2#剪刀翻转气缸"
        };

        /// <summary>
        ///  3#剪刀夹料上下
        /// </summary>
        public static IoPoint T1DO14 = new IoPoint(ApsController, 0, 22, IoModes.Signal)
        {
            Name = "T1DO14",
            Description = "3#剪刀翻转气缸"
        };

        /// <summary>
        ///   4#剪刀夹料上下
        /// </summary>
        public static IoPoint T1DO15 = new IoPoint(ApsController, 0, 23, IoModes.Signal)
        {
            Name = "T1DO15",
            Description = "4#剪刀翻转气缸"
        };
        /// <summary>
        /// 1#剪切夹料前后
        /// </summary>
        public static IoPoint T1DO16 = new IoPoint(ApsController, 0, 24, IoModes.Signal)
        {
            Name = "T1DO16",
            Description = "1#剪切夹料前后"
        };

        /// <summary>
        ///  2#剪切夹料前后
        /// </summary>
        public static IoPoint T1DO17 = new IoPoint(ApsController, 0, 25, IoModes.Signal)
        {
            Name = "T1DO17",
            Description = "2#剪切夹料前后"
        };

        /// <summary>
        ///  3#剪切夹料前后
        /// </summary>
        public static IoPoint T1DO18 = new IoPoint(ApsController, 0, 26, IoModes.Signal)
        {
            Name = "T1DO18",
            Description = "3#剪切夹料前后"
        };

        /// <summary>
        ///  4#剪切夹料前后
        /// </summary>
        public static IoPoint T1DO19 = new IoPoint(ApsController, 0, 27, IoModes.Signal)
        {
            Name = "T1DO19",
            Description = "4#剪切夹料前后"
        };

        /// <summary>
        ///   1#剪切夹料夹爪
        /// </summary>
        public static IoPoint T1DO20 = new IoPoint(ApsController, 0, 28, IoModes.Signal)
        {
            Name = "T1DO20",
            Description = "1#剪切夹料夹爪"
        };

        /// <summary>
        ///  2#剪切夹料夹爪
        /// </summary>
        public static IoPoint T1DO21 = new IoPoint(ApsController, 0, 29, IoModes.Signal)
        {
            Name = "T1DO21",
            Description = "2#剪切夹料夹爪"
        };

        /// <summary>
        ///  3#剪切夹料夹爪
        /// </summary>
        public static IoPoint T1DO22 = new IoPoint(ApsController, 0, 30, IoModes.Signal)
        {
            Name = "T1DO22",
            Description = "3#剪切夹料夹爪"
        };

        /// <summary>
        ///  4#剪切夹料夹爪
        /// </summary>
        public static IoPoint T1DO23 = new IoPoint(ApsController, 0, 31, IoModes.Signal)
        {
            Name = "T1DO23",
            Description = "4#剪切夹料夹爪"
        };

        /// <summary>
        ///  1#吸笔吸真空
        /// </summary>
        public static IoPoint T1DO24 = new IoPoint(ApsController, 0, 32, IoModes.Signal)
        {
            Name = "T1DO24",
            Description = "1#吸笔吸真空"
        };

        /// <summary>
        ///  1#吸笔破真空
        /// </summary>
        public static IoPoint T1DO25 = new IoPoint(ApsController, 0, 33, IoModes.Signal)
        {
            Name = "T1DO25",
            Description = "1#吸笔破真空"
        };

        /// <summary>
        ///  2#吸笔吸真空
        /// </summary>
        public static IoPoint T1DO26 = new IoPoint(ApsController, 0, 34, IoModes.Signal)
        {
            Name = "T1DO26",
            Description = "2#吸笔吸真空"
        };

        /// <summary>
        /// 2#吸笔破真空
        /// </summary>
        public static IoPoint T1DO27 = new IoPoint(ApsController, 0, 35, IoModes.Signal)
        {
            Name = "T1DO27",
            Description = "2#吸笔破真空"
        };

        /// <summary>
        ///   3#吸笔吸真空
        /// </summary>
        public static IoPoint T1DO28 = new IoPoint(ApsController, 0, 36, IoModes.Signal)
        {
            Name = "T1DO28",
            Description = "3#吸笔吸真空"
        };

        /// <summary>
        ///  3#吸笔破真空
        /// </summary>
        public static IoPoint T1DO29 = new IoPoint(ApsController, 0, 37, IoModes.Signal)
        {
            Name = "T1DO29",
            Description = "3#吸笔破真空"
        };

        /// <summary>
        ///  4#吸笔吸真空
        /// </summary>
        public static IoPoint T1DO30 = new IoPoint(ApsController, 0, 38, IoModes.Signal)
        {
            Name = "T1DO30",
            Description = "4#吸笔吸真空"
        };

        /// <summary>
        ///   4#吸笔破真空
        /// </summary>
        public static IoPoint T1DO31 = new IoPoint(ApsController, 0, 39, IoModes.Signal)
        {
            Name = "T1DO31",
            Description = "4#吸笔破真空"
        };

        #endregion

        #region  IO输出 - 2


        /// <summary>
        /// 1#3#4#吸笔移动
        /// </summary>
        public static IoPoint T2DO0 = new IoPoint(ApsController, 0, 40, IoModes.Signal)
        {
            Name = "T2DO0",
            Description = "1#2#4#吸笔移动"
        };

        /// <summary>
        ///   拉盘气缸上下气缸
        /// </summary>
        public static IoPoint T2DO1 = new IoPoint(ApsController, 0, 41, IoModes.Signal)
        {
            Name = "T2DO1",
            Description = "拉盘气缸上下气缸"
        };

        /// <summary>
        ///   摆盘卡紧
        /// </summary>
        public static IoPoint T2DO2 = new IoPoint(ApsController, 0, 42, IoModes.Signal)
        {
            Name = "T2DO2",
            Description = "摆盘卡紧"
        };

        /// <summary>
        ///  安全门原点动作
        /// </summary>
        public static IoPoint T2DO3 = new IoPoint(ApsController, 0, 43, IoModes.Signal)
        {
            Name = "T2DO3",
            Description = "安全门原点动作"
        };

        /// <summary>
        ///   安全门动点动作
        /// </summary>
        public static IoPoint T2DO4 = new IoPoint(ApsController, 0, 44, IoModes.Signal)
        {
            Name = "T2DO4",
            Description = "安全门动点动作"
        };

        /// <summary>
        /// 照明灯控制继电器
        /// </summary>
        public static IoPoint T2DO5 = new IoPoint(ApsController, 0, 45, IoModes.Signal)
        {
            Name = "T2DO5",
            Description = "照明灯控制继电器"
        };

        /// <summary>
        ///  1#刀加热棒控制继电器
        /// </summary>
        public static IoPoint T2DO6 = new IoPoint(ApsController, 0, 46, IoModes.Signal)
        {
            Name = "T2DO6",
            Description = "1#刀加热棒控制继电器"
        };

        /// <summary>
        ///   2#刀加热棒控制继电器
        /// </summary>
        public static IoPoint T2DO7 = new IoPoint(ApsController, 0, 47, IoModes.Signal)
        {
            Name = "T2DO7",
            Description = "2#刀加热棒控制继电器"
        };

        /// <summary>
        ///  3#刀加热棒控制继电器
        /// </summary>
        public static IoPoint T2DO8 = new IoPoint(ApsController, 0, 48, IoModes.Signal)
        {
            Name = "T2DO8",
            Description = "3#刀加热棒控制继电器"
        };

        /// <summary>
        ///  4#刀加热棒控制继电器
        /// </summary>
        public static IoPoint T2DO9 = new IoPoint(ApsController, 0, 49, IoModes.Signal)
        {
            Name = "T2DO9",
            Description = "4#刀加热棒控制继电器"
        };

        /// <summary>
        ///  碎料电机开关
        /// </summary>
        public static IoPoint T2DO10 = new IoPoint(ApsController, 0, 50, IoModes.Signal)
        {
            Name = "T2DO10",
            Description = "碎料电机开关"
        };

        /// <summary>
        ///  缓冲升降动点动作
        /// </summary>
        public static IoPoint T2DO11 = new IoPoint(ApsController, 0, 51, IoModes.Signal)
        {
            Name = "T2DO11",
            Description = "缓冲升降动点动作"
        };

        /// <summary>
        ///  离子棒放电控制
        /// </summary>
        public static IoPoint T2DO12 = new IoPoint(ApsController, 0, 52, IoModes.Signal)
        {
            Name = "T2DO12",
            Description = "离子棒放电控制"
        };

        /// <summary>
        ///   备用
        /// </summary>
        public static IoPoint T2DO13 = new IoPoint(ApsController, 0, 53, IoModes.Signal)
        {
            Name = "T2DO13",
            Description = "备用"
        };
        /// <summary>
        /// 备用
        /// </summary>
        public static IoPoint T2DO14 = new IoPoint(ApsController, 0, 54, IoModes.Signal)
        {
            Name = "T2DO14",
            Description = "备用"
        };

        /// <summary>
        ///   备用
        /// </summary>
        public static IoPoint T2DO15 = new IoPoint(ApsController, 0, 55, IoModes.Signal)
        {
            Name = "T2DO15",
            Description = "备用"
        };

        /// <summary>
        ///  启动按钮灯
        /// </summary>
        public static IoPoint T2DO16 = new IoPoint(ApsController, 0, 56, IoModes.Signal)
        {
            Name = "T2DO16",
            Description = "启动按钮灯"
        };

        /// <summary>
        ///  停止按钮灯
        /// </summary>
        public static IoPoint T2DO17 = new IoPoint(ApsController, 0, 57, IoModes.Signal)
        {
            Name = "T2DO17",
            Description = "停止按钮灯"
        };

        /// <summary>
        ///    复位按钮灯
        /// </summary>
        public static IoPoint T2DO18 = new IoPoint(ApsController, 0, 58, IoModes.Signal)
        {
            Name = "T2DO18",
            Description = "复位按钮灯"
        };

        /// <summary>
        ///  暂停按钮灯
        /// </summary>
        public static IoPoint T2DO19 = new IoPoint(ApsController, 0, 59, IoModes.Signal)
        {
            Name = "T2DO19",
            Description = "暂停按钮灯"
        };

        /// <summary>
        /// 报警清除按钮灯
        /// </summary>
        public static IoPoint T2DO20 = new IoPoint(ApsController, 0, 60, IoModes.Signal)
        {
            Name = "T2DO20",
            Description = "报警清除按钮灯"
        };

        /// <summary>
        ///  红灯
        /// </summary>
        public static IoPoint T2DO21 = new IoPoint(ApsController, 0, 61, IoModes.Signal)
        {
            Name = "T2DO21",
            Description = "红灯"
        };

        /// <summary>
        ///  黄灯
        /// </summary>
        public static IoPoint T2DO22 = new IoPoint(ApsController, 0, 62, IoModes.Signal)
        {
            Name = "T2DO22",
            Description = "黄灯"
        };

        /// <summary>
        ///  绿灯
        /// </summary>
        public static IoPoint T2DO23 = new IoPoint(ApsController, 0, 63, IoModes.Signal)
        {
            Name = "T2DO23",
            Description = "绿灯"
        };

        /// <summary>
        ///  蜂鸣器
        /// </summary>
        public static IoPoint T2DO24 = new IoPoint(ApsController, 0, 64, IoModes.Signal)
        {
            Name = "T2DO24",
            Description = "蜂鸣器"
        };

        /// <summary>
        ///  备用
        /// </summary>
        public static IoPoint T2DO25 = new IoPoint(ApsController, 0, 65, IoModes.Signal)
        {
            Name = "T2DO25",
            Description = "备用"
        };

        /// <summary>
        ///   备用
        /// </summary>
        public static IoPoint T2DO26 = new IoPoint(ApsController, 0, 66, IoModes.Signal)
        {
            Name = "T2DO26",
            Description = "备用"
        };

        /// <summary>
        ///  备用
        /// </summary>
        public static IoPoint T2DO27 = new IoPoint(ApsController, 0, 67, IoModes.Signal)
        {
            Name = "T2DO27",
            Description = "备用"
        };

        /// <summary>
        ///  备用
        /// </summary>
        public static IoPoint T2DO28 = new IoPoint(ApsController, 0, 68, IoModes.Signal)
        {
            Name = "T2DO28",
            Description = "备用"
        };

        /// <summary>
        ///   备用
        /// </summary>
        public static IoPoint T2DO29 = new IoPoint(ApsController, 0, 69, IoModes.Signal)
        {
            Name = "T2DO29",
            Description = "备用"
        };

        /// <summary>
        ///  备用
        /// </summary>
        public static IoPoint T2DO30 = new IoPoint(ApsController, 0, 70, IoModes.Signal)
        {
            Name = "T2DO30",
            Description = "备用"
        };

        /// <summary>
        ///   虚似输出端子(请勿接线)
        /// </summary>
        public static IoPoint T2DO31 = new IoPoint(ApsController, 0, 71, IoModes.Signal)
        {
            Name = "T2DO31",
            Description = "虚似输出端子(请勿接线)"
        };

        #endregion
    }
}