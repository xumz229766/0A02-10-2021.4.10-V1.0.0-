using System.Windows.Forms;
using System.ToolKit;
namespace System.Enginee
{
    public partial class Point3DView : UserControl
    {
        public Point3DView()
        {
            InitializeComponent();
        }
        public Point3D<double> Point
        {
            set
            {
                lblGetProductX.Text = value.X.ToString("0.000");
                lblGetProductY.Text = value.Y.ToString("0.000");
                lblGetProductZ.Text = value.Z.ToString("0.000");
            }
        }
    }
}
