using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using MachineJPDll.Models;
using MachineJPDll.Utils;

namespace MachineJPDll
{
    /// <summary>
    /// 售货机接口(骏鹏接口)
    /// </summary>
    public partial class MachineJP
    {
        #region 变量
        /// <summary>
        /// 串口资源
        /// </summary>
        private SerialPort m_SerialPort = null;
        /// <summary>
        /// 待发送给串口的命令列表
        /// </summary>
        private List<Cmd> m_CommandList = new List<Cmd>();
        /// <summary>
        /// 等待ACK_RPT或NAK_RPT的PC端向VMC端发送的消息列表
        /// </summary>
        private List<MT> m_WaitResultMTList = new List<MT>();
        /// <summary>
        /// 从串口接收的数据集合(数据已通过验证)
        /// </summary>
        private ReceiveDataCollection m_ReceiveDataCollection = new ReceiveDataCollection();
        #endregion

        #region 构造函数与析构函数
        /// <summary>
        /// 售货机接口(骏鹏接口)
        /// </summary>
        public MachineJP()
        {

        }

        ~MachineJP()
        {
            if (m_SerialPort != null)
            {
                m_SerialPort.Close();
                m_SerialPort.Dispose();
                m_SerialPort = null;
            }
        }
        #endregion

        #region 读取串口数据
        /// <summary>
        /// 读取串口数据
        /// </summary>
        /// <returns>从串口读取的数据</returns>
        private byte[] ReadPort()
        {
            //读取串口数据
            DateTime dt = DateTime.Now;
            while (m_SerialPort.BytesToRead < 2)
            {
                Thread.Sleep(1);

                if (DateTime.Now.Subtract(dt).TotalMilliseconds > 1500) //超时
                {
                    throw new Exception("ReadPort读取串口数据超时");
                }
            }
            List<byte> recList = new List<byte>();
            byte[] recData = new byte[m_SerialPort.BytesToRead];
            m_SerialPort.Read(recData, 0, recData.Length);
            recList.AddRange(recData);
            int length = recData[1] + 2; //报文数据总长度
            while (recList.Count < length)
            {
                if (m_SerialPort.BytesToRead > 0)
                {
                    recData = new byte[m_SerialPort.BytesToRead];
                    m_SerialPort.Read(recData, 0, recData.Length);
                    recList.AddRange(recData);
                }
                Thread.Sleep(1);
            }

            return recList.ToArray();
        }
        #endregion

        #region 向串口发送数据
        /// <summary>
        ///  向串口发送数据
        /// </summary>
        /// <param name="cmd">待发送的命令</param>
        /// <param name="SN">序列号</param>
        private void WritePort(Cmd cmd, byte SN)
        {
            //发送数据
            List<byte> sendData = cmd.Data;
            sendData[1] = (byte)sendData.Count;
            sendData[2] = SN;
            byte[] checkCode = CommonUtil.CalCheckCode(sendData, sendData.Count);
            sendData.AddRange(checkCode);
            if (cmd.Mt != null)
            {
                m_WaitResultMTList.Add(cmd.Mt);
            }
            m_SerialPort.Write(sendData.ToArray(), 0, sendData.Count);
            LogHelper.Log(LogMsgType.Info, true, sendData.ToArray());
        }
        #endregion

        #region 发送ACK消息
        /// <summary>
        /// 发送ACK消息
        /// </summary>
        /// <param name="SN">序列号</param>
        private void SendACK(byte SN)
        {
            List<byte> sendData = new List<byte>() { 0xE5, 0x00, 0x00, 0x40, 0x80 };
            WritePort(new Cmd(sendData), SN);
        }
        #endregion

        #region Init 初始化
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="com">串口号(例：COM1)</param>
        public void Init(string com)
        {
            if (m_SerialPort == null)
            {
                m_SerialPort = new SerialPort(com, 9600, Parity.None, 8, StopBits.One);
                m_SerialPort.ReadBufferSize = 1024;
                m_SerialPort.WriteBufferSize = 1024;
                m_SerialPort.DataReceived += new SerialDataReceivedEventHandler(serialPort_DataReceived);
            }

            if (!m_SerialPort.IsOpen)
            {
                m_SerialPort.Open();
            }

            GET_SETUP();
            CONTROL_IND(0x13, new byte[] { 0x00 }); //初始化完成标志
            GET_STATUS();

            SetDecimalPlaces(2); //设置小数点位数
        }
        #endregion

        #region Close 关闭连接
        /// <summary>
        /// 关闭连接
        /// </summary>
        public void Close()
        {
            m_SerialPort.Close();
        }
        #endregion

        #region 接收串口数据
        /// <summary>
        /// 接收串口数据
        /// </summary>
        /// <param name="type">消息类型</param>
        /// <param name="subtype">消息子类型</param>
        public byte[] Receive(byte type, byte subtype)
        {
            return m_ReceiveDataCollection.Get(type, subtype);
        }

        /// <summary>
        /// 接收串口数据
        /// </summary>
        /// <param name="type">消息类型</param>
        /// <param name="subtype">消息子类型</param>
        public byte[] WaitReceive(byte type, byte subtype)
        {
            DateTime time = DateTime.Now;
            while (true)
            {
                byte[] receiveData = m_ReceiveDataCollection.Get(type, subtype);
                if (receiveData != null) return receiveData;

                if (DateTime.Now.Subtract(time).TotalMinutes > 1)
                {
                    throw new Exception("WaitReceive超时，十进制消息类型：" + type.ToString() + "，十进制消息子类型：" + subtype.ToString());
                }

                Thread.Sleep(50);
            }
        }

        /// <summary>
        /// 接收串口数据
        /// </summary>
        /// <param name="type">消息类型</param>
        public byte[] WaitReceive(byte type)
        {
            DateTime time = DateTime.Now;
            while (true)
            {
                byte[] receiveData = m_ReceiveDataCollection.Get(type);
                if (receiveData != null) return receiveData;

                if (DateTime.Now.Subtract(time).TotalMinutes > 1)
                {
                    throw new Exception("WaitReceive超时，十进制消息类型：" + type.ToString());
                }

                Thread.Sleep(50);
            }
        }
        #endregion

        #region 判断消息是否发送成功
        /// <summary>
        /// 判断消息是否发送成功
        /// </summary>
        public bool SendSuccess(byte type, byte subtype)
        {
            DateTime time = DateTime.Now;
            while (true)
            {
                if (DateTime.Now.Subtract(time).TotalMinutes > 1)
                {
                    throw new Exception("WaitReceive超时，十进制消息类型：" + type.ToString() + "，十进制消息子类型：" + subtype.ToString());
                }
                byte[] ack = m_ReceiveDataCollection.Get(type, subtype);
                byte[] nak = m_ReceiveDataCollection.Get(type, subtype);
                if (ack != null) return true;
                if (nak != null) return false;

                Thread.Sleep(1);
            }
        }
        #endregion

    }
}
