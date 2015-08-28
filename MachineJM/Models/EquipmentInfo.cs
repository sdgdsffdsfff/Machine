using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MachineJMDll.Models
{
    /// <summary>
    /// 制冷压缩机/照明/除雾/广告灯/工控机等设备控制策略参数
    /// </summary>
    public class EquipmentInfo
    {
        public EquipmentParameter 制冷压缩机 { get; set; }
        public EquipmentParameter 照明设备 { get; set; }
        public EquipmentParameter 除雾设备 { get; set; }
        public EquipmentParameter 广告灯 { get; set; }
        public EquipmentParameter 工控机显示器机箱风扇 { get; set; }
        public EquipmentParameter 预留设备1 { get; set; }
        public EquipmentParameter 预留设备2 { get; set; }

        public EquipmentInfo()
        {
            制冷压缩机 = new EquipmentParameter();
            照明设备 = new EquipmentParameter();
            除雾设备 = new EquipmentParameter();
            广告灯 = new EquipmentParameter();
            工控机显示器机箱风扇 = new EquipmentParameter();
            预留设备1 = new EquipmentParameter();
            预留设备2 = new EquipmentParameter();
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("制冷压缩机：{0}\r\n", 制冷压缩机.ToString());
            sb.AppendFormat("照明设备：{0}\r\n", 照明设备.ToString());
            sb.AppendFormat("除雾设备：{0}\r\n", 除雾设备.ToString());
            sb.AppendFormat("广告灯：{0}\r\n", 广告灯.ToString());
            sb.AppendFormat("工控机显示器机箱风扇：{0}\r\n", 工控机显示器机箱风扇.ToString());
            sb.AppendFormat("预留设备1：{0}\r\n", 预留设备1.ToString());
            sb.AppendFormat("预留设备2：{0}\r\n", 预留设备2.ToString());

            return sb.ToString();
        }

    }
}
