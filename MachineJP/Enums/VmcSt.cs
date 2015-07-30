using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MachineJPDll.Enums
{
    /// <summary>
    /// VMC状态
    /// </summary>
    public enum VmcSt
    {
        正常 = 0,
        正常货道商品全部售空 = 1,
        故障 = 2,
        维护模式 = 3
    }
}
