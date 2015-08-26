using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO;
using System.Windows.Forms;
using MachineJPDll.Models;

namespace MachineJPDll.Utils
{
    /// <summary>
    /// 写日志类
    /// </summary>
    public class LogHelper
    {
        #region 字段
        /// <summary>
        /// 锁
        /// </summary>
        public static object _lock = new object();
        #endregion

        #region Log 写日志
        /// <summary>
        /// 写日志
        /// </summary>
        /// <param name="msg">日志内容</param>
        private static void Log(string msg)
        {
            new Thread(new ThreadStart(delegate()
            {
                lock (_lock)
                {
                    string logPath = Application.StartupPath + "\\Log\\";
                    string fileName = DateTime.Now.ToString("yyyyMMdd") + ".txt";
                    string path = logPath + fileName;

                    if (!Directory.Exists(logPath))
                    {
                        Directory.CreateDirectory(logPath);
                    }

                    if (!File.Exists(path))
                    {
                        using (FileStream fs = new FileStream(path, FileMode.Create)) { fs.Close(); }
                    }

                    using (FileStream fs = new FileStream(path, FileMode.Append, FileAccess.Write))
                    {
                        using (StreamWriter sw = new StreamWriter(fs))
                        {
                            sw.WriteLine(msg);
                            sw.Flush();
                        }
                        fs.Close();
                    }
                }
            })).Start();
        }
        #endregion

        #region LogMachine 写日志
        /// <summary>
        /// 写日志(数据异常)
        /// </summary>
        public static void LogException(LogMsgType logMsgType, bool PC2VMC, string errorMsg, byte[] data)
        {
            string time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            string strPC2VMC = PC2VMC ? "PC-->VM" : "VM-->PC";
            if (data != null && data.Length > 0)
            {
                errorMsg = string.Join(" ", data.ToList<byte>().ConvertAll<string>(a => a.ToString("X2"))) + " " + errorMsg;
            }
            Log(string.Format("{0} |{1}| {2}:{3}", time, logMsgType.ToString().PadRight(6, ' '), strPC2VMC, errorMsg));
        }
        /// <summary>
        /// 写日志(错误日志)
        /// </summary>
        public static void LogError(LogMsgType logMsgType, bool PC2VMC, byte type, byte subtype, string msg)
        {
            string time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            string strPC2VMC = PC2VMC ? "PC-->VM" : "VM-->PC";
            Log(string.Format("{0} |{1}| {2}[MT:{3},subMT:{4}]:{5}", time, logMsgType.ToString().PadRight(6, ' '), strPC2VMC, type.ToString("X2"), subtype.ToString("X2"), msg));
        }
        /// <summary>
        /// 写日志
        /// </summary>
        public static void LogError(LogMsgType logMsgType, bool PC2VMC, byte[] data, string errorMsg)
        {
            string time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            string strPC2VMC = PC2VMC ? "PC-->VM" : "VM-->PC";
            string strData = "";
            if (data != null && data.Length > 0)
            {
                strData = string.Join(" ", data.ToList<byte>().ConvertAll<string>(a => a.ToString("X2")));
            }
            MT mt = new MT(data);
            string strSubtype = mt.Subtype == 0x00 ? "  " : mt.Subtype.ToString("X2");
            Log(string.Format("{0} |{1}| {2}[MT:{3},subMT:{4}]:{5} {6}", time, logMsgType.ToString().PadRight(6, ' '), strPC2VMC, mt.Type.ToString("X2"), strSubtype, strData, errorMsg));
        }
        /// <summary>
        /// 写日志
        /// </summary>
        public static void Log(LogMsgType logMsgType, bool PC2VMC, byte[] data)
        {
            string time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            string strPC2VMC = PC2VMC ? "PC-->VM" : "VM-->PC";
            string msg = "";
            if (data != null && data.Length > 0)
            {
                msg = string.Join(" ", data.ToList<byte>().ConvertAll<string>(a => a.ToString("X2")));
            }
            MT mt = new MT(data);
            string strSubtype = mt.Subtype == 0x00 ? "  " : mt.Subtype.ToString("X2");
            Log(string.Format("{0} |{1}| {2}[MT:{3},subMT:{4}]:{5}", time, logMsgType.ToString().PadRight(6, ' '), strPC2VMC, mt.Type.ToString("X2"), strSubtype, msg));
        }
        #endregion

    }

    #region 日志信息类型
    /// <summary>
    /// 日志信息类型
    /// </summary>
    public enum LogMsgType
    {
        Info,
        Error
    }
    #endregion

}
