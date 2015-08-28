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
            OperateResult operateResult;

            try
            {
                operateResult = MachineFactory.Machine.OpenCoinPaper();
            }
            catch (Exception ex)
            {
                operateResult = new OperateResult();
                operateResult.Success = false;
                operateResult.ErrorMsg = ex.Message;
                FileLogger.LogError("开纸硬币器失败" + ex.Message);
            }

            return operateResult;
        }
        /// <summary>
        /// 关纸硬币器
        /// </summary>
        public OperateResult CloseCoinPaper()
        {
            OperateResult operateResult;

            try
            {
                operateResult = MachineFactory.Machine.CloseCoinPaper();
            }
            catch (Exception ex)
            {
                operateResult = new OperateResult();
                operateResult.Success = false;
                operateResult.ErrorMsg = ex.Message;
                FileLogger.LogError("关纸硬币器失败" + ex.Message);
            }

            return operateResult;
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
            OperateResult operateResult;

            try
            {
                IMachine machine = MachineFactory.GetMachine(com);
                operateResult = machine.Shipment(box, floor, num, cash, cost, check);
            }
            catch (Exception ex)
            {
                operateResult = new OperateResult();
                operateResult.Success = false;
                operateResult.ErrorMsg = ex.Message;
                FileLogger.LogError("出货失败" + ex.Message);
            }

            return operateResult;
        }
        /// <summary>
        /// 查询投币金额
        /// </summary>
        public AmountRpt QueryAmount()
        {
            AmountRpt amountRpt;

            try
            {
                amountRpt = MachineFactory.Machine.QueryAmount();
            }
            catch (Exception ex)
            {
                amountRpt = new AmountRpt();
                amountRpt.Amount = 0;
                amountRpt.Type = 0;
                FileLogger.LogError("查询投币金额失败" + ex.Message);
            }

            return amountRpt;
        }
        /// <summary>
        /// 同步投币总额
        /// </summary>
        public int SyncAmount()
        {
            try
            {
                return MachineFactory.Machine.SyncAmount();
            }
            catch (Exception ex)
            {
                FileLogger.LogError("同步投币总额失败" + ex.Message);
                return 0;
            }
        }
        /// <summary>
        /// 清除金额
        /// </summary>
        public void ClearAmount()
        {
            try
            {
                MachineFactory.Machine.ClearAmount();
            }
            catch (Exception ex)
            {
                FileLogger.LogError("清除金额失败" + ex.Message);
            }
        }
        /// <summary>
        /// 退币
        /// </summary>
        /// <param name="amount">退币金额(单位：分)</param>
        public OperateResult RefundMoney(int amount)
        {
            OperateResult operateResult;

            try
            {
                operateResult = MachineFactory.Machine.RefundMoney(amount);
            }
            catch (Exception ex)
            {
                operateResult = new OperateResult();
                operateResult.Success = false;
                operateResult.ErrorMsg = ex.Message;
                FileLogger.LogError("退币(金额：" + amount + "分)失败" + ex.Message);
            }

            return operateResult;
        }
        /// <summary>
        /// 货机主机信息
        /// </summary>
        public MachineRpt MachineInfo()
        {
            MachineRpt machineRpt;

            try
            {
                machineRpt = MachineFactory.Machine.MachineInfo();
            }
            catch (Exception ex)
            {
                machineRpt = new MachineRpt();
                machineRpt.HasError = true;
                machineRpt.ErrorMsg = ex.Message;
                FileLogger.LogError("获取货机主机信息失败" + ex.Message);
            }

            return machineRpt;
        }
        /// <summary>
        /// 货柜信息
        /// </summary>
        /// <param name="com">货柜串口号</param>
        /// <param name="box">货柜号</param>
        public BoxRpt BoxInfo(string com, int box)
        {
            BoxRpt boxRpt;

            try
            {
                IMachine machine = MachineFactory.GetMachine(com);
                boxRpt = machine.BoxInfo(box);
            }
            catch (Exception ex)
            {
                boxRpt = new BoxRpt();
                boxRpt.HasError = true;
                boxRpt.ErrorMsg = ex.Message;
                FileLogger.LogError("获取货柜信息失败" + ex.Message);
            }

            return boxRpt;
        }
        /// <summary>
        /// 查询单个货道信息
        /// </summary>
        public RoadRpt QueryRoadRpt(string com, int box, int floor, int num)
        {
            RoadRpt roadRpt;

            try
            {
                IMachine machine = MachineFactory.GetMachine(com);
                roadRpt = machine.QueryRoadRpt(box, floor, num);
            }
            catch (Exception ex)
            {
                roadRpt = new RoadRpt();
                roadRpt.IsOK = false;
                roadRpt.ErrorMsg = ex.Message;
                FileLogger.LogError("查询单个货道信息失败" + ex.Message);
            }

            return roadRpt;
        }

    }
}
