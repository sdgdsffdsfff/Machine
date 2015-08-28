using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IMachineDll.Models
{
    /// <summary>
    /// WCF返回结果接口
    /// </summary>
    public interface IWCFResult
    {
        /// <summary>
        /// 是否出错
        /// </summary>
        bool HasError { get; set; }
        /// <summary>
        /// 错误信息
        /// </summary>
        string ErrorMsg { get; set; }
    }
}
