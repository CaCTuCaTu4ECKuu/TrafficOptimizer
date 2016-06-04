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

        public float Length
        {
            get;
            private set;
        }
        public float Width
        {
            get;
            private set;
        }
        public float Height
        {
            get;
            private set;
        }

        private static Dictionary<VehicleContainer, float> _positons = new Dictionary<VehicleContainer, float>();
        public float PositionAt(VehicleContainer container)
        {
            return _positons[container];
        }
        public void Enter(VehicleContainer container, float position)
        {
            _positons.Add(container, position);
        }
        public void Leave(VehicleContainer container)
        {
            _positons.Remove(container);
        }

        public float Speed
        {
            get;
            set;
        }
        public float Acceleration
        {
            get;
            set;
        }

        public Vehicle(float length, float width, float height)
        {
            ID = _instances++;

            Length = length;
            Width = width;
            Height = height;
        }
    }
}
