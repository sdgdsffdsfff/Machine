using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MachineJMDll.Models
{
    /// <summary>
    /// 制冷压缩机/风机/照明/除雾/广告灯/工控机等设备状态
    /// </summary>
    public class EquipmentsStatus
    {
        public StatusInfo 制冷设备状态 { get; set; }
        public StatusInfo 风机状态 { get; set; }
        public StatusInfo 照明灯状态 { get; set; }
        public StatusInfo 除雾器状态 { get; set; }
        public StatusInfo 广告灯状态 { get; set; }
        public StatusInfo 工控机显示器机箱风扇状态 { get; set; }
        public StatusInfo 预留设备1状态 { get; set; }
        public StatusInfo 预留设备2状态 { get; set; }

        public EquipmentsStatus()
        {
            制冷设备状态 = new StatusInfo();
            风机状态 = new StatusInfo();
            照明灯状态 = new StatusInfo();
            除雾器状态 = new StatusInfo();
            广告灯状态 = new StatusInfo();
            工控机显示器机箱风扇状态 = new StatusInfo();
            预留设备1状态 = new StatusInfo();
            预留设备2状态 = new StatusInfo();
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("制冷设备状态：{0}\r\n", 制冷设备状态.Msg);
            sb.AppendFormat("风机状态：{0}\r\n", 风机状态.Msg);
            sb.AppendFormat("照明灯状态：{0}\r\n", 照明灯状态.Msg);
            sb.AppendFormat("除雾器状态：{0}\r\n", 除雾器状态.Msg);
            sb.AppendFormat("广告灯状态：{0}\r\n", 广告灯状态.Msg);
            sb.AppendFormat("工控机显示器机箱风扇状态：{0}\r\n", 工控机显示器机箱风扇状态.Msg);
            sb.AppendFormat("预留设备1状态：{0}\r\n", 预留设备1状态.Msg);
            sb.AppendFormat("预留设备2状态：{0}\r\n", 预留设备2状态.Msg);

            return sb.ToString();
        }
    }
}
