using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MachineJPDll.Enums
{
    /// <summary>
    /// 设备状态
    /// </summary>
    public enum DeviceSt
    {
        正常 = 0,
        被软件临时禁用 = 1,
        故障 = 2,
        设备不存在 = 3
    }
}
