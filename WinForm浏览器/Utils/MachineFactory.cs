using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyWebBrowser.Models;
using MachineDll;
using MyWebBrowser.Enums;
using System.Threading;
using MachineJPDll.Models;
using MachineJPDll.Enums;
using MachineDll.Models;
using Newtonsoft.Json.Linq;
using System.Data;

namespace MyWebBrowser.Utils
{
    /// <summary>
    /// 售货机接口工厂类
    /// </summary>
    public class MachineFactory
    {
        #region 变量
        /// <summary>
        /// 售货机接口类型列表
        /// </summary>
        public static List<MachineModel> MachineList { get; set; }
        #endregion

        #region 初始化
        public static void Init(List<Dictionary<string, string>> list)
        {
            MachineList = new List<MachineModel>();

            foreach (Dictionary<string, string> dict in list)
            {
                MachineModel machineModel;

                switch (dict["comFileName"])
                {
                    case "Machine":
                        machineModel = new MachineModel(dict["comName"], MachineType.金码);
                        break;
                    case "MachineJP":
                        machineModel = new MachineModel(dict["comName"], MachineType.骏鹏);
                        break;
                    default:
                        throw new Exception("请检查mas_box_info表的comFileName字段值");
                }

                MachineList.Add(machineModel);
            }
        }
        #endregion

        #region 获取接口
        private static MachineModel GetMachine(string com)
        {
            return MachineList.Find(item => item.Com == com);
        }
        #endregion

        #region 出货
        /// <summary>
        /// 出货
        /// </summary>
        /// <param name="cost">价格(单位：分)</param>
        /// <param name="checkDrop">是否掉货检测</param>
        /// <param name="orderId">订单ID</param>
        public static bool Shipment(int cost, bool checkDrop, string orderId, out string msg)
        {
            string boxId = HttpRequestUtil.PostUrl("GetBoxId", "orderId=" + orderId);
            string com = HttpRequestUtil.PostUrl("GetCom", "boxId=" + boxId);
            MachineModel machine = GetMachine(com);
            List<Dictionary<string, string>> roadList = JsonHelper.JsonToListDic(HttpRequestUtil.PostUrl("GetRoad", "orderId=" + orderId));

            #region 金码
            if (machine.Type == MachineType.金码)
            {
                int remainder = 0;
                bool isSuccess = true;

                foreach (Dictionary<string, string> road in roadList)
                {
                    for (int i = 0; i < int.Parse(road["productNum"]); i++)
                    {
                        int boxNo = int.Parse(HttpRequestUtil.PostUrl("GetBoxNo", "boxId=" + boxId));
                        cost = (int)(double.Parse(road["productPrice"]) * 100);
                        if (machine.Machine.Connect(out msg) && machine.Machine.Shipment(boxNo, int.Parse(road["layerNo"].ToString()), int.Parse(road["roadNo"]), true, cost, checkDrop, out msg))
                        {
                            bool isSucessTemp = false;
                            string msgTemp = null;
                            while (!machine.Machine.QueryShipment(out isSucessTemp, out remainder, false, out msgTemp))
                            {
                                Thread.Sleep(50);
                            }
                            if (!isSuccess)
                            {
                                msg += msgTemp;
                                isSuccess = false;
                                HttpRequestUtil.PostUrl("AddBreakdownAlarm", "msg=" + msg); //故障报警
                            }
                            else
                            {
                                //更新商品出货明细
                                HttpRequestUtil.PostUrl("AddOrderSendDetail", "orderId=" + orderId + "&productId=" + road["productId"] + "&boxId=" + boxId + "&roadId=" + road["roadId"]);
                                //更新每盒跟踪
                                HttpRequestUtil.PostUrl("UpdateRoadProduct", "roadId=" + road["roadId"]);
                                //更新商品批次仓库关联表
                                HttpRequestUtil.PostUrl("UpdateProductBatchesStore", "roadId=" + road["roadId"]);
                                //更新商品销售数量表
                                HttpRequestUtil.PostUrl("UpdateProductSaleNum", "productId=" + road["productId"]);
                            }
                        }
                        else
                        {
                            //更新硬纸币器硬纸币数量
                            int type;
                            int amountValue = 0;
                            int sumAmount = 0;
                            while ((amountValue = machine.Machine.QueryAmount(out type)) > 0)
                            {
                                sumAmount += amountValue;
                                HttpRequestUtil.PostUrl("PlusCoinAndPaperCount", "amountValue=" + amountValue + "&type=" + type);
                            }
                            //纸硬币器禁能
                            string msgTemp = null;
                            machine.Machine.CoinDisable(out msgTemp);
                            machine.Machine.PaperMoneyDisable(out msgTemp);
                            //投币支付表和投币找零记录表
                            HttpRequestUtil.PostUrl("AddCoinChangeRecords", "orderId=" + orderId + "&amount=" + sumAmount);

                            HttpRequestUtil.PostUrl("AddBreakdownAlarm", "msg=" + msg); //故障报警

                            msg = "出货失败：" + msg;
                            return false;
                        }
                    }

                    //更新货道现有商品数量
                    HttpRequestUtil.PostUrl("UpdateCoinAndPaperCount", "roadId=" + road["roadId"] + "&sellNum=" + road["productNum"]);
                    //商品出货记录表
                    HttpRequestUtil.PostUrl("AddDeliveryRecord", "orderId=" + orderId + "&productId=" + road["productId"] + "&boxId=" + boxId + "&shipmentNum=" + road["productNum"]);
                }

                //更新硬纸币器硬纸币数量
                int type2;
                int amountValue2 = 0;
                int sumAmount2 = 0;
                while ((amountValue2 = machine.Machine.QueryAmount(out type2)) > 0)
                {
                    sumAmount2 += amountValue2;
                    HttpRequestUtil.PostUrl("PlusCoinAndPaperCount", "amountValue=" + amountValue2 + "&type=" + type2);
                }
                //纸硬币器禁能
                string msgTemp2 = null;
                machine.Machine.CoinDisable(out msgTemp2);
                machine.Machine.PaperMoneyDisable(out msgTemp2);
                //投币支付表和投币找零记录表
                HttpRequestUtil.PostUrl("AddCoinChangeRecords", "orderId=" + orderId + "&amount=" + sumAmount2);
                //更新订单状态
                HttpRequestUtil.PostUrl("UpdateOrderStatus", "orderId=" + orderId);
                //交易记录表
                HttpRequestUtil.PostUrl("AddPaymentInfo", "orderId=" + orderId + "&payPrice=" + ((sumAmount2 - remainder) / 100.0).ToString());

                msg = "出货完成";

                return true;
            }
            #endregion

            #region 骏鹏
            if (machine.Type == MachineType.骏鹏)
            {
                int remainder = 0;

                foreach (Dictionary<string, string> road in roadList)
                {
                    for (int i = 0; i < int.Parse(road["productNum"]); i++)
                    {
                        int boxNo = int.Parse(HttpRequestUtil.PostUrl("GetBoxNo", "boxId=" + boxId));
                        cost = (int)(double.Parse(road["productPrice"]) * 100);
                        byte HDID = byte.Parse(HttpRequestUtil.PostUrl("GetHDID", "boxId=" + boxId + "&layerNo=" + road["layerNo"] + "&roadNo=" + road["roadNo"]));
                        VendoutRpt vendoutRpt = machine.MachineJP.VENDOUT_IND((byte)boxNo, 2, HDID, 0, cost);
                        if (vendoutRpt.status == 0)
                        {
                            //更新商品出货明细
                            HttpRequestUtil.PostUrl("AddOrderSendDetail", "orderId=" + orderId + "&productId=" + road["productId"] + "&boxId=" + boxId + "&roadId=" + road["roadId"]);
                            //更新每盒跟踪
                            HttpRequestUtil.PostUrl("UpdateRoadProduct", "roadId=" + road["roadId"]);
                            //更新商品批次仓库关联表
                            HttpRequestUtil.PostUrl("UpdateProductBatchesStore", "roadId=" + road["roadId"]);
                            //更新商品销售数量表
                            HttpRequestUtil.PostUrl("UpdateProductSaleNum", "productId=" + road["productId"]);
                        }
                        else
                        {
                            //更新硬纸币器硬纸币数量
                            int sumAmount = 0;
                            PayinRpt payinRpt = null;
                            while ((payinRpt = machine.MachineJP.GetPayinRpt()) != null)
                            {
                                sumAmount += payinRpt.value;
                                HttpRequestUtil.PostUrl("PlusCoinAndPaperCount", "amountValue=" + payinRpt.value + "&type=" + (int)payinRpt.dt);
                            }
                            //纸硬币器禁能
                            machine.MachineJP.CtrlCoinPaper(false);
                            //投币支付表和投币找零记录表
                            HttpRequestUtil.PostUrl("AddCoinChangeRecords", "orderId=" + orderId + "&amount=" + sumAmount);

                            msg = "出货失败";
                            HttpRequestUtil.PostUrl("AddBreakdownAlarm", "msg=" + msg); //故障报警

                            return false;
                        }
                    }

                    //更新货道现有商品数量
                    HttpRequestUtil.PostUrl("UpdateCoinAndPaperCount", "roadId=" + road["roadId"] + "&sellNum=" + road["productNum"]);
                    //商品出货记录表
                    HttpRequestUtil.PostUrl("AddDeliveryRecord", "orderId=" + orderId + "&productId=" + road["productId"] + "&boxId=" + boxId + "&shipmentNum=" + road["productNum"]);
                }

                //更新硬纸币器硬纸币数量
                int sumAmount2 = 0;
                PayinRpt payinRpt2 = null;
                while ((payinRpt2 = machine.MachineJP.GetPayinRpt()) != null)
                {
                    sumAmount2 += payinRpt2.value;
                    HttpRequestUtil.PostUrl("PlusCoinAndPaperCount", "amountValue=" + payinRpt2.value + "&type=" + (int)payinRpt2.dt);
                }
                //纸硬币器禁能
                machine.MachineJP.CtrlCoinPaper(false);
                //投币支付表和投币找零记录表
                HttpRequestUtil.PostUrl("AddCoinChangeRecords", "orderId=" + orderId + "&amount=" + sumAmount2);
                //更新订单状态
                HttpRequestUtil.PostUrl("UpdateOrderStatus", "orderId=" + orderId);
                //交易记录表
                HttpRequestUtil.PostUrl("AddPaymentInfo", "orderId=" + orderId + "&payPrice=" + ((sumAmount2 - remainder) / 100.0).ToString());

                msg = "出货完成";

                return true;
            }
            #endregion

            msg = "";
            return false;
        }
        #endregion

        #region 直接取货
        /// <summary>
        /// 直接取货
        /// </summary>
        /// <param name="checkDrop">是否掉货检测</param>
        /// <param name="orderId">订单ID</param>
        public static bool TakeGood(bool checkDrop, string orderId)
        {
            string boxId = HttpRequestUtil.PostUrl("GetBoxId", "orderId=" + orderId);
            string com = HttpRequestUtil.PostUrl("GetCom", "boxId=" + boxId);
            MachineModel machine = GetMachine(com);
            List<Dictionary<string, string>> roadList = JsonHelper.JsonToListDic(HttpRequestUtil.PostUrl("GetRoad", "orderId=" + orderId));

            #region 金码
            if (machine.Type == MachineType.金码)
            {
                int remainder = 0;
                bool isSuccess = true;
                string msg;

                foreach (Dictionary<string, string> road in roadList)
                {
                    for (int i = 0; i < int.Parse(road["productNum"]); i++)
                    {
                        int boxNo = int.Parse(HttpRequestUtil.PostUrl("GetBoxNo", "boxId=" + boxId));
                        if (machine.Machine.Connect(out msg) && machine.Machine.Shipment(boxNo, int.Parse(road["layerNo"]), int.Parse(road["roadNo"]), false, 0, checkDrop, out msg))
                        {
                            bool isSucessTemp = false;
                            string msgTemp = null;
                            while (!machine.Machine.QueryShipment(out isSucessTemp, out remainder, false, out msgTemp))
                            {
                                Thread.Sleep(50);
                            }
                            if (!isSuccess)
                            {
                                msg += msgTemp;
                                isSuccess = false;
                                HttpRequestUtil.PostUrl("AddBreakdownAlarm", "msg=" + msg); //故障报警
                            }
                            else
                            {
                                //更新商品出货明细
                                HttpRequestUtil.PostUrl("AddOrderSendDetail", "orderId=" + orderId + "&productId=" + road["productId"] + "&boxId=" + boxId + "&roadId=" + road["roadId"]);
                                //更新每盒跟踪
                                HttpRequestUtil.PostUrl("UpdateRoadProduct", "roadId=" + road["roadId"]);
                                //更新商品批次仓库关联表
                                HttpRequestUtil.PostUrl("UpdateProductBatchesStore", "roadId=" + road["roadId"]);
                                //更新商品销售数量表
                                HttpRequestUtil.PostUrl("UpdateProductSaleNum", "productId=" + road["productId"]);
                            }
                        }
                        else
                        {
                            HttpRequestUtil.PostUrl("AddBreakdownAlarm", "msg=" + msg); //故障报警

                            return false;
                        }
                    }

                    //更新货道现有商品数量
                    HttpRequestUtil.PostUrl("UpdateCoinAndPaperCount", "roadId=" + road["roadId"] + "&sellNum=" + road["productNum"]);
                    //商品出货记录表
                    HttpRequestUtil.PostUrl("AddDeliveryRecord", "orderId=" + orderId + "&productId=" + road["productId"] + "&boxId=" + boxId + "&shipmentNum=" + road["productNum"]);
                }

                //更新订单状态
                HttpRequestUtil.PostUrl("UpdateOrderStatus", "orderId=" + orderId);

                return true;
            }
            #endregion

            #region 骏鹏
            if (machine.Type == MachineType.骏鹏)
            {
                int remainder = 0;
                bool isSuccess = true;
                string msg;

                foreach (Dictionary<string, string> road in roadList)
                {
                    for (int i = 0; i < int.Parse(road["productNum"]); i++)
                    {
                        int boxNo = int.Parse(HttpRequestUtil.PostUrl("GetBoxNo", "boxId=" + boxId));
                        byte HDID = byte.Parse(HttpRequestUtil.PostUrl("GetHDID", "boxId=" + boxId + "&layerNo=" + road["layerNo"] + "&roadNo=" + road["roadNo"]));
                        VendoutRpt vendoutRpt = machine.MachineJP.VENDOUT_IND((byte)boxNo, 2, HDID, 1, 0);
                        if (vendoutRpt.status == 0)
                        {
                            //更新商品出货明细
                            HttpRequestUtil.PostUrl("AddOrderSendDetail", "orderId=" + orderId + "&productId=" + road["productId"] + "&boxId=" + boxId + "&roadId=" + road["roadId"]);
                            //更新每盒跟踪
                            HttpRequestUtil.PostUrl("UpdateRoadProduct", "roadId=" + road["roadId"]);
                            //更新商品批次仓库关联表
                            HttpRequestUtil.PostUrl("UpdateProductBatchesStore", "roadId=" + road["roadId"]);
                            //更新商品销售数量表
                            HttpRequestUtil.PostUrl("UpdateProductSaleNum", "productId=" + road["productId"]);
                        }
                        else
                        {
                            HttpRequestUtil.PostUrl("AddBreakdownAlarm", "msg=出货失败"); //故障报警

                            return false;
                        }
                    }

                    //更新货道现有商品数量
                    HttpRequestUtil.PostUrl("UpdateCoinAndPaperCount", "roadId=" + road["roadId"] + "&sellNum=" + road["productNum"]);
                    //商品出货记录表
                    HttpRequestUtil.PostUrl("AddDeliveryRecord", "orderId=" + orderId + "&productId=" + road["productId"] + "&boxId=" + boxId + "&shipmentNum=" + road["productNum"]);
                }

                //更新订单状态
                HttpRequestUtil.PostUrl("UpdateOrderStatus", "orderId=" + orderId);

                return true;
            }
            #endregion

            return false;
        }
        #endregion

        #region 微信取货
        /// <summary>
        /// 微信取货
        /// </summary>
        /// <param name="path">网站bin目录</param>
        /// <param name="checkDrop">是否掉货检测</param>
        /// <param name="jsonData">数据</param>
        public bool WXTakeGood(string path, bool checkDrop, string jsonData)
        {
            JObject jObject = JObject.Parse(jsonData);

            foreach (JToken jToken in jObject["pickup"])
            {
                string orderId = jToken["orderId"].ToString();
                string productId = jToken["productId"].ToString();
                string productNum = jToken["productNum"].ToString();
                string productPrice = jToken["productPrice"].ToString();
                string orderType = jToken["orderType"].ToString();
                string businessesId = jToken["businessesId"].ToString();
                string orderNo = jToken["orderNo"].ToString();

                List<Dictionary<string, string>> roadList = JsonHelper.JsonToListDic(HttpRequestUtil.PostUrl("GetRoadByProductId", "productId=" + productId));
                string boxId = HttpRequestUtil.PostUrl("GetBoxId", "orderId=" + orderId);
                string com = HttpRequestUtil.PostUrl("GetCom", "boxId=" + boxId);
                MachineModel machine = GetMachine(com);

                #region 金码
                int remainder = 0;
                bool isSuccess = true;
                string msg = "";
                if (machine.Type == MachineType.金码)
                {
                    foreach (Dictionary<string, string> road in roadList)
                    {
                        for (int i = 0; i < int.Parse(productNum); i++)
                        {
                            int boxNo = int.Parse(HttpRequestUtil.PostUrl("GetBoxNo", "boxId=" + boxId));
                            if (machine.Machine.Connect(out msg) && machine.Machine.Shipment(boxNo, int.Parse(road["layerNo"]), int.Parse(road["roadNo"]), false, 0, checkDrop, out msg))
                            {
                                bool isSucessTemp = false;
                                string msgTemp = null;
                                while (!machine.Machine.QueryShipment(out isSucessTemp, out remainder, false, out msgTemp))
                                {
                                    Thread.Sleep(50);
                                }
                                if (!isSuccess)
                                {
                                    msg += msgTemp;
                                    isSuccess = false;
                                    HttpRequestUtil.PostUrl("AddBreakdownAlarm", "msg=" + msg); //故障报警
                                }
                                else
                                {
                                    //更新商品出货明细
                                    HttpRequestUtil.PostUrl("AddOrderSendDetail", "orderId=" + orderId + "&productId=" + road["productId"] + "&boxId=" + boxId + "&roadId=" + road["roadId"]);
                                    //更新每盒跟踪
                                    HttpRequestUtil.PostUrl("UpdateRoadProduct", "roadId=" + road["roadId"]);
                                    //更新商品批次仓库关联表
                                    HttpRequestUtil.PostUrl("UpdateProductBatchesStore", "roadId=" + road["roadId"]);
                                    //更新商品销售数量表
                                    HttpRequestUtil.PostUrl("UpdateProductSaleNum", "productId=" + road["productId"]);
                                }
                            }
                            else
                            {
                                HttpRequestUtil.PostUrl("AddBreakdownAlarm", "msg=" + msg); //故障报警

                                return false;
                            }
                        }

                        //更新货道现有商品数量
                        HttpRequestUtil.PostUrl("UpdateCoinAndPaperCount", "roadId=" + road["roadId"] + "&sellNum=" + road["productNum"]);
                        //商品出货记录表
                        HttpRequestUtil.PostUrl("AddDeliveryRecord", "orderId=" + orderId + "&productId=" + road["productId"] + "&boxId=" + boxId + "&shipmentNum=" + road["productNum"]);
                    }
                }
                #endregion

                #region 骏鹏
                if (machine.Type == MachineType.骏鹏)
                {
                    foreach (Dictionary<string, string> road in roadList)
                    {
                        for (int i = 0; i < int.Parse(productNum); i++)
                        {
                            int boxNo = int.Parse(HttpRequestUtil.PostUrl("GetBoxNo", "boxId=" + boxId));
                            byte HDID = byte.Parse(HttpRequestUtil.PostUrl("GetHDID", "boxId=" + boxId + "&layerNo=" + road["layerNo"] + "&roadNo=" + road["roadNo"]));
                            VendoutRpt vendoutRpt = machine.MachineJP.VENDOUT_IND((byte)boxNo, 2, HDID, 1, 0);
                            if (vendoutRpt.status == 0)
                            {
                                //更新商品出货明细
                                HttpRequestUtil.PostUrl("AddOrderSendDetail", "orderId=" + orderId + "&productId=" + road["productId"] + "&boxId=" + boxId + "&roadId=" + road["roadId"]);
                                //更新每盒跟踪
                                HttpRequestUtil.PostUrl("UpdateRoadProduct", "roadId=" + road["roadId"]);
                                //更新商品批次仓库关联表
                                HttpRequestUtil.PostUrl("UpdateProductBatchesStore", "roadId=" + road["roadId"]);
                                //更新商品销售数量表
                                HttpRequestUtil.PostUrl("UpdateProductSaleNum", "productId=" + road["productId"]);
                            }
                            else
                            {
                                HttpRequestUtil.PostUrl("AddBreakdownAlarm", "msg=出货失败"); //故障报警

                                return false;
                            }
                        }

                        //更新货道现有商品数量
                        HttpRequestUtil.PostUrl("UpdateCoinAndPaperCount", "roadId=" + road["roadId"] + "&sellNum=" + road["productNum"]);
                        //商品出货记录表
                        HttpRequestUtil.PostUrl("AddDeliveryRecord", "orderId=" + orderId + "&productId=" + road["productId"] + "&boxId=" + boxId + "&shipmentNum=" + road["productNum"]);
                    }
                }
                #endregion

            }

            return true;
        }
        #endregion

        #region 查询投币金额
        /// <summary>
        /// 查询投币金额
        /// </summary>
        public static int QueryAmount(string orderId)
        {
            string boxId = HttpRequestUtil.PostUrl("GetBoxId", "orderId=" + orderId);
            string com = HttpRequestUtil.PostUrl("GetCom", "boxId=" + boxId);
            MachineModel machine = GetMachine(com);

            #region 骏鹏
            if (machine.Type == MachineType.骏鹏)
            {
                PayinRpt infoRpt_3 = machine.MachineJP.GetPayinRpt();
                if (infoRpt_3 != null)
                {
                    return infoRpt_3.total_value;
                }
                else
                {
                    return 0;
                }
            }
            #endregion

            #region 金码
            if (machine.Type == MachineType.金码)
            {
                int type;
                return machine.Machine.QueryAmount(out type);
            }
            #endregion

            return 0;
        }
        #endregion

        #region 同步投币金额
        /// <summary>
        /// 同步投币金额
        /// </summary>
        public static int SyncAmount(string orderId)
        {
            string boxId = HttpRequestUtil.PostUrl("GetBoxId", "orderId=" + orderId);
            string com = HttpRequestUtil.PostUrl("GetCom", "boxId=" + boxId);
            MachineModel machine = GetMachine(com);

            #region 骏鹏
            if (machine.Type == MachineType.骏鹏)
            {
                InfoRpt_3 infoRpt_3 = machine.MachineJP.GetRemaiderAmount();
                if (infoRpt_3 != null)
                {
                    return infoRpt_3.total_value;
                }
                else
                {
                    return 0;
                }
            }
            #endregion

            #region 金码
            if (machine.Type == MachineType.金码)
            {
                return machine.Machine.SyncAmount();
            }
            #endregion

            return 0;
        }
        #endregion

        #region 退币
        /// <summary>
        /// 退币
        /// </summary>
        public static bool RefoundMoney(int amount)
        {
            string com = HttpRequestUtil.PostUrl("GetDefaultComName");
            MachineModel machine = GetMachine(com);

            #region 骏鹏
            if (machine.Type == MachineType.骏鹏)
            {
                PayoutRpt payoutRpt = machine.MachineJP.PAYOUT_IND(PayoutType.硬币出币, amount, 1);
                if (payoutRpt != null)
                {
                    return true;
                }
            }
            #endregion

            #region 金码
            if (machine.Type == MachineType.金码)
            {
                string msg = "";
                bool bl = machine.Machine.RefundMoney(amount, out msg);
                return bl;
            }
            #endregion

            return false;
        }
        #endregion

        #region 货机检测-主机
        /// <summary>
        /// 货机检测-主机
        /// </summary>
        public static string MachineCheck_Main()
        {
            string com = HttpRequestUtil.PostUrl("GetDefaultComName");
            MachineModel machine = GetMachine(com);

            #region 骏鹏
            if (machine.Type == MachineType.骏鹏)
            {
                //VMC状态
                StatusRpt statusRpt = machine.MachineJP.GET_STATUS();
                //硬币器
                InfoRpt_17 infoRpt_17 = machine.MachineJP.GetCoinInfo();
                //纸币器
                InfoRpt_16 infoRpt_16 = machine.MachineJP.GetPaperInfo();

                return string.Format("VMC状态：{0}\r\n硬币器信息：{1}\r\n纸币器信息：{2}\r\n",
                    statusRpt.ToString(), infoRpt_17.ToString(), infoRpt_16.ToString());
            }
            #endregion

            #region 金码
            if (machine.Type == MachineType.金码)
            {
                //主板
                StatusInfoCollection statusInfoCollection = machine.Machine.QueryMainBoardInfo();
                return statusInfoCollection.ToString();
            }
            #endregion

            return "";
        }
        #endregion

        #region 货机检测-柜子
        /// <summary>
        /// 货机检测-柜子
        /// </summary>
        public static string MachineCheck_Box()
        {
            StringBuilder sb = new StringBuilder();
            string json = HttpRequestUtil.PostUrl("GetAllCom");
            string boxJson = HttpRequestUtil.PostUrl("GetAllBox");
            List<Dictionary<string, string>> list = JsonHelper.JsonToListDic(json);
            List<Dictionary<string, string>> boxList = JsonHelper.JsonToListDic(boxJson);

            foreach (Dictionary<string, string> box in boxList)
            {
                Dictionary<string, string> boxType = boxList.Find(item => item["boxTypeId"] == box["id"]);
                MachineModel machine = GetMachine(boxType["comName"]);

                #region 金码
                if (machine.Type == MachineType.金码)
                {
                    StatusInfoCollection statusInfoCollection = machine.Machine.QueryEquipmentsStatus(int.Parse(box["boxNo"]));
                    sb.AppendFormat("{0}\r\n", statusInfoCollection.ToString());
                    StatusInfoCollection boxStatus = machine.Machine.QueryBoxStatus(int.Parse(box["boxNo"]));
                    sb.AppendFormat("{0}\r\n", boxStatus.ToString());
                    string roadJson = HttpRequestUtil.PostUrl("GetRoadListByBoxId", "boxId=" + box["id"].ToString());
                    List<Dictionary<string, string>> roadList = JsonHelper.JsonToListDic(roadJson);
                    List<string> roadNoList = new List<string>();
                    foreach (Dictionary<string, string> road in roadList)
                    {
                        int floor = int.Parse(road["layerNo"].ToString());
                        int num = int.Parse(road["sort"].ToString());
                        string roadNo = MachineDll.Utils.CommonUtil.NumToABCD(floor) + num.ToString();
                        roadNoList.Add(roadNo);
                    }
                    List<StatusInfoCollection> roadStatus = machine.Machine.QueryRoadInfo(int.Parse(box["boxNo"]), roadNoList);
                    sb.AppendFormat("货道信息：\r\n");
                    foreach (StatusInfoCollection road in roadStatus)
                    {
                        sb.AppendFormat("{0}\r\n", road.ToString());
                    }
                }
                #endregion

                #region 骏鹏
                if (machine.Type == MachineType.骏鹏)
                {
                    HuoDaoRpt huoDaoRpt = machine.MachineJP.GET_HUODAO(byte.Parse(box["boxNo"]));
                    sb.AppendFormat("货道信息：\r\n{0}\r\n", huoDaoRpt.ToString());
                }
                #endregion

            }

            return sb.ToString();
        }
        #endregion

    }
}
