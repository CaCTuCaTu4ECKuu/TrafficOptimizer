using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrafficOptimizer.RoadMap.Model
{
    public class Section
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

        private void setSection(List<Road> inRoads, List<Road> outRoads)
        {
            InRoads = inRoads;
            OutRoads = outRoads;
        }
        public Section(IEnumerable<Road> inRoads, IEnumerable<Road> outRoads)
        {
            setSection(new List<Road>(inRoads), new List<Road>(outRoads));
        }
        public Section(Road inRoad, Road outRoad)
        {
            List<Road> inr = new List<Road>();
            List<Road> outr = new List<Road>();

            if (inRoad != null)
                inr.Add(inRoad);
            if (outRoad != null)
                outr.Add(outRoad);

            setSection(inr, outr);
        }
    }
}
