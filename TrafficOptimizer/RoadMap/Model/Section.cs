using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrafficOptimizer.RoadMap.Model
{
    public partial class Section
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

        public Road MaxStreaks
        {
            get
            {
                Road max = InRoads.Count > 0 ? InRoads[0] : OutRoads.Count > 0 ? OutRoads[0] : null;
                if (max != null)
                {
                    foreach (var r in InRoads)
                    {
                        if (r.Streaks > max.Streaks)
                            max = r;
                    }
                    foreach (var r in OutRoads)
                    {
                        if (r.Streaks > max.Streaks)
                            max = r;
                    }
                }
                return max;
            }
        }

        public Section(IEnumerable<Road> inRoads, IEnumerable<Road> outRoads)
        {
            InRoads = new List<Road>();
            if (inRoads != null)
                InRoads.AddRange(inRoads);
            OutRoads = new List<Road>();
            if (outRoads != null)
                OutRoads.AddRange(outRoads);
        }
    }
}
