using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MachineJPDll.Models
{
    /// <summary>
    /// 从串口接收到的数据集合(数据已通过验证)
    /// </summary>
    public class ReceiveDataCollection
    {
        /// <summary>
        /// 从串口接收到的数据集合(数据已通过验证)
        /// </summary>
        private List<ReceiveData> m_ReceiveDataList = new List<ReceiveData>();
        /// <summary>
        /// 数据过期时间
        /// </summary>
        private int m_Timeout = 3;
        private static object _lock = new object();

        /// <summary>
        /// 添加到集合
        /// </summary>
        /// <param name="type">消息类型</param>
        /// <param name="subtype">消息子类型</param>
        /// <param name="data">从串口接收到的数据(数据已通过验证)</param>
        public void Add(byte type, byte subtype, byte[] data)
        {
            lock (_lock)
            {
                ReceiveData receiveData = new ReceiveData(type, subtype, data, DateTime.Now);
                m_ReceiveDataList.Add(receiveData);
                for (int i = m_ReceiveDataList.Count - 1; i >= 0; i--)
                {
                    if (DateTime.Now.Subtract(m_ReceiveDataList[i].AddTime).TotalMinutes > m_Timeout)
                    {
                        m_ReceiveDataList.RemoveAt(i);
                    }
                }
            }
        }

        /// <summary>
        /// 从集合中获取串口接收到的数据(数据已通过验证)
        /// </summary>
        /// <param name="type">消息类型</param>
        /// <param name="subtype">消息子类型</param>
        /// <returns>从串口接收到的数据(数据已通过验证)</returns>
        public byte[] Get(byte type, byte subtype)
        {
            lock (_lock)
            {
                ReceiveData receiveData = null;
                for (int i = 0; i < m_ReceiveDataList.Count; i++)
                {
                    if (m_ReceiveDataList[i].Type == type && m_ReceiveDataList[i].Subtype == subtype)
                    {
                        receiveData = m_ReceiveDataList[i];
                        m_ReceiveDataList.RemoveAt(i);
                        return receiveData.Data;
                    }
                }
                return null;
            }
        }

        /// <summary>
        /// 从集合中获取串口接收到的数据(数据已通过验证)
        /// </summary>
        /// <param name="type">消息类型</param>
        /// <returns>从串口接收到的数据(数据已通过验证)</returns>
        public byte[] Get(byte type)
        {
            lock (_lock)
            {
                ReceiveData receiveData = null;
                for (int i = 0; i < m_ReceiveDataList.Count; i++)
                {
                    if (m_ReceiveDataList[i].Type == type)
                    {
                        receiveData = m_ReceiveDataList[i];
                        m_ReceiveDataList.RemoveAt(i);
                        return receiveData.Data;
                    }
                }
                return null;
            }
        }
    }
}
