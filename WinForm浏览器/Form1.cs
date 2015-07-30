using System;
using System.ComponentModel;
using System.Configuration;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using Microsoft.Win32;
using System.Drawing;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using MyWebBrowser.Utils;

namespace MyWebBrowser
{
    public partial class Form1 : Form
    {
        #region 外部方法
        /// <summary>
        /// 清除Session
        /// </summary>
        [DllImport("wininet.dll", SetLastError = true)]
        private static extern bool InternetSetOption(IntPtr hInternet, int dwOption, IntPtr lpBuffer, int lpdwBufferLength);
        #endregion

        #region 变量
        /// <summary>
        /// 切换到广告时间
        /// </summary>
        private static int timeout = int.Parse(ConfigurationManager.AppSettings["Timeout"]) * 10;
        /// <summary>
        /// 无操作计数
        /// </summary>
        private static int timeoutCount = 0;
        /// <summary>
        /// 定时器，检测页面有无操作
        /// </summary>
        private static System.Windows.Forms.Timer timer;
        /// <summary>
        /// 当前显示的是否是广告页
        /// </summary>
        private static bool isAdvert = false;
        private static int lastX = -1;
        private static int lastY = -1;
        private static HtmlElementEventHandler htmlElementEventHandler = null;
        #endregion

        #region 构造函数
        public Form1()
        {
            InitializeComponent();
        }
        #endregion

        #region 窗体初始化
        private void Form1_Load(object sender, EventArgs e)
        {
            #region 注册表
            try
            {
                RegistryKey regKey = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Internet Explorer\MAIN\FeatureControl\FEATURE_BROWSER_EMULATION", true);
                string path = Application.ExecutablePath;
                string name = Path.GetFileName(path).Replace(".EXE", ".exe");
                if (regKey.GetValue(name) == null)
                {
                    //regKey.SetValue(name, "9999", RegistryValueKind.DWord); //ie9
                    regKey.SetValue(name, "10001", RegistryValueKind.DWord); //ie10
                    //regKey.SetValue(name, "11001", RegistryValueKind.DWord); //ie11
                }
            }
            catch { }
            #endregion

            #region 获取URL
            Uri url = null;
            Uri advertUrl = null;
            try
            {
                url = new Uri(ConfigurationManager.AppSettings["Url"]);
                advertUrl = new Uri(ConfigurationManager.AppSettings["AdvertUrl"]);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                Application.Exit();
                return;
            }
            #endregion

            #region 浏览器窗口全屏
            //this.WindowState = FormWindowState.Maximized;
            //this.TopMost = true;

            Rectangle rect = SystemInformation.VirtualScreen;
            webBrowser1.Width = rect.Width + 20;
            webBrowser1.Height = rect.Height;
            webBrowser1.Top = 0;
            webBrowser1.Left = 0;
            webBrowser2.Width = rect.Width + 20;
            webBrowser2.Height = rect.Height;
            webBrowser2.Top = 0;
            webBrowser2.Left = 0;
            #endregion

            #region 打开URL
            webBrowser1.ObjectForScripting = new JSInterface(webBrowser1, this);
            webBrowser1.ScriptErrorsSuppressed = true;
            webBrowser1.Url = url;
            webBrowser2.Url = advertUrl;
            webBrowser2.Visible = false;
            #endregion

            #region 定时器
            timer = new System.Windows.Forms.Timer();
            timer.Interval = 100;
            timer.Tick += new EventHandler(delegate(object obj, EventArgs ea)
            {
                if (timeoutCount < int.MaxValue) timeoutCount++;
                if (timeoutCount > timeout && isAdvert == false) //如果超时无操作且不是广告页
                {
                    timeoutCount = 0;
                    isAdvert = true;
                    webBrowser1.Visible = false;
                    webBrowser2.Visible = true;
                    Uri backFromAdvertUrl = new Uri(ConfigurationManager.AppSettings["BackFromAdvertUrl"]);
                    webBrowser1.Url = backFromAdvertUrl;
                    InternetSetOption(IntPtr.Zero, 42, IntPtr.Zero, 0);

                    Thread thread = new Thread(new ThreadStart(delegate()
                    {
                        Thread.Sleep(1000);

                        InvokeDelegate invoke;
                        htmlElementEventHandler = new HtmlElementEventHandler(delegate(object obj3, HtmlElementEventArgs heea)
                        {
                            //从广告页切换回来
                            invoke = new InvokeDelegate(delegate()
                            {
                                isAdvert = false;
                                webBrowser1.Visible = true;
                                webBrowser2.Visible = false;
                                webBrowser2.Document.MouseDown -= htmlElementEventHandler;
                            });
                            InvokeUtil.Invoke(this, invoke);
                        });

                        //当广告页鼠标移动时，切换回来
                        invoke = new InvokeDelegate(delegate()
                        {
                            webBrowser2.Document.MouseDown += htmlElementEventHandler;
                        });
                        InvokeUtil.Invoke(this, invoke);
                    }));
                    thread.Start();
                }
            });
            timer.Start();
            #endregion

            #region 启动守护进程
            if (Process.GetProcesses().ToList<Process>().Find(a => a.ProcessName == "SHJC") == null) //守护进程没有在运行
            {
                string path = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + "\\SHJC.exe";
                if (File.Exists(path)) //守护进程路径存在
                {
                    Process.Start(path);
                }
            }
            #endregion
        }
        #endregion

        #region webBrowser1_NewWindow
        private void webBrowser1_NewWindow(object sender, CancelEventArgs e)
        {
            e.Cancel = true;
            string url = webBrowser1.Document.ActiveElement.GetAttribute("href");
            webBrowser1.Url = new Uri(url);
        }
        #endregion

        #region webBrowser1_DocumentCompleted
        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            HtmlElementEventHandler htmlElementEventHandler = new HtmlElementEventHandler(delegate(object obj, HtmlElementEventArgs heea)
            {
                //当页面有操作时，设置intervalCount为0
                if (timeoutCount > 0 && lastX != heea.ClientMousePosition.X && lastY != heea.ClientMousePosition.Y)
                {
                    timeoutCount = 0;
                    lastX = heea.ClientMousePosition.X;
                    lastY = heea.ClientMousePosition.Y;
                }
            });

            BindMouseMove(webBrowser1.Document, htmlElementEventHandler);
        }
        #endregion

        #region webBrowser1_PreviewKeyDown
        private void webBrowser1_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            timeoutCount = 0;
        }
        #endregion

        #region 递归设置页面及子页面的MouseMove事件
        /// <summary>
        /// 递归设置页面及子页面的MouseMove事件
        /// </summary>
        private void BindMouseMove(HtmlDocument doc, HtmlElementEventHandler htmlElementEventHandler)
        {
            doc.MouseMove += htmlElementEventHandler;
            foreach (HtmlWindow frameWindow in doc.Window.Frames)
            {
                BindMouseMove(frameWindow.Document, htmlElementEventHandler);
            }
        }
        #endregion

        #region webBrowser2_PreviewKeyDown
        private void webBrowser2_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            isAdvert = false;
            webBrowser1.Visible = true;
            webBrowser2.Visible = false;
            webBrowser2.Document.MouseDown -= htmlElementEventHandler;
        }
        #endregion

        private void button1_Click(object sender, EventArgs e)
        {
            new Thread(new ThreadStart(delegate()
            {
                string msg = "";
                bool bl = MachineFactory.Shipment(200, false, "bc813603f04b4b09b2c1c9eca78f95b2", out msg);
                if (bl) MessageBox.Show("出货完成");
                //MachineFactory.Shipment(200, false, "55a805aabd254c1699dfea6e17a7e486", out msg);
            })).Start();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            MessageBox.Show("abc");
        }

    }
}
