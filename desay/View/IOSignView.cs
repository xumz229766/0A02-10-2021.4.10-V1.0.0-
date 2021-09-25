using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Interfaces;
namespace desay
{
    public partial class IOSignView : UserControl
    {
        private readonly IoPoint _signalPoint;
        public IOSignView()
        {
            InitializeComponent();
        }
        public IOSignView(IoPoint ioPoint):this()
        {
            _signalPoint = ioPoint;
        }
        public void StateRefresh()
        {
            pic.Image = _signalPoint.Value ? Properties.Resources.LedGreen : Properties.Resources.LedRed;
            lblName.Text = _signalPoint.Name;
            lblDes.Text = _signalPoint.Description;
        }
    }
}
