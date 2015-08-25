using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MachineJPDll.Models
{
    /// <summary>
    /// 消息类型
    /// </summary>
    public class MT
    {
        /// <summary>
        /// 数据(不一定要通过验证，包含type和subtype即可)
        /// </summary>
        private byte[] m_data;

        /// <summary>
        /// 消息类型
        /// </summary>
        /// <param name="data">数据(不一定要通过验证，包含type和subtype即可)</param>
        public MT(byte[] data)
        {
            m_data = data;
        }

        /// <summary>
        /// 消息类型
        /// </summary>
        /// <param name="data">数据(不一定要通过验证，包含type和subtype即可)</param>
        public MT(List<byte> data)
        {
            m_data = data.ToArray();
        }

        /// <summary>
        /// 消息类型
        /// </summary>
        public byte Type
        {
            get
            {
                return m_data[4];
            }
        }

        /// <summary>
        /// 消息子类型
        /// </summary>
        public byte Subtype
        {
            get
            {
                if (Type == 0x85         //CONTROL_IND
                    || Type == 0x8C      //GET_SETUP
                    || Type == 0x11      //INFO_RPT
                    || Type == 0x0B)     //ACTION_RPT
                {
                    return m_data[5];
                }
                return 0x00;
            }
        }

        /// <summary>
        /// 是否存在消息子类型
        /// </summary>
        public bool HasSubtype
        {
            get
            {
                if (Subtype == 0x00)
                {
                    return false;
                }
                return true;
            }
        }

    }
}
