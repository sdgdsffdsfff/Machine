using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MachineJPDll.Models
{
    /// <summary>
    /// VMC确认结果
    /// </summary>
    public class AckNakRpt
    {
        /// <summary>
        /// 从串口读取的通过验证的数据
        /// </summary>
        private byte[] m_data;

        /// <summary>
        /// VMC系统参数
        /// </summary>
        /// <param name="data">从串口读取的通过验证的数据</param>
        public AckNakRpt(byte[] data)
        {
            m_data = data;
        }

        public override string ToString()
        {
            return Success ? "成功" : "失败";
        }

        /// <summary>
        /// 是否成功
        /// </summary>
        public bool Success
        {
            get
            {
                if (m_data[4] == 0x01)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

    }
}
