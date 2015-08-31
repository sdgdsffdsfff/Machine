using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using IMachineDll;
using IMachineDll.Models;
using MachineFactoryDll;
using WCFServerDll;

namespace MachineForm
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            new Thread(new ThreadStart(delegate()
            {
                MachineFactory.Init();
                OpenWCFServer();
            })).Start();
        }

        #region 启动服务
        /// <summary>
        /// 启动服务
        /// </summary>
        private void OpenWCFServer()
        {
            WSHttpBinding wsHttp = new WSHttpBinding();
            wsHttp.MaxBufferPoolSize = 524288;
            wsHttp.MaxReceivedMessageSize = 2147483647;
            wsHttp.ReaderQuotas.MaxArrayLength = 6553600;
            wsHttp.ReaderQuotas.MaxStringContentLength = 2147483647;
            wsHttp.ReaderQuotas.MaxBytesPerRead = 6553600;
            wsHttp.ReaderQuotas.MaxDepth = 6553600;
            wsHttp.ReaderQuotas.MaxNameTableCharCount = 6553600;
            wsHttp.CloseTimeout = new TimeSpan(0, 1, 0);
            wsHttp.OpenTimeout = new TimeSpan(0, 1, 0);
            wsHttp.ReceiveTimeout = new TimeSpan(0, 10, 0);
            wsHttp.SendTimeout = new TimeSpan(0, 10, 0);
            wsHttp.Security.Mode = SecurityMode.None;

            Uri baseAddress = new Uri("http://127.0.0.1:9999/wcfserver");
            ServiceHost host = new ServiceHost(typeof(WCFServer), baseAddress);

            ServiceMetadataBehavior smb = new ServiceMetadataBehavior();
            smb.HttpGetEnabled = true;
            host.Description.Behaviors.Add(smb);

            ServiceBehaviorAttribute sba = host.Description.Behaviors.Find<ServiceBehaviorAttribute>();
            sba.MaxItemsInObjectGraph = 2147483647;

            host.AddServiceEndpoint(typeof(IWCFServer), wsHttp, "");

            host.Open();
        }
        #endregion

        private void button1_Click(object sender, EventArgs e)
        {
            //AmountRpt amountRpt = MachineFactory.Machine.QueryAmount();
            //if (amountRpt != null)
            //{
            //    MessageBox.Show(amountRpt.Amount.ToString());
            //}
            //else
            //{
            //    MessageBox.Show("无");
            //}
            string com = "COM3";
            IMachine machine = MachineFactory.GetMachine(com);
            OperateResult result = machine.Shipment(1, 2, 3, false, 0, false);
            if (result.Success)
            {
                MessageBox.Show("成功");
            }
            else
            {
                MessageBox.Show(result.ErrorMsg);
            }
            //machine.RefundMoney(100);
            //machine.ClearAmount();
        }

    }
}
