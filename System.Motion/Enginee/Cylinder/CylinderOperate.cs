using System;
using System.Drawing;
using System.Windows.Forms;
using System.Interfaces;

namespace System.Enginee
{
    public partial class CylinderOperate : UserControl
    {
        private readonly Action _manualOperate;
        private Cylinder m_cylinder;
        public CylinderOperate()
        {
            InitializeComponent();
        }
        public CylinderOperate(Action ManualOperate) : this()
        {
            _manualOperate = ManualOperate;
        }
        public CylinderOperate(Cylinder cylinder) : this()
        {
            _manualOperate = () => cylinder.ManualExecute();
            m_cylinder = cylinder;
            CylinderName = cylinder.Name;
        }

        public string CylinderName
        {
            set
            {
                btn.Text = value;
            }
        }
        public bool InOrigin
        {
            set
            {
                if (value)
                    picOrigin.BackColor = Color.Green;
                else
                    picOrigin.BackColor = Color.Red;
            }
        }
        public bool InMove
        {
            set
            {
                if (value)
                    picMove.BackColor = Color.Green;
                else
                    picMove.BackColor = Color.Red;
            }
        }
        public bool OutMove
        {
            set
            {
                btn.BackColor = value ? Color.PaleGreen : Color.Transparent;
            }
        }
        private void btn_Click(object sender, EventArgs e)
        {
            _manualOperate();
        }

        public void Refreshing()
        {
            InOrigin = m_cylinder.OutOriginStatus;
            InMove = m_cylinder.OutMoveStatus;
            OutMove = m_cylinder.IsOutMove;
        }
    }
}
