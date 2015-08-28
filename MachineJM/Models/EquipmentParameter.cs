using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MachineJMDll.Models
{
    /// <summary>
    /// 制冷压缩机/照明/除雾/广告灯/工控机等设备控制策略参数
    /// </summary>
    public class EquipmentParameter
    {
        /// <summary>
        /// 是否正常
        /// </summary>
        public bool IsOK { get; set; }
        /// <summary>
        /// 错误信息
        /// </summary>
        public string ErrorMsg { get; set; }

        public string 目标温度值 { get; set; }
        public string 控制模式 { get; set; }
        public string 定时时间段1 { get; set; }
        public string 定时时间段2 { get; set; }

        public EquipmentParameter()
        {
            目标温度值 = string.Empty;
            控制模式 = string.Empty;
            定时时间段1 = string.Empty;
            定时时间段2 = string.Empty;

            ErrorMsg = string.Empty;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            if (this.IsOK)
            {
                sb.Append("\r\n");
                sb.AppendFormat("目标温度值：{0}\r\n", 目标温度值);
                sb.AppendFormat("控制模式：{0}\r\n", 控制模式);
                sb.AppendFormat("定时时间段1：{0}\r\n", 定时时间段1);
                sb.AppendFormat("定时时间段2：{0}\r\n", 定时时间段2);
            }
            else
            {
                sb.AppendFormat(ErrorMsg + "\r\n");
            }

            return sb.ToString();
        }
    }
}
