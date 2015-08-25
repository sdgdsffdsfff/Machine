using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using CommonDll;
using IMachineDll;
using IMachineDll.Models;
using MachineFactoryDll.Enums;
using MachineFactoryDll.Models;
using MachineJMAdapterDll;
using MachineJPAdapterDll;

namespace MachineFactoryDll
{
    /// <summary>
    /// 售货机接口工厂
    /// 2015年08月24日
    /// </summary>
    public class MachineFactory
    {
        #region 变量
        /// <summary>
        /// 售货机接口列表
        /// </summary>
        public static List<MachineModel> MachineList { get; set; }
        /// <summary>
        /// 售货机主机接口
        /// </summary>
        public static IMachine Machine { get; set; }
        #endregion

        #region 获取接口
        public static IMachine GetMachine(string com)
        {
            MachineModel machine = MachineList.Find(item => item.Com == com);
            if (machine == null)
            {
                FileLogger.LogError("根据串口号获取接口失败，找不到该串口" + com + "号对应的接口");
                throw new Exception("根据串口号获取接口失败，找不到该串口" + com + "号对应的接口");
            }
            return machine.Machine;
        }
        #endregion

        #region 初始化
        public static void Init()
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load("MachineConfig.xml");
            XmlNode machineNode = xmlDoc.SelectSingleNode("machine");
            bool initResult = true;
            OperateResult operateResult;
            MachineList = new List<MachineModel>();

            //主机接口
            MachineModel mainMachine = null;
            switch (machineNode.Attributes["type"].Value)
            {
                case "MachineJM":
                    Machine = new MachineJMAdapter(machineNode.Attributes["com"].Value);
                    mainMachine = new MachineModel(machineNode.Attributes["com"].Value, MachineType.金码, Machine);
                    break;
                case "MachineJP":
                    Machine = new MachineJPAdapter(machineNode.Attributes["com"].Value);
                    mainMachine = new MachineModel(machineNode.Attributes["com"].Value, MachineType.骏鹏, Machine);
                    break;
            }
            MachineList.Add(mainMachine);
            operateResult = Machine.Connect();
            if (!operateResult.Success) initResult = false;

            //辅机接口
            for (int i = 0; i < machineNode.ChildNodes.Count; i++)
            {
                XmlNode boxNode = machineNode.ChildNodes[i];

                MachineModel machineModel = null;

                switch (boxNode.Attributes["type"].Value)
                {
                    case "MachineJM":
                        machineModel = new MachineModel(boxNode.Attributes["com"].Value, MachineType.金码);
                        break;
                    case "MachineJP":
                        machineModel = new MachineModel(boxNode.Attributes["com"].Value, MachineType.骏鹏);
                        break;
                }
                operateResult = machineModel.Machine.Connect();
                if (!operateResult.Success) initResult = false;

                MachineList.Add(machineModel);
            }

            if (initResult)
            {
                FileLogger.Log("售货机接口工厂初始化成功，没有发生错误");
            }
        }
        #endregion

    }
}
