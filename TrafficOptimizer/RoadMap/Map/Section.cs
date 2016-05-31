using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrafficOptimizer.RoadMap.Map
{
    using Model;

    public abstract class Section
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

        public Section(IEnumerable<Road> inRoads, IEnumerable<Road> outRoads)
        {
            if (inRoads == null)
                InRoads = new List<Road>();
            else
                InRoads = new List<Road>(inRoads);

            if (outRoads == null)
                OutRoads = new List<Road>();
            else
                OutRoads = new List<Road>(outRoads);
        }
    }
}
