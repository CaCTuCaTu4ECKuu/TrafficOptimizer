using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrafficOptimizer.RoadMap.Model;

namespace TrafficOptimizer.RoadMap.RatioControls
{
    using Model.Vehicles;
    using Graph;
    using Graph.Model;

    public class RatioCollection : IRatioCollection, IRatioProvider
    {
        private class RatioPair
        {
            private float _ratioSum = 0;
            private bool dropped = false;
            private int _samples = 0;
            public float RatioSum
            {
                get { return _ratioSum; }
                set
                {
                    if (value >= float.MaxValue)
                    {
                        dropped = true;
                        _ratioSum /= 2;
                        _samples /= 2;
                    }
                    else
                        _ratioSum = value;
                }
            }
            public int SampleSize
            {
                get { return _samples; }
                set
                {
                    if (dropped)
                        dropped = false;
                    else
                        _samples = value;
                }
            }
            public float Average
            {
                get { return RatioSum / SampleSize; }
            }
            public RatioPair(float ratio, int samples)
            {
                RatioSum = ratio;
                SampleSize = samples;
            }
        }

        private RoadMap RoadMap
        {
            get;
            set;
        }

        private Dictionary<VehicleContainer, RatioPair> _average;
        private List<RatioHistorySlot> _history;

        private int _collectionSize = 100000;
        private int _sampleSize;
        private int _influenceAmount = 1;
        private TimeSpan _influenceTime = new TimeSpan(0, 30, 0);

        /// <summary>
        /// Размер коллекции, которая хранит последний результаты
        /// (Может влиять на быстродейтсвие и потребление памяти)
        /// Указание меньшего значение приведет к потере части истории значений
        /// </summary>
        public int CollectionSize
        {
            get { return _collectionSize; }
            set
            {
                if (value < _collectionSize)
                {
                    int diff = _collectionSize - value;
                    _collectionSize = value;
                    _history.RemoveRange(_collectionSize - diff, diff);
                }
            }
        }
        /// <summary>
        /// Размер выборки для подсчёта коэффициента.
        /// Указывает максимальное количество последний результатов, которые учитываются при подсчёте коэффициента.
        /// Минимум 10
        /// </summary>
        public int SampleSize
        {
            get { return _sampleSize; }
            set
            {
                if (value >= 10)
                    _sampleSize = value;
            }
        }
        /// <summary>
        /// Количество последних результатов, начиная с которого данные используются при рассчёте коффициета
        /// Если результатов было меньше этого количество, коффициент будет равен 1.
        /// (Мимимальное значение - 1, максимальное - значение поля Space)
        /// </summary>
        public int InfluenceAmount
        {
            get { return _influenceAmount; }
            set
            {
                if (value < _sampleSize && value > 1)
                {
                    _influenceAmount = value;
                }
            }
        }
        /// <summary>
        /// Отрезок времени, на протяжении которого данные используются для рассчёта коэффициента
        /// </summary>
        public TimeSpan InfluenceTime
        {
            get { return _influenceTime; }
            set
            {
                _influenceTime = value;
            }
        }

        /// <summary>
        /// Позволяет использовать отрицательные весовые значения в результирующем словаре
        /// Иначе минимальный коэффициент будет устанавливается как 1
        /// </summary>
        public bool AllowNegativeRatio
        {
            get;
            set;
        }
        /// <summary>
        /// Коэффициент влияния результата на рассчёт
        /// (Будет приближать значения коллекции к 1 на этот коэффициент)
        /// </summary>
        public float CollectionInfluenceRatio
        {
            get;
            set;
        }

        /// <summary>
        /// Коллекция коэффициентов для карты
        /// </summary>
        /// <param name="map">Карта, для которой указаны коэффициенты</param>
        /// <param name="sampleSize">Размер выборки для подсчёта коэффициентов</param>
        public RatioCollection(RoadMap map, int sampleSize = 100)
        {
            RoadMap = map;
            _average = new Dictionary<VehicleContainer, RatioPair>();
            _history = new List<RatioHistorySlot>();
            _sampleSize = sampleSize;

            AllowNegativeRatio = false;
            CollectionInfluenceRatio = 1f;
        }

        /// <summary>
        /// Добавить новое значение коэффициента для участка на определенный момент
        /// </summary>
        /// <param name="container">Контейнер, для которого записывается значение</param>
        /// <param name="ratio">Значение коэффициента</param>
        /// <param name="simulationMoment">Момент времени</param>
        public void UpdateRatio(VehicleContainer container, float ratio, TimeSpan simulationMoment)
        {
            if (!_average.ContainsKey(container))
                _average.Add(container, new RatioPair(ratio, 1));
            else
            {
                _average[container].RatioSum += ratio;
                _average[container].SampleSize++;
            }
            _history.Insert(0, new RatioHistorySlot(container, ratio, simulationMoment));

            if (_history.Count > _collectionSize)
                _history.RemoveAt(_history.Count - 1);
        }

        private float calcInfluence(float ratio)
        {
            float affectedValue = 1.0f - ratio;
            if (ratio > 1.0f)
                return 1.0f - (affectedValue * CollectionInfluenceRatio);
            else
                return 1.0f + (affectedValue * CollectionInfluenceRatio);
        }
        public float GetRatio(VehicleContainer container, TimeSpan moment)
        {
            float currentRatio = 1.0f;
            if (_average.ContainsKey(container))
            {
                TimeSpan from = moment - InfluenceTime;
                // Выборка области не более размера space в промежутке между указанным моментом и моментом за timeInfluence времени до него
                var scope = _history.Where(s => s.SimulationMoment >= from && s.SimulationMoment <= moment).Take(_sampleSize);
                if (scope.Count() >= _influenceAmount)
                {
                    float scopeAverage = 0;
                    foreach (var e in scope)
                        scopeAverage += e.Value;
                    scopeAverage = scopeAverage / scope.Count();

                    currentRatio = calcInfluence(scopeAverage / _average[container].Average);
                }
                else
                    currentRatio = calcInfluence(_average[container].Average);

                if (currentRatio < 1.0f && !AllowNegativeRatio)
                    currentRatio = 1.0f;
            }
            return currentRatio;
        }
        /// <summary>
        /// Получить набор коэффициентов
        /// </summary>
        /// <param name="moment">Момент времени на который нужны коэффициенты</param>
        /// <param name="subjects">Список контейнеров, для которых нужны коэффициенты</param>
        /// <returns></returns>
        public Dictionary<VehicleContainer, float> GetRatioCollection(List<VehicleContainer> subjects, TimeSpan? moment = null)
        {
            if (subjects == null || subjects.Count == 0)
                throw new ArgumentException("В subjects должны быть указаны контейнеры, для которых нужно получить коэффициенты");

            Dictionary<VehicleContainer, float> res = new Dictionary<VehicleContainer, float>();
            foreach (var c in subjects)
            {
                res.Add(c, GetRatio(c, moment.HasValue ? moment.Value : RoadMap.SimulationTime));
            }
            return res;
        }

        /// <summary>
        /// Получить среднее значение коэффициента для контейнера
        /// </summary>
        /// <param name="container">Контейнер, для которого нужно получить коэффициент</param>
        /// <returns></returns>
        public float GetAverage(VehicleContainer container)
        {
            return _average[container].Average;
        }
        /// <summary>
        /// Задает значение среднего коэффициента для контейнера
        /// </summary>
        /// <param name="container">Контейнер</param>
        /// <param name="ratioSum">Сумма коэффициентов</param>
        /// <param name="ratioSum">Количество измерений коэффициентов</param>
        public void SetAverage(VehicleContainer container, float ratioSum, int samples)
        {
            if (!_average.ContainsKey(container))
                _average.Add(container, new RatioPair(ratioSum, samples));
            else
            {
                if (_average[container].RatioSum + ratioSum < float.MaxValue)
                {
                    _average[container].RatioSum += ratioSum;
                    _average[container].SampleSize += samples;
                }
                else
                {
                    // Если не рассчитать отлельно количество не засчитается (после сброса первое обновление размера не засчитывается)
                    float newAvg = (_average[container].RatioSum / 2) + (ratioSum / 2);
                    int newSize = (_average[container].SampleSize / 2) + (samples / 2);
                    SetAverage(container, newAvg, newSize);
                }
            }
        }

        public float GetRatio(Edge edge)
        {
            // TODO : MAKE IT RIGHT
            foreach (var e in _average)
            {
                var etype = e.GetType();
                if (etype == typeof(Streak))
                {
                    if (((Streak)e.Key).Line.Edge == edge)
                        return GetRatio(((Streak)e.Key).Line.Edge);
                }
                if (etype == typeof(SectionLink))
                {
                    if (((SectionLink)e.Key).Edge == edge)
                        return GetRatio(((SectionLink)e.Key).Edge);
                }
            }
            return 1.0f;
        }

        /// <summary>
        /// Ячейка, которая хранит информацию о полученном коффициенте, который рассчитывает после окончания движения
        /// </summary>
        private struct RatioHistorySlot
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
            public TimeSpan SimulationMoment
            {
                get;
                private set;
            }

            /// <summary>
            /// Создает ячейку с данными о коэффициенте для указанного контейнера, полученную в заданный момент времени
            /// </summary>
            /// <param name="container">Контейнер</param>
            /// <param name="value">Значение коэффициента</param>
            /// <param name="moment">Время получения коэффициента</param>
            public RatioHistorySlot(VehicleContainer container, float value, TimeSpan moment)
            {
                Container = container;
                Value = value;
                SimulationMoment = moment;
            }
        }
    }
}
