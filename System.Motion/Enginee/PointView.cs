using System.Windows.Forms;
using System.ToolKit;
namespace System.Enginee
{
    public partial class PointView : UserControl
    {
        public PointView()
        {
            InitializeComponent();
        }
        public Point<double> Point
        {
            set
            {
                lblGetProductX.Text = value.X.ToString("0.000");
                lblGetProductY.Text = value.Y.ToString("0.000");
            }
        }
    }
}
