using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrafficOptimizer.RoadMap
{
    using Graph;
    using Graph.Model;
    using Tools;

    public class RoadMap
    {
        private Dictionary<Point, Node> _nodes = new Dictionary<Point, Node>();
        private Dictionary<Node, Point> _points = new Dictionary<Node, Point>();
        private Dictionary<Node, Section> _sections = new Dictionary<Node, Section>();
        private Dictionary<Edge, Road> _roads = new Dictionary<Edge, Road>();

        public Graph Graph
        {
            get;
            private set;
        }

        public Direction GetDirection(Point p1, Point p2)
        {
            return new Direction(_nodes[p1], _nodes[p2]);
        }
        private Node[] GetOrCreate(Point p1, Point p2)
        {
            Node[] res = new Node[2];
            if (_nodes.ContainsKey(p1))
                res[0] = _nodes[p1];
            else
            {
                res[0] = Graph.MakeNode();
                _nodes.Add(p1, res[0]);
                _points.Add(res[0], p1);
            }
            
            if (_nodes.ContainsKey(p2))
                res[1] = _nodes[p2];
            else
            {
                res[1] = Graph.MakeNode();
                _nodes.Add(p2, res[1]);
                _points.Add(res[1], p2);
            }
            return res;
        }
        public void AddRoad(Point start, Point end)
        {
            Node[] nodes = GetOrCreate(start, end);
            Direction dir = new Direction(nodes[0], nodes[1]);
            Edge e1;
            if ((e1 = Graph.GetEdge(dir)) != null)
            {

            }
            else
            {
                var weight = Calculator.Length(start, end);
                e1 = Graph.Unite(nodes[0], nodes[1], weight);
                Edge e2 = Graph.Unite(nodes[1], nodes[0], weight);
                Road r = new Road(e1, e2);

                dir = GetDirection(start, end);
                _roads.Add(e1, r);
                _roads.Add(e2, r);
            }
        }
        public void ChangePosition(Point oldPosition, Point newPosition)
        {
            var node = _nodes[oldPosition];
            _nodes.Remove(oldPosition);

            _nodes.Add(newPosition, node);
            _points[node] = newPosition;

            foreach (var e in node.RelatedEdges.Values)
            {
                e.Weight = Calculator.Length(newPosition, _points[e.Destination]);
            }
            foreach (var e in Graph.GetRelatedTo(node))
            {
                e.Weight = Calculator.Length(_points[e.Source], newPosition);
            }
        }

        public RoadMap(Graph graph)
        {
            Graph = graph;
        }
    }
}
