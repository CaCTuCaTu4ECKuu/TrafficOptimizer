using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Diagnostics;

namespace TrafficOptimizer.RoadMap.Model
{
    using Graph.Model;
    using Tools;

    [DebuggerDisplay("[{Edge.Source.ID}]-[{Edge.Destination.ID}] {LineLeftStart} - {LineLeftEnd}")]
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
                if (Streaks.Count > 0)
                {
                    return (Streaks[Streaks.Count - 1]).BorderRightStart;
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
                if (Streaks.Count > 0)
                {
                    return (Streaks[Streaks.Count - 1]).BorderRightEnd;
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
            Streaks = new List<Streak>();

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

            if (Streaks.Count > 0)
            {
                PointF oldStart = newStart;
                PointF stStart = Tools.HeightPoint(Parameters.StreakHalf, false, newStart, newEnd);
                PointF stEnd = Tools.HeightPoint(Parameters.StreakHalf, true, newEnd, oldStart);
                Streaks[0].Move(stStart, stEnd);

                for (int i = 1; i < Streaks.Count; i++)
                {
                    oldStart = stStart;
                    stStart = Tools.HeightPoint(Parameters.StreakSize, false, stStart, stEnd);
                    stEnd = Tools.HeightPoint(Parameters.StreakSize, true, stEnd, oldStart);

                    Streaks[i].Move(stStart, stEnd);
                }
            }
        }
        public void ChangeStreaks(int newCount)
        {
            if (newCount != Streaks.Count)
            {
                if (newCount < Streaks.Count)
                {
                    for (int i = Streaks.Count - 1; i >= newCount; i--)
                    {
                        Streaks.RemoveAt(i);
                    }
                }
                else
                {
                    // Добавляем полосы
                    int i = newCount - Streaks.Count;
                    while (i-- > 0)
                    {
                        Streaks.Insert(Streaks.Count, new Streak(this, new PointF(), new PointF()));
                    }
                }
                // Обновляем положение всех полос (плевать, это нечасто)
                Move(LineLeftStart, LineLeftEnd);
            }
        }
    }
}
