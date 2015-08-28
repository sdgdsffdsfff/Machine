using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MachineJMDll.Models
{
    /// <summary>
    /// 控制主板信息
    /// </summary>
    public class MainBoardInfo
    {
        /// <summary>
        /// 控制主板协议版本号
        /// </summary>
        public string Version { get; set; }
        /// <summary>
        /// 控制主版时间
        /// </summary>
        public string Time { get; set; }
        /// <summary>
        /// 控制主板序列号
        /// </summary>
        public string SerialNumber { get; set; }
        /// <summary>
        /// 控制主板软件版本号
        /// </summary>
        public string SoftVersion { get; set; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("控制主板协议版本号：{0}\r\n", Version);
            sb.AppendFormat("控制主版时间：{0}\r\n", Time);
            sb.AppendFormat("控制主板序列号：{0}\r\n", SerialNumber);
            sb.AppendFormat("控制主板软件版本号：{0}\r\n", SoftVersion);

            return sb.ToString();
        }
    }
}
