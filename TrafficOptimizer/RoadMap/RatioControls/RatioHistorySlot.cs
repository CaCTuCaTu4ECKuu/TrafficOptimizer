using System;

namespace TrafficOptimizer.RoadMap.RatioControls
{
    using Model;
    using Model.Vehicles;

    /// <summary>
    /// Ячейка, которая хранит информацию о полученном коффициенте, который рассчитывает после окончания движения
    /// </summary>
    public struct RatioHistorySlot
    {
        /// <summary>
        /// Контейнер, для которого хранится коэффициент
        /// </summary>
        public VehicleContainer Container
        {
            get;
            private set;
        }
        /// <summary>
        /// Значение коэффициента для этого объекта
        /// </summary>
        public float Value
        {
            get;
            private set;
        }
        /// <summary>
        /// Время записи значения
        /// </summary>
        public DateTime Arrive
        {
            get;
            private set;
        }

        /// <summary>
        /// Создает ячейку с данными о коэффициенте для указанного контейнера, полученную в заданный момент времени
        /// </summary>
        /// <param name="container">Контейнер</param>
        /// <param name="value">Значение коэффициента</param>
        /// <param name="arrive">Время получения коэффициента</param>
        public RatioHistorySlot(VehicleContainer container, float value, DateTime arrive)
        {
            Container = container;
            Value = value;
            Arrive = arrive;
        }
    }
}
