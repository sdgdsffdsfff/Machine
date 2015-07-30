using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.IO;
using System.IO.Ports;

namespace IMachineDll
{
    /// <summary>
    /// 售货机工厂类
    /// </summary>
    public class MachineFactory
    {
        /// <summary>
        /// 货机接口缓存
        /// </summary>
        private static Dictionary<string, IMachine> dicMachine = new Dictionary<string, IMachine>();
        /// <summary>
        /// 锁变量
        /// </summary>
        public static object _lock = new object();

        /// <summary>
        /// 创建售货机类
        /// </summary>
        /// <param name="path">DLL物理路径</param>
        /// <param name="dllName">DLL名称(不含扩展名)，命名空间必须为DLL名称加“Dll”后缀，类名必须和DLL名称相同</param>
        ///  <param name="com">串口名称，如：COM1</param>
        public static IMachine Create(string path, string dllName, string com)
        {
            if (!dicMachine.ContainsKey(dllName)
                || dicMachine[dllName] == null)
            {
                using (FileStream fs = new FileStream(path + dllName + ".dll", FileMode.Open, FileAccess.Read))
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        byte[] byteArray = new byte[4096];
                        while (fs.Read(byteArray, 0, byteArray.Length) > 0)
                        {
                            ms.Write(byteArray, 0, byteArray.Length);
                        }

                        Assembly assembly = Assembly.Load(ms.ToArray());
                        dicMachine[dllName] = (IMachine)assembly.CreateInstance(dllName + "Dll." + dllName, false, BindingFlags.Default, null, new object[] { com }, null, null);
                    }
                }
            }

            return dicMachine[dllName];
        }
    }
}
