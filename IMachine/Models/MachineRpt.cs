using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IMachineDll.Models
{
    /// <summary>
    /// 货机主机信息
    /// </summary>
    public class MachineRpt : IWCFResult
    {
        /// <summary>
        /// 是否出错
        /// </summary>
        public bool HasError { get; set; }
        /// <summary>
        /// 错误信息
        /// </summary>
        public string ErrorMsg { get; set; }
        /// <summary>
        /// 货机参数
        /// </summary>
        public string VmcSetup { get; set; }
        /// <summary>
        /// 货机状态
        /// </summary>
        public string VmcStatus { get; set; }
        /// <summary>
        /// 硬币器信息
        /// </summary>
        public string CoinRpt { get; set; }
        /// <summary>
        /// 纸币器信息
        /// </summary>
        public string PaperRpt { get; set; }

        public MachineRpt()
        {
            HasError = false;
            VmcSetup = string.Empty;
            VmcStatus = string.Empty;
            CoinRpt = string.Empty;
            PaperRpt = string.Empty;
            ErrorMsg = string.Empty;
        }

    }
}
