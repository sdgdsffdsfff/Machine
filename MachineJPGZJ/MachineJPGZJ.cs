using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using IMachineDll.Models;
using MachineJPGZJDll.Utils;

namespace MachineJPGZJDll
{
    /// <summary>
    /// 售货机接口(骏鹏格子机接口)
    /// </summary>
    public class MachineJPGZJ
    {
        #region 变量与构造函数
        /// <summary>
        /// 串口资源
        /// </summary>
        private SerialPort serialPort = null;
        /// <summary>
        /// 锁变量
        /// </summary>
        public static object _lock = new object();

        public MachineJPGZJ(string com)
        {
            serialPort = new SerialPort(com, 9600, Parity.None, 8, StopBits.One);
            serialPort.ReadBufferSize = 1024;
            serialPort.WriteBufferSize = 1024;
        }
        #endregion

        #region 向串口发送数据，读取返回数据
        /// <summary>
        /// 向串口发送数据，读取返回数据
        /// </summary>
        /// <param name="sendData">发送的数据</param>
        /// <returns>返回的数据</returns>
        private byte[] ReadPort(byte[] sendData)
        {
            lock (_lock)
            {
                //打开连接
                if (!serialPort.IsOpen) serialPort.Open();

                //发送数据
                serialPort.Write(sendData, 0, sendData.Length);

                //读取返回数据
                DateTime dt = DateTime.Now;
                while (serialPort.BytesToRead < 1)
                {
                    Thread.Sleep(1);

                    if (DateTime.Now.Subtract(dt).TotalMilliseconds > 5000) //如果5秒后仍然无数据返回，则视为超时
                    {
                        throw new Exception("主版无响应");
                    }
                }
                List<byte> recList = new List<byte>();
                byte[] recData = new byte[serialPort.BytesToRead];
                serialPort.Read(recData, 0, recData.Length);
                recList.AddRange(recData);
                int length = recData[1] + 2; //报文数据总长度
                while (recList.Count < length)
                {
                    if (serialPort.BytesToRead > 0)
                    {
                        recData = new byte[serialPort.BytesToRead];
                        serialPort.Read(recData, 0, recData.Length);
                        recList.AddRange(recData);
                    }
                    Thread.Sleep(1);
                }

                //关闭连接
                if (serialPort.IsOpen) serialPort.Close();

                return recList.ToArray();
            }
        }
        #endregion

        #region 门控板查询状态
        /// <summary>
        /// 门控板查询状态
        /// </summary>
        /// <param name="boxAddr">柜子地址，0x00、0x01、0x02……</param>
        public StatusInfoCollection QueryBoxStatus(byte boxAddr)
        {
            List<byte> sendData = new List<byte>() { 0xC7, 0x07, boxAddr, 0x51, boxAddr, 0x00, boxAddr };
            byte[] checkCode = CommonUtil.CalCheckCode(sendData, sendData.Count);
            sendData.AddRange(checkCode);
            byte[] recData = ReadPort(sendData.ToArray());

            if (recData.Length == 18
                && CommonUtil.ValidCheckCode(recData)
                && recData[0] == 0xC8
                && recData[2] == 0x81
                && recData[3] == 0x61)
            {
                StatusInfoCollection statusInfoCollection = new StatusInfoCollection();
                statusInfoCollection.Name = "门控板状态";
                statusInfoCollection.IsNormal = true;
                statusInfoCollection.Msg = "";
                List<StatusInfo> statusInfoList = new List<StatusInfo>();
                StatusInfo statusInfo = new StatusInfo();
                statusInfo.Title = "盒子个数";
                statusInfo.Content = recData[6].ToString();
                statusInfo.IsNormal = true;
                statusInfoList.Add(statusInfo);
                statusInfo = new StatusInfo();
                statusInfo.Title = "Magic";
                statusInfo.Content = recData[7].ToString();
                statusInfo.IsNormal = true;
                statusInfoList.Add(statusInfo);
                statusInfo = new StatusInfo();
                statusInfo.Title = "Feature";
                statusInfo.Content = recData[8].ToString();
                statusInfo.IsNormal = true;
                statusInfoList.Add(statusInfo);
                statusInfo = new StatusInfo();
                statusInfo.Title = "唯一ID";
                StringBuilder uniqueId = new StringBuilder();
                for (int i = 9; i <= 15; i++)
                {
                    uniqueId.Append(recData[i].ToString("X2"));
                }
                statusInfo.Content = uniqueId.ToString();
                statusInfo.IsNormal = true;
                statusInfoList.Add(statusInfo);
                statusInfoCollection.List = statusInfoList;

                return statusInfoCollection;
            }
            else
            {
                throw new Exception("门控板返回的数据格式不正确");
            }
        }
        #endregion

        #region 开门
        /// <summary>
        /// 开门
        /// </summary>
        /// <param name="boxAddr">柜子地址，0x00、0x01、0x02……</param>
        /// <param name="gridNo">格式地址，1、2、3……</param>
        public bool OpenGrid(byte boxAddr, int gridNo)
        {
            List<byte> sendData = new List<byte>() { 0xC7, 0x07, boxAddr, 0x52, boxAddr, 0x00, (byte)gridNo };
            byte[] checkCode = CommonUtil.CalCheckCode(sendData, sendData.Count);
            sendData.AddRange(checkCode);
            byte[] recData = ReadPort(sendData.ToArray());

            if (recData.Length == 8
                && CommonUtil.ValidCheckCode(recData)
                && recData[0] == 0xC8
                && recData[2] == 0x81
                && recData[3] == 0x62)
            {
                return true;
            }
            else
            {
                throw new Exception("门控板返回的数据格式不正确");
            }
        }
        #endregion

    }
}
