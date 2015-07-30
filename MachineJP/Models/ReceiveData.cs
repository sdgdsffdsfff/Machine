using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MachineJPDll.Models
{
    /// <summary>
    /// 从串口接收到的数据(数据已通过验证)
    /// </summary>
    public class ReceiveData
    {
        /// <summary>
        /// 从串口接收到的数据(数据已通过验证)
        /// </summary>
        public byte[] Data { get; set; }
        /// <summary>
        /// 添加到集合ReceiveDataCollection的时间
        /// </summary>
        public DateTime AddTime { get; set; }
        /// <summary>
        /// 消息类型
        /// </summary>
        public byte Type { get; set; }
        /// <summary>
        /// 消息子类型
        /// </summary>
        public byte Subtype { get; set; }

        /// <summary>
        /// 从串口接收到的数据(数据已通过验证)
        /// </summary>
        /// <param name="type">消息类型</param>
        /// <param name="subtype">消息子类型</param>
        /// <param name="data">从串口接收到的数据(数据已通过验证)</param>
        /// <param name="addTime">添加到集合ReceiveDataCollection的时间</param>
        public ReceiveData(byte type, byte subtype, byte[] data, DateTime addTime)
        {
            this.Type = type;
            this.Subtype = subtype;
            this.Data = data;
            this.AddTime = addTime;
        }
    }
}
