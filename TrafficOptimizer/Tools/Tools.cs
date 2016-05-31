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
    }
}
