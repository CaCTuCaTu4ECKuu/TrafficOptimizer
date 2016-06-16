using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrafficOptimizer.RoadMap.Model
{
    public class Streak : VehicleContainer
    {
        public Line Line
        {
            get;
            private set;
        }

        public Streak(Line line, IEnumerable<Streak> destinations) 
            : base(destinations)
        {
            Line = line;
        }
    }
}
