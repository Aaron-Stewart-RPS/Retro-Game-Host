using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Threading;
using GameHost.Asteroids.Engine;
using GameHost.Asteroids.Engine.CelestialObjects;
using GameHost.Shared.GameState.Vector;

namespace GameHost.Presentaion.GameRender;

public sealed class GameSurface : Control
{
    private readonly DispatcherTimer _renderTimer;
    private GameState                _gameState;
    private readonly GameInput       _keyState = new GameInput();

    //

    public GameSurface()
    {
        double framerate = 1000 / 100;

        _gameState = new GameState();

        // Render loop (60 FPS)
        _renderTimer = new DispatcherTimer
        {
            Interval = TimeSpan.FromMilliseconds(framerate)
        };
        _renderTimer.Tick += _renderTimer_Tick;
        _renderTimer.Start();
    }

    internal GameInput KeyState
    {
        get { return _keyState; }
    }

    public override void Render(DrawingContext context)
    {
        base.Render(context);

        context.FillRectangle(Brushes.Black, new Rect(Bounds.Size));

        foreach (Star star in _gameState.Stars)
        {
            DrawStar(context, star);
        }

        DrawShip(context, _gameState.ship );
    }

    //

    private void _renderTimer_Tick(object? sender, EventArgs e)
    {
        _gameState.Animate(_keyState);
        InvalidateVisual();
    }

    //

    private void DrawStar(DrawingContext context, Star star)
    {
        Position scaled = ScalePos(star.Position);
        double Rad = 2;
        Brush brush = new SolidColorBrush(Colors.White, star.Brightness);

        context.DrawEllipse(brush, null, new Rect(scaled.X - Rad, scaled.Y - Rad, Rad * 2, Rad * 2));
    }

    private void DrawShip(DrawingContext context, Ship ship)
    {
        Position scaled = ScalePos(ship.Position);
        Geometry shipShape = PathGeometry.Parse("M 0 -20  L 12 10  L 0 5  L -12 10 Z");

        using (context.PushTransform(Matrix.CreateRotation(ship.AngleRad) * Matrix.CreateTranslation(scaled.X, scaled.Y)))
        {
            context.DrawGeometry(
                Brushes.DarkViolet,
                new Pen(Brushes.White, 2),
                shipShape
            );
        }
    }

    //

    private Position ScalePos(Position position)
    {
        double ScaledX = (position.X / 100) * Bounds.Width;
        double ScaledY = (position.Y / 100) * Bounds.Height;
        return new Position(ScaledX, ScaledY);
    }
}