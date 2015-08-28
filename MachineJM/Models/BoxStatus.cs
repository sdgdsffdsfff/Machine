using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MachineJMDll.Models
{
    /// <summary>
    /// 机器设备状态
    /// </summary>
    public class BoxStatus
    {
        public StatusInfo 驱动板状态 { get; set; }
        public StatusInfo 温度传感器状态 { get; set; }
        public StatusInfo 当前温度值 { get; set; }
        public StatusInfo 目标温度值 { get; set; }
        public StatusInfo 门碰开关状态 { get; set; }
        public StatusInfo 掉货检测设备状态 { get; set; }
        public StatusInfo 网络设备状态 { get; set; }
        public StatusInfo 网络待发数据数量 { get; set; }

        public BoxStatus()
        {
            驱动板状态 = new StatusInfo();
            温度传感器状态 = new StatusInfo();
            当前温度值 = new StatusInfo();
            目标温度值 = new StatusInfo();
            门碰开关状态 = new StatusInfo();
            掉货检测设备状态 = new StatusInfo();
            网络设备状态 = new StatusInfo();
            网络待发数据数量 = new StatusInfo();
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("驱动板状态：{0}\r\n", 驱动板状态.Msg);
            sb.AppendFormat("温度传感器状态：{0}\r\n", 温度传感器状态.Msg);
            sb.AppendFormat("当前温度值：{0}\r\n", 当前温度值.Msg);
            sb.AppendFormat("目标温度值：{0}\r\n", 目标温度值.Msg);
            sb.AppendFormat("门碰开关状态：{0}\r\n", 门碰开关状态.Msg);
            sb.AppendFormat("掉货检测设备状态：{0}\r\n", 掉货检测设备状态.Msg);
            sb.AppendFormat("网络设备状态：{0}\r\n", 网络设备状态.Msg);
            sb.AppendFormat("网络待发数据数量：{0}\r\n", 网络待发数据数量.Msg);

            return sb.ToString();
        }
    }
}
