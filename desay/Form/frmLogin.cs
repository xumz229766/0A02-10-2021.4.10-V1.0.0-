using System;
using System.Windows.Forms;
using System.ToolKit;

namespace desay
{
    public partial class frmLogin : Form
    {
        #region 变量
       
        private int i;
        #endregion

        #region 构造函数
        public   frmLogin()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;
        }

     
        #endregion

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (cbxUers.Text == UserLevel.设备厂商.ToString())
            {
                if (MD5.TextToMd5(txtPassword.Text) == Config.Instance.ManufacturerPassword)
                {
                    Config.Instance.userLevel = UserLevel.设备厂商;
                    Config.Instance.userName = cbxUers.Text;
                    LogHelper.Info(string.Format("用户 {0} 已登录", Config.Instance.userName));
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("请输入正确的用户名与密码");
                }
            }
            if (cbxUers.Text == UserLevel.设计者.ToString())
            {
                if (MD5.TextToMd5(txtPassword.Text) == Config.Instance.AdminPassword)
                {
                    Config.Instance.userLevel = UserLevel.设计者;
                    Config.Instance.userName = cbxUers.Text;
                    LogHelper.Info(string.Format("用户 {0} 已登录", Config.Instance.userName));
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("请输入正确的用户名与密码");
                }
            }
            if (cbxUers.Text == UserLevel.工程师.ToString())
            {
                if (MD5.TextToMd5(txtPassword.Text) == Config.Instance.EngineerPassword)
                {
                    Config.Instance.userLevel = UserLevel.工程师;
                    Config.Instance.userName = cbxUers.Text;
                    LogHelper.Info(string.Format("用户 {0} 已登录", Config.Instance.userName));
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("请输入正确的用户名与密码");
                }
            }
            if (cbxUers.Text == UserLevel.操作员.ToString())
            {
                if (MD5.TextToMd5(txtPassword.Text) == Config.Instance.OperatePassword)
                {
                    Config.Instance.userLevel = UserLevel.操作员;
                    Config.Instance.userName = cbxUers.Text;
                    LogHelper.Info(string.Format("用户 {0} 已登录", Config.Instance.userName));
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                    MessageBox.Show("请输入正确的用户名与密码");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.No;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            //new frmPasswordChange().ShowDialog();
        }

        private void frmLogin_Load(object sender, EventArgs e)
        {
            if (Config.Instance.userLevel == UserLevel.None) btnCancel.Enabled = true;
            cbxUers.Items.Add(UserLevel.操作员.ToString());
            cbxUers.Items.Add(UserLevel.工程师.ToString());
            cbxUers.Items.Add(UserLevel.设计者.ToString());
            cbxUers.Items.Add(UserLevel.设备厂商.ToString());
            cbxUers.SelectedIndex = 0;
        }

        private void txtPassword_TextChanged(object sender, EventArgs e)
        {
           if (txtPassword .Text == "5201314")
            {
                btnCancel.Visible = true;
            }
        }
    }
}
