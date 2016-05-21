using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrafficOptimizer.RoadMap
{
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

        public Edge PrimaryEdge
        {
            get;
            private set;
        }
        public Edge SlaveEdge
        {
            get;
            private set;
        }

        public double Length
        {
            get
            {
                return PrimaryEdge.Weight;
            }
        }

        public Road(Edge primary, Edge slave)
        {
            ID = _instances++;
            PrimaryEdge = primary;
            SlaveEdge = slave;
        }
    }
}
