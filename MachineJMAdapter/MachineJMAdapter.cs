using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using IMachineDll;
using IMachineDll.Models;
using MachineJMDll;

namespace MachineJMAdapterDll
{
    /// <summary>
    /// 金码售货机适配器
    /// </summary>
    public class MachineJMAdapter : MachineJM, IMachine
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
        public MachineJMAdapter(string com)
            : base(com)
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
                string msg = string.Empty;
                bool bl = base.Connect(out msg);
                result.Success = bl;
                result.ErrorMsg = msg;
            }
            catch (Exception ex)
            {
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
                string msgCoin = string.Empty;
                string msgPaper = string.Empty;
                bool blCoin = base.CoinEnable(out msgCoin);
                bool blPaper = base.PaperMoneyEnable(out msgPaper);

                result.Success = blCoin && blPaper;
                if (!blCoin)
                {
                    result.ErrorMsg += msgCoin;
                }
                if (!blPaper)
                {
                    result.ErrorMsg += msgPaper;
                }
            }
            catch (Exception ex)
            {
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
                string msgCoin = string.Empty;
                string msgPaper = string.Empty;
                bool blCoin = base.CoinDisable(out msgCoin);
                bool blPaper = base.PaperMoneyDisable(out msgPaper);

                result.Success = blCoin && blPaper;
                if (!blCoin)
                {
                    result.ErrorMsg += msgCoin;
                }
                if (!blPaper)
                {
                    result.ErrorMsg += msgPaper;
                }
            }
            catch (Exception ex)
            {
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
            string msg = string.Empty;
            bool bl = base.Shipment(box, floor, num, cash, cost, check, out msg);
            if (bl)
            {
                bool isSuccess = false;
                int remainder = 0;
                string msgTemp = string.Empty;
                while (!base.QueryShipment(out isSuccess, out remainder, false, out msgTemp))
                {
                    Thread.Sleep(50);
                }
                if (isSuccess)
                {
                    result.Success = true;
                }
                else
                {
                    result.Success = false;
                    result.ErrorMsg = msgTemp;
                }
            }
            else
            {
                result.Success = false;
                result.ErrorMsg = msg;
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
            int type;
            int amount = base.QueryAmount(out type);
            amountRpt.Amount = amount;
            amountRpt.Type = type;
            return amountRpt;
        }
        #endregion

        #region 同步投币总额
        /// <summary>
        /// 同步投币总额
        /// </summary>
        public new int SyncAmount()
        {
            return base.SyncAmount();
        }
        #endregion

        #region 清除金额
        /// <summary>
        /// 清除金额
        /// </summary>
        public new void ClearAmount()
        {
            base.ClearAmount();
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

            string msg = string.Empty;
            bool bl = base.RefundMoney(amount, out msg);
            if (bl)
            {
                int remainder = 0;
                bool isSuccess = false;
                while (!base.QueryRefundMoney(out isSuccess, out remainder, out msg))
                {
                    Thread.Sleep(50);
                }
                if (isSuccess)
                {
                    result.Success = true;
                }
                else
                {
                    result.Success = false;
                    result.ErrorMsg = msg;
                }
            }
            else
            {
                result.Success = false;
                result.ErrorMsg = msg;
            }

            return result;
        }
        #endregion

    }
}
