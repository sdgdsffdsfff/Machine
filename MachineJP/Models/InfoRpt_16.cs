using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MachineJPDll.Utils;

namespace MachineJPDll.Models
{
    /// <summary>
    /// 纸币器信息
    /// </summary>
    public class InfoRpt_16
    {
        /// <summary>
        /// 从串口读取的通过验证的数据
        /// </summary>
        private byte[] m_data;

        /// <summary>
        /// 纸币器信息
        /// </summary>
        /// <param name="data">从串口读取的通过验证的数据</param>
        public InfoRpt_16(byte[] data)
        {
            m_data = data;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("纸币器级别：{0}\r\n", level.ToString());
            sb.AppendFormat("货币代码：{0}\r\n", CC.ToString());
            sb.AppendFormat("纸币基数值：{0}\r\n", BillScalingFacto.ToString());
            sb.AppendFormat("小数点位数：{0}\r\n", DecimalPlaces.ToString());
            sb.AppendFormat("钞箱容量：{0}\r\n", 钞箱容量.ToString());
            sb.AppendFormat("安全等级：{0}\r\n", 安全等级.ToString());
            sb.AppendFormat("暂存功能：{0}\r\n", (暂存功能 ? "有" : "无"));
            sb.AppendFormat("纸币1基数：{0}\r\n", 纸币1.ToString());
            sb.AppendFormat("纸币2基数：{0}\r\n", 纸币2.ToString());
            sb.AppendFormat("纸币3基数：{0}\r\n", 纸币3.ToString());
            sb.AppendFormat("纸币4基数：{0}\r\n", 纸币4.ToString());
            sb.AppendFormat("纸币5基数：{0}\r\n", 纸币5.ToString());
            sb.AppendFormat("纸币6基数：{0}\r\n", 纸币6.ToString());
            sb.AppendFormat("纸币7基数：{0}\r\n", 纸币7.ToString());
            sb.AppendFormat("纸币8基数：{0}\r\n", 纸币8.ToString());
            sb.AppendFormat("Identification：{0}\r\n", Z.ToString());

            return sb.ToString();
        }

        /// <summary>
        /// 纸币器级别：level=1 或2
        /// </summary>
        public int level
        {
            get
            {
                return m_data[6];
            }
        }

        /// <summary>
        /// 货币代码
        /// </summary>
        public int CC
        {
            get
            {
                return CommonUtil.ByteArray2Int(m_data, 7, 2);
            }
        }

        /// <summary>
        /// 纸币基数值
        /// </summary>
        public int BillScalingFacto
        {
            get
            {
                return CommonUtil.ByteArray2Int(m_data, 9, 2);
            }
        }

        /// <summary>
        /// 小数点位数，此小数点位数只能由VMC 读取上来，所有金额读取发送的比例换算斗按照此位来确定，
        /// 例如：0：表示以元为单位，则VMC 上报PC 金额300 分，则实际的值为3
        /// 1：表示以角单位 2 表示以分为单位
        /// </summary>
        public int DecimalPlaces
        {
            get
            {
                return m_data[11];
            }
        }

        /// <summary>
        /// 钞箱容量
        /// </summary>
        public int 钞箱容量
        {
            get
            {
                return CommonUtil.ByteArray2Int(m_data, 12, 2);
            }
        }

        /// <summary>
        /// 安全等级
        /// bit0-bit7，分别指代纸币1#到纸币8#的安全等级
        /// </summary>
        public string 安全等级
        {
            get
            {
                return Convert.ToString(m_data[14], 2).PadLeft(8, '0');
            }
        }

        /// <summary>
        /// 暂存功能
        /// </summary>
        public bool 暂存功能
        {
            get
            {
                if (m_data[15] == 0x00)
                {
                    return false;
                }
                return true;
            }
        }

        /// <summary>
        /// 纸币1基数
        /// </summary>
        public int 纸币1
        {
            get
            {
                return m_data[16];
            }
        }

        /// <summary>
        /// 纸币2基数
        /// </summary>
        public int 纸币2
        {
            get
            {
                return m_data[17];
            }
        }

        /// <summary>
        /// 纸币3基数
        /// </summary>
        public int 纸币3
        {
            get
            {
                return m_data[18];
            }
        }

        /// <summary>
        /// 纸币4基数
        /// </summary>
        public int 纸币4
        {
            get
            {
                return m_data[19];
            }
        }

        /// <summary>
        /// 纸币5基数
        /// </summary>
        public int 纸币5
        {
            get
            {
                return m_data[20];
            }
        }

        /// <summary>
        /// 纸币6基数
        /// </summary>
        public int 纸币6
        {
            get
            {
                return m_data[21];
            }
        }

        /// <summary>
        /// 纸币7基数
        /// </summary>
        public int 纸币7
        {
            get
            {
                return m_data[22];
            }
        }

        /// <summary>
        /// 纸币8基数
        /// </summary>
        public int 纸币8
        {
            get
            {
                return m_data[23];
            }
        }

        /// <summary>
        /// 纸币器最高level的IDENTIFICATION
        /// </summary>
        public string Z
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                for (int i = 24; i < 57; i++)
                {
                    sb.Append(m_data[i].ToString("X2"));
                }

                return sb.ToString();
            }
        }

    }
}
