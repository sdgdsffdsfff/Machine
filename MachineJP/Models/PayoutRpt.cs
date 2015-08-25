using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MachineJPDll.Enums;
using MachineJPDll.Utils;

namespace MachineJPDll.Models
{
    /// <summary>
    /// VMC出币报告
    /// </summary>
    public class PayoutRpt
    {
        /// <summary>
        /// 从串口读取的通过验证的数据
        /// </summary>
        private byte[] m_data;

        /// <summary>
        /// VMC出币报告
        /// </summary>
        /// <param name="data">从串口读取的通过验证的数据</param>
        public PayoutRpt(byte[] data)
        {
            m_data = data;
        }

        public override string ToString()
        {
            StringBuilder result = new StringBuilder();
            result.AppendFormat("出币类型：{0}\r\n", device.ToString());
            result.AppendFormat("实际退币金额：{0}\r\n", value.ToString());
            result.AppendFormat("退币结束后，用户投币余额总额：{0}\r\n", total_value.ToString());
            result.AppendFormat("type：{0}\r\n", type.ToString());

            return result.ToString();
        }

        /// <summary>
        /// 出币类型
        /// </summary>
        public PayoutType device
        {
            get
            {
                return (PayoutType)m_data[5];
            }
        }

        /// <summary>
        /// 实际退币金额
        /// </summary>
        public int value
        {
            get
            {
                return CommonUtil.ByteArray2Int(m_data, 6, 2);
            }
        }

        /// <summary>
        /// 退币结束后，用户投币余额总额
        /// </summary>
        public int total_value
        {
            get
            {
                return CommonUtil.ByteArray2Int(m_data, 8, 2);
            }
        }

        /// <summary>
        /// 如果是用户手工退币（包括CONTROL_IND 中type=6 的情况），则type=0
        /// 如果是PAYOUT_IND 发起的出币，则type=PAYOUT_IND 中的type
        /// </summary>
        public byte type
        {
            get
            {
                return m_data[10];
            }
        }

    }
}
