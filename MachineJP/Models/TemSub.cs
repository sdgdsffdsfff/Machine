using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MachineJPDll.Enums;

namespace MachineJPDll.Models
{
    /// <summary>
    /// 货仓温度
    /// </summary>
    public class TemSub
    {
        /// <summary>
        /// 原始数据
        /// </summary>
        public byte Data { get; set; }
        /// <summary>
        /// 货仓温度状态
        /// </summary>
        public TemSubSt TemSubSt { get; set; }
        /// <summary>
        /// 货仓温度值(单位：℃)
        /// </summary>
        public int Tem { get; set; }

        /// <summary>
        /// 货仓温度
        /// </summary>
        /// <param name="data">原始数据</param>
        public TemSub(byte data)
        {
            Data = data;

            switch ((TemSubSt)data)
            {
                case TemSubSt.故障:
                    this.TemSubSt = TemSubSt.故障;
                    break;
                case TemSubSt.不存在此货仓:
                    this.TemSubSt = TemSubSt.不存在此货仓;
                    break;
                case TemSubSt.该温度无意义:
                    this.TemSubSt = TemSubSt.该温度无意义;
                    break;
                default:
                    this.TemSubSt = TemSubSt.正常;
                    if (data <= 0x80)
                    {
                        Tem = data;
                    }
                    else
                    {
                        Tem = 0x80 - data;
                    }
                    break;
            }
        }

        public override string ToString()
        {
            if (this.TemSubSt == TemSubSt.正常)
            {
                return this.Tem.ToString() + "℃";
            }
            else
            {
                return this.TemSubSt.ToString();
            }
        }
    }
}
