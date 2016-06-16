using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrafficOptimizer.RoadMap.Model
{
    public class SectionLink : VehicleContainer
    {
        /// <summary>
        /// Коэффициент на перемещение к пути по отношению к движению по прямой
        /// Например для поворота направо - 0.75, а для поворота на дорогу слева - 1.25
        /// </summary>
        public Dictionary<VehicleContainer, float> _moveRatio = new Dictionary<VehicleContainer, float>();
        public SectionLink(VehicleContainer dst, float moveRatio) 
            : base(new VehicleContainer[] { dst })
        {
            _moveRatio.Add(dst, moveRatio);
        }

        public float MoveRatio(VehicleContainer dst)
        {
            if (_moveRatio.ContainsKey(dst))
            {
                return _moveRatio[dst];
            }
            throw new InvalidOperationException("Нет такого пути");
        }

        public void AddDestination(VehicleContainer dst, float ratio)
        {
            if (!_destinations.Contains(dst))
            {
                _destinations.Add(dst);
                _moveRatio.Add(dst, ratio);
            }
        }
        public new void RemoveDestination(VehicleContainer dst)
        {
            _destinations.Remove(dst);
            _moveRatio.Remove(dst);
        }
        [Obsolete("Use another method", true)]
        public new void AddDestination(VehicleContainer dst)
        {
            throw new InvalidOperationException("Вызов этого метода для данного класса запрещен");
        }
    }
}
