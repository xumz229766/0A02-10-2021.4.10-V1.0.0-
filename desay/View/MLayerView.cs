using System;
using System.Enginee;
using System.Interfaces;
using System.Windows.Forms;
using System.Linq;
using System.ToolKit;
using System.ToolKit.Helper;
namespace desay
{
    public partial class MLayerView<T> : UserControl, IRefreshing
    {
        private AxisOperate m_AxisOperate;
        private ApsAxis m_axis;
        private CylinderOperate[] CylinderOperate;
        //private double average, pos;
        public MLayerView()
        {
            InitializeComponent();
        }

        public MLayerView(ApsAxis maxis, CylinderOperate[] mCylinderOperate) : this()
        {
            m_axis = maxis;
            CylinderOperate = mCylinderOperate;
            m_AxisOperate = new AxisOperate(m_axis);
            tableLayoutPanel2.Controls.Add(m_AxisOperate, 0, 0);
        }

        public AxisMoveMode MoveMode
        {
            set
            {
                m_AxisOperate.MoveMode = value;
            }
        }

        private void MLayerView_Load(object sender, EventArgs e)
        {
            if (CylinderOperate != null)
            { foreach (var Cylinder in CylinderOperate) { flowLayoutPanel1.Controls.Add(Cylinder); } }
            numUpMLayerCount.Value = (decimal)Position.Instance.MLayerCount;
            MLayerDistance.Text = Position.Instance.MDistance.ToString();
        }

        public void Refreshing()
        {
            lblMLayerStartPosition.Text = Position.Instance.MStartPosition.ToString("0000.000");
            MStandbyPos.Text = Position.Instance.MStandbyPosition.ToString("0000.000");
            if(MStandbyPosOpen.Checked != Position.Instance.MStandbyPositionOpen )
            {
                MStandbyPosOpen.Checked = Position.Instance.MStandbyPositionOpen;
            }

            m_AxisOperate.Refreshing();
            if (CylinderOperate != null)
            {
                foreach (var Cylinder in CylinderOperate)
                {
                    Cylinder.Refreshing();
                }
            }
        }

        private void btnMLayerStartPosition_Click(object sender, EventArgs e)
        {
            if (m_axis.IsAlarmed || m_axis.IsEmg || !m_axis.IsServon)
            {
                return;
            }

            m_axis.MoveTo(Position.Instance.MStartPosition, new VelocityCurve(0, m_axis.Speed ?? 0, 0));
        }

        private void btnMLayerStartPositionGet_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("是否保存当前位置数据？", "提示", MessageBoxButtons.YesNo) == DialogResult.No)
            {
                return;
            }

            Position.Instance.MStartPosition = m_axis.CurrentPos;
            Marking.ModifyParameterMarker = true;
        }

        private void MLayerDistanceSave_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("是否保存当前位置数据？", "提示", MessageBoxButtons.YesNo) == DialogResult.No)
            {
                return;
            }

            Position.Instance.MDistance = Convert.ToDouble(MLayerDistance.Text);
            Marking.ModifyParameterMarker = true;
        }

        private void btnMLayerCurrentIndex_Click(object sender, EventArgs e)
        {
            if (m_axis.IsAlarmed || m_axis.IsEmg || !m_axis.IsServon)
            {
                return;
            }

            var MLpos = Position.Instance.MStartPosition + Position.Instance.MDistance * ((int)ndnMIndex.Value - 1);
            m_axis.MoveTo(MLpos, new VelocityCurve(0, m_axis.Speed ?? 0, 0));
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("是否保存当前位置数据？", "提示", MessageBoxButtons.YesNo) == DialogResult.No)
            {
                return;
            }

            Position.Instance.MLayerCount = (int)numUpMLayerCount.Value;
            Marking.ModifyParameterMarker = true;
        }

        private void btnMStandbyPos_Click(object sender, EventArgs e)
        {
            if (m_axis.IsAlarmed || m_axis.IsEmg || !m_axis.IsServon)
            {
                return;
            }

            m_axis.MoveTo(Position.Instance.MStandbyPosition, new VelocityCurve(0, m_axis.Speed ?? 0, 0));
        }

        private void btnMStandbySave_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("是否保存当前位置数据？", "提示", MessageBoxButtons.YesNo) == DialogResult.No)
            {
                return;
            }

            Position.Instance.MStandbyPosition = m_axis.CurrentPos;
            Marking.ModifyParameterMarker = true;
        }

        private void MStandbyPosOpen_CheckedChanged(object sender, EventArgs e)
        {
            Position.Instance.MStandbyPositionOpen = MStandbyPosOpen.Checked;
            Marking.ModifyParameterMarker = true;
        }
    }
}
