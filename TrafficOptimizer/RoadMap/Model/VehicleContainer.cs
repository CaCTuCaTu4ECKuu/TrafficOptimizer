using System;
using System.Collections.Generic;

namespace TrafficOptimizer.RoadMap.Model
{
    public abstract class VehicleContainer
    {
        private static uint _instances = 0;
        public uint ID
        {
            get;
            private set;
        }

        public VehicleContainer()
        {
            ID = _instances++;
        }
    }
}
