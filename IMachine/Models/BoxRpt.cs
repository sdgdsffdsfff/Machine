using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IMachineDll.Models
{
    /// <summary>
    /// 货柜信息
    /// </summary>
    public class BoxRpt : IWCFResult
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
        /// 货柜参数
        /// </summary>
        public string BoxSetup { get; set; }
        /// <summary>
        /// 货柜状态
        /// </summary>
        public string BoxStatus { get; set; }
        /// <summary>
        /// 货道信息集合
        /// </summary>
        public RoadCollectionRpt RoadCollection { get; set; }

        public BoxRpt()
        {
            HasError = false;
            BoxSetup = string.Empty;
            BoxStatus = string.Empty;
            RoadCollection = new RoadCollectionRpt();
            ErrorMsg = string.Empty;
        }
    }
}
