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
        protected Dictionary<Section, Node> _sections = new Dictionary<Section, Node>();
        protected Dictionary<Segment, Road> _roads = new Dictionary<Segment, Road>();

        public IEnumerable<Road> Roads
        {
            get
            {
                return _roads.Values;
            }
        }

        public Node GetNode(Section section)
        {
            if (section != null && _sections.ContainsKey(section))
                return _sections[section];
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
        public Section GetSection(Node node)
        {
            if (node != null)
                return _sections.Where(n => n.Value == node).Select(e => e.Key).FirstOrDefault();
            return null;
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

        public Section MakeSection()
        {
            Node n = Graph.MakeNode();
            Section s = new Section(null);
            _sections.Add(s, n);
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
                    if (road.PrimaryLine.Streaks.Count() == 0)
                        Graph.ChangeWeight(_sections[road.Source], _sections[road.Destination], weight);
                    road.PrimaryLine.AddStreak();
                }
                else
                {
                    // Мы добавляем встречную
                    if (road.SlaveLine.Streaks.Count() == 0)
                        Graph.ChangeWeight(_sections[road.Destination], _sections[road.Source], weight);
                    road.SlaveLine.AddStreak();
                }
            }
            else
            {
                Edge e1 = Graph.Unite(_sections[source], _sections[destination], weight);
                Edge e2 = Graph.Unite(_sections[destination], _sections[source], weight);

                road = new Road(this, source, destination, e1, e2);
                road.PrimaryLine.OnStreakRemove += Road_OnStreakRemove;
                road.SlaveLine.OnStreakRemove += Road_OnStreakRemove;
                if (OnSectionAdd != null)
                {
                    OnSectionAdd(road.Source);
                    OnSectionAdd(road.Destination);
                }
                if (OnRoadAdd != null)
                    OnRoadAdd(road);
                road.PrimaryLine.AddStreak();
                road.SlaveLine.AddStreak();
                _roads.Add(seg, road);

                source.AddRoad(road);
                destination.AddRoad(road);
            }

            return road;
        }
        public void RemoveRoad(Segment segment)
        {
            if (_roads.ContainsKey(segment))
            {
                Road r = _roads[segment];
                r.PrimaryLine.OnStreakRemove -= Road_OnStreakRemove;
                r.SlaveLine.OnStreakRemove -= Road_OnStreakRemove;

                r.Source.RemoveRoad(r);
                r.Destination.RemoveRoad(r);

                Graph.Divide(_sections[r.Source], _sections[r.Destination]);
                Graph.Divide(_sections[r.Destination], _sections[r.Source]);
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
                Graph.ChangeWeight(_sections[road.Source], _sections[road.Destination], newWeight);
                Graph.ChangeWeight(_sections[road.Destination], _sections[road.Source], newWeight);
                if (OnRoadWeightChange != null)
                    OnRoadWeightChange(road);
            }
        }

        private void Road_OnStreakRemove(Road road, Line line)
        {
            if (road.Streaks == 0)
            {
                RemoveRoad(GetSegment(road));
            }
            else if (line.Streaks.Count() == 0)
            {
                Graph.ChangeWeight(_sections[line.Source], _sections[line.Destination], float.PositiveInfinity);
            }
        }

        private void setRoadMap(Graph graph)
        {
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
