# Retro-Game-Host — project plan

This document describes where the project is headed, what is in place today, and a practical order of work. It is meant to stay aligned with the repo; update it when milestones or structure change.

## Vision

1. **Host** — A desktop shell that lets the player pick a game, adjust shared settings (window, input, audio), and return to a menu without restarting the app.
2. **Games** — Each title is a small set of projects: **Engine** (pure simulation, testable) and **Rendering** (how that state is drawn), plus optional game-specific assets.
3. **Shared** — Cross-game math, input contracts, and rendering hooks so new retro games do not duplicate boilerplate.

Success looks like: adding a second game is mostly “new folder under `Games/` + register in host,” not rewriting the Avalonia app.

## Current snapshot (startup phase)

### Implemented

| Area | Detail |
|------|--------|
| Host shell | Single `MainWindow` filling the client area with `GameSurface`; `MainViewModel` is empty. |
| Loop | `DispatcherTimer` drives `GameState.Animate` and `InvalidateVisual`. |
| Asteroids engine | `GameState`, `Ship`, `Star`; `IGameControl` for input abstraction. |
| Shared math | `Position`, `Vector` (polar), wrap and compose helpers. |
| Input | `GameInput` implements `IGameControl`; arrow keys on the window. |

### Scaffold only (in solution, little or no code)

- `GameHost.Asteroids.Rendering`
- `GameHost.Shared.Rendering` (includes an empty `Contract/` folder)
- `Presentation/GameHost.Presentaion/GameEngine/` folder placeholder
- Automated tests (no test projects referenced by the solution yet)

### Intentional gaps in Asteroids

- No asteroid entities, fragmentation, or waves
- No projectiles or firing
- No collision detection or damage
- No score, lives, or game-over flow
- Starfield is decorative only (not gameplay asteroids)

### Architecture drift (to address deliberately)

| Topic | Today | Target |
|-------|--------|--------|
| Drawing | `GameSurface` in Presentation draws stars and ship | Asteroids drawing in **GameHost.Asteroids.Rendering**; host only hosts surfaces |
| Shared rendering | Empty project | Interfaces such as “render this frame from engine snapshot” in **Shared.Rendering** |
| Game discovery | Hard-coded `GameState` in `GameSurface` | Host loads a registered list of `IGameModule` (or similar) |
| Naming | `Presentaion` in assembly/namespace paths | Rename to `Presentation` when convenient (wide find/replace + slnx) |
| Time step | `DateTime` deltas; variable frame rate | Fixed simulation step with optional interpolation for display |
| RNG | `new Random()` per star in `GameState` | Single seeded `Random` or deterministic spawn tables |

## Target architecture

```text
┌─────────────────────────────────────────────────────────┐
│  GameHost.Presentation (Avalonia)                        │
│  · Main menu / game picker                               │
│  · Window, focus, global input routing                   │
│  · Hosts IGameSession / render control per selected game │
└──────────────────────────┬──────────────────────────────┘
                           │
         ┌─────────────────┼─────────────────┐
         ▼                 ▼                 ▼
  Shared.GameState   Shared.Rendering    (per game)
  · Vector/Position  · draw contracts    Engine + Rendering
```

**Dependency rules (goal):**

- **Engine** → Shared.GameState only (no Avalonia, no drawing).
- **Game.Rendering** → Engine + Shared.Rendering (+ Avalonia or skia draw helpers as needed).
- **Presentation** → Shared.* + game Rendering/registration; avoids referencing Engine internals except through facades.

## Roadmap

Phases are ordered so each step leaves the app runnable.

### Phase 0 — Foundations (current → stable prototype)

- [ ] Document controls and frame behavior in README when they change.
- [ ] Fix nullable warnings (e.g. `_ship` before `Initialize()`).
- [ ] Normalize namespaces in XAML (`GameHost.Presentation` vs `GameHost.Presentaion`).
- [ ] Choose a fixed tick rate (e.g. 60 Hz sim, display on Avalonia timer or vsync-friendly interval).

**Exit criteria:** Clean build with no nullable noise; consistent naming in new code.

### Phase 1 — Complete Asteroids core loop

Engine work in **GameHost.Asteroids.Engine**:

- [ ] `Asteroid` entity (size tiers, velocity, rotation).
- [ ] Spawning: initial field + wave progression.
- [ ] `Projectile` with lifetime and wrap.
- [ ] Collision: bullet–asteroid, ship–asteroid (with brief invulnerability after hit).
- [ ] Score, lives, respawn, and game-over state machine.
- [ ] `IGameControl` extension: fire, maybe hyperspace (optional classic feature).

**Exit criteria:** Playable single-player Asteroids entirely drivable through `IGameControl` and unit-testable without UI.

### Phase 2 — Extract rendering

- [ ] Define minimal types in **Shared.Rendering** (e.g. camera/scale from normalized world to pixels).
- [ ] Move star/ship/asteroid/bullet drawing from `GameSurface` into **GameHost.Asteroids.Rendering**.
- [ ] Slim `GameSurface` to: allocate renderer, pass bounds, forward input, call engine + renderer each tick.

**Exit criteria:** Presentation project has no Asteroids-specific draw logic; only hosts the control and input.

### Phase 3 — Host experience

- [ ] Implement `MainViewModel` and a simple menu: title, “Play Asteroids,” quit.
- [ ] Game session lifecycle: start, pause, exit to menu (dispose timers/state).
- [ ] Optional: fullscreen, resize handling (scale already uses `Bounds` in `ScalePos`).
- [ ] Register games via a small catalog (reflection or explicit list in one `GamesRegistry` class).

**Exit criteria:** User launches host, selects Asteroids, plays, returns to menu.

### Phase 4 — Quality and second game

- [ ] Add `tests/` projects: Shared.GameState math, Asteroids collisions and scoring.
- [ ] CI: `dotnet build` + `dotnet test` on push (GitHub Actions or similar).
- [ ] Spike a second mini-game (e.g. Snake or Breakout) to validate the “new game” path.
- [ ] Refine Shared contracts based on what two games actually share.

**Exit criteria:** Second game loads from menu; core shared code is proven by two consumers.

### Phase 5 — Polish (ongoing)

- Sound effects and mute in settings.
- High-score persistence (local file or lightweight store).
- Theming / retro CRT-style shader (optional).
- Controller or remappable keys.

## Asteroids design notes (for implementation)

| Topic | Proposal |
|-------|----------|
| World space | Keep 0–100 normalized coords; map to pixels in rendering layer only. |
| Ship | Current thrust/friction model is a good base; tune max speed and turn rate per playtest. |
| Stars | Keep as background layer; do not mix with collision geometry. |
| Asteroids | Large / medium / small with split rules; random velocities on spawn and split. |
| Difficulty | Increase asteroid count or speed per wave; cap entities for performance. |

## Adding a new game (future checklist)

1. Create `Games/GameHost.<Name>.Engine` and `Games/GameHost.<Name>.Rendering`.
2. Reference **Shared.GameState** (and **Shared.Rendering** once defined).
3. Expose a small facade: create state, tick with input interface, expose read-only snapshot for renderer.
4. Register in host catalog; add menu entry and thumbnail/description.
5. Add engine tests before wiring UI.

## Risks and decisions to revisit

- **Avalonia package versions** — Presentation mixes Avalonia 12.x with ReactiveUI/Diagnostics on 11.x; consolidate when touching packages.
- **ReactiveUI** — Referenced but unused; either adopt for MVVM in Phase 3 or remove to reduce surface area.
- **Single-window vs multiple scenes** — Plan assumes one main window and swapped content; acceptable for v1.

## References in repo

| Concept | Location |
|---------|----------|
| Simulation tick | `GameHost.Asteroids.Engine/GameState.cs` |
| Input contract | `GameHost.Asteroids.Engine/Interface/IGameControl.cs` |
| Host draw loop | `Presentation/GameHost.Presentaion/GameRender/GameSurface.cs` |
| Desktop entry | `Presentation/GameHost.Presentaion.Desktop/Program.cs` |

---

*Last aligned with codebase: startup phase — Asteroids prototype with starfield and ship movement only.*
