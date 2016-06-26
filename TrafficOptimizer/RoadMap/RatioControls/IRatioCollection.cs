using System;
using System.Collections.Generic;

namespace TrafficOptimizer.RoadMap.RatioControls
{
    using Model.Vehicles;

    public interface IRatioCollection
    {
        /// <summary>
        /// Получает коэффициенты для дороги до указанного момента времени (времени моделирования)
        /// </summary>
        /// <param name="moment">Время моделирования, на которое необходимо получить коэффициенты</param>
        /// <returns></returns>
        Dictionary<VehicleContainer, float> GetRatioCollection(List<VehicleContainer> subjects, TimeSpan? simulationMoment = null);
    }
}
