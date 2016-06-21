using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace TrafficOptimizer.RoadMap.Model
{
    using Vehicles;

    [DebuggerDisplay("[{ID}] In: {InRoads.Count()} Out: {OutRoads.Count()}")]
    public class Section : VehicleContainer
    {
        public RoadMap RoadMap
        {
            get;
            private set;
        }
        private List<Road> _relatedRoads;
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
                return _relatedRoads.Where(r => r.SlaveLine.Source == this);
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

        private Dictionary<StreaksLink, VehicleContainer> _links 
            = new Dictionary<StreaksLink, VehicleContainer>();

        /// <summary>
        /// Можно ли проехать к месту назначения из источника через этот узел
        /// </summary>
        /// <param name="src">Начало</param>
        /// <param name="dst">Место назначения</param>
        public override bool AllowToMove(VehicleContainer src,  VehicleContainer dst)
        {
            // Только если указаны 2 streak-а нужно проверять есть ли между ними связь
            bool linked = true;
            if (src.GetType() == typeof(Streak) && dst.GetType() == typeof(Streak))
                return _links.ContainsKey(new StreaksLink((Streak)src, (Streak)dst));
            return base.AllowToMove(src, dst) && linked;
        }

        public Section(RoadMap map, IEnumerable<Road> relatedRoads)
            : base((VehicleContainer)null)
        {
            RoadMap = map;
            _relatedRoads = new List<Road>();
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
        }
        public void AddRoad(Road road)
        {
            if (!_relatedRoads.Contains(road))
            {
                _relatedRoads.Add(road);
                road.PrimaryLine.OnStreakAdd += RoadChanged;
                road.PrimaryLine.OnStreakRemove += RoadChanged;
                road.SlaveLine.OnStreakAdd += RoadChanged;
                road.SlaveLine.OnStreakRemove += RoadChanged;
            }
        }
        public void RemoveRoad(Road road)
        {
            if (_relatedRoads.Contains(road))
            {
                _relatedRoads.Remove(road);
                road.PrimaryLine.OnStreakAdd -= RoadChanged;
                road.PrimaryLine.OnStreakRemove -= RoadChanged;
                road.SlaveLine.OnStreakAdd -= RoadChanged;
                road.SlaveLine.OnStreakRemove -= RoadChanged;
            }
        }

        public void Link(Streak src, Streak dst, float length = 0, float moveRatio = 1f)
        {
            if (Destinations.Contains(dst))
            {
                if (src.Destinations.Contains(this) && this.Destinations.Contains(dst))
                {
                    StreaksLink link = new StreaksLink(src, dst);
                    if (!_links.ContainsKey(link))
                        _links.Add(link, new SectionLink(dst, moveRatio, length));
                    RoadChanged(null, null);
                }
                else
                    throw new ApplicationException("Одна из дорог не связана с этой секцией");
            }
        }
        public void Unlink(Streak src, Streak dst)
        {
            StreaksLink link = new StreaksLink(src, dst);
            _links.Remove(link);
            RoadChanged(null, null);
        }
    }
    // TODO: Добавить Enumerator для Links чтобы можно было смотреть связи по индексу
}