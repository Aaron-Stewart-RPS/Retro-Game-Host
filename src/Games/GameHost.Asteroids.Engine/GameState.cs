using GameHost.Asteroids.Engine.CelestialObjects;
using GameHost.Asteroids.Engine.Interface;
using GameHost.Shared.GameState.Vector;

namespace GameHost.Asteroids.Engine
{
    public class GameState
    {
        private Ship _ship;
        private List<Star> _stars = new List<Star>();
        private DateTime _lastupdate;

        //

        public GameState()
        {
            Initialize();
        }

        //

        public List<Star> Stars
        {
            get { return _stars; }
        }

        public Ship ship { get { return _ship; } }

        //

        internal void Initialize()
        {
            _stars.Clear();
            //
            for (int i = 0; i < 500; i++)
            {
                Position pos = CreateStarPosition();
                (double offset, double speed, double amplitude) = CreateStarQualities();
                _stars.Add(new Star(pos, offset, speed, amplitude));
            }
            //
            _ship = new Ship(new Position(50,50));
            //
            _lastupdate = DateTime.Now;
        }

        public void Animate(IGameControl gameControl)
        {
            DateTime now = DateTime.Now;
            TimeSpan lapse = now - _lastupdate;

            foreach (Star star in _stars)
            {
                star.Animate(now, lapse);
            }

            if (gameControl.IsHoldingLeft())
            {
                ship.Angle -= 2.5;
            }

            if (gameControl.IsHoldingRight())
            {
                ship.Angle += 2.5;
            }

            if (gameControl.IsThrusting())
            {
                Vector thrust       = new Vector(0.01, ship.Angle);
                Vector shipVelocity = _ship.Velocity;

                shipVelocity = shipVelocity.AddVector(thrust).Clamp(1.5);

                _ship.Velocity = shipVelocity;
            }

            _ship.Animate(now, lapse);
            _ship.Position = _ship.Position.Wrap(0, 100, 0, 100);

            _lastupdate = now;
        }

        //

        private (double offset, double speed, double amplitude) CreateStarQualities() 
        {
            Random rand = new Random();

            double offset = rand.NextDouble() * 24;
            double speed = rand.NextDouble() * 2.5;
            double amp = rand.NextDouble() * 0.5;

            return  (offset, speed, amp);
        }
        
        private Position CreateStarPosition()
        {
            Random rand = new Random();
            //
            double x = rand.NextDouble() * 100;
            double y = rand.NextDouble() * 100;
            //
            return new Position(x, y);
        }
    }
}
