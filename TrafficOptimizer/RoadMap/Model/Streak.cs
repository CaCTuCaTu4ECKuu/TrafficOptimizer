namespace TrafficOptimizer.RoadMap.Model
{
    public class Streak : VehicleContainer
    {
        private Line _line;

        public override bool IsOrdered
        {
            get
            {
                for (int i = 0; i < Vehicles.Count - 1; i++)
                {
                    if (Vehicles[i + 1].PositionAt(this) < Vehicles[i].PositionAt(this))
                        return false;
                }
                return true;
            }
        }
        public override bool IsOrderedCorrectly
        {
            get
            {
                for (int i = Vehicles.Count - 1; i > 0; i--)
                {
                    var space = Vehicles[i].PositionAt(this) - Vehicles[i - 1].PositionAt(this);
                    if (space < 0 || Vehicles[i].Length >= space)
                        return false;
                }
                return true;
            }
        }
        public override double InputSpace
        {
            get
            {
                ThrowIfNotOrderedCorrectly();

                if (Vehicles.Count > 0)
                    return Vehicles[0].PositionAt(this) > 0 ? Vehicles[0].PositionAt(this) : 0;
                else
                    return _line.Length;
            }
        }

        public override bool CanFit(Vehicle vehicle, double position)
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

        public Streak(Line line) : base()
        {
            _line = line;
        }
    }
}
