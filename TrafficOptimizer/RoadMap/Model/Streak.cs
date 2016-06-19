using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrafficOptimizer.RoadMap.Model.Vehicles;

namespace TrafficOptimizer.RoadMap.Model
{
    public class Streak : VehicleContainer
    {
        public Line Line
        {
            get;
            private set;
        }
        public override float Length
        {
            get { return Line.Length; }
        }

        public Streak(Line line, IEnumerable<Streak> destinations) 
            : base(destinations)
        {
            Line = line;
        }

        public override IEnumerable<VehicleContainer> Destinations
        {
            get
            {
                return _destinations;
            }
        }
    }
}
