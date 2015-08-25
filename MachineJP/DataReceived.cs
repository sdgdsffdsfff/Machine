using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using MachineJPDll.Models;
using MachineJPDll.Utils;

/*
 * VMC->PC数据的接收,货机事件的分发
 */

namespace MachineJPDll
{
    partial class MachineJP
    {
        #region serialPort_DataReceived
        /// <summary>
        /// 数据接收事件的方法
        /// </summary>
        public void serialPort_DataReceived(object obj, SerialDataReceivedEventArgs args)
        {
            byte[] receiveData = ReadPort();

            if (CommonUtil.ValidReceiveData(receiveData)) //只处理验证正确的数据，不正确的数据抛弃不处理
            {
                LogHelper.Log(LogMsgType.Info, false, receiveData);
                byte SN = CommonUtil.GetSN(receiveData);
                MT mt = new MT(receiveData);

                #region 轮询(POLL)
                if (mt.Type == 0x03)
                {
                    if (m_CommandList.Count > 0)
                    {
                        WritePort(m_CommandList[0], SN);
                        m_CommandList.RemoveAt(0);
                    }
                    else
                    {
                        //发送ACK消息
                        SendACK(SN);
                    }
                }
                #endregion

                #region 发送ACK消息
                if (CommonUtil.NeedACK(receiveData))
                {
                    SendACK(SN); //发送ACK消息
                }
                #endregion

                #region VMC系统参数
                if (mt.Type == 0x05)
                {
                    m_ReceiveDataCollection.Add(mt.Type, mt.Subtype, receiveData);
                }
                #endregion

                #region ACK_RPT或NAK_RPT
                if (mt.Type == 0x01 //ACK_RPT
                    || mt.Type == 0x02) //NAK_RPT
                {
                    if (m_WaitResultMTList.Count > 0)
                    {
                        m_ReceiveDataCollection.Add(m_WaitResultMTList[0].Type, m_WaitResultMTList[0].Subtype, receiveData);
                        m_WaitResultMTList.RemoveAt(0);
                    }
                }
                #endregion

                #region INFO_RPT 数据报告
                if (mt.Type == 0x11)
                {
                    #region 纸币器信息
                    if (mt.Subtype == 16)
                    {
                        m_ReceiveDataCollection.Add(mt.Type, mt.Subtype, receiveData);
                    }
                    #endregion

                    #region 硬币器信息
                    if (mt.Subtype == 17)
                    {
                        m_ReceiveDataCollection.Add(mt.Type, mt.Subtype, receiveData);
                    }
                    #endregion

                    #region 用户投币余额
                    if (mt.Subtype == 3)
                    {
                        m_ReceiveDataCollection.Add(mt.Type, mt.Subtype, receiveData);
                    }
                    #endregion
                }
                #endregion

                #region VENDOUT_RPT 出货报告
                if (mt.Type == 0x08)
                {
                    m_ReceiveDataCollection.Add(mt.Type, mt.Subtype, receiveData);
                }
                #endregion

                #region STATUS_RPT VMC整机状态报告
                if (mt.Type == 0x0D)
                {
                    m_ReceiveDataCollection.Add(mt.Type, mt.Subtype, receiveData);
                }
                #endregion

                #region SALEPRICE_IND 设置当前商品售价
                if (mt.Type == 0x8E)
                {
                    m_ReceiveDataCollection.Add(mt.Type, mt.Subtype, receiveData);
                }
                #endregion

                #region PAYIN_RPT VMC发送现金投币报告给PC
                if (mt.Type == 0x06)
                {
                    m_ReceiveDataCollection.Add(mt.Type, mt.Subtype, receiveData);
                }
                #endregion

                #region PAYOUT_RPT 出币报告
                if (mt.Type == 0x07)
                {
                    m_ReceiveDataCollection.Add(mt.Type, mt.Subtype, receiveData);
                }
                #endregion

                #region COST_RPT VMC扣款报告
                if (mt.Type == 0x10)
                {
                    m_ReceiveDataCollection.Add(mt.Type, mt.Subtype, receiveData);
                }
                #endregion

                #region ACTION_RPT 售货机行为报告
                if (mt.Type == 0x0B)
                {
                    m_ReceiveDataCollection.Add(mt.Type, mt.Subtype, receiveData);
                }
                #endregion

                #region HUODAO_RPT VMC货道报告
                if (mt.Type == 0x0E)
                {
                    m_ReceiveDataCollection.Add(mt.Type, mt.Subtype, receiveData);
                }
                #endregion
            }
            else //接收到的数据没有验证通过
            {
                LogHelper.LogException(LogMsgType.Error, false, "数据异常", receiveData);
            }
        }
        #endregion

    }
}
