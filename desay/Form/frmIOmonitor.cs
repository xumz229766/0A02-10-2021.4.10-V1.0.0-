using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Interfaces;
namespace desay
{
    public partial class frmIOmonitor : Form
    {
        private IoPoint[] Input;
        private IoPoint[] Output;
        public frmIOmonitor()
        {
            InitializeComponent();
        }
        private void frmIOmonitor_Load(object sender, EventArgs e)
        {
            Input = new IoPoint[]
            {
                IoPoints.T1IN0,IoPoints.T1IN1,IoPoints.T1IN2,IoPoints.T1IN3,
                IoPoints.T1IN4,IoPoints.T1IN5,IoPoints.T1IN6,IoPoints.T1IN7,
                IoPoints.T1IN8,IoPoints.T1IN9,IoPoints.T1IN10,IoPoints.T1IN11,
                IoPoints.T1IN12,IoPoints.T1IN13,IoPoints.T1IN14,IoPoints.T1IN15,
                IoPoints.T1IN16,IoPoints.T1IN17,IoPoints.T1IN18,IoPoints.T1IN19,
                IoPoints.T1IN20,IoPoints.T1IN21,IoPoints.T1IN22,IoPoints.T1IN23,
                IoPoints.T1IN24,IoPoints.T1IN25,IoPoints.T1IN26,IoPoints.T1IN27,
                IoPoints.T1IN28,IoPoints.T1IN29,IoPoints.T1IN30,IoPoints.T1IN31,
                IoPoints.T2IN0,IoPoints.T2IN1,IoPoints.T2IN2,IoPoints.T2IN3,
                IoPoints.T2IN4,IoPoints.T2IN5,IoPoints.T2IN6,IoPoints.T2IN7,
                IoPoints.T2IN8,IoPoints.T2IN9,IoPoints.T2IN10,IoPoints.T2IN11,
                IoPoints.T2IN12,IoPoints.T2IN13,IoPoints.T2IN14,IoPoints.T2IN15,
                IoPoints.T2IN16,IoPoints.T2IN17,IoPoints.T2IN18,IoPoints.T2IN19,
                IoPoints.T2IN20,IoPoints.T2IN21,IoPoints.T2IN22,IoPoints.T2IN23,
                IoPoints.T2IN24,IoPoints.T2IN25,IoPoints.T2IN26,IoPoints.T2IN27,
                IoPoints.T2IN28,IoPoints.T2IN29,IoPoints.T2IN30,IoPoints.T2IN31,
                IoPoints.T3IN0,IoPoints.T3IN1,IoPoints.T3IN2,IoPoints.T3IN3,
                IoPoints.T3IN4,IoPoints.T3IN5,IoPoints.T3IN6,IoPoints.T3IN7,
                IoPoints.T3IN8,IoPoints.T3IN9,IoPoints.T3IN10,IoPoints.T3IN11,
                IoPoints.T3IN12,IoPoints.T3IN13,IoPoints.T3IN14,IoPoints.T3IN15,
                IoPoints.T3IN16,IoPoints.T3IN17,IoPoints.T3IN18,IoPoints.T3IN19,
                IoPoints.T3IN20,IoPoints.T3IN21,IoPoints.T3IN22,IoPoints.T3IN23,
                IoPoints.T3IN24,IoPoints.T3IN25,IoPoints.T3IN26,IoPoints.T3IN27,
                IoPoints.T3IN28,IoPoints.T3IN29,IoPoints.T3IN30,IoPoints.T3IN31,
            };
            Output = new IoPoint[]
            {
                IoPoints.T1DO0,IoPoints.T1DO1,IoPoints.T1DO2,IoPoints.T1DO3,
                IoPoints.T1DO4,IoPoints.T1DO5,IoPoints.T1DO6,IoPoints.T1DO7,
                IoPoints.T1DO8,IoPoints.T1DO9,IoPoints.T1DO10,IoPoints.T1DO11,
                IoPoints.T1DO12,IoPoints.T1DO13,IoPoints.T1DO14,IoPoints.T1DO15,
                IoPoints.T1DO16,IoPoints.T1DO17,IoPoints.T1DO18,IoPoints.T1DO19,
                IoPoints.T1DO20,IoPoints.T1DO21,IoPoints.T1DO22,IoPoints.T1DO23,
                IoPoints.T1DO24,IoPoints.T1DO25,IoPoints.T1DO26,IoPoints.T1DO27,
                IoPoints.T1DO28,IoPoints.T1DO29,IoPoints.T1DO30,IoPoints.T1DO31,
                IoPoints.T2DO0,IoPoints.T2DO1,IoPoints.T2DO2,IoPoints.T2DO3,
                IoPoints.T2DO4,IoPoints.T2DO5,IoPoints.T2DO6,IoPoints.T2DO7,
                IoPoints.T2DO8,IoPoints.T2DO9,IoPoints.T2DO10,IoPoints.T2DO11,
                IoPoints.T2DO12,IoPoints.T2DO13,IoPoints.T2DO14,IoPoints.T2DO15,
                IoPoints.T2DO16,IoPoints.T2DO17,IoPoints.T2DO18,IoPoints.T2DO19,
                IoPoints.T2DO20,IoPoints.T2DO21,IoPoints.T2DO22,IoPoints.T2DO23,
                IoPoints.T2DO24,IoPoints.T2DO25,IoPoints.T2DO26,IoPoints.T2DO27,
                IoPoints.T2DO28,IoPoints.T2DO29,IoPoints.T2DO30,IoPoints.T2DO31,
            };
            InitdgvInputViewRows();
            InitdgvOutputViewRows();
            timer1.Enabled = true;
        }
        private void InitdgvInputViewRows()
        {
            this.dgvInputView.Rows.Clear();
            //in a real scenario, you may need to add different rows
            var i = 1;
            foreach (var di in Input)
            {
                dgvInputView.Rows.Add(new object[] {
                    i.ToString(),
                    di.Value?Properties.Resources.LedGreen:Properties.Resources.LedNone,
                    di.Name,
                    di.Description
                });
                i++;
            }
        }
        private void InitdgvOutputViewRows()
        {
            this.dgvOutputView.Rows.Clear();
            //in a real scenario, you may need to add different rows
            var i = 1;
            foreach (var DO in Output)
            {
                dgvOutputView.Rows.Add(new object[] {
                    i.ToString(),
                    DO.Value?Properties.Resources.LedGreen:Properties.Resources.LedNone,
                    DO.Name,
                    DO.Description
                });
                i++;
            }
        }
        private void refreshdgvInputViewRows()
        {
            //in a real scenario, you may need to add different rows
            var i = 1;
            foreach (var DI in Input)
            {
                dgvInputView.Rows[i-1].SetValues(new object[] {
                    i.ToString(),
                    DI.Value?Properties.Resources.LedGreen:Properties.Resources.LedNone,
                    DI.Name,
                    DI.Description
                });
                i++;
            }
        }
        private void refreshdgvOutputViewRows()
        {
            //in a real scenario, you may need to add different rows
            var i = 1;
            foreach (var DO in Output)
            {
                dgvOutputView.Rows[i - 1].SetValues(new object[] {
                    i.ToString(),
                    DO.Value?Properties.Resources.LedGreen:Properties.Resources.LedNone,
                    DO.Name,
                    DO.Description
                });
                i++;
            }
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Enabled = false;
            refreshdgvInputViewRows();
            refreshdgvOutputViewRows();
            timer1.Enabled = true;
        }
    }
}
