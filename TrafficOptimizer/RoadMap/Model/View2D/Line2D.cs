using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace TrafficOptimizer.RoadMap.Model
{
    using Graph.Model;
    using Tools;

    public partial class Line
    {
        public RoadMapParameters Parameters
        {
            get
            {
                return Road.RoadMap.RoadMapParametrs;
            }
        }

        /// <summary>
        /// Начало стороны, справа от которой располагаются все полосы
        /// </summary>
        public PointF LineLeftStart
        {
            get;
            private set;
        }
        /// <summary>
        /// Конец полосы, справа от которой располагаются все полосы
        /// </summary>
        public PointF LineLeftEnd
        {
            get;
            private set;
        }
        /// <summary>
        /// Начало границы бордюра (совпадает с левой стороной, если полос нет)
        /// </summary>
        public PointF LineRightStart
        {
            get
            {
                if (Streaks > 0)
                {
                    return (_streaks[_streaks.Count - 1]).BorderRightStart;
                }
                return LineLeftStart;
            }
        }
        /// <summary>
        /// Конец границы бордюра (совпадает с левой стороной, если полос нет)
        /// </summary>
        public PointF LineRightEnd
        {
            get
            {
                if (Streaks > 0)
                {
                    return (_streaks[_streaks.Count - 1]).BorderRightEnd;
                }
                return LineLeftEnd;
            }
        }

        /// <summary>
        /// Создает новую сторону движения
        /// </summary>
        /// <param name="road">Дорога, на которой располагается сторона</param>
        /// <param name="edge">Грань, которую реализует сторона</param>
        /// <param name="streaks">Количество полос движения</param>
        /// <param name="src">Начало центра дороги</param>
        /// <param name="dst">Конец центра дороги</param>
        public Line(Road road, Edge edge, int streaks, PointF src, PointF dst)
        {
            Road = road;
            Edge = edge;

            ChangeStreaks(streaks);
            Move(src, dst);
        }

        /// <summary>
        /// Изменяет расположение линии и полос в координатной плоскости
        /// </summary>
        /// <param name="newStart">Начало центра дороги, относительно которого идет полоса</param>
        /// <param name="newEnd">Конец центра дороги, относительно которого идет полоса</param>
        public void Move(PointF newStart, PointF newEnd)
        {
            LineLeftStart = newStart;
            LineLeftEnd = newEnd;

            if (Streaks > 0)
            {
                PointF oldStart = newStart;
                PointF stStart = Tools.HeightPoint(Parameters.StreakHalf, true, newStart, newEnd);
                PointF stEnd = Tools.HeightPoint(Parameters.StreakHalf, false, newEnd, oldStart);
                (_streaks[0]).Move(stStart, stEnd);

                for (int i = 1; i < _streaks.Count; i++)
                {
                    oldStart = stStart;
                    stStart = Tools.HeightPoint(Parameters.StreakHalf, true, stStart, stEnd);
                    stEnd = Tools.HeightPoint(Parameters.StreakHalf, false, stEnd, oldStart);

                    (_streaks[i]).Move(stStart, stEnd);
                }
            }
        }
        public void ChangeStreaks(int newCount)
        {
            if (newCount != Streaks)
            {
                if (newCount < _streaks.Count)
                {
                    for (int i = _streaks.Count - 1; i >= newCount; i--)
                    {
                        _streaks.RemoveAt(i);
                    }
                }
                else
                {
                    // Добавляем полосы
                    int i = newCount - _streaks.Count;
                    while (i-- > 0)
                    {
                        _streaks.Insert(_streaks.Count, new Streak(this, new PointF(), new PointF()));
                    }
                }
                // Обновляем положение всех полос (плевать, это нечасто)
                Move(LineLeftStart, LineLeftEnd);
            }
        }
    }
}
