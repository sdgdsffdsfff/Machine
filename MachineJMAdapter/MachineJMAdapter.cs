using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using IMachineDll;
using IMachineDll.Models;
using MachineJMAdapterDll.Models;
using MachineJMAdapterDll.Utils;
using MachineJMDll;
using MachineJMDll.Models;

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

        #region 货机主机信息
        /// <summary>
        /// 货机主机信息
        /// </summary>
        public MachineRpt MachineInfo()
        {
            MachineRpt machineRpt = new MachineRpt();
            //主板
            MainBoardInfo mainBoardInfo = base.QueryMainBoardInfo();
            machineRpt.VmcStatus = mainBoardInfo.ToString();

            return machineRpt;
        }
        #endregion

        #region 货柜信息
        /// <summary>
        /// 货柜信息
        /// </summary>
        public BoxRpt BoxInfo(int box)
        {
            BoxRpt boxRpt = new BoxRpt();

            //机器设备状态
            BoxStatus boxStatus = base.QueryBoxStatus(box);
            boxRpt.BoxStatus += string.Format("机器设备状态：\r\n{0}\r\n", boxStatus.ToString());
            //制冷压缩机/风机/照明/除雾/广告灯/工控机等设备状态
            EquipmentsStatus equipmentsStatus = base.QueryEquipmentsStatus(box);
            boxRpt.BoxStatus += string.Format("制冷压缩机/风机/照明/除雾/广告灯/工控机等设备状态：\r\n{0}\r\n", equipmentsStatus.ToString());

            //制冷压缩机/照明/除雾/广告灯/工控机等设备控制策略参数
            EquipmentInfo equipmentAll = base.QueryEquipmentAll(box);
            boxRpt.BoxSetup += equipmentAll.ToString();

            //货道信息
            RoadModelCollection roadModelCollection = JMBoxConfigUtil.GetRoadsConfig(box);
            foreach (RoadModel road in roadModelCollection.RoadList)
            {
                RoadInfo roadInfo = base.QueryRoadInfo(box, road.Floor, road.Num);

                RoadRpt roadRpt = new RoadRpt();
                roadRpt.Floor = road.Floor;
                roadRpt.Num = road.Num;
                roadRpt.IsOK = roadInfo.IsOK;
                roadRpt.ErrorMsg = roadInfo.ErrorMsg;
                roadRpt.Price = roadInfo.Price;

                boxRpt.RoadCollection.RoadList.Add(roadRpt);
            }
            boxRpt.RoadCollection.FloorCount = roadModelCollection.FloorCount;

            return boxRpt;
        }
        #endregion

        #region 查询单个货道信息
        /// <summary>
        /// 查询单个货道信息
        /// </summary>
        public RoadRpt QueryRoadRpt(int box, int floor, int num)
        {
            RoadRpt roadRpt = new RoadRpt();
            int cost;
            string status;
            bool bl = base.QueryRoadInfo(box, floor, num, out cost, out status);
            roadRpt.Floor = floor;
            roadRpt.Num = num;
            if (bl)
            {
                roadRpt.IsOK = true;
                roadRpt.Price = cost;
            }
            else
            {
                roadRpt.IsOK = false;
                roadRpt.ErrorMsg = status;
            }
            return roadRpt;
        }
        #endregion

    }
}
