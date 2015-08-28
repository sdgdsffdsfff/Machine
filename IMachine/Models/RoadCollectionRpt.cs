using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IMachineDll.Models
{
    /// <summary>
    /// 货道信息集合
    /// </summary>
    public class RoadCollectionRpt
    {
        /// <summary>
        /// 层数
        /// </summary>
        public int FloorCount { get; set; }
        /// <summary>
        /// 货道集合
        /// </summary>
        public List<RoadRpt> RoadList { get; set; }

        public RoadCollectionRpt()
        {
            RoadList = new List<RoadRpt>();
        }
    }
}
