using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MachineJMAdapterDll.Models
{
    /// <summary>
    /// 货道集合
    /// </summary>
    public class RoadModelCollection
    {
        /// <summary>
        /// 层数
        /// </summary>
        public int FloorCount { get; set; }
        /// <summary>
        /// 货道集合
        /// </summary>
        public List<RoadModel> RoadList { get; set; }

        public RoadModelCollection()
        {
            RoadList = new List<RoadModel>();
        }
    }
}
