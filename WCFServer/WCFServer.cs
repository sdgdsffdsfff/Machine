using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using CommonDll;
using IMachineDll;
using IMachineDll.Models;
using MachineFactoryDll;

namespace WCFServerDll
{
    [ServiceBehavior()]
    public class WCFServer : IWCFServer
    {
        /// <summary>
        /// 开纸硬币器
        /// </summary>
        public OperateResult OpenCoinPaper()
        {
            return MachineFactory.Machine.OpenCoinPaper();
        }
        /// <summary>
        /// 关纸硬币器
        /// </summary>
        public OperateResult CloseCoinPaper()
        {
            return MachineFactory.Machine.CloseCoinPaper();
        }
        /// <summary>
        /// 出货
        /// </summary>
        /// <param name="com">串口号</param>
        /// <param name="box">货柜</param>
        /// <param name="floor">货道层</param>
        /// <param name="num">货道列</param>
        /// <param name="cash">是否现金支付</param>
        /// <param name="cost">金额(单位：分)</param>
        /// <param name="check">是否掉货检测</param>
        public OperateResult Shipment(string com, int box, int floor, int num, bool cash, int cost, bool check)
        {
            IMachine machine = MachineFactory.GetMachine(com);
            return machine.Shipment(box, floor, num, cash, cost, check);
        }
        /// <summary>
        /// 查询投币金额
        /// </summary>
        public AmountRpt QueryAmount()
        {
            return MachineFactory.Machine.QueryAmount();
        }
        /// <summary>
        /// 同步投币总额
        /// </summary>
        public int SyncAmount()
        {
            return MachineFactory.Machine.SyncAmount();
        }
        /// <summary>
        /// 清除金额
        /// </summary>
        public void ClearAmount()
        {
            MachineFactory.Machine.ClearAmount();
        }
        /// <summary>
        /// 退币
        /// </summary>
        /// <param name="amount">退币金额(单位：分)</param>
        public OperateResult RefundMoney(int amount)
        {
            return MachineFactory.Machine.RefundMoney(amount);
        }
        /// <summary>
        /// 货机主机信息
        /// </summary>
        public string MachineInfo()
        {
            return MachineFactory.Machine.MachineInfo();
        }
        /// <summary>
        /// 货柜信息
        /// </summary>
        public string BoxInfo(int box)
        {
            return MachineFactory.Machine.BoxInfo(box);
        }

    }
}
