using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommonDll;
using CommonDll.Enums;
using MachineJMAdapterDll.Models;

namespace MachineJMAdapterDll.Utils
{
    /// <summary>
    /// 金码货柜配置工具类
    /// </summary>
    public class JMBoxConfigUtil
    {
        #region 获取货道配置
        /// <summary>
        /// 获取货道配置
        /// </summary>
        /// <returns></returns>
        public static RoadModelCollection GetRoadsConfig(int box)
        {
            RoadModelCollection roadModelCollection = new RoadModelCollection();

            List<string> list = BoxConfigUtil.GetConfig(MachineType.金码, box);
            int start = list.FindIndex(item => item.Trim() == "[roads]") + 1;

            int floor = 1;
            for (int i = start; i < list.Count; i++)
            {
                if (string.IsNullOrWhiteSpace(list[i]))
                {
                    break;
                }

                string strfloor = list[i].Split('=')[1];
                string[] strroads = strfloor.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string strroad in strroads)
                {
                    RoadModel road = new RoadModel();
                    road.Floor = floor;
                    road.Num = int.Parse(strroad);
                    roadModelCollection.RoadList.Add(road);
                }

                floor++;
            }

            roadModelCollection.FloorCount = floor - 1;
            return roadModelCollection;
        }
        #endregion

    }
}
