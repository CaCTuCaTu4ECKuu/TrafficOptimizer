using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrafficOptimizer.RoadMap.Model
{
    public partial class Road
    {
        private static uint _instances = 0;
        public uint ID
        {
            get;
            private set;
        }

        public RoadMap RoadMap
        {
            get;
            private set;
        }

        /// <summary>
        /// Полоса основного движения
        /// </summary>
        public Line PrimaryLine
        {
            get;
            private set;
        }
        /// <summary>
        /// Полоса обратного движения
        /// </summary>
        public Line SlaveLine
        {
            get;
            private set;
        }
        /// <summary>
        /// Общее количество полос движения
        /// </summary>
        public int Streaks
        {
            get
            {
                return PrimaryLine.Streaks + SlaveLine.Streaks;
            }
        }

        /// <summary>
        /// Длинна отрезка
        /// </summary>
        public double Length
        {
            get
            {
                return PrimaryLine.Length;
            }
        }
    }
}
