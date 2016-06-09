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
    using Model;
    using Tools;

    public partial class RoadMap
    {
        protected Dictionary<PointF, Node> _nodes = new Dictionary<PointF, Node>();
        protected Dictionary<Node, PointF> _points = new Dictionary<Node, PointF>();

        protected Dictionary<Node, Section> _sections = new Dictionary<Node, Section>();
        protected Dictionary<Direction, Road> _roads = new Dictionary<Direction, Road>();

        public Graph Graph
        {
            get;
            private set;
        }
        public VehicleController VehicleController
        {
            get;
            private set;
        }

        public Direction GetDirection(PointF p1, PointF p2)
        {
            if (_nodes.ContainsKey(p1) && _nodes.ContainsKey(p2))
                return new Direction(_nodes[p1], _nodes[p2]);
            return null;
        }
        protected Road GetRoad(Direction dir)
        {
            if (dir != null)
            {
                if (_roads.ContainsKey(dir))
                    return _roads[dir];
                else if (_roads.ContainsKey(dir = new Direction(dir.Destination, dir.Source)))
                    return _roads[dir];
            }
            return null;

        }

        /// <summary>
        /// Добавление новой дороги или полосы на карту 
        /// </summary>
        /// <param name="start">Начальная точка</param>
        /// <param name="end">Конечная точка</param>
        public void AddRoad(PointF start, PointF end)
        {
            Road road = GetRoad(GetDirection(start, end));
            if (road != null)
            {
                if (road.PrimaryLine.Edge.Source == _nodes[start])
                {
                    // Мы добавляем основную дорогу
                    road.PrimaryLine.ChangeStreaks(road.PrimaryLine.Streaks.Count + 1);
                }
                else
                {
                    // Мы добавляем встречную
                    road.SlaveLine.ChangeStreaks(road.SlaveLine.Streaks.Count + 1);
                }
            }
            else
            {
                Node n1 = _nodes.ContainsKey(start) ? _nodes[start] : Graph.MakeNode();
                Node n2 = _nodes.ContainsKey(end) ? _nodes[end] : Graph.MakeNode();

                Section s1 = _sections.ContainsKey(n1) ? _sections[n1] : new Section(null, null);
                Section s2 = _sections.ContainsKey(n2) ? _sections[n2] : new Section(null, null);

                float weight = Tools.Distance(start, end);

                Edge prim_e = Graph.Unite(n1, n2, weight);
                Edge slave_e = Graph.Unite(n2, n1, weight);

                Road r = new Road(this, prim_e, slave_e, start, end);

                if (!_sections.ContainsKey(n1))
                {
                    _sections.Add(n1, s1);
                }
                _sections[n1].OutRoads.Add(r);
                if (!_sections.ContainsKey(n2))
                {
                    _sections.Add(n2, s2);
                }
                _sections[n2].InRoads.Add(r);

                // Обозначаем граф

                // Добавляем расположение

                if (!_nodes.ContainsKey(start))
                    _nodes.Add(start, n1);
                if (!_nodes.ContainsKey(end))
                    _nodes.Add(end, n2);
                if (!_points.ContainsKey(n1))
                    _points.Add(n1, start);
                if (!_points.ContainsKey(n2))
                    _points.Add(n2, end);



                _roads.Add(new Direction(n1, n2), r);
            }
        }
        public void ChangePosition(PointF oldPosition, PointF newPosition)
        {
            
        }
        public void AddInterSection(Point position)
        {
            Section isec = new Section(null, null);
            if (_nodes.ContainsKey(position))
            {

            }
            else
            {

            }
        }

        private void setRoadMap(Graph graph, RoadMapParameters parameters)
        {
            if (graph != null)
                Graph = graph;
            else
                Graph = new Graph();

            if (parameters != null)
                RoadMapParametrs = parameters;
            else
                RoadMapParametrs = new RoadMapParameters();

            VehicleController = new VehicleController(this);
        }
        public RoadMap(Graph graph, RoadMapParameters parameters)
        {
            setRoadMap(graph, parameters);
        }
        public RoadMap()
        {
            setRoadMap(null, null);
        }

        public void Step()
        {

        }
    }
}
