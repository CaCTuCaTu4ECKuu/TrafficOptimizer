using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrafficOptimizer.RoadMap.RatioControls
{
    using Model;

    public interface IRatioController
    {
        /// <summary>
        /// Получает коэффициенты для дороги до указанного момента времени (времени моделирования)
        /// </summary>
        /// <param name="moment">Модельное время</param>
        /// <returns></returns>
        Dictionary<VehicleContainer, float> GetGlobalRatio(DateTime moment, List<VehicleContainer> subjects);
    }
}
