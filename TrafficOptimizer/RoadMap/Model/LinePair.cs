using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrafficOptimizer.RoadMap.Model
{
    public struct LinePair : IEquatable<LinePair>
    {
        public Line Source
        {
            get;
            private set;
        }
        public Line Destination
        {
            get;
            private set;
        }

        public LinePair(Line src, Line dst)
        {
            if (src.Road.RoadMap == dst.Road.RoadMap)
            {
                Source = src;
                Destination = dst;
            }
            else
                throw new NotImplementedException("Полосы от разных карт");
        }

        public bool Equals(LinePair other)
        {
            return Source == other.Source && Destination == other.Destination;
        }
        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            if (obj.GetType() != typeof(LinePair))
                return false;

            return Equals((LinePair)obj);
        }
        public override int GetHashCode()
        {
            return Source.GetHashCode() ^ Destination.GetHashCode();
        }
    }
}
