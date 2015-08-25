using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MachineJPDll.Enums
{
    /// <summary>
    /// 投币类型
    /// </summary>
    public enum PayinType
    {
        硬币投币 = 0,
        纸币投币 = 1,
        纸币暂存入 = 100,
        纸币暂存出 = 101
    }
}
