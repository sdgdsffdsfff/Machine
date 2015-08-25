using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IMachineDll.Models
{
    /// <summary>
    /// 用户一次投币金额
    /// </summary>
    public class AmountRpt
    {
        /// <summary>
        /// 金额(面额)
        /// </summary>
        public int Amount { get; set; }
        /// <summary>
        /// 类型：0硬币，1纸币
        /// </summary>
        public int Type { get; set; }
    }
}
