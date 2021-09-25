using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ToolKit;

namespace System.Enginee
{
    /// <summary>
    /// 任意三点以上不在同一直线上的点，计算Tray盘的坐标
    /// </summary>
    public class ArrayCal
    {
        //public Point3D<double> BasePos { get; set; }

        //public PositionOffset DeltaPos { get; set; }
        /////// <summary>
        /////// 教导点
        /////// </summary>
        ////public Dictionary<int,Point3D<double>> TeachPos { get; set; }
        /////// <summary>
        /////// 盘格编号位置
        /////// </summary>
        ////public Dictionary <int,Index> TeachPoint { get; set; }
        //public ArrayCal()
        //{
        //    //TeachPos = new Dictionary<int, Point3D<double>>();
        //    //TeachPoint = new Dictionary<int, Index>();
        //}
        /// <summary>
        /// 计算基准坐标，及各项偏差值
        /// </summary>
        /// <param name="TeachPos">教导坐标集合</param>
        /// <param name="TrayPoint">Tray盘点阵标记值集合</param>
        /// <returns></returns>
        public static PositionOffset? BaseCalculate(Dictionary<int, Point3D<double>> TeachPos, Dictionary<int, Index> TrayPoint)
        {
            //判断教导点数量是否大于等于3，否则计算失败！
            if (TeachPos.Count < 3) return null;
            int[] keys = new int[TeachPos.Count];
            var i = 0;
            foreach (var key in TrayPoint.Keys)
            {
                keys[i] = key;
                i++;
            }
            try
            {
                //排列组合计算
                List<int[]> ListCombination = PermutationAndCombination<int>.GetCombination(keys, 3);
                List<int[]> ListSave = new List<int[]>();
                //找出任意三点不在同一直线上的项
                foreach (var list in ListCombination)
                {
                    var Ra = TrayPoint[list[0]].Row - TrayPoint[list[1]].Row;
                    var Rb = TrayPoint[list[0]].Row - TrayPoint[list[2]].Row;
                    var Ca = TrayPoint[list[0]].Col - TrayPoint[list[1]].Col;
                    var Cb = TrayPoint[list[0]].Col - TrayPoint[list[2]].Col;

                    if (Ra != Rb && Ca != Cb) ListSave.Add(list);
                }
                if (ListSave.Count < 1) return null;
                PositionOffset[] PosOffset = new PositionOffset[ListSave.Count];
                var j = 0;
                foreach (var list in ListSave)
                {
                    //X2-X1=(a2-a1)Cxf+(b2-b1)Rxf
                    //X3-X1=(a3-a1)Cxf+(b3-b1)Rxf
                    //Y2-Y1=(a2-a1)Cyf+(b2-b1)Ryf
                    //Y3-Y1=(a3-a1)Cyf+(b3-b1)Ryf

                    //若a2-a1==0
                    var DeltaRa = TrayPoint[list[1]].Row - TrayPoint[list[0]].Row;
                    //若b2-b1==0
                    var DeltaRb = TrayPoint[list[1]].Col - TrayPoint[list[0]].Col;
                    //若a3-a1==0
                    var DeltaCa = TrayPoint[list[2]].Row - TrayPoint[list[0]].Row;
                    //若b3-b1==0
                    var DeltaCb = TrayPoint[list[2]].Col - TrayPoint[list[0]].Col;
                    //若a2-a1==0
                    if (DeltaRa == 0 && DeltaRb != 0 && DeltaCa != 0 && DeltaCb != 0)
                    {
                        PosOffset[j].Cxf = (TeachPos[list[1]].X - TeachPos[list[0]].X) / DeltaRb;
                        PosOffset[j].Cyf = (TeachPos[list[1]].Y - TeachPos[list[0]].Y) / DeltaRb;
                        PosOffset[j].Rxf = (TeachPos[list[2]].X - TeachPos[list[0]].X - PosOffset[j].Cxf * DeltaCb) / DeltaCa;
                        PosOffset[j].Ryf = (TeachPos[list[2]].Y - TeachPos[list[0]].Y - PosOffset[j].Cyf * DeltaCb) / DeltaCa;
                    }
                    //若b2-b1==0
                    if (DeltaRa != 0 && DeltaRb == 0 && DeltaCa != 0 && DeltaCb != 0)
                    {
                        PosOffset[j].Rxf = (TeachPos[list[1]].X - TeachPos[list[0]].X) / DeltaRa;
                        PosOffset[j].Ryf = (TeachPos[list[1]].Y - TeachPos[list[0]].Y) / DeltaRa;
                        PosOffset[j].Cxf = (TeachPos[list[2]].X - TeachPos[list[0]].X - PosOffset[j].Rxf * DeltaCa) / DeltaCb;
                        PosOffset[j].Cyf = (TeachPos[list[2]].Y - TeachPos[list[0]].Y - PosOffset[j].Ryf * DeltaCa) / DeltaCb;
                    }
                    //若a3-a1==0
                    if (DeltaRa != 0 && DeltaRb != 0 && DeltaCa == 0 && DeltaCb != 0)
                    {
                        PosOffset[j].Cxf = (TeachPos[list[2]].X - TeachPos[list[0]].X) / DeltaCb;
                        PosOffset[j].Cyf = (TeachPos[list[2]].Y - TeachPos[list[0]].Y) / DeltaCb;
                        PosOffset[j].Rxf = (TeachPos[list[1]].X - TeachPos[list[0]].X - PosOffset[j].Cxf * DeltaRb) / DeltaRa;
                        PosOffset[j].Ryf = (TeachPos[list[1]].Y - TeachPos[list[0]].Y - PosOffset[j].Cyf * DeltaRb) / DeltaRa;
                    }
                    //若b3-b1==0
                    if (DeltaRa != 0 && DeltaRb != 0 && DeltaCa != 0 && DeltaCb == 0)
                    {
                        PosOffset[j].Rxf = (TeachPos[list[2]].X - TeachPos[list[0]].X) / DeltaCa;
                        PosOffset[j].Ryf = (TeachPos[list[2]].Y - TeachPos[list[0]].Y) / DeltaCa;
                        PosOffset[j].Cxf = (TeachPos[list[1]].X - TeachPos[list[0]].X - PosOffset[j].Rxf * DeltaRa) / DeltaRb;
                        PosOffset[j].Cyf = (TeachPos[list[1]].Y - TeachPos[list[0]].Y - PosOffset[j].Ryf * DeltaRa) / DeltaRb;
                    }
                    //若a2-a1!=0、b2-b1!=0、a3-a1!=0、b3-b1!=0
                    if (DeltaRa != 0 && DeltaRb != 0 && DeltaCa != 0 && DeltaCb != 0)
                    {
                        var NumeratorX = (TeachPos[list[2]].X - TeachPos[list[0]].X) * DeltaRa - (TeachPos[list[1]].X - TeachPos[list[0]].X) * DeltaCa;
                        var NumeratorY = (TeachPos[list[2]].Y - TeachPos[list[0]].Y) * DeltaRa - (TeachPos[list[1]].Y - TeachPos[list[0]].Y) * DeltaCa;
                        var Denominator = DeltaRa * DeltaCb - DeltaCa * DeltaRb;

                        PosOffset[j].Cxf = NumeratorX / Denominator;
                        PosOffset[j].Cyf = NumeratorY / Denominator;
                        PosOffset[j].Rxf = (TeachPos[list[1]].X - TeachPos[list[0]].X - PosOffset[j].Cxf * DeltaRb) / DeltaRa;
                        PosOffset[j].Ryf = (TeachPos[list[1]].Y - TeachPos[list[0]].Y - PosOffset[j].Cyf * DeltaRb) / DeltaRa;
                    }

                    //计算出起始点坐标
                    PosOffset[j].BasePos.X = TeachPos[list[0]].X - TrayPoint[list[0]].Row * PosOffset[j].Rxf - TrayPoint[list[0]].Col * PosOffset[j].Cxf;
                    PosOffset[j].BasePos.Y = TeachPos[list[0]].Y - TrayPoint[list[0]].Row * PosOffset[j].Ryf - TrayPoint[list[0]].Col * PosOffset[j].Cyf;
                    PosOffset[j].BasePos.Z = TeachPos[list[0]].Z;
                }
                var Num = ListSave.Count;
                double TotalRxf=0, TotalRyf=0, TotalCxf=0, TotalCyf=0;
                Point3D<double> TotalPos=new Point3D<double>();
                for(var k=0;k<Num;k++)
                {
                    TotalRxf += PosOffset[k].Rxf;
                    TotalRyf += PosOffset[k].Ryf;
                    TotalCxf += PosOffset[k].Cxf;
                    TotalCyf += PosOffset[k].Cyf;
                    TotalPos.X += PosOffset[k].BasePos.X;
                    TotalPos.Y += PosOffset[k].BasePos.Y;
                    TotalPos.Z += PosOffset[k].BasePos.Z;
                }
                //求各项数据的平均值
                var Value = new PositionOffset();
                Value.Rxf = TotalRxf / Num;
                Value.Ryf = TotalRyf / Num;
                Value.Cxf = TotalCxf / Num;
                Value.Cyf = TotalCyf / Num;
                Value.BasePos.X = TotalPos.X / Num;
                Value.BasePos.Y = TotalPos.Y / Num;
                Value.BasePos.Z = TotalPos.Z / Num;
                return Value;
            }
            catch(Exception)
            {
                return null;
            }
        }

        public static Point3D<double>? PosistionCalculate(PositionOffset BasePosition,Index index)
        {
            var pos = new Point3D<double>();
            try
            {
                pos.X = BasePosition.BasePos.X + index.Row * BasePosition.Rxf + index.Col * BasePosition.Cxf;
                pos.Y = BasePosition.BasePos.Y + index.Row * BasePosition.Ryf + index.Col * BasePosition.Cyf;
                pos.X = BasePosition.BasePos.Z;
            }
            catch(Exception)
            {
                return null;
            }
            return pos;
        }
    }
    /// <summary>
    /// 偏差及坐标
    /// </summary>
    public struct PositionOffset
    {
        /// <summary>
        /// 基准点
        /// </summary>
        public Point3D<double> BasePos;
        /// <summary>
        /// 列方向上x偏差 Column x dir offset
        /// </summary>
        public double Cxf;
        /// <summary>
        /// 列方向上y偏差 Column y dir offset
        /// </summary>
        public double Cyf;
        /// <summary>
        /// 行方向上x偏差 Row x dir offset
        /// </summary>
        public double Rxf;
        /// <summary>
        /// 行方向上y偏差 Row y dir offset
        /// </summary>
        public double Ryf;
    }
}
