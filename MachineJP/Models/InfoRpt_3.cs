using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MachineJPDll.Utils;

namespace MachineJPDll.Models
{
    /// <summary>
    /// 用户投币余额
    /// </summary>
    public class InfoRpt_3
    {
        /// <summary>
        /// 从串口读取的通过验证的数据
        /// </summary>
        private byte[] m_data;

        /// <summary>
        /// 用户投币余额
        /// </summary>
        /// <param name="data">从串口读取的通过验证的数据</param>
        public InfoRpt_3(byte[] data)
        {
            m_data = data;
        }

        public override string ToString()
        {
            return string.Format("用户投币余额：{0}", total_value.ToString());
        }

        /// <summary>
        /// 用户投币余额
        /// </summary>
        public int total_value
        {
            get
            {
                return CommonUtil.ByteArray2Int(m_data, 6, 2);
            }
        }
    }
}
