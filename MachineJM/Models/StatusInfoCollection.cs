using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MachineJMDll.Models
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

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("{0} {1}({2})\r\n", this.Name, this.IsNormal ? "正常" : "异常", this.Msg);
            foreach (StatusInfo statusInfo in List)
            {
                sb.AppendFormat("{0}：{1}\r\n", statusInfo.Title, statusInfo.Content);
            }

            return sb.ToString();
        }
    }
}
