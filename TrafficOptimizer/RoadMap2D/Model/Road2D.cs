using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Diagnostics;

namespace TrafficOptimizer.RoadMap2D.Model
{
    using RoadMap.Model;

    [DebuggerDisplay("{StartPoint} - {EndPoint} | {Streaks}")]
    public class Road2D
    {
        /// <summary>
        /// Начало центра дороги
        /// </summary>
        public PointF StartPoint
        {
            get;
            private set;
        }
        /// <summary>
        /// Конец центра дороги
        /// </summary>
        public PointF EndPoint
        {
            get;
            private set;
        }

        public Line2D PrimaryLine
        {
            get;
            private set;
        }
        public Line2D SlaveLine
        {
            get;
            private set;
        }

        public int Streaks
        {
            get
            {
                return PrimaryLine.Streaks.Count + SlaveLine.Streaks.Count;
            }
        }

        private Road2D(PointF start, PointF end, int primStreaks, int slaveStreaks, float streakWidth)
        {
            StartPoint = start;
            EndPoint = end;

            PrimaryLine = new Line2D(start, end, primStreaks, streakWidth);
            SlaveLine = new Line2D(end, start, slaveStreaks, streakWidth);
        }

        public static Road2D GetRoad(Road road, PointF start, PointF end, float streakWidth)
        {
            if (road != null)
                return new Road2D(start, end, road.PrimaryLine.Streaks.Count(), road.SlaveLine.Streaks.Count(), streakWidth);
            return null;
        }
    }
}
