using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrafficOptimizer.RoadMap.Model
{
    public class TrafficLight
    {
        public List<object> Colors
        {
            get;
            private set;
        }
        public List<int> Intervals
        {
            get;
            private set;
        }
        public object Color
        {
            get;
        }
    }
}
