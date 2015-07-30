using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MachineJPGZJDll.Utils
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
        public static bool ValidCheckCode(byte[] data)
        {
            if (data.Length < 3) throw new Exception("字节数组长度不能小于3");

            byte[] checkCode = CalCheckCode(data, data.Length - 2);

            if (checkCode[0] == data[data.Length - 2]
                && checkCode[1] == data[data.Length - 1])
            {
                return true;
            }
            return false;
        }
        #endregion

    }
}
