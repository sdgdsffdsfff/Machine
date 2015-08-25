using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace CommonDll
{
    /// <summary>
    /// 售货机配置工具类
    /// </summary>
    public class MachineConfigUtil
    {
        #region 根据货柜号获取串口号
        /// <summary>
        /// 根据货柜号获取串口号
        /// </summary>
        /// <param name="box">货柜号</param>
        public static string GetComByBox(int box)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load("MachineConfig.xml");
            XmlNode machineNode = xmlDoc.SelectSingleNode("machine");
            if (machineNode.Attributes["boxno"].Value == box.ToString())
            {
                return machineNode.Attributes["com"].Value;
            }

            for (int i = 0; i < machineNode.ChildNodes.Count; i++)
            {
                XmlNode boxNode = machineNode.ChildNodes[i];
                if (boxNode.Attributes["boxno"].Value == box.ToString())
                {
                    return machineNode.Attributes["com"].Value;
                }
            }

            FileLogger.LogError("找不到货柜号" + box.ToString() + "对应的串口号");
            throw new Exception("找不到货柜号" + box.ToString() + "对应的串口号");
        }
        #endregion

    }
}
