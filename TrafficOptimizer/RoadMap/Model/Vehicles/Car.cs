using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrafficOptimizer.RoadMap.Model.Vehicles
{
    using Map;

    public class Car : Vehicle
    {

        public Car(float length, float width, float height, EndPoint destination) 
            : base(length, width, height, destination)
        {

        }
    }
}
