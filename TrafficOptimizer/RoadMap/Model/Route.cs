using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrafficOptimizer.RoadMap.Model
{
    using Model.Vehicles;

    public class Route
    {
        public Section Destination
        {
            get;
            private set;
        }
        private List<Section> _path = null;
        public IEnumerable<VehicleContainer> Path
        {
            get { return _path.AsReadOnly(); }
        }
        public float Length
        {
            get
            {
                float res = float.PositiveInfinity;
                if (IsRouteSolid())
                {
                    res = 0;
                    for (int c = 0; c < _path.Count - 2; c++)
                    {
                        res += _path[c].LengthTo(_path[c + 1]);
                        if (res >= float.PositiveInfinity)
                            break;
                    }
                }
                return res;
            }
        }

        public bool IsRouteSolid()
        {
            if (_path == null)
                return false;

            for (int c = 0; c < _path.Count - 2; c++)
            {
                if (!_path[c].Destinations.Contains(_path[c + 1]))
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Возвращает контейнер, который идет после заданного в маршруте.
        /// Если такого контейнера нет в маршруте, возвращает null
        /// </summary>
        /// <param name="target">Контейнер, по отношений к которому нужно найти следующий пункт назначения</param>
        /// <returns></returns>
        public Section DestinationAfter(VehicleContainer target)
        {
            if (_path == null || target == null)
                return null;

            var ttype = target.GetType();
            Section tSection = null;
            if (ttype == typeof(Section))
            {
                tSection = (Section)target;
            }
            else if (ttype == typeof(Streak))
            {
                tSection = ((Streak)target).Line.Destination;
            }
            else if (ttype == typeof(SectionLink))
            {
                tSection = ((Streak)target.Destinations.FirstOrDefault()).Line.Destination;
            }

            if (target == Destination)
            {
                return Destination;
            }
            else if (_path.Contains(target))
            {
                // Всегда как минимум назначение и источник, поэтому всегда больше 1, 
                // а если нет то target == destination или нет такого элемента вообще
                return _path[_path.IndexOf(tSection) + 1];
            }
            else
                return null;
        }

        public Route(Section destination)
        {
            Destination = destination;
        }
        /// <summary>
        /// Строит или перестраивает маршрут от до указанной секции
        /// </summary>
        /// <param name="source"></param>
        public void BuildRoute(Section source)
        {
            var map = source.RoadMap;
            var path = map.Graph.FindPath(map.GetNode(source), map.GetNode(Destination));
            if (path.IsSolid)
            {
                _path = new List<Section>();
                _path.Add(source);
                foreach (var e in path)
                {
                    _path.Add(map.GetSection(e.Destination));
                }
            }
            else
                throw new ApplicationException("Нет цельного пути, секции не связанны");
        }
    }
}
