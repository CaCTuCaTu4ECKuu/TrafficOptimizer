using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace TrafficOptimizer.Graph.Model
{
    public delegate void EdgeRemoveDelegate(Node n, Edge e);

    /// <summary>
    /// Узел графа
    /// </summary>
    [DebuggerDisplay("[{ID}] - [{RelatedEdges.Count}]")]
    [Serializable]
    public class Node
    {
        public event EdgeRemoveDelegate OnEdgeRemove;

        /// <summary>
        /// Грани, которые идут к другим узлам из того
        /// </summary>
        private Dictionary<Node, Edge> _relatedEdges = new Dictionary<Node, Edge>();
        /// <summary>
        /// Ребра, исходящие из этого узла
        /// </summary>
        public Dictionary<Node, Edge> RelatedEdges
        {
            get { return _relatedEdges; }
        }

        /// <summary>
        /// Вес ребра от этого узла до указанного. (float.PositiveInfinity если такого ребра нет)
        /// </summary>
        /// <param name="dst">Узел назначения</param>
        /// <returns>Вес ребра</returns>
        public float WeightTo(Node dst)
        {
            if (_relatedEdges.ContainsKey(dst))
            {
                return _relatedEdges[dst].Weight;
            }
            return float.PositiveInfinity;
        }
        /// <summary>
        /// Проверяет, существует ли ребра, исходящие от этого узла к заданному
        /// </summary>
        /// <param name="dst">Узел, наличие ребра к которому нужно проверить</param>
        public bool IsRelateTo(Node dst)
        {
            return RelatedEdges.ContainsKey(dst);
        }
    }
}
