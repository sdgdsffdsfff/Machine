using IMachineDll.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMachineDll
{
    /// <summary>
    /// 售货机接口
    /// </summary>
    public interface IMachine
    {
        /// <summary>
        /// 联机
        /// </summary>
        /// <param name="msg">传出错误信息</param>
        /// <returns>联机是否成功</returns>
        bool Connect(out string msg);
        /// <summary>
        /// 出货
        /// </summary>
        /// <param name="boxNo">机柜编号，1、2、3…</param>
        /// <param name="floor">货道层</param>
        /// <param name="num">货道号</param>
        /// <param name="cashPay">是否现金支付，true现金支付，false非现金支付</param>
        /// <param name="cost">货道价格(单位：分)</param>
        /// <param name="checkDrop">是否检测掉货</param>
        /// <param name="msg">传出错误信息</param>
        /// <returns>是否正常出货</returns>
        bool Shipment(int boxNo, int floor, int num, bool cashPay, int cost, bool checkDrop, out string msg);
        /// <summary>
        /// 硬币器使能
        /// </summary>
        /// <param name="msg">传出错误信息</param>
        /// <returns>硬币器使能成功或失败</returns>
        bool CoinEnable(out string msg);
        /// <summary>
        /// 硬币器禁能
        /// </summary>
        /// <returns>硬币器禁能成功或失败</returns>
        bool CoinDisable(out string msg);
        /// <summary>
        /// 纸币器使能
        /// </summary>
        /// <param name="msg">传出错误信息</param>
        /// <returns>纸币器使能成功或失败</returns>
        bool PaperMoneyEnable(out string msg);
        /// <summary>
        /// 纸币器禁能
        /// </summary>
        /// <returns>纸币器禁能成功或失败</returns>
        bool PaperMoneyDisable(out string msg);
        /// <summary>
        /// 查询金额(单位：分)
        /// </summary>
        /// <param name="type">货币类型(0：硬币，1：纸币)</param>
        /// <returns>金额数(单位：分)</returns>
        int QueryAmount(out int type);
        /// <summary>
        /// 同步金额(单位：分)
        /// </summary>
        /// <returns>金额数</returns>
        int SyncAmount();
        /// <summary>
        /// 清除金额(单位：分)
        /// </summary>
        /// <returns>控制主板上现在的可用金额（完成清除后返回0）</returns>
        int ClearAmount();
        /// <summary>
        /// 查询出货结果
        /// </summary>
        /// <param name="isSuccess">出货是否成功</param>
        /// <param name="remainder">余额(单位:分)</param>
        /// <param name="checkDrop">是否检测掉货</param>
        /// <param name="msg">传出错误信息</param>
        /// <returns>出货是否已完成</returns>
        bool QueryShipment(out bool isSuccess, out int remainder, bool checkDrop, out string msg);
        /// <summary>
        /// 查询可找硬币金额(单位：分)，返回-1表示查询失败
        /// </summary>
        /// <param name="msg">传出错误信息</param>
        /// <returns>金额(单位：分)</returns>
        int QueryGiveChange(out string msg);
        /// <summary>
        /// 退币
        /// </summary>
        /// <param name="amount">要退出的金额(单位：分)</param>
        /// <param name="msg">传出错误信息</param>
        /// <returns>是否正常退币</returns>
        bool RefundMoney(int amount, out string msg);
        /// <summary>
        /// 查询退币结果
        /// </summary>
        /// <param name="isSuccess">退币是否成功</param>
        /// <param name="remainder">余额(单位:分)</param>
        /// <param name="msg">传出错误信息</param>
        /// <returns>退币是否已完成</returns>
        bool QueryRefundMoney(out bool isSuccess, out int remainder, out string msg);
        /// <summary>
        /// 查询控制主板信息
        /// </summary>
        StatusInfoCollection QueryMainBoardInfo();
        /// <summary>
        /// 设置控制主板时间
        /// </summary>
        /// <param name="msg">传出错误信息</param>
        /// <returns>是否设置成功</returns>
        bool SetMainBoardTime(out string msg);
        /// <summary>
        /// 查询货道信息
        /// </summary>
        /// <param name="boxNo">机柜编号</param>
        /// <param name="floor">货道层</param>
        /// <param name="num">货道号</param>
        /// <param name="cost">货道价格(单位：分)</param>
        /// <param name="status">货道状态</param>
        /// <returns>是否正常</returns>
        bool QueryRoadInfo(int boxNo, int floor, int num, out int cost, out string status);
        /// <summary>
        /// 查询货道信息
        /// </summary>
        /// <param name="boxNo">机柜编号</param>
        /// <param name="floor">货道层</param>
        /// <param name="num">货道号</param>
        /// <returns>货道状态</returns>
        StatusInfoCollection QueryRoadInfo(int boxNo, int floor, int num);
        /// <summary>
        /// 查询货道信息
        /// </summary>
        /// <param name="boxNo">机柜编号</param>
        /// <param name="roadNoList">货道编号集合，货道编号例：B12</param>
        /// <returns>货道集合的状态集合</returns>
        List<StatusInfoCollection> QueryRoadInfo(int boxNo, List<string> roadNoList);
        /// <summary>
        /// 货道电机测试
        /// </summary>
        /// <param name="boxNo">机柜编号，1、2、3…</param>
        /// <param name="floor">货道层</param>
        /// <param name="num">货道号</param>
        /// <param name="msg">传出错误信息</param>
        /// <returns>是否正常</returns>
        bool TestRoad(int boxNo, int floor, int num, out string msg);
        /// <summary>
        /// 查询货道电机测试
        /// </summary>
        /// <param name="isSuccess">测试是否成功</param>
        /// <param name="msg">传出错误信息</param>
        /// <returns>测试是否已完成</returns>
        bool QueryTestRoad(out bool isSuccess, out string msg);
        /// <summary>
        /// 查询机器设备状态
        /// </summary>
        /// <param name="boxNo">机柜编号</param>
        /// <returns>机器状态集合</returns>
        StatusInfoCollection QueryBoxStatus(int boxNo);
        /// <summary>
        /// 查询制冷压缩机/风机/照明/除雾/广告灯/工控机等设备状态
        /// </summary>
        /// <param name="boxNo">机柜编号</param>
        /// <returns>设备状态集合</returns>
        StatusInfoCollection QueryEquipmentsStatus(int boxNo);
        /// <summary>
        /// 设置制冷压缩机/照明/除雾/广告灯/工控机等设备控制策略参数
        /// </summary>
        /// <param name="boxNo">机柜编号</param>
        /// <param name="equipmentType">设备类型(0：制冷压缩机， 2：照明设备， 3：除雾设备， 4：广告灯， 5：工控机/显示器/机箱风扇， 6：预留设备1， 7：预留设备2)</param>
        /// <param name="temperature">目标温度(3—15℃)</param>
        /// <param name="controlMode">控制模式(0：定时开启，1：全时段开启，2：全时段关闭，如果设备类型为5（工控机/显示器/机箱风扇），控制模式不能为2（全时段关闭）)</param>
        /// <param name="periodOfTime1">定时时间段1(格式HHmm)</param>
        /// <param name="periodOfTime2">定时时间段2(格式HHmm)</param>
        /// <param name="msg">传出错误信息</param>
        /// <returns>是否设置成功</returns>
        bool SetEquipments(int boxNo, int equipmentType, int temperature, int controlMode, string periodOfTime1, string periodOfTime2, out string msg);
        /// <summary>
        /// 查询制冷压缩机/照明/除雾/广告灯/工控机等设备控制策略参数
        /// </summary>
        /// <param name="boxNo">机柜编号</param>
        /// <param name="equipmentType">设备类型(0：制冷压缩机， 2：照明设备， 3：除雾设备， 4：广告灯， 5：工控机/显示器/机箱风扇， 6：预留设备1， 7：预留设备2)</param>
        /// <returns>策略参数集合</returns>
        StatusInfoCollection QueryEquipment(int boxNo, int equipmentType);
        /// <summary>
        /// 查询制冷压缩机/照明/除雾/广告灯/工控机等设备控制策略参数
        /// </summary>
        /// <param name="boxNo">机柜编号</param>
        /// <returns>策略参数集合</returns>
        List<StatusInfoCollection> QueryEquipmentAll(int boxNo);
    }
}
