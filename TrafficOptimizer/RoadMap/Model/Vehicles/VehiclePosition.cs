using System;

namespace TrafficOptimizer.RoadMap.Model.Vehicles
{
    public class VehiclePosition : IEquatable<VehiclePosition>
    {
        public VehicleContainer Container
        {
            get;
            private set;
        }
        public float Position
        {
            get;
            set;
        }

        public VehiclePosition(VehicleContainer container, float position)
        {
            Container = container;
            Position = position;
        }

        public bool Equals(VehiclePosition other)
        {
            return Container == other.Container && Position == other.Position;
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            VehiclePosition pos = (VehiclePosition)obj;
            if (pos == null)
                return false;

            return Equals(pos);
        }
        public override int GetHashCode()
        {
            return Container.GetHashCode() ^ Position.GetHashCode();
        }
    }
}
