using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace TrafficOptimizer.RoadMap.Model
{
    [DebuggerDisplay("[{ID}] In: {InRoads.Count()} Out: {OutRoads.Count()}")]
    public class Section : VehicleContainer
    {
        private List<Road> _relatedRoads;
        public IEnumerable<Road> RelatedRoads
        {
            get
            {
                return _relatedRoads.AsReadOnly();
            }
        }
        public IEnumerable<Road> InRoads
        {
            get
            {
                return _relatedRoads.Where(r => r.PrimaryLine.Destination == this);
            }
        }
        public IEnumerable<Road> OutRoads
        {
            get
            {
                return _relatedRoads.Where(r => r.SlaveLine.Source == this);
            }
        }

        public override IEnumerable<VehicleContainer> Destinations
        {
            get
            {
                if (_destinations == null)
                {
                    _destinations = new List<VehicleContainer>();

                }
                return _destinations;
            }
        }
        public Dictionary<VehicleContainer, List<VehicleContainer>> Links
        {
            get;
            private set;
        }
        public bool AllowToMove(VehicleContainer src,  VehicleContainer dst)
        {
            return Destinations.Contains(dst) && Links.ContainsKey(src) && Links[src].Contains(dst);
        }


        public Section(IEnumerable<Road> relatedRoads)
            : base(null)
        {
            _relatedRoads = new List<Road>();
            if (relatedRoads != null)
                _relatedRoads.AddRange(relatedRoads);
            resetDestination();
        }

        private void resetDestination()
        {
            _destinations = null;
        }
        public void AddRoad(Road road)
        {
            if (!_relatedRoads.Contains(road))
            {
                _relatedRoads.Add(road);
                resetDestination();
            }
        }
        public void RemoveRoad(Road road)
        {
            if (_relatedRoads.Contains(road))
            {
                _relatedRoads.Remove(road);
                resetDestination();
            }
        }
    }
}
