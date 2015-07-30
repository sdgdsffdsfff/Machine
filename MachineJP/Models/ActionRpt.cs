using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MachineJPDll.Enums;

namespace MachineJPDll.Models
{
    /// <summary>
    /// 售货机行为报告
    /// </summary>
    public class ActionRpt
    {
        /// <summary>
        /// 从串口读取的通过验证的数据
        /// </summary>
        private byte[] m_data;

        /// <summary>
        /// 售货机行为报告
        /// </summary>
        /// <param name="data">从串口读取的通过验证的数据</param>
        public ActionRpt(byte[] data)
        {
            m_data = data;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            if ((int)action == 5)
            {
                if (value == 0)
                {
                    sb.AppendFormat("售货机行为：{0}\r\n", "VMC退出维护模式");
                }
                else
                {
                    sb.AppendFormat("售货机行为：{0}\r\n", "VMC在维护模式中");
                }
            }
            else
            {
                sb.AppendFormat("售货机行为：{0}\r\n", action.ToString());
            }

            return sb.ToString();
        }

        /// <summary>
        /// 售货机行为
        /// </summary>
        public ActionSt action
        {
            get
            {
                return (ActionSt)m_data[5];
            }
        }

        /// <summary>
        /// action=5时，0：VMC 退出维护模式 非0：VMC 在维护模式中
        /// </summary>
        public int value
        {
            get
            {
                if ((int)action == 5)
                {
                    return m_data[6];
                }
                return 0x00;
            }
        }

    }
}
