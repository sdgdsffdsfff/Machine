using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MachineJPDll.Enums
{
    /// <summary>
    /// 货仓状态设置值
    /// </summary>
    public enum TemSt
    {
        常温 = 0x00,      // 对应int值：0
        制冷 = 0x01,      // 对应int值：1
        加热 = 0x10,      // 对应int值：16
        无此货仓 = 0x11   // 对应int值：17
    }
}
