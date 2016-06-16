using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;

namespace TrafficOptimizer.Graph
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
        /// <summary>
        /// Добавляет узел в граф
        /// </summary>
        /// <returns></returns>
        public Node MakeNode()
        {
            Node n = new Node();
            _nodes.Add(n);
            return n;
        }
        public void RemoveNode(Node node)
        {
            lock (_edges)
            {
                _nodes.Remove(node);
                foreach (var e in GetRelatedTo(node))
                {
                    _edges.Remove(e.Direction);
                    e.Source.RelatedEdges.Remove(node);
                }
                foreach (var e in node.RelatedEdges.Values)
                {
                    _edges.Remove(e.Direction);
                }
            }
        }

        public Edge Unite(Node src, Node dst, float weight)
        {
            lock (_edges)
            {
                if (_nodes.Contains(src) && _nodes.Contains(dst))
                {
                    Direction d = new Direction(src, dst);
                    if (!_edges.ContainsKey(d))
                    {
                        Edge e = new Edge(src, dst, weight);
                        src.RelatedEdges.Add(dst, e);
                        _edges.Add(d, e);
                        return e;
                    }
                    else
                        throw new Exception("Ребро уже существует");
                }
                else
                    throw new Exception("В этом графе не существует одного из указанных узлов");
            }
        }
        private void clearDividing(Node n)
        {
            if (GetRelatedFrom(n).Count == 0 && GetRelatedTo(n).Count == 0)
                _nodes.Remove(n);
        }
        public void Divide(Node src, Node dst)
        {
            if (_nodes.Contains(src) && _nodes.Contains(dst))
            {
                lock (_edges)
                {
                    Direction d = new Direction(src, dst);
                    if (_edges.ContainsKey(d))
                    {
                        Edge e = _edges[d];
                        _edges.Remove(d);
                        src.RelatedEdges.Remove(dst);

                        clearDividing(src);
                        clearDividing(dst);
                    }
                    else
                        throw new Exception("Такого ребра не существует");
                }
            }
            else
                throw new Exception("В этом графе не существует одного из указанных узлов");
        }

        public void ChangeWeight(Node src, Node dst, float newWeight)
        {
            Edge e = GetEdge(new Direction(src, dst));
            if (e != null)
            {
                e.Weight = newWeight;
            }
        }

        public Edge GetEdge(Direction direction)
        {
            if (_edges.ContainsKey(direction))
                return _edges[direction];
            return null;
        }
        public List<Edge> GetRelatedFrom(Node src)
        {
            if (_nodes.Contains(src))
                return src.RelatedEdges.Values.ToList();
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

        public Edge this[Direction direction]
        {
            get
            {
                if (_edges.ContainsKey(direction))
                    return _edges[direction];
                return null;
            }
        }

        public Graph()
        {
            ID = _instances++;
        }
    }
}
