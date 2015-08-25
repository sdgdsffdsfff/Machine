using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MachineJPDll.Utils;

namespace MachineJPDll.Models
{
    /// <summary>
    /// VMC扣款报告
    /// </summary>
    public class CostRpt
    {
        /// <summary>
        /// 从串口读取的通过验证的数据
        /// </summary>
        private byte[] m_data;

        /// <summary>
        /// VMC扣款报告
        /// </summary>
        /// <param name="data">从串口读取的通过验证的数据</param>
        public CostRpt(byte[] data)
        {
            m_data = data;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("实际成功扣款金额：{0}\r\n", value.ToString());
            sb.AppendFormat("用户现金投币余额：{0}\r\n", total_value.ToString());

            return sb.ToString();
        }

        /// <summary>
        /// 实际扣款方式
        /// device=0，从用户投币总额中扣款；优先从用户非暂存金额中扣除（纸币尽量滞后压钞）
        /// </summary>
        public byte device
        {
            get
            {
                return m_data[5];
            }
        }

        /// <summary>
        /// 实际成功扣款金额；
        /// 注意：如果由于故障，导致实际扣款金额为0，也需要发送COST_RPT
        /// </summary>
        public int value
        {
            get
            {
                return CommonUtil.ByteArray2Int(m_data, 6, 2);
            }
        }

        /// <summary>
        /// 用户现金投币余额
        /// </summary>
        public int total_value
        {
            get
            {
                return CommonUtil.ByteArray2Int(m_data, 8, 2);
            }
        }

        /// <summary>
        /// VMC 不用理解type 的含义，只需上报对应的COST_RPT 时回传即可
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
