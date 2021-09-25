using System.Drawing;
using System.ToolKit;
using System.Windows.Forms;
namespace System.Tray
{
    public partial class TestTray : Form
    {
        private Tray tray = null;//当前托盘对象
        private TrayPanel tp;
        string[] ID = new string[] { "8Hole", "12Hole", "16Hole", "20Hole", "24Hole", "32Hole" };
        string STID = "SpecialTray";


        public TestTray()
        {
            //初始化托盘 
            InitializeComponent();
        }

        private void TestTray_Load(object sender, EventArgs e)
        {
            radioButton2.Checked = true;
            init(false,false);

            timer1.Enabled = true;
        }

        private void Refreshing()
        {
            var Keys = TrayFactory.GetTrayDict.Keys;
            //sskey = Keys;
            var count = Keys.Count;
            cmbId.Items.Clear();
            bool stTrue = radioButton3.Checked;
            bool seTrue = radioButton1.Checked;
            bool isHave = false;
            foreach (var key in Keys)
            {
                isHave = false;
                if (stTrue)
                {
                    if (key == STID)
                    {
                        cmbId.Items.Add(key);
                    }
                }
                else if(seTrue)
                {
                    foreach (var id in ID)
                    {
                        if (key == id)
                        {
                            cmbId.Items.Add(key);
                        }
                    }
                }
                else
                {
                    foreach (var id in ID)
                    {
                        if (key == id)
                        {
                            isHave = true;
                        }
                    }
                    if (!isHave) { cmbId.Items.Add(key); }
                }

            }
        }

        private void init(bool stTrue, bool seTrue)
        {
            //var i = 1;
            var Keys = TrayFactory.GetTrayDict.Keys;
            //sskey = Keys;
            var count = Keys.Count;
            cmbId.Items.Clear();
            bool isHave=false;
            foreach (var key in Keys)
            {
                isHave = false;
                if (stTrue)
                {
                    if (key == STID)
                    {
                        cmbId.Items.Add(key);
                    }
                }
                else if (seTrue)
                {
                    foreach (var id in ID)
                    {
                        if (key == id)
                        {
                            cmbId.Items.Add(key);
                        }
                    }                
                }
                else
                {
                    foreach (var id in ID)
                    {
                        if (key == id)
                        {
                            isHave = true;
                        }
                    }
                    if (!isHave) { cmbId.Items.Add(key); }                  
                }
             
            }
            cmbStart.SelectedIndex = 0;
            cmbDirect.SelectedIndex = 0;
            cmbChangeLine.SelectedIndex = 0;
            tp = new TrayPanel();
            tp.Dock = DockStyle.Fill;
            panel1.Controls.Clear();
            panel1.Controls.Add(tp);
            if (count > 0)
            {
                cmbId.SelectedIndex = 0;
            }
            else
            {
                return;
            }

            tray = TrayFactory.GetTrayFactory(cmbId.Text);
            tp.SetTrayObj(tray, Color.Gray);
            initControls();
            chShow.Checked = true;
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            if (cmbId.Items.Count <= 0)
            {
                return;
            }

            cbUnregular1.Checked = false;
            cbUnregular2.Checked = false;
            chShow.Checked = false;
            tray = new Tray(cmbId.Text, txtName.Text, (int)nudRow.Value, (int)nudCol.Value);
            ndnFinalBaseIndex.Value = 1;
            ndnFinalRowIndex.Value = (int)nudRow.Value;
            ndnFinalColumnIndex.Value = (int)nudRow.Value * ((int)nudCol.Value - 1) + 1;
            tp.SetTrayObj(tray, Color.Blue);
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            tp.BFlag = chxAddSheild.Checked;
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            tp.BRemoveFlag = chxRemoveSheild.Checked;
        }

        private void chShow_CheckedChanged(object sender, EventArgs e)
        {
            if (tray == null)
            {
                MessageBox.Show("当前盘为空，请先创建托盘！");
                return;
            }
            tp.BShowModel = chShow.Checked;
            tray.StartPose = (EStartPos)Enum.Parse(typeof(EStartPos), cmbStart.Text);
            tray.Direction = (EIndexDirect)Enum.Parse(typeof(EIndexDirect), cmbDirect.Text);
            if (chShow.Checked)
            {
                tp.SetLayout(Color.Blue);
            }
            else
            {
                tp.SetLayout(Color.Blue);
            }

            panel3.Enabled = chShow.Checked;
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            tray.StartPose = (EStartPos)Enum.Parse(typeof(EStartPos), cmbStart.Text);
            tray.Direction = (EIndexDirect)Enum.Parse(typeof(EIndexDirect), cmbDirect.Text);
            tray.ChangeLineType = (EChangeLine)Enum.Parse(typeof(EChangeLine), cmbChangeLine.Text);
            tp.SetLayout(Color.Blue);
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            var cout = cmbId.Items.Count;
            if (cout <= 0)
            {
                return;
            }

            tray = TrayFactory.GetTrayFactory(cmbId.Text);
            tp.SetTrayObj(tray, Color.Blue);
            initControls();
            chShow.Checked = true;
        }
        //根据选择的托盘对象初始化值
        private void initControls()
        {
            if (tray != null)
            {
                try
                {
                    txtName.Text = tray.Name;
                    nudRow.Value = tray.Row;
                    nudCol.Value = tray.Column;
                    cmbStart.Text = tray.StartPose.ToString();
                    cmbDirect.Text = tray.Direction.ToString();
                    cmbChangeLine.Text = tray.ChangeLineType.ToString();
                    ndnFinalBaseIndex.Value = tray.BaseIndex;
                    ndnFinalRowIndex.Value = tray.RowIndex;
                    ndnFinalColumnIndex.Value = tray.ColumnIndex;
                    ndnRowXoffset.Value = (decimal)tray.RowXoffset;
                    ndnRowYoffset.Value = (decimal)tray.RowYoffset;
                    ndnColumnXoffset.Value = (decimal)tray.ColumnXoffset;
                    ndnColumnYoffset.Value = (decimal)tray.ColumnYoffset;
                }
                catch (Exception ex) { }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveParameter();
            TrayFactory.SetTray(cmbId.Text, tray);
            if (TrayFactory.SaveTrayFactory(AppConfig.ConfigTrayName))
            {
                MessageBox.Show("保存托盘参数成功!");
            }
            else
            {
                MessageBox.Show("保存托盘参数失败!");
            }

            tray.IsCalibration = false;
        }

        private void cbUnregular1_CheckedChanged(object sender, EventArgs e)
        {

            if (chShow.Checked && cbUnregular1.Checked)
            {
                MessageBox.Show("当前为显示模式，无法更改！");
                cbUnregular1.Checked = false;
                return;
            }
            if (cbUnregular1.Checked)
            {
                cbUnregular2.Checked = false;
                tp.CreateUnRegular1();
            }
        }

        private void cbUnregular2_CheckedChanged(object sender, EventArgs e)
        {
            if (chShow.Checked && cbUnregular2.Checked)
            {
                MessageBox.Show("当前为显示模式，无法更改！");
                cbUnregular2.Checked = false;
                return;
            }
            if (cbUnregular2.Checked)
            {
                cbUnregular1.Checked = false;
                tp.CreateUnRegular2();
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("是否新建托盘型号？", "新建托盘型号", MessageBoxButtons.OKCancel);
            if (result == DialogResult.Cancel)
            {
                return;
            }

            var strType = txtNewPlateType.Text.Trim();
            if (strType == "")
            {
                MessageBox.Show("输入的字符为空！");
                return;
            }
            if (!IsDigitOrNumber(strType))
            {
                MessageBox.Show("输入的字符不是0-9,a-z,A-Z");
                return;
            }
            var Keys = TrayFactory.GetTrayDict.Keys;
            var count = Keys.Count;
            foreach (var key in Keys)
            {
                if (strType == key)
                {
                    MessageBox.Show("输入的型号已存在！");
                    return;
                }
            }
            var m_tray = new Tray(strType, "", 5, 5);
            TrayFactory.SetTray(strType, m_tray);
            cmbId.Items.Add(strType);
            cmbId.SelectedIndex = cmbId.Items.Count - 1;
            tray = TrayFactory.GetTrayFactory(cmbId.Text);
            tp.SetTrayObj(tray, Color.Gray);
            initControls();
            chShow.Checked = true;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("是否删除当前托盘型号？", "删除托盘型号", MessageBoxButtons.OKCancel);
            if (result == DialogResult.Cancel)
            {
                return;
            }

            IniFile.EraseSection(cmbId.Text, AppConfig.ConfigTrayName);
            TrayFactory.RemoveTray(cmbId.Text);
            cmbId.Items.Remove(cmbId.Text);            
            if (cmbId.Items.Count <= 0)
            {
                return;
            }

            cmbId.SelectedIndex = 0;
            tray = TrayFactory.GetTrayFactory(cmbId.Text);
            tp.SetTrayObj(tray, Color.Gray);
            initControls();
            chShow.Checked = true;
        }

        private bool IsDigitOrNumber(string str)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(str, @"(?i)^[0-9a-zA-Z]+$"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (tray != null)
            {
                lblIsCalibration.Text = tray.IsCalibration ? "已标定" : "未标定";
                lblIsCalibration.ForeColor = tray.IsCalibration ? Color.Green : Color.Red;
            }
        }

        #region 标定位置
        private void SaveParameter()
        {
            tray.BaseIndex = (int)ndnFinalBaseIndex.Value;

            tray.RowIndex = (int)ndnFinalRowIndex.Value;

            tray.ColumnIndex = (int)ndnFinalColumnIndex.Value;

            tray.RowXoffset = (double)ndnRowXoffset.Value;

            tray.RowYoffset = (double)ndnRowYoffset.Value;

            tray.ColumnXoffset = (double)ndnColumnXoffset.Value;

            tray.ColumnYoffset = (double)ndnColumnYoffset.Value;
        }
        #endregion

        private void TestTray_Enter(object sender, EventArgs e)
        {
            Refreshing();
        }

        private void RadioButton1_CheckedChanged(object sender, EventArgs e)
        {
            Refreshing();
        }

        private void RadioButton2_CheckedChanged(object sender, EventArgs e)
        {
            Refreshing();
        }
    }
}
