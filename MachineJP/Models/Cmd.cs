using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MachineJPDll.Models
{
    /// <summary>
    /// PC->VMC的命令信息
    /// </summary>
    public class Cmd
    {
        /// <summary>
        /// 待发送到VMC的数据
        /// </summary>
        public List<byte> Data { get; set; }
        /// <summary>
        /// 消息类型
        /// 如果需要VMC回应ACK_RPT或NAK_RPT则不为空，如果不需要VMC回应ACK_RPT或NAK_RPT则为null
        /// </summary>
        public MT Mt { get; set; }

        /// <summary>
        /// PC->VMC的命令信息
        /// </summary>
        /// <param name="data">待发送到VMC的数据</param>
        /// <param name="mt">如果需要VMC回应ACK_RPT或NAK_RPT则不为空，如果不需要VMC回应ACK_RPT或NAK_RPT则为null</param>
        public Cmd(List<byte> data, MT mt = null)
        {
            this.Data = data;
            this.Mt = mt;
        }
    }
}
