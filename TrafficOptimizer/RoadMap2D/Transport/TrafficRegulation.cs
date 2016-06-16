using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrafficOptimizer.RoadMap2D.Transport
{
    public class TrafficRegulation
    {
        public float MinimumDistanceBetweenCars
        {
            get;
            private set;
        }
        public float MinimumDistanceBetweenMovingCars
        {
            get;
            set;
        }

        public TrafficRegulation()
        {
            MinimumDistanceBetweenCars = 1.0f;
            MinimumDistanceBetweenMovingCars = 3.0f;
        }
    }
}
