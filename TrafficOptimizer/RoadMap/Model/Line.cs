﻿using System;
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
        protected List<Streak> _streaks = new List<Streak>();
        public int Streaks
        {
            get
            {
                return _streaks.Count;
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
    }
}
