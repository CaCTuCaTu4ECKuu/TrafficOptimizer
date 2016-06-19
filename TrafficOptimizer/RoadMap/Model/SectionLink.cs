using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrafficOptimizer.RoadMap.Model
{
    using Vehicles;

    public class SectionLink : VehicleContainer
    {
        private float _length;
        public override float Length
        {
            get
            {
                return _length;
            }
        }

        public override IEnumerable<VehicleContainer> Destinations
        {
            get
            {
                return _destinations;
            }
        }

        public SectionLink(VehicleContainer destination, float moveRatio, float lenght)
            : base(destination, moveRatio)
        {
            _length = lenght;
        }
    }
}
