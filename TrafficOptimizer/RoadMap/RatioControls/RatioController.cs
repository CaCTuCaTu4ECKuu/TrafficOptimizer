using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrafficOptimizer.RoadMap.Model;

namespace TrafficOptimizer.RoadMap.RatioControls
{
    public abstract class RatioController : IRatioController
    {
        private RoadMap RoadMap
        {
            get;
            set;
        }

        private Dictionary<VehicleContainer, float> _average;
        private List<RatioHistorySlot> _history;

        private int _space = 100;
        /// <summary>
        /// Размер выборки для подсчёта коффициента.
        /// Указывает максимальное количество последний результатов, которые учитываются при подсчёте коэффициента.
        /// Минимум 10
        /// </summary>
        public int Space
        {
            get { return _space; }
            set
            {
                if (value >= 10)
                    _space = value;
            }
        }
        private int _influence = 1;
        /// <summary>
        /// Количество последних результатов, начиная с которого данные используются при рассчёте коффициета
        /// Если результатов было меньше этого количество, коффициент будет равен 1.
        /// (Мимимальное значение - 1, максимальное - значение поля Space)
        /// </summary>
        public int Influence
        {
            get { return _influence; }
            set
            {
                if (value < _space && value > 1)
                {
                    _influence = value;
                }
            }
        }
        private TimeSpan _timeInfluence = new TimeSpan(0, 30, 0);
        /// <summary>
        /// Отрезок времени, на протяжении которого данные актуальны
        /// </summary>
        public TimeSpan TimeInfluence
        {
            get { return _timeInfluence; }
            set
            {
                _timeInfluence = value;
            }
        }
        private int _collectionSize = 100000;
        /// <summary>
        /// Размер коллекции, которая хранит последний результаты
        /// (Может влиять на быстродейтсвие и потребление памяти)
        /// </summary>
        public int CollectionSize
        {
            get { return _collectionSize; }
            set
            {
                if (value < _collectionSize)
                {
                    int diff = _collectionSize - value;
                    _history.RemoveRange(_collectionSize - diff, diff);
                }
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
        public float CollectionInfluenceRatio
        {
            get;
            set;
        }

        public RatioController(RoadMap map, int space)
        {
            RoadMap = map;
            _average = new Dictionary<VehicleContainer, float>();
            _history = new List<RatioHistorySlot>();

            AllowNegativeRatio = false;
            CollectionInfluenceRatio = 1f;
        }

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

        public Dictionary<VehicleContainer, float> GetGlobalRatio(DateTime moment, List<VehicleContainer> subjects)
        {
            Dictionary<VehicleContainer, float> res = new Dictionary<VehicleContainer, float>();
            foreach (var c in subjects)
            {
                if (_average.ContainsKey(c))
                {
                    DateTime from = moment - TimeInfluence;
                    // Выборка области не более размера space в промежутке между указанным моментом и моментом за timeInfluence времени до него
                    var scope = _history.Where(s => s.Arrive >= from && s.Arrive <= moment).Take(_space);
                    if (scope.Count() >= _influence)
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

        public float GetAverage(VehicleContainer container)
        {
            return _average[container];
        }
    }
}
