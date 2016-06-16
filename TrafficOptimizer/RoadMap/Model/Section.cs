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
                    var lines = InRoads.Select(x => x.SlaveLine).Union(OutRoads.Select(x => x.PrimaryLine));
                    _destinations = lines.SelectMany(l => l.Streaks).ToList<VehicleContainer>();
                }
                return _destinations;
            }
        }

        private Dictionary<VehicleContainer, SectionLink> _links = new Dictionary<VehicleContainer, SectionLink>();

        public bool AllowToMove(VehicleContainer src,  VehicleContainer dst)
        {
            return Destinations.Contains(dst) && _links.ContainsKey(src) && _links[src].IsLeadsTo(dst);
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
        public void Link(VehicleContainer src, VehicleContainer dst, float moveRatio = 1f)
        {
            if (Destinations.Contains(dst))
            {
                if (!_links.ContainsKey(src))
                    _links.Add(src, new SectionLink(dst, moveRatio));
                else
                    _links[src].AddDestination(dst, moveRatio);
            }
        }
        public void Unlink(VehicleContainer src, VehicleContainer dst)
        {
            if (_links.ContainsKey(src) && _links[src].IsLeadsTo(dst))
            {
                if (_links[src].Destinations.Count() == 1)
                    _links.Remove(src);
                else
                    _links[src].RemoveDestination(dst);
            }
        }
    }
}