using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace TrafficOptimizer.Base.Model
{
    [DebuggerDisplay("Source.ID => Destination.ID")]
    [Serializable]
    public class Direction : IEqualityComparer<Direction>, IEquatable<Direction>
    {
        private KeyValuePair<Node, Node> _key;

        /// <summary>
        /// Начальный узел
        /// </summary>
        public Node Source
        {
            get { return _key.Key; }
        }
        /// <summary>
        /// Конечный узел
        /// </summary>
        public Node Destination
        {
            get { return _key.Value; }
        }

        public Direction(Node src, Node dst)
        {
            _key = new KeyValuePair<Node, Node>(src, dst);
        }
        public bool Equals(Direction x, Direction y)
        {
            return x.Source == y.Source && x.Destination == y.Destination;
        }
        public bool Equals(Direction other)
        {
            return Equals(this, other);
        }
        public int GetHashCode(Direction obj)
        {
            return obj.GetHashCode();
        }
        public override int GetHashCode()
        {
            return _key.Key.GetHashCode() ^ _key.Value.GetHashCode();
        }
    }
}
