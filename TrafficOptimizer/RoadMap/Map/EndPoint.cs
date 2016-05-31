using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrafficOptimizer.RoadMap.Map
{
    using Model;

    public class EndPoint : Section
    {

        public EndPoint(IEnumerable<Road> inRoads, IEnumerable<Road> outRoads) : base(inRoads, outRoads)
        {

        }
    }
}
