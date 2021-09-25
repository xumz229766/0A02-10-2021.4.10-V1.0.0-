using System;
using System.Enginee;
using System.Threading;
using System.ToolKit;
using System.ToolKit.Helper;
using System.Tray;
using System.Windows.Forms;


namespace desay
{
    public partial class frmParameter : Form
    {

        public frmParameter()
        {
            InitializeComponent();
        }
       
        private void btnNew_Click(object sender, EventArgs e)
        {
            LogHelper.Info("新增型号操作");
            for (var i = 0; i < Config.Instance.ProductType.Count; i++)
            {
                if (txtProductType.Text.Trim() == Config.Instance.ProductType[i])
                {
                    MessageBox.Show("有相同型号，不能新增！");
                    return;
                }
            }
            IniFile.CreateDir(txtProductType.Text.Trim());
            Config.Instance.ProductType.Add(txtProductType.Text.Trim());
            Config.Instance.CurrentProductType = txtProductType.Text.Trim();
            lbxProductType.Items.Clear();
            for (var i = 0; i < Config.Instance.ProductType.Count; i++)
            {
                lbxProductType.Items.Add(Config.Instance.ProductType[i]);
            }

        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            LogHelper.Info("删除型号操作");
            for (var i = 0; i < Config.Instance.ProductType.Count; i++)
            {
                if (txtProductType.Text.Trim() == Config.Instance.CurrentProductType)
                {
                    MessageBox.Show("该型号正在使用，不能删除！");
                    return;
                }
                else
                {
                    if (txtProductType.Text.Trim() == Config.Instance.ProductType[i])
                    {
                        Config.Instance.ProductType.Remove(txtProductType.Text.Trim());
                        lbxProductType.Items.Clear();
                        for (var j = 0; j < Config.Instance.ProductType.Count; j++)
                        {
                            lbxProductType.Items.Add(Config.Instance.ProductType[j]);
                        }

                        return;
                    }
                }
            }
        }

        private void btnSelectProductType_Click(object sender, EventArgs e)
        {
            LogHelper.Info("型号切换");
            DialogResult result = MessageBox.Show("是否保存前一型号的数据？", "提示", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                SerializerManager<Config>.Instance.Save(AppConfig.ConfigFileName, Config.Instance);
            }
            Thread.Sleep(20);
            Global.BigTray = null;
            //Global.ConeTray1 = new Tray[Global.LensTray.Column * Global.LensTray.Row];
            //for (int i = 0; i < Global.ConeTray1.Length; i++)
            //{
            //    Global.ConeTray1[i] = null;
            //}
           
            Config.Instance.CurrentProductType = lbxProductType.Text;
            lblCurrentProductType.Text = Config.Instance.CurrentProductType;
            AppConfig.ProductPath = Config.Instance.CurrentProductType;
            TrayFactory.LoadTrayFactory(AppConfig.ConfigTrayName);
            Global.BigTray = TrayFactory.GetTrayFactory(Config.Instance.BigPlateID);
            Global.SmallTray = TrayFactory.GetTrayFactory(Config.Instance.YoungPlateID);
            Delay.Instance = SerializerManager<Delay>.Instance.Load(AppConfig.ConfigDelayName);
            AxisParameter.Instance = SerializerManager<AxisParameter>.Instance.Load(AppConfig.ConfigAxisName);
            Thread.Sleep(200);
            Position.Instance = SerializerManager<Position>.Instance.Load(AppConfig.ConfigPositionName);
          
        }

        private void frmParameter_Load(object sender, EventArgs e)
        {
            lbxProductType.Items.Clear();
            for (var i = 0; i < Config.Instance.ProductType.Count; i++)
            {
                lbxProductType.Items.Add(Config.Instance.ProductType[i]);
            }

            lblCurrentProductType.Text = Config.Instance.CurrentProductType;          
        }
    }
}
