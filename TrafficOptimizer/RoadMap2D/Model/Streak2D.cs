using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Diagnostics;

namespace TrafficOptimizer.RoadMap2D.Model
{
    using Tools;

    [DebuggerDisplay("{MiddleStart} - {MiddleEnd}")]
    public class Streak2D
    {
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
        /// Сдвигает расположение полосы в координатной плоскости
        /// </summary>
        /// <param name="newStart">Точка начало нового центра полосы</param>
        /// <param name="newEnd">Точка окончания нового центра полосы</param>
        public Streak2D(PointF newStart, PointF newEnd, float streakHalfWidth)
        {
            MiddleStart = newStart;
            MiddleEnd = newEnd;

            // Right Border
            BorderLeftStart = Tools.HeightPoint(streakHalfWidth, true, MiddleStart, MiddleEnd);
            BorderLeftEnd = Tools.HeightPoint(streakHalfWidth, false, MiddleEnd, MiddleStart);
            // Left Border
            BorderRightStart = Tools.HeightPoint(streakHalfWidth, false, MiddleStart, MiddleEnd);
            BorderRightEnd = Tools.HeightPoint(streakHalfWidth, true, MiddleEnd, MiddleStart);
        }
    }

}
