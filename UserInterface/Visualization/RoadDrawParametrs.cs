using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserInterface.Visualization
{
    public class RoadDrawParametrs
    {
        public float BorderSize
        {
            get;
            set;
        }
        public float StreakSize
        {
            get;
            set;
        }
        public float CarSize
        {
            get;
            set;
        }
        public float ViewDistance
        {
            get;
            set;
        }
        public RoadDrawParametrs()
        {
            BorderSize = 0.1f;
            StreakSize = 1.2f;
            CarSize = 1f;
            ViewDistance = 25f;
        }
    }
}
