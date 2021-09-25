using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.ToolKit
{
    /// <summary>
    /// 用户等级
    /// </summary>
    public enum UserLevel 
    {
        None,
       操作员,
       工程师,
       设计者,
       设备厂商
    };

    public struct ArcParam<T>
    {
        public T X;
        public T Y;
        public T R;
        public int DIR;
    }
    public struct Point3D<T>
    {
        public T X;
        public T Y;
        public T Z;
        public override string ToString()
        {
            return X.ToString() + "," + Y.ToString() + "," + Z.ToString();
        }
        public static Point3D<T> Parse(string str)
        {
            string[] strValue = str.Split(',');
            var point3D = new Point3D<T>();
            point3D.X = (T)Convert.ChangeType(strValue[0], typeof(T));
            point3D.Y = (T)Convert.ChangeType(strValue[1], typeof(T));
            point3D.Z = (T)Convert.ChangeType(strValue[2], typeof(T));
            return point3D;
        }
    }
    public struct Point<T>
    {
        public T X;
        public T Y;
        public override string ToString() => X.ToString() + "," + Y.ToString();
        public static Point<T> Parse(string str)
        {
            var pos = new Point<T>();
            string[] strValue = str.Split(',');
            pos.X = (T)Convert.ChangeType(strValue[0], typeof(T));
            pos.Y = (T)Convert.ChangeType(strValue[1], typeof(T));
            return pos;
        }
    }


}
