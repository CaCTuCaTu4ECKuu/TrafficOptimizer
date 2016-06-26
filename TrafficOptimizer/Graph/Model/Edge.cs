using System;
using System.Diagnostics;

namespace TrafficOptimizer.Graph.Model
{
    /// <summary>
    /// Ребро графа
    /// </summary>
    [DebuggerDisplay("{Source.GetHashCode()}=>{Destination.GetHashCode()} ({Weight})")]
    [Serializable]
    public class Edge
    {
        /// <summary>
        /// Направление грани
        /// </summary>
        public Direction Direction
        {
            get;
            private set;
        }
        /// <summary>
        /// Узел, от которого идет грань
        /// </summary>
        public Node Source
        {
            get { return Direction.Source; }
        }
        /// <summary>
        /// Узел, к которому идет грань
        /// </summary>
        public Node Destination
        {
            get { return Direction.Destination; }
        }
        /// <summary>
        /// Вес ребра
        /// </summary>
        public float Weight
        {
            get;
            set;
        }

        /// <summary>
        /// Задает ребру источник, назначение и вес
        /// </summary>
        /// <param name="src">Узел, из которого выходит ребро</param>
        /// <param name="dst">Узел, к которому ведет ребро</param>э
        /// <param name="dst">Вес ребра</param>
        public Edge(Node src, Node dst, float weight)
        {
            Direction = new Direction(src, dst);
            Weight = weight;
        }
    }
}
