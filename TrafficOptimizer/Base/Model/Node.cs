using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace TrafficOptimizer.Base.Model
{
    public delegate void EdgeRemoveDelegate(Node n, Edge e);

    /// <summary>
    /// Узел графа
    /// </summary>
    [DebuggerDisplay("[{ID}] - [{RelatedEdges.Count}]")]
    [Serializable]
    public class Node
    {
        private static uint _instances = 0;
        /// <summary>
        /// Идентификатор узла
        /// </summary>
        public uint ID
        {
            get; private set;
        }

        public event EdgeRemoveDelegate OnEdgeRemove;

        private Dictionary<Node, Edge> _relatedEdges = new Dictionary<Node, Edge>();
        /// <summary>
        /// Ребра, исходящие из этого узла
        /// </summary>
        public Dictionary<Node, Edge> RelatedEdges
        {
            get { return _relatedEdges; }
        }

        /// <summary>
        /// Вес ребра от этого узла до указанного. (decimal.MaxValue если такого ребра нет)
        /// </summary>
        /// <param name="dst">Узел назначения</param>
        /// <returns>Вес ребра</returns>
        public decimal WeightTo(Node dst)
        {
            if (_relatedEdges.ContainsKey(dst))
            {
                return _relatedEdges[dst].Weight;
            }
            return decimal.MaxValue;
        }
        /// <summary>
        /// Проверяет, существует ли ребра, исходящие от этого узла к заданному
        /// </summary>
        /// <param name="dst">Узел, наличие ребра к которому нужно проверить</param>
        public bool IsRelateTo(Node dst)
        {
            return RelatedEdges.ContainsKey(dst);
        }

        /// <summary>
        /// Добавить ребро
        /// </summary>
        /// <param name="dst">Узел назначения</param>
        /// <param name="weight">Вес грани к заданному узлу</param>
        /// <returns>Добавленная грань</returns>
        public Edge SetEdge(Node dst, decimal weight)
        {
            if (!_relatedEdges.ContainsKey(dst))
            {
                Edge e = new Edge(this, dst, weight);
                _relatedEdges.Add(dst, e);
                return e;
            }
            else
            {
                _relatedEdges[dst].Weight = weight;
                return _relatedEdges[dst];
            }
        }
        /// <summary>
        /// Убрать ребро
        /// </summary>
        /// <param name="dst">Узел назначения</param>
        public void UnsetEdge(Node dst)
        {
            if (_relatedEdges.ContainsKey(dst))
            {
                if (OnEdgeRemove != null)
                    OnEdgeRemove(this, _relatedEdges[dst]);
                _relatedEdges.Remove(dst);
            }
        }
        public void ClearEdges()
        {
            foreach (var n in _relatedEdges.Keys)
                UnsetEdge(n);
        }

        public Node()
        {
            ID = _instances++;
        }
    }
}
