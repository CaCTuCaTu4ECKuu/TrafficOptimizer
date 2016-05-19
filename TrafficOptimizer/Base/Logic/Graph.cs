using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;

namespace TrafficOptimizer.Base.Logic
{
    using Model;

    /// <summary>
    /// Граф
    /// </summary>
    [DebuggerDisplay("[{ID}] - Nodes: {_nodes.Count}, Edges: {_edges.Count}")]
    [Serializable]
    public class Graph
    {
        private static uint _instances = 0;
        /// <summary>
        /// Идентификатор графа
        /// </summary>
        public uint ID
        {
            get;
            private set;
        }

        private List<Node> _nodes = new List<Node>();
        private Dictionary<Direction, Edge> _edges = new Dictionary<Direction, Edge>();

        #region Nodes & Edges
        private delegate void NodeRemoveDelegate(Node n);
        private event NodeRemoveDelegate OnNodeRemove;
        private void RelatedEdgeClean(Node n, Edge e)
        {
            if (_edges.ContainsKey(e.Direction))
            {
                _edges.Remove(e.Direction);
            }

        }

        /// <summary>
        /// Добавляет узел в граф
        /// </summary>
        /// <returns></returns>
        public Node MakeNode()
        {
            Node n = new Node();
            lock (_edges)
            {
                OnNodeRemove += n.UnsetEdge;
                n.OnEdgeRemove += RelatedEdgeClean;
                _nodes.Add(n);
            }
            return n;
        }
        public void RemoveNode(Node node)
        {
            lock (_edges)
            {
                node.OnEdgeRemove -= RelatedEdgeClean;
                OnNodeRemove -= node.UnsetEdge;

                node.ClearEdges();
                if (OnNodeRemove != null)
                {
                    OnNodeRemove(node);
                }
            }
        }
        public Edge Unite(Node src, Node dst, decimal weight)
        {
            if (_nodes.Contains(src) && _nodes.Contains(dst))
            {
                Direction d = new Direction(src, dst);
                if (!_edges.ContainsKey(d))
                {
                    Edge e = src.SetEdge(dst, weight);
                    _edges.Add(d, e);
                    return e;
                }
                else
                    throw new Exception("Ребро уже существует");
            }
            else
                throw new Exception("В этом графе не существует одного из указанных узлов");
        }
        public void Divide(Node src, Node dst)
        {
            if (_nodes.Contains(src) && _nodes.Contains(dst))
            {
                Direction d = new Direction(src, dst);
                if (_edges.ContainsKey(d))
                {
                    src.UnsetEdge(dst);
                }
                else
                    throw new Exception("Такого ребра не существует");
            }
            else
                throw new Exception("В этом графе не существует одного из указанных узлов");
        }

        public Edge GetEdge(Direction direction)
        {
            return _edges[direction];
        }
        public List<Edge> GetRelatedFrom(Node src)
        {
            if (_nodes.Contains(src))
                return src.RelatedEdges.Select(e => e.Value).ToList();
            else
                throw new Exception("В графе нет указанного узла");
        }
        public List<Edge> GetRelatedTo(Node dst)
        {
            if (_nodes.Contains(dst))
                return _edges.Where(e => e.Key.Destination == dst).Select(e => e.Value).ToList();
            else
                throw new Exception("В грефе не существует указанного узла");
        }
        #endregion

        #region Pathes
        public Path FindPath(Node src, Node dst)
        {
            return PathBuilder.FindShortestPath(_nodes, _edges, new Direction(src, dst));
        }
        public Path FindPath(Node src, Node dst, List<Edge> restricked)
        {
            Dictionary<Direction, Edge> allowedEdges = _edges.Where(e => !restricked.Contains(e.Value)).ToDictionary(e => e.Key, e => e.Value);
            return PathBuilder.FindShortestPath(_nodes, allowedEdges, new Direction(src, dst));

        }
        public Path FindPath(List<Node> scope, Node src, Node dst)
        {
            return PathBuilder.FindShortestPath(scope, _edges, new Direction(src, dst));
        }
        public Path FindPath(List<Node> scope, Node src, Node dst, List<Edge> restricked)
        {
            if (scope.Contains(src) && scope.Contains(dst))
            {
                var allow = _edges.Where(e => scope.Contains(e.Key.Source) && scope.Contains(e.Key.Destination));
                Dictionary<Direction, Edge> allowedEdges = allow.Where(e => !restricked.Contains(e.Value)).ToDictionary(e => e.Key, e => e.Value);
                return PathBuilder.FindShortestPath(_nodes, allowedEdges, new Direction(src, dst));
            }
            else
                throw new ArgumentException("Начало или конец вне зоны поиска");
        }
        #endregion

        public Graph()
        {
            ID = _instances++;
        }
    }
}
