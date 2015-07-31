using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Configuration;

namespace MyWebBrowser.Utils
{
    /// <summary>
    /// 供web页面JS调用的接口
    /// </summary>
    [System.Runtime.InteropServices.ComVisible(true)]
    public partial class JSInterface
    {
        #region 变量和构造函数
        /// <summary>
        /// 应用程序启动时间
        /// </summary>
        private static DateTime m_ApplicationStartTime = DateTime.Now;
        private WebBrowser m_WebBrowser;
        private Form1 m_Form1;

        public JSInterface(WebBrowser webBrowser, Form1 form1)
        {
            this.m_Form1 = form1;
            this.m_WebBrowser = webBrowser;

            WaitForStart waitForStart = new WaitForStart();
            waitForStart.Dock = DockStyle.Fill;
            form1.Controls.Add(waitForStart);
            waitForStart.BringToFront();
            new Thread(new ThreadStart(delegate()
            {
                string json = HttpRequestUtil.PostUrl("GetAllCom");
                List<Dictionary<string, string>> list = JsonHelper.JsonToListDic(json);
                MachineFactory.Init(list);
                InvokeUtil.Invoke(form1, new InvokeDelegate(delegate()
                {
                    form1.Controls.Remove(waitForStart);
                }));
            })).Start();
        }
        #endregion

        #region 获取货机连续运行时间
        public string GetRunTime()
        {
            string result;

            DateTime now = DateTime.Now;
            TimeSpan timeSpan = now.Subtract(m_ApplicationStartTime);
            if (timeSpan.TotalHours < 1)
            {
                int totalMinutes = (int)timeSpan.TotalMinutes;
                result = string.Format("{0}分钟", totalMinutes);
            }
            else if (timeSpan.TotalDays < 1)
            {
                int totalHours = (int)timeSpan.TotalHours;
                result = string.Format("{0}小时", totalHours);
            }
            else
            {
                int totalHours = (int)timeSpan.TotalHours;
                int days = totalHours / 24;
                int hours = totalHours % 24;
                result = string.Format("{0}天{1}小时", days, hours);
            }

            return result;
        }
        #endregion

        #region 关闭程序
        /// <summary>
        /// 关闭程序
        /// </summary>
        public void CloseWinForm()
        {
            Application.Exit();
        }
        #endregion

        #region 购买
        /// <summary>
        /// 购买
        /// </summary>
        public void Buy(int cost, string orderId)
        {
            new Thread(new ThreadStart(delegate()
            {
                Buy buy = new Buy();
                InvokeUtil.Invoke(m_WebBrowser, new InvokeDelegate(delegate()
                {
                    buy.Dock = DockStyle.Fill;
                    m_Form1.Controls.Add(buy);
                    buy.BringToFront();
                }));
                int amount = 0;
                while ((amount = MachineFactory.QueryAmount(orderId)) == 0)
                {
                    Thread.Sleep(100);
                }
                string msg = "";
                InvokeUtil.Invoke(m_WebBrowser, new InvokeDelegate(delegate()
                {
                    buy.label1.Text = "已投币" + amount + "……";
                }));
                bool bl = MachineFactory.Shipment(cost, false, orderId, out msg);
                if (bl)
                {
                    InvokeUtil.Invoke(m_WebBrowser, new InvokeDelegate(delegate()
                    {
                        m_Form1.Controls.Remove(buy);
                        object[] objects = new object[1];
                        objects[0] = "";
                        m_WebBrowser.Document.InvokeScript("success", null);
                    }));
                }
                else
                {
                }
            })).Start();
        }
        #endregion

        #region 直接取货
        /// <summary>
        /// 直接取货
        /// </summary>
        public void TakeGood(bool checkDrop, string orderId)
        {
            new Thread(new ThreadStart(delegate()
            {
                MachineFactory.TakeGood(checkDrop, orderId);
            })).Start();
        }
        #endregion

        #region 微信取货
        /// <summary>
        /// 微信取货
        /// </summary>
        public void WXTakeGood(bool checkDrop, string orderId)
        {
            new Thread(new ThreadStart(delegate()
            {
                while (true)
                {
                    try
                    {
                        string shopId = HttpRequestUtil.PostUrl("GetShopId");
                        string url = ConfigurationManager.AppSettings["RemoteUrl"] + "/TMGO2O_Wechat/pickup/cargoGoods.html?shopId=" + shopId;
                        string strJson = HttpRequestUtil.PostRemoteUrl(url);

                        MachineFactory.WXTakeGood(false, strJson);
                    }
                    catch (Exception ex)
                    {
                        //在这里写错误日志
                    }

                    Thread.Sleep(3000);
                }
            })).Start();
        }
        #endregion

        #region 货机检测
        /// <summary>
        /// 微信取货
        /// </summary>
        public void MachineCheck(bool checkDrop, string orderId)
        {
            new Thread(new ThreadStart(delegate()
            {
                string result = MachineFactory.MachineCheck_Main() + MachineFactory.MachineCheck_Box();
                //在这里调用JS
                InvokeUtil.Invoke(m_WebBrowser, new InvokeDelegate(delegate()
                {
                    object[] objects = new object[1];
                    objects[0] = result;
                    m_WebBrowser.Document.InvokeScript("machineCheckResult", objects);
                }));
            })).Start();
        }
        #endregion

    }
}
