using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrafficOptimizer.RoadMap.Model;

namespace TrafficOptimizer.RoadMap.RatioControls
{
    using Model.Vehicles;

    public class RatioCollection : IRatioCollection
    {
        private RoadMap RoadMap
        {
            get;
            set;
        }

        private Dictionary<VehicleContainer, float> _average;
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
            _average = new Dictionary<VehicleContainer, float>();
            _history = new List<RatioHistorySlot>();
            _sampleSize = sampleSize;

            AllowNegativeRatio = false;
            CollectionInfluenceRatio = 1f;
        }

        /// <summary>
        /// Добавить новое значение коэффициента для участка на определенный момент
        /// </summary>
        /// <param name="container">Контейнер, для которого записывается значение</param>
        /// <param name="value">Значение коэффициента</param>
        /// <param name="moment">Момент времени</param>
        public void UpdateFactor(VehicleContainer container, float value, DateTime moment)
        {
            if (!_average.ContainsKey(container))
                _average.Add(container, value);
            else
                _average[container] = (_average[container] + value) / 2;
            _history.Insert(0, new RatioHistorySlot(container, value, moment));

            if (_history.Count > _collectionSize)
                _history.RemoveAt(_history.Count - 1);
        }

        /// <summary>
        /// Получить набор коэффициентов
        /// </summary>
        /// <param name="moment">Момент времени на который нужны коэффициенты</param>
        /// <param name="subjects">Список контейнеров, для которых нужны коэффициенты</param>
        /// <returns></returns>
        public Dictionary<VehicleContainer, float> GetRatioCollection(DateTime moment, List<VehicleContainer> subjects)
        {
            if (subjects == null || subjects.Count == 0)
                throw new ArgumentException("В subjects должны быть указаны контейнеры, для которых нужно получить коэффициенты");

            Dictionary<VehicleContainer, float> res = new Dictionary<VehicleContainer, float>();
            foreach (var c in subjects)
            {
                if (_average.ContainsKey(c))
                {
                    DateTime from = moment - InfluenceTime;
                    // Выборка области не более размера space в промежутке между указанным моментом и моментом за timeInfluence времени до него
                    var scope = _history.Where(s => s.Moment >= from && s.Moment <= moment).Take(_sampleSize);
                    if (scope.Count() >= _influenceAmount)
                    {
                        float scopeAverage = 0;
                        foreach (var e in scope)
                            scopeAverage += e.Value;
                        scopeAverage = scopeAverage / scope.Count();

                        float currentRatio = scopeAverage / _average[c];
                        // TODO: Учесть коффициент влияния на коллекцию правильно
                        if (currentRatio < 1f)
                        {
                            if (AllowNegativeRatio)
                                res.Add(c, currentRatio);
                            else
                                res.Add(c, 1f);
                        }
                        else
                            res.Add(c, currentRatio);
                    }
                    else
                        res.Add(c, 1f);
                }
                else
                    res.Add(c, 1f);
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
            return _average[container];
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
            public DateTime Moment
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
            public RatioHistorySlot(VehicleContainer container, float value, DateTime moment)
            {
                Container = container;
                Value = value;
                Moment = moment;
            }
        }
    }
}
