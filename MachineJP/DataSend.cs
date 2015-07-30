using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using MachineJPDll.Enums;
using MachineJPDll.Models;
using MachineJPDll.Utils;

/*
 * PC->VMC数据的发送(并非直接发送,只是添加到发送列表)
 */

namespace MachineJPDll
{
    partial class MachineJP
    {
        #region GET_SETUP
        /// <summary>
        /// PC通知VMC发送VMC_SETUP
        /// </summary>
        public VmcSetup GET_SETUP()
        {
            List<byte> sendData = new List<byte>() { 0xE5, 0x00, 0x00, 0x40, 0x90 };
            m_CommandList.Add(new Cmd(sendData));

            byte[] receiveData = WaitReceive(0x05);
            if (receiveData != null)
            {
                return new VmcSetup(receiveData);
            }
            return null;
        }
        #endregion

        #region CONTROL_IND PC控制售货机完成对应的动作
        /// <summary>
        /// PC控制VMC
        /// </summary>
        public bool CONTROL_IND(byte subtype, byte[] value)
        {
            List<byte> sendData = new List<byte>() { 0xE5, 0x00, 0x00, 0x41, 0x85 };
            sendData.Add(subtype);
            if (value != null && value.Length > 0)
            {
                sendData.AddRange(value);
            }
            m_CommandList.Add(new Cmd(sendData, new MT(sendData)));

            return SendSuccess(0x85, subtype);
        }
        #endregion

        #region 设置小数点位数
        /// <summary>
        /// 设置小数点位数
        /// 用于PC 通知VMC，双方的金额数据比例系数关系，PC 每次启动时，都会给
        /// VMC 下发一次type=18 的消息，VMC 需要自己永久保存该数据，直到被PC 再
        /// 次更新。
        /// 取值范围：0、1、2 分别表示以 元、 角 、分 为单位
        /// </summary>
        public bool SetDecimalPlaces(int data)
        {
            return CONTROL_IND(18, new byte[] { (byte)data });
        }
        #endregion

        #region GET_STATUS PC通知VMC发送STATUS_RPT
        /// <summary>
        /// PC通知VMC发送STATUS_RPT
        /// </summary>
        public StatusRpt GET_STATUS()
        {
            List<byte> sendData = new List<byte>() { 0xE5, 0x00, 0x00, 0x40, 0x86 };
            m_CommandList.Add(new Cmd(sendData));

            byte[] receiveData = WaitReceive(0x0D);
            if (receiveData != null)
            {
                return new StatusRpt(receiveData);
            }
            return null;
        }
        #endregion

        #region GET_INFO PC通知VMC发送INFO_RPT
        /// <summary>
        /// PC通知VMC发送INFO_RPT
        /// </summary>
        public byte[] GET_INFO(byte subtype)
        {
            List<byte> sendData = new List<byte>() { 0xE5, 0x00, 0x00, 0x40, 0x8C };
            sendData.Add(subtype);
            m_CommandList.Add(new Cmd(sendData));

            return WaitReceive(0x11);
        }
        #endregion

        #region VENDOUT_IND 出货
        /// <summary>
        /// PC出货指示
        /// </summary>
        /// <param name="device">售货机的箱体号 例如柜1 为 0x01 以此类推</param>
        /// <param name="method">method =1：VMC 通过商品ID 指示出货，如果商品ID 不存在，回复NAK_RPT method =2：VMC 通过货道ID 指示VMC 出货，如果货道ID 不存在，回复NAK_RPT</param>
        /// <param name="sp_id_hd_id">sp_id：通过商品ID 指示VMC 出货 hd_id：通过货道ID 指示VMC 出货</param>
        /// <param name="type">如果type=0，cost 代表本次出货扣款金额 如果TYPE 不为0，则COST 必须为0</param>
        /// <param name="cost">cost 代表本次出货扣款金额</param>
        public VendoutRpt VENDOUT_IND(byte device, byte method, byte sp_id_hd_id, byte type, int cost)
        {
            List<byte> sendData = new List<byte>() { 0xE5, 0x00, 0x00, 0x41, 0x83 };
            sendData.AddRange(new byte[] { device, method, sp_id_hd_id, type });
            sendData.AddRange(CommonUtil.Int2ByteArray(cost, 2));
            m_CommandList.Add(new Cmd(sendData, new MT(sendData)));

            if (SendSuccess(0x83, 0x00))
            {
                byte[] receiveData = WaitReceive(0x08);
                if (receiveData != null)
                {
                    return new VendoutRpt(receiveData);
                }
            }
            return null;
        }
        #endregion

        #region HUODAO_SET_IND 设置货道商品数量
        /// <summary>
        /// PC通知VMC，当前货道对应商品的数量等信息
        /// </summary>
        /// <param name="device">表示箱柜号</param>
        /// <param name="huodao">zyxxxxxx “z”固定填0 “y”固定填0 “xxxxxx”，表示商品余量，如果商品余量大于63，则统一为63</param>
        public bool HUODAO_SET_IND(byte device, List<int> huodao)
        {
            List<byte> sendData = new List<byte>() { 0xE5, 0x00, 0x00, 0x41, 0x8F };
            sendData.Add(device);
            for (int i = 0; i < huodao.Count; i++)
            {
                if (huodao[i] > 63)
                {
                    huodao[i] = 63;
                }
            }
            sendData.AddRange(huodao.ConvertAll<byte>(a => (byte)a));
            m_CommandList.Add(new Cmd(sendData, new MT(sendData)));

            return SendSuccess(0x8F, 0x00);
        }
        #endregion

        #region SALEPRICE_IND 设置当前商品售价
        /// <summary>
        /// PC通知VMC，当前商品售价
        /// </summary>
        /// <param name="device">表示箱柜号</param>
        /// <param name="type">表示设置单价的方式；Type = 0：为按商品ID 发送单价，可以变长发送，商品种类最大不超过80 个；Type = 1: 为按货道号发送，固定发送80 个货道的单价信息</param>
        /// <param name="sp_price">商品售价</param>
        public bool SALEPRICE_IND(byte device, byte type, List<int> sp_price)
        {
            List<byte> sendData = new List<byte>() { 0xE5, 0x00, 0x00, 0x41, 0x8E };
            sendData.Add(device);
            sendData.Add(type);
            sendData.AddRange(sp_price.ConvertAll<byte>(a => (byte)a));
            m_CommandList.Add(new Cmd(sendData, new MT(sendData)));

            return SendSuccess(0x8E, 0x00);
        }
        #endregion

        #region PAYOUT_IND PC指示VMC出币
        /// <summary>
        /// PC指示VMC出币
        /// </summary>
        /// <param name="device">出币设备</param>
        /// <param name="value">本次出币总金额</param>
        /// <param name="type">出币类型 无需理解type 的含义，只需要在出币完成后的PAYOUT_RPT 中将该type 值回传给PC 即可</param>
        public PayoutRpt PAYOUT_IND(PayoutType device, int value, byte type)
        {
            List<byte> sendData = new List<byte>() { 0xE5, 0x00, 0x00, 0x41, 0x89 };
            sendData.Add((byte)device);
            sendData.AddRange(CommonUtil.Int2ByteArray(value, 2));
            sendData.Add(type);
            m_CommandList.Add(new Cmd(sendData, new MT(sendData)));

            if (SendSuccess(0x89, 0x00))
            {
                byte[] receiveData = WaitReceive(0x07);
                if (receiveData != null)
                {
                    return new PayoutRpt(receiveData);
                }
            }
            return null;
        }
        #endregion

        #region COST_IND PC扣款指示
        /// <summary>
        /// PC扣款指示
        /// </summary>
        /// <param name="device">device=0，从用户投币总额中扣款；优先从用户非暂存金额中扣除（纸币尽量滞后压钞），参见《现金扣款顺序》</param>
        /// <param name="value">扣款金额</param>
        /// <param name="type">VMC 不用理解type 的含义，只需上报对应的COST_RPT 时回传即可</param>
        public CostRpt COST_IND(byte device, int value, byte type)
        {
            List<byte> sendData = new List<byte>() { 0xE5, 0x00, 0x00, 0x41, 0x8B };
            sendData.Add(device);
            sendData.AddRange(CommonUtil.Int2ByteArray(value, 2));
            sendData.Add(type);
            m_CommandList.Add(new Cmd(sendData, new MT(sendData)));

            if (SendSuccess(0x8B, 0x00))
            {
                byte[] receiveData = WaitReceive(0x10);
                if (receiveData != null)
                {
                    return new CostRpt(receiveData);
                }
            }
            return null;
        }
        #endregion

        #region GET_HUODAO PC通知VMC上报HUODAO_RPT
        /// <summary>
        /// PC通知VMC上报HUODAO_RPT
        /// </summary>
        /// <param name="device">箱柜号</param>
        public HuoDaoRpt GET_HUODAO(byte device)
        {
            List<byte> sendData = new List<byte>() { 0xE5, 0x00, 0x00, 0x40, 0x8A };
            sendData.Add(device);
            m_CommandList.Add(new Cmd(sendData));

            byte[] receiveData = WaitReceive(0x0E);
            if (receiveData != null)
            {
                return new HuoDaoRpt(receiveData);
            }
            return null;
        }
        #endregion

        #region SET_HUODAO PC发送配置货道信息
        /// <summary>
        /// PC发送配置货道信息
        /// </summary>
        /// <param name="Cabinet">箱柜号</param>
        /// <param name="hd_no">货道逻辑编号，十进制</param>
        /// <param name="Hd_id">商品ID号</param>
        /// <param name="Hd_count">货道剩余量</param>
        /// <param name="Hd_price">货道单价</param>
        /// <param name="Reserved">保留字段VMC忽略此字段，PC最好将此字段置为0</param>
        public bool SET_HUODAO(byte Cabinet, int hd_no, int Hd_id, int Hd_count, int Hd_price, int Reserved = 0)
        {
            List<byte> sendData = new List<byte>() { 0xE5, 0x00, 0x00, 0x41, 0x93 };
            sendData.Add(Cabinet);
            sendData.Add((byte)hd_no);
            sendData.Add((byte)Hd_id);
            sendData.Add((byte)Hd_count);
            sendData.AddRange(CommonUtil.Int2ByteArray(Hd_price, 2));
            sendData.AddRange(CommonUtil.Int2ByteArray(Reserved, 2));
            m_CommandList.Add(new Cmd(sendData, new MT(sendData)));

            return SendSuccess(0x93, 0x00);
        }
        #endregion

        #region 开启纸硬币器
        /// <summary>
        /// 现金收银模组（纸币器、硬币器）开关
        /// </summary>
        /// <param name="open">true:开,false:关</param>
        public bool CtrlCoinPaper(bool open)
        {
            if (open)
            {
                return CONTROL_IND(2, new byte[] { 1 });
            }
            else
            {
                return CONTROL_IND(2, new byte[] { 0 });
            }
        }
        #endregion

        #region 找零
        /// <summary>
        /// 找零
        /// 与手工拨弄物理找零开关相同
        /// </summary>
        public bool MakeChange()
        {
            return CONTROL_IND(6, new byte[] { 0 });
        }
        #endregion

        #region 获取硬币器信息
        /// <summary>
        /// 获取硬币器信息
        /// </summary>
        public InfoRpt_17 GetCoinInfo()
        {
            return new InfoRpt_17(GET_INFO(17));
        }
        #endregion

        #region 获取纸币器信息
        /// <summary>
        /// 获取纸币器信息
        /// </summary>
        public InfoRpt_16 GetPaperInfo()
        {
            return new InfoRpt_16(GET_INFO(16));
        }
        #endregion

        #region 获取现金投币报告
        /// <summary>
        /// 获取现金投币报告
        /// </summary>
        public PayinRpt GetPayinRpt()
        {
            byte[] receiveData = Receive(0x06, 0x00);
            if (receiveData != null)
            {
                return new PayinRpt(receiveData);
            }
            return null;
        }
        #endregion

        #region 获取用户投币余额
        /// <summary>
        /// 获取用户投币余额
        /// </summary>
        public InfoRpt_3 GetRemaiderAmount()
        {
            byte[] receiveData = WaitReceive(0x11, 0x003);
            if (receiveData != null)
            {
                return new InfoRpt_3(receiveData);
            }
            return null;
        }
        #endregion

    }
}
