using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrafficOptimizer.RoadMap
{
    using Model;

    public class Intersection
    {
        public List<Road> InRoads
        {
            get;
            private set;
        }
        public List<Road> OutRoads
        {
            get;
            private set;
        }
    }
}
