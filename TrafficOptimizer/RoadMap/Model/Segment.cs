using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace TrafficOptimizer.RoadMap.Model
{
    [DebuggerDisplay("[{_sites.Key.ID}:{_sites.Value.ID}]-[{_sites.Value.ID}:{_sites.Key.ID}]")]
    public struct Segment : IEquatable<Segment>
    {
        private KeyValuePair<Section, Section> _sites;

        public Segment(Section s1, Section s2)
        {
            _sites = new KeyValuePair<Section, Section>(s1, s2);
        }

        public bool Equals(Segment other)
        {
            return Equals(this, other);
        }
        public bool Equals(Segment x, Segment y)
        {
            return x._sites.Key == y._sites.Key && x._sites.Value == y._sites.Value ||
                x._sites.Key == y._sites.Value && x._sites.Value == y._sites.Key;
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            if (obj.GetType() != typeof(Segment))
                return false;

            return Equals((Segment)obj);
        }
        public override int GetHashCode()
        {
            return _sites.Key.GetHashCode() ^ _sites.Value.GetHashCode();
        }
    }
}
