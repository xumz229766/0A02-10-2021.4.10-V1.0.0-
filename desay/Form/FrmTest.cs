using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace desay
{
    public partial class FrmTest : Form
    {
        private Splice m_Splice;
        private Buffer m_Buffer;
        private Feeder m_Feeder;
        private Move m_Move;
        private LeftC m_LeftC;
        private LeftCut1 m_LeftCut1;
        private LeftCut2 m_LeftCut2;
        private RightCut1 m_RightCut1;
        private RightCut2 m_RightCut2;
        private Platform m_Platform;
        private Storing m_Storing;
        public FrmTest(Splice Splice, Buffer Buffer, Feeder Feeder, Move Move, LeftC LeftC,
           LeftCut1 LeftCut1, LeftCut2 LeftCut2, RightCut1 RightCut1, RightCut2 RightCut2, Platform Platform, Storing Storing)
        {
            InitializeComponent();
            m_Splice = Splice;
            m_Buffer = Buffer;
            m_Feeder = Feeder;
            m_Move = Move;
            m_LeftC = LeftC;
            m_LeftCut1 = LeftCut1;
            m_LeftCut2 = LeftCut2;
            m_RightCut1 = RightCut1;
            m_RightCut2 = RightCut2;
            m_Platform = Platform;
            m_Storing = Storing;
        }

        private void FrmTest_Load(object sender, EventArgs e)
        {
            modelOperate1.StationIni = m_Splice.stationInitialize;
            modelOperate1.StationOpe = m_Splice.stationOperate;
            modelOperate2.StationIni = m_Buffer.stationInitialize;
            modelOperate2.StationOpe = m_Buffer.stationOperate;
            modelOperate3.StationIni = m_Feeder.stationInitialize;
            modelOperate3.StationOpe = m_Feeder.stationOperate;
            modelOperate4.StationIni = m_Move.stationInitialize;
            modelOperate4.StationOpe = m_Move.stationOperate;
            modelOperate5.StationIni = m_LeftC.stationInitialize;
            modelOperate5.StationOpe = m_LeftC.stationOperate;
            modelOperate6.StationIni = m_LeftCut1.stationInitialize;
            modelOperate6.StationOpe = m_LeftCut1.stationOperate;
            modelOperate7.StationIni = m_LeftCut2.stationInitialize;
            modelOperate7.StationOpe = m_LeftCut2.stationOperate;
            modelOperate8.StationIni = m_RightCut1.stationInitialize;
            modelOperate8.StationOpe = m_RightCut1.stationOperate;
            modelOperate9.StationIni = m_RightCut2.stationInitialize;
            modelOperate9.StationOpe = m_RightCut2.stationOperate;
            modelOperate10.StationIni = m_Platform.stationInitialize;
            modelOperate10.StationOpe = m_Platform.stationOperate;
            modelOperate11.StationIni = m_Storing.stationInitialize;
            modelOperate11.StationOpe = m_Storing.stationOperate;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            modelOperate1.Refreshing();           
            modelOperate2.Refreshing();           
         //   modelOperate3.Refreshing();
            modelOperate4.Refreshing();
            modelOperate5.Refreshing();
            modelOperate6.Refreshing();
            modelOperate7.Refreshing();
            modelOperate8.Refreshing();
            modelOperate9.Refreshing();
            modelOperate10.Refreshing();
            modelOperate11.Refreshing();
        }
    }
}
