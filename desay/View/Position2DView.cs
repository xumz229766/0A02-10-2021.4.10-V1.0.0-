using System;
using System.Threading;
using System.Windows.Forms;
using System.Enginee;
using System.Interfaces;
namespace desay
{
    public partial class Position2DView<T> : UserControl, IRefreshing
    {
        private AxisOperate[] m_AxisOperate;
        private ApsAxis[] m_axis;
        private readonly Action m_SaveValue, m_Location;

        public Position2DView()
        {
            InitializeComponent();
        }

        public Position2DView(ApsAxis[] axis, Action SaveValue, Action Location) : this()
        {
            m_axis = axis;
            m_SaveValue = SaveValue;
            m_Location = Location;
            lblName1.Text = m_axis[0].Name;
            lblName2.Text = m_axis[1].Name;

            m_AxisOperate = new AxisOperate[2]
            {
                new AxisOperate(m_axis[0]),
                new AxisOperate(m_axis[1])
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
            }
        }

        public T[] Point { get; set; }

        private void uc1DPointView_Load(object sender, EventArgs e)
        {

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("是否保存当前位置数据？", "提示", MessageBoxButtons.YesNo) == DialogResult.No) return;
            Point = new T[] { (T)Convert.ChangeType(m_axis[0].CurrentPos, typeof(T)),
                (T)Convert.ChangeType(m_axis[1].CurrentPos, typeof(T)) };
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
        }
    }
}
