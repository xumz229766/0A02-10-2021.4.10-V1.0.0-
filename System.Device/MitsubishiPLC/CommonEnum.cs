﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Device.MitsubishiPLC
{
    [Flags]
    public enum McFrame
    {
        MC3E = 0x01,
        MC4E = 0x02,

        ASCII_FLAG = 0x80,

        MC3E_BINARY = MC3E,
        MC3E_ASCII = MC3E | ASCII_FLAG,
    }

    ///PLC文件种类定义的列举体 
    public enum PLCDeviceType
    {
        M = 0x90,
        SM = 0x91,
        L = 0x92,
        F = 0x93,
        V = 0x94,
        S = 0x98,
        X = 0x9C,
        Y = 0x9D,
        B = 0xA0,
        SB = 0xA1,
        DX = 0xA2,
        DY = 0xA3,
        D = 0xA8,
        SD = 0xA9,
        R = 0xAF,
        ZR = 0xB0,
        W = 0xB4,
        SW = 0xB5,
        TC = 0xC0,
        TS = 0xC1,
        TN = 0xC2,
        CC = 0xC3,
        CS = 0xC4,
        CN = 0xC5,
        SC = 0xC6,
        SS = 0xC7,
        SN = 0xC8,
        Z = 0xCC,
        TT,
        TM,
        CT,
        CM,
        A,
        Max
    }
}
