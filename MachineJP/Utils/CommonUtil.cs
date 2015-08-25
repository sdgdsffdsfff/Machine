using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using MachineJPDll.Models;

namespace MachineJPDll.Utils
{
    /// <summary>
    /// 通用工具
    /// </summary>
    public class CommonUtil
    {
        #region 计算校验码
        /// <summary>
        /// 计算校验码，计算结果是两个字节
        /// </summary>
        public static byte[] CalCheckCode(byte[] data, int length)
        {
            ushort i, j;
            ushort crc = 0;
            ushort current;

            for (i = 0; i < length; i++)
            {
                current = (ushort)(data[i] << 8);

                for (j = 0; j < 8; j++)
                {
                    if ((short)(crc ^ current) < 0)
                    {
                        crc = (ushort)((crc << 1) ^ 0x1021);
                    }
                    else
                    {
                        crc <<= 1;
                    }

                    current <<= 1;
                }
            }

            byte[] result = new byte[2];
            result[0] = (byte)(crc / 256);
            result[1] = (byte)(crc % 256);

            return result;
        }
        #endregion

        #region 计算校验码
        /// <summary>
        /// 计算校验码，计算结果是两个字节
        /// </summary>
        public static byte[] CalCheckCode(List<byte> data, int length)
        {
            ushort i, j;
            ushort crc = 0;
            ushort current;

            for (i = 0; i < length; i++)
            {
                current = (ushort)(data[i] << 8);

                for (j = 0; j < 8; j++)
                {
                    if ((short)(crc ^ current) < 0)
                    {
                        crc = (ushort)((crc << 1) ^ 0x1021);
                    }
                    else
                    {
                        crc <<= 1;
                    }

                    current <<= 1;
                }
            }

            byte[] result = new byte[2];
            result[0] = (byte)(crc / 256);
            result[1] = (byte)(crc % 256);

            return result;
        }
        #endregion

        #region 验证校验码
        /// <summary>
        /// 验证校验码,字节数组长度不能小于4
        /// </summary>
        private static bool ValidCheckCode(byte[] data)
        {
            if (data.Length < 3) return false;

            byte[] checkCode = CalCheckCode(data, data.Length - 2);
            if (checkCode[0] == data[data.Length - 2]
                && checkCode[1] == data[data.Length - 1])
            {
                return true;
            }

            return false;
        }
        #endregion

        #region 验证从串口接收的数据是否正确
        /// <summary>
        /// 验证从串口接收的数据是否正确
        /// </summary>
        public static bool ValidReceiveData(byte[] data)
        {
            if (data != null
                && data.Length >= 7
                && data[0] == 0xE5
                && data[1] == data.Length - 2
                && ValidCheckCode(data))
            {
                return true;
            }
            return false;
        }
        #endregion

        #region 字节数组转int
        /// <summary>
        /// 字节数组转int
        /// </summary>
        public static int ByteArray2Int(byte[] data, int start, int length)
        {
            if (!(data != null
                && data.Length > 0
                && start >= 0
                && length > 0
                && start < data.Length
                && start + length <= data.Length))
            {
                throw new Exception("字节数组转int出错");
            }

            int sum = 0;
            int k = 1;
            for (int i = start + length - 1; i >= start; i--)
            {
                sum += k * data[i];
                k *= 256;
            }
            return sum;
        }
        #endregion

        #region int转字节数组
        /// <summary>
        /// int转字节数组
        /// </summary>
        /// <param name="value">要转换的值</param>
        /// <param name="length">转换后字节数组的长度</param>
        public static byte[] Int2ByteArray(int value, int length)
        {
            if (value < 0) throw new Exception("参数value必须大于或等于0");

            List<byte> byteList = new List<byte>();
            do
            {
                int mod = value % 256;
                value = value / 256;
                byteList.Insert(0, (byte)mod);
            } while (value > 0);

            int k = length - byteList.Count;
            for (int i = 0; i < k; i++)
            {
                byteList.Insert(0, 0);
            }

            return byteList.ToArray();
        }
        #endregion

        #region 从串口接收的数据中获取序列号(SN)
        /// <summary>
        /// 从串口接收的数据中获取序列号(SN)
        /// </summary>
        /// <param name="receiveData">从串口接收的数据(数据已通过ValidReceiveData方法验证)</param>
        /// <returns>序列号(SN)</returns>
        public static byte GetSN(byte[] receiveData)
        {
            return receiveData[2];
        }
        #endregion

        #region 找零算法
        /// <summary>
        /// 计算找零方案
        /// </summary>
        /// <param name="length">数组的长度</param>
        /// <param name="coins">可找零币种</param>
        /// <param name="lefts">每种币剩余量</param>
        /// <param name="results">找零结果（返回结果"成功"时有效）</param>
        /// <param name="money">总金额</param>
        /// <returns>返回1：成功；返回0：失败</returns>
        public static int MakeChange(int length, int[] coins, int[] lefts, int[] results, int money)
        {
            int i = 0;
            for (i = 0; i < length; ++i)
            {
                int coin = coins[i];
                int left = lefts[i];
                if (left <= 0 || coin > money)
                {
                    continue;
                }
                int count = money / coin;
                if (count > left)
                {
                    count = left;
                }
                lefts[i] = left - count;
                results[i] = count;
                money -= count * coin;
                if (money == 0)
                {
                    break;
                }
            }
            if (money == 0)
            {
                return 1;
            }
            else
            {
                for (i = 0; i < length; ++i)
                {
                    results[i] = 0;
                }
                return 0;
            }
        }
        #endregion

        #region 从字节中获取值
        /// <summary>
        /// 从字节中获取值
        /// </summary>
        /// <param name="data">数据</param>
        /// <param name="start">开始bit</param>
        /// <param name="length">bit长度</param>
        /// <returns>值</returns>
        public static int GetFromByte(byte data, int start, int length)
        {
            string binaryString = Convert.ToString(data, 2).PadLeft(8, '0');
            string temp = binaryString.Substring(start, length);
            return Convert.ToInt32(temp, 2);
        }
        #endregion

        #region 获取确认标志F0
        /// <summary>
        /// 获取确认标志F0
        /// 如果F0 值为0，表示消息接收方不需要响应发送方，是否收到并正确处理该消息。
        /// 如果F0 值为1，表示消息接收方需要响应发送方，是否正确处理该消息。
        /// </summary>
        /// <param name="receiveData">从串口接收的数据(数据已通过ValidReceiveData方法验证)</param>
        /// <returns>确认标志F0</returns>
        public static int GetF0(byte[] receiveData)
        {
            return GetFromByte(receiveData[3], 4, 4);
        }
        #endregion

        #region 判断PC是否需要发送ACK给VMC
        /// <summary>
        /// 判断PC是否需要发送ACK给VMC
        /// </summary>
        /// <param name="receiveData"></param>
        /// <returns>true:需要发送ACK,false:不需要发送ACK或不在此处自动发送</returns>
        public static bool NeedACK(byte[] receiveData)
        {
            MT mt = new MT(receiveData);
            if (mt.Type != 0x03
                && mt.Type != 0x0A
                && mt.Type != 0x12)
            {
                if (CommonUtil.GetF0(receiveData) == 1)
                {
                    return true;
                }
            }
            return false;
        }
        #endregion

    }
}
