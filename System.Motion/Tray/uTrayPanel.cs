using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.ToolKit;
namespace System.Tray
{
    public partial class uTrayPanel : UserControl
    {
        bool bFlag = false;//添加屏蔽标志
        bool bRemoveFlag = false;//移除屏蔽标志
        bool bShowModel = true;//显示模式
        Color backColor = Color.Black;//背景色
        private SynchronizationContext context = null;
        int Row = 3;
        int Col = 3;
        Tray t = null;
        public void SetTrayObj(Tray tray,Color initColor)
        {
            t = tray;
            Row = t.Row;
            Col = t.Column;
            SetLayout(initColor);
        }
        public bool BRemoveFlag
        {
            get { return bRemoveFlag; }
            set { bRemoveFlag = value;
               
            }
        }

        public bool BFlag
        {
            get { return bFlag; }
            set { bFlag = value; }
        }
        public bool BShowModel
        {
            get { return bShowModel; }
            set { bShowModel = value; }
        }
        public uTrayPanel()
        {
            InitializeComponent();
            context = SynchronizationContext.Current;
        }

        /// <summary>  
        /// 动态布局  
        /// </summary>  
        /// <param name="layoutPanel">布局面板</param>  
        /// <param name="Row">行</param>  
        /// <param name="Col">列</param>  
        private void DynamicLayout(TableLayoutPanel layoutPanel,Color bColor)
        {
            layoutPanel.Controls.Clear();
            layoutPanel.RowStyles.Clear();
            layoutPanel.ColumnStyles.Clear();
            layoutPanel.RowCount = Row+1;    //设置分成几行  
            float pRow = Convert.ToSingle((100 / Row).ToString("0.00"));
            float pColumn = Convert.ToSingle((100 / Col).ToString("0.00"));
            for (int i = 0; i < Row; i++)
            {
                layoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 15));
            }
            layoutPanel.ColumnCount = Col+1;    //设置分成几列  
            for (int i = 0; i < Col; i++)
            {
                layoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 15));
            }
            for (int i = 0; i < Row; i++)
            {

                for (int j = 0; j < Col; j++)
                {
                    uButton btn = new uButton();
                    btn.Anchor = AnchorStyles.None;
                    btn.Margin = new Padding(0, 0, 0, 0);
                    btn.Dock = DockStyle.Fill;
                    btn.BackColor = Color.Black;
                    btn.Font=new Font("宋体", 5.0F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(134)));
                    btn.Name = i.ToString()+"_"+j.ToString();
                    Index pos = new Index(i, j);                   
                    int num = t.FindPos(pos);
                    if(num > -1)
                     t.dic_Index[num].SetColor(bColor);//初始化颜色
                    //如果是显示模式
                    if (bShowModel)
                    {                      
                        //屏蔽的点不显示名称
                        if (num < 0)
                        {
                            btn.Text = "";
                            SetBtnColor(btn, backColor);
                        }
                        else
                        {
                            //不屏蔽的点显示顺序号
                            btn.Text = num.ToString();
                            SetBtnColor(btn, bColor);
                        }

                    }
                    else
                    {
                        if (num < 0)
                        {
                            SetBtnColor(btn, backColor);
                        }
                        else {

                            SetBtnColor(btn,bColor);
                        }
                        btn.Text = i.ToString() + "," + j.ToString();
                       
                    }
                   
                    btn.Click += new EventHandler(this.label_Click);
                    layoutPanel.Controls.Add(btn,j,i);
                }
               
            }
        }
        private void SetBtnColor(uButton btn,Color color)
        {
            if (color == Color.Yellow)
            {
                btn.LanternBackground = Color.Yellow;

            }else if (color == Color.Green)
            {
                btn.LanternBackground = Color.Green;

            }else if (color == Color.Blue)
            {
                btn.LanternBackground = Color.Blue;
            }
            else if (color == Color.Red)
            {
                btn.LanternBackground = Color.Red;
            }
            else if (color == Color.Gray)
            {
                btn.LanternBackground = Color.Gray;
            }
            else
            {
                btn.LanternBackground = color;
            }
        }
        public void SetLayout(Color bColor)
        {
            t.SortTray(t.StartPose,t.Direction,t.ChangeLineType);
            DynamicLayout(tableLayoutPanel1, bColor);
        }
       
        private void label_Click(object sender, EventArgs e)
        {
            var btn = (uButton)sender;
            var str = btn.Name.Split('_');
            var locate = new Coordinate();
            locate.Row = int.Parse(str[0]);
            locate.Column = int.Parse(str[1]);
            Locate = locate;
            if (bFlag && !bShowModel)
            {
                SetBtnColor(btn, backColor);
                t.AddEmptyPos(btn.Name);
            }
            if (bRemoveFlag && !bShowModel)
            {
                SetBtnColor(btn, Color.Blue);
                t.RemoveEmptyPos(btn.Name);
            }
        }
        public void CreateUnRegular1()
        {
            if (bShowModel)
                return;
            for (int i = 0; i < Row; i++)
            {
                for (int j = 0; j < Col; j++) {

                    if (i % 2 == 0)
                    {
                        if (j % 2 == 0)
                        {
                            uButton btn = (uButton)tableLayoutPanel1.GetControlFromPosition(j, i);
                            SetBtnColor(btn, backColor);
                            t.AddEmptyPos(btn.Name);
                        }
                        else {
                            uButton btn = (uButton)tableLayoutPanel1.GetControlFromPosition(j, i);
                            SetBtnColor(btn, Color.Blue);
                            t.RemoveEmptyPos(btn.Name);
                        }
                    }
                    else {
                        if (j % 2 == 1)
                        {
                            uButton btn = (uButton)tableLayoutPanel1.GetControlFromPosition(j, i);
                            SetBtnColor(btn, backColor);
                            t.AddEmptyPos(btn.Name);
                        }
                        else
                        {
                            uButton btn = (uButton)tableLayoutPanel1.GetControlFromPosition(j, i);
                            SetBtnColor(btn, Color.Blue);
                            t.RemoveEmptyPos(btn.Name);
                        }
                    }
                
                }
            }
        }
        public void CreateUnRegular2()
        {
            if (bShowModel)
                return;

            for (int i = 0; i < Row; i++)
            {
                for (int j = 0; j < Col; j++)
                {

                    if (i % 2 == 0)
                    {
                        if (j % 2 == 1)
                        {
                            uButton btn = (uButton)tableLayoutPanel1.GetControlFromPosition(j, i);
                            SetBtnColor(btn, backColor);
                            t.AddEmptyPos(btn.Name);
                        }
                        else
                        {
                            uButton btn = (uButton)tableLayoutPanel1.GetControlFromPosition(j, i);
                            SetBtnColor(btn, Color.Blue);
                            t.RemoveEmptyPos(btn.Name);
                        }
                    }
                    else
                    {
                        if (j % 2 == 0)
                        {
                            uButton btn = (uButton)tableLayoutPanel1.GetControlFromPosition(j, i);
                            SetBtnColor(btn, backColor);
                            t.AddEmptyPos(btn.Name);
                        }
                        else
                        {
                            uButton btn = (uButton)tableLayoutPanel1.GetControlFromPosition(j, i);
                            SetBtnColor(btn, Color.Blue);
                            t.RemoveEmptyPos(btn.Name);
                        }
                    }

                }
            }
        }
        public Coordinate Locate { get;private set; }
        public void UpdateColor()
        {
            context.Post(updateUI, null);
            
        }
        private void updateUI2(object obj)
        {
  
            foreach (KeyValuePair<int, Index> pair in t.dic_Index)
            {
                int col = pair.Value.Col;
                int row = pair.Value.Row;
                uButton btn = (uButton)tableLayoutPanel1.GetControlFromPosition(col, row);
                SetBtnColor(btn, pair.Value.color);
            }
        }
        private void updateUI(object obj)
        {
            if (t.dic_Index.Count > 0)
            {
                Dictionary<int, Index> tempDictionary = new Dictionary<int, Index>(t.dic_Index);
                int[] tempKeyList = tempDictionary.Keys.ToArray<int>();
                for (int i = 0; i < tempKeyList.Length; i++)
                {

                    int col = tempDictionary[tempKeyList[i]].Col;
                    int row = tempDictionary[tempKeyList[i]].Row;
                    uButton btn = (uButton)tableLayoutPanel1.GetControlFromPosition(col, row);
                    SetBtnColor(btn, tempDictionary[tempKeyList[i]].color);
                }
            }
        }
    }
    public struct Coordinate
    {
        public int Row;
        public int Column;
    }
}
