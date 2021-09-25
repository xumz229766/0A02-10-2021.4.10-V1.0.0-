using System;
using System.Windows.Forms;
using System.ToolKit;

namespace desay
{
    public partial class frmPasswordChange : Form
    {
       

        public frmPasswordChange(UserLevel userLevel)
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;

            comboBox_UserSelect.Items.Add(userLevel.ToString());           
            comboBox_UserSelect.SelectedIndex = 0;
        }
        private void button_Yes_Click(object sender, EventArgs e)
        {
            LogHelper.Info("密码变更操作");
            if (comboBox_UserSelect.Text == UserLevel.设备厂商.ToString())
            {
                if (MD5.TextToMd5(textBox_OldPassword.Text.Trim()) == Config.Instance.ManufacturerPassword)
                {
                    if (textBox_NewPassword.Text.Trim() == textBox_ConfirmPassword.Text.Trim())
                    {
                        Config.Instance.ManufacturerPassword = MD5.TextToMd5(textBox_NewPassword.Text.Trim());
                        LogHelper.Debug("密码修改成功");
                        MessageBox.Show("密码修改成功");
                    }
                    else
                    {
                        LogHelper.Debug("新密码与确认密码不一致，请重新输入");
                        MessageBox.Show("新密码与确认密码不一致，请重新输入");
                    }
                }
                else
                {
                    LogHelper.Debug("原始密码输入错误，请重新输入");
                    MessageBox.Show("原始密码输入错误，请重新输入");
                }
            }
            else if (comboBox_UserSelect.Text == UserLevel.设计者.ToString())
            {
                if (MD5.TextToMd5(textBox_OldPassword.Text.Trim()) == Config.Instance.AdminPassword)
                {
                    if (textBox_NewPassword.Text.Trim() == textBox_ConfirmPassword.Text.Trim())
                    {
                        Config.Instance.AdminPassword = MD5.TextToMd5(textBox_NewPassword.Text.Trim());
                        LogHelper.Debug("密码修改成功");
                        MessageBox.Show("密码修改成功");
                    }
                    else
                    {
                        LogHelper.Debug("新密码与确认密码不一致，请重新输入");
                        MessageBox.Show("新密码与确认密码不一致，请重新输入");
                    }
                }
                else
                {
                    LogHelper.Debug("原始密码输入错误，请重新输入");
                    MessageBox.Show("原始密码输入错误，请重新输入");   
                }
            }
            else if (comboBox_UserSelect.Text == UserLevel.操作员.ToString())
            {
                if (MD5.TextToMd5(textBox_OldPassword.Text.Trim()) == Config.Instance.OperatePassword)
                {
                    if (textBox_NewPassword.Text.Trim() == textBox_ConfirmPassword.Text.Trim())
                    {
                        Config.Instance.OperatePassword = MD5.TextToMd5(textBox_NewPassword.Text.Trim());
                        LogHelper.Debug("密码修改成功");
                        MessageBox.Show("密码修改成功"); 
                    }
                    else
                    {
                        LogHelper.Debug("新密码与确认密码不一致，请重新输入");
                        MessageBox.Show("新密码与确认密码不一致，请重新输入");
                    }
                }
                else
                {
                    LogHelper.Debug("原始密码输入错误，请重新输入");
                    MessageBox.Show("原始密码输入错误，请重新输入");
                }
            }
            if (comboBox_UserSelect.Text == UserLevel.工程师.ToString())
            {
                if (MD5.TextToMd5(textBox_OldPassword.Text.Trim()) == Config.Instance.EngineerPassword)
                {
                    if (textBox_NewPassword.Text.Trim() == textBox_ConfirmPassword.Text.Trim())
                    {
                        Config.Instance.EngineerPassword = MD5.TextToMd5(textBox_NewPassword.Text.Trim());
                        LogHelper.Debug("密码修改成功");
                        MessageBox.Show("密码修改成功");
                    }
                    else
                    {
                        LogHelper.Debug("新密码与确认密码不一致，请重新输入");
                        MessageBox.Show("新密码与确认密码不一致，请重新输入");
                    }
                }
                else
                {
                    LogHelper.Debug("原始密码输入错误，请重新输入");
                    MessageBox.Show("原始密码输入错误，请重新输入");
                }
            }
            else
            {
                LogHelper.Debug("用户操作类型非法!");
                MessageBox.Show("用户操作类型非法!");
                return;
            }
        }
        private void button_No_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
