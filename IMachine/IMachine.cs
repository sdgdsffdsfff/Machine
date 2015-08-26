using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMachineDll.Models;

namespace IMachineDll
{
    /// <summary>
    /// 售货机接口
    /// 2015年08月24日
    /// </summary>
    public interface IMachine
    {
        /// <summary>
        /// 联机
        /// </summary>
        OperateResult Connect();
        /// <summary>
        /// 开纸硬币器
        /// </summary>
        OperateResult OpenCoinPaper();
        /// <summary>
        /// 关纸硬币器
        /// </summary>
        OperateResult CloseCoinPaper();
        /// <summary>
        /// 出货
        /// </summary>
        /// <param name="box">货柜</param>
        /// <param name="floor">货道层</param>
        /// <param name="num">货道列</param>
        /// <param name="cash">是否现金支付</param>
        /// <param name="cost">金额(单位：分)</param>
        /// <param name="check">是否掉货检测</param>
        OperateResult Shipment(int box, int floor, int num, bool cash, int cost, bool check);
        /// <summary>
        /// 查询投币金额
        /// </summary>
        AmountRpt QueryAmount();
        /// <summary>
        /// 同步投币总额
        /// </summary>
        int SyncAmount();
        /// <summary>
        /// 清除金额
        /// </summary>
        void ClearAmount();
        /// <summary>
        /// 退币
        /// 骏鹏接口的退币 PAYOUT_IND 不会减少用户余额！
        /// </summary>
        /// <param name="amount">退币金额(单位：分)</param>
        OperateResult RefundMoney(int amount);
        /// <summary>
        /// 货机主机信息
        /// </summary>
        string MachineInfo();
        /// <summary>
        /// 货柜信息
        /// </summary>
        string BoxInfo(int box);
    }
}
