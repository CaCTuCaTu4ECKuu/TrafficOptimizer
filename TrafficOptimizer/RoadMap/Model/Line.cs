using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrafficOptimizer.RoadMap.Model
{
    using Graph.Model;

    public class Line
    {
        public Edge Edge
        {
            get;
            private set;
        }
        public double Length
        {
            get
            {
                return Edge.Weight;
            }
        }
        private List<Streak> _streaks = new List<Streak>();
        public int Streaks
        {
            get
            {
                return _streaks.Count;
            }
            set
            {
                if (value < _streaks.Count)
                {
                    for (int i = _streaks.Count - 1; i >= value; i--)
                    {
                        _streaks.RemoveAt(i);
                    }
                }
                else
                {
                    int i = value - _streaks.Count;
                    while (i-- > 0)
                    {
                        _streaks.Insert(_streaks.Count, new Streak(this));
                    }
                }
            }
        }

        public bool CanFit(Vehicle vehicle, int streak, double position)
        {
            return _streaks[streak].CanFit(vehicle, position);
        }
        public void SetVehicle(Vehicle vehicle, int streak, double position)
        {
            if (CanFit(vehicle, streak, position))
                _streaks[streak].SetVehicle(vehicle, position);
        } 

        public bool IsOrderedCorrectly
        {
            get
            {
                foreach (var s in _streaks)
                {
                    if (!s.IsOrderedCorrectly)
                        return false;
                }
                return true;
            }
        }
        public double InputSpaceAt(int streak)
        {
            return _streaks[streak].InputSpace;
        }

        public Line(Edge edge, int streaks)
        {
            Edge = edge;
            Streaks = streaks;
        }
    }
}
