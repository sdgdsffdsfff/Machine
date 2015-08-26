using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using CommonDll.Enums;

namespace CommonDll
{
    /// <summary>
    /// 货柜配置工具类
    /// </summary>
    public class BoxConfigUtil
    {
        #region 变量
        private static object _lock = new object();
        #endregion

        #region 获取配置
        /// <summary>
        /// 获取配置
        /// </summary>
        public static List<string> GetConfig(MachineType machineType, int box)
        {
            string prefix = string.Empty;
            switch (machineType)
            {
                case MachineType.金码:
                    prefix = "jm-";
                    break;
                case MachineType.骏鹏:
                    prefix = "jp-";
                    break;
            }
            string fileName = prefix + "box" + box.ToString() + ".config";

            List<string> result = new List<string>();
            lock (_lock)
            {
                try
                {
                    using (FileStream fs = new FileStream("config/" + fileName, FileMode.Open, FileAccess.Read))
                    {
                        using (StreamReader sr = new StreamReader(fs))
                        {
                            while (!sr.EndOfStream)
                            {
                                string line = sr.ReadLine();
                                if (!(line.IndexOf("#") == 0))
                                {
                                    result.Add(line);
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    FileLogger.LogError("读取货柜配置错误，请检查货柜配置");
                }
            }

            return result;
        }
        #endregion

    }
}
