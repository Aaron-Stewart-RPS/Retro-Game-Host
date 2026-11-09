using Avalonia.Input;
using GameHost.Asteroids.Engine.Interface;

namespace GameHost.Presentaion.GameRender
{
    internal class GameInput : IGameControl
    {
        private bool isRotatingLeft;
        private bool isRotatingRight;
        private bool isThrusting;

        //

        public void OnKeyDown(KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Left:
                    isRotatingLeft = true;
                    break;

                case Key.Right:
                    isRotatingRight = true;
                    break;

                case Key.Up:
                    isThrusting = true;
                    break;
            }
        }

        public void OnKeyUp(KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Left:
                    isRotatingLeft = false;
                    break;

                case Key.Right:
                    isRotatingRight = false;
                    break;

                case Key.Up:
                    isThrusting = false;
                    break;
            }
        }

        //

        public bool IsHoldingLeft()
        {
            return isRotatingLeft;
        }

        public bool IsHoldingRight()
        {
            return isRotatingRight;
        }

        public bool IsThrusting()
        {
            return isThrusting;
        }
    }
}
