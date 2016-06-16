using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace TrafficOptimizer.RoadMap.Model
{
    using Graph.Model;

    [DebuggerDisplay("[{ID}] {Source.ID}-{Destination.ID} | ({Streaks})")]
    public class Road
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
        public Section Source
        {
            get;
            private set;
        }
        public Section Destination
        {
            get;
            private set;
        }
        public Segment Segment
        {
            get { return new Segment(Source, Destination); }
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
                return PrimaryLine.Streaks.Count() + SlaveLine.Streaks.Count();
            }
        }

        public Road(RoadMap roadMap, Section source, Section destination, Edge primary, Edge slave)
        {
            ID = _instances++;

            RoadMap = roadMap;
            Source = source;
            Destination = destination;

            PrimaryLine = new Line(this, primary);
            SlaveLine = new Line(this, slave);
        }
    }
}
