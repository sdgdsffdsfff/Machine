using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IMachineDll.Models
{
    /// <summary>
    /// 货道信息
    /// </summary>
    public class RoadRpt
    {
        /// <summary>
        /// 层
        /// </summary>
        public int Floor { get; set; }
        /// <summary>
        /// 列
        /// </summary>
        public int Num { get; set; }
        /// <summary>
        /// 是否正常
        /// </summary>
        public bool IsOK { get; set; }
        /// <summary>
        /// 故障信息
        /// </summary>
        public string ErrorMsg { get; set; }
        /// <summary>
        /// 货道价格(单位：分)
        /// </summary>
        public int? Price { get; set; }
        /// <summary>
        /// 商品余量
        /// </summary>
        public int? Remainder { get; set; }

        public RoadRpt()
        {
            ErrorMsg = string.Empty;
            Price = null;
            Remainder = null;
        }
    }
}
