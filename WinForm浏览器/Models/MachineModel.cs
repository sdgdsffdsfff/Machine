using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyWebBrowser.Enums;
using MachineDll;
using MachineJPDll;

namespace MyWebBrowser.Models
{
    /// <summary>
    /// 售货机接口
    /// 协议和串口的组合不同，MachineModel不同
    /// </summary>
    public class MachineModel
    {
        /// <summary>
        /// 串口号
        /// </summary>
        public string Com { get; set; }
        /// <summary>
        /// 货机接口的类型
        /// 协议不同，类型不同
        /// </summary>
        public MachineType Type { get; set; }

        public Machine Machine { get; set; }
        public MachineJP MachineJP { get; set; }

        public MachineModel(string com, MachineType type)
        {
            Com = com;
            Type = type;
            switch (type)
            {
                case MachineType.金码:
                    Machine = new Machine(com);
                    string msg = "";
                    try
                    {
                        Machine.Connect(out msg);
                    }
                    catch { }
                    break;
                case MachineType.骏鹏:
                    MachineJP = new MachineJP();
                    MachineJP.Init(com);
                    break;
            }
        }
    }
}
