using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrafficOptimizer.RoadMap
{
    using Model;
    using RatioControls;

    public class RatioController
    {
        /// <summary>
        /// Карта дорог
        /// </summary>
        public RoadMap RoadMap
        {
            get;
            private set;
        }
        public DateTime SimulationTime;

        public RatioCollection MovementRatio
        {
            get;
            private set;
        }
        public List<IRatioCollection> RatioFactors
        {
            get;
            private set;
        }

        public bool UseMovementRatio
        {
            get;
            set;
        }

        #region Statistics
        /*
        /// <summary>
        /// Начал движение
        /// </summary>
        public event VehicleContainerDelegate Vehicle_BeginMovement;
        /// <summary>
        /// Вьехал на указанную сторону движения (пересек ее начало)
        /// </summary>
        public event VehicleContainerDelegate Vehicle_EnterLine;
        /// <summary>
        /// Остановился на указанной стороне движения (затормозил)
        /// </summary>
        public event VehicleContainerDelegate Vehicle_Stopped;
        /// <summary>
        /// Продолжил движение на указанной стороне движения (дал газу после остановки)
        /// </summary>
        public event VehicleContainerDelegate Vehicle_Started;
        /// <summary>
        /// Покинул указанную сторону движения (полностью пересек ее конец)
        /// </summary>
        public event VehicleContainerDelegate Vehicle_LeaveLine;
        /// <summary>
        /// Закончил движения
        /// </summary>
        public event VehicleContainerDelegate Vehicle_EndMovement;
        */
        #endregion

        public RatioController(RoadMap map, bool useMovementRatio)
        {
            RoadMap = map;
            SimulationTime = DateTime.Now.Date;
            RatioFactors = new List<IRatioCollection>();
            UseMovementRatio = useMovementRatio;

            //var mr = new MovementRatioCollection(map, 100);
            //MovementRatio = mr;
            //Vehicle_EnterLine += mr.Vehicle_OnEnterLine;
            //Vehicle_LeaveLine += mr.Vehicle_LeaveLine;
        }
    }
}
