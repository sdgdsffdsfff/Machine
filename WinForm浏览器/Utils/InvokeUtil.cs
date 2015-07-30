using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace MyWebBrowser.Utils
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
        public static void Invoke(Control ctrl, InvokeDelegate de)
        {
            if (ctrl.IsHandleCreated)
            {
                ctrl.BeginInvoke(de);
            }
        }

        /// <summary>
        /// 在线程中执行代码
        /// </summary>
        public static void ExecuteCode(Control ctrl, InvokeDelegate de)
        {
            new Thread(new ThreadStart(delegate()
            {
                InvokeUtil.Invoke(ctrl, de);
            })).Start();
        }
    }
}