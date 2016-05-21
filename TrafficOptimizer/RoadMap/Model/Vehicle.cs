using System.Collections.Generic;

namespace TrafficOptimizer.RoadMap.Model
{
    public abstract class Vehicle
    {
        private static uint _instances;
        public uint ID
        {
            get;
            private set;
        }

        public double Length
        {
            get;
            private set;
        }
        public double Width
        {
            get;
            private set;
        }
        public double Height
        {
            get;
            private set;
        }

        private static Dictionary<VehicleContainer, double> _positons = new Dictionary<VehicleContainer, double>();
        public double PositionAt(VehicleContainer container)
        {
            return _positons[container];
        }
        public void Enter(VehicleContainer container, double position)
        {
            _positons.Add(container, position);
        }
        public void Leave(VehicleContainer container)
        {
            _positons.Remove(container);
        }

        public double Speed
        {
            get;
            set;
        }
        public double Acceleration
        {
            get;
            set;
        }

        public Vehicle(double length, double width, double height)
        {
            ID = _instances++;

            Length = length;
            Width = width;
            Height = height;
        }
    }
}
