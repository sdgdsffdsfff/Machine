using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MachineJPDll;
using MachineJPDll.Enums;
using MachineJPDll.Utils;

namespace MachineJPDll.Models
{
    /// <summary>
    /// VMC状态报告
    /// </summary>
    public class StatusRpt
    {
        /// <summary>
        /// 从串口读取的通过验证的数据
        /// </summary>
        private byte[] m_data;

        /// <summary>
        /// VMC状态报告
        /// </summary>
        /// <param name="data">从串口读取的通过验证的数据</param>
        public StatusRpt(byte[] data)
        {
            m_data = data;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("出货检测设备状态：{0}\r\n", check_st.ToString());
            sb.AppendFormat("纸币器状态：{0}\r\n", bv_st.ToString());
            sb.AppendFormat("硬币器状态：{0}\r\n", cc_st.ToString());
            sb.AppendFormat("VMC状态：{0}\r\n", vmc_st.ToString());
            sb.AppendFormat("展示位状态：{0} {1} {2} {3}\r\n", pos_st[0].ToString(), pos_st[1].ToString(), pos_st[2].ToString(), pos_st[3].ToString());
            sb.AppendFormat("机器中可用的找零量总金额(包括硬币和纸币)：{0}\r\n", change.ToString());
            sb.AppendFormat("货仓1货仓2货仓3货仓4温度：{0} {1} {2} {3}\r\n", tem1.ToString(), tem2.ToString(), tem3.ToString(), tem4.ToString());
            sb.AppendFormat("货仓状态设置值：{0} {1} {2} {3}\r\n", tem_st[0].ToString(), tem_st[1].ToString(), tem_st[2].ToString(), tem_st[3].ToString());
            if (this.自动退币 == 255)
            {
                sb.AppendFormat("自动退币时间：永不自动退币\r\n");
            }
            else
            {
                sb.AppendFormat("自动退币时间：{0}\r\n", 自动退币.ToString());
            }
            sb.AppendFormat("找零余量(1#--6#)：{0} {1} {2} {3} {4} {5}\r\n", this.找零余量1, this.找零余量2, this.找零余量3, this.找零余量4, this.找零余量5, this.找零余量6);

            return sb.ToString();
        }

        /// <summary>
        /// 出货检测设备状态
        /// </summary>
        public CheckSt check_st
        {
            get
            {
                byte val = m_data[5];
                return (CheckSt)CommonUtil.GetFromByte(val, 0, 2);
            }
        }

        /// <summary>
        /// 纸币器状态
        /// </summary>
        public DeviceSt bv_st
        {
            get
            {
                byte val = m_data[5];
                return (DeviceSt)CommonUtil.GetFromByte(val, 2, 2);
            }
        }

        /// <summary>
        /// 硬币器状态
        /// </summary>
        public DeviceSt cc_st
        {
            get
            {
                byte val = m_data[5];
                return (DeviceSt)CommonUtil.GetFromByte(val, 4, 2);
            }
        }

        /// <summary>
        /// VMC状态
        /// </summary>
        public VmcSt vmc_st
        {
            get
            {
                byte val = m_data[5];
                return (VmcSt)CommonUtil.GetFromByte(val, 6, 2);
            }
        }

        /// <summary>
        /// 展示位状态
        /// </summary>
        public List<DeviceSt> pos_st
        {
            get
            {
                List<DeviceSt> deviceStList = new List<DeviceSt>();

                byte val = m_data[6];
                for (int i = 0; i < 4; i++)
                {
                    DeviceSt deviceSt = (DeviceSt)CommonUtil.GetFromByte(val, i * 2, 2);
                    deviceStList.Add(deviceSt);
                }

                return deviceStList;
            }
        }

        /// <summary>
        /// 机器中，可用的找零量总金额（包括硬币和纸币）
        /// </summary>
        public int change
        {
            get
            {
                return CommonUtil.ByteArray2Int(m_data, 7, 2);
            }
        }

        /// <summary>
        /// 货仓1 温度，8 位有符号数，该温度通过货仓内传感器获取，单位：℃
        /// </summary>
        public TemSub tem1
        {
            get
            {
                return new TemSub(m_data[9]);
            }
        }

        /// <summary>
        /// 货仓2 温度，8 位有符号数，该温度通过货仓内传感器获取，单位：℃
        /// </summary>
        public TemSub tem2
        {
            get
            {
                return new TemSub(m_data[10]);
            }
        }

        /// <summary>
        /// 货仓3 温度，8 位有符号数，该温度通过货仓内传感器获取，单位：℃
        /// </summary>
        public TemSub tem3
        {
            get
            {
                return new TemSub(m_data[11]);
            }
        }

        /// <summary>
        /// 货仓4 温度，8 位有符号数，该温度通过货仓内传感器获取，单位：℃
        /// </summary>
        public TemSub tem4
        {
            get
            {
                return new TemSub(m_data[12]);
            }
        }

        /// <summary>
        /// 货仓状态设置值，共支持4 个货仓
        /// </summary>
        public List<TemSt> tem_st
        {
            get
            {
                List<TemSt> temStList = new List<TemSt>();
                for (int i = 0; i < 4; i++)
                {
                    TemSt temSt = (TemSt)CommonUtil.GetFromByte(m_data[13], i * 2, 2);
                    temStList.Add(temSt);
                }
                return temStList;
            }
        }

        /// <summary>
        /// 自动退币时间。
        /// 0：表示商品出货后，立即自动退币
        /// 255：表示永不自动退币
        /// 1-254：表示商品出货后，自动退币时间（单位：秒）
        /// </summary>
        public int 自动退币
        {
            get
            {
                return m_data[14];
            }
        }

        /// <summary>
        /// 找零余量“找零量1#”…“找零量6#”，分别对应硬币器信息INFO_RPT.type=17 的“找零
        /// 1#”…“找零6#”中每种钱币的找零数量；
        /// * 找零量最大为255，超过255 时上报为255；
        /// * 找零量单位为“个”，代表可找零硬币的个数。
        /// </summary>
        public int 找零余量1
        {
            get
            {
                return m_data[15];
            }
        }

        /// <summary>
        /// 找零余量“找零量1#”…“找零量6#”，分别对应硬币器信息INFO_RPT.type=17 的“找零
        /// 1#”…“找零6#”中每种钱币的找零数量；
        /// * 找零量最大为255，超过255 时上报为255；
        /// * 找零量单位为“个”，代表可找零硬币的个数。
        /// </summary>
        public int 找零余量2
        {
            get
            {
                return m_data[16];
            }
        }

        /// <summary>
        /// 找零余量“找零量1#”…“找零量6#”，分别对应硬币器信息INFO_RPT.type=17 的“找零
        /// 1#”…“找零6#”中每种钱币的找零数量；
        /// * 找零量最大为255，超过255 时上报为255；
        /// * 找零量单位为“个”，代表可找零硬币的个数。
        /// </summary>
        public int 找零余量3
        {
            get
            {
                return m_data[17];
            }
        }

        /// <summary>
        /// 找零余量“找零量1#”…“找零量6#”，分别对应硬币器信息INFO_RPT.type=17 的“找零
        /// 1#”…“找零6#”中每种钱币的找零数量；
        /// * 找零量最大为255，超过255 时上报为255；
        /// * 找零量单位为“个”，代表可找零硬币的个数。
        /// </summary>
        public int 找零余量4
        {
            get
            {
                return m_data[18];
            }
        }

        /// <summary>
        /// 找零余量“找零量1#”…“找零量6#”，分别对应硬币器信息INFO_RPT.type=17 的“找零
        /// 1#”…“找零6#”中每种钱币的找零数量；
        /// * 找零量最大为255，超过255 时上报为255；
        /// * 找零量单位为“个”，代表可找零硬币的个数。
        /// </summary>
        public int 找零余量5
        {
            get
            {
                return m_data[19];
            }
        }

        /// <summary>
        /// 找零余量“找零量1#”…“找零量6#”，分别对应硬币器信息INFO_RPT.type=17 的“找零
        /// 1#”…“找零6#”中每种钱币的找零数量；
        /// * 找零量最大为255，超过255 时上报为255；
        /// * 找零量单位为“个”，代表可找零硬币的个数。
        /// </summary>
        public int 找零余量6
        {
            get
            {
                return m_data[20];
            }
        }

    }
}
