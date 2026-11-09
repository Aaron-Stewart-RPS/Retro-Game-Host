using GameHost.Shared.GameState.Vector;

namespace GameHost.Asteroids.Engine.CelestialObjects
{
    public class Star
    {
        private readonly Position _position;
        private readonly double _offset;
        private readonly double _speed;
        private readonly double _amplitude;
        //
        private double _brightness;

        //

        public Star(Position position, double offset, double speed, double amplitude)
        {
            _position = position;
            _offset = offset;
            _speed = speed;
            _amplitude = amplitude;
        }

        //

        public Position Position
        {
            get { return _position; }
        }

        public double Brightness
        {
            get { return _brightness; }
        }

        //

        public void Animate(DateTime now, TimeSpan lapse)
        {
            double time = ((double)now.Ticks / (double)TimeSpan.TicksPerSecond);
            _brightness = GetBrightness(time);
        }

        //

        private double GetBrightness(double time)
        {
            // Smooth sine wave from -1..1
            double wave = Math.Sin(_offset + (time * _speed));
            // Convert to 0..1
            double normalized = (wave + 1) / 2.0;
            // Select the range
            return _amplitude + (normalized * (1 - _amplitude));
        }
    }
}
