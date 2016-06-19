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

    public delegate void RoadLineChangedDelegate(Road road, Line line);

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

        public event RoadLineChangedDelegate OnStreakAdd;
        public event RoadLineChangedDelegate OnStreakRemove;

        private List<Streak> _streaks;
        public IEnumerable<Streak> Streaks
        {
            get { return _streaks.AsReadOnly(); }
        }
        public void AddStreak()
        {
            Streak newStreak = new Streak(this, null);
            if (_streaks.Count > 0)
            {
                Streak last = _streaks[_streaks.Count - 1];
                newStreak.AddDestination(newStreak);
                last.AddDestination(newStreak);
            }
            _streaks.Add(newStreak);
            if (OnStreakAdd != null)
                OnStreakAdd(Road, this);
        }
        public void RemoveStreak()
        {
            if (_streaks.Count > 0)
            {
                Streak last = _streaks[_streaks.Count - 1];
                if (_streaks.Count > 1)
                {
                    Streak preLast = _streaks[_streaks.Count - 2];
                    preLast.RemoveDestination(last);
                }
                _streaks.Remove(last);
                if (OnStreakRemove != null)
                    OnStreakRemove(Road, this);
            }
        }

        public Line(Road road, Edge edge)
        {
            Road = road;
            Edge = edge;
            _streaks = new List<Streak>();
        }
    }
}
