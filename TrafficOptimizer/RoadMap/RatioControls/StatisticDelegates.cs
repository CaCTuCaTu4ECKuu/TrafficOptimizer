using System;

namespace TrafficOptimizer.RoadMap.RatioControls
{
    using Model;
    using Model.Vehicles;

    public delegate void VehicleContainerDelegate(Vehicle vehicle, VehicleContainer section, DateTime moment);
}
