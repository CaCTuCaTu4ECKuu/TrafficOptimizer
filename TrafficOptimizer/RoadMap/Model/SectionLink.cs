using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrafficOptimizer.RoadMap.Model
{
    using Graph.Model;
    using Vehicles;
    
    public class SectionLink : VehicleContainer
    {
        public Section Section
        {
            get;
            private set;
        }
        public RoadMap RoadMap
        {
            get { return Section.RoadMap; }
        }
        public Edge Edge
        {
            get;
            private set;
        }
        private Dictionary<Streak, List<Streak>> _links = new Dictionary<Streak, List<Streak>>();
        public IDictionary<Streak, List<Streak>> Links
        {
            get
            {
                return _links;
            }
        }

        private float _weight;
        private bool _active = true;
        public override float Length
        {
            get
            {
                return _weight;
            }
        }
        public void ChangeWeight(float newWeight)
        {
            if (_active)
            {
                Edge.Weight = newWeight;
            }
            _weight = newWeight;
        }
        public void ChangeMoveRatio(float newValue)
        {
            MoveRatio = newValue;
        }
        public bool Active
        {
            get { return _active; }
            set
            {
                if (value != _active)
                {
                    _active = value;
                    if (value)
                    {
                        Edge.Weight = _weight;
                    }
                    else
                    {
                        _weight = Length;
                        Edge.Weight = float.PositiveInfinity;
                    }
                }
            }
        }
        
        public override IEnumerable<VehicleContainer> Destinations
        {
            get
            {
                if (_destinations == null)
                    _destinations = _links.SelectMany(e => e.Value).Distinct().Cast<VehicleContainer>().ToList();
                return _destinations;
            }
        }
        public override bool AllowToMove(VehicleContainer src, VehicleContainer dst)
        {
            if (src.GetType() == typeof(Streak) && dst.GetType() == typeof(Streak))
                return _links.ContainsKey((Streak)src) && _links[(Streak)src].Contains((Streak)dst);
            throw new ArgumentException("Only streaks allowed as arguments");
        }

        public SectionLink(Section section, Edge edge, float moveRatio)
            : base((IEnumerable<VehicleContainer>)null, moveRatio)
        {
            Section = section;
            Edge = edge;
            _weight = edge.Weight;
        }

        private void changeDestinations()
        {
            _destinations = null;
        }
        public void Link(Streak src, Streak dst)
        {
            if (!Destinations.Contains(dst))
            {
                if (!_links.ContainsKey(src))
                    _links.Add(src, new List<Streak>());
                _links[src].Add(dst);
                changeDestinations();
            }
        }
        public void Unlink(Streak src, Streak dst)
        {
            if (Destinations.Contains(dst))
            {
                _links[src].Remove(dst);
                if (_links[src].Count == 0)
                    _links.Remove(src);
                changeDestinations();
            }
        }
    }
}
