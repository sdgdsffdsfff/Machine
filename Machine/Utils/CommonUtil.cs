using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineDll.Utils
{
    /// <summary>
    /// 通用工具
    /// </summary>
    public class CommonUtil
    {
        #region 计算校验码
        /// <summary>
        /// 计算校验码，字节数组最后一个字节存放校验码
        /// </summary>
        public static void CalCheckCode(byte[] data)
        {
            if (data.Length < 4) throw new Exception("字节数组长度不能小于4");

            byte b = data[1];
            for (int i = 2; i < data.Length - 1; i++)
            {
                b = (byte)(b ^ data[i]);
            }

            data[data.Length - 1] = b;
        }
        #endregion

        #region 计算校验码
        /// <summary>
        /// 计算校验码，字节数组最后一个字节存放校验码
        /// </summary>
        public static void CalCheckCode(List<byte> data)
        {
            if (data.Count < 4) throw new Exception("字节数组长度不能小于4");

            byte b = data[1];
            for (int i = 2; i < data.Count - 1; i++)
            {
                b = (byte)(b ^ data[i]);
            }

            data[data.Count - 1] = b;
        }
        #endregion

        #region 验证校验码
        /// <summary>
        /// 验证校验码,字节数组长度不能小于4
        /// </summary>
        public static bool ValidCheckCode(byte[] data)
        {
            if (data.Length < 4) throw new Exception("字节数组长度不能小于4");

            byte b = data[1];
            for (int i = 2; i < data.Length - 1; i++)
            {
                b = (byte)(b ^ data[i]);
            }

            if (data[data.Length - 1] == b)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion

        #region 根据货道层和货道号生成货道编号
        /// <summary>
        /// 根据货道层和货道号生成货道编号
        /// </summary>
        public static byte CreateRoadNo(int floor, int num)
        {
            int sum = (floor - 1) * 16 + num;
            if (sum >= 0 && sum <= 255)
            {
                return (byte)sum;
            }
            else
            {
                throw new Exception("根据货道层和货道号生成货道编号出错");
            }
        }
        #endregion

        #region 字节数组转int
        /// <summary>
        /// 字节数组转int
        /// </summary>
        public static int ByteArray2Int(byte[] data)
        {
            if (data.Length == 0) throw new Exception("字节数组长度必须大于0");

            int sum = 0;
            int k = 1;
            for (int i = data.Length - 1; i >= 0; i--)
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
            if (value < 0) throw new Exception("参数n必须大于或等于0");

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

        #region 数字转换成ABCD
        /// <summary>
        /// 数字转换成ABCD
        /// </summary>
        public static string NumToABCD(int num)
        {
            return ((char)(64 + num)).ToString();
        }
        #endregion

        #region ABCD转换成数字
        /// <summary>
        /// ABCD转换成数字
        /// </summary>
        public static int ABCDToNum(char c)
        {
            return (int)c - 64;
        }
        #endregion

        #region 匿名对象转换
        /// <summary>
        /// 匿名对象转换
        /// 例：var obj = CommonUtil.ChangeType(olist[0], new { Name = "", Age = 0 });
        /// </summary>
        public static T ChangeType<T>(object obj, T t)
        {
            return (T)obj;
        }
        #endregion

    }
}
