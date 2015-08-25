using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IMachineDll.Models
{
    /// <summary>
    /// 售货机操作结果
    /// </summary>
    public class OperateResult
    {
        /// <summary>
        /// 操作是否成功
        /// </summary>
        public bool Success { get; set; }
        /// <summary>
        /// 错误消息
        /// </summary>
        public string ErrorMsg { get; set; }

        /// <summary>
        /// 售货机操作结果
        /// </summary>
        public OperateResult()
        {
            ErrorMsg = string.Empty;
        }
    }
}
