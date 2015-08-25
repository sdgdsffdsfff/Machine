using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MachineJPDll.Utils;

namespace MachineJPDll.Models
{
    /// <summary>
    /// VMC系统参数
    /// </summary>
    public class VmcSetup
    {
        /// <summary>
        /// 从串口读取的通过验证的数据
        /// </summary>
        private byte[] m_data;

        /// <summary>
        /// VMC系统参数
        /// </summary>
        /// <param name="data">从串口读取的通过验证的数据</param>
        public VmcSetup(byte[] data)
        {
            m_data = data;
        }

        public override string ToString()
        {
            StringBuilder result = new StringBuilder();
            result.AppendFormat("VMC支持的货柜数量：{0}\r\n", bin_num);
            result.AppendFormat("VMC支持的展示位数量：{0}\r\n", pos_num);
            result.Append(feature);

            return result.ToString();
        }

        /// <summary>
        /// VMC 支持的货柜数量。
        /// 即箱体数量 单指主柜副柜 1 表示只有主柜 2 表示带副柜。主柜副柜都没有则为0
        /// </summary>
        public int bin_num
        {
            get
            {
                return m_data[5];
            }
        }

        /// <summary>
        /// VMC 支持的展示位数量需要上报实际展示位数量某些机型没有展示位，则pos_num=0
        /// 目前固定为0
        /// </summary>
        public int pos_num
        {
            get
            {
                return m_data[6];
            }
        }

        /// <summary>
        /// 保留
        /// 售货机机型标识
        /// 本标识根据不同厂商VMC 种类的不同而不同，由骏鹏统一分配
        /// </summary>
        public int magic1
        {
            get
            {
                return CommonUtil.ByteArray2Int(m_data, 7, 2);
            }
        }

        public int reserved1
        {
            get
            {
                return m_data[9];
            }
        }

        public int reserved2
        {
            get
            {
                return m_data[10];
            }
        }

        public string feature
        {
            get
            {
                int value = CommonUtil.ByteArray2Int(m_data, 11, 4);
                string binaryVal = Convert.ToString(value, 2).PadLeft(32, '0');
                StringBuilder result = new StringBuilder();

                #region 解析feature
                if (binaryVal[0] == 1)
                {
                    result.Append("支持PAYOUT_IND\r\n");
                }
                else
                {
                    result.Append("不支持PAYOUT_IND\r\n");
                }
                if (binaryVal[1] == 1)
                {
                    result.Append("支持COST_IND\r\n");
                }
                else
                {
                    result.Append("不支持COST_IND\r\n");
                }
                if (binaryVal[2] == 1)
                {
                    result.Append("支持CONTROL_IND（type=2、6）和BUTTON_RPT（type=4）\r\n");
                }
                else
                {
                    result.Append("不支持CONTROL_IND（type=2、6）和BUTTON_RPT（type=4）\r\n");
                }
                if (binaryVal[3] == 1)
                {
                    result.Append("支持CONTROL_IND（type=7）\r\n");
                }
                else
                {
                    result.Append("不支持CONTROL_IND（type=7）\r\n");
                }
                if (binaryVal[4] == 1)
                {
                    result.Append("支持CONTROL_IND（type=8）\r\n");
                }
                else
                {
                    result.Append("不支持CONTROL_IND（type=8）\r\n");
                }
                if (binaryVal[5] == 1)
                {
                    result.Append("支持CONTROL_IND（type=9）\r\n");
                }
                else
                {
                    result.Append("不支持CONTROL_IND（type=9）\r\n");
                }
                if (binaryVal[6] == 1)
                {
                    result.Append("支持CONTROL_IND（type=17）和GET_INFO（type=5）\r\n");
                }
                else
                {
                    result.Append("不支持CONTROL_IND（type=17）和GET_INFO（type=5）\r\n");
                }
                if (binaryVal[7] == 1)
                {
                    result.Append("支持离线售卖\r\n");
                }
                else
                {
                    result.Append("不支持离线售卖\r\n");
                }
                if (binaryVal[8] == 1)
                {
                    result.Append("支持出货检测\r\n");
                }
                else
                {
                    result.Append("不支出货检测\r\n");
                }
                if (binaryVal[9] == 1)
                {
                    result.Append("支持total_value 和GET_INFO（type=3）\r\n");
                }
                else
                {
                    result.Append("不支total_value 和GET_INFO（type=3）\r\n");
                }
                if (binaryVal[10] == 1)
                {
                    result.Append("支持GET_INFO（type=6）\r\n");
                }
                else
                {
                    result.Append("不支GET_INFO（type=6）\r\n");
                }
                if (binaryVal[11] == 1)
                {
                    result.Append("带盒饭机\r\n");
                }
                else
                {
                    result.Append("不带盒饭机\r\n");
                }
                if (binaryVal[15] == 1)
                {
                    result.Append("支持制冷\r\n");
                }
                else
                {
                    result.Append("不支持制冷\r\n");
                }
                if (binaryVal[16] == 1)
                {
                    result.Append("支持加热\r\n");
                }
                else
                {
                    result.Append("不支持加热\r\n");
                }
                if (binaryVal[17] == 0)
                {
                    result.Append("支持时钟\r\n");
                }
                else
                {
                    result.Append("不支持时钟\r\n");
                }
                #endregion

                return result.ToString();
            }
        }

    }
}
