namespace GameHost.Asteroids.Engine.Interface
{
    public interface IGameControl
    {
        bool IsHoldingLeft();
        bool IsHoldingRight();
        bool IsThrusting();
    }
}