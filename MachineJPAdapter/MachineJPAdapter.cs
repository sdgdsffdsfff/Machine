using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using CommonDll;
using IMachineDll;
using IMachineDll.Models;
using MachineJPDll;
using MachineJPDll.Enums;
using MachineJPDll.Models;

namespace MachineJPAdapterDll
{
    /// <summary>
    /// 骏鹏售货机适配器
    /// </summary>
    public class MachineJPAdapter : MachineJP, IMachine
    {
        #region 属性或变量
        /// <summary>
        /// 串口号
        /// </summary>
        private string m_com;
        #endregion

        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="com">串口号，如：COM1</param>
        public MachineJPAdapter(string com)
        {
            m_com = com;
        }
        #endregion

        #region 联机
        /// <summary>
        /// 联机
        /// </summary>
        public OperateResult Connect()
        {
            OperateResult result = new OperateResult();

            try
            {
                base.Init(this.m_com);
                result.Success = true;
            }
            catch (Exception ex)
            {
                FileLogger.LogError("联机失败：" + ex.Message);
                result.Success = false;
                result.ErrorMsg = ex.Message;
            }

            return result;
        }
        #endregion

        #region 开纸硬币器
        /// <summary>
        /// 开纸硬币器
        /// </summary>
        public OperateResult OpenCoinPaper()
        {
            OperateResult result = new OperateResult();

            try
            {
                bool bl = base.CtrlCoinPaper(true);
                result.Success = bl;
            }
            catch (Exception ex)
            {
                FileLogger.LogError("开纸硬币器失败：" + ex.Message);
                result.Success = false;
                result.ErrorMsg = ex.Message;
            }

            return result;
        }
        #endregion

        #region 关纸硬币器
        /// <summary>
        /// 关纸硬币器
        /// </summary>
        public OperateResult CloseCoinPaper()
        {
            OperateResult result = new OperateResult();

            try
            {
                bool bl = base.CtrlCoinPaper(false);
                result.Success = bl;
            }
            catch (Exception ex)
            {
                FileLogger.LogError("关纸硬币器失败：" + ex.Message);
                result.Success = false;
                result.ErrorMsg = ex.Message;
            }

            return result;
        }
        #endregion

        #region 出货
        /// <summary>
        /// 出货
        /// </summary>
        /// <param name="box">货柜</param>
        /// <param name="floor">货道层</param>
        /// <param name="num">货道列</param>
        /// <param name="cash">是否现金支付</param>
        /// <param name="cost">金额(单位：分)</param>
        /// <param name="check">是否掉货检测</param>
        public OperateResult Shipment(int box, int floor, int num, bool cash, int cost, bool check)
        {
            OperateResult result = new OperateResult();

            byte hd_id = 0x00;

            #region 计算货道
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load("MachineConfig.xml");
            XmlNode machineNode = xmlDoc.SelectSingleNode("machine");
            for (int i = 0; i < machineNode.ChildNodes.Count; i++)
            {
                XmlNode boxNode = machineNode.ChildNodes[i];
                if (boxNode.Attributes["com"].Value == m_com)
                {
                    int colcount = int.Parse(boxNode.Attributes["colcount"].Value);
                    hd_id = (byte)((floor - 1) * colcount + num);
                    break;
                }
            }
            #endregion

            byte type = cash ? (byte)0x00 : (byte)0x01;

            VendoutRpt vendoutRpt = base.VENDOUT_IND((byte)box, 2, hd_id, type, cost);
            if (vendoutRpt.status == 0)
            {
                if (cash)
                {
                    //出货完成扣款
                    CostRpt costRpt = base.COST_IND(0, cost, (byte)0x00);
                    if (costRpt.value == cost)
                    {
                        result.Success = true;
                    }
                    else
                    {
                        result.Success = false;
                        result.ErrorMsg = "扣款失败";
                        FileLogger.LogError(string.Format("扣款失败,出货柜号：{0}，层：{1}，列：{2}", box, floor, num));
                    }
                }
                else
                {
                    result.Success = true;
                }
            }
            else
            {
                result.Success = false;
                result.ErrorMsg = "出货失败";
                FileLogger.LogError(string.Format("出货失败,柜号：{0}，层：{1}，列：{2}", box, floor, num));
            }

            return result;
        }
        #endregion

        #region 查询投币金额
        /// <summary>
        /// 查询投币金额
        /// </summary>
        public AmountRpt QueryAmount()
        {
            AmountRpt amountRpt = new AmountRpt();
            PayinRpt payinRpt = base.GetPayinRpt();
            amountRpt.Amount = payinRpt.value;
            if (payinRpt.dt == PayinType.硬币投币)
            {
                amountRpt.Type = 0;
            }
            else
            {
                amountRpt.Type = 1;
            }
            return amountRpt;
        }
        #endregion

        #region 同步投币总额
        /// <summary>
        /// 同步投币总额
        /// </summary>
        public int SyncAmount()
        {
            InfoRpt_3 infoRpt_3 = base.GetRemaiderAmount();
            return infoRpt_3.total_value;
        }
        #endregion

        #region 清除金额
        /// <summary>
        /// 清除金额
        /// </summary>
        public void ClearAmount()
        {
            base.COST_IND(0, this.SyncAmount(), (byte)0x00);
        }
        #endregion

        #region 退币
        /// <summary>
        /// 退币
        /// </summary>
        /// <param name="amount">退币金额(单位：分)</param>
        public OperateResult RefundMoney(int amount)
        {
            OperateResult result = new OperateResult();

            int yb = amount % 500;
            int zb = amount - yb;
            PayoutRpt ybRpt = base.PAYOUT_IND(PayoutType.硬币出币, yb, (byte)0x00);
            PayoutRpt zbRpt = base.PAYOUT_IND(PayoutType.纸币出币, zb, (byte)0x00);
            //由于PAYOUT_IND 不会减少用户余额，退币后扣款
            CostRpt costRpt = base.COST_IND(0, amount, (byte)0x00);
            if (costRpt.value == amount)
            {
                result.Success = true;
            }
            else
            {
                result.Success = false;
                result.ErrorMsg = "扣款失败";
                FileLogger.LogError(string.Format("扣款失败，金额：{0}分", amount));
            }

            if (amount > 0 && ybRpt.value == 0 && zbRpt.value == 0)
            {
                result.Success = false;
                result.ErrorMsg = "退币失败";
                FileLogger.LogError(string.Format("退币失败，应退金额：{0}分，实退金额：{1}分", amount, ybRpt.value + zbRpt.value));
            }

            if (ybRpt.value + zbRpt.value < amount)
            {
                result.Success = false;
                result.ErrorMsg = "退币失败";
                FileLogger.LogError(string.Format("退币失败，应退金额：{0}分，实退金额：{1}分", amount, ybRpt.value + zbRpt.value));
            }

            return result;
        }
        #endregion

        #region 货机主机信息
        /// <summary>
        /// 货机主机信息
        /// </summary>
        public string MachineInfo()
        {
            //VMC参数
            VmcSetup vmcSetup = base.GET_SETUP();
            //VMC状态
            StatusRpt statusRpt = base.GET_STATUS();
            //硬币器
            InfoRpt_17 infoRpt_17 = base.GetCoinInfo();
            //纸币器
            InfoRpt_16 infoRpt_16 = base.GetPaperInfo();

            return string.Format("VMC系统参数：{0}\r\nVMC状态：{1}\r\n硬币器信息：{2}\r\n纸币器信息：{3}\r\n",
                vmcSetup.ToString(), statusRpt.ToString(), infoRpt_17.ToString(), infoRpt_16.ToString());
        }
        #endregion

        #region 货柜信息
        /// <summary>
        /// 货柜信息
        /// </summary>
        public string BoxInfo(int box)
        {
            HuoDaoRpt huoDaoRpt = base.GET_HUODAO((byte)box);
            return "货道信息：\r\n" + huoDaoRpt.ToString() + "\r\n";
        }
        #endregion

    }
}
