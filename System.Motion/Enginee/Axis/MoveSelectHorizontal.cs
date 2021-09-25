using System;
using System.Windows.Forms;

namespace System.Enginee
{
    public partial class MoveSelectHorizontal : UserControl
    {
        public MoveSelectHorizontal()
        {
            InitializeComponent();
            rbnContinueMoveSelect.Checked = true;
            rbnPos1um.Checked = true;
        }
        public AxisMoveMode MoveMode { get;private set; }
        private void rbnContinueMoveSelect_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton temp = sender as RadioButton;
            if (temp == null || temp.Name != "rbnContinueMoveSelect") return;
            if (!rbnContinueMoveSelect.Checked) return;
            var mode = new AxisMoveMode();
            mode = MoveMode;
            mode.Continue = true;
            MoveMode = mode;
        }

        private void rbnLocationMoveSelect_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton temp = sender as RadioButton;
            if (temp == null || temp.Name != "rbnLocationMoveSelect") return;
            if (!rbnLocationMoveSelect.Checked) return;
            var mode = new AxisMoveMode();
            mode = MoveMode;
            mode.Continue = false;
            MoveMode = mode;
        }
        private void rbnPos1um_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton temp = sender as RadioButton;
            if (temp == null || temp.Name != "rbnPos1um") return;
            var mode = new AxisMoveMode();
            mode = MoveMode;
            mode.Distance = 0.001;
            MoveMode = mode;
        }
        private void rbnPos10um_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton temp = sender as RadioButton;
            if (temp == null || temp.Name != "rbnPos10um") return;
            var mode = new AxisMoveMode();
            mode = MoveMode;
            mode.Distance = 0.01;
            MoveMode = mode;
        }
        private void rbnPos100um_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton temp = sender as RadioButton;
            if (temp == null || temp.Name != "rbnPos100um") return;
            var mode = new AxisMoveMode();
            mode = MoveMode;
            mode.Distance = 0.10;
            MoveMode = mode;
        }
        private void rbnPos1000um_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton temp = sender as RadioButton;
            if (temp == null || temp.Name != "rbnPos1000um") return;
            var mode = new AxisMoveMode();
            mode = MoveMode;
            mode.Distance = 1.00;
            MoveMode = mode;
        }
    }
}
