using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Diagnostics;

namespace TrafficOptimizer.RoadMap.Model
{
    using Graph.Model;
    using TrafficOptimizer.RoadMap;

    [DebuggerDisplay("[{PrimaryLine.Edge.Source.ID}]-[{PrimaryLine.Edge.Destination.ID}] {StartPoint} - {EndPoint}")]
    public partial class Road
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

        public Road(RoadMap rmp, Edge primary, Edge slave, PointF start, PointF end)
        {
            ID = _instances++;
            RoadMap = rmp;
            PrimaryLine = new Line(this, primary, 1, start, end);
            SlaveLine = new Line(this, slave, 1, end, start);

            Move(start, end);
        }

        public void Move(PointF newStart, PointF newEnd)
        {
            StartPoint = newStart;
            EndPoint = newEnd;

            PrimaryLine.Move(newStart, newEnd);
            SlaveLine.Move(newEnd, newStart);
        }
    }
}
