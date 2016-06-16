using System.Drawing;

namespace TrafficOptimizer.RoadMap2D
{
    public class RoadMapParameters
    {
        /// <summary>
        /// Толщина бордюра
        /// </summary>
        public float DividingLineSize
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
        /// Расстояние просмотра
        /// </summary>
        public float ViewScale
        {
            get;
            set;
        }
        public float ViewScaleStep
        {
            get;
            set;
        }

        public RoadMapParameters()
        {
            DividingLineSize = 0.1f;
            StreakSize = 5.0f;

            ViewScale = 0.005f;
            ViewScaleStep = 0.5f;
        }
    }
}
