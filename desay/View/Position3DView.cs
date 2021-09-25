using System;
using System.Threading;
using System.Windows.Forms;

using System.Enginee;
using System.Interfaces;
using System.ToolKit;
namespace desay
{
    public partial class Position3DView<T> : UserControl, IRefreshing
    {
        private AxisOperate[] m_AxisOperate;
        private ApsAxis[] m_axis;
        private readonly Action m_SaveValue, m_Location;
        private CylinderOperate[] CylinderOperate;       
        public Position3DView()
        {
            InitializeComponent();
        }
        public Position3DView(ApsAxis[] axis, CylinderOperate[] mCylinderOperate, Action SaveValue, Action Location) : this()
        {
            m_axis = axis;
            m_SaveValue = SaveValue;
            m_Location = Location;
            lblName1.Text = m_axis[0].Name;
            lblName2.Text = m_axis[1].Name;
            lblName3.Text = m_axis[2].Name;
            CylinderOperate = mCylinderOperate;           
           
            m_AxisOperate = new AxisOperate[3]
            {
                new AxisOperate(m_axis[0], m_axis[2]),
                new AxisOperate(m_axis[1], m_axis[2]),
                new AxisOperate(m_axis[2])
            };

            foreach (var tempaxis in m_AxisOperate)
                flpView.Controls.Add(tempaxis);
        }
        public Position3DView(ApsAxis[] axis, Action SaveValue, Action Location) : this()
        {
            m_axis = axis;
            m_SaveValue = SaveValue;
            m_Location = Location;
            lblName1.Text = m_axis[0].Name;
            lblName2.Text = m_axis[1].Name;
            lblName3.Text = m_axis[2].Name;          
            m_AxisOperate = new AxisOperate[3]
            {
                new AxisOperate(m_axis[0], m_axis[2]),
                new AxisOperate(m_axis[1], m_axis[2]),
                new AxisOperate(m_axis[2])
            };

            foreach (var tempaxis in m_AxisOperate)
                flpView.Controls.Add(tempaxis);
        }

        public AxisMoveMode MoveMode
        {
            set
            {
                m_AxisOperate[0].MoveMode = value;
                m_AxisOperate[1].MoveMode = value;
                m_AxisOperate[2].MoveMode = value;
            }
        }
        public T[] Point { get; set; }
        private void uc3DPointView_Load(object sender, EventArgs e)
        {
            if (CylinderOperate != null)
            { foreach (var Cylinder in CylinderOperate) { flowLayoutPanel1.Controls.Add(Cylinder); } }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("是否保存当前位置数据？", "提示", MessageBoxButtons.YesNo) == DialogResult.No) return;
            Point = new T[] { (T)Convert.ChangeType(m_axis[0].CurrentPos, typeof(T)),
                (T)Convert.ChangeType(m_axis[1].CurrentPos, typeof(T)),
                (T)Convert.ChangeType(m_axis[2].CurrentPos, typeof(T)) };
            m_SaveValue?.Invoke();
        }

        private void btnGoto_Click(object sender, EventArgs e)
        {
            m_Location?.Invoke();
        }

        public void Refreshing()
        {
            foreach (var axis in m_AxisOperate) axis.Refreshing();
            lblPointX.Text = ((double)Convert.ChangeType(Point[0], typeof(double))).ToString("0.000");
            lblPointY.Text = ((double)Convert.ChangeType(Point[1], typeof(double))).ToString("0.000");
            lblPointZ.Text = ((double)Convert.ChangeType(Point[2], typeof(double))).ToString("0.000");
            if (CylinderOperate != null)
            {
                foreach (var Cylinder in CylinderOperate)
                {
                    Cylinder.Refreshing();
                }
            }
        }
    }
}
