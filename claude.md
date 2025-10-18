# Claude Code Context - Enceladus

## Current Architecture (Last Updated: 2025-01-18)

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

### Coordinate System Rules
- **Entities**: Always centered (Position = center point, hitboxes centered at origin)
- **Cells**: Always top-left grid-aligned (Position = (X, Y) integer coordinates)
- **Never mix**: VertexExtractor handles the translation between systems

## Primary Developer Notes - Paul_VB
- Commit after major refactors work and tests pass
- Line-by-line review before committing
- Keep working code separate from experimental changes
- Keep comments in code to a miniumum. Comments can become out-of-sync from the code if the code changes. I prefer commends be reserved for explaining weird or unclear complex logic. The code should be clean enough that it is mostly self-documenting
