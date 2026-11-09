namespace GameHost.Shared.GameState.Vector
{

    public class Vector
    {
        private readonly double _amount;
        private readonly double _direction;

        //

        public Vector(double amount, double direction)
        {
            _amount = amount;
            if (_amount < 0)
            {
                _amount = 0;
            }
            //
            _direction = direction % 360;
            if (_direction < 0)
            {
                _direction += 360;
            }
        }

        //

        public double Amount
        {
            get { return _amount; }
        }

        public double Direction
        {
            get { return _direction; }
        }

        public double DirectionRad
        {
            get { return _direction * (Math.PI / 180.0); }
        }

        //

        public Vector Clamp(double maxAmount)
        {
            if (_amount <= maxAmount)
            {
                return this;
            }
            return new Vector(maxAmount, _direction);
        }

        //

        public Vector AddVector(Vector other)
        {
            // Convert THIS (old vector) to components
            double aRad = this.DirectionRad - (Math.PI / 2);
            double ax = this.Amount * Math.Cos(aRad);
            double ay = this.Amount * Math.Sin(aRad);

            // Convert OTHER (new vector) to components
            double bRad = other.DirectionRad - (Math.PI / 2);
            double bx = other.Amount * Math.Cos(bRad);
            double by = other.Amount * Math.Sin(bRad);

            // Add components
            double sx = ax + bx;
            double sy = ay + by;

            // Convert back to polar
            double newAmount = Math.Sqrt(sx * sx + sy * sy);
            double newDirection = Math.Atan2(sy, sx) * (180.0 / Math.PI);

            // Return a brand new vector
            return new Vector(newAmount, newDirection + 90);
        }
    }
}
