using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMachineDll;
using CommonDll.Enums;
using MachineJMAdapterDll;
using MachineJPAdapterDll;

namespace MachineFactoryDll.Models
{
    /// <summary>
    /// 售货机接口
    /// </summary>
    public class MachineModel
    {
        /// <summary>
        /// 串口号
        /// </summary>
        public string Com { get; set; }
        /// <summary>
        /// 售货机类型
        /// </summary>
        public MachineType Type { get; set; }
        /// <summary>
        /// 售货机接口
        /// </summary>
        public IMachine Machine { get; set; }

        public MachineModel(string com, MachineType type, IMachine machine)
        {
            Com = com;
            Type = type;
            Machine = machine;
        }

        public MachineModel(string com, MachineType type)
        {
            Com = com;
            Type = type;
            switch (type)
            {
                case MachineType.金码:
                    Machine = new MachineJMAdapter(com);
                    break;
                case MachineType.骏鹏:
                    Machine = new MachineJPAdapter(com);
                    break;
            }
        }
    }
}
