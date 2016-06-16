using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrafficOptimizer.RoadMap.Model;

namespace TrafficOptimizer.RoadMap.RatioControls
{
    using Model.Vehicles;

    public class MovementRatio : RatioController
    {
        public MovementRatio(RoadMap map, int space) : base(map, space)
        {

        }

        

        public void Vehicle_LeaveLine(Vehicle vehicle, VehicleContainer line, DateTime moment)
        {
            throw new NotImplementedException();
        }
        
        public void Vehicle_OnEnterLine(Vehicle vehicle, VehicleContainer line, DateTime moment)
        {
            throw new NotImplementedException();
        }
    }
}
