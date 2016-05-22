using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrafficOptimizer.RoadMap
{
    using Model;
    using Graph;
    using Graph.Model;

    public class Road
    {
        private static uint _instances = 0;
        public uint ID
        {
            get;
            private set;
        }

        public Line PrimaryLine
        {
            get;
            private set;
        }
        public Line SlaveLine
        {
            get;
            private set;
        }
        public int Streaks
        {
            get
            {
                return PrimaryLine.Streaks + SlaveLine.Streaks;
            }
        }

        public double Length
        {
            get
            {
                return PrimaryLine.Length;
            }
        }

        public Road(Edge primary, Edge slave)
        {
            ID = _instances++;
            PrimaryLine = new Line(primary, 1);
            SlaveLine = new Line(slave, 1);
        }
    }
}
