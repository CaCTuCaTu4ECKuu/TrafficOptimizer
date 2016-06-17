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

        public float Length
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

        public Section Destination
        {
            get;
            private set;
        }
        public Route Route
        {
            get;
            private set;
        }
        public VehicleContainer DestinationAfter(VehicleContainer container)
        {
            if (Route != null)
                return Route.DestinationAfter(container);
            return null;
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

        public Vehicle(float length, float maxSpeed, float maxAcceleration)
        {
            ID = _instances++;

            Length = length;
            MaxSpeed = maxSpeed;
            MaxAcceleration = maxAcceleration;

            Destination = null;
            Route = null;
            FrontPosition = null;
            BackPosition = null;
        }

        public void SetDestination(Section destination, RoadMap map)
        {
            if (FrontPosition != null)
            {
                Destination = destination;
                //TODO: Build route
            }
            else
                throw new ApplicationException("Не установлено положение машины на дороге");
        }
        public void SetPosition(VehicleContainer container, float position)
        {
            if (container.IsSpaceFree(Route, 0, -Length))
            {
                FrontPosition = new VehiclePosition(container, 0);
                BackPosition = new VehiclePosition(container, -Length);
            }
            else
                throw new ApplicationException("Нельзя разместить машину в указанном месте. Недостаточно пространства");
        }
    }
}
