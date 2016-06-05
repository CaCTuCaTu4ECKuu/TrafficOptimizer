using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrafficOptimizer.RoadMap
{
    using Model;
    using Graph.Model;

    public class VehicleController
    {
        public RoadMap RoadMap
        {
            get;
            private set;
        }
        public Dictionary<Vehicle, VehicleContainer> Positions
        {
            get;
            private set;
        }
        public Dictionary<Vehicle, Path> Pathes
        {
            get;
            private set;
        }

        public VehicleController(RoadMap map)
        {
            RoadMap = map;
        }



        public void AddVehicle(Vehicle vehicle, VehicleContainer container, float position)
        {
            Positions.Add(vehicle, container);
        }
        /*
        public override bool CanFit(Vehicle vehicle, float position)
        {
            ThrowIfNotOrderedCorrectly();

            if (Vehicles.Count > 0)
            {
                int i = 0;
                if (Vehicles.Count > 1)
                {
                    i = 1;
                    // Находим машину перед нами (или сзади, если перед нет)
                    while (i < Vehicles.Count - 1)
                    {
                        if (Vehicles[i].PositionAt(this) >= position)
                            break;
                        i++;
                    }
                    // Сзади всегда будет как минимум 1 машина
                }

                if (position > Vehicles[i].PositionAt(this))
                {
                    // Перед нами нет машин (i - машина сзади)
                    // Проверяем что нам не упрутся в жопу
                    return position - vehicle.Length > Vehicles[i].PositionAt(this);
                }
                else
                {
                    // Перед нами есть машина
                    // Проверяем что мы не упремся в жопу
                    bool forwardFit = Vehicles[i].PositionAt(this) - Vehicles[i].Length > position;
                    if (i == 0)
                    {
                        // Больше машин нет - проверено
                        return forwardFit;
                    }
                    else
                    {
                        // Проверяем чтобы машина сзади не упералась нам в жопу
                        return forwardFit && position - vehicle.Length > Vehicles[i - 1].PositionAt(this);
                    }
                }
            }
            return true;
        }
        */
    }
}
