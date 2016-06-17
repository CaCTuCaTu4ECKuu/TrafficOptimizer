using System;
using System.Collections.Generic;

namespace TrafficOptimizer.RoadMap.Model.Vehicles
{
    using Tools;

    public delegate void VehicleMoveDelegate(VehicleContainer container, Vehicle vehicle);

    public abstract class VehicleContainer
    {
        private static uint _instances = 0;
        public uint ID
        {
            get;
            private set;
        }

        protected List<VehicleContainer> _destinations = new List<VehicleContainer>();
        protected List<Vehicle> _vehicles = new List<Vehicle>();
        public virtual IEnumerable<VehicleContainer> Destinations
        {
            get { return _destinations.AsReadOnly(); }
        }
        public virtual IEnumerable<Vehicle> Vehicles
        {
            get { return _vehicles.AsReadOnly(); }
        }
        public abstract float Length
        {
            get;
        }
        public bool IsLeadsTo(VehicleContainer dest)
        {
            return _destinations.Contains(dest);
        }

        public VehicleContainer(IEnumerable<VehicleContainer> destinations)
        {
            ID = _instances++;
            if (destinations != null)
                _destinations.AddRange(destinations);
        }

        public virtual void AddDestination(VehicleContainer dst)
        {
            if (!_destinations.Contains(dst))
                _destinations.Add(dst);
        }
        public virtual void RemoveDestination(VehicleContainer dst)
        {
            _destinations.Remove(dst);
        }

        public event VehicleMoveDelegate OnVehicleEnter;
        public event VehicleMoveDelegate OnVehicleLeave;

        public bool IsSpaceFree(object Route,float front, float back)
        {
            foreach (Vehicle v in _vehicles)
            {
                float vFront = v.FrontPosition.Container == this ? v.FrontPosition.Position : Length + v.FrontPosition.Position;
                float vBack = vFront = v.Length;
                if (Tools.IsSegmentsOverlapping(front, back, vFront, vBack))
                    return false;
            }
            return true;
        }
        public void Move(Vehicle vehicle)
        {
            if (_vehicles.Contains(vehicle))
            {

            }
            else if (vehicle.FrontPosition.Container.IsLeadsTo(this))
            {
                
            }
            else
                throw new ArgumentException("Расположение машины ведет к этому контейнеру");
        }
    }
}
