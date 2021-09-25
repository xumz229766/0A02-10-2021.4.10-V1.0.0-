using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace desay
{
    public enum SpliceAlarm:int
    {
        无消息,
        无杆接料气缸复位中,
        无料超时报警,
        接料异常报警
    }
    public enum BufferAlarm : int
    {
        无消息,
        缓冲升降气缸复位中,
        缓冲左右气缸复位中,
        缓冲夹子气缸复位中
    }
    public enum FeederAlarm : int
    {
        无消息,
        进料气缸复位中
    }
    public enum MoveAlarm : int
    {
        无消息,
        移料上下气缸复位中,
        移料左右气缸复位中,
        移料夹子气缸复位中,
        碎料气缸复位中,
        排料气缸复位中,
        碎料盖子气缸复位中,
        C轴推进气缸不在原点
    }
    public enum LeftCAlarm : int
    {
        无消息,
        角度计算异常,
        推进1轴复位中,
        推进2轴复位中,
        推进3轴复位中,
        推进4轴复位中,
        C1轴复位中,
        C2轴复位中,
        C3轴复位中,
        C4轴复位中
    }
    public enum Left1CutAlarm : int
    {
        无消息,
        Z1力矩剪切未切断,
        Z1闭合位需大于缓冲位,
        Z1前后气缸复位中,
        Z1翻转气缸复位中,
        Z1夹爪气缸复位中,
        Z1剪切轴复位中
    }
    public enum Left2CutAlarm : int
    {
        无消息,
        Z2力矩剪切未切断,
        Z2闭合位需大于缓冲位,
        Z2前后气缸复位中,
        Z2翻转气缸复位中,
        Z2夹爪气缸复位中,
        Z2剪切轴复位中
    }
    public enum Right1CutAlarm : int
    {
        无消息,
        Z3力矩剪切未切断,
        Z3闭合位需大于缓冲位,
        Z3前后气缸复位中,
        Z3翻转气缸复位中,
        Z3夹爪气缸复位中,
        Z3剪切轴复位中
    }
    public enum Right2CutAlarm : int
    {
        无消息,
        Z4力矩剪切未切断,
        Z4闭合位需大于缓冲位,
        Z4前后气缸复位中,
        Z4翻转气缸复位中,
        Z4夹爪气缸复位中,
        Z4剪切轴复位中
    }
   
    public enum PlateformAlarm : int
    {
        无消息,
        T2IN26摆盘前感应故障,
        T2IN27摆盘前感应故障,
        T2IN28摆盘后感应故障,
        T2IN29摆盘后感应故障,
        T2IN18左卡盘感应故障,
        T2IN19右卡盘感应故障,
        获取Tray盘坐标点错误,
        抽检大盘穴数超出最大穴数,
        取放盘气缸没有复位,
        取放盘上下气缸复位中,
        Z轴复位中,
        X轴复位中,
        Y轴复位中,
        吸笔左右气缸复位中,
        摆盘卡紧气缸复位中,
        剪切穴号错误
    }
    public enum StoreAlarm : int
    {
        无消息,
        左右卡盘感应到信号,
        仓储初始化故障,
        设备气压低故障,
        仓储已满盘,
        仓储轴复位中
    }
}
