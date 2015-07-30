using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MachineJPDll.Enums;

namespace MachineJPDll.Models
{
    /// <summary>
    /// 货道信息
    /// </summary>
    public class HuoDaoInfo
    {
        /// <summary>
        /// 货道状态
        /// </summary>
        public HuoDaoSt HuoDaoSt { get; set; }
        /// <summary>
        /// 商品余量
        /// </summary>
        public int Remainder { get; set; }
    }
}
