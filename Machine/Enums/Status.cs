using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineDll.Enums
{
    #region 纸币器状态
    /// <summary>
    /// 纸币器状态
    /// </summary>
    public enum PaperMoneyStatus
    {
        /// <summary>
        /// 故障
        /// </summary>
        [Description("故障")]
        Status_01 = 0x01,
        /// <summary>
        /// 正常
        /// </summary>
        [Description("正常")]
        Status_02 = 0x02,
        /// <summary>
        /// 电机故障
        /// </summary>
        [Description("电机故障")]
        Status_03 = 0x03,
        /// <summary>
        /// 感应器故障
        /// </summary>
        [Description("感应器故障")]
        Status_04 = 0x04,
        /// <summary>
        /// 纸币通道卡塞
        /// </summary>
        [Description("纸币通道卡塞")]
        Status_05 = 0x05,
        /// <summary>
        /// ROM校验和错误
        /// </summary>
        [Description("ROM校验和错误")]
        Status_06 = 0x06,
        /// <summary>
        /// 纸币卡塞在接收通道
        /// </summary>
        [Description("纸币卡塞在接收通道")]
        Status_07 = 0x07,
        /// <summary>
        /// 一个纸币非正常移除
        /// </summary>
        [Description("一个纸币非正常移除")]
        Status_09 = 0x09,
        /// <summary>
        /// 钞箱被拿走
        /// </summary>
        [Description("钞箱被拿走")]
        Status_0A = 0x0A,
        /// <summary>
        /// 钞箱已满
        /// </summary>
        [Description("钞箱已满")]
        Status_FE = 0xFE,
        /// <summary>
        /// 和纸币器断开连接
        /// </summary>
        [Description("和纸币器断开连接")]
        Status_FF = 0xFF
    }
    #endregion

    #region 硬币器状态
    /// <summary>
    /// 硬币器状态
    /// </summary>
    public enum CoinStatus
    {
        /// <summary>
        /// 故障
        /// </summary>
        [Description("故障")]
        Status_01 = 0x01,
        /// <summary>
        /// 正常
        /// </summary>
        [Description("正常")]
        Status_02 = 0x02,
        /// <summary>
        /// 退币按钮（指售货机设备上的硬件退币按钮）被触发
        /// </summary>
        [Description("退币按钮（指售货机设备上的硬件退币按钮）被触发")]
        Status_03 = 0x03,
        /// <summary>
        /// 识别硬币面值失败
        /// </summary>
        [Description("识别硬币面值失败")]
        Status_05 = 0x05,
        /// <summary>
        /// 检测到币桶感应器异常
        /// </summary>
        [Description("检测到币桶感应器异常")]
        Status_06 = 0x06,
        /// <summary>
        /// 两个硬币一起被接受
        /// </summary>
        [Description("两个硬币一起被接受")]
        Status_07 = 0x07,
        /// <summary>
        /// 找不到硬币识别头
        /// </summary>
        [Description("找不到硬币识别头")]
        Status_08 = 0x08,
        /// <summary>
        /// 一个储币管卡塞
        /// </summary>
        [Description("一个储币管卡塞")]
        Status_09 = 0x09,
        /// <summary>
        /// ROM校验和错误
        /// </summary>
        [Description("ROM校验和错误")]
        Status_0A = 0x0A,
        /// <summary>
        /// 硬币接收路径错误
        /// </summary>
        [Description("硬币接收路径错误")]
        Status_0B = 0x0B,
        /// <summary>
        /// 接收通道有硬币卡塞
        /// </summary>
        [Description("接收通道有硬币卡塞")]
        Status_0E = 0x0E,
        /// <summary>
        /// 硬币盒已满
        /// </summary>
        [Description("硬币盒已满")]
        Status_FA = 0xFA,
        /// <summary>
        /// 硬币盒被取走
        /// </summary>
        [Description("硬币盒被取走")]
        Status_FB = 0xFB,
        /// <summary>
        /// 可找零
        /// </summary>
        [Description("可找零")]
        Status_FC = 0xFC,
        /// <summary>
        /// 零钱不足
        /// </summary>
        [Description("零钱不足")]
        Status_FD = 0xFD,
        /// <summary>
        /// 和硬币器断开连接
        /// </summary>
        [Description("和硬币器断开连接")]
        Status_FF = 0xFF
    }
    #endregion

    #region 货道状态
    /// <summary>
    /// 货道状态
    /// </summary>
    public enum RoadStatus
    {
        /// <summary>
        /// 货柜不存在
        /// </summary>
        [Description("货柜不存在")]
        Status_00 = 0x00,
        /// <summary>
        /// 故障
        /// </summary>
        [Description("故障")]
        Status_01 = 0x01,
        /// <summary>
        /// 正常
        /// </summary>
        [Description("正常")]
        Status_02 = 0x02,
        /// <summary>
        /// 线路不通
        /// </summary>
        [Description("线路不通")]
        Status_03 = 0x03,
        /// <summary>
        /// 未安装电机（电机转接板未安装）
        /// </summary>
        [Description("未安装电机（电机转接板未安装）")]
        Status_04 = 0x04,
        /// <summary>
        /// 电机在4秒时限内不能压下微动开关到达相应位置
        /// </summary>
        [Description("电机在4秒时限内不能压下微动开关到达相应位置")]
        Status_05 = 0x05,
        /// <summary>
        /// 电机在4秒时限内不能归位到达相应位置，无掉货检测时可扣费
        /// </summary>
        [Description("电机在4秒时限内不能归位到达相应位置，无掉货检测时可扣费")]
        Status_06 = 0x06,
        /// <summary>
        /// 驱动IC出错
        /// </summary>
        [Description("驱动IC出错")]
        Status_07 = 0x07,
        /// <summary>
        /// 电机不在正确位置（电机不在位）
        /// </summary>
        [Description("电机不在正确位置（电机不在位）")]
        Status_08 = 0x08,
        /// <summary>
        /// 电机卡塞
        /// </summary>
        [Description("电机卡塞")]
        Status_09 = 0x09,
        /// <summary>
        /// 电机电流超限（电机过流）
        /// </summary>
        [Description("电机电流超限（电机过流）")]
        Status_0E = 0x0E
    }
    #endregion

    #region 驱动版状态
    /// <summary>
    /// 驱动版状态
    /// </summary>
    public enum BoardStatus
    {
        /// <summary>
        /// 故障(驱动板没有连接)
        /// </summary>
        [Description("故障(驱动板没有连接)")]
        Status_01 = 0x01,
        /// <summary>
        /// 正常
        /// </summary>
        [Description("正常")]
        Status_02 = 0x02
    }
    #endregion

}
