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
        private List<Route> _history;
        private List<VehicleContainer> _path;
        public IEnumerable<VehicleContainer> Path
        {
            get { return _path.AsReadOnly(); }
        }
        public IEnumerable<Route> History
        {
            get { return _history != null ? _history.AsReadOnly() : null; }
        }

        /// <summary>
        /// Возвращает контейнер, который идет после заданного в маршруте.
        /// Если такого контейнера нет в маршруте, возвращает null
        /// </summary>
        /// <param name="target">Контейнер, по отношений к которому нужно найти следующий пункт назначения</param>
        /// <returns></returns>
        public VehicleContainer DestinationAfter(VehicleContainer target)
        {
            if (target == Destination)
            {
                return Destination;
            }
            else if (_path.Contains(target))
            {
                return _path[_path.IndexOf(target) + 1];
            }
            else
                return null;
        }

        public Route(Section destination)
        {
            Destination = destination;
        }
    }
}
