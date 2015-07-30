using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MachineJPDll.Utils;

namespace MachineJPDll.Models
{
    /// <summary>
    /// VMC出货报告
    /// </summary>
    public class VendoutRpt
    {
        /// <summary>
        /// 从串口读取的通过验证的数据
        /// </summary>
        private byte[] m_data;

        /// <summary>
        /// VMC出货报告
        /// </summary>
        /// <param name="data">从串口读取的通过验证的数据</param>
        public VendoutRpt(byte[] data)
        {
            m_data = data;
        }

        /// <summary>
        /// 出货的货柜号
        /// </summary>
        public int device
        {
            get
            {
                return m_data[5];
            }
        }

        /// <summary>
        /// status=0，出货成功完成 status=2，出货失败；出货失败的情况，参见《出货失败和货道故障》
        /// </summary>
        public int status
        {
            get
            {
                return m_data[6];
            }
        }

        /// <summary>
        /// 实际出货货道
        /// 该hd_id 指PC 逻辑货道，VMC 需要自动将物理货道映射到逻辑货道，参见《VMC 物理货道和PC 逻辑货道》
        /// </summary>
        public int hd_id
        {
            get
            {
                return m_data[7];
            }
        }

        /// <summary>
        /// 如果是现金购物，则type=0
        /// 如果是VENDOUT_IND 发起的出货，则type=VENDOUT_IND.type type 的具体含义，参见《INFO_RPT（type=6）》
        /// </summary>
        public int type
        {
            get
            {
                return m_data[8];
            }
        }

        /// <summary>
        /// 如果是非现金购物，cost=0
        /// 如果是现金购物，且status=0，cost 代表用户购买商品的花费
        /// 注意：对于STATUS=2 的出货失败情况，如果是现金购物（即TYPE=0），则 COST 字段
        /// 表示*正确返还给用户的金额，正常情况下COST 应该与商品售价相同，如果COST 等于
        /// 0，说明本次出货失败没有给用户返还金额（比如找零量为0 的情况下使用纸币进行的购买）；参见《用户投币及找零》。
        /// </summary>
        public int cost
        {
            get
            {
                return CommonUtil.ByteArray2Int(m_data, 9, 2);
            }
        }

        /// <summary>
        /// 出货完成后，用户投币余额
        /// </summary>
        public int total_value
        {
            get
            {
                return CommonUtil.ByteArray2Int(m_data, 11, 2);
            }
        }

        /// <summary>
        /// hd_id 对应的货道中，剩余商品个数
        /// </summary>
        public int huodao
        {
            get
            {
                return m_data[13];
            }
        }

    }
}
