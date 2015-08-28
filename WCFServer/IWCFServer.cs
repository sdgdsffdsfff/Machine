using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using IMachineDll.Models;

namespace WCFServerDll
{
    [ServiceContract]
    public interface IWCFServer
    {
        /// <summary>
        /// 开纸硬币器
        /// </summary>
        [OperationContract]
        OperateResult OpenCoinPaper();
        /// <summary>
        /// 关纸硬币器
        /// </summary>
        [OperationContract]
        OperateResult CloseCoinPaper();
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
        [OperationContract]
        OperateResult Shipment(string com, int box, int floor, int num, bool cash, int cost, bool check);
        /// <summary>
        /// 查询投币金额
        /// </summary>
        [OperationContract]
        AmountRpt QueryAmount();
        /// <summary>
        /// 同步投币总额
        /// </summary>
        [OperationContract]
        int SyncAmount();
        /// <summary>
        /// 清除金额
        /// </summary>
        [OperationContract]
        void ClearAmount();
        /// <summary>
        /// 退币
        /// </summary>
        /// <param name="amount">退币金额(单位：分)</param>
        [OperationContract]
        OperateResult RefundMoney(int amount);
        /// <summary>
        /// 货机主机信息
        /// </summary>
        [OperationContract]
        MachineRpt MachineInfo();
        /// <summary>
        /// 货柜信息
        /// </summary>
        /// <param name="com">货柜串口号</param>
        /// <param name="box">货柜号</param>
        [OperationContract]
        BoxRpt BoxInfo(string com, int box);
        /// <summary>
        /// 查询单个货道信息
        /// </summary>
        [OperationContract]
        RoadRpt QueryRoadRpt(string com, int box, int floor, int num);
    }
}
