using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Diagnostics;

namespace TrafficOptimizer.RoadMap2D.Model
{
    using Graph.Model;
    using Tools;

    [DebuggerDisplay("{LineLeftStart} - {LineLeftEnd}")]
    public class Line2D
    {
        public List<Streak2D> Streaks
        {
            get;
            private set;
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
        public Line2D(PointF src, PointF dst, int streaks, float streakWidth)
        {
            float streakHalf = streakWidth / 2;
            LineLeftStart = src;
            LineLeftEnd = dst;

            Streaks = new List<Streak2D>();

            if (streaks > 0)
            {
                PointF oldStart = src;
                PointF stStart = Tools.HeightPoint(streakHalf, false, src, dst);
                PointF stEnd = Tools.HeightPoint(streakHalf, true, dst, oldStart);
                Streaks.Add(new Streak2D(stStart, stEnd, streakHalf));

                for (int i = 1; i < streaks; i++)
                {
                    oldStart = stStart;
                    stStart = Tools.HeightPoint(streakWidth, false, stStart, stEnd);
                    stEnd = Tools.HeightPoint(streakWidth, true, stEnd, oldStart);

                    Streaks.Add(new Streak2D(stStart, stEnd, streakHalf));
                }
            }
        }
    }
}
