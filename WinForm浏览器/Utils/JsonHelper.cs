using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Web.Script.Serialization;

namespace MyWebBrowser.Utils
{
    public class JsonHelper
    {
        /// <summary>
        /// 对象转json
        /// </summary>
        public static string ObjectToJson(object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }

        /// <summary>
        /// json转对象
        /// </summary>
        public static List<string> JsonToListString(string json)
        {
            List<string> list = new List<string>();

            JObject jObject = JObject.Parse("{\"data\":" + json + "}");
            foreach (JToken jToken in jObject["data"])
            {
                list.Add(jToken.Value<string>());
            }

            return list;
        }

        /// <summary>
        /// json转对象
        /// </summary>
        public static List<Dictionary<string, string>> JsonToListDic(string json)
        {
            List<Dictionary<string, string>> list = new List<Dictionary<string, string>>();

            JObject jObject = JObject.Parse("{\"data\":" + json + "}");
            foreach (JToken jToken in jObject["data"])
            {
                Dictionary<string, string> dict = new Dictionary<string, string>();
                foreach (JProperty jProperty in jToken)
                {
                    dict.Add(jProperty.Name, jProperty.Value.Value<string>());
                }
                list.Add(dict);
            }

            return list;
        }

        /// <summary>
        /// json转对象
        /// </summary>
        public static Dictionary<string, string> JsonToDic(string json)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();

            JObject jObject = JObject.Parse("{\"data\":" + json + "}");
            foreach (JToken jToken in jObject["data"])
            {
                foreach (JProperty jProperty in jToken)
                {
                    dict.Add(jProperty.Name, jProperty.Value.Value<string>());
                }
            }

            return dict;
        }

    }
}
