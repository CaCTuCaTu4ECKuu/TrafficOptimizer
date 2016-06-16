using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace TrafficOptimizer.Graph.Model
{
    [DebuggerDisplay("Source.ID => Destination.ID")]
    [Serializable]
    public class Direction : IEquatable<Direction>
    {
        /// <summary>
        /// Начальный узел
        /// </summary>
        public Node Source
        {
            get;
            private set;
        }
        /// <summary>
        /// Конечный узел
        /// </summary>
        public Node Destination
        {
            get;
            private set;
        }

        public Direction Inversed
        {
            get
            {
                return new Direction(Destination, Source);
            }
        }
        public bool IsInversed(Direction direction)
        {
            return Inversed == direction;
        }

        public Direction(Node src, Node dst)
        {
            Source = src;
            Destination = dst;
        }
        public bool Equals(Direction other)
        {
            return Source == other.Source && Destination == other.Destination;
        }
        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            Direction d = obj as Direction;
            if (d == null)
                return false;

            return Equals(d);
        }
        public override int GetHashCode()
        {
            return Source.GetHashCode() ^ Destination.GetHashCode();
        }
    }
}
