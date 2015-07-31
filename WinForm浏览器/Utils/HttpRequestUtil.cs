using System;
using System.IO;
using System.Net;
using System.Text;
using System.Web;
using System.Configuration;

namespace MyWebBrowser.Utils
{
    /// <summary>
    /// HTTP请求工具类
    /// </summary>
    public class HttpRequestUtil
    {
        #region Url
        public static string Url
        {
            get
            {
                string url = "http://localhost:5189/";// ConfigurationManager.AppSettings["Url"];
                if (url.LastIndexOf("/") == url.Length - 1)
                {
                    url = url.Substring(0, url.Length - 1);
                }
                return url;
            }
        }
        #endregion

        #region 请求Url，不发送数据
        /// <summary>
        /// 请求Url，不发送数据
        /// </summary>
        public static string PostUrl(string url)
        {
            return PostUrl(url, String.Empty);
        }
        #endregion

        #region 请求Url，发送数据
        /// <summary>
        /// 请求Url，发送数据
        /// </summary>
        public static string PostUrl(string url, string postData)
        {
            if (postData == null) postData = String.Empty;
            url = HttpRequestUtil.Url + "/MachineWinform/" + url;
            byte[] data = Encoding.UTF8.GetBytes(postData);

            // 设置参数
            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
            CookieContainer cookieContainer = new CookieContainer();
            request.CookieContainer = cookieContainer;
            request.AllowAutoRedirect = true;
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = data.Length;
            Stream outstream = request.GetRequestStream();
            outstream.Write(data, 0, data.Length);
            outstream.Close();

            //发送请求并获取相应回应数据
            HttpWebResponse response = request.GetResponse() as HttpWebResponse;
            //直到request.GetResponse()程序才开始向目标网页发送Post请求
            Stream instream = response.GetResponseStream();
            StreamReader sr = new StreamReader(instream, Encoding.UTF8);
            //返回结果网页（html）代码
            string content = sr.ReadToEnd();
            return content;
        }
        #endregion

        #region 请求Url，不发送数据
        /// <summary>
        /// 请求Url，不发送数据
        /// </summary>
        public static string PostRemoteUrl(string url)
        {
            return PostRemoteUrl(url, String.Empty);
        }
        #endregion

        #region 请求Url，发送数据
        /// <summary>
        /// 请求Url，发送数据
        /// </summary>
        public static string PostRemoteUrl(string url, string postData)
        {
            if (postData == null) postData = String.Empty;
            byte[] data = Encoding.UTF8.GetBytes(postData);

            // 设置参数
            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
            CookieContainer cookieContainer = new CookieContainer();
            request.CookieContainer = cookieContainer;
            request.AllowAutoRedirect = true;
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = data.Length;
            Stream outstream = request.GetRequestStream();
            outstream.Write(data, 0, data.Length);
            outstream.Close();

            //发送请求并获取相应回应数据
            HttpWebResponse response = request.GetResponse() as HttpWebResponse;
            //直到request.GetResponse()程序才开始向目标网页发送Post请求
            Stream instream = response.GetResponseStream();
            StreamReader sr = new StreamReader(instream, Encoding.UTF8);
            //返回结果网页（html）代码
            string content = sr.ReadToEnd();
            return content;
        }
        #endregion

    }
}