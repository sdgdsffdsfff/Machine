using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MachineJPDll.Utils;

namespace MachineJPDll.Models
{
    /// <summary>
    /// 硬币器信息
    /// </summary>
    public class InfoRpt_17
    {
        /// <summary>
        /// 从串口读取的通过验证的数据
        /// </summary>
        private byte[] m_data;

        /// <summary>
        /// 硬币器信息
        /// </summary>
        /// <param name="data">从串口读取的通过验证的数据</param>
        public InfoRpt_17(byte[] data)
        {
            m_data = data;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("硬币器级别：{0}\r\n", level.ToString());
            sb.AppendFormat("硬币代码：{0}\r\n", CC.ToString());
            sb.AppendFormat("硬币基数值：{0}\r\n", CoinScalingFacto.ToString());
            sb.AppendFormat("小数点位数：{0}\r\n", DecimalPlaces.ToString());
            sb.AppendFormat("硬币1基数：{0}\r\n", 硬币1.ToString());
            sb.AppendFormat("硬币2基数：{0}\r\n", 硬币2.ToString());
            sb.AppendFormat("硬币3基数：{0}\r\n", 硬币3.ToString());
            sb.AppendFormat("硬币4基数：{0}\r\n", 硬币4.ToString());
            sb.AppendFormat("硬币5基数：{0}\r\n", 硬币5.ToString());
            sb.AppendFormat("硬币6基数：{0}\r\n", 硬币6.ToString());
            sb.AppendFormat("找零1：{0}\r\n", 找零1.ToString());
            sb.AppendFormat("找零2：{0}\r\n", 找零2.ToString());
            sb.AppendFormat("找零3：{0}\r\n", 找零3.ToString());
            sb.AppendFormat("找零4：{0}\r\n", 找零4.ToString());
            sb.AppendFormat("找零5：{0}\r\n", 找零5.ToString());
            sb.AppendFormat("找零6：{0}\r\n", 找零6.ToString());
            sb.AppendFormat("Identification：{0}\r\n", Z.ToString());

            return sb.ToString();
        }

        /// <summary>
        /// 硬币器级别：level=2 或3
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
        /// 硬币基数值
        /// </summary>
        public int CoinScalingFacto
        {
            get
            {
                return m_data[9];
            }
        }

        /// <summary>
        /// 小数点位数 1：表示分 10：表示角 100：表示元。
        /// </summary>
        public int DecimalPlaces
        {
            get
            {
                return m_data[10];
            }
        }

        /// <summary>
        /// 硬币1基数
        /// </summary>
        public int 硬币1
        {
            get
            {
                return m_data[11];
            }
        }

        /// <summary>
        /// 硬币2基数
        /// </summary>
        public int 硬币2
        {
            get
            {
                return m_data[12];
            }
        }

        /// <summary>
        /// 硬币3基数
        /// </summary>
        public int 硬币3
        {
            get
            {
                return m_data[13];
            }
        }

        /// <summary>
        /// 硬币4基数
        /// </summary>
        public int 硬币4
        {
            get
            {
                return m_data[14];
            }
        }

        /// <summary>
        /// 硬币5基数
        /// </summary>
        public int 硬币5
        {
            get
            {
                return m_data[15];
            }
        }

        /// <summary>
        /// 硬币6基数
        /// </summary>
        public int 硬币6
        {
            get
            {
                return m_data[16];
            }
        }

        /// <summary>
        /// 找零1
        /// </summary>
        public int 找零1
        {
            get
            {
                return m_data[17];
            }
        }

        /// <summary>
        /// 找零2
        /// </summary>
        public int 找零2
        {
            get
            {
                return m_data[18];
            }
        }

        /// <summary>
        /// 找零3
        /// </summary>
        public int 找零3
        {
            get
            {
                return m_data[19];
            }
        }

        /// <summary>
        /// 找零4
        /// </summary>
        public int 找零4
        {
            get
            {
                return m_data[20];
            }
        }

        /// <summary>
        /// 找零5
        /// </summary>
        public int 找零5
        {
            get
            {
                return m_data[21];
            }
        }

        /// <summary>
        /// 找零6
        /// </summary>
        public int 找零6
        {
            get
            {
                return m_data[22];
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
                for (int i = 23; i < 56; i++)
                {
                    sb.Append(m_data[i].ToString("X2"));
                }

                return sb.ToString();
            }
        }

    }
}
