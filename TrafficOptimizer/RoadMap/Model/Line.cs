using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrafficOptimizer.RoadMap.Model
{
    using Graph.Model;

    public partial class Line
    {
        public Road Road
        {
            get;
            private set;
        }
        public Edge Edge
        {
            get;
            private set;
        }
        public TrafficLight TrafficLight
        {
            get;
            set;
        }

        public float Length
        {
            get
            {
                return Edge.Weight;
            }
        }
        public List<Streak> Streaks
        {
            get;
            private set;
        }
    }
}
