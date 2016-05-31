using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrafficOptimizer.RoadMap.Map
{
    using Model;

    public class Intersection : Section
    {

        public Intersection(IEnumerable<Road> inRoads, IEnumerable<Road> outRoads) : base(inRoads, outRoads)
        {

        }
    }
}
