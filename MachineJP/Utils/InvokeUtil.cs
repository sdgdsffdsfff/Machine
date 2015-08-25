using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MachineJPDll.Utils
{
    /// <summary>
    /// 跨线程访问控件的委托
    /// </summary>
    public delegate void InvokeDelegate();

    /// <summary>
    /// 跨线程访问控件类
    /// </summary>
    public class InvokeUtil
    {
        /// <summary>
        /// 跨线程访问控件
        /// </summary>
        /// <param name="ctrl">Form对象</param>
        /// <param name="de">委托</param>
        public static void Invoke(Control ctrl, Delegate de)
        {
            if (ctrl.IsHandleCreated)
            {
                ctrl.BeginInvoke(de);
            }
        }
    }
}
