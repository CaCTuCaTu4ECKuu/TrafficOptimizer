using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrafficOptimizer.RoadMap.Model
{
    public class RoadMapParameters
    {
        /// <summary>
        /// Толщина бордюра
        /// </summary>
        public float BorderSize
        {
            get;
            set;
        }
        /// <summary>
        /// Размер полосы
        /// </summary>
        public float StreakSize
        {
            get;
            set;
        }
        /// <summary>
        /// Половина размера полосы
        /// </summary>
        public float StreakHalf
        {
            get
            {
                return StreakSize / 2;
            }
        }
        /// <summary>
        /// Размер машины (радиус круга)
        /// </summary>
        public float CarSize
        {
            get;
            set;
        }
        /// <summary>
        /// Расстояние просмотра
        /// </summary>
        public float ViewDistance
        {
            get;
            set;
        }

        public RoadMapParameters()
        {
            BorderSize = 0.1f;
            StreakSize = 1.2f;
            CarSize = 1f;
            ViewDistance = 25f;
        }
    }
}
