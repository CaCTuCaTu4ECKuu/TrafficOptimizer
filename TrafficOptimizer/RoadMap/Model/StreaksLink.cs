using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrafficOptimizer.RoadMap.Model
{
    /// <summary>
    /// Описывает связь входящей полосы движения с исходящей в секции
    /// </summary>
    public struct StreaksLink : IEquatable<StreaksLink>
    {
        public Streak Source
        {
            get;
            set;
        }
        public Streak Destination
        {
            get;
            set;
        }

        public StreaksLink(Streak src, Streak dst)
        {
            Source = src;
            Destination = dst;
        }

        public bool Equals(StreaksLink other)
        {
            return Source == other.Source && Destination == other.Destination;
        }
        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            if (obj.GetType() != typeof(StreaksLink))
                return false;

            return Equals((StreaksLink)obj);
        }
        public override int GetHashCode()
        {
            return Source.GetHashCode() ^ Destination.GetHashCode();
        }
    }
}
