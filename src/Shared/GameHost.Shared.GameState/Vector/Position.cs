namespace GameHost.Shared.GameState.Vector
{
    public class Position 
    {
        private readonly double _x;
        private readonly double _y;

        //

        // Runs when we create a position
        public Position(double X, double Y)
        {
            _x = X;
            _y = Y;
        }

        //

        // Gets X
        public double X
        {
            get { return _x; }
        }

        // Gets Y
        public double Y
        {
            get { return _y; }
        }

        //

        public Position Wrap(double max_X, double max_Y)
        {
            return Wrap(0, max_X, 0, max_Y);
        }

        public Position Wrap(double min_X, double max_X, double min_Y, double max_Y)
        {
            if(_x >= min_X && _x <= max_X &&
                _y >= min_Y && _y <= max_Y)
            {
                return this;
            }

            double newX = _x;
            double newY = _y;

            if (newX > max_X)
            {
                newX = min_X;
            }

            if (newX < min_X)
            {
                newX = max_X;
            }

            if (newY > max_Y)
            {
                newY = min_Y;
            }

            if (newY < min_Y)
            {
                newY = max_Y;
            }

            return new Position(newX, newY);
        }

        public Position AddVector(Vector Vector)
        {
            double angle = Vector.DirectionRad - (Math.PI / 2);
            double vX = Vector.Amount * Math.Cos(angle);
            double vY = Vector.Amount * Math.Sin(angle);

            double newX = _x + vX;
            double newY = _y + vY;

            return new Position (newX, newY);
        }
    }
}
