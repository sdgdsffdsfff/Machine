using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MachineJPDll.Enums
{
    /// <summary>
    /// 货仓温度状态
    /// </summary>
    public enum TemSubSt
    {
        正常 = 0x00,
        故障 = 0xFF,
        不存在此货仓 = 0xFE,
        该温度无意义 = 0xFD
    }
}
