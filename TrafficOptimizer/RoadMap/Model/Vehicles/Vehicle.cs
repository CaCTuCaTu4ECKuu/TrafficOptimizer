using System;
using System.Collections.Generic;

namespace TrafficOptimizer.RoadMap.Model.Vehicles
{
    public abstract class Vehicle
    {
        private static uint _instances;
        public uint ID
        {
            get;
            private set;
        }

        public Section Destination
        {
            get;
            set;
        }
        
        public float Length
        {
            get;
            private set;
        }
        public VehiclePosition FrontPosition
        {
            get;
            private set;
        }
        public VehiclePosition BackPosition
        {
            get;
            private set;
        }
        public float MaxSpeed
        {
            get;
            private set;
        }
        public float MaxAcceleration
        {
            get;
            private set;
        }

        public void Move(VehicleContainer container, float newPosition)
        {
            VehiclePosition pos = new VehiclePosition(container, newPosition);
            if (FrontPosition == null)
            {
                // Проверять нет ли машины в указанном месте
                if (true)
                {
                    FrontPosition = new VehiclePosition(container, newPosition);
                }
            }
            else
            {
                if (FrontPosition != pos)
                {
                    BackPosition = FrontPosition;
                    FrontPosition = pos;
                    // TODO: еще там какие-то оповещения должны быть
                }
            }
        }

        public Vehicle(Section destination, float length, float maxSpeed, float maxAcceleration)
        {
            ID = _instances++;

            Destination = destination;

            Length = length;
            MaxSpeed = maxSpeed;
            MaxAcceleration = maxAcceleration;

            FrontPosition = null;
            BackPosition = null;
        }
    }
}
