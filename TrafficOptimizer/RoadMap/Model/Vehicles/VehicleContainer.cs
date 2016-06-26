using System;
using System.Linq;
using System.Collections.Generic;

namespace TrafficOptimizer.RoadMap.Model.Vehicles
{
    using Tools;

    public delegate void VehicleMoveDelegate(VehicleContainer container, Vehicle vehicle);

    public abstract class VehicleContainer
    {
        private static uint _instances = 0;
        public uint ID
        {
            get;
            private set;
        }

        protected List<VehicleContainer> _destinations = new List<VehicleContainer>();
        protected List<Vehicle> _vehicles = new List<Vehicle>();
        /// <summary>
        /// Пути, к которым можно добратся из данного контейнера
        /// </summary>
        public abstract IEnumerable<VehicleContainer> Destinations
        {
            get;
        }
        public virtual IEnumerable<Vehicle> Vehicles
        {
            get { return _vehicles.AsReadOnly(); }
        }
        public abstract float Length
        {
            get;
        }
        /// <summary>
        /// Коэффициент, определяющий замедление скорости преодоления относительно прямолинейного движения
        /// </summary>
        public float MoveRatio
        {
            get;
            protected set;
        }

        /// <summary>
        /// Разрешен ли путь от этого контейнера к указанному
        /// </summary>
        /// <param name="dest">Место назначения</param>
        /// <returns></returns>
        public virtual bool AllowToMove(VehicleContainer src, VehicleContainer dst)
        {
            return (src == this || src.Destinations.Contains(this)) && this.Destinations.Contains(dst);
        }

        private void setContainer(IEnumerable<VehicleContainer> destinations, float moveRatio)
        {
            ID = _instances++;
            MoveRatio = moveRatio;

            if (destinations != null)
                _destinations.AddRange(destinations);

            OnVehicleEnter += VehicleContainer_OnVehicleEnter;
            OnVehicleLeave += VehicleContainer_OnVehicleLeave;

        }
        public VehicleContainer(IEnumerable<VehicleContainer> destinations, float moveRatio = 1.0f)
        {
            setContainer(destinations, moveRatio);
        }
        public VehicleContainer(VehicleContainer destination, float moveRatio = 1.0f)
        {
            setContainer(new VehicleContainer[] { destination }, moveRatio);
        }

        private void VehicleContainer_OnVehicleLeave(VehicleContainer container, Vehicle vehicle)
        {
            if (container == this)
            {
                _vehicles.Remove(vehicle);
            }
            else
                throw new ApplicationException("Ну чё бля за ху кто трогал код?");
        }

        private void VehicleContainer_OnVehicleEnter(VehicleContainer container, Vehicle vehicle)
        {
            if (container == this)
            {
                _vehicles.Add(vehicle);
            }
            else
                throw new ApplicationException("НЕТ БЛЯ, сюда ток машину, которая вьехала");
        }

        public virtual void AddDestination(VehicleContainer dst)
        {
            if (!_destinations.Contains(dst))
                _destinations.Add(dst);
        }
        public virtual void RemoveDestination(VehicleContainer dst)
        {
            _destinations.Remove(dst);
        }

        public event VehicleMoveDelegate OnVehicleEnter;
        public event VehicleMoveDelegate OnVehicleLeave;

        public bool IsSpaceFree(float front, float back)
        {
            foreach (Vehicle v in _vehicles)
            {
                if (v.Length > Length)
                    return false;
                float vFront = v.FrontPosition.Container == this ? v.FrontPosition.Position : Length + v.FrontPosition.Position;
                float vBack = vFront - v.Length;
                if (Tools.IsSegmentsOverlapping(front, back, vFront, vBack))
                    return false;
            }
            return true;
        }

        public void SetVehicle(Vehicle vehicle)
        {
            if (vehicle.FrontPosition == null)
            {
                vehicle.SetPosition(this, 0);
            }
            else
            {
                _vehicles.Add(vehicle);
            }
        }
    }
}
