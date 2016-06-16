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

        protected List<VehicleContainer> _destinations;
        public virtual IEnumerable<VehicleContainer> Destinations
        {
            get { return _destinations.AsReadOnly(); }
        }

        public bool IsLeadsTo(VehicleContainer dest)
        {
            return _destinations.Contains(dest);
        }

        public VehicleContainer(IEnumerable<VehicleContainer> destinations)
        {
            ID = _instances++;
            _destinations = new List<VehicleContainer>();
            if (destinations != null)
                _destinations.AddRange(destinations);
        }

        public void AddDestination(VehicleContainer dst)
        {
            if (_destinations.Contains(dst))
                _destinations.Add(dst);
        }
        public void RemoveDestination(VehicleContainer dst)
        {
            _destinations.Remove(dst);
        }
    }
}
