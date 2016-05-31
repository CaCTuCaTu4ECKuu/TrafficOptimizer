using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace TrafficOptimizer.RoadMap.Model
{
    using Tools;

    public partial class Streak
    {
        public RoadMapParameters Parameters
        {
            get
            {
                return Line.Road.RoadMap.RoadMapParametrs;
            }
        }

        /// <summary>
        /// Начало средины полосы
        /// </summary>
        public PointF MiddleStart
        {
            get;
            private set;
        }
        /// <summary>
        /// Конец средины полосы
        /// </summary>
        public PointF MiddleEnd
        {
            get;
            private set;
        }
        /// <summary>
        /// Начало правой границы полосы
        /// </summary>
        public PointF BorderRightStart
        {
            get;
            private set;
        }
        /// <summary>
        /// Конец правой границы полосы
        /// </summary>
        public PointF BorderRightEnd
        {
            get;
            private set;
        }
        /// <summary>
        /// Начало левой границы полосы
        /// </summary>
        public PointF BorderLeftStart
        {
            get;
            private set;
        }
        /// <summary>
        /// Конец левой границы полосы
        /// </summary>
        public PointF BorderLeftEnd
        {
            get;
            private set;
        }

        /// <summary>
        /// Создает новую полосу движения и задает ее расположение на плоскости
        /// </summary>
        /// <param name="line">Сторона движения, частью которой является эта полоса</param>
        /// <param name="midStart">Точка начала центра полосы</param>
        /// <param name="midEnd">Точка конца центра полосы</param>
        public Streak(Line line, PointF midStart, PointF midEnd)
        {
            Line = line;

            Move(midStart, midEnd);
        }

        /// <summary>
        /// Сдвигает расположение полосы в координатной плоскости
        /// </summary>
        /// <param name="newStart">Точка начало нового центра полосы</param>
        /// <param name="newEnd">Точка окончания нового центра полосы</param>
        public void Move(PointF newStart, PointF newEnd)
        {
            MiddleStart = newStart;
            MiddleEnd = newEnd;

            // Right Border
            BorderRightStart = Tools.HeightPoint(Parameters.StreakHalf, true, MiddleStart, MiddleEnd);
            BorderRightEnd = Tools.HeightPoint(Parameters.StreakHalf, false, MiddleStart, MiddleEnd);
            // Left Border
            BorderLeftStart = Tools.HeightPoint(Parameters.StreakHalf, false, MiddleStart, MiddleEnd);
            BorderLeftEnd = Tools.HeightPoint(Parameters.StreakHalf, true, MiddleStart, MiddleEnd);
        }
    }

}
