using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MachineJPDll.Enums;
using MachineJPDll.Utils;

namespace MachineJPDll.Models
{
    /// <summary>
    /// 现金投币报告
    /// </summary>
    public class PayinRpt
    {
        /// <summary>
        /// 从串口读取的通过验证的数据
        /// </summary>
        private byte[] m_data;

        /// <summary>
        /// 现金投币报告
        /// </summary>
        /// <param name="data">从串口读取的通过验证的数据</param>
        public PayinRpt(byte[] data)
        {
            m_data = data;
        }

        public override string ToString()
        {
            StringBuilder result = new StringBuilder();
            result.AppendFormat("投币类型：{0}\r\n", dt.ToString());
            result.AppendFormat("现金投币的面额：{0}\r\n", value.ToString());
            result.AppendFormat("用户现金投币余额：{0}\r\n", total_value.ToString());

            return result.ToString();
        }

        /// <summary>
        /// 投币类型
        /// </summary>
        public PayinType dt
        {
            get
            {
                return (PayinType)m_data[5];
            }
        }

        /// <summary>
        /// 现金投币的面额
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

    }
}
