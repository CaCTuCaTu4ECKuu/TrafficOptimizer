using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrafficOptimizer.Tools
{
    public static class Tools
    {
        /// <summary>
        /// Расстояние между двумя точками
        /// </summary>
        /// <param name="p1">Первая точка</param>
        /// <param name="p2">Вторая точка</param>
        /// <returns></returns>
        public static float Distance(PointF p1, PointF p2)
        {
            return (float)Math.Sqrt(Math.Pow((p2.X - p1.X), 2) + Math.Pow((p2.Y - p1.Y), 2));
        }
        /// <summary>
        /// Находит точку на указанном расстоянии от прямой и под прямым углом к стартовой точке
        /// </summary>
        /// <param name="height">Расстояние от точки до прямой</param>
        /// <param name="side">С какой стороны искать точку (true - справа)</param>
        /// <param name="SrcPoint">Точка, по отношению к которой угол будет прямым</param>
        /// <param name="DstPoint">Точка, относительной котрой рассчитывается положение</param>
        /// <returns></returns>
        public static PointF HeightPoint(float height, bool side, PointF SrcPoint, PointF DstPoint)
        {
            int sign = side ? 1 : -1;
            var d = Distance(SrcPoint, DstPoint);
            float x = SrcPoint.X - sign * (height * (DstPoint.Y - SrcPoint.Y) / d);
            float y = SrcPoint.Y + sign * (height * (DstPoint.X - SrcPoint.X) / d);
            return new PointF(x, y);
        }
        /// <summary>
        /// Находит точку, находящуюся между двумя точками на указанном расстоянии от начала
        /// </summary>
        /// <param name="src">Начальная точка</param>
        /// <param name="dst">Конечная точка</param>
        /// <param name="distance">Расстояние от первой точки</param>
        public static PointF PointBetween(PointF src, PointF dst, float distance)
        {
            float dist = Distance(src, dst) - distance;
            float ratio = distance / dist;
            float x = (src.X + (ratio * dst.X)) / (1 + ratio);
            float y = (src.Y + (ratio * dst.Y)) / (1 + ratio);
            return new PointF(x, y);
        }

        #region Collision Detection
        public static bool IsPointInside(PointF point, PointF center, float radius)
        {
            var dx = point.X - center.X;
            var dy = point.Y - center.Y;
            var dist = Math.Sqrt(dx*dx + dy*dy);
            return dist < radius;
        }
        public static bool IsLineIntersect(PointF A1, PointF A2, PointF B1, PointF B2)
        {
            var denom = ((B2.Y - B1.Y) * (A2.X - A1.X)) -
                ((B2.X - B1.X) * (A2.Y - A1.Y));
            return denom != 0;
        }
        public static bool IsPointInside(PointF point, PointF[] block)
        {
            if (block.Length == 4)
            {
                float dist = Distance(block[1], block[3]);
                PointF center = PointBetween(block[1], block[3], dist / 2);

                for (int i = 0; i < 3; i++)
                {
                    if (IsLineIntersect(center, point, block[i], block[i + 1]))
                        return false;
                }
                if (IsLineIntersect(center, point, block[3], block[0]))
                    return false;

                return true;
            }
            else
                throw new FormatException("block must have 4 vertexes, it's a rectangle, sort of");
        }
        public static bool IsRoundsIntersect(PointF center1, float radius1, PointF center2, float radius2)
        {
            var dx = center1.X - center2.X;
            var dy = center1.Y - center2.Y;
            var dist = Math.Sqrt(dx * dx + dy * dy);
            return dist < radius1 + radius2;
        }
        #endregion
    }
}