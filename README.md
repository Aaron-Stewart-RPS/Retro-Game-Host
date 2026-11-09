# Retro-Game-Host

A desktop application that hosts a collection of retro-style games behind a single shell. The codebase is organized so each game keeps its own simulation logic while sharing common math and (eventually) rendering contracts across titles.

**Status:** Early startup. The solution builds and runs; only **Asteroids** exists, and it is a playable prototype (starfield, ship movement) rather than a full game or a finished host experience.

## What works today

- Avalonia desktop window with a custom `GameSurface` render loop (~10 FPS tick; stars animate continuously).
- **Asteroids** engine: 500 twinkling stars, a ship with rotation and thrust, friction, and screen wrapping in normalized 0–100 world space.
- Keyboard input: **← / →** rotate, **↑** thrust (forwarded from `MainWindow` to `GameInput`).

Not implemented yet: game launcher/menu, scoring, asteroids, bullets, collisions, lives, sound, or separation of Asteroids drawing into its dedicated rendering project.

## Tech stack

| Area | Choice |
|------|--------|
| Runtime | .NET 10 (`net10.0`) |
| UI | [Avalonia](https://avaloniaui.net/) 12 (Fluent theme, ReactiveUI referenced) |
| Solution | Visual Studio / `dotnet` (.slnx) |

## Repository layout

```
Retro-Game-Host/
├── docs/                          Project plans and design notes
├── src/
│   ├── GameHost.Presentaion.slnx  Solution entry point
│   ├── Presentation/
│   │   ├── GameHost.Presentaion.Desktop/   WinExe host (Program.cs)
│   │   └── GameHost.Presentaion/             Avalonia app, views, GameSurface
│   ├── Games/
│   │   ├── GameHost.Asteroids.Engine/        Simulation (GameState, Ship, Star)
│   │   └── GameHost.Asteroids.Rendering/     Placeholder (not wired yet)
│   └── Shared/
│       ├── GameHost.Shared.GameState/        Vector, Position
│       └── GameHost.Shared.Rendering/        Placeholder for shared draw contracts
└── tests/                         (reserved — no test projects in solution yet)
```

### Project roles

| Project | Role |
|---------|------|
| **GameHost.Presentaion.Desktop** | Process entry; starts Avalonia. |
| **GameHost.Presentaion** | Shell UI, input bridge, and current Asteroids drawing (`GameSurface`). |
| **GameHost.Asteroids.Engine** | Game rules and entity update loop; depends only on **Shared.GameState**. |
| **GameHost.Asteroids.Rendering** | Intended home for Asteroids-specific drawing (empty today). |
| **GameHost.Shared.GameState** | Reusable 2D position/vector math (polar vectors, wrap, add). |
| **GameHost.Shared.Rendering** | Intended shared rendering abstractions (empty today). |

Cross-cutting note: simulation and presentation are split at the project level, but **drawing still lives in Presentation** until rendering projects are implemented. See [docs/Plan.md](docs/Plan.md) for the intended end state.

## Build and run

**Prerequisites:** [.NET SDK](https://dotnet.microsoft.com/download) compatible with `net10.0` (e.g. .NET 10).

```powershell
cd src
dotnet build GameHost.Presentaion.slnx
dotnet run --project Presentation/GameHost.Presentaion.Desktop/GameHost.Presentaion.Desktop.csproj
```

Open `src/GameHost.Presentaion.slnx` in Visual Studio 2022+ for designer and debugging.

## Documentation

- **[docs/Plan.md](docs/Plan.md)** — roadmap, architecture goals, Asteroids milestones, and known cleanup items.

## License

MIT — see [LICENSE](LICENSE).
