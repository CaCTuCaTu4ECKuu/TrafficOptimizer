using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrafficOptimizer.RoadMap
{
    using Graph;
    using Graph.Model;
    using Model;

    public delegate void RoadChangedDelegate(Road road);
    public delegate void SectionChangedDelegate(Section section);

    public class RoadMap
    {
        protected List<Section> _sections = new List<Section>();
        protected Dictionary<Segment, Road> _roads = new Dictionary<Segment, Road>();

        public TimeSpan SimulationTime
        {
            get;
            private set;
        }

        public IEnumerable<Road> Roads
        {
            get
            {
                return _roads.Values;
            }
        }

        public Section GetSection(Node node)
        {
            if (Graph.NodeExists(node))
            {
                foreach (Section s in _sections)
                {
                    // Ищем в какой секции есть этот узел (может быть только в одной)
                    if (s.ContainedNodes.Contains(node))
                        return s;
                }
            }
            return null;
        }
        public Segment GetSegment(Road road)
        {
            if (road != null)
                return _roads.Where(r => r.Value == road).Select(e => e.Key).FirstOrDefault();
            return new Segment();
        }
        public Road GetRoad(Segment segment)
        {
            if (_roads.ContainsKey(segment))
                return _roads[segment];
            return null;
        }
        public Road GetRoad(Section s1, Section s2)
        {
            return GetRoad(new Segment(s1, s2));
        }

        public event RoadChangedDelegate OnRoadAdd;
        public event RoadChangedDelegate OnRoadRemove;
        public event RoadChangedDelegate OnRoadWeightChange;
        public event SectionChangedDelegate OnSectionAdd;
        public event SectionChangedDelegate OnSectionRemove;

        public Graph Graph
        {
            get;
            private set;
        }
        public RatioController RatioController
        {
            get;
            private set;
        }

        public Section MakeSection(IEnumerable<Road> roads = null)
        {
            Node src = Graph.MakeNode();
            Node drn = Graph.MakeNode();
            Section s = new Section(this, src, drn, roads);
            _sections.Add(s);

            if (OnSectionAdd != null)
                OnSectionAdd(s);

            return s;
        }
        public Road SetRoad(Section source, Section destination, float weight)
        {
            Segment seg = new Segment(source, destination);
            Road road = GetRoad(seg);

            if (road != null)
            {
                if (road.PrimaryLine.Source == source)
                {
                    // Мы добавляем основную дорогу
                    road.PrimaryLine.AddStreak();
                }
                else
                {
                    // Мы добавляем встречную
                    road.SlaveLine.AddStreak();
                }
            }
            else
            {
                Node psNode = Graph.MakeNode();  // Начало главной
                Node peNode = Graph.MakeNode();  // Конец главной
                Node ssNode = Graph.MakeNode();  // Начало обратной
                Node seNode = Graph.MakeNode();  // Конец обратной
                Edge pEdge = Graph.Unite(psNode, peNode, float.PositiveInfinity); // Прямо
                Edge sEdge = Graph.Unite(ssNode, seNode, float.PositiveInfinity); // Обратно

                road = new Road(this, source, destination, pEdge, sEdge, weight);

                _roads.Add(seg, road);
                source.AddRoad(road);
                destination.AddRoad(road);

                road.PrimaryLine.AddStreak();
                road.SlaveLine.AddStreak();

                if (OnRoadAdd != null)
                    OnRoadAdd(road);
            }

            return road;
        }
        public void RemoveRoad(Segment segment)
        {
            if (_roads.ContainsKey(segment))
            {
                Road r = _roads[segment];

                // Разделяем дороги (узлы удалятся автоматически)
                Graph.Divide(r.PrimaryLine.Edge.Source, r.PrimaryLine.Edge.Destination);
                Graph.Divide(r.SlaveLine.Edge.Source, r.SlaveLine.Edge.Destination);

                r.Source.RemoveRoad(r);
                r.Destination.RemoveRoad(r);

                _roads.Remove(segment);

                if (r.Source.RelatedRoads.Count() == 0)
                {
                    _sections.Remove(r.Source);
                    if (OnSectionRemove != null)
                        OnSectionRemove(r.Source);
                }
                if (r.Destination.RelatedRoads.Count() == 0)
                {
                    _sections.Remove(r.Destination);
                    if (OnSectionRemove != null)
                        OnSectionRemove(r.Destination);
                }
                if (OnRoadRemove != null)
                    OnRoadRemove(r);
            }
        }
        public void RemoveStreak(Section source, Section destination)
        {
            Segment seg = new Segment(source, destination);
            Road road = GetRoad(seg);
            if (road != null)
            {
                if (road.Source == source)
                    road.PrimaryLine.RemoveStreak();
                else
                    road.SlaveLine.RemoveStreak();
            }
        }
        public void ChangeWeight(Road road, float newWeight)
        {
            if (_roads.ContainsValue(road))
            {
                road.ChangeWeight(newWeight);
                if (OnRoadWeightChange != null)
                    OnRoadWeightChange(road);
            }
        }

        private void setRoadMap(Graph graph)
        {
            SimulationTime = new TimeSpan(0, 0, 0);
            if (graph != null)
                Graph = graph;
            else
                Graph = new Graph();

            RatioController = new RatioController(this, true);
        }
        public RoadMap(Graph graph)
        {
            setRoadMap(graph);
        }
        public RoadMap()
        {
            setRoadMap(null);
        }
    }
}
