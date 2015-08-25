using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MachineJPDll.Enums;
using MachineJPDll.Utils;

namespace MachineJPDll.Models
{
    /// <summary>
    /// VMC货道报告
    /// </summary>
    public class HuoDaoRpt
    {
        /// <summary>
        /// 从串口读取的通过验证的数据
        /// </summary>
        private byte[] m_data;

        /// <summary>
        /// VMC货道报告
        /// </summary>
        /// <param name="data">从串口读取的通过验证的数据</param>
        public HuoDaoRpt(byte[] data)
        {
            m_data = data;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("箱柜号：{0}\r\n", device.ToString());
            StringBuilder msg = new StringBuilder();
            for (int i = 0; i < HuoDaoInfoList.Count; i++)
            {
                HuoDaoInfo huoDaoInfo = HuoDaoInfoList[i];
                msg.AppendFormat("货道{0}:{1}[商品余量{2}]\r\n", (i + 1).ToString().PadLeft(2, ' '), huoDaoInfo.HuoDaoSt.ToString(), huoDaoInfo.Remainder.ToString().PadRight(2, ' '));
            }
            sb.AppendFormat("货道信息：\r\n{0}\r\n", msg.ToString());

            return sb.ToString();
        }

        /// <summary>
        /// 箱柜号
        /// </summary>
        public byte device
        {
            get
            {
                return m_data[5];
            }
        }

        /// <summary>
        /// 货道信息集合
        /// 个数固定80
        /// </summary>
        public List<HuoDaoInfo> HuoDaoInfoList
        {
            get
            {
                List<HuoDaoInfo> list = new List<HuoDaoInfo>();

                if (m_data.Length == 88)
                {

                    for (int i = 0; i < 80; i++)
                    {
                        HuoDaoInfo huoDaoInfo = new HuoDaoInfo();
                        byte b = m_data[6 + i];
                        huoDaoInfo.HuoDaoSt = (HuoDaoSt)CommonUtil.GetFromByte(b, 0, 2);
                        huoDaoInfo.Remainder = CommonUtil.GetFromByte(b, 2, 6);
                        list.Add(huoDaoInfo);
                    }
                }
                else
                {
                    LogHelper.LogError(LogMsgType.Error, false, m_data, "huodao字段的个数不等于80");
                }

                return list;
            }
        }
    }
}
