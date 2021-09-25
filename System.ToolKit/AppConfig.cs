using System.IO;

namespace System.ToolKit
{
    public class AppConfig
    {
        public static string ProductPath;
        public static string ConfigFileName => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Config\\Config.xml");

        /// <summary>
        /// 卡配置路径
        /// </summary>
        public static string ConfigCardIniName => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Config\\ConfigCardIni.ini");
        public static string ConfigCardEniName => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Config\\ConfigCardEni.eni");
        public static string ConfigAxisIniName => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Config\\ConfigAxisIni.ini");
        /// <summary>
        /// 保存托盘的路径（带型号）
        /// </summary>
        public static string ConfigTrayName => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Config\\Tray.ini");
        public static string ConfigTrayName1 => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Config\\Tray.xml");
        /// <summary>
        /// 保存延时的路径（带型号）
        /// </summary>
        public static string ConfigDelayName => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Config\\" + ProductPath + "\\Delay.xml");
        /// <summary>
        /// 保存轴参数的路径
        /// </summary>
        public static string ConfigAxisName => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Config\\" + ProductPath + "\\AxisParam.xml");
        /// <summary>
        /// 位置路径（带型号）
        /// </summary>
        public static string ConfigPositionName => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Config\\" + ProductPath + "\\Position.xml");
        public static string LogFileName => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Log\\" + DateTime.Now.ToString("yyyy-MM-dd") + ".log");
        public static string ProductDataFileName => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ProductData.ini");
        /// <summary>
        /// 生产信息文件路径
        /// </summary>
        public static string ProdutionInfoFileName => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ProdutionInfo.ini");
        /// <summary>
        /// MES配置参数文件路径
        /// </summary>
        public static string MESConfigFileName => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "MESConfig.ini");
        /// <summary>
        /// 本地数据库文件夹路径
        /// </summary>
        public static string dataBaseDirectoryPath => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "DataBase");
        /// <summary>
        /// 本地数据库文件路径
        /// </summary>
        public static string dataBaseFileName => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, string.Format("{0}.db3", DateTime.Now.ToString("yyyyMMdd")));
    }
}
