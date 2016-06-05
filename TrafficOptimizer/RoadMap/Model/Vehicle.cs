using System.Collections.Generic;

namespace TrafficOptimizer.RoadMap.Model
{
    using Map;
    public abstract class Vehicle
    {
        private static uint _instances;
        public uint ID
        {
            get;
            private set;
        }

        /// <summary>
        /// Длинна транспортного средства
        /// </summary>
        public float Length
        {
            get;
            private set;
        }
        /// <summary>
        /// Ширина транспортного средства
        /// </summary>
        public float Width
        {
            get;
            private set;
        }
        /// <summary>
        /// Высота транспортного средства
        /// </summary>
        public float Height
        {
            get;
            private set;
        }

        public EndPoint Destination
        {
            get;
            set;
        }

        /// <summary>
        /// Текущая скорость движения транспортного средства
        /// </summary>
        public float Speed
        {
            get;
            set;
        }
        /// <summary>
        /// Текущее ускорение транспортного средства
        /// </summary>
        public float Acceleration
        {
            get;
            set;
        }

        public Vehicle(float length, float width, float height, EndPoint destination)
        {
            ID = _instances++;

            Length = length;
            Width = width;
            Height = height;

            Destination = destination;

            Speed = 0;
            Acceleration = 0;
        }
    }
}
