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

        public double Length
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

        public bool CanFit(Vehicle vehicle, int streak, double position)
        {
            return Streaks[streak].CanFit(vehicle, position);
        }
        public void SetVehicle(Vehicle vehicle, int streak, double position)
        {
            if (CanFit(vehicle, streak, position))
                Streaks[streak].SetVehicle(vehicle, position);
        } 

        public bool IsOrderedCorrectly
        {
            get
            {
                foreach (var s in Streaks)
                {
                    if (!s.IsOrderedCorrectly)
                        return false;
                }
                return true;
            }
        }
        public double InputSpaceAt(int streak)
        {
            return Streaks[streak].InputSpace;
        }
    }
}
