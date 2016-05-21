using System;
using System.Collections.Generic;

namespace TrafficOptimizer.RoadMap.Model
{
    public abstract class VehicleContainer
    {
        private static uint _instances = 0;
        public uint ID
        {
            get;
            private set;
        }

        public List<Vehicle> Vehicles
        {
            get;
            private set;
        }
        /// <summary>
        /// Вставляет машину (без проверки на корректность)
        /// </summary>
        /// <param name="vehicle">Машина</param>
        public void SetVehicle(Vehicle vehicle, double position)
        {
            int i = Vehicles.Count;
            while (Vehicles[i - 1].PositionAt(this) > position)
            {
                i--;
            }
            vehicle.Enter(this, position);
            Vehicles.Insert(i, vehicle);
        }
        public void VehicleLeave(Vehicle vehicle)
        {
            vehicle.Leave(this);
        }

        public abstract bool CanFit(Vehicle vehicle, double position);
        public abstract bool IsOrdered
        {
            get;
        }
        public abstract bool IsOrderedCorrectly
        {
            get;
        }
        public abstract double InputSpace
        {
            get;
        }

        public VehicleContainer()
        {
            ID = _instances++;
            Vehicles = new List<Vehicle>();
        }

        public void ThrowIfNotOrderedCorrectly()
        {
            if (!IsOrderedCorrectly)
                throw new Exception("Vehicles not ordered correctly");
        }
    }
}
