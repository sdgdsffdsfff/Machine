using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MachineDll.Models
{
    /// <summary>
    /// 设备状态信息(设备的单个状态信息)
    /// </summary>
    public class StatusInfo
    {
        /// <summary>
        /// 是否正常
        /// </summary>
        public bool IsNormal { get; set; }
        /// <summary>
        /// 状态标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 状态内容
        /// </summary>
        public string Content { get; set; }
    }
}
