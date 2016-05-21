using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrafficOptimizer.RoadMap
{
    public class Section
    {
        public Point Position
        {
            get;
            private set;
        }

        public Section(Point position)
        {
            Position = position;
        }
    }
}
