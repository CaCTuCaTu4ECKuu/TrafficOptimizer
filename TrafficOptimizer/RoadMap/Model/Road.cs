using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace TrafficOptimizer.RoadMap.Model
{
    using System.Collections;
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
        private float _lastWeight;
        public float Length
        {
            get { return PrimaryLine.Length; }
        }

        public Road(RoadMap roadMap, Section source, Section destination, Edge primaryEdge, Edge slaveEdge, float weight)
        {
            if (roadMap == null)
                throw new ArgumentException("Нужно указать дорожную карту");

            if (source.RoadMap != roadMap || destination.RoadMap != roadMap)
                throw new ArgumentException("Источник и назначение от различных карт");

            ID = _instances++;

            RoadMap = roadMap;
            Source = source;
            Destination = destination;

            _lastWeight = weight;

            PrimaryLine = new Line(this, primaryEdge);
            SlaveLine = new Line(this, slaveEdge);

            PrimaryLine.OnStreakRemove += Line_OnStreakRemove;
            SlaveLine.OnStreakRemove += Line_OnStreakRemove;
            PrimaryLine.OnStreakAdd += Line_OnStreakAdd;
            SlaveLine.OnStreakAdd += Line_OnStreakAdd;
        }
        ~Road()
        {
            PrimaryLine.OnStreakRemove -= Line_OnStreakRemove;
            SlaveLine.OnStreakRemove -= Line_OnStreakRemove;
            PrimaryLine.OnStreakAdd -= Line_OnStreakAdd;
            SlaveLine.OnStreakAdd -= Line_OnStreakAdd;
        }

        public void ChangeWeight(float newWeight)
        {
            _lastWeight = newWeight;
            if (PrimaryLine.Streaks.Count() > 0)
                RoadMap.Graph.ChangeWeight(PrimaryLine.Edge, newWeight);
            if (SlaveLine.Streaks.Count() > 0)
                RoadMap.Graph.ChangeWeight(SlaveLine.Edge, newWeight);
        }

        private void Line_OnStreakAdd(Road road, Line line)
        {
            if (line.Streaks.Count() == 1)
            {
                RoadMap.Graph.ChangeWeight(line.Edge, _lastWeight);
            }
        }
        private void Line_OnStreakRemove(Road road, Line line)
        {
            if (road.Streaks == 0)
            {
                RoadMap.RemoveRoad(RoadMap.GetSegment(road));
            }
            else if (line.Streaks.Count() == 0)
            {
                RoadMap.Graph.ChangeWeight(line.Edge, float.PositiveInfinity);
            }
        }
    }
}
