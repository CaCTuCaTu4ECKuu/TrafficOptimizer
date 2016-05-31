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
    using Map;
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
            Road road = null;
            if ((road = GetRoad(GetDirection(start, end))) != null)
            {
                if (road.PrimaryLine.Edge.Source == _nodes[start])
                {
                    // Мы добавляем основную дорогу
                    road.PrimaryLine.ChangeStreaks(road.PrimaryLine.Streaks + 1);
                }
                else
                {
                    // Мы добавляем встречную
                    road.SlaveLine.ChangeStreaks(road.SlaveLine.Streaks + 1);
                }
                throw new NotImplementedException("Добавть дорогу");
            }
            else
            {
                // Добавляем новую дорогу
                float weight = Tools.Distance(start, end);
                // Обозначаем граф
                Node n1 = Graph.MakeNode();
                Node n2 = Graph.MakeNode();
                Edge pe = Graph.Unite(n1, n2, weight);
                Edge se = Graph.Unite(n2, n1, weight);

                // Добавляем расположение
                Road[] r = new Road[1];
                r[0] = new Road(this, pe, se, start, end);

                Section sec_start = new EndPoint(null, r);
                Section sec_end = new EndPoint(r, null);
            }
        }
        public void ChangePosition(PointF oldPosition, PointF newPosition)
        {
            
        }
        public void AddInterSection(Point position)
        {
            Intersection isec = new Intersection(null, null);
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
