using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrafficOptimizer.RoadMap.Model.Vehicles
{

    public class Car : Vehicle
    {

        public Car(float length, float width, float height, Section destination) 
            : base(length, width, height, destination)
        {

        }
    }
}
