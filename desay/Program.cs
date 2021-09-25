using System;
using System.Windows.Forms;
using System.ToolKit;
using System.ToolKit.Helper;
using System.Threading;
using System.Tray;
namespace desay
{
    static class Program
    {

        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            bool isRunning;
            using (System.Threading.Mutex mutex = new System.Threading.Mutex(true, Application.ProductName, out isRunning))
            {
                if (isRunning)
                {
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);

                    Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
                    Application.ThreadException += new System.Threading.ThreadExceptionEventHandler(UI_ThreadException);//处理UI线程异常
                    AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);//处理非UI线程异常

                    //加载配置文件
                    Thread.Sleep(200);
                    Config.Instance = SerializerManager<Config>.Instance.Load(AppConfig.ConfigFileName);
                    AppConfig.ProductPath = Config.Instance.CurrentProductType;
                    TrayFactory.LoadTrayFactory(AppConfig.ConfigTrayName);
                    Global.BigTray = TrayFactory.GetTrayFactory(Config.Instance.BigPlateID);
                    Global.SmallTray = TrayFactory.GetTrayFactory(Config.Instance.YoungPlateID);
                    Global.SpecialTray = TrayFactory.GetTrayFactory(Config.Instance.SpecialPlateID);
                    Delay.Instance = SerializerManager<Delay>.Instance.Load(AppConfig.ConfigDelayName);
                    AxisParameter.Instance = SerializerManager<AxisParameter>.Instance.Load(AppConfig.ConfigAxisName);

                    Position.Instance = SerializerManager<Position>.Instance.Load(AppConfig.ConfigPositionName);
                    Application.Run(new frmMain());
                }
                else
                {
                    MessageBox.Show("程序已经启动！");
                }
            }
        }
        /// <summary>
        /// 处理UI线程异常
        /// </summary>
        static void UI_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            DialogResult result = MessageBox.Show(e.Exception.ToString(), "发生未捕获主线程异常", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //Global.isErrorExit = true;
            if (result == DialogResult.OK)
            {
                SerializerManager<Config>.Instance.Save(AppConfig.ConfigFileName, Config.Instance);
                SerializerManager<Delay>.Instance.Save(AppConfig.ConfigDelayName, Delay.Instance);
                SerializerManager<AxisParameter>.Instance.Save(AppConfig.ConfigAxisName, AxisParameter.Instance);
                Thread.Sleep(200);
                SerializerManager<Position>.Instance.Save(AppConfig.ConfigPositionName, Position.Instance);
            }
            LogHelper.Fatal(e.Exception.Message);
            Application.Exit();
        }
        /// <summary>
        /// 处理非UI线程异常
        /// </summary>
        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            DialogResult result = MessageBox.Show(e.ExceptionObject.ToString(), "发生未捕获多线程异常", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //Global.isErrorExit = true;
            if (result == DialogResult.OK)
            {
                SerializerManager<Config>.Instance.Save(AppConfig.ConfigFileName, Config.Instance);
                SerializerManager<Delay>.Instance.Save(AppConfig.ConfigDelayName, Delay.Instance);
                SerializerManager<AxisParameter>.Instance.Save(AppConfig.ConfigAxisName, AxisParameter.Instance);
                Thread.Sleep(200);
                SerializerManager<Position>.Instance.Save(AppConfig.ConfigPositionName, Position.Instance);
            }
            LogHelper.Fatal(e.ExceptionObject.ToString());
            Application.Exit();
        }
    }
}
