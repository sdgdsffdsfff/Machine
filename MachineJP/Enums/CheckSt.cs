using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MachineJPDll.Enums
{
    /// <summary>
    /// 出货检测设备状态
    /// </summary>
    public enum CheckSt
    {
        正常 = 0,
        被软件禁用 = 1,
        故障 = 2,
        不支持出货检测功能 = 3
    }
}
