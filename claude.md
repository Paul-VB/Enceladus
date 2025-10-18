# Claude Code Context - Enceladus

## Game Overview

### Core Concept
- **Genre**: 2D Submersible Action/Mining/Survival
- **Setting**: Deep subsurface ocean of Saturn's moon Enceladus (lawless resource exploitation)
- **Physics**: Low gravity, near-neutral buoyancy = unique "floaty flying" movement

### Key Gameplay Systems (Future Implementation)
- **Dual Mode Controls**:
  - Coarse Mode (traversal): High-speed movement, auto-levels for thrust
  - Fine Mode (precision): Slow slewing, fixed attitude for drilling/combat
- **Equipment Systems**:
  - Mark System (defines base power/efficiency)
  - Quality System (defines augment slots)
  - Augment Extraction (salvage augments from items)
- **Environment**:
  - Tunnel refreeze (time pressure)
  - Iceologic instability (drilling limits per sector)
  - Biomes with unique hostile life and loot
- **Economy**:
  - Hostile rival mining subs
  - Base trading (sell haul, repairs, refuel)
  - Orbital trader (rare high-tier items, time-limited)

## Current Architecture

### Entity System
- **All entities are ICollidable** (nullable Hitbox for non-collidables)
- **Component pattern**: Use components ONLY for interfaces with complex method logic
  - `MovableComponent` exists for `IMovable` (has Accelerate, ApplyTorque methods)
  - NO CollisionComponent needed - ICollidable is just properties
- **Hierarchy**: `Entity : ICollidable` → `MovableEntity : IMovable`

### Collision System
- **Coordinate systems**:
  - Entities: Position = CENTER (hitboxes centered at origin, transformed to world)
  - Cells: Position = TOP-LEFT (grid-aligned at integer coordinates)
  - `VertexExtractor.ExtractWorldVertices()` handles both via type check
- **Performance**: EntityRegistry maintains pre-filtered lists (MovableEntities, StaticEntities)
  - Type checks happen ONCE at registration, not per-frame
- **Collision flow**:
  - CollisionService → CollisionChecker (detection) → CollisionResolver (resolution)
  - CollisionService has IEntityRegistry injected
  - CollisionChecker receives IEntityRegistry as parameter (consider injecting for consistency)

## Important Files

### Core Architecture
- `MovableEntity.cs` - Delegates to MovableComponent
- `EntityRegistry.cs` - Pre-filtered entity lists for performance
- `IMovable.cs` - Interface with complex methods (requires component)
- `ICollidable.cs` - Interface with simple properties (no component needed)

### Collision System
- `VertexExtractor.cs` - **CRITICAL**: Handles cell vs entity coordinate systems
- `CollisionService.cs` - Orchestrates collision detection and resolution
- `CollisionChecker.cs` - Implements CheckPair pattern for entity-to-entity
- `CollisionResolver.cs` - Dispatches to ResolveEntityToStatic or ResolveEntityToMovable
- `SatCollisionDetector.cs` - SAT algorithm for polygon collision

### Testing
- `TestHelpers.cs` - EntityHelpers and MapHelpers factories
- `VertexExtractorTestFixture.cs` - Regression tests for cell coordinate bug

## Testing Patterns
- **Entity creation**: Always use `EntityHelpers.CreateTestEntity(position, hitbox, rotation)`
- **Map creation**: Use `MapHelpers.CreateMapWithCells(...)` for auto-chunk creation
- **Regression tests**: VertexExtractorTestFixture catches cell coordinate bugs

## Architecture Principles

### Todo tracking/roadmap
- Make sure that the todo.md file in the root of the repo is kept up to date so we cna track bugs and goals

### When to Use Components
**USE** components when:
- Interface has complex method logic (e.g., `IMovable.Accelerate()` with physics math)
- Logic would be duplicated across multiple entity types
- Avoids diamond problem in inheritance

**DON'T USE** components when:
- Interface only has simple properties (e.g., `ICollidable.Hitbox`)
- No shared logic to extract
- Would be needless abstraction

### Performance Patterns
1. **Move work from hot path to initialization**
   - Type checks at registration, not per-frame
   - Pre-filter collections instead of LINQ queries in loops
2. **Avoid allocations in tight loops**
   - No `.ToList()` in per-frame code
   - Reuse collections where possible

## Known Gotchas

### Cells vs Entities Coordinate Systems
- **CRITICAL**: Cells use grid coordinates (top-left), entities use centered coordinates
- `VertexExtractor.ExtractWorldVertices()` has type check to handle both correctly
- Cell at (5, 10) occupies (5,10) to (6,11), NOT centered at (5,10)
- If collision feels "off by 0.5 units", check coordinate system handling
- Regression test: `VertexExtractorTestFixture` will catch this bug

### EntityRegistry Performance
- **NEVER** use `entities.OfType<T>().ToList()` or similar LINQ in per-frame code
- **ALWAYS** use `EntityRegistry.MovableEntities` or `EntityRegistry.StaticEntities`
- Type filtering happens once at registration, not per-frame
- With 100 entities at 60fps, this saves ~6,000 type checks/second

### Component Pattern Usage
- **ONLY** create components for interfaces with complex method logic
- `IMovable` needs `MovableComponent` because `Accelerate()` has physics math
- `ICollidable` does NOT need component - it's just properties (this point might become outdated if IColldiable ever changes. PLEASE remove this example if icollidable ever gets its own component with complex logic)
- Don't over-engineer - simple properties don't need abstraction

### Collision Detection
- Only `MovableEntity` can be in collisions (as the primary entity)
- Static entities and cells can be the "other object" in collisions
- `CollisionResult.Entity` is always `MovableEntity`, `OtherObject` is `ICollidable`
- Collision normal points from OtherObject TO Entity (for proper resolution)

## Primary Developer Notes - Paul_VB
- Commit after major refactors work and tests pass
- Line-by-line review before committing
- Keep working code separate from experimental changes
- Keep comments in code to a miniumum. Comments can become out-of-sync from the code if the code changes. I prefer commends be reserved for explaining weird or unclear complex logic. The code should be clean enough that it is mostly self-documenting
- 
