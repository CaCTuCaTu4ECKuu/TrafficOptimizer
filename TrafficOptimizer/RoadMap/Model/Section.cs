using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace TrafficOptimizer.RoadMap.Model
{
    using Graph.Model;
    using Vehicles;

    [DebuggerDisplay("In: {InRoads.Count()} Out: {OutRoads.Count()}")]
    public class Section : VehicleContainer
    {
        public RoadMap RoadMap
        {
            get;
            private set;
        }
        /// <summary>
        /// Узел графа, который представляет эту секцию как источник пути
        /// </summary>
        public Node Source
        {
            get;
            private set;
        }
        /// <summary>
        /// Узел графа, который представляет эту секцию как пункт назначения
        /// </summary>
        public Node Drain
        {
            get;
            private set;
        }

        private List<Road> _relatedRoads = new List<Road>();
        /// <summary>
        /// Все дороги, которые проходят через эту секцию
        /// </summary>
        public IEnumerable<Road> RelatedRoads
        {
            get
            {
                return _relatedRoads.AsReadOnly();
            }
        }
        /// <summary>
        /// Дороги, которые идут к данной секции
        /// </summary>
        public IEnumerable<Road> InRoads
        {
            get
            {
                return _relatedRoads.Where(r => r.PrimaryLine.Destination == this);
            }
        }
        /// <summary>
        /// Дороги, идущие от данной секции
        /// </summary>
        public IEnumerable<Road> OutRoads
        {
            get
            {
                return _relatedRoads.Where(r => r.PrimaryLine.Source == this);
            }
        }
        private List<Node> _containedNodes;
        public IEnumerable<Node> ContainedNodes
        {
            get
            {
                if (_containedNodes == null)
                {
                    _containedNodes = new List<Node>();
                    _containedNodes.Add(Source);
                    _containedNodes.Add(Drain);
                    foreach (var r in _relatedRoads)
                    {
                        if (r.Destination == this)
                        {
                            _containedNodes.Add(r.PrimaryLine.Edge.Destination);
                            _containedNodes.Add(r.SlaveLine.Edge.Source);
                        }
                        else
                        {
                            _containedNodes.Add(r.PrimaryLine.Edge.Source);
                            _containedNodes.Add(r.SlaveLine.Edge.Destination);
                        }
                    }
                }
                return _containedNodes;
            }
        }

        /// <summary>
        /// Полосы и секции, в которые можно попасть из этой секции
        /// </summary>
        public override IEnumerable<VehicleContainer> Destinations
        {
            get
            {
                if (_destinations == null)
                {
                    var lines = InRoads.Select(x => x.SlaveLine).Union(OutRoads.Select(x => x.PrimaryLine));
                    _destinations = lines.SelectMany(s => s.Streaks).Cast<VehicleContainer>()
                        .Union(lines.Select(l => l.Destination).Cast<VehicleContainer>()).Distinct().ToList();
                }
                return _destinations;
            }
        }

        public override float Length
        {
            get
            {
                return 0;
            }
        }
        public float LengthTo(Section target)
        {
            if (_destinations.Contains(target))
            {
                return RoadMap.GetRoad(this, target).Length;
            }
            return float.PositiveInfinity;
        }

        private Dictionary<LinePair, SectionLink> _lineLinks = new Dictionary<LinePair, SectionLink>();

        /// <summary>
        /// Можно ли проехать к месту назначения из источника через этот узел.
        /// Если это две полосы, то также проверяет есть ли путь между ними
        /// </summary>
        /// <param name="src">Начало</param>
        /// <param name="dst">Место назначения</param>
        public override bool AllowToMove(VehicleContainer src,  VehicleContainer dst)
        {
            // Только если указаны 2 streak-а нужно проверять есть ли между ними связь
            if (src.GetType() == typeof(Streak) && dst.GetType() == typeof(Streak))
            {
                LinePair pair = new LinePair(((Streak)src).Line, ((Streak)dst).Line);
                if (_lineLinks.ContainsKey(pair))
                    return _lineLinks[pair].AllowToMove(src, dst);
                return false;
            }
            
            return base.AllowToMove(src, dst);
        }

        public Section(RoadMap map, Node source, Node drain, IEnumerable<Road> relatedRoads)
            : base((VehicleContainer)null)
        {
            RoadMap = map;
            Source = source;
            Drain = drain;

            if (relatedRoads != null)
            {
                foreach (var r in relatedRoads)
                    AddRoad(r);
            }
            RoadChanged(null, null);
        }

        private void RoadChanged(Road road, Line line)
        {
            _destinations = null;
            _containedNodes = null;
        }
        public void AddRoad(Road road)
        {
            if (!_relatedRoads.Contains(road))
            {
                if (road.Source == this)
                {
                    // Дорога выходит отсюда
                    // Задает в графе путь от этой секции на выходящую сторону движения
                    RoadMap.Graph.Unite(Source, road.PrimaryLine.Edge.Source, 0);
                    // Задает в графе путь от входящей стороны движения в эту секцию
                    RoadMap.Graph.Unite(road.SlaveLine.Edge.Destination, Drain, 0);
                }
                else
                {
                    // Дорога идет сюда
                    // Задает в графе путь от входящей полосы к этой секции
                    RoadMap.Graph.Unite(road.PrimaryLine.Edge.Destination, Drain, 0);
                    // Задает в графе путь от этой секции к выходящей полосе
                    RoadMap.Graph.Unite(Source, road.SlaveLine.Edge.Source, 0);
                }

                _relatedRoads.Add(road);
                road.PrimaryLine.OnStreakAdd += RoadChanged;
                road.PrimaryLine.OnStreakRemove += RoadChanged;
                road.SlaveLine.OnStreakAdd += RoadChanged;
                road.SlaveLine.OnStreakRemove += RoadChanged;
                RoadChanged(null, null);
            }
        }
        public void RemoveRoad(Road road)
        {
            if (_relatedRoads.Contains(road))
            {
                if (road.Source == this)
                {
                    // Дорога выходит отсюда
                    // Задает в графе путь от этой секции на выходящую сторону движения
                    RoadMap.Graph.Divide(Source, road.PrimaryLine.Edge.Source);
                    // Задает в графе путь от входящей стороны движения в эту секцию
                    RoadMap.Graph.Divide(road.SlaveLine.Edge.Destination, Drain);
                }
                else
                {
                    // Дорога идет сюда
                    // Задает в графе путь от входящей полосы к этой секции
                    RoadMap.Graph.Divide(road.PrimaryLine.Edge.Destination, Drain);
                    // Задает в графе путь от этой секции к выходящей полосе
                    RoadMap.Graph.Divide(Source, road.SlaveLine.Edge.Source);
                }

                _relatedRoads.Remove(road);
                road.PrimaryLine.OnStreakAdd -= RoadChanged;
                road.PrimaryLine.OnStreakRemove -= RoadChanged;
                road.SlaveLine.OnStreakAdd -= RoadChanged;
                road.SlaveLine.OnStreakRemove -= RoadChanged;
                RoadChanged(null, null);
            }
        }

        public void SetLink(Line src, Line dst, float moveRatio = 1.0f, float length = 0)
        {
            if (src.Destination == this && Destinations.Contains(dst.Destination))
            {
                LinePair pair = new LinePair(src, dst);
                if (!_lineLinks.ContainsKey(pair))
                {
                    Edge e = RoadMap.Graph.Unite(src.Edge.Destination, dst.Edge.Source, length);
                    _lineLinks.Add(pair, new SectionLink(this, e, moveRatio));
                    src.OnStreakAdd += Link_OnStreakAdd;
                    src.OnStreakRemove += Link_OnStreakRemove;
                    dst.OnStreakAdd += Link_OnStreakAdd;
                    dst.OnStreakRemove += Link_OnStreakRemove;
                }
                else
                    throw new NotImplementedException("Эти стороны движения уже связаны");
            }
            else
                throw new ArgumentException("Одна из дорог не связана с этой секцией");
        }

        private void Link_OnStreakRemove(Road road, Line line)
        {
            if (line.Streaks.Count() == 0)
            {
                //TODO: проверить что я все правильно написал
                foreach (var link in _lineLinks)
                {
                    if (link.Value.Edge.Source == line.Edge.Destination 
                        || link.Value.Edge.Destination == line.Edge.Source)
                    {
                        RoadMap.Graph.ChangeWeight(link.Value.Edge, float.PositiveInfinity);
                    }
                }
            }
        }

        private void Link_OnStreakAdd(Road road, Line line)
        {
            if (line.Streaks.Count() == 1)
            {
                //TODO: проверить что я все правильно написал
                foreach (var link in _lineLinks)
                {
                    if (link.Value.Edge.Source == line.Edge.Destination
                        || link.Value.Edge.Destination == line.Edge.Source)
                    {
                        RoadMap.Graph.ChangeWeight(link.Value.Edge, line.Length);
                    }
                }
            }
        }

        public void UnsetLink(Line src, Line dst)
        {
            LinePair pair = new LinePair(src, dst);
            if (_lineLinks.ContainsKey(pair))
            {
                RoadMap.Graph.Divide(src.Edge.Destination, dst.Edge.Source);
                _lineLinks.Remove(pair);
                src.OnStreakAdd -= Link_OnStreakAdd;
                src.OnStreakRemove -= Link_OnStreakRemove;
                dst.OnStreakAdd -= Link_OnStreakAdd;
                dst.OnStreakRemove -= Link_OnStreakRemove;
            }
        }
        /*
        public void Link(Streak src, Streak dst)
        {
            LinePair pair = new LinePair(src.Line, dst.Line);
            if (_lineLinks.ContainsKey(pair))
            {
                _lineLinks[pair].Link(src, dst);
                RoadChanged(null, null);
            }
            else
                throw new ApplicationException("Lines not linked yet");
        }
        public void Unlink(Streak src, Streak dst)
        {
            LinePair pair = new LinePair(src.Line, dst.Line);
            if (_lineLinks.ContainsKey(pair))
            {
                _lineLinks[pair].Unlink(src, dst);
                RoadChanged(null, null);
            }
        }
        public void UnlinkAll()
        {
            while (_lineLinks.Count > 0)
            {
                Edge e = _lineLinks.ElementAt(0).Value.Edge;
                RoadMap.Graph.Divide(e.Source, e.Destination);
                _lineLinks.Remove(_lineLinks.ElementAt(0).Key);
            }
        }
        */
    }
    // TODO: Добавить Enumerator для Links чтобы можно было смотреть связи по индексу
}