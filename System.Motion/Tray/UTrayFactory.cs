using System;
using System.Collections.Generic;
using System.ToolKit;
namespace System.Tray
{
    /// <summary>
    /// 托盘工厂模式
    /// </summary>
    /// <typeparam name="T">泛型:int,uint,long,double,float,short</typeparam>
    public class UTrayFactory
    {
        /// <summary>
        /// 存放所有托盘对象的集合
        /// </summary>
        public static Dictionary<string, UTray> TrayDictionary = new Dictionary<string, UTray>();
        /// <summary>
        /// 通过指定字符串获取对象
        /// </summary>
        /// <param name="key">字符串</param>
        /// <returns>返回托盘对象</returns>
        public static UTray GetTrayFactory(string key) => TrayDictionary.ContainsKey(key) ? TrayDictionary[key] : null;
        public static Dictionary<string, UTray> GetTrayDict
        {
            get
            {
                return TrayDictionary;
            }
        }
        /// <summary>
        /// 获取托盘的个数
        /// </summary>
        /// <returns>返回托盘个数</returns>
        public static int GetTrayCount() => TrayDictionary.Count;
        /// <summary>
        /// 设置或添加对象
        /// </summary>
        /// <param name="key">托盘对应的字符串</param>
        /// <param name="tray">托盘对象</param>
        public static void SetTray(string key, UTray tray)
        {
            if (TrayDictionary.ContainsKey(key)) TrayDictionary[key] = tray;
            else TrayDictionary.Add(key, tray);
        }
        public static void RemoveTray(string key)
        {
            TrayDictionary.Remove(key);
        }
        /// <summary>
        /// 读取文件初始化对象
        /// </summary>
        /// <param name="strPath"></param>
        public static void LoadTrayFactory(string strPath)
        {
            TrayDictionary.Clear();
            var sectionList = IniFile.GetAllSectionNames(strPath);
            foreach (var section in sectionList)
            {
                var type1 = IniFile.ReadValue(section, "type1", strPath, "");
                var type = IniFile.ReadValue(section, "Id", strPath, "1");
                var strName = IniFile.ReadValue(section, "Name", strPath, "托盘" + section);
                var Row = IniFile.ReadValue(section, "Row", strPath, 10);
                var Column = IniFile.ReadValue(section, "Column", strPath, 10);
                var strStart = IniFile.ReadValue(section, "StartPose", strPath, "左上角");
                var strDirect = IniFile.ReadValue(section, "Direction", strPath, "行");
                var strChangeLineType = IniFile.ReadValue(section, "ChangeLineType", strPath, "Z");
                var strEmpty = IniFile.ReadValue(section, "Empty", strPath, "");
                var bIsCalibration = IniFile.ReadValue(section, "IsCalibration", strPath, false);
                var iRowXoffset = IniFile.ReadValue(section, "RowXoffset", strPath, 0.0);
                var iRowYoffset = IniFile.ReadValue(section, "RowYoffset", strPath, 0.0);
                var iColumnXoffset = IniFile.ReadValue(section, "ColumnXoffset", strPath, 0.0);
                var iColumnYoffset = IniFile.ReadValue(section, "ColumnYoffset", strPath, 0.0);
                var iBaseIndex = IniFile.ReadValue(section, "BaseIndex", strPath, 1);
                var iBasePosition = Point3D<double>.Parse(IniFile.ReadValue(section, "BasePosition", strPath, "0,0,0"));
                var iRowIndex = IniFile.ReadValue(section, "RowIndex", strPath, 1);
                var iRowPosition = Point3D<double>.Parse(IniFile.ReadValue(section, "RowPosition", strPath, "0,0,0"));
                var iColumnIndex = IniFile.ReadValue(section, "ColumnIndex", strPath, 1);
                var iColumnPosition = Point3D<double>.Parse(IniFile.ReadValue(section, "ColumnPosition", strPath, "0,0,0"));
                var tray = new UTray(type, strName, Row, Column)
                {
                    IsCalibration = bIsCalibration,
                    RowXoffset = iRowXoffset,
                    RowYoffset = iRowYoffset,
                    ColumnXoffset = iColumnXoffset,
                    ColumnYoffset = iColumnYoffset,
                    BaseIndex = iBaseIndex,
                    BasePosition = iBasePosition,
                    RowIndex = iRowIndex,
                    RowPosition = iRowPosition,
                    ColumnIndex = iColumnIndex,
                    ColumnPosition = iColumnPosition
                };
                tray.InitTray(strStart, strDirect, strChangeLineType, strEmpty);
                SetTray(section, tray);
            }
        }
        /// <summary>
        /// 保存所有托盘对象参数到文件
        /// </summary>
        /// <param name="strPath">文件名,以.ini结尾</param>
        /// <returns>返回保存状态</returns>
        public static bool SaveTrayFactory(string strPath)
        {
            var bResult = true;
            foreach (KeyValuePair<string, UTray> m_tray in TrayDictionary)
            {
                UTray tray = m_tray.Value;
                var strSection = m_tray.Key;
                bResult &= IniFile.WriteValue(strSection, "Id", tray.Type.ToString(), strPath) != 0;
                bResult &= IniFile.WriteValue(strSection, "Name", tray.Name, strPath) != 0;
                bResult &= IniFile.WriteValue(strSection, "Row", tray.Row.ToString(), strPath) != 0;
                bResult &= IniFile.WriteValue(strSection, "Column", tray.Column.ToString(), strPath) != 0;
                bResult &= IniFile.WriteValue(strSection, "StartPose", tray.StartPose.ToString(), strPath) != 0;
                bResult &= IniFile.WriteValue(strSection, "Direction", tray.Direction.ToString(), strPath) != 0;
                bResult &= IniFile.WriteValue(strSection, "ChangeLineType", tray.ChangeLineType.ToString(), strPath) != 0;
                bResult &= IniFile.WriteValue(strSection, "Empty", tray.Empty, strPath) != 0;
                bResult &= IniFile.WriteValue(strSection, "IsCalibration", tray.IsCalibration.ToString(), strPath) != 0;
                bResult &= IniFile.WriteValue(strSection, "RowXoffset", tray.RowXoffset.ToString(), strPath) != 0;
                bResult &= IniFile.WriteValue(strSection, "RowYoffset", tray.RowYoffset.ToString(), strPath) != 0;
                bResult &= IniFile.WriteValue(strSection, "ColumnXoffset", tray.ColumnXoffset.ToString(), strPath) != 0;
                bResult &= IniFile.WriteValue(strSection, "ColumnYoffset", tray.ColumnYoffset.ToString(), strPath) != 0;
                bResult &= IniFile.WriteValue(strSection, "BaseIndex", tray.BaseIndex.ToString(), strPath) != 0;
                bResult &= IniFile.WriteValue(strSection, "BasePosition", tray.BasePosition.ToString(), strPath) != 0;
                bResult &= IniFile.WriteValue(strSection, "RowIndex", tray.RowIndex.ToString(), strPath) != 0;
                bResult &= IniFile.WriteValue(strSection, "RowPosition", tray.RowPosition.ToString(), strPath) != 0;
                bResult &= IniFile.WriteValue(strSection, "ColumnIndex", tray.ColumnIndex.ToString(), strPath) != 0;
                bResult &= IniFile.WriteValue(strSection, "ColumnPosition", tray.ColumnPosition.ToString(), strPath) != 0;
            }
            return bResult;
        }

        /// <summary>
        /// 标定计算
        /// </summary>
        /// <param name="key">键</param>
        /// <returns></returns>
        public static bool Calibration(string key)
        {
            Tray tray = TrayFactory.GetTrayFactory(key);
            if (tray == null) throw new Exception("托盘不存在！");
            var retR12 = (tray.dic_Index[tray.RowIndex].Row - tray.dic_Index[tray.BaseIndex].Row) != 0;
            var retC12 = (tray.dic_Index[tray.RowIndex].Col - tray.dic_Index[tray.BaseIndex].Col) != 0;
            var retR13 = (tray.dic_Index[tray.ColumnIndex].Row - tray.dic_Index[tray.BaseIndex].Row) != 0;
            var retC13 = (tray.dic_Index[tray.ColumnIndex].Col - tray.dic_Index[tray.BaseIndex].Col) != 0;
            if ((retR12 == retR13) || (retC12 == retC13)) throw new Exception("三点重合，或者三点再同一直线上！");
            if ((retR12 == retC12) || (retR13 == retC13)) throw new Exception("三点无法形成直角坐标系，非有效点！");
            var iRow = 0;
            var iColumn = 0;
            double detaRowX, detaRowY, detaColX, detaColY;
            if (retR12 && !retR13)
            {
                iRow = tray.dic_Index[tray.RowIndex].Row - tray.dic_Index[tray.BaseIndex].Row;
                iColumn = tray.dic_Index[tray.ColumnIndex].Col - tray.dic_Index[tray.BaseIndex].Col;
                detaRowX = tray.RowPosition.X - tray.BasePosition.X;
                detaRowY = tray.RowPosition.Y - tray.BasePosition.Y;
                detaColX = tray.ColumnPosition.X - tray.BasePosition.X;
                detaColY = tray.ColumnPosition.Y - tray.BasePosition.Y;
            }
            else
            {
                iRow = tray.dic_Index[tray.ColumnIndex].Row - tray.dic_Index[tray.BaseIndex].Row;
                iColumn = tray.dic_Index[tray.RowIndex].Col - tray.dic_Index[tray.BaseIndex].Col;
                detaColX = tray.RowPosition.X - tray.BasePosition.X;
                detaColY = tray.RowPosition.Y - tray.BasePosition.Y;
                detaRowX = tray.ColumnPosition.X - tray.BasePosition.X;
                detaRowY = tray.ColumnPosition.Y - tray.BasePosition.Y;
            }
            tray.RowXoffset = detaRowX / iRow;
            tray.RowYoffset = detaRowY / iRow;
            tray.ColumnXoffset = detaColX / iColumn;
            tray.ColumnYoffset = detaColY / iColumn;
            tray.IsCalibration = true;
            return true;
        }

    }
}
