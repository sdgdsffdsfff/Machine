using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IMachineDll.Models
{
    /// <summary>
    /// 设备状态信息集合
    /// </summary>
    public class StatusInfoCollection
    {
        /// <summary>
        /// 设备名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 设备状态信息集合
        /// </summary>
        public List<StatusInfo> List { get; set; }
        /// <summary>
        /// 是否正常
        /// </summary>
        public bool IsNormal { get; set; }
        /// <summary>
        /// 错误信息
        /// </summary>
        public string Msg { get; set; }
    }
}
