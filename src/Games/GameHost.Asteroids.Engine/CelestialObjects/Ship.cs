using GameHost.Shared.GameState.Vector;

namespace GameHost.Asteroids.Engine.CelestialObjects
{
    public class Ship
    {
        private Position _position;
        private Vector _velocity;
        private double _angle;

        //

        public Ship(Position position)
        {
        
            _position = position;
            _velocity = new Vector(0,0);
            _angle = 0;  
        
        }

        //

        public Position Position
        {
            get { return _position; }
            set { _position = value; }
        }

        public double Angle
        {
            get { return _angle; }
            set
            {
                _angle = value % 360;
                if (_angle < 0)
                {
                    _angle += 360;
                }
            }
        }

        public double AngleRad
        {
            get { return _angle * (Math.PI / 180.0); }
        }

        public Vector Velocity
        {
            get { return _velocity; }
            set { _velocity = value; }
        }

        //Receive a position

        // based on the position, we can convert it into a degree to face.
        //receive degree
        //call animate


        public void Animate(DateTime now, TimeSpan lapse)
        {
            double time = ((double)now.Ticks / (double)TimeSpan.TicksPerSecond);

            Vector friciton = new Vector(0.0025, _velocity.Direction + 180);

            _velocity = _velocity.AddVector(friciton);

            Position = Position.AddVector(_velocity);
        }
    }
}
