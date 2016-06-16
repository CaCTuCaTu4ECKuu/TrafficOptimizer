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

    [DebuggerDisplay("[{Road.ID}] {Source.ID} -> {Destination.ID}")]
    public class Line
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
        public bool IsPrimary
        {
            get
            {
                return Road.PrimaryLine == this;
            }
        }
        public Section Source
        {
            get
            {
                return IsPrimary ? Road.Source : Road.Destination;
            }
        }
        public Section Destination
        {
            get
            {
                return IsPrimary ? Road.Destination : Road.Source;
            }
        }
        public float Length
        {
            get { return Edge.Weight; }
        }

        public List<Streak> Streaks
        {
            get;
            private set;
        }
        public void AddStreak()
        {
            Streak newStreak = new Streak(this, null);
            if (Streaks.Count > 0)
            {
                Streak last = Streaks[Streaks.Count - 1];
                newStreak.AddDestination(newStreak);
                last.AddDestination(newStreak);
            }
            Streaks.Add(newStreak);
        }
        public void RemoveStreak()
        {
            if (Streaks.Count > 0)
            {
                Streak last = Streaks[Streaks.Count - 1];
                if (Streaks.Count > 1)
                {
                    Streak preLast = Streaks[Streaks.Count - 2];
                    preLast.RemoveDestination(last);
                }
            Streaks.Remove(last);
            }
        }

        public Line(Road road, Edge edge)
        {
            Road = road;
            Edge = edge;
            Streaks = new List<Streak>();
        }
    }
}
