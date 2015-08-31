using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using CommonDll;

namespace MachineJPAdapterDll.Utils
{
    /// <summary>
    /// 骏鹏货柜配置工具类
    /// </summary>
    public class JPBoxConfigUtil
    {
        #region 根据串口号获取每层最大货道数
        /// <summary>
        /// 根据串口号获取每层最大货道数
        /// </summary>
        public static int GetColcount(string com)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load("MachineConfig.xml");
            XmlNode machineNode = xmlDoc.SelectSingleNode("machine");
            if (machineNode.Attributes["com"].Value == com)
            {
                if (machineNode.Attributes["colcount"] != null)
                {
                    return int.Parse(machineNode.Attributes["colcount"].Value);
                }
                else
                {
                    FileLogger.LogError("获取colcount失败，请检查MachineConfig配置");
                    throw new Exception("获取colcount失败，请检查MachineConfig配置");
                }
            }
            for (int i = 0; i < machineNode.ChildNodes.Count; i++)
            {
                XmlNode boxNode = machineNode.ChildNodes[i];
                if (boxNode.Attributes["com"].Value == com)
                {
                    return int.Parse(boxNode.Attributes["colcount"].Value);
                }
            }
            FileLogger.LogError("获取colcount失败，请检查MachineConfig配置");
            throw new Exception("获取colcount失败，请检查MachineConfig配置");
        }
        #endregion

    }
}
